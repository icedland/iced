// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.ConstantOffsets;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.internal.IcedConstants;

final class JmpInstr extends Instr {
	private final byte bitness;
	private Instruction instruction;
	private TargetInstr targetInstr;
	private BlockData pointerData;
	private byte instrKind;
	private final byte shortInstructionSize;
	private final byte nearInstructionSize;

	private static final class InstrKind {
		static final byte UNCHANGED = 0;
		static final byte SHORT = 1;
		static final byte NEAR = 2;
		static final byte LONG = 3;
		static final byte UNINITIALIZED = 4;
	}

	public JmpInstr(BlockEncoder blockEncoder, Block block, Instruction instruction) {
		super(block, instruction.getIP());
		this.bitness = (byte)blockEncoder.getBitness();
		this.instruction = instruction;
		instrKind = InstrKind.UNINITIALIZED;

		Instruction instrCopy;

		if (!blockEncoder.fixBranches()) {
			instrKind = InstrKind.UNCHANGED;
			instrCopy = instruction.copy();
			instrCopy.setNearBranch64(0);
			size = blockEncoder.getInstructionSize(instrCopy, 0);
			shortInstructionSize = 0;
			nearInstructionSize = 0;
		}
		else {
			instrCopy = instruction.copy();
			instrCopy.setCode(Code.toShortBranch(instruction.getCode()));
			instrCopy.setNearBranch64(0);
			shortInstructionSize = (byte)blockEncoder.getInstructionSize(instrCopy, 0);

			instrCopy.setCode(Code.toNearBranch(instruction.getCode()));
			instrCopy.setNearBranch64(0);
			nearInstructionSize = (byte)blockEncoder.getInstructionSize(instrCopy, 0);

			if (blockEncoder.getBitness() == 64) {
				// Make sure it's not shorter than the real instruction. It can happen if there are extra prefixes.
				size = Math.max(nearInstructionSize, CALL_OR_JMP_POINTER_DATA_INSTRUCTION_SIZE64);
			}
			else
				size = nearInstructionSize;
		}
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
		if (instrKind == InstrKind.UNCHANGED || instrKind == InstrKind.SHORT) {
			done = true;
			return false;
		}

		long targetAddress = targetInstr.getAddress();
		long nextRip = ip + shortInstructionSize;
		long diff = targetAddress - nextRip;
		diff = convertDiffToBitnessDiff(bitness, correctDiff(targetInstr.isInBlock(block), diff, gained));
		if (-0x80 <= diff && diff <= 0x7F) {
			if (pointerData != null)
				pointerData.isValid = false;
			instrKind = InstrKind.SHORT;
			size = shortInstructionSize;
			done = true;
			return true;
		}

		// If it's in the same block, we assume the target is at most 2GB away.
		boolean useNear = bitness != 64 || targetInstr.isInBlock(block);
		if (!useNear) {
			targetAddress = targetInstr.getAddress();
			nextRip = ip + nearInstructionSize;
			diff = targetAddress - nextRip;
			diff = convertDiffToBitnessDiff(bitness, correctDiff(targetInstr.isInBlock(block), diff, gained));
			useNear = -0x8000_0000 <= diff && diff <= 0x7FFF_FFFF;
		}
		if (useNear) {
			if (pointerData != null)
				pointerData.isValid = false;
			if (diff < IcedConstants.MAX_INSTRUCTION_LENGTH * -0x80 || diff > IcedConstants.MAX_INSTRUCTION_LENGTH * 0x7F) {
				done = true;
			}
			instrKind = InstrKind.NEAR;
			size = nearInstructionSize;
			return true;
		}

		if (pointerData == null)
			pointerData = block.allocPointerLocation();
		instrKind = InstrKind.LONG;
		return false;
	}

	@Override
	String tryEncode(Encoder encoder, TryEncodeResult result) {
		Object encResult;
		Instruction instruction;
		switch (instrKind) {
		case InstrKind.UNCHANGED:
		case InstrKind.SHORT:
		case InstrKind.NEAR:
			result.isOriginalInstruction = true;
			instruction = this.instruction.copy();
			if (instrKind == InstrKind.UNCHANGED) {
				// nothing
			}
			else if (instrKind == InstrKind.SHORT)
				instruction.setCode(Code.toShortBranch(instruction.getCode()));
			else {
				assert instrKind == InstrKind.NEAR : instrKind;
				instruction.setCode(Code.toNearBranch(instruction.getCode()));
			}
			instruction.setNearBranch64(targetInstr.getAddress());
			encResult = encoder.tryEncode(instruction, ip);
			if (encResult instanceof String)
				return createErrorMessage((String)encResult, instruction);
			result.constantOffsets = encoder.getConstantOffsets();
			return null;

		case InstrKind.LONG:
			assert pointerData != null;
			result.isOriginalInstruction = false;
			result.constantOffsets = new ConstantOffsets();
			pointerData.data = targetInstr.getAddress();
			encResult = encodeBranchToPointerData(encoder, false, ip, pointerData, size);
			if (encResult instanceof String)
				return createErrorMessage((String)encResult, this.instruction);
			return null;

		case InstrKind.UNINITIALIZED:
		default:
			throw new UnsupportedOperationException();
		}
	}
}
