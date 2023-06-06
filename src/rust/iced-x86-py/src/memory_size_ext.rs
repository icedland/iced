// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::enum_utils::to_memory_size;
use crate::memory_size_info::MemorySizeInfo;
use pyo3::prelude::*;

/// :class:`MemorySize` enum extension methods, see also :class:`MemorySizeInfo`
#[pyclass(module = "iced_x86._iced_x86_py")]
pub(crate) struct MemorySizeExt {}

#[pymethods]
impl MemorySizeExt {
	/// Gets the memory size info
	///
	/// Args:
	///     `memory_size` (:class:`MemorySize`): Enum value
	///
	/// Returns:
	///     :class:`MemorySizeInfo`: Memory size info
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     info = MemorySizeExt.info(MemorySize.PACKED256_UINT16)
	///     assert info.size == 32
	#[pyo3(text_signature = "(memory_size)")]
	#[staticmethod]
	fn info(memory_size: u32) -> PyResult<MemorySizeInfo> {
		Ok(MemorySizeInfo { info: to_memory_size(memory_size)?.info() })
	}

	/// Gets the size in bytes of the memory location or 0 if it's not accessed by the instruction or unknown or variable sized
	///
	/// Args:
	///     `memory_size` (:class:`MemorySize`): Enum value
	///
	/// Returns:
	///     int: (``u32``) Size in bytes of the memory location or
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert MemorySizeExt.size(MemorySize.UINT32) == 4
	///     assert MemorySizeExt.size(MemorySize.PACKED256_UINT16) == 32
	///     assert MemorySizeExt.size(MemorySize.BROADCAST512_UINT64) == 8
	#[pyo3(text_signature = "(memory_size)")]
	#[staticmethod]
	fn size(memory_size: u32) -> PyResult<u32> {
		Ok(to_memory_size(memory_size)?.size() as u32)
	}

	/// Gets the size in bytes of the packed element. If it's not a packed data type, it's equal to :class:`MemorySizeExt.size`.
	///
	/// Args:
	///     `memory_size` (:class:`MemorySize`): Enum value
	///
	/// Returns:
	///     int: (``u32``) Size in bytes of the packed element
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert MemorySizeExt.element_size(MemorySize.UINT32) == 4
	///     assert MemorySizeExt.element_size(MemorySize.PACKED256_UINT16) == 2
	///     assert MemorySizeExt.element_size(MemorySize.BROADCAST512_UINT64) == 8
	#[pyo3(text_signature = "(memory_size)")]
	#[staticmethod]
	fn element_size(memory_size: u32) -> PyResult<u32> {
		Ok(to_memory_size(memory_size)?.element_size() as u32)
	}

	/// Gets the element type if it's packed data or the input value if it's not packed data
	///
	/// Args:
	///     `memory_size` (:class:`MemorySize`): Enum value
	///
	/// Returns:
	///     :class:`MemorySize`: Element type
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert MemorySizeExt.element_type(MemorySize.UINT32) == MemorySize.UINT32
	///     assert MemorySizeExt.element_type(MemorySize.PACKED256_UINT16) == MemorySize.UINT16
	///     assert MemorySizeExt.element_type(MemorySize.BROADCAST512_UINT64) == MemorySize.UINT64
	#[pyo3(text_signature = "(memory_size)")]
	#[staticmethod]
	fn element_type(memory_size: u32) -> PyResult<u32> {
		Ok(to_memory_size(memory_size)?.element_type() as u32)
	}

	/// Gets the element type info if it's packed data or the input value if it's not packed data
	///
	/// Args:
	///     `memory_size` (:class:`MemorySize`): Enum value
	///
	/// Returns:
	///     :class:`MemorySizeInfo`: Element type info
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert MemorySizeExt.element_type_info(MemorySize.UINT32).memory_size == MemorySize.UINT32
	///     assert MemorySizeExt.element_type_info(MemorySize.PACKED256_UINT16).memory_size == MemorySize.UINT16
	///     assert MemorySizeExt.element_type_info(MemorySize.BROADCAST512_UINT64).memory_size == MemorySize.UINT64
	#[pyo3(text_signature = "(memory_size)")]
	#[staticmethod]
	fn element_type_info(memory_size: u32) -> PyResult<MemorySizeInfo> {
		Ok(MemorySizeInfo { info: to_memory_size(memory_size)?.element_type_info() })
	}

	/// ``True`` if it's signed data (signed integer or a floating point value)
	///
	/// Args:
	///     `memory_size` (:class:`MemorySize`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if it's signed data
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert not MemorySizeExt.is_signed(MemorySize.UINT32)
	///     assert MemorySizeExt.is_signed(MemorySize.INT32)
	///     assert MemorySizeExt.is_signed(MemorySize.FLOAT64)
	#[pyo3(text_signature = "(memory_size)")]
	#[staticmethod]
	fn is_signed(memory_size: u32) -> PyResult<bool> {
		Ok(to_memory_size(memory_size)?.is_signed())
	}

	/// ``True`` if this is a packed data type, eg. :class:`MemorySize.PACKED128_FLOAT32`
	///
	/// Args:
	///     `memory_size` (:class:`MemorySize`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if this is a packed data type
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert not MemorySizeExt.is_packed(MemorySize.UINT32)
	///     assert MemorySizeExt.is_packed(MemorySize.PACKED256_UINT16)
	///     assert not MemorySizeExt.is_packed(MemorySize.BROADCAST512_UINT64)
	#[pyo3(text_signature = "(memory_size)")]
	#[staticmethod]
	fn is_packed(memory_size: u32) -> PyResult<bool> {
		Ok(to_memory_size(memory_size)?.is_packed())
	}

	/// Gets the number of elements in the packed data type or ``1`` if it's not packed data (:class:`MemorySizeExt.is_packed`)
	///
	/// Args:
	///     `memory_size` (:class:`MemorySize`): Enum value
	///
	/// Returns:
	///     int: (``u32``) Number of elements in the packed data type
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert MemorySizeExt.element_count(MemorySize.UINT32) == 1
	///     assert MemorySizeExt.element_count(MemorySize.PACKED256_UINT16) == 16
	///     assert MemorySizeExt.element_count(MemorySize.BROADCAST512_UINT64) == 1
	#[pyo3(text_signature = "(memory_size)")]
	#[staticmethod]
	fn element_count(memory_size: u32) -> PyResult<u32> {
		Ok(to_memory_size(memory_size)?.element_count() as u32)
	}

	/// ``True`` if it is a broadcast memory type
	///
	/// Args:
	///     `memory_size` (:class:`MemorySize`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if it is a broadcast memory type
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert not MemorySizeExt.is_broadcast(MemorySize.PACKED64_FLOAT16)
	///     assert MemorySizeExt.is_broadcast(MemorySize.BROADCAST512_UINT64)
	#[pyo3(text_signature = "(memory_size)")]
	#[staticmethod]
	fn is_broadcast(memory_size: u32) -> PyResult<bool> {
		Ok(to_memory_size(memory_size)?.is_broadcast())
	}
}
