// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import java.util.ArrayList;
import java.util.HashSet;

import com.github.icedland.iced.x86.CodeUtils;
import com.github.icedland.iced.x86.DecoderConstants;
import com.github.icedland.iced.x86.FileUtils;
import com.github.icedland.iced.x86.HexUtils;
import com.github.icedland.iced.x86.NumberConverter;
import com.github.icedland.iced.x86.StringUtils2;
import com.github.icedland.iced.x86.ToCode;

final class OptionsTestsReader {
	public static ArrayList<OptionsTestCase> readFile(String filename, HashSet<Integer> ignored) {
		ArrayList<OptionsTestCase> result = new ArrayList<OptionsTestCase>();
		ArrayList<String> lines = FileUtils.readAllLines(filename);
		int lineNo = 0;
		int testCaseNo = 0;
		for (String line : lines) {
			lineNo++;
			if (line.length() == 0 || line.charAt(0) == '#')
				continue;
			OptionsTestCase testCase;
			try {
				testCase = readTestCase(line, lineNo);
			} catch (Exception ex) {
				throw new UnsupportedOperationException(
						String.format("Error parsing options test case file '%s', line %d: %s", filename, lineNo, ex.getMessage()));
			}
			if (testCase != null)
				result.add(testCase);
			else
				ignored.add(testCaseNo);
			testCaseNo++;
		}
		return result;
	}

	static OptionsTestCase readTestCase(String line, int lineNo) {
		String[] parts = StringUtils2.split(line, ",");
		if (parts.length != 4)
			throw new UnsupportedOperationException(String.format("Invalid number of commas (%d commas)", parts.length - 1));

		int bitness = NumberConverter.toInt32(parts[0].trim());
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
		String hexBytes = parts[1].trim();
		HexUtils.toByteArray(hexBytes);
		String codeStr = parts[2].trim();
		if (CodeUtils.isIgnored(codeStr))
			return null;
		int code = ToCode.get(codeStr);

		ArrayList<OptionsTestCase.Info> properties = new ArrayList<OptionsTestCase.Info>();
		for (String part : parts[3].split(" ")) {
			if (part.equals(""))
				continue;
			properties.add(OptionsParser.parseOption(part));
		}

		return new OptionsTestCase(bitness, hexBytes, ip, code, properties.toArray(new OptionsTestCase.Info[0]));
	}
}
