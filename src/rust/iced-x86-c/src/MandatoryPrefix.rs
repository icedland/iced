/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86_rust::MandatoryPrefix;
use std::mem::transmute;// Enum<->Int

#[no_mangle]
pub unsafe extern "C" fn MandatoryPrefix_AsString( MandatoryPrefix : u8, Output : *mut u8, Size : usize ) { // FFI-Unsafe: MandatoryPrefix
    if Output.is_null() {
        return;
    }
    if Size <= 0 {
        return;
    }
    
    let mandatoryPrefix : MandatoryPrefix = transmute( MandatoryPrefix as u8 );
    
    let output = format!("{mandatoryPrefix:?}");
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