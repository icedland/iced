// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import com.github.icedland.iced.x86.PathUtils;

final class DecoderTestCases {
	public static final DecoderTestCase[] testCases16;
	public static final DecoderTestCase[] testCases32;
	public static final DecoderTestCase[] testCases64;
	public static final DecoderTestCase[] testCasesMisc16;
	public static final DecoderTestCase[] testCasesMisc32;
	public static final DecoderTestCase[] testCasesMisc64;
	public static final DecoderMemoryTestCase[] testCasesMemory16;
	public static final DecoderMemoryTestCase[] testCasesMemory32;
	public static final DecoderMemoryTestCase[] testCasesMemory64;

	static {
		testCases16 = readTestCases(16);
		testCases32 = readTestCases(32);
		testCases64 = readTestCases(64);
		testCasesMisc16 = readMiscTestCases(16);
		testCasesMisc32 = readMiscTestCases(32);
		testCasesMisc64 = readMiscTestCases(64);
		testCasesMemory16 = readMemoryTestCases(16);
		testCasesMemory32 = readMemoryTestCases(32);
		testCasesMemory64 = readMemoryTestCases(64);
	}

	public static DecoderTestCase[] getTestCases(int bitness) {
		switch (bitness) {
		case 16:
			return testCases16;
		case 32:
			return testCases32;
		case 64:
			return testCases64;
		default:
			throw new UnsupportedOperationException();
		}
	}

	public static DecoderTestCase[] getMiscTestCases(int bitness) {
		switch (bitness) {
		case 16:
			return testCasesMisc16;
		case 32:
			return testCasesMisc32;
		case 64:
			return testCasesMisc64;
		default:
			throw new UnsupportedOperationException();
		}
	}

	public static DecoderMemoryTestCase[] getMemoryTestCases(int bitness) {
		switch (bitness) {
		case 16:
			return testCasesMemory16;
		case 32:
			return testCasesMemory32;
		case 64:
			return testCasesMemory64;
		default:
			throw new UnsupportedOperationException();
		}
	}

	private static DecoderTestCase[] readTestCases(int bitness) {
		String filename = PathUtils.getTestTextFilename("Decoder", String.format("DecoderTest%d.txt", bitness));
		return DecoderTestParser.readFile(bitness, filename);
	}

	private static DecoderTestCase[] readMiscTestCases(int bitness) {
		String filename = PathUtils.getTestTextFilename("Decoder", String.format("DecoderTestMisc%d.txt", bitness));
		return DecoderTestParser.readFile(bitness, filename);
	}

	private static DecoderMemoryTestCase[] readMemoryTestCases(int bitness) {
		String filename = PathUtils.getTestTextFilename("Decoder", String.format("MemoryTest%d.txt", bitness));
		return MemoryDecoderTestParser.readFile(bitness, filename);
	}
}
