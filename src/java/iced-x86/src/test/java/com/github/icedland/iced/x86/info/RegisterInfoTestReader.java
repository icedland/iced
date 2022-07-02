// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

import java.util.ArrayList;

import com.github.icedland.iced.x86.FileUtils;
import com.github.icedland.iced.x86.NumberConverter;
import com.github.icedland.iced.x86.PathUtils;
import com.github.icedland.iced.x86.ToRegister;
import com.github.icedland.iced.x86.internal.IcedConstants;

final class RegisterInfoTestReader {
	public static RegisterInfoTestCase[] getTestCases() {
		String filename = PathUtils.getTestTextFilename("InstructionInfo", "RegisterInfo.txt");
		ArrayList<String> lines = FileUtils.readAllLines(filename);
		RegisterInfoTestCase[] result = new RegisterInfoTestCase[IcedConstants.REGISTER_ENUM_COUNT];
		int lineNo = 0;
		for (String line : lines) {
			lineNo++;
			if (line.length() == 0 || line.charAt(0) == '#')
				continue;

			RegisterInfoTestCase tc;
			try {
				tc = parseLine(line, lineNo);
			}
			catch (Exception ex) {
				throw new UnsupportedOperationException(String.format("Invalid line %d (%s): %s", lineNo, filename, ex.getMessage()));
			}
			if (result[tc.register] != null)
				throw new UnsupportedOperationException(String.format("Duplicate test, %s, line %d", filename, lineNo));
			result[tc.register] = tc;
		}

		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < result.length; i++) {
			RegisterInfoTestCase tc = result[i];
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

	private static RegisterInfoTestCase parseLine(String line, int lineNo) {
		int expValue = 7;
		if (MiscInstrInfoTestConstants.REGISTER_ELEMS_PER_LINE != expValue)
			throw new UnsupportedOperationException();
		String[] elems = line.split(",", MiscInstrInfoTestConstants.REGISTER_ELEMS_PER_LINE);
		if (elems.length != MiscInstrInfoTestConstants.REGISTER_ELEMS_PER_LINE)
			throw new UnsupportedOperationException(String.format("Expected %d commas", MiscInstrInfoTestConstants.REGISTER_ELEMS_PER_LINE - 1));

		RegisterInfoTestCase tc = new RegisterInfoTestCase();
		tc.lineNumber = lineNo;
		tc.register = ToRegister.get(elems[0].trim());
		tc.number = NumberConverter.toInt32(elems[1].trim());
		tc.baseRegister = ToRegister.get(elems[2].trim());
		tc.fullRegister = ToRegister.get(elems[3].trim());
		tc.fullRegister32 = ToRegister.get(elems[4].trim());
		tc.size = NumberConverter.toInt32(elems[5].trim());
		for (String value : elems[6].split(" ")) {
			if (value.equals(""))
				continue;
			Integer flags = InstrInfoDicts.toRegisterFlags.get(value);
			if (flags == null)
				throw new UnsupportedOperationException(String.format("Invalid flags value: %s", value));
			tc.flags |= flags;
		}
		return tc;
	}
}
