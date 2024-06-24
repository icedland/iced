/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

#![allow( non_snake_case )]
#![allow( dead_code )] // TFormatterType
extern crate libc;

mod FreeMemory;
mod Instruction;
mod InstructionInfoFactory;
mod Decoder;
mod Encoder;
mod BlockEncoder;

mod SymbolResolver;
mod OptionsProvider;
mod OutputCallback;
mod VirtualAddressResolver;

mod Formatter;
mod MasmFormatter;
mod NasmFormatter;
mod GasFormatter;
mod IntelFormatter;
mod FastFormatter;
mod SpecializedFormatter;

mod InstructionWith;