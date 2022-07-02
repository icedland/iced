// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import com.github.icedland.iced.x86.fmt.intel.IntelFormatter;

final class IntelFormatterFactory {
	private static IntelFormatter createFormatter(SymbolResolver symbolResolver) {
		IntelFormatter formatter = new IntelFormatter(symbolResolver);
		formatter.getOptions().setUppercaseHex(false);
		formatter.getOptions().setHexPrefix("0x");
		formatter.getOptions().setHexSuffix(null);
		formatter.getOptions().setOctalPrefix("0o");
		formatter.getOptions().setOctalSuffix(null);
		formatter.getOptions().setBinaryPrefix("0b");
		formatter.getOptions().setBinarySuffix(null);
		formatter.getOptions().setSpaceAfterOperandSeparator(true);
		return formatter;
	}

	public static IntelFormatter create_MemDefault() {
		IntelFormatter formatter = createFormatter(null);
		formatter.getOptions().setMemorySizeOptions(MemorySizeOptions.DEFAULT);
		formatter.getOptions().setShowBranchSize(false);
		formatter.getOptions().setRipRelativeAddresses(true);
		formatter.getOptions().setSignedImmediateOperands(false);
		formatter.getOptions().setSpaceAfterOperandSeparator(false);
		return formatter;
	}

	public static IntelFormatter create_MemAlways() {
		IntelFormatter formatter = createFormatter(null);
		formatter.getOptions().setMemorySizeOptions(MemorySizeOptions.ALWAYS);
		formatter.getOptions().setShowBranchSize(true);
		formatter.getOptions().setRipRelativeAddresses(false);
		formatter.getOptions().setSignedImmediateOperands(true);
		formatter.getOptions().setSpaceAfterOperandSeparator(true);
		return formatter;
	}

	public static IntelFormatter create_MemMinimum() {
		IntelFormatter formatter = createFormatter(null);
		formatter.getOptions().setMemorySizeOptions(MemorySizeOptions.MINIMAL);
		formatter.getOptions().setShowBranchSize(true);
		formatter.getOptions().setRipRelativeAddresses(false);
		formatter.getOptions().setSignedImmediateOperands(true);
		formatter.getOptions().setSpaceAfterOperandSeparator(true);
		return formatter;
	}

	public static IntelFormatter create() {
		IntelFormatter formatter = createFormatter(null);
		formatter.getOptions().setMemorySizeOptions(MemorySizeOptions.ALWAYS);
		formatter.getOptions().setShowBranchSize(true);
		formatter.getOptions().setRipRelativeAddresses(false);
		return formatter;
	}

	public static IntelFormatter create_Options() {
		IntelFormatter formatter = createFormatter(null);
		formatter.getOptions().setMemorySizeOptions(MemorySizeOptions.DEFAULT);
		formatter.getOptions().setShowBranchSize(false);
		formatter.getOptions().setRipRelativeAddresses(true);
		formatter.getOptions().setSpaceAfterOperandSeparator(false);
		return formatter;
	}

	public static IntelFormatter create_Registers() {
		IntelFormatter formatter = createFormatter(null);
		return formatter;
	}

	public static IntelFormatter create_Numbers() {
		IntelFormatter formatter = createFormatter(null);
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
		IntelFormatter formatter = createFormatter(symbolResolver);
		formatter.getOptions().setMemorySizeOptions(MemorySizeOptions.DEFAULT);
		formatter.getOptions().setShowBranchSize(false);
		formatter.getOptions().setRipRelativeAddresses(true);
		formatter.getOptions().setSpaceAfterOperandSeparator(false);
		return new SymbolResolverTests.Info(formatter, symbolResolver);
	}
}
