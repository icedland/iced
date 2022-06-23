// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

import java.util.ArrayList;

import org.junit.jupiter.api.*;
import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.params.provider.*;

import com.github.icedland.iced.x86.CodeUtils;
import com.github.icedland.iced.x86.ToCode;
import com.github.icedland.iced.x86.internal.IcedConstants;

final class CodeValueTests {
	@Test
	void make_sure_all_Code_values_are_tested() {
		boolean[] tested = new boolean[IcedConstants.CODE_ENUM_COUNT];

		for (Arguments args : getTests()) {
			InstructionInfoTestCase tc = (InstructionInfoTestCase)args.get()[0];
			tested[tc.code] = true;
		}

		StringBuilder sb = new StringBuilder();
		int missing = 0;
		String[] codeNames = ToCode.names();
		assertEquals(tested.length, codeNames.length);
		for (int i = 0; i < tested.length; i++) {
			if (!tested[i] && !CodeUtils.isIgnored(codeNames[i])) {
				sb.append(codeNames[i] + " ");
				missing++;
			}
		}
		assertEquals("0 ins ", String.format("%d ins %s", missing, sb.toString()));
	}

	private static Iterable<Arguments> getTests() {
		ArrayList<Arguments> result = new ArrayList<Arguments>();
		for (Arguments args : InstructionInfoTests.test16_InstructionInfo_Data())
			result.add(args);
		for (Arguments args : InstructionInfoTests.test32_InstructionInfo_Data())
			result.add(args);
		for (Arguments args : InstructionInfoTests.test64_InstructionInfo_Data())
			result.add(args);
		return result;
	}
}
