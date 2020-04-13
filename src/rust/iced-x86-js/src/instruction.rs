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

#[cfg(any(all(feature = "encoder", feature = "instr_api"), feature = "instr_create"))]
use super::code::code_to_iced;
#[cfg(feature = "instr_api")]
use super::code::iced_to_code;
#[cfg(any(feature = "instr_api", feature = "instr_create"))]
use super::code::Code;
#[cfg(feature = "encoder")]
#[cfg(feature = "instr_api")]
use super::code_size::code_size_to_iced;
#[cfg(feature = "instr_api")]
use super::code_size::{iced_to_code_size, CodeSize};
#[cfg(feature = "instr_info")]
#[cfg(feature = "instr_api")]
use super::condition_code::{iced_to_condition_code, ConditionCode};
#[cfg(feature = "instr_info")]
#[cfg(feature = "instr_api")]
use super::encoding_kind::{iced_to_encoding_kind, EncodingKind};
#[cfg(feature = "instr_info")]
#[cfg(feature = "instr_api")]
use super::flow_control::{iced_to_flow_control, FlowControl};
#[cfg(feature = "instr_create")]
use super::memory_operand::MemoryOperand;
#[cfg(feature = "instr_api")]
use super::memory_size::{iced_to_memory_size, MemorySize};
#[cfg(feature = "instr_api")]
use super::mnemonic::{iced_to_mnemonic, Mnemonic};
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
#[cfg(feature = "instr_api")]
use super::op_code_info::OpCodeInfo;
#[cfg(feature = "encoder")]
#[cfg(feature = "instr_api")]
use super::op_kind::op_kind_to_iced;
#[cfg(feature = "instr_api")]
use super::op_kind::{iced_to_op_kind, OpKind};
#[cfg(feature = "instr_api")]
use super::register::iced_to_register;
#[cfg(any(all(feature = "encoder", feature = "instr_api"), feature = "instr_create"))]
use super::register::register_to_iced;
#[cfg(any(feature = "instr_api", feature = "instr_create"))]
use super::register::Register;
#[cfg(feature = "instr_create")]
use super::rep_prefix_kind::{rep_prefix_kind_to_iced, RepPrefixKind};
#[cfg(feature = "encoder")]
#[cfg(feature = "instr_api")]
use super::rounding_control::rounding_control_to_iced;
#[cfg(feature = "instr_api")]
use super::rounding_control::{iced_to_rounding_control, RoundingControl};
use wasm_bindgen::prelude::*;

/// A 16/32/64-bit x86 instruction. Created by [`Decoder`] or by `Instruction.with*()` methods.
///
/// [`Decoder`]: struct.Decoder.html
#[wasm_bindgen]
pub struct Instruction(pub(crate) iced_x86::Instruction);

// ip() and length() are useful when disassembling code so they're always available
#[wasm_bindgen]
#[allow(clippy::len_without_is_empty)]
#[allow(clippy::new_without_default)]
impl Instruction {
	/// Creates an empty `Instruction` (all fields are cleared). See also the `create*()` constructor methods.
	#[wasm_bindgen(constructor)]
	pub fn new() -> Self {
		Self(iced_x86::Instruction::new())
	}

	/// Gets the low 32 bits of the 64-bit IP of the instruction.
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn ipLo(&self) -> u32 {
		self.0.ip() as u32
	}

	/// Gets the high 32 bits of the 64-bit IP of the instruction.
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn ipHi(&self) -> u32 {
		(self.0.ip() >> 32) as u32
	}

	/// Gets the 64-bit IP of the instruction
	#[wasm_bindgen(getter)]
	#[cfg(feature = "bigint")]
	pub fn ip(&self) -> u64 {
		self.0.ip()
	}

	/// Gets the length of the instruction, 0-15 bytes. This is just informational. If you modify the instruction
	/// or create a new one, this method could return the wrong value.
	#[wasm_bindgen(getter)]
	pub fn length(&self) -> u32 {
		self.0.len() as u32
	}
}

#[wasm_bindgen]
#[cfg(feature = "instr_api")]
impl Instruction {
	/// Checks if two instructions are equal, comparing all bits, not ignoring anything. `equals()` ignores some fields.
	#[wasm_bindgen(js_name = "equalsAllBits")]
	pub fn eq_all_bits(&self, other: &Instruction) -> bool {
		self.0.eq_all_bits(&other.0)
	}

	/// Gets the 16-bit IP of the instruction
	#[wasm_bindgen(getter)]
	pub fn ip16(&self) -> u16 {
		self.0.ip16()
	}

	/// Sets the 16-bit IP of the instruction
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_ip16(&mut self, newValue: u16) {
		self.0.set_ip16(newValue)
	}

	/// Gets the 32-bit IP of the instruction
	#[wasm_bindgen(getter)]
	pub fn ip32(&self) -> u32 {
		self.0.ip32()
	}

	/// Sets the 32-bit IP of the instruction
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_ip32(&mut self, newValue: u32) {
		self.0.set_ip32(newValue)
	}

	/// Sets the low 32 bits of the 64-bit IP of the instruction.
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// # Arguments
	///
	/// * `lo`: Low 32 bits
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_ipLo(&mut self, lo: u32) {
		let ip = (self.0.ip() & !0xFFFF_FFFF) | (lo as u64);
		self.0.set_ip(ip)
	}

	/// Sets the high 32 bits of the 64-bit IP of the instruction.
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// # Arguments
	///
	/// * `hi`: High 32 bits
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_ipHi(&mut self, hi: u32) {
		let ip = ((hi as u64) << 32) | (self.0.ip() as u32 as u64);
		self.0.set_ip(ip)
	}

	/// Sets the 64-bit IP of the instruction
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(feature = "bigint")]
	pub fn set_ip(&mut self, newValue: u64) {
		self.0.set_ip(newValue)
	}

	/// Gets the 16-bit IP of the next instruction
	#[wasm_bindgen(getter)]
	pub fn nextIP16(&self) -> u16 {
		self.0.next_ip16()
	}

	/// Sets the 16-bit IP of the next instruction
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_nextIP16(&mut self, newValue: u16) {
		self.0.set_next_ip16(newValue)
	}

	/// Gets the 32-bit IP of the next instruction
	#[wasm_bindgen(getter)]
	pub fn nextIP32(&self) -> u32 {
		self.0.next_ip32()
	}

	/// Sets the 32-bit IP of the next instruction
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_nextIP32(&mut self, newValue: u32) {
		self.0.set_next_ip32(newValue)
	}

	/// Gets the low 32 bits of the 64-bit IP of the next instruction.
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn nextIPLo(&self) -> u32 {
		self.0.next_ip() as u32
	}

	/// Gets the high 32 bits of the 64-bit IP of the next instruction.
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn nextIPHi(&self) -> u32 {
		(self.0.next_ip() >> 32) as u32
	}

	/// Gets the 64-bit IP of the next instruction
	#[wasm_bindgen(getter)]
	#[cfg(feature = "bigint")]
	pub fn nextIP(&self) -> u64 {
		self.0.next_ip()
	}

	/// Sets the low 32 bits of the 64-bit IP of the next instruction.
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_nextIPLo(&mut self, lo: u32) {
		let ip = (self.0.next_ip() & !0xFFFF_FFFF) | (lo as u64);
		self.0.set_next_ip(ip);
	}

	/// Sets the high 32 bits of the 64-bit IP of the next instruction.
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_nextIPHi(&mut self, hi: u32) {
		let ip = ((hi as u64) << 32) | (self.0.next_ip() as u32 as u64);
		self.0.set_next_ip(ip);
	}

	/// Sets the 64-bit IP of the next instruction
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(feature = "bigint")]
	pub fn set_nextIP(&mut self, newValue: u64) {
		self.0.set_next_ip(newValue)
	}

	/// Gets the code size (a [`CodeSize`] enum value) when the instruction was decoded. This value is informational and can
	/// be used by a formatter.
	///
	/// [`CodeSize`]: enum.CodeSize.html
	#[wasm_bindgen(getter)]
	pub fn codeSize(&self) -> CodeSize {
		iced_to_code_size(self.0.code_size())
	}

	/// Gets the code size (a [`CodeSize`] enum value) when the instruction was decoded. This value is informational and can
	/// be used by a formatter.
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_codeSize(&mut self, newValue: CodeSize) {
		self.0.set_code_size(code_size_to_iced(newValue))
	}

	/// Gets the instruction code (a [`Code`] enum value), see also [`mnemonic`].
	///
	/// [`mnemonic`]: #method.mnemonic
	/// [`Code`]: enum.Code.html
	#[wasm_bindgen(getter)]
	pub fn code(&self) -> Code {
		iced_to_code(self.0.code())
	}

	/// Sets the instruction code (a [`Code`] enum value)
	///
	/// [`Code`]: enum.Code.html
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_code(&mut self, newValue: Code) {
		self.0.set_code(code_to_iced(newValue))
	}

	/// Gets the mnemonic (a [`Mnemonic`] enum value), see also [`code`]
	///
	/// [`code`]: #method.code
	/// [`Mnemonic`]: enum.Mnemonic.html
	#[wasm_bindgen(getter)]
	pub fn mnemonic(&self) -> Mnemonic {
		iced_to_mnemonic(self.0.mnemonic())
	}

	/// Gets the operand count. An instruction can have 0-5 operands.
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Decoder, DecoderOptions } = require("iced-x86-js");
	///
	/// // add [rax],ebx
	/// const bytes = new Uint8Array([0x01, 0x18]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	/// const instr = decoder.decode();
	///
	/// assert.equal(instr.opCount, 2);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "opCount")]
	pub fn op_count(&self) -> u32 {
		self.0.op_count()
	}

	/// Sets the length of the instruction, 0-15 bytes. This is just informational. If you modify the instruction
	/// or create a new one, this method could return the wrong value.
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_length(&mut self, newValue: u32) {
		self.0.set_len(newValue as usize)
	}

	/// `true` if the instruction has the `XACQUIRE` prefix (`F2`)
	#[wasm_bindgen(getter)]
	pub fn hasXacquirePrefix(&self) -> bool {
		self.0.has_xacquire_prefix()
	}

	/// `true` if the instruction has the `XACQUIRE` prefix (`F2`)
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_hasXacquirePrefix(&mut self, newValue: bool) {
		self.0.set_has_xacquire_prefix(newValue)
	}

	/// `true` if the instruction has the `XRELEASE` prefix (`F3`)
	#[wasm_bindgen(getter)]
	pub fn hasXreleasePrefix(&self) -> bool {
		self.0.has_xrelease_prefix()
	}

	/// `true` if the instruction has the `XRELEASE` prefix (`F3`)
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_hasXreleasePrefix(&mut self, newValue: bool) {
		self.0.set_has_xrelease_prefix(newValue)
	}

	/// `true` if the instruction has the `REPE` or `REP` prefix (`F3`)
	#[wasm_bindgen(getter)]
	pub fn hasRepPrefix(&self) -> bool {
		self.0.has_rep_prefix()
	}

	/// `true` if the instruction has the `REPE` or `REP` prefix (`F3`)
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_hasRepPrefix(&mut self, newValue: bool) {
		self.0.set_has_rep_prefix(newValue)
	}

	/// `true` if the instruction has the `REPE` or `REP` prefix (`F3`)
	#[wasm_bindgen(getter)]
	pub fn hasRepePrefix(&self) -> bool {
		self.0.has_repe_prefix()
	}

	/// `true` if the instruction has the `REPE` or `REP` prefix (`F3`)
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_hasRepePrefix(&mut self, newValue: bool) {
		self.0.set_has_repe_prefix(newValue)
	}

	/// `true` if the instruction has the `REPNE` prefix (`F2`)
	#[wasm_bindgen(getter)]
	pub fn hasRepnePrefix(&self) -> bool {
		self.0.has_repne_prefix()
	}

	/// `true` if the instruction has the `REPNE` prefix (`F2`)
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_hasRepnePrefix(&mut self, newValue: bool) {
		self.0.set_has_repne_prefix(newValue)
	}

	/// `true` if the instruction has the `LOCK` prefix (`F0`)
	#[wasm_bindgen(getter)]
	pub fn hasLockPrefix(&self) -> bool {
		self.0.has_lock_prefix()
	}

	/// `true` if the instruction has the `LOCK` prefix (`F0`)
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_hasLockPrefix(&mut self, newValue: bool) {
		self.0.set_has_lock_prefix(newValue)
	}

	/// Gets operand #0's kind (an [`OpKind`] enum value) if the operand exists (see [`opCount`] and [`opKind`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	/// [`opKind`]: #method.opKind
	#[wasm_bindgen(getter)]
	pub fn op0Kind(&self) -> OpKind {
		iced_to_op_kind(self.0.op0_kind())
	}

	/// Sets operand #0's kind (an [`OpKind`] enum value) if the operand exists (see [`opCount`] and [`opKind`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	/// [`opKind`]: #method.setOpKind
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_op0Kind(&mut self, newValue: OpKind) {
		self.0.set_op0_kind(op_kind_to_iced(newValue))
	}

	/// Gets operand #1's kind (an [`OpKind`] enum value) if the operand exists (see [`opCount`] and [`opKind`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	/// [`opKind`]: #method.opKind
	#[wasm_bindgen(getter)]
	pub fn op1Kind(&self) -> OpKind {
		iced_to_op_kind(self.0.op1_kind())
	}

	/// Sets operand #1's kind (an [`OpKind`] enum value) if the operand exists (see [`opCount`] and [`opKind`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	/// [`opKind`]: #method.setOpKind
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_op1Kind(&mut self, newValue: OpKind) {
		self.0.set_op1_kind(op_kind_to_iced(newValue))
	}

	/// Gets operand #2's kind (an [`OpKind`] enum value) if the operand exists (see [`opCount`] and [`opKind`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	/// [`opKind`]: #method.opKind
	#[wasm_bindgen(getter)]
	pub fn op2Kind(&self) -> OpKind {
		iced_to_op_kind(self.0.op2_kind())
	}

	/// Sets operand #2's kind (an [`OpKind`] enum value) if the operand exists (see [`opCount`] and [`opKind`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	/// [`opKind`]: #method.setOpKind
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_op2Kind(&mut self, newValue: OpKind) {
		self.0.set_op2_kind(op_kind_to_iced(newValue))
	}

	/// Gets operand #3's kind (an [`OpKind`] enum value) if the operand exists (see [`opCount`] and [`opKind`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	/// [`opKind`]: #method.opKind
	#[wasm_bindgen(getter)]
	pub fn op3Kind(&self) -> OpKind {
		iced_to_op_kind(self.0.op3_kind())
	}

	/// Sets operand #3's kind (an [`OpKind`] enum value) if the operand exists (see [`opCount`] and [`opKind`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	/// [`opKind`]: #method.setOpKind
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_op3Kind(&mut self, newValue: OpKind) {
		self.0.set_op3_kind(op_kind_to_iced(newValue))
	}

	/// Gets operand #4's kind (an [`OpKind`] enum value) if the operand exists (see [`opCount`] and [`opKind`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	/// [`opKind`]: #method.opKind
	#[wasm_bindgen(getter)]
	pub fn op4Kind(&self) -> OpKind {
		iced_to_op_kind(self.0.op4_kind())
	}

	/// Sets operand #4's kind (an [`OpKind`] enum value) if the operand exists (see [`opCount`] and [`opKind`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	/// [`opKind`]: #method.setOpKind
	///
	/// # Throws
	///
	/// Throws if `newValue` is invalid.
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_op4Kind(&mut self, newValue: OpKind) {
		self.0.set_op4_kind(op_kind_to_iced(newValue))
	}

	/// Gets an operand's kind (an [`OpKind`] enum value) if it exists (see [`opCount`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	///
	/// # Throws
	///
	/// Throws if `operand` is invalid
	///
	/// # Arguments
	///
	/// * `operand`: Operand number, 0-4
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Decoder, DecoderOptions, OpKind, Register } = require("iced-x86-js");
	///
	/// // add [rax],ebx
	/// const bytes = new Uint8Array([0x01, 0x18]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	/// const instr = decoder.decode();
	///
	/// assert.equal(instr.opCount, 2);
	/// assert.equal(instr.opKind(0), OpKind.Memory);
	/// assert.equal(instr.memoryBase, Register.RAX);
	/// assert.equal(instr.memoryIndex, Register.None);
	/// assert.equal(instr.opKind(1), OpKind.Register);
	/// assert.equal(instr.opRegister(1), Register.EBX);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	pub fn opKind(&self, operand: u32) -> OpKind {
		iced_to_op_kind(self.0.op_kind(operand))
	}

	/// Sets an operand's kind (an [`OpKind`] enum value)
	///
	/// [`OpKind`]: enum.OpKind.html
	///
	/// # Throws
	///
	/// Throws if `operand` is invalid
	///
	/// # Arguments
	///
	/// * `operand`: Operand number, 0-4
	/// * `opKind`: Operand kind
	#[cfg(feature = "encoder")]
	pub fn setOpKind(&mut self, operand: u32, opKind: OpKind) {
		self.0.set_op_kind(operand, op_kind_to_iced(opKind))
	}

	/// Checks if the instruction has a segment override prefix, see [`segmentPrefix`]
	///
	/// [`segmentPrefix`]: #method.segmentPrefix
	#[wasm_bindgen(getter)]
	pub fn hasSegmentPrefix(&self) -> bool {
		self.0.has_segment_prefix()
	}

	/// Gets the segment override prefix (a [`Register`] enum value) or [`Register.None`] if none. See also [`memorySegment`].
	/// Use this method if the operand has kind [`OpKind.Memory`], [`OpKind.Memory64`],
	/// [`OpKind.MemorySegSI`], [`OpKind.MemorySegESI`], [`OpKind.MemorySegRSI`]
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`memorySegment`]: #method.memory_segment
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	/// [`OpKind.Memory64`]: enum.OpKind.html#variant.Memory64
	/// [`OpKind.MemorySegSI`]: enum.OpKind.html#variant.MemorySegSI
	/// [`OpKind.MemorySegESI`]: enum.OpKind.html#variant.MemorySegESI
	/// [`OpKind.MemorySegESI`]: enum.OpKind.html#variant.MemorySegESI
	/// [`OpKind.MemorySegRSI`]: enum.OpKind.html#variant.MemorySegRSI
	#[wasm_bindgen(getter)]
	pub fn segmentPrefix(&self) -> Register {
		iced_to_register(self.0.segment_prefix())
	}

	/// Sets the segment override prefix (a [`Register`] enum value) or [`Register.None`] if none. See also [`memorySegment`].
	/// Use this method if the operand has kind [`OpKind.Memory`], [`OpKind.Memory64`],
	/// [`OpKind.MemorySegSI`], [`OpKind.MemorySegESI`], [`OpKind.MemorySegRSI`]
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`memorySegment`]: #method.memory_segment
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	/// [`OpKind.Memory64`]: enum.OpKind.html#variant.Memory64
	/// [`OpKind.MemorySegSI`]: enum.OpKind.html#variant.MemorySegSI
	/// [`OpKind.MemorySegESI`]: enum.OpKind.html#variant.MemorySegESI
	/// [`OpKind.MemorySegESI`]: enum.OpKind.html#variant.MemorySegESI
	/// [`OpKind.MemorySegRSI`]: enum.OpKind.html#variant.MemorySegRSI
	///
	/// # Arguments
	///
	/// * `newValue`: Segment register prefix
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_segmentPrefix(&mut self, newValue: Register) {
		self.0.set_segment_prefix(register_to_iced(newValue))
	}

	/// Gets the effective segment register (a [`Register`] enum value) used to reference the memory location.
	/// Use this method if the operand has kind [`OpKind.Memory`], [`OpKind.Memory64`],
	/// [`OpKind.MemorySegSI`], [`OpKind.MemorySegESI`], [`OpKind.MemorySegRSI`]
	///
	/// [`Register`]: enum.Register.html
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	/// [`OpKind.Memory64`]: enum.OpKind.html#variant.Memory64
	/// [`OpKind.MemorySegSI`]: enum.OpKind.html#variant.MemorySegSI
	/// [`OpKind.MemorySegESI`]: enum.OpKind.html#variant.MemorySegESI
	/// [`OpKind.MemorySegRSI`]: enum.OpKind.html#variant.MemorySegRSI
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "memorySegment")]
	pub fn memory_segment(&self) -> Register {
		iced_to_register(self.0.memory_segment())
	}

	/// Gets the size of the memory displacement in bytes. Valid values are `0`, `1` (16/32/64-bit), `2` (16-bit), `4` (32-bit), `8` (64-bit).
	/// Note that the return value can be 1 and [`memoryDisplacement`] may still not fit in
	/// a signed byte if it's an EVEX encoded instruction.
	/// Use this method if the operand has kind [`OpKind.Memory`]
	///
	/// [`memoryDisplacement`]: #method.memoryDisplacement
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	#[wasm_bindgen(getter)]
	pub fn memoryDisplSize(&self) -> u32 {
		self.0.memory_displ_size()
	}

	/// Sets the size of the memory displacement in bytes. Valid values are `0`, `1` (16/32/64-bit), `2` (16-bit), `4` (32-bit), `8` (64-bit).
	/// Note that the return value can be 1 and [`memoryDisplacement`] may still not fit in
	/// a signed byte if it's an EVEX encoded instruction.
	/// Use this method if the operand has kind [`OpKind.Memory`]
	///
	/// [`memoryDisplacement`]: #method.memoryDisplacement
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	///
	/// # Arguments
	///
	/// * `newValue`: Displacement size
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_memoryDisplSize(&mut self, newValue: u32) {
		self.0.set_memory_displ_size(newValue)
	}

	/// `true` if the data is broadcasted (EVEX instructions only)
	#[wasm_bindgen(getter)]
	pub fn isBroadcast(&self) -> bool {
		self.0.is_broadcast()
	}

	/// Sets the is broadcast flag (EVEX instructions only)
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_isBroadcast(&mut self, newValue: bool) {
		self.0.set_is_broadcast(newValue)
	}

	/// Gets the size of the memory location (a [`MemorySize`] enum value) that is referenced by the operand. See also [`isBroadcast`].
	/// Use this method if the operand has kind [`OpKind.Memory`], [`OpKind.Memory64`],
	/// [`OpKind.MemorySegSI`], [`OpKind.MemorySegESI`], [`OpKind.MemorySegRSI`],
	/// [`OpKind.MemoryESDI`], [`OpKind.MemoryESEDI`], [`OpKind.MemoryESRDI`]
	///
	/// [`MemorySize`]: enum.MemorySize.html
	/// [`isBroadcast`]: #method.isBroadcast
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	/// [`OpKind.Memory64`]: enum.OpKind.html#variant.Memory64
	/// [`OpKind.MemorySegSI`]: enum.OpKind.html#variant.MemorySegSI
	/// [`OpKind.MemorySegESI`]: enum.OpKind.html#variant.MemorySegESI
	/// [`OpKind.MemorySegRSI`]: enum.OpKind.html#variant.MemorySegRSI
	/// [`OpKind.MemoryESDI`]: enum.OpKind.html#variant.MemoryESDI
	/// [`OpKind.MemoryESEDI`]: enum.OpKind.html#variant.MemoryESEDI
	/// [`OpKind.MemoryESRDI`]: enum.OpKind.html#variant.MemoryESRDI
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "memorySize")]
	pub fn memory_size(&self) -> MemorySize {
		iced_to_memory_size(self.0.memory_size())
	}

	/// Gets the index register scale value, valid values are `*1`, `*2`, `*4`, `*8`. Use this method if the operand has kind [`OpKind.Memory`]
	///
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	#[wasm_bindgen(getter)]
	pub fn memoryIndexScale(&self) -> u32 {
		self.0.memory_index_scale()
	}

	/// Sets the index register scale value, valid values are `*1`, `*2`, `*4`, `*8`. Use this method if the operand has kind [`OpKind.Memory`]
	///
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	///
	/// # Arguments
	///
	/// * `newValue`: New value (1, 2, 4 or 8)
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_memoryIndexScale(&mut self, newValue: u32) {
		self.0.set_memory_index_scale(newValue)
	}

	/// Gets the memory operand's displacement. This should be sign extended to 64 bits if it's 64-bit addressing (see [`memoryDisplacement64`]).
	/// Use this method if the operand has kind [`OpKind.Memory`]
	///
	/// [`memoryDisplacement64`]: #method.memory_displacement64
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	#[wasm_bindgen(getter)]
	pub fn memoryDisplacement(&self) -> u32 {
		self.0.memory_displacement()
	}

	/// Gets the memory operand's displacement. This should be sign extended to 64 bits if it's 64-bit addressing (see [`memoryDisplacement64`]).
	/// Use this method if the operand has kind [`OpKind.Memory`]
	///
	/// [`memoryDisplacement64`]: #method.memory_displacement64
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_memoryDisplacement(&mut self, newValue: u32) {
		self.0.set_memory_displacement(newValue)
	}

	/// Gets the low 32 bits of the memory operand's displacement sign extended to 64 bits.
	/// Use this method if the operand has kind [`OpKind.Memory`].
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "memoryDisplacement64Lo")]
	#[cfg(not(feature = "bigint"))]
	pub fn memory_displacement64_lo(&self) -> u32 {
		self.0.memory_displacement64() as u32
	}

	/// Gets the high 32 bits of the memory operand's displacement sign extended to 64 bits.
	/// Use this method if the operand has kind [`OpKind.Memory`].
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "memoryDisplacement64Hi")]
	#[cfg(not(feature = "bigint"))]
	pub fn memory_displacement64_hi(&self) -> u32 {
		(self.0.memory_displacement64() >> 32) as u32
	}

	/// Gets the memory operand's displacement sign extended to 64 bits.
	/// Use this method if the operand has kind [`OpKind.Memory`]
	///
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "memoryDisplacement64")]
	#[cfg(feature = "bigint")]
	pub fn memory_displacement64(&self) -> u64 {
		self.0.memory_displacement64()
	}

	/// Gets the low 32 bits of an operand's immediate value.
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// # Throws
	///
	/// Throws if `operand` is invalid or if it's not an immediate operand
	///
	/// # Arguments
	///
	/// * `operand`: Operand number, 0-4
	#[cfg(not(feature = "bigint"))]
	pub fn immediateLo(&self, operand: u32) -> u32 {
		self.0.immediate(operand) as u32
	}

	/// Gets the high 32 bits of an operand's immediate value.
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// # Throws
	///
	/// Throws if `operand` is invalid or if it's not an immediate operand
	///
	/// # Arguments
	///
	/// * `operand`: Operand number, 0-4
	#[cfg(not(feature = "bigint"))]
	pub fn immediateHi(&self, operand: u32) -> u32 {
		(self.0.immediate(operand) >> 32) as u32
	}

	/// Gets an operand's immediate value
	///
	/// # Throws
	///
	/// Throws if `operand` is invalid or if it's not an immediate operand
	///
	/// # Arguments
	///
	/// * `operand`: Operand number, 0-4
	#[cfg(feature = "bigint")]
	pub fn immediate(&self, operand: u32) -> u64 {
		self.0.immediate(operand)
	}

	/// Sets an operand's immediate value
	///
	/// # Throws
	///
	/// Throws if `operand` is invalid or if it's not an immediate operand
	///
	/// # Arguments
	///
	/// * `operand`: Operand number, 0-4
	/// * `newValue`: Immediate
	#[wasm_bindgen(js_name = "setImmediateI32")]
	#[cfg(feature = "encoder")]
	pub fn set_immediate_i32(&mut self, operand: u32, newValue: i32) {
		self.0.set_immediate_i32(operand, newValue)
	}

	/// Sets an operand's immediate value
	///
	/// # Throws
	///
	/// Throws if `operand` is invalid or if it's not an immediate operand
	///
	/// # Arguments
	///
	/// * `operand`: Operand number, 0-4
	/// * `newValue`: Immediate
	#[wasm_bindgen(js_name = "setImmediateU32")]
	#[cfg(feature = "encoder")]
	pub fn set_immediate_u32(&mut self, operand: u32, newValue: u32) {
		self.0.set_immediate_u32(operand, newValue)
	}

	/// Sets an operand's immediate value.
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// # Throws
	///
	/// Throws if `operand` is invalid or if it's not an immediate operand
	///
	/// # Arguments
	///
	/// * `operand`: Operand number, 0-4
	/// - `hi`: High 32 bits of immediate
	/// - `lo`: Low 32 bits of immediate
	#[wasm_bindgen(js_name = "setImmediateI64")]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_immediate_i64(&mut self, operand: u32, hi: u32, lo: u32) {
		let new_value = (((hi as u64) << 32) | (lo as u64)) as i64;
		self.0.set_immediate_i64(operand, new_value)
	}

	/// Sets an operand's immediate value
	///
	/// # Throws
	///
	/// Throws if `operand` is invalid or if it's not an immediate operand
	///
	/// # Arguments
	///
	/// * `operand`: Operand number, 0-4
	/// * `newValue`: Immediate
	#[wasm_bindgen(js_name = "setImmediateI64")]
	#[cfg(feature = "encoder")]
	#[cfg(feature = "bigint")]
	pub fn set_immediate_i64(&mut self, operand: u32, newValue: i64) {
		self.0.set_immediate_i64(operand, newValue)
	}

	/// Sets an operand's immediate value.
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// # Throws
	///
	/// Throws if `operand` is invalid or if it's not an immediate operand
	///
	/// # Arguments
	///
	/// * `operand`: Operand number, 0-4
	/// - `hi`: High 32 bits of immediate
	/// - `lo`: Low 32 bits of immediate
	#[wasm_bindgen(js_name = "setImmediateU64")]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_immediate_u64(&mut self, operand: u32, hi: u32, lo: u32) {
		let new_value = ((hi as u64) << 32) | (lo as u64);
		self.0.set_immediate_u64(operand, new_value)
	}

	/// Sets an operand's immediate value
	///
	/// # Throws
	///
	/// Throws if `operand` is invalid or if it's not an immediate operand
	///
	/// # Arguments
	///
	/// * `operand`: Operand number, 0-4
	/// * `newValue`: Immediate
	#[wasm_bindgen(js_name = "setImmediateU64")]
	#[cfg(feature = "encoder")]
	#[cfg(feature = "bigint")]
	pub fn set_immediate_u64(&mut self, operand: u32, newValue: u64) {
		self.0.set_immediate_u64(operand, newValue)
	}

	/// Gets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate8`]
	///
	/// [`OpKind.Immediate8`]: enum.OpKind.html#variant.Immediate8
	#[wasm_bindgen(getter)]
	pub fn immediate8(&self) -> u8 {
		self.0.immediate8()
	}

	/// Sets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate8`]
	///
	/// [`OpKind.Immediate8`]: enum.OpKind.html#variant.Immediate8
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_immediate8(&mut self, newValue: u8) {
		self.0.set_immediate8(newValue)
	}

	/// Gets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate8_2nd`]
	///
	/// [`OpKind.Immediate8_2nd`]: enum.OpKind.html#variant.Immediate8_2nd
	#[wasm_bindgen(getter)]
	pub fn immediate8_2nd(&self) -> u8 {
		self.0.immediate8_2nd()
	}

	/// Sets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate8_2nd`]
	///
	/// [`OpKind.Immediate8_2nd`]: enum.OpKind.html#variant.Immediate8_2nd
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_immediate8_2nd(&mut self, newValue: u8) {
		self.0.set_immediate8_2nd(newValue)
	}

	/// Gets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate16`]
	///
	/// [`OpKind.Immediate16`]: enum.OpKind.html#variant.Immediate16
	#[wasm_bindgen(getter)]
	pub fn immediate16(&self) -> u16 {
		self.0.immediate16()
	}

	/// Sets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate16`]
	///
	/// [`OpKind.Immediate16`]: enum.OpKind.html#variant.Immediate16
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_immediate16(&mut self, newValue: u16) {
		self.0.set_immediate16(newValue)
	}

	/// Gets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate32`]
	///
	/// [`OpKind.Immediate32`]: enum.OpKind.html#variant.Immediate32
	#[wasm_bindgen(getter)]
	pub fn immediate32(&self) -> u32 {
		self.0.immediate32()
	}

	/// Sets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate32`]
	///
	/// [`OpKind.Immediate32`]: enum.OpKind.html#variant.Immediate32
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_immediate32(&mut self, newValue: u32) {
		self.0.set_immediate32(newValue)
	}

	/// Gets the low 32 bits of the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate64`].
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`OpKind.Immediate64`]: enum.OpKind.html#variant.Immediate64
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn immediate64Lo(&self) -> u32 {
		self.0.immediate64() as u32
	}

	/// Gets the high 32 bits of the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate64`].
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`OpKind.Immediate64`]: enum.OpKind.html#variant.Immediate64
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn immediate64Hi(&self) -> u32 {
		(self.0.immediate64() >> 32) as u32
	}

	/// Gets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate64`]
	///
	/// [`OpKind.Immediate64`]: enum.OpKind.html#variant.Immediate64
	#[wasm_bindgen(getter)]
	#[cfg(feature = "bigint")]
	pub fn immediate64(&self) -> u64 {
		self.0.immediate64()
	}

	/// Sets the low 32 bits of the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate64`].
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`OpKind.Immediate64`]: enum.OpKind.html#variant.Immediate64
	///
	/// # Arguments
	///
	/// * `lo`: Low 32 bits
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_immediate64Lo(&mut self, lo: u32) {
		let new_value = (self.0.immediate64() & !0xFFFF_FFFF) | (lo as u64);
		self.0.set_immediate64(new_value);
	}

	/// Sets the high 32 bits of the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate64`].
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`OpKind.Immediate64`]: enum.OpKind.html#variant.Immediate64
	///
	/// # Arguments
	///
	/// * `hi`: High 32 bits
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_immediate64Hi(&mut self, hi: u32) {
		let new_value = ((hi as u64) << 32) | (self.0.immediate64() as u32 as u64);
		self.0.set_immediate64(new_value);
	}

	/// Sets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate64`]
	///
	/// [`OpKind.Immediate64`]: enum.OpKind.html#variant.Immediate64
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(feature = "bigint")]
	pub fn set_immediate64(&mut self, newValue: u64) {
		self.0.set_immediate64(newValue)
	}

	/// Gets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate8to16`]
	///
	/// [`OpKind.Immediate8to16`]: enum.OpKind.html#variant.Immediate8to16
	#[wasm_bindgen(getter)]
	pub fn immediate8to16(&self) -> i16 {
		self.0.immediate8to16()
	}

	/// Sets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate8to16`]
	///
	/// [`OpKind.Immediate8to16`]: enum.OpKind.html#variant.Immediate8to16
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_immediate8to16(&mut self, newValue: i16) {
		self.0.set_immediate8to16(newValue)
	}

	/// Gets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate8to32`]
	///
	/// [`OpKind.Immediate8to32`]: enum.OpKind.html#variant.Immediate8to32
	#[wasm_bindgen(getter)]
	pub fn immediate8to32(&self) -> i32 {
		self.0.immediate8to32()
	}

	/// Sets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate8to32`]
	///
	/// [`OpKind.Immediate8to32`]: enum.OpKind.html#variant.Immediate8to32
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_immediate8to32(&mut self, newValue: i32) {
		self.0.set_immediate8to32(newValue)
	}

	/// Gets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate8to64`].
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`OpKind.Immediate8to64`]: enum.OpKind.html#variant.Immediate8to64
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn immediate8to64(&self) -> i32 {
		self.0.immediate8to64() as i32
	}

	/// Gets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate8to64`]
	///
	/// [`OpKind.Immediate8to64`]: enum.OpKind.html#variant.Immediate8to64
	#[wasm_bindgen(getter)]
	#[cfg(feature = "bigint")]
	pub fn immediate8to64(&self) -> i64 {
		self.0.immediate8to64()
	}

	/// Sets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate8to64`].
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`OpKind.Immediate8to64`]: enum.OpKind.html#variant.Immediate8to64
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_immediate8to64(&mut self, newValue: i32) {
		self.0.set_immediate8to64(newValue as i64)
	}

	/// Sets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate8to64`]
	///
	/// [`OpKind.Immediate8to64`]: enum.OpKind.html#variant.Immediate8to64
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(feature = "bigint")]
	pub fn set_immediate8to64(&mut self, newValue: i64) {
		self.0.set_immediate8to64(newValue)
	}

	/// Gets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate32to64`].
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`OpKind.Immediate32to64`]: enum.OpKind.html#variant.Immediate32to64
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn immediate32to64(&self) -> i32 {
		self.0.immediate32to64() as i32
	}

	/// Gets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate32to64`]
	///
	/// [`OpKind.Immediate32to64`]: enum.OpKind.html#variant.Immediate32to64
	#[wasm_bindgen(getter)]
	#[cfg(feature = "bigint")]
	pub fn immediate32to64(&self) -> i64 {
		self.0.immediate32to64()
	}

	/// Sets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate32to64`].
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`OpKind.Immediate32to64`]: enum.OpKind.html#variant.Immediate32to64
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_immediate32to64(&mut self, newValue: i32) {
		self.0.set_immediate32to64(newValue as i64)
	}

	/// Sets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate32to64`]
	///
	/// [`OpKind.Immediate32to64`]: enum.OpKind.html#variant.Immediate32to64
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(feature = "bigint")]
	pub fn set_immediate32to64(&mut self, newValue: i64) {
		self.0.set_immediate32to64(newValue)
	}

	/// Gets the low 32 bits of the operand's 64-bit address value. Use this method if the operand has kind [`OpKind.Memory64`].
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`OpKind.Memory64`]: enum.OpKind.html#variant.Memory64
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn memoryAddress64Lo(&self) -> u32 {
		self.0.memory_address64() as u32
	}

	/// Gets the high 32 bits of the operand's 64-bit address value. Use this method if the operand has kind [`OpKind.Memory64`].
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`OpKind.Memory64`]: enum.OpKind.html#variant.Memory64
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn memoryAddress64Hi(&self) -> u32 {
		(self.0.memory_address64() >> 32) as u32
	}

	/// Gets the operand's 64-bit address value. Use this method if the operand has kind [`OpKind.Memory64`]
	///
	/// [`OpKind.Memory64`]: enum.OpKind.html#variant.Memory64
	#[wasm_bindgen(getter)]
	#[cfg(feature = "bigint")]
	pub fn memoryAddress64(&self) -> u64 {
		self.0.memory_address64()
	}

	/// Sets the low 32 bits of the operand's 64-bit address value. Use this method if the operand has kind [`OpKind.Memory64`].
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`OpKind.Memory64`]: enum.OpKind.html#variant.Memory64
	///
	/// # Arguments
	///
	/// * `lo`: Low 32 bits
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_memoryAddress64Lo(&mut self, lo: u32) {
		let new_value = (self.0.memory_address64() & !0xFFFF_FFFF) | (lo as u64);
		self.0.set_memory_address64(new_value);
	}

	/// Sets the high 32 bits of the operand's 64-bit address value. Use this method if the operand has kind [`OpKind.Memory64`].
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`OpKind.Memory64`]: enum.OpKind.html#variant.Memory64
	///
	/// # Arguments
	///
	/// * `hi`: High 32 bits
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_memoryAddress64Hi(&mut self, hi: u32) {
		let new_value = ((hi as u64) << 32) | (self.0.memory_address64() as u32 as u64);
		self.0.set_memory_address64(new_value)
	}

	/// Sets the operand's 64-bit address value. Use this method if the operand has kind [`OpKind.Memory64`]
	///
	/// [`OpKind.Memory64`]: enum.OpKind.html#variant.Memory64
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(feature = "bigint")]
	pub fn set_memoryAddress64(&mut self, newValue: u64) {
		self.0.set_memory_address64(newValue)
	}

	/// Gets the operand's branch target. Use this method if the operand has kind [`OpKind.NearBranch16`]
	///
	/// [`OpKind.NearBranch16`]: enum.OpKind.html#variant.NearBranch16
	#[wasm_bindgen(getter)]
	pub fn nearBranch16(&self) -> u16 {
		self.0.near_branch16()
	}

	/// Sets the operand's branch target. Use this method if the operand has kind [`OpKind.NearBranch16`]
	///
	/// [`OpKind.NearBranch16`]: enum.OpKind.html#variant.NearBranch16
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_nearBranch16(&mut self, newValue: u16) {
		self.0.set_near_branch16(newValue)
	}

	/// Gets the operand's branch target. Use this method if the operand has kind [`OpKind.NearBranch32`]
	///
	/// [`OpKind.NearBranch32`]: enum.OpKind.html#variant.NearBranch32
	#[wasm_bindgen(getter)]
	pub fn nearBranch32(&self) -> u32 {
		self.0.near_branch32()
	}

	/// Sets the operand's branch target. Use this method if the operand has kind [`OpKind.NearBranch32`]
	///
	/// [`OpKind.NearBranch32`]: enum.OpKind.html#variant.NearBranch32
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_nearBranch32(&mut self, newValue: u32) {
		self.0.set_near_branch32(newValue)
	}

	/// Gets the low 32 bits of the operand's branch target. Use this method if the operand has kind [`OpKind.NearBranch64`].
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`OpKind.NearBranch64`]: enum.OpKind.html#variant.NearBranch64
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn nearBranch64Lo(&self) -> u32 {
		self.0.near_branch64() as u32
	}

	/// Gets the high 32 bits of the operand's branch target. Use this method if the operand has kind [`OpKind.NearBranch64`].
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`OpKind.NearBranch64`]: enum.OpKind.html#variant.NearBranch64
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn nearBranch64Hi(&self) -> u32 {
		(self.0.near_branch64() >> 32) as u32
	}

	/// Gets the operand's branch target. Use this method if the operand has kind [`OpKind.NearBranch64`]
	///
	/// [`OpKind.NearBranch64`]: enum.OpKind.html#variant.NearBranch64
	#[wasm_bindgen(getter)]
	#[cfg(feature = "bigint")]
	pub fn nearBranch64(&self) -> u64 {
		self.0.near_branch64()
	}

	/// Sets the low 32 bits of the operand's branch target. Use this method if the operand has kind [`OpKind.NearBranch64`].
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`OpKind.NearBranch64`]: enum.OpKind.html#variant.NearBranch64
	///
	/// # Arguments
	///
	/// * `lo`: Low 32 bits
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_nearBranch64Lo(&mut self, lo: u32) {
		let new_value = (self.0.near_branch64() & !0xFFFF_FFFF) | (lo as u64);
		self.0.set_near_branch64(new_value)
	}

	/// Sets the high 32 bits of the operand's branch target. Use this method if the operand has kind [`OpKind.NearBranch64`].
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`OpKind.NearBranch64`]: enum.OpKind.html#variant.NearBranch64
	///
	/// # Arguments
	///
	/// * `hi`: High 32 bits
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_nearBranch64Hi(&mut self, hi: u32) {
		let new_value = ((hi as u64) << 32) | (self.0.near_branch64() as u32 as u64);
		self.0.set_near_branch64(new_value)
	}

	/// Sets the operand's branch target. Use this method if the operand has kind [`OpKind.NearBranch64`]
	///
	/// [`OpKind.NearBranch64`]: enum.OpKind.html#variant.NearBranch64
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(feature = "bigint")]
	pub fn set_nearBranch64(&mut self, newValue: u64) {
		self.0.set_near_branch64(newValue)
	}

	/// Gets the low 32 bits of the near branch target if it's a `CALL`/`JMP`/`Jcc` near branch instruction
	/// (i.e., if [`op0Kind`] is [`OpKind.NearBranch16`], [`OpKind.NearBranch32`] or [`OpKind.NearBranch64`]).
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`op0Kind`]: #method.op0Kind
	/// [`OpKind.NearBranch16`]: enum.OpKind.html#variant.NearBranch16
	/// [`OpKind.NearBranch32`]: enum.OpKind.html#variant.NearBranch32
	/// [`OpKind.NearBranch64`]: enum.OpKind.html#variant.NearBranch64
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn nearBranchTargetLo(&self) -> u32 {
		self.0.near_branch_target() as u32
	}

	/// Gets the high 32 bits of the near branch target if it's a `CALL`/`JMP`/`Jcc` near branch instruction
	/// (i.e., if [`op0Kind`] is [`OpKind.NearBranch16`], [`OpKind.NearBranch32`] or [`OpKind.NearBranch64`]).
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`op0Kind`]: #method.op0Kind
	/// [`OpKind.NearBranch16`]: enum.OpKind.html#variant.NearBranch16
	/// [`OpKind.NearBranch32`]: enum.OpKind.html#variant.NearBranch32
	/// [`OpKind.NearBranch64`]: enum.OpKind.html#variant.NearBranch64
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn nearBranchTargetHi(&self) -> u32 {
		(self.0.near_branch_target() >> 32) as u32
	}

	/// Gets the near branch target if it's a `CALL`/`JMP`/`Jcc` near branch instruction
	/// (i.e., if [`op0Kind`] is [`OpKind.NearBranch16`], [`OpKind.NearBranch32`] or [`OpKind.NearBranch64`])
	///
	/// [`op0Kind`]: #method.op0Kind
	/// [`OpKind.NearBranch16`]: enum.OpKind.html#variant.NearBranch16
	/// [`OpKind.NearBranch32`]: enum.OpKind.html#variant.NearBranch32
	/// [`OpKind.NearBranch64`]: enum.OpKind.html#variant.NearBranch64
	#[wasm_bindgen(getter)]
	#[cfg(feature = "bigint")]
	pub fn nearBranchTarget(&self) -> u64 {
		self.0.near_branch_target()
	}

	/// Gets the operand's branch target. Use this method if the operand has kind [`OpKind.FarBranch16`]
	///
	/// [`OpKind.FarBranch16`]: enum.OpKind.html#variant.FarBranch16
	#[wasm_bindgen(getter)]
	pub fn farBranch16(&self) -> u16 {
		self.0.far_branch16()
	}

	/// Sets the operand's branch target. Use this method if the operand has kind [`OpKind.FarBranch16`]
	///
	/// [`OpKind.FarBranch16`]: enum.OpKind.html#variant.FarBranch16
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_farBranch16(&mut self, newValue: u16) {
		self.0.set_far_branch16(newValue)
	}

	/// Gets the operand's branch target. Use this method if the operand has kind [`OpKind.FarBranch32`]
	///
	/// [`OpKind.FarBranch32`]: enum.OpKind.html#variant.FarBranch32
	#[wasm_bindgen(getter)]
	pub fn farBranch32(&self) -> u32 {
		self.0.far_branch32()
	}

	/// Sets the operand's branch target. Use this method if the operand has kind [`OpKind.FarBranch32`]
	///
	/// [`OpKind.FarBranch32`]: enum.OpKind.html#variant.FarBranch32
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_farBranch32(&mut self, newValue: u32) {
		self.0.set_far_branch32(newValue)
	}

	/// Gets the operand's branch target selector. Use this method if the operand has kind [`OpKind.FarBranch16`] or [`OpKind.FarBranch32`]
	///
	/// [`OpKind.FarBranch16`]: enum.OpKind.html#variant.FarBranch16
	/// [`OpKind.FarBranch32`]: enum.OpKind.html#variant.FarBranch32
	#[wasm_bindgen(getter)]
	pub fn farBranchSelector(&self) -> u16 {
		self.0.far_branch_selector()
	}

	/// Sets the operand's branch target selector. Use this method if the operand has kind [`OpKind.FarBranch16`] or [`OpKind.FarBranch32`]
	///
	/// [`OpKind.FarBranch16`]: enum.OpKind.html#variant.FarBranch16
	/// [`OpKind.FarBranch32`]: enum.OpKind.html#variant.FarBranch32
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_farBranchSelector(&mut self, newValue: u16) {
		self.0.set_far_branch_selector(newValue)
	}

	/// Gets the memory operand's base register (a [`Register`] enum value) or [`Register.None`] if none. Use this method if the operand has kind [`OpKind.Memory`]
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	#[wasm_bindgen(getter)]
	pub fn memoryBase(&self) -> Register {
		iced_to_register(self.0.memory_base())
	}

	/// Sets the memory operand's base register (a [`Register`] enum value) or [`Register.None`] if none. Use this method if the operand has kind [`OpKind.Memory`]
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_memoryBase(&mut self, newValue: Register) {
		self.0.set_memory_base(register_to_iced(newValue))
	}

	/// Gets the memory operand's index register (a [`Register`] enum value) or [`Register.None`] if none. Use this method if the operand has kind [`OpKind.Memory`]
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	#[wasm_bindgen(getter)]
	pub fn memoryIndex(&self) -> Register {
		iced_to_register(self.0.memory_index())
	}

	/// Sets the memory operand's index register (a [`Register`] enum value) or [`Register.None`] if none. Use this method if the operand has kind [`OpKind.Memory`]
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_memoryIndex(&mut self, newValue: Register) {
		self.0.set_memory_index(register_to_iced(newValue))
	}

	/// Gets operand #0's register value (a [`Register`] enum value). Use this method if operand #0 ([`op0Kind`]) has kind [`OpKind.Register`], see [`opCount`] and [`opRegister`]
	///
	/// [`Register`]: enum.Register.html
	/// [`op0Kind`]: #method.op0Kind
	/// [`opCount`]: #method.op_count
	/// [`opRegister`]: #method.opRegister
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	#[wasm_bindgen(getter)]
	pub fn op0Register(&self) -> Register {
		iced_to_register(self.0.op0_register())
	}

	/// Sets operand #0's register value (a [`Register`] enum value). Use this method if operand #0 ([`op0Kind`]) has kind [`OpKind.Register`], see [`opCount`] and [`opRegister`]
	///
	/// [`Register`]: enum.Register.html
	/// [`op0Kind`]: #method.op0Kind
	/// [`opCount`]: #method.op_count
	/// [`opRegister`]: #method.opRegister
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_op0Register(&mut self, newValue: Register) {
		self.0.set_op0_register(register_to_iced(newValue))
	}

	/// Gets operand #1's register value (a [`Register`] enum value). Use this method if operand #1 ([`op0Kind`]) has kind [`OpKind.Register`], see [`opCount`] and [`opRegister`]
	///
	/// [`Register`]: enum.Register.html
	/// [`op0Kind`]: #method.op0Kind
	/// [`opCount`]: #method.op_count
	/// [`opRegister`]: #method.opRegister
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	#[wasm_bindgen(getter)]
	pub fn op1Register(&self) -> Register {
		iced_to_register(self.0.op1_register())
	}

	/// Sets operand #1's register value (a [`Register`] enum value). Use this method if operand #1 ([`op0Kind`]) has kind [`OpKind.Register`], see [`opCount`] and [`opRegister`]
	///
	/// [`Register`]: enum.Register.html
	/// [`op0Kind`]: #method.op0Kind
	/// [`opCount`]: #method.op_count
	/// [`opRegister`]: #method.opRegister
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_op1Register(&mut self, newValue: Register) {
		self.0.set_op1_register(register_to_iced(newValue))
	}

	/// Gets operand #2's register value (a [`Register`] enum value). Use this method if operand #2 ([`op0Kind`]) has kind [`OpKind.Register`], see [`opCount`] and [`opRegister`]
	///
	/// [`Register`]: enum.Register.html
	/// [`op0Kind`]: #method.op0Kind
	/// [`opCount`]: #method.op_count
	/// [`opRegister`]: #method.opRegister
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	#[wasm_bindgen(getter)]
	pub fn op2Register(&self) -> Register {
		iced_to_register(self.0.op2_register())
	}

	/// Sets operand #2's register value (a [`Register`] enum value). Use this method if operand #2 ([`op0Kind`]) has kind [`OpKind.Register`], see [`opCount`] and [`opRegister`]
	///
	/// [`Register`]: enum.Register.html
	/// [`op0Kind`]: #method.op0Kind
	/// [`opCount`]: #method.op_count
	/// [`opRegister`]: #method.opRegister
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_op2Register(&mut self, newValue: Register) {
		self.0.set_op2_register(register_to_iced(newValue))
	}

	/// Gets operand #3's register value (a [`Register`] enum value). Use this method if operand #3 ([`op0Kind`]) has kind [`OpKind.Register`], see [`opCount`] and [`opRegister`]
	///
	/// [`Register`]: enum.Register.html
	/// [`op0Kind`]: #method.op0Kind
	/// [`opCount`]: #method.op_count
	/// [`opRegister`]: #method.opRegister
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	#[wasm_bindgen(getter)]
	pub fn op3Register(&self) -> Register {
		iced_to_register(self.0.op3_register())
	}

	/// Sets operand #3's register value (a [`Register`] enum value). Use this method if operand #3 ([`op0Kind`]) has kind [`OpKind.Register`], see [`opCount`] and [`opRegister`]
	///
	/// [`Register`]: enum.Register.html
	/// [`op0Kind`]: #method.op0Kind
	/// [`opCount`]: #method.op_count
	/// [`opRegister`]: #method.opRegister
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_op3Register(&mut self, newValue: Register) {
		self.0.set_op3_register(register_to_iced(newValue))
	}

	/// Gets operand #4's register value (a [`Register`] enum value). Use this method if operand #4 ([`op0Kind`]) has kind [`OpKind.Register`], see [`opCount`] and [`opRegister`]
	///
	/// [`Register`]: enum.Register.html
	/// [`op0Kind`]: #method.op0Kind
	/// [`opCount`]: #method.op_count
	/// [`opRegister`]: #method.opRegister
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	#[wasm_bindgen(getter)]
	pub fn op4Register(&self) -> Register {
		iced_to_register(self.0.op4_register())
	}

	/// Sets operand #4's register value (a [`Register`] enum value). Use this method if operand #4 ([`op0Kind`]) has kind [`OpKind.Register`], see [`opCount`] and [`opRegister`]
	///
	/// [`Register`]: enum.Register.html
	/// [`op0Kind`]: #method.op0Kind
	/// [`opCount`]: #method.op_count
	/// [`opRegister`]: #method.opRegister
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	///
	/// # Throws
	///
	/// Throws if `newValue` is invalid
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_op4Register(&mut self, newValue: Register) {
		self.0.set_op4_register(register_to_iced(newValue))
	}

	/// Gets the operand's register value (a [`Register`] enum value). Use this method if the operand has kind [`OpKind.Register`]
	///
	/// [`Register`]: enum.Register.html
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	///
	/// # Throws
	///
	/// Throws if `operand` is invalid
	///
	/// # Arguments
	///
	/// * `operand`: Operand number, 0-4
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Decoder, DecoderOptions, OpKind, Register } = require("iced-x86-js");
	///
	/// // add [rax],ebx
	/// const bytes = new Uint8Array([0x01, 0x18]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	/// const instr = decoder.decode();
	///
	/// assert.equal(instr.opCount, 2);
	/// assert.equal(instr.opKind(0), OpKind.Memory);
	/// assert.equal(instr.opKind(1), OpKind.Register);
	/// assert.equal(instr.opRegister(1), Register.EBX);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	pub fn opRegister(&self, operand: u32) -> Register {
		iced_to_register(self.0.op_register(operand))
	}

	/// Sets the operand's register value (a [`Register`] enum value). Use this method if the operand has kind [`OpKind.Register`]
	///
	/// [`Register`]: enum.Register.html
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	///
	/// # Throws
	///
	/// Throws if `operand` is invalid
	///
	/// # Arguments
	///
	/// * `operand`: Operand number, 0-4
	/// * `newValue`: New value
	#[cfg(feature = "encoder")]
	pub fn setOpRegister(&mut self, operand: u32, newValue: Register) {
		self.0.set_op_register(operand, register_to_iced(newValue))
	}

	/// Gets the op mask register ([`Register.K1`] - [`Register.K7`]) or [`Register.None`] if none
	///
	/// [`Register.K1`]: enum.Register.html#variant.K1
	/// [`Register.K7`]: enum.Register.html#variant.K7
	/// [`Register.None`]: enum.Register.html#variant.None
	#[wasm_bindgen(getter)]
	pub fn opMask(&self) -> Register {
		iced_to_register(self.0.op_mask())
	}

	/// Sets the op mask register ([`Register.K1`] - [`Register.K7`]) or [`Register.None`] if none
	///
	/// [`Register.K1`]: enum.Register.html#variant.K1
	/// [`Register.K7`]: enum.Register.html#variant.K7
	/// [`Register.None`]: enum.Register.html#variant.None
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_opMask(&mut self, newValue: Register) {
		self.0.set_op_mask(register_to_iced(newValue))
	}

	/// Checks if there's an op mask register ([`opMask`])
	///
	/// [`opMask`]: #method.opMask
	#[wasm_bindgen(getter)]
	pub fn hasOpMask(&self) -> bool {
		self.0.has_op_mask()
	}

	/// `true` if zeroing-masking, `false` if merging-masking.
	/// Only used by most EVEX encoded instructions that use op mask registers.
	#[wasm_bindgen(getter)]
	pub fn zeroingMasking(&self) -> bool {
		self.0.zeroing_masking()
	}

	/// `true` if zeroing-masking, `false` if merging-masking.
	/// Only used by most EVEX encoded instructions that use op mask registers.
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_zeroingMasking(&mut self, newValue: bool) {
		self.0.set_zeroing_masking(newValue)
	}

	/// `true` if merging-masking, `false` if zeroing-masking.
	/// Only used by most EVEX encoded instructions that use op mask registers.
	#[wasm_bindgen(getter)]
	pub fn mergingMasking(&self) -> bool {
		self.0.merging_masking()
	}

	/// `true` if merging-masking, `false` if zeroing-masking.
	/// Only used by most EVEX encoded instructions that use op mask registers.
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_mergingMasking(&mut self, newValue: bool) {
		self.0.set_merging_masking(newValue)
	}

	/// Gets the rounding control (a [`RoundingControl`] enum value) ([`suppressAllExceptions`] is implied but still returns `false`)
	/// or [`RoundingControl.None`] if the instruction doesn't use it.
	///
	/// [`RoundingControl`]: enum.RoundingControl.html
	/// [`suppressAllExceptions`]: #method.suppressAllExceptions
	/// [`RoundingControl.None`]: enum.RoundingControl.html#variant.None
	#[wasm_bindgen(getter)]
	pub fn roundingControl(&self) -> RoundingControl {
		iced_to_rounding_control(self.0.rounding_control())
	}

	/// Sets the rounding control (a [`RoundingControl`] enum value) ([`suppressAllExceptions`] is implied but still returns `false`)
	/// or [`RoundingControl.None`] if the instruction doesn't use it.
	///
	/// [`RoundingControl`]: enum.RoundingControl.html
	/// [`suppressAllExceptions`]: #method.suppressAllExceptions
	/// [`RoundingControl.None`]: enum.RoundingControl.html#variant.None
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_roundingControl(&mut self, newValue: RoundingControl) {
		self.0.set_rounding_control(rounding_control_to_iced(newValue))
	}

	/// Checks if this is a VSIB instruction, see also [`isVsib32`], [`isVsib64`]
	///
	/// [`isVsib32`]: #method.isVsib32
	/// [`isVsib64`]: #method.isVsib64
	#[wasm_bindgen(getter)]
	pub fn isVsib(&self) -> bool {
		self.0.is_vsib()
	}

	/// VSIB instructions only ([`isVsib`]): `true` if it's using 32-bit indexes, `false` if it's using 64-bit indexes
	///
	/// [`isVsib`]: #method.isVsib
	#[wasm_bindgen(getter)]
	pub fn isVsib32(&self) -> bool {
		self.0.is_vsib32()
	}

	/// VSIB instructions only ([`isVsib`]): `true` if it's using 64-bit indexes, `false` if it's using 32-bit indexes
	///
	/// [`isVsib`]: #method.isVsib
	#[wasm_bindgen(getter)]
	pub fn isVsib64(&self) -> bool {
		self.0.is_vsib64()
	}

	/// Checks if it's a vsib instruction.
	///
	/// # Returns
	///
	/// * `true` if it's a VSIB instruction with 64-bit indexes
	/// * `false` if it's a VSIB instruction with 32-bit indexes
	/// * `undefined` if it's not a VSIB instruction.
	#[wasm_bindgen(getter)]
	pub fn vsib(&self) -> Option<bool> {
		self.0.vsib()
	}

	/// Gets the suppress all exceptions flag (EVEX encoded instructions). Note that if [`roundingControl`] is
	/// not [`RoundingControl.None`], SAE is implied but this method will still return `false`.
	///
	/// [`roundingControl`]: #method.roundingControl
	/// [`RoundingControl.None`]: enum.RoundingControl.html#variant.None
	#[wasm_bindgen(getter)]
	pub fn suppressAllExceptions(&self) -> bool {
		self.0.suppress_all_exceptions()
	}

	/// Sets the suppress all exceptions flag (EVEX encoded instructions). Note that if [`roundingControl`] is
	/// not [`RoundingControl.None`], SAE is implied but this method will still return `false`.
	///
	/// [`roundingControl`]: #method.roundingControl
	/// [`RoundingControl.None`]: enum.RoundingControl.html#variant.None
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_suppressAllExceptions(&mut self, newValue: bool) {
		self.0.set_suppress_all_exceptions(newValue)
	}

	/// Checks if the memory operand is `RIP`/`EIP` relative
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isIpRelMemoryOperand")]
	pub fn is_ip_rel_memory_operand(&self) -> bool {
		self.0.is_ip_rel_memory_operand()
	}

	/// Gets the low 32 bits of the `RIP`/`EIP` releative address (([`nextIP`] or [`nextIP32`]) + [`memoryDisplacement`]).
	/// This method is only valid if there's a memory operand with `RIP`/`EIP` relative addressing, see [`isIpRelMemoryOperand`].
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`nextIP`]: #method.nextIP
	/// [`nextIP32`]: #method.nextIP32
	/// [`memoryDisplacement`]: #method.memoryDisplacement
	/// [`isIpRelMemoryOperand`]: #method.is_ip_rel_memory_operand
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "ipRelMemoryAddressLo")]
	#[cfg(not(feature = "bigint"))]
	pub fn ip_rel_memory_address_lo(&self) -> u32 {
		self.0.ip_rel_memory_address() as u32
	}

	/// Gets the high 32 bits of the `RIP`/`EIP` releative address (([`nextIP`] or [`nextIP32`]) + [`memoryDisplacement`]).
	/// This method is only valid if there's a memory operand with `RIP`/`EIP` relative addressing, see [`isIpRelMemoryOperand`].
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`nextIP`]: #method.nextIP
	/// [`nextIP32`]: #method.nextIP32
	/// [`memoryDisplacement`]: #method.memoryDisplacement
	/// [`isIpRelMemoryOperand`]: #method.is_ip_rel_memory_operand
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "ipRelMemoryAddressHi")]
	#[cfg(not(feature = "bigint"))]
	pub fn ip_rel_memory_address_hi(&self) -> u32 {
		(self.0.ip_rel_memory_address() >> 32) as u32
	}

	/// Gets the `RIP`/`EIP` releative address (([`nextIP`] or [`nextIP32`]) + [`memoryDisplacement`]).
	/// This method is only valid if there's a memory operand with `RIP`/`EIP` relative addressing, see [`isIpRelMemoryOperand`]
	///
	/// [`nextIP`]: #method.nextIP
	/// [`nextIP32`]: #method.nextIP32
	/// [`memoryDisplacement`]: #method.memoryDisplacement
	/// [`isIpRelMemoryOperand`]: #method.is_ip_rel_memory_operand
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "ipRelMemoryAddress")]
	#[cfg(feature = "bigint")]
	pub fn ip_rel_memory_address(&self) -> u64 {
		self.0.ip_rel_memory_address()
	}
}

#[wasm_bindgen]
#[cfg(feature = "instr_info")]
#[cfg(feature = "instr_api")]
impl Instruction {
	/// Gets the number of bytes added to `SP`/`ESP`/`RSP` or 0 if it's not an instruction that pushes or pops data. This method assumes
	/// the instruction doesn't change the privilege level (eg. `IRET/D/Q`). If it's the `LEAVE` instruction, this method returns 0.
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Decoder, DecoderOptions } = require("iced-x86-js");
	///
	/// // pushfq
	/// const bytes = new Uint8Array([0x9C]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	/// const instr = decoder.decode();
	///
	/// assert.ok(instr.isStackInstruction);
	/// assert.equal(instr.stackPointerIncrement, -8);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "stackPointerIncrement")]
	pub fn stack_pointer_increment(&self) -> i32 {
		self.0.stack_pointer_increment()
	}

	/// Instruction encoding (a [`EncodingKind`] enum value), eg. legacy, VEX, EVEX, ...
	///
	/// [`EncodingKind`]: enum.EncodingKind.html
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Decoder, DecoderOptions, EncodingKind } = require("iced-x86-js");
	///
	/// // vmovaps xmm1,xmm5
	/// const bytes = new Uint8Array([0xC5, 0xF8, 0x28, 0xCD]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	/// const instr = decoder.decode();
	///
	/// assert.equal(instr.encoding, EncodingKind.VEX);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	#[wasm_bindgen(getter)]
	pub fn encoding(&self) -> EncodingKind {
		iced_to_encoding_kind(self.0.encoding())
	}

	/// Gets the CPU or CPUID feature flags (an array of [`CpuidFeature`] values)
	///
	/// [`CpuidFeature`]: enum.CpuidFeature.html
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Decoder, DecoderOptions, CpuidFeature } = require("iced-x86-js");
	///
	/// // vmovaps xmm1,xmm5
	/// // vmovaps xmm10{k3}{z},xmm19
	/// const bytes = new Uint8Array([0xC5, 0xF8, 0x28, 0xCD, 0x62, 0x31, 0x7C, 0x8B, 0x28, 0xD3]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	///
	/// // vmovaps xmm1,xmm5
	/// const instr = decoder.decode();
	/// let cpuid = instr.cpuidFeatures();
	/// assert.equal(cpuid.length, 1);
	/// assert.equal(cpuid[0], CpuidFeature.AVX);
	///
	/// // vmovaps xmm10{k3}{z},xmm19
	/// decoder.decodeOut(instr);
	/// cpuid = instr.cpuidFeatures();
	/// assert.equal(cpuid.length, 2);
	/// assert.equal(cpuid[0], CpuidFeature.AVX512VL);
	/// assert.equal(cpuid[1], CpuidFeature.AVX512F);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	#[wasm_bindgen(js_name = "cpuidFeatures")]
	pub fn cpuid_features(&self) -> Vec<i32> {
		// It's not possible to return a Vec<CpuidFeature>
		self.0.cpuid_features().iter().map(|&a| a as i32).collect()
	}

	/// Flow control info (a [`FlowControl`] enum value)
	///
	/// [`FlowControl`]: enum.FlowControl.html
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Decoder, DecoderOptions, FlowControl } = require("iced-x86-js");
	///
	/// // or ecx,esi
	/// // ud0 rcx,rsi
	/// // call rcx
	/// const bytes = new Uint8Array([0x0B, 0xCE, 0x48, 0x0F, 0xFF, 0xCE, 0xFF, 0xD1]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	///
	/// // or ecx,esi
	/// const instr = decoder.decode();
	/// assert.equal(instr.flowControl, FlowControl.Next);
	///
	/// // ud0 rcx,rsi
	/// decoder.decodeOut(instr);
	/// assert.equal(instr.flowControl, FlowControl.Exception);
	///
	/// // call rcx
	/// decoder.decodeOut(instr);
	/// assert.equal(instr.flowControl, FlowControl.IndirectCall);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "flowControl")]
	pub fn flow_control(&self) -> FlowControl {
		iced_to_flow_control(self.0.flow_control())
	}

	/// `true` if the instruction isn't available in real mode or virtual 8086 mode
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isProtectedMode")]
	pub fn is_protected_mode(&self) -> bool {
		self.0.is_protected_mode()
	}

	/// `true` if this is a privileged instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isPrivileged")]
	pub fn is_privileged(&self) -> bool {
		self.0.is_privileged()
	}

	/// `true` if this is an instruction that implicitly uses the stack pointer (`SP`/`ESP`/`RSP`), eg. `CALL`, `PUSH`, `POP`, `RET`, etc.
	/// See also [`stackPointerIncrement`]
	///
	/// [`stackPointerIncrement`]: #method.stack_pointer_increment
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Decoder, DecoderOptions } = require("iced-x86-js");
	///
	/// // or ecx,esi
	/// // push rax
	/// const bytes = new Uint8Array([0x0B, 0xCE, 0x50]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	///
	/// // or ecx,esi
	/// const instr = decoder.decode();
	/// assert.ok(!instr.isStackInstruction);
	///
	/// // push rax
	/// decoder.decodeOut(instr);
	/// assert.ok(instr.isStackInstruction);
	/// assert.equal(instr.stackPointerIncrement, -8);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isStackInstruction")]
	pub fn is_stack_instruction(&self) -> bool {
		self.0.is_stack_instruction()
	}

	/// `true` if it's an instruction that saves or restores too many registers (eg. `FXRSTOR`, `XSAVE`, etc).
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isSaveRestoreInstruction")]
	pub fn is_save_restore_instruction(&self) -> bool {
		self.0.is_save_restore_instruction()
	}

	/// All flags that are read by the CPU when executing the instruction.
	/// This method returns a [`RflagsBits`] value. See also [`rflagsModified`].
	///
	/// [`RflagsBits`]: enum.RflagsBits.html
	/// [`rflagsModified`]: #method.rflags_modified
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Decoder, DecoderOptions, RflagsBits } = require("iced-x86-js");
	///
	/// // adc rsi,rcx
	/// // xor rdi,5Ah
	/// const bytes = new Uint8Array([0x48, 0x11, 0xCE, 0x48, 0x83, 0xF7, 0x5A]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	///
	/// // adc rsi,rcx
	/// const instr = decoder.decode();
	/// assert.equal(instr.rflagsRead, RflagsBits.CF);
	/// assert.equal(instr.rflagsWritten, RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF);
	/// assert.equal(instr.rflagsCleared, RflagsBits.None);
	/// assert.equal(instr.rflagsSet, RflagsBits.None);
	/// assert.equal(instr.rflagsUndefined, RflagsBits.None);
	/// assert.equal(instr.rflagsModified, RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF);
	///
	/// // xor rdi,5Ah
	/// decoder.decodeOut(instr);
	/// assert.equal(instr.rflagsRead, RflagsBits.None);
	/// assert.equal(instr.rflagsWritten, RflagsBits.SF | RflagsBits.ZF | RflagsBits.PF);
	/// assert.equal(instr.rflagsCleared, RflagsBits.OF | RflagsBits.CF);
	/// assert.equal(instr.rflagsSet, RflagsBits.None);
	/// assert.equal(instr.rflagsUndefined, RflagsBits.AF);
	/// assert.equal(instr.rflagsModified, RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "rflagsRead")]
	pub fn rflags_read(&self) -> u32 {
		self.0.rflags_read()
	}

	/// All flags that are written by the CPU, except those flags that are known to be undefined, always set or always cleared.
	/// This method returns a [`RflagsBits`] value. See also [`rflagsModified`].
	///
	/// [`RflagsBits`]: enum.RflagsBits.html
	/// [`rflagsModified`]: #method.rflags_modified
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Decoder, DecoderOptions, RflagsBits } = require("iced-x86-js");
	///
	/// // adc rsi,rcx
	/// // xor rdi,5Ah
	/// const bytes = new Uint8Array([0x48, 0x11, 0xCE, 0x48, 0x83, 0xF7, 0x5A]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	///
	/// // adc rsi,rcx
	/// const instr = decoder.decode();
	/// assert.equal(instr.rflagsRead, RflagsBits.CF);
	/// assert.equal(instr.rflagsWritten, RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF);
	/// assert.equal(instr.rflagsCleared, RflagsBits.None);
	/// assert.equal(instr.rflagsSet, RflagsBits.None);
	/// assert.equal(instr.rflagsUndefined, RflagsBits.None);
	/// assert.equal(instr.rflagsModified, RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF);
	///
	/// // xor rdi,5Ah
	/// decoder.decodeOut(instr);
	/// assert.equal(instr.rflagsRead, RflagsBits.None);
	/// assert.equal(instr.rflagsWritten, RflagsBits.SF | RflagsBits.ZF | RflagsBits.PF);
	/// assert.equal(instr.rflagsCleared, RflagsBits.OF | RflagsBits.CF);
	/// assert.equal(instr.rflagsSet, RflagsBits.None);
	/// assert.equal(instr.rflagsUndefined, RflagsBits.AF);
	/// assert.equal(instr.rflagsModified, RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "rflagsWritten")]
	pub fn rflags_written(&self) -> u32 {
		self.0.rflags_written()
	}

	/// All flags that are always cleared by the CPU.
	/// This method returns a [`RflagsBits`] value. See also [`rflagsModified`].
	///
	/// [`RflagsBits`]: enum.RflagsBits.html
	/// [`rflagsModified`]: #method.rflags_modified
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Decoder, DecoderOptions, RflagsBits } = require("iced-x86-js");
	///
	/// // adc rsi,rcx
	/// // xor rdi,5Ah
	/// const bytes = new Uint8Array([0x48, 0x11, 0xCE, 0x48, 0x83, 0xF7, 0x5A]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	///
	/// // adc rsi,rcx
	/// const instr = decoder.decode();
	/// assert.equal(instr.rflagsRead, RflagsBits.CF);
	/// assert.equal(instr.rflagsWritten, RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF);
	/// assert.equal(instr.rflagsCleared, RflagsBits.None);
	/// assert.equal(instr.rflagsSet, RflagsBits.None);
	/// assert.equal(instr.rflagsUndefined, RflagsBits.None);
	/// assert.equal(instr.rflagsModified, RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF);
	///
	/// // xor rdi,5Ah
	/// decoder.decodeOut(instr);
	/// assert.equal(instr.rflagsRead, RflagsBits.None);
	/// assert.equal(instr.rflagsWritten, RflagsBits.SF | RflagsBits.ZF | RflagsBits.PF);
	/// assert.equal(instr.rflagsCleared, RflagsBits.OF | RflagsBits.CF);
	/// assert.equal(instr.rflagsSet, RflagsBits.None);
	/// assert.equal(instr.rflagsUndefined, RflagsBits.AF);
	/// assert.equal(instr.rflagsModified, RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "rflagsCleared")]
	pub fn rflags_cleared(&self) -> u32 {
		self.0.rflags_cleared()
	}

	/// All flags that are always set by the CPU.
	/// This method returns a [`RflagsBits`] value. See also [`rflagsModified`].
	///
	/// [`RflagsBits`]: enum.RflagsBits.html
	/// [`rflagsModified`]: #method.rflags_modified
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Decoder, DecoderOptions, RflagsBits } = require("iced-x86-js");
	///
	/// // adc rsi,rcx
	/// // xor rdi,5Ah
	/// const bytes = new Uint8Array([0x48, 0x11, 0xCE, 0x48, 0x83, 0xF7, 0x5A]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	///
	/// // adc rsi,rcx
	/// const instr = decoder.decode();
	/// assert.equal(instr.rflagsRead, RflagsBits.CF);
	/// assert.equal(instr.rflagsWritten, RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF);
	/// assert.equal(instr.rflagsCleared, RflagsBits.None);
	/// assert.equal(instr.rflagsSet, RflagsBits.None);
	/// assert.equal(instr.rflagsUndefined, RflagsBits.None);
	/// assert.equal(instr.rflagsModified, RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF);
	///
	/// // xor rdi,5Ah
	/// decoder.decodeOut(instr);
	/// assert.equal(instr.rflagsRead, RflagsBits.None);
	/// assert.equal(instr.rflagsWritten, RflagsBits.SF | RflagsBits.ZF | RflagsBits.PF);
	/// assert.equal(instr.rflagsCleared, RflagsBits.OF | RflagsBits.CF);
	/// assert.equal(instr.rflagsSet, RflagsBits.None);
	/// assert.equal(instr.rflagsUndefined, RflagsBits.AF);
	/// assert.equal(instr.rflagsModified, RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "rflagsSet")]
	pub fn rflags_set(&self) -> u32 {
		self.0.rflags_set()
	}

	/// All flags that are undefined after executing the instruction.
	/// This method returns a [`RflagsBits`] value. See also [`rflagsModified`].
	///
	/// [`RflagsBits`]: enum.RflagsBits.html
	/// [`rflagsModified`]: #method.rflags_modified
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Decoder, DecoderOptions, RflagsBits } = require("iced-x86-js");
	///
	/// // adc rsi,rcx
	/// // xor rdi,5Ah
	/// const bytes = new Uint8Array([0x48, 0x11, 0xCE, 0x48, 0x83, 0xF7, 0x5A]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	///
	/// // adc rsi,rcx
	/// const instr = decoder.decode();
	/// assert.equal(instr.rflagsRead, RflagsBits.CF);
	/// assert.equal(instr.rflagsWritten, RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF);
	/// assert.equal(instr.rflagsCleared, RflagsBits.None);
	/// assert.equal(instr.rflagsSet, RflagsBits.None);
	/// assert.equal(instr.rflagsUndefined, RflagsBits.None);
	/// assert.equal(instr.rflagsModified, RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF);
	///
	/// // xor rdi,5Ah
	/// decoder.decodeOut(instr);
	/// assert.equal(instr.rflagsRead, RflagsBits.None);
	/// assert.equal(instr.rflagsWritten, RflagsBits.SF | RflagsBits.ZF | RflagsBits.PF);
	/// assert.equal(instr.rflagsCleared, RflagsBits.OF | RflagsBits.CF);
	/// assert.equal(instr.rflagsSet, RflagsBits.None);
	/// assert.equal(instr.rflagsUndefined, RflagsBits.AF);
	/// assert.equal(instr.rflagsModified, RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "rflagsUndefined")]
	pub fn rflags_undefined(&self) -> u32 {
		self.0.rflags_undefined()
	}

	/// All flags that are modified by the CPU. This is `rflagsWritten + rflagsCleared + rflagsSet + rflagsUndefined`.
	/// This method returns a [`RflagsBits`] value.
	///
	/// [`RflagsBits`]: enum.RflagsBits.html
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Decoder, DecoderOptions, RflagsBits } = require("iced-x86-js");
	///
	/// // adc rsi,rcx
	/// // xor rdi,5Ah
	/// const bytes = new Uint8Array([0x48, 0x11, 0xCE, 0x48, 0x83, 0xF7, 0x5A]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	///
	/// // adc rsi,rcx
	/// const instr = decoder.decode();
	/// assert.equal(instr.rflagsRead, RflagsBits.CF);
	/// assert.equal(instr.rflagsWritten, RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF);
	/// assert.equal(instr.rflagsCleared, RflagsBits.None);
	/// assert.equal(instr.rflagsSet, RflagsBits.None);
	/// assert.equal(instr.rflagsUndefined, RflagsBits.None);
	/// assert.equal(instr.rflagsModified, RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF);
	///
	/// // xor rdi,5Ah
	/// decoder.decodeOut(instr);
	/// assert.equal(instr.rflagsRead, RflagsBits.None);
	/// assert.equal(instr.rflagsWritten, RflagsBits.SF | RflagsBits.ZF | RflagsBits.PF);
	/// assert.equal(instr.rflagsCleared, RflagsBits.OF | RflagsBits.CF);
	/// assert.equal(instr.rflagsSet, RflagsBits.None);
	/// assert.equal(instr.rflagsUndefined, RflagsBits.AF);
	/// assert.equal(instr.rflagsModified, RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "rflagsModified")]
	pub fn rflags_modified(&self) -> u32 {
		self.0.rflags_modified()
	}

	/// Checks if it's a `Jcc SHORT` or `Jcc NEAR` instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isJccShortOrNear")]
	pub fn is_jcc_short_or_near(&self) -> bool {
		self.0.is_jcc_short_or_near()
	}

	/// Checks if it's a `Jcc NEAR` instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isJccNear")]
	pub fn is_jcc_near(&self) -> bool {
		self.0.is_jcc_near()
	}

	/// Checks if it's a `Jcc SHORT` instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isJccShort")]
	pub fn is_jcc_short(&self) -> bool {
		self.0.is_jcc_short()
	}

	/// Checks if it's a `JMP SHORT` instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isJmpShort")]
	pub fn is_jmp_short(&self) -> bool {
		self.0.is_jmp_short()
	}

	/// Checks if it's a `JMP NEAR` instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isJmpNear")]
	pub fn is_jmp_near(&self) -> bool {
		self.0.is_jmp_near()
	}

	/// Checks if it's a `JMP SHORT` or a `JMP NEAR` instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isJmpShortOrNear")]
	pub fn is_jmp_short_or_near(&self) -> bool {
		self.0.is_jmp_short_or_near()
	}

	/// Checks if it's a `JMP FAR` instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isJmpFar")]
	pub fn is_jmp_far(&self) -> bool {
		self.0.is_jmp_far()
	}

	/// Checks if it's a `CALL NEAR` instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isCallNear")]
	pub fn is_call_near(&self) -> bool {
		self.0.is_call_near()
	}

	/// Checks if it's a `CALL FAR` instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isCallFar")]
	pub fn is_call_far(&self) -> bool {
		self.0.is_call_far()
	}

	/// Checks if it's a `JMP NEAR reg/[mem]` instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isJmpNearIndirect")]
	pub fn is_jmp_near_indirect(&self) -> bool {
		self.0.is_jmp_near_indirect()
	}

	/// Checks if it's a `JMP FAR [mem]` instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isJmpFarIndirect")]
	pub fn is_jmp_far_indirect(&self) -> bool {
		self.0.is_jmp_far_indirect()
	}

	/// Checks if it's a `CALL NEAR reg/[mem]` instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isCallNearIndirect")]
	pub fn is_call_near_indirect(&self) -> bool {
		self.0.is_call_near_indirect()
	}

	/// Checks if it's a `CALL FAR [mem]` instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isCallFarIndirect")]
	pub fn is_call_far_indirect(&self) -> bool {
		self.0.is_call_far_indirect()
	}

	/// Negates the condition code, eg. `JE` -> `JNE`. Can be used if it's `Jcc`, `SETcc`, `CMOVcc` and does
	/// nothing if the instruction doesn't have a condition code.
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Code, ConditionCode, Decoder, DecoderOptions } = require("iced-x86-js");
	///
	/// // setbe al
	/// const bytes = new Uint8Array([0x0F, 0x96, 0xC0]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	///
	/// const instr = decoder.decode();
	/// assert.equal(instr.code, Code.Setbe_rm8);
	/// assert.equal(instr.conditionCode, ConditionCode.be);
	/// instr.negateConditionCode();
	/// assert.equal(instr.code, Code.Seta_rm8);
	/// assert.equal(instr.conditionCode, ConditionCode.a);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	#[wasm_bindgen(js_name = "negateConditionCode")]
	#[cfg(feature = "encoder")]
	pub fn negate_condition_code(&mut self) {
		self.0.negate_condition_code()
	}

	/// Converts `Jcc/JMP NEAR` to `Jcc/JMP SHORT` and does nothing if it's not a `Jcc/JMP NEAR` instruction
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Code, Decoder, DecoderOptions } = require("iced-x86-js");
	///
	/// // jbe near ptr label
	/// const bytes = new Uint8Array([0x0F, 0x86, 0x5A, 0xA5, 0x12, 0x34]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	///
	/// const instr = decoder.decode();
	/// assert.equal(instr.code, Code.Jbe_rel32_64);
	/// instr.asShortBranch();
	/// assert.equal(instr.code, Code.Jbe_rel8_64);
	/// instr.asShortBranch();
	/// assert.equal(instr.code, Code.Jbe_rel8_64);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	#[wasm_bindgen(js_name = "asShortBranch")]
	#[cfg(feature = "encoder")]
	pub fn as_short_branch(&mut self) {
		self.0.as_short_branch()
	}

	/// Converts `Jcc/JMP SHORT` to `Jcc/JMP NEAR` and does nothing if it's not a `Jcc/JMP SHORT` instruction
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Code, Decoder, DecoderOptions } = require("iced-x86-js");
	///
	/// // jbe short label
	/// const bytes = new Uint8Array([0x76, 0x5A]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	///
	/// const instr = decoder.decode();
	/// assert.equal(instr.code, Code.Jbe_rel8_64);
	/// instr.asNearBranch();
	/// assert.equal(instr.code, Code.Jbe_rel32_64);
	/// instr.asNearBranch();
	/// assert.equal(instr.code, Code.Jbe_rel32_64);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	#[wasm_bindgen(js_name = "asNearBranch")]
	#[cfg(feature = "encoder")]
	pub fn as_near_branch(&mut self) {
		self.0.as_near_branch()
	}

	/// Gets the condition code (a [`ConditionCode`] enum value) if it's `Jcc`, `SETcc`, `CMOVcc` else [`ConditionCode.None`] is returned
	///
	/// [`ConditionCode`]: enum.ConditionCode.html
	/// [`ConditionCode.None`]: enum.ConditionCode.html#variant.None
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { ConditionCode, Decoder, DecoderOptions } = require("iced-x86-js");
	///
	/// // setbe al
	/// // jl short label
	/// // cmovne ecx,esi
	/// // nop
	/// const bytes = new Uint8Array([0x0F, 0x96, 0xC0, 0x7C, 0x5A, 0x0F, 0x45, 0xCE, 0x90]);
	/// const decoder = new Decoder(64, bytes, DecoderOptions.None);
	///
	/// // setbe al
	/// const instr = decoder.decode();
	/// assert.equal(instr.conditionCode, ConditionCode.be);
	///
	/// // jl short label
	/// decoder.decodeOut(instr);
	/// assert.equal(instr.conditionCode, ConditionCode.l);
	///
	/// // cmovne ecx,esi
	/// decoder.decodeOut(instr);
	/// assert.equal(instr.conditionCode, ConditionCode.ne);
	///
	/// // nop
	/// decoder.decodeOut(instr);
	/// assert.equal(instr.conditionCode, ConditionCode.None);
	///
	/// // Free wasm memory
	/// decoder.free();
	/// instr.free();
	/// ```
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "conditionCode")]
	pub fn condition_code(&self) -> ConditionCode {
		iced_to_condition_code(self.0.condition_code())
	}
}

#[wasm_bindgen]
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
#[cfg(feature = "instr_api")]
impl Instruction {
	/// Gets the [`OpCodeInfo`]
	///
	/// [`OpCodeInfo`]: struct.OpCodeInfo.html
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "opCode")]
	pub fn op_code(&self) -> OpCodeInfo {
		OpCodeInfo(self.0.op_code())
	}
}

#[wasm_bindgen]
#[cfg(feature = "instr_api")]
impl Instruction {
	/// Checks if this instance is equal to another instance. It ignores some fields,
	/// such as the IP address, code size, etc.
	pub fn equals(&self, other: &Instruction) -> bool {
		self.0.eq(&other.0)
	}

	/// Clones this instance
	#[wasm_bindgen(js_name = "clone")]
	pub fn clone_js(&self) -> Self {
		Self(self.0)
	}
}

#[wasm_bindgen]
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
impl Instruction {
	/// Gets the default disassembly string
	#[wasm_bindgen(js_name = "toString")]
	pub fn to_string_js(&self) -> String {
		self.0.to_string()
	}
}

#[wasm_bindgen]
#[cfg(feature = "instr_create")]
impl Instruction {
	/// Gets the number of elements in a `db`/`dw`/`dd`/`dq` directive.
	/// Can only be called if [`code`] is [`Code.DeclareByte`], [`Code.DeclareWord`], [`Code.DeclareDword`], [`Code.DeclareQword`]
	///
	/// [`code`]: #method.code
	/// [`Code.DeclareByte`]: enum.Code.html#variant.DeclareByte
	/// [`Code.DeclareWord`]: enum.Code.html#variant.DeclareWord
	/// [`Code.DeclareDword`]: enum.Code.html#variant.DeclareDword
	/// [`Code.DeclareQword`]: enum.Code.html#variant.DeclareQword
	#[wasm_bindgen(getter)]
	pub fn declareDataLength(&self) -> u32 {
		self.0.declare_data_len() as u32
	}

	/// Sets the number of elements in a `db`/`dw`/`dd`/`dq` directive.
	/// Can only be called if [`code`] is [`Code.DeclareByte`], [`Code.DeclareWord`], [`Code.DeclareDword`], [`Code.DeclareQword`]
	///
	/// [`code`]: #method.code
	/// [`Code.DeclareByte`]: enum.Code.html#variant.DeclareByte
	/// [`Code.DeclareWord`]: enum.Code.html#variant.DeclareWord
	/// [`Code.DeclareDword`]: enum.Code.html#variant.DeclareDword
	/// [`Code.DeclareQword`]: enum.Code.html#variant.DeclareQword
	///
	/// # Arguments
	///
	/// * `newValue`: New value: `db`: 1-16; `dw`: 1-8; `dd`: 1-4; `dq`: 1-2
	#[wasm_bindgen(setter)]
	pub fn set_declareDataLength(&mut self, newValue: u32) {
		self.0.set_declare_data_len(newValue as usize)
	}

	/// Sets a new `db` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareByte`]
	///
	/// [`declareDataLength()`]: #method.declareDataLength
	/// [`code`]: #method.code
	/// [`Code.DeclareByte`]: enum.Code.html#variant.DeclareByte
	///
	/// # Throws
	///
	/// Throws if `index` is invalid
	///
	/// # Arguments
	///
	/// * `index`: Index (0-15)
	/// * `newValue`: New value
	#[wasm_bindgen(js_name = "setDeclareByteValueI8")]
	pub fn set_declare_byte_value_i8(&mut self, index: u32, newValue: i8) {
		self.0.set_declare_byte_value(index as usize, newValue as u8)
	}

	/// Sets a new `db` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareByte`]
	///
	/// [`declareDataLength`]: #method.declareDataLength
	/// [`code`]: #method.code
	/// [`Code.DeclareByte`]: enum.Code.html#variant.DeclareByte
	///
	/// # Throws
	///
	/// Throws if `index` is invalid
	///
	/// # Arguments
	///
	/// * `index`: Index (0-15)
	/// * `newValue`: New value
	#[wasm_bindgen(js_name = "setDeclareByteValue")]
	pub fn set_declare_byte_value(&mut self, index: u32, newValue: u8) {
		self.0.set_declare_byte_value(index as usize, newValue)
	}

	/// Gets a `db` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareByte`]
	///
	/// [`declareDataLength`]: #method.declareDataLength
	/// [`code`]: #method.code
	/// [`Code.DeclareByte`]: enum.Code.html#variant.DeclareByte
	///
	/// # Throws
	///
	/// Throws if `index` is invalid
	///
	/// # Arguments
	///
	/// * `index`: Index (0-15)
	#[wasm_bindgen(js_name = "getDeclareByteValue")]
	pub fn get_declare_byte_value(&self, index: u32) -> u8 {
		self.0.get_declare_byte_value(index as usize)
	}

	/// Sets a new `dw` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareWord`]
	///
	/// [`declareDataLength`]: #method.declareDataLength
	/// [`code`]: #method.code
	/// [`Code.DeclareWord`]: enum.Code.html#variant.DeclareWord
	///
	/// # Throws
	///
	/// Throws if `index` is invalid
	///
	/// # Arguments
	///
	/// * `index`: Index (0-7)
	/// * `newValue`: New value
	#[wasm_bindgen(js_name = "setDeclareWordValueI16")]
	pub fn set_declare_word_value_i16(&mut self, index: u32, newValue: i16) {
		self.0.set_declare_word_value_i16(index as usize, newValue)
	}

	/// Sets a new `dw` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareWord`]
	///
	/// [`declareDataLength`]: #method.declareDataLength
	/// [`code`]: #method.code
	/// [`Code.DeclareWord`]: enum.Code.html#variant.DeclareWord
	///
	/// # Throws
	///
	/// Throws if `index` is invalid
	///
	/// # Arguments
	///
	/// * `index`: Index (0-7)
	/// * `newValue`: New value
	#[wasm_bindgen(js_name = "setDeclareWordValue")]
	pub fn set_declare_word_value(&mut self, index: u32, newValue: u16) {
		self.0.set_declare_word_value(index as usize, newValue)
	}

	/// Gets a `dw` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareWord`]
	///
	/// [`declareDataLength`]: #method.declareDataLength
	/// [`code`]: #method.code
	/// [`Code.DeclareWord`]: enum.Code.html#variant.DeclareWord
	///
	/// # Throws
	///
	/// Throws if `index` is invalid
	///
	/// # Arguments
	///
	/// * `index`: Index (0-7)
	#[wasm_bindgen(js_name = "getDeclareWordValue")]
	pub fn get_declare_word_value(&self, index: u32) -> u16 {
		self.0.get_declare_word_value(index as usize)
	}

	/// Sets a new `dd` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareDword`]
	///
	/// [`declareDataLength`]: #method.declareDataLength
	/// [`code`]: #method.code
	/// [`Code.DeclareDword`]: enum.Code.html#variant.DeclareDword
	///
	/// # Throws
	///
	/// Throws if `index` is invalid
	///
	/// # Arguments
	///
	/// * `index`: Index (0-3)
	/// * `newValue`: New value
	#[wasm_bindgen(js_name = "setDeclareDwordValueI32")]
	pub fn set_declare_dword_value_i32(&mut self, index: u32, newValue: i32) {
		self.0.set_declare_dword_value_i32(index as usize, newValue)
	}

	/// Sets a new `dd` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareDword`]
	///
	/// [`declareDataLength`]: #method.declareDataLength
	/// [`code`]: #method.code
	/// [`Code.DeclareDword`]: enum.Code.html#variant.DeclareDword
	///
	/// # Throws
	///
	/// Throws if `index` is invalid
	///
	/// # Arguments
	///
	/// * `index`: Index (0-3)
	/// * `newValue`: New value
	#[wasm_bindgen(js_name = "setDeclareDwordValue")]
	pub fn set_declare_dword_value(&mut self, index: u32, newValue: u32) {
		self.0.set_declare_dword_value(index as usize, newValue)
	}

	/// Gets a `dd` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareDword`]
	///
	/// [`declareDataLength`]: #method.declareDataLength
	/// [`code`]: #method.code
	/// [`Code.DeclareDword`]: enum.Code.html#variant.DeclareDword
	///
	/// # Throws
	///
	/// Throws if `index` is invalid
	///
	/// # Arguments
	///
	/// * `index`: Index (0-3)
	#[wasm_bindgen(js_name = "getDeclareDwordValue")]
	pub fn get_declare_dword_value(&self, index: u32) -> u32 {
		self.0.get_declare_dword_value(index as usize)
	}

	/// Sets a new `dq` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareQword`]
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`declareDataLength`]: #method.declareDataLength
	/// [`code`]: #method.code
	/// [`Code.DeclareQword`]: enum.Code.html#variant.DeclareQword
	///
	/// # Throws
	///
	/// Throws if `index` is invalid
	///
	/// # Arguments
	///
	/// * `index`: Index (0-1)
	/// * `newValueHi`: High 32 bits of new value
	/// * `newValueLo`: Low 32 bits of new value
	#[wasm_bindgen(js_name = "setDeclareQwordValueI64")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_declare_qword_value_i64(&mut self, index: u32, newValueHi: u32, newValueLo: u32) {
		let new_value = (((newValueHi as u64) << 32) | (newValueLo as u64)) as i64;
		self.0.set_declare_qword_value_i64(index as usize, new_value)
	}

	/// Sets a new `dq` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareQword`]
	///
	/// [`declareDataLength`]: #method.declareDataLength
	/// [`code`]: #method.code
	/// [`Code.DeclareQword`]: enum.Code.html#variant.DeclareQword
	///
	/// # Throws
	///
	/// Throws if `index` is invalid
	///
	/// # Arguments
	///
	/// * `index`: Index (0-1)
	/// * `newValue`: New value
	#[wasm_bindgen(js_name = "setDeclareQwordValueI64")]
	#[cfg(feature = "bigint")]
	pub fn set_declare_qword_value_i64(&mut self, index: u32, newValue: i64) {
		self.0.set_declare_qword_value_i64(index as usize, newValue)
	}

	/// Sets a new `dq` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareQword`]
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`declareDataLength`]: #method.declareDataLength
	/// [`code`]: #method.code
	/// [`Code.DeclareQword`]: enum.Code.html#variant.DeclareQword
	///
	/// # Throws
	///
	/// Throws if `index` is invalid
	///
	/// # Arguments
	///
	/// * `index`: Index (0-1)
	/// * `newValueHi`: High 32 bits of new value
	/// * `newValueLo`: Low 32 bits of new value
	#[wasm_bindgen(js_name = "setDeclareQwordValue")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_declare_qword_value(&mut self, index: u32, newValueHi: u32, newValueLo: u32) {
		let new_value = ((newValueHi as u64) << 32) | (newValueLo as u64);
		self.0.set_declare_qword_value(index as usize, new_value)
	}

	/// Sets a new `dq` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareQword`]
	///
	/// [`declareDataLength`]: #method.declareDataLength
	/// [`code`]: #method.code
	/// [`Code.DeclareQword`]: enum.Code.html#variant.DeclareQword
	///
	/// # Throws
	///
	/// Throws if `index` is invalid
	///
	/// # Arguments
	///
	/// * `index`: Index (0-1)
	/// * `newValue`: New value
	#[wasm_bindgen(js_name = "setDeclareQwordValue")]
	#[cfg(feature = "bigint")]
	pub fn set_declare_qword_value(&mut self, index: u32, newValue: u64) {
		self.0.set_declare_qword_value(index as usize, newValue)
	}

	/// Gets a `dq` value (high 32 bits), see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareQword`]
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`declareDataLength`]: #method.declareDataLength
	/// [`code`]: #method.code
	/// [`Code.DeclareQword`]: enum.Code.html#variant.DeclareQword
	///
	/// # Throws
	///
	/// Throws if `index` is invalid
	///
	/// # Arguments
	///
	/// * `index`: Index (0-1)
	#[wasm_bindgen(js_name = "getDeclareQwordValueHi")]
	#[cfg(not(feature = "bigint"))]
	pub fn get_declare_qword_value_hi(&self, index: u32) -> u32 {
		(self.0.get_declare_qword_value(index as usize) >> 32) as u32
	}

	/// Gets a `dq` value (low 32 bits), see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareQword`]
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// [`declareDataLength`]: #method.declareDataLength
	/// [`code`]: #method.code
	/// [`Code.DeclareQword`]: enum.Code.html#variant.DeclareQword
	///
	/// # Throws
	///
	/// Throws if `index` is invalid
	///
	/// # Arguments
	///
	/// * `index`: Index (0-1)
	#[wasm_bindgen(js_name = "getDeclareQwordValueLo")]
	#[cfg(not(feature = "bigint"))]
	pub fn get_declare_qword_value_lo(&self, index: u32) -> u32 {
		self.0.get_declare_qword_value(index as usize) as u32
	}

	/// Gets a `dq` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareQword`]
	///
	/// [`declareDataLength`]: #method.declareDataLength
	/// [`code`]: #method.code
	/// [`Code.DeclareQword`]: enum.Code.html#variant.DeclareQword
	///
	/// # Throws
	///
	/// Throws if `index` is invalid
	///
	/// # Arguments
	///
	/// * `index`: Index (0-1)
	#[wasm_bindgen(js_name = "getDeclareQwordValue")]
	#[cfg(feature = "bigint")]
	pub fn get_declare_qword_value(&self, index: u32) -> u64 {
		self.0.get_declare_qword_value(index as usize)
	}

	// GENERATOR-BEGIN: Create
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	/// Creates an instruction with no operands
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	///
	/// [`Code`]: enum.Code.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "create")]
	pub fn with(code: Code) -> Self {
		Self(iced_x86::Instruction::with(code_to_iced(code)))
	}

	/// Creates an instruction with 1 operand
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register`: op0: Register (a [`Register`] enum value)
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createReg")]
	pub fn with_reg(code: Code, register: Register) -> Self {
		Self(iced_x86::Instruction::with_reg(code_to_iced(code), register_to_iced(register)))
	}

	/// Creates an instruction with 1 operand
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `immediate`: op0: Immediate value
	///
	/// [`Code`]: enum.Code.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createI32")]
	pub fn with_i32(code: Code, immediate: i32) -> Self {
		Self(iced_x86::Instruction::with_i32(code_to_iced(code), immediate))
	}

	/// Creates an instruction with 1 operand
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `immediate`: op0: Immediate value
	///
	/// [`Code`]: enum.Code.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createU32")]
	pub fn with_u32(code: Code, immediate: u32) -> Self {
		Self(iced_x86::Instruction::with_u32(code_to_iced(code), immediate))
	}

	/// Creates an instruction with 1 operand
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `memory`: op0: Memory operand
	///
	/// [`Code`]: enum.Code.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createMem")]
	pub fn with_mem(code: Code, memory: MemoryOperand) -> Self {
		Self(iced_x86::Instruction::with_mem(code_to_iced(code), memory.0))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register1`: op0: Register (a [`Register`] enum value)
	/// * `register2`: op1: Register (a [`Register`] enum value)
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegReg")]
	pub fn with_reg_reg(code: Code, register1: Register, register2: Register) -> Self {
		Self(iced_x86::Instruction::with_reg_reg(code_to_iced(code), register_to_iced(register1), register_to_iced(register2)))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register`: op0: Register (a [`Register`] enum value)
	/// * `immediate`: op1: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegI32")]
	pub fn with_reg_i32(code: Code, register: Register, immediate: i32) -> Self {
		Self(iced_x86::Instruction::with_reg_i32(code_to_iced(code), register_to_iced(register), immediate))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register`: op0: Register (a [`Register`] enum value)
	/// * `immediate`: op1: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegU32")]
	pub fn with_reg_u32(code: Code, register: Register, immediate: u32) -> Self {
		Self(iced_x86::Instruction::with_reg_u32(code_to_iced(code), register_to_iced(register), immediate))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register`: op0: Register (a [`Register`] enum value)
	/// * `immediate`: op1: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[cfg(feature = "bigint")]
	#[wasm_bindgen(js_name = "createRegI64")]
	pub fn with_reg_i64(code: Code, register: Register, immediate: i64) -> Self {
		Self(iced_x86::Instruction::with_reg_i64(code_to_iced(code), register_to_iced(register), immediate))
	}

	/// Creates an instruction with 2 operands
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register`: op0: Register (a [`Register`] enum value)
	/// * `immediateHi`: op1: Immediate value (high 32 bits)
	/// * `immediateLo`: op1: Immediate value (low 32 bits)
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[cfg(not(feature = "bigint"))]
	#[wasm_bindgen(js_name = "createRegI64")]
	pub fn with_reg_i64(code: Code, register: Register, immediateHi: u32, immediateLo: u32) -> Self {
		let immediate = (((immediateHi as u64) << 32) | (immediateLo as u64)) as i64;
		Self(iced_x86::Instruction::with_reg_i64(code_to_iced(code), register_to_iced(register), immediate))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register`: op0: Register (a [`Register`] enum value)
	/// * `immediate`: op1: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[cfg(feature = "bigint")]
	#[wasm_bindgen(js_name = "createRegU64")]
	pub fn with_reg_u64(code: Code, register: Register, immediate: u64) -> Self {
		Self(iced_x86::Instruction::with_reg_u64(code_to_iced(code), register_to_iced(register), immediate))
	}

	/// Creates an instruction with 2 operands
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register`: op0: Register (a [`Register`] enum value)
	/// * `immediateHi`: op1: Immediate value (high 32 bits)
	/// * `immediateLo`: op1: Immediate value (low 32 bits)
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[cfg(not(feature = "bigint"))]
	#[wasm_bindgen(js_name = "createRegU64")]
	pub fn with_reg_u64(code: Code, register: Register, immediateHi: u32, immediateLo: u32) -> Self {
		let immediate = ((immediateHi as u64) << 32) | (immediateLo as u64);
		Self(iced_x86::Instruction::with_reg_u64(code_to_iced(code), register_to_iced(register), immediate))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register`: op0: Register (a [`Register`] enum value)
	/// * `memory`: op1: Memory operand
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegMem")]
	pub fn with_reg_mem(code: Code, register: Register, memory: MemoryOperand) -> Self {
		Self(iced_x86::Instruction::with_reg_mem(code_to_iced(code), register_to_iced(register), memory.0))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `immediate`: op0: Immediate value
	/// * `register`: op1: Register (a [`Register`] enum value)
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createI32Reg")]
	pub fn with_i32_reg(code: Code, immediate: i32, register: Register) -> Self {
		Self(iced_x86::Instruction::with_i32_reg(code_to_iced(code), immediate, register_to_iced(register)))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `immediate`: op0: Immediate value
	/// * `register`: op1: Register (a [`Register`] enum value)
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createU32Reg")]
	pub fn with_u32_reg(code: Code, immediate: u32, register: Register) -> Self {
		Self(iced_x86::Instruction::with_u32_reg(code_to_iced(code), immediate, register_to_iced(register)))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `immediate1`: op0: Immediate value
	/// * `immediate2`: op1: Immediate value
	///
	/// [`Code`]: enum.Code.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createI32I32")]
	pub fn with_i32_i32(code: Code, immediate1: i32, immediate2: i32) -> Self {
		Self(iced_x86::Instruction::with_i32_i32(code_to_iced(code), immediate1, immediate2))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `immediate1`: op0: Immediate value
	/// * `immediate2`: op1: Immediate value
	///
	/// [`Code`]: enum.Code.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createU32U32")]
	pub fn with_u32_u32(code: Code, immediate1: u32, immediate2: u32) -> Self {
		Self(iced_x86::Instruction::with_u32_u32(code_to_iced(code), immediate1, immediate2))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `memory`: op0: Memory operand
	/// * `register`: op1: Register (a [`Register`] enum value)
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createMemReg")]
	pub fn with_mem_reg(code: Code, memory: MemoryOperand, register: Register) -> Self {
		Self(iced_x86::Instruction::with_mem_reg(code_to_iced(code), memory.0, register_to_iced(register)))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `memory`: op0: Memory operand
	/// * `immediate`: op1: Immediate value
	///
	/// [`Code`]: enum.Code.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createMemI32")]
	pub fn with_mem_i32(code: Code, memory: MemoryOperand, immediate: i32) -> Self {
		Self(iced_x86::Instruction::with_mem_i32(code_to_iced(code), memory.0, immediate))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `memory`: op0: Memory operand
	/// * `immediate`: op1: Immediate value
	///
	/// [`Code`]: enum.Code.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createMemU32")]
	pub fn with_mem_u32(code: Code, memory: MemoryOperand, immediate: u32) -> Self {
		Self(iced_x86::Instruction::with_mem_u32(code_to_iced(code), memory.0, immediate))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register1`: op0: Register (a [`Register`] enum value)
	/// * `register2`: op1: Register (a [`Register`] enum value)
	/// * `register3`: op2: Register (a [`Register`] enum value)
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegRegReg")]
	pub fn with_reg_reg_reg(code: Code, register1: Register, register2: Register, register3: Register) -> Self {
		Self(iced_x86::Instruction::with_reg_reg_reg(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), register_to_iced(register3)))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register1`: op0: Register (a [`Register`] enum value)
	/// * `register2`: op1: Register (a [`Register`] enum value)
	/// * `immediate`: op2: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegRegI32")]
	pub fn with_reg_reg_i32(code: Code, register1: Register, register2: Register, immediate: i32) -> Self {
		Self(iced_x86::Instruction::with_reg_reg_i32(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), immediate))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register1`: op0: Register (a [`Register`] enum value)
	/// * `register2`: op1: Register (a [`Register`] enum value)
	/// * `immediate`: op2: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegRegU32")]
	pub fn with_reg_reg_u32(code: Code, register1: Register, register2: Register, immediate: u32) -> Self {
		Self(iced_x86::Instruction::with_reg_reg_u32(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), immediate))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register1`: op0: Register (a [`Register`] enum value)
	/// * `register2`: op1: Register (a [`Register`] enum value)
	/// * `memory`: op2: Memory operand
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegRegMem")]
	pub fn with_reg_reg_mem(code: Code, register1: Register, register2: Register, memory: MemoryOperand) -> Self {
		Self(iced_x86::Instruction::with_reg_reg_mem(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), memory.0))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register`: op0: Register (a [`Register`] enum value)
	/// * `immediate1`: op1: Immediate value
	/// * `immediate2`: op2: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegI32I32")]
	pub fn with_reg_i32_i32(code: Code, register: Register, immediate1: i32, immediate2: i32) -> Self {
		Self(iced_x86::Instruction::with_reg_i32_i32(code_to_iced(code), register_to_iced(register), immediate1, immediate2))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register`: op0: Register (a [`Register`] enum value)
	/// * `immediate1`: op1: Immediate value
	/// * `immediate2`: op2: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegU32U32")]
	pub fn with_reg_u32_u32(code: Code, register: Register, immediate1: u32, immediate2: u32) -> Self {
		Self(iced_x86::Instruction::with_reg_u32_u32(code_to_iced(code), register_to_iced(register), immediate1, immediate2))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register1`: op0: Register (a [`Register`] enum value)
	/// * `memory`: op1: Memory operand
	/// * `register2`: op2: Register (a [`Register`] enum value)
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegMemReg")]
	pub fn with_reg_mem_reg(code: Code, register1: Register, memory: MemoryOperand, register2: Register) -> Self {
		Self(iced_x86::Instruction::with_reg_mem_reg(code_to_iced(code), register_to_iced(register1), memory.0, register_to_iced(register2)))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register`: op0: Register (a [`Register`] enum value)
	/// * `memory`: op1: Memory operand
	/// * `immediate`: op2: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegMemI32")]
	pub fn with_reg_mem_i32(code: Code, register: Register, memory: MemoryOperand, immediate: i32) -> Self {
		Self(iced_x86::Instruction::with_reg_mem_i32(code_to_iced(code), register_to_iced(register), memory.0, immediate))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register`: op0: Register (a [`Register`] enum value)
	/// * `memory`: op1: Memory operand
	/// * `immediate`: op2: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegMemU32")]
	pub fn with_reg_mem_u32(code: Code, register: Register, memory: MemoryOperand, immediate: u32) -> Self {
		Self(iced_x86::Instruction::with_reg_mem_u32(code_to_iced(code), register_to_iced(register), memory.0, immediate))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `memory`: op0: Memory operand
	/// * `register1`: op1: Register (a [`Register`] enum value)
	/// * `register2`: op2: Register (a [`Register`] enum value)
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createMemRegReg")]
	pub fn with_mem_reg_reg(code: Code, memory: MemoryOperand, register1: Register, register2: Register) -> Self {
		Self(iced_x86::Instruction::with_mem_reg_reg(code_to_iced(code), memory.0, register_to_iced(register1), register_to_iced(register2)))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `memory`: op0: Memory operand
	/// * `register`: op1: Register (a [`Register`] enum value)
	/// * `immediate`: op2: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createMemRegI32")]
	pub fn with_mem_reg_i32(code: Code, memory: MemoryOperand, register: Register, immediate: i32) -> Self {
		Self(iced_x86::Instruction::with_mem_reg_i32(code_to_iced(code), memory.0, register_to_iced(register), immediate))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `memory`: op0: Memory operand
	/// * `register`: op1: Register (a [`Register`] enum value)
	/// * `immediate`: op2: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createMemRegU32")]
	pub fn with_mem_reg_u32(code: Code, memory: MemoryOperand, register: Register, immediate: u32) -> Self {
		Self(iced_x86::Instruction::with_mem_reg_u32(code_to_iced(code), memory.0, register_to_iced(register), immediate))
	}

	/// Creates an instruction with 4 operands
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register1`: op0: Register (a [`Register`] enum value)
	/// * `register2`: op1: Register (a [`Register`] enum value)
	/// * `register3`: op2: Register (a [`Register`] enum value)
	/// * `register4`: op3: Register (a [`Register`] enum value)
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegRegRegReg")]
	pub fn with_reg_reg_reg_reg(code: Code, register1: Register, register2: Register, register3: Register, register4: Register) -> Self {
		Self(iced_x86::Instruction::with_reg_reg_reg_reg(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), register_to_iced(register3), register_to_iced(register4)))
	}

	/// Creates an instruction with 4 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register1`: op0: Register (a [`Register`] enum value)
	/// * `register2`: op1: Register (a [`Register`] enum value)
	/// * `register3`: op2: Register (a [`Register`] enum value)
	/// * `immediate`: op3: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegRegRegI32")]
	pub fn with_reg_reg_reg_i32(code: Code, register1: Register, register2: Register, register3: Register, immediate: i32) -> Self {
		Self(iced_x86::Instruction::with_reg_reg_reg_i32(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), register_to_iced(register3), immediate))
	}

	/// Creates an instruction with 4 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register1`: op0: Register (a [`Register`] enum value)
	/// * `register2`: op1: Register (a [`Register`] enum value)
	/// * `register3`: op2: Register (a [`Register`] enum value)
	/// * `immediate`: op3: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegRegRegU32")]
	pub fn with_reg_reg_reg_u32(code: Code, register1: Register, register2: Register, register3: Register, immediate: u32) -> Self {
		Self(iced_x86::Instruction::with_reg_reg_reg_u32(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), register_to_iced(register3), immediate))
	}

	/// Creates an instruction with 4 operands
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register1`: op0: Register (a [`Register`] enum value)
	/// * `register2`: op1: Register (a [`Register`] enum value)
	/// * `register3`: op2: Register (a [`Register`] enum value)
	/// * `memory`: op3: Memory operand
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegRegRegMem")]
	pub fn with_reg_reg_reg_mem(code: Code, register1: Register, register2: Register, register3: Register, memory: MemoryOperand) -> Self {
		Self(iced_x86::Instruction::with_reg_reg_reg_mem(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), register_to_iced(register3), memory.0))
	}

	/// Creates an instruction with 4 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register1`: op0: Register (a [`Register`] enum value)
	/// * `register2`: op1: Register (a [`Register`] enum value)
	/// * `immediate1`: op2: Immediate value
	/// * `immediate2`: op3: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegRegI32I32")]
	pub fn with_reg_reg_i32_i32(code: Code, register1: Register, register2: Register, immediate1: i32, immediate2: i32) -> Self {
		Self(iced_x86::Instruction::with_reg_reg_i32_i32(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), immediate1, immediate2))
	}

	/// Creates an instruction with 4 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register1`: op0: Register (a [`Register`] enum value)
	/// * `register2`: op1: Register (a [`Register`] enum value)
	/// * `immediate1`: op2: Immediate value
	/// * `immediate2`: op3: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegRegU32U32")]
	pub fn with_reg_reg_u32_u32(code: Code, register1: Register, register2: Register, immediate1: u32, immediate2: u32) -> Self {
		Self(iced_x86::Instruction::with_reg_reg_u32_u32(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), immediate1, immediate2))
	}

	/// Creates an instruction with 4 operands
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register1`: op0: Register (a [`Register`] enum value)
	/// * `register2`: op1: Register (a [`Register`] enum value)
	/// * `memory`: op2: Memory operand
	/// * `register3`: op3: Register (a [`Register`] enum value)
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegRegMemReg")]
	pub fn with_reg_reg_mem_reg(code: Code, register1: Register, register2: Register, memory: MemoryOperand, register3: Register) -> Self {
		Self(iced_x86::Instruction::with_reg_reg_mem_reg(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), memory.0, register_to_iced(register3)))
	}

	/// Creates an instruction with 4 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register1`: op0: Register (a [`Register`] enum value)
	/// * `register2`: op1: Register (a [`Register`] enum value)
	/// * `memory`: op2: Memory operand
	/// * `immediate`: op3: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegRegMemI32")]
	pub fn with_reg_reg_mem_i32(code: Code, register1: Register, register2: Register, memory: MemoryOperand, immediate: i32) -> Self {
		Self(iced_x86::Instruction::with_reg_reg_mem_i32(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), memory.0, immediate))
	}

	/// Creates an instruction with 4 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register1`: op0: Register (a [`Register`] enum value)
	/// * `register2`: op1: Register (a [`Register`] enum value)
	/// * `memory`: op2: Memory operand
	/// * `immediate`: op3: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegRegMemU32")]
	pub fn with_reg_reg_mem_u32(code: Code, register1: Register, register2: Register, memory: MemoryOperand, immediate: u32) -> Self {
		Self(iced_x86::Instruction::with_reg_reg_mem_u32(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), memory.0, immediate))
	}

	/// Creates an instruction with 5 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register1`: op0: Register (a [`Register`] enum value)
	/// * `register2`: op1: Register (a [`Register`] enum value)
	/// * `register3`: op2: Register (a [`Register`] enum value)
	/// * `register4`: op3: Register (a [`Register`] enum value)
	/// * `immediate`: op4: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegRegRegRegI32")]
	pub fn with_reg_reg_reg_reg_i32(code: Code, register1: Register, register2: Register, register3: Register, register4: Register, immediate: i32) -> Self {
		Self(iced_x86::Instruction::with_reg_reg_reg_reg_i32(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), register_to_iced(register3), register_to_iced(register4), immediate))
	}

	/// Creates an instruction with 5 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register1`: op0: Register (a [`Register`] enum value)
	/// * `register2`: op1: Register (a [`Register`] enum value)
	/// * `register3`: op2: Register (a [`Register`] enum value)
	/// * `register4`: op3: Register (a [`Register`] enum value)
	/// * `immediate`: op4: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegRegRegRegU32")]
	pub fn with_reg_reg_reg_reg_u32(code: Code, register1: Register, register2: Register, register3: Register, register4: Register, immediate: u32) -> Self {
		Self(iced_x86::Instruction::with_reg_reg_reg_reg_u32(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), register_to_iced(register3), register_to_iced(register4), immediate))
	}

	/// Creates an instruction with 5 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register1`: op0: Register (a [`Register`] enum value)
	/// * `register2`: op1: Register (a [`Register`] enum value)
	/// * `register3`: op2: Register (a [`Register`] enum value)
	/// * `memory`: op3: Memory operand
	/// * `immediate`: op4: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegRegRegMemI32")]
	pub fn with_reg_reg_reg_mem_i32(code: Code, register1: Register, register2: Register, register3: Register, memory: MemoryOperand, immediate: i32) -> Self {
		Self(iced_x86::Instruction::with_reg_reg_reg_mem_i32(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), register_to_iced(register3), memory.0, immediate))
	}

	/// Creates an instruction with 5 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register1`: op0: Register (a [`Register`] enum value)
	/// * `register2`: op1: Register (a [`Register`] enum value)
	/// * `register3`: op2: Register (a [`Register`] enum value)
	/// * `memory`: op3: Memory operand
	/// * `immediate`: op4: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegRegRegMemU32")]
	pub fn with_reg_reg_reg_mem_u32(code: Code, register1: Register, register2: Register, register3: Register, memory: MemoryOperand, immediate: u32) -> Self {
		Self(iced_x86::Instruction::with_reg_reg_reg_mem_u32(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), register_to_iced(register3), memory.0, immediate))
	}

	/// Creates an instruction with 5 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register1`: op0: Register (a [`Register`] enum value)
	/// * `register2`: op1: Register (a [`Register`] enum value)
	/// * `memory`: op2: Memory operand
	/// * `register3`: op3: Register (a [`Register`] enum value)
	/// * `immediate`: op4: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegRegMemRegI32")]
	pub fn with_reg_reg_mem_reg_i32(code: Code, register1: Register, register2: Register, memory: MemoryOperand, register3: Register, immediate: i32) -> Self {
		Self(iced_x86::Instruction::with_reg_reg_mem_reg_i32(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), memory.0, register_to_iced(register3), immediate))
	}

	/// Creates an instruction with 5 operands
	///
	/// # Throws
	///
	/// Throws if the immediate is invalid
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register1`: op0: Register (a [`Register`] enum value)
	/// * `register2`: op1: Register (a [`Register`] enum value)
	/// * `memory`: op2: Memory operand
	/// * `register3`: op3: Register (a [`Register`] enum value)
	/// * `immediate`: op4: Immediate value
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRegRegMemRegU32")]
	pub fn with_reg_reg_mem_reg_u32(code: Code, register1: Register, register2: Register, memory: MemoryOperand, register3: Register, immediate: u32) -> Self {
		Self(iced_x86::Instruction::with_reg_reg_mem_reg_u32(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), memory.0, register_to_iced(register3), immediate))
	}

	/// Creates a new near/short branch instruction
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `target`: Target address
	///
	/// [`Code`]: enum.Code.html
	#[rustfmt::skip]
	#[cfg(feature = "bigint")]
	#[wasm_bindgen(js_name = "createBranch")]
	pub fn with_branch(code: Code, target: u64) -> Self {
		Self(iced_x86::Instruction::with_branch(code_to_iced(code), target))
	}

	/// Creates a new near/short branch instruction
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `targetHi`: Target address (high 32 bits)
	/// * `targetLo`: Target address (low 32 bits)
	///
	/// [`Code`]: enum.Code.html
	#[rustfmt::skip]
	#[cfg(not(feature = "bigint"))]
	#[wasm_bindgen(js_name = "createBranch")]
	pub fn with_branch(code: Code, targetHi: u32, targetLo: u32) -> Self {
		let target = ((targetHi as u64) << 32) | (targetLo as u64);
		Self(iced_x86::Instruction::with_branch(code_to_iced(code), target))
	}

	/// Creates a new far branch instruction
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `selector`: Selector/segment value
	/// * `offset`: Offset
	///
	/// [`Code`]: enum.Code.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createFarBranch")]
	pub fn with_far_branch(code: Code, selector: u16, offset: u32) -> Self {
		Self(iced_x86::Instruction::with_far_branch(code_to_iced(code), selector, offset))
	}

	/// Creates a new `XBEGIN` instruction
	///
	/// # Throws
	///
	/// Throws if `bitness` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `bitness`: 16, 32, or 64
	/// * `target`: Target address
	#[rustfmt::skip]
	#[cfg(feature = "bigint")]
	#[wasm_bindgen(js_name = "createXbegin")]
	pub fn with_xbegin(bitness: u32, target: u64) -> Self {
		Self(iced_x86::Instruction::with_xbegin(bitness, target))
	}

	/// Creates a new `XBEGIN` instruction
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// # Throws
	///
	/// Throws if `bitness` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `bitness`: 16, 32, or 64
	/// * `targetHi`: Target address (high 32 bits)
	/// * `targetLo`: Target address (low 32 bits)
	#[rustfmt::skip]
	#[cfg(not(feature = "bigint"))]
	#[wasm_bindgen(js_name = "createXbegin")]
	pub fn with_xbegin(bitness: u32, targetHi: u32, targetLo: u32) -> Self {
		let target = ((targetHi as u64) << 32) | (targetLo as u64);
		Self(iced_x86::Instruction::with_xbegin(bitness, target))
	}

	/// Creates an instruction with a 64-bit memory offset as the second operand, eg. `mov al,[123456789ABCDEF0]`
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register`: Register (`AL`, `AX`, `EAX`, `RAX`) (a [`Register`] enum value)
	/// * `address`: 64-bit address
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	#[rustfmt::skip]
	#[cfg(feature = "bigint")]
	#[wasm_bindgen(js_name = "createRegMem64")]
	pub fn with_reg_mem64(code: Code, register: Register, address: u64, segmentPrefix: Register) -> Self {
		Self(iced_x86::Instruction::with_reg_mem64(code_to_iced(code), register_to_iced(register), address, register_to_iced(segmentPrefix)))
	}

	/// Creates an instruction with a 64-bit memory offset as the second operand, eg. `mov al,[123456789ABCDEF0]`
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `register`: Register (`AL`, `AX`, `EAX`, `RAX`) (a [`Register`] enum value)
	/// * `addressHi`: 64-bit address (high 32 bits)
	/// * `addressLo`: 64-bit address (low 32 bits)
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	#[rustfmt::skip]
	#[cfg(not(feature = "bigint"))]
	#[wasm_bindgen(js_name = "createRegMem64")]
	pub fn with_reg_mem64(code: Code, register: Register, addressHi: u32, addressLo: u32, segmentPrefix: Register) -> Self {
		let address = ((addressHi as u64) << 32) | (addressLo as u64);
		Self(iced_x86::Instruction::with_reg_mem64(code_to_iced(code), register_to_iced(register), address, register_to_iced(segmentPrefix)))
	}

	/// Creates an instruction with a 64-bit memory offset as the first operand, eg. `mov [123456789ABCDEF0],al`
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `address`: 64-bit address
	/// * `register`: Register (`AL`, `AX`, `EAX`, `RAX`) (a [`Register`] enum value)
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	#[rustfmt::skip]
	#[cfg(feature = "bigint")]
	#[wasm_bindgen(js_name = "createMem64Reg")]
	pub fn with_mem64_reg(code: Code, address: u64, register: Register, segmentPrefix: Register) -> Self {
		Self(iced_x86::Instruction::with_mem64_reg(code_to_iced(code), address, register_to_iced(register), register_to_iced(segmentPrefix)))
	}

	/// Creates an instruction with a 64-bit memory offset as the first operand, eg. `mov [123456789ABCDEF0],al`
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `addressHi`: 64-bit address (high 32 bits)
	/// * `addressLo`: 64-bit address (low 32 bits)
	/// * `register`: Register (`AL`, `AX`, `EAX`, `RAX`) (a [`Register`] enum value)
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	///
	/// [`Code`]: enum.Code.html
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	#[rustfmt::skip]
	#[cfg(not(feature = "bigint"))]
	#[wasm_bindgen(js_name = "createMem64Reg")]
	pub fn with_mem64_reg(code: Code, addressHi: u32, addressLo: u32, register: Register, segmentPrefix: Register) -> Self {
		let address = ((addressHi as u64) << 32) | (addressLo as u64);
		Self(iced_x86::Instruction::with_mem64_reg(code_to_iced(code), address, register_to_iced(register), register_to_iced(segmentPrefix)))
	}

	/// Creates a `OUTSB` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`Register`]: enum.Register.html
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createOutsb")]
	pub fn with_outsb(addressSize: u32, segmentPrefix: Register, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_outsb(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REP OUTSB` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepOutsb")]
	pub fn with_rep_outsb(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_rep_outsb(addressSize))
	}

	/// Creates a `OUTSW` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`Register`]: enum.Register.html
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createOutsw")]
	pub fn with_outsw(addressSize: u32, segmentPrefix: Register, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_outsw(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REP OUTSW` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepOutsw")]
	pub fn with_rep_outsw(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_rep_outsw(addressSize))
	}

	/// Creates a `OUTSD` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`Register`]: enum.Register.html
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createOutsd")]
	pub fn with_outsd(addressSize: u32, segmentPrefix: Register, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_outsd(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REP OUTSD` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepOutsd")]
	pub fn with_rep_outsd(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_rep_outsd(addressSize))
	}

	/// Creates a `LODSB` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`Register`]: enum.Register.html
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createLodsb")]
	pub fn with_lodsb(addressSize: u32, segmentPrefix: Register, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_lodsb(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REP LODSB` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepLodsb")]
	pub fn with_rep_lodsb(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_rep_lodsb(addressSize))
	}

	/// Creates a `LODSW` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`Register`]: enum.Register.html
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createLodsw")]
	pub fn with_lodsw(addressSize: u32, segmentPrefix: Register, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_lodsw(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REP LODSW` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepLodsw")]
	pub fn with_rep_lodsw(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_rep_lodsw(addressSize))
	}

	/// Creates a `LODSD` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`Register`]: enum.Register.html
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createLodsd")]
	pub fn with_lodsd(addressSize: u32, segmentPrefix: Register, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_lodsd(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REP LODSD` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepLodsd")]
	pub fn with_rep_lodsd(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_rep_lodsd(addressSize))
	}

	/// Creates a `LODSQ` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`Register`]: enum.Register.html
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createLodsq")]
	pub fn with_lodsq(addressSize: u32, segmentPrefix: Register, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_lodsq(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REP LODSQ` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepLodsq")]
	pub fn with_rep_lodsq(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_rep_lodsq(addressSize))
	}

	/// Creates a `SCASB` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createScasb")]
	pub fn with_scasb(addressSize: u32, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_scasb(addressSize, rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REPE SCASB` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepeScasb")]
	pub fn with_repe_scasb(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_repe_scasb(addressSize))
	}

	/// Creates a `REPNE SCASB` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepneScasb")]
	pub fn with_repne_scasb(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_repne_scasb(addressSize))
	}

	/// Creates a `SCASW` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createScasw")]
	pub fn with_scasw(addressSize: u32, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_scasw(addressSize, rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REPE SCASW` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepeScasw")]
	pub fn with_repe_scasw(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_repe_scasw(addressSize))
	}

	/// Creates a `REPNE SCASW` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepneScasw")]
	pub fn with_repne_scasw(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_repne_scasw(addressSize))
	}

	/// Creates a `SCASD` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createScasd")]
	pub fn with_scasd(addressSize: u32, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_scasd(addressSize, rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REPE SCASD` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepeScasd")]
	pub fn with_repe_scasd(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_repe_scasd(addressSize))
	}

	/// Creates a `REPNE SCASD` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepneScasd")]
	pub fn with_repne_scasd(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_repne_scasd(addressSize))
	}

	/// Creates a `SCASQ` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createScasq")]
	pub fn with_scasq(addressSize: u32, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_scasq(addressSize, rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REPE SCASQ` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepeScasq")]
	pub fn with_repe_scasq(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_repe_scasq(addressSize))
	}

	/// Creates a `REPNE SCASQ` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepneScasq")]
	pub fn with_repne_scasq(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_repne_scasq(addressSize))
	}

	/// Creates a `INSB` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createInsb")]
	pub fn with_insb(addressSize: u32, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_insb(addressSize, rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REP INSB` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepInsb")]
	pub fn with_rep_insb(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_rep_insb(addressSize))
	}

	/// Creates a `INSW` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createInsw")]
	pub fn with_insw(addressSize: u32, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_insw(addressSize, rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REP INSW` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepInsw")]
	pub fn with_rep_insw(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_rep_insw(addressSize))
	}

	/// Creates a `INSD` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createInsd")]
	pub fn with_insd(addressSize: u32, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_insd(addressSize, rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REP INSD` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepInsd")]
	pub fn with_rep_insd(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_rep_insd(addressSize))
	}

	/// Creates a `STOSB` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createStosb")]
	pub fn with_stosb(addressSize: u32, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_stosb(addressSize, rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REP STOSB` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepStosb")]
	pub fn with_rep_stosb(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_rep_stosb(addressSize))
	}

	/// Creates a `STOSW` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createStosw")]
	pub fn with_stosw(addressSize: u32, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_stosw(addressSize, rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REP STOSW` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepStosw")]
	pub fn with_rep_stosw(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_rep_stosw(addressSize))
	}

	/// Creates a `STOSD` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createStosd")]
	pub fn with_stosd(addressSize: u32, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_stosd(addressSize, rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REP STOSD` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepStosd")]
	pub fn with_rep_stosd(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_rep_stosd(addressSize))
	}

	/// Creates a `STOSQ` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createStosq")]
	pub fn with_stosq(addressSize: u32, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_stosq(addressSize, rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REP STOSQ` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepStosq")]
	pub fn with_rep_stosq(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_rep_stosq(addressSize))
	}

	/// Creates a `CMPSB` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`Register`]: enum.Register.html
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createCmpsb")]
	pub fn with_cmpsb(addressSize: u32, segmentPrefix: Register, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_cmpsb(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REPE CMPSB` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepeCmpsb")]
	pub fn with_repe_cmpsb(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_repe_cmpsb(addressSize))
	}

	/// Creates a `REPNE CMPSB` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepneCmpsb")]
	pub fn with_repne_cmpsb(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_repne_cmpsb(addressSize))
	}

	/// Creates a `CMPSW` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`Register`]: enum.Register.html
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createCmpsw")]
	pub fn with_cmpsw(addressSize: u32, segmentPrefix: Register, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_cmpsw(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REPE CMPSW` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepeCmpsw")]
	pub fn with_repe_cmpsw(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_repe_cmpsw(addressSize))
	}

	/// Creates a `REPNE CMPSW` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepneCmpsw")]
	pub fn with_repne_cmpsw(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_repne_cmpsw(addressSize))
	}

	/// Creates a `CMPSD` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`Register`]: enum.Register.html
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createCmpsd")]
	pub fn with_cmpsd(addressSize: u32, segmentPrefix: Register, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_cmpsd(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REPE CMPSD` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepeCmpsd")]
	pub fn with_repe_cmpsd(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_repe_cmpsd(addressSize))
	}

	/// Creates a `REPNE CMPSD` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepneCmpsd")]
	pub fn with_repne_cmpsd(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_repne_cmpsd(addressSize))
	}

	/// Creates a `CMPSQ` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`Register`]: enum.Register.html
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createCmpsq")]
	pub fn with_cmpsq(addressSize: u32, segmentPrefix: Register, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_cmpsq(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REPE CMPSQ` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepeCmpsq")]
	pub fn with_repe_cmpsq(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_repe_cmpsq(addressSize))
	}

	/// Creates a `REPNE CMPSQ` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepneCmpsq")]
	pub fn with_repne_cmpsq(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_repne_cmpsq(addressSize))
	}

	/// Creates a `MOVSB` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`Register`]: enum.Register.html
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createMovsb")]
	pub fn with_movsb(addressSize: u32, segmentPrefix: Register, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_movsb(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REP MOVSB` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepMovsb")]
	pub fn with_rep_movsb(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_rep_movsb(addressSize))
	}

	/// Creates a `MOVSW` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`Register`]: enum.Register.html
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createMovsw")]
	pub fn with_movsw(addressSize: u32, segmentPrefix: Register, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_movsw(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REP MOVSW` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepMovsw")]
	pub fn with_rep_movsw(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_rep_movsw(addressSize))
	}

	/// Creates a `MOVSD` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`Register`]: enum.Register.html
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createMovsd")]
	pub fn with_movsd(addressSize: u32, segmentPrefix: Register, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_movsd(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REP MOVSD` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepMovsd")]
	pub fn with_rep_movsd(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_rep_movsd(addressSize))
	}

	/// Creates a `MOVSQ` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	/// * `repPrefix`: Rep prefix or [`RepPrefixKind.None`] (a [`RepPrefixKind`] enum value)
	///
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`Register`]: enum.Register.html
	/// [`RepPrefixKind.None`]: enum.RepPrefixKind.html#variant.None
	/// [`RepPrefixKind`]: enum.RepPrefixKind.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createMovsq")]
	pub fn with_movsq(addressSize: u32, segmentPrefix: Register, repPrefix: RepPrefixKind) -> Self {
		Self(iced_x86::Instruction::with_movsq(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)))
	}

	/// Creates a `REP MOVSQ` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createRepMovsq")]
	pub fn with_rep_movsq(addressSize: u32) -> Self {
		Self(iced_x86::Instruction::with_rep_movsq(addressSize))
	}

	/// Creates a `MASKMOVQ` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `register1`: Register (a [`Register`] enum value)
	/// * `register2`: Register (a [`Register`] enum value)
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createMaskmovq")]
	pub fn with_maskmovq(addressSize: u32, register1: Register, register2: Register, segmentPrefix: Register) -> Self {
		Self(iced_x86::Instruction::with_maskmovq(addressSize, register_to_iced(register1), register_to_iced(register2), register_to_iced(segmentPrefix)))
	}

	/// Creates a `MASKMOVDQU` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `register1`: Register (a [`Register`] enum value)
	/// * `register2`: Register (a [`Register`] enum value)
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createMaskmovdqu")]
	pub fn with_maskmovdqu(addressSize: u32, register1: Register, register2: Register, segmentPrefix: Register) -> Self {
		Self(iced_x86::Instruction::with_maskmovdqu(addressSize, register_to_iced(register1), register_to_iced(register2), register_to_iced(segmentPrefix)))
	}

	/// Creates a `VMASKMOVDQU` instruction
	///
	/// # Throws
	///
	/// Throws if `addressSize` is not one of 16, 32, 64.
	///
	/// # Arguments
	///
	/// * `addressSize`: 16, 32, or 64
	/// * `register1`: Register (a [`Register`] enum value)
	/// * `register2`: Register (a [`Register`] enum value)
	/// * `segmentPrefix`: Segment override or [`Register.None`] (a [`Register`] enum value)
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createVmaskmovdqu")]
	pub fn with_vmaskmovdqu(addressSize: u32, register1: Register, register2: Register, segmentPrefix: Register) -> Self {
		Self(iced_x86::Instruction::with_vmaskmovdqu(addressSize, register_to_iced(register1), register_to_iced(register2), register_to_iced(segmentPrefix)))
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareByte_1")]
	pub fn with_declare_byte_1(b0: u8) -> Self {
		Self(iced_x86::Instruction::with_declare_byte_1(b0))
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareByte_2")]
	pub fn with_declare_byte_2(b0: u8, b1: u8) -> Self {
		Self(iced_x86::Instruction::with_declare_byte_2(b0, b1))
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareByte_3")]
	pub fn with_declare_byte_3(b0: u8, b1: u8, b2: u8) -> Self {
		Self(iced_x86::Instruction::with_declare_byte_3(b0, b1, b2))
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareByte_4")]
	pub fn with_declare_byte_4(b0: u8, b1: u8, b2: u8, b3: u8) -> Self {
		Self(iced_x86::Instruction::with_declare_byte_4(b0, b1, b2, b3))
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareByte_5")]
	pub fn with_declare_byte_5(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8) -> Self {
		Self(iced_x86::Instruction::with_declare_byte_5(b0, b1, b2, b3, b4))
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareByte_6")]
	pub fn with_declare_byte_6(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8) -> Self {
		Self(iced_x86::Instruction::with_declare_byte_6(b0, b1, b2, b3, b4, b5))
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	/// * `b6`: Byte 6
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareByte_7")]
	pub fn with_declare_byte_7(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8) -> Self {
		Self(iced_x86::Instruction::with_declare_byte_7(b0, b1, b2, b3, b4, b5, b6))
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	/// * `b6`: Byte 6
	/// * `b7`: Byte 7
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareByte_8")]
	pub fn with_declare_byte_8(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8) -> Self {
		Self(iced_x86::Instruction::with_declare_byte_8(b0, b1, b2, b3, b4, b5, b6, b7))
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	/// * `b6`: Byte 6
	/// * `b7`: Byte 7
	/// * `b8`: Byte 8
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareByte_9")]
	pub fn with_declare_byte_9(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8) -> Self {
		Self(iced_x86::Instruction::with_declare_byte_9(b0, b1, b2, b3, b4, b5, b6, b7, b8))
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	/// * `b6`: Byte 6
	/// * `b7`: Byte 7
	/// * `b8`: Byte 8
	/// * `b9`: Byte 9
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareByte_10")]
	pub fn with_declare_byte_10(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8) -> Self {
		Self(iced_x86::Instruction::with_declare_byte_10(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9))
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	/// * `b6`: Byte 6
	/// * `b7`: Byte 7
	/// * `b8`: Byte 8
	/// * `b9`: Byte 9
	/// * `b10`: Byte 10
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareByte_11")]
	pub fn with_declare_byte_11(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8) -> Self {
		Self(iced_x86::Instruction::with_declare_byte_11(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10))
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	/// * `b6`: Byte 6
	/// * `b7`: Byte 7
	/// * `b8`: Byte 8
	/// * `b9`: Byte 9
	/// * `b10`: Byte 10
	/// * `b11`: Byte 11
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareByte_12")]
	pub fn with_declare_byte_12(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8) -> Self {
		Self(iced_x86::Instruction::with_declare_byte_12(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11))
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	/// * `b6`: Byte 6
	/// * `b7`: Byte 7
	/// * `b8`: Byte 8
	/// * `b9`: Byte 9
	/// * `b10`: Byte 10
	/// * `b11`: Byte 11
	/// * `b12`: Byte 12
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareByte_13")]
	pub fn with_declare_byte_13(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8, b12: u8) -> Self {
		Self(iced_x86::Instruction::with_declare_byte_13(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12))
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	/// * `b6`: Byte 6
	/// * `b7`: Byte 7
	/// * `b8`: Byte 8
	/// * `b9`: Byte 9
	/// * `b10`: Byte 10
	/// * `b11`: Byte 11
	/// * `b12`: Byte 12
	/// * `b13`: Byte 13
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareByte_14")]
	pub fn with_declare_byte_14(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8, b12: u8, b13: u8) -> Self {
		Self(iced_x86::Instruction::with_declare_byte_14(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13))
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	/// * `b6`: Byte 6
	/// * `b7`: Byte 7
	/// * `b8`: Byte 8
	/// * `b9`: Byte 9
	/// * `b10`: Byte 10
	/// * `b11`: Byte 11
	/// * `b12`: Byte 12
	/// * `b13`: Byte 13
	/// * `b14`: Byte 14
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareByte_15")]
	pub fn with_declare_byte_15(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8, b12: u8, b13: u8, b14: u8) -> Self {
		Self(iced_x86::Instruction::with_declare_byte_15(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14))
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	/// * `b2`: Byte 2
	/// * `b3`: Byte 3
	/// * `b4`: Byte 4
	/// * `b5`: Byte 5
	/// * `b6`: Byte 6
	/// * `b7`: Byte 7
	/// * `b8`: Byte 8
	/// * `b9`: Byte 9
	/// * `b10`: Byte 10
	/// * `b11`: Byte 11
	/// * `b12`: Byte 12
	/// * `b13`: Byte 13
	/// * `b14`: Byte 14
	/// * `b15`: Byte 15
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareByte_16")]
	pub fn with_declare_byte_16(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8, b12: u8, b13: u8, b14: u8, b15: u8) -> Self {
		Self(iced_x86::Instruction::with_declare_byte_16(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14, b15))
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Throws
	///
	/// Throws if `data.length` is not 1-16
	///
	/// # Arguments
	///
	/// * `data`: Data
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareByte")]
	pub fn with_declare_byte(data: &[u8]) -> Self {
		Self(iced_x86::Instruction::with_declare_byte(data))
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// # Arguments
	///
	/// * `w0`: Word 0
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareWord_1")]
	pub fn with_declare_word_1(w0: u16) -> Self {
		Self(iced_x86::Instruction::with_declare_word_1(w0))
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// # Arguments
	///
	/// * `w0`: Word 0
	/// * `w1`: Word 1
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareWord_2")]
	pub fn with_declare_word_2(w0: u16, w1: u16) -> Self {
		Self(iced_x86::Instruction::with_declare_word_2(w0, w1))
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// # Arguments
	///
	/// * `w0`: Word 0
	/// * `w1`: Word 1
	/// * `w2`: Word 2
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareWord_3")]
	pub fn with_declare_word_3(w0: u16, w1: u16, w2: u16) -> Self {
		Self(iced_x86::Instruction::with_declare_word_3(w0, w1, w2))
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// # Arguments
	///
	/// * `w0`: Word 0
	/// * `w1`: Word 1
	/// * `w2`: Word 2
	/// * `w3`: Word 3
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareWord_4")]
	pub fn with_declare_word_4(w0: u16, w1: u16, w2: u16, w3: u16) -> Self {
		Self(iced_x86::Instruction::with_declare_word_4(w0, w1, w2, w3))
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// # Arguments
	///
	/// * `w0`: Word 0
	/// * `w1`: Word 1
	/// * `w2`: Word 2
	/// * `w3`: Word 3
	/// * `w4`: Word 4
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareWord_5")]
	pub fn with_declare_word_5(w0: u16, w1: u16, w2: u16, w3: u16, w4: u16) -> Self {
		Self(iced_x86::Instruction::with_declare_word_5(w0, w1, w2, w3, w4))
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// # Arguments
	///
	/// * `w0`: Word 0
	/// * `w1`: Word 1
	/// * `w2`: Word 2
	/// * `w3`: Word 3
	/// * `w4`: Word 4
	/// * `w5`: Word 5
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareWord_6")]
	pub fn with_declare_word_6(w0: u16, w1: u16, w2: u16, w3: u16, w4: u16, w5: u16) -> Self {
		Self(iced_x86::Instruction::with_declare_word_6(w0, w1, w2, w3, w4, w5))
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// # Arguments
	///
	/// * `w0`: Word 0
	/// * `w1`: Word 1
	/// * `w2`: Word 2
	/// * `w3`: Word 3
	/// * `w4`: Word 4
	/// * `w5`: Word 5
	/// * `w6`: Word 6
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareWord_7")]
	pub fn with_declare_word_7(w0: u16, w1: u16, w2: u16, w3: u16, w4: u16, w5: u16, w6: u16) -> Self {
		Self(iced_x86::Instruction::with_declare_word_7(w0, w1, w2, w3, w4, w5, w6))
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// # Arguments
	///
	/// * `w0`: Word 0
	/// * `w1`: Word 1
	/// * `w2`: Word 2
	/// * `w3`: Word 3
	/// * `w4`: Word 4
	/// * `w5`: Word 5
	/// * `w6`: Word 6
	/// * `w7`: Word 7
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareWord_8")]
	pub fn with_declare_word_8(w0: u16, w1: u16, w2: u16, w3: u16, w4: u16, w5: u16, w6: u16, w7: u16) -> Self {
		Self(iced_x86::Instruction::with_declare_word_8(w0, w1, w2, w3, w4, w5, w6, w7))
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// # Throws
	///
	/// Throws if `data.length` is not 1-8
	///
	/// # Arguments
	///
	/// * `data`: Data
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareWord")]
	pub fn with_declare_word(data: &[u16]) -> Self {
		Self(iced_x86::Instruction::with_declare_word(data))
	}

	/// Creates a `dd`/`.int` asm directive
	///
	/// # Arguments
	///
	/// * `d0`: Dword 0
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareDword_1")]
	pub fn with_declare_dword_1(d0: u32) -> Self {
		Self(iced_x86::Instruction::with_declare_dword_1(d0))
	}

	/// Creates a `dd`/`.int` asm directive
	///
	/// # Arguments
	///
	/// * `d0`: Dword 0
	/// * `d1`: Dword 1
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareDword_2")]
	pub fn with_declare_dword_2(d0: u32, d1: u32) -> Self {
		Self(iced_x86::Instruction::with_declare_dword_2(d0, d1))
	}

	/// Creates a `dd`/`.int` asm directive
	///
	/// # Arguments
	///
	/// * `d0`: Dword 0
	/// * `d1`: Dword 1
	/// * `d2`: Dword 2
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareDword_3")]
	pub fn with_declare_dword_3(d0: u32, d1: u32, d2: u32) -> Self {
		Self(iced_x86::Instruction::with_declare_dword_3(d0, d1, d2))
	}

	/// Creates a `dd`/`.int` asm directive
	///
	/// # Arguments
	///
	/// * `d0`: Dword 0
	/// * `d1`: Dword 1
	/// * `d2`: Dword 2
	/// * `d3`: Dword 3
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareDword_4")]
	pub fn with_declare_dword_4(d0: u32, d1: u32, d2: u32, d3: u32) -> Self {
		Self(iced_x86::Instruction::with_declare_dword_4(d0, d1, d2, d3))
	}

	/// Creates a `dd`/`.int` asm directive
	///
	/// # Throws
	///
	/// Throws if `data.length` is not 1-4
	///
	/// # Arguments
	///
	/// * `data`: Data
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareDword")]
	pub fn with_declare_dword(data: &[u32]) -> Self {
		Self(iced_x86::Instruction::with_declare_dword(data))
	}

	/// Creates a `dq`/`.quad` asm directive
	///
	/// # Arguments
	///
	/// * `q0`: Qword 0
	#[rustfmt::skip]
	#[cfg(feature = "bigint")]
	#[wasm_bindgen(js_name = "createDeclareQword_1")]
	pub fn with_declare_qword_1(q0: u64) -> Self {
		Self(iced_x86::Instruction::with_declare_qword_1(q0))
	}

	/// Creates a `dq`/`.quad` asm directive
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// # Arguments
	///
	/// * `q0Hi`: Qword 0 (high 32 bits)
	/// * `q0Lo`: Qword 0 (low 32 bits)
	#[rustfmt::skip]
	#[cfg(not(feature = "bigint"))]
	#[wasm_bindgen(js_name = "createDeclareQword_1")]
	pub fn with_declare_qword_1(q0Hi: u32, q0Lo: u32) -> Self {
		let q0 = ((q0Hi as u64) << 32) | (q0Lo as u64);
		Self(iced_x86::Instruction::with_declare_qword_1(q0))
	}

	/// Creates a `dq`/`.quad` asm directive
	///
	/// # Arguments
	///
	/// * `q0`: Qword 0
	/// * `q1`: Qword 1
	#[rustfmt::skip]
	#[cfg(feature = "bigint")]
	#[wasm_bindgen(js_name = "createDeclareQword_2")]
	pub fn with_declare_qword_2(q0: u64, q1: u64) -> Self {
		Self(iced_x86::Instruction::with_declare_qword_2(q0, q1))
	}

	/// Creates a `dq`/`.quad` asm directive
	///
	/// Enable the `bigint` feature to use APIs with 64-bit numbers (requires `BigInt`).
	///
	/// # Arguments
	///
	/// * `q0Hi`: Qword 0 (high 32 bits)
	/// * `q0Lo`: Qword 0 (low 32 bits)
	/// * `q1Hi`: Qword 1 (high 32 bits)
	/// * `q1Lo`: Qword 1 (low 32 bits)
	#[rustfmt::skip]
	#[cfg(not(feature = "bigint"))]
	#[wasm_bindgen(js_name = "createDeclareQword_2")]
	pub fn with_declare_qword_2(q0Hi: u32, q0Lo: u32, q1Hi: u32, q1Lo: u32) -> Self {
		let q0 = ((q0Hi as u64) << 32) | (q0Lo as u64);
		let q1 = ((q1Hi as u64) << 32) | (q1Lo as u64);
		Self(iced_x86::Instruction::with_declare_qword_2(q0, q1))
	}

	/// Creates a `dq`/`.quad` asm directive
	///
	/// # Throws
	///
	/// Throws if `data.length` is not 1-2
	///
	/// # Arguments
	///
	/// * `data`: Data
	#[rustfmt::skip]
	#[cfg(feature = "bigint")]
	#[wasm_bindgen(js_name = "createDeclareQword")]
	pub fn with_declare_qword(data: &[u64]) -> Self {
		Self(iced_x86::Instruction::with_declare_qword(data))
	}
	// GENERATOR-END: Create
}
