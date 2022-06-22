// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.instr;

import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;

import com.github.icedland.iced.x86.DecoderConstants;
import com.github.icedland.iced.x86.HexUtils;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.dec.ByteArrayCodeReader;
import com.github.icedland.iced.x86.dec.Decoder;

final class GetVirtualAddressTests {
	@ParameterizedTest
	@MethodSource("vaTestsData")
	void VATests(VirtualAddressTestCase tc) {
		Decoder decoder = new Decoder(tc.bitness, new ByteArrayCodeReader(HexUtils.toByteArray(tc.hexBytes)), tc.decoderOptions);
		switch (tc.bitness) {
		case 16:
			decoder.setIP(DecoderConstants.DEFAULT_IP16);
			break;
		case 32:
			decoder.setIP(DecoderConstants.DEFAULT_IP32);
			break;
		case 64:
			decoder.setIP(DecoderConstants.DEFAULT_IP64);
			break;
		default:
			throw new UnsupportedOperationException();
		}
		Instruction instruction = decoder.decode();
		VAGetRegisterValueImpl getRegValue = new VAGetRegisterValueImpl(tc.registerValues);
		VAGetRegisterValueImpl getRegValueFail = new VAGetRegisterValueImpl(new VARegisterValue[0]);

		Long value1 = instruction.getVirtualAddress(tc.operand, tc.elementIndex, getRegValue);
		assertNotNull(value1);
		assertEquals(tc.expectedValue, value1);

		assertNull(instruction.getVirtualAddress(tc.operand, tc.elementIndex, getRegValueFail));
	}

	public static Iterable<Arguments> vaTestsData() {
		ArrayList<Arguments> result = new ArrayList<Arguments>(VirtualAddressTestCases.tests.length);
		for (VirtualAddressTestCase tc : VirtualAddressTestCases.tests) {
			if (tc.operand >= 0)
				result.add(Arguments.of(tc));
		}
		return result;
	}
}
