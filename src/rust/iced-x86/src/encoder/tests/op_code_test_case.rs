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

use super::super::super::*;
#[cfg(not(feature = "std"))]
use alloc::string::String;

#[derive(Default)]
pub(super) struct OpCodeInfoTestCase {
	pub(super) line_number: u32,
	pub(super) code: Code,
	pub(super) mnemonic: Mnemonic,
	pub(super) op_code_string: String,
	pub(super) instruction_string: String,
	pub(super) encoding: EncodingKind,
	pub(super) is_instruction: bool,
	pub(super) mode16: bool,
	pub(super) mode32: bool,
	pub(super) mode64: bool,
	pub(super) fwait: bool,
	pub(super) operand_size: u32,
	pub(super) address_size: u32,
	pub(super) l: u32,
	pub(super) w: u32,
	pub(super) is_lig: bool,
	pub(super) is_wig: bool,
	pub(super) is_wig32: bool,
	pub(super) tuple_type: TupleType,
	pub(super) memory_size: MemorySize,
	pub(super) broadcast_memory_size: MemorySize,
	pub(super) decoder_option: u32,
	pub(super) can_broadcast: bool,
	pub(super) can_use_rounding_control: bool,
	pub(super) can_suppress_all_exceptions: bool,
	pub(super) can_use_op_mask_register: bool,
	pub(super) require_op_mask_register: bool,
	pub(super) can_use_zeroing_masking: bool,
	pub(super) can_use_lock_prefix: bool,
	pub(super) can_use_xacquire_prefix: bool,
	pub(super) can_use_xrelease_prefix: bool,
	pub(super) can_use_rep_prefix: bool,
	pub(super) can_use_repne_prefix: bool,
	pub(super) can_use_bnd_prefix: bool,
	pub(super) can_use_hint_taken_prefix: bool,
	pub(super) can_use_notrack_prefix: bool,
	pub(super) ignores_rounding_control: bool,
	pub(super) amd_lock_reg_bit: bool,
	pub(super) default_op_size64: bool,
	pub(super) force_op_size64: bool,
	pub(super) intel_force_op_size64: bool,
	pub(super) cpl0: bool,
	pub(super) cpl1: bool,
	pub(super) cpl2: bool,
	pub(super) cpl3: bool,
	pub(super) is_input_output: bool,
	pub(super) is_nop: bool,
	pub(super) is_reserved_nop: bool,
	pub(super) is_serializing_intel: bool,
	pub(super) is_serializing_amd: bool,
	pub(super) may_require_cpl0: bool,
	pub(super) is_cet_tracked: bool,
	pub(super) is_non_temporal: bool,
	pub(super) is_fpu_no_wait: bool,
	pub(super) ignores_mod_bits: bool,
	pub(super) no66: bool,
	pub(super) nfx: bool,
	pub(super) requires_unique_reg_nums: bool,
	pub(super) is_privileged: bool,
	pub(super) is_save_restore: bool,
	pub(super) is_stack_instruction: bool,
	pub(super) ignores_segment: bool,
	pub(super) is_op_mask_read_write: bool,
	pub(super) real_mode: bool,
	pub(super) protected_mode: bool,
	pub(super) virtual8086_mode: bool,
	pub(super) compatibility_mode: bool,
	pub(super) long_mode: bool,
	pub(super) use_outside_smm: bool,
	pub(super) use_in_smm: bool,
	pub(super) use_outside_enclave_sgx: bool,
	pub(super) use_in_enclave_sgx1: bool,
	pub(super) use_in_enclave_sgx2: bool,
	pub(super) use_outside_vmx_op: bool,
	pub(super) use_in_vmx_root_op: bool,
	pub(super) use_in_vmx_non_root_op: bool,
	pub(super) use_outside_seam: bool,
	pub(super) use_in_seam: bool,
	pub(super) tdx_non_root_gen_ud: bool,
	pub(super) tdx_non_root_gen_ve: bool,
	pub(super) tdx_non_root_may_gen_ex: bool,
	pub(super) intel_vm_exit: bool,
	pub(super) intel_may_vm_exit: bool,
	pub(super) intel_smm_vm_exit: bool,
	pub(super) amd_vm_exit: bool,
	pub(super) amd_may_vm_exit: bool,
	pub(super) tsx_abort: bool,
	pub(super) tsx_impl_abort: bool,
	pub(super) tsx_may_abort: bool,
	pub(super) intel_decoder16: bool,
	pub(super) intel_decoder32: bool,
	pub(super) intel_decoder64: bool,
	pub(super) amd_decoder16: bool,
	pub(super) amd_decoder32: bool,
	pub(super) amd_decoder64: bool,
	pub(super) table: OpCodeTableKind,
	pub(super) mandatory_prefix: MandatoryPrefix,
	pub(super) op_code: u32,
	pub(super) op_code_len: u32,
	pub(super) is_group: bool,
	pub(super) group_index: i32,
	pub(super) is_rm_group: bool,
	pub(super) rm_group_index: i32,
	pub(super) op_count: u32,
	pub(super) op0_kind: OpCodeOperandKind,
	pub(super) op1_kind: OpCodeOperandKind,
	pub(super) op2_kind: OpCodeOperandKind,
	pub(super) op3_kind: OpCodeOperandKind,
	pub(super) op4_kind: OpCodeOperandKind,
}
