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
