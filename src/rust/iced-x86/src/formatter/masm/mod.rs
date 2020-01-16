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

mod enums;
mod fmt_data;
mod fmt_tbl;
mod info;
mod mem_size_tbl;
mod regs;

use self::enums::*;
use self::fmt_tbl::ALL_INFOS;
use self::info::*;
use self::mem_size_tbl::Info;
use self::mem_size_tbl::MEM_SIZE_TBL;
use self::regs::*;
use super::super::*;
use super::fmt_consts::*;
use super::fmt_utils::*;
use super::instruction_internal::get_address_size_in_bytes;
use super::num_fmt::*;
use super::regs_tbl::REGS_TBL;
use super::*;
#[cfg(not(feature = "std"))]
use alloc::boxed::Box;
#[cfg(not(feature = "std"))]
use alloc::vec::Vec;
use core::{mem, u16, u32, u8};

/// Masm formatter
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
/// let mut formatter = MasmFormatter::new();
/// formatter.options_mut().set_upper_case_mnemonics(true);
/// formatter.format(&instr, &mut output);
/// assert_eq!("VCVTNE2PS2BF16 zmm2{k5}{z},zmm6,dword bcst [rax+4]", output);
/// ```
#[allow(missing_debug_implementations)]
pub struct MasmFormatter<'a> {
	options: FormatterOptions,
	symbol_resolver: Option<&'a mut SymbolResolver>,
	options_provider: Option<&'a mut FormatterOptionsProvider>,
	all_registers: &'static Vec<FormatterString>,
	instr_infos: &'static Vec<Box<InstrInfo + Sync + Send>>,
	all_memory_sizes: &'static Vec<Info>,
	number_formatter: NumberFormatter,
	str_: &'static FormatterConstants,
	vec_: &'static FormatterArrayConstants,
}

impl<'a> Default for MasmFormatter<'a> {
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn default() -> Self {
		MasmFormatter::new()
	}
}

impl<'a> MasmFormatter<'a> {
	/// Creates a masm formatter
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn new() -> Self {
		MasmFormatter::with_options(None, None)
	}

	/// Creates a masm formatter
	///
	/// # Arguments
	///
	/// - `symbol_resolver`: Symbol resolver or `None`
	/// - `options_provider`: Operand options provider or `None`
	#[cfg_attr(has_must_use, must_use)]
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	pub fn with_options(symbol_resolver: Option<&'a mut SymbolResolver>, options_provider: Option<&'a mut FormatterOptionsProvider>) -> Self {
		Self {
			options: FormatterOptions::with_masm(),
			symbol_resolver,
			options_provider,
			all_registers: &*REGS_TBL,
			instr_infos: &*ALL_INFOS,
			all_memory_sizes: &*MEM_SIZE_TBL,
			number_formatter: NumberFormatter::new(),
			str_: &*FORMATTER_CONSTANTS,
			vec_: &*ARRAY_CONSTS,
		}
	}

	fn format_mnemonic(
		&mut self, instruction: &Instruction, output: &mut FormatterOutput, op_info: &InstrOpInfo, column: &mut u32, mnemonic_options: u32,
	) {
		let mut need_space = false;
		if (mnemonic_options & FormatMnemonicOptions::NO_PREFIXES) == 0 && (op_info.flags & InstrOpInfoFlags::MNEMONIC_IS_DIRECTIVE as u16) == 0 {
			let prefix_seg = instruction.segment_prefix();
			let has_notrack_prefix = prefix_seg == Register::DS && is_notrack_prefix_branch(instruction.code());
			if !has_notrack_prefix && prefix_seg != Register::None && MasmFormatter::show_segment_prefix(op_info) {
				self.format_prefix(
					output,
					instruction,
					column,
					&self.all_registers[prefix_seg as usize],
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

			if (op_info.flags & InstrOpInfoFlags::JCC_NOT_TAKEN as u16) != 0 {
				self.format_prefix(output, instruction, column, &self.str_.hnt, PrefixKind::HintNotTaken, &mut need_space);
			} else if (op_info.flags & InstrOpInfoFlags::JCC_TAKEN as u16) != 0 {
				self.format_prefix(output, instruction, column, &self.str_.ht, PrefixKind::HintTaken, &mut need_space);
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
				| InstrOpKind::ExtraImmediate8_Value3
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
				operand_options = FormatterOperandOptions::new(if self.options.show_branch_size() {
					FormatterOperandOptionsFlags::NONE
				} else {
					FormatterOperandOptionsFlags::NO_BRANCH_SIZE
				});
				if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
					symbol_resolver.symbol(instruction, operand, instruction_operand, imm64, imm_size)
				} else {
					None
				} {
					self.format_flow_control(output, get_flow_control(instruction), operand_options);
					let mut number_options = NumberFormattingOptions::with_branch(&self.options);
					if let Some(ref mut options_provider) = self.options_provider {
						options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
					}
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
					self.format_flow_control(output, flow_control, operand_options);
					let mut number_options = NumberFormattingOptions::with_branch(&self.options);
					if let Some(ref mut options_provider) = self.options_provider {
						options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
					}
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
				operand_options = FormatterOperandOptions::new(if self.options.show_branch_size() {
					FormatterOperandOptionsFlags::NONE
				} else {
					FormatterOperandOptionsFlags::NO_BRANCH_SIZE
				});
				if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
					symbol_resolver.symbol(instruction, operand, instruction_operand, imm64 as u32 as u64, imm_size)
				} else {
					None
				} {
					self.format_flow_control(output, get_flow_control(instruction), operand_options);
					debug_assert!(operand + 1 == 1);
					let mut number_options = NumberFormattingOptions::with_branch(&self.options);
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
					output.write(":", FormatterOutputTextKind::Punctuation);
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
					self.format_flow_control(output, flow_control, operand_options);
					let mut number_options = NumberFormattingOptions::with_branch(&self.options);
					if let Some(ref mut options_provider) = self.options_provider {
						options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
					}
					{
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
					output.write(":", FormatterOutputTextKind::Punctuation);
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

			InstrOpKind::Immediate8 | InstrOpKind::Immediate8_2nd | InstrOpKind::ExtraImmediate8_Value3 | InstrOpKind::DeclareByte => {
				if op_kind == InstrOpKind::Immediate8 {
					imm8 = instruction.immediate8();
				} else if op_kind == InstrOpKind::ExtraImmediate8_Value3 {
					imm8 = 3;
				} else if op_kind == InstrOpKind::Immediate8_2nd {
					imm8 = instruction.immediate8_2nd();
				} else {
					imm8 = instruction.get_declare_byte_value(operand as usize);
				}
				operand_options = FormatterOperandOptions::default();
				if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
					symbol_resolver.symbol(instruction, operand, instruction_operand, imm8 as u64, 1)
				} else {
					None
				} {
					if (symbol.flags & SymbolFlags::RELATIVE) == 0 {
						self.format_keyword(output, &self.str_.offset);
						output.write(" ", FormatterOutputTextKind::Text);
					}
					let mut number_options = NumberFormattingOptions::with_immediate(&self.options);
					if let Some(ref mut options_provider) = self.options_provider {
						options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
					}
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
					let mut number_options = NumberFormattingOptions::with_immediate(&self.options);
					if let Some(ref mut options_provider) = self.options_provider {
						options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
					}
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
				if op_kind == InstrOpKind::Immediate16 {
					imm16 = instruction.immediate16();
				} else if op_kind == InstrOpKind::Immediate8to16 {
					imm16 = instruction.immediate8to16() as u16;
				} else {
					imm16 = instruction.get_declare_word_value(operand as usize);
				}
				operand_options = FormatterOperandOptions::default();
				if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
					symbol_resolver.symbol(instruction, operand, instruction_operand, imm16 as u64, 2)
				} else {
					None
				} {
					if (symbol.flags & SymbolFlags::RELATIVE) == 0 {
						self.format_keyword(output, &self.str_.offset);
						output.write(" ", FormatterOutputTextKind::Text);
					}
					let mut number_options = NumberFormattingOptions::with_immediate(&self.options);
					if let Some(ref mut options_provider) = self.options_provider {
						options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
					}
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
					let mut number_options = NumberFormattingOptions::with_immediate(&self.options);
					if let Some(ref mut options_provider) = self.options_provider {
						options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
					}
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
				if op_kind == InstrOpKind::Immediate32 {
					imm32 = instruction.immediate32();
				} else if op_kind == InstrOpKind::Immediate8to32 {
					imm32 = instruction.immediate8to32() as u32;
				} else {
					imm32 = instruction.get_declare_dword_value(operand as usize);
				}
				operand_options = FormatterOperandOptions::default();
				if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
					symbol_resolver.symbol(instruction, operand, instruction_operand, imm32 as u64, 4)
				} else {
					None
				} {
					if (symbol.flags & SymbolFlags::RELATIVE) == 0 {
						self.format_keyword(output, &self.str_.offset);
						output.write(" ", FormatterOutputTextKind::Text);
					}
					let mut number_options = NumberFormattingOptions::with_immediate(&self.options);
					if let Some(ref mut options_provider) = self.options_provider {
						options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
					}
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
					let mut number_options = NumberFormattingOptions::with_immediate(&self.options);
					if let Some(ref mut options_provider) = self.options_provider {
						options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
					}
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
				if let Some(ref symbol) = if let Some(ref mut symbol_resolver) = self.symbol_resolver {
					symbol_resolver.symbol(instruction, operand, instruction_operand, imm64, 8)
				} else {
					None
				} {
					if (symbol.flags & SymbolFlags::RELATIVE) == 0 {
						self.format_keyword(output, &self.str_.offset);
						output.write(" ", FormatterOutputTextKind::Text);
					}
					let mut number_options = NumberFormattingOptions::with_immediate(&self.options);
					if let Some(ref mut options_provider) = self.options_provider {
						options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
					}
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
					let mut number_options = NumberFormattingOptions::with_immediate(&self.options);
					if let Some(ref mut options_provider) = self.options_provider {
						options_provider.operand_options(instruction, operand, instruction_operand, &mut operand_options, &mut number_options);
					}
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
				op_info.flags as u32,
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
				op_info.flags as u32,
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
				op_info.flags as u32,
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
				op_info.flags as u32,
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
				op_info.flags as u32,
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
				op_info.flags as u32,
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
				op_info.flags as u32,
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
				op_info.flags as u32,
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
				op_info.flags as u32,
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
				op_info.flags as u32,
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
					op_info.flags as u32,
				);
			}
		}

		if operand == 0 && instruction.has_op_mask() {
			output.write("{", FormatterOutputTextKind::Punctuation);
			self.format_register_internal(output, instruction, operand, instruction_operand, instruction.op_mask() as u32);
			output.write("}", FormatterOutputTextKind::Punctuation);
			if instruction.zeroing_masking() {
				self.format_decorator(output, instruction, operand, instruction_operand, &self.str_.z, DecoratorKind::ZeroingMasking);
			}
		}
		if operand + 1 == op_info.op_count as u32 {
			let rc = instruction.rounding_control();
			if rc != RoundingControl::None {
				const_assert_eq!(0, RoundingControl::None as u32);
				const_assert_eq!(1, RoundingControl::RoundToNearest as u32);
				const_assert_eq!(2, RoundingControl::RoundDown as u32);
				const_assert_eq!(3, RoundingControl::RoundUp as u32);
				const_assert_eq!(4, RoundingControl::RoundTowardZero as u32);
				output.write(" ", FormatterOutputTextKind::Text);
				self.format_decorator(
					output,
					instruction,
					operand,
					instruction_operand,
					&self.vec_.masm_rc_strings[rc as usize - 1],
					DecoratorKind::RoundingControl,
				);
			} else if instruction.suppress_all_exceptions() {
				output.write(" ", FormatterOutputTextKind::Text);
				self.format_decorator(output, instruction, operand, instruction_operand, &self.str_.sae, DecoratorKind::SuppressAllExceptions);
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
	fn get_reg_str(&self, reg: u32) -> &'static str {
		debug_assert!((reg as usize) < self.all_registers.len());
		let reg_str = &self.all_registers[reg as usize];
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
		addr_size: u32, flags: u32,
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

		let mut use_scale = scale != 0 || self.options.always_show_scale();
		if !use_scale {
			// [rsi] = base reg, [rsi*1] = index reg
			if base_reg == Register::None {
				use_scale = true;
			}
		}
		if addr_size == 2 {
			use_scale = false;
		}

		let code_size = instruction.code_size();
		let is1632 = code_size == CodeSize::Code16 || code_size == CodeSize::Code32;
		let has_mem_reg = base_reg != Register::None || index_reg != Register::None;
		let displ_in_brackets = if (!is1632 && !has_mem_reg && symbol.is_none())
			|| (is1632 && !has_mem_reg && symbol.is_none() && !self.options.masm_add_ds_prefix32() && seg_override == Register::None)
		{
			true
		} else {
			if symbol.is_some() {
				self.options.masm_symbol_displ_in_brackets()
			} else {
				self.options.masm_displ_in_brackets()
			}
		};
		let need_brackets = has_mem_reg || displ_in_brackets;

		self.format_memory_size(output, instruction, &symbol, mem_size, flags, operand_options);

		let notrack_prefix = seg_override == Register::DS
			&& is_notrack_prefix_branch(instruction.code())
			&& !((code_size == CodeSize::Code16 || code_size == CodeSize::Code32)
				&& (base_reg == Register::BP || base_reg == Register::EBP || base_reg == Register::ESP));
		if self.options.always_show_segment_register()
			|| (seg_override != Register::None && !notrack_prefix)
			|| (is1632 && !has_mem_reg && symbol.is_none() && self.options.masm_add_ds_prefix32())
		{
			self.format_register_internal(output, instruction, operand, instruction_operand, seg_reg as u32);
			output.write(":", FormatterOutputTextKind::Punctuation);
		}
		if !displ_in_brackets {
			self.format_memory_displ(
				output,
				instruction,
				operand,
				instruction_operand,
				&symbol,
				&mut operand_options,
				abs_addr,
				displ,
				displ_size,
				addr_size,
				false,
				!has_mem_reg,
			);
		}
		if need_brackets {
			output.write("[", FormatterOutputTextKind::Punctuation);
			if self.options.space_after_memory_bracket() {
				output.write(" ", FormatterOutputTextKind::Text);
			}
		}

		let mut need_plus = if base_reg != Register::None {
			self.format_register_internal(output, instruction, operand, instruction_operand, base_reg as u32);
			true
		} else {
			false
		};

		if index_reg != Register::None {
			if need_plus {
				if self.options.space_between_memory_add_operators() {
					output.write(" ", FormatterOutputTextKind::Text);
				}
				output.write("+", FormatterOutputTextKind::Operator);
				if self.options.space_between_memory_add_operators() {
					output.write(" ", FormatterOutputTextKind::Text);
				}
			}
			need_plus = true;

			if !use_scale {
				self.format_register_internal(output, instruction, operand, instruction_operand, index_reg as u32);
			} else if self.options.scale_before_index() {
				output.write_number(
					instruction,
					operand,
					instruction_operand,
					SCALE_NUMBERS[scale as usize],
					1u64 << scale,
					NumberKind::Int32,
					FormatterOutputTextKind::Number,
				);
				if self.options.space_between_memory_mul_operators() {
					output.write(" ", FormatterOutputTextKind::Text);
				}
				output.write("*", FormatterOutputTextKind::Operator);
				if self.options.space_between_memory_mul_operators() {
					output.write(" ", FormatterOutputTextKind::Text);
				}
				self.format_register_internal(output, instruction, operand, instruction_operand, index_reg as u32);
			} else {
				self.format_register_internal(output, instruction, operand, instruction_operand, index_reg as u32);
				if self.options.space_between_memory_mul_operators() {
					output.write(" ", FormatterOutputTextKind::Text);
				}
				output.write("*", FormatterOutputTextKind::Operator);
				if self.options.space_between_memory_mul_operators() {
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

		if displ_in_brackets {
			self.format_memory_displ(
				output,
				instruction,
				operand,
				instruction_operand,
				&symbol,
				&mut operand_options,
				abs_addr,
				displ,
				displ_size,
				addr_size,
				need_plus,
				!need_plus,
			);
		}

		if need_brackets {
			if self.options.space_after_memory_bracket() {
				output.write(" ", FormatterOutputTextKind::Text);
			}
			output.write("]", FormatterOutputTextKind::Punctuation);
		}
	}

	#[cfg_attr(feature = "cargo-clippy", allow(clippy::too_many_arguments))]
	fn format_memory_displ(
		&mut self, output: &mut FormatterOutput, instruction: &Instruction, operand: u32, instruction_operand: Option<u32>,
		symbol: &Option<SymbolResult>, operand_options: &mut FormatterOperandOptions, abs_addr: u64, mut displ: i64, mut displ_size: u32,
		addr_size: u32, need_plus: bool, force_displ: bool,
	) {
		let mut number_options = NumberFormattingOptions::with_displacement(&self.options);
		if let Some(ref mut options_provider) = self.options_provider {
			options_provider.operand_options(instruction, operand, instruction_operand, operand_options, &mut number_options);
		}
		if let &Some(ref symbol) = symbol {
			if need_plus {
				if self.options.space_between_memory_add_operators() {
					output.write(" ", FormatterOutputTextKind::Text);
				}
				if (symbol.flags & SymbolFlags::SIGNED) != 0 {
					output.write("-", FormatterOutputTextKind::Operator);
				} else {
					output.write("+", FormatterOutputTextKind::Operator);
				}
				if self.options.space_between_memory_add_operators() {
					output.write(" ", FormatterOutputTextKind::Text);
				}
			} else if (symbol.flags & SymbolFlags::SIGNED) != 0 {
				output.write("-", FormatterOutputTextKind::Operator);
			}

			FormatterOutputMethods::write2(
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
				false,
				self.options.space_between_memory_add_operators(),
			);
		} else if force_displ || (displ_size != 0 && (self.options.show_zero_displacements() || displ != 0)) {
			let orig_displ = displ as u64;
			let is_signed;
			if need_plus {
				is_signed = number_options.signed_number;
				if self.options.space_between_memory_add_operators() {
					output.write(" ", FormatterOutputTextKind::Text);
				}

				if addr_size == 4 {
					if !number_options.signed_number {
						output.write("+", FormatterOutputTextKind::Operator);
					} else if (displ as i32) < 0 {
						output.write("-", FormatterOutputTextKind::Operator);
						displ = (-(displ as i32)) as u32 as i64;
					} else {
						output.write("+", FormatterOutputTextKind::Operator);
					}
					if number_options.displacement_leading_zeroes {
						debug_assert!(displ_size <= 4);
						displ_size = 4;
					}
				} else if addr_size == 8 {
					if !number_options.signed_number {
						output.write("+", FormatterOutputTextKind::Operator);
					} else if displ < 0 {
						output.write("-", FormatterOutputTextKind::Operator);
						displ = -displ;
					} else {
						output.write("+", FormatterOutputTextKind::Operator);
					}
					if number_options.displacement_leading_zeroes {
						debug_assert!(displ_size <= 8);
						displ_size = 8;
					}
				} else {
					debug_assert_eq!(2, addr_size);
					if !number_options.signed_number {
						output.write("+", FormatterOutputTextKind::Operator);
					} else if (displ as i16) < 0 {
						output.write("-", FormatterOutputTextKind::Operator);
						displ = (-(displ as i16)) as u16 as i64;
					} else {
						output.write("+", FormatterOutputTextKind::Operator);
					}
					if number_options.displacement_leading_zeroes {
						debug_assert!(displ_size <= 2);
						displ_size = 2;
					}
				}
				if self.options.space_between_memory_add_operators() {
					output.write(" ", FormatterOutputTextKind::Text);
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

	fn format_memory_size(
		&mut self, output: &mut FormatterOutput, instruction: &Instruction, symbol: &Option<SymbolResult>, mem_size: MemorySize, flags: u32,
		operand_options: FormatterOperandOptions,
	) {
		let mem_size_options = operand_options.memory_size_options();
		if mem_size_options == MemorySizeOptions::Never {
			return;
		}

		let mem_type = flags & InstrOpInfoFlags::MEM_SIZE_MASK;
		if mem_type == InstrOpInfoFlags::MEM_SIZE_NOTHING {
			return;
		}

		debug_assert!((mem_size as usize) < self.all_memory_sizes.len());
		let mem_info = &self.all_memory_sizes[mem_size as usize];
		let mut mem_size_strings = mem_info.keywords;

		match mem_info.size {
			0 => {
				if mem_type == InstrOpInfoFlags::MEM_SIZE_DWORD_OR_QWORD {
					if instruction.code_size() == CodeSize::Code16 || instruction.code_size() == CodeSize::Code32 {
						mem_size_strings = &self.vec_.dword_ptr;
					} else {
						mem_size_strings = &self.vec_.qword_ptr;
					}
				}
			}

			8 => {
				if mem_type == InstrOpInfoFlags::MEM_SIZE_MMX {
					mem_size_strings = &self.vec_.mmword_ptr;
				}
			}

			16 => {
				if mem_type == InstrOpInfoFlags::MEM_SIZE_NORMAL {
					mem_size_strings = &self.vec_.oword_ptr;
				}
			}

			_ => {}
		}

		if mem_type == InstrOpInfoFlags::MEM_SIZE_XMMWORD_PTR {
			mem_size_strings = &self.vec_.xmmword_ptr;
		}

		if mem_size_options == MemorySizeOptions::Default {
			if symbol.is_some() && symbol.as_ref().unwrap().has_symbol_size() {
				if self.is_same_mem_size(mem_size_strings, mem_info.is_broadcast, symbol.as_ref().unwrap()) {
					return;
				}
			} else if (flags & InstrOpInfoFlags::SHOW_NO_MEM_SIZE_FORCE_SIZE) == 0 && !mem_info.is_broadcast {
				return;
			}
		} else if mem_size_options == MemorySizeOptions::Minimum {
			if symbol.is_some() && symbol.as_ref().unwrap().has_symbol_size() {
				if self.is_same_mem_size(mem_size_strings, mem_info.is_broadcast, symbol.as_ref().unwrap()) {
					return;
				}
			}
			if (flags & InstrOpInfoFlags::SHOW_MIN_MEM_SIZE_FORCE_SIZE) == 0 && !mem_info.is_broadcast {
				return;
			}
		} else {
			debug_assert_eq!(MemorySizeOptions::Always, mem_size_options);
		}

		for &name in mem_size_strings.iter() {
			self.format_keyword(output, name);
			output.write(" ", FormatterOutputTextKind::Text);
		}
	}

	fn is_same_mem_size(&self, mem_size_strings: &[&FormatterString], is_broadcast: bool, symbol: &SymbolResult) -> bool {
		if is_broadcast {
			return false;
		}
		debug_assert!((symbol.symbol_size as usize) < self.all_memory_sizes.len());
		let symbol_mem_info = &self.all_memory_sizes[symbol.symbol_size as usize];
		if symbol_mem_info.is_broadcast {
			false
		} else {
			MasmFormatter::is_same_mem_size_slice(mem_size_strings, symbol_mem_info.keywords)
		}
	}

	fn is_same_mem_size_slice(a: &[&FormatterString], b: &[&FormatterString]) -> bool {
		if a.len() != b.len() {
			return false;
		}
		for i in 0..a.len() {
			if a[i].get(false) != b[i].get(false) {
				return false;
			}
		}
		true
	}

	fn format_keyword(&mut self, output: &mut FormatterOutput, keyword: &FormatterString) {
		output.write(keyword.get(self.options.upper_case_keywords() || self.options.upper_case_all()), FormatterOutputTextKind::Keyword);
	}

	fn format_flow_control(&mut self, output: &mut FormatterOutput, kind: FormatterFlowControl, operand_options: FormatterOperandOptions) {
		if !operand_options.branch_size() {
			return;
		}

		match kind {
			FormatterFlowControl::AlwaysShortBranch => {}

			FormatterFlowControl::ShortBranch => {
				self.format_keyword(output, &self.str_.short);
				output.write(" ", FormatterOutputTextKind::Text);
			}

			FormatterFlowControl::NearBranch => {
				self.format_keyword(output, &self.str_.near);
				output.write(" ", FormatterOutputTextKind::Text);
				self.format_keyword(output, &self.str_.ptr);
				output.write(" ", FormatterOutputTextKind::Text);
			}

			FormatterFlowControl::NearCall => {}

			FormatterFlowControl::FarBranch | FormatterFlowControl::FarCall => {
				self.format_keyword(output, &self.str_.far);
				output.write(" ", FormatterOutputTextKind::Text);
				self.format_keyword(output, &self.str_.ptr);
				output.write(" ", FormatterOutputTextKind::Text);
			}

			FormatterFlowControl::Xbegin => {}
		}
	}
}

impl<'a> Formatter for MasmFormatter<'a> {
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
