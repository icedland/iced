/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86::{Instruction, Formatter, NasmFormatter};
use crate::SymbolResolver::{TSymbolResolver, TSymbolResolverCallback};
use crate::OptionsProvider::{TFormatterOptionsProvider, TFormatterOptionsProviderCallback};
use crate::OutputCallback::TFormatterOutput;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Nasm-Formatter

// Creates a Nasm formatter
//
// # Arguments
// - `symbol_resolver`: Symbol resolver or `None`
// - `options_provider`: Operand options provider or `None`
#[no_mangle]
pub extern "C" fn NasmFormatter_Create( SymbolResolver : Option<TSymbolResolverCallback>, OptionsProvider : Option<TFormatterOptionsProviderCallback>, UserData : *const usize ) -> *mut NasmFormatter {   
    if !SymbolResolver.is_none() && !OptionsProvider.is_none() {
        let symbols = Box::new( TSymbolResolver { callback:SymbolResolver, userData:UserData });
        let options = Box::new( TFormatterOptionsProvider { callback:OptionsProvider, userData:UserData });
        Box::into_raw( Box::new( NasmFormatter::with_options( Some( symbols ), Some( options ) ) ) )        
    }else if !SymbolResolver.is_none() {
        let symbols = Box::new( TSymbolResolver { callback:SymbolResolver, userData:UserData });
        Box::into_raw( Box::new( NasmFormatter::with_options( Some( symbols ), None ) ) )                
    }else if !OptionsProvider.is_none() {
        let options = Box::new( TFormatterOptionsProvider { callback:OptionsProvider, userData:UserData });
        Box::into_raw( Box::new( NasmFormatter::with_options( None, Some( options ) ) ) )
    }else {
        Box::into_raw( Box::new( NasmFormatter::with_options( None, None ) ) )
    }
}

// Format Instruction
#[no_mangle]
pub unsafe extern "C" fn NasmFormatter_Format( NasmFormatter: *mut NasmFormatter, Instruction: *mut Instruction, Output : *mut u8, Size : usize ) {     
    if NasmFormatter.is_null() {
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

    let mut obj = Box::from_raw( NasmFormatter );
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

#[no_mangle]
pub unsafe extern "C" fn NasmFormatter_FormatCallback( NasmFormatter: *mut NasmFormatter, Instruction: *mut Instruction, FormatterOutput : *mut TFormatterOutput ) {     
    if NasmFormatter.is_null() {
        return;
    }
    if Instruction.is_null() {
        return;
    }
    if FormatterOutput.is_null() {
        return;
    }

    let mut obj = Box::from_raw( NasmFormatter );
    let mut output = Box::from_raw( FormatterOutput );
    obj.format( Instruction.as_mut().unwrap(), output.as_mut() );
    Box::into_raw( output );
    Box::into_raw( obj );
}

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// ( nasm only ): Shows `BYTE`, `WORD`, `DWORD` or `QWORD` if it's a sign extended immediate operand value
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `or rcx,byte -1`
// 👍 | `false` | `or rcx,-1`
#[no_mangle]
pub unsafe extern "C" fn NasmFormatter_GetShowSignExtendedImmediateSize( Formatter: *mut NasmFormatter ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    let value = obj.options_mut().nasm_show_sign_extended_immediate_size();

    Box::into_raw( obj );
 
    return value;
}

// ( nasm only ): Shows `BYTE`, `WORD`, `DWORD` or `QWORD` if it's a sign extended immediate operand value
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `or rcx,byte -1`
// 👍 | `false` | `or rcx,-1`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn NasmFormatter_SetShowSignExtendedImmediateSize( Formatter: *mut NasmFormatter, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    obj.options_mut().set_nasm_show_sign_extended_immediate_size( Value );

    Box::into_raw( obj );

    return true;
}