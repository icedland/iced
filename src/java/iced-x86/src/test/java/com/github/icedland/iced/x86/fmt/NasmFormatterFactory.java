// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import com.github.icedland.iced.x86.fmt.nasm.NasmFormatter;

final class NasmFormatterFactory {
	private static NasmFormatter createFormatter(SymbolResolver symbolResolver) {
		NasmFormatter formatter = new NasmFormatter(symbolResolver);
		formatter.getOptions().setUppercaseHex(false);
		formatter.getOptions().setHexPrefix("0x");
		formatter.getOptions().setHexSuffix(null);
		formatter.getOptions().setOctalPrefix("0o");
		formatter.getOptions().setOctalSuffix(null);
		formatter.getOptions().setBinaryPrefix("0b");
		formatter.getOptions().setBinarySuffix(null);
		return formatter;
	}

	public static NasmFormatter create_MemDefault() {
		NasmFormatter formatter = createFormatter(null);
		formatter.getOptions().setMemorySizeOptions(MemorySizeOptions.DEFAULT);
		formatter.getOptions().setNasmShowSignExtendedImmediateSize(true);
		formatter.getOptions().setShowBranchSize(false);
		formatter.getOptions().setRipRelativeAddresses(true);
		formatter.getOptions().setSignedImmediateOperands(false);
		formatter.getOptions().setSpaceAfterOperandSeparator(false);
		return formatter;
	}

	public static NasmFormatter create_MemAlways() {
		NasmFormatter formatter = createFormatter(null);
		formatter.getOptions().setMemorySizeOptions(MemorySizeOptions.ALWAYS);
		formatter.getOptions().setNasmShowSignExtendedImmediateSize(true);
		formatter.getOptions().setShowBranchSize(true);
		formatter.getOptions().setRipRelativeAddresses(false);
		formatter.getOptions().setSignedImmediateOperands(true);
		formatter.getOptions().setSpaceAfterOperandSeparator(true);
		return formatter;
	}

	public static NasmFormatter create_MemMinimum() {
		NasmFormatter formatter = createFormatter(null);
		formatter.getOptions().setMemorySizeOptions(MemorySizeOptions.MINIMAL);
		formatter.getOptions().setNasmShowSignExtendedImmediateSize(true);
		formatter.getOptions().setShowBranchSize(true);
		formatter.getOptions().setRipRelativeAddresses(false);
		formatter.getOptions().setSignedImmediateOperands(true);
		formatter.getOptions().setSpaceAfterOperandSeparator(true);
		return formatter;
	}

	public static NasmFormatter create() {
		NasmFormatter formatter = createFormatter(null);
		formatter.getOptions().setMemorySizeOptions(MemorySizeOptions.DEFAULT);
		formatter.getOptions().setNasmShowSignExtendedImmediateSize(true);
		formatter.getOptions().setShowBranchSize(false);
		formatter.getOptions().setRipRelativeAddresses(true);
		return formatter;
	}

	public static NasmFormatter create_Options() {
		NasmFormatter formatter = createFormatter(null);
		formatter.getOptions().setMemorySizeOptions(MemorySizeOptions.DEFAULT);
		formatter.getOptions().setNasmShowSignExtendedImmediateSize(false);
		formatter.getOptions().setShowBranchSize(false);
		formatter.getOptions().setRipRelativeAddresses(true);
		return formatter;
	}

	public static NasmFormatter create_Registers() {
		NasmFormatter formatter = createFormatter(null);
		return formatter;
	}

	public static NasmFormatter create_Numbers() {
		NasmFormatter formatter = createFormatter(null);
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
		NasmFormatter formatter = createFormatter(symbolResolver);
		formatter.getOptions().setMemorySizeOptions(MemorySizeOptions.DEFAULT);
		formatter.getOptions().setNasmShowSignExtendedImmediateSize(false);
		formatter.getOptions().setShowBranchSize(false);
		formatter.getOptions().setRipRelativeAddresses(true);
		return new SymbolResolverTests.Info(formatter, symbolResolver);
	}
}
