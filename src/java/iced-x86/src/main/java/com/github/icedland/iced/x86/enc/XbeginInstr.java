// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.Instruction;

final class XbeginInstr extends Instr {
	private Instruction instruction;
	private TargetInstr targetInstr;
	private byte instrKind;
	private final byte shortInstructionSize;
	private final byte nearInstructionSize;

	private static final class InstrKind {
		static final byte UNCHANGED = 0;
		static final byte REL16 = 1;
		static final byte REL32 = 2;
		static final byte UNINITIALIZED = 3;
	}

	XbeginInstr(BlockEncoder blockEncoder, Block block, Instruction instruction) {
		super(block, instruction.getIP());
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
			instrCopy.setCode(Code.XBEGIN_REL16);
			instrCopy.setNearBranch64(0);
			shortInstructionSize = (byte)blockEncoder.getInstructionSize(instrCopy, 0);

			instrCopy.setCode(Code.XBEGIN_REL32);
			instrCopy.setNearBranch64(0);
			nearInstructionSize = (byte)blockEncoder.getInstructionSize(instrCopy, 0);

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
		if (instrKind == InstrKind.UNCHANGED || instrKind == InstrKind.REL16) {
			done = true;
			return false;
		}

		long targetAddress = targetInstr.getAddress();
		long nextRip = ip + shortInstructionSize;
		long diff = targetAddress - nextRip;
		diff = correctDiff(targetInstr.isInBlock(block), diff, gained);
		if (-0x8000 <= diff && diff <= 0x7FFF) {
			instrKind = InstrKind.REL16;
			size = shortInstructionSize;
			return true;
		}

		instrKind = InstrKind.REL32;
		size = nearInstructionSize;
		return false;
	}

	@Override
	String tryEncode(Encoder encoder, TryEncodeResult result) {
		switch (instrKind) {
		case InstrKind.UNCHANGED:
		case InstrKind.REL16:
		case InstrKind.REL32:
			result.isOriginalInstruction = true;
			Instruction instruction = this.instruction.copy();
			if (instrKind == InstrKind.UNCHANGED) {
				// nothing
			}
			else if (instrKind == InstrKind.REL16)
				instruction.setCode(Code.XBEGIN_REL16);
			else {
				assert instrKind == InstrKind.REL32 : instrKind;
				instruction.setCode(Code.XBEGIN_REL32);
			}
			instruction.setNearBranch64(targetInstr.getAddress());
			Object encResult = encoder.tryEncode(instruction, ip);
			if (encResult instanceof String) {
				return createErrorMessage((String)encResult, instruction);
			}
			result.constantOffsets = encoder.getConstantOffsets();
			return null;

		case InstrKind.UNINITIALIZED:
		default:
			throw new UnsupportedOperationException();
		}
	}
}
