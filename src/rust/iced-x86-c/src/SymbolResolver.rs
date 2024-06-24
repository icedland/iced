/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

use iced_x86::{Instruction, SymbolResolver, SymbolResult};
use std::{slice, str};
use libc::{c_char, strlen};

// ~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
// Callbacks

// Tries to resolve a symbol
//
// # Arguments
// - `instruction`: Instruction
// - `operand`: Operand number, 0-based. This is a formatter operand and isn't necessarily the same as an instruction operand.
// - `instruction_operand`: Instruction operand number, 0-based, or `None` if it's an operand created by the formatter.
// - `address`: Address
// - `address_size`: Size of `address` in bytes ( eg. 1, 2, 4 or 8 )
pub(crate) type
  TSymbolResolverCallback = unsafe extern "C" fn( Instruction: *const Instruction, Operand: u32, InstructionOperand: u32, Address: u64, Size: u32, UserData : *const usize ) -> *const c_char;

  pub(crate) struct TSymbolResolver {
    //map: HashMap<u64, String>,
    pub(crate) userData: *const usize,
    pub(crate) callback: Option<TSymbolResolverCallback>
}

impl SymbolResolver for TSymbolResolver {
    fn symbol( &mut self, Instruction: &Instruction, Operand: u32, InstructionOperand: Option<u32>, Address: u64, Size: u32 ) -> Option<SymbolResult> {
        unsafe {
            if !self.callback.is_none() {   
                //let value = self.callback.unwrap()( &mut u as *mut u32, &mut i as *mut i32 );// Var-Parameter-Sample

                let instructionoperand: u32;
                match InstructionOperand {
                    None => instructionoperand = 0xFFFF_FFFF,
                    _Some => instructionoperand = InstructionOperand.unwrap()
                }

                let value = self.callback.unwrap()( Instruction, Operand, instructionoperand, Address, Size, self.userData );
                let symbol_string = str::from_utf8_unchecked( slice::from_raw_parts( value as *const u8, strlen( value ) ) );
                if !symbol_string.is_empty() {
                    // The 'Address' arg is the address of the symbol and doesn't have to be identical
                    // to the 'address' arg passed to symbol(). If it's different from the input
                    // address, the formatter will add +N or -N, eg. '[ rax+symbol+123 ]'
                    return Some( SymbolResult::with_str( Address, symbol_string ) )
                }else {
                    return None
                    /*
                    if let Some( symbol_string ) = self.map.get( &addr ) {
                        // The 'address' arg is the address of the symbol and doesn't have to be identical
                        // to the 'address' arg passed to symbol(). If it's different from the input
                        // address, the formatter will add +N or -N, eg. '[ rax+symbol+123 ]'
                        return Some( SymbolResult::with_str( addr, symbol_string.as_str() ) )
                        //Some( SymbolResult::with_str( Address, symbol_string.as_str() ) )
                    }else {
                        return None
                    }
                    */                    
              }
            }else {
                return None
                /*
                if let Some( symbol_string ) = self.map.get( &address ) {
                    // The 'address' arg is the address of the symbol and doesn't have to be identical
                    // to the 'address' arg passed to symbol(). If it's different from the input
                    // address, the formatter will add +N or -N, eg. '[ rax+symbol+123 ]'
                    return Some( SymbolResult::with_str( addr, symbol_string.as_str() ) )
                    //return Some( SymbolResult::with_str( Address, symbol_string.as_str() ) )
                }else {
                    return None
                }
                */
            }
        };
    }
}
