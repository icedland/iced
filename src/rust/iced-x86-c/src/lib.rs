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
mod Code;
mod Register;
mod OpKind;
mod RoundingControl;
mod MemorySize;
mod CodeSize;
mod Instruction;

#[cfg(feature = "op_code_info")]
mod OpCodeInfo;
#[cfg(feature = "op_code_info")]
mod MandatoryPrefix;
#[cfg(feature = "op_code_info")]
mod OpCodeOperandKind;
#[cfg(feature = "op_code_info")]
mod OpCodeTableKind;

#[cfg(feature = "instr_info")]
mod CPUIdFeature;
#[cfg(feature = "instr_info")]
mod ConditionCode;
#[cfg(feature = "instr_info")]
mod FlowControl;
#[cfg(feature = "instr_info")]
mod OpAccess;
#[cfg(feature = "instr_info")]
mod InstructionInfoFactory;

#[cfg(feature = "mvex")]
mod MvexEHBit;
#[cfg(feature = "mvex")]
mod MvexTupleTypeLutKind;
#[cfg(feature = "mvex")]
mod MvexConvFn;
#[cfg(feature = "mvex")]
mod MvexRegMemConv;

#[cfg(feature = "instr_create")]
mod RepPrefixKind;
#[cfg(feature = "instr_create")]
mod InstructionWith;

#[cfg(feature = "decoder")]
mod EncodingKind;
#[cfg(feature = "decoder")]
mod TupleType;
#[cfg(feature = "decoder")]
mod DecoderError;
#[cfg(feature = "decoder")]
mod Decoder;

#[cfg(feature = "encoder")]
mod Encoder;

#[cfg(feature = "block_encoder")]
mod BlockEncoder;

#[cfg(feature = "formatter")]
mod FormatterTextKind;
#[cfg(feature = "formatter")]
mod NumberBase;
#[cfg(feature = "formatter")]
mod MemorySizeOptions;

#[cfg(feature = "formatter")]
mod SymbolResolver;
#[cfg(feature = "formatter")]
mod OptionsProvider;
#[cfg(feature = "formatter")]
mod OutputCallback;
mod VirtualAddressResolver;

#[cfg(feature = "formatter")]
mod Formatter;
#[cfg(feature = "masm")]
mod MasmFormatter;
#[cfg(feature = "nasm")]
mod NasmFormatter;
#[cfg(feature = "gas")]
mod GasFormatter;
#[cfg(feature = "intel")]
mod IntelFormatter;
#[cfg(feature = "fast_fmt")]
mod FastFormatter;
// ~100KB more
#[cfg(feature = "fast_fmt")]
mod SpecializedFormatterTraitOptions;
#[cfg(feature = "fast_fmt")]
mod SpecializedFormatter;