# Minimal iced-x86 C++ Disassembler

This is a minimal example that demonstrates how to use the iced-x86 C++ library (decoder-only build) to disassemble binary files.

## Building

The project includes a CMakeLists.txt that builds the disassembler along with the necessary iced-x86 source files:

```bash
mkdir build
cd build
cmake -G "Unix Makefiles" ..
make
```

This creates `disassembler.exe` in the build directory.

## Usage

```
disassembler.exe <filename> [bitness]
```

- `filename`: Path to the binary file to disassemble
- `bitness`: Optional CPU mode (16, 32, or 64-bit, default: 64)

## Example

```bash
# Create a test binary file
python3 -c "
data = bytes([0x90, 0x89, 0xD8, 0x05, 0x78, 0x56, 0x34, 0x12, 0xC3])
with open('test.bin', 'wb') as f:
    f.write(data)
"

# Disassemble it
./disassembler.exe test.bin
```

Output:
```
Disassembling test.bin (64-bit mode):

00001000: 90                 nop
00001001: 89 d8              mov eax, ebx
00001003: 05 78 56 34 12     add eax, 0x12345678
00001008: c3                 ret

Disassembly complete.
```

## Features

- Disassembles entire binary files
- Supports 16-bit, 32-bit, and 64-bit modes
- Shows instruction addresses, bytes, and disassembly
- Simple text-based output format
- Minimal dependencies (only standard C++ libraries)

## Notes

- This is a decoder-only build of iced-x86 (no encoding or formatting features)
- The output format is basic - for more advanced formatting, the full iced-x86 formatter would be needed
- Instructions are disassembled starting from address 0x1000