/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

//#![allow( non_snake_case )]
//extern crate libc;

use iced_x86::{FormatterOutput, FormatterTextKind};
use std::{str, ptr::null_mut};
use libc::{c_char};
use std::ffi::CString;

// Formatter Output Callback
// Used by a [`Formatter`] to write all text. `String` also implements this trait.
//
// The only method that must be implemented is [`write()`], all other methods call it if they're not overridden.
type
  TFormatterOutputCallback = unsafe extern "C" fn( Text: *const c_char, Kind: u8 /*FormatterTextKind*/, UserData : *const usize );

pub struct TFormatterOutput {
    pub(crate) userData : *const usize,
    pub(crate) callback: Option<TFormatterOutputCallback>    
}

impl FormatterOutput for TFormatterOutput {
    fn write(&mut self, text: &str, kind: FormatterTextKind) {
        let value = CString::new( text ).unwrap();
        unsafe {
            self.callback.unwrap()( value.to_bytes().as_ptr() as *const i8, kind as u8, self.userData );
        }
    }
}

// Creates a formatter Output Callback
#[no_mangle]
pub extern "C" fn FormatterOutput_Create( Callback : Option<TFormatterOutputCallback>, UserData : *const usize ) -> *mut TFormatterOutput {   
    if Callback.is_none() {
        return null_mut();
    }

    Box::into_raw( Box::new( TFormatterOutput { callback:Callback, userData:UserData }) )
}