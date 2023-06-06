// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::enum_utils::to_register;
use pyo3::prelude::*;

/// :class:`Register` enum info, see also :class:`RegisterExt`
///
/// Args:
///     `register` (:class:`Register`): Enum value
///
/// Examples:
///
/// .. testcode::
///
///     from iced_x86 import *
///
///     info = RegisterInfo(Register.GS)
///     assert info.number == 5
#[pyclass(module = "iced_x86._iced_x86_py")]
pub(crate) struct RegisterInfo {
	pub(crate) info: &'static iced_x86::RegisterInfo,
}

#[pymethods]
impl RegisterInfo {
	#[new]
	#[pyo3(text_signature = "(register)")]
	fn new(register: u32) -> PyResult<Self> {
		Ok(RegisterInfo { info: to_register(register)?.info() })
	}

	/// :class:`Register`: Gets the register value passed into the constructor
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     info = RegisterInfo(Register.EAX)
	///     assert info.register == Register.EAX
	#[getter]
	fn register(&self) -> u32 {
		self.info.register() as u32
	}

	/// :class:`Register`: Gets the base register, eg. ``AL``, ``AX``, ``EAX``, ``RAX``, ``MM0``, ``XMM0``, ``YMM0``, ``ZMM0``, ``ES``
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     info = RegisterInfo(Register.GS)
	///     assert info.base == Register.ES
	///     info = RegisterInfo(Register.RDX)
	///     assert info.base == Register.RAX
	///     info = RegisterInfo(Register.XMM13)
	///     assert info.base == Register.XMM0
	///     info = RegisterInfo(Register.YMM13)
	///     assert info.base == Register.YMM0
	///     info = RegisterInfo(Register.ZMM13)
	///     assert info.base == Register.ZMM0
	#[getter]
	fn base(&self) -> u32 {
		self.info.base() as u32
	}

	/// int: The register number (index) relative to :class:`RegisterInfo.base`, eg. 0-15, or 0-31, or if 8-bit GPR, 0-19
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     info = RegisterInfo(Register.GS)
	///     assert info.number == 5
	///     info = RegisterInfo(Register.RDX)
	///     assert info.number == 2
	///     info = RegisterInfo(Register.XMM13)
	///     assert info.number == 13
	///     info = RegisterInfo(Register.YMM13)
	///     assert info.number == 13
	///     info = RegisterInfo(Register.ZMM13)
	///     assert info.number == 13
	#[getter]
	fn number(&self) -> u32 {
		self.info.number() as u32
	}

	/// :class:`Register`: The full register that this one is a part of, eg. ``CL``/``CH``/``CX``/``ECX``/``RCX`` -> ``RCX``, ``XMM11``/``YMM11``/``ZMM11`` -> ``ZMM11``
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     info = RegisterInfo(Register.GS)
	///     assert info.full_register == Register.GS
	///     info = RegisterInfo(Register.BH)
	///     assert info.full_register == Register.RBX
	///     info = RegisterInfo(Register.DX)
	///     assert info.full_register == Register.RDX
	///     info = RegisterInfo(Register.ESP)
	///     assert info.full_register == Register.RSP
	///     info = RegisterInfo(Register.RCX)
	///     assert info.full_register == Register.RCX
	///     info = RegisterInfo(Register.XMM3)
	///     assert info.full_register == Register.ZMM3
	///     info = RegisterInfo(Register.YMM3)
	///     assert info.full_register == Register.ZMM3
	///     info = RegisterInfo(Register.ZMM3)
	///     assert info.full_register == Register.ZMM3
	#[getter]
	fn full_register(&self) -> u32 {
		self.info.full_register() as u32
	}

	/// :class:`Register`: Gets the full register that this one is a part of, except if it's a GPR in which case the 32-bit register is returned,
	/// eg. ``CL``/``CH``/``CX``/``ECX``/``RCX`` -> ``ECX``, ``XMM11``/``YMM11``/``ZMM11`` -> ``ZMM11``
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     info = RegisterInfo(Register.GS)
	///     assert info.full_register32 == Register.GS
	///     info = RegisterInfo(Register.BH)
	///     assert info.full_register32 == Register.EBX
	///     info = RegisterInfo(Register.DX)
	///     assert info.full_register32 == Register.EDX
	///     info = RegisterInfo(Register.ESP)
	///     assert info.full_register32 == Register.ESP
	///     info = RegisterInfo(Register.RCX)
	///     assert info.full_register32 == Register.ECX
	///     info = RegisterInfo(Register.XMM3)
	///     assert info.full_register32 == Register.ZMM3
	///     info = RegisterInfo(Register.YMM3)
	///     assert info.full_register32 == Register.ZMM3
	///     info = RegisterInfo(Register.ZMM3)
	///     assert info.full_register32 == Register.ZMM3
	#[getter]
	fn full_register32(&self) -> u32 {
		self.info.full_register32() as u32
	}

	/// int: Size of the register in bytes
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     info = RegisterInfo(Register.GS)
	///     assert info.size == 2
	///     info = RegisterInfo(Register.BH)
	///     assert info.size == 1
	///     info = RegisterInfo(Register.DX)
	///     assert info.size == 2
	///     info = RegisterInfo(Register.ESP)
	///     assert info.size == 4
	///     info = RegisterInfo(Register.RCX)
	///     assert info.size == 8
	///     info = RegisterInfo(Register.XMM3)
	///     assert info.size == 16
	///     info = RegisterInfo(Register.YMM3)
	///     assert info.size == 32
	///     info = RegisterInfo(Register.ZMM3)
	///     assert info.size == 64
	#[getter]
	fn size(&self) -> u32 {
		self.info.size() as u32
	}
}
