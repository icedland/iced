// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use pyo3::exceptions::{PyTypeError, PyValueError};
use pyo3::prelude::*;
use pyo3::types::{PyByteArray, PyBytes};
use std::fmt::Display;

/// Gets a ref to the bytes or an error. It assumes the input data is not modified
/// if it's mutable (eg. if it's a `bytearray`)
pub(crate) unsafe fn get_temporary_byte_array_ref<'a>(data: &'a Bound<'_, PyAny>) -> PyResult<&'a [u8]> {
	if let Ok(bytes) = data.downcast::<PyBytes>() {
		Ok(bytes.as_bytes())
	} else if let Ok(bytearray) = data.downcast::<PyByteArray>() {
		Ok(bytearray.as_bytes())
	} else {
		//TODO: support memoryview (also update docs and Decoder ctor and the message below)
		Err(PyTypeError::new_err("Expected one of these types: bytes, bytearray"))
	}
}

#[inline(never)]
pub(crate) fn to_value_error<S: Display>(error: S) -> PyErr {
	PyValueError::new_err(format!("{}", error))
}
