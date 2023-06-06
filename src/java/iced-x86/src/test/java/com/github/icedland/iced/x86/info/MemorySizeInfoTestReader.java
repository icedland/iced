// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

import java.util.ArrayList;

import com.github.icedland.iced.x86.FileUtils;
import com.github.icedland.iced.x86.NumberConverter;
import com.github.icedland.iced.x86.PathUtils;
import com.github.icedland.iced.x86.ToMemorySize;
import com.github.icedland.iced.x86.internal.IcedConstants;

final class MemorySizeInfoTestReader {
	public static MemorySizeInfoTestCase[] getTestCases() {
		String filename = PathUtils.getTestTextFilename("InstructionInfo", "MemorySizeInfo.txt");
		ArrayList<String> lines = FileUtils.readAllLines(filename);
		MemorySizeInfoTestCase[] result = new MemorySizeInfoTestCase[IcedConstants.MEMORY_SIZE_ENUM_COUNT];
		int lineNo = 0;
		for (String line : lines) {
			lineNo++;
			if (line.length() == 0 || line.charAt(0) == '#')
				continue;

			MemorySizeInfoTestCase tc;
			try {
				tc = parseLine(line, lineNo);
			}
			catch (Exception ex) {
				throw new UnsupportedOperationException(String.format("Invalid line %d (%s): %s", lineNo, filename, ex.getMessage()));
			}
			if (result[tc.memorySize] != null)
				throw new UnsupportedOperationException(String.format("Duplicate test, %s, line %d", filename, lineNo));
			result[tc.memorySize] = tc;
		}

		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < result.length; i++) {
			MemorySizeInfoTestCase tc = result[i];
			if (tc == null) {
				if (sb.length() > 0)
					sb.append(", ");
				sb.append(i);
			}
		}
		if (sb.length() != 0)
			throw new UnsupportedOperationException(String.format("Missing tests in %s: %s", filename, sb.toString()));
		return result;
	}

	private static MemorySizeInfoTestCase parseLine(String line, int lineNo) {
		int expValue = 6;
		if (MiscInstrInfoTestConstants.MEMORY_SIZE_ELEMS_PER_LINE != expValue)
			throw new UnsupportedOperationException();
		String[] elems = line.split(",", MiscInstrInfoTestConstants.MEMORY_SIZE_ELEMS_PER_LINE);
		if (elems.length != MiscInstrInfoTestConstants.MEMORY_SIZE_ELEMS_PER_LINE)
			throw new UnsupportedOperationException(String.format("Expected %d commas", MiscInstrInfoTestConstants.MEMORY_SIZE_ELEMS_PER_LINE - 1));

		MemorySizeInfoTestCase tc = new MemorySizeInfoTestCase();
		tc.lineNumber = lineNo;
		tc.memorySize = ToMemorySize.get(elems[0].trim());
		tc.size = NumberConverter.toInt32(elems[1].trim());
		tc.elementSize = NumberConverter.toInt32(elems[2].trim());
		tc.elementType = ToMemorySize.get(elems[3].trim());
		tc.elementCount = NumberConverter.toInt32(elems[4].trim());
		for (String value : elems[5].split(" ")) {
			if (value.equals(""))
				continue;
			Integer flags = InstrInfoDicts.toMemorySizeFlags.get(value);
			if (flags == null)
				throw new UnsupportedOperationException(String.format("Invalid flags value: %s", value));
			tc.flags |= flags;
		}
		return tc;
	}
}
