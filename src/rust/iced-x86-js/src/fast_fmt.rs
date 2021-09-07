// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::instruction::Instruction;
use wasm_bindgen::prelude::*;

/// x86 formatter that uses less code (smaller wasm files)
#[wasm_bindgen]
pub struct FastFormatter(iced_x86_rust::FastFormatter);

#[wasm_bindgen]
impl FastFormatter {
	/// Creates an x86 formatter
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Decoder, DecoderOptions, FastFormatter } = require("iced-x86");
	///
	/// const bytes = new Uint8Array([0x62, 0xF2, 0x4F, 0xDD, 0x72, 0x50, 0x01]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	/// const instr = decoder.decode();
	///
	/// const formatter = new FastFormatter();
	/// formatter.spaceAfterOperandSeparator = true;
	/// const disasm = formatter.format(instr);
	/// assert.equal(disasm, "vcvtne2ps2bf16 zmm2{k5}{z}, zmm6, dword bcst [rax+4h]");
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// formatter.free();
	/// ```
	///
	/// [`FormatterSyntax`]: enum.FormatterSyntax.html
	#[allow(clippy::new_without_default)]
	#[wasm_bindgen(constructor)]
	pub fn new() -> Self {
		Self(iced_x86_rust::FastFormatter::new())
	}

	/// Formats the whole instruction: prefixes, mnemonic, operands
	///
	/// # Arguments
	///
	/// - `instruction`: Instruction
	#[wasm_bindgen]
	pub fn format(&mut self, instruction: &Instruction) -> String {
		let mut output = String::new();
		self.0.format(&instruction.0, &mut output);
		output
	}

	// NOTE: These tables must render correctly by `cargo doc` and inside of IDEs, eg. VSCode.

	/// Add a space after the operand separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// _ | `true` | `mov rax, rcx`
	/// 👍 | `false` | `mov rax,rcx`
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "spaceAfterOperandSeparator")]
	pub fn space_after_operand_separator(&self) -> bool {
		self.0.options().space_after_operand_separator()
	}

	/// Add a space after the operand separator
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// _ | `true` | `mov rax, rcx`
	/// 👍 | `false` | `mov rax,rcx`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "spaceAfterOperandSeparator")]
	pub fn set_space_after_operand_separator(&mut self, value: bool) {
		self.0.options_mut().set_space_after_operand_separator(value);
	}

	/// Show `RIP+displ` or the virtual address
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// _ | `true` | `mov eax,[rip+12345678h]`
	/// 👍 | `false` | `mov eax,[1029384756AFBECDh]`
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "ripRelativeAddresses")]
	pub fn rip_relative_addresses(&self) -> bool {
		self.0.options().rip_relative_addresses()
	}

	/// Show `RIP+displ` or the virtual address
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// _ | `true` | `mov eax,[rip+12345678h]`
	/// 👍 | `false` | `mov eax,[1029384756AFBECDh]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "ripRelativeAddresses")]
	pub fn set_rip_relative_addresses(&mut self, value: bool) {
		self.0.options_mut().set_rip_relative_addresses(value);
	}

	/// Use pseudo instructions
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// 👍 | `true` | `vcmpnltsd xmm2,xmm6,xmm3`
	/// _ | `false` | `vcmpsd xmm2,xmm6,xmm3,5`
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "usePseudoOps")]
	pub fn use_pseudo_ops(&self) -> bool {
		self.0.options().use_pseudo_ops()
	}

	/// Use pseudo instructions
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// 👍 | `true` | `vcmpnltsd xmm2,xmm6,xmm3`
	/// _ | `false` | `vcmpsd xmm2,xmm6,xmm3,5`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "usePseudoOps")]
	pub fn set_use_pseudo_ops(&mut self, value: bool) {
		self.0.options_mut().set_use_pseudo_ops(value);
	}

	/// Show the original value after the symbol name
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// _ | `true` | `mov eax,[myfield (12345678)]`
	/// 👍 | `false` | `mov eax,[myfield]`
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "showSymbolAddress")]
	pub fn show_symbol_address(&self) -> bool {
		self.0.options().show_symbol_address()
	}

	/// Show the original value after the symbol name
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// _ | `true` | `mov eax,[myfield (12345678)]`
	/// 👍 | `false` | `mov eax,[myfield]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "showSymbolAddress")]
	pub fn set_show_symbol_address(&mut self, value: bool) {
		self.0.options_mut().set_show_symbol_address(value);
	}

	/// Always show the effective segment register. If the option is `false`, only show the segment register if
	/// there's a segment override prefix.
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// _ | `true` | `mov eax,ds:[ecx]`
	/// 👍 | `false` | `mov eax,[ecx]`
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "alwaysShowSegmentRegister")]
	pub fn always_show_segment_register(&self) -> bool {
		self.0.options().always_show_segment_register()
	}

	/// Always show the effective segment register. If the option is `false`, only show the segment register if
	/// there's a segment override prefix.
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// _ | `true` | `mov eax,ds:[ecx]`
	/// 👍 | `false` | `mov eax,[ecx]`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "alwaysShowSegmentRegister")]
	pub fn set_always_show_segment_register(&mut self, value: bool) {
		self.0.options_mut().set_always_show_segment_register(value);
	}

	/// Always show the size of memory operands
	///
	/// Default | Value | Example | Example
	/// --------|-------|---------|--------
	/// _ | `true` | `mov eax,dword ptr [ebx]` | `add byte ptr [eax],0x12`
	/// 👍 | `false` | `mov eax,[ebx]` | `add byte ptr [eax],0x12`
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "alwaysShowMemorySize")]
	pub fn always_show_memory_size(&self) -> bool {
		self.0.options().always_show_memory_size()
	}

	/// Always show the size of memory operands
	///
	/// Default | Value | Example | Example
	/// --------|-------|---------|--------
	/// _ | `true` | `mov eax,dword ptr [ebx]` | `add byte ptr [eax],0x12`
	/// 👍 | `false` | `mov eax,[ebx]` | `add byte ptr [eax],0x12`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "alwaysShowMemorySize")]
	pub fn set_always_show_memory_size(&mut self, value: bool) {
		self.0.options_mut().set_always_show_memory_size(value)
	}

	/// Use uppercase hex digits
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// 👍 | `true` | `0xFF`
	/// _ | `false` | `0xff`
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "uppercaseHex")]
	pub fn uppercase_hex(&self) -> bool {
		self.0.options().uppercase_hex()
	}

	/// Use uppercase hex digits
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// 👍 | `true` | `0xFF`
	/// _ | `false` | `0xff`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "uppercaseHex")]
	pub fn set_uppercase_hex(&mut self, value: bool) {
		self.0.options_mut().set_uppercase_hex(value);
	}

	/// Use a hex prefix (`0x`) or a hex suffix (`h`)
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// _ | `true` | `0x5A`
	/// 👍 | `false` | `5Ah`
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "useHexPrefix")]
	pub fn use_hex_prefix(&self) -> bool {
		self.0.options().use_hex_prefix()
	}

	/// Use a hex prefix (`0x`) or a hex suffix (`h`)
	///
	/// Default | Value | Example
	/// --------|-------|--------
	/// _ | `true` | `0x5A`
	/// 👍 | `false` | `5Ah`
	///
	/// # Arguments
	///
	/// * `value`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "useHexPrefix")]
	pub fn set_use_hex_prefix(&mut self, value: bool) {
		self.0.options_mut().set_use_hex_prefix(value)
	}
}
