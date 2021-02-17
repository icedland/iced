// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if INTEL
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests.Intel {
	static class FormatterFactory {
		static FormatterOptions CreateOptions() {
			var options = FormatterOptions.CreateIntel();
			options.UppercaseHex = false;
			options.HexPrefix = "0x";
			options.HexSuffix = null;
			options.OctalPrefix = "0o";
			options.OctalSuffix = null;
			options.BinaryPrefix = "0b";
			options.BinarySuffix = null;
			options.SpaceAfterOperandSeparator = true;
			return options;
		}

		public static IntelFormatter Create_MemDefault() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Default;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			options.SignedImmediateOperands = false;
			options.SpaceAfterOperandSeparator = false;
			return new IntelFormatter(options);
		}

		public static IntelFormatter Create_MemAlways() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Always;
			options.ShowBranchSize = true;
			options.RipRelativeAddresses = false;
			options.SignedImmediateOperands = true;
			options.SpaceAfterOperandSeparator = true;
			return new IntelFormatter(options);
		}

		public static IntelFormatter Create_MemMinimum() {
			var options = CreateOptions();
			options.MemorySizeOptions = MemorySizeOptions.Minimal;
			options.ShowBranchSize = true;
			options.RipRelativeAddresses = false;
			options.SignedImmediateOperands = true;
			options.SpaceAfterOperandSeparator = true;
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
			options.UppercaseHex = true;
			options.HexPrefix = null;
			options.HexSuffix = null;
			options.OctalPrefix = null;
			options.OctalSuffix = null;
			options.BinaryPrefix = null;
			options.BinarySuffix = null;
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
