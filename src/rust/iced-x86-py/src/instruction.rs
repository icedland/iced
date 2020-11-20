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


#![allow(clippy::unit_arg)]

use core::hash::{Hash, Hasher};
use pyo3::class::basic::CompareOp;
use pyo3::class::PySequenceProtocol;
use pyo3::exceptions::PyValueError;
use pyo3::prelude::*;
use pyo3::PyObjectProtocol;
use std::collections::hash_map::DefaultHasher;

fn to_register(value: u32) -> PyResult<iced_x86::Register> {
	//TODO: verify input value
	Ok(unsafe { core::mem::transmute(value as u8) })
}

fn to_rounding_control(value: u32) -> PyResult<iced_x86::RoundingControl> {
	//TODO: verify input value
	Ok(unsafe { core::mem::transmute(value as u8) })
}

fn to_code_size(value: u32) -> PyResult<iced_x86::CodeSize> {
	//TODO: verify input value
	Ok(unsafe { core::mem::transmute(value as u8) })
}

fn to_code(value: u32) -> PyResult<iced_x86::Code> {
	//TODO: verify input value
	Ok(unsafe { core::mem::transmute(value as u16) })
}

fn to_op_kind(value: u32) -> PyResult<iced_x86::OpKind> {
	//TODO: verify input value
	Ok(unsafe { core::mem::transmute(value as u8) })
}

/// A 16/32/64-bit x86 instruction. Created by `Decoder` or by `Instruction::create*()` methods.
#[pyclass(module = "iced_x86_py")]
#[text_signature = "(/)"]
#[derive(Copy, Clone)]
pub struct Instruction {
	pub(crate) instr: iced_x86::Instruction,
}

#[pymethods]
impl Instruction {
	/// Creates an empty `Instruction` (all fields are cleared).
	#[new]
	pub(crate) fn new() -> Self {
		Self { instr: iced_x86::Instruction::default() }
	}

	/// Returns a copy of this instance.
	///
	/// This is identical to `clone()`
	#[text_signature = "($self, /)"]
	fn __copy__(&self) -> Self {
		*self
	}

	/// Returns a copy of this instance.
	///
	/// This is identical to `clone()`
	#[text_signature = "($self, memo, /)"]
	fn __deepcopy__(&self, _memo: &PyAny) -> Self {
		*self
	}

	/// Returns a copy of this instance.
	#[text_signature = "($self, /)"]
	fn clone(&self) -> Self {
		*self
	}

	/// Checks if two instructions are equal, comparing all bits, not ignoring anything. `==` ignores some fields.
	#[text_signature = "($self, other, /)"]
	fn eq_all_bits(&self, other: &Self) -> bool {
		self.instr.eq_all_bits(&other.instr)
	}

	/// Gets the 16-bit IP of the instruction
	#[getter]
	fn ip16(&self) -> u16 {
		self.instr.ip16()
	}

	#[setter]
	fn set_ip16(&mut self, new_value: u16) {
		self.instr.set_ip16(new_value)
	}

	/// Gets the 32-bit IP of the instruction
	#[getter]
	fn ip32(&self) -> u32 {
		self.instr.ip32()
	}

	#[setter]
	fn set_ip32(&mut self, new_value: u32) {
		self.instr.set_ip32(new_value)
	}

	/// Gets the 64-bit IP of the instruction
	#[getter]
	fn ip(&self) -> u64 {
		self.instr.ip()
	}

	#[setter]
	fn set_ip(&mut self, new_value: u64) {
		self.instr.set_ip(new_value)
	}

	/// Gets the 16-bit IP of the next instruction
	#[getter]
	fn next_ip16(&self) -> u16 {
		self.instr.next_ip16()
	}

	#[setter]
	fn set_next_ip16(&mut self, new_value: u16) {
		self.instr.set_next_ip16(new_value)
	}

	/// Gets the 32-bit IP of the next instruction
	#[getter]
	fn next_ip32(&self) -> u32 {
		self.instr.next_ip32()
	}

	#[setter]
	fn set_next_ip32(&mut self, new_value: u32) {
		self.instr.set_next_ip32(new_value)
	}

	/// Gets the 64-bit IP of the next instruction
	#[getter]
	fn next_ip(&self) -> u64 {
		self.instr.next_ip()
	}

	#[setter]
	fn set_next_ip(&mut self, new_value: u64) {
		self.instr.set_next_ip(new_value)
	}

	/// Gets the code size (a `CodeSize` enum value) when the instruction was decoded.
	///
	/// Note:
	/// 	This value is informational and can be used by a formatter.
	#[getter]
	fn code_size(&self) -> u32 {
		self.instr.code_size() as u32
	}

	#[setter]
	fn set_code_size(&mut self, new_value: u32) -> PyResult<()> {
		Ok(self.instr.set_code_size(to_code_size(new_value)?))
	}

	/// Checks if it's an invalid instruction (`code` == `Code.INVALID`)
	#[getter]
	fn is_invalid(&self) -> bool {
		self.instr.is_invalid()
	}

	/// Gets the instruction code (a `Code` enum value), see also `mnemonic`
	#[getter]
	fn code(&self) -> u32 {
		self.instr.code() as u32
	}

	#[setter]
	fn set_code(&mut self, new_value: u32) -> PyResult<()> {
		Ok(self.instr.set_code(to_code(new_value)?))
	}

	/// Gets the mnemonic (a `Mnemonic` enum value), see also `code`
	#[getter]
	fn mnemonic(&self) -> u32 {
		self.instr.mnemonic() as u32
	}

	/// Gets the operand count. An instruction can have 0-5 operands.
	///
	/// Examples:
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
	#[getter]
	fn op_count(&self) -> u32 {
		self.instr.op_count()
	}

	/// Gets the length of the instruction, 0-15 bytes.
	///
	/// Note:
	/// 	This is just informational. If you modify the instruction or create a new one,
	///			this method could return the wrong value.
	#[getter]
	fn len(&self) -> usize {
		self.instr.len()
	}

	#[setter]
	fn set_len(&mut self, new_value: usize) {
		self.instr.set_len(new_value)
	}

	/// `True` if the instruction has the `XACQUIRE` prefix (`F2`)
	#[getter]
	fn has_xacquire_prefix(&self) -> bool {
		self.instr.has_xacquire_prefix()
	}

	#[setter]
	fn set_has_xacquire_prefix(&mut self, new_value: bool) {
		self.instr.set_has_xacquire_prefix(new_value)
	}

	/// `True` if the instruction has the `XRELEASE` prefix (`F3`)
	#[getter]
	fn has_xrelease_prefix(&self) -> bool {
		self.instr.has_xrelease_prefix()
	}

	#[setter]
	fn set_has_xrelease_prefix(&mut self, new_value: bool) {
		self.instr.set_has_xrelease_prefix(new_value)
	}

	/// `True` if the instruction has the `REPE` or `REP` prefix (`F3`)
	#[getter]
	fn has_rep_prefix(&self) -> bool {
		self.instr.has_rep_prefix()
	}

	#[setter]
	fn set_has_rep_prefix(&mut self, new_value: bool) {
		self.instr.set_has_rep_prefix(new_value)
	}

	/// `True` if the instruction has the `REPE` or `REP` prefix (`F3`)
	#[getter]
	fn has_repe_prefix(&self) -> bool {
		self.instr.has_repe_prefix()
	}

	#[setter]
	fn set_has_repe_prefix(&mut self, new_value: bool) {
		self.instr.set_has_repe_prefix(new_value)
	}

	/// `True` if the instruction has the `REPNE` prefix (`F2`)
	#[getter]
	fn has_repne_prefix(&self) -> bool {
		self.instr.has_repne_prefix()
	}

	#[setter]
	fn set_has_repne_prefix(&mut self, new_value: bool) {
		self.instr.set_has_repne_prefix(new_value)
	}

	/// `True` if the instruction has the `LOCK` prefix (`F0`)
	#[getter]
	fn has_lock_prefix(&self) -> bool {
		self.instr.has_lock_prefix()
	}

	#[setter]
	fn set_has_lock_prefix(&mut self, new_value: bool) {
		self.instr.set_has_lock_prefix(new_value)
	}

	/// Gets operand #0's kind (an `OpKind` enum value) if the operand exists (see `op_count` and `op_kind(int)`)
	#[getter]
	fn op0_kind(&self) -> u32 {
		self.instr.op0_kind() as u32
	}

	#[setter]
	fn set_op0_kind(&mut self, new_value: u32) -> PyResult<()> {
		Ok(self.instr.set_op0_kind(to_op_kind(new_value)?))
	}

	/// Gets operand #1's kind (an `OpKind` enum value) if the operand exists (see `op_count` and `op_kind(int)`)
	#[getter]
	fn op1_kind(&self) -> u32 {
		self.instr.op1_kind() as u32
	}

	#[setter]
	fn set_op1_kind(&mut self, new_value: u32) -> PyResult<()> {
		Ok(self.instr.set_op1_kind(to_op_kind(new_value)?))
	}

	/// Gets operand #2's kind (an `OpKind` enum value) if the operand exists (see `op_count` and `op_kind(int)`)
	#[getter]
	fn op2_kind(&self) -> u32 {
		self.instr.op2_kind() as u32
	}

	#[setter]
	fn set_op2_kind(&mut self, new_value: u32) -> PyResult<()> {
		Ok(self.instr.set_op2_kind(to_op_kind(new_value)?))
	}

	/// Gets operand #3's kind (an `OpKind` enum value) if the operand exists (see `op_count` and `op_kind(int)`)
	#[getter]
	fn op3_kind(&self) -> u32 {
		self.instr.op3_kind() as u32
	}

	#[setter]
	fn set_op3_kind(&mut self, new_value: u32) -> PyResult<()> {
		Ok(self.instr.set_op3_kind(to_op_kind(new_value)?))
	}

	/// Gets operand #4's kind (an `OpKind` enum value) if the operand exists (see `op_count` and `op_kind(int)`)
	#[getter]
	fn op4_kind(&self) -> u32 {
		self.instr.op4_kind() as u32
	}

	#[setter]
	fn set_op4_kind(&mut self, new_value: u32) -> PyResult<()> {
		if new_value != iced_x86::OpKind::Immediate8 as u32 {
			Err(PyValueError::new_err("Invalid op kind"))
		} else {
			Ok(self.instr.set_op4_kind(to_op_kind(new_value)?))
		}
	}

	/// Gets an operand's kind (an `OpKind` enum value) if it exists (see `op_count`)
	///
	/// Args:
	/// 	`operand`: Operand number, 0-4
	///
	/// Raises:
	/// 	Panics if `operand` is invalid
	///
	/// Examples:
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
	#[text_signature = "($self, operand, /)"]
	fn op_kind(&self, operand: u32) -> PyResult<u32> {
		if operand < 5 {
			Ok(self.instr.op_kind(operand) as u32)
		} else {
			Err(PyValueError::new_err("Invalid operand"))
		}
	}

	/// Sets an operand's kind
	///
	/// Args:
	/// 	`operand`: Operand number, 0-4
	/// 	`op_kind`: Operand kind
	///
	/// Raises:
	/// 	Panics if `operand` is invalid
	#[text_signature = "($self, operand, op_kind, /)"]
	fn set_op_kind(&mut self, operand: u32, op_kind: u32) -> PyResult<()> {
		if operand < 5 {
			Ok(self.instr.set_op_kind(operand, to_op_kind(op_kind)?))
		} else {
			Err(PyValueError::new_err("Invalid operand"))
		}
	}

	/// Checks if the instruction has a segment override prefix, see `segment_prefix`
	#[getter]
	fn has_segment_prefix(&self) -> bool {
		self.instr.has_segment_prefix()
	}

	/// Gets the segment override prefix (a `Register` enum value) or `Register.None` if none.
	///
	/// See also `memory_segment`.
	///
	/// Use this method if the operand has kind `OpKind.Memory`, `OpKind.Memory64`,
	/// `OpKind.MemorySegSI`, `OpKind.MemorySegESI`, `OpKind.MemorySegRSI`
	#[getter]
	fn segment_prefix(&self) -> u32 {
		self.instr.segment_prefix() as u32
	}

	#[setter]
	fn set_segment_prefix(&mut self, new_value: u32) -> PyResult<()> {
		Ok(self.instr.set_segment_prefix(to_register(new_value)?))
	}

	/// Gets the effective segment register used to reference the memory location (a `Register` enum value).
	///
	/// Use this method if the operand has kind `OpKind.Memory`, `OpKind.Memory64`,
	/// `OpKind.MemorySegSI`, `OpKind.MemorySegESI`, `OpKind.MemorySegRSI`
	#[getter]
	fn memory_segment(&self) -> u32 {
		self.instr.memory_segment() as u32
	}

	/// Gets the size of the memory displacement in bytes.
	///
	/// Valid values are `0`, `1` (16/32/64-bit), `2` (16-bit), `4` (32-bit), `8` (64-bit).
	///
	/// Note that the return value can be 1 and `memory_displacement` may still not fit in
	/// a signed byte if it's an EVEX encoded instruction.
	///
	/// Use this method if the operand has kind `OpKind.Memory`
	#[getter]
	fn memory_displ_size(&self) -> u32 {
		self.instr.memory_displ_size()
	}

	#[setter]
	fn set_memory_displ_size(&mut self, new_value: u32) {
		self.instr.set_memory_displ_size(new_value)
	}

	/// `True` if the data is broadcasted (EVEX instructions only)
	#[getter]
	fn is_broadcast(&self) -> bool {
		self.instr.is_broadcast()
	}

	#[setter]
	fn set_is_broadcast(&mut self, new_value: bool) {
		self.instr.set_is_broadcast(new_value)
	}

	/// Gets the size of the memory location (a `MemorySize` enum value) that is referenced by the operand.
	///
	/// See also `is_broadcast`.
	///
	/// Use this method if the operand has kind `OpKind.Memory`, `OpKind.Memory64`,
	/// `OpKind.MemorySegSI`, `OpKind.MemorySegESI`, `OpKind.MemorySegRSI`,
	/// `OpKind.MemoryESDI`, `OpKind.MemoryESEDI`, `OpKind.MemoryESRDI`
	#[getter]
	fn memory_size(&self) -> u32 {
		self.instr.memory_size() as u32
	}

	/// Gets the index register scale value, valid values are `*1`, `*2`, `*4`, `*8`.
	///
	/// Use this method if the operand has kind `OpKind.Memory`
	#[getter]
	fn memory_index_scale(&self) -> u32 {
		self.instr.memory_index_scale()
	}

	#[setter]
	fn set_memory_index_scale(&mut self, new_value: u32) {
		self.instr.set_memory_index_scale(new_value)
	}

	/// Gets the memory operand's displacement.
	///
	/// This should be sign extended to 64 bits if it's 64-bit addressing (see `memory_displacement64`).
	///
	/// Use this method if the operand has kind `OpKind.Memory`
	#[getter]
	fn memory_displacement(&self) -> u32 {
		self.instr.memory_displacement()
	}

	#[setter]
	fn set_memory_displacement(&mut self, new_value: u32) {
		self.instr.set_memory_displacement(new_value)
	}

	/// Gets the memory operand's displacement sign extended to 64 bits.
	///
	/// Use this method if the operand has kind `OpKind.Memory`
	#[getter]
	fn memory_displacement64(&self) -> u64 {
		self.instr.memory_displacement64()
	}

	/// Gets an operand's immediate value
	///
	/// Args:
	/// 	`operand`: Operand number, 0-4
	///
	/// Raises:
	/// 	Panics if `operand` is invalid or not immediate.
	#[text_signature = "($self, operand, /)"]
	fn immediate(&self, operand: u32) -> u64 {
		self.instr.immediate(operand)
	}

	/// Sets an operand's immediate value
	///
	/// Args:
	/// 	`operand`: Operand number, 0-4
	/// 	`new_value`: Immediate
	///
	/// Raises:
	/// 	Panics if `operand` is invalid or if it's not an immediate operand
	#[text_signature = "($self, operand, new_value, /)"]
	fn set_immediate_i32(&mut self, operand: u32, new_value: i32) {
		self.instr.set_immediate_i32(operand, new_value);
	}

	/// Sets an operand's immediate value
	///
	/// Args:
	/// 	`operand`: Operand number, 0-4
	/// 	`new_value`: Immediate
	///
	/// Raises:
	/// 	Panics if `operand` is invalid or if it's not an immediate operand
	#[text_signature = "($self, operand, new_value, /)"]
	fn set_immediate_u32(&mut self, operand: u32, new_value: u32) {
		self.instr.set_immediate_u32(operand, new_value);
	}

	/// Sets an operand's immediate value
	///
	/// Args:
	/// 	`operand`: Operand number, 0-4
	/// 	`new_value`: Immediate
	///
	/// Raises:
	/// 	Panics if `operand` is invalid or if it's not an immediate operand
	#[text_signature = "($self, operand, new_value, /)"]
	fn set_immediate_i64(&mut self, operand: u32, new_value: i64) {
		self.instr.set_immediate_i64(operand, new_value);
	}

	/// Sets an operand's immediate value
	///
	/// Args:
	/// 	`operand`: Operand number, 0-4
	/// 	`new_value`: Immediate
	///
	/// Raises:
	/// 	Panics if `operand` is invalid or if it's not an immediate operand
	#[text_signature = "($self, operand, new_value, /)"]
	fn set_immediate_u64(&mut self, operand: u32, new_value: u64) {
		self.instr.set_immediate_u64(operand, new_value)
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate8`
	#[getter]
	fn immediate8(&self) -> u8 {
		self.instr.immediate8()
	}

	#[setter]
	fn set_immediate8(&mut self, new_value: u8) {
		self.instr.set_immediate8(new_value)
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate8_2nd`
	#[getter]
	fn immediate8_2nd(&self) -> u8 {
		self.instr.immediate8_2nd()
	}

	#[setter]
	fn set_immediate8_2nd(&mut self, new_value: u8) {
		self.instr.set_immediate8_2nd(new_value)
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate16`
	#[getter]
	fn immediate16(&self) -> u16 {
		self.instr.immediate16()
	}

	#[setter]
	fn set_immediate16(&mut self, new_value: u16) {
		self.instr.set_immediate16(new_value);
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate32`
	#[getter]
	fn immediate32(&self) -> u32 {
		self.instr.immediate32()
	}

	#[setter]
	fn set_immediate32(&mut self, new_value: u32) {
		self.instr.set_immediate32(new_value);
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate64`
	#[getter]
	fn immediate64(&self) -> u64 {
		self.instr.immediate64()
	}

	#[setter]
	fn set_immediate64(&mut self, new_value: u64) {
		self.instr.set_immediate64(new_value);
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate8to16`
	#[getter]
	fn immediate8to16(&self) -> i16 {
		self.instr.immediate8to16()
	}

	#[setter]
	fn set_immediate8to16(&mut self, new_value: i16) {
		self.instr.set_immediate8to16(new_value)
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate8to32`
	#[getter]
	fn immediate8to32(&self) -> i32 {
		self.instr.immediate8to32()
	}

	#[setter]
	fn set_immediate8to32(&mut self, new_value: i32) {
		self.instr.set_immediate8to32(new_value)
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate8to64`
	#[getter]
	fn immediate8to64(&self) -> i64 {
		self.instr.immediate8to64()
	}

	#[setter]
	fn set_immediate8to64(&mut self, new_value: i64) {
		self.instr.set_immediate8to64(new_value);
	}

	/// Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind `OpKind.Immediate32to64`
	#[getter]
	fn immediate32to64(&self) -> i64 {
		self.instr.immediate32to64()
	}

	#[setter]
	fn set_immediate32to64(&mut self, new_value: i64) {
		self.instr.set_immediate32to64(new_value);
	}

	/// Gets the operand's 64-bit address value.
	///
	/// Use this method if the operand has kind `OpKind.Memory64`
	#[getter]
	fn memory_address64(&self) -> u64 {
		self.instr.memory_address64()
	}

	#[setter]
	fn set_memory_address64(&mut self, new_value: u64) {
		self.instr.set_memory_address64(new_value);
	}

	/// Gets the operand's branch target.
	///
	/// Use this method if the operand has kind `OpKind.NearBranch16`
	#[getter]
	fn near_branch16(&self) -> u16 {
		self.instr.near_branch16()
	}

	#[setter]
	fn set_near_branch16(&mut self, new_value: u16) {
		self.instr.set_near_branch16(new_value);
	}

	/// Gets the operand's branch target.
	///
	/// Use this method if the operand has kind `OpKind.NearBranch32`
	#[getter]
	fn near_branch32(&self) -> u32 {
		self.instr.near_branch32()
	}

	#[setter]
	fn set_near_branch32(&mut self, new_value: u32) {
		self.instr.set_near_branch32(new_value);
	}

	/// Gets the operand's branch target.
	///
	/// Use this method if the operand has kind `OpKind.NearBranch64`
	#[getter]
	fn near_branch64(&self) -> u64 {
		self.instr.near_branch64()
	}

	#[setter]
	fn set_near_branch64(&mut self, new_value: u64) {
		self.instr.set_near_branch64(new_value);
	}

	/// Gets the near branch target if it's a `CALL`/`JMP`/`Jcc` near branch instruction
	///
	/// (i.e., if `op0_kind` is `OpKind.NearBranch16`, `OpKind.NearBranch32` or `OpKind.NearBranch64`)
	#[getter]
	fn near_branch_target(&self) -> u64 {
		self.instr.near_branch_target()
	}

	/// Gets the operand's branch target.
	///
	/// Use this method if the operand has kind `OpKind.FarBranch16`
	#[getter]
	fn far_branch16(&self) -> u16 {
		self.instr.far_branch16()
	}

	#[setter]
	fn set_far_branch16(&mut self, new_value: u16) {
		self.instr.set_far_branch16(new_value);
	}

	/// Gets the operand's branch target.
	///
	/// Use this method if the operand has kind `OpKind.FarBranch32`
	#[getter]
	fn far_branch32(&self) -> u32 {
		self.instr.far_branch32()
	}

	#[setter]
	fn set_far_branch32(&mut self, new_value: u32) {
		self.instr.set_far_branch32(new_value);
	}

	/// Gets the operand's branch target selector.
	///
	/// Use this method if the operand has kind `OpKind.FarBranch16` or `OpKind.FarBranch32`
	#[getter]
	fn far_branch_selector(&self) -> u16 {
		self.instr.far_branch_selector()
	}

	#[setter]
	fn set_far_branch_selector(&mut self, new_value: u16) {
		self.instr.set_far_branch_selector(new_value);
	}

	/// Gets the memory operand's base register (a `Register` enum value) or `Register.None` if none.
	///
	/// Use this method if the operand has kind `OpKind.Memory`
	#[getter]
	fn memory_base(&self) -> u32 {
		self.instr.memory_base() as u32
	}

	#[setter]
	fn set_memory_base(&mut self, new_value: u32) -> PyResult<()> {
		Ok(self.instr.set_memory_base(to_register(new_value)?))
	}

	/// Gets the memory operand's index register (a `Register` enum value) or `Register.None` if none.
	///
	/// Use this method if the operand has kind `OpKind.Memory`
	#[getter]
	fn memory_index(&self) -> u32 {
		self.instr.memory_index() as u32
	}

	#[setter]
	fn set_memory_index(&mut self, new_value: u32) -> PyResult<()> {
		Ok(self.instr.set_memory_index(to_register(new_value)?))
	}

	/// Gets operand #0's register value (a `Register` enum value).
	///
	/// Use this method if operand #0 (`op0_kind`) has kind `OpKind.Register`, see `op_count` and `op_register`
	#[getter]
	fn op0_register(&self) -> u32 {
		self.instr.op0_register() as u32
	}

	#[setter]
	fn set_op0_register(&mut self, new_value: u32) -> PyResult<()> {
		Ok(self.instr.set_op0_register(to_register(new_value)?))
	}

	/// Gets operand #1's register value (a `Register` enum value).
	///
	/// Use this method if operand #1 (`op0_kind`) has kind `OpKind.Register`, see `op_count` and `op_register`
	#[getter]
	fn op1_register(&self) -> u32 {
		self.instr.op1_register() as u32
	}

	#[setter]
	fn set_op1_register(&mut self, new_value: u32) -> PyResult<()> {
		Ok(self.instr.set_op1_register(to_register(new_value)?))
	}

	/// Gets operand #2's register value (a `Register` enum value).
	///
	/// Use this method if operand #2 (`op0_kind`) has kind `OpKind.Register`, see `op_count` and `op_register`
	#[getter]
	fn op2_register(&self) -> u32 {
		self.instr.op2_register() as u32
	}

	#[setter]
	fn set_op2_register(&mut self, new_value: u32) -> PyResult<()> {
		Ok(self.instr.set_op2_register(to_register(new_value)?))
	}

	/// Gets operand #3's register value (a `Register` enum value).
	///
	/// Use this method if operand #3 (`op0_kind`) has kind `OpKind.Register`, see `op_count` and `op_register`
	#[getter]
	fn op3_register(&self) -> u32 {
		self.instr.op3_register() as u32
	}

	#[setter]
	fn set_op3_register(&mut self, new_value: u32) -> PyResult<()> {
		Ok(self.instr.set_op3_register(to_register(new_value)?))
	}

	/// Gets operand #4's register value (a `Register` enum value).
	///
	/// Use this method if operand #4 (`op0_kind`) has kind `OpKind.Register`, see `op_count` and `op_register`
	#[getter]
	fn op4_register(&self) -> u32 {
		self.instr.op4_register() as u32
	}

	#[setter]
	fn set_op4_register(&mut self, new_value: u32) -> PyResult<()> {
		if new_value != iced_x86::Register::None as u32 {
			Err(PyValueError::new_err("Invalid register"))
		} else {
			Ok(self.instr.set_op4_register(to_register(new_value)?))
		}
	}

	/// Gets the operand's register value (a `Register` enum value).
	///
	/// Use this method if the operand has kind `OpKind.Register`
	///
	/// Args:
	/// 	`operand`: Operand number, 0-4
	///
	/// Raises:
	/// 	Panics if `operand` is invalid
	///
	/// Examples:
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
	#[text_signature = "($self, operand, /)"]
	fn op_register(&self, operand: u32) -> PyResult<u32> {
		if operand < 5 {
			Ok(self.instr.op_register(operand) as u32)
		} else {
			Err(PyValueError::new_err("Invalid operand"))
		}
	}

	/// Sets the operand's register value.
	///
	/// Use this method if the operand has kind `OpKind.Register`
	///
	/// Args:
	/// 	`operand`: Operand number, 0-4
	/// 	`new_value`: New value
	///
	/// Raises:
	/// 	Panics if `operand` is invalid
	#[text_signature = "($self, operand, new_value, /)"]
	fn set_op_register(&mut self, operand: u32, new_value: u32) -> PyResult<()> {
		if operand < 5 {
			Ok(self.instr.set_op_register(operand, to_register(new_value)?))
		} else {
			Err(PyValueError::new_err("Invalid operand"))
		}
	}

	/// Gets the op mask register (`Register.K1` - `Register.K7`) or `Register.None` if none (a `Register` enum value)
	#[getter]
	fn op_mask(&self) -> u32 {
		self.instr.op_mask() as u32
	}

	#[setter]
	fn set_op_mask(&mut self, new_value: u32) -> PyResult<()> {
		Ok(self.instr.set_op_mask(to_register(new_value)?))
	}

	/// Checks if there's an op mask register (`op_mask`)
	#[getter]
	fn has_op_mask(&self) -> bool {
		self.instr.has_op_mask()
	}

	/// `True` if zeroing-masking, `False` if merging-masking.
	///
	/// Only used by most EVEX encoded instructions that use op mask registers.
	#[getter]
	fn zeroing_masking(&self) -> bool {
		self.instr.zeroing_masking()
	}

	#[setter]
	fn set_zeroing_masking(&mut self, new_value: bool) {
		self.instr.set_zeroing_masking(new_value);
	}

	/// `True` if merging-masking, `False` if zeroing-masking.
	///
	/// Only used by most EVEX encoded instructions that use op mask registers.
	#[getter]
	fn merging_masking(&self) -> bool {
		self.instr.merging_masking()
	}

	#[setter]
	fn set_merging_masking(&mut self, new_value: bool) {
		self.instr.set_merging_masking(new_value);
	}

	/// Gets the rounding control (a `RoundingControl` enum value) or `RoundingControl.None` if the instruction doesn't use it.
	///
	/// Note:
	/// 	`suppress_all_exceptions` is implied but still returns `False`.
	#[getter]
	fn rounding_control(&self) -> u32 {
		self.instr.rounding_control() as u32
	}

	#[setter]
	fn set_rounding_control(&mut self, new_value: u32) -> PyResult<()> {
		Ok(self.instr.set_rounding_control(to_rounding_control(new_value)?))
	}

	/// Gets the number of elements in a `db`/`dw`/`dd`/`dq` directive.
	///
	/// Can only be called if `code` is `Code.DeclareByte`, `Code.DeclareWord`, `Code.DeclareDword`, `Code.DeclareQword`
	#[getter]
	fn declare_data_len(&self) -> usize {
		self.instr.declare_data_len()
	}

	#[setter]
	fn set_declare_data_len(&mut self, new_value: usize) {
		self.instr.set_declare_data_len(new_value);
	}

	/// Sets a new `db` value, see also `declare_data_len`.
	///
	/// Can only be called if `code` is `Code.DeclareByte`
	///
	/// Args:
	/// 	`index`: Index (0-15)
	/// 	`new_value`: New value
	///
	/// Raises:
	/// 	- Panics if `index` is invalid
	/// 	- Panics if `db` feature wasn't enabled
	#[text_signature = "($self, index, new_value, /)"]
	fn set_declare_byte_value_i8(&mut self, index: usize, new_value: i8) {
		self.instr.set_declare_byte_value_i8(index, new_value);
	}

	/// Sets a new `db` value, see also `declare_data_len`.
	///
	/// Can only be called if `code` is `Code.DeclareByte`
	///
	/// Args:
	/// 	`index`: Index (0-15)
	/// 	`new_value`: New value
	///
	/// Raises:
	/// 	- Panics if `index` is invalid
	/// 	- Panics if `db` feature wasn't enabled
	#[text_signature = "($self, index, new_value, /)"]
	fn set_declare_byte_value(&mut self, index: usize, new_value: u8) {
		self.instr.set_declare_byte_value(index, new_value);
	}

	/// Gets a `db` value, see also `declare_data_len`.
	///
	/// Can only be called if `code` is `Code.DeclareByte`
	///
	/// Args:
	/// 	`index`: Index (0-15)
	///
	/// Raises:
	/// 	Panics if `index` is invalid
	#[text_signature = "($self, index, /)"]
	fn get_declare_byte_value(&self, index: usize) -> u8 {
		self.instr.get_declare_byte_value(index)
	}

	/// Sets a new `dw` value, see also `declare_data_len`.
	///
	/// Can only be called if `code` is `Code.DeclareWord`
	///
	/// Args:
	/// 	`index`: Index (0-7)
	/// 	`new_value`: New value
	///
	/// Raises:
	/// 	- Panics if `index` is invalid
	/// 	- Panics if `db` feature wasn't enabled
	#[text_signature = "($self, index, new_value, /)"]
	fn set_declare_word_value_i16(&mut self, index: usize, new_value: i16) {
		self.instr.set_declare_word_value_i16(index, new_value);
	}

	/// Sets a new `dw` value, see also `declare_data_len`.
	///
	/// Can only be called if `code` is `Code.DeclareWord`
	///
	/// Args:
	/// 	`index`: Index (0-7)
	/// 	`new_value`: New value
	///
	/// Raises:
	/// 	- Panics if `index` is invalid
	/// 	- Panics if `db` feature wasn't enabled
	#[text_signature = "($self, index, new_value, /)"]
	fn set_declare_word_value(&mut self, index: usize, new_value: u16) {
		self.instr.set_declare_word_value(index, new_value);
	}

	/// Gets a `dw` value, see also `declare_data_len`.
	///
	/// Can only be called if `code` is `Code.DeclareWord`
	///
	/// Args:
	/// 	`index`: Index (0-7)
	///
	/// Raises:
	/// 	Panics if `index` is invalid
	#[text_signature = "($self, index, /)"]
	fn get_declare_word_value(&self, index: usize) -> u16 {
		self.instr.get_declare_word_value(index)
	}

	/// Sets a new `dd` value, see also `declare_data_len`.
	///
	/// Can only be called if `code` is `Code.DeclareDword`
	///
	/// Args:
	/// 	`index`: Index (0-3)
	/// 	`new_value`: New value
	///
	/// Raises:
	/// 	- Panics if `index` is invalid
	/// 	- Panics if `db` feature wasn't enabled
	#[text_signature = "($self, index, new_value, /)"]
	fn set_declare_dword_value_i32(&mut self, index: usize, new_value: i32) {
		self.instr.set_declare_dword_value_i32(index, new_value);
	}

	/// Sets a new `dd` value, see also `declare_data_len`.
	///
	/// Can only be called if `code` is `Code.DeclareDword`
	///
	/// Args:
	/// 	`index`: Index (0-3)
	/// 	`new_value`: New value
	///
	/// Raises:
	/// 	- Panics if `index` is invalid
	/// 	- Panics if `db` feature wasn't enabled
	#[text_signature = "($self, index, new_value, /)"]
	fn set_declare_dword_value(&mut self, index: usize, new_value: u32) {
		self.instr.set_declare_dword_value(index, new_value);
	}

	/// Gets a `dd` value, see also `declare_data_len`.
	///
	/// Can only be called if `code` is `Code.DeclareDword`
	///
	/// Args:
	/// 	`index`: Index (0-3)
	///
	/// Raises:
	/// 	Panics if `index` is invalid
	#[text_signature = "($self, index, /)"]
	fn get_declare_dword_value(&self, index: usize) -> u32 {
		self.instr.get_declare_dword_value(index)
	}

	/// Sets a new `dq` value, see also `declare_data_len`.
	///
	/// Can only be called if `code` is `Code.DeclareQword`
	///
	/// Args:
	/// 	`index`: Index (0-1)
	/// 	`new_value`: New value
	///
	/// Raises:
	/// 	- Panics if `index` is invalid
	/// 	- Panics if `db` feature wasn't enabled
	#[text_signature = "($self, index, new_value, /)"]
	fn set_declare_qword_value_i64(&mut self, index: usize, new_value: i64) {
		self.instr.set_declare_qword_value_i64(index, new_value);
	}

	/// Sets a new `dq` value, see also `declare_data_len`.
	///
	/// Can only be called if `code` is `Code.DeclareQword`
	///
	/// Args:
	/// 	`index`: Index (0-1)
	/// 	`new_value`: New value
	///
	/// Raises:
	/// 	- Panics if `index` is invalid
	/// 	- Panics if `db` feature wasn't enabled
	#[text_signature = "($self, index, new_value, /)"]
	fn set_declare_qword_value(&mut self, index: usize, new_value: u64) {
		self.instr.set_declare_qword_value(index, new_value);
	}

	/// Gets a `dq` value, see also `declare_data_len`.
	///
	/// Can only be called if `code` is `Code.DeclareQword`
	///
	/// Args:
	/// 	`index`: Index (0-1)
	///
	/// Raises:
	/// 	Panics if `index` is invalid
	#[text_signature = "($self, index, /)"]
	fn get_declare_qword_value(&self, index: usize) -> u64 {
		self.instr.get_declare_qword_value(index)
	}

	/// Checks if this is a VSIB instruction, see also `is_vsib32`, `is_vsib64`
	#[getter]
	fn is_vsib(&self) -> bool {
		self.instr.is_vsib()
	}

	/// VSIB instructions only (`is_vsib`): `True` if it's using 32-bit indexes, `False` if it's using 64-bit indexes
	#[getter]
	fn is_vsib32(&self) -> bool {
		self.instr.is_vsib32()
	}

	/// VSIB instructions only (`is_vsib`): `True` if it's using 64-bit indexes, `False` if it's using 32-bit indexes
	#[getter]
	fn is_vsib64(&self) -> bool {
		self.instr.is_vsib64()
	}

	/// Checks if it's a vsib instruction.
	///
	/// Returns:
	/// 	`True`: If it's a VSIB instruction with 64-bit indexes
	/// 	`False`: If it's a VSIB instruction with 32-bit indexes
	/// 	`None`: If it's not a VSIB instruction.
	#[getter]
	fn vsib(&self) -> Option<bool> {
		self.instr.vsib()
	}

	/// Gets the suppress all exceptions flag (EVEX encoded instructions). Note that if `rounding_control` is not `RoundingControl.None`, SAE is implied but this method will still return `False`.
	#[getter]
	fn suppress_all_exceptions(&self) -> bool {
		self.instr.suppress_all_exceptions()
	}

	#[setter]
	fn set_suppress_all_exceptions(&mut self, new_value: bool) {
		self.instr.set_suppress_all_exceptions(new_value);
	}

	/// Checks if the memory operand is `RIP`/`EIP` relative
	#[getter]
	fn is_ip_rel_memory_operand(&self) -> bool {
		self.instr.is_ip_rel_memory_operand()
	}

	/// Gets the `RIP`/`EIP` releative address ((`next_ip` or `next_ip32`) + `memory_displacement`).
	///
	/// This method is only valid if there's a memory operand with `RIP`/`EIP` relative addressing, see `is_ip_rel_memory_operand`
	#[getter]
	fn ip_rel_memory_address(&self) -> u64 {
		self.instr.ip_rel_memory_address()
	}

	/// Gets the virtual address of a memory operand
	///
	/// Args:
	/// 	`operand`: Operand number, must be a memory operand
	/// 	`element_index`: Only used if it's a vsib memory operand. This is the element index of the vector index register.
	///
	/// Raises:
	/// 	Panics if `operand` is invalid
	///
	/// Examples:
	/// ```
	/// use iced_x86::*;
	///
	/// // add [rdi+r12*8-5AA5EDCCh],esi
	/// let bytes = b"\x42\x01\xB4\xE7\x34\x12\x5A\xA5";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	/// let instr = decoder.decode();
	///
	/// let va = instr.try_virtual_address(0, 0, |register, element_index, element_size| {
	///     match register {
	///         // The base address of ES, CS, SS and DS is always 0 in 64-bit mode
	///         Register::DS => Some(0x0000_0000_0000_0000),
	///         Register::RDI => Some(0x0000_0000_1000_0000),
	///         Register::R12 => Some(0x0000_0004_0000_0000),
	///         _ => None,
	///     }
	/// });
	/// assert_eq!(Some(0x0000_001F_B55A_1234), va);
	/// ```
	#[text_signature = "($self, operand, element_index, /)"]
	fn try_virtual_address(&self, _operand: u32, _element_index: usize) -> Option<u64> {
		None
	}
}

/// Contains the FPU `TOP` increment, whether it's conditional and whether the instruction writes to `TOP`
///
/// Args:
///		`increment`: Used if `writes_top` is `True`: Value added to `TOP`.
///		`conditional`: `True` if it's a conditional push/pop (eg. `FPTAN` or `FSINCOS`)
///		`writes_top`: `True` if `TOP` is written (it's a conditional/unconditional push/pop, `FNSAVE`, `FLDENV`, etc)
#[pyclass]
#[derive(Debug, Default, Copy, Clone, Eq, PartialEq, Hash)]
#[text_signature = "(increment, conditional, writes_top, /)"]
pub struct FpuStackIncrementInfo {
	info: iced_x86::FpuStackIncrementInfo,
}

#[pymethods]
impl FpuStackIncrementInfo {
	#[new]
	fn new(increment: i32, conditional: bool, writes_top: bool) -> Self {
		Self { info: iced_x86::FpuStackIncrementInfo::new(increment, conditional, writes_top) }
	}

	/// Used if `writes_top` is `True`: Value added to `TOP`.
	///
	/// This is negative if it pushes one or more values and positive if it pops one or more values
	/// and `0` if it writes to `TOP` (eg. `FLDENV`, etc) without pushing/popping anything.
	#[getter]
	pub fn increment(&self) -> i32 {
		self.info.increment()
	}

	/// `True` if it's a conditional push/pop (eg. `FPTAN` or `FSINCOS`)
	#[getter]
	pub fn conditional(&self) -> bool {
		self.info.conditional()
	}

	/// `True` if `TOP` is written (it's a conditional/unconditional push/pop, `FNSAVE`, `FLDENV`, etc)
	#[getter]
	pub fn writes_top(&self) -> bool {
		self.info.writes_top()
	}
}

#[pymethods]
impl Instruction {
	/// Gets the number of bytes added to `SP`/`ESP`/`RSP` or 0 if it's not an instruction that pushes or pops data.
	///
	/// This method assumes the instruction doesn't change the privilege level (eg. `IRET/D/Q`). If it's the `LEAVE`
	/// instruction, this method returns 0.
	///
	/// Examples:
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
	#[getter]
	fn stack_pointer_increment(&self) -> i32 {
		self.instr.stack_pointer_increment()
	}

	/// Gets the FPU status word's `TOP` increment and whether it's a conditional or unconditional push/pop and whether `TOP` is written.
	///
	/// Examples:
	/// ```
	/// use iced_x86::*;
	///
	/// // ficomp dword ptr [rax]
	/// let bytes = b"\xDA\x18";
	/// let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
	/// let instr = decoder.decode();
	///
	/// let info = instr.fpu_stack_increment_info();
	/// // It pops the stack once
	/// assert_eq!(1, info.increment());
	/// assert!(!info.conditional());
	/// assert!(info.writes_top());
	/// ```
	#[text_signature = "($self, /)"]
	fn fpu_stack_increment_info(&self) -> FpuStackIncrementInfo {
		FpuStackIncrementInfo { info: self.instr.fpu_stack_increment_info() }
	}

	/// Instruction encoding, eg. Legacy, 3DNow!, VEX, EVEX, XOP (an `EncodingKind` enum value)
	///
	/// Examples:
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
	#[getter]
	fn encoding(&self) -> u32 {
		self.instr.encoding() as u32
	}

	/// Gets the CPU or CPUID feature flags (a list of `CpuidFeature` enum values)
	///
	/// Examples:
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
	#[text_signature = "($self, /)"]
	fn cpuid_features(&self) -> Vec<u32> {
		self.instr.cpuid_features().iter().map(|x| *x as u32).collect()
	}

	/// Control flow info (a `FlowControl` enum value)
	///
	/// Examples:
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
	#[getter]
	fn flow_control(&self) -> u32 {
		self.instr.flow_control() as u32
	}

	/// `True` if it's a privileged instruction (all CPL=0 instructions (except `VMCALL`) and IOPL instructions `IN`, `INS`, `OUT`, `OUTS`, `CLI`, `STI`)
	#[getter]
	fn is_privileged(&self) -> bool {
		self.instr.is_privileged()
	}

	/// `True` if this is an instruction that implicitly uses the stack pointer (`SP`/`ESP`/`RSP`), eg. `CALL`, `PUSH`, `POP`, `RET`, etc.
	///
	/// See also `stack_pointer_increment`
	///
	/// Examples:
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
	#[getter]
	fn is_stack_instruction(&self) -> bool {
		self.instr.is_stack_instruction()
	}

	/// `True` if it's an instruction that saves or restores too many registers (eg. `FXRSTOR`, `XSAVE`, etc).
	#[getter]
	fn is_save_restore_instruction(&self) -> bool {
		self.instr.is_save_restore_instruction()
	}

	/// All flags that are read by the CPU when executing the instruction.
	///
	/// This method returns a `RflagsBits` value. See also `rflags_modified`.
	///
	/// Examples:
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
	#[getter]
	fn rflags_read(&self) -> u32 {
		self.instr.rflags_read()
	}

	/// All flags that are written by the CPU, except those flags that are known to be undefined, always set or always cleared.
	///
	/// This method returns a `RflagsBits` value. See also `rflags_modified`.
	///
	/// Examples:
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
	#[getter]
	fn rflags_written(&self) -> u32 {
		self.instr.rflags_written()
	}

	/// All flags that are always cleared by the CPU.
	///
	/// This method returns a `RflagsBits` value. See also `rflags_modified`.
	///
	/// Examples:
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
	#[getter]
	fn rflags_cleared(&self) -> u32 {
		self.instr.rflags_cleared()
	}

	/// All flags that are always set by the CPU.
	///
	/// This method returns a `RflagsBits` value. See also `rflags_modified`.
	///
	/// Examples:
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
	#[getter]
	fn rflags_set(&self) -> u32 {
		self.instr.rflags_set()
	}

	/// All flags that are undefined after executing the instruction.
	///
	/// This method returns a `RflagsBits` value. See also `rflags_modified`.
	///
	/// Examples:
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
	#[getter]
	fn rflags_undefined(&self) -> u32 {
		self.instr.rflags_undefined()
	}

	/// All flags that are modified by the CPU. This is `rflags_written + rflags_cleared + rflags_set + rflags_undefined`.
	///
	/// This method returns a `RflagsBits` value.
	///
	/// Examples:
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
	#[getter]
	fn rflags_modified(&self) -> u32 {
		self.instr.rflags_modified()
	}

	/// Checks if it's a `Jcc SHORT` or `Jcc NEAR` instruction
	#[getter]
	fn is_jcc_short_or_near(&self) -> bool {
		self.instr.is_jcc_short_or_near()
	}

	/// Checks if it's a `Jcc NEAR` instruction
	#[getter]
	fn is_jcc_near(&self) -> bool {
		self.instr.is_jcc_near()
	}

	/// Checks if it's a `Jcc SHORT` instruction
	#[getter]
	fn is_jcc_short(&self) -> bool {
		self.instr.is_jcc_short()
	}

	/// Checks if it's a `JMP SHORT` instruction
	#[getter]
	fn is_jmp_short(&self) -> bool {
		self.instr.is_jmp_short()
	}

	/// Checks if it's a `JMP NEAR` instruction
	#[getter]
	fn is_jmp_near(&self) -> bool {
		self.instr.is_jmp_near()
	}

	/// Checks if it's a `JMP SHORT` or a `JMP NEAR` instruction
	#[getter]
	fn is_jmp_short_or_near(&self) -> bool {
		self.instr.is_jmp_short_or_near()
	}

	/// Checks if it's a `JMP FAR` instruction
	#[getter]
	fn is_jmp_far(&self) -> bool {
		self.instr.is_jmp_far()
	}

	/// Checks if it's a `CALL NEAR` instruction
	#[getter]
	fn is_call_near(&self) -> bool {
		self.instr.is_call_near()
	}

	/// Checks if it's a `CALL FAR` instruction
	#[getter]
	fn is_call_far(&self) -> bool {
		self.instr.is_call_far()
	}

	/// Checks if it's a `JMP NEAR reg/[mem]` instruction
	#[getter]
	fn is_jmp_near_indirect(&self) -> bool {
		self.instr.is_jmp_near_indirect()
	}

	/// Checks if it's a `JMP FAR [mem]` instruction
	#[getter]
	fn is_jmp_far_indirect(&self) -> bool {
		self.instr.is_jmp_far_indirect()
	}

	/// Checks if it's a `CALL NEAR reg/[mem]` instruction
	#[getter]
	fn is_call_near_indirect(&self) -> bool {
		self.instr.is_call_near_indirect()
	}

	/// Checks if it's a `CALL FAR [mem]` instruction
	#[getter]
	fn is_call_far_indirect(&self) -> bool {
		self.instr.is_call_far_indirect()
	}

	/// Negates the condition code, eg. `JE` -> `JNE`.
	///
	/// Can be used if it's `Jcc`, `SETcc`, `CMOVcc`, `LOOPcc` and does nothing if the instruction doesn't have a condition code.
	///
	/// Examples:
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
	#[text_signature = "($self, /)"]
	fn negate_condition_code(&mut self) {
		self.instr.negate_condition_code()
	}

	/// Converts `Jcc/JMP NEAR` to `Jcc/JMP SHORT` and does nothing if it's not a `Jcc/JMP NEAR` instruction
	///
	/// Examples:
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
	#[text_signature = "($self, /)"]
	fn as_short_branch(&mut self) {
		self.instr.as_short_branch()
	}

	/// Converts `Jcc/JMP SHORT` to `Jcc/JMP NEAR` and does nothing if it's not a `Jcc/JMP SHORT` instruction
	///
	/// Examples:
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
	#[text_signature = "($self, /)"]
	fn as_near_branch(&mut self) {
		self.instr.as_near_branch()
	}

	/// Gets the condition code (a `ConditionCode` enum value) if it's `Jcc`, `SETcc`, `CMOVcc`, `LOOPcc` else `ConditionCode.None` is returned
	///
	/// Examples:
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
	#[getter]
	fn condition_code(&self) -> u32 {
		self.instr.condition_code() as u32
	}
}

#[pymethods]
#[cfg(xxxxxxx)] //TODO: REMOVE
impl Instruction {
	/// Gets the `OpCodeInfo`
	#[getter]
	fn op_code(&self) -> &'static OpCodeInfo {
		self.instr.op_code()
	}
}

impl Instruction {
	fn format(&self, format_spec: &str) -> PyResult<String> {
		enum FormatSpecFmtKind {
			Fast,
			Gas,
			Intel,
			Masm,
			Nasm,
		}

		let mut fmt_kind = FormatSpecFmtKind::Masm;

		for c in format_spec.chars() {
			match c {
				'f' => fmt_kind = FormatSpecFmtKind::Fast,
				'g' => fmt_kind = FormatSpecFmtKind::Gas,
				'i' => fmt_kind = FormatSpecFmtKind::Intel,
				'm' => fmt_kind = FormatSpecFmtKind::Masm,
				'n' => fmt_kind = FormatSpecFmtKind::Nasm,
				_ => {}
			}
		}

		let mut fmt_opts = match fmt_kind {
			FormatSpecFmtKind::Fast => iced_x86::FormatterOptions::with_masm(),
			FormatSpecFmtKind::Gas => iced_x86::FormatterOptions::with_gas(),
			FormatSpecFmtKind::Intel => iced_x86::FormatterOptions::with_intel(),
			FormatSpecFmtKind::Masm => iced_x86::FormatterOptions::with_masm(),
			FormatSpecFmtKind::Nasm => iced_x86::FormatterOptions::with_nasm(),
		};

		for c in format_spec.chars() {
			match c {
				'f' | 'g' | 'i' | 'm' | 'n' => {}
				'X' | 'x' => {
					fmt_opts.set_uppercase_hex(c == 'X');
					fmt_opts.set_hex_prefix("0x");
					fmt_opts.set_hex_suffix("");
				}
				'H' | 'h' => {
					fmt_opts.set_uppercase_hex(c == 'H');
					fmt_opts.set_hex_prefix("");
					fmt_opts.set_hex_suffix("h");
				}
				'r' => fmt_opts.set_rip_relative_addresses(true),
				'U' => fmt_opts.set_uppercase_all(true),
				's' => fmt_opts.set_space_after_operand_separator(true),
				'S' => fmt_opts.set_always_show_segment_register(true),
				'B' => fmt_opts.set_show_branch_size(false),
				'G' => fmt_opts.set_gas_show_mnemonic_size_suffix(true),
				'M' => fmt_opts.set_memory_size_options(iced_x86::MemorySizeOptions::Always),
				_ => return Err(PyValueError::new_err(format!("Unknown format code '{}'", format_spec))),
			}
		}

		let mut output = String::new();
		match fmt_kind {
			FormatSpecFmtKind::Fast => {
				let mut formatter = iced_x86::FastFormatter::new();
				formatter.options_mut().set_space_after_operand_separator(fmt_opts.space_after_operand_separator());
				formatter.options_mut().set_rip_relative_addresses(fmt_opts.rip_relative_addresses());
				formatter.options_mut().set_always_show_segment_register(fmt_opts.always_show_segment_register());
				formatter.options_mut().set_always_show_memory_size(fmt_opts.memory_size_options() == iced_x86::MemorySizeOptions::Always);
				formatter.options_mut().set_uppercase_hex(fmt_opts.uppercase_hex());
				formatter.options_mut().set_use_hex_prefix(fmt_opts.hex_prefix() == "0x");
				formatter.format(&self.instr, &mut output);
			}
			FormatSpecFmtKind::Gas => {
				use iced_x86::Formatter;
				let mut formatter = iced_x86::GasFormatter::new();
				*formatter.options_mut() = fmt_opts;
				formatter.format(&self.instr, &mut output);
			}
			FormatSpecFmtKind::Intel => {
				use iced_x86::Formatter;
				let mut formatter = iced_x86::IntelFormatter::new();
				*formatter.options_mut() = fmt_opts;
				formatter.format(&self.instr, &mut output);
			}
			FormatSpecFmtKind::Masm => {
				use iced_x86::Formatter;
				let mut formatter = iced_x86::MasmFormatter::new();
				*formatter.options_mut() = fmt_opts;
				formatter.format(&self.instr, &mut output);
			}
			FormatSpecFmtKind::Nasm => {
				use iced_x86::Formatter;
				let mut formatter = iced_x86::NasmFormatter::new();
				*formatter.options_mut() = fmt_opts;
				formatter.format(&self.instr, &mut output);
			}
		};

		Ok(output)
	}
}

#[pyproto]
impl PyObjectProtocol for Instruction {
	fn __repr__(&self) -> PyResult<String> {
		self.format("")
	}

	fn __str__(&self) -> PyResult<String> {
		self.format("")
	}

	fn __format__(&self, format_spec: &str) -> PyResult<String> {
		self.format(format_spec)
	}

	fn __richcmp__(&self, other: PyRef<Instruction>, op: CompareOp) -> PyObject {
		match op {
			CompareOp::Eq => (self.instr == other.instr).into_py(other.py()),
			CompareOp::Ne => (self.instr != other.instr).into_py(other.py()),
			_ => other.py().NotImplemented(),
		}
	}

	fn __hash__(&self) -> u64 {
		let mut hasher = DefaultHasher::new();
		self.instr.hash(&mut hasher);
		hasher.finish()
	}

	fn __bool__(&self) -> bool {
		!self.instr.is_invalid()
	}
}

#[pyproto]
impl PySequenceProtocol for Instruction {
	fn __len__(&self) -> usize {
		self.instr.len()
	}
}

#[pymethods]
impl Instruction {
	//TODO: all generated create() methods
}
