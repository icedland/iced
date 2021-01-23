/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

//! iced-x86
//! [![Latest version](https://img.shields.io/crates/v/iced-x86.svg)](https://crates.io/crates/iced-x86)
//! [![Documentation](https://docs.rs/iced-x86/badge.svg)](https://docs.rs/iced-x86)
//! [![Minimum rustc version](https://img.shields.io/badge/rustc-1.41.0+-yellow.svg)](#minimum-supported-rustc-version)
//! ![License](https://img.shields.io/crates/l/iced-x86.svg)
//!
//! iced-x86 is a high performance and correct x86 (16/32/64-bit) instruction decoder, disassembler and assembler written in Rust.
//!
//! It can be used for static analysis of x86/x64 binaries, to rewrite code (eg. remove garbage instructions), to relocate code or as a disassembler.
//!
//! - ✔️Supports all Intel and AMD instructions
//! - ✔️Correct: All instructions are tested and iced has been tested against other disassemblers/assemblers (xed, gas, objdump, masm, dumpbin, nasm, ndisasm) and fuzzed
//! - ✔️100% Rust code
//! - ✔️The formatter supports masm, nasm, gas (AT&T), Intel (XED) and there are many options to customize the output
//! - ✔️The decoder is 4x+ faster than other similar libraries and doesn't allocate any memory
//! - ✔️Small decoded instructions, only 40 bytes
//! - ✔️The encoder can be used to re-encode decoded instructions at any address
//! - ✔️API to get instruction info, eg. read/written registers, memory and rflags bits; CPUID feature flag, control flow info, etc
//! - ✔️Supports `#![no_std]` and `WebAssembly`
//! - ✔️Supports `rustc` `1.41.0` or later
//! - ✔️Few dependencies (`static_assertions` and `lazy_static`)
//! - ✔️License: MIT
//!
//! ## Usage
//!
//! Add this to your `Cargo.toml`:
//!
//! ```toml
//! [dependencies]
//! iced-x86 = "1.10.2"
//! ```
//!
//! Or to customize which features to use:
//!
//! ```toml
//! [dependencies.iced-x86]
//! version = "1.10.2"
//! default-features = false
//! # See below for all features
//! features = ["std", "decoder", "masm"]
//! ```
//!
//! If you're using Rust 2015 edition you must also add this to your `lib.rs` or `main.rs`:
//!
//! ```rust
//! extern crate iced_x86;
//! ```
//!
//! ## Crate feature flags
//!
//! You can enable/disable these in your `Cargo.toml` file.
//!
//! - `decoder`: (✔️Enabled by default) Enables the decoder
//! - `encoder`: (✔️Enabled by default) Enables the encoder
//! - `block_encoder`: (✔️Enabled by default) Enables the [`BlockEncoder`]. This feature enables `encoder`
//! - `op_code_info`: (✔️Enabled by default) Enables getting instruction metadata ([`OpCodeInfo`]). This feature enables `encoder`
//! - `instr_info`: (✔️Enabled by default) Enables the instruction info code
//! - `gas`: (✔️Enabled by default) Enables the GNU Assembler (AT&T) formatter
//! - `intel`: (✔️Enabled by default) Enables the Intel (XED) formatter
//! - `masm`: (✔️Enabled by default) Enables the masm formatter
//! - `nasm`: (✔️Enabled by default) Enables the nasm formatter
//! - `fast_fmt`: (✔️Enabled by default) Enables `FastFormatter` (masm syntax) which is ~1.9x faster than the other formatters (the time includes decoding + formatting). Use it if formatting speed is more important than being able to re-assemble formatted instructions or if targeting wasm (this formatter uses less code).
//! - `db`: Enables creating `db`, `dw`, `dd`, `dq` instructions. It's not enabled by default because it's possible to store up to 16 bytes in the instruction and then use another method to read an enum value.
//! - `std`: (✔️Enabled by default) Enables the `std` crate. `std` or `no_std` must be defined, but not both.
//! - `no_std`: Enables `#![no_std]`. `std` or `no_std` must be defined, but not both. This feature uses the `alloc` crate and the `hashbrown` crate.
//! - `exhaustive_enums`: Enables exhaustive enums, i.e., no enum has the `#[non_exhaustive]` attribute
//! - `no_vex`: Disables all `VEX` instructions. See below for more info.
//! - `no_evex`: Disables all `EVEX` instructions. See below for more info.
//! - `no_xop`: Disables all `XOP` instructions. See below for more info.
//! - `no_d3now`: Disables all `3DNow!` instructions. See below for more info.
//!
//! If you use `no_vex`, `no_evex`, `no_xop` or `no_d3now`, you should run the generator again (before building iced) to generate even smaller output.
//!
//! [`BlockEncoder`]: struct.BlockEncoder.html
//! [`OpCodeInfo`]: struct.OpCodeInfo.html
//!
//! ## How-tos
//!
//! - [Disassemble (decode and format instructions)](#disassemble-decode-and-format-instructions)
//! - [Create and encode instructions](#create-and-encode-instructions)
//! - [Disassemble with a symbol resolver](#disassemble-with-a-symbol-resolver)
//! - [Disassemble with colorized text](#disassemble-with-colorized-text)
//! - [Move code in memory (eg. hook a function)](#move-code-in-memory-eg-hook-a-function)
//! - [Get instruction info, eg. read/written regs/mem, control flow info, etc](#get-instruction-info-eg-readwritten-regsmem-control-flow-info-etc)
//! - [Get the virtual address of a memory operand](#get-the-virtual-address-of-a-memory-operand)
//! - [Disassemble old/deprecated CPU instructions](#disassemble-olddeprecated-cpu-instructions)
//!
//! ## Disassemble (decode and format instructions)
//!
//! This example uses a [`Decoder`] and one of the [`Formatter`]s to decode and format the code,
//! eg. [`GasFormatter`], [`IntelFormatter`], [`MasmFormatter`], [`NasmFormatter`], [`FastFormatter`].
//!
//! [`Decoder`]: struct.Decoder.html
//! [`Formatter`]: trait.Formatter.html
//! [`GasFormatter`]: struct.GasFormatter.html
//! [`IntelFormatter`]: struct.IntelFormatter.html
//! [`MasmFormatter`]: struct.MasmFormatter.html
//! [`NasmFormatter`]: struct.NasmFormatter.html
//! [`FastFormatter`]: struct.FastFormatter.html
//!
//! ```rust
//! use iced_x86::{Decoder, DecoderOptions, Formatter, Instruction, NasmFormatter};
//!
//! /*
//! This method produces the following output:
//! 00007FFAC46ACDA4 48895C2410           mov       [rsp+10h],rbx
//! 00007FFAC46ACDA9 4889742418           mov       [rsp+18h],rsi
//! 00007FFAC46ACDAE 55                   push      rbp
//! 00007FFAC46ACDAF 57                   push      rdi
//! 00007FFAC46ACDB0 4156                 push      r14
//! 00007FFAC46ACDB2 488DAC2400FFFFFF     lea       rbp,[rsp-100h]
//! 00007FFAC46ACDBA 4881EC00020000       sub       rsp,200h
//! 00007FFAC46ACDC1 488B0518570A00       mov       rax,[rel 7FFA`C475`24E0h]
//! 00007FFAC46ACDC8 4833C4               xor       rax,rsp
//! 00007FFAC46ACDCB 488985F0000000       mov       [rbp+0F0h],rax
//! 00007FFAC46ACDD2 4C8B052F240A00       mov       r8,[rel 7FFA`C474`F208h]
//! 00007FFAC46ACDD9 488D05787C0400       lea       rax,[rel 7FFA`C46F`4A58h]
//! 00007FFAC46ACDE0 33FF                 xor       edi,edi
//! */
//! pub(crate) fn how_to_disassemble() {
//!     let bytes = EXAMPLE_CODE;
//!     let mut decoder = Decoder::new(EXAMPLE_CODE_BITNESS, bytes, DecoderOptions::NONE);
//!     decoder.set_ip(EXAMPLE_CODE_RIP);
//!
//!     // Formatters: Masm*, Nasm*, Gas* (AT&T) and Intel* (XED).
//!     // There's also `FastFormatter` which is ~1.9x faster. Use it if formatting speed is more
//!     // important than being able to re-assemble formatted instructions.
//!     let mut formatter = NasmFormatter::new();
//!
//!     // Change some options, there are many more
//!     formatter.options_mut().set_digit_separator("`");
//!     formatter.options_mut().set_first_operand_char_index(10);
//!
//!     // String implements FormatterOutput
//!     let mut output = String::new();
//!
//!     // Initialize this outside the loop because decode_out() writes to every field
//!     let mut instruction = Instruction::default();
//!
//!     // The decoder also implements Iterator/IntoIterator so you could use a for loop:
//!     //      for instruction in &mut decoder { /* ... */ }
//!     // or collect():
//!     //      let instructions: Vec<_> = decoder.into_iter().collect();
//!     // but can_decode()/decode_out() is a little faster:
//!     while decoder.can_decode() {
//!         // There's also a decode() method that returns an instruction but that also
//!         // means it copies an instruction (40 bytes):
//!         //     instruction = decoder.decode();
//!         decoder.decode_out(&mut instruction);
//!
//!         // Format the instruction ("disassemble" it)
//!         output.clear();
//!         formatter.format(&instruction, &mut output);
//!
//!         // Eg. "00007FFAC46ACDB2 488DAC2400FFFFFF     lea       rbp,[rsp-100h]"
//!         print!("{:016X} ", instruction.ip());
//!         let start_index = (instruction.ip() - EXAMPLE_CODE_RIP) as usize;
//!         let instr_bytes = &bytes[start_index..start_index + instruction.len()];
//!         for b in instr_bytes.iter() {
//!             print!("{:02X}", b);
//!         }
//!         if instr_bytes.len() < HEXBYTES_COLUMN_BYTE_LENGTH {
//!             for _ in 0..HEXBYTES_COLUMN_BYTE_LENGTH - instr_bytes.len() {
//!                 print!("  ");
//!             }
//!         }
//!         println!(" {}", output);
//!     }
//! }
//!
//! const HEXBYTES_COLUMN_BYTE_LENGTH: usize = 10;
//! const EXAMPLE_CODE_BITNESS: u32 = 64;
//! const EXAMPLE_CODE_RIP: u64 = 0x0000_7FFA_C46A_CDA4;
//! static EXAMPLE_CODE: &[u8] = &[
//!     0x48, 0x89, 0x5C, 0x24, 0x10, 0x48, 0x89, 0x74, 0x24, 0x18, 0x55, 0x57, 0x41, 0x56, 0x48, 0x8D,
//!     0xAC, 0x24, 0x00, 0xFF, 0xFF, 0xFF, 0x48, 0x81, 0xEC, 0x00, 0x02, 0x00, 0x00, 0x48, 0x8B, 0x05,
//!     0x18, 0x57, 0x0A, 0x00, 0x48, 0x33, 0xC4, 0x48, 0x89, 0x85, 0xF0, 0x00, 0x00, 0x00, 0x4C, 0x8B,
//!     0x05, 0x2F, 0x24, 0x0A, 0x00, 0x48, 0x8D, 0x05, 0x78, 0x7C, 0x04, 0x00, 0x33, 0xFF,
//! ];
//! ```
//!
//! ## Create and encode instructions
//!
//! This example uses a [`BlockEncoder`] to encode created [`Instruction`]s. This example needs the `db` feature because it creates `db` "instructions".
//!
//! [`BlockEncoder`]: struct.BlockEncoder.html
//! [`Instruction`]: struct.Instruction.html
//!
//! ```rust
//! use iced_x86::{
//!     BlockEncoder, BlockEncoderOptions, Code, Decoder, DecoderOptions, Formatter, GasFormatter,
//!     Instruction, InstructionBlock, MemoryOperand, Register,
//! };
//!
//! pub(crate) fn how_to_encode_instructions() {
//!     let bitness = 64;
//!
//!     // All created instructions get an IP of 0. The label id is just an IP.
//!     // The branch instruction's *target* IP should be equal to the IP of the
//!     // target instruction.
//!     let mut label_id: u64 = 1;
//!     let mut create_label = || {
//!         let id = label_id;
//!         label_id += 1;
//!         id
//!     };
//!     fn add_label(id: u64, mut instruction: Instruction) -> Instruction {
//!         instruction.set_ip(id);
//!         instruction
//!     }
//!
//!     let label1 = create_label();
//!
//!     let mut instructions: Vec<Instruction> = Vec::new();
//!     instructions.push(Instruction::with_reg(Code::Push_r64, Register::RBP));
//!     instructions.push(Instruction::with_reg(Code::Push_r64, Register::RDI));
//!     instructions.push(Instruction::with_reg(Code::Push_r64, Register::RSI));
//!     instructions
//!         .push(Instruction::try_with_reg_u32(Code::Sub_rm64_imm32, Register::RSP, 0x50).unwrap());
//!     instructions.push(Instruction::with(Code::VEX_Vzeroupper));
//!     instructions.push(Instruction::with_reg_mem(
//!         Code::Lea_r64_m,
//!         Register::RBP,
//!         MemoryOperand::with_base_displ(Register::RSP, 0x60),
//!     ));
//!     instructions.push(Instruction::with_reg_reg(Code::Mov_r64_rm64, Register::RSI, Register::RCX));
//!     instructions.push(Instruction::with_reg_mem(
//!         Code::Lea_r64_m,
//!         Register::RDI,
//!         MemoryOperand::with_base_displ(Register::RBP, -0x38),
//!     ));
//!     instructions
//!         .push(Instruction::try_with_reg_i32(Code::Mov_r32_imm32, Register::ECX, 0x0A).unwrap());
//!     instructions.push(Instruction::with_reg_reg(Code::Xor_r32_rm32, Register::EAX, Register::EAX));
//!     instructions.push(Instruction::try_with_rep_stosd(bitness).unwrap());
//!     instructions.push(
//!         Instruction::try_with_reg_u64(Code::Cmp_rm64_imm32, Register::RSI, 0x1234_5678).unwrap(),
//!     );
//!     // Create a branch instruction that references label1
//!     instructions.push(Instruction::try_with_branch(Code::Jne_rel32_64, label1).unwrap());
//!     instructions.push(Instruction::with(Code::Nopd));
//!     // Add the instruction that is the target of the branch
//!     instructions.push(add_label(
//!         label1,
//!         Instruction::with_reg_reg(Code::Xor_r32_rm32, Register::R15D, Register::R15D),
//!     ));
//!
//!     // Create an instruction that accesses some data using an RIP relative memory operand
//!     let data1 = create_label();
//!     instructions.push(Instruction::with_reg_mem(
//!         Code::Lea_r64_m,
//!         Register::R14,
//!         MemoryOperand::with_base_displ(Register::RIP, data1 as i64),
//!     ));
//!     instructions.push(Instruction::with(Code::Nopd));
//!     let raw_data: &[u8] = &[0x12, 0x34, 0x56, 0x78];
//!     // Creating db/dw/dd/dq instructions requires the `db` feature or it will fail
//!     instructions.push(add_label(data1, Instruction::try_with_declare_byte(raw_data).unwrap()));
//!
//!     // Use BlockEncoder to encode a block of instructions. This block can contain any
//!     // number of branches and any number of instructions. It does support encoding more
//!     // than one block but it's rarely needed.
//!     // It uses Encoder to encode all instructions.
//!     // If the target of a branch is too far away, it can fix it to use a longer branch.
//!     // This can be disabled by enabling some BlockEncoderOptions flags.
//!     let target_rip = 0x0000_1248_FC84_0000;
//!     let block = InstructionBlock::new(&instructions, target_rip);
//!     let result = match BlockEncoder::encode(bitness, block, BlockEncoderOptions::NONE) {
//!         Err(error) => panic!("Failed to encode it: {}", error),
//!         Ok(result) => result,
//!     };
//!
//!     // Now disassemble the encoded instructions. Note that the 'jmp near'
//!     // instruction was turned into a 'jmp short' instruction because we
//!     // didn't disable branch optimizations.
//!     let bytes = result.code_buffer;
//!     let mut output = String::new();
//!     let bytes_code = &bytes[0..bytes.len() - raw_data.len()];
//!     let bytes_data = &bytes[bytes.len() - raw_data.len()..];
//!     let mut decoder = Decoder::new(bitness, bytes_code, DecoderOptions::NONE);
//!     decoder.set_ip(target_rip);
//!     let mut formatter = GasFormatter::new();
//!     formatter.options_mut().set_first_operand_char_index(8);
//!     for instruction in &mut decoder {
//!         output.clear();
//!         formatter.format(&instruction, &mut output);
//!         println!("{:016X} {}", instruction.ip(), output);
//!     }
//!     // Creating db/dw/dd/dq instructions requires the `db` feature or it will panic!()
//!     let db = Instruction::try_with_declare_byte(bytes_data).unwrap();
//!     output.clear();
//!     formatter.format(&db, &mut output);
//!     println!("{:016X} {}", decoder.ip(), output);
//! }
//! /*
//! Output:
//! 00001248FC840000 push    %rbp
//! 00001248FC840001 push    %rdi
//! 00001248FC840002 push    %rsi
//! 00001248FC840003 sub     $0x50,%rsp
//! 00001248FC84000A vzeroupper
//! 00001248FC84000D lea     0x60(%rsp),%rbp
//! 00001248FC840012 mov     %rcx,%rsi
//! 00001248FC840015 lea     -0x38(%rbp),%rdi
//! 00001248FC840019 mov     $0xA,%ecx
//! 00001248FC84001E xor     %eax,%eax
//! 00001248FC840020 rep stos %eax,(%rdi)
//! 00001248FC840022 cmp     $0x12345678,%rsi
//! 00001248FC840029 jne     0x00001248FC84002C
//! 00001248FC84002B nop
//! 00001248FC84002C xor     %r15d,%r15d
//! 00001248FC84002F lea     0x1248FC840037,%r14
//! 00001248FC840036 nop
//! 00001248FC840037 .byte   0x12,0x34,0x56,0x78
//! */
//! ```
//!
//! ## Disassemble with a symbol resolver
//!
//! Creates a custom [`SymbolResolver`] that is called by a [`Formatter`].
//!
//! [`SymbolResolver`]: trait.SymbolResolver.html
//! [`Formatter`]: trait.Formatter.html
//!
//! ```rust
//! use iced_x86::{
//!     Decoder, DecoderOptions, Formatter, Instruction, MasmFormatter, SymbolResolver, SymbolResult,
//! };
//! use std::collections::HashMap;
//!
//! struct MySymbolResolver {
//!     map: HashMap<u64, String>,
//! }
//!
//! impl SymbolResolver for MySymbolResolver {
//!     fn symbol(
//!         &mut self, _instruction: &Instruction, _operand: u32, _instruction_operand: Option<u32>,
//!         address: u64, _address_size: u32,
//!     ) -> Option<SymbolResult> {
//!         if let Some(symbol_string) = self.map.get(&address) {
//!             // The 'address' arg is the address of the symbol and doesn't have to be identical
//!             // to the 'address' arg passed to symbol(). If it's different from the input
//!             // address, the formatter will add +N or -N, eg. '[rax+symbol+123]'
//!             Some(SymbolResult::with_str(address, symbol_string.as_str()))
//!         } else {
//!             None
//!         }
//!     }
//! }
//!
//! pub(crate) fn how_to_resolve_symbols() {
//!     let bytes = b"\x48\x8B\x8A\xA5\x5A\xA5\x5A";
//!     let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
//!     let instr = decoder.decode();
//!
//!     let mut sym_map: HashMap<u64, String> = HashMap::new();
//!     sym_map.insert(0x5AA5_5AA5, String::from("my_data"));
//!
//!     let mut output = String::new();
//!     let resolver = Box::new(MySymbolResolver { map: sym_map });
//!     // Create a formatter that uses our symbol resolver
//!     let mut formatter = MasmFormatter::with_options(Some(resolver), None);
//!
//!     // This will call the symbol resolver for each immediate / displacement
//!     // it finds in the instruction.
//!     formatter.format(&instr, &mut output);
//!
//!     // Prints: mov rcx,[rdx+my_data]
//!     println!("{}", output);
//! }
//! ```
//!
//! ## Disassemble with colorized text
//!
//! Creates a custom [`FormatterOutput`] that is called by a [`Formatter`].
//!
//! This example will fail to compile unless you install the `colored` crate, see below.
//!
//! [`FormatterOutput`]: trait.FormatterOutput.html
//! [`Formatter`]: trait.Formatter.html
//!
//! ```rust compile_fail
//! // This example uses crate colored = "2.0.0"
//! use colored::{ColoredString, Colorize};
//! use iced_x86::{
//!     Decoder, DecoderOptions, Formatter, FormatterOutput, FormatterTextKind, IntelFormatter,
//! };
//!
//! // Custom formatter output that stores the output in a vector.
//! struct MyFormatterOutput {
//!     vec: Vec<(String, FormatterTextKind)>,
//! }
//!
//! impl MyFormatterOutput {
//!     pub fn new() -> Self {
//!         Self { vec: Vec::new() }
//!     }
//! }
//!
//! impl FormatterOutput for MyFormatterOutput {
//!     fn write(&mut self, text: &str, kind: FormatterTextKind) {
//!         // This allocates a string. If that's a problem, just call print!() here
//!         // instead of storing the result in a vector.
//!         self.vec.push((String::from(text), kind));
//!     }
//! }
//!
//! pub(crate) fn how_to_colorize_text() {
//!     let bytes = EXAMPLE_CODE;
//!     let mut decoder = Decoder::new(EXAMPLE_CODE_BITNESS, bytes, DecoderOptions::NONE);
//!     decoder.set_ip(EXAMPLE_CODE_RIP);
//!
//!     let mut formatter = IntelFormatter::new();
//!     formatter.options_mut().set_first_operand_char_index(8);
//!     let mut output = MyFormatterOutput::new();
//!     for instruction in &mut decoder {
//!         output.vec.clear();
//!         // The formatter calls output.write() which will update vec with text/colors
//!         formatter.format(&instruction, &mut output);
//!         for (text, kind) in output.vec.iter() {
//!             print!("{}", get_color(text.as_str(), *kind));
//!         }
//!         println!();
//!     }
//! }
//!
//! fn get_color(s: &str, kind: FormatterTextKind) -> ColoredString {
//!     match kind {
//!         FormatterTextKind::Directive | FormatterTextKind::Keyword => s.bright_yellow(),
//!         FormatterTextKind::Prefix | FormatterTextKind::Mnemonic => s.bright_red(),
//!         FormatterTextKind::Register => s.bright_blue(),
//!         FormatterTextKind::Number => s.bright_cyan(),
//!         _ => s.white(),
//!     }
//! }
//!
//! const EXAMPLE_CODE_BITNESS: u32 = 64;
//! const EXAMPLE_CODE_RIP: u64 = 0x0000_7FFA_C46A_CDA4;
//! static EXAMPLE_CODE: &[u8] = &[
//!     0x48, 0x89, 0x5C, 0x24, 0x10, 0x48, 0x89, 0x74, 0x24, 0x18, 0x55, 0x57, 0x41, 0x56, 0x48, 0x8D,
//!     0xAC, 0x24, 0x00, 0xFF, 0xFF, 0xFF, 0x48, 0x81, 0xEC, 0x00, 0x02, 0x00, 0x00, 0x48, 0x8B, 0x05,
//!     0x18, 0x57, 0x0A, 0x00, 0x48, 0x33, 0xC4, 0x48, 0x89, 0x85, 0xF0, 0x00, 0x00, 0x00, 0x4C, 0x8B,
//!     0x05, 0x2F, 0x24, 0x0A, 0x00, 0x48, 0x8D, 0x05, 0x78, 0x7C, 0x04, 0x00, 0x33, 0xFF,
//! ];
//! ```
//!
//! ## Move code in memory (eg. hook a function)
//!
//! Uses instruction info API and the encoder to patch a function to jump to the programmer's function.
//!
//! ```rust
//! use iced_x86::{
//!     BlockEncoder, BlockEncoderOptions, Code, Decoder, DecoderOptions, FlowControl, Formatter,
//!     Instruction, InstructionBlock, NasmFormatter, OpKind,
//! };
//!
//! // Decodes instructions from some address, then encodes them starting at some
//! // other address. This can be used to hook a function. You decode enough instructions
//! // until you have enough bytes to add a JMP instruction that jumps to your code.
//! // Your code will then conditionally jump to the original code that you re-encoded.
//! //
//! // This code uses the BlockEncoder which will help with some things, eg. converting
//! // short branches to longer branches if the target is too far away.
//! //
//! // 64-bit mode also supports RIP relative addressing, but the encoder can't rewrite
//! // those to use a longer displacement. If any of the moved instructions have RIP
//! // relative addressing and it tries to access data too far away, the encoder will fail.
//! // The easiest solution is to use OS alloc functions that allocate memory close to the
//! // original code (+/-2GB).
//!
//! /*
//! This method produces the following output:
//! Original code:
//! 00007FFAC46ACDA4 mov [rsp+10h],rbx
//! 00007FFAC46ACDA9 mov [rsp+18h],rsi
//! 00007FFAC46ACDAE push rbp
//! 00007FFAC46ACDAF push rdi
//! 00007FFAC46ACDB0 push r14
//! 00007FFAC46ACDB2 lea rbp,[rsp-100h]
//! 00007FFAC46ACDBA sub rsp,200h
//! 00007FFAC46ACDC1 mov rax,[rel 7FFAC47524E0h]
//! 00007FFAC46ACDC8 xor rax,rsp
//! 00007FFAC46ACDCB mov [rbp+0F0h],rax
//! 00007FFAC46ACDD2 mov r8,[rel 7FFAC474F208h]
//! 00007FFAC46ACDD9 lea rax,[rel 7FFAC46F4A58h]
//! 00007FFAC46ACDE0 xor edi,edi
//!
//! Original + patched code:
//! 00007FFAC46ACDA4 mov rax,123456789ABCDEF0h
//! 00007FFAC46ACDAE jmp rax
//! 00007FFAC46ACDB0 push r14
//! 00007FFAC46ACDB2 lea rbp,[rsp-100h]
//! 00007FFAC46ACDBA sub rsp,200h
//! 00007FFAC46ACDC1 mov rax,[rel 7FFAC47524E0h]
//! 00007FFAC46ACDC8 xor rax,rsp
//! 00007FFAC46ACDCB mov [rbp+0F0h],rax
//! 00007FFAC46ACDD2 mov r8,[rel 7FFAC474F208h]
//! 00007FFAC46ACDD9 lea rax,[rel 7FFAC46F4A58h]
//! 00007FFAC46ACDE0 xor edi,edi
//!
//! Moved code:
//! 00007FFAC48ACDA4 mov [rsp+10h],rbx
//! 00007FFAC48ACDA9 mov [rsp+18h],rsi
//! 00007FFAC48ACDAE push rbp
//! 00007FFAC48ACDAF push rdi
//! 00007FFAC48ACDB0 jmp 00007FFAC46ACDB0h
//! */
//! pub(crate) fn how_to_move_code() {
//!     let example_code = EXAMPLE_CODE.to_vec();
//!     println!("Original code:");
//!     disassemble(&example_code, EXAMPLE_CODE_RIP);
//!
//!     let mut decoder = Decoder::new(EXAMPLE_CODE_BITNESS, &example_code, DecoderOptions::NONE);
//!     decoder.set_ip(EXAMPLE_CODE_RIP);
//!
//!     // In 64-bit mode, we need 12 bytes to jump to any address:
//!     //      mov rax,imm64   // 10
//!     //      jmp rax         // 2
//!     // We overwrite rax because it's probably not used by the called function.
//!     // In 32-bit mode, a normal JMP is just 5 bytes
//!     let required_bytes = 10 + 2;
//!     let mut total_bytes = 0;
//!     let mut orig_instructions: Vec<Instruction> = Vec::new();
//!     for instr in &mut decoder {
//!         orig_instructions.push(instr);
//!         total_bytes += instr.len() as u32;
//!         if instr.is_invalid() {
//!             panic!("Found garbage");
//!         }
//!         if total_bytes >= required_bytes {
//!             break;
//!         }
//!
//!         match instr.flow_control() {
//!             FlowControl::Next => {}
//!
//!             FlowControl::UnconditionalBranch => {
//!                 if instr.op0_kind() == OpKind::NearBranch64 {
//!                     let _target = instr.near_branch_target();
//!                     // You could check if it's just jumping forward a few bytes and follow it
//!                     // but this is a simple example so we'll fail.
//!                 }
//!                 panic!("Not supported by this simple example");
//!             }
//!
//!             FlowControl::IndirectBranch
//!             | FlowControl::ConditionalBranch
//!             | FlowControl::Return
//!             | FlowControl::Call
//!             | FlowControl::IndirectCall
//!             | FlowControl::Interrupt
//!             | FlowControl::XbeginXabortXend
//!             | FlowControl::Exception => panic!("Not supported by this simple example"),
//!         }
//!     }
//!     if total_bytes < required_bytes {
//!         panic!("Not enough bytes!");
//!     }
//!     assert!(!orig_instructions.is_empty());
//!     // Create a JMP instruction that branches to the original code, except those instructions
//!     // that we'll re-encode. We don't need to do it if it already ends in 'ret'
//!     let (jmp_back_addr, add) = {
//!         let last_instr = orig_instructions.last().unwrap();
//!         if last_instr.flow_control() != FlowControl::Return {
//!             (last_instr.next_ip(), true)
//!         } else {
//!             (last_instr.next_ip(), false)
//!         }
//!     };
//!     if add {
//!         orig_instructions
//!             .push(Instruction::try_with_branch(Code::Jmp_rel32_64, jmp_back_addr).unwrap());
//!     }
//!
//!     // Relocate the code to some new location. It can fix short/near branches and
//!     // convert them to short/near/long forms if needed. This also works even if it's a
//!     // jrcxz/loop/loopcc instruction which only have short forms.
//!     //
//!     // It can currently only fix RIP relative operands if the new location is within 2GB
//!     // of the target data location.
//!     //
//!     // Note that a block is not the same thing as a basic block. A block can contain any
//!     // number of instructions, including any number of branch instructions. One block
//!     // should be enough unless you must relocate different blocks to different locations.
//!     let relocated_base_address = EXAMPLE_CODE_RIP + 0x20_0000;
//!     let block = InstructionBlock::new(&orig_instructions, relocated_base_address);
//!     // This method can also encode more than one block but that's rarely needed, see above comment.
//!     let result = match BlockEncoder::encode(decoder.bitness(), block, BlockEncoderOptions::NONE) {
//!         Err(err) => panic!("{}", err),
//!         Ok(result) => result,
//!     };
//!     let new_code = result.code_buffer;
//!
//!     // Patch the original code. Pretend that we use some OS API to write to memory...
//!     // We could use the BlockEncoder/Encoder for this but it's easy to do yourself too.
//!     // This is 'mov rax,imm64; jmp rax'
//!     const YOUR_FUNC: u64 = 0x1234_5678_9ABC_DEF0; // Address of your code
//!     let mut example_code = example_code.to_vec();
//!     example_code[0] = 0x48; // \ 'MOV RAX,imm64'
//!     example_code[1] = 0xB8; // /
//!     let mut v = YOUR_FUNC;
//!     for p in &mut example_code[2..10] {
//!         *p = v as u8;
//!         v >>= 8;
//!     }
//!     example_code[10] = 0xFF; // \ JMP RAX
//!     example_code[11] = 0xE0; // /
//!
//!     // Disassemble it
//!     println!("Original + patched code:");
//!     disassemble(&example_code, EXAMPLE_CODE_RIP);
//!
//!     // Disassemble the moved code
//!     println!("Moved code:");
//!     disassemble(&new_code, relocated_base_address);
//! }
//!
//! fn disassemble(data: &[u8], ip: u64) {
//!     let mut formatter = NasmFormatter::new();
//!     let mut output = String::new();
//!     let mut decoder = Decoder::new(EXAMPLE_CODE_BITNESS, data, DecoderOptions::NONE);
//!     decoder.set_ip(ip);
//!     for instruction in &mut decoder {
//!         output.clear();
//!         formatter.format(&instruction, &mut output);
//!         println!("{:016X} {}", instruction.ip(), output);
//!     }
//!     println!();
//! }
//!
//! const EXAMPLE_CODE_BITNESS: u32 = 64;
//! const EXAMPLE_CODE_RIP: u64 = 0x0000_7FFA_C46A_CDA4;
//! static EXAMPLE_CODE: &[u8] = &[
//!     0x48, 0x89, 0x5C, 0x24, 0x10, 0x48, 0x89, 0x74, 0x24, 0x18, 0x55, 0x57, 0x41, 0x56, 0x48, 0x8D,
//!     0xAC, 0x24, 0x00, 0xFF, 0xFF, 0xFF, 0x48, 0x81, 0xEC, 0x00, 0x02, 0x00, 0x00, 0x48, 0x8B, 0x05,
//!     0x18, 0x57, 0x0A, 0x00, 0x48, 0x33, 0xC4, 0x48, 0x89, 0x85, 0xF0, 0x00, 0x00, 0x00, 0x4C, 0x8B,
//!     0x05, 0x2F, 0x24, 0x0A, 0x00, 0x48, 0x8D, 0x05, 0x78, 0x7C, 0x04, 0x00, 0x33, 0xFF,
//! ];
//! ```
//!
//! ## Get instruction info, eg. read/written regs/mem, control flow info, etc
//!
//! Shows how to get used registers/memory and other info. It uses [`Instruction`] methods
//! and an [`InstructionInfoFactory`] to get this info.
//!
//! [`Instruction`]: struct.Instruction.html
//! [`InstructionInfoFactory`]: struct.InstructionInfoFactory.html
//!
//! ```rust
//! use iced_x86::{
//!     ConditionCode, Decoder, DecoderOptions, Instruction, InstructionInfoFactory, OpKind, RflagsBits,
//! };
//!
//! /*
//! This method produces the following output:
//! 00007FFAC46ACDA4 mov [rsp+10h],rbx
//!     OpCode: o64 89 /r
//!     Instruction: MOV r/m64, r64
//!     Encoding: Legacy
//!     Mnemonic: Mov
//!     Code: Mov_rm64_r64
//!     CpuidFeature: X64
//!     FlowControl: Next
//!     Displacement offset = 4, size = 1
//!     Memory size: 8
//!     Op0Access: Write
//!     Op1Access: Read
//!     Op0: r64_or_mem
//!     Op1: r64_reg
//!     Used reg: RSP:Read
//!     Used reg: RBX:Read
//!     Used mem: [SS:RSP+0x10;UInt64;Write]
//! 00007FFAC46ACDA9 mov [rsp+18h],rsi
//!     OpCode: o64 89 /r
//!     Instruction: MOV r/m64, r64
//!     Encoding: Legacy
//!     Mnemonic: Mov
//!     Code: Mov_rm64_r64
//!     CpuidFeature: X64
//!     FlowControl: Next
//!     Displacement offset = 4, size = 1
//!     Memory size: 8
//!     Op0Access: Write
//!     Op1Access: Read
//!     Op0: r64_or_mem
//!     Op1: r64_reg
//!     Used reg: RSP:Read
//!     Used reg: RSI:Read
//!     Used mem: [SS:RSP+0x18;UInt64;Write]
//! 00007FFAC46ACDAE push rbp
//!     OpCode: o64 50+ro
//!     Instruction: PUSH r64
//!     Encoding: Legacy
//!     Mnemonic: Push
//!     Code: Push_r64
//!     CpuidFeature: X64
//!     FlowControl: Next
//!     SP Increment: -8
//!     Op0Access: Read
//!     Op0: r64_opcode
//!     Used reg: RBP:Read
//!     Used reg: RSP:ReadWrite
//!     Used mem: [SS:RSP+0xFFFFFFFFFFFFFFF8;UInt64;Write]
//! 00007FFAC46ACDAF push rdi
//!     OpCode: o64 50+ro
//!     Instruction: PUSH r64
//!     Encoding: Legacy
//!     Mnemonic: Push
//!     Code: Push_r64
//!     CpuidFeature: X64
//!     FlowControl: Next
//!     SP Increment: -8
//!     Op0Access: Read
//!     Op0: r64_opcode
//!     Used reg: RDI:Read
//!     Used reg: RSP:ReadWrite
//!     Used mem: [SS:RSP+0xFFFFFFFFFFFFFFF8;UInt64;Write]
//! 00007FFAC46ACDB0 push r14
//!     OpCode: o64 50+ro
//!     Instruction: PUSH r64
//!     Encoding: Legacy
//!     Mnemonic: Push
//!     Code: Push_r64
//!     CpuidFeature: X64
//!     FlowControl: Next
//!     SP Increment: -8
//!     Op0Access: Read
//!     Op0: r64_opcode
//!     Used reg: R14:Read
//!     Used reg: RSP:ReadWrite
//!     Used mem: [SS:RSP+0xFFFFFFFFFFFFFFF8;UInt64;Write]
//! 00007FFAC46ACDB2 lea rbp,[rsp-100h]
//!     OpCode: o64 8D /r
//!     Instruction: LEA r64, m
//!     Encoding: Legacy
//!     Mnemonic: Lea
//!     Code: Lea_r64_m
//!     CpuidFeature: X64
//!     FlowControl: Next
//!     Displacement offset = 4, size = 4
//!     Op0Access: Write
//!     Op1Access: NoMemAccess
//!     Op0: r64_reg
//!     Op1: mem
//!     Used reg: RBP:Write
//!     Used reg: RSP:Read
//! 00007FFAC46ACDBA sub rsp,200h
//!     OpCode: o64 81 /5 id
//!     Instruction: SUB r/m64, imm32
//!     Encoding: Legacy
//!     Mnemonic: Sub
//!     Code: Sub_rm64_imm32
//!     CpuidFeature: X64
//!     FlowControl: Next
//!     Immediate offset = 3, size = 4
//!     RFLAGS Written: OF, SF, ZF, AF, CF, PF
//!     RFLAGS Modified: OF, SF, ZF, AF, CF, PF
//!     Op0Access: ReadWrite
//!     Op1Access: Read
//!     Op0: r64_or_mem
//!     Op1: imm32sex64
//!     Used reg: RSP:ReadWrite
//! 00007FFAC46ACDC1 mov rax,[7FFAC47524E0h]
//!     OpCode: o64 8B /r
//!     Instruction: MOV r64, r/m64
//!     Encoding: Legacy
//!     Mnemonic: Mov
//!     Code: Mov_r64_rm64
//!     CpuidFeature: X64
//!     FlowControl: Next
//!     Displacement offset = 3, size = 4
//!     Memory size: 8
//!     Op0Access: Write
//!     Op1Access: Read
//!     Op0: r64_reg
//!     Op1: r64_or_mem
//!     Used reg: RAX:Write
//!     Used mem: [DS:0x7FFAC47524E0;UInt64;Read]
//! 00007FFAC46ACDC8 xor rax,rsp
//!     OpCode: o64 33 /r
//!     Instruction: XOR r64, r/m64
//!     Encoding: Legacy
//!     Mnemonic: Xor
//!     Code: Xor_r64_rm64
//!     CpuidFeature: X64
//!     FlowControl: Next
//!     RFLAGS Written: SF, ZF, PF
//!     RFLAGS Cleared: OF, CF
//!     RFLAGS Undefined: AF
//!     RFLAGS Modified: OF, SF, ZF, AF, CF, PF
//!     Op0Access: ReadWrite
//!     Op1Access: Read
//!     Op0: r64_reg
//!     Op1: r64_or_mem
//!     Used reg: RAX:ReadWrite
//!     Used reg: RSP:Read
//! 00007FFAC46ACDCB mov [rbp+0F0h],rax
//!     OpCode: o64 89 /r
//!     Instruction: MOV r/m64, r64
//!     Encoding: Legacy
//!     Mnemonic: Mov
//!     Code: Mov_rm64_r64
//!     CpuidFeature: X64
//!     FlowControl: Next
//!     Displacement offset = 3, size = 4
//!     Memory size: 8
//!     Op0Access: Write
//!     Op1Access: Read
//!     Op0: r64_or_mem
//!     Op1: r64_reg
//!     Used reg: RBP:Read
//!     Used reg: RAX:Read
//!     Used mem: [SS:RBP+0xF0;UInt64;Write]
//! 00007FFAC46ACDD2 mov r8,[7FFAC474F208h]
//!     OpCode: o64 8B /r
//!     Instruction: MOV r64, r/m64
//!     Encoding: Legacy
//!     Mnemonic: Mov
//!     Code: Mov_r64_rm64
//!     CpuidFeature: X64
//!     FlowControl: Next
//!     Displacement offset = 3, size = 4
//!     Memory size: 8
//!     Op0Access: Write
//!     Op1Access: Read
//!     Op0: r64_reg
//!     Op1: r64_or_mem
//!     Used reg: R8:Write
//!     Used mem: [DS:0x7FFAC474F208;UInt64;Read]
//! 00007FFAC46ACDD9 lea rax,[7FFAC46F4A58h]
//!     OpCode: o64 8D /r
//!     Instruction: LEA r64, m
//!     Encoding: Legacy
//!     Mnemonic: Lea
//!     Code: Lea_r64_m
//!     CpuidFeature: X64
//!     FlowControl: Next
//!     Displacement offset = 3, size = 4
//!     Op0Access: Write
//!     Op1Access: NoMemAccess
//!     Op0: r64_reg
//!     Op1: mem
//!     Used reg: RAX:Write
//! 00007FFAC46ACDE0 xor edi,edi
//!     OpCode: o32 33 /r
//!     Instruction: XOR r32, r/m32
//!     Encoding: Legacy
//!     Mnemonic: Xor
//!     Code: Xor_r32_rm32
//!     CpuidFeature: INTEL386
//!     FlowControl: Next
//!     RFLAGS Cleared: OF, SF, CF
//!     RFLAGS Set: ZF, PF
//!     RFLAGS Undefined: AF
//!     RFLAGS Modified: OF, SF, ZF, AF, CF, PF
//!     Op0Access: Write
//!     Op1Access: None
//!     Op0: r32_reg
//!     Op1: r32_or_mem
//!     Used reg: RDI:Write
//! */
//! pub(crate) fn how_to_get_instruction_info() {
//!     let mut decoder = Decoder::new(EXAMPLE_CODE_BITNESS, EXAMPLE_CODE, DecoderOptions::NONE);
//!     decoder.set_ip(EXAMPLE_CODE_RIP);
//!
//!     // Use a factory to create the instruction info if you need register and
//!     // memory usage. If it's something else, eg. encoding, flags, etc, there
//!     // are Instruction methods that can be used instead.
//!     let mut info_factory = InstructionInfoFactory::new();
//!     let mut instr = Instruction::default();
//!     while decoder.can_decode() {
//!         decoder.decode_out(&mut instr);
//!
//!         // Gets offsets in the instruction of the displacement and immediates and their sizes.
//!         // This can be useful if there are relocations in the binary. The encoder has a similar
//!         // method. This method must be called after decode() and you must pass in the last
//!         // instruction decode() returned.
//!         let offsets = decoder.get_constant_offsets(&instr);
//!
//!         // For quick hacks, it's fine to use the Display trait to format an instruction,
//!         // but for real code, use a formatter, eg. MasmFormatter. See other examples.
//!         println!("{:016X} {}", instr.ip(), instr);
//!
//!         let op_code = instr.op_code();
//!         let info = info_factory.info(&instr);
//!         let fpu_info = instr.fpu_stack_increment_info();
//!         println!("    OpCode: {}", op_code.op_code_string());
//!         println!("    Instruction: {}", op_code.instruction_string());
//!         println!("    Encoding: {:?}", instr.encoding());
//!         println!("    Mnemonic: {:?}", instr.mnemonic());
//!         println!("    Code: {:?}", instr.code());
//!         println!(
//!             "    CpuidFeature: {}",
//!             instr
//!                 .cpuid_features()
//!                 .iter()
//!                 .map(|&a| format!("{:?}", a))
//!                 .collect::<Vec<String>>()
//!                 .join(" and ")
//!         );
//!         println!("    FlowControl: {:?}", instr.flow_control());
//!         if offsets.has_displacement() {
//!             println!(
//!                 "    Displacement offset = {}, size = {}",
//!                 offsets.displacement_offset(),
//!                 offsets.displacement_size()
//!             );
//!         }
//!         if fpu_info.writes_top() {
//!             if fpu_info.increment() == 0 {
//!                 println!("    FPU TOP: the instruction overwrites TOP");
//!             } else {
//!                 println!("    FPU TOP inc: {}", fpu_info.increment());
//!             }
//!             println!(
//!                 "    FPU TOP cond write: {}",
//!                 if fpu_info.conditional() { "true" } else { "false" }
//!             );
//!         }
//!         if offsets.has_immediate() {
//!             println!(
//!                 "    Immediate offset = {}, size = {}",
//!                 offsets.immediate_offset(),
//!                 offsets.immediate_size()
//!             );
//!         }
//!         if offsets.has_immediate2() {
//!             println!(
//!                 "    Immediate #2 offset = {}, size = {}",
//!                 offsets.immediate_offset2(),
//!                 offsets.immediate_size2()
//!             );
//!         }
//!         if instr.is_stack_instruction() {
//!             println!("    SP Increment: {}", instr.stack_pointer_increment());
//!         }
//!         if instr.condition_code() != ConditionCode::None {
//!             println!("    Condition code: {:?}", instr.condition_code());
//!         }
//!         if instr.rflags_read() != RflagsBits::NONE {
//!             println!("    RFLAGS Read: {}", flags(instr.rflags_read()));
//!         }
//!         if instr.rflags_written() != RflagsBits::NONE {
//!             println!("    RFLAGS Written: {}", flags(instr.rflags_written()));
//!         }
//!         if instr.rflags_cleared() != RflagsBits::NONE {
//!             println!("    RFLAGS Cleared: {}", flags(instr.rflags_cleared()));
//!         }
//!         if instr.rflags_set() != RflagsBits::NONE {
//!             println!("    RFLAGS Set: {}", flags(instr.rflags_set()));
//!         }
//!         if instr.rflags_undefined() != RflagsBits::NONE {
//!             println!("    RFLAGS Undefined: {}", flags(instr.rflags_undefined()));
//!         }
//!         if instr.rflags_modified() != RflagsBits::NONE {
//!             println!("    RFLAGS Modified: {}", flags(instr.rflags_modified()));
//!         }
//!         for i in 0..instr.op_count() {
//!             let op_kind = instr.try_op_kind(i).unwrap();
//!             if op_kind == OpKind::Memory {
//!                 let size = instr.memory_size().size();
//!                 if size != 0 {
//!                     println!("    Memory size: {}", size);
//!                 }
//!                 break;
//!             }
//!         }
//!         for i in 0..instr.op_count() {
//!             println!("    Op{}Access: {:?}", i, info.try_op_access(i).unwrap());
//!         }
//!         for i in 0..op_code.op_count() {
//!             println!("    Op{}: {:?}", i, op_code.try_op_kind(i).unwrap());
//!         }
//!         for reg_info in info.used_registers() {
//!             println!("    Used reg: {:?}", reg_info);
//!         }
//!         for mem_info in info.used_memory() {
//!             println!("    Used mem: {:?}", mem_info);
//!         }
//!     }
//! }
//!
//! fn flags(rf: u32) -> String {
//!     fn append(sb: &mut String, s: &str) {
//!         if !sb.is_empty() {
//!             sb.push_str(", ");
//!         }
//!         sb.push_str(s);
//!     }
//!
//!     let mut sb = String::new();
//!     if (rf & RflagsBits::OF) != 0 {
//!         append(&mut sb, "OF");
//!     }
//!     if (rf & RflagsBits::SF) != 0 {
//!         append(&mut sb, "SF");
//!     }
//!     if (rf & RflagsBits::ZF) != 0 {
//!         append(&mut sb, "ZF");
//!     }
//!     if (rf & RflagsBits::AF) != 0 {
//!         append(&mut sb, "AF");
//!     }
//!     if (rf & RflagsBits::CF) != 0 {
//!         append(&mut sb, "CF");
//!     }
//!     if (rf & RflagsBits::PF) != 0 {
//!         append(&mut sb, "PF");
//!     }
//!     if (rf & RflagsBits::DF) != 0 {
//!         append(&mut sb, "DF");
//!     }
//!     if (rf & RflagsBits::IF) != 0 {
//!         append(&mut sb, "IF");
//!     }
//!     if (rf & RflagsBits::AC) != 0 {
//!         append(&mut sb, "AC");
//!     }
//!     if (rf & RflagsBits::UIF) != 0 {
//!         append(&mut sb, "UIF");
//!     }
//!     if sb.is_empty() {
//!         sb.push_str("<empty>");
//!     }
//!     sb
//! }
//!
//! const EXAMPLE_CODE_BITNESS: u32 = 64;
//! const EXAMPLE_CODE_RIP: u64 = 0x0000_7FFA_C46A_CDA4;
//! static EXAMPLE_CODE: &[u8] = &[
//!     0x48, 0x89, 0x5C, 0x24, 0x10, 0x48, 0x89, 0x74, 0x24, 0x18, 0x55, 0x57, 0x41, 0x56, 0x48, 0x8D,
//!     0xAC, 0x24, 0x00, 0xFF, 0xFF, 0xFF, 0x48, 0x81, 0xEC, 0x00, 0x02, 0x00, 0x00, 0x48, 0x8B, 0x05,
//!     0x18, 0x57, 0x0A, 0x00, 0x48, 0x33, 0xC4, 0x48, 0x89, 0x85, 0xF0, 0x00, 0x00, 0x00, 0x4C, 0x8B,
//!     0x05, 0x2F, 0x24, 0x0A, 0x00, 0x48, 0x8D, 0x05, 0x78, 0x7C, 0x04, 0x00, 0x33, 0xFF,
//! ];
//! ```
//!
//! ## Get the virtual address of a memory operand
//!
//! ```rust
//! use iced_x86::{Decoder, DecoderOptions, Register};
//!
//! pub(crate) fn how_to_get_virtual_address() {
//!     // add [rdi+r12*8-5AA5EDCCh],esi
//!     let bytes = b"\x42\x01\xB4\xE7\x34\x12\x5A\xA5";
//!     let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
//!     let instr = decoder.decode();
//!
//!     let va = instr.try_virtual_address(0, 0, |register, _element_index, _element_size| {
//!         match register {
//!             // The base address of ES, CS, SS and DS is always 0 in 64-bit mode
//!             Register::ES | Register::CS | Register::SS | Register::DS => Some(0),
//!             Register::RDI => Some(0x0000_0000_1000_0000),
//!             Register::R12 => Some(0x0000_0004_0000_0000),
//!             _ => None,
//!         }
//!     });
//!     assert_eq!(Some(0x0000_001F_B55A_1234), va);
//! }
//! ```
//!
//! ## Disassemble old/deprecated CPU instructions
//!
//! ```rust
//! use iced_x86::{Decoder, DecoderOptions, Formatter, Instruction, NasmFormatter};
//!
//! /*
//! This method produces the following output:
//! 731E0A03 bndmov bnd1, [eax]
//! 731E0A07 mov tr3, esi
//! 731E0A0A rdshr [eax]
//! 731E0A0D dmint
//! 731E0A0F svdc [eax], cs
//! 731E0A12 cpu_read
//! 731E0A14 pmvzb mm1, [eax]
//! 731E0A17 frinear
//! 731E0A19 altinst
//! */
//! pub(crate) fn how_to_disassemble_old_instrs() {
//!     #[rustfmt::skip]
//!     let bytes = &[
//!         // bndmov bnd1,[eax]
//!         0x66, 0x0F, 0x1A, 0x08,
//!         // mov tr3,esi
//!         0x0F, 0x26, 0xDE,
//!         // rdshr [eax]
//!         0x0F, 0x36, 0x00,
//!         // dmint
//!         0x0F, 0x39,
//!         // svdc [eax],cs
//!         0x0F, 0x78, 0x08,
//!         // cpu_read
//!         0x0F, 0x3D,
//!         // pmvzb mm1,[eax]
//!         0x0F, 0x58, 0x08,
//!         // frinear
//!         0xDF, 0xFC,
//!         // altinst
//!         0x0F, 0x3F,
//!     ];
//!
//!     // Enable decoding of Cyrix/Geode instructions, Centaur ALTINST, MOV to/from TR
//!     // and MPX instructions.
//!     // There are other options to enable other instructions such as UMOV, etc.
//!     // These are deprecated instructions or only used by old CPUs so they're not
//!     // enabled by default. Some newer instructions also use the same opcodes as
//!     // some of these old instructions.
//!     const DECODER_OPTIONS: u32 = DecoderOptions::MPX
//!         | DecoderOptions::MOV_TR
//!         | DecoderOptions::CYRIX
//!         | DecoderOptions::CYRIX_DMI
//!         | DecoderOptions::ALTINST;
//!     let mut decoder = Decoder::new(32, bytes, DECODER_OPTIONS);
//!     decoder.set_ip(0x731E_0A03);
//!
//!     let mut formatter = NasmFormatter::new();
//!     formatter.options_mut().set_space_after_operand_separator(true);
//!     let mut output = String::new();
//!
//!     let mut instruction = Instruction::default();
//!     while decoder.can_decode() {
//!         decoder.decode_out(&mut instruction);
//!
//!         output.clear();
//!         formatter.format(&instruction, &mut output);
//!
//!         println!("{:08X} {}", instruction.ip(), &output);
//!     }
//! }
//! ```
//!
//! ## Minimum supported `rustc` version
//!
//! iced-x86 supports `rustc` `1.41.0` or later.
//! This is checked in CI builds where the minimum supported version and the latest stable version are used to build the source code and run tests.
//!
//! Bumping the minimum supported version of `rustc` is considered a minor breaking change. The minor version of iced-x86 will be incremented.

#![doc(html_logo_url = "https://raw.githubusercontent.com/0xd4d/iced/master/logo.png")]
#![doc(html_root_url = "https://docs.rs/iced-x86/1.10.2")]
#![allow(unknown_lints)]
#![warn(absolute_paths_not_starting_with_crate)]
#![warn(anonymous_parameters)]
#![warn(deprecated_in_future)]
#![warn(keyword_idents)]
#![warn(meta_variable_misuse)]
#![warn(missing_copy_implementations)]
#![warn(missing_debug_implementations)]
#![warn(missing_docs)]
#![warn(non_ascii_idents)]
#![warn(trivial_casts)]
#![warn(trivial_numeric_casts)]
#![warn(unused_extern_crates)]
#![warn(unused_import_braces)]
#![warn(unused_labels)]
#![warn(unused_lifetimes)]
#![warn(unused_must_use)]
#![warn(unused_qualifications)]
#![warn(unused_results)]
#![allow(clippy::cast_lossless)]
#![allow(clippy::collapsible_if)]
#![allow(clippy::field_reassign_with_default)]
#![allow(clippy::manual_range_contains)]
#![allow(clippy::manual_strip)] // Not supported if < 1.45.0
#![allow(clippy::match_like_matches_macro)] // Not supported if < 1.42.0
#![allow(clippy::match_ref_pats)]
#![allow(clippy::ptr_eq)]
#![allow(clippy::too_many_arguments)]
#![allow(clippy::type_complexity)]
#![allow(clippy::wrong_self_convention)]
#![warn(clippy::clone_on_ref_ptr)]
#![warn(clippy::dbg_macro)]
#![warn(clippy::debug_assert_with_mut_call)]
#![warn(clippy::default_trait_access)]
#![warn(clippy::doc_markdown)]
#![warn(clippy::empty_line_after_outer_attr)]
#![warn(clippy::explicit_into_iter_loop)]
#![warn(clippy::explicit_iter_loop)]
#![warn(clippy::fallible_impl_from)]
#![warn(clippy::implicit_saturating_sub)]
#![warn(clippy::large_digit_groups)]
#![warn(clippy::let_unit_value)]
#![warn(clippy::match_bool)]
#![warn(clippy::missing_errors_doc)]
#![warn(clippy::missing_inline_in_public_items)]
#![warn(clippy::must_use_candidate)]
#![warn(clippy::needless_borrow)]
#![warn(clippy::print_stdout)]
#![warn(clippy::rc_buffer)]
#![warn(clippy::redundant_closure_for_method_calls)]
#![warn(clippy::redundant_closure)]
#![warn(clippy::same_functions_in_if_condition)]
#![warn(clippy::todo)]
#![warn(clippy::unimplemented)]
#![warn(clippy::unnested_or_patterns)]
#![warn(clippy::unreadable_literal)]
#![warn(clippy::unused_self)]
#![warn(clippy::used_underscore_binding)]
#![warn(clippy::useless_let_if_seq)]
#![warn(clippy::useless_transmute)]
#![cfg_attr(not(feature = "std"), no_std)]

// This should be the only place in the source code that uses no_std
#[cfg(all(feature = "std", feature = "no_std"))]
compile_error!("`std` and `no_std` features can't be used at the same time");
#[cfg(all(not(feature = "std"), not(feature = "no_std")))]
compile_error!("`std` or `no_std` feature must be defined");

#[cfg(any(not(feature = "std"), feature = "encoder", feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
#[cfg_attr(any(feature = "encoder", feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"), macro_use)]
extern crate alloc;
#[cfg(feature = "std")]
extern crate core;
#[cfg(any(feature = "decoder", feature = "encoder", feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
#[cfg_attr(
	any(feature = "decoder", feature = "encoder", feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"),
	macro_use
)]
extern crate lazy_static;
#[macro_use]
extern crate static_assertions;
#[cfg(not(feature = "std"))]
#[cfg(all(feature = "encoder", feature = "block_encoder"))]
extern crate hashbrown;

#[cfg(all(feature = "encoder", feature = "block_encoder"))]
mod block_enc;
mod code;
#[cfg(any(feature = "decoder", feature = "encoder"))]
mod constant_offsets;
#[cfg(any(feature = "decoder", feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
mod data_reader;
#[cfg(feature = "decoder")]
mod decoder;
#[cfg(feature = "encoder")]
mod encoder;
mod enums;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
mod formatter;
pub(crate) mod iced_constants;
mod iced_error;
mod iced_features;
#[cfg(feature = "instr_info")]
mod info;
mod instruction;
mod instruction_internal;
mod instruction_memory_sizes;
mod instruction_op_counts;
mod memory_size;
mod mnemonic;
mod mnemonics;
mod register;
#[cfg(test)]
pub(crate) mod test;
#[cfg(test)]
pub(crate) mod test_utils;
#[cfg(any(feature = "decoder", feature = "encoder"))]
mod tuple_type_tbl;

#[cfg(all(feature = "encoder", feature = "block_encoder"))]
pub use self::block_enc::*;
pub use self::code::*;
#[cfg(any(feature = "decoder", feature = "encoder"))]
pub use self::constant_offsets::*;
#[cfg(feature = "decoder")]
pub use self::decoder::*;
#[cfg(feature = "encoder")]
pub use self::encoder::*;
pub use self::enums::*;
#[cfg(any(feature = "gas", feature = "intel", feature = "masm", feature = "nasm", feature = "fast_fmt"))]
pub use self::formatter::*;
pub use self::iced_error::*;
pub use self::iced_features::*;
#[cfg(feature = "instr_info")]
pub use self::info::*;
pub use self::instruction::*;
pub use self::memory_size::*;
pub use self::mnemonic::*;
pub use self::register::*;
