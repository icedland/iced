// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

/**
 * Contains the FPU {@code TOP} increment, whether it's conditional and whether the instruction writes to {@code TOP}
 */
public final class FpuStackIncrementInfo {
	/**
	 * Used if {@link #writesTop} is {@code true}.
	 * <p>
	 * Value added to {@code TOP}.
	 * <p>
	 * This is negative if it pushes one or more values and positive if it pops one or more values
	 * and {@code 0} if it writes to {@code TOP} (eg. {@code FLDENV}, etc) without pushing/popping anything.
	 */
	public final int increment;

	/**
	 * {@code true} if it's a conditional push/pop (eg. {@code FPTAN} or {@code FSINCOS})
	 */
	public final boolean conditional;

	/**
	 * {@code true} if {@code TOP} is written (it's a conditional/unconditional push/pop, {@code FNSAVE}, {@code FLDENV}, etc)
	 */
	public final boolean writesTop;

	/**
	 * Constructor
	 */
	public FpuStackIncrementInfo(int increment, boolean conditional, boolean writesTop) {
		this.increment = increment;
		this.conditional = conditional;
		this.writesTop = writesTop;
	}
}
