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

#![allow(unused_variables)] //TODO: REMOVE

mod enums;
mod fmt_opts;
#[cfg(any(feature = "gas_formatter", feature = "all_formatters"))]
mod gas;
#[cfg(any(feature = "intel_formatter", feature = "all_formatters"))]
mod intel;
#[cfg(any(feature = "masm_formatter", feature = "all_formatters"))]
mod masm;
#[cfg(any(feature = "nasm_formatter", feature = "all_formatters"))]
mod nasm;
mod num_fmt_opts;
mod symres;

pub use self::enums::*;
pub use self::fmt_opts::*;
#[cfg(any(feature = "gas_formatter", feature = "all_formatters"))]
pub use self::gas::*;
#[cfg(any(feature = "intel_formatter", feature = "all_formatters"))]
pub use self::intel::*;
#[cfg(any(feature = "masm_formatter", feature = "all_formatters"))]
pub use self::masm::*;
#[cfg(any(feature = "nasm_formatter", feature = "all_formatters"))]
pub use self::nasm::*;
pub use self::num_fmt_opts::*;
pub use self::symres::*;
use super::*;
use std::{u32, u8};

/// Used by a [`Formatter`] to write all text
///
/// [`Formatter`]: trait.Formatter.html
pub trait FormatterOutput {
	/// Writes text and text kind
	///
	/// # Arguments
	///
	/// - `text`: Text, can be an empty string
	/// - `kind`: Text kind. This value can be identical to the previous value passed to this method. It's the responsibility of the implementer to merge any such strings if needed.
	fn write(&self, text: &str, kind: FormatterOutputTextKind);

	/// Writes a prefix
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `text`: Prefix text
	/// - `prefix`: Prefix
	#[inline]
	fn write_prefix(&self, instruction: &Instruction, text: &str, prefix: PrefixKind) {
		self.write(text, FormatterOutputTextKind::Prefix);
	}

	/// Writes a mnemonic (see [`Instruction::mnemonic()`])
	///
	/// [`Instruction::mnemonic()`]: struct.Instruction.html#method.mnemonic
	///
	/// - `instruction`: Instruction
	/// - `text`: Mnemonic text
	#[inline]
	fn write_mnemonic(&self, instruction: &Instruction, text: &str) {
		self.write(text, FormatterOutputTextKind::Mnemonic);
	}

	/// Writes a number
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `operand`: Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
	/// - `instruction_operand`: Instruction operand number, 0-based, or -1 if it's an operand created by the formatter.
	/// - `text`: Number text
	/// - `value`: Value
	/// - `number_kind`: Number kind
	/// - `kind`: Text kind
	#[inline]
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::too_many_arguments))]
	fn write_number(
		&self, instruction: &Instruction, operand: u32, instruction_operand: i32, text: &str, value: u64, number_kind: NumberKind,
		kind: FormatterOutputTextKind,
	) {
		self.write(text, kind);
	}

	/// Writes a decorator
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `operand`: Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
	/// - `instruction_operand`: Instruction operand number, 0-based, or -1 if it's an operand created by the formatter.
	/// - `text`: Decorator text
	/// - `decorator`: Decorator
	#[inline]
	fn write_decorator(&self, instruction: &Instruction, operand: u32, instruction_operand: i32, text: &str, decorator: DecoratorKind) {
		self.write(text, FormatterOutputTextKind::Decorator);
	}

	/// Writes a register
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `operand`: Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
	/// - `instruction_operand`: Instruction operand number, 0-based, or -1 if it's an operand created by the formatter.
	/// - `text`: Register text
	/// - `register`: Register
	#[inline]
	fn write_register(&self, instruction: &Instruction, operand: u32, instruction_operand: i32, text: &str, register: Register) {
		self.write(text, FormatterOutputTextKind::Register);
	}

	/// Writes a symbol
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `operand`: Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
	/// - `instruction_operand`: Instruction operand number, 0-based, or -1 if it's an operand created by the formatter.
	/// - `address`: Address
	/// - `symbol`: Symbol
	#[inline]
	fn write_symbol(&self, instruction: &Instruction, operand: u32, instruction_operand: i32, address: u64, symbol: &SymbolResult) {
		FormatterOutputMethods::write_text(self, &symbol.text)
	}
}

struct FormatterOutputMethods;
impl FormatterOutputMethods {
	pub(crate) fn write_text<T: FormatterOutput + ?Sized>(this: &T, text: &TextInfo) {
		//TODO:
	}
}

trait Formatter {
	#[cfg_attr(has_must_use, must_use)]
	fn options(&self) -> &FormatterOptions;
	#[cfg_attr(has_must_use, must_use)]
	fn options_mut(&mut self) -> &mut FormatterOptions;
	#[inline]
	fn format_mnemonic(&self, instruction: &Instruction, output: &FormatterOutput) {
		self.format_mnemonic_options(instruction, output, FormatMnemonicOptions::NONE);
	}
	fn format_mnemonic_options(&self, instruction: &Instruction, output: &FormatterOutput, options: u32);
	#[cfg_attr(has_must_use, must_use)]
	fn operand_count(&self, instruction: &Instruction) -> u32;
	#[cfg(feature = "instr_info")]
	fn op_access(&self, instruction: &Instruction, operand: u32) -> Option<OpAccess>;
	#[cfg_attr(has_must_use, must_use)]
	fn instruction_operand(&self, instruction: &Instruction, operand: u32) -> u32;
	#[cfg_attr(has_must_use, must_use)]
	fn formatter_operand(&self, instruction: &Instruction, instruction_operand: i32) -> u32;
	fn format_operand(&self, instruction: &Instruction, output: &FormatterOutput, operand: u32);
	fn format_operand_separator(&self, instruction: &Instruction, output: &FormatterOutput);
	fn format_all_operands(&self, instruction: &Instruction, output: &FormatterOutput);
	fn format(&self, instruction: &Instruction, output: &FormatterOutput);
	#[cfg_attr(has_must_use, must_use)]
	fn format_register(&self, register: Register) -> &'static str;
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_i8(&self, value: i8) -> String {
		self.format_i8_options(value, &NumberFormattingOptions::with_immediate(self.options()))
	}
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_i16(&self, value: i16) -> String {
		self.format_i16_options(value, &NumberFormattingOptions::with_immediate(self.options()))
	}
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_i32(&self, value: i32) -> String {
		self.format_i32_options(value, &NumberFormattingOptions::with_immediate(self.options()))
	}
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_i64(&self, value: i64) -> String {
		self.format_i64_options(value, &NumberFormattingOptions::with_immediate(self.options()))
	}
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_u8(&self, value: u8) -> String {
		self.format_u8_options(value, &NumberFormattingOptions::with_immediate(self.options()))
	}
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_u16(&self, value: u16) -> String {
		self.format_u16_options(value, &NumberFormattingOptions::with_immediate(self.options()))
	}
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_u32(&self, value: u32) -> String {
		self.format_u32_options(value, &NumberFormattingOptions::with_immediate(self.options()))
	}
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn format_u64(&self, value: u64) -> String {
		self.format_u64_options(value, &NumberFormattingOptions::with_immediate(self.options()))
	}
	#[cfg_attr(has_must_use, must_use)]
	fn format_i8_options(&self, value: i8, options: &NumberFormattingOptions) -> String;
	#[cfg_attr(has_must_use, must_use)]
	fn format_i16_options(&self, value: i16, options: &NumberFormattingOptions) -> String;
	#[cfg_attr(has_must_use, must_use)]
	fn format_i32_options(&self, value: i32, options: &NumberFormattingOptions) -> String;
	#[cfg_attr(has_must_use, must_use)]
	fn format_i64_options(&self, value: i64, options: &NumberFormattingOptions) -> String;
	#[cfg_attr(has_must_use, must_use)]
	fn format_u8_options(&self, value: u8, options: &NumberFormattingOptions) -> String;
	#[cfg_attr(has_must_use, must_use)]
	fn format_u16_options(&self, value: u16, options: &NumberFormattingOptions) -> String;
	#[cfg_attr(has_must_use, must_use)]
	fn format_u32_options(&self, value: u32, options: &NumberFormattingOptions) -> String;
	#[cfg_attr(has_must_use, must_use)]
	fn format_u64_options(&self, value: u64, options: &NumberFormattingOptions) -> String;
}
