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

#if !NO_MASM
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests.Masm {
	static class FormatterFactory {
		static FormatterOptions CreateOptions() => FormatterOptions.CreateMasm();

		public static MasmFormatter Create_MemDefault() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Default;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			options.SignedImmediateOperands = false;
			options.SpaceAfterOperandSeparator = false;
			options.MasmAddDsPrefix32 = true;
			return new MasmFormatter(options);
		}

		public static MasmFormatter Create_MemAlways() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Always;
			options.ShowBranchSize = true;
			options.RipRelativeAddresses = false;
			options.SignedImmediateOperands = true;
			options.SpaceAfterOperandSeparator = true;
			options.MasmAddDsPrefix32 = true;
			return new MasmFormatter(options);
		}

		public static MasmFormatter Create_MemMinimum() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Minimum;
			options.ShowBranchSize = true;
			options.RipRelativeAddresses = false;
			options.SignedImmediateOperands = true;
			options.SpaceAfterOperandSeparator = true;
			options.MasmAddDsPrefix32 = false;
			return new MasmFormatter(options);
		}

		public static MasmFormatter Create() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Always;
			options.ShowBranchSize = true;
			options.RipRelativeAddresses = false;
			return new MasmFormatter(options);
		}

		public static MasmFormatter Create_Options() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Default;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			return new MasmFormatter(options);
		}

		public static MasmFormatter Create_Registers() {
			var options = CreateOptions();
			return new MasmFormatter(options);
		}

		public static MasmFormatter Create_Numbers() {
			var options = CreateOptions();
			options.UpperCaseHex = true;
			options.HexPrefix = null;
			options.HexSuffix = null;
			options.OctalPrefix = null;
			options.OctalSuffix = null;
			options.BinaryPrefix = null;
			options.BinarySuffix = null;
			return new MasmFormatter(options);
		}

		public static (Formatter formatter, ISymbolResolver symbolResolver) Create_Resolver(ISymbolResolver symbolResolver) {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Default;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			return (new MasmFormatter(options, symbolResolver), symbolResolver);
		}
	}
}
#endif
