/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86::{Instruction, Formatter, IntelFormatter};
use crate::SymbolResolver::{TSymbolResolver, TSymbolResolverCallback};
use crate::OptionsProvider::{TFormatterOptionsProvider, TFormatterOptionsProviderCallback};
use crate::OutputCallback::TFormatterOutput;

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Intel-Formatter

// Creates a Intel formatter
//
// # Arguments
// - `symbol_resolver`: Symbol resolver or `None`
// - `options_provider`: Operand options provider or `None`
#[no_mangle]
pub extern "C" fn IntelFormatter_Create( SymbolResolver : Option<TSymbolResolverCallback>, OptionsProvider : Option<TFormatterOptionsProviderCallback>, UserData : *const usize ) -> *mut IntelFormatter {   
    if !SymbolResolver.is_none() && !OptionsProvider.is_none() {
        let symbols = Box::new( TSymbolResolver { callback:SymbolResolver, userData:UserData });
        let options = Box::new( TFormatterOptionsProvider { callback:OptionsProvider, userData:UserData });
        Box::into_raw( Box::new( IntelFormatter::with_options( Some( symbols ), Some( options ) ) ) )        
    }else if !SymbolResolver.is_none() {
        let symbols = Box::new( TSymbolResolver { callback:SymbolResolver, userData:UserData });
        Box::into_raw( Box::new( IntelFormatter::with_options( Some( symbols ), None ) ) )                
    }else if !OptionsProvider.is_none() {
        let options = Box::new( TFormatterOptionsProvider { callback:OptionsProvider, userData:UserData });
        Box::into_raw( Box::new( IntelFormatter::with_options( None, Some( options ) ) ) )
    }else {
        Box::into_raw( Box::new( IntelFormatter::with_options( None, None ) ) )
    }
}

// Format Instruction
#[no_mangle]
pub unsafe extern "C" fn IntelFormatter_Format( IntelFormatter: *mut IntelFormatter, Instruction: *mut Instruction, Output : *mut u8, Size : usize ) {     
    if IntelFormatter.is_null() {
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

    let mut obj = Box::from_raw( IntelFormatter );
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
pub unsafe extern "C" fn IntelFormatter_FormatCallback( IntelFormatter: *mut IntelFormatter, Instruction: *mut Instruction, FormatterOutput : *mut TFormatterOutput ) {     
    if IntelFormatter.is_null() {
        return;
    }
    if Instruction.is_null() {
        return;
    }
    if FormatterOutput.is_null() {
        return;
    }

    let mut obj = Box::from_raw( IntelFormatter );
    let mut output = Box::from_raw( FormatterOutput );
    obj.format( Instruction.as_mut().unwrap(), output.as_mut() );
    Box::into_raw( output );
    Box::into_raw( obj );
}
