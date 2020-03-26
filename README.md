# Iced [![NuGet](https://img.shields.io/nuget/v/Iced.svg)](https://www.nuget.org/packages/Iced/) [![crates.io](https://img.shields.io/crates/v/iced-x86.svg)](https://crates.io/crates/iced-x86) [![GitHub builds](https://github.com/0xd4d/iced/workflows/GitHub%20CI/badge.svg)](https://github.com/0xd4d/iced/actions) [![codecov](https://codecov.io/gh/0xd4d/iced/branch/master/graph/badge.svg)](https://codecov.io/gh/0xd4d/iced)

<img align="right" width="160px" height="160px" src="logo.png">

Iced is a high performance x86 (16/32/64-bit) instruction decoder, disassembler and assembler.

It can be used for static analysis of x86/x64 binaries, to rewrite code (eg. remove garbage instructions), to relocate code or as a disassembler.

- ✔️Supports all Intel and AMD instructions
- ✔️Supports .NET, Rust, JavaScript (WebAssembly)
- ✔️The formatter supports masm, nasm, gas (AT&T), Intel (XED) and there are many options to customize the output
- ✔️The decoder is 4x+ faster than other similar libraries and doesn't allocate any memory
- ✔️Small decoded instructions, only 32 bytes
- ✔️High level Assembler (.NET) providing a simple and lean syntax (e.g `asm.mov(eax, edx)`))
- ✔️The encoder can be used to re-encode decoded instructions at any address
- ✔️API to get instruction info, eg. read/written registers, memory and rflags bits; CPUID feature flag, flow control info, etc
- ✔️All instructions are tested (decode, encode, format, instruction info)
- ✔️License: MIT

# Examples and/or Build Instructions

- .NET: [README](https://github.com/0xd4d/iced/blob/master/src/csharp/Intel/README.md)
- Rust: [README](https://github.com/0xd4d/iced/blob/master/src/rust/iced-x86/README.md)
- JavaScript + WebAssembly: [README](https://github.com/0xd4d/iced/blob/master/src/rust/iced-x86-js/README.md)

# License

MIT

# Icon

Logo `processor` by [Creative Stall](https://thenounproject.com/creativestall/) from the Noun Project
