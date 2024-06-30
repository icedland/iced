/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86::OpCodeOperandKind;
use std::mem::transmute;// Enum<->Int

#[no_mangle]
pub unsafe extern "C" fn OpCodeOperandKind_AsString( OpCodeOperandKind : u8, Output : *mut u8, Size : usize ) { // FFI-Unsafe: OpCodeOperandKind
    if Output.is_null() {
        return;
    }
    if Size <= 0 {
        return;
    }
    
    let opCodeOperandKind : OpCodeOperandKind = transmute( OpCodeOperandKind as u8 );
    
    let output = format!("{opCodeOperandKind:?}");
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