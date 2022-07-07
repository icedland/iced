// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import com.github.icedland.iced.x86.fmt.masm.MasmFormatter;

public final class MasmFormatterFactory {
	private static MasmFormatter createFormatter(SymbolResolver symbolResolver) {
		return new MasmFormatter(symbolResolver);
	}

	public static MasmFormatter create_MemDefault() {
		MasmFormatter formatter = createFormatter(null);
		formatter.getOptions().setMemorySizeOptions(MemorySizeOptions.DEFAULT);
		formatter.getOptions().setShowBranchSize(false);
		formatter.getOptions().setRipRelativeAddresses(true);
		formatter.getOptions().setSignedImmediateOperands(false);
		formatter.getOptions().setSpaceAfterOperandSeparator(false);
		formatter.getOptions().setMasmAddDsPrefix32(true);
		return formatter;
	}

	public static MasmFormatter create_MemAlways() {
		MasmFormatter formatter = createFormatter(null);
		formatter.getOptions().setMemorySizeOptions(MemorySizeOptions.ALWAYS);
		formatter.getOptions().setShowBranchSize(true);
		formatter.getOptions().setRipRelativeAddresses(false);
		formatter.getOptions().setSignedImmediateOperands(true);
		formatter.getOptions().setSpaceAfterOperandSeparator(true);
		formatter.getOptions().setMasmAddDsPrefix32(true);
		return formatter;
	}

	public static MasmFormatter create_MemMinimum() {
		MasmFormatter formatter = createFormatter(null);
		formatter.getOptions().setMemorySizeOptions(MemorySizeOptions.MINIMAL);
		formatter.getOptions().setShowBranchSize(true);
		formatter.getOptions().setRipRelativeAddresses(false);
		formatter.getOptions().setSignedImmediateOperands(true);
		formatter.getOptions().setSpaceAfterOperandSeparator(true);
		formatter.getOptions().setMasmAddDsPrefix32(false);
		return formatter;
	}

	public static MasmFormatter create() {
		MasmFormatter formatter = createFormatter(null);
		formatter.getOptions().setMemorySizeOptions(MemorySizeOptions.ALWAYS);
		formatter.getOptions().setShowBranchSize(true);
		formatter.getOptions().setRipRelativeAddresses(false);
		return formatter;
	}

	public static MasmFormatter create_Options() {
		MasmFormatter formatter = createFormatter(null);
		formatter.getOptions().setMemorySizeOptions(MemorySizeOptions.DEFAULT);
		formatter.getOptions().setShowBranchSize(false);
		formatter.getOptions().setRipRelativeAddresses(true);
		return formatter;
	}

	public static MasmFormatter create_Registers() {
		MasmFormatter formatter = createFormatter(null);
		return formatter;
	}

	public static MasmFormatter create_Numbers() {
		MasmFormatter formatter = createFormatter(null);
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
		MasmFormatter formatter = createFormatter(symbolResolver);
		formatter.getOptions().setMemorySizeOptions(MemorySizeOptions.DEFAULT);
		formatter.getOptions().setShowBranchSize(false);
		formatter.getOptions().setRipRelativeAddresses(true);
		return new SymbolResolverTests.Info(formatter, symbolResolver);
	}
}
