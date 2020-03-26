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

#[cfg(feature = "gas")]
use iced_x86::GasFormatter;
#[cfg(feature = "intel")]
use iced_x86::IntelFormatter;
#[cfg(feature = "masm")]
use iced_x86::MasmFormatter;
#[cfg(feature = "nasm")]
use iced_x86::NasmFormatter;
use iced_x86::{Formatter, Instruction, MemorySizeOptions, NumberBase, OpAccess, Register};
use wasm_bindgen::prelude::*;

/// Formatter syntax (GNU Assembler, Intel XED, masm, nasm)
#[wasm_bindgen]
#[derive(Debug, Copy, Clone, Eq, PartialEq, Ord, PartialOrd, Hash)]
pub enum FormatterSyntaxX86 {
	/// GNU Assembler (AT&T)
	#[cfg(feature = "gas")]
	Gas,
	/// Intel XED
	#[cfg(feature = "intel")]
	Intel,
	/// masm
	#[cfg(feature = "masm")]
	Masm,
	/// nasm
	#[cfg(feature = "nasm")]
	Nasm,
}

/// X86 formatter that supports GNU Assembler, Intel XED, masm and nasm syntax
#[wasm_bindgen]
#[allow(missing_debug_implementations)]
pub struct FormatterX86 {
	formatter: Box<dyn Formatter>,
}

#[wasm_bindgen]
impl FormatterX86 {
	/// Creates an X86 formatter
	///
	/// # Arguments
	///
	/// * `syntax`: Formatter syntax, see [`FormatterSyntaxX86`]
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
	/// let mut formatter = FormatterX86::new(FormatterSyntaxX86::Masm);
	/// formatter.options_mut().set_uppercase_mnemonics(true);
	/// formatter.format(&instr, &mut output);
	/// assert_eq!("VCVTNE2PS2BF16 zmm2{k5}{z},zmm6,dword bcst [rax+4]", output);
	/// ```
	///
	/// [`FormatterSyntaxX86`]: enum.FormatterSyntaxX86.html
	#[must_use]
	#[wasm_bindgen(constructor)]
	pub fn new(syntax: FormatterSyntaxX86) -> Self {
		let formatter: Box<dyn Formatter> = match syntax {
			#[cfg(feature = "gas")]
			FormatterSyntaxX86::Gas => Box::new(GasFormatter::new()),
			#[cfg(feature = "intel")]
			FormatterSyntaxX86::Intel => Box::new(IntelFormatter::new()),
			#[cfg(feature = "masm")]
			FormatterSyntaxX86::Masm => Box::new(MasmFormatter::new()),
			#[cfg(feature = "nasm")]
			FormatterSyntaxX86::Nasm => Box::new(NasmFormatter::new()),
		};
		Self { formatter }
	}

	/// Formats the whole instruction: prefixes, mnemonic, operands
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	pub fn format(&mut self, instruction: &Instruction) -> String {
		let mut output = String::new();
		self.formatter.format(instruction, &mut output);
		output
	}

	/// Formats the mnemonic and any prefixes
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	pub fn format_mnemonic(&mut self, instruction: &Instruction) -> String {
		let mut output = String::new();
		self.formatter.format_mnemonic(instruction, &mut output);
		output
	}

	/// Formats the mnemonic and any prefixes
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `options`: Options, see [`FormatMnemonicOptions`]
	///
	/// [`FormatMnemonicOptions`]: struct.FormatMnemonicOptions.html
	pub fn format_mnemonic_options(&mut self, instruction: &Instruction, options: u32) -> String {
		let mut output = String::new();
		self.formatter.format_mnemonic_options(instruction, &mut output, options);
		output
	}

	/// Gets the number of operands that will be formatted. A formatter can add and remove operands
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	#[must_use]
	pub fn operand_count(&mut self, instruction: &Instruction) -> u32 {
		self.formatter.operand_count(instruction)
	}

	/// Returns the operand access but only if it's an operand added by the formatter. If it's an
	/// operand that is part of [`Instruction`], you should call eg. [`InstructionInfoFactory::info()`].
	///
	/// # Panics
	///
	/// Panics if `operand` is invalid
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `operand`: Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand. See [`operand_count()`]
	///
	/// [`Instruction`]: struct.Instruction.html
	/// [`InstructionInfoFactory::info()`]: struct.InstructionInfoFactory.html#method.info
	/// [`operand_count()`]: #tymethod.operand_count
	#[cfg(feature = "instr_info")]
	#[must_use]
	pub fn op_access(&mut self, instruction: &Instruction, operand: u32) -> Option<OpAccess> {
		self.formatter.op_access(instruction, operand)
	}

	/// Converts a formatter operand index to an instruction operand index. Returns `None` if it's an operand added by the formatter
	///
	/// # Panics
	///
	/// Panics if `operand` is invalid
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `operand`: Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand. See [`operand_count()`]
	///
	/// [`operand_count()`]: #tymethod.operand_count
	#[must_use]
	pub fn get_instruction_operand(&mut self, instruction: &Instruction, operand: u32) -> Option<u32> {
		self.formatter.get_instruction_operand(instruction, operand)
	}

	/// Converts an instruction operand index to a formatter operand index. Returns `None` if the instruction operand isn't used by the formatter
	///
	/// # Panics
	///
	/// Panics if `instruction_operand` is invalid
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `instruction_operand`: Instruction operand
	#[must_use]
	pub fn get_formatter_operand(&mut self, instruction: &Instruction, instruction_operand: u32) -> Option<u32> {
		self.formatter.get_formatter_operand(instruction, instruction_operand)
	}

	/// Formats an operand. This is a formatter operand and not necessarily a real instruction operand.
	/// A formatter can add and remove operands.
	///
	/// # Panics
	///
	/// Panics if `operand` is invalid
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `operand`: Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand. See [`operand_count()`]
	///
	/// [`operand_count()`]: #tymethod.operand_count
	pub fn format_operand(&mut self, instruction: &Instruction, operand: u32) -> String {
		let mut output = String::new();
		self.formatter.format_operand(instruction, &mut output, operand);
		output
	}

	/// Formats an operand separator
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	pub fn format_operand_separator(&mut self, instruction: &Instruction) -> String {
		let mut output = String::new();
		self.formatter.format_operand_separator(instruction, &mut output);
		output
	}

	/// Formats all operands
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	pub fn format_all_operands(&mut self, instruction: &Instruction) -> String {
		let mut output = String::new();
		self.formatter.format_all_operands(instruction, &mut output);
		output
	}

	/// Formats a register
	///
	/// # Arguments
	///
	/// - `register`: Register
	#[must_use]
	pub fn format_register(&mut self, register: Register) -> String {
		self.formatter.format_register(register).to_owned()
	}

	/// Formats a `i8`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[must_use]
	pub fn format_i8(&mut self, value: i8) -> String {
		self.formatter.format_i8(value).to_owned()
	}

	/// Formats a `i16`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[must_use]
	pub fn format_i16(&mut self, value: i16) -> String {
		self.formatter.format_i16(value).to_owned()
	}

	/// Formats a `i32`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[must_use]
	pub fn format_i32(&mut self, value: i32) -> String {
		self.formatter.format_i32(value).to_owned()
	}

	/// Formats a `i64`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[must_use]
	pub fn format_i64(&mut self, value: i64) -> String {
		self.formatter.format_i64(value).to_owned()
	}

	/// Formats a `u8`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[must_use]
	pub fn format_u8(&mut self, value: u8) -> String {
		self.formatter.format_u8(value).to_owned()
	}

	/// Formats a `u16`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[must_use]
	pub fn format_u16(&mut self, value: u16) -> String {
		self.formatter.format_u16(value).to_owned()
	}

	/// Formats a `u32`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[must_use]
	pub fn format_u32(&mut self, value: u32) -> String {
		self.formatter.format_u32(value).to_owned()
	}

	/// Formats a `u64`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[must_use]
	pub fn format_u64(&mut self, value: u64) -> String {
		self.formatter.format_u64(value).to_owned()
	}

	// NOTE: These tables must render correctly by `cargo doc` and inside of IDEs, eg. VSCode.
	// An extra `-` is needed for `cargo doc`.

	/// Prefixes are upper cased
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `REP stosd`
	/// Yes | `false` | `rep stosd`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn uppercase_prefixes(&self) -> bool {
		self.formatter.options().uppercase_prefixes()
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
	#[wasm_bindgen(setter)]
	pub fn set_uppercase_prefixes(&mut self, value: bool) {
		self.formatter.options_mut().set_uppercase_prefixes(value);
	}

	/// Mnemonics are upper cased
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `MOV rcx,rax`
	/// Yes | `false` | `mov rcx,rax`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn uppercase_mnemonics(&self) -> bool {
		self.formatter.options().uppercase_mnemonics()
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
	#[wasm_bindgen(setter)]
	pub fn set_uppercase_mnemonics(&mut self, value: bool) {
		self.formatter.options_mut().set_uppercase_mnemonics(value);
	}

	/// Registers are upper cased
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov RCX,[RAX+RDX*8]`
	/// Yes | `false` | `mov rcx,[rax+rdx*8]`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn uppercase_registers(&self) -> bool {
		self.formatter.options().uppercase_registers()
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
	#[wasm_bindgen(setter)]
	pub fn set_uppercase_registers(&mut self, value: bool) {
		self.formatter.options_mut().set_uppercase_registers(value);
	}

	/// Keywords are upper cased (eg. `BYTE PTR`, `SHORT`)
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov BYTE PTR [rcx],12h`
	/// Yes | `false` | `mov byte ptr [rcx],12h`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn uppercase_keywords(&self) -> bool {
		self.formatter.options().uppercase_keywords()
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
	#[wasm_bindgen(setter)]
	pub fn set_uppercase_keywords(&mut self, value: bool) {
		self.formatter.options_mut().set_uppercase_keywords(value);
	}

	/// Upper case decorators, eg. `{z}`, `{sae}`, `{rd-sae}` (but not op mask registers: `{k1}`)
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `vunpcklps xmm2{k5}{Z},xmm6,dword bcst [rax+4]`
	/// Yes | `false` | `vunpcklps xmm2{k5}{z},xmm6,dword bcst [rax+4]`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn uppercase_decorators(&self) -> bool {
		self.formatter.options().uppercase_decorators()
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
	#[wasm_bindgen(setter)]
	pub fn set_uppercase_decorators(&mut self, value: bool) {
		self.formatter.options_mut().set_uppercase_decorators(value);
	}

	/// Everything is upper cased, except numbers and their prefixes/suffixes
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `MOV EAX,GS:[RCX*4+0ffh]`
	/// Yes | `false` | `mov eax,gs:[rcx*4+0ffh]`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn uppercase_all(&self) -> bool {
		self.formatter.options().uppercase_all()
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
	#[wasm_bindgen(setter)]
	pub fn set_uppercase_all(&mut self, value: bool) {
		self.formatter.options_mut().set_uppercase_all(value);
	}

	/// Character index (0-based) where the first operand is formatted. Can be set to 0 to format it immediately after the mnemonic.
	/// At least one space or tab is always added between the mnemonic and the first operand.
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `0` | `mov•rcx,rbp`
	/// - | `8` | `mov•••••rcx,rbp`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn first_operand_char_index(&self) -> u32 {
		self.formatter.options().first_operand_char_index()
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
	#[wasm_bindgen(setter)]
	pub fn set_first_operand_char_index(&mut self, value: u32) {
		self.formatter.options_mut().set_first_operand_char_index(value);
	}

	/// Size of a tab character or 0 to use spaces
	///
	/// - Default: `0`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn tab_size(&self) -> u32 {
		self.formatter.options().tab_size()
	}

	/// Size of a tab character or 0 to use spaces
	///
	/// - Default: `0`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[wasm_bindgen(setter)]
	pub fn set_tab_size(&mut self, value: u32) {
		self.formatter.options_mut().set_tab_size(value);
	}

	/// Add a space after the operand separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov rax, rcx`
	/// Yes | `false` | `mov rax,rcx`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn space_after_operand_separator(&self) -> bool {
		self.formatter.options().space_after_operand_separator()
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
	#[wasm_bindgen(setter)]
	pub fn set_space_after_operand_separator(&mut self, value: bool) {
		self.formatter.options_mut().set_space_after_operand_separator(value);
	}

	/// Add a space between the memory expression and the brackets
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[ rcx+rdx ]`
	/// Yes | `false` | `mov eax,[rcx+rdx]`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn space_after_memory_bracket(&self) -> bool {
		self.formatter.options().space_after_memory_bracket()
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
	#[wasm_bindgen(setter)]
	pub fn set_space_after_memory_bracket(&mut self, value: bool) {
		self.formatter.options_mut().set_space_after_memory_bracket(value);
	}

	/// Add spaces between memory operand `+` and `-` operators
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rcx + rdx*8 - 80h]`
	/// Yes | `false` | `mov eax,[rcx+rdx*8-80h]`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn space_between_memory_add_operators(&self) -> bool {
		self.formatter.options().space_between_memory_add_operators()
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
	#[wasm_bindgen(setter)]
	pub fn set_space_between_memory_add_operators(&mut self, value: bool) {
		self.formatter.options_mut().set_space_between_memory_add_operators(value);
	}

	/// Add spaces between memory operand `*` operator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rcx+rdx * 8-80h]`
	/// Yes | `false` | `mov eax,[rcx+rdx*8-80h]`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn space_between_memory_mul_operators(&self) -> bool {
		self.formatter.options().space_between_memory_mul_operators()
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
	#[wasm_bindgen(setter)]
	pub fn set_space_between_memory_mul_operators(&mut self, value: bool) {
		self.formatter.options_mut().set_space_between_memory_mul_operators(value);
	}

	/// Show memory operand scale value before the index register
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[8*rdx]`
	/// Yes | `false` | `mov eax,[rdx*8]`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn scale_before_index(&self) -> bool {
		self.formatter.options().scale_before_index()
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
	#[wasm_bindgen(setter)]
	pub fn set_scale_before_index(&mut self, value: bool) {
		self.formatter.options_mut().set_scale_before_index(value);
	}

	/// Always show the scale value even if it's `*1`
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rbx+rcx*1]`
	/// Yes | `false` | `mov eax,[rbx+rcx]`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn always_show_scale(&self) -> bool {
		self.formatter.options().always_show_scale()
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
	#[wasm_bindgen(setter)]
	pub fn set_always_show_scale(&mut self, value: bool) {
		self.formatter.options_mut().set_always_show_scale(value);
	}

	/// Always show the effective segment register. If the option is `false`, only show the segment register if
	/// there's a segment override prefix.
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,ds:[ecx]`
	/// Yes | `false` | `mov eax,[ecx]`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn always_show_segment_register(&self) -> bool {
		self.formatter.options().always_show_segment_register()
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
	#[wasm_bindgen(setter)]
	pub fn set_always_show_segment_register(&mut self, value: bool) {
		self.formatter.options_mut().set_always_show_segment_register(value);
	}

	/// Show zero displacements
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rcx*2+0]`
	/// Yes | `false` | `mov eax,[rcx*2]`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn show_zero_displacements(&self) -> bool {
		self.formatter.options().show_zero_displacements()
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
	#[wasm_bindgen(setter)]
	pub fn set_show_zero_displacements(&mut self, value: bool) {
		self.formatter.options_mut().set_show_zero_displacements(value);
	}

	/// Hex number prefix or an empty string, eg. `"0x"`
	///
	/// - Default: `""` (masm/nasm/intel), `"0x"` (gas)
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn hex_prefix(&self) -> String {
		self.formatter.options().hex_prefix().to_owned()
	}

	/// Hex number prefix or an empty string, eg. `"0x"`
	///
	/// - Default: `""` (masm/nasm/intel), `"0x"` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[wasm_bindgen(setter)]
	pub fn set_hex_prefix(&mut self, value: String) {
		self.formatter.options_mut().set_hex_prefix_string(value);
	}

	/// Hex number suffix or an empty string, eg. `"h"`
	///
	/// - Default: `"h"` (masm/nasm/intel), `""` (gas)
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn hex_suffix(&self) -> String {
		self.formatter.options().hex_suffix().to_owned()
	}

	/// Hex number suffix or an empty string, eg. `"h"`
	///
	/// - Default: `"h"` (masm/nasm/intel), `""` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[wasm_bindgen(setter)]
	pub fn set_hex_suffix(&mut self, value: String) {
		self.formatter.options_mut().set_hex_suffix_string(value);
	}

	/// Size of a digit group, see also [`digit_separator()`]
	///
	/// [`digit_separator()`]: #method.digit_separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `0` | `0x12345678`
	/// Yes | `4` | `0x1234_5678`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn hex_digit_group_size(&self) -> u32 {
		self.formatter.options().hex_digit_group_size()
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
	#[wasm_bindgen(setter)]
	pub fn set_hex_digit_group_size(&mut self, value: u32) {
		self.formatter.options_mut().set_hex_digit_group_size(value);
	}

	/// Decimal number prefix or an empty string
	///
	/// - Default: `""`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn decimal_prefix(&self) -> String {
		self.formatter.options().decimal_prefix().to_owned()
	}

	/// Decimal number prefix or an empty string
	///
	/// - Default: `""`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[wasm_bindgen(setter)]
	pub fn set_decimal_prefix(&mut self, value: String) {
		self.formatter.options_mut().set_decimal_prefix_string(value);
	}

	/// Decimal number suffix or an empty string
	///
	/// - Default: `""`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn decimal_suffix(&self) -> String {
		self.formatter.options().decimal_suffix().to_owned()
	}

	/// Decimal number suffix or an empty string
	///
	/// - Default: `""`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[wasm_bindgen(setter)]
	pub fn set_decimal_suffix(&mut self, value: String) {
		self.formatter.options_mut().set_decimal_suffix_string(value);
	}

	/// Size of a digit group, see also [`digit_separator()`]
	///
	/// [`digit_separator()`]: #method.digit_separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `0` | `12345678`
	/// Yes | `3` | `12_345_678`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn decimal_digit_group_size(&self) -> u32 {
		self.formatter.options().decimal_digit_group_size()
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
	#[wasm_bindgen(setter)]
	pub fn set_decimal_digit_group_size(&mut self, value: u32) {
		self.formatter.options_mut().set_decimal_digit_group_size(value);
	}

	/// Octal number prefix or an empty string
	///
	/// - Default: `""` (masm/nasm/intel), `"0"` (gas)
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn octal_prefix(&self) -> String {
		self.formatter.options().octal_prefix().to_owned()
	}

	/// Octal number prefix or an empty string
	///
	/// - Default: `""` (masm/nasm/intel), `"0"` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[wasm_bindgen(setter)]
	pub fn set_octal_prefix(&mut self, value: String) {
		self.formatter.options_mut().set_octal_prefix_string(value);
	}

	/// Octal number suffix or an empty string
	///
	/// - Default: `"o"` (masm/nasm/intel), `""` (gas)
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn octal_suffix(&self) -> String {
		self.formatter.options().octal_suffix().to_owned()
	}

	/// Octal number suffix or an empty string
	///
	/// - Default: `"o"` (masm/nasm/intel), `""` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[wasm_bindgen(setter)]
	pub fn set_octal_suffix(&mut self, value: String) {
		self.formatter.options_mut().set_octal_suffix_string(value);
	}

	/// Size of a digit group, see also [`digit_separator()`]
	///
	/// [`digit_separator()`]: #method.digit_separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `0` | `12345670`
	/// Yes | `4` | `1234_5670`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn octal_digit_group_size(&self) -> u32 {
		self.formatter.options().octal_digit_group_size()
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
	#[wasm_bindgen(setter)]
	pub fn set_octal_digit_group_size(&mut self, value: u32) {
		self.formatter.options_mut().set_octal_digit_group_size(value);
	}

	/// Binary number prefix or an empty string
	///
	/// - Default: `""` (masm/nasm/intel), `"0b"` (gas)
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn binary_prefix(&self) -> String {
		self.formatter.options().binary_prefix().to_owned()
	}

	/// Binary number prefix or an empty string
	///
	/// - Default: `""` (masm/nasm/intel), `"0b"` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[wasm_bindgen(setter)]
	pub fn set_binary_prefix(&mut self, value: String) {
		self.formatter.options_mut().set_binary_prefix_string(value);
	}

	/// Binary number suffix or an empty string
	///
	/// - Default: `"b"` (masm/nasm/intel), `""` (gas)
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn binary_suffix(&self) -> String {
		self.formatter.options().binary_suffix().to_owned()
	}

	/// Binary number suffix or an empty string
	///
	/// - Default: `"b"` (masm/nasm/intel), `""` (gas)
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[wasm_bindgen(setter)]
	pub fn set_binary_suffix(&mut self, value: String) {
		self.formatter.options_mut().set_binary_suffix_string(value);
	}

	/// Size of a digit group, see also [`digit_separator()`]
	///
	/// [`digit_separator()`]: #method.digit_separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `0` | `11010111`
	/// Yes | `4` | `1101_0111`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn binary_digit_group_size(&self) -> u32 {
		self.formatter.options().binary_digit_group_size()
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
	#[wasm_bindgen(setter)]
	pub fn set_binary_digit_group_size(&mut self, value: u32) {
		self.formatter.options_mut().set_binary_digit_group_size(value);
	}

	/// Digit separator or an empty string. See also eg. [`hex_digit_group_size()`]
	///
	/// [`hex_digit_group_size()`]: #method.hex_digit_group_size
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `""` | `0x12345678`
	/// - | `"_"` | `0x1234_5678`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn digit_separator(&self) -> String {
		self.formatter.options().digit_separator().to_owned()
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
	#[wasm_bindgen(setter)]
	pub fn set_digit_separator(&mut self, value: String) {
		self.formatter.options_mut().set_digit_separator_string(value);
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
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn leading_zeroes(&self) -> bool {
		self.formatter.options().leading_zeroes()
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
	#[wasm_bindgen(setter)]
	pub fn set_leading_zeroes(&mut self, value: bool) {
		self.formatter.options_mut().set_leading_zeroes(value);
	}

	/// Use upper case hex digits
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `0xFF`
	/// - | `false` | `0xff`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn uppercase_hex(&self) -> bool {
		self.formatter.options().uppercase_hex()
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
	#[wasm_bindgen(setter)]
	pub fn set_uppercase_hex(&mut self, value: bool) {
		self.formatter.options_mut().set_uppercase_hex(value);
	}

	/// Small hex numbers (-9 .. 9) are shown in decimal
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `9`
	/// - | `false` | `0x9`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn small_hex_numbers_in_decimal(&self) -> bool {
		self.formatter.options().small_hex_numbers_in_decimal()
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
	#[wasm_bindgen(setter)]
	pub fn set_small_hex_numbers_in_decimal(&mut self, value: bool) {
		self.formatter.options_mut().set_small_hex_numbers_in_decimal(value);
	}

	/// Add a leading zero to hex numbers if there's no prefix and the number starts with hex digits `A-F`
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `0FFh`
	/// - | `false` | `FFh`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn add_leading_zero_to_hex_numbers(&self) -> bool {
		self.formatter.options().add_leading_zero_to_hex_numbers()
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
	#[wasm_bindgen(setter)]
	pub fn set_add_leading_zero_to_hex_numbers(&mut self, value: bool) {
		self.formatter.options_mut().set_add_leading_zero_to_hex_numbers(value);
	}

	/// Number base
	///
	/// - Default: [`Hexadecimal`]
	///
	/// [`Hexadecimal`]: enum.NumberBase.html#variant.Hexadecimal
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn number_base(&self) -> NumberBase {
		self.formatter.options().number_base()
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
	#[wasm_bindgen(setter)]
	pub fn set_number_base(&mut self, value: NumberBase) {
		self.formatter.options_mut().set_number_base(value);
	}

	/// Add leading zeroes to branch offsets. Used by `CALL NEAR`, `CALL FAR`, `JMP NEAR`, `JMP FAR`, `Jcc`, `LOOP`, `LOOPcc`, `XBEGIN`
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `je 00000123h`
	/// - | `false` | `je 123h`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn branch_leading_zeroes(&self) -> bool {
		self.formatter.options().branch_leading_zeroes()
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
	#[wasm_bindgen(setter)]
	pub fn set_branch_leading_zeroes(&mut self, value: bool) {
		self.formatter.options_mut().set_branch_leading_zeroes(value);
	}

	/// Show immediate operands as signed numbers
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,-1`
	/// Yes | `false` | `mov eax,FFFFFFFF`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn signed_immediate_operands(&self) -> bool {
		self.formatter.options().signed_immediate_operands()
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
	#[wasm_bindgen(setter)]
	pub fn set_signed_immediate_operands(&mut self, value: bool) {
		self.formatter.options_mut().set_signed_immediate_operands(value);
	}

	/// Displacements are signed numbers
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `mov al,[eax-2000h]`
	/// - | `false` | `mov al,[eax+0FFFFE000h]`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn signed_memory_displacements(&self) -> bool {
		self.formatter.options().signed_memory_displacements()
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
	#[wasm_bindgen(setter)]
	pub fn set_signed_memory_displacements(&mut self, value: bool) {
		self.formatter.options_mut().set_signed_memory_displacements(value);
	}

	/// Add leading zeroes to displacements
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov al,[eax+00000012h]`
	/// Yes | `false` | `mov al,[eax+12h]`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn displacement_leading_zeroes(&self) -> bool {
		self.formatter.options().displacement_leading_zeroes()
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
	#[wasm_bindgen(setter)]
	pub fn set_displacement_leading_zeroes(&mut self, value: bool) {
		self.formatter.options_mut().set_displacement_leading_zeroes(value);
	}

	/// Options that control if the memory size (eg. `DWORD PTR`) is shown or not.
	/// This is ignored by the gas (AT&T) formatter.
	///
	/// - Default: [`Default`]
	///
	/// [`Default`]: enum.MemorySizeOptions.html#variant.Default
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn memory_size_options(&self) -> MemorySizeOptions {
		self.formatter.options().memory_size_options()
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
	#[wasm_bindgen(setter)]
	pub fn set_memory_size_options(&mut self, value: MemorySizeOptions) {
		self.formatter.options_mut().set_memory_size_options(value);
	}

	/// Show `RIP+displ` or the virtual address
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rip+12345678h]`
	/// Yes | `false` | `mov eax,[1029384756AFBECDh]`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn rip_relative_addresses(&self) -> bool {
		self.formatter.options().rip_relative_addresses()
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
	#[wasm_bindgen(setter)]
	pub fn set_rip_relative_addresses(&mut self, value: bool) {
		self.formatter.options_mut().set_rip_relative_addresses(value);
	}

	/// Show `NEAR`, `SHORT`, etc if it's a branch instruction
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `je short 1234h`
	/// - | `false` | `je 1234h`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn show_branch_size(&self) -> bool {
		self.formatter.options().show_branch_size()
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
	#[wasm_bindgen(setter)]
	pub fn set_show_branch_size(&mut self, value: bool) {
		self.formatter.options_mut().set_show_branch_size(value);
	}

	/// Use pseudo instructions
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `vcmpnltsd xmm2,xmm6,xmm3`
	/// - | `false` | `vcmpsd xmm2,xmm6,xmm3,5`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn use_pseudo_ops(&self) -> bool {
		self.formatter.options().use_pseudo_ops()
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
	#[wasm_bindgen(setter)]
	pub fn set_use_pseudo_ops(&mut self, value: bool) {
		self.formatter.options_mut().set_use_pseudo_ops(value);
	}

	/// Show the original value after the symbol name
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[myfield (12345678)]`
	/// Yes | `false` | `mov eax,[myfield]`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn show_symbol_address(&self) -> bool {
		self.formatter.options().show_symbol_address()
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
	#[wasm_bindgen(setter)]
	pub fn set_show_symbol_address(&mut self, value: bool) {
		self.formatter.options_mut().set_show_symbol_address(value);
	}

	/// (gas only): If `true`, the formatter doesn't add `%` to registers
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,ecx`
	/// Yes | `false` | `mov %eax,%ecx`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn gas_naked_registers(&self) -> bool {
		self.formatter.options().gas_naked_registers()
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
	#[wasm_bindgen(setter)]
	pub fn set_gas_naked_registers(&mut self, value: bool) {
		self.formatter.options_mut().set_gas_naked_registers(value);
	}

	/// (gas only): Shows the mnemonic size suffix even when not needed
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `movl %eax,%ecx`
	/// Yes | `false` | `mov %eax,%ecx`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn gas_show_mnemonic_size_suffix(&self) -> bool {
		self.formatter.options().gas_show_mnemonic_size_suffix()
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
	#[wasm_bindgen(setter)]
	pub fn set_gas_show_mnemonic_size_suffix(&mut self, value: bool) {
		self.formatter.options_mut().set_gas_show_mnemonic_size_suffix(value);
	}

	/// (gas only): Add a space after the comma if it's a memory operand
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `(%eax, %ecx, 2)`
	/// Yes | `false` | `(%eax,%ecx,2)`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn gas_space_after_memory_operand_comma(&self) -> bool {
		self.formatter.options().gas_space_after_memory_operand_comma()
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
	#[wasm_bindgen(setter)]
	pub fn set_gas_space_after_memory_operand_comma(&mut self, value: bool) {
		self.formatter.options_mut().set_gas_space_after_memory_operand_comma(value);
	}

	/// (masm only): Add a `DS` segment override even if it's not present. Used if it's 16/32-bit code and mem op is a displ
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `mov eax,ds:[12345678]`
	/// - | `false` | `mov eax,[12345678]`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn masm_add_ds_prefix32(&self) -> bool {
		self.formatter.options().masm_add_ds_prefix32()
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
	#[wasm_bindgen(setter)]
	pub fn set_masm_add_ds_prefix32(&mut self, value: bool) {
		self.formatter.options_mut().set_masm_add_ds_prefix32(value);
	}

	/// (masm only): Show symbols in brackets
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `[ecx+symbol]` / `[symbol]`
	/// - | `false` | `symbol[ecx]` / `symbol`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn masm_symbol_displ_in_brackets(&self) -> bool {
		self.formatter.options().masm_symbol_displ_in_brackets()
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
	#[wasm_bindgen(setter)]
	pub fn set_masm_symbol_displ_in_brackets(&mut self, value: bool) {
		self.formatter.options_mut().set_masm_symbol_displ_in_brackets(value);
	}

	/// (masm only): Show displacements in brackets
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `[ecx+1234h]`
	/// - | `false` | `1234h[ecx]`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn masm_displ_in_brackets(&self) -> bool {
		self.formatter.options().masm_displ_in_brackets()
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
	#[wasm_bindgen(setter)]
	pub fn set_masm_displ_in_brackets(&mut self, value: bool) {
		self.formatter.options_mut().set_masm_displ_in_brackets(value);
	}

	/// (nasm only): Shows `BYTE`, `WORD`, `DWORD` or `QWORD` if it's a sign extended immediate operand value
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `or rcx,byte -1`
	/// Yes | `false` | `or rcx,-1`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn nasm_show_sign_extended_immediate_size(&self) -> bool {
		self.formatter.options().nasm_show_sign_extended_immediate_size()
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
	#[wasm_bindgen(setter)]
	pub fn set_nasm_show_sign_extended_immediate_size(&mut self, value: bool) {
		self.formatter.options_mut().set_nasm_show_sign_extended_immediate_size(value);
	}

	/// Use `st(0)` instead of `st` if `st` can be used. Ignored by the nasm formatter.
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `fadd st(0),st(3)`
	/// Yes | `false` | `fadd st,st(3)`
	#[must_use]
	#[wasm_bindgen(getter)]
	pub fn prefer_st0(&self) -> bool {
		self.formatter.options().prefer_st0()
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
	#[wasm_bindgen(setter)]
	pub fn set_prefer_st0(&mut self, value: bool) {
		self.formatter.options_mut().set_prefer_st0(value);
	}
}
