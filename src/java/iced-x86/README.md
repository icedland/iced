# iced [![maven](https://img.shields.io/maven-central/v/io.github.icedland.iced/iced-x86)](https://central.sonatype.com/artifact/io.github.icedland.iced/iced-x86/1.21.0) [![GitHub builds](https://github.com/icedland/iced/workflows/GitHub%20CI/badge.svg)](https://github.com/icedland/iced/actions)

iced is a blazing fast and correct x86 (16/32/64-bit) instruction decoder, disassembler and assembler written in Java.

- 👍 Supports all Intel and AMD instructions
- 👍 Correct: All instructions are tested and iced has been tested against other disassemblers/assemblers (xed, gas, objdump, masm, dumpbin, nasm, ndisasm) and fuzzed
- 👍 100% Java code
- 👍 The formatter supports masm, nasm, gas (AT&T), Intel (XED) and there are many options to customize the output
- 👍 The decoder decodes >100 MB/s
- 👍 Small decoded instructions, only 40 bytes and the decoder doesn't allocate any memory
- 👍 Create instructions with [code assembler](#assemble-instructions), eg. `asm.mov(eax, edx)`
- 👍 The encoder can be used to re-encode decoded instructions at any address
- 👍 API to get instruction info, eg. read/written registers, memory and rflags bits; CPUID feature flag, control flow info, etc
- 👍 Supports Java 8 or later
- 👍 License: MIT

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

```java
import java.util.ArrayList;

import com.github.icedland.iced.x86.*;
import com.github.icedland.iced.x86.dec.*;
import com.github.icedland.iced.x86.fmt.*;
import com.github.icedland.iced.x86.fmt.nasm.*;

final class Main {
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
    public static void main(String[] args) {
        byte[] codeBytes = exampleCode;
        ByteArrayCodeReader codeReader = new ByteArrayCodeReader(codeBytes);
        Decoder decoder = new Decoder(exampleCodeBitness, codeReader);
        decoder.setIP(exampleCodeRIP);
        long endRip = decoder.getIP() + codeBytes.length;

        ArrayList<Instruction> instructions = new ArrayList<Instruction>();
        while (decoder.getIP() < endRip)
            instructions.add(decoder.decode());

        // Formatters: Masm*, Nasm*, Gas* (AT&T) and Intel* (XED).
        NasmFormatter formatter = new NasmFormatter();
        formatter.getOptions().setDigitSeparator("`");
        formatter.getOptions().setFirstOperandCharIndex(10);
        StringOutput output = new StringOutput();
        for (Instruction instr : instructions) {
            // Don't use instr.toString(), it allocates more, uses masm syntax and default options
            formatter.format(instr, output);
            System.out.print(String.format("%016X", instr.getIP()));
            System.out.print(" ");
            int instrLen = instr.getLength();
            int byteBaseIndex = (int)(instr.getIP() - exampleCodeRIP);
            for (int i = 0; i < instrLen; i++)
                System.out.print(String.format("%02X", codeBytes[byteBaseIndex + i]));
            int missingBytes = HEXBYTES_COLUMN_BYTE_LENGTH - instrLen;
            for (int i = 0; i < missingBytes; i++)
                System.out.print("  ");
            System.out.print(" ");
            System.out.println(output.toStringAndReset());
        }
    }

    static final int HEXBYTES_COLUMN_BYTE_LENGTH = 10;
    static final int exampleCodeBitness = 64;
    static final long exampleCodeRIP = 0x00007FFAC46ACDA4L;
    static final byte[] exampleCode = new byte[] {
        (byte)0x48, (byte)0x89, (byte)0x5C, (byte)0x24, (byte)0x10, (byte)0x48, (byte)0x89, (byte)0x74,
        (byte)0x24, (byte)0x18, (byte)0x55, (byte)0x57, (byte)0x41, (byte)0x56, (byte)0x48, (byte)0x8D,
        (byte)0xAC, (byte)0x24, (byte)0x00, (byte)0xFF, (byte)0xFF, (byte)0xFF, (byte)0x48, (byte)0x81,
        (byte)0xEC, (byte)0x00, (byte)0x02, (byte)0x00, (byte)0x00, (byte)0x48, (byte)0x8B, (byte)0x05,
        (byte)0x18, (byte)0x57, (byte)0x0A, (byte)0x00, (byte)0x48, (byte)0x33, (byte)0xC4, (byte)0x48,
        (byte)0x89, (byte)0x85, (byte)0xF0, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x4C, (byte)0x8B,
        (byte)0x05, (byte)0x2F, (byte)0x24, (byte)0x0A, (byte)0x00, (byte)0x48, (byte)0x8D, (byte)0x05,
        (byte)0x78, (byte)0x7C, (byte)0x04, (byte)0x00, (byte)0x33, (byte)0xFF,
    };
}
```

## Assemble instructions

```java
import java.io.ByteArrayOutputStream;

import com.github.icedland.iced.x86.*;
import com.github.icedland.iced.x86.asm.*;
import static com.github.icedland.iced.x86.asm.AsmRegisters.*;
import com.github.icedland.iced.x86.dec.*;
import com.github.icedland.iced.x86.fmt.*;
import com.github.icedland.iced.x86.fmt.nasm.*;

final class Main {
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
    public static void main(String[] args) {
        // The assembler supports all modes: 16-bit, 32-bit and 64-bit.
        CodeAssembler c = new CodeAssembler(64);

        CodeLabel label1 = c.createLabel();
        CodeLabel data1 = c.createLabel();

        c.push(r15);
        c.add(rax, r15);

        // If the memory operand can only have one size, mem_ptr() can be used. The assembler ignores
        // the memory size unless it's an ambiguous instruction, eg. 'add [mem],123'
        c.mov(rax, mem_ptr(rax));
        c.mov(rax, qword_ptr(rax));

        // The assembler must know the memory size to pick the correct instruction
        // [rax + rcx*8 + 0x10]
        c.cmp(dword_ptr(rax, rcx, 8, 0x10), -1);
        c.jne(label1); // Jump to Label1

        c.inc(rax);

        // Labels can be referenced by memory operands (64-bit only) and call/jmp/jcc/loopcc instructions
        c.lea(rcx, mem_ptr(data1));

        // The assembler has prefix properties that will be added to the following instruction
        c.rep().stosd();
        c.xacquire().lock().add(qword_ptr(rax, rcx), 123);

        // The assembler defaults to VEX instructions. If you need EVEX instructions, set PreferVex=false
        c.setPreferVex(false);
        // or call `c.vex` or `c.evex` prefixes to override the default encoding.
        // AVX-512 decorators are properties on the memory and register operands
        c.vaddpd(zmm1.k3().z(), zmm2, zmm3.rz_sae());
        // To broadcast memory, use the dword_bcst/qword_bcst memory types
        c.vunpcklps(xmm2.k5().z(), xmm6, dword_bcst(rax));

        // You can create anonymous labels, just like in eg. masm, @@, @F and @B
        c.anonymousLabel(); // same as @@: in masm
        c.inc(rax);
        c.je(c.b()); // reference the previous anonymous label
        c.inc(rcx);
        c.je(c.f()); // reference the next anonymous label
        c.nop();
        c.anonymousLabel();
        c.nop();

        // Emit label1:
        c.label(label1);
        // If needed, a zero-bytes instruction can be used as a label but this is optional
        c.zero_bytes();
        c.pop(r15);
        c.ret();
        c.label(data1);
        c.db(0xF3, 0x90); // pause

        final long RIP = 0x1234_5678_1000_0000L;
        // We write all bytes here, see lambda below
        ByteArrayOutputStream generatedBytes = new ByteArrayOutputStream();
        Object result = c.assemble(b -> generatedBytes.write(b), RIP);
        // Check if there was an error
        if (result instanceof String)
            throw new UnsupportedOperationException((String)result);
        CodeAssemblerResult asmResult = (CodeAssemblerResult)result;

        // Disassemble the result
        ByteArrayCodeReader reader = new ByteArrayCodeReader(generatedBytes.toByteArray());
        Decoder decoder = new Decoder(64, reader);
        decoder.setIP(RIP);
        Instruction instr = new Instruction();
        while (reader.canReadByte()) {
            decoder.decode(instr);
            System.out.println(String.format("%X = %s", instr.getIP(), instr));
        }
    }
}
```

## Disassemble with a symbol resolver

```java
import java.util.HashMap;

import com.github.icedland.iced.x86.*;
import com.github.icedland.iced.x86.dec.*;
import com.github.icedland.iced.x86.fmt.*;
import com.github.icedland.iced.x86.fmt.gas.*;

final class Main {
    public static void main(String[] args) {
        HashMap<Long, String> symbols = new HashMap<Long, String>();
        symbols.put(0x5AA55AA5L, "my_data");

        SymbolResolver symResolver = (Instruction instruction, int operand, int instructionOperand,
            long address, int addressSize) -> {
            String symbolText = symbols.get(address);
            if (symbolText != null) {
                // The 'address' arg is the address of the symbol and doesn't have to be identical
                // to the 'address' arg passed to getSymbol(). If it's different from the input
                // address, the formatter will add +N or -N, eg. '[rax+symbol+123]'
                return new SymbolResult(address, symbolText);
            }
            return null;
        };
        byte[] codeBytes = new byte[] {
            (byte)0x48, (byte)0x8B, (byte)0x8A, (byte)0xA5,
            (byte)0x5A, (byte)0xA5, (byte)0x5A, 
        };
        Decoder decoder = new Decoder(64, codeBytes);
        Instruction instr = decoder.decode();

        GasFormatter formatter = new GasFormatter(symResolver);
        StringOutput output = new StringOutput();
        formatter.format(instr, output);
        // Prints: mov my_data(%rdx),%rcx
        System.out.println(output.toStringAndReset());
    }
}
```

## Disassemble with colorized text

```java
// NOTE: If on Windows, you may need to use the new
// Windows Terminal to see any colors.

import com.github.icedland.iced.x86.*;
import com.github.icedland.iced.x86.dec.*;
import com.github.icedland.iced.x86.fmt.*;
import com.github.icedland.iced.x86.fmt.masm.*;

final class Main {
    public static void main(String[] args) {
        Decoder decoder = new Decoder(exampleCodeBitness, exampleCode);
        decoder.setIP(exampleCodeRIP);

        MasmFormatter formatter = new MasmFormatter();
        // Create our custom formatter output
        FormatterOutput output = new FormatterOutput() {
            @Override
            public void write(String text, int kind) {
                System.out.print(getColor(kind));
                System.out.print(text);
                // Reset colors
                System.out.print("\u001B[0m");
            }
        };
        for (Instruction instr : decoder) {
            formatter.format(instr, output);
            System.out.println();
        }
    }

    static String getColor(int kind) {
        switch (kind) {
        case FormatterTextKind.DIRECTIVE:
        case FormatterTextKind.KEYWORD:
            // Yellow
            return "\u001B[33m";

        case FormatterTextKind.PREFIX:
        case FormatterTextKind.MNEMONIC:
            // Red
            return "\u001B[31m";

        case FormatterTextKind.REGISTER:
            // Magenta
            return "\u001B[35m";

        case FormatterTextKind.NUMBER:
            // Green
            return "\u001B[32m";

        default:
            // White
            return "\u001B[37m";
        }
    }

    static final int exampleCodeBitness = 64;
    static final long exampleCodeRIP = 0x00007FFAC46ACDA4L;
    static final byte[] exampleCode = new byte[] {
        (byte)0x48, (byte)0x89, (byte)0x5C, (byte)0x24, (byte)0x10, (byte)0x48, (byte)0x89, (byte)0x74,
        (byte)0x24, (byte)0x18, (byte)0x55, (byte)0x57, (byte)0x41, (byte)0x56, (byte)0x48, (byte)0x8D,
        (byte)0xAC, (byte)0x24, (byte)0x00, (byte)0xFF, (byte)0xFF, (byte)0xFF, (byte)0x48, (byte)0x81,
        (byte)0xEC, (byte)0x00, (byte)0x02, (byte)0x00, (byte)0x00, (byte)0x48, (byte)0x8B, (byte)0x05,
        (byte)0x18, (byte)0x57, (byte)0x0A, (byte)0x00, (byte)0x48, (byte)0x33, (byte)0xC4, (byte)0x48,
        (byte)0x89, (byte)0x85, (byte)0xF0, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x4C, (byte)0x8B,
        (byte)0x05, (byte)0x2F, (byte)0x24, (byte)0x0A, (byte)0x00, (byte)0x48, (byte)0x8D, (byte)0x05,
        (byte)0x78, (byte)0x7C, (byte)0x04, (byte)0x00, (byte)0x33, (byte)0xFF,
    };
}
```

## Move code in memory (eg. hook a function)

```java
import java.io.ByteArrayOutputStream;
import java.util.ArrayList;

import com.github.icedland.iced.x86.*;
import com.github.icedland.iced.x86.dec.*;
import com.github.icedland.iced.x86.enc.*;
import com.github.icedland.iced.x86.fmt.*;
import com.github.icedland.iced.x86.fmt.nasm.*;

final class Main {
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
    public static void main(String[] args) {
        System.out.println("Original code:");
        disassemble(exampleCode, exampleCodeRIP);

        ByteArrayCodeReader codeReader = new ByteArrayCodeReader(exampleCode);
        Decoder decoder = new Decoder(exampleCodeBitness, codeReader);
        decoder.setIP(exampleCodeRIP);

        // In 64-bit mode, we need 12 bytes to jump to any address:
        //      mov rax,imm64   // 10
        //      jmp rax         // 2
        // We overwrite rax because it's probably not used by the called function.
        // In 32-bit mode, a normal JMP is just 5 bytes
        final int requiredBytes = 10 + 2;
        int totalBytes = 0;
        ArrayList<Instruction> origInstructions = new ArrayList<Instruction>();
        while (codeReader.canReadByte()) {
            Instruction instr = decoder.decode();
            origInstructions.add(instr);
            totalBytes += instr.getLength();
            if (instr.isInvalid())
                throw new UnsupportedOperationException("Found garbage");
            if (totalBytes >= requiredBytes)
                break;

            switch (instr.getFlowControl()) {
            case FlowControl.NEXT:
                break;

            case FlowControl.UNCONDITIONAL_BRANCH:
                if (instr.getOp0Kind() == OpKind.NEAR_BRANCH64) {
                    long target = instr.getNearBranchTarget();
                    // You could check if it's just jumping forward a few bytes and follow it
                    // but this is a simple example so we'll fail.
                }
                throw new UnsupportedOperationException("Not supported by this simple example");

            case FlowControl.INDIRECT_BRANCH:// eg. jmp reg/mem
            case FlowControl.CONDITIONAL_BRANCH:// eg. je, jno, etc
            case FlowControl.RETURN:// eg. ret
            case FlowControl.CALL:// eg. call method
            case FlowControl.INDIRECT_CALL:// eg. call reg/mem
            case FlowControl.INTERRUPT:// eg. int n
            case FlowControl.XBEGIN_XABORT_XEND:
            case FlowControl.EXCEPTION:// eg. ud0
            default:
                throw new UnsupportedOperationException("Not supported by this simple example");
            }
        }
        if (totalBytes < requiredBytes)
            throw new UnsupportedOperationException("Not enough bytes!");
        // Create a JMP instruction that branches to the original code, except those instructions
        // that we'll re-encode. We don't need to do it if it already ends in 'ret'
        Instruction lastInstr = origInstructions.get(origInstructions.size() - 1);
        if (lastInstr.getFlowControl() != FlowControl.RETURN)
            origInstructions.add(Instruction.createBranch(Code.JMP_REL32_64, lastInstr.getNextIP()));

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
        // We write all bytes here, see lambda below
        ByteArrayOutputStream generatedBytes = new ByteArrayOutputStream();
        long relocatedBaseAddress = exampleCodeRIP + 0x200000;
        InstructionBlock block = new InstructionBlock(b -> generatedBytes.write(b), origInstructions, relocatedBaseAddress);
        // This method can also encode more than one block but that's rarely needed, see above comment.
        Object result = BlockEncoder.tryEncode(decoder.getBitness(), block);
        // Check if there was an error
        if (result instanceof String) {
            System.out.println("ERROR: " + (String)result);
            return;
        }
        byte[] newCode = generatedBytes.toByteArray();

        // Patch the original code. Pretend that we use some OS API to write to memory...
        // We could use the BlockEncoder/Encoder for this but it's easy to do yourself too.
        // This is 'mov rax,imm64; jmp rax'
        final long YOUR_FUNC = 0x123456789ABCDEF0L;// Address of your code
        exampleCode[0] = 0x48;      // \ 'MOV RAX,imm64'
        exampleCode[1] = (byte)0xB8;// /
        long v = YOUR_FUNC;
        for (int i = 0; i < 8; i++, v >>= 8)
            exampleCode[2 + i] = (byte)v;
        exampleCode[10] = (byte)0xFF;// \ JMP RAX
        exampleCode[11] = (byte)0xE0;// /

        // Disassemble it
        System.out.println("Original + patched code:");
        NasmFormatter formatter = new NasmFormatter();
        StringOutput output = new StringOutput();
        codeReader = new ByteArrayCodeReader(exampleCode);
        decoder = new Decoder(exampleCodeBitness, codeReader);
        decoder.setIP(exampleCodeRIP);
        while (codeReader.canReadByte()) {
            Instruction instr;
            if (decoder.getIP() == exampleCodeRIP + requiredBytes && lastInstr.getNextIP() - decoder.getIP() != 0) {
                // The instruction was partially overwritten, so just show it as a 'db x,y,z' instead of garbage
                int len = (int)(lastInstr.getNextIP() - decoder.getIP());
                int index = (int)(decoder.getIP() - exampleCodeRIP);
                instr = Instruction.createDeclareByte(exampleCode, index, len);
                instr.setNextIP(decoder.getIP());
                for (int i = 0; i < len; i++)
                    codeReader.readByte();
                decoder.setIP(decoder.getIP() + len);
            }
            else
                instr = decoder.decode();
            formatter.format(instr, output);
            System.out.println(String.format("%016X %s", instr.getIP(), output.toStringAndReset()));
        }
        System.out.println();

        // Disassemble the moved code
        System.out.println("Moved code:");
        disassemble(newCode, relocatedBaseAddress);
    }

    static void disassemble(byte[] data, long ip) {
        NasmFormatter formatter = new NasmFormatter();
        StringOutput output = new StringOutput();
        ByteArrayCodeReader codeReader = new ByteArrayCodeReader(data);
        Decoder decoder = new Decoder(exampleCodeBitness, codeReader);
        decoder.setIP(ip);
        Instruction instr = new Instruction();
        while (codeReader.canReadByte()) {
            decoder.decode(instr);
            formatter.format(instr, output);
            System.out.println(String.format("%016X %s", instr.getIP(), output.toStringAndReset()));
        }
        System.out.println();
    }

    static final int exampleCodeBitness = 64;
    static final long exampleCodeRIP = 0x00007FFAC46ACDA4L;
    static final byte[] exampleCode = new byte[] {
        (byte)0x48, (byte)0x89, (byte)0x5C, (byte)0x24, (byte)0x10, (byte)0x48, (byte)0x89, (byte)0x74,
        (byte)0x24, (byte)0x18, (byte)0x55, (byte)0x57, (byte)0x41, (byte)0x56, (byte)0x48, (byte)0x8D,
        (byte)0xAC, (byte)0x24, (byte)0x00, (byte)0xFF, (byte)0xFF, (byte)0xFF, (byte)0x48, (byte)0x81,
        (byte)0xEC, (byte)0x00, (byte)0x02, (byte)0x00, (byte)0x00, (byte)0x48, (byte)0x8B, (byte)0x05,
        (byte)0x18, (byte)0x57, (byte)0x0A, (byte)0x00, (byte)0x48, (byte)0x33, (byte)0xC4, (byte)0x48,
        (byte)0x89, (byte)0x85, (byte)0xF0, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x4C, (byte)0x8B,
        (byte)0x05, (byte)0x2F, (byte)0x24, (byte)0x0A, (byte)0x00, (byte)0x48, (byte)0x8D, (byte)0x05,
        (byte)0x78, (byte)0x7C, (byte)0x04, (byte)0x00, (byte)0x33, (byte)0xFF,
    };
}
```

## Get instruction info, eg. read/written regs/mem, control flow info, etc

```java
import java.lang.reflect.Field;
import java.lang.reflect.Modifier;
import java.util.HashMap;

import com.github.icedland.iced.x86.*;
import com.github.icedland.iced.x86.dec.*;
import com.github.icedland.iced.x86.info.*;

final class Main {
    /*
     * This method produces the following output:
00007FFAC46ACDA4 mov [rsp+10h],rbx
    OpCode: o64 89 /r
    Instruction: MOV r/m64, r64
    Encoding: LEGACY
    Mnemonic: MOV
    Code: MOV_RM64_R64
    CpuidFeature: X64
    FlowControl: NEXT
    Displacement offset = 4, size = 1
    Memory size: 8
    Op0Access: WRITE
    Op1Access: READ
    Op0: R64_OR_MEM
    Op1: R64_REG
    Used reg: RSP:READ
    Used reg: RBX:READ
    Used mem: [SS:RSP+0x10;UINT64;WRITE]
00007FFAC46ACDA9 mov [rsp+18h],rsi
    OpCode: o64 89 /r
    Instruction: MOV r/m64, r64
    Encoding: LEGACY
    Mnemonic: MOV
    Code: MOV_RM64_R64
    CpuidFeature: X64
    FlowControl: NEXT
    Displacement offset = 4, size = 1
    Memory size: 8
    Op0Access: WRITE
    Op1Access: READ
    Op0: R64_OR_MEM
    Op1: R64_REG
    Used reg: RSP:READ
    Used reg: RSI:READ
    Used mem: [SS:RSP+0x18;UINT64;WRITE]
00007FFAC46ACDAE push rbp
    OpCode: o64 50+ro
    Instruction: PUSH r64
    Encoding: LEGACY
    Mnemonic: PUSH
    Code: PUSH_R64
    CpuidFeature: X64
    FlowControl: NEXT
    SP Increment: -8
    Op0Access: READ
    Op0: R64_OPCODE
    Used reg: RBP:READ
    Used reg: RSP:READ_WRITE
    Used mem: [SS:RSP+0xFFFFFFFFFFFFFFF8;UINT64;WRITE]
00007FFAC46ACDAF push rdi
    OpCode: o64 50+ro
    Instruction: PUSH r64
    Encoding: LEGACY
    Mnemonic: PUSH
    Code: PUSH_R64
    CpuidFeature: X64
    FlowControl: NEXT
    SP Increment: -8
    Op0Access: READ
    Op0: R64_OPCODE
    Used reg: RDI:READ
    Used reg: RSP:READ_WRITE
    Used mem: [SS:RSP+0xFFFFFFFFFFFFFFF8;UINT64;WRITE]
00007FFAC46ACDB0 push r14
    OpCode: o64 50+ro
    Instruction: PUSH r64
    Encoding: LEGACY
    Mnemonic: PUSH
    Code: PUSH_R64
    CpuidFeature: X64
    FlowControl: NEXT
    SP Increment: -8
    Op0Access: READ
    Op0: R64_OPCODE
    Used reg: R14:READ
    Used reg: RSP:READ_WRITE
    Used mem: [SS:RSP+0xFFFFFFFFFFFFFFF8;UINT64;WRITE]
00007FFAC46ACDB2 lea rbp,[rsp-100h]
    OpCode: o64 8D /r
    Instruction: LEA r64, m
    Encoding: LEGACY
    Mnemonic: LEA
    Code: LEA_R64_M
    CpuidFeature: X64
    FlowControl: NEXT
    Displacement offset = 4, size = 4
    Op0Access: WRITE
    Op1Access: NO_MEM_ACCESS
    Op0: R64_REG
    Op1: MEM
    Used reg: RBP:WRITE
    Used reg: RSP:READ
00007FFAC46ACDBA sub rsp,200h
    OpCode: o64 81 /5 id
    Instruction: SUB r/m64, imm32
    Encoding: LEGACY
    Mnemonic: SUB
    Code: SUB_RM64_IMM32
    CpuidFeature: X64
    FlowControl: NEXT
    Immediate offset = 3, size = 4
    RFLAGS Written: OF, SF, ZF, AF, CF, PF
    RFLAGS Modified: OF, SF, ZF, AF, CF, PF
    Op0Access: READ_WRITE
    Op1Access: READ
    Op0: R64_OR_MEM
    Op1: IMM32SEX64
    Used reg: RSP:READ_WRITE
00007FFAC46ACDC1 mov rax,[7FFAC47524E0h]
    OpCode: o64 8B /r
    Instruction: MOV r64, r/m64
    Encoding: LEGACY
    Mnemonic: MOV
    Code: MOV_R64_RM64
    CpuidFeature: X64
    FlowControl: NEXT
    Displacement offset = 3, size = 4
    Memory size: 8
    Op0Access: WRITE
    Op1Access: READ
    Op0: R64_REG
    Op1: R64_OR_MEM
    Used reg: RAX:WRITE
    Used mem: [DS:0x7FFAC47524E0;UINT64;READ]
00007FFAC46ACDC8 xor rax,rsp
    OpCode: o64 33 /r
    Instruction: XOR r64, r/m64
    Encoding: LEGACY
    Mnemonic: XOR
    Code: XOR_R64_RM64
    CpuidFeature: X64
    FlowControl: NEXT
    RFLAGS Written: SF, ZF, PF
    RFLAGS Cleared: OF, CF
    RFLAGS Undefined: AF
    RFLAGS Modified: OF, SF, ZF, AF, CF, PF
    Op0Access: READ_WRITE
    Op1Access: READ
    Op0: R64_REG
    Op1: R64_OR_MEM
    Used reg: RAX:READ_WRITE
    Used reg: RSP:READ
00007FFAC46ACDCB mov [rbp+0F0h],rax
    OpCode: o64 89 /r
    Instruction: MOV r/m64, r64
    Encoding: LEGACY
    Mnemonic: MOV
    Code: MOV_RM64_R64
    CpuidFeature: X64
    FlowControl: NEXT
    Displacement offset = 3, size = 4
    Memory size: 8
    Op0Access: WRITE
    Op1Access: READ
    Op0: R64_OR_MEM
    Op1: R64_REG
    Used reg: RBP:READ
    Used reg: RAX:READ
    Used mem: [SS:RBP+0xF0;UINT64;WRITE]
00007FFAC46ACDD2 mov r8,[7FFAC474F208h]
    OpCode: o64 8B /r
    Instruction: MOV r64, r/m64
    Encoding: LEGACY
    Mnemonic: MOV
    Code: MOV_R64_RM64
    CpuidFeature: X64
    FlowControl: NEXT
    Displacement offset = 3, size = 4
    Memory size: 8
    Op0Access: WRITE
    Op1Access: READ
    Op0: R64_REG
    Op1: R64_OR_MEM
    Used reg: R8:WRITE
    Used mem: [DS:0x7FFAC474F208;UINT64;READ]
00007FFAC46ACDD9 lea rax,[7FFAC46F4A58h]
    OpCode: o64 8D /r
    Instruction: LEA r64, m
    Encoding: LEGACY
    Mnemonic: LEA
    Code: LEA_R64_M
    CpuidFeature: X64
    FlowControl: NEXT
    Displacement offset = 3, size = 4
    Op0Access: WRITE
    Op1Access: NO_MEM_ACCESS
    Op0: R64_REG
    Op1: MEM
    Used reg: RAX:WRITE
00007FFAC46ACDE0 xor edi,edi
    OpCode: o32 33 /r
    Instruction: XOR r32, r/m32
    Encoding: LEGACY
    Mnemonic: XOR
    Code: XOR_R32_RM32
    CpuidFeature: INTEL386
    FlowControl: NEXT
    RFLAGS Cleared: OF, SF, CF
    RFLAGS Set: ZF, PF
    RFLAGS Undefined: AF
    RFLAGS Modified: OF, SF, ZF, AF, CF, PF
    Op0Access: WRITE
    Op1Access: NONE
    Op0: R32_REG
    Op1: R32_OR_MEM
    Used reg: RDI:WRITE
     */
    public static void main(String[] args) {
        ByteArrayCodeReader codeReader = new ByteArrayCodeReader(exampleCode);
        Decoder decoder = new Decoder(exampleCodeBitness, codeReader);
        decoder.setIP(exampleCodeRIP);

        // Use a factory to create the instruction info if you need register and
        // memory usage. If it's something else, eg. encoding, flags, etc, there
        // are properties on Instruction that can be used instead.
        InstructionInfoFactory instrInfoFactory = new InstructionInfoFactory();
        Instruction instr = new Instruction();
        while (codeReader.canReadByte()) {
            decoder.decode(instr);

            // Gets offsets in the instruction of the displacement and immediates and their sizes.
            // This can be useful if there are relocations in the binary. The encoder has a similar
            // method. This method must be called after Decode() and you must pass in the last
            // instruction Decode() returned.
            ConstantOffsets offsets = decoder.getConstantOffsets(instr);

            System.out.println(String.format("%016X %s", instr.getIP(), instr));

            OpCodeInfo opCode = instr.getOpCode();
            InstructionInfo info = instrInfoFactory.getInfo(instr);
            FpuStackIncrementInfo fpuInfo = instr.getFpuStackIncrementInfo();
            System.out.println(String.format("    OpCode: %s", opCode.toOpCodeString()));
            System.out.println(String.format("    Instruction: %s", opCode.toInstructionString()));
            System.out.println(String.format("    Encoding: %s", toEncoding(instr.getEncoding())));
            System.out.println(String.format("    Mnemonic: %s", toMnemonic(instr.getMnemonic())));
            System.out.println(String.format("    Code: %s", toCode(instr.getCode())));
            System.out.println(String.format("    CpuidFeature: %s", toCpuidFeatures(instr.getCpuidFeatures())));
            System.out.println(String.format("    FlowControl: %s", toFlowControl(instr.getFlowControl())));
            if (fpuInfo.writesTop) {
                if (fpuInfo.increment == 0)
                    System.out.println("    FPU TOP: the instruction overwrites TOP");
                else
                    System.out.println(String.format("    FPU TOP inc: %d", fpuInfo.increment));
                System.out.println(String.format("    FPU TOP cond write: %s", fpuInfo.conditional ? "true" : "false"));
            }
            if (offsets.hasDisplacement())
                System.out.println(String.format("    Displacement offset = %d, size = %d", offsets.displacementOffset, offsets.displacementSize));
            if (offsets.hasImmediate())
                System.out.println(String.format("    Immediate offset = %d, size = %d", offsets.immediateOffset, offsets.immediateSize));
            if (offsets.hasImmediate2())
                System.out.println(String.format("    Immediate #2 offset = %d, size = %d", offsets.immediateOffset2, offsets.immediateSize2));
            if (instr.isStackInstruction())
                System.out.println(String.format("    SP Increment: %d", instr.getStackPointerIncrement()));
            if (instr.getConditionCode() != ConditionCode.NONE)
                System.out.println(String.format("    Condition code: %s", toConditionCode(instr.getConditionCode())));
            if (instr.getRflagsRead() != RflagsBits.NONE)
                System.out.println(String.format("    RFLAGS Read: %s", toRflagsBits(instr.getRflagsRead())));
            if (instr.getRflagsWritten() != RflagsBits.NONE)
                System.out.println(String.format("    RFLAGS Written: %s", toRflagsBits(instr.getRflagsWritten())));
            if (instr.getRflagsCleared() != RflagsBits.NONE)
                System.out.println(String.format("    RFLAGS Cleared: %s", toRflagsBits(instr.getRflagsCleared())));
            if (instr.getRflagsSet() != RflagsBits.NONE)
                System.out.println(String.format("    RFLAGS Set: %s", toRflagsBits(instr.getRflagsSet())));
            if (instr.getRflagsUndefined() != RflagsBits.NONE)
                System.out.println(String.format("    RFLAGS Undefined: %s", toRflagsBits(instr.getRflagsUndefined())));
            if (instr.getRflagsModified() != RflagsBits.NONE)
                System.out.println(String.format("    RFLAGS Modified: %s", toRflagsBits(instr.getRflagsModified())));
            for (int i = 0; i < instr.getOpCount(); i++) {
                int opKind = instr.getOpKind(i);
                if (opKind == OpKind.MEMORY) {
                    int size = MemorySize.getSize(instr.getMemorySize());
                    if (size != 0)
                        System.out.println(String.format("    Memory size: %d", size));
                    break;
                }
            }
            for (int i = 0; i < instr.getOpCount(); i++)
                System.out.println(String.format("    Op%dAccess: %s", i, toOpAccess(info.getOpAccess(i))));
            for (int i = 0; i < opCode.getOpCount(); i++)
                System.out.println(String.format("    Op%d: %s", i, toOpCodeOperandKind(opCode.getOpKind(i))));
            for (UsedRegister regInfo : info.getUsedRegisters())
                System.out.println(String.format("    Used reg: %s", toUsedRegister(regInfo)));
            for (UsedMemory memInfo : info.getUsedMemory())
                System.out.println(String.format("    Used mem: %s", toUsedMemory(memInfo)));
        }
    }

    static final HashMap<Integer, String> encodingMap;
    static final HashMap<Integer, String> mnemonicMap;
    static final HashMap<Integer, String> codeMap;
    static final HashMap<Integer, String> cpuidFeatureMap;
    static final HashMap<Integer, String> flowControlMap;
    static final HashMap<Integer, String> opAccessMap;
    static final HashMap<Integer, String> opCodeOperandKindMap;
    static final HashMap<Integer, String> conditionCodeMap;
    static final HashMap<Integer, String> registerMap;
    static final HashMap<Integer, String> memorySizeMap;

    static {
        encodingMap = createHashMap(EncodingKind.class);
        mnemonicMap = createHashMap(Mnemonic.class);
        codeMap = createHashMap(Code.class);
        cpuidFeatureMap = createHashMap(CpuidFeature.class);
        flowControlMap = createHashMap(FlowControl.class);
        opAccessMap = createHashMap(OpAccess.class);
        opCodeOperandKindMap = createHashMap(OpCodeOperandKind.class);
        conditionCodeMap = createHashMap(ConditionCode.class);
        registerMap = createHashMap(Register.class);
        memorySizeMap = createHashMap(MemorySize.class);
    }

    static HashMap<Integer, String> createHashMap(Class cls) {
        HashMap<Integer, String> result = new HashMap<Integer, String>();
        for (Field field : cls.getDeclaredFields()) {
            if ((field.getModifiers() & Modifier.FINAL) == 0)
                continue;
            if ((field.getModifiers() & Modifier.STATIC) == 0)
                continue;
            try {
                result.put(field.getInt(null), field.getName());
            } catch (IllegalAccessException ex) {
                throw new UnsupportedOperationException(ex);
            }
        }
        return result;
    }

    static String getMapValue(HashMap<Integer, String> map, int value) {
        String name = map.get(value);
        if (name != null)
            return name;
        return String.format("0x%X", value);
    }

    static String toEncoding(int value) { return getMapValue(encodingMap, value); }
    static String toMnemonic(int value) { return getMapValue(mnemonicMap, value); }
    static String toCode(int value) { return getMapValue(codeMap, value); }
    static String toCpuidFeature(int value) { return getMapValue(cpuidFeatureMap, value); }
    static String toFlowControl(int value) { return getMapValue(flowControlMap, value); }
    static String toConditionCode(int value) { return getMapValue(conditionCodeMap, value); }
    static String toOpAccess(int value) { return getMapValue(opAccessMap, value); }
    static String toOpCodeOperandKind(int value) { return getMapValue(opCodeOperandKindMap, value); }
    static String toRegister(int value) { return getMapValue(registerMap, value); }
    static String toMemorySize(int value) { return getMapValue(memorySizeMap, value); }

    static String toCpuidFeatures(int[] cpuidFeatures) {
        StringBuilder sb = new StringBuilder();
        for (int cpuidFeature : cpuidFeatures) {
            if (sb.length() > 0)
                sb.append(" and ");
            sb.append(toCpuidFeature(cpuidFeature));
        }
        return sb.toString();
    }

    static int addBit(StringBuilder sb, int value, int flag, String name) {
        if ((value & flag) != 0) {
            if (sb.length() != 0)
                sb.append(", ");
            sb.append(name);
            value &= ~flag;
        }
        return value;
    }

    static String toRflagsBits(int value) {
        StringBuilder sb = new StringBuilder();

        value = addBit(sb, value, RflagsBits.OF, "OF");
        value = addBit(sb, value, RflagsBits.SF, "SF");
        value = addBit(sb, value, RflagsBits.ZF, "ZF");
        value = addBit(sb, value, RflagsBits.AF, "AF");
        value = addBit(sb, value, RflagsBits.CF, "CF");
        value = addBit(sb, value, RflagsBits.PF, "PF");
        value = addBit(sb, value, RflagsBits.DF, "DF");
        value = addBit(sb, value, RflagsBits.IF, "IF");
        value = addBit(sb, value, RflagsBits.AC, "AC");
        value = addBit(sb, value, RflagsBits.UIF, "UIF");
        value = addBit(sb, value, RflagsBits.C0, "C0");
        value = addBit(sb, value, RflagsBits.C1, "C1");
        value = addBit(sb, value, RflagsBits.C2, "C2");
        value = addBit(sb, value, RflagsBits.C3, "C3");
        if (value != 0) {
            if (sb.length() != 0)
                sb.append(", ");
            sb.append(String.format("0x%X", value));
        }

        return sb.toString();
    }

    static String toUsedRegister(UsedRegister reg) {
        return toRegister(reg.getRegister()) + ":" + toOpAccess(reg.getAccess());
    }

    static String toUsedMemory(UsedMemory mem) {
        StringBuilder sb = new StringBuilder();
        sb.append('[');
        sb.append(toRegister(mem.getSegment()));
        sb.append(':');
        boolean needPlus = false;
        if (mem.getBase() != Register.NONE) {
            sb.append(toRegister(mem.getBase()));
            needPlus = true;
        }
        if (mem.getIndex() != Register.NONE) {
            if (needPlus)
                sb.append('+');
            needPlus = true;
            sb.append(toRegister(mem.getIndex()));
            if (mem.getScale() != 1) {
                sb.append('*');
                sb.append((char)('0' + mem.getScale()));
            }
        }
        if (mem.getDisplacement() != 0 || !needPlus) {
            if (needPlus)
                sb.append('+');
            sb.append("0x");
            sb.append(String.format("%X", mem.getDisplacement()));
        }
        sb.append(';');
        sb.append(toMemorySize(mem.getMemorySize()));
        sb.append(';');
        sb.append(toOpAccess(mem.getAccess()));
        sb.append(']');
        return sb.toString();
    }

    static final int exampleCodeBitness = 64;
    static final long exampleCodeRIP = 0x00007FFAC46ACDA4L;
    static final byte[] exampleCode = new byte[] {
        (byte)0x48, (byte)0x89, (byte)0x5C, (byte)0x24, (byte)0x10, (byte)0x48, (byte)0x89, (byte)0x74,
        (byte)0x24, (byte)0x18, (byte)0x55, (byte)0x57, (byte)0x41, (byte)0x56, (byte)0x48, (byte)0x8D,
        (byte)0xAC, (byte)0x24, (byte)0x00, (byte)0xFF, (byte)0xFF, (byte)0xFF, (byte)0x48, (byte)0x81,
        (byte)0xEC, (byte)0x00, (byte)0x02, (byte)0x00, (byte)0x00, (byte)0x48, (byte)0x8B, (byte)0x05,
        (byte)0x18, (byte)0x57, (byte)0x0A, (byte)0x00, (byte)0x48, (byte)0x33, (byte)0xC4, (byte)0x48,
        (byte)0x89, (byte)0x85, (byte)0xF0, (byte)0x00, (byte)0x00, (byte)0x00, (byte)0x4C, (byte)0x8B,
        (byte)0x05, (byte)0x2F, (byte)0x24, (byte)0x0A, (byte)0x00, (byte)0x48, (byte)0x8D, (byte)0x05,
        (byte)0x78, (byte)0x7C, (byte)0x04, (byte)0x00, (byte)0x33, (byte)0xFF,
    };
}
```

## Get the virtual address of a memory operand

```java
import com.github.icedland.iced.x86.*;
import com.github.icedland.iced.x86.dec.*;

final class Main {
    public static void main(String[] args) {
        // add [rdi+r12*8-5AA5EDCCh],esi
        byte[] code = new byte[] {
            (byte)0x42, (byte)0x01, (byte)0xB4, (byte)0xE7, (byte)0x34,
            (byte)0x12, (byte)0x5A, (byte)0xA5, 
        };
        Decoder decoder = new Decoder(64, code);
        Instruction instr = decoder.decode();

        Long va = instr.getVirtualAddress(0, 0, (register, elementIndex, elementSize) -> {
            switch (register) {
            // The base address of ES, CS, SS and DS is always 0 in 64-bit mode
            case Register.ES:
            case Register.CS:
            case Register.SS:
            case Register.DS:
                return 0L;
            case Register.RDI:
                return 0x0000_0000_1000_0000L;
            case Register.R12:
                return 0x0000_0004_0000_0000L;
            default:
                throw new UnsupportedOperationException();
            }
        });
        if (va == null)
            System.out.println("Couldn't get the va");
        else {
            System.out.println(String.format("va = 0x%X", va));
            if (va != 0x0000_001F_B55A_1234L)
                throw new UnsupportedOperationException();
        }
    }
}
```

## Disassemble old/deprecated CPU instructions

```java
import com.github.icedland.iced.x86.*;
import com.github.icedland.iced.x86.dec.*;
import com.github.icedland.iced.x86.fmt.*;
import com.github.icedland.iced.x86.fmt.nasm.*;

final class Main {
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
    public static void main(String[] args) {
        byte[] codeBytes = new byte[] {
            // bndmov bnd1,[eax]
            0x66, 0x0F, 0x1A, 0x08,
            // mov tr3,esi
            0x0F, 0x26, (byte)0xDE,
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
            (byte)0xDF, (byte)0xFC,
            // altinst
            0x0F, 0x3F,
        };

        // Enable decoding of Cyrix/Geode instructions, Centaur ALTINST, MOV to/from TR
        // and MPX instructions.
        // There are other options to enable other instructions such as UMOV, KNC, etc.
        // These are deprecated instructions or only used by old CPUs so they're not
        // enabled by default. Some newer instructions also use the same opcodes as
        // some of these old instructions.
        final int decoderOptions = DecoderOptions.MPX | DecoderOptions.MOV_TR |
            DecoderOptions.CYRIX | DecoderOptions.CYRIX_DMI | DecoderOptions.ALTINST;
        ByteArrayCodeReader codeReader = new ByteArrayCodeReader(codeBytes);
        Decoder decoder = new Decoder(32, codeReader, decoderOptions);
        decoder.setIP(0x731E_0A03);

        NasmFormatter formatter = new NasmFormatter();
        formatter.getOptions().setSpaceAfterOperandSeparator(true);
        StringOutput output = new StringOutput();

        Instruction instr = new Instruction();
        while (codeReader.canReadByte()) {
            decoder.decode(instr);
            formatter.format(instr, output);
            System.out.println(String.format("%08X %s", instr.getIP(), output.toStringAndReset()));
        }
    }
}
```
