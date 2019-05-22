/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

#if !NO_MASM_FORMATTER && !NO_FORMATTER
using System;
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Masm {
	public sealed class MasmSymbolResolverTests : SymbolResolverTests {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, int resultDispl, SymbolInstructionInfo info, string formattedString) => FormatBase(index, resultDispl, info, formattedString, MasmFormatterFactory.Create_Resolver(info.SymbolResolver.Clone()));
		public static IEnumerable<object[]> Format_Data => GetFormatData("Masm", nameof(MasmSymbolResolverTests));

		[Flags]
		enum Flags {
			None					= 0,
			// Symbol is found
			Symbol					= 1,
			// Signed symbol
			Signed					= 2,
			// options.SymbolDisplInBrackets
			SymbolDisplInBrackets	= 4,
			// options.DisplInBrackets
			DisplInBrackets			= 8,
			// options.RipRelativeAddresses
			Rip						= 0x10,
			// options.ShowZeroDisplacements
			ShowZeroDisplacements	= 0x20,
			// !options.AddDsPrefix32
			NoAddDsPrefix32			= 0x40,
		}

		[Theory]

		// symbols

		[InlineData("66 8B 06 3412", 16, "mov eax,[symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets)]
		[InlineData("66 8B 06 3412", 16, "mov eax,symbol", Flags.Symbol)]

		[InlineData("8B 05 78563412", 32, "mov eax,[symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets)]
		[InlineData("8B 05 78563412", 32, "mov eax,symbol", Flags.Symbol)]

		[InlineData("8B 04 25 78563412", 64, "mov eax,[symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets)]
		[InlineData("8B 04 25 78563412", 64, "mov eax,symbol", Flags.Symbol)]

		[InlineData("8B 05 78563412", 64, "mov eax,[rip+symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets | Flags.Rip)]
		[InlineData("8B 05 78563412", 64, "mov eax,symbol[rip]", Flags.Symbol | Flags.Rip)]
		[InlineData("8B 05 78563412", 64, "mov eax,[symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets)]
		[InlineData("8B 05 78563412", 64, "mov eax,symbol", Flags.Symbol)]

		[InlineData("8B 80 78563412", 64, "mov eax,[rax+symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets)]
		[InlineData("8B 80 78563412", 64, "mov eax,symbol[rax]", Flags.Symbol)]

		[InlineData("8B 04 4D 78563412", 64, "mov eax,[rcx*2+symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets)]
		[InlineData("8B 04 4D 78563412", 64, "mov eax,symbol[rcx*2]", Flags.Symbol)]

		[InlineData("8B 84 48 78563412", 64, "mov eax,[rax+rcx*2+symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets)]
		[InlineData("8B 84 48 78563412", 64, "mov eax,symbol[rax+rcx*2]", Flags.Symbol)]

		// symbols + seg override

		[InlineData("64 66 8B 06 3412", 16, "mov eax,fs:[symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets)]
		[InlineData("64 66 8B 06 3412", 16, "mov eax,fs:symbol", Flags.Symbol)]

		[InlineData("64 8B 05 78563412", 32, "mov eax,fs:[symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets)]
		[InlineData("64 8B 05 78563412", 32, "mov eax,fs:symbol", Flags.Symbol)]

		[InlineData("64 8B 04 25 78563412", 64, "mov eax,fs:[symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets)]
		[InlineData("64 8B 04 25 78563412", 64, "mov eax,fs:symbol", Flags.Symbol)]

		[InlineData("64 8B 05 78563412", 64, "mov eax,fs:[rip+symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets | Flags.Rip)]
		[InlineData("64 8B 05 78563412", 64, "mov eax,fs:symbol[rip]", Flags.Symbol | Flags.Rip)]
		[InlineData("64 8B 05 78563412", 64, "mov eax,fs:[symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets)]
		[InlineData("64 8B 05 78563412", 64, "mov eax,fs:symbol", Flags.Symbol)]

		[InlineData("64 8B 80 78563412", 64, "mov eax,fs:[rax+symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets)]
		[InlineData("64 8B 80 78563412", 64, "mov eax,fs:symbol[rax]", Flags.Symbol)]

		[InlineData("64 8B 04 4D 78563412", 64, "mov eax,fs:[rcx*2+symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets)]
		[InlineData("64 8B 04 4D 78563412", 64, "mov eax,fs:symbol[rcx*2]", Flags.Symbol)]

		[InlineData("64 8B 84 48 78563412", 64, "mov eax,fs:[rax+rcx*2+symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets)]
		[InlineData("64 8B 84 48 78563412", 64, "mov eax,fs:symbol[rax+rcx*2]", Flags.Symbol)]

		// negative symbols

		[InlineData("66 8B 06 3412", 16, "mov eax,[-symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets | Flags.Signed)]
		[InlineData("66 8B 06 3412", 16, "mov eax,-symbol", Flags.Symbol | Flags.Signed)]

		[InlineData("8B 05 78563412", 32, "mov eax,[-symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets | Flags.Signed)]
		[InlineData("8B 05 78563412", 32, "mov eax,-symbol", Flags.Symbol | Flags.Signed)]

		[InlineData("8B 04 25 78563412", 64, "mov eax,[-symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets | Flags.Signed)]
		[InlineData("8B 04 25 78563412", 64, "mov eax,-symbol", Flags.Symbol | Flags.Signed)]

		[InlineData("8B 05 78563412", 64, "mov eax,[rip-symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets | Flags.Rip | Flags.Signed)]
		[InlineData("8B 05 78563412", 64, "mov eax,-symbol[rip]", Flags.Symbol | Flags.Rip | Flags.Signed)]
		[InlineData("8B 05 78563412", 64, "mov eax,[-symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets | Flags.Signed)]
		[InlineData("8B 05 78563412", 64, "mov eax,-symbol", Flags.Symbol | Flags.Signed)]

		[InlineData("8B 80 78563412", 64, "mov eax,[rax-symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets | Flags.Signed)]
		[InlineData("8B 80 78563412", 64, "mov eax,-symbol[rax]", Flags.Symbol | Flags.Signed)]

		[InlineData("8B 04 4D 78563412", 64, "mov eax,[rcx*2-symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets | Flags.Signed)]
		[InlineData("8B 04 4D 78563412", 64, "mov eax,-symbol[rcx*2]", Flags.Symbol | Flags.Signed)]

		[InlineData("8B 84 48 78563412", 64, "mov eax,[rax+rcx*2-symbol]", Flags.Symbol | Flags.SymbolDisplInBrackets | Flags.Signed)]
		[InlineData("8B 84 48 78563412", 64, "mov eax,-symbol[rax+rcx*2]", Flags.Symbol | Flags.Signed)]

		// no symbols

		[InlineData("66 8B 06 3412", 16, "mov eax,ds:[1234h]", Flags.DisplInBrackets)]
		[InlineData("66 8B 06 3412", 16, "mov eax,ds:1234h", Flags.None)]
		[InlineData("66 8B 06 0000", 16, "mov eax,ds:[0]", Flags.DisplInBrackets)]
		[InlineData("66 8B 06 0000", 16, "mov eax,ds:0", Flags.None)]
		[InlineData("66 8B 06 3412", 16, "mov eax,[1234h]", Flags.DisplInBrackets | Flags.NoAddDsPrefix32)]
		[InlineData("66 8B 06 3412", 16, "mov eax,[1234h]", Flags.NoAddDsPrefix32)]
		[InlineData("66 8B 06 0000", 16, "mov eax,[0]", Flags.DisplInBrackets | Flags.NoAddDsPrefix32)]
		[InlineData("66 8B 06 0000", 16, "mov eax,[0]", Flags.NoAddDsPrefix32)]

		[InlineData("8B 05 78563412", 32, "mov eax,ds:[12345678h]", Flags.DisplInBrackets)]
		[InlineData("8B 05 78563412", 32, "mov eax,ds:12345678h", Flags.None)]
		[InlineData("8B 05 00000000", 32, "mov eax,ds:[0]", Flags.DisplInBrackets)]
		[InlineData("8B 05 00000000", 32, "mov eax,ds:0", Flags.None)]
		[InlineData("8B 05 78563412", 32, "mov eax,[12345678h]", Flags.DisplInBrackets | Flags.NoAddDsPrefix32)]
		[InlineData("8B 05 78563412", 32, "mov eax,[12345678h]", Flags.NoAddDsPrefix32)]
		[InlineData("8B 05 00000000", 32, "mov eax,[0]", Flags.DisplInBrackets | Flags.NoAddDsPrefix32)]
		[InlineData("8B 05 00000000", 32, "mov eax,[0]", Flags.NoAddDsPrefix32)]

		[InlineData("8B 04 25 78563412", 64, "mov eax,[12345678h]", Flags.DisplInBrackets)]
		[InlineData("8B 04 25 78563412", 64, "mov eax,[12345678h]", Flags.None)]
		[InlineData("8B 04 25 00000000", 64, "mov eax,[0]", Flags.DisplInBrackets)]
		[InlineData("8B 04 25 00000000", 64, "mov eax,[0]", Flags.None)]
		[InlineData("8B 04 25 78563412", 64, "mov eax,[12345678h]", Flags.DisplInBrackets | Flags.NoAddDsPrefix32)]
		[InlineData("8B 04 25 78563412", 64, "mov eax,[12345678h]", Flags.NoAddDsPrefix32)]
		[InlineData("8B 04 25 00000000", 64, "mov eax,[0]", Flags.DisplInBrackets | Flags.NoAddDsPrefix32)]
		[InlineData("8B 04 25 00000000", 64, "mov eax,[0]", Flags.NoAddDsPrefix32)]

		[InlineData("8B 05 78563412", 64, "mov eax,[rip+12345678h]", Flags.DisplInBrackets | Flags.Rip)]
		[InlineData("8B 05 78563412", 64, "mov eax,12345678h[rip]", Flags.Rip)]
		[InlineData("8B 05 78563412", 64, "mov eax,[800000001234566Eh]", Flags.DisplInBrackets)]
		[InlineData("8B 05 78563412", 64, "mov eax,[800000001234566Eh]", Flags.None)]
		[InlineData("8B 05 00000000", 64, "mov eax,[rip+0]", Flags.DisplInBrackets | Flags.Rip | Flags.ShowZeroDisplacements)]
		[InlineData("8B 05 00000000", 64, "mov eax,0[rip]", Flags.Rip | Flags.ShowZeroDisplacements)]
		[InlineData("8B 05 00000000", 64, "mov eax,[rip]", Flags.DisplInBrackets | Flags.Rip)]
		[InlineData("8B 05 00000000", 64, "mov eax,[rip]", Flags.Rip)]

		[InlineData("8B 80 78563412", 64, "mov eax,[rax+12345678h]", Flags.DisplInBrackets)]
		[InlineData("8B 80 78563412", 64, "mov eax,12345678h[rax]", Flags.None)]
		[InlineData("8B 80 00000000", 64, "mov eax,[rax+0]", Flags.DisplInBrackets | Flags.ShowZeroDisplacements)]
		[InlineData("8B 80 00000000", 64, "mov eax,0[rax]", Flags.ShowZeroDisplacements)]
		[InlineData("8B 80 00000000", 64, "mov eax,[rax]", Flags.DisplInBrackets)]
		[InlineData("8B 80 00000000", 64, "mov eax,[rax]", Flags.None)]

		[InlineData("8B 04 4D 78563412", 64, "mov eax,[rcx*2+12345678h]", Flags.DisplInBrackets)]
		[InlineData("8B 04 4D 78563412", 64, "mov eax,12345678h[rcx*2]", Flags.None)]
		[InlineData("8B 04 4D 00000000", 64, "mov eax,[rcx*2+0]", Flags.DisplInBrackets | Flags.ShowZeroDisplacements)]
		[InlineData("8B 04 4D 00000000", 64, "mov eax,0[rcx*2]", Flags.ShowZeroDisplacements)]
		[InlineData("8B 04 4D 00000000", 64, "mov eax,[rcx*2]", Flags.DisplInBrackets)]
		[InlineData("8B 04 4D 00000000", 64, "mov eax,[rcx*2]", Flags.None)]

		[InlineData("8B 84 48 78563412", 64, "mov eax,[rax+rcx*2+12345678h]", Flags.DisplInBrackets)]
		[InlineData("8B 84 48 78563412", 64, "mov eax,12345678h[rax+rcx*2]", Flags.None)]
		[InlineData("8B 84 48 00000000", 64, "mov eax,[rax+rcx*2+0]", Flags.DisplInBrackets | Flags.ShowZeroDisplacements)]
		[InlineData("8B 84 48 00000000", 64, "mov eax,0[rax+rcx*2]", Flags.ShowZeroDisplacements)]
		[InlineData("8B 84 48 00000000", 64, "mov eax,[rax+rcx*2]", Flags.DisplInBrackets)]
		[InlineData("8B 84 48 00000000", 64, "mov eax,[rax+rcx*2]", Flags.None)]

		// no symbols + seg override

		[InlineData("64 66 8B 06 3412", 16, "mov eax,fs:[1234h]", Flags.DisplInBrackets)]
		[InlineData("64 66 8B 06 3412", 16, "mov eax,fs:1234h", Flags.None)]
		[InlineData("64 66 8B 06 0000", 16, "mov eax,fs:[0]", Flags.DisplInBrackets)]
		[InlineData("64 66 8B 06 0000", 16, "mov eax,fs:0", Flags.None)]
		[InlineData("64 66 8B 06 3412", 16, "mov eax,fs:[1234h]", Flags.DisplInBrackets | Flags.NoAddDsPrefix32)]
		[InlineData("64 66 8B 06 3412", 16, "mov eax,fs:1234h", Flags.NoAddDsPrefix32)]
		[InlineData("64 66 8B 06 0000", 16, "mov eax,fs:[0]", Flags.DisplInBrackets | Flags.NoAddDsPrefix32)]
		[InlineData("64 66 8B 06 0000", 16, "mov eax,fs:0", Flags.NoAddDsPrefix32)]

		[InlineData("64 8B 05 78563412", 32, "mov eax,fs:[12345678h]", Flags.DisplInBrackets)]
		[InlineData("64 8B 05 78563412", 32, "mov eax,fs:12345678h", Flags.None)]
		[InlineData("64 8B 05 00000000", 32, "mov eax,fs:[0]", Flags.DisplInBrackets)]
		[InlineData("64 8B 05 00000000", 32, "mov eax,fs:0", Flags.None)]
		[InlineData("64 8B 05 78563412", 32, "mov eax,fs:[12345678h]", Flags.DisplInBrackets | Flags.NoAddDsPrefix32)]
		[InlineData("64 8B 05 78563412", 32, "mov eax,fs:12345678h", Flags.NoAddDsPrefix32)]
		[InlineData("64 8B 05 00000000", 32, "mov eax,fs:[0]", Flags.DisplInBrackets | Flags.NoAddDsPrefix32)]
		[InlineData("64 8B 05 00000000", 32, "mov eax,fs:0", Flags.NoAddDsPrefix32)]

		[InlineData("64 8B 04 25 78563412", 64, "mov eax,fs:[12345678h]", Flags.DisplInBrackets)]
		[InlineData("64 8B 04 25 78563412", 64, "mov eax,fs:[12345678h]", Flags.None)]
		[InlineData("64 8B 04 25 00000000", 64, "mov eax,fs:[0]", Flags.DisplInBrackets)]
		[InlineData("64 8B 04 25 00000000", 64, "mov eax,fs:[0]", Flags.None)]
		[InlineData("64 8B 04 25 78563412", 64, "mov eax,fs:[12345678h]", Flags.DisplInBrackets | Flags.NoAddDsPrefix32)]
		[InlineData("64 8B 04 25 78563412", 64, "mov eax,fs:[12345678h]", Flags.NoAddDsPrefix32)]
		[InlineData("64 8B 04 25 00000000", 64, "mov eax,fs:[0]", Flags.DisplInBrackets | Flags.NoAddDsPrefix32)]
		[InlineData("64 8B 04 25 00000000", 64, "mov eax,fs:[0]", Flags.NoAddDsPrefix32)]

		[InlineData("64 8B 05 78563412", 64, "mov eax,fs:[rip+12345678h]", Flags.DisplInBrackets | Flags.Rip)]
		[InlineData("64 8B 05 78563412", 64, "mov eax,fs:12345678h[rip]", Flags.Rip)]
		[InlineData("64 8B 05 78563412", 64, "mov eax,fs:[800000001234566Fh]", Flags.DisplInBrackets)]
		[InlineData("64 8B 05 78563412", 64, "mov eax,fs:[800000001234566Fh]", Flags.None)]
		[InlineData("64 8B 05 00000000", 64, "mov eax,fs:[rip+0]", Flags.DisplInBrackets | Flags.Rip | Flags.ShowZeroDisplacements)]
		[InlineData("64 8B 05 00000000", 64, "mov eax,fs:0[rip]", Flags.Rip | Flags.ShowZeroDisplacements)]
		[InlineData("64 8B 05 00000000", 64, "mov eax,fs:[rip]", Flags.DisplInBrackets | Flags.Rip)]
		[InlineData("64 8B 05 00000000", 64, "mov eax,fs:[rip]", Flags.Rip)]

		[InlineData("64 8B 80 78563412", 64, "mov eax,fs:[rax+12345678h]", Flags.DisplInBrackets)]
		[InlineData("64 8B 80 78563412", 64, "mov eax,fs:12345678h[rax]", Flags.None)]
		[InlineData("64 8B 80 00000000", 64, "mov eax,fs:[rax+0]", Flags.DisplInBrackets | Flags.ShowZeroDisplacements)]
		[InlineData("64 8B 80 00000000", 64, "mov eax,fs:0[rax]", Flags.ShowZeroDisplacements)]
		[InlineData("64 8B 80 00000000", 64, "mov eax,fs:[rax]", Flags.DisplInBrackets)]
		[InlineData("64 8B 80 00000000", 64, "mov eax,fs:[rax]", Flags.None)]

		[InlineData("64 8B 04 4D 78563412", 64, "mov eax,fs:[rcx*2+12345678h]", Flags.DisplInBrackets)]
		[InlineData("64 8B 04 4D 78563412", 64, "mov eax,fs:12345678h[rcx*2]", Flags.None)]
		[InlineData("64 8B 04 4D 00000000", 64, "mov eax,fs:[rcx*2+0]", Flags.DisplInBrackets | Flags.ShowZeroDisplacements)]
		[InlineData("64 8B 04 4D 00000000", 64, "mov eax,fs:0[rcx*2]", Flags.ShowZeroDisplacements)]
		[InlineData("64 8B 04 4D 00000000", 64, "mov eax,fs:[rcx*2]", Flags.DisplInBrackets)]
		[InlineData("64 8B 04 4D 00000000", 64, "mov eax,fs:[rcx*2]", Flags.None)]

		[InlineData("64 8B 84 48 78563412", 64, "mov eax,fs:[rax+rcx*2+12345678h]", Flags.DisplInBrackets)]
		[InlineData("64 8B 84 48 78563412", 64, "mov eax,fs:12345678h[rax+rcx*2]", Flags.None)]
		[InlineData("64 8B 84 48 00000000", 64, "mov eax,fs:[rax+rcx*2+0]", Flags.DisplInBrackets | Flags.ShowZeroDisplacements)]
		[InlineData("64 8B 84 48 00000000", 64, "mov eax,fs:0[rax+rcx*2]", Flags.ShowZeroDisplacements)]
		[InlineData("64 8B 84 48 00000000", 64, "mov eax,fs:[rax+rcx*2]", Flags.DisplInBrackets)]
		[InlineData("64 8B 84 48 00000000", 64, "mov eax,fs:[rax+rcx*2]", Flags.None)]

		void DisplInBrackets(string hexBytes, int bitness, string formattedString, Flags flags) {
			var decoder = Decoder.Create(bitness, new ByteArrayCodeReader(hexBytes));
			switch (bitness) {
			case 16: decoder.IP = DecoderConstants.DEFAULT_IP16; break;
			case 32: decoder.IP = DecoderConstants.DEFAULT_IP32; break;
			case 64: decoder.IP = DecoderConstants.DEFAULT_IP64; break;
			default: throw new InvalidOperationException();
			}
			decoder.Decode(out var instr);

			var resolver = new TestSymbolResolver {
				tryGetSymbol = (int operand, int instructionOperand, in Instruction instruction, ulong address, int addressSize, out SymbolResult symbol) => {
					if (instructionOperand == 1 && (flags & Flags.Symbol) != 0) {
						symbol = new SymbolResult(address, "symbol", FormatterOutputTextKind.Data, (flags & Flags.Signed) != 0 ? SymbolFlags.Signed : SymbolFlags.None);
						return true;
					}
					symbol = default;
					return false;
				},
			};
			var formatter = (MasmFormatter)MasmFormatterFactory.Create_Resolver(resolver).formatter;
			formatter.MasmOptions.SymbolDisplInBrackets = (flags & Flags.SymbolDisplInBrackets) != 0;
			formatter.MasmOptions.DisplInBrackets = (flags & Flags.DisplInBrackets) != 0;
			formatter.MasmOptions.RipRelativeAddresses = (flags & Flags.Rip) != 0;
			formatter.MasmOptions.ShowZeroDisplacements = (flags & Flags.ShowZeroDisplacements) != 0;
			formatter.MasmOptions.AddDsPrefix32 = (flags & Flags.NoAddDsPrefix32) == 0;

			var output = new StringBuilderFormatterOutput();
			formatter.Format(instr, output);
			var actualFormattedString = output.ToStringAndReset();
#pragma warning disable xUnit2006 // Do not use invalid string equality check
			// Show the full string without ellipses by using Equal<string>() instead of Equal()
			Assert.Equal<string>(formattedString, actualFormattedString);
#pragma warning restore xUnit2006 // Do not use invalid string equality check
		}
	}
}
#endif
