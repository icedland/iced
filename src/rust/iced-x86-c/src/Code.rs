/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86_rust::Code;
#[cfg(feature = "instr_info")]
use iced_x86_rust::CpuidFeature;
#[cfg(feature = "op_code_info")]
use iced_x86_rust::OpCodeOperandKind;
#[cfg(feature = "op_code_info")]
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

    let aOutput = Output as *mut [u8;1024];
    let aSource = output.as_bytes();
        
    let n = std::cmp::min( aSource.len(), Size/*(*aOutput).len()*/ );
    (*aOutput)[0..n].copy_from_slice(&aSource[0..n]);
    (*aOutput)[n] = 0;
}

#[no_mangle]
pub unsafe extern "C" fn Code_Mnemonic( Code : u16 ) -> u16/*Mnemonic*/ { // FFI-Unsafe: Code, Mnemonic
    let code : Code = transmute( Code as u16 );
    code.mnemonic() as u16
}

#[cfg(feature = "op_code_info")]
#[no_mangle]
pub unsafe extern "C" fn Code_OPCode( Code : u16, Info : *mut TOpCodeInfo ) { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    let info = code.op_code();

/*
    // OpCodeString
    let output = info.op_code_string().as_bytes();
    let n = std::cmp::min( output.len(), (*Info).op_code_string.len() );
    (*Info).op_code_string[0..n].copy_from_slice(&output[0..n]);
    (*Info).op_code_string[n] = 0;

    // InstructionString
    let output = info.instruction_string().as_bytes();
    let n = std::cmp::min( output.len(), (*Info).instruction_string.len() );
    (*Info).instruction_string[0..n].copy_from_slice(&output[0..n]);
    (*Info).instruction_string[n] = 0;
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

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Code_Encoding( Code : u16 ) -> u16/*EncodingKind*/ { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );

    code.encoding() as u16
}

// Gets the CPU or CPUID feature flags
#[cfg(feature = "instr_info")]
#[allow( non_upper_case_globals )]
pub const CPUIDFeaturesMaxEntries : usize = 5;
#[cfg(feature = "instr_info")]
//#[repr(C)]
#[repr(packed)]
pub struct TCPUIDFeaturesArray { 
    pub Entries : [CpuidFeature;CPUIDFeaturesMaxEntries], 
    pub Count : u8
}

#[cfg(feature = "instr_info")]
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

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Code_FlowControl( Code : u16 ) -> u16/*FlowControl*/ { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.flow_control() as u16
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Code_IsPrivileged( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_privileged()
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Code_IsStackInstruction( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_stack_instruction()    
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Code_IsSaveRestoreInstruction( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_save_restore_instruction()
}

#[cfg(feature = "mvex")]
#[no_mangle]
pub unsafe extern "C" fn Code_IsJccShort( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_jkcc_short()    
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Code_IsJmpShort( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_jmp_short()
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Code_IsJmpShortOrNear( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_jmp_short_or_near()
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Code_IsJmpNear( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_jmp_near()
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Code_IsJmpFar( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_jmp_far()
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Code_IsCallNear( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_call_near()
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Code_IsCallFar( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_call_far()
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Code_IsJmpNearIndirect( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_jmp_near_indirect()
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Code_IsJmpFarIndirect( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_jmp_far_indirect()
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Code_IsCallNearIndirect( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_call_near_indirect()
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Code_IsCallFarIndirect( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_call_far_indirect()
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Code_ConditionCode( Code : u16 ) -> u8/*ConditionCode*/ { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.condition_code() as u8
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Code_IsJcxShort( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_jcx_short()
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Code_IsLoopCC( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_loopcc()
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Code_IsLoop( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_loop()
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Code_IsJccShortOrNear( Code : u16 ) -> bool { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.is_jcc_short_or_near()
}

#[cfg(feature = "instr_create")]
#[no_mangle]
pub unsafe extern "C" fn Code_NegateConditionCode( Code : u16 ) -> u16/*Code*/ { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.negate_condition_code() as u16
}

#[cfg(feature = "instr_create")]
#[no_mangle]
pub unsafe extern "C" fn Code_AsShortBranch( Code : u16 ) -> u16/*Code*/ { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.as_short_branch() as u16
}

#[cfg(feature = "instr_create")]
#[no_mangle]
pub unsafe extern "C" fn Code_AsNearBranch( Code : u16 ) -> u16/*Code*/ { // FFI-Unsafe: Code
    let code : Code = transmute( Code as u16 );
    code.as_near_branch() as u16
}