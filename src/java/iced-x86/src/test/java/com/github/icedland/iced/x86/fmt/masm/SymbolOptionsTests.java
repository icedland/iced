// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt.masm;

import static org.junit.jupiter.api.Assertions.*;

import java.util.ArrayList;

import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;

import com.github.icedland.iced.x86.HexUtils;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.PathUtils;
import com.github.icedland.iced.x86.dec.ByteArrayCodeReader;
import com.github.icedland.iced.x86.dec.Decoder;
import com.github.icedland.iced.x86.fmt.Formatter;
import com.github.icedland.iced.x86.fmt.FormatterTextKind;
import com.github.icedland.iced.x86.fmt.MasmFormatterFactory;
import com.github.icedland.iced.x86.fmt.StringOutput;
import com.github.icedland.iced.x86.fmt.SymbolFlags;
import com.github.icedland.iced.x86.fmt.SymbolResolver;
import com.github.icedland.iced.x86.fmt.SymbolResult;

final class SymbolOptionsTests {
	static class MySymbolResolver implements SymbolResolver {
		final int flags;

		public MySymbolResolver(int flags) {
			this.flags = flags;
		}

		public SymbolResult getSymbol(Instruction instruction, int operand, int instructionOperand, long address, int addressSize) {
			if (instructionOperand == 1 && (flags & SymbolTestFlags.SYMBOL) != 0)
				return new SymbolResult(address, "symbol", FormatterTextKind.DATA,
						(flags & SymbolTestFlags.SIGNED) != 0 ? SymbolFlags.SIGNED : SymbolFlags.NONE);
			return null;
		}
	}

	@ParameterizedTest
	@MethodSource("testIt_Data")
	void testIt(String hexBytes, int bitness, long ip, String formattedString, int flags) {
		Decoder decoder = new Decoder(bitness, new ByteArrayCodeReader(HexUtils.toByteArray(hexBytes)));
		decoder.setIP(ip);
		Instruction instruction = decoder.decode();

		Formatter formatter = MasmFormatterFactory.create_Resolver(new MySymbolResolver(flags)).formatter;
		formatter.getOptions().setMasmSymbolDisplInBrackets((flags & SymbolTestFlags.SYMBOL_DISPL_IN_BRACKETS) != 0);
		formatter.getOptions().setMasmDisplInBrackets((flags & SymbolTestFlags.DISPL_IN_BRACKETS) != 0);
		formatter.getOptions().setRipRelativeAddresses((flags & SymbolTestFlags.RIP) != 0);
		formatter.getOptions().setShowZeroDisplacements((flags & SymbolTestFlags.SHOW_ZERO_DISPLACEMENTS) != 0);
		formatter.getOptions().setMasmAddDsPrefix32((flags & SymbolTestFlags.NO_ADD_DS_PREFIX32) == 0);

		StringOutput output = new StringOutput();
		formatter.format(instruction, output);
		String actualFormattedString = output.toStringAndReset();
		assertEquals(formattedString, actualFormattedString);
	}

	public static Iterable<Arguments> testIt_Data() {
		ArrayList<Arguments> result = new ArrayList<Arguments>();
		String filename = PathUtils.getTestTextFilename("Formatter", "Masm", "SymbolOptions.txt");
		for (SymbolOptionsTestCase tc : SymbolOptionsTestsReader.readFile(filename))
			result.add(Arguments.of(tc.hexBytes, tc.bitness, tc.ip, tc.formattedString, tc.flags));
		return result;
	}
}
