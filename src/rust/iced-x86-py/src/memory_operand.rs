// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::enum_utils::to_register;
use core::hash::{Hash, Hasher};
use pyo3::class::basic::CompareOp;
use pyo3::prelude::*;
use std::collections::hash_map::DefaultHasher;

/// Memory operand passed to one of :class:`Instruction`'s `create*()` constructor methods
///
/// See also ``MemoryOperand.ctor_u64()`` if you need to pass in a ``u64`` ``displ`` argument value.
///
/// Args:
///     `base` (:class:`Register`): (default = :class:`Register.NONE`) Base register or :class:`Register.NONE`
///     `index` (:class:`Register`): (default = :class:`Register.NONE`) Index register or :class:`Register.NONE`
///     `scale` (int): (default = ``1``) Index register scale (1, 2, 4, or 8)
///     `displ` (int): (``i64``) (default = ``0``) Memory displacement
///     `displ_size` (int): (default = ``0``) 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
///     `is_broadcast` (bool): (default = ``False``) ``True`` if it's broadcast memory (EVEX instructions)
///     `seg` (:class:`Register`): (default = :class:`Register.NONE`) Segment override or :class:`Register.NONE`
#[pyclass(module = "iced_x86._iced_x86_py")]
#[derive(Copy, Clone)]
pub(crate) struct MemoryOperand {
	pub(crate) mem: iced_x86::MemoryOperand,
}

#[pymethods]
impl MemoryOperand {
	#[new]
	#[pyo3(signature = (base = 0, index = 0, scale = 1, displ = 0, displ_size = 0, is_broadcast = false, seg = 0))]
	fn new(base: u32, index: u32, scale: u32, displ: i64, mut displ_size: u32, is_broadcast: bool, seg: u32) -> PyResult<Self> {
		// #[pyo3(signature = (...))] line assumption
		const _: () = assert!(iced_x86::Register::None as u32 == 0);

		if displ != 0 && displ_size == 0 {
			displ_size = 1
		}

		Ok(Self {
			mem: iced_x86::MemoryOperand::new(to_register(base)?, to_register(index)?, scale, displ, displ_size, is_broadcast, to_register(seg)?),
		})
	}

	/// Memory operand passed to one of :class:`Instruction`'s `create*()` constructor methods
	///
	/// The only difference between this method and the constructor is that this method takes a ``u64`` ``displ`` argument instead of an ``i64``.
	///
	/// Args:
	///     `base` (:class:`Register`): (default = :class:`Register.NONE`) Base register or :class:`Register.NONE`
	///     `index` (:class:`Register`): (default = :class:`Register.NONE`) Index register or :class:`Register.NONE`
	///     `scale` (int): (default = ``1``) Index register scale (1, 2, 4, or 8)
	///     `displ` (int): (``u64``) (default = ``0``) Memory displacement
	///     `displ_size` (int): (default = ``0``) 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
	///     `is_broadcast` (bool): (default = ``False``) ``True`` if it's broadcast memory (EVEX instructions)
	///     `seg` (:class:`Register`): (default = :class:`Register.NONE`) Segment override or :class:`Register.NONE`
	///
	/// Returns:
	///     MemoryOperand: A new instance
	#[pyo3(text_signature = "(base = 0, index = 0, scale = 1, displ = 0, displ_size = 0, is_broadcast = false, seg = 0)")]
	#[pyo3(signature = (base = 0, index = 0, scale = 1, displ = 0, displ_size = 0, is_broadcast = false, seg = 0))]
	#[staticmethod]
	fn ctor_u64(base: u32, index: u32, scale: u32, displ: u64, displ_size: u32, is_broadcast: bool, seg: u32) -> PyResult<Self> {
		MemoryOperand::new(base, index, scale, displ as i64, displ_size, is_broadcast, seg)
	}

	/// Returns a copy of this instance.
	///
	/// Returns:
	///     MemoryOperand: A copy of this instance
	///
	/// This is identical to :class:`MemoryOperand.copy`
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
	///     MemoryOperand: A copy of this instance
	///
	/// This is identical to :class:`MemoryOperand.copy`
	#[pyo3(text_signature = "($self, memo)")]
	fn __deepcopy__(&self, _memo: &Bound<'_, PyAny>) -> Self {
		*self
	}

	/// Returns a copy of this instance.
	///
	/// Returns:
	///     MemoryOperand: A copy of this instance
	#[pyo3(text_signature = "($self)")]
	fn copy(&self) -> Self {
		*self
	}

	fn __richcmp__(&self, other: PyRef<'_, MemoryOperand>, op: CompareOp) -> PyObject {
		match op {
			CompareOp::Eq => (self.mem == other.mem).into_py(other.py()),
			CompareOp::Ne => (self.mem != other.mem).into_py(other.py()),
			_ => other.py().NotImplemented(),
		}
	}

	fn __hash__(&self) -> u64 {
		let mut hasher = DefaultHasher::new();
		self.mem.hash(&mut hasher);
		hasher.finish()
	}
}
