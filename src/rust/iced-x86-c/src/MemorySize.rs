/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86_rust::MemorySize;
#[cfg(feature = "instr_create")]
use iced_x86_rust::MemorySizeInfo;
use std::mem::transmute;// Enum<->Int

#[no_mangle]
pub unsafe extern "C" fn MemorySize_AsString( MemorySize : u8, Output : *mut u8, Size : usize ) { // FFI-Unsafe: MemorySize
    if Output.is_null() {
        return;
    }
    if Size <= 0 {
        return;
    }
    
    let memorySize : MemorySize = transmute( MemorySize as u8 );
    
    let output = format!("{memorySize:?}");
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

#[cfg(feature = "instr_create")]
#[no_mangle]
pub unsafe extern "C" fn MemorySize_Info( MemorySize : u8, Info : *mut MemorySizeInfo ) { // FFI-Unsafe: MemorySize
    if Info.is_null() {
        return;
    }
    
    let memorySize : MemorySize = transmute( MemorySize as u8 );
    let info = memorySize.info();
    *Info = *info;
}