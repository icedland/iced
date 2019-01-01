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

#if !NO_INTEL_FORMATTER && !NO_FORMATTER
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests.Intel {
	static class IntelFormatterFactory {
		static IntelFormatterOptions CreateOptions() => new IntelFormatterOptions {
			UpperCaseHex = false,
			HexPrefix = "0x",
			HexSuffix = null,
			OctalPrefix = "0o",
			OctalSuffix = null,
			BinaryPrefix = "0b",
			BinarySuffix = null,
			SpaceAfterOperandSeparator = true,
		};

		public static IntelFormatter Create_NoMemSize() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Default;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			options.SignedImmediateOperands = false;
			return new IntelFormatter(options);
		}

		public static IntelFormatter Create_ForceMemSize() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Always;
			options.ShowBranchSize = true;
			options.RipRelativeAddresses = false;
			options.SignedImmediateOperands = true;
			return new IntelFormatter(options);
		}

		public static IntelFormatter Create() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Always;
			options.ShowBranchSize = true;
			options.RipRelativeAddresses = false;
			return new IntelFormatter(options);
		}

		public static IntelFormatter Create_Options() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Default;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			options.SpaceAfterOperandSeparator = false;
			return new IntelFormatter(options);
		}

		public static IntelFormatter Create_Registers() {
			var options = CreateOptions();
			return new IntelFormatter(options);
		}

		public static IntelFormatter Create_Numbers() {
			var options = CreateOptions();
			return new IntelFormatter(options);
		}

		public static (Formatter formatter, ISymbolResolver symbolResolver) Create_Resolver(ISymbolResolver symbolResolver) {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Default;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			options.SpaceAfterOperandSeparator = false;
			return (new IntelFormatter(options, symbolResolver), symbolResolver);
		}
	}
}
#endif
