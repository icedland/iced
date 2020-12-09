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

use crate::enum_utils::to_memory_size;
use pyo3::prelude::*;

/// :class:`MemorySize` enum info, see also :class:`MemorySizeExt`
///
/// Args:
///     `memory_size` (:class:`MemorySize`): Enum value
///
/// Examples:
///
/// .. testcode::
///
///     from iced_x86 import *
///
///     info = MemorySizeInfo(MemorySize.PACKED256_UINT16);
///     assert info.size == 32
#[pyclass(module = "_iced_x86_py")]
#[text_signature = "(memory_size, /)"]
pub(crate) struct MemorySizeInfo {
	pub(crate) info: &'static iced_x86::MemorySizeInfo,
}

#[pymethods]
impl MemorySizeInfo {
	#[new]
	fn new(memory_size: u32) -> PyResult<Self> {
		Ok(MemorySizeInfo { info: to_memory_size(memory_size)?.info() })
	}

	/// :class:`MemorySize`: Gets the :class:`MemorySize` value
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     info = MemorySizeInfo(MemorySize.PACKED256_UINT16);
	///     assert info.memory_size == MemorySize.PACKED256_UINT16
	#[getter]
	fn memory_size(&self) -> u32 {
		self.info.memory_size() as u32
	}

	/// int: (``u32``) Gets the size in bytes of the memory location or 0 if it's not accessed or unknown
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     info = MemorySizeInfo(MemorySize.UINT32);
	///     assert info.size == 4
	///     info = MemorySizeInfo(MemorySize.PACKED256_UINT16);
	///     assert info.size == 32
	///     info = MemorySizeInfo(MemorySize.BROADCAST512_UINT64);
	///     assert info.size == 8
	#[getter]
	fn size(&self) -> u32 {
		self.info.size() as u32
	}

	/// int: (``u32``) Gets the size in bytes of the packed element. If it's not a packed data type, it's equal to :class:`MemorySizeInfo.size`.
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     info = MemorySizeInfo(MemorySize.UINT32);
	///     assert info.element_size == 4
	///     info = MemorySizeInfo(MemorySize.PACKED256_UINT16);
	///     assert info.element_size == 2
	///     info = MemorySizeInfo(MemorySize.BROADCAST512_UINT64);
	///     assert info.element_size == 8
	#[getter]
	fn element_size(&self) -> u32 {
		self.info.element_size() as u32
	}

	/// :class:`MemorySize`: Gets the element type if it's packed data or the type itself if it's not packed data
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     info = MemorySizeInfo(MemorySize.UINT32);
	///     assert info.element_type == MemorySize.UINT32
	///     info = MemorySizeInfo(MemorySize.PACKED256_UINT16);
	///     assert info.element_type == MemorySize.UINT16
	///     info = MemorySizeInfo(MemorySize.BROADCAST512_UINT64);
	///     assert info.element_type == MemorySize.UINT64
	#[getter]
	fn element_type(&self) -> u32 {
		self.info.element_type() as u32
	}

	/// :class:`MemorySizeInfo`: Gets the element type if it's packed data or the type itself if it's not packed data
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     info = MemorySizeInfo(MemorySize.UINT32).element_type_info;
	///     assert info.memory_size == MemorySize.UINT32
	///     info = MemorySizeInfo(MemorySize.PACKED256_UINT16).element_type_info;
	///     assert info.memory_size == MemorySize.UINT16
	///     info = MemorySizeInfo(MemorySize.BROADCAST512_UINT64).element_type_info;
	///     assert info.memory_size == MemorySize.UINT64
	#[getter]
	fn element_type_info(&self) -> Self {
		Self { info: self.info.element_type().info() }
	}

	/// bool: ``True`` if it's signed data (signed integer or a floating point value)
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     info = MemorySizeInfo(MemorySize.UINT32);
	///     assert not info.is_signed
	///     info = MemorySizeInfo(MemorySize.INT32);
	///     assert info.is_signed
	///     info = MemorySizeInfo(MemorySize.FLOAT64);
	///     assert info.is_signed
	#[getter]
	fn is_signed(&self) -> bool {
		self.info.is_signed()
	}

	/// bool: ``True`` if it's a broadcast memory type
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     info = MemorySizeInfo(MemorySize.UINT32);
	///     assert not info.is_broadcast
	///     info = MemorySizeInfo(MemorySize.PACKED256_UINT16);
	///     assert not info.is_broadcast
	///     info = MemorySizeInfo(MemorySize.BROADCAST512_UINT64);
	///     assert info.is_broadcast
	#[getter]
	fn is_broadcast(&self) -> bool {
		self.info.is_broadcast()
	}

	/// bool: ``True`` if this is a packed data type, eg. :class:`MemorySize.PACKED128_FLOAT32`. See also :class:`MemorySizeInfo.element_count`
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     info = MemorySizeInfo(MemorySize.UINT32);
	///     assert not info.is_packed
	///     info = MemorySizeInfo(MemorySize.PACKED256_UINT16);
	///     assert info.is_packed
	///     info = MemorySizeInfo(MemorySize.BROADCAST512_UINT64);
	///     assert not info.is_packed
	#[getter]
	fn is_packed(&self) -> bool {
		self.info.is_packed()
	}

	/// int: (``u32``) Gets the number of elements in the packed data type or `1` if it's not packed data (:class:`MemorySizeInfo.is_packed`)
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     info = MemorySizeInfo(MemorySize.UINT32);
	///     assert info.element_count == 1
	///     info = MemorySizeInfo(MemorySize.PACKED256_UINT16);
	///     assert info.element_count == 16
	///     info = MemorySizeInfo(MemorySize.BROADCAST512_UINT64);
	///     assert info.element_count == 1
	#[getter]
	fn element_count(&self) -> u32 {
		self.info.element_count() as u32
	}
}
