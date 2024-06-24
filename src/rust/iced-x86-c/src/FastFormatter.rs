/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86::{Instruction, SpecializedFormatter, DefaultFastFormatterTraitOptions};
use crate::SymbolResolver::{TSymbolResolver, TSymbolResolverCallback};
use std::ptr::null_mut;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Fast-Formatter
pub(crate) type
  TFastFormatter = SpecializedFormatter<DefaultFastFormatterTraitOptions>;

// Creates a Fast formatter
#[no_mangle]
pub extern "C" fn FastFormatter_Create( SymbolResolver : Option<TSymbolResolverCallback>, UserData : *const usize ) -> *mut TFastFormatter {   
    if !SymbolResolver.is_none() {
        let symbols = Box::new( TSymbolResolver { callback:SymbolResolver, userData:UserData });

        match TFastFormatter::try_with_options( Some( symbols ) ) {
            Ok( value ) => return Box::into_raw( Box::new( value ) ),
            Err( _e ) => return null_mut()
        }
    }else {
        match TFastFormatter::try_with_options( None ) {
            Ok( value ) => return Box::into_raw( Box::new( value ) ),
            Err( _e ) => return null_mut()
        }
    }
}

// Format Instruction
#[no_mangle]
pub unsafe extern "C" fn FastFormatter_Format( Formatter: *mut TFastFormatter, Instruction: *mut Instruction, Output : *mut u8, Size : usize ) {     
    if Formatter.is_null() {
        return;
    }
    if Instruction.is_null() {
        return;
    }
    if Output.is_null() {
        return;
    }
    if Size <= 0 {
        return;
    }

    let mut obj = Box::from_raw( Formatter );
    let mut output = String::new();
    obj.format( Instruction.as_mut().unwrap(), &mut output );
    Box::into_raw( obj );

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