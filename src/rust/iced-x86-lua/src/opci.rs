// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

use crate::enum_utils::to_code;
use loona::lua_api::lua_CFunction;
use loona::prelude::*;

lua_struct_module! { luaopen_iced_x86_OpCodeInfo : OpCodeInfo }
lua_impl_userdata! { OpCodeInfo }

/// Opcode info, returned by `Instruction:op_code()` or created by the constructor
/// @class OpCodeInfo
#[allow(clippy::doc_markdown)]
pub(crate) struct OpCodeInfo {
	inner: &'static iced_x86::OpCodeInfo,
}

impl OpCodeInfo {
	pub(crate) unsafe fn push_new<'lua>(lua: &Lua<'lua>, code: iced_x86::Code) -> &'lua mut OpCodeInfo {
		unsafe {
			let opci = code.op_code();
			let opci = OpCodeInfo { inner: opci };
			let opci = lua.push_user_data(opci);

			lua_get_or_init_metatable!(OpCodeInfo: lua);
			let _ = lua.set_metatable(-2);
			opci
		}
	}

	unsafe fn init_metatable(lua: &Lua<'_>) {
		unsafe {
			lua.push("__index");
			lua.new_table();

			for &(name, method) in OP_CODE_INFO_EXPORTS {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}

			// Write to __index
			lua.raw_set(-3);

			#[rustfmt::skip]
			let special_methods: &[(&str, lua_CFunction)] = &[
				("__tostring", op_code_info_tostring),
				("__eq", op_code_info_eq),
			];
			for &(name, method) in special_methods {
				lua.push(name);
				lua.push(method);
				lua.raw_set(-3);
			}
		}
	}
}

lua_pub_methods! { static OP_CODE_INFO_EXPORTS =>
	/// Creates a new instance
	///
	/// @param code Code # Code value
	/// @return OpCodeInfo
	///
	/// # Examples
	/// ```lua
	/// local Code = require("iced_x86.Code")
	/// local EncodingKind = require("iced_x86.EncodingKind")
	/// local OpCodeInfo = require("iced_x86.OpCodeInfo")
	///
	/// local op_code = OpCodeInfo.new(Code.EVEX_Vmovapd_ymm_k1z_ymmm256)
	/// assert(op_code:op_code_string() == "EVEX.256.66.0F.W1 28 /r")
	/// assert(op_code:encoding() == EncodingKind.EVEX)
	/// assert(OpCodeInfo.new(Code.Sub_r8_rm8):op_code() == 0x2A)
	/// assert(OpCodeInfo.new(Code.Cvtpi2ps_xmm_mmm64):op_code() == 0x2A)
	/// ```
	unsafe fn new(lua, code: u32) -> 1 {
		unsafe { let _ = OpCodeInfo::push_new(lua, to_code(lua, code)); }
	}

	/// Gets the code (a `Code` enum value)
	/// @return integer # A `Code` enum value
	///
	/// # Examples
	/// ```lua
	/// local Code = require("iced_x86.Code")
	/// local OpCodeInfo = require("iced_x86.OpCodeInfo")
	///
	/// local op_code = OpCodeInfo.new(Code.EVEX_Vmovapd_ymm_k1z_ymmm256)
	/// assert(op_code:code() == Code.EVEX_Vmovapd_ymm_k1z_ymmm256)
	/// ```
	unsafe fn code(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.code() as u32); }
	}

	/// Gets the mnemonic (a `Mnemonic` enum value)
	/// @return integer # A `Mnemonic` enum value
	///
	/// # Examples
	/// ```lua
	/// local Code = require("iced_x86.Code")
	/// local OpCodeInfo = require("iced_x86.OpCodeInfo")
	/// local Mnemonic = require("iced_x86.Mnemonic")
	///
	/// local op_code = OpCodeInfo.new(Code.EVEX_Vmovapd_ymm_k1z_ymmm256)
	/// assert(op_code:mnemonic() == Mnemonic.Vmovapd)
	/// ```
	unsafe fn mnemonic(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.mnemonic() as u32); }
	}

	/// Gets the encoding (an `EncodingKind` enum value)
	/// @return integer # An `EncodingKind` enum value
	///
	/// # Examples
	/// ```lua
	/// local Code = require("iced_x86.Code")
	/// local EncodingKind = require("iced_x86.EncodingKind")
	/// local OpCodeInfo = require("iced_x86.OpCodeInfo")
	///
	/// local op_code = OpCodeInfo.new(Code.EVEX_Vmovapd_ymm_k1z_ymmm256)
	/// assert(op_code:encoding() == EncodingKind.EVEX)
	/// ```
	unsafe fn encoding(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.encoding() as u32); }
	}

	/// `true` if it's an instruction, `false` if it's eg. `Code.INVALID`, `db`, `dw`, `dd`, `dq`, `zero_bytes`
	/// @return boolean
	///
	/// # Examples
	/// ```lua
	/// local Code = require("iced_x86.Code")
	/// local OpCodeInfo = require("iced_x86.OpCodeInfo")
	///
	/// assert(OpCodeInfo.new(Code.EVEX_Vmovapd_ymm_k1z_ymmm256):is_instruction())
	/// assert(not OpCodeInfo.new(Code.INVALID):is_instruction())
	/// assert(not OpCodeInfo.new(Code.DeclareByte):is_instruction())
	/// ```
	unsafe fn is_instruction(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.is_instruction()); }
	}

	/// `true` if it's an instruction available in 16-bit mode
	/// @return boolean
	unsafe fn mode16(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.mode16()); }
	}

	/// `true` if it's an instruction available in 32-bit mode
	/// @return boolean
	unsafe fn mode32(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.mode32()); }
	}

	/// `true` if it's an instruction available in 64-bit mode
	/// @return boolean
	unsafe fn mode64(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.mode64()); }
	}

	/// `true` if an `FWAIT` (`9B`) instruction is added before the instruction
	/// @return boolean
	unsafe fn fwait(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.fwait()); }
	}

	/// (Legacy encoding) Gets the required operand size (16,32,64) or 0
	/// @return integer
	unsafe fn operand_size(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.operand_size()); }
	}

	/// (Legacy encoding) Gets the required address size (16,32,64) or 0
	/// @return integer
	unsafe fn address_size(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.address_size()); }
	}

	/// (VEX/XOP/EVEX) `L` / `L'L` value or default value if `OpCodeInfo:is_lig()` is `true`
	/// @return integer
	unsafe fn l(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.l()); }
	}

	/// (VEX/XOP/EVEX/MVEX) `W` value or default value if `OpCodeInfo:is_wig()` or `OpCodeInfo:is_wig32()` is `true`
	/// @return integer
	unsafe fn w(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.w()); }
	}

	/// (VEX/XOP/EVEX) `true` if the `L` / `L'L` fields are ignored.
	///
	/// EVEX: if reg-only ops and `{er}` (`EVEX.b` is set), `L'L` is the rounding control and not ignored.
	///
	/// @return boolean
	unsafe fn is_lig(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.is_lig()); }
	}

	/// (VEX/XOP/EVEX/MVEX) `true` if the `W` field is ignored in 16/32/64-bit modes
	/// @return boolean
	unsafe fn is_wig(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.is_wig()); }
	}

	/// (VEX/XOP/EVEX/MVEX) `true` if the `W` field is ignored in 16/32-bit modes (but not 64-bit mode)
	/// @return boolean
	unsafe fn is_wig32(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.is_wig32()); }
	}

	/// (EVEX/MVEX) Gets the tuple type (a `TupleType` enum value)
	/// @return integer # A `TupleType` enum value
	unsafe fn tuple_type(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.tuple_type() as u32); }
	}

	/// (MVEX) Gets the `EH` bit that's required to encode this instruction (an `MvexEHBit` enum value)
	/// @return integer # An `MvexEHBit` enum value
	unsafe fn mvex_eh_bit(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.mvex_eh_bit() as u32); }
	}

	/// (MVEX) `true` if the instruction supports eviction hint (if it has a memory operand)
	/// @return boolean
	unsafe fn mvex_can_use_eviction_hint(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.mvex_can_use_eviction_hint()); }
	}

	/// (MVEX) `true` if the instruction's rounding control bits are stored in `imm8[1:0]`
	/// @return boolean
	unsafe fn mvex_can_use_imm_rounding_control(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.mvex_can_use_imm_rounding_control()); }
	}

	/// (MVEX) `true` if the instruction ignores op mask registers (eg. `{k1}`)
	/// @return boolean
	unsafe fn mvex_ignores_op_mask_register(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.mvex_ignores_op_mask_register()); }
	}

	/// (MVEX) `true` if the instruction must have `MVEX.SSS=000` if `MVEX.EH=1`
	/// @return boolean
	unsafe fn mvex_no_sae_rc(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.mvex_no_sae_rc()); }
	}

	/// (MVEX) Gets the tuple type / conv lut kind (an `MvexTupleTypeLutKind` enum value)
	/// @return integer # An `MvexTupleTypeLutKind` enum value
	unsafe fn mvex_tuple_type_lut_kind(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.mvex_tuple_type_lut_kind() as u32); }
	}

	/// (MVEX) Gets the conversion function, eg. `Sf32` (an `MvexConvFn` enum value)
	/// @return integer # An `MvexConvFn` enum value
	unsafe fn mvex_conversion_func(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.mvex_conversion_func() as u32); }
	}

	/// (MVEX) Gets flags indicating which conversion functions are valid (bit 0 == func 0)
	/// @return integer
	unsafe fn mvex_valid_conversion_funcs_mask(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.mvex_valid_conversion_funcs_mask()); }
	}

	/// (MVEX) Gets flags indicating which swizzle functions are valid (bit 0 == func 0)
	/// @return integer
	unsafe fn mvex_valid_swizzle_funcs_mask(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.mvex_valid_swizzle_funcs_mask()); }
	}

	/// If it has a memory operand, gets the `MemorySize` (non-broadcast memory type)
	/// @return integer # A `MemorySize` enum value
	unsafe fn memory_size(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.memory_size() as u32); }
	}

	/// If it has a memory operand, gets the `MemorySize` (broadcast memory type)
	/// @return integer # A `MemorySize` enum value
	unsafe fn broadcast_memory_size(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.broadcast_memory_size() as u32); }
	}

	/// (EVEX) `true` if the instruction supports broadcasting (`EVEX.b` bit) (if it has a memory operand)
	/// @return boolean
	unsafe fn can_broadcast(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.can_broadcast()); }
	}

	/// (EVEX/MVEX) `true` if the instruction supports rounding control
	/// @return boolean
	unsafe fn can_use_rounding_control(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.can_use_rounding_control()); }
	}

	/// (EVEX/MVEX) `true` if the instruction supports suppress all exceptions
	/// @return boolean
	unsafe fn can_suppress_all_exceptions(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.can_suppress_all_exceptions()); }
	}

	/// (EVEX/MVEX) `true` if an opmask register can be used
	/// @return boolean
	unsafe fn can_use_op_mask_register(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.can_use_op_mask_register()); }
	}

	/// (EVEX/MVEX) `true` if a non-zero opmask register must be used
	/// @return boolean
	unsafe fn require_op_mask_register(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.require_op_mask_register()); }
	}

	/// (EVEX) `true` if the instruction supports zeroing masking (if one of the opmask registers `K1`-`K7` is used and destination operand is not a memory operand)
	/// @return boolean
	unsafe fn can_use_zeroing_masking(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.can_use_zeroing_masking()); }
	}

	/// `true` if the `LOCK` (`F0`) prefix can be used
	/// @return boolean
	unsafe fn can_use_lock_prefix(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.can_use_lock_prefix()); }
	}

	/// `true` if the `XACQUIRE` (`F2`) prefix can be used
	/// @return boolean
	unsafe fn can_use_xacquire_prefix(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.can_use_xacquire_prefix()); }
	}

	/// `true` if the `XRELEASE` (`F3`) prefix can be used
	/// @return boolean
	unsafe fn can_use_xrelease_prefix(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.can_use_xrelease_prefix()); }
	}

	/// `true` if the `REP` / `REPE` (`F3`) prefixes can be used
	/// @return boolean
	unsafe fn can_use_rep_prefix(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.can_use_rep_prefix()); }
	}

	/// `true` if the `REPNE` (`F2`) prefix can be used
	/// @return boolean
	unsafe fn can_use_repne_prefix(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.can_use_repne_prefix()); }
	}

	/// `true` if the `BND` (`F2`) prefix can be used
	/// @return boolean
	unsafe fn can_use_bnd_prefix(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.can_use_bnd_prefix()); }
	}

	/// `true` if the `HINT-TAKEN` (`3E`) and `HINT-NOT-TAKEN` (`2E`) prefixes can be used
	/// @return boolean
	unsafe fn can_use_hint_taken_prefix(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.can_use_hint_taken_prefix()); }
	}

	/// `true` if the `NOTRACK` (`3E`) prefix can be used
	/// @return boolean
	unsafe fn can_use_notrack_prefix(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.can_use_notrack_prefix()); }
	}

	/// `true` if rounding control is ignored (#UD is not generated)
	/// @return boolean
	unsafe fn ignores_rounding_control(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.ignores_rounding_control()); }
	}

	/// `true` if the `LOCK` prefix can be used as an extra register bit (bit 3) to access registers 8-15 without a `REX` prefix (eg. in 32-bit mode)
	/// @return boolean
	unsafe fn amd_lock_reg_bit(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.amd_lock_reg_bit()); }
	}

	/// `true` if the default operand size is 64 in 64-bit mode. A `66` prefix can switch to 16-bit operand size.
	/// @return boolean
	unsafe fn default_op_size64(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.default_op_size64()); }
	}

	/// `true` if the operand size is always 64 in 64-bit mode. A `66` prefix is ignored.
	/// @return boolean
	unsafe fn force_op_size64(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.force_op_size64()); }
	}

	/// `true` if the Intel decoder forces 64-bit operand size. A `66` prefix is ignored.
	/// @return boolean
	unsafe fn intel_force_op_size64(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.intel_force_op_size64()); }
	}

	/// `true` if it can only be executed when CPL=0
	/// @return boolean
	unsafe fn must_be_cpl0(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.must_be_cpl0()); }
	}

	/// `true` if it can be executed when CPL=0
	/// @return boolean
	unsafe fn cpl0(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.cpl0()); }
	}

	/// `true` if it can be executed when CPL=1
	/// @return boolean
	unsafe fn cpl1(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.cpl1()); }
	}

	/// `true` if it can be executed when CPL=2
	/// @return boolean
	unsafe fn cpl2(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.cpl2()); }
	}

	/// `true` if it can be executed when CPL=3
	/// @return boolean
	unsafe fn cpl3(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.cpl3()); }
	}

	/// `true` if the instruction accesses the I/O address space (eg. `IN`, `OUT`, `INS`, `OUTS`)
	/// @return boolean
	unsafe fn is_input_output(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.is_input_output()); }
	}

	/// `true` if it's one of the many nop instructions (does not include FPU nop instructions, eg. `FNOP`)
	/// @return boolean
	unsafe fn is_nop(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.is_nop()); }
	}

	/// `true` if it's one of the many reserved nop instructions (eg. `0F0D`, `0F18-0F1F`)
	/// @return boolean
	unsafe fn is_reserved_nop(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.is_reserved_nop()); }
	}

	/// `true` if it's a serializing instruction (Intel CPUs)
	/// @return boolean
	unsafe fn is_serializing_intel(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.is_serializing_intel()); }
	}

	/// `true` if it's a serializing instruction (AMD CPUs)
	/// @return boolean
	unsafe fn is_serializing_amd(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.is_serializing_amd()); }
	}

	/// `true` if the instruction requires either CPL=0 or CPL<=3 depending on some CPU option (eg. `CR4.TSD`, `CR4.PCE`, `CR4.UMIP`)
	/// @return boolean
	unsafe fn may_require_cpl0(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.may_require_cpl0()); }
	}

	/// `true` if it's a tracked `JMP`/`CALL` indirect instruction (CET)
	/// @return boolean
	unsafe fn is_cet_tracked(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.is_cet_tracked()); }
	}

	/// `true` if it's a non-temporal hint memory access (eg. `MOVNTDQ`)
	/// @return boolean
	unsafe fn is_non_temporal(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.is_non_temporal()); }
	}

	/// `true` if it's a no-wait FPU instruction, eg. `FNINIT`
	/// @return boolean
	unsafe fn is_fpu_no_wait(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.is_fpu_no_wait()); }
	}

	/// `true` if the mod bits are ignored and it's assumed `modrm[7:6] == 11b`
	/// @return boolean
	unsafe fn ignores_mod_bits(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.ignores_mod_bits()); }
	}

	/// `true` if the `66` prefix is not allowed (it will #UD)
	/// @return boolean
	unsafe fn no66(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.no66()); }
	}

	/// `true` if the `F2`/`F3` prefixes aren't allowed
	/// @return boolean
	unsafe fn nfx(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.nfx()); }
	}

	/// `true` if the index reg's reg-num (vsib op) (if any) and register ops' reg-nums must be unique,
	/// @return boolean
	/// eg. `MNEMONIC XMM1,YMM1,[RAX+ZMM1*2]` is invalid. Registers = `XMM`/`YMM`/`ZMM`/`TMM`.
	unsafe fn requires_unique_reg_nums(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.requires_unique_reg_nums()); }
	}

	/// `true` if the destination register's reg-num must not be present in any other operand, eg.
	/// @return boolean
	/// `MNEMONIC XMM1,YMM1,[RAX+ZMM1*2]` is invalid. Registers = `XMM`/`YMM`/`ZMM`/`TMM`.
	unsafe fn requires_unique_dest_reg_num(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.requires_unique_dest_reg_num()); }
	}

	/// `true` if it's a privileged instruction (all CPL=0 instructions (except `VMCALL`) and IOPL instructions `IN`, `INS`, `OUT`, `OUTS`, `CLI`, `STI`)
	/// @return boolean
	unsafe fn is_privileged(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.is_privileged()); }
	}

	/// `true` if it reads/writes too many registers
	/// @return boolean
	unsafe fn is_save_restore(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.is_save_restore()); }
	}

	/// `true` if it's an instruction that implicitly uses the stack register, eg. `CALL`, `POP`, etc
	/// @return boolean
	unsafe fn is_stack_instruction(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.is_stack_instruction()); }
	}

	/// `true` if the instruction doesn't read the segment register if it uses a memory operand
	/// @return boolean
	unsafe fn ignores_segment(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.ignores_segment()); }
	}

	/// `true` if the opmask register is read and written (instead of just read). This also implies that it can't be `K0`.
	/// @return boolean
	unsafe fn is_op_mask_read_write(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.is_op_mask_read_write()); }
	}

	/// `true` if it can be executed in real mode
	/// @return boolean
	unsafe fn real_mode(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.real_mode()); }
	}

	/// `true` if it can be executed in protected mode
	/// @return boolean
	unsafe fn protected_mode(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.protected_mode()); }
	}

	/// `true` if it can be executed in virtual 8086 mode
	/// @return boolean
	unsafe fn virtual8086_mode(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.virtual8086_mode()); }
	}

	/// `true` if it can be executed in compatibility mode
	/// @return boolean
	unsafe fn compatibility_mode(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.compatibility_mode()); }
	}

	/// `true` if it can be executed in 64-bit mode
	/// @return boolean
	unsafe fn long_mode(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.long_mode()); }
	}

	/// `true` if it can be used outside SMM
	/// @return boolean
	unsafe fn use_outside_smm(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.use_outside_smm()); }
	}

	/// `true` if it can be used in SMM
	/// @return boolean
	unsafe fn use_in_smm(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.use_in_smm()); }
	}

	/// `true` if it can be used outside an enclave (SGX)
	/// @return boolean
	unsafe fn use_outside_enclave_sgx(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.use_outside_enclave_sgx()); }
	}

	/// `true` if it can be used inside an enclave (SGX1)
	/// @return boolean
	unsafe fn use_in_enclave_sgx1(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.use_in_enclave_sgx1()); }
	}

	/// `true` if it can be used inside an enclave (SGX2)
	/// @return boolean
	unsafe fn use_in_enclave_sgx2(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.use_in_enclave_sgx2()); }
	}

	/// `true` if it can be used outside VMX operation
	/// @return boolean
	unsafe fn use_outside_vmx_op(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.use_outside_vmx_op()); }
	}

	/// `true` if it can be used in VMX root operation
	/// @return boolean
	unsafe fn use_in_vmx_root_op(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.use_in_vmx_root_op()); }
	}

	/// `true` if it can be used in VMX non-root operation
	/// @return boolean
	unsafe fn use_in_vmx_non_root_op(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.use_in_vmx_non_root_op()); }
	}

	/// `true` if it can be used outside SEAM
	/// @return boolean
	unsafe fn use_outside_seam(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.use_outside_seam()); }
	}

	/// `true` if it can be used in SEAM
	/// @return boolean
	unsafe fn use_in_seam(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.use_in_seam()); }
	}

	/// `true` if #UD is generated in TDX non-root operation
	/// @return boolean
	unsafe fn tdx_non_root_gen_ud(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.tdx_non_root_gen_ud()); }
	}

	/// `true` if #VE is generated in TDX non-root operation
	/// @return boolean
	unsafe fn tdx_non_root_gen_ve(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.tdx_non_root_gen_ve()); }
	}

	/// `true` if an exception (eg. #GP(0), #VE) may be generated in TDX non-root operation
	/// @return boolean
	unsafe fn tdx_non_root_may_gen_ex(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.tdx_non_root_may_gen_ex()); }
	}

	/// (Intel VMX) `true` if it causes a VM exit in VMX non-root operation
	/// @return boolean
	unsafe fn intel_vm_exit(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.intel_vm_exit()); }
	}

	/// (Intel VMX) `true` if it may cause a VM exit in VMX non-root operation
	/// @return boolean
	unsafe fn intel_may_vm_exit(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.intel_may_vm_exit()); }
	}

	/// (Intel VMX) `true` if it causes an SMM VM exit in VMX root operation (if dual-monitor treatment is activated)
	/// @return boolean
	unsafe fn intel_smm_vm_exit(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.intel_smm_vm_exit()); }
	}

	/// (AMD SVM) `true` if it causes a #VMEXIT in guest mode
	/// @return boolean
	unsafe fn amd_vm_exit(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.amd_vm_exit()); }
	}

	/// (AMD SVM) `true` if it may cause a #VMEXIT in guest mode
	/// @return boolean
	unsafe fn amd_may_vm_exit(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.amd_may_vm_exit()); }
	}

	/// `true` if it causes a TSX abort inside a TSX transaction
	/// @return boolean
	unsafe fn tsx_abort(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.tsx_abort()); }
	}

	/// `true` if it causes a TSX abort inside a TSX transaction depending on the implementation
	/// @return boolean
	unsafe fn tsx_impl_abort(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.tsx_impl_abort()); }
	}

	/// `true` if it may cause a TSX abort inside a TSX transaction depending on some condition
	/// @return boolean
	unsafe fn tsx_may_abort(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.tsx_may_abort()); }
	}

	/// `true` if it's decoded by iced's 16-bit Intel decoder
	/// @return boolean
	unsafe fn intel_decoder16(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.intel_decoder16()); }
	}

	/// `true` if it's decoded by iced's 32-bit Intel decoder
	/// @return boolean
	unsafe fn intel_decoder32(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.intel_decoder32()); }
	}

	/// `true` if it's decoded by iced's 64-bit Intel decoder
	/// @return boolean
	unsafe fn intel_decoder64(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.intel_decoder64()); }
	}

	/// `true` if it's decoded by iced's 16-bit AMD decoder
	/// @return boolean
	unsafe fn amd_decoder16(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.amd_decoder16()); }
	}

	/// `true` if it's decoded by iced's 32-bit AMD decoder
	/// @return boolean
	unsafe fn amd_decoder32(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.amd_decoder32()); }
	}

	/// `true` if it's decoded by iced's 64-bit AMD decoder
	/// @return boolean
	unsafe fn amd_decoder64(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.amd_decoder64()); }
	}

	/// Gets the decoder option that's needed to decode the instruction or `DecoderOptions.None`.
	/// @return integer # A `DecoderOptions` enum value
	unsafe fn decoder_option(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.decoder_option()); }
	}

	/// Gets the opcode table (an `OpCodeTableKind` enum value)
	/// @return integer # An `OpCodeTableKind` enum value
	unsafe fn table(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.table() as u32); }
	}

	/// Gets the mandatory prefix (a `MandatoryPrefix` enum value)
	/// @return integer # A `MandatoryPrefix` enum value
	unsafe fn mandatory_prefix(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.mandatory_prefix() as u32); }
	}

	/// Gets the opcode byte(s). The low byte(s) of this value is the opcode. The length is in `OpCodeInfo:op_code_len()`.
	/// It doesn't include the table value, see `OpCodeInfo:table()`.
	/// @return integer
	///
	/// # Examples
	/// ```lua
	/// local Code = require("iced_x86.Code")
	/// local OpCodeInfo = require("iced_x86.OpCodeInfo")
	///
	/// assert(OpCodeInfo.new(Code.Ffreep_sti):op_code() == 0xDFC0)
	/// assert(OpCodeInfo.new(Code.Vmrunw):op_code() == 0x01D8)
	/// assert(OpCodeInfo.new(Code.Sub_r8_rm8):op_code() == 0x2A)
	/// assert(OpCodeInfo.new(Code.Cvtpi2ps_xmm_mmm64):op_code() == 0x2A)
	/// ```
	unsafe fn op_code(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.op_code()); }
	}

	/// Gets the length of the opcode bytes (`OpCodeInfo:op_code()`). The low bytes is the opcode value.
	/// @return integer
	///
	/// # Examples
	/// ```lua
	/// local Code = require("iced_x86.Code")
	/// local OpCodeInfo = require("iced_x86.OpCodeInfo")
	///
	/// assert(OpCodeInfo.new(Code.Ffreep_sti):op_code_len() == 2)
	/// assert(OpCodeInfo.new(Code.Vmrunw):op_code_len() == 2)
	/// assert(OpCodeInfo.new(Code.Sub_r8_rm8):op_code_len() == 1)
	/// assert(OpCodeInfo.new(Code.Cvtpi2ps_xmm_mmm64):op_code_len() == 1)
	/// ```
	unsafe fn op_code_len(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.op_code_len()); }
	}

	/// `true` if it's part of a group
	/// @return boolean
	unsafe fn is_group(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.is_group()); }
	}

	/// Group index (0-7) or -1. If it's 0-7, it's stored in the `reg` field of the `modrm` byte.
	/// @return integer
	unsafe fn group_index(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.group_index()); }
	}

	/// `true` if it's part of a modrm.rm group
	/// @return boolean
	unsafe fn is_rm_group(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.is_rm_group()); }
	}

	/// Group index (0-7) or -1. If it's 0-7, it's stored in the `rm` field of the `modrm` byte.
	/// @return integer
	unsafe fn rm_group_index(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.rm_group_index()); }
	}

	/// Gets the number of operands
	/// @return integer
	unsafe fn op_count(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.op_count()); }
	}

	/// Gets operand #0's opkind (an `OpCodeOperandKind` enum value)
	/// @return integer # An `OpCodeOperandKind` enum value
	unsafe fn op0_kind(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.op0_kind() as u32); }
	}

	/// Gets operand #1's opkind (an `OpCodeOperandKind` enum value)
	/// @return integer # An `OpCodeOperandKind` enum value
	unsafe fn op1_kind(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.op1_kind() as u32); }
	}

	/// Gets operand #2's opkind (an `OpCodeOperandKind` enum value)
	/// @return integer # An `OpCodeOperandKind` enum value
	unsafe fn op2_kind(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.op2_kind() as u32); }
	}

	/// Gets operand #3's opkind (an `OpCodeOperandKind` enum value)
	/// @return integer # An `OpCodeOperandKind` enum value
	unsafe fn op3_kind(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.op3_kind() as u32); }
	}

	/// Gets operand #4's opkind (an `OpCodeOperandKind` enum value)
	/// @return integer # An `OpCodeOperandKind` enum value
	unsafe fn op4_kind(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.op4_kind() as u32); }
	}

	/// Gets an operand's opkind (an `OpCodeOperandKind` enum value)
	///
	/// @param operand integer # Operand number, 0-4
	/// @return integer # (An `OpCodeOperandKind` enum value) Operand kind
	unsafe fn op_kind(lua, this: &OpCodeInfo, operand: u32) -> 1 {
		match this.inner.try_op_kind(operand) {
			Ok(op_kind) => unsafe { lua.push(op_kind as u32) },
			Err(e) => unsafe { lua.throw_error(e) },
		}
	}

	/// Gets all operand kinds (a list of `OpCodeOperandKind` enum values)
	/// @return integer[] # (`OpCodeOperandKind[]`) All operand kinds
	unsafe fn op_kinds(lua, this: &OpCodeInfo) -> 1 {
		let op_kinds = this.inner.op_kinds();
		unsafe { lua.push_array(op_kinds, |_, op_kind| *op_kind as u32); }
	}

	/// Checks if the instruction is available in 16-bit mode, 32-bit mode or 64-bit mode
	///
	/// @param bitness integer # 16, 32 or 64
	/// @return boolean # `true` if it's available in the mode
	unsafe fn is_available_in_mode(lua, this: &OpCodeInfo, bitness: u32) -> 1 {
		unsafe { lua.push(this.inner.is_available_in_mode(bitness)); }
	}

	/// Gets the opcode string, eg. `VEX.128.66.0F38.W0 78 /r`, see also `OpCodeInfo:instruction_string()`
	/// @return string
	///
	/// # Examples
	/// ```lua
	/// local Code = require("iced_x86.Code")
	/// local OpCodeInfo = require("iced_x86.OpCodeInfo")
	///
	/// local op_code = OpCodeInfo.new(Code.EVEX_Vmovapd_ymm_k1z_ymmm256)
	/// assert(op_code:op_code_string() == "EVEX.256.66.0F.W1 28 /r")
	/// ```
	unsafe fn op_code_string(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.op_code_string()); }
	}

	/// Gets the instruction string, eg. `VPBROADCASTB xmm1, xmm2/m8`, see also `OpCodeInfo:op_code_string()`
	/// @return string
	///
	/// # Examples
	/// ```lua
	/// local Code = require("iced_x86.Code")
	/// local OpCodeInfo = require("iced_x86.OpCodeInfo")
	///
	/// local op_code = OpCodeInfo.new(Code.EVEX_Vmovapd_ymm_k1z_ymmm256)
	/// assert(op_code:instruction_string() == "VMOVAPD ymm1 {k1}{z}, ymm2/m256")
	/// ```
	unsafe fn instruction_string(lua, this: &OpCodeInfo) -> 1 {
		unsafe { lua.push(this.inner.instruction_string()); }
	}
}

lua_methods! {
	unsafe fn op_code_info_tostring(lua, opci: &OpCodeInfo) -> 1 {
		unsafe { lua.push(opci.inner.instruction_string()) }
	}

	unsafe fn op_code_info_eq(lua, opci: &OpCodeInfo, opci2: &OpCodeInfo) -> 1 {
		#[allow(trivial_casts)]
		unsafe { lua.push(opci.inner as *const _ == opci2.inner as *const _) }
	}
}
