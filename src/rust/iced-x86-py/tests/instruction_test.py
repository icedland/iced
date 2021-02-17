# SPDX-License-Identifier: MIT
# Copyright (C) 2018-present iced project and contributors

import copy
import pytest
from iced_x86 import *

def test_ctor():
	instr = Instruction()
	assert instr.code == Code.INVALID
	assert instr.code_size == CodeSize.UNKNOWN
	assert instr.len == 0
	assert instr.ip == 0

def test_eq_ne_hash():
	decodera = Decoder(64, b"\xC4\xE3\x49\x48\x10\x41" b"\xC4\xE3\x49\x48\x10\x42", ip=0x1234_5678_9ABC_DEF1)
	decoderb = Decoder(64, b"\xC4\xE3\x49\x48\x10\x41", ip=0xABCD_EF01_1234_5678)
	instr1 = decodera.decode()
	instr2 = decodera.decode()
	instr3 = decoderb.decode()

	assert instr1 == instr1
	assert not (instr1 != instr1)
	assert instr1 != instr2
	assert not (instr1 == instr2)
	assert instr1 == instr3
	assert not (instr1 != instr3)
	assert hash(instr1) == hash(instr3)

	assert instr1 != 1
	assert instr1 != 1.23
	assert instr1 != None
	assert instr1 != []
	assert instr1 != {}
	assert instr1 != (1, 2)

	assert not (instr1 == 1)
	assert not (instr1 == 1.23)
	assert not (instr1 == None)
	assert not (instr1 == [])
	assert not (instr1 == {})
	assert not (instr1 == (1, 2))

def test_invalid():
	instr = Instruction()
	assert not instr
	assert instr.is_invalid
	instr.code = Code.ADD_AL_IMM8
	assert instr
	assert not instr.is_invalid

@pytest.mark.parametrize("copy_instr", [
	lambda instr: copy.copy(instr),
	lambda instr: copy.deepcopy(instr),
	lambda instr: instr.copy(),
])
def test_copy_deepcopy_mcopy(copy_instr):
	decoder = Decoder(64, b"\xC4\xE3\x49\x48\x10\x41", ip=0x1234_5678_9ABC_DEF1)
	instr = decoder.decode()
	instr2 = copy_instr(instr)
	assert instr is not instr2
	assert id(instr) != id(instr2)

	assert instr == instr2
	assert not (instr != instr2)
	assert instr.eq_all_bits(instr2)
	assert instr2.eq_all_bits(instr)
	assert hash(instr) == hash(instr2)

	instr2.ip += 1
	assert instr == instr2
	assert not (instr != instr2)
	assert not instr.eq_all_bits(instr2)
	assert not instr2.eq_all_bits(instr)
	assert hash(instr) == hash(instr2)

def test_some_props1():
	decoder = Decoder(64, b"\xC4\xE3\x49\x48\x10\x41", ip=0x1234_5678_9ABC_DEF1)
	instr = decoder.decode()

	assert instr.len == 6
	assert len(instr) == 6

	assert instr.ip16 == 0xDEF1
	assert instr.ip32 == 0x9ABC_DEF1
	assert instr.ip == 0x1234_5678_9ABC_DEF1
	assert instr.next_ip16 == 0xDEF7
	assert instr.next_ip32 == 0x9ABC_DEF7
	assert instr.next_ip == 0x1234_5678_9ABC_DEF7

	instr.ip = 0x9ABC_DEF0_1234_5678
	assert instr.ip16 == 0x5678
	assert instr.ip32 == 0x1234_5678
	assert instr.ip == 0x9ABC_DEF0_1234_5678
	assert instr.next_ip16 == 0x567E
	assert instr.next_ip32 == 0x1234_567E
	assert instr.next_ip == 0x9ABC_DEF0_1234_567E

	instr.ip = 0x9ABC_DEF0_1234_5678
	instr.ip32 = 0xABCD_EF01
	assert instr.ip16 == 0xEF01
	assert instr.ip32 == 0xABCD_EF01
	assert instr.ip == 0xABCD_EF01
	assert instr.next_ip16 == 0xEF07
	assert instr.next_ip32 == 0xABCD_EF07
	assert instr.next_ip == 0xABCD_EF07

	instr.ip = 0x9ABC_DEF0_1234_5678
	instr.ip16 = 0xABCD
	assert instr.ip16 == 0xABCD
	assert instr.ip32 == 0xABCD
	assert instr.ip == 0xABCD
	assert instr.next_ip16 == 0xABD3
	assert instr.next_ip32 == 0xABD3
	assert instr.next_ip == 0xABD3

	instr.next_ip = 0x9ABC_DEF0_1234_5678
	assert instr.ip16 == 0x5672
	assert instr.ip32 == 0x1234_5672
	assert instr.ip == 0x9ABC_DEF0_1234_5672
	assert instr.next_ip16 == 0x5678
	assert instr.next_ip32 == 0x1234_5678
	assert instr.next_ip == 0x9ABC_DEF0_1234_5678

	instr.next_ip = 0x9ABC_DEF0_1234_5678
	instr.next_ip32 = 0xABCD_EF01
	assert instr.ip16 == 0xEEFB
	assert instr.ip32 == 0xABCD_EEFB
	assert instr.ip == 0xABCD_EEFB
	assert instr.next_ip16 == 0xEF01
	assert instr.next_ip32 == 0xABCD_EF01
	assert instr.next_ip == 0xABCD_EF01

	instr.next_ip = 0x9ABC_DEF0_1234_5678
	instr.next_ip16 = 0xABCD
	assert instr.ip16 == 0xABC7
	assert instr.ip32 == 0xABC7
	assert instr.ip == 0xABC7
	assert instr.next_ip16 == 0xABCD
	assert instr.next_ip32 == 0xABCD
	assert instr.next_ip == 0xABCD

	assert instr.code == Code.VEX_VPERMIL2PS_XMM_XMM_XMMM128_XMM_IMM4
	assert instr.mnemonic == Mnemonic.VPERMIL2PS
	assert instr.op_count == 5
	instr.code = Code.ADD_AL_IMM8
	assert instr.code == Code.ADD_AL_IMM8
	assert instr.mnemonic == Mnemonic.ADD
	assert instr.op_count == 2

	assert instr.len == 6
	assert len(instr) == 6
	instr.len = 1
	assert instr.len == 1
	assert len(instr) == 1

def test_some_props2():
	decoder = Decoder(64, b"\x00\xCE", ip=0x1234_5678_9ABC_DEF1)
	instr = decoder.decode()

	assert not instr.has_xacquire_prefix
	instr.has_xacquire_prefix = True
	assert instr.has_xacquire_prefix

	assert not instr.has_xrelease_prefix
	instr.has_xrelease_prefix = True
	assert instr.has_xrelease_prefix

	assert not instr.has_rep_prefix
	assert not instr.has_repe_prefix
	instr.has_rep_prefix = True
	assert instr.has_rep_prefix
	assert instr.has_repe_prefix
	instr.has_repe_prefix = False
	assert not instr.has_rep_prefix
	assert not instr.has_repe_prefix

	assert not instr.has_repne_prefix
	instr.has_repne_prefix = True
	assert instr.has_repne_prefix

	assert not instr.has_lock_prefix
	instr.has_lock_prefix = True
	assert instr.has_lock_prefix

	assert instr.op_mask == Register.NONE
	assert not instr.has_op_mask
	instr.op_mask = Register.K2
	assert instr.op_mask == Register.K2
	assert instr.has_op_mask

	assert not instr.zeroing_masking
	assert instr.merging_masking
	instr.zeroing_masking = True
	assert instr.zeroing_masking
	assert not instr.merging_masking
	instr.merging_masking = True
	assert not instr.zeroing_masking
	assert instr.merging_masking

	assert instr.rounding_control == RoundingControl.NONE
	instr.rounding_control = RoundingControl.ROUND_TO_NEAREST
	assert instr.rounding_control == RoundingControl.ROUND_TO_NEAREST

	assert not instr.suppress_all_exceptions
	instr.suppress_all_exceptions = True
	assert instr.suppress_all_exceptions

	assert not instr.is_privileged
	assert not instr.is_save_restore_instruction

@pytest.mark.parametrize("bitness, code_size, data", [
	(16, CodeSize.CODE16, b"\x90"),
	(32, CodeSize.CODE32, b"\x90"),
	(64, CodeSize.CODE64, b"\x90"),
])
def test_code_size(bitness, code_size, data):
	instr = Decoder(bitness, data).decode()
	assert instr.code_size == code_size
	for new_size in [CodeSize.UNKNOWN, CodeSize.CODE16, CodeSize.CODE32, CodeSize.CODE64]:
		instr.code_size = new_size
		assert instr.code_size == new_size

def test_op_kind():
	decoder = Decoder(64, b"\xC4\xE3\x49\x48\x10\x41", ip=0x1234_5678_9ABC_DEF1)
	instr = decoder.decode()

	assert instr.op0_kind == OpKind.REGISTER
	assert instr.op1_kind == OpKind.REGISTER
	assert instr.op2_kind == OpKind.MEMORY
	assert instr.op3_kind == OpKind.REGISTER
	assert instr.op4_kind == OpKind.IMMEDIATE8

	assert instr.op0_kind == instr.op_kind(0)
	assert instr.op1_kind == instr.op_kind(1)
	assert instr.op2_kind == instr.op_kind(2)
	assert instr.op3_kind == instr.op_kind(3)
	assert instr.op4_kind == instr.op_kind(4)

	instr.op0_kind = OpKind.FAR_BRANCH16
	instr.op1_kind = OpKind.FAR_BRANCH32
	instr.op2_kind = OpKind.IMMEDIATE16
	instr.op3_kind = OpKind.IMMEDIATE32

	assert instr.op0_kind == OpKind.FAR_BRANCH16
	assert instr.op1_kind == OpKind.FAR_BRANCH32
	assert instr.op2_kind == OpKind.IMMEDIATE16
	assert instr.op3_kind == OpKind.IMMEDIATE32
	with pytest.raises(ValueError):
		instr.op4_kind = OpKind.IMMEDIATE64
	instr.op4_kind = OpKind.IMMEDIATE8

	assert instr.op0_kind == instr.op_kind(0)
	assert instr.op1_kind == instr.op_kind(1)
	assert instr.op2_kind == instr.op_kind(2)
	assert instr.op3_kind == instr.op_kind(3)
	assert instr.op4_kind == instr.op_kind(4)

	instr.set_op_kind(0, OpKind.IMMEDIATE8)
	instr.set_op_kind(1, OpKind.IMMEDIATE8TO16)
	instr.set_op_kind(2, OpKind.IMMEDIATE8TO32)
	instr.set_op_kind(3, OpKind.IMMEDIATE8TO64)

	assert instr.op0_kind == OpKind.IMMEDIATE8
	assert instr.op1_kind == OpKind.IMMEDIATE8TO16
	assert instr.op2_kind == OpKind.IMMEDIATE8TO32
	assert instr.op3_kind == OpKind.IMMEDIATE8TO64
	with pytest.raises(ValueError):
		instr.set_op_kind(4, OpKind.IMMEDIATE64)
	instr.set_op_kind(4, OpKind.IMMEDIATE8)

	assert instr.op0_kind == instr.op_kind(0)
	assert instr.op1_kind == instr.op_kind(1)
	assert instr.op2_kind == instr.op_kind(2)
	assert instr.op3_kind == instr.op_kind(3)
	assert instr.op4_kind == instr.op_kind(4)

def test_op_register():
	decoder = Decoder(64, b"\xC4\xE3\x49\x48\xD3\x40", ip=0x1234_5678_9ABC_DEF1)
	instr = decoder.decode()
	assert instr.op0_kind == OpKind.REGISTER
	assert instr.op1_kind == OpKind.REGISTER
	assert instr.op2_kind == OpKind.REGISTER
	assert instr.op3_kind == OpKind.REGISTER
	assert instr.op4_kind == OpKind.IMMEDIATE8

	assert instr.op0_register == Register.XMM2
	assert instr.op1_register == Register.XMM6
	assert instr.op2_register == Register.XMM3
	assert instr.op3_register == Register.XMM4
	assert instr.op4_register == Register.NONE

	assert instr.op_register(0) == Register.XMM2
	assert instr.op_register(1) == Register.XMM6
	assert instr.op_register(2) == Register.XMM3
	assert instr.op_register(3) == Register.XMM4
	assert instr.op_register(4) == Register.NONE

	instr.op0_register = Register.XMM1
	instr.op1_register = Register.XMM5
	instr.op2_register = Register.XMM7
	instr.op3_register = Register.XMM13
	with pytest.raises(ValueError):
		instr.op4_register = Register.XMM15
	instr.op4_register = Register.NONE

	assert instr.op0_register == Register.XMM1
	assert instr.op1_register == Register.XMM5
	assert instr.op2_register == Register.XMM7
	assert instr.op3_register == Register.XMM13
	assert instr.op4_register == Register.NONE

	assert instr.op_register(0) == Register.XMM1
	assert instr.op_register(1) == Register.XMM5
	assert instr.op_register(2) == Register.XMM7
	assert instr.op_register(3) == Register.XMM13
	assert instr.op_register(4) == Register.NONE

	instr.set_op_register(0, Register.XMM0)
	instr.set_op_register(1, Register.XMM8)
	instr.set_op_register(2, Register.XMM10)
	instr.set_op_register(3, Register.XMM11)
	with pytest.raises(ValueError):
		instr.set_op_register(4, Register.XMM14)
	instr.set_op_register(4, Register.NONE)

	assert instr.op0_register == Register.XMM0
	assert instr.op1_register == Register.XMM8
	assert instr.op2_register == Register.XMM10
	assert instr.op3_register == Register.XMM11
	assert instr.op4_register == Register.NONE

	assert instr.op_register(0) == Register.XMM0
	assert instr.op_register(1) == Register.XMM8
	assert instr.op_register(2) == Register.XMM10
	assert instr.op_register(3) == Register.XMM11
	assert instr.op_register(4) == Register.NONE

def test_mem():
	decoder = Decoder(64, b"\xC4\xE3\x49\x48\x10\x41", ip=0x1234_5678_9ABC_DEF1)
	instr = decoder.decode()

	assert not instr.has_segment_prefix
	assert instr.segment_prefix == Register.NONE
	assert instr.memory_segment == Register.DS
	instr.segment_prefix = Register.GS
	assert instr.has_segment_prefix
	assert instr.segment_prefix == Register.GS
	assert instr.memory_segment == Register.GS

	assert instr.memory_displ_size == 0
	instr.memory_displ_size = 1
	assert instr.memory_displ_size == 1
	instr.memory_displ_size = 2
	assert instr.memory_displ_size == 2
	instr.memory_displ_size = 4
	assert instr.memory_displ_size == 4
	instr.memory_displ_size = 8
	assert instr.memory_displ_size == 8
	instr.memory_displ_size = 0
	assert instr.memory_displ_size == 0

	assert instr.memory_size == MemorySize.PACKED128_FLOAT32

	assert not instr.is_broadcast
	instr.is_broadcast = True
	assert instr.is_broadcast

	assert instr.memory_index_scale == 1
	instr.memory_index_scale = 2
	assert instr.memory_index_scale == 2
	instr.memory_index_scale = 4
	assert instr.memory_index_scale == 4
	instr.memory_index_scale = 8
	assert instr.memory_index_scale == 8
	instr.memory_index_scale = 1
	assert instr.memory_index_scale == 1

	assert instr.memory_displacement == 0
	instr.memory_displacement = 0xFEDC_BA98_7654_3210
	assert instr.memory_displacement == 0xFEDC_BA98_7654_3210
	instr.memory_displacement = 0x1234_5678_9ABC_DEF1
	assert instr.memory_displacement == 0x1234_5678_9ABC_DEF1

	assert instr.memory_base == Register.RAX
	instr.memory_base = Register.R15D
	assert instr.memory_base == Register.R15D

	assert instr.memory_index == Register.NONE
	instr.memory_index = Register.XMM13
	assert instr.memory_index == Register.XMM13

def test_imm8():
	instr = Instruction()
	instr.op0_kind = OpKind.IMMEDIATE8
	instr.immediate8 = 0xFE
	assert instr.immediate8 == 0xFE
	assert instr.immediate(0) == 0xFE
	instr.set_immediate_i32(0, -0x12)
	assert instr.immediate8 == 0xEE
	assert instr.immediate(0) == 0xEE
	instr.set_immediate_u32(0, 0xFE)
	assert instr.immediate8 == 0xFE
	assert instr.immediate(0) == 0xFE
	instr.set_immediate_i64(0, -0x12)
	assert instr.immediate8 == 0xEE
	assert instr.immediate(0) == 0xEE
	instr.set_immediate_u64(0, 0xFE)
	assert instr.immediate8 == 0xFE
	assert instr.immediate(0) == 0xFE

def test_imm8_2nd():
	instr = Instruction()
	instr.op0_kind = OpKind.IMMEDIATE8_2ND
	instr.immediate8_2nd = 0xFE
	assert instr.immediate8_2nd == 0xFE
	assert instr.immediate(0) == 0xFE
	instr.set_immediate_i32(0, -0x12)
	assert instr.immediate8_2nd == 0xEE
	assert instr.immediate(0) == 0xEE
	instr.set_immediate_u32(0, 0xFE)
	assert instr.immediate8_2nd == 0xFE
	assert instr.immediate(0) == 0xFE
	instr.set_immediate_i64(0, -0x12)
	assert instr.immediate8_2nd == 0xEE
	assert instr.immediate(0) == 0xEE
	instr.set_immediate_u64(0, 0xFE)
	assert instr.immediate8_2nd == 0xFE
	assert instr.immediate(0) == 0xFE

def test_imm16():
	instr = Instruction()
	instr.op0_kind = OpKind.IMMEDIATE16
	instr.immediate16 = 0xFEDC
	assert instr.immediate16 == 0xFEDC
	assert instr.immediate(0) == 0xFEDC
	instr.set_immediate_i32(0, -0x1234)
	assert instr.immediate16 == 0xEDCC
	assert instr.immediate(0) == 0xEDCC
	instr.set_immediate_u32(0, 0xFEDC)
	assert instr.immediate16 == 0xFEDC
	assert instr.immediate(0) == 0xFEDC
	instr.set_immediate_i64(0, -0x1234)
	assert instr.immediate16 == 0xEDCC
	assert instr.immediate(0) == 0xEDCC
	instr.set_immediate_u64(0, 0xFEDC)
	assert instr.immediate16 == 0xFEDC
	assert instr.immediate(0) == 0xFEDC

def test_imm32():
	instr = Instruction()
	instr.op0_kind = OpKind.IMMEDIATE32
	instr.immediate32 = 0xFEDC_BA98
	assert instr.immediate32 == 0xFEDC_BA98
	assert instr.immediate(0) == 0xFEDC_BA98
	instr.set_immediate_i32(0, -0x1234_5678)
	assert instr.immediate32 == 0xEDCB_A988
	assert instr.immediate(0) == 0xEDCB_A988
	instr.set_immediate_u32(0, 0xFEDC_BA98)
	assert instr.immediate32 == 0xFEDC_BA98
	assert instr.immediate(0) == 0xFEDC_BA98
	instr.set_immediate_i64(0, -0x1234_5678)
	assert instr.immediate32 == 0xEDCB_A988
	assert instr.immediate(0) == 0xEDCB_A988
	instr.set_immediate_u64(0, 0xFEDC_BA98)
	assert instr.immediate32 == 0xFEDC_BA98
	assert instr.immediate(0) == 0xFEDC_BA98

def test_imm64():
	instr = Instruction()
	instr.op0_kind = OpKind.IMMEDIATE64
	instr.immediate64 = 0xFEDC_BA98_7654_3219
	assert instr.immediate64 == 0xFEDC_BA98_7654_3219
	assert instr.immediate(0) == 0xFEDC_BA98_7654_3219
	instr.set_immediate_i32(0, -0x1234_5678)
	assert instr.immediate64 == 0xFFFF_FFFF_EDCB_A988
	assert instr.immediate(0) == 0xFFFF_FFFF_EDCB_A988
	instr.set_immediate_u32(0, 0xFEDC_BA98)
	assert instr.immediate64 == 0xFEDC_BA98
	assert instr.immediate(0) == 0xFEDC_BA98
	instr.set_immediate_i64(0, -0x1234_5678_9ABC_DEF1)
	assert instr.immediate64 == 0xEDCB_A987_6543_210F
	assert instr.immediate(0) == 0xEDCB_A987_6543_210F
	instr.set_immediate_u64(0, 0xFEDC_BA98_7654_3219)
	assert instr.immediate64 == 0xFEDC_BA98_7654_3219
	assert instr.immediate(0) == 0xFEDC_BA98_7654_3219

def test_imm8to16():
	instr = Instruction()
	instr.op0_kind = OpKind.IMMEDIATE8TO16
	instr.immediate8to16 = -0x12
	assert instr.immediate8to16 == -0x12
	assert instr.immediate(0) == 0xFFFF_FFFF_FFFF_FFEE
	instr.immediate8to16 = 0x12
	assert instr.immediate8to16 == 0x12
	assert instr.immediate(0) == 0x12
	instr.set_immediate_i32(0, -0x12)
	assert instr.immediate8to16 == -0x12
	assert instr.immediate(0) == 0xFFFF_FFFF_FFFF_FFEE
	instr.set_immediate_u32(0, 0x12)
	assert instr.immediate8to16 == 0x12
	assert instr.immediate(0) == 0x12
	instr.set_immediate_i64(0, -0x12)
	assert instr.immediate8to16 == -0x12
	assert instr.immediate(0) == 0xFFFF_FFFF_FFFF_FFEE
	instr.set_immediate_u64(0, 0x12)
	assert instr.immediate8to16 == 0x12
	assert instr.immediate(0) == 0x12

def test_imm8to32():
	instr = Instruction()
	instr.op0_kind = OpKind.IMMEDIATE8TO32
	instr.immediate8to32 = -0x12
	assert instr.immediate8to32 == -0x12
	assert instr.immediate(0) == 0xFFFF_FFFF_FFFF_FFEE
	instr.immediate8to32 = 0x12
	assert instr.immediate8to32 == 0x12
	assert instr.immediate(0) == 0x12
	instr.set_immediate_i32(0, -0x12)
	assert instr.immediate8to32 == -0x12
	assert instr.immediate(0) == 0xFFFF_FFFF_FFFF_FFEE
	instr.set_immediate_u32(0, 0x12)
	assert instr.immediate8to32 == 0x12
	assert instr.immediate(0) == 0x12
	instr.set_immediate_i64(0, -0x12)
	assert instr.immediate8to32 == -0x12
	assert instr.immediate(0) == 0xFFFF_FFFF_FFFF_FFEE
	instr.set_immediate_u64(0, 0x12)
	assert instr.immediate8to32 == 0x12
	assert instr.immediate(0) == 0x12

def test_imm8to64():
	instr = Instruction()
	instr.op0_kind = OpKind.IMMEDIATE8TO64
	instr.immediate8to64 = -0x12
	assert instr.immediate8to64 == -0x12
	assert instr.immediate(0) == 0xFFFF_FFFF_FFFF_FFEE
	instr.immediate8to64 = 0x12
	assert instr.immediate8to64 == 0x12
	assert instr.immediate(0) == 0x12
	instr.set_immediate_i32(0, -0x12)
	assert instr.immediate8to64 == -0x12
	assert instr.immediate(0) == 0xFFFF_FFFF_FFFF_FFEE
	instr.set_immediate_u32(0, 0x12)
	assert instr.immediate8to64 == 0x12
	assert instr.immediate(0) == 0x12
	instr.set_immediate_i64(0, -0x12)
	assert instr.immediate8to64 == -0x12
	assert instr.immediate(0) == 0xFFFF_FFFF_FFFF_FFEE
	instr.set_immediate_u64(0, 0x12)
	assert instr.immediate8to64 == 0x12
	assert instr.immediate(0) == 0x12

def test_imm32to64():
	instr = Instruction()
	instr.op0_kind = OpKind.IMMEDIATE32TO64
	instr.immediate32to64 = -0x1234_5678
	assert instr.immediate32to64 == -0x1234_5678
	assert instr.immediate(0) == 0xFFFF_FFFF_EDCB_A988
	instr.immediate32to64 = 0x1234_5678
	assert instr.immediate32to64 == 0x1234_5678
	assert instr.immediate(0) == 0x1234_5678
	instr.set_immediate_i32(0, -0x1234_5678)
	assert instr.immediate32to64 == -0x1234_5678
	assert instr.immediate(0) == 0xFFFF_FFFF_EDCB_A988
	instr.set_immediate_u32(0, 0x1234_5678)
	assert instr.immediate32to64 == 0x1234_5678
	assert instr.immediate(0) == 0x1234_5678
	instr.set_immediate_i64(0, -0x1234_5678)
	assert instr.immediate32to64 == -0x1234_5678
	assert instr.immediate(0) == 0xFFFF_FFFF_EDCB_A988
	instr.set_immediate_u64(0, 0x1234_5678)
	assert instr.immediate32to64 == 0x1234_5678
	assert instr.immediate(0) == 0x1234_5678

def test_near_br16():
	instr = Instruction()
	instr.op0_kind = OpKind.NEAR_BRANCH16
	instr.near_branch16 = 0xFEDC
	assert instr.near_branch16 == 0xFEDC
	assert instr.near_branch_target == 0xFEDC
	instr.near_branch16 = 0x1234
	assert instr.near_branch16 == 0x1234
	assert instr.near_branch_target == 0x1234

def test_near_br32():
	instr = Instruction()
	instr.op0_kind = OpKind.NEAR_BRANCH32
	instr.near_branch32 = 0xFEDC_BA98
	assert instr.near_branch32 == 0xFEDC_BA98
	assert instr.near_branch_target == 0xFEDC_BA98
	instr.near_branch32 = 0x1234_5678
	assert instr.near_branch32 == 0x1234_5678
	assert instr.near_branch_target == 0x1234_5678

def test_near_br64():
	instr = Instruction()
	instr.op0_kind = OpKind.NEAR_BRANCH64
	instr.near_branch64 = 0xFEDC_BA98_7654_321F
	assert instr.near_branch64 == 0xFEDC_BA98_7654_321F
	assert instr.near_branch_target == 0xFEDC_BA98_7654_321F
	instr.near_branch64 = 0x1234_5678_9ABC_DEF1
	assert instr.near_branch64 == 0x1234_5678_9ABC_DEF1
	assert instr.near_branch_target == 0x1234_5678_9ABC_DEF1

def test_far_br16():
	instr = Instruction()
	instr.op0_kind = OpKind.FAR_BRANCH16
	instr.far_branch16 = 0x1234
	instr.far_branch_selector = 0xABCD
	assert instr.far_branch16 == 0x1234
	assert instr.far_branch_selector == 0xABCD
	instr.far_branch16 = 0xABCD
	instr.far_branch_selector = 0x1234
	assert instr.far_branch16 == 0xABCD
	assert instr.far_branch_selector == 0x1234

def test_far_br32():
	instr = Instruction()
	instr.op0_kind = OpKind.FAR_BRANCH32
	instr.far_branch32 = 0x1234_5678
	instr.far_branch_selector = 0xABCD
	assert instr.far_branch32 == 0x1234_5678
	assert instr.far_branch_selector == 0xABCD
	instr.far_branch32 = 0xABCD_EF01
	instr.far_branch_selector = 0x1234
	assert instr.far_branch32 == 0xABCD_EF01
	assert instr.far_branch_selector == 0x1234

@pytest.mark.parametrize("data", [
	[0x12],
	[0x12, 0x34],
	[0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77, 0x88],
])
def test_db_u(data):
	assert 1 <= len(data) <= 16
	instr = Instruction()
	instr.code = Code.DECLAREBYTE
	instr.declare_data_len = len(data)
	assert instr.declare_data_len == len(data)
	for i, d in enumerate(data):
		instr.set_declare_byte_value(i, d)
	for i, d in enumerate(data):
		assert instr.get_declare_byte_value(i) == d

@pytest.mark.parametrize("data", [
	[0x12],
	[0x12, 0x34],
	[0x12, 0x34, 0x56, 0x78, -0x12, -0x34, -0x56, -0x78, 0x11, 0x22, 0x33, 0x44, -0x11, -0x22, -0x33, -0x44],
])
def test_db_i(data):
	assert 1 <= len(data) <= 16
	instr = Instruction()
	instr.code = Code.DECLAREBYTE
	instr.declare_data_len = len(data)
	assert instr.declare_data_len == len(data)
	for i, d in enumerate(data):
		instr.set_declare_byte_value_i8(i, d)
	for i, d in enumerate(data):
		assert instr.get_declare_byte_value_i8(i) == d

@pytest.mark.parametrize("data", [
	[0x1234],
	[0x1234, 0x89AB],
	[0x1234, 0x89AB, 0x4567, 0xCDEF, 0x1122, 0x8899, 0x3344, 0xAABB],
])
def test_dw_u(data):
	assert 1 <= len(data) <= 8
	instr = Instruction()
	instr.code = Code.DECLAREWORD
	instr.declare_data_len = len(data)
	assert instr.declare_data_len == len(data)
	for i, d in enumerate(data):
		instr.set_declare_word_value(i, d)
	for i, d in enumerate(data):
		assert instr.get_declare_word_value(i) == d

@pytest.mark.parametrize("data", [
	[0x1234],
	[0x1234, -0x1234],
	[0x1234, -0x1234, 0x4567, -0x4567, 0x1122, -0x1122, 0x3344, -0x3344],
])
def test_dw_i(data):
	assert 1 <= len(data) <= 8
	instr = Instruction()
	instr.code = Code.DECLAREWORD
	instr.declare_data_len = len(data)
	assert instr.declare_data_len == len(data)
	for i, d in enumerate(data):
		instr.set_declare_word_value_i16(i, d)
	for i, d in enumerate(data):
		assert instr.get_declare_word_value_i16(i) == d

@pytest.mark.parametrize("data", [
	[0x1234_5678],
	[0x1234_5678, 0x89AB_CDEF],
	[0x1234_5678, 0x9ABC_CDEF, 0x1122_3344, 0x8899_AABB],
])
def test_dd_u(data):
	assert 1 <= len(data) <= 4
	instr = Instruction()
	instr.code = Code.DECLAREDWORD
	instr.declare_data_len = len(data)
	assert instr.declare_data_len == len(data)
	for i, d in enumerate(data):
		instr.set_declare_dword_value(i, d)
	for i, d in enumerate(data):
		assert instr.get_declare_dword_value(i) == d

@pytest.mark.parametrize("data", [
	[0x1234_5678],
	[0x1234_5678, -0x1234_5678],
	[0x1234_5678, -0x1234_5678, 0x1122_3344, -0x1122_3344],
])
def test_dd_i(data):
	assert 1 <= len(data) <= 4
	instr = Instruction()
	instr.code = Code.DECLAREDWORD
	instr.declare_data_len = len(data)
	assert instr.declare_data_len == len(data)
	for i, d in enumerate(data):
		instr.set_declare_dword_value_i32(i, d)
	for i, d in enumerate(data):
		assert instr.get_declare_dword_value_i32(i) == d

@pytest.mark.parametrize("data", [
	[0x1234_5678_9ABC_DEF0],
	[0x1234_5678_9ABC_DEF0, 0xABCD_EF01_2345_6789],
])
def test_dq_u(data):
	assert 1 <= len(data) <= 2
	instr = Instruction()
	instr.code = Code.DECLAREQWORD
	instr.declare_data_len = len(data)
	assert instr.declare_data_len == len(data)
	for i, d in enumerate(data):
		instr.set_declare_qword_value(i, d)
	for i, d in enumerate(data):
		assert instr.get_declare_qword_value(i) == d

@pytest.mark.parametrize("data", [
	[0x1234_5678_9ABC_DEF0],
	[0x1234_5678_9ABC_DEF0, -0x1234_5678_9ABC_DEF0],
])
def test_dq_i(data):
	assert 1 <= len(data) <= 2
	instr = Instruction()
	instr.code = Code.DECLAREQWORD
	instr.declare_data_len = len(data)
	assert instr.declare_data_len == len(data)
	for i, d in enumerate(data):
		instr.set_declare_qword_value_i64(i, d)
	for i, d in enumerate(data):
		assert instr.get_declare_qword_value_i64(i) == d

@pytest.mark.parametrize("bitness, data, vsib", [
	(64, b"\x29\x18", 0),
	(64, b"\xC4\xE2\x49\x90\x54\xA1\x01", 32),
	(64, b"\xC4\xE2\x49\x91\x54\xA1\x01", 64),
])
def test_vsib(bitness, data, vsib):
	instr = Decoder(bitness, data).decode()
	if vsib == 0:
		assert instr.vsib is None
		assert not instr.is_vsib
		assert not instr.is_vsib32
		assert not instr.is_vsib64
	elif vsib == 32:
		assert instr.vsib is False
		assert instr.is_vsib
		assert instr.is_vsib32
		assert not instr.is_vsib64
	elif vsib == 64:
		assert instr.vsib is True
		assert instr.is_vsib
		assert not instr.is_vsib32
		assert instr.is_vsib64
	else:
		raise ValueError(f"Invalid vsib value: {vsib}")

def test_ip_rel_addr():
	decoder = Decoder(64, b"\x00\x00" b"\x01\x35\x34\x12\x5A\xA5" b"\x67\x01\x35\x34\x12\x5A\xA5", ip=0x1234_5678_9ABC_DEF0)

	instr = decoder.decode()
	assert not instr.is_ip_rel_memory_operand

	instr = decoder.decode()
	assert instr.is_ip_rel_memory_operand
	assert instr.ip_rel_memory_address == 0x1234_5678_4016_F12C

	instr = decoder.decode()
	assert instr.is_ip_rel_memory_operand
	assert instr.ip_rel_memory_address == 0x4016_F133

def test_sp_inc():
	decoder = Decoder(64, b"\x90\x56\x5E")

	instr = decoder.decode()
	assert not instr.is_stack_instruction
	assert instr.stack_pointer_increment == 0

	instr = decoder.decode()
	assert instr.is_stack_instruction
	assert instr.stack_pointer_increment == -8

	instr = decoder.decode()
	assert instr.is_stack_instruction
	assert instr.stack_pointer_increment == 8

@pytest.mark.parametrize("bitness, data, encoding", [
	(64, b"\x56", EncodingKind.LEGACY),
	(64, b"\xC5\xF8\x10\x10", EncodingKind.VEX),
	(64, b"\x62\xF1\x7C\x08\x10\x50\x01", EncodingKind.EVEX),
	(64, b"\x8F\xE8\x48\x85\x10\x40", EncodingKind.XOP),
	(64, b"\x0F\x0F\x88\x34\x12\x5A\xA5\x0C", EncodingKind.D3NOW),
])
def test_encoding(bitness, data, encoding):
	instr = Decoder(bitness, data).decode()
	assert instr.encoding == encoding

def test_cpuid_features():
	instr = Decoder(64, b"\x62\xF1\x7C\x08\x10\x50\x01").decode()
	cpuid_features = instr.cpuid_features()
	assert type(cpuid_features) == list
	assert len(cpuid_features) == 2
	assert set(cpuid_features) == set([CpuidFeature.AVX512VL, CpuidFeature.AVX512F])

def test_cflow():
	decoder = Decoder(64, b"\x90\xCC\x70\x00")

	assert decoder.decode().flow_control == FlowControl.NEXT
	assert decoder.decode().flow_control == FlowControl.INTERRUPT
	assert decoder.decode().flow_control == FlowControl.CONDITIONAL_BRANCH

def test_rflags():
	decoder = Decoder(64, b"\x33\xC0")
	instr = decoder.decode()

	assert instr.rflags_read == RflagsBits.NONE
	assert instr.rflags_written == RflagsBits.NONE
	assert instr.rflags_cleared == RflagsBits.OF | RflagsBits.SF | RflagsBits.CF
	assert instr.rflags_set == RflagsBits.ZF | RflagsBits.PF
	assert instr.rflags_undefined == RflagsBits.AF
	assert instr.rflags_modified == RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF

def test_br_checks():
	instr = Decoder(64, b"\x70\x00").decode()
	assert instr.is_jcc_short_or_near
	assert instr.is_jcc_short
	assert not instr.is_jcc_near

	instr = Decoder(64, b"\x0F\x80\x00\x00\x00\x00").decode()
	assert instr.is_jcc_short_or_near
	assert not instr.is_jcc_short
	assert instr.is_jcc_near

	instr = Decoder(64, b"\xEB\x00").decode()
	assert instr.is_jmp_short_or_near
	assert instr.is_jmp_short
	assert not instr.is_jmp_near

	instr = Decoder(64, b"\xE9\x00\x00\x00\x00").decode()
	assert instr.is_jmp_short_or_near
	assert not instr.is_jmp_short
	assert instr.is_jmp_near

	instr = Decoder(32, b"\xEA\x00\x00\x00\x00\x00\x00").decode()
	assert instr.is_jmp_far
	assert not instr.is_call_far
	assert not instr.is_jmp_far_indirect
	assert not instr.is_call_far_indirect

	instr = Decoder(32, b"\x9A\x00\x00\x00\x00\x00\x00").decode()
	assert not instr.is_jmp_far
	assert instr.is_call_far
	assert not instr.is_jmp_far_indirect
	assert not instr.is_call_far_indirect

	instr = Decoder(64, b"\x48\xFF\x28").decode()
	assert not instr.is_jmp_far
	assert not instr.is_call_far
	assert instr.is_jmp_far_indirect
	assert not instr.is_call_far_indirect

	instr = Decoder(64, b"\x48\xFF\x18").decode()
	assert not instr.is_jmp_far
	assert not instr.is_call_far
	assert not instr.is_jmp_far_indirect
	assert instr.is_call_far_indirect

	instr = Decoder(64, b"\xE8\x00\x00\x00\x00").decode()
	assert instr.is_call_near
	assert not instr.is_jmp_near_indirect
	assert not instr.is_call_near_indirect

	instr = Decoder(64, b"\xFF\x20").decode()
	assert not instr.is_call_near
	assert instr.is_jmp_near_indirect
	assert not instr.is_call_near_indirect

	instr = Decoder(64, b"\xFF\x10").decode()
	assert not instr.is_call_near
	assert not instr.is_jmp_near_indirect
	assert instr.is_call_near_indirect

def test_condition_code():
	instr = Decoder(64, b"\x70\x00").decode()

	assert instr.condition_code == ConditionCode.O
	instr.negate_condition_code()
	assert instr.condition_code == ConditionCode.NO

def test_short_near_br():
	instr = Decoder(64, b"\x70\x00").decode()

	assert instr.code == Code.JO_REL8_64
	instr.as_short_branch()
	assert instr.code == Code.JO_REL8_64
	instr.as_near_branch()
	assert instr.code == Code.JO_REL32_64
	instr.as_near_branch()
	assert instr.code == Code.JO_REL32_64

def test_op_code():
	instr = Decoder(64, b"\x70\x00").decode()
	idef1 = instr.op_code()
	assert idef1.code == Code.JO_REL8_64
	assert idef1 == OpCodeInfo(Code.JO_REL8_64)

def test_repr_str():
	instr = Decoder(64, b"\x48\x05\xA5\x5A\x34\x82").decode()
	assert repr(instr) == "add rax,0FFFFFFFF82345AA5h"
	assert str(instr) == "add rax,0FFFFFFFF82345AA5h"

def test_format():
	decoder = Decoder(64, b"\x48\x05\xA5\x5A\x34\x82" b"\x48\x8B\x05\x88\xA9\xCB\xED" b"\x70\x00", ip=0x1234_5678_9ABC_DEF0)

	instr = decoder.decode()
	assert f"{instr}" == "add rax,0FFFFFFFF82345AA5h"
	assert f"{instr:f}" == "add rax,0FFFFFFFF82345AA5h"
	assert f"{instr:g}" == "add $0xFFFFFFFF82345AA5,%rax"
	assert f"{instr:i}" == "add rax,0FFFFFFFF82345AA5h"
	assert f"{instr:m}" == "add rax,0FFFFFFFF82345AA5h"
	assert f"{instr:n}" == "add rax,0FFFFFFFF82345AA5h"
	assert f"{instr:x}" == "add rax,0xffffffff82345aa5"
	assert f"{instr:X}" == "add rax,0xFFFFFFFF82345AA5"
	assert f"{instr:h}" == "add rax,0ffffffff82345aa5h"
	assert f"{instr:H}" == "add rax,0FFFFFFFF82345AA5h"
	assert f"{instr:Uh}" == "ADD RAX,0ffffffff82345aa5h"
	assert f"{instr:s}" == "add rax, 0FFFFFFFF82345AA5h"
	assert f"{instr:gG}" == "addq $0xFFFFFFFF82345AA5,%rax"
	assert f"{instr:_}" == "add rax,0_FFFF_FFFF_8234_5AA5h"
	assert f"{instr:ixUs_}" == "ADD RAX, 0xffff_ffff_8234_5aa5"

	instr = decoder.decode()
	assert f"{instr}" == "mov rax,[1234567888888885h]"
	assert f"{instr:r}" == "mov rax,[rip-12345678h]"
	assert f"{instr:S}" == "mov rax,ds:[1234567888888885h]"
	assert f"{instr:M}" == "mov rax,qword ptr [1234567888888885h]"

	instr = decoder.decode()
	assert f"{instr}" == "jo short 123456789ABCDEFFh"
	assert f"{instr:B}" == "jo 123456789ABCDEFFh"

def test_format_raise():
	instr = Decoder(64, b"\x48\x05\xA5\x5A\x34\x82").decode()
	with pytest.raises(ValueError):
		f"{instr:!}"

def test_op_kind_raise():
	instr = Instruction()
	with pytest.raises(ValueError):
		instr.op_kind(100)
	with pytest.raises(ValueError):
		instr.set_op_kind(100, OpKind.REGISTER)
	with pytest.raises(ValueError):
		instr.set_op_kind(0, 12345)

def test_op_register_raise():
	instr = Instruction()
	with pytest.raises(ValueError):
		instr.op_register(100)
	with pytest.raises(ValueError):
		instr.set_op_register(100, Register.RAX)
	with pytest.raises(ValueError):
		instr.set_op_register(0, 12345)

def test_immediate_raise():
	instr = Instruction()

	not_imm_op_kinds = [
		OpKind.REGISTER,
		OpKind.NEAR_BRANCH16,
		OpKind.NEAR_BRANCH32,
		OpKind.NEAR_BRANCH64,
		OpKind.FAR_BRANCH16,
		OpKind.FAR_BRANCH32,
		OpKind.MEMORY_SEG_SI,
		OpKind.MEMORY_SEG_ESI,
		OpKind.MEMORY_SEG_RSI,
		OpKind.MEMORY_SEG_DI,
		OpKind.MEMORY_SEG_EDI,
		OpKind.MEMORY_SEG_RDI,
		OpKind.MEMORY_ESDI,
		OpKind.MEMORY_ESEDI,
		OpKind.MEMORY_ESRDI,
		OpKind.MEMORY,
	]

	imm_op_kinds = [
		OpKind.IMMEDIATE8,
		OpKind.IMMEDIATE8_2ND,
		OpKind.IMMEDIATE16,
		OpKind.IMMEDIATE32,
		OpKind.IMMEDIATE64,
		OpKind.IMMEDIATE8TO16,
		OpKind.IMMEDIATE8TO32,
		OpKind.IMMEDIATE8TO64,
		OpKind.IMMEDIATE32TO64,
	]

	for kind in not_imm_op_kinds:
		instr.op0_kind = kind
		with pytest.raises(ValueError):
			instr.immediate(0)
		with pytest.raises(ValueError):
			instr.set_immediate_i32(0, 0)
		with pytest.raises(ValueError):
			instr.set_immediate_u32(0, 0)
		with pytest.raises(ValueError):
			instr.set_immediate_i64(0, 0)
		with pytest.raises(ValueError):
			instr.set_immediate_u64(0, 0)
	for kind in imm_op_kinds:
		instr.op0_kind = kind
		instr.immediate(0)
		instr.set_immediate_i32(0, 0)
		instr.set_immediate_u32(0, 0)
		instr.set_immediate_i64(0, 0)
		instr.set_immediate_u64(0, 0)

	instr.op0_kind = OpKind.IMMEDIATE8
	instr.immediate(0)
	with pytest.raises(ValueError):
		instr.immediate(100)
	with pytest.raises(ValueError):
		instr.set_immediate_i32(100, 0)
	with pytest.raises(ValueError):
		instr.set_immediate_u32(100, 0)
	with pytest.raises(ValueError):
		instr.set_immediate_i64(100, 0)
	with pytest.raises(ValueError):
		instr.set_immediate_u64(100, 0)

def test_db_raise():
	instr = Instruction()
	instr.code = Code.DECLAREBYTE
	NUM = 16
	instr.set_declare_byte_value_i8(NUM - 1, 0)
	instr.set_declare_byte_value(NUM - 1, 0)
	instr.get_declare_byte_value(NUM - 1)
	instr.get_declare_byte_value_i8(NUM - 1)
	with pytest.raises(ValueError):
		instr.set_declare_byte_value_i8(NUM, 0)
	with pytest.raises(ValueError):
		instr.set_declare_byte_value(NUM, 0)
	with pytest.raises(ValueError):
		instr.get_declare_byte_value(NUM)
	with pytest.raises(ValueError):
		instr.get_declare_byte_value_i8(NUM)

def test_dw_raise():
	instr = Instruction()
	instr.code = Code.DECLAREWORD
	NUM = 8
	instr.set_declare_word_value_i16(NUM - 1, 0)
	instr.set_declare_word_value(NUM - 1, 0)
	instr.get_declare_word_value(NUM - 1)
	instr.get_declare_word_value_i16(NUM - 1)
	with pytest.raises(ValueError):
		instr.set_declare_word_value_i16(NUM, 0)
	with pytest.raises(ValueError):
		instr.set_declare_word_value(NUM, 0)
	with pytest.raises(ValueError):
		instr.get_declare_word_value(NUM)
	with pytest.raises(ValueError):
		instr.get_declare_word_value_i16(NUM)

def test_dd_raise():
	instr = Instruction()
	instr.code = Code.DECLAREDWORD
	NUM = 4
	instr.set_declare_dword_value_i32(NUM - 1, 0)
	instr.set_declare_dword_value(NUM - 1, 0)
	instr.get_declare_dword_value(NUM - 1)
	instr.get_declare_dword_value_i32(NUM - 1)
	with pytest.raises(ValueError):
		instr.set_declare_dword_value_i32(NUM, 0)
	with pytest.raises(ValueError):
		instr.set_declare_dword_value(NUM, 0)
	with pytest.raises(ValueError):
		instr.get_declare_dword_value(NUM)
	with pytest.raises(ValueError):
		instr.get_declare_dword_value_i32(NUM)

def test_dq_raise():
	instr = Instruction()
	instr.code = Code.DECLAREQWORD
	NUM = 2
	instr.set_declare_qword_value_i64(NUM - 1, 0)
	instr.set_declare_qword_value(NUM - 1, 0)
	instr.get_declare_qword_value(NUM - 1)
	instr.get_declare_qword_value_i64(NUM - 1)
	with pytest.raises(ValueError):
		instr.set_declare_qword_value_i64(NUM, 0)
	with pytest.raises(ValueError):
		instr.set_declare_qword_value(NUM, 0)
	with pytest.raises(ValueError):
		instr.get_declare_qword_value(NUM)
	with pytest.raises(ValueError):
		instr.get_declare_qword_value_i64(NUM)

def test_code_size_raise():
	instr = Instruction()
	instr.code_size = CodeSize.CODE64
	with pytest.raises(ValueError):
		instr.code_size = 1234

def test_code_raise():
	instr = Instruction()
	instr.code = Code.EVEX_VAESENCLAST_ZMM_ZMM_ZMMM512
	with pytest.raises(ValueError):
		instr.code = 10000

def test_segment_prefix_raise():
	instr = Instruction()
	instr.segment_prefix = Register.FS
	with pytest.raises(ValueError):
		instr.segment_prefix = 1234

def test_memory_base_raise():
	instr = Instruction()
	instr.memory_base = Register.RAX
	with pytest.raises(ValueError):
		instr.memory_base = 1234

def test_memory_index_raise():
	instr = Instruction()
	instr.memory_index = Register.RAX
	with pytest.raises(ValueError):
		instr.memory_index = 1234

def test_op_register_raise():
	instr = Instruction()

	instr.op0_register = Register.RAX
	with pytest.raises(ValueError):
		instr.op0_register = 1234

	instr.set_op_register(0, Register.RAX)
	with pytest.raises(ValueError):
		instr.set_op_register(0, 1234)

	instr.op1_register = Register.RAX
	with pytest.raises(ValueError):
		instr.op1_register = 1234

	instr.set_op_register(1, Register.RAX)
	with pytest.raises(ValueError):
		instr.set_op_register(1, 1234)

	instr.op2_register = Register.RAX
	with pytest.raises(ValueError):
		instr.op2_register = 1234

	instr.set_op_register(2, Register.RAX)
	with pytest.raises(ValueError):
		instr.set_op_register(2, 1234)

	instr.op3_register = Register.RAX
	with pytest.raises(ValueError):
		instr.op3_register = 1234

	instr.set_op_register(3, Register.RAX)
	with pytest.raises(ValueError):
		instr.set_op_register(3, 1234)

	instr.op4_register = Register.NONE
	with pytest.raises(ValueError):
		instr.op4_register = 1234

	instr.set_op_register(4, Register.NONE)
	with pytest.raises(ValueError):
		instr.set_op_register(4, 1234)

def test_op_mask_raise():
	instr = Instruction()
	instr.op_mask = Register.K1
	with pytest.raises(ValueError):
		instr.op_mask = 1234

def test_fpu_stack_increment_info():
	instr = Decoder(64, b"\xDA\x18").decode()
	info = instr.fpu_stack_increment_info()
	assert info.increment == 1
	assert not info.conditional
	assert info.writes_top
