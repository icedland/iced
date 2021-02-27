// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::enum_utils::to_register;
use core::hash::{Hash, Hasher};
use pyo3::class::basic::CompareOp;
use pyo3::prelude::*;
use pyo3::PyObjectProtocol;
use static_assertions::const_assert_eq;
use std::collections::hash_map::DefaultHasher;

/// Memory operand passed to one of :class:`Instruction`'s `create*()` constructor methods
///
/// Args:
///     `base` (:class:`Register`): (default = :class:`Register.NONE`) Base register or :class:`Register.NONE`
///     `index` (:class:`Register`): (default = :class:`Register.NONE`) Index register or :class:`Register.NONE`
///     `scale` (int): (default = ``1``) Index register scale (1, 2, 4, or 8)
///     `displ` (int): (``i32``) (default = ``0``) Memory displacement
///     `displ_size` (int): (default = ``0``) 0 (no displ), 1 (16/32/64-bit, but use 2/4/8 if it doesn't fit in a `i8`), 2 (16-bit), 4 (32-bit) or 8 (64-bit)
///     `is_broadcast` (bool): (default = ``False``) ``True`` if it's broadcasted memory (EVEX instructions)
///     `seg` (:class:`Register`): (default = :class:`Register.NONE`) Segment override or :class:`Register.NONE`
#[pyclass(module = "_iced_x86_py")]
#[text_signature = "(base, index, scale, displ, displ_size, is_broadcast, seg, /)"]
#[derive(Copy, Clone)]
pub(crate) struct MemoryOperand {
	pub(crate) mem: iced_x86::MemoryOperand,
}

#[pymethods]
impl MemoryOperand {
	#[new]
	#[args(base = 0, index = 0, scale = 1, displ = 0, displ_size = 0, is_broadcast = false, seg = 0)]
	fn new(base: u32, index: u32, scale: u32, displ: i64, mut displ_size: u32, is_broadcast: bool, seg: u32) -> PyResult<Self> {
		// #[args] line assumption
		const_assert_eq!(iced_x86::Register::None as u32, 0);

		if displ != 0 && displ_size == 0 {
			displ_size = 1
		}

		Ok(Self {
			mem: iced_x86::MemoryOperand::new(to_register(base)?, to_register(index)?, scale, displ, displ_size, is_broadcast, to_register(seg)?),
		})
	}

	/// Returns a copy of this instance.
	///
	/// Returns:
	///     MemoryOperand: A copy of this instance
	///
	/// This is identical to :class:`MemoryOperand.copy`
	#[text_signature = "($self, /)"]
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
	#[text_signature = "($self, memo, /)"]
	fn __deepcopy__(&self, _memo: &PyAny) -> Self {
		*self
	}

	/// Returns a copy of this instance.
	///
	/// Returns:
	///     MemoryOperand: A copy of this instance
	#[text_signature = "($self, /)"]
	fn copy(&self) -> Self {
		*self
	}
}

#[pyproto]
impl PyObjectProtocol for MemoryOperand {
	fn __richcmp__(&self, other: PyRef<MemoryOperand>, op: CompareOp) -> PyObject {
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
