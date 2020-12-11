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

use super::iced_constants::IcedConstants;
use pyo3::exceptions::PyValueError;
use pyo3::prelude::*;

pub(super) fn to_register(value: u32) -> PyResult<iced_x86::Register> {
	if value >= IcedConstants::REGISTER_ENUM_COUNT as u32 {
		Err(PyValueError::new_err("Invalid Register value"))
	} else {
		Ok(unsafe { core::mem::transmute(value as u8) })
	}
}

pub(super) fn to_rounding_control(value: u32) -> PyResult<iced_x86::RoundingControl> {
	if value >= IcedConstants::ROUNDING_CONTROL_ENUM_COUNT as u32 {
		Err(PyValueError::new_err("Invalid RoundingControl value"))
	} else {
		Ok(unsafe { core::mem::transmute(value as u8) })
	}
}

pub(super) fn to_code_size(value: u32) -> PyResult<iced_x86::CodeSize> {
	if value >= IcedConstants::CODE_SIZE_ENUM_COUNT as u32 {
		Err(PyValueError::new_err("Invalid CodeSize value"))
	} else {
		Ok(unsafe { core::mem::transmute(value as u8) })
	}
}

pub(super) fn to_code(value: u32) -> PyResult<iced_x86::Code> {
	if value >= IcedConstants::CODE_ENUM_COUNT as u32 {
		Err(PyValueError::new_err("Invalid Code value"))
	} else {
		Ok(unsafe { core::mem::transmute(value as u16) })
	}
}

pub(super) fn to_op_kind(value: u32) -> PyResult<iced_x86::OpKind> {
	if value >= IcedConstants::OP_KIND_ENUM_COUNT as u32 {
		Err(PyValueError::new_err("Invalid OpKind value"))
	} else {
		Ok(unsafe { core::mem::transmute(value as u8) })
	}
}

pub(super) fn to_memory_size_options(value: u32) -> PyResult<iced_x86::MemorySizeOptions> {
	if value >= IcedConstants::MEMORY_SIZE_OPTIONS_ENUM_COUNT as u32 {
		Err(PyValueError::new_err("Invalid MemorySizeOptions value"))
	} else {
		Ok(unsafe { core::mem::transmute(value as u8) })
	}
}

pub(super) fn to_cc_b(value: u32) -> PyResult<iced_x86::CC_b> {
	if value >= IcedConstants::CC_B_ENUM_COUNT as u32 {
		Err(PyValueError::new_err("Invalid CC_b value"))
	} else {
		Ok(unsafe { core::mem::transmute(value as u8) })
	}
}

pub(super) fn to_cc_ae(value: u32) -> PyResult<iced_x86::CC_ae> {
	if value >= IcedConstants::CC_AE_ENUM_COUNT as u32 {
		Err(PyValueError::new_err("Invalid CC_ae value"))
	} else {
		Ok(unsafe { core::mem::transmute(value as u8) })
	}
}

pub(super) fn to_cc_e(value: u32) -> PyResult<iced_x86::CC_e> {
	if value >= IcedConstants::CC_E_ENUM_COUNT as u32 {
		Err(PyValueError::new_err("Invalid CC_e value"))
	} else {
		Ok(unsafe { core::mem::transmute(value as u8) })
	}
}

pub(super) fn to_cc_ne(value: u32) -> PyResult<iced_x86::CC_ne> {
	if value >= IcedConstants::CC_NE_ENUM_COUNT as u32 {
		Err(PyValueError::new_err("Invalid CC_ne value"))
	} else {
		Ok(unsafe { core::mem::transmute(value as u8) })
	}
}

pub(super) fn to_cc_be(value: u32) -> PyResult<iced_x86::CC_be> {
	if value >= IcedConstants::CC_BE_ENUM_COUNT as u32 {
		Err(PyValueError::new_err("Invalid CC_be value"))
	} else {
		Ok(unsafe { core::mem::transmute(value as u8) })
	}
}

pub(super) fn to_cc_a(value: u32) -> PyResult<iced_x86::CC_a> {
	if value >= IcedConstants::CC_A_ENUM_COUNT as u32 {
		Err(PyValueError::new_err("Invalid CC_a value"))
	} else {
		Ok(unsafe { core::mem::transmute(value as u8) })
	}
}

pub(super) fn to_cc_p(value: u32) -> PyResult<iced_x86::CC_p> {
	if value >= IcedConstants::CC_P_ENUM_COUNT as u32 {
		Err(PyValueError::new_err("Invalid CC_p value"))
	} else {
		Ok(unsafe { core::mem::transmute(value as u8) })
	}
}

pub(super) fn to_cc_np(value: u32) -> PyResult<iced_x86::CC_np> {
	if value >= IcedConstants::CC_NP_ENUM_COUNT as u32 {
		Err(PyValueError::new_err("Invalid CC_np value"))
	} else {
		Ok(unsafe { core::mem::transmute(value as u8) })
	}
}

pub(super) fn to_cc_l(value: u32) -> PyResult<iced_x86::CC_l> {
	if value >= IcedConstants::CC_L_ENUM_COUNT as u32 {
		Err(PyValueError::new_err("Invalid CC_l value"))
	} else {
		Ok(unsafe { core::mem::transmute(value as u8) })
	}
}

pub(super) fn to_cc_ge(value: u32) -> PyResult<iced_x86::CC_ge> {
	if value >= IcedConstants::CC_GE_ENUM_COUNT as u32 {
		Err(PyValueError::new_err("Invalid CC_ge value"))
	} else {
		Ok(unsafe { core::mem::transmute(value as u8) })
	}
}

pub(super) fn to_cc_le(value: u32) -> PyResult<iced_x86::CC_le> {
	if value >= IcedConstants::CC_LE_ENUM_COUNT as u32 {
		Err(PyValueError::new_err("Invalid CC_le value"))
	} else {
		Ok(unsafe { core::mem::transmute(value as u8) })
	}
}

pub(super) fn to_cc_g(value: u32) -> PyResult<iced_x86::CC_g> {
	if value >= IcedConstants::CC_G_ENUM_COUNT as u32 {
		Err(PyValueError::new_err("Invalid CC_g value"))
	} else {
		Ok(unsafe { core::mem::transmute(value as u8) })
	}
}

pub(super) fn to_memory_size(value: u32) -> PyResult<iced_x86::MemorySize> {
	if value >= IcedConstants::MEMORY_SIZE_ENUM_COUNT as u32 {
		Err(PyValueError::new_err("Invalid MemorySize value"))
	} else {
		Ok(unsafe { core::mem::transmute(value as u8) })
	}
}

pub(super) fn to_rep_prefix_kind(value: u32) -> PyResult<iced_x86::RepPrefixKind> {
	if value >= IcedConstants::REP_PREFIX_KIND_ENUM_COUNT as u32 {
		Err(PyValueError::new_err("Invalid RepPrefixKind value"))
	} else {
		Ok(unsafe { core::mem::transmute(value as u8) })
	}
}
