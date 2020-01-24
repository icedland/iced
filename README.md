# Iced [![NuGet](https://img.shields.io/nuget/v/Iced.svg)](https://www.nuget.org/packages/Iced/) [![GitHub builds](https://github.com/0xd4d/iced/workflows/GitHub%20CI/badge.svg)](https://github.com/0xd4d/iced/actions) [![codecov](https://codecov.io/gh/0xd4d/iced/branch/master/graph/badge.svg)](https://codecov.io/gh/0xd4d/iced)


<img align="right" width="160px" height="160px" src="logo.png">

High performance x86 (16/32/64-bit) instruction decoder, disassembler and assembler.
It can be used for static analysis of x86/x64 binaries, to rewrite code (eg. remove garbage instructions), to relocate code or as a disassembler.

- Supports all Intel and AMD instructions
- High level [Assembler](#Assembler) providing a simple and lean syntax (e.g `asm.mov(eax, edx)`))
- [Decoding](#Decoding) and disassembler support:
  - The decoder doesn't allocate any memory and is 2x-5x+ faster than other similar libraries written in C or C#
  - Small decoded instructions, only 32 bytes
  - The formatter supports masm, nasm, gas (AT&T), Intel (XED) and there are many options to customize the output
- Encoding support:  
  - The encoder can be used to re-encode decoded instructions at any address
  - The block encoder encodes a list of instructions and optimizes branches to short, near or 'long' (64-bit: 1 or more instructions)
- API to get instruction info, eg. read/written registers, memory and rflags bits; CPUID feature flag, flow control info, etc
- All instructions are tested (decode, encode, format, instruction info)
- Supports of `.NET Standard 2.0/2.1+` and `.NET Framework 4.5+`

# Classes

See below for some examples. All classes are in the `Iced.Intel` namespace.

Decoder:

- `Decoder`
- `Instruction` (and `Instruction.Create()` methods)
- `CodeReader`
	- `ByteArrayCodeReader`
	- `StreamCodeReader`
- `InstructionList`
- `ConstantOffsets`
- `IcedFeatures.Initialize()`

Formatters:

- `Formatter`
	- `MasmFormatter`
	- `NasmFormatter`
	- `GasFormatter`
	- `IntelFormatter`
- `FormatterOptions`
	- `MasmFormatterOptions`
	- `NasmFormatterOptions`
	- `GasFormatterOptions`
	- `IntelFormatterOptions`
- `FormatterOutput`
	- `StringOutput`
- `ISymbolResolver`
- `IFormatterOptionsProvider`

Encoder:

- `Encoder`
- `BlockEncoder`
- `CodeWriter`
  - `StreamCodeWriter`
- `ConstantOffsets`
- `OpCodeInfo` (`Instruction.OpCode` and `Code.ToOpCode()`)

Instruction info:

- `InstructionInfo`
- `InstructionInfoFactory`
- `InstructionInfoExtensions`
- `MemorySizeExtensions`
- `RegisterExtensions`

High Level Assembler:

- `Assembler`
- `Label`
- `AssemblerRegisters` (use `using static` to have access directly to registers e.g `eax`, `rdi`, `xmm1`...)

# Examples

## Decoding

For another example, see [JitDasm](https://github.com/0xd4d/JitDasm).

```C#
using System;
using System.Collections.Generic;
using Iced.Intel;

namespace Iced.Examples {
    static class Program {
        const int HEXBYTES_COLUMN_BYTE_LENGTH = 10;

        static void Main(string[] args) {
            IcedFeatures.Initialize();
            DecoderFormatterExample();
            EncoderExample();
            CreateInstructionsExample();
            InstructionInfoExample();
        }

        const int exampleCodeBitness = 64;
        const ulong exampleCodeRIP = 0x00007FFAC46ACDA4;
        static readonly byte[] exampleCode = new byte[] {
            0x48, 0x89, 0x5C, 0x24, 0x10, 0x48, 0x89, 0x74, 0x24, 0x18, 0x55, 0x57, 0x41, 0x56, 0x48, 0x8D,
            0xAC, 0x24, 0x00, 0xFF, 0xFF, 0xFF, 0x48, 0x81, 0xEC, 0x00, 0x02, 0x00, 0x00, 0x48, 0x8B, 0x05,
            0x18, 0x57, 0x0A, 0x00, 0x48, 0x33, 0xC4, 0x48, 0x89, 0x85, 0xF0, 0x00, 0x00, 0x00, 0x4C, 0x8B,
            0x05, 0x2F, 0x24, 0x0A, 0x00, 0x48, 0x8D, 0x05, 0x78, 0x7C, 0x04, 0x00, 0x33, 0xFF
        };

        /*
         * This method produces the following output:
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
        static void DecoderFormatterExample() {
            // You can also pass in a hex string, eg. "90 91 929394", or you can use your own CodeReader
            // reading data from a file or memory etc
            var codeBytes = exampleCode;
            var codeReader = new ByteArrayCodeReader(codeBytes);
            var decoder = Decoder.Create(exampleCodeBitness, codeReader);
            decoder.IP = exampleCodeRIP;
            ulong endRip = decoder.IP + (uint)codeBytes.Length;

            // This list is faster than List<Instruction> since it uses refs to the Instructions
            // instead of copying them (each Instruction is 32 bytes in size). It has a ref indexer,
            // and a ref iterator. Add() uses 'in' (ref readonly).
            var instructions = new InstructionList();
            while (decoder.IP < endRip) {
                // The method allocates an uninitialized element at the end of the list and
                // returns a reference to it which is initialized by Decode().
                decoder.Decode(out instructions.AllocUninitializedElement());
            }

            // Formatters: Masm*, Nasm*, Gas* (AT&T) and Intel* (XED)
            var formatter = new NasmFormatter();
            formatter.Options.DigitSeparator = "`";
            formatter.Options.FirstOperandCharIndex = 10;
            var output = new StringOutput();
            // Use InstructionList's ref iterator (C# 7.3) to prevent copying 32 bytes every iteration
            foreach (ref var instr in instructions) {
                // Don't use instr.ToString(), it allocates more, uses masm syntax and default options
                formatter.Format(instr, output);
                Console.Write(instr.IP.ToString("X16"));
                Console.Write(" ");
                int instrLen = instr.Length;
                int byteBaseIndex = (int)(instr.IP - exampleCodeRIP);
                for (int i = 0; i < instrLen; i++)
                    Console.Write(codeBytes[byteBaseIndex + i].ToString("X2"));
                int missingBytes = HEXBYTES_COLUMN_BYTE_LENGTH - instrLen;
                for (int i = 0; i < missingBytes; i++)
                    Console.Write("  ");
                Console.Write(" ");
                Console.WriteLine(output.ToStringAndReset());
            }
        }

        /*
         * This method produces the following output:
New code bytes:
0x48, 0x89, 0x5C, 0x24, 0x10, 0x48, 0x89, 0x74, 0x24, 0x18, 0x55, 0x57, 0x41, 0x56, 0x48, 0x8D
0xAC, 0x24, 0x00, 0xFF, 0xFF, 0xFF, 0x48, 0x81, 0xEC, 0x00, 0x02, 0x00, 0x00, 0x48, 0x8B, 0x05
0x18, 0x57, 0xEA, 0xFF, 0x48, 0x33, 0xC4, 0x48, 0x89, 0x85, 0xF0, 0x00, 0x00, 0x00, 0x4C, 0x8B
0x05, 0x2F, 0x24, 0xEA, 0xFF, 0x48, 0x8D, 0x05, 0x78, 0x7C, 0xE4, 0xFF, 0x33, 0xFF
Disassembled code:
00007FFAC48ACDA4 mov       [rsp+10h],rbx
00007FFAC48ACDA9 mov       [rsp+18h],rsi
00007FFAC48ACDAE push      rbp
00007FFAC48ACDAF push      rdi
00007FFAC48ACDB0 push      r14
00007FFAC48ACDB2 lea       rbp,[rsp-100h]
00007FFAC48ACDBA sub       rsp,200h
00007FFAC48ACDC1 mov       rax,[rel 7FFA`C475`24E0h]
00007FFAC48ACDC8 xor       rax,rsp
00007FFAC48ACDCB mov       [rbp+0F0h],rax
00007FFAC48ACDD2 mov       r8,[rel 7FFA`C474`F208h]
00007FFAC48ACDD9 lea       rax,[rel 7FFA`C46F`4A58h]
00007FFAC48ACDE0 xor       edi,edi
         */
        static void EncoderExample() {
            var codeReader = new ByteArrayCodeReader(exampleCode);
            var decoder = Decoder.Create(exampleCodeBitness, codeReader);
            decoder.IP = exampleCodeRIP;
            ulong endRip = decoder.IP + (uint)exampleCode.Length;

            var instructions = new InstructionList();
            while (decoder.IP < endRip)
                decoder.Decode(out instructions.AllocUninitializedElement());

            // Relocate the code to some new location. It can fix short/near branches and
            // convert them to short/near/long forms if needed. This also works even if it's a
            // jrcxz/loop/loopcc instruction which only has a short form.
            //
            // It can currently only fix RIP relative operands if the new location is within 2GB
            // of the target data location.
            //
            // There's also a simpler Encoder class which is used by BlockEncoder, but it can only
            // encode one instruction at a time and doesn't fix branches.
            //
            // Note that a block is not the same thing as a basic block. A block can contain any
            // number of instructions, including any number of branch instructions. One block
            // should be enough unless you must relocate different blocks to different locations.
            var codeWriter = new CodeWriterImpl();
            ulong relocatedBaseAddress = exampleCodeRIP + 0x200000;
            var block = new InstructionBlock(codeWriter, instructions, relocatedBaseAddress);
            // This method can also encode more than one block but that's rarely needed, see above comment.
            bool success = BlockEncoder.TryEncode(decoder.Bitness, block, out var errorMessage, out _);
            if (!success) {
                Console.WriteLine($"ERROR: {errorMessage}");
                return;
            }
            var newCode = codeWriter.ToArray();
            Console.WriteLine("New code bytes:");
            for (int i = 0; i < newCode.Length;) {
                for (int j = 0; j < 16 && i < newCode.Length; i++, j++) {
                    if (j != 0)
                        Console.Write(", ");
                    Console.Write("0x");
                    Console.Write(newCode[i].ToString("X2"));
                }
                Console.WriteLine();
            }

            // Disassemble the new relocated code. It's identical to the original code except that
            // the RIP relative instructions have been updated.
            Console.WriteLine("Disassembled code:");
            var formatter = new NasmFormatter();
            formatter.Options.DigitSeparator = "`";
            formatter.Options.FirstOperandCharIndex = 10;
            var output = new StringOutput();
            var newDecoder = Decoder.Create(decoder.Bitness, new ByteArrayCodeReader(newCode));
            newDecoder.IP = block.RIP;
            endRip = newDecoder.IP + (uint)newCode.Length;
            while (newDecoder.IP < endRip) {
                newDecoder.Decode(out var instr);
                formatter.Format(instr, output);
                Console.WriteLine($"{instr.IP:X16} {output.ToStringAndReset()}");
            }
        }
        // Simple and inefficient code writer that stores the data in a List<byte>, with a ToArray() method
        // to get the data
        sealed class CodeWriterImpl : CodeWriter {
            readonly List<byte> allBytes = new List<byte>();
            public override void WriteByte(byte value) => allBytes.Add(value);
            public byte[] ToArray() => allBytes.ToArray();
        }

        /*
         * This method produces the following output:
Disassembled code:
00007FFAC48ACDA4 push      rbp
00007FFAC48ACDA5 push      rdi
00007FFAC48ACDA6 push      rsi
00007FFAC48ACDA7 sub       rsp,50h
00007FFAC48ACDAE vzeroupper
00007FFAC48ACDB1 lea       rbp,[rsp+60h]
00007FFAC48ACDB6 mov       rsi,rcx
00007FFAC48ACDB9 lea       rdi,[rbp-38h]
00007FFAC48ACDBD mov       ecx,0Ah
00007FFAC48ACDC2 xor       eax,eax
00007FFAC48ACDC4 rep stosd
00007FFAC48ACDC6 mov       rcx,rsi
00007FFAC48ACDC9 mov       [rbp+10h],rcx
00007FFAC48ACDCD mov       [rbp+18h],rdx
         */
        static void CreateInstructionsExample() {
            const int bitness = 64;

            var instructions = new InstructionList();
            // push    rbp
            instructions.Add(Instruction.Create(Code.Push_r64, Register.RBP));
            // push    rdi
            instructions.Add(Instruction.Create(Code.Push_r64, Register.RDI));
            // push    rsi
            instructions.Add(Instruction.Create(Code.Push_r64, Register.RSI));
            // sub     rsp,50h
            instructions.Add(Instruction.Create(Code.Sub_rm64_imm32, Register.RSP, 0x50));
            // vzeroupper
            instructions.Add(Instruction.Create(Code.VEX_Vzeroupper));
            // lea     rbp,[rsp+60h]
            instructions.Add(Instruction.Create(Code.Lea_r64_m, Register.RBP, new MemoryOperand(Register.RSP, 0x60)));
            // mov     rsi,rcx
            instructions.Add(Instruction.Create(Code.Mov_r64_rm64, Register.RSI, Register.RCX));
            // lea     rdi,[rbp-38h]
            instructions.Add(Instruction.Create(Code.Lea_r64_m, Register.RDI, new MemoryOperand(Register.RBP, -0x38)));
            // mov     ecx,0Ah
            instructions.Add(Instruction.Create(Code.Mov_r32_imm32, Register.ECX, 0x0A));
            // xor     eax,eax
            instructions.Add(Instruction.Create(Code.Xor_r32_rm32, Register.EAX, Register.EAX));
            // rep stosd
            instructions.Add(Instruction.CreateStosd(bitness, RepPrefixKind.Repe));
            // mov     rcx,rsi
            instructions.Add(Instruction.Create(Code.Mov_r64_rm64, Register.RCX, Register.RSI));
            // mov     [rbp+10h],rcx
            instructions.Add(Instruction.Create(Code.Mov_rm64_r64, new MemoryOperand(Register.RBP, 0x10), Register.RCX));
            // mov     [rbp+18h],rdx
            instructions.Add(Instruction.Create(Code.Mov_rm64_r64, new MemoryOperand(Register.RBP, 0x18), Register.RDX));

            var codeWriter = new CodeWriterImpl();
            ulong relocatedBaseAddress = exampleCodeRIP + 0x200000;
            var block = new InstructionBlock(codeWriter, instructions, relocatedBaseAddress);
            bool success = BlockEncoder.TryEncode(bitness, block, out var errorMessage, out _);
            if (!success) {
                Console.WriteLine($"ERROR: {errorMessage}");
                return;
            }

            var newCode = codeWriter.ToArray();
            Console.WriteLine("Disassembled code:");
            var formatter = new NasmFormatter();
            formatter.Options.DigitSeparator = "`";
            formatter.Options.FirstOperandCharIndex = 10;
            var output = new StringOutput();
            var newDecoder = Decoder.Create(bitness, new ByteArrayCodeReader(newCode));
            newDecoder.IP = block.RIP;
            ulong endRip = newDecoder.IP + (uint)newCode.Length;
            while (newDecoder.IP < endRip) {
                newDecoder.Decode(out var instr);
                formatter.Format(instr, output);
                Console.WriteLine($"{instr.IP:X16} {output.ToStringAndReset()}");
            }
        }

        /*
         * This method produces the following output:
00007FFAC46ACDA4 mov [rsp+10h],rbx
    OpCode: REX.W 89 /r
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
    RSP:Read
    RBX:Read
    [SS:RSP+0x10;UInt64;Write]
00007FFAC46ACDA9 mov [rsp+18h],rsi
    OpCode: REX.W 89 /r
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
    RSP:Read
    RSI:Read
    [SS:RSP+0x18;UInt64;Write]
00007FFAC46ACDAE push rbp
    OpCode: 50+ro
    Instruction: PUSH r64
    Encoding: Legacy
    Mnemonic: Push
    Code: Push_r64
    CpuidFeature: X64
    FlowControl: Next
    SP Increment: -8
    Op0Access: Read
    Op0: r64_opcode
    RBP:Read
    RSP:ReadWrite
    [SS:RSP+0xFFFFFFFFFFFFFFF8;UInt64;Write]
00007FFAC46ACDAF push rdi
    OpCode: 50+ro
    Instruction: PUSH r64
    Encoding: Legacy
    Mnemonic: Push
    Code: Push_r64
    CpuidFeature: X64
    FlowControl: Next
    SP Increment: -8
    Op0Access: Read
    Op0: r64_opcode
    RDI:Read
    RSP:ReadWrite
    [SS:RSP+0xFFFFFFFFFFFFFFF8;UInt64;Write]
00007FFAC46ACDB0 push r14
    OpCode: 50+ro
    Instruction: PUSH r64
    Encoding: Legacy
    Mnemonic: Push
    Code: Push_r64
    CpuidFeature: X64
    FlowControl: Next
    SP Increment: -8
    Op0Access: Read
    Op0: r64_opcode
    R14:Read
    RSP:ReadWrite
    [SS:RSP+0xFFFFFFFFFFFFFFF8;UInt64;Write]
00007FFAC46ACDB2 lea rbp,[rsp-100h]
    OpCode: REX.W 8D /r
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
    RBP:Write
    RSP:Read
00007FFAC46ACDBA sub rsp,200h
    OpCode: REX.W 81 /5 id
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
    RSP:ReadWrite
00007FFAC46ACDC1 mov rax,[7FFAC47524E0h]
    OpCode: REX.W 8B /r
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
    RAX:Write
    [DS:0x7FFAC47524E0;UInt64;Read]
00007FFAC46ACDC8 xor rax,rsp
    OpCode: REX.W 33 /r
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
    RAX:ReadWrite
    RSP:Read
00007FFAC46ACDCB mov [rbp+0F0h],rax
    OpCode: REX.W 89 /r
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
    RBP:Read
    RAX:Read
    [SS:RBP+0xF0;UInt64;Write]
00007FFAC46ACDD2 mov r8,[7FFAC474F208h]
    OpCode: REX.W 8B /r
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
    R8:Write
    [DS:0x7FFAC474F208;UInt64;Read]
00007FFAC46ACDD9 lea rax,[7FFAC46F4A58h]
    OpCode: REX.W 8D /r
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
    RAX:Write
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
    RDI:Write
         */
        static void InstructionInfoExample() {
            var codeReader = new ByteArrayCodeReader(exampleCode);
            var decoder = Decoder.Create(exampleCodeBitness, codeReader);
            decoder.IP = exampleCodeRIP;
            ulong endRip = decoder.IP + (uint)exampleCode.Length;

            // Use a factory to create the instruction info if you need register and
            // memory usage. If it's something else, eg. encoding, flags, etc, there
            // are properties on Instruction that can be used instead.
            var instrInfoFactory = new InstructionInfoFactory();
            while (decoder.IP < endRip) {
                decoder.Decode(out var instr);

                // Gets offsets in the instruction of the displacement and immediates and their sizes.
                // This can be useful if there are relocations in the binary. The encoder has a similar
                // method. This method must be called after Decode() and you must pass in the last
                // instruction Decode() returned.
                var offsets = decoder.GetConstantOffsets(instr);

                // A formatter is recommended since this ToString() method defaults to masm syntax,
                // uses default options, and allocates every single time it's called.
                var disasmStr = instr.ToString();
                Console.WriteLine($"{instr.IP:X16} {disasmStr}");

                var opCode = instr.OpCode;
                var info = instrInfoFactory.GetInfo(instr);
                const string tab = "    ";
                Console.WriteLine($"{tab}OpCode: {opCode.ToOpCodeString()}");
                Console.WriteLine($"{tab}Instruction: {opCode.ToInstructionString()}");
                Console.WriteLine($"{tab}Encoding: {instr.Encoding}");
                Console.WriteLine($"{tab}Mnemonic: {instr.Mnemonic}");
                Console.WriteLine($"{tab}Code: {instr.Code}");
                Console.WriteLine($"{tab}CpuidFeature: {string.Join(" and ", instr.CpuidFeatures)}");
                Console.WriteLine($"{tab}FlowControl: {instr.FlowControl}");
                if (offsets.HasDisplacement)
                    Console.WriteLine($"{tab}Displacement offset = {offsets.DisplacementOffset}, size = {offsets.DisplacementSize}");
                if (offsets.HasImmediate)
                    Console.WriteLine($"{tab}Immediate offset = {offsets.ImmediateOffset}, size = {offsets.ImmediateSize}");
                if (offsets.HasImmediate2)
                    Console.WriteLine($"{tab}Immediate #2 offset = {offsets.ImmediateOffset2}, size = {offsets.ImmediateSize2}");
                if (instr.IsStackInstruction)
                    Console.WriteLine($"{tab}SP Increment: {instr.StackPointerIncrement}");
                if (instr.ConditionCode != ConditionCode.None)
                    Console.WriteLine($"{tab}Condition code: {instr.ConditionCode}");
                if (instr.RflagsRead != RflagsBits.None)
                    Console.WriteLine($"{tab}RFLAGS Read: {instr.RflagsRead}");
                if (instr.RflagsWritten != RflagsBits.None)
                    Console.WriteLine($"{tab}RFLAGS Written: {instr.RflagsWritten}");
                if (instr.RflagsCleared != RflagsBits.None)
                    Console.WriteLine($"{tab}RFLAGS Cleared: {instr.RflagsCleared}");
                if (instr.RflagsSet != RflagsBits.None)
                    Console.WriteLine($"{tab}RFLAGS Set: {instr.RflagsSet}");
                if (instr.RflagsUndefined != RflagsBits.None)
                    Console.WriteLine($"{tab}RFLAGS Undefined: {instr.RflagsUndefined}");
                if (instr.RflagsModified != RflagsBits.None)
                    Console.WriteLine($"{tab}RFLAGS Modified: {instr.RflagsModified}");
                for (int i = 0; i < instr.OpCount; i++) {
                    var opKind = instr.GetOpKind(i);
                    if (opKind == OpKind.Memory || opKind == OpKind.Memory64) {
                        int size = instr.MemorySize.GetSize();
                        if (size != 0)
                            Console.WriteLine($"{tab}Memory size: {size}");
                        break;
                    }
                }
                for (int i = 0; i < instr.OpCount; i++)
                    Console.WriteLine($"{tab}Op{i}Access: {info.GetOpAccess(i)}");
                for (int i = 0; i < opCode.OpCount; i++)
                    Console.WriteLine($"{tab}Op{i}: {opCode.GetOpKind(i)}");
                // The returned iterator is a struct, nothing is allocated unless you box it
                foreach (var regInfo in info.GetUsedRegisters())
                    Console.WriteLine($"{tab}{regInfo.ToString()}");
                foreach (var memInfo in info.GetUsedMemory())
                    Console.WriteLine($"{tab}{memInfo.ToString()}");
            }
        }
    }
}
```

## Assembler

Iced provide a high level assembler by providing a friendly syntax close to what a regular assembler could provide:

```c#
using Iced.Intel;
// The following using static allows to import assembler registers
// e.g `eax` directly accessible as variables
using static Iced.Intel.AssemblerRegisters; 

public static class AssemblerPlay
{
	public static MemoryStream GenerateCode()
	{
		var c = new Assembler(64);

		c.push(r15);
		c.mov(r15, rsi);
		c.mov(rax, __[rdi]);
		c.add(rax, r15);
		c.pop(r15);
		c.ret();

		var stream = new MemoryStream();
		var writer = new StreamCodeWriter(stream)
		c.Assemble(writer);

		return stream;
	}
}
```

The assembler supports the entire instruction set available through the `Encoder` class.

- For prefixes (e.g `rep`, `xacquire`) they are directly accessible on the `Assembler`. For example: `c.rep.movsd()`.
- AVX512+ k1z/sae/rounding flags can be set directly on registers: `c.vaddpd(zmm1.k3.z, zmm2, zmm3.rz_sae)`
- AVX512+ broadcasting can be specified via e.g `__dword_bcst`: `c.vunpcklps(xmm2.k5.z, xmm6, __dword_bcst[rax])`

Labels can be created via `Assembler.CreateLabel`, placed just before an instruction via `Assembler.Label` and referenced by branch instructions:

```c#
using Iced.Intel;
// The following using static allows to import assembler registers
// e.g `eax` directly accessible as variables
using static Iced.Intel.AssemblerRegisters;

public static class AssemblerPlay
{
	public static MemoryStream GenerateCode()
	{
		var c = new Assembler(64);

		// Create a label
		var label1 = c.CreateLabel();

		c.push(r15);
		c.add(rax, r15);
		c.jne(label1); // Jump to Label1

		c.inc(rax);

		// Emit label1:
		c.Label(ref label1);
		c.pop(r15);
		c.ret();

		var stream = new MemoryStream();
		var writer = new StreamCodeWriter(stream)
		c.Assemble(writer);

		return stream;
	}
}
```

# License

MIT


# Icon

Logo `processor` by [Creative Stall](https://thenounproject.com/creativestall/) from the Noun Project
