// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt.nasm;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeSize;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.RoundingControl;
import com.github.icedland.iced.x86.fmt.FormatterOptions;
import com.github.icedland.iced.x86.info.OpAccess;
import com.github.icedland.iced.x86.internal.MvexInfo;
import com.github.icedland.iced.x86.internal.fmt.FormatterString;

final class InstrOpInfo {
	FormatterString Mnemonic;
	int Flags;
	byte OpCount;
	byte Op0Kind;
	byte Op1Kind;
	byte Op2Kind;
	byte Op3Kind;
	byte Op4Kind;
	byte Op0Register;
	byte Op1Register;
	byte Op2Register;
	byte Op3Register;
	private byte Op4Register;
	byte Op0Index;
	byte Op1Index;
	byte Op2Index;
	byte Op3Index;
	byte Op4Index;

	int getMemorySize() {
		return (Flags >>> InstrOpInfoFlags.MEMORY_SIZE_SHIFT) & InstrOpInfoFlags.MEMORY_SIZE_MASK;
	}

	void setMemorySize(int value) {
		Flags = (Flags & ~(InstrOpInfoFlags.MEMORY_SIZE_MASK << InstrOpInfoFlags.MEMORY_SIZE_SHIFT)) |
				((value & InstrOpInfoFlags.MEMORY_SIZE_MASK) << InstrOpInfoFlags.MEMORY_SIZE_SHIFT);
	}

	@SuppressWarnings("deprecation")
	int getOpRegister(int operand) {
		final int REG_MASK = com.github.icedland.iced.x86.internal.Constants.REG_MASK;
		switch (operand) {
		case 0:
			return Op0Register & REG_MASK;
		case 1:
			return Op1Register & REG_MASK;
		case 2:
			return Op2Register & REG_MASK;
		case 3:
			return Op3Register & REG_MASK;
		case 4:
			return Op4Register & REG_MASK;
		default:
			throw new IllegalArgumentException("operand");
		}
	}

	int getOpKind(int operand) {
		switch (operand) {
		case 0:
			return Op0Kind;
		case 1:
			return Op1Kind;
		case 2:
			return Op2Kind;
		case 3:
			return Op3Kind;
		case 4:
			return Op4Kind;
		default:
			assert Op0Kind == InstrOpKind.DECLARE_BYTE || Op0Kind == InstrOpKind.DECLARE_WORD || Op0Kind == InstrOpKind.DECLARE_DWORD
					|| Op0Kind == InstrOpKind.DECLARE_QWORD : Op0Kind;
			return Op0Kind;
		}
	}

	int getInstructionIndex(int operand) {
		int instructionOperand;
		switch (operand) {
		case 0:
			instructionOperand = Op0Index;
			break;
		case 1:
			instructionOperand = Op1Index;
			break;
		case 2:
			instructionOperand = Op2Index;
			break;
		case 3:
			instructionOperand = Op3Index;
			break;
		case 4:
			instructionOperand = Op4Index;
			break;
		default:
			assert Op0Kind == InstrOpKind.DECLARE_BYTE || Op0Kind == InstrOpKind.DECLARE_WORD || Op0Kind == InstrOpKind.DECLARE_DWORD
					|| Op0Kind == InstrOpKind.DECLARE_QWORD : Op0Kind;
			instructionOperand = -1;
			break;
		}
		return instructionOperand < 0 ? -1 : instructionOperand;
	}

	Integer tryGetOpAccess(int operand) {
		int instructionOperand;
		switch (operand) {
		case 0:
			instructionOperand = Op0Index;
			break;
		case 1:
			instructionOperand = Op1Index;
			break;
		case 2:
			instructionOperand = Op2Index;
			break;
		case 3:
			instructionOperand = Op3Index;
			break;
		case 4:
			instructionOperand = Op4Index;
			break;
		default:
			assert Op0Kind == InstrOpKind.DECLARE_BYTE || Op0Kind == InstrOpKind.DECLARE_WORD || Op0Kind == InstrOpKind.DECLARE_DWORD
					|| Op0Kind == InstrOpKind.DECLARE_QWORD : Op0Kind;
			instructionOperand = Op0Index;
			break;
		}
		if (instructionOperand < InstrInfo.OP_ACCESS_INVALID)
			return -instructionOperand - 2;
		return null;
	}

	int getOperandIndex(int instructionOperand) {
		int index;
		if (instructionOperand == Op0Index)
			index = 0;
		else if (instructionOperand == Op1Index)
			index = 1;
		else if (instructionOperand == Op2Index)
			index = 2;
		else if (instructionOperand == Op3Index)
			index = 3;
		else if (instructionOperand == Op4Index)
			index = 4;
		else
			index = -1;
		return index < OpCount ? index : -1;
	}

	InstrOpInfo() {
	}

	InstrOpInfo(FormatterString mnemonic, Instruction instruction, int flags) {
		Mnemonic = mnemonic;
		Flags = flags | (instruction.getMemorySize() << InstrOpInfoFlags.MEMORY_SIZE_SHIFT);
		Op0Kind = (byte)instruction.getOp0Kind();
		Op1Kind = (byte)instruction.getOp1Kind();
		Op2Kind = (byte)instruction.getOp2Kind();
		Op3Kind = (byte)instruction.getOp3Kind();
		Op4Kind = (byte)instruction.getOp4Kind();
		Op0Register = (byte)instruction.getOp0Register();
		Op1Register = (byte)instruction.getOp1Register();
		Op2Register = (byte)instruction.getOp2Register();
		Op3Register = (byte)instruction.getOp3Register();
		Op4Register = (byte)instruction.getOp4Register();
		int opCount = instruction.getOpCount();
		OpCount = (byte)opCount;
		switch (opCount) {
		case 0:
			Op0Index = InstrInfo.OP_ACCESS_INVALID;
			Op1Index = InstrInfo.OP_ACCESS_INVALID;
			Op2Index = InstrInfo.OP_ACCESS_INVALID;
			Op3Index = InstrInfo.OP_ACCESS_INVALID;
			Op4Index = InstrInfo.OP_ACCESS_INVALID;
			break;

		case 1:
			Op0Index = 0;
			Op1Index = InstrInfo.OP_ACCESS_INVALID;
			Op2Index = InstrInfo.OP_ACCESS_INVALID;
			Op3Index = InstrInfo.OP_ACCESS_INVALID;
			Op4Index = InstrInfo.OP_ACCESS_INVALID;
			break;

		case 2:
			Op0Index = 0;
			Op1Index = 1;
			Op2Index = InstrInfo.OP_ACCESS_INVALID;
			Op3Index = InstrInfo.OP_ACCESS_INVALID;
			Op4Index = InstrInfo.OP_ACCESS_INVALID;
			break;

		case 3:
			Op0Index = 0;
			Op1Index = 1;
			Op2Index = 2;
			Op3Index = InstrInfo.OP_ACCESS_INVALID;
			Op4Index = InstrInfo.OP_ACCESS_INVALID;
			break;

		case 4:
			Op0Index = 0;
			Op1Index = 1;
			Op2Index = 2;
			Op3Index = 3;
			Op4Index = InstrInfo.OP_ACCESS_INVALID;
			break;

		case 5:
			Op0Index = 0;
			Op1Index = 1;
			Op2Index = 2;
			Op3Index = 3;
			Op4Index = 4;
			break;

		default:
			throw new UnsupportedOperationException();
		}
	}
}

abstract class InstrInfo {
	protected static final int OP_ACCESS_INVALID = -1;
	protected static final int OP_ACCESS_NONE = -(OpAccess.NONE + 2);
	protected static final int OP_ACCESS_READ = -(OpAccess.READ + 2);
	protected static final int OP_ACCESS_COND_READ = -(OpAccess.COND_READ + 2);
	protected static final int OP_ACCESS_WRITE = -(OpAccess.WRITE + 2);
	protected static final int OP_ACCESS_COND_WRITE = -(OpAccess.COND_WRITE + 2);
	protected static final int OP_ACCESS_READ_WRITE = -(OpAccess.READ_WRITE + 2);
	protected static final int OP_ACCESS_READ_COND_WRITE = -(OpAccess.READ_COND_WRITE + 2);
	protected static final int OP_ACCESS_NO_MEM_ACCESS = -(OpAccess.NO_MEM_ACCESS + 2);

	abstract InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction);

	protected static int getBitness(int codeSize) {
		switch (codeSize) {
		case CodeSize.CODE16:
			return 16;
		case CodeSize.CODE32:
			return 32;
		case CodeSize.CODE64:
			return 64;
		default:
			return 0;
		}
	}
}

final class SimpleInstrInfo extends InstrInfo {
	private final FormatterString mnemonic;
	private final int flags;

	SimpleInstrInfo(String mnemonic) {
		this(mnemonic, InstrOpInfoFlags.NONE);
	}

	SimpleInstrInfo(String mnemonic, int flags) {
		this.mnemonic = new FormatterString(mnemonic);
		this.flags = flags;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_cc extends InstrInfo {
	private final int ccIndex;
	private final FormatterString[] mnemonics;

	SimpleInstrInfo_cc(int ccIndex, String[] mnemonics) {
		this.ccIndex = ccIndex;
		this.mnemonics = FormatterString.create(mnemonics);
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		final int flags = InstrOpInfoFlags.NONE;
		FormatterString mnemonic = com.github.icedland.iced.x86.internal.fmt.MnemonicCC.getMnemonicCC(options, ccIndex, mnemonics);
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_push_imm8 extends InstrInfo {
	private final int bitness;
	private final int sexInfo;
	private final FormatterString mnemonic;

	SimpleInstrInfo_push_imm8(int bitness, int sexInfo, String mnemonic) {
		this.bitness = bitness;
		this.sexInfo = sexInfo;
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = sexInfo << InstrOpInfoFlags.SIGN_EXTEND_INFO_SHIFT;

		int instrBitness = getBitness(instruction.getCodeSize());
		if (bitness != 0 && instrBitness != 0 && instrBitness != bitness) {
			if (bitness == 16)
				flags |= InstrOpInfoFlags.OP_SIZE16;
			else if (bitness == 32)
				flags |= InstrOpInfoFlags.OP_SIZE32;
			else
				flags |= InstrOpInfoFlags.OP_SIZE64;
		}

		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_push_imm extends InstrInfo {
	private final int bitness;
	private final int sexInfo;
	private final FormatterString mnemonic;

	SimpleInstrInfo_push_imm(int bitness, int sexInfo, String mnemonic) {
		this.bitness = bitness;
		this.sexInfo = sexInfo;
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = InstrOpInfoFlags.NONE;

		boolean signExtend = true;
		int instrBitness = getBitness(instruction.getCodeSize());
		if (bitness != 0 && instrBitness != 0 && instrBitness != bitness) {
			if (instrBitness == 64)
				flags |= InstrOpInfoFlags.OP_SIZE16;
		}
		else if (bitness == 16 && instrBitness == 16)
			signExtend = false;

		if (signExtend)
			flags |= sexInfo << InstrOpInfoFlags.SIGN_EXTEND_INFO_SHIFT;

		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_SignExt extends InstrInfo {
	private final int sexInfoReg;
	private final int sexInfoMem;
	private final FormatterString mnemonic;
	private final int flags;

	SimpleInstrInfo_SignExt(int sexInfo, String mnemonic, int flags) {
		this(sexInfo, sexInfo, mnemonic, flags);
	}

	SimpleInstrInfo_SignExt(int sexInfoReg, int sexInfoMem, String mnemonic, int flags) {
		this.sexInfoReg = sexInfoReg;
		this.sexInfoMem = sexInfoMem;
		this.mnemonic = new FormatterString(mnemonic);
		this.flags = flags;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		assert instruction.getOpCount() == 2 : instruction.getOpCount();
		int sexInfo = instruction.getOp0Kind() == OpKind.MEMORY || instruction.getOp1Kind() == OpKind.MEMORY ? sexInfoMem : sexInfoReg;
		int flags = this.flags;
		flags |= sexInfo << InstrOpInfoFlags.SIGN_EXTEND_INFO_SHIFT;
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_imul extends InstrInfo {
	private final int sexInfo;
	private final FormatterString mnemonic;

	SimpleInstrInfo_imul(int sexInfo, String mnemonic) {
		this.sexInfo = sexInfo;
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = sexInfo << InstrOpInfoFlags.SIGN_EXTEND_INFO_SHIFT;
		InstrOpInfo info = new InstrOpInfo(mnemonic, instruction, flags);
		assert info.OpCount == 3 : info.OpCount;
		if (options.getUsePseudoOps() && info.Op0Kind == InstrOpKind.REGISTER && info.Op1Kind == InstrOpKind.REGISTER
				&& info.Op0Register == info.Op1Register) {
			info.OpCount--;
			info.Op0Index = OP_ACCESS_READ_WRITE;
			info.Op1Kind = (byte)info.Op2Kind;
			info.Op1Index = 2;
			info.Op2Index = OP_ACCESS_INVALID;
		}
		return info;
	}
}

final class SimpleInstrInfo_AamAad extends InstrInfo {
	private final FormatterString mnemonic;

	SimpleInstrInfo_AamAad(String mnemonic) {
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		if (instruction.getImmediate8() == 10) {
			InstrOpInfo info = new InstrOpInfo();
			info.Mnemonic = mnemonic;
			return info;
		}
		else
			return new InstrOpInfo(mnemonic, instruction, InstrOpInfoFlags.NONE);
	}
}

final class SimpleInstrInfo_String extends InstrInfo {
	private final FormatterString mnemonic;

	SimpleInstrInfo_String(String mnemonic) {
		this.mnemonic = new FormatterString(mnemonic);
	}

	static int getAddressSizeFlags(int opKind) {
		switch (opKind) {
		case OpKind.MEMORY_SEG_SI:
		case OpKind.MEMORY_SEG_DI:
		case OpKind.MEMORY_ESDI:
			return InstrOpInfoFlags.ADDR_SIZE16;

		case OpKind.MEMORY_SEG_ESI:
		case OpKind.MEMORY_SEG_EDI:
		case OpKind.MEMORY_ESEDI:
			return InstrOpInfoFlags.ADDR_SIZE32;

		case OpKind.MEMORY_SEG_RSI:
		case OpKind.MEMORY_SEG_RDI:
		case OpKind.MEMORY_ESRDI:
			return InstrOpInfoFlags.ADDR_SIZE64;

		default:
			return 0;
		}
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int opKind = instruction.getOp0Kind() != OpKind.REGISTER ? instruction.getOp0Kind() : instruction.getOp1Kind();
		int opKindFlags = getAddressSizeFlags(opKind);
		int instrFlags;
		switch (instruction.getCodeSize()) {
		case CodeSize.UNKNOWN:
			instrFlags = opKindFlags;
			break;
		case CodeSize.CODE16:
			instrFlags = InstrOpInfoFlags.ADDR_SIZE16;
			break;
		case CodeSize.CODE32:
			instrFlags = InstrOpInfoFlags.ADDR_SIZE32;
			break;
		case CodeSize.CODE64:
			instrFlags = InstrOpInfoFlags.ADDR_SIZE64;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		int flags = 0;
		if (opKindFlags != instrFlags)
			flags |= opKindFlags;
		InstrOpInfo info = new InstrOpInfo();
		info.Flags = flags;
		info.Mnemonic = mnemonic;
		return info;
	}
}

final class SimpleInstrInfo_XLAT extends InstrInfo {
	private final FormatterString mnemonic;

	SimpleInstrInfo_XLAT(String mnemonic) {
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int baseReg;
		switch (instruction.getCodeSize()) {
		case CodeSize.UNKNOWN:
			baseReg = instruction.getMemoryBase();
			break;
		case CodeSize.CODE16:
			baseReg = Register.BX;
			break;
		case CodeSize.CODE32:
			baseReg = Register.EBX;
			break;
		case CodeSize.CODE64:
			baseReg = Register.RBX;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		int flags = 0;
		int memBaseReg = instruction.getMemoryBase();
		if (memBaseReg != baseReg) {
			if (memBaseReg == Register.BX)
				flags |= InstrOpInfoFlags.ADDR_SIZE16;
			else if (memBaseReg == Register.EBX)
				flags |= InstrOpInfoFlags.ADDR_SIZE32;
			else if (memBaseReg == Register.RBX)
				flags |= InstrOpInfoFlags.ADDR_SIZE64;
		}
		InstrOpInfo info = new InstrOpInfo();
		info.Flags = flags;
		info.Mnemonic = mnemonic;
		return info;
	}
}

final class SimpleInstrInfo_nop extends InstrInfo {
	private final int bitness;
	private final FormatterString mnemonic;
	private final int register;

	SimpleInstrInfo_nop(int bitness, String mnemonic, int register) {
		this.bitness = bitness;
		this.mnemonic = new FormatterString(mnemonic);
		this.register = register;
	}

	private static final FormatterString str_xchg = new FormatterString("xchg");

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int instrBitness = getBitness(instruction.getCodeSize());
		if (instrBitness == 0 || (instrBitness & bitness) != 0)
			return new InstrOpInfo(mnemonic, instruction, InstrOpInfoFlags.NONE);
		else {
			InstrOpInfo info = new InstrOpInfo();
			info.Mnemonic = str_xchg;
			info.OpCount = 2;
			info.Op0Register = (byte)register;
			info.Op1Register = (byte)register;
			info.Op0Index = OP_ACCESS_NONE;
			info.Op1Index = OP_ACCESS_NONE;
			return info;
		}
	}
}

final class SimpleInstrInfo_STIG1 extends InstrInfo {
	private final FormatterString mnemonic;
	private final boolean pseudoOp;

	SimpleInstrInfo_STIG1(String mnemonic, boolean pseudoOp) {
		this.mnemonic = new FormatterString(mnemonic);
		this.pseudoOp = pseudoOp;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo();
		info.Mnemonic = mnemonic;
		assert instruction.getOpCount() == 2 : instruction.getOpCount();
		assert instruction.getOp0Kind() == OpKind.REGISTER && instruction.getOp0Register() == Register.ST0;
		if (!pseudoOp || !(options.getUsePseudoOps() && instruction.getOp1Register() == Register.ST1)) {
			info.OpCount = 1;
			info.Op0Register = (byte)instruction.getOp1Register();
			info.Op0Index = 1;
		}
		return info;
	}
}

final class SimpleInstrInfo_STIG2 extends InstrInfo {
	private final int flags;
	private final FormatterString mnemonic;
	private final boolean pseudoOp;

	SimpleInstrInfo_STIG2(String mnemonic, boolean pseudoOp) {
		this(mnemonic, 0, pseudoOp);
	}

	SimpleInstrInfo_STIG2(String mnemonic, int flags) {
		this(mnemonic, flags, false);
	}

	SimpleInstrInfo_STIG2(String mnemonic, int flags, boolean pseudoOp) {
		this.flags = flags;
		this.mnemonic = new FormatterString(mnemonic);
		this.pseudoOp = pseudoOp;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo();
		info.Flags = flags;
		info.Mnemonic = mnemonic;
		assert instruction.getOpCount() == 2 : instruction.getOpCount();
		assert instruction.getOp1Kind() == OpKind.REGISTER && instruction.getOp1Register() == Register.ST0;
		if (!pseudoOp || !(options.getUsePseudoOps() && instruction.getOp0Register() == Register.ST1)) {
			info.OpCount = 1;
			info.Op0Register = (byte)instruction.getOp0Register();
		}
		return info;
	}
}

final class SimpleInstrInfo_as extends InstrInfo {
	private final int bitness;
	private final FormatterString mnemonic;

	SimpleInstrInfo_as(int bitness, String mnemonic) {
		this.bitness = bitness;
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = InstrOpInfoFlags.NONE;
		int instrBitness = getBitness(instruction.getCodeSize());
		if (instrBitness != 0 && instrBitness != bitness) {
			if (bitness == 16)
				flags |= InstrOpInfoFlags.ADDR_SIZE16;
			else if (bitness == 32)
				flags |= InstrOpInfoFlags.ADDR_SIZE32;
			else
				flags |= InstrOpInfoFlags.ADDR_SIZE64;
		}
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_maskmovq extends InstrInfo {
	private final FormatterString mnemonic;

	SimpleInstrInfo_maskmovq(String mnemonic) {
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		assert instruction.getOpCount() == 3 : instruction.getOpCount();

		int instrBitness = getBitness(instruction.getCodeSize());

		int bitness;
		switch (instruction.getOp0Kind()) {
		case OpKind.MEMORY_SEG_DI:
			bitness = 16;
			break;
		case OpKind.MEMORY_SEG_EDI:
			bitness = 32;
			break;
		case OpKind.MEMORY_SEG_RDI:
			bitness = 64;
			break;
		default:
			bitness = instrBitness;
			break;
		}

		InstrOpInfo info = new InstrOpInfo();
		info.Mnemonic = mnemonic;
		info.OpCount = 2;
		info.Op0Kind = (byte)instruction.getOp1Kind();
		info.Op0Index = 1;
		info.Op0Register = (byte)instruction.getOp1Register();
		info.Op1Kind = (byte)instruction.getOp2Kind();
		info.Op1Index = 2;
		info.Op1Register = (byte)instruction.getOp2Register();
		if (instrBitness != 0 && instrBitness != bitness) {
			if (bitness == 16)
				info.Flags |= InstrOpInfoFlags.ADDR_SIZE16;
			else if (bitness == 32)
				info.Flags |= InstrOpInfoFlags.ADDR_SIZE32;
			else
				info.Flags |= InstrOpInfoFlags.ADDR_SIZE64;
		}
		return info;
	}
}

final class SimpleInstrInfo_pblendvb extends InstrInfo {
	private final FormatterString mnemonic;
	private final int memSize;

	SimpleInstrInfo_pblendvb(String mnemonic, int memSize) {
		this.mnemonic = new FormatterString(mnemonic);
		this.memSize = memSize;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo();
		assert instruction.getOpCount() == 2 : instruction.getOpCount();
		info.Mnemonic = mnemonic;
		info.OpCount = 3;
		info.Op0Kind = (byte)instruction.getOp0Kind();
		info.Op0Register = (byte)instruction.getOp0Register();
		info.Op1Kind = (byte)instruction.getOp1Kind();
		info.Op1Index = 1;
		info.Op1Register = (byte)instruction.getOp1Register();
		info.Op2Register = (byte)Register.XMM0;
		info.setMemorySize(memSize);
		info.Op2Index = OP_ACCESS_READ;
		return info;
	}
}

final class SimpleInstrInfo_reverse extends InstrInfo {
	private final FormatterString mnemonic;

	SimpleInstrInfo_reverse(String mnemonic) {
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo();
		info.Mnemonic = mnemonic;
		assert instruction.getOpCount() == 2 : instruction.getOpCount();
		info.OpCount = 2;
		info.Op0Kind = (byte)instruction.getOp1Kind();
		info.Op0Index = 1;
		info.Op0Register = (byte)instruction.getOp1Register();
		info.Op1Kind = (byte)instruction.getOp0Kind();
		info.Op1Register = (byte)instruction.getOp0Register();
		info.setMemorySize(instruction.getMemorySize());
		return info;
	}
}

final class SimpleInstrInfo_OpSize extends InstrInfo {
	private final int codeSize;
	private final FormatterString[] mnemonics;

	SimpleInstrInfo_OpSize(int codeSize, String mnemonic, String mnemonic16, String mnemonic32, String mnemonic64) {
		this.codeSize = codeSize;
		mnemonics = new FormatterString[4];
		mnemonics[CodeSize.UNKNOWN] = new FormatterString(mnemonic);
		mnemonics[CodeSize.CODE16] = new FormatterString(mnemonic16);
		mnemonics[CodeSize.CODE32] = new FormatterString(mnemonic32);
		mnemonics[CodeSize.CODE64] = new FormatterString(mnemonic64);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		FormatterString mnemonic;
		if (instruction.getCodeSize() == codeSize)
			mnemonic = mnemonics[CodeSize.UNKNOWN];
		else
			mnemonic = mnemonics[codeSize];
		return new InstrOpInfo(mnemonic, instruction, InstrOpInfoFlags.NONE);
	}
}

final class SimpleInstrInfo_OpSize2_bnd extends InstrInfo {
	private final FormatterString[] mnemonics;

	SimpleInstrInfo_OpSize2_bnd(String mnemonic, String mnemonic16, String mnemonic32, String mnemonic64) {
		mnemonics = new FormatterString[4];
		mnemonics[CodeSize.UNKNOWN] = new FormatterString(mnemonic);
		mnemonics[CodeSize.CODE16] = new FormatterString(mnemonic16);
		mnemonics[CodeSize.CODE32] = new FormatterString(mnemonic32);
		mnemonics[CodeSize.CODE64] = new FormatterString(mnemonic64);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = InstrOpInfoFlags.NONE;
		if (instruction.getRepnePrefix())
			flags |= InstrOpInfoFlags.BND_PREFIX;
		FormatterString mnemonic = mnemonics[instruction.getCodeSize()];
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_OpSize3 extends InstrInfo {
	private final int bitness;
	private final FormatterString mnemonicDefault;
	private final FormatterString mnemonicFull;

	SimpleInstrInfo_OpSize3(int bitness, String mnemonicDefault, String mnemonicFull) {
		this.bitness = bitness;
		this.mnemonicDefault = new FormatterString(mnemonicDefault);
		this.mnemonicFull = new FormatterString(mnemonicFull);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int instrBitness = getBitness(instruction.getCodeSize());
		FormatterString mnemonic;
		if (instrBitness == 0 || (instrBitness & bitness) != 0)
			mnemonic = mnemonicDefault;
		else
			mnemonic = mnemonicFull;
		return new InstrOpInfo(mnemonic, instruction, InstrOpInfoFlags.NONE);
	}
}

final class SimpleInstrInfo_os extends InstrInfo {
	private final int bitness;
	private final FormatterString mnemonic;
	private final int flags;

	SimpleInstrInfo_os(int bitness, String mnemonic) {
		this(bitness, mnemonic, InstrOpInfoFlags.NONE);
	}

	SimpleInstrInfo_os(int bitness, String mnemonic, int flags) {
		this.bitness = bitness;
		this.mnemonic = new FormatterString(mnemonic);
		this.flags = flags;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = this.flags;
		int instrBitness = getBitness(instruction.getCodeSize());
		if (instrBitness != 0 && instrBitness != bitness) {
			if (bitness == 16)
				flags |= InstrOpInfoFlags.OP_SIZE16;
			else if (bitness == 32)
				flags |= InstrOpInfoFlags.OP_SIZE32;
			else
				flags |= InstrOpInfoFlags.OP_SIZE64;
		}
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_os_mem extends InstrInfo {
	private final int bitness;
	private final FormatterString mnemonic;

	SimpleInstrInfo_os_mem(int bitness, String mnemonic) {
		this.bitness = bitness;
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = InstrOpInfoFlags.NONE;
		int instrBitness = getBitness(instruction.getCodeSize());
		boolean hasMemOp = instruction.getOp0Kind() == OpKind.MEMORY || instruction.getOp1Kind() == OpKind.MEMORY;
		if (hasMemOp && !(instrBitness == 0 || (instrBitness != 64 && instrBitness == bitness) || (instrBitness == 64 && bitness == 32))) {
			if (bitness == 16)
				flags |= InstrOpInfoFlags.OP_SIZE16;
			else if (bitness == 32)
				flags |= InstrOpInfoFlags.OP_SIZE32;
			else
				flags |= InstrOpInfoFlags.OP_SIZE64;
		}
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_os_mem2 extends InstrInfo {
	private final int flags;
	private final int bitness;
	private final FormatterString mnemonic;

	SimpleInstrInfo_os_mem2(int bitness, String mnemonic, int flags) {
		this.flags = flags;
		this.bitness = bitness;
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = this.flags;
		int instrBitness = getBitness(instruction.getCodeSize());
		if (instrBitness != 0 && (instrBitness & bitness) == 0) {
			if (instrBitness != 16)
				flags |= InstrOpInfoFlags.OP_SIZE16;
			else
				flags |= InstrOpInfoFlags.OP_SIZE32;
		}
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_os_mem_reg16 extends InstrInfo {
	private final int bitness;
	private final FormatterString mnemonic;

	SimpleInstrInfo_os_mem_reg16(int bitness, String mnemonic) {
		this.bitness = bitness;
		this.mnemonic = new FormatterString(mnemonic);
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		final int REG_MASK = com.github.icedland.iced.x86.internal.Constants.REG_MASK;
		int flags = InstrOpInfoFlags.NONE;
		int instrBitness = getBitness(instruction.getCodeSize());
		assert instruction.getOpCount() == 1 : instruction.getOpCount();
		if (instruction.getOp0Kind() == OpKind.MEMORY) {
			if (!(instrBitness == 0 || (instrBitness != 64 && instrBitness == bitness) || (instrBitness == 64 && bitness == 32))) {
				if (bitness == 16)
					flags |= InstrOpInfoFlags.OP_SIZE16;
				else if (bitness == 32)
					flags |= InstrOpInfoFlags.OP_SIZE32;
				else
					flags |= InstrOpInfoFlags.OP_SIZE64;
			}
		}
		InstrOpInfo info = new InstrOpInfo(mnemonic, instruction, flags);
		if (instruction.getOp0Kind() == OpKind.REGISTER) {
			int reg = info.Op0Register & REG_MASK;
			int regSize = 0;
			if (Register.AX <= reg && reg <= Register.R15W)
				regSize = 16;
			else if (Register.EAX <= reg && reg <= Register.R15D) {
				regSize = 32;
				reg = reg - Register.EAX + Register.AX;
			}
			else if (Register.RAX <= reg && reg <= Register.R15) {
				regSize = 64;
				reg = reg - Register.RAX + Register.AX;
			}
			assert regSize != 0 : regSize;
			if (regSize != 0) {
				info.Op0Register = (byte)reg;
				if (!((instrBitness != 64 && instrBitness == regSize) || (instrBitness == 64 && regSize == 32))) {
					if (bitness == 16)
						info.Flags |= InstrOpInfoFlags.OP_SIZE16;
					else if (bitness == 32)
						info.Flags |= InstrOpInfoFlags.OP_SIZE32;
					else
						info.Flags |= InstrOpInfoFlags.OP_SIZE64;
				}
			}
		}
		return info;
	}
}

final class SimpleInstrInfo_os_jcc extends InstrInfo {
	private final int bitness;
	private final int ccIndex;
	private final FormatterString[] mnemonics;
	private final int flags;

	SimpleInstrInfo_os_jcc(int bitness, int ccIndex, String[] mnemonics) {
		this(bitness, ccIndex, mnemonics, InstrOpInfoFlags.NONE);
	}

	SimpleInstrInfo_os_jcc(int bitness, int ccIndex, String[] mnemonics, int flags) {
		this.bitness = bitness;
		this.ccIndex = ccIndex;
		this.mnemonics = FormatterString.create(mnemonics);
		this.flags = flags;
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = this.flags;
		int instrBitness = getBitness(instruction.getCodeSize());
		if (flags != InstrOpInfoFlags.NONE) {
			if (instrBitness != 0 && instrBitness != bitness) {
				if (bitness == 16)
					flags |= InstrOpInfoFlags.OP_SIZE16;
				else if (bitness == 32)
					flags |= InstrOpInfoFlags.OP_SIZE32;
				else
					flags |= InstrOpInfoFlags.OP_SIZE64;
			}
		}
		else {
			int branchInfo = BranchSizeInfo.NEAR;
			if (instrBitness != 0 && instrBitness != bitness) {
				if (bitness == 16)
					branchInfo = BranchSizeInfo.NEAR_WORD;
				else if (bitness == 32)
					branchInfo = BranchSizeInfo.NEAR_DWORD;
			}
			flags |= branchInfo << InstrOpInfoFlags.BRANCH_SIZE_INFO_SHIFT;
		}
		int prefixSeg = instruction.getSegmentPrefix();
		if (prefixSeg == Register.CS)
			flags |= InstrOpInfoFlags.JCC_NOT_TAKEN;
		else if (prefixSeg == Register.DS)
			flags |= InstrOpInfoFlags.JCC_TAKEN;
		if (instruction.getRepnePrefix())
			flags |= InstrOpInfoFlags.BND_PREFIX;
		FormatterString mnemonic = com.github.icedland.iced.x86.internal.fmt.MnemonicCC.getMnemonicCC(options, ccIndex, mnemonics);
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_os_loop extends InstrInfo {
	private final int bitness;
	private final int ccIndex;
	private final int register;
	private final FormatterString[] mnemonics;

	SimpleInstrInfo_os_loop(int bitness, int ccIndex, int register, String[] mnemonics) {
		this.bitness = bitness;
		this.ccIndex = ccIndex;
		this.register = register;
		this.mnemonics = FormatterString.create(mnemonics);
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = InstrOpInfoFlags.NONE;
		int instrBitness = getBitness(instruction.getCodeSize());
		int expectedReg;
		switch (instrBitness) {
		case 0:
			expectedReg = register;
			break;
		case 16:
			expectedReg = Register.CX;
			break;
		case 32:
			expectedReg = Register.ECX;
			break;
		case 64:
			expectedReg = Register.RCX;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		boolean addReg = expectedReg != register;
		if (instrBitness != 0 && instrBitness != bitness) {
			if (bitness == 16)
				flags |= InstrOpInfoFlags.OP_SIZE16;
			else if (bitness == 32)
				flags |= InstrOpInfoFlags.OP_SIZE32;
			else
				flags |= InstrOpInfoFlags.OP_SIZE64;
		}
		FormatterString mnemonic = ccIndex == -1 ? mnemonics[0]
				: com.github.icedland.iced.x86.internal.fmt.MnemonicCC.getMnemonicCC(options, ccIndex, mnemonics);
		InstrOpInfo info = new InstrOpInfo(mnemonic, instruction, flags);
		if (addReg) {
			assert info.OpCount == 1 : info.OpCount;
			info.OpCount = 2;
			info.Op1Kind = InstrOpKind.REGISTER;
			info.Op1Index = OP_ACCESS_READ_WRITE;
			info.Op1Register = (byte)register;
		}
		return info;
	}
}

final class SimpleInstrInfo_os_call extends InstrInfo {
	private final int bitness;
	private final FormatterString mnemonic;
	private final boolean canHaveBndPrefix;

	SimpleInstrInfo_os_call(int bitness, String mnemonic, boolean canHaveBndPrefix) {
		this.bitness = bitness;
		this.mnemonic = new FormatterString(mnemonic);
		this.canHaveBndPrefix = canHaveBndPrefix;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = InstrOpInfoFlags.NONE;
		if (canHaveBndPrefix && instruction.getRepnePrefix())
			flags |= InstrOpInfoFlags.BND_PREFIX;
		int instrBitness = getBitness(instruction.getCodeSize());
		int branchInfo = BranchSizeInfo.NONE;
		if (instrBitness != 0 && instrBitness != bitness) {
			if (bitness == 16)
				branchInfo = BranchSizeInfo.WORD;
			else if (bitness == 32)
				branchInfo = BranchSizeInfo.DWORD;
		}
		flags |= branchInfo << InstrOpInfoFlags.BRANCH_SIZE_INFO_SHIFT;
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_far extends InstrInfo {
	private final int bitness;
	private final FormatterString mnemonic;

	SimpleInstrInfo_far(int bitness, String mnemonic) {
		this.bitness = bitness;
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = InstrOpInfoFlags.NONE;
		int instrBitness = getBitness(instruction.getCodeSize());
		int branchInfo = BranchSizeInfo.NONE;
		if (instrBitness != 0 && instrBitness != bitness) {
			if (bitness == 16)
				branchInfo = BranchSizeInfo.WORD;
			else
				branchInfo = BranchSizeInfo.DWORD;
		}
		flags |= branchInfo << InstrOpInfoFlags.BRANCH_SIZE_INFO_SHIFT;
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_far_mem extends InstrInfo {
	private final int bitness;
	private final FormatterString mnemonic;

	SimpleInstrInfo_far_mem(int bitness, String mnemonic) {
		this.bitness = bitness;
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = InstrOpInfoFlags.SHOW_NO_MEM_SIZE_FORCE_SIZE;
		int instrBitness = getBitness(instruction.getCodeSize());
		int farMemSizeInfo = FarMemorySizeInfo.NONE;
		if (instrBitness != 0 && instrBitness != bitness) {
			if (bitness == 16)
				farMemSizeInfo = FarMemorySizeInfo.WORD;
			else
				farMemSizeInfo = FarMemorySizeInfo.DWORD;
		}
		flags |= farMemSizeInfo << InstrOpInfoFlags.FAR_MEMORY_SIZE_INFO_SHIFT;
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_movabs extends InstrInfo {
	private final FormatterString mnemonic;

	SimpleInstrInfo_movabs(String mnemonic) {
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = InstrOpInfoFlags.NONE;
		int instrBitness = getBitness(instruction.getCodeSize());
		int memSize;
		switch (instruction.getMemoryDisplSize()) {
		case 2:
			memSize = 16;
			break;
		case 4:
			memSize = 32;
			break;
		default:
			memSize = 64;
			break;
		}
		if (instrBitness == 0)
			instrBitness = memSize;
		int memSizeInfo = MemorySizeInfo.NONE;
		if (instrBitness == 64) {
			if (memSize == 32)
				flags |= InstrOpInfoFlags.ADDR_SIZE32;
			else
				memSizeInfo = MemorySizeInfo.QWORD;
		}
		else if (instrBitness != memSize) {
			assert memSize == 16 || memSize == 32 : memSize;
			if (memSize == 16)
				memSizeInfo = MemorySizeInfo.WORD;
			else
				memSizeInfo = MemorySizeInfo.DWORD;
		}
		flags |= memSizeInfo << InstrOpInfoFlags.MEMORY_SIZE_INFO_SHIFT;
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_er extends InstrInfo {
	private final int erIndex;
	private final FormatterString mnemonic;
	private final int flags;

	SimpleInstrInfo_er(int erIndex, String mnemonic) {
		this(erIndex, mnemonic, InstrOpInfoFlags.NONE);
	}

	SimpleInstrInfo_er(int erIndex, String mnemonic, int flags) {
		this.erIndex = erIndex;
		this.mnemonic = new FormatterString(mnemonic);
		this.flags = flags;
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo(mnemonic, instruction, flags);
		if (MvexInfo.isMvex(instruction.getCode())) {
			int rc = instruction.getRoundingControl();
			if (rc != RoundingControl.NONE) {
				int rcOpKind;
				if (instruction.getSuppressAllExceptions()) {
					switch (rc) {
					case RoundingControl.ROUND_TO_NEAREST:
						rcOpKind = InstrOpKind.RN_SAE;
						break;
					case RoundingControl.ROUND_DOWN:
						rcOpKind = InstrOpKind.RD_SAE;
						break;
					case RoundingControl.ROUND_UP:
						rcOpKind = InstrOpKind.RU_SAE;
						break;
					case RoundingControl.ROUND_TOWARD_ZERO:
						rcOpKind = InstrOpKind.RZ_SAE;
						break;
					default:
						return info;
					}
				}
				else {
					switch (rc) {
					case RoundingControl.ROUND_TO_NEAREST:
						rcOpKind = InstrOpKind.RN;
						break;
					case RoundingControl.ROUND_DOWN:
						rcOpKind = InstrOpKind.RD;
						break;
					case RoundingControl.ROUND_UP:
						rcOpKind = InstrOpKind.RU;
						break;
					case RoundingControl.ROUND_TOWARD_ZERO:
						rcOpKind = InstrOpKind.RZ;
						break;
					default:
						return info;
					}
				}
				moveOperands(info, erIndex, rcOpKind);
			}
			else if (instruction.getSuppressAllExceptions())
				SimpleInstrInfo_er.moveOperands(info, erIndex, InstrOpKind.SAE);
		}
		else {
			int rc = instruction.getRoundingControl();
			if (rc != RoundingControl.NONE) {
				if (!com.github.icedland.iced.x86.internal.fmt.FormatterUtils.canShowRoundingControl(instruction, options))
					return info;
				int rcOpKind;
				switch (rc) {
				case RoundingControl.ROUND_TO_NEAREST:
					rcOpKind = InstrOpKind.RN_SAE;
					break;
				case RoundingControl.ROUND_DOWN:
					rcOpKind = InstrOpKind.RD_SAE;
					break;
				case RoundingControl.ROUND_UP:
					rcOpKind = InstrOpKind.RU_SAE;
					break;
				case RoundingControl.ROUND_TOWARD_ZERO:
					rcOpKind = InstrOpKind.RZ_SAE;
					break;
				default:
					return info;
				}
				moveOperands(info, erIndex, rcOpKind);
			}
		}
		return info;
	}

	static void moveOperands(InstrOpInfo info, int index, int newOpKind) {
		assert info.OpCount <= 4 : info.OpCount;

		switch (index) {
		case 2:
			assert info.OpCount < 4 || info.Op3Kind != InstrOpKind.REGISTER;
			info.Op4Kind = info.Op3Kind;
			info.Op3Kind = info.Op2Kind;
			info.Op3Register = info.Op2Register;
			info.Op2Kind = (byte)newOpKind;
			info.Op4Index = info.Op3Index;
			info.Op3Index = info.Op2Index;
			info.Op2Index = OP_ACCESS_NONE;
			info.OpCount++;
			break;

		case 3:
			assert info.OpCount < 4 || info.Op3Kind != InstrOpKind.REGISTER;
			info.Op4Kind = info.Op3Kind;
			info.Op3Kind = (byte)newOpKind;
			info.Op4Index = info.Op3Index;
			info.Op3Index = OP_ACCESS_NONE;
			info.OpCount++;
			break;

		default:
			throw new UnsupportedOperationException();
		}
	}
}

final class SimpleInstrInfo_sae extends InstrInfo {
	private final int saeIndex;
	private final FormatterString mnemonic;

	SimpleInstrInfo_sae(int saeIndex, String mnemonic) {
		this.saeIndex = saeIndex;
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo(mnemonic, instruction, InstrOpInfoFlags.NONE);
		if (instruction.getSuppressAllExceptions())
			SimpleInstrInfo_er.moveOperands(info, saeIndex, InstrOpKind.SAE);
		return info;
	}
}

final class SimpleInstrInfo_bcst extends InstrInfo {
	private final FormatterString mnemonic;
	private final int flagsNoBroadcast;

	SimpleInstrInfo_bcst(String mnemonic, int flagsNoBroadcast) {
		this.mnemonic = new FormatterString(mnemonic);
		this.flagsNoBroadcast = flagsNoBroadcast;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		MemorySizes.Info memInfo = MemorySizes.allMemorySizes[instruction.getMemorySize()];
		int flags = memInfo.bcstTo.getLength() != 0 ? InstrOpInfoFlags.NONE : flagsNoBroadcast;
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_bnd extends InstrInfo {
	private final FormatterString mnemonic;
	private final int flags;

	SimpleInstrInfo_bnd(String mnemonic, int flags) {
		this.mnemonic = new FormatterString(mnemonic);
		this.flags = flags;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = this.flags;
		if (instruction.getRepnePrefix())
			flags |= InstrOpInfoFlags.BND_PREFIX;
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_pops extends InstrInfo {
	private final FormatterString mnemonic;
	private final FormatterString[] pseudo_ops;

	SimpleInstrInfo_pops(String mnemonic, FormatterString[] pseudo_ops) {
		this.mnemonic = new FormatterString(mnemonic);
		this.pseudo_ops = pseudo_ops;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo(mnemonic, instruction, InstrOpInfoFlags.NONE);
		if (instruction.getSuppressAllExceptions())
			SimpleInstrInfo_er.moveOperands(info, instruction.getOpCount() - 1, InstrOpKind.SAE);
		int imm = instruction.getImmediate8();
		if (options.getUsePseudoOps() && Integer.compareUnsigned(imm, pseudo_ops.length) < 0) {
			info.Mnemonic = pseudo_ops[imm];
			removeLastOp(info);
		}
		return info;
	}

	static void removeLastOp(InstrOpInfo info) {
		switch (info.OpCount) {
		case 5:
			info.Op4Index = OP_ACCESS_INVALID;
			break;
		case 4:
			info.Op3Index = OP_ACCESS_INVALID;
			break;
		case 3:
			info.Op2Index = OP_ACCESS_INVALID;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		info.OpCount--;
	}
}

final class SimpleInstrInfo_pclmulqdq extends InstrInfo {
	private final FormatterString mnemonic;
	private final FormatterString[] pseudo_ops;

	SimpleInstrInfo_pclmulqdq(String mnemonic, FormatterString[] pseudo_ops) {
		this.mnemonic = new FormatterString(mnemonic);
		this.pseudo_ops = pseudo_ops;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo(mnemonic, instruction, InstrOpInfoFlags.NONE);
		if (options.getUsePseudoOps()) {
			int index;
			int imm = instruction.getImmediate8();
			if (imm == 0)
				index = 0;
			else if (imm == 1)
				index = 1;
			else if (imm == 0x10)
				index = 2;
			else if (imm == 0x11)
				index = 3;
			else
				index = -1;
			if (index >= 0) {
				info.Mnemonic = pseudo_ops[index];
				SimpleInstrInfo_pops.removeLastOp(info);
			}
		}
		return info;
	}
}

final class SimpleInstrInfo_Reg16 extends InstrInfo {
	private final FormatterString mnemonic;

	SimpleInstrInfo_Reg16(String mnemonic) {
		this.mnemonic = new FormatterString(mnemonic);
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		final int REG_MASK = com.github.icedland.iced.x86.internal.Constants.REG_MASK;
		final int flags = InstrOpInfoFlags.NONE;
		InstrOpInfo info = new InstrOpInfo(mnemonic, instruction, flags);
		if (Register.EAX <= (info.Op0Register & REG_MASK) && (info.Op0Register & REG_MASK) <= Register.R15D)
			info.Op0Register = (byte)((info.Op0Register & REG_MASK) - Register.EAX + Register.AX);
		if (Register.EAX <= (info.Op1Register & REG_MASK) && (info.Op1Register & REG_MASK) <= Register.R15D)
			info.Op1Register = (byte)((info.Op1Register & REG_MASK) - Register.EAX + Register.AX);
		if (Register.EAX <= (info.Op2Register & REG_MASK) && (info.Op2Register & REG_MASK) <= Register.R15D)
			info.Op2Register = (byte)((info.Op2Register & REG_MASK) - Register.EAX + Register.AX);
		return info;
	}
}

final class SimpleInstrInfo_Reg32 extends InstrInfo {
	private final FormatterString mnemonic;

	SimpleInstrInfo_Reg32(String mnemonic) {
		this.mnemonic = new FormatterString(mnemonic);
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		final int REG_MASK = com.github.icedland.iced.x86.internal.Constants.REG_MASK;
		final int flags = InstrOpInfoFlags.NONE;
		InstrOpInfo info = new InstrOpInfo(mnemonic, instruction, flags);
		if (Register.RAX <= (info.Op0Register & REG_MASK) && (info.Op0Register & REG_MASK) <= Register.R15)
			info.Op0Register = (byte)((info.Op0Register & REG_MASK) - Register.RAX + Register.EAX);
		if (Register.RAX <= (info.Op1Register & REG_MASK) && (info.Op1Register & REG_MASK) <= Register.R15)
			info.Op1Register = (byte)((info.Op1Register & REG_MASK) - Register.RAX + Register.EAX);
		if (Register.RAX <= (info.Op2Register & REG_MASK) && (info.Op2Register & REG_MASK) <= Register.R15)
			info.Op2Register = (byte)((info.Op2Register & REG_MASK) - Register.RAX + Register.EAX);
		return info;
	}
}

final class SimpleInstrInfo_invlpga extends InstrInfo {
	private final int bitness;
	private final FormatterString mnemonic;

	SimpleInstrInfo_invlpga(int bitness, String mnemonic) {
		this.bitness = bitness;
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo();
		info.Mnemonic = mnemonic;
		info.OpCount = 2;
		info.Op0Kind = InstrOpKind.REGISTER;
		info.Op1Kind = InstrOpKind.REGISTER;
		info.Op1Register = (byte)Register.ECX;
		info.Op0Index = OP_ACCESS_READ;
		info.Op1Index = OP_ACCESS_READ;

		switch (bitness) {
		case 16:
			info.Op0Register = (byte)Register.AX;
			break;

		case 32:
			info.Op0Register = (byte)Register.EAX;
			break;

		case 64:
			info.Op0Register = (byte)Register.RAX;
			break;

		default:
			throw new UnsupportedOperationException();
		}
		return info;
	}
}

final class SimpleInstrInfo_DeclareData extends InstrInfo {
	private final FormatterString mnemonic;
	private final int opKind;

	SimpleInstrInfo_DeclareData(int code, String mnemonic) {
		this.mnemonic = new FormatterString(mnemonic);
		switch (code) {
		case Code.DECLAREBYTE:
			opKind = InstrOpKind.DECLARE_BYTE;
			break;
		case Code.DECLAREWORD:
			opKind = InstrOpKind.DECLARE_WORD;
			break;
		case Code.DECLAREDWORD:
			opKind = InstrOpKind.DECLARE_DWORD;
			break;
		case Code.DECLAREQWORD:
			opKind = InstrOpKind.DECLARE_QWORD;
			break;
		default:
			throw new UnsupportedOperationException();
		}
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo(mnemonic, instruction, InstrOpInfoFlags.MNEMONIC_IS_DIRECTIVE);
		info.OpCount = (byte)instruction.getDeclareDataCount();
		info.Op0Kind = (byte)opKind;
		info.Op1Kind = (byte)opKind;
		info.Op2Kind = (byte)opKind;
		info.Op3Kind = (byte)opKind;
		info.Op4Kind = (byte)opKind;
		info.Op0Index = OP_ACCESS_READ;
		info.Op1Index = OP_ACCESS_READ;
		info.Op2Index = OP_ACCESS_READ;
		info.Op3Index = OP_ACCESS_READ;
		info.Op4Index = OP_ACCESS_READ;
		return info;
	}
}
