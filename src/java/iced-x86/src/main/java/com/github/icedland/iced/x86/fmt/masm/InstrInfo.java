// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt.masm;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.CodeSize;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.OpKind;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.fmt.FormatterOptions;
import com.github.icedland.iced.x86.info.OpAccess;
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
	private byte op3Register;
	private byte op4Register;
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
		int opCount = instruction.getOpCount();
		this.opCount = (byte)opCount;
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
			op0Index = 0;
			op1Index = 1;
			op2Index = InstrInfo.OP_ACCESS_INVALID;
			op3Index = InstrInfo.OP_ACCESS_INVALID;
			op4Index = InstrInfo.OP_ACCESS_INVALID;
			break;

		case 3:
			op0Index = 0;
			op1Index = 1;
			op2Index = 2;
			op3Index = InstrInfo.OP_ACCESS_INVALID;
			op4Index = InstrInfo.OP_ACCESS_INVALID;
			break;

		case 4:
			op0Index = 0;
			op1Index = 1;
			op2Index = 2;
			op3Index = 3;
			op4Index = InstrInfo.OP_ACCESS_INVALID;
			break;

		case 5:
			op0Index = 0;
			op1Index = 1;
			op2Index = 2;
			op3Index = 3;
			op4Index = 4;
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
	private final int flags;

	SimpleInstrInfo_cc(int ccIndex, String[] mnemonics) {
		this(ccIndex, mnemonics, InstrOpInfoFlags.NONE);
	}

	SimpleInstrInfo_cc(int ccIndex, String[] mnemonics, int flags) {
		this.ccIndex = ccIndex;
		this.mnemonics = FormatterString.create(mnemonics);
		this.flags = flags;
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
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
				: InstrOpInfoFlags.MEM_SIZE_NORMAL | InstrOpInfoFlags.SHOW_NO_MEM_SIZE_FORCE_SIZE | InstrOpInfoFlags.SHOW_MIN_MEM_SIZE_FORCE_SIZE;
		return new InstrOpInfo(mnemonic, instruction, flags);
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

final class SimpleInstrInfo_Int3 extends InstrInfo {
	private final FormatterString mnemonic;

	SimpleInstrInfo_Int3(String mnemonic) {
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo();
		info.mnemonic = mnemonic;
		info.opCount = 1;
		info.op0Kind = InstrOpKind.EXTRA_IMMEDIATE8_VALUE3;
		info.op0Index = OP_ACCESS_READ;
		return info;
	}
}

final class SimpleInstrInfo_YD extends InstrInfo {
	private final FormatterString mnemonic_args;
	private final FormatterString mnemonic_no_args;
	private final int flags = InstrOpInfoFlags.SHOW_NO_MEM_SIZE_FORCE_SIZE | InstrOpInfoFlags.SHOW_MIN_MEM_SIZE_FORCE_SIZE;

	SimpleInstrInfo_YD(String mnemonic_args, String mnemonic_no_args) {
		this.mnemonic_args = new FormatterString(mnemonic_args);
		this.mnemonic_no_args = new FormatterString(mnemonic_no_args);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int shortFormOpKind;
		switch (instruction.getCodeSize()) {
		case CodeSize.UNKNOWN:
			shortFormOpKind = instruction.getOp0Kind();
			break;
		case CodeSize.CODE16:
			shortFormOpKind = OpKind.MEMORY_ESDI;
			break;
		case CodeSize.CODE32:
			shortFormOpKind = OpKind.MEMORY_ESEDI;
			break;
		case CodeSize.CODE64:
			shortFormOpKind = OpKind.MEMORY_ESRDI;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		boolean shortForm = instruction.getOp0Kind() == shortFormOpKind;
		if (!shortForm)
			return new InstrOpInfo(mnemonic_args, instruction, flags);
		else {
			InstrOpInfo info = new InstrOpInfo();
			info.flags = flags;
			info.mnemonic = mnemonic_no_args;
			return info;
		}
	}
}

final class SimpleInstrInfo_DX extends InstrInfo {
	private final FormatterString mnemonic_args;
	private final FormatterString mnemonic_no_args;
	private final int flags = InstrOpInfoFlags.SHOW_NO_MEM_SIZE_FORCE_SIZE | InstrOpInfoFlags.SHOW_MIN_MEM_SIZE_FORCE_SIZE;

	SimpleInstrInfo_DX(String mnemonic_args, String mnemonic_no_args) {
		this.mnemonic_args = new FormatterString(mnemonic_args);
		this.mnemonic_no_args = new FormatterString(mnemonic_no_args);
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int shortFormOpKind;
		switch (instruction.getCodeSize()) {
		case CodeSize.UNKNOWN:
			shortFormOpKind = instruction.getOp1Kind();
			break;
		case CodeSize.CODE16:
			shortFormOpKind = OpKind.MEMORY_SEG_SI;
			break;
		case CodeSize.CODE32:
			shortFormOpKind = OpKind.MEMORY_SEG_ESI;
			break;
		case CodeSize.CODE64:
			shortFormOpKind = OpKind.MEMORY_SEG_RSI;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		boolean shortForm = instruction.getOp1Kind() == shortFormOpKind &&
				(instruction.getSegmentPrefix() == Register.NONE
						|| !com.github.icedland.iced.x86.internal.fmt.FormatterUtils.showSegmentPrefix(Register.DS, instruction, options));
		if (!shortForm)
			return new InstrOpInfo(mnemonic_args, instruction, flags);
		else {
			InstrOpInfo info = new InstrOpInfo();
			info.flags = flags;
			info.mnemonic = mnemonic_no_args;
			return info;
		}
	}
}

final class SimpleInstrInfo_YX extends InstrInfo {
	private final FormatterString mnemonic_args;
	private final FormatterString mnemonic_no_args;
	private final int flags = InstrOpInfoFlags.SHOW_NO_MEM_SIZE_FORCE_SIZE | InstrOpInfoFlags.SHOW_MIN_MEM_SIZE_FORCE_SIZE;

	SimpleInstrInfo_YX(String mnemonic_args, String mnemonic_no_args) {
		this.mnemonic_args = new FormatterString(mnemonic_args);
		this.mnemonic_no_args = new FormatterString(mnemonic_no_args);
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int shortFormOpKind;
		switch (instruction.getCodeSize()) {
		case CodeSize.UNKNOWN:
			shortFormOpKind = instruction.getOp0Kind();
			break;
		case CodeSize.CODE16:
			shortFormOpKind = OpKind.MEMORY_ESDI;
			break;
		case CodeSize.CODE32:
			shortFormOpKind = OpKind.MEMORY_ESEDI;
			break;
		case CodeSize.CODE64:
			shortFormOpKind = OpKind.MEMORY_ESRDI;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		boolean shortForm = instruction.getOp0Kind() == shortFormOpKind &&
				(instruction.getSegmentPrefix() == Register.NONE
						|| !com.github.icedland.iced.x86.internal.fmt.FormatterUtils.showSegmentPrefix(Register.DS, instruction, options));
		if (!shortForm)
			return new InstrOpInfo(mnemonic_args, instruction, flags);
		else {
			InstrOpInfo info = new InstrOpInfo();
			info.flags = flags;
			info.mnemonic = mnemonic_no_args;
			return info;
		}
	}
}

final class SimpleInstrInfo_XY extends InstrInfo {
	private final FormatterString mnemonic_args;
	private final FormatterString mnemonic_no_args;
	private final int flags = InstrOpInfoFlags.SHOW_NO_MEM_SIZE_FORCE_SIZE | InstrOpInfoFlags.SHOW_MIN_MEM_SIZE_FORCE_SIZE;

	SimpleInstrInfo_XY(String mnemonic_args, String mnemonic_no_args) {
		this.mnemonic_args = new FormatterString(mnemonic_args);
		this.mnemonic_no_args = new FormatterString(mnemonic_no_args);
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int shortFormOpKind;
		switch (instruction.getCodeSize()) {
		case CodeSize.UNKNOWN:
			shortFormOpKind = instruction.getOp1Kind();
			break;
		case CodeSize.CODE16:
			shortFormOpKind = OpKind.MEMORY_ESDI;
			break;
		case CodeSize.CODE32:
			shortFormOpKind = OpKind.MEMORY_ESEDI;
			break;
		case CodeSize.CODE64:
			shortFormOpKind = OpKind.MEMORY_ESRDI;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		boolean shortForm = instruction.getOp1Kind() == shortFormOpKind &&
				(instruction.getSegmentPrefix() == Register.NONE
						|| !com.github.icedland.iced.x86.internal.fmt.FormatterUtils.showSegmentPrefix(Register.DS, instruction, options));
		if (!shortForm)
			return new InstrOpInfo(mnemonic_args, instruction, flags);
		else {
			InstrOpInfo info = new InstrOpInfo();
			info.flags = flags;
			info.mnemonic = mnemonic_no_args;
			return info;
		}
	}
}

final class SimpleInstrInfo_YA extends InstrInfo {
	private final FormatterString mnemonic_args;
	private final FormatterString mnemonic_no_args;
	private final int flags = InstrOpInfoFlags.SHOW_NO_MEM_SIZE_FORCE_SIZE | InstrOpInfoFlags.SHOW_MIN_MEM_SIZE_FORCE_SIZE;

	SimpleInstrInfo_YA(String mnemonic_args, String mnemonic_no_args) {
		this.mnemonic_args = new FormatterString(mnemonic_args);
		this.mnemonic_no_args = new FormatterString(mnemonic_no_args);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int shortFormOpKind;
		switch (instruction.getCodeSize()) {
		case CodeSize.UNKNOWN:
			shortFormOpKind = instruction.getOp0Kind();
			break;
		case CodeSize.CODE16:
			shortFormOpKind = OpKind.MEMORY_ESDI;
			break;
		case CodeSize.CODE32:
			shortFormOpKind = OpKind.MEMORY_ESEDI;
			break;
		case CodeSize.CODE64:
			shortFormOpKind = OpKind.MEMORY_ESRDI;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		boolean shortForm = instruction.getOp0Kind() == shortFormOpKind;
		InstrOpInfo info;
		if (!shortForm) {
			info = new InstrOpInfo();
			info.flags = flags;
			info.mnemonic = mnemonic_args;
			info.opCount = 1;
			info.op0Kind = (byte)instruction.getOp0Kind();
		}
		else {
			info = new InstrOpInfo();
			info.flags = flags;
			info.mnemonic = mnemonic_no_args;
		}
		return info;
	}
}

final class SimpleInstrInfo_AX extends InstrInfo {
	private final FormatterString mnemonic_args;
	private final FormatterString mnemonic_no_args;
	private final int flags = InstrOpInfoFlags.SHOW_NO_MEM_SIZE_FORCE_SIZE | InstrOpInfoFlags.SHOW_MIN_MEM_SIZE_FORCE_SIZE;

	SimpleInstrInfo_AX(String mnemonic_args, String mnemonic_no_args) {
		this.mnemonic_args = new FormatterString(mnemonic_args);
		this.mnemonic_no_args = new FormatterString(mnemonic_no_args);
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int shortFormOpKind;
		switch (instruction.getCodeSize()) {
		case CodeSize.UNKNOWN:
			shortFormOpKind = instruction.getOp1Kind();
			break;
		case CodeSize.CODE16:
			shortFormOpKind = OpKind.MEMORY_SEG_SI;
			break;
		case CodeSize.CODE32:
			shortFormOpKind = OpKind.MEMORY_SEG_ESI;
			break;
		case CodeSize.CODE64:
			shortFormOpKind = OpKind.MEMORY_SEG_RSI;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		boolean shortForm = instruction.getOp1Kind() == shortFormOpKind &&
				(instruction.getSegmentPrefix() == Register.NONE
						|| !com.github.icedland.iced.x86.internal.fmt.FormatterUtils.showSegmentPrefix(Register.DS, instruction, options));
		InstrOpInfo info;
		if (!shortForm) {
			info = new InstrOpInfo();
			info.flags = flags;
			info.mnemonic = mnemonic_args;
			info.opCount = 1;
			info.op0Kind = (byte)instruction.getOp1Kind();
			info.op0Index = 1;
		}
		else {
			info = new InstrOpInfo();
			info.flags = flags;
			info.mnemonic = mnemonic_no_args;
		}
		return info;
	}
}

final class SimpleInstrInfo_AY extends InstrInfo {
	private final FormatterString mnemonic_args;
	private final FormatterString mnemonic_no_args;
	private final int flags = InstrOpInfoFlags.SHOW_NO_MEM_SIZE_FORCE_SIZE | InstrOpInfoFlags.SHOW_MIN_MEM_SIZE_FORCE_SIZE;

	SimpleInstrInfo_AY(String mnemonic_args, String mnemonic_no_args) {
		this.mnemonic_args = new FormatterString(mnemonic_args);
		this.mnemonic_no_args = new FormatterString(mnemonic_no_args);
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int shortFormOpKind;
		switch (instruction.getCodeSize()) {
		case CodeSize.UNKNOWN:
			shortFormOpKind = instruction.getOp1Kind();
			break;
		case CodeSize.CODE16:
			shortFormOpKind = OpKind.MEMORY_ESDI;
			break;
		case CodeSize.CODE32:
			shortFormOpKind = OpKind.MEMORY_ESEDI;
			break;
		case CodeSize.CODE64:
			shortFormOpKind = OpKind.MEMORY_ESRDI;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		boolean shortForm = instruction.getOp1Kind() == shortFormOpKind &&
				(instruction.getSegmentPrefix() == Register.NONE
						|| !com.github.icedland.iced.x86.internal.fmt.FormatterUtils.showSegmentPrefix(Register.DS, instruction, options));
		InstrOpInfo info;
		if (!shortForm) {
			info = new InstrOpInfo();
			info.flags = flags;
			info.mnemonic = mnemonic_args;
			info.opCount = 1;
			info.op0Kind = (byte)instruction.getOp1Kind();
			info.op0Index = 1;
		}
		else {
			info = new InstrOpInfo();
			info.flags = flags;
			info.mnemonic = mnemonic_no_args;
		}
		return info;
	}
}

final class SimpleInstrInfo_XLAT extends InstrInfo {
	private final FormatterString mnemonic_args;
	private final FormatterString mnemonic_no_args;

	SimpleInstrInfo_XLAT(String mnemonic_args, String mnemonic_no_args) {
		this.mnemonic_args = new FormatterString(mnemonic_args);
		this.mnemonic_no_args = new FormatterString(mnemonic_no_args);
	}

	@SuppressWarnings("deprecation")
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
		boolean shortForm = instruction.getMemoryBase() == baseReg &&
				(instruction.getSegmentPrefix() == Register.NONE
						|| !com.github.icedland.iced.x86.internal.fmt.FormatterUtils.showSegmentPrefix(Register.DS, instruction, options));
		if (!shortForm)
			return new InstrOpInfo(mnemonic_args, instruction, InstrOpInfoFlags.SHOW_NO_MEM_SIZE_FORCE_SIZE | InstrOpInfoFlags.IGNORE_INDEX_REG);
		else {
			InstrOpInfo info = new InstrOpInfo();
			info.mnemonic = mnemonic_no_args;
			return info;
		}
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
			info.mnemonic = str_xchg;
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
		InstrOpInfo info;
		if (pseudoOp && options.getUsePseudoOps() && (instruction.getOp0Register() == Register.ST1 || instruction.getOp1Register() == Register.ST1)) {
			info = new InstrOpInfo();
			info.mnemonic = mnemonic;
		}
		else {
			info = new InstrOpInfo(mnemonic, instruction, InstrOpInfoFlags.NONE);
			assert info.getOpRegister(1) == Register.ST0 : info.getOpRegister(1);
			info.op1Register = (byte)Registers.REGISTER_ST;
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
		assert info.getOpRegister(0) == Register.ST0 : info.getOpRegister(0);
		info.op0Register = (byte)Registers.REGISTER_ST;
		return info;
	}
}

final class SimpleInstrInfo_monitor extends InstrInfo {
	private final FormatterString mnemonic;
	private final int register1;
	private final int register2;
	private final int register3;

	SimpleInstrInfo_monitor(String mnemonic, int register1, int register2, int register3) {
		this.mnemonic = new FormatterString(mnemonic);
		this.register1 = register1;
		this.register2 = register2;
		this.register3 = register3;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo();
		info.mnemonic = mnemonic;
		info.opCount = 3;
		info.op0Kind = InstrOpKind.REGISTER;
		info.op0Register = (byte)register1;
		info.op1Kind = InstrOpKind.REGISTER;
		info.op1Register = (byte)register2;
		info.op2Kind = InstrOpKind.REGISTER;
		info.op2Register = (byte)register3;
		info.op0Index = OP_ACCESS_READ;
		info.op1Index = OP_ACCESS_READ;
		info.op2Index = OP_ACCESS_READ;
		if ((instruction.getCodeSize() == CodeSize.CODE64 || instruction.getCodeSize() == CodeSize.UNKNOWN)
				&& (Register.EAX <= register2 && register2 <= Register.R15D)) {
			info.op1Register += 0x10;
			info.op2Register += 0x10;
		}
		return info;
	}
}

final class SimpleInstrInfo_mwait extends InstrInfo {
	private final FormatterString mnemonic;

	SimpleInstrInfo_mwait(String mnemonic) {
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo();
		info.mnemonic = mnemonic;
		info.opCount = 2;
		info.op0Kind = InstrOpKind.REGISTER;
		info.op1Kind = InstrOpKind.REGISTER;
		info.op0Index = OP_ACCESS_READ;
		info.op1Index = OP_ACCESS_READ;

		switch (instruction.getCodeSize()) {
		case CodeSize.CODE16:
			info.op0Register = (byte)Register.AX;
			info.op1Register = (byte)Register.ECX;
			break;
		case CodeSize.CODE32:
			info.op0Register = (byte)Register.EAX;
			info.op1Register = (byte)Register.ECX;
			break;
		case CodeSize.UNKNOWN:
		case CodeSize.CODE64:
			info.op0Register = (byte)Register.RAX;
			info.op1Register = (byte)Register.RCX;
			break;
		}
		return info;
	}
}

final class SimpleInstrInfo_mwaitx extends InstrInfo {
	private final FormatterString mnemonic;

	SimpleInstrInfo_mwaitx(String mnemonic) {
		this.mnemonic = new FormatterString(mnemonic);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo();
		info.mnemonic = mnemonic;
		info.opCount = 3;
		info.op0Kind = InstrOpKind.REGISTER;
		info.op1Kind = InstrOpKind.REGISTER;
		info.op2Kind = InstrOpKind.REGISTER;
		info.op0Index = OP_ACCESS_READ;
		info.op1Index = OP_ACCESS_READ;
		info.op2Index = OP_ACCESS_COND_READ;

		switch (instruction.getCodeSize()) {
		case CodeSize.CODE16:
			info.op0Register = (byte)Register.AX;
			info.op1Register = (byte)Register.ECX;
			info.op2Register = (byte)Register.EBX;
			break;
		case CodeSize.CODE32:
			info.op0Register = (byte)Register.EAX;
			info.op1Register = (byte)Register.ECX;
			info.op2Register = (byte)Register.EBX;
			break;
		case CodeSize.UNKNOWN:
		case CodeSize.CODE64:
			info.op0Register = (byte)Register.RAX;
			info.op1Register = (byte)Register.RCX;
			info.op2Register = (byte)Register.RBX;
			break;
		}
		return info;
	}
}

final class SimpleInstrInfo_maskmovq extends InstrInfo {
	private final FormatterString mnemonic;
	private final int flags;

	SimpleInstrInfo_maskmovq(String mnemonic, int flags) {
		this.mnemonic = new FormatterString(mnemonic);
		this.flags = flags;
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		assert instruction.getOpCount() == 3 : instruction.getOpCount();
		int shortFormOpKind;
		switch (instruction.getCodeSize()) {
		case CodeSize.UNKNOWN:
			shortFormOpKind = instruction.getOp0Kind();
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
		boolean shortForm = instruction.getOp0Kind() == shortFormOpKind &&
				(instruction.getSegmentPrefix() == Register.NONE
						|| !com.github.icedland.iced.x86.internal.fmt.FormatterUtils.showSegmentPrefix(Register.DS, instruction, options));
		if (!shortForm)
			return new InstrOpInfo(mnemonic, instruction, flags);
		else {
			InstrOpInfo info = new InstrOpInfo();
			info.flags = flags;
			info.mnemonic = mnemonic;
			info.opCount = 2;
			info.op0Kind = (byte)instruction.getOp1Kind();
			info.op0Index = 1;
			info.op0Register = (byte)instruction.getOp1Register();
			info.op1Kind = (byte)instruction.getOp2Kind();
			info.op1Index = 2;
			info.op1Register = (byte)instruction.getOp2Register();
			return info;
		}
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
		info.op0Kind = (byte)instruction.getOp0Kind();
		info.op0Register = (byte)instruction.getOp0Register();
		info.op1Kind = (byte)instruction.getOp1Kind();
		info.op1Index = 1;
		info.op1Register = (byte)instruction.getOp1Register();
		info.op2Kind = InstrOpKind.REGISTER;
		info.op2Register = (byte)Register.XMM0;
		info.op2Index = OP_ACCESS_READ;
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
		info.mnemonic = mnemonic;
		assert instruction.getOpCount() == 2 : instruction.getOpCount();
		info.opCount = 2;
		info.op0Kind = (byte)instruction.getOp1Kind();
		info.op0Index = 1;
		info.op0Register = (byte)instruction.getOp1Register();
		info.op1Kind = (byte)instruction.getOp0Kind();
		info.op1Register = (byte)instruction.getOp0Register();
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

final class SimpleInstrInfo_OpSize_cc extends InstrInfo {
	private final int codeSize;
	private final int ccIndex;
	private final FormatterString[] mnemonics;
	private final FormatterString[] mnemonics_other;

	SimpleInstrInfo_OpSize_cc(int codeSize, int ccIndex, String[] mnemonics, String[] mnemonics_other) {
		this.codeSize = codeSize;
		this.ccIndex = ccIndex;
		this.mnemonics = FormatterString.create(mnemonics);
		this.mnemonics_other = FormatterString.create(mnemonics_other);
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		FormatterString[] mnemonics;
		if (instruction.getCodeSize() == codeSize)
			mnemonics = this.mnemonics;
		else
			mnemonics = mnemonics_other;
		FormatterString mnemonic = com.github.icedland.iced.x86.internal.fmt.MnemonicCC.getMnemonicCC(options, ccIndex, mnemonics);
		return new InstrOpInfo(mnemonic, instruction, InstrOpInfoFlags.NONE);
	}
}

final class SimpleInstrInfo_OpSize2 extends InstrInfo {
	private final FormatterString[] mnemonics;
	private final boolean canUseBnd;

	SimpleInstrInfo_OpSize2(String mnemonic, String mnemonic16, String mnemonic32, String mnemonic64, boolean canUseBnd) {
		mnemonics = new FormatterString[4];
		mnemonics[CodeSize.UNKNOWN] = new FormatterString(mnemonic);
		mnemonics[CodeSize.CODE16] = new FormatterString(mnemonic16);
		mnemonics[CodeSize.CODE32] = new FormatterString(mnemonic32);
		mnemonics[CodeSize.CODE64] = new FormatterString(mnemonic64);
		this.canUseBnd = canUseBnd;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		FormatterString mnemonic = mnemonics[instruction.getCodeSize()];
		int flags = InstrOpInfoFlags.NONE;
		if (canUseBnd && instruction.getRepnePrefix())
			flags |= InstrOpInfoFlags.BND_PREFIX;
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_fword extends InstrInfo {
	private final int codeSize;
	private final int flags;
	private final FormatterString mnemonic;
	private final FormatterString mnemonic2;

	SimpleInstrInfo_fword(int codeSize, int flags, String mnemonic, String mnemonic2) {
		this.codeSize = codeSize;
		this.flags = flags;
		this.mnemonic = new FormatterString(mnemonic);
		this.mnemonic2 = new FormatterString(mnemonic2);
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		FormatterString mnemonic;
		if (instruction.getCodeSize() == codeSize || instruction.getCodeSize() == CodeSize.UNKNOWN)
			mnemonic = this.mnemonic;
		else
			mnemonic = mnemonic2;
		return new InstrOpInfo(mnemonic, instruction, flags);
	}
}

final class SimpleInstrInfo_jcc extends InstrInfo {
	private final int ccIndex;
	private final FormatterString[] mnemonics;

	SimpleInstrInfo_jcc(int ccIndex, String[] mnemonics) {
		this.ccIndex = ccIndex;
		this.mnemonics = FormatterString.create(mnemonics);
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		int flags = InstrOpInfoFlags.NONE;
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
	private final int flags;

	SimpleInstrInfo_pops(String mnemonic, FormatterString[] pseudo_ops) {
		this(mnemonic, pseudo_ops, InstrOpInfoFlags.NONE);
	}

	SimpleInstrInfo_pops(String mnemonic, FormatterString[] pseudo_ops, int flags) {
		this.mnemonic = new FormatterString(mnemonic);
		this.pseudo_ops = pseudo_ops;
		this.flags = flags;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo(mnemonic, instruction, flags);
		int imm = instruction.getImmediate8() & 0xFF;
		if (options.getUsePseudoOps() && Integer.compareUnsigned(imm, pseudo_ops.length) < 0) {
			info.mnemonic = pseudo_ops[imm];
			removeLastOp(info);
		}
		return info;
	}

	static void removeLastOp(InstrOpInfo info) {
		switch (info.opCount) {
		case 4:
			info.op3Index = OP_ACCESS_INVALID;
			break;
		case 3:
			info.op2Index = OP_ACCESS_INVALID;
			break;
		default:
			throw new UnsupportedOperationException();
		}
		info.opCount--;
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
				info.mnemonic = pseudo_ops[index];
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
		assert info.opCount == 3 : info.opCount;
		if (options.getUsePseudoOps() && info.op0Kind == InstrOpKind.REGISTER && info.op1Kind == InstrOpKind.REGISTER
				&& info.op0Register == info.op1Register) {
			info.opCount--;
			info.op0Index = OP_ACCESS_READ_WRITE;
			info.op1Kind = info.op2Kind;
			info.op1Index = 2;
			info.op2Index = OP_ACCESS_INVALID;
		}
		return info;
	}
}

final class SimpleInstrInfo_Reg16 extends InstrInfo {
	private final FormatterString mnemonic;
	private final int flags;

	SimpleInstrInfo_Reg16(String mnemonic, int flags) {
		this.mnemonic = new FormatterString(mnemonic);
		this.flags = flags;
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		final int REG_MASK = com.github.icedland.iced.x86.internal.Constants.REG_MASK;
		InstrOpInfo info = new InstrOpInfo(mnemonic, instruction, flags);
		if (Register.EAX <= (info.op0Register & REG_MASK) && (info.op0Register & REG_MASK) <= Register.R15)
			info.op0Register = (byte)((((info.op0Register & REG_MASK) - Register.EAX) & 0xF) + Register.AX);
		if (Register.EAX <= (info.op1Register & REG_MASK) && (info.op1Register & REG_MASK) <= Register.R15)
			info.op1Register = (byte)((((info.op1Register & REG_MASK) - Register.EAX) & 0xF) + Register.AX);
		if (Register.EAX <= (info.op2Register & REG_MASK) && (info.op2Register & REG_MASK) <= Register.R15)
			info.op2Register = (byte)((((info.op2Register & REG_MASK) - Register.EAX) & 0xF) + Register.AX);
		return info;
	}
}

final class SimpleInstrInfo_Reg32 extends InstrInfo {
	private final FormatterString mnemonic;
	private final int flags;

	SimpleInstrInfo_Reg32(String mnemonic, int flags) {
		this.mnemonic = new FormatterString(mnemonic);
		this.flags = flags;
	}

	@SuppressWarnings("deprecation")
	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		final int REG_MASK = com.github.icedland.iced.x86.internal.Constants.REG_MASK;
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

final class SimpleInstrInfo_reg extends InstrInfo {
	private final FormatterString mnemonic;
	private final int register;

	SimpleInstrInfo_reg(String mnemonic, int register) {
		this.mnemonic = new FormatterString(mnemonic);
		this.register = register;
	}

	@Override
	InstrOpInfo getOpInfo(FormatterOptions options, Instruction instruction) {
		InstrOpInfo info = new InstrOpInfo();
		info.mnemonic = mnemonic;
		info.opCount = 1;
		info.op0Kind = InstrOpKind.REGISTER;
		info.op0Register = (byte)register;
		info.op0Index = OP_ACCESS_READ;
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
		info.mnemonic = mnemonic;
		info.opCount = 2;
		info.op0Kind = InstrOpKind.REGISTER;
		info.op1Kind = InstrOpKind.REGISTER;
		info.op1Register = (byte)Register.ECX;
		info.op0Index = OP_ACCESS_READ;
		info.op1Index = OP_ACCESS_READ;

		switch (bitness) {
		case 16:
			info.op0Register = (byte)Register.AX;
			break;

		case 32:
			info.op0Register = (byte)Register.EAX;
			break;

		case 64:
			info.op0Register = (byte)Register.RAX;
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
