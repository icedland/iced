// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod enums;
mod enums_shared;
#[cfg(feature = "fast_fmt")]
mod fast;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod fmt_consts;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod fmt_opt_provider;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod fmt_opts;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod fmt_utils;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
mod fmt_utils_all;
#[cfg(feature = "gas")]
mod gas;
#[cfg(feature = "intel")]
mod intel;
#[cfg(feature = "masm")]
mod masm;
#[cfg(feature = "nasm")]
mod nasm;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod num_fmt;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod num_fmt_opts;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod pseudo_ops;
mod regs_tbl;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod regs_tbl_ls;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod string_output;
mod strings_data;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod strings_tbl;
mod symres;
#[cfg(test)]
pub(crate) mod tests;

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub use crate::formatter::enums::*;
pub use crate::formatter::enums_shared::*;
#[cfg(feature = "fast_fmt")]
pub use crate::formatter::fast::*;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub use crate::formatter::fmt_opt_provider::*;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub use crate::formatter::fmt_opts::*;
#[cfg(feature = "gas")]
pub use crate::formatter::gas::*;
#[cfg(feature = "intel")]
pub use crate::formatter::intel::*;
#[cfg(feature = "masm")]
pub use crate::formatter::masm::*;
#[cfg(feature = "nasm")]
pub use crate::formatter::nasm::*;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
use crate::formatter::num_fmt::NumberFormatter;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub use crate::formatter::num_fmt_opts::*;
pub use crate::formatter::symres::*;
use crate::*;
use alloc::string::String;
use alloc::vec::Vec;

#[cfg(any(feature = "gas", feature = "intel", feature = "masm"))]
#[allow(deprecated)]
const REGISTER_ST: Register = Register::DontUse0;

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
fn r_to_r16(reg: Register) -> Register {
	if Register::EAX <= reg && reg <= Register::R15 {
		Register::try_from((((reg as u32 - Register::AX as u32) & 0xF) + Register::AX as u32) as usize).unwrap_or(reg)
	} else {
		reg
	}
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
fn r64_to_r32(reg: Register) -> Register {
	if Register::RAX <= reg && reg <= Register::R15 {
		Register::try_from((reg as u32 - Register::RAX as u32 + Register::EAX as u32) as usize).unwrap_or(reg)
	} else {
		reg
	}
}

#[derive(Debug, Default, Clone)]
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
struct FormatterString {
	lower: String,
	upper: String,
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
impl FormatterString {
	#[must_use]
	fn new(lower: String) -> Self {
		debug_assert_eq!(lower.to_lowercase(), lower);
		Self { upper: lower.to_uppercase(), lower }
	}

	#[must_use]
	fn with_strings(strings: Vec<String>) -> Vec<Self> {
		strings.into_iter().map(FormatterString::new).collect()
	}

	#[must_use]
	fn new_str(lower: &str) -> Self {
		debug_assert_eq!(lower.to_lowercase(), lower);
		Self { lower: String::from(lower), upper: lower.to_uppercase() }
	}

	#[must_use]
	#[inline]
	fn len(&self) -> usize {
		self.lower.len()
	}

	#[cfg(any(feature = "gas", feature = "intel", feature = "nasm"))]
	#[must_use]
	#[inline]
	fn is_default(&self) -> bool {
		self.lower.is_empty()
	}

	#[must_use]
	#[inline]
	fn get(&self, upper: bool) -> &str {
		if upper {
			&self.upper
		} else {
			&self.lower
		}
	}
}

/// Used by a [`Formatter`] to write all text. `String` also implements this trait.
///
/// The only method that must be implemented is [`write()`], all other methods call it if they're not overridden.
///
/// [`Formatter`]: trait.Formatter.html
/// [`write()`]: #tymethod.write
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub trait FormatterOutput {
	/// Writes text and text kind
	///
	/// # Arguments
	///
	/// - `text`: Text
	/// - `kind`: Text kind
	fn write(&mut self, text: &str, kind: FormatterTextKind);

	/// Writes a prefix
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `text`: Prefix text
	/// - `prefix`: Prefix
	#[inline]
	#[allow(unused_variables)]
	fn write_prefix(&mut self, instruction: &Instruction, text: &str, prefix: PrefixKind) {
		self.write(text, FormatterTextKind::Prefix);
	}

	/// Writes a mnemonic (see [`Instruction::mnemonic()`])
	///
	/// [`Instruction::mnemonic()`]: struct.Instruction.html#method.mnemonic
	///
	/// - `instruction`: Instruction
	/// - `text`: Mnemonic text
	#[inline]
	#[allow(unused_variables)]
	fn write_mnemonic(&mut self, instruction: &Instruction, text: &str) {
		self.write(text, FormatterTextKind::Mnemonic);
	}

	/// Writes a number
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `operand`: Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
	/// - `instruction_operand`: Instruction operand number, 0-based, or `None` if it's an operand created by the formatter.
	/// - `text`: Number text
	/// - `value`: Value
	/// - `number_kind`: Number kind
	/// - `kind`: Text kind
	#[inline]
	#[allow(clippy::too_many_arguments)]
	#[allow(unused_variables)]
	fn write_number(
		&mut self, instruction: &Instruction, operand: u32, instruction_operand: Option<u32>, text: &str, value: u64, number_kind: NumberKind,
		kind: FormatterTextKind,
	) {
		self.write(text, kind);
	}

	/// Writes a decorator
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `operand`: Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
	/// - `instruction_operand`: Instruction operand number, 0-based, or `None` if it's an operand created by the formatter.
	/// - `text`: Decorator text
	/// - `decorator`: Decorator
	#[inline]
	#[allow(unused_variables)]
	fn write_decorator(&mut self, instruction: &Instruction, operand: u32, instruction_operand: Option<u32>, text: &str, decorator: DecoratorKind) {
		self.write(text, FormatterTextKind::Decorator);
	}

	/// Writes a register
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `operand`: Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
	/// - `instruction_operand`: Instruction operand number, 0-based, or `None` if it's an operand created by the formatter.
	/// - `text`: Register text
	/// - `register`: Register
	#[inline]
	#[allow(unused_variables)]
	fn write_register(&mut self, instruction: &Instruction, operand: u32, instruction_operand: Option<u32>, text: &str, register: Register) {
		self.write(text, FormatterTextKind::Register);
	}

	/// Writes a symbol
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `operand`: Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
	/// - `instruction_operand`: Instruction operand number, 0-based, or `None` if it's an operand created by the formatter.
	/// - `address`: Address
	/// - `symbol`: Symbol
	#[inline]
	#[allow(unused_variables)]
	fn write_symbol(&mut self, instruction: &Instruction, operand: u32, instruction_operand: Option<u32>, address: u64, symbol: &SymbolResult<'_>) {
		match symbol.text {
			SymResTextInfo::Text(ref part) => {
				let s = match &part.text {
					&SymResString::Str(s) => s,
					SymResString::String(s) => s.as_str(),
				};
				self.write(s, part.color);
			}

			SymResTextInfo::TextVec(v) => {
				for part in v {
					let s = match &part.text {
						&SymResString::Str(s) => s,
						SymResString::String(s) => s.as_str(),
					};
					self.write(s, part.color);
				}
			}
		}
	}
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
struct FormatterOutputMethods;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
impl FormatterOutputMethods {
	#[allow(clippy::too_many_arguments)]
	fn write1(
		output: &mut dyn FormatterOutput, instruction: &Instruction, operand: u32, instruction_operand: Option<u32>, options: &FormatterOptions,
		number_formatter: &mut NumberFormatter, number_options: &NumberFormattingOptions<'_>, address: u64, symbol: &SymbolResult<'_>,
		show_symbol_address: bool,
	) {
		FormatterOutputMethods::write2(
			output,
			instruction,
			operand,
			instruction_operand,
			options,
			number_formatter,
			number_options,
			address,
			symbol,
			show_symbol_address,
			true,
			false,
		);
	}

	#[allow(clippy::too_many_arguments)]
	fn write2(
		output: &mut dyn FormatterOutput, instruction: &Instruction, operand: u32, instruction_operand: Option<u32>, options: &FormatterOptions,
		number_formatter: &mut NumberFormatter, number_options: &NumberFormattingOptions<'_>, address: u64, symbol: &SymbolResult<'_>,
		show_symbol_address: bool, write_minus_if_signed: bool, spaces_between_op: bool,
	) {
		let mut displ = address.wrapping_sub(symbol.address) as i64;
		if (symbol.flags & SymbolFlags::SIGNED) != 0 {
			if write_minus_if_signed {
				output.write("-", FormatterTextKind::Operator);
			}
			displ = displ.wrapping_neg();
		}
		output.write_symbol(instruction, operand, instruction_operand, address, symbol);
		let mut number_kind: NumberKind;
		if displ != 0 {
			if spaces_between_op {
				output.write(" ", FormatterTextKind::Text);
			}
			let orig_displ = displ as u64;
			if displ < 0 {
				output.write("-", FormatterTextKind::Operator);
				displ = displ.wrapping_neg();
				if displ <= i8::MAX as i64 + 1 {
					number_kind = NumberKind::Int8;
				} else if displ <= i16::MAX as i64 + 1 {
					number_kind = NumberKind::Int16;
				} else if displ <= i32::MAX as i64 + 1 {
					number_kind = NumberKind::Int32;
				} else {
					number_kind = NumberKind::Int64;
				}
			} else {
				output.write("+", FormatterTextKind::Operator);
				if displ <= i8::MAX as i64 {
					number_kind = NumberKind::Int8;
				} else if displ <= i16::MAX as i64 {
					number_kind = NumberKind::Int16;
				} else if displ <= i32::MAX as i64 {
					number_kind = NumberKind::Int32;
				} else {
					number_kind = NumberKind::Int64;
				}
			}
			if spaces_between_op {
				output.write(" ", FormatterTextKind::Text);
			}
			let s = number_formatter.format_u64_zeros(options, number_options, displ as u64, false);
			output.write_number(instruction, operand, instruction_operand, s, orig_displ, number_kind, FormatterTextKind::Number);
		}
		if show_symbol_address {
			output.write(" ", FormatterTextKind::Text);
			output.write("(", FormatterTextKind::Punctuation);
			let s = if address <= u16::MAX as u64 {
				number_kind = NumberKind::UInt16;
				number_formatter.format_u16_zeros(options, number_options, address as u16, true)
			} else if address <= u32::MAX as u64 {
				number_kind = NumberKind::UInt32;
				number_formatter.format_u32_zeros(options, number_options, address as u32, true)
			} else {
				number_kind = NumberKind::UInt64;
				number_formatter.format_u64_zeros(options, number_options, address, true)
			};
			output.write_number(instruction, operand, instruction_operand, s, address, number_kind, FormatterTextKind::Number);
			output.write(")", FormatterTextKind::Punctuation);
		}
	}
}

/// Formats instructions
///
/// This trait is sealed and cannot be implemented by your own types.
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
pub trait Formatter: private::Sealed {
	/// Formats the whole instruction: prefixes, mnemonic, operands
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `output`: Output, eg. a `String`
	fn format(&mut self, instruction: &Instruction, output: &mut dyn FormatterOutput);

	/// Gets the formatter options (immutable)
	#[must_use]
	fn options(&self) -> &FormatterOptions;

	/// Gets the formatter options (mutable)
	#[must_use]
	fn options_mut(&mut self) -> &mut FormatterOptions;

	/// Formats the mnemonic and any prefixes
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `output`: Output, eg. a `String`
	#[inline]
	fn format_mnemonic(&mut self, instruction: &Instruction, output: &mut dyn FormatterOutput) {
		self.format_mnemonic_options(instruction, output, FormatMnemonicOptions::NONE);
	}

	/// Formats the mnemonic and/or any prefixes
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `output`: Output, eg. a `String`
	/// - `options`: Options, see [`FormatMnemonicOptions`]
	///
	/// [`FormatMnemonicOptions`]: struct.FormatMnemonicOptions.html
	fn format_mnemonic_options(&mut self, instruction: &Instruction, output: &mut dyn FormatterOutput, options: u32);

	/// Gets the number of operands that will be formatted. A formatter can add and remove operands
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	#[must_use]
	fn operand_count(&mut self, instruction: &Instruction) -> u32;

	/// Returns the operand access but only if it's an operand added by the formatter. If it's an
	/// operand that is part of [`Instruction`], you should call eg. [`InstructionInfoFactory::info()`].
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `operand`: Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand. See [`operand_count()`]
	///
	/// # Errors
	///
	/// This fails if `operand` is invalid.
	///
	/// [`Instruction`]: struct.Instruction.html
	/// [`InstructionInfoFactory::info()`]: struct.InstructionInfoFactory.html#method.info
	/// [`operand_count()`]: #tymethod.operand_count
	#[cfg(feature = "instr_info")]
	fn op_access(&mut self, instruction: &Instruction, operand: u32) -> Result<Option<OpAccess>, IcedError>;

	/// Converts a formatter operand index to an instruction operand index. Returns `None` if it's an operand added by the formatter
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `operand`: Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand. See [`operand_count()`]
	///
	/// # Errors
	///
	/// This fails if `operand` is invalid.
	///
	/// [`operand_count()`]: #tymethod.operand_count
	fn get_instruction_operand(&mut self, instruction: &Instruction, operand: u32) -> Result<Option<u32>, IcedError>;

	/// Converts an instruction operand index to a formatter operand index. Returns `None` if the instruction operand isn't used by the formatter
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `instruction_operand`: Instruction operand
	///
	/// # Errors
	///
	/// This fails if `instruction_operand` is invalid.
	fn get_formatter_operand(&mut self, instruction: &Instruction, instruction_operand: u32) -> Result<Option<u32>, IcedError>;

	/// Formats an operand. This is a formatter operand and not necessarily a real instruction operand.
	/// A formatter can add and remove operands.
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `output`: Output, eg. a `String`
	/// - `operand`: Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand. See [`operand_count()`]
	///
	/// # Errors
	///
	/// This fails if `operand` is invalid.
	///
	/// [`operand_count()`]: #tymethod.operand_count
	fn format_operand(&mut self, instruction: &Instruction, output: &mut dyn FormatterOutput, operand: u32) -> Result<(), IcedError>;

	/// Formats an operand separator
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `output`: Output, eg. a `String`
	fn format_operand_separator(&mut self, instruction: &Instruction, output: &mut dyn FormatterOutput);

	/// Formats all operands
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `output`: Output, eg. a `String`
	fn format_all_operands(&mut self, instruction: &Instruction, output: &mut dyn FormatterOutput);

	/// Formats a register
	///
	/// # Arguments
	///
	/// - `register`: Register
	#[must_use]
	fn format_register(&mut self, register: Register) -> &str;

	/// Formats a `i8`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[must_use]
	fn format_i8(&mut self, value: i8) -> &str;

	/// Formats a `i16`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[must_use]
	fn format_i16(&mut self, value: i16) -> &str;

	/// Formats a `i32`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[must_use]
	fn format_i32(&mut self, value: i32) -> &str;

	/// Formats a `i64`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[must_use]
	fn format_i64(&mut self, value: i64) -> &str;

	/// Formats a `u8`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[must_use]
	fn format_u8(&mut self, value: u8) -> &str;

	/// Formats a `u16`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[must_use]
	fn format_u16(&mut self, value: u16) -> &str;

	/// Formats a `u32`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[must_use]
	fn format_u32(&mut self, value: u32) -> &str;

	/// Formats a `u64`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[must_use]
	fn format_u64(&mut self, value: u64) -> &str;

	/// Formats a `i8`
	///
	/// # Arguments
	///
	/// - `value`: Value
	/// - `number_options`: Options
	#[must_use]
	fn format_i8_options(&mut self, value: i8, number_options: &NumberFormattingOptions<'_>) -> &str;

	/// Formats a `i16`
	///
	/// # Arguments
	///
	/// - `value`: Value
	/// - `number_options`: Options
	#[must_use]
	fn format_i16_options(&mut self, value: i16, number_options: &NumberFormattingOptions<'_>) -> &str;

	/// Formats a `i32`
	///
	/// # Arguments
	///
	/// - `value`: Value
	/// - `number_options`: Options
	#[must_use]
	fn format_i32_options(&mut self, value: i32, number_options: &NumberFormattingOptions<'_>) -> &str;

	/// Formats a `i64`
	///
	/// # Arguments
	///
	/// - `value`: Value
	/// - `number_options`: Options
	#[must_use]
	fn format_i64_options(&mut self, value: i64, number_options: &NumberFormattingOptions<'_>) -> &str;

	/// Formats a `u8`
	///
	/// # Arguments
	///
	/// - `value`: Value
	/// - `number_options`: Options
	#[must_use]
	fn format_u8_options(&mut self, value: u8, number_options: &NumberFormattingOptions<'_>) -> &str;

	/// Formats a `u16`
	///
	/// # Arguments
	///
	/// - `value`: Value
	/// - `number_options`: Options
	#[must_use]
	fn format_u16_options(&mut self, value: u16, number_options: &NumberFormattingOptions<'_>) -> &str;

	/// Formats a `u32`
	///
	/// # Arguments
	///
	/// - `value`: Value
	/// - `number_options`: Options
	#[must_use]
	fn format_u32_options(&mut self, value: u32, number_options: &NumberFormattingOptions<'_>) -> &str;

	/// Formats a `u64`
	///
	/// # Arguments
	///
	/// - `value`: Value
	/// - `number_options`: Options
	#[must_use]
	fn format_u64_options(&mut self, value: u64, number_options: &NumberFormattingOptions<'_>) -> &str;
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
mod private {
	pub trait Sealed {}
	#[cfg(feature = "gas")]
	impl Sealed for crate::GasFormatter {}
	#[cfg(feature = "intel")]
	impl Sealed for crate::IntelFormatter {}
	#[cfg(feature = "masm")]
	impl Sealed for crate::MasmFormatter {}
	#[cfg(feature = "nasm")]
	impl Sealed for crate::NasmFormatter {}
}

#[allow(clippy::manual_map)] // It's wrong
fn to_owned<'a>(sym_res: Option<SymbolResult<'_>>, vec: &'a mut Vec<SymResTextPart<'a>>) -> Option<SymbolResult<'a>> {
	match sym_res {
		None => None,
		Some(sym_res) => Some(sym_res.to_owned(vec)),
	}
}

#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
fn get_mnemonic_cc<'a>(options: &FormatterOptions, cc_index: u32, mnemonics: &'a [FormatterString]) -> &'a FormatterString {
	use crate::iced_constants::IcedConstants;
	let index = match cc_index {
		// o
		0 => {
			debug_assert_eq!(mnemonics.len(), 1);
			0
		}
		// no
		1 => {
			debug_assert_eq!(mnemonics.len(), 1);
			0
		}
		// b, c, nae
		2 => {
			debug_assert_eq!(mnemonics.len(), IcedConstants::CC_B_ENUM_COUNT);
			options.cc_b() as usize
		}
		// ae, nb, nc
		3 => {
			debug_assert_eq!(mnemonics.len(), IcedConstants::CC_AE_ENUM_COUNT);
			options.cc_ae() as usize
		}
		// e, z
		4 => {
			debug_assert_eq!(mnemonics.len(), IcedConstants::CC_E_ENUM_COUNT);
			options.cc_e() as usize
		}
		// ne, nz
		5 => {
			debug_assert_eq!(mnemonics.len(), IcedConstants::CC_NE_ENUM_COUNT);
			options.cc_ne() as usize
		}
		// be, na
		6 => {
			debug_assert_eq!(mnemonics.len(), IcedConstants::CC_BE_ENUM_COUNT);
			options.cc_be() as usize
		}
		// a, nbe
		7 => {
			debug_assert_eq!(mnemonics.len(), IcedConstants::CC_A_ENUM_COUNT);
			options.cc_a() as usize
		}
		// s
		8 => {
			debug_assert_eq!(mnemonics.len(), 1);
			0
		}
		// ns
		9 => {
			debug_assert_eq!(mnemonics.len(), 1);
			0
		}
		// p, pe
		10 => {
			debug_assert_eq!(mnemonics.len(), IcedConstants::CC_P_ENUM_COUNT);
			options.cc_p() as usize
		}
		// np, po
		11 => {
			debug_assert_eq!(mnemonics.len(), IcedConstants::CC_NP_ENUM_COUNT);
			options.cc_np() as usize
		}
		// l, nge
		12 => {
			debug_assert_eq!(mnemonics.len(), IcedConstants::CC_L_ENUM_COUNT);
			options.cc_l() as usize
		}
		// ge, nl
		13 => {
			debug_assert_eq!(mnemonics.len(), IcedConstants::CC_GE_ENUM_COUNT);
			options.cc_ge() as usize
		}
		// le, ng
		14 => {
			debug_assert_eq!(mnemonics.len(), IcedConstants::CC_LE_ENUM_COUNT);
			options.cc_le() as usize
		}
		// g, nle
		15 => {
			debug_assert_eq!(mnemonics.len(), IcedConstants::CC_G_ENUM_COUNT);
			options.cc_g() as usize
		}
		_ => unreachable!(),
	};
	debug_assert!(index < mnemonics.len());
	&mnemonics[index]
}
