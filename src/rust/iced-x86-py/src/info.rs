// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::instruction::Instruction;
use crate::utils::to_value_error;
use core::hash::{Hash, Hasher};
use pyo3::class::basic::CompareOp;
use pyo3::prelude::*;
use std::collections::hash_map::DefaultHasher;

/// A register used by an instruction
#[pyclass(module = "iced_x86._iced_x86_py")]
#[derive(Copy, Clone)]
pub(crate) struct UsedRegister {
	info: iced_x86::UsedRegister,
}

#[pymethods]
impl UsedRegister {
	/// :class:`Register`: Gets the register
	#[getter]
	fn register(&self) -> u32 {
		self.info.register() as u32
	}

	/// :class:`OpAccess`: Gets the register access
	#[getter]
	fn access(&self) -> u32 {
		self.info.access() as u32
	}

	/// Returns a copy of this instance.
	///
	/// Returns:
	///     UsedRegister: A copy of this instance
	///
	/// This is identical to :class:`UsedRegister.copy`
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
	///     UsedRegister: A copy of this instance
	///
	/// This is identical to :class:`UsedRegister.copy`
	#[pyo3(text_signature = "($self, memo)")]
	fn __deepcopy__(&self, _memo: &Bound<'_, PyAny>) -> Self {
		*self
	}

	/// Returns a copy of this instance.
	///
	/// Returns:
	///     UsedRegister: A copy of this instance
	#[pyo3(text_signature = "($self)")]
	fn copy(&self) -> Self {
		*self
	}

	fn __richcmp__(&self, other: PyRef<'_, UsedRegister>, op: CompareOp) -> PyObject {
		match op {
			CompareOp::Eq => (self.info == other.info).into_py(other.py()),
			CompareOp::Ne => (self.info != other.info).into_py(other.py()),
			_ => other.py().NotImplemented(),
		}
	}

	fn __hash__(&self) -> u64 {
		let mut hasher = DefaultHasher::new();
		self.info.hash(&mut hasher);
		hasher.finish()
	}
}

/// A memory location used by an instruction
#[pyclass(module = "iced_x86._iced_x86_py")]
#[derive(Copy, Clone)]
pub(crate) struct UsedMemory {
	info: iced_x86::UsedMemory,
}

#[pymethods]
impl UsedMemory {
	/// :class:`Register`: Effective segment register or :class:`Register.NONE` if the segment register is ignored
	#[getter]
	fn segment(&self) -> u32 {
		self.info.segment() as u32
	}

	/// :class:`Register`: Base register or :class:`Register.NONE` if none
	#[getter]
	fn base(&self) -> u32 {
		self.info.base() as u32
	}

	/// :class:`Register`: Index register or :class:`Register.NONE` if none
	#[getter]
	fn index(&self) -> u32 {
		self.info.index() as u32
	}

	/// int: Index scale (1, 2, 4 or 8)
	#[getter]
	fn scale(&self) -> u32 {
		self.info.scale()
	}

	/// int: (``u64``) Displacement
	#[getter]
	fn displacement(&self) -> u64 {
		self.info.displacement()
	}

	/// int: (``i64``) Displacement
	#[getter]
	fn displacement_i64(&self) -> i64 {
		self.info.displacement() as i64
	}

	/// :class:`MemorySize`: Size of location (enum value)
	#[getter]
	fn memory_size(&self) -> u32 {
		self.info.memory_size() as u32
	}

	/// :class:`OpAccess`: Memory access
	#[getter]
	fn access(&self) -> u32 {
		self.info.access() as u32
	}

	/// :class:`CodeSize`: Address size
	#[getter]
	fn address_size(&self) -> u32 {
		self.info.address_size() as u32
	}

	/// int: VSIB size (`0`, `4` or `8`)
	#[getter]
	fn vsib_size(&self) -> u32 {
		self.info.vsib_size()
	}

	/// Returns a copy of this instance.
	///
	/// Returns:
	///     UsedMemory: A copy of this instance
	///
	/// This is identical to :class:`UsedMemory.copy`
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
	///     UsedMemory: A copy of this instance
	///
	/// This is identical to :class:`UsedMemory.copy`
	#[pyo3(text_signature = "($self, memo)")]
	fn __deepcopy__(&self, _memo: &Bound<'_, PyAny>) -> Self {
		*self
	}

	/// Returns a copy of this instance.
	///
	/// Returns:
	///     UsedMemory: A copy of this instance
	#[pyo3(text_signature = "($self)")]
	fn copy(&self) -> Self {
		*self
	}

	fn __richcmp__(&self, other: PyRef<'_, UsedMemory>, op: CompareOp) -> PyObject {
		match op {
			CompareOp::Eq => (self.info == other.info).into_py(other.py()),
			CompareOp::Ne => (self.info != other.info).into_py(other.py()),
			_ => other.py().NotImplemented(),
		}
	}

	fn __hash__(&self) -> u64 {
		let mut hasher = DefaultHasher::new();
		self.info.hash(&mut hasher);
		hasher.finish()
	}
}

/// Contains accessed registers and memory locations
#[pyclass(module = "iced_x86._iced_x86_py")]
pub(crate) struct InstructionInfo {
	info: iced_x86::InstructionInfo,
}

#[pymethods]
impl InstructionInfo {
	/// Gets all accessed registers.
	///
	/// This method doesn't return all accessed registers if :class:`Instruction.is_save_restore_instruction` is ``True``.
	///
	/// Returns:
	///     List[UsedRegister]: All accessed registers
	///
	/// Some instructions have a ``r16``/``r32`` operand but only use the low 8 bits of the register. In that case
	/// this method returns the 8-bit register even if it's ``SPL``, ``BPL``, ``SIL``, ``DIL`` and the
	/// instruction was decoded in 16 or 32-bit mode. This is more accurate than returning the ``r16``/``r32``
	/// register. Example instructions that do this: ``PINSRB``, ``ARPL``
	#[pyo3(text_signature = "($self)")]
	fn used_registers(&self) -> Vec<UsedRegister> {
		self.info.used_registers().iter().map(|a| UsedRegister { info: *a }).collect()
	}

	/// Gets all accessed memory locations
	///
	/// Returns:
	///     List[UsedMemory]: All accessed memory locations
	#[pyo3(text_signature = "($self)")]
	fn used_memory(&self) -> Vec<UsedMemory> {
		self.info.used_memory().iter().map(|a| UsedMemory { info: *a }).collect()
	}

	/// :class:`OpAccess`: Operand #0 access
	#[getter]
	fn op0_access(&self) -> u32 {
		self.info.op0_access() as u32
	}

	/// :class:`OpAccess`: Operand #1 access
	#[getter]
	fn op1_access(&self) -> u32 {
		self.info.op1_access() as u32
	}

	/// :class:`OpAccess`: Operand #2 access
	#[getter]
	fn op2_access(&self) -> u32 {
		self.info.op2_access() as u32
	}

	/// :class:`OpAccess`: Operand #3 access
	#[getter]
	fn op3_access(&self) -> u32 {
		self.info.op3_access() as u32
	}

	/// :class:`OpAccess`: Operand #4 access
	#[getter]
	fn op4_access(&self) -> u32 {
		self.info.op4_access() as u32
	}

	/// Gets operand access
	///
	/// Args:
	///     `operand` (int): Operand number, 0-4
	///
	/// Returns:
	///     :class:`OpAccess`: Operand access
	///
	/// Raises:
	///     ValueError: If `operand` is invalid
	#[pyo3(text_signature = "($self, operand)")]
	fn op_access(&self, operand: u32) -> PyResult<u32> {
		self.info.try_op_access(operand).map_or_else(|e| Err(to_value_error(e)), |op_access| Ok(op_access as u32))
	}
}

/// Returns used registers and memory locations
///
/// Examples:
///
/// .. testcode::
///
///     from iced_x86 import *
///
///     # add [rdi+r12*8-5AA5EDCCh],esi
///     data = b"\x42\x01\xB4\xE7\x34\x12\x5A\xA5"
///     decoder = Decoder(64, data)
///
///     def create_enum_dict(module):
///         return {module.__dict__[key]:key for key in module.__dict__ if isinstance(module.__dict__[key], int)}
///
///     reg_to_str = create_enum_dict(Register)
///     op_access_to_str = create_enum_dict(OpAccess)
///     memsz_to_str = create_enum_dict(MemorySize)
///
///     info_factory = InstructionInfoFactory()
///     for instr in decoder:
///         print(f"Instruction: {instr}")
///
///         info = info_factory.info(instr)
///
///         for mem_info in info.used_memory():
///             # Register and OpAccess enum values
///             print(f"Used memory:")
///             print(f"  seg: {reg_to_str[mem_info.segment]}")
///             print(f"  base: {reg_to_str[mem_info.base]}")
///             print(f"  index: {reg_to_str[mem_info.index]}")
///             print(f"  scale: {mem_info.scale}")
///             print(f"  displacement: 0x{mem_info.displacement:X}")
///             print(f"  MemorySize enum: {memsz_to_str[mem_info.memory_size]}")
///             print(f"  OpAccess enum: {op_access_to_str[mem_info.access]}")
///
///         for reg_info in info.used_registers():
///             print(f"Used register: reg={reg_to_str[reg_info.register]} access={op_access_to_str[reg_info.access]}")
///
/// Output:
///
/// .. testoutput::
///
///     Instruction: add [rdi+r12*8-5AA5EDCCh],esi
///     Used memory:
///       seg: DS
///       base: RDI
///       index: R12
///       scale: 8
///       displacement: 0xFFFFFFFFA55A1234
///       MemorySize enum: UINT32
///       OpAccess enum: READ_WRITE
///     Used register: reg=RDI access=READ
///     Used register: reg=R12 access=READ
///     Used register: reg=ESI access=READ
#[pyclass(module = "iced_x86._iced_x86_py")]
pub(crate) struct InstructionInfoFactory {
	info: iced_x86::InstructionInfoFactory,
}

#[pymethods]
impl InstructionInfoFactory {
	#[new]
	#[pyo3(text_signature = "()")]
	fn new() -> Self {
		Self { info: iced_x86::InstructionInfoFactory::new() }
	}

	/// Gets all accessed registers and memory locations
	///
	/// Args:
	///     `instruction` (Instruction): The instruction that should be analyzed
	///
	/// Returns:
	///     InstructionInfo: Accessed registers and memory locations
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     # add [rdi+r12*8-5AA5EDCCh],esi
	///     data = b"\x42\x01\xB4\xE7\x34\x12\x5A\xA5"
	///     decoder = Decoder(64, data)
	///     info_factory = InstructionInfoFactory()
	///
	///     instr = decoder.decode()
	///     info = info_factory.info(instr)
	///
	///     mem_list = info.used_memory()
	///     assert len(mem_list) == 1
	///     mem = mem_list[0]
	///     assert mem.segment == Register.DS
	///     assert mem.base == Register.RDI
	///     assert mem.index == Register.R12
	///     assert mem.scale == 8
	///     assert mem.displacement == 0xFFFFFFFFA55A1234
	///     assert mem.memory_size == MemorySize.UINT32
	///     assert mem.access == OpAccess.READ_WRITE
	///
	///     regs = info.used_registers()
	///     assert len(regs) == 3
	///     assert regs[0].register == Register.RDI
	///     assert regs[0].access == OpAccess.READ
	///     assert regs[1].register == Register.R12
	///     assert regs[1].access == OpAccess.READ
	///     assert regs[2].register == Register.ESI
	///     assert regs[2].access == OpAccess.READ
	#[pyo3(text_signature = "($self, instruction)")]
	fn info(&mut self, instruction: &Instruction) -> InstructionInfo {
		InstructionInfo { info: self.info.info(&instruction.instr).clone() }
	}
}
