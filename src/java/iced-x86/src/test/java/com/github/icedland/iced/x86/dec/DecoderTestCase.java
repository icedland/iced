// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.dec;

import com.github.icedland.iced.x86.ConstantOffsets;

public final class DecoderTestCase {
	public int lineNumber;
	public int testOptions;
	public int decoderError;
	public int decoderOptions;
	public int bitness;
	public String hexBytes;
	public String encodedHexBytes;
	public long ip;
	public int code;
	public int mnemonic;
	public int opCount;
	public boolean zeroingMasking;
	public boolean suppressAllExceptions;
	public boolean isBroadcast;
	public boolean hasXacquirePrefix;
	public boolean hasXreleasePrefix;
	public boolean hasRepePrefix;
	public boolean hasRepnePrefix;
	public boolean hasLockPrefix;
	public int vsibBitness;
	public int opMask;
	public int roundingControl;
	public int op0Kind, op1Kind, op2Kind, op3Kind, op4Kind;
	public int segmentPrefix;
	public int memorySegment;
	public int memoryBase;
	public int memoryIndex;
	public int memoryDisplSize;
	public int memorySize;
	public int memoryIndexScale;
	public long memoryDisplacement;
	public long immediate;
	public byte immediate_2nd;
	public long nearBranch;
	public int farBranch;
	public short farBranchSelector;
	public int op0Register, op1Register, op2Register, op3Register, op4Register;
	public ConstantOffsets constantOffsets = new ConstantOffsets();
	public boolean mvexEvictionHint;
	public int mvexRegMemConv;

	public int getOpKind(int operand) {
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
			throw new UnsupportedOperationException();
		}
	}

	public void setOpKind(int operand, int opKind) {
		switch (operand) {
		case 0:
			op0Kind = opKind;
			break;
		case 1:
			op1Kind = opKind;
			break;
		case 2:
			op2Kind = opKind;
			break;
		case 3:
			op3Kind = opKind;
			break;
		case 4:
			op4Kind = opKind;
			break;
		default:
			throw new UnsupportedOperationException();
		}
	}

	public int getOpRegister(int operand) {
		switch (operand) {
		case 0:
			return op0Register;
		case 1:
			return op1Register;
		case 2:
			return op2Register;
		case 3:
			return op3Register;
		case 4:
			return op4Register;
		default:
			throw new UnsupportedOperationException();
		}
	}

	public void setOpRegister(int operand, int register) {
		switch (operand) {
		case 0:
			op0Register = register;
			break;
		case 1:
			op1Register = register;
			break;
		case 2:
			op2Register = register;
			break;
		case 3:
			op3Register = register;
			break;
		case 4:
			op4Register = register;
			break;
		default:
			throw new UnsupportedOperationException();
		}
	}
}
