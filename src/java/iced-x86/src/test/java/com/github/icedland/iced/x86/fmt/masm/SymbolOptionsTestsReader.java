// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt.masm;

import java.util.ArrayList;

import com.github.icedland.iced.x86.CodeUtils;
import com.github.icedland.iced.x86.DecoderConstants;
import com.github.icedland.iced.x86.FileUtils;
import com.github.icedland.iced.x86.NumberConverter;
import com.github.icedland.iced.x86.StringUtils2;

final class SymbolOptionsTestsReader {
	static SymbolOptionsTestCase[] readFile(String filename) {
		ArrayList<SymbolOptionsTestCase> result = new ArrayList<SymbolOptionsTestCase>();
		int lineNo = 0;
		for (String line : FileUtils.readAllLines(filename)) {
			lineNo++;
			if (line.length() == 0 || line.charAt(0) == '#')
				continue;

			SymbolOptionsTestCase tc;
			try {
				tc = parseLine(line);
			} catch (Exception ex) {
				throw new UnsupportedOperationException(String.format("Invalid line %d (%s): %s", lineNo, filename, ex.getMessage()));
			}
			if (tc != null)
				result.add(tc);
		}
		return result.toArray(new SymbolOptionsTestCase[0]);
	}

	static SymbolOptionsTestCase parseLine(String line) {
		String[] elems = StringUtils2.split(line, ",");
		if (elems.length != 5)
			throw new UnsupportedOperationException(String.format("Invalid number of commas: %d", elems.length - 1));

		String hexBytes = elems[0].trim();
		if (CodeUtils.isIgnored(elems[1].trim()))
			return null;
		int bitness = NumberConverter.toInt32(elems[2].trim());
		long ip;
		switch (bitness) {
		case 16:
			ip = DecoderConstants.DEFAULT_IP16;
			break;
		case 32:
			ip = DecoderConstants.DEFAULT_IP32;
			break;
		case 64:
			ip = DecoderConstants.DEFAULT_IP64;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		String formattedString = elems[3].trim().replace('|', ',');
		int flags = SymbolTestFlags.NONE;
		for (String value : elems[4].split(" ")) {
			if (value.equals(""))
				continue;
			Integer f = SymbolTestFlagsDict.toSymbolTestFlags.get(value);
			if (f == null)
				throw new UnsupportedOperationException(String.format("Invalid flags value: %s", value));
			flags |= f;
		}
		return new SymbolOptionsTestCase(hexBytes, bitness, ip, formattedString, flags);
	}
}
