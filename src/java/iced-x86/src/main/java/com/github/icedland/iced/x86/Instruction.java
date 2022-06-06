// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

import com.github.icedland.iced.x86.internal.IcedConstants;
import com.github.icedland.iced.x86.internal.InstrFlags1;
import com.github.icedland.iced.x86.internal.InstructionMemorySizes;
import com.github.icedland.iced.x86.internal.InstructionOpCounts;
import com.github.icedland.iced.x86.internal.MvexInstrFlags;

/**
 * A 16/32/64-bit instruction.
 *
 * Created by {@link com.github.icedland.iced.x86.dec.Decoder} or by <code>Instruction.create()</code> methods.
 */
public final class Instruction {
	// If this changes to a short, this can be removed (just return the register field without masking)
	static final int REG_MASK = 0xFF;
	long nextRip;
	long memDispl;
	int flags1; // InstrFlags1
	int immediate;
	short code;
	byte memBaseReg; // Register
	byte memIndexReg; // Register
	byte reg0, reg1, reg2, reg3; // Register
	byte opKind0, opKind1, opKind2, opKind3; // OpKind
	byte scale;
	byte displSize;
	byte len;
	byte pad;

	/**
	 * Creates a new empty instruction
	 */
	public Instruction() {
	}

	/**
	 * Creates a new copy of this instance, but unlike {@link Object#clone clone()}, this never throws.
	 */
	public Instruction copy() {
		Instruction instr = new Instruction();
		instr.nextRip = nextRip;
		instr.memDispl = memDispl;
		instr.flags1 = flags1;
		instr.immediate = immediate;
		instr.code = code;
		instr.memBaseReg = memBaseReg;
		instr.memIndexReg = memIndexReg;
		instr.reg0 = reg0;
		instr.reg1 = reg1;
		instr.reg2 = reg2;
		instr.reg3 = reg3;
		instr.opKind0 = opKind0;
		instr.opKind1 = opKind1;
		instr.opKind2 = opKind2;
		instr.opKind3 = opKind3;
		instr.scale = scale;
		instr.displSize = displSize;
		instr.len = len;
		instr.pad = pad;
		return instr;
	}

	/**
	 * Gets the hash code of this instance, ignoring some fields, eg.<!-- --> <code>IP</code>, <code>len</code> and a few unimportant fields
	 */
	@Override
	public int hashCode() {
		final int prime = 31;
		int result = 1;
		result = prime * result + ((int)memDispl ^ (int)(memDispl >>> 32));
		result = prime * result + (flags1 & ~InstrFlags1.EQUALS_IGNORE_MASK);
		result = prime * result + immediate;
		result = prime * result + code;
		result = prime * result + memBaseReg;
		result = prime * result + memIndexReg;
		result = prime * result + reg3;
		result = prime * result + reg2;
		result = prime * result + reg1;
		result = prime * result + reg0;
		result = prime * result + opKind3;
		result = prime * result + opKind2;
		result = prime * result + opKind1;
		result = prime * result + opKind0;
		result = prime * result + scale;
		result = prime * result + displSize;
		result = prime * result + pad;
		return result;
	}

	/**
	 * Checks if <code>this</code> is equal to <code>obj</code>, ignoring some fields, eg.<!-- --> <code>IP</code>, <code>len</code> and a few
	 * unimportant fields
	 *
	 * @see #equalsAllBits(Instruction)
	 *
	 * @param obj Other instruction
	 */
	@Override
	public boolean equals(Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;
		if (getClass() != obj.getClass())
			return false;
		Instruction other = (Instruction)obj;
		return memDispl == other.memDispl &&
				((flags1 ^ other.flags1) & ~InstrFlags1.EQUALS_IGNORE_MASK) == 0 &&
				immediate == other.immediate &&
				code == other.code &&
				memBaseReg == other.memBaseReg &&
				memIndexReg == other.memIndexReg &&
				reg0 == other.reg0 &&
				reg1 == other.reg1 &&
				reg2 == other.reg2 &&
				reg3 == other.reg3 &&
				opKind0 == other.opKind0 &&
				opKind1 == other.opKind1 &&
				opKind2 == other.opKind2 &&
				opKind3 == other.opKind3 &&
				scale == other.scale &&
				displSize == other.displSize &&
				pad == other.pad;
	}

	/**
	 * Checks if two instructions are equal, comparing all bits, not ignoring anything
	 *
	 * @param other Other instruction
	 */
	public boolean equalsAllBits(Instruction other) {
		if (other == null)
			return false;
		return nextRip == other.nextRip &&
				memDispl == other.memDispl &&
				flags1 == other.flags1 &&
				immediate == other.immediate &&
				code == other.code &&
				memBaseReg == other.memBaseReg &&
				memIndexReg == other.memIndexReg &&
				reg0 == other.reg0 &&
				reg1 == other.reg1 &&
				reg2 == other.reg2 &&
				reg3 == other.reg3 &&
				opKind0 == other.opKind0 &&
				opKind1 == other.opKind1 &&
				opKind2 == other.opKind2 &&
				opKind3 == other.opKind3 &&
				scale == other.scale &&
				displSize == other.displSize &&
				len == other.len &&
				pad == other.pad;
	}

	/**
	 * Formats the instruction using the default formatter with default formatter options
	 */
	@Override
	public String toString() {
		throw new UnsupportedOperationException(); // TODO:
	}

	/**
	 * 16-bit IP of the instruction
	 */
	public short getIP16() {
		return (short)((int)nextRip - getLength());
	}

	/**
	 * 16-bit IP of the instruction
	 */
	public void setIP16(short value) {
		nextRip = (value + getLength()) & 0xFFFF;
	}

	/**
	 * 32-bit IP of the instruction
	 */
	public int getIP32() {
		return (int)nextRip - getLength();
	}

	/**
	 * 32-bit IP of the instruction
	 */
	public void setIP32(int value) {
		nextRip = (value + getLength()) & 0xFFFF_FFFF;
	}

	/**
	 * 64-bit IP of the instruction
	 */
	public long getIP() {
		return nextRip - getLength();
	}

	/**
	 * 64-bit IP of the instruction
	 */
	public void setIP(long value) {
		nextRip = value + getLength();
	}

	/**
	 * 16-bit IP of the next instruction
	 */
	public short getNextIP16() {
		return (short)nextRip;
	}

	/**
	 * 16-bit IP of the next instruction
	 */
	public void setNextIP16(short value) {
		nextRip = value & 0xFFFF;
	}

	/**
	 * 32-bit IP of the next instruction
	 */
	public int getNextIP32() {
		return (int)nextRip;
	}

	/**
	 * 32-bit IP of the next instruction
	 */
	public void setNextIP32(int value) {
		nextRip = value & 0xFFFF_FFFF;
	}

	/**
	 * 64-bit IP of the next instruction
	 */
	public long getNextIP() {
		return nextRip;
	}

	/**
	 * 64-bit IP of the next instruction
	 */
	public void setNextIP(long value) {
		nextRip = value;
	}

	/**
	 * Gets the code size (a {@link CodeSize} enum variant) when the instruction was decoded.
	 *
	 * This value is informational and can be used by a formatter.
	 */
	public int getCodeSize() {
		return (flags1 >>> InstrFlags1.CODE_SIZE_SHIFT) & InstrFlags1.CODE_SIZE_MASK;
	}

	/**
	 * Gets the code size (a {@link CodeSize} enum variant) when the instruction was decoded.
	 *
	 * This value is informational and can be used by a formatter.
	 */
	public void setCodeSize(int value) {
		flags1 = ((flags1 & ~(InstrFlags1.CODE_SIZE_MASK << InstrFlags1.CODE_SIZE_SHIFT))
				| ((value & InstrFlags1.CODE_SIZE_MASK) << InstrFlags1.CODE_SIZE_SHIFT));
	}

	/**
	 * Checks if it's an invalid instruction ({@link #getCode()} == {@link Code#INVALID})
	 */
	public boolean isInvalid() {
		return code == Code.INVALID;
	}

	/**
	 * Instruction code (a {@link Code} enum variant)
	 *
	 * @see #getMnemonic()
	 */
	public int getCode() {
		return code;
	}

	/**
	 * Instruction code (a {@link Code} enum variant)
	 *
	 * @see #getMnemonic()
	 */
	public void setCode(int value) {
		if (Integer.compareUnsigned(value, IcedConstants.CODE_ENUM_COUNT) >= 0)
			throw new IllegalArgumentException();
		code = (short)value;
	}

	/**
	 * Gets the mnemonic (a {@link Mnemonic} enum variant)
	 *
	 * @see #getCode()
	 */
	public int getMnemonic() {
		return Code.mnemonic(getCode());
	}

	/**
	 * Gets the operand count. An instruction can have 0-5 operands.
	 */
	public int getOpCount() {
		return InstructionOpCounts.opCount[code] & 0xFF;
	}

	/**
	 * Gets the length of the instruction, 0-15 bytes. This is just informational. If you modify the instruction
	 * or create a new one, this property could return the wrong value.
	 */
	public int getLength() {
		return len & 0xFF;
	}

	/**
	 * Gets the length of the instruction, 0-15 bytes. This is just informational. If you modify the instruction
	 * or create a new one, this property could return the wrong value.
	 */
	public void setLength(int value) {
		len = (byte)value;
	}

	boolean isXacquireInstr() {
		if (getOp0Kind() != OpKind.MEMORY)
			return false;
		if (getLockPrefix())
			return getCode() != Code.CMPXCHG16B_M128;
		return getMnemonic() == Mnemonic.XCHG;
	}

	boolean isXreleaseInstr() {
		if (getOp0Kind() != OpKind.MEMORY)
			return false;
		if (getLockPrefix())
			return getCode() != Code.CMPXCHG16B_M128;
		switch (getCode()) {
		case Code.XCHG_RM8_R8:
		case Code.XCHG_RM16_R16:
		case Code.XCHG_RM32_R32:
		case Code.XCHG_RM64_R64:
		case Code.MOV_RM8_R8:
		case Code.MOV_RM16_R16:
		case Code.MOV_RM32_R32:
		case Code.MOV_RM64_R64:
		case Code.MOV_RM8_IMM8:
		case Code.MOV_RM16_IMM16:
		case Code.MOV_RM32_IMM32:
		case Code.MOV_RM64_IMM32:
			return true;
		default:
			return false;
		}
	}

	/**
	 * <code>true</code> if the instruction has the <code>XACQUIRE</code> prefix (<code>F2</code>)
	 */
	public boolean getXacquirePrefix() {
		return (flags1 & InstrFlags1.REPNE_PREFIX) != 0 && isXacquireInstr();
	}

	/**
	 * <code>true</code> if the instruction has the <code>XACQUIRE</code> prefix (<code>F2</code>)
	 */
	public void setXacquirePrefix(boolean value) {
		if (value)
			flags1 |= InstrFlags1.REPNE_PREFIX;
		else
			flags1 &= ~InstrFlags1.REPNE_PREFIX;
	}

	/**
	 * <code>true</code> if the instruction has the <code>XRELEASE</code> prefix (<code>F3</code>)
	 */
	public boolean getXreleasePrefix() {
		return (flags1 & InstrFlags1.REPE_PREFIX) != 0 && isXreleaseInstr();
	}

	/**
	 * <code>true</code> if the instruction has the <code>XRELEASE</code> prefix (<code>F3</code>)
	 */
	public void setXreleasePrefix(boolean value) {
		if (value)
			flags1 |= InstrFlags1.REPE_PREFIX;
		else
			flags1 &= ~InstrFlags1.REPE_PREFIX;
	}

	/**
	 * <code>true</code> if the instruction has the <code>REPE</code> or <code>REP</code> prefix (<code>F3</code>)
	 */
	public boolean getRepPrefix() {
		return (flags1 & InstrFlags1.REPE_PREFIX) != 0;
	}

	/**
	 * <code>true</code> if the instruction has the <code>REPE</code> or <code>REP</code> prefix (<code>F3</code>)
	 */
	public void setRepPrefix(boolean value) {
		if (value)
			flags1 |= InstrFlags1.REPE_PREFIX;
		else
			flags1 &= ~InstrFlags1.REPE_PREFIX;
	}

	/**
	 * <code>true</code> if the instruction has the <code>REPE</code> or <code>REP</code> prefix (<code>F3</code>)
	 */
	public boolean getRepePrefix() {
		return (flags1 & InstrFlags1.REPE_PREFIX) != 0;
	}

	/**
	 * <code>true</code> if the instruction has the <code>REPE</code> or <code>REP</code> prefix (<code>F3</code>)
	 */
	public void setRepePrefix(boolean value) {
		if (value)
			flags1 |= InstrFlags1.REPE_PREFIX;
		else
			flags1 &= ~InstrFlags1.REPE_PREFIX;
	}

	/**
	 * <code>true</code> if the instruction has the <code>REPNE</code> prefix (<code>F2</code>)
	 */
	public boolean getRepnePrefix() {
		return (flags1 & InstrFlags1.REPNE_PREFIX) != 0;
	}

	/**
	 * <code>true</code> if the instruction has the <code>REPNE</code> prefix (<code>F2</code>)
	 */
	public void setRepnePrefix(boolean value) {
		if (value)
			flags1 |= InstrFlags1.REPNE_PREFIX;
		else
			flags1 &= ~InstrFlags1.REPNE_PREFIX;
	}

	/**
	 * <code>true</code> if the instruction has the <code>LOCK</code> prefix (<code>F0</code>)
	 */
	public boolean getLockPrefix() {
		return (flags1 & InstrFlags1.LOCK_PREFIX) != 0;
	}

	/**
	 * <code>true</code> if the instruction has the <code>LOCK</code> prefix (<code>F0</code>)
	 */
	public void setLockPrefix(boolean value) {
		if (value)
			flags1 |= InstrFlags1.LOCK_PREFIX;
		else
			flags1 &= ~InstrFlags1.LOCK_PREFIX;
	}

	/**
	 * Gets operand #0's kind (an {@link OpKind} enum variant) if the operand exists
	 *
	 * @see #getOpCount()
	 * @see #getOpKind(int)
	 */
	public int getOp0Kind() {
		return opKind0;
	}

	/**
	 * Gets operand #0's kind (an {@link OpKind} enum variant) if the operand exists
	 *
	 * @see #getOpCount()
	 * @see #getOpKind(int)
	 */
	public void setOp0Kind(int value) {
		opKind0 = (byte)value;
	}

	/**
	 * Gets operand #1's kind (an {@link OpKind} enum variant) if the operand exists
	 *
	 * @see #getOpCount()
	 * @see #getOpKind(int)
	 */
	public int getOp1Kind() {
		return opKind1;
	}

	/**
	 * Gets operand #1's kind (an {@link OpKind} enum variant) if the operand exists
	 *
	 * @see #getOpCount()
	 * @see #getOpKind(int)
	 */
	public void setOp1Kind(int value) {
		opKind1 = (byte)value;
	}

	/**
	 * Gets operand #2's kind (an {@link OpKind} enum variant) if the operand exists
	 *
	 * @see #getOpCount()
	 * @see #getOpKind(int)
	 */
	public int getOp2Kind() {
		return opKind2;
	}

	/**
	 * Gets operand #2's kind (an {@link OpKind} enum variant) if the operand exists
	 *
	 * @see #getOpCount()
	 * @see #getOpKind(int)
	 */
	public void setOp2Kind(int value) {
		opKind2 = (byte)value;
	}

	/**
	 * Gets operand #3's kind (an {@link OpKind} enum variant) if the operand exists
	 *
	 * @see #getOpCount()
	 * @see #getOpKind(int)
	 */
	public int getOp3Kind() {
		return opKind3;
	}

	/**
	 * Gets operand #3's kind (an {@link OpKind} enum variant) if the operand exists
	 *
	 * @see #getOpCount()
	 * @see #getOpKind(int)
	 */
	public void setOp3Kind(int value) {
		opKind3 = (byte)value;
	}

	/**
	 * Gets operand #4's kind (an {@link OpKind} enum variant) if the operand exists
	 *
	 * @see #getOpCount()
	 * @see #getOpKind(int)
	 */
	public int getOp4Kind() {
		return OpKind.IMMEDIATE8;
	}

	/**
	 * Gets operand #4's kind (an {@link OpKind} enum variant) if the operand exists
	 *
	 * @see #getOpCount()
	 * @see #getOpKind(int)
	 */
	public void setOp4Kind(int value) {
		if (value != OpKind.IMMEDIATE8)
			throw new IllegalArgumentException();
	}

	/**
	 * Gets an operand's kind (an {@link OpKind} enum variant) if it exists
	 *
	 * @param operand Operand number, 0-4
	 *
	 * @see #getOpCount()
	 */
	public int getOpKind(int operand) {
		switch (operand) {
		case 0:
			return getOp0Kind();
		case 1:
			return getOp1Kind();
		case 2:
			return getOp2Kind();
		case 3:
			return getOp3Kind();
		case 4:
			return getOp4Kind();
		default:
			throw new IllegalArgumentException();
		}
	}

	/**
	 * Gets whether a specific operand's kind exists
	 *
	 * @param opKind Operand kind (an {@link OpKind} enum variant)
	 */
	public boolean hasOpKind(int opKind) {
		for (int i = 0; i < getOpCount(); i++) {
			if (getOpKind(i) == opKind)
				return true;
		}
		return false;
	}

	/**
	 * Sets an operand's kind
	 *
	 * @param operand Operand number, 0-4
	 * @param opKind  Operand kind (an {@link OpKind} enum variant)
	 */
	public void setOpKind(int operand, int opKind) {
		switch (operand) {
		case 0:
			setOp0Kind(opKind);
			break;
		case 1:
			setOp1Kind(opKind);
			break;
		case 2:
			setOp2Kind(opKind);
			break;
		case 3:
			setOp3Kind(opKind);
			break;
		case 4:
			setOp4Kind(opKind);
			break;
		default:
			throw new IllegalArgumentException();
		}
	}

	/**
	 * Checks if the instruction has a segment override prefix
	 *
	 * @see #getSegmentPrefix()
	 */
	public boolean hasSegmentPrefix() {
		return (((flags1 >>> InstrFlags1.SEGMENT_PREFIX_SHIFT) & InstrFlags1.SEGMENT_PREFIX_MASK) - 1) < 6;
	}

	/**
	 * Gets the segment override prefix (a {@link Register} enum variant) or {@link Register#NONE} if none.
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY},
	 * {@link OpKind#MEMORY_SEG_SI}, {@link OpKind#MEMORY_SEG_ESI}, {@link OpKind#MEMORY_SEG_RSI}
	 *
	 * @see #getMemorySegment()
	 */
	public int getSegmentPrefix() {
		int index = ((flags1 >>> InstrFlags1.SEGMENT_PREFIX_SHIFT) & InstrFlags1.SEGMENT_PREFIX_MASK) - 1;
		return index < 6 ? Register.ES + index : Register.NONE;
	}

	/**
	 * Gets the segment override prefix (a {@link Register} enum variant) or {@link Register#NONE} if none.
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY},
	 * {@link OpKind#MEMORY_SEG_SI}, {@link OpKind#MEMORY_SEG_ESI}, {@link OpKind#MEMORY_SEG_RSI}
	 *
	 * @see #getMemorySegment()
	 */
	public void setSegmentPrefix(int value) {
		int encValue;
		if (value == Register.NONE)
			encValue = 0;
		else
			encValue = (value - Register.ES + 1) & InstrFlags1.SEGMENT_PREFIX_MASK;
		flags1 = (flags1 & ~(InstrFlags1.SEGMENT_PREFIX_MASK << InstrFlags1.SEGMENT_PREFIX_SHIFT)) |
				(encValue << InstrFlags1.SEGMENT_PREFIX_SHIFT);
	}

	/**
	 * Gets the effective segment register used to reference the memory location (a {@link Register} enum variant).
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY},
	 * {@link OpKind#MEMORY_SEG_SI}, {@link OpKind#MEMORY_SEG_ESI}, {@link OpKind#MEMORY_SEG_RSI}
	 */
	public int getMemorySegment() {
		int segReg = getSegmentPrefix();
		if (segReg != Register.NONE)
			return segReg;
		int baseReg = getMemoryBase();
		if (baseReg == Register.BP || baseReg == Register.EBP || baseReg == Register.ESP || baseReg == Register.RBP || baseReg == Register.RSP)
			return Register.SS;
		return Register.DS;
	}

	/**
	 * Gets the size of the memory displacement in bytes.
	 *
	 * Valid values are <code>0</code>, <code>1</code> (16/32/64-bit), <code>2</code> (16-bit),
	 * <code>4</code> (32-bit), <code>8</code> (64-bit).
	 *
	 * Note that the return value can be 1 and {@link #getMemoryDisplacement64()} may still not fit in
	 * a signed byte if it's an EVEX/MVEX encoded instruction.
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY}
	 */
	public int getMemoryDisplSize() {
		switch (displSize) {
		case 0:
			return 0;
		case 1:
			return 1;
		case 2:
			return 2;
		case 3:
			return 4;
		default:
			return 8;
		}
	}

	/**
	 * Gets the size of the memory displacement in bytes.
	 *
	 * Valid values are <code>0</code>, <code>1</code> (16/32/64-bit), <code>2</code> (16-bit),
	 * <code>4</code> (32-bit), <code>8</code> (64-bit).
	 *
	 * Note that the return value can be 1 and {@link #getMemoryDisplacement64()} may still not fit in
	 * a signed byte if it's an EVEX/MVEX encoded instruction.
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY}
	 */
	public void setMemoryDisplSize(int value) {
		switch (value) {
		case 0:
			displSize = 0;
			break;
		case 1:
			displSize = 1;
			break;
		case 2:
			displSize = 2;
			break;
		case 4:
			displSize = 3;
			break;
		default:
			displSize = 4;
			break;
		}
	}

	/**
	 * <code>true</code> if the data is broadcast (EVEX instructions only)
	 */
	public boolean getBroadcast() {
		return (flags1 & InstrFlags1.BROADCAST) != 0;
	}

	/**
	 * <code>true</code> if the data is broadcast (EVEX instructions only)
	 */
	public void setBroadcast(boolean value) {
		if (value)
			flags1 |= InstrFlags1.BROADCAST;
		else
			flags1 &= ~InstrFlags1.BROADCAST;
	}

	/**
	 * <code>true</code> if eviction hint bit is set (<code>{eh}</code>) (MVEX instructions only)
	 */
	public boolean getMvexEvictionHint() {
		throw new UnsupportedOperationException(); // TODO:
		// TODO: return IcedConstants.isMvex(getCode()) && (immediate & MvexInstrFlags.EVICTION_HINT) != 0;
	}

	/**
	 * <code>true</code> if eviction hint bit is set (<code>{eh}</code>) (MVEX instructions only)
	 */
	public void setMvexEvictionHint(boolean value) {
		if (value)
			immediate |= MvexInstrFlags.EVICTION_HINT;
		else
			immediate &= ~MvexInstrFlags.EVICTION_HINT;
	}

	/**
	 * (MVEX) Register/memory operand conversion function (an {@link MvexRegMemConv} enum variant)
	 */
	public int getMvexRegMemConv() {
		throw new UnsupportedOperationException(); // TODO:
		// TODO:
		// if (!IcedConstants.isMvex(getCode()))
		// return MvexRegMemConv.NONE;
		// return (immediate >>> MvexInstrFlags.MVEX_REG_MEM_CONV_SHIFT) & MvexInstrFlags.MVEX_REG_MEM_CONV_MASK;
	}

	/**
	 * (MVEX) Register/memory operand conversion function (an {@link MvexRegMemConv} enum variant)
	 */
	public void setMvexRegMemConv(int value) {
		immediate = (immediate & ~(MvexInstrFlags.MVEX_REG_MEM_CONV_MASK << MvexInstrFlags.MVEX_REG_MEM_CONV_SHIFT)) |
				(value << MvexInstrFlags.MVEX_REG_MEM_CONV_SHIFT);
	}

	/**
	 * Gets the size of the memory location (a {@link MemorySize} enum variant) that is referenced by the operand.
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY},
	 * {@link OpKind#MEMORY_SEG_SI}, {@link OpKind#MEMORY_SEG_ESI}, {@link OpKind#MEMORY_SEG_RSI},
	 * {@link OpKind#MEMORY_ESDI}, {@link OpKind#MEMORY_ESEDI}, {@link OpKind#MEMORY_ESRDI}
	 *
	 * @see #getBroadcast()
	 */
	public int getMemorySize() {
		int index = getCode();
		// TODO:
		// if (IcedConstants.isMvex(index)) {
		// MvexInfo mvex = new MvexInfo(index);
		// int sss = (getMvexRegMemConv() - MvexRegMemConv.MEM_CONV_NONE) & 7;
		// return MvexMemorySizeLut.Data[mvex.getTupleTypeLutKind() * 8 + sss];
		// }
		if (getBroadcast())
			return InstructionMemorySizes.sizesBcst[index];
		else
			return InstructionMemorySizes.sizesNormal[index];
	}

	/**
	 * Gets the index register scale value, valid values are <code>*1</code>, <code>*2</code>, <code>*4</code>, <code>*8</code>.
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY}
	 */
	public int getMemoryIndexScale() {
		return 1 << scale;
	}

	/**
	 * Gets the index register scale value, valid values are <code>*1</code>, <code>*2</code>, <code>*4</code>, <code>*8</code>.
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY}
	 */
	public void setMemoryIndexScale(int value) {
		if (value == 1)
			scale = 0;
		else if (value == 2)
			scale = 1;
		else if (value == 4)
			scale = 2;
		else {
			assert value == 8 : value;
			scale = 3;
		}
	}

	/**
	 * Gets the memory operand's displacement or the 32-bit absolute address if it's
	 * an <code>EIP</code> or <code>RIP</code> relative memory operand.
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY}
	 */
	public int getMemoryDisplacement32() {
		return (int)memDispl;
	}

	/**
	 * Gets the memory operand's displacement or the 32-bit absolute address if it's
	 * an <code>EIP</code> or <code>RIP</code> relative memory operand.
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY}
	 */
	public void setMemoryDisplacement32(int value) {
		memDispl = value;
	}

	/**
	 * Gets the memory operand's displacement or the 64-bit absolute address if it's
	 * an <code>EIP</code> or <code>RIP</code> relative memory operand.
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY}
	 */
	public long getMemoryDisplacement64() {
		return memDispl;
	}

	/**
	 * Gets the memory operand's displacement or the 64-bit absolute address if it's
	 * an <code>EIP</code> or <code>RIP</code> relative memory operand.
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY}
	 */
	public void setMemoryDisplacement64(long value) {
		memDispl = value;
	}

	/**
	 * Gets an operand's immediate value
	 *
	 * @param operand Operand number, 0-4
	 */
	public long getImmediate(int operand) {
		switch (getOpKind(operand)) {
		case OpKind.IMMEDIATE8:
			return getImmediate8();
		case OpKind.IMMEDIATE8_2ND:
			return getImmediate8_2nd();
		case OpKind.IMMEDIATE16:
			return getImmediate16();
		case OpKind.IMMEDIATE32:
			return getImmediate32();
		case OpKind.IMMEDIATE64:
			return getImmediate64();
		case OpKind.IMMEDIATE8TO16:
			return (long)getImmediate8to16();
		case OpKind.IMMEDIATE8TO32:
			return (long)getImmediate8to32();
		case OpKind.IMMEDIATE8TO64:
			return (long)getImmediate8to64();
		case OpKind.IMMEDIATE32TO64:
			return (long)getImmediate32to64();
		default:
			throw new IllegalArgumentException();
		}
	}

	/**
	 * Sets an operand's immediate value
	 *
	 * @param operand   Operand number, 0-4
	 * @param immediate New immediate
	 */
	public void setImmediate(int operand, int immediate) {
		setImmediate(operand, (long)immediate);
	}

	/**
	 * Sets an operand's immediate value
	 *
	 * @param operand   Operand number, 0-4
	 * @param immediate New immediate
	 */
	public void setImmediate(int operand, long immediate) {
		switch (getOpKind(operand)) {
		case OpKind.IMMEDIATE8:
			setImmediate8((byte)immediate);
			break;
		case OpKind.IMMEDIATE8TO16:
			setImmediate8to16((short)immediate);
			break;
		case OpKind.IMMEDIATE8TO32:
			setImmediate8to32((int)immediate);
			break;
		case OpKind.IMMEDIATE8TO64:
			setImmediate8to64(immediate);
			break;
		case OpKind.IMMEDIATE8_2ND:
			setImmediate8_2nd((byte)immediate);
			break;
		case OpKind.IMMEDIATE16:
			setImmediate16((short)immediate);
			break;
		case OpKind.IMMEDIATE32TO64:
			setImmediate32to64(immediate);
			break;
		case OpKind.IMMEDIATE32:
			setImmediate32((int)immediate);
			break;
		case OpKind.IMMEDIATE64:
			setImmediate64(immediate);
			break;
		default:
			throw new IllegalArgumentException();
		}
	}

	/**
	 * Gets the operand's immediate value. Use this property if the operand has kind {@link OpKind#IMMEDIATE8}
	 */
	public byte getImmediate8() {
		return (byte)immediate;
	}

	/**
	 * Gets the operand's immediate value. Use this property if the operand has kind {@link OpKind#IMMEDIATE8}
	 */
	public void setImmediate8(byte value) {
		immediate = (immediate & 0xFFFF_FF00) | (value & 0xFF);
	}

	/**
	 * Gets the operand's immediate value. Use this property if the operand has kind {@link OpKind#IMMEDIATE8_2ND}
	 */
	public byte getImmediate8_2nd() {
		return (byte)memDispl;
	}

	/**
	 * Gets the operand's immediate value. Use this property if the operand has kind {@link OpKind#IMMEDIATE8_2ND}
	 */
	public void setImmediate8_2nd(byte value) {
		memDispl = value & 0xFF;
	}

	/**
	 * Gets the operand's immediate value. Use this property if the operand has kind {@link OpKind#IMMEDIATE16}
	 */
	public short getImmediate16() {
		return (short)immediate;
	}

	/**
	 * Gets the operand's immediate value. Use this property if the operand has kind {@link OpKind#IMMEDIATE16}
	 */
	public void setImmediate16(short value) {
		immediate = (int)value & 0xFFFF;
	}

	/**
	 * Gets the operand's immediate value. Use this property if the operand has kind {@link OpKind#IMMEDIATE32}
	 */
	public int getImmediate32() {
		return immediate;
	}

	/**
	 * Gets the operand's immediate value. Use this property if the operand has kind {@link OpKind#IMMEDIATE32}
	 */
	public void setImmediate32(int value) {
		immediate = value;
	}

	/**
	 * Gets the operand's immediate value. Use this property if the operand has kind {@link OpKind#IMMEDIATE64}
	 */
	public long getImmediate64() {
		return (memDispl << 32) | ((long)immediate & 0xFFFF_FFFF);
	}

	/**
	 * Gets the operand's immediate value. Use this property if the operand has kind {@link OpKind#IMMEDIATE64}
	 */
	public void setImmediate64(long value) {
		immediate = (int)value;
		memDispl = value >>> 32;
	}

	/**
	 * Gets the operand's immediate value. Use this property if the operand has kind {@link OpKind#IMMEDIATE8TO16}
	 */
	public short getImmediate8to16() {
		return (byte)immediate;
	}

	/**
	 * Gets the operand's immediate value. Use this property if the operand has kind {@link OpKind#IMMEDIATE8TO16}
	 */
	public void setImmediate8to16(short value) {
		immediate = (int)(byte)value;
	}

	/**
	 * Gets the operand's immediate value. Use this property if the operand has kind {@link OpKind#IMMEDIATE8TO32}
	 */
	public int getImmediate8to32() {
		return (byte)immediate;
	}

	/**
	 * Gets the operand's immediate value. Use this property if the operand has kind {@link OpKind#IMMEDIATE8TO32}
	 */
	public void setImmediate8to32(int value) {
		immediate = (int)(byte)value;
	}

	/**
	 * Gets the operand's immediate value. Use this property if the operand has kind {@link OpKind#IMMEDIATE8TO64}
	 */
	public long getImmediate8to64() {
		return (byte)immediate;
	}

	/**
	 * Gets the operand's immediate value. Use this property if the operand has kind {@link OpKind#IMMEDIATE8TO64}
	 */
	public void setImmediate8to64(long value) {
		immediate = (int)(byte)value;
	}

	/**
	 * Gets the operand's immediate value. Use this property if the operand has kind {@link OpKind#IMMEDIATE32TO64}
	 */
	public long getImmediate32to64() {
		return (int)immediate;
	}

	/**
	 * Gets the operand's immediate value. Use this property if the operand has kind {@link OpKind#IMMEDIATE32TO64}
	 */
	public void setImmediate32to64(long value) {
		immediate = (int)value;
	}

	/**
	 * Gets the operand's branch target. Use this property if the operand has kind {@link OpKind#NEAR_BRANCH16}
	 */
	public short getNearBranch16() {
		return (short)memDispl;
	}

	/**
	 * Gets the operand's branch target. Use this property if the operand has kind {@link OpKind#NEAR_BRANCH16}
	 */
	public void setNearBranch16(short value) {
		memDispl = value & 0xFFFF;
	}

	/**
	 * Gets the operand's branch target. Use this property if the operand has kind {@link OpKind#NEAR_BRANCH32}
	 */
	public int getNearBranch32() {
		return (int)memDispl;
	}

	/**
	 * Gets the operand's branch target. Use this property if the operand has kind {@link OpKind#NEAR_BRANCH32}
	 */
	public void setNearBranch32(int value) {
		memDispl = value & 0xFFFF_FFFF;
	}

	/**
	 * Gets the operand's branch target. Use this property if the operand has kind {@link OpKind#NEAR_BRANCH64}
	 */
	public long getNearBranch64() {
		return memDispl;
	}

	/**
	 * Gets the operand's branch target. Use this property if the operand has kind {@link OpKind#NEAR_BRANCH64}
	 */
	public void setNearBranch64(long value) {
		memDispl = value;
	}

	/**
	 * Gets the near branch target if it's a <code>CALL</code>/<code>JMP</code>/<code>Jcc</code> near branch instruction.
	 *
	 * (i.e., if {@link #getOp0Kind()} is {@link OpKind#NEAR_BRANCH16}, {@link OpKind#NEAR_BRANCH32} or {@link OpKind#NEAR_BRANCH64})
	 */
	public long getNearBranchTarget() {
		int opKind = getOp0Kind();
		// Check if JKZD/JKNZD
		if (getOpCount() == 2)
			opKind = getOp1Kind();
		switch (opKind) {
		case OpKind.NEAR_BRANCH16:
			return getNearBranch16();
		case OpKind.NEAR_BRANCH32:
			return getNearBranch32();
		case OpKind.NEAR_BRANCH64:
			return getNearBranch64();
		default:
			return 0;
		}
	}

	/**
	 * Gets the operand's branch target. Use this property if the operand has kind {@link OpKind#FAR_BRANCH16}
	 */
	public short getFarBranch16() {
		return (short)immediate;
	}

	/**
	 * Gets the operand's branch target. Use this property if the operand has kind {@link OpKind#FAR_BRANCH16}
	 */
	public void setFarBranch16(short value) {
		immediate = value & 0xFFFF;
	}

	/**
	 * Gets the operand's branch target. Use this property if the operand has kind {@link OpKind#FAR_BRANCH32}
	 */
	public int getFarBranch32() {
		return immediate;
	}

	/**
	 * Gets the operand's branch target. Use this property if the operand has kind {@link OpKind#FAR_BRANCH32}
	 */
	public void setFarBranch32(int value) {
		immediate = value;
	}

	/**
	 * Gets the operand's branch target selector. Use this property if the operand has kind {@link OpKind#FAR_BRANCH16} or
	 * {@link OpKind#FAR_BRANCH32}
	 */
	public short getFarBranchSelector() {
		return (short)memDispl;
	}

	/**
	 * Gets the operand's branch target selector. Use this property if the operand has kind {@link OpKind#FAR_BRANCH16} or
	 * {@link OpKind#FAR_BRANCH32}
	 */
	public void setFarBranchSelector(short value) {
		memDispl = value & 0xFFFF;
	}

	/**
	 * Gets the memory operand's base register (a {@link Register} enum variant) or {@link Register#NONE} if none.
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY}
	 */
	public int getMemoryBase() {
		return memBaseReg & REG_MASK;
	}

	/**
	 * Gets the memory operand's base register (a {@link Register} enum variant) or {@link Register#NONE} if none.
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY}
	 */
	public void setMemoryBase(int value) {
		memBaseReg = (byte)value;
	}

	/**
	 * Gets the memory operand's index register (a {@link Register} enum variant) or {@link Register#NONE} if none.
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY}
	 */
	public int getMemoryIndex() {
		return memIndexReg & REG_MASK;
	}

	/**
	 * Gets the memory operand's index register (a {@link Register} enum variant) or {@link Register#NONE} if none.
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY}
	 */
	public void setMemoryIndex(int value) {
		memIndexReg = (byte)value;
	}

	/**
	 * Gets operand #0's register value (a {@link Register} enum variant).
	 *
	 * Use this property if operand #0 ({@link #getOp0Kind()}) has kind {@link OpKind#REGISTER}
	 *
	 * @see #getOpCount()
	 * @see #getOpRegister(int)
	 */
	public int getOp0Register() {
		return reg0 & REG_MASK;
	}

	/**
	 * Gets operand #0's register value (a {@link Register} enum variant).
	 *
	 * Use this property if operand #0 ({@link #getOp0Kind()}) has kind {@link OpKind#REGISTER}
	 *
	 * @see #getOpCount()
	 * @see #getOpRegister(int)
	 */
	public void setOp0Register(int value) {
		reg0 = (byte)value;
	}

	/**
	 * Gets operand #1's register value (a {@link Register} enum variant).
	 *
	 * Use this property if operand #1 ({@link #getOp1Kind()}) has kind {@link OpKind#REGISTER}
	 *
	 * @see #getOpCount()
	 * @see #getOpRegister(int)
	 */
	public int getOp1Register() {
		return reg1 & REG_MASK;
	}

	/**
	 * Gets operand #1's register value (a {@link Register} enum variant).
	 *
	 * Use this property if operand #1 ({@link #getOp1Kind()}) has kind {@link OpKind#REGISTER}
	 *
	 * @see #getOpCount()
	 * @see #getOpRegister(int)
	 */
	public void setOp1Register(int value) {
		reg1 = (byte)value;
	}

	/**
	 * Gets operand #2's register value (a {@link Register} enum variant).
	 *
	 * Use this property if operand #2 ({@link #getOp2Kind()}) has kind {@link OpKind#REGISTER}
	 *
	 * @see #getOpCount()
	 * @see #getOpRegister(int)
	 */
	public int getOp2Register() {
		return reg2 & REG_MASK;
	}

	/**
	 * Gets operand #2's register value (a {@link Register} enum variant).
	 *
	 * Use this property if operand #2 ({@link #getOp2Kind()}) has kind {@link OpKind#REGISTER}
	 *
	 * @see #getOpCount()
	 * @see #getOpRegister(int)
	 */
	public void setOp2Register(int value) {
		reg2 = (byte)value;
	}

	/**
	 * Gets operand #3's register value (a {@link Register} enum variant).
	 *
	 * Use this property if operand #3 ({@link #getOp3Kind()}) has kind {@link OpKind#REGISTER}
	 *
	 * @see #getOpCount()
	 * @see #getOpRegister(int)
	 */
	public int getOp3Register() {
		return reg3 & REG_MASK;
	}

	/**
	 * Gets operand #3's register value (a {@link Register} enum variant).
	 *
	 * Use this property if operand #3 ({@link #getOp3Kind()}) has kind {@link OpKind#REGISTER}
	 *
	 * @see #getOpCount()
	 * @see #getOpRegister(int)
	 */
	public void setOp3Register(int value) {
		reg3 = (byte)value;
	}

	/**
	 * Gets operand #4's register value (a {@link Register} enum variant).
	 *
	 * Use this property if operand #4 ({@link #getOp4Kind()}) has kind {@link OpKind#REGISTER}
	 *
	 * @see #getOpCount()
	 * @see #getOpRegister(int)
	 */
	public int getOp4Register() {
		return Register.NONE;
	}

	/**
	 * Gets operand #4's register value (a {@link Register} enum variant).
	 *
	 * Use this property if operand #4 ({@link #getOp4Kind()}) has kind {@link OpKind#REGISTER}
	 *
	 * @see #getOpCount()
	 * @see #getOpRegister(int)
	 */
	public void setOp4Register(int value) {
		if (value != Register.NONE)
			throw new IllegalArgumentException();
	}

	/**
	 * Gets the operand's register value (a {@link Register} enum variant).
	 *
	 * Use this property if the operand has kind {@link OpKind#REGISTER}
	 *
	 * @param operand Operand number, 0-4
	 */
	public int getOpRegister(int operand) {
		switch (operand) {
		case 0:
			return getOp0Register();
		case 1:
			return getOp1Register();
		case 2:
			return getOp2Register();
		case 3:
			return getOp3Register();
		case 4:
			return getOp4Register();
		default:
			throw new IllegalArgumentException();
		}
	}

	/**
	 * Sets the operand's register value (a {@link Register} enum variant).
	 *
	 * Use this property if the operand has kind {@link OpKind#REGISTER}
	 *
	 * @param operand  Operand number, 0-4
	 * @param register Register
	 */
	public void setOpRegister(int operand, int register) {
		switch (operand) {
		case 0:
			setOp0Register(register);
			break;
		case 1:
			setOp1Register(register);
			break;
		case 2:
			setOp2Register(register);
			break;
		case 3:
			setOp3Register(register);
			break;
		case 4:
			setOp4Register(register);
			break;
		default:
			throw new IllegalArgumentException();
		}
	}

	/**
	 * Gets the opmask register ({@link Register#K1} - {@link Register#K7}) or {@link Register#NONE} if none
	 */
	public int getOpMask() {
		int r = (flags1 >>> InstrFlags1.OP_MASK_SHIFT) & InstrFlags1.OP_MASK_MASK;
		return r == 0 ? Register.NONE : r + Register.K0;
	}

	/**
	 * Gets the opmask register ({@link Register#K1} - {@link Register#K7}) or {@link Register#NONE} if none
	 */
	public void setOpMask(int value) {
		int r;
		if (value == Register.NONE)
			r = 0;
		else
			r = (value - Register.K0) & InstrFlags1.OP_MASK_MASK;
		flags1 = (flags1 & ~(InstrFlags1.OP_MASK_MASK << InstrFlags1.OP_MASK_SHIFT)) |
				(r << InstrFlags1.OP_MASK_SHIFT);
	}

	/**
	 * <code>true</code> if there's an opmask register ({@link #getOpMask()})
	 */
	public boolean getHasOpMask() {
		return (flags1 & (InstrFlags1.OP_MASK_MASK << InstrFlags1.OP_MASK_SHIFT)) != 0;
	}

	/**
	 * <code>true</code> if zeroing-masking, <code>false</code> if merging-masking.
	 *
	 * Only used by most EVEX encoded instructions that use opmask registers.
	 */
	public boolean getZeroingMasking() {
		return (flags1 & InstrFlags1.ZEROING_MASKING) != 0;
	}

	/**
	 * <code>true</code> if zeroing-masking, <code>false</code> if merging-masking.
	 *
	 * Only used by most EVEX encoded instructions that use opmask registers.
	 */
	public void setZeroingMasking(boolean value) {
		if (value)
			flags1 |= InstrFlags1.ZEROING_MASKING;
		else
			flags1 &= ~InstrFlags1.ZEROING_MASKING;
	}

	/**
	 * <code>true</code> if merging-masking, <code>false</code> if zeroing-masking.
	 *
	 * Only used by most EVEX encoded instructions that use opmask registers.
	 */
	public boolean getMergingMasking() {
		return (flags1 & InstrFlags1.ZEROING_MASKING) == 0;
	}

	/**
	 * <code>true</code> if merging-masking, <code>false</code> if zeroing-masking.
	 *
	 * Only used by most EVEX encoded instructions that use opmask registers.
	 */
	public void setMergingMasking(boolean value) {
		if (value)
			flags1 &= ~InstrFlags1.ZEROING_MASKING;
		else
			flags1 |= InstrFlags1.ZEROING_MASKING;
	}

	/**
	 * Rounding control (a {@link RoundingControl} enum variant) or {@link RoundingControl#NONE} if the instruction doesn't use it.
	 *
	 * SAE is implied but {@link #getSuppressAllExceptions()} still returns <code>false</code>.
	 */
	public int getRoundingControl() {
		return (flags1 >>> InstrFlags1.ROUNDING_CONTROL_SHIFT) & InstrFlags1.ROUNDING_CONTROL_MASK;
	}

	/**
	 * Rounding control (a {@link RoundingControl} enum variant) or {@link RoundingControl#NONE} if the instruction doesn't use it.
	 *
	 * SAE is implied but {@link #getSuppressAllExceptions()} still returns <code>false</code>.
	 */
	public void setRoundingControl(int value) {
		flags1 = (flags1 & ~(InstrFlags1.ROUNDING_CONTROL_MASK << InstrFlags1.ROUNDING_CONTROL_SHIFT))
				| (value << InstrFlags1.ROUNDING_CONTROL_SHIFT);
	}

	/**
	 * Number of elements in a <code>db</code>/<code>dw</code>/<code>dd</code>/<code>dq</code> directive: <code>db</code>: 1-16; <code>dw</code>: 1-8;
	 * <code>dd</code>:
	 * 1-4; <code>dq</code>: 1-2.
	 *
	 * Can only be called if {@link #getCode()} is {@link Code#DECLAREBYTE}, {@link Code#DECLAREWORD}, {@link Code#DECLAREDWORD},
	 * {@link Code#DECLAREQWORD}
	 */
	public int getDeclareDataCount() {
		return ((flags1 >>> InstrFlags1.DATA_LENGTH_SHIFT) & InstrFlags1.DATA_LENGTH_MASK) + 1;
	}

	/**
	 * Number of elements in a <code>db</code>/<code>dw</code>/<code>dd</code>/<code>dq</code> directive: <code>db</code>: 1-16; <code>dw</code>: 1-8;
	 * <code>dd</code>:
	 * 1-4; <code>dq</code>: 1-2.
	 *
	 * Can only be called if {@link #getCode()} is {@link Code#DECLAREBYTE}, {@link Code#DECLAREWORD}, {@link Code#DECLAREDWORD},
	 * {@link Code#DECLAREQWORD}
	 */
	public void setDeclareDataCount(int value) {
		flags1 = (flags1 & ~(InstrFlags1.DATA_LENGTH_MASK << InstrFlags1.DATA_LENGTH_SHIFT))
				| (((value - 1) & InstrFlags1.DATA_LENGTH_MASK) << InstrFlags1.DATA_LENGTH_SHIFT);
	}

	/**
	 * Sets a new <code>db</code> value.
	 *
	 * Can only be called if {@link #getCode()} is {@link Code#DECLAREBYTE}
	 *
	 * @param index Index (0-15)
	 * @param value New value
	 *
	 * @see #getDeclareDataCount()
	 */
	public void setDeclareByteValue(int index, byte value) {
		switch (index) {
		case 0:
			reg0 = value;
			break;
		case 1:
			reg1 = value;
			break;
		case 2:
			reg2 = value;
			break;
		case 3:
			reg3 = value;
			break;
		case 4:
			immediate = (immediate & 0xFFFFFF00) | (value & 0xFF);
			break;
		case 5:
			immediate = (immediate & 0xFFFF00FF) | ((value & 0xFF) << 8);
			break;
		case 6:
			immediate = (immediate & 0xFF00FFFF) | ((value & 0xFF) << 16);
			break;
		case 7:
			immediate = (immediate & 0x00FFFFFF) | ((value & 0xFF) << 24);
			break;
		case 8:
			memDispl = (memDispl & 0xFFFF_FFFF_FFFF_FF00L) | (long)(value & 0xFF);
			break;
		case 9:
			memDispl = (memDispl & 0xFFFF_FFFF_FFFF_00FFL) | ((long)(value & 0xFF) << 8);
			break;
		case 10:
			memDispl = (memDispl & 0xFFFF_FFFF_FF00_FFFFL) | ((long)(value & 0xFF) << 16);
			break;
		case 11:
			memDispl = (memDispl & 0xFFFF_FFFF_00FF_FFFFL) | ((long)(value & 0xFF) << 24);
			break;
		case 12:
			memDispl = (memDispl & 0xFFFF_FF00_FFFF_FFFFL) | ((long)(value & 0xFF) << 32);
			break;
		case 13:
			memDispl = (memDispl & 0xFFFF_00FF_FFFF_FFFFL) | ((long)(value & 0xFF) << 40);
			break;
		case 14:
			memDispl = (memDispl & 0xFF00_FFFF_FFFF_FFFFL) | ((long)(value & 0xFF) << 48);
			break;
		case 15:
			memDispl = (memDispl & 0x00FF_FFFF_FFFF_FFFFL) | ((long)value << 56);
			break;
		default:
			throw new IllegalArgumentException();
		}
	}

	/**
	 * Gets a <code>db</code> value.
	 *
	 * Can only be called if {@link #getCode()} is {@link Code#DECLAREBYTE}
	 *
	 * @param index Index (0-15)
	 *
	 * @see #getDeclareDataCount()
	 */
	public byte getDeclareByteValue(int index) {
		switch (index) {
		case 0:
			return reg0;
		case 1:
			return reg1;
		case 2:
			return reg2;
		case 3:
			return reg3;
		case 4:
			return (byte)immediate;
		case 5:
			return (byte)(immediate >>> 8);
		case 6:
			return (byte)(immediate >>> 16);
		case 7:
			return (byte)(immediate >>> 24);
		case 8:
			return (byte)memDispl;
		case 9:
			return (byte)((int)memDispl >>> 8);
		case 10:
			return (byte)((int)memDispl >>> 16);
		case 11:
			return (byte)((int)memDispl >>> 24);
		case 12:
			return (byte)(memDispl >>> 32);
		case 13:
			return (byte)(memDispl >>> 40);
		case 14:
			return (byte)(memDispl >>> 48);
		case 15:
			return (byte)(memDispl >>> 56);
		default:
			throw new IllegalArgumentException();
		}
	}

	/**
	 * Sets a new <code>dw</code> value.
	 *
	 * Can only be called if {@link #getCode()} is {@link Code#DECLAREWORD}
	 *
	 * @param index Index (0-7)
	 * @param value New value
	 *
	 * @see #getDeclareDataCount()
	 */
	public void setDeclareWordValue(int index, short value) {
		switch (index) {
		case 0:
			reg0 = (byte)value;
			reg1 = (byte)(value >>> 8);
			break;
		case 1:
			reg2 = (byte)value;
			reg3 = (byte)(value >>> 8);
			break;
		case 2:
			immediate = (immediate & 0xFFFF0000) | (value & 0xFFFF);
			break;
		case 3:
			immediate = (immediate & 0xFFFF) | (value << 16);
			break;
		case 4:
			memDispl = (memDispl & 0xFFFF_FFFF_FFFF_0000L) | (long)(value & 0xFFFF);
			break;
		case 5:
			memDispl = (memDispl & 0xFFFF_FFFF_0000_FFFFL) | ((long)(value & 0xFFFF) << 16);
			break;
		case 6:
			memDispl = (memDispl & 0xFFFF_0000_FFFF_FFFFL) | ((long)(value & 0xFFFF) << 32);
			break;
		case 7:
			memDispl = (memDispl & 0x0000_FFFF_FFFF_FFFFL) | ((long)value << 48);
			break;
		default:
			throw new IllegalArgumentException();
		}
	}

	/**
	 * Gets a <code>dw</code> value.
	 *
	 * Can only be called if {@link #getCode()} is {@link Code#DECLAREWORD}
	 *
	 * @param index Index (0-7)
	 *
	 * @see #getDeclareDataCount()
	 */
	public short getDeclareWordValue(int index) {
		switch (index) {
		case 0:
			return (short)((reg0 & 0xFF) | ((reg1 & 0xFF) << 8));
		case 1:
			return (short)((reg2 & 0xFF) | ((reg3 & 0xFF) << 8));
		case 2:
			return (short)immediate;
		case 3:
			return (short)(immediate >>> 16);
		case 4:
			return (short)memDispl;
		case 5:
			return (short)((int)memDispl >>> 16);
		case 6:
			return (short)(memDispl >>> 32);
		case 7:
			return (short)(memDispl >>> 48);
		default:
			throw new IllegalArgumentException();
		}
	}

	/**
	 * Sets a new <code>dd</code> value.
	 *
	 * Can only be called if {@link #getCode()} is {@link Code#DECLAREDWORD}
	 *
	 * @param index Index (0-3)
	 * @param value New value
	 *
	 * @see #getDeclareDataCount()
	 */
	public void setDeclareDwordValue(int index, int value) {
		switch (index) {
		case 0:
			reg0 = (byte)value;
			reg1 = (byte)(value >>> 8);
			reg2 = (byte)(value >>> 16);
			reg3 = (byte)(value >>> 24);
			break;
		case 1:
			immediate = value;
			break;
		case 2:
			memDispl = (memDispl & 0xFFFF_FFFF_0000_0000L) | ((long)value & 0xFFFF_FFFF);
			break;
		case 3:
			memDispl = (memDispl & 0x0000_0000_FFFF_FFFFL) | ((long)value << 32);
			break;
		default:
			throw new IllegalArgumentException();
		}
	}

	/**
	 * Gets a <code>dd</code> value.
	 *
	 * Can only be called if {@link #getCode()} is {@link Code#DECLAREDWORD}
	 *
	 * @param index Index (0-3)
	 *
	 * @see #getDeclareDataCount()
	 */
	public int getDeclareDwordValue(int index) {
		switch (index) {
		case 0:
			return (reg0 & 0xFF) | ((reg1 & 0xFF) << 8) | ((reg2 & 0xFF) << 16) | (reg3 << 24);
		case 1:
			return immediate;
		case 2:
			return (int)memDispl;
		case 3:
			return (int)(memDispl >>> 32);
		default:
			throw new IllegalArgumentException();
		}
	}

	/**
	 * Sets a new <code>dq</code> value.
	 *
	 * Can only be called if {@link #getCode()} is {@link Code#DECLAREQWORD}
	 *
	 * @param index Index (0-1)
	 * @param value New value
	 *
	 * @see #getDeclareDataCount()
	 */
	public void setDeclareQwordValue(int index, long value) {
		int v;
		switch (index) {
		case 0:
			v = (int)value;
			reg0 = (byte)v;
			reg1 = (byte)(v >>> 8);
			reg2 = (byte)(v >>> 16);
			reg3 = (byte)(v >>> 24);
			immediate = (int)(value >>> 32);
			break;
		case 1:
			memDispl = value;
			break;
		default:
			throw new IllegalArgumentException();
		}
	}

	/**
	 * Gets a <code>dq</code> value.
	 *
	 * Can only be called if {@link #getCode()} is {@link Code#DECLAREQWORD}
	 *
	 * @param index Index (0-1)
	 *
	 * @see #getDeclareDataCount()
	 */
	public long getDeclareQwordValue(int index) {
		switch (index) {
		case 0:
			return (long)(reg0 & 0xFF) | (long)((reg1 & 0xFF) << 8) | (long)((reg2 & 0xFF) << 16) | (((long)reg3 & 0xFF) << 24)
					| ((long)immediate << 32);
		case 1:
			return memDispl;
		default:
			throw new IllegalArgumentException();
		}
	}

	/**
	 * Checks if this is a VSIB instruction.
	 *
	 * @see #isVsib32()
	 * @see #isVsib64()
	 * @see #getVsib()
	 */
	public boolean isVsib() {
		return (getVsib() & VsibFlags.VSIB) != 0;
	}

	/**
	 * VSIB instructions only ({@link #isVsib()}): <code>true</code> if it's using 32-bit indexes, <code>false</code> if it's using 64-bit indexes
	 *
	 * @see #getVsib()
	 */
	public boolean isVsib32() {
		return (getVsib() & VsibFlags.VSIB32) != 0;
	}

	/**
	 * VSIB instructions only ({@link #isVsib()}): <code>true</code> if it's using 64-bit indexes, <code>false</code> if it's using 32-bit indexes
	 *
	 * @see #getVsib()
	 */
	public boolean isVsib64() {
		return (getVsib() & VsibFlags.VSIB64) != 0;
	}

	/**
	 * Checks if it's a VSIB instruction
	 *
	 * @return A {@link VsibFlags} flags value
	 */
	public int getVsib() {
		switch (getCode()) {
		// GENERATOR-BEGIN: Vsib32
		// ⚠️This was generated by GENERATOR!🦹‍♂️
		case Code.VEX_VPGATHERDD_XMM_VM32X_XMM:
		case Code.VEX_VPGATHERDD_YMM_VM32Y_YMM:
		case Code.VEX_VPGATHERDQ_XMM_VM32X_XMM:
		case Code.VEX_VPGATHERDQ_YMM_VM32X_YMM:
		case Code.EVEX_VPGATHERDD_XMM_K1_VM32X:
		case Code.EVEX_VPGATHERDD_YMM_K1_VM32Y:
		case Code.EVEX_VPGATHERDD_ZMM_K1_VM32Z:
		case Code.EVEX_VPGATHERDQ_XMM_K1_VM32X:
		case Code.EVEX_VPGATHERDQ_YMM_K1_VM32X:
		case Code.EVEX_VPGATHERDQ_ZMM_K1_VM32Y:
		case Code.VEX_VGATHERDPS_XMM_VM32X_XMM:
		case Code.VEX_VGATHERDPS_YMM_VM32Y_YMM:
		case Code.VEX_VGATHERDPD_XMM_VM32X_XMM:
		case Code.VEX_VGATHERDPD_YMM_VM32X_YMM:
		case Code.EVEX_VGATHERDPS_XMM_K1_VM32X:
		case Code.EVEX_VGATHERDPS_YMM_K1_VM32Y:
		case Code.EVEX_VGATHERDPS_ZMM_K1_VM32Z:
		case Code.EVEX_VGATHERDPD_XMM_K1_VM32X:
		case Code.EVEX_VGATHERDPD_YMM_K1_VM32X:
		case Code.EVEX_VGATHERDPD_ZMM_K1_VM32Y:
		case Code.EVEX_VPSCATTERDD_VM32X_K1_XMM:
		case Code.EVEX_VPSCATTERDD_VM32Y_K1_YMM:
		case Code.EVEX_VPSCATTERDD_VM32Z_K1_ZMM:
		case Code.EVEX_VPSCATTERDQ_VM32X_K1_XMM:
		case Code.EVEX_VPSCATTERDQ_VM32X_K1_YMM:
		case Code.EVEX_VPSCATTERDQ_VM32Y_K1_ZMM:
		case Code.EVEX_VSCATTERDPS_VM32X_K1_XMM:
		case Code.EVEX_VSCATTERDPS_VM32Y_K1_YMM:
		case Code.EVEX_VSCATTERDPS_VM32Z_K1_ZMM:
		case Code.EVEX_VSCATTERDPD_VM32X_K1_XMM:
		case Code.EVEX_VSCATTERDPD_VM32X_K1_YMM:
		case Code.EVEX_VSCATTERDPD_VM32Y_K1_ZMM:
		case Code.EVEX_VGATHERPF0DPS_VM32Z_K1:
		case Code.EVEX_VGATHERPF0DPD_VM32Y_K1:
		case Code.EVEX_VGATHERPF1DPS_VM32Z_K1:
		case Code.EVEX_VGATHERPF1DPD_VM32Y_K1:
		case Code.EVEX_VSCATTERPF0DPS_VM32Z_K1:
		case Code.EVEX_VSCATTERPF0DPD_VM32Y_K1:
		case Code.EVEX_VSCATTERPF1DPS_VM32Z_K1:
		case Code.EVEX_VSCATTERPF1DPD_VM32Y_K1:
		case Code.MVEX_VPGATHERDD_ZMM_K1_MVT:
		case Code.MVEX_VPGATHERDQ_ZMM_K1_MVT:
		case Code.MVEX_VGATHERDPS_ZMM_K1_MVT:
		case Code.MVEX_VGATHERDPD_ZMM_K1_MVT:
		case Code.MVEX_VPSCATTERDD_MVT_K1_ZMM:
		case Code.MVEX_VPSCATTERDQ_MVT_K1_ZMM:
		case Code.MVEX_VSCATTERDPS_MVT_K1_ZMM:
		case Code.MVEX_VSCATTERDPD_MVT_K1_ZMM:
		case Code.MVEX_UNDOC_ZMM_K1_MVT_512_66_0F38_W0_B0:
		case Code.MVEX_UNDOC_ZMM_K1_MVT_512_66_0F38_W0_B2:
		case Code.MVEX_UNDOC_ZMM_K1_MVT_512_66_0F38_W0_C0:
		case Code.MVEX_VGATHERPF0HINTDPS_MVT_K1:
		case Code.MVEX_VGATHERPF0HINTDPD_MVT_K1:
		case Code.MVEX_VGATHERPF0DPS_MVT_K1:
		case Code.MVEX_VGATHERPF1DPS_MVT_K1:
		case Code.MVEX_VSCATTERPF0HINTDPS_MVT_K1:
		case Code.MVEX_VSCATTERPF0HINTDPD_MVT_K1:
		case Code.MVEX_VSCATTERPF0DPS_MVT_K1:
		case Code.MVEX_VSCATTERPF1DPS_MVT_K1:
			return VsibFlags.VSIB | VsibFlags.VSIB32;
		// GENERATOR-END: Vsib32

		// GENERATOR-BEGIN: Vsib64
		// ⚠️This was generated by GENERATOR!🦹‍♂️
		case Code.VEX_VPGATHERQD_XMM_VM64X_XMM:
		case Code.VEX_VPGATHERQD_XMM_VM64Y_XMM:
		case Code.VEX_VPGATHERQQ_XMM_VM64X_XMM:
		case Code.VEX_VPGATHERQQ_YMM_VM64Y_YMM:
		case Code.EVEX_VPGATHERQD_XMM_K1_VM64X:
		case Code.EVEX_VPGATHERQD_XMM_K1_VM64Y:
		case Code.EVEX_VPGATHERQD_YMM_K1_VM64Z:
		case Code.EVEX_VPGATHERQQ_XMM_K1_VM64X:
		case Code.EVEX_VPGATHERQQ_YMM_K1_VM64Y:
		case Code.EVEX_VPGATHERQQ_ZMM_K1_VM64Z:
		case Code.VEX_VGATHERQPS_XMM_VM64X_XMM:
		case Code.VEX_VGATHERQPS_XMM_VM64Y_XMM:
		case Code.VEX_VGATHERQPD_XMM_VM64X_XMM:
		case Code.VEX_VGATHERQPD_YMM_VM64Y_YMM:
		case Code.EVEX_VGATHERQPS_XMM_K1_VM64X:
		case Code.EVEX_VGATHERQPS_XMM_K1_VM64Y:
		case Code.EVEX_VGATHERQPS_YMM_K1_VM64Z:
		case Code.EVEX_VGATHERQPD_XMM_K1_VM64X:
		case Code.EVEX_VGATHERQPD_YMM_K1_VM64Y:
		case Code.EVEX_VGATHERQPD_ZMM_K1_VM64Z:
		case Code.EVEX_VPSCATTERQD_VM64X_K1_XMM:
		case Code.EVEX_VPSCATTERQD_VM64Y_K1_XMM:
		case Code.EVEX_VPSCATTERQD_VM64Z_K1_YMM:
		case Code.EVEX_VPSCATTERQQ_VM64X_K1_XMM:
		case Code.EVEX_VPSCATTERQQ_VM64Y_K1_YMM:
		case Code.EVEX_VPSCATTERQQ_VM64Z_K1_ZMM:
		case Code.EVEX_VSCATTERQPS_VM64X_K1_XMM:
		case Code.EVEX_VSCATTERQPS_VM64Y_K1_XMM:
		case Code.EVEX_VSCATTERQPS_VM64Z_K1_YMM:
		case Code.EVEX_VSCATTERQPD_VM64X_K1_XMM:
		case Code.EVEX_VSCATTERQPD_VM64Y_K1_YMM:
		case Code.EVEX_VSCATTERQPD_VM64Z_K1_ZMM:
		case Code.EVEX_VGATHERPF0QPS_VM64Z_K1:
		case Code.EVEX_VGATHERPF0QPD_VM64Z_K1:
		case Code.EVEX_VGATHERPF1QPS_VM64Z_K1:
		case Code.EVEX_VGATHERPF1QPD_VM64Z_K1:
		case Code.EVEX_VSCATTERPF0QPS_VM64Z_K1:
		case Code.EVEX_VSCATTERPF0QPD_VM64Z_K1:
		case Code.EVEX_VSCATTERPF1QPS_VM64Z_K1:
		case Code.EVEX_VSCATTERPF1QPD_VM64Z_K1:
			return VsibFlags.VSIB | VsibFlags.VSIB64;
		// GENERATOR-END: Vsib64

		default:
			return VsibFlags.NONE;
		}
	}

	/**
	 * Suppress all exceptions (EVEX/MVEX encoded instructions).
	 *
	 * Note that if {@link #getRoundingControl()} is not {@link RoundingControl#NONE},
	 * SAE is implied but this property will still return <code>false</code>.
	 */
	public boolean getSuppressAllExceptions() {
		return (flags1 & InstrFlags1.SUPPRESS_ALL_EXCEPTIONS) != 0;
	}

	/**
	 * Suppress all exceptions (EVEX/MVEX encoded instructions).
	 *
	 * Note that if {@link #getRoundingControl()} is not {@link RoundingControl#NONE},
	 * SAE is implied but this property will still return <code>false</code>.
	 */
	public void setSuppressAllExceptions(boolean value) {
		if (value)
			flags1 |= InstrFlags1.SUPPRESS_ALL_EXCEPTIONS;
		else
			flags1 &= ~InstrFlags1.SUPPRESS_ALL_EXCEPTIONS;
	}

	/**
	 * Checks if the memory operand is <code>RIP</code>/<code>EIP</code> relative
	 */
	public boolean isIPRelativeMemoryOperand() {
		return getMemoryBase() == Register.RIP || getMemoryBase() == Register.EIP;
	}

	/**
	 * Gets the <code>RIP</code>/<code>EIP</code> releative address ({@link #getMemoryDisplacement32()} or {@link #getMemoryDisplacement64()}).
	 *
	 * This property is only valid if there's a memory operand with <code>RIP</code>/<code>EIP</code> relative addressing, see
	 * {@link #isIPRelativeMemoryOperand}
	 */
	public long ipRelativeMemoryAddress() {
		return getMemoryBase() == Register.EIP ? getMemoryDisplacement32() : getMemoryDisplacement64();
	}

	// TODO:
	// /**
	// * Gets the {@link OpCodeInfo}
	// */
	// public OpCodeInfo getOpCode() {
	// return getCode().ToOpCode();
	// }

	/**
	 * TODO: doc this
	 */
	public int getStackPointerIncrement() {
		throw new UnsupportedOperationException(); // TODO:
	}
}
