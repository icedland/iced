/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86_rust::{Instruction, Formatter, GasFormatter};
#[cfg(feature = "decoder")]
use iced_x86_rust::Decoder;
use crate::SymbolResolver::{TSymbolResolver, TSymbolResolverCallback};
use crate::OptionsProvider::{TFormatterOptionsProvider, TFormatterOptionsProviderCallback};
use crate::OutputCallback::TFormatterOutput;
#[cfg(feature = "decoder")]
use crate::Decoder::TDecoderFormatCallback;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Gas-Formatter
pub(crate) struct TGasFormatter {
    pub Formatter : GasFormatter,
    pub Output : String,
  }

// Creates a Gas formatter
//
// # Arguments
// - `symbol_resolver`: Symbol resolver or `None`
// - `options_provider`: Operand options provider or `None`
#[no_mangle]
pub extern "C" fn GasFormatter_Create( SymbolResolver : Option<TSymbolResolverCallback>, OptionsProvider : Option<TFormatterOptionsProviderCallback>, UserData : *const usize ) -> *mut TGasFormatter {   
    if !SymbolResolver.is_none() && !OptionsProvider.is_none() {
        let symbols = Box::new( TSymbolResolver { callback:SymbolResolver, userData:UserData });
        let options = Box::new( TFormatterOptionsProvider { callback:OptionsProvider, userData:UserData });
        Box::into_raw( Box::new( TGasFormatter { Formatter: GasFormatter::with_options( Some( symbols ), Some( options ) ), Output: String::new() } ) )
    } else if !SymbolResolver.is_none() {
        let symbols = Box::new( TSymbolResolver { callback:SymbolResolver, userData:UserData });
        Box::into_raw( Box::new( TGasFormatter { Formatter: GasFormatter::with_options( Some( symbols ), None ), Output: String::new() } ) )
    } else if !OptionsProvider.is_none() {
        let options = Box::new( TFormatterOptionsProvider { callback:OptionsProvider, userData:UserData });
        Box::into_raw( Box::new( TGasFormatter { Formatter: GasFormatter::with_options( None, Some( options ) ), Output: String::new() } ) )
    } else {
        Box::into_raw( Box::new( TGasFormatter { Formatter: GasFormatter::with_options( None, None ), Output: String::new() } ) )
    }
}

// Format Instruction
#[no_mangle]
pub unsafe extern "C" fn GasFormatter_Format( Formatter: *mut TGasFormatter, Instruction: *mut Instruction, Output : *mut *const u8, Size : *mut usize ) {     
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
pub unsafe extern "C" fn GasFormatter_FormatCallback( Formatter: *mut TGasFormatter, Instruction: *mut Instruction, FormatterOutput : *mut TFormatterOutput ) {     
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
pub unsafe extern "C" fn GasFormatter_DecodeFormat( Decoder: *mut Decoder, Formatter: *mut TGasFormatter, Instruction: *mut Instruction, Output : *mut *const u8, Size : *mut usize ) {
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
pub unsafe extern "C" fn GasFormatter_DecodeFormatCallback( Decoder: *mut Decoder, Formatter: *mut TGasFormatter, Instruction: *mut Instruction, FormatterOutput : *mut TFormatterOutput ) {     
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
pub unsafe extern "C" fn GasFormatter_DecodeFormatToEnd( Decoder: *mut Decoder, Formatter: *mut TGasFormatter, Callback: Option<TDecoderFormatCallback>, UserData : *const usize ) {
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
// ( gas only ): If `true`, the formatter doesn't add `%` to registers
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,ecx`
// 👍 | `false` | `mov %eax,%ecx`
#[no_mangle]
pub unsafe extern "C" fn GasFormatter_GetNakedRegisters( Formatter: *mut TGasFormatter ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    let value = obj.Formatter.options_mut().gas_naked_registers();

    Box::into_raw( obj );
 
    return value;
}

// ( gas only ): If `true`, the formatter doesn't add `%` to registers
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `mov eax,ecx`
// 👍 | `false` | `mov %eax,%ecx`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn GasFormatter_SetNakedRegisters( Formatter: *mut TGasFormatter, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    obj.Formatter.options_mut().set_gas_naked_registers( Value );

    Box::into_raw( obj );

    return true;
}

// ( gas only ): Shows the mnemonic size suffix even when not needed
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `movl %eax,%ecx`
// 👍 | `false` | `mov %eax,%ecx`
#[no_mangle]
pub unsafe extern "C" fn GasFormatter_GetShowMnemonicSizeSuffix( Formatter: *mut TGasFormatter ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    let value = obj.Formatter.options_mut().gas_show_mnemonic_size_suffix();

    Box::into_raw( obj );
 
    return value;
}

// ( gas only ): Shows the mnemonic size suffix even when not needed
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `movl %eax,%ecx`
// 👍 | `false` | `mov %eax,%ecx`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn GasFormatter_SetShowMnemonicSizeSuffix( Formatter: *mut TGasFormatter, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    obj.Formatter.options_mut().set_gas_show_mnemonic_size_suffix( Value );

    Box::into_raw( obj );

    return true;
}

// ( gas only ): Add a space after the comma if it's a memory operand
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `( %eax, %ecx, 2 )`
// 👍 | `false` | `( %eax,%ecx,2 )`
#[no_mangle]
pub unsafe extern "C" fn GasFormatter_GetSpaceAfterMemoryOperandComma( Formatter: *mut TGasFormatter ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    let value = obj.Formatter.options_mut().gas_space_after_memory_operand_comma();

    Box::into_raw( obj );
 
    return value;
}

// ( gas only ): Add a space after the comma if it's a memory operand
//
// Default | Value | Example
// --------|-------|--------
// _ | `true` | `( %eax, %ecx, 2 )`
// 👍 | `false` | `( %eax,%ecx,2 )`
//
// # Arguments
// * `value`: New value
#[no_mangle]
pub unsafe extern "C" fn GasFormatter_SetSpaceAfterMemoryOperandComma( Formatter: *mut TGasFormatter, Value : bool ) -> bool {
    if Formatter.is_null() {
        return false;
    }
    let mut obj = Box::from_raw( Formatter );

    obj.Formatter.options_mut().set_gas_space_after_memory_operand_comma( Value );

    Box::into_raw( obj );

    return true;
}