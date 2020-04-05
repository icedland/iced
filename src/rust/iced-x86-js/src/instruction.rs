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

#[cfg(feature = "encoder")]
#[cfg(feature = "instruction_api")]
use super::code::code_to_iced;
#[cfg(feature = "instruction_api")]
use super::code::{iced_to_code, Code};
#[cfg(feature = "encoder")]
#[cfg(feature = "instruction_api")]
use super::code_size::code_size_to_iced;
#[cfg(feature = "instruction_api")]
use super::code_size::{iced_to_code_size, CodeSize};
#[cfg(feature = "instr_info")]
#[cfg(feature = "instruction_api")]
use super::condition_code::{iced_to_condition_code, ConditionCode};
#[cfg(feature = "instr_info")]
#[cfg(feature = "instruction_api")]
use super::encoding_kind::{iced_to_encoding_kind, EncodingKind};
#[cfg(feature = "instr_info")]
#[cfg(feature = "instruction_api")]
use super::flow_control::{iced_to_flow_control, FlowControl};
#[cfg(feature = "instruction_api")]
use super::memory_size::{iced_to_memory_size, MemorySize};
#[cfg(feature = "instruction_api")]
use super::mnemonic::{iced_to_mnemonic, Mnemonic};
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
#[cfg(feature = "instruction_api")]
use super::op_code_info::OpCodeInfo;
#[cfg(feature = "encoder")]
#[cfg(feature = "instruction_api")]
use super::op_kind::op_kind_to_iced;
#[cfg(feature = "instruction_api")]
use super::op_kind::{iced_to_op_kind, OpKind};
#[cfg(feature = "encoder")]
#[cfg(feature = "instruction_api")]
use super::register::register_to_iced;
#[cfg(feature = "instruction_api")]
use super::register::{iced_to_register, Register};
#[cfg(feature = "encoder")]
#[cfg(feature = "instruction_api")]
use super::rounding_control::rounding_control_to_iced;
#[cfg(feature = "instruction_api")]
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
impl Instruction {
	/// Gets the low 32 bits of the 64-bit IP of the instruction.
	/// Enable the `bigint` feature to support `BigInt`.
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn ip_lo(&self) -> u32 {
		self.0.ip() as u32
	}

	/// Gets the high 32 bits of the 64-bit IP of the instruction.
	/// Enable the `bigint` feature to support `BigInt`.
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn ip_hi(&self) -> u32 {
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
#[allow(clippy::new_without_default)]
#[cfg(feature = "instruction_api")]
impl Instruction {
	/// Creates an empty `Instruction` (all fields are cleared). See also the `with*()` constructor methods.
	#[wasm_bindgen(constructor)]
	pub fn new() -> Self {
		Self(iced_x86::Instruction::new())
	}

	/// Checks if two instructions are equal, comparing all bits, not ignoring anything. `equals()` ignores some fields.
	#[allow(trivial_casts)]
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
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// # Arguments
	///
	/// * `lo`: Low 32 bits
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_ip_lo(&mut self, lo: u32) {
		let ip = (self.0.ip() & !0xFFFF_FFFF) | (lo as u64);
		self.0.set_ip(ip)
	}

	/// Sets the high 32 bits of the 64-bit IP of the instruction.
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// # Arguments
	///
	/// * `hi`: High 32 bits
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_ip_hi(&mut self, hi: u32) {
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
	/// Enable the `bigint` feature to support `BigInt`.
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn nextIP_lo(&self) -> u32 {
		self.0.next_ip() as u32
	}

	/// Gets the high 32 bits of the 64-bit IP of the next instruction.
	/// Enable the `bigint` feature to support `BigInt`.
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn nextIP_hi(&self) -> u32 {
		(self.0.next_ip() >> 32) as u32
	}

	/// Gets the 64-bit IP of the next instruction
	#[wasm_bindgen(getter)]
	#[cfg(feature = "bigint")]
	pub fn nextIP(&self) -> u64 {
		self.0.next_ip()
	}

	/// Sets the low 32 bits of the 64-bit IP of the next instruction.
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_nextIP_lo(&mut self, lo: u32) {
		let ip = (self.0.next_ip() & !0xFFFF_FFFF) | (lo as u64);
		self.0.set_next_ip(ip);
	}

	/// Sets the high 32 bits of the 64-bit IP of the next instruction.
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_nextIP_hi(&mut self, hi: u32) {
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

	/// Gets the instruction code (a [`CodeSize`] enum value), see also [`mnemonic`].
	///
	/// [`mnemonic`]: #method.mnemonic
	/// [`Code`]: enum.Code.html
	#[wasm_bindgen(getter)]
	pub fn code(&self) -> Code {
		iced_to_code(self.0.code())
	}

	/// Sets the instruction code (a [`CodeSize`] enum value)
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
	/// ```
	/// use iced_x86::*;
	///
	/// // add [rax],ebx
	/// let bytes = b"\x01\x18";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	/// let instr = decoder.decode();
	///
	/// assert_eq!(2, instr.op_count());
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
	#[allow(clippy::unused_self)]
	pub fn op4Kind(&self) -> OpKind {
		iced_to_op_kind(self.0.op4_kind())
	}

	/// Sets operand #4's kind (an [`OpKind`] enum value) if the operand exists (see [`opCount`] and [`opKind`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	/// [`opKind`]: #method.setOpKind
	///
	/// # Panics
	///
	/// Panics if `newValue` is invalid.
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[allow(clippy::unused_self)]
	#[cfg(feature = "encoder")]
	pub fn set_op4Kind(&mut self, newValue: OpKind) {
		self.0.set_op4_kind(op_kind_to_iced(newValue))
	}

	/// Gets an operand's kind (an [`OpKind`] enum value) if it exists (see [`opCount`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	///
	/// # Panics
	///
	/// Panics if `operand` is invalid
	///
	/// # Arguments
	///
	/// * `operand`: Operand number, 0-4
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// // add [rax],ebx
	/// let bytes = b"\x01\x18";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	/// let instr = decoder.decode();
	///
	/// assert_eq!(2, instr.op_count());
	/// assert_eq!(OpKind::Memory, instr.op_kind(0));
	/// assert_eq!(Register::RAX, instr.memory_base());
	/// assert_eq!(Register::None, instr.memory_index());
	/// assert_eq!(OpKind::Register, instr.op_kind(1));
	/// assert_eq!(Register::EBX, instr.op_register(1));
	/// ```
	pub fn opKind(&self, operand: u32) -> OpKind {
		iced_to_op_kind(self.0.op_kind(operand))
	}

	/// Sets an operand's kind (an [`OpKind`] enum value)
	///
	/// [`OpKind`]: enum.OpKind.html
	///
	/// # Panics
	///
	/// Panics if `operand` is invalid
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
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "memoryDisplacement64_lo")]
	#[cfg(not(feature = "bigint"))]
	pub fn memory_displacement64_lo(&self) -> u32 {
		self.0.memory_displacement64() as u32
	}

	/// Gets the high 32 bits of the memory operand's displacement sign extended to 64 bits.
	/// Use this method if the operand has kind [`OpKind.Memory`].
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "memoryDisplacement64_hi")]
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
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// # Panics
	///
	/// Panics if `operand` is invalid or if it's not an immediate operand
	///
	/// # Arguments
	///
	/// * `operand`: Operand number, 0-4
	#[cfg(not(feature = "bigint"))]
	pub fn immediate_lo(&self, operand: u32) -> u32 {
		self.0.immediate(operand) as u32
	}

	/// Gets the high 32 bits of an operand's immediate value.
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// # Panics
	///
	/// Panics if `operand` is invalid or if it's not an immediate operand
	///
	/// # Arguments
	///
	/// * `operand`: Operand number, 0-4
	#[cfg(not(feature = "bigint"))]
	pub fn immediate_hi(&self, operand: u32) -> u32 {
		(self.0.immediate(operand) >> 32) as u32
	}

	/// Gets an operand's immediate value
	///
	/// # Panics
	///
	/// Panics if `operand` is invalid or if it's not an immediate operand
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
	/// # Panics
	///
	/// Panics if `operand` is invalid or if it's not an immediate operand
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
	/// # Panics
	///
	/// Panics if `operand` is invalid or if it's not an immediate operand
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
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// # Panics
	///
	/// Panics if `operand` is invalid or if it's not an immediate operand
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
	/// # Panics
	///
	/// Panics if `operand` is invalid or if it's not an immediate operand
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
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// # Panics
	///
	/// Panics if `operand` is invalid or if it's not an immediate operand
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
	/// # Panics
	///
	/// Panics if `operand` is invalid or if it's not an immediate operand
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
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// [`OpKind.Immediate64`]: enum.OpKind.html#variant.Immediate64
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn immediate64_lo(&self) -> u32 {
		self.0.immediate64() as u32
	}

	/// Gets the high 32 bits of the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate64`].
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// [`OpKind.Immediate64`]: enum.OpKind.html#variant.Immediate64
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn immediate64_hi(&self) -> u32 {
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
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// [`OpKind.Immediate64`]: enum.OpKind.html#variant.Immediate64
	///
	/// # Arguments
	///
	/// * `lo`: Low 32 bits
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_immediate64_lo(&mut self, lo: u32) {
		let new_value = (self.0.immediate64() & !0xFFFF_FFFF) | (lo as u64);
		self.0.set_immediate64(new_value);
	}

	/// Sets the high 32 bits of the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate64`].
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// [`OpKind.Immediate64`]: enum.OpKind.html#variant.Immediate64
	///
	/// # Arguments
	///
	/// * `hi`: High 32 bits
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_immediate64_hi(&mut self, hi: u32) {
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
	/// Enable the `bigint` feature to support `BigInt`.
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
	/// Enable the `bigint` feature to support `BigInt`.
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
	/// Enable the `bigint` feature to support `BigInt`.
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
	/// Enable the `bigint` feature to support `BigInt`.
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
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// [`OpKind.Memory64`]: enum.OpKind.html#variant.Memory64
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn memoryAddress64_lo(&self) -> u32 {
		self.0.memory_address64() as u32
	}

	/// Gets the high 32 bits of the operand's 64-bit address value. Use this method if the operand has kind [`OpKind.Memory64`].
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// [`OpKind.Memory64`]: enum.OpKind.html#variant.Memory64
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn memoryAddress64_hi(&self) -> u32 {
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
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// [`OpKind.Memory64`]: enum.OpKind.html#variant.Memory64
	///
	/// # Arguments
	///
	/// * `lo`: Low 32 bits
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_memoryAddress64_lo(&mut self, lo: u32) {
		let new_value = (self.0.memory_address64() & !0xFFFF_FFFF) | (lo as u64);
		self.0.set_memory_address64(new_value);
	}

	/// Sets the high 32 bits of the operand's 64-bit address value. Use this method if the operand has kind [`OpKind.Memory64`].
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// [`OpKind.Memory64`]: enum.OpKind.html#variant.Memory64
	///
	/// # Arguments
	///
	/// * `hi`: High 32 bits
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_memoryAddress64_hi(&mut self, hi: u32) {
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
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// [`OpKind.NearBranch64`]: enum.OpKind.html#variant.NearBranch64
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn nearBranch64_lo(&self) -> u32 {
		self.0.near_branch64() as u32
	}

	/// Gets the high 32 bits of the operand's branch target. Use this method if the operand has kind [`OpKind.NearBranch64`].
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// [`OpKind.NearBranch64`]: enum.OpKind.html#variant.NearBranch64
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn nearBranch64_hi(&self) -> u32 {
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
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// [`OpKind.NearBranch64`]: enum.OpKind.html#variant.NearBranch64
	///
	/// # Arguments
	///
	/// * `lo`: Low 32 bits
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_nearBranch64_lo(&mut self, lo: u32) {
		let new_value = (self.0.near_branch64() & !0xFFFF_FFFF) | (lo as u64);
		self.0.set_near_branch64(new_value)
	}

	/// Sets the high 32 bits of the operand's branch target. Use this method if the operand has kind [`OpKind.NearBranch64`].
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// [`OpKind.NearBranch64`]: enum.OpKind.html#variant.NearBranch64
	///
	/// # Arguments
	///
	/// * `hi`: High 32 bits
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[cfg(not(feature = "bigint"))]
	pub fn set_nearBranch64_hi(&mut self, hi: u32) {
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
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// [`op0Kind`]: #method.op0Kind
	/// [`OpKind.NearBranch16`]: enum.OpKind.html#variant.NearBranch16
	/// [`OpKind.NearBranch32`]: enum.OpKind.html#variant.NearBranch32
	/// [`OpKind.NearBranch64`]: enum.OpKind.html#variant.NearBranch64
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn nearBranchTarget_lo(&self) -> u32 {
		self.0.near_branch_target() as u32
	}

	/// Gets the high 32 bits of the near branch target if it's a `CALL`/`JMP`/`Jcc` near branch instruction
	/// (i.e., if [`op0Kind`] is [`OpKind.NearBranch16`], [`OpKind.NearBranch32`] or [`OpKind.NearBranch64`]).
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// [`op0Kind`]: #method.op0Kind
	/// [`OpKind.NearBranch16`]: enum.OpKind.html#variant.NearBranch16
	/// [`OpKind.NearBranch32`]: enum.OpKind.html#variant.NearBranch32
	/// [`OpKind.NearBranch64`]: enum.OpKind.html#variant.NearBranch64
	#[wasm_bindgen(getter)]
	#[cfg(not(feature = "bigint"))]
	pub fn nearBranchTarget_hi(&self) -> u32 {
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
	#[allow(clippy::unused_self)]
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
	/// # Panics
	///
	/// Panics if `newValue` is invalid
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[allow(clippy::unused_self)]
	#[cfg(feature = "encoder")]
	pub fn set_op4Register(&mut self, newValue: Register) {
		self.0.set_op4_register(register_to_iced(newValue))
	}

	/// Gets the operand's register value (a [`Register`] enum value). Use this method if the operand has kind [`OpKind.Register`]
	///
	/// [`Register`]: enum.Register.html
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	///
	/// # Panics
	///
	/// Panics if `operand` is invalid
	///
	/// # Arguments
	///
	/// * `operand`: Operand number, 0-4
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// // add [rax],ebx
	/// let bytes = b"\x01\x18";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	/// let instr = decoder.decode();
	///
	/// assert_eq!(2, instr.op_count());
	/// assert_eq!(OpKind::Memory, instr.op_kind(0));
	/// assert_eq!(OpKind::Register, instr.op_kind(1));
	/// assert_eq!(Register::EBX, instr.op_register(1));
	/// ```
	pub fn opRegister(&self, operand: u32) -> Register {
		iced_to_register(self.0.op_register(operand))
	}

	/// Sets the operand's register value (a [`Register`] enum value). Use this method if the operand has kind [`OpKind.Register`]
	///
	/// [`Register`]: enum.Register.html
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	///
	/// # Panics
	///
	/// Panics if `operand` is invalid
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
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// [`nextIP`]: #method.nextIP
	/// [`nextIP32`]: #method.nextIP32
	/// [`memoryDisplacement`]: #method.memoryDisplacement
	/// [`isIpRelMemoryOperand`]: #method.is_ip_rel_memory_operand
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "ipRelMemoryAddress_lo")]
	#[cfg(not(feature = "bigint"))]
	pub fn ip_rel_memory_address_lo(&self) -> u32 {
		self.0.ip_rel_memory_address() as u32
	}

	/// Gets the high 32 bits of the `RIP`/`EIP` releative address (([`nextIP`] or [`nextIP32`]) + [`memoryDisplacement`]).
	/// This method is only valid if there's a memory operand with `RIP`/`EIP` relative addressing, see [`isIpRelMemoryOperand`].
	/// Enable the `bigint` feature to support `BigInt`.
	///
	/// [`nextIP`]: #method.nextIP
	/// [`nextIP32`]: #method.nextIP32
	/// [`memoryDisplacement`]: #method.memoryDisplacement
	/// [`isIpRelMemoryOperand`]: #method.is_ip_rel_memory_operand
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "ipRelMemoryAddress_hi")]
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
#[cfg(feature = "instruction_api")]
impl Instruction {
	/// Gets the number of bytes added to `SP`/`ESP`/`RSP` or 0 if it's not an instruction that pushes or pops data. This method assumes
	/// the instruction doesn't change the privilege level (eg. `IRET/D/Q`). If it's the `LEAVE` instruction, this method returns 0.
	///
	/// # Examples
	///
	/// ```
	/// use iced_x86::*;
	///
	/// // pushfq
	/// let bytes = b"\x9C";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	/// let instr = decoder.decode();
	///
	/// assert!(instr.is_stack_instruction());
	/// assert_eq!(-8, instr.stack_pointer_increment());
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
	/// ```
	/// use iced_x86::*;
	///
	/// // vmovaps xmm1,xmm5
	/// let bytes = b"\xC5\xF8\x28\xCD";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	/// let instr = decoder.decode();
	///
	/// assert_eq!(EncodingKind::VEX, instr.encoding());
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
	/// ```
	/// use iced_x86::*;
	///
	/// // vmovaps xmm1,xmm5
	/// // vmovaps xmm10{k3}{z},xmm19
	/// let bytes = b"\xC5\xF8\x28\xCD\x62\x31\x7C\x8B\x28\xD3";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	///
	/// // vmovaps xmm1,xmm5
	/// let instr = decoder.decode();
	/// let cpuid = instr.cpuid_features();
	/// assert_eq!(1, cpuid.len());
	/// assert_eq!(CpuidFeature::AVX, cpuid[0]);
	///
	/// // vmovaps xmm10{k3}{z},xmm19
	/// let instr = decoder.decode();
	/// let cpuid = instr.cpuid_features();
	/// assert_eq!(2, cpuid.len());
	/// assert_eq!(CpuidFeature::AVX512VL, cpuid[0]);
	/// assert_eq!(CpuidFeature::AVX512F, cpuid[1]);
	/// ```
	#[wasm_bindgen(getter)]
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
	/// ```
	/// use iced_x86::*;
	///
	/// // or ecx,esi
	/// // ud0 rcx,rsi
	/// // call rcx
	/// let bytes = b"\x0B\xCE\x48\x0F\xFF\xCE\xFF\xD1";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	///
	/// // or ecx,esi
	/// let instr = decoder.decode();
	/// assert_eq!(FlowControl::Next, instr.flow_control());
	///
	/// // ud0 rcx,rsi
	/// let instr = decoder.decode();
	/// assert_eq!(FlowControl::Exception, instr.flow_control());
	///
	/// // call rcx
	/// let instr = decoder.decode();
	/// assert_eq!(FlowControl::IndirectCall, instr.flow_control());
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
	/// ```
	/// use iced_x86::*;
	///
	/// // or ecx,esi
	/// // push rax
	/// let bytes = b"\x0B\xCE\x50";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	///
	/// // or ecx,esi
	/// let instr = decoder.decode();
	/// assert!(!instr.is_stack_instruction());
	///
	/// // push rax
	/// let instr = decoder.decode();
	/// assert!(instr.is_stack_instruction());
	/// assert_eq!(-8, instr.stack_pointer_increment());
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
	/// ```
	/// use iced_x86::*;
	///
	/// // adc rsi,rcx
	/// // xor rdi,5Ah
	/// let bytes = b"\x48\x11\xCE\x48\x83\xF7\x5A";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	///
	/// // adc rsi,rcx
	/// let instr = decoder.decode();
	/// assert_eq!(RflagsBits::CF, instr.rflags_read());
	/// assert_eq!(RflagsBits::OF | RflagsBits::SF | RflagsBits::ZF | RflagsBits::AF | RflagsBits::CF | RflagsBits::PF, instr.rflags_written());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_cleared());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_set());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_undefined());
	/// assert_eq!(RflagsBits::OF | RflagsBits::SF | RflagsBits::ZF | RflagsBits::AF | RflagsBits::CF | RflagsBits::PF, instr.rflags_modified());
	///
	/// // xor rdi,5Ah
	/// let instr = decoder.decode();
	/// assert_eq!(RflagsBits::NONE, instr.rflags_read());
	/// assert_eq!(RflagsBits::SF | RflagsBits::ZF | RflagsBits::PF, instr.rflags_written());
	/// assert_eq!(RflagsBits::OF | RflagsBits::CF, instr.rflags_cleared());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_set());
	/// assert_eq!(RflagsBits::AF, instr.rflags_undefined());
	/// assert_eq!(RflagsBits::OF | RflagsBits::SF | RflagsBits::ZF | RflagsBits::AF | RflagsBits::CF | RflagsBits::PF, instr.rflags_modified());
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
	/// ```
	/// use iced_x86::*;
	///
	/// // adc rsi,rcx
	/// // xor rdi,5Ah
	/// let bytes = b"\x48\x11\xCE\x48\x83\xF7\x5A";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	///
	/// // adc rsi,rcx
	/// let instr = decoder.decode();
	/// assert_eq!(RflagsBits::CF, instr.rflags_read());
	/// assert_eq!(RflagsBits::OF | RflagsBits::SF | RflagsBits::ZF | RflagsBits::AF | RflagsBits::CF | RflagsBits::PF, instr.rflags_written());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_cleared());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_set());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_undefined());
	/// assert_eq!(RflagsBits::OF | RflagsBits::SF | RflagsBits::ZF | RflagsBits::AF | RflagsBits::CF | RflagsBits::PF, instr.rflags_modified());
	///
	/// // xor rdi,5Ah
	/// let instr = decoder.decode();
	/// assert_eq!(RflagsBits::NONE, instr.rflags_read());
	/// assert_eq!(RflagsBits::SF | RflagsBits::ZF | RflagsBits::PF, instr.rflags_written());
	/// assert_eq!(RflagsBits::OF | RflagsBits::CF, instr.rflags_cleared());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_set());
	/// assert_eq!(RflagsBits::AF, instr.rflags_undefined());
	/// assert_eq!(RflagsBits::OF | RflagsBits::SF | RflagsBits::ZF | RflagsBits::AF | RflagsBits::CF | RflagsBits::PF, instr.rflags_modified());
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
	/// ```
	/// use iced_x86::*;
	///
	/// // adc rsi,rcx
	/// // xor rdi,5Ah
	/// let bytes = b"\x48\x11\xCE\x48\x83\xF7\x5A";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	///
	/// // adc rsi,rcx
	/// let instr = decoder.decode();
	/// assert_eq!(RflagsBits::CF, instr.rflags_read());
	/// assert_eq!(RflagsBits::OF | RflagsBits::SF | RflagsBits::ZF | RflagsBits::AF | RflagsBits::CF | RflagsBits::PF, instr.rflags_written());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_cleared());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_set());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_undefined());
	/// assert_eq!(RflagsBits::OF | RflagsBits::SF | RflagsBits::ZF | RflagsBits::AF | RflagsBits::CF | RflagsBits::PF, instr.rflags_modified());
	///
	/// // xor rdi,5Ah
	/// let instr = decoder.decode();
	/// assert_eq!(RflagsBits::NONE, instr.rflags_read());
	/// assert_eq!(RflagsBits::SF | RflagsBits::ZF | RflagsBits::PF, instr.rflags_written());
	/// assert_eq!(RflagsBits::OF | RflagsBits::CF, instr.rflags_cleared());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_set());
	/// assert_eq!(RflagsBits::AF, instr.rflags_undefined());
	/// assert_eq!(RflagsBits::OF | RflagsBits::SF | RflagsBits::ZF | RflagsBits::AF | RflagsBits::CF | RflagsBits::PF, instr.rflags_modified());
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
	/// ```
	/// use iced_x86::*;
	///
	/// // adc rsi,rcx
	/// // xor rdi,5Ah
	/// let bytes = b"\x48\x11\xCE\x48\x83\xF7\x5A";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	///
	/// // adc rsi,rcx
	/// let instr = decoder.decode();
	/// assert_eq!(RflagsBits::CF, instr.rflags_read());
	/// assert_eq!(RflagsBits::OF | RflagsBits::SF | RflagsBits::ZF | RflagsBits::AF | RflagsBits::CF | RflagsBits::PF, instr.rflags_written());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_cleared());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_set());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_undefined());
	/// assert_eq!(RflagsBits::OF | RflagsBits::SF | RflagsBits::ZF | RflagsBits::AF | RflagsBits::CF | RflagsBits::PF, instr.rflags_modified());
	///
	/// // xor rdi,5Ah
	/// let instr = decoder.decode();
	/// assert_eq!(RflagsBits::NONE, instr.rflags_read());
	/// assert_eq!(RflagsBits::SF | RflagsBits::ZF | RflagsBits::PF, instr.rflags_written());
	/// assert_eq!(RflagsBits::OF | RflagsBits::CF, instr.rflags_cleared());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_set());
	/// assert_eq!(RflagsBits::AF, instr.rflags_undefined());
	/// assert_eq!(RflagsBits::OF | RflagsBits::SF | RflagsBits::ZF | RflagsBits::AF | RflagsBits::CF | RflagsBits::PF, instr.rflags_modified());
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
	/// ```
	/// use iced_x86::*;
	///
	/// // adc rsi,rcx
	/// // xor rdi,5Ah
	/// let bytes = b"\x48\x11\xCE\x48\x83\xF7\x5A";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	///
	/// // adc rsi,rcx
	/// let instr = decoder.decode();
	/// assert_eq!(RflagsBits::CF, instr.rflags_read());
	/// assert_eq!(RflagsBits::OF | RflagsBits::SF | RflagsBits::ZF | RflagsBits::AF | RflagsBits::CF | RflagsBits::PF, instr.rflags_written());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_cleared());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_set());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_undefined());
	/// assert_eq!(RflagsBits::OF | RflagsBits::SF | RflagsBits::ZF | RflagsBits::AF | RflagsBits::CF | RflagsBits::PF, instr.rflags_modified());
	///
	/// // xor rdi,5Ah
	/// let instr = decoder.decode();
	/// assert_eq!(RflagsBits::NONE, instr.rflags_read());
	/// assert_eq!(RflagsBits::SF | RflagsBits::ZF | RflagsBits::PF, instr.rflags_written());
	/// assert_eq!(RflagsBits::OF | RflagsBits::CF, instr.rflags_cleared());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_set());
	/// assert_eq!(RflagsBits::AF, instr.rflags_undefined());
	/// assert_eq!(RflagsBits::OF | RflagsBits::SF | RflagsBits::ZF | RflagsBits::AF | RflagsBits::CF | RflagsBits::PF, instr.rflags_modified());
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
	/// ```
	/// use iced_x86::*;
	///
	/// // adc rsi,rcx
	/// // xor rdi,5Ah
	/// let bytes = b"\x48\x11\xCE\x48\x83\xF7\x5A";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	///
	/// // adc rsi,rcx
	/// let instr = decoder.decode();
	/// assert_eq!(RflagsBits::CF, instr.rflags_read());
	/// assert_eq!(RflagsBits::OF | RflagsBits::SF | RflagsBits::ZF | RflagsBits::AF | RflagsBits::CF | RflagsBits::PF, instr.rflags_written());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_cleared());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_set());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_undefined());
	/// assert_eq!(RflagsBits::OF | RflagsBits::SF | RflagsBits::ZF | RflagsBits::AF | RflagsBits::CF | RflagsBits::PF, instr.rflags_modified());
	///
	/// // xor rdi,5Ah
	/// let instr = decoder.decode();
	/// assert_eq!(RflagsBits::NONE, instr.rflags_read());
	/// assert_eq!(RflagsBits::SF | RflagsBits::ZF | RflagsBits::PF, instr.rflags_written());
	/// assert_eq!(RflagsBits::OF | RflagsBits::CF, instr.rflags_cleared());
	/// assert_eq!(RflagsBits::NONE, instr.rflags_set());
	/// assert_eq!(RflagsBits::AF, instr.rflags_undefined());
	/// assert_eq!(RflagsBits::OF | RflagsBits::SF | RflagsBits::ZF | RflagsBits::AF | RflagsBits::CF | RflagsBits::PF, instr.rflags_modified());
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
	/// ```
	/// use iced_x86::*;
	///
	/// // setbe al
	/// let bytes = b"\x0F\x96\xC0";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	///
	/// let mut instr = decoder.decode();
	/// assert_eq!(Code::Setbe_rm8, instr.code());
	/// assert_eq!(ConditionCode::be, instr.condition_code());
	/// instr.negate_condition_code();
	/// assert_eq!(Code::Seta_rm8, instr.code());
	/// assert_eq!(ConditionCode::a, instr.condition_code());
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
	/// ```
	/// use iced_x86::*;
	///
	/// // jbe near ptr label
	/// let bytes = b"\x0F\x86\x5A\xA5\x12\x34";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	///
	/// let mut instr = decoder.decode();
	/// assert_eq!(Code::Jbe_rel32_64, instr.code());
	/// instr.as_short_branch();
	/// assert_eq!(Code::Jbe_rel8_64, instr.code());
	/// instr.as_short_branch();
	/// assert_eq!(Code::Jbe_rel8_64, instr.code());
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
	/// ```
	/// use iced_x86::*;
	///
	/// // jbe short label
	/// let bytes = b"\x76\x5A";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	///
	/// let mut instr = decoder.decode();
	/// assert_eq!(Code::Jbe_rel8_64, instr.code());
	/// instr.as_near_branch();
	/// assert_eq!(Code::Jbe_rel32_64, instr.code());
	/// instr.as_near_branch();
	/// assert_eq!(Code::Jbe_rel32_64, instr.code());
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
	/// ```
	/// use iced_x86::*;
	///
	/// // setbe al
	/// // jl short label
	/// // cmovne ecx,esi
	/// // nop
	/// let bytes = b"\x0F\x96\xC0\x7C\x5A\x0F\x45\xCE\x90";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	///
	/// // setbe al
	/// let instr = decoder.decode();
	/// assert_eq!(ConditionCode::be, instr.condition_code());
	///
	/// // jl short label
	/// let instr = decoder.decode();
	/// assert_eq!(ConditionCode::l, instr.condition_code());
	///
	/// // cmovne ecx,esi
	/// let instr = decoder.decode();
	/// assert_eq!(ConditionCode::ne, instr.condition_code());
	///
	/// // nop
	/// let instr = decoder.decode();
	/// assert_eq!(ConditionCode::None, instr.condition_code());
	/// ```
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "conditionCode")]
	pub fn condition_code(&self) -> ConditionCode {
		iced_to_condition_code(self.0.condition_code())
	}
}

#[wasm_bindgen]
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
#[cfg(feature = "instruction_api")]
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
#[cfg(feature = "instruction_api")]
impl Instruction {
	/// Checks if this instance is equal to another instance. It ignores some fields,
	/// such as the IP address, code size, etc.
	pub fn equals(&self, other: &Instruction) -> bool {
		self.0.eq(&other.0)
	}

	/// Gets the default disassembly string
	#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm"))]
	#[wasm_bindgen(js_name = "toDisplay")]
	pub fn to_string_js(&self) -> String {
		self.0.to_string()
	}

	/// Clones this instance
	#[wasm_bindgen(js_name = "clone")]
	pub fn clone_js(&self) -> Self {
		Self(self.0)
	}
}
