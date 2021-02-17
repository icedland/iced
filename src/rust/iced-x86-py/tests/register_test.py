# SPDX-License-Identifier: MIT
# Copyright (C) 2018-present iced project and contributors

import pytest
from iced_x86 import *

def test_register_ext():
	assert RegisterExt.base(Register.DL) == Register.AL
	assert RegisterExt.base(Register.R8W) == Register.AX
	assert RegisterExt.base(Register.R15D) == Register.EAX
	assert RegisterExt.base(Register.R13) == Register.RAX
	assert RegisterExt.base(Register.FS) == Register.ES
	assert RegisterExt.base(Register.XMM2) == Register.XMM0
	assert RegisterExt.base(Register.YMM20) == Register.YMM0
	assert RegisterExt.base(Register.ZMM31) == Register.ZMM0

	assert RegisterExt.number(Register.DL) == 2
	assert RegisterExt.number(Register.R15) == 15
	assert RegisterExt.number(Register.YMM21) == 21

	assert RegisterExt.full_register(Register.CL) == Register.RCX
	assert RegisterExt.full_register(Register.DX) == Register.RDX
	assert RegisterExt.full_register(Register.EBX) == Register.RBX
	assert RegisterExt.full_register(Register.RSP) == Register.RSP
	assert RegisterExt.full_register(Register.XMM2) == Register.ZMM2
	assert RegisterExt.full_register(Register.YMM22) == Register.ZMM22
	assert RegisterExt.full_register(Register.ZMM11) == Register.ZMM11

	assert RegisterExt.full_register32(Register.CL) == Register.ECX
	assert RegisterExt.full_register32(Register.DX) == Register.EDX
	assert RegisterExt.full_register32(Register.EBX) == Register.EBX
	assert RegisterExt.full_register32(Register.RSP) == Register.ESP
	assert RegisterExt.full_register32(Register.XMM2) == Register.ZMM2
	assert RegisterExt.full_register32(Register.YMM22) == Register.ZMM22
	assert RegisterExt.full_register32(Register.ZMM11) == Register.ZMM11

	assert RegisterExt.size(Register.DL) == 1
	assert RegisterExt.size(Register.R8W) == 2
	assert RegisterExt.size(Register.R15D) == 4
	assert RegisterExt.size(Register.R13) == 8
	assert RegisterExt.size(Register.FS) == 2
	assert RegisterExt.size(Register.XMM2) == 16
	assert RegisterExt.size(Register.YMM20) == 32
	assert RegisterExt.size(Register.ZMM31) == 64

	assert not RegisterExt.is_segment_register(Register.CX)
	assert RegisterExt.is_segment_register(Register.GS)

	assert RegisterExt.is_gpr(Register.CL)
	assert RegisterExt.is_gpr(Register.DX)
	assert RegisterExt.is_gpr(Register.ESP)
	assert RegisterExt.is_gpr(Register.R15)
	assert not RegisterExt.is_gpr(Register.ES)

	assert RegisterExt.is_gpr8(Register.CL)
	assert not RegisterExt.is_gpr8(Register.DX)
	assert not RegisterExt.is_gpr8(Register.ESP)
	assert not RegisterExt.is_gpr8(Register.R15)
	assert not RegisterExt.is_gpr8(Register.ES)

	assert not RegisterExt.is_gpr16(Register.CL)
	assert RegisterExt.is_gpr16(Register.DX)
	assert not RegisterExt.is_gpr16(Register.ESP)
	assert not RegisterExt.is_gpr16(Register.R15)
	assert not RegisterExt.is_gpr16(Register.ES)

	assert not RegisterExt.is_gpr32(Register.CL)
	assert not RegisterExt.is_gpr32(Register.DX)
	assert RegisterExt.is_gpr32(Register.ESP)
	assert not RegisterExt.is_gpr32(Register.R15)
	assert not RegisterExt.is_gpr32(Register.ES)

	assert not RegisterExt.is_gpr64(Register.CL)
	assert not RegisterExt.is_gpr64(Register.DX)
	assert not RegisterExt.is_gpr64(Register.ESP)
	assert RegisterExt.is_gpr64(Register.R15)
	assert not RegisterExt.is_gpr64(Register.ES)

	assert not RegisterExt.is_vector_register(Register.CL)
	assert RegisterExt.is_vector_register(Register.XMM1)
	assert RegisterExt.is_vector_register(Register.YMM2)
	assert RegisterExt.is_vector_register(Register.ZMM3)

	assert not RegisterExt.is_xmm(Register.CL)
	assert RegisterExt.is_xmm(Register.XMM1)
	assert not RegisterExt.is_xmm(Register.YMM2)
	assert not RegisterExt.is_xmm(Register.ZMM3)

	assert not RegisterExt.is_ymm(Register.CL)
	assert not RegisterExt.is_ymm(Register.XMM1)
	assert RegisterExt.is_ymm(Register.YMM2)
	assert not RegisterExt.is_ymm(Register.ZMM3)

	assert not RegisterExt.is_zmm(Register.CL)
	assert not RegisterExt.is_zmm(Register.XMM1)
	assert not RegisterExt.is_zmm(Register.YMM2)
	assert RegisterExt.is_zmm(Register.ZMM3)

	assert not RegisterExt.is_ip(Register.CL)
	assert RegisterExt.is_ip(Register.EIP)
	assert RegisterExt.is_ip(Register.RIP)

	assert not RegisterExt.is_k(Register.CL)
	assert RegisterExt.is_k(Register.K3)

	assert not RegisterExt.is_cr(Register.CL)
	assert RegisterExt.is_cr(Register.CR3)

	assert not RegisterExt.is_dr(Register.CL)
	assert RegisterExt.is_dr(Register.DR3)

	assert not RegisterExt.is_tr(Register.CL)
	assert RegisterExt.is_tr(Register.TR3)

	assert not RegisterExt.is_st(Register.CL)
	assert RegisterExt.is_st(Register.ST3)

	assert not RegisterExt.is_bnd(Register.CL)
	assert RegisterExt.is_bnd(Register.BND3)

	assert not RegisterExt.is_mm(Register.CL)
	assert RegisterExt.is_mm(Register.MM3)

	assert not RegisterExt.is_tmm(Register.CL)
	assert RegisterExt.is_tmm(Register.TMM3)

@pytest.mark.parametrize("create", [
	lambda register: RegisterExt.info(register),
	lambda register: RegisterInfo(register),
])
def test_register_info(create):
	info = create(Register.R10D)
	assert info.register == Register.R10D
	assert info.base == Register.EAX
	assert info.number == 10
	assert info.full_register == Register.R10
	assert info.full_register32 == Register.R10D
	assert info.size == 4

@pytest.mark.parametrize("create", [
	lambda register: RegisterExt.info(register),
	lambda register: RegisterInfo(register),
])
def test_register_invalid_arg(create):
	with pytest.raises(ValueError):
		create(1234)

def test_ext_base_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.base(1234)

def test_ext_number_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.number(1234)

def test_ext_full_register_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.full_register(1234)

def test_ext_full_register32_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.full_register32(1234)

def test_ext_size_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.size(1234)

def test_ext_is_segment_register_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.is_segment_register(1234)

def test_ext_is_gpr_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.is_gpr(1234)

def test_ext_is_gpr8_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.is_gpr8(1234)

def test_ext_is_gpr16_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.is_gpr16(1234)

def test_ext_is_gpr32_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.is_gpr32(1234)

def test_ext_is_gpr64_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.is_gpr64(1234)

def test_ext_is_xmm_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.is_xmm(1234)

def test_ext_is_ymm_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.is_ymm(1234)

def test_ext_is_zmm_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.is_zmm(1234)

def test_ext_is_vector_register_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.is_vector_register(1234)

def test_ext_is_ip_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.is_ip(1234)

def test_ext_is_k_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.is_k(1234)

def test_ext_is_cr_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.is_cr(1234)

def test_ext_is_dr_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.is_dr(1234)

def test_ext_is_tr_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.is_tr(1234)

def test_ext_is_st_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.is_st(1234)

def test_ext_is_bnd_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.is_bnd(1234)

def test_ext_is_mm_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.is_mm(1234)

def test_ext_is_tmm_invalid_arg():
	with pytest.raises(ValueError):
		RegisterExt.is_tmm(1234)
