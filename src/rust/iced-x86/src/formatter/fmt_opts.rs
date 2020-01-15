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

use super::enums::*;
#[cfg(not(feature = "std"))]
use alloc::string::String;

struct Flags1;
impl Flags1 {
	pub(crate) const UPPER_CASE_PREFIXES: u32 = 0x0000_0001;
	pub(crate) const UPPER_CASE_MNEMONICS: u32 = 0x0000_0002;
	pub(crate) const UPPER_CASE_REGISTERS: u32 = 0x0000_0004;
	pub(crate) const UPPER_CASE_KEYWORDS: u32 = 0x0000_0008;
	pub(crate) const UPPER_CASE_DECORATORS: u32 = 0x0000_0010;
	pub(crate) const UPPER_CASE_ALL: u32 = 0x0000_0020;
	pub(crate) const SPACE_AFTER_OPERAND_SEPARATOR: u32 = 0x0000_0040;
	pub(crate) const SPACE_AFTER_MEMORY_BRACKET: u32 = 0x0000_0080;
	pub(crate) const SPACE_BETWEEN_MEMORY_ADD_OPERATORS: u32 = 0x0000_0100;
	pub(crate) const SPACE_BETWEEN_MEMORY_MUL_OPERATORS: u32 = 0x0000_0200;
	pub(crate) const SCALE_BEFORE_INDEX: u32 = 0x0000_0400;
	pub(crate) const ALWAYS_SHOW_SCALE: u32 = 0x0000_0800;
	pub(crate) const ALWAYS_SHOW_SEGMENT_REGISTER: u32 = 0x0000_1000;
	pub(crate) const SHOW_ZERO_DISPLACEMENTS: u32 = 0x0000_2000;
	pub(crate) const LEADING_ZEROES: u32 = 0x0000_4000;
	pub(crate) const UPPER_CASE_HEX: u32 = 0x0000_8000;
	pub(crate) const SMALL_HEX_NUMBERS_IN_DECIMAL: u32 = 0x0001_0000;
	pub(crate) const ADD_LEADING_ZERO_TO_HEX_NUMBERS: u32 = 0x0002_0000;
	pub(crate) const BRANCH_LEADING_ZEROES: u32 = 0x0004_0000;
	pub(crate) const SIGNED_IMMEDIATE_OPERANDS: u32 = 0x0008_0000;
	pub(crate) const SIGNED_MEMORY_DISPLACEMENTS: u32 = 0x0010_0000;
	pub(crate) const SIGN_EXTEND_MEMORY_DISPLACEMENTS: u32 = 0x0020_0000;
	pub(crate) const RIP_RELATIVE_ADDRESSES: u32 = 0x0040_0000;
	pub(crate) const SHOW_BRANCH_SIZE: u32 = 0x0080_0000;
	pub(crate) const USE_PSEUDO_OPS: u32 = 0x0100_0000;
	pub(crate) const SHOW_SYMBOL_ADDRESS: u32 = 0x0200_0000;
	pub(crate) const GAS_NAKED_REGISTERS: u32 = 0x0400_0000;
	pub(crate) const GAS_SHOW_MNEMONIC_SIZE_SUFFIX: u32 = 0x0800_0000;
	pub(crate) const GAS_SPACE_AFTER_MEMORY_OPERAND_COMMA: u32 = 0x1000_0000;
	pub(crate) const MASM_ADD_DS_PREFIX32: u32 = 0x2000_0000;
	pub(crate) const MASM_SYMBOL_DISPL_IN_BRACKETS: u32 = 0x4000_0000;
	pub(crate) const MASM_DISPL_IN_BRACKETS: u32 = 0x8000_0000;
}

struct Flags2;
impl Flags2 {
	pub(crate) const NASM_SHOW_SIGN_EXTENDED_IMMEDIATE_SIZE: u32 = 0x0000_0001;
}

/// Formatter options
#[derive(Debug, Clone, Eq, PartialEq, Hash)]
pub struct FormatterOptions {
	hex_prefix: String,
	hex_suffix: String,
	decimal_prefix: String,
	decimal_suffix: String,
	octal_prefix: String,
	octal_suffix: String,
	binary_prefix: String,
	binary_suffix: String,
	digit_separator: String,
	hex_digit_group_size: u32,
	decimal_digit_group_size: u32,
	octal_digit_group_size: u32,
	binary_digit_group_size: u32,
	options1: u32,
	options2: u32,
	first_operand_char_index: u32,
	tab_size: u32,
	number_base: NumberBase,
	memory_size_options: MemorySizeOptions,
}

impl FormatterOptions {
	/// Creates default formatter options
	#[cfg_attr(has_must_use, must_use)]
	#[cfg_attr(feature = "cargo-clippy", allow(clippy::missing_inline_in_public_items))]
	pub fn new() -> Self {
		Self {
			hex_prefix: String::default(),
			hex_suffix: String::default(),
			decimal_prefix: String::default(),
			decimal_suffix: String::default(),
			octal_prefix: String::default(),
			octal_suffix: String::default(),
			binary_prefix: String::default(),
			binary_suffix: String::default(),
			digit_separator: String::default(),
			hex_digit_group_size: 4,
			decimal_digit_group_size: 3,
			octal_digit_group_size: 4,
			binary_digit_group_size: 4,
			options1: Flags1::UPPER_CASE_HEX
				| Flags1::SMALL_HEX_NUMBERS_IN_DECIMAL
				| Flags1::ADD_LEADING_ZERO_TO_HEX_NUMBERS
				| Flags1::BRANCH_LEADING_ZEROES
				| Flags1::SIGNED_MEMORY_DISPLACEMENTS
				| Flags1::SHOW_BRANCH_SIZE
				| Flags1::USE_PSEUDO_OPS
				| Flags1::MASM_ADD_DS_PREFIX32
				| Flags1::MASM_SYMBOL_DISPL_IN_BRACKETS
				| Flags1::MASM_DISPL_IN_BRACKETS,
			options2: 0,
			first_operand_char_index: 0,
			tab_size: 0,
			number_base: NumberBase::Hexadecimal,
			memory_size_options: MemorySizeOptions::Default,
		}
	}

	/// Creates default gas (AT&T) formatter options
	#[cfg(any(feature = "gas_formatter", feature = "all_formatters"))]
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_gas() -> Self {
		let mut options = FormatterOptions::new();
		options.set_hex_prefix(String::from("0x"));
		options.set_octal_prefix(String::from("0"));
		options.set_binary_prefix(String::from("0b"));
		options
	}

	/// Creates default Intel (XED) formatter options
	#[cfg(any(feature = "intel_formatter", feature = "all_formatters"))]
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_intel() -> Self {
		let mut options = FormatterOptions::new();
		options.set_hex_suffix(String::from("h"));
		options.set_octal_suffix(String::from("o"));
		options.set_binary_suffix(String::from("b"));
		options
	}

	/// Creates default masm formatter options
	#[cfg(any(feature = "masm_formatter", feature = "all_formatters"))]
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_masm() -> Self {
		let mut options = FormatterOptions::new();
		options.set_hex_suffix(String::from("h"));
		options.set_octal_suffix(String::from("o"));
		options.set_binary_suffix(String::from("b"));
		options
	}

	/// Creates default nasm formatter options
	#[cfg(any(feature = "nasm_formatter", feature = "all_formatters"))]
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_nasm() -> Self {
		let mut options = FormatterOptions::new();
		options.set_hex_suffix(String::from("h"));
		options.set_octal_suffix(String::from("o"));
		options.set_binary_suffix(String::from("b"));
		options
	}

	/// Prefixes are upper cased
	///
	/// - Default: `false`
	/// - `true`: `REP stosd`
	/// - `false`: `rep stosd`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn upper_case_prefixes(&self) -> bool {
		(self.options1 & Flags1::UPPER_CASE_PREFIXES) != 0
	}

	/// Prefixes are upper cased
	///
	/// - Default: `false`
	/// - `true`: `REP stosd`
	/// - `false`: `rep stosd`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_upper_case_prefixes(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::UPPER_CASE_PREFIXES;
		} else {
			self.options1 &= !Flags1::UPPER_CASE_PREFIXES;
		}
	}

	/// Mnemonics are upper cased
	///
	/// - Default: `false`
	/// - `true`: `MOV rcx,rax`
	/// - `false`: `mov rcx,rax`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn upper_case_mnemonics(&self) -> bool {
		(self.options1 & Flags1::UPPER_CASE_MNEMONICS) != 0
	}

	/// Mnemonics are upper cased
	///
	/// - Default: `false`
	/// - `true`: `MOV rcx,rax`
	/// - `false`: `mov rcx,rax`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_upper_case_mnemonics(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::UPPER_CASE_MNEMONICS;
		} else {
			self.options1 &= !Flags1::UPPER_CASE_MNEMONICS;
		}
	}

	/// Registers are upper cased
	///
	/// - Default: `false`
	/// - `true`: `mov RCX,[RAX+RDX*8]`
	/// - `false`: `mov rcx,[rax+rdx*8]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn upper_case_registers(&self) -> bool {
		(self.options1 & Flags1::UPPER_CASE_REGISTERS) != 0
	}

	/// Registers are upper cased
	///
	/// - Default: `false`
	/// - `true`: `mov RCX,[RAX+RDX*8]`
	/// - `false`: `mov rcx,[rax+rdx*8]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_upper_case_registers(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::UPPER_CASE_REGISTERS;
		} else {
			self.options1 &= !Flags1::UPPER_CASE_REGISTERS;
		}
	}

	/// Keywords are upper cased (eg. `BYTE PTR`, `SHORT`)
	///
	/// - Default: `false`
	/// - `true`: `mov BYTE PTR [rcx],12h`
	/// - `false`: `mov byte ptr [rcx],12h`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn upper_case_keywords(&self) -> bool {
		(self.options1 & Flags1::UPPER_CASE_KEYWORDS) != 0
	}

	/// Keywords are upper cased (eg. `BYTE PTR`, `SHORT`)
	///
	/// - Default: `false`
	/// - `true`: `mov BYTE PTR [rcx],12h`
	/// - `false`: `mov byte ptr [rcx],12h`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_upper_case_keywords(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::UPPER_CASE_KEYWORDS;
		} else {
			self.options1 &= !Flags1::UPPER_CASE_KEYWORDS;
		}
	}

	/// Upper case decorators, eg. `{z}`, `{sae}`, `{rd-sae}` (but not op mask registers: `{k1}`)
	///
	/// - Default: `false`
	/// - `true`: `vunpcklps xmm2{k5}{Z},xmm6,dword bcst [rax+4]`
	/// - `false`: `vunpcklps xmm2{k5}{z},xmm6,dword bcst [rax+4]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn upper_case_decorators(&self) -> bool {
		(self.options1 & Flags1::UPPER_CASE_DECORATORS) != 0
	}

	/// Upper case decorators, eg. `{z}`, `{sae}`, `{rd-sae}` (but not op mask registers: `{k1}`)
	///
	/// - Default: `false`
	/// - `true`: `vunpcklps xmm2{k5}{Z},xmm6,dword bcst [rax+4]`
	/// - `false`: `vunpcklps xmm2{k5}{z},xmm6,dword bcst [rax+4]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_upper_case_decorators(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::UPPER_CASE_DECORATORS;
		} else {
			self.options1 &= !Flags1::UPPER_CASE_DECORATORS;
		}
	}

	/// Everything is upper cased, except numbers and their prefixes/suffixes
	///
	/// - Default: `false`
	/// - `true`: `MOV EAX,GS:[RCX*4+0ffh]`
	/// - `false`: `mov eax,gs:[rcx*4+0ffh]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn upper_case_all(&self) -> bool {
		(self.options1 & Flags1::UPPER_CASE_ALL) != 0
	}

	/// Everything is upper cased, except numbers and their prefixes/suffixes
	///
	/// - Default: `false`
	/// - `true`: `MOV EAX,GS:[RCX*4+0ffh]`
	/// - `false`: `mov eax,gs:[rcx*4+0ffh]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_upper_case_all(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::UPPER_CASE_ALL;
		} else {
			self.options1 &= !Flags1::UPPER_CASE_ALL;
		}
	}

	/// Character index (0-based) where the first operand is formatted. Can be set to 0 to format it immediately after the mnemonic.
	/// At least one space or tab is always added between the mnemonic and the first operand.
	///
	/// - Default: `0`
	/// - `0`: `mov•rcx,rbp`
	/// - `8`: `mov•••••rcx,rbp`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn first_operand_char_index(&self) -> u32 {
		self.first_operand_char_index
	}

	/// Character index (0-based) where the first operand is formatted. Can be set to 0 to format it immediately after the mnemonic.
	/// At least one space or tab is always added between the mnemonic and the first operand.
	///
	/// - Default: `0`
	/// - `0`: `mov•rcx,rbp`
	/// - `8`: `mov•••••rcx,rbp`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_first_operand_char_index(&mut self, value: u32) {
		self.first_operand_char_index = value
	}

	/// Size of a tab character or 0 to use spaces
	///
	/// - Default: `0`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn tab_size(&self) -> u32 {
		self.tab_size
	}

	/// Size of a tab character or 0 to use spaces
	///
	/// - Default: `0`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_tab_size(&mut self, value: u32) {
		self.tab_size = value
	}

	/// Add a space after the operand separator
	///
	/// - Default: `false`
	/// - `true`: `mov rax, rcx`
	/// - `false`: `mov rax,rcx`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn space_after_operand_separator(&self) -> bool {
		(self.options1 & Flags1::SPACE_AFTER_OPERAND_SEPARATOR) != 0
	}

	/// Add a space after the operand separator
	///
	/// - Default: `false`
	/// - `true`: `mov rax, rcx`
	/// - `false`: `mov rax,rcx`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_space_after_operand_separator(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::SPACE_AFTER_OPERAND_SEPARATOR;
		} else {
			self.options1 &= !Flags1::SPACE_AFTER_OPERAND_SEPARATOR;
		}
	}

	/// Add a space between the memory expression and the brackets
	///
	/// - Default: `false`
	/// - `true`: `mov eax,[ rcx+rdx ]`
	/// - `false`: `mov eax,[rcx+rdx]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn space_after_memory_bracket(&self) -> bool {
		(self.options1 & Flags1::SPACE_AFTER_MEMORY_BRACKET) != 0
	}

	/// Add a space between the memory expression and the brackets
	///
	/// - Default: `false`
	/// - `true`: `mov eax,[ rcx+rdx ]`
	/// - `false`: `mov eax,[rcx+rdx]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_space_after_memory_bracket(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::SPACE_AFTER_MEMORY_BRACKET;
		} else {
			self.options1 &= !Flags1::SPACE_AFTER_MEMORY_BRACKET;
		}
	}

	/// Add spaces between memory operand `+` and `-` operators
	///
	/// - Default: `false`
	/// - `true`: `mov eax,[rcx + rdx*8 - 80h]`
	/// - `false`: `mov eax,[rcx+rdx*8-80h]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn space_between_memory_add_operators(&self) -> bool {
		(self.options1 & Flags1::SPACE_BETWEEN_MEMORY_ADD_OPERATORS) != 0
	}

	/// Add spaces between memory operand `+` and `-` operators
	///
	/// - Default: `false`
	/// - `true`: `mov eax,[rcx + rdx*8 - 80h]`
	/// - `false`: `mov eax,[rcx+rdx*8-80h]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_space_between_memory_add_operators(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::SPACE_BETWEEN_MEMORY_ADD_OPERATORS;
		} else {
			self.options1 &= !Flags1::SPACE_BETWEEN_MEMORY_ADD_OPERATORS;
		}
	}

	/// Add spaces between memory operand `*` operator
	///
	/// - Default: `false`
	/// - `true`: `mov eax,[rcx+rdx * 8-80h]`
	/// - `false`: `mov eax,[rcx+rdx*8-80h]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn space_between_memory_mul_operators(&self) -> bool {
		(self.options1 & Flags1::SPACE_BETWEEN_MEMORY_MUL_OPERATORS) != 0
	}

	/// Add spaces between memory operand `*` operator
	///
	/// - Default: `false`
	/// - `true`: `mov eax,[rcx+rdx * 8-80h]`
	/// - `false`: `mov eax,[rcx+rdx*8-80h]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_space_between_memory_mul_operators(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::SPACE_BETWEEN_MEMORY_MUL_OPERATORS;
		} else {
			self.options1 &= !Flags1::SPACE_BETWEEN_MEMORY_MUL_OPERATORS;
		}
	}

	/// Show memory operand scale value before the index register
	///
	/// - Default: `false`
	/// - `true`: `mov eax,[8*rdx]`
	/// - `false`: `mov eax,[rdx*8]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn scale_before_index(&self) -> bool {
		(self.options1 & Flags1::SCALE_BEFORE_INDEX) != 0
	}

	/// Show memory operand scale value before the index register
	///
	/// - Default: `false`
	/// - `true`: `mov eax,[8*rdx]`
	/// - `false`: `mov eax,[rdx*8]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_scale_before_index(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::SCALE_BEFORE_INDEX;
		} else {
			self.options1 &= !Flags1::SCALE_BEFORE_INDEX;
		}
	}

	/// Always show the scale value even if it's `*1`
	///
	/// - Default: `false`
	/// - `true`: `mov eax,[rbx+rcx*1]`
	/// - `false`: `mov eax,[rbx+rcx]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn always_show_scale(&self) -> bool {
		(self.options1 & Flags1::ALWAYS_SHOW_SCALE) != 0
	}

	/// Always show the scale value even if it's `*1`
	///
	/// - Default: `false`
	/// - `true`: `mov eax,[rbx+rcx*1]`
	/// - `false`: `mov eax,[rbx+rcx]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_always_show_scale(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::ALWAYS_SHOW_SCALE;
		} else {
			self.options1 &= !Flags1::ALWAYS_SHOW_SCALE;
		}
	}

	/// Always show the effective segment register. If the option is `false`, only show the segment register if
	/// there's a segment override prefix.
	///
	/// - Default: `false`
	/// - `true`: `mov eax,ds:[ecx]`
	/// - `false`: `mov eax,[ecx]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn always_show_segment_register(&self) -> bool {
		(self.options1 & Flags1::ALWAYS_SHOW_SEGMENT_REGISTER) != 0
	}

	/// Always show the effective segment register. If the option is `false`, only show the segment register if
	/// there's a segment override prefix.
	///
	/// - Default: `false`
	/// - `true`: `mov eax,ds:[ecx]`
	/// - `false`: `mov eax,[ecx]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_always_show_segment_register(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::ALWAYS_SHOW_SEGMENT_REGISTER;
		} else {
			self.options1 &= !Flags1::ALWAYS_SHOW_SEGMENT_REGISTER;
		}
	}

	/// Show zero displacements
	///
	/// - Default: `false`
	/// - `true`: `mov eax,[rcx*2+0]`
	/// - `false`: `mov eax,[rcx*2]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn show_zero_displacements(&self) -> bool {
		(self.options1 & Flags1::SHOW_ZERO_DISPLACEMENTS) != 0
	}

	/// Show zero displacements
	///
	/// - Default: `false`
	/// - `true`: `mov eax,[rcx*2+0]`
	/// - `false`: `mov eax,[rcx*2]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_show_zero_displacements(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::SHOW_ZERO_DISPLACEMENTS;
		} else {
			self.options1 &= !Flags1::SHOW_ZERO_DISPLACEMENTS;
		}
	}

	/// Hex number prefix or an empty string, eg. `"0x"`
	///
	/// - Default: `""` (masm/nasm/intel), `"0x"` (gas)
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn hex_prefix(&self) -> &str {
		&self.hex_prefix
	}

	/// Hex number prefix or an empty string, eg. `"0x"`
	///
	/// - Default: `""` (masm/nasm/intel), `"0x"` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_hex_prefix(&mut self, value: String) {
		self.hex_prefix = value
	}

	/// Hex number suffix or an empty string, eg. `"h"`
	///
	/// - Default: `"h"` (masm/nasm/intel), `""` (gas)
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn hex_suffix(&self) -> &str {
		&self.hex_suffix
	}

	/// Hex number suffix or an empty string, eg. `"h"`
	///
	/// - Default: `"h"` (masm/nasm/intel), `""` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_hex_suffix(&mut self, value: String) {
		self.hex_suffix = value
	}

	/// Size of a digit group, see also [`digit_separator()`]
	///
	/// [`digit_separator()`]: #method.digit_separator
	///
	/// - Default: `4`
	/// - `0`: `0x12345678`
	/// - `4`: `0x1234_5678`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn hex_digit_group_size(&self) -> u32 {
		self.hex_digit_group_size
	}

	/// Size of a digit group, see also [`digit_separator()`]
	///
	/// [`digit_separator()`]: #method.digit_separator
	///
	/// - Default: `4`
	/// - `0`: `0x12345678`
	/// - `4`: `0x1234_5678`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_hex_digit_group_size(&mut self, value: u32) {
		self.hex_digit_group_size = value
	}

	/// Decimal number prefix or an empty string
	///
	/// - Default: `""`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn decimal_prefix(&self) -> &str {
		&self.decimal_prefix
	}

	/// Decimal number prefix or an empty string
	///
	/// - Default: `""`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_decimal_prefix(&mut self, value: String) {
		self.decimal_prefix = value
	}

	/// Decimal number suffix or an empty string
	///
	/// - Default: `""`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn decimal_suffix(&self) -> &str {
		&self.decimal_suffix
	}

	/// Decimal number suffix or an empty string
	///
	/// - Default: `""`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_decimal_suffix(&mut self, value: String) {
		self.decimal_suffix = value
	}

	/// Size of a digit group, see also [`digit_separator()`]
	///
	/// [`digit_separator()`]: #method.digit_separator
	///
	/// - Default: `3`
	/// - `0`: `12345678`
	/// - `3`: `12_345_678`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn decimal_digit_group_size(&self) -> u32 {
		self.decimal_digit_group_size
	}

	/// Size of a digit group, see also [`digit_separator()`]
	///
	/// [`digit_separator()`]: #method.digit_separator
	///
	/// - Default: `3`
	/// - `0`: `12345678`
	/// - `3`: `12_345_678`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_decimal_digit_group_size(&mut self, value: u32) {
		self.decimal_digit_group_size = value
	}

	/// Octal number prefix or an empty string
	///
	/// - Default: `""` (masm/nasm/intel), `"0"` (gas)
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn octal_prefix(&self) -> &str {
		&self.octal_prefix
	}

	/// Octal number prefix or an empty string
	///
	/// - Default: `""` (masm/nasm/intel), `"0"` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_octal_prefix(&mut self, value: String) {
		self.octal_prefix = value
	}

	/// Octal number suffix or an empty string
	///
	/// - Default: `"o"` (masm/nasm/intel), `""` (gas)
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn octal_suffix(&self) -> &str {
		&self.octal_suffix
	}

	/// Octal number suffix or an empty string
	///
	/// - Default: `"o"` (masm/nasm/intel), `""` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_octal_suffix(&mut self, value: String) {
		self.octal_suffix = value
	}

	/// Size of a digit group, see also [`digit_separator()`]
	///
	/// [`digit_separator()`]: #method.digit_separator
	///
	/// - Default: `4`
	/// - `0`: `12345670`
	/// - `4`: `1234_5670`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn octal_digit_group_size(&self) -> u32 {
		self.octal_digit_group_size
	}

	/// Size of a digit group, see also [`digit_separator()`]
	///
	/// [`digit_separator()`]: #method.digit_separator
	///
	/// - Default: `4`
	/// - `0`: `12345670`
	/// - `4`: `1234_5670`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_octal_digit_group_size(&mut self, value: u32) {
		self.octal_digit_group_size = value
	}

	/// Binary number prefix or an empty string
	///
	/// - Default: `""` (masm/nasm/intel), `"0b"` (gas)
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn binary_prefix(&self) -> &str {
		&self.binary_prefix
	}

	/// Binary number prefix or an empty string
	///
	/// - Default: `""` (masm/nasm/intel), `"0b"` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_binary_prefix(&mut self, value: String) {
		self.binary_prefix = value
	}

	/// Binary number suffix or an empty string
	///
	/// - Default: `"b"` (masm/nasm/intel), `""` (gas)
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn binary_suffix(&self) -> &str {
		&self.binary_suffix
	}

	/// Binary number suffix or an empty string
	///
	/// - Default: `"b"` (masm/nasm/intel), `""` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_binary_suffix(&mut self, value: String) {
		self.binary_suffix = value
	}

	/// Size of a digit group, see also [`digit_separator()`]
	///
	/// [`digit_separator()`]: #method.digit_separator
	///
	/// - Default: `4`
	/// - `0`: `11010111`
	/// - `4`: `1101_0111`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn binary_digit_group_size(&self) -> u32 {
		self.binary_digit_group_size
	}

	/// Size of a digit group, see also [`digit_separator()`]
	///
	/// [`digit_separator()`]: #method.digit_separator
	///
	/// - Default: `4`
	/// - `0`: `11010111`
	/// - `4`: `1101_0111`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_binary_digit_group_size(&mut self, value: u32) {
		self.binary_digit_group_size = value
	}

	/// Digit separator or an empty string
	///
	/// - Default: `""`
	/// - `""`: `0x12345678`
	/// - `"_"`: `0x1234_5678`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn digit_separator(&self) -> &str {
		&self.digit_separator
	}

	/// Digit separator or an empty string
	///
	/// - Default: `""`
	/// - `""`: `0x12345678`
	/// - `"_"`: `0x1234_5678`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_digit_separator(&mut self, value: String) {
		self.digit_separator = value
	}

	/// Add leading zeroes to hexadecimal/octal/binary numbers.
	/// This option has no effect on branch targets, use [`branch_leading_zeroes`].
	///
	/// - Default: `true`
	/// - `true`: `0x0000000A`/`0000000Ah`
	/// - `false`: `0xA`/`0Ah`
	///
	/// [`branch_leading_zeroes`]: #structfield.branch_leading_zeroes
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn leading_zeroes(&self) -> bool {
		(self.options1 & Flags1::LEADING_ZEROES) != 0
	}

	/// Add leading zeroes to hexadecimal/octal/binary numbers.
	/// This option has no effect on branch targets, use [`branch_leading_zeroes()`].
	///
	/// - Default: `true`
	/// - `true`: `0x0000000A`/`0000000Ah`
	/// - `false`: `0xA`/`0Ah`
	///
	/// [`branch_leading_zeroes()`]: #method.branch_leading_zeroes
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_leading_zeroes(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::LEADING_ZEROES;
		} else {
			self.options1 &= !Flags1::LEADING_ZEROES;
		}
	}

	/// Use upper case hex digits
	///
	/// - Default: `true`
	/// - `true`: `0xFF`
	/// - `false`: `0xff`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn upper_case_hex(&self) -> bool {
		(self.options1 & Flags1::UPPER_CASE_HEX) != 0
	}

	/// Use upper case hex digits
	///
	/// - Default: `true`
	/// - `true`: `0xFF`
	/// - `false`: `0xff`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_upper_case_hex(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::UPPER_CASE_HEX;
		} else {
			self.options1 &= !Flags1::UPPER_CASE_HEX;
		}
	}

	/// Small hex numbers (-9 .. 9) are shown in decimal
	///
	/// - Default: `true`
	/// - `true`: `9`
	/// - `false`: `0x9`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn small_hex_numbers_in_decimal(&self) -> bool {
		(self.options1 & Flags1::SMALL_HEX_NUMBERS_IN_DECIMAL) != 0
	}

	/// Small hex numbers (-9 .. 9) are shown in decimal
	///
	/// - Default: `true`
	/// - `true`: `9`
	/// - `false`: `0x9`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_small_hex_numbers_in_decimal(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::SMALL_HEX_NUMBERS_IN_DECIMAL;
		} else {
			self.options1 &= !Flags1::SMALL_HEX_NUMBERS_IN_DECIMAL;
		}
	}

	/// Add a leading zero to numbers if there's no prefix and the number starts with hex digits `A-F`
	///
	/// - Default: `true`
	/// - `true`: `0FFh`
	/// - `false`: `FFh`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn add_leading_zero_to_hex_numbers(&self) -> bool {
		(self.options1 & Flags1::ADD_LEADING_ZERO_TO_HEX_NUMBERS) != 0
	}

	/// Add a leading zero to numbers if there's no prefix and the number starts with hex digits `A-F`
	///
	/// - Default: `true`
	/// - `true`: `0FFh`
	/// - `false`: `FFh`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_add_leading_zero_to_hex_numbers(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::ADD_LEADING_ZERO_TO_HEX_NUMBERS;
		} else {
			self.options1 &= !Flags1::ADD_LEADING_ZERO_TO_HEX_NUMBERS;
		}
	}

	/// Number base
	///
	/// - Default: [`Hexadecimal`]
	///
	/// [`Hexadecimal`]: enum.NumberBase.html#variant.Hexadecimal
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn number_base(&self) -> NumberBase {
		self.number_base
	}

	/// Number base
	///
	/// - Default: [`Hexadecimal`]
	///
	/// [`Hexadecimal`]: enum.NumberBase.html#variant.Hexadecimal
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_number_base(&mut self, value: NumberBase) {
		self.number_base = value
	}

	/// Add leading zeroes to branch offsets. Used by `CALL NEAR`, `CALL FAR`, `JMP NEAR`, `JMP FAR`, `Jcc`, `LOOP`, `LOOPcc`, `XBEGIN`
	///
	/// - Default: `false`
	/// - `true`: `je 00000123h`
	/// - `false`: `je 123h`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn branch_leading_zeroes(&self) -> bool {
		(self.options1 & Flags1::BRANCH_LEADING_ZEROES) != 0
	}

	/// Add leading zeroes to branch offsets. Used by `CALL NEAR`, `CALL FAR`, `JMP NEAR`, `JMP FAR`, `Jcc`, `LOOP`, `LOOPcc`, `XBEGIN`
	///
	/// - Default: `false`
	/// - `true`: `je 00000123h`
	/// - `false`: `je 123h`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_branch_leading_zeroes(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::BRANCH_LEADING_ZEROES;
		} else {
			self.options1 &= !Flags1::BRANCH_LEADING_ZEROES;
		}
	}

	/// Show immediate operands as signed numbers
	///
	/// - Default: `false`
	/// - `true`: `mov eax,-1`
	/// - `false`: `mov eax,FFFFFFFF`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn signed_immediate_operands(&self) -> bool {
		(self.options1 & Flags1::SIGNED_IMMEDIATE_OPERANDS) != 0
	}

	/// Show immediate operands as signed numbers
	///
	/// - Default: `false`
	/// - `true`: `mov eax,-1`
	/// - `false`: `mov eax,FFFFFFFF`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_signed_immediate_operands(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::SIGNED_IMMEDIATE_OPERANDS;
		} else {
			self.options1 &= !Flags1::SIGNED_IMMEDIATE_OPERANDS;
		}
	}

	/// Displacements are signed numbers
	///
	/// - Default: `true`
	/// - `true`: `mov al,[eax-2000h]`
	/// - `false`: `mov al,[eax+0FFFFE000h]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn signed_memory_displacements(&self) -> bool {
		(self.options1 & Flags1::SIGNED_MEMORY_DISPLACEMENTS) != 0
	}

	/// Displacements are signed numbers
	///
	/// - Default: `true`
	/// - `true`: `mov al,[eax-2000h]`
	/// - `false`: `mov al,[eax+0FFFFE000h]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_signed_memory_displacements(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::SIGNED_MEMORY_DISPLACEMENTS;
		} else {
			self.options1 &= !Flags1::SIGNED_MEMORY_DISPLACEMENTS;
		}
	}

	/// Sign extend memory displacements to the address size (16-bit, 32-bit, 64-bit)
	///
	/// - Default: `false`
	/// - `true`: `mov al,[eax+00000012h]`
	/// - `false`: `mov al,[eax+12h]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn sign_extend_memory_displacements(&self) -> bool {
		(self.options1 & Flags1::SIGN_EXTEND_MEMORY_DISPLACEMENTS) != 0
	}

	/// Sign extend memory displacements to the address size (16-bit, 32-bit, 64-bit)
	///
	/// - Default: `false`
	/// - `true`: `mov al,[eax+00000012h]`
	/// - `false`: `mov al,[eax+12h]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_sign_extend_memory_displacements(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::SIGN_EXTEND_MEMORY_DISPLACEMENTS;
		} else {
			self.options1 &= !Flags1::SIGN_EXTEND_MEMORY_DISPLACEMENTS;
		}
	}

	/// Options that control if the memory size (eg. `DWORD PTR`) is shown or not.
	/// This is ignored by the gas (AT&T) formatter.
	///
	/// - Default: [`Default`]
	///
	/// [`Default`]: enum.MemorySizeOptions.html#variant.Default
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn memory_size_options(&self) -> MemorySizeOptions {
		self.memory_size_options
	}

	/// Options that control if the memory size (eg. `DWORD PTR`) is shown or not.
	/// This is ignored by the gas (AT&T) formatter.
	///
	/// - Default: [`Default`]
	///
	/// [`Default`]: enum.MemorySizeOptions.html#variant.Default
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_memory_size_options(&mut self, value: MemorySizeOptions) {
		self.memory_size_options = value
	}

	/// Show `RIP+displ` or the virtual address
	///
	/// - Default: `false`
	/// - `true`: `mov eax,[rip+12345678h]`
	/// - `false`: `mov eax,[1029384756AFBECDh]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn rip_relative_addresses(&self) -> bool {
		(self.options1 & Flags1::RIP_RELATIVE_ADDRESSES) != 0
	}

	/// Show `RIP+displ` or the virtual address
	///
	/// - Default: `false`
	/// - `true`: `mov eax,[rip+12345678h]`
	/// - `false`: `mov eax,[1029384756AFBECDh]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_rip_relative_addresses(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::RIP_RELATIVE_ADDRESSES;
		} else {
			self.options1 &= !Flags1::RIP_RELATIVE_ADDRESSES;
		}
	}

	/// Show `NEAR`, `SHORT`, etc if it's a branch instruction
	///
	/// - Default: `true`
	/// - `true`: `je short 1234h`
	/// - `false`: `je 1234h`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn show_branch_size(&self) -> bool {
		(self.options1 & Flags1::SHOW_BRANCH_SIZE) != 0
	}

	/// Show `NEAR`, `SHORT`, etc if it's a branch instruction
	///
	/// - Default: `true`
	/// - `true`: `je short 1234h`
	/// - `false`: `je 1234h`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_show_branch_size(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::SHOW_BRANCH_SIZE;
		} else {
			self.options1 &= !Flags1::SHOW_BRANCH_SIZE;
		}
	}

	/// Use pseudo instructions
	///
	/// - Default: `true`
	/// - `true`: `vcmpnltsd xmm2,xmm6,xmm3`
	/// - `false`: `vcmpsd xmm2,xmm6,xmm3,5`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn use_pseudo_ops(&self) -> bool {
		(self.options1 & Flags1::USE_PSEUDO_OPS) != 0
	}

	/// Use pseudo instructions
	///
	/// - Default: `true`
	/// - `true`: `vcmpnltsd xmm2,xmm6,xmm3`
	/// - `false`: `vcmpsd xmm2,xmm6,xmm3,5`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_use_pseudo_ops(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::USE_PSEUDO_OPS;
		} else {
			self.options1 &= !Flags1::USE_PSEUDO_OPS;
		}
	}

	/// Show the original value after the symbol name
	///
	/// - Default: `false`
	/// - `true`: `mov eax,[myfield (12345678)]`
	/// - `false`: `mov eax,[myfield]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn show_symbol_address(&self) -> bool {
		(self.options1 & Flags1::SHOW_SYMBOL_ADDRESS) != 0
	}

	/// Show the original value after the symbol name
	///
	/// - Default: `false`
	/// - `true`: `mov eax,[myfield (12345678)]`
	/// - `false`: `mov eax,[myfield]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_show_symbol_address(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::SHOW_SYMBOL_ADDRESS;
		} else {
			self.options1 &= !Flags1::SHOW_SYMBOL_ADDRESS;
		}
	}

	/// (gas only): If `true`, the formatter doesn't add `%` to registers
	///
	/// - Default: `false`
	/// - `true`: `mov eax,ecx`
	/// - `false`: `mov %eax,%ecx`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn gas_naked_registers(&self) -> bool {
		(self.options1 & Flags1::GAS_NAKED_REGISTERS) != 0
	}

	/// (gas only): If `true`, the formatter doesn't add `%` to registers
	///
	/// - Default: `false`
	/// - `true`: `mov eax,ecx`
	/// - `false`: `mov %eax,%ecx`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_gas_naked_registers(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::GAS_NAKED_REGISTERS;
		} else {
			self.options1 &= !Flags1::GAS_NAKED_REGISTERS;
		}
	}

	/// (gas only): Shows the mnemonic size suffix even when not needed
	///
	/// - Default: `false`
	/// - `true`: `movl %eax,%ecx`
	/// - `false`: `mov %eax,%ecx`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn gas_show_mnemonic_size_suffix(&self) -> bool {
		(self.options1 & Flags1::GAS_SHOW_MNEMONIC_SIZE_SUFFIX) != 0
	}

	/// (gas only): Shows the mnemonic size suffix even when not needed
	///
	/// - Default: `false`
	/// - `true`: `movl %eax,%ecx`
	/// - `false`: `mov %eax,%ecx`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_gas_show_mnemonic_size_suffix(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::GAS_SHOW_MNEMONIC_SIZE_SUFFIX;
		} else {
			self.options1 &= !Flags1::GAS_SHOW_MNEMONIC_SIZE_SUFFIX;
		}
	}

	/// (gas only): Add a space after the comma if it's a memory operand
	///
	/// - Default: `false`
	/// - `true`: `(%eax, %ecx, 2)`
	/// - `false`: `(%eax,%ecx,2)`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn gas_space_after_memory_operand_comma(&self) -> bool {
		(self.options1 & Flags1::GAS_SPACE_AFTER_MEMORY_OPERAND_COMMA) != 0
	}

	/// (gas only): Add a space after the comma if it's a memory operand
	///
	/// - Default: `false`
	/// - `true`: `(%eax, %ecx, 2)`
	/// - `false`: `(%eax,%ecx,2)`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_gas_space_after_memory_operand_comma(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::GAS_SPACE_AFTER_MEMORY_OPERAND_COMMA;
		} else {
			self.options1 &= !Flags1::GAS_SPACE_AFTER_MEMORY_OPERAND_COMMA;
		}
	}

	/// (masm only): Add a `DS` segment override even if it's not present. Used if it's 16/32-bit code and mem op is a displ
	///
	/// - Default: `true`
	/// - `true`: `mov eax,ds:[12345678]`
	/// - `false`: `mov eax,[12345678]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn masm_add_ds_prefix32(&self) -> bool {
		(self.options1 & Flags1::MASM_ADD_DS_PREFIX32) != 0
	}

	/// (masm only): Add a `DS` segment override even if it's not present. Used if it's 16/32-bit code and mem op is a displ
	///
	/// - Default: `true`
	/// - `true`: `mov eax,ds:[12345678]`
	/// - `false`: `mov eax,[12345678]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_masm_add_ds_prefix32(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::MASM_ADD_DS_PREFIX32;
		} else {
			self.options1 &= !Flags1::MASM_ADD_DS_PREFIX32;
		}
	}

	/// (masm only): Show symbols in brackets
	///
	/// - Default: `true`
	/// - `true`: `[ecx+symbol]` / `[symbol]`
	/// - `false`: `symbol[ecx]` / `symbol`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn masm_symbol_displ_in_brackets(&self) -> bool {
		(self.options1 & Flags1::MASM_SYMBOL_DISPL_IN_BRACKETS) != 0
	}

	/// (masm only): Show symbols in brackets
	///
	/// - Default: `true`
	/// - `true`: `[ecx+symbol]` / `[symbol]`
	/// - `false`: `symbol[ecx]` / `symbol`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_masm_symbol_displ_in_brackets(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::MASM_SYMBOL_DISPL_IN_BRACKETS;
		} else {
			self.options1 &= !Flags1::MASM_SYMBOL_DISPL_IN_BRACKETS;
		}
	}

	/// (masm only): Show displacements in brackets
	///
	/// - Default: `true`
	/// - `true`: `[ecx+1234h]`
	/// - `false`: `1234h[ecx]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn masm_displ_in_brackets(&self) -> bool {
		(self.options1 & Flags1::MASM_DISPL_IN_BRACKETS) != 0
	}

	/// (masm only): Show displacements in brackets
	///
	/// - Default: `true`
	/// - `true`: `[ecx+1234h]`
	/// - `false`: `1234h[ecx]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_masm_displ_in_brackets(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::MASM_DISPL_IN_BRACKETS;
		} else {
			self.options1 &= !Flags1::MASM_DISPL_IN_BRACKETS;
		}
	}

	/// (nasm only): Shows `BYTE`, `WORD`, `DWORD` or `QWORD` if it's a sign extended immediate operand value
	///
	/// - Default: `false`
	/// - `true`: `or rcx,byte -1`
	/// - `false`: `or rcx,-1`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn nasm_show_sign_extended_immediate_size(&self) -> bool {
		(self.options2 & Flags2::NASM_SHOW_SIGN_EXTENDED_IMMEDIATE_SIZE) != 0
	}

	/// (nasm only): Shows `BYTE`, `WORD`, `DWORD` or `QWORD` if it's a sign extended immediate operand value
	///
	/// - Default: `false`
	/// - `true`: `or rcx,byte -1`
	/// - `false`: `or rcx,-1`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_nasm_show_sign_extended_immediate_size(&mut self, value: bool) {
		if value {
			self.options2 |= Flags2::NASM_SHOW_SIGN_EXTENDED_IMMEDIATE_SIZE;
		} else {
			self.options2 &= !Flags2::NASM_SHOW_SIGN_EXTENDED_IMMEDIATE_SIZE;
		}
	}
}

impl Default for FormatterOptions {
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn default() -> Self {
		FormatterOptions::new()
	}
}
