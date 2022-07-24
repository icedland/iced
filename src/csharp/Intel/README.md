# iced [![NuGet](https://img.shields.io/nuget/v/iced.svg)](https://www.nuget.org/packages/iced/) [![GitHub builds](https://github.com/icedland/iced/workflows/GitHub%20CI/badge.svg)](https://github.com/icedland/iced/actions) [![codecov](https://codecov.io/gh/icedland/iced/branch/master/graph/badge.svg)](https://codecov.io/gh/icedland/iced)

<img align="right" width="160px" height="160px" src="../../../logo.png">

iced is a blazing fast and correct x86 (16/32/64-bit) instruction decoder, disassembler and assembler written in C#.

- 👍 Supports all Intel and AMD instructions
- 👍 Correct: All instructions are tested and iced has been tested against other disassemblers/assemblers (xed, gas, objdump, masm, dumpbin, nasm, ndisasm) and fuzzed
- 👍 100% C# code
- 👍 The formatter supports masm, nasm, gas (AT&T), Intel (XED) and there are many options to customize the output
- 👍 The decoder decodes >130 MB/s
- 👍 Small decoded instructions, only 40 bytes and the decoder doesn't allocate any memory
- 👍 Create instructions with [code assembler](#assemble-instructions), eg. `asm.mov(eax, edx)`
- 👍 The encoder can be used to re-encode decoded instructions at any address
- 👍 API to get instruction info, eg. read/written registers, memory and rflags bits; CPUID feature flag, control flow info, etc
- 👍 Supports `.NET Standard 2.0/2.1+` and `.NET Framework 4.5+`
- 👍 License: MIT

# Classes

See below for some examples. All classes are in the `Iced.Intel` namespace.

Decoder:

- `Decoder`
- `Instruction` (and `Instruction.Create()` methods)
- `CodeReader`
    - `ByteArrayCodeReader`
    - `StreamCodeReader`
- `ConstantOffsets`
- `IcedFeatures.Initialize()`

Formatters:

- `Formatter`
    - `MasmFormatter`
    - `NasmFormatter`
    - `GasFormatter`
    - `IntelFormatter`
    - `FastFormatter`
- `FormatterOptions`
- `FormatterOutput`
    - `StringOutput`
- `ISymbolResolver`
- `IFormatterOptionsProvider`

Assembler:

- `Assembler`
- `Label`
- `AssemblerRegisters` (use `using static` to have access directly to registers e.g `eax`, `rdi`, `xmm1`...)

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

# How-tos

- [Disassemble (decode and format instructions)](#disassemble-decode-and-format-instructions)
- [Assemble instructions](#assemble-instructions)
- [Disassemble with a symbol resolver](#disassemble-with-a-symbol-resolver)
- [Disassemble with colorized text](#disassemble-with-colorized-text)
- [Move code in memory (eg. hook a function)](#move-code-in-memory-eg-hook-a-function)
- [Get instruction info, eg. read/written regs/mem, control flow info, etc](#get-instruction-info-eg-readwritten-regsmem-control-flow-info-etc)
- [Get the virtual address of a memory operand](#get-the-virtual-address-of-a-memory-operand)
- [Disassemble old/deprecated CPU instructions](#disassemble-olddeprecated-cpu-instructions)

## Disassemble (decode and format instructions)

```cs
using System;
using System.Collections.Generic;
using Iced.Intel;

static class HowTo_Disassemble {
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
    public static void Example() {
        // You can also pass in a hex string, eg. "90 91 929394", or you can use your own CodeReader
        // reading data from a file or memory etc
        var codeBytes = exampleCode;
        var codeReader = new ByteArrayCodeReader(codeBytes);
        var decoder = Decoder.Create(exampleCodeBitness, codeReader);
        decoder.IP = exampleCodeRIP;
        ulong endRip = decoder.IP + (uint)codeBytes.Length;

        var instructions = new List<Instruction>();
        while (decoder.IP < endRip)
            instructions.Add(decoder.Decode());

        // Formatters: Masm*, Nasm*, Gas* (AT&T) and Intel* (XED).
        // There's also `FastFormatter` which is ~2x faster. Use it if formatting speed is more
        // important than being able to re-assemble formatted instructions.
        var formatter = new NasmFormatter();
        formatter.Options.DigitSeparator = "`";
        formatter.Options.FirstOperandCharIndex = 10;
        var output = new StringOutput();
        foreach (var instr in instructions) {
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

    const int HEXBYTES_COLUMN_BYTE_LENGTH = 10;
    const int exampleCodeBitness = 64;
    const ulong exampleCodeRIP = 0x00007FFAC46ACDA4;
    static readonly byte[] exampleCode = new byte[] {
        0x48, 0x89, 0x5C, 0x24, 0x10, 0x48, 0x89, 0x74, 0x24, 0x18, 0x55, 0x57, 0x41, 0x56, 0x48, 0x8D,
        0xAC, 0x24, 0x00, 0xFF, 0xFF, 0xFF, 0x48, 0x81, 0xEC, 0x00, 0x02, 0x00, 0x00, 0x48, 0x8B, 0x05,
        0x18, 0x57, 0x0A, 0x00, 0x48, 0x33, 0xC4, 0x48, 0x89, 0x85, 0xF0, 0x00, 0x00, 0x00, 0x4C, 0x8B,
        0x05, 0x2F, 0x24, 0x0A, 0x00, 0x48, 0x8D, 0x05, 0x78, 0x7C, 0x04, 0x00, 0x33, 0xFF
    };
}
```

## Assemble instructions

```cs
using System;
using System.IO;
using Iced.Intel;
using static Iced.Intel.AssemblerRegisters;

static class HowTo_Assemble {
    /*
     * This method produces the following output:
1234567810000000 = push r15
1234567810000002 = add rax,r15
1234567810000005 = mov rax,[rax]
1234567810000008 = mov rax,[rax]
123456781000000B = cmp dword ptr [rax+rcx*8+10h],0FFFFFFFFh
1234567810000010 = jne short 123456781000003Dh
1234567810000012 = inc rax
1234567810000015 = lea rcx,[1234567810000040h]
123456781000001C = rep stosd
123456781000001E = xacquire lock add qword ptr [rax+rcx],7Bh
1234567810000025 = vaddpd zmm1{k3}{z},zmm2,zmm3{rz-sae}
123456781000002B = vunpcklps xmm2{k5}{z},xmm6,dword bcst [rax]
1234567810000031 = inc rax
1234567810000034 = je short 1234567810000031h
1234567810000036 = inc rcx
1234567810000039 = je short 123456781000003Ch
123456781000003B = nop
123456781000003C = nop
123456781000003D = pop r15
123456781000003F = ret
1234567810000040 = pause
     */
    public static MemoryStream Example() {
        // The assembler supports all modes: 16-bit, 32-bit and 64-bit.
        var c = new Assembler(64);

        var label1 = c.CreateLabel();
        var data1 = c.CreateLabel();

        c.push(r15);
        c.add(rax, r15);

        // If the memory operand can only have one size, __[] can be used. The assembler ignores
        // the memory size unless it's an ambiguous instruction, eg. 'add [mem],123'
        c.mov(rax, __[rax]);
        c.mov(rax, __qword_ptr[rax]);

        // The assembler must know the memory size to pick the correct instruction
        c.cmp(__dword_ptr[rax + rcx * 8 + 0x10], -1);
        c.jne(label1); // Jump to Label1

        c.inc(rax);

        // Labels can be referenced by memory operands (64-bit only) and call/jmp/jcc/loopcc instructions
        c.lea(rcx, __[data1]);

        // The assembler has prefix properties that will be added to the following instruction
        c.rep.stosd();
        c.xacquire.@lock.add(__qword_ptr[rax + rcx], 123);

        // The assembler defaults to VEX instructions. If you need EVEX instructions, set PreferVex=false
        c.PreferVex = false;
        // or call `c.vex` or `c.evex` prefixes to override the default encoding.
        // AVX-512 decorators are properties on the memory and register operands
        c.vaddpd(zmm1.k3.z, zmm2, zmm3.rz_sae);
        // To broadcast memory, use the __dword_bcst/__qword_bcst memory types
        c.vunpcklps(xmm2.k5.z, xmm6, __dword_bcst[rax]);

        // You can create anonymous labels, just like in eg. masm, @@, @F and @B
        c.AnonymousLabel(); // same as @@: in masm
        c.inc(rax);
        c.je(c.@B); // reference the previous anonymous label
        c.inc(rcx);
        c.je(c.@F); // reference the next anonymous label
        c.nop();
        c.AnonymousLabel();
        c.nop();

        // Emit label1:
        c.Label(ref label1);
        // If needed, a zero-bytes instruction can be used as a label but this is optional
        c.zero_bytes();
        c.pop(r15);
        c.ret();
        c.Label(ref data1);
        c.db(0xF3, 0x90); // pause

        const ulong RIP = 0x1234_5678_1000_0000;
        var stream = new MemoryStream();
        c.Assemble(new StreamCodeWriter(stream), RIP);

        // Disassemble the result
        stream.Position = 0;
        var reader = new StreamCodeReader(stream);
        var decoder = Decoder.Create(64, reader);
        decoder.IP = RIP;
        while (stream.Position < stream.Length) {
            decoder.Decode(out var instr);
            Console.WriteLine($"{instr.IP:X} = {instr}");
        }

        return stream;
    }
}
```

## Disassemble with a symbol resolver

```cs
using System;
using System.Collections.Generic;
using Iced.Intel;

static class HowTo_SymbolResolver {
    sealed class SymbolResolver : ISymbolResolver {
        readonly Dictionary<ulong, string> symbolDict;

        public SymbolResolver(Dictionary<ulong, string> symbolDict) {
            this.symbolDict = symbolDict;
        }

        public bool TryGetSymbol(in Instruction instruction, int operand, int instructionOperand,
            ulong address, int addressSize, out SymbolResult symbol) {
            if (symbolDict.TryGetValue(address, out var symbolText)) {
                // The 'address' arg is the address of the symbol and doesn't have to be identical
                // to the 'address' arg passed to TryGetSymbol(). If it's different from the input
                // address, the formatter will add +N or -N, eg. '[rax+symbol+123]'
                symbol = new SymbolResult(address, symbolText);
                return true;
            }
            symbol = default;
            return false;
        }
    }

    public static void Example() {
        var symbols = new Dictionary<ulong, string> {
            { 0x5AA55AA5UL, "my_data" },
        };
        var symbolResolver = new SymbolResolver(symbols);
        var decoder = Decoder.Create(64, new ByteArrayCodeReader("488B8AA55AA55A"));
        decoder.Decode(out var instr);

        var formatter = new GasFormatter(symbolResolver);
        var output = new StringOutput();
        formatter.Format(instr, output);
        // Prints: mov my_data(%rdx),%rcx
        Console.WriteLine(output.ToStringAndReset());
    }
}
```

## Disassemble with colorized text

```cs
using System;
using System.Collections.Generic;
using Iced.Intel;

static class HowTo_ColorizedText {
    public static void Example() {
        var codeReader = new ByteArrayCodeReader(exampleCode);
        var decoder = Decoder.Create(exampleCodeBitness, codeReader);
        decoder.IP = exampleCodeRIP;

        var formatter = new MasmFormatter();
        var output = new FormatterOutputImpl();
        foreach (var instr in decoder) {
            output.List.Clear();
            formatter.Format(instr, output);
            foreach (var (text, kind) in output.List) {
                Console.ForegroundColor = GetColor(kind);
                Console.Write(text);
            }
            Console.WriteLine();
        }
        Console.ResetColor();
    }

    sealed class FormatterOutputImpl : FormatterOutput {
        public readonly List<(string text, FormatterTextKind kind)> List =
            new List<(string text, FormatterTextKind kind)>();
        public override void Write(string text, FormatterTextKind kind) => List.Add((text, kind));
    }

    static ConsoleColor GetColor(FormatterTextKind kind) {
        switch (kind) {
        case FormatterTextKind.Directive:
        case FormatterTextKind.Keyword:
            return ConsoleColor.Yellow;

        case FormatterTextKind.Prefix:
        case FormatterTextKind.Mnemonic:
            return ConsoleColor.Red;

        case FormatterTextKind.Register:
            return ConsoleColor.Magenta;

        case FormatterTextKind.Number:
            return ConsoleColor.Green;

        default:
            return ConsoleColor.White;
        }
    }

    const int exampleCodeBitness = 64;
    const ulong exampleCodeRIP = 0x00007FFAC46ACDA4;
    static readonly byte[] exampleCode = new byte[] {
        0x48, 0x89, 0x5C, 0x24, 0x10, 0x48, 0x89, 0x74, 0x24, 0x18, 0x55, 0x57, 0x41, 0x56, 0x48, 0x8D,
        0xAC, 0x24, 0x00, 0xFF, 0xFF, 0xFF, 0x48, 0x81, 0xEC, 0x00, 0x02, 0x00, 0x00, 0x48, 0x8B, 0x05,
        0x18, 0x57, 0x0A, 0x00, 0x48, 0x33, 0xC4, 0x48, 0x89, 0x85, 0xF0, 0x00, 0x00, 0x00, 0x4C, 0x8B,
        0x05, 0x2F, 0x24, 0x0A, 0x00, 0x48, 0x8D, 0x05, 0x78, 0x7C, 0x04, 0x00, 0x33, 0xFF
    };
}
```

## Move code in memory (eg. hook a function)

```cs
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Iced.Intel;

static class HowTo_MoveCode {
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
     * This method produces the following output:
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
    public static void Example() {
        Console.WriteLine("Original code:");
        Disassemble(exampleCode, exampleCodeRIP);

        var codeReader = new ByteArrayCodeReader(exampleCode);
        var decoder = Decoder.Create(exampleCodeBitness, codeReader);
        decoder.IP = exampleCodeRIP;

        // In 64-bit mode, we need 12 bytes to jump to any address:
        //      mov rax,imm64   // 10
        //      jmp rax         // 2
        // We overwrite rax because it's probably not used by the called function.
        // In 32-bit mode, a normal JMP is just 5 bytes
        const uint requiredBytes = 10 + 2;
        uint totalBytes = 0;
        var origInstructions = new List<Instruction>();
        while (codeReader.CanReadByte) {
            decoder.Decode(out var instr);
            origInstructions.Add(instr);
            totalBytes += (uint)instr.Length;
            if (instr.IsInvalid)
                throw new Exception("Found garbage");
            if (totalBytes >= requiredBytes)
                break;

            switch (instr.FlowControl) {
            case FlowControl.Next:
                break;

            case FlowControl.UnconditionalBranch:
                if (instr.Op0Kind == OpKind.NearBranch64) {
                    var target = instr.NearBranchTarget;
                    // You could check if it's just jumping forward a few bytes and follow it
                    // but this is a simple example so we'll fail.
                }
                goto default;

            case FlowControl.IndirectBranch:// eg. jmp reg/mem
            case FlowControl.ConditionalBranch:// eg. je, jno, etc
            case FlowControl.Return:// eg. ret
            case FlowControl.Call:// eg. call method
            case FlowControl.IndirectCall:// eg. call reg/mem
            case FlowControl.Interrupt:// eg. int n
            case FlowControl.XbeginXabortXend:
            case FlowControl.Exception:// eg. ud0
            default:
                throw new Exception("Not supported by this simple example");
            }
        }
        if (totalBytes < requiredBytes)
            throw new Exception("Not enough bytes!");
        Debug.Assert(origInstructions.Count > 0);
        // Create a JMP instruction that branches to the original code, except those instructions
        // that we'll re-encode. We don't need to do it if it already ends in 'ret'
        var lastInstr = origInstructions[origInstructions.Count - 1];
        if (lastInstr.FlowControl != FlowControl.Return)
            origInstructions.Add(Instruction.CreateBranch(Code.Jmp_rel32_64, lastInstr.NextIP));

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
        var codeWriter = new CodeWriterImpl();
        ulong relocatedBaseAddress = exampleCodeRIP + 0x200000;
        var block = new InstructionBlock(codeWriter, origInstructions, relocatedBaseAddress);
        // This method can also encode more than one block but that's rarely needed, see above comment.
        bool success = BlockEncoder.TryEncode(decoder.Bitness, block, out var errorMessage, out _);
        if (!success) {
            Console.WriteLine($"ERROR: {errorMessage}");
            return;
        }
        var newCode = codeWriter.ToArray();

        // Patch the original code. Pretend that we use some OS API to write to memory...
        // We could use the BlockEncoder/Encoder for this but it's easy to do yourself too.
        // This is 'mov rax,imm64; jmp rax'
        const ulong YOUR_FUNC = 0x123456789ABCDEF0;// Address of your code
        exampleCode[0] = 0x48;// \ 'MOV RAX,imm64'
        exampleCode[1] = 0xB8;// /
        ulong v = YOUR_FUNC;
        for (int i = 0; i < 8; i++, v >>= 8)
            exampleCode[2 + i] = (byte)v;
        exampleCode[10] = 0xFF;// \ JMP RAX
        exampleCode[11] = 0xE0;// /

        // Disassemble it
        Console.WriteLine("Original + patched code:");
        var formatter = new NasmFormatter();
        var output = new StringOutput();
        codeReader = new ByteArrayCodeReader(exampleCode);
        decoder = Decoder.Create(exampleCodeBitness, codeReader);
        decoder.IP = exampleCodeRIP;
        while (codeReader.CanReadByte) {
            Instruction instr;
            if (decoder.IP == exampleCodeRIP + requiredBytes && lastInstr.NextIP - decoder.IP != 0) {
                // The instruction was partially overwritten, so just show it as a 'db x,y,z' instead of garbage
                var len = (int)(lastInstr.NextIP - decoder.IP);
                var index = (int)(decoder.IP - exampleCodeRIP);
                instr = Instruction.CreateDeclareByte(exampleCode, index, len);
                instr.NextIP = decoder.IP;
                for (int i = 0; i < len; i++)
                    codeReader.ReadByte();
                decoder.IP += (ulong)len;
            }
            else
                instr = decoder.Decode();
            formatter.Format(instr, output);
            Console.WriteLine($"{instr.IP:X16} {output.ToStringAndReset()}");
        }
        Console.WriteLine();

        // Disassemble the moved code
        Console.WriteLine("Moved code:");
        Disassemble(newCode, relocatedBaseAddress);
    }
    static void Disassemble(byte[] data, ulong ip) {
        var formatter = new NasmFormatter();
        var output = new StringOutput();
        var codeReader = new ByteArrayCodeReader(data);
        var decoder = Decoder.Create(exampleCodeBitness, codeReader);
        decoder.IP = ip;
        while (codeReader.CanReadByte) {
            decoder.Decode(out var instr);
            formatter.Format(instr, output);
            Console.WriteLine($"{instr.IP:X16} {output.ToStringAndReset()}");
        }
        Console.WriteLine();
    }
    sealed class CodeWriterImpl : CodeWriter {
        readonly List<byte> allBytes = new List<byte>();
        public override void WriteByte(byte value) => allBytes.Add(value);
        public byte[] ToArray() => allBytes.ToArray();
    }

    const int exampleCodeBitness = 64;
    const ulong exampleCodeRIP = 0x00007FFAC46ACDA4;
    static readonly byte[] exampleCode = new byte[] {
        0x48, 0x89, 0x5C, 0x24, 0x10, 0x48, 0x89, 0x74, 0x24, 0x18, 0x55, 0x57, 0x41, 0x56, 0x48, 0x8D,
        0xAC, 0x24, 0x00, 0xFF, 0xFF, 0xFF, 0x48, 0x81, 0xEC, 0x00, 0x02, 0x00, 0x00, 0x48, 0x8B, 0x05,
        0x18, 0x57, 0x0A, 0x00, 0x48, 0x33, 0xC4, 0x48, 0x89, 0x85, 0xF0, 0x00, 0x00, 0x00, 0x4C, 0x8B,
        0x05, 0x2F, 0x24, 0x0A, 0x00, 0x48, 0x8D, 0x05, 0x78, 0x7C, 0x04, 0x00, 0x33, 0xFF
    };
}
```

## Get instruction info, eg. read/written regs/mem, control flow info, etc

```cs
using System;
using Iced.Intel;

static class HowTo_InstructionInfo {
    /*
     * This method produces the following output:
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
    public static void Example() {
        var codeReader = new ByteArrayCodeReader(exampleCode);
        var decoder = Decoder.Create(exampleCodeBitness, codeReader);
        decoder.IP = exampleCodeRIP;

        // Use a factory to create the instruction info if you need register and
        // memory usage. If it's something else, eg. encoding, flags, etc, there
        // are properties on Instruction that can be used instead.
        var instrInfoFactory = new InstructionInfoFactory();
        while (codeReader.CanReadByte) {
            decoder.Decode(out var instr);

            // Gets offsets in the instruction of the displacement and immediates and their sizes.
            // This can be useful if there are relocations in the binary. The encoder has a similar
            // method. This method must be called after Decode() and you must pass in the last
            // instruction Decode() returned.
            var offsets = decoder.GetConstantOffsets(instr);

            Console.WriteLine($"{instr.IP:X16} {instr}");

            var opCode = instr.OpCode;
            // It returns it by ref, so use `ref readonly` to avoid a useless struct copy
            ref readonly var info = ref instrInfoFactory.GetInfo(instr);
            var fpuInfo = instr.GetFpuStackIncrementInfo();
            Console.WriteLine($"    OpCode: {opCode.ToOpCodeString()}");
            Console.WriteLine($"    Instruction: {opCode.ToInstructionString()}");
            Console.WriteLine($"    Encoding: {instr.Encoding}");
            Console.WriteLine($"    Mnemonic: {instr.Mnemonic}");
            Console.WriteLine($"    Code: {instr.Code}");
            Console.WriteLine($"    CpuidFeature: {string.Join(" and ", instr.CpuidFeatures)}");
            Console.WriteLine($"    FlowControl: {instr.FlowControl}");
            if (fpuInfo.WritesTop) {
                if (fpuInfo.Increment == 0)
                    Console.WriteLine($"    FPU TOP: the instruction overwrites TOP");
                else
                    Console.WriteLine($"    FPU TOP inc: {fpuInfo.Increment}");
                Console.WriteLine($"    FPU TOP cond write: {(fpuInfo.Conditional ? "true" : "false")}");
            }
            if (offsets.HasDisplacement)
                Console.WriteLine($"    Displacement offset = {offsets.DisplacementOffset}, size = {offsets.DisplacementSize}");
            if (offsets.HasImmediate)
                Console.WriteLine($"    Immediate offset = {offsets.ImmediateOffset}, size = {offsets.ImmediateSize}");
            if (offsets.HasImmediate2)
                Console.WriteLine($"    Immediate #2 offset = {offsets.ImmediateOffset2}, size = {offsets.ImmediateSize2}");
            if (instr.IsStackInstruction)
                Console.WriteLine($"    SP Increment: {instr.StackPointerIncrement}");
            if (instr.ConditionCode != ConditionCode.None)
                Console.WriteLine($"    Condition code: {instr.ConditionCode}");
            if (instr.RflagsRead != RflagsBits.None)
                Console.WriteLine($"    RFLAGS Read: {instr.RflagsRead}");
            if (instr.RflagsWritten != RflagsBits.None)
                Console.WriteLine($"    RFLAGS Written: {instr.RflagsWritten}");
            if (instr.RflagsCleared != RflagsBits.None)
                Console.WriteLine($"    RFLAGS Cleared: {instr.RflagsCleared}");
            if (instr.RflagsSet != RflagsBits.None)
                Console.WriteLine($"    RFLAGS Set: {instr.RflagsSet}");
            if (instr.RflagsUndefined != RflagsBits.None)
                Console.WriteLine($"    RFLAGS Undefined: {instr.RflagsUndefined}");
            if (instr.RflagsModified != RflagsBits.None)
                Console.WriteLine($"    RFLAGS Modified: {instr.RflagsModified}");
            for (int i = 0; i < instr.OpCount; i++) {
                var opKind = instr.GetOpKind(i);
                if (opKind == OpKind.Memory) {
                    int size = instr.MemorySize.GetSize();
                    if (size != 0)
                        Console.WriteLine($"    Memory size: {size}");
                    break;
                }
            }
            for (int i = 0; i < instr.OpCount; i++)
                Console.WriteLine($"    Op{i}Access: {info.GetOpAccess(i)}");
            for (int i = 0; i < opCode.OpCount; i++)
                Console.WriteLine($"    Op{i}: {opCode.GetOpKind(i)}");
            // The returned iterator is a struct, nothing is allocated unless you box it
            foreach (var regInfo in info.GetUsedRegisters())
                Console.WriteLine($"    Used reg: {regInfo.ToString()}");
            foreach (var memInfo in info.GetUsedMemory())
                Console.WriteLine($"    Used mem: {memInfo.ToString()}");
        }
    }

    const int exampleCodeBitness = 64;
    const ulong exampleCodeRIP = 0x00007FFAC46ACDA4;
    static readonly byte[] exampleCode = new byte[] {
        0x48, 0x89, 0x5C, 0x24, 0x10, 0x48, 0x89, 0x74, 0x24, 0x18, 0x55, 0x57, 0x41, 0x56, 0x48, 0x8D,
        0xAC, 0x24, 0x00, 0xFF, 0xFF, 0xFF, 0x48, 0x81, 0xEC, 0x00, 0x02, 0x00, 0x00, 0x48, 0x8B, 0x05,
        0x18, 0x57, 0x0A, 0x00, 0x48, 0x33, 0xC4, 0x48, 0x89, 0x85, 0xF0, 0x00, 0x00, 0x00, 0x4C, 0x8B,
        0x05, 0x2F, 0x24, 0x0A, 0x00, 0x48, 0x8D, 0x05, 0x78, 0x7C, 0x04, 0x00, 0x33, 0xFF
    };
}
```

## Get the virtual address of a memory operand

```cs
using System;
using System.Diagnostics;
using Iced.Intel;

static class HowTo_GetVirtualAddress {
    public static void Example() {
        // add [rdi+r12*8-5AA5EDCCh],esi
        var reader = new ByteArrayCodeReader("4201B4E734125AA5");
        var decoder = Decoder.Create(64, reader);
        var instr = decoder.Decode();

        // There's also a TryGetVirtualAddress() method
        var va = instr.GetVirtualAddress(0, 0, (register, elementIndex, elementSize) => {
            switch (register) {
            // The base address of ES, CS, SS and DS is always 0 in 64-bit mode
            case Register.ES:
            case Register.CS:
            case Register.SS:
            case Register.DS:
                return 0;
            case Register.RDI:
                return 0x0000_0000_1000_0000;
            case Register.R12:
                return 0x0000_0004_0000_0000;
            default:
                throw new NotImplementedException();
            }
        });
        Debug.Assert(va == 0x0000_001F_B55A_1234);
    }
}
```

## Disassemble old/deprecated CPU instructions

```cs
using System;
using Iced.Intel;

static class HowTo_DisassembleOldInstructions {
    /*
     * This method produces the following output:
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
    public static void Example() {
        var codeBytes = new byte[] {
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
        };

        // Enable decoding of Cyrix/Geode instructions, Centaur ALTINST, MOV to/from TR
        // and MPX instructions.
        // There are other options to enable other instructions such as UMOV, KNC, etc.
        // These are deprecated instructions or only used by old CPUs so they're not
        // enabled by default. Some newer instructions also use the same opcodes as
        // some of these old instructions.
        const DecoderOptions decoderOptions = DecoderOptions.MPX | DecoderOptions.MovTr |
            DecoderOptions.Cyrix | DecoderOptions.Cyrix_DMI | DecoderOptions.ALTINST;
        var codeReader = new ByteArrayCodeReader(codeBytes);
        var decoder = Decoder.Create(32, codeReader, decoderOptions);
        decoder.IP = 0x731E_0A03;

        var formatter = new NasmFormatter();
        formatter.Options.SpaceAfterOperandSeparator = true;
        var output = new StringOutput();

        while (codeReader.CanReadByte) {
            decoder.Decode(out var instr);
            formatter.Format(instr, output);
            Console.WriteLine($"{instr.IP:X8} {output.ToStringAndReset()}");
        }
    }
}
```
