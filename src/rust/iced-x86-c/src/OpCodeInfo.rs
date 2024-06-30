/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86::Code;
use std::mem::transmute;// Enum<->Int

//#[allow( non_upper_case_globals )]
//pub const OpCodeInfoCharCount : usize = 30;

//#[repr(C)]
#[repr(packed)]
pub struct TOpCodeInfo { 
//    pub op_code_string: [u8;OpCodeInfoCharCount],
//    pub instruction_string: [u8;OpCodeInfoCharCount],

	pub code: u16/*Code*/,
	pub op_code: u16,
	pub encoding: u8/*EncodingKind*/,
	pub operand_size: u8,
	pub address_size: u8,
	pub l: u8,
	pub tuple_type: u8/*TupleType*/,
	pub table: u8/*OpCodeTableKind*/,
	pub mandatory_prefix: u8/*MandatoryPrefix*/,
	pub group_index: i8,
	pub rm_group_index: i8,
	pub op_kinds: [u8/*OpCodeOperandKind*/; 5/*IcedConstants::MAX_OP_COUNT*/],
}

// Gets the opcode string, eg. `VEX.128.66.0F38.W0 78 /r`, see also [`instruction_string()`]
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_OpCodeString( Code : u16, Output : *mut u8, Size : usize ) { // FFI-Unsafe: Code    
    if Output.is_null() {
        return;
    }
    if Size <= 0 {
        return;
    }
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();
    
    let output = info.op_code_string();
    let mut l = output.len();
    if l > Size {
        l = Size;
    }
    
    if l > 0 {
        for i in 0..l {
            *( Output.add( i ) ) = output.as_bytes()[ i ];
        }
    }
    *( Output.add( l ) ) = 0;
}

// Gets the instruction string, eg. `VPBROADCASTB xmm1, xmm2/m8`, see also [`op_code_string()`]
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_InstructionString( Code : u16, Output : *mut u8, Size : usize ) { // FFI-Unsafe: Code    
    if Output.is_null() {
        return;
    }
    if Size <= 0 {
        return;
    }
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();    
    
    let output = info.instruction_string();
    let mut l = output.len();
    if l > Size {
        l = Size;
    }
    
    if l > 0 {
        for i in 0..l {
            *( Output.add( i ) ) = output.as_bytes()[ i ];
        }
    }
    *( Output.add( l ) ) = 0;
}

// `true` if it's an instruction available in 16-bit mode
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_Mode16( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();
    
    return info.mode16();
}

// `true` if it's an instruction available in 32-bit mode
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_Mode32( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.mode32();
}

// `true` if it's an instruction available in 64-bit mode
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_Mode64( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.mode64();
}

// `true` if an `FWAIT` (`9B`) instruction is added before the instruction
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_Fwait( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.fwait();
}

// (VEX/XOP/EVEX/MVEX) `W` value or default value if [`is_wig()`] or [`is_wig32()`] is `true`
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_W( Code : u16 ) -> u32 { // FFI Unsafe: Code
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.w();
}

// (VEX/XOP/EVEX) `true` if the `L` / `L'L` fields are ignored.
//
// EVEX: if reg-only ops and `{er}` (`EVEX.b` is set), `L'L` is the rounding control and not ignored.
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IsLig( Code : u16 ) -> bool { // FFI Unsafe: Code
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.is_lig();
}

// (VEX/XOP/EVEX/MVEX) `true` if the `W` field is ignored in 16/32/64-bit modes
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IsWig( Code : u16 ) -> bool { // FFI Unsafe: Code
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.is_wig();
}

// (VEX/XOP/EVEX/MVEX) `true` if the `W` field is ignored in 16/32-bit modes (but not 64-bit mode)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IsWig32( Code : u16 ) -> bool { // FFI Unsafe: Code
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.is_wig32();
}

// (MVEX) Gets the `EH` bit that's required to encode this instruction
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_MvexEhBit( Code : u16 ) -> u8 { // FFI Unsafe: Code, MvexEHBit
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.mvex_eh_bit() as u8;
}

// (MVEX) `true` if the instruction supports eviction hint (if it has a memory operand)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_MvexCanUseEvictionHint( Code : u16 ) -> bool { // FFI Unsafe: Code
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.mvex_can_use_eviction_hint();
}

// (MVEX) `true` if the instruction's rounding control bits are stored in `imm8[1:0]`
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_MvexCanUseImmRoundingControl( Code : u16 ) -> bool { // FFI Unsafe: Code
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.mvex_can_use_imm_rounding_control();
}

// (MVEX) `true` if the instruction ignores op mask registers (eg. `{k1}`)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_MvexIgnoresOpMaskRegister( Code : u16 ) -> bool { // FFI Unsafe: Code
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.mvex_ignores_op_mask_register();
}

// (MVEX) `true` if the instruction must have `MVEX.SSS=000` if `MVEX.EH=1`
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_MvexNoSaeRc( Code : u16 ) -> bool { // FFI Unsafe: Code
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.mvex_no_sae_rc();
}

// (MVEX) Gets the tuple type / conv lut kind
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_MvexTupleTypeLutKind( Code : u16 ) -> u8 { // FFI Unsafe: Code, MvexTupleTypeLutKind
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.mvex_tuple_type_lut_kind() as u8;
}

// (MVEX) Gets the conversion function, eg. `Sf32`
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_MvexConversionFunc( Code : u16 ) -> u8 { // FFI Unsafe: Code, MvexConvFn
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.mvex_conversion_func() as u8;
}

// (MVEX) Gets flags indicating which conversion functions are valid (bit 0 == func 0)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_MvexValidConversionFuncsMask( Code : u16 ) -> u8 { // FFI Unsafe: Code
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.mvex_valid_conversion_funcs_mask();
}

// (MVEX) Gets flags indicating which swizzle functions are valid (bit 0 == func 0)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_MvexValidSwizzleFuncsMask( Code : u16 ) -> u8 { // FFI Unsafe: Code
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.mvex_valid_swizzle_funcs_mask();
}

// If it has a memory operand, gets the [`MemorySize`] (non-broadcast memory type)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_MemorySize( Code : u16 ) -> u8 { // FFI Unsafe: Code, MemorySize
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.memory_size() as u8;
}

// If it has a memory operand, gets the [`MemorySize`] (broadcast memory type)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_BroadcastMemorySize( Code : u16 ) -> u8 { // FFI Unsafe: Code, MemorySize
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.broadcast_memory_size() as u8;
}

// (EVEX) `true` if the instruction supports broadcasting (`EVEX.b` bit) (if it has a memory operand)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_CanBroadcast( Code : u16 ) -> bool { // FFI Unsafe: Code
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.can_broadcast();
}

// (EVEX/MVEX) `true` if the instruction supports rounding control
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_CanUseRoundingControl( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.can_use_rounding_control();
}

// (EVEX/MVEX) `true` if the instruction supports suppress all exceptions
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_CanSuppressAllExceptions( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.can_suppress_all_exceptions();
}

// (EVEX/MVEX) `true` if an opmask register can be used
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_CanUseOpMaskRegister( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.can_use_op_mask_register();
}

// (EVEX/MVEX) `true` if a non-zero opmask register must be used
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_RequireOpMaskRegister( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.require_op_mask_register();
}

// (EVEX) `true` if the instruction supports zeroing masking (if one of the opmask registers `K1`-`K7` is used and destination operand is not a memory operand)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_CanUseZeroingMasking( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.can_use_zeroing_masking();
}

// `true` if the `LOCK` (`F0`) prefix can be used
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_CanUseLockPrefix( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.can_use_lock_prefix();
}

// `true` if the `XACQUIRE` (`F2`) prefix can be used
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_CanUseXacquirePrefix( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.can_use_xacquire_prefix();
}

// `true` if the `XRELEASE` (`F3`) prefix can be used
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_CanUseXreleasePrefix( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.can_use_xrelease_prefix();
}

// `true` if the `REP` / `REPE` (`F3`) prefixes can be used
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_CanUseRepPrefix( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.can_use_rep_prefix();
}

// `true` if the `REPNE` (`F2`) prefix can be used
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_CanUseRepnePrefix( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.can_use_repne_prefix();
}

// `true` if the `BND` (`F2`) prefix can be used
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_CanUseBndPrefix( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.can_use_bnd_prefix();
}

// `true` if the `HINT-TAKEN` (`3E`) and `HINT-NOT-TAKEN` (`2E`) prefixes can be used
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_CanUseHintTakenPrefix( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.can_use_hint_taken_prefix();
}

// `true` if the `NOTRACK` (`3E`) prefix can be used
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_CanUseNotrackPrefix( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.can_use_notrack_prefix();
}

// `true` if rounding control is ignored (#UD is not generated)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IgnoresRoundingControl( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.ignores_rounding_control();
}

// `true` if the `LOCK` prefix can be used as an extra register bit (bit 3) to access registers 8-15 without a `REX` prefix (eg. in 32-bit mode)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_AmdLockRegBit( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.amd_lock_reg_bit();
}

// `true` if the default operand size is 64 in 64-bit mode. A `66` prefix can switch to 16-bit operand size.
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_DefaultOpSize64( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();
 
    return info.default_op_size64();
}

// `true` if the operand size is always 64 in 64-bit mode. A `66` prefix is ignored.
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_ForceOpSize64( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.force_op_size64();
}

// `true` if the Intel decoder forces 64-bit operand size. A `66` prefix is ignored.
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IntelForceOpSize64( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.intel_force_op_size64();
}

// `true` if it can only be executed when CPL=0
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_MustBeCpl0( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.must_be_cpl0();
}

// `true` if it can be executed when CPL=0
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_Cpl0( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.cpl0();
}

// `true` if it can be executed when CPL=1
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_Cpl1( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.cpl1();
}

// `true` if it can be executed when CPL=2
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_Cpl2( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.cpl2();
}

// `true` if it can be executed when CPL=3
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_Cpl3( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.cpl3();
}

// `true` if the instruction accesses the I/O address space (eg. `IN`, `OUT`, `INS`, `OUTS`)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IsInputOutput( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.is_input_output();
}

// `true` if it's one of the many nop instructions (does not include FPU nop instructions, eg. `FNOP`)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IsNop( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.is_nop();
}

// `true` if it's one of the many reserved nop instructions (eg. `0F0D`, `0F18-0F1F`)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IsReservedNop( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.is_reserved_nop();
}

// `true` if it's a serializing instruction (Intel CPUs)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IsSerializingIntel( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.is_serializing_intel();
}

// `true` if it's a serializing instruction (AMD CPUs)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IsSerializingAmd( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();
 
    return info.is_serializing_amd();
}

// `true` if the instruction requires either CPL=0 or CPL<=3 depending on some CPU option (eg. `CR4.TSD`, `CR4.PCE`, `CR4.UMIP`)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_MayRequireCpl0( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.may_require_cpl0();
}

// `true` if it's a tracked `JMP`/`CALL` indirect instruction (CET)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IsCetTracked( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.is_cet_tracked();
}

// `true` if it's a non-temporal hint memory access (eg. `MOVNTDQ`)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IsNonTemporal( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.is_non_temporal();
}

// `true` if it's a no-wait FPU instruction, eg. `FNINIT`
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IsFpuNoWait( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.is_fpu_no_wait();
}

// `true` if the mod bits are ignored and it's assumed `modrm[7:6] == 11b`
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IgnoresModBits( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.ignores_mod_bits();
}

// `true` if the `66` prefix is not allowed (it will #UD)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_No66( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.no66();
}

// `true` if the `F2`/`F3` prefixes aren't allowed
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_Nfx( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.nfx();
}

// `true` if the index reg's reg-num (vsib op) (if any) and register ops' reg-nums must be unique,
// eg. `MNEMONIC XMM1,YMM1,[RAX+ZMM1*2]` is invalid. Registers = `XMM`/`YMM`/`ZMM`/`TMM`.
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_RequiresUniqueRegNums( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.requires_unique_reg_nums();
}

// `true` if the destination register's reg-num must not be present in any other operand, eg. `MNEMONIC XMM1,YMM1,[RAX+ZMM1*2]`
// is invalid. Registers = `XMM`/`YMM`/`ZMM`/`TMM`.
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_RequiresUniqueDestRegNum( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.requires_unique_dest_reg_num();
}

// `true` if it's a privileged instruction (all CPL=0 instructions (except `VMCALL`) and IOPL instructions `IN`, `INS`, `OUT`, `OUTS`, `CLI`, `STI`)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IsPrivileged( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.is_privileged();
}

// `true` if it reads/writes too many registers
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IsSaveRestore( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.is_save_restore();
}

// `true` if it's an instruction that implicitly uses the stack register, eg. `CALL`, `POP`, etc
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IsStackInstruction( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.is_stack_instruction();
}

// `true` if the instruction doesn't read the segment register if it uses a memory operand
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IgnoresSegment( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.ignores_segment();
}

// `true` if the opmask register is read and written (instead of just read). This also implies that it can't be `K0`.
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IsOpMaskReadWrite( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.is_op_mask_read_write();
}

// `true` if it can be executed in real mode
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_RealMode( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.real_mode();
}

// `true` if it can be executed in protected mode
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_ProtectedMode( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.protected_mode();
}

// `true` if it can be executed in virtual 8086 mode
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_Virtual8086Mode( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.virtual8086_mode();
}

// `true` if it can be executed in compatibility mode
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_CompatibilityMode( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.compatibility_mode();
}

// `true` if it can be executed in 64-bit mode
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_LongMode( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.long_mode();
}

// `true` if it can be used outside SMM
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_UseOutsideSmm( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.use_outside_smm();
}

// `true` if it can be used in SMM
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_UseInSmm( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.use_in_smm();
}

// `true` if it can be used outside an enclave (SGX)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_UseOutsideEnclaveSgx( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.use_outside_enclave_sgx();
}

// `true` if it can be used inside an enclave (SGX1)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_UseInEnclaveSgx1( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.use_in_enclave_sgx1();
}

// `true` if it can be used inside an enclave (SGX2)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_UseInEnclaveSgx2( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.use_in_enclave_sgx2();
}

// `true` if it can be used outside VMX operation
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_UseOutsideVmxOp( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.use_outside_vmx_op();
}

// `true` if it can be used in VMX root operation
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_UseInVmxRootOp( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.use_in_vmx_root_op();
}

// `true` if it can be used in VMX non-root operation
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_UseInVmxNonRootOp( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.use_in_vmx_non_root_op();
}

// `true` if it can be used outside SEAM
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_UseOutsideSeam( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.use_outside_seam();
}

// `true` if it can be used in SEAM
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_UseInSeam( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.use_in_seam();
}

// `true` if #UD is generated in TDX non-root operation
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_TdxNonRootGenUd( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.tdx_non_root_gen_ud();
}

// `true` if #VE is generated in TDX non-root operation
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_TdxNonRootGenVe( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.tdx_non_root_gen_ve();
}

// `true` if an exception (eg. #GP(0), #VE) may be generated in TDX non-root operation
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_TdxNonRootMayGenEx( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.tdx_non_root_may_gen_ex();
}

// (Intel VMX) `true` if it causes a VM exit in VMX non-root operation
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IntelVMExit( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.intel_vm_exit();
}

// (Intel VMX) `true` if it may cause a VM exit in VMX non-root operation
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IntelMayVMExit( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.intel_may_vm_exit();
}

// (Intel VMX) `true` if it causes an SMM VM exit in VMX root operation (if dual-monitor treatment is activated)
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IntelSmmVMExit( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.intel_smm_vm_exit();
}

// (AMD SVM) `true` if it causes a #VMEXIT in guest mode
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_AmdVMExit( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.amd_vm_exit();
}

// (AMD SVM) `true` if it may cause a #VMEXIT in guest mode
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_AmdMayVMExit( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.amd_may_vm_exit();
}

// `true` if it causes a TSX abort inside a TSX transaction
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_TsxAbort( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.tsx_abort();
}

// `true` if it causes a TSX abort inside a TSX transaction depending on the implementation
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_TsxImplAbort( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.tsx_impl_abort();
}

// `true` if it may cause a TSX abort inside a TSX transaction depending on some condition
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_TsxMayAbort( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.tsx_may_abort();
}

// `true` if it's decoded by iced's 16-bit Intel decoder
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IntelDecoder16( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.intel_decoder16();
}

// `true` if it's decoded by iced's 32-bit Intel decoder
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IntelDecoder32( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.intel_decoder32();
}

// `true` if it's decoded by iced's 64-bit Intel decoder
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_IntelDecoder64( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.intel_decoder64();
}

// `true` if it's decoded by iced's 16-bit AMD decoder
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_AmdDecoder16( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.amd_decoder16();
}

// `true` if it's decoded by iced's 32-bit AMD decoder
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_AmdDecoder32( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.amd_decoder32();
}

// `true` if it's decoded by iced's 64-bit AMD decoder
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_AmdDecoder64( Code : u16 ) -> bool {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.amd_decoder64();
}

// Gets the decoder option that's needed to decode the instruction or [`DecoderOptions::NONE`].
// The return value is a [`DecoderOptions`] value.
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_DecoderOption( Code : u16 ) -> u32 {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.decoder_option();
}

// Gets the length of the opcode bytes ([`op_code()`]). The low bytes is the opcode value.
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_OpCodeLen( Code : u16 ) -> u32 {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.op_code_len();
}

// Gets the number of operands
#[no_mangle]
pub unsafe extern "C" fn OpCodeInfo_OPCount( Code : u16 ) -> u32 {
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

    return info.op_count();
}