/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86_rust::{Instruction, SpecializedFormatter, DefaultFastFormatterTraitOptions};
use crate::SymbolResolver::{TSymbolResolver, TSymbolResolverCallback};
use std::ptr::null_mut;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Fast-Formatter
pub(crate) type
  TFastFormatterDefault = SpecializedFormatter<DefaultFastFormatterTraitOptions>;

pub(crate) struct TFastFormatter {
    pub Formatter : TFastFormatterDefault,
    pub Output : String,
}

// Creates a Fast formatter
#[no_mangle]
pub extern "C" fn FastFormatter_Create( SymbolResolver : Option<TSymbolResolverCallback>, UserData : *const usize ) -> *mut TFastFormatter {   
    if !SymbolResolver.is_none() {
        let symbols = Box::new( TSymbolResolver { callback:SymbolResolver, userData:UserData });

        match TFastFormatterDefault::try_with_options( Some( symbols ) ) {
            Ok( value ) => return Box::into_raw( Box::new( TFastFormatter { Formatter: value, Output: String::new() } ) ),
            Err( _e ) => return null_mut()
        }
    } else {
        match TFastFormatterDefault::try_with_options( None ) {
            Ok( value ) => return Box::into_raw( Box::new( TFastFormatter { Formatter: value, Output: String::new() } ) ),
            Err( _e ) => return null_mut()
        }
    }
}

// Format Instruction
#[no_mangle]
pub unsafe extern "C" fn FastFormatter_Format( Formatter: *mut TFastFormatter, Instruction: *mut Instruction, Output : *mut *const u8, Size : *mut usize ) {
    if Formatter.is_null() {
        return;
    }
    if Instruction.is_null() {
        return;
    }
    if Output.is_null() {
        return;
    }
    if Size.is_null() {
        return;
    }

    let mut obj = Box::from_raw( Formatter );
    obj.Output.clear();
    obj.Formatter.format( Instruction.as_mut().unwrap(), &mut obj.Output );
    if obj.Output.len() < 315/*MAX_FMT_INSTR_LEN*/ {
        *obj.Output.as_mut_ptr().add(obj.Output.len()) = 0;
    } else {
        let newsize = obj.Output.len()+1;
        obj.Output.as_mut_vec().resize( newsize, 0 );    
    }
    (*Output) = obj.Output.as_ptr();
    (*Size) = obj.Output.len();
    Box::into_raw( obj );
}