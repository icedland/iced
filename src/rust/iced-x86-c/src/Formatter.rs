/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86::{Instruction, Formatter, MasmFormatter, NasmFormatter, GasFormatter, IntelFormatter};
use crate::OutputCallback::TFormatterOutput;
use crate::FastFormatter::TFastFormatter;
use crate::SpecializedFormatter::TSpecializedFormatter;
use std::{slice, str};
use libc::{c_char, strlen};
use std::mem::transmute;// Enum<->Int

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Formatter Options
enum TFormatterType {
    Masm,
    Nasm,
    Gas,
    Intel,
    Fast,
    Specialized,
    Capstone
}

#[no_mangle]
pub unsafe extern "C" fn Formatter_Format( Formatter: *mut MasmFormatter, FormatterType : u8, Instruction: *mut Instruction, Output : *mut u8, Size : usize ) {     
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
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.format( Instruction.as_mut().unwrap(), &mut output );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.format( Instruction.as_mut().unwrap(), &mut output );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.format( Instruction.as_mut().unwrap(), &mut output );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.format( Instruction.as_mut().unwrap(), &mut output );
            Box::into_raw( obj );
        }
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.format( Instruction.as_mut().unwrap(), &mut output );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.format( Instruction.as_mut().unwrap(), &mut output );
            Box::into_raw( obj );
        }
//        _ => { return; }
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

#[no_mangle]
pub unsafe extern "C" fn Formatter_FormatCallback( Formatter: *mut MasmFormatter, FormatterType : u8, Instruction: *mut Instruction, FormatterOutput : *mut TFormatterOutput ) {     
    if Formatter.is_null() {
        return;
    }
    if Instruction.is_null() {
        return;
    }
    if FormatterOutput.is_null() {
        return;
    }       
    
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            let mut output = Box::from_raw( FormatterOutput );
            obj.format( Instruction.as_mut().unwrap(), output.as_mut() );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            let mut output = Box::from_raw( FormatterOutput );
            obj.format( Instruction.as_mut().unwrap(), output.as_mut() );
            Box::into_raw( obj );
            Box::into_raw( output );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            let mut output = Box::from_raw( FormatterOutput );
            obj.format( Instruction.as_mut().unwrap(), output.as_mut() );
            Box::into_raw( obj );
            Box::into_raw( output );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            let mut output = Box::from_raw( FormatterOutput );
            obj.format( Instruction.as_mut().unwrap(), output.as_mut() );
            Box::into_raw( obj );
            Box::into_raw( output );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            let mut output = Box::from_raw( FormatterOutput );
            obj.format( Instruction.as_mut().unwrap(), output.as_mut() );
            Box::into_raw( obj );
            Box::into_raw( output );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            let mut output = Box::from_raw( FormatterOutput );
            obj.format( Instruction.as_mut().unwrap(), output.as_mut() );
            Box::into_raw( obj );
            Box::into_raw( output );
        }
 */
        _ => { return; }
    }    
}

// Prefixes are uppercased
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `REP stosd`
// 👍 | `false` | `rep stosd`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetUpperCasePrefixes( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;    
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().uppercase_prefixes();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().uppercase_prefixes();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().uppercase_prefixes();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().uppercase_prefixes();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().uppercase_prefixes();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().uppercase_prefixes();
            Box::into_raw( obj );
        }
 */     
        _ => { return false; }
    }
 
    return value;
}

// Prefixes are uppercased
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `REP stosd`
// 👍 | `false` | `rep stosd`
//
// # Arguments
//
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetUpperCasePrefixes( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_uppercase_prefixes( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_uppercase_prefixes( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_uppercase_prefixes( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_uppercase_prefixes( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_uppercase_prefixes( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_uppercase_prefixes( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Mnemonics are uppercased
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `MOV rcx,rax`
// 👍 | `false` | `mov rcx,rax`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetUpperCaseMnemonics( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;    
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().uppercase_mnemonics();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().uppercase_mnemonics();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().uppercase_mnemonics();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().uppercase_mnemonics();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().uppercase_mnemonics();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().uppercase_mnemonics();
            Box::into_raw( obj );
        }
 */     
        _ => { return false; }
    }
 
    return value;
}

// Mnemonics are uppercased
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `MOV rcx,rax`
// 👍 | `false` | `mov rcx,rax`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetUpperCaseMnemonics( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_uppercase_mnemonics( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_uppercase_mnemonics( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_uppercase_mnemonics( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_uppercase_mnemonics( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_uppercase_mnemonics( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_uppercase_mnemonics( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Registers are uppercased
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov RCX,[ RAX+RDX*8 ]`
// 👍 | `false` | `mov rcx,[ rax+rdx*8 ]`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetUpperCaseRegisters( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().uppercase_registers();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().uppercase_registers();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().uppercase_registers();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().uppercase_registers();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().uppercase_registers();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().uppercase_registers();
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }
 
    return value;
}

// Registers are uppercased
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov RCX,[ RAX+RDX*8 ]`
// 👍 | `false` | `mov rcx,[ rax+rdx*8 ]`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetUpperCaseRegisters( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_uppercase_registers( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_uppercase_registers( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_uppercase_registers( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_uppercase_registers( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_uppercase_registers( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_uppercase_registers( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Keywords are uppercased ( eg. `BYTE PTR`, `SHORT` )
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov BYTE PTR [ rcx ],12h`
// 👍 | `false` | `mov byte ptr [ rcx ],12h`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetUpperCaseKeyWords( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().uppercase_keywords();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().uppercase_keywords();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().uppercase_keywords();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().uppercase_keywords();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().uppercase_keywords();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().uppercase_keywords();
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }
 
    return value;
}

// Keywords are uppercased ( eg. `BYTE PTR`, `SHORT` )
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov BYTE PTR [ rcx ],12h`
// 👍 | `false` | `mov byte ptr [ rcx ],12h`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetUpperCaseKeyWords( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_uppercase_keywords( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_uppercase_keywords( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_uppercase_keywords( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_uppercase_keywords( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_uppercase_keywords( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_uppercase_keywords( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Uppercase decorators, eg. `{z}`, `{sae}`, `{rd-sae}` ( but not opmask registers: `{k1}` )
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `vunpcklps xmm2{k5}{Z},xmm6,dword bcst [ rax+4 ]`
// 👍 | `false` | `vunpcklps xmm2{k5}{z},xmm6,dword bcst [ rax+4 ]`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetUpperCaseDecorators( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => { 
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().uppercase_decorators();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().uppercase_decorators();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().uppercase_decorators();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().uppercase_decorators();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().uppercase_decorators();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().uppercase_decorators();
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }
 
    return value;
}

// Uppercase decorators, eg. `{z}`, `{sae}`, `{rd-sae}` ( but not opmask registers: `{k1}` )
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `vunpcklps xmm2{k5}{Z},xmm6,dword bcst [ rax+4 ]`
// 👍 | `false` | `vunpcklps xmm2{k5}{z},xmm6,dword bcst [ rax+4 ]`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetUpperCaseDecorators( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_uppercase_decorators( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_uppercase_decorators( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_uppercase_decorators( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_uppercase_decorators( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_uppercase_decorators( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_uppercase_decorators( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Everything is uppercased, except numbers and their prefixes/suffixes
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `MOV EAX,GS:[ RCX*4+0ffh ]`
// 👍 | `false` | `mov eax,gs:[ rcx*4+0ffh ]`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetUpperCaseEverything( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().uppercase_all();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().uppercase_all();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().uppercase_all();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().uppercase_all();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().uppercase_all();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().uppercase_all();
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }
 
    return value;
}

// Everything is uppercased, except numbers and their prefixes/suffixes
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `MOV EAX,GS:[ RCX*4+0ffh ]`
// 👍 | `false` | `mov eax,gs:[ rcx*4+0ffh ]`
//
// # Arguments
//
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetUpperCaseEverything( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_uppercase_all( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_uppercase_all( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_uppercase_all( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_uppercase_all( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_uppercase_all( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_uppercase_all( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Character index ( 0-based ) where the first operand is formatted. Can be set to 0 to format it immediately after the mnemonic.
// At least one space or tab is always added between the mnemonic and the first operand.
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `0` | `mov•rcx,rbp`
// _ | `8` | `mov•••••rcx,rbp`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetFirstOperandCharIndex( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> u32 {
    if Formatter.is_null() {
        return 0;
    }

    let value: u32;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().first_operand_char_index();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().first_operand_char_index();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().first_operand_char_index();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().first_operand_char_index();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().first_operand_char_index();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().first_operand_char_index();
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }
 
    return value;
}

// Character index ( 0-based ) where the first operand is formatted. Can be set to 0 to format it immediately after the mnemonic.
// At least one space or tab is always added between the mnemonic and the first operand.
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `0` | `mov•rcx,rbp`
// _ | `8` | `mov•••••rcx,rbp`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetFirstOperandCharIndex( Formatter: *mut MasmFormatter, FormatterType : u8, Value : u32 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_first_operand_char_index( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_first_operand_char_index( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_first_operand_char_index( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_first_operand_char_index( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_first_operand_char_index( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_first_operand_char_index( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Size of a tab character or 0 to use spaces
//
// - Default: `0`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetTabSize( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> u32 {
    if Formatter.is_null() {
        return 0;
    }

    let value: u32;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().tab_size();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().tab_size();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().tab_size();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().tab_size();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().tab_size();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().tab_size();
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }
 
    return value;
}

// Size of a tab character or 0 to use spaces
//
// - Default: `0`
//
// # Arguments
//
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetTabSize( Formatter: *mut MasmFormatter, FormatterType : u8, Value : u32 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_tab_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_tab_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_tab_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_tab_size( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_tab_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_tab_size( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Add a space after the operand separator
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov rax, rcx`
// 👍 | `false` | `mov rax,rcx`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetSpaceAfterOperandSeparator( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().space_after_operand_separator();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().space_after_operand_separator();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().space_after_operand_separator();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().space_after_operand_separator();
            Box::into_raw( obj );
        }
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().space_after_operand_separator();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().space_after_operand_separator();
            Box::into_raw( obj );
        }
//        _ => { return false; }
    }
 
    return value;
}

// Add a space after the operand separator
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov rax, rcx`
// 👍 | `false` | `mov rax,rcx`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetSpaceAfterOperandSeparator( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_space_after_operand_separator( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_space_after_operand_separator( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_space_after_operand_separator( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_space_after_operand_separator( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_space_after_operand_separator( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_space_after_operand_separator( Value );
            Box::into_raw( obj );
        }
//        _ => { return false; }
    }

    return true;
}

// Add a space between the memory expression and the brackets
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,[rcx+rdx ]`
// 👍 | `false` | `mov eax,[ rcx+rdx ]`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetSpaceAfterMemoryBracket( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().space_after_memory_bracket();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().space_after_memory_bracket();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().space_after_memory_bracket();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().space_after_memory_bracket();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().space_after_memory_bracket();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().space_after_memory_bracket();
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }
 
    return value;
}

// Add a space between the memory expression and the brackets
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,[rcx+rdx ]`
// 👍 | `false` | `mov eax,[ rcx+rdx ]`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetSpaceAfterMemoryBracket( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_space_after_memory_bracket( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_space_after_memory_bracket( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_space_after_memory_bracket( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_space_after_memory_bracket( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_space_after_memory_bracket( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_space_after_memory_bracket( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Add spaces between memory operand `+` and `-` operators
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,[ rcx + rdx*8 - 80h ]`
// 👍 | `false` | `mov eax,[ rcx+rdx*8-80h ]`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetSpaceBetweenMemoryAddOperators( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().space_between_memory_add_operators();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().space_between_memory_add_operators();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().space_between_memory_add_operators();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().space_between_memory_add_operators();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().space_between_memory_add_operators();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().space_between_memory_add_operators();
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }
 
    return value;
}

// Add spaces between memory operand `+` and `-` operators
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,[ rcx + rdx*8 - 80h ]`
// 👍 | `false` | `mov eax,[ rcx+rdx*8-80h ]`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetSpaceBetweenMemoryAddOperators( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_space_between_memory_add_operators( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_space_between_memory_add_operators( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_space_between_memory_add_operators( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_space_between_memory_add_operators( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_space_between_memory_add_operators( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_space_between_memory_add_operators( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Add spaces between memory operand `*` operator
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,[ rcx+rdx * 8-80h ]`
// 👍 | `false` | `mov eax,[ rcx+rdx*8-80h ]`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetSpaceBetweenMemoryMulOperators( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().space_between_memory_mul_operators();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().space_between_memory_mul_operators();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().space_between_memory_mul_operators();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().space_between_memory_mul_operators();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().space_between_memory_mul_operators();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().space_between_memory_mul_operators();
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }
 
    return value;
}

// Add spaces between memory operand `*` operator
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,[ rcx+rdx * 8-80h ]`
// 👍 | `false` | `mov eax,[ rcx+rdx*8-80h ]`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetSpaceBetweenMemoryMulOperators( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_space_between_memory_mul_operators( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_space_between_memory_mul_operators( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_space_between_memory_mul_operators( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_space_between_memory_mul_operators( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_space_between_memory_mul_operators( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_space_between_memory_mul_operators( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Show memory operand scale value before the index register
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,[ 8*rdx ]`
// 👍 | `false` | `mov eax,[ rdx*8 ]`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetScaleBeforeIndex( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().scale_before_index();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().scale_before_index();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().scale_before_index();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().scale_before_index();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().scale_before_index();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().scale_before_index();
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }
 
    return value;
}

// Show memory operand scale value before the index register
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,[ 8*rdx ]`
// 👍 | `false` | `mov eax,[ rdx*8 ]`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetScaleBeforeIndex( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_scale_before_index( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_scale_before_index( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_scale_before_index( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_scale_before_index( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_scale_before_index( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_scale_before_index( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Always show the scale value even if it's `*1`
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,[ rbx+rcx*1 ]`
// 👍 | `false` | `mov eax,[ rbx+rcx ]`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetAlwaysShowScale( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().always_show_scale();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().always_show_scale();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().always_show_scale();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().always_show_scale();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().always_show_scale();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().always_show_scale();
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }
 
    return value;
}

// Always show the scale value even if it's `*1`
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,[ rbx+rcx*1 ]`
// 👍 | `false` | `mov eax,[ rbx+rcx ]`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetAlwaysShowScale( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_always_show_scale( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_always_show_scale( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_always_show_scale( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_always_show_scale( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_always_show_scale( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_always_show_scale( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Always show the effective segment register. If the option is `false`, only show the segment register if
// there's a segment override prefix.
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,ds:[ ecx ]`
// 👍 | `false` | `mov eax,[ ecx ]`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetAlwaysShowSegmentRegister( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().always_show_segment_register();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().always_show_segment_register();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().always_show_segment_register();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().always_show_segment_register();
            Box::into_raw( obj );
        }
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().always_show_segment_register();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().always_show_segment_register();
            Box::into_raw( obj );
        }
//        _ => { return false; }
    }
 
    return value;
}

// Always show the effective segment register. If the option is `false`, only show the segment register if
// there's a segment override prefix.
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,ds:[ ecx ]`
// 👍 | `false` | `mov eax,[ ecx ]`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetAlwaysShowSegmentRegister( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_always_show_segment_register( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_always_show_segment_register( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_always_show_segment_register( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_always_show_segment_register( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_always_show_segment_register( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_always_show_segment_register( Value );
            Box::into_raw( obj );
        }
//        _ => { return false; }
    }

    return true;
}

// Show zero displacements
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,[ rcx*2+0 ]`
// 👍 | `false` | `mov eax,[ rcx*2 ]`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetShowZeroDisplacements( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().show_zero_displacements();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().show_zero_displacements();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().show_zero_displacements();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().show_zero_displacements();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().show_zero_displacements();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().show_zero_displacements();
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }
 
    return value;
}

// Show zero displacements
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,[ rcx*2+0 ]`
// 👍 | `false` | `mov eax,[ rcx*2 ]`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetShowZeroDisplacements( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_show_zero_displacements( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_show_zero_displacements( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_show_zero_displacements( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_show_zero_displacements( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_show_zero_displacements( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_show_zero_displacements( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Hex number prefix or an empty string, eg. `"0x"`
//
// - Default: `""` ( masm/nasm/intel ), `"0x"` ( gas )
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetHexPrefix( Formatter: *mut MasmFormatter, FormatterType: u8, Value : *mut u8, Size : usize ) -> usize {    
    if Formatter.is_null() {
        return 0;
    }
    if Value.is_null() {
        return 0;
    }

    if Size <= 0 {
        return 0
    }

    let mut l: usize;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            let tmp = obj.options_mut().hex_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            let tmp = obj.options_mut().hex_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            let tmp = obj.options_mut().hex_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            let tmp = obj.options_mut().hex_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            let tmp = obj.options_mut().hex_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            let tmp = obj.options_mut().hex_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }

    return l;
}

// Hex number prefix or an empty string, eg. `"0x"`
//
// - Default: `""` ( masm/nasm/intel ), `"0x"` ( gas )
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetHexPrefix( Formatter: *mut MasmFormatter, FormatterType : u8, Value : *const c_char ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value = {
        let c_s = Value;
        str::from_utf8_unchecked( slice::from_raw_parts( c_s as *const u8, strlen( c_s ) ) ).to_owned()
    };

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_hex_prefix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_hex_prefix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_hex_prefix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_hex_prefix_string( value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_hex_prefix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_hex_prefix_string( value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Hex number suffix or an empty string, eg. `"h"`
//
// - Default: `"h"` ( masm/nasm/intel ), `""` ( gas )
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetHexSuffix( Formatter: *mut MasmFormatter, FormatterType: u8, Value : *mut u8, Size : usize ) -> usize {
    if Formatter.is_null() {
        return 0;
    }
    if Value.is_null() {
        return 0;
    }

    if Size <= 0 {
        return 0
    }

    let mut l: usize;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            let tmp = obj.options_mut().hex_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            let tmp = obj.options_mut().hex_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            let tmp = obj.options_mut().hex_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            let tmp = obj.options_mut().hex_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            let tmp = obj.options_mut().hex_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            let tmp = obj.options_mut().hex_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }

    return l;
}

// Hex number suffix or an empty string, eg. `"h"`
//
// - Default: `"h"` ( masm/nasm/intel ), `""` ( gas )
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetHexSuffix( Formatter: *mut MasmFormatter, FormatterType : u8, Value : *const c_char ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value = {
        let c_s = Value;
        str::from_utf8_unchecked( slice::from_raw_parts( c_s as *const u8, strlen( c_s ) ) ).to_owned()
    };

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_hex_suffix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_hex_suffix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_hex_suffix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_hex_suffix_string( value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_hex_suffix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_hex_suffix_string( value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Size of a digit group, see also [ `digit_separator()` ]
//
// [ `digit_separator()` ]: #method.digit_separator
//
// Default | Value | Example
// --------|-------|--------
// _ | `0` | `0x12345678`
// 👍 | `4` | `0x1234_5678`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetHexDigitGroupSize( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> u32 {
    if Formatter.is_null() {
        return 0;
    }

    let value: u32;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().hex_digit_group_size();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().hex_digit_group_size();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().hex_digit_group_size();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().hex_digit_group_size();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().hex_digit_group_size();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().hex_digit_group_size();
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }
 
    return value;
}

// Size of a digit group, see also [ `digit_separator()` ]
//
// [ `digit_separator()` ]: #method.digit_separator
//
// Default | Value | Example
// --------|-------|--------
// _ | `0` | `0x12345678`
// 👍 | `4` | `0x1234_5678`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetHexDigitGroupSize( Formatter: *mut MasmFormatter, FormatterType : u8, Value : u32 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_hex_digit_group_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_hex_digit_group_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_hex_digit_group_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_hex_digit_group_size( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_hex_digit_group_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_hex_digit_group_size( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Decimal number prefix or an empty string
//
// - Default: `""`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetDecimalPrefix( Formatter: *mut MasmFormatter, FormatterType: u8, Value : *mut u8, Size : usize ) -> usize {
    if Formatter.is_null() {
        return 0;
    }
    if Value.is_null() {
        return 0;
    }

    if Size <= 0 {
        return 0
    }

    let mut l: usize;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            let tmp = obj.options_mut().decimal_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            let tmp = obj.options_mut().decimal_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            let tmp = obj.options_mut().decimal_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            let tmp = obj.options_mut().decimal_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            let tmp = obj.options_mut().decimal_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            let tmp = obj.options_mut().decimal_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }

    return l;
}

// Decimal number prefix or an empty string
//
// - Default: `""`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetDecimalPrefix( Formatter: *mut MasmFormatter, FormatterType : u8, Value : *const c_char ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value = {
        let c_s = Value;
        str::from_utf8_unchecked( slice::from_raw_parts( c_s as *const u8, strlen( c_s ) ) ).to_owned()
    };

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_decimal_prefix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_decimal_prefix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_decimal_prefix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_decimal_prefix_string( value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_decimal_prefix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_decimal_prefix_string( value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Decimal number suffix or an empty string
//
// - Default: `""`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetDecimalSuffix( Formatter: *mut MasmFormatter, FormatterType: u8, Value : *mut u8, Size : usize ) -> usize {
    if Formatter.is_null() {
        return 0;
    }
    if Value.is_null() {
        return 0;
    }

    if Size <= 0 {
        return 0
    }

    let mut l: usize;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            let tmp = obj.options_mut().decimal_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            let tmp = obj.options_mut().decimal_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            let tmp = obj.options_mut().decimal_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            let tmp = obj.options_mut().decimal_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            let tmp = obj.options_mut().decimal_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            let tmp = obj.options_mut().decimal_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }

    return l;
}

// Decimal number suffix or an empty string
//
// - Default: `""`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetDecimalSuffix( Formatter: *mut MasmFormatter, FormatterType : u8, Value : *const c_char ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value = {
        let c_s = Value;
        str::from_utf8_unchecked( slice::from_raw_parts( c_s as *const u8, strlen( c_s ) ) ).to_owned()
    };

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_decimal_suffix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_decimal_suffix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_decimal_suffix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_decimal_suffix_string( value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_decimal_suffix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_decimal_suffix_string( value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Size of a digit group, see also [ `digit_separator()` ]
//
// [ `digit_separator()` ]: #method.digit_separator
//
// Default | Value | Example
// --------|-------|--------
// _ | `0` | `12345678`
// 👍 | `3` | `12_345_678`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetDecimalDigitGroupSize( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> u32 {
    if Formatter.is_null() {
        return 0;
    }

    let value: u32;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().decimal_digit_group_size();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().decimal_digit_group_size();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().decimal_digit_group_size();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().decimal_digit_group_size();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().decimal_digit_group_size();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().decimal_digit_group_size();
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }

    return value;
}

// Size of a digit group, see also [ `digit_separator()` ]
//
// [ `digit_separator()` ]: #method.digit_separator
//
// Default | Value | Example
// --------|-------|--------
// _ | `0` | `12345678`
// 👍 | `3` | `12_345_678`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetDecimalDigitGroupSize( Formatter: *mut MasmFormatter, FormatterType : u8, Value : u32 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_decimal_digit_group_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_decimal_digit_group_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_decimal_digit_group_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_decimal_digit_group_size( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_decimal_digit_group_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_decimal_digit_group_size( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Octal number prefix or an empty string
//
// - Default: `""` ( masm/nasm/intel ), `"0"` ( gas )
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetOctalPrefix( Formatter: *mut MasmFormatter, FormatterType: u8, Value : *mut u8, Size : usize ) -> usize {
    if Formatter.is_null() {
        return 0;
    }
    if Value.is_null() {
        return 0;
    }

    if Size <= 0 {
        return 0
    }

    let mut l: usize;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            let tmp = obj.options_mut().octal_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            let tmp = obj.options_mut().octal_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            let tmp = obj.options_mut().octal_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            let tmp = obj.options_mut().octal_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            let tmp = obj.options_mut().octal_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            let tmp = obj.options_mut().octal_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }

    return l;
}

// Octal number prefix or an empty string
//
// - Default: `""` ( masm/nasm/intel ), `"0"` ( gas )
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetOctalPrefix( Formatter: *mut MasmFormatter, FormatterType : u8, Value : *const c_char ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value = {
        let c_s = Value;
        str::from_utf8_unchecked( slice::from_raw_parts( c_s as *const u8, strlen( c_s ) ) ).to_owned()
    };

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_octal_prefix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_octal_prefix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_octal_prefix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_octal_prefix_string( value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_octal_prefix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_octal_prefix_string( value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Octal number suffix or an empty string
//
// - Default: `"o"` ( masm/nasm/intel ), `""` ( gas )
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetOctalSuffix( Formatter: *mut MasmFormatter, FormatterType: u8, Value : *mut u8, Size : usize ) -> usize {
    if Formatter.is_null() {
        return 0;
    }
    if Value.is_null() {
        return 0;
    }

    if Size <= 0 {
        return 0
    }

    let mut l: usize;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            let tmp = obj.options_mut().octal_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            let tmp = obj.options_mut().octal_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            let tmp = obj.options_mut().octal_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            let tmp = obj.options_mut().octal_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            let tmp = obj.options_mut().octal_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            let tmp = obj.options_mut().octal_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }

    return l;
}

// Octal number suffix or an empty string
//
// - Default: `"o"` ( masm/nasm/intel ), `""` ( gas )
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetOctalSuffix( Formatter: *mut MasmFormatter, FormatterType : u8, Value : *const c_char ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value = {
        let c_s = Value;
        str::from_utf8_unchecked( slice::from_raw_parts( c_s as *const u8, strlen( c_s ) ) ).to_owned()
    };

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_octal_suffix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_octal_suffix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_octal_suffix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_octal_suffix_string( value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_octal_suffix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_octal_suffix_string( value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Size of a digit group, see also [ `digit_separator()` ]
//
// [ `digit_separator()` ]: #method.digit_separator
//
// Default | Value | Example
// --------|-------|--------
// _ | `0` | `12345670`
// 👍 | `4` | `1234_5670`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetOctalDigitGroupSize( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> u32 {
    if Formatter.is_null() {
        return 0;
    }

    let value: u32;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().octal_digit_group_size();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().octal_digit_group_size();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().octal_digit_group_size();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().octal_digit_group_size();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().octal_digit_group_size();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().octal_digit_group_size();
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }
 
    return value;
}

// Size of a digit group, see also [ `digit_separator()` ]
//
// [ `digit_separator()` ]: #method.digit_separator
//
// Default | Value | Example
// --------|-------|--------
// _ | `0` | `12345670`
// 👍 | `4` | `1234_5670`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetOctalDigitGroupSize( Formatter: *mut MasmFormatter, FormatterType : u8, Value : u32 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_octal_digit_group_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_octal_digit_group_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_octal_digit_group_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_octal_digit_group_size( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_octal_digit_group_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_octal_digit_group_size( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Binary number prefix or an empty string
//
// - Default: `""` ( masm/nasm/intel ), `"0b"` ( gas )
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetBinaryPrefix( Formatter: *mut MasmFormatter, FormatterType: u8, Value : *mut u8, Size : usize ) -> usize {
    if Formatter.is_null() {
        return 0;
    }
    if Value.is_null() {
        return 0;
    }

    if Size <= 0 {
        return 0
    }

    let mut l: usize;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            let tmp = obj.options_mut().binary_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            let tmp = obj.options_mut().binary_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            let tmp = obj.options_mut().binary_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            let tmp = obj.options_mut().binary_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            let tmp = obj.options_mut().binary_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            let tmp = obj.options_mut().binary_prefix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }

    return l;
}

// Binary number prefix or an empty string
//
// - Default: `""` ( masm/nasm/intel ), `"0b"` ( gas )
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetBinaryPrefix( Formatter: *mut MasmFormatter, FormatterType : u8, Value : *const c_char ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value = {
        let c_s = Value;
        str::from_utf8_unchecked( slice::from_raw_parts( c_s as *const u8, strlen( c_s ) ) ).to_owned()
    };

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_binary_prefix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_binary_prefix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_binary_prefix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_binary_prefix_string( value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_binary_prefix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_binary_prefix_string( value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Binary number suffix or an empty string
//
// - Default: `"b"` ( masm/nasm/intel ), `""` ( gas )
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetBinarySuffix( Formatter: *mut MasmFormatter, FormatterType: u8, Value : *mut u8, Size : usize ) -> usize {
    if Formatter.is_null() {
        return 0;
    }
    if Value.is_null() {
        return 0;
    }

    if Size <= 0 {
        return 0
    }

    let mut l: usize;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            let tmp = obj.options_mut().binary_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            let tmp = obj.options_mut().binary_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            let tmp = obj.options_mut().binary_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            let tmp = obj.options_mut().binary_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            let tmp = obj.options_mut().binary_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            let tmp = obj.options_mut().binary_suffix();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }

    return l;
}

// Binary number suffix or an empty string
//
// - Default: `"b"` ( masm/nasm/intel ), `""` ( gas )
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetBinarySuffix( Formatter: *mut MasmFormatter, FormatterType : u8, Value : *const c_char ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value = {
        let c_s = Value;
        str::from_utf8_unchecked( slice::from_raw_parts( c_s as *const u8, strlen( c_s ) ) ).to_owned()
    };

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_binary_suffix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_binary_suffix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_binary_suffix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_binary_suffix_string( value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_binary_suffix_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_binary_suffix_string( value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Size of a digit group, see also [ `digit_separator()` ]
//
// [ `digit_separator()` ]: #method.digit_separator
//
// Default | Value | Example
// --------|-------|--------
// _ | `0` | `11010111`
// 👍 | `4` | `1101_0111`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetBinaryDigitGroupSize( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> u32 {
    if Formatter.is_null() {
        return 0;
    }

    let value: u32;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().binary_digit_group_size();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().binary_digit_group_size();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().binary_digit_group_size();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().binary_digit_group_size();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().binary_digit_group_size();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().binary_digit_group_size();
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }
 
    return value;
}

// Size of a digit group, see also [ `digit_separator()` ]
//
// [ `digit_separator()` ]: #method.digit_separator
//
// Default | Value | Example
// --------|-------|--------
// _ | `0` | `11010111`
// 👍 | `4` | `1101_0111`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetBinaryDigitGroupSize( Formatter: *mut MasmFormatter, FormatterType : u8, Value : u32 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_binary_digit_group_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_binary_digit_group_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_binary_digit_group_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_binary_digit_group_size( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_binary_digit_group_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_binary_digit_group_size( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Digit separator or an empty string. See also eg. [ `hex_digit_group_size()` ]
//
// [ `hex_digit_group_size()` ]: #method.hex_digit_group_size
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `""` | `0x12345678`
// _ | `"_"` | `0x1234_5678`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetDigitSeparator( Formatter: *mut MasmFormatter, FormatterType: u8, Value : *mut u8, Size : usize ) -> usize {
    if Formatter.is_null() {
        return 0;
    }
    if Value.is_null() {
        return 0;
    }

    if Size <= 0 {
        return 0
    }

    let mut l: usize;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            let tmp = obj.options_mut().digit_separator();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            let tmp = obj.options_mut().digit_separator();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            let tmp = obj.options_mut().digit_separator();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            let tmp = obj.options_mut().digit_separator();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            let tmp = obj.options_mut().digit_separator();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            let tmp = obj.options_mut().digit_separator();
            l = tmp.len();
            if l > Size {
                l = Size;
            }
            
            if l > 0 {
                for i in 0..l {
                    *( Value.add( i ) ) = tmp.as_bytes()[ i ];
                }
            }
            *( Value.add( l ) ) = 0;
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }

    return l;
}

// Digit separator or an empty string. See also eg. [ `hex_digit_group_size()` ]
//
// [ `hex_digit_group_size()` ]: #method.hex_digit_group_size
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `""` | `0x12345678`
// _ | `"_"` | `0x1234_5678`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetDigitSeparator( Formatter: *mut MasmFormatter, FormatterType : u8, Value : *const c_char ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value = {
        let c_s = Value;
        str::from_utf8_unchecked( slice::from_raw_parts( c_s as *const u8, strlen( c_s ) ) ).to_owned()
    };

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_digit_separator_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_digit_separator_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_digit_separator_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_digit_separator_string( value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_digit_separator_string( value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_digit_separator_string( value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Add leading zeros to hexadecimal/octal/binary numbers.
// This option has no effect on branch targets and displacements, use [ `branch_leading_zeros` ]
// and [ `displacement_leading_zeros` ].
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `0x0000000A`/`0000000Ah`
// 👍 | `false` | `0xA`/`0Ah`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetLeadingZeros( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().leading_zeros();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().leading_zeros();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().leading_zeros();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().leading_zeros();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().leading_zeros();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().leading_zeros();
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }
 
    return value;
}

// Add leading zeros to hexadecimal/octal/binary numbers.
// This option has no effect on branch targets and displacements, use [ `branch_leading_zeros` ]
// and [ `displacement_leading_zeros` ].
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `0x0000000A`/`0000000Ah`
// 👍 | `false` | `0xA`/`0Ah`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetLeadingZeros( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_leading_zeros( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_leading_zeros( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_leading_zeros( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_leading_zeros( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_leading_zeros( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_leading_zeros( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Use uppercase hex digits
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `true` | `0xFF`
// _ | `false` | `0xff`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetUppercaseHex( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().uppercase_hex();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().uppercase_hex();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().uppercase_hex();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().uppercase_hex();
            Box::into_raw( obj );
        }
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().uppercase_hex();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().uppercase_hex();
            Box::into_raw( obj );
        }
//        _ => { return false; }
    }
 
    return value;
}

// Use uppercase hex digits
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `true` | `0xFF`
// _ | `false` | `0xff`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetUppercaseHex( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_uppercase_hex( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_uppercase_hex( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_uppercase_hex( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_uppercase_hex( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_uppercase_hex( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_uppercase_hex( Value );
            Box::into_raw( obj );
        }
//        _ => { return false; }
    }

    return true;
}

// Small hex numbers ( -9 .. 9 ) are shown in decimal
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `true` | `9`
// _ | `false` | `0x9`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetSmallHexNumbersInDecimal( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().small_hex_numbers_in_decimal();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().small_hex_numbers_in_decimal();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().small_hex_numbers_in_decimal();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().small_hex_numbers_in_decimal();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().small_hex_numbers_in_decimal();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().small_hex_numbers_in_decimal();
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }
 
    return value;
}

// Small hex numbers ( -9 .. 9 ) are shown in decimal
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `true` | `9`
// _ | `false` | `0x9`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetSmallHexNumbersInDecimal( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_small_hex_numbers_in_decimal( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_small_hex_numbers_in_decimal( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_small_hex_numbers_in_decimal( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_small_hex_numbers_in_decimal( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_small_hex_numbers_in_decimal( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_small_hex_numbers_in_decimal( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Add a leading zero to hex numbers if there's no prefix and the number starts with hex digits `A-F`
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `true` | `0FFh`
// _ | `false` | `FFh`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetAddLeadingZeroToHexNumbers( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().add_leading_zero_to_hex_numbers();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().add_leading_zero_to_hex_numbers();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().add_leading_zero_to_hex_numbers();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().add_leading_zero_to_hex_numbers();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().add_leading_zero_to_hex_numbers();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().add_leading_zero_to_hex_numbers();
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }
 
    return value;
}

// Add a leading zero to hex numbers if there's no prefix and the number starts with hex digits `A-F`
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `true` | `0FFh`
// _ | `false` | `FFh`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetAddLeadingZeroToHexNumbers( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_add_leading_zero_to_hex_numbers( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_add_leading_zero_to_hex_numbers( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_add_leading_zero_to_hex_numbers( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_add_leading_zero_to_hex_numbers( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_add_leading_zero_to_hex_numbers( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_add_leading_zero_to_hex_numbers( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Number base
//
// - Default: [ `Hexadecimal` ]
//
// [ `Hexadecimal` ]: enum.NumberBase.html#variant.Hexadecimal
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetNumberBase( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> u32 { // FFI-Unsafe: NumberBase
    if Formatter.is_null() {
        return 0;// NumberBase::Hexadecimal;
    }

    let value: u32/*TNumberBase*/;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = transmute( obj.options_mut().number_base() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = transmute( obj.options_mut().number_base() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = transmute( obj.options_mut().number_base() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = transmute( obj.options_mut().number_base() as u32 );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = transmute( obj.options_mut().number_base() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = transmute( obj.options_mut().number_base() as u32 );
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }    
   
    return value;
}

// Number base
//
// - Default: [ `Hexadecimal` ]
//
// [ `Hexadecimal` ]: enum.NumberBase.html#variant.Hexadecimal
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetNumberBase( Formatter: *mut MasmFormatter, FormatterType : u8, Value : u32 /*NumberBase*/ ) -> bool { // FFI-Unsafe:
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_number_base( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_number_base( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_number_base( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_number_base( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_number_base( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_number_base( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Add leading zeros to branch offsets. Used by `CALL NEAR`, `CALL FAR`, `JMP NEAR`, `JMP FAR`, `Jcc`, `LOOP`, `LOOPcc`, `XBEGIN`
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `true` | `je 00000123h`
// _ | `false` | `je 123h`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetBranchLeadingZeros( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().show_zero_displacements();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().branch_leading_zeros();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().branch_leading_zeros();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().branch_leading_zeros();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().branch_leading_zeros();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().branch_leading_zeros();
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return value;
}

// Add leading zeros to branch offsets. Used by `CALL NEAR`, `CALL FAR`, `JMP NEAR`, `JMP FAR`, `Jcc`, `LOOP`, `LOOPcc`, `XBEGIN`
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `true` | `je 00000123h`
// _ | `false` | `je 123h`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetBranchLeadingZeros( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_branch_leading_zeros( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_branch_leading_zeros( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_branch_leading_zeros( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_branch_leading_zeros( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_branch_leading_zeros( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_branch_leading_zeros( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Show immediate operands as signed numbers
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,-1`
// 👍 | `false` | `mov eax,FFFFFFFF`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetSignedImmediateOperands( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().signed_immediate_operands();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().signed_immediate_operands();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().signed_immediate_operands();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().signed_immediate_operands();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().signed_immediate_operands();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().signed_immediate_operands();
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }
 
    return value;
}

// Show immediate operands as signed numbers
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,-1`
// 👍 | `false` | `mov eax,FFFFFFFF`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetSignedImmediateOperands( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_signed_immediate_operands( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_signed_immediate_operands( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_signed_immediate_operands( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_signed_immediate_operands( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_signed_immediate_operands( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_signed_immediate_operands( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Displacements are signed numbers
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `true` | `mov al,[ eax-2000h ]`
// _ | `false` | `mov al,[ eax+0FFFFE000h ]`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetSignedMemoryDisplacements( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().signed_memory_displacements();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().signed_memory_displacements();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().signed_memory_displacements();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().signed_memory_displacements();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().signed_memory_displacements();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().signed_memory_displacements();
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }
 
    return value;
}

// Displacements are signed numbers
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `true` | `mov al,[ eax-2000h ]`
// _ | `false` | `mov al,[ eax+0FFFFE000h ]`
//
// # Arguments
//
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetSignedMemoryDisplacements( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_signed_memory_displacements( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_signed_memory_displacements( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_signed_memory_displacements( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_signed_memory_displacements( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_signed_memory_displacements( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_signed_memory_displacements( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Add leading zeros to displacements
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov al,[ eax+00000012h ]`
// 👍 | `false` | `mov al,[ eax+12h ]`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetDisplacementLeadingZeros( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().displacement_leading_zeros();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().displacement_leading_zeros();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().displacement_leading_zeros();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().displacement_leading_zeros();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().displacement_leading_zeros();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().displacement_leading_zeros();
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }
 
    return value;
}

// Add leading zeros to displacements
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov al,[ eax+00000012h ]`
// 👍 | `false` | `mov al,[ eax+12h ]`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetDisplacementLeadingZeros( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_displacement_leading_zeros( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_displacement_leading_zeros( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_displacement_leading_zeros( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_displacement_leading_zeros( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_displacement_leading_zeros( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_displacement_leading_zeros( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Options that control if the memory size ( eg. `DWORD PTR` ) is shown or not.
// This is ignored by the gas ( AT&T ) formatter.
//
// - Default: [ `Default` ]
//
// [ `Default` ]: enum.MemorySizeOptions.html#variant.Default
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetMemorySizeOptions( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> u32 { // FFI-Unsafe: MemorySizeOptions
    if Formatter.is_null() {
        return 0;// MemorySizeOptions::Default;
    }

    let value: u32;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = transmute( obj.options_mut().memory_size_options() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = transmute( obj.options_mut().memory_size_options() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = transmute( obj.options_mut().memory_size_options() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = transmute( obj.options_mut().memory_size_options() as u32 );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = transmute( obj.options_mut().memory_size_options() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = transmute( obj.options_mut().memory_size_options() as u32 );
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }

    return value;
}

// Options that control if the memory size ( eg. `DWORD PTR` ) is shown or not.
// This is ignored by the gas ( AT&T ) formatter.
//
// - Default: [ `Default` ]
//
// [ `Default` ]: enum.MemorySizeOptions.html#variant.Default
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetMemorySizeOptions( Formatter: *mut MasmFormatter, FormatterType : u8, Value : u32 /*MemorySizeOptions*/ ) -> bool { // FFI-Unsafe: MemorySizeOptions
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_memory_size_options( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_memory_size_options( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_memory_size_options( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_memory_size_options( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_memory_size_options( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_memory_size_options( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Show `RIP+displ` or the virtual address
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,[ rip+12345678h ]`
// 👍 | `false` | `mov eax,[ 1029384756AFBECDh ]`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetRipRelativeAddresses( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().rip_relative_addresses();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().rip_relative_addresses();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().rip_relative_addresses();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().rip_relative_addresses();
            Box::into_raw( obj );
        }
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().rip_relative_addresses();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().rip_relative_addresses();
            Box::into_raw( obj );
        }
//        _ => { return false; }
    }
 
    return value;
}

// Show `RIP+displ` or the virtual address
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,[ rip+12345678h ]`
// 👍 | `false` | `mov eax,[ 1029384756AFBECDh ]`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetRipRelativeAddresses( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_rip_relative_addresses( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_rip_relative_addresses( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_rip_relative_addresses( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_rip_relative_addresses( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_rip_relative_addresses( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_rip_relative_addresses( Value );
            Box::into_raw( obj );
        }
//        _ => { return false; }
    }

    return true;
}

// Show `NEAR`, `SHORT`, etc if it's a branch instruction
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `true` | `je short 1234h`
// _ | `false` | `je 1234h`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetShowBranchSize( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().show_branch_size();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().show_branch_size();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().show_branch_size();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().show_branch_size();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().show_branch_size();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().show_branch_size();
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return value;
}

// Show `NEAR`, `SHORT`, etc if it's a branch instruction
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `true` | `je short 1234h`
// _ | `false` | `je 1234h`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetShowBranchSize( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_show_branch_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_show_branch_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_show_branch_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_show_branch_size( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_show_branch_size( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_show_branch_size( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Use pseudo instructions
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `true` | `vcmpnltsd xmm2,xmm6,xmm3`
// _ | `false` | `vcmpsd xmm2,xmm6,xmm3,5`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetUsePseudoOps( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().use_pseudo_ops();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().use_pseudo_ops();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().use_pseudo_ops();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().use_pseudo_ops();
            Box::into_raw( obj );
        }
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().use_pseudo_ops();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().use_pseudo_ops();
            Box::into_raw( obj );
        }
//        _ => { return false; }
    }
 
    return value;
}

// Use pseudo instructions
//
// Default | Value | Example
// --------|-------|--------
// 👍 | `true` | `vcmpnltsd xmm2,xmm6,xmm3`
// _ | `false` | `vcmpsd xmm2,xmm6,xmm3,5`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetUsePseudoOps( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_use_pseudo_ops( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_use_pseudo_ops( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_use_pseudo_ops( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_use_pseudo_ops( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_use_pseudo_ops( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_use_pseudo_ops( Value );
            Box::into_raw( obj );
        }
//        _ => { return false; }
    }

    return true;
}

// Show the original value after the symbol name
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,[ myfield ( 12345678 ) ]`
// 👍 | `false` | `mov eax,[ myfield ]`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetShowSymbolAddress( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().show_symbol_address();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().show_symbol_address();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().show_symbol_address();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().show_symbol_address();
            Box::into_raw( obj );
        }
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().show_symbol_address();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().show_symbol_address();
            Box::into_raw( obj );
        }
//        _ => { return false; }
    }

    return value;
}

// Show the original value after the symbol name
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,[ myfield ( 12345678 ) ]`
// 👍 | `false` | `mov eax,[ myfield ]`
//
// # Arguments
//
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetShowSymbolAddress( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_show_symbol_address( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_show_symbol_address( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_show_symbol_address( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_show_symbol_address( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_show_symbol_address( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_show_symbol_address( Value );
            Box::into_raw( obj );
        }
//        _ => { return false; }
    }

    return true;
}

// Use `st( 0 )` instead of `st` if `st` can be used. Ignored by the nasm formatter.
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `fadd st( 0 ),st( 3 )`
// 👍 | `false` | `fadd st,st( 3 )`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetPreferST0( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().prefer_st0();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().prefer_st0();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().prefer_st0();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().prefer_st0();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().prefer_st0();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().prefer_st0();
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }
 
    return value;
}

// Use `st( 0 )` instead of `st` if `st` can be used. Ignored by the nasm formatter.
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `fadd st( 0 ),st( 3 )`
// 👍 | `false` | `fadd st,st( 3 )`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetPreferST0( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_prefer_st0( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_prefer_st0( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_prefer_st0( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_prefer_st0( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_prefer_st0( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_prefer_st0( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Show useless prefixes. If it has useless prefixes, it could be data and not code.
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `es rep add eax,ecx`
// 👍 | `false` | `add eax,ecx`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetShowUselessPrefixes( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let value: bool;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = obj.options_mut().show_useless_prefixes();
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = obj.options_mut().show_useless_prefixes();
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = obj.options_mut().show_useless_prefixes();
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = obj.options_mut().show_useless_prefixes();
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = obj.options_mut().show_useless_prefixes();
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = obj.options_mut().show_useless_prefixes();
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }
 
    return value;
}

// Show useless prefixes. If it has useless prefixes, it could be data and not code.
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `es rep add eax,ecx`
// 👍 | `false` | `add eax,ecx`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetShowUselessPrefixes( Formatter: *mut MasmFormatter, FormatterType : u8, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_show_useless_prefixes( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_show_useless_prefixes( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_show_useless_prefixes( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_show_useless_prefixes( Value );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_show_useless_prefixes( Value );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_show_useless_prefixes( Value );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Mnemonic condition code selector ( eg. `JB` / `JC` / `JNAE` )
//
// Default: `JB`, `CMOVB`, `SETB`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetCC_b( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> u32 { // FFI-Unsafe: CC_b 
    if Formatter.is_null() {
        return 0;// CC_b::b;
    }

    let value: u32;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = transmute( obj.options_mut().cc_b() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = transmute( obj.options_mut().cc_b() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = transmute( obj.options_mut().cc_b() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = transmute( obj.options_mut().cc_b() as u32 );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = transmute( obj.options_mut().cc_b() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = transmute( obj.options_mut().cc_b() as u32 );
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }
   
    return value;
}

// Mnemonic condition code selector ( eg. `JB` / `JC` / `JNAE` )
//
// Default: `JB`, `CMOVB`, `SETB`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetCC_b( Formatter: *mut MasmFormatter, FormatterType : u8, Value : u32/*CC_b*/ ) -> bool { // FFI-Unsafe: CC_b
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_cc_b( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_cc_b( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_cc_b( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_cc_b( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_cc_b( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_cc_b( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Mnemonic condition code selector ( eg. `JAE` / `JNB` / `JNC` )
//
// Default: `JAE`, `CMOVAE`, `SETAE`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetCC_ae( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> u32 { // FFI-Unsafe: CC_ae
    if Formatter.is_null() {
        return 0;// CC_ae::ae;
    }

    let value: u32;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = transmute( obj.options_mut().cc_ae() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = transmute( obj.options_mut().cc_ae() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = transmute( obj.options_mut().cc_ae() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = transmute( obj.options_mut().cc_ae() as u32 );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = transmute( obj.options_mut().cc_ae() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = transmute( obj.options_mut().cc_ae() as u32 );
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }
   
    return value;
}

// Mnemonic condition code selector ( eg. `JAE` / `JNB` / `JNC` )
//
// Default: `JAE`, `CMOVAE`, `SETAE`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetCC_ae( Formatter: *mut MasmFormatter, FormatterType : u8, Value : u32 ) -> bool { // FFI-Unsafe: CC_ae
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_cc_ae( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_cc_ae( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_cc_ae( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_cc_ae( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_cc_ae( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_cc_ae( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Mnemonic condition code selector ( eg. `JE` / `JZ` )
//
// Default: `JE`, `CMOVE`, `SETE`, `LOOPE`, `REPE`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetCC_e( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> u32 { // FFI-Unsafe: CC_e
    if Formatter.is_null() {
        return 0;// CC_e::e;
    }

    let value: u32;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = transmute( obj.options_mut().cc_e() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = transmute( obj.options_mut().cc_e() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = transmute( obj.options_mut().cc_e() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = transmute( obj.options_mut().cc_e() as u32 );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = transmute( obj.options_mut().cc_e() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = transmute( obj.options_mut().cc_e() as u32 );
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }
   
    return value;
}

// Mnemonic condition code selector ( eg. `JE` / `JZ` )
//
// Default: `JE`, `CMOVE`, `SETE`, `LOOPE`, `REPE`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetCC_e( Formatter: *mut MasmFormatter, FormatterType : u8, Value : u32 ) -> bool { // FFI-Unsafe: CC_e
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_cc_e( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_cc_e( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_cc_e( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_cc_e( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_cc_e( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_cc_e( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Mnemonic condition code selector ( eg. `JNE` / `JNZ` )
//
// Default: `JNE`, `CMOVNE`, `SETNE`, `LOOPNE`, `REPNE`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetCC_ne( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> u32 { // FFI-Unsafe: CC_ne
    if Formatter.is_null() {
        return 0;// CC_ne::ne;
    }

    let value: u32;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = transmute( obj.options_mut().cc_ne() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = transmute( obj.options_mut().cc_ne() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = transmute( obj.options_mut().cc_ne() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = transmute( obj.options_mut().cc_ne() as u32 );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = transmute( obj.options_mut().cc_ne() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = transmute( obj.options_mut().cc_ne() as u32 );
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }
   
    return value;
}

// Mnemonic condition code selector ( eg. `JNE` / `JNZ` )
//
// Default: `JNE`, `CMOVNE`, `SETNE`, `LOOPNE`, `REPNE`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetCC_ne( Formatter: *mut MasmFormatter, FormatterType : u8, Value : u32 ) -> bool { // FFI-Unsafe: CC_ne
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_cc_ne( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_cc_ne( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_cc_ne( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_cc_ne( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_cc_ne( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_cc_ne( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Mnemonic condition code selector ( eg. `JBE` / `JNA` )
//
// Default: `JBE`, `CMOVBE`, `SETBE`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetCC_be( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> u32 { // FFI-Unsafe: CC_be
    if Formatter.is_null() {
        return 0;// CC_be::be;
    }

    let value: u32;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = transmute( obj.options_mut().cc_be() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = transmute( obj.options_mut().cc_be() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = transmute( obj.options_mut().cc_be() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = transmute( obj.options_mut().cc_be() as u32 );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = transmute( obj.options_mut().cc_be() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = transmute( obj.options_mut().cc_be() as u32 );
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }
   
    return value;
}

// Mnemonic condition code selector ( eg. `JBE` / `JNA` )
//
// Default: `JBE`, `CMOVBE`, `SETBE`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetCC_be( Formatter: *mut MasmFormatter, FormatterType : u8, Value : u32 ) -> bool { // FFI-Unsafe: CC_be
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_cc_be( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_cc_be( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_cc_be( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_cc_be( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_cc_be( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_cc_be( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Mnemonic condition code selector ( eg. `JA` / `JNBE` )
//
// Default: `JA`, `CMOVA`, `SETA`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetCC_a( Formatter: *mut MasmFormatter, FormatterType : u8 ) -> u32 { // FFI-Unsafe: CC_a
    if Formatter.is_null() {
        return 0;// CC_a::a;
    }

    let value: u32;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = transmute( obj.options_mut().cc_a() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = transmute( obj.options_mut().cc_a() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = transmute( obj.options_mut().cc_a() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = transmute( obj.options_mut().cc_a() as u32 );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = transmute( obj.options_mut().cc_a() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = transmute( obj.options_mut().cc_a() as u32 );
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }
   
    return value;
}

// Mnemonic condition code selector ( eg. `JA` / `JNBE` )
//
// Default: `JA`, `CMOVA`, `SETA`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetCC_a( Formatter: *mut MasmFormatter, FormatterType : u8, Value : u32 ) -> bool { // FFI-Unsafe: CC_a
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_cc_a( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_cc_a( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_cc_a( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_cc_a( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_cc_a( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_cc_a( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Mnemonic condition code selector ( eg. `JP` / `JPE` )
//
// Default: `JP`, `CMOVP`, `SETP`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetCC_p( Formatter: *mut MasmFormatter, FormatterType : u8 ) -> u32 { // FFI-Unsafe: CC_p
    if Formatter.is_null() {
        return 0;// CC_p::p;
    }

    let value: u32;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = transmute( obj.options_mut().cc_p() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = transmute( obj.options_mut().cc_p() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = transmute( obj.options_mut().cc_p() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = transmute( obj.options_mut().cc_p() as u32 );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = transmute( obj.options_mut().cc_p() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = transmute( obj.options_mut().cc_p() as u32 );
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }
   
    return value;
}

// Mnemonic condition code selector ( eg. `JP` / `JPE` )
//
// Default: `JP`, `CMOVP`, `SETP`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetCC_p( Formatter: *mut MasmFormatter, FormatterType : u8, Value : u32 ) -> bool { // FFI-Unsafe: CC_p
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_cc_p( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_cc_p( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_cc_p( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_cc_p( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_cc_p( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_cc_p( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Mnemonic condition code selector ( eg. `JNP` / `JPO` )
//
// Default: `JNP`, `CMOVNP`, `SETNP`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetCC_np( Formatter: *mut MasmFormatter, FormatterType : u8 ) -> u32 { // FFI-Unsafe: CC_np
    if Formatter.is_null() {
        return 0;// CC_np::np;
    }

    let value: u32;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = transmute( obj.options_mut().cc_np() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = transmute( obj.options_mut().cc_np() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = transmute( obj.options_mut().cc_np() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = transmute( obj.options_mut().cc_np() as u32 );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = transmute( obj.options_mut().cc_np() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = transmute( obj.options_mut().cc_np() as u32 );
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }
   
    return value;
}

// Mnemonic condition code selector ( eg. `JNP` / `JPO` )
//
// Default: `JNP`, `CMOVNP`, `SETNP`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetCC_np( Formatter: *mut MasmFormatter, FormatterType : u8, Value : u32 ) -> bool { // FFI-Unsafe: CC_np
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_cc_np( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_cc_np( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_cc_np( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_cc_np( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_cc_np( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_cc_np( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Mnemonic condition code selector ( eg. `JL` / `JNGE` )
//
// Default: `JL`, `CMOVL`, `SETL`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetCC_l( Formatter: *mut MasmFormatter, FormatterType: u8 ) -> u32 { // FFI-Unsafe: CC_l
    if Formatter.is_null() {
        return 0;// CC_l::l;
    }

    let value: u32;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = transmute( obj.options_mut().cc_l() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = transmute( obj.options_mut().cc_l() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = transmute( obj.options_mut().cc_l() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = transmute( obj.options_mut().cc_l() as u32 );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = transmute( obj.options_mut().cc_l() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = transmute( obj.options_mut().cc_l() as u32 );
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }
   
    return value;
}

// Mnemonic condition code selector ( eg. `JL` / `JNGE` )
//
// Default: `JL`, `CMOVL`, `SETL`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetCC_l( Formatter: *mut MasmFormatter, FormatterType : u8, Value : u32 ) -> bool { // FFI-Unsafe: CC_l
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_cc_l( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_cc_l( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_cc_l( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_cc_l( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_cc_l( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_cc_l( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Mnemonic condition code selector ( eg. `JGE` / `JNL` )
//
// Default: `JGE`, `CMOVGE`, `SETGE`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetCC_ge( Formatter: *mut MasmFormatter, FormatterType : u8 ) -> u32 { // FFI-Unsafe: CC_ge
    if Formatter.is_null() {
        return 0;// CC_ge::ge;
    }

    let value: u32;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = transmute( obj.options_mut().cc_ge() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = transmute( obj.options_mut().cc_ge() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = transmute( obj.options_mut().cc_ge() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = transmute( obj.options_mut().cc_ge() as u32 );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = transmute( obj.options_mut().cc_ge() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = transmute( obj.options_mut().cc_ge() as u32 );
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }
   
    return value;
}

// Mnemonic condition code selector ( eg. `JGE` / `JNL` )
//
// Default: `JGE`, `CMOVGE`, `SETGE`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetCC_ge( Formatter: *mut MasmFormatter, FormatterType : u8, Value : u32 ) -> bool { // FFI-Unsafe: CC_ge
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_cc_ge( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_cc_ge( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_cc_ge( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_cc_ge( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_cc_ge( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_cc_ge( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Mnemonic condition code selector ( eg. `JLE` / `JNG` )
//
// Default: `JLE`, `CMOVLE`, `SETLE`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetCC_le( Formatter: *mut MasmFormatter, FormatterType : u8 ) -> u32 { // FFI-Unsafe: CC_le
    if Formatter.is_null() {
        return 0;// CC_le::le;
    }

    let value: u32;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = transmute( obj.options_mut().cc_le() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = transmute( obj.options_mut().cc_le() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = transmute( obj.options_mut().cc_le() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = transmute( obj.options_mut().cc_le() as u32 );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = transmute( obj.options_mut().cc_le() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = transmute( obj.options_mut().cc_le() as u32 );
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }
   
    return value;
}

// Mnemonic condition code selector ( eg. `JLE` / `JNG` )
//
// Default: `JLE`, `CMOVLE`, `SETLE`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetCC_le( Formatter: *mut MasmFormatter, FormatterType : u8, Value : u32 ) -> bool { // FFI-Unsafe: CC_le
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_cc_le( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_cc_le( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_cc_le( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_cc_le( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_cc_le( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_cc_le( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}

// Mnemonic condition code selector ( eg. `JG` / `JNLE` )
//
// Default: `JG`, `CMOVG`, `SETG`
#[no_mangle]
pub unsafe extern "C" fn Formatter_GetCC_g( Formatter: *mut MasmFormatter, FormatterType : u8 ) -> u32 { // FFI-Unsafe: CC_g
    if Formatter.is_null() {
        return 0;// CC_g::g;
    }

    let value: u32;
    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            value = transmute( obj.options_mut().cc_g() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            value = transmute( obj.options_mut().cc_g() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            value = transmute( obj.options_mut().cc_g() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            value = transmute( obj.options_mut().cc_g() as u32 );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            value = transmute( obj.options_mut().cc_g() as u32 );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            value = transmute( obj.options_mut().cc_g() as u32 );
            Box::into_raw( obj );
        }
 */
        _ => { return 0; }
    }
   
    return value;
}

// Mnemonic condition code selector ( eg. `JG` / `JNLE` )
//
// Default: `JG`, `CMOVG`, `SETG`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn Formatter_SetCC_g( Formatter: *mut MasmFormatter, FormatterType : u8, Value : u32 ) -> bool { // FFI-Unsafe: CC_h
    if Formatter.is_null() {
        return false;
    }

    let formatterType : TFormatterType = transmute( FormatterType as u8 );
    match formatterType {
        TFormatterType::Masm |
        TFormatterType::Capstone => {
            let mut obj = Box::from_raw( Formatter );
            obj.options_mut().set_cc_g( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Nasm => { 
            let mut obj = Box::from_raw( Formatter as *mut NasmFormatter );
            obj.options_mut().set_cc_g( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Gas => { 
            let mut obj = Box::from_raw( Formatter as *mut GasFormatter );
            obj.options_mut().set_cc_g( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Intel => { 
            let mut obj = Box::from_raw( Formatter as *mut IntelFormatter );
            obj.options_mut().set_cc_g( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
/*
        TFormatterType::Fast => { 
            let mut obj = Box::from_raw( Formatter as *mut TFastFormatter );
            obj.options_mut().set_cc_g( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
        TFormatterType::Specialized => { 
            let mut obj = Box::from_raw( Formatter as *mut TSpecializedFormatter );
            obj.options_mut().set_cc_g( transmute( Value as u8 ) );
            Box::into_raw( obj );
        }
 */
        _ => { return false; }
    }

    return true;
}