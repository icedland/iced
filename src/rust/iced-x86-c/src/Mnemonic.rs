/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86::Mnemonic;
use std::mem::transmute;// Enum<->Int

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