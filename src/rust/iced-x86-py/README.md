iced-x86 disassembler Python bindings [![pypi](https://img.shields.io/pypi/v/iced-x86.svg)](https://pypi.org/project/iced-x86/) ![Python](https://img.shields.io/pypi/pyversions/iced-x86.svg) ![License](https://img.shields.io/pypi/l/iced-x86.svg)

iced-x86 is a blazing fast and correct x86 (16/32/64-bit) disassembler for Python.

- 👍 Supports all Intel and AMD instructions
- 👍 Correct: All instructions are tested and iced has been tested against other disassemblers/assemblers (xed, gas, objdump, masm, dumpbin, nasm, ndisasm) and fuzzed
- 👍 The formatter supports masm, nasm, gas (AT&T), Intel (XED) and there are many options to customize the output
- 👍 The encoder can be used to re-encode decoded instructions at any address
- 👍 API to get instruction info, eg. read/written registers, memory and rflags bits; CPUID feature flag, control flow info, etc
- 👍 Rust + Python
- 👍 License: MIT

Rust crate: https://github.com/icedland/iced/blob/master/src/rust/iced-x86/README.md

## Installing iced-x86

It's on [PyPI](https://pypi.org/project/iced-x86/) with built wheels for Windows, macOS and Linux so this command should work (use `python` or `py` if on Windows):

```sh
python3 -m pip install -U iced-x86
```

If `pip` tries to build it from source and fails, see below for all required build tools (eg. `python3 -m pip install setuptools wheel setuptools-rust` and Rust https://www.rust-lang.org/tools/install).

## Building the code

If on Windows, replace `python3` in all commands with `python` or `py`.

Prerequisites:

- Rust: https://www.rust-lang.org/tools/install
- Python >= 3.8: https://www.python.org/downloads/
- `python3 -m pip install -r requirements.txt`

```sh
# Create the wheel
python3 setup.py bdist_wheel
# Install the built wheel
python3 -m pip install iced-x86 --no-index -f dist --only-binary iced-x86
# Uninstall your built copy
python3 -m pip uninstall iced-x86
```

Prerequisites (tests/docs):

- `python3 -m pip install -r requirements-dev.txt`

Tests:

```sh
python3 setup.py bdist_wheel
python3 -m pip install iced-x86 --no-index -f dist --only-binary iced-x86
python3 -m pytest
python3 -m pip uninstall -y iced-x86
```

Docs:

```sh
# Need the built module in build/lib/
python3 setup.py bdist_wheel
# Build the docs
python3 -m sphinx --color -n -W --keep-going -b html docs docs/_build
# Test the doc examples
python3 -m sphinx --color -n -W --keep-going -b doctest docs docs/_build
```

## How-tos

- [Disassemble (decode and format instructions)](#disassemble-decode-and-format-instructions)
- [Adding type annotations](#adding-type-annotations)
- [Create and encode instructions](#create-and-encode-instructions)
- [Move code in memory (eg. hook a function)](#move-code-in-memory-eg-hook-a-function)
- [Get instruction info, eg. read/written regs/mem, control flow info, etc](#get-instruction-info-eg-readwritten-regsmem-control-flow-info-etc)
- [Disassemble old/deprecated CPU instructions](#disassemble-olddeprecated-cpu-instructions)

## Disassemble (decode and format instructions)

This example uses a `Decoder` and one of the `Formatter`s to decode and format the code.
The last part shows how to use format specifiers to format instructions.

```python
from iced_x86 import *

# This example produces the following output:
# 00007FFAC46ACDA4 48895C2410           mov       [rsp+10h],rbx
# 00007FFAC46ACDA9 4889742418           mov       [rsp+18h],rsi
# 00007FFAC46ACDAE 55                   push      rbp
# 00007FFAC46ACDAF 57                   push      rdi
# 00007FFAC46ACDB0 4156                 push      r14
# 00007FFAC46ACDB2 488DAC2400FFFFFF     lea       rbp,[rsp-100h]
# 00007FFAC46ACDBA 4881EC00020000       sub       rsp,200h
# 00007FFAC46ACDC1 488B0518570A00       mov       rax,[rel 7FFA`C475`24E0h]
# 00007FFAC46ACDC8 4833C4               xor       rax,rsp
# 00007FFAC46ACDCB 488985F0000000       mov       [rbp+0F0h],rax
# 00007FFAC46ACDD2 4C8B052F240A00       mov       r8,[rel 7FFA`C474`F208h]
# 00007FFAC46ACDD9 488D05787C0400       lea       rax,[rel 7FFA`C46F`4A58h]
# 00007FFAC46ACDE0 33FF                 xor       edi,edi
#
# Format specifiers example:
# xchg [rdx+rsi+16h],ah
# xchg %ah,0x16(%rdx,%rsi)
# xchg [rdx+rsi+16h],ah
# xchg ah,[rdx+rsi+16h]
# xchg ah,[rdx+rsi+16h]
# xchgb %ah, %ds:0x16(%rdx,%rsi)

EXAMPLE_CODE_BITNESS = 64
EXAMPLE_CODE_RIP = 0x0000_7FFA_C46A_CDA4
EXAMPLE_CODE = \
    b"\x48\x89\x5C\x24\x10\x48\x89\x74\x24\x18\x55\x57\x41\x56\x48\x8D" \
    b"\xAC\x24\x00\xFF\xFF\xFF\x48\x81\xEC\x00\x02\x00\x00\x48\x8B\x05" \
    b"\x18\x57\x0A\x00\x48\x33\xC4\x48\x89\x85\xF0\x00\x00\x00\x4C\x8B" \
    b"\x05\x2F\x24\x0A\x00\x48\x8D\x05\x78\x7C\x04\x00\x33\xFF"

# Create the decoder and initialize RIP
decoder = Decoder(EXAMPLE_CODE_BITNESS, EXAMPLE_CODE, ip=EXAMPLE_CODE_RIP)

# Formatters: MASM, NASM, GAS (AT&T) and INTEL (XED).
# There's also `FastFormatter` which is ~1.25x faster. Use it if formatting
# speed is more important than being able to re-assemble formatted
# instructions.
#    formatter = FastFormatter()
formatter = Formatter(FormatterSyntax.NASM)

# Change some options, there are many more
formatter.digit_separator = "`"
formatter.first_operand_char_index = 10

# You can also call decoder.can_decode + decoder.decode()/decode_out(instr)
# but the iterator is faster
for instr in decoder:
    disasm = formatter.format(instr)
    # You can also get only the mnemonic string, or only one or more of the operands:
    #   mnemonic_str = formatter.format_mnemonic(instr, FormatMnemonicOptions.NO_PREFIXES)
    #   op0_str = formatter.format_operand(instr, 0)
    #   operands_str = formatter.format_all_operands(instr)

    start_index = instr.ip - EXAMPLE_CODE_RIP
    bytes_str = EXAMPLE_CODE[start_index:start_index + instr.len].hex().upper()
    # Eg. "00007FFAC46ACDB2 488DAC2400FFFFFF     lea       rbp,[rsp-100h]"
    print(f"{instr.ip:016X} {bytes_str:20} {disasm}")

# Instruction also supports format specifiers, see the table below
decoder = Decoder(64, b"\x86\x64\x32\x16", ip=0x1234_5678)
instr = decoder.decode()

print()
print("Format specifiers example:")
print(f"{instr:f}")
print(f"{instr:g}")
print(f"{instr:i}")
print(f"{instr:m}")
print(f"{instr:n}")
print(f"{instr:gG_xSs}")

# ====== =============================================================================
# F-Spec Description
# ====== =============================================================================
# f      Fast formatter (masm-like syntax)
# g      GNU Assembler formatter
# i      Intel (XED) formatter
# m      masm formatter
# n      nasm formatter
# X      Uppercase hex numbers with ``0x`` prefix
# x      Lowercase hex numbers with ``0x`` prefix
# H      Uppercase hex numbers with ``h`` suffix
# h      Lowercase hex numbers with ``h`` suffix
# r      RIP-relative memory operands use RIP register instead of abs addr (``[rip+123h]`` vs ``[123456789ABCDEF0h]``)
# U      Uppercase everything except numbers and hex prefixes/suffixes (ignored by fast fmt)
# s      Add a space after the operand separator
# S      Always show the segment register (memory operands)
# B      Don't show the branch size (``SHORT`` or ``NEAR PTR``) (ignored by fast fmt)
# G      (GNU Assembler): Add mnemonic size suffix (eg. ``movl`` vs ``mov``)
# M      Always show the memory size (eg. ``BYTE PTR``) even when not needed
# _      Use digit separators (eg. ``0x12345678`` vs ``0x1234_5678``) (ignored by fast fmt)
# ====== =============================================================================
```

## Adding type annotations

For performance reasons, real Python enums are not used. They're just too slow. Instead, all enums are
currently modules with constants in them. However, this causes problems with type checkers such as
`mypy` since it sees `int`egers instead of eg. `Register`s.

If you add type annotations to methods or variables, the enum name to use is the enum name with an
appended `_`, eg. if the enum is `Register` (which is a module), use `Register_` as the type name.

You don't need to do this with classes, eg. `Instruction`, since they're not enums.

```python
from iced_x86 import *

BASE_REG: Register_ = Register.RAX

def my_fun(code: Code_, reg: Register_, reg2: Register_) -> Register_:
	return reg2

my_fun(Code.RDTSC, BASE_REG, Register.ECX)
```

## Create and encode instructions

This example uses a `BlockEncoder` to encode created `Instruction`s.

```python
from iced_x86 import *

bitness = 64

# All created instructions get an IP of 0. The label id is just an IP.
# The branch instruction's *target* IP should be equal to the IP of the
# target instruction.
label_id: int = 1

def create_label() -> int:
    global label_id
    idd = label_id
    label_id += 1
    return idd

def add_label(id: int, instruction: Instruction) -> Instruction:
    instruction.ip = id
    return instruction

label1 = create_label()

instructions = []
instructions.append(Instruction.create_reg(Code.PUSH_R64, Register.RBP))
instructions.append(Instruction.create_reg(Code.PUSH_R64, Register.RDI))
instructions.append(Instruction.create_reg(Code.PUSH_R64, Register.RSI))
instructions.append(Instruction.create_reg_u32(
    Code.SUB_RM64_IMM32, Register.RSP, 0x50))
instructions.append(Instruction.create(Code.VEX_VZEROUPPER))
instructions.append(Instruction.create_reg_mem(
    Code.LEA_R64_M, Register.RBP, MemoryOperand(Register.RSP, displ=0x60)))
instructions.append(Instruction.create_reg_reg(
    Code.MOV_R64_RM64, Register.RSI, Register.RCX))
instructions.append(Instruction.create_reg_mem(
    Code.LEA_R64_M, Register.RDI, MemoryOperand(Register.RBP, displ=-0x38)))
instructions.append(Instruction.create_reg_i32(
    Code.MOV_R32_IMM32, Register.ECX, 0x0A))
instructions.append(Instruction.create_reg_reg(
    Code.XOR_R32_RM32, Register.EAX, Register.EAX))
instructions.append(Instruction.create_rep_stosd(bitness))
instructions.append(Instruction.create_reg_u64(
    Code.CMP_RM64_IMM32, Register.RSI, 0x1234_5678))
# Create a branch instruction that references label1
instructions.append(Instruction.create_branch(Code.JNE_REL32_64, label1))
instructions.append(Instruction.create(Code.NOPD))
# Add the instruction that is the target of the branch
instructions.append(add_label(label1, Instruction.create_reg_reg(
    Code.XOR_R32_RM32, Register.R15D, Register.R15D)))

# Create an instruction that accesses some data using an RIP relative memory operand
data1 = create_label()
instructions.append(Instruction.create_reg_mem(
    Code.LEA_R64_M, Register.R14, MemoryOperand(Register.RIP, displ=data1)))
instructions.append(Instruction.create(Code.NOPD))
raw_data = b"\x12\x34\x56\x78"
instructions.append(
    add_label(data1, Instruction.create_declare_byte(raw_data)))

# Use BlockEncoder to encode a block of instructions. This block can contain any
# number of branches and any number of instructions.
# It uses Encoder to encode all instructions.
# If the target of a branch is too far away, it can fix it to use a longer branch.
# This can be disabled by passing in `False` to the ctor.
target_rip = 0x0000_1248_FC84_0000
encoder = BlockEncoder(bitness)
encoder.add_many(instructions)
encoded_bytes = encoder.encode(target_rip)

# Now disassemble the encoded instructions. Note that the 'jmp near'
# instruction was turned into a 'jmp short' instruction because we
# didn't disable branch optimizations.
bytes_code = encoded_bytes[0:len(encoded_bytes) - len(raw_data)]
bytes_data = encoded_bytes[len(encoded_bytes) - len(raw_data):]
decoder = Decoder(bitness, bytes_code, ip=target_rip)
formatter = Formatter(FormatterSyntax.GAS)
formatter.first_operand_char_index = 8
for instruction in decoder:
    disasm = formatter.format(instruction)
    print(f"{instruction.ip:016X} {disasm}")

db = Instruction.create_declare_byte(bytes_data)
print(f"{decoder.ip:016X} {formatter.format(db)}")

# Output:
# 00001248FC840000 push    %rbp
# 00001248FC840001 push    %rdi
# 00001248FC840002 push    %rsi
# 00001248FC840003 sub     $0x50,%rsp
# 00001248FC84000A vzeroupper
# 00001248FC84000D lea     0x60(%rsp),%rbp
# 00001248FC840012 mov     %rcx,%rsi
# 00001248FC840015 lea     -0x38(%rbp),%rdi
# 00001248FC840019 mov     $0xA,%ecx
# 00001248FC84001E xor     %eax,%eax
# 00001248FC840020 rep stos %eax,(%rdi)
# 00001248FC840022 cmp     $0x12345678,%rsi
# 00001248FC840029 jne     0x00001248FC84002C
# 00001248FC84002B nop
# 00001248FC84002C xor     %r15d,%r15d
# 00001248FC84002F lea     0x1248FC840037,%r14
# 00001248FC840036 nop
# 00001248FC840037 .byte   0x12,0x34,0x56,0x78
```

## Move code in memory (eg. hook a function)

Uses instruction info API and the encoder to patch a function to jump to the programmer's function.

```python
from iced_x86 import *

# Decodes instructions from some address, then encodes them starting at some
# other address. This can be used to hook a function. You decode enough instructions
# until you have enough bytes to add a JMP instruction that jumps to your code.
# Your code will then conditionally jump to the original code that you re-encoded.
#
# This code uses the BlockEncoder which will help with some things, eg. converting
# short branches to longer branches if the target is too far away.
#
# 64-bit mode also supports RIP relative addressing, but the encoder can't rewrite
# those to use a longer displacement. If any of the moved instructions have RIP
# relative addressing and it tries to access data too far away, the encoder will fail.
# The easiest solution is to use OS alloc functions that allocate memory close to the
# original code (+/-2GB).


# This example produces the following output:
# Original code:
# 00007FFAC46ACDA4 mov [rsp+10h],rbx
# 00007FFAC46ACDA9 mov [rsp+18h],rsi
# 00007FFAC46ACDAE push rbp
# 00007FFAC46ACDAF push rdi
# 00007FFAC46ACDB0 push r14
# 00007FFAC46ACDB2 lea rbp,[rsp-100h]
# 00007FFAC46ACDBA sub rsp,200h
# 00007FFAC46ACDC1 mov rax,[rel 7FFAC47524E0h]
# 00007FFAC46ACDC8 xor rax,rsp
# 00007FFAC46ACDCB mov [rbp+0F0h],rax
# 00007FFAC46ACDD2 mov r8,[rel 7FFAC474F208h]
# 00007FFAC46ACDD9 lea rax,[rel 7FFAC46F4A58h]
# 00007FFAC46ACDE0 xor edi,edi
#
# Original + patched code:
# 00007FFAC46ACDA4 mov rax,123456789ABCDEF0h
# 00007FFAC46ACDAE jmp rax
# 00007FFAC46ACDB0 push r14
# 00007FFAC46ACDB2 lea rbp,[rsp-100h]
# 00007FFAC46ACDBA sub rsp,200h
# 00007FFAC46ACDC1 mov rax,[rel 7FFAC47524E0h]
# 00007FFAC46ACDC8 xor rax,rsp
# 00007FFAC46ACDCB mov [rbp+0F0h],rax
# 00007FFAC46ACDD2 mov r8,[rel 7FFAC474F208h]
# 00007FFAC46ACDD9 lea rax,[rel 7FFAC46F4A58h]
# 00007FFAC46ACDE0 xor edi,edi
#
# Moved code:
# 00007FFAC48ACDA4 mov [rsp+10h],rbx
# 00007FFAC48ACDA9 mov [rsp+18h],rsi
# 00007FFAC48ACDAE push rbp
# 00007FFAC48ACDAF push rdi
# 00007FFAC48ACDB0 jmp 00007FFAC46ACDB0h

def disassemble(data: bytes, ip: int) -> None:
    formatter = Formatter(FormatterSyntax.NASM)
    decoder = Decoder(EXAMPLE_CODE_BITNESS, data, ip=ip)
    for instruction in decoder:
        disasm = formatter.format(instruction)
        print(f"{instruction.ip:016X} {disasm}")
    print()

def how_to_move_code() -> None:
    print("Original code:")
    disassemble(EXAMPLE_CODE, EXAMPLE_CODE_RIP)

    decoder = Decoder(EXAMPLE_CODE_BITNESS, EXAMPLE_CODE, ip=EXAMPLE_CODE_RIP)

    # In 64-bit mode, we need 12 bytes to jump to any address:
    #      mov rax,imm64   # 10
    #      jmp rax         # 2
    # We overwrite rax because it's probably not used by the called function.
    # In 32-bit mode, a normal JMP is just 5 bytes
    required_bytes = 10 + 2
    total_bytes = 0
    orig_instructions = []
    for instr in decoder:
        orig_instructions.append(instr)
        total_bytes += instr.len
        if not instr:
            raise ValueError("Found garbage")
        if total_bytes >= required_bytes:
            break

        cflow = instr.flow_control
        if cflow == FlowControl.NEXT:
            pass  # nothing
        elif cflow == FlowControl.UNCONDITIONAL_BRANCH:
            if instr.op0_kind == OpKind.NEAR_BRANCH64:
                _target = instr.near_branch_target
                # You could check if it's just jumping forward a few bytes and follow it
                # but this is a simple example so we'll fail.
            raise ValueError("Not supported by this simple example")
        else:
            raise ValueError("Not supported by this simple example")
    if total_bytes < required_bytes:
        raise ValueError("Not enough bytes!")
    if len(orig_instructions) == 0:
        raise ValueError("Should not be empty here")
    # Create a JMP instruction that branches to the original code, except those instructions
    # that we'll re-encode. We don't need to do it if it already ends in 'ret'
    last_instr = orig_instructions[-1]
    if last_instr.flow_control != FlowControl.RETURN:
        orig_instructions.append(Instruction.create_branch(Code.JMP_REL32_64, last_instr.next_ip))

    # Relocate the code to some new location. It can fix short/near branches and
    # convert them to short/near/long forms if needed. This also works even if it's a
    # jrcxz/loop/loopcc instruction which only have short forms.
    #
    # It can currently only fix RIP relative operands if the new location is within 2GB
    # of the target data location.
    relocated_base_address = EXAMPLE_CODE_RIP + 0x20_0000
    encoder = BlockEncoder(decoder.bitness)
    encoder.add_many(orig_instructions)
    new_code = encoder.encode(relocated_base_address)

    # Patch the original code. Pretend that we use some OS API to write to memory...
    # We could use the BlockEncoder/Encoder for this but it's easy to do yourself too.
    # This is 'mov rax,imm64; jmp rax'
    YOUR_FUNC: int = 0x1234_5678_9ABC_DEF0  # Address of your code
    example_code = bytearray(EXAMPLE_CODE)
    example_code[0] = 0x48  # \ 'MOV RAX,imm64'
    example_code[1] = 0xB8  # /
    v = YOUR_FUNC
    for i in range(2, 10):
        example_code[i] = v & 0xFF
        v >>= 8
    example_code[10] = 0xFF  # \ JMP RAX
    example_code[11] = 0xE0  # /

    # Disassemble it
    print("Original + patched code:")
    disassemble(example_code, EXAMPLE_CODE_RIP)

    # Disassemble the moved code
    print("Moved code:")
    disassemble(new_code, relocated_base_address)


EXAMPLE_CODE_BITNESS: int = 64
EXAMPLE_CODE_RIP: int = 0x0000_7FFA_C46A_CDA4
EXAMPLE_CODE: bytes = \
    b"\x48\x89\x5C\x24\x10\x48\x89\x74\x24\x18\x55\x57\x41\x56\x48\x8D" \
    b"\xAC\x24\x00\xFF\xFF\xFF\x48\x81\xEC\x00\x02\x00\x00\x48\x8B\x05" \
    b"\x18\x57\x0A\x00\x48\x33\xC4\x48\x89\x85\xF0\x00\x00\x00\x4C\x8B" \
    b"\x05\x2F\x24\x0A\x00\x48\x8D\x05\x78\x7C\x04\x00\x33\xFF"

how_to_move_code()
```

## Get instruction info, eg. read/written regs/mem, control flow info, etc

Shows how to get used registers/memory and other info. It uses `Instruction` methods
and an `InstructionInfoFactory` to get this info.

```python
from iced_x86 import *
from typing import Dict, Sequence
from types import ModuleType

# This code produces the following output:
# 00007FFAC46ACDA4 mov [rsp+10h],rbx
#     OpCode: o64 89 /r
#     Instruction: MOV r/m64, r64
#     Encoding: LEGACY
#     Mnemonic: MOV
#     Code: MOV_RM64_R64
#     CpuidFeature: X64
#     FlowControl: NEXT
#     Displacement offset = 4, size = 1
#     Memory size: 8
#     Op0Access: WRITE
#     Op1Access: READ
#     Op0: R64_OR_MEM
#     Op1: R64_REG
#     Used reg: RSP:READ
#     Used reg: RBX:READ
#     Used mem: [SS:RSP+0x10;UINT64;WRITE]
# 00007FFAC46ACDA9 mov [rsp+18h],rsi
#     OpCode: o64 89 /r
#     Instruction: MOV r/m64, r64
#     Encoding: LEGACY
#     Mnemonic: MOV
#     Code: MOV_RM64_R64
#     CpuidFeature: X64
#     FlowControl: NEXT
#     Displacement offset = 4, size = 1
#     Memory size: 8
#     Op0Access: WRITE
#     Op1Access: READ
#     Op0: R64_OR_MEM
#     Op1: R64_REG
#     Used reg: RSP:READ
#     Used reg: RSI:READ
#     Used mem: [SS:RSP+0x18;UINT64;WRITE]
# 00007FFAC46ACDAE push rbp
#     OpCode: o64 50+ro
#     Instruction: PUSH r64
#     Encoding: LEGACY
#     Mnemonic: PUSH
#     Code: PUSH_R64
#     CpuidFeature: X64
#     FlowControl: NEXT
#     SP Increment: -8
#     Op0Access: READ
#     Op0: R64_OPCODE
#     Used reg: RBP:READ
#     Used reg: RSP:READ_WRITE
#     Used mem: [SS:RSP+0xFFFFFFFFFFFFFFF8;UINT64;WRITE]
# 00007FFAC46ACDAF push rdi
#     OpCode: o64 50+ro
#     Instruction: PUSH r64
#     Encoding: LEGACY
#     Mnemonic: PUSH
#     Code: PUSH_R64
#     CpuidFeature: X64
#     FlowControl: NEXT
#     SP Increment: -8
#     Op0Access: READ
#     Op0: R64_OPCODE
#     Used reg: RDI:READ
#     Used reg: RSP:READ_WRITE
#     Used mem: [SS:RSP+0xFFFFFFFFFFFFFFF8;UINT64;WRITE]
# 00007FFAC46ACDB0 push r14
#     OpCode: o64 50+ro
#     Instruction: PUSH r64
#     Encoding: LEGACY
#     Mnemonic: PUSH
#     Code: PUSH_R64
#     CpuidFeature: X64
#     FlowControl: NEXT
#     SP Increment: -8
#     Op0Access: READ
#     Op0: R64_OPCODE
#     Used reg: R14:READ
#     Used reg: RSP:READ_WRITE
#     Used mem: [SS:RSP+0xFFFFFFFFFFFFFFF8;UINT64;WRITE]
# 00007FFAC46ACDB2 lea rbp,[rsp-100h]
#     OpCode: o64 8D /r
#     Instruction: LEA r64, m
#     Encoding: LEGACY
#     Mnemonic: LEA
#     Code: LEA_R64_M
#     CpuidFeature: X64
#     FlowControl: NEXT
#     Displacement offset = 4, size = 4
#     Op0Access: WRITE
#     Op1Access: NO_MEM_ACCESS
#     Op0: R64_REG
#     Op1: MEM
#     Used reg: RBP:WRITE
#     Used reg: RSP:READ
# 00007FFAC46ACDBA sub rsp,200h
#     OpCode: o64 81 /5 id
#     Instruction: SUB r/m64, imm32
#     Encoding: LEGACY
#     Mnemonic: SUB
#     Code: SUB_RM64_IMM32
#     CpuidFeature: X64
#     FlowControl: NEXT
#     Immediate offset = 3, size = 4
#     RFLAGS Written: OF, SF, ZF, AF, CF, PF
#     RFLAGS Modified: OF, SF, ZF, AF, CF, PF
#     Op0Access: READ_WRITE
#     Op1Access: READ
#     Op0: R64_OR_MEM
#     Op1: IMM32SEX64
#     Used reg: RSP:READ_WRITE
# 00007FFAC46ACDC1 mov rax,[7FFAC47524E0h]
#     OpCode: o64 8B /r
#     Instruction: MOV r64, r/m64
#     Encoding: LEGACY
#     Mnemonic: MOV
#     Code: MOV_R64_RM64
#     CpuidFeature: X64
#     FlowControl: NEXT
#     Displacement offset = 3, size = 4
#     Memory size: 8
#     Op0Access: WRITE
#     Op1Access: READ
#     Op0: R64_REG
#     Op1: R64_OR_MEM
#     Used reg: RAX:WRITE
#     Used mem: [DS:0x7FFAC47524E0;UINT64;READ]
# 00007FFAC46ACDC8 xor rax,rsp
#     OpCode: o64 33 /r
#     Instruction: XOR r64, r/m64
#     Encoding: LEGACY
#     Mnemonic: XOR
#     Code: XOR_R64_RM64
#     CpuidFeature: X64
#     FlowControl: NEXT
#     RFLAGS Written: SF, ZF, PF
#     RFLAGS Cleared: OF, CF
#     RFLAGS Undefined: AF
#     RFLAGS Modified: OF, SF, ZF, AF, CF, PF
#     Op0Access: READ_WRITE
#     Op1Access: READ
#     Op0: R64_REG
#     Op1: R64_OR_MEM
#     Used reg: RAX:READ_WRITE
#     Used reg: RSP:READ
# 00007FFAC46ACDCB mov [rbp+0F0h],rax
#     OpCode: o64 89 /r
#     Instruction: MOV r/m64, r64
#     Encoding: LEGACY
#     Mnemonic: MOV
#     Code: MOV_RM64_R64
#     CpuidFeature: X64
#     FlowControl: NEXT
#     Displacement offset = 3, size = 4
#     Memory size: 8
#     Op0Access: WRITE
#     Op1Access: READ
#     Op0: R64_OR_MEM
#     Op1: R64_REG
#     Used reg: RBP:READ
#     Used reg: RAX:READ
#     Used mem: [SS:RBP+0xF0;UINT64;WRITE]
# 00007FFAC46ACDD2 mov r8,[7FFAC474F208h]
#     OpCode: o64 8B /r
#     Instruction: MOV r64, r/m64
#     Encoding: LEGACY
#     Mnemonic: MOV
#     Code: MOV_R64_RM64
#     CpuidFeature: X64
#     FlowControl: NEXT
#     Displacement offset = 3, size = 4
#     Memory size: 8
#     Op0Access: WRITE
#     Op1Access: READ
#     Op0: R64_REG
#     Op1: R64_OR_MEM
#     Used reg: R8:WRITE
#     Used mem: [DS:0x7FFAC474F208;UINT64;READ]
# 00007FFAC46ACDD9 lea rax,[7FFAC46F4A58h]
#     OpCode: o64 8D /r
#     Instruction: LEA r64, m
#     Encoding: LEGACY
#     Mnemonic: LEA
#     Code: LEA_R64_M
#     CpuidFeature: X64
#     FlowControl: NEXT
#     Displacement offset = 3, size = 4
#     Op0Access: WRITE
#     Op1Access: NO_MEM_ACCESS
#     Op0: R64_REG
#     Op1: MEM
#     Used reg: RAX:WRITE
# 00007FFAC46ACDE0 xor edi,edi
#     OpCode: o32 33 /r
#     Instruction: XOR r32, r/m32
#     Encoding: LEGACY
#     Mnemonic: XOR
#     Code: XOR_R32_RM32
#     CpuidFeature: INTEL386
#     FlowControl: NEXT
#     RFLAGS Cleared: OF, SF, CF
#     RFLAGS Set: ZF, PF
#     RFLAGS Undefined: AF
#     RFLAGS Modified: OF, SF, ZF, AF, CF, PF
#     Op0Access: WRITE
#     Op1Access: NONE
#     Op0: R32_REG
#     Op1: R32_OR_MEM
#     Used reg: RDI:WRITE
def how_to_get_instruction_info() -> None:
    decoder = Decoder(EXAMPLE_CODE_BITNESS, EXAMPLE_CODE, ip=EXAMPLE_CODE_RIP)

    # Use a factory to create the instruction info if you need register and
    # memory usage. If it's something else, eg. encoding, flags, etc, there
    # are Instruction methods that can be used instead.
    info_factory = InstructionInfoFactory()
    for instr in decoder:
        # Gets offsets in the instruction of the displacement and immediates and their sizes.
        # This can be useful if there are relocations in the binary. The encoder has a similar
        # method. This method must be called after decode() and you must pass in the last
        # instruction decode() returned.
        offsets = decoder.get_constant_offsets(instr)

        print(f"{instr.ip:016X} {instr}")

        op_code = instr.op_code()
        info = info_factory.info(instr)
        fpu_info = instr.fpu_stack_increment_info()
        print(f"    OpCode: {op_code.op_code_string}")
        print(f"    Instruction: {op_code.instruction_string}")
        print(f"    Encoding: {encoding_kind_to_string(instr.encoding)}")
        print(f"    Mnemonic: {mnemonic_to_string(instr.mnemonic)}")
        print(f"    Code: {code_to_string(instr.code)}")
        print(f"    CpuidFeature: {cpuid_features_to_string(instr.cpuid_features())}")
        print(f"    FlowControl: {flow_control_to_string(instr.flow_control)}")
        if fpu_info.writes_top:
            if fpu_info.increment == 0:
                print(f"    FPU TOP: the instruction overwrites TOP")
            else:
                print(f"    FPU TOP inc: {fpu_info.increment}")
            cond_write = "True" if fpu_info.conditional else "False"
            print(f"    FPU TOP cond write: {cond_write}")
        if offsets.has_displacement:
            print(f"    Displacement offset = {offsets.displacement_offset}, size = {offsets.displacement_size}")
        if offsets.has_immediate:
            print(f"    Immediate offset = {offsets.immediate_offset}, size = {offsets.immediate_size}")
        if offsets.has_immediate2:
            print(f"    Immediate #2 offset = {offsets.immediate_offset2}, size = {offsets.immediate_size2}")
        if instr.is_stack_instruction:
            print(f"    SP Increment: {instr.stack_pointer_increment}")
        if instr.condition_code != ConditionCode.NONE:
            print(f"    Condition code: {condition_code_to_string(instr.condition_code)}")
        if instr.rflags_read != RflagsBits.NONE:
            print(f"    RFLAGS Read: {rflags_bits_to_string(instr.rflags_read)}")
        if instr.rflags_written != RflagsBits.NONE:
            print(f"    RFLAGS Written: {rflags_bits_to_string(instr.rflags_written)}")
        if instr.rflags_cleared != RflagsBits.NONE:
            print(f"    RFLAGS Cleared: {rflags_bits_to_string(instr.rflags_cleared)}")
        if instr.rflags_set != RflagsBits.NONE:
            print(f"    RFLAGS Set: {rflags_bits_to_string(instr.rflags_set)}")
        if instr.rflags_undefined != RflagsBits.NONE:
            print(f"    RFLAGS Undefined: {rflags_bits_to_string(instr.rflags_undefined)}")
        if instr.rflags_modified != RflagsBits.NONE:
            print(f"    RFLAGS Modified: {rflags_bits_to_string(instr.rflags_modified)}")
        for i in range(instr.op_count):
            op_kind = instr.op_kind(i)
            if op_kind == OpKind.MEMORY:
                size = MemorySizeExt.size(instr.memory_size)
                if size != 0:
                    print(f"    Memory size: {size}")
                break
        for i in range(instr.op_count):
            print(f"    Op{i}Access: {op_access_to_string(info.op_access(i))}")
        for i in range(op_code.op_count):
            print(f"    Op{i}: {op_code_operand_kind_to_string(op_code.op_kind(i))}")
        for reg_info in info.used_registers():
            print(f"    Used reg: {used_reg_to_string(reg_info)}")
        for mem_info in info.used_memory():
            print(f"    Used mem: {used_mem_to_string(mem_info)}")

def rflags_bits_to_string(rf: int) -> str:
    def append(sb: str, s: str) -> str:
        if len(sb) != 0:
            sb += ", "
        return sb + s

    sb = ""
    if (rf & RflagsBits.OF) != 0:
        sb = append(sb, "OF")
    if (rf & RflagsBits.SF) != 0:
        sb = append(sb, "SF")
    if (rf & RflagsBits.ZF) != 0:
        sb = append(sb, "ZF")
    if (rf & RflagsBits.AF) != 0:
        sb = append(sb, "AF")
    if (rf & RflagsBits.CF) != 0:
        sb = append(sb, "CF")
    if (rf & RflagsBits.PF) != 0:
        sb = append(sb, "PF")
    if (rf & RflagsBits.DF) != 0:
        sb = append(sb, "DF")
    if (rf & RflagsBits.IF) != 0:
        sb = append(sb, "IF")
    if (rf & RflagsBits.AC) != 0:
        sb = append(sb, "AC")
    if (rf & RflagsBits.UIF) != 0:
        sb = append(sb, "UIF")
    if len(sb) == 0:
        return "<empty>"
    return sb

EXAMPLE_CODE_BITNESS: int = 64
EXAMPLE_CODE_RIP: int = 0x0000_7FFA_C46A_CDA4
EXAMPLE_CODE: bytes = \
    b"\x48\x89\x5C\x24\x10\x48\x89\x74\x24\x18\x55\x57\x41\x56\x48\x8D" \
    b"\xAC\x24\x00\xFF\xFF\xFF\x48\x81\xEC\x00\x02\x00\x00\x48\x8B\x05" \
    b"\x18\x57\x0A\x00\x48\x33\xC4\x48\x89\x85\xF0\x00\x00\x00\x4C\x8B" \
    b"\x05\x2F\x24\x0A\x00\x48\x8D\x05\x78\x7C\x04\x00\x33\xFF"

def create_enum_dict(module: ModuleType) -> Dict[int, str]:
    return {module.__dict__[key]:key for key in module.__dict__ if isinstance(module.__dict__[key], int)}

REGISTER_TO_STRING: Dict[Register_, str] = create_enum_dict(Register)
def register_to_string(value: Register_) -> str:
    s = REGISTER_TO_STRING.get(value)
    if s is None:
        return str(value) + " /*Register enum*/"
    return s

OP_ACCESS_TO_STRING: Dict[OpAccess_, str] = create_enum_dict(OpAccess)
def op_access_to_string(value: OpAccess_) -> str:
    s = OP_ACCESS_TO_STRING.get(value)
    if s is None:
        return str(value) + " /*OpAccess enum*/"
    return s

ENCODING_KIND_TO_STRING: Dict[EncodingKind_, str] = create_enum_dict(EncodingKind)
def encoding_kind_to_string(value: EncodingKind_) -> str:
    s = ENCODING_KIND_TO_STRING.get(value)
    if s is None:
        return str(value) + " /*EncodingKind enum*/"
    return s

MNEMONIC_TO_STRING: Dict[Mnemonic_, str] = create_enum_dict(Mnemonic)
def mnemonic_to_string(value: Mnemonic_) -> str:
    s = MNEMONIC_TO_STRING.get(value)
    if s is None:
        return str(value) + " /*Mnemonic enum*/"
    return s

CODE_TO_STRING: Dict[Code_, str] = create_enum_dict(Code)
def code_to_string(value: Code_) -> str:
    s = CODE_TO_STRING.get(value)
    if s is None:
        return str(value) + " /*Code enum*/"
    return s

FLOW_CONTROL_TO_STRING: Dict[FlowControl_, str] = create_enum_dict(FlowControl)
def flow_control_to_string(value: FlowControl_) -> str:
    s = FLOW_CONTROL_TO_STRING.get(value)
    if s is None:
        return str(value) + " /*FlowControl enum*/"
    return s

OP_CODE_OPERAND_KIND_TO_STRING: Dict[OpCodeOperandKind_, str] = create_enum_dict(OpCodeOperandKind)
def op_code_operand_kind_to_string(value: OpCodeOperandKind_) -> str:
    s = OP_CODE_OPERAND_KIND_TO_STRING.get(value)
    if s is None:
        return str(value) + " /*OpCodeOperandKind enum*/"
    return s

CPUID_FEATURE_TO_STRING: Dict[CpuidFeature_, str] = create_enum_dict(CpuidFeature)
def cpuid_feature_to_string(value: CpuidFeature_) -> str:
    s = CPUID_FEATURE_TO_STRING.get(value)
    if s is None:
        return str(value) + " /*CpuidFeature enum*/"
    return s

def cpuid_features_to_string(cpuid_features: Sequence[int]) -> str:
    return " and ".join([cpuid_feature_to_string(f) for f in cpuid_features])

MEMORY_SIZE_TO_STRING: Dict[MemorySize_, str] = create_enum_dict(MemorySize)
def memory_size_to_string(value: MemorySize_) -> str:
    s = MEMORY_SIZE_TO_STRING.get(value)
    if s is None:
        return str(value) + " /*MemorySize enum*/"
    return s

CONDITION_CODE_TO_STRING: Dict[ConditionCode_, str] = create_enum_dict(ConditionCode)
def condition_code_to_string(value: ConditionCode_) -> str:
    s = CONDITION_CODE_TO_STRING.get(value)
    if s is None:
        return str(value) + " /*ConditionCode enum*/"
    return s

def used_reg_to_string(reg_info: UsedRegister) -> str:
    return register_to_string(reg_info.register) + ":" + op_access_to_string(reg_info.access)

def used_mem_to_string(mem_info: UsedMemory) -> str:
    sb = "[" + register_to_string(mem_info.segment) + ":"
    need_plus = mem_info.base != Register.NONE
    if need_plus:
        sb += register_to_string(mem_info.base)
    if mem_info.index != Register.NONE:
        if need_plus:
            sb += "+"
        need_plus = True
        sb += register_to_string(mem_info.index)
        if mem_info.scale != 1:
            sb += "*" + str(mem_info.scale)
    if mem_info.displacement != 0 or not need_plus:
        if need_plus:
            sb += "+"
        sb += f"0x{mem_info.displacement:X}"
    sb += ";" + memory_size_to_string(mem_info.memory_size) + ";" + op_access_to_string(mem_info.access) + "]"
    return sb

how_to_get_instruction_info()
```

## Disassemble old/deprecated CPU instructions

```python
from iced_x86 import *

# This example produces the following output:
# 731E0A03 bndmov bnd1, [eax]
# 731E0A07 mov tr3, esi
# 731E0A0A rdshr [eax]
# 731E0A0D dmint
# 731E0A0F svdc [eax], cs
# 731E0A12 cpu_read
# 731E0A14 pmvzb mm1, [eax]
# 731E0A17 frinear
# 731E0A19 altinst

TEST_CODE = \
    b"\x66\x0F\x1A\x08" \
    b"\x0F\x26\xDE" \
    b"\x0F\x36\x00" \
    b"\x0F\x39" \
    b"\x0F\x78\x08" \
    b"\x0F\x3D" \
    b"\x0F\x58\x08" \
    b"\xDF\xFC" \
    b"\x0F\x3F"

# Enable decoding of Cyrix/Geode instructions, Centaur ALTINST,
# MOV to/from TR and MPX instructions.
# There are other options to enable other instructions such as UMOV, KNC, etc.
# These are deprecated instructions or only used by old CPUs so they're not
# enabled by default. Some newer instructions also use the same opcodes as
# some of these old instructions.
DECODER_OPTIONS = DecoderOptions.MPX | \
    DecoderOptions.MOV_TR | \
    DecoderOptions.CYRIX | \
    DecoderOptions.CYRIX_DMI | \
    DecoderOptions.ALTINST
decoder = Decoder(32, TEST_CODE, DECODER_OPTIONS, ip=0x731E_0A03)

for instr in decoder:
    # 'n' format specifier means NASM formatter, see the disassemble
    # example for all possible format specifiers
    print(f"{instr.ip:08X} {instr:ns}")
```
