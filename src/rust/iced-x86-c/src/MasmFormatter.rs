/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86_rust::{Instruction, Formatter, MasmFormatter};
#[cfg(feature = "decoder")]
use iced_x86_rust::Decoder;
use crate::SymbolResolver::{TSymbolResolver, TSymbolResolverCallback};
use crate::OptionsProvider::{TFormatterOptionsProvider, TFormatterOptionsProviderCallback};
use crate::OutputCallback::TFormatterOutput;
#[cfg(feature = "decoder")]
use crate::Decoder::TDecoderFormatCallback;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Masm-Formatter
pub(crate) struct TMasmFormatter {
    pub Formatter : MasmFormatter,
    pub Output : String,
  }

// Creates a masm formatter
//
// # Arguments
// - `symbol_resolver`: Symbol resolver or `None`
// - `options_provider`: Operand options provider or `None`
#[no_mangle]
pub extern "C" fn MasmFormatter_Create( SymbolResolver : Option<TSymbolResolverCallback>, OptionsProvider : Option<TFormatterOptionsProviderCallback>, UserData : *const usize ) -> *mut TMasmFormatter {   
    if !SymbolResolver.is_none() && !OptionsProvider.is_none() {
        let symbols = Box::new( TSymbolResolver { callback:SymbolResolver, userData:UserData });
        let options = Box::new( TFormatterOptionsProvider { callback:OptionsProvider, userData:UserData });
        Box::into_raw( Box::new( TMasmFormatter { Formatter: MasmFormatter::with_options( Some( symbols ), Some( options ) ), Output: String::new() } ) )
    } else if !SymbolResolver.is_none() {
        let symbols = Box::new( TSymbolResolver { callback:SymbolResolver, userData:UserData });
        Box::into_raw( Box::new( TMasmFormatter { Formatter: MasmFormatter::with_options( Some( symbols ), None ), Output: String::new() } ) )
    } else if !OptionsProvider.is_none() {
        let options = Box::new( TFormatterOptionsProvider { callback:OptionsProvider, userData:UserData });
        Box::into_raw( Box::new( TMasmFormatter { Formatter: MasmFormatter::with_options( None, Some( options ) ), Output: String::new() } ) )
    } else {
        Box::into_raw( Box::new( TMasmFormatter { Formatter: MasmFormatter::with_options( None, None ), Output: String::new() } ) )
    }
}

// Format Instruction
#[no_mangle]
pub unsafe extern "C" fn MasmFormatter_Format( Formatter: *mut TMasmFormatter, Instruction: *mut Instruction, Output : *mut *const u8, Size : *mut usize ) {     
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
pub unsafe extern "C" fn MasmFormatter_FormatCallback( Formatter: *mut TMasmFormatter, Instruction: *mut Instruction, FormatterOutput : *mut TFormatterOutput ) {     
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
pub unsafe extern "C" fn MasmFormatter_DecodeFormat( Decoder: *mut Decoder, Formatter: *mut TMasmFormatter, Instruction: *mut Instruction, Output : *mut *const u8, Size : *mut usize ) {
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
pub unsafe extern "C" fn MasmFormatter_DecodeFormatCallback( Decoder: *mut Decoder, Formatter: *mut TMasmFormatter, Instruction: *mut Instruction, FormatterOutput : *mut TFormatterOutput ) {     
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
pub unsafe extern "C" fn MasmFormatter_DecodeFormatToEnd( Decoder: *mut Decoder, Formatter: *mut TMasmFormatter, Callback: Option<TDecoderFormatCallback>, UserData : *const usize ) {
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

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// ( masm only ): Add a `DS` segment override even if it's not present. Used if it's 16/32-bit code and mem op is a displ
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `true` | `mov eax,ds:[ 12345678 ]`
// _ | `false` | `mov eax,[ 12345678 ]`
#[no_mangle]
pub unsafe extern "C" fn MasmFormatter_GetAddDsPrefix32( Formatter: *mut TMasmFormatter ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    let value = obj.Formatter.options_mut().masm_add_ds_prefix32();

    Box::into_raw( obj );
 
    return value;
}

// ( masm only ): Add a `DS` segment override even if it's not present. Used if it's 16/32-bit code and mem op is a displ
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `true` | `mov eax,ds:[ 12345678 ]`
// _ | `false` | `mov eax,[ 12345678 ]`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn MasmFormatter_SetAddDsPrefix32( Formatter: *mut TMasmFormatter, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    obj.Formatter.options_mut().set_masm_add_ds_prefix32( Value );

    Box::into_raw( obj );

    return true;
}

// ( masm only ): Show symbols in brackets
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `true` | `[ ecx+symbol ]` / `[ symbol ]`
// _ | `false` | `symbol[ ecx ]` / `symbol`
#[no_mangle]
pub unsafe extern "C" fn MasmFormatter_GetSymbolDisplacementInBrackets( Formatter: *mut TMasmFormatter ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    let value = obj.Formatter.options_mut().masm_symbol_displ_in_brackets();

    Box::into_raw( obj );
 
    return value;
}

// ( masm only ): Show symbols in brackets
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `true` | `[ ecx+symbol ]` / `[ symbol ]`
// _ | `false` | `symbol[ ecx ]` / `symbol`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn MasmFormatter_SetSymbolDisplacementInBrackets( Formatter: *mut TMasmFormatter, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    obj.Formatter.options_mut().set_masm_symbol_displ_in_brackets( Value );

    Box::into_raw( obj );

    return true;
}

// ( masm only ): Show displacements in brackets
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `true` | `[ ecx+1234h ]`
// _ | `false` | `1234h[ ecx ]`
#[no_mangle]
pub unsafe extern "C" fn MasmFormatter_GetDisplacementInBrackets( Formatter: *mut TMasmFormatter ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    let value = obj.Formatter.options_mut().masm_displ_in_brackets();

    Box::into_raw( obj );
 
    return value;
}

// ( masm only ): Show displacements in brackets
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `true` | `[ ecx+1234h ]`
// _ | `false` | `1234h[ ecx ]`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn MasmFormatter_SetDisplacementInBrackets( Formatter: *mut TMasmFormatter, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    obj.Formatter.options_mut().set_masm_displ_in_brackets( Value );

    Box::into_raw( obj );

    return true;
}