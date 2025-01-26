/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86_rust::OpAccess;
use std::mem::transmute;// Enum<->Int

#[no_mangle]
pub unsafe extern "C" fn OpAccess_AsString( OpAccess : u8, Output : *mut u8, Size : usize ) { // FFI-Unsafe: OpAccess
    if Output.is_null() {
        return;
    }
    if Size <= 0 {
        return;
    }
    
    let opAccess : OpAccess = transmute( OpAccess as u8 );
    let output = format!("{opAccess:?}");

    let aOutput = Output as *mut [u8;1024];
    let aSource = output.as_bytes();
        
    let n = std::cmp::min( aSource.len(), Size/*(*aOutput).len()*/ );
    (*aOutput)[0..n].copy_from_slice(&aSource[0..n]);
    (*aOutput)[n] = 0;
}