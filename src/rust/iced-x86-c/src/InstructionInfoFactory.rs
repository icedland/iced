/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86::{Instruction, InstructionInfoFactory, UsedRegister, UsedMemory, OpAccess};

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// InstructionInfoFactory

// Creates a new instance.
//
// If you don't need to know register and memory usage, it's faster to call [`Instruction`] and
// [`Code`] methods such as [`Instruction::flow_control()`] instead of getting that info from this struct.
//
// [`Instruction`]: struct.Instruction.html
// [`Code`]: enum.Code.html
// [`Instruction::flow_control()`]: struct.Instruction.html#method.flow_control
#[no_mangle]
pub extern "C" fn InstructionInfoFactory_Create() -> *mut InstructionInfoFactory {
    return Box::into_raw( Box::new( InstructionInfoFactory::new() ) );
}

// Creates a new [`InstructionInfo`], see also [`info()`].
//
// If you don't need to know register and memory usage, it's faster to call [`Instruction`] and
// [`Code`] methods such as [`Instruction::flow_control()`] instead of getting that info from this struct.
#[allow( non_upper_case_globals )]
pub const UsedRegisterMaxEntries : usize = 100;
#[repr(C)]
pub struct TUsedRegisterArray { 
    Entries : [UsedRegister;UsedRegisterMaxEntries], 
    Count : u8
}

#[allow( non_upper_case_globals )]
pub const UsedMemoryMaxEntries : usize = 255;
#[repr(C)]
pub struct TUsedMemoryArray { 
    Entries : [UsedMemory;UsedMemoryMaxEntries], 
    Count : u8
}

#[repr(C)]
pub struct TInstructionInfo {
	used_registers: TUsedRegisterArray,
	used_memory_locations: TUsedMemoryArray,
	op_accesses: [OpAccess;5/*IcedConstants::MAX_OP_COUNT*/]
}

#[no_mangle]
pub unsafe extern "C" fn InstructionInfoFactory_Info( InstructionInfoFactory: *mut InstructionInfoFactory, Instruction: *mut Instruction, InstructionInfo: *mut TInstructionInfo, Options: u32/*InstructionInfoOptions*/ ) -> bool { 
    if InstructionInfoFactory.is_null() {
        return false;
    }
    if Instruction.is_null() {
        return false;
    }
    if InstructionInfo.is_null() {
        return false;
    }

    let value = &*(*InstructionInfoFactory).info_options( &(*Instruction), Options );

    let usedregistersA = value.used_registers();
    (*InstructionInfo).used_registers.Count = usedregistersA.len() as u8;
    for ( i, x ) in usedregistersA.iter().enumerate() {
        if i < (*InstructionInfo).used_registers.Entries.len() {
            (*InstructionInfo).used_registers.Entries[ i ] = *x;
        }
    }

    let usedmemoryA = value.used_memory();
    (*InstructionInfo).used_memory_locations.Count = usedmemoryA.len() as u8;
    for ( i, x ) in usedmemoryA.iter().enumerate() {
        if i < (*InstructionInfo).used_memory_locations.Entries.len() { 
            (*InstructionInfo).used_memory_locations.Entries[ i ] = *x;
        }
    }
    
    (*InstructionInfo).op_accesses[ 0 ] = value.op0_access();
    (*InstructionInfo).op_accesses[ 1 ] = value.op1_access();
    (*InstructionInfo).op_accesses[ 2 ] = value.op2_access();
    (*InstructionInfo).op_accesses[ 3 ] = value.op3_access();
    (*InstructionInfo).op_accesses[ 4 ] = value.op4_access();

    return true;
}