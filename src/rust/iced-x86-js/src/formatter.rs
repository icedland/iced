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

#![allow(non_snake_case)]

use super::instruction::Instruction;
use super::memory_size_options::{iced_to_memory_size_options, memory_size_options_to_iced, MemorySizeOptions};
use super::number_base::{iced_to_number_base, number_base_to_iced, NumberBase};
#[cfg(feature = "instr_info")]
use super::op_access::{iced_to_op_access, OpAccess};
#[cfg(feature = "instruction_api")]
use super::register::{register_to_iced, Register};
#[cfg(feature = "gas")]
use iced_x86::GasFormatter;
#[cfg(feature = "intel")]
use iced_x86::IntelFormatter;
#[cfg(feature = "masm")]
use iced_x86::MasmFormatter;
#[cfg(feature = "nasm")]
use iced_x86::NasmFormatter;
use wasm_bindgen::prelude::*;

/// Formatter syntax (GNU Assembler, Intel XED, masm, nasm)
#[wasm_bindgen]
pub enum FormatterSyntax {
	/// GNU Assembler (AT&T)
	Gas,
	/// Intel XED
	Intel,
	/// masm
	Masm,
	/// nasm
	Nasm,
}

/// X86 formatter that supports GNU Assembler, Intel XED, masm and nasm syntax
#[wasm_bindgen]
#[allow(missing_debug_implementations)]
pub struct Formatter {
	formatter: Box<dyn iced_x86::Formatter>,
}

#[wasm_bindgen]
impl Formatter {
	/// Creates an x86 formatter
	///
	/// # Arguments
	///
	/// * `syntax`: Formatter syntax, see [`FormatterSyntax`]
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
	/// let mut formatter = Formatter::new(FormatterSyntax::Masm);
	/// formatter.options_mut().set_uppercase_mnemonics(true);
	/// formatter.format(&instr, &mut output);
	/// assert_eq!("VCVTNE2PS2BF16 zmm2{k5}{z},zmm6,dword bcst [rax+4]", output);
	/// ```
	///
	/// [`FormatterSyntax`]: enum.FormatterSyntax.html
	#[wasm_bindgen(constructor)]
	pub fn new(syntax: FormatterSyntax) -> Self {
		let formatter: Box<dyn iced_x86::Formatter> = match syntax {
			#[cfg(feature = "gas")]
			FormatterSyntax::Gas => Box::new(GasFormatter::new()),
			#[cfg(feature = "intel")]
			FormatterSyntax::Intel => Box::new(IntelFormatter::new()),
			#[cfg(feature = "masm")]
			FormatterSyntax::Masm => Box::new(MasmFormatter::new()),
			#[cfg(feature = "nasm")]
			FormatterSyntax::Nasm => Box::new(NasmFormatter::new()),
			#[allow(unreachable_patterns)]
			_ => panic!(),
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
		self.formatter.format(&instruction.0, &mut output);
		output
	}

	/// Formats the mnemonic and any prefixes
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	#[wasm_bindgen(js_name = "formatMnemonic")]
	pub fn format_mnemonic(&mut self, instruction: &Instruction) -> String {
		let mut output = String::new();
		self.formatter.format_mnemonic(&instruction.0, &mut output);
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
	#[wasm_bindgen(js_name = "formatMnemonicOptions")]
	pub fn format_mnemonic_options(&mut self, instruction: &Instruction, options: u32) -> String {
		let mut output = String::new();
		self.formatter.format_mnemonic_options(&instruction.0, &mut output, options);
		output
	}

	/// Gets the number of operands that will be formatted. A formatter can add and remove operands
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	#[wasm_bindgen(js_name = "operandCount")]
	pub fn operand_count(&mut self, instruction: &Instruction) -> u32 {
		self.formatter.operand_count(&instruction.0)
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
	#[wasm_bindgen(js_name = "opAccess")]
	pub fn op_access(&mut self, instruction: &Instruction, operand: u32) -> Option<OpAccess> {
		self.formatter.op_access(&instruction.0, operand).map(iced_to_op_access)
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
	#[wasm_bindgen(js_name = "getInstructionOperand")]
	pub fn get_instruction_operand(&mut self, instruction: &Instruction, operand: u32) -> Option<u32> {
		self.formatter.get_instruction_operand(&instruction.0, operand)
	}

	/// Converts an instruction operand index to a formatter operand index. Returns `None` if the instruction operand isn't used by the formatter
	///
	/// # Panics
	///
	/// Panics if `instructionOperand` is invalid
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	/// - `instructionOperand`: Instruction operand
	#[wasm_bindgen(js_name = "getFormatterOperand")]
	pub fn get_formatter_operand(&mut self, instruction: &Instruction, instructionOperand: u32) -> Option<u32> {
		self.formatter.get_formatter_operand(&instruction.0, instructionOperand)
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
	#[wasm_bindgen(js_name = "formatOperand")]
	pub fn format_operand(&mut self, instruction: &Instruction, operand: u32) -> String {
		let mut output = String::new();
		self.formatter.format_operand(&instruction.0, &mut output, operand);
		output
	}

	/// Formats an operand separator
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	#[wasm_bindgen(js_name = "formatOperandSeparator")]
	pub fn format_operand_separator(&mut self, instruction: &Instruction) -> String {
		let mut output = String::new();
		self.formatter.format_operand_separator(&instruction.0, &mut output);
		output
	}

	/// Formats all operands
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	#[wasm_bindgen(js_name = "formatAllOperands")]
	pub fn format_all_operands(&mut self, instruction: &Instruction) -> String {
		let mut output = String::new();
		self.formatter.format_all_operands(&instruction.0, &mut output);
		output
	}

	/// Formats a register
	///
	/// # Arguments
	///
	/// - `register`: Register
	#[wasm_bindgen(js_name = "formatRegister")]
	// This adds the Register enum to the js/ts files, but this API won't be called often so disable it by default.
	#[cfg(feature = "instruction_api")]
	pub fn format_register(&mut self, register: Register) -> String {
		self.formatter.format_register(register_to_iced(register)).to_owned()
	}

	/// Formats a `i8`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[wasm_bindgen(js_name = "formatI8")]
	pub fn format_i8(&mut self, value: i8) -> String {
		self.formatter.format_i8(value).to_owned()
	}

	/// Formats a `i16`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[wasm_bindgen(js_name = "formatI16")]
	pub fn format_i16(&mut self, value: i16) -> String {
		self.formatter.format_i16(value).to_owned()
	}

	/// Formats a `i32`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[wasm_bindgen(js_name = "formatI32")]
	pub fn format_i32(&mut self, value: i32) -> String {
		self.formatter.format_i32(value).to_owned()
	}

	/// Formats a `i64`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[wasm_bindgen(js_name = "formatI64")]
	pub fn format_i64(&mut self, value: i64) -> String {
		self.formatter.format_i64(value).to_owned()
	}

	/// Formats a `u8`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[wasm_bindgen(js_name = "formatU8")]
	pub fn format_u8(&mut self, value: u8) -> String {
		self.formatter.format_u8(value).to_owned()
	}

	/// Formats a `u16`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[wasm_bindgen(js_name = "formatU16")]
	pub fn format_u16(&mut self, value: u16) -> String {
		self.formatter.format_u16(value).to_owned()
	}

	/// Formats a `u32`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[wasm_bindgen(js_name = "formatU32")]
	pub fn format_u32(&mut self, value: u32) -> String {
		self.formatter.format_u32(value).to_owned()
	}

	/// Formats a `u64`
	///
	/// # Arguments
	///
	/// - `value`: Value
	#[wasm_bindgen(js_name = "formatU64")]
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
	#[wasm_bindgen(getter)]
	pub fn uppercasePrefixes(&self) -> bool {
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
	pub fn set_uppercasePrefixes(&mut self, value: bool) {
		self.formatter.options_mut().set_uppercase_prefixes(value);
	}

	/// Mnemonics are upper cased
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `MOV rcx,rax`
	/// Yes | `false` | `mov rcx,rax`
	#[wasm_bindgen(getter)]
	pub fn uppercaseMnemonics(&self) -> bool {
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
	pub fn set_uppercaseMnemonics(&mut self, value: bool) {
		self.formatter.options_mut().set_uppercase_mnemonics(value);
	}

	/// Registers are upper cased
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov RCX,[RAX+RDX*8]`
	/// Yes | `false` | `mov rcx,[rax+rdx*8]`
	#[wasm_bindgen(getter)]
	pub fn uppercaseRegisters(&self) -> bool {
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
	pub fn set_uppercaseRegisters(&mut self, value: bool) {
		self.formatter.options_mut().set_uppercase_registers(value);
	}

	/// Keywords are upper cased (eg. `BYTE PTR`, `SHORT`)
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov BYTE PTR [rcx],12h`
	/// Yes | `false` | `mov byte ptr [rcx],12h`
	#[wasm_bindgen(getter)]
	pub fn uppercaseKeywords(&self) -> bool {
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
	pub fn set_uppercaseKeywords(&mut self, value: bool) {
		self.formatter.options_mut().set_uppercase_keywords(value);
	}

	/// Upper case decorators, eg. `{z}`, `{sae}`, `{rd-sae}` (but not op mask registers: `{k1}`)
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `vunpcklps xmm2{k5}{Z},xmm6,dword bcst [rax+4]`
	/// Yes | `false` | `vunpcklps xmm2{k5}{z},xmm6,dword bcst [rax+4]`
	#[wasm_bindgen(getter)]
	pub fn uppercaseDecorators(&self) -> bool {
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
	pub fn set_uppercaseDecorators(&mut self, value: bool) {
		self.formatter.options_mut().set_uppercase_decorators(value);
	}

	/// Everything is upper cased, except numbers and their prefixes/suffixes
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `MOV EAX,GS:[RCX*4+0ffh]`
	/// Yes | `false` | `mov eax,gs:[rcx*4+0ffh]`
	#[wasm_bindgen(getter)]
	pub fn uppercaseAll(&self) -> bool {
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
	pub fn set_uppercaseAll(&mut self, value: bool) {
		self.formatter.options_mut().set_uppercase_all(value);
	}

	/// Character index (0-based) where the first operand is formatted. Can be set to 0 to format it immediately after the mnemonic.
	/// At least one space or tab is always added between the mnemonic and the first operand.
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `0` | `mov•rcx,rbp`
	/// - | `8` | `mov•••••rcx,rbp`
	#[wasm_bindgen(getter)]
	pub fn firstOperandCharIndex(&self) -> u32 {
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
	pub fn set_firstOperandCharIndex(&mut self, value: u32) {
		self.formatter.options_mut().set_first_operand_char_index(value);
	}

	/// Size of a tab character or 0 to use spaces
	///
	/// - Default: `0`
	#[wasm_bindgen(getter)]
	pub fn tabSize(&self) -> u32 {
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
	pub fn set_tabSize(&mut self, value: u32) {
		self.formatter.options_mut().set_tab_size(value);
	}

	/// Add a space after the operand separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov rax, rcx`
	/// Yes | `false` | `mov rax,rcx`
	#[wasm_bindgen(getter)]
	pub fn spaceAfterOperandSeparator(&self) -> bool {
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
	pub fn set_spaceAfterOperandSeparator(&mut self, value: bool) {
		self.formatter.options_mut().set_space_after_operand_separator(value);
	}

	/// Add a space between the memory expression and the brackets
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[ rcx+rdx ]`
	/// Yes | `false` | `mov eax,[rcx+rdx]`
	#[wasm_bindgen(getter)]
	pub fn spaceAfterMemoryBracket(&self) -> bool {
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
	pub fn set_spaceAfterMemoryBracket(&mut self, value: bool) {
		self.formatter.options_mut().set_space_after_memory_bracket(value);
	}

	/// Add spaces between memory operand `+` and `-` operators
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rcx + rdx*8 - 80h]`
	/// Yes | `false` | `mov eax,[rcx+rdx*8-80h]`
	#[wasm_bindgen(getter)]
	pub fn spaceBetweenMemoryAddOperators(&self) -> bool {
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
	pub fn set_spaceBetweenMemoryAddOperators(&mut self, value: bool) {
		self.formatter.options_mut().set_space_between_memory_add_operators(value);
	}

	/// Add spaces between memory operand `*` operator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rcx+rdx * 8-80h]`
	/// Yes | `false` | `mov eax,[rcx+rdx*8-80h]`
	#[wasm_bindgen(getter)]
	pub fn spaceBetweenMemoryMulOperators(&self) -> bool {
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
	pub fn set_spaceBetweenMemoryMulOperators(&mut self, value: bool) {
		self.formatter.options_mut().set_space_between_memory_mul_operators(value);
	}

	/// Show memory operand scale value before the index register
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[8*rdx]`
	/// Yes | `false` | `mov eax,[rdx*8]`
	#[wasm_bindgen(getter)]
	pub fn scaleBeforeIndex(&self) -> bool {
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
	pub fn set_scaleBeforeIndex(&mut self, value: bool) {
		self.formatter.options_mut().set_scale_before_index(value);
	}

	/// Always show the scale value even if it's `*1`
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rbx+rcx*1]`
	/// Yes | `false` | `mov eax,[rbx+rcx]`
	#[wasm_bindgen(getter)]
	pub fn alwaysShowScale(&self) -> bool {
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
	pub fn set_alwaysShowScale(&mut self, value: bool) {
		self.formatter.options_mut().set_always_show_scale(value);
	}

	/// Always show the effective segment register. If the option is `false`, only show the segment register if
	/// there's a segment override prefix.
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,ds:[ecx]`
	/// Yes | `false` | `mov eax,[ecx]`
	#[wasm_bindgen(getter)]
	pub fn alwaysShowSegmentRegister(&self) -> bool {
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
	pub fn set_alwaysShowSegmentRegister(&mut self, value: bool) {
		self.formatter.options_mut().set_always_show_segment_register(value);
	}

	/// Show zero displacements
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rcx*2+0]`
	/// Yes | `false` | `mov eax,[rcx*2]`
	#[wasm_bindgen(getter)]
	pub fn showZeroDisplacements(&self) -> bool {
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
	pub fn set_showZeroDisplacements(&mut self, value: bool) {
		self.formatter.options_mut().set_show_zero_displacements(value);
	}

	/// Hex number prefix or an empty string, eg. `"0x"`
	///
	/// - Default: `""` (masm/nasm/intel), `"0x"` (gas)
	#[wasm_bindgen(getter)]
	pub fn hexPrefix(&self) -> String {
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
	pub fn set_hexPrefix(&mut self, value: String) {
		self.formatter.options_mut().set_hex_prefix_string(value);
	}

	/// Hex number suffix or an empty string, eg. `"h"`
	///
	/// - Default: `"h"` (masm/nasm/intel), `""` (gas)
	#[wasm_bindgen(getter)]
	pub fn hexSuffix(&self) -> String {
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
	pub fn set_hexSuffix(&mut self, value: String) {
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
	#[wasm_bindgen(getter)]
	pub fn hexDigitGroupSize(&self) -> u32 {
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
	pub fn set_hexDigitGroupSize(&mut self, value: u32) {
		self.formatter.options_mut().set_hex_digit_group_size(value);
	}

	/// Decimal number prefix or an empty string
	///
	/// - Default: `""`
	#[wasm_bindgen(getter)]
	pub fn decimalPrefix(&self) -> String {
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
	pub fn set_decimalPrefix(&mut self, value: String) {
		self.formatter.options_mut().set_decimal_prefix_string(value);
	}

	/// Decimal number suffix or an empty string
	///
	/// - Default: `""`
	#[wasm_bindgen(getter)]
	pub fn decimalSuffix(&self) -> String {
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
	pub fn set_decimalSuffix(&mut self, value: String) {
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
	#[wasm_bindgen(getter)]
	pub fn decimalDigitGroupSize(&self) -> u32 {
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
	pub fn set_decimalDigitGroupSize(&mut self, value: u32) {
		self.formatter.options_mut().set_decimal_digit_group_size(value);
	}

	/// Octal number prefix or an empty string
	///
	/// - Default: `""` (masm/nasm/intel), `"0"` (gas)
	#[wasm_bindgen(getter)]
	pub fn octalPrefix(&self) -> String {
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
	pub fn set_octalPrefix(&mut self, value: String) {
		self.formatter.options_mut().set_octal_prefix_string(value);
	}

	/// Octal number suffix or an empty string
	///
	/// - Default: `"o"` (masm/nasm/intel), `""` (gas)
	#[wasm_bindgen(getter)]
	pub fn octalSuffix(&self) -> String {
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
	pub fn set_octalSuffix(&mut self, value: String) {
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
	#[wasm_bindgen(getter)]
	pub fn octalDigitGroupSize(&self) -> u32 {
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
	pub fn set_octalDigitGroupSize(&mut self, value: u32) {
		self.formatter.options_mut().set_octal_digit_group_size(value);
	}

	/// Binary number prefix or an empty string
	///
	/// - Default: `""` (masm/nasm/intel), `"0b"` (gas)
	#[wasm_bindgen(getter)]
	pub fn binaryPrefix(&self) -> String {
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
	pub fn set_binaryPrefix(&mut self, value: String) {
		self.formatter.options_mut().set_binary_prefix_string(value);
	}

	/// Binary number suffix or an empty string
	///
	/// - Default: `"b"` (masm/nasm/intel), `""` (gas)
	#[wasm_bindgen(getter)]
	pub fn binarySuffix(&self) -> String {
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
	pub fn set_binarySuffix(&mut self, value: String) {
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
	#[wasm_bindgen(getter)]
	pub fn binaryDigitGroupSize(&self) -> u32 {
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
	pub fn set_binaryDigitGroupSize(&mut self, value: u32) {
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
	#[wasm_bindgen(getter)]
	pub fn digitSeparator(&self) -> String {
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
	pub fn set_digitSeparator(&mut self, value: String) {
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
	#[wasm_bindgen(getter)]
	pub fn leadingZeroes(&self) -> bool {
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
	pub fn set_leadingZeroes(&mut self, value: bool) {
		self.formatter.options_mut().set_leading_zeroes(value);
	}

	/// Use upper case hex digits
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `0xFF`
	/// - | `false` | `0xff`
	#[wasm_bindgen(getter)]
	pub fn uppercaseHex(&self) -> bool {
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
	pub fn set_uppercaseHex(&mut self, value: bool) {
		self.formatter.options_mut().set_uppercase_hex(value);
	}

	/// Small hex numbers (-9 .. 9) are shown in decimal
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `9`
	/// - | `false` | `0x9`
	#[wasm_bindgen(getter)]
	pub fn smallHexNumbersInDecimal(&self) -> bool {
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
	pub fn set_smallHexNumbersInDecimal(&mut self, value: bool) {
		self.formatter.options_mut().set_small_hex_numbers_in_decimal(value);
	}

	/// Add a leading zero to hex numbers if there's no prefix and the number starts with hex digits `A-F`
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `0FFh`
	/// - | `false` | `FFh`
	#[wasm_bindgen(getter)]
	pub fn addLeadingZeroToHexNumbers(&self) -> bool {
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
	pub fn set_addLeadingZeroToHexNumbers(&mut self, value: bool) {
		self.formatter.options_mut().set_add_leading_zero_to_hex_numbers(value);
	}

	/// Number base
	///
	/// - Default: [`Hexadecimal`]
	///
	/// [`Hexadecimal`]: enum.NumberBase.html#variant.Hexadecimal
	#[wasm_bindgen(getter)]
	pub fn numberBase(&self) -> NumberBase {
		iced_to_number_base(self.formatter.options().number_base())
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
	pub fn set_numberBase(&mut self, value: NumberBase) {
		self.formatter.options_mut().set_number_base(number_base_to_iced(value));
	}

	/// Add leading zeroes to branch offsets. Used by `CALL NEAR`, `CALL FAR`, `JMP NEAR`, `JMP FAR`, `Jcc`, `LOOP`, `LOOPcc`, `XBEGIN`
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `je 00000123h`
	/// - | `false` | `je 123h`
	#[wasm_bindgen(getter)]
	pub fn branchLeadingZeroes(&self) -> bool {
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
	pub fn set_branchLeadingZeroes(&mut self, value: bool) {
		self.formatter.options_mut().set_branch_leading_zeroes(value);
	}

	/// Show immediate operands as signed numbers
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,-1`
	/// Yes | `false` | `mov eax,FFFFFFFF`
	#[wasm_bindgen(getter)]
	pub fn signedImmediateOperands(&self) -> bool {
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
	pub fn set_signedImmediateOperands(&mut self, value: bool) {
		self.formatter.options_mut().set_signed_immediate_operands(value);
	}

	/// Displacements are signed numbers
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `mov al,[eax-2000h]`
	/// - | `false` | `mov al,[eax+0FFFFE000h]`
	#[wasm_bindgen(getter)]
	pub fn signedMemoryDisplacements(&self) -> bool {
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
	pub fn set_signedMemoryDisplacements(&mut self, value: bool) {
		self.formatter.options_mut().set_signed_memory_displacements(value);
	}

	/// Add leading zeroes to displacements
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov al,[eax+00000012h]`
	/// Yes | `false` | `mov al,[eax+12h]`
	#[wasm_bindgen(getter)]
	pub fn displacementLeadingZeroes(&self) -> bool {
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
	pub fn set_displacementLeadingZeroes(&mut self, value: bool) {
		self.formatter.options_mut().set_displacement_leading_zeroes(value);
	}

	/// Options that control if the memory size (eg. `DWORD PTR`) is shown or not.
	/// This is ignored by the gas (AT&T) formatter.
	///
	/// - Default: [`Default`]
	///
	/// [`Default`]: enum.MemorySizeOptions.html#variant.Default
	#[wasm_bindgen(getter)]
	pub fn memorySizeOptions(&self) -> MemorySizeOptions {
		iced_to_memory_size_options(self.formatter.options().memory_size_options())
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
	pub fn set_memorySizeOptions(&mut self, value: MemorySizeOptions) {
		self.formatter.options_mut().set_memory_size_options(memory_size_options_to_iced(value));
	}

	/// Show `RIP+displ` or the virtual address
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[rip+12345678h]`
	/// Yes | `false` | `mov eax,[1029384756AFBECDh]`
	#[wasm_bindgen(getter)]
	pub fn ripRelativeAddresses(&self) -> bool {
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
	pub fn set_ripRelativeAddresses(&mut self, value: bool) {
		self.formatter.options_mut().set_rip_relative_addresses(value);
	}

	/// Show `NEAR`, `SHORT`, etc if it's a branch instruction
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `je short 1234h`
	/// - | `false` | `je 1234h`
	#[wasm_bindgen(getter)]
	pub fn showBranchSize(&self) -> bool {
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
	pub fn set_showBranchSize(&mut self, value: bool) {
		self.formatter.options_mut().set_show_branch_size(value);
	}

	/// Use pseudo instructions
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `vcmpnltsd xmm2,xmm6,xmm3`
	/// - | `false` | `vcmpsd xmm2,xmm6,xmm3,5`
	#[wasm_bindgen(getter)]
	pub fn usePseudoOps(&self) -> bool {
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
	pub fn set_usePseudoOps(&mut self, value: bool) {
		self.formatter.options_mut().set_use_pseudo_ops(value);
	}

	/// Show the original value after the symbol name
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,[myfield (12345678)]`
	/// Yes | `false` | `mov eax,[myfield]`
	#[wasm_bindgen(getter)]
	pub fn showSymbolAddress(&self) -> bool {
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
	pub fn set_showSymbolAddress(&mut self, value: bool) {
		self.formatter.options_mut().set_show_symbol_address(value);
	}

	/// (gas only): If `true`, the formatter doesn't add `%` to registers
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `mov eax,ecx`
	/// Yes | `false` | `mov %eax,%ecx`
	#[wasm_bindgen(getter)]
	pub fn gasNakedRegisters(&self) -> bool {
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
	pub fn set_gasNakedRegisters(&mut self, value: bool) {
		self.formatter.options_mut().set_gas_naked_registers(value);
	}

	/// (gas only): Shows the mnemonic size suffix even when not needed
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `movl %eax,%ecx`
	/// Yes | `false` | `mov %eax,%ecx`
	#[wasm_bindgen(getter)]
	pub fn gasShowMnemonicSizeSuffix(&self) -> bool {
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
	pub fn set_gasShowMnemonicSizeSuffix(&mut self, value: bool) {
		self.formatter.options_mut().set_gas_show_mnemonic_size_suffix(value);
	}

	/// (gas only): Add a space after the comma if it's a memory operand
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `(%eax, %ecx, 2)`
	/// Yes | `false` | `(%eax,%ecx,2)`
	#[wasm_bindgen(getter)]
	pub fn gasSpaceAfterMemoryOperandComma(&self) -> bool {
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
	pub fn set_gasSpaceAfterMemoryOperandComma(&mut self, value: bool) {
		self.formatter.options_mut().set_gas_space_after_memory_operand_comma(value);
	}

	/// (masm only): Add a `DS` segment override even if it's not present. Used if it's 16/32-bit code and mem op is a displ
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `mov eax,ds:[12345678]`
	/// - | `false` | `mov eax,[12345678]`
	#[wasm_bindgen(getter)]
	pub fn masmAddDsPrefix32(&self) -> bool {
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
	pub fn set_masmAddDsPrefix32(&mut self, value: bool) {
		self.formatter.options_mut().set_masm_add_ds_prefix32(value);
	}

	/// (masm only): Show symbols in brackets
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `[ecx+symbol]` / `[symbol]`
	/// - | `false` | `symbol[ecx]` / `symbol`
	#[wasm_bindgen(getter)]
	pub fn masmSymbolDisplInBrackets(&self) -> bool {
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
	pub fn set_masmSymbolDisplInBrackets(&mut self, value: bool) {
		self.formatter.options_mut().set_masm_symbol_displ_in_brackets(value);
	}

	/// (masm only): Show displacements in brackets
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// Yes | `true` | `[ecx+1234h]`
	/// - | `false` | `1234h[ecx]`
	#[wasm_bindgen(getter)]
	pub fn masmDisplInBrackets(&self) -> bool {
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
	pub fn set_masmDisplInBrackets(&mut self, value: bool) {
		self.formatter.options_mut().set_masm_displ_in_brackets(value);
	}

	/// (nasm only): Shows `BYTE`, `WORD`, `DWORD` or `QWORD` if it's a sign extended immediate operand value
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `or rcx,byte -1`
	/// Yes | `false` | `or rcx,-1`
	#[wasm_bindgen(getter)]
	pub fn nasmShowSignExtendedImmediateSize(&self) -> bool {
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
	pub fn set_nasmShowSignExtendedImmediateSize(&mut self, value: bool) {
		self.formatter.options_mut().set_nasm_show_sign_extended_immediate_size(value);
	}

	/// Use `st(0)` instead of `st` if `st` can be used. Ignored by the nasm formatter.
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// - | `true` | `fadd st(0),st(3)`
	/// Yes | `false` | `fadd st,st(3)`
	#[wasm_bindgen(getter)]
	pub fn preferSt0(&self) -> bool {
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
	pub fn set_preferSt0(&mut self, value: bool) {
		self.formatter.options_mut().set_prefer_st0(value);
	}
}
