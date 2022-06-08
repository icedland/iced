// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

/**
 * DO NOT USE: INTERNAL API
 *
 * @deprecated Not part of the public API
 */
@Deprecated
public final class InternalInstructionUtils {
	private InternalInstructionUtils() {
	}

	/**
	 * DO NOT USE: INTERNAL API
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public static int getAddressSizeInBytes(int baseReg, int indexReg, int displSize, int codeSize) {
		if ((Register.RAX <= baseReg && baseReg <= Register.R15) || (Register.RAX <= indexReg && indexReg <= Register.R15) || baseReg == Register.RIP)
			return 8;
		if ((Register.EAX <= baseReg && baseReg <= Register.R15D) || (Register.EAX <= indexReg && indexReg <= Register.R15D) || baseReg == Register.EIP)
			return 4;
		if ((Register.AX <= baseReg && baseReg <= Register.DI) || (Register.AX <= indexReg && indexReg <= Register.DI))
			return 2;
		if (displSize == 2 || displSize == 4 || displSize == 8)
			return displSize;

		switch (codeSize) {
		case CodeSize.CODE64:
			return 8;
		case CodeSize.CODE32:
			return 4;
		case CodeSize.CODE16:
			return 2;
		default:
			return 8;
		}
	}
}
