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
use core::hash::{Hash, Hasher};

#[derive(Debug, Clone)]
enum FormatterOptionString {
	String(String),
	Str(&'static str),
}

impl FormatterOptionString {
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn as_str(&self) -> &str {
		match self {
			&FormatterOptionString::String(ref s) => s.as_str(),
			&FormatterOptionString::Str(s) => s,
		}
	}
}

impl Default for FormatterOptionString {
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn default() -> Self {
		FormatterOptionString::Str("")
	}
}

impl Eq for FormatterOptionString {}
impl PartialEq<FormatterOptionString> for FormatterOptionString {
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	fn eq(&self, other: &FormatterOptionString) -> bool {
		self.as_str().eq(other.as_str())
	}
}

impl Hash for FormatterOptionString {
	#[inline]
	fn hash<H: Hasher>(&self, state: &mut H) {
		state.write(self.as_str().as_bytes());
	}
}

struct Flags1;
impl Flags1 {
	const UPPERCASE_PREFIXES: u32 = 0x0000_0001;
	const UPPERCASE_MNEMONICS: u32 = 0x0000_0002;
	const UPPERCASE_REGISTERS: u32 = 0x0000_0004;
	const UPPERCASE_KEYWORDS: u32 = 0x0000_0008;
	const UPPERCASE_DECORATORS: u32 = 0x0000_0010;
	const UPPERCASE_ALL: u32 = 0x0000_0020;
	const SPACE_AFTER_OPERAND_SEPARATOR: u32 = 0x0000_0040;
	const SPACE_AFTER_MEMORY_BRACKET: u32 = 0x0000_0080;
	const SPACE_BETWEEN_MEMORY_ADD_OPERATORS: u32 = 0x0000_0100;
	const SPACE_BETWEEN_MEMORY_MUL_OPERATORS: u32 = 0x0000_0200;
	const SCALE_BEFORE_INDEX: u32 = 0x0000_0400;
	const ALWAYS_SHOW_SCALE: u32 = 0x0000_0800;
	const ALWAYS_SHOW_SEGMENT_REGISTER: u32 = 0x0000_1000;
	const SHOW_ZERO_DISPLACEMENTS: u32 = 0x0000_2000;
	const LEADING_ZEROES: u32 = 0x0000_4000;
	const UPPERCASE_HEX: u32 = 0x0000_8000;
	const SMALL_HEX_NUMBERS_IN_DECIMAL: u32 = 0x0001_0000;
	const ADD_LEADING_ZERO_TO_HEX_NUMBERS: u32 = 0x0002_0000;
	const BRANCH_LEADING_ZEROES: u32 = 0x0004_0000;
	const SIGNED_IMMEDIATE_OPERANDS: u32 = 0x0008_0000;
	const SIGNED_MEMORY_DISPLACEMENTS: u32 = 0x0010_0000;
	const DISPLACEMENT_LEADING_ZEROES: u32 = 0x0020_0000;
	const RIP_RELATIVE_ADDRESSES: u32 = 0x0040_0000;
	const SHOW_BRANCH_SIZE: u32 = 0x0080_0000;
	const USE_PSEUDO_OPS: u32 = 0x0100_0000;
	const SHOW_SYMBOL_ADDRESS: u32 = 0x0200_0000;
	const GAS_NAKED_REGISTERS: u32 = 0x0400_0000;
	const GAS_SHOW_MNEMONIC_SIZE_SUFFIX: u32 = 0x0800_0000;
	const GAS_SPACE_AFTER_MEMORY_OPERAND_COMMA: u32 = 0x1000_0000;
	const MASM_ADD_DS_PREFIX32: u32 = 0x2000_0000;
	const MASM_SYMBOL_DISPL_IN_BRACKETS: u32 = 0x4000_0000;
	const MASM_DISPL_IN_BRACKETS: u32 = 0x8000_0000;
}

struct Flags2;
impl Flags2 {
	const NASM_SHOW_SIGN_EXTENDED_IMMEDIATE_SIZE: u32 = 0x0000_0001;
	const PREFER_ST0: u32 = 0x0000_0002;
}

/// Formatter options
#[derive(Debug, Clone, Eq, PartialEq, Hash)]
pub struct FormatterOptions {
	hex_prefix: FormatterOptionString,
	hex_suffix: FormatterOptionString,
	decimal_prefix: FormatterOptionString,
	decimal_suffix: FormatterOptionString,
	octal_prefix: FormatterOptionString,
	octal_suffix: FormatterOptionString,
	binary_prefix: FormatterOptionString,
	binary_suffix: FormatterOptionString,
	digit_separator: FormatterOptionString,
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
			hex_prefix: FormatterOptionString::default(),
			hex_suffix: FormatterOptionString::default(),
			decimal_prefix: FormatterOptionString::default(),
			decimal_suffix: FormatterOptionString::default(),
			octal_prefix: FormatterOptionString::default(),
			octal_suffix: FormatterOptionString::default(),
			binary_prefix: FormatterOptionString::default(),
			binary_suffix: FormatterOptionString::default(),
			digit_separator: FormatterOptionString::default(),
			hex_digit_group_size: 4,
			decimal_digit_group_size: 3,
			octal_digit_group_size: 4,
			binary_digit_group_size: 4,
			options1: Flags1::UPPERCASE_HEX
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
	#[cfg(feature = "gas")]
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_gas() -> Self {
		let mut options = FormatterOptions::new();
		options.set_hex_prefix("0x");
		options.set_octal_prefix("0");
		options.set_binary_prefix("0b");
		options
	}

	/// Creates default Intel (XED) formatter options
	#[cfg(feature = "intel")]
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_intel() -> Self {
		let mut options = FormatterOptions::new();
		options.set_hex_suffix("h");
		options.set_octal_suffix("o");
		options.set_binary_suffix("b");
		options
	}

	/// Creates default masm formatter options
	#[cfg(feature = "masm")]
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_masm() -> Self {
		let mut options = FormatterOptions::new();
		options.set_hex_suffix("h");
		options.set_octal_suffix("o");
		options.set_binary_suffix("b");
		options
	}

	/// Creates default nasm formatter options
	#[cfg(feature = "nasm")]
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn with_nasm() -> Self {
		let mut options = FormatterOptions::new();
		options.set_hex_suffix("h");
		options.set_octal_suffix("o");
		options.set_binary_suffix("b");
		options
	}

	// NOTE: These tables must render correctly by `cargo doc` and inside of IDEs, eg. VSCode.
	// An extra `-` is needed for `cargo doc`.

	/// Prefixes are upper cased
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `REP stosd`
	/// Yes | `false` | `rep stosd`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn uppercase_prefixes(&self) -> bool {
		(self.options1 & Flags1::UPPERCASE_PREFIXES) != 0
	}

	/// Prefixes are upper cased
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `REP stosd`
	/// Yes | `false` | `rep stosd`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_uppercase_prefixes(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::UPPERCASE_PREFIXES;
		} else {
			self.options1 &= !Flags1::UPPERCASE_PREFIXES;
		}
	}

	/// Mnemonics are upper cased
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `MOV rcx,rax`
	/// Yes | `false` | `mov rcx,rax`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn uppercase_mnemonics(&self) -> bool {
		(self.options1 & Flags1::UPPERCASE_MNEMONICS) != 0
	}

	/// Mnemonics are upper cased
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `MOV rcx,rax`
	/// Yes | `false` | `mov rcx,rax`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_uppercase_mnemonics(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::UPPERCASE_MNEMONICS;
		} else {
			self.options1 &= !Flags1::UPPERCASE_MNEMONICS;
		}
	}

	/// Registers are upper cased
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov RCX,[RAX+RDX*8]`
	/// Yes | `false` | `mov rcx,[rax+rdx*8]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn uppercase_registers(&self) -> bool {
		(self.options1 & Flags1::UPPERCASE_REGISTERS) != 0
	}

	/// Registers are upper cased
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov RCX,[RAX+RDX*8]`
	/// Yes | `false` | `mov rcx,[rax+rdx*8]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_uppercase_registers(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::UPPERCASE_REGISTERS;
		} else {
			self.options1 &= !Flags1::UPPERCASE_REGISTERS;
		}
	}

	/// Keywords are upper cased (eg. `BYTE PTR`, `SHORT`)
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov BYTE PTR [rcx],12h`
	/// Yes | `false` | `mov byte ptr [rcx],12h`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn uppercase_keywords(&self) -> bool {
		(self.options1 & Flags1::UPPERCASE_KEYWORDS) != 0
	}

	/// Keywords are upper cased (eg. `BYTE PTR`, `SHORT`)
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov BYTE PTR [rcx],12h`
	/// Yes | `false` | `mov byte ptr [rcx],12h`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_uppercase_keywords(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::UPPERCASE_KEYWORDS;
		} else {
			self.options1 &= !Flags1::UPPERCASE_KEYWORDS;
		}
	}

	/// Upper case decorators, eg. `{z}`, `{sae}`, `{rd-sae}` (but not op mask registers: `{k1}`)
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `vunpcklps xmm2{k5}{Z},xmm6,dword bcst [rax+4]`
	/// Yes | `false` | `vunpcklps xmm2{k5}{z},xmm6,dword bcst [rax+4]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn uppercase_decorators(&self) -> bool {
		(self.options1 & Flags1::UPPERCASE_DECORATORS) != 0
	}

	/// Upper case decorators, eg. `{z}`, `{sae}`, `{rd-sae}` (but not op mask registers: `{k1}`)
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `vunpcklps xmm2{k5}{Z},xmm6,dword bcst [rax+4]`
	/// Yes | `false` | `vunpcklps xmm2{k5}{z},xmm6,dword bcst [rax+4]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_uppercase_decorators(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::UPPERCASE_DECORATORS;
		} else {
			self.options1 &= !Flags1::UPPERCASE_DECORATORS;
		}
	}

	/// Everything is upper cased, except numbers and their prefixes/suffixes
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `MOV EAX,GS:[RCX*4+0ffh]`
	/// Yes | `false` | `mov eax,gs:[rcx*4+0ffh]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn uppercase_all(&self) -> bool {
		(self.options1 & Flags1::UPPERCASE_ALL) != 0
	}

	/// Everything is upper cased, except numbers and their prefixes/suffixes
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `MOV EAX,GS:[RCX*4+0ffh]`
	/// Yes | `false` | `mov eax,gs:[rcx*4+0ffh]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_uppercase_all(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::UPPERCASE_ALL;
		} else {
			self.options1 &= !Flags1::UPPERCASE_ALL;
		}
	}

	/// Character index (0-based) where the first operand is formatted. Can be set to 0 to format it immediately after the mnemonic.
	/// At least one space or tab is always added between the mnemonic and the first operand.
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `0` | `mov•rcx,rbp`
	/// - | `8` | `mov•••••rcx,rbp`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn first_operand_char_index(&self) -> u32 {
		self.first_operand_char_index
	}

	/// Character index (0-based) where the first operand is formatted. Can be set to 0 to format it immediately after the mnemonic.
	/// At least one space or tab is always added between the mnemonic and the first operand.
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `0` | `mov•rcx,rbp`
	/// - | `8` | `mov•••••rcx,rbp`
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov rax, rcx`
	/// Yes | `false` | `mov rax,rcx`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn space_after_operand_separator(&self) -> bool {
		(self.options1 & Flags1::SPACE_AFTER_OPERAND_SEPARATOR) != 0
	}

	/// Add a space after the operand separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov rax, rcx`
	/// Yes | `false` | `mov rax,rcx`
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[ rcx+rdx ]`
	/// Yes | `false` | `mov eax,[rcx+rdx]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn space_after_memory_bracket(&self) -> bool {
		(self.options1 & Flags1::SPACE_AFTER_MEMORY_BRACKET) != 0
	}

	/// Add a space between the memory expression and the brackets
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[ rcx+rdx ]`
	/// Yes | `false` | `mov eax,[rcx+rdx]`
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rcx + rdx*8 - 80h]`
	/// Yes | `false` | `mov eax,[rcx+rdx*8-80h]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn space_between_memory_add_operators(&self) -> bool {
		(self.options1 & Flags1::SPACE_BETWEEN_MEMORY_ADD_OPERATORS) != 0
	}

	/// Add spaces between memory operand `+` and `-` operators
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rcx + rdx*8 - 80h]`
	/// Yes | `false` | `mov eax,[rcx+rdx*8-80h]`
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rcx+rdx * 8-80h]`
	/// Yes | `false` | `mov eax,[rcx+rdx*8-80h]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn space_between_memory_mul_operators(&self) -> bool {
		(self.options1 & Flags1::SPACE_BETWEEN_MEMORY_MUL_OPERATORS) != 0
	}

	/// Add spaces between memory operand `*` operator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rcx+rdx * 8-80h]`
	/// Yes | `false` | `mov eax,[rcx+rdx*8-80h]`
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[8*rdx]`
	/// Yes | `false` | `mov eax,[rdx*8]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn scale_before_index(&self) -> bool {
		(self.options1 & Flags1::SCALE_BEFORE_INDEX) != 0
	}

	/// Show memory operand scale value before the index register
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[8*rdx]`
	/// Yes | `false` | `mov eax,[rdx*8]`
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rbx+rcx*1]`
	/// Yes | `false` | `mov eax,[rbx+rcx]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn always_show_scale(&self) -> bool {
		(self.options1 & Flags1::ALWAYS_SHOW_SCALE) != 0
	}

	/// Always show the scale value even if it's `*1`
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rbx+rcx*1]`
	/// Yes | `false` | `mov eax,[rbx+rcx]`
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,ds:[ecx]`
	/// Yes | `false` | `mov eax,[ecx]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn always_show_segment_register(&self) -> bool {
		(self.options1 & Flags1::ALWAYS_SHOW_SEGMENT_REGISTER) != 0
	}

	/// Always show the effective segment register. If the option is `false`, only show the segment register if
	/// there's a segment override prefix.
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,ds:[ecx]`
	/// Yes | `false` | `mov eax,[ecx]`
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rcx*2+0]`
	/// Yes | `false` | `mov eax,[rcx*2]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn show_zero_displacements(&self) -> bool {
		(self.options1 & Flags1::SHOW_ZERO_DISPLACEMENTS) != 0
	}

	/// Show zero displacements
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rcx*2+0]`
	/// Yes | `false` | `mov eax,[rcx*2]`
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
		self.hex_prefix.as_str()
	}

	/// Hex number prefix or an empty string, eg. `"0x"`
	///
	/// - Default: `""` (masm/nasm/intel), `"0x"` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_hex_prefix(&mut self, value: &'static str) {
		self.hex_prefix = FormatterOptionString::Str(value)
	}

	/// Hex number prefix or an empty string, eg. `"0x"`
	///
	/// - Default: `""` (masm/nasm/intel), `"0x"` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_hex_prefix_string(&mut self, value: String) {
		self.hex_prefix = FormatterOptionString::String(value)
	}

	/// Hex number suffix or an empty string, eg. `"h"`
	///
	/// - Default: `"h"` (masm/nasm/intel), `""` (gas)
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn hex_suffix(&self) -> &str {
		self.hex_suffix.as_str()
	}

	/// Hex number suffix or an empty string, eg. `"h"`
	///
	/// - Default: `"h"` (masm/nasm/intel), `""` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_hex_suffix(&mut self, value: &'static str) {
		self.hex_suffix = FormatterOptionString::Str(value)
	}

	/// Hex number suffix or an empty string, eg. `"h"`
	///
	/// - Default: `"h"` (masm/nasm/intel), `""` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_hex_suffix_string(&mut self, value: String) {
		self.hex_suffix = FormatterOptionString::String(value)
	}

	/// Size of a digit group, see also [`digit_separator()`]
	///
	/// [`digit_separator()`]: #method.digit_separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `0` | `0x12345678`
	/// Yes | `4` | `0x1234_5678`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn hex_digit_group_size(&self) -> u32 {
		self.hex_digit_group_size
	}

	/// Size of a digit group, see also [`digit_separator()`]
	///
	/// [`digit_separator()`]: #method.digit_separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `0` | `0x12345678`
	/// Yes | `4` | `0x1234_5678`
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
		self.decimal_prefix.as_str()
	}

	/// Decimal number prefix or an empty string
	///
	/// - Default: `""`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_decimal_prefix(&mut self, value: &'static str) {
		self.decimal_prefix = FormatterOptionString::Str(value)
	}

	/// Decimal number prefix or an empty string
	///
	/// - Default: `""`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_decimal_prefix_string(&mut self, value: String) {
		self.decimal_prefix = FormatterOptionString::String(value)
	}

	/// Decimal number suffix or an empty string
	///
	/// - Default: `""`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn decimal_suffix(&self) -> &str {
		self.decimal_suffix.as_str()
	}

	/// Decimal number suffix or an empty string
	///
	/// - Default: `""`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_decimal_suffix(&mut self, value: &'static str) {
		self.decimal_suffix = FormatterOptionString::Str(value)
	}

	/// Decimal number suffix or an empty string
	///
	/// - Default: `""`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_decimal_suffix_string(&mut self, value: String) {
		self.decimal_suffix = FormatterOptionString::String(value)
	}

	/// Size of a digit group, see also [`digit_separator()`]
	///
	/// [`digit_separator()`]: #method.digit_separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `0` | `12345678`
	/// Yes | `3` | `12_345_678`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn decimal_digit_group_size(&self) -> u32 {
		self.decimal_digit_group_size
	}

	/// Size of a digit group, see also [`digit_separator()`]
	///
	/// [`digit_separator()`]: #method.digit_separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `0` | `12345678`
	/// Yes | `3` | `12_345_678`
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
		self.octal_prefix.as_str()
	}

	/// Octal number prefix or an empty string
	///
	/// - Default: `""` (masm/nasm/intel), `"0"` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_octal_prefix(&mut self, value: &'static str) {
		self.octal_prefix = FormatterOptionString::Str(value)
	}

	/// Octal number prefix or an empty string
	///
	/// - Default: `""` (masm/nasm/intel), `"0"` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_octal_prefix_string(&mut self, value: String) {
		self.octal_prefix = FormatterOptionString::String(value)
	}

	/// Octal number suffix or an empty string
	///
	/// - Default: `"o"` (masm/nasm/intel), `""` (gas)
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn octal_suffix(&self) -> &str {
		self.octal_suffix.as_str()
	}

	/// Octal number suffix or an empty string
	///
	/// - Default: `"o"` (masm/nasm/intel), `""` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_octal_suffix(&mut self, value: &'static str) {
		self.octal_suffix = FormatterOptionString::Str(value)
	}

	/// Octal number suffix or an empty string
	///
	/// - Default: `"o"` (masm/nasm/intel), `""` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_octal_suffix_string(&mut self, value: String) {
		self.octal_suffix = FormatterOptionString::String(value)
	}

	/// Size of a digit group, see also [`digit_separator()`]
	///
	/// [`digit_separator()`]: #method.digit_separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `0` | `12345670`
	/// Yes | `4` | `1234_5670`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn octal_digit_group_size(&self) -> u32 {
		self.octal_digit_group_size
	}

	/// Size of a digit group, see also [`digit_separator()`]
	///
	/// [`digit_separator()`]: #method.digit_separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `0` | `12345670`
	/// Yes | `4` | `1234_5670`
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
		self.binary_prefix.as_str()
	}

	/// Binary number prefix or an empty string
	///
	/// - Default: `""` (masm/nasm/intel), `"0b"` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_binary_prefix(&mut self, value: &'static str) {
		self.binary_prefix = FormatterOptionString::Str(value)
	}

	/// Binary number prefix or an empty string
	///
	/// - Default: `""` (masm/nasm/intel), `"0b"` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_binary_prefix_string(&mut self, value: String) {
		self.binary_prefix = FormatterOptionString::String(value)
	}

	/// Binary number suffix or an empty string
	///
	/// - Default: `"b"` (masm/nasm/intel), `""` (gas)
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn binary_suffix(&self) -> &str {
		self.binary_suffix.as_str()
	}

	/// Binary number suffix or an empty string
	///
	/// - Default: `"b"` (masm/nasm/intel), `""` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_binary_suffix(&mut self, value: &'static str) {
		self.binary_suffix = FormatterOptionString::Str(value)
	}

	/// Binary number suffix or an empty string
	///
	/// - Default: `"b"` (masm/nasm/intel), `""` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_binary_suffix_string(&mut self, value: String) {
		self.binary_suffix = FormatterOptionString::String(value)
	}

	/// Size of a digit group, see also [`digit_separator()`]
	///
	/// [`digit_separator()`]: #method.digit_separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `0` | `11010111`
	/// Yes | `4` | `1101_0111`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn binary_digit_group_size(&self) -> u32 {
		self.binary_digit_group_size
	}

	/// Size of a digit group, see also [`digit_separator()`]
	///
	/// [`digit_separator()`]: #method.digit_separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `0` | `11010111`
	/// Yes | `4` | `1101_0111`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_binary_digit_group_size(&mut self, value: u32) {
		self.binary_digit_group_size = value
	}

	/// Digit separator or an empty string. See also eg. [`hex_digit_group_size()`]
	///
	/// [`hex_digit_group_size()`]: #method.hex_digit_group_size
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `""` | `0x12345678`
	/// - | `"_"` | `0x1234_5678`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn digit_separator(&self) -> &str {
		self.digit_separator.as_str()
	}

	/// Digit separator or an empty string. See also eg. [`hex_digit_group_size()`]
	///
	/// [`hex_digit_group_size()`]: #method.hex_digit_group_size
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `""` | `0x12345678`
	/// - | `"_"` | `0x1234_5678`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_digit_separator(&mut self, value: &'static str) {
		self.digit_separator = FormatterOptionString::Str(value)
	}

	/// Digit separator or an empty string. See also eg. [`hex_digit_group_size()`]
	///
	/// [`hex_digit_group_size()`]: #method.hex_digit_group_size
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `""` | `0x12345678`
	/// - | `"_"` | `0x1234_5678`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_digit_separator_string(&mut self, value: String) {
		self.digit_separator = FormatterOptionString::String(value)
	}

	/// Add leading zeroes to hexadecimal/octal/binary numbers.
	/// This option has no effect on branch targets and displacements, use [`branch_leading_zeroes`]
	/// and [`displacement_leading_zeroes`].
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `0x0000000A`/`0000000Ah`
	/// Yes | `false` | `0xA`/`0Ah`
	///
	/// [`branch_leading_zeroes`]: #structfield.branch_leading_zeroes
	/// [`displacement_leading_zeroes`]: #structfield.displacement_leading_zeroes
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn leading_zeroes(&self) -> bool {
		(self.options1 & Flags1::LEADING_ZEROES) != 0
	}

	/// Add leading zeroes to hexadecimal/octal/binary numbers.
	/// This option has no effect on branch targets and displacements, use [`branch_leading_zeroes`]
	/// and [`displacement_leading_zeroes`].
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `0x0000000A`/`0000000Ah`
	/// Yes | `false` | `0xA`/`0Ah`
	///
	/// [`branch_leading_zeroes`]: #structfield.branch_leading_zeroes
	/// [`displacement_leading_zeroes`]: #structfield.displacement_leading_zeroes
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `0xFF`
	/// - | `false` | `0xff`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn uppercase_hex(&self) -> bool {
		(self.options1 & Flags1::UPPERCASE_HEX) != 0
	}

	/// Use upper case hex digits
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `0xFF`
	/// - | `false` | `0xff`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_uppercase_hex(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::UPPERCASE_HEX;
		} else {
			self.options1 &= !Flags1::UPPERCASE_HEX;
		}
	}

	/// Small hex numbers (-9 .. 9) are shown in decimal
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `9`
	/// - | `false` | `0x9`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn small_hex_numbers_in_decimal(&self) -> bool {
		(self.options1 & Flags1::SMALL_HEX_NUMBERS_IN_DECIMAL) != 0
	}

	/// Small hex numbers (-9 .. 9) are shown in decimal
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `9`
	/// - | `false` | `0x9`
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

	/// Add a leading zero to hex numbers if there's no prefix and the number starts with hex digits `A-F`
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `0FFh`
	/// - | `false` | `FFh`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn add_leading_zero_to_hex_numbers(&self) -> bool {
		(self.options1 & Flags1::ADD_LEADING_ZERO_TO_HEX_NUMBERS) != 0
	}

	/// Add a leading zero to hex numbers if there's no prefix and the number starts with hex digits `A-F`
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `0FFh`
	/// - | `false` | `FFh`
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `je 00000123h`
	/// - | `false` | `je 123h`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn branch_leading_zeroes(&self) -> bool {
		(self.options1 & Flags1::BRANCH_LEADING_ZEROES) != 0
	}

	/// Add leading zeroes to branch offsets. Used by `CALL NEAR`, `CALL FAR`, `JMP NEAR`, `JMP FAR`, `Jcc`, `LOOP`, `LOOPcc`, `XBEGIN`
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `je 00000123h`
	/// - | `false` | `je 123h`
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,-1`
	/// Yes | `false` | `mov eax,FFFFFFFF`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn signed_immediate_operands(&self) -> bool {
		(self.options1 & Flags1::SIGNED_IMMEDIATE_OPERANDS) != 0
	}

	/// Show immediate operands as signed numbers
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,-1`
	/// Yes | `false` | `mov eax,FFFFFFFF`
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `mov al,[eax-2000h]`
	/// - | `false` | `mov al,[eax+0FFFFE000h]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn signed_memory_displacements(&self) -> bool {
		(self.options1 & Flags1::SIGNED_MEMORY_DISPLACEMENTS) != 0
	}

	/// Displacements are signed numbers
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `mov al,[eax-2000h]`
	/// - | `false` | `mov al,[eax+0FFFFE000h]`
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

	/// Add leading zeroes to displacements
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov al,[eax+00000012h]`
	/// Yes | `false` | `mov al,[eax+12h]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn displacement_leading_zeroes(&self) -> bool {
		(self.options1 & Flags1::DISPLACEMENT_LEADING_ZEROES) != 0
	}

	/// Add leading zeroes to displacements
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov al,[eax+00000012h]`
	/// Yes | `false` | `mov al,[eax+12h]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_displacement_leading_zeroes(&mut self, value: bool) {
		if value {
			self.options1 |= Flags1::DISPLACEMENT_LEADING_ZEROES;
		} else {
			self.options1 &= !Flags1::DISPLACEMENT_LEADING_ZEROES;
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rip+12345678h]`
	/// Yes | `false` | `mov eax,[1029384756AFBECDh]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn rip_relative_addresses(&self) -> bool {
		(self.options1 & Flags1::RIP_RELATIVE_ADDRESSES) != 0
	}

	/// Show `RIP+displ` or the virtual address
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rip+12345678h]`
	/// Yes | `false` | `mov eax,[1029384756AFBECDh]`
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `je short 1234h`
	/// - | `false` | `je 1234h`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn show_branch_size(&self) -> bool {
		(self.options1 & Flags1::SHOW_BRANCH_SIZE) != 0
	}

	/// Show `NEAR`, `SHORT`, etc if it's a branch instruction
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `je short 1234h`
	/// - | `false` | `je 1234h`
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `vcmpnltsd xmm2,xmm6,xmm3`
	/// - | `false` | `vcmpsd xmm2,xmm6,xmm3,5`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn use_pseudo_ops(&self) -> bool {
		(self.options1 & Flags1::USE_PSEUDO_OPS) != 0
	}

	/// Use pseudo instructions
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `vcmpnltsd xmm2,xmm6,xmm3`
	/// - | `false` | `vcmpsd xmm2,xmm6,xmm3,5`
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[myfield (12345678)]`
	/// Yes | `false` | `mov eax,[myfield]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn show_symbol_address(&self) -> bool {
		(self.options1 & Flags1::SHOW_SYMBOL_ADDRESS) != 0
	}

	/// Show the original value after the symbol name
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[myfield (12345678)]`
	/// Yes | `false` | `mov eax,[myfield]`
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,ecx`
	/// Yes | `false` | `mov %eax,%ecx`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn gas_naked_registers(&self) -> bool {
		(self.options1 & Flags1::GAS_NAKED_REGISTERS) != 0
	}

	/// (gas only): If `true`, the formatter doesn't add `%` to registers
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,ecx`
	/// Yes | `false` | `mov %eax,%ecx`
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `movl %eax,%ecx`
	/// Yes | `false` | `mov %eax,%ecx`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn gas_show_mnemonic_size_suffix(&self) -> bool {
		(self.options1 & Flags1::GAS_SHOW_MNEMONIC_SIZE_SUFFIX) != 0
	}

	/// (gas only): Shows the mnemonic size suffix even when not needed
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `movl %eax,%ecx`
	/// Yes | `false` | `mov %eax,%ecx`
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `(%eax, %ecx, 2)`
	/// Yes | `false` | `(%eax,%ecx,2)`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn gas_space_after_memory_operand_comma(&self) -> bool {
		(self.options1 & Flags1::GAS_SPACE_AFTER_MEMORY_OPERAND_COMMA) != 0
	}

	/// (gas only): Add a space after the comma if it's a memory operand
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `(%eax, %ecx, 2)`
	/// Yes | `false` | `(%eax,%ecx,2)`
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `mov eax,ds:[12345678]`
	/// - | `false` | `mov eax,[12345678]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn masm_add_ds_prefix32(&self) -> bool {
		(self.options1 & Flags1::MASM_ADD_DS_PREFIX32) != 0
	}

	/// (masm only): Add a `DS` segment override even if it's not present. Used if it's 16/32-bit code and mem op is a displ
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `mov eax,ds:[12345678]`
	/// - | `false` | `mov eax,[12345678]`
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `[ecx+symbol]` / `[symbol]`
	/// - | `false` | `symbol[ecx]` / `symbol`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn masm_symbol_displ_in_brackets(&self) -> bool {
		(self.options1 & Flags1::MASM_SYMBOL_DISPL_IN_BRACKETS) != 0
	}

	/// (masm only): Show symbols in brackets
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `[ecx+symbol]` / `[symbol]`
	/// - | `false` | `symbol[ecx]` / `symbol`
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `[ecx+1234h]`
	/// - | `false` | `1234h[ecx]`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn masm_displ_in_brackets(&self) -> bool {
		(self.options1 & Flags1::MASM_DISPL_IN_BRACKETS) != 0
	}

	/// (masm only): Show displacements in brackets
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `[ecx+1234h]`
	/// - | `false` | `1234h[ecx]`
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
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `or rcx,byte -1`
	/// Yes | `false` | `or rcx,-1`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn nasm_show_sign_extended_immediate_size(&self) -> bool {
		(self.options2 & Flags2::NASM_SHOW_SIGN_EXTENDED_IMMEDIATE_SIZE) != 0
	}

	/// (nasm only): Shows `BYTE`, `WORD`, `DWORD` or `QWORD` if it's a sign extended immediate operand value
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `or rcx,byte -1`
	/// Yes | `false` | `or rcx,-1`
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

	/// Use `st(0)` instead of `st` if `st` can be used. Ignored by the nasm formatter.
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `fadd st(0),st(3)`
	/// Yes | `false` | `fadd st,st(3)`
	#[cfg_attr(has_must_use, must_use)]
	#[inline]
	pub fn prefer_st0(&self) -> bool {
		(self.options2 & Flags2::PREFER_ST0) != 0
	}

	/// Use `st(0)` instead of `st` if `st` can be used. Ignored by the nasm formatter.
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `fadd st(0),st(3)`
	/// Yes | `false` | `fadd st,st(3)`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[inline]
	pub fn set_prefer_st0(&mut self, value: bool) {
		if value {
			self.options2 |= Flags2::PREFER_ST0;
		} else {
			self.options2 &= !Flags2::PREFER_ST0;
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
