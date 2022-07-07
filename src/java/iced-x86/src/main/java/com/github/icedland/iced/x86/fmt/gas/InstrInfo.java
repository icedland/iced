// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt.gas;

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
	FormatterString mnemonic;
	int flags;
	byte opCount;
	byte op0Kind;
	byte op1Kind;
	byte op2Kind;
	byte op3Kind;
	byte op4Kind;
	byte op0Register;
	byte op1Register;
	byte op2Register;
	byte op3Register;
	byte op4Register;
	byte op0Index;
	byte op1Index;
	byte op2Index;
	byte op3Index;
	byte op4Index;

	@SuppressWarnings("deprecation")
	int getOpRegister(int operand) {
		final int REG_MASK = com.github.icedland.iced.x86.internal.Constants.REG_MASK;
		switch (operand) {
		case 0:
			return op0Register & REG_MASK;
		case 1:
			return op1Register & REG_MASK;
		case 2:
			return op2Register & REG_MASK;
		case 3:
			return op3Register & REG_MASK;
		case 4:
			return op4Register & REG_MASK;
		default:
			throw new IllegalArgumentException("operand");
		}
	}

	int getOpKind(int operand) {
		switch (operand) {
		case 0:
			return op0Kind;
		case 1:
			return op1Kind;
		case 2:
			return op2Kind;
		case 3:
			return op3Kind;
		case 4:
			return op4Kind;
		default:
			assert op0Kind == InstrOpKind.DECLARE_BYTE || op0Kind == InstrOpKind.DECLARE_WORD || op0Kind == InstrOpKind.DECLARE_DWORD
					|| op0Kind == InstrOpKind.DECLARE_QWORD : op0Kind;
			return op0Kind;
		}
	}

	int getInstructionIndex(int operand) {
		int instructionOperand;
		switch (operand) {
		case 0:
			instructionOperand = op0Index;
			break;
		case 1:
			instructionOperand = op1Index;
			break;
		case 2:
			instructionOperand = op2Index;
			break;
		case 3:
			instructionOperand = op3Index;
			break;
		case 4:
			instructionOperand = op4Index;
			break;
		default:
			assert op0Kind == InstrOpKind.DECLARE_BYTE || op0Kind == InstrOpKind.DECLARE_WORD || op0Kind == InstrOpKind.DECLARE_DWORD
					|| op0Kind == InstrOpKind.DECLARE_QWORD : op0Kind;
			instructionOperand = -1;
			break;
		}
		return instructionOperand < 0 ? -1 : instructionOperand;
	}

	Integer tryGetOpAccess(int operand) {
		int instructionOperand;
		switch (operand) {
		case 0:
			instructionOperand = op0Index;
			break;
		case 1:
			instructionOperand = op1Index;
			break;
		case 2:
			instructionOperand = op2Index;
			break;
		case 3:
			instructionOperand = op3Index;
			break;
		case 4:
			instructionOperand = op4Index;
			break;
		default:
			assert op0Kind == InstrOpKind.DECLARE_BYTE || op0Kind == InstrOpKind.DECLARE_WORD || op0Kind == InstrOpKind.DECLARE_DWORD
					|| op0Kind == InstrOpKind.DECLARE_QWORD : op0Kind;
			instructionOperand = op0Index;
			break;
		}
		if (instructionOperand < InstrInfo.OP_ACCESS_INVALID)
			return -instructionOperand - 2;
		return null;
	}

	int getOperandIndex(int instructionOperand) {
		int index;
		if (instructionOperand == op0Index)
			index = 0;
		else if (instructionOperand == op1Index)
			index = 1;
		else if (instructionOperand == op2Index)
			index = 2;
		else if (instructionOperand == op3Index)
			index = 3;
		else if (instructionOperand == op4Index)
			index = 4;
		else
			index = -1;
		return index < opCount ? index : -1;
	}

	InstrOpInfo() {
	}

	InstrOpInfo(FormatterString mnemonic, Instruction instruction, int flags) {
		this.mnemonic = mnemonic;
		this.flags = flags;
		int opCount = instruction.getOpCount();
		this.opCount = (byte)opCount;
		if ((flags & InstrOpInfoFlags.KEEP_OPERAND_ORDER) != 0) {
			op0Kind = (byte)instruction.getOp0Kind();
			op1Kind = (byte)instruction.getOp1Kind();
			op2Kind = (byte)instruction.getOp2Kind();
			op3Kind = (byte)instruction.getOp3Kind();
			op4Kind = (byte)instruction.getOp4Kind();
			op0Register = (byte)instruction.getOp0Register();
			op1Register = (byte)instruction.getOp1Register();
			op2Register = (byte)instruction.getOp2Register();
			op3Register = (byte)instruction.getOp3Register();
			op4Register = (byte)instruction.getOp4Register();
		}
		else {
			switch (opCount) {
			case 0:
				op0Kind = 0;
				op1Kind = 0;
				op2Kind = 0;
				op3Kind = 0;
				op4Kind = 0;
				op0Register = 0;
				op1Register = 0;
				op2Register = 0;
				op3Register = 0;
				op4Register = 0;
				break;

			case 1:
				op0Kind = (byte)instruction.getOp0Kind();
				op1Kind = 0;
				op2Kind = 0;
				op3Kind = 0;
				op4Kind = 0;
				op0Register = (byte)instruction.getOp0Register();
				op1Register = 0;
				op2Register = 0;
				op3Register = 0;
				op4Register = 0;
				break;

			case 2:
				op0Kind = (byte)instruction.getOp1Kind();
				op1Kind = (byte)instruction.getOp0Kind();
				op2Kind = 0;
				op3Kind = 0;
				op4Kind = 0;
				op0Register = (byte)instruction.getOp1Register();
				op1Register = (byte)instruction.getOp0Register();
				op2Register = 0;
				op3Register = 0;
				op4Register = 0;
				break;

			case 3:
				op0Kind = (byte)instruction.getOp2Kind();
				op1Kind = (byte)instruction.getOp1Kind();
				op2Kind = (byte)instruction.getOp0Kind();
				op3Kind = 0;
				op4Kind = 0;
				op0Register = (byte)instruction.getOp2Register();
				op1Register = (byte)instruction.getOp1Register();
				op2Register = (byte)instruction.getOp0Register();
				op3Register = 0;
				op4Register = 0;
				break;

			case 4:
				op0Kind = (byte)instruction.getOp3Kind();
				op1Kind = (byte)instruction.getOp2Kind();
				op2Kind = (byte)instruction.getOp1Kind();
				op3Kind = (byte)instruction.getOp0Kind();
				op4Kind = 0;
				op0Register = (byte)instruction.getOp3Register();
				op1Register = (byte)instruction.getOp2Register();
				op2Register = (byte)instruction.getOp1Register();
				op3Register = (byte)instruction.getOp0Register();
				op4Register = 0;
				break;

			case 5:
				op0Kind = (byte)instruction.getOp4Kind();
				op1Kind = (byte)instruction.getOp3Kind();
				op2Kind = (byte)instruction.getOp2Kind();
				op3Kind = (byte)instruction.getOp1Kind();
				op4Kind = (byte)instruction.getOp0Kind();
				op0Register = (byte)instruction.getOp4Register();
				op1Register = (byte)instruction.getOp3Register();
				op2Register = (byte)instruction.getOp2Register();
				op3Register = (byte)instruction.getOp1Register();
				op4Register = (byte)instruction.getOp0Register();
				break;

			default:
				throw new UnsupportedOperationException();
			}
		}
		switch (opCount) {
		case 0:
			op0Index = InstrInfo.OP_ACCESS_INVALID;
			op1Index = InstrInfo.OP_ACCESS_INVALID;
			op2Index = InstrInfo.OP_ACCESS_INVALID;
			op3Index = InstrInfo.OP_ACCESS_INVALID;
			op4Index = InstrInfo.OP_ACCESS_INVALID;
			break;

		case 1:
			op0Index = 0;
			op1Index = InstrInfo.OP_ACCESS_INVALID;
			op2Index = InstrInfo.OP_ACCESS_INVALID;
			op3Index = InstrInfo.OP_ACCESS_INVALID;
			op4Index = InstrInfo.OP_ACCESS_INVALID;
			break;

		case 2:
			op0Index = 1;
			op1Index = 0;
			op2Index = InstrInfo.OP_ACCESS_INVALID;
			op3Index = InstrInfo.OP_ACCESS_INVALID;
			op4Index = InstrInfo.OP_ACCESS_INVALID;
			break;

		case 3:
			op0Index = 2;
			op1Index = 1;
			op2Index = 0;
			op3Index = InstrInfo.OP_ACCESS_INVALID;
			op4Index = InstrInfo.OP_ACCESS_INVALID;
			break;

		case 4:
			op0Index = 3;
			op1Index = 2;
			op2Index = 1;
			op3Index = 0;
			op4Index = InstrInfo.OP_ACCESS_INVALID;
			break;

		case 5:
			op0Index = 4;
			op1Index = 3;
			op2Index = 2;
			op3Index = 1;
			op4Index = 0;
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

	protected static FormatterString getMnemonic(FormatterOptions options, Instruction instruction, FormatterString mnemonic,
			FormatterString mnemonic_suffix, int flags) {
		if (options.getGasShowMnemonicSizeSuffix())
			return mnemonic_suffix;
		if ((flags & InstrOpInfoFlags.MNEMONIC_SUFFIX_IF_MEM) != 0 && MemorySizes.allMemorySizes[instruction.getMemorySize()].getLength() == 0) {
			if (instruction.getOp0Kind() == OpKind.MEMORY ||
					instruction.getOp1Kind() == OpKind.MEMORY ||
					instruction.getOp2Kind() == OpKind.MEMORY) {
				return mnemonic_suffix;
			}
		}
		return mnemonic;
	}
}

final class SimpleInstrInfo extends InstrInfo {
	private final FormatterString mnemonic;
	private final FormatterString mnemonic_suffix;
	private final int flags;

	SimpleInstrInfo(String mnemonic) {
		this(mnemonic, mnemonic, InstrOpInfoFlags.NONE);
	}

	SimpleInstrInfo(String mnemonic, int flags) {
		this(mnemonic, mnemonic, flags);
	}

	SimpleInstrInfo(String mnemonic, String mnemonic_suffix) {
		this(mnemonic, mnemonic_suffix, InstrOpInfoFlags.NONE);
	}

	SimpleInstrInfo(String mnemonic, String mnemonic_suffix, int flags) {
		this.mnemonic = new FormatterString(mnemonic);
		this.mnemonic_suffix = new FormatterString(mnemonic_suffix);
		this.flags = flags;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		return new InstrOpInfo(getMnemonic(options, instruction, mnemonic, mnemonic_suffix, flags), instruction, flags);
	}
}

final class SimpleInstrInfo_cc extends InstrInfo {
	private final int ccIndex;
	private final FormatterString[] mnemonics;
	private final FormatterString[] mnemonics_suffix;

	SimpleInstrInfo_cc(int ccIndex, String[] mnemonics, String[] mnemonics_suffix) {
		this.ccIndex = ccIndex;
		this.mnemonics = FormatterString.create(mnemonics);
		this.mnemonics_suffix = FormatterString.create(mnemonics_suffix);
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		final int flags = InstrOpInfoFlags.NONE;
		FormatterString mnemonic = com.github.icedland.iced.x86.internal.fmt.MnemonicCC.getMnemonicCC(options, ccIndex, mnemonics);
		FormatterString mnemonic_suffix = com.github.icedland.iced.x86.internal.fmt.MnemonicCC.getMnemonicCC(options, ccIndex, mnemonics_suffix);
		return new InstrOpInfo(getMnemonic(options, instruction, mnemonic, mnemonic_suffix, flags), instruction, flags);
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
			info.mnemonic = mnemonic;
			return info;
		}
		else
			return new InstrOpInfo(mnemonic, instruction, InstrOpInfoFlags.NONE);
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
	private static final FormatterString str_xchgw = new FormatterString("xchgw");
	private static final FormatterString str_xchgl = new FormatterString("xchgl");
	private static final FormatterString str_xchgq = new FormatterString("xchgq");

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int instrBitness = getBitness(instruction.getCodeSize());
		if (instrBitness == 0 || (instrBitness & bitness) != 0)
			return new InstrOpInfo(mnemonic, instruction, InstrOpInfoFlags.NONE);
		else {
			InstrOpInfo info = new InstrOpInfo();
			if (!options.getGasShowMnemonicSizeSuffix())
				info.mnemonic = str_xchg;
			else if (register == Register.AX)
				info.mnemonic = str_xchgw;
			else if (register == Register.EAX)
				info.mnemonic = str_xchgl;
			else if (register == Register.RAX)
				info.mnemonic = str_xchgq;
			else
				throw new UnsupportedOperationException();
			info.opCount = 2;
			info.op0Register = (byte)register;
			info.op1Register = (byte)register;
			info.op0Index = OP_ACCESS_NONE;
			info.op1Index = OP_ACCESS_NONE;
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
		info.mnemonic = mnemonic;
		assert instruction.getOpCount() == 2 : instruction.getOpCount();
		assert instruction.getOp0Kind() == OpKind.REGISTER && instruction.getOp0Register() == Register.ST0;
		if (!pseudoOp || !(options.getUsePseudoOps() && instruction.getOp1Register() == Register.ST1)) {
			info.opCount = 1;
			info.op0Register = (byte)instruction.getOp1Register();
			info.op0Index = 1;
		}
		return info;
	}
}

final class SimpleInstrInfo_STi_ST extends InstrInfo {
	private final FormatterString mnemonic;
	private final boolean pseudoOp;

	SimpleInstrInfo_STi_ST(String mnemonic, boolean pseudoOp) {
		this.mnemonic = new FormatterString(mnemonic);
		this.pseudoOp = pseudoOp;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		final int flags = 0;
		InstrOpInfo info;
		if (pseudoOp && options.getUsePseudoOps() && (instruction.getOp0Register() == Register.ST1 || instruction.getOp1Register() == Register.ST1)) {
			info = new InstrOpInfo();
			info.mnemonic = mnemonic;
		}
		else {
			info = new InstrOpInfo(mnemonic, instruction, flags);
			assert info.getOpRegister(0) == Register.ST0 : info.getOpRegister(0);
			info.op0Register = (byte)Registers.REGISTER_ST;
		}
		return info;
	}
}

final class SimpleInstrInfo_ST_STi extends InstrInfo {
	private final FormatterString mnemonic;

	SimpleInstrInfo_ST_STi(String mnemonic) {
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo(mnemonic, instruction, InstrOpInfoFlags.NONE);
		assert info.getOpRegister(1) == Register.ST0 : info.getOpRegister(1);
		info.op1Register = (byte)Registers.REGISTER_ST;
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
		int flags = 0;
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
		info.mnemonic = mnemonic;
		info.opCount = 2;
		info.op0Kind = (byte)instruction.getOp2Kind();
		info.op0Register = (byte)instruction.getOp2Register();
		info.op0Index = 2;
		info.op1Kind = (byte)instruction.getOp1Kind();
		info.op1Register = (byte)instruction.getOp1Register();
		info.op1Index = 1;
		if (instrBitness != 0 && instrBitness != bitness) {
			if (bitness == 16)
				info.flags |= InstrOpInfoFlags.ADDR_SIZE16;
			else if (bitness == 32)
				info.flags |= InstrOpInfoFlags.ADDR_SIZE32;
			else
				info.flags |= InstrOpInfoFlags.ADDR_SIZE64;
		}
		return info;
	}
}

final class SimpleInstrInfo_pblendvb extends InstrInfo {
	private final FormatterString mnemonic;

	SimpleInstrInfo_pblendvb(String mnemonic) {
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo();
		assert instruction.getOpCount() == 2 : instruction.getOpCount();
		info.mnemonic = mnemonic;
		info.opCount = 3;
		info.op0Register = (byte)Register.XMM0;
		info.op0Index = OP_ACCESS_READ;
		info.op1Kind = (byte)instruction.getOp1Kind();
		info.op1Index = 1;
		info.op1Register = (byte)instruction.getOp1Register();
		info.op2Kind = (byte)instruction.getOp0Kind();
		info.op2Register = (byte)instruction.getOp0Register();
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
		if (instruction.getCodeSize() == codeSize && !options.getGasShowMnemonicSizeSuffix())
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
		int codeSize = instruction.getCodeSize();
		if (options.getGasShowMnemonicSizeSuffix())
			codeSize = CodeSize.CODE64;
		FormatterString mnemonic = mnemonics[codeSize];
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_OpSize3 extends InstrInfo {
	private final int bitness;
	private final FormatterString mnemonic;
	private final FormatterString mnemonic_suffix;

	SimpleInstrInfo_OpSize3(int bitness, String mnemonic, String mnemonic_suffix) {
		this.bitness = bitness;
		this.mnemonic = new FormatterString(mnemonic);
		this.mnemonic_suffix = new FormatterString(mnemonic_suffix);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int instrBitness = getBitness(instruction.getCodeSize());
		FormatterString mnemonic;
		if (!options.getGasShowMnemonicSizeSuffix() && (instrBitness == 0 || (instrBitness & bitness) != 0))
			mnemonic = this.mnemonic;
		else
			mnemonic = mnemonic_suffix;
		return new InstrOpInfo(mnemonic, instruction, InstrOpInfoFlags.NONE);
	}
}

final class SimpleInstrInfo_os2 extends InstrInfo {
	private final int bitness;
	private final FormatterString mnemonic;
	private final FormatterString mnemonic_suffix;
	private final boolean canUseBnd;
	private final int flags;

	SimpleInstrInfo_os2(int bitness, String mnemonic, String mnemonic_suffix, boolean canUseBnd, int flags) {
		this.bitness = bitness;
		this.mnemonic = new FormatterString(mnemonic);
		this.mnemonic_suffix = new FormatterString(mnemonic_suffix);
		this.canUseBnd = canUseBnd;
		this.flags = flags;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = this.flags;
		if (canUseBnd && instruction.getRepnePrefix())
			flags |= InstrOpInfoFlags.BND_PREFIX;
		FormatterString mnemonic;
		int instrBitness = getBitness(instruction.getCodeSize());
		if (instrBitness != 0 && instrBitness != bitness)
			mnemonic = mnemonic_suffix;
		else
			mnemonic = getMnemonic(options, instruction, this.mnemonic, mnemonic_suffix, flags);
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_os extends InstrInfo {
	private final int bitness;
	private final FormatterString mnemonic;
	private final boolean canUseBnd;
	private final int flags;

	SimpleInstrInfo_os(int bitness, String mnemonic, boolean canUseBnd, int flags) {
		this.bitness = bitness;
		this.mnemonic = new FormatterString(mnemonic);
		this.canUseBnd = canUseBnd;
		this.flags = flags;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = this.flags;
		if (canUseBnd && instruction.getRepnePrefix())
			flags |= InstrOpInfoFlags.BND_PREFIX;
		int instrBitness = getBitness(instruction.getCodeSize());
		if (instrBitness != 0 && instrBitness != bitness) {
			if (bitness == 16)
				flags |= InstrOpInfoFlags.OP_SIZE16;
			else if (bitness == 32)
				flags |= InstrOpInfoFlags.OP_SIZE32;
			else
				flags |= InstrOpInfoFlags.OP_SIZE64;
		}
		return new InstrOpInfo(getMnemonic(options, instruction, mnemonic, mnemonic, flags), instruction, flags);
	}
}

final class SimpleInstrInfo_os_mem extends InstrInfo {
	private final int bitness;
	private final FormatterString mnemonic;
	private final FormatterString mnemonic_suffix;

	SimpleInstrInfo_os_mem(int bitness, String mnemonic, String mnemonic_suffix) {
		this.bitness = bitness;
		this.mnemonic = new FormatterString(mnemonic);
		this.mnemonic_suffix = new FormatterString(mnemonic_suffix);
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
		FormatterString mnemonic = hasMemOp ? this.mnemonic : getMnemonic(options, instruction, this.mnemonic, mnemonic_suffix, flags);
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_os_mem2 extends InstrInfo {
	private final int bitness;
	private final FormatterString mnemonic;
	private final FormatterString mnemonic_suffix;

	SimpleInstrInfo_os_mem2(int bitness, String mnemonic, String mnemonic_suffix) {
		this.bitness = bitness;
		this.mnemonic = new FormatterString(mnemonic);
		this.mnemonic_suffix = new FormatterString(mnemonic_suffix);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		final int flags = InstrOpInfoFlags.NONE;
		int instrBitness = getBitness(instruction.getCodeSize());
		FormatterString mnemonic;
		if (instrBitness != 0 && (instrBitness & bitness) == 0)
			mnemonic = mnemonic_suffix;
		else
			mnemonic = getMnemonic(options, instruction, this.mnemonic, mnemonic_suffix, flags);
		return new InstrOpInfo(mnemonic, instruction, InstrOpInfoFlags.NONE);
	}
}

final class SimpleInstrInfo_Reg16 extends InstrInfo {
	private final FormatterString mnemonic;
	private final FormatterString mnemonic_suffix;

	SimpleInstrInfo_Reg16(String mnemonic, String mnemonic_suffix) {
		this.mnemonic = new FormatterString(mnemonic);
		this.mnemonic_suffix = new FormatterString(mnemonic_suffix);
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		final int REG_MASK = com.github.icedland.iced.x86.internal.Constants.REG_MASK;
		int flags = InstrOpInfoFlags.NONE;
		InstrOpInfo info = new InstrOpInfo(getMnemonic(options, instruction, mnemonic, mnemonic_suffix, flags), instruction, flags);
		if (Register.EAX <= (info.op0Register & REG_MASK) && (info.op0Register & REG_MASK) <= Register.R15)
			info.op0Register = (byte)((((info.op0Register & REG_MASK) - Register.EAX) & 0xF) + Register.AX);
		if (Register.EAX <= (info.op1Register & REG_MASK) && (info.op1Register & REG_MASK) <= Register.R15)
			info.op1Register = (byte)((((info.op1Register & REG_MASK) - Register.EAX) & 0xF) + Register.AX);
		if (Register.EAX <= (info.op2Register & REG_MASK) && (info.op2Register & REG_MASK) <= Register.R15)
			info.op2Register = (byte)((((info.op2Register & REG_MASK) - Register.EAX) & 0xF) + Register.AX);
		return info;
	}
}

final class SimpleInstrInfo_mem16 extends InstrInfo {
	private final FormatterString mnemonic;
	private final FormatterString mnemonic_reg_suffix;
	private final FormatterString mnemonic_mem_suffix;

	SimpleInstrInfo_mem16(String mnemonic, String mnemonic_reg_suffix, String mnemonic_mem_suffix) {
		this.mnemonic = new FormatterString(mnemonic);
		this.mnemonic_reg_suffix = new FormatterString(mnemonic_reg_suffix);
		this.mnemonic_mem_suffix = new FormatterString(mnemonic_mem_suffix);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		final int flags = InstrOpInfoFlags.NONE;
		FormatterString mnemonic_suffix;
		if (instruction.getOp0Kind() == OpKind.MEMORY || instruction.getOp1Kind() == OpKind.MEMORY)
			mnemonic_suffix = mnemonic_mem_suffix;
		else
			mnemonic_suffix = mnemonic_reg_suffix;
		return new InstrOpInfo(getMnemonic(options, instruction, mnemonic, mnemonic_suffix, flags), instruction, flags);
	}
}

final class SimpleInstrInfo_os_loop extends InstrInfo {
	private final int bitness;
	private final int regSize;
	private final int ccIndex;
	private final FormatterString[] mnemonics;
	private final FormatterString[] mnemonics_suffix;

	SimpleInstrInfo_os_loop(int bitness, int regSize, int ccIndex, String[] mnemonics, String[] mnemonics_suffix) {
		this.bitness = bitness;
		this.regSize = regSize;
		this.ccIndex = ccIndex;
		this.mnemonics = FormatterString.create(mnemonics);
		this.mnemonics_suffix = FormatterString.create(mnemonics_suffix);
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = InstrOpInfoFlags.NONE;
		int instrBitness = getBitness(instruction.getCodeSize());
		FormatterString[] mnemonics = this.mnemonics;
		if ((instrBitness != 0 && instrBitness != regSize) || options.getGasShowMnemonicSizeSuffix())
			mnemonics = mnemonics_suffix;
		if (instrBitness != 0 && instrBitness != bitness) {
			if (bitness == 16)
				flags |= InstrOpInfoFlags.OP_SIZE16 | InstrOpInfoFlags.OP_SIZE_IS_BYTE_DIRECTIVE;
			else if (bitness == 32)
				flags |= InstrOpInfoFlags.OP_SIZE32 | InstrOpInfoFlags.OP_SIZE_IS_BYTE_DIRECTIVE;
			else
				flags |= InstrOpInfoFlags.OP_SIZE64;
		}
		FormatterString mnemonic = ccIndex == -1 ? mnemonics[0]
				: com.github.icedland.iced.x86.internal.fmt.MnemonicCC.getMnemonicCC(options, ccIndex, mnemonics);
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_os_jcc extends InstrInfo {
	private final int bitness;
	private final int ccIndex;
	private final FormatterString[] mnemonics;

	SimpleInstrInfo_os_jcc(int bitness, int ccIndex, String[] mnemonics) {
		this.bitness = bitness;
		this.ccIndex = ccIndex;
		this.mnemonics = FormatterString.create(mnemonics);
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = InstrOpInfoFlags.NONE;
		int instrBitness = getBitness(instruction.getCodeSize());
		if (instrBitness != 0 && instrBitness != bitness) {
			if (bitness == 16)
				flags |= InstrOpInfoFlags.OP_SIZE16;
			else if (bitness == 32)
				flags |= InstrOpInfoFlags.OP_SIZE32;
			else
				flags |= InstrOpInfoFlags.OP_SIZE64;
		}
		int prefixSeg = instruction.getSegmentPrefix();
		if (prefixSeg == Register.CS)
			flags |= InstrOpInfoFlags.JCC_NOT_TAKEN;
		else if (prefixSeg == Register.DS)
			flags |= InstrOpInfoFlags.JCC_TAKEN;
		if (instruction.getRepnePrefix())
			flags |= InstrOpInfoFlags.BND_PREFIX;
		FormatterString mnemonic = com.github.icedland.iced.x86.internal.fmt.MnemonicCC.getMnemonicCC(options, ccIndex, mnemonics);
		return new InstrOpInfo(getMnemonic(options, instruction, mnemonic, mnemonic, flags), instruction, flags);
	}
}

final class SimpleInstrInfo_movabs extends InstrInfo {
	private final FormatterString mnemonic;
	private final FormatterString mnemonic_suffix;
	private final FormatterString mnemonic64;
	private final FormatterString mnemonic_suffix64;

	SimpleInstrInfo_movabs(String mnemonic, String mnemonic_suffix, String mnemonic64, String mnemonic_suffix64) {
		this.mnemonic = new FormatterString(mnemonic);
		this.mnemonic_suffix = new FormatterString(mnemonic_suffix);
		this.mnemonic64 = new FormatterString(mnemonic64);
		this.mnemonic_suffix64 = new FormatterString(mnemonic_suffix64);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = InstrOpInfoFlags.NONE;
		int instrBitness = getBitness(instruction.getCodeSize());
		int memSize;
		FormatterString mnemonic, mnemonic_suffix;
		switch (instruction.getMemoryDisplSize()) {
		case 2:
			mnemonic = this.mnemonic;
			mnemonic_suffix = this.mnemonic_suffix;
			memSize = 16;
			break;
		case 4:
			mnemonic = this.mnemonic;
			mnemonic_suffix = this.mnemonic_suffix;
			memSize = 32;
			break;
		default:
			mnemonic = mnemonic64;
			mnemonic_suffix = mnemonic_suffix64;
			memSize = 64;
			break;
		}
		if (instrBitness == 0)
			instrBitness = memSize;
		if (instrBitness == 64) {
			if (memSize == 32)
				flags |= InstrOpInfoFlags.ADDR_SIZE32;
		}
		else if (instrBitness != memSize) {
			assert memSize == 16 || memSize == 32 : memSize;
			if (memSize == 16)
				flags |= InstrOpInfoFlags.ADDR_SIZE16;
			else
				flags |= InstrOpInfoFlags.ADDR_SIZE32;
		}
		return new InstrOpInfo(getMnemonic(options, instruction, mnemonic, mnemonic_suffix, flags), instruction, flags);
	}
}

final class SimpleInstrInfo_er extends InstrInfo {
	private final int erIndex;
	private final FormatterString mnemonic;
	private final FormatterString mnemonic_suffix;
	private final int flags;

	SimpleInstrInfo_er(int erIndex, String mnemonic) {
		this(erIndex, mnemonic, mnemonic, InstrOpInfoFlags.NONE);
	}

	SimpleInstrInfo_er(int erIndex, String mnemonic, String mnemonic_suffix, int flags) {
		this.erIndex = erIndex;
		this.mnemonic = new FormatterString(mnemonic);
		this.mnemonic_suffix = new FormatterString(mnemonic_suffix);
		this.flags = flags;
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo(getMnemonic(options, instruction, mnemonic, mnemonic_suffix, flags), instruction, flags);
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
		assert info.opCount <= 4 : info.opCount;

		switch (index) {
		case 0:
			info.op4Kind = info.op3Kind;
			info.op4Register = info.op3Register;
			info.op3Kind = info.op2Kind;
			info.op3Register = info.op2Register;
			info.op2Kind = info.op1Kind;
			info.op2Register = info.op1Register;
			info.op1Kind = info.op0Kind;
			info.op1Register = info.op0Register;
			info.op0Kind = (byte)newOpKind;
			info.op4Index = info.op3Index;
			info.op3Index = info.op2Index;
			info.op2Index = info.op1Index;
			info.op1Index = info.op0Index;
			info.op0Index = OP_ACCESS_NONE;
			info.opCount++;
			break;

		case 1:
			info.op4Kind = info.op3Kind;
			info.op4Register = info.op3Register;
			info.op3Kind = info.op2Kind;
			info.op3Register = info.op2Register;
			info.op2Kind = info.op1Kind;
			info.op2Register = info.op1Register;
			info.op1Kind = (byte)newOpKind;
			info.op4Index = info.op3Index;
			info.op3Index = info.op2Index;
			info.op2Index = info.op1Index;
			info.op1Index = OP_ACCESS_NONE;
			info.opCount++;
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

final class SimpleInstrInfo_far extends InstrInfo {
	private final int bitness;
	private final FormatterString mnemonic;
	private final FormatterString mnemonic_suffix;

	SimpleInstrInfo_far(int bitness, String mnemonic, String mnemonic_suffix) {
		this.bitness = bitness;
		this.mnemonic = new FormatterString(mnemonic);
		this.mnemonic_suffix = new FormatterString(mnemonic_suffix);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = InstrOpInfoFlags.INDIRECT_OPERAND;
		int instrBitness = getBitness(instruction.getCodeSize());
		FormatterString mnemonic;
		if (instrBitness == 0)
			instrBitness = bitness;
		if (bitness == 64) {
			flags |= InstrOpInfoFlags.OP_SIZE64;
			assert this.mnemonic.get(false) == mnemonic_suffix.get(false);
			mnemonic = this.mnemonic;
		}
		else {
			if (bitness != instrBitness || options.getGasShowMnemonicSizeSuffix())
				mnemonic = mnemonic_suffix;
			else
				mnemonic = this.mnemonic;
		}
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_bnd extends InstrInfo {
	private final FormatterString mnemonic;
	private final FormatterString mnemonic_suffix;
	private final int flags;

	SimpleInstrInfo_bnd(String mnemonic, String mnemonic_suffix, int flags) {
		this.mnemonic = new FormatterString(mnemonic);
		this.mnemonic_suffix = new FormatterString(mnemonic_suffix);
		this.flags = flags;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = this.flags;
		if (instruction.getRepnePrefix())
			flags |= InstrOpInfoFlags.BND_PREFIX;
		return new InstrOpInfo(getMnemonic(options, instruction, mnemonic, mnemonic_suffix, flags), instruction, flags);
	}
}

final class SimpleInstrInfo_pops extends InstrInfo {
	private final FormatterString mnemonic;
	private final FormatterString[] pseudo_ops;
	private final boolean canUseSae;

	SimpleInstrInfo_pops(String mnemonic, FormatterString[] pseudo_ops, boolean canUseSae) {
		this.mnemonic = new FormatterString(mnemonic);
		this.pseudo_ops = pseudo_ops;
		this.canUseSae = canUseSae;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo(mnemonic, instruction, InstrOpInfoFlags.NONE);
		if (canUseSae && instruction.getSuppressAllExceptions())
			SimpleInstrInfo_er.moveOperands(info, 1, InstrOpKind.SAE);
		int imm = instruction.getImmediate8() & 0xFF;
		if (options.getUsePseudoOps() && Integer.compareUnsigned(imm, pseudo_ops.length) < 0) {
			removeFirstImm8Operand(info);
			info.mnemonic = pseudo_ops[imm];
		}
		return info;
	}

	static void removeFirstImm8Operand(InstrOpInfo info) {
		assert info.op0Kind == InstrOpKind.IMMEDIATE8 : info.op0Kind;
		info.opCount--;
		switch (info.opCount) {
		case 0:
			info.op0Index = OP_ACCESS_INVALID;
			break;

		case 1:
			info.op0Kind = info.op1Kind;
			info.op0Register = info.op1Register;
			info.op0Index = info.op1Index;
			info.op1Index = OP_ACCESS_INVALID;
			break;

		case 2:
			info.op0Kind = info.op1Kind;
			info.op0Register = info.op1Register;
			info.op1Kind = info.op2Kind;
			info.op1Register = info.op2Register;
			info.op0Index = info.op1Index;
			info.op1Index = info.op2Index;
			info.op2Index = OP_ACCESS_INVALID;
			break;

		case 3:
			info.op0Kind = info.op1Kind;
			info.op0Register = info.op1Register;
			info.op1Kind = info.op2Kind;
			info.op1Register = info.op2Register;
			info.op2Kind = info.op3Kind;
			info.op2Register = info.op3Register;
			info.op0Index = info.op1Index;
			info.op1Index = info.op2Index;
			info.op2Index = info.op3Index;
			info.op3Index = OP_ACCESS_INVALID;
			break;

		case 4:
			info.op0Kind = info.op1Kind;
			info.op0Register = info.op1Register;
			info.op1Kind = info.op2Kind;
			info.op1Register = info.op2Register;
			info.op2Kind = info.op3Kind;
			info.op2Register = info.op3Register;
			info.op3Kind = info.op4Kind;
			info.op3Register = info.op4Register;
			info.op0Index = info.op1Index;
			info.op1Index = info.op2Index;
			info.op2Index = info.op3Index;
			info.op3Index = info.op4Index;
			info.op4Index = OP_ACCESS_INVALID;
			break;

		default:
			throw new UnsupportedOperationException();
		}
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
			int imm = instruction.getImmediate8() & 0xFF;
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
				SimpleInstrInfo_pops.removeFirstImm8Operand(info);
				info.mnemonic = pseudo_ops[index];
			}
		}
		return info;
	}
}

final class SimpleInstrInfo_imul extends InstrInfo {
	private final FormatterString mnemonic;
	private final FormatterString mnemonic_suffix;

	SimpleInstrInfo_imul(String mnemonic, String mnemonic_suffix) {
		this.mnemonic = new FormatterString(mnemonic);
		this.mnemonic_suffix = new FormatterString(mnemonic_suffix);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		final int flags = InstrOpInfoFlags.NONE;
		InstrOpInfo info = new InstrOpInfo(getMnemonic(options, instruction, mnemonic, mnemonic_suffix, flags), instruction, flags);
		assert info.opCount == 3 : info.opCount;
		if (options.getUsePseudoOps() && info.op1Kind == InstrOpKind.REGISTER && info.op2Kind == InstrOpKind.REGISTER
				&& info.op1Register == info.op2Register) {
			info.opCount--;
			info.op1Index = OP_ACCESS_READ_WRITE;
			info.op2Index = OP_ACCESS_INVALID;
		}
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
		if (Register.RAX <= (info.op0Register & REG_MASK) && (info.op0Register & REG_MASK) <= Register.R15)
			info.op0Register = (byte)((info.op0Register & REG_MASK) - Register.RAX + Register.EAX);
		if (Register.RAX <= (info.op1Register & REG_MASK) && (info.op1Register & REG_MASK) <= Register.R15)
			info.op1Register = (byte)((info.op1Register & REG_MASK) - Register.RAX + Register.EAX);
		if (Register.RAX <= (info.op2Register & REG_MASK) && (info.op2Register & REG_MASK) <= Register.R15)
			info.op2Register = (byte)((info.op2Register & REG_MASK) - Register.RAX + Register.EAX);
		return info;
	}
}

final class SimpleInstrInfo_DeclareData extends InstrInfo {
	private final FormatterString mnemonic;
	private final int opKind;

	SimpleInstrInfo_DeclareData(int code, String mnemonic) {
		this.mnemonic = new FormatterString(mnemonic);
		int opKind;
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
		this.opKind = opKind;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo(mnemonic, instruction, InstrOpInfoFlags.KEEP_OPERAND_ORDER | InstrOpInfoFlags.MNEMONIC_IS_DIRECTIVE);
		info.opCount = (byte)instruction.getDeclareDataCount();
		info.op0Kind = (byte)opKind;
		info.op1Kind = (byte)opKind;
		info.op2Kind = (byte)opKind;
		info.op3Kind = (byte)opKind;
		info.op4Kind = (byte)opKind;
		info.op0Index = OP_ACCESS_READ;
		info.op1Index = OP_ACCESS_READ;
		info.op2Index = OP_ACCESS_READ;
		info.op3Index = OP_ACCESS_READ;
		info.op4Index = OP_ACCESS_READ;
		return info;
	}
}
