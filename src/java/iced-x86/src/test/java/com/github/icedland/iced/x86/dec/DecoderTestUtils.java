// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import java.util.ArrayList;
import java.util.HashSet;

import com.github.icedland.iced.x86.Code;

public final class DecoderTestUtils {
	public static final HashSet<Integer> notDecoded;
	public static final HashSet<Integer> notDecoded32Only;
	public static final HashSet<Integer> notDecoded64Only;
	public static final HashSet<Integer> code32Only;
	public static final HashSet<Integer> code64Only;

	static {
		notDecoded = CodeValueReader.read("Code.NotDecoded.txt");
		notDecoded32Only = CodeValueReader.read("Code.NotDecoded32Only.txt");
		notDecoded64Only = CodeValueReader.read("Code.NotDecoded64Only.txt");
		code32Only = CodeValueReader.read("Code.32Only.txt");
		code64Only = CodeValueReader.read("Code.64Only.txt");
	}

	public static Iterable<DecoderTestInfo> getEncoderTests(boolean includeOtherTests, boolean includeInvalid) {
		ArrayList<DecoderTestInfo> result = new ArrayList<DecoderTestInfo>();
		for (DecoderTestInfo info : getDecoderTests(includeOtherTests, includeInvalid)) {
			if ((info.testOptions & DecoderTestOptions.NO_ENCODE) == 0)
				result.add(info);
		}
		return result;
	}

	public static Iterable<DecoderTestInfo> getDecoderTests(boolean includeOtherTests, boolean includeInvalid) {
		ArrayList<DecoderTestInfo> result = new ArrayList<DecoderTestInfo>();
		for (DecoderTestInfo info : getDecoderTests(includeOtherTests)) {
			if (includeInvalid || info.code != Code.INVALID)
				result.add(info);
		}
		return result;
	}

	static Iterable<DecoderTestInfo> getDecoderTests(boolean includeOtherTests) {
		ArrayList<DecoderTestInfo> result = new ArrayList<DecoderTestInfo>();
		int id = 0;
		for (DecoderTestCase tc : DecoderTestCases.testCases16)
			result.add(new DecoderTestInfo(id++, tc.bitness, tc.ip, tc.code, tc.hexBytes, tc.encodedHexBytes, tc.decoderOptions, tc.testOptions));
		for (DecoderTestCase tc : DecoderTestCases.testCases32)
			result.add(new DecoderTestInfo(id++, tc.bitness, tc.ip, tc.code, tc.hexBytes, tc.encodedHexBytes, tc.decoderOptions, tc.testOptions));
		for (DecoderTestCase tc : DecoderTestCases.testCases64)
			result.add(new DecoderTestInfo(id++, tc.bitness, tc.ip, tc.code, tc.hexBytes, tc.encodedHexBytes, tc.decoderOptions, tc.testOptions));

		if (includeOtherTests) {
			for (DecoderTestCase tc : DecoderTestCases.testCasesMisc16)
				result.add(new DecoderTestInfo(id++, tc.bitness, tc.ip, tc.code, tc.hexBytes, tc.encodedHexBytes, tc.decoderOptions, tc.testOptions));
			for (DecoderTestCase tc : DecoderTestCases.testCasesMisc32)
				result.add(new DecoderTestInfo(id++, tc.bitness, tc.ip, tc.code, tc.hexBytes, tc.encodedHexBytes, tc.decoderOptions, tc.testOptions));
			for (DecoderTestCase tc : DecoderTestCases.testCasesMisc64)
				result.add(new DecoderTestInfo(id++, tc.bitness, tc.ip, tc.code, tc.hexBytes, tc.encodedHexBytes, tc.decoderOptions, tc.testOptions));

			for (DecoderMemoryTestCase tc : DecoderTestCases.testCasesMemory16)
				result.add(new DecoderTestInfo(id++, tc.bitness, tc.ip, tc.code, tc.hexBytes, tc.encodedHexBytes, tc.decoderOptions, tc.testOptions));
			for (DecoderMemoryTestCase tc : DecoderTestCases.testCasesMemory32)
				result.add(new DecoderTestInfo(id++, tc.bitness, tc.ip, tc.code, tc.hexBytes, tc.encodedHexBytes, tc.decoderOptions, tc.testOptions));
			for (DecoderMemoryTestCase tc : DecoderTestCases.testCasesMemory64)
				result.add(new DecoderTestInfo(id++, tc.bitness, tc.ip, tc.code, tc.hexBytes, tc.encodedHexBytes, tc.decoderOptions, tc.testOptions));
		}
		return result;
	}
}
