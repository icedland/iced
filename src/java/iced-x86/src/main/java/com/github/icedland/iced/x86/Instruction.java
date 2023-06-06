// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

import com.github.icedland.iced.x86.enc.InternalEncoderOpCodeHandlers;
import com.github.icedland.iced.x86.enc.Op;
import com.github.icedland.iced.x86.enc.OpCodeHandler;
import com.github.icedland.iced.x86.info.OpCodeInfo;
import com.github.icedland.iced.x86.internal.IcedConstants;
import com.github.icedland.iced.x86.internal.InstrFlags1;
import com.github.icedland.iced.x86.internal.InstrInfoTable;
import com.github.icedland.iced.x86.internal.InstructionMemorySizes;
import com.github.icedland.iced.x86.internal.InstructionOpCounts;
import com.github.icedland.iced.x86.internal.MvexInfo;
import com.github.icedland.iced.x86.internal.MvexInstrFlags;
import com.github.icedland.iced.x86.internal.MvexMemorySizeLut;
import com.github.icedland.iced.x86.internal.info.ImpliedAccess;
import com.github.icedland.iced.x86.internal.info.InfoFlags1;
import com.github.icedland.iced.x86.internal.info.RflagsInfo;
import com.github.icedland.iced.x86.internal.info.RflagsInfoConstants;

/**
 * A 16/32/64-bit instruction.
 *
 * Created by {@link com.github.icedland.iced.x86.dec.Decoder} or by {@code Instruction.create()} methods.
 */
public final class Instruction {
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
	 * Clears all fields
	 */
	public void clear() {
		nextRip = 0;
		memDispl = 0;
		flags1 = 0;
		immediate = 0;
		code = 0;
		memBaseReg = 0;
		memIndexReg = 0;
		reg0 = 0;
		reg1 = 0;
		reg2 = 0;
		reg3 = 0;
		opKind0 = 0;
		opKind1 = 0;
		opKind2 = 0;
		opKind3 = 0;
		scale = 0;
		displSize = 0;
		len = 0;
		pad = 0;
	}

	/**
	 * Gets the hash code of this instance, ignoring some fields, eg.<!-- --> {@code IP}, {@code len} and a few unimportant fields
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
	 * Checks if {@code this} is equal to {@code obj}, ignoring some fields, eg.<!-- --> {@code IP}, {@code len} and a few
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
		com.github.icedland.iced.x86.fmt.masm.MasmFormatter formatter = new com.github.icedland.iced.x86.fmt.masm.MasmFormatter();
		com.github.icedland.iced.x86.fmt.StringOutput output = new com.github.icedland.iced.x86.fmt.StringOutput();
		formatter.format(this, output);
		return output.toString();
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
		nextRip = (long)(value + getLength()) & 0xFFFF_FFFFL;
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
		nextRip = (long)value & 0xFFFF_FFFFL;
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

	private boolean isXacquireInstr() {
		if (getOp0Kind() != OpKind.MEMORY)
			return false;
		if (getLockPrefix())
			return getCode() != Code.CMPXCHG16B_M128;
		return getMnemonic() == Mnemonic.XCHG;
	}

	private boolean isXreleaseInstr() {
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
	 * {@code true} if the instruction has the {@code XACQUIRE} prefix ({@code F2})
	 */
	public boolean getXacquirePrefix() {
		return (flags1 & InstrFlags1.REPNE_PREFIX) != 0 && isXacquireInstr();
	}

	/**
	 * {@code true} if the instruction has the {@code XACQUIRE} prefix ({@code F2})
	 */
	public void setXacquirePrefix(boolean value) {
		if (value)
			flags1 |= InstrFlags1.REPNE_PREFIX;
		else
			flags1 &= ~InstrFlags1.REPNE_PREFIX;
	}

	/**
	 * {@code true} if the instruction has the {@code XRELEASE} prefix ({@code F3})
	 */
	public boolean getXreleasePrefix() {
		return (flags1 & InstrFlags1.REPE_PREFIX) != 0 && isXreleaseInstr();
	}

	/**
	 * {@code true} if the instruction has the {@code XRELEASE} prefix ({@code F3})
	 */
	public void setXreleasePrefix(boolean value) {
		if (value)
			flags1 |= InstrFlags1.REPE_PREFIX;
		else
			flags1 &= ~InstrFlags1.REPE_PREFIX;
	}

	/**
	 * {@code true} if the instruction has the {@code REPE} or {@code REP} prefix ({@code F3})
	 */
	public boolean getRepPrefix() {
		return (flags1 & InstrFlags1.REPE_PREFIX) != 0;
	}

	/**
	 * {@code true} if the instruction has the {@code REPE} or {@code REP} prefix ({@code F3})
	 */
	public void setRepPrefix(boolean value) {
		if (value)
			flags1 |= InstrFlags1.REPE_PREFIX;
		else
			flags1 &= ~InstrFlags1.REPE_PREFIX;
	}

	/**
	 * {@code true} if the instruction has the {@code REPE} or {@code REP} prefix ({@code F3})
	 */
	public boolean getRepePrefix() {
		return (flags1 & InstrFlags1.REPE_PREFIX) != 0;
	}

	/**
	 * {@code true} if the instruction has the {@code REPE} or {@code REP} prefix ({@code F3})
	 */
	public void setRepePrefix(boolean value) {
		if (value)
			flags1 |= InstrFlags1.REPE_PREFIX;
		else
			flags1 &= ~InstrFlags1.REPE_PREFIX;
	}

	/**
	 * {@code true} if the instruction has the {@code REPNE} prefix ({@code F2})
	 */
	public boolean getRepnePrefix() {
		return (flags1 & InstrFlags1.REPNE_PREFIX) != 0;
	}

	/**
	 * {@code true} if the instruction has the {@code REPNE} prefix ({@code F2})
	 */
	public void setRepnePrefix(boolean value) {
		if (value)
			flags1 |= InstrFlags1.REPNE_PREFIX;
		else
			flags1 &= ~InstrFlags1.REPNE_PREFIX;
	}

	/**
	 * {@code true} if the instruction has the {@code LOCK} prefix ({@code F0})
	 */
	public boolean getLockPrefix() {
		return (flags1 & InstrFlags1.LOCK_PREFIX) != 0;
	}

	/**
	 * {@code true} if the instruction has the {@code LOCK} prefix ({@code F0})
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
		return Integer.compareUnsigned(((flags1 >>> InstrFlags1.SEGMENT_PREFIX_SHIFT) & InstrFlags1.SEGMENT_PREFIX_MASK) - 1, 6) < 0;
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
		return Integer.compareUnsigned(index, 6) < 0 ? Register.ES + index : Register.NONE;
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
	 * Valid values are {@code 0}, {@code 1} (16/32/64-bit), {@code 2} (16-bit),
	 * {@code 4} (32-bit), {@code 8} (64-bit).
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
	 * Valid values are {@code 0}, {@code 1} (16/32/64-bit), {@code 2} (16-bit),
	 * {@code 4} (32-bit), {@code 8} (64-bit).
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
	 * {@code true} if the data is broadcast (EVEX instructions only)
	 */
	public boolean getBroadcast() {
		return (flags1 & InstrFlags1.BROADCAST) != 0;
	}

	/**
	 * {@code true} if the data is broadcast (EVEX instructions only)
	 */
	public void setBroadcast(boolean value) {
		if (value)
			flags1 |= InstrFlags1.BROADCAST;
		else
			flags1 &= ~InstrFlags1.BROADCAST;
	}

	/**
	 * {@code true} if eviction hint bit is set ({@code {eh}}) (MVEX instructions only)
	 */
	public boolean getMvexEvictionHint() {
		return MvexInfo.isMvex(getCode()) && (immediate & MvexInstrFlags.EVICTION_HINT) != 0;
	}

	/**
	 * {@code true} if eviction hint bit is set ({@code {eh}}) (MVEX instructions only)
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
		if (!MvexInfo.isMvex(getCode()))
			return MvexRegMemConv.NONE;
		return (immediate >>> MvexInstrFlags.MVEX_REG_MEM_CONV_SHIFT) & MvexInstrFlags.MVEX_REG_MEM_CONV_MASK;
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
		if (MvexInfo.isMvex(index)) {
			int sss = (getMvexRegMemConv() - MvexRegMemConv.MEM_CONV_NONE) & 7;
			return MvexMemorySizeLut.data[MvexInfo.getTupleTypeLutKind(index) * 8 + sss];
		}
		if (getBroadcast())
			return InstructionMemorySizes.sizesBcst[index] & 0xFF;
		else
			return InstructionMemorySizes.sizesNormal[index] & 0xFF;
	}

	/**
	 * Gets the index register scale value, valid values are {@code *1}, {@code *2}, {@code *4}, {@code *8}.
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY}
	 *
	 * @see #getRawMemoryIndexScale()
	 */
	public int getMemoryIndexScale() {
		return 1 << scale;
	}

	/**
	 * Gets the index register scale value, valid values are {@code *1}, {@code *2}, {@code *4}, {@code *8}.
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
	 * Gets the index register scale value, valid values are {@code 0-3}.
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY}
	 *
	 * @see #getMemoryIndexScale()
	 */
	public int getRawMemoryIndexScale() {
		return scale;
	}

	/**
	 * Gets the index register scale value, valid values are {@code 0-3}.
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY}
	 *
	 * @see #getMemoryIndexScale()
	 */
	public void setRawMemoryIndexScale(int value) {
		scale = (byte)(value & 3);
	}

	/**
	 * Gets the memory operand's displacement or the 32-bit absolute address if it's
	 * an {@code EIP} or {@code RIP} relative memory operand.
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY}
	 */
	public int getMemoryDisplacement32() {
		return (int)memDispl;
	}

	/**
	 * Gets the memory operand's displacement or the 32-bit absolute address if it's
	 * an {@code EIP} or {@code RIP} relative memory operand.
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY}
	 */
	public void setMemoryDisplacement32(int value) {
		memDispl = (long)value & 0xFFFF_FFFFL;
	}

	/**
	 * Gets the memory operand's displacement or the 64-bit absolute address if it's
	 * an {@code EIP} or {@code RIP} relative memory operand.
	 *
	 * Use this property if the operand has kind {@link OpKind#MEMORY}
	 */
	public long getMemoryDisplacement64() {
		return memDispl;
	}

	/**
	 * Gets the memory operand's displacement or the 64-bit absolute address if it's
	 * an {@code EIP} or {@code RIP} relative memory operand.
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
			return getImmediate8() & 0xFF;
		case OpKind.IMMEDIATE8_2ND:
			return getImmediate8_2nd() & 0xFF;
		case OpKind.IMMEDIATE16:
			return getImmediate16() & 0xFFFF;
		case OpKind.IMMEDIATE32:
			return getImmediate32() & 0xFFFF_FFFFL;
		case OpKind.IMMEDIATE64:
			return getImmediate64();
		case OpKind.IMMEDIATE8TO16:
			return (long)getImmediate8to16();
		case OpKind.IMMEDIATE8TO32:
			return (long)getImmediate8to32();
		case OpKind.IMMEDIATE8TO64:
			return getImmediate8to64();
		case OpKind.IMMEDIATE32TO64:
			return getImmediate32to64();
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
		return (memDispl << 32) | ((long)immediate & 0xFFFF_FFFFL);
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
		return immediate;
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
		memDispl = (long)value & 0xFFFF_FFFFL;
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
	 * Gets the near branch target if it's a {@code CALL}/{@code JMP}/{@code Jcc} near branch instruction.
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
			return getNearBranch16() & 0xFFFF;
		case OpKind.NEAR_BRANCH32:
			return getNearBranch32() & 0xFFFF_FFFFL;
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
	@SuppressWarnings("deprecation")
	public int getMemoryBase() {
		return memBaseReg & com.github.icedland.iced.x86.internal.Constants.REG_MASK;
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
	@SuppressWarnings("deprecation")
	public int getMemoryIndex() {
		return memIndexReg & com.github.icedland.iced.x86.internal.Constants.REG_MASK;
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
	@SuppressWarnings("deprecation")
	public int getOp0Register() {
		return reg0 & com.github.icedland.iced.x86.internal.Constants.REG_MASK;
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
	@SuppressWarnings("deprecation")
	public int getOp1Register() {
		return reg1 & com.github.icedland.iced.x86.internal.Constants.REG_MASK;
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
	@SuppressWarnings("deprecation")
	public int getOp2Register() {
		return reg2 & com.github.icedland.iced.x86.internal.Constants.REG_MASK;
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
	@SuppressWarnings("deprecation")
	public int getOp3Register() {
		return reg3 & com.github.icedland.iced.x86.internal.Constants.REG_MASK;
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
	 * Gets the raw opmask register (0-7)
	 */
	public int getRawOpMask() {
		return (flags1 >>> InstrFlags1.OP_MASK_SHIFT) & InstrFlags1.OP_MASK_MASK;
	}

	/**
	 * Gets the raw opmask register (0-7)
	 */
	public void setRawOpMask(int value) {
		int r = value & InstrFlags1.OP_MASK_MASK;
		flags1 = (flags1 & ~(InstrFlags1.OP_MASK_MASK << InstrFlags1.OP_MASK_SHIFT)) |
				(r << InstrFlags1.OP_MASK_SHIFT);
	}

	/**
	 * {@code true} if there's an opmask register ({@link #getOpMask()})
	 */
	public boolean hasOpMask() {
		return (flags1 & (InstrFlags1.OP_MASK_MASK << InstrFlags1.OP_MASK_SHIFT)) != 0;
	}

	/**
	 * {@code true} if zeroing-masking, {@code false} if merging-masking.
	 *
	 * Only used by most EVEX encoded instructions that use opmask registers.
	 */
	public boolean getZeroingMasking() {
		return (flags1 & InstrFlags1.ZEROING_MASKING) != 0;
	}

	/**
	 * {@code true} if zeroing-masking, {@code false} if merging-masking.
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
	 * {@code true} if merging-masking, {@code false} if zeroing-masking.
	 *
	 * Only used by most EVEX encoded instructions that use opmask registers.
	 */
	public boolean getMergingMasking() {
		return (flags1 & InstrFlags1.ZEROING_MASKING) == 0;
	}

	/**
	 * {@code true} if merging-masking, {@code false} if zeroing-masking.
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
	 * SAE is implied but {@link #getSuppressAllExceptions()} still returns {@code false}.
	 */
	public int getRoundingControl() {
		return (flags1 >>> InstrFlags1.ROUNDING_CONTROL_SHIFT) & InstrFlags1.ROUNDING_CONTROL_MASK;
	}

	/**
	 * Rounding control (a {@link RoundingControl} enum variant) or {@link RoundingControl#NONE} if the instruction doesn't use it.
	 *
	 * SAE is implied but {@link #getSuppressAllExceptions()} still returns {@code false}.
	 */
	public void setRoundingControl(int value) {
		flags1 = (flags1 & ~(InstrFlags1.ROUNDING_CONTROL_MASK << InstrFlags1.ROUNDING_CONTROL_SHIFT))
				| (value << InstrFlags1.ROUNDING_CONTROL_SHIFT);
	}

	/**
	 * Number of elements in a {@code db}/{@code dw}/{@code dd}/{@code dq} directive: {@code db}: 1-16; {@code dw}: 1-8;
	 * {@code dd}:
	 * 1-4; {@code dq}: 1-2.
	 *
	 * Can only be called if {@link #getCode()} is {@link Code#DECLAREBYTE}, {@link Code#DECLAREWORD}, {@link Code#DECLAREDWORD},
	 * {@link Code#DECLAREQWORD}
	 */
	public int getDeclareDataCount() {
		return ((flags1 >>> InstrFlags1.DATA_LENGTH_SHIFT) & InstrFlags1.DATA_LENGTH_MASK) + 1;
	}

	/**
	 * Number of elements in a {@code db}/{@code dw}/{@code dd}/{@code dq} directive: {@code db}: 1-16; {@code dw}: 1-8;
	 * {@code dd}:
	 * 1-4; {@code dq}: 1-2.
	 *
	 * Can only be called if {@link #getCode()} is {@link Code#DECLAREBYTE}, {@link Code#DECLAREWORD}, {@link Code#DECLAREDWORD},
	 * {@link Code#DECLAREQWORD}
	 */
	public void setDeclareDataCount(int value) {
		flags1 = (flags1 & ~(InstrFlags1.DATA_LENGTH_MASK << InstrFlags1.DATA_LENGTH_SHIFT))
				| (((value - 1) & InstrFlags1.DATA_LENGTH_MASK) << InstrFlags1.DATA_LENGTH_SHIFT);
	}

	/**
	 * Sets a new {@code db} value.
	 *
	 * Can only be called if {@link #getCode()} is {@link Code#DECLAREBYTE}
	 *
	 * @param index Index (0-15)
	 * @param value New value
	 *
	 * @see #getDeclareDataCount()
	 */
	public void setDeclareByteValue(int index, int value) {
		byte v = toByte(value);
		switch (index) {
		case 0:
			reg0 = v;
			break;
		case 1:
			reg1 = v;
			break;
		case 2:
			reg2 = v;
			break;
		case 3:
			reg3 = v;
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
	 * Gets a {@code db} value.
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
	 * Sets a new {@code dw} value.
	 *
	 * Can only be called if {@link #getCode()} is {@link Code#DECLAREWORD}
	 *
	 * @param index Index (0-7)
	 * @param value New value
	 *
	 * @see #getDeclareDataCount()
	 */
	public void setDeclareWordValue(int index, int value) {
		toShort(value); // Ignore return value, verify that it's a short
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
	 * Gets a {@code dw} value.
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
	 * Sets a new {@code dd} value.
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
			memDispl = (memDispl & 0xFFFF_FFFF_0000_0000L) | ((long)value & 0xFFFF_FFFFL);
			break;
		case 3:
			memDispl = (memDispl & 0x0000_0000_FFFF_FFFFL) | ((long)value << 32);
			break;
		default:
			throw new IllegalArgumentException();
		}
	}

	/**
	 * Gets a {@code dd} value.
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
	 * Sets a new {@code dq} value.
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
	 * Gets a {@code dq} value.
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
	 * VSIB instructions only ({@link #isVsib()}): {@code true} if it's using 32-bit indexes, {@code false} if it's using 64-bit indexes
	 *
	 * @see #getVsib()
	 */
	public boolean isVsib32() {
		return (getVsib() & VsibFlags.VSIB32) != 0;
	}

	/**
	 * VSIB instructions only ({@link #isVsib()}): {@code true} if it's using 64-bit indexes, {@code false} if it's using 32-bit indexes
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
	 * SAE is implied but this property will still return {@code false}.
	 */
	public boolean getSuppressAllExceptions() {
		return (flags1 & InstrFlags1.SUPPRESS_ALL_EXCEPTIONS) != 0;
	}

	/**
	 * Suppress all exceptions (EVEX/MVEX encoded instructions).
	 *
	 * Note that if {@link #getRoundingControl()} is not {@link RoundingControl#NONE},
	 * SAE is implied but this property will still return {@code false}.
	 */
	public void setSuppressAllExceptions(boolean value) {
		if (value)
			flags1 |= InstrFlags1.SUPPRESS_ALL_EXCEPTIONS;
		else
			flags1 &= ~InstrFlags1.SUPPRESS_ALL_EXCEPTIONS;
	}

	/**
	 * Checks if the memory operand is {@code RIP}/{@code EIP} relative
	 */
	public boolean isIPRelativeMemoryOperand() {
		return getMemoryBase() == Register.RIP || getMemoryBase() == Register.EIP;
	}

	/**
	 * Gets the {@code RIP}/{@code EIP} releative address ({@link #getMemoryDisplacement32()} or {@link #getMemoryDisplacement64()}).
	 *
	 * This property is only valid if there's a memory operand with {@code RIP}/{@code EIP} relative addressing, see
	 * {@link #isIPRelativeMemoryOperand}
	 */
	public long ipRelativeMemoryAddress() {
		return getMemoryBase() == Register.EIP ? (long)getMemoryDisplacement32() & 0xFFFF_FFFFL : getMemoryDisplacement64();
	}

	/**
	 * Gets an {@link OpCodeInfo}
	 */
	public OpCodeInfo getOpCode() {
		return Code.toOpCode(getCode());
	}

	/**
	 * Gets the number of bytes added to {@code SP}/{@code ESP}/{@code RSP} or 0 if it's not an instruction that pushes or pops data.
	 * <p>
	 * This method assumes the instruction doesn't change the privilege level (eg. {@code IRET/D/Q}). If it's the {@code LEAVE} instruction,
	 * this method returns 0.
	 */
	public int getStackPointerIncrement() {
		switch (getCode()) {
		// GENERATOR-BEGIN: StackPointerIncrementTable
		// ⚠️This was generated by GENERATOR!🦹‍♂️
		case Code.PUSHAD:
			return -32;
		case Code.PUSHAW:
		case Code.CALL_M1664:
			return -16;
		case Code.PUSH_R64:
		case Code.PUSHQ_IMM32:
		case Code.PUSHQ_IMM8:
		case Code.CALL_PTR1632:
		case Code.PUSHFQ:
		case Code.CALL_REL32_64:
		case Code.CALL_RM64:
		case Code.CALL_M1632:
		case Code.PUSH_RM64:
		case Code.PUSHQ_FS:
		case Code.PUSHQ_GS:
			return -8;
		case Code.PUSHD_ES:
		case Code.PUSHD_CS:
		case Code.PUSHD_SS:
		case Code.PUSHD_DS:
		case Code.PUSH_R32:
		case Code.PUSHD_IMM32:
		case Code.PUSHD_IMM8:
		case Code.CALL_PTR1616:
		case Code.PUSHFD:
		case Code.CALL_REL32_32:
		case Code.CALL_RM32:
		case Code.CALL_M1616:
		case Code.PUSH_RM32:
		case Code.PUSHD_FS:
		case Code.PUSHD_GS:
			return -4;
		case Code.PUSHW_ES:
		case Code.PUSHW_CS:
		case Code.PUSHW_SS:
		case Code.PUSHW_DS:
		case Code.PUSH_R16:
		case Code.PUSH_IMM16:
		case Code.PUSHW_IMM8:
		case Code.PUSHFW:
		case Code.CALL_REL16:
		case Code.CALL_RM16:
		case Code.PUSH_RM16:
		case Code.PUSHW_FS:
		case Code.PUSHW_GS:
			return -2;
		case Code.POPW_ES:
		case Code.POPW_CS:
		case Code.POPW_SS:
		case Code.POPW_DS:
		case Code.POP_R16:
		case Code.POP_RM16:
		case Code.POPFW:
		case Code.RETNW:
		case Code.POPW_FS:
		case Code.POPW_GS:
			return 2;
		case Code.POPD_ES:
		case Code.POPD_SS:
		case Code.POPD_DS:
		case Code.POP_R32:
		case Code.POP_RM32:
		case Code.POPFD:
		case Code.RETND:
		case Code.RETFW:
		case Code.POPD_FS:
		case Code.POPD_GS:
			return 4;
		case Code.POP_R64:
		case Code.POP_RM64:
		case Code.POPFQ:
		case Code.RETNQ:
		case Code.RETFD:
		case Code.POPQ_FS:
		case Code.POPQ_GS:
			return 8;
		case Code.POPAW:
		case Code.RETFQ:
			return 16;
		case Code.UIRET:
			return 24;
		case Code.POPAD:
			return 32;
		case Code.IRETQ:
			return 40;
		case Code.ERETU:
		case Code.ERETS:
			return 48;
		case Code.ENTERW_IMM16_IMM8:
			return -(2 + (getImmediate8_2nd() & 0x1F) * 2 + (getImmediate16() & 0xFFFF));
		case Code.ENTERD_IMM16_IMM8:
			return -(4 + (getImmediate8_2nd() & 0x1F) * 4 + (getImmediate16() & 0xFFFF));
		case Code.ENTERQ_IMM16_IMM8:
			return -(8 + (getImmediate8_2nd() & 0x1F) * 8 + (getImmediate16() & 0xFFFF));
		case Code.IRETW:
			return getCodeSize() == CodeSize.CODE64 ? 2 * 5 : 2 * 3;
		case Code.IRETD:
			return getCodeSize() == CodeSize.CODE64 ? 4 * 5 : 4 * 3;
		case Code.RETNW_IMM16:
			return 2 + (getImmediate16() & 0xFFFF);
		case Code.RETND_IMM16:
		case Code.RETFW_IMM16:
			return 4 + (getImmediate16() & 0xFFFF);
		case Code.RETNQ_IMM16:
		case Code.RETFD_IMM16:
			return 8 + (getImmediate16() & 0xFFFF);
		case Code.RETFQ_IMM16:
			return 16 + (getImmediate16() & 0xFFFF);
		// GENERATOR-END: StackPointerIncrementTable
		default:
			return 0;
		}
	}

	/**
	 * Gets the FPU status word's {@code TOP} increment value and whether it's a conditional or unconditional push/pop
	 * and whether {@code TOP} is written.
	 */
	public FpuStackIncrementInfo getFpuStackIncrementInfo() {
		switch (getCode()) {
		// GENERATOR-BEGIN: FpuStackIncrementInfoTable
		// ⚠️This was generated by GENERATOR!🦹‍♂️
		case Code.FLD_M32FP:
		case Code.FLD_STI:
		case Code.FLD1:
		case Code.FLDL2T:
		case Code.FLDL2E:
		case Code.FLDPI:
		case Code.FLDLG2:
		case Code.FLDLN2:
		case Code.FLDZ:
		case Code.FXTRACT:
		case Code.FDECSTP:
		case Code.FILD_M32INT:
		case Code.FLD_M80FP:
		case Code.FLD_M64FP:
		case Code.FILD_M16INT:
		case Code.FBLD_M80BCD:
		case Code.FILD_M64INT:
			return new FpuStackIncrementInfo(-1, false, true);
		case Code.FPTAN:
		case Code.FSINCOS:
			return new FpuStackIncrementInfo(-1, true, true);
		case Code.FLDENV_M14BYTE:
		case Code.FLDENV_M28BYTE:
		case Code.FNINIT:
		case Code.FINIT:
		case Code.FRSTOR_M94BYTE:
		case Code.FRSTOR_M108BYTE:
		case Code.FNSAVE_M94BYTE:
		case Code.FSAVE_M94BYTE:
		case Code.FNSAVE_M108BYTE:
		case Code.FSAVE_M108BYTE:
			return new FpuStackIncrementInfo(0, false, true);
		case Code.FCOMP_M32FP:
		case Code.FCOMP_ST0_STI:
		case Code.FSTP_M32FP:
		case Code.FSTPNCE_STI:
		case Code.FYL2X:
		case Code.FPATAN:
		case Code.FINCSTP:
		case Code.FYL2XP1:
		case Code.FICOMP_M32INT:
		case Code.FISTTP_M32INT:
		case Code.FISTP_M32INT:
		case Code.FSTP_M80FP:
		case Code.FCOMP_M64FP:
		case Code.FCOMP_ST0_STI_DCD8:
		case Code.FISTTP_M64INT:
		case Code.FSTP_M64FP:
		case Code.FSTP_STI:
		case Code.FUCOMP_ST0_STI:
		case Code.FICOMP_M16INT:
		case Code.FADDP_STI_ST0:
		case Code.FMULP_STI_ST0:
		case Code.FCOMP_ST0_STI_DED0:
		case Code.FSUBRP_STI_ST0:
		case Code.FSUBP_STI_ST0:
		case Code.FDIVRP_STI_ST0:
		case Code.FDIVP_STI_ST0:
		case Code.FISTTP_M16INT:
		case Code.FISTP_M16INT:
		case Code.FBSTP_M80BCD:
		case Code.FISTP_M64INT:
		case Code.FFREEP_STI:
		case Code.FSTP_STI_DFD0:
		case Code.FSTP_STI_DFD8:
		case Code.FUCOMIP_ST0_STI:
		case Code.FCOMIP_ST0_STI:
		case Code.FTSTP:
			return new FpuStackIncrementInfo(1, false, true);
		case Code.FUCOMPP:
		case Code.FCOMPP:
			return new FpuStackIncrementInfo(2, false, true);
		// GENERATOR-END: FpuStackIncrementInfoTable
		default:
			return new FpuStackIncrementInfo(0, false, false);
		}
	}

	/**
	 * Instruction encoding (an {@link EncodingKind} enum variant), eg.<!-- --> Legacy, 3DNow!, VEX, EVEX, XOP
	 */
	public int getEncoding() {
		return Code.encoding(getCode());
	}

	/**
	 * Gets the CPU or CPUID feature flags (an array of {@link CpuidFeature} enum variants)
	 */
	public int[] getCpuidFeatures() {
		return Code.cpuidFeatures(getCode());
	}

	/**
	 * Control flow info (a {@link FlowControl} enum variant)
	 */
	public int getFlowControl() {
		return Code.flowControl(getCode());
	}

	/**
	 * {@code true} if it's a privileged instruction (all CPL=0 instructions (except {@code VMCALL}) and IOPL instructions {@code IN},
	 * {@code INS}, {@code OUT}, {@code OUTS}, {@code CLI}, {@code STI})
	 */
	public boolean isPrivileged() {
		return Code.isPrivileged(getCode());
	}

	/**
	 * {@code true} if this is an instruction that implicitly uses the stack pointer ({@code SP}/{@code ESP}/{@code RSP}), eg.<!--
	 * --> {@code CALL}, {@code PUSH}, {@code POP}, {@code RET}, etc.
	 *
	 * @see #getStackPointerIncrement()
	 */
	public boolean isStackInstruction() {
		return Code.isStackInstruction(getCode());
	}

	/**
	 * {@code true} if it's an instruction that saves or restores too many registers (eg.<!-- --> {@code FXRSTOR}, {@code XSAVE}, etc).
	 */
	public boolean isSaveRestoreInstruction() {
		return Code.isSaveRestoreInstruction(getCode());
	}

	private int getRflagsInfo() {
		int flags1 = InstrInfoTable.data[getCode() << 1];
		int impliedAccess = (flags1 >>> InfoFlags1.IMPLIED_ACCESS_SHIFT) & InfoFlags1.IMPLIED_ACCESS_MASK;
		int result = (flags1 >>> InfoFlags1.RFLAGS_INFO_SHIFT) & InfoFlags1.RFLAGS_INFO_MASK;
		int e = impliedAccess - ImpliedAccess.SHIFT_IB_MASK1_FMOD9;
		switch (e) {
		case ImpliedAccess.SHIFT_IB_MASK1_FMOD9 - ImpliedAccess.SHIFT_IB_MASK1_FMOD9:
		case ImpliedAccess.SHIFT_IB_MASK1_FMOD11 - ImpliedAccess.SHIFT_IB_MASK1_FMOD9:
			int m = e == ImpliedAccess.SHIFT_IB_MASK1_FMOD9 - ImpliedAccess.SHIFT_IB_MASK1_FMOD9 ? 9 : 17;
			switch ((getImmediate8() & 0x1F) % m) {
			case 0:
				return RflagsInfo.NONE;
			case 1:
				return RflagsInfo.R_C_W_CO;
			}
			break;

		case ImpliedAccess.SHIFT_IB_MASK1_F - ImpliedAccess.SHIFT_IB_MASK1_FMOD9:
		case ImpliedAccess.SHIFT_IB_MASK3_F - ImpliedAccess.SHIFT_IB_MASK1_FMOD9:
			int mask = e == ImpliedAccess.SHIFT_IB_MASK1_F - ImpliedAccess.SHIFT_IB_MASK1_FMOD9 ? 0x1F : 0x3F;
			switch (getImmediate8() & mask) {
			case 0:
				return RflagsInfo.NONE;
			case 1:
				if (result == RflagsInfo.W_C_U_O)
					return RflagsInfo.W_CO;
				else if (result == RflagsInfo.R_C_W_C_U_O)
					return RflagsInfo.R_C_W_CO;
				else {
					assert result == RflagsInfo.W_CPSZ_U_AO : result;
					return RflagsInfo.W_COPSZ_U_A;
				}
			}
			break;

		case ImpliedAccess.CLEAR_RFLAGS - ImpliedAccess.SHIFT_IB_MASK1_FMOD9:
			if (getOp0Register() != getOp1Register())
				break;
			if (getOp0Kind() != OpKind.REGISTER || getOp1Kind() != OpKind.REGISTER)
				break;
			if (getMnemonic() == Mnemonic.XOR)
				return RflagsInfo.C_COS_S_PZ_U_A;
			else
				return RflagsInfo.C_ACOS_S_PZ;
		}
		return result;
	}

	/**
	 * All flags (an {@link RflagsBits} flags value) that are read by the CPU when executing the instruction
	 *
	 * @see #getRflagsModified()
	 */
	public int getRflagsRead() {
		// If the method call is used without a temp index, the jitter generates worse code.
		// It stores the array in a temp local, then it calls the method, and then it reads
		// the temp local and checks if we can read the array.
		int index = getRflagsInfo();
		return RflagsInfoConstants.flagsRead[index];
	}

	/**
	 * All flags (an {@link RflagsBits} flags value) that are written by the CPU, except those flags that are known to be undefined, always set or
	 * always cleared
	 *
	 * @see #getRflagsModified()
	 */
	public int getRflagsWritten() {
		// See RflagsRead for the reason why a temp index is used here
		int index = getRflagsInfo();
		return RflagsInfoConstants.flagsWritten[index];
	}

	/**
	 * All flags (an {@link RflagsBits} flags value) that are always cleared by the CPU
	 *
	 * @see #getRflagsModified()
	 */
	public int getRflagsCleared() {
		// See RflagsRead for the reason why a temp index is used here
		int index = getRflagsInfo();
		return RflagsInfoConstants.flagsCleared[index];
	}

	/**
	 * All flags (an {@link RflagsBits} flags value) that are always set by the CPU
	 *
	 * @see #getRflagsModified()
	 */
	public int getRflagsSet() {
		// See RflagsRead for the reason why a temp index is used here
		int index = getRflagsInfo();
		return RflagsInfoConstants.flagsSet[index];
	}

	/**
	 * All flags (an {@link RflagsBits} flags value) that are undefined after executing the instruction
	 *
	 * @see #getRflagsModified()
	 */
	public int getRflagsUndefined() {
		// See RflagsRead for the reason why a temp index is used here
		int index = getRflagsInfo();
		return RflagsInfoConstants.flagsUndefined[index];
	}

	/**
	 * All flags (an {@link RflagsBits} flags value) that are modified by the CPU.
	 * <p>
	 * This is {@link #getRflagsWritten()} + {@link #getRflagsCleared()} + {@link #getRflagsSet()} + {@link #getRflagsUndefined()}
	 */
	public int getRflagsModified() {
		// See RflagsRead for the reason why a temp index is used here
		int index = getRflagsInfo();
		return RflagsInfoConstants.flagsModified[index];
	}

	/**
	 * Checks if it's a {@code Jcc SHORT} or {@code Jcc NEAR} instruction
	 */
	public boolean isJccShortOrNear() {
		return Code.isJccShortOrNear(getCode());
	}

	/**
	 * Checks if it's a {@code Jcc NEAR} instruction
	 */
	public boolean isJccNear() {
		return Code.isJccNear(getCode());
	}

	/**
	 * Checks if it's a {@code Jcc SHORT} instruction
	 */
	public boolean isJccShort() {
		return Code.isJccShort(getCode());
	}

	/**
	 * Checks if it's a {@code JMP SHORT} instruction
	 */
	public boolean isJmpShort() {
		return Code.isJmpShort(getCode());
	}

	/**
	 * Checks if it's a {@code JMP NEAR} instruction
	 */
	public boolean isJmpNear() {
		return Code.isJmpNear(getCode());
	}

	/**
	 * Checks if it's a {@code JMP SHORT} or a {@code JMP NEAR} instruction
	 */
	public boolean isJmpShortOrNear() {
		return Code.isJmpShortOrNear(getCode());
	}

	/**
	 * Checks if it's a {@code JMP FAR} instruction
	 */
	public boolean isJmpFar() {
		return Code.isJmpFar(getCode());
	}

	/**
	 * Checks if it's a {@code CALL NEAR} instruction
	 */
	public boolean isCallNear() {
		return Code.isCallNear(getCode());
	}

	/**
	 * Checks if it's a {@code CALL FAR} instruction
	 */
	public boolean isCallFar() {
		return Code.isCallFar(getCode());
	}

	/**
	 * Checks if it's a {@code JMP NEAR reg/[mem]} instruction
	 */
	public boolean isJmpNearIndirect() {
		return Code.isJmpNearIndirect(getCode());
	}

	/**
	 * Checks if it's a {@code JMP FAR [mem]} instruction
	 */
	public boolean isJmpFarIndirect() {
		return Code.isJmpFarIndirect(getCode());
	}

	/**
	 * Checks if it's a {@code CALL NEAR reg/[mem]} instruction
	 */
	public boolean isCallNearIndirect() {
		return Code.isCallNearIndirect(getCode());
	}

	/**
	 * Checks if it's a {@code CALL FAR [mem]} instruction
	 */
	public boolean isCallFarIndirect() {
		return Code.isCallFarIndirect(getCode());
	}

	/**
	 * Checks if it's a {@code JKccD SHORT} or {@code JKccD NEAR} instruction
	 */
	public boolean isJkccShortOrNear() {
		return Code.isJkccShortOrNear(getCode());
	}

	/**
	 * Checks if it's a {@code JKccD NEAR} instruction
	 */
	public boolean isJkccNear() {
		return Code.isJkccNear(getCode());
	}

	/**
	 * Checks if it's a {@code JKccD SHORT} instruction
	 */
	public boolean isJkccShort() {
		return Code.isJkccShort(getCode());
	}

	/**
	 * Checks if it's a {@code JCXZ SHORT}, {@code JECXZ SHORT} or {@code JRCXZ SHORT} instruction
	 */
	public boolean isJcxShort() {
		return Code.isJcxShort(getCode());
	}

	/**
	 * Checks if it's a {@code LOOPcc SHORT} instruction
	 */
	public boolean isLoopcc() {
		return Code.isLoopcc(getCode());
	}

	/**
	 * Checks if it's a {@code LOOP SHORT} instruction
	 */
	public boolean isLoop() {
		return Code.isLoop(getCode());
	}

	/**
	 * Negates the condition code, eg.<!-- --> {@code JE} -&gt; {@code JNE}. Can be used if it's {@code Jcc}, {@code SETcc},
	 * {@code CMOVcc}, {@code CMPccXADD}, {@code LOOPcc}
	 * and does nothing if the instruction doesn't have a condition code.
	 */
	public void negateConditionCode() {
		setCode(Code.negateConditionCode(getCode()));
	}

	/**
	 * Converts {@code Jcc/JMP NEAR} to {@code Jcc/JMP SHORT} and does nothing if it's not a {@code Jcc/JMP NEAR} instruction
	 */
	public void toShortBranch() {
		setCode(Code.toShortBranch(getCode()));
	}

	/**
	 * Converts {@code Jcc/JMP SHORT} to {@code Jcc/JMP NEAR} and does nothing if it's not a {@code Jcc/JMP SHORT} instruction
	 */
	public void toNearBranch() {
		setCode(Code.toNearBranch(getCode()));
	}

	/**
	 * Gets the condition code (a {@link ConditionCode} enum variant) if it's {@code Jcc}, {@code SETcc}, {@code CMOVcc}, {@code CMPccXADD},
	 * {@code LOOPcc} else {@link ConditionCode#NONE} is returned
	 */
	public int getConditionCode() {
		return Code.conditionCode(getCode());
	}

	/**
	 * Checks if it's a string instruction such as {@code MOVS}, {@code LODS}, {@code STOS}, etc.
	 */
	public boolean isStringInstruction() {
		return Code.isStringInstruction(getCode());
	}

	/**
	 * Gets the virtual address of a memory operand
	 *
	 * @param operand          Operand number, must be a memory operand
	 * @param elementIndex     Only used if it's a vsib memory operand. This is the element index of the vector index register.
	 * @param getRegisterValue Returns values of registers and segment base addresses
	 * @return {@code null} if it failed to read all registers, else the calculated virtual address
	 */
	@SuppressWarnings("deprecation")
	public Long getVirtualAddress(int operand, int elementIndex, VAGetRegisterValue getRegisterValue) {
		if (getRegisterValue == null)
			throw new NullPointerException("getRegisterValue");
		Long seg, base;
		switch (getOpKind(operand)) {
		case OpKind.REGISTER:
		case OpKind.NEAR_BRANCH16:
		case OpKind.NEAR_BRANCH32:
		case OpKind.NEAR_BRANCH64:
		case OpKind.FAR_BRANCH16:
		case OpKind.FAR_BRANCH32:
		case OpKind.IMMEDIATE8:
		case OpKind.IMMEDIATE8_2ND:
		case OpKind.IMMEDIATE16:
		case OpKind.IMMEDIATE32:
		case OpKind.IMMEDIATE64:
		case OpKind.IMMEDIATE8TO16:
		case OpKind.IMMEDIATE8TO32:
		case OpKind.IMMEDIATE8TO64:
		case OpKind.IMMEDIATE32TO64:
			return 0L;

		case OpKind.MEMORY_SEG_SI:
			if ((seg = getRegisterValue.get(getMemorySegment(), 0, 0)) != null &&
				(base = getRegisterValue.get(Register.SI, 0, 0)) != null) {
				return seg.longValue() + (base.longValue() & 0xFFFF);
			}
			break;

		case OpKind.MEMORY_SEG_ESI:
			if ((seg = getRegisterValue.get(getMemorySegment(), 0, 0)) != null &&
				(base = getRegisterValue.get(Register.ESI, 0, 0)) != null) {
				return seg.longValue() + (base.longValue() & 0xFFFF_FFFFL);
			}
			break;

		case OpKind.MEMORY_SEG_RSI:
			if ((seg = getRegisterValue.get(getMemorySegment(), 0, 0)) != null &&
				(base = getRegisterValue.get(Register.RSI, 0, 0)) != null) {
				return seg.longValue() + base.longValue();
			}
			break;

		case OpKind.MEMORY_SEG_DI:
			if ((seg = getRegisterValue.get(getMemorySegment(), 0, 0)) != null &&
				(base = getRegisterValue.get(Register.DI, 0, 0)) != null) {
				return seg.longValue() + (base.longValue() & 0xFFFF);
			}
			break;

		case OpKind.MEMORY_SEG_EDI:
			if ((seg = getRegisterValue.get(getMemorySegment(), 0, 0)) != null &&
				(base = getRegisterValue.get(Register.EDI, 0, 0)) != null) {
				return seg.longValue() + (base.longValue() & 0xFFFF_FFFFL);
			}
			break;

		case OpKind.MEMORY_SEG_RDI:
			if ((seg = getRegisterValue.get(getMemorySegment(), 0, 0)) != null &&
				(base = getRegisterValue.get(Register.RDI, 0, 0)) != null) {
				return seg.longValue() + base.longValue();
			}
			break;

		case OpKind.MEMORY_ESDI:
			if ((seg = getRegisterValue.get(Register.ES, 0, 0)) != null &&
				(base = getRegisterValue.get(Register.DI, 0, 0)) != null) {
				return seg.longValue() + (base.longValue() & 0xFFFF);
			}
			break;

		case OpKind.MEMORY_ESEDI:
			if ((seg = getRegisterValue.get(Register.ES, 0, 0)) != null &&
				(base = getRegisterValue.get(Register.EDI, 0, 0)) != null) {
				return seg.longValue() + (base.longValue() & 0xFFFF_FFFFL);
			}
			break;

		case OpKind.MEMORY_ESRDI:
			if ((seg = getRegisterValue.get(Register.ES, 0, 0)) != null &&
				(base = getRegisterValue.get(Register.RDI, 0, 0)) != null) {
				return seg.longValue() + base.longValue();
			}
			break;

		case OpKind.MEMORY:
			int baseReg = getMemoryBase();
			int indexReg = getMemoryIndex();
			int addrSize = com.github.icedland.iced.x86.InternalInstructionUtils.getAddressSizeInBytes(baseReg, indexReg, getMemoryDisplSize(), getCodeSize());
			long offset = getMemoryDisplacement64();
			long offsetMask;
			if (addrSize == 8)
				offsetMask = 0xFFFF_FFFF_FFFF_FFFFL;
			else if (addrSize == 4)
				offsetMask = 0xFFFF_FFFFL;
			else {
				assert addrSize == 2 : addrSize;
				offsetMask = 0xFFFF;
			}
			if (baseReg != Register.NONE && baseReg != Register.RIP && baseReg != Register.EIP) {
				if ((base = getRegisterValue.get(baseReg, 0, 0)) == null)
					break;
				offset += base;
			}
			int code = getCode();
			if (indexReg != Register.NONE && !Code.ignoresIndex(code) && !Code.isTileStrideIndex(code)) {
				int vsib = getVsib();
				if ((vsib & VsibFlags.VSIB) != 0) {
					long base2;
					if ((vsib & VsibFlags.VSIB64) != 0) {
						if ((base = getRegisterValue.get(indexReg, elementIndex, 8)) == null)
							break;
						base2 = base.longValue();
					}
					else {
						if ((base = getRegisterValue.get(indexReg, elementIndex, 4)) == null)
							break;
						base2 = (int)base.longValue();
					}
					offset += base2 << getRawMemoryIndexScale();
				}
				else {
					if ((base = getRegisterValue.get(indexReg, 0, 0)) == null)
						break;
					offset += base.longValue() << getRawMemoryIndexScale();
				}
			}
			if (code >= Code.MVEX_VLOADUNPACKHD_ZMM_K1_MT && code <= Code.MVEX_VPACKSTOREHPD_MT_K1_ZMM)
				offset -= 0x40;
			offset &= offsetMask;
			if (!Code.ignoresSegment(code)) {
				if ((seg = getRegisterValue.get(getMemorySegment(), 0, 0)) == null)
					break;
				offset += seg.longValue();
			}
			return offset;

		default:
			throw new UnsupportedOperationException();
		}

		return null;
	}

	private static byte toByte(int value) {
		if (-0x80 <= value && value <= 0xFF)
			return (byte)value;
		throw new IllegalArgumentException("Invalid byte value, must fit in 8 bits: -0x80 .. 0xFF");
	}

	private static short toShort(int value) {
		if (-0x8000 <= value && value <= 0xFFFF)
			return (short)value;
		throw new IllegalArgumentException("Invalid short value, must fit in 16 bits: -0x8000 .. 0xFFFF");
	}

	private static void initializeSignedImmediate(Instruction instruction, int operand, long immediate) {
		int opKind = getImmediateOpKind(instruction.getCode(), operand);
		instruction.setOpKind(operand, opKind);

		switch (opKind) {
		case OpKind.IMMEDIATE8:
			// All byte and all ubyte values can be used
			if (!(-0x80 <= immediate && immediate <= 0xFF))
				throw new IllegalArgumentException("immediate");
			instruction.setImmediate8((byte)immediate);
			break;

		case OpKind.IMMEDIATE8_2ND:
			// All byte and all ubyte values can be used
			if (!(-0x80 <= immediate && immediate <= 0xFF))
				throw new IllegalArgumentException("immediate");
			instruction.setImmediate8_2nd((byte)immediate);
			break;

		case OpKind.IMMEDIATE8TO16:
			if (!(-0x80 <= immediate && immediate <= 0x7F) && !(0xFF80 <= immediate && immediate <= 0xFFFF))
				throw new IllegalArgumentException("immediate");
			instruction.setImmediate8((byte)immediate);
			break;

		case OpKind.IMMEDIATE8TO32:
			if (!(-0x80 <= immediate && immediate <= 0x7F) && !(0xFFFF_FF80L <= immediate && immediate <= 0xFFFF_FFFFL))
				throw new IllegalArgumentException("immediate");
			instruction.setImmediate8((byte)immediate);
			break;

		case OpKind.IMMEDIATE8TO64:
			if (!(-0x80 <= immediate && immediate <= 0x7F))
				throw new IllegalArgumentException("immediate");
			instruction.setImmediate8((byte)immediate);
			break;

		case OpKind.IMMEDIATE16:
			// All short and all ushort values can be used
			if (!(-0x8000 <= immediate && immediate <= 0xFFFF))
				throw new IllegalArgumentException("immediate");
			instruction.setImmediate16((short)immediate);
			break;

		case OpKind.IMMEDIATE32:
			// All int and all uint values can be used
			if (!(-0x8000_0000 <= immediate && immediate <= 0xFFFF_FFFFL))
				throw new IllegalArgumentException("immediate");
			instruction.setImmediate32((int)immediate);
			break;

		case OpKind.IMMEDIATE32TO64:
			if (!(-0x8000_0000 <= immediate && immediate <= 0x7FFF_FFFF))
				throw new IllegalArgumentException("immediate");
			instruction.setImmediate32((int)immediate);
			break;

		case OpKind.IMMEDIATE64:
			instruction.setImmediate64(immediate);
			break;

		default:
			throw new IllegalArgumentException("instruction");
		}
	}

	private static int getImmediateOpKind(int code, int operand) {
		OpCodeHandler[] handlers = InternalEncoderOpCodeHandlers.handlers;
		if (Integer.compareUnsigned(code, handlers.length) >= 0)
			throw new IllegalArgumentException("code");
		@SuppressWarnings("deprecation")
		Op[] operands = handlers[code].operands;
		if (Integer.compareUnsigned(operand, operands.length) >= 0)
			throw new IllegalArgumentException(String.format("Code %d doesn't have at least %d operands", code, operand + 1));
		@SuppressWarnings("deprecation")
		int opKind = operands[operand].getImmediateOpKind();
		if (opKind == OpKind.IMMEDIATE8 &&
				operand > 0 &&
				operand + 1 == operands.length) {
			@SuppressWarnings("deprecation")
			int opKindPrev = operands[operand - 1].getImmediateOpKind();
			if (opKindPrev == OpKind.IMMEDIATE8 || opKindPrev == OpKind.IMMEDIATE16)
				opKind = OpKind.IMMEDIATE8_2ND;
		}
		if (opKind == -1)
			throw new IllegalArgumentException(String.format("Code %d's op%d isn't an immediate operand", code, operand));
		return opKind;
	}

	private static int getNearBranchOpKind(int code, int operand) {
		OpCodeHandler[] handlers = InternalEncoderOpCodeHandlers.handlers;
		if (Integer.compareUnsigned(code, handlers.length) >= 0)
			throw new IllegalArgumentException("code");
		@SuppressWarnings("deprecation")
		Op[] operands = handlers[code].operands;
		if (Integer.compareUnsigned(operand, operands.length) >= 0)
			throw new IllegalArgumentException(String.format("Code %d doesn't have at least %d operands", code, operand + 1));
		@SuppressWarnings("deprecation")
		int opKind = operands[operand].getNearBranchOpKind();
		if (opKind == -1)
			throw new IllegalArgumentException(String.format("Code %d's op%d isn't a near branch operand", code, operand));
		return opKind;
	}

	private static int getFarBranchOpKind(int code, int operand) {
		OpCodeHandler[] handlers = InternalEncoderOpCodeHandlers.handlers;
		if (Integer.compareUnsigned(code, handlers.length) >= 0)
			throw new IllegalArgumentException("code");
		@SuppressWarnings("deprecation")
		Op[] operands = handlers[code].operands;
		if (Integer.compareUnsigned(operand, operands.length) >= 0)
			throw new IllegalArgumentException(String.format("Code %d doesn't have at least %d operands", code, operand + 1));
		@SuppressWarnings("deprecation")
		int opKind = operands[operand].getFarBranchOpKind();
		if (opKind == -1)
			throw new IllegalArgumentException(String.format("Code %d's op%d isn't a far branch operand", code, operand));
		return opKind;
	}

	private static Instruction createString_Reg_SegRSI(int code, int addressSize, int register, int segmentPrefix, int repPrefix) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		if (repPrefix == RepPrefixKind.REPE)
			instruction.setRepePrefix(true);
		else if (repPrefix == RepPrefixKind.REPNE)
			instruction.setRepnePrefix(true);
		else
			assert repPrefix == RepPrefixKind.NONE : repPrefix;

		instruction.setOp0Register(register);

		if (addressSize == 64)
			instruction.setOp1Kind(OpKind.MEMORY_SEG_RSI);
		else if (addressSize == 32)
			instruction.setOp1Kind(OpKind.MEMORY_SEG_ESI);
		else if (addressSize == 16)
			instruction.setOp1Kind(OpKind.MEMORY_SEG_SI);
		else
			throw new IllegalArgumentException("addressSize");

		instruction.setSegmentPrefix(segmentPrefix);

		assert instruction.getOpCount() == 2 : instruction.getOpCount();
		return instruction;
	}

	private static Instruction createString_Reg_ESRDI(int code, int addressSize, int register, int repPrefix) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		if (repPrefix == RepPrefixKind.REPE)
			instruction.setRepePrefix(true);
		else if (repPrefix == RepPrefixKind.REPNE)
			instruction.setRepnePrefix(true);
		else
			assert repPrefix == RepPrefixKind.NONE : repPrefix;

		instruction.setOp0Register(register);

		if (addressSize == 64)
			instruction.setOp1Kind(OpKind.MEMORY_ESRDI);
		else if (addressSize == 32)
			instruction.setOp1Kind(OpKind.MEMORY_ESEDI);
		else if (addressSize == 16)
			instruction.setOp1Kind(OpKind.MEMORY_ESDI);
		else
			throw new IllegalArgumentException("addressSize");

		assert instruction.getOpCount() == 2 : instruction.getOpCount();
		return instruction;
	}

	private static Instruction createString_ESRDI_Reg(int code, int addressSize, int register, int repPrefix) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		if (repPrefix == RepPrefixKind.REPE)
			instruction.setRepePrefix(true);
		else if (repPrefix == RepPrefixKind.REPNE)
			instruction.setRepnePrefix(true);
		else
			assert repPrefix == RepPrefixKind.NONE : repPrefix;

		if (addressSize == 64)
			instruction.setOp0Kind(OpKind.MEMORY_ESRDI);
		else if (addressSize == 32)
			instruction.setOp0Kind(OpKind.MEMORY_ESEDI);
		else if (addressSize == 16)
			instruction.setOp0Kind(OpKind.MEMORY_ESDI);
		else
			throw new IllegalArgumentException("addressSize");

		instruction.setOp1Register(register);

		assert instruction.getOpCount() == 2 : instruction.getOpCount();
		return instruction;
	}

	private static Instruction createString_SegRSI_ESRDI(int code, int addressSize, int segmentPrefix, int repPrefix) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		if (repPrefix == RepPrefixKind.REPE)
			instruction.setRepePrefix(true);
		else if (repPrefix == RepPrefixKind.REPNE)
			instruction.setRepnePrefix(true);
		else
			assert repPrefix == RepPrefixKind.NONE : repPrefix;

		if (addressSize == 64) {
			instruction.setOp0Kind(OpKind.MEMORY_SEG_RSI);
			instruction.setOp1Kind(OpKind.MEMORY_ESRDI);
		}
		else if (addressSize == 32) {
			instruction.setOp0Kind(OpKind.MEMORY_SEG_ESI);
			instruction.setOp1Kind(OpKind.MEMORY_ESEDI);
		}
		else if (addressSize == 16) {
			instruction.setOp0Kind(OpKind.MEMORY_SEG_SI);
			instruction.setOp1Kind(OpKind.MEMORY_ESDI);
		}
		else
			throw new IllegalArgumentException("addressSize");

		instruction.setSegmentPrefix(segmentPrefix);

		assert instruction.getOpCount() == 2 : instruction.getOpCount();
		return instruction;
	}

	private static Instruction createString_ESRDI_SegRSI(int code, int addressSize, int segmentPrefix, int repPrefix) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		if (repPrefix == RepPrefixKind.REPE)
			instruction.setRepePrefix(true);
		else if (repPrefix == RepPrefixKind.REPNE)
			instruction.setRepnePrefix(true);
		else
			assert repPrefix == RepPrefixKind.NONE : repPrefix;

		if (addressSize == 64) {
			instruction.setOp0Kind(OpKind.MEMORY_ESRDI);
			instruction.setOp1Kind(OpKind.MEMORY_SEG_RSI);
		}
		else if (addressSize == 32) {
			instruction.setOp0Kind(OpKind.MEMORY_ESEDI);
			instruction.setOp1Kind(OpKind.MEMORY_SEG_ESI);
		}
		else if (addressSize == 16) {
			instruction.setOp0Kind(OpKind.MEMORY_ESDI);
			instruction.setOp1Kind(OpKind.MEMORY_SEG_SI);
		}
		else
			throw new IllegalArgumentException("addressSize");

		instruction.setSegmentPrefix(segmentPrefix);

		assert instruction.getOpCount() == 2 : instruction.getOpCount();
		return instruction;
	}

	private static Instruction createMaskmov(int code, int addressSize, int register1, int register2, int segmentPrefix) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		if (addressSize == 64)
			instruction.setOp0Kind(OpKind.MEMORY_SEG_RDI);
		else if (addressSize == 32)
			instruction.setOp0Kind(OpKind.MEMORY_SEG_EDI);
		else if (addressSize == 16)
			instruction.setOp0Kind(OpKind.MEMORY_SEG_DI);
		else
			throw new IllegalArgumentException("addressSize");

		instruction.setOp1Register(register1);
		instruction.setOp2Register(register2);
		instruction.setSegmentPrefix(segmentPrefix);

		assert instruction.getOpCount() == 3 : instruction.getOpCount();
		return instruction;
	}

	private static void initMemoryOperand(Instruction instruction, MemoryOperand memory) {
		instruction.setMemoryBase(memory.base.get());
		instruction.setMemoryIndex(memory.index.get());
		instruction.setMemoryIndexScale(memory.scale);
		instruction.setMemoryDisplSize(memory.displSize);
		instruction.setMemoryDisplacement64(memory.displacement);
		instruction.setBroadcast(memory.isBroadcast);
		instruction.setSegmentPrefix(memory.segmentPrefix.get());
	}

	// GENERATOR-BEGIN: Create
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	/**
	 * Creates an instruction with no operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 */
	public static Instruction create(int code) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 1 operand
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param register op0: Register (see {@link ICRegisters})
	 */
	public static Instruction create(int code, ICRegister register) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Register(register.get());

		assert instruction.getOpCount() == 1 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 1 operand
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param immediate op0: Immediate value
	 */
	public static Instruction create(int code, int immediate) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		initializeSignedImmediate(instruction, 0, immediate);

		assert instruction.getOpCount() == 1 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 1 operand
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param memory op0: Memory operand
	 */
	public static Instruction create(int code, MemoryOperand memory) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Kind(OpKind.MEMORY);
		initMemoryOperand(instruction, memory);

		assert instruction.getOpCount() == 1 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 2 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param register1 op0: Register (see {@link ICRegisters})
	 * @param register2 op1: Register (see {@link ICRegisters})
	 */
	public static Instruction create(int code, ICRegister register1, ICRegister register2) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Register(register1.get());

		instruction.setOp1Register(register2.get());

		assert instruction.getOpCount() == 2 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 2 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param register op0: Register (see {@link ICRegisters})
	 * @param immediate op1: Immediate value
	 */
	public static Instruction create(int code, ICRegister register, int immediate) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Register(register.get());

		initializeSignedImmediate(instruction, 1, immediate);

		assert instruction.getOpCount() == 2 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 2 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param register op0: Register (see {@link ICRegisters})
	 * @param immediate op1: Immediate value
	 */
	public static Instruction create(int code, ICRegister register, long immediate) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Register(register.get());

		initializeSignedImmediate(instruction, 1, immediate);

		assert instruction.getOpCount() == 2 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 2 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param register op0: Register (see {@link ICRegisters})
	 * @param memory op1: Memory operand
	 */
	public static Instruction create(int code, ICRegister register, MemoryOperand memory) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Register(register.get());

		instruction.setOp1Kind(OpKind.MEMORY);
		initMemoryOperand(instruction, memory);

		assert instruction.getOpCount() == 2 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 2 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param immediate op0: Immediate value
	 * @param register op1: Register (see {@link ICRegisters})
	 */
	public static Instruction create(int code, int immediate, ICRegister register) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		initializeSignedImmediate(instruction, 0, immediate);

		instruction.setOp1Register(register.get());

		assert instruction.getOpCount() == 2 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 2 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param immediate1 op0: Immediate value
	 * @param immediate2 op1: Immediate value
	 */
	public static Instruction create(int code, int immediate1, int immediate2) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		initializeSignedImmediate(instruction, 0, immediate1);

		initializeSignedImmediate(instruction, 1, immediate2);

		assert instruction.getOpCount() == 2 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 2 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param memory op0: Memory operand
	 * @param register op1: Register (see {@link ICRegisters})
	 */
	public static Instruction create(int code, MemoryOperand memory, ICRegister register) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Kind(OpKind.MEMORY);
		initMemoryOperand(instruction, memory);

		instruction.setOp1Register(register.get());

		assert instruction.getOpCount() == 2 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 2 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param memory op0: Memory operand
	 * @param immediate op1: Immediate value
	 */
	public static Instruction create(int code, MemoryOperand memory, int immediate) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Kind(OpKind.MEMORY);
		initMemoryOperand(instruction, memory);

		initializeSignedImmediate(instruction, 1, immediate);

		assert instruction.getOpCount() == 2 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 3 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param register1 op0: Register (see {@link ICRegisters})
	 * @param register2 op1: Register (see {@link ICRegisters})
	 * @param register3 op2: Register (see {@link ICRegisters})
	 */
	public static Instruction create(int code, ICRegister register1, ICRegister register2, ICRegister register3) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Register(register1.get());

		instruction.setOp1Register(register2.get());

		instruction.setOp2Register(register3.get());

		assert instruction.getOpCount() == 3 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 3 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param register1 op0: Register (see {@link ICRegisters})
	 * @param register2 op1: Register (see {@link ICRegisters})
	 * @param immediate op2: Immediate value
	 */
	public static Instruction create(int code, ICRegister register1, ICRegister register2, int immediate) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Register(register1.get());

		instruction.setOp1Register(register2.get());

		initializeSignedImmediate(instruction, 2, immediate);

		assert instruction.getOpCount() == 3 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 3 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param register1 op0: Register (see {@link ICRegisters})
	 * @param register2 op1: Register (see {@link ICRegisters})
	 * @param memory op2: Memory operand
	 */
	public static Instruction create(int code, ICRegister register1, ICRegister register2, MemoryOperand memory) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Register(register1.get());

		instruction.setOp1Register(register2.get());

		instruction.setOp2Kind(OpKind.MEMORY);
		initMemoryOperand(instruction, memory);

		assert instruction.getOpCount() == 3 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 3 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param register op0: Register (see {@link ICRegisters})
	 * @param immediate1 op1: Immediate value
	 * @param immediate2 op2: Immediate value
	 */
	public static Instruction create(int code, ICRegister register, int immediate1, int immediate2) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Register(register.get());

		initializeSignedImmediate(instruction, 1, immediate1);

		initializeSignedImmediate(instruction, 2, immediate2);

		assert instruction.getOpCount() == 3 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 3 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param register1 op0: Register (see {@link ICRegisters})
	 * @param memory op1: Memory operand
	 * @param register2 op2: Register (see {@link ICRegisters})
	 */
	public static Instruction create(int code, ICRegister register1, MemoryOperand memory, ICRegister register2) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Register(register1.get());

		instruction.setOp1Kind(OpKind.MEMORY);
		initMemoryOperand(instruction, memory);

		instruction.setOp2Register(register2.get());

		assert instruction.getOpCount() == 3 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 3 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param register op0: Register (see {@link ICRegisters})
	 * @param memory op1: Memory operand
	 * @param immediate op2: Immediate value
	 */
	public static Instruction create(int code, ICRegister register, MemoryOperand memory, int immediate) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Register(register.get());

		instruction.setOp1Kind(OpKind.MEMORY);
		initMemoryOperand(instruction, memory);

		initializeSignedImmediate(instruction, 2, immediate);

		assert instruction.getOpCount() == 3 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 3 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param memory op0: Memory operand
	 * @param register1 op1: Register (see {@link ICRegisters})
	 * @param register2 op2: Register (see {@link ICRegisters})
	 */
	public static Instruction create(int code, MemoryOperand memory, ICRegister register1, ICRegister register2) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Kind(OpKind.MEMORY);
		initMemoryOperand(instruction, memory);

		instruction.setOp1Register(register1.get());

		instruction.setOp2Register(register2.get());

		assert instruction.getOpCount() == 3 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 3 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param memory op0: Memory operand
	 * @param register op1: Register (see {@link ICRegisters})
	 * @param immediate op2: Immediate value
	 */
	public static Instruction create(int code, MemoryOperand memory, ICRegister register, int immediate) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Kind(OpKind.MEMORY);
		initMemoryOperand(instruction, memory);

		instruction.setOp1Register(register.get());

		initializeSignedImmediate(instruction, 2, immediate);

		assert instruction.getOpCount() == 3 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 4 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param register1 op0: Register (see {@link ICRegisters})
	 * @param register2 op1: Register (see {@link ICRegisters})
	 * @param register3 op2: Register (see {@link ICRegisters})
	 * @param register4 op3: Register (see {@link ICRegisters})
	 */
	public static Instruction create(int code, ICRegister register1, ICRegister register2, ICRegister register3, ICRegister register4) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Register(register1.get());

		instruction.setOp1Register(register2.get());

		instruction.setOp2Register(register3.get());

		instruction.setOp3Register(register4.get());

		assert instruction.getOpCount() == 4 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 4 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param register1 op0: Register (see {@link ICRegisters})
	 * @param register2 op1: Register (see {@link ICRegisters})
	 * @param register3 op2: Register (see {@link ICRegisters})
	 * @param immediate op3: Immediate value
	 */
	public static Instruction create(int code, ICRegister register1, ICRegister register2, ICRegister register3, int immediate) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Register(register1.get());

		instruction.setOp1Register(register2.get());

		instruction.setOp2Register(register3.get());

		initializeSignedImmediate(instruction, 3, immediate);

		assert instruction.getOpCount() == 4 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 4 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param register1 op0: Register (see {@link ICRegisters})
	 * @param register2 op1: Register (see {@link ICRegisters})
	 * @param register3 op2: Register (see {@link ICRegisters})
	 * @param memory op3: Memory operand
	 */
	public static Instruction create(int code, ICRegister register1, ICRegister register2, ICRegister register3, MemoryOperand memory) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Register(register1.get());

		instruction.setOp1Register(register2.get());

		instruction.setOp2Register(register3.get());

		instruction.setOp3Kind(OpKind.MEMORY);
		initMemoryOperand(instruction, memory);

		assert instruction.getOpCount() == 4 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 4 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param register1 op0: Register (see {@link ICRegisters})
	 * @param register2 op1: Register (see {@link ICRegisters})
	 * @param immediate1 op2: Immediate value
	 * @param immediate2 op3: Immediate value
	 */
	public static Instruction create(int code, ICRegister register1, ICRegister register2, int immediate1, int immediate2) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Register(register1.get());

		instruction.setOp1Register(register2.get());

		initializeSignedImmediate(instruction, 2, immediate1);

		initializeSignedImmediate(instruction, 3, immediate2);

		assert instruction.getOpCount() == 4 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 4 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param register1 op0: Register (see {@link ICRegisters})
	 * @param register2 op1: Register (see {@link ICRegisters})
	 * @param memory op2: Memory operand
	 * @param register3 op3: Register (see {@link ICRegisters})
	 */
	public static Instruction create(int code, ICRegister register1, ICRegister register2, MemoryOperand memory, ICRegister register3) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Register(register1.get());

		instruction.setOp1Register(register2.get());

		instruction.setOp2Kind(OpKind.MEMORY);
		initMemoryOperand(instruction, memory);

		instruction.setOp3Register(register3.get());

		assert instruction.getOpCount() == 4 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 4 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param register1 op0: Register (see {@link ICRegisters})
	 * @param register2 op1: Register (see {@link ICRegisters})
	 * @param memory op2: Memory operand
	 * @param immediate op3: Immediate value
	 */
	public static Instruction create(int code, ICRegister register1, ICRegister register2, MemoryOperand memory, int immediate) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Register(register1.get());

		instruction.setOp1Register(register2.get());

		instruction.setOp2Kind(OpKind.MEMORY);
		initMemoryOperand(instruction, memory);

		initializeSignedImmediate(instruction, 3, immediate);

		assert instruction.getOpCount() == 4 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 5 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param register1 op0: Register (see {@link ICRegisters})
	 * @param register2 op1: Register (see {@link ICRegisters})
	 * @param register3 op2: Register (see {@link ICRegisters})
	 * @param register4 op3: Register (see {@link ICRegisters})
	 * @param immediate op4: Immediate value
	 */
	public static Instruction create(int code, ICRegister register1, ICRegister register2, ICRegister register3, ICRegister register4, int immediate) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Register(register1.get());

		instruction.setOp1Register(register2.get());

		instruction.setOp2Register(register3.get());

		instruction.setOp3Register(register4.get());

		initializeSignedImmediate(instruction, 4, immediate);

		assert instruction.getOpCount() == 5 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 5 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param register1 op0: Register (see {@link ICRegisters})
	 * @param register2 op1: Register (see {@link ICRegisters})
	 * @param register3 op2: Register (see {@link ICRegisters})
	 * @param memory op3: Memory operand
	 * @param immediate op4: Immediate value
	 */
	public static Instruction create(int code, ICRegister register1, ICRegister register2, ICRegister register3, MemoryOperand memory, int immediate) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Register(register1.get());

		instruction.setOp1Register(register2.get());

		instruction.setOp2Register(register3.get());

		instruction.setOp3Kind(OpKind.MEMORY);
		initMemoryOperand(instruction, memory);

		initializeSignedImmediate(instruction, 4, immediate);

		assert instruction.getOpCount() == 5 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates an instruction with 5 operands
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param register1 op0: Register (see {@link ICRegisters})
	 * @param register2 op1: Register (see {@link ICRegisters})
	 * @param memory op2: Memory operand
	 * @param register3 op3: Register (see {@link ICRegisters})
	 * @param immediate op4: Immediate value
	 */
	public static Instruction create(int code, ICRegister register1, ICRegister register2, MemoryOperand memory, ICRegister register3, int immediate) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Register(register1.get());

		instruction.setOp1Register(register2.get());

		instruction.setOp2Kind(OpKind.MEMORY);
		initMemoryOperand(instruction, memory);

		instruction.setOp3Register(register3.get());

		initializeSignedImmediate(instruction, 4, immediate);

		assert instruction.getOpCount() == 5 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a new near/short branch instruction
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param target Target address
	 */
	public static Instruction createBranch(int code, long target) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Kind(getNearBranchOpKind(code, 0));
		instruction.setNearBranch64(target);

		assert instruction.getOpCount() == 1 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a new far branch instruction
	 *
	 * @param code Code value (a {@link com.github.icedland.iced.x86.Code} enum variant)
	 * @param selector Selector/segment value
	 * @param offset Offset
	 */
	public static Instruction createBranch(int code, int selector, int offset) {
		Instruction instruction = new Instruction();
		instruction.setCode(code);

		instruction.setOp0Kind(getFarBranchOpKind(code, 0));
		instruction.setFarBranchSelector(toShort(selector));
		instruction.setFarBranch32(offset);

		assert instruction.getOpCount() == 1 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a new {@code XBEGIN} instruction
	 *
	 * @param bitness 16, 32, or 64
	 * @param target Target address
	 */
	public static Instruction createXbegin(int bitness, long target) {
		Instruction instruction = new Instruction();
		switch (bitness) {
		case 16:
			instruction.setCode(Code.XBEGIN_REL16);
			instruction.setOp0Kind(OpKind.NEAR_BRANCH32);
			instruction.setNearBranch32((int)target);
			break;

		case 32:
			instruction.setCode(Code.XBEGIN_REL32);
			instruction.setOp0Kind(OpKind.NEAR_BRANCH32);
			instruction.setNearBranch32((int)target);
			break;

		case 64:
			instruction.setCode(Code.XBEGIN_REL32);
			instruction.setOp0Kind(OpKind.NEAR_BRANCH64);
			instruction.setNearBranch64(target);
			break;

		default:
			throw new IllegalArgumentException("bitness");
		}

		assert instruction.getOpCount() == 1 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code OUTSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createOutsb(int addressSize) {
		return createString_Reg_SegRSI(Code.OUTSB_DX_M8, addressSize, Register.DX, Register.NONE, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code OUTSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 */
	public static Instruction createOutsb(int addressSize, ICRegister segmentPrefix) {
		return createString_Reg_SegRSI(Code.OUTSB_DX_M8, addressSize, Register.DX, segmentPrefix.get(), RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code OUTSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createOutsb(int addressSize, ICRegister segmentPrefix, int repPrefix) {
		return createString_Reg_SegRSI(Code.OUTSB_DX_M8, addressSize, Register.DX, segmentPrefix.get(), repPrefix);
	}

	/**
	 * Creates a {@code REP OUTSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepOutsb(int addressSize) {
		return createString_Reg_SegRSI(Code.OUTSB_DX_M8, addressSize, Register.DX, Register.NONE, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code OUTSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createOutsw(int addressSize) {
		return createString_Reg_SegRSI(Code.OUTSW_DX_M16, addressSize, Register.DX, Register.NONE, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code OUTSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 */
	public static Instruction createOutsw(int addressSize, ICRegister segmentPrefix) {
		return createString_Reg_SegRSI(Code.OUTSW_DX_M16, addressSize, Register.DX, segmentPrefix.get(), RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code OUTSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createOutsw(int addressSize, ICRegister segmentPrefix, int repPrefix) {
		return createString_Reg_SegRSI(Code.OUTSW_DX_M16, addressSize, Register.DX, segmentPrefix.get(), repPrefix);
	}

	/**
	 * Creates a {@code REP OUTSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepOutsw(int addressSize) {
		return createString_Reg_SegRSI(Code.OUTSW_DX_M16, addressSize, Register.DX, Register.NONE, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code OUTSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createOutsd(int addressSize) {
		return createString_Reg_SegRSI(Code.OUTSD_DX_M32, addressSize, Register.DX, Register.NONE, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code OUTSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 */
	public static Instruction createOutsd(int addressSize, ICRegister segmentPrefix) {
		return createString_Reg_SegRSI(Code.OUTSD_DX_M32, addressSize, Register.DX, segmentPrefix.get(), RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code OUTSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createOutsd(int addressSize, ICRegister segmentPrefix, int repPrefix) {
		return createString_Reg_SegRSI(Code.OUTSD_DX_M32, addressSize, Register.DX, segmentPrefix.get(), repPrefix);
	}

	/**
	 * Creates a {@code REP OUTSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepOutsd(int addressSize) {
		return createString_Reg_SegRSI(Code.OUTSD_DX_M32, addressSize, Register.DX, Register.NONE, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code LODSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createLodsb(int addressSize) {
		return createString_Reg_SegRSI(Code.LODSB_AL_M8, addressSize, Register.AL, Register.NONE, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code LODSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 */
	public static Instruction createLodsb(int addressSize, ICRegister segmentPrefix) {
		return createString_Reg_SegRSI(Code.LODSB_AL_M8, addressSize, Register.AL, segmentPrefix.get(), RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code LODSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createLodsb(int addressSize, ICRegister segmentPrefix, int repPrefix) {
		return createString_Reg_SegRSI(Code.LODSB_AL_M8, addressSize, Register.AL, segmentPrefix.get(), repPrefix);
	}

	/**
	 * Creates a {@code REP LODSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepLodsb(int addressSize) {
		return createString_Reg_SegRSI(Code.LODSB_AL_M8, addressSize, Register.AL, Register.NONE, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code LODSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createLodsw(int addressSize) {
		return createString_Reg_SegRSI(Code.LODSW_AX_M16, addressSize, Register.AX, Register.NONE, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code LODSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 */
	public static Instruction createLodsw(int addressSize, ICRegister segmentPrefix) {
		return createString_Reg_SegRSI(Code.LODSW_AX_M16, addressSize, Register.AX, segmentPrefix.get(), RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code LODSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createLodsw(int addressSize, ICRegister segmentPrefix, int repPrefix) {
		return createString_Reg_SegRSI(Code.LODSW_AX_M16, addressSize, Register.AX, segmentPrefix.get(), repPrefix);
	}

	/**
	 * Creates a {@code REP LODSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepLodsw(int addressSize) {
		return createString_Reg_SegRSI(Code.LODSW_AX_M16, addressSize, Register.AX, Register.NONE, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code LODSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createLodsd(int addressSize) {
		return createString_Reg_SegRSI(Code.LODSD_EAX_M32, addressSize, Register.EAX, Register.NONE, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code LODSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 */
	public static Instruction createLodsd(int addressSize, ICRegister segmentPrefix) {
		return createString_Reg_SegRSI(Code.LODSD_EAX_M32, addressSize, Register.EAX, segmentPrefix.get(), RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code LODSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createLodsd(int addressSize, ICRegister segmentPrefix, int repPrefix) {
		return createString_Reg_SegRSI(Code.LODSD_EAX_M32, addressSize, Register.EAX, segmentPrefix.get(), repPrefix);
	}

	/**
	 * Creates a {@code REP LODSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepLodsd(int addressSize) {
		return createString_Reg_SegRSI(Code.LODSD_EAX_M32, addressSize, Register.EAX, Register.NONE, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code LODSQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createLodsq(int addressSize) {
		return createString_Reg_SegRSI(Code.LODSQ_RAX_M64, addressSize, Register.RAX, Register.NONE, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code LODSQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 */
	public static Instruction createLodsq(int addressSize, ICRegister segmentPrefix) {
		return createString_Reg_SegRSI(Code.LODSQ_RAX_M64, addressSize, Register.RAX, segmentPrefix.get(), RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code LODSQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createLodsq(int addressSize, ICRegister segmentPrefix, int repPrefix) {
		return createString_Reg_SegRSI(Code.LODSQ_RAX_M64, addressSize, Register.RAX, segmentPrefix.get(), repPrefix);
	}

	/**
	 * Creates a {@code REP LODSQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepLodsq(int addressSize) {
		return createString_Reg_SegRSI(Code.LODSQ_RAX_M64, addressSize, Register.RAX, Register.NONE, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code SCASB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createScasb(int addressSize) {
		return createString_Reg_ESRDI(Code.SCASB_AL_M8, addressSize, Register.AL, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code SCASB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createScasb(int addressSize, int repPrefix) {
		return createString_Reg_ESRDI(Code.SCASB_AL_M8, addressSize, Register.AL, repPrefix);
	}

	/**
	 * Creates a {@code REPE SCASB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepeScasb(int addressSize) {
		return createString_Reg_ESRDI(Code.SCASB_AL_M8, addressSize, Register.AL, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code REPNE SCASB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepneScasb(int addressSize) {
		return createString_Reg_ESRDI(Code.SCASB_AL_M8, addressSize, Register.AL, RepPrefixKind.REPNE);
	}

	/**
	 * Creates a {@code SCASW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createScasw(int addressSize) {
		return createString_Reg_ESRDI(Code.SCASW_AX_M16, addressSize, Register.AX, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code SCASW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createScasw(int addressSize, int repPrefix) {
		return createString_Reg_ESRDI(Code.SCASW_AX_M16, addressSize, Register.AX, repPrefix);
	}

	/**
	 * Creates a {@code REPE SCASW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepeScasw(int addressSize) {
		return createString_Reg_ESRDI(Code.SCASW_AX_M16, addressSize, Register.AX, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code REPNE SCASW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepneScasw(int addressSize) {
		return createString_Reg_ESRDI(Code.SCASW_AX_M16, addressSize, Register.AX, RepPrefixKind.REPNE);
	}

	/**
	 * Creates a {@code SCASD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createScasd(int addressSize) {
		return createString_Reg_ESRDI(Code.SCASD_EAX_M32, addressSize, Register.EAX, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code SCASD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createScasd(int addressSize, int repPrefix) {
		return createString_Reg_ESRDI(Code.SCASD_EAX_M32, addressSize, Register.EAX, repPrefix);
	}

	/**
	 * Creates a {@code REPE SCASD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepeScasd(int addressSize) {
		return createString_Reg_ESRDI(Code.SCASD_EAX_M32, addressSize, Register.EAX, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code REPNE SCASD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepneScasd(int addressSize) {
		return createString_Reg_ESRDI(Code.SCASD_EAX_M32, addressSize, Register.EAX, RepPrefixKind.REPNE);
	}

	/**
	 * Creates a {@code SCASQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createScasq(int addressSize) {
		return createString_Reg_ESRDI(Code.SCASQ_RAX_M64, addressSize, Register.RAX, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code SCASQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createScasq(int addressSize, int repPrefix) {
		return createString_Reg_ESRDI(Code.SCASQ_RAX_M64, addressSize, Register.RAX, repPrefix);
	}

	/**
	 * Creates a {@code REPE SCASQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepeScasq(int addressSize) {
		return createString_Reg_ESRDI(Code.SCASQ_RAX_M64, addressSize, Register.RAX, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code REPNE SCASQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepneScasq(int addressSize) {
		return createString_Reg_ESRDI(Code.SCASQ_RAX_M64, addressSize, Register.RAX, RepPrefixKind.REPNE);
	}

	/**
	 * Creates a {@code INSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createInsb(int addressSize) {
		return createString_ESRDI_Reg(Code.INSB_M8_DX, addressSize, Register.DX, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code INSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createInsb(int addressSize, int repPrefix) {
		return createString_ESRDI_Reg(Code.INSB_M8_DX, addressSize, Register.DX, repPrefix);
	}

	/**
	 * Creates a {@code REP INSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepInsb(int addressSize) {
		return createString_ESRDI_Reg(Code.INSB_M8_DX, addressSize, Register.DX, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code INSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createInsw(int addressSize) {
		return createString_ESRDI_Reg(Code.INSW_M16_DX, addressSize, Register.DX, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code INSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createInsw(int addressSize, int repPrefix) {
		return createString_ESRDI_Reg(Code.INSW_M16_DX, addressSize, Register.DX, repPrefix);
	}

	/**
	 * Creates a {@code REP INSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepInsw(int addressSize) {
		return createString_ESRDI_Reg(Code.INSW_M16_DX, addressSize, Register.DX, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code INSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createInsd(int addressSize) {
		return createString_ESRDI_Reg(Code.INSD_M32_DX, addressSize, Register.DX, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code INSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createInsd(int addressSize, int repPrefix) {
		return createString_ESRDI_Reg(Code.INSD_M32_DX, addressSize, Register.DX, repPrefix);
	}

	/**
	 * Creates a {@code REP INSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepInsd(int addressSize) {
		return createString_ESRDI_Reg(Code.INSD_M32_DX, addressSize, Register.DX, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code STOSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createStosb(int addressSize) {
		return createString_ESRDI_Reg(Code.STOSB_M8_AL, addressSize, Register.AL, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code STOSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createStosb(int addressSize, int repPrefix) {
		return createString_ESRDI_Reg(Code.STOSB_M8_AL, addressSize, Register.AL, repPrefix);
	}

	/**
	 * Creates a {@code REP STOSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepStosb(int addressSize) {
		return createString_ESRDI_Reg(Code.STOSB_M8_AL, addressSize, Register.AL, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code STOSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createStosw(int addressSize) {
		return createString_ESRDI_Reg(Code.STOSW_M16_AX, addressSize, Register.AX, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code STOSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createStosw(int addressSize, int repPrefix) {
		return createString_ESRDI_Reg(Code.STOSW_M16_AX, addressSize, Register.AX, repPrefix);
	}

	/**
	 * Creates a {@code REP STOSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepStosw(int addressSize) {
		return createString_ESRDI_Reg(Code.STOSW_M16_AX, addressSize, Register.AX, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code STOSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createStosd(int addressSize) {
		return createString_ESRDI_Reg(Code.STOSD_M32_EAX, addressSize, Register.EAX, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code STOSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createStosd(int addressSize, int repPrefix) {
		return createString_ESRDI_Reg(Code.STOSD_M32_EAX, addressSize, Register.EAX, repPrefix);
	}

	/**
	 * Creates a {@code REP STOSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepStosd(int addressSize) {
		return createString_ESRDI_Reg(Code.STOSD_M32_EAX, addressSize, Register.EAX, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code STOSQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createStosq(int addressSize) {
		return createString_ESRDI_Reg(Code.STOSQ_M64_RAX, addressSize, Register.RAX, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code STOSQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createStosq(int addressSize, int repPrefix) {
		return createString_ESRDI_Reg(Code.STOSQ_M64_RAX, addressSize, Register.RAX, repPrefix);
	}

	/**
	 * Creates a {@code REP STOSQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepStosq(int addressSize) {
		return createString_ESRDI_Reg(Code.STOSQ_M64_RAX, addressSize, Register.RAX, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code CMPSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createCmpsb(int addressSize) {
		return createString_SegRSI_ESRDI(Code.CMPSB_M8_M8, addressSize, Register.NONE, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code CMPSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 */
	public static Instruction createCmpsb(int addressSize, ICRegister segmentPrefix) {
		return createString_SegRSI_ESRDI(Code.CMPSB_M8_M8, addressSize, segmentPrefix.get(), RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code CMPSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createCmpsb(int addressSize, ICRegister segmentPrefix, int repPrefix) {
		return createString_SegRSI_ESRDI(Code.CMPSB_M8_M8, addressSize, segmentPrefix.get(), repPrefix);
	}

	/**
	 * Creates a {@code REPE CMPSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepeCmpsb(int addressSize) {
		return createString_SegRSI_ESRDI(Code.CMPSB_M8_M8, addressSize, Register.NONE, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code REPNE CMPSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepneCmpsb(int addressSize) {
		return createString_SegRSI_ESRDI(Code.CMPSB_M8_M8, addressSize, Register.NONE, RepPrefixKind.REPNE);
	}

	/**
	 * Creates a {@code CMPSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createCmpsw(int addressSize) {
		return createString_SegRSI_ESRDI(Code.CMPSW_M16_M16, addressSize, Register.NONE, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code CMPSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 */
	public static Instruction createCmpsw(int addressSize, ICRegister segmentPrefix) {
		return createString_SegRSI_ESRDI(Code.CMPSW_M16_M16, addressSize, segmentPrefix.get(), RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code CMPSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createCmpsw(int addressSize, ICRegister segmentPrefix, int repPrefix) {
		return createString_SegRSI_ESRDI(Code.CMPSW_M16_M16, addressSize, segmentPrefix.get(), repPrefix);
	}

	/**
	 * Creates a {@code REPE CMPSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepeCmpsw(int addressSize) {
		return createString_SegRSI_ESRDI(Code.CMPSW_M16_M16, addressSize, Register.NONE, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code REPNE CMPSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepneCmpsw(int addressSize) {
		return createString_SegRSI_ESRDI(Code.CMPSW_M16_M16, addressSize, Register.NONE, RepPrefixKind.REPNE);
	}

	/**
	 * Creates a {@code CMPSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createCmpsd(int addressSize) {
		return createString_SegRSI_ESRDI(Code.CMPSD_M32_M32, addressSize, Register.NONE, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code CMPSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 */
	public static Instruction createCmpsd(int addressSize, ICRegister segmentPrefix) {
		return createString_SegRSI_ESRDI(Code.CMPSD_M32_M32, addressSize, segmentPrefix.get(), RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code CMPSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createCmpsd(int addressSize, ICRegister segmentPrefix, int repPrefix) {
		return createString_SegRSI_ESRDI(Code.CMPSD_M32_M32, addressSize, segmentPrefix.get(), repPrefix);
	}

	/**
	 * Creates a {@code REPE CMPSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepeCmpsd(int addressSize) {
		return createString_SegRSI_ESRDI(Code.CMPSD_M32_M32, addressSize, Register.NONE, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code REPNE CMPSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepneCmpsd(int addressSize) {
		return createString_SegRSI_ESRDI(Code.CMPSD_M32_M32, addressSize, Register.NONE, RepPrefixKind.REPNE);
	}

	/**
	 * Creates a {@code CMPSQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createCmpsq(int addressSize) {
		return createString_SegRSI_ESRDI(Code.CMPSQ_M64_M64, addressSize, Register.NONE, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code CMPSQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 */
	public static Instruction createCmpsq(int addressSize, ICRegister segmentPrefix) {
		return createString_SegRSI_ESRDI(Code.CMPSQ_M64_M64, addressSize, segmentPrefix.get(), RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code CMPSQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createCmpsq(int addressSize, ICRegister segmentPrefix, int repPrefix) {
		return createString_SegRSI_ESRDI(Code.CMPSQ_M64_M64, addressSize, segmentPrefix.get(), repPrefix);
	}

	/**
	 * Creates a {@code REPE CMPSQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepeCmpsq(int addressSize) {
		return createString_SegRSI_ESRDI(Code.CMPSQ_M64_M64, addressSize, Register.NONE, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code REPNE CMPSQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepneCmpsq(int addressSize) {
		return createString_SegRSI_ESRDI(Code.CMPSQ_M64_M64, addressSize, Register.NONE, RepPrefixKind.REPNE);
	}

	/**
	 * Creates a {@code MOVSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createMovsb(int addressSize) {
		return createString_ESRDI_SegRSI(Code.MOVSB_M8_M8, addressSize, Register.NONE, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code MOVSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 */
	public static Instruction createMovsb(int addressSize, ICRegister segmentPrefix) {
		return createString_ESRDI_SegRSI(Code.MOVSB_M8_M8, addressSize, segmentPrefix.get(), RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code MOVSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createMovsb(int addressSize, ICRegister segmentPrefix, int repPrefix) {
		return createString_ESRDI_SegRSI(Code.MOVSB_M8_M8, addressSize, segmentPrefix.get(), repPrefix);
	}

	/**
	 * Creates a {@code REP MOVSB} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepMovsb(int addressSize) {
		return createString_ESRDI_SegRSI(Code.MOVSB_M8_M8, addressSize, Register.NONE, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code MOVSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createMovsw(int addressSize) {
		return createString_ESRDI_SegRSI(Code.MOVSW_M16_M16, addressSize, Register.NONE, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code MOVSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 */
	public static Instruction createMovsw(int addressSize, ICRegister segmentPrefix) {
		return createString_ESRDI_SegRSI(Code.MOVSW_M16_M16, addressSize, segmentPrefix.get(), RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code MOVSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createMovsw(int addressSize, ICRegister segmentPrefix, int repPrefix) {
		return createString_ESRDI_SegRSI(Code.MOVSW_M16_M16, addressSize, segmentPrefix.get(), repPrefix);
	}

	/**
	 * Creates a {@code REP MOVSW} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepMovsw(int addressSize) {
		return createString_ESRDI_SegRSI(Code.MOVSW_M16_M16, addressSize, Register.NONE, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code MOVSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createMovsd(int addressSize) {
		return createString_ESRDI_SegRSI(Code.MOVSD_M32_M32, addressSize, Register.NONE, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code MOVSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 */
	public static Instruction createMovsd(int addressSize, ICRegister segmentPrefix) {
		return createString_ESRDI_SegRSI(Code.MOVSD_M32_M32, addressSize, segmentPrefix.get(), RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code MOVSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createMovsd(int addressSize, ICRegister segmentPrefix, int repPrefix) {
		return createString_ESRDI_SegRSI(Code.MOVSD_M32_M32, addressSize, segmentPrefix.get(), repPrefix);
	}

	/**
	 * Creates a {@code REP MOVSD} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepMovsd(int addressSize) {
		return createString_ESRDI_SegRSI(Code.MOVSD_M32_M32, addressSize, Register.NONE, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code MOVSQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createMovsq(int addressSize) {
		return createString_ESRDI_SegRSI(Code.MOVSQ_M64_M64, addressSize, Register.NONE, RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code MOVSQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 */
	public static Instruction createMovsq(int addressSize, ICRegister segmentPrefix) {
		return createString_ESRDI_SegRSI(Code.MOVSQ_M64_M64, addressSize, segmentPrefix.get(), RepPrefixKind.NONE);
	}

	/**
	 * Creates a {@code MOVSQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 * @param repPrefix Rep prefix or {@link com.github.icedland.iced.x86.RepPrefixKind#NONE} (a {@link com.github.icedland.iced.x86.RepPrefixKind} enum variant)
	 */
	public static Instruction createMovsq(int addressSize, ICRegister segmentPrefix, int repPrefix) {
		return createString_ESRDI_SegRSI(Code.MOVSQ_M64_M64, addressSize, segmentPrefix.get(), repPrefix);
	}

	/**
	 * Creates a {@code REP MOVSQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 */
	public static Instruction createRepMovsq(int addressSize) {
		return createString_ESRDI_SegRSI(Code.MOVSQ_M64_M64, addressSize, Register.NONE, RepPrefixKind.REPE);
	}

	/**
	 * Creates a {@code MASKMOVQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param register1 Register (see {@link ICRegisters})
	 * @param register2 Register (see {@link ICRegisters})
	 */
	public static Instruction createMaskmovq(int addressSize, ICRegister register1, ICRegister register2) {
		return createMaskmov(Code.MASKMOVQ_RDI_MM_MM, addressSize, register1.get(), register2.get(), Register.NONE);
	}

	/**
	 * Creates a {@code MASKMOVQ} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param register1 Register (see {@link ICRegisters})
	 * @param register2 Register (see {@link ICRegisters})
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 */
	public static Instruction createMaskmovq(int addressSize, ICRegister register1, ICRegister register2, ICRegister segmentPrefix) {
		return createMaskmov(Code.MASKMOVQ_RDI_MM_MM, addressSize, register1.get(), register2.get(), segmentPrefix.get());
	}

	/**
	 * Creates a {@code MASKMOVDQU} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param register1 Register (see {@link ICRegisters})
	 * @param register2 Register (see {@link ICRegisters})
	 */
	public static Instruction createMaskmovdqu(int addressSize, ICRegister register1, ICRegister register2) {
		return createMaskmov(Code.MASKMOVDQU_RDI_XMM_XMM, addressSize, register1.get(), register2.get(), Register.NONE);
	}

	/**
	 * Creates a {@code MASKMOVDQU} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param register1 Register (see {@link ICRegisters})
	 * @param register2 Register (see {@link ICRegisters})
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 */
	public static Instruction createMaskmovdqu(int addressSize, ICRegister register1, ICRegister register2, ICRegister segmentPrefix) {
		return createMaskmov(Code.MASKMOVDQU_RDI_XMM_XMM, addressSize, register1.get(), register2.get(), segmentPrefix.get());
	}

	/**
	 * Creates a {@code VMASKMOVDQU} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param register1 Register (see {@link ICRegisters})
	 * @param register2 Register (see {@link ICRegisters})
	 */
	public static Instruction createVmaskmovdqu(int addressSize, ICRegister register1, ICRegister register2) {
		return createMaskmov(Code.VEX_VMASKMOVDQU_RDI_XMM_XMM, addressSize, register1.get(), register2.get(), Register.NONE);
	}

	/**
	 * Creates a {@code VMASKMOVDQU} instruction
	 *
	 * @param addressSize 16, 32, or 64
	 * @param register1 Register (see {@link ICRegisters})
	 * @param register2 Register (see {@link ICRegisters})
	 * @param segmentPrefix Segment override or {@link com.github.icedland.iced.x86.Register#NONE} (see {@link ICRegisters})
	 */
	public static Instruction createVmaskmovdqu(int addressSize, ICRegister register1, ICRegister register2, ICRegister segmentPrefix) {
		return createMaskmov(Code.VEX_VMASKMOVDQU_RDI_XMM_XMM, addressSize, register1.get(), register2.get(), segmentPrefix.get());
	}

	/**
	 * Creates a {@code db}/{@code .byte} asm directive
	 *
	 * @param b0 Byte 0
	 */
	public static Instruction createDeclareByte(int b0) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREBYTE);
		instruction.setDeclareDataCount(1);

		instruction.setDeclareByteValue(0, toByte(b0));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code db}/{@code .byte} asm directive
	 *
	 * @param b0 Byte 0
	 * @param b1 Byte 1
	 */
	public static Instruction createDeclareByte(int b0, int b1) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREBYTE);
		instruction.setDeclareDataCount(2);

		instruction.setDeclareByteValue(0, toByte(b0));
		instruction.setDeclareByteValue(1, toByte(b1));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code db}/{@code .byte} asm directive
	 *
	 * @param b0 Byte 0
	 * @param b1 Byte 1
	 * @param b2 Byte 2
	 */
	public static Instruction createDeclareByte(int b0, int b1, int b2) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREBYTE);
		instruction.setDeclareDataCount(3);

		instruction.setDeclareByteValue(0, toByte(b0));
		instruction.setDeclareByteValue(1, toByte(b1));
		instruction.setDeclareByteValue(2, toByte(b2));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code db}/{@code .byte} asm directive
	 *
	 * @param b0 Byte 0
	 * @param b1 Byte 1
	 * @param b2 Byte 2
	 * @param b3 Byte 3
	 */
	public static Instruction createDeclareByte(int b0, int b1, int b2, int b3) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREBYTE);
		instruction.setDeclareDataCount(4);

		instruction.setDeclareByteValue(0, toByte(b0));
		instruction.setDeclareByteValue(1, toByte(b1));
		instruction.setDeclareByteValue(2, toByte(b2));
		instruction.setDeclareByteValue(3, toByte(b3));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code db}/{@code .byte} asm directive
	 *
	 * @param b0 Byte 0
	 * @param b1 Byte 1
	 * @param b2 Byte 2
	 * @param b3 Byte 3
	 * @param b4 Byte 4
	 */
	public static Instruction createDeclareByte(int b0, int b1, int b2, int b3, int b4) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREBYTE);
		instruction.setDeclareDataCount(5);

		instruction.setDeclareByteValue(0, toByte(b0));
		instruction.setDeclareByteValue(1, toByte(b1));
		instruction.setDeclareByteValue(2, toByte(b2));
		instruction.setDeclareByteValue(3, toByte(b3));
		instruction.setDeclareByteValue(4, toByte(b4));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code db}/{@code .byte} asm directive
	 *
	 * @param b0 Byte 0
	 * @param b1 Byte 1
	 * @param b2 Byte 2
	 * @param b3 Byte 3
	 * @param b4 Byte 4
	 * @param b5 Byte 5
	 */
	public static Instruction createDeclareByte(int b0, int b1, int b2, int b3, int b4, int b5) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREBYTE);
		instruction.setDeclareDataCount(6);

		instruction.setDeclareByteValue(0, toByte(b0));
		instruction.setDeclareByteValue(1, toByte(b1));
		instruction.setDeclareByteValue(2, toByte(b2));
		instruction.setDeclareByteValue(3, toByte(b3));
		instruction.setDeclareByteValue(4, toByte(b4));
		instruction.setDeclareByteValue(5, toByte(b5));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code db}/{@code .byte} asm directive
	 *
	 * @param b0 Byte 0
	 * @param b1 Byte 1
	 * @param b2 Byte 2
	 * @param b3 Byte 3
	 * @param b4 Byte 4
	 * @param b5 Byte 5
	 * @param b6 Byte 6
	 */
	public static Instruction createDeclareByte(int b0, int b1, int b2, int b3, int b4, int b5, int b6) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREBYTE);
		instruction.setDeclareDataCount(7);

		instruction.setDeclareByteValue(0, toByte(b0));
		instruction.setDeclareByteValue(1, toByte(b1));
		instruction.setDeclareByteValue(2, toByte(b2));
		instruction.setDeclareByteValue(3, toByte(b3));
		instruction.setDeclareByteValue(4, toByte(b4));
		instruction.setDeclareByteValue(5, toByte(b5));
		instruction.setDeclareByteValue(6, toByte(b6));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code db}/{@code .byte} asm directive
	 *
	 * @param b0 Byte 0
	 * @param b1 Byte 1
	 * @param b2 Byte 2
	 * @param b3 Byte 3
	 * @param b4 Byte 4
	 * @param b5 Byte 5
	 * @param b6 Byte 6
	 * @param b7 Byte 7
	 */
	public static Instruction createDeclareByte(int b0, int b1, int b2, int b3, int b4, int b5, int b6, int b7) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREBYTE);
		instruction.setDeclareDataCount(8);

		instruction.setDeclareByteValue(0, toByte(b0));
		instruction.setDeclareByteValue(1, toByte(b1));
		instruction.setDeclareByteValue(2, toByte(b2));
		instruction.setDeclareByteValue(3, toByte(b3));
		instruction.setDeclareByteValue(4, toByte(b4));
		instruction.setDeclareByteValue(5, toByte(b5));
		instruction.setDeclareByteValue(6, toByte(b6));
		instruction.setDeclareByteValue(7, toByte(b7));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code db}/{@code .byte} asm directive
	 *
	 * @param b0 Byte 0
	 * @param b1 Byte 1
	 * @param b2 Byte 2
	 * @param b3 Byte 3
	 * @param b4 Byte 4
	 * @param b5 Byte 5
	 * @param b6 Byte 6
	 * @param b7 Byte 7
	 * @param b8 Byte 8
	 */
	public static Instruction createDeclareByte(int b0, int b1, int b2, int b3, int b4, int b5, int b6, int b7, int b8) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREBYTE);
		instruction.setDeclareDataCount(9);

		instruction.setDeclareByteValue(0, toByte(b0));
		instruction.setDeclareByteValue(1, toByte(b1));
		instruction.setDeclareByteValue(2, toByte(b2));
		instruction.setDeclareByteValue(3, toByte(b3));
		instruction.setDeclareByteValue(4, toByte(b4));
		instruction.setDeclareByteValue(5, toByte(b5));
		instruction.setDeclareByteValue(6, toByte(b6));
		instruction.setDeclareByteValue(7, toByte(b7));
		instruction.setDeclareByteValue(8, toByte(b8));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code db}/{@code .byte} asm directive
	 *
	 * @param b0 Byte 0
	 * @param b1 Byte 1
	 * @param b2 Byte 2
	 * @param b3 Byte 3
	 * @param b4 Byte 4
	 * @param b5 Byte 5
	 * @param b6 Byte 6
	 * @param b7 Byte 7
	 * @param b8 Byte 8
	 * @param b9 Byte 9
	 */
	public static Instruction createDeclareByte(int b0, int b1, int b2, int b3, int b4, int b5, int b6, int b7, int b8, int b9) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREBYTE);
		instruction.setDeclareDataCount(10);

		instruction.setDeclareByteValue(0, toByte(b0));
		instruction.setDeclareByteValue(1, toByte(b1));
		instruction.setDeclareByteValue(2, toByte(b2));
		instruction.setDeclareByteValue(3, toByte(b3));
		instruction.setDeclareByteValue(4, toByte(b4));
		instruction.setDeclareByteValue(5, toByte(b5));
		instruction.setDeclareByteValue(6, toByte(b6));
		instruction.setDeclareByteValue(7, toByte(b7));
		instruction.setDeclareByteValue(8, toByte(b8));
		instruction.setDeclareByteValue(9, toByte(b9));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code db}/{@code .byte} asm directive
	 *
	 * @param b0 Byte 0
	 * @param b1 Byte 1
	 * @param b2 Byte 2
	 * @param b3 Byte 3
	 * @param b4 Byte 4
	 * @param b5 Byte 5
	 * @param b6 Byte 6
	 * @param b7 Byte 7
	 * @param b8 Byte 8
	 * @param b9 Byte 9
	 * @param b10 Byte 10
	 */
	public static Instruction createDeclareByte(int b0, int b1, int b2, int b3, int b4, int b5, int b6, int b7, int b8, int b9, int b10) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREBYTE);
		instruction.setDeclareDataCount(11);

		instruction.setDeclareByteValue(0, toByte(b0));
		instruction.setDeclareByteValue(1, toByte(b1));
		instruction.setDeclareByteValue(2, toByte(b2));
		instruction.setDeclareByteValue(3, toByte(b3));
		instruction.setDeclareByteValue(4, toByte(b4));
		instruction.setDeclareByteValue(5, toByte(b5));
		instruction.setDeclareByteValue(6, toByte(b6));
		instruction.setDeclareByteValue(7, toByte(b7));
		instruction.setDeclareByteValue(8, toByte(b8));
		instruction.setDeclareByteValue(9, toByte(b9));
		instruction.setDeclareByteValue(10, toByte(b10));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code db}/{@code .byte} asm directive
	 *
	 * @param b0 Byte 0
	 * @param b1 Byte 1
	 * @param b2 Byte 2
	 * @param b3 Byte 3
	 * @param b4 Byte 4
	 * @param b5 Byte 5
	 * @param b6 Byte 6
	 * @param b7 Byte 7
	 * @param b8 Byte 8
	 * @param b9 Byte 9
	 * @param b10 Byte 10
	 * @param b11 Byte 11
	 */
	public static Instruction createDeclareByte(int b0, int b1, int b2, int b3, int b4, int b5, int b6, int b7, int b8, int b9, int b10, int b11) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREBYTE);
		instruction.setDeclareDataCount(12);

		instruction.setDeclareByteValue(0, toByte(b0));
		instruction.setDeclareByteValue(1, toByte(b1));
		instruction.setDeclareByteValue(2, toByte(b2));
		instruction.setDeclareByteValue(3, toByte(b3));
		instruction.setDeclareByteValue(4, toByte(b4));
		instruction.setDeclareByteValue(5, toByte(b5));
		instruction.setDeclareByteValue(6, toByte(b6));
		instruction.setDeclareByteValue(7, toByte(b7));
		instruction.setDeclareByteValue(8, toByte(b8));
		instruction.setDeclareByteValue(9, toByte(b9));
		instruction.setDeclareByteValue(10, toByte(b10));
		instruction.setDeclareByteValue(11, toByte(b11));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code db}/{@code .byte} asm directive
	 *
	 * @param b0 Byte 0
	 * @param b1 Byte 1
	 * @param b2 Byte 2
	 * @param b3 Byte 3
	 * @param b4 Byte 4
	 * @param b5 Byte 5
	 * @param b6 Byte 6
	 * @param b7 Byte 7
	 * @param b8 Byte 8
	 * @param b9 Byte 9
	 * @param b10 Byte 10
	 * @param b11 Byte 11
	 * @param b12 Byte 12
	 */
	public static Instruction createDeclareByte(int b0, int b1, int b2, int b3, int b4, int b5, int b6, int b7, int b8, int b9, int b10, int b11, int b12) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREBYTE);
		instruction.setDeclareDataCount(13);

		instruction.setDeclareByteValue(0, toByte(b0));
		instruction.setDeclareByteValue(1, toByte(b1));
		instruction.setDeclareByteValue(2, toByte(b2));
		instruction.setDeclareByteValue(3, toByte(b3));
		instruction.setDeclareByteValue(4, toByte(b4));
		instruction.setDeclareByteValue(5, toByte(b5));
		instruction.setDeclareByteValue(6, toByte(b6));
		instruction.setDeclareByteValue(7, toByte(b7));
		instruction.setDeclareByteValue(8, toByte(b8));
		instruction.setDeclareByteValue(9, toByte(b9));
		instruction.setDeclareByteValue(10, toByte(b10));
		instruction.setDeclareByteValue(11, toByte(b11));
		instruction.setDeclareByteValue(12, toByte(b12));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code db}/{@code .byte} asm directive
	 *
	 * @param b0 Byte 0
	 * @param b1 Byte 1
	 * @param b2 Byte 2
	 * @param b3 Byte 3
	 * @param b4 Byte 4
	 * @param b5 Byte 5
	 * @param b6 Byte 6
	 * @param b7 Byte 7
	 * @param b8 Byte 8
	 * @param b9 Byte 9
	 * @param b10 Byte 10
	 * @param b11 Byte 11
	 * @param b12 Byte 12
	 * @param b13 Byte 13
	 */
	public static Instruction createDeclareByte(int b0, int b1, int b2, int b3, int b4, int b5, int b6, int b7, int b8, int b9, int b10, int b11, int b12, int b13) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREBYTE);
		instruction.setDeclareDataCount(14);

		instruction.setDeclareByteValue(0, toByte(b0));
		instruction.setDeclareByteValue(1, toByte(b1));
		instruction.setDeclareByteValue(2, toByte(b2));
		instruction.setDeclareByteValue(3, toByte(b3));
		instruction.setDeclareByteValue(4, toByte(b4));
		instruction.setDeclareByteValue(5, toByte(b5));
		instruction.setDeclareByteValue(6, toByte(b6));
		instruction.setDeclareByteValue(7, toByte(b7));
		instruction.setDeclareByteValue(8, toByte(b8));
		instruction.setDeclareByteValue(9, toByte(b9));
		instruction.setDeclareByteValue(10, toByte(b10));
		instruction.setDeclareByteValue(11, toByte(b11));
		instruction.setDeclareByteValue(12, toByte(b12));
		instruction.setDeclareByteValue(13, toByte(b13));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code db}/{@code .byte} asm directive
	 *
	 * @param b0 Byte 0
	 * @param b1 Byte 1
	 * @param b2 Byte 2
	 * @param b3 Byte 3
	 * @param b4 Byte 4
	 * @param b5 Byte 5
	 * @param b6 Byte 6
	 * @param b7 Byte 7
	 * @param b8 Byte 8
	 * @param b9 Byte 9
	 * @param b10 Byte 10
	 * @param b11 Byte 11
	 * @param b12 Byte 12
	 * @param b13 Byte 13
	 * @param b14 Byte 14
	 */
	public static Instruction createDeclareByte(int b0, int b1, int b2, int b3, int b4, int b5, int b6, int b7, int b8, int b9, int b10, int b11, int b12, int b13, int b14) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREBYTE);
		instruction.setDeclareDataCount(15);

		instruction.setDeclareByteValue(0, toByte(b0));
		instruction.setDeclareByteValue(1, toByte(b1));
		instruction.setDeclareByteValue(2, toByte(b2));
		instruction.setDeclareByteValue(3, toByte(b3));
		instruction.setDeclareByteValue(4, toByte(b4));
		instruction.setDeclareByteValue(5, toByte(b5));
		instruction.setDeclareByteValue(6, toByte(b6));
		instruction.setDeclareByteValue(7, toByte(b7));
		instruction.setDeclareByteValue(8, toByte(b8));
		instruction.setDeclareByteValue(9, toByte(b9));
		instruction.setDeclareByteValue(10, toByte(b10));
		instruction.setDeclareByteValue(11, toByte(b11));
		instruction.setDeclareByteValue(12, toByte(b12));
		instruction.setDeclareByteValue(13, toByte(b13));
		instruction.setDeclareByteValue(14, toByte(b14));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code db}/{@code .byte} asm directive
	 *
	 * @param b0 Byte 0
	 * @param b1 Byte 1
	 * @param b2 Byte 2
	 * @param b3 Byte 3
	 * @param b4 Byte 4
	 * @param b5 Byte 5
	 * @param b6 Byte 6
	 * @param b7 Byte 7
	 * @param b8 Byte 8
	 * @param b9 Byte 9
	 * @param b10 Byte 10
	 * @param b11 Byte 11
	 * @param b12 Byte 12
	 * @param b13 Byte 13
	 * @param b14 Byte 14
	 * @param b15 Byte 15
	 */
	public static Instruction createDeclareByte(int b0, int b1, int b2, int b3, int b4, int b5, int b6, int b7, int b8, int b9, int b10, int b11, int b12, int b13, int b14, int b15) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREBYTE);
		instruction.setDeclareDataCount(16);

		instruction.setDeclareByteValue(0, toByte(b0));
		instruction.setDeclareByteValue(1, toByte(b1));
		instruction.setDeclareByteValue(2, toByte(b2));
		instruction.setDeclareByteValue(3, toByte(b3));
		instruction.setDeclareByteValue(4, toByte(b4));
		instruction.setDeclareByteValue(5, toByte(b5));
		instruction.setDeclareByteValue(6, toByte(b6));
		instruction.setDeclareByteValue(7, toByte(b7));
		instruction.setDeclareByteValue(8, toByte(b8));
		instruction.setDeclareByteValue(9, toByte(b9));
		instruction.setDeclareByteValue(10, toByte(b10));
		instruction.setDeclareByteValue(11, toByte(b11));
		instruction.setDeclareByteValue(12, toByte(b12));
		instruction.setDeclareByteValue(13, toByte(b13));
		instruction.setDeclareByteValue(14, toByte(b14));
		instruction.setDeclareByteValue(15, toByte(b15));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code db}/{@code .byte} asm directive
	 *
	 * @param data Data
	 */
	public static Instruction createDeclareByte(byte[] data) {
		if (data == null)
			throw new NullPointerException("data");
		return createDeclareByte(data, 0, data.length);
	}

	/**
	 * Creates a {@code db}/{@code .byte} asm directive
	 *
	 * @param data Data
	 * @param index Start index
	 * @param length Number of bytes
	 */
	public static Instruction createDeclareByte(byte[] data, int index, int length) {
		if (data == null)
			throw new NullPointerException("data");
		if (Integer.compareUnsigned(length - 1, 16 - 1) > 0)
			throw new IllegalArgumentException("length");
		if (Long.compareUnsigned(((long)index & 0xFFFF_FFFFL) + ((long)length & 0xFFFF_FFFFL), (long)data.length & 0xFFFF_FFFFL) > 0)
			throw new IllegalArgumentException("index");

		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREBYTE);
		instruction.setDeclareDataCount(length);

		for (int i = 0; i < length; i++)
			instruction.setDeclareByteValue(i, data[index + i]);

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code dw}/{@code .word} asm directive
	 *
	 * @param w0 Word 0
	 */
	public static Instruction createDeclareWord(int w0) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREWORD);
		instruction.setDeclareDataCount(1);

		instruction.setDeclareWordValue(0, toShort(w0));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code dw}/{@code .word} asm directive
	 *
	 * @param w0 Word 0
	 * @param w1 Word 1
	 */
	public static Instruction createDeclareWord(int w0, int w1) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREWORD);
		instruction.setDeclareDataCount(2);

		instruction.setDeclareWordValue(0, toShort(w0));
		instruction.setDeclareWordValue(1, toShort(w1));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code dw}/{@code .word} asm directive
	 *
	 * @param w0 Word 0
	 * @param w1 Word 1
	 * @param w2 Word 2
	 */
	public static Instruction createDeclareWord(int w0, int w1, int w2) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREWORD);
		instruction.setDeclareDataCount(3);

		instruction.setDeclareWordValue(0, toShort(w0));
		instruction.setDeclareWordValue(1, toShort(w1));
		instruction.setDeclareWordValue(2, toShort(w2));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code dw}/{@code .word} asm directive
	 *
	 * @param w0 Word 0
	 * @param w1 Word 1
	 * @param w2 Word 2
	 * @param w3 Word 3
	 */
	public static Instruction createDeclareWord(int w0, int w1, int w2, int w3) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREWORD);
		instruction.setDeclareDataCount(4);

		instruction.setDeclareWordValue(0, toShort(w0));
		instruction.setDeclareWordValue(1, toShort(w1));
		instruction.setDeclareWordValue(2, toShort(w2));
		instruction.setDeclareWordValue(3, toShort(w3));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code dw}/{@code .word} asm directive
	 *
	 * @param w0 Word 0
	 * @param w1 Word 1
	 * @param w2 Word 2
	 * @param w3 Word 3
	 * @param w4 Word 4
	 */
	public static Instruction createDeclareWord(int w0, int w1, int w2, int w3, int w4) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREWORD);
		instruction.setDeclareDataCount(5);

		instruction.setDeclareWordValue(0, toShort(w0));
		instruction.setDeclareWordValue(1, toShort(w1));
		instruction.setDeclareWordValue(2, toShort(w2));
		instruction.setDeclareWordValue(3, toShort(w3));
		instruction.setDeclareWordValue(4, toShort(w4));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code dw}/{@code .word} asm directive
	 *
	 * @param w0 Word 0
	 * @param w1 Word 1
	 * @param w2 Word 2
	 * @param w3 Word 3
	 * @param w4 Word 4
	 * @param w5 Word 5
	 */
	public static Instruction createDeclareWord(int w0, int w1, int w2, int w3, int w4, int w5) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREWORD);
		instruction.setDeclareDataCount(6);

		instruction.setDeclareWordValue(0, toShort(w0));
		instruction.setDeclareWordValue(1, toShort(w1));
		instruction.setDeclareWordValue(2, toShort(w2));
		instruction.setDeclareWordValue(3, toShort(w3));
		instruction.setDeclareWordValue(4, toShort(w4));
		instruction.setDeclareWordValue(5, toShort(w5));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code dw}/{@code .word} asm directive
	 *
	 * @param w0 Word 0
	 * @param w1 Word 1
	 * @param w2 Word 2
	 * @param w3 Word 3
	 * @param w4 Word 4
	 * @param w5 Word 5
	 * @param w6 Word 6
	 */
	public static Instruction createDeclareWord(int w0, int w1, int w2, int w3, int w4, int w5, int w6) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREWORD);
		instruction.setDeclareDataCount(7);

		instruction.setDeclareWordValue(0, toShort(w0));
		instruction.setDeclareWordValue(1, toShort(w1));
		instruction.setDeclareWordValue(2, toShort(w2));
		instruction.setDeclareWordValue(3, toShort(w3));
		instruction.setDeclareWordValue(4, toShort(w4));
		instruction.setDeclareWordValue(5, toShort(w5));
		instruction.setDeclareWordValue(6, toShort(w6));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code dw}/{@code .word} asm directive
	 *
	 * @param w0 Word 0
	 * @param w1 Word 1
	 * @param w2 Word 2
	 * @param w3 Word 3
	 * @param w4 Word 4
	 * @param w5 Word 5
	 * @param w6 Word 6
	 * @param w7 Word 7
	 */
	public static Instruction createDeclareWord(int w0, int w1, int w2, int w3, int w4, int w5, int w6, int w7) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREWORD);
		instruction.setDeclareDataCount(8);

		instruction.setDeclareWordValue(0, toShort(w0));
		instruction.setDeclareWordValue(1, toShort(w1));
		instruction.setDeclareWordValue(2, toShort(w2));
		instruction.setDeclareWordValue(3, toShort(w3));
		instruction.setDeclareWordValue(4, toShort(w4));
		instruction.setDeclareWordValue(5, toShort(w5));
		instruction.setDeclareWordValue(6, toShort(w6));
		instruction.setDeclareWordValue(7, toShort(w7));

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code dw}/{@code .word} asm directive
	 *
	 * @param data Data
	 */
	public static Instruction createDeclareWord(byte[] data) {
		if (data == null)
			throw new NullPointerException("data");
		return createDeclareWord(data, 0, data.length);
	}

	/**
	 * Creates a {@code dw}/{@code .word} asm directive
	 *
	 * @param data Data
	 * @param index Start index
	 * @param length Number of bytes
	 */
	public static Instruction createDeclareWord(byte[] data, int index, int length) {
		if (data == null)
			throw new NullPointerException("data");
		if (Integer.compareUnsigned(length - 1, 16 - 1) > 0 || (length & 1) != 0)
			throw new IllegalArgumentException("length");
		if (Long.compareUnsigned(((long)index & 0xFFFF_FFFFL) + ((long)length & 0xFFFF_FFFFL), (long)data.length & 0xFFFF_FFFFL) > 0)
			throw new IllegalArgumentException("index");

		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREWORD);
		instruction.setDeclareDataCount(length / 2);

		for (int i = 0; i < length; i += 2) {
			int v = (data[index + i] & 0xFF) | ((data[index + i + 1] & 0xFF) << 8);
			instruction.setDeclareWordValue(i / 2, (short)v);
		}

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code dw}/{@code .word} asm directive
	 *
	 * @param data Data
	 */
	public static Instruction createDeclareWord(short[] data) {
		if (data == null)
			throw new NullPointerException("data");
		return createDeclareWord(data, 0, data.length);
	}

	/**
	 * Creates a {@code dw}/{@code .word} asm directive
	 *
	 * @param data Data
	 * @param index Start index
	 * @param length Number of elements
	 */
	public static Instruction createDeclareWord(short[] data, int index, int length) {
		if (data == null)
			throw new NullPointerException("data");
		if (Integer.compareUnsigned(length - 1, 8 - 1) > 0)
			throw new IllegalArgumentException("length");
		if (Long.compareUnsigned(((long)index & 0xFFFF_FFFFL) + ((long)length & 0xFFFF_FFFFL), (long)data.length & 0xFFFF_FFFFL) > 0)
			throw new IllegalArgumentException("index");

		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREWORD);
		instruction.setDeclareDataCount(length);

		for (int i = 0; i < length; i++)
			instruction.setDeclareWordValue(i, data[index + i]);

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code dd}/{@code .int} asm directive
	 *
	 * @param d0 Dword 0
	 */
	public static Instruction createDeclareDword(int d0) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREDWORD);
		instruction.setDeclareDataCount(1);

		instruction.setDeclareDwordValue(0, d0);

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code dd}/{@code .int} asm directive
	 *
	 * @param d0 Dword 0
	 * @param d1 Dword 1
	 */
	public static Instruction createDeclareDword(int d0, int d1) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREDWORD);
		instruction.setDeclareDataCount(2);

		instruction.setDeclareDwordValue(0, d0);
		instruction.setDeclareDwordValue(1, d1);

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code dd}/{@code .int} asm directive
	 *
	 * @param d0 Dword 0
	 * @param d1 Dword 1
	 * @param d2 Dword 2
	 */
	public static Instruction createDeclareDword(int d0, int d1, int d2) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREDWORD);
		instruction.setDeclareDataCount(3);

		instruction.setDeclareDwordValue(0, d0);
		instruction.setDeclareDwordValue(1, d1);
		instruction.setDeclareDwordValue(2, d2);

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code dd}/{@code .int} asm directive
	 *
	 * @param d0 Dword 0
	 * @param d1 Dword 1
	 * @param d2 Dword 2
	 * @param d3 Dword 3
	 */
	public static Instruction createDeclareDword(int d0, int d1, int d2, int d3) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREDWORD);
		instruction.setDeclareDataCount(4);

		instruction.setDeclareDwordValue(0, d0);
		instruction.setDeclareDwordValue(1, d1);
		instruction.setDeclareDwordValue(2, d2);
		instruction.setDeclareDwordValue(3, d3);

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code dd}/{@code .int} asm directive
	 *
	 * @param data Data
	 */
	public static Instruction createDeclareDword(byte[] data) {
		if (data == null)
			throw new NullPointerException("data");
		return createDeclareDword(data, 0, data.length);
	}

	/**
	 * Creates a {@code dd}/{@code .int} asm directive
	 *
	 * @param data Data
	 * @param index Start index
	 * @param length Number of bytes
	 */
	public static Instruction createDeclareDword(byte[] data, int index, int length) {
		if (data == null)
			throw new NullPointerException("data");
		if (Integer.compareUnsigned(length - 1, 16 - 1) > 0 || (length & 3) != 0)
			throw new IllegalArgumentException("length");
		if (Long.compareUnsigned(((long)index & 0xFFFF_FFFFL) + ((long)length & 0xFFFF_FFFFL), (long)data.length & 0xFFFF_FFFFL) > 0)
			throw new IllegalArgumentException("index");

		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREDWORD);
		instruction.setDeclareDataCount(length / 4);

		for (int i = 0; i < length; i += 4) {
			int v = (data[index + i] & 0xFF) | ((data[index + i + 1] & 0xFF) << 8) | ((data[index + i + 2] & 0xFF) << 16) | (data[index + i + 3] << 24);
			instruction.setDeclareDwordValue(i / 4, v);
		}

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code dd}/{@code .int} asm directive
	 *
	 * @param data Data
	 */
	public static Instruction createDeclareDword(int[] data) {
		if (data == null)
			throw new NullPointerException("data");
		return createDeclareDword(data, 0, data.length);
	}

	/**
	 * Creates a {@code dd}/{@code .int} asm directive
	 *
	 * @param data Data
	 * @param index Start index
	 * @param length Number of elements
	 */
	public static Instruction createDeclareDword(int[] data, int index, int length) {
		if (data == null)
			throw new NullPointerException("data");
		if (Integer.compareUnsigned(length - 1, 4 - 1) > 0)
			throw new IllegalArgumentException("length");
		if (Long.compareUnsigned(((long)index & 0xFFFF_FFFFL) + ((long)length & 0xFFFF_FFFFL), (long)data.length & 0xFFFF_FFFFL) > 0)
			throw new IllegalArgumentException("index");

		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREDWORD);
		instruction.setDeclareDataCount(length);

		for (int i = 0; i < length; i++)
			instruction.setDeclareDwordValue(i, data[index + i]);

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code dq}/{@code .quad} asm directive
	 *
	 * @param q0 Qword 0
	 */
	public static Instruction createDeclareQword(long q0) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREQWORD);
		instruction.setDeclareDataCount(1);

		instruction.setDeclareQwordValue(0, q0);

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code dq}/{@code .quad} asm directive
	 *
	 * @param q0 Qword 0
	 * @param q1 Qword 1
	 */
	public static Instruction createDeclareQword(long q0, long q1) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREQWORD);
		instruction.setDeclareDataCount(2);

		instruction.setDeclareQwordValue(0, q0);
		instruction.setDeclareQwordValue(1, q1);

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code dq}/{@code .quad} asm directive
	 *
	 * @param data Data
	 */
	public static Instruction createDeclareQword(byte[] data) {
		if (data == null)
			throw new NullPointerException("data");
		return createDeclareQword(data, 0, data.length);
	}

	/**
	 * Creates a {@code dq}/{@code .quad} asm directive
	 *
	 * @param data Data
	 * @param index Start index
	 * @param length Number of bytes
	 */
	public static Instruction createDeclareQword(byte[] data, int index, int length) {
		if (data == null)
			throw new NullPointerException("data");
		if (Integer.compareUnsigned(length - 1, 16 - 1) > 0 || (length & 7) != 0)
			throw new IllegalArgumentException("length");
		if (Long.compareUnsigned(((long)index & 0xFFFF_FFFFL) + ((long)length & 0xFFFF_FFFFL), (long)data.length & 0xFFFF_FFFFL) > 0)
			throw new IllegalArgumentException("index");

		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREQWORD);
		instruction.setDeclareDataCount(length / 8);

		for (int i = 0; i < length; i += 8) {
			int v1 = (data[index + i] & 0xFF) | ((data[index + i + 1] & 0xFF) << 8) | ((data[index + i + 2] & 0xFF) << 16) | (data[index + i + 3] << 24);
			int v2 = (data[index + i + 4] & 0xFF) | ((data[index + i + 5] & 0xFF) << 8) | ((data[index + i + 6] & 0xFF) << 16) | (data[index + i + 7] << 24);
			instruction.setDeclareQwordValue(i / 8, ((long)v1 & 0xFFFF_FFFFL) | ((long)v2 << 32));
		}

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}

	/**
	 * Creates a {@code dq}/{@code .quad} asm directive
	 *
	 * @param data Data
	 */
	public static Instruction createDeclareQword(long[] data) {
		if (data == null)
			throw new NullPointerException("data");
		return createDeclareQword(data, 0, data.length);
	}

	/**
	 * Creates a {@code dq}/{@code .quad} asm directive
	 *
	 * @param data Data
	 * @param index Start index
	 * @param length Number of elements
	 */
	public static Instruction createDeclareQword(long[] data, int index, int length) {
		if (data == null)
			throw new NullPointerException("data");
		if (Integer.compareUnsigned(length - 1, 2 - 1) > 0)
			throw new IllegalArgumentException("length");
		if (Long.compareUnsigned(((long)index & 0xFFFF_FFFFL) + ((long)length & 0xFFFF_FFFFL), (long)data.length & 0xFFFF_FFFFL) > 0)
			throw new IllegalArgumentException("index");

		Instruction instruction = new Instruction();
		instruction.setCode(Code.DECLAREQWORD);
		instruction.setDeclareDataCount(length);

		for (int i = 0; i < length; i++)
			instruction.setDeclareQwordValue(i, data[index + i]);

		assert instruction.getOpCount() == 0 : instruction.getOpCount();
		return instruction;
	}
	// GENERATOR-END: Create
}
