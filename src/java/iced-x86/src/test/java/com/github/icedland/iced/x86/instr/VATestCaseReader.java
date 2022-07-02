// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.instr;

import java.util.ArrayList;

import com.github.icedland.iced.x86.CodeUtils;
import com.github.icedland.iced.x86.FileUtils;
import com.github.icedland.iced.x86.NumberConverter;
import com.github.icedland.iced.x86.ToDecoderOptions;
import com.github.icedland.iced.x86.ToRegister;
import com.github.icedland.iced.x86.dec.DecoderOptions;

final class VATestCaseReader {
	public static VirtualAddressTestCase[] readFile(String filename) {
		ArrayList<String> lines = FileUtils.readAllLines(filename);
		ArrayList<VirtualAddressTestCase> result = new ArrayList<VirtualAddressTestCase>(lines.size());
		int lineNo = 0;
		for (String line : lines) {
			lineNo++;
			if (line.length() == 0 || line.charAt(0) == '#')
				continue;

			VirtualAddressTestCase tc;
			try {
				tc = parseLine(line);
			}
			catch (Exception ex) {
				throw new UnsupportedOperationException(String.format("Invalid line %d (%s): %s", lineNo, filename, ex.getMessage()));
			}
			if (tc != null)
				result.add(tc);
		}
		return result.toArray(new VirtualAddressTestCase[result.size()]);
	}

	private static VirtualAddressTestCase parseLine(String line) {
		String[] elems = line.split(",");
		if (elems.length != 9)
			throw new UnsupportedOperationException(String.format("Invalid number of commas: %d", elems.length - 1));

		int bitness = NumberConverter.toInt32(elems[0].trim());
		if (CodeUtils.isIgnored(elems[1].trim()))
			return null;
		String hexBytes = elems[2].trim();
		int operand = NumberConverter.toInt32(elems[3].trim());
		int usedMemIndex = NumberConverter.toInt32(elems[4].trim());
		int elementIndex = NumberConverter.toInt32(elems[5].trim());
		long expectedValue = NumberConverter.toUInt64(elems[6].trim());
		String decOptStr = elems[7].trim();
		int decoderOptions = decOptStr.equals("") ? DecoderOptions.NONE : ToDecoderOptions.get(decOptStr);

		ArrayList<VARegisterValue> registerValues = new ArrayList<VARegisterValue>();
		for (String tmp : elems[8].split(" ")) {
			if (tmp.length() == 0)
				continue;
			String[] kv = tmp.split("=");
			if (kv.length != 2)
				throw new UnsupportedOperationException(String.format("Expected key=value: %s", tmp));
			String key = kv[0];
			String valueStr = kv[1];

			int register;
			int expectedElementIndex;
			int expectedElementSize;
			if (key.indexOf(';') >= 0) {
				String[] parts = key.split(";");
				if (parts.length != 3)
					throw new UnsupportedOperationException(String.format("Invalid number of semicolons: %d", parts.length - 1));
				register = ToRegister.get(parts[0]);
				expectedElementIndex = NumberConverter.toInt32(parts[1]);
				expectedElementSize = NumberConverter.toInt32(parts[2]);
			} else {
				register = ToRegister.get(key);
				expectedElementIndex = 0;
				expectedElementSize = 0;
			}
			long value = NumberConverter.toUInt64(valueStr);
			registerValues.add(new VARegisterValue(register, expectedElementIndex, expectedElementSize, value));
		}

		return new VirtualAddressTestCase(bitness, hexBytes, decoderOptions, operand, usedMemIndex, elementIndex, expectedValue,
				registerValues.toArray(new VARegisterValue[registerValues.size()]));
	}
}
