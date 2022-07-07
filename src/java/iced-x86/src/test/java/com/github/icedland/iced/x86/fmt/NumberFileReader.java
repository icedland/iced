// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import java.util.ArrayList;

import com.github.icedland.iced.x86.FileUtils;
import com.github.icedland.iced.x86.NumberConverter;

final class NumberFileReader {
	public static TestNumber[] readNumberFile(String filename) {
		ArrayList<TestNumber> result = new ArrayList<TestNumber>();
		int lineNo = 0;
		for (String line : FileUtils.readAllLines(FmtFileUtils.getFormatterFilename("Number"))) {
			lineNo++;
			if (line.length() == 0 || line.charAt(0) == '#')
				continue;
			TestNumber testCase;
			try {
				testCase = readTestCase(line, lineNo);
			}
			catch (Exception ex) {
				throw new UnsupportedOperationException(
						String.format("Error parsing number test case file '%s', line %d: %s", filename, lineNo, ex.getMessage()));
			}
			result.add(testCase);
		}
		return result.toArray(new TestNumber[0]);
	}

	static TestNumber readTestCase(String line, int lineNo) {
		String[] parts = line.split(",");
		if (parts.length != 2)
			throw new UnsupportedOperationException(String.format("Invalid number of commas (%d commas)", parts.length - 1));

		String valueStr = parts[1].trim();
		switch (parts[0].trim()) {
		case "i8":
			return new TestNumber(TestNumberKind.BYTE, NumberConverter.toInt8(valueStr));
		case "u8":
			return new TestNumber(TestNumberKind.UBYTE, NumberConverter.toUInt8(valueStr));
		case "i16":
			return new TestNumber(TestNumberKind.SHORT, NumberConverter.toInt16(valueStr));
		case "u16":
			return new TestNumber(TestNumberKind.USHORT, NumberConverter.toUInt16(valueStr));
		case "i32":
			return new TestNumber(TestNumberKind.INT, NumberConverter.toInt32(valueStr));
		case "u32":
			return new TestNumber(TestNumberKind.UINT, NumberConverter.toUInt32(valueStr));
		case "i64":
			return new TestNumber(TestNumberKind.LONG, NumberConverter.toInt64(valueStr));
		case "u64":
			return new TestNumber(TestNumberKind.ULONG, NumberConverter.toUInt64(valueStr));
		default:
			throw new UnsupportedOperationException(String.format("Invalid type: %s", parts[0]));
		}
	}
}
