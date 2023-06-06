// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::enum_utils::to_register;
use crate::register_info::RegisterInfo;
use pyo3::prelude::*;

/// :class:`Register` enum extension methods, see also :class:`RegisterInfo`
#[pyclass(module = "iced_x86._iced_x86_py")]
pub(crate) struct RegisterExt {}

#[pymethods]
impl RegisterExt {
	/// Gets register info
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     :class:`RegisterInfo`: Register info
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     info = RegisterExt.info(Register.EAX)
	///     assert info.size == 4
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn info(register: u32) -> PyResult<RegisterInfo> {
		Ok(RegisterInfo { info: to_register(register)?.info() })
	}

	/// Gets the base register, eg. ``AL``, ``AX``, ``EAX``, ``RAX``, ``MM0``, ``XMM0``, ``YMM0``, ``ZMM0``, ``ES``
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     :class:`Register`: Base register
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert RegisterExt.base(Register.GS) == Register.ES
	///     assert RegisterExt.base(Register.SIL) == Register.AL
	///     assert RegisterExt.base(Register.SP) == Register.AX
	///     assert RegisterExt.base(Register.R13D) == Register.EAX
	///     assert RegisterExt.base(Register.RBP) == Register.RAX
	///     assert RegisterExt.base(Register.MM6) == Register.MM0
	///     assert RegisterExt.base(Register.XMM28) == Register.XMM0
	///     assert RegisterExt.base(Register.YMM12) == Register.YMM0
	///     assert RegisterExt.base(Register.ZMM31) == Register.ZMM0
	///     assert RegisterExt.base(Register.K3) == Register.K0
	///     assert RegisterExt.base(Register.BND1) == Register.BND0
	///     assert RegisterExt.base(Register.ST7) == Register.ST0
	///     assert RegisterExt.base(Register.CR8) == Register.CR0
	///     assert RegisterExt.base(Register.DR6) == Register.DR0
	///     assert RegisterExt.base(Register.TR3) == Register.TR0
	///     assert RegisterExt.base(Register.RIP) == Register.EIP
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn base(register: u32) -> PyResult<u32> {
		Ok(to_register(register)?.base() as u32)
	}

	/// The register number (index) relative to :class:`RegisterExt.base`, eg. 0-15, or 0-31, or if 8-bit GPR, 0-19
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     int: Register number (index) relative to the base register
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert RegisterExt.number(Register.GS) == 5
	///     assert RegisterExt.number(Register.SIL) == 10
	///     assert RegisterExt.number(Register.SP) == 4
	///     assert RegisterExt.number(Register.R13D) == 13
	///     assert RegisterExt.number(Register.RBP) == 5
	///     assert RegisterExt.number(Register.MM6) == 6
	///     assert RegisterExt.number(Register.XMM28) == 28
	///     assert RegisterExt.number(Register.YMM12) == 12
	///     assert RegisterExt.number(Register.ZMM31) == 31
	///     assert RegisterExt.number(Register.K3) == 3
	///     assert RegisterExt.number(Register.BND1) == 1
	///     assert RegisterExt.number(Register.ST7) == 7
	///     assert RegisterExt.number(Register.CR8) == 8
	///     assert RegisterExt.number(Register.DR6) == 6
	///     assert RegisterExt.number(Register.TR3) == 3
	///     assert RegisterExt.number(Register.RIP) == 1
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn number(register: u32) -> PyResult<u32> {
		Ok(to_register(register)?.number() as u32)
	}

	/// Gets the full register that this one is a part of, eg. ``CL``/``CH``/``CX``/``ECX``/``RCX`` -> ``RCX``, ``XMM11``/``YMM11``/``ZMM11`` -> ``ZMM11``
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     :class:`Register`: Full register (64-bit GPRs)
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert RegisterExt.full_register(Register.GS) == Register.GS
	///     assert RegisterExt.full_register(Register.SIL) == Register.RSI
	///     assert RegisterExt.full_register(Register.SP) == Register.RSP
	///     assert RegisterExt.full_register(Register.R13D) == Register.R13
	///     assert RegisterExt.full_register(Register.RBP) == Register.RBP
	///     assert RegisterExt.full_register(Register.MM6) == Register.MM6
	///     assert RegisterExt.full_register(Register.XMM10) == Register.ZMM10
	///     assert RegisterExt.full_register(Register.YMM10) == Register.ZMM10
	///     assert RegisterExt.full_register(Register.ZMM10) == Register.ZMM10
	///     assert RegisterExt.full_register(Register.K3) == Register.K3
	///     assert RegisterExt.full_register(Register.BND1) == Register.BND1
	///     assert RegisterExt.full_register(Register.ST7) == Register.ST7
	///     assert RegisterExt.full_register(Register.CR8) == Register.CR8
	///     assert RegisterExt.full_register(Register.DR6) == Register.DR6
	///     assert RegisterExt.full_register(Register.TR3) == Register.TR3
	///     assert RegisterExt.full_register(Register.RIP) == Register.RIP
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn full_register(register: u32) -> PyResult<u32> {
		Ok(to_register(register)?.full_register() as u32)
	}

	/// Gets the full register that this one is a part of, except if it's a GPR in which case the 32-bit register is returned,
	/// eg. ``CL``/``CH``/``CX``/``ECX``/``RCX`` -> ``ECX``, ``XMM11``/``YMM11``/``ZMM11`` -> ``ZMM11``
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     :class:`Register`: Full register (32-bit GPRs)
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert RegisterExt.full_register32(Register.GS) == Register.GS
	///     assert RegisterExt.full_register32(Register.SIL) == Register.ESI
	///     assert RegisterExt.full_register32(Register.SP) == Register.ESP
	///     assert RegisterExt.full_register32(Register.R13D) == Register.R13D
	///     assert RegisterExt.full_register32(Register.RBP) == Register.EBP
	///     assert RegisterExt.full_register32(Register.MM6) == Register.MM6
	///     assert RegisterExt.full_register32(Register.XMM10) == Register.ZMM10
	///     assert RegisterExt.full_register32(Register.YMM10) == Register.ZMM10
	///     assert RegisterExt.full_register32(Register.ZMM10) == Register.ZMM10
	///     assert RegisterExt.full_register32(Register.K3) == Register.K3
	///     assert RegisterExt.full_register32(Register.BND1) == Register.BND1
	///     assert RegisterExt.full_register32(Register.ST7) == Register.ST7
	///     assert RegisterExt.full_register32(Register.CR8) == Register.CR8
	///     assert RegisterExt.full_register32(Register.DR6) == Register.DR6
	///     assert RegisterExt.full_register32(Register.TR3) == Register.TR3
	///     assert RegisterExt.full_register32(Register.RIP) == Register.RIP
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn full_register32(register: u32) -> PyResult<u32> {
		Ok(to_register(register)?.full_register32() as u32)
	}

	/// Gets the size of the register in bytes
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     int: Size of the register in bytes
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert RegisterExt.size(Register.GS) == 2
	///     assert RegisterExt.size(Register.SIL) == 1
	///     assert RegisterExt.size(Register.SP) == 2
	///     assert RegisterExt.size(Register.R13D) == 4
	///     assert RegisterExt.size(Register.RBP) == 8
	///     assert RegisterExt.size(Register.MM6) == 8
	///     assert RegisterExt.size(Register.XMM10) == 16
	///     assert RegisterExt.size(Register.YMM10) == 32
	///     assert RegisterExt.size(Register.ZMM10) == 64
	///     assert RegisterExt.size(Register.K3) == 8
	///     assert RegisterExt.size(Register.BND1) == 16
	///     assert RegisterExt.size(Register.ST7) == 10
	///     assert RegisterExt.size(Register.CR8) == 8
	///     assert RegisterExt.size(Register.DR6) == 8
	///     assert RegisterExt.size(Register.TR3) == 4
	///     assert RegisterExt.size(Register.RIP) == 8
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn size(register: u32) -> PyResult<u32> {
		Ok(to_register(register)?.size() as u32)
	}

	/// Checks if it's a segment register (``ES``, ``CS``, ``SS``, ``DS``, ``FS``, ``GS``)
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if it's a segment register
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert RegisterExt.is_segment_register(Register.GS)
	///     assert not RegisterExt.is_segment_register(Register.RCX)
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn is_segment_register(register: u32) -> PyResult<bool> {
		Ok(to_register(register)?.is_segment_register())
	}

	/// Checks if it's a general purpose register (``AL``-``R15L``, ``AX``-``R15W``, ``EAX``-``R15D``, ``RAX``-``R15``)
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if it's a general purpose register
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert not RegisterExt.is_gpr(Register.GS)
	///     assert RegisterExt.is_gpr(Register.CH)
	///     assert RegisterExt.is_gpr(Register.DX)
	///     assert RegisterExt.is_gpr(Register.R13D)
	///     assert RegisterExt.is_gpr(Register.RSP)
	///     assert not RegisterExt.is_gpr(Register.XMM0)
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn is_gpr(register: u32) -> PyResult<bool> {
		Ok(to_register(register)?.is_gpr())
	}

	/// Checks if it's an 8-bit general purpose register (``AL``-``R15L``)
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if it's an 8-bit general purpose register
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert not RegisterExt.is_gpr8(Register.GS)
	///     assert RegisterExt.is_gpr8(Register.CH)
	///     assert not RegisterExt.is_gpr8(Register.DX)
	///     assert not RegisterExt.is_gpr8(Register.R13D)
	///     assert not RegisterExt.is_gpr8(Register.RSP)
	///     assert not RegisterExt.is_gpr8(Register.XMM0)
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn is_gpr8(register: u32) -> PyResult<bool> {
		Ok(to_register(register)?.is_gpr8())
	}

	/// Checks if it's a 16-bit general purpose register (``AX``-``R15W``)
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if it's a 16-bit general purpose register
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert not RegisterExt.is_gpr16(Register.GS)
	///     assert not RegisterExt.is_gpr16(Register.CH)
	///     assert RegisterExt.is_gpr16(Register.DX)
	///     assert not RegisterExt.is_gpr16(Register.R13D)
	///     assert not RegisterExt.is_gpr16(Register.RSP)
	///     assert not RegisterExt.is_gpr16(Register.XMM0)
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn is_gpr16(register: u32) -> PyResult<bool> {
		Ok(to_register(register)?.is_gpr16())
	}

	/// Checks if it's a 32-bit general purpose register (``EAX``-``R15D``)
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if it's a 32-bit general purpose register
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert not RegisterExt.is_gpr32(Register.GS)
	///     assert not RegisterExt.is_gpr32(Register.CH)
	///     assert not RegisterExt.is_gpr32(Register.DX)
	///     assert RegisterExt.is_gpr32(Register.R13D)
	///     assert not RegisterExt.is_gpr32(Register.RSP)
	///     assert not RegisterExt.is_gpr32(Register.XMM0)
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn is_gpr32(register: u32) -> PyResult<bool> {
		Ok(to_register(register)?.is_gpr32())
	}

	/// Checks if it's a 64-bit general purpose register (``RAX``-``R15``)
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if it's a 64-bit general purpose register
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert not RegisterExt.is_gpr64(Register.GS)
	///     assert not RegisterExt.is_gpr64(Register.CH)
	///     assert not RegisterExt.is_gpr64(Register.DX)
	///     assert not RegisterExt.is_gpr64(Register.R13D)
	///     assert RegisterExt.is_gpr64(Register.RSP)
	///     assert not RegisterExt.is_gpr64(Register.XMM0)
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn is_gpr64(register: u32) -> PyResult<bool> {
		Ok(to_register(register)?.is_gpr64())
	}

	/// Checks if it's a 128-bit vector register (``XMM0``-``XMM31``)
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if it's an XMM register
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert not RegisterExt.is_xmm(Register.R13D)
	///     assert not RegisterExt.is_xmm(Register.RSP)
	///     assert RegisterExt.is_xmm(Register.XMM0)
	///     assert not RegisterExt.is_xmm(Register.YMM0)
	///     assert not RegisterExt.is_xmm(Register.ZMM0)
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn is_xmm(register: u32) -> PyResult<bool> {
		Ok(to_register(register)?.is_xmm())
	}

	/// Checks if it's a 256-bit vector register (``YMM0``-``YMM31``)
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if it's a YMM register
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert not RegisterExt.is_ymm(Register.R13D)
	///     assert not RegisterExt.is_ymm(Register.RSP)
	///     assert not RegisterExt.is_ymm(Register.XMM0)
	///     assert RegisterExt.is_ymm(Register.YMM0)
	///     assert not RegisterExt.is_ymm(Register.ZMM0)
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn is_ymm(register: u32) -> PyResult<bool> {
		Ok(to_register(register)?.is_ymm())
	}

	/// Checks if it's a 512-bit vector register (``ZMM0``-``ZMM31``)
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if it's a ZMM register
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert not RegisterExt.is_zmm(Register.R13D)
	///     assert not RegisterExt.is_zmm(Register.RSP)
	///     assert not RegisterExt.is_zmm(Register.XMM0)
	///     assert not RegisterExt.is_zmm(Register.YMM0)
	///     assert RegisterExt.is_zmm(Register.ZMM0)
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn is_zmm(register: u32) -> PyResult<bool> {
		Ok(to_register(register)?.is_zmm())
	}

	/// Checks if it's an ``XMM``, ``YMM`` or ``ZMM`` register
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if it's a vector register
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert not RegisterExt.is_vector_register(Register.R13D)
	///     assert not RegisterExt.is_vector_register(Register.RSP)
	///     assert RegisterExt.is_vector_register(Register.XMM0)
	///     assert RegisterExt.is_vector_register(Register.YMM0)
	///     assert RegisterExt.is_vector_register(Register.ZMM0)
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn is_vector_register(register: u32) -> PyResult<bool> {
		Ok(to_register(register)?.is_vector_register())
	}

	/// Checks if it's ``EIP``/``RIP``
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if it's ``EIP``/``RIP``
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert RegisterExt.is_ip(Register.EIP)
	///     assert RegisterExt.is_ip(Register.RIP)
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn is_ip(register: u32) -> PyResult<bool> {
		Ok(to_register(register)?.is_ip())
	}

	/// Checks if it's an opmask register (``K0``-``K7``)
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if it's an opmask register
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert not RegisterExt.is_k(Register.R13D)
	///     assert RegisterExt.is_k(Register.K3)
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn is_k(register: u32) -> PyResult<bool> {
		Ok(to_register(register)?.is_k())
	}

	/// Checks if it's a control register (``CR0``-``CR15``)
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if it's a control register
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert not RegisterExt.is_cr(Register.R13D)
	///     assert RegisterExt.is_cr(Register.CR3)
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn is_cr(register: u32) -> PyResult<bool> {
		Ok(to_register(register)?.is_cr())
	}

	/// Checks if it's a debug register (``DR0``-``DR15``)
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if it's a debug register
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert not RegisterExt.is_dr(Register.R13D)
	///     assert RegisterExt.is_dr(Register.DR3)
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn is_dr(register: u32) -> PyResult<bool> {
		Ok(to_register(register)?.is_dr())
	}

	/// Checks if it's a test register (``TR0``-``TR7``)
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if it's a test register
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert not RegisterExt.is_tr(Register.R13D)
	///     assert RegisterExt.is_tr(Register.TR3)
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn is_tr(register: u32) -> PyResult<bool> {
		Ok(to_register(register)?.is_tr())
	}

	/// Checks if it's an FPU stack register (``ST0``-``ST7``)
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if it's an FPU register
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert not RegisterExt.is_st(Register.R13D)
	///     assert RegisterExt.is_st(Register.ST3)
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn is_st(register: u32) -> PyResult<bool> {
		Ok(to_register(register)?.is_st())
	}

	/// Checks if it's a bound register (``BND0``-``BND3``)
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if it's a bnd register
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert not RegisterExt.is_bnd(Register.R13D)
	///     assert RegisterExt.is_bnd(Register.BND3)
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn is_bnd(register: u32) -> PyResult<bool> {
		Ok(to_register(register)?.is_bnd())
	}

	/// Checks if it's an MMX register (``MM0``-``MM7``)
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if it's an mmx register
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert not RegisterExt.is_mm(Register.R13D)
	///     assert RegisterExt.is_mm(Register.MM3)
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn is_mm(register: u32) -> PyResult<bool> {
		Ok(to_register(register)?.is_mm())
	}

	/// Checks if it's a tile register (``TMM0``-``TMM7``)
	///
	/// Args:
	///     `register` (:class:`Register`): Enum value
	///
	/// Returns:
	///     bool: ``True`` if it's a tmm register
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert not RegisterExt.is_tmm(Register.R13D)
	///     assert RegisterExt.is_tmm(Register.TMM3)
	#[staticmethod]
	#[pyo3(text_signature = "(register)")]
	fn is_tmm(register: u32) -> PyResult<bool> {
		Ok(to_register(register)?.is_tmm())
	}
}
