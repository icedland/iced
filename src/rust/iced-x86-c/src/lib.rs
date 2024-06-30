/*
    Iced (Dis)Assembler
    C-Compatible Exports
  
    TetzkatLipHoka 2022-2024
*/

#![allow( non_snake_case )]
#![allow( dead_code )] // TFormatterType
extern crate libc;

mod FreeMemory;
mod Mnemonic;
mod OpCodeInfo;
mod Code;
mod Register;
mod OpKind;
mod EncodingKind;
mod CPUIdFeature;
mod ConditionCode;
mod FlowControl;
mod TupleType;
mod MvexEHBit;
mod MvexTupleTypeLutKind;
mod MvexConvFn;
mod MvexRegMemConv;
mod RoundingControl;
mod MemorySize;
mod MandatoryPrefix;
mod OpCodeOperandKind;
mod OpAccess;
mod OpCodeTableKind;
mod CodeSize;
mod FormatterTextKind;
mod NumberBase;
mod RepPrefixKind;
mod MemorySizeOptions;
mod Instruction;
mod InstructionWith;
mod InstructionInfoFactory;
mod DecoderError;
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
// ~100KB more
mod SpecializedFormatterTraitOptions;
mod SpecializedFormatter;