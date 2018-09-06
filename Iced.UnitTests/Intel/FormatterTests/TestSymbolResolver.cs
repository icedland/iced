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

#if (!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER
using Iced.Intel;

namespace Iced.UnitTests.Intel.FormatterTests {
	sealed class TestSymbolResolver : SymbolResolver {
		public delegate bool TryGetBranchSymbolDelegate(ulong address, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options);
		public TryGetBranchSymbolDelegate tryGetBranchSymbol;
		public override bool TryGetBranchSymbol(int operand, Code code, ulong address, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) {
			if (tryGetBranchSymbol != null)
				return tryGetBranchSymbol(address, out symbol, ref showBranchSize, ref options);
			return base.TryGetBranchSymbol(operand, code, address, out symbol, ref showBranchSize, ref options);
		}

		public delegate bool TryGetFarBranchSymbolDelegate(ushort selector, uint address, out SymbolResult symbolSelector, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options);
		public TryGetFarBranchSymbolDelegate tryGetFarBranchSymbol;
		public override bool TryGetFarBranchSymbol(int operand, Code code, ushort selector, uint address, out SymbolResult symbolSelector, out SymbolResult symbol, ref bool showBranchSize, ref NumberFormattingOptions options) {
			if (tryGetFarBranchSymbol != null)
				return tryGetFarBranchSymbol(selector, address, out symbolSelector, out symbol, ref showBranchSize, ref options);
			return base.TryGetFarBranchSymbol(operand, code, selector, address, out symbolSelector, out symbol, ref showBranchSize, ref options);
		}

		public delegate bool TryGetImmediateSymbolDelegate(ulong immediate, out SymbolResult symbol, ref NumberFormattingOptions options);
		public TryGetImmediateSymbolDelegate tryGetImmediateSymbol;
		public override bool TryGetImmediateSymbol(int operand, Code code, ulong immediate, out SymbolResult symbol, ref NumberFormattingOptions options) {
			if (tryGetImmediateSymbol != null)
				return tryGetImmediateSymbol(immediate, out symbol, ref options);
			return base.TryGetImmediateSymbol(operand, code, immediate, out symbol, ref options);
		}

		public delegate bool TryGetDisplSymbolDelegate(ulong displacement, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options);
		public TryGetDisplSymbolDelegate tryGetDisplSymbol;
		public override bool TryGetDisplSymbol(int operand, Code code, ulong displacement, ref bool ripRelativeAddresses, out SymbolResult symbol, ref NumberFormattingOptions options) {
			if (tryGetDisplSymbol != null)
				return tryGetDisplSymbol(displacement, ref ripRelativeAddresses, out symbol, ref options);
			return base.TryGetDisplSymbol(operand, code, displacement, ref ripRelativeAddresses, out symbol, ref options);
		}
	}
}
#endif
