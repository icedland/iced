/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86_rust::Register;
use std::mem::transmute;// Enum<->Int

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Register_Base( Register : u8 ) -> u8 { // FFI-Unsafe: Register   
    let register : Register = transmute( Register as u8 );
    transmute( register.base() as u8 )
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Register_Number( Register : u8 ) -> usize { // FFI-Unsafe: Register   
    let register : Register = transmute( Register as u8 );
    register.number()
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Register_FullRegister( Register : u8 ) -> u8 { // FFI-Unsafe: Register   
    let register : Register = transmute( Register as u8 );
    transmute( register.full_register() as u8 )
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Register_FullRegister32( Register : u8 ) -> u8 { // FFI-Unsafe: Register   
    let register : Register = transmute( Register as u8 );
    transmute( register.full_register32() as u8 )
}

#[cfg(feature = "instr_info")]
#[no_mangle]
pub unsafe extern "C" fn Register_Size( Register : u8 ) -> usize { // FFI-Unsafe: Register   
    let register : Register = transmute( Register as u8 );
    register.size()
}

#[no_mangle]
pub unsafe extern "C" fn Register_AsString( Register : u8, Output : *mut u8, Size : usize ) { // FFI-Unsafe: Register
    if Output.is_null() {
        return;
    }
    if Size <= 0 {
        return;
    }
    
    let register : Register = transmute( Register as u8 );
    let output = format!("{register:?}");
    
    let aOutput = Output as *mut [u8;1024];
    let aSource = output.as_bytes();
        
    let n = std::cmp::min( aSource.len(), Size/*(*aOutput).len()*/ );
    (*aOutput)[0..n].copy_from_slice(&aSource[0..n]);
    (*aOutput)[n] = 0;
}