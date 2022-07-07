// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.HashSet;

import org.junit.jupiter.params.provider.*;

import com.github.icedland.iced.x86.CodeUtils;
import com.github.icedland.iced.x86.DecoderConstants;
import com.github.icedland.iced.x86.FileUtils;
import com.github.icedland.iced.x86.ToCode;
import com.github.icedland.iced.x86.ToDecoderOptions;
import com.github.icedland.iced.x86.dec.DecoderOptions;
import com.github.icedland.iced.x86.dec.NonDecodedTestCase;

public final class FormatterTestCases {
	public static class Info {
		public FormatterTestCase[] tests;
		public HashSet<Integer> ignored;
	}

	static final Info tests16;
	static final Info tests32;
	static final Info tests64;
	static final Info tests16_Misc;
	static final Info tests32_Misc;
	static final Info tests64_Misc;

	static {
		tests16 = createTests(16, false);
		tests32 = createTests(32, false);
		tests64 = createTests(64, false);
		tests16_Misc = createTests(16, true);
		tests32_Misc = createTests(32, true);
		tests64_Misc = createTests(64, true);
	}

	public static Info getTests(int bitness, boolean isMisc) {
		if (isMisc) {
			switch (bitness) {
			case 16:
				return tests16_Misc;
			case 32:
				return tests32_Misc;
			case 64:
				return tests64_Misc;
			default:
				throw new UnsupportedOperationException();
			}
		}
		else {
			switch (bitness) {
			case 16:
				return tests16;
			case 32:
				return tests32;
			case 64:
				return tests64;
			default:
				throw new UnsupportedOperationException();
			}
		}
	}

	private static Info createTests(int bitness, boolean isMisc) {
		String filename = "InstructionInfos" + bitness;
		if (isMisc)
			filename += "_Misc";
		Info info = new Info();
		info.ignored = new HashSet<Integer>();
		info.tests = readTests(filename, bitness, info.ignored).toArray(new FormatterTestCase[0]);
		return info;
	}

	private static ArrayList<FormatterTestCase> readTests(String filename, int bitness, HashSet<Integer> ignored) {
		int lineNo = 0;
		int testCaseNo = 0;
		filename = FmtFileUtils.getFormatterFilename(filename);
		ArrayList<String> lines = FileUtils.readAllLines(filename);
		ArrayList<FormatterTestCase> result = new ArrayList<FormatterTestCase>();
		for (String line : lines) {
			lineNo++;
			if (line.length() == 0 || line.charAt(0) == '#')
				continue;
			String[] parts = line.split(",");
			int options;
			if (parts.length == 2)
				options = DecoderOptions.NONE;
			else if (parts.length == 3) {
				Integer value = ToDecoderOptions.tryGet(parts[2].trim());
				if (value == null)
					throw new UnsupportedOperationException(String.format("Invalid line #%d in file %s", lineNo, filename));
				options = value;
			}
			else
				throw new UnsupportedOperationException(String.format("Invalid line #%d in file %s", lineNo, filename));
			String hexBytes = parts[0].trim();
			String codeStr = parts[1].trim();
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
			if (CodeUtils.isIgnored(codeStr))
				ignored.add(testCaseNo);
			else {
				Integer value = ToCode.tryGet(codeStr);
				if (value == null)
					throw new UnsupportedOperationException(String.format("Invalid line #%d in file %s", lineNo, filename));
				int code = value;
				result.add(new FormatterTestCase(bitness, hexBytes, ip, code, options));
			}
			testCaseNo++;
		}
		return result;
	}

	public static Iterable<Arguments> getFormatData(int bitness, String formatterDir, String formattedStringsFile) {
		return getFormatData(bitness, formatterDir, formattedStringsFile, false);
	}

	public static Iterable<Arguments> getFormatData(int bitness, String formatterDir, String formattedStringsFile, boolean isMisc) {
		Info info = getTests(bitness, isMisc);
		String[] formattedStrings = FmtFileUtils
				.readRawStrings(Paths.get(formatterDir, String.format("Test%d_%s", bitness, formattedStringsFile)).toString()).toArray(new String[0]);
		return getFormatData(info.tests, info.ignored, formattedStrings);
	}

	private static Iterable<Arguments> getFormatData(FormatterTestCase[] tests, HashSet<Integer> ignored, String[] formattedStrings) {
		formattedStrings = Utils.filter(formattedStrings, ignored);
		if (tests.length != formattedStrings.length)
			throw new UnsupportedOperationException();
		ArrayList<Arguments> result = new ArrayList<Arguments>(tests.length);
		for (int i = 0; i < tests.length; i++)
			result.add(Arguments.of(i, tests[i], formattedStrings[i]));
		return result;
	}

	public static Iterable<Arguments> getFormatData(int bitness, NonDecodedTestCase[] tests, String formatterDir, String formattedStringsFile) {
		String[] formattedStrings = FmtFileUtils
				.readRawStrings(Paths.get(formatterDir, String.format("Test%d_%s", bitness, formattedStringsFile)).toString()).toArray(new String[0]);
		return getFormatData(tests, formattedStrings);
	}

	private static Iterable<Arguments> getFormatData(NonDecodedTestCase[] tests, String[] formattedStrings) {
		if (tests.length != formattedStrings.length)
			throw new UnsupportedOperationException();
		ArrayList<Arguments> result = new ArrayList<Arguments>(tests.length);
		for (int i = 0; i < tests.length; i++)
			result.add(Arguments.of(i, tests[i].instruction, formattedStrings[i]));
		return result;
	}
}
