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

#if GAS || INTEL || MASM || NASM
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests {
	public readonly struct SymbolResolverTestCase {
		internal readonly int Bitness;
		internal readonly string HexBytes;
		internal readonly Code Code;
		internal readonly (OptionsProps property, object value)[] Options;
		internal readonly SymbolResultTestCase[] SymbolResults;
		internal SymbolResolverTestCase(int bitness, string hexBytes, Code code, (OptionsProps property, object value)[] options, SymbolResultTestCase[] symbolResults) {
			Bitness = bitness;
			HexBytes = hexBytes;
			Code = code;
			Options = options;
			SymbolResults = symbolResults;
		}
	}

	readonly struct SymbolResultTestCase {
		public readonly ulong Address;
		public readonly ulong SymbolAddress;
		public readonly int AddressSize;
		public readonly SymbolFlags Flags;
		public readonly MemorySize? MemorySize;
		public readonly string[] SymbolParts;
		public SymbolResultTestCase(ulong address, ulong symbolAddress, int addressSize, SymbolFlags flags, MemorySize? memorySize, string[] symbolParts) {
			Address = address;
			SymbolAddress = symbolAddress;
			AddressSize = addressSize;
			Flags = flags;
			MemorySize = memorySize;
			SymbolParts = symbolParts;
		}
	}

	public abstract class SymbolResolverTests {
		protected static IEnumerable<object[]> GetFormatData(string formatterDir, string formattedStringsFile) {
			var infos = SymbolResolverTestInfos.AllInfos;
			var formattedStrings = FileUtils.ReadRawStrings(Path.Combine(formatterDir, formattedStringsFile)).ToArray();
			if (infos.Length != formattedStrings.Length)
				throw new ArgumentException($"(infos.Length) {infos.Length} != (formattedStrings.Length) {formattedStrings.Length} . infos[0].HexBytes = {(infos.Length == 0 ? "<EMPTY>" : infos[0].HexBytes)} & formattedStrings[0] = {(formattedStrings.Length == 0 ? "<EMPTY>" : formattedStrings[0])}");
			var res = new object[infos.Length][];
			for (int i = 0; i < infos.Length; i++)
				res[i] = new object[3] { i, infos[i], formattedStrings[i] };
			return res;
		}

		protected void FormatBase(int index, in SymbolResolverTestCase info, string formattedString, (Formatter formatter, ISymbolResolver symbolResolver) formatterInfo) {
			var infoCopy = info;
			var formatter = formatterInfo.formatter;
			OptionsPropsUtils.Initialize(formatter.Options, infoCopy.Options);
			FormatterTestUtils.SimpleFormatTest(infoCopy.Bitness, infoCopy.HexBytes, infoCopy.Code, DecoderOptions.None, formattedString,
				formatter, decoder => OptionsPropsUtils.Initialize(decoder, infoCopy.Options));
		}
	}

	sealed class TestSymbolResolver : ISymbolResolver {
		readonly SymbolResolverTestCase info;

		public TestSymbolResolver(in SymbolResolverTestCase info) => this.info = info;

		public bool TryGetSymbol(in Instruction instruction, int operand, int instructionOperand, ulong address, int addressSize, out SymbolResult symbol) {
			foreach (var tc in info.SymbolResults) {
				if (tc.Address != address || tc.AddressSize != addressSize)
					continue;
				var text = new TextInfo(tc.SymbolParts.Select(a => new TextPart(a, FormatterTextKind.Text)).ToArray());
				if (tc.MemorySize != null)
					symbol = new SymbolResult(tc.SymbolAddress, text, tc.Flags, tc.MemorySize.Value);
				else
					symbol = new SymbolResult(tc.SymbolAddress, text, tc.Flags);
				return true;
			}
			symbol = default;
			return false;
		}
	}
}
#endif
