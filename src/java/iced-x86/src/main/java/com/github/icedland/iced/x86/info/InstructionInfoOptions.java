// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.info;

/**
 * Instruction info options used by {@link InstructionInfoFactory}
 */
public final class InstructionInfoOptions {
	private InstructionInfoOptions() {
	}

	/**
	 * No option is enabled
	 */
	public static final int NONE = 0;

	/**
	 * Don't include memory usage, i.e., {@link InstructionInfo#getUsedMemory()} will return an empty iterator. All
	 * registers that are used by memory operands are still returned by {@link InstructionInfo#getUsedRegisters()}.
	 */
	public static final int NO_MEMORY_USAGE = 0x0000_0001;

	/**
	 * Don't include register usage, i.e., {@link InstructionInfo#getUsedRegisters()} will return an empty iterator
	 */
	public static final int NO_REGISTER_USAGE = 0x0000_0002;
}
