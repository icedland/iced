iced-x86 C-Compatible Exports
[![Latest version](https://img.shields.io/crates/v/iced-x86.svg)](https://crates.io/crates/iced-x86)
[![Documentation](https://docs.rs/iced-x86/badge.svg)](https://docs.rs/iced-x86)
[![Minimum rustc version](https://img.shields.io/badge/rustc-1.60.0+-blue.svg)](#minimum-supported-rustc-version)
![License](https://img.shields.io/crates/l/iced-x86.svg)

iced-x86 is a blazing fast and correct x86 (16/32/64-bit) instruction decoder, disassembler and assembler written in Rust.

- 👍 Supports all Intel and AMD instructions
- 👍 Correct: All instructions are tested and iced has been tested against other disassemblers/assemblers (xed, gas, objdump, masm, dumpbin, nasm, ndisasm) and fuzzed
- 👍 100% Rust code with C-Compatible Exports
- 👍 The formatter supports masm, nasm, gas (AT&T), Intel (XED) and there are many options to customize the output
- 👍 Blazing fast: Decodes >200 MB/s, 93MB/s with Formatting
- 👍 Small decoded instructions, only 40 bytes and the decoder doesn't allocate any memory
- 👍 The encoder can be used to re-encode decoded instructions at any address
- 👍 API to get instruction info, eg. read/written registers, memory and rflags bits; CPUID feature flag, control flow info, etc
- 👍 Supports `#![no_std]`
- 👍 Supports `rustc` `1.60.0` or later
- 👍 License: MIT

## Usage
Build using cargo or run _Build.bat  

## Crate feature flags

- `decoder`: (👍 Enabled by default) Enables the decoder
- `encoder`: (👍 Enabled by default) Enables the encoder
- `block_encoder`: (👍 Enabled by default) Enables the [`BlockEncoder`]. This feature enables `encoder`
- `op_code_info`: (👍 Enabled by default) Enables getting instruction metadata ([`OpCodeInfo`]). This feature enables `encoder`
- `instr_info`: (👍 Enabled by default) Enables the instruction info code
- `gas`: (👍 Enabled by default) Enables the GNU Assembler (AT&T) formatter
- `intel`: (👍 Enabled by default) Enables the Intel (XED) formatter
- `masm`: (👍 Enabled by default) Enables the masm formatter
- `nasm`: (👍 Enabled by default) Enables the nasm formatter
- `fast_fmt`: (👍 Enabled by default) Enables [`SpecializedFormatter<TraitOptions>`] (and [`FastFormatter`]) (masm syntax) which is ~3.3x faster than the other formatters (the time includes decoding + formatting). Use it if formatting speed is more important than being able to re-assemble formatted instructions or if targeting wasm (this formatter uses less code).
- `std`: (👍 Enabled by default) Enables the `std` crate. `std` or `no_std` must be defined, but not both.
- `mvex`: Enables `MVEX` instructions (Knights Corner). You must also pass in `DecoderOptions::KNC` to the [`Decoder`] constructor.

[`BlockEncoder`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.BlockEncoder.html
[`CodeAssembler`]: https://docs.rs/iced-x86/1.21.0/iced_x86/code_asm/struct.CodeAssembler.html
[`Instruction`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.Instruction.html
[`OpCodeInfo`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.OpCodeInfo.html

## Minimum supported `rustc` version

iced-x86 supports `rustc` `1.60.0` or later.
This is checked in CI builds where the minimum supported version and the latest stable version are used to build the source code and run tests.

Bumping the minimum supported version of `rustc` is considered a minor breaking change. The minor version of iced-x86 will be incremented.
