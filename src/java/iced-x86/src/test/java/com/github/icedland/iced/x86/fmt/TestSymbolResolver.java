// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import com.github.icedland.iced.x86.Instruction;

final class TestSymbolResolver implements SymbolResolver {
	final SymbolResolverTestCase tc;

	public TestSymbolResolver(SymbolResolverTestCase tc) {
		this.tc = tc;
	}

	public SymbolResult getSymbol(Instruction instruction, int operand, int instructionOperand, long address, int addressSize) {
		for (SymbolResultTestCase tc : this.tc.symbolResults) {
			if (tc.address != address || tc.addressSize != addressSize)
				continue;
			TextPart[] parts = new TextPart[tc.symbolParts.length];
			for (int i = 0; i < parts.length; i++)
				parts[i] = new TextPart(tc.symbolParts[i], FormatterTextKind.TEXT);
			TextInfo text = new TextInfo(parts);
			if (tc.memorySize != null)
				return new SymbolResult(tc.symbolAddress, text, tc.flags, tc.memorySize);
			return new SymbolResult(tc.symbolAddress, text, tc.flags, true);
		}
		return null;
	}
}
