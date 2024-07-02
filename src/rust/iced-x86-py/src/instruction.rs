// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::enum_utils::{to_code, to_code_size, to_mvex_reg_mem_conv, to_op_kind, to_register, to_rep_prefix_kind, to_rounding_control};
use crate::memory_operand::MemoryOperand;
use crate::op_code_info::OpCodeInfo;
use crate::utils::{get_temporary_byte_array_ref, to_value_error};
use bincode::{deserialize, serialize};
use core::hash::{Hash, Hasher};
use pyo3::class::basic::CompareOp;
use pyo3::exceptions::PyValueError;
use pyo3::prelude::*;
use pyo3::types::PyBytes;
use std::collections::hash_map::DefaultHasher;

/// A 16/32/64-bit x86 instruction. Created by :class:`Decoder` or by ``Instruction.create*()`` methods.
///
/// Examples:
///
/// A decoder is usually used to create instructions:
///
/// .. testcode::
///
///     from iced_x86 import *
///
///     # xchg ah,[rdx+rsi+16h]
///     data = b"\x86\x64\x32\x16"
///     decoder = Decoder(64, data, ip=0x1234_5678)
///
///     instr = decoder.decode()
///
///     # Instruction supports __bool__() and returns True if it's
///     # a valid instruction:
///     if not instr:
///         print("Invalid instruction (garbage, data, etc)")
///     # The above code is the same as:
///     if instr.code == Code.INVALID:
///         print("Not an instruction")
///
/// But there are also static `Instruction.create*()` methods that can be used to create instructions:
///
/// .. testcode::
///
///     nop = Instruction.create(Code.NOPD)
///     xor = Instruction.create_reg_i32(Code.XOR_RM64_IMM8, Register.R14, -1)
///     rep_stosd = Instruction.create_rep_stosd(64)
///     add = Instruction.create_mem_i32(Code.ADD_RM64_IMM8, MemoryOperand(Register.RCX, Register.RDX, 8, 0x1234_5678), 2)
///     print(f"{nop}")
///     print(f"{xor}")
///     print(f"{rep_stosd}")
///     print(f"{add}")
///
/// Output:
///
/// .. testoutput::
///
///     nop
///     xor r14,0FFFFFFFFFFFFFFFFh
///     rep stosd
///     add qword ptr [rcx+rdx*8+12345678h],2
///
/// Once you have an instruction you can format it either by using a :class:`Formatter`
/// or by calling the instruction's ``__repr__()``, ``__str__()`` or ``__format__()`` methods.
///
/// .. testcode::
///
///     # Continued from the above example
///
///     formatter = Formatter(FormatterSyntax.INTEL)
///
///     # Change some options
///     formatter.uppercase_mnemonics = True
///     formatter.space_after_operand_separator = True
///     formatter.first_operand_char_index = 8
///
///     print(f"disasm  : {formatter.format(instr)}")
///     # `instr.mnemonic` also returns a `Mnemonic` enum
///     print(f"mnemonic: {formatter.format_mnemonic(instr, FormatMnemonicOptions.NO_PREFIXES)}")
///     print(f"operands: {formatter.format_all_operands(instr)}")
///     # `instr.op0_kind`/etc return operand kind, see also `instr.op0_register`, etc to get reg/mem info
///     print(f"op #0   : {formatter.format_operand(instr, 0)}")
///     print(f"op #1   : {formatter.format_operand(instr, 1)}")
///     print(f"reg RCX : {formatter.format_register(Register.RCX)}")
///
/// Output:
///
/// .. testoutput::
///
///     disasm  : XCHG    [rdx+rsi+16h], ah
///     mnemonic: XCHG
///     operands: [rdx+rsi+16h], ah
///     op #0   : [rdx+rsi+16h]
///     op #1   : ah
///     reg RCX : rcx
///
/// .. testcode::
///
///     # A formatter isn't needed if you like most of the default options.
///     # repr() == str() == format() all return the same thing.
///     print(f"disasm  : {repr(instr)}")
///     print(f"disasm  : {str(instr)}")
///     print(f"disasm  : {instr}")
///
/// Output:
///
/// .. testoutput::
///
///     disasm  : xchg ah,[rdx+rsi+16h]
///     disasm  : xchg ah,[rdx+rsi+16h]
///     disasm  : xchg ah,[rdx+rsi+16h]
///
/// .. testcode::
///
///     # __format__() supports a format spec argument, see the table below
///     print(f"disasm  : {instr:f}")
///     print(f"disasm  : {instr:g}")
///     print(f"disasm  : {instr:i}")
///     print(f"disasm  : {instr:m}")
///     print(f"disasm  : {instr:n}")
///     print(f"disasm  : {instr:gxsSG}")
///
/// Output:
///
/// .. testoutput::
///
///     disasm  : xchg [rdx+rsi+16h],ah
///     disasm  : xchg %ah,0x16(%rdx,%rsi)
///     disasm  : xchg [rdx+rsi+16h],ah
///     disasm  : xchg ah,[rdx+rsi+16h]
///     disasm  : xchg ah,[rdx+rsi+16h]
///     disasm  : xchgb %ah, %ds:0x16(%rdx,%rsi)
///
/// The following format specifiers are supported in any order. If you omit the
/// formatter kind, the default formatter is used (eg. masm):
///
/// ====== =============================================================================
/// F-Spec Description
/// ====== =============================================================================
/// f      Fast formatter (masm-like syntax)
/// g      GNU Assembler formatter
/// i      Intel (XED) formatter
/// m      masm formatter
/// n      nasm formatter
/// X      Uppercase hex numbers with ``0x`` prefix
/// x      Lowercase hex numbers with ``0x`` prefix
/// H      Uppercase hex numbers with ``h`` suffix
/// h      Lowercase hex numbers with ``h`` suffix
/// r      RIP-relative memory operands use RIP register instead of abs addr (``[rip+123h]`` vs ``[123456789ABCDEF0h]``)
/// U      Uppercase everything except numbers and hex prefixes/suffixes (ignored by fast fmt)
/// s      Add a space after the operand separator
/// S      Always show the segment register (memory operands)
/// B      Don't show the branch size (``SHORT`` or ``NEAR PTR``) (ignored by fast fmt)
/// G      (GNU Assembler): Add mnemonic size suffix (eg. ``movl`` vs ``mov``)
/// M      Always show the memory size (eg. ``BYTE PTR``) even when not needed
/// _      Use digit separators (eg. ``0x12345678`` vs ``0x1234_5678``) (ignored by fast fmt)
/// ====== =============================================================================
#[pyclass(module = "iced_x86._iced_x86_py")]
#[derive(Copy, Clone)]
pub(crate) struct Instruction {
	pub(crate) instr: iced_x86::Instruction,
}

#[pymethods]
impl Instruction {
	#[new]
	#[pyo3(text_signature = "()")]
	pub(crate) fn new() -> Self {
		Self { instr: iced_x86::Instruction::default() }
	}

	/// Set the internal state with the given unpickled state.
	///
	/// Args:
	///     state (Any): unpickled state
	#[pyo3(text_signature = "($self, state)")]
	fn __setstate__(&mut self, state: &Bound<'_, PyAny>) -> PyResult<()> {
		match state.downcast::<PyBytes>() {
			Ok(s) => {
				self.instr = deserialize(s.as_bytes()).map_err(to_value_error)?;
				Ok(())
			}
			Err(e) => Err(to_value_error(e)),
		}
	}

	/// Get the unpickled state corresponding to the instruction.
	///
	/// Returns:
	///     bytes: The unpickled state
	#[pyo3(text_signature = "($self)")]
	fn __getstate__(&self, py: Python<'_>) -> PyResult<PyObject> {
		let state = PyBytes::new_bound(py, &serialize(&self.instr).map_err(to_value_error)?).to_object(py);
		Ok(state)
	}

	/// Returns a copy of this instance.
	///
	/// Returns:
	///     Instruction: A copy of this instance
	///
	/// This is identical to :class:`Instruction.copy`
	#[pyo3(text_signature = "($self)")]
	fn __copy__(&self) -> Self {
		*self
	}

	/// Returns a copy of this instance.
	///
	/// Args:
	///     memo (Any): memo dict
	///
	/// Returns:
	///     Instruction: A copy of this instance
	///
	/// This is identical to :class:`Instruction.copy`
	#[pyo3(text_signature = "($self, memo)")]
	fn __deepcopy__(&self, _memo: &Bound<'_, PyAny>) -> Self {
		*self
	}

	/// Returns a copy of this instance.
	///
	/// Returns:
	///     Instruction: A copy of this instance
	#[pyo3(text_signature = "($self)")]
	fn copy(&self) -> Self {
		*self
	}

	/// Checks if two instructions are equal, comparing all bits, not ignoring anything. ``==`` ignores some fields.
	///
	/// Args:
	///     other (:class:`Instruction`): Other instruction
	///
	/// Returns:
	///     bool: ``True`` if `other` is exactly identical to this instance
	#[pyo3(text_signature = "($self, other)")]
	fn eq_all_bits(&self, other: &Self) -> bool {
		self.instr.eq_all_bits(&other.instr)
	}

	/// int: (``u16``) Gets the 16-bit IP of the instruction
	#[getter]
	fn ip16(&self) -> u16 {
		self.instr.ip16()
	}

	#[setter]
	fn set_ip16(&mut self, new_value: u16) {
		self.instr.set_ip16(new_value)
	}

	/// int: (``u32``) Gets the 32-bit IP of the instruction
	#[getter]
	fn ip32(&self) -> u32 {
		self.instr.ip32()
	}

	#[setter]
	fn set_ip32(&mut self, new_value: u32) {
		self.instr.set_ip32(new_value)
	}

	/// int: (``u64``) Gets the 64-bit IP of the instruction
	#[getter]
	fn ip(&self) -> u64 {
		self.instr.ip()
	}

	#[setter]
	fn set_ip(&mut self, new_value: u64) {
		self.instr.set_ip(new_value)
	}

	/// int: (``u16``) Gets the 16-bit IP of the next instruction
	#[getter]
	fn next_ip16(&self) -> u16 {
		self.instr.next_ip16()
	}

	#[setter]
	fn set_next_ip16(&mut self, new_value: u16) {
		self.instr.set_next_ip16(new_value)
	}

	/// int: (``u32``) Gets the 32-bit IP of the next instruction
	#[getter]
	fn next_ip32(&self) -> u32 {
		self.instr.next_ip32()
	}

	#[setter]
	fn set_next_ip32(&mut self, new_value: u32) {
		self.instr.set_next_ip32(new_value)
	}

	/// int: (``u64``) Gets the 64-bit IP of the next instruction
	#[getter]
	fn next_ip(&self) -> u64 {
		self.instr.next_ip()
	}

	#[setter]
	fn set_next_ip(&mut self, new_value: u64) {
		self.instr.set_next_ip(new_value)
	}

	/// :class:`CodeSize`: Gets the code size (a :class:`CodeSize` enum value) when the instruction was decoded.
	///
	/// Note:
	///     This value is informational and can be used by a formatter.
	#[getter]
	fn code_size(&self) -> u32 {
		self.instr.code_size() as u32
	}

	#[setter]
	fn set_code_size(&mut self, new_value: u32) -> PyResult<()> {
		self.instr.set_code_size(to_code_size(new_value)?);
		Ok(())
	}

	/// bool: Checks if it's an invalid instruction (:class:`Instruction.code` == :class:`Code.INVALID`)
	#[getter]
	fn is_invalid(&self) -> bool {
		self.instr.is_invalid()
	}

	/// :class:`Code`: Gets the instruction code (a :class:`Code` enum value), see also :class:`Instruction.mnemonic`
	#[getter]
	fn code(&self) -> u32 {
		self.instr.code() as u32
	}

	#[setter]
	fn set_code(&mut self, new_value: u32) -> PyResult<()> {
		self.instr.set_code(to_code(new_value)?);
		Ok(())
	}

	/// :class:`Mnemonic`: Gets the mnemonic (a :class:`Mnemonic` enum value), see also :class:`Instruction.code`
	#[getter]
	fn mnemonic(&self) -> u32 {
		self.instr.mnemonic() as u32
	}

	/// int: Gets the operand count. An instruction can have 0-5 operands.
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # add [rax],ebx
	///     data = b"\x01\x18"
	///     decoder = Decoder(64, data)
	///     instr = decoder.decode()
	///
	///     assert instr.op_count == 2
	#[getter]
	fn op_count(&self) -> u32 {
		self.instr.op_count()
	}

	/// int: (``u8``) Gets the length of the instruction, 0-15 bytes.
	///
	/// You can also call `len(instr)` to get this value.
	///
	/// Note:
	///     This is just informational. If you modify the instruction or create a new one, this method could return the wrong value.
	#[getter]
	fn len(&self) -> usize {
		self.instr.len()
	}

	#[setter]
	fn set_len(&mut self, new_value: usize) {
		self.instr.set_len(new_value)
	}

	/// bool: ``True`` if the instruction has the ``XACQUIRE`` prefix (``F2``)
	#[getter]
	fn has_xacquire_prefix(&self) -> bool {
		self.instr.has_xacquire_prefix()
	}

	#[setter]
	fn set_has_xacquire_prefix(&mut self, new_value: bool) {
		self.instr.set_has_xacquire_prefix(new_value)
	}

	/// bool: ``True`` if the instruction has the ``XRELEASE`` prefix (``F3``)
	#[getter]
	fn has_xrelease_prefix(&self) -> bool {
		self.instr.has_xrelease_prefix()
	}

	#[setter]
	fn set_has_xrelease_prefix(&mut self, new_value: bool) {
		self.instr.set_has_xrelease_prefix(new_value)
	}

	/// bool: ``True`` if the instruction has the ``REPE`` or ``REP`` prefix (``F3``)
	#[getter]
	fn has_rep_prefix(&self) -> bool {
		self.instr.has_rep_prefix()
	}

	#[setter]
	fn set_has_rep_prefix(&mut self, new_value: bool) {
		self.instr.set_has_rep_prefix(new_value)
	}

	/// bool: ``True`` if the instruction has the ``REPE`` or ``REP`` prefix (``F3``)
	#[getter]
	fn has_repe_prefix(&self) -> bool {
		self.instr.has_repe_prefix()
	}

	#[setter]
	fn set_has_repe_prefix(&mut self, new_value: bool) {
		self.instr.set_has_repe_prefix(new_value)
	}

	/// bool: ``True`` if the instruction has the ``REPNE`` prefix (``F2``)
	#[getter]
	fn has_repne_prefix(&self) -> bool {
		self.instr.has_repne_prefix()
	}

	#[setter]
	fn set_has_repne_prefix(&mut self, new_value: bool) {
		self.instr.set_has_repne_prefix(new_value)
	}

	/// bool: ``True`` if the instruction has the ``LOCK`` prefix (``F0``)
	#[getter]
	fn has_lock_prefix(&self) -> bool {
		self.instr.has_lock_prefix()
	}

	#[setter]
	fn set_has_lock_prefix(&mut self, new_value: bool) {
		self.instr.set_has_lock_prefix(new_value)
	}

	/// :class:`OpKind`: Gets operand #0's kind (an :class:`OpKind` enum value) if the operand exists (see :class:`Instruction.op_count` and :class:`Instruction.op_kind`)
	#[getter]
	fn op0_kind(&self) -> u32 {
		self.instr.op0_kind() as u32
	}

	#[setter]
	fn set_op0_kind(&mut self, new_value: u32) -> PyResult<()> {
		self.instr.set_op0_kind(to_op_kind(new_value)?);
		Ok(())
	}

	/// :class:`OpKind`: Gets operand #1's kind (an :class:`OpKind` enum value) if the operand exists (see :class:`Instruction.op_count` and :class:`Instruction.op_kind`)
	#[getter]
	fn op1_kind(&self) -> u32 {
		self.instr.op1_kind() as u32
	}

	#[setter]
	fn set_op1_kind(&mut self, new_value: u32) -> PyResult<()> {
		self.instr.set_op1_kind(to_op_kind(new_value)?);
		Ok(())
	}

	/// :class:`OpKind`: Gets operand #2's kind (an :class:`OpKind` enum value) if the operand exists (see :class:`Instruction.op_count` and :class:`Instruction.op_kind`)
	#[getter]
	fn op2_kind(&self) -> u32 {
		self.instr.op2_kind() as u32
	}

	#[setter]
	fn set_op2_kind(&mut self, new_value: u32) -> PyResult<()> {
		self.instr.set_op2_kind(to_op_kind(new_value)?);
		Ok(())
	}

	/// :class:`OpKind`: Gets operand #3's kind (an :class:`OpKind` enum value) if the operand exists (see :class:`Instruction.op_count` and :class:`Instruction.op_kind`)
	#[getter]
	fn op3_kind(&self) -> u32 {
		self.instr.op3_kind() as u32
	}

	#[setter]
	fn set_op3_kind(&mut self, new_value: u32) -> PyResult<()> {
		self.instr.set_op3_kind(to_op_kind(new_value)?);
		Ok(())
	}

	/// :class:`OpKind`: Gets operand #4's kind (an :class:`OpKind` enum value) if the operand exists (see :class:`Instruction.op_count` and :class:`Instruction.op_kind`)
	#[getter]
	fn op4_kind(&self) -> u32 {
		self.instr.op4_kind() as u32
	}

	#[setter]
	fn set_op4_kind(&mut self, new_value: u32) -> PyResult<()> {
		self.instr.try_set_op4_kind(to_op_kind(new_value)?).map_err(to_value_error)
	}

	/// Gets an operand's kind (an :class:`OpKind` enum value) if it exists (see :class:`Instruction.op_count`)
	///
	/// Args:
	///     `operand` (int): Operand number, 0-4
	///
	/// Returns:
	///     :class:`OpKind`: The operand's operand kind
	///
	/// Raises:
	///     ValueError: If `operand` is invalid
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # add [rax],ebx
	///     data = b"\x01\x18"
	///     decoder = Decoder(64, data)
	///     instr = decoder.decode()
	///
	///     assert instr.op_count == 2
	///     assert instr.op_kind(0) == OpKind.MEMORY
	///     assert instr.memory_base == Register.RAX
	///     assert instr.memory_index == Register.NONE
	///     assert instr.op_kind(1) == OpKind.REGISTER
	///     assert instr.op_register(1) == Register.EBX
	#[pyo3(text_signature = "($self, operand)")]
	fn op_kind(&self, operand: u32) -> PyResult<u32> {
		self.instr.try_op_kind(operand).map_or_else(|e| Err(to_value_error(e)), |op_kind| Ok(op_kind as u32))
	}

	/// Sets an operand's kind
	///
	/// Args:
	///     `operand` (int): Operand number, 0-4
	///     `op_kind` (:class:`OpKind`): Operand kind
	///
	/// Raises:
	///     ValueError: If `operand` is invalid
	#[pyo3(text_signature = "($self, operand, op_kind)")]
	fn set_op_kind(&mut self, operand: u32, op_kind: u32) -> PyResult<()> {
		self.instr.try_set_op_kind(operand, to_op_kind(op_kind)?).map_err(to_value_error)
	}

	/// bool: Checks if the instruction has a segment override prefix, see :class:`Instruction.segment_prefix`
	#[getter]
	fn has_segment_prefix(&self) -> bool {
		self.instr.has_segment_prefix()
	}

	/// :class:`Register`: Gets the segment override prefix (a :class:`Register` enum value) or :class:`Register.NONE` if none.
	///
	/// See also :class:`Instruction.memory_segment`.
	///
	/// Use this method if the operand has kind :class:`OpKind.MEMORY`,
	/// :class:`OpKind.MEMORY_SEG_SI`, :class:`OpKind.MEMORY_SEG_ESI`, :class:`OpKind.MEMORY_SEG_RSI`
	#[getter]
	fn segment_prefix(&self) -> u32 {
		self.instr.segment_prefix() as u32
	}

	#[setter]
	fn set_segment_prefix(&mut self, new_value: u32) -> PyResult<()> {
		self.instr.set_segment_prefix(to_register(new_value)?);
		Ok(())
	}

	/// :class:`Register`: Gets the effective segment register used to reference the memory location (a :class:`Register` enum value).
	///
	/// Use this method if the operand has kind :class:`OpKind.MEMORY`,
	/// :class:`OpKind.MEMORY_SEG_SI`, :class:`OpKind.MEMORY_SEG_ESI`, :class:`OpKind.MEMORY_SEG_RSI`
	#[getter]
	fn memory_segment(&self) -> u32 {
		self.instr.memory_segment() as u32
	}

	/// int: (``u8``) Gets the size of the memory displacement in bytes.
	///
	/// Valid values are ``0``, ``1`` (16/32/64-bit), ``2`` (16-bit), ``4`` (32-bit), ``8`` (64-bit).
	///
	/// Note that the return value can be 1 and :class:`Instruction.memory_displacement` may still not fit in
	/// a signed byte if it's an EVEX/MVEX encoded instruction.
	///
	/// Use this method if the operand has kind :class:`OpKind.MEMORY`
	#[getter]
	fn memory_displ_size(&self) -> u32 {
		self.instr.memory_displ_size()
	}

	#[setter]
	fn set_memory_displ_size(&mut self, new_value: u32) {
		self.instr.set_memory_displ_size(new_value)
	}

	/// bool: ``True`` if the data is broadcast (EVEX instructions only)
	#[getter]
	fn is_broadcast(&self) -> bool {
		self.instr.is_broadcast()
	}

	#[setter]
	fn set_is_broadcast(&mut self, new_value: bool) {
		self.instr.set_is_broadcast(new_value)
	}

	/// bool: ``True`` if eviction hint bit is set (``{eh}``) (MVEX instructions only)
	#[getter]
	fn is_mvex_eviction_hint(&self) -> bool {
		self.instr.is_mvex_eviction_hint()
	}

	#[setter]
	fn set_is_mvex_eviction_hint(&mut self, new_value: bool) {
		self.instr.set_is_mvex_eviction_hint(new_value)
	}

	/// :class:`MvexRegMemConv`: (MVEX) Register/memory operand conversion function (an :class:`MvexRegMemConv` enum value)
	#[getter]
	fn mvex_reg_mem_conv(&self) -> u32 {
		self.instr.mvex_reg_mem_conv() as u32
	}

	#[setter]
	fn set_mvex_reg_mem_conv(&mut self, new_value: u32) -> PyResult<()> {
		self.instr.set_mvex_reg_mem_conv(to_mvex_reg_mem_conv(new_value)?);
		Ok(())
	}

	/// :class:`MemorySize`: Gets the size of the memory location (a :class:`MemorySize` enum value) that is referenced by the operand.
	///
	/// See also :class:`Instruction.is_broadcast`.
	///
	/// Use this method if the operand has kind :class:`OpKind.MEMORY`,
	/// :class:`OpKind.MEMORY_SEG_SI`, :class:`OpKind.MEMORY_SEG_ESI`, :class:`OpKind.MEMORY_SEG_RSI`,
	/// :class:`OpKind.MEMORY_ESDI`, :class:`OpKind.MEMORY_ESEDI`, :class:`OpKind.MEMORY_ESRDI`
	#[getter]
	fn memory_size(&self) -> u32 {
		self.instr.memory_size() as u32
	}

	/// int: (``u8``) Gets the index register scale value, valid values are ``*1``, ``*2``, ``*4``, ``*8``.
	///
	/// Use this method if the operand has kind :class:`OpKind.MEMORY`
	#[getter]
	fn memory_index_scale(&self) -> u32 {
		self.instr.memory_index_scale()
	}

	#[setter]
	fn set_memory_index_scale(&mut self, new_value: u32) {
		self.instr.set_memory_index_scale(new_value)
	}

	/// int: (``u64``) Gets the memory operand's displacement or the 64-bit absolute address if it's
	/// an ``EIP`` or ``RIP`` relative memory operand.
	///
	/// Use this method if the operand has kind :class:`OpKind.MEMORY`
	#[getter]
	fn memory_displacement(&self) -> u64 {
		self.instr.memory_displacement64()
	}

	#[setter]
	fn set_memory_displacement(&mut self, new_value: u64) {
		self.instr.set_memory_displacement64(new_value)
	}

	/// Gets an operand's immediate value
	///
	/// Args:
	///     `operand` (int): Operand number, 0-4
	///
	/// Returns:
	///     int: (``u64``) The immediate
	///
	/// Raises:
	///     ValueError: If `operand` is invalid or not immediate.
	#[pyo3(text_signature = "($self, operand)")]
	fn immediate(&self, operand: u32) -> PyResult<u64> {
		self.instr.try_immediate(operand).map_err(to_value_error)
	}

	/// Sets an operand's immediate value
	///
	/// Args:
	///     `operand` (int): Operand number, 0-4
	///     `new_value` (int): (``i32``) Immediate
	///
	/// Raises:
	///     ValueError: If `operand` is invalid or if it's not an immediate operand
	#[pyo3(text_signature = "($self, operand, new_value)")]
	fn set_immediate_i32(&mut self, operand: u32, new_value: i32) -> PyResult<()> {
		self.instr.try_set_immediate_i32(operand, new_value).map_err(to_value_error)
	}

	/// Sets an operand's immediate value
	///
	/// Args:
	///     `operand` (int): Operand number, 0-4
	///     `new_value` (int): (``u32``) Immediate
	///
	/// Raises:
	///     ValueError: If `operand` is invalid or if it's not an immediate operand
	#[pyo3(text_signature = "($self, operand, new_value)")]
	fn set_immediate_u32(&mut self, operand: u32, new_value: u32) -> PyResult<()> {
		self.instr.try_set_immediate_u32(operand, new_value).map_err(to_value_error)
	}

	/// Sets an operand's immediate value
	///
	/// Args:
	///     `operand` (int): Operand number, 0-4
	///     `new_value` (int): (``i64``) Immediate
	///
	/// Raises:
	///     ValueError: If `operand` is invalid or if it's not an immediate operand
	#[pyo3(text_signature = "($self, operand, new_value)")]
	fn set_immediate_i64(&mut self, operand: u32, new_value: i64) -> PyResult<()> {
		self.instr.try_set_immediate_i64(operand, new_value).map_err(to_value_error)
	}

	/// Sets an operand's immediate value
	///
	/// Args:
	///     `operand` (int): Operand number, 0-4
	///     `new_value` (int): (``u64``) Immediate
	///
	/// Raises:
	///     ValueError: If `operand` is invalid or if it's not an immediate operand
	#[pyo3(text_signature = "($self, operand, new_value)")]
	fn set_immediate_u64(&mut self, operand: u32, new_value: u64) -> PyResult<()> {
		self.instr.try_set_immediate_u64(operand, new_value).map_err(to_value_error)
	}

	/// int: (``u8``) Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind :class:`OpKind.IMMEDIATE8`
	#[getter]
	fn immediate8(&self) -> u8 {
		self.instr.immediate8()
	}

	#[setter]
	fn set_immediate8(&mut self, new_value: u8) {
		self.instr.set_immediate8(new_value)
	}

	/// int: (``u8``) Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind :class:`OpKind.IMMEDIATE8_2ND`
	#[getter]
	fn immediate8_2nd(&self) -> u8 {
		self.instr.immediate8_2nd()
	}

	#[setter]
	fn set_immediate8_2nd(&mut self, new_value: u8) {
		self.instr.set_immediate8_2nd(new_value)
	}

	/// int: (``u16``) Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind :class:`OpKind.IMMEDIATE16`
	#[getter]
	fn immediate16(&self) -> u16 {
		self.instr.immediate16()
	}

	#[setter]
	fn set_immediate16(&mut self, new_value: u16) {
		self.instr.set_immediate16(new_value);
	}

	/// int: (``u32``) Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind :class:`OpKind.IMMEDIATE32`
	#[getter]
	fn immediate32(&self) -> u32 {
		self.instr.immediate32()
	}

	#[setter]
	fn set_immediate32(&mut self, new_value: u32) {
		self.instr.set_immediate32(new_value);
	}

	/// int: (``u64``) Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind :class:`OpKind.IMMEDIATE64`
	#[getter]
	fn immediate64(&self) -> u64 {
		self.instr.immediate64()
	}

	#[setter]
	fn set_immediate64(&mut self, new_value: u64) {
		self.instr.set_immediate64(new_value);
	}

	/// int: (``i16``) Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind :class:`OpKind.IMMEDIATE8TO16`
	#[getter]
	fn immediate8to16(&self) -> i16 {
		self.instr.immediate8to16()
	}

	#[setter]
	fn set_immediate8to16(&mut self, new_value: i16) {
		self.instr.set_immediate8to16(new_value)
	}

	/// int: (``i32``) Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind :class:`OpKind.IMMEDIATE8TO32`
	#[getter]
	fn immediate8to32(&self) -> i32 {
		self.instr.immediate8to32()
	}

	#[setter]
	fn set_immediate8to32(&mut self, new_value: i32) {
		self.instr.set_immediate8to32(new_value)
	}

	/// int: (``i64``) Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind :class:`OpKind.IMMEDIATE8TO64`
	#[getter]
	fn immediate8to64(&self) -> i64 {
		self.instr.immediate8to64()
	}

	#[setter]
	fn set_immediate8to64(&mut self, new_value: i64) {
		self.instr.set_immediate8to64(new_value);
	}

	/// int: (``i64``) Gets the operand's immediate value.
	///
	/// Use this method if the operand has kind :class:`OpKind.IMMEDIATE32TO64`
	#[getter]
	fn immediate32to64(&self) -> i64 {
		self.instr.immediate32to64()
	}

	#[setter]
	fn set_immediate32to64(&mut self, new_value: i64) {
		self.instr.set_immediate32to64(new_value);
	}

	/// int: (``u16``) Gets the operand's branch target.
	///
	/// Use this method if the operand has kind :class:`OpKind.NEAR_BRANCH16`
	#[getter]
	fn near_branch16(&self) -> u16 {
		self.instr.near_branch16()
	}

	#[setter]
	fn set_near_branch16(&mut self, new_value: u16) {
		self.instr.set_near_branch16(new_value);
	}

	/// int: (``u32``) Gets the operand's branch target.
	///
	/// Use this method if the operand has kind :class:`OpKind.NEAR_BRANCH32`
	#[getter]
	fn near_branch32(&self) -> u32 {
		self.instr.near_branch32()
	}

	#[setter]
	fn set_near_branch32(&mut self, new_value: u32) {
		self.instr.set_near_branch32(new_value);
	}

	/// int: (``u64``) Gets the operand's branch target.
	///
	/// Use this method if the operand has kind :class:`OpKind.NEAR_BRANCH64`
	#[getter]
	fn near_branch64(&self) -> u64 {
		self.instr.near_branch64()
	}

	#[setter]
	fn set_near_branch64(&mut self, new_value: u64) {
		self.instr.set_near_branch64(new_value);
	}

	/// int: (``u64``) Gets the near branch target if it's a ``CALL``/``JMP``/``Jcc`` near branch instruction
	///
	/// (i.e., if :class:`Instruction.op0_kind` is :class:`OpKind.NEAR_BRANCH16`, :class:`OpKind.NEAR_BRANCH32` or :class:`OpKind.NEAR_BRANCH64`)
	#[getter]
	fn near_branch_target(&self) -> u64 {
		self.instr.near_branch_target()
	}

	/// int: (``u16``) Gets the operand's branch target.
	///
	/// Use this method if the operand has kind :class:`OpKind.FAR_BRANCH16`
	#[getter]
	fn far_branch16(&self) -> u16 {
		self.instr.far_branch16()
	}

	#[setter]
	fn set_far_branch16(&mut self, new_value: u16) {
		self.instr.set_far_branch16(new_value);
	}

	/// int: (``u32``) Gets the operand's branch target.
	///
	/// Use this method if the operand has kind :class:`OpKind.FAR_BRANCH32`
	#[getter]
	fn far_branch32(&self) -> u32 {
		self.instr.far_branch32()
	}

	#[setter]
	fn set_far_branch32(&mut self, new_value: u32) {
		self.instr.set_far_branch32(new_value);
	}

	/// int: (`16``) Gets the operand's branch target selector.
	///
	/// Use this method if the operand has kind :class:`OpKind.FAR_BRANCH16` or :class:`OpKind.FAR_BRANCH32`
	#[getter]
	fn far_branch_selector(&self) -> u16 {
		self.instr.far_branch_selector()
	}

	#[setter]
	fn set_far_branch_selector(&mut self, new_value: u16) {
		self.instr.set_far_branch_selector(new_value);
	}

	/// :class:`Register`: Gets the memory operand's base register (a :class:`Register` enum value) or :class:`Register.NONE` if none.
	///
	/// Use this method if the operand has kind :class:`OpKind.MEMORY`
	#[getter]
	fn memory_base(&self) -> u32 {
		self.instr.memory_base() as u32
	}

	#[setter]
	fn set_memory_base(&mut self, new_value: u32) -> PyResult<()> {
		self.instr.set_memory_base(to_register(new_value)?);
		Ok(())
	}

	/// :class:`Register`: Gets the memory operand's index register (a :class:`Register` enum value) or :class:`Register.NONE` if none.
	///
	/// Use this method if the operand has kind :class:`OpKind.MEMORY`
	#[getter]
	fn memory_index(&self) -> u32 {
		self.instr.memory_index() as u32
	}

	#[setter]
	fn set_memory_index(&mut self, new_value: u32) -> PyResult<()> {
		self.instr.set_memory_index(to_register(new_value)?);
		Ok(())
	}

	/// :class:`Register`: Gets operand #0's register value (a :class:`Register` enum value).
	///
	/// Use this method if operand #0 (:class:`Instruction.op0_kind`) has kind :class:`OpKind.REGISTER`, see :class:`Instruction.op_count` and :class:`Instruction.op_register`
	#[getter]
	fn op0_register(&self) -> u32 {
		self.instr.op0_register() as u32
	}

	#[setter]
	fn set_op0_register(&mut self, new_value: u32) -> PyResult<()> {
		self.instr.set_op0_register(to_register(new_value)?);
		Ok(())
	}

	/// :class:`Register`: Gets operand #1's register value (a :class:`Register` enum value).
	///
	/// Use this method if operand #1 (:class:`Instruction.op0_kind`) has kind :class:`OpKind.REGISTER`, see :class:`Instruction.op_count` and :class:`Instruction.op_register`
	#[getter]
	fn op1_register(&self) -> u32 {
		self.instr.op1_register() as u32
	}

	#[setter]
	fn set_op1_register(&mut self, new_value: u32) -> PyResult<()> {
		self.instr.set_op1_register(to_register(new_value)?);
		Ok(())
	}

	/// :class:`Register`: Gets operand #2's register value (a :class:`Register` enum value).
	///
	/// Use this method if operand #2 (:class:`Instruction.op0_kind`) has kind :class:`OpKind.REGISTER`, see :class:`Instruction.op_count` and :class:`Instruction.op_register`
	#[getter]
	fn op2_register(&self) -> u32 {
		self.instr.op2_register() as u32
	}

	#[setter]
	fn set_op2_register(&mut self, new_value: u32) -> PyResult<()> {
		self.instr.set_op2_register(to_register(new_value)?);
		Ok(())
	}

	/// :class:`Register`: Gets operand #3's register value (a :class:`Register` enum value).
	///
	/// Use this method if operand #3 (:class:`Instruction.op0_kind`) has kind :class:`OpKind.REGISTER`, see :class:`Instruction.op_count` and :class:`Instruction.op_register`
	#[getter]
	fn op3_register(&self) -> u32 {
		self.instr.op3_register() as u32
	}

	#[setter]
	fn set_op3_register(&mut self, new_value: u32) -> PyResult<()> {
		self.instr.set_op3_register(to_register(new_value)?);
		Ok(())
	}

	/// :class:`Register`: Gets operand #4's register value (a :class:`Register` enum value).
	///
	/// Use this method if operand #4 (:class:`Instruction.op0_kind`) has kind :class:`OpKind.REGISTER`, see :class:`Instruction.op_count` and :class:`Instruction.op_register`
	#[getter]
	fn op4_register(&self) -> u32 {
		self.instr.op4_register() as u32
	}

	#[setter]
	fn set_op4_register(&mut self, new_value: u32) -> PyResult<()> {
		self.instr.try_set_op4_register(to_register(new_value)?).map_err(to_value_error)
	}

	/// Gets the operand's register value (a :class:`Register` enum value).
	///
	/// Use this method if the operand has kind :class:`OpKind.REGISTER`
	///
	/// Args:
	///     `operand` (int): Operand number, 0-4
	///
	/// Returns:
	///     :class:`Register`: The operand's register value
	///
	/// Raises:
	///     ValueError: If `operand` is invalid
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # add [rax],ebx
	///     data = b"\x01\x18"
	///     decoder = Decoder(64, data)
	///     instr = decoder.decode()
	///
	///     assert instr.op_count == 2
	///     assert instr.op_kind(0) == OpKind.MEMORY
	///     assert instr.op_kind(1) == OpKind.REGISTER
	///     assert instr.op_register(1) == Register.EBX
	#[pyo3(text_signature = "($self, operand)")]
	fn op_register(&self, operand: u32) -> PyResult<u32> {
		self.instr.try_op_register(operand).map_or_else(|e| Err(to_value_error(e)), |register| Ok(register as u32))
	}

	/// Sets the operand's register value.
	///
	/// Use this method if the operand has kind :class:`OpKind.REGISTER`
	///
	/// Args:
	///     `operand` (int): Operand number, 0-4
	///     `new_value` (:class:`Register`): New value
	///
	/// Raises:
	///     ValueError: If `operand` is invalid
	#[pyo3(text_signature = "($self, operand, new_value)")]
	fn set_op_register(&mut self, operand: u32, new_value: u32) -> PyResult<()> {
		self.instr.try_set_op_register(operand, to_register(new_value)?).map_err(to_value_error)
	}

	/// :class:`Register`: Gets the opmask register (:class:`Register.K1` - :class:`Register.K7`) or :class:`Register.NONE` if none (a :class:`Register` enum value)
	#[getter]
	fn op_mask(&self) -> u32 {
		self.instr.op_mask() as u32
	}

	#[setter]
	fn set_op_mask(&mut self, new_value: u32) -> PyResult<()> {
		self.instr.set_op_mask(to_register(new_value)?);
		Ok(())
	}

	/// bool: Checks if there's an opmask register (:class:`Instruction.op_mask`)
	#[getter]
	fn has_op_mask(&self) -> bool {
		self.instr.has_op_mask()
	}

	/// bool: ``True`` if zeroing-masking, ``False`` if merging-masking.
	///
	/// Only used by most EVEX encoded instructions that use opmask registers.
	#[getter]
	fn zeroing_masking(&self) -> bool {
		self.instr.zeroing_masking()
	}

	#[setter]
	fn set_zeroing_masking(&mut self, new_value: bool) {
		self.instr.set_zeroing_masking(new_value);
	}

	/// bool: ``True`` if merging-masking, ``False`` if zeroing-masking.
	///
	/// Only used by most EVEX encoded instructions that use opmask registers.
	#[getter]
	fn merging_masking(&self) -> bool {
		self.instr.merging_masking()
	}

	#[setter]
	fn set_merging_masking(&mut self, new_value: bool) {
		self.instr.set_merging_masking(new_value);
	}

	/// :class:`RoundingControl`: Gets the rounding control (a :class:`RoundingControl` enum value) or :class:`RoundingControl.NONE` if the instruction doesn't use it.
	///
	/// Note:
	///     SAE is implied but :class:`Instruction.suppress_all_exceptions` still returns ``False``.
	#[getter]
	fn rounding_control(&self) -> u32 {
		self.instr.rounding_control() as u32
	}

	#[setter]
	fn set_rounding_control(&mut self, new_value: u32) -> PyResult<()> {
		self.instr.set_rounding_control(to_rounding_control(new_value)?);
		Ok(())
	}

	/// int: (``u8``) Gets the number of elements in a ``db``/``dw``/``dd``/``dq`` directive.
	///
	/// Can only be called if :class:`Instruction.code` is :class:`Code.DECLAREBYTE`, :class:`Code.DECLAREWORD`, :class:`Code.DECLAREDWORD`, :class:`Code.DECLAREQWORD`
	#[getter]
	fn declare_data_len(&self) -> usize {
		self.instr.declare_data_len()
	}

	#[setter]
	fn set_declare_data_len(&mut self, new_value: usize) {
		self.instr.set_declare_data_len(new_value);
	}

	/// Sets a new ``db`` value, see also :class:`Instruction.declare_data_len`.
	///
	/// Can only be called if :class:`Instruction.code` is :class:`Code.DECLAREBYTE`
	///
	/// Args:
	///     `index` (int): Index (0-15)
	///     `new_value` (int): (``i8``) New value
	///
	/// Raises:
	///     ValueError: If `index` is invalid
	#[pyo3(text_signature = "($self, index, new_value)")]
	fn set_declare_byte_value_i8(&mut self, index: usize, new_value: i8) -> PyResult<()> {
		self.instr.try_set_declare_byte_value_i8(index, new_value).map_err(to_value_error)
	}

	/// Sets a new ``db`` value, see also :class:`Instruction.declare_data_len`.
	///
	/// Can only be called if :class:`Instruction.code` is :class:`Code.DECLAREBYTE`
	///
	/// Args:
	///     `index` (int): Index (0-15)
	///     `new_value` (int): (``u8``) New value
	///
	/// Raises:
	///     ValueError: If `index` is invalid
	#[pyo3(text_signature = "($self, index, new_value)")]
	fn set_declare_byte_value(&mut self, index: usize, new_value: u8) -> PyResult<()> {
		self.instr.try_set_declare_byte_value(index, new_value).map_err(to_value_error)
	}

	/// Gets a ``db`` value, see also :class:`Instruction.declare_data_len`.
	///
	/// Can only be called if :class:`Instruction.code` is :class:`Code.DECLAREBYTE`
	///
	/// Args:
	///     `index` (int): Index (0-15)
	///
	/// Returns:
	///     int: (``u8``) The value
	///
	/// Raises:
	///     ValueError: If `index` is invalid
	#[pyo3(text_signature = "($self, index)")]
	fn get_declare_byte_value(&self, index: usize) -> PyResult<u8> {
		let value = self.instr.try_get_declare_byte_value(index).map_err(to_value_error)?;
		Ok(value)
	}

	/// Gets a ``db`` value, see also :class:`Instruction.declare_data_len`.
	///
	/// Can only be called if :class:`Instruction.code` is :class:`Code.DECLAREBYTE`
	///
	/// Args:
	///     `index` (int): Index (0-15)
	///
	/// Returns:
	///     int: (``i8``) The value
	///
	/// Raises:
	///     ValueError: If `index` is invalid
	#[pyo3(text_signature = "($self, index)")]
	fn get_declare_byte_value_i8(&self, index: usize) -> PyResult<i8> {
		let value = self.instr.try_get_declare_byte_value(index).map_err(to_value_error)?;
		Ok(value as i8)
	}

	/// Sets a new ``dw`` value, see also :class:`Instruction.declare_data_len`.
	///
	/// Can only be called if :class:`Instruction.code` is :class:`Code.DECLAREWORD`
	///
	/// Args:
	///     `index` (int): Index (0-7)
	///     `new_value` (int): (``i16``) New value
	///
	/// Raises:
	///     ValueError: If `index` is invalid
	#[pyo3(text_signature = "($self, index, new_value)")]
	fn set_declare_word_value_i16(&mut self, index: usize, new_value: i16) -> PyResult<()> {
		self.instr.try_set_declare_word_value_i16(index, new_value).map_err(to_value_error)
	}

	/// Sets a new ``dw`` value, see also :class:`Instruction.declare_data_len`.
	///
	/// Can only be called if :class:`Instruction.code` is :class:`Code.DECLAREWORD`
	///
	/// Args:
	///     `index` (int): Index (0-7)
	///     `new_value` (int): (``u16``) New value
	///
	/// Raises:
	///     ValueError: If `index` is invalid
	#[pyo3(text_signature = "($self, index, new_value)")]
	fn set_declare_word_value(&mut self, index: usize, new_value: u16) -> PyResult<()> {
		self.instr.try_set_declare_word_value(index, new_value).map_err(to_value_error)
	}

	/// Gets a ``dw`` value, see also :class:`Instruction.declare_data_len`.
	///
	/// Can only be called if :class:`Instruction.code` is :class:`Code.DECLAREWORD`
	///
	/// Args:
	///     `index` (int): Index (0-7)
	///
	/// Returns:
	///     int: (``u16``) The value
	///
	/// Raises:
	///     ValueError: If `index` is invalid
	#[pyo3(text_signature = "($self, index)")]
	fn get_declare_word_value(&self, index: usize) -> PyResult<u16> {
		let value = self.instr.try_get_declare_word_value(index).map_err(to_value_error)?;
		Ok(value)
	}

	/// Gets a ``dw`` value, see also :class:`Instruction.declare_data_len`.
	///
	/// Can only be called if :class:`Instruction.code` is :class:`Code.DECLAREWORD`
	///
	/// Args:
	///     `index` (int): Index (0-7)
	///
	/// Returns:
	///     int: (``i16``) The value
	///
	/// Raises:
	///     ValueError: If `index` is invalid
	#[pyo3(text_signature = "($self, index)")]
	fn get_declare_word_value_i16(&self, index: usize) -> PyResult<i16> {
		let value = self.instr.try_get_declare_word_value(index).map_err(to_value_error)?;
		Ok(value as i16)
	}

	/// Sets a new ``dd`` value, see also :class:`Instruction.declare_data_len`.
	///
	/// Can only be called if :class:`Instruction.code` is :class:`Code.DECLAREDWORD`
	///
	/// Args:
	///     `index` (int): Index (0-3)
	///     `new_value` (int): (``i32``) New value
	///
	/// Raises:
	///     ValueError: If `index` is invalid
	#[pyo3(text_signature = "($self, index, new_value)")]
	fn set_declare_dword_value_i32(&mut self, index: usize, new_value: i32) -> PyResult<()> {
		self.instr.try_set_declare_dword_value_i32(index, new_value).map_err(to_value_error)
	}

	/// Sets a new ``dd`` value, see also :class:`Instruction.declare_data_len`.
	///
	/// Can only be called if :class:`Instruction.code` is :class:`Code.DECLAREDWORD`
	///
	/// Args:
	///     `index` (int): Index (0-3)
	///     `new_value` (int): (``u32``) New value
	///
	/// Raises:
	///     ValueError: If `index` is invalid
	#[pyo3(text_signature = "($self, index, new_value)")]
	fn set_declare_dword_value(&mut self, index: usize, new_value: u32) -> PyResult<()> {
		self.instr.try_set_declare_dword_value(index, new_value).map_err(to_value_error)
	}

	/// Gets a ``dd`` value, see also :class:`Instruction.declare_data_len`.
	///
	/// Can only be called if :class:`Instruction.code` is :class:`Code.DECLAREDWORD`
	///
	/// Args:
	///     `index` (int): Index (0-3)
	///
	/// Returns:
	///     int: (``u32``) The value
	///
	/// Raises:
	///     ValueError: If `index` is invalid
	#[pyo3(text_signature = "($self, index)")]
	fn get_declare_dword_value(&self, index: usize) -> PyResult<u32> {
		let value = self.instr.try_get_declare_dword_value(index).map_err(to_value_error)?;
		Ok(value)
	}

	/// Gets a ``dd`` value, see also :class:`Instruction.declare_data_len`.
	///
	/// Can only be called if :class:`Instruction.code` is :class:`Code.DECLAREDWORD`
	///
	/// Args:
	///     `index` (int): Index (0-3)
	///
	/// Returns:
	///     int: (``i32``) The value
	///
	/// Raises:
	///     ValueError: If `index` is invalid
	#[pyo3(text_signature = "($self, index)")]
	fn get_declare_dword_value_i32(&self, index: usize) -> PyResult<i32> {
		let value = self.instr.try_get_declare_dword_value(index).map_err(to_value_error)?;
		Ok(value as i32)
	}

	/// Sets a new ``dq`` value, see also :class:`Instruction.declare_data_len`.
	///
	/// Can only be called if :class:`Instruction.code` is :class:`Code.DECLAREQWORD`
	///
	/// Args:
	///     `index` (int): Index (0-1)
	///     `new_value` (int): (``i64``) New value
	///
	/// Raises:
	///     ValueError: If `index` is invalid
	#[pyo3(text_signature = "($self, index, new_value)")]
	fn set_declare_qword_value_i64(&mut self, index: usize, new_value: i64) -> PyResult<()> {
		self.instr.try_set_declare_qword_value_i64(index, new_value).map_err(to_value_error)
	}

	/// Sets a new ``dq`` value, see also :class:`Instruction.declare_data_len`.
	///
	/// Can only be called if :class:`Instruction.code` is :class:`Code.DECLAREQWORD`
	///
	/// Args:
	///     `index` (int): Index (0-1)
	///     `new_value` (int): (``u64``) New value
	///
	/// Raises:
	///     ValueError: If `index` is invalid
	#[pyo3(text_signature = "($self, index, new_value)")]
	fn set_declare_qword_value(&mut self, index: usize, new_value: u64) -> PyResult<()> {
		self.instr.try_set_declare_qword_value(index, new_value).map_err(to_value_error)
	}

	/// Gets a ``dq`` value, see also :class:`Instruction.declare_data_len`.
	///
	/// Can only be called if :class:`Instruction.code` is :class:`Code.DECLAREQWORD`
	///
	/// Args:
	///     `index` (int): Index (0-1)
	///
	/// Returns:
	///     int: (``u64``) The value
	///
	/// Raises:
	///     ValueError: If `index` is invalid
	#[pyo3(text_signature = "($self, index)")]
	fn get_declare_qword_value(&self, index: usize) -> PyResult<u64> {
		let value = self.instr.try_get_declare_qword_value(index).map_err(to_value_error)?;
		Ok(value)
	}

	/// Gets a ``dq`` value, see also :class:`Instruction.declare_data_len`.
	///
	/// Can only be called if :class:`Instruction.code` is :class:`Code.DECLAREQWORD`
	///
	/// Args:
	///     `index` (int): Index (0-1)
	///
	/// Returns:
	///     int: (``i64``) The value
	///
	/// Raises:
	///     ValueError: If `index` is invalid
	#[pyo3(text_signature = "($self, index)")]
	fn get_declare_qword_value_i64(&self, index: usize) -> PyResult<i64> {
		let value = self.instr.try_get_declare_qword_value(index).map_err(to_value_error)?;
		Ok(value as i64)
	}

	/// bool: Checks if this is a VSIB instruction, see also :class:`Instruction.is_vsib32`, :class:`Instruction.is_vsib64`
	#[getter]
	fn is_vsib(&self) -> bool {
		self.instr.is_vsib()
	}

	/// bool: VSIB instructions only (:class:`Instruction.is_vsib`): ``True`` if it's using 32-bit indexes, ``False`` if it's using 64-bit indexes
	#[getter]
	fn is_vsib32(&self) -> bool {
		self.instr.is_vsib32()
	}

	/// bool: VSIB instructions only (:class:`Instruction.is_vsib`): ``True`` if it's using 64-bit indexes, ``False`` if it's using 32-bit indexes
	#[getter]
	fn is_vsib64(&self) -> bool {
		self.instr.is_vsib64()
	}

	/// bool, None: Checks if it's a vsib instruction.
	///
	/// - Returns ``True`` if it's a VSIB instruction with 64-bit indexes
	/// - Returns ``False`` if it's a VSIB instruction with 32-bit indexes
	/// - Returns `None` if it's not a VSIB instruction.
	#[getter]
	fn vsib(&self) -> Option<bool> {
		self.instr.vsib()
	}

	/// bool: Gets the suppress all exceptions flag (EVEX/MVEX encoded instructions). Note that if :class:`Instruction.rounding_control` is not :class:`RoundingControl.NONE`, SAE is implied but this method will still return ``False``.
	#[getter]
	fn suppress_all_exceptions(&self) -> bool {
		self.instr.suppress_all_exceptions()
	}

	#[setter]
	fn set_suppress_all_exceptions(&mut self, new_value: bool) {
		self.instr.set_suppress_all_exceptions(new_value);
	}

	/// bool: Checks if the memory operand is ``RIP``/``EIP`` relative
	#[getter]
	fn is_ip_rel_memory_operand(&self) -> bool {
		self.instr.is_ip_rel_memory_operand()
	}

	/// int: (``u64``) Gets the ``RIP``/``EIP`` releative address (:class:`Instruction.memory_displacement`).
	///
	/// This method is only valid if there's a memory operand with ``RIP``/``EIP`` relative addressing, see :class:`Instruction.is_ip_rel_memory_operand`
	#[getter]
	fn ip_rel_memory_address(&self) -> u64 {
		self.instr.ip_rel_memory_address()
	}

	/// int: (``i32``) Gets the number of bytes added to ``SP``/``ESP``/``RSP`` or 0 if it's not an instruction that pushes or pops data.
	///
	/// This method assumes the instruction doesn't change the privilege level (eg. ``IRET/D/Q``). If it's the ``LEAVE``
	/// instruction, this method returns 0.
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # pushfq
	///     data = b"\x9C"
	///     decoder = Decoder(64, data)
	///     instr = decoder.decode()
	///
	///     assert instr.is_stack_instruction
	///     assert instr.stack_pointer_increment == -8
	#[getter]
	fn stack_pointer_increment(&self) -> i32 {
		self.instr.stack_pointer_increment()
	}

	/// Gets the FPU status word's ``TOP`` increment value and whether it's a conditional or unconditional push/pop and whether ``TOP`` is written.
	///
	/// Returns:
	///     :class:`FpuStackIncrementInfo`: FPU stack info
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # ficomp dword ptr [rax]
	///     data = b"\xDA\x18"
	///     decoder = Decoder(64, data)
	///     instr = decoder.decode()
	///
	///     info = instr.fpu_stack_increment_info()
	///     # It pops the stack once
	///     assert info.increment == 1
	///     assert not info.conditional
	///     assert info.writes_top
	#[pyo3(text_signature = "($self)")]
	fn fpu_stack_increment_info(&self) -> FpuStackIncrementInfo {
		FpuStackIncrementInfo { info: self.instr.fpu_stack_increment_info() }
	}

	/// :class:`EncodingKind`: Instruction encoding, eg. Legacy, 3DNow!, VEX, EVEX, XOP (an :class:`EncodingKind` enum value)
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # vmovaps xmm1,xmm5
	///     data = b"\xC5\xF8\x28\xCD"
	///     decoder = Decoder(64, data)
	///     instr = decoder.decode()
	///
	///     assert instr.encoding == EncodingKind.VEX
	#[getter]
	fn encoding(&self) -> u32 {
		self.instr.encoding() as u32
	}

	/// Gets the CPU or CPUID feature flags (a list of :class:`CpuidFeature` enum values)
	///
	/// Returns:
	///     List[:class:`CpuidFeature`]: CPU or CPUID feature flags
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # vmovaps xmm1,xmm5
	///     # vmovaps xmm10{k3}{z},xmm19
	///     data = b"\xC5\xF8\x28\xCD\x62\x31\x7C\x8B\x28\xD3"
	///     decoder = Decoder(64, data)
	///
	///     # vmovaps xmm1,xmm5
	///     instr = decoder.decode()
	///     cpuid = instr.cpuid_features()
	///     assert len(cpuid) == 1
	///     assert cpuid[0] == CpuidFeature.AVX
	///
	///     # vmovaps xmm10{k3}{z},xmm19
	///     instr = decoder.decode()
	///     cpuid = instr.cpuid_features()
	///     assert len(cpuid) == 2
	///     assert cpuid[0] == CpuidFeature.AVX512VL
	///     assert cpuid[1] == CpuidFeature.AVX512F
	#[pyo3(text_signature = "($self)")]
	fn cpuid_features(&self) -> Vec<u32> {
		self.instr.cpuid_features().iter().map(|x| *x as u32).collect()
	}

	/// :class:`FlowControl`: Control flow info (a :class:`FlowControl` enum value)
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # or ecx,esi
	///     # ud0 rcx,rsi
	///     # call rcx
	///     data = b"\x0B\xCE\x48\x0F\xFF\xCE\xFF\xD1"
	///     decoder = Decoder(64, data)
	///
	///     # or ecx,esi
	///     instr = decoder.decode()
	///     assert instr.flow_control == FlowControl.NEXT
	///
	///     # ud0 rcx,rsi
	///     instr = decoder.decode()
	///     assert instr.flow_control == FlowControl.EXCEPTION
	///
	///     # call rcx
	///     instr = decoder.decode()
	///     assert instr.flow_control == FlowControl.INDIRECT_CALL
	#[getter]
	fn flow_control(&self) -> u32 {
		self.instr.flow_control() as u32
	}

	/// bool: ``True`` if it's a privileged instruction (all CPL=0 instructions (except ``VMCALL``) and IOPL instructions ``IN``, ``INS``, ``OUT``, ``OUTS``, ``CLI``, ``STI``)
	#[getter]
	fn is_privileged(&self) -> bool {
		self.instr.is_privileged()
	}

	/// bool: ``True`` if this is an instruction that implicitly uses the stack pointer (``SP``/``ESP``/``RSP``), eg. ``CALL``, ``PUSH``, ``POP``, ``RET``, etc.
	///
	/// See also :class:`Instruction.stack_pointer_increment`
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # or ecx,esi
	///     # push rax
	///     data = b"\x0B\xCE\x50"
	///     decoder = Decoder(64, data)
	///
	///     # or ecx,esi
	///     instr = decoder.decode()
	///     assert not instr.is_stack_instruction
	///
	///     # push rax
	///     instr = decoder.decode()
	///     assert instr.is_stack_instruction
	///     assert instr.stack_pointer_increment == -8
	#[getter]
	fn is_stack_instruction(&self) -> bool {
		self.instr.is_stack_instruction()
	}

	/// bool: ``True`` if it's an instruction that saves or restores too many registers (eg. ``FXRSTOR``, ``XSAVE``, etc).
	#[getter]
	fn is_save_restore_instruction(&self) -> bool {
		self.instr.is_save_restore_instruction()
	}

	/// bool: ``True`` if it's a "string" instruction, such as ``MOVS``, ``LODS``, ``SCAS``, etc.
	#[getter]
	fn is_string_instruction(&self) -> bool {
		self.instr.is_string_instruction()
	}

	/// :class:`RflagsBits`: All flags that are read by the CPU when executing the instruction.
	///
	/// This method returns an :class:`RflagsBits` value. See also :class:`Instruction.rflags_modified`.
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # adc rsi,rcx
	///     # xor rdi,5Ah
	///     data = b"\x48\x11\xCE\x48\x83\xF7\x5A"
	///     decoder = Decoder(64, data)
	///
	///     # adc rsi,rcx
	///     instr = decoder.decode()
	///     assert instr.rflags_read == RflagsBits.CF
	///     assert instr.rflags_written == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	///     assert instr.rflags_cleared == RflagsBits.NONE
	///     assert instr.rflags_set == RflagsBits.NONE
	///     assert instr.rflags_undefined == RflagsBits.NONE
	///     assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	///
	///     # xor rdi,5Ah
	///     instr = decoder.decode()
	///     assert instr.rflags_read == RflagsBits.NONE
	///     assert instr.rflags_written == RflagsBits.SF | RflagsBits.ZF | RflagsBits.PF
	///     assert instr.rflags_cleared == RflagsBits.OF | RflagsBits.CF
	///     assert instr.rflags_set == RflagsBits.NONE
	///     assert instr.rflags_undefined == RflagsBits.AF
	///     assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	#[getter]
	fn rflags_read(&self) -> u32 {
		self.instr.rflags_read()
	}

	/// :class:`RflagsBits`: All flags that are written by the CPU, except those flags that are known to be undefined, always set or always cleared.
	///
	/// This method returns an :class:`RflagsBits` value. See also :class:`Instruction.rflags_modified`.
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # adc rsi,rcx
	///     # xor rdi,5Ah
	///     data = b"\x48\x11\xCE\x48\x83\xF7\x5A"
	///     decoder = Decoder(64, data)
	///
	///     # adc rsi,rcx
	///     instr = decoder.decode()
	///     assert instr.rflags_read == RflagsBits.CF
	///     assert instr.rflags_written == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	///     assert instr.rflags_cleared == RflagsBits.NONE
	///     assert instr.rflags_set == RflagsBits.NONE
	///     assert instr.rflags_undefined == RflagsBits.NONE
	///     assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	///
	///     # xor rdi,5Ah
	///     instr = decoder.decode()
	///     assert instr.rflags_read == RflagsBits.NONE
	///     assert instr.rflags_written == RflagsBits.SF | RflagsBits.ZF | RflagsBits.PF
	///     assert instr.rflags_cleared == RflagsBits.OF | RflagsBits.CF
	///     assert instr.rflags_set == RflagsBits.NONE
	///     assert instr.rflags_undefined == RflagsBits.AF
	///     assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	#[getter]
	fn rflags_written(&self) -> u32 {
		self.instr.rflags_written()
	}

	/// :class:`RflagsBits`: All flags that are always cleared by the CPU.
	///
	/// This method returns an :class:`RflagsBits` value. See also :class:`Instruction.rflags_modified`.
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # adc rsi,rcx
	///     # xor rdi,5Ah
	///     data = b"\x48\x11\xCE\x48\x83\xF7\x5A"
	///     decoder = Decoder(64, data)
	///
	///     # adc rsi,rcx
	///     instr = decoder.decode()
	///     assert instr.rflags_read == RflagsBits.CF
	///     assert instr.rflags_written == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	///     assert instr.rflags_cleared == RflagsBits.NONE
	///     assert instr.rflags_set == RflagsBits.NONE
	///     assert instr.rflags_undefined == RflagsBits.NONE
	///     assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	///
	///     # xor rdi,5Ah
	///     instr = decoder.decode()
	///     assert instr.rflags_read == RflagsBits.NONE
	///     assert instr.rflags_written == RflagsBits.SF | RflagsBits.ZF | RflagsBits.PF
	///     assert instr.rflags_cleared == RflagsBits.OF | RflagsBits.CF
	///     assert instr.rflags_set == RflagsBits.NONE
	///     assert instr.rflags_undefined == RflagsBits.AF
	///     assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	#[getter]
	fn rflags_cleared(&self) -> u32 {
		self.instr.rflags_cleared()
	}

	/// :class:`RflagsBits`: All flags that are always set by the CPU.
	///
	/// This method returns an :class:`RflagsBits` value. See also :class:`Instruction.rflags_modified`.
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # adc rsi,rcx
	///     # xor rdi,5Ah
	///     data = b"\x48\x11\xCE\x48\x83\xF7\x5A"
	///     decoder = Decoder(64, data)
	///
	///     # adc rsi,rcx
	///     instr = decoder.decode()
	///     assert instr.rflags_read == RflagsBits.CF
	///     assert instr.rflags_written == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	///     assert instr.rflags_cleared == RflagsBits.NONE
	///     assert instr.rflags_set == RflagsBits.NONE
	///     assert instr.rflags_undefined == RflagsBits.NONE
	///     assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	///
	///     # xor rdi,5Ah
	///     instr = decoder.decode()
	///     assert instr.rflags_read == RflagsBits.NONE
	///     assert instr.rflags_written == RflagsBits.SF | RflagsBits.ZF | RflagsBits.PF
	///     assert instr.rflags_cleared == RflagsBits.OF | RflagsBits.CF
	///     assert instr.rflags_set == RflagsBits.NONE
	///     assert instr.rflags_undefined == RflagsBits.AF
	///     assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	#[getter]
	fn rflags_set(&self) -> u32 {
		self.instr.rflags_set()
	}

	/// :class:`RflagsBits`: All flags that are undefined after executing the instruction.
	///
	/// This method returns an :class:`RflagsBits` value. See also :class:`Instruction.rflags_modified`.
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # adc rsi,rcx
	///     # xor rdi,5Ah
	///     data = b"\x48\x11\xCE\x48\x83\xF7\x5A"
	///     decoder = Decoder(64, data)
	///
	///     # adc rsi,rcx
	///     instr = decoder.decode()
	///     assert instr.rflags_read == RflagsBits.CF
	///     assert instr.rflags_written == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	///     assert instr.rflags_cleared == RflagsBits.NONE
	///     assert instr.rflags_set == RflagsBits.NONE
	///     assert instr.rflags_undefined == RflagsBits.NONE
	///     assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	///
	///     # xor rdi,5Ah
	///     instr = decoder.decode()
	///     assert instr.rflags_read == RflagsBits.NONE
	///     assert instr.rflags_written == RflagsBits.SF | RflagsBits.ZF | RflagsBits.PF
	///     assert instr.rflags_cleared == RflagsBits.OF | RflagsBits.CF
	///     assert instr.rflags_set == RflagsBits.NONE
	///     assert instr.rflags_undefined == RflagsBits.AF
	///     assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	#[getter]
	fn rflags_undefined(&self) -> u32 {
		self.instr.rflags_undefined()
	}

	/// :class:`RflagsBits`: All flags that are modified by the CPU. This is ``rflags_written + rflags_cleared + rflags_set + rflags_undefined``.
	///
	/// This method returns an :class:`RflagsBits` value.
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # adc rsi,rcx
	///     # xor rdi,5Ah
	///     data = b"\x48\x11\xCE\x48\x83\xF7\x5A"
	///     decoder = Decoder(64, data)
	///
	///     # adc rsi,rcx
	///     instr = decoder.decode()
	///     assert instr.rflags_read == RflagsBits.CF
	///     assert instr.rflags_written == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	///     assert instr.rflags_cleared == RflagsBits.NONE
	///     assert instr.rflags_set == RflagsBits.NONE
	///     assert instr.rflags_undefined == RflagsBits.NONE
	///     assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	///
	///     # xor rdi,5Ah
	///     instr = decoder.decode()
	///     assert instr.rflags_read == RflagsBits.NONE
	///     assert instr.rflags_written == RflagsBits.SF | RflagsBits.ZF | RflagsBits.PF
	///     assert instr.rflags_cleared == RflagsBits.OF | RflagsBits.CF
	///     assert instr.rflags_set == RflagsBits.NONE
	///     assert instr.rflags_undefined == RflagsBits.AF
	///     assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF
	#[getter]
	fn rflags_modified(&self) -> u32 {
		self.instr.rflags_modified()
	}

	/// bool: Checks if it's a ``Jcc SHORT`` or ``Jcc NEAR`` instruction
	#[getter]
	fn is_jcc_short_or_near(&self) -> bool {
		self.instr.is_jcc_short_or_near()
	}

	/// bool: Checks if it's a ``Jcc NEAR`` instruction
	#[getter]
	fn is_jcc_near(&self) -> bool {
		self.instr.is_jcc_near()
	}

	/// bool: Checks if it's a ``Jcc SHORT`` instruction
	#[getter]
	fn is_jcc_short(&self) -> bool {
		self.instr.is_jcc_short()
	}

	/// bool: Checks if it's a ``JMP SHORT`` instruction
	#[getter]
	fn is_jmp_short(&self) -> bool {
		self.instr.is_jmp_short()
	}

	/// bool: Checks if it's a ``JMP NEAR`` instruction
	#[getter]
	fn is_jmp_near(&self) -> bool {
		self.instr.is_jmp_near()
	}

	/// bool: Checks if it's a ``JMP SHORT`` or a ``JMP NEAR`` instruction
	#[getter]
	fn is_jmp_short_or_near(&self) -> bool {
		self.instr.is_jmp_short_or_near()
	}

	/// bool: Checks if it's a ``JMP FAR`` instruction
	#[getter]
	fn is_jmp_far(&self) -> bool {
		self.instr.is_jmp_far()
	}

	/// bool: Checks if it's a ``CALL NEAR`` instruction
	#[getter]
	fn is_call_near(&self) -> bool {
		self.instr.is_call_near()
	}

	/// bool: Checks if it's a ``CALL FAR`` instruction
	#[getter]
	fn is_call_far(&self) -> bool {
		self.instr.is_call_far()
	}

	/// bool: Checks if it's a ``JMP NEAR reg/[mem]`` instruction
	#[getter]
	fn is_jmp_near_indirect(&self) -> bool {
		self.instr.is_jmp_near_indirect()
	}

	/// bool: Checks if it's a ``JMP FAR [mem]`` instruction
	#[getter]
	fn is_jmp_far_indirect(&self) -> bool {
		self.instr.is_jmp_far_indirect()
	}

	/// bool: Checks if it's a ``CALL NEAR reg/[mem]`` instruction
	#[getter]
	fn is_call_near_indirect(&self) -> bool {
		self.instr.is_call_near_indirect()
	}

	/// bool: Checks if it's a ``CALL FAR [mem]`` instruction
	#[getter]
	fn is_call_far_indirect(&self) -> bool {
		self.instr.is_call_far_indirect()
	}

	/// bool: Checks if it's a ``JKccD SHORT`` or ``JKccD NEAR`` instruction
	#[getter]
	fn is_jkcc_short_or_near(&self) -> bool {
		self.instr.is_jkcc_short_or_near()
	}

	/// bool: Checks if it's a ``JKccD NEAR`` instruction
	#[getter]
	fn is_jkcc_near(&self) -> bool {
		self.instr.is_jkcc_near()
	}

	/// bool: Checks if it's a ``JKccD SHORT`` instruction
	#[getter]
	fn is_jkcc_short(&self) -> bool {
		self.instr.is_jkcc_short()
	}

	/// bool: Checks if it's a ``JCXZ SHORT``, ``JECXZ SHORT`` or ``JRCXZ SHORT`` instruction
	#[getter]
	pub fn is_jcx_short(&self) -> bool {
		self.instr.code().is_jcx_short()
	}

	/// bool: Checks if it's a ``LOOPcc SHORT`` instruction
	#[getter]
	pub fn is_loopcc(&self) -> bool {
		self.instr.code().is_loopcc()
	}

	/// bool: Checks if it's a ``LOOP SHORT`` instruction
	#[getter]
	pub fn is_loop(&self) -> bool {
		self.instr.code().is_loop()
	}

	/// Negates the condition code, eg. ``JE`` -> ``JNE``.
	///
	/// Can be used if it's ``Jcc``, ``SETcc``, ``CMOVcc``, ``CMPccXADD``, ``LOOPcc`` and does nothing if the instruction doesn't have a condition code.
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # setbe al
	///     data = b"\x0F\x96\xC0"
	///     decoder = Decoder(64, data)
	///
	///     instr = decoder.decode()
	///     assert instr.code == Code.SETBE_RM8
	///     assert instr.condition_code == ConditionCode.BE
	///     instr.negate_condition_code()
	///     assert instr.code == Code.SETA_RM8
	///     assert instr.condition_code == ConditionCode.A
	#[pyo3(text_signature = "($self)")]
	fn negate_condition_code(&mut self) {
		self.instr.negate_condition_code()
	}

	/// Converts ``Jcc/JMP NEAR`` to ``Jcc/JMP SHORT`` and does nothing if it's not a ``Jcc/JMP NEAR`` instruction
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # jbe near ptr label
	///     data = b"\x0F\x86\x5A\xA5\x12\x34"
	///     decoder = Decoder(64, data)
	///
	///     instr = decoder.decode()
	///     assert instr.code == Code.JBE_REL32_64
	///     instr.as_short_branch()
	///     assert instr.code == Code.JBE_REL8_64
	///     instr.as_short_branch()
	///     assert instr.code == Code.JBE_REL8_64
	#[pyo3(text_signature = "($self)")]
	fn as_short_branch(&mut self) {
		self.instr.as_short_branch()
	}

	/// Converts ``Jcc/JMP SHORT`` to ``Jcc/JMP NEAR`` and does nothing if it's not a ``Jcc/JMP SHORT`` instruction
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # jbe short label
	///     data = b"\x76\x5A"
	///     decoder = Decoder(64, data)
	///
	///     instr = decoder.decode()
	///     assert instr.code == Code.JBE_REL8_64
	///     instr.as_near_branch()
	///     assert instr.code == Code.JBE_REL32_64
	///     instr.as_near_branch()
	///     assert instr.code == Code.JBE_REL32_64
	#[pyo3(text_signature = "($self)")]
	fn as_near_branch(&mut self) {
		self.instr.as_near_branch()
	}

	/// :class:`ConditionCode`: Gets the condition code (a :class:`ConditionCode` enum value) if it's ``Jcc``, ``SETcc``, ``CMOVcc``, ``CMPccXADD``, ``LOOPcc`` else :class:`ConditionCode.NONE` is returned
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # setbe al
	///     # jl short label
	///     # cmovne ecx,esi
	///     # nop
	///     data = b"\x0F\x96\xC0\x7C\x5A\x0F\x45\xCE\x90"
	///     decoder = Decoder(64, data)
	///
	///     # setbe al
	///     instr = decoder.decode()
	///     assert instr.condition_code == ConditionCode.BE
	///
	///     # jl short label
	///     instr = decoder.decode()
	///     assert instr.condition_code == ConditionCode.L
	///
	///     # cmovne ecx,esi
	///     instr = decoder.decode()
	///     assert instr.condition_code == ConditionCode.NE
	///
	///     # nop
	///     instr = decoder.decode()
	///     assert instr.condition_code == ConditionCode.NONE
	#[getter]
	fn condition_code(&self) -> u32 {
		self.instr.condition_code() as u32
	}

	/// Gets the :class:`OpCodeInfo`
	///
	/// Returns:
	///     :class:`OpCodeInfo`: Op code info
	#[pyo3(text_signature = "($self)")]
	fn op_code(&self) -> PyResult<OpCodeInfo> {
		OpCodeInfo::new(self.instr.code() as u32)
	}

	// GENERATOR-BEGIN: Create
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	/// Creates an instruction with no operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code)")]
	fn create(code: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		Ok(Instruction { instr: iced_x86::Instruction::with(code) })
	}

	/// Creates an instruction with 1 operand
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register` (:class:`Register`): op0: Register
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register)")]
	fn create_reg(code: u32, register: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register = to_register(register)?;
		Ok(Instruction { instr: iced_x86::Instruction::with1(code, register).map_err(to_value_error)? })
	}

	/// Creates an instruction with 1 operand
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `immediate` (int): (``i32``) op0: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, immediate)")]
	fn create_i32(code: u32, immediate: i32) -> PyResult<Self> {
		let code = to_code(code)?;
		Ok(Instruction { instr: iced_x86::Instruction::with1(code, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 1 operand
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `immediate` (int): (``u32``) op0: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, immediate)")]
	fn create_u32(code: u32, immediate: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		Ok(Instruction { instr: iced_x86::Instruction::with1(code, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 1 operand
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `memory` (:class:`MemoryOperand`): op0: Memory operand
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, memory)")]
	fn create_mem(code: u32, memory: MemoryOperand) -> PyResult<Self> {
		let code = to_code(code)?;
		Ok(Instruction { instr: iced_x86::Instruction::with1(code, memory.mem).map_err(to_value_error)? })
	}

	/// Creates an instruction with 2 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register1` (:class:`Register`): op0: Register
	///     `register2` (:class:`Register`): op1: Register
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register1, register2)")]
	fn create_reg_reg(code: u32, register1: u32, register2: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		Ok(Instruction { instr: iced_x86::Instruction::with2(code, register1, register2).map_err(to_value_error)? })
	}

	/// Creates an instruction with 2 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register` (:class:`Register`): op0: Register
	///     `immediate` (int): (``i32``) op1: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register, immediate)")]
	fn create_reg_i32(code: u32, register: u32, immediate: i32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register = to_register(register)?;
		Ok(Instruction { instr: iced_x86::Instruction::with2(code, register, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 2 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register` (:class:`Register`): op0: Register
	///     `immediate` (int): (``u32``) op1: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register, immediate)")]
	fn create_reg_u32(code: u32, register: u32, immediate: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register = to_register(register)?;
		Ok(Instruction { instr: iced_x86::Instruction::with2(code, register, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 2 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register` (:class:`Register`): op0: Register
	///     `immediate` (int): (``i64``) op1: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register, immediate)")]
	fn create_reg_i64(code: u32, register: u32, immediate: i64) -> PyResult<Self> {
		let code = to_code(code)?;
		let register = to_register(register)?;
		Ok(Instruction { instr: iced_x86::Instruction::with2(code, register, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 2 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register` (:class:`Register`): op0: Register
	///     `immediate` (int): (``u64``) op1: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register, immediate)")]
	fn create_reg_u64(code: u32, register: u32, immediate: u64) -> PyResult<Self> {
		let code = to_code(code)?;
		let register = to_register(register)?;
		Ok(Instruction { instr: iced_x86::Instruction::with2(code, register, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 2 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register` (:class:`Register`): op0: Register
	///     `memory` (:class:`MemoryOperand`): op1: Memory operand
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register, memory)")]
	fn create_reg_mem(code: u32, register: u32, memory: MemoryOperand) -> PyResult<Self> {
		let code = to_code(code)?;
		let register = to_register(register)?;
		Ok(Instruction { instr: iced_x86::Instruction::with2(code, register, memory.mem).map_err(to_value_error)? })
	}

	/// Creates an instruction with 2 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `immediate` (int): (``i32``) op0: Immediate value
	///     `register` (:class:`Register`): op1: Register
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, immediate, register)")]
	fn create_i32_reg(code: u32, immediate: i32, register: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register = to_register(register)?;
		Ok(Instruction { instr: iced_x86::Instruction::with2(code, immediate, register).map_err(to_value_error)? })
	}

	/// Creates an instruction with 2 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `immediate` (int): (``u32``) op0: Immediate value
	///     `register` (:class:`Register`): op1: Register
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, immediate, register)")]
	fn create_u32_reg(code: u32, immediate: u32, register: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register = to_register(register)?;
		Ok(Instruction { instr: iced_x86::Instruction::with2(code, immediate, register).map_err(to_value_error)? })
	}

	/// Creates an instruction with 2 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `immediate1` (int): (``i32``) op0: Immediate value
	///     `immediate2` (int): (``i32``) op1: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, immediate1, immediate2)")]
	fn create_i32_i32(code: u32, immediate1: i32, immediate2: i32) -> PyResult<Self> {
		let code = to_code(code)?;
		Ok(Instruction { instr: iced_x86::Instruction::with2(code, immediate1, immediate2).map_err(to_value_error)? })
	}

	/// Creates an instruction with 2 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `immediate1` (int): (``u32``) op0: Immediate value
	///     `immediate2` (int): (``u32``) op1: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, immediate1, immediate2)")]
	fn create_u32_u32(code: u32, immediate1: u32, immediate2: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		Ok(Instruction { instr: iced_x86::Instruction::with2(code, immediate1, immediate2).map_err(to_value_error)? })
	}

	/// Creates an instruction with 2 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `memory` (:class:`MemoryOperand`): op0: Memory operand
	///     `register` (:class:`Register`): op1: Register
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, memory, register)")]
	fn create_mem_reg(code: u32, memory: MemoryOperand, register: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register = to_register(register)?;
		Ok(Instruction { instr: iced_x86::Instruction::with2(code, memory.mem, register).map_err(to_value_error)? })
	}

	/// Creates an instruction with 2 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `memory` (:class:`MemoryOperand`): op0: Memory operand
	///     `immediate` (int): (``i32``) op1: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, memory, immediate)")]
	fn create_mem_i32(code: u32, memory: MemoryOperand, immediate: i32) -> PyResult<Self> {
		let code = to_code(code)?;
		Ok(Instruction { instr: iced_x86::Instruction::with2(code, memory.mem, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 2 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `memory` (:class:`MemoryOperand`): op0: Memory operand
	///     `immediate` (int): (``u32``) op1: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, memory, immediate)")]
	fn create_mem_u32(code: u32, memory: MemoryOperand, immediate: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		Ok(Instruction { instr: iced_x86::Instruction::with2(code, memory.mem, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 3 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register1` (:class:`Register`): op0: Register
	///     `register2` (:class:`Register`): op1: Register
	///     `register3` (:class:`Register`): op2: Register
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register1, register2, register3)")]
	fn create_reg_reg_reg(code: u32, register1: u32, register2: u32, register3: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		let register3 = to_register(register3)?;
		Ok(Instruction { instr: iced_x86::Instruction::with3(code, register1, register2, register3).map_err(to_value_error)? })
	}

	/// Creates an instruction with 3 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register1` (:class:`Register`): op0: Register
	///     `register2` (:class:`Register`): op1: Register
	///     `immediate` (int): (``i32``) op2: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register1, register2, immediate)")]
	fn create_reg_reg_i32(code: u32, register1: u32, register2: u32, immediate: i32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		Ok(Instruction { instr: iced_x86::Instruction::with3(code, register1, register2, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 3 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register1` (:class:`Register`): op0: Register
	///     `register2` (:class:`Register`): op1: Register
	///     `immediate` (int): (``u32``) op2: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register1, register2, immediate)")]
	fn create_reg_reg_u32(code: u32, register1: u32, register2: u32, immediate: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		Ok(Instruction { instr: iced_x86::Instruction::with3(code, register1, register2, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 3 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register1` (:class:`Register`): op0: Register
	///     `register2` (:class:`Register`): op1: Register
	///     `memory` (:class:`MemoryOperand`): op2: Memory operand
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register1, register2, memory)")]
	fn create_reg_reg_mem(code: u32, register1: u32, register2: u32, memory: MemoryOperand) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		Ok(Instruction { instr: iced_x86::Instruction::with3(code, register1, register2, memory.mem).map_err(to_value_error)? })
	}

	/// Creates an instruction with 3 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register` (:class:`Register`): op0: Register
	///     `immediate1` (int): (``i32``) op1: Immediate value
	///     `immediate2` (int): (``i32``) op2: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register, immediate1, immediate2)")]
	fn create_reg_i32_i32(code: u32, register: u32, immediate1: i32, immediate2: i32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register = to_register(register)?;
		Ok(Instruction { instr: iced_x86::Instruction::with3(code, register, immediate1, immediate2).map_err(to_value_error)? })
	}

	/// Creates an instruction with 3 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register` (:class:`Register`): op0: Register
	///     `immediate1` (int): (``u32``) op1: Immediate value
	///     `immediate2` (int): (``u32``) op2: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register, immediate1, immediate2)")]
	fn create_reg_u32_u32(code: u32, register: u32, immediate1: u32, immediate2: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register = to_register(register)?;
		Ok(Instruction { instr: iced_x86::Instruction::with3(code, register, immediate1, immediate2).map_err(to_value_error)? })
	}

	/// Creates an instruction with 3 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register1` (:class:`Register`): op0: Register
	///     `memory` (:class:`MemoryOperand`): op1: Memory operand
	///     `register2` (:class:`Register`): op2: Register
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register1, memory, register2)")]
	fn create_reg_mem_reg(code: u32, register1: u32, memory: MemoryOperand, register2: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		Ok(Instruction { instr: iced_x86::Instruction::with3(code, register1, memory.mem, register2).map_err(to_value_error)? })
	}

	/// Creates an instruction with 3 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register` (:class:`Register`): op0: Register
	///     `memory` (:class:`MemoryOperand`): op1: Memory operand
	///     `immediate` (int): (``i32``) op2: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register, memory, immediate)")]
	fn create_reg_mem_i32(code: u32, register: u32, memory: MemoryOperand, immediate: i32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register = to_register(register)?;
		Ok(Instruction { instr: iced_x86::Instruction::with3(code, register, memory.mem, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 3 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register` (:class:`Register`): op0: Register
	///     `memory` (:class:`MemoryOperand`): op1: Memory operand
	///     `immediate` (int): (``u32``) op2: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register, memory, immediate)")]
	fn create_reg_mem_u32(code: u32, register: u32, memory: MemoryOperand, immediate: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register = to_register(register)?;
		Ok(Instruction { instr: iced_x86::Instruction::with3(code, register, memory.mem, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 3 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `memory` (:class:`MemoryOperand`): op0: Memory operand
	///     `register1` (:class:`Register`): op1: Register
	///     `register2` (:class:`Register`): op2: Register
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, memory, register1, register2)")]
	fn create_mem_reg_reg(code: u32, memory: MemoryOperand, register1: u32, register2: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		Ok(Instruction { instr: iced_x86::Instruction::with3(code, memory.mem, register1, register2).map_err(to_value_error)? })
	}

	/// Creates an instruction with 3 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `memory` (:class:`MemoryOperand`): op0: Memory operand
	///     `register` (:class:`Register`): op1: Register
	///     `immediate` (int): (``i32``) op2: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, memory, register, immediate)")]
	fn create_mem_reg_i32(code: u32, memory: MemoryOperand, register: u32, immediate: i32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register = to_register(register)?;
		Ok(Instruction { instr: iced_x86::Instruction::with3(code, memory.mem, register, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 3 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `memory` (:class:`MemoryOperand`): op0: Memory operand
	///     `register` (:class:`Register`): op1: Register
	///     `immediate` (int): (``u32``) op2: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, memory, register, immediate)")]
	fn create_mem_reg_u32(code: u32, memory: MemoryOperand, register: u32, immediate: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register = to_register(register)?;
		Ok(Instruction { instr: iced_x86::Instruction::with3(code, memory.mem, register, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 4 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register1` (:class:`Register`): op0: Register
	///     `register2` (:class:`Register`): op1: Register
	///     `register3` (:class:`Register`): op2: Register
	///     `register4` (:class:`Register`): op3: Register
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register1, register2, register3, register4)")]
	fn create_reg_reg_reg_reg(code: u32, register1: u32, register2: u32, register3: u32, register4: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		let register3 = to_register(register3)?;
		let register4 = to_register(register4)?;
		Ok(Instruction { instr: iced_x86::Instruction::with4(code, register1, register2, register3, register4).map_err(to_value_error)? })
	}

	/// Creates an instruction with 4 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register1` (:class:`Register`): op0: Register
	///     `register2` (:class:`Register`): op1: Register
	///     `register3` (:class:`Register`): op2: Register
	///     `immediate` (int): (``i32``) op3: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register1, register2, register3, immediate)")]
	fn create_reg_reg_reg_i32(code: u32, register1: u32, register2: u32, register3: u32, immediate: i32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		let register3 = to_register(register3)?;
		Ok(Instruction { instr: iced_x86::Instruction::with4(code, register1, register2, register3, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 4 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register1` (:class:`Register`): op0: Register
	///     `register2` (:class:`Register`): op1: Register
	///     `register3` (:class:`Register`): op2: Register
	///     `immediate` (int): (``u32``) op3: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register1, register2, register3, immediate)")]
	fn create_reg_reg_reg_u32(code: u32, register1: u32, register2: u32, register3: u32, immediate: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		let register3 = to_register(register3)?;
		Ok(Instruction { instr: iced_x86::Instruction::with4(code, register1, register2, register3, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 4 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register1` (:class:`Register`): op0: Register
	///     `register2` (:class:`Register`): op1: Register
	///     `register3` (:class:`Register`): op2: Register
	///     `memory` (:class:`MemoryOperand`): op3: Memory operand
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register1, register2, register3, memory)")]
	fn create_reg_reg_reg_mem(code: u32, register1: u32, register2: u32, register3: u32, memory: MemoryOperand) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		let register3 = to_register(register3)?;
		Ok(Instruction { instr: iced_x86::Instruction::with4(code, register1, register2, register3, memory.mem).map_err(to_value_error)? })
	}

	/// Creates an instruction with 4 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register1` (:class:`Register`): op0: Register
	///     `register2` (:class:`Register`): op1: Register
	///     `immediate1` (int): (``i32``) op2: Immediate value
	///     `immediate2` (int): (``i32``) op3: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register1, register2, immediate1, immediate2)")]
	fn create_reg_reg_i32_i32(code: u32, register1: u32, register2: u32, immediate1: i32, immediate2: i32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		Ok(Instruction { instr: iced_x86::Instruction::with4(code, register1, register2, immediate1, immediate2).map_err(to_value_error)? })
	}

	/// Creates an instruction with 4 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register1` (:class:`Register`): op0: Register
	///     `register2` (:class:`Register`): op1: Register
	///     `immediate1` (int): (``u32``) op2: Immediate value
	///     `immediate2` (int): (``u32``) op3: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register1, register2, immediate1, immediate2)")]
	fn create_reg_reg_u32_u32(code: u32, register1: u32, register2: u32, immediate1: u32, immediate2: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		Ok(Instruction { instr: iced_x86::Instruction::with4(code, register1, register2, immediate1, immediate2).map_err(to_value_error)? })
	}

	/// Creates an instruction with 4 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register1` (:class:`Register`): op0: Register
	///     `register2` (:class:`Register`): op1: Register
	///     `memory` (:class:`MemoryOperand`): op2: Memory operand
	///     `register3` (:class:`Register`): op3: Register
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register1, register2, memory, register3)")]
	fn create_reg_reg_mem_reg(code: u32, register1: u32, register2: u32, memory: MemoryOperand, register3: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		let register3 = to_register(register3)?;
		Ok(Instruction { instr: iced_x86::Instruction::with4(code, register1, register2, memory.mem, register3).map_err(to_value_error)? })
	}

	/// Creates an instruction with 4 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register1` (:class:`Register`): op0: Register
	///     `register2` (:class:`Register`): op1: Register
	///     `memory` (:class:`MemoryOperand`): op2: Memory operand
	///     `immediate` (int): (``i32``) op3: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register1, register2, memory, immediate)")]
	fn create_reg_reg_mem_i32(code: u32, register1: u32, register2: u32, memory: MemoryOperand, immediate: i32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		Ok(Instruction { instr: iced_x86::Instruction::with4(code, register1, register2, memory.mem, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 4 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register1` (:class:`Register`): op0: Register
	///     `register2` (:class:`Register`): op1: Register
	///     `memory` (:class:`MemoryOperand`): op2: Memory operand
	///     `immediate` (int): (``u32``) op3: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register1, register2, memory, immediate)")]
	fn create_reg_reg_mem_u32(code: u32, register1: u32, register2: u32, memory: MemoryOperand, immediate: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		Ok(Instruction { instr: iced_x86::Instruction::with4(code, register1, register2, memory.mem, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 5 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register1` (:class:`Register`): op0: Register
	///     `register2` (:class:`Register`): op1: Register
	///     `register3` (:class:`Register`): op2: Register
	///     `register4` (:class:`Register`): op3: Register
	///     `immediate` (int): (``i32``) op4: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register1, register2, register3, register4, immediate)")]
	fn create_reg_reg_reg_reg_i32(code: u32, register1: u32, register2: u32, register3: u32, register4: u32, immediate: i32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		let register3 = to_register(register3)?;
		let register4 = to_register(register4)?;
		Ok(Instruction { instr: iced_x86::Instruction::with5(code, register1, register2, register3, register4, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 5 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register1` (:class:`Register`): op0: Register
	///     `register2` (:class:`Register`): op1: Register
	///     `register3` (:class:`Register`): op2: Register
	///     `register4` (:class:`Register`): op3: Register
	///     `immediate` (int): (``u32``) op4: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register1, register2, register3, register4, immediate)")]
	fn create_reg_reg_reg_reg_u32(code: u32, register1: u32, register2: u32, register3: u32, register4: u32, immediate: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		let register3 = to_register(register3)?;
		let register4 = to_register(register4)?;
		Ok(Instruction { instr: iced_x86::Instruction::with5(code, register1, register2, register3, register4, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 5 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register1` (:class:`Register`): op0: Register
	///     `register2` (:class:`Register`): op1: Register
	///     `register3` (:class:`Register`): op2: Register
	///     `memory` (:class:`MemoryOperand`): op3: Memory operand
	///     `immediate` (int): (``i32``) op4: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register1, register2, register3, memory, immediate)")]
	fn create_reg_reg_reg_mem_i32(code: u32, register1: u32, register2: u32, register3: u32, memory: MemoryOperand, immediate: i32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		let register3 = to_register(register3)?;
		Ok(Instruction { instr: iced_x86::Instruction::with5(code, register1, register2, register3, memory.mem, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 5 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register1` (:class:`Register`): op0: Register
	///     `register2` (:class:`Register`): op1: Register
	///     `register3` (:class:`Register`): op2: Register
	///     `memory` (:class:`MemoryOperand`): op3: Memory operand
	///     `immediate` (int): (``u32``) op4: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register1, register2, register3, memory, immediate)")]
	fn create_reg_reg_reg_mem_u32(code: u32, register1: u32, register2: u32, register3: u32, memory: MemoryOperand, immediate: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		let register3 = to_register(register3)?;
		Ok(Instruction { instr: iced_x86::Instruction::with5(code, register1, register2, register3, memory.mem, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 5 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register1` (:class:`Register`): op0: Register
	///     `register2` (:class:`Register`): op1: Register
	///     `memory` (:class:`MemoryOperand`): op2: Memory operand
	///     `register3` (:class:`Register`): op3: Register
	///     `immediate` (int): (``i32``) op4: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register1, register2, memory, register3, immediate)")]
	fn create_reg_reg_mem_reg_i32(code: u32, register1: u32, register2: u32, memory: MemoryOperand, register3: u32, immediate: i32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		let register3 = to_register(register3)?;
		Ok(Instruction { instr: iced_x86::Instruction::with5(code, register1, register2, memory.mem, register3, immediate).map_err(to_value_error)? })
	}

	/// Creates an instruction with 5 operands
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `register1` (:class:`Register`): op0: Register
	///     `register2` (:class:`Register`): op1: Register
	///     `memory` (:class:`MemoryOperand`): op2: Memory operand
	///     `register3` (:class:`Register`): op3: Register
	///     `immediate` (int): (``u32``) op4: Immediate value
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If one of the operands is invalid (basic checks)
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, register1, register2, memory, register3, immediate)")]
	fn create_reg_reg_mem_reg_u32(code: u32, register1: u32, register2: u32, memory: MemoryOperand, register3: u32, immediate: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		let register3 = to_register(register3)?;
		Ok(Instruction { instr: iced_x86::Instruction::with5(code, register1, register2, memory.mem, register3, immediate).map_err(to_value_error)? })
	}

	/// Creates a new near/short branch instruction
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `target` (int): (``u64``) Target address
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If the created instruction doesn't have a near branch operand
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, target)")]
	fn create_branch(code: u32, target: u64) -> PyResult<Self> {
		let code = to_code(code)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_branch(code, target).map_err(to_value_error)? })
	}

	/// Creates a new far branch instruction
	///
	/// Args:
	///     `code` (:class:`Code`): Code value
	///     `selector` (int): (``u16``) Selector/segment value
	///     `offset` (int): (``u32``) Offset
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If the created instruction doesn't have a far branch operand
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(code, selector, offset)")]
	fn create_far_branch(code: u32, selector: u16, offset: u32) -> PyResult<Self> {
		let code = to_code(code)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_far_branch(code, selector, offset).map_err(to_value_error)? })
	}

	/// Creates a new ``XBEGIN`` instruction
	///
	/// Args:
	///     `bitness` (int): (``u32``) 16, 32, or 64
	///     `target` (int): (``u64``) Target address
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `bitness` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(bitness, target)")]
	fn create_xbegin(bitness: u32, target: u64) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_xbegin(bitness, target).map_err(to_value_error)? })
	}

	/// Creates a ``OUTSB`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `segment_prefix` (:class:`Register`): Segment override or :class:`Register.NONE`
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, segment_prefix = 0, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, segment_prefix = 0, rep_prefix = 0))]
	fn create_outsb(address_size: u32, segment_prefix: u32, rep_prefix: u32) -> PyResult<Self> {
		let segment_prefix = to_register(segment_prefix)?;
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_outsb(address_size, segment_prefix, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REP OUTSB`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_rep_outsb(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_rep_outsb(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``OUTSW`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `segment_prefix` (:class:`Register`): Segment override or :class:`Register.NONE`
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, segment_prefix = 0, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, segment_prefix = 0, rep_prefix = 0))]
	fn create_outsw(address_size: u32, segment_prefix: u32, rep_prefix: u32) -> PyResult<Self> {
		let segment_prefix = to_register(segment_prefix)?;
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_outsw(address_size, segment_prefix, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REP OUTSW`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_rep_outsw(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_rep_outsw(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``OUTSD`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `segment_prefix` (:class:`Register`): Segment override or :class:`Register.NONE`
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, segment_prefix = 0, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, segment_prefix = 0, rep_prefix = 0))]
	fn create_outsd(address_size: u32, segment_prefix: u32, rep_prefix: u32) -> PyResult<Self> {
		let segment_prefix = to_register(segment_prefix)?;
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_outsd(address_size, segment_prefix, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REP OUTSD`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_rep_outsd(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_rep_outsd(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``LODSB`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `segment_prefix` (:class:`Register`): Segment override or :class:`Register.NONE`
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, segment_prefix = 0, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, segment_prefix = 0, rep_prefix = 0))]
	fn create_lodsb(address_size: u32, segment_prefix: u32, rep_prefix: u32) -> PyResult<Self> {
		let segment_prefix = to_register(segment_prefix)?;
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_lodsb(address_size, segment_prefix, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REP LODSB`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_rep_lodsb(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_rep_lodsb(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``LODSW`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `segment_prefix` (:class:`Register`): Segment override or :class:`Register.NONE`
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, segment_prefix = 0, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, segment_prefix = 0, rep_prefix = 0))]
	fn create_lodsw(address_size: u32, segment_prefix: u32, rep_prefix: u32) -> PyResult<Self> {
		let segment_prefix = to_register(segment_prefix)?;
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_lodsw(address_size, segment_prefix, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REP LODSW`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_rep_lodsw(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_rep_lodsw(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``LODSD`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `segment_prefix` (:class:`Register`): Segment override or :class:`Register.NONE`
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, segment_prefix = 0, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, segment_prefix = 0, rep_prefix = 0))]
	fn create_lodsd(address_size: u32, segment_prefix: u32, rep_prefix: u32) -> PyResult<Self> {
		let segment_prefix = to_register(segment_prefix)?;
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_lodsd(address_size, segment_prefix, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REP LODSD`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_rep_lodsd(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_rep_lodsd(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``LODSQ`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `segment_prefix` (:class:`Register`): Segment override or :class:`Register.NONE`
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, segment_prefix = 0, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, segment_prefix = 0, rep_prefix = 0))]
	fn create_lodsq(address_size: u32, segment_prefix: u32, rep_prefix: u32) -> PyResult<Self> {
		let segment_prefix = to_register(segment_prefix)?;
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_lodsq(address_size, segment_prefix, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REP LODSQ`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_rep_lodsq(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_rep_lodsq(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``SCASB`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, rep_prefix = 0))]
	fn create_scasb(address_size: u32, rep_prefix: u32) -> PyResult<Self> {
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_scasb(address_size, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REPE SCASB`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_repe_scasb(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_repe_scasb(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``REPNE SCASB`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_repne_scasb(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_repne_scasb(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``SCASW`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, rep_prefix = 0))]
	fn create_scasw(address_size: u32, rep_prefix: u32) -> PyResult<Self> {
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_scasw(address_size, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REPE SCASW`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_repe_scasw(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_repe_scasw(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``REPNE SCASW`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_repne_scasw(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_repne_scasw(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``SCASD`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, rep_prefix = 0))]
	fn create_scasd(address_size: u32, rep_prefix: u32) -> PyResult<Self> {
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_scasd(address_size, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REPE SCASD`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_repe_scasd(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_repe_scasd(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``REPNE SCASD`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_repne_scasd(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_repne_scasd(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``SCASQ`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, rep_prefix = 0))]
	fn create_scasq(address_size: u32, rep_prefix: u32) -> PyResult<Self> {
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_scasq(address_size, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REPE SCASQ`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_repe_scasq(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_repe_scasq(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``REPNE SCASQ`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_repne_scasq(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_repne_scasq(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``INSB`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, rep_prefix = 0))]
	fn create_insb(address_size: u32, rep_prefix: u32) -> PyResult<Self> {
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_insb(address_size, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REP INSB`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_rep_insb(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_rep_insb(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``INSW`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, rep_prefix = 0))]
	fn create_insw(address_size: u32, rep_prefix: u32) -> PyResult<Self> {
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_insw(address_size, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REP INSW`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_rep_insw(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_rep_insw(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``INSD`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, rep_prefix = 0))]
	fn create_insd(address_size: u32, rep_prefix: u32) -> PyResult<Self> {
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_insd(address_size, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REP INSD`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_rep_insd(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_rep_insd(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``STOSB`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, rep_prefix = 0))]
	fn create_stosb(address_size: u32, rep_prefix: u32) -> PyResult<Self> {
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_stosb(address_size, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REP STOSB`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_rep_stosb(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_rep_stosb(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``STOSW`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, rep_prefix = 0))]
	fn create_stosw(address_size: u32, rep_prefix: u32) -> PyResult<Self> {
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_stosw(address_size, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REP STOSW`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_rep_stosw(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_rep_stosw(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``STOSD`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, rep_prefix = 0))]
	fn create_stosd(address_size: u32, rep_prefix: u32) -> PyResult<Self> {
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_stosd(address_size, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REP STOSD`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_rep_stosd(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_rep_stosd(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``STOSQ`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, rep_prefix = 0))]
	fn create_stosq(address_size: u32, rep_prefix: u32) -> PyResult<Self> {
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_stosq(address_size, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REP STOSQ`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_rep_stosq(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_rep_stosq(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``CMPSB`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `segment_prefix` (:class:`Register`): Segment override or :class:`Register.NONE`
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, segment_prefix = 0, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, segment_prefix = 0, rep_prefix = 0))]
	fn create_cmpsb(address_size: u32, segment_prefix: u32, rep_prefix: u32) -> PyResult<Self> {
		let segment_prefix = to_register(segment_prefix)?;
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_cmpsb(address_size, segment_prefix, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REPE CMPSB`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_repe_cmpsb(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_repe_cmpsb(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``REPNE CMPSB`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_repne_cmpsb(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_repne_cmpsb(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``CMPSW`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `segment_prefix` (:class:`Register`): Segment override or :class:`Register.NONE`
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, segment_prefix = 0, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, segment_prefix = 0, rep_prefix = 0))]
	fn create_cmpsw(address_size: u32, segment_prefix: u32, rep_prefix: u32) -> PyResult<Self> {
		let segment_prefix = to_register(segment_prefix)?;
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_cmpsw(address_size, segment_prefix, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REPE CMPSW`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_repe_cmpsw(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_repe_cmpsw(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``REPNE CMPSW`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_repne_cmpsw(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_repne_cmpsw(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``CMPSD`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `segment_prefix` (:class:`Register`): Segment override or :class:`Register.NONE`
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, segment_prefix = 0, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, segment_prefix = 0, rep_prefix = 0))]
	fn create_cmpsd(address_size: u32, segment_prefix: u32, rep_prefix: u32) -> PyResult<Self> {
		let segment_prefix = to_register(segment_prefix)?;
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_cmpsd(address_size, segment_prefix, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REPE CMPSD`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_repe_cmpsd(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_repe_cmpsd(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``REPNE CMPSD`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_repne_cmpsd(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_repne_cmpsd(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``CMPSQ`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `segment_prefix` (:class:`Register`): Segment override or :class:`Register.NONE`
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, segment_prefix = 0, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, segment_prefix = 0, rep_prefix = 0))]
	fn create_cmpsq(address_size: u32, segment_prefix: u32, rep_prefix: u32) -> PyResult<Self> {
		let segment_prefix = to_register(segment_prefix)?;
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_cmpsq(address_size, segment_prefix, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REPE CMPSQ`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_repe_cmpsq(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_repe_cmpsq(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``REPNE CMPSQ`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_repne_cmpsq(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_repne_cmpsq(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``MOVSB`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `segment_prefix` (:class:`Register`): Segment override or :class:`Register.NONE`
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, segment_prefix = 0, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, segment_prefix = 0, rep_prefix = 0))]
	fn create_movsb(address_size: u32, segment_prefix: u32, rep_prefix: u32) -> PyResult<Self> {
		let segment_prefix = to_register(segment_prefix)?;
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_movsb(address_size, segment_prefix, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REP MOVSB`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_rep_movsb(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_rep_movsb(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``MOVSW`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `segment_prefix` (:class:`Register`): Segment override or :class:`Register.NONE`
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, segment_prefix = 0, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, segment_prefix = 0, rep_prefix = 0))]
	fn create_movsw(address_size: u32, segment_prefix: u32, rep_prefix: u32) -> PyResult<Self> {
		let segment_prefix = to_register(segment_prefix)?;
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_movsw(address_size, segment_prefix, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REP MOVSW`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_rep_movsw(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_rep_movsw(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``MOVSD`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `segment_prefix` (:class:`Register`): Segment override or :class:`Register.NONE`
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, segment_prefix = 0, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, segment_prefix = 0, rep_prefix = 0))]
	fn create_movsd(address_size: u32, segment_prefix: u32, rep_prefix: u32) -> PyResult<Self> {
		let segment_prefix = to_register(segment_prefix)?;
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_movsd(address_size, segment_prefix, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REP MOVSD`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_rep_movsd(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_rep_movsd(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``MOVSQ`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `segment_prefix` (:class:`Register`): Segment override or :class:`Register.NONE`
	///     `rep_prefix` (:class:`RepPrefixKind`): Rep prefix or :class:`RepPrefixKind.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, segment_prefix = 0, rep_prefix = 0)")]
	#[pyo3(signature = (address_size, segment_prefix = 0, rep_prefix = 0))]
	fn create_movsq(address_size: u32, segment_prefix: u32, rep_prefix: u32) -> PyResult<Self> {
		let segment_prefix = to_register(segment_prefix)?;
		let rep_prefix = to_rep_prefix_kind(rep_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_movsq(address_size, segment_prefix, rep_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``REP MOVSQ`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size)")]
	fn create_rep_movsq(address_size: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::with_rep_movsq(address_size).map_err(to_value_error)? })
	}

	/// Creates a ``MASKMOVQ`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `register1` (:class:`Register`): Register
	///     `register2` (:class:`Register`): Register
	///     `segment_prefix` (:class:`Register`): Segment override or :class:`Register.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, register1, register2, segment_prefix = 0)")]
	#[pyo3(signature = (address_size, register1, register2, segment_prefix = 0))]
	fn create_maskmovq(address_size: u32, register1: u32, register2: u32, segment_prefix: u32) -> PyResult<Self> {
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		let segment_prefix = to_register(segment_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_maskmovq(address_size, register1, register2, segment_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``MASKMOVDQU`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `register1` (:class:`Register`): Register
	///     `register2` (:class:`Register`): Register
	///     `segment_prefix` (:class:`Register`): Segment override or :class:`Register.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, register1, register2, segment_prefix = 0)")]
	#[pyo3(signature = (address_size, register1, register2, segment_prefix = 0))]
	fn create_maskmovdqu(address_size: u32, register1: u32, register2: u32, segment_prefix: u32) -> PyResult<Self> {
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		let segment_prefix = to_register(segment_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_maskmovdqu(address_size, register1, register2, segment_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``VMASKMOVDQU`` instruction
	///
	/// Args:
	///     `address_size` (int): (``u32``) 16, 32, or 64
	///     `register1` (:class:`Register`): Register
	///     `register2` (:class:`Register`): Register
	///     `segment_prefix` (:class:`Register`): Segment override or :class:`Register.NONE`
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `address_size` is not one of 16, 32, 64.
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(address_size, register1, register2, segment_prefix = 0)")]
	#[pyo3(signature = (address_size, register1, register2, segment_prefix = 0))]
	fn create_vmaskmovdqu(address_size: u32, register1: u32, register2: u32, segment_prefix: u32) -> PyResult<Self> {
		let register1 = to_register(register1)?;
		let register2 = to_register(register2)?;
		let segment_prefix = to_register(segment_prefix)?;
		Ok(Instruction { instr: iced_x86::Instruction::with_vmaskmovdqu(address_size, register1, register2, segment_prefix).map_err(to_value_error)? })
	}

	/// Creates a ``db``/``.byte`` asm directive
	///
	/// Args:
	///     `b0` (int): (``u8``) Byte 0
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(b0)")]
	fn create_declare_byte_1(b0: u8) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_byte_1(b0).map_err(to_value_error)? })
	}

	/// Creates a ``db``/``.byte`` asm directive
	///
	/// Args:
	///     `b0` (int): (``u8``) Byte 0
	///     `b1` (int): (``u8``) Byte 1
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(b0, b1)")]
	fn create_declare_byte_2(b0: u8, b1: u8) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_byte_2(b0, b1).map_err(to_value_error)? })
	}

	/// Creates a ``db``/``.byte`` asm directive
	///
	/// Args:
	///     `b0` (int): (``u8``) Byte 0
	///     `b1` (int): (``u8``) Byte 1
	///     `b2` (int): (``u8``) Byte 2
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(b0, b1, b2)")]
	fn create_declare_byte_3(b0: u8, b1: u8, b2: u8) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_byte_3(b0, b1, b2).map_err(to_value_error)? })
	}

	/// Creates a ``db``/``.byte`` asm directive
	///
	/// Args:
	///     `b0` (int): (``u8``) Byte 0
	///     `b1` (int): (``u8``) Byte 1
	///     `b2` (int): (``u8``) Byte 2
	///     `b3` (int): (``u8``) Byte 3
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(b0, b1, b2, b3)")]
	fn create_declare_byte_4(b0: u8, b1: u8, b2: u8, b3: u8) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_byte_4(b0, b1, b2, b3).map_err(to_value_error)? })
	}

	/// Creates a ``db``/``.byte`` asm directive
	///
	/// Args:
	///     `b0` (int): (``u8``) Byte 0
	///     `b1` (int): (``u8``) Byte 1
	///     `b2` (int): (``u8``) Byte 2
	///     `b3` (int): (``u8``) Byte 3
	///     `b4` (int): (``u8``) Byte 4
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(b0, b1, b2, b3, b4)")]
	fn create_declare_byte_5(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_byte_5(b0, b1, b2, b3, b4).map_err(to_value_error)? })
	}

	/// Creates a ``db``/``.byte`` asm directive
	///
	/// Args:
	///     `b0` (int): (``u8``) Byte 0
	///     `b1` (int): (``u8``) Byte 1
	///     `b2` (int): (``u8``) Byte 2
	///     `b3` (int): (``u8``) Byte 3
	///     `b4` (int): (``u8``) Byte 4
	///     `b5` (int): (``u8``) Byte 5
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(b0, b1, b2, b3, b4, b5)")]
	fn create_declare_byte_6(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_byte_6(b0, b1, b2, b3, b4, b5).map_err(to_value_error)? })
	}

	/// Creates a ``db``/``.byte`` asm directive
	///
	/// Args:
	///     `b0` (int): (``u8``) Byte 0
	///     `b1` (int): (``u8``) Byte 1
	///     `b2` (int): (``u8``) Byte 2
	///     `b3` (int): (``u8``) Byte 3
	///     `b4` (int): (``u8``) Byte 4
	///     `b5` (int): (``u8``) Byte 5
	///     `b6` (int): (``u8``) Byte 6
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(b0, b1, b2, b3, b4, b5, b6)")]
	fn create_declare_byte_7(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_byte_7(b0, b1, b2, b3, b4, b5, b6).map_err(to_value_error)? })
	}

	/// Creates a ``db``/``.byte`` asm directive
	///
	/// Args:
	///     `b0` (int): (``u8``) Byte 0
	///     `b1` (int): (``u8``) Byte 1
	///     `b2` (int): (``u8``) Byte 2
	///     `b3` (int): (``u8``) Byte 3
	///     `b4` (int): (``u8``) Byte 4
	///     `b5` (int): (``u8``) Byte 5
	///     `b6` (int): (``u8``) Byte 6
	///     `b7` (int): (``u8``) Byte 7
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(b0, b1, b2, b3, b4, b5, b6, b7)")]
	fn create_declare_byte_8(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_byte_8(b0, b1, b2, b3, b4, b5, b6, b7).map_err(to_value_error)? })
	}

	/// Creates a ``db``/``.byte`` asm directive
	///
	/// Args:
	///     `b0` (int): (``u8``) Byte 0
	///     `b1` (int): (``u8``) Byte 1
	///     `b2` (int): (``u8``) Byte 2
	///     `b3` (int): (``u8``) Byte 3
	///     `b4` (int): (``u8``) Byte 4
	///     `b5` (int): (``u8``) Byte 5
	///     `b6` (int): (``u8``) Byte 6
	///     `b7` (int): (``u8``) Byte 7
	///     `b8` (int): (``u8``) Byte 8
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(b0, b1, b2, b3, b4, b5, b6, b7, b8)")]
	fn create_declare_byte_9(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_byte_9(b0, b1, b2, b3, b4, b5, b6, b7, b8).map_err(to_value_error)? })
	}

	/// Creates a ``db``/``.byte`` asm directive
	///
	/// Args:
	///     `b0` (int): (``u8``) Byte 0
	///     `b1` (int): (``u8``) Byte 1
	///     `b2` (int): (``u8``) Byte 2
	///     `b3` (int): (``u8``) Byte 3
	///     `b4` (int): (``u8``) Byte 4
	///     `b5` (int): (``u8``) Byte 5
	///     `b6` (int): (``u8``) Byte 6
	///     `b7` (int): (``u8``) Byte 7
	///     `b8` (int): (``u8``) Byte 8
	///     `b9` (int): (``u8``) Byte 9
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9)")]
	fn create_declare_byte_10(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_byte_10(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9).map_err(to_value_error)? })
	}

	/// Creates a ``db``/``.byte`` asm directive
	///
	/// Args:
	///     `b0` (int): (``u8``) Byte 0
	///     `b1` (int): (``u8``) Byte 1
	///     `b2` (int): (``u8``) Byte 2
	///     `b3` (int): (``u8``) Byte 3
	///     `b4` (int): (``u8``) Byte 4
	///     `b5` (int): (``u8``) Byte 5
	///     `b6` (int): (``u8``) Byte 6
	///     `b7` (int): (``u8``) Byte 7
	///     `b8` (int): (``u8``) Byte 8
	///     `b9` (int): (``u8``) Byte 9
	///     `b10` (int): (``u8``) Byte 10
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10)")]
	fn create_declare_byte_11(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_byte_11(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10).map_err(to_value_error)? })
	}

	/// Creates a ``db``/``.byte`` asm directive
	///
	/// Args:
	///     `b0` (int): (``u8``) Byte 0
	///     `b1` (int): (``u8``) Byte 1
	///     `b2` (int): (``u8``) Byte 2
	///     `b3` (int): (``u8``) Byte 3
	///     `b4` (int): (``u8``) Byte 4
	///     `b5` (int): (``u8``) Byte 5
	///     `b6` (int): (``u8``) Byte 6
	///     `b7` (int): (``u8``) Byte 7
	///     `b8` (int): (``u8``) Byte 8
	///     `b9` (int): (``u8``) Byte 9
	///     `b10` (int): (``u8``) Byte 10
	///     `b11` (int): (``u8``) Byte 11
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11)")]
	fn create_declare_byte_12(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_byte_12(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11).map_err(to_value_error)? })
	}

	/// Creates a ``db``/``.byte`` asm directive
	///
	/// Args:
	///     `b0` (int): (``u8``) Byte 0
	///     `b1` (int): (``u8``) Byte 1
	///     `b2` (int): (``u8``) Byte 2
	///     `b3` (int): (``u8``) Byte 3
	///     `b4` (int): (``u8``) Byte 4
	///     `b5` (int): (``u8``) Byte 5
	///     `b6` (int): (``u8``) Byte 6
	///     `b7` (int): (``u8``) Byte 7
	///     `b8` (int): (``u8``) Byte 8
	///     `b9` (int): (``u8``) Byte 9
	///     `b10` (int): (``u8``) Byte 10
	///     `b11` (int): (``u8``) Byte 11
	///     `b12` (int): (``u8``) Byte 12
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12)")]
	fn create_declare_byte_13(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8, b12: u8) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_byte_13(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12).map_err(to_value_error)? })
	}

	/// Creates a ``db``/``.byte`` asm directive
	///
	/// Args:
	///     `b0` (int): (``u8``) Byte 0
	///     `b1` (int): (``u8``) Byte 1
	///     `b2` (int): (``u8``) Byte 2
	///     `b3` (int): (``u8``) Byte 3
	///     `b4` (int): (``u8``) Byte 4
	///     `b5` (int): (``u8``) Byte 5
	///     `b6` (int): (``u8``) Byte 6
	///     `b7` (int): (``u8``) Byte 7
	///     `b8` (int): (``u8``) Byte 8
	///     `b9` (int): (``u8``) Byte 9
	///     `b10` (int): (``u8``) Byte 10
	///     `b11` (int): (``u8``) Byte 11
	///     `b12` (int): (``u8``) Byte 12
	///     `b13` (int): (``u8``) Byte 13
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13)")]
	fn create_declare_byte_14(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8, b12: u8, b13: u8) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_byte_14(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13).map_err(to_value_error)? })
	}

	/// Creates a ``db``/``.byte`` asm directive
	///
	/// Args:
	///     `b0` (int): (``u8``) Byte 0
	///     `b1` (int): (``u8``) Byte 1
	///     `b2` (int): (``u8``) Byte 2
	///     `b3` (int): (``u8``) Byte 3
	///     `b4` (int): (``u8``) Byte 4
	///     `b5` (int): (``u8``) Byte 5
	///     `b6` (int): (``u8``) Byte 6
	///     `b7` (int): (``u8``) Byte 7
	///     `b8` (int): (``u8``) Byte 8
	///     `b9` (int): (``u8``) Byte 9
	///     `b10` (int): (``u8``) Byte 10
	///     `b11` (int): (``u8``) Byte 11
	///     `b12` (int): (``u8``) Byte 12
	///     `b13` (int): (``u8``) Byte 13
	///     `b14` (int): (``u8``) Byte 14
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14)")]
	fn create_declare_byte_15(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8, b12: u8, b13: u8, b14: u8) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_byte_15(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14).map_err(to_value_error)? })
	}

	/// Creates a ``db``/``.byte`` asm directive
	///
	/// Args:
	///     `b0` (int): (``u8``) Byte 0
	///     `b1` (int): (``u8``) Byte 1
	///     `b2` (int): (``u8``) Byte 2
	///     `b3` (int): (``u8``) Byte 3
	///     `b4` (int): (``u8``) Byte 4
	///     `b5` (int): (``u8``) Byte 5
	///     `b6` (int): (``u8``) Byte 6
	///     `b7` (int): (``u8``) Byte 7
	///     `b8` (int): (``u8``) Byte 8
	///     `b9` (int): (``u8``) Byte 9
	///     `b10` (int): (``u8``) Byte 10
	///     `b11` (int): (``u8``) Byte 11
	///     `b12` (int): (``u8``) Byte 12
	///     `b13` (int): (``u8``) Byte 13
	///     `b14` (int): (``u8``) Byte 14
	///     `b15` (int): (``u8``) Byte 15
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14, b15)")]
	fn create_declare_byte_16(b0: u8, b1: u8, b2: u8, b3: u8, b4: u8, b5: u8, b6: u8, b7: u8, b8: u8, b9: u8, b10: u8, b11: u8, b12: u8, b13: u8, b14: u8, b15: u8) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_byte_16(b0, b1, b2, b3, b4, b5, b6, b7, b8, b9, b10, b11, b12, b13, b14, b15).map_err(to_value_error)? })
	}

	/// Creates a ``db``/``.byte`` asm directive
	///
	/// Args:
	///     `data` (bytes, bytearray): Data
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	///
	/// Raises:
	///     ValueError: If `len(data)` is not 1-16
	///     TypeError: If `data` is not a supported type
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(data)")]
	fn create_declare_byte(data: &Bound<'_, PyAny>) -> PyResult<Self> {
		let data = unsafe { get_temporary_byte_array_ref(data)? };
		Ok(Instruction { instr: iced_x86::Instruction::with_declare_byte(data).map_err(to_value_error)? })
	}

	/// Creates a ``dw``/``.word`` asm directive
	///
	/// Args:
	///     `w0` (int): (``u16``) Word 0
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(w0)")]
	fn create_declare_word_1(w0: u16) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_word_1(w0).map_err(to_value_error)? })
	}

	/// Creates a ``dw``/``.word`` asm directive
	///
	/// Args:
	///     `w0` (int): (``u16``) Word 0
	///     `w1` (int): (``u16``) Word 1
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(w0, w1)")]
	fn create_declare_word_2(w0: u16, w1: u16) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_word_2(w0, w1).map_err(to_value_error)? })
	}

	/// Creates a ``dw``/``.word`` asm directive
	///
	/// Args:
	///     `w0` (int): (``u16``) Word 0
	///     `w1` (int): (``u16``) Word 1
	///     `w2` (int): (``u16``) Word 2
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(w0, w1, w2)")]
	fn create_declare_word_3(w0: u16, w1: u16, w2: u16) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_word_3(w0, w1, w2).map_err(to_value_error)? })
	}

	/// Creates a ``dw``/``.word`` asm directive
	///
	/// Args:
	///     `w0` (int): (``u16``) Word 0
	///     `w1` (int): (``u16``) Word 1
	///     `w2` (int): (``u16``) Word 2
	///     `w3` (int): (``u16``) Word 3
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(w0, w1, w2, w3)")]
	fn create_declare_word_4(w0: u16, w1: u16, w2: u16, w3: u16) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_word_4(w0, w1, w2, w3).map_err(to_value_error)? })
	}

	/// Creates a ``dw``/``.word`` asm directive
	///
	/// Args:
	///     `w0` (int): (``u16``) Word 0
	///     `w1` (int): (``u16``) Word 1
	///     `w2` (int): (``u16``) Word 2
	///     `w3` (int): (``u16``) Word 3
	///     `w4` (int): (``u16``) Word 4
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(w0, w1, w2, w3, w4)")]
	fn create_declare_word_5(w0: u16, w1: u16, w2: u16, w3: u16, w4: u16) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_word_5(w0, w1, w2, w3, w4).map_err(to_value_error)? })
	}

	/// Creates a ``dw``/``.word`` asm directive
	///
	/// Args:
	///     `w0` (int): (``u16``) Word 0
	///     `w1` (int): (``u16``) Word 1
	///     `w2` (int): (``u16``) Word 2
	///     `w3` (int): (``u16``) Word 3
	///     `w4` (int): (``u16``) Word 4
	///     `w5` (int): (``u16``) Word 5
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(w0, w1, w2, w3, w4, w5)")]
	fn create_declare_word_6(w0: u16, w1: u16, w2: u16, w3: u16, w4: u16, w5: u16) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_word_6(w0, w1, w2, w3, w4, w5).map_err(to_value_error)? })
	}

	/// Creates a ``dw``/``.word`` asm directive
	///
	/// Args:
	///     `w0` (int): (``u16``) Word 0
	///     `w1` (int): (``u16``) Word 1
	///     `w2` (int): (``u16``) Word 2
	///     `w3` (int): (``u16``) Word 3
	///     `w4` (int): (``u16``) Word 4
	///     `w5` (int): (``u16``) Word 5
	///     `w6` (int): (``u16``) Word 6
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(w0, w1, w2, w3, w4, w5, w6)")]
	fn create_declare_word_7(w0: u16, w1: u16, w2: u16, w3: u16, w4: u16, w5: u16, w6: u16) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_word_7(w0, w1, w2, w3, w4, w5, w6).map_err(to_value_error)? })
	}

	/// Creates a ``dw``/``.word`` asm directive
	///
	/// Args:
	///     `w0` (int): (``u16``) Word 0
	///     `w1` (int): (``u16``) Word 1
	///     `w2` (int): (``u16``) Word 2
	///     `w3` (int): (``u16``) Word 3
	///     `w4` (int): (``u16``) Word 4
	///     `w5` (int): (``u16``) Word 5
	///     `w6` (int): (``u16``) Word 6
	///     `w7` (int): (``u16``) Word 7
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(w0, w1, w2, w3, w4, w5, w6, w7)")]
	fn create_declare_word_8(w0: u16, w1: u16, w2: u16, w3: u16, w4: u16, w5: u16, w6: u16, w7: u16) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_word_8(w0, w1, w2, w3, w4, w5, w6, w7).map_err(to_value_error)? })
	}

	/// Creates a ``dd``/``.int`` asm directive
	///
	/// Args:
	///     `d0` (int): (``u32``) Dword 0
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(d0)")]
	fn create_declare_dword_1(d0: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_dword_1(d0).map_err(to_value_error)? })
	}

	/// Creates a ``dd``/``.int`` asm directive
	///
	/// Args:
	///     `d0` (int): (``u32``) Dword 0
	///     `d1` (int): (``u32``) Dword 1
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(d0, d1)")]
	fn create_declare_dword_2(d0: u32, d1: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_dword_2(d0, d1).map_err(to_value_error)? })
	}

	/// Creates a ``dd``/``.int`` asm directive
	///
	/// Args:
	///     `d0` (int): (``u32``) Dword 0
	///     `d1` (int): (``u32``) Dword 1
	///     `d2` (int): (``u32``) Dword 2
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(d0, d1, d2)")]
	fn create_declare_dword_3(d0: u32, d1: u32, d2: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_dword_3(d0, d1, d2).map_err(to_value_error)? })
	}

	/// Creates a ``dd``/``.int`` asm directive
	///
	/// Args:
	///     `d0` (int): (``u32``) Dword 0
	///     `d1` (int): (``u32``) Dword 1
	///     `d2` (int): (``u32``) Dword 2
	///     `d3` (int): (``u32``) Dword 3
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(d0, d1, d2, d3)")]
	fn create_declare_dword_4(d0: u32, d1: u32, d2: u32, d3: u32) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_dword_4(d0, d1, d2, d3).map_err(to_value_error)? })
	}

	/// Creates a ``dq``/``.quad`` asm directive
	///
	/// Args:
	///     `q0` (int): (``u64``) Qword 0
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(q0)")]
	fn create_declare_qword_1(q0: u64) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_qword_1(q0).map_err(to_value_error)? })
	}

	/// Creates a ``dq``/``.quad`` asm directive
	///
	/// Args:
	///     `q0` (int): (``u64``) Qword 0
	///     `q1` (int): (``u64``) Qword 1
	///
	/// Returns:
	///     :class:`Instruction`: Created instruction
	#[rustfmt::skip]
	#[staticmethod]
	#[pyo3(text_signature = "(q0, q1)")]
	fn create_declare_qword_2(q0: u64, q1: u64) -> PyResult<Self> {
		Ok(Instruction { instr: iced_x86::Instruction::try_with_declare_qword_2(q0, q1).map_err(to_value_error)? })
	}
	// GENERATOR-END: Create

	fn __format__(&self, format_spec: &str) -> PyResult<String> {
		self.format(format_spec)
	}

	fn __repr__(&self) -> PyResult<String> {
		self.format("")
	}

	fn __str__(&self) -> PyResult<String> {
		self.format("")
	}

	fn __richcmp__(&self, other: PyRef<'_, Instruction>, op: CompareOp) -> PyObject {
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

	fn __len__(&self) -> usize {
		self.instr.len()
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
				'_' => fmt_opts.set_digit_separator("_"),
				_ => return Err(PyValueError::new_err(format!("Unknown format specifier '{}' ('{}')", c, format_spec))),
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

/// Contains the FPU ``TOP`` increment, whether it's conditional and whether the instruction writes to ``TOP``
///
/// Args:
///     `increment` (int): (``i32``) Used if `writes_top` is ``True``. Value added to ``TOP``.
///     `conditional` (bool): ``True`` if it's a conditional push/pop (eg. ``FPTAN`` or ``FSINCOS``)
///     `writes_top` (bool): ``True`` if ``TOP`` is written (it's a conditional/unconditional push/pop, ``FNSAVE``, ``FLDENV``, etc)
#[pyclass(module = "iced_x86._iced_x86_py")]
pub(crate) struct FpuStackIncrementInfo {
	info: iced_x86::FpuStackIncrementInfo,
}

#[pymethods]
impl FpuStackIncrementInfo {
	#[new]
	#[pyo3(text_signature = "(increment, conditional, writes_top)")]
	fn new(increment: i32, conditional: bool, writes_top: bool) -> Self {
		Self { info: iced_x86::FpuStackIncrementInfo::new(increment, conditional, writes_top) }
	}

	/// int: (``i32``) Used if :class:`FpuStackIncrementInfo.writes_top` is ``True``. Value added to ``TOP``.
	///
	/// This is negative if it pushes one or more values and positive if it pops one or more values
	/// and ``0`` if it writes to ``TOP`` (eg. ``FLDENV``, etc) without pushing/popping anything.
	#[getter]
	fn increment(&self) -> i32 {
		self.info.increment()
	}

	/// bool: ``True`` if it's a conditional push/pop (eg. ``FPTAN`` or ``FSINCOS``)
	#[getter]
	fn conditional(&self) -> bool {
		self.info.conditional()
	}

	/// bool: ``True`` if ``TOP`` is written (it's a conditional/unconditional push/pop, ``FNSAVE``, ``FLDENV``, etc)
	#[getter]
	fn writes_top(&self) -> bool {
		self.info.writes_top()
	}
}
