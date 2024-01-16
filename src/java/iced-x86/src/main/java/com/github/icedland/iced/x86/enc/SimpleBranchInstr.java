// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.ConstantOffsets;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.internal.IcedConstants;

final class SimpleBranchInstr extends Instr {
	private final byte bitness;
	private Instruction instruction;
	private TargetInstr targetInstr;
	private BlockData pointerData;
	private byte instrKind;
	private final byte shortInstructionSize;
	private final byte nearInstructionSize;
	private final byte longInstructionSize;
	private final byte nativeInstructionSize;
	private final short nativeCode;

	private static final class InstrKind {
		static final byte UNCHANGED = 0;
		static final byte SHORT = 1;
		static final byte NEAR = 2;
		static final byte LONG = 3;
		static final byte UNINITIALIZED = 4;
	}

	public SimpleBranchInstr(BlockEncoder blockEncoder, Block block, Instruction instruction) {
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
			longInstructionSize = 0;
			nativeInstructionSize = 0;
			nativeCode = 0;
		}
		else {
			instrCopy = instruction.copy();
			instrCopy.setNearBranch64(0);
			shortInstructionSize = (byte)blockEncoder.getInstructionSize(instrCopy, 0);

			nativeCode = (short)toNativeBranchCode(instruction.getCode(), blockEncoder.getBitness());
			if (nativeCode == instruction.getCode())
				nativeInstructionSize = shortInstructionSize;
			else {
				instrCopy = instruction.copy();
				instrCopy.setCode(nativeCode & 0xFFFF);
				instrCopy.setNearBranch64(0);
				nativeInstructionSize = (byte)blockEncoder.getInstructionSize(instrCopy, 0);
			}

			switch (blockEncoder.getBitness()) {
			case 16:
				nearInstructionSize = (byte)(nativeInstructionSize + 2 + 3);
				break;
			case 32:
			case 64:
				nearInstructionSize = (byte)(nativeInstructionSize + 2 + 5);
				break;
			default:
				throw new UnsupportedOperationException();
			}

			if (blockEncoder.getBitness() == 64) {
				longInstructionSize = (byte)(nativeInstructionSize + 2 + CALL_OR_JMP_POINTER_DATA_INSTRUCTION_SIZE64);
				size = Math.max(Math.max(shortInstructionSize, nearInstructionSize), longInstructionSize);
			}
			else {
				longInstructionSize = 0;
				size = Math.max(shortInstructionSize, nearInstructionSize);
			}
		}
	}

	static int toNativeBranchCode(int code, int bitness) {
		int c16, c32, c64;
		switch (code) {
		case Code.LOOPNE_REL8_16_CX:
		case Code.LOOPNE_REL8_32_CX:
			c16 = Code.LOOPNE_REL8_16_CX;
			c32 = Code.LOOPNE_REL8_32_CX;
			c64 = Code.INVALID;
			break;

		case Code.LOOPNE_REL8_16_ECX:
		case Code.LOOPNE_REL8_32_ECX:
		case Code.LOOPNE_REL8_64_ECX:
			c16 = Code.LOOPNE_REL8_16_ECX;
			c32 = Code.LOOPNE_REL8_32_ECX;
			c64 = Code.LOOPNE_REL8_64_ECX;
			break;

		case Code.LOOPNE_REL8_16_RCX:
		case Code.LOOPNE_REL8_64_RCX:
			c16 = Code.LOOPNE_REL8_16_RCX;
			c32 = Code.INVALID;
			c64 = Code.LOOPNE_REL8_64_RCX;
			break;

		case Code.LOOPE_REL8_16_CX:
		case Code.LOOPE_REL8_32_CX:
			c16 = Code.LOOPE_REL8_16_CX;
			c32 = Code.LOOPE_REL8_32_CX;
			c64 = Code.INVALID;
			break;

		case Code.LOOPE_REL8_16_ECX:
		case Code.LOOPE_REL8_32_ECX:
		case Code.LOOPE_REL8_64_ECX:
			c16 = Code.LOOPE_REL8_16_ECX;
			c32 = Code.LOOPE_REL8_32_ECX;
			c64 = Code.LOOPE_REL8_64_ECX;
			break;

		case Code.LOOPE_REL8_16_RCX:
		case Code.LOOPE_REL8_64_RCX:
			c16 = Code.LOOPE_REL8_16_RCX;
			c32 = Code.INVALID;
			c64 = Code.LOOPE_REL8_64_RCX;
			break;

		case Code.LOOP_REL8_16_CX:
		case Code.LOOP_REL8_32_CX:
			c16 = Code.LOOP_REL8_16_CX;
			c32 = Code.LOOP_REL8_32_CX;
			c64 = Code.INVALID;
			break;

		case Code.LOOP_REL8_16_ECX:
		case Code.LOOP_REL8_32_ECX:
		case Code.LOOP_REL8_64_ECX:
			c16 = Code.LOOP_REL8_16_ECX;
			c32 = Code.LOOP_REL8_32_ECX;
			c64 = Code.LOOP_REL8_64_ECX;
			break;

		case Code.LOOP_REL8_16_RCX:
		case Code.LOOP_REL8_64_RCX:
			c16 = Code.LOOP_REL8_16_RCX;
			c32 = Code.INVALID;
			c64 = Code.LOOP_REL8_64_RCX;
			break;

		case Code.JCXZ_REL8_16:
		case Code.JCXZ_REL8_32:
			c16 = Code.JCXZ_REL8_16;
			c32 = Code.JCXZ_REL8_32;
			c64 = Code.INVALID;
			break;

		case Code.JECXZ_REL8_16:
		case Code.JECXZ_REL8_32:
		case Code.JECXZ_REL8_64:
			c16 = Code.JECXZ_REL8_16;
			c32 = Code.JECXZ_REL8_32;
			c64 = Code.JECXZ_REL8_64;
			break;

		case Code.JRCXZ_REL8_16:
		case Code.JRCXZ_REL8_64:
			c16 = Code.JRCXZ_REL8_16;
			c32 = Code.INVALID;
			c64 = Code.JRCXZ_REL8_64;
			break;

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
		Instruction instr;
		int size;
		switch (instrKind) {
		case InstrKind.UNCHANGED:
		case InstrKind.SHORT:
			result.isOriginalInstruction = true;
			instr = instruction.copy();
			instr.setNearBranch64(targetInstr.getAddress());
			encResult = encoder.tryEncode(instr, ip);
			if (encResult instanceof String)
				return createErrorMessage((String)encResult, instr);
			result.constantOffsets = encoder.getConstantOffsets();
			return null;

		case InstrKind.NEAR:
			result.isOriginalInstruction = false;
			result.constantOffsets = new ConstantOffsets();

			// Code:
			//		brins tmp		; nativeInstructionSize
			//		jmp short skip	; 2
			//	tmp:
			//		jmp near target	; 3/5/5
			//	skip:

			instr = instruction.copy();
			instr.setCode(nativeCode & 0xFFFF);
			instr.setNearBranch64(ip + nativeInstructionSize + 2);
			encResult = encoder.tryEncode(instr, ip);
			if (encResult instanceof String)
				return createErrorMessage((String)encResult, instruction);
			size = ((Integer)encResult).intValue();

			instr = new Instruction();
			instr.setNearBranch64(ip + nearInstructionSize);
			int codeNear;
			switch (encoder.getBitness()) {
			case 16:
				instr.setCode(Code.JMP_REL8_16);
				codeNear = Code.JMP_REL16;
				instr.setOp0Kind(OpKind.NEAR_BRANCH16);
				break;

			case 32:
				instr.setCode(Code.JMP_REL8_32);
				codeNear = Code.JMP_REL32_32;
				instr.setOp0Kind(OpKind.NEAR_BRANCH32);
				break;

			case 64:
				instr.setCode(Code.JMP_REL8_64);
				codeNear = Code.JMP_REL32_64;
				instr.setOp0Kind(OpKind.NEAR_BRANCH64);
				break;

			default:
				throw new UnsupportedOperationException();
			}
			encResult = encoder.tryEncode(instr, ip + size);
			if (encResult instanceof String)
				return createErrorMessage((String)encResult, instruction);
			size += ((Integer)encResult).intValue();

			instr.setCode(codeNear);
			instr.setNearBranch64(targetInstr.getAddress());
			encResult = encoder.tryEncode(instr, ip + size);
			if (encResult instanceof String)
				return createErrorMessage((String)encResult, instruction);
			return null;

		case InstrKind.LONG:
			assert encoder.getBitness() == 64 : encoder.getBitness();
			assert pointerData != null;
			result.isOriginalInstruction = false;
			result.constantOffsets = new ConstantOffsets();
			pointerData.data = targetInstr.getAddress();

			// Code:
			//		brins tmp		; nativeInstructionSize
			//		jmp short skip	; 2
			//	tmp:
			//		jmp [mem_loc]	; 6
			//	skip:

			instr = instruction.copy();
			instr.setCode(nativeCode & 0xFFFF);
			instr.setNearBranch64(ip + nativeInstructionSize + 2);
			encResult = encoder.tryEncode(instr, ip);
			if (encResult instanceof String)
				return createErrorMessage((String)encResult, instruction);
			size = ((Integer)encResult).intValue();

			instr = new Instruction();
			instr.setNearBranch64(ip + longInstructionSize);
			switch (encoder.getBitness()) {
			case 16:
				instr.setCode(Code.JMP_REL8_16);
				instr.setOp0Kind(OpKind.NEAR_BRANCH16);
				break;

			case 32:
				instr.setCode(Code.JMP_REL8_32);
				instr.setOp0Kind(OpKind.NEAR_BRANCH32);
				break;

			case 64:
				instr.setCode(Code.JMP_REL8_64);
				instr.setOp0Kind(OpKind.NEAR_BRANCH64);
				break;

			default:
				throw new UnsupportedOperationException();
			}
			encResult = encoder.tryEncode(instr, ip + size);
			if (encResult instanceof String)
				return createErrorMessage((String)encResult, instruction);
			size += ((Integer)encResult).intValue();

			encResult = encodeBranchToPointerData(encoder, false, ip + size, pointerData, this.size - size);
			if (encResult instanceof String)
				return createErrorMessage((String)encResult, instruction);
			return null;

		case InstrKind.UNINITIALIZED:
		default:
			throw new UnsupportedOperationException();
		}
	}
}
