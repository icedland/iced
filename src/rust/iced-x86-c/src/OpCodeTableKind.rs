/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86_rust::OpCodeTableKind;
use std::mem::transmute;// Enum<->Int

#[no_mangle]
pub unsafe extern "C" fn OpCodeTableKind_AsString( OpCodeTableKind : u8, Output : *mut u8, Size : usize ) { // FFI-Unsafe: OpCodeTableKind
    if Output.is_null() {
        return;
    }
    if Size <= 0 {
        return;
    }
    
    let opCodeTableKind : OpCodeTableKind = transmute( OpCodeTableKind as u8 );    
    let output = format!("{opCodeTableKind:?}");
    
    let aOutput = Output as *mut [u8;1024];
    let aSource = output.as_bytes();
        
    let n = std::cmp::min( aSource.len(), Size/*(*aOutput).len()*/ );
    (*aOutput)[0..n].copy_from_slice(&aSource[0..n]);
    (*aOutput)[n] = 0;
}