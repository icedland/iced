# iced [![crates.io](https://img.shields.io/crates/v/iced-x86.svg)](https://crates.io/crates/iced-x86) [![NuGet](https://img.shields.io/nuget/v/iced.svg)](https://www.nuget.org/packages/iced/) [![maven](https://img.shields.io/maven-central/v/io.github.icedland.iced/iced-x86)](https://central.sonatype.com/artifact/io.github.icedland.iced/iced-x86/1.21.0) [![pypi](https://img.shields.io/pypi/v/iced-x86.svg)](https://pypi.org/project/iced-x86/) [![GitHub builds](https://github.com/icedland/iced/workflows/GitHub%20CI/badge.svg)](https://github.com/icedland/iced/actions) [![codecov](https://codecov.io/gh/icedland/iced/branch/master/graph/badge.svg)](https://codecov.io/gh/icedland/iced)

<img align="right" width="160px" height="160px" src="logo.png">

iced is a blazing fast and correct x86 (16/32/64-bit) instruction decoder, disassembler and assembler.

- 👍 Supports all Intel and AMD instructions
- 👍 Correct: All instructions are tested and iced has been tested against other disassemblers/assemblers (xed, gas, objdump, masm, dumpbin, nasm, ndisasm) and fuzzed
- 👍 Supports .NET, Rust, Python, JavaScript (WebAssembly)
- 👍 The formatter supports masm, nasm, gas (AT&T), Intel (XED) and there are many options to customize the output
- 👍 Blazing fast: Decodes >250 MB/s and decode+format >130 MB/s (Rust, [see here](https://github.com/icedland/disas-bench/tree/a865849deacfb6c33ee0e78f3a3ad7f4c82099f5#results))
- 👍 Small decoded instructions, only 40 bytes and the decoder doesn't allocate any memory
- 👍 Create instructions with code assembler, eg. `asm.mov(eax, edx)`
- 👍 The encoder can be used to re-encode decoded instructions at any address
- 👍 API to get instruction info, eg. read/written registers, memory and rflags bits; CPUID feature flag, control flow info, etc
- 👍 License: MIT

# Examples

- Rust: [README](https://github.com/icedland/iced/blob/master/src/rust/iced-x86/README.md)
- .NET: [README](https://github.com/icedland/iced/blob/master/src/csharp/Intel/README.md)
- Java: [README](https://github.com/icedland/iced/blob/master/src/java/iced-x86/README.md)
- Python: [README](https://github.com/icedland/iced/blob/master/src/rust/iced-x86-py/README.md)
- JavaScript + WebAssembly: [README](https://github.com/icedland/iced/blob/master/src/rust/iced-x86-js/README.md)
- Lua: [README](https://github.com/icedland/iced/blob/master/src/rust/iced-x86-lua/README.md)

# License

MIT

# Icon

Logo `processor` by [Creative Stall](https://thenounproject.com/creativestall/) from the Noun Project
