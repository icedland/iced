// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.asm;

import org.junit.jupiter.api.Test;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.ICRegisters;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.MemoryOperand;

final class CodeAssembler16Tests extends CodeAssemblerTestsBase {
	private CodeAssembler16Tests() {
		super(16);
	}

	@Test
	void xlatb() {
		testAssembler(c -> c.xlatb(), Instruction.create(Code.XLAT_M8, new MemoryOperand(ICRegisters.bx, ICRegisters.al)));
	}

	@Test
	void call_far() {
		testAssembler(c -> c.call(0x1234, 0x5678), Instruction.createBranch(Code.CALL_PTR1616, 0x1234, 0x5678));
	}

	@Test
	void jmp_far() {
		testAssembler(c -> c.jmp(0x1234, 0x5678), Instruction.createBranch(Code.JMP_PTR1616, 0x1234, 0x5678));
	}

	@Test
	public void xbegin_label() {
		testAssembler(c -> c.xbegin(createAndEmitLabel(c)), assignLabel(Instruction.createXbegin(getBitness(), FIRST_LABEL_ID), FIRST_LABEL_ID),
				TestInstrFlags.BRANCH);
	}

	@Test
	public void xbegin_offset() {
		testAssembler(c -> c.xbegin(12752), Instruction.createXbegin(getBitness(), 12752), TestInstrFlags.BRANCH_U64);
	}
}
