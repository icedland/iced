// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.asm;

import com.github.icedland.iced.x86.RoundingControl;

/**
 * Assembler operand flags.
 */
public final class AsmOperandFlags {
	private AsmOperandFlags() {
	}

	/**
	 * No flags.
	 */
	public static final int NONE = 0;
	/**
	 * Broadcast.
	 */
	public static final int BROADCAST = 1;
	/**
	 * Zeroing mask.
	 */
	public static final int ZEROING = 1 << 1;
	/**
	 * Suppress all exceptions ({@code .sae()}).
	 */
	public static final int SUPPRESS_ALL_EXCEPTIONS = 1 << 2;
	/**
	 * Round to nearest ({@code .rn_sae()}).
	 */
	public static final int ROUND_TO_NEAREST = RoundingControl.ROUND_TO_NEAREST << 3;
	/**
	 * Round to down ({@code .rd_sae()}).
	 */
	public static final int ROUND_DOWN = RoundingControl.ROUND_DOWN << 3;
	/**
	 * Round to up ({@code .ru_sae()}).
	 */
	public static final int ROUND_UP = RoundingControl.ROUND_UP << 3;
	/**
	 * Round towards zero ({@code .rz_sae()}).
	 */
	public static final int ROUND_TOWARD_ZERO = RoundingControl.ROUND_TOWARD_ZERO << 3;
	/**
	 * RoundingControl mask.
	 */
	public static final int ROUNDING_CONTROL_MASK = 0x7 << 3;
	/**
	 * Mask register K1.
	 */
	public static final int K1 = 1 << 6;
	/**
	 * Mask register K2.
	 */
	public static final int K2 = 2 << 6;
	/**
	 * Mask register K3.
	 */
	public static final int K3 = 3 << 6;
	/**
	 * Mask register K4.
	 */
	public static final int K4 = 4 << 6;
	/**
	 * Mask register K5.
	 */
	public static final int K5 = 5 << 6;
	/**
	 * Mask register K6.
	 */
	public static final int K6 = 6 << 6;
	/**
	 * Mask register K7.
	 */
	public static final int K7 = 7 << 6;
	/**
	 * Mask for K registers.
	 */
	public static final int REGISTER_MASK = 0x7 << 6;
}
