/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86::{Instruction, FpuStackIncrementInfo, OpKind};

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

// Gets the number of bytes added to `SP`/`ESP`/`RSP` or 0 if it's not an instruction that pushes or pops data. This method assumes
// the instruction doesn't change the privilege level (eg. `IRET/D/Q`). If it's the `LEAVE` instruction, this method returns 0.
#[no_mangle]
pub unsafe extern "C" fn Instruction_StackPointerIncrement( Instruction: *mut Instruction ) -> i32 { 
    if Instruction.is_null() {
        return 0;
    }

    return (*Instruction).stack_pointer_increment();
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

// Gets all op kinds ([`op_count()`] values)
#[allow( non_upper_case_globals )]
pub const OPKindsMaxEntries : usize = 5;
//#[repr(C)]
#[repr(packed)]
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

    return (*Instruction).memory_size() as u8;
}

// Gets the operand count. An instruction can have 0-5 operands.
#[no_mangle]
pub unsafe extern "C" fn Instruction_OPCount( Instruction: *mut Instruction ) -> u32 { 
    if Instruction.is_null() {
        return 0;
    }

    return (*Instruction).op_count();
}

// `true` if eviction hint bit is set (`{eh}`) (MVEX instructions only)
#[no_mangle]
pub unsafe extern "C" fn Instruction_IsMvexEvictionHint( Instruction: *mut Instruction ) -> bool { 
    if Instruction.is_null() {
        return false;
    }

    return (*Instruction).is_mvex_eviction_hint();
}

// (MVEX) Register/memory operand conversion function
#[no_mangle]
pub unsafe extern "C" fn Instruction_MvexRegMemConv( Instruction: *mut Instruction ) -> u8 { 
    if Instruction.is_null() {
        return 0;
    }

    return (*Instruction).mvex_reg_mem_conv() as u8;
}