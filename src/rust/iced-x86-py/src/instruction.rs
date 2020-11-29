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

use crate::enum_utils::{to_code, to_code_size, to_op_kind, to_register, to_rounding_control};
use crate::iced_constants::IcedConstants;
use crate::op_code_info::OpCodeInfo;
use core::hash::{Hash, Hasher};
use pyo3::class::basic::CompareOp;
use pyo3::class::PySequenceProtocol;
use pyo3::exceptions::PyValueError;
use pyo3::prelude::*;
use pyo3::PyObjectProtocol;
use std::collections::hash_map::DefaultHasher;

/// A 16/32/64-bit x86 instruction. Created by :class:`Decoder` or by ``Instruction::create*()`` methods.
///
/// Examples:
///
/// A decoder is usually used to create instructions:
///
/// .. code-block:: python
///
///     from iced_x86 import *
///
///     # xchg ah,[rdx+rsi+16h]
///     data = b"\x86\x64\x32\x16"
///     decoder = Decoder(64, data)
///     decoder.ip = 0x1234_5678
///
///     instr = decoder.decode()
///
///     # Instruction supports __bool__() and returns True if it's a valid instruction:
///     if not instr:
///         print("Invalid instruction (garbage, data, or a future instr (update iced))")
///     # The above code is the same as:
///     if instr.code == Code.INVALID:
///         print("Not an instruction")
///
/// Once you have an instruction you can format it either by using a :class:`Formatter`
/// or by calling the instruction's ``__repr__()``, ``__str__()`` or ``__format__()`` methods.
///
/// .. code-block:: python
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
///     print(f"mnemonic: {formatter.format_mnemonic(instr)}")
///     print(f"operands: {formatter.format_all_operands(instr)}")
///     print(f"op #0   : {formatter.format_operand(instr, 0)}")
///     print(f"op #1   : {formatter.format_operand(instr, 1)}")
///     print(f"reg RCX : {formatter.format_register(Register.RCX)}")
///     # The code prints this:
///     # disasm  : XCHG    [rdx+rsi+16h], ah
///     # mnemonic: XCHG
///     # operands: [rdx+rsi+16h], ah
///     # op #0   : [rdx+rsi+16h]
///     # op #1   : ah
///     # reg RCX : rcx
///
///     # A formatter isn't needed if you like most of the default options.
///     # repr() == str() == format() all return the same thing.
///     print(f"disasm  : {repr(instr)}")
///     print(f"disasm  : {str(instr)}")
///     print(f"disasm  : {instr}")
///     # The code prints this:
///     # disasm  : xchg ah,[rdx+rsi+16h]
///     # disasm  : xchg ah,[rdx+rsi+16h]
///     # disasm  : xchg ah,[rdx+rsi+16h]
///
///     # __format__() supports a format spec argument, see the table below
///     print(f"disasm  : {instr:f}")
///     print(f"disasm  : {instr:g}")
///     print(f"disasm  : {instr:i}")
///     print(f"disasm  : {instr:m}")
///     print(f"disasm  : {instr:n}")
///     print(f"disasm  : {instr:gxsSG}")
///     # The code prints this:
///     # disasm  : xchg [rdx+rsi+16h],ah
///     # disasm  : xchg %ah,0x16(%rdx,%rsi)
///     # disasm  : xchg [rdx+rsi+16h],ah
///     # disasm  : xchg ah,[rdx+rsi+16h]
///     # disasm  : xchg ah,[rdx+rsi+16h]
///     # disasm  : xchgb %ah, %ds:0x16(%rdx,%rsi)
///
/// The following format specifiers are supported in any order. If you omit the
/// formatter kind, the default formatter is used (eg. masm):
///
/// ======  =============================================================================
/// Option  Description
/// ======  =============================================================================
/// f       Fast formatter (masm-like syntax)
/// g       GNU Assembler formatter
/// i       Intel (XED) formatter
/// m       masm formatter
/// n       nasm formatter
/// X       Uppercase hex numbers with ``0x`` prefix
/// x       Lowercase hex numbers with ``0x`` prefix
/// H       Uppercase hex numbers with ``h`` suffix
/// h       Lowercase hex numbers with ``h`` suffix
/// r       RIP-relative memory operands use RIP register instead of abs addr (``[rip+123h]`` vs ``[123456789ABCDEF0h]``)
/// U       Uppercase everything except numbers and hex prefixes/suffixes (ignored by fast fmt)
/// s       Add a space after the operand separator
/// S       Always show the segment register
/// B       Don't show the branch size (``SHORT`` or ``NEAR PTR``) (ignored by fast fmt)
/// G       (GNU Assembler): Add mnemonic size suffix (eg. ``movl`` vs ``mov``)
/// M       Always show the memory size (eg. ``BYTE PTR``) even when not needed
/// _       Use digit separators (eg. ``0x12345678`` vs ``0x1234_5678``) (ignored by fast fmt)
/// ======  =============================================================================
#[pyclass(module = "_iced_x86_py")]
#[text_signature = "(/)"]
#[derive(Copy, Clone)]
pub struct Instruction {
	pub(crate) instr: iced_x86::Instruction,
}

impl Instruction {
	const MAX_DB_COUNT: usize = 16;
	const MAX_DW_COUNT: usize = Instruction::MAX_DB_COUNT / 2;
	const MAX_DD_COUNT: usize = Instruction::MAX_DB_COUNT / 4;
	const MAX_DQ_COUNT: usize = Instruction::MAX_DB_COUNT / 8;

	fn check_db_index(index: usize) -> PyResult<usize> {
		if index >= Instruction::MAX_DB_COUNT {
			Err(PyValueError::new_err("Invalid db index"))
		} else {
			Ok(index)
		}
	}

	fn check_dw_index(index: usize) -> PyResult<usize> {
		if index >= Instruction::MAX_DW_COUNT {
			Err(PyValueError::new_err("Invalid dw index"))
		} else {
			Ok(index)
		}
	}

	fn check_dd_index(index: usize) -> PyResult<usize> {
		if index >= Instruction::MAX_DD_COUNT {
			Err(PyValueError::new_err("Invalid dd index"))
		} else {
			Ok(index)
		}
	}

	fn check_dq_index(index: usize) -> PyResult<usize> {
		if index >= Instruction::MAX_DQ_COUNT {
			Err(PyValueError::new_err("Invalid dq index"))
		} else {
			Ok(index)
		}
	}

	fn check_immediate_op_kind(&self, operand: u32) -> PyResult<()> {
		match self.instr.op_kind(operand) {
			iced_x86::OpKind::Immediate8
			| iced_x86::OpKind::Immediate8to16
			| iced_x86::OpKind::Immediate8to32
			| iced_x86::OpKind::Immediate8to64
			| iced_x86::OpKind::Immediate8_2nd
			| iced_x86::OpKind::Immediate16
			| iced_x86::OpKind::Immediate32to64
			| iced_x86::OpKind::Immediate32
			| iced_x86::OpKind::Immediate64 => Ok(()),
			_ => Err(PyValueError::new_err("Operand is not an immediate")),
		}
	}
}

#[pymethods]
impl Instruction {
	#[new]
	pub(crate) fn new() -> Self {
		Self { instr: iced_x86::Instruction::default() }
	}

	/// Returns a copy of this instance.
	///
	/// Returns:
	///     Instruction: A copy of this instance
	///
	/// This is identical to :class:`Instruction.clone`
	#[text_signature = "($self, /)"]
	fn __copy__(&self) -> Self {
		*self
	}

	/// Returns a copy of this instance.
	///
	/// Returns:
	///     Instruction: A copy of this instance
	///
	/// This is identical to :class:`Instruction.clone`
	#[text_signature = "($self, memo, /)"]
	fn __deepcopy__(&self, _memo: &PyAny) -> Self {
		*self
	}

	/// Returns a copy of this instance.
	///
	/// Returns:
	///     Instruction: A copy of this instance
	#[text_signature = "($self, /)"]
	fn clone(&self) -> Self {
		*self
	}

	/// Checks if two instructions are equal, comparing all bits, not ignoring anything. ``==`` ignores some fields.
	///
	/// Returns:
	///     bool: `True` if `other` is exactly identical to this instance
	#[text_signature = "($self, other, /)"]
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
	/// .. code-block:: python
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
	///     This is just informational. If you modify the instruction or create a new one,
	///     this method could return the wrong value.
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
		if new_value != iced_x86::OpKind::Immediate8 as u32 {
			Err(PyValueError::new_err("Invalid op kind"))
		} else {
			self.instr.set_op4_kind(to_op_kind(new_value)?);
			Ok(())
		}
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
	/// .. code-block:: python
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
	#[text_signature = "($self, operand, /)"]
	fn op_kind(&self, operand: u32) -> PyResult<u32> {
		const_assert_eq!(5, IcedConstants::MAX_OP_COUNT);
		match operand {
			0 => Ok(self.op0_kind()),
			1 => Ok(self.op1_kind()),
			2 => Ok(self.op2_kind()),
			3 => Ok(self.op3_kind()),
			4 => Ok(self.op4_kind()),
			_ => Err(PyValueError::new_err("Invalid operand")),
		}
	}

	/// Sets an operand's kind
	///
	/// Args:
	///     `operand` (int): Operand number, 0-4
	///     `op_kind` (:class:`OpKind`): Operand kind
	///
	/// Raises:
	///     ValueError: If `operand` is invalid
	#[text_signature = "($self, operand, op_kind, /)"]
	fn set_op_kind(&mut self, operand: u32, op_kind: u32) -> PyResult<()> {
		const_assert_eq!(5, IcedConstants::MAX_OP_COUNT);
		match operand {
			0 => self.set_op0_kind(op_kind),
			1 => self.set_op1_kind(op_kind),
			2 => self.set_op2_kind(op_kind),
			3 => self.set_op3_kind(op_kind),
			4 => self.set_op4_kind(op_kind),
			_ => Err(PyValueError::new_err("Invalid operand")),
		}
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
	/// Use this method if the operand has kind :class:`OpKind.MEMORY`, :class:`OpKind.MEMORY64`,
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
	/// Use this method if the operand has kind :class:`OpKind.MEMORY`, :class:`OpKind.MEMORY64`,
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
	/// a signed byte if it's an EVEX encoded instruction.
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

	/// bool: ``True`` if the data is broadcasted (EVEX instructions only)
	#[getter]
	fn is_broadcast(&self) -> bool {
		self.instr.is_broadcast()
	}

	#[setter]
	fn set_is_broadcast(&mut self, new_value: bool) {
		self.instr.set_is_broadcast(new_value)
	}

	/// :class:`MemorySize`: Gets the size of the memory location (a :class:`MemorySize` enum value) that is referenced by the operand.
	///
	/// See also :class:`Instruction.is_broadcast`.
	///
	/// Use this method if the operand has kind :class:`OpKind.MEMORY`, :class:`OpKind.MEMORY64`,
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

	/// int: (``u32``) Gets the memory operand's displacement.
	///
	/// This should be sign extended to 64 bits if it's 64-bit addressing (see :class:`Instruction.memory_displacement64`).
	///
	/// Use this method if the operand has kind :class:`OpKind.MEMORY`
	#[getter]
	fn memory_displacement(&self) -> u32 {
		self.instr.memory_displacement()
	}

	#[setter]
	fn set_memory_displacement(&mut self, new_value: u32) {
		self.instr.set_memory_displacement(new_value)
	}

	/// int: (``u64``) Gets the memory operand's displacement sign extended to 64 bits.
	///
	/// Use this method if the operand has kind :class:`OpKind.MEMORY`
	#[getter]
	fn memory_displacement64(&self) -> u64 {
		self.instr.memory_displacement64()
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
	#[text_signature = "($self, operand, /)"]
	fn immediate(&self, operand: u32) -> PyResult<u64> {
		if let Some(value) = self.instr.try_immediate(operand) {
			Ok(value)
		} else {
			Err(PyValueError::new_err("Not an immediate operand"))
		}
	}

	/// Sets an operand's immediate value
	///
	/// Args:
	///     `operand` (int): Operand number, 0-4
	///     `new_value` (int): (``i32``) Immediate
	///
	/// Raises:
	///     ValueError: If `operand` is invalid or if it's not an immediate operand
	#[text_signature = "($self, operand, new_value, /)"]
	fn set_immediate_i32(&mut self, operand: u32, new_value: i32) -> PyResult<()> {
		self.check_immediate_op_kind(operand)?;
		self.instr.set_immediate_i32(operand, new_value);
		Ok(())
	}

	/// Sets an operand's immediate value
	///
	/// Args:
	///     `operand` (int): Operand number, 0-4
	///     `new_value` (int): (``u32``) Immediate
	///
	/// Raises:
	///     ValueError: If `operand` is invalid or if it's not an immediate operand
	#[text_signature = "($self, operand, new_value, /)"]
	fn set_immediate_u32(&mut self, operand: u32, new_value: u32) -> PyResult<()> {
		self.check_immediate_op_kind(operand)?;
		self.instr.set_immediate_u32(operand, new_value);
		Ok(())
	}

	/// Sets an operand's immediate value
	///
	/// Args:
	///     `operand` (int): Operand number, 0-4
	///     `new_value` (int): (``i64``) Immediate
	///
	/// Raises:
	///     ValueError: If `operand` is invalid or if it's not an immediate operand
	#[text_signature = "($self, operand, new_value, /)"]
	fn set_immediate_i64(&mut self, operand: u32, new_value: i64) -> PyResult<()> {
		self.check_immediate_op_kind(operand)?;
		self.instr.set_immediate_i64(operand, new_value);
		Ok(())
	}

	/// Sets an operand's immediate value
	///
	/// Args:
	///     `operand` (int): Operand number, 0-4
	///     `new_value` (int): (``u64``) Immediate
	///
	/// Raises:
	///     ValueError: If `operand` is invalid or if it's not an immediate operand
	#[text_signature = "($self, operand, new_value, /)"]
	fn set_immediate_u64(&mut self, operand: u32, new_value: u64) -> PyResult<()> {
		self.check_immediate_op_kind(operand)?;
		self.instr.set_immediate_u64(operand, new_value);
		Ok(())
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

	/// int: (``u64``) Gets the operand's 64-bit address value.
	///
	/// Use this method if the operand has kind :class:`OpKind.MEMORY64`
	#[getter]
	fn memory_address64(&self) -> u64 {
		self.instr.memory_address64()
	}

	#[setter]
	fn set_memory_address64(&mut self, new_value: u64) {
		self.instr.set_memory_address64(new_value);
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
		if new_value != iced_x86::Register::None as u32 {
			Err(PyValueError::new_err("Invalid register"))
		} else {
			self.instr.set_op4_register(to_register(new_value)?);
			Ok(())
		}
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
	/// .. code-block:: python
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
	#[text_signature = "($self, operand, /)"]
	fn op_register(&self, operand: u32) -> PyResult<u32> {
		const_assert_eq!(5, IcedConstants::MAX_OP_COUNT);
		match operand {
			0 => Ok(self.op0_register()),
			1 => Ok(self.op1_register()),
			2 => Ok(self.op2_register()),
			3 => Ok(self.op3_register()),
			4 => Ok(self.op4_register()),
			_ => Err(PyValueError::new_err("Invalid operand")),
		}
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
	#[text_signature = "($self, operand, new_value, /)"]
	fn set_op_register(&mut self, operand: u32, new_value: u32) -> PyResult<()> {
		const_assert_eq!(5, IcedConstants::MAX_OP_COUNT);
		match operand {
			0 => self.set_op0_register(new_value),
			1 => self.set_op1_register(new_value),
			2 => self.set_op2_register(new_value),
			3 => self.set_op3_register(new_value),
			4 => self.set_op4_register(new_value),
			_ => Err(PyValueError::new_err("Invalid operand")),
		}
	}

	/// :class:`Register`: Gets the op mask register (:class:`Register.K1` - :class:`Register.K7`) or :class:`Register.NONE` if none (a :class:`Register` enum value)
	#[getter]
	fn op_mask(&self) -> u32 {
		self.instr.op_mask() as u32
	}

	#[setter]
	fn set_op_mask(&mut self, new_value: u32) -> PyResult<()> {
		self.instr.set_op_mask(to_register(new_value)?);
		Ok(())
	}

	/// bool: Checks if there's an op mask register (:class:`Instruction.op_mask`)
	#[getter]
	fn has_op_mask(&self) -> bool {
		self.instr.has_op_mask()
	}

	/// bool: ``True`` if zeroing-masking, ``False`` if merging-masking.
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

	/// bool: ``True`` if merging-masking, ``False`` if zeroing-masking.
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
	#[text_signature = "($self, index, new_value, /)"]
	fn set_declare_byte_value_i8(&mut self, index: usize, new_value: i8) -> PyResult<()> {
		self.instr.set_declare_byte_value_i8(Instruction::check_db_index(index)?, new_value);
		Ok(())
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
	#[text_signature = "($self, index, new_value, /)"]
	fn set_declare_byte_value(&mut self, index: usize, new_value: u8) -> PyResult<()> {
		self.instr.set_declare_byte_value(Instruction::check_db_index(index)?, new_value);
		Ok(())
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
	#[text_signature = "($self, index, /)"]
	fn get_declare_byte_value(&self, index: usize) -> PyResult<u8> {
		Ok(self.instr.get_declare_byte_value(Instruction::check_db_index(index)?))
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
	#[text_signature = "($self, index, new_value, /)"]
	fn set_declare_word_value_i16(&mut self, index: usize, new_value: i16) -> PyResult<()> {
		self.instr.set_declare_word_value_i16(Instruction::check_dw_index(index)?, new_value);
		Ok(())
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
	#[text_signature = "($self, index, new_value, /)"]
	fn set_declare_word_value(&mut self, index: usize, new_value: u16) -> PyResult<()> {
		self.instr.set_declare_word_value(Instruction::check_dw_index(index)?, new_value);
		Ok(())
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
	#[text_signature = "($self, index, /)"]
	fn get_declare_word_value(&self, index: usize) -> PyResult<u16> {
		Ok(self.instr.get_declare_word_value(Instruction::check_dw_index(index)?))
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
	#[text_signature = "($self, index, new_value, /)"]
	fn set_declare_dword_value_i32(&mut self, index: usize, new_value: i32) -> PyResult<()> {
		self.instr.set_declare_dword_value_i32(Instruction::check_dd_index(index)?, new_value);
		Ok(())
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
	#[text_signature = "($self, index, new_value, /)"]
	fn set_declare_dword_value(&mut self, index: usize, new_value: u32) -> PyResult<()> {
		self.instr.set_declare_dword_value(Instruction::check_dd_index(index)?, new_value);
		Ok(())
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
	#[text_signature = "($self, index, /)"]
	fn get_declare_dword_value(&self, index: usize) -> PyResult<u32> {
		Ok(self.instr.get_declare_dword_value(Instruction::check_dd_index(index)?))
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
	#[text_signature = "($self, index, new_value, /)"]
	fn set_declare_qword_value_i64(&mut self, index: usize, new_value: i64) -> PyResult<()> {
		self.instr.set_declare_qword_value_i64(Instruction::check_dq_index(index)?, new_value);
		Ok(())
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
	#[text_signature = "($self, index, new_value, /)"]
	fn set_declare_qword_value(&mut self, index: usize, new_value: u64) -> PyResult<()> {
		self.instr.set_declare_qword_value(Instruction::check_dq_index(index)?, new_value);
		Ok(())
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
	#[text_signature = "($self, index, /)"]
	fn get_declare_qword_value(&self, index: usize) -> PyResult<u64> {
		Ok(self.instr.get_declare_qword_value(Instruction::check_dq_index(index)?))
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

	/// Checks if it's a vsib instruction.
	///
	/// - Returns ``True`` if it's a VSIB instruction with 64-bit indexes
	/// - Returns ``False`` if it's a VSIB instruction with 32-bit indexes
	/// - Returns `None` if it's not a VSIB instruction.
	///
	/// Returns:
	///     bool, None: See above
	#[getter]
	fn vsib(&self) -> Option<bool> {
		self.instr.vsib()
	}

	/// bool: Gets the suppress all exceptions flag (EVEX encoded instructions). Note that if :class:`Instruction.rounding_control` is not :class:`RoundingControl.NONE`, SAE is implied but this method will still return ``False``.
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

	/// int: (``u64``) Gets the ``RIP``/``EIP`` releative address ((:class:`Instruction.next_ip` or :class:`Instruction.next_ip32`) + :class:`Instruction.memory_displacement`).
	///
	/// This method is only valid if there's a memory operand with ``RIP``/``EIP`` relative addressing, see :class:`Instruction.is_ip_rel_memory_operand`
	#[getter]
	fn ip_rel_memory_address(&self) -> u64 {
		self.instr.ip_rel_memory_address()
	}

	/// Gets the virtual address of a memory operand
	///
	/// Args:
	///     `operand` (int): Operand number, 0-4, must be a memory operand
	///     `element_index` (int): Only used if it's a vsib memory operand. This is the element index of the vector index register.
	///
	/// Returns:
	///     int, None: (``u64``) TODO:
	///
	/// Raises:
	///     ValueError: If `operand` is invalid
	///
	/// Examples:
	///
	/// .. code-block:: python
	///
	///     #TODO: Add an example here
	#[text_signature = "($self, operand, element_index, /)"]
	#[allow(clippy::unused_self)] //TODO:
	fn try_virtual_address(&self, _operand: u32, _element_index: usize) -> Option<u64> {
		None
	}
}

/// Contains the FPU ``TOP`` increment, whether it's conditional and whether the instruction writes to ``TOP``
///
/// Args:
///     `increment` (int): (``i32``) Used if `writes_top` is ``True``. Value added to ``TOP``.
///     `conditional` (bool): ``True`` if it's a conditional push/pop (eg. ``FPTAN`` or ``FSINCOS``)
///     `writes_top` (bool): ``True`` if ``TOP`` is written (it's a conditional/unconditional push/pop, ``FNSAVE``, ``FLDENV``, etc)
#[pyclass(module = "_iced_x86_py")]
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

#[pymethods]
impl Instruction {
	/// int: (``i32``) Gets the number of bytes added to ``SP``/``ESP``/``RSP`` or 0 if it's not an instruction that pushes or pops data.
	///
	/// This method assumes the instruction doesn't change the privilege level (eg. ``IRET/D/Q``). If it's the ``LEAVE``
	/// instruction, this method returns 0.
	///
	/// Examples:
	///
	/// .. code-block:: python
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

	/// Gets the FPU status word's ``TOP`` increment and whether it's a conditional or unconditional push/pop and whether ``TOP`` is written.
	///
	/// Returns:
	///     :class:`FpuStackIncrementInfo`: FPU stack info
	///
	/// Examples:
	///
	/// .. code-block:: python
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
	#[text_signature = "($self, /)"]
	fn fpu_stack_increment_info(&self) -> FpuStackIncrementInfo {
		FpuStackIncrementInfo { info: self.instr.fpu_stack_increment_info() }
	}

	/// :class:`EncodingKind`: Instruction encoding, eg. Legacy, 3DNow!, VEX, EVEX, XOP (an :class:`EncodingKind` enum value)
	///
	/// Examples:
	///
	/// .. code-block:: python
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
	/// Examples:
	///
	/// .. code-block:: python
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
	#[text_signature = "($self, /)"]
	fn cpuid_features(&self) -> Vec<u32> {
		self.instr.cpuid_features().iter().map(|x| *x as u32).collect()
	}

	/// :class:`FlowControl`: Control flow info (a :class:`FlowControl` enum value)
	///
	/// Examples:
	///
	/// .. code-block:: python
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
	/// .. code-block:: python
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

	/// :class:`RflagsBits`: All flags that are read by the CPU when executing the instruction.
	///
	/// This method returns a :class:`RflagsBits` value. See also :class:`Instruction.rflags_modified`.
	///
	/// Examples:
	///
	/// .. code-block:: python
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
	/// This method returns a :class:`RflagsBits` value. See also :class:`Instruction.rflags_modified`.
	///
	/// Examples:
	///
	/// .. code-block:: python
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
	/// This method returns a :class:`RflagsBits` value. See also :class:`Instruction.rflags_modified`.
	///
	/// Examples:
	///
	/// .. code-block:: python
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
	/// This method returns a :class:`RflagsBits` value. See also :class:`Instruction.rflags_modified`.
	///
	/// Examples:
	///
	/// .. code-block:: python
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
	/// This method returns a :class:`RflagsBits` value. See also :class:`Instruction.rflags_modified`.
	///
	/// Examples:
	///
	/// .. code-block:: python
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
	/// This method returns a :class:`RflagsBits` value.
	///
	/// Examples:
	///
	/// .. code-block:: python
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

	/// Negates the condition code, eg. ``JE`` -> ``JNE``.
	///
	/// Can be used if it's ``Jcc``, ``SETcc``, ``CMOVcc``, ``LOOPcc`` and does nothing if the instruction doesn't have a condition code.
	///
	/// Examples:
	///
	/// .. code-block:: python
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
	#[text_signature = "($self, /)"]
	fn negate_condition_code(&mut self) {
		self.instr.negate_condition_code()
	}

	/// Converts ``Jcc/JMP NEAR`` to ``Jcc/JMP SHORT`` and does nothing if it's not a ``Jcc/JMP NEAR`` instruction
	///
	/// Examples:
	///
	/// .. code-block:: python
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
	#[text_signature = "($self, /)"]
	fn as_short_branch(&mut self) {
		self.instr.as_short_branch()
	}

	/// Converts ``Jcc/JMP SHORT`` to ``Jcc/JMP NEAR`` and does nothing if it's not a ``Jcc/JMP SHORT`` instruction
	///
	/// Examples:
	///
	/// .. code-block:: python
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
	#[text_signature = "($self, /)"]
	fn as_near_branch(&mut self) {
		self.instr.as_near_branch()
	}

	/// :class:`ConditionCode`: Gets the condition code (a :class:`ConditionCode` enum value) if it's ``Jcc``, ``SETcc``, ``CMOVcc``, ``LOOPcc`` else :class:`ConditionCode.NONE` is returned
	///
	/// Examples:
	///
	/// .. code-block:: python
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
}

#[pymethods]
impl Instruction {
	/// Gets the :class:`OpCodeInfo`
	///
	/// Returns:
	///     :class:`OpCodeInfo`: Op code info
	#[text_signature = "(/)"]
	fn op_code(&self) -> PyResult<OpCodeInfo> {
		OpCodeInfo::new(self.instr.code() as u32)
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