/*
    Copyright (C) 2018 de4dot@gmail.com

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

#if !NO_MASM_FORMATTER && !NO_FORMATTER
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests.Masm {
	static class MasmFormatterFactory {
		static MasmFormatterOptions CreateOptions() => new MasmFormatterOptions { UpperCaseHex = true };

		public static MasmFormatter Create_NoMemSize() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Default;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			options.SignedImmediateOperands = false;
			return new MasmFormatter(options);
		}

		public static MasmFormatter Create_ForceMemSize() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Always;
			options.ShowBranchSize = true;
			options.RipRelativeAddresses = false;
			options.SignedImmediateOperands = true;
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

		public static MasmFormatter Create_Resolver(SymbolResolver symbolResolver) {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Default;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			return new MasmFormatter(options, symbolResolver);
		}
	}
}
#endif
