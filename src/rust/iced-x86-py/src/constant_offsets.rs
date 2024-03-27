// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use core::hash::{Hash, Hasher};
use pyo3::class::basic::CompareOp;
use pyo3::prelude::*;
use std::collections::hash_map::DefaultHasher;

/// Contains the offsets of the displacement and immediate.
///
/// Call :class:`Decoder.get_constant_offsets` or :class:`Encoder.get_constant_offsets` to get the
/// offsets of the constants after the instruction has been decoded/encoded.
#[pyclass(module = "iced_x86._iced_x86_py")]
#[derive(Copy, Clone)]
pub(crate) struct ConstantOffsets {
	pub(crate) offsets: iced_x86::ConstantOffsets,
}

#[pymethods]
impl ConstantOffsets {
	/// int: (``u32``) The offset of the displacement, if any
	#[getter]
	fn displacement_offset(&self) -> u32 {
		self.offsets.displacement_offset() as u32
	}

	/// int: (``u32``) Size in bytes of the displacement, or 0 if there's no displacement
	#[getter]
	fn displacement_size(&self) -> u32 {
		self.offsets.displacement_size() as u32
	}

	/// int: (``u32``) The offset of the first immediate, if any.
	///
	/// This field can be invalid even if the operand has an immediate if it's an immediate that isn't part
	/// of the instruction stream, eg. ``SHL AL,1``.
	#[getter]
	fn immediate_offset(&self) -> u32 {
		self.offsets.immediate_offset() as u32
	}

	/// int: (``u32``) Size in bytes of the first immediate, or 0 if there's no immediate
	#[getter]
	fn immediate_size(&self) -> u32 {
		self.offsets.immediate_size() as u32
	}

	/// int: (``u32``) The offset of the second immediate, if any.
	#[getter]
	fn immediate_offset2(&self) -> u32 {
		self.offsets.immediate_offset2() as u32
	}

	/// int: (``u32``) Size in bytes of the second immediate, or 0 if there's no second immediate
	#[getter]
	fn immediate_size2(&self) -> u32 {
		self.offsets.immediate_size2() as u32
	}

	/// bool: ``True`` if :class:`ConstantOffsets.displacement_offset` and :class:`ConstantOffsets.displacement_size` are valid
	#[getter]
	fn has_displacement(&self) -> bool {
		self.offsets.has_displacement()
	}

	/// bool: ``True`` if :class:`ConstantOffsets.immediate_offset` and :class:`ConstantOffsets.immediate_size` are valid
	#[getter]
	fn has_immediate(&self) -> bool {
		self.offsets.has_immediate()
	}

	/// bool: ``True`` if :class:`ConstantOffsets.immediate_offset2` and :class:`ConstantOffsets.immediate_size2` are valid
	#[getter]
	fn has_immediate2(&self) -> bool {
		self.offsets.has_immediate2()
	}

	/// Returns a copy of this instance.
	///
	/// Returns:
	///     ConstantOffsets: A copy of this instance
	///
	/// This is identical to :class:`ConstantOffsets.copy`
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
	///     ConstantOffsets: A copy of this instance
	///
	/// This is identical to :class:`ConstantOffsets.copy`
	#[pyo3(text_signature = "($self, memo)")]
	fn __deepcopy__(&self, _memo: &Bound<'_, PyAny>) -> Self {
		*self
	}

	/// Returns a copy of this instance.
	///
	/// Returns:
	///     ConstantOffsets: A copy of this instance
	#[pyo3(text_signature = "($self)")]
	fn copy(&self) -> Self {
		*self
	}

	fn __richcmp__(&self, other: PyRef<'_, ConstantOffsets>, op: CompareOp) -> PyObject {
		match op {
			CompareOp::Eq => (self.offsets == other.offsets).into_py(other.py()),
			CompareOp::Ne => (self.offsets != other.offsets).into_py(other.py()),
			_ => other.py().NotImplemented(),
		}
	}

	fn __hash__(&self) -> u64 {
		let mut hasher = DefaultHasher::new();
		self.offsets.hash(&mut hasher);
		hasher.finish()
	}
}
