// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.internal.enc;

import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.enc.Encoder;

/** DO NOT USE: INTERNAL API */
public abstract class Op {
	/** DO NOT USE: INTERNAL API */
	public static final Op[] operands_3dnow = new Op[] {
		new OpModRM_reg(Register.MM0, Register.MM7),
		new OpModRM_rm(Register.MM0, Register.MM7),
	};

	/** DO NOT USE: INTERNAL API */
	public abstract void encode(Encoder encoder, Instruction instruction, int operand);

	/** DO NOT USE: INTERNAL API */
	public int getImmediateOpKind() {
		return -1;
	}

	/** DO NOT USE: INTERNAL API */
	public int getNearBranchOpKind() {
		return -1;
	}

	/** DO NOT USE: INTERNAL API */
	public int getFarBranchOpKind() {
		return -1;
	}
}

final class OpModRM_rm_mem_only extends Op {
	final boolean mustUseSib;

	public OpModRM_rm_mem_only(boolean mustUseSib) {
		this.mustUseSib = mustUseSib;
	}

	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		if (mustUseSib)
			encoder.encoderFlags |= EncoderFlags.MUST_USE_SIB;
		encoder.addRegOrMem(instruction, operand, Register.NONE, Register.NONE, true, false);
		*/
	}
}

final class OpModRM_rm extends Op {
	final int regLo;
	final int regHi;

	public OpModRM_rm(int regLo, int regHi) {
		this.regLo = regLo;
		this.regHi = regHi;
	}

	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		encoder.addRegOrMem(instruction, operand, regLo, regHi, true, true);
		*/
	}
}

final class OpRegEmbed8 extends Op {
	final int regLo;
	final int regHi;

	public OpRegEmbed8(int regLo, int regHi) {
		this.regLo = regLo;
		this.regHi = regHi;
	}

	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		encoder.addReg(instruction, operand, regLo, regHi);
		*/
	}
}

final class OpModRM_rm_reg_only extends Op {
	final int regLo;
	final int regHi;

	public OpModRM_rm_reg_only(int regLo, int regHi) {
		this.regLo = regLo;
		this.regHi = regHi;
	}

	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		encoder.addRegOrMem(instruction, operand, regLo, regHi, false, true);
		*/
	}
}

final class OpModRM_reg extends Op {
	final int regLo;
	final int regHi;

	public OpModRM_reg(int regLo, int regHi) {
		this.regLo = regLo;
		this.regHi = regHi;
	}

	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		encoder.addModRMRegister(instruction, operand, regLo, regHi);
		*/
	}
}

final class OpModRM_reg_mem extends Op {
	final int regLo;
	final int regHi;

	public OpModRM_reg_mem(int regLo, int regHi) {
		this.regLo = regLo;
		this.regHi = regHi;
	}

	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		encoder.addModRMRegister(instruction, operand, regLo, regHi);
		encoder.encoderFlags |= EncoderFlags.REG_IS_MEMORY;
		*/
	}
}

final class OpModRM_regF0 extends Op {
	final int regLo;
	final int regHi;

	public OpModRM_regF0(int regLo, int regHi) {
		this.regLo = regLo;
		this.regHi = regHi;
	}

	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		if (encoder.getBitness() != 64 && instruction.getOpKind(operand) == OpKind.REGISTER && instruction.getOpRegister(operand) >= regLo + 8
				&& instruction.getOpRegister(operand) <= regLo + 15) {
			encoder.encoderFlags |= EncoderFlags.PF0;
			encoder.addModRMRegister(instruction, operand, regLo + 8, regLo + 15);
		}
		else
			encoder.addModRMRegister(instruction, operand, regLo, regHi);
		*/
	}
}

final class OpReg extends Op {
	final int register;

	public OpReg(int register) {
		this.register = register;
	}

	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		encoder.verify(operand, OpKind.REGISTER, instruction.getOpKind(operand));
		encoder.verify(operand, register, instruction.getOpRegister(operand));
		*/
	}
}

final class OpRegSTi extends Op {
	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		if (!encoder.verify(operand, OpKind.REGISTER, instruction.getOpKind(operand)))
			return;
		int reg = instruction.getOpRegister(operand);
		if (!encoder.verify(operand, reg, Register.ST0, Register.ST7))
			return;
		assert (encoder.opCode & 7) == 0 : encoder.opCode;
		encoder.opCode |= (reg - Register.ST0);
		*/
	}
}

final class OprDI extends Op {
	static int GetRegSize(int opKind) {
		if (opKind == OpKind.MEMORY_SEG_RDI)
			return 8;
		if (opKind == OpKind.MEMORY_SEG_EDI)
			return 4;
		if (opKind == OpKind.MEMORY_SEG_DI)
			return 2;
		return 0;
	}

	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		int regSize = GetRegSize(instruction.getOpKind(operand));
		if (regSize == 0) {
			encoder.setErrorMessage(String.format("Operand %d: expected OpKind = MEMORY_SEG_DI, MEMORY_SEG_EDI or MEMORY_SEG_RDI", operand));
			return;
		}
		encoder.setAddrSize(regSize);
		*/
	}
}

final class OpIb extends Op {
	final int opKind;

	public OpIb(int opKind) {
		this.opKind = opKind;
	}

	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		switch (encoder.immSize) {
		case ImmSize.SIZE1:
			if (!encoder.verify(operand, OpKind.IMMEDIATE8_2ND, instruction.getOpKind(operand)))
				return;
			encoder.immSize = ImmSize.SIZE1_1;
			encoder.immediateHi = instruction.getImmediate8_2nd();
			break;
		case ImmSize.SIZE2:
			if (!encoder.verify(operand, OpKind.IMMEDIATE8_2ND, instruction.getOpKind(operand)))
				return;
			encoder.immSize = ImmSize.SIZE2_1;
			encoder.immediateHi = instruction.getImmediate8_2nd();
			break;
		default:
			int opImmKind = instruction.getOpKind(operand);
			if (!encoder.verify(operand, opKind, opImmKind))
				return;
			encoder.immSize = ImmSize.SIZE1;
			encoder.immediate = instruction.getImmediate8();
			break;
		}
		*/
	}

	@Override
	public int getImmediateOpKind() {
		return opKind;
	}
}

final class OpIw extends Op {
	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		if (!encoder.verify(operand, OpKind.IMMEDIATE16, instruction.getOpKind(operand)))
			return;
		encoder.immSize = ImmSize.SIZE2;
		encoder.immediate = instruction.getImmediate16();
		*/
	}

	@Override
	public int getImmediateOpKind() {
		return OpKind.IMMEDIATE16;
	}
}

final class OpId extends Op {
	final int opKind;

	public OpId(int opKind) {
		this.opKind = opKind;
	}

	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		int opImmKind = instruction.getOpKind(operand);
		if (!encoder.verify(operand, opKind, opImmKind))
			return;
		encoder.immSize = ImmSize.SIZE4;
		encoder.immediate = instruction.getImmediate32();
		*/
	}

	@Override
	public int getImmediateOpKind() {
		return opKind;
	}
}

final class OpIq extends Op {
	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		if (!encoder.verify(operand, OpKind.IMMEDIATE64, instruction.getOpKind(operand)))
			return;
		encoder.immSize = ImmSize.SIZE8;
		long imm = instruction.getImmediate64();
		encoder.immediate = (int)imm;
		encoder.immediateHi = (int)(imm >>> 32);
		*/
	}

	@Override
	public int getImmediateOpKind() {
		return OpKind.IMMEDIATE64;
	}
}

final class OpI4 extends Op {
	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		int opImmKind = instruction.getOpKind(operand);
		if (!encoder.verify(operand, OpKind.IMMEDIATE8, opImmKind))
			return;
		assert encoder.immSize == ImmSize.SIZE_IB_REG : encoder.immSize;
		assert (encoder.immediate & 0xF) == 0 : encoder.immediate;
		if (instruction.getImmediate8() > 0xF) {
			encoder.setErrorMessage(
					String.format("Operand %d: Immediate value must be 0-15, but value is 0x%02X", operand, instruction.getImmediate8()));
			return;
		}
		encoder.immSize = ImmSize.SIZE1;
		encoder.immediate |= instruction.getImmediate8();
		*/
	}

	@Override
	public int getImmediateOpKind() {
		return OpKind.IMMEDIATE8;
	}
}

final class OpX extends Op {
	static int GetXRegSize(int opKind) {
		if (opKind == OpKind.MEMORY_SEG_RSI)
			return 8;
		if (opKind == OpKind.MEMORY_SEG_ESI)
			return 4;
		if (opKind == OpKind.MEMORY_SEG_SI)
			return 2;
		return 0;
	}

	static int GetYRegSize(int opKind) {
		if (opKind == OpKind.MEMORY_ESRDI)
			return 8;
		if (opKind == OpKind.MEMORY_ESEDI)
			return 4;
		if (opKind == OpKind.MEMORY_ESDI)
			return 2;
		return 0;
	}

	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		int regXSize = GetXRegSize(instruction.getOpKind(operand));
		if (regXSize == 0) {
			encoder.setErrorMessage(String.format("Operand %d: expected OpKind = MEMORY_SEG_SI, MEMORY_SEG_ESI or MEMORY_SEG_RSI", operand));
			return;
		}
		switch (instruction.getCode()) {
		case Code.MOVSB_M8_M8:
		case Code.MOVSW_M16_M16:
		case Code.MOVSD_M32_M32:
		case Code.MOVSQ_M64_M64:
			int regYSize = GetYRegSize(instruction.getOp0Kind());
			if (regXSize != regYSize) {
				encoder.setErrorMessage(String.format("Same sized register must be used: reg #1 size = %d, reg #2 size = %d", regYSize * 8, regXSize * 8));
				return;
			}
			break;
		}
		encoder.setAddrSize(regXSize);
		*/
	}
}

final class OpY extends Op {
	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		int regYSize = OpX.GetYRegSize(instruction.getOpKind(operand));
		if (regYSize == 0) {
			encoder.setErrorMessage(String.format("Operand %d: expected OpKind = MEMORY_ESDI, MEMORY_ESEDI or MEMORY_ESRDI", operand));
			return;
		}
		switch (instruction.getCode()) {
		case Code.CMPSB_M8_M8:
		case Code.CMPSW_M16_M16:
		case Code.CMPSD_M32_M32:
		case Code.CMPSQ_M64_M64:
			int regXSize = OpX.GetXRegSize(instruction.getOp0Kind());
			if (regXSize != regYSize) {
				encoder.setErrorMessage(
						String.format("Same sized register must be used: reg #1 size = %d, reg #2 size = %d", regXSize * 8, regYSize * 8));
				return;
			}
			break;
		}
		encoder.setAddrSize(regYSize);
		*/
	}
}

final class OpMRBX extends Op {
	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		if (!encoder.verify(operand, OpKind.MEMORY, instruction.getOpKind(operand)))
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
		*/
	}
}

final class OpJ extends Op {
	final int opKind;
	final int immSize;

	public OpJ(int opKind, int immSize) {
		this.opKind = opKind;
		this.immSize = immSize;
	}

	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		encoder.addBranch(opKind, immSize, instruction, operand);
		*/
	}

	@Override
	public int getNearBranchOpKind() {
		return opKind;
	}
}

final class OpJx extends Op {
	final int immSize;

	public OpJx(int immSize) {
		this.immSize = immSize;
	}

	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		encoder.addBranchX(immSize, instruction, operand);
		*/
	}

	@Override
	public int getNearBranchOpKind() {
		// xbegin is special and doesn't mask the target IP. We need to know the code size to return the correct value.
		// Instruction.CreateXbegin() should be used to create the instruction and this method should never be called.
		assert false : "Call Instruction.CreateXbegin()";
		return super.getNearBranchOpKind();
	}
}

final class OpJdisp extends Op {
	final int displSize;

	public OpJdisp(int displSize) {
		this.displSize = displSize;
	}

	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		encoder.addBranchDisp(displSize, instruction, operand);
		*/
	}

	@Override
	public int getNearBranchOpKind() {
		return displSize == 2 ? OpKind.NEAR_BRANCH16 : OpKind.NEAR_BRANCH32;
	}
}

final class OpA extends Op {
	final int size;

	public OpA(int size) {
		assert size == 2 || size == 4 : size;
		this.size = size;
	}

	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		encoder.addFarBranch(instruction, operand, size);
		*/
	}

	@Override
	public int getFarBranchOpKind() {
		assert size == 2 || size == 4 : size;
		return size == 2 ? OpKind.FAR_BRANCH16 : OpKind.FAR_BRANCH32;
	}
}

final class OpO extends Op {
	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		encoder.addAbsMem(instruction, operand);
		*/
	}
}

final class OpImm extends Op {
	final byte value;

	public OpImm(int value) {
		assert -0x80 <= value && value <= 0x7F : value;
		this.value = (byte)value;
	}

	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		if (!encoder.verify(operand, OpKind.IMMEDIATE8, instruction.getOpKind(operand)))
			return;
		if (instruction.getImmediate8() != value) {
			encoder.setErrorMessage(String.format("Operand %d: Expected 0x%02X, actual: 0x%02X", operand, value, instruction.getImmediate8()));
			return;
		}
		*/
	}

	@Override
	public int getImmediateOpKind() {
		return OpKind.IMMEDIATE8;
	}
}

final class OpHx extends Op {
	final int regLo;
	final int regHi;

	public OpHx(int regLo, int regHi) {
		this.regLo = regLo;
		this.regHi = regHi;
	}

	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		if (!encoder.verify(operand, OpKind.REGISTER, instruction.getOpKind(operand)))
			return;
		int reg = instruction.getOpRegister(operand);
		if (!encoder.verify(operand, reg, regLo, regHi))
			return;
		encoder.encoderFlags |= (reg - regLo) << EncoderFlags.VVVVV_SHIFT;
		*/
	}
}

final class OpVsib extends Op {
	final int vsibIndexRegLo;
	final int vsibIndexRegHi;

	public OpVsib(int regLo, int regHi) {
		vsibIndexRegLo = regLo;
		vsibIndexRegHi = regHi;
	}

	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		encoder.encoderFlags |= EncoderFlags.MUST_USE_SIB;
		encoder.addRegOrMem(instruction, operand, Register.NONE, Register.NONE, vsibIndexRegLo, vsibIndexRegHi, true, false);
		*/
	}
}

final class OpIsX extends Op {
	final int regLo;
	final int regHi;

	public OpIsX(int regLo, int regHi) {
		this.regLo = regLo;
		this.regHi = regHi;
	}

	@Override
	public void encode(Encoder encoder, Instruction instruction, int operand) {
		throw new UnsupportedOperationException(); // TODO:
		/*TODO:
		if (!encoder.verify(operand, OpKind.REGISTER, instruction.getOpKind(operand)))
			return;
		int reg = instruction.getOpRegister(operand);
		if (!encoder.verify(operand, reg, regLo, regHi))
			return;
		encoder.immSize = ImmSize.SIZE_IB_REG;
		encoder.immediate = (reg - regLo) << 4;
		*/
	}
}
