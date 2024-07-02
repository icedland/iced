/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86_rust::{Instruction, Formatter, MasmFormatter};
use crate::SymbolResolver::{TSymbolResolver, TSymbolResolverCallback};
use crate::OptionsProvider::{TFormatterOptionsProvider, TFormatterOptionsProviderCallback};
use crate::OutputCallback::TFormatterOutput;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Masm-Formatter

// Creates a masm formatter
//
// # Arguments
// - `symbol_resolver`: Symbol resolver or `None`
// - `options_provider`: Operand options provider or `None`
#[no_mangle]
pub extern "C" fn MasmFormatter_Create( SymbolResolver : Option<TSymbolResolverCallback>, OptionsProvider : Option<TFormatterOptionsProviderCallback>, UserData : *const usize ) -> *mut MasmFormatter {   
    if !SymbolResolver.is_none() && !OptionsProvider.is_none() {
        let symbols = Box::new( TSymbolResolver { callback:SymbolResolver, userData:UserData });
        let options = Box::new( TFormatterOptionsProvider { callback:OptionsProvider, userData:UserData });
        Box::into_raw( Box::new( MasmFormatter::with_options( Some( symbols ), Some( options ) ) ) )
    }else if !SymbolResolver.is_none() {
        let symbols = Box::new( TSymbolResolver { callback:SymbolResolver, userData:UserData });
        Box::into_raw( Box::new( MasmFormatter::with_options( Some( symbols ), None ) ) )                
    }else if !OptionsProvider.is_none() {
        let options = Box::new( TFormatterOptionsProvider { callback:OptionsProvider, userData:UserData });
        Box::into_raw( Box::new( MasmFormatter::with_options( None, Some( options ) ) ) )
    }else {
        Box::into_raw( Box::new( MasmFormatter::with_options( None, None ) ) )
    }
}

// Format Instruction
#[no_mangle]
pub unsafe extern "C" fn MasmFormatter_Format( MasmFormatter: *mut MasmFormatter, Instruction: *mut Instruction, Output : *mut u8, Size : usize ) {     
    if MasmFormatter.is_null() {
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

    let mut obj = Box::from_raw( MasmFormatter );
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
pub unsafe extern "C" fn MasmFormatter_FormatCallback( MasmFormatter: *mut MasmFormatter, Instruction: *mut Instruction, FormatterOutput : *mut TFormatterOutput ) {     
    if MasmFormatter.is_null() {
        return;
    }
    if Instruction.is_null() {
        return;
    }
    if FormatterOutput.is_null() {
        return;
    }

    let mut obj = Box::from_raw( MasmFormatter );
    let mut output = Box::from_raw( FormatterOutput );
    obj.format( Instruction.as_mut().unwrap(), output.as_mut() );
    Box::into_raw( output );
    Box::into_raw( obj );
}

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// ( masm only ): Add a `DS` segment override even if it's not present. Used if it's 16/32-bit code and mem op is a displ
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `true` | `mov eax,ds:[ 12345678 ]`
// _ | `false` | `mov eax,[ 12345678 ]`
#[no_mangle]
pub unsafe extern "C" fn MasmFormatter_GetAddDsPrefix32( Formatter: *mut MasmFormatter ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    let value = obj.options_mut().masm_add_ds_prefix32();

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
pub unsafe extern "C" fn MasmFormatter_SetAddDsPrefix32( Formatter: *mut MasmFormatter, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    obj.options_mut().set_masm_add_ds_prefix32( Value );

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
pub unsafe extern "C" fn MasmFormatter_GetSymbolDisplacementInBrackets( Formatter: *mut MasmFormatter ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    let value = obj.options_mut().masm_symbol_displ_in_brackets();

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
pub unsafe extern "C" fn MasmFormatter_SetSymbolDisplacementInBrackets( Formatter: *mut MasmFormatter, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    obj.options_mut().set_masm_symbol_displ_in_brackets( Value );

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
pub unsafe extern "C" fn MasmFormatter_GetDisplacementInBrackets( Formatter: *mut MasmFormatter ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    let value = obj.options_mut().masm_displ_in_brackets();

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
pub unsafe extern "C" fn MasmFormatter_SetDisplacementInBrackets( Formatter: *mut MasmFormatter, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    obj.options_mut().set_masm_displ_in_brackets( Value );

    Box::into_raw( obj );

    return true;
}