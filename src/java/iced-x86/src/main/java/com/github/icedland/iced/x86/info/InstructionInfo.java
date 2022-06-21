// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

import java.util.ArrayList;

import com.github.icedland.iced.x86.internal.IcedConstants;

/**
 * Contains information about an instruction, eg. read/written registers and memory locations, operand accesses
 */
public final class InstructionInfo {
	ArrayList<UsedRegister> usedRegisters;
	ArrayList<UsedMemory> usedMemoryLocations;
	byte[] opAccesses;

	InstructionInfo() {
		usedRegisters = new ArrayList<UsedRegister>();
		usedMemoryLocations = new ArrayList<UsedMemory>();
		opAccesses = new byte[IcedConstants.MAX_OP_COUNT];
	}

	/**
	 * Gets an iterator that returns all accessed registers. This method doesn't return all accessed registers if
	 * {@link com.github.icedland.iced.x86.Instruction#isSaveRestoreInstruction()} is {@code true}.
	 * <p>
	 * Some instructions have a {@code r16}/{@code r32} operand but only use the low 8 bits of the register. In that case this method
	 * returns the 8-bit register even if it's {@code SPL}, {@code BPL}, {@code SIL}, {@code DIL} and the instruction was decoded
	 * in 16 or 32-bit mode. This is more accurate than returning the {@code r16}/{@code r32} register. Example instructions that do this:
	 * {@code PINSRB}, {@code ARPL}
	 */
	public Iterable<UsedRegister> getUsedRegisters() {
		return usedRegisters;
	}

	/**
	 * Gets an iterator that returns all accessed memory locations
	 */
	public Iterable<UsedMemory> getUsedMemory() {
		return usedMemoryLocations;
	}

	/**
	 * Operand #0 access (an {@link com.github.icedland.iced.x86.info.OpAccess} enum variant)
	 */
	public int getOp0Access() {
		return opAccesses[0];
	}

	/**
	 * Operand #1 access (an {@link com.github.icedland.iced.x86.info.OpAccess} enum variant)
	 */
	public int getOp1Access() {
		return opAccesses[1];
	}

	/**
	 * Operand #2 access (an {@link com.github.icedland.iced.x86.info.OpAccess} enum variant)
	 */
	public int getOp2Access() {
		return opAccesses[2];
	}

	/**
	 * Operand #3 access (an {@link com.github.icedland.iced.x86.info.OpAccess} enum variant)
	 */
	public int getOp3Access() {
		return opAccesses[3];
	}

	/**
	 * Operand #4 access (an {@link com.github.icedland.iced.x86.info.OpAccess} enum variant)
	 */
	public int getOp4Access() {
		return opAccesses[4];
	}

	/**
	 * Gets operand access (an {@link com.github.icedland.iced.x86.info.OpAccess} enum variant)
	 *
	 * @param operand Operand number, 0-4
	 */
	public int getOpAccess(int operand) {
		switch (operand) {
		case 0:
			return getOp0Access();
		case 1:
			return getOp1Access();
		case 2:
			return getOp2Access();
		case 3:
			return getOp3Access();
		case 4:
			return getOp4Access();
		default:
			throw new IllegalArgumentException("operand");
		}
	}
}
