// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::enum_utils::to_code;
use crate::utils::to_value_error;
use core::hash::{Hash, Hasher};
use pyo3::class::basic::CompareOp;
use pyo3::exceptions::PyValueError;
use pyo3::prelude::*;
use std::collections::hash_map::DefaultHasher;

/// Opcode info, returned by :class:`Instruction.op_code` or created by the constructor
///
/// Args:
///     code (:class:`Code`): Code value
///
/// Examples:
///
/// .. testcode::
///
///     from iced_x86 import *
///
///     op_code = OpCodeInfo(Code.EVEX_VMOVAPD_YMM_K1Z_YMMM256)
///     assert op_code.op_code_string == "EVEX.256.66.0F.W1 28 /r"
///     assert op_code.encoding == EncodingKind.EVEX
///     assert OpCodeInfo(Code.SUB_R8_RM8).op_code == 0x2A
///     assert OpCodeInfo(Code.CVTPI2PS_XMM_MMM64).op_code == 0x2A
#[pyclass(module = "iced_x86._iced_x86_py")]
pub(crate) struct OpCodeInfo {
	info: &'static iced_x86::OpCodeInfo,
}

#[pymethods]
impl OpCodeInfo {
	#[new]
	#[pyo3(text_signature = "(code)")]
	pub(crate) fn new(code: u32) -> PyResult<Self> {
		Ok(Self { info: to_code(code)?.op_code() })
	}

	/// :class:`Code`: Gets the code (a :class:`Code` enum value)
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     op_code = OpCodeInfo(Code.EVEX_VMOVAPD_YMM_K1Z_YMMM256)
	///     assert op_code.code == Code.EVEX_VMOVAPD_YMM_K1Z_YMMM256
	#[getter]
	fn code(&self) -> u32 {
		self.info.code() as u32
	}

	/// :class:`Mnemonic`: Gets the mnemonic (a :class:`Mnemonic` enum value)
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     op_code = OpCodeInfo(Code.EVEX_VMOVAPD_YMM_K1Z_YMMM256)
	///     assert op_code.mnemonic == Mnemonic.VMOVAPD
	#[getter]
	fn mnemonic(&self) -> u32 {
		self.info.mnemonic() as u32
	}

	/// :class:`EncodingKind`: Gets the encoding (an :class:`EncodingKind` enum value)
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     op_code = OpCodeInfo(Code.EVEX_VMOVAPD_YMM_K1Z_YMMM256)
	///     assert op_code.encoding == EncodingKind.EVEX
	#[getter]
	fn encoding(&self) -> u32 {
		self.info.encoding() as u32
	}

	/// bool: ``True`` if it's an instruction, ``False`` if it's eg. :class:`Code.INVALID`, ``db``, ``dw``, ``dd``, ``dq``, ``zero_bytes``
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert OpCodeInfo(Code.EVEX_VMOVAPD_YMM_K1Z_YMMM256).is_instruction
	///     assert not OpCodeInfo(Code.INVALID).is_instruction
	///     assert not OpCodeInfo(Code.DECLAREBYTE).is_instruction
	#[getter]
	fn is_instruction(&self) -> bool {
		self.info.is_instruction()
	}

	/// bool: ``True`` if it's an instruction available in 16-bit mode
	#[getter]
	fn mode16(&self) -> bool {
		self.info.mode16()
	}

	/// bool: ``True`` if it's an instruction available in 32-bit mode
	#[getter]
	fn mode32(&self) -> bool {
		self.info.mode32()
	}

	/// bool: ``True`` if it's an instruction available in 64-bit mode
	#[getter]
	fn mode64(&self) -> bool {
		self.info.mode64()
	}

	/// bool: ``True`` if an ``FWAIT`` (``9B``) instruction is added before the instruction
	#[getter]
	fn fwait(&self) -> bool {
		self.info.fwait()
	}

	/// int: (``u8``) (Legacy encoding) Gets the required operand size (16,32,64) or 0
	#[getter]
	fn operand_size(&self) -> u32 {
		self.info.operand_size()
	}

	/// int: (``u8``) (Legacy encoding) Gets the required address size (16,32,64) or 0
	#[getter]
	fn address_size(&self) -> u32 {
		self.info.address_size()
	}

	/// int: (``u8``) (VEX/XOP/EVEX) ``L`` / ``L'L`` value or default value if :class:`OpCodeInfo.is_lig` is ``True``
	#[getter]
	fn l(&self) -> u32 {
		self.info.l()
	}

	/// int: (``u8``) (VEX/XOP/EVEX/MVEX) ``W`` value or default value if :class:`OpCodeInfo.is_wig` or :class:`OpCodeInfo.is_wig32` is ``True``
	#[getter]
	fn w(&self) -> u32 {
		self.info.w()
	}

	/// bool: (VEX/XOP/EVEX) ``True`` if the ``L`` / ``L'L`` fields are ignored.
	///
	/// EVEX: if reg-only ops and ``{er}`` (``EVEX.b`` is set), ``L'L`` is the rounding control and not ignored.
	#[getter]
	fn is_lig(&self) -> bool {
		self.info.is_lig()
	}

	/// bool: (VEX/XOP/EVEX/MVEX) ``True`` if the ``W`` field is ignored in 16/32/64-bit modes
	#[getter]
	fn is_wig(&self) -> bool {
		self.info.is_wig()
	}

	/// bool: (VEX/XOP/EVEX/MVEX) ``True`` if the ``W`` field is ignored in 16/32-bit modes (but not 64-bit mode)
	#[getter]
	fn is_wig32(&self) -> bool {
		self.info.is_wig32()
	}

	/// :class:`TupleType`: (EVEX/MVEX) Gets the tuple type (a :class:`TupleType` enum value)
	#[getter]
	fn tuple_type(&self) -> u32 {
		self.info.tuple_type() as u32
	}

	/// :class:`MvexEHBit`: (MVEX) Gets the ``EH`` bit that's required to encode this instruction (an :class:`MvexEHBit` enum value)
	#[getter]
	fn mvex_eh_bit(&self) -> u32 {
		self.info.mvex_eh_bit() as u32
	}

	/// bool: (MVEX) ``True`` if the instruction supports eviction hint (if it has a memory operand)
	#[getter]
	fn mvex_can_use_eviction_hint(&self) -> bool {
		self.info.mvex_can_use_eviction_hint()
	}

	/// bool: (MVEX) ``True`` if the instruction's rounding control bits are stored in ``imm8[1:0]``
	#[getter]
	fn mvex_can_use_imm_rounding_control(&self) -> bool {
		self.info.mvex_can_use_imm_rounding_control()
	}

	/// bool: (MVEX) ``True`` if the instruction ignores op mask registers (eg. ``{k1}``)
	#[getter]
	fn mvex_ignores_op_mask_register(&self) -> bool {
		self.info.mvex_ignores_op_mask_register()
	}

	/// bool: (MVEX) ``True`` if the instruction must have ``MVEX.SSS=000`` if ``MVEX.EH=1``
	#[getter]
	fn mvex_no_sae_rc(&self) -> bool {
		self.info.mvex_no_sae_rc()
	}

	/// :class:`MvexTupleTypeLutKind`: (MVEX) Gets the tuple type / conv lut kind (an :class:`MvexTupleTypeLutKind` enum value)
	#[getter]
	fn mvex_tuple_type_lut_kind(&self) -> u32 {
		self.info.mvex_tuple_type_lut_kind() as u32
	}

	/// :class:`MvexConvFn`: (MVEX) Gets the conversion function, eg. ``Sf32`` (an :class:`MvexConvFn` enum value)
	#[getter]
	fn mvex_conversion_func(&self) -> u32 {
		self.info.mvex_conversion_func() as u32
	}

	/// int: (``u8``) (MVEX) Gets flags indicating which conversion functions are valid (bit 0 == func 0)
	#[getter]
	fn mvex_valid_conversion_funcs_mask(&self) -> u8 {
		self.info.mvex_valid_conversion_funcs_mask()
	}

	/// int: (``u8``) (MVEX) Gets flags indicating which swizzle functions are valid (bit 0 == func 0)
	#[getter]
	fn mvex_valid_swizzle_funcs_mask(&self) -> u8 {
		self.info.mvex_valid_swizzle_funcs_mask()
	}

	/// :class:`MemorySize`: If it has a memory operand, gets the :class:`MemorySize` (non-broadcast memory type)
	#[getter]
	fn memory_size(&self) -> u32 {
		self.info.memory_size() as u32
	}

	/// :class:`MemorySize`: If it has a memory operand, gets the :class:`MemorySize` (broadcast memory type)
	#[getter]
	fn broadcast_memory_size(&self) -> u32 {
		self.info.broadcast_memory_size() as u32
	}

	/// bool: (EVEX) ``True`` if the instruction supports broadcasting (``EVEX.b`` bit) (if it has a memory operand)
	#[getter]
	fn can_broadcast(&self) -> bool {
		self.info.can_broadcast()
	}

	/// bool: (EVEX/MVEX) ``True`` if the instruction supports rounding control
	#[getter]
	fn can_use_rounding_control(&self) -> bool {
		self.info.can_use_rounding_control()
	}

	/// bool: (EVEX/MVEX) ``True`` if the instruction supports suppress all exceptions
	#[getter]
	fn can_suppress_all_exceptions(&self) -> bool {
		self.info.can_suppress_all_exceptions()
	}

	/// bool: (EVEX/MVEX) ``True`` if an opmask register can be used
	#[getter]
	fn can_use_op_mask_register(&self) -> bool {
		self.info.can_use_op_mask_register()
	}

	/// bool: (EVEX/MVEX) ``True`` if a non-zero opmask register must be used
	#[getter]
	fn require_op_mask_register(&self) -> bool {
		self.info.require_op_mask_register()
	}

	/// bool: (EVEX) ``True`` if the instruction supports zeroing masking (if one of the opmask registers ``K1``-``K7`` is used and destination operand is not a memory operand)
	#[getter]
	fn can_use_zeroing_masking(&self) -> bool {
		self.info.can_use_zeroing_masking()
	}

	/// bool: ``True`` if the ``LOCK`` (``F0``) prefix can be used
	#[getter]
	fn can_use_lock_prefix(&self) -> bool {
		self.info.can_use_lock_prefix()
	}

	/// bool: ``True`` if the ``XACQUIRE`` (``F2``) prefix can be used
	#[getter]
	fn can_use_xacquire_prefix(&self) -> bool {
		self.info.can_use_xacquire_prefix()
	}

	/// bool: ``True`` if the ``XRELEASE`` (``F3``) prefix can be used
	#[getter]
	fn can_use_xrelease_prefix(&self) -> bool {
		self.info.can_use_xrelease_prefix()
	}

	/// bool: ``True`` if the ``REP`` / ``REPE`` (``F3``) prefixes can be used
	#[getter]
	fn can_use_rep_prefix(&self) -> bool {
		self.info.can_use_rep_prefix()
	}

	/// bool: ``True`` if the ``REPNE`` (``F2``) prefix can be used
	#[getter]
	fn can_use_repne_prefix(&self) -> bool {
		self.info.can_use_repne_prefix()
	}

	/// bool: ``True`` if the ``BND`` (``F2``) prefix can be used
	#[getter]
	fn can_use_bnd_prefix(&self) -> bool {
		self.info.can_use_bnd_prefix()
	}

	/// bool: ``True`` if the ``HINT-TAKEN`` (``3E``) and ``HINT-NOT-TAKEN`` (``2E``) prefixes can be used
	#[getter]
	fn can_use_hint_taken_prefix(&self) -> bool {
		self.info.can_use_hint_taken_prefix()
	}

	/// bool: ``True`` if the ``NOTRACK`` (``3E``) prefix can be used
	#[getter]
	fn can_use_notrack_prefix(&self) -> bool {
		self.info.can_use_notrack_prefix()
	}

	/// bool: ``True`` if rounding control is ignored (#UD is not generated)
	#[getter]
	fn ignores_rounding_control(&self) -> bool {
		self.info.ignores_rounding_control()
	}

	/// bool: ``True`` if the ``LOCK`` prefix can be used as an extra register bit (bit 3) to access registers 8-15 without a ``REX`` prefix (eg. in 32-bit mode)
	#[getter]
	fn amd_lock_reg_bit(&self) -> bool {
		self.info.amd_lock_reg_bit()
	}

	/// bool: ``True`` if the default operand size is 64 in 64-bit mode. A ``66`` prefix can switch to 16-bit operand size.
	#[getter]
	fn default_op_size64(&self) -> bool {
		self.info.default_op_size64()
	}

	/// bool: ``True`` if the operand size is always 64 in 64-bit mode. A ``66`` prefix is ignored.
	#[getter]
	fn force_op_size64(&self) -> bool {
		self.info.force_op_size64()
	}

	/// bool: ``True`` if the Intel decoder forces 64-bit operand size. A ``66`` prefix is ignored.
	#[getter]
	fn intel_force_op_size64(&self) -> bool {
		self.info.intel_force_op_size64()
	}

	/// bool: ``True`` if it can only be executed when CPL=0
	#[getter]
	fn must_be_cpl0(&self) -> bool {
		self.info.must_be_cpl0()
	}

	/// bool: ``True`` if it can be executed when CPL=0
	#[getter]
	fn cpl0(&self) -> bool {
		self.info.cpl0()
	}

	/// bool: ``True`` if it can be executed when CPL=1
	#[getter]
	fn cpl1(&self) -> bool {
		self.info.cpl1()
	}

	/// bool: ``True`` if it can be executed when CPL=2
	#[getter]
	fn cpl2(&self) -> bool {
		self.info.cpl2()
	}

	/// bool: ``True`` if it can be executed when CPL=3
	#[getter]
	fn cpl3(&self) -> bool {
		self.info.cpl3()
	}

	/// bool: ``True`` if the instruction accesses the I/O address space (eg. ``IN``, ``OUT``, ``INS``, ``OUTS``)
	#[getter]
	fn is_input_output(&self) -> bool {
		self.info.is_input_output()
	}

	/// bool: ``True`` if it's one of the many nop instructions (does not include FPU nop instructions, eg. ``FNOP``)
	#[getter]
	fn is_nop(&self) -> bool {
		self.info.is_nop()
	}

	/// bool: ``True`` if it's one of the many reserved nop instructions (eg. ``0F0D``, ``0F18-0F1F``)
	#[getter]
	fn is_reserved_nop(&self) -> bool {
		self.info.is_reserved_nop()
	}

	/// bool: ``True`` if it's a serializing instruction (Intel CPUs)
	#[getter]
	fn is_serializing_intel(&self) -> bool {
		self.info.is_serializing_intel()
	}

	/// bool: ``True`` if it's a serializing instruction (AMD CPUs)
	#[getter]
	fn is_serializing_amd(&self) -> bool {
		self.info.is_serializing_amd()
	}

	/// bool: ``True`` if the instruction requires either CPL=0 or CPL<=3 depending on some CPU option (eg. ``CR4.TSD``, ``CR4.PCE``, ``CR4.UMIP``)
	#[getter]
	fn may_require_cpl0(&self) -> bool {
		self.info.may_require_cpl0()
	}

	/// bool: ``True`` if it's a tracked ``JMP``/``CALL`` indirect instruction (CET)
	#[getter]
	fn is_cet_tracked(&self) -> bool {
		self.info.is_cet_tracked()
	}

	/// bool: ``True`` if it's a non-temporal hint memory access (eg. ``MOVNTDQ``)
	#[getter]
	fn is_non_temporal(&self) -> bool {
		self.info.is_non_temporal()
	}

	/// bool: ``True`` if it's a no-wait FPU instruction, eg. ``FNINIT``
	#[getter]
	fn is_fpu_no_wait(&self) -> bool {
		self.info.is_fpu_no_wait()
	}

	/// bool: ``True`` if the mod bits are ignored and it's assumed ``modrm[7:6] == 11b``
	#[getter]
	fn ignores_mod_bits(&self) -> bool {
		self.info.ignores_mod_bits()
	}

	/// bool: ``True`` if the ``66`` prefix is not allowed (it will #UD)
	#[getter]
	fn no66(&self) -> bool {
		self.info.no66()
	}

	/// bool: ``True`` if the ``F2``/``F3`` prefixes aren't allowed
	#[getter]
	fn nfx(&self) -> bool {
		self.info.nfx()
	}

	/// bool: ``True`` if the index reg's reg-num (vsib op) (if any) and register ops' reg-nums must be unique,
	/// eg. ``MNEMONIC XMM1,YMM1,[RAX+ZMM1*2]`` is invalid. Registers = ``XMM``/``YMM``/``ZMM``/``TMM``.
	#[getter]
	fn requires_unique_reg_nums(&self) -> bool {
		self.info.requires_unique_reg_nums()
	}

	/// bool: ``True`` if the destination register's reg-num must not be present in any other operand, eg.
	/// `MNEMONIC XMM1,YMM1,[RAX+ZMM1*2]` is invalid. Registers = `XMM`/`YMM`/`ZMM`/`TMM`.
	#[getter]
	fn requires_unique_dest_reg_num(&self) -> bool {
		self.info.requires_unique_dest_reg_num()
	}

	/// bool: ``True`` if it's a privileged instruction (all CPL=0 instructions (except ``VMCALL``) and IOPL instructions ``IN``, ``INS``, ``OUT``, ``OUTS``, ``CLI``, ``STI``)
	#[getter]
	fn is_privileged(&self) -> bool {
		self.info.is_privileged()
	}

	/// bool: ``True`` if it reads/writes too many registers
	#[getter]
	fn is_save_restore(&self) -> bool {
		self.info.is_save_restore()
	}

	/// bool: ``True`` if it's an instruction that implicitly uses the stack register, eg. ``CALL``, ``POP``, etc
	#[getter]
	fn is_stack_instruction(&self) -> bool {
		self.info.is_stack_instruction()
	}

	/// bool: ``True`` if the instruction doesn't read the segment register if it uses a memory operand
	#[getter]
	fn ignores_segment(&self) -> bool {
		self.info.ignores_segment()
	}

	/// bool: ``True`` if the opmask register is read and written (instead of just read). This also implies that it can't be ``K0``.
	#[getter]
	fn is_op_mask_read_write(&self) -> bool {
		self.info.is_op_mask_read_write()
	}

	/// bool: ``True`` if it can be executed in real mode
	#[getter]
	fn real_mode(&self) -> bool {
		self.info.real_mode()
	}

	/// bool: ``True`` if it can be executed in protected mode
	#[getter]
	fn protected_mode(&self) -> bool {
		self.info.protected_mode()
	}

	/// bool: ``True`` if it can be executed in virtual 8086 mode
	#[getter]
	fn virtual8086_mode(&self) -> bool {
		self.info.virtual8086_mode()
	}

	/// bool: ``True`` if it can be executed in compatibility mode
	#[getter]
	fn compatibility_mode(&self) -> bool {
		self.info.compatibility_mode()
	}

	/// bool: ``True`` if it can be executed in 64-bit mode
	#[getter]
	fn long_mode(&self) -> bool {
		self.info.long_mode()
	}

	/// bool: ``True`` if it can be used outside SMM
	#[getter]
	fn use_outside_smm(&self) -> bool {
		self.info.use_outside_smm()
	}

	/// bool: ``True`` if it can be used in SMM
	#[getter]
	fn use_in_smm(&self) -> bool {
		self.info.use_in_smm()
	}

	/// bool: ``True`` if it can be used outside an enclave (SGX)
	#[getter]
	fn use_outside_enclave_sgx(&self) -> bool {
		self.info.use_outside_enclave_sgx()
	}

	/// bool: ``True`` if it can be used inside an enclave (SGX1)
	#[getter]
	fn use_in_enclave_sgx1(&self) -> bool {
		self.info.use_in_enclave_sgx1()
	}

	/// bool: ``True`` if it can be used inside an enclave (SGX2)
	#[getter]
	fn use_in_enclave_sgx2(&self) -> bool {
		self.info.use_in_enclave_sgx2()
	}

	/// bool: ``True`` if it can be used outside VMX operation
	#[getter]
	fn use_outside_vmx_op(&self) -> bool {
		self.info.use_outside_vmx_op()
	}

	/// bool: ``True`` if it can be used in VMX root operation
	#[getter]
	fn use_in_vmx_root_op(&self) -> bool {
		self.info.use_in_vmx_root_op()
	}

	/// bool: ``True`` if it can be used in VMX non-root operation
	#[getter]
	fn use_in_vmx_non_root_op(&self) -> bool {
		self.info.use_in_vmx_non_root_op()
	}

	/// bool: ``True`` if it can be used outside SEAM
	#[getter]
	fn use_outside_seam(&self) -> bool {
		self.info.use_outside_seam()
	}

	/// bool: ``True`` if it can be used in SEAM
	#[getter]
	fn use_in_seam(&self) -> bool {
		self.info.use_in_seam()
	}

	/// bool: ``True`` if #UD is generated in TDX non-root operation
	#[getter]
	fn tdx_non_root_gen_ud(&self) -> bool {
		self.info.tdx_non_root_gen_ud()
	}

	/// bool: ``True`` if #VE is generated in TDX non-root operation
	#[getter]
	fn tdx_non_root_gen_ve(&self) -> bool {
		self.info.tdx_non_root_gen_ve()
	}

	/// bool: ``True`` if an exception (eg. #GP(0), #VE) may be generated in TDX non-root operation
	#[getter]
	fn tdx_non_root_may_gen_ex(&self) -> bool {
		self.info.tdx_non_root_may_gen_ex()
	}

	/// bool: (Intel VMX) ``True`` if it causes a VM exit in VMX non-root operation
	#[getter]
	fn intel_vm_exit(&self) -> bool {
		self.info.intel_vm_exit()
	}

	/// bool: (Intel VMX) ``True`` if it may cause a VM exit in VMX non-root operation
	#[getter]
	fn intel_may_vm_exit(&self) -> bool {
		self.info.intel_may_vm_exit()
	}

	/// bool: (Intel VMX) ``True`` if it causes an SMM VM exit in VMX root operation (if dual-monitor treatment is activated)
	#[getter]
	fn intel_smm_vm_exit(&self) -> bool {
		self.info.intel_smm_vm_exit()
	}

	/// bool: (AMD SVM) ``True`` if it causes a #VMEXIT in guest mode
	#[getter]
	fn amd_vm_exit(&self) -> bool {
		self.info.amd_vm_exit()
	}

	/// bool: (AMD SVM) ``True`` if it may cause a #VMEXIT in guest mode
	#[getter]
	fn amd_may_vm_exit(&self) -> bool {
		self.info.amd_may_vm_exit()
	}

	/// bool: ``True`` if it causes a TSX abort inside a TSX transaction
	#[getter]
	fn tsx_abort(&self) -> bool {
		self.info.tsx_abort()
	}

	/// bool: ``True`` if it causes a TSX abort inside a TSX transaction depending on the implementation
	#[getter]
	fn tsx_impl_abort(&self) -> bool {
		self.info.tsx_impl_abort()
	}

	/// bool: ``True`` if it may cause a TSX abort inside a TSX transaction depending on some condition
	#[getter]
	fn tsx_may_abort(&self) -> bool {
		self.info.tsx_may_abort()
	}

	/// bool: ``True`` if it's decoded by iced's 16-bit Intel decoder
	#[getter]
	fn intel_decoder16(&self) -> bool {
		self.info.intel_decoder16()
	}

	/// bool: ``True`` if it's decoded by iced's 32-bit Intel decoder
	#[getter]
	fn intel_decoder32(&self) -> bool {
		self.info.intel_decoder32()
	}

	/// bool: ``True`` if it's decoded by iced's 64-bit Intel decoder
	#[getter]
	fn intel_decoder64(&self) -> bool {
		self.info.intel_decoder64()
	}

	/// bool: ``True`` if it's decoded by iced's 16-bit AMD decoder
	#[getter]
	fn amd_decoder16(&self) -> bool {
		self.info.amd_decoder16()
	}

	/// bool: ``True`` if it's decoded by iced's 32-bit AMD decoder
	#[getter]
	fn amd_decoder32(&self) -> bool {
		self.info.amd_decoder32()
	}

	/// bool: ``True`` if it's decoded by iced's 64-bit AMD decoder
	#[getter]
	fn amd_decoder64(&self) -> bool {
		self.info.amd_decoder64()
	}

	/// :class:`DecoderOptions`: Gets the decoder option that's needed to decode the instruction or :class:`DecoderOptions.NONE`.
	#[getter]
	fn decoder_option(&self) -> u32 {
		self.info.decoder_option()
	}

	/// :class:`OpCodeTableKind`: Gets the opcode table (an :class:`OpCodeTableKind` enum value)
	#[getter]
	fn table(&self) -> u32 {
		self.info.table() as u32
	}

	/// :class:`MandatoryPrefix`: Gets the mandatory prefix (a :class:`MandatoryPrefix` enum value)
	#[getter]
	fn mandatory_prefix(&self) -> u32 {
		self.info.mandatory_prefix() as u32
	}

	/// int: (``u32``) Gets the opcode byte(s). The low byte(s) of this value is the opcode. The length is in :class:`OpCodeInfo.op_code_len`.
	/// It doesn't include the table value, see :class:`OpCodeInfo.table`.
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert OpCodeInfo(Code.FFREEP_STI).op_code == 0xDFC0
	///     assert OpCodeInfo(Code.VMRUNW).op_code == 0x01D8
	///     assert OpCodeInfo(Code.SUB_R8_RM8).op_code == 0x2A
	///     assert OpCodeInfo(Code.CVTPI2PS_XMM_MMM64).op_code == 0x2A
	#[getter]
	fn op_code(&self) -> u32 {
		self.info.op_code()
	}

	/// int: (``u8``) Gets the length of the opcode bytes (:class:`OpCodeInfo.op_code`). The low bytes is the opcode value.
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     assert OpCodeInfo(Code.FFREEP_STI).op_code_len == 2
	///     assert OpCodeInfo(Code.VMRUNW).op_code_len == 2
	///     assert OpCodeInfo(Code.SUB_R8_RM8).op_code_len == 1
	///     assert OpCodeInfo(Code.CVTPI2PS_XMM_MMM64).op_code_len == 1
	#[getter]
	fn op_code_len(&self) -> u32 {
		self.info.op_code_len()
	}

	/// bool: ``True`` if it's part of a group
	#[getter]
	fn is_group(&self) -> bool {
		self.info.is_group()
	}

	/// int: (``i8``) Group index (0-7) or -1. If it's 0-7, it's stored in the ``reg`` field of the ``modrm`` byte.
	#[getter]
	fn group_index(&self) -> i32 {
		self.info.group_index()
	}

	/// bool: ``True`` if it's part of a modrm.rm group
	#[getter]
	fn is_rm_group(&self) -> bool {
		self.info.is_rm_group()
	}

	/// int: (``i8``) Group index (0-7) or -1. If it's 0-7, it's stored in the ``rm`` field of the ``modrm`` byte.
	#[getter]
	fn rm_group_index(&self) -> i32 {
		self.info.rm_group_index()
	}

	/// int: (``u8``) Gets the number of operands
	#[getter]
	fn op_count(&self) -> u32 {
		self.info.op_count()
	}

	/// :class:`OpCodeOperandKind`: Gets operand #0's opkind (an :class:`OpCodeOperandKind` enum value)
	#[getter]
	fn op0_kind(&self) -> u32 {
		self.info.op0_kind() as u32
	}

	/// :class:`OpCodeOperandKind`: Gets operand #1's opkind (an :class:`OpCodeOperandKind` enum value)
	#[getter]
	fn op1_kind(&self) -> u32 {
		self.info.op1_kind() as u32
	}

	/// :class:`OpCodeOperandKind`: Gets operand #2's opkind (an :class:`OpCodeOperandKind` enum value)
	#[getter]
	fn op2_kind(&self) -> u32 {
		self.info.op2_kind() as u32
	}

	/// :class:`OpCodeOperandKind`: Gets operand #3's opkind (an :class:`OpCodeOperandKind` enum value)
	#[getter]
	fn op3_kind(&self) -> u32 {
		self.info.op3_kind() as u32
	}

	/// :class:`OpCodeOperandKind`: Gets operand #4's opkind (an :class:`OpCodeOperandKind` enum value)
	#[getter]
	fn op4_kind(&self) -> u32 {
		self.info.op4_kind() as u32
	}

	/// Gets an operand's opkind (an :class:`OpCodeOperandKind` enum value)
	///
	/// Args:
	///     `operand` (int): Operand number, 0-4
	///
	/// Returns:
	///     :class:`OpCodeOperandKind`: Operand kind
	///
	/// Raises:
	///     ValueError: If `operand` is invalid
	#[pyo3(text_signature = "($self, operand)")]
	fn op_kind(&self, operand: u32) -> PyResult<u32> {
		self.info.try_op_kind(operand).map_or_else(|e| Err(to_value_error(e)), |op_kind| Ok(op_kind as u32))
	}

	/// Gets all operand kinds (a list of :class:`OpCodeOperandKind` enum values)
	///
	/// Returns:
	///     List[:class:`OpCodeOperandKind`]: All operand kinds
	#[pyo3(text_signature = "($self)")]
	fn op_kinds(&self) -> Vec<u32> {
		self.info.op_kinds().iter().map(|x| *x as u32).collect()
	}

	/// Checks if the instruction is available in 16-bit mode, 32-bit mode or 64-bit mode
	///
	/// Args:
	///     `bitness` (int): 16, 32 or 64
	///
	/// Returns:
	///     bool: ``True`` if it's available in the mode
	#[pyo3(text_signature = "($self, bitness)")]
	fn is_available_in_mode(&self, bitness: u32) -> bool {
		self.info.is_available_in_mode(bitness)
	}

	/// str: Gets the opcode string, eg. ``VEX.128.66.0F38.W0 78 /r``, see also :class:`OpCodeInfo.instruction_string`
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     op_code = OpCodeInfo(Code.EVEX_VMOVAPD_YMM_K1Z_YMMM256)
	///     assert op_code.op_code_string == "EVEX.256.66.0F.W1 28 /r"
	#[getter]
	fn op_code_string(&self) -> &str {
		self.info.op_code_string()
	}

	/// str: Gets the instruction string, eg. ``VPBROADCASTB xmm1, xmm2/m8``, see also :class:`OpCodeInfo.op_code_string`
	///
	/// Examples:
	///
	/// .. testcode::
	///
	///     from iced_x86 import *
	///
	///     op_code = OpCodeInfo(Code.EVEX_VMOVAPD_YMM_K1Z_YMMM256)
	///     assert op_code.instruction_string == "VMOVAPD ymm1 {k1}{z}, ymm2/m256"
	#[getter]
	fn instruction_string(&self) -> &str {
		self.info.instruction_string()
	}

	fn __format__(&self, format_spec: &str) -> PyResult<&str> {
		match format_spec {
			"" | "i" => Ok(self.info.instruction_string()),
			"o" => Ok(self.info.op_code_string()),
			_ => Err(PyValueError::new_err(format!("Unknown format specifier '{}'", format_spec))),
		}
	}

	fn __repr__(&self) -> &str {
		self.info.instruction_string()
	}

	fn __str__(&self) -> &str {
		self.info.instruction_string()
	}

	fn __richcmp__(&self, other: PyRef<'_, OpCodeInfo>, op: CompareOp) -> PyObject {
		match op {
			CompareOp::Eq => (self.info.code() == other.info.code()).into_py(other.py()),
			CompareOp::Ne => (self.info.code() != other.info.code()).into_py(other.py()),
			_ => other.py().NotImplemented(),
		}
	}

	fn __hash__(&self) -> u64 {
		let mut hasher = DefaultHasher::new();
		self.info.code().hash(&mut hasher);
		hasher.finish()
	}
}
