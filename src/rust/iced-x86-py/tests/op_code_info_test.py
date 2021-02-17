# SPDX-License-Identifier: MIT
# Copyright (C) 2018-present iced project and contributors

import pytest
from iced_x86 import *

def test_props():
	idef = OpCodeInfo(Code.EVEX_VMOVAPD_YMM_K1Z_YMMM256)

	assert idef.code == Code.EVEX_VMOVAPD_YMM_K1Z_YMMM256
	assert idef.mnemonic == Mnemonic.VMOVAPD
	assert idef.encoding == EncodingKind.EVEX
	assert idef.is_instruction
	assert idef.mode16
	assert idef.mode32
	assert idef.mode64
	assert not idef.fwait
	assert idef.operand_size == 0
	assert idef.address_size == 0
	assert idef.l == 1
	assert idef.w == 1
	assert not idef.is_lig
	assert not idef.is_wig
	assert not idef.is_wig32
	assert idef.tuple_type == TupleType.N32
	assert idef.memory_size == MemorySize.PACKED256_FLOAT64
	assert idef.broadcast_memory_size == MemorySize.UNKNOWN
	assert not idef.can_broadcast
	assert not idef.can_use_rounding_control
	assert not idef.can_suppress_all_exceptions
	assert idef.can_use_op_mask_register
	assert not idef.require_op_mask_register
	assert idef.can_use_zeroing_masking
	assert not idef.can_use_lock_prefix
	assert not idef.can_use_xacquire_prefix
	assert not idef.can_use_xrelease_prefix
	assert not idef.can_use_rep_prefix
	assert not idef.can_use_repne_prefix
	assert not idef.can_use_bnd_prefix
	assert not idef.can_use_hint_taken_prefix
	assert not idef.can_use_notrack_prefix
	assert not idef.ignores_rounding_control
	assert not idef.amd_lock_reg_bit
	assert not idef.default_op_size64
	assert not idef.force_op_size64
	assert not idef.intel_force_op_size64
	assert not idef.must_be_cpl0
	assert idef.cpl0
	assert idef.cpl1
	assert idef.cpl2
	assert idef.cpl3
	assert not idef.is_input_output
	assert not idef.is_nop
	assert not idef.is_reserved_nop
	assert not idef.is_serializing_intel
	assert not idef.is_serializing_amd
	assert not idef.may_require_cpl0
	assert not idef.is_cet_tracked
	assert not idef.is_non_temporal
	assert not idef.is_fpu_no_wait
	assert not idef.ignores_mod_bits
	assert not idef.no66
	assert not idef.nfx
	assert not idef.requires_unique_reg_nums
	assert not idef.is_privileged
	assert not idef.is_save_restore
	assert not idef.is_stack_instruction
	assert not idef.ignores_segment
	assert not idef.is_op_mask_read_write
	assert not idef.real_mode
	assert idef.protected_mode
	assert not idef.virtual8086_mode
	assert idef.compatibility_mode
	assert idef.long_mode
	assert idef.use_outside_smm
	assert idef.use_in_smm
	assert idef.use_outside_enclave_sgx
	assert idef.use_in_enclave_sgx1
	assert idef.use_in_enclave_sgx2
	assert idef.use_outside_vmx_op
	assert idef.use_in_vmx_root_op
	assert idef.use_in_vmx_non_root_op
	assert idef.use_outside_seam
	assert idef.use_in_seam
	assert not idef.tdx_non_root_gen_ud
	assert not idef.tdx_non_root_gen_ve
	assert not idef.tdx_non_root_may_gen_ex
	assert not idef.intel_vm_exit
	assert not idef.intel_may_vm_exit
	assert not idef.intel_smm_vm_exit
	assert not idef.amd_vm_exit
	assert not idef.amd_may_vm_exit
	assert not idef.tsx_abort
	assert not idef.tsx_impl_abort
	assert not idef.tsx_may_abort
	assert idef.intel_decoder16
	assert idef.intel_decoder32
	assert idef.intel_decoder64
	assert idef.amd_decoder16
	assert idef.amd_decoder32
	assert idef.amd_decoder64
	assert idef.decoder_option == DecoderOptions.NONE
	assert idef.table == OpCodeTableKind.T0F
	assert idef.mandatory_prefix == MandatoryPrefix.P66
	assert idef.op_code == 0x28
	assert idef.op_code_len == 1
	assert not idef.is_group
	assert idef.group_index == -1
	assert not idef.is_rm_group
	assert idef.rm_group_index == -1
	assert idef.op_count == 2
	assert idef.op0_kind == OpCodeOperandKind.YMM_REG
	assert idef.op1_kind == OpCodeOperandKind.YMM_OR_MEM
	assert idef.op2_kind == OpCodeOperandKind.NONE
	assert idef.op3_kind == OpCodeOperandKind.NONE
	assert idef.op4_kind == OpCodeOperandKind.NONE
	assert idef.op_kind(0) == idef.op0_kind
	assert idef.op_kind(1) == idef.op1_kind
	assert idef.op_kind(2) == idef.op2_kind
	assert idef.op_kind(3) == idef.op3_kind
	assert idef.op_kind(4) == idef.op4_kind
	assert type(idef.op_kinds()) == list
	assert idef.op_kinds() == [OpCodeOperandKind.YMM_REG, OpCodeOperandKind.YMM_OR_MEM]
	assert idef.is_available_in_mode(16)
	assert idef.is_available_in_mode(32)
	assert idef.is_available_in_mode(64)
	assert idef.op_code_string == "EVEX.256.66.0F.W1 28 /r"
	assert idef.instruction_string == "VMOVAPD ymm1 {k1}{z}, ymm2/m256"
	assert repr(idef) == "VMOVAPD ymm1 {k1}{z}, ymm2/m256"
	assert str(idef) == "VMOVAPD ymm1 {k1}{z}, ymm2/m256"
	assert f"{idef}" == "VMOVAPD ymm1 {k1}{z}, ymm2/m256"
	assert f"{idef:}" == "VMOVAPD ymm1 {k1}{z}, ymm2/m256"
	assert f"{idef:i}" == "VMOVAPD ymm1 {k1}{z}, ymm2/m256"
	assert f"{idef:o}" == "EVEX.256.66.0F.W1 28 /r"

def test_eq_ne_hash():
	idef1 = OpCodeInfo(Code.EVEX_VMOVAPD_YMM_K1Z_YMMM256)
	idef2 = OpCodeInfo(Code.EVEX_VMOVAPD_YMM_K1Z_YMMM256)
	idef3 = OpCodeInfo(Code.EVEX_VMOVAPD_XMM_K1Z_XMMM128)

	assert idef1 == idef1
	assert idef1 == idef2
	assert idef1 != idef3

	assert not (idef1 != idef1)
	assert not (idef1 != idef2)
	assert not (idef1 == idef3)

	assert idef1 != 1
	assert idef1 != 1.23
	assert idef1 != None
	assert idef1 != []
	assert idef1 != {}
	assert idef1 != (1, 2)

	assert not (idef1 == 1)
	assert not (idef1 == 1.23)
	assert not (idef1 == None)
	assert not (idef1 == [])
	assert not (idef1 == {})
	assert not (idef1 == (1, 2))

	assert hash(idef1) == hash(idef1)
	assert hash(idef1) == hash(idef2)

def test_invalid_format_spec():
	idef = OpCodeInfo(Code.EVEX_VMOVAPD_YMM_K1Z_YMMM256)
	with pytest.raises(ValueError):
		f"{idef:Q}"

def test_invalid_op_kind_arg():
	idef = OpCodeInfo(Code.EVEX_VMOVAPD_YMM_K1Z_YMMM256)
	with pytest.raises(ValueError):
		idef.op_kind(100)

@pytest.mark.parametrize("bitness", [16, 32, 64, 0, 15, 128])
def test_invalid_bitness(bitness):
	idef = OpCodeInfo(Code.EVEX_VMOVAPD_YMM_K1Z_YMMM256)
	idef.is_available_in_mode(bitness)

def test_op_code_raise():
	with pytest.raises(ValueError):
		OpCodeInfo(10000)
