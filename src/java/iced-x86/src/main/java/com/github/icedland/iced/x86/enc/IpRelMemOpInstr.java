// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.Register;

final class IpRelMemOpInstr extends Instr {
	private Instruction instruction;
	private byte instrKind;
	private final byte eipInstructionSize;
	private final byte ripInstructionSize;
	private TargetInstr targetInstr;

	private static final class InstrKind {
		static final byte UNCHANGED = 0;
		static final byte RIP = 1;
		static final byte EIP = 2;
		static final byte LONG = 3;
		static final byte UNINITIALIZED = 4;
	}

	IpRelMemOpInstr(BlockEncoder blockEncoder, Block block, Instruction instruction) {
		super(block, instruction.getIP());
		assert instruction.isIPRelativeMemoryOperand();
		this.instruction = instruction;
		instrKind = InstrKind.UNINITIALIZED;

		Instruction instrCopy = instruction.copy();
		instrCopy.setMemoryBase(Register.RIP);
		instrCopy.setMemoryDisplacement64(0);
		ripInstructionSize = (byte)blockEncoder.getInstructionSize(instrCopy, instrCopy.ipRelativeMemoryAddress());

		instrCopy.setMemoryBase(Register.EIP);
		eipInstructionSize = (byte)blockEncoder.getInstructionSize(instrCopy, instrCopy.ipRelativeMemoryAddress());

		assert eipInstructionSize >= ripInstructionSize;
		size = eipInstructionSize;
	}

	@Override
	void initialize(BlockEncoder blockEncoder) {
		targetInstr = blockEncoder.getTarget(instruction.ipRelativeMemoryAddress());
	}

	@Override
	boolean optimize(long gained) {
		return tryOptimize(gained);
	}

	private boolean tryOptimize(long gained) {
		if (instrKind == InstrKind.UNCHANGED || instrKind == InstrKind.RIP || instrKind == InstrKind.EIP) {
			done = true;
			return false;
		}

		// If it's in the same block, we assume the target is at most 2GB away.
		boolean useRip = targetInstr.isInBlock(block);
		long targetAddress = targetInstr.getAddress();
		if (!useRip) {
			long nextRip = ip + ripInstructionSize;
			long diff = targetAddress - nextRip;
			diff = correctDiff(targetInstr.isInBlock(block), diff, gained);
			useRip = -0x8000_0000 <= diff && diff <= 0x7FFF_FFFF;
		}

		if (useRip) {
			size = ripInstructionSize;
			instrKind = InstrKind.RIP;
			done = true;
			return true;
		}

		// If it's in the low 4GB we can use EIP relative addressing
		if (Long.compareUnsigned(targetAddress, 0xFFFF_FFFFL) <= 0) {
			size = eipInstructionSize;
			instrKind = InstrKind.EIP;
			done = true;
			return true;
		}

		instrKind = InstrKind.LONG;
		return false;
	}

	@Override
	String tryEncode(Encoder encoder, TryEncodeResult result) {
		switch (instrKind) {
		case InstrKind.UNCHANGED:
		case InstrKind.RIP:
		case InstrKind.EIP:
			result.isOriginalInstruction = true;

			Instruction instruction = this.instruction.copy();
			if (instrKind == InstrKind.RIP)
				instruction.setMemoryBase(Register.RIP);
			else if (instrKind == InstrKind.EIP)
				instruction.setMemoryBase(Register.EIP);
			else
				assert instrKind == InstrKind.UNCHANGED : instrKind;

			long targetAddress = targetInstr.getAddress();
			instruction.setMemoryDisplacement64(targetAddress);
			Object encResult = encoder.tryEncode(instruction, ip);
			boolean b = instruction
					.ipRelativeMemoryAddress() == (instruction.getMemoryBase() == Register.EIP ? targetAddress & 0xFFFF_FFFFL : targetAddress);
			assert b;
			if (!b)
				encResult = "Invalid IP relative address";
			if (encResult instanceof String)
				return createErrorMessage((String)encResult, instruction);
			result.constantOffsets = encoder.getConstantOffsets();
			return null;

		case InstrKind.LONG:
			return "IP relative memory operand is too far away and isn't currently supported. " +
					"Try to allocate memory close to the original instruction (+/-2GB).";

		case InstrKind.UNINITIALIZED:
		default:
			throw new UnsupportedOperationException();
		}
	}
}
