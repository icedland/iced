// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.Test;

import com.github.icedland.iced.x86.HexUtils;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.dec.ByteArrayCodeReader;
import com.github.icedland.iced.x86.dec.Decoder;
import com.github.icedland.iced.x86.dec.DecoderTestInfo;
import com.github.icedland.iced.x86.dec.DecoderTestUtils;
import com.github.icedland.iced.x86.info.OpAccess;
import com.github.icedland.iced.x86.internal.IcedConstants;

final class OpIndexTests {
	@Test
	void gas_test() {
		opIndexTests(GasFormatterFactory.create());
	}

	@Test
	void intel_test() {
		opIndexTests(IntelFormatterFactory.create());
	}

	@Test
	void masm_test() {
		opIndexTests(MasmFormatterFactory.create());
	}

	@Test
	void nasm_test() {
		opIndexTests(NasmFormatterFactory.create());
	}

	private void opIndexTests(Formatter formatter) {
		int[] instrToFormatter = new int[IcedConstants.MAX_OP_COUNT];
		for (DecoderTestInfo tc : DecoderTestUtils.getDecoderTests(true, false)) {
			Decoder decoder = new Decoder(tc.bitness, new ByteArrayCodeReader(HexUtils.toByteArray(tc.hexBytes)), tc.options);
			Instruction instruction = new Instruction();
			decoder.decode(instruction);
			assertEquals(tc.code, instruction.getCode());

			for (int i = 0; i < instrToFormatter.length; i++)
				instrToFormatter[i] = -1;

			int formatterOpCount = formatter.getOperandCount(instruction);
			int instrOpCount = instruction.getOpCount();

			int instrOpUsed = 0;
			assertTrue(instrOpCount <= 32);// uint is 32 bits
			for (int formatterOpIndex = 0; formatterOpIndex < formatterOpCount; formatterOpIndex++) {
				int instrOpIndex = formatter.getInstructionOperand(instruction, formatterOpIndex);
				if (instrOpIndex >= 0) {
					assertTrue(instrOpIndex < instrOpCount);
					instrToFormatter[instrOpIndex] = formatterOpIndex;

					Integer access = formatter.tryGetOpAccess(instruction, formatterOpIndex);
					assertNull(access);

					int instrOpBit = 1 << instrOpIndex;
					assertTrue(0 == (instrOpUsed & instrOpBit), "More than one formatter operand index maps to the same instruction op index");
					instrOpUsed |= instrOpBit;

					assertEquals(formatterOpIndex, formatter.getFormatterOperand(instruction, instrOpIndex));
				}
				else {
					assertEquals(-1, instrOpIndex);
					Integer access = formatter.tryGetOpAccess(instruction, formatterOpIndex);
					assertNotNull(access);
					assertTrue(access >= OpAccess.NONE && access <= OpAccess.NO_MEM_ACCESS);
				}
			}

			for (int instrOpIndex = 0; instrOpIndex < instrOpCount; instrOpIndex++) {
				int formatterOpIndex = formatter.getFormatterOperand(instruction, instrOpIndex);
				assertEquals(instrToFormatter[instrOpIndex], formatterOpIndex);
			}

			for (int instrOpIndex = instrOpCount; instrOpIndex < IcedConstants.MAX_OP_COUNT; instrOpIndex++) {
				final int instrOpIndex2 = instrOpIndex;
				assertThrows(IllegalArgumentException.class, () -> formatter.getFormatterOperand(instruction, instrOpIndex2));
			}
		}
	}
}
