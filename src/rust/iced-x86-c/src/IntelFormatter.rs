/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86_rust::{Instruction, Formatter, IntelFormatter};
#[cfg(feature = "decoder")]
use iced_x86_rust::Decoder;
use crate::SymbolResolver::{TSymbolResolver, TSymbolResolverCallback};
use crate::OptionsProvider::{TFormatterOptionsProvider, TFormatterOptionsProviderCallback};
use crate::OutputCallback::TFormatterOutput;
#[cfg(feature = "decoder")]
use crate::Decoder::TDecoderFormatCallback;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Intel-Formatter
pub(crate) struct TIntelFormatter {
    pub Formatter : IntelFormatter,
    pub Output : String,
  }

// Creates a Intel formatter
//
// # Arguments
// - `symbol_resolver`: Symbol resolver or `None`
// - `options_provider`: Operand options provider or `None`
#[no_mangle]
pub extern "C" fn IntelFormatter_Create( SymbolResolver : Option<TSymbolResolverCallback>, OptionsProvider : Option<TFormatterOptionsProviderCallback>, UserData : *const usize ) -> *mut TIntelFormatter {   
    if !SymbolResolver.is_none() && !OptionsProvider.is_none() {
        let symbols = Box::new( TSymbolResolver { callback:SymbolResolver, userData:UserData });
        let options = Box::new( TFormatterOptionsProvider { callback:OptionsProvider, userData:UserData });
        Box::into_raw( Box::new( TIntelFormatter { Formatter: IntelFormatter::with_options( Some( symbols ), Some( options ) ), Output: String::new() } ) )
    } else if !SymbolResolver.is_none() {
        let symbols = Box::new( TSymbolResolver { callback:SymbolResolver, userData:UserData });
        Box::into_raw( Box::new( TIntelFormatter { Formatter: IntelFormatter::with_options( Some( symbols ), None ), Output: String::new() } ) )
    } else if !OptionsProvider.is_none() {
        let options = Box::new( TFormatterOptionsProvider { callback:OptionsProvider, userData:UserData });
        Box::into_raw( Box::new( TIntelFormatter { Formatter: IntelFormatter::with_options( None, Some( options ) ), Output: String::new() } ) )
    } else {
        Box::into_raw( Box::new( TIntelFormatter { Formatter: IntelFormatter::with_options( None, None ), Output: String::new() } ) )
    }
}

// Format Instruction
#[no_mangle]
pub unsafe extern "C" fn IntelFormatter_Format( Formatter: *mut TIntelFormatter, Instruction: *mut Instruction, Output : *mut *const u8, Size : *mut usize ) {     
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
    let newsize = obj.Output.len()+1;
    obj.Output.as_mut_vec().resize( newsize, 0 );    
    (*Output) = obj.Output.as_ptr();
    (*Size) = obj.Output.len();
    Box::into_raw( obj );
}

#[no_mangle]
pub unsafe extern "C" fn IntelFormatter_FormatCallback( Formatter: *mut TIntelFormatter, Instruction: *mut Instruction, FormatterOutput : *mut TFormatterOutput ) {     
    if Formatter.is_null() {
        return;
    }
    if Instruction.is_null() {
        return;
    }
    if FormatterOutput.is_null() {
        return;
    }

    let mut obj = Box::from_raw( Formatter );
    let mut output = Box::from_raw( FormatterOutput );
    obj.Formatter.format( Instruction.as_mut().unwrap(), output.as_mut() );
    Box::into_raw( output );
    Box::into_raw( obj );
}

// Decode and Format Instruction
#[cfg(feature = "decoder")]
#[no_mangle]
pub unsafe extern "C" fn IntelFormatter_DecodeFormat( Decoder: *mut Decoder, Formatter: *mut TIntelFormatter, Instruction: *mut Instruction, Output : *mut *const u8, Size : *mut usize ) {
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
    let newsize = obj.Output.len()+1;
    obj.Output.as_mut_vec().resize( newsize, 0 );    
    (*Output) = obj.Output.as_ptr();
    (*Size) = obj.Output.len();
    Box::into_raw( obj );
}

#[cfg(feature = "decoder")]
#[no_mangle]
pub unsafe extern "C" fn IntelFormatter_DecodeFormatCallback( Decoder: *mut Decoder, Formatter: *mut TIntelFormatter, Instruction: *mut Instruction, FormatterOutput : *mut TFormatterOutput ) {     
    if Decoder.is_null() {
        return;
    }    
    if Formatter.is_null() {
        return;
    }
    if Instruction.is_null() {
        return;
    }
    if FormatterOutput.is_null() {
        return;
    }

    // Decode
    let mut obj = Box::from_raw( Decoder );    
    obj.decode_out( Instruction.as_mut().unwrap() );
    Box::into_raw( obj );

    // Format
    let mut obj = Box::from_raw( Formatter );
    let mut output = Box::from_raw( FormatterOutput );
    obj.Formatter.format( Instruction.as_mut().unwrap(), output.as_mut() );
    Box::into_raw( output );
    Box::into_raw( obj );
}

// Decode and Format Instruction until end
#[cfg(feature = "decoder")]
#[no_mangle]
pub unsafe extern "C" fn IntelFormatter_DecodeFormatToEnd( Decoder: *mut Decoder, Formatter: *mut TIntelFormatter, Callback: Option<TDecoderFormatCallback>, UserData : *const usize ) {
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
        let newsize = formatter.Output.len()+1;
        formatter.Output.as_mut_vec().resize( newsize, 0 );

        Callback.unwrap()( &mut instruction, formatter.Output.as_ptr(), formatter.Output.len(), &mut stop, UserData );
    }
    Box::into_raw( formatter );
    Box::into_raw( decoder );    
}