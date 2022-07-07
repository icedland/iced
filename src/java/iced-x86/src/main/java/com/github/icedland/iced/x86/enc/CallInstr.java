// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import com.github.icedland.iced.x86.ConstantOffsets;
import com.github.icedland.iced.x86.Instruction;

final class CallInstr extends Instr {
	private final byte bitness;
	private Instruction instruction;
	private TargetInstr targetInstr;
	private final byte origInstructionSize;
	private BlockData pointerData;
	private boolean useOrigInstruction;

	public CallInstr(BlockEncoder blockEncoder, Block block, Instruction instruction) {
		super(block, instruction.getIP());
		bitness = (byte)blockEncoder.getBitness();
		this.instruction = instruction;
		Instruction instrCopy = instruction.copy();
		instrCopy.setNearBranch64(0);
		origInstructionSize = (byte)blockEncoder.getInstructionSize(instrCopy, 0);
		if (!blockEncoder.fixBranches()) {
			size = origInstructionSize;
			useOrigInstruction = true;
		}
		else if (blockEncoder.getBitness() == 64) {
			// Make sure it's not shorter than the real instruction. It can happen if there are extra prefixes.
			size = Math.max(origInstructionSize, CALL_OR_JMP_POINTER_DATA_INSTRUCTION_SIZE64);
		}
		else
			size = origInstructionSize;
	}

	@Override
	void initialize(BlockEncoder blockEncoder) {
		targetInstr = blockEncoder.getTarget(instruction.getNearBranchTarget());
	}

	@Override
	boolean optimize(long gained) {
		return tryOptimize(gained);
	}

	private boolean tryOptimize(long gained) {
		if (done || useOrigInstruction) {
			done = true;
			return false;
		}

		// If it's in the same block, we assume the target is at most 2GB away.
		boolean useShort = bitness != 64 || targetInstr.isInBlock(block);
		if (!useShort) {
			long targetAddress = targetInstr.getAddress();
			long nextRip = ip + origInstructionSize;
			long diff = targetAddress - nextRip;
			diff = correctDiff(targetInstr.isInBlock(block), diff, gained);
			useShort = -0x8000_0000 <= diff && diff <= 0x7FFF_FFFF;
		}

		if (useShort) {
			if (pointerData != null)
				pointerData.isValid = false;
			size = origInstructionSize;
			useOrigInstruction = true;
			done = true;
			return true;
		}

		if (pointerData == null)
			pointerData = block.allocPointerLocation();
		return false;
	}

	@Override
	String tryEncode(Encoder encoder, TryEncodeResult result) {
		if (useOrigInstruction) {
			result.isOriginalInstruction = true;
			Instruction instruction = this.instruction.copy();
			instruction.setNearBranch64(targetInstr.getAddress());
			Object encResult = encoder.tryEncode(instruction, ip);
			if (encResult instanceof String) {
				return createErrorMessage((String)encResult, instruction);
			}
			result.constantOffsets = encoder.getConstantOffsets();
			return null;
		}
		else {
			assert pointerData != null;
			result.isOriginalInstruction = false;
			result.constantOffsets = new ConstantOffsets();
			pointerData.data = targetInstr.getAddress();
			Object encResult = encodeBranchToPointerData(encoder, true, ip, pointerData, size);
			if (encResult instanceof String)
				return createErrorMessage((String)encResult, instruction);
			return null;
		}
	}
}
