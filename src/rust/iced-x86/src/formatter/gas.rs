// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

pub(super) mod enums;
mod fmt_data;
mod fmt_tbl;
mod info;
mod mem_size_tbl;
mod regs;
#[cfg(test)]
mod tests;

use crate::formatter::fmt_consts::*;
use crate::formatter::fmt_utils::*;
use crate::formatter::fmt_utils_all::*;
use crate::formatter::gas::enums::*;
use crate::formatter::gas::fmt_tbl::ALL_INFOS;
use crate::formatter::gas::info::*;
use crate::formatter::gas::mem_size_tbl::MEM_SIZE_TBL;
use crate::formatter::gas::regs::*;
use crate::formatter::instruction_internal::get_address_size_in_bytes;
use crate::formatter::num_fmt::*;
use crate::formatter::regs_tbl_ls::REGS_TBL;
use crate::formatter::*;
use crate::iced_constants::IcedConstants;
use crate::iced_error::IcedError;
use crate::instruction_internal;
use crate::*;
use alloc::boxed::Box;
use alloc::vec::Vec;
use core::mem;

/// GNU assembler (AT&T) formatter
///
/// # Examples
///
/// ```
/// use iced_x86::*;
///
/// let bytes = b"\x62\xF2\x4F\xDD\x72\x50\x01";
/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
/// let instr = decoder.decode();
///
/// let mut output = String::new();
/// let mut formatter = GasFormatter::new();
/// formatter.options_mut().set_uppercase_mnemonics(true);
/// formatter.format(&instr, &mut output);
/// assert_eq!("VCVTNE2PS2BF16 4(%rax){1to16},%zmm6,%zmm2{%k5}{z}", output);
/// ```
///
/// # Using a symbol resolver
///
/// ```
/// use iced_x86::*;
/// use std::collections::HashMap;
///
/// let bytes = b"\x48\x8B\x8A\xA5\x5A\xA5\x5A";
/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
/// let instr = decoder.decode();
///
/// struct MySymbolResolver { map: HashMap<u64, String> }
/// impl SymbolResolver for MySymbolResolver {
///     fn symbol(&mut self, _instruction: &Instruction, _operand: u32, _instruction_operand: Option<u32>,
///          address: u64, _address_size: u32) -> Option<SymbolResult> {
///         if let Some(symbol_string) = self.map.get(&address) {
///             // The 'address' arg is the address of the symbol and doesn't have to be identical
///             // to the 'address' arg passed to symbol(). If it's different from the input
///             // address, the formatter will add +N or -N, eg. '[rax+symbol+123]'
///             Some(SymbolResult::with_str(address, symbol_string.as_str()))
///         } else {
///             None
///         }
///     }
/// }
///
/// // Hard code the symbols, it's just an example!😄
/// let mut sym_map: HashMap<u64, String> = HashMap::new();
/// sym_map.insert(0x5AA55AA5, String::from("my_data"));
///
/// let mut output = String::new();
/// let resolver = Box::new(MySymbolResolver { map: sym_map });
/// let mut formatter = GasFormatter::with_options(Some(resolver), None);
/// formatter.format(&instr, &mut output);
/// assert_eq!("mov my_data(%rdx),%rcx", output);
/// ```
#[allow(missing_debug_implementations)]
pub struct GasFormatter {
	d: SelfData,
	number_formatter: NumberFormatter,
	symbol_resolver: Option<Box<dyn SymbolResolver>>,
	options_provider: Option<Box<dyn FormatterOptionsProvider>>,
}

impl Default for GasFormatter {
	#[inline]
	fn default() -> Self {
		GasFormatter::new()
	}
}

// Read-only data which is needed a couple of times due to borrow checker
struct SelfData {
	options: FormatterOptions,
	all_registers: &'static [FormatterString; IcedConstants::REGISTER_ENUM_COUNT],
	all_registers_naked: &'static [FormatterString; IcedConstants::REGISTER_ENUM_COUNT],
	instr_infos: &'static [Box<dyn InstrInfo + Send + Sync>; IcedConstants::CODE_ENUM_COUNT],
	all_memory_sizes: &'static [&'static FormatterString; IcedConstants::MEMORY_SIZE_ENUM_COUNT],
	str_: &'static FormatterConstants,
	vec_: &'static FormatterArrayConstants,
}

impl GasFormatter {
	const IMMEDIATE_VALUE_PREFIX: &'static str = "$";

	/// Creates a gas (AT&T) formatter
	#[must_use]
	#[inline]
	pub fn new() -> Self {
		GasFormatter::with_options(None, None)
	}

	/// Creates a gas (AT&T) formatter
	///
	/// # Arguments
	///
	/// - `symbol_resolver`: Symbol resolver or `None`
	/// - `options_provider`: Operand options provider or `None`
	#[must_use]
	#[allow(clippy::missing_inline_in_public_items)]
	pub fn with_options(symbol_resolver: Option<Box<dyn SymbolResolver>>, options_provider: Option<Box<dyn FormatterOptionsProvider>>) -> Self {
		Self {
			d: SelfData {
				options: FormatterOptions::with_gas(),
				all_registers: &ALL_REGISTERS,
				all_registers_naked: &REGS_TBL,
				instr_infos: &ALL_INFOS,
				all_memory_sizes: &MEM_SIZE_TBL,
				str_: &FORMATTER_CONSTANTS,
				vec_: &ARRAY_CONSTS,
			},
			number_formatter: NumberFormatter::new(),
			symbol_resolver,
			options_provider,
		}
	}

	const fn all_registers(d: &SelfData) -> &'static [FormatterString; IcedConstants::REGISTER_ENUM_COUNT] {
		if d.options.gas_naked_registers() {
			d.all_registers_naked
		} else {
			d.all_registers
		}
	}

	fn format_mnemonic(
		&mut self, instruction: &Instruction, output: &mut dyn FormatterOutput, op_info: &InstrOpInfo<'_>, column: &mut u32, mnemonic_options: u32,
	) {
		let mut need_space = false;
		if (mnemonic_options & FormatMnemonicOptions::NO_PREFIXES) == 0 && (op_info.flags & InstrOpInfoFlags::MNEMONIC_IS_DIRECTIVE as u16) == 0 {
			let prefix_seg = instruction.segment_prefix();

			const PREFIX_FLAGS: u32 = (InstrOpInfoFlags::SIZE_OVERRIDE_MASK << InstrOpInfoFlags::OP_SIZE_SHIFT)
				| (InstrOpInfoFlags::SIZE_OVERRIDE_MASK << InstrOpInfoFlags::ADDR_SIZE_SHIFT)
				| InstrOpInfoFlags::BND_PREFIX;
			if ((prefix_seg as u32)
				| instruction_internal::internal_has_any_of_lock_rep_repne_prefix(instruction)
				| ((op_info.flags as u32) & PREFIX_FLAGS))
				!= 0
			{
				let mut prefix;

				if (op_info.flags & InstrOpInfoFlags::OP_SIZE_IS_BYTE_DIRECTIVE as u16) != 0 {
					// SAFETY: generated data is valid
					let size_override: SizeOverride = unsafe {
						mem::transmute((((op_info.flags as u32) >> InstrOpInfoFlags::OP_SIZE_SHIFT) & InstrOpInfoFlags::SIZE_OVERRIDE_MASK) as u8)
					};
					match size_override {
						SizeOverride::None => {}

						SizeOverride::Size16 | SizeOverride::Size32 => {
							output.write(
								self.d.str_.dot_byte.get(self.d.options.uppercase_keywords() || self.d.options.uppercase_all()),
								FormatterTextKind::Directive,
							);
							output.write(" ", FormatterTextKind::Text);
							let number_options = NumberFormattingOptions::with_immediate(&self.d.options);
							let s = self.number_formatter.format_u8(&self.d.options, &number_options, 0x66);
							output.write(s, FormatterTextKind::Number);
							output.write(";", FormatterTextKind::Punctuation);
							output.write(" ", FormatterTextKind::Text);
							*column += (self.d.str_.dot_byte.len() + 1 + s.len() + 1 + 1) as u32;
						}

						SizeOverride::Size64 => GasFormatter::format_prefix(
							&self.d.options,
							output,
							instruction,
							column,
							&self.d.str_.rex_w,
							PrefixKind::OperandSize,
							&mut need_space,
						),
					}
				} else {
					prefix = &self.d.vec_.gas_op_size_strings
						[((op_info.flags as usize) >> InstrOpInfoFlags::OP_SIZE_SHIFT) & InstrOpInfoFlags::SIZE_OVERRIDE_MASK as usize];
					if !prefix.is_default() {
						GasFormatter::format_prefix(&self.d.options, output, instruction, column, prefix, PrefixKind::OperandSize, &mut need_space);
					}
				}

				prefix = &self.d.vec_.gas_addr_size_strings
					[((op_info.flags as usize) >> InstrOpInfoFlags::ADDR_SIZE_SHIFT) & InstrOpInfoFlags::SIZE_OVERRIDE_MASK as usize];
				if !prefix.is_default() {
					GasFormatter::format_prefix(&self.d.options, output, instruction, column, prefix, PrefixKind::AddressSize, &mut need_space);
				}

				let has_notrack_prefix = prefix_seg == Register::DS && is_notrack_prefix_branch(instruction.code());
				if !has_notrack_prefix && prefix_seg != Register::None && self.show_segment_prefix(instruction, op_info) {
					GasFormatter::format_prefix(
						&self.d.options,
						output,
						instruction,
						column,
						&self.d.all_registers_naked[prefix_seg as usize],
						get_segment_register_prefix_kind(prefix_seg),
						&mut need_space,
					);
				}

				if instruction.has_xacquire_prefix() {
					GasFormatter::format_prefix(
						&self.d.options,
						output,
						instruction,
						column,
						&self.d.str_.xacquire,
						PrefixKind::Xacquire,
						&mut need_space,
					);
				}
				if instruction.has_xrelease_prefix() {
					GasFormatter::format_prefix(
						&self.d.options,
						output,
						instruction,
						column,
						&self.d.str_.xrelease,
						PrefixKind::Xrelease,
						&mut need_space,
					);
				}
				if instruction.has_lock_prefix() {
					GasFormatter::format_prefix(&self.d.options, output, instruction, column, &self.d.str_.lock, PrefixKind::Lock, &mut need_space);
				}

				if has_notrack_prefix {
					GasFormatter::format_prefix(
						&self.d.options,
						output,
						instruction,
						column,
						&self.d.str_.notrack,
						PrefixKind::Notrack,
						&mut need_space,
					);
				}
				let has_bnd = (op_info.flags & InstrOpInfoFlags::BND_PREFIX as u16) != 0;
				if has_bnd {
					GasFormatter::format_prefix(&self.d.options, output, instruction, column, &self.d.str_.bnd, PrefixKind::Bnd, &mut need_space);
				}

				if instruction.has_repe_prefix() && show_rep_or_repe_prefix(instruction.code(), &self.d.options) {
					if is_repe_or_repne_instruction(instruction.code()) {
						GasFormatter::format_prefix(
							&self.d.options,
							output,
							instruction,
							column,
							get_mnemonic_cc(&self.d.options, 4, &self.d.str_.repe),
							PrefixKind::Repe,
							&mut need_space,
						);
					} else {
						GasFormatter::format_prefix(&self.d.options, output, instruction, column, &self.d.str_.rep, PrefixKind::Rep, &mut need_space);
					}
				}
				if !has_bnd && instruction.has_repne_prefix() && show_repne_prefix(instruction.code(), &self.d.options) {
					GasFormatter::format_prefix(
						&self.d.options,
						output,
						instruction,
						column,
						get_mnemonic_cc(&self.d.options, 5, &self.d.str_.repne),
						PrefixKind::Repne,
						&mut need_space,
					);
				}
			}
		}

		if (mnemonic_options & FormatMnemonicOptions::NO_MNEMONIC) == 0 {
			if need_space {
				output.write(" ", FormatterTextKind::Text);
				*column += 1;
			}
			let mnemonic = op_info.mnemonic;
			if (op_info.flags & InstrOpInfoFlags::MNEMONIC_IS_DIRECTIVE as u16) != 0 {
				output.write(mnemonic.get(self.d.options.uppercase_keywords() || self.d.options.uppercase_all()), FormatterTextKind::Directive);
			} else {
				output.write_mnemonic(instruction, mnemonic.get(self.d.options.uppercase_mnemonics() || self.d.options.uppercase_all()));
			}
			*column += mnemonic.len() as u32;
		}
		if (mnemonic_options & FormatMnemonicOptions::NO_PREFIXES) == 0 {
			if (op_info.flags & InstrOpInfoFlags::JCC_NOT_TAKEN as u16) != 0 {
				GasFormatter::format_branch_hint(&self.d.options, output, column, &self.d.str_.pn);
			} else if (op_info.flags & InstrOpInfoFlags::JCC_TAKEN as u16) != 0 {
				GasFormatter::format_branch_hint(&self.d.options, output, column, &self.d.str_.pt);
			}
		}
	}

	fn format_branch_hint(options: &FormatterOptions, output: &mut dyn FormatterOutput, column: &mut u32, br_hint: &FormatterString) {
		output.write(",", FormatterTextKind::Text);
		output.write(br_hint.get(options.uppercase_prefixes() || options.uppercase_all()), FormatterTextKind::Keyword);
		*column += 1 + br_hint.len() as u32;
	}

	fn show_segment_prefix(&self, instruction: &Instruction, op_info: &InstrOpInfo<'_>) -> bool {
		if (op_info.flags & (InstrOpInfoFlags::JCC_NOT_TAKEN | InstrOpInfoFlags::JCC_TAKEN) as u16) != 0 {
			return false;
		}

		match instruction.code() {
			Code::Monitorw
			| Code::Monitord
			| Code::Monitorq
			| Code::Monitorxw
			| Code::Monitorxd
			| Code::Monitorxq
			| Code::Clzerow
			| Code::Clzerod
			| Code::Clzeroq
			| Code::Umonitor_r16
			| Code::Umonitor_r32
			| Code::Umonitor_r64
			| Code::Maskmovq_rDI_mm_mm
			| Code::Maskmovdqu_rDI_xmm_xmm => return show_segment_prefix(Register::DS, instruction, &self.d.options),
			#[cfg(not(feature = "no_vex"))]
			Code::VEX_Vmaskmovdqu_rDI_xmm_xmm => return show_segment_prefix(Register::DS, instruction, &self.d.options),

			_ => {}
		}

		for i in 0..op_info.op_count as u32 {
			match op_info.op_kind(i) {
				InstrOpKind::Register
				| InstrOpKind::NearBranch16
				| InstrOpKind::NearBranch32
				| InstrOpKind::NearBranch64
				| InstrOpKind::FarBranch16
				| InstrOpKind::FarBranch32
				| InstrOpKind::Immediate8
				| InstrOpKind::Immediate8_2nd
				| InstrOpKind::Immediate16
				| InstrOpKind::Immediate32
				| InstrOpKind::Immediate64
				| InstrOpKind::Immediate8to16
				| InstrOpKind::Immediate8to32
				| InstrOpKind::Immediate8to64
				| InstrOpKind::Immediate32to64
				| InstrOpKind::MemoryESDI
				| InstrOpKind::MemoryESEDI
				| InstrOpKind::MemoryESRDI
				| InstrOpKind::Sae
				| InstrOpKind::RnSae
				| InstrOpKind::RdSae
				| InstrOpKind::RuSae
				| InstrOpKind::RzSae
				| InstrOpKind::Rn
				| InstrOpKind::Rd
				| InstrOpKind::Ru
				| InstrOpKind::Rz
				| InstrOpKind::DeclareByte
				| InstrOpKind::DeclareWord
				| InstrOpKind::DeclareDword
				| InstrOpKind::DeclareQword => {}

				InstrOpKind::MemorySegSI
				| InstrOpKind::MemorySegESI
				| InstrOpKind::MemorySegRSI
				| InstrOpKind::MemorySegDI
				| InstrOpKind::MemorySegEDI
				| InstrOpKind::MemorySegRDI
				| InstrOpKind::Memory => return false,
			}
		}
		self.d.options.show_useless_prefixes()
	}

	fn format_prefix(
		options: &FormatterOptions, output: &mut dyn FormatterOutput, instruction: &Instruction, column: &mut u32, prefix: &FormatterString,
		prefix_kind: PrefixKind, need_space: &mut bool,
	) {
		if *need_space {
			*column += 1;
			output.write(" ", FormatterTextKind::Text);
		}
		output.write_prefix(instruction, prefix.get(options.uppercase_prefixes() || options.uppercase_all()), prefix_kind);
		*column += prefix.len() as u32;
		*need_space = true;
	}

	fn format_operands(&mut self, instruction: &Instruction, output: &mut dyn FormatterOutput, op_info: &InstrOpInfo<'_>) {
		for i in 0..op_info.op_count as u32 {
			if i > 0 {
				output.write(",", FormatterTextKind::Punctuation);
				if self.d.options.space_after_operand_separator() {
					output.write(" ", FormatterTextKind::Text);
				}
			}
			self.format_operand(instruction, output, op_info, i);
		}
	}

	fn format_operand(&mut self, instruction: &Instruction, output: &mut dyn FormatterOutput, op_info: &InstrOpInfo<'_>, operand: u32) {
		debug_assert!(operand < op_info.op_count as u32);

		#[cfg(feature = "mvex")]
		let mvex_rm_operand = {
			if IcedConstants::is_mvex(instruction.code()) {
				let op_count = instruction.op_count();
				debug_assert_ne!(op_count, 0);
				(instruction.op_kind(op_count.wrapping_sub(1)) == OpKind::Immediate8 && op_info.op_count as u32 == op_count) as u32
			} else {
				u32::MAX
			}
		};

		let instruction_operand = op_info.instruction_index(operand);

		if (op_info.flags & InstrOpInfoFlags::INDIRECT_OPERAND as u16) != 0 {
			output.write("*", FormatterTextKind::Operator);
		}

		let flow_control;
		let mut imm8;
		let mut imm16;
		let mut imm32;
		let mut imm64;
		let value64;
		let imm_size;
		let mut operand_options;
		let number_kind;
		let op_kind = op_info.op_kind(operand);
		match op_kind {
			InstrOpKind::Register => {
				GasFormatter::format_register_internal(&self.d, output, instruction, operand, instruction_operand, op_info.op_register(operand))
			}

			InstrOpKind::NearBranch16 | InstrOpKind::NearBranch32 | InstrOpKind::NearBranch64 => {
				if op_kind == InstrOpKind::NearBranch64 {
					imm_size = 8;
					imm64 = instruction.near_branch64();
					number_kind = NumberKind::UInt64;
				} else if op_kind == InstrOpKind::NearBranch32 {
					imm_size = 4;
					imm64 = instruction.near_branch32() as u64;
					number_kind = NumberKind::UInt32;
				} else {
					imm_size = 2;
					imm64 = instruction.near_branch16() as u64;
					number_kind = NumberKind::UInt16;
				}
				let mut number_options = NumberFormattingOptions::with_branch(&self.d.options);
				operand_options = FormatterOperandOptions::default();
				if let Some(ref mut options_provider) = self.options_provider {
					options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
				}
				if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
					symbol_resolver.symbol(instruction, operand, instruction_operand, imm64, imm_size)
				} else {
					None
				} {
					FormatterOutputMethods::write1(
						output,
						instruction,
						operand,
						instruction_operand,
						&self.d.options,
						&mut self.number_formatter,
						&number_options,
						imm64,
						symbol,
						self.d.options.show_symbol_address(),
					);
				} else {
					flow_control = get_flow_control(instruction);
					let s = if op_kind == InstrOpKind::NearBranch32 {
						self.number_formatter.format_u32_zeros(
							&self.d.options,
							&number_options,
							instruction.near_branch32(),
							number_options.leading_zeros,
						)
					} else if op_kind == InstrOpKind::NearBranch64 {
						self.number_formatter.format_u64_zeros(
							&self.d.options,
							&number_options,
							instruction.near_branch64(),
							number_options.leading_zeros,
						)
					} else {
						self.number_formatter.format_u16_zeros(
							&self.d.options,
							&number_options,
							instruction.near_branch16(),
							number_options.leading_zeros,
						)
					};
					output.write_number(
						instruction,
						operand,
						instruction_operand,
						s,
						imm64,
						number_kind,
						if is_call(flow_control) { FormatterTextKind::FunctionAddress } else { FormatterTextKind::LabelAddress },
					);
				}
			}

			InstrOpKind::FarBranch16 | InstrOpKind::FarBranch32 => {
				if op_kind == InstrOpKind::FarBranch32 {
					imm_size = 4;
					imm64 = instruction.far_branch32() as u64;
					number_kind = NumberKind::UInt32;
				} else {
					imm_size = 2;
					imm64 = instruction.far_branch16() as u64;
					number_kind = NumberKind::UInt16;
				}
				let mut number_options = NumberFormattingOptions::with_branch(&self.d.options);
				operand_options = FormatterOperandOptions::default();
				if let Some(ref mut options_provider) = self.options_provider {
					options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
				}
				let mut vec: Vec<SymResTextPart<'_>> = Vec::new();
				if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
					to_owned(symbol_resolver.symbol(instruction, operand, instruction_operand, imm64 as u32 as u64, imm_size), &mut vec)
				} else {
					None
				} {
					output.write(GasFormatter::IMMEDIATE_VALUE_PREFIX, FormatterTextKind::Operator);
					debug_assert!(operand + 1 == 1);
					let selector_symbol = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
						symbol_resolver.symbol(instruction, operand + 1, instruction_operand, instruction.far_branch_selector() as u64, 2)
					} else {
						None
					};
					if let Some(ref selector_symbol) = selector_symbol {
						FormatterOutputMethods::write1(
							output,
							instruction,
							operand,
							instruction_operand,
							&self.d.options,
							&mut self.number_formatter,
							&number_options,
							instruction.far_branch_selector() as u64,
							selector_symbol,
							self.d.options.show_symbol_address(),
						);
					} else {
						let s = self.number_formatter.format_u16_zeros(
							&self.d.options,
							&number_options,
							instruction.far_branch_selector(),
							number_options.leading_zeros,
						);
						output.write_number(
							instruction,
							operand,
							instruction_operand,
							s,
							instruction.far_branch_selector() as u64,
							NumberKind::UInt16,
							FormatterTextKind::SelectorValue,
						);
					}
					output.write(",", FormatterTextKind::Punctuation);
					if self.d.options.space_after_operand_separator() {
						output.write(" ", FormatterTextKind::Text);
					}
					output.write(GasFormatter::IMMEDIATE_VALUE_PREFIX, FormatterTextKind::Operator);
					FormatterOutputMethods::write1(
						output,
						instruction,
						operand,
						instruction_operand,
						&self.d.options,
						&mut self.number_formatter,
						&number_options,
						imm64,
						symbol,
						self.d.options.show_symbol_address(),
					);
				} else {
					flow_control = get_flow_control(instruction);
					let s = self.number_formatter.format_u16_zeros(
						&self.d.options,
						&number_options,
						instruction.far_branch_selector(),
						number_options.leading_zeros,
					);
					output.write(GasFormatter::IMMEDIATE_VALUE_PREFIX, FormatterTextKind::Operator);
					output.write_number(
						instruction,
						operand,
						instruction_operand,
						s,
						instruction.far_branch_selector() as u64,
						NumberKind::UInt16,
						FormatterTextKind::SelectorValue,
					);
					output.write(",", FormatterTextKind::Punctuation);
					if self.d.options.space_after_operand_separator() {
						output.write(" ", FormatterTextKind::Text);
					}
					let s = if op_kind == InstrOpKind::FarBranch32 {
						self.number_formatter.format_u32_zeros(
							&self.d.options,
							&number_options,
							instruction.far_branch32(),
							number_options.leading_zeros,
						)
					} else {
						self.number_formatter.format_u16_zeros(
							&self.d.options,
							&number_options,
							instruction.far_branch16(),
							number_options.leading_zeros,
						)
					};
					output.write(GasFormatter::IMMEDIATE_VALUE_PREFIX, FormatterTextKind::Operator);
					output.write_number(
						instruction,
						operand,
						instruction_operand,
						s,
						imm64,
						number_kind,
						if is_call(flow_control) { FormatterTextKind::FunctionAddress } else { FormatterTextKind::LabelAddress },
					);
				}
			}

			InstrOpKind::Immediate8 | InstrOpKind::Immediate8_2nd | InstrOpKind::DeclareByte => {
				if op_kind != InstrOpKind::DeclareByte {
					output.write(GasFormatter::IMMEDIATE_VALUE_PREFIX, FormatterTextKind::Operator);
				}
				if op_kind == InstrOpKind::Immediate8 {
					imm8 = instruction.immediate8();
				} else if op_kind == InstrOpKind::Immediate8_2nd {
					imm8 = instruction.immediate8_2nd();
				} else {
					imm8 = instruction.get_declare_byte_value(operand as usize);
				}
				let mut number_options = NumberFormattingOptions::with_immediate(&self.d.options);
				operand_options = FormatterOperandOptions::default();
				if let Some(ref mut options_provider) = self.options_provider {
					options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
				}
				if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
					symbol_resolver.symbol(instruction, operand, instruction_operand, imm8 as u64, 1)
				} else {
					None
				} {
					FormatterOutputMethods::write1(
						output,
						instruction,
						operand,
						instruction_operand,
						&self.d.options,
						&mut self.number_formatter,
						&number_options,
						imm8 as u64,
						symbol,
						self.d.options.show_symbol_address(),
					);
				} else {
					if number_options.signed_number {
						imm64 = imm8 as i8 as u64;
						number_kind = NumberKind::Int8;
						if (imm8 as i8) < 0 {
							output.write("-", FormatterTextKind::Operator);
							imm8 = (imm8 as i8).wrapping_neg() as u8;
						}
					} else {
						imm64 = imm8 as u64;
						number_kind = NumberKind::UInt8;
					}
					let s = self.number_formatter.format_u8(&self.d.options, &number_options, imm8);
					output.write_number(instruction, operand, instruction_operand, s, imm64, number_kind, FormatterTextKind::Number);
				}
			}

			InstrOpKind::Immediate16 | InstrOpKind::Immediate8to16 | InstrOpKind::DeclareWord => {
				if op_kind != InstrOpKind::DeclareWord {
					output.write(GasFormatter::IMMEDIATE_VALUE_PREFIX, FormatterTextKind::Operator);
				}
				if op_kind == InstrOpKind::Immediate16 {
					imm16 = instruction.immediate16();
				} else if op_kind == InstrOpKind::Immediate8to16 {
					imm16 = instruction.immediate8to16() as u16;
				} else {
					imm16 = instruction.get_declare_word_value(operand as usize);
				}
				let mut number_options = NumberFormattingOptions::with_immediate(&self.d.options);
				operand_options = FormatterOperandOptions::default();
				if let Some(ref mut options_provider) = self.options_provider {
					options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
				}
				if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
					symbol_resolver.symbol(instruction, operand, instruction_operand, imm16 as u64, 2)
				} else {
					None
				} {
					FormatterOutputMethods::write1(
						output,
						instruction,
						operand,
						instruction_operand,
						&self.d.options,
						&mut self.number_formatter,
						&number_options,
						imm16 as u64,
						symbol,
						self.d.options.show_symbol_address(),
					);
				} else {
					if number_options.signed_number {
						imm64 = imm16 as i16 as u64;
						number_kind = NumberKind::Int16;
						if (imm16 as i16) < 0 {
							output.write("-", FormatterTextKind::Operator);
							imm16 = (imm16 as i16).wrapping_neg() as u16;
						}
					} else {
						imm64 = imm16 as u64;
						number_kind = NumberKind::UInt16;
					}
					let s = self.number_formatter.format_u16(&self.d.options, &number_options, imm16);
					output.write_number(instruction, operand, instruction_operand, s, imm64, number_kind, FormatterTextKind::Number);
				}
			}

			InstrOpKind::Immediate32 | InstrOpKind::Immediate8to32 | InstrOpKind::DeclareDword => {
				if op_kind != InstrOpKind::DeclareDword {
					output.write(GasFormatter::IMMEDIATE_VALUE_PREFIX, FormatterTextKind::Operator);
				}
				if op_kind == InstrOpKind::Immediate32 {
					imm32 = instruction.immediate32();
				} else if op_kind == InstrOpKind::Immediate8to32 {
					imm32 = instruction.immediate8to32() as u32;
				} else {
					imm32 = instruction.get_declare_dword_value(operand as usize);
				}
				let mut number_options = NumberFormattingOptions::with_immediate(&self.d.options);
				operand_options = FormatterOperandOptions::default();
				if let Some(ref mut options_provider) = self.options_provider {
					options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
				}
				if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
					symbol_resolver.symbol(instruction, operand, instruction_operand, imm32 as u64, 4)
				} else {
					None
				} {
					FormatterOutputMethods::write1(
						output,
						instruction,
						operand,
						instruction_operand,
						&self.d.options,
						&mut self.number_formatter,
						&number_options,
						imm32 as u64,
						symbol,
						self.d.options.show_symbol_address(),
					);
				} else {
					if number_options.signed_number {
						imm64 = imm32 as i32 as u64;
						number_kind = NumberKind::Int32;
						if (imm32 as i32) < 0 {
							output.write("-", FormatterTextKind::Operator);
							imm32 = (imm32 as i32).wrapping_neg() as u32;
						}
					} else {
						imm64 = imm32 as u64;
						number_kind = NumberKind::UInt32;
					}
					let s = self.number_formatter.format_u32(&self.d.options, &number_options, imm32);
					output.write_number(instruction, operand, instruction_operand, s, imm64, number_kind, FormatterTextKind::Number);
				}
			}

			InstrOpKind::Immediate64 | InstrOpKind::Immediate8to64 | InstrOpKind::Immediate32to64 | InstrOpKind::DeclareQword => {
				if op_kind != InstrOpKind::DeclareQword {
					output.write(GasFormatter::IMMEDIATE_VALUE_PREFIX, FormatterTextKind::Operator);
				}
				if op_kind == InstrOpKind::Immediate32to64 {
					imm64 = instruction.immediate32to64() as u64;
				} else if op_kind == InstrOpKind::Immediate8to64 {
					imm64 = instruction.immediate8to64() as u64;
				} else if op_kind == InstrOpKind::Immediate64 {
					imm64 = instruction.immediate64();
				} else {
					imm64 = instruction.get_declare_qword_value(operand as usize);
				}
				let mut number_options = NumberFormattingOptions::with_immediate(&self.d.options);
				operand_options = FormatterOperandOptions::default();
				if let Some(ref mut options_provider) = self.options_provider {
					options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
				}
				if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
					symbol_resolver.symbol(instruction, operand, instruction_operand, imm64, 8)
				} else {
					None
				} {
					FormatterOutputMethods::write1(
						output,
						instruction,
						operand,
						instruction_operand,
						&self.d.options,
						&mut self.number_formatter,
						&number_options,
						imm64,
						symbol,
						self.d.options.show_symbol_address(),
					);
				} else {
					value64 = imm64;
					if number_options.signed_number {
						number_kind = NumberKind::Int64;
						if (imm64 as i64) < 0 {
							output.write("-", FormatterTextKind::Operator);
							imm64 = (imm64 as i64).wrapping_neg() as u64;
						}
					} else {
						number_kind = NumberKind::UInt64;
					}
					let s = self.number_formatter.format_u64(&self.d.options, &number_options, imm64);
					output.write_number(instruction, operand, instruction_operand, s, value64, number_kind, FormatterTextKind::Number);
				}
			}

			InstrOpKind::MemorySegSI => self.format_memory(
				output,
				instruction,
				operand,
				instruction_operand,
				instruction.memory_segment(),
				Register::SI,
				Register::None,
				0,
				0,
				0,
				2,
			),
			InstrOpKind::MemorySegESI => self.format_memory(
				output,
				instruction,
				operand,
				instruction_operand,
				instruction.memory_segment(),
				Register::ESI,
				Register::None,
				0,
				0,
				0,
				4,
			),
			InstrOpKind::MemorySegRSI => self.format_memory(
				output,
				instruction,
				operand,
				instruction_operand,
				instruction.memory_segment(),
				Register::RSI,
				Register::None,
				0,
				0,
				0,
				8,
			),
			InstrOpKind::MemorySegDI => self.format_memory(
				output,
				instruction,
				operand,
				instruction_operand,
				instruction.memory_segment(),
				Register::DI,
				Register::None,
				0,
				0,
				0,
				2,
			),
			InstrOpKind::MemorySegEDI => self.format_memory(
				output,
				instruction,
				operand,
				instruction_operand,
				instruction.memory_segment(),
				Register::EDI,
				Register::None,
				0,
				0,
				0,
				4,
			),
			InstrOpKind::MemorySegRDI => self.format_memory(
				output,
				instruction,
				operand,
				instruction_operand,
				instruction.memory_segment(),
				Register::RDI,
				Register::None,
				0,
				0,
				0,
				8,
			),
			InstrOpKind::MemoryESDI => {
				self.format_memory(output, instruction, operand, instruction_operand, Register::ES, Register::DI, Register::None, 0, 0, 0, 2)
			}
			InstrOpKind::MemoryESEDI => {
				self.format_memory(output, instruction, operand, instruction_operand, Register::ES, Register::EDI, Register::None, 0, 0, 0, 4)
			}
			InstrOpKind::MemoryESRDI => {
				self.format_memory(output, instruction, operand, instruction_operand, Register::ES, Register::RDI, Register::None, 0, 0, 0, 8)
			}

			InstrOpKind::Memory => {
				let displ_size = instruction.memory_displ_size();
				let base_reg = instruction.memory_base();
				let mut index_reg = instruction.memory_index();
				let addr_size = get_address_size_in_bytes(base_reg, index_reg, displ_size, instruction.code_size());
				let displ = if addr_size == 8 { instruction.memory_displacement64() as i64 } else { instruction.memory_displacement32() as i64 };
				if (op_info.flags & InstrOpInfoFlags::IGNORE_INDEX_REG as u16) != 0 {
					index_reg = Register::None;
				}
				self.format_memory(
					output,
					instruction,
					operand,
					instruction_operand,
					instruction.memory_segment(),
					base_reg,
					index_reg,
					instruction_internal::internal_get_memory_index_scale(instruction),
					displ_size,
					displ,
					addr_size,
				);
			}

			InstrOpKind::Sae => GasFormatter::format_decorator(
				&self.d.options,
				output,
				instruction,
				operand,
				instruction_operand,
				&self.d.str_.sae,
				DecoratorKind::SuppressAllExceptions,
			),
			InstrOpKind::RnSae => GasFormatter::format_decorator(
				&self.d.options,
				output,
				instruction,
				operand,
				instruction_operand,
				&self.d.str_.rn_sae,
				DecoratorKind::RoundingControl,
			),
			InstrOpKind::RdSae => GasFormatter::format_decorator(
				&self.d.options,
				output,
				instruction,
				operand,
				instruction_operand,
				&self.d.str_.rd_sae,
				DecoratorKind::RoundingControl,
			),
			InstrOpKind::RuSae => GasFormatter::format_decorator(
				&self.d.options,
				output,
				instruction,
				operand,
				instruction_operand,
				&self.d.str_.ru_sae,
				DecoratorKind::RoundingControl,
			),
			InstrOpKind::RzSae => GasFormatter::format_decorator(
				&self.d.options,
				output,
				instruction,
				operand,
				instruction_operand,
				&self.d.str_.rz_sae,
				DecoratorKind::RoundingControl,
			),
			InstrOpKind::Rn => GasFormatter::format_decorator(
				&self.d.options,
				output,
				instruction,
				operand,
				instruction_operand,
				&self.d.str_.rn,
				DecoratorKind::RoundingControl,
			),
			InstrOpKind::Rd => GasFormatter::format_decorator(
				&self.d.options,
				output,
				instruction,
				operand,
				instruction_operand,
				&self.d.str_.rd,
				DecoratorKind::RoundingControl,
			),
			InstrOpKind::Ru => GasFormatter::format_decorator(
				&self.d.options,
				output,
				instruction,
				operand,
				instruction_operand,
				&self.d.str_.ru,
				DecoratorKind::RoundingControl,
			),
			InstrOpKind::Rz => GasFormatter::format_decorator(
				&self.d.options,
				output,
				instruction,
				operand,
				instruction_operand,
				&self.d.str_.rz,
				DecoratorKind::RoundingControl,
			),
		}

		if operand + 1 == op_info.op_count as u32 && instruction_internal::internal_has_op_mask_or_zeroing_masking(instruction) {
			if instruction.has_op_mask() {
				output.write("{", FormatterTextKind::Punctuation);
				GasFormatter::format_register_internal(&self.d, output, instruction, operand, instruction_operand, instruction.op_mask());
				output.write("}", FormatterTextKind::Punctuation);
			}
			if instruction.zeroing_masking() {
				GasFormatter::format_decorator(
					&self.d.options,
					output,
					instruction,
					operand,
					instruction_operand,
					&self.d.str_.z,
					DecoratorKind::ZeroingMasking,
				);
			}
		}
		#[cfg(feature = "mvex")]
		if mvex_rm_operand == operand {
			let conv = instruction.mvex_reg_mem_conv();
			if conv != MvexRegMemConv::None {
				let mvex = crate::mvex::get_mvex_info(instruction.code());
				if mvex.conv_fn != MvexConvFn::None {
					let tbl = if mvex.is_conv_fn_32() { &self.d.vec_.mvex_reg_mem_consts_32 } else { &self.d.vec_.mvex_reg_mem_consts_64 };
					let s = tbl[conv as usize];
					if s.len() != 0 {
						Self::format_decorator(&self.d.options, output, instruction, operand, instruction_operand, s, DecoratorKind::SwizzleMemConv);
					}
				}
			}
		}
	}

	fn format_decorator(
		options: &FormatterOptions, output: &mut dyn FormatterOutput, instruction: &Instruction, operand: u32, instruction_operand: Option<u32>,
		text: &FormatterString, decorator: DecoratorKind,
	) {
		output.write("{", FormatterTextKind::Punctuation);
		output.write_decorator(
			instruction,
			operand,
			instruction_operand,
			text.get(options.uppercase_decorators() || options.uppercase_all()),
			decorator,
		);
		output.write("}", FormatterTextKind::Punctuation);
	}

	#[inline]
	fn get_reg_str(d: &SelfData, mut reg: Register) -> &'static str {
		if d.options.prefer_st0() && reg == REGISTER_ST {
			reg = Register::ST0;
		}
		let reg_str = &GasFormatter::all_registers(d)[reg as usize];
		reg_str.get(d.options.uppercase_registers() || d.options.uppercase_all())
	}

	#[inline]
	fn format_register_internal(
		d: &SelfData, output: &mut dyn FormatterOutput, instruction: &Instruction, operand: u32, instruction_operand: Option<u32>, reg: Register,
	) {
		output.write_register(instruction, operand, instruction_operand, GasFormatter::get_reg_str(d, reg), reg);
	}

	#[allow(clippy::too_many_arguments)]
	fn format_memory(
		&mut self, output: &mut dyn FormatterOutput, instruction: &Instruction, operand: u32, instruction_operand: Option<u32>, seg_reg: Register,
		mut base_reg: Register, index_reg: Register, scale: u32, mut displ_size: u32, mut displ: i64, addr_size: u32,
	) {
		debug_assert!((scale as usize) < SCALE_NUMBERS.len());
		debug_assert!(get_address_size_in_bytes(base_reg, index_reg, displ_size, instruction.code_size()) == addr_size);

		let mut operand_options = FormatterOperandOptions::with_memory_size_options(self.d.options.memory_size_options());
		operand_options.set_rip_relative_addresses(self.d.options.rip_relative_addresses());
		let mut number_options = NumberFormattingOptions::with_displacement(&self.d.options);
		if let Some(ref mut options_provider) = self.options_provider {
			options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
		}

		let abs_addr;
		if base_reg == Register::RIP {
			abs_addr = displ as u64;
			if self.d.options.rip_relative_addresses() {
				displ = displ.wrapping_sub(instruction.next_ip() as i64);
			} else {
				debug_assert_eq!(index_reg, Register::None);
				base_reg = Register::None;
			}
			displ_size = 8;
		} else if base_reg == Register::EIP {
			abs_addr = displ as u32 as u64;
			if self.d.options.rip_relative_addresses() {
				displ = (displ as u32).wrapping_sub(instruction.next_ip32()) as i32 as i64;
			} else {
				debug_assert_eq!(index_reg, Register::None);
				base_reg = Register::None;
			}
			displ_size = 4;
		} else {
			abs_addr = displ as u64;
		}

		let symbol = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
			symbol_resolver.symbol(instruction, operand, instruction_operand, abs_addr, addr_size)
		} else {
			None
		};

		let use_scale =
			if addr_size == 2 || !show_index_scale(instruction, &self.d.options) { false } else { scale != 0 || self.d.options.always_show_scale() };

		let has_base_or_index_reg = base_reg != Register::None || index_reg != Register::None;

		let code_size = instruction.code_size();
		let seg_override = instruction.segment_prefix();
		let notrack_prefix = seg_override == Register::DS
			&& is_notrack_prefix_branch(instruction.code())
			&& !((code_size == CodeSize::Code16 || code_size == CodeSize::Code32)
				&& (base_reg == Register::BP || base_reg == Register::EBP || base_reg == Register::ESP));
		if self.d.options.always_show_segment_register()
			|| (seg_override != Register::None && !notrack_prefix && show_segment_prefix(Register::None, instruction, &self.d.options))
		{
			GasFormatter::format_register_internal(&self.d, output, instruction, operand, instruction_operand, seg_reg);
			output.write(":", FormatterTextKind::Punctuation);
		}

		if let Some(ref symbol) = symbol {
			FormatterOutputMethods::write1(
				output,
				instruction,
				operand,
				instruction_operand,
				&self.d.options,
				&mut self.number_formatter,
				&number_options,
				abs_addr,
				symbol,
				self.d.options.show_symbol_address(),
			);
		} else if !has_base_or_index_reg || (displ_size != 0 && (self.d.options.show_zero_displacements() || displ != 0)) {
			let orig_displ = displ as u64;
			let is_signed;
			if has_base_or_index_reg {
				is_signed = number_options.signed_number;
				if addr_size == 8 {
					if number_options.signed_number && displ < 0 {
						output.write("-", FormatterTextKind::Operator);
						displ = displ.wrapping_neg();
					}
					if number_options.displacement_leading_zeros {
						displ_size = 4;
					}
				} else if addr_size == 4 {
					if number_options.signed_number && (displ as i32) < 0 {
						output.write("-", FormatterTextKind::Operator);
						displ = (displ as i32).wrapping_neg() as u32 as i64;
					}
					if number_options.displacement_leading_zeros {
						displ_size = 4;
					}
				} else {
					debug_assert_eq!(addr_size, 2);
					if number_options.signed_number && (displ as i16) < 0 {
						output.write("-", FormatterTextKind::Operator);
						displ = (displ as i16).wrapping_neg() as u16 as i64;
					}
					if number_options.displacement_leading_zeros {
						displ_size = 2;
					}
				}
			} else {
				is_signed = false;
			}

			let (s, displ_kind) = if displ_size <= 1 && displ as u64 <= u8::MAX as u64 {
				(
					self.number_formatter.format_displ_u8(&self.d.options, &number_options, displ as u8),
					if is_signed { NumberKind::Int8 } else { NumberKind::UInt8 },
				)
			} else if displ_size <= 2 && displ as u64 <= u16::MAX as u64 {
				(
					self.number_formatter.format_displ_u16(&self.d.options, &number_options, displ as u16),
					if is_signed { NumberKind::Int16 } else { NumberKind::UInt16 },
				)
			} else if displ_size <= 4 && displ as u64 <= u32::MAX as u64 {
				(
					self.number_formatter.format_displ_u32(&self.d.options, &number_options, displ as u32),
					if is_signed { NumberKind::Int32 } else { NumberKind::UInt32 },
				)
			} else if displ_size <= 8 {
				(
					self.number_formatter.format_displ_u64(&self.d.options, &number_options, displ as u64),
					if is_signed { NumberKind::Int64 } else { NumberKind::UInt64 },
				)
			} else {
				unreachable!();
			};
			output.write_number(instruction, operand, instruction_operand, s, orig_displ, displ_kind, FormatterTextKind::Number);
		}

		if has_base_or_index_reg {
			output.write("(", FormatterTextKind::Punctuation);
			if self.d.options.space_after_memory_bracket() {
				output.write(" ", FormatterTextKind::Text);
			}

			if base_reg != Register::None && index_reg == Register::None && !use_scale {
				GasFormatter::format_register_internal(&self.d, output, instruction, operand, instruction_operand, base_reg);
			} else {
				if base_reg != Register::None {
					GasFormatter::format_register_internal(&self.d, output, instruction, operand, instruction_operand, base_reg);
				}

				output.write(",", FormatterTextKind::Punctuation);
				if self.d.options.gas_space_after_memory_operand_comma() {
					output.write(" ", FormatterTextKind::Text);
				}

				if index_reg != Register::None {
					GasFormatter::format_register_internal(&self.d, output, instruction, operand, instruction_operand, index_reg);

					if use_scale {
						output.write(",", FormatterTextKind::Punctuation);
						if self.d.options.gas_space_after_memory_operand_comma() {
							output.write(" ", FormatterTextKind::Text);
						}

						output.write_number(
							instruction,
							operand,
							instruction_operand,
							SCALE_NUMBERS[scale as usize],
							1u64 << scale,
							NumberKind::Int32,
							FormatterTextKind::Number,
						);
					}
				}
			}

			if self.d.options.space_after_memory_bracket() {
				output.write(" ", FormatterTextKind::Text);
			}
			output.write(")", FormatterTextKind::Punctuation);
		}

		let mem_size = instruction.memory_size();
		debug_assert!((mem_size as usize) < self.d.all_memory_sizes.len());
		let bcst_to = self.d.all_memory_sizes[mem_size as usize];
		if !bcst_to.is_default() {
			GasFormatter::format_decorator(&self.d.options, output, instruction, operand, instruction_operand, bcst_to, DecoratorKind::Broadcast);
		}
		#[cfg(feature = "mvex")]
		if instruction.is_mvex_eviction_hint() {
			Self::format_decorator(
				&self.d.options,
				output,
				instruction,
				operand,
				instruction_operand,
				&self.d.str_.mvex.eh,
				DecoratorKind::EvictionHint,
			);
		}
	}
}

impl Formatter for GasFormatter {
	#[inline]
	fn options(&self) -> &FormatterOptions {
		&self.d.options
	}

	#[inline]
	fn options_mut(&mut self) -> &mut FormatterOptions {
		&mut self.d.options
	}

	#[allow(clippy::missing_inline_in_public_items)]
	fn format_mnemonic_options(&mut self, instruction: &Instruction, output: &mut dyn FormatterOutput, options: u32) {
		let instr_info = &self.d.instr_infos[instruction.code() as usize];
		let op_info = instr_info.op_info(&self.d.options, instruction);
		let mut column = 0;
		self.format_mnemonic(instruction, output, &op_info, &mut column, options);
	}

	#[allow(clippy::missing_inline_in_public_items)]
	fn operand_count(&mut self, instruction: &Instruction) -> u32 {
		let instr_info = &self.d.instr_infos[instruction.code() as usize];
		let op_info = instr_info.op_info(&self.d.options, instruction);
		op_info.op_count as u32
	}

	#[cfg(feature = "instr_info")]
	#[allow(clippy::missing_inline_in_public_items)]
	fn op_access(&mut self, instruction: &Instruction, operand: u32) -> Result<Option<OpAccess>, IcedError> {
		let instr_info = &self.d.instr_infos[instruction.code() as usize];
		let op_info = instr_info.op_info(&self.d.options, instruction);
		if operand >= op_info.op_count as u32 {
			Err(IcedError::new("Invalid operand"))
		} else {
			Ok(op_info.op_access(operand))
		}
	}

	#[allow(clippy::missing_inline_in_public_items)]
	fn get_instruction_operand(&mut self, instruction: &Instruction, operand: u32) -> Result<Option<u32>, IcedError> {
		let instr_info = &self.d.instr_infos[instruction.code() as usize];
		let op_info = instr_info.op_info(&self.d.options, instruction);
		if operand >= op_info.op_count as u32 {
			Err(IcedError::new("Invalid operand"))
		} else {
			Ok(op_info.instruction_index(operand))
		}
	}

	#[allow(clippy::missing_inline_in_public_items)]
	fn get_formatter_operand(&mut self, instruction: &Instruction, instruction_operand: u32) -> Result<Option<u32>, IcedError> {
		let instr_info = &self.d.instr_infos[instruction.code() as usize];
		let op_info = instr_info.op_info(&self.d.options, instruction);
		if instruction_operand >= instruction.op_count() {
			Err(IcedError::new("Invalid instruction operand"))
		} else {
			Ok(op_info.operand_index(instruction_operand))
		}
	}

	#[allow(clippy::missing_inline_in_public_items)]
	fn format_operand(&mut self, instruction: &Instruction, output: &mut dyn FormatterOutput, operand: u32) -> Result<(), IcedError> {
		let instr_info = &self.d.instr_infos[instruction.code() as usize];
		let op_info = instr_info.op_info(&self.d.options, instruction);

		if operand >= op_info.op_count as u32 {
			Err(IcedError::new("Invalid operand"))
		} else {
			self.format_operand(instruction, output, &op_info, operand);
			Ok(())
		}
	}

	#[allow(clippy::missing_inline_in_public_items)]
	fn format_operand_separator(&mut self, _instruction: &Instruction, output: &mut dyn FormatterOutput) {
		output.write(",", FormatterTextKind::Punctuation);
		if self.d.options.space_after_operand_separator() {
			output.write(" ", FormatterTextKind::Text);
		}
	}

	#[allow(clippy::missing_inline_in_public_items)]
	fn format_all_operands(&mut self, instruction: &Instruction, output: &mut dyn FormatterOutput) {
		let instr_info = &self.d.instr_infos[instruction.code() as usize];
		let op_info = instr_info.op_info(&self.d.options, instruction);
		self.format_operands(instruction, output, &op_info);
	}

	#[allow(clippy::missing_inline_in_public_items)]
	fn format(&mut self, instruction: &Instruction, output: &mut dyn FormatterOutput) {
		let instr_info = &self.d.instr_infos[instruction.code() as usize];
		let op_info = instr_info.op_info(&self.d.options, instruction);

		let mut column = 0;
		self.format_mnemonic(instruction, output, &op_info, &mut column, FormatMnemonicOptions::NONE);

		if op_info.op_count != 0 {
			add_tabs(output, column, self.d.options.first_operand_char_index(), self.d.options.tab_size());
			self.format_operands(instruction, output, &op_info);
		}
	}

	#[inline]
	fn format_register(&mut self, register: Register) -> &str {
		GasFormatter::get_reg_str(&self.d, register)
	}

	#[inline]
	fn format_i8(&mut self, value: i8) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.d.options);
		self.number_formatter.format_i8(&self.d.options, &number_options, value)
	}

	#[inline]
	fn format_i16(&mut self, value: i16) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.d.options);
		self.number_formatter.format_i16(&self.d.options, &number_options, value)
	}

	#[inline]
	fn format_i32(&mut self, value: i32) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.d.options);
		self.number_formatter.format_i32(&self.d.options, &number_options, value)
	}

	#[inline]
	fn format_i64(&mut self, value: i64) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.d.options);
		self.number_formatter.format_i64(&self.d.options, &number_options, value)
	}

	#[inline]
	fn format_u8(&mut self, value: u8) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.d.options);
		self.number_formatter.format_u8(&self.d.options, &number_options, value)
	}

	#[inline]
	fn format_u16(&mut self, value: u16) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.d.options);
		self.number_formatter.format_u16(&self.d.options, &number_options, value)
	}

	#[inline]
	fn format_u32(&mut self, value: u32) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.d.options);
		self.number_formatter.format_u32(&self.d.options, &number_options, value)
	}

	#[inline]
	fn format_u64(&mut self, value: u64) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.d.options);
		self.number_formatter.format_u64(&self.d.options, &number_options, value)
	}

	#[inline]
	fn format_i8_options(&mut self, value: i8, number_options: &NumberFormattingOptions<'_>) -> &str {
		self.number_formatter.format_i8(&self.d.options, number_options, value)
	}

	#[inline]
	fn format_i16_options(&mut self, value: i16, number_options: &NumberFormattingOptions<'_>) -> &str {
		self.number_formatter.format_i16(&self.d.options, number_options, value)
	}

	#[inline]
	fn format_i32_options(&mut self, value: i32, number_options: &NumberFormattingOptions<'_>) -> &str {
		self.number_formatter.format_i32(&self.d.options, number_options, value)
	}

	#[inline]
	fn format_i64_options(&mut self, value: i64, number_options: &NumberFormattingOptions<'_>) -> &str {
		self.number_formatter.format_i64(&self.d.options, number_options, value)
	}

	#[inline]
	fn format_u8_options(&mut self, value: u8, number_options: &NumberFormattingOptions<'_>) -> &str {
		self.number_formatter.format_u8(&self.d.options, number_options, value)
	}

	#[inline]
	fn format_u16_options(&mut self, value: u16, number_options: &NumberFormattingOptions<'_>) -> &str {
		self.number_formatter.format_u16(&self.d.options, number_options, value)
	}

	#[inline]
	fn format_u32_options(&mut self, value: u32, number_options: &NumberFormattingOptions<'_>) -> &str {
		self.number_formatter.format_u32(&self.d.options, number_options, value)
	}

	#[inline]
	fn format_u64_options(&mut self, value: u64, number_options: &NumberFormattingOptions<'_>) -> &str {
		self.number_formatter.format_u64(&self.d.options, number_options, value)
	}
}

pub(super) fn eager_init() {
	let _ = &fmt_tbl::ALL_INFOS.as_ptr();
	let _ = &mem_size_tbl::MEM_SIZE_TBL.as_ptr();
	let _ = &regs::ALL_REGISTERS.as_ptr();
}
