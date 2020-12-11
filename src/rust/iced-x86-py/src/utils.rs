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

use pyo3::exceptions::{PyTypeError, PyValueError};
use pyo3::prelude::*;
use pyo3::types::{PyByteArray, PyBytes};

/// Gets a ref to the bytes or an error. It assumes the input data is not modified
/// if it's mutable (eg. if it's a `bytearray`)
pub(crate) unsafe fn get_temporary_byte_array_ref<'a>(data: &'a PyAny) -> PyResult<&'a [u8]> {
	if let Ok(bytes) = <PyBytes as PyTryFrom>::try_from(data) {
		Ok(bytes.as_bytes())
	} else if let Ok(bytearray) = <PyByteArray as PyTryFrom>::try_from(data) {
		Ok(bytearray.as_bytes())
	} else {
		//TODO: support memoryview (also update docs and Decoder ctor and the message below)
		Err(PyTypeError::new_err("Expected one of these types: bytes, bytearray"))
	}
}

#[inline(never)]
pub(crate) fn to_value_error(error: iced_x86::IcedError) -> PyErr {
	PyValueError::new_err(format!("{}", error))
}
