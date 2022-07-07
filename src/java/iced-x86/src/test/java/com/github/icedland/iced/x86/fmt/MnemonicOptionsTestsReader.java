// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import java.util.ArrayList;

import com.github.icedland.iced.x86.CodeUtils;
import com.github.icedland.iced.x86.DecoderConstants;
import com.github.icedland.iced.x86.FileUtils;
import com.github.icedland.iced.x86.NumberConverter;
import com.github.icedland.iced.x86.StringUtils2;
import com.github.icedland.iced.x86.ToCode;

final class MnemonicOptionsTestsReader {
	public static MnemonicOptionsTestCase[] readFile(String filename) {
		ArrayList<MnemonicOptionsTestCase> result = new ArrayList<MnemonicOptionsTestCase>();
		int lineNo = 0;
		for (String line : FileUtils.readAllLines(filename)) {
			lineNo++;
			if (line.length() == 0 || line.charAt(0) == '#')
				continue;

			MnemonicOptionsTestCase tc;
			try {
				tc = parseLine(line);
			}
			catch (Exception ex) {
				throw new UnsupportedOperationException(String.format("Invalid line %d (%s): %s", lineNo, filename, ex.getMessage()));
			}
			if (tc != null)
				result.add(tc);
		}
		return result.toArray(new MnemonicOptionsTestCase[0]);
	}

	static MnemonicOptionsTestCase parseLine(String line) {
		String[] elems = StringUtils2.split(line, ",");
		if (elems.length != 5)
			throw new UnsupportedOperationException(String.format("Invalid number of commas: %d", elems.length - 1));

		String hexBytes = elems[0].trim();
		String codeStr = elems[1].trim();
		if (CodeUtils.isIgnored(codeStr))
			return null;
		int code = ToCode.get(codeStr);
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
		int flags = FormatMnemonicOptions.NONE;
		for (String value : elems[4].split(" ")) {
			if (value.equals(""))
				continue;
			Integer f = MnemonicOptionsDict.toFormatMnemonicOptions.get(value);
			if (f == null)
				throw new UnsupportedOperationException(String.format("Invalid flags value: %s", value));
			flags |= f;
		}
		return new MnemonicOptionsTestCase(hexBytes, code, bitness, ip, formattedString, flags);
	}
}
