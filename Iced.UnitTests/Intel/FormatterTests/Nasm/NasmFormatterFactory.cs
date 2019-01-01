/*
    Copyright (C) 2018-2019 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

#if !NO_NASM_FORMATTER && !NO_FORMATTER
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests.Nasm {
	static class NasmFormatterFactory {
		static NasmFormatterOptions CreateOptions() => new NasmFormatterOptions {
			UpperCaseHex = false,
			HexPrefix = "0x",
			HexSuffix = null,
			OctalPrefix = "0o",
			OctalSuffix = null,
			BinaryPrefix = "0b",
			BinarySuffix = null,
		};

		public static NasmFormatter Create_NoMemSize() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Default;
			options.ShowSignExtendedImmediateSize = true;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			options.SignedImmediateOperands = false;
			return new NasmFormatter(options);
		}

		public static NasmFormatter Create_ForceMemSize() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Always;
			options.ShowSignExtendedImmediateSize = true;
			options.ShowBranchSize = true;
			options.RipRelativeAddresses = false;
			options.SignedImmediateOperands = true;
			return new NasmFormatter(options);
		}

		public static NasmFormatter Create() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Default;
			options.ShowSignExtendedImmediateSize = true;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			return new NasmFormatter(options);
		}

		public static NasmFormatter Create_Options() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Default;
			options.ShowSignExtendedImmediateSize = false;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			return new NasmFormatter(options);
		}

		public static NasmFormatter Create_Registers() {
			var options = CreateOptions();
			return new NasmFormatter(options);
		}

		public static NasmFormatter Create_Numbers() {
			var options = CreateOptions();
			return new NasmFormatter(options);
		}

		public static (Formatter formatter, ISymbolResolver symbolResolver) Create_Resolver(ISymbolResolver symbolResolver) {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Default;
			options.ShowSignExtendedImmediateSize = false;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			return (new NasmFormatter(options, symbolResolver), symbolResolver);
		}
	}
}
#endif
