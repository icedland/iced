// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.enc;

import java.util.ArrayList;

import com.github.icedland.iced.x86.ConstantOffsets;

/**
 * {@link BlockEncoder} result if it was successful
 */
public final class BlockEncoderResult {
	private static final int[] emptyIntArray = new int[0];
	private static final ConstantOffsets[] emptyConstantOffsets = new ConstantOffsets[0];

	/**
	 * Base IP of all encoded instructions
	 */
	public final long rip;

	/**
	 * If {@link BlockEncoderOptions#RETURN_RELOC_INFOS} option was enabled:
	 * <p>
	 * All {@link RelocInfo}s
	 * <p>
	 * else this will be {@code null}
	 */
	public final ArrayList<RelocInfo> relocInfos;

	/**
	 * If {@link BlockEncoderOptions#RETURN_NEW_INSTRUCTION_OFFSETS} option was enabled:
	 * <p>
	 * Offsets of the instructions relative to the base IP. If the instruction was rewritten to a new instruction
	 * (eg. {@code JE TARGET_TOO_FAR_AWAY} -&gt; {@code JNE SHORT SKIP ; JMP QWORD PTR [MEM]}),
	 * the value {@code 0xFFFF_FFFF} is stored in that array element.
	 */
	public final int[] newInstructionOffsets;

	/**
	 * If {@link BlockEncoderOptions#RETURN_CONSTANT_OFFSETS} option was enabled:
	 * <p>
	 * Offsets of all constants in the new encoded instructions. If the instruction was rewritten,
	 * the 'default' value is stored in the corresponding array element.
	 */
	public final ConstantOffsets[] constantOffsets;

	BlockEncoderResult(long rip, ArrayList<RelocInfo> relocInfos, int[] newInstructionOffsets, ConstantOffsets[] constantOffsets) {
		if (newInstructionOffsets == null)
			newInstructionOffsets = emptyIntArray;
		if (constantOffsets == null)
			constantOffsets = emptyConstantOffsets;
		this.rip = rip;
		this.relocInfos = relocInfos;
		this.newInstructionOffsets = newInstructionOffsets;
		this.constantOffsets = constantOffsets;
	}
}
