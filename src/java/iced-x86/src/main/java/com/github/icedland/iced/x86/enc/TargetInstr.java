// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

final class TargetInstr {
	final Instr instruction;
	final long address;

	public TargetInstr(Instr instruction) {
		this.instruction = instruction;
		address = 0;
	}

	public TargetInstr(long address) {
		instruction = null;
		this.address = address;
	}

	public boolean isInBlock(Block block) {
		return instruction != null && instruction.block == block;
	}

	public long getAddress() {
		Instr instruction = this.instruction;
		if (instruction == null)
			return address;
		return instruction.ip;
	}
}
