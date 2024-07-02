/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86_rust::{Instruction, SpecializedFormatter, DefaultSpecializedFormatterTraitOptions, SymbolResolver};
use crate::SymbolResolver::{TSymbolResolver, TSymbolResolverCallback};
use crate::SpecializedFormatterTraitOptions::*;

use std::ptr::null_mut;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Specialized-Formatter
pub(crate) type
  TSpecializedFormatter = SpecializedFormatter<DefaultSpecializedFormatterTraitOptions>;

// Creates a Specialized formatter
#[no_mangle]
pub extern "C" fn SpecializedFormatter_Create( SymbolResolver : Option<TSymbolResolverCallback>, UserData : *const usize ) -> *mut TSpecializedFormatter {
    let mut symbols: Option<Box<dyn SymbolResolver>> = None;
    if !SymbolResolver.is_none() {
        symbols = Some( Box::new( TSymbolResolver { callback:SymbolResolver, userData:UserData } ) );
    }

    match TSpecializedFormatter::try_with_options( symbols ) {
        Ok( value ) => return Box::into_raw( Box::new( value ) ),
        Err( _e ) => return null_mut()
    }    
}

// Format Instruction
#[no_mangle]
/*
pub unsafe extern "C" fn SpecializedFormatter_Format( Formatter: *mut TSpecializedFormatter, Options : u16, Instruction: *mut Instruction, Output : *mut u8, Size : usize ) {     
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
*/

pub unsafe extern "C" fn SpecializedFormatter_Format( Formatter: *mut TSpecializedFormatter, Options : u8, Instruction: *mut Instruction, Output : *mut u8, Size : usize ) {     
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

    let mut output = String::new();

    match Options {
        0 => {
            let mut obj = Box::from_raw(Formatter as *mut TSpecializedFormatter000);
            obj.format(Instruction.as_mut().unwrap(), &mut output);
            Box::into_raw(obj);
        }

        1 => {
            let mut obj = Box::from_raw(Formatter as *mut TSpecializedFormatter001);
            obj.format(Instruction.as_mut().unwrap(), &mut output);
            Box::into_raw(obj);
        }
    
        2 => {
            let mut obj = Box::from_raw(Formatter as *mut TSpecializedFormatter010);
            obj.format(Instruction.as_mut().unwrap(), &mut output);
            Box::into_raw(obj);
        }
    
        3 => {
            let mut obj = Box::from_raw(Formatter as *mut TSpecializedFormatter011);
            obj.format(Instruction.as_mut().unwrap(), &mut output);
            Box::into_raw(obj);
        }
    
        4 => {
            let mut obj = Box::from_raw(Formatter as *mut TSpecializedFormatter100);
            obj.format(Instruction.as_mut().unwrap(), &mut output);
            Box::into_raw(obj);
        }
    
        5 => {
            let mut obj = Box::from_raw(Formatter as *mut TSpecializedFormatter101);
            obj.format(Instruction.as_mut().unwrap(), &mut output);
            Box::into_raw(obj);
        }
    
        6 => {
            let mut obj = Box::from_raw(Formatter as *mut TSpecializedFormatter110);
            obj.format(Instruction.as_mut().unwrap(), &mut output);
            Box::into_raw(obj);
        }
    
        7 => {
            let mut obj = Box::from_raw(Formatter as *mut TSpecializedFormatter111);
            obj.format(Instruction.as_mut().unwrap(), &mut output);
            Box::into_raw(obj);
        }   

        _ => {
            //let mut obj = Box::from_raw( Formatter );
            //obj.format( Instruction.as_mut().unwrap(), &mut output );
            //Box::into_raw( obj ); 
            return;
        }        
    }

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

// NOTE: Specialized Formatter only supports the following Options
// Options

// Always show the size of memory operands 
//
// Default | Value | Example | Example
// --------|-------|---------|--------
// _ | `true` | `mov eax,dword ptr [ ebx ]` | `add byte ptr [ eax ],0x12`
// 👍 | `false` | `mov eax,[ ebx ]` | `add byte ptr [ eax ],0x12`
#[no_mangle]
pub unsafe extern "C" fn SpecializedFormatter_GetAlwaysShowMemorySize( Formatter: *mut TSpecializedFormatter ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    let value = obj.options_mut().always_show_memory_size();

    Box::into_raw( obj );
 
    return value;
}

// Always show the size of memory operands
//
// Default | Value | Example | Example
// --------|-------|---------|--------
// _ | `true` | `mov eax,dword ptr [ ebx ]` | `add byte ptr [ eax ],0x12`
// 👍 | `false` | `mov eax,[ ebx ]` | `add byte ptr [ eax ],0x12`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn SpecializedFormatter_SetAlwaysShowMemorySize( Formatter: *mut TSpecializedFormatter, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    obj.options_mut().set_always_show_memory_size( Value );

    Box::into_raw( obj );

    return true;
}

// Use a hex prefix ( `0x` ) or a hex suffix ( `h` )
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `0x5A`
// X | `false` | `5Ah`
#[no_mangle]
pub unsafe extern "C" fn SpecializedFormatter_GetUseHexPrefix( Formatter: *mut TSpecializedFormatter ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    let value = obj.options_mut().use_hex_prefix();

    Box::into_raw( obj );
 
    return value;
}

// Use a hex prefix ( `0x` ) or a hex suffix ( `h` )
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `0x5A`
// X | `false` | `5Ah`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn SpecializedFormatter_SetUseHexPrefix( Formatter: *mut TSpecializedFormatter, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    obj.options_mut().set_use_hex_prefix( Value );

    Box::into_raw( obj );

    return true;
}