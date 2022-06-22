// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import org.junit.jupiter.api.*;
import static org.junit.jupiter.api.Assertions.*;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeUtils;
import com.github.icedland.iced.x86.ToCode;
import com.github.icedland.iced.x86.internal.IcedConstants;

public final class CodeValueTests {
	public static final int[] NonDecodedCodeValues1632 = new int[] {
		Code.POPW_CS,
		Code.FSTDW_AX,
		Code.FSTSG_AX,
	};
	public static final int[] NonDecodedCodeValues = new int[] {
		Code.DECLAREBYTE,
		Code.DECLAREDWORD,
		Code.DECLAREQWORD,
		Code.DECLAREWORD,
		Code.ZERO_BYTES,
		Code.FCLEX,
		Code.FDISI,
		Code.FENI,
		Code.FINIT,
		Code.FSAVE_M108BYTE,
		Code.FSAVE_M94BYTE,
		Code.FSETPM,
		Code.FSTCW_M2BYTE,
		Code.FSTENV_M14BYTE,
		Code.FSTENV_M28BYTE,
		Code.FSTSW_AX,
		Code.FSTSW_M2BYTE,
	};

	@Test
	void make_sure_all_code_values_are_tested_in_16_32_64_bit_modes() {
		final byte T16 = 0x01;
		final byte T32 = 0x02;
		final byte T64 = 0x04;
		byte[] tested = new byte[IcedConstants.CODE_ENUM_COUNT];
		tested[Code.INVALID] = T16 | T32 | T64;

		for (DecoderTestInfo info : DecoderTestUtils.getDecoderTests(false, false)) {
			assertFalse(DecoderTestUtils.notDecoded.contains(info.code),
					String.format("Code %d has a decoder test but it shouldn't be decoded", info.code));

			int flags;
			switch (info.bitness) {
			case 16:
				flags = T16;
				break;
			case 32:
				flags = T32;
				break;
			case 64:
				flags = T64;
				break;
			default:
				throw new UnsupportedOperationException();
			}
			tested[info.code] |= flags;
		}

		for (NonDecodedTestCase info : NonDecodedInstructions.getTests()) {
			int flags;
			switch (info.bitness) {
			case 16:
				flags = T16;
				break;
			case 32:
				flags = T32;
				break;
			case 64:
				flags = T64;
				break;
			default:
				throw new UnsupportedOperationException();
			}
			tested[info.instruction.getCode()] |= flags;
		}

		for (Integer c : DecoderTestUtils.notDecoded) {
			assertFalse(DecoderTestUtils.code32Only.contains(c));
			assertFalse(DecoderTestUtils.code64Only.contains(c));
		}

		for (Integer c : DecoderTestUtils.notDecoded32Only)
			tested[c] ^= T64;
		for (Integer c : DecoderTestUtils.notDecoded64Only)
			tested[c] ^= T16 | T32;

		for (Integer c : DecoderTestUtils.code32Only) {
			assertFalse(DecoderTestUtils.code64Only.contains(c));
			tested[c] ^= T64;
		}

		for (Integer c : DecoderTestUtils.code64Only) {
			assertFalse(DecoderTestUtils.code32Only.contains(c));
			tested[c] ^= T16 | T32;
		}

		StringBuilder sb16 = new StringBuilder();
		StringBuilder sb32 = new StringBuilder();
		StringBuilder sb64 = new StringBuilder();
		int missing16 = 0, missing32 = 0, missing64 = 0;
		String[] codeNames = ToCode.names();
		assertEquals(tested.length, codeNames.length);
		for (int i = 0; i < tested.length; i++) {
			if (tested[i] != (T16 | T32 | T64) && !CodeUtils.isIgnored(codeNames[i])) {
				if ((tested[i] & T16) == 0) {
					sb16.append(codeNames[i] + " ");
					missing16++;
				}
				if ((tested[i] & T32) == 0) {
					sb32.append(codeNames[i] + " ");
					missing32++;
				}
				if ((tested[i] & T64) == 0) {
					sb64.append(codeNames[i] + " ");
					missing64++;
				}
			}
		}
		assertEquals("16: 0 ins ", String.format("16: %d ins %s", missing16, sb16));
		assertEquals("32: 0 ins ", String.format("32: %d ins %s", missing32, sb32));
		assertEquals("64: 0 ins ", String.format("64: %d ins %s", missing64, sb64));
	}
}
