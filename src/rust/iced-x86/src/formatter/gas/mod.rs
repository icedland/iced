/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

pub(super) mod enums;
mod fmt_data;
mod fmt_tbl;
mod info;
mod mem_size_tbl;
mod regs;

use self::enums::*;
use self::fmt_tbl::ALL_INFOS;
use self::info::*;
use self::mem_size_tbl::MEM_SIZE_TBL;
use self::regs::*;
use super::super::*;
use super::fmt_consts::*;
use super::fmt_utils::*;
use super::instruction_internal::get_address_size_in_bytes;
use super::num_fmt::*;
use super::regs_tbl::REGS_TBL;
use super::*;
use std::{mem, u16, u32, u8};

/// GNU assembler (AT&T) formatter
#[allow(missing_debug_implementations)]
pub struct GasFormatter<'a> {
	options: FormatterOptions,
	symbol_resolver: Option<&'a mut SymbolResolver>,
	options_provider: Option<&'a mut FormatterOptionsProvider>,
	all_registers: &'static Vec<FormatterString>,
	all_registers_naked: &'static Vec<FormatterString>,
	instr_infos: &'static Vec<Box<InstrInfo + Sync>>,
	all_memory_sizes: &'static Vec<&'static FormatterString>,
	number_formatter: NumberFormatter,
	str_: &'static FormatterConstants,
	vec_: &'static FormatterArrayConstants,
}

impl<'a> Default for GasFormatter<'a> {
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn default() -> Self {
		GasFormatter::new()
	}
}

impl<'a> GasFormatter<'a> {
	const IMMEDIATE_VALUE_PREFIX: &'static str = "$";

	/// Creates a gas (AT&T) formatter
	#[cfg_attr(has_must_use, must_use)]
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
	#[cfg_attr(has_must_use, must_use)]
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	pub fn with_options(symbol_resolver: Option<&'a mut SymbolResolver>, options_provider: Option<&'a mut FormatterOptionsProvider>) -> Self {
		Self {
			options: FormatterOptions::with_gas(),
			symbol_resolver,
			options_provider,
			all_registers: &*ALL_REGISTERS,
			all_registers_naked: &*REGS_TBL,
			instr_infos: &*ALL_INFOS,
			all_memory_sizes: &*MEM_SIZE_TBL,
			number_formatter: NumberFormatter::new(),
			str_: &*FORMATTER_CONSTANTS,
			vec_: &*ARRAY_CONSTS,
		}
	}

	fn all_registers(&self) -> &'static Vec<FormatterString> {
		if self.options.gas_naked_registers() {
			self.all_registers_naked
		} else {
			self.all_registers
		}
	}

	fn format_mnemonic(
		&mut self, instruction: &Instruction, output: &mut FormatterOutput, op_info: &InstrOpInfo, column: &mut u32, mnemonic_options: u32,
	) {
		let mut need_space = false;
		if (mnemonic_options & FormatMnemonicOptions::NO_PREFIXES) == 0 && (op_info.flags & InstrOpInfoFlags::MNEMONIC_IS_DIRECTIVE as u16) == 0 {
			let mut prefix;

			if (op_info.flags & InstrOpInfoFlags::OP_SIZE_IS_BYTE_DIRECTIVE as u16) != 0 {
				let size_override: SizeOverride = unsafe {
					mem::transmute((((op_info.flags as u32) >> InstrOpInfoFlags::OP_SIZE_SHIFT) & InstrOpInfoFlags::SIZE_OVERRIDE_MASK) as u8)
				};
				match size_override {
					SizeOverride::None => {}

					SizeOverride::Size16 | SizeOverride::Size32 => {
						output.write(
							self.str_.dot_byte.get(self.options.upper_case_keywords() || self.options.upper_case_all()),
							FormatterOutputTextKind::Directive,
						);
						output.write(" ", FormatterOutputTextKind::Text);
						let number_options = NumberFormattingOptions::with_immediate(&self.options);
						let s = self.number_formatter.format_u8(&self.options, &number_options, 0x66);
						output.write(s, FormatterOutputTextKind::Number);
						output.write(";", FormatterOutputTextKind::Punctuation);
						output.write(" ", FormatterOutputTextKind::Text);
						*column += (self.str_.dot_byte.len() + 1 + s.len() + 1 + 1) as u32;
					}

					SizeOverride::Size64 => {
						self.format_prefix(output, instruction, column, &self.str_.rex_w, PrefixKind::OperandSize, &mut need_space)
					}
				}
			} else {
				prefix = &self.vec_.gas_op_size_strings
					[((op_info.flags as usize) >> InstrOpInfoFlags::OP_SIZE_SHIFT) & InstrOpInfoFlags::SIZE_OVERRIDE_MASK as usize];
				if !prefix.is_default() {
					self.format_prefix(output, instruction, column, prefix, PrefixKind::OperandSize, &mut need_space);
				}
			}

			prefix = &self.vec_.gas_addr_size_strings
				[((op_info.flags as usize) >> InstrOpInfoFlags::ADDR_SIZE_SHIFT) & InstrOpInfoFlags::SIZE_OVERRIDE_MASK as usize];
			if !prefix.is_default() {
				self.format_prefix(output, instruction, column, prefix, PrefixKind::AddressSize, &mut need_space);
			}

			let prefix_seg = instruction.segment_prefix();
			let has_notrack_prefix = prefix_seg == Register::DS && is_notrack_prefix_branch(instruction.code());
			if !has_notrack_prefix && prefix_seg != Register::None && GasFormatter::show_segment_prefix(op_info) {
				self.format_prefix(
					output,
					instruction,
					column,
					&self.all_registers_naked[prefix_seg as usize],
					get_segment_register_prefix_kind(prefix_seg),
					&mut need_space,
				);
			}

			if instruction.has_xacquire_prefix() {
				self.format_prefix(output, instruction, column, &self.str_.xacquire, PrefixKind::Xacquire, &mut need_space);
			}
			if instruction.has_xrelease_prefix() {
				self.format_prefix(output, instruction, column, &self.str_.xrelease, PrefixKind::Xrelease, &mut need_space);
			}
			if instruction.has_lock_prefix() {
				self.format_prefix(output, instruction, column, &self.str_.lock, PrefixKind::Lock, &mut need_space);
			}

			let has_bnd = (op_info.flags & InstrOpInfoFlags::BND_PREFIX as u16) != 0;
			if instruction.has_repe_prefix() {
				if is_repe_or_repne_instruction(instruction.code()) {
					self.format_prefix(output, instruction, column, &self.str_.repe, PrefixKind::Repe, &mut need_space);
				} else {
					self.format_prefix(output, instruction, column, &self.str_.rep, PrefixKind::Rep, &mut need_space);
				}
			}
			if instruction.has_repne_prefix() && !has_bnd {
				self.format_prefix(output, instruction, column, &self.str_.repne, PrefixKind::Repne, &mut need_space);
			}

			if has_notrack_prefix {
				self.format_prefix(output, instruction, column, &self.str_.notrack, PrefixKind::Notrack, &mut need_space);
			}

			if has_bnd {
				self.format_prefix(output, instruction, column, &self.str_.bnd, PrefixKind::Bnd, &mut need_space);
			}
		}

		if (mnemonic_options & FormatMnemonicOptions::NO_MNEMONIC) == 0 {
			if need_space {
				output.write(" ", FormatterOutputTextKind::Text);
				*column += 1;
			}
			let mnemonic = op_info.mnemonic;
			if (op_info.flags & InstrOpInfoFlags::MNEMONIC_IS_DIRECTIVE as u16) != 0 {
				output.write(mnemonic.get(self.options.upper_case_keywords() || self.options.upper_case_all()), FormatterOutputTextKind::Directive);
			} else {
				output.write_mnemonic(instruction, mnemonic.get(self.options.upper_case_mnemonics() || self.options.upper_case_all()));
			}
			*column += mnemonic.len() as u32;
		}
		if (mnemonic_options & FormatMnemonicOptions::NO_PREFIXES) == 0 {
			if (op_info.flags & InstrOpInfoFlags::JCC_NOT_TAKEN as u16) != 0 {
				self.format_branch_hint(output, column, &self.str_.pn);
			} else if (op_info.flags & InstrOpInfoFlags::JCC_TAKEN as u16) != 0 {
				self.format_branch_hint(output, column, &self.str_.pt);
			}
		}
	}

	fn format_branch_hint(&mut self, output: &mut FormatterOutput, column: &mut u32, br_hint: &FormatterString) {
		output.write(",", FormatterOutputTextKind::Text);
		output.write(br_hint.get(self.options.upper_case_prefixes() || self.options.upper_case_all()), FormatterOutputTextKind::Keyword);
		*column += 1 + br_hint.len() as u32;
	}

	fn show_segment_prefix(op_info: &InstrOpInfo) -> bool {
		if (op_info.flags & (InstrOpInfoFlags::JCC_NOT_TAKEN | InstrOpInfoFlags::JCC_TAKEN) as u16) != 0 {
			return false;
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
				| InstrOpKind::Memory64
				| InstrOpKind::Memory => return false,
			}
		}
		true
	}

	fn format_prefix(
		&mut self, output: &mut FormatterOutput, instruction: &Instruction, column: &mut u32, prefix: &FormatterString, prefix_kind: PrefixKind,
		need_space: &mut bool,
	) {
		if *need_space {
			*column += 1;
			output.write(" ", FormatterOutputTextKind::Text);
		}
		output.write_prefix(instruction, prefix.get(self.options.upper_case_prefixes() || self.options.upper_case_all()), prefix_kind);
		*column += prefix.len() as u32;
		*need_space = true;
	}

	fn format_operands(&mut self, instruction: &Instruction, output: &mut FormatterOutput, op_info: &InstrOpInfo) {
		for i in 0..op_info.op_count as u32 {
			if i > 0 {
				output.write(",", FormatterOutputTextKind::Punctuation);
				if self.options.space_after_operand_separator() {
					output.write(" ", FormatterOutputTextKind::Text);
				}
			}
			self.format_operand(instruction, output, op_info, i);
		}
	}

	fn format_operand(&mut self, instruction: &Instruction, output: &mut FormatterOutput, op_info: &InstrOpInfo, operand: u32) {
		debug_assert!(operand < op_info.op_count as u32);

		let instruction_operand = op_info.instruction_index(operand);

		if (op_info.flags & InstrOpInfoFlags::INDIRECT_OPERAND as u16) != 0 {
			output.write("*", FormatterOutputTextKind::Operator);
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
				self.format_register_internal(output, instruction, operand, instruction_operand, op_info.op_register(operand) as u32)
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
				let mut number_options = NumberFormattingOptions::with_branch(&self.options);
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
						&self.options,
						&mut self.number_formatter,
						&number_options,
						imm64,
						symbol,
						self.options.show_symbol_address(),
					);
				} else {
					flow_control = get_flow_control(instruction);
					let s = if op_kind == InstrOpKind::NearBranch32 {
						self.number_formatter.format_u32_zeroes(
							&self.options,
							&number_options,
							instruction.near_branch32(),
							number_options.leading_zeroes,
						)
					} else if op_kind == InstrOpKind::NearBranch64 {
						self.number_formatter.format_u64_zeroes(
							&self.options,
							&number_options,
							instruction.near_branch64(),
							number_options.leading_zeroes,
						)
					} else {
						self.number_formatter.format_u16_zeroes(
							&self.options,
							&number_options,
							instruction.near_branch16(),
							number_options.leading_zeroes,
						)
					};
					output.write_number(
						instruction,
						operand,
						instruction_operand,
						s,
						imm64,
						number_kind,
						if is_call(flow_control) { FormatterOutputTextKind::FunctionAddress } else { FormatterOutputTextKind::LabelAddress },
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
				let mut number_options = NumberFormattingOptions::with_branch(&self.options);
				operand_options = FormatterOperandOptions::default();
				if let Some(ref mut options_provider) = self.options_provider {
					options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
				}
				if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
					symbol_resolver.symbol(instruction, operand, instruction_operand, imm64 as u32 as u64, imm_size)
				} else {
					None
				} {
					output.write(GasFormatter::IMMEDIATE_VALUE_PREFIX, FormatterOutputTextKind::Operator);
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
							&self.options,
							&mut self.number_formatter,
							&number_options,
							instruction.far_branch_selector() as u64,
							selector_symbol,
							self.options.show_symbol_address(),
						);
					} else {
						let s = self.number_formatter.format_u16_zeroes(
							&self.options,
							&number_options,
							instruction.far_branch_selector(),
							number_options.leading_zeroes,
						);
						output.write_number(
							instruction,
							operand,
							instruction_operand,
							s,
							instruction.far_branch_selector() as u64,
							NumberKind::UInt16,
							FormatterOutputTextKind::SelectorValue,
						);
					}
					output.write(",", FormatterOutputTextKind::Punctuation);
					if self.options.space_after_operand_separator() {
						output.write(" ", FormatterOutputTextKind::Text);
					}
					output.write(GasFormatter::IMMEDIATE_VALUE_PREFIX, FormatterOutputTextKind::Operator);
					FormatterOutputMethods::write1(
						output,
						instruction,
						operand,
						instruction_operand,
						&self.options,
						&mut self.number_formatter,
						&number_options,
						imm64,
						symbol,
						self.options.show_symbol_address(),
					);
				} else {
					flow_control = get_flow_control(instruction);
					{
						let s = self.number_formatter.format_u16_zeroes(
							&self.options,
							&number_options,
							instruction.far_branch_selector(),
							number_options.leading_zeroes,
						);
						output.write(GasFormatter::IMMEDIATE_VALUE_PREFIX, FormatterOutputTextKind::Operator);
						output.write_number(
							instruction,
							operand,
							instruction_operand,
							s,
							instruction.far_branch_selector() as u64,
							NumberKind::UInt16,
							FormatterOutputTextKind::SelectorValue,
						);
					}
					output.write(",", FormatterOutputTextKind::Punctuation);
					if self.options.space_after_operand_separator() {
						output.write(" ", FormatterOutputTextKind::Text);
					}
					let s = if op_kind == InstrOpKind::FarBranch32 {
						self.number_formatter.format_u32_zeroes(
							&self.options,
							&number_options,
							instruction.far_branch32(),
							number_options.leading_zeroes,
						)
					} else {
						self.number_formatter.format_u16_zeroes(
							&self.options,
							&number_options,
							instruction.far_branch16(),
							number_options.leading_zeroes,
						)
					};
					output.write(GasFormatter::IMMEDIATE_VALUE_PREFIX, FormatterOutputTextKind::Operator);
					output.write_number(
						instruction,
						operand,
						instruction_operand,
						s,
						imm64,
						number_kind,
						if is_call(flow_control) { FormatterOutputTextKind::FunctionAddress } else { FormatterOutputTextKind::LabelAddress },
					);
				}
			}

			InstrOpKind::Immediate8 | InstrOpKind::Immediate8_2nd | InstrOpKind::DeclareByte => {
				if op_kind != InstrOpKind::DeclareByte {
					output.write(GasFormatter::IMMEDIATE_VALUE_PREFIX, FormatterOutputTextKind::Operator);
				}
				if op_kind == InstrOpKind::Immediate8 {
					imm8 = instruction.immediate8();
				} else if op_kind == InstrOpKind::Immediate8_2nd {
					imm8 = instruction.immediate8_2nd();
				} else {
					imm8 = instruction.get_declare_byte_value(operand as usize);
				}
				let mut number_options = NumberFormattingOptions::with_immediate(&self.options);
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
						&self.options,
						&mut self.number_formatter,
						&number_options,
						imm8 as u64,
						symbol,
						self.options.show_symbol_address(),
					);
				} else {
					if number_options.signed_number {
						imm64 = imm8 as i8 as u64;
						number_kind = NumberKind::Int8;
						if (imm8 as i8) < 0 {
							output.write("-", FormatterOutputTextKind::Operator);
							imm8 = -(imm8 as i8) as u8;
						}
					} else {
						imm64 = imm8 as u64;
						number_kind = NumberKind::UInt8;
					}
					let s = self.number_formatter.format_u8(&self.options, &number_options, imm8);
					output.write_number(instruction, operand, instruction_operand, s, imm64, number_kind, FormatterOutputTextKind::Number);
				}
			}

			InstrOpKind::Immediate16 | InstrOpKind::Immediate8to16 | InstrOpKind::DeclareWord => {
				if op_kind != InstrOpKind::DeclareWord {
					output.write(GasFormatter::IMMEDIATE_VALUE_PREFIX, FormatterOutputTextKind::Operator);
				}
				if op_kind == InstrOpKind::Immediate16 {
					imm16 = instruction.immediate16();
				} else if op_kind == InstrOpKind::Immediate8to16 {
					imm16 = instruction.immediate8to16() as u16;
				} else {
					imm16 = instruction.get_declare_word_value(operand as usize);
				}
				let mut number_options = NumberFormattingOptions::with_immediate(&self.options);
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
						&self.options,
						&mut self.number_formatter,
						&number_options,
						imm16 as u64,
						symbol,
						self.options.show_symbol_address(),
					);
				} else {
					if number_options.signed_number {
						imm64 = imm16 as i16 as u64;
						number_kind = NumberKind::Int16;
						if (imm16 as i16) < 0 {
							output.write("-", FormatterOutputTextKind::Operator);
							imm16 = -(imm16 as i16) as u16;
						}
					} else {
						imm64 = imm16 as u64;
						number_kind = NumberKind::UInt16;
					}
					let s = self.number_formatter.format_u16(&self.options, &number_options, imm16);
					output.write_number(instruction, operand, instruction_operand, s, imm64, number_kind, FormatterOutputTextKind::Number);
				}
			}

			InstrOpKind::Immediate32 | InstrOpKind::Immediate8to32 | InstrOpKind::DeclareDword => {
				if op_kind != InstrOpKind::DeclareDword {
					output.write(GasFormatter::IMMEDIATE_VALUE_PREFIX, FormatterOutputTextKind::Operator);
				}
				if op_kind == InstrOpKind::Immediate32 {
					imm32 = instruction.immediate32();
				} else if op_kind == InstrOpKind::Immediate8to32 {
					imm32 = instruction.immediate8to32() as u32;
				} else {
					imm32 = instruction.get_declare_dword_value(operand as usize);
				}
				let mut number_options = NumberFormattingOptions::with_immediate(&self.options);
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
						&self.options,
						&mut self.number_formatter,
						&number_options,
						imm32 as u64,
						symbol,
						self.options.show_symbol_address(),
					);
				} else {
					if number_options.signed_number {
						imm64 = imm32 as i32 as u64;
						number_kind = NumberKind::Int32;
						if (imm32 as i32) < 0 {
							output.write("-", FormatterOutputTextKind::Operator);
							imm32 = -(imm32 as i32) as u32;
						}
					} else {
						imm64 = imm32 as u64;
						number_kind = NumberKind::UInt32;
					}
					let s = self.number_formatter.format_u32(&self.options, &number_options, imm32);
					output.write_number(instruction, operand, instruction_operand, s, imm64, number_kind, FormatterOutputTextKind::Number);
				}
			}

			InstrOpKind::Immediate64 | InstrOpKind::Immediate8to64 | InstrOpKind::Immediate32to64 | InstrOpKind::DeclareQword => {
				if op_kind != InstrOpKind::DeclareQword {
					output.write(GasFormatter::IMMEDIATE_VALUE_PREFIX, FormatterOutputTextKind::Operator);
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
				let mut number_options = NumberFormattingOptions::with_immediate(&self.options);
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
						&self.options,
						&mut self.number_formatter,
						&number_options,
						imm64,
						symbol,
						self.options.show_symbol_address(),
					);
				} else {
					value64 = imm64;
					if number_options.signed_number {
						number_kind = NumberKind::Int64;
						if (imm64 as i64) < 0 {
							output.write("-", FormatterOutputTextKind::Operator);
							imm64 = -(imm64 as i64) as u64;
						}
					} else {
						number_kind = NumberKind::UInt64;
					}
					let s = self.number_formatter.format_u64(&self.options, &number_options, imm64);
					output.write_number(instruction, operand, instruction_operand, s, value64, number_kind, FormatterOutputTextKind::Number);
				}
			}

			InstrOpKind::MemorySegSI => self.format_memory(
				output,
				instruction,
				operand,
				instruction_operand,
				instruction.memory_size(),
				instruction.segment_prefix(),
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
				instruction.memory_size(),
				instruction.segment_prefix(),
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
				instruction.memory_size(),
				instruction.segment_prefix(),
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
				instruction.memory_size(),
				instruction.segment_prefix(),
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
				instruction.memory_size(),
				instruction.segment_prefix(),
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
				instruction.memory_size(),
				instruction.segment_prefix(),
				instruction.memory_segment(),
				Register::RDI,
				Register::None,
				0,
				0,
				0,
				8,
			),
			InstrOpKind::MemoryESDI => self.format_memory(
				output,
				instruction,
				operand,
				instruction_operand,
				instruction.memory_size(),
				instruction.segment_prefix(),
				Register::ES,
				Register::DI,
				Register::None,
				0,
				0,
				0,
				2,
			),
			InstrOpKind::MemoryESEDI => self.format_memory(
				output,
				instruction,
				operand,
				instruction_operand,
				instruction.memory_size(),
				instruction.segment_prefix(),
				Register::ES,
				Register::EDI,
				Register::None,
				0,
				0,
				0,
				4,
			),
			InstrOpKind::MemoryESRDI => self.format_memory(
				output,
				instruction,
				operand,
				instruction_operand,
				instruction.memory_size(),
				instruction.segment_prefix(),
				Register::ES,
				Register::RDI,
				Register::None,
				0,
				0,
				0,
				8,
			),
			InstrOpKind::Memory64 => self.format_memory(
				output,
				instruction,
				operand,
				instruction_operand,
				instruction.memory_size(),
				instruction.segment_prefix(),
				instruction.memory_segment(),
				Register::None,
				Register::None,
				0,
				8,
				instruction.memory_address64() as i64,
				8,
			),

			InstrOpKind::Memory => {
				let displ_size = instruction.memory_displ_size();
				let base_reg = instruction.memory_base();
				let mut index_reg = instruction.memory_index();
				let addr_size = get_address_size_in_bytes(base_reg, index_reg, displ_size, instruction.code_size());
				let displ = if addr_size == 8 { instruction.memory_displacement64() as i64 } else { instruction.memory_displacement() as i64 };
				if (op_info.flags & InstrOpInfoFlags::IGNORE_INDEX_REG as u16) != 0 {
					index_reg = Register::None;
				}
				self.format_memory(
					output,
					instruction,
					operand,
					instruction_operand,
					instruction.memory_size(),
					instruction.segment_prefix(),
					instruction.memory_segment(),
					base_reg,
					index_reg,
					super::super::instruction_internal::internal_get_memory_index_scale(instruction),
					displ_size,
					displ,
					addr_size,
				);
			}

			InstrOpKind::Sae => {
				self.format_decorator(output, instruction, operand, instruction_operand, &self.str_.sae, DecoratorKind::SuppressAllExceptions)
			}
			InstrOpKind::RnSae => {
				self.format_decorator(output, instruction, operand, instruction_operand, &self.str_.rn_sae, DecoratorKind::RoundingControl)
			}
			InstrOpKind::RdSae => {
				self.format_decorator(output, instruction, operand, instruction_operand, &self.str_.rd_sae, DecoratorKind::RoundingControl)
			}
			InstrOpKind::RuSae => {
				self.format_decorator(output, instruction, operand, instruction_operand, &self.str_.ru_sae, DecoratorKind::RoundingControl)
			}
			InstrOpKind::RzSae => {
				self.format_decorator(output, instruction, operand, instruction_operand, &self.str_.rz_sae, DecoratorKind::RoundingControl)
			}
		}

		if operand + 1 == op_info.op_count as u32 && instruction.has_op_mask() {
			output.write("{", FormatterOutputTextKind::Punctuation);
			self.format_register_internal(output, instruction, operand, instruction_operand, instruction.op_mask() as u32);
			output.write("}", FormatterOutputTextKind::Punctuation);
			if instruction.zeroing_masking() {
				self.format_decorator(output, instruction, operand, instruction_operand, &self.str_.z, DecoratorKind::ZeroingMasking);
			}
		}
	}

	fn format_decorator(
		&mut self, output: &mut FormatterOutput, instruction: &Instruction, operand: u32, instruction_operand: Option<u32>, text: &FormatterString,
		decorator: DecoratorKind,
	) {
		output.write("{", FormatterOutputTextKind::Punctuation);
		output.write_decorator(
			instruction,
			operand,
			instruction_operand,
			text.get(self.options.upper_case_decorators() || self.options.upper_case_all()),
			decorator,
		);
		output.write("}", FormatterOutputTextKind::Punctuation);
	}

	#[inline]
	fn get_reg_str(&self, reg_num: u32) -> &'static str {
		debug_assert!((reg_num as usize) < self.all_registers.len());
		let reg_str = &self.all_registers[reg_num as usize];
		reg_str.get(self.options.upper_case_registers() || self.options.upper_case_all())
	}

	#[inline]
	fn format_register_internal(
		&mut self, output: &mut FormatterOutput, instruction: &Instruction, operand: u32, instruction_operand: Option<u32>, reg_num: u32,
	) {
		const_assert_eq!(1, Registers::EXTRA_REGISTERS);
		output.write_register(
			instruction,
			operand,
			instruction_operand,
			self.get_reg_str(reg_num),
			if reg_num == Registers::REGISTER_ST { Register::ST0 } else { unsafe { mem::transmute(reg_num as u8) } },
		);
	}

	#[cfg_attr(feature = "cargo-clippy", allow(clippy::too_many_arguments))]
	fn format_memory(
		&mut self, output: &mut FormatterOutput, instruction: &Instruction, operand: u32, instruction_operand: Option<u32>, mem_size: MemorySize,
		seg_override: Register, seg_reg: Register, mut base_reg: Register, index_reg: Register, scale: u32, mut displ_size: u32, mut displ: i64,
		addr_size: u32,
	) {
		debug_assert!((scale as usize) < SCALE_NUMBERS.len());
		debug_assert!(get_address_size_in_bytes(base_reg, index_reg, displ_size, instruction.code_size()) == addr_size);

		let mut operand_options = FormatterOperandOptions::with_memory_size_options(self.options.memory_size_options());
		operand_options.set_rip_relative_addresses(self.options.rip_relative_addresses());
		// We have to call this method twice because of borrowck
		if let Some(ref mut options_provider) = self.options_provider {
			let mut number_options = NumberFormattingOptions::with_displacement(&self.options);
			options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
		}

		let abs_addr;
		if base_reg == Register::RIP {
			abs_addr = (instruction.next_ip() as i64).wrapping_add(displ as i32 as i64) as u64;
			if !operand_options.rip_relative_addresses() {
				debug_assert_eq!(Register::None, index_reg);
				base_reg = Register::None;
				displ = abs_addr as i64;
				displ_size = 8;
			}
		} else if base_reg == Register::EIP {
			abs_addr = instruction.next_ip32().wrapping_add(displ as u32) as u64;
			if !operand_options.rip_relative_addresses() {
				debug_assert_eq!(Register::None, index_reg);
				base_reg = Register::None;
				displ = abs_addr as i64;
				displ_size = 4;
			}
		} else {
			abs_addr = displ as u64;
		}

		let symbol = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
			symbol_resolver.symbol(instruction, operand, instruction_operand, abs_addr, addr_size)
		} else {
			None
		};

		let use_scale = if addr_size == 2 { false } else { scale != 0 || self.options.always_show_scale() };

		let has_base_or_index_reg = base_reg != Register::None || index_reg != Register::None;

		let code_size = instruction.code_size();
		let notrack_prefix = seg_override == Register::DS
			&& is_notrack_prefix_branch(instruction.code())
			&& !((code_size == CodeSize::Code16 || code_size == CodeSize::Code32)
				&& (base_reg == Register::BP || base_reg == Register::EBP || base_reg == Register::ESP));
		if self.options.always_show_segment_register() || (seg_override != Register::None && !notrack_prefix) {
			self.format_register_internal(output, instruction, operand, instruction_operand, seg_reg as u32);
			output.write(":", FormatterOutputTextKind::Punctuation);
		}

		{
			let mut number_options = NumberFormattingOptions::with_displacement(&self.options);
			if let Some(ref mut options_provider) = self.options_provider {
				options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
			}
			if let Some(ref symbol) = symbol {
				FormatterOutputMethods::write1(
					output,
					instruction,
					operand,
					instruction_operand,
					&self.options,
					&mut self.number_formatter,
					&number_options,
					abs_addr,
					symbol,
					self.options.show_symbol_address(),
				);
			} else if !has_base_or_index_reg || (displ_size != 0 && (self.options.show_zero_displacements() || displ != 0)) {
				let orig_displ = displ as u64;
				let is_signed;
				if has_base_or_index_reg {
					is_signed = number_options.signed_number;
					if addr_size == 4 {
						if number_options.signed_number && (displ as i32) < 0 {
							output.write("-", FormatterOutputTextKind::Operator);
							displ = (-(displ as i32)) as u32 as i64;
						}
						if number_options.sign_extend_immediate {
							debug_assert!(displ_size <= 4);
							displ_size = 4;
						}
					} else if addr_size == 8 {
						if number_options.signed_number && displ < 0 {
							output.write("-", FormatterOutputTextKind::Operator);
							displ = -displ;
						}
						if number_options.sign_extend_immediate {
							debug_assert!(displ_size <= 8);
							displ_size = 8;
						}
					} else {
						debug_assert_eq!(2, addr_size);
						if number_options.signed_number && (displ as i16) < 0 {
							output.write("-", FormatterOutputTextKind::Operator);
							displ = (-(displ as i16)) as u16 as i64;
						}
						if number_options.sign_extend_immediate {
							debug_assert!(displ_size <= 2);
							displ_size = 2;
						}
					}
				} else {
					is_signed = false;
				}

				let (s, displ_kind) = if displ_size <= 1 && displ as u64 <= u8::MAX as u64 {
					(
						self.number_formatter.format_u8(&self.options, &number_options, displ as u8),
						if is_signed { NumberKind::Int8 } else { NumberKind::UInt8 },
					)
				} else if displ_size <= 2 && displ as u64 <= u16::MAX as u64 {
					(
						self.number_formatter.format_u16(&self.options, &number_options, displ as u16),
						if is_signed { NumberKind::Int16 } else { NumberKind::UInt16 },
					)
				} else if displ_size <= 4 && displ as u64 <= u32::MAX as u64 {
					(
						self.number_formatter.format_u32(&self.options, &number_options, displ as u32),
						if is_signed { NumberKind::Int32 } else { NumberKind::UInt32 },
					)
				} else if displ_size <= 8 {
					(
						self.number_formatter.format_u64(&self.options, &number_options, displ as u64),
						if is_signed { NumberKind::Int64 } else { NumberKind::UInt64 },
					)
				} else {
					unreachable!();
				};
				output.write_number(instruction, operand, instruction_operand, s, orig_displ, displ_kind, FormatterOutputTextKind::Number);
			}
		}

		if has_base_or_index_reg {
			output.write("(", FormatterOutputTextKind::Punctuation);
			if self.options.space_after_memory_bracket() {
				output.write(" ", FormatterOutputTextKind::Text);
			}

			if base_reg != Register::None && index_reg == Register::None && !use_scale {
				self.format_register_internal(output, instruction, operand, instruction_operand, base_reg as u32);
			} else {
				if base_reg != Register::None {
					self.format_register_internal(output, instruction, operand, instruction_operand, base_reg as u32);
				}

				output.write(",", FormatterOutputTextKind::Punctuation);
				if self.options.gas_space_after_memory_operand_comma() {
					output.write(" ", FormatterOutputTextKind::Text);
				}

				if index_reg != Register::None {
					self.format_register_internal(output, instruction, operand, instruction_operand, index_reg as u32);
				}

				if use_scale {
					output.write(",", FormatterOutputTextKind::Punctuation);
					if self.options.gas_space_after_memory_operand_comma() {
						output.write(" ", FormatterOutputTextKind::Text);
					}

					output.write_number(
						instruction,
						operand,
						instruction_operand,
						SCALE_NUMBERS[scale as usize],
						1u64 << scale,
						NumberKind::Int32,
						FormatterOutputTextKind::Number,
					);
				}
			}

			if self.options.space_after_memory_bracket() {
				output.write(" ", FormatterOutputTextKind::Text);
			}
			output.write(")", FormatterOutputTextKind::Punctuation);
		}

		debug_assert!((mem_size as usize) < self.all_memory_sizes.len());
		let bcst_to = self.all_memory_sizes[mem_size as usize];
		if !bcst_to.is_default() {
			self.format_decorator(output, instruction, operand, instruction_operand, bcst_to, DecoratorKind::Broadcast);
		}
	}
}

impl<'a> Formatter for GasFormatter<'a> {
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn options(&self) -> &FormatterOptions {
		&self.options
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn options_mut(&mut self) -> &mut FormatterOptions {
		&mut self.options
	}

	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	fn format_mnemonic_options(&mut self, instruction: &Instruction, output: &mut FormatterOutput, options: u32) {
		let instr_info = &self.instr_infos[instruction.code() as usize];
		let op_info = instr_info.op_info(&self.options, instruction);
		let mut column = 0;
		self.format_mnemonic(instruction, output, &op_info, &mut column, options);
	}

	#[cfg_attr(has_must_use, must_use)]
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	fn operand_count(&mut self, instruction: &Instruction) -> u32 {
		let instr_info = &self.instr_infos[instruction.code() as usize];
		let op_info = instr_info.op_info(&self.options, instruction);
		op_info.op_count as u32
	}

	#[cfg(feature = "instr_info")]
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	fn op_access(&mut self, instruction: &Instruction, operand: u32) -> Option<OpAccess> {
		let instr_info = &self.instr_infos[instruction.code() as usize];
		let op_info = instr_info.op_info(&self.options, instruction);
		if operand >= op_info.op_count as u32 {
			panic!();
		}
		op_info.op_access(operand)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	fn get_instruction_operand(&mut self, instruction: &Instruction, operand: u32) -> Option<u32> {
		let instr_info = &self.instr_infos[instruction.code() as usize];
		let op_info = instr_info.op_info(&self.options, instruction);
		if operand >= op_info.op_count as u32 {
			panic!();
		}
		op_info.instruction_index(operand)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	fn get_formatter_operand(&mut self, instruction: &Instruction, instruction_operand: u32) -> Option<u32> {
		let instr_info = &self.instr_infos[instruction.code() as usize];
		let op_info = instr_info.op_info(&self.options, instruction);
		if instruction_operand >= instruction.op_count() {
			panic!();
		}
		op_info.operand_index(instruction_operand)
	}

	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	fn format_operand(&mut self, instruction: &Instruction, output: &mut FormatterOutput, operand: u32) {
		let instr_info = &self.instr_infos[instruction.code() as usize];
		let op_info = instr_info.op_info(&self.options, instruction);

		if operand >= op_info.op_count as u32 {
			panic!();
		}
		self.format_operand(instruction, output, &op_info, operand);
	}

	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	fn format_operand_separator(&mut self, _instruction: &Instruction, output: &mut FormatterOutput) {
		output.write(",", FormatterOutputTextKind::Punctuation);
		if self.options.space_after_operand_separator() {
			output.write(" ", FormatterOutputTextKind::Text);
		}
	}

	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	fn format_all_operands(&mut self, instruction: &Instruction, output: &mut FormatterOutput) {
		let instr_info = &self.instr_infos[instruction.code() as usize];
		let op_info = instr_info.op_info(&self.options, instruction);
		self.format_operands(instruction, output, &op_info);
	}

	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	fn format(&mut self, instruction: &Instruction, output: &mut FormatterOutput) {
		let instr_info = &self.instr_infos[instruction.code() as usize];
		let op_info = instr_info.op_info(&self.options, instruction);

		let mut column = 0;
		self.format_mnemonic(instruction, output, &op_info, &mut column, FormatMnemonicOptions::NONE);

		if op_info.op_count != 0 {
			add_tabs(output, column, self.options.first_operand_char_index(), self.options.tab_size());
			self.format_operands(instruction, output, &op_info);
		}
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_register(&mut self, register: Register) -> &str {
		self.get_reg_str(register as u32)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_i8(&mut self, value: i8) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.options);
		self.number_formatter.format_i8(&self.options, &number_options, value)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_i16(&mut self, value: i16) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.options);
		self.number_formatter.format_i16(&self.options, &number_options, value)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_i32(&mut self, value: i32) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.options);
		self.number_formatter.format_i32(&self.options, &number_options, value)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_i64(&mut self, value: i64) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.options);
		self.number_formatter.format_i64(&self.options, &number_options, value)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_u8(&mut self, value: u8) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.options);
		self.number_formatter.format_u8(&self.options, &number_options, value)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_u16(&mut self, value: u16) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.options);
		self.number_formatter.format_u16(&self.options, &number_options, value)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_u32(&mut self, value: u32) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.options);
		self.number_formatter.format_u32(&self.options, &number_options, value)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_u64(&mut self, value: u64) -> &str {
		let number_options = NumberFormattingOptions::with_immediate(&self.options);
		self.number_formatter.format_u64(&self.options, &number_options, value)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_i8_options(&mut self, value: i8, number_options: &NumberFormattingOptions) -> &str {
		self.number_formatter.format_i8(&self.options, &number_options, value)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_i16_options(&mut self, value: i16, number_options: &NumberFormattingOptions) -> &str {
		self.number_formatter.format_i16(&self.options, &number_options, value)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_i32_options(&mut self, value: i32, number_options: &NumberFormattingOptions) -> &str {
		self.number_formatter.format_i32(&self.options, &number_options, value)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_i64_options(&mut self, value: i64, number_options: &NumberFormattingOptions) -> &str {
		self.number_formatter.format_i64(&self.options, &number_options, value)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_u8_options(&mut self, value: u8, number_options: &NumberFormattingOptions) -> &str {
		self.number_formatter.format_u8(&self.options, &number_options, value)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_u16_options(&mut self, value: u16, number_options: &NumberFormattingOptions) -> &str {
		self.number_formatter.format_u16(&self.options, &number_options, value)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_u32_options(&mut self, value: u32, number_options: &NumberFormattingOptions) -> &str {
		self.number_formatter.format_u32(&self.options, &number_options, value)
	}

	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_u64_options(&mut self, value: u64, number_options: &NumberFormattingOptions) -> &str {
		self.number_formatter.format_u64(&self.options, &number_options, value)
	}
}
