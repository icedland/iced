// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if MASM
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
			options.MemorySizeOptions = MemorySizeOptions.Minimal;
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
			options.UppercaseHex = true;
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
