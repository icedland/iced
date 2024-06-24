/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86::{Instruction, FpuStackIncrementInfo, CpuidFeature, OpKind, OpCodeOperandKind};
use std::mem::transmute;// Enum<->Int

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Instruction

// Gets the FPU status word's `TOP` increment and whether it's a conditional or unconditional push/pop
// and whether `TOP` is written.
#[no_mangle]
pub unsafe extern "C" fn Instruction_FPU_StackIncrementInfo( Instruction: *mut Instruction, Info: *mut FpuStackIncrementInfo ) -> bool { 
    if Instruction.is_null() {
        return false;
    }
    if Info.is_null() {
        return false;
    }
    *Info = (*Instruction).fpu_stack_increment_info();

    return true;
}

// Instruction encoding, eg. Legacy, 3DNow!, VEX, EVEX, XOP
#[no_mangle]
pub unsafe extern "C" fn Instruction_Encoding( Instruction: *mut Instruction ) -> u32 { // FFI-Unsafe: EncodingKind 
    if Instruction.is_null() {
        return 0;// EncodingKind::Legacy;
    }
    
    return transmute( (*Instruction).encoding() as u32 );
}

// Gets the mnemonic, see also [`code()`]
#[no_mangle]
pub unsafe extern "C" fn Instruction_Mnemonic( Instruction: *mut Instruction ) -> u32 { // FFI-Unsafe: Mnemonic
    if Instruction.is_null() {
        return 0;// Mnemonic::INVALID;
    }
    
    return transmute( (*Instruction).mnemonic() as u32 );
}

// `true` if this is an instruction that implicitly uses the stack pointer (`SP`/`ESP`/`RSP`), eg. `CALL`, `PUSH`, `POP`, `RET`, etc.
// See also [`stack_pointer_increment()`]
//
// [`stack_pointer_increment()`]: #method.stack_pointer_increment
#[no_mangle]
pub unsafe extern "C" fn Instruction_IsStackInstruction( Instruction: *mut Instruction ) -> bool { 
    if Instruction.is_null() {
        return false;
    }

    return (*Instruction).is_stack_instruction();
}

// Gets the number of bytes added to `SP`/`ESP`/`RSP` or 0 if it's not an instruction that pushes or pops data. This method assumes
// the instruction doesn't change the privilege level (eg. `IRET/D/Q`). If it's the `LEAVE` instruction, this method returns 0.
#[no_mangle]
pub unsafe extern "C" fn Instruction_StackPointerIncrement( Instruction: *mut Instruction ) -> i32 { 
    if Instruction.is_null() {
        return 0;
    }

    return (*Instruction).stack_pointer_increment();
}

// Gets the condition code if it's `Jcc`, `SETcc`, `CMOVcc`, `LOOPcc` else [`ConditionCode::None`] is returned
//
// [`ConditionCode::None`]: enum.ConditionCode.html#variant.None
#[no_mangle]
pub unsafe extern "C" fn Instruction_ConditionCode( Instruction: *mut Instruction ) -> u32 { // FFI-Unsafe: ConditionCode
    if Instruction.is_null() {
        return 0;// ConditionCode::None;
    }

    return transmute( (*Instruction).condition_code() as u32 );
}

// All flags that are read by the CPU when executing the instruction.
// This method returns an [`RflagsBits`] value. See also [`rflags_modified()`].
#[no_mangle]
pub unsafe extern "C" fn Instruction_RFlagsRead( Instruction: *mut Instruction ) -> u32 { 
    if Instruction.is_null() {
        return 0;
    }

    return (*Instruction).rflags_read();
}

// All flags that are written by the CPU, except those flags that are known to be undefined, always set or always cleared.
// This method returns an [`RflagsBits`] value. See also [`rflags_modified()`].
#[no_mangle]
pub unsafe extern "C" fn Instruction_RFlagsWritten( Instruction: *mut Instruction ) -> u32 { 
    if Instruction.is_null() {
        return 0;
    }

    return (*Instruction).rflags_written();
}

// All flags that are always cleared by the CPU.
// This method returns an [`RflagsBits`] value. See also [`rflags_modified()`].
#[no_mangle]
pub unsafe extern "C" fn Instruction_RFlagsCleared( Instruction: *mut Instruction ) -> u32 { 
    if Instruction.is_null() {
        return 0;
    }

    return (*Instruction).rflags_cleared();
}

// All flags that are always set by the CPU.
// This method returns an [`RflagsBits`] value. See also [`rflags_modified()`].
#[no_mangle]
pub unsafe extern "C" fn Instruction_RFlagsSet( Instruction: *mut Instruction ) -> u32 { 
    if Instruction.is_null() {
        return 0;
    }

    return (*Instruction).rflags_set();
}

// All flags that are undefined after executing the instruction.
// This method returns an [`RflagsBits`] value. See also [`rflags_modified()`].
#[no_mangle]
pub unsafe extern "C" fn Instruction_RFlagsUndefined( Instruction: *mut Instruction ) -> u32 { 
    if Instruction.is_null() {
        return 0;
    }

    return (*Instruction).rflags_undefined();
}

// All flags that are modified by the CPU. This is `rflags_written() + rflags_cleared() + rflags_set() + rflags_undefined()`. This method returns an [`RflagsBits`] value.
#[no_mangle]
pub unsafe extern "C" fn Instruction_RFlagsModified( Instruction: *mut Instruction ) -> u32 { 
    if Instruction.is_null() {
        return 0;
    }

    return (*Instruction).rflags_modified();
}

// Control flow info
#[no_mangle]
pub unsafe extern "C" fn Instruction_FlowControl( Instruction: *mut Instruction ) -> u32 { // FFI-Unsafe: FlowControl
    if Instruction.is_null() {
        return 0;// FlowControl::Next;
    }

    return transmute( (*Instruction).flow_control() as u32 );
}

// Gets the CPU or CPUID feature flags
#[allow( non_upper_case_globals )]
pub const CPUIDFeaturesMaxEntries : usize = 5;
#[repr(C)]
pub struct TCPUIDFeaturesArray { 
    Entries : [CpuidFeature;CPUIDFeaturesMaxEntries], 
    Count : u8
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_CPUIDFeatures( Instruction: *mut Instruction, CPUIDFeatures : *mut TCPUIDFeaturesArray ) -> bool { 
    if Instruction.is_null() {
        return false;
    }
    if CPUIDFeatures.is_null() {
        return false;
    }

    let cpuidfeaturesA = (*Instruction).cpuid_features();

    (*CPUIDFeatures).Count = cpuidfeaturesA.len() as u8;
    for ( i, x ) in cpuidfeaturesA.iter().enumerate() {
        if i < (*CPUIDFeatures).Entries.len() {
            (*CPUIDFeatures).Entries[ i ] = *x;
        }
    }

    return true;
}

// Gets all op kinds ([`op_count()`] values)
#[allow( non_upper_case_globals )]
pub const OPKindsMaxEntries : usize = 5;
#[repr(C)]
pub struct TOPKindsArray { 
    Entries : [OpKind;OPKindsMaxEntries], 
    Count : u8
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_OPKinds( Instruction: *mut Instruction, OPKinds : *mut TOPKindsArray ) -> bool { 
    if Instruction.is_null() {
        return false;
    }
    if OPKinds.is_null() {
        return false;
    }

    let opkindsA = (*Instruction).op_kinds();

    (*OPKinds).Count = opkindsA.len() as u8;
    for ( i, x ) in opkindsA.enumerate() {
        if i < (*OPKinds).Entries.len() {
            (*OPKinds).Entries[ i ] = x;
        }
    }

    return true;
}

// Gets the size of the memory location that is referenced by the operand. See also [`is_broadcast()`].
// Use this method if the operand has kind [`OpKind::Memory`],
#[no_mangle]
pub unsafe extern "C" fn Instruction_MemorySize( Instruction: *mut Instruction ) -> u8 { 
    if Instruction.is_null() {
        return 0;
    }

    return (*Instruction).memory_size().size() as u8;
}

// Gets the operand count. An instruction can have 0-5 operands.
#[no_mangle]
pub unsafe extern "C" fn Instruction_OPCount( Instruction: *mut Instruction ) -> u32 { 
    if Instruction.is_null() {
        return 0;
    }

    return (*Instruction).op_count();
}

// Gets the code
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_Code( Instruction: *mut Instruction ) -> u32 { // FFI Unsafe: Code
    if Instruction.is_null() {
        return 0;
    }

    return (*Instruction).op_code().code() as u32;
}

// Gets the mnemonic
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_Mnemonic( Instruction: *mut Instruction ) -> u32 { // FFI Unsafe: Mnemonic
    if Instruction.is_null() {
        return 0;
    }

    return (*Instruction).op_code().mnemonic() as u32;
}

// Gets the encoding
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_Encoding( Instruction: *mut Instruction ) -> u32 { // FFI Unsafe: Encoding
    if Instruction.is_null() {
        return 0;
    }

    return (*Instruction).op_code().encoding() as u32;
}

// `true` if it's an instruction, `false` if it's eg. [`Code::INVALID`], [`db`], [`dw`], [`dd`], [`dq`], [`zero_bytes`]
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IsInstruction( Instruction: *mut Instruction ) -> bool { 
    if Instruction.is_null() {
        return false;
    }

    return (*Instruction).op_code().is_instruction();
}

// `true` if it's an instruction available in 16-bit mode
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_Mode16( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().mode16();
}

// `true` if it's an instruction available in 32-bit mode
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_Mode32( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().mode32();
}

// `true` if it's an instruction available in 64-bit mode
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_Mode64( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().mode64();
}

// `true` if an `FWAIT` (`9B`) instruction is added before the instruction
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_Fwait( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().fwait();
}

// (Legacy encoding) Gets the required operand size (16,32,64) or 0
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_OperandSize( Instruction: *mut Instruction ) -> u32 {
  if Instruction.is_null() {
      return 0;
  }

  return (*Instruction).op_code().operand_size();
}

// (Legacy encoding) Gets the required address size (16,32,64) or 0
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_AddressSize( Instruction: *mut Instruction ) -> u32 {
  if Instruction.is_null() {
      return 0;
  }

  return (*Instruction).op_code().address_size();
}

// (VEX/XOP/EVEX) `L` / `L'L` value or default value if [`is_lig()`] is `true`
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_L( Instruction: *mut Instruction ) -> u32 {
  if Instruction.is_null() {
      return 0;
  }

  return (*Instruction).op_code().l();
}

// (VEX/XOP/EVEX/MVEX) `W` value or default value if [`is_wig()`] or [`is_wig32()`] is `true`
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_W( Instruction: *mut Instruction ) -> u32 {
  if Instruction.is_null() {
      return 0;
  }

  return (*Instruction).op_code().w();
}

// (VEX/XOP/EVEX) `true` if the `L` / `L'L` fields are ignored.
//
// EVEX: if reg-only ops and `{er}` (`EVEX.b` is set), `L'L` is the rounding control and not ignored.
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IsLig( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().is_lig();
}

// (VEX/XOP/EVEX/MVEX) `true` if the `W` field is ignored in 16/32/64-bit modes
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IsWig( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().is_wig();
}

// (VEX/XOP/EVEX/MVEX) `true` if the `W` field is ignored in 16/32-bit modes (but not 64-bit mode)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IsWig32( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().is_wig32();
}

// (EVEX/MVEX) Gets the tuple type
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_TupleType( Instruction: *mut Instruction ) -> u32 { // FFI Unsafe: TupleType
  if Instruction.is_null() {
      return 0;// TupleType::N1;
  }

  return (*Instruction).op_code().tuple_type() as u32;
}

// (MVEX) Gets the `EH` bit that's required to encode this instruction
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_MvexEhBit( Instruction: *mut Instruction ) -> u32 { // FFI Unsafe: MvexEHBit
  if Instruction.is_null() {
      return 0;// MvexEHBit::None;
  }

  return (*Instruction).op_code().mvex_eh_bit() as u32;
}

// (MVEX) `true` if the instruction supports eviction hint (if it has a memory operand)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_MvexCanUseEvictionHint( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().mvex_can_use_eviction_hint();
}

// (MVEX) `true` if the instruction's rounding control bits are stored in `imm8[1:0]`
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_MvexCanUseImmRoundingControl( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().mvex_can_use_imm_rounding_control();
}

// (MVEX) `true` if the instruction ignores op mask registers (eg. `{k1}`)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_MvexIgnoresOpMaskRegister( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().mvex_ignores_op_mask_register();
}

// (MVEX) `true` if the instruction must have `MVEX.SSS=000` if `MVEX.EH=1`
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_MvexNoSaeRc( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().mvex_no_sae_rc();
}

// (MVEX) Gets the tuple type / conv lut kind
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_MvexTupleTypeLutKind( Instruction: *mut Instruction ) -> u32 { // FFI Unsafe: MvexTupleTypeLutKind
  if Instruction.is_null() {
      return 0;// MvexTupleTypeLutKind::Int32;
  }

  return (*Instruction).op_code().mvex_tuple_type_lut_kind() as u32;
}

// (MVEX) Gets the conversion function, eg. `Sf32`
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_MvexConversionFunc( Instruction: *mut Instruction ) -> u32 { // FFI Unsafe: MvexConvFn
  if Instruction.is_null() {
      return 0;// MvexConvFn::None;
  }

  return (*Instruction).op_code().mvex_conversion_func() as u32;
}

// (MVEX) Gets flags indicating which conversion functions are valid (bit 0 == func 0)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_MvexValidConversionFuncsMask( Instruction: *mut Instruction ) -> u8 {
  if Instruction.is_null() {
      return 0;
  }

  return (*Instruction).op_code().mvex_valid_conversion_funcs_mask();
}

// (MVEX) Gets flags indicating which swizzle functions are valid (bit 0 == func 0)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_MvexValidSwizzleFuncsMask( Instruction: *mut Instruction ) -> u8 {
  if Instruction.is_null() {
      return 0;
  }

  return (*Instruction).op_code().mvex_valid_swizzle_funcs_mask();
}

// If it has a memory operand, gets the [`MemorySize`] (non-broadcast memory type)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_MemorySize( Instruction: *mut Instruction ) -> u32 { // FFI Unsafe: MemorySize
  if Instruction.is_null() {
      return 0;// MemorySize::Unknown;
  }

  return (*Instruction).op_code().memory_size() as u32;
}

// If it has a memory operand, gets the [`MemorySize`] (broadcast memory type)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_BroadcastMemorySize( Instruction: *mut Instruction ) -> u32 { // FFI Unsafe: MemorySize
  if Instruction.is_null() {
      return 0;// MemorySize::Unknown;
  }

  return (*Instruction).op_code().broadcast_memory_size() as u32;
}

// (EVEX) `true` if the instruction supports broadcasting (`EVEX.b` bit) (if it has a memory operand)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_CanBroadcast( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().can_broadcast();
}

// (EVEX/MVEX) `true` if the instruction supports rounding control
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_CanUseRoundingControl( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().can_use_rounding_control();
}

// (EVEX/MVEX) `true` if the instruction supports suppress all exceptions
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_CanSuppressAllExceptions( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().can_suppress_all_exceptions();
}

// (EVEX/MVEX) `true` if an opmask register can be used
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_CanUseOpMaskRegister( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().can_use_op_mask_register();
}

// (EVEX/MVEX) `true` if a non-zero opmask register must be used
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_RequireOpMaskRegister( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().require_op_mask_register();
}

// (EVEX) `true` if the instruction supports zeroing masking (if one of the opmask registers `K1`-`K7` is used and destination operand is not a memory operand)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_CanUseZeroingMasking( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().can_use_zeroing_masking();
}

// `true` if the `LOCK` (`F0`) prefix can be used
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_CanUseLockPrefix( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().can_use_lock_prefix();
}

// `true` if the `XACQUIRE` (`F2`) prefix can be used
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_CanUseXacquirePrefix( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().can_use_xacquire_prefix();
}

// `true` if the `XRELEASE` (`F3`) prefix can be used
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_CanUseXreleasePrefix( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().can_use_xrelease_prefix();
}

// `true` if the `REP` / `REPE` (`F3`) prefixes can be used
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_CanUseRepPrefix( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().can_use_rep_prefix();
}

// `true` if the `REPNE` (`F2`) prefix can be used
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_CanUseRepnePrefix( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().can_use_repne_prefix();
}

// `true` if the `BND` (`F2`) prefix can be used
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_CanUseBndPrefix( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().can_use_bnd_prefix();
}

// `true` if the `HINT-TAKEN` (`3E`) and `HINT-NOT-TAKEN` (`2E`) prefixes can be used
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_CanUseHintTakenPrefix( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().can_use_hint_taken_prefix();
}

// `true` if the `NOTRACK` (`3E`) prefix can be used
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_CanUseNotrackPrefix( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().can_use_notrack_prefix();
}

// `true` if rounding control is ignored (#UD is not generated)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IgnoresRoundingControl( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().ignores_rounding_control();
}

// `true` if the `LOCK` prefix can be used as an extra register bit (bit 3) to access registers 8-15 without a `REX` prefix (eg. in 32-bit mode)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_AmdLockRegBit( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().amd_lock_reg_bit();
}

// `true` if the default operand size is 64 in 64-bit mode. A `66` prefix can switch to 16-bit operand size.
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_DefaultOpSize64( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().default_op_size64();
}

// `true` if the operand size is always 64 in 64-bit mode. A `66` prefix is ignored.
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_ForceOpSize64( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().force_op_size64();
}

// `true` if the Intel decoder forces 64-bit operand size. A `66` prefix is ignored.
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IntelForceOpSize64( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().intel_force_op_size64();
}

// `true` if it can only be executed when CPL=0
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_MustBeCpl0( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().must_be_cpl0();
}

// `true` if it can be executed when CPL=0
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_Cpl0( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().cpl0();
}

// `true` if it can be executed when CPL=1
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_Cpl1( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().cpl1();
}

// `true` if it can be executed when CPL=2
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_Cpl2( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().cpl2();
}

// `true` if it can be executed when CPL=3
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_Cpl3( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().cpl3();
}

// `true` if the instruction accesses the I/O address space (eg. `IN`, `OUT`, `INS`, `OUTS`)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IsInputOutput( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().is_input_output();
}

// `true` if it's one of the many nop instructions (does not include FPU nop instructions, eg. `FNOP`)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IsNop( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().is_nop();
}

// `true` if it's one of the many reserved nop instructions (eg. `0F0D`, `0F18-0F1F`)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IsReservedNop( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().is_reserved_nop();
}

// `true` if it's a serializing instruction (Intel CPUs)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IsSerializingIntel( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().is_serializing_intel();
}

// `true` if it's a serializing instruction (AMD CPUs)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IsSerializingAmd( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().is_serializing_amd();
}

// `true` if the instruction requires either CPL=0 or CPL<=3 depending on some CPU option (eg. `CR4.TSD`, `CR4.PCE`, `CR4.UMIP`)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_MayRequireCpl0( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().may_require_cpl0();
}

// `true` if it's a tracked `JMP`/`CALL` indirect instruction (CET)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IsCetTracked( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().is_cet_tracked();
}

// `true` if it's a non-temporal hint memory access (eg. `MOVNTDQ`)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IsNonTemporal( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().is_non_temporal();
}

// `true` if it's a no-wait FPU instruction, eg. `FNINIT`
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IsFpuNoWait( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().is_fpu_no_wait();
}

// `true` if the mod bits are ignored and it's assumed `modrm[7:6] == 11b`
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IgnoresModBits( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().ignores_mod_bits();
}

// `true` if the `66` prefix is not allowed (it will #UD)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_No66( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().no66();
}

// `true` if the `F2`/`F3` prefixes aren't allowed
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_Nfx( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().nfx();
}

// `true` if the index reg's reg-num (vsib op) (if any) and register ops' reg-nums must be unique,
// eg. `MNEMONIC XMM1,YMM1,[RAX+ZMM1*2]` is invalid. Registers = `XMM`/`YMM`/`ZMM`/`TMM`.
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_RequiresUniqueRegNums( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().requires_unique_reg_nums();
}

// `true` if the destination register's reg-num must not be present in any other operand, eg. `MNEMONIC XMM1,YMM1,[RAX+ZMM1*2]`
// is invalid. Registers = `XMM`/`YMM`/`ZMM`/`TMM`.
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_RequiresUniqueDestRegNum( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().requires_unique_dest_reg_num();
}

// `true` if it's a privileged instruction (all CPL=0 instructions (except `VMCALL`) and IOPL instructions `IN`, `INS`, `OUT`, `OUTS`, `CLI`, `STI`)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IsPrivileged( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().is_privileged();
}

// `true` if it reads/writes too many registers
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IsSaveRestore( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().is_save_restore();
}

// `true` if it's an instruction that implicitly uses the stack register, eg. `CALL`, `POP`, etc
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IsStackInstruction( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().is_stack_instruction();
}

// `true` if the instruction doesn't read the segment register if it uses a memory operand
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IgnoresSegment( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().ignores_segment();
}

// `true` if the opmask register is read and written (instead of just read). This also implies that it can't be `K0`.
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IsOpMaskReadWrite( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().is_op_mask_read_write();
}

// `true` if it can be executed in real mode
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_RealMode( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().real_mode();
}

// `true` if it can be executed in protected mode
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_ProtectedMode( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().protected_mode();
}

// `true` if it can be executed in virtual 8086 mode
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_Virtual8086Mode( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().virtual8086_mode();
}

// `true` if it can be executed in compatibility mode
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_CompatibilityMode( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().compatibility_mode();
}

// `true` if it can be executed in 64-bit mode
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_LongMode( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().long_mode();
}

// `true` if it can be used outside SMM
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_UseOutsideSmm( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().use_outside_smm();
}

// `true` if it can be used in SMM
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_UseInSmm( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().use_in_smm();
}

// `true` if it can be used outside an enclave (SGX)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_UseOutsideEnclaveSgx( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().use_outside_enclave_sgx();
}

// `true` if it can be used inside an enclave (SGX1)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_UseInEnclaveSgx1( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().use_in_enclave_sgx1();
}

// `true` if it can be used inside an enclave (SGX2)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_UseInEnclaveSgx2( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().use_in_enclave_sgx2();
}

// `true` if it can be used outside VMX operation
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_UseOutsideVmxOp( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().use_outside_vmx_op();
}

// `true` if it can be used in VMX root operation
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_UseInVmxRootOp( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().use_in_vmx_root_op();
}

// `true` if it can be used in VMX non-root operation
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_UseInVmxNonRootOp( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().use_in_vmx_non_root_op();
}

// `true` if it can be used outside SEAM
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_UseOutsideSeam( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().use_outside_seam();
}

// `true` if it can be used in SEAM
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_UseInSeam( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().use_in_seam();
}

// `true` if #UD is generated in TDX non-root operation
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_TdxNonRootGenUd( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().tdx_non_root_gen_ud();
}

// `true` if #VE is generated in TDX non-root operation
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_TdxNonRootGenVe( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().tdx_non_root_gen_ve();
}

// `true` if an exception (eg. #GP(0), #VE) may be generated in TDX non-root operation
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_TdxNonRootMayGenEx( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().tdx_non_root_may_gen_ex();
}

// (Intel VMX) `true` if it causes a VM exit in VMX non-root operation
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IntelVMExit( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().intel_vm_exit();
}

// (Intel VMX) `true` if it may cause a VM exit in VMX non-root operation
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IntelMayVMExit( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().intel_may_vm_exit();
}

// (Intel VMX) `true` if it causes an SMM VM exit in VMX root operation (if dual-monitor treatment is activated)
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IntelSmmVMExit( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().intel_smm_vm_exit();
}

// (AMD SVM) `true` if it causes a #VMEXIT in guest mode
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_AmdVMExit( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().amd_vm_exit();
}

// (AMD SVM) `true` if it may cause a #VMEXIT in guest mode
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_AmdMayVMExit( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().amd_may_vm_exit();
}

// `true` if it causes a TSX abort inside a TSX transaction
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_TsxAbort( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().tsx_abort();
}

// `true` if it causes a TSX abort inside a TSX transaction depending on the implementation
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_TsxImplAbort( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().tsx_impl_abort();
}

// `true` if it may cause a TSX abort inside a TSX transaction depending on some condition
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_TsxMayAbort( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().tsx_may_abort();
}

// `true` if it's decoded by iced's 16-bit Intel decoder
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IntelDecoder16( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().intel_decoder16();
}

// `true` if it's decoded by iced's 32-bit Intel decoder
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IntelDecoder32( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().intel_decoder32();
}

// `true` if it's decoded by iced's 64-bit Intel decoder
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IntelDecoder64( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().intel_decoder64();
}

// `true` if it's decoded by iced's 16-bit AMD decoder
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_AmdDecoder16( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().amd_decoder16();
}

// `true` if it's decoded by iced's 32-bit AMD decoder
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_AmdDecoder32( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().amd_decoder32();
}

// `true` if it's decoded by iced's 64-bit AMD decoder
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_AmdDecoder64( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().amd_decoder64();
}

// Gets the decoder option that's needed to decode the instruction or [`DecoderOptions::NONE`].
// The return value is a [`DecoderOptions`] value.
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_DecoderOption( Instruction: *mut Instruction ) -> u32 {
  if Instruction.is_null() {
      return 0;
  }

  return (*Instruction).op_code().decoder_option();
}

// Gets the opcode table
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_Table( Instruction: *mut Instruction ) -> u32 { // FFI Unsafe: OpCodeTableKind 
  if Instruction.is_null() {
      return 0;// OpCodeTableKind::Normal;
  }

  return (*Instruction).op_code().table() as u32;
}

// Gets the mandatory prefix
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_MandatoryPrefix( Instruction: *mut Instruction ) -> u32 { // FFI Unsafe: MandatoryPrefix 
  if Instruction.is_null() {
      return 0;// MandatoryPrefix::None;
  }

  return (*Instruction).op_code().mandatory_prefix() as u32;
}

// Gets the opcode byte(s). The low byte(s) of this value is the opcode. The length is in [`op_code_len()`].
// It doesn't include the table value, see [`table()`].
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_OpCode( Instruction: *mut Instruction ) -> u32 {
  if Instruction.is_null() {
      return 0;
  }

  return (*Instruction).op_code().op_code();
}

// Gets the length of the opcode bytes ([`op_code()`]). The low bytes is the opcode value.
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_OpCodeLen( Instruction: *mut Instruction ) -> u32 {
  if Instruction.is_null() {
      return 0;
  }

  return (*Instruction).op_code().op_code_len();
}

// `true` if it's part of a group
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IsGroup( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().is_group();
}

// Group index (0-7) or -1. If it's 0-7, it's stored in the `reg` field of the `modrm` byte.
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_GroupIndex( Instruction: *mut Instruction ) -> i32 {
  if Instruction.is_null() {
      return 0;
  }

  return (*Instruction).op_code().group_index();
}

// `true` if it's part of a modrm.rm group
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IsRMGroup( Instruction: *mut Instruction ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().is_rm_group();
}

// Group index (0-7) or -1. If it's 0-7, it's stored in the `rm` field of the `modrm` byte.
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_RMGroupIndex( Instruction: *mut Instruction ) -> i32 {
  if Instruction.is_null() {
      return 0;
  }

  return (*Instruction).op_code().rm_group_index();
}

// Gets the number of operands
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_OPCount( Instruction: *mut Instruction ) -> u32 {
  if Instruction.is_null() {
      return 0;
  }

  return (*Instruction).op_code().op_count();
}

// Gets operand #0's opkind
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_OP0Kind( Instruction: *mut Instruction ) -> u32 { // FFI Unsafe: OpCodeOperandKind 
  if Instruction.is_null() {
      return 0;// OpCodeOperandKind::None;
  }

  return (*Instruction).op_code().op0_kind() as u32;
}

// Gets operand #1's opkind
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_OP1Kind( Instruction: *mut Instruction ) -> u32 { // FFI Unsafe: OpCodeOperandKind 
  if Instruction.is_null() {
      return 0;// OpCodeOperandKind::None;
  }

  return (*Instruction).op_code().op1_kind() as u32;
}

// Gets operand #2's opkind
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_OP2Kind( Instruction: *mut Instruction ) -> u32 { // FFI Unsafe: OpCodeOperandKind 
  if Instruction.is_null() {
      return 0;// OpCodeOperandKind::None;
  }

  return (*Instruction).op_code().op2_kind() as u32;
}

// Gets operand #3's opkind
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_OP3Kind( Instruction: *mut Instruction ) -> u32 { // FFI Unsafe: OpCodeOperandKind 
  if Instruction.is_null() {
      return 0;// OpCodeOperandKind::None;
  }

  return (*Instruction).op_code().op3_kind() as u32;
}

// Gets operand #4's opkind
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_OP4Kind( Instruction: *mut Instruction ) -> u32 { // FFI Unsafe: OpCodeOperandKind 
  if Instruction.is_null() {
      return 0;// OpCodeOperandKind::None;
  }

  return (*Instruction).op_code().op4_kind() as u32;
}

// Gets an operand's opkind
//
// # Arguments
//
// * `operand`: Operand number, 0-4
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_OPKind( Instruction: *mut Instruction, operand: u32 ) -> u32 { // FFI Unsafe: OpCodeOperandKind
  if Instruction.is_null() {
      return 0;// OpCodeOperandKind::None;
  }

  return (*Instruction).op_code().op_kind( operand ) as u32;
}

// Gets all operand kinds
#[allow( non_upper_case_globals )]
pub const TOPCodeOperandKindArrayMaxEntries : usize = 5;
#[repr(C)]
pub struct TOPCodeOperandKindArray { 
    Entries : [OpCodeOperandKind;TOPCodeOperandKindArrayMaxEntries], 
    Count : u8
}

#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_OPKinds( Instruction: *mut Instruction, OPKinds : *mut TOPCodeOperandKindArray ) -> bool { 
    if Instruction.is_null() {
        return false;
    }
    if OPKinds.is_null() {
        return false;
    }

    let opkindsA = (*Instruction).op_code().op_kinds();

    (*OPKinds).Count = opkindsA.len() as u8;
    for ( i, x ) in opkindsA.iter().enumerate() {
        if i < (*OPKinds).Entries.len() {
            (*OPKinds).Entries[ i ] = *x;
        }
    }

    return true;
}

// Checks if the instruction is available in 16-bit mode, 32-bit mode or 64-bit mode
//
// # Arguments
//
// * `bitness`: 16, 32 or 64
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_IsAvailableInMode( Instruction: *mut Instruction, Bitness: u32 ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  return (*Instruction).op_code().is_available_in_mode( Bitness );
}

// Gets the opcode string, eg. `VEX.128.66.0F38.W0 78 /r`, see also [`instruction_string()`]
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_OpCodeString( Instruction: *mut Instruction, Output : *mut u8, Size : usize ) -> bool {
  if Instruction.is_null() {
      return false;
  }
  if Output.is_null() {
      return false;
  }
  if Size <= 0 {
      return false;
  }

  let output = (*Instruction).op_code().op_code_string();

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

  return true;
}

// Gets the instruction string, eg. `VPBROADCASTB xmm1, xmm2/m8`, see also [`op_code_string()`]
#[no_mangle]
pub unsafe extern "C" fn Instruction_OpCodeInfo_InstructionString( Instruction: *mut Instruction, Output : *mut u8, Size : usize ) -> bool {
  if Instruction.is_null() {
      return false;
  }

  if Output.is_null() {
    return false;
  }
  if Size <= 0 {
    return false;
  }

  let output = (*Instruction).op_code().instruction_string();

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

  return true;
}