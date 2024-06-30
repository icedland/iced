/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86::{Code, CpuidFeature, OpCodeOperandKind};
use crate::OpCodeInfo::TOpCodeInfo;
use std::mem::transmute;// Enum<->Int

#[no_mangle]
pub unsafe extern "C" fn Code_AsString( Code : u16, Output : *mut u8, Size : usize ) { // FFI-Unsafe: Code
    if Output.is_null() {
        return;
    }
    if Size <= 0 {
        return;
    }

    let code : Code = transmute( Code as u16 );
    
    let output = format!("{code:?}");
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

#[no_mangle]
pub unsafe extern "C" fn Code_Mnemonic( Code : u16 ) -> u16/*Mnemonic*/ { // FFI-Unsafe: Code, Mnemonic
    let code : Code = transmute( Code as u16 );
    code.mnemonic() as u16
}

#[no_mangle]
pub unsafe extern "C" fn Code_OPCode( Code : u16, Info : *mut TOpCodeInfo ) { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

/*
    // OpCodeString
    let mut s = info.op_code_string();
    let mut l = s.len();
    if l > (*Info).op_code_string.len() {
        l = (*Info).op_code_string.len();
    }
    
    if l > 0 {
        for i in 0..l {
            (*Info).op_code_string[ i ] = s.as_bytes()[ i ];        
        }
    }
    (*Info).op_code_string[ l ] = 0;

    // InstructionString
    s = info.instruction_string();
    let mut l = s.len();
    if l > (*Info).instruction_string.len() {
        l = (*Info).instruction_string.len();
    }
    
    if l > 0 {
        for i in 0..l {
            (*Info).instruction_string[ i ] = s.as_bytes()[ i ];        
        }
    }
    (*Info).instruction_string[ l ] = 0;    
*/    

    (*Info).code = info.code() as u16;
    (*Info).op_code = info.op_code() as u16;
    (*Info).encoding = info.encoding() as u8;
    (*Info).operand_size = info.operand_size() as u8;
    (*Info).address_size = info.address_size() as u8;
    (*Info).l = info.l() as u8;
    (*Info).tuple_type = info.tuple_type() as u8;
    (*Info).table = info.table() as u8;
    (*Info).mandatory_prefix = info.mandatory_prefix() as u8;
    (*Info).group_index = info.group_index() as i8;
    (*Info).rm_group_index = info.rm_group_index() as i8;
    for i in 0..(info.op_count() as usize) {
        (*Info).op_kinds[ i ] = info.op_kinds()[ i ] as u8;
    }         

    for i in (info.op_count() as usize)..(*Info).op_kinds.len() {
        (*Info).op_kinds[ i ] = OpCodeOperandKind::None as u8;
    }   
}

#[no_mangle]
pub unsafe extern "C" fn Code_Encoding( Code : u16 ) -> u16/*EncodingKind*/ { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );

    code.encoding() as u16
}

// Gets the CPU or CPUID feature flags
#[allow( non_upper_case_globals )]
pub const CPUIDFeaturesMaxEntries : usize = 5;
//#[repr(C)]
#[repr(packed)]
pub struct TCPUIDFeaturesArray { 
    pub Entries : [CpuidFeature;CPUIDFeaturesMaxEntries], 
    pub Count : u8
}

#[no_mangle]
pub unsafe extern "C" fn Code_CPUidFeature( Code : u16, CPUIDFeatures : *mut TCPUIDFeaturesArray ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    if CPUIDFeatures.is_null() {
        return false;
    }

    let cpuidfeaturesA = code.cpuid_features();

    (*CPUIDFeatures).Count = cpuidfeaturesA.len() as u8;
    for ( i, x ) in cpuidfeaturesA.iter().enumerate() {
        if i < (*CPUIDFeatures).Entries.len() {
            (*CPUIDFeatures).Entries[ i ] = *x;
        }
    }

    return true;
}

#[no_mangle]
pub unsafe extern "C" fn Code_FlowControl( Code : u16 ) -> u16/*FlowControl*/ { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.flow_control() as u16
}

#[no_mangle]
pub unsafe extern "C" fn Code_IsPrivileged( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_privileged()
}

#[no_mangle]
pub unsafe extern "C" fn Code_IsStackInstruction( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_stack_instruction()    
}

#[no_mangle]
pub unsafe extern "C" fn Code_IsSaveRestoreInstruction( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_save_restore_instruction()
}

#[no_mangle]
pub unsafe extern "C" fn Code_IsJccShort( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_jkcc_short()    
}

#[no_mangle]
pub unsafe extern "C" fn Code_IsJmpShort( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_jmp_short()
}

#[no_mangle]
pub unsafe extern "C" fn Code_IsJmpShortOrNear( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_jmp_short_or_near()
}

#[no_mangle]
pub unsafe extern "C" fn Code_IsJmpNear( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_jmp_near()
}

#[no_mangle]
pub unsafe extern "C" fn Code_IsJmpFar( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_jmp_far()
}

#[no_mangle]
pub unsafe extern "C" fn Code_IsCallNear( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_call_near()
}

#[no_mangle]
pub unsafe extern "C" fn Code_IsCallFar( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_call_far()
}

#[no_mangle]
pub unsafe extern "C" fn Code_IsJmpNearIndirect( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_jmp_near_indirect()
}

#[no_mangle]
pub unsafe extern "C" fn Code_IsJmpFarIndirect( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_jmp_far_indirect()
}

#[no_mangle]
pub unsafe extern "C" fn Code_IsCallNearIndirect( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_call_near_indirect()
}

#[no_mangle]
pub unsafe extern "C" fn Code_IsCallFarIndirect( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_call_far_indirect()
}

#[no_mangle]
pub unsafe extern "C" fn Code_ConditionCode( Code : u16 ) -> u8/*ConditionCode*/ { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.condition_code() as u8
}

#[no_mangle]
pub unsafe extern "C" fn Code_IsJcxShort( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_jcx_short()
}

#[no_mangle]
pub unsafe extern "C" fn Code_IsLoopCC( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_loopcc()
}

#[no_mangle]
pub unsafe extern "C" fn Code_IsLoop( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_loop()
}

#[no_mangle]
pub unsafe extern "C" fn Code_IsJccShortOrNear( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_jcc_short_or_near()
}

#[no_mangle]
pub unsafe extern "C" fn Code_NegateConditionCode( Code : u16 ) -> u16/*Code*/ { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.negate_condition_code() as u16
}

#[no_mangle]
pub unsafe extern "C" fn Code_AsShortBranch( Code : u16 ) -> u16/*Code*/ { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.as_short_branch() as u16
}

#[no_mangle]
pub unsafe extern "C" fn Code_AsNearBranch( Code : u16 ) -> u16/*Code*/ { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.as_near_branch() as u16
}