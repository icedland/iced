// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#![allow(clippy::needless_question_mark)]

#[cfg(any(all(feature = "encoder", feature = "instr_api"), feature = "instr_create"))]
use crate::code::code_to_iced;
#[cfg(feature = "instr_api")]
use crate::code::iced_to_code;
#[cfg(any(feature = "instr_api", feature = "instr_create"))]
use crate::code::Code;
#[cfg(feature = "encoder")]
#[cfg(feature = "instr_api")]
use crate::code_size::code_size_to_iced;
#[cfg(feature = "instr_api")]
use crate::code_size::{iced_to_code_size, CodeSize};
#[cfg(feature = "instr_info")]
#[cfg(feature = "instr_api")]
use crate::condition_code::{iced_to_condition_code, ConditionCode};
#[cfg(feature = "instr_info")]
#[cfg(feature = "instr_api")]
use crate::encoding_kind::{iced_to_encoding_kind, EncodingKind};
#[cfg(any(feature = "instr_api", feature = "instr_create"))]
use crate::ex_utils::to_js_error;
#[cfg(feature = "instr_info")]
#[cfg(feature = "instr_api")]
use crate::flow_control::{iced_to_flow_control, FlowControl};
#[cfg(feature = "instr_create")]
use crate::memory_operand::MemoryOperand;
#[cfg(feature = "instr_api")]
use crate::memory_size::{iced_to_memory_size, MemorySize};
#[cfg(feature = "instr_api")]
use crate::mnemonic::{iced_to_mnemonic, Mnemonic};
#[cfg(all(feature = "encoder", feature = "mvex"))]
use crate::mvex_rm_conv::mvex_reg_mem_conv_to_iced;
#[cfg(all(feature = "instr_api", feature = "mvex"))]
use crate::mvex_rm_conv::{iced_to_mvex_reg_mem_conv, MvexRegMemConv};
#[cfg(all(feature = "encoder", feature = "op_code_info"))]
#[cfg(feature = "instr_api")]
use crate::op_code_info::OpCodeInfo;
#[cfg(feature = "encoder")]
#[cfg(feature = "instr_api")]
use crate::op_kind::op_kind_to_iced;
#[cfg(feature = "instr_api")]
use crate::op_kind::{iced_to_op_kind, OpKind};
#[cfg(feature = "instr_api")]
use crate::register::iced_to_register;
#[cfg(any(all(feature = "encoder", feature = "instr_api"), feature = "instr_create"))]
use crate::register::register_to_iced;
#[cfg(any(feature = "instr_api", feature = "instr_create"))]
use crate::register::Register;
#[cfg(feature = "instr_create")]
use crate::rep_prefix_kind::{rep_prefix_kind_to_iced, RepPrefixKind};
#[cfg(feature = "encoder")]
#[cfg(feature = "instr_api")]
use crate::rounding_control::rounding_control_to_iced;
#[cfg(feature = "instr_api")]
use crate::rounding_control::{iced_to_rounding_control, RoundingControl};
use wasm_bindgen::prelude::*;

/// A 16/32/64-bit x86 instruction. Created by [`Decoder`] or by `Instruction.with*()` methods.
///
/// [`Decoder`]: struct.Decoder.html
#[wasm_bindgen]
pub struct Instruction(pub(crate) iced_x86_rust::Instruction);

// ip() and length() are useful when disassembling code so they're always available
#[wasm_bindgen]
#[allow(clippy::len_without_is_empty)]
#[allow(clippy::new_without_default)]
impl Instruction {
	/// Creates an empty `Instruction` (all fields are cleared). See also the `create*()` constructor methods.
	#[wasm_bindgen(constructor)]
	pub fn new() -> Self {
		Self(iced_x86_rust::Instruction::new())
	}

	/// Gets the 64-bit IP of the instruction
	#[wasm_bindgen(getter)]
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
	pub fn set_ip16(&mut self, #[allow(non_snake_case)] newValue: u16) {
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
	pub fn set_ip32(&mut self, #[allow(non_snake_case)] newValue: u32) {
		self.0.set_ip32(newValue)
	}

	/// Sets the 64-bit IP of the instruction
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	pub fn set_ip(&mut self, #[allow(non_snake_case)] newValue: u64) {
		self.0.set_ip(newValue)
	}

	/// Gets the 16-bit IP of the next instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "nextIP16")]
	pub fn next_ip16(&self) -> u16 {
		self.0.next_ip16()
	}

	/// Sets the 16-bit IP of the next instruction
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "nextIP16")]
	#[cfg(feature = "encoder")]
	pub fn set_next_ip16(&mut self, #[allow(non_snake_case)] newValue: u16) {
		self.0.set_next_ip16(newValue)
	}

	/// Gets the 32-bit IP of the next instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "nextIP32")]
	pub fn next_ip32(&self) -> u32 {
		self.0.next_ip32()
	}

	/// Sets the 32-bit IP of the next instruction
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "nextIP32")]
	#[cfg(feature = "encoder")]
	pub fn set_next_ip32(&mut self, #[allow(non_snake_case)] newValue: u32) {
		self.0.set_next_ip32(newValue)
	}

	/// Gets the 64-bit IP of the next instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "nextIP")]
	pub fn next_ip(&self) -> u64 {
		self.0.next_ip()
	}

	/// Sets the 64-bit IP of the next instruction
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "nextIP")]
	#[cfg(feature = "encoder")]
	pub fn set_next_ip(&mut self, #[allow(non_snake_case)] newValue: u64) {
		self.0.set_next_ip(newValue)
	}

	/// Gets the code size (a [`CodeSize`] enum value) when the instruction was decoded. This value is informational and can
	/// be used by a formatter.
	///
	/// [`CodeSize`]: enum.CodeSize.html
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "codeSize")]
	pub fn code_size(&self) -> CodeSize {
		iced_to_code_size(self.0.code_size())
	}

	/// Gets the code size (a [`CodeSize`] enum value) when the instruction was decoded. This value is informational and can
	/// be used by a formatter.
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "codeSize")]
	#[cfg(feature = "encoder")]
	pub fn set_code_size(&mut self, #[allow(non_snake_case)] newValue: CodeSize) {
		self.0.set_code_size(code_size_to_iced(newValue))
	}

	/// Checks if it's an invalid instruction ([`code`] == [`Code.INVALID`])
	///
	/// [`code`]: #method.code
	/// [`Code.INVALID`]: enum.Code.html#variant.INVALID
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isInvalid")]
	pub fn is_invalid(&self) -> bool {
		self.0.is_invalid()
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
	pub fn set_code(&mut self, #[allow(non_snake_case)] newValue: Code) {
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
	/// const { Decoder, DecoderOptions } = require("iced-x86");
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
	pub fn set_length(&mut self, #[allow(non_snake_case)] newValue: u32) {
		self.0.set_len(newValue as usize)
	}

	/// `true` if the instruction has the `XACQUIRE` prefix (`F2`)
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "hasXacquirePrefix")]
	pub fn has_xacquire_prefix(&self) -> bool {
		self.0.has_xacquire_prefix()
	}

	/// `true` if the instruction has the `XACQUIRE` prefix (`F2`)
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "hasXacquirePrefix")]
	#[cfg(feature = "encoder")]
	pub fn set_has_xacquire_prefix(&mut self, #[allow(non_snake_case)] newValue: bool) {
		self.0.set_has_xacquire_prefix(newValue)
	}

	/// `true` if the instruction has the `XRELEASE` prefix (`F3`)
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "hasXreleasePrefix")]
	pub fn has_xrelease_prefix(&self) -> bool {
		self.0.has_xrelease_prefix()
	}

	/// `true` if the instruction has the `XRELEASE` prefix (`F3`)
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "hasXreleasePrefix")]
	#[cfg(feature = "encoder")]
	pub fn set_has_xrelease_prefix(&mut self, #[allow(non_snake_case)] newValue: bool) {
		self.0.set_has_xrelease_prefix(newValue)
	}

	/// `true` if the instruction has the `REPE` or `REP` prefix (`F3`)
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "hasRepPrefix")]
	pub fn has_rep_prefix(&self) -> bool {
		self.0.has_rep_prefix()
	}

	/// `true` if the instruction has the `REPE` or `REP` prefix (`F3`)
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "hasRepPrefix")]
	#[cfg(feature = "encoder")]
	pub fn set_has_rep_prefix(&mut self, #[allow(non_snake_case)] newValue: bool) {
		self.0.set_has_rep_prefix(newValue)
	}

	/// `true` if the instruction has the `REPE` or `REP` prefix (`F3`)
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "hasRepePrefix")]
	pub fn has_repe_prefix(&self) -> bool {
		self.0.has_repe_prefix()
	}

	/// `true` if the instruction has the `REPE` or `REP` prefix (`F3`)
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "hasRepePrefix")]
	#[cfg(feature = "encoder")]
	pub fn set_has_repe_prefix(&mut self, #[allow(non_snake_case)] newValue: bool) {
		self.0.set_has_repe_prefix(newValue)
	}

	/// `true` if the instruction has the `REPNE` prefix (`F2`)
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "hasRepnePrefix")]
	pub fn has_repne_prefix(&self) -> bool {
		self.0.has_repne_prefix()
	}

	/// `true` if the instruction has the `REPNE` prefix (`F2`)
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "hasRepnePrefix")]
	#[cfg(feature = "encoder")]
	pub fn set_has_repne_prefix(&mut self, #[allow(non_snake_case)] newValue: bool) {
		self.0.set_has_repne_prefix(newValue)
	}

	/// `true` if the instruction has the `LOCK` prefix (`F0`)
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "hasLockPrefix")]
	pub fn has_lock_prefix(&self) -> bool {
		self.0.has_lock_prefix()
	}

	/// `true` if the instruction has the `LOCK` prefix (`F0`)
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "hasLockPrefix")]
	#[cfg(feature = "encoder")]
	pub fn set_has_lock_prefix(&mut self, #[allow(non_snake_case)] newValue: bool) {
		self.0.set_has_lock_prefix(newValue)
	}

	/// Gets operand #0's kind (an [`OpKind`] enum value) if the operand exists (see [`opCount`] and [`opKind`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	/// [`opKind`]: #method.op_kind
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op0Kind")]
	pub fn op0_kind(&self) -> OpKind {
		iced_to_op_kind(self.0.op0_kind())
	}

	/// Sets operand #0's kind (an [`OpKind`] enum value) if the operand exists (see [`opCount`] and [`opKind`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	/// [`opKind`]: #method.set_op_kind
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "op0Kind")]
	#[cfg(feature = "encoder")]
	pub fn set_op0_kind(&mut self, #[allow(non_snake_case)] newValue: OpKind) {
		self.0.set_op0_kind(op_kind_to_iced(newValue))
	}

	/// Gets operand #1's kind (an [`OpKind`] enum value) if the operand exists (see [`opCount`] and [`opKind`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	/// [`opKind`]: #method.op_kind
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op1Kind")]
	pub fn op1_kind(&self) -> OpKind {
		iced_to_op_kind(self.0.op1_kind())
	}

	/// Sets operand #1's kind (an [`OpKind`] enum value) if the operand exists (see [`opCount`] and [`opKind`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	/// [`opKind`]: #method.set_op_kind
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "op1Kind")]
	#[cfg(feature = "encoder")]
	pub fn set_op1_kind(&mut self, #[allow(non_snake_case)] newValue: OpKind) {
		self.0.set_op1_kind(op_kind_to_iced(newValue))
	}

	/// Gets operand #2's kind (an [`OpKind`] enum value) if the operand exists (see [`opCount`] and [`opKind`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	/// [`opKind`]: #method.op_kind
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op2Kind")]
	pub fn op2_kind(&self) -> OpKind {
		iced_to_op_kind(self.0.op2_kind())
	}

	/// Sets operand #2's kind (an [`OpKind`] enum value) if the operand exists (see [`opCount`] and [`opKind`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	/// [`opKind`]: #method.set_op_kind
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "op2Kind")]
	#[cfg(feature = "encoder")]
	pub fn set_op2_kind(&mut self, #[allow(non_snake_case)] newValue: OpKind) {
		self.0.set_op2_kind(op_kind_to_iced(newValue))
	}

	/// Gets operand #3's kind (an [`OpKind`] enum value) if the operand exists (see [`opCount`] and [`opKind`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	/// [`opKind`]: #method.op_kind
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op3Kind")]
	pub fn op3_kind(&self) -> OpKind {
		iced_to_op_kind(self.0.op3_kind())
	}

	/// Sets operand #3's kind (an [`OpKind`] enum value) if the operand exists (see [`opCount`] and [`opKind`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	/// [`opKind`]: #method.set_op_kind
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "op3Kind")]
	#[cfg(feature = "encoder")]
	pub fn set_op3_kind(&mut self, #[allow(non_snake_case)] newValue: OpKind) {
		self.0.set_op3_kind(op_kind_to_iced(newValue))
	}

	/// Gets operand #4's kind (an [`OpKind`] enum value) if the operand exists (see [`opCount`] and [`opKind`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	/// [`opKind`]: #method.op_kind
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op4Kind")]
	pub fn op4_kind(&self) -> OpKind {
		iced_to_op_kind(self.0.op4_kind())
	}

	/// Sets operand #4's kind (an [`OpKind`] enum value) if the operand exists (see [`opCount`] and [`opKind`])
	///
	/// [`OpKind`]: enum.OpKind.html
	/// [`opCount`]: #method.op_count
	/// [`opKind`]: #method.set_op_kind
	///
	/// # Throws
	///
	/// Throws if `newValue` is invalid.
	///
	/// # Arguments
	///
	/// * `newValue`: new value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "op4Kind")]
	#[cfg(feature = "encoder")]
	pub fn set_op4_kind(&mut self, #[allow(non_snake_case)] newValue: OpKind) -> Result<(), JsValue> {
		self.0.try_set_op4_kind(op_kind_to_iced(newValue)).map_err(to_js_error)
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
	/// const { Decoder, DecoderOptions, OpKind, Register } = require("iced-x86");
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
	#[wasm_bindgen(js_name = "opKind")]
	pub fn op_kind(&self, operand: u32) -> Result<OpKind, JsValue> {
		Ok(iced_to_op_kind(self.0.try_op_kind(operand).map_err(to_js_error)?))
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
	#[wasm_bindgen(js_name = "setOpKind")]
	#[cfg(feature = "encoder")]
	pub fn set_op_kind(&mut self, operand: u32, #[allow(non_snake_case)] opKind: OpKind) -> Result<(), JsValue> {
		self.0.try_set_op_kind(operand, op_kind_to_iced(opKind)).map_err(to_js_error)
	}

	/// Checks if the instruction has a segment override prefix, see [`segmentPrefix`]
	///
	/// [`segmentPrefix`]: #method.segment_prefix
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "hasSegmentPrefix")]
	pub fn has_segment_prefix(&self) -> bool {
		self.0.has_segment_prefix()
	}

	/// Gets the segment override prefix (a [`Register`] enum value) or [`Register.None`] if none. See also [`memorySegment`].
	/// Use this method if the operand has kind [`OpKind.Memory`],
	/// [`OpKind.MemorySegSI`], [`OpKind.MemorySegESI`], [`OpKind.MemorySegRSI`]
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`memorySegment`]: #method.memory_segment
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	/// [`OpKind.MemorySegSI`]: enum.OpKind.html#variant.MemorySegSI
	/// [`OpKind.MemorySegESI`]: enum.OpKind.html#variant.MemorySegESI
	/// [`OpKind.MemorySegESI`]: enum.OpKind.html#variant.MemorySegESI
	/// [`OpKind.MemorySegRSI`]: enum.OpKind.html#variant.MemorySegRSI
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "segmentPrefix")]
	pub fn segment_prefix(&self) -> Register {
		iced_to_register(self.0.segment_prefix())
	}

	/// Sets the segment override prefix (a [`Register`] enum value) or [`Register.None`] if none. See also [`memorySegment`].
	/// Use this method if the operand has kind [`OpKind.Memory`],
	/// [`OpKind.MemorySegSI`], [`OpKind.MemorySegESI`], [`OpKind.MemorySegRSI`]
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`memorySegment`]: #method.memory_segment
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	/// [`OpKind.MemorySegSI`]: enum.OpKind.html#variant.MemorySegSI
	/// [`OpKind.MemorySegESI`]: enum.OpKind.html#variant.MemorySegESI
	/// [`OpKind.MemorySegESI`]: enum.OpKind.html#variant.MemorySegESI
	/// [`OpKind.MemorySegRSI`]: enum.OpKind.html#variant.MemorySegRSI
	///
	/// # Arguments
	///
	/// * `newValue`: Segment register prefix
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "segmentPrefix")]
	#[cfg(feature = "encoder")]
	pub fn set_segment_prefix(&mut self, #[allow(non_snake_case)] newValue: Register) {
		self.0.set_segment_prefix(register_to_iced(newValue))
	}

	/// Gets the effective segment register (a [`Register`] enum value) used to reference the memory location.
	/// Use this method if the operand has kind [`OpKind.Memory`],
	/// [`OpKind.MemorySegSI`], [`OpKind.MemorySegESI`], [`OpKind.MemorySegRSI`]
	///
	/// [`Register`]: enum.Register.html
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
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
	/// a signed byte if it's an EVEX/MVEX encoded instruction.
	/// Use this method if the operand has kind [`OpKind.Memory`]
	///
	/// [`memoryDisplacement`]: #method.memory_displacement
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "memoryDisplSize")]
	pub fn memory_displ_size(&self) -> u32 {
		self.0.memory_displ_size()
	}

	/// Sets the size of the memory displacement in bytes. Valid values are `0`, `1` (16/32/64-bit), `2` (16-bit), `4` (32-bit), `8` (64-bit).
	/// Note that the return value can be 1 and [`memoryDisplacement`] may still not fit in
	/// a signed byte if it's an EVEX/MVEX encoded instruction.
	/// Use this method if the operand has kind [`OpKind.Memory`]
	///
	/// [`memoryDisplacement`]: #method.memory_displacement
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	///
	/// # Arguments
	///
	/// * `newValue`: Displacement size
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "memoryDisplSize")]
	#[cfg(feature = "encoder")]
	pub fn set_memory_displ_size(&mut self, #[allow(non_snake_case)] newValue: u32) {
		self.0.set_memory_displ_size(newValue)
	}

	/// `true` if the data is broadcast (EVEX instructions only)
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isBroadcast")]
	pub fn is_broadcast(&self) -> bool {
		self.0.is_broadcast()
	}

	/// Sets the is broadcast flag (EVEX instructions only)
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "isBroadcast")]
	#[cfg(feature = "encoder")]
	pub fn set_is_broadcast(&mut self, #[allow(non_snake_case)] newValue: bool) {
		self.0.set_is_broadcast(newValue)
	}

	/// `true` if eviction hint bit is set (`{eh}`) (MVEX instructions only)
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isMvexEvictionHint")]
	#[cfg(feature = "mvex")]
	pub fn is_mvex_eviction_hint(&self) -> bool {
		self.0.is_mvex_eviction_hint()
	}

	/// `true` if eviction hint bit is set (`{eh}`) (MVEX instructions only)
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "isMvexEvictionHint")]
	#[cfg(all(feature = "encoder", feature = "mvex"))]
	pub fn set_is_mvex_eviction_hint(&mut self, #[allow(non_snake_case)] newValue: bool) {
		self.0.set_is_mvex_eviction_hint(newValue)
	}

	/// (MVEX) Register/memory operand conversion function
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "mvexRegMemConv")]
	#[cfg(feature = "mvex")]
	pub fn mvex_reg_mem_conv(&self) -> MvexRegMemConv {
		iced_to_mvex_reg_mem_conv(self.0.mvex_reg_mem_conv())
	}

	/// (MVEX) Register/memory operand conversion function
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "mvexRegMemConv")]
	#[cfg(all(feature = "encoder", feature = "mvex"))]
	pub fn set_mvex_reg_mem_conv(&mut self, #[allow(non_snake_case)] newValue: MvexRegMemConv) {
		self.0.set_mvex_reg_mem_conv(mvex_reg_mem_conv_to_iced(newValue))
	}

	/// Gets the size of the memory location (a [`MemorySize`] enum value) that is referenced by the operand. See also [`isBroadcast`].
	/// Use this method if the operand has kind [`OpKind.Memory`],
	/// [`OpKind.MemorySegSI`], [`OpKind.MemorySegESI`], [`OpKind.MemorySegRSI`],
	/// [`OpKind.MemoryESDI`], [`OpKind.MemoryESEDI`], [`OpKind.MemoryESRDI`]
	///
	/// [`MemorySize`]: enum.MemorySize.html
	/// [`isBroadcast`]: #method.is_broadcast
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
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
	#[wasm_bindgen(js_name = "memoryIndexScale")]
	pub fn memory_index_scale(&self) -> u32 {
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
	#[wasm_bindgen(js_name = "memoryIndexScale")]
	#[cfg(feature = "encoder")]
	pub fn set_memory_index_scale(&mut self, #[allow(non_snake_case)] newValue: u32) {
		self.0.set_memory_index_scale(newValue)
	}

	/// Gets signed 64 bits of the memory operand's displacement or the 64-bit absolute address if it's
	/// an `EIP` or `RIP` relative memory operand.
	/// Use this method if the operand has kind [`OpKind.Memory`]
	///
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "memoryDisplacement")]
	pub fn memory_displacement(&self) -> i64 {
		self.0.memory_displacement64() as i64
	}

	/// Gets signed 64 bits of the memory operand's displacement or the 64-bit absolute address if it's
	/// an `EIP` or `RIP` relative memory operand.
	/// Use this method if the operand has kind [`OpKind.Memory`]
	///
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "memoryDisplacement")]
	#[cfg(feature = "encoder")]
	pub fn set_memory_displacement(&mut self, #[allow(non_snake_case)] newValue: i64) {
		self.0.set_memory_displacement64(newValue as u64)
	}

	/// Gets unsigned 64 bits of the memory operand's displacement or the 64-bit absolute address if it's
	/// an `EIP` or `RIP` relative memory operand.
	/// Use this method if the operand has kind [`OpKind.Memory`]
	///
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "memoryDisplacementU64")]
	pub fn memory_displacement_u64(&self) -> u64 {
		self.0.memory_displacement64()
	}

	/// Gets unsigned 64 bits of the memory operand's displacement or the 64-bit absolute address if it's
	/// an `EIP` or `RIP` relative memory operand.
	/// Use this method if the operand has kind [`OpKind.Memory`]
	///
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "memoryDisplacementU64")]
	#[cfg(feature = "encoder")]
	pub fn set_memory_displacement_u64(&mut self, #[allow(non_snake_case)] newValue: u64) {
		self.0.set_memory_displacement64(newValue)
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
	pub fn immediate(&self, operand: u32) -> Result<u64, JsValue> {
		self.0.try_immediate(operand).map_err(to_js_error)
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
	pub fn set_immediate_i32(&mut self, operand: u32, #[allow(non_snake_case)] newValue: i32) -> Result<(), JsValue> {
		self.0.try_set_immediate_i32(operand, newValue).map_err(to_js_error)
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
	pub fn set_immediate_u32(&mut self, operand: u32, #[allow(non_snake_case)] newValue: u32) -> Result<(), JsValue> {
		self.0.try_set_immediate_u32(operand, newValue).map_err(to_js_error)
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
	pub fn set_immediate_i64(&mut self, operand: u32, #[allow(non_snake_case)] newValue: i64) -> Result<(), JsValue> {
		self.0.try_set_immediate_i64(operand, newValue).map_err(to_js_error)
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
	pub fn set_immediate_u64(&mut self, operand: u32, #[allow(non_snake_case)] newValue: u64) -> Result<(), JsValue> {
		self.0.try_set_immediate_u64(operand, newValue).map_err(to_js_error)
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
	pub fn set_immediate8(&mut self, #[allow(non_snake_case)] newValue: u8) {
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
	pub fn set_immediate8_2nd(&mut self, #[allow(non_snake_case)] newValue: u8) {
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
	pub fn set_immediate16(&mut self, #[allow(non_snake_case)] newValue: u16) {
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
	pub fn set_immediate32(&mut self, #[allow(non_snake_case)] newValue: u32) {
		self.0.set_immediate32(newValue)
	}

	/// Gets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate64`]
	///
	/// [`OpKind.Immediate64`]: enum.OpKind.html#variant.Immediate64
	#[wasm_bindgen(getter)]
	pub fn immediate64(&self) -> u64 {
		self.0.immediate64()
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
	pub fn set_immediate64(&mut self, #[allow(non_snake_case)] newValue: u64) {
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
	pub fn set_immediate8to16(&mut self, #[allow(non_snake_case)] newValue: i16) {
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
	pub fn set_immediate8to32(&mut self, #[allow(non_snake_case)] newValue: i32) {
		self.0.set_immediate8to32(newValue)
	}

	/// Gets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate8to64`]
	///
	/// [`OpKind.Immediate8to64`]: enum.OpKind.html#variant.Immediate8to64
	#[wasm_bindgen(getter)]
	pub fn immediate8to64(&self) -> i64 {
		self.0.immediate8to64()
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
	pub fn set_immediate8to64(&mut self, #[allow(non_snake_case)] newValue: i64) {
		self.0.set_immediate8to64(newValue)
	}

	/// Gets the operand's immediate value. Use this method if the operand has kind [`OpKind.Immediate32to64`]
	///
	/// [`OpKind.Immediate32to64`]: enum.OpKind.html#variant.Immediate32to64
	#[wasm_bindgen(getter)]
	pub fn immediate32to64(&self) -> i64 {
		self.0.immediate32to64()
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
	pub fn set_immediate32to64(&mut self, #[allow(non_snake_case)] newValue: i64) {
		self.0.set_immediate32to64(newValue)
	}

	/// Gets the operand's branch target. Use this method if the operand has kind [`OpKind.NearBranch16`]
	///
	/// [`OpKind.NearBranch16`]: enum.OpKind.html#variant.NearBranch16
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "nearBranch16")]
	pub fn near_branch16(&self) -> u16 {
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
	#[wasm_bindgen(js_name = "nearBranch16")]
	#[cfg(feature = "encoder")]
	pub fn set_near_branch16(&mut self, #[allow(non_snake_case)] newValue: u16) {
		self.0.set_near_branch16(newValue)
	}

	/// Gets the operand's branch target. Use this method if the operand has kind [`OpKind.NearBranch32`]
	///
	/// [`OpKind.NearBranch32`]: enum.OpKind.html#variant.NearBranch32
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "nearBranch32")]
	pub fn near_branch32(&self) -> u32 {
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
	#[wasm_bindgen(js_name = "nearBranch32")]
	#[cfg(feature = "encoder")]
	pub fn set_near_branch32(&mut self, #[allow(non_snake_case)] newValue: u32) {
		self.0.set_near_branch32(newValue)
	}

	/// Gets the operand's branch target. Use this method if the operand has kind [`OpKind.NearBranch64`]
	///
	/// [`OpKind.NearBranch64`]: enum.OpKind.html#variant.NearBranch64
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "nearBranch64")]
	pub fn near_branch64(&self) -> u64 {
		self.0.near_branch64()
	}

	/// Sets the operand's branch target. Use this method if the operand has kind [`OpKind.NearBranch64`]
	///
	/// [`OpKind.NearBranch64`]: enum.OpKind.html#variant.NearBranch64
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "nearBranch64")]
	#[cfg(feature = "encoder")]
	pub fn set_near_branch64(&mut self, #[allow(non_snake_case)] newValue: u64) {
		self.0.set_near_branch64(newValue)
	}

	/// Gets the near branch target if it's a `CALL`/`JMP`/`Jcc` near branch instruction
	/// (i.e., if [`op0Kind`] is [`OpKind.NearBranch16`], [`OpKind.NearBranch32`] or [`OpKind.NearBranch64`])
	///
	/// [`op0Kind`]: #method.op0_kind
	/// [`OpKind.NearBranch16`]: enum.OpKind.html#variant.NearBranch16
	/// [`OpKind.NearBranch32`]: enum.OpKind.html#variant.NearBranch32
	/// [`OpKind.NearBranch64`]: enum.OpKind.html#variant.NearBranch64
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "nearBranchTarget")]
	pub fn near_branch_target(&self) -> u64 {
		self.0.near_branch_target()
	}

	/// Gets the operand's branch target. Use this method if the operand has kind [`OpKind.FarBranch16`]
	///
	/// [`OpKind.FarBranch16`]: enum.OpKind.html#variant.FarBranch16
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "farBranch16")]
	pub fn far_branch16(&self) -> u16 {
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
	#[wasm_bindgen(js_name = "farBranch16")]
	#[cfg(feature = "encoder")]
	pub fn set_far_branch16(&mut self, #[allow(non_snake_case)] newValue: u16) {
		self.0.set_far_branch16(newValue)
	}

	/// Gets the operand's branch target. Use this method if the operand has kind [`OpKind.FarBranch32`]
	///
	/// [`OpKind.FarBranch32`]: enum.OpKind.html#variant.FarBranch32
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "farBranch32")]
	pub fn far_branch32(&self) -> u32 {
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
	#[wasm_bindgen(js_name = "farBranch32")]
	#[cfg(feature = "encoder")]
	pub fn set_far_branch32(&mut self, #[allow(non_snake_case)] newValue: u32) {
		self.0.set_far_branch32(newValue)
	}

	/// Gets the operand's branch target selector. Use this method if the operand has kind [`OpKind.FarBranch16`] or [`OpKind.FarBranch32`]
	///
	/// [`OpKind.FarBranch16`]: enum.OpKind.html#variant.FarBranch16
	/// [`OpKind.FarBranch32`]: enum.OpKind.html#variant.FarBranch32
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "farBranchSelector")]
	pub fn far_branch_selector(&self) -> u16 {
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
	#[wasm_bindgen(js_name = "farBranchSelector")]
	#[cfg(feature = "encoder")]
	pub fn set_far_branch_selector(&mut self, #[allow(non_snake_case)] newValue: u16) {
		self.0.set_far_branch_selector(newValue)
	}

	/// Gets the memory operand's base register (a [`Register`] enum value) or [`Register.None`] if none. Use this method if the operand has kind [`OpKind.Memory`]
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "memoryBase")]
	pub fn memory_base(&self) -> Register {
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
	#[wasm_bindgen(js_name = "memoryBase")]
	#[cfg(feature = "encoder")]
	pub fn set_memory_base(&mut self, #[allow(non_snake_case)] newValue: Register) {
		self.0.set_memory_base(register_to_iced(newValue))
	}

	/// Gets the memory operand's index register (a [`Register`] enum value) or [`Register.None`] if none. Use this method if the operand has kind [`OpKind.Memory`]
	///
	/// [`Register`]: enum.Register.html
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Memory`]: enum.OpKind.html#variant.Memory
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "memoryIndex")]
	pub fn memory_index(&self) -> Register {
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
	#[wasm_bindgen(js_name = "memoryIndex")]
	#[cfg(feature = "encoder")]
	pub fn set_memory_index(&mut self, #[allow(non_snake_case)] newValue: Register) {
		self.0.set_memory_index(register_to_iced(newValue))
	}

	/// Gets operand #0's register value (a [`Register`] enum value). Use this method if operand #0 ([`op0Kind`]) has kind [`OpKind.Register`], see [`opCount`] and [`opRegister`]
	///
	/// [`Register`]: enum.Register.html
	/// [`op0Kind`]: #method.op0_kind
	/// [`opCount`]: #method.op_count
	/// [`opRegister`]: #method.op_register
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op0Register")]
	pub fn op0_register(&self) -> Register {
		iced_to_register(self.0.op0_register())
	}

	/// Sets operand #0's register value (a [`Register`] enum value). Use this method if operand #0 ([`op0Kind`]) has kind [`OpKind.Register`], see [`opCount`] and [`opRegister`]
	///
	/// [`Register`]: enum.Register.html
	/// [`op0Kind`]: #method.op0_kind
	/// [`opCount`]: #method.op_count
	/// [`opRegister`]: #method.op_register
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "op0Register")]
	#[cfg(feature = "encoder")]
	pub fn set_op0_register(&mut self, #[allow(non_snake_case)] newValue: Register) {
		self.0.set_op0_register(register_to_iced(newValue))
	}

	/// Gets operand #1's register value (a [`Register`] enum value). Use this method if operand #1 ([`op0Kind`]) has kind [`OpKind.Register`], see [`opCount`] and [`opRegister`]
	///
	/// [`Register`]: enum.Register.html
	/// [`op0Kind`]: #method.op0_kind
	/// [`opCount`]: #method.op_count
	/// [`opRegister`]: #method.op_register
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op1Register")]
	pub fn op1_register(&self) -> Register {
		iced_to_register(self.0.op1_register())
	}

	/// Sets operand #1's register value (a [`Register`] enum value). Use this method if operand #1 ([`op0Kind`]) has kind [`OpKind.Register`], see [`opCount`] and [`opRegister`]
	///
	/// [`Register`]: enum.Register.html
	/// [`op0Kind`]: #method.op0_kind
	/// [`opCount`]: #method.op_count
	/// [`opRegister`]: #method.op_register
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "op1Register")]
	#[cfg(feature = "encoder")]
	pub fn set_op1_register(&mut self, #[allow(non_snake_case)] newValue: Register) {
		self.0.set_op1_register(register_to_iced(newValue))
	}

	/// Gets operand #2's register value (a [`Register`] enum value). Use this method if operand #2 ([`op0Kind`]) has kind [`OpKind.Register`], see [`opCount`] and [`opRegister`]
	///
	/// [`Register`]: enum.Register.html
	/// [`op0Kind`]: #method.op0_kind
	/// [`opCount`]: #method.op_count
	/// [`opRegister`]: #method.op_register
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op2Register")]
	pub fn op2_register(&self) -> Register {
		iced_to_register(self.0.op2_register())
	}

	/// Sets operand #2's register value (a [`Register`] enum value). Use this method if operand #2 ([`op0Kind`]) has kind [`OpKind.Register`], see [`opCount`] and [`opRegister`]
	///
	/// [`Register`]: enum.Register.html
	/// [`op0Kind`]: #method.op0_kind
	/// [`opCount`]: #method.op_count
	/// [`opRegister`]: #method.op_register
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "op2Register")]
	#[cfg(feature = "encoder")]
	pub fn set_op2_register(&mut self, #[allow(non_snake_case)] newValue: Register) {
		self.0.set_op2_register(register_to_iced(newValue))
	}

	/// Gets operand #3's register value (a [`Register`] enum value). Use this method if operand #3 ([`op0Kind`]) has kind [`OpKind.Register`], see [`opCount`] and [`opRegister`]
	///
	/// [`Register`]: enum.Register.html
	/// [`op0Kind`]: #method.op0_kind
	/// [`opCount`]: #method.op_count
	/// [`opRegister`]: #method.op_register
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op3Register")]
	pub fn op3_register(&self) -> Register {
		iced_to_register(self.0.op3_register())
	}

	/// Sets operand #3's register value (a [`Register`] enum value). Use this method if operand #3 ([`op0Kind`]) has kind [`OpKind.Register`], see [`opCount`] and [`opRegister`]
	///
	/// [`Register`]: enum.Register.html
	/// [`op0Kind`]: #method.op0_kind
	/// [`opCount`]: #method.op_count
	/// [`opRegister`]: #method.op_register
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "op3Register")]
	#[cfg(feature = "encoder")]
	pub fn set_op3_register(&mut self, #[allow(non_snake_case)] newValue: Register) {
		self.0.set_op3_register(register_to_iced(newValue))
	}

	/// Gets operand #4's register value (a [`Register`] enum value). Use this method if operand #4 ([`op0Kind`]) has kind [`OpKind.Register`], see [`opCount`] and [`opRegister`]
	///
	/// [`Register`]: enum.Register.html
	/// [`op0Kind`]: #method.op0_kind
	/// [`opCount`]: #method.op_count
	/// [`opRegister`]: #method.op_register
	/// [`Register.None`]: enum.Register.html#variant.None
	/// [`OpKind.Register`]: enum.OpKind.html#variant.Register
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "op4Register")]
	pub fn op4_register(&self) -> Register {
		iced_to_register(self.0.op4_register())
	}

	/// Sets operand #4's register value (a [`Register`] enum value). Use this method if operand #4 ([`op0Kind`]) has kind [`OpKind.Register`], see [`opCount`] and [`opRegister`]
	///
	/// [`Register`]: enum.Register.html
	/// [`op0Kind`]: #method.op0_kind
	/// [`opCount`]: #method.op_count
	/// [`opRegister`]: #method.op_register
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
	#[wasm_bindgen(js_name = "op4Register")]
	#[cfg(feature = "encoder")]
	pub fn set_op4_register(&mut self, #[allow(non_snake_case)] newValue: Register) -> Result<(), JsValue> {
		self.0.try_set_op4_register(register_to_iced(newValue)).map_err(to_js_error)
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
	/// const { Decoder, DecoderOptions, OpKind, Register } = require("iced-x86");
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
	#[wasm_bindgen(js_name = "opRegister")]
	pub fn op_register(&self, operand: u32) -> Result<Register, JsValue> {
		Ok(iced_to_register(self.0.try_op_register(operand).map_err(to_js_error)?))
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
	#[wasm_bindgen(js_name = "setOpRegister")]
	#[cfg(feature = "encoder")]
	pub fn set_op_register(&mut self, operand: u32, #[allow(non_snake_case)] newValue: Register) -> Result<(), JsValue> {
		self.0.try_set_op_register(operand, register_to_iced(newValue)).map_err(to_js_error)
	}

	/// Gets the opmask register ([`Register.K1`] - [`Register.K7`]) or [`Register.None`] if none
	///
	/// [`Register.K1`]: enum.Register.html#variant.K1
	/// [`Register.K7`]: enum.Register.html#variant.K7
	/// [`Register.None`]: enum.Register.html#variant.None
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "opMask")]
	pub fn op_mask(&self) -> Register {
		iced_to_register(self.0.op_mask())
	}

	/// Sets the opmask register ([`Register.K1`] - [`Register.K7`]) or [`Register.None`] if none
	///
	/// [`Register.K1`]: enum.Register.html#variant.K1
	/// [`Register.K7`]: enum.Register.html#variant.K7
	/// [`Register.None`]: enum.Register.html#variant.None
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "opMask")]
	#[cfg(feature = "encoder")]
	pub fn set_op_mask(&mut self, #[allow(non_snake_case)] newValue: Register) {
		self.0.set_op_mask(register_to_iced(newValue))
	}

	/// Checks if there's an opmask register ([`opMask`])
	///
	/// [`opMask`]: #method.op_mask
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "hasOpMask")]
	pub fn has_op_mask(&self) -> bool {
		self.0.has_op_mask()
	}

	/// `true` if zeroing-masking, `false` if merging-masking.
	/// Only used by most EVEX encoded instructions that use opmask registers.
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "zeroingMasking")]
	pub fn zeroing_masking(&self) -> bool {
		self.0.zeroing_masking()
	}

	/// `true` if zeroing-masking, `false` if merging-masking.
	/// Only used by most EVEX encoded instructions that use opmask registers.
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "zeroingMasking")]
	#[cfg(feature = "encoder")]
	pub fn set_zeroing_masking(&mut self, #[allow(non_snake_case)] newValue: bool) {
		self.0.set_zeroing_masking(newValue)
	}

	/// `true` if merging-masking, `false` if zeroing-masking.
	/// Only used by most EVEX encoded instructions that use opmask registers.
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "mergingMasking")]
	pub fn merging_masking(&self) -> bool {
		self.0.merging_masking()
	}

	/// `true` if merging-masking, `false` if zeroing-masking.
	/// Only used by most EVEX encoded instructions that use opmask registers.
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "mergingMasking")]
	#[cfg(feature = "encoder")]
	pub fn set_merging_masking(&mut self, #[allow(non_snake_case)] newValue: bool) {
		self.0.set_merging_masking(newValue)
	}

	/// Gets the rounding control (a [`RoundingControl`] enum value) (SAE is implied but [`suppressAllExceptions`] still returns `false`)
	/// or [`RoundingControl.None`] if the instruction doesn't use it.
	///
	/// [`RoundingControl`]: enum.RoundingControl.html
	/// [`suppressAllExceptions`]: #method.suppress_all_exceptions
	/// [`RoundingControl.None`]: enum.RoundingControl.html#variant.None
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "roundingControl")]
	pub fn rounding_control(&self) -> RoundingControl {
		iced_to_rounding_control(self.0.rounding_control())
	}

	/// Sets the rounding control (a [`RoundingControl`] enum value) (SAE is implied but [`suppressAllExceptions`] still returns `false`)
	/// or [`RoundingControl.None`] if the instruction doesn't use it.
	///
	/// [`RoundingControl`]: enum.RoundingControl.html
	/// [`suppressAllExceptions`]: #method.suppress_all_exceptions
	/// [`RoundingControl.None`]: enum.RoundingControl.html#variant.None
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[wasm_bindgen(js_name = "roundingControl")]
	#[cfg(feature = "encoder")]
	pub fn set_rounding_control(&mut self, #[allow(non_snake_case)] newValue: RoundingControl) {
		self.0.set_rounding_control(rounding_control_to_iced(newValue))
	}

	/// Checks if this is a VSIB instruction, see also [`isVsib32`], [`isVsib64`]
	///
	/// [`isVsib32`]: #method.is_vsib32
	/// [`isVsib64`]: #method.is_vsib64
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isVsib")]
	pub fn is_vsib(&self) -> bool {
		self.0.is_vsib()
	}

	/// VSIB instructions only ([`isVsib`]): `true` if it's using 32-bit indexes, `false` if it's using 64-bit indexes
	///
	/// [`isVsib`]: #method.is_vsib
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isVsib32")]
	pub fn is_vsib32(&self) -> bool {
		self.0.is_vsib32()
	}

	/// VSIB instructions only ([`isVsib`]): `true` if it's using 64-bit indexes, `false` if it's using 32-bit indexes
	///
	/// [`isVsib`]: #method.is_vsib
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isVsib64")]
	pub fn is_vsib64(&self) -> bool {
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

	/// Gets the suppress all exceptions flag (EVEX/MVEX encoded instructions). Note that if [`roundingControl`] is
	/// not [`RoundingControl.None`], SAE is implied but this method will still return `false`.
	///
	/// [`roundingControl`]: #method.rounding_control
	/// [`RoundingControl.None`]: enum.RoundingControl.html#variant.None
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "suppressAllExceptions")]
	pub fn suppress_all_exceptions(&self) -> bool {
		self.0.suppress_all_exceptions()
	}

	/// Sets the suppress all exceptions flag (EVEX/MVEX encoded instructions). Note that if [`roundingControl`] is
	/// not [`RoundingControl.None`], SAE is implied but this method will still return `false`.
	///
	/// [`roundingControl`]: #method.rounding_control
	/// [`RoundingControl.None`]: enum.RoundingControl.html#variant.None
	///
	/// # Arguments
	///
	/// * `newValue`: New value
	#[wasm_bindgen(setter)]
	#[cfg(feature = "encoder")]
	#[wasm_bindgen(js_name = "suppressAllExceptions")]
	pub fn set_suppress_all_exceptions(&mut self, #[allow(non_snake_case)] newValue: bool) {
		self.0.set_suppress_all_exceptions(newValue)
	}

	/// Checks if the memory operand is `RIP`/`EIP` relative
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isIpRelMemoryOperand")]
	pub fn is_ip_rel_memory_operand(&self) -> bool {
		self.0.is_ip_rel_memory_operand()
	}

	/// Gets the `RIP`/`EIP` releative address ([`memoryDisplacement`]).
	/// This method is only valid if there's a memory operand with `RIP`/`EIP` relative addressing, see [`isIpRelMemoryOperand`]
	///
	/// [`memoryDisplacement`]: #method.memory_displacement
	/// [`isIpRelMemoryOperand`]: #method.is_ip_rel_memory_operand
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "ipRelMemoryAddress")]
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
	/// const { Decoder, DecoderOptions } = require("iced-x86");
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

	/// Used if [`fpuWritesTop()`] is `true`:
	///
	/// Value added to `TOP`.
	///
	/// This is negative if it pushes one or more values and positive if it pops one or more values
	/// and `0` if it writes to `TOP` (eg. `FLDENV`, etc) without pushing/popping anything.
	///
	/// [`fpuWritesTop()`]: #method.fpu_writes_top
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "fpuTopIncrement")]
	pub fn fpu_top_increment(&self) -> i32 {
		self.0.fpu_stack_increment_info().increment()
	}

	/// `true` if it's a conditional push/pop (eg. `FPTAN` or `FSINCOS`)
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "fpuCondWritesTop")]
	pub fn fpu_cond_writes_top(&self) -> bool {
		self.0.fpu_stack_increment_info().conditional()
	}

	/// `true` if `TOP` is written (it's a conditional/unconditional push/pop, `FNSAVE`, `FLDENV`, etc)
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "fpuWritesTop")]
	pub fn fpu_writes_top(&self) -> bool {
		self.0.fpu_stack_increment_info().writes_top()
	}

	/// Instruction encoding (a [`EncodingKind`] enum value), eg. Legacy, 3DNow!, VEX, EVEX, XOP
	///
	/// [`EncodingKind`]: enum.EncodingKind.html
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Decoder, DecoderOptions, EncodingKind } = require("iced-x86");
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
	/// const { Decoder, DecoderOptions, CpuidFeature } = require("iced-x86");
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

	/// Control flow info (a [`FlowControl`] enum value)
	///
	/// [`FlowControl`]: enum.FlowControl.html
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Decoder, DecoderOptions, FlowControl } = require("iced-x86");
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

	/// `true` if it's a privileged instruction (all CPL=0 instructions (except `VMCALL`) and IOPL instructions `IN`, `INS`, `OUT`, `OUTS`, `CLI`, `STI`)
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
	/// const { Decoder, DecoderOptions } = require("iced-x86");
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

	/// `true` if it's a "string" instruction, such as `MOVS`, `LODS`, `SCAS`, etc.
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isStringInstruction")]
	pub fn is_string_instruction(&self) -> bool {
		self.0.is_string_instruction()
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
	/// const { Decoder, DecoderOptions, RflagsBits } = require("iced-x86");
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
	/// const { Decoder, DecoderOptions, RflagsBits } = require("iced-x86");
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
	/// const { Decoder, DecoderOptions, RflagsBits } = require("iced-x86");
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
	/// const { Decoder, DecoderOptions, RflagsBits } = require("iced-x86");
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
	/// const { Decoder, DecoderOptions, RflagsBits } = require("iced-x86");
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
	/// const { Decoder, DecoderOptions, RflagsBits } = require("iced-x86");
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

	/// Checks if it's a `JKccD SHORT` or `JKccD NEAR` instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isJkccShortOrNear")]
	#[cfg(feature = "mvex")]
	pub fn is_jkcc_short_or_near(&self) -> bool {
		self.0.is_jkcc_short_or_near()
	}

	/// Checks if it's a `JKccD NEAR` instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isJkccNear")]
	#[cfg(feature = "mvex")]
	pub fn is_jkcc_near(&self) -> bool {
		self.0.is_jkcc_near()
	}

	/// Checks if it's a `JKccD SHORT` instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isJkccShort")]
	#[cfg(feature = "mvex")]
	pub fn is_jkcc_short(&self) -> bool {
		self.0.is_jkcc_short()
	}

	/// Checks if it's a `JCXZ SHORT`, `JECXZ SHORT` or `JRCXZ SHORT` instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isJcxShort")]
	pub fn is_jcx_short(&self) -> bool {
		self.0.is_jcx_short()
	}

	/// Checks if it's a `LOOPcc SHORT` instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isLoopcc")]
	pub fn is_loopcc(&self) -> bool {
		self.0.is_loopcc()
	}

	/// Checks if it's a `LOOP SHORT` instruction
	#[wasm_bindgen(getter)]
	#[wasm_bindgen(js_name = "isLoop")]
	pub fn is_loop(&self) -> bool {
		self.0.is_loop()
	}

	/// Negates the condition code, eg. `JE` -> `JNE`. Can be used if it's `Jcc`, `SETcc`, `CMOVcc`, `CMPccXADD` and does
	/// nothing if the instruction doesn't have a condition code.
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { Code, ConditionCode, Decoder, DecoderOptions } = require("iced-x86");
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
	/// const { Code, Decoder, DecoderOptions } = require("iced-x86");
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
	/// const { Code, Decoder, DecoderOptions } = require("iced-x86");
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

	/// Gets the condition code (a [`ConditionCode`] enum value) if it's `Jcc`, `SETcc`, `CMOVcc`, `CMPccXADD` else [`ConditionCode.None`] is returned
	///
	/// [`ConditionCode`]: enum.ConditionCode.html
	/// [`ConditionCode.None`]: enum.ConditionCode.html#variant.None
	///
	/// # Examples
	///
	/// ```js
	/// const assert = require("assert").strict;
	/// const { ConditionCode, Decoder, DecoderOptions } = require("iced-x86");
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
	#[wasm_bindgen(js_name = "declareDataLength")]
	pub fn declare_data_len(&self) -> u32 {
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
	#[wasm_bindgen(js_name = "declareDataLength")]
	pub fn set_declare_data_len(&mut self, #[allow(non_snake_case)] newValue: u32) {
		self.0.set_declare_data_len(newValue as usize)
	}

	/// Sets a new `db` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareByte`]
	///
	/// [`declareDataLength()`]: #method.declare_data_length
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
	pub fn set_declare_byte_value_i8(&mut self, index: u32, #[allow(non_snake_case)] newValue: i8) -> Result<(), JsValue> {
		self.0.try_set_declare_byte_value(index as usize, newValue as u8).map_err(to_js_error)
	}

	/// Sets a new `db` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareByte`]
	///
	/// [`declareDataLength`]: #method.declare_data_length
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
	pub fn set_declare_byte_value(&mut self, index: u32, #[allow(non_snake_case)] newValue: u8) -> Result<(), JsValue> {
		self.0.try_set_declare_byte_value(index as usize, newValue).map_err(to_js_error)
	}

	/// Gets a `db` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareByte`]
	///
	/// [`declareDataLength`]: #method.declare_data_length
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
	pub fn get_declare_byte_value(&self, index: u32) -> Result<u8, JsValue> {
		self.0.try_get_declare_byte_value(index as usize).map_err(to_js_error)
	}

	/// Gets a `db` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareByte`]
	///
	/// [`declareDataLength`]: #method.declare_data_length
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
	#[wasm_bindgen(js_name = "getDeclareByteValueI8")]
	pub fn get_declare_byte_value_i8(&self, index: u32) -> Result<i8, JsValue> {
		Ok(self.0.try_get_declare_byte_value(index as usize).map_err(to_js_error)? as i8)
	}

	/// Sets a new `dw` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareWord`]
	///
	/// [`declareDataLength`]: #method.declare_data_length
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
	pub fn set_declare_word_value_i16(&mut self, index: u32, #[allow(non_snake_case)] newValue: i16) -> Result<(), JsValue> {
		self.0.try_set_declare_word_value_i16(index as usize, newValue).map_err(to_js_error)
	}

	/// Sets a new `dw` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareWord`]
	///
	/// [`declareDataLength`]: #method.declare_data_length
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
	pub fn set_declare_word_value(&mut self, index: u32, #[allow(non_snake_case)] newValue: u16) -> Result<(), JsValue> {
		self.0.try_set_declare_word_value(index as usize, newValue).map_err(to_js_error)
	}

	/// Gets a `dw` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareWord`]
	///
	/// [`declareDataLength`]: #method.declare_data_length
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
	pub fn get_declare_word_value(&self, index: u32) -> Result<u16, JsValue> {
		self.0.try_get_declare_word_value(index as usize).map_err(to_js_error)
	}

	/// Gets a `dw` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareWord`]
	///
	/// [`declareDataLength`]: #method.declare_data_length
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
	#[wasm_bindgen(js_name = "getDeclareWordValueI16")]
	pub fn get_declare_word_value_i16(&self, index: u32) -> Result<i16, JsValue> {
		Ok(self.0.try_get_declare_word_value(index as usize).map_err(to_js_error)? as i16)
	}

	/// Sets a new `dd` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareDword`]
	///
	/// [`declareDataLength`]: #method.declare_data_length
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
	pub fn set_declare_dword_value_i32(&mut self, index: u32, #[allow(non_snake_case)] newValue: i32) -> Result<(), JsValue> {
		self.0.try_set_declare_dword_value_i32(index as usize, newValue).map_err(to_js_error)
	}

	/// Sets a new `dd` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareDword`]
	///
	/// [`declareDataLength`]: #method.declare_data_length
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
	pub fn set_declare_dword_value(&mut self, index: u32, #[allow(non_snake_case)] newValue: u32) -> Result<(), JsValue> {
		self.0.try_set_declare_dword_value(index as usize, newValue).map_err(to_js_error)
	}

	/// Gets a `dd` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareDword`]
	///
	/// [`declareDataLength`]: #method.declare_data_length
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
	pub fn get_declare_dword_value(&self, index: u32) -> Result<u32, JsValue> {
		self.0.try_get_declare_dword_value(index as usize).map_err(to_js_error)
	}

	/// Gets a `dd` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareDword`]
	///
	/// [`declareDataLength`]: #method.declare_data_length
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
	#[wasm_bindgen(js_name = "getDeclareDwordValueI32")]
	pub fn get_declare_dword_value_i32(&self, index: u32) -> Result<i32, JsValue> {
		Ok(self.0.try_get_declare_dword_value(index as usize).map_err(to_js_error)? as i32)
	}

	/// Sets a new `dq` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareQword`]
	///
	/// [`declareDataLength`]: #method.declare_data_length
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
	pub fn set_declare_qword_value_i64(&mut self, index: u32, #[allow(non_snake_case)] newValue: i64) -> Result<(), JsValue> {
		self.0.try_set_declare_qword_value_i64(index as usize, newValue).map_err(to_js_error)
	}

	/// Sets a new `dq` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareQword`]
	///
	/// [`declareDataLength`]: #method.declare_data_length
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
	pub fn set_declare_qword_value(&mut self, index: u32, #[allow(non_snake_case)] newValue: u64) -> Result<(), JsValue> {
		self.0.try_set_declare_qword_value(index as usize, newValue).map_err(to_js_error)
	}

	/// Gets a `dq` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareQword`]
	///
	/// [`declareDataLength`]: #method.declare_data_length
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
	pub fn get_declare_qword_value(&self, index: u32) -> Result<u64, JsValue> {
		Ok(self.0.try_get_declare_qword_value(index as usize).map_err(to_js_error)?)
	}

	/// Gets a `dq` value, see also [`declareDataLength`].
	/// Can only be called if [`code`] is [`Code.DeclareQword`]
	///
	/// [`declareDataLength`]: #method.declare_data_length
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
	#[wasm_bindgen(js_name = "getDeclareQwordValueI64")]
	pub fn get_declare_qword_value_i64(&self, index: u32) -> Result<i64, JsValue> {
		Ok(self.0.try_get_declare_qword_value(index as usize).map_err(to_js_error)? as i64)
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
		Self(iced_x86_rust::Instruction::with(code_to_iced(code)))
	}

	/// Creates an instruction with 1 operand
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg(code: Code, register: Register) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with1(code_to_iced(code), register_to_iced(register)).map_err(to_js_error)?))
	}

	/// Creates an instruction with 1 operand
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `immediate`: op0: Immediate value
	///
	/// [`Code`]: enum.Code.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createI32")]
	pub fn with_i32(code: Code, immediate: i32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with1(code_to_iced(code), immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 1 operand
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `immediate`: op0: Immediate value
	///
	/// [`Code`]: enum.Code.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createU32")]
	pub fn with_u32(code: Code, immediate: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with1(code_to_iced(code), immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 1 operand
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `memory`: op0: Memory operand
	///
	/// [`Code`]: enum.Code.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createMem")]
	pub fn with_mem(code: Code, memory: MemoryOperand) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with1(code_to_iced(code), memory.0).map_err(to_js_error)?))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_reg(code: Code, register1: Register, register2: Register) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with2(code_to_iced(code), register_to_iced(register1), register_to_iced(register2)).map_err(to_js_error)?))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_i32(code: Code, register: Register, immediate: i32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with2(code_to_iced(code), register_to_iced(register), immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_u32(code: Code, register: Register, immediate: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with2(code_to_iced(code), register_to_iced(register), immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	#[wasm_bindgen(js_name = "createRegI64")]
	pub fn with_reg_i64(code: Code, register: Register, immediate: i64) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with2(code_to_iced(code), register_to_iced(register), immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	#[wasm_bindgen(js_name = "createRegU64")]
	pub fn with_reg_u64(code: Code, register: Register, immediate: u64) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with2(code_to_iced(code), register_to_iced(register), immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_mem(code: Code, register: Register, memory: MemoryOperand) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with2(code_to_iced(code), register_to_iced(register), memory.0).map_err(to_js_error)?))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_i32_reg(code: Code, immediate: i32, register: Register) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with2(code_to_iced(code), immediate, register_to_iced(register)).map_err(to_js_error)?))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_u32_reg(code: Code, immediate: u32, register: Register) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with2(code_to_iced(code), immediate, register_to_iced(register)).map_err(to_js_error)?))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_i32_i32(code: Code, immediate1: i32, immediate2: i32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with2(code_to_iced(code), immediate1, immediate2).map_err(to_js_error)?))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_u32_u32(code: Code, immediate1: u32, immediate2: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with2(code_to_iced(code), immediate1, immediate2).map_err(to_js_error)?))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_mem_reg(code: Code, memory: MemoryOperand, register: Register) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with2(code_to_iced(code), memory.0, register_to_iced(register)).map_err(to_js_error)?))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_mem_i32(code: Code, memory: MemoryOperand, immediate: i32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with2(code_to_iced(code), memory.0, immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 2 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_mem_u32(code: Code, memory: MemoryOperand, immediate: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with2(code_to_iced(code), memory.0, immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_reg_reg(code: Code, register1: Register, register2: Register, register3: Register) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with3(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), register_to_iced(register3)).map_err(to_js_error)?))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_reg_i32(code: Code, register1: Register, register2: Register, immediate: i32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with3(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_reg_u32(code: Code, register1: Register, register2: Register, immediate: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with3(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_reg_mem(code: Code, register1: Register, register2: Register, memory: MemoryOperand) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with3(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), memory.0).map_err(to_js_error)?))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_i32_i32(code: Code, register: Register, immediate1: i32, immediate2: i32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with3(code_to_iced(code), register_to_iced(register), immediate1, immediate2).map_err(to_js_error)?))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_u32_u32(code: Code, register: Register, immediate1: u32, immediate2: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with3(code_to_iced(code), register_to_iced(register), immediate1, immediate2).map_err(to_js_error)?))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_mem_reg(code: Code, register1: Register, memory: MemoryOperand, register2: Register) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with3(code_to_iced(code), register_to_iced(register1), memory.0, register_to_iced(register2)).map_err(to_js_error)?))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_mem_i32(code: Code, register: Register, memory: MemoryOperand, immediate: i32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with3(code_to_iced(code), register_to_iced(register), memory.0, immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_mem_u32(code: Code, register: Register, memory: MemoryOperand, immediate: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with3(code_to_iced(code), register_to_iced(register), memory.0, immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_mem_reg_reg(code: Code, memory: MemoryOperand, register1: Register, register2: Register) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with3(code_to_iced(code), memory.0, register_to_iced(register1), register_to_iced(register2)).map_err(to_js_error)?))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_mem_reg_i32(code: Code, memory: MemoryOperand, register: Register, immediate: i32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with3(code_to_iced(code), memory.0, register_to_iced(register), immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 3 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_mem_reg_u32(code: Code, memory: MemoryOperand, register: Register, immediate: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with3(code_to_iced(code), memory.0, register_to_iced(register), immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 4 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_reg_reg_reg(code: Code, register1: Register, register2: Register, register3: Register, register4: Register) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with4(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), register_to_iced(register3), register_to_iced(register4)).map_err(to_js_error)?))
	}

	/// Creates an instruction with 4 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_reg_reg_i32(code: Code, register1: Register, register2: Register, register3: Register, immediate: i32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with4(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), register_to_iced(register3), immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 4 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_reg_reg_u32(code: Code, register1: Register, register2: Register, register3: Register, immediate: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with4(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), register_to_iced(register3), immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 4 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_reg_reg_mem(code: Code, register1: Register, register2: Register, register3: Register, memory: MemoryOperand) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with4(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), register_to_iced(register3), memory.0).map_err(to_js_error)?))
	}

	/// Creates an instruction with 4 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_reg_i32_i32(code: Code, register1: Register, register2: Register, immediate1: i32, immediate2: i32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with4(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), immediate1, immediate2).map_err(to_js_error)?))
	}

	/// Creates an instruction with 4 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_reg_u32_u32(code: Code, register1: Register, register2: Register, immediate1: u32, immediate2: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with4(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), immediate1, immediate2).map_err(to_js_error)?))
	}

	/// Creates an instruction with 4 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_reg_mem_reg(code: Code, register1: Register, register2: Register, memory: MemoryOperand, register3: Register) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with4(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), memory.0, register_to_iced(register3)).map_err(to_js_error)?))
	}

	/// Creates an instruction with 4 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_reg_mem_i32(code: Code, register1: Register, register2: Register, memory: MemoryOperand, immediate: i32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with4(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), memory.0, immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 4 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_reg_mem_u32(code: Code, register1: Register, register2: Register, memory: MemoryOperand, immediate: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with4(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), memory.0, immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 5 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_reg_reg_reg_i32(code: Code, register1: Register, register2: Register, register3: Register, register4: Register, immediate: i32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with5(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), register_to_iced(register3), register_to_iced(register4), immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 5 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_reg_reg_reg_u32(code: Code, register1: Register, register2: Register, register3: Register, register4: Register, immediate: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with5(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), register_to_iced(register3), register_to_iced(register4), immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 5 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_reg_reg_mem_i32(code: Code, register1: Register, register2: Register, register3: Register, memory: MemoryOperand, immediate: i32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with5(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), register_to_iced(register3), memory.0, immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 5 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_reg_reg_mem_u32(code: Code, register1: Register, register2: Register, register3: Register, memory: MemoryOperand, immediate: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with5(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), register_to_iced(register3), memory.0, immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 5 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_reg_mem_reg_i32(code: Code, register1: Register, register2: Register, memory: MemoryOperand, register3: Register, immediate: i32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with5(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), memory.0, register_to_iced(register3), immediate).map_err(to_js_error)?))
	}

	/// Creates an instruction with 5 operands
	///
	/// # Throws
	///
	/// Throws if one of the operands is invalid (basic checks)
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
	pub fn with_reg_reg_mem_reg_u32(code: Code, register1: Register, register2: Register, memory: MemoryOperand, register3: Register, immediate: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with5(code_to_iced(code), register_to_iced(register1), register_to_iced(register2), memory.0, register_to_iced(register3), immediate).map_err(to_js_error)?))
	}

	/// Creates a new near/short branch instruction
	///
	/// # Throws
	///
	/// Throws if the created instruction doesn't have a near branch operand
	///
	/// # Arguments
	///
	/// * `code`: Code value (a [`Code`] enum value)
	/// * `target`: Target address
	///
	/// [`Code`]: enum.Code.html
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createBranch")]
	pub fn with_branch(code: Code, target: u64) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_branch(code_to_iced(code), target).map_err(to_js_error)?))
	}

	/// Creates a new far branch instruction
	///
	/// # Throws
	///
	/// Throws if the created instruction doesn't have a far branch operand
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
	pub fn with_far_branch(code: Code, selector: u16, offset: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_far_branch(code_to_iced(code), selector, offset).map_err(to_js_error)?))
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
	#[wasm_bindgen(js_name = "createXbegin")]
	pub fn with_xbegin(bitness: u32, target: u64) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_xbegin(bitness, target).map_err(to_js_error)?))
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
	pub fn with_outsb(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] segmentPrefix: Register, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_outsb(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_rep_outsb(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_rep_outsb(addressSize).map_err(to_js_error)?))
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
	pub fn with_outsw(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] segmentPrefix: Register, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_outsw(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_rep_outsw(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_rep_outsw(addressSize).map_err(to_js_error)?))
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
	pub fn with_outsd(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] segmentPrefix: Register, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_outsd(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_rep_outsd(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_rep_outsd(addressSize).map_err(to_js_error)?))
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
	pub fn with_lodsb(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] segmentPrefix: Register, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_lodsb(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_rep_lodsb(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_rep_lodsb(addressSize).map_err(to_js_error)?))
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
	pub fn with_lodsw(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] segmentPrefix: Register, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_lodsw(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_rep_lodsw(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_rep_lodsw(addressSize).map_err(to_js_error)?))
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
	pub fn with_lodsd(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] segmentPrefix: Register, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_lodsd(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_rep_lodsd(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_rep_lodsd(addressSize).map_err(to_js_error)?))
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
	pub fn with_lodsq(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] segmentPrefix: Register, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_lodsq(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_rep_lodsq(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_rep_lodsq(addressSize).map_err(to_js_error)?))
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
	pub fn with_scasb(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_scasb(addressSize, rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_repe_scasb(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_repe_scasb(addressSize).map_err(to_js_error)?))
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
	pub fn with_repne_scasb(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_repne_scasb(addressSize).map_err(to_js_error)?))
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
	pub fn with_scasw(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_scasw(addressSize, rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_repe_scasw(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_repe_scasw(addressSize).map_err(to_js_error)?))
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
	pub fn with_repne_scasw(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_repne_scasw(addressSize).map_err(to_js_error)?))
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
	pub fn with_scasd(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_scasd(addressSize, rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_repe_scasd(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_repe_scasd(addressSize).map_err(to_js_error)?))
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
	pub fn with_repne_scasd(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_repne_scasd(addressSize).map_err(to_js_error)?))
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
	pub fn with_scasq(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_scasq(addressSize, rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_repe_scasq(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_repe_scasq(addressSize).map_err(to_js_error)?))
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
	pub fn with_repne_scasq(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_repne_scasq(addressSize).map_err(to_js_error)?))
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
	pub fn with_insb(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_insb(addressSize, rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_rep_insb(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_rep_insb(addressSize).map_err(to_js_error)?))
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
	pub fn with_insw(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_insw(addressSize, rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_rep_insw(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_rep_insw(addressSize).map_err(to_js_error)?))
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
	pub fn with_insd(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_insd(addressSize, rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_rep_insd(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_rep_insd(addressSize).map_err(to_js_error)?))
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
	pub fn with_stosb(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_stosb(addressSize, rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_rep_stosb(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_rep_stosb(addressSize).map_err(to_js_error)?))
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
	pub fn with_stosw(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_stosw(addressSize, rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_rep_stosw(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_rep_stosw(addressSize).map_err(to_js_error)?))
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
	pub fn with_stosd(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_stosd(addressSize, rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_rep_stosd(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_rep_stosd(addressSize).map_err(to_js_error)?))
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
	pub fn with_stosq(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_stosq(addressSize, rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_rep_stosq(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_rep_stosq(addressSize).map_err(to_js_error)?))
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
	pub fn with_cmpsb(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] segmentPrefix: Register, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_cmpsb(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_repe_cmpsb(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_repe_cmpsb(addressSize).map_err(to_js_error)?))
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
	pub fn with_repne_cmpsb(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_repne_cmpsb(addressSize).map_err(to_js_error)?))
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
	pub fn with_cmpsw(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] segmentPrefix: Register, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_cmpsw(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_repe_cmpsw(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_repe_cmpsw(addressSize).map_err(to_js_error)?))
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
	pub fn with_repne_cmpsw(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_repne_cmpsw(addressSize).map_err(to_js_error)?))
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
	pub fn with_cmpsd(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] segmentPrefix: Register, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_cmpsd(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_repe_cmpsd(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_repe_cmpsd(addressSize).map_err(to_js_error)?))
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
	pub fn with_repne_cmpsd(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_repne_cmpsd(addressSize).map_err(to_js_error)?))
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
	pub fn with_cmpsq(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] segmentPrefix: Register, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_cmpsq(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_repe_cmpsq(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_repe_cmpsq(addressSize).map_err(to_js_error)?))
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
	pub fn with_repne_cmpsq(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_repne_cmpsq(addressSize).map_err(to_js_error)?))
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
	pub fn with_movsb(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] segmentPrefix: Register, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_movsb(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_rep_movsb(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_rep_movsb(addressSize).map_err(to_js_error)?))
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
	pub fn with_movsw(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] segmentPrefix: Register, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_movsw(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_rep_movsw(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_rep_movsw(addressSize).map_err(to_js_error)?))
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
	pub fn with_movsd(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] segmentPrefix: Register, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_movsd(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_rep_movsd(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_rep_movsd(addressSize).map_err(to_js_error)?))
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
	pub fn with_movsq(#[allow(non_snake_case)] addressSize: u32, #[allow(non_snake_case)] segmentPrefix: Register, #[allow(non_snake_case)] repPrefix: RepPrefixKind) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_movsq(addressSize, register_to_iced(segmentPrefix), rep_prefix_kind_to_iced(repPrefix)).map_err(to_js_error)?))
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
	pub fn with_rep_movsq(#[allow(non_snake_case)] addressSize: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_rep_movsq(addressSize).map_err(to_js_error)?))
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
	pub fn with_maskmovq(#[allow(non_snake_case)] addressSize: u32, register1: Register, register2: Register, #[allow(non_snake_case)] segmentPrefix: Register) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_maskmovq(addressSize, register_to_iced(register1), register_to_iced(register2), register_to_iced(segmentPrefix)).map_err(to_js_error)?))
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
	pub fn with_maskmovdqu(#[allow(non_snake_case)] addressSize: u32, register1: Register, register2: Register, #[allow(non_snake_case)] segmentPrefix: Register) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_maskmovdqu(addressSize, register_to_iced(register1), register_to_iced(register2), register_to_iced(segmentPrefix)).map_err(to_js_error)?))
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
	pub fn with_vmaskmovdqu(#[allow(non_snake_case)] addressSize: u32, register1: Register, register2: Register, #[allow(non_snake_case)] segmentPrefix: Register) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_vmaskmovdqu(addressSize, register_to_iced(register1), register_to_iced(register2), register_to_iced(segmentPrefix)).map_err(to_js_error)?))
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareByte_1")]
	pub fn with_declare_byte_1(b0: u8) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_byte_1(b0).map_err(to_js_error)?))
	}

	/// Creates a `db`/`.byte` asm directive
	///
	/// # Arguments
	///
	/// * `b0`: Byte 0
	/// * `b1`: Byte 1
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareByte_2")]
	pub fn with_declare_byte_2(b0: u8, b1: u8) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_byte_2(b0, b1).map_err(to_js_error)?))
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
	pub fn with_declare_byte_3(b0: u8, b1: u8, b2: u8) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_byte_3(b0, b1, b2).map_err(to_js_error)?))
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
	pub fn with_declare_byte_4(b0: u8, b1: u8, b2: u8, b3: u8) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_byte_4(b0, b1, b2, b3).map_err(to_js_error)?))
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
	pub fn with_declare_byte_5(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_byte_5(b0, b1, b2, b3, b4).map_err(to_js_error)?))
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
	pub fn with_declare_byte_6(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_byte_6(b0, b1, b2, b3, b4, b5).map_err(to_js_error)?))
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
	pub fn with_declare_byte_7(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_byte_7(b0, b1, b2, b3, b4, b5, b6).map_err(to_js_error)?))
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
	pub fn with_declare_byte_8(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_byte_8(b0, b1, b2, b3, b4, b5, b6, b7).map_err(to_js_error)?))
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
	pub fn with_declare_byte_9(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_byte_9(b0, b1, b2, b3, b4, b5, b6, b7, b8).map_err(to_js_error)?))
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
	pub fn with_declare_byte_10(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_byte_10(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9).map_err(to_js_error)?))
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
	pub fn with_declare_byte_11(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_byte_11(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10).map_err(to_js_error)?))
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
	pub fn with_declare_byte_12(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_byte_12(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11).map_err(to_js_error)?))
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
	pub fn with_declare_byte_13(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8, b12: u8) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_byte_13(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12).map_err(to_js_error)?))
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
	pub fn with_declare_byte_14(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8, b12: u8, b13: u8) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_byte_14(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13).map_err(to_js_error)?))
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
	pub fn with_declare_byte_15(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8, b12: u8, b13: u8, b14: u8) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_byte_15(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14).map_err(to_js_error)?))
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
	pub fn with_declare_byte_16(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8, b12: u8, b13: u8, b14: u8, b15: u8) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_byte_16(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14, b15).map_err(to_js_error)?))
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
	pub fn with_declare_byte(data: &[u8]) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_declare_byte(data).map_err(to_js_error)?))
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// # Arguments
	///
	/// * `w0`: Word 0
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareWord_1")]
	pub fn with_declare_word_1(w0: u16) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_word_1(w0).map_err(to_js_error)?))
	}

	/// Creates a `dw`/`.word` asm directive
	///
	/// # Arguments
	///
	/// * `w0`: Word 0
	/// * `w1`: Word 1
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareWord_2")]
	pub fn with_declare_word_2(w0: u16, w1: u16) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_word_2(w0, w1).map_err(to_js_error)?))
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
	pub fn with_declare_word_3(w0: u16, w1: u16, w2: u16) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_word_3(w0, w1, w2).map_err(to_js_error)?))
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
	pub fn with_declare_word_4(w0: u16, w1: u16, w2: u16, w3: u16) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_word_4(w0, w1, w2, w3).map_err(to_js_error)?))
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
	pub fn with_declare_word_5(w0: u16, w1: u16, w2: u16, w3: u16, w4: u16) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_word_5(w0, w1, w2, w3, w4).map_err(to_js_error)?))
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
	pub fn with_declare_word_6(w0: u16, w1: u16, w2: u16, w3: u16, w4: u16, w5: u16) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_word_6(w0, w1, w2, w3, w4, w5).map_err(to_js_error)?))
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
	pub fn with_declare_word_7(w0: u16, w1: u16, w2: u16, w3: u16, w4: u16, w5: u16, w6: u16) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_word_7(w0, w1, w2, w3, w4, w5, w6).map_err(to_js_error)?))
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
	pub fn with_declare_word_8(w0: u16, w1: u16, w2: u16, w3: u16, w4: u16, w5: u16, w6: u16, w7: u16) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_word_8(w0, w1, w2, w3, w4, w5, w6, w7).map_err(to_js_error)?))
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
	pub fn with_declare_word(data: &[u16]) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_declare_word(data).map_err(to_js_error)?))
	}

	/// Creates a `dd`/`.int` asm directive
	///
	/// # Arguments
	///
	/// * `d0`: Dword 0
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareDword_1")]
	pub fn with_declare_dword_1(d0: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_dword_1(d0).map_err(to_js_error)?))
	}

	/// Creates a `dd`/`.int` asm directive
	///
	/// # Arguments
	///
	/// * `d0`: Dword 0
	/// * `d1`: Dword 1
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareDword_2")]
	pub fn with_declare_dword_2(d0: u32, d1: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_dword_2(d0, d1).map_err(to_js_error)?))
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
	pub fn with_declare_dword_3(d0: u32, d1: u32, d2: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_dword_3(d0, d1, d2).map_err(to_js_error)?))
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
	pub fn with_declare_dword_4(d0: u32, d1: u32, d2: u32, d3: u32) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_dword_4(d0, d1, d2, d3).map_err(to_js_error)?))
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
	pub fn with_declare_dword(data: &[u32]) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_declare_dword(data).map_err(to_js_error)?))
	}

	/// Creates a `dq`/`.quad` asm directive
	///
	/// # Arguments
	///
	/// * `q0`: Qword 0
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareQword_1")]
	pub fn with_declare_qword_1(q0: u64) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_qword_1(q0).map_err(to_js_error)?))
	}

	/// Creates a `dq`/`.quad` asm directive
	///
	/// # Arguments
	///
	/// * `q0`: Qword 0
	/// * `q1`: Qword 1
	#[rustfmt::skip]
	#[wasm_bindgen(js_name = "createDeclareQword_2")]
	pub fn with_declare_qword_2(q0: u64, q1: u64) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::try_with_declare_qword_2(q0, q1).map_err(to_js_error)?))
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
	#[wasm_bindgen(js_name = "createDeclareQword")]
	pub fn with_declare_qword(data: &[u64]) -> Result<Instruction, JsValue> {
		Ok(Self(iced_x86_rust::Instruction::with_declare_qword(data).map_err(to_js_error)?))
	}
	// GENERATOR-END: Create
}
