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

#if GAS || INTEL || MASM || NASM || FAST_FMT
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
}
#endif
