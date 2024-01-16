// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.ConstantOffsets;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.internal.IcedConstants;

final class JccInstr extends Instr {
	private final byte bitness;
	private Instruction instruction;
	private TargetInstr targetInstr;
	private BlockData pointerData;
	private byte instrKind;
	private final byte shortInstructionSize;
	private final byte nearInstructionSize;
	private final byte longInstructionSize64;

	private static int getLongInstructionSize64(Instruction instruction) {
		// Check if JKZD/JKNZD
		if (instruction.getOpCount() == 2)
			return 5 + CALL_OR_JMP_POINTER_DATA_INSTRUCTION_SIZE64;
		// Code:
		//		!jcc short skip		; negated jcc opcode
		//		jmp qword ptr [rip+mem]
		//	skip:
		return 2 + CALL_OR_JMP_POINTER_DATA_INSTRUCTION_SIZE64;
	}

	private static final class InstrKind {
		static final byte UNCHANGED = 0;
		static final byte SHORT = 1;
		static final byte NEAR = 2;
		static final byte LONG = 3;
		static final byte UNINITIALIZED = 4;
	}

	JccInstr(BlockEncoder blockEncoder, Block block, Instruction instruction) {
		super(block, instruction.getIP());
		this.bitness = (byte)blockEncoder.getBitness();
		this.instruction = instruction;
		instrKind = InstrKind.UNINITIALIZED;
		longInstructionSize64 = (byte)getLongInstructionSize64(instruction);

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
				size = Math.max(nearInstructionSize, longInstructionSize64);
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
			instruction = this.instruction; // No copy(), we don't mutate it
			Instruction instr = new Instruction();
			instr.setCode(shortBrToNativeBr(Code.toShortBranch(Code.negateConditionCode(instruction.getCode())), encoder.getBitness()));
			if (instruction.getOpCount() == 1)
				instr.setOp0Kind(OpKind.NEAR_BRANCH64);
			else {
				assert instruction.getOpCount() == 2 : instruction.getOpCount();
				instr.setOp0Kind(OpKind.REGISTER);
				instr.setOp0Register(instruction.getOp0Register());
				instr.setOp1Kind(OpKind.NEAR_BRANCH64);
			}
			assert encoder.getBitness() == 64 : encoder.getBitness();
			assert longInstructionSize64 <= 0x7F : longInstructionSize64;
			instr.setNearBranch64(ip + longInstructionSize64);
			encResult = encoder.tryEncode(instr, ip);
			if (encResult instanceof String)
				return createErrorMessage((String)encResult, instruction);
			int instrLen = ((Integer)encResult).intValue();
			encResult = encodeBranchToPointerData(encoder, false, ip + instrLen, pointerData, size - instrLen);
			if (encResult instanceof String)
				return createErrorMessage((String)encResult, instruction);
			return null;

		case InstrKind.UNINITIALIZED:
		default:
			throw new UnsupportedOperationException();
		}
	}

	static int shortBrToNativeBr(int code, int bitness) {
		int c16, c32, c64;
		switch (code) {
		case Code.JO_REL8_16:
		case Code.JO_REL8_32:
		case Code.JO_REL8_64:
			c16 = Code.JO_REL8_16;
			c32 = Code.JO_REL8_32;
			c64 = Code.JO_REL8_64;
			break;

		case Code.JNO_REL8_16:
		case Code.JNO_REL8_32:
		case Code.JNO_REL8_64:
			c16 = Code.JNO_REL8_16;
			c32 = Code.JNO_REL8_32;
			c64 = Code.JNO_REL8_64;
			break;

		case Code.JB_REL8_16:
		case Code.JB_REL8_32:
		case Code.JB_REL8_64:
			c16 = Code.JB_REL8_16;
			c32 = Code.JB_REL8_32;
			c64 = Code.JB_REL8_64;
			break;

		case Code.JAE_REL8_16:
		case Code.JAE_REL8_32:
		case Code.JAE_REL8_64:
			c16 = Code.JAE_REL8_16;
			c32 = Code.JAE_REL8_32;
			c64 = Code.JAE_REL8_64;
			break;

		case Code.JE_REL8_16:
		case Code.JE_REL8_32:
		case Code.JE_REL8_64:
			c16 = Code.JE_REL8_16;
			c32 = Code.JE_REL8_32;
			c64 = Code.JE_REL8_64;
			break;

		case Code.JNE_REL8_16:
		case Code.JNE_REL8_32:
		case Code.JNE_REL8_64:
			c16 = Code.JNE_REL8_16;
			c32 = Code.JNE_REL8_32;
			c64 = Code.JNE_REL8_64;
			break;

		case Code.JBE_REL8_16:
		case Code.JBE_REL8_32:
		case Code.JBE_REL8_64:
			c16 = Code.JBE_REL8_16;
			c32 = Code.JBE_REL8_32;
			c64 = Code.JBE_REL8_64;
			break;

		case Code.JA_REL8_16:
		case Code.JA_REL8_32:
		case Code.JA_REL8_64:
			c16 = Code.JA_REL8_16;
			c32 = Code.JA_REL8_32;
			c64 = Code.JA_REL8_64;
			break;

		case Code.JS_REL8_16:
		case Code.JS_REL8_32:
		case Code.JS_REL8_64:
			c16 = Code.JS_REL8_16;
			c32 = Code.JS_REL8_32;
			c64 = Code.JS_REL8_64;
			break;

		case Code.JNS_REL8_16:
		case Code.JNS_REL8_32:
		case Code.JNS_REL8_64:
			c16 = Code.JNS_REL8_16;
			c32 = Code.JNS_REL8_32;
			c64 = Code.JNS_REL8_64;
			break;

		case Code.JP_REL8_16:
		case Code.JP_REL8_32:
		case Code.JP_REL8_64:
			c16 = Code.JP_REL8_16;
			c32 = Code.JP_REL8_32;
			c64 = Code.JP_REL8_64;
			break;

		case Code.JNP_REL8_16:
		case Code.JNP_REL8_32:
		case Code.JNP_REL8_64:
			c16 = Code.JNP_REL8_16;
			c32 = Code.JNP_REL8_32;
			c64 = Code.JNP_REL8_64;
			break;

		case Code.JL_REL8_16:
		case Code.JL_REL8_32:
		case Code.JL_REL8_64:
			c16 = Code.JL_REL8_16;
			c32 = Code.JL_REL8_32;
			c64 = Code.JL_REL8_64;
			break;

		case Code.JGE_REL8_16:
		case Code.JGE_REL8_32:
		case Code.JGE_REL8_64:
			c16 = Code.JGE_REL8_16;
			c32 = Code.JGE_REL8_32;
			c64 = Code.JGE_REL8_64;
			break;

		case Code.JLE_REL8_16:
		case Code.JLE_REL8_32:
		case Code.JLE_REL8_64:
			c16 = Code.JLE_REL8_16;
			c32 = Code.JLE_REL8_32;
			c64 = Code.JLE_REL8_64;
			break;

		case Code.JG_REL8_16:
		case Code.JG_REL8_32:
		case Code.JG_REL8_64:
			c16 = Code.JG_REL8_16;
			c32 = Code.JG_REL8_32;
			c64 = Code.JG_REL8_64;
			break;

		case Code.VEX_KNC_JKZD_KR_REL8_64:
		case Code.VEX_KNC_JKNZD_KR_REL8_64:
			if (bitness == 64)
				return code;
			throw new UnsupportedOperationException();

		default:
			throw new IllegalArgumentException("code");
		}

		switch (bitness) {
		case 16:
			return c16;
		case 32:
			return c32;
		case 64:
			return c64;
		default:
			throw new IllegalArgumentException("bitness");
		}
	}
}
