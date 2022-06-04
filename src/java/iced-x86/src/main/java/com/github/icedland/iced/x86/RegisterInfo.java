// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

import com.github.icedland.iced.x86.internal.IcedConstants;

/**
 * {@link Register} information
 */
public final class RegisterInfo {
	static final RegisterInfo[] infos = createRegisterInfos();
	final byte register;
	final byte baseRegister;
	final byte fullRegister;
	final short size;

	static RegisterInfo[] createRegisterInfos() {
		RegisterInfo[] regInfos = new RegisterInfo[IcedConstants.REGISTER_ENUM_COUNT];

		regInfos[(int)Register.EIP] = new RegisterInfo(Register.EIP, Register.EIP, Register.RIP, 4);
		regInfos[(int)Register.RIP] = new RegisterInfo(Register.RIP, Register.EIP, Register.RIP, 8);

		@SuppressWarnings("deprecation")
		byte[] data = new byte[] {
			(byte)Register.AL, (byte)Register.R15L, (byte)Register.RAX, 1, 0,
			(byte)Register.AX, (byte)Register.R15W, (byte)Register.RAX, 2, 0,
			(byte)Register.EAX, (byte)Register.R15D, (byte)Register.RAX, 4, 0,
			(byte)Register.RAX, (byte)Register.R15, (byte)Register.RAX, 8, 0,
			(byte)Register.ES, (byte)Register.GS, (byte)Register.ES, 2, 0,
			(byte)Register.XMM0, (byte)Register.XMM31, (byte)Register.ZMM0, 16, 0,
			(byte)Register.YMM0, (byte)Register.YMM31, (byte)Register.ZMM0, 32, 0,
			(byte)Register.ZMM0, (byte)Register.ZMM31, (byte)Register.ZMM0, 64, 0,
			(byte)Register.K0, (byte)Register.K7, (byte)Register.K0, 8, 0,
			(byte)Register.BND0, (byte)Register.BND3, (byte)Register.BND0, 16, 0,
			(byte)Register.CR0, (byte)Register.CR15, (byte)Register.CR0, 8, 0,
			(byte)Register.DR0, (byte)Register.DR15, (byte)Register.DR0, 8, 0,
			(byte)Register.ST0, (byte)Register.ST7, (byte)Register.ST0, 10, 0,
			(byte)Register.MM0, (byte)Register.MM7, (byte)Register.MM0, 8, 0,
			(byte)Register.TR0, (byte)Register.TR7, (byte)Register.TR0, 4, 0,
			(byte)Register.TMM0, (byte)Register.TMM7, (byte)Register.TMM0, 0, 4,
			(byte)Register.DONTUSE0, (byte)Register.DONTUSE0, (byte)Register.DONTUSE0, 0, 0,
			(byte)Register.DONTUSEFA, (byte)Register.DONTUSEFF, (byte)Register.DONTUSEFA, 0, 0,
		};

		int i;
		for (i = 0; i < data.length; i += 5) {
			int baseReg = data[i] & 0xFF;
			int reg = baseReg;
			int regEnd = data[i + 1] & 0xFF;
			int fullReg = data[i + 2] & 0xFF;
			int size = (data[i + 3] & 0xFF) | ((data[i + 4] & 0xFF) << 8);
			while (reg <= regEnd) {
				regInfos[(int)reg] = new RegisterInfo(reg, baseReg, fullReg, size);
				reg++;
				fullReg++;
				if (reg == Register.AH)
					fullReg -= 4;
			}
		}
		if (i != data.length)
			throw new UnsupportedOperationException();

		return regInfos;
	}

	/**
	 * Gets the register (a {@link Register} enum variant)
	 */
	public int getRegister() {
		return register;
	}

	/**
	 * Gets the base register (a {@link Register} enum variant), eg.<!-- --> <code>AL</code>, <code>AX</code>, <code>EAX</code>, <code>RAX</code>,
	 * <code>MM0</code>, <code>XMM0</code>, <code>YMM0</code>, <code>ZMM0</code>, <code>ES</code>
	 */
	public int getBase() {
		return baseRegister;
	}

	/**
	 * The register number (index) relative to {@link #getBase()}, eg.<!-- --> 0-15, or 0-31, or if 8-bit GPR, 0-19
	 */
	public int getNumber() {
		return register - baseRegister;
	}

	/**
	 * The full register (a {@link Register} enum variant) that this one is a part of, eg.<!-- -->
	 * <code>CL</code>/<code>CH</code>/<code>CX</code>/<code>ECX</code>/<code>RCX</code> -> <code>RCX</code>,
	 * <code>XMM11</code>/<code>YMM11</code>/<code>ZMM11</code> -> <code>ZMM11</code>
	 */
	public int getFullRegister() {
		return fullRegister;
	}

	/**
	 * Gets the full register (a {@link Register} enum variant) that this one is a part of, except if it's a GPR in which case the 32-bit register is
	 * returned, eg.<!-- --> <code>CL</code>/<code>CH</code>/<code>CX</code>/<code>ECX</code>/<code>RCX</code> -> <code>ECX</code>,
	 * <code>XMM11</code>/<code>YMM11</code>/<code>ZMM11</code> -> <code>ZMM11</code>
	 */
	public int getFullRegister32() {
		int fullRegister = (int)this.fullRegister;
		if (Register.isGPR(fullRegister)) {
			assert Register.RAX <= fullRegister && fullRegister <= Register.R15 : fullRegister;
			return fullRegister - Register.RAX + Register.EAX;
		}
		return fullRegister;
	}

	/**
	 * Size of the register in bytes
	 */
	public int getSize() {
		return size;
	}

	private RegisterInfo(int register, int baseRegister, int fullRegister, int size) {
		assert baseRegister <= register : baseRegister;
		assert register >= 0 && register <= 0xFF : register;
		this.register = (byte)register;
		assert baseRegister >= 0 && baseRegister <= 0xFF : baseRegister;
		this.baseRegister = (byte)baseRegister;
		assert fullRegister >= 0 && fullRegister <= 0xFF : fullRegister;
		this.fullRegister = (byte)fullRegister;
		assert size >= 0 && size <= 0xFFFF : size;
		this.size = (short)size;
	}
}
