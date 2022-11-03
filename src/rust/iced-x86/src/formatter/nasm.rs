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
use crate::formatter::instruction_internal::get_address_size_in_bytes;
use crate::formatter::nasm::enums::*;
use crate::formatter::nasm::fmt_tbl::ALL_INFOS;
use crate::formatter::nasm::info::*;
use crate::formatter::nasm::mem_size_tbl::Info;
use crate::formatter::nasm::mem_size_tbl::MEM_SIZE_TBL;
use crate::formatter::nasm::regs::*;
use crate::formatter::num_fmt::*;
use crate::formatter::*;
use crate::iced_constants::IcedConstants;
use crate::iced_error::IcedError;
use crate::instruction_internal;
use crate::*;
use alloc::boxed::Box;
use alloc::vec::Vec;
use core::mem;

/// Nasm formatter
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
/// let mut formatter = NasmFormatter::new();
/// formatter.options_mut().set_uppercase_mnemonics(true);
/// formatter.format(&instr, &mut output);
/// assert_eq!("VCVTNE2PS2BF16 zmm2{k5}{z},zmm6,[rax+4]{1to16}", output);
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
/// let mut formatter = NasmFormatter::with_options(Some(resolver), None);
/// formatter.format(&instr, &mut output);
/// assert_eq!("mov rcx,[rdx+my_data]", output);
/// ```
#[allow(missing_debug_implementations)]
pub struct NasmFormatter {
	d: SelfData,
	number_formatter: NumberFormatter,
	symbol_resolver: Option<Box<dyn SymbolResolver>>,
	options_provider: Option<Box<dyn FormatterOptionsProvider>>,
}

impl Default for NasmFormatter {
	#[must_use]
	#[inline]
	fn default() -> Self {
		NasmFormatter::new()
	}
}

// Read-only data which is needed a couple of times due to borrow checker
struct SelfData {
	options: FormatterOptions,
	all_registers: &'static [FormatterString; IcedConstants::REGISTER_ENUM_COUNT],
	instr_infos: &'static [Box<dyn InstrInfo + Send + Sync>; IcedConstants::CODE_ENUM_COUNT],
	all_memory_sizes: &'static [Info; IcedConstants::MEMORY_SIZE_ENUM_COUNT],
	str_: &'static FormatterConstants,
	vec_: &'static FormatterArrayConstants,
}

impl NasmFormatter {
	/// Creates a nasm formatter
	#[must_use]
	#[inline]
	pub fn new() -> Self {
		NasmFormatter::with_options(None, None)
	}

	/// Creates a nasm formatter
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
				options: FormatterOptions::with_nasm(),
				all_registers: &ALL_REGISTERS,
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

	fn format_mnemonic(
		&mut self, instruction: &Instruction, output: &mut dyn FormatterOutput, op_info: &InstrOpInfo<'_>, column: &mut u32, mnemonic_options: u32,
	) {
		let mut need_space = false;
		if (mnemonic_options & FormatMnemonicOptions::NO_PREFIXES) == 0 && (op_info.flags & InstrOpInfoFlags::MNEMONIC_IS_DIRECTIVE) == 0 {
			let prefix_seg = instruction.segment_prefix();

			const PREFIX_FLAGS: u32 = (InstrOpInfoFlags::SIZE_OVERRIDE_MASK << InstrOpInfoFlags::OP_SIZE_SHIFT)
				| (InstrOpInfoFlags::SIZE_OVERRIDE_MASK << InstrOpInfoFlags::ADDR_SIZE_SHIFT)
				| InstrOpInfoFlags::BND_PREFIX;
			if ((prefix_seg as u32) | instruction_internal::internal_has_any_of_lock_rep_repne_prefix(instruction) | (op_info.flags & PREFIX_FLAGS))
				!= 0
			{
				let mut prefix;

				prefix = &self.d.vec_.nasm_op_size_strings
					[((op_info.flags as usize) >> InstrOpInfoFlags::OP_SIZE_SHIFT) & InstrOpInfoFlags::SIZE_OVERRIDE_MASK as usize];
				if !prefix.is_default() {
					NasmFormatter::format_prefix(&self.d.options, output, instruction, column, prefix, PrefixKind::OperandSize, &mut need_space);
				}

				prefix = &self.d.vec_.nasm_addr_size_strings
					[((op_info.flags as usize) >> InstrOpInfoFlags::ADDR_SIZE_SHIFT) & InstrOpInfoFlags::SIZE_OVERRIDE_MASK as usize];
				if !prefix.is_default() {
					NasmFormatter::format_prefix(&self.d.options, output, instruction, column, prefix, PrefixKind::AddressSize, &mut need_space);
				}

				let has_notrack_prefix = prefix_seg == Register::DS && is_notrack_prefix_branch(instruction.code());
				if !has_notrack_prefix && prefix_seg != Register::None && self.show_segment_prefix(instruction, op_info) {
					NasmFormatter::format_prefix(
						&self.d.options,
						output,
						instruction,
						column,
						&self.d.all_registers[prefix_seg as usize],
						get_segment_register_prefix_kind(prefix_seg),
						&mut need_space,
					);
				}

				if instruction.has_xacquire_prefix() {
					NasmFormatter::format_prefix(
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
					NasmFormatter::format_prefix(
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
					NasmFormatter::format_prefix(&self.d.options, output, instruction, column, &self.d.str_.lock, PrefixKind::Lock, &mut need_space);
				}

				if has_notrack_prefix {
					NasmFormatter::format_prefix(
						&self.d.options,
						output,
						instruction,
						column,
						&self.d.str_.notrack,
						PrefixKind::Notrack,
						&mut need_space,
					);
				}
				let has_bnd = (op_info.flags & InstrOpInfoFlags::BND_PREFIX) != 0;
				if has_bnd {
					NasmFormatter::format_prefix(&self.d.options, output, instruction, column, &self.d.str_.bnd, PrefixKind::Bnd, &mut need_space);
				}

				if instruction.has_repe_prefix() && show_rep_or_repe_prefix(instruction.code(), &self.d.options) {
					if is_repe_or_repne_instruction(instruction.code()) {
						NasmFormatter::format_prefix(
							&self.d.options,
							output,
							instruction,
							column,
							get_mnemonic_cc(&self.d.options, 4, &self.d.str_.repe),
							PrefixKind::Repe,
							&mut need_space,
						);
					} else {
						NasmFormatter::format_prefix(
							&self.d.options,
							output,
							instruction,
							column,
							&self.d.str_.rep,
							PrefixKind::Rep,
							&mut need_space,
						);
					}
				}
				if !has_bnd && instruction.has_repne_prefix() && show_repne_prefix(instruction.code(), &self.d.options) {
					NasmFormatter::format_prefix(
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
			if (op_info.flags & InstrOpInfoFlags::MNEMONIC_IS_DIRECTIVE) != 0 {
				output.write(mnemonic.get(self.d.options.uppercase_keywords() || self.d.options.uppercase_all()), FormatterTextKind::Directive);
			} else {
				output.write_mnemonic(instruction, mnemonic.get(self.d.options.uppercase_mnemonics() || self.d.options.uppercase_all()));
			}
			*column += mnemonic.len() as u32;
		}
	}

	fn show_segment_prefix(&self, instruction: &Instruction, op_info: &InstrOpInfo<'_>) -> bool {
		if (op_info.flags & (InstrOpInfoFlags::JCC_NOT_TAKEN | InstrOpInfoFlags::JCC_TAKEN)) != 0 {
			return true;
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
			| Code::Maskmovdqu_rDI_xmm_xmm
			| Code::Xlat_m8
			| Code::Outsb_DX_m8
			| Code::Outsw_DX_m16
			| Code::Outsd_DX_m32
			| Code::Movsb_m8_m8
			| Code::Movsw_m16_m16
			| Code::Movsd_m32_m32
			| Code::Movsq_m64_m64
			| Code::Cmpsb_m8_m8
			| Code::Cmpsw_m16_m16
			| Code::Cmpsd_m32_m32
			| Code::Cmpsq_m64_m64
			| Code::Lodsb_AL_m8
			| Code::Lodsw_AX_m16
			| Code::Lodsd_EAX_m32
			| Code::Lodsq_RAX_m64 => return show_segment_prefix(Register::DS, instruction, &self.d.options),
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
				if instruction.op_kind(op_count.wrapping_sub(1)) == OpKind::Immediate8 {
					op_count.wrapping_sub(2)
				} else {
					op_count.wrapping_sub(1)
				}
			} else {
				u32::MAX
			}
		};

		let instruction_operand = op_info.instruction_index(operand);

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
				if (op_info.flags & InstrOpInfoFlags::REGISTER_TO) != 0 {
					NasmFormatter::format_keyword(&self.d.options, output, &self.d.str_.to);
					output.write(" ", FormatterTextKind::Text);
				}
				NasmFormatter::format_register_internal(&self.d, output, instruction, operand, instruction_operand, op_info.op_register(operand));
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
				operand_options = FormatterOperandOptions::new(if self.d.options.show_branch_size() {
					FormatterOperandOptionsFlags::NONE
				} else {
					FormatterOperandOptionsFlags::NO_BRANCH_SIZE
				});
				if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
					symbol_resolver.symbol(instruction, operand, instruction_operand, imm64, imm_size)
				} else {
					None
				} {
					NasmFormatter::format_flow_control(&self.d, output, op_info.flags, operand_options);
					let mut number_options = NumberFormattingOptions::with_branch(&self.d.options);
					if let Some(ref mut options_provider) = self.options_provider {
						options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
					}
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
					operand_options = FormatterOperandOptions::new(if self.d.options.show_branch_size() {
						FormatterOperandOptionsFlags::NONE
					} else {
						FormatterOperandOptionsFlags::NO_BRANCH_SIZE
					});
					flow_control = get_flow_control(instruction);
					NasmFormatter::format_flow_control(&self.d, output, op_info.flags, operand_options);
					let mut number_options = NumberFormattingOptions::with_branch(&self.d.options);
					if let Some(ref mut options_provider) = self.options_provider {
						options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
					}
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
				operand_options = FormatterOperandOptions::new(if self.d.options.show_branch_size() {
					FormatterOperandOptionsFlags::NONE
				} else {
					FormatterOperandOptionsFlags::NO_BRANCH_SIZE
				});
				let mut vec: Vec<SymResTextPart<'_>> = Vec::new();
				if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
					to_owned(symbol_resolver.symbol(instruction, operand, instruction_operand, imm64 as u32 as u64, imm_size), &mut vec)
				} else {
					None
				} {
					NasmFormatter::format_flow_control(&self.d, output, op_info.flags, operand_options);
					debug_assert!(operand + 1 == 1);
					let mut number_options = NumberFormattingOptions::with_branch(&self.d.options);
					if let Some(ref mut options_provider) = self.options_provider {
						options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
					}
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
					output.write(":", FormatterTextKind::Punctuation);
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
					NasmFormatter::format_flow_control(&self.d, output, op_info.flags, operand_options);
					let mut number_options = NumberFormattingOptions::with_branch(&self.d.options);
					if let Some(ref mut options_provider) = self.options_provider {
						options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
					}
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
					output.write(":", FormatterTextKind::Punctuation);
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
				if op_kind == InstrOpKind::Immediate8 {
					imm8 = instruction.immediate8();
				} else if op_kind == InstrOpKind::Immediate8_2nd {
					imm8 = instruction.immediate8_2nd();
				} else {
					imm8 = instruction.get_declare_byte_value(operand as usize);
				}
				operand_options = FormatterOperandOptions::default();
				let mut number_options = NumberFormattingOptions::with_immediate(&self.d.options);
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
				NasmFormatter::show_sign_extend_info(&self.d, output, op_info.flags);
				if op_kind == InstrOpKind::Immediate16 {
					imm16 = instruction.immediate16();
				} else if op_kind == InstrOpKind::Immediate8to16 {
					imm16 = instruction.immediate8to16() as u16;
				} else {
					imm16 = instruction.get_declare_word_value(operand as usize);
				}
				operand_options = FormatterOperandOptions::default();
				let mut number_options = NumberFormattingOptions::with_immediate(&self.d.options);
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
				NasmFormatter::show_sign_extend_info(&self.d, output, op_info.flags);
				if op_kind == InstrOpKind::Immediate32 {
					imm32 = instruction.immediate32();
				} else if op_kind == InstrOpKind::Immediate8to32 {
					imm32 = instruction.immediate8to32() as u32;
				} else {
					imm32 = instruction.get_declare_dword_value(operand as usize);
				}
				operand_options = FormatterOperandOptions::default();
				let mut number_options = NumberFormattingOptions::with_immediate(&self.d.options);
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
				NasmFormatter::show_sign_extend_info(&self.d, output, op_info.flags);
				if op_kind == InstrOpKind::Immediate32to64 {
					imm64 = instruction.immediate32to64() as u64;
				} else if op_kind == InstrOpKind::Immediate8to64 {
					imm64 = instruction.immediate8to64() as u64;
				} else if op_kind == InstrOpKind::Immediate64 {
					imm64 = instruction.immediate64();
				} else {
					imm64 = instruction.get_declare_qword_value(operand as usize);
				}
				operand_options = FormatterOperandOptions::default();
				let mut number_options = NumberFormattingOptions::with_immediate(&self.d.options);
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
				op_info.memory_size(),
				instruction.memory_segment(),
				Register::SI,
				Register::None,
				0,
				0,
				0,
				2,
				op_info.flags,
			),
			InstrOpKind::MemorySegESI => self.format_memory(
				output,
				instruction,
				operand,
				instruction_operand,
				op_info.memory_size(),
				instruction.memory_segment(),
				Register::ESI,
				Register::None,
				0,
				0,
				0,
				4,
				op_info.flags,
			),
			InstrOpKind::MemorySegRSI => self.format_memory(
				output,
				instruction,
				operand,
				instruction_operand,
				op_info.memory_size(),
				instruction.memory_segment(),
				Register::RSI,
				Register::None,
				0,
				0,
				0,
				8,
				op_info.flags,
			),
			InstrOpKind::MemorySegDI => self.format_memory(
				output,
				instruction,
				operand,
				instruction_operand,
				op_info.memory_size(),
				instruction.memory_segment(),
				Register::DI,
				Register::None,
				0,
				0,
				0,
				2,
				op_info.flags,
			),
			InstrOpKind::MemorySegEDI => self.format_memory(
				output,
				instruction,
				operand,
				instruction_operand,
				op_info.memory_size(),
				instruction.memory_segment(),
				Register::EDI,
				Register::None,
				0,
				0,
				0,
				4,
				op_info.flags,
			),
			InstrOpKind::MemorySegRDI => self.format_memory(
				output,
				instruction,
				operand,
				instruction_operand,
				op_info.memory_size(),
				instruction.memory_segment(),
				Register::RDI,
				Register::None,
				0,
				0,
				0,
				8,
				op_info.flags,
			),
			InstrOpKind::MemoryESDI => self.format_memory(
				output,
				instruction,
				operand,
				instruction_operand,
				op_info.memory_size(),
				Register::ES,
				Register::DI,
				Register::None,
				0,
				0,
				0,
				2,
				op_info.flags,
			),
			InstrOpKind::MemoryESEDI => self.format_memory(
				output,
				instruction,
				operand,
				instruction_operand,
				op_info.memory_size(),
				Register::ES,
				Register::EDI,
				Register::None,
				0,
				0,
				0,
				4,
				op_info.flags,
			),
			InstrOpKind::MemoryESRDI => self.format_memory(
				output,
				instruction,
				operand,
				instruction_operand,
				op_info.memory_size(),
				Register::ES,
				Register::RDI,
				Register::None,
				0,
				0,
				0,
				8,
				op_info.flags,
			),

			InstrOpKind::Memory => {
				let displ_size = instruction.memory_displ_size();
				let base_reg = instruction.memory_base();
				let index_reg = instruction.memory_index();
				let addr_size = get_address_size_in_bytes(base_reg, index_reg, displ_size, instruction.code_size());
				let displ = if addr_size == 8 { instruction.memory_displacement64() as i64 } else { instruction.memory_displacement32() as i64 };
				self.format_memory(
					output,
					instruction,
					operand,
					instruction_operand,
					op_info.memory_size(),
					instruction.memory_segment(),
					base_reg,
					index_reg,
					instruction_internal::internal_get_memory_index_scale(instruction),
					displ_size,
					displ,
					addr_size,
					op_info.flags,
				);
			}

			InstrOpKind::Sae => NasmFormatter::format_decorator(
				&self.d.options,
				output,
				instruction,
				operand,
				instruction_operand,
				&self.d.str_.sae,
				DecoratorKind::SuppressAllExceptions,
			),
			InstrOpKind::RnSae => NasmFormatter::format_decorator(
				&self.d.options,
				output,
				instruction,
				operand,
				instruction_operand,
				&self.d.str_.rn_sae,
				DecoratorKind::RoundingControl,
			),
			InstrOpKind::RdSae => NasmFormatter::format_decorator(
				&self.d.options,
				output,
				instruction,
				operand,
				instruction_operand,
				&self.d.str_.rd_sae,
				DecoratorKind::RoundingControl,
			),
			InstrOpKind::RuSae => NasmFormatter::format_decorator(
				&self.d.options,
				output,
				instruction,
				operand,
				instruction_operand,
				&self.d.str_.ru_sae,
				DecoratorKind::RoundingControl,
			),
			InstrOpKind::RzSae => NasmFormatter::format_decorator(
				&self.d.options,
				output,
				instruction,
				operand,
				instruction_operand,
				&self.d.str_.rz_sae,
				DecoratorKind::RoundingControl,
			),
			InstrOpKind::Rn => NasmFormatter::format_decorator(
				&self.d.options,
				output,
				instruction,
				operand,
				instruction_operand,
				&self.d.str_.rn,
				DecoratorKind::RoundingControl,
			),
			InstrOpKind::Rd => NasmFormatter::format_decorator(
				&self.d.options,
				output,
				instruction,
				operand,
				instruction_operand,
				&self.d.str_.rd,
				DecoratorKind::RoundingControl,
			),
			InstrOpKind::Ru => NasmFormatter::format_decorator(
				&self.d.options,
				output,
				instruction,
				operand,
				instruction_operand,
				&self.d.str_.ru,
				DecoratorKind::RoundingControl,
			),
			InstrOpKind::Rz => NasmFormatter::format_decorator(
				&self.d.options,
				output,
				instruction,
				operand,
				instruction_operand,
				&self.d.str_.rz,
				DecoratorKind::RoundingControl,
			),
		}

		if operand == 0 && instruction_internal::internal_has_op_mask_or_zeroing_masking(instruction) {
			if instruction.has_op_mask() {
				output.write("{", FormatterTextKind::Punctuation);
				NasmFormatter::format_register_internal(&self.d, output, instruction, operand, instruction_operand, instruction.op_mask());
				output.write("}", FormatterTextKind::Punctuation);
			}
			if instruction.zeroing_masking() {
				NasmFormatter::format_decorator(
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

	fn show_sign_extend_info(d: &SelfData, output: &mut dyn FormatterOutput, flags: u32) {
		if !d.options.nasm_show_sign_extended_immediate_size() {
			return;
		}

		// SAFETY: generated data is valid
		let sex_info: SignExtendInfo =
			unsafe { mem::transmute(((flags >> InstrOpInfoFlags::SIGN_EXTEND_INFO_SHIFT) & InstrOpInfoFlags::SIGN_EXTEND_INFO_MASK) as u8) };
		let keyword = match sex_info {
			SignExtendInfo::None => return,
			SignExtendInfo::Sex1to2 | SignExtendInfo::Sex1to4 | SignExtendInfo::Sex1to8 => &d.str_.byte,
			SignExtendInfo::Sex2 => &d.str_.word,
			SignExtendInfo::Sex4 => &d.str_.dword,
			SignExtendInfo::Sex4to8 => &d.str_.qword,
		};

		NasmFormatter::format_keyword(&d.options, output, keyword);
		output.write(" ", FormatterTextKind::Text);
	}

	fn format_flow_control(d: &SelfData, output: &mut dyn FormatterOutput, flags: u32, operand_options: FormatterOperandOptions) {
		if !operand_options.branch_size() {
			return;
		}
		let keywords = &d.vec_.nasm_branch_infos
			[((flags as usize) >> InstrOpInfoFlags::BRANCH_SIZE_INFO_SHIFT) & InstrOpInfoFlags::BRANCH_SIZE_INFO_MASK as usize];
		for &keyword in keywords {
			NasmFormatter::format_keyword(&d.options, output, keyword);
			output.write(" ", FormatterTextKind::Text);
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
	fn get_reg_str(d: &SelfData, reg: Register) -> &'static str {
		let reg_str = &d.all_registers[reg as usize];
		reg_str.get(d.options.uppercase_registers() || d.options.uppercase_all())
	}

	#[inline]
	fn format_register_internal(
		d: &SelfData, output: &mut dyn FormatterOutput, instruction: &Instruction, operand: u32, instruction_operand: Option<u32>, reg: Register,
	) {
		output.write_register(instruction, operand, instruction_operand, NasmFormatter::get_reg_str(d, reg), reg);
	}

	#[allow(clippy::too_many_arguments)]
	fn format_memory(
		&mut self, output: &mut dyn FormatterOutput, instruction: &Instruction, operand: u32, instruction_operand: Option<u32>, mem_size: MemorySize,
		seg_reg: Register, mut base_reg: Register, index_reg: Register, scale: u32, mut displ_size: u32, mut displ: i64, addr_size: u32,
		mut flags: u32,
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
		let mut add_rel_keyword = false;
		if base_reg == Register::RIP {
			abs_addr = displ as u64;
			if self.d.options.rip_relative_addresses() {
				displ = displ.wrapping_sub(instruction.next_ip() as i64);
			} else {
				debug_assert_eq!(index_reg, Register::None);
				base_reg = Register::None;
				flags &= !(InstrOpInfoFlags::MEMORY_SIZE_INFO_MASK << InstrOpInfoFlags::MEMORY_SIZE_INFO_SHIFT);
				add_rel_keyword = true;
			}
			displ_size = 8;
		} else if base_reg == Register::EIP {
			abs_addr = displ as u32 as u64;
			if self.d.options.rip_relative_addresses() {
				displ = (displ as u32).wrapping_sub(instruction.next_ip32()) as i32 as i64;
			} else {
				debug_assert_eq!(index_reg, Register::None);
				base_reg = Register::None;
				flags = (flags & !(InstrOpInfoFlags::MEMORY_SIZE_INFO_MASK << InstrOpInfoFlags::MEMORY_SIZE_INFO_SHIFT))
					| ((self::enums::MemorySizeInfo::Dword as u32) << InstrOpInfoFlags::MEMORY_SIZE_INFO_SHIFT);
				add_rel_keyword = true;
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

		let mut use_scale = scale != 0 || self.d.options.always_show_scale();
		if !use_scale {
			// [rsi] = base reg, [rsi*1] = index reg
			if base_reg == Register::None {
				use_scale = true;
			}
		}
		if addr_size == 2 || !show_index_scale(instruction, &self.d.options) {
			use_scale = false;
		}

		NasmFormatter::format_memory_size(&self.d, output, mem_size, flags, operand_options);

		output.write("[", FormatterTextKind::Punctuation);
		if self.d.options.space_after_memory_bracket() {
			output.write(" ", FormatterTextKind::Text);
		}

		let mem_size_name = &self.d.vec_.nasm_mem_size_infos
			[((flags >> InstrOpInfoFlags::MEMORY_SIZE_INFO_SHIFT) & InstrOpInfoFlags::MEMORY_SIZE_INFO_MASK) as usize];
		if !mem_size_name.is_default() {
			NasmFormatter::format_keyword(&self.d.options, output, mem_size_name);
			output.write(" ", FormatterTextKind::Text);
		}

		if add_rel_keyword {
			NasmFormatter::format_keyword(&self.d.options, output, &self.d.str_.rel);
			output.write(" ", FormatterTextKind::Text);
		}

		let code_size = instruction.code_size();
		let seg_override = instruction.segment_prefix();
		let notrack_prefix = seg_override == Register::DS
			&& is_notrack_prefix_branch(instruction.code())
			&& !((code_size == CodeSize::Code16 || code_size == CodeSize::Code32)
				&& (base_reg == Register::BP || base_reg == Register::EBP || base_reg == Register::ESP));
		if self.d.options.always_show_segment_register()
			|| (seg_override != Register::None && !notrack_prefix && show_segment_prefix(Register::None, instruction, &self.d.options))
		{
			NasmFormatter::format_register_internal(&self.d, output, instruction, operand, instruction_operand, seg_reg);
			output.write(":", FormatterTextKind::Punctuation);
		}

		let mut need_plus = if base_reg != Register::None {
			NasmFormatter::format_register_internal(&self.d, output, instruction, operand, instruction_operand, base_reg);
			true
		} else {
			false
		};

		if index_reg != Register::None {
			if need_plus {
				if self.d.options.space_between_memory_add_operators() {
					output.write(" ", FormatterTextKind::Text);
				}
				output.write("+", FormatterTextKind::Operator);
				if self.d.options.space_between_memory_add_operators() {
					output.write(" ", FormatterTextKind::Text);
				}
			}
			need_plus = true;

			if !use_scale {
				NasmFormatter::format_register_internal(&self.d, output, instruction, operand, instruction_operand, index_reg);
			} else if self.d.options.scale_before_index() {
				output.write_number(
					instruction,
					operand,
					instruction_operand,
					SCALE_NUMBERS[scale as usize],
					1u64 << scale,
					NumberKind::Int32,
					FormatterTextKind::Number,
				);
				if self.d.options.space_between_memory_mul_operators() {
					output.write(" ", FormatterTextKind::Text);
				}
				output.write("*", FormatterTextKind::Operator);
				if self.d.options.space_between_memory_mul_operators() {
					output.write(" ", FormatterTextKind::Text);
				}
				NasmFormatter::format_register_internal(&self.d, output, instruction, operand, instruction_operand, index_reg);
			} else {
				NasmFormatter::format_register_internal(&self.d, output, instruction, operand, instruction_operand, index_reg);
				if self.d.options.space_between_memory_mul_operators() {
					output.write(" ", FormatterTextKind::Text);
				}
				output.write("*", FormatterTextKind::Operator);
				if self.d.options.space_between_memory_mul_operators() {
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

		if let Some(ref symbol) = symbol {
			if need_plus {
				if self.d.options.space_between_memory_add_operators() {
					output.write(" ", FormatterTextKind::Text);
				}
				if (symbol.flags & SymbolFlags::SIGNED) != 0 {
					output.write("-", FormatterTextKind::Operator);
				} else {
					output.write("+", FormatterTextKind::Operator);
				}
				if self.d.options.space_between_memory_add_operators() {
					output.write(" ", FormatterTextKind::Text);
				}
			} else if (symbol.flags & SymbolFlags::SIGNED) != 0 {
				output.write("-", FormatterTextKind::Operator);
			}

			FormatterOutputMethods::write2(
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
				false,
				self.d.options.space_between_memory_add_operators(),
			);
		} else if !need_plus || (displ_size != 0 && (self.d.options.show_zero_displacements() || displ != 0)) {
			let orig_displ = displ as u64;
			let is_signed;
			if need_plus {
				is_signed = number_options.signed_number;
				if self.d.options.space_between_memory_add_operators() {
					output.write(" ", FormatterTextKind::Text);
				}

				if addr_size == 8 {
					if !number_options.signed_number {
						output.write("+", FormatterTextKind::Operator);
					} else if displ < 0 {
						displ = displ.wrapping_neg();
						output.write("-", FormatterTextKind::Operator);
					} else {
						output.write("+", FormatterTextKind::Operator);
					}
					if number_options.displacement_leading_zeros {
						displ_size = 4;
					}
				} else if addr_size == 4 {
					if !number_options.signed_number {
						output.write("+", FormatterTextKind::Operator);
					} else if (displ as i32) < 0 {
						displ = (displ as i32).wrapping_neg() as u32 as i64;
						output.write("-", FormatterTextKind::Operator);
					} else {
						output.write("+", FormatterTextKind::Operator);
					}
					if number_options.displacement_leading_zeros {
						displ_size = 4;
					}
				} else {
					debug_assert_eq!(addr_size, 2);
					if !number_options.signed_number {
						output.write("+", FormatterTextKind::Operator);
					} else if (displ as i16) < 0 {
						displ = (displ as i16).wrapping_neg() as u16 as i64;
						output.write("-", FormatterTextKind::Operator);
					} else {
						output.write("+", FormatterTextKind::Operator);
					}
					if number_options.displacement_leading_zeros {
						displ_size = 2;
					}
				}
				if self.d.options.space_between_memory_add_operators() {
					output.write(" ", FormatterTextKind::Text);
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

		if self.d.options.space_after_memory_bracket() {
			output.write(" ", FormatterTextKind::Text);
		}
		output.write("]", FormatterTextKind::Punctuation);

		debug_assert!((mem_size as usize) < self.d.all_memory_sizes.len());
		let bcst_to = &self.d.all_memory_sizes[mem_size as usize].bcst_to;
		if !bcst_to.is_default() {
			NasmFormatter::format_decorator(&self.d.options, output, instruction, operand, instruction_operand, bcst_to, DecoratorKind::Broadcast);
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

	fn format_memory_size(
		d: &SelfData, output: &mut dyn FormatterOutput, mem_size: MemorySize, flags: u32, operand_options: FormatterOperandOptions,
	) {
		let mem_size_options = operand_options.memory_size_options();
		if mem_size_options == MemorySizeOptions::Never {
			return;
		}

		if (flags & InstrOpInfoFlags::MEM_SIZE_NOTHING) != 0 {
			return;
		}

		debug_assert!((mem_size as usize) < d.all_memory_sizes.len());
		let mem_info = &d.all_memory_sizes[mem_size as usize];
		let keyword = &mem_info.keyword;
		if keyword.is_default() {
			return;
		}

		if mem_size_options == MemorySizeOptions::Default {
			if (flags & InstrOpInfoFlags::SHOW_NO_MEM_SIZE_FORCE_SIZE) == 0 {
				return;
			}
		} else if mem_size_options == MemorySizeOptions::Minimal {
			if (flags & InstrOpInfoFlags::SHOW_MIN_MEM_SIZE_FORCE_SIZE) == 0 {
				return;
			}
		} else {
			debug_assert_eq!(mem_size_options, MemorySizeOptions::Always);
		}

		let far_kind = &d.vec_.nasm_far_mem_size_infos
			[((flags as usize) >> InstrOpInfoFlags::FAR_MEMORY_SIZE_INFO_SHIFT) & InstrOpInfoFlags::FAR_MEMORY_SIZE_INFO_MASK as usize];
		if !far_kind.is_default() {
			NasmFormatter::format_keyword(&d.options, output, far_kind);
			output.write(" ", FormatterTextKind::Text);
		}
		NasmFormatter::format_keyword(&d.options, output, keyword);
		output.write(" ", FormatterTextKind::Text);
	}

	fn format_keyword(options: &FormatterOptions, output: &mut dyn FormatterOutput, keyword: &FormatterString) {
		output.write(keyword.get(options.uppercase_keywords() || options.uppercase_all()), FormatterTextKind::Keyword);
	}
}

impl Formatter for NasmFormatter {
	#[must_use]
	#[inline]
	fn options(&self) -> &FormatterOptions {
		&self.d.options
	}

	#[must_use]
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

	#[must_use]
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

	#[must_use]
	#[inline]
	fn format_register(&mut self, register: Register) -> &str {
		NasmFormatter::get_reg_str(&self.d, register)
	}

	#[must_use]
	#[inline]
	fn format_i8(&mut self, value: i8) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.d.options);
		self.number_formatter.format_i8(&self.d.options, &number_options, value)
	}

	#[must_use]
	#[inline]
	fn format_i16(&mut self, value: i16) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.d.options);
		self.number_formatter.format_i16(&self.d.options, &number_options, value)
	}

	#[must_use]
	#[inline]
	fn format_i32(&mut self, value: i32) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.d.options);
		self.number_formatter.format_i32(&self.d.options, &number_options, value)
	}

	#[must_use]
	#[inline]
	fn format_i64(&mut self, value: i64) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.d.options);
		self.number_formatter.format_i64(&self.d.options, &number_options, value)
	}

	#[must_use]
	#[inline]
	fn format_u8(&mut self, value: u8) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.d.options);
		self.number_formatter.format_u8(&self.d.options, &number_options, value)
	}

	#[must_use]
	#[inline]
	fn format_u16(&mut self, value: u16) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.d.options);
		self.number_formatter.format_u16(&self.d.options, &number_options, value)
	}

	#[must_use]
	#[inline]
	fn format_u32(&mut self, value: u32) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.d.options);
		self.number_formatter.format_u32(&self.d.options, &number_options, value)
	}

	#[must_use]
	#[inline]
	fn format_u64(&mut self, value: u64) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.d.options);
		self.number_formatter.format_u64(&self.d.options, &number_options, value)
	}

	#[must_use]
	#[inline]
	fn format_i8_options(&mut self, value: i8, number_options: &NumberFormattingOptions<'_>) -> &str {
		self.number_formatter.format_i8(&self.d.options, number_options, value)
	}

	#[must_use]
	#[inline]
	fn format_i16_options(&mut self, value: i16, number_options: &NumberFormattingOptions<'_>) -> &str {
		self.number_formatter.format_i16(&self.d.options, number_options, value)
	}

	#[must_use]
	#[inline]
	fn format_i32_options(&mut self, value: i32, number_options: &NumberFormattingOptions<'_>) -> &str {
		self.number_formatter.format_i32(&self.d.options, number_options, value)
	}

	#[must_use]
	#[inline]
	fn format_i64_options(&mut self, value: i64, number_options: &NumberFormattingOptions<'_>) -> &str {
		self.number_formatter.format_i64(&self.d.options, number_options, value)
	}

	#[must_use]
	#[inline]
	fn format_u8_options(&mut self, value: u8, number_options: &NumberFormattingOptions<'_>) -> &str {
		self.number_formatter.format_u8(&self.d.options, number_options, value)
	}

	#[must_use]
	#[inline]
	fn format_u16_options(&mut self, value: u16, number_options: &NumberFormattingOptions<'_>) -> &str {
		self.number_formatter.format_u16(&self.d.options, number_options, value)
	}

	#[must_use]
	#[inline]
	fn format_u32_options(&mut self, value: u32, number_options: &NumberFormattingOptions<'_>) -> &str {
		self.number_formatter.format_u32(&self.d.options, number_options, value)
	}

	#[must_use]
	#[inline]
	fn format_u64_options(&mut self, value: u64, number_options: &NumberFormattingOptions<'_>) -> &str {
		self.number_formatter.format_u64(&self.d.options, number_options, value)
	}
}
