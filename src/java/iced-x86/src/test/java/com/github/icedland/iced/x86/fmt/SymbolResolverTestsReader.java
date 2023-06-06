// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import java.util.ArrayList;
import java.util.HashSet;

import com.github.icedland.iced.x86.CodeUtils;
import com.github.icedland.iced.x86.DecoderConstants;
import com.github.icedland.iced.x86.FileUtils;
import com.github.icedland.iced.x86.NumberConverter;
import com.github.icedland.iced.x86.StringUtils2;
import com.github.icedland.iced.x86.ToCode;
import com.github.icedland.iced.x86.ToMemorySize;

final class SymbolResolverTestsReader {
	public static ArrayList<SymbolResolverTestCase> readFile(String filename, HashSet<Integer> ignored) {
		ArrayList<SymbolResolverTestCase> result = new ArrayList<SymbolResolverTestCase>();
		ArrayList<String> lines = FileUtils.readAllLines(filename);
		int lineNo = 0;
		int testCaseNo = 0;
		for (String line : lines) {
			lineNo++;
			if (line.length() == 0 || line.charAt(0) == '#')
				continue;

			SymbolResolverTestCase tc;
			try {
				tc = parseLine(line);
			} catch (Exception ex) {
				throw new UnsupportedOperationException(String.format("Invalid line %d (%s): %s", lineNo, filename, ex.getMessage()));
			}
			if (tc != null)
				result.add(tc);
			else
				ignored.add(testCaseNo);
			testCaseNo++;
		}
		return result;
	}

	static SymbolResolverTestCase parseLine(String line) {
		final int SYM_RES_INDEX = 4;
		String[] elems = StringUtils2.split(line, ",");
		if (elems.length < SYM_RES_INDEX)
			throw new UnsupportedOperationException(String.format("Invalid number of commas: %d", elems.length - 1));

		int bitness = NumberConverter.toInt32(elems[0].trim());
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
		String hexBytes = elems[1].trim();
		String codeStr = elems[2].trim();
		if (CodeUtils.isIgnored(codeStr))
			return null;
		int code = ToCode.get(codeStr);

		ArrayList<OptionsTestCase.Info> options = new ArrayList<OptionsTestCase.Info>();
		for (String part : elems[3].split(" ")) {
			if (part.equals(""))
				continue;
			options.add(OptionsParser.parseOption(part));
		}

		SymbolResultTestCase[] symbolResults = new SymbolResultTestCase[elems.length - SYM_RES_INDEX];
		for (int i = 0; i < symbolResults.length; i++) {
			String[] symParts = StringUtils2.split(elems[SYM_RES_INDEX + i], ";");
			if (symParts.length != 5)
				throw new UnsupportedOperationException(String.format("Invalid number of semicolons: %d", symParts.length - 1));

			long address = NumberConverter.toUInt64(symParts[0].trim());
			long symbolAddress = NumberConverter.toUInt64(symParts[1].trim());
			int addressSize = NumberConverter.toInt32(symParts[2].trim());
			String[] symbolParts = symParts[3].split("\\|");

			Integer memorySize = null;
			int flags = SymbolFlags.NONE;
			for (String value : symParts[4].split(" ")) {
				if (value.equals(""))
					continue;
				Integer f = SymbolResolverDicts.toSymbolFlags.get(value);
				if (f != null)
					flags |= f;
				else
					memorySize = ToMemorySize.get(value);
			}

			symbolResults[i] = new SymbolResultTestCase(address, symbolAddress, addressSize, flags, memorySize, symbolParts);
		}

		return new SymbolResolverTestCase(bitness, hexBytes, ip, code, options.toArray(new OptionsTestCase.Info[0]), symbolResults);
	}
}
