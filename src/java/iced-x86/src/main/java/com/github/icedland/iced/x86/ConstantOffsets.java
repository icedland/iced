// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

/**
 * Contains the offsets of the displacement and immediate.
 * <p>
 * Call the {@link com.github.icedland.iced.x86.dec.Decoder#getConstantOffsets(com.github.icedland.iced.x86.Instruction)} method to get the offsets of
 * the constants after the instruction has been decoded. The {@link com.github.icedland.iced.x86.enc.Encoder} has a similar method.
 */
public final class ConstantOffsets {
	/**
	 * The offset of the displacement, if any
	 */
	public byte displacementOffset;

	/**
	 * Size in bytes of the displacement, or 0 if there's no displacement
	 */
	public byte displacementSize;

	/**
	 * The offset of the first immediate, if any.
	 * <p>
	 * This field can be invalid even if the operand has an immediate if it's an immediate that isn't part of the instruction stream, eg.
	 * <code>SHL AL,1</code>.
	 */
	public byte immediateOffset;

	/**
	 * Size in bytes of the first immediate, or 0 if there's no immediate
	 */
	public byte immediateSize;

	/**
	 * The offset of the second immediate, if any.
	 */
	public byte immediateOffset2;

	/**
	 * Size in bytes of the second immediate, or 0 if there's no second immediate
	 */
	public byte immediateSize2;

	/**
	 * <code>true</code> if {@link #displacementOffset} and {@link #displacementSize} are valid
	 */
	public boolean hasDisplacement() {
		return displacementSize != 0;
	}

	/**
	 * <code>true</code> if {@link #immediateOffset} and {@link #immediateSize} are valid
	 */
	public boolean hasImmediate() {
		return immediateSize != 0;
	}

	/**
	 * <code>true</code> if {@link #immediateOffset2} and {@link #immediateSize2} are valid
	 */
	public boolean hasImmediate2() {
		return immediateSize2 != 0;
	}
}
