/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86_rust::{Instruction, SpecializedFormatter, DefaultFastFormatterTraitOptions};
#[cfg(feature = "decoder")]
use iced_x86_rust::Decoder;
use crate::SymbolResolver::{TSymbolResolver, TSymbolResolverCallback};
#[cfg(feature = "decoder")]
use crate::Decoder::TDecoderFormatCallback;
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

// Decode and Format Instruction
#[cfg(feature = "decoder")]
#[no_mangle]
pub unsafe extern "C" fn FastFormatter_DecodeFormat( Decoder: *mut Decoder, Formatter: *mut TFastFormatter, Instruction: *mut Instruction, Output : *mut *const u8, Size : *mut usize ) {
    if Decoder.is_null() {
        return;
    }
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

    // Decode
    let mut obj = Box::from_raw( Decoder );    
    obj.decode_out( Instruction.as_mut().unwrap() );
    Box::into_raw( obj );

    // Format
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

// Decode and Format Instruction until end
#[cfg(feature = "decoder")]
#[no_mangle]
pub unsafe extern "C" fn FastFormatter_DecodeFormatToEnd( Decoder: *mut Decoder, Formatter: *mut TFastFormatter, Callback: Option<TDecoderFormatCallback>, UserData : *const usize ) {
    if Decoder.is_null() {
        return;
    }
    if Formatter.is_null() {
        return;
    }
    if Callback.is_none() {
        return;
    }

    // Decode
    let mut decoder = Box::from_raw( Decoder );
    let mut formatter = Box::from_raw( Formatter );
    let mut stop: bool = false;
    let mut instruction: Instruction = Instruction::new();
    while decoder.can_decode() && !stop {
        decoder.decode_out( &mut instruction );

        // Format
        formatter.Output.clear();
        formatter.Formatter.format( &instruction, &mut formatter.Output );

        if formatter.Output.len() < 315/*MAX_FMT_INSTR_LEN*/ {
            *formatter.Output.as_mut_ptr().add(formatter.Output.len()) = 0;
        } else {
            let newsize = formatter.Output.len()+1;
            formatter.Output.as_mut_vec().resize( newsize, 0 );    
        }

        Callback.unwrap()( &mut instruction, formatter.Output.as_ptr(), formatter.Output.len(), &mut stop, UserData );
    }
    Box::into_raw( formatter );
    Box::into_raw( decoder );    
}