// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.internal.enc.EncFlags2;
import com.github.icedland.iced.x86.internal.enc.EncFlags3;

/** DO NOT USE: INTERNAL API */
public abstract class OpCodeHandler {
	final int opCode;
	final boolean is2ByteOpCode;
	final int groupIndex;
	final int rmGroupIndex;
	final boolean isSpecialInstr;
	final int encFlags3;
	final int opSize;
	final int addrSize;
	final TryConvertToDisp8N tryConvertToDisp8N;
	/**
	 * DO NOT USE: INTERNAL API
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public final Op[] operands;

	abstract static class TryConvertToDisp8N {
		abstract Integer convert(Encoder encoder, OpCodeHandler handler, Instruction instruction, int displ);
	}

	OpCodeHandler(int encFlags2, int encFlags3, boolean isSpecialInstr, TryConvertToDisp8N tryConvertToDisp8N, Op[] operands) {
		this.encFlags3 = encFlags3;
		this.opCode = getOpCode(encFlags2);
		this.is2ByteOpCode = (encFlags2 & EncFlags2.OP_CODE_IS2_BYTES) != 0;
		this.groupIndex = (encFlags2 & EncFlags2.HAS_GROUP_INDEX) == 0 ? -1 : ((encFlags2 >>> EncFlags2.GROUP_INDEX_SHIFT) & 7);
		this.rmGroupIndex = (encFlags3 & EncFlags3.HAS_RM_GROUP_INDEX) == 0 ? -1 : ((encFlags2 >>> EncFlags2.GROUP_INDEX_SHIFT) & 7);
		this.isSpecialInstr = isSpecialInstr;
		this.opSize = (encFlags3 >>> EncFlags3.OPERAND_SIZE_SHIFT) & EncFlags3.OPERAND_SIZE_MASK;
		this.addrSize = (encFlags3 >>> EncFlags3.ADDRESS_SIZE_SHIFT) & EncFlags3.ADDRESS_SIZE_MASK;
		this.tryConvertToDisp8N = tryConvertToDisp8N;
		this.operands = operands;
	}

	static int getOpCode(int encFlags2) {
		return (encFlags2 >>> EncFlags2.OP_CODE_SHIFT) & 0xFFFF;
	}

	abstract void encode(Encoder encoder, Instruction instruction);
}
