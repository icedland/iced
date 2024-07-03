/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86_rust::{Instruction, SpecializedFormatter, DefaultSpecializedFormatterTraitOptions};
use crate::SymbolResolver::{TSymbolResolver, TSymbolResolverCallback};
use crate::SpecializedFormatterTraitOptions::*;

use std::ptr::null_mut;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Specialized-Formatter
pub(crate) type
TSpecializedFormatterDefault = SpecializedFormatter<DefaultSpecializedFormatterTraitOptions>;

pub(crate) struct TSpecializedFormatter {
    pub Formatter : TSpecializedFormatterDefault,
    pub Output : String,
}  
// Creates a Specialized formatter
#[no_mangle]
pub extern "C" fn SpecializedFormatter_Create( SymbolResolver : Option<TSymbolResolverCallback>, UserData : *const usize ) -> *mut TSpecializedFormatter {
    if !SymbolResolver.is_none() {
        let symbols = Box::new( TSymbolResolver { callback:SymbolResolver, userData:UserData });

        match TSpecializedFormatterDefault::try_with_options( Some( symbols ) ) {
            Ok( value ) => return Box::into_raw( Box::new( TSpecializedFormatter { Formatter: value, Output: String::new() } ) ),
        Err( _e ) => return null_mut()
    }    
    } else {
        match TSpecializedFormatterDefault::try_with_options( None ) {
            Ok( value ) => return Box::into_raw( Box::new( TSpecializedFormatter { Formatter: value, Output: String::new() } ) ),
            Err( _e ) => return null_mut()
        }
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
*/

pub unsafe extern "C" fn SpecializedFormatter_Format( Formatter: *mut TSpecializedFormatter, Options : u8, Instruction: *mut Instruction, Output : *mut *const u8, Size : *mut usize ) {    
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

    match Options {
        0 => {
            let mut obj = Box::from_raw(Formatter as *mut TSpecializedFormatter000);
            obj.Output.clear();
            obj.Formatter.format(Instruction.as_mut().unwrap(), &mut obj.Output);
            if obj.Output.len() < 315/*MAX_FMT_INSTR_LEN*/ {
                *obj.Output.as_mut_ptr().add(obj.Output.len()) = 0;
            } else {
                let newsize = obj.Output.len()+1;
                obj.Output.as_mut_vec().resize( newsize, 0 );    
            }            
            (*Output) = obj.Output.as_ptr();
            (*Size) = obj.Output.len();
            Box::into_raw(obj);
        }

        1 => {
            let mut obj = Box::from_raw(Formatter as *mut TSpecializedFormatter001);
            obj.Output.clear();
            obj.Formatter.format(Instruction.as_mut().unwrap(), &mut obj.Output);
            if obj.Output.len() < 315/*MAX_FMT_INSTR_LEN*/ {
                *obj.Output.as_mut_ptr().add(obj.Output.len()) = 0;
            } else {
                let newsize = obj.Output.len()+1;
                obj.Output.as_mut_vec().resize( newsize, 0 );    
            }            
            (*Output) = obj.Output.as_ptr();
            (*Size) = obj.Output.len();
            Box::into_raw(obj);
        }
    
        2 => {
            let mut obj = Box::from_raw(Formatter as *mut TSpecializedFormatter010);
            obj.Output.clear();
            obj.Formatter.format(Instruction.as_mut().unwrap(), &mut obj.Output);
            (*Output) = obj.Output.as_ptr();
            (*Size) = obj.Output.len();
            *obj.Output.as_mut_ptr().add(obj.Output.len()) = 0;
            Box::into_raw(obj);
        }
    
        3 => {
            let mut obj = Box::from_raw(Formatter as *mut TSpecializedFormatter011);
            obj.Output.clear();
            obj.Formatter.format(Instruction.as_mut().unwrap(), &mut obj.Output);
            if obj.Output.len() < 315/*MAX_FMT_INSTR_LEN*/ {
                *obj.Output.as_mut_ptr().add(obj.Output.len()) = 0;
            } else {
                let newsize = obj.Output.len()+1;
                obj.Output.as_mut_vec().resize( newsize, 0 );    
            }            
            (*Output) = obj.Output.as_ptr();
            (*Size) = obj.Output.len();
            Box::into_raw(obj);
        }
    
        4 => {
            let mut obj = Box::from_raw(Formatter as *mut TSpecializedFormatter100);
            obj.Output.clear();
            obj.Formatter.format(Instruction.as_mut().unwrap(), &mut obj.Output);
            if obj.Output.len() < 315/*MAX_FMT_INSTR_LEN*/ {
                *obj.Output.as_mut_ptr().add(obj.Output.len()) = 0;
            } else {
                let newsize = obj.Output.len()+1;
                obj.Output.as_mut_vec().resize( newsize, 0 );    
            }            
            (*Output) = obj.Output.as_ptr();
            (*Size) = obj.Output.len();
            Box::into_raw(obj);
        }
    
        5 => {
            let mut obj = Box::from_raw(Formatter as *mut TSpecializedFormatter101);
            obj.Output.clear();
            obj.Formatter.format(Instruction.as_mut().unwrap(), &mut obj.Output);
            if obj.Output.len() < 315/*MAX_FMT_INSTR_LEN*/ {
                *obj.Output.as_mut_ptr().add(obj.Output.len()) = 0;
            } else {
                let newsize = obj.Output.len()+1;
                obj.Output.as_mut_vec().resize( newsize, 0 );    
            }            
            (*Output) = obj.Output.as_ptr();
            (*Size) = obj.Output.len();
            Box::into_raw(obj);
        }
    
        6 => {
            let mut obj = Box::from_raw(Formatter as *mut TSpecializedFormatter110);
            obj.Output.clear();
            obj.Formatter.format(Instruction.as_mut().unwrap(), &mut obj.Output);
            if obj.Output.len() < 315/*MAX_FMT_INSTR_LEN*/ {
                *obj.Output.as_mut_ptr().add(obj.Output.len()) = 0;
            } else {
                let newsize = obj.Output.len()+1;
                obj.Output.as_mut_vec().resize( newsize, 0 );    
            }            
            (*Output) = obj.Output.as_ptr();
            (*Size) = obj.Output.len();
            Box::into_raw(obj);
        }
    
        7 => {
            let mut obj = Box::from_raw(Formatter as *mut TSpecializedFormatter111);
            obj.Output.clear();
            obj.Formatter.format(Instruction.as_mut().unwrap(), &mut obj.Output);
            if obj.Output.len() < 315/*MAX_FMT_INSTR_LEN*/ {
                *obj.Output.as_mut_ptr().add(obj.Output.len()) = 0;
            } else {
                let newsize = obj.Output.len()+1;
                obj.Output.as_mut_vec().resize( newsize, 0 );    
            }
            (*Output) = obj.Output.as_ptr();
            (*Size) = obj.Output.len();
            Box::into_raw(obj);
        }   

        _ => {
            //let mut obj = Box::from_raw( Formatter );
            //obj.Output.clear();
            //obj.Formatter.format( Instruction.as_mut().unwrap(), &mut obj.Output );
            //*obj.Output.as_mut_ptr().add(obj.Output.len()) = 0;
            //Box::into_raw( obj ); 
            return;
        }        
    }
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

    let value = obj.Formatter.options_mut().always_show_memory_size();

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

    obj.Formatter.options_mut().set_always_show_memory_size( Value );

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

    let value = obj.Formatter.options_mut().use_hex_prefix();

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

    obj.Formatter.options_mut().set_use_hex_prefix( Value );

    Box::into_raw( obj );

    return true;
}