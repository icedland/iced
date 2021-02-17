// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests.Gas {
	static class FormatterFactory {
		static FormatterOptions CreateOptions() {
			var options = FormatterOptions.CreateGas();
			options.UppercaseHex = false;
			return options;
		}

		public static GasFormatter Create_NoSuffix() {
			var options = CreateOptions();
			options.GasShowMnemonicSizeSuffix = false;
			options.GasNakedRegisters = false;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			options.SignedImmediateOperands = false;
			options.SpaceAfterOperandSeparator = false;
			options.GasSpaceAfterMemoryOperandComma = true;
			return new GasFormatter(options);
		}

		public static GasFormatter Create_ForceSuffix() {
			var options = CreateOptions();
			options.GasShowMnemonicSizeSuffix = true;
			options.GasNakedRegisters = true;
			options.ShowBranchSize = true;
			options.RipRelativeAddresses = false;
			options.SignedImmediateOperands = true;
			options.SpaceAfterOperandSeparator = true;
			options.GasSpaceAfterMemoryOperandComma = false;
			return new GasFormatter(options);
		}

		public static GasFormatter Create() {
			var options = CreateOptions();
			options.GasShowMnemonicSizeSuffix = false;
			options.GasNakedRegisters = false;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			return new GasFormatter(options);
		}

		public static GasFormatter Create_Options() {
			var options = CreateOptions();
			options.GasShowMnemonicSizeSuffix = false;
			options.GasNakedRegisters = false;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			return new GasFormatter(options);
		}

		public static GasFormatter Create_Registers(bool nakedRegisters) {
			var options = CreateOptions();
			options.GasNakedRegisters = nakedRegisters;
			return new GasFormatter(options);
		}

		public static GasFormatter Create_Numbers() {
			var options = CreateOptions();
			options.UppercaseHex = true;
			options.HexPrefix = null;
			options.HexSuffix = null;
			options.OctalPrefix = null;
			options.OctalSuffix = null;
			options.BinaryPrefix = null;
			options.BinarySuffix = null;
			return new GasFormatter(options);
		}

		public static (Formatter formatter, ISymbolResolver symbolResolver) Create_Resolver(ISymbolResolver symbolResolver) {
			var options = CreateOptions();
			options.GasShowMnemonicSizeSuffix = false;
			options.GasNakedRegisters = false;
			options.ShowBranchSize = false;
			options.RipRelativeAddresses = true;
			return (new GasFormatter(options, symbolResolver), symbolResolver);
		}
	}
}
#endif
