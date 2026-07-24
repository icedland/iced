/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2026
*/

use iced_x86_rust::MemorySize;
#[cfg(feature = "instr_create")]
use iced_x86_rust::MemorySizeInfo;
use std::mem::transmute;// Enum<->Int
use std::slice;

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
    let aSource = output.as_bytes();

    let n = std::cmp::min(aSource.len(), Size - 1);
    let aOutput = slice::from_raw_parts_mut(Output, Size);

    aOutput[..n].copy_from_slice(&aSource[..n]);
    aOutput[n] = 0;
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