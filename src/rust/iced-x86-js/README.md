iced-x86 JavaScript bindings (Rust -> WebAssembly) [![npm](https://img.shields.io/npm/v/iced-x86.svg)](https://www.npmjs.com/package/iced-x86) [![GitHub builds](https://github.com/icedland/iced/workflows/GitHub%20CI/badge.svg)](https://github.com/icedland/iced/actions) ![Minimum rustc version](https://img.shields.io/badge/rustc-1.63.0+-blue.svg) ![License](https://img.shields.io/crates/l/iced-x86.svg)

iced-x86 is a blazing fast and correct x86 (16/32/64-bit) disassembler for JavaScript (WebAssembly).

- 👍 Supports all Intel and AMD instructions
- 👍 Correct: All instructions are tested and iced has been tested against other disassemblers/assemblers (xed, gas, objdump, masm, dumpbin, nasm, ndisasm) and fuzzed
- 👍 The formatter supports masm, nasm, gas (AT&T), Intel (XED) and there are many options to customize the output
- 👍 The encoder can be used to re-encode decoded instructions at any address
- 👍 API to get instruction info, eg. read/written registers, memory and rflags bits; CPUID feature flag, control flow info, etc
- 👍 Rust + WebAssembly + JavaScript
- 👍 License: MIT

Rust crate: https://github.com/icedland/iced/blob/master/src/rust/iced-x86/README.md

## Building the code

Prerequisites:

- `Rust`: https://www.rust-lang.org/tools/install
- `wasm-pack`: https://rustwasm.github.io/wasm-pack/installer/
- Add wasm32 target: `rustup target add wasm32-unknown-unknown`

You can override which features to build to reduce the size of the wasm/ts/js files, see [Feature flags](#feature-flags).

This example assumes you need features `decoder` and `fast_fmt` and use a JavaScript bundler eg. webpack (change `bundler` to `nodejs` for Node.js support):

```sh
cd src/rust/iced-x86-js
wasm-pack build --mode force --target bundler -- --no-default-features --features "decoder fast_fmt"
```

`--target` docs [are here](https://rustwasm.github.io/docs/wasm-bindgen/reference/deployment.html) (`bundler`, `web`, `nodejs`, `no-modules`).

The result is stored in the `pkg/` sub dir. The js file isn't minified.

## Optimize wasm for speed

Edit `Cargo.toml` and change `opt-level = "z"` to [`opt-level = 3`](https://doc.rust-lang.org/cargo/reference/profiles.html#opt-level) (no double quotes) and update `wasm-opt` args to `-O4`.

```toml
[profile.release]
codegen-units = 1
lto = true
opt-level = 3

[package.metadata.wasm-pack.profile.release]
wasm-opt = ["-O4"]
```

## Testing the code

Prerequisites:

- Same as building it, see above👆
- `Node.js` == latest

This tests the JS API. The tests in `../iced-x86` test everything else.

```sh
cd src/rust/iced-x86-js
wasm-pack build --mode force --target nodejs
cd src/tests
npm install
npm test
```

## Feature flags

Here's a list of all features you can enable when building the wasm file

- `instr_api`: (👍 Enabled by default) Enables `Instruction` methods and properties to get eg. mnemonic, operands, etc.
- `decoder`: (👍 Enabled by default) Enables the decoder. Required to disassemble code.
- `encoder`: (👍 Enabled by default) Enables the encoder
- `block_encoder`: (👍 Enabled by default) Enables the `BlockEncoder`. Requires `encoder`
- `instr_create`: (👍 Enabled by default) Enables `Instruction.create*()` methods
- `op_code_info`: (👍 Enabled by default) Get instruction metadata, see the `Instruction.opCode` property. Requires `encoder`
- `instr_info`: (👍 Enabled by default) Enables instruction info code (read/written regs/mem, flags, control flow info etc)
- `gas`: (👍 Enabled by default) Enables the GNU Assembler (AT&T) formatter
- `intel`: (👍 Enabled by default) Enables the Intel (XED) formatter
- `masm`: (👍 Enabled by default) Enables the masm formatter
- `nasm`: (👍 Enabled by default) Enables the nasm formatter
- `fast_fmt`: (👍 Enabled by default) Enables `FastFormatter` (masm syntax) which uses less code (smaller wasm files)
- `no_vex`: Disables all `VEX` instructions. See below for more info.
- `no_evex`: Disables all `EVEX` instructions. See below for more info.
- `no_xop`: Disables all `XOP` instructions. See below for more info.
- `no_d3now`: Disables all `3DNow!` instructions. See below for more info.
- `mvex`: Enables `MVEX` instructions (Knights Corner). You must also pass in `DecoderOptions.KNC` to the `Decoder` constructor.

`"decoder fast_fmt"` is all you need to disassemble code (or replace `fast_fmt` with eg. `nasm` or `gas`).

`"decoder fast_fmt instr_api instr_info"` if you want to analyze the code and disassemble it. Add `encoder` and optionally `block_encoder` if you want to re-encode the decoded instructions.

## How-tos

- [Disassemble (decode and format instructions)](#disassemble-decode-and-format-instructions)
- [Create and encode instructions](#create-and-encode-instructions)
- [Move code in memory (eg. hook a function)](#move-code-in-memory-eg-hook-a-function)
- [Get instruction info, eg. read/written regs/mem, control flow info, etc](#get-instruction-info-eg-readwritten-regsmem-control-flow-info-etc)
- [Disassemble old/deprecated CPU instructions](#disassemble-olddeprecated-cpu-instructions)

## Disassemble (decode and format instructions)

This example uses a `Decoder` and one of the `Formatter`s to decode and format the code.

```js
// iced-x86 features needed: --features "decoder nasm"
const { Decoder, DecoderOptions, Formatter, FormatterSyntax } = require("iced-x86");

/*
This code produces the following output:
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

const exampleBitness = 64;
const exampleRip = 0x00007FFAC46ACDA4n;
const exampleCode = new Uint8Array([
    0x48, 0x89, 0x5C, 0x24, 0x10, 0x48, 0x89, 0x74, 0x24, 0x18, 0x55, 0x57, 0x41, 0x56, 0x48, 0x8D,
    0xAC, 0x24, 0x00, 0xFF, 0xFF, 0xFF, 0x48, 0x81, 0xEC, 0x00, 0x02, 0x00, 0x00, 0x48, 0x8B, 0x05,
    0x18, 0x57, 0x0A, 0x00, 0x48, 0x33, 0xC4, 0x48, 0x89, 0x85, 0xF0, 0x00, 0x00, 0x00, 0x4C, 0x8B,
    0x05, 0x2F, 0x24, 0x0A, 0x00, 0x48, 0x8D, 0x05, 0x78, 0x7C, 0x04, 0x00, 0x33, 0xFF
]);
const hexBytesColumnByteLength = 10;

const decoder = new Decoder(exampleBitness, exampleCode, DecoderOptions.None);
decoder.ip = exampleRip;
// This decodes all bytes. There's also `decode()` which decodes the next instruction,
// `decodeInstructions(count)` which decodes `count` instructions and `decodeOut(instruction)`
// which overwrites an existing instruction.
const instructions = decoder.decodeAll();

// Create a nasm formatter. It supports: Masm, Nasm, Gas (AT&T) and Intel (XED).
// There's also `FastFormatter` which uses less code (smaller wasm files).
//     const formatter = new FastFormatter();
const formatter = new Formatter(FormatterSyntax.Nasm);

// Change some options, there are many more
formatter.digitSeparator = "`";
formatter.firstOperandCharIndex = 10;

// Format the instructions
instructions.forEach(instruction => {
    const disasm = formatter.format(instruction);

    // Eg. "00007FFAC46ACDB2 488DAC2400FFFFFF     lea       rbp,[rsp-100h]"
    let line = ("000000000000000" + instruction.ip.toString(16)).substr(-16).toUpperCase();
    line += " ";
    const startIndex = Number(instruction.ip - exampleRip);
    exampleCode.slice(startIndex, startIndex + instruction.length).forEach(b => {
        line += ("0" + b.toString(16)).substr(-2).toUpperCase();
    });
    for (let i = instruction.length; i < hexBytesColumnByteLength; i++)
        line += "  ";
    line += " ";
    line += disasm;

    console.log(line);
});

// Free wasm memory
instructions.forEach(instruction => instruction.free());
formatter.free();
decoder.free();
```

## Create and encode instructions

This example uses a `BlockEncoder` to encode created `Instruction`s.

```js
// iced-x86 features needed: --features "decoder gas encoder instr_create block_encoder instr_api"
const {
    BlockEncoder, BlockEncoderOptions, Code, Decoder, DecoderOptions, Formatter, FormatterSyntax,
    Instruction, MemoryOperand, Register,
} = require("iced-x86");

const bitness = 64;

// All created instructions get an IP of 0. The label id is just an IP.
// The branch instruction's *target* IP should be equal to the IP of the
// target instruction.
let labelId = 1n;
function createLabel() {
    return labelId++;
}
function addLabel(id, instruction) {
    instruction.ip = id;
    return instruction;
}
function getAddress(addr) {
    return ("000000000000000" + addr.toString(16)).substr(-16).toUpperCase();
}

const label1 = createLabel();

const instructions = [];
instructions.push(Instruction.createReg(Code.Push_r64, Register.RBP));
instructions.push(Instruction.createReg(Code.Push_r64, Register.RDI));
instructions.push(Instruction.createReg(Code.Push_r64, Register.RSI));
instructions.push(Instruction.createRegU32(Code.Sub_rm64_imm32, Register.RSP, 0x50));
instructions.push(Instruction.create(Code.VEX_Vzeroupper));
instructions.push(Instruction.createRegMem(Code.Lea_r64_m, Register.RBP, MemoryOperand.createBaseDispl(Register.RSP, 0x60n)));
instructions.push(Instruction.createRegReg(Code.Mov_r64_rm64, Register.RSI, Register.RCX));
instructions.push(Instruction.createRegMem(Code.Lea_r64_m, Register.RDI, MemoryOperand.createBaseDispl(Register.RBP, -0x38n)));
instructions.push(Instruction.createRegI32(Code.Mov_r32_imm32, Register.ECX, 0x0A));
instructions.push(Instruction.createRegReg(Code.Xor_r32_rm32, Register.EAX, Register.EAX));
instructions.push(Instruction.createRepStosd(bitness));
instructions.push(Instruction.createRegU64(Code.Cmp_rm64_imm32, Register.RSI, 0x12345678n));
// Create a branch instruction that references label1
instructions.push(Instruction.createBranch(Code.Jne_rel32_64, label1));
instructions.push(Instruction.create(Code.Nopd));
// Add the instruction that is the target of the branch
instructions.push(addLabel(label1, Instruction.createRegReg(Code.Xor_r32_rm32, Register.R15D, Register.R15D)));

// Create an instruction that accesses some data using an RIP relative memory operand
const data1 = createLabel();
instructions.push(Instruction.createRegMem(Code.Lea_r64_m, Register.R14, MemoryOperand.createBaseDispl(Register.RIP, data1)));
instructions.push(Instruction.create(Code.Nopd));
const rawData = new Uint8Array([0x12, 0x34, 0x56, 0x78]);
instructions.push(addLabel(data1, Instruction.createDeclareByte(rawData)));

// Use BlockEncoder to encode a block of instructions. This block can contain any
// number of branches and any number of instructions.
// It uses Encoder to encode all instructions.
// If the target of a branch is too far away, it can fix it to use a longer branch.
// This can be disabled by enabling some BlockEncoderOptions flags.
const targetRip = 0x00001248FC840000n;
const blockEncoder = new BlockEncoder(bitness, BlockEncoderOptions.None);
instructions.forEach(instruction => blockEncoder.add(instruction));
const bytes = blockEncoder.encode(targetRip);

// Now disassemble the encoded instructions. Note that the 'jmp near'
// instruction was turned into a 'jmp short' instruction because we
// didn't disable branch optimizations.
const bytesCode = bytes.slice(0, bytes.length - rawData.length);
const bytesData = bytes.slice(bytes.length - rawData.length);
const decoder = new Decoder(bitness, bytesCode, DecoderOptions.None);
decoder.ip = targetRip;
const formatter = new Formatter(FormatterSyntax.Gas);
formatter.firstOperandCharIndex = 8;
const decodedInstructions = decoder.decodeAll();
decodedInstructions.forEach(instruction => {
    const disasm = formatter.format(instruction);
    console.log("%s %s", getAddress(instruction.ip), disasm);
});
const db = Instruction.createDeclareByte(bytesData);
const disasm = formatter.format(db);
console.log("%s %s", getAddress(decoder.ip), disasm);

// Free wasm memory
decodedInstructions.forEach(instruction => instruction.free());
instructions.forEach(instruction => instruction.free());
blockEncoder.free();
decoder.free();
formatter.free();
db.free();

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

## Move code in memory (eg. hook a function)

Uses instruction info API and the encoder to patch a function to jump to the programmer's function.

```js
// iced-x86 features needed: --features "decoder nasm instr_api encoder instr_create block_encoder instr_info"
const {
    BlockEncoder, BlockEncoderOptions, Code, Decoder, DecoderOptions, FlowControl, Formatter,
    FormatterSyntax, Instruction, OpKind
} = require("iced-x86");

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
This code produces the following output:
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

const exampleBitness = 64;
const exampleRip = 0x00007FFAC46ACDA4n;
const exampleCode = new Uint8Array([
    0x48, 0x89, 0x5C, 0x24, 0x10, 0x48, 0x89, 0x74, 0x24, 0x18, 0x55, 0x57, 0x41, 0x56, 0x48, 0x8D,
    0xAC, 0x24, 0x00, 0xFF, 0xFF, 0xFF, 0x48, 0x81, 0xEC, 0x00, 0x02, 0x00, 0x00, 0x48, 0x8B, 0x05,
    0x18, 0x57, 0x0A, 0x00, 0x48, 0x33, 0xC4, 0x48, 0x89, 0x85, 0xF0, 0x00, 0x00, 0x00, 0x4C, 0x8B,
    0x05, 0x2F, 0x24, 0x0A, 0x00, 0x48, 0x8D, 0x05, 0x78, 0x7C, 0x04, 0x00, 0x33, 0xFF
]);

function disassemble(code, rip) {
    const formatter = new Formatter(FormatterSyntax.Nasm);
    const decoder = new Decoder(exampleBitness, code, DecoderOptions.None);
    decoder.ip = rip;

    // decoder.decodeAll() can be used too but it's shown in another example. This shows
    // how to do it one instruction at a time.
    const instruction = new Instruction();
    while (decoder.canDecode) {
        // Decode the next instruction, overwriting an already created instruction
        decoder.decodeOut(instruction);
        const disasm = formatter.format(instruction);
        const address = ("000000000000000" + instruction.ip.toString(16)).substr(-16).toUpperCase();
        console.log("%s %s", address, disasm)
    }
    console.log();

    // Free wasm memory
    instruction.free();
    decoder.free();
    formatter.free();
}

console.log("Original code:");
disassemble(exampleCode, exampleRip);

const decoder = new Decoder(exampleBitness, exampleCode, DecoderOptions.None);
decoder.ip = exampleRip;

// In 64-bit mode, we need 12 bytes to jump to any address:
//      mov rax,imm64   // 10
//      jmp rax         // 2
// We overwrite rax because it's probably not used by the called function.
// In 32-bit mode, a normal JMP is just 5 bytes
const requiredBytes = 10 + 2;
let totalBytes = 0;
const origInstructions = [];
while (decoder.canDecode) {
    const instr = decoder.decode();
    origInstructions.push(instr);
    totalBytes += instr.length;
    if (instr.isInvalid)
        throw new Error("Found garbage");
    if (totalBytes >= requiredBytes)
        break;

    switch (instr.flowControl) {
        case FlowControl.Next:
            break;

        case FlowControl.UnconditionalBranch:
            if (instr.op0Kind === OpKind.NearBranch64) {
                const target = instr.nearBranchTarget;
                // You could check if it's just jumping forward a few bytes and follow it
                // but this is a simple example so we'll fail.
            }
            throw new Error("Not supported by this simple example");

        case FlowControl.IndirectBranch:
        case FlowControl.ConditionalBranch:
        case FlowControl.Return:
        case FlowControl.Call:
        case FlowControl.IndirectCall:
        case FlowControl.Interrupt:
        case FlowControl.XbeginXabortXend:
        case FlowControl.Exception:
        default:
            throw new Error("Not supported by this simple example");
    }
}
if (totalBytes < requiredBytes)
    throw new Error("Not enough bytes!");
const lastInstr = origInstructions[origInstructions.length - 1];
if (lastInstr.flowControl !== FlowControl.Return) {
    const jmp = Instruction.createBranch(Code.Jmp_rel32_64, lastInstr.nextIP);
    origInstructions.push(jmp);
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
const relocatedBaseAddress = exampleRip + 0x200000n;
const blockEncoder = new BlockEncoder(exampleBitness, BlockEncoderOptions.None);
origInstructions.forEach(instruction => blockEncoder.add(instruction));
const newCode = blockEncoder.encode(relocatedBaseAddress);

// Patch the original code. Pretend that we use some OS API to write to memory...
// We could use the BlockEncoder/Encoder for this but it's easy to do yourself too.
// This is 'mov rax,imm64; jmp rax'
const YOUR_FUNC = 0x123456789ABCDEF0n;// Address of your code
exampleCode[0] = 0x48;// \ 'MOV RAX,imm64'
exampleCode[1] = 0xB8;// /
let v = YOUR_FUNC;
for (let i = 0; i < 8; i++, v >>= 8n)
    exampleCode[2 + i] = Number(v & 0xFFn);
exampleCode[10] = 0xFF;// \ JMP RAX
exampleCode[11] = 0xE0;// /

// Disassemble it
console.log("Original + patched code:");
disassemble(exampleCode, exampleRip);

// Disassemble the moved code
console.log("Moved code:");
disassemble(newCode, relocatedBaseAddress);

// Free wasm memory
blockEncoder.free();
origInstructions.forEach(instruction => instruction.free());
decoder.free();
```

## Get instruction info, eg. read/written regs/mem, control flow info, etc

Shows how to get used registers/memory and other info. It uses `Instruction` methods
and an `InstructionInfoFactory` to get this info.

```js
// iced-x86 features needed: --features "decoder masm instr_api encoder op_code_info instr_info"
const {
    Code, ConditionCode, CpuidFeature, Decoder, DecoderOptions, EncodingKind, FlowControl,
    Instruction, InstructionInfoFactory, MemorySize, MemorySizeExt, Mnemonic, OpAccess,
    OpCodeOperandKind, OpKind, Register, RflagsBits,
} = require("iced-x86");

/*
This code produces the following output:
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

const exampleBitness = 64;
const exampleRip = 0x00007FFAC46ACDA4n;
const exampleCode = new Uint8Array([
    0x48, 0x89, 0x5C, 0x24, 0x10, 0x48, 0x89, 0x74, 0x24, 0x18, 0x55, 0x57, 0x41, 0x56, 0x48, 0x8D,
    0xAC, 0x24, 0x00, 0xFF, 0xFF, 0xFF, 0x48, 0x81, 0xEC, 0x00, 0x02, 0x00, 0x00, 0x48, 0x8B, 0x05,
    0x18, 0x57, 0x0A, 0x00, 0x48, 0x33, 0xC4, 0x48, 0x89, 0x85, 0xF0, 0x00, 0x00, 0x00, 0x4C, 0x8B,
    0x05, 0x2F, 0x24, 0x0A, 0x00, 0x48, 0x8D, 0x05, 0x78, 0x7C, 0x04, 0x00, 0x33, 0xFF
]);

const decoder = new Decoder(exampleBitness, exampleCode, DecoderOptions.None);
decoder.ip = exampleRip;

// Use a factory to create the instruction info if you need register and
// memory usage. If it's something else, eg. encoding, flags, etc, there
// are Instruction methods that can be used instead.
const infoFactory = new InstructionInfoFactory();
const instr = new Instruction();
while (decoder.canDecode) {
    // Decode the next instruction, overwriting `instr`
    decoder.decodeOut(instr);

    // Gets offsets in the instruction of the displacement and immediates and their sizes.
    // This can be useful if there are relocations in the binary. The encoder has a similar
    // method. This method must be called after decode() and you must pass in the last
    // instruction decode() returned.
    const offsets = decoder.getConstantOffsets(instr);

    // For quick hacks, it's fine to call toString() to format an instruction,
    // but for real code, use a formatter. See other examples.
    const address = ("000000000000000" + instr.ip.toString(16)).substr(-16).toUpperCase();
    console.log("%s %s", address, instr.toString());

    const opCode = instr.opCode;
    const info = infoFactory.info(instr);

    console.log("    OpCode: %s", opCode.opCodeString);
    console.log("    Instruction: %s", opCode.instructionString);
    console.log("    Encoding: %s", encodingKindToString(instr.encoding));
    console.log("    Mnemonic: %s", mnemonicToString(instr.mnemonic));
    console.log("    Code: %s", codeToString(instr.code));
    console.log("    CpuidFeature: %s", cpuidFeaturesToString(instr.cpuidFeatures()));
    console.log("    FlowControl: %s", flowControlToString(instr.flowControl));
    if (instr.fpuWritesTop) {
        if (instr.fpuTopIncrement == 0)
            console.log("    FPU TOP: the instruction overwrites TOP");
        else
            console.log("    FPU TOP inc: " + instr.fpuTopIncrement);
        console.log("    FPU TOP cond write: " + (instr.fpuCondWritesTop ? "true" : "false"));
    }
    if (offsets.hasDisplacement)
        console.log("    Displacement offset = %d, size = %d", offsets.displacementOffset, offsets.displacementSize);
    if (offsets.hasImmediate)
        console.log("    Immediate offset = %d, size = %d", offsets.immediateOffset, offsets.immediateSize);
    if (offsets.hasImmediate2)
        console.log("    Immediate #2 offset = %d, size = %d", offsets.immediateOffset2, offsets.immediateSize2);
    if (instr.isStackInstruction)
        console.log("    SP Increment: %d", instr.stackPointerIncrement);
    if (instr.conditionCode !== ConditionCode.None)
        console.log("    Condition code: %d // ConditionCode enum", instr.conditionCode);
    if (instr.rflagsRead !== RflagsBits.None)
        console.log("    RFLAGS Read: %s", rflagsBitsToString(instr.rflagsRead));
    if (instr.rflagsWritten !== RflagsBits.None)
        console.log("    RFLAGS Written: %s", rflagsBitsToString(instr.rflagsWritten));
    if (instr.rflagsCleared !== RflagsBits.None)
        console.log("    RFLAGS Cleared: %s", rflagsBitsToString(instr.rflagsCleared));
    if (instr.rflagsSet !== RflagsBits.None)
        console.log("    RFLAGS Set: %s", rflagsBitsToString(instr.rflagsSet));
    if (instr.rflagsUndefined !== RflagsBits.None)
        console.log("    RFLAGS Undefined: %s", rflagsBitsToString(instr.rflagsUndefined));
    if (instr.rflagsModified !== RflagsBits.None)
        console.log("    RFLAGS Modified: %s", rflagsBitsToString(instr.rflagsModified));
    for (let i = 0; i < instr.opCount; i++) {
        const opKind = instr.opKind(i);
        if (opKind === OpKind.Memory) {
            const size = MemorySizeExt.size(instr.memorySize);
            if (size !== 0)
                console.log("    Memory size: %d", size);
            break;
        }
    }
    for (let i = 0; i < instr.opCount; i++)
        console.log("    Op%dAccess: %s", i, opAccessToString(info.opAccess(i)));
    for (let i = 0; i < opCode.opCount; i++)
        console.log("    Op%d: %s", i, opCodeOperandKindToString(opCode.opKind(i)));
    for (const regInfo of info.usedRegisters()) {
        console.log("    Used reg: %s", usedRegisterToString(regInfo));
        // Free wasm memory
        regInfo.free();
    }
    for (const memInfo of info.usedMemory()) {
        console.log("    Used mem: %s", usedMemoryToString(memInfo))
        // Free wasm memory
        memInfo.free();
    }

    // Free wasm memory
    info.free();
    opCode.free();
    offsets.free();
}

// Free wasm memory
instr.free();
infoFactory.free();
decoder.free();

function rflagsBitsToString(value) {
    function append(s, f) {
        if (s.length !== 0) {
            s += ", ";
        }
        return s + f;
    }
    let sb = "";
    if ((value & RflagsBits.OF) !== 0) { sb = append(sb, "OF"); }
    if ((value & RflagsBits.SF) !== 0) { sb = append(sb, "SF"); }
    if ((value & RflagsBits.ZF) !== 0) { sb = append(sb, "ZF"); }
    if ((value & RflagsBits.AF) !== 0) { sb = append(sb, "AF"); }
    if ((value & RflagsBits.CF) !== 0) { sb = append(sb, "CF"); }
    if ((value & RflagsBits.PF) !== 0) { sb = append(sb, "PF"); }
    if ((value & RflagsBits.DF) !== 0) { sb = append(sb, "DF"); }
    if ((value & RflagsBits.IF) !== 0) { sb = append(sb, "IF"); }
    if ((value & RflagsBits.AC) !== 0) { sb = append(sb, "AC"); }
    if ((value & RflagsBits.C0) !== 0) { sb = append(sb, "C0"); }
    if ((value & RflagsBits.C1) !== 0) { sb = append(sb, "C1"); }
    if ((value & RflagsBits.C2) !== 0) { sb = append(sb, "C2"); }
    if ((value & RflagsBits.C3) !== 0) { sb = append(sb, "C3"); }
    if ((value & RflagsBits.UIF) !== 0) { sb = append(sb, "UIF"); }
    if (sb.length === 0)
        return "<empty>";
    return sb;
}

function registerToString(value) {
    switch (value) {
        case Register.EDI: return "EDI";
        case Register.RAX: return "RAX";
        case Register.RBX: return "RBX";
        case Register.RSP: return "RSP";
        case Register.RBP: return "RBP";
        case Register.RSI: return "RSI";
        case Register.RDI: return "RDI";
        case Register.R8: return "R8";
        case Register.R14: return "R14";
        case Register.SS: return "SS";
        case Register.DS: return "DS";
        default: return value + " /*Register enum*/";
    }
}

function opAccessToString(value) {
    switch (value) {
        case OpAccess.None: return "None";
        case OpAccess.Read: return "Read";
        case OpAccess.CondRead: return "CondRead";
        case OpAccess.Write: return "Write";
        case OpAccess.CondWrite: return "CondWrite";
        case OpAccess.ReadWrite: return "ReadWrite";
        case OpAccess.ReadCondWrite: return "ReadCondWrite";
        case OpAccess.NoMemAccess: return "NoMemAccess";
        default: return value + " /*OpAccess enum*/";
    }
}

function encodingKindToString(value) {
    switch (value) {
        case EncodingKind.Legacy: return "Legacy";
        case EncodingKind.VEX: return "VEX";
        case EncodingKind.EVEX: return "EVEX";
        case EncodingKind.XOP: return "XOP";
        case EncodingKind.D3NOW: return "D3NOW";
        case EncodingKind.MVEX: return "MVEX";
        default: return value + " /*EncodingKind enum*/";
    }
}

function mnemonicToString(value) {
    switch (value) {
        case Mnemonic.Lea: return "Lea";
        case Mnemonic.Mov: return "Mov";
        case Mnemonic.Push: return "Push";
        case Mnemonic.Sub: return "Sub";
        case Mnemonic.Xor: return "Xor";
        default: return value + " /*Mnemonic enum*/";
    }
}

function codeToString(value) {
    switch (value) {
        case Code.Lea_r64_m: return "Lea_r64_m";
        case Code.Mov_r64_rm64: return "Mov_r64_rm64";
        case Code.Mov_rm64_r64: return "Mov_rm64_r64";
        case Code.Push_r64: return "Push_r64";
        case Code.Sub_rm64_imm32: return "Sub_rm64_imm32";
        case Code.Xor_r32_rm32: return "Xor_r32_rm32";
        case Code.Xor_r64_rm64: return "Xor_r64_rm64";
        default: return value + " /*Code enum*/";
    }
}

function flowControlToString(value) {
    switch (value) {
        case FlowControl.Next: return "Next";
        default: return value + " /*FlowControl enum*/";
    }
}

function opCodeOperandKindToString(value) {
    switch (value) {
        case OpCodeOperandKind.imm32sex64: return "imm32sex64";
        case OpCodeOperandKind.mem: return "mem";
        case OpCodeOperandKind.r32_or_mem: return "r32_or_mem";
        case OpCodeOperandKind.r32_reg: return "r32_reg";
        case OpCodeOperandKind.r64_opcode: return "r64_opcode";
        case OpCodeOperandKind.r64_or_mem: return "r64_or_mem";
        case OpCodeOperandKind.r64_reg: return "r64_reg";
        default: return value + " /*OpCodeOperandKind enum*/";
    }
}

function cpuidFeatureToString(value) {
    switch (value) {
        case CpuidFeature.INTEL386: return "INTEL386";
        case CpuidFeature.X64: return "X64";
        default: return value + " /*CpuidFeature enum*/";
    }
}

function cpuidFeaturesToString(cpuidFeatures) {
    let sb = "";
    for (const cpuidFeature of cpuidFeatures) {
        if (sb.length !== 0)
            sb += " and ";
        sb += cpuidFeatureToString(cpuidFeature);
    }
    return sb;
}

function memorySizeToString(value) {
    switch (value) {
        case MemorySize.UInt64: return "UInt64";
        default: return value + " /*MemorySize enum*/";
    }
}

function usedRegisterToString(regInfo) {
    return registerToString(regInfo.register) + ":" + opAccessToString(regInfo.access);
}

function usedMemoryToString(memInfo) {
    let sb = "[" + registerToString(memInfo.segment) + ":";
    let needPlus = memInfo.base !== Register.None;
    if (needPlus)
        sb += registerToString(memInfo.base);
    if (memInfo.index !== Register.None) {
        if (needPlus)
            sb += "+";
        needPlus = true;
        sb += registerToString(memInfo.index);
        if (memInfo.scale !== 1)
            sb += "*" + memInfo.scale;
    }
    if (memInfo.displacement !== 0n || !needPlus) {
        if (needPlus)
            sb += "+";
        if (memInfo.displacement <= 9)
            sb += memInfo.displacement;
        else
            sb += "0x" + memInfo.displacement.toString(16).toUpperCase();
    }
    sb += ";" + memorySizeToString(memInfo.memorySize) + ";" + opAccessToString(memInfo.access) + "]";
    return sb;
}
```

## Disassemble old/deprecated CPU instructions

```js
// iced-x86 features needed: --features "decoder nasm"
const { Decoder, DecoderOptions, Formatter, FormatterSyntax } = require("iced-x86");

/*
This code produces the following output:
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

const bytes = new Uint8Array([
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
]);

// Enable decoding of Cyrix/Geode instructions, Centaur ALTINST, MOV to/from TR
// and MPX instructions.
// There are other options to enable other instructions such as UMOV, KNC, etc.
// These are deprecated instructions or only used by old CPUs so they're not
// enabled by default. Some newer instructions also use the same opcodes as
// some of these old instructions.
let decoderOptions = DecoderOptions.MPX | DecoderOptions.MovTr |
    DecoderOptions.Cyrix | DecoderOptions.Cyrix_DMI | DecoderOptions.ALTINST;
const decoder = new Decoder(32, bytes, decoderOptions);
decoder.ip = 0x731E0A03n;

const formatter = new Formatter(FormatterSyntax.Nasm);
formatter.spaceAfterOperandSeparator = true;

const instructions = decoder.decodeAll();
instructions.forEach(instruction => {
    const disasm = formatter.format(instruction);

    let line = ("0000000" + instruction.ip.toString(16)).substr(-8).toUpperCase();
    line += " ";
    line += disasm;

    console.log(line);
});

// Free wasm memory
instructions.forEach(instruction => instruction.free());
formatter.free();
decoder.free();
```
