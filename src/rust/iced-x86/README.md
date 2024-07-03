iced-x86
[![Latest version](https://img.shields.io/crates/v/iced-x86.svg)](https://crates.io/crates/iced-x86)
[![Documentation](https://docs.rs/iced-x86/badge.svg)](https://docs.rs/iced-x86)
[![Minimum rustc version](https://img.shields.io/badge/rustc-1.63.0+-blue.svg)](#minimum-supported-rustc-version)
![License](https://img.shields.io/crates/l/iced-x86.svg)

iced-x86 is a blazing fast and correct x86 (16/32/64-bit) instruction decoder, disassembler and assembler written in Rust.

- 👍 Supports all Intel and AMD instructions
- 👍 Correct: All instructions are tested and iced has been tested against other disassemblers/assemblers (xed, gas, objdump, masm, dumpbin, nasm, ndisasm) and fuzzed
- 👍 100% Rust code
- 👍 The formatter supports masm, nasm, gas (AT&T), Intel (XED) and there are many options to customize the output
- 👍 Blazing fast: Decodes >250 MB/s and decode+format >130 MB/s ([see here](https://github.com/icedland/disas-bench/tree/a865849deacfb6c33ee0e78f3a3ad7f4c82099f5#results))
- 👍 Small decoded instructions, only 40 bytes and the decoder doesn't allocate any memory
- 👍 Create instructions with code assembler, eg. `asm.mov(eax, edx)`
- 👍 The encoder can be used to re-encode decoded instructions at any address
- 👍 API to get instruction info, eg. read/written registers, memory and rflags bits; CPUID feature flag, control flow info, etc
- 👍 Supports `#![no_std]` and `WebAssembly`
- 👍 Supports `rustc` `1.63.0` or later
- 👍 Few dependencies (`lazy_static`)
- 👍 License: MIT

## Usage

Add this to your `Cargo.toml`:

```toml
[dependencies]
iced-x86 = "1.21.0"
```

Or to customize which features to use:

```toml
[dependencies.iced-x86]
version = "1.21.0"
default-features = false
# See below for all features
features = ["std", "decoder", "masm"]
```

## Crate feature flags

You can enable/disable these in your `Cargo.toml` file.

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
- `code_asm`: Enables [`CodeAssembler`] to allow easy creation of instructions, eg. `a.xor(ecx, dword_ptr(edx))` instead of using the more verbose `Instruction::with*()` methods.
- `serde`: Enables serialization support ([`Instruction`]). Not guaranteed to work if different versions of iced was used to serialize and deserialize it.
- `std`: (👍 Enabled by default) Enables the `std` crate. `std` or `no_std` must be defined, but not both.
- `no_std`: Enables `#![no_std]`. `std` or `no_std` must be defined, but not both. This feature uses the `alloc` crate.
- `mvex`: Enables `MVEX` instructions (Knights Corner). You must also pass in `DecoderOptions::KNC` to the [`Decoder`] constructor.
- `exhaustive_enums`: Enables exhaustive enums, i.e., no enum has the `#[non_exhaustive]` attribute

[`BlockEncoder`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.BlockEncoder.html
[`CodeAssembler`]: https://docs.rs/iced-x86/1.21.0/iced_x86/code_asm/struct.CodeAssembler.html
[`Instruction`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.Instruction.html
[`OpCodeInfo`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.OpCodeInfo.html

## How-tos

- [Disassemble (decode and format instructions)](#disassemble-decode-and-format-instructions)
- [Assemble instructions](#assemble-instructions)
- [Disassemble with a symbol resolver](#disassemble-with-a-symbol-resolver)
- [Disassemble with colorized text](#disassemble-with-colorized-text)
- [Move code in memory (eg. hook a function)](#move-code-in-memory-eg-hook-a-function)
- [Get instruction info, eg. read/written regs/mem, control flow info, etc](#get-instruction-info-eg-readwritten-regsmem-control-flow-info-etc)
- [Get the virtual address of a memory operand](#get-the-virtual-address-of-a-memory-operand)
- [Disassemble old/deprecated CPU instructions](#disassemble-olddeprecated-cpu-instructions)
- [Disassemble as fast as possible](#disassemble-as-fast-as-possible)
- [Create and encode instructions](#create-and-encode-instructions)

## Disassemble (decode and format instructions)

This example uses a [`Decoder`] and one of the [`Formatter`]s to decode and format the code,
eg. [`GasFormatter`], [`IntelFormatter`], [`MasmFormatter`], [`NasmFormatter`], [`SpecializedFormatter<TraitOptions>`] (or [`FastFormatter`]).

[`Decoder`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.Decoder.html
[`Formatter`]: https://docs.rs/iced-x86/1.21.0/iced_x86/trait.Formatter.html
[`GasFormatter`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.GasFormatter.html
[`IntelFormatter`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.IntelFormatter.html
[`MasmFormatter`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.MasmFormatter.html
[`NasmFormatter`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.NasmFormatter.html
[`SpecializedFormatter<TraitOptions>`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.SpecializedFormatter.html
[`FastFormatter`]: https://docs.rs/iced-x86/1.21.0/iced_x86/type.FastFormatter.html

```rust
use iced_x86::{Decoder, DecoderOptions, Formatter, Instruction, NasmFormatter};

/*
This method produces the following output:
00007FFAC46ACDA4 48895C2410           mov       [rsp+10h],rbx
00007FFAC46ACDA9 4889742418           mov       [rsp+18h],rsi
00007FFAC46ACDAE 55                   push      rbp
00007FFAC46ACDAF 57                   push      rdi
00007FFAC46ACDB0 4156                 push      r14
00007FFAC46ACDB2 488DAC2400FFFFFF     lea       rbp,[rsp-100h]
00007FFAC46ACDBA 4881EC00020000       sub       rsp,200h
00007FFAC46ACDC1 488B0518570A00       mov       rax,[rel 7FFA`C475`24E0h]
00007FFAC46ACDC8 4833C4               xor       rax,rsp
00007FFAC46ACDCB 488985F0000000       mov       [rbp+0F0h],rax
00007FFAC46ACDD2 4C8B052F240A00       mov       r8,[rel 7FFA`C474`F208h]
00007FFAC46ACDD9 488D05787C0400       lea       rax,[rel 7FFA`C46F`4A58h]
00007FFAC46ACDE0 33FF                 xor       edi,edi
*/
#[allow(dead_code)]
pub(crate) fn how_to_disassemble() {
    let bytes = EXAMPLE_CODE;
    let mut decoder =
        Decoder::with_ip(EXAMPLE_CODE_BITNESS, bytes, EXAMPLE_CODE_RIP, DecoderOptions::NONE);

    // Formatters: Masm*, Nasm*, Gas* (AT&T) and Intel* (XED).
    // For fastest code, see `SpecializedFormatter` which is ~3.3x faster. Use it if formatting
    // speed is more important than being able to re-assemble formatted instructions.
    let mut formatter = NasmFormatter::new();

    // Change some options, there are many more
    formatter.options_mut().set_digit_separator("`");
    formatter.options_mut().set_first_operand_char_index(10);

    // String implements FormatterOutput
    let mut output = String::new();

    // Initialize this outside the loop because decode_out() writes to every field
    let mut instruction = Instruction::default();

    // The decoder also implements Iterator/IntoIterator so you could use a for loop:
    //      for instruction in &mut decoder { /* ... */ }
    // or collect():
    //      let instructions: Vec<_> = decoder.into_iter().collect();
    // but can_decode()/decode_out() is a little faster:
    while decoder.can_decode() {
        // There's also a decode() method that returns an instruction but that also
        // means it copies an instruction (40 bytes):
        //     instruction = decoder.decode();
        decoder.decode_out(&mut instruction);

        // Format the instruction ("disassemble" it)
        output.clear();
        formatter.format(&instruction, &mut output);

        // Eg. "00007FFAC46ACDB2 488DAC2400FFFFFF     lea       rbp,[rsp-100h]"
        print!("{:016X} ", instruction.ip());
        let start_index = (instruction.ip() - EXAMPLE_CODE_RIP) as usize;
        let instr_bytes = &bytes[start_index..start_index + instruction.len()];
        for b in instr_bytes.iter() {
            print!("{:02X}", b);
        }
        if instr_bytes.len() < HEXBYTES_COLUMN_BYTE_LENGTH {
            for _ in 0..HEXBYTES_COLUMN_BYTE_LENGTH - instr_bytes.len() {
                print!("  ");
            }
        }
        println!(" {}", output);
    }
}

const HEXBYTES_COLUMN_BYTE_LENGTH: usize = 10;
const EXAMPLE_CODE_BITNESS: u32 = 64;
const EXAMPLE_CODE_RIP: u64 = 0x0000_7FFA_C46A_CDA4;
static EXAMPLE_CODE: &[u8] = &[
    0x48, 0x89, 0x5C, 0x24, 0x10, 0x48, 0x89, 0x74, 0x24, 0x18, 0x55, 0x57, 0x41, 0x56, 0x48, 0x8D,
    0xAC, 0x24, 0x00, 0xFF, 0xFF, 0xFF, 0x48, 0x81, 0xEC, 0x00, 0x02, 0x00, 0x00, 0x48, 0x8B, 0x05,
    0x18, 0x57, 0x0A, 0x00, 0x48, 0x33, 0xC4, 0x48, 0x89, 0x85, 0xF0, 0x00, 0x00, 0x00, 0x4C, 0x8B,
    0x05, 0x2F, 0x24, 0x0A, 0x00, 0x48, 0x8D, 0x05, 0x78, 0x7C, 0x04, 0x00, 0x33, 0xFF,
];
```

## Assemble instructions

This allows you to easily create instructions (eg. `a.xor(eax, ecx)?`) without having to use the more verbose `Instruction::with*()` functions.

This requires the `code_asm` feature to use (not enabled by default). Add it to your `Cargo.toml`:

```toml
[dependencies.iced-x86]
version = "1.21.0"
features = ["code_asm"]
```

```rust
use iced_x86::code_asm::*;

#[allow(dead_code)]
pub(crate) fn how_to_use_code_assembler() -> Result<(), IcedError> {
    let mut a = CodeAssembler::new(64)?;

    // Anytime you add something to a register (or subtract from it), you create a
    // memory operand. You can also call word_ptr(), dword_bcst() etc to create memory
    // operands.
    let _ = rax; // register
    let _ = rax + 0; // memory with no size hint
    let _ = ptr(rax); // memory with no size hint
    let _ = rax + rcx * 4 - 123; // memory with no size hint
    // To create a memory operand with only a displacement or only a base register,
    // you can call one of the memory fns:
    let _ = qword_ptr(123); // memory with a qword size hint
    let _ = dword_bcst(rcx); // memory (broadcast) with a dword size hint
    // To add a segment override, call the segment methods:
    let _ = ptr(rax).fs(); // fs:[rax]

    // Each mnemonic is a method
    a.push(rcx)?;
    // There are a few exceptions where you must append `_<opcount>` to the mnemonic to
    // get the instruction you need:
    a.ret()?;
    a.ret_1(123)?;
    // Use byte_ptr(), word_bcst(), etc to force the arg to a memory operand and to add a
    // size hint
    a.xor(byte_ptr(rdx+r14*4+123), 0x10)?;
    // Prefixes are also methods
    a.rep().stosd()?;
    // Sometimes, you must add an integer suffix to help the compiler:
    a.mov(rax, 0x1234_5678_9ABC_DEF0u64)?;

    // Create labels that can be referenced by code
    let mut loop_lbl1 = a.create_label();
    let mut after_loop1 = a.create_label();
    a.mov(ecx, 10)?;
    a.set_label(&mut loop_lbl1)?;
    // If needed, a zero-bytes instruction can be used as a label but this is optional
    a.zero_bytes()?;
    a.dec(ecx)?;
    a.jp(after_loop1)?;
    a.jne(loop_lbl1)?;
    a.set_label(&mut after_loop1)?;

    // It's possible to reference labels with RIP-relative addressing
    let mut skip_data = a.create_label();
    let mut data = a.create_label();
    a.jmp(skip_data)?;
    a.set_label(&mut data)?;
    a.db(b"\x90\xCC\xF1\x90")?;
    a.set_label(&mut skip_data)?;
    a.lea(rax, ptr(data))?;

    // AVX512 opmasks, {z}, {sae}, {er} and broadcasting are also supported:
    a.vsqrtps(zmm16.k2().z(), dword_bcst(rcx))?;
    a.vsqrtps(zmm1.k2().z(), zmm23.rd_sae())?;
    // Sometimes, the encoder doesn't know if you want VEX or EVEX encoding.
    // You can force EVEX globally like so:
    a.set_prefer_vex(false);
    a.vucomiss(xmm31, xmm15.sae())?;
    a.vucomiss(xmm31, ptr(rcx))?;
    // or call vex()/evex() to override the encoding option:
    a.evex().vucomiss(xmm31, xmm15.sae())?;
    a.vex().vucomiss(xmm15, xmm14)?;

    // Encode all added instructions.
    // Use `assemble_options()` if you must get the address of a label
    let bytes = a.assemble(0x1234_5678)?;
    assert_eq!(bytes.len(), 82);
    // If you don't want to encode them, you can get all instructions by calling
    // one of these methods:
    let instrs = a.instructions(); // Get a reference to the internal vec
    assert_eq!(instrs.len(), 19);
    let instrs = a.take_instructions(); // Take ownership of the vec with all instructions
    assert_eq!(instrs.len(), 19);
    assert_eq!(a.instructions().len(), 0);

    Ok(())
}
```

## Disassemble with a symbol resolver

Creates a custom [`SymbolResolver`] that is called by a [`Formatter`].

[`SymbolResolver`]: https://docs.rs/iced-x86/1.21.0/iced_x86/trait.SymbolResolver.html
[`Formatter`]: https://docs.rs/iced-x86/1.21.0/iced_x86/trait.Formatter.html

```rust
use iced_x86::{
    Decoder, DecoderOptions, Formatter, Instruction, MasmFormatter, SymbolResolver, SymbolResult,
};
use std::collections::HashMap;

struct MySymbolResolver {
    map: HashMap<u64, String>,
}

impl SymbolResolver for MySymbolResolver {
    fn symbol(
        &mut self, _instruction: &Instruction, _operand: u32, _instruction_operand: Option<u32>,
        address: u64, _address_size: u32,
    ) -> Option<SymbolResult> {
        if let Some(symbol_string) = self.map.get(&address) {
            // The 'address' arg is the address of the symbol and doesn't have to be identical
            // to the 'address' arg passed to symbol(). If it's different from the input
            // address, the formatter will add +N or -N, eg. '[rax+symbol+123]'
            Some(SymbolResult::with_str(address, symbol_string.as_str()))
        } else {
            None
        }
    }
}

#[allow(dead_code)]
pub(crate) fn how_to_resolve_symbols() {
    let bytes = b"\x48\x8B\x8A\xA5\x5A\xA5\x5A";
    let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
    let instr = decoder.decode();

    let mut sym_map: HashMap<u64, String> = HashMap::new();
    sym_map.insert(0x5AA5_5AA5, String::from("my_data"));

    let mut output = String::new();
    let resolver = Box::new(MySymbolResolver { map: sym_map });
    // Create a formatter that uses our symbol resolver
    let mut formatter = MasmFormatter::with_options(Some(resolver), None);

    // This will call the symbol resolver for each immediate / displacement
    // it finds in the instruction.
    formatter.format(&instr, &mut output);

    // Prints: mov rcx,[rdx+my_data]
    println!("{}", output);
}
```

## Disassemble with colorized text

Creates a custom [`FormatterOutput`] that is called by a [`Formatter`].

This example will fail to compile unless you install the `colored` crate, see below.

[`FormatterOutput`]: https://docs.rs/iced-x86/1.21.0/iced_x86/trait.FormatterOutput.html
[`Formatter`]: https://docs.rs/iced-x86/1.21.0/iced_x86/trait.Formatter.html

```rust compile_fail
// This example uses crate colored = "2.0.0"
use colored::{ColoredString, Colorize};
use iced_x86::{
    Decoder, DecoderOptions, Formatter, FormatterOutput, FormatterTextKind, IntelFormatter,
};

// Custom formatter output that stores the output in a vector.
struct MyFormatterOutput {
    vec: Vec<(String, FormatterTextKind)>,
}

impl MyFormatterOutput {
    pub fn new() -> Self {
        Self { vec: Vec::new() }
    }
}

impl FormatterOutput for MyFormatterOutput {
    fn write(&mut self, text: &str, kind: FormatterTextKind) {
        // This allocates a string. If that's a problem, just call print!() here
        // instead of storing the result in a vector.
        self.vec.push((String::from(text), kind));
    }
}

#[allow(dead_code)]
pub(crate) fn how_to_colorize_text() {
    let bytes = EXAMPLE_CODE;
    let mut decoder =
        Decoder::with_ip(EXAMPLE_CODE_BITNESS, bytes, EXAMPLE_CODE_RIP, DecoderOptions::NONE);

    let mut formatter = IntelFormatter::new();
    formatter.options_mut().set_first_operand_char_index(8);
    let mut output = MyFormatterOutput::new();
    for instruction in &mut decoder {
        output.vec.clear();
        // The formatter calls output.write() which will update vec with text/colors
        formatter.format(&instruction, &mut output);
        for (text, kind) in output.vec.iter() {
            print!("{}", get_color(text.as_str(), *kind));
        }
        println!();
    }
}

fn get_color(s: &str, kind: FormatterTextKind) -> ColoredString {
    match kind {
        FormatterTextKind::Directive | FormatterTextKind::Keyword => s.bright_yellow(),
        FormatterTextKind::Prefix | FormatterTextKind::Mnemonic => s.bright_red(),
        FormatterTextKind::Register => s.bright_blue(),
        FormatterTextKind::Number => s.bright_cyan(),
        _ => s.white(),
    }
}

const EXAMPLE_CODE_BITNESS: u32 = 64;
const EXAMPLE_CODE_RIP: u64 = 0x0000_7FFA_C46A_CDA4;
static EXAMPLE_CODE: &[u8] = &[
    0x48, 0x89, 0x5C, 0x24, 0x10, 0x48, 0x89, 0x74, 0x24, 0x18, 0x55, 0x57, 0x41, 0x56, 0x48, 0x8D,
    0xAC, 0x24, 0x00, 0xFF, 0xFF, 0xFF, 0x48, 0x81, 0xEC, 0x00, 0x02, 0x00, 0x00, 0x48, 0x8B, 0x05,
    0x18, 0x57, 0x0A, 0x00, 0x48, 0x33, 0xC4, 0x48, 0x89, 0x85, 0xF0, 0x00, 0x00, 0x00, 0x4C, 0x8B,
    0x05, 0x2F, 0x24, 0x0A, 0x00, 0x48, 0x8D, 0x05, 0x78, 0x7C, 0x04, 0x00, 0x33, 0xFF,
];
```

## Move code in memory (eg. hook a function)

Uses instruction info API and the encoder to patch a function to jump to the programmer's function.

```rust
use iced_x86::{
    BlockEncoder, BlockEncoderOptions, Code, Decoder, DecoderOptions, FlowControl, Formatter,
    IcedError, Instruction, InstructionBlock, NasmFormatter, OpKind,
};

// Decodes instructions from some address, then encodes them starting at some
// other address. This can be used to hook a function. You decode enough instructions
// until you have enough bytes to add a JMP instruction that jumps to your code.
// Your code will then conditionally jump to the original code that you re-encoded.
//
// This code uses the BlockEncoder which will help with some things, eg. converting
// short branches to longer branches if the target is too far away.
//
// 64-bit mode also supports RIP relative addressing, but the encoder can't rewrite
// those to use a longer displacement. If any of the moved instructions have RIP
// relative addressing and it tries to access data too far away, the encoder will fail.
// The easiest solution is to use OS alloc functions that allocate memory close to the
// original code (+/-2GB).

/*
This method produces the following output:
Original code:
00007FFAC46ACDA4 mov [rsp+10h],rbx
00007FFAC46ACDA9 mov [rsp+18h],rsi
00007FFAC46ACDAE push rbp
00007FFAC46ACDAF push rdi
00007FFAC46ACDB0 push r14
00007FFAC46ACDB2 lea rbp,[rsp-100h]
00007FFAC46ACDBA sub rsp,200h
00007FFAC46ACDC1 mov rax,[rel 7FFAC47524E0h]
00007FFAC46ACDC8 xor rax,rsp
00007FFAC46ACDCB mov [rbp+0F0h],rax
00007FFAC46ACDD2 mov r8,[rel 7FFAC474F208h]
00007FFAC46ACDD9 lea rax,[rel 7FFAC46F4A58h]
00007FFAC46ACDE0 xor edi,edi

Original + patched code:
00007FFAC46ACDA4 mov rax,123456789ABCDEF0h
00007FFAC46ACDAE jmp rax
00007FFAC46ACDB0 push r14
00007FFAC46ACDB2 lea rbp,[rsp-100h]
00007FFAC46ACDBA sub rsp,200h
00007FFAC46ACDC1 mov rax,[rel 7FFAC47524E0h]
00007FFAC46ACDC8 xor rax,rsp
00007FFAC46ACDCB mov [rbp+0F0h],rax
00007FFAC46ACDD2 mov r8,[rel 7FFAC474F208h]
00007FFAC46ACDD9 lea rax,[rel 7FFAC46F4A58h]
00007FFAC46ACDE0 xor edi,edi

Moved code:
00007FFAC48ACDA4 mov [rsp+10h],rbx
00007FFAC48ACDA9 mov [rsp+18h],rsi
00007FFAC48ACDAE push rbp
00007FFAC48ACDAF push rdi
00007FFAC48ACDB0 jmp 00007FFAC46ACDB0h
*/
#[allow(dead_code)]
pub(crate) fn how_to_move_code() -> Result<(), IcedError> {
    let example_code = EXAMPLE_CODE.to_vec();
    println!("Original code:");
    disassemble(&example_code, EXAMPLE_CODE_RIP);

    let mut decoder = Decoder::with_ip(
        EXAMPLE_CODE_BITNESS,
        &example_code,
        EXAMPLE_CODE_RIP,
        DecoderOptions::NONE,
    );

    // In 64-bit mode, we need 12 bytes to jump to any address:
    //      mov rax,imm64   // 10
    //      jmp rax         // 2
    // We overwrite rax because it's probably not used by the called function.
    // In 32-bit mode, a normal JMP is just 5 bytes
    let required_bytes = 10 + 2;
    let mut total_bytes = 0;
    let mut orig_instructions: Vec<Instruction> = Vec::new();
    for instr in &mut decoder {
        orig_instructions.push(instr);
        total_bytes += instr.len() as u32;
        if instr.is_invalid() {
            panic!("Found garbage");
        }
        if total_bytes >= required_bytes {
            break;
        }

        match instr.flow_control() {
            FlowControl::Next => {}

            FlowControl::UnconditionalBranch => {
                if instr.op0_kind() == OpKind::NearBranch64 {
                    let _target = instr.near_branch_target();
                    // You could check if it's just jumping forward a few bytes and follow it
                    // but this is a simple example so we'll fail.
                }
                panic!("Not supported by this simple example");
            }

            FlowControl::IndirectBranch
            | FlowControl::ConditionalBranch
            | FlowControl::Return
            | FlowControl::Call
            | FlowControl::IndirectCall
            | FlowControl::Interrupt
            | FlowControl::XbeginXabortXend
            | FlowControl::Exception => panic!("Not supported by this simple example"),
        }
    }
    if total_bytes < required_bytes {
        panic!("Not enough bytes!");
    }
    assert!(!orig_instructions.is_empty());
    // Create a JMP instruction that branches to the original code, except those instructions
    // that we'll re-encode. We don't need to do it if it already ends in 'ret'
    let (jmp_back_addr, add) = {
        let last_instr = orig_instructions.last().unwrap();
        if last_instr.flow_control() != FlowControl::Return {
            (last_instr.next_ip(), true)
        } else {
            (last_instr.next_ip(), false)
        }
    };
    if add {
        orig_instructions.push(Instruction::with_branch(Code::Jmp_rel32_64, jmp_back_addr)?);
    }

    // Relocate the code to some new location. It can fix short/near branches and
    // convert them to short/near/long forms if needed. This also works even if it's a
    // jrcxz/loop/loopcc instruction which only have short forms.
    //
    // It can currently only fix RIP relative operands if the new location is within 2GB
    // of the target data location.
    //
    // Note that a block is not the same thing as a basic block. A block can contain any
    // number of instructions, including any number of branch instructions. One block
    // should be enough unless you must relocate different blocks to different locations.
    let relocated_base_address = EXAMPLE_CODE_RIP + 0x20_0000;
    let block = InstructionBlock::new(&orig_instructions, relocated_base_address);
    // This method can also encode more than one block but that's rarely needed, see above comment.
    let result = match BlockEncoder::encode(decoder.bitness(), block, BlockEncoderOptions::NONE) {
        Err(err) => panic!("{}", err),
        Ok(result) => result,
    };
    let new_code = result.code_buffer;

    // Patch the original code. Pretend that we use some OS API to write to memory...
    // We could use the BlockEncoder/Encoder for this but it's easy to do yourself too.
    // This is 'mov rax,imm64; jmp rax'
    const YOUR_FUNC: u64 = 0x1234_5678_9ABC_DEF0; // Address of your code
    let mut example_code = example_code.to_vec();
    example_code[0] = 0x48; // \ 'MOV RAX,imm64'
    example_code[1] = 0xB8; // /
    let mut v = YOUR_FUNC;
    for p in &mut example_code[2..10] {
        *p = v as u8;
        v >>= 8;
    }
    example_code[10] = 0xFF; // \ JMP RAX
    example_code[11] = 0xE0; // /

    // Disassemble it
    println!("Original + patched code:");
    disassemble(&example_code, EXAMPLE_CODE_RIP);

    // Disassemble the moved code
    println!("Moved code:");
    disassemble(&new_code, relocated_base_address);

	Ok(())
}

fn disassemble(data: &[u8], ip: u64) {
    let mut formatter = NasmFormatter::new();
    let mut output = String::new();
    let mut decoder = Decoder::with_ip(EXAMPLE_CODE_BITNESS, data, ip, DecoderOptions::NONE);
    for instruction in &mut decoder {
        output.clear();
        formatter.format(&instruction, &mut output);
        println!("{:016X} {}", instruction.ip(), output);
    }
    println!();
}

const EXAMPLE_CODE_BITNESS: u32 = 64;
const EXAMPLE_CODE_RIP: u64 = 0x0000_7FFA_C46A_CDA4;
static EXAMPLE_CODE: &[u8] = &[
    0x48, 0x89, 0x5C, 0x24, 0x10, 0x48, 0x89, 0x74, 0x24, 0x18, 0x55, 0x57, 0x41, 0x56, 0x48, 0x8D,
    0xAC, 0x24, 0x00, 0xFF, 0xFF, 0xFF, 0x48, 0x81, 0xEC, 0x00, 0x02, 0x00, 0x00, 0x48, 0x8B, 0x05,
    0x18, 0x57, 0x0A, 0x00, 0x48, 0x33, 0xC4, 0x48, 0x89, 0x85, 0xF0, 0x00, 0x00, 0x00, 0x4C, 0x8B,
    0x05, 0x2F, 0x24, 0x0A, 0x00, 0x48, 0x8D, 0x05, 0x78, 0x7C, 0x04, 0x00, 0x33, 0xFF,
];
```

## Get instruction info, eg. read/written regs/mem, control flow info, etc

Shows how to get used registers/memory and other info. It uses [`Instruction`] methods
and an [`InstructionInfoFactory`] to get this info.

[`Instruction`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.Instruction.html
[`InstructionInfoFactory`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.InstructionInfoFactory.html

```rust
use iced_x86::{
    ConditionCode, Decoder, DecoderOptions, Instruction, InstructionInfoFactory, OpKind, RflagsBits,
};

/*
This method produces the following output:
00007FFAC46ACDA4 mov [rsp+10h],rbx
    OpCode: o64 89 /r
    Instruction: MOV r/m64, r64
    Encoding: Legacy
    Mnemonic: Mov
    Code: Mov_rm64_r64
    CpuidFeature: X64
    FlowControl: Next
    Displacement offset = 4, size = 1
    Memory size: 8
    Op0Access: Write
    Op1Access: Read
    Op0: r64_or_mem
    Op1: r64_reg
    Used reg: RSP:Read
    Used reg: RBX:Read
    Used mem: [SS:RSP+0x10;UInt64;Write]
00007FFAC46ACDA9 mov [rsp+18h],rsi
    OpCode: o64 89 /r
    Instruction: MOV r/m64, r64
    Encoding: Legacy
    Mnemonic: Mov
    Code: Mov_rm64_r64
    CpuidFeature: X64
    FlowControl: Next
    Displacement offset = 4, size = 1
    Memory size: 8
    Op0Access: Write
    Op1Access: Read
    Op0: r64_or_mem
    Op1: r64_reg
    Used reg: RSP:Read
    Used reg: RSI:Read
    Used mem: [SS:RSP+0x18;UInt64;Write]
00007FFAC46ACDAE push rbp
    OpCode: o64 50+ro
    Instruction: PUSH r64
    Encoding: Legacy
    Mnemonic: Push
    Code: Push_r64
    CpuidFeature: X64
    FlowControl: Next
    SP Increment: -8
    Op0Access: Read
    Op0: r64_opcode
    Used reg: RBP:Read
    Used reg: RSP:ReadWrite
    Used mem: [SS:RSP+0xFFFFFFFFFFFFFFF8;UInt64;Write]
00007FFAC46ACDAF push rdi
    OpCode: o64 50+ro
    Instruction: PUSH r64
    Encoding: Legacy
    Mnemonic: Push
    Code: Push_r64
    CpuidFeature: X64
    FlowControl: Next
    SP Increment: -8
    Op0Access: Read
    Op0: r64_opcode
    Used reg: RDI:Read
    Used reg: RSP:ReadWrite
    Used mem: [SS:RSP+0xFFFFFFFFFFFFFFF8;UInt64;Write]
00007FFAC46ACDB0 push r14
    OpCode: o64 50+ro
    Instruction: PUSH r64
    Encoding: Legacy
    Mnemonic: Push
    Code: Push_r64
    CpuidFeature: X64
    FlowControl: Next
    SP Increment: -8
    Op0Access: Read
    Op0: r64_opcode
    Used reg: R14:Read
    Used reg: RSP:ReadWrite
    Used mem: [SS:RSP+0xFFFFFFFFFFFFFFF8;UInt64;Write]
00007FFAC46ACDB2 lea rbp,[rsp-100h]
    OpCode: o64 8D /r
    Instruction: LEA r64, m
    Encoding: Legacy
    Mnemonic: Lea
    Code: Lea_r64_m
    CpuidFeature: X64
    FlowControl: Next
    Displacement offset = 4, size = 4
    Op0Access: Write
    Op1Access: NoMemAccess
    Op0: r64_reg
    Op1: mem
    Used reg: RBP:Write
    Used reg: RSP:Read
00007FFAC46ACDBA sub rsp,200h
    OpCode: o64 81 /5 id
    Instruction: SUB r/m64, imm32
    Encoding: Legacy
    Mnemonic: Sub
    Code: Sub_rm64_imm32
    CpuidFeature: X64
    FlowControl: Next
    Immediate offset = 3, size = 4
    RFLAGS Written: OF, SF, ZF, AF, CF, PF
    RFLAGS Modified: OF, SF, ZF, AF, CF, PF
    Op0Access: ReadWrite
    Op1Access: Read
    Op0: r64_or_mem
    Op1: imm32sex64
    Used reg: RSP:ReadWrite
00007FFAC46ACDC1 mov rax,[7FFAC47524E0h]
    OpCode: o64 8B /r
    Instruction: MOV r64, r/m64
    Encoding: Legacy
    Mnemonic: Mov
    Code: Mov_r64_rm64
    CpuidFeature: X64
    FlowControl: Next
    Displacement offset = 3, size = 4
    Memory size: 8
    Op0Access: Write
    Op1Access: Read
    Op0: r64_reg
    Op1: r64_or_mem
    Used reg: RAX:Write
    Used mem: [DS:0x7FFAC47524E0;UInt64;Read]
00007FFAC46ACDC8 xor rax,rsp
    OpCode: o64 33 /r
    Instruction: XOR r64, r/m64
    Encoding: Legacy
    Mnemonic: Xor
    Code: Xor_r64_rm64
    CpuidFeature: X64
    FlowControl: Next
    RFLAGS Written: SF, ZF, PF
    RFLAGS Cleared: OF, CF
    RFLAGS Undefined: AF
    RFLAGS Modified: OF, SF, ZF, AF, CF, PF
    Op0Access: ReadWrite
    Op1Access: Read
    Op0: r64_reg
    Op1: r64_or_mem
    Used reg: RAX:ReadWrite
    Used reg: RSP:Read
00007FFAC46ACDCB mov [rbp+0F0h],rax
    OpCode: o64 89 /r
    Instruction: MOV r/m64, r64
    Encoding: Legacy
    Mnemonic: Mov
    Code: Mov_rm64_r64
    CpuidFeature: X64
    FlowControl: Next
    Displacement offset = 3, size = 4
    Memory size: 8
    Op0Access: Write
    Op1Access: Read
    Op0: r64_or_mem
    Op1: r64_reg
    Used reg: RBP:Read
    Used reg: RAX:Read
    Used mem: [SS:RBP+0xF0;UInt64;Write]
00007FFAC46ACDD2 mov r8,[7FFAC474F208h]
    OpCode: o64 8B /r
    Instruction: MOV r64, r/m64
    Encoding: Legacy
    Mnemonic: Mov
    Code: Mov_r64_rm64
    CpuidFeature: X64
    FlowControl: Next
    Displacement offset = 3, size = 4
    Memory size: 8
    Op0Access: Write
    Op1Access: Read
    Op0: r64_reg
    Op1: r64_or_mem
    Used reg: R8:Write
    Used mem: [DS:0x7FFAC474F208;UInt64;Read]
00007FFAC46ACDD9 lea rax,[7FFAC46F4A58h]
    OpCode: o64 8D /r
    Instruction: LEA r64, m
    Encoding: Legacy
    Mnemonic: Lea
    Code: Lea_r64_m
    CpuidFeature: X64
    FlowControl: Next
    Displacement offset = 3, size = 4
    Op0Access: Write
    Op1Access: NoMemAccess
    Op0: r64_reg
    Op1: mem
    Used reg: RAX:Write
00007FFAC46ACDE0 xor edi,edi
    OpCode: o32 33 /r
    Instruction: XOR r32, r/m32
    Encoding: Legacy
    Mnemonic: Xor
    Code: Xor_r32_rm32
    CpuidFeature: INTEL386
    FlowControl: Next
    RFLAGS Cleared: OF, SF, CF
    RFLAGS Set: ZF, PF
    RFLAGS Undefined: AF
    RFLAGS Modified: OF, SF, ZF, AF, CF, PF
    Op0Access: Write
    Op1Access: None
    Op0: r32_reg
    Op1: r32_or_mem
    Used reg: RDI:Write
*/
#[allow(dead_code)]
pub(crate) fn how_to_get_instruction_info() {
    let mut decoder = Decoder::with_ip(
        EXAMPLE_CODE_BITNESS,
        EXAMPLE_CODE,
        EXAMPLE_CODE_RIP,
        DecoderOptions::NONE,
    );

    // Use a factory to create the instruction info if you need register and
    // memory usage. If it's something else, eg. encoding, flags, etc, there
    // are Instruction methods that can be used instead.
    let mut info_factory = InstructionInfoFactory::new();
    let mut instr = Instruction::default();
    while decoder.can_decode() {
        decoder.decode_out(&mut instr);

        // Gets offsets in the instruction of the displacement and immediates and their sizes.
        // This can be useful if there are relocations in the binary. The encoder has a similar
        // method. This method must be called after decode() and you must pass in the last
        // instruction decode() returned.
        let offsets = decoder.get_constant_offsets(&instr);

        // For quick hacks, it's fine to use the Display trait to format an instruction,
        // but for real code, use a formatter, eg. MasmFormatter. See other examples.
        println!("{:016X} {}", instr.ip(), instr);

        let op_code = instr.op_code();
        let info = info_factory.info(&instr);
        let fpu_info = instr.fpu_stack_increment_info();
        println!("    OpCode: {}", op_code.op_code_string());
        println!("    Instruction: {}", op_code.instruction_string());
        println!("    Encoding: {:?}", instr.encoding());
        println!("    Mnemonic: {:?}", instr.mnemonic());
        println!("    Code: {:?}", instr.code());
        println!(
            "    CpuidFeature: {}",
            instr
                .cpuid_features()
                .iter()
                .map(|&a| format!("{:?}", a))
                .collect::<Vec<String>>()
                .join(" and ")
        );
        println!("    FlowControl: {:?}", instr.flow_control());
        if fpu_info.writes_top() {
            if fpu_info.increment() == 0 {
                println!("    FPU TOP: the instruction overwrites TOP");
            } else {
                println!("    FPU TOP inc: {}", fpu_info.increment());
            }
            println!(
                "    FPU TOP cond write: {}",
                if fpu_info.conditional() { "true" } else { "false" }
            );
        }
        if offsets.has_displacement() {
            println!(
                "    Displacement offset = {}, size = {}",
                offsets.displacement_offset(),
                offsets.displacement_size()
            );
        }
        if offsets.has_immediate() {
            println!(
                "    Immediate offset = {}, size = {}",
                offsets.immediate_offset(),
                offsets.immediate_size()
            );
        }
        if offsets.has_immediate2() {
            println!(
                "    Immediate #2 offset = {}, size = {}",
                offsets.immediate_offset2(),
                offsets.immediate_size2()
            );
        }
        if instr.is_stack_instruction() {
            println!("    SP Increment: {}", instr.stack_pointer_increment());
        }
        if instr.condition_code() != ConditionCode::None {
            println!("    Condition code: {:?}", instr.condition_code());
        }
        if instr.rflags_read() != RflagsBits::NONE {
            println!("    RFLAGS Read: {}", flags(instr.rflags_read()));
        }
        if instr.rflags_written() != RflagsBits::NONE {
            println!("    RFLAGS Written: {}", flags(instr.rflags_written()));
        }
        if instr.rflags_cleared() != RflagsBits::NONE {
            println!("    RFLAGS Cleared: {}", flags(instr.rflags_cleared()));
        }
        if instr.rflags_set() != RflagsBits::NONE {
            println!("    RFLAGS Set: {}", flags(instr.rflags_set()));
        }
        if instr.rflags_undefined() != RflagsBits::NONE {
            println!("    RFLAGS Undefined: {}", flags(instr.rflags_undefined()));
        }
        if instr.rflags_modified() != RflagsBits::NONE {
            println!("    RFLAGS Modified: {}", flags(instr.rflags_modified()));
        }
        if instr.op_kinds().any(|op_kind| op_kind == OpKind::Memory) {
            let size = instr.memory_size().size();
            if size != 0 {
                println!("    Memory size: {}", size);
            }
        }
        for i in 0..instr.op_count() {
            println!("    Op{}Access: {:?}", i, info.op_access(i));
        }
        for i in 0..op_code.op_count() {
            println!("    Op{}: {:?}", i, op_code.op_kind(i));
        }
        for reg_info in info.used_registers() {
            println!("    Used reg: {:?}", reg_info);
        }
        for mem_info in info.used_memory() {
            println!("    Used mem: {:?}", mem_info);
        }
    }
}

fn flags(rf: u32) -> String {
    fn append(sb: &mut String, s: &str) {
        if !sb.is_empty() {
            sb.push_str(", ");
        }
        sb.push_str(s);
    }

    let mut sb = String::new();
    if (rf & RflagsBits::OF) != 0 {
        append(&mut sb, "OF");
    }
    if (rf & RflagsBits::SF) != 0 {
        append(&mut sb, "SF");
    }
    if (rf & RflagsBits::ZF) != 0 {
        append(&mut sb, "ZF");
    }
    if (rf & RflagsBits::AF) != 0 {
        append(&mut sb, "AF");
    }
    if (rf & RflagsBits::CF) != 0 {
        append(&mut sb, "CF");
    }
    if (rf & RflagsBits::PF) != 0 {
        append(&mut sb, "PF");
    }
    if (rf & RflagsBits::DF) != 0 {
        append(&mut sb, "DF");
    }
    if (rf & RflagsBits::IF) != 0 {
        append(&mut sb, "IF");
    }
    if (rf & RflagsBits::AC) != 0 {
        append(&mut sb, "AC");
    }
    if (rf & RflagsBits::UIF) != 0 {
        append(&mut sb, "UIF");
    }
    if sb.is_empty() {
        sb.push_str("<empty>");
    }
    sb
}

const EXAMPLE_CODE_BITNESS: u32 = 64;
const EXAMPLE_CODE_RIP: u64 = 0x0000_7FFA_C46A_CDA4;
static EXAMPLE_CODE: &[u8] = &[
    0x48, 0x89, 0x5C, 0x24, 0x10, 0x48, 0x89, 0x74, 0x24, 0x18, 0x55, 0x57, 0x41, 0x56, 0x48, 0x8D,
    0xAC, 0x24, 0x00, 0xFF, 0xFF, 0xFF, 0x48, 0x81, 0xEC, 0x00, 0x02, 0x00, 0x00, 0x48, 0x8B, 0x05,
    0x18, 0x57, 0x0A, 0x00, 0x48, 0x33, 0xC4, 0x48, 0x89, 0x85, 0xF0, 0x00, 0x00, 0x00, 0x4C, 0x8B,
    0x05, 0x2F, 0x24, 0x0A, 0x00, 0x48, 0x8D, 0x05, 0x78, 0x7C, 0x04, 0x00, 0x33, 0xFF,
];
```

## Get the virtual address of a memory operand

```rust
use iced_x86::{Decoder, DecoderOptions, Register};

#[allow(dead_code)]
pub(crate) fn how_to_get_virtual_address() {
    // add [rdi+r12*8-5AA5EDCCh],esi
    let bytes = b"\x42\x01\xB4\xE7\x34\x12\x5A\xA5";
    let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);
    let instr = decoder.decode();

    let va = instr.virtual_address(0, 0, |register, _element_index, _element_size| {
        match register {
            // The base address of ES, CS, SS and DS is always 0 in 64-bit mode
            Register::ES | Register::CS | Register::SS | Register::DS => Some(0),
            Register::RDI => Some(0x0000_0000_1000_0000),
            Register::R12 => Some(0x0000_0004_0000_0000),
            _ => None,
        }
    });
    assert_eq!(va, Some(0x0000_001F_B55A_1234));
}
```

## Disassemble old/deprecated CPU instructions

```rust
use iced_x86::{Decoder, DecoderOptions, Formatter, Instruction, NasmFormatter};

/*
This method produces the following output:
731E0A03 bndmov bnd1, [eax]
731E0A07 mov tr3, esi
731E0A0A rdshr [eax]
731E0A0D dmint
731E0A0F svdc [eax], cs
731E0A12 cpu_read
731E0A14 pmvzb mm1, [eax]
731E0A17 frinear
731E0A19 altinst
*/
#[allow(dead_code)]
pub(crate) fn how_to_disassemble_old_instrs() {
    #[rustfmt::skip]
    let bytes = &[
        // bndmov bnd1,[eax]
        0x66, 0x0F, 0x1A, 0x08,
        // mov tr3,esi
        0x0F, 0x26, 0xDE,
        // rdshr [eax]
        0x0F, 0x36, 0x00,
        // dmint
        0x0F, 0x39,
        // svdc [eax],cs
        0x0F, 0x78, 0x08,
        // cpu_read
        0x0F, 0x3D,
        // pmvzb mm1,[eax]
        0x0F, 0x58, 0x08,
        // frinear
        0xDF, 0xFC,
        // altinst
        0x0F, 0x3F,
    ];

    // Enable decoding of Cyrix/Geode instructions, Centaur ALTINST, MOV to/from TR
    // and MPX instructions.
    // There are other options to enable other instructions such as UMOV, KNC, etc.
    // These are deprecated instructions or only used by old CPUs so they're not
    // enabled by default. Some newer instructions also use the same opcodes as
    // some of these old instructions.
    const DECODER_OPTIONS: u32 = DecoderOptions::MPX
        | DecoderOptions::MOV_TR
        | DecoderOptions::CYRIX
        | DecoderOptions::CYRIX_DMI
        | DecoderOptions::ALTINST;
    let mut decoder = Decoder::with_ip(32, bytes, 0x731E_0A03, DECODER_OPTIONS);

    let mut formatter = NasmFormatter::new();
    formatter.options_mut().set_space_after_operand_separator(true);
    let mut output = String::new();

    let mut instruction = Instruction::default();
    while decoder.can_decode() {
        decoder.decode_out(&mut instruction);

        output.clear();
        formatter.format(&instruction, &mut output);

        println!("{:08X} {}", instruction.ip(), &output);
    }
}
```

## Disassemble as fast as possible

For fastest possible disassembly you should set [`ENABLE_DB_DW_DD_DQ`] to `false`
and you should also override the unsafe [`verify_output_has_enough_bytes_left()`] and return `false`.

[`ENABLE_DB_DW_DD_DQ`]: https://docs.rs/iced-x86/trait.SpecializedFormatterTraitOptions.html#associatedconstant.ENABLE_DB_DW_DD_DQ
[`verify_output_has_enough_bytes_left()`]: https://docs.rs/iced-x86/trait.SpecializedFormatterTraitOptions.html#method.verify_output_has_enough_bytes_left

```rust
use iced_x86::{
    Decoder, DecoderOptions, Instruction, SpecializedFormatter, SpecializedFormatterTraitOptions,
};

#[allow(dead_code)]
pub(crate) fn how_to_disassemble_really_fast() {
    struct MyTraitOptions;
    impl SpecializedFormatterTraitOptions for MyTraitOptions {
        // If you never create a db/dw/dd/dq 'instruction', we don't need this feature.
        const ENABLE_DB_DW_DD_DQ: bool = false;
        // For a few percent faster code, you can also override `verify_output_has_enough_bytes_left()` and return `false`
        // unsafe fn verify_output_has_enough_bytes_left() -> bool {
        //     false
        // }
    }
    type MyFormatter = SpecializedFormatter<MyTraitOptions>;

    // Assume this is a big slice and not just one instruction
    let bytes = b"\x62\xF2\x4F\xDD\x72\x50\x01";
    let mut decoder = Decoder::new(64, bytes, DecoderOptions::NONE);

    let mut output = String::new();
    let mut instruction = Instruction::default();
    let mut formatter = MyFormatter::new();
    while decoder.can_decode() {
        decoder.decode_out(&mut instruction);
        output.clear();
        formatter.format(&instruction, &mut output);
        // do something with 'output' here, eg.:
        //     println!("{}", output);
    }
}
```

Also add this to your `Cargo.toml` file:

```toml
[profile.release]
codegen-units = 1
lto = true
opt-level = 3
```

## Create and encode instructions

NOTE: It's much easier to just use [`CodeAssembler`], see the example above.
This example shows how to create instructions without using it.

This example uses a [`BlockEncoder`] to encode created [`Instruction`]s.

[`BlockEncoder`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.BlockEncoder.html
[`CodeAssembler`]: https://docs.rs/iced-x86/1.21.0/iced_x86/code_asm/struct.CodeAssembler.html
[`Instruction`]: https://docs.rs/iced-x86/1.21.0/iced_x86/struct.Instruction.html

```rust
use iced_x86::{
    BlockEncoder, BlockEncoderOptions, Code, Decoder, DecoderOptions, Formatter, GasFormatter,
    IcedError, Instruction, InstructionBlock, MemoryOperand, Register,
};

#[allow(dead_code)]
pub(crate) fn how_to_encode_instructions() -> Result<(), IcedError> {
    let bitness = 64;

    // All created instructions get an IP of 0. The label id is just an IP.
    // The branch instruction's *target* IP should be equal to the IP of the
    // target instruction.
    let mut label_id: u64 = 1;
    let mut create_label = || {
        let id = label_id;
        label_id += 1;
        id
    };
    fn add_label(id: u64, mut instruction: Instruction) -> Instruction {
        instruction.set_ip(id);
        instruction
    }

    let label1 = create_label();

    let mut instructions = vec![
        Instruction::with1(Code::Push_r64, Register::RBP)?,
        Instruction::with1(Code::Push_r64, Register::RDI)?,
        Instruction::with1(Code::Push_r64, Register::RSI)?,
        Instruction::with2(Code::Sub_rm64_imm32, Register::RSP, 0x50)?,
        Instruction::with(Code::VEX_Vzeroupper),
        Instruction::with2(
            Code::Lea_r64_m,
            Register::RBP,
            MemoryOperand::with_base_displ(Register::RSP, 0x60),
        )?,
        Instruction::with2(Code::Mov_r64_rm64, Register::RSI, Register::RCX)?,
        Instruction::with2(
            Code::Lea_r64_m,
            Register::RDI,
            MemoryOperand::with_base_displ(Register::RBP, -0x38),
        )?,
        Instruction::with2(Code::Mov_r32_imm32, Register::ECX, 0x0A)?,
        Instruction::with2(Code::Xor_r32_rm32, Register::EAX, Register::EAX)?,
        Instruction::with_rep_stosd(bitness)?,
        Instruction::with2(Code::Cmp_rm64_imm32, Register::RSI, 0x1234_5678)?,
        // Create a branch instruction that references label1
        Instruction::with_branch(Code::Jne_rel32_64, label1)?,
        Instruction::with(Code::Nopd),
        // Add the instruction that is the target of the branch
        add_label(label1, Instruction::with2(Code::Xor_r32_rm32, Register::R15D, Register::R15D)?),
    ];

    // Create an instruction that accesses some data using an RIP relative memory operand
    let data1 = create_label();
    instructions.push(Instruction::with2(
        Code::Lea_r64_m,
        Register::R14,
        MemoryOperand::with_base_displ(Register::RIP, data1 as i64),
    )?);
    instructions.push(Instruction::with(Code::Nopd));
    let raw_data: &[u8] = &[0x12, 0x34, 0x56, 0x78];
    instructions.push(add_label(data1, Instruction::with_declare_byte(raw_data)?));

    // Use BlockEncoder to encode a block of instructions. This block can contain any
    // number of branches and any number of instructions. It does support encoding more
    // than one block but it's rarely needed.
    // It uses Encoder to encode all instructions.
    // If the target of a branch is too far away, it can fix it to use a longer branch.
    // This can be disabled by enabling some BlockEncoderOptions flags.
    let target_rip = 0x0000_1248_FC84_0000;
    let block = InstructionBlock::new(&instructions, target_rip);
    let result = match BlockEncoder::encode(bitness, block, BlockEncoderOptions::NONE) {
        Err(error) => panic!("Failed to encode it: {}", error),
        Ok(result) => result,
    };

    // Now disassemble the encoded instructions. Note that the 'jmp near'
    // instruction was turned into a 'jmp short' instruction because we
    // didn't disable branch optimizations.
    let bytes = result.code_buffer;
    let mut output = String::new();
    let bytes_code = &bytes[0..bytes.len() - raw_data.len()];
    let bytes_data = &bytes[bytes.len() - raw_data.len()..];
    let mut decoder = Decoder::with_ip(bitness, bytes_code, target_rip, DecoderOptions::NONE);
    let mut formatter = GasFormatter::new();
    formatter.options_mut().set_first_operand_char_index(8);
    for instruction in &mut decoder {
        output.clear();
        formatter.format(&instruction, &mut output);
        println!("{:016X} {}", instruction.ip(), output);
    }
    let db = Instruction::with_declare_byte(bytes_data)?;
    output.clear();
    formatter.format(&db, &mut output);
    println!("{:016X} {}", decoder.ip(), output);
    Ok(())
}
/*
Output:
00001248FC840000 push    %rbp
00001248FC840001 push    %rdi
00001248FC840002 push    %rsi
00001248FC840003 sub     $0x50,%rsp
00001248FC84000A vzeroupper
00001248FC84000D lea     0x60(%rsp),%rbp
00001248FC840012 mov     %rcx,%rsi
00001248FC840015 lea     -0x38(%rbp),%rdi
00001248FC840019 mov     $0xA,%ecx
00001248FC84001E xor     %eax,%eax
00001248FC840020 rep stos %eax,(%rdi)
00001248FC840022 cmp     $0x12345678,%rsi
00001248FC840029 jne     0x00001248FC84002C
00001248FC84002B nop
00001248FC84002C xor     %r15d,%r15d
00001248FC84002F lea     0x1248FC840037,%r14
00001248FC840036 nop
00001248FC840037 .byte   0x12,0x34,0x56,0x78
*/
```

## Minimum supported `rustc` version

iced-x86 supports `rustc` `1.63.0` or later.
This is checked in CI builds where the minimum supported version and the latest stable version are used to build the source code and run tests.

Bumping the minimum supported version of `rustc` is considered a minor breaking change. The minor version of iced-x86 will be incremented.
