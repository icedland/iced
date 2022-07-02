// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.*;

import com.github.icedland.iced.x86.CodeUtils;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.ToCode;
import com.github.icedland.iced.x86.dec.Decoder;
import com.github.icedland.iced.x86.dec.DecoderOptions;
import com.github.icedland.iced.x86.dec.NonDecodedInstructions;
import com.github.icedland.iced.x86.dec.NonDecodedTestCase;
import com.github.icedland.iced.x86.internal.IcedConstants;

public final class Misc2Tests {
	static final class TupleIntBool {
		public final int value;
		public final boolean b;

		TupleIntBool(int value, boolean b) {
			this.value = value;
			this.b = b;
		}
	}

	@Test
	void make_sure_all_Code_values_are_formatted() {
		byte[] tested = new byte[IcedConstants.CODE_ENUM_COUNT];

		TupleIntBool[] allArgs = new TupleIntBool[] {
			new TupleIntBool(16, false),
			new TupleIntBool(32, false),
			new TupleIntBool(64, false),
			new TupleIntBool(16, true),
			new TupleIntBool(32, true),
			new TupleIntBool(64, true),
		};
		for (TupleIntBool args : allArgs) {
			FormatterTestCases.Info data = FormatterTestCases.getTests(args.value, args.b);
			for (FormatterTestCase tc : data.tests)
				tested[tc.code] = 1;
		}
		for (NonDecodedTestCase tc : NonDecodedInstructions.getTests())
			tested[tc.instruction.getCode()] = 1;

		StringBuilder sb = new StringBuilder();
		int missing = 0;
		String[] codeNames = ToCode.names();
		for (int i = 0; i < tested.length; i++) {
			if (tested[i] != 1 && !CodeUtils.isIgnored(codeNames[i])) {
				sb.append(codeNames[i] + " ");
				missing++;
			}
		}
		assertEquals("Fmt: 0 ins ", String.format("Fmt: %s ins %s", missing, sb));
	}

	@Test
	void instruction_ToString() {
		Decoder decoder = new Decoder(64, new byte[] { 0x00, (byte)0xCE }, DecoderOptions.NONE);
		Instruction instr = decoder.decode();
		assertEquals("add dh,cl", instr.toString());
	}
}
