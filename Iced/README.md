
High performance x86 (16/32/64-bit) instruction decoder, encoder and formatter.
It can be used for static analysis of x86/x64 binaries, to rewrite code (eg. remove garbage instructions), to relocate code or as a disassembler.

- Supports all Intel and AMD instructions
- The decoder doesn't allocate any memory and is 2x-5x+ faster than other similar libraries written in C or C#
- Small decoded instructions, only 32 bytes (compared to other libraries that have up to 1KB-sized decoded instructions)
- The formatter supports masm, nasm, gas (AT&T) and Intel (xed) and there are many options to customize the output
- The encoder can be used to re-encode decoded instructions at any address
- The block encoder encodes a list of instructions and optimizes branches to short, near or 'long' (64-bit: 1 or more instructions)
- API to get instruction info, eg. read/written registers, memory and rflags bits; CPUID feature flag, flow control info, etc
- All instructions are tested (decode, encode, format, instruction info)

= Classes

See below for some examples.

Decoder:

- `Decoder`
- `Instruction`
- `CodeReader`
	- `ByteArrayCodeReader`
- `ConstantOffsets`

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
	- `StringBuilderFormatterOutput`
- `SymbolResolver`

Encoder:

- `Encoder`
- `BlockEncoder`
- `CodeWriter`
- `ConstantOffsets`

Instruction info:

- `Instruction.GetInfo()`
- `InstructionInfo`
- `InstructionInfoFactory`
- `InstructionInfoExtensions`
- `MemorySizeExtensions`
- `RegisterExtensions`

= Examples

TODO:

= License

LGPL v3 or later (GNU Lesser General Public License v3 or later)
