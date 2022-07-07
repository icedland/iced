// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86;

import com.github.icedland.iced.x86.internal.IcedConstants;

/**
 * A register
 */
public final class Register {
	private Register() {
	}

	/**
	 * Gets register info
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static RegisterInfo getInfo(int register) {
		RegisterInfo[] infos = RegisterInfo.infos;
		return infos[register];
	}

	/**
	 * Gets the base register (a {@link Register} enum variant), eg.<!-- --> {@code AL}, {@code AX}, {@code EAX}, {@code RAX},
	 * {@code MM0}, {@code XMM0}, {@code YMM0}, {@code ZMM0}, {@code ES}
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static int getBaseRegister(int register) {
		return Register.getInfo(register).getBase();
	}

	/**
	 * The register number (index) relative to {@link #getBaseRegister(int)}, eg.<!-- --> 0-15, or 0-31, or if 8-bit GPR, 0-19
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static int getNumber(int register) {
		return Register.getInfo(register).getNumber();
	}

	/**
	 * Gets the full register (a {@link Register} enum variant) that this one is a part of, eg.<!-- --> CL/CH/CX/ECX/RCX -&gt; RCX, XMM11/YMM11/ZMM11 -&gt;
	 * ZMM11
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static int getFullRegister(int register) {
		return Register.getInfo(register).getFullRegister();
	}

	/**
	 * Gets the full register (a {@link Register} enum variant) that this one is a part of, except if it's a GPR in which case the 32-bit register is
	 * returned, eg.<!-- --> CL/CH/CX/ECX/RCX -&gt; ECX, XMM11/YMM11/ZMM11 -&gt; ZMM11
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static int getFullRegister32(int register) {
		return Register.getInfo(register).getFullRegister32();
	}

	/**
	 * Gets the size of the register in bytes
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static int getSize(int register) {
		return Register.getInfo(register).getSize();
	}

	/**
	 * Checks if it's a segment register ({@code ES}, {@code CS}, {@code SS}, {@code DS}, {@code FS}, {@code GS})
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static boolean isSegmentRegister(int register) {
		return Register.ES <= register && register <= Register.GS;
	}

	/**
	 * Checks if it's a general purpose register ({@code AL}-{@code R15L}, {@code AX}-{@code R15W},
	 * {@code EAX}-{@code R15D}, {@code RAX}-{@code R15})
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static boolean isGPR(int register) {
		return Register.AL <= register && register <= Register.R15;
	}

	/**
	 * Checks if it's an 8-bit general purpose register ({@code AL}-{@code R15L})
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static boolean isGPR8(int register) {
		return Register.AL <= register && register <= Register.R15L;
	}

	/**
	 * Checks if it's a 16-bit general purpose register ({@code AX}-{@code R15W})
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static boolean isGPR16(int register) {
		return Register.AX <= register && register <= Register.R15W;
	}

	/**
	 * Checks if it's a 32-bit general purpose register ({@code EAX}-{@code R15D})
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static boolean isGPR32(int register) {
		return Register.EAX <= register && register <= Register.R15D;
	}

	/**
	 * Checks if it's a 64-bit general purpose register ({@code RAX}-{@code R15})
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static boolean isGPR64(int register) {
		return Register.RAX <= register && register <= Register.R15;
	}

	/**
	 * Checks if it's a 128-bit vector register ({@code XMM0}-{@code XMM31})
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static boolean isXMM(int register) {
		return Register.XMM0 <= register && register <= IcedConstants.XMM_LAST;
	}

	/**
	 * Checks if it's a 256-bit vector register ({@code YMM0}-{@code YMM31})
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static boolean isYMM(int register) {
		return Register.YMM0 <= register && register <= IcedConstants.YMM_LAST;
	}

	/**
	 * Checks if it's a 512-bit vector register ({@code ZMM0}-{@code ZMM31})
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static boolean isZMM(int register) {
		return Register.ZMM0 <= register && register <= IcedConstants.ZMM_LAST;
	}

	/**
	 * Checks if it's {@code EIP}/{@code RIP}
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static boolean isIP(int register) {
		return register == Register.EIP || register == Register.RIP;
	}

	/**
	 * Checks if it's an opmask register ({@code K0}-{@code K7})
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static boolean isK(int register) {
		return Register.K0 <= register && register <= Register.K7;
	}

	/**
	 * Checks if it's a control register ({@code CR0}-{@code CR15})
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static boolean isCR(int register) {
		return Register.CR0 <= register && register <= Register.CR15;
	}

	/**
	 * Checks if it's a debug register ({@code DR0}-{@code DR15})
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static boolean isDR(int register) {
		return Register.DR0 <= register && register <= Register.DR15;
	}

	/**
	 * Checks if it's a test register ({@code TR0}-{@code TR7})
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static boolean isTR(int register) {
		return Register.TR0 <= register && register <= Register.TR7;
	}

	/**
	 * Checks if it's an FPU stack register ({@code ST0}-{@code ST7})
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static boolean isST(int register) {
		return Register.ST0 <= register && register <= Register.ST7;
	}

	/**
	 * Checks if it's a bound register ({@code BND0}-{@code BND3})
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static boolean isBND(int register) {
		return Register.BND0 <= register && register <= Register.BND3;
	}

	/**
	 * Checks if it's an MMX register ({@code MM0}-{@code MM7})
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static boolean isMM(int register) {
		return Register.MM0 <= register && register <= Register.MM7;
	}

	/**
	 * Checks if it's a tile register ({@code TMM0}-{@code TMM7})
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static boolean isTMM(int register) {
		return Register.TMM0 <= register && register <= IcedConstants.TMM_LAST;
	}

	/**
	 * Checks if it's an {@code XMM}, {@code YMM} or {@code ZMM} register
	 *
	 * @param register Register (a {@link Register} enum variant)
	 */
	public static boolean isVectorRegister(int register) {
		return Register.XMM0 <= register && register <= IcedConstants.VMM_LAST;
	}

	// GENERATOR-BEGIN: Variants
	// ⚠️This was generated by GENERATOR!🦹‍♂️
	public static final int NONE = 0;
	public static final int AL = 1;
	public static final int CL = 2;
	public static final int DL = 3;
	public static final int BL = 4;
	public static final int AH = 5;
	public static final int CH = 6;
	public static final int DH = 7;
	public static final int BH = 8;
	public static final int SPL = 9;
	public static final int BPL = 10;
	public static final int SIL = 11;
	public static final int DIL = 12;
	public static final int R8L = 13;
	public static final int R9L = 14;
	public static final int R10L = 15;
	public static final int R11L = 16;
	public static final int R12L = 17;
	public static final int R13L = 18;
	public static final int R14L = 19;
	public static final int R15L = 20;
	public static final int AX = 21;
	public static final int CX = 22;
	public static final int DX = 23;
	public static final int BX = 24;
	public static final int SP = 25;
	public static final int BP = 26;
	public static final int SI = 27;
	public static final int DI = 28;
	public static final int R8W = 29;
	public static final int R9W = 30;
	public static final int R10W = 31;
	public static final int R11W = 32;
	public static final int R12W = 33;
	public static final int R13W = 34;
	public static final int R14W = 35;
	public static final int R15W = 36;
	public static final int EAX = 37;
	public static final int ECX = 38;
	public static final int EDX = 39;
	public static final int EBX = 40;
	public static final int ESP = 41;
	public static final int EBP = 42;
	public static final int ESI = 43;
	public static final int EDI = 44;
	public static final int R8D = 45;
	public static final int R9D = 46;
	public static final int R10D = 47;
	public static final int R11D = 48;
	public static final int R12D = 49;
	public static final int R13D = 50;
	public static final int R14D = 51;
	public static final int R15D = 52;
	public static final int RAX = 53;
	public static final int RCX = 54;
	public static final int RDX = 55;
	public static final int RBX = 56;
	public static final int RSP = 57;
	public static final int RBP = 58;
	public static final int RSI = 59;
	public static final int RDI = 60;
	public static final int R8 = 61;
	public static final int R9 = 62;
	public static final int R10 = 63;
	public static final int R11 = 64;
	public static final int R12 = 65;
	public static final int R13 = 66;
	public static final int R14 = 67;
	public static final int R15 = 68;
	public static final int EIP = 69;
	public static final int RIP = 70;
	public static final int ES = 71;
	public static final int CS = 72;
	public static final int SS = 73;
	public static final int DS = 74;
	public static final int FS = 75;
	public static final int GS = 76;
	public static final int XMM0 = 77;
	public static final int XMM1 = 78;
	public static final int XMM2 = 79;
	public static final int XMM3 = 80;
	public static final int XMM4 = 81;
	public static final int XMM5 = 82;
	public static final int XMM6 = 83;
	public static final int XMM7 = 84;
	public static final int XMM8 = 85;
	public static final int XMM9 = 86;
	public static final int XMM10 = 87;
	public static final int XMM11 = 88;
	public static final int XMM12 = 89;
	public static final int XMM13 = 90;
	public static final int XMM14 = 91;
	public static final int XMM15 = 92;
	public static final int XMM16 = 93;
	public static final int XMM17 = 94;
	public static final int XMM18 = 95;
	public static final int XMM19 = 96;
	public static final int XMM20 = 97;
	public static final int XMM21 = 98;
	public static final int XMM22 = 99;
	public static final int XMM23 = 100;
	public static final int XMM24 = 101;
	public static final int XMM25 = 102;
	public static final int XMM26 = 103;
	public static final int XMM27 = 104;
	public static final int XMM28 = 105;
	public static final int XMM29 = 106;
	public static final int XMM30 = 107;
	public static final int XMM31 = 108;
	public static final int YMM0 = 109;
	public static final int YMM1 = 110;
	public static final int YMM2 = 111;
	public static final int YMM3 = 112;
	public static final int YMM4 = 113;
	public static final int YMM5 = 114;
	public static final int YMM6 = 115;
	public static final int YMM7 = 116;
	public static final int YMM8 = 117;
	public static final int YMM9 = 118;
	public static final int YMM10 = 119;
	public static final int YMM11 = 120;
	public static final int YMM12 = 121;
	public static final int YMM13 = 122;
	public static final int YMM14 = 123;
	public static final int YMM15 = 124;
	public static final int YMM16 = 125;
	public static final int YMM17 = 126;
	public static final int YMM18 = 127;
	public static final int YMM19 = 128;
	public static final int YMM20 = 129;
	public static final int YMM21 = 130;
	public static final int YMM22 = 131;
	public static final int YMM23 = 132;
	public static final int YMM24 = 133;
	public static final int YMM25 = 134;
	public static final int YMM26 = 135;
	public static final int YMM27 = 136;
	public static final int YMM28 = 137;
	public static final int YMM29 = 138;
	public static final int YMM30 = 139;
	public static final int YMM31 = 140;
	public static final int ZMM0 = 141;
	public static final int ZMM1 = 142;
	public static final int ZMM2 = 143;
	public static final int ZMM3 = 144;
	public static final int ZMM4 = 145;
	public static final int ZMM5 = 146;
	public static final int ZMM6 = 147;
	public static final int ZMM7 = 148;
	public static final int ZMM8 = 149;
	public static final int ZMM9 = 150;
	public static final int ZMM10 = 151;
	public static final int ZMM11 = 152;
	public static final int ZMM12 = 153;
	public static final int ZMM13 = 154;
	public static final int ZMM14 = 155;
	public static final int ZMM15 = 156;
	public static final int ZMM16 = 157;
	public static final int ZMM17 = 158;
	public static final int ZMM18 = 159;
	public static final int ZMM19 = 160;
	public static final int ZMM20 = 161;
	public static final int ZMM21 = 162;
	public static final int ZMM22 = 163;
	public static final int ZMM23 = 164;
	public static final int ZMM24 = 165;
	public static final int ZMM25 = 166;
	public static final int ZMM26 = 167;
	public static final int ZMM27 = 168;
	public static final int ZMM28 = 169;
	public static final int ZMM29 = 170;
	public static final int ZMM30 = 171;
	public static final int ZMM31 = 172;
	public static final int K0 = 173;
	public static final int K1 = 174;
	public static final int K2 = 175;
	public static final int K3 = 176;
	public static final int K4 = 177;
	public static final int K5 = 178;
	public static final int K6 = 179;
	public static final int K7 = 180;
	public static final int BND0 = 181;
	public static final int BND1 = 182;
	public static final int BND2 = 183;
	public static final int BND3 = 184;
	public static final int CR0 = 185;
	public static final int CR1 = 186;
	public static final int CR2 = 187;
	public static final int CR3 = 188;
	public static final int CR4 = 189;
	public static final int CR5 = 190;
	public static final int CR6 = 191;
	public static final int CR7 = 192;
	public static final int CR8 = 193;
	public static final int CR9 = 194;
	public static final int CR10 = 195;
	public static final int CR11 = 196;
	public static final int CR12 = 197;
	public static final int CR13 = 198;
	public static final int CR14 = 199;
	public static final int CR15 = 200;
	public static final int DR0 = 201;
	public static final int DR1 = 202;
	public static final int DR2 = 203;
	public static final int DR3 = 204;
	public static final int DR4 = 205;
	public static final int DR5 = 206;
	public static final int DR6 = 207;
	public static final int DR7 = 208;
	public static final int DR8 = 209;
	public static final int DR9 = 210;
	public static final int DR10 = 211;
	public static final int DR11 = 212;
	public static final int DR12 = 213;
	public static final int DR13 = 214;
	public static final int DR14 = 215;
	public static final int DR15 = 216;
	public static final int ST0 = 217;
	public static final int ST1 = 218;
	public static final int ST2 = 219;
	public static final int ST3 = 220;
	public static final int ST4 = 221;
	public static final int ST5 = 222;
	public static final int ST6 = 223;
	public static final int ST7 = 224;
	public static final int MM0 = 225;
	public static final int MM1 = 226;
	public static final int MM2 = 227;
	public static final int MM3 = 228;
	public static final int MM4 = 229;
	public static final int MM5 = 230;
	public static final int MM6 = 231;
	public static final int MM7 = 232;
	public static final int TR0 = 233;
	public static final int TR1 = 234;
	public static final int TR2 = 235;
	public static final int TR3 = 236;
	public static final int TR4 = 237;
	public static final int TR5 = 238;
	public static final int TR6 = 239;
	public static final int TR7 = 240;
	public static final int TMM0 = 241;
	public static final int TMM1 = 242;
	public static final int TMM2 = 243;
	public static final int TMM3 = 244;
	public static final int TMM4 = 245;
	public static final int TMM5 = 246;
	public static final int TMM6 = 247;
	public static final int TMM7 = 248;
	/**
	 * Don't use it!
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public static final int DONTUSE0 = 249;
	/**
	 * Don't use it!
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public static final int DONTUSEFA = 250;
	/**
	 * Don't use it!
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public static final int DONTUSEFB = 251;
	/**
	 * Don't use it!
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public static final int DONTUSEFC = 252;
	/**
	 * Don't use it!
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public static final int DONTUSEFD = 253;
	/**
	 * Don't use it!
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public static final int DONTUSEFE = 254;
	/**
	 * Don't use it!
	 *
	 * @deprecated Not part of the public API
	 */
	@Deprecated
	public static final int DONTUSEFF = 255;
	// GENERATOR-END: Variants
}
