/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86::MvexRegMemConv;
use std::mem::transmute;// Enum<->Int

#[no_mangle]
pub unsafe extern "C" fn MvexRegMemConv_AsString( MvexRegMemConv : u8, Output : *mut u8, Size : usize ) { // FFI-Unsafe: MvexRegMemConv
    if Output.is_null() {
        return;
    }
    if Size <= 0 {
        return;
    }
    
    let MvexRegMemConv : MvexRegMemConv = transmute( MvexRegMemConv as u8 );
    
    let output = format!("{MvexRegMemConv:?}");
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