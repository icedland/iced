// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

import java.util.ArrayList;

import com.github.icedland.iced.x86.EncodingKind;
import com.github.icedland.iced.x86.FlowControl;
import com.github.icedland.iced.x86.RflagsBits;
import com.github.icedland.iced.x86.internal.IcedConstants;

final class InstructionInfoTestCase {
	InstructionInfoTestCase() {
		int value = 5;
		if (IcedConstants.MAX_OP_COUNT != value)
			throw new UnsupportedOperationException();
	}
	int lineNo;
	String hexBytes;
	int code;
	int options;
	long ip = 0;
	int encoding = EncodingKind.LEGACY;
	int[] cpuidFeatures = new int[0];
	int rflagsRead = RflagsBits.NONE;
	int rflagsUndefined = RflagsBits.NONE;
	int rflagsWritten = RflagsBits.NONE;
	int rflagsCleared = RflagsBits.NONE;
	int rflagsSet = RflagsBits.NONE;
	int stackPointerIncrement = 0;
	boolean isPrivileged = false;
	boolean isStackInstruction = false;
	boolean isSaveRestoreInstruction = false;
	boolean isSpecial = false;
	final ArrayList<UsedRegister> usedRegisters = new ArrayList<UsedRegister>();
	final ArrayList<UsedMemory> usedMemory = new ArrayList<UsedMemory>();
	int flowControl = FlowControl.NEXT;
	int op0Access = OpAccess.NONE;
	int op1Access = OpAccess.NONE;
	int op2Access = OpAccess.NONE;
	int op3Access = OpAccess.NONE;
	int op4Access = OpAccess.NONE;
	int fpuTopIncrement = 0;
	boolean fpuConditionalTop = false;
	boolean fpuWritesTop = false;
}
