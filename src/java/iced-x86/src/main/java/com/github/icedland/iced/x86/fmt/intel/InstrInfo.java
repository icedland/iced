// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt.intel;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeSize;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.fmt.FormatterOptions;
import com.github.icedland.iced.x86.info.OpAccess;
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
	private byte Op3Register;
	private byte Op4Register;
	byte Op0Index;
	byte Op1Index;
	byte Op2Index;
	byte Op3Index;
	byte Op4Index;

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
		Flags = flags;
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

final class SimpleInstrInfo_memsize extends InstrInfo {
	private final int bitness;
	private final FormatterString mnemonic;

	SimpleInstrInfo_memsize(int bitness, String mnemonic) {
		this.bitness = bitness;
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int instrBitness = getBitness(instruction.getCodeSize());
		int flags = instrBitness == 0 || (instrBitness & bitness) != 0 ? InstrOpInfoFlags.MEM_SIZE_NOTHING
				: InstrOpInfoFlags.SHOW_NO_MEM_SIZE_FORCE_SIZE | InstrOpInfoFlags.SHOW_MIN_MEM_SIZE_FORCE_SIZE;
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_StringIg1 extends InstrInfo {
	private final FormatterString mnemonic;

	SimpleInstrInfo_StringIg1(String mnemonic) {
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo();
		info.Mnemonic = mnemonic;
		info.OpCount = 1;
		info.Op0Kind = (byte)instruction.getOp0Kind();
		return info;
	}
}

final class SimpleInstrInfo_StringIg0 extends InstrInfo {
	private final FormatterString mnemonic;

	SimpleInstrInfo_StringIg0(String mnemonic) {
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo();
		info.Mnemonic = mnemonic;
		info.OpCount = 1;
		info.Op0Kind = (byte)instruction.getOp1Kind();
		info.Op0Index = info.Op1Index;
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
			info.Op0Kind = InstrOpKind.REGISTER;
			info.Op1Kind = InstrOpKind.REGISTER;
			info.Op0Register = (byte)register;
			info.Op1Register = (byte)register;
			info.Op0Index = OP_ACCESS_NONE;
			info.Op1Index = OP_ACCESS_NONE;
			return info;
		}
	}
}

final class SimpleInstrInfo_ST1 extends InstrInfo {
	private final FormatterString mnemonic;
	private final int flags;
	private final int op0Access;

	SimpleInstrInfo_ST1(String mnemonic, int flags) {
		this(mnemonic, flags, false);
	}

	SimpleInstrInfo_ST1(String mnemonic, int flags, boolean isLoad) {
		this.mnemonic = new FormatterString(mnemonic);
		this.flags = flags;
		op0Access = isLoad ? OP_ACCESS_WRITE : OP_ACCESS_READ_WRITE;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo(mnemonic, instruction, flags);
		assert instruction.getOpCount() == 1 : instruction.getOpCount();
		info.OpCount = 2;
		info.Op1Kind = info.Op0Kind;
		info.Op1Register = info.Op0Register;
		info.Op0Kind = InstrOpKind.REGISTER;
		info.Op0Register = (byte)Registers.REGISTER_ST;
		info.Op1Index = info.Op0Index;
		info.Op0Index = (byte)op0Access;
		return info;
	}
}

final class SimpleInstrInfo_ST2 extends InstrInfo {
	private final FormatterString mnemonic;
	private final int flags;

	SimpleInstrInfo_ST2(String mnemonic, int flags) {
		this.mnemonic = new FormatterString(mnemonic);
		this.flags = flags;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo(mnemonic, instruction, flags);
		assert instruction.getOpCount() == 1 : instruction.getOpCount();
		info.OpCount = 2;
		info.Op1Kind = InstrOpKind.REGISTER;
		info.Op1Register = (byte)Registers.REGISTER_ST;
		info.Op1Index = OP_ACCESS_READ;
		return info;
	}
}

final class SimpleInstrInfo_maskmovq extends InstrInfo {
	private final FormatterString mnemonic;

	SimpleInstrInfo_maskmovq(String mnemonic) {
		this.mnemonic = new FormatterString(mnemonic);
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		assert instruction.getOpCount() == 3 : instruction.getOpCount();

		int opKind = instruction.getOp0Kind();
		int shortFormOpKind;
		switch (instruction.getCodeSize()) {
		case CodeSize.UNKNOWN:
			shortFormOpKind = opKind;
			break;
		case CodeSize.CODE16:
			shortFormOpKind = OpKind.MEMORY_SEG_DI;
			break;
		case CodeSize.CODE32:
			shortFormOpKind = OpKind.MEMORY_SEG_EDI;
			break;
		case CodeSize.CODE64:
			shortFormOpKind = OpKind.MEMORY_SEG_RDI;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		int flags = InstrOpInfoFlags.IGNORE_SEGMENT_PREFIX;
		if (opKind != shortFormOpKind) {
			if (opKind == OpKind.MEMORY_SEG_DI)
				flags |= InstrOpInfoFlags.ADDR_SIZE16;
			else if (opKind == OpKind.MEMORY_SEG_EDI)
				flags |= InstrOpInfoFlags.ADDR_SIZE32;
			else if (opKind == OpKind.MEMORY_SEG_RDI)
				flags |= InstrOpInfoFlags.ADDR_SIZE64;
		}
		InstrOpInfo info = new InstrOpInfo();
		info.Flags = flags;
		info.Mnemonic = mnemonic;
		info.OpCount = 2;
		info.Op0Kind = (byte)instruction.getOp1Kind();
		info.Op0Index = 1;
		info.Op0Register = (byte)instruction.getOp1Register();
		info.Op1Kind = (byte)instruction.getOp2Kind();
		info.Op1Index = 2;
		info.Op1Register = (byte)instruction.getOp2Register();
		int segReg = instruction.getSegmentPrefix();
		if (segReg != Register.NONE
				&& com.github.icedland.iced.x86.internal.fmt.FormatterUtils.showSegmentPrefix(Register.DS, instruction, options)) {
			info.OpCount = 3;
			info.Op2Kind = InstrOpKind.REGISTER;
			info.Op2Register = (byte)segReg;
			info.Op2Index = OP_ACCESS_READ;
		}
		return info;
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
			else if (bitness == 32) {
				if (instrBitness != 64)
					flags |= InstrOpInfoFlags.OP_SIZE32;
			}
			else
				flags |= InstrOpInfoFlags.OP_SIZE64;
		}
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_os_bnd extends InstrInfo {
	private final int bitness;
	private final FormatterString mnemonic;

	SimpleInstrInfo_os_bnd(int bitness, String mnemonic) {
		this.bitness = bitness;
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = InstrOpInfoFlags.NONE;
		if (instruction.getRepnePrefix())
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
		return new InstrOpInfo(mnemonic, instruction, flags);
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
			flags |= InstrOpInfoFlags.IGNORE_SEGMENT_PREFIX | InstrOpInfoFlags.JCC_NOT_TAKEN;
		else if (prefixSeg == Register.DS)
			flags |= InstrOpInfoFlags.IGNORE_SEGMENT_PREFIX | InstrOpInfoFlags.JCC_TAKEN;
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
		if (instrBitness != 0 && instrBitness != bitness) {
			if (bitness == 16)
				flags |= InstrOpInfoFlags.OP_SIZE16;
			else if (bitness == 32)
				flags |= InstrOpInfoFlags.OP_SIZE32;
			else
				flags |= InstrOpInfoFlags.OP_SIZE64;
		}
		if (expectedReg != register) {
			if (register == Register.CX)
				flags |= InstrOpInfoFlags.ADDR_SIZE16;
			else if (register == Register.ECX)
				flags |= InstrOpInfoFlags.ADDR_SIZE32;
			else
				flags |= InstrOpInfoFlags.ADDR_SIZE64;
		}
		FormatterString mnemonic = ccIndex == -1 ? mnemonics[0]
				: com.github.icedland.iced.x86.internal.fmt.MnemonicCC.getMnemonicCC(options, ccIndex, mnemonics);
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
		if (instrBitness != memSize) {
			if (memSize == 16)
				flags |= InstrOpInfoFlags.ADDR_SIZE16;
			else if (memSize == 32)
				flags |= InstrOpInfoFlags.ADDR_SIZE32;
			else
				flags |= InstrOpInfoFlags.ADDR_SIZE64;
		}
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_opmask_op extends InstrInfo {
	private final FormatterString mnemonic;

	SimpleInstrInfo_opmask_op(String mnemonic) {
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		assert instruction.getOpCount() <= 2 : instruction.getOpCount();
		InstrOpInfo info = new InstrOpInfo(mnemonic, instruction, InstrOpInfoFlags.NONE);
		int kreg = instruction.getOpMask();
		if (kreg != Register.NONE) {
			info.OpCount++;
			info.Op2Kind = info.Op1Kind;
			info.Op2Register = info.Op1Register;
			info.Op2Index = 1;
			info.Op1Kind = InstrOpKind.REGISTER;
			info.Op1Register = (byte)kreg;
			info.Op1Index = OP_ACCESS_READ;
			info.Flags |= InstrOpInfoFlags.IGNORE_OP_MASK;
		}
		return info;
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

final class SimpleInstrInfo_ST_STi extends InstrInfo {
	private final FormatterString mnemonic;
	private final boolean pseudoOp;

	SimpleInstrInfo_ST_STi(String mnemonic, boolean pseudoOp) {
		this.mnemonic = new FormatterString(mnemonic);
		this.pseudoOp = pseudoOp;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		final int flags = 0;
		InstrOpInfo info;
		if (pseudoOp && options.getUsePseudoOps() && (instruction.getOp0Register() == Register.ST1 || instruction.getOp1Register() == Register.ST1)) {
			info = new InstrOpInfo();
			info.Mnemonic = mnemonic;
		}
		else {
			info = new InstrOpInfo(mnemonic, instruction, flags);
			assert info.getOpRegister(0) == Register.ST0 : info.getOpRegister(0);
			info.Op0Register = (byte)Registers.REGISTER_ST;
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
			info.Mnemonic = mnemonic;
		}
		else {
			info = new InstrOpInfo(mnemonic, instruction, flags);
			assert info.getOpRegister(1) == Register.ST0 : info.getOpRegister(1);
			info.Op1Register = (byte)Registers.REGISTER_ST;
		}
		return info;
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
		int imm = instruction.getImmediate8();
		if (options.getUsePseudoOps() && Integer.compareUnsigned(imm, pseudo_ops.length) < 0) {
			info.Mnemonic = pseudo_ops[imm];
			removeLastOp(info);
		}
		return info;
	}

	static void removeLastOp(InstrOpInfo info) {
		switch (info.OpCount) {
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

final class SimpleInstrInfo_imul extends InstrInfo {
	private final FormatterString mnemonic;

	SimpleInstrInfo_imul(String mnemonic) {
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo(mnemonic, instruction, InstrOpInfoFlags.NONE);
		assert info.OpCount == 3 : info.OpCount;
		if (options.getUsePseudoOps() && info.Op0Kind == InstrOpKind.REGISTER && info.Op1Kind == InstrOpKind.REGISTER
				&& info.Op0Register == info.Op1Register) {
			info.OpCount--;
			info.Op0Index = OP_ACCESS_READ_WRITE;
			info.Op1Kind = info.Op2Kind;
			info.Op1Index = 2;
			info.Op2Index = OP_ACCESS_INVALID;
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
		if (Register.EAX <= (info.Op0Register & REG_MASK) && (info.Op0Register & REG_MASK) <= Register.R15)
			info.Op0Register = (byte)((((info.Op0Register & REG_MASK) - Register.EAX) & 0xF) + Register.AX);
		if (Register.EAX <= (info.Op1Register & REG_MASK) && (info.Op1Register & REG_MASK) <= Register.R15)
			info.Op1Register = (byte)((((info.Op1Register & REG_MASK) - Register.EAX) & 0xF) + Register.AX);
		if (Register.EAX <= (info.Op2Register & REG_MASK) && (info.Op2Register & REG_MASK) <= Register.R15)
			info.Op2Register = (byte)((((info.Op2Register & REG_MASK) - Register.EAX) & 0xF) + Register.AX);
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

final class SimpleInstrInfo_reg extends InstrInfo {
	private final FormatterString mnemonic;
	private final int register;

	SimpleInstrInfo_reg(String mnemonic, int register) {
		this.mnemonic = new FormatterString(mnemonic);
		this.register = register;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo(mnemonic, instruction, InstrOpInfoFlags.NONE);
		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		info.OpCount = 1;
		info.Op0Kind = InstrOpKind.REGISTER;
		info.Op0Register = (byte)register;
		if (instruction.getCode() == Code.SKINIT)
			info.Op0Index = OP_ACCESS_READ_WRITE;
		else
			info.Op0Index = OP_ACCESS_READ;
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
