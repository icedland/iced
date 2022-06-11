// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import com.github.icedland.iced.x86.Instruction;

final class SimpleInstr extends Instr {
	private Instruction instruction;

	SimpleInstr(BlockEncoder blockEncoder, Block block, Instruction instruction) {
		super(block, instruction.getIP());
		done = true;
		this.instruction = instruction;
		size = blockEncoder.getInstructionSize(instruction, instruction.getIP());
	}

	@Override
	void initialize(BlockEncoder blockEncoder) {
	}

	@Override
	boolean optimize(long gained) {
		return false;
	}

	@Override
	String tryEncode(Encoder encoder, TryEncodeResult result) {
		result.isOriginalInstruction = true;
		Object encResult = encoder.tryEncode(instruction, ip);
		if (encResult instanceof Integer) {
			result.constantOffsets = encoder.getConstantOffsets();
			return null;
		}
		return createErrorMessage((String)encResult, instruction);
	}
}
