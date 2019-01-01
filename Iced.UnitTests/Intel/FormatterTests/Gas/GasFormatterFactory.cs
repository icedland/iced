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

#if !NO_GAS_FORMATTER && !NO_FORMATTER
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests.Gas {
	static class GasFormatterFactory {
		static GasFormatterOptions CreateOptions() => new GasFormatterOptions { UpperCaseHex = false };

		public static GasFormatter Create_NoSuffix() {
			var options = CreateOptions();
			options.ShowMnemonicSizeSuffix = false;
			options.NakedRegisters = false;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			options.SignedImmediateOperands = false;
			return new GasFormatter(options);
		}

		public static GasFormatter Create_ForceSuffix() {
			var options = CreateOptions();
			options.ShowMnemonicSizeSuffix = true;
			options.NakedRegisters = true;
			options.ShowBranchSize = true;
			options.RipRelativeAddresses = false;
			options.SignedImmediateOperands = true;
			return new GasFormatter(options);
		}

		public static GasFormatter Create() {
			var options = CreateOptions();
			options.ShowMnemonicSizeSuffix = false;
			options.NakedRegisters = false;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			return new GasFormatter(options);
		}

		public static GasFormatter Create_Options() {
			var options = CreateOptions();
			options.ShowMnemonicSizeSuffix = false;
			options.NakedRegisters = false;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			return new GasFormatter(options);
		}

		public static (Formatter formatter, ISymbolResolver symbolResolver) Create_Resolver(ISymbolResolver symbolResolver) {
			var options = CreateOptions();
			options.ShowMnemonicSizeSuffix = false;
			options.NakedRegisters = false;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			return (new GasFormatter(options, symbolResolver), symbolResolver);
		}

		public static GasFormatter Create_Registers(bool nakedRegisters) {
			var options = CreateOptions();
			options.NakedRegisters = nakedRegisters;
			return new GasFormatter(options);
		}

		public static GasFormatter Create_Numbers() {
			var options = CreateOptions();
			return new GasFormatter(options);
		}
	}
}
#endif
