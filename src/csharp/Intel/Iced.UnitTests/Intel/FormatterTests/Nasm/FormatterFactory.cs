// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if NASM
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests.Nasm {
	static class FormatterFactory {
		static FormatterOptions CreateOptions() {
			var options = FormatterOptions.CreateNasm();
			options.UppercaseHex = false;
			options.HexPrefix = "0x";
			options.HexSuffix = null;
			options.OctalPrefix = "0o";
			options.OctalSuffix = null;
			options.BinaryPrefix = "0b";
			options.BinarySuffix = null;
			return options;
		}

		public static NasmFormatter Create_MemDefault() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Default;
			options.NasmShowSignExtendedImmediateSize = true;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			options.SignedImmediateOperands = false;
			options.SpaceAfterOperandSeparator = false;
			return new NasmFormatter(options);
		}

		public static NasmFormatter Create_MemAlways() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Always;
			options.NasmShowSignExtendedImmediateSize = true;
			options.ShowBranchSize = true;
			options.RipRelativeAddresses = false;
			options.SignedImmediateOperands = true;
			options.SpaceAfterOperandSeparator = true;
			return new NasmFormatter(options);
		}

		public static NasmFormatter Create_MemMinimum() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Minimal;
			options.NasmShowSignExtendedImmediateSize = true;
			options.ShowBranchSize = true;
			options.RipRelativeAddresses = false;
			options.SignedImmediateOperands = true;
			options.SpaceAfterOperandSeparator = true;
			return new NasmFormatter(options);
		}

		public static NasmFormatter Create() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Default;
			options.NasmShowSignExtendedImmediateSize = true;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			return new NasmFormatter(options);
		}

		public static NasmFormatter Create_Options() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Default;
			options.NasmShowSignExtendedImmediateSize = false;
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
			options.UppercaseHex = true;
			options.HexPrefix = null;
			options.HexSuffix = null;
			options.OctalPrefix = null;
			options.OctalSuffix = null;
			options.BinaryPrefix = null;
			options.BinarySuffix = null;
			return new NasmFormatter(options);
		}

		public static (Formatter formatter, ISymbolResolver symbolResolver) Create_Resolver(ISymbolResolver symbolResolver) {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Default;
			options.NasmShowSignExtendedImmediateSize = false;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			return (new NasmFormatter(options, symbolResolver), symbolResolver);
		}
	}
}
#endif
