// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

// These access Encoder fields/methods so must be in the same package

package com.github.icedland.iced.x86.enc;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.internal.enc.EncoderFlags;
import com.github.icedland.iced.x86.internal.enc.ImmSize;

/** DO NOT USE: INTERNAL API */
public abstract class Op {
	Op() {}

	static final Op[] operands_3dnow = new Op[] {
		new OpModRM_reg(Register.MM0, Register.MM7),
		new OpModRM_rm(Register.MM0, Register.MM7),
	};

	abstract void encode(Encoder encoder, Instruction instruction, int operand);

	/**
	 * DO NOT USE: INTERNAL API
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public int getImmediateOpKind() {
		return -1;
	}

	/**
	 * DO NOT USE: INTERNAL API
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public int getNearBranchOpKind() {
		return -1;
	}

	/**
	 * DO NOT USE: INTERNAL API
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public int getFarBranchOpKind() {
		return -1;
	}
}

final class OpModRM_rm_mem_only extends Op {
	private final boolean mustUseSib;

	OpModRM_rm_mem_only(boolean mustUseSib) {
		this.mustUseSib = mustUseSib;
	}

	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		if (mustUseSib)
			encoder.encoderFlags |= EncoderFlags.MUST_USE_SIB;
		encoder.addRegOrMem(instruction, operand, Register.NONE, Register.NONE, true, false);
	}
}

final class OpModRM_rm extends Op {
	private final int regLo;
	private final int regHi;

	OpModRM_rm(int regLo, int regHi) {
		this.regLo = regLo;
		this.regHi = regHi;
	}

	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		encoder.addRegOrMem(instruction, operand, regLo, regHi, true, true);
	}
}

final class OpRegEmbed8 extends Op {
	private final int regLo;
	private final int regHi;

	OpRegEmbed8(int regLo, int regHi) {
		this.regLo = regLo;
		this.regHi = regHi;
	}

	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		encoder.addReg(instruction, operand, regLo, regHi);
	}
}

final class OpModRM_rm_reg_only extends Op {
	private final int regLo;
	private final int regHi;

	OpModRM_rm_reg_only(int regLo, int regHi) {
		this.regLo = regLo;
		this.regHi = regHi;
	}

	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		encoder.addRegOrMem(instruction, operand, regLo, regHi, false, true);
	}
}

final class OpModRM_reg extends Op {
	private final int regLo;
	private final int regHi;

	OpModRM_reg(int regLo, int regHi) {
		this.regLo = regLo;
		this.regHi = regHi;
	}

	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		encoder.addModRMRegister(instruction, operand, regLo, regHi);
	}
}

final class OpModRM_reg_mem extends Op {
	private final int regLo;
	private final int regHi;

	OpModRM_reg_mem(int regLo, int regHi) {
		this.regLo = regLo;
		this.regHi = regHi;
	}

	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		encoder.addModRMRegister(instruction, operand, regLo, regHi);
		encoder.encoderFlags |= EncoderFlags.REG_IS_MEMORY;
	}
}

final class OpModRM_regF0 extends Op {
	private final int regLo;
	private final int regHi;

	OpModRM_regF0(int regLo, int regHi) {
		this.regLo = regLo;
		this.regHi = regHi;
	}

	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		if (encoder.getBitness() != 64 && instruction.getOpKind(operand) == OpKind.REGISTER && instruction.getOpRegister(operand) >= regLo + 8
				&& instruction.getOpRegister(operand) <= regLo + 15) {
			encoder.encoderFlags |= EncoderFlags.PF0;
			encoder.addModRMRegister(instruction, operand, regLo + 8, regLo + 15);
		}
		else
			encoder.addModRMRegister(instruction, operand, regLo, regHi);
	}
}

final class OpReg extends Op {
	private final int register;

	OpReg(int register) {
		this.register = register;
	}

	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		encoder.verifyOpKind(operand, OpKind.REGISTER, instruction.getOpKind(operand));
		encoder.verifyRegister(operand, register, instruction.getOpRegister(operand));
	}
}

final class OpRegSTi extends Op {
	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		if (!encoder.verifyOpKind(operand, OpKind.REGISTER, instruction.getOpKind(operand)))
			return;
		int reg = instruction.getOpRegister(operand);
		if (!encoder.verify(operand, reg, Register.ST0, Register.ST7))
			return;
		assert (encoder.opCode & 7) == 0 : encoder.opCode;
		encoder.opCode |= (reg - Register.ST0);
	}
}

final class OprDI extends Op {
	static int getRegSize(int opKind) {
		if (opKind == OpKind.MEMORY_SEG_RDI)
			return 8;
		if (opKind == OpKind.MEMORY_SEG_EDI)
			return 4;
		if (opKind == OpKind.MEMORY_SEG_DI)
			return 2;
		return 0;
	}

	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		int regSize = getRegSize(instruction.getOpKind(operand));
		if (regSize == 0) {
			encoder.setErrorMessage(String.format("Operand %d: expected OpKind = MEMORY_SEG_DI, MEMORY_SEG_EDI or MEMORY_SEG_RDI", operand));
			return;
		}
		encoder.setAddrSize(regSize);
	}
}

final class OpIb extends Op {
	private final int opKind;

	OpIb(int opKind) {
		this.opKind = opKind;
	}

	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		switch (encoder.immSize) {
		case ImmSize.SIZE1:
			if (!encoder.verifyOpKind(operand, OpKind.IMMEDIATE8_2ND, instruction.getOpKind(operand)))
				return;
			encoder.immSize = ImmSize.SIZE1_1;
			encoder.immediateHi = instruction.getImmediate8_2nd();
			break;
		case ImmSize.SIZE2:
			if (!encoder.verifyOpKind(operand, OpKind.IMMEDIATE8_2ND, instruction.getOpKind(operand)))
				return;
			encoder.immSize = ImmSize.SIZE2_1;
			encoder.immediateHi = instruction.getImmediate8_2nd();
			break;
		default:
			int opImmKind = instruction.getOpKind(operand);
			if (!encoder.verifyOpKind(operand, opKind, opImmKind))
				return;
			encoder.immSize = ImmSize.SIZE1;
			encoder.immediate = instruction.getImmediate8();
			break;
		}
	}

	@SuppressWarnings("deprecation")
	@Override
	public int getImmediateOpKind() {
		return opKind;
	}
}

final class OpIw extends Op {
	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		if (!encoder.verifyOpKind(operand, OpKind.IMMEDIATE16, instruction.getOpKind(operand)))
			return;
		encoder.immSize = ImmSize.SIZE2;
		encoder.immediate = instruction.getImmediate16();
	}

	@SuppressWarnings("deprecation")
	@Override
	public int getImmediateOpKind() {
		return OpKind.IMMEDIATE16;
	}
}

final class OpId extends Op {
	private final int opKind;

	OpId(int opKind) {
		this.opKind = opKind;
	}

	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		int opImmKind = instruction.getOpKind(operand);
		if (!encoder.verifyOpKind(operand, opKind, opImmKind))
			return;
		encoder.immSize = ImmSize.SIZE4;
		encoder.immediate = instruction.getImmediate32();
	}

	@SuppressWarnings("deprecation")
	@Override
	public int getImmediateOpKind() {
		return opKind;
	}
}

final class OpIq extends Op {
	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		if (!encoder.verifyOpKind(operand, OpKind.IMMEDIATE64, instruction.getOpKind(operand)))
			return;
		encoder.immSize = ImmSize.SIZE8;
		long imm = instruction.getImmediate64();
		encoder.immediate = (int)imm;
		encoder.immediateHi = (int)(imm >>> 32);
	}

	@SuppressWarnings("deprecation")
	@Override
	public int getImmediateOpKind() {
		return OpKind.IMMEDIATE64;
	}
}

final class OpI4 extends Op {
	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		int opImmKind = instruction.getOpKind(operand);
		if (!encoder.verifyOpKind(operand, OpKind.IMMEDIATE8, opImmKind))
			return;
		assert encoder.immSize == ImmSize.SIZE_IB_REG : encoder.immSize;
		assert (encoder.immediate & 0xF) == 0 : encoder.immediate;
		if ((instruction.getImmediate8() & 0xFF) > 0xF) {
			encoder.setErrorMessage(
					String.format("Operand %d: Immediate value must be 0-15, but value is 0x%02X", operand, instruction.getImmediate8()));
			return;
		}
		encoder.immSize = ImmSize.SIZE1;
		encoder.immediate |= instruction.getImmediate8();
	}

	@SuppressWarnings("deprecation")
	@Override
	public int getImmediateOpKind() {
		return OpKind.IMMEDIATE8;
	}
}

final class OpX extends Op {
	static int getXRegSize(int opKind) {
		if (opKind == OpKind.MEMORY_SEG_RSI)
			return 8;
		if (opKind == OpKind.MEMORY_SEG_ESI)
			return 4;
		if (opKind == OpKind.MEMORY_SEG_SI)
			return 2;
		return 0;
	}

	static int getYRegSize(int opKind) {
		if (opKind == OpKind.MEMORY_ESRDI)
			return 8;
		if (opKind == OpKind.MEMORY_ESEDI)
			return 4;
		if (opKind == OpKind.MEMORY_ESDI)
			return 2;
		return 0;
	}

	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		int regXSize = getXRegSize(instruction.getOpKind(operand));
		if (regXSize == 0) {
			encoder.setErrorMessage(String.format("Operand %d: expected OpKind = MEMORY_SEG_SI, MEMORY_SEG_ESI or MEMORY_SEG_RSI", operand));
			return;
		}
		switch (instruction.getCode()) {
		case Code.MOVSB_M8_M8:
		case Code.MOVSW_M16_M16:
		case Code.MOVSD_M32_M32:
		case Code.MOVSQ_M64_M64:
			int regYSize = getYRegSize(instruction.getOp0Kind());
			if (regXSize != regYSize) {
				encoder.setErrorMessage(String.format("Same sized register must be used: reg #1 size = %d, reg #2 size = %d", regYSize * 8, regXSize * 8));
				return;
			}
			break;
		}
		encoder.setAddrSize(regXSize);
	}
}

final class OpY extends Op {
	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		int regYSize = OpX.getYRegSize(instruction.getOpKind(operand));
		if (regYSize == 0) {
			encoder.setErrorMessage(String.format("Operand %d: expected OpKind = MEMORY_ESDI, MEMORY_ESEDI or MEMORY_ESRDI", operand));
			return;
		}
		switch (instruction.getCode()) {
		case Code.CMPSB_M8_M8:
		case Code.CMPSW_M16_M16:
		case Code.CMPSD_M32_M32:
		case Code.CMPSQ_M64_M64:
			int regXSize = OpX.getXRegSize(instruction.getOp0Kind());
			if (regXSize != regYSize) {
				encoder.setErrorMessage(
						String.format("Same sized register must be used: reg #1 size = %d, reg #2 size = %d", regXSize * 8, regYSize * 8));
				return;
			}
			break;
		}
		encoder.setAddrSize(regYSize);
	}
}

final class OpMRBX extends Op {
	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		if (!encoder.verifyOpKind(operand, OpKind.MEMORY, instruction.getOpKind(operand)))
			return;
		int baseReg = instruction.getMemoryBase();
		if (instruction.getMemoryDisplSize() != 0 || instruction.getMemoryDisplacement64() != 0 ||
				instruction.getMemoryIndexScale() != 1 || instruction.getMemoryIndex() != Register.AL ||
				(baseReg != Register.BX && baseReg != Register.EBX && baseReg != Register.RBX)) {
			encoder.setErrorMessage(String.format("Operand %d: Operand must be [bx+al], [ebx+al], or [rbx+al]", operand));
			return;
		}
		int regSize;
		if (baseReg == Register.RBX)
			regSize = 8;
		else if (baseReg == Register.EBX)
			regSize = 4;
		else {
			assert baseReg == Register.BX : baseReg;
			regSize = 2;
		}
		encoder.setAddrSize(regSize);
	}
}

final class OpJ extends Op {
	private final int opKind;
	private final int immSize;

	OpJ(int opKind, int immSize) {
		this.opKind = opKind;
		this.immSize = immSize;
	}

	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		encoder.addBranch(opKind, immSize, instruction, operand);
	}

	@SuppressWarnings("deprecation")
	@Override
	public int getNearBranchOpKind() {
		return opKind;
	}
}

final class OpJx extends Op {
	private final int immSize;

	OpJx(int immSize) {
		this.immSize = immSize;
	}

	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		encoder.addBranchX(immSize, instruction, operand);
	}

	@SuppressWarnings("deprecation")
	@Override
	public int getNearBranchOpKind() {
		// xbegin is special and doesn't mask the target IP. We need to know the code size to return the correct value.
		// Instruction.CreateXbegin() should be used to create the instruction and this method should never be called.
		assert false : "Call Instruction.CreateXbegin()";
		return super.getNearBranchOpKind();
	}
}

final class OpJdisp extends Op {
	private final int displSize;

	OpJdisp(int displSize) {
		this.displSize = displSize;
	}

	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		encoder.addBranchDisp(displSize, instruction, operand);
	}

	@SuppressWarnings("deprecation")
	@Override
	public int getNearBranchOpKind() {
		return displSize == 2 ? OpKind.NEAR_BRANCH16 : OpKind.NEAR_BRANCH32;
	}
}

final class OpA extends Op {
	private final int size;

	OpA(int size) {
		assert size == 2 || size == 4 : size;
		this.size = size;
	}

	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		encoder.addFarBranch(instruction, operand, size);
	}

	@SuppressWarnings("deprecation")
	@Override
	public int getFarBranchOpKind() {
		assert size == 2 || size == 4 : size;
		return size == 2 ? OpKind.FAR_BRANCH16 : OpKind.FAR_BRANCH32;
	}
}

final class OpO extends Op {
	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		encoder.addAbsMem(instruction, operand);
	}
}

final class OpImm extends Op {
	private final byte value;

	OpImm(int value) {
		assert -0x80 <= value && value <= 0x7F : value;
		this.value = (byte)value;
	}

	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		if (!encoder.verifyOpKind(operand, OpKind.IMMEDIATE8, instruction.getOpKind(operand)))
			return;
		if ((instruction.getImmediate8() & 0xFF) != value) {
			encoder.setErrorMessage(String.format("Operand %d: Expected 0x%02X, actual: 0x%02X", operand, value, instruction.getImmediate8()));
			return;
		}
	}

	@SuppressWarnings("deprecation")
	@Override
	public int getImmediateOpKind() {
		return OpKind.IMMEDIATE8;
	}
}

final class OpHx extends Op {
	private final int regLo;
	private final int regHi;

	OpHx(int regLo, int regHi) {
		this.regLo = regLo;
		this.regHi = regHi;
	}

	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		if (!encoder.verifyOpKind(operand, OpKind.REGISTER, instruction.getOpKind(operand)))
			return;
		int reg = instruction.getOpRegister(operand);
		if (!encoder.verify(operand, reg, regLo, regHi))
			return;
		encoder.encoderFlags |= (reg - regLo) << EncoderFlags.VVVVV_SHIFT;
	}
}

final class OpVsib extends Op {
	private final int vsibIndexRegLo;
	private final int vsibIndexRegHi;

	OpVsib(int regLo, int regHi) {
		vsibIndexRegLo = regLo;
		vsibIndexRegHi = regHi;
	}

	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		encoder.encoderFlags |= EncoderFlags.MUST_USE_SIB;
		encoder.addRegOrMem(instruction, operand, Register.NONE, Register.NONE, vsibIndexRegLo, vsibIndexRegHi, true, false);
	}
}

final class OpIsX extends Op {
	private final int regLo;
	private final int regHi;

	OpIsX(int regLo, int regHi) {
		this.regLo = regLo;
		this.regHi = regHi;
	}

	@Override
	void encode(Encoder encoder, Instruction instruction, int operand) {
		if (!encoder.verifyOpKind(operand, OpKind.REGISTER, instruction.getOpKind(operand)))
			return;
		int reg = instruction.getOpRegister(operand);
		if (!encoder.verify(operand, reg, regLo, regHi))
			return;
		encoder.immSize = ImmSize.SIZE_IB_REG;
		encoder.immediate = (reg - regLo) << 4;
	}
}
