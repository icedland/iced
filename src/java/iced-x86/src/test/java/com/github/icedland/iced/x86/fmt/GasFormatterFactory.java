// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import com.github.icedland.iced.x86.fmt.gas.GasFormatter;

final class GasFormatterFactory {
	private static GasFormatter createFormatter(SymbolResolver symbolResolver) {
		GasFormatter formatter = new GasFormatter(symbolResolver);
		formatter.getOptions().setUppercaseHex(false);
		return formatter;
	}

	public static GasFormatter create_NoSuffix() {
		GasFormatter formatter = createFormatter(null);
		formatter.getOptions().setGasShowMnemonicSizeSuffix(false);
		formatter.getOptions().setGasNakedRegisters(false);
		formatter.getOptions().setShowBranchSize(false);
		formatter.getOptions().setRipRelativeAddresses(true);
		formatter.getOptions().setSignedImmediateOperands(false);
		formatter.getOptions().setSpaceAfterOperandSeparator(false);
		formatter.getOptions().setGasSpaceAfterMemoryOperandComma(true);
		return formatter;
	}

	public static GasFormatter create_ForceSuffix() {
		GasFormatter formatter = createFormatter(null);
		formatter.getOptions().setGasShowMnemonicSizeSuffix(true);
		formatter.getOptions().setGasNakedRegisters(true);
		formatter.getOptions().setShowBranchSize(true);
		formatter.getOptions().setRipRelativeAddresses(false);
		formatter.getOptions().setSignedImmediateOperands(true);
		formatter.getOptions().setSpaceAfterOperandSeparator(true);
		formatter.getOptions().setGasSpaceAfterMemoryOperandComma(false);
		return formatter;
	}

	public static GasFormatter create() {
		GasFormatter formatter = createFormatter(null);
		formatter.getOptions().setGasShowMnemonicSizeSuffix(false);
		formatter.getOptions().setGasNakedRegisters(false);
		formatter.getOptions().setShowBranchSize(false);
		formatter.getOptions().setRipRelativeAddresses(true);
		return formatter;
	}

	public static GasFormatter create_Options() {
		GasFormatter formatter = createFormatter(null);
		formatter.getOptions().setGasShowMnemonicSizeSuffix(false);
		formatter.getOptions().setGasNakedRegisters(false);
		formatter.getOptions().setShowBranchSize(false);
		formatter.getOptions().setRipRelativeAddresses(true);
		return formatter;
	}

	public static GasFormatter create_Registers(boolean nakedRegisters) {
		GasFormatter formatter = createFormatter(null);
		formatter.getOptions().setGasNakedRegisters(nakedRegisters);
		return formatter;
	}

	public static GasFormatter create_Numbers() {
		GasFormatter formatter = createFormatter(null);
		formatter.getOptions().setUppercaseHex(true);
		formatter.getOptions().setHexPrefix(null);
		formatter.getOptions().setHexSuffix(null);
		formatter.getOptions().setOctalPrefix(null);
		formatter.getOptions().setOctalSuffix(null);
		formatter.getOptions().setBinaryPrefix(null);
		formatter.getOptions().setBinarySuffix(null);
		return formatter;
	}

	public static SymbolResolverTests.Info create_Resolver(SymbolResolver symbolResolver) {
		GasFormatter formatter = createFormatter(symbolResolver);
		formatter.getOptions().setGasShowMnemonicSizeSuffix(false);
		formatter.getOptions().setGasNakedRegisters(false);
		formatter.getOptions().setShowBranchSize(false);
		formatter.getOptions().setRipRelativeAddresses(true);
		return new SymbolResolverTests.Info(formatter, symbolResolver);
	}
}
