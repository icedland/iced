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

#if FAST_FMT
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests.Fast {
	static class FormatterFactory {
		public static FastFormatter Create_Default() => new FastFormatter();

		public static FastFormatter Create_Inverted() {
			var fast = new FastFormatter();
			fast.Options.SpaceAfterOperandSeparator ^= true;
			fast.Options.RipRelativeAddresses ^= true;
			fast.Options.UsePseudoOps ^= true;
			fast.Options.ShowSymbolAddress ^= true;
			fast.Options.AlwaysShowSegmentRegister ^= true;
			fast.Options.AlwaysShowMemorySize ^= true;
			fast.Options.UppercaseHex ^= true;
			fast.Options.UseHexPrefix ^= true;
			return fast;
		}

		public static FastFormatter Create_Options() {
			var fast = new FastFormatter();
			fast.Options.RipRelativeAddresses = true;
			return fast;
		}

		public static (FastFormatter formatter, ISymbolResolver symbolResolver) Create_Resolver(ISymbolResolver symbolResolver) {
			var fast = new FastFormatter(symbolResolver);
			fast.Options.RipRelativeAddresses = true;
			return (fast, symbolResolver);
		}
	}
}
#endif
