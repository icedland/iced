// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

/**
 * Contains the FPU <code>TOP</code> increment, whether it's conditional and whether the instruction writes to <code>TOP</code>
 */
public final class FpuStackIncrementInfo {
	/**
	 * Used if {@link #writesTop} is <code>true</code>.
	 * <p>
	 * Value added to <code>TOP</code>.
	 * <p>
	 * This is negative if it pushes one or more values and positive if it pops one or more values
	 * and <code>0</code> if it writes to <code>TOP</code> (eg. <code>FLDENV</code>, etc) without pushing/popping anything.
	 */
	public final int increment;

	/**
	 * <code>true</code> if it's a conditional push/pop (eg. <code>FPTAN</code> or <code>FSINCOS</code>)
	 */
	public final boolean conditional;

	/**
	 * <code>true</code> if <code>TOP</code> is written (it's a conditional/unconditional push/pop, <code>FNSAVE</code>, <code>FLDENV</code>, etc)
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
