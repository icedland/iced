TODO:

## Building the code

If on Windows, replace `python3` in all commands with `python` or `py`.

Prerequisites:

- Rust: https://www.rust-lang.org/tools/install
- Python >= 3.6: https://www.python.org/downloads/
- `python3 -m pip install -r requirements.txt`

```sh
# Create the wheel
python3 setup.py bdist_wheel
# Install the built wheel
python3 -m pip install iced-x86 --no-index -f dist
# Uninstall your built copy
python3 -m pip uninstall iced-x86
```

Tests:

- `python3 -m pip install -r requirements-dev.txt`

```sh
python3 setup.py bdist_wheel
python3 -m pip install iced-x86 --no-index -f dist
python3 -m pytest
python3 -m pip uninstall -y iced-x86
```

## How-tos

- [Disassemble (decode and format instructions)](#disassemble-decode-and-format-instructions)
- [Disassemble old/deprecated CPU instructions](#disassemble-olddeprecated-cpu-instructions)

TODO: more examples here

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

HEXBYTES_COLUMN_BYTE_LENGTH = 10
EXAMPLE_CODE_BITNESS = 64
EXAMPLE_CODE_RIP = 0x0000_7FFA_C46A_CDA4
EXAMPLE_CODE = \
    b"\x48\x89\x5C\x24\x10\x48\x89\x74\x24\x18\x55\x57\x41\x56\x48\x8D" \
    b"\xAC\x24\x00\xFF\xFF\xFF\x48\x81\xEC\x00\x02\x00\x00\x48\x8B\x05" \
    b"\x18\x57\x0A\x00\x48\x33\xC4\x48\x89\x85\xF0\x00\x00\x00\x4C\x8B" \
    b"\x05\x2F\x24\x0A\x00\x48\x8D\x05\x78\x7C\x04\x00\x33\xFF"

# Create the decoder and initialize RIP
decoder = Decoder(EXAMPLE_CODE_BITNESS, EXAMPLE_CODE)
decoder.ip = EXAMPLE_CODE_RIP

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
decoder = Decoder(64, b"\x86\x64\x32\x16")
decoder.ip = 0x1234_5678
instr = decoder.decode()

print()
print("Format specifiers example:")
print(f"{instr:f}")
print(f"{instr:g}")
print(f"{instr:i}")
print(f"{instr:m}")
print(f"{instr:n}")
print(f"{instr:gG_xSs}")

# ======  =============================================================================
# F-Spec  Description
# ======  =============================================================================
# f       Fast formatter (masm-like syntax)
# g       GNU Assembler formatter
# i       Intel (XED) formatter
# m       masm formatter
# n       nasm formatter
# X       Uppercase hex numbers with ``0x`` prefix
# x       Lowercase hex numbers with ``0x`` prefix
# H       Uppercase hex numbers with ``h`` suffix
# h       Lowercase hex numbers with ``h`` suffix
# r       RIP-relative memory operands use RIP register instead of abs addr (``[rip+123h]`` vs ``[123456789ABCDEF0h]``)
# U       Uppercase everything except numbers and hex prefixes/suffixes (ignored by fast fmt)
# s       Add a space after the operand separator
# S       Always show the segment register
# B       Don't show the branch size (``SHORT`` or ``NEAR PTR``) (ignored by fast fmt)
# G       (GNU Assembler): Add mnemonic size suffix (eg. ``movl`` vs ``mov``)
# M       Always show the memory size (eg. ``BYTE PTR``) even when not needed
# _       Use digit separators (eg. ``0x12345678`` vs ``0x1234_5678``) (ignored by fast fmt)
# ======  =============================================================================
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
# There are other options to enable other instructions such as UMOV, etc.
# These are deprecated instructions or only used by old CPUs so they're not
# enabled by default. Some newer instructions also use the same opcodes as
# some of these old instructions.
DECODER_OPTIONS = DecoderOptions.MPX | \
    DecoderOptions.MOV_TR | \
    DecoderOptions.CYRIX | \
    DecoderOptions.CYRIX_DMI | \
    DecoderOptions.ALTINST
decoder = Decoder(32, TEST_CODE, DECODER_OPTIONS)
decoder.ip = 0x731E_0A03

for instr in decoder:
    # 'n' format specifier means NASM formatter, see the disassemble
    # example for all possible format specifiers
    print(f"{instr.ip:08X} {instr:ns}")
```
