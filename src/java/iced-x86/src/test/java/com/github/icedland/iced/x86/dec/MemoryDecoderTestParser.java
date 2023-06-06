// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import java.util.ArrayList;

import com.github.icedland.iced.x86.CodeUtils;
import com.github.icedland.iced.x86.ConstantOffsets;
import com.github.icedland.iced.x86.DecoderConstants;
import com.github.icedland.iced.x86.FileUtils;
import com.github.icedland.iced.x86.NumberConverter;
import com.github.icedland.iced.x86.ToCode;
import com.github.icedland.iced.x86.ToRegister;

final class MemoryDecoderTestParser {
	public static DecoderMemoryTestCase[] readFile(int bitness, String filename) {
		ArrayList<String> lines = FileUtils.readAllLines(filename);
		ArrayList<DecoderMemoryTestCase> result = new ArrayList<DecoderMemoryTestCase>(lines.size());
		int lineNumber = 0;
		for (String line : lines) {
			lineNumber++;
			if (line.length() == 0 || line.charAt(0) == '#')
				continue;
			DecoderMemoryTestCase testCase;
			try {
				testCase = readTestCase(bitness, line, lineNumber);
			}
			catch (Exception ex) {
				throw new UnsupportedOperationException(
						String.format("Error parsing decoder test case file '%s', line %d: %s", filename, lineNumber, ex.getMessage()));
			}
			if (testCase != null)
				result.add(testCase);
		}
		return result.toArray(new DecoderMemoryTestCase[result.size()]);
	}

	private static DecoderMemoryTestCase readTestCase(int bitness, String line, int lineNumber) {
		String[] parts = line.split(",");
		if (parts.length != 11 && parts.length != 12)
			throw new UnsupportedOperationException();
		DecoderMemoryTestCase tc = new DecoderMemoryTestCase();
		tc.lineNumber = lineNumber;
		tc.bitness = bitness;
		switch (bitness) {
		case 16:
			tc.ip = DecoderConstants.DEFAULT_IP16;
			break;
		case 32:
			tc.ip = DecoderConstants.DEFAULT_IP32;
			break;
		case 64:
			tc.ip = DecoderConstants.DEFAULT_IP64;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		tc.hexBytes = parts[0].trim();
		String code = parts[1].trim();
		if (CodeUtils.isIgnored(code))
			return null;
		tc.code = ToCode.get(code);
		tc.register = ToRegister.get(parts[2].trim());
		tc.segmentPrefix = ToRegister.get(parts[3].trim());
		tc.segmentRegister = ToRegister.get(parts[4].trim());
		tc.baseRegister = ToRegister.get(parts[5].trim());
		tc.indexRegister = ToRegister.get(parts[6].trim());
		tc.scale = NumberConverter.toUInt32(parts[7].trim());
		tc.displacement = NumberConverter.toUInt64(parts[8].trim());
		tc.displacementSize = NumberConverter.toUInt32(parts[9].trim());
		String coStr = parts[10].trim();
		ConstantOffsets co = DecoderTestParser.tryParseConstantOffsets(coStr);
		if (co == null)
			throw new UnsupportedOperationException(String.format("Invalid ConstantOffsets: '%s'", coStr));
		tc.constantOffsets = co;
		tc.encodedHexBytes = parts.length > 11 ? parts[11].trim() : tc.hexBytes;
		tc.decoderOptions = DecoderOptions.NONE;
		tc.testOptions = DecoderTestOptions.NONE;
		return tc;
	}
}
