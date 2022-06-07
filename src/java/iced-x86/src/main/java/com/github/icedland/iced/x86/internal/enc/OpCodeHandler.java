// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.internal.enc;

import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.enc.Encoder;

/** DO NOT USE: INTERNAL API */
public abstract class OpCodeHandler {
	/** DO NOT USE: INTERNAL API */
	public final int opCode;
	/** DO NOT USE: INTERNAL API */
	public final boolean is2ByteOpCode;
	/** DO NOT USE: INTERNAL API */
	public final int groupIndex;
	/** DO NOT USE: INTERNAL API */
	public final int rmGroupIndex;
	/** DO NOT USE: INTERNAL API */
	public final boolean isSpecialInstr;
	/** DO NOT USE: INTERNAL API */
	public final int encFlags3;
	/** DO NOT USE: INTERNAL API */
	public final int opSize;
	/** DO NOT USE: INTERNAL API */
	public final int addrSize;
	/** DO NOT USE: INTERNAL API */
	public final TryConvertToDisp8N tryConvertToDisp8N;
	/** DO NOT USE: INTERNAL API */
	public final Op[] operands;
	/** DO NOT USE: INTERNAL API */
	public abstract static class TryConvertToDisp8N {
		public abstract boolean convert(Encoder encoder, OpCodeHandler handler, Instruction instruction, int displ, byte compressedValue);
	}
	/** DO NOT USE: INTERNAL API */
	protected OpCodeHandler(int encFlags2, int encFlags3, boolean isSpecialInstr, TryConvertToDisp8N tryConvertToDisp8N, Op[] operands) {
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

	/** DO NOT USE: INTERNAL API */
	protected static int getOpCode(int encFlags2) {
		return (encFlags2 >>> EncFlags2.OP_CODE_SHIFT) & 0xFFFF;
	}
	/** DO NOT USE: INTERNAL API */
	public abstract void encode(Encoder encoder, Instruction instruction);
}
