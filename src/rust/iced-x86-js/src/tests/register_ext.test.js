// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

const { Register, RegisterExt } = require("iced-x86");

test("RegisterExt funcs", () => {
	expect(RegisterExt.base(Register.DL)).toBe(Register.AL);
	expect(RegisterExt.base(Register.R8W)).toBe(Register.AX);
	expect(RegisterExt.base(Register.R15D)).toBe(Register.EAX);
	expect(RegisterExt.base(Register.R13)).toBe(Register.RAX);
	expect(RegisterExt.base(Register.FS)).toBe(Register.ES);
	expect(RegisterExt.base(Register.XMM2)).toBe(Register.XMM0);
	expect(RegisterExt.base(Register.YMM20)).toBe(Register.YMM0);
	expect(RegisterExt.base(Register.ZMM31)).toBe(Register.ZMM0);

	expect(RegisterExt.number(Register.DL)).toBe(2);
	expect(RegisterExt.number(Register.R15)).toBe(15);
	expect(RegisterExt.number(Register.YMM21)).toBe(21);

	expect(RegisterExt.fullRegister(Register.CL)).toBe(Register.RCX);
	expect(RegisterExt.fullRegister(Register.DX)).toBe(Register.RDX);
	expect(RegisterExt.fullRegister(Register.EBX)).toBe(Register.RBX);
	expect(RegisterExt.fullRegister(Register.RSP)).toBe(Register.RSP);
	expect(RegisterExt.fullRegister(Register.XMM2)).toBe(Register.ZMM2);
	expect(RegisterExt.fullRegister(Register.YMM22)).toBe(Register.ZMM22);
	expect(RegisterExt.fullRegister(Register.ZMM11)).toBe(Register.ZMM11);

	expect(RegisterExt.fullRegister32(Register.CL)).toBe(Register.ECX);
	expect(RegisterExt.fullRegister32(Register.DX)).toBe(Register.EDX);
	expect(RegisterExt.fullRegister32(Register.EBX)).toBe(Register.EBX);
	expect(RegisterExt.fullRegister32(Register.RSP)).toBe(Register.ESP);
	expect(RegisterExt.fullRegister32(Register.XMM2)).toBe(Register.ZMM2);
	expect(RegisterExt.fullRegister32(Register.YMM22)).toBe(Register.ZMM22);
	expect(RegisterExt.fullRegister32(Register.ZMM11)).toBe(Register.ZMM11);

	expect(RegisterExt.size(Register.DL)).toBe(1);
	expect(RegisterExt.size(Register.R8W)).toBe(2);
	expect(RegisterExt.size(Register.R15D)).toBe(4);
	expect(RegisterExt.size(Register.R13)).toBe(8);
	expect(RegisterExt.size(Register.FS)).toBe(2);
	expect(RegisterExt.size(Register.XMM2)).toBe(16);
	expect(RegisterExt.size(Register.YMM20)).toBe(32);
	expect(RegisterExt.size(Register.ZMM31)).toBe(64);

	expect(RegisterExt.isSegmentRegister(Register.CX)).toBe(false);
	expect(RegisterExt.isSegmentRegister(Register.GS)).toBe(true);

	expect(RegisterExt.isGPR(Register.CL)).toBe(true);
	expect(RegisterExt.isGPR(Register.DX)).toBe(true);
	expect(RegisterExt.isGPR(Register.ESP)).toBe(true);
	expect(RegisterExt.isGPR(Register.R15)).toBe(true);
	expect(RegisterExt.isGPR(Register.ES)).toBe(false);

	expect(RegisterExt.isGPR8(Register.CL)).toBe(true);
	expect(RegisterExt.isGPR8(Register.DX)).toBe(false);
	expect(RegisterExt.isGPR8(Register.ESP)).toBe(false);
	expect(RegisterExt.isGPR8(Register.R15)).toBe(false);
	expect(RegisterExt.isGPR8(Register.ES)).toBe(false);

	expect(RegisterExt.isGPR16(Register.CL)).toBe(false);
	expect(RegisterExt.isGPR16(Register.DX)).toBe(true);
	expect(RegisterExt.isGPR16(Register.ESP)).toBe(false);
	expect(RegisterExt.isGPR16(Register.R15)).toBe(false);
	expect(RegisterExt.isGPR16(Register.ES)).toBe(false);

	expect(RegisterExt.isGPR32(Register.CL)).toBe(false);
	expect(RegisterExt.isGPR32(Register.DX)).toBe(false);
	expect(RegisterExt.isGPR32(Register.ESP)).toBe(true);
	expect(RegisterExt.isGPR32(Register.R15)).toBe(false);
	expect(RegisterExt.isGPR32(Register.ES)).toBe(false);

	expect(RegisterExt.isGPR64(Register.CL)).toBe(false);
	expect(RegisterExt.isGPR64(Register.DX)).toBe(false);
	expect(RegisterExt.isGPR64(Register.ESP)).toBe(false);
	expect(RegisterExt.isGPR64(Register.R15)).toBe(true);
	expect(RegisterExt.isGPR64(Register.ES)).toBe(false);

	expect(RegisterExt.isVectorRegister(Register.CL)).toBe(false);
	expect(RegisterExt.isVectorRegister(Register.XMM1)).toBe(true);
	expect(RegisterExt.isVectorRegister(Register.YMM2)).toBe(true);
	expect(RegisterExt.isVectorRegister(Register.ZMM3)).toBe(true);

	expect(RegisterExt.isXMM(Register.CL)).toBe(false);
	expect(RegisterExt.isXMM(Register.XMM1)).toBe(true);
	expect(RegisterExt.isXMM(Register.YMM2)).toBe(false);
	expect(RegisterExt.isXMM(Register.ZMM3)).toBe(false);

	expect(RegisterExt.isYMM(Register.CL)).toBe(false);
	expect(RegisterExt.isYMM(Register.XMM1)).toBe(false);
	expect(RegisterExt.isYMM(Register.YMM2)).toBe(true);
	expect(RegisterExt.isYMM(Register.ZMM3)).toBe(false);

	expect(RegisterExt.isZMM(Register.CL)).toBe(false);
	expect(RegisterExt.isZMM(Register.XMM1)).toBe(false);
	expect(RegisterExt.isZMM(Register.YMM2)).toBe(false);
	expect(RegisterExt.isZMM(Register.ZMM3)).toBe(true);

	expect(RegisterExt.isIP(Register.CL)).toBe(false);
	expect(RegisterExt.isIP(Register.EIP)).toBe(true);
	expect(RegisterExt.isIP(Register.RIP)).toBe(true);

	expect(RegisterExt.isK(Register.CL)).toBe(false);
	expect(RegisterExt.isK(Register.K3)).toBe(true);

	expect(RegisterExt.isCR(Register.CL)).toBe(false);
	expect(RegisterExt.isCR(Register.CR3)).toBe(true);

	expect(RegisterExt.isDR(Register.CL)).toBe(false);
	expect(RegisterExt.isDR(Register.DR3)).toBe(true);

	expect(RegisterExt.isTR(Register.CL)).toBe(false);
	expect(RegisterExt.isTR(Register.TR3)).toBe(true);

	expect(RegisterExt.isST(Register.CL)).toBe(false);
	expect(RegisterExt.isST(Register.ST3)).toBe(true);

	expect(RegisterExt.isBND(Register.CL)).toBe(false);
	expect(RegisterExt.isBND(Register.BND3)).toBe(true);

	expect(RegisterExt.isMM(Register.CL)).toBe(false);
	expect(RegisterExt.isMM(Register.MM3)).toBe(true);

	expect(RegisterExt.isTMM(Register.CL)).toBe(false);
	expect(RegisterExt.isTMM(Register.TMM3)).toBe(true);
});