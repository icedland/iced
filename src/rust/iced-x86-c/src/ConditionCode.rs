/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2026
*/

use iced_x86_rust::ConditionCode;
use std::mem::transmute;// Enum<->Int
use std::slice;

#[no_mangle]
pub unsafe extern "C" fn ConditionCode_AsString( ConditionCode : u8, Output : *mut u8, Size : usize ) { // FFI-Unsafe: ConditionCode
    if Output.is_null() {
        return;
    }
    if Size <= 0 {
        return;
    }
    
    let conditionCode : ConditionCode = transmute( ConditionCode as u8 );
    let output = format!("{conditionCode:?}");
    let aSource = output.as_bytes();

    let n = std::cmp::min(aSource.len(), Size - 1);
    let aOutput = slice::from_raw_parts_mut(Output, Size);

    aOutput[..n].copy_from_slice(&aSource[..n]);
    aOutput[n] = 0;
}