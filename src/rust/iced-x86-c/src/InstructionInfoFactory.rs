/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86::{Instruction, InstructionInfoFactory, OpAccess};

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
//#[repr(C)]
#[repr(packed)]
pub struct TUsedRegister {
	register: u8/*Register*/,
	access: u8/*OpAccess*/,
}
//#[repr(C)]
#[repr(packed)]
pub struct TUsedRegisterArray { 
    Entries : [TUsedRegister;UsedRegisterMaxEntries], 
    Count : u8
}

//#[repr(C)]
#[repr(packed)]
pub struct TUsedMemory {
	displacement: u64,
	segment: u8/*Register*/,
	base: u8/*Register*/,
	index: u8/*Register*/,
	scale: u8,
	memory_size: u8/*MemorySize*/,
	access: u8/*OpAccess*/,
	address_size: u8/*CodeSize*/,
	vsib_size: u8,
}

#[allow( non_upper_case_globals )]
pub const UsedMemoryMaxEntries : usize = 255;
//#[repr(C)]
#[repr(packed)]
pub struct TUsedMemoryArray { 
    Entries : [TUsedMemory;UsedMemoryMaxEntries], 
    Count : u8
}

//#[repr(C)]
#[repr(packed)]
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
        if i < UsedRegisterMaxEntries {
            (*InstructionInfo).used_registers.Entries[ i ].register = x.register() as u8;
            (*InstructionInfo).used_registers.Entries[ i ].access = x.access() as u8;
        }
    }

    let usedmemoryA = value.used_memory();
    (*InstructionInfo).used_memory_locations.Count = usedmemoryA.len() as u8;
    for ( i, x ) in usedmemoryA.iter().enumerate() {
        if i < UsedMemoryMaxEntries { 
            (*InstructionInfo).used_memory_locations.Entries[ i ].displacement = x.displacement();
            (*InstructionInfo).used_memory_locations.Entries[ i ].segment = x.segment() as u8;
            (*InstructionInfo).used_memory_locations.Entries[ i ].base = x.base() as u8;
            (*InstructionInfo).used_memory_locations.Entries[ i ].index = x.index() as u8;
            (*InstructionInfo).used_memory_locations.Entries[ i ].scale = x.scale() as u8;
            (*InstructionInfo).used_memory_locations.Entries[ i ].memory_size = x.memory_size() as u8;
            (*InstructionInfo).used_memory_locations.Entries[ i ].access = x.access() as u8;
            (*InstructionInfo).used_memory_locations.Entries[ i ].address_size = x.address_size() as u8;
            (*InstructionInfo).used_memory_locations.Entries[ i ].vsib_size = x.vsib_size() as u8;
        }
    }
    
    (*InstructionInfo).op_accesses[ 0 ] = value.op0_access();
    (*InstructionInfo).op_accesses[ 1 ] = value.op1_access();
    (*InstructionInfo).op_accesses[ 2 ] = value.op2_access();
    (*InstructionInfo).op_accesses[ 3 ] = value.op3_access();
    (*InstructionInfo).op_accesses[ 4 ] = value.op4_access();

    return true;
}