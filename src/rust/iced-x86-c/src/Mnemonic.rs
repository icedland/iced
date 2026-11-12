/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2026
*/

use iced_x86_rust::Mnemonic;
use std::mem::transmute;// Enum<->Int
use std::slice;

#[no_mangle]
pub unsafe extern "C" fn Mnemonic_AsString( Mnemonic : u16, Output : *mut u8, Size : usize ) { // FFI-Unsafe: Mnemonic
    if Output.is_null() {
        return;
    }
    if Size <= 0 {
        return;
    }
    
    let mnemonic : Mnemonic = transmute( Mnemonic as u16 );
    let output = format!("{mnemonic:?}");
    let aSource = output.as_bytes();

    let n = std::cmp::min(aSource.len(), Size - 1);
    let aOutput = slice::from_raw_parts_mut(Output, Size);

    aOutput[..n].copy_from_slice(&aSource[..n]);
    aOutput[n] = 0;
}