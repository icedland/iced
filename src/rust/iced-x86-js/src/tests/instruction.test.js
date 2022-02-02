// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

const {
	Code, CodeSize, ConditionCode, CpuidFeature, Decoder, DecoderOptions, EncodingKind,
	FlowControl, getIcedFeatures, Instruction, MemoryOperand, MemorySize, Mnemonic, OpKind, Register,
	RepPrefixKind, RflagsBits, RoundingControl
} = require("iced-x86");

test("Call Instruction ctor", () => {
	const instr = new Instruction();

	expect(instr.code).toBe(Code.INVALID);
	expect(instr.ip).toBe(0n);
	expect(instr.length).toBe(0);

	instr.free();
});

test("Instruction props", () => {
	// xchg ah,[rdx+rsi+16h]
	const bytes = new Uint8Array([0x86, 0x64, 0x32, 0x16]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);
	decoder.ip = 0x123456789ABCDEF1n;
	const instr = decoder.decode();

	expect(instr.ip).toBe(0x123456789ABCDEF1n);
	expect(instr.ip32).toBe(0x9ABCDEF1);
	expect(instr.ip16).toBe(0xDEF1);
	expect(instr.length).toBe(4);
	expect(instr.nextIP).toBe(0x123456789ABCDEF5n);
	expect(instr.nextIP32).toBe(0x9ABCDEF5);
	expect(instr.nextIP16).toBe(0xDEF5);
	expect(instr.codeSize).toBe(CodeSize.Code64);
	expect(instr.code).toBe(Code.Xchg_rm8_r8);
	expect(instr.mnemonic).toBe(Mnemonic.Xchg);
	expect(instr.opCount).toBe(2);
	expect(instr.hasXacquirePrefix).toBe(false);
	expect(instr.hasXreleasePrefix).toBe(false);
	expect(instr.hasRepPrefix).toBe(false);
	expect(instr.hasRepePrefix).toBe(false);
	expect(instr.hasRepnePrefix).toBe(false);
	expect(instr.hasLockPrefix).toBe(false);
	expect(instr.op0Kind).toBe(OpKind.Memory);
	expect(instr.op1Kind).toBe(OpKind.Register);
	expect(instr.opKind(0)).toBe(OpKind.Memory);
	expect(instr.opKind(1)).toBe(OpKind.Register);
	expect(instr.hasSegmentPrefix).toBe(false);
	expect(instr.segmentPrefix).toBe(Register.None);
	expect(instr.memorySegment).toBe(Register.DS);
	expect(instr.memoryDisplSize).toBe(1);
	expect(instr.isBroadcast).toBe(false);
	expect(instr.memorySize).toBe(MemorySize.UInt8);
	// Check if MVEX support
	if ((getIcedFeatures() & 0x10) != 0) {
		const { MvexRegMemConv } = require("iced-x86");
		expect(instr.isMvexEvictionHint).toBe(false);
		expect(instr.mvexRegMemConv).toBe(MvexRegMemConv.None);
	}
	expect(instr.memoryIndexScale).toBe(1);
	expect(instr.memoryDisplacement).toBe(0x16n);
	expect(instr.memoryDisplacementU64).toBe(0x16n);
	expect(instr.memoryBase).toBe(Register.RDX);
	expect(instr.memoryIndex).toBe(Register.RSI);
	expect(instr.op1Register).toBe(Register.AH);
	expect(instr.opRegister(1)).toBe(Register.AH);
	expect(instr.opMask).toBe(Register.None);
	expect(instr.hasOpMask).toBe(false);
	expect(instr.zeroingMasking).toBe(false);
	expect(instr.roundingControl).toBe(RoundingControl.None);
	expect(instr.isVsib).toBe(false);
	expect(instr.isVsib32).toBe(false);
	expect(instr.isVsib64).toBe(false);
	expect(instr.vsib).toBe(undefined);
	expect(instr.suppressAllExceptions).toBe(false);
	expect(instr.isIpRelMemoryOperand).toBe(false);
	expect(instr.stackPointerIncrement).toBe(0);
	expect(instr.encoding).toBe(EncodingKind.Legacy);
	expect(instr.cpuidFeatures()).toStrictEqual(new Int32Array([CpuidFeature.INTEL8086]));
	expect(instr.flowControl).toBe(FlowControl.Next);
	expect(instr.isPrivileged).toBe(false);
	expect(instr.isStackInstruction).toBe(false);
	expect(instr.isSaveRestoreInstruction).toBe(false);
	expect(instr.rflagsRead).toBe(RflagsBits.None);
	expect(instr.rflagsWritten).toBe(RflagsBits.None);
	expect(instr.rflagsCleared).toBe(RflagsBits.None);
	expect(instr.rflagsSet).toBe(RflagsBits.None);
	expect(instr.rflagsUndefined).toBe(RflagsBits.None);
	expect(instr.rflagsModified).toBe(RflagsBits.None);
	expect(instr.isJccShortOrNear).toBe(false);
	expect(instr.isJccNear).toBe(false);
	expect(instr.isJccShort).toBe(false);
	expect(instr.isJmpShort).toBe(false);
	expect(instr.isJmpNear).toBe(false);
	expect(instr.isJmpShortOrNear).toBe(false);
	expect(instr.isJmpFar).toBe(false);
	expect(instr.isCallNear).toBe(false);
	expect(instr.isCallFar).toBe(false);
	expect(instr.isJmpNearIndirect).toBe(false);
	expect(instr.isJmpFarIndirect).toBe(false);
	expect(instr.isCallNearIndirect).toBe(false);
	expect(instr.isCallFarIndirect).toBe(false);
	// Check if MVEX support
	if ((getIcedFeatures() & 0x10) != 0) {
		expect(instr.isJkccShortOrNear).toBe(false);
		expect(instr.isJkccNear).toBe(false);
		expect(instr.isJkccShort).toBe(false);
	}
	expect(instr.isJcxShort).toBe(false);
	expect(instr.isLoopcc).toBe(false);
	expect(instr.isLoop).toBe(false);
	expect(instr.conditionCode).toBe(ConditionCode.None);
	expect(instr.isStringInstruction).toBe(false);

	const instr2 = instr.clone();
	expect(instr).not.toBe(instr2);
	expect(instr.equalsAllBits(instr2)).toBe(true);
	expect(instr.equals(instr2)).toBe(true);

	expect(instr.toString()).toBe("xchg ah,[rdx+rsi+16h]");

	decoder.free();
	instr.free();
	instr2.free();
});

test("Near branch instr", () => {
	const bytes = new Uint8Array([0x70, 0x02]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);
	decoder.ip = 0x123456789ABCDEF1n;
	const instr = decoder.decode();

	expect(instr.nearBranchTarget).toBe(0x123456789ABCDEF5n);
	expect(instr.nearBranch64).toBe(0x123456789ABCDEF5n);
	expect(instr.nearBranch32).toBe(0x9ABCDEF5);
	expect(instr.nearBranch16).toBe(0xDEF5);

	expect(instr.code).toBe(Code.Jo_rel8_64);
	expect(instr.conditionCode).toBe(ConditionCode.o);
	instr.negateConditionCode();
	expect(instr.code).toBe(Code.Jno_rel8_64);
	expect(instr.conditionCode).toBe(ConditionCode.no);
	instr.asNearBranch();
	expect(instr.code).toBe(Code.Jno_rel32_64);
	instr.asShortBranch();
	expect(instr.code).toBe(Code.Jno_rel8_64);

	decoder.free();
	instr.free();
});

test("Far branch instr", () => {
	const bytes = new Uint8Array([0x9A, 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC]);
	const decoder = new Decoder(32, bytes, DecoderOptions.None);
	decoder.ip = 0x9ABCDEF1n;
	const instr = decoder.decode();

	expect(instr.code).toBe(Code.Call_ptr1632);
	expect(instr.opCount).toBe(1);
	expect(instr.op0Kind).toBe(OpKind.FarBranch32);
	expect(instr.farBranchSelector).toBe(0xBC9A);
	expect(instr.farBranch32).toBe(0x78563412);
	expect(instr.farBranch16).toBe(0x3412);

	decoder.free();
	instr.free();
});

test("Instr u32 u32", () => {
	const bytes = new Uint8Array([0x66, 0xC8, 0x5A, 0xA5, 0xA6]);
	const decoder = new Decoder(32, bytes, DecoderOptions.None);
	const instr = decoder.decode();

	expect(instr.opCount).toBe(2);
	expect(instr.op0Kind).toBe(OpKind.Immediate16);
	expect(instr.op1Kind).toBe(OpKind.Immediate8_2nd);
	expect(instr.immediate16).toBe(0xA55A);
	expect(instr.immediate8_2nd).toBe(0xA6);

	decoder.free();
	instr.free();
})

test("Instr u8", () => {
	const bytes = new Uint8Array([0xC6, 0xF8, 0x5A]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);
	const instr = decoder.decode();

	expect(instr.opCount).toBe(1);
	expect(instr.op0Kind).toBe(OpKind.Immediate8);
	expect(instr.immediate8).toBe(0x5A);
	expect(instr.immediate(0)).toBe(0x5An);

	decoder.free();
	instr.free();
});

test("Instr u32", () => {
	const bytes = new Uint8Array([0x68, 0x5A, 0xA5, 0x12, 0x34]);
	const decoder = new Decoder(32, bytes, DecoderOptions.None);
	const instr = decoder.decode();

	expect(instr.opCount).toBe(1);
	expect(instr.op0Kind).toBe(OpKind.Immediate32);
	expect(instr.immediate32).toBe(0x3412A55A);
	expect(instr.immediate(0)).toBe(0x3412A55An);

	decoder.free();
	instr.free();
});

test("Instr mem64", () => {
	const bytes = new Uint8Array([0x64, 0xA2, 0x12, 0x34, 0x56, 0x78, 0x9A, 0xBC, 0xDE, 0xF0]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);
	const instr = decoder.decode();

	expect(instr.opCount).toBe(2);
	expect(instr.op0Kind).toBe(OpKind.Memory);
	expect(instr.op1Kind).toBe(OpKind.Register);
	expect(instr.memoryDisplacement).toBe(-0xF21436587A9CBEEn);
	expect(instr.memoryDisplacementU64).toBe(0xF0DEBC9A78563412n);
	expect(instr.op1Register).toBe(Register.AL);
	expect(instr.memorySegment).toBe(Register.FS);
	expect(instr.segmentPrefix).toBe(Register.FS);

	decoder.free();
	instr.free();
});

function hexToBin(c) {
	if (c >= 0x30 && c <= 0x39)
		return c - 0x30;
	if (c >= 0x41 && c <= 0x46)
		return c - 0x41 + 10;
	if (c >= 0x61 && c <= 0x66)
		return c - 0x61 + 10;
	throw new Error("Invalid hex char");
}

function parseHex(s) {
	const bytes = [];
	if ((s.length & 1) != 0)
		throw new Error("Invalid hex string");
	for (let i = 0; i < s.length; i += 2) {
		const hi = hexToBin(s.charCodeAt(i));
		const lo = hexToBin(s.charCodeAt(i + 1));
		bytes.push((hi << 4) | lo);
	}
	return new Uint8Array(bytes);
}

test("Instruction.create*()", () => {
	const data = [
		[64, "90", DecoderOptions.None, Instruction.create(Code.Nopd)],
		[64, "48B9FFFFFFFFFFFFFFFF", DecoderOptions.None, Instruction.createRegI64(Code.Mov_r64_imm64, Register.RCX, 0xFFFFFFFFFFFFFFFFn)],
		[64, "48B9FFFFFFFFFFFFFFFF", DecoderOptions.None, Instruction.createRegI32(Code.Mov_r64_imm64, Register.RCX, -1)],
		[64, "48B9123456789ABCDE31", DecoderOptions.None, Instruction.createRegU64(Code.Mov_r64_imm64, Register.RCX, 0x31DEBC9A78563412n)],
		[64, "48B9FFFFFFFF00000000", DecoderOptions.None, Instruction.createRegU32(Code.Mov_r64_imm64, Register.RCX, 0xFFFFFFFF)],
		[64, "8FC1", DecoderOptions.None, Instruction.createReg(Code.Pop_rm64, Register.RCX)],
		[64, "648F847501EFCDAB", DecoderOptions.None, Instruction.createMem(Code.Pop_rm64, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS))],
		[64, "C6F85A", DecoderOptions.None, Instruction.createU32(Code.Xabort_imm8, 0x5A)],
		[64, "66685AA5", DecoderOptions.None, Instruction.createI32(Code.Push_imm16, 0xA55A)],
		[32, "685AA51234", DecoderOptions.None, Instruction.createI32(Code.Pushd_imm32, 0x3412A55A)],
		[64, "666A5A", DecoderOptions.None, Instruction.createI32(Code.Pushw_imm8, 0x5A)],
		[32, "6A5A", DecoderOptions.None, Instruction.createI32(Code.Pushd_imm8, 0x5A)],
		[64, "6A5A", DecoderOptions.None, Instruction.createI32(Code.Pushq_imm8, 0x5A)],
		[64, "685AA512A4", DecoderOptions.None, Instruction.createI32(Code.Pushq_imm32, -0x5BED5AA6)],
		[32, "66705A", DecoderOptions.None, Instruction.createBranch(Code.Jo_rel8_16, 0x4Dn)],
		[32, "705A", DecoderOptions.None, Instruction.createBranch(Code.Jo_rel8_32, 0x8000004Cn)],
		[64, "705A", DecoderOptions.None, Instruction.createBranch(Code.Jo_rel8_64, 0x800000000000004Cn)],
		[32, "669A12345678", DecoderOptions.None, Instruction.createFarBranch(Code.Call_ptr1616, 0x7856, 0x3412)],
		[32, "9A123456789ABC", DecoderOptions.None, Instruction.createFarBranch(Code.Call_ptr1632, 0xBC9A, 0x78563412)],
		[16, "C7F85AA5", DecoderOptions.None, Instruction.createXbegin(16, 0x254En)],
		[32, "C7F85AA51234", DecoderOptions.None, Instruction.createXbegin(32, 0xB412A550n)],
		[64, "C7F85AA51234", DecoderOptions.None, Instruction.createXbegin(64, 0x800000003412A550n)],
		[64, "00D1", DecoderOptions.None, Instruction.createRegReg(Code.Add_rm8_r8, Register.CL, Register.DL)],
		[64, "64028C7501EFCDAB", DecoderOptions.None, Instruction.createRegMem(Code.Add_r8_rm8, Register.CL, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS))],
		[64, "80C15A", DecoderOptions.None, Instruction.createRegI32(Code.Add_rm8_imm8, Register.CL, 0x5A)],
		[64, "6681C15AA5", DecoderOptions.None, Instruction.createRegI32(Code.Add_rm16_imm16, Register.CX, 0xA55A)],
		[64, "81C15AA51234", DecoderOptions.None, Instruction.createRegI32(Code.Add_rm32_imm32, Register.ECX, 0x3412A55A)],
		[64, "48B904152637A55A5678", DecoderOptions.None, Instruction.createRegU64(Code.Mov_r64_imm64, Register.RCX, 0x78565AA537261504n)],
		[64, "6683C15A", DecoderOptions.None, Instruction.createRegI32(Code.Add_rm16_imm8, Register.CX, 0x5A)],
		[64, "83C15A", DecoderOptions.None, Instruction.createRegI32(Code.Add_rm32_imm8, Register.ECX, 0x5A)],
		[64, "4883C15A", DecoderOptions.None, Instruction.createRegI32(Code.Add_rm64_imm8, Register.RCX, 0x5A)],
		[64, "4881C15AA51234", DecoderOptions.None, Instruction.createRegI32(Code.Add_rm64_imm32, Register.RCX, 0x3412A55A)],
		[64, "64A0123456789ABCDEF0", DecoderOptions.None, Instruction.createRegMem(Code.Mov_AL_moffs8, Register.AL, MemoryOperand.new64(Register.None, Register.None, 1, 0xF0DEBC9A78563412n, 8, false, Register.FS))],
		[64, "6400947501EFCDAB", DecoderOptions.None, Instruction.createMemReg(Code.Add_rm8_r8, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), Register.DL)],
		[64, "6480847501EFCDAB5A", DecoderOptions.None, Instruction.createMemI32(Code.Add_rm8_imm8, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), 0x5A)],
		[64, "646681847501EFCDAB5AA5", DecoderOptions.None, Instruction.createMemU32(Code.Add_rm16_imm16, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), 0xA55A)],
		[64, "6481847501EFCDAB5AA51234", DecoderOptions.None, Instruction.createMemI32(Code.Add_rm32_imm32, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), 0x3412A55A)],
		[64, "646683847501EFCDAB5A", DecoderOptions.None, Instruction.createMemI32(Code.Add_rm16_imm8, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), 0x5A)],
		[64, "6483847501EFCDAB5A", DecoderOptions.None, Instruction.createMemI32(Code.Add_rm32_imm8, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), 0x5A)],
		[64, "644883847501EFCDAB5A", DecoderOptions.None, Instruction.createMemI32(Code.Add_rm64_imm8, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), 0x5A)],
		[64, "644881847501EFCDAB5AA51234", DecoderOptions.None, Instruction.createMemI32(Code.Add_rm64_imm32, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), 0x3412A55A)],
		[64, "E65A", DecoderOptions.None, Instruction.createI32Reg(Code.Out_imm8_AL, 0x5A, Register.AL)],
		[64, "E65A", DecoderOptions.None, Instruction.createU32Reg(Code.Out_imm8_AL, 0x5A, Register.AL)],
		[64, "66C85AA5A6", DecoderOptions.None, Instruction.createI32I32(Code.Enterw_imm16_imm8, 0xA55A, 0xA6)],
		[64, "66C85AA5A6", DecoderOptions.None, Instruction.createU32U32(Code.Enterw_imm16_imm8, 0xA55A, 0xA6)],
		[64, "64A2123456789ABCDEF0", DecoderOptions.None, Instruction.createMemReg(Code.Mov_moffs8_AL, MemoryOperand.new64(Register.None, Register.None, 1, 0xF0DEBC9A78563412n, 8, false, Register.FS), Register.AL)],
		[64, "6669CAA55A", DecoderOptions.None, Instruction.createRegRegU32(Code.Imul_r16_rm16_imm16, Register.CX, Register.DX, 0x5AA5)],
		[64, "69CA5AA51234", DecoderOptions.None, Instruction.createRegRegI32(Code.Imul_r32_rm32_imm32, Register.ECX, Register.EDX, 0x3412A55A)],
		[64, "666BCA5A", DecoderOptions.None, Instruction.createRegRegI32(Code.Imul_r16_rm16_imm8, Register.CX, Register.DX, 0x5A)],
		[64, "6BCA5A", DecoderOptions.None, Instruction.createRegRegI32(Code.Imul_r32_rm32_imm8, Register.ECX, Register.EDX, 0x5A)],
		[64, "486BCA5A", DecoderOptions.None, Instruction.createRegRegI32(Code.Imul_r64_rm64_imm8, Register.RCX, Register.RDX, 0x5A)],
		[64, "4869CA5AA512A4", DecoderOptions.None, Instruction.createRegRegI32(Code.Imul_r64_rm64_imm32, Register.RCX, Register.RDX, -0x5BED5AA6)],
		[64, "6466698C7501EFCDAB5AA5", DecoderOptions.None, Instruction.createRegMemU32(Code.Imul_r16_rm16_imm16, Register.CX, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), 0xA55A)],
		[64, "64698C7501EFCDAB5AA51234", DecoderOptions.None, Instruction.createRegMemI32(Code.Imul_r32_rm32_imm32, Register.ECX, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), 0x3412A55A)],
		[64, "64666B8C7501EFCDAB5A", DecoderOptions.None, Instruction.createRegMemI32(Code.Imul_r16_rm16_imm8, Register.CX, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), 0x5A)],
		[64, "646B8C7501EFCDAB5A", DecoderOptions.None, Instruction.createRegMemI32(Code.Imul_r32_rm32_imm8, Register.ECX, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), 0x5A)],
		[64, "64486B8C7501EFCDAB5A", DecoderOptions.None, Instruction.createRegMemI32(Code.Imul_r64_rm64_imm8, Register.RCX, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), 0x5A)],
		[64, "6448698C7501EFCDAB5AA512A4", DecoderOptions.None, Instruction.createRegMemI32(Code.Imul_r64_rm64_imm32, Register.RCX, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), -0x5BED5AA6)],
		[64, "660F78C1A5FD", DecoderOptions.None, Instruction.createRegI32I32(Code.Extrq_xmm_imm8_imm8, Register.XMM1, 0xA5, 0xFD)],
		[64, "660F78C1A5FD", DecoderOptions.None, Instruction.createRegU32U32(Code.Extrq_xmm_imm8_imm8, Register.XMM1, 0xA5, 0xFD)],
		[64, "64660FA4947501EFCDAB5A", DecoderOptions.None, Instruction.createMemRegI32(Code.Shld_rm16_r16_imm8, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), Register.DX, 0x5A)],
		[64, "64660FA4947501EFCDAB5A", DecoderOptions.None, Instruction.createMemRegU32(Code.Shld_rm16_r16_imm8, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), Register.DX, 0x5A)],
		[64, "F20F78CAA5FD", DecoderOptions.None, Instruction.createRegRegI32I32(Code.Insertq_xmm_xmm_imm8_imm8, Register.XMM1, Register.XMM2, 0xA5, 0xFD)],
		[64, "F20F78CAA5FD", DecoderOptions.None, Instruction.createRegRegU32U32(Code.Insertq_xmm_xmm_imm8_imm8, Register.XMM1, Register.XMM2, 0xA5, 0xFD)],
		[16, "0FB855AA", DecoderOptions.Jmpe, Instruction.createBranch(Code.Jmpe_disp16, 0xAA55n)],
		[32, "0FB8123455AA", DecoderOptions.Jmpe, Instruction.createBranch(Code.Jmpe_disp32, 0xAA553412n)],
		[32, "64676E", DecoderOptions.None, Instruction.createOutsb(16, Register.FS, RepPrefixKind.None)],
		[64, "64676E", DecoderOptions.None, Instruction.createOutsb(32, Register.FS, RepPrefixKind.None)],
		[64, "646E", DecoderOptions.None, Instruction.createOutsb(64, Register.FS, RepPrefixKind.None)],
		[32, "6466676F", DecoderOptions.None, Instruction.createOutsw(16, Register.FS, RepPrefixKind.None)],
		[64, "6466676F", DecoderOptions.None, Instruction.createOutsw(32, Register.FS, RepPrefixKind.None)],
		[64, "64666F", DecoderOptions.None, Instruction.createOutsw(64, Register.FS, RepPrefixKind.None)],
		[32, "64676F", DecoderOptions.None, Instruction.createOutsd(16, Register.FS, RepPrefixKind.None)],
		[64, "64676F", DecoderOptions.None, Instruction.createOutsd(32, Register.FS, RepPrefixKind.None)],
		[64, "646F", DecoderOptions.None, Instruction.createOutsd(64, Register.FS, RepPrefixKind.None)],
		[32, "67AE", DecoderOptions.None, Instruction.createScasb(16, RepPrefixKind.None)],
		[64, "67AE", DecoderOptions.None, Instruction.createScasb(32, RepPrefixKind.None)],
		[64, "AE", DecoderOptions.None, Instruction.createScasb(64, RepPrefixKind.None)],
		[32, "6667AF", DecoderOptions.None, Instruction.createScasw(16, RepPrefixKind.None)],
		[64, "6667AF", DecoderOptions.None, Instruction.createScasw(32, RepPrefixKind.None)],
		[64, "66AF", DecoderOptions.None, Instruction.createScasw(64, RepPrefixKind.None)],
		[32, "67AF", DecoderOptions.None, Instruction.createScasd(16, RepPrefixKind.None)],
		[64, "67AF", DecoderOptions.None, Instruction.createScasd(32, RepPrefixKind.None)],
		[64, "AF", DecoderOptions.None, Instruction.createScasd(64, RepPrefixKind.None)],
		[64, "6748AF", DecoderOptions.None, Instruction.createScasq(32, RepPrefixKind.None)],
		[64, "48AF", DecoderOptions.None, Instruction.createScasq(64, RepPrefixKind.None)],
		[32, "6467AC", DecoderOptions.None, Instruction.createLodsb(16, Register.FS, RepPrefixKind.None)],
		[64, "6467AC", DecoderOptions.None, Instruction.createLodsb(32, Register.FS, RepPrefixKind.None)],
		[64, "64AC", DecoderOptions.None, Instruction.createLodsb(64, Register.FS, RepPrefixKind.None)],
		[32, "646667AD", DecoderOptions.None, Instruction.createLodsw(16, Register.FS, RepPrefixKind.None)],
		[64, "646667AD", DecoderOptions.None, Instruction.createLodsw(32, Register.FS, RepPrefixKind.None)],
		[64, "6466AD", DecoderOptions.None, Instruction.createLodsw(64, Register.FS, RepPrefixKind.None)],
		[32, "6467AD", DecoderOptions.None, Instruction.createLodsd(16, Register.FS, RepPrefixKind.None)],
		[64, "6467AD", DecoderOptions.None, Instruction.createLodsd(32, Register.FS, RepPrefixKind.None)],
		[64, "64AD", DecoderOptions.None, Instruction.createLodsd(64, Register.FS, RepPrefixKind.None)],
		[64, "646748AD", DecoderOptions.None, Instruction.createLodsq(32, Register.FS, RepPrefixKind.None)],
		[64, "6448AD", DecoderOptions.None, Instruction.createLodsq(64, Register.FS, RepPrefixKind.None)],
		[32, "676C", DecoderOptions.None, Instruction.createInsb(16, RepPrefixKind.None)],
		[64, "676C", DecoderOptions.None, Instruction.createInsb(32, RepPrefixKind.None)],
		[64, "6C", DecoderOptions.None, Instruction.createInsb(64, RepPrefixKind.None)],
		[32, "66676D", DecoderOptions.None, Instruction.createInsw(16, RepPrefixKind.None)],
		[64, "66676D", DecoderOptions.None, Instruction.createInsw(32, RepPrefixKind.None)],
		[64, "666D", DecoderOptions.None, Instruction.createInsw(64, RepPrefixKind.None)],
		[32, "676D", DecoderOptions.None, Instruction.createInsd(16, RepPrefixKind.None)],
		[64, "676D", DecoderOptions.None, Instruction.createInsd(32, RepPrefixKind.None)],
		[64, "6D", DecoderOptions.None, Instruction.createInsd(64, RepPrefixKind.None)],
		[32, "67AA", DecoderOptions.None, Instruction.createStosb(16, RepPrefixKind.None)],
		[64, "67AA", DecoderOptions.None, Instruction.createStosb(32, RepPrefixKind.None)],
		[64, "AA", DecoderOptions.None, Instruction.createStosb(64, RepPrefixKind.None)],
		[32, "6667AB", DecoderOptions.None, Instruction.createStosw(16, RepPrefixKind.None)],
		[64, "6667AB", DecoderOptions.None, Instruction.createStosw(32, RepPrefixKind.None)],
		[64, "66AB", DecoderOptions.None, Instruction.createStosw(64, RepPrefixKind.None)],
		[32, "67AB", DecoderOptions.None, Instruction.createStosd(16, RepPrefixKind.None)],
		[64, "67AB", DecoderOptions.None, Instruction.createStosd(32, RepPrefixKind.None)],
		[64, "AB", DecoderOptions.None, Instruction.createStosd(64, RepPrefixKind.None)],
		[64, "6748AB", DecoderOptions.None, Instruction.createStosq(32, RepPrefixKind.None)],
		[64, "48AB", DecoderOptions.None, Instruction.createStosq(64, RepPrefixKind.None)],
		[32, "6467A6", DecoderOptions.None, Instruction.createCmpsb(16, Register.FS, RepPrefixKind.None)],
		[64, "6467A6", DecoderOptions.None, Instruction.createCmpsb(32, Register.FS, RepPrefixKind.None)],
		[64, "64A6", DecoderOptions.None, Instruction.createCmpsb(64, Register.FS, RepPrefixKind.None)],
		[32, "646667A7", DecoderOptions.None, Instruction.createCmpsw(16, Register.FS, RepPrefixKind.None)],
		[64, "646667A7", DecoderOptions.None, Instruction.createCmpsw(32, Register.FS, RepPrefixKind.None)],
		[64, "6466A7", DecoderOptions.None, Instruction.createCmpsw(64, Register.FS, RepPrefixKind.None)],
		[32, "6467A7", DecoderOptions.None, Instruction.createCmpsd(16, Register.FS, RepPrefixKind.None)],
		[64, "6467A7", DecoderOptions.None, Instruction.createCmpsd(32, Register.FS, RepPrefixKind.None)],
		[64, "64A7", DecoderOptions.None, Instruction.createCmpsd(64, Register.FS, RepPrefixKind.None)],
		[64, "646748A7", DecoderOptions.None, Instruction.createCmpsq(32, Register.FS, RepPrefixKind.None)],
		[64, "6448A7", DecoderOptions.None, Instruction.createCmpsq(64, Register.FS, RepPrefixKind.None)],
		[32, "6467A4", DecoderOptions.None, Instruction.createMovsb(16, Register.FS, RepPrefixKind.None)],
		[64, "6467A4", DecoderOptions.None, Instruction.createMovsb(32, Register.FS, RepPrefixKind.None)],
		[64, "64A4", DecoderOptions.None, Instruction.createMovsb(64, Register.FS, RepPrefixKind.None)],
		[32, "646667A5", DecoderOptions.None, Instruction.createMovsw(16, Register.FS, RepPrefixKind.None)],
		[64, "646667A5", DecoderOptions.None, Instruction.createMovsw(32, Register.FS, RepPrefixKind.None)],
		[64, "6466A5", DecoderOptions.None, Instruction.createMovsw(64, Register.FS, RepPrefixKind.None)],
		[32, "6467A5", DecoderOptions.None, Instruction.createMovsd(16, Register.FS, RepPrefixKind.None)],
		[64, "6467A5", DecoderOptions.None, Instruction.createMovsd(32, Register.FS, RepPrefixKind.None)],
		[64, "64A5", DecoderOptions.None, Instruction.createMovsd(64, Register.FS, RepPrefixKind.None)],
		[64, "646748A5", DecoderOptions.None, Instruction.createMovsq(32, Register.FS, RepPrefixKind.None)],
		[64, "6448A5", DecoderOptions.None, Instruction.createMovsq(64, Register.FS, RepPrefixKind.None)],
		[32, "64670FF7D3", DecoderOptions.None, Instruction.createMaskmovq(16, Register.MM2, Register.MM3, Register.FS)],
		[64, "64670FF7D3", DecoderOptions.None, Instruction.createMaskmovq(32, Register.MM2, Register.MM3, Register.FS)],
		[64, "640FF7D3", DecoderOptions.None, Instruction.createMaskmovq(64, Register.MM2, Register.MM3, Register.FS)],
		[32, "6467660FF7D3", DecoderOptions.None, Instruction.createMaskmovdqu(16, Register.XMM2, Register.XMM3, Register.FS)],
		[64, "6467660FF7D3", DecoderOptions.None, Instruction.createMaskmovdqu(32, Register.XMM2, Register.XMM3, Register.FS)],
		[64, "64660FF7D3", DecoderOptions.None, Instruction.createMaskmovdqu(64, Register.XMM2, Register.XMM3, Register.FS)],

		[32, "6467F36E", DecoderOptions.None, Instruction.createOutsb(16, Register.FS, RepPrefixKind.Repe)],
		[64, "6467F36E", DecoderOptions.None, Instruction.createOutsb(32, Register.FS, RepPrefixKind.Repe)],
		[64, "64F36E", DecoderOptions.None, Instruction.createOutsb(64, Register.FS, RepPrefixKind.Repe)],
		[32, "646667F36F", DecoderOptions.None, Instruction.createOutsw(16, Register.FS, RepPrefixKind.Repe)],
		[64, "646667F36F", DecoderOptions.None, Instruction.createOutsw(32, Register.FS, RepPrefixKind.Repe)],
		[64, "6466F36F", DecoderOptions.None, Instruction.createOutsw(64, Register.FS, RepPrefixKind.Repe)],
		[32, "6467F36F", DecoderOptions.None, Instruction.createOutsd(16, Register.FS, RepPrefixKind.Repe)],
		[64, "6467F36F", DecoderOptions.None, Instruction.createOutsd(32, Register.FS, RepPrefixKind.Repe)],
		[64, "64F36F", DecoderOptions.None, Instruction.createOutsd(64, Register.FS, RepPrefixKind.Repe)],
		[32, "67F3AE", DecoderOptions.None, Instruction.createScasb(16, RepPrefixKind.Repe)],
		[64, "67F3AE", DecoderOptions.None, Instruction.createScasb(32, RepPrefixKind.Repe)],
		[64, "F3AE", DecoderOptions.None, Instruction.createScasb(64, RepPrefixKind.Repe)],
		[32, "6667F3AF", DecoderOptions.None, Instruction.createScasw(16, RepPrefixKind.Repe)],
		[64, "6667F3AF", DecoderOptions.None, Instruction.createScasw(32, RepPrefixKind.Repe)],
		[64, "66F3AF", DecoderOptions.None, Instruction.createScasw(64, RepPrefixKind.Repe)],
		[32, "67F3AF", DecoderOptions.None, Instruction.createScasd(16, RepPrefixKind.Repe)],
		[64, "67F3AF", DecoderOptions.None, Instruction.createScasd(32, RepPrefixKind.Repe)],
		[64, "F3AF", DecoderOptions.None, Instruction.createScasd(64, RepPrefixKind.Repe)],
		[64, "67F348AF", DecoderOptions.None, Instruction.createScasq(32, RepPrefixKind.Repe)],
		[64, "F348AF", DecoderOptions.None, Instruction.createScasq(64, RepPrefixKind.Repe)],
		[32, "6467F3AC", DecoderOptions.None, Instruction.createLodsb(16, Register.FS, RepPrefixKind.Repe)],
		[64, "6467F3AC", DecoderOptions.None, Instruction.createLodsb(32, Register.FS, RepPrefixKind.Repe)],
		[64, "64F3AC", DecoderOptions.None, Instruction.createLodsb(64, Register.FS, RepPrefixKind.Repe)],
		[32, "646667F3AD", DecoderOptions.None, Instruction.createLodsw(16, Register.FS, RepPrefixKind.Repe)],
		[64, "646667F3AD", DecoderOptions.None, Instruction.createLodsw(32, Register.FS, RepPrefixKind.Repe)],
		[64, "6466F3AD", DecoderOptions.None, Instruction.createLodsw(64, Register.FS, RepPrefixKind.Repe)],
		[32, "6467F3AD", DecoderOptions.None, Instruction.createLodsd(16, Register.FS, RepPrefixKind.Repe)],
		[64, "6467F3AD", DecoderOptions.None, Instruction.createLodsd(32, Register.FS, RepPrefixKind.Repe)],
		[64, "64F3AD", DecoderOptions.None, Instruction.createLodsd(64, Register.FS, RepPrefixKind.Repe)],
		[64, "6467F348AD", DecoderOptions.None, Instruction.createLodsq(32, Register.FS, RepPrefixKind.Repe)],
		[64, "64F348AD", DecoderOptions.None, Instruction.createLodsq(64, Register.FS, RepPrefixKind.Repe)],
		[32, "67F36C", DecoderOptions.None, Instruction.createInsb(16, RepPrefixKind.Repe)],
		[64, "67F36C", DecoderOptions.None, Instruction.createInsb(32, RepPrefixKind.Repe)],
		[64, "F36C", DecoderOptions.None, Instruction.createInsb(64, RepPrefixKind.Repe)],
		[32, "6667F36D", DecoderOptions.None, Instruction.createInsw(16, RepPrefixKind.Repe)],
		[64, "6667F36D", DecoderOptions.None, Instruction.createInsw(32, RepPrefixKind.Repe)],
		[64, "66F36D", DecoderOptions.None, Instruction.createInsw(64, RepPrefixKind.Repe)],
		[32, "67F36D", DecoderOptions.None, Instruction.createInsd(16, RepPrefixKind.Repe)],
		[64, "67F36D", DecoderOptions.None, Instruction.createInsd(32, RepPrefixKind.Repe)],
		[64, "F36D", DecoderOptions.None, Instruction.createInsd(64, RepPrefixKind.Repe)],
		[32, "67F3AA", DecoderOptions.None, Instruction.createStosb(16, RepPrefixKind.Repe)],
		[64, "67F3AA", DecoderOptions.None, Instruction.createStosb(32, RepPrefixKind.Repe)],
		[64, "F3AA", DecoderOptions.None, Instruction.createStosb(64, RepPrefixKind.Repe)],
		[32, "6667F3AB", DecoderOptions.None, Instruction.createStosw(16, RepPrefixKind.Repe)],
		[64, "6667F3AB", DecoderOptions.None, Instruction.createStosw(32, RepPrefixKind.Repe)],
		[64, "66F3AB", DecoderOptions.None, Instruction.createStosw(64, RepPrefixKind.Repe)],
		[32, "67F3AB", DecoderOptions.None, Instruction.createStosd(16, RepPrefixKind.Repe)],
		[64, "67F3AB", DecoderOptions.None, Instruction.createStosd(32, RepPrefixKind.Repe)],
		[64, "F3AB", DecoderOptions.None, Instruction.createStosd(64, RepPrefixKind.Repe)],
		[64, "67F348AB", DecoderOptions.None, Instruction.createStosq(32, RepPrefixKind.Repe)],
		[64, "F348AB", DecoderOptions.None, Instruction.createStosq(64, RepPrefixKind.Repe)],
		[32, "6467F3A6", DecoderOptions.None, Instruction.createCmpsb(16, Register.FS, RepPrefixKind.Repe)],
		[64, "6467F3A6", DecoderOptions.None, Instruction.createCmpsb(32, Register.FS, RepPrefixKind.Repe)],
		[64, "64F3A6", DecoderOptions.None, Instruction.createCmpsb(64, Register.FS, RepPrefixKind.Repe)],
		[32, "646667F3A7", DecoderOptions.None, Instruction.createCmpsw(16, Register.FS, RepPrefixKind.Repe)],
		[64, "646667F3A7", DecoderOptions.None, Instruction.createCmpsw(32, Register.FS, RepPrefixKind.Repe)],
		[64, "6466F3A7", DecoderOptions.None, Instruction.createCmpsw(64, Register.FS, RepPrefixKind.Repe)],
		[32, "6467F3A7", DecoderOptions.None, Instruction.createCmpsd(16, Register.FS, RepPrefixKind.Repe)],
		[64, "6467F3A7", DecoderOptions.None, Instruction.createCmpsd(32, Register.FS, RepPrefixKind.Repe)],
		[64, "64F3A7", DecoderOptions.None, Instruction.createCmpsd(64, Register.FS, RepPrefixKind.Repe)],
		[64, "6467F348A7", DecoderOptions.None, Instruction.createCmpsq(32, Register.FS, RepPrefixKind.Repe)],
		[64, "64F348A7", DecoderOptions.None, Instruction.createCmpsq(64, Register.FS, RepPrefixKind.Repe)],
		[32, "6467F3A4", DecoderOptions.None, Instruction.createMovsb(16, Register.FS, RepPrefixKind.Repe)],
		[64, "6467F3A4", DecoderOptions.None, Instruction.createMovsb(32, Register.FS, RepPrefixKind.Repe)],
		[64, "64F3A4", DecoderOptions.None, Instruction.createMovsb(64, Register.FS, RepPrefixKind.Repe)],
		[32, "646667F3A5", DecoderOptions.None, Instruction.createMovsw(16, Register.FS, RepPrefixKind.Repe)],
		[64, "646667F3A5", DecoderOptions.None, Instruction.createMovsw(32, Register.FS, RepPrefixKind.Repe)],
		[64, "6466F3A5", DecoderOptions.None, Instruction.createMovsw(64, Register.FS, RepPrefixKind.Repe)],
		[32, "6467F3A5", DecoderOptions.None, Instruction.createMovsd(16, Register.FS, RepPrefixKind.Repe)],
		[64, "6467F3A5", DecoderOptions.None, Instruction.createMovsd(32, Register.FS, RepPrefixKind.Repe)],
		[64, "64F3A5", DecoderOptions.None, Instruction.createMovsd(64, Register.FS, RepPrefixKind.Repe)],
		[64, "6467F348A5", DecoderOptions.None, Instruction.createMovsq(32, Register.FS, RepPrefixKind.Repe)],
		[64, "64F348A5", DecoderOptions.None, Instruction.createMovsq(64, Register.FS, RepPrefixKind.Repe)],

		[32, "6467F26E", DecoderOptions.None, Instruction.createOutsb(16, Register.FS, RepPrefixKind.Repne)],
		[64, "6467F26E", DecoderOptions.None, Instruction.createOutsb(32, Register.FS, RepPrefixKind.Repne)],
		[64, "64F26E", DecoderOptions.None, Instruction.createOutsb(64, Register.FS, RepPrefixKind.Repne)],
		[32, "646667F26F", DecoderOptions.None, Instruction.createOutsw(16, Register.FS, RepPrefixKind.Repne)],
		[64, "646667F26F", DecoderOptions.None, Instruction.createOutsw(32, Register.FS, RepPrefixKind.Repne)],
		[64, "6466F26F", DecoderOptions.None, Instruction.createOutsw(64, Register.FS, RepPrefixKind.Repne)],
		[32, "6467F26F", DecoderOptions.None, Instruction.createOutsd(16, Register.FS, RepPrefixKind.Repne)],
		[64, "6467F26F", DecoderOptions.None, Instruction.createOutsd(32, Register.FS, RepPrefixKind.Repne)],
		[64, "64F26F", DecoderOptions.None, Instruction.createOutsd(64, Register.FS, RepPrefixKind.Repne)],
		[32, "67F2AE", DecoderOptions.None, Instruction.createScasb(16, RepPrefixKind.Repne)],
		[64, "67F2AE", DecoderOptions.None, Instruction.createScasb(32, RepPrefixKind.Repne)],
		[64, "F2AE", DecoderOptions.None, Instruction.createScasb(64, RepPrefixKind.Repne)],
		[32, "6667F2AF", DecoderOptions.None, Instruction.createScasw(16, RepPrefixKind.Repne)],
		[64, "6667F2AF", DecoderOptions.None, Instruction.createScasw(32, RepPrefixKind.Repne)],
		[64, "66F2AF", DecoderOptions.None, Instruction.createScasw(64, RepPrefixKind.Repne)],
		[32, "67F2AF", DecoderOptions.None, Instruction.createScasd(16, RepPrefixKind.Repne)],
		[64, "67F2AF", DecoderOptions.None, Instruction.createScasd(32, RepPrefixKind.Repne)],
		[64, "F2AF", DecoderOptions.None, Instruction.createScasd(64, RepPrefixKind.Repne)],
		[64, "67F248AF", DecoderOptions.None, Instruction.createScasq(32, RepPrefixKind.Repne)],
		[64, "F248AF", DecoderOptions.None, Instruction.createScasq(64, RepPrefixKind.Repne)],
		[32, "6467F2AC", DecoderOptions.None, Instruction.createLodsb(16, Register.FS, RepPrefixKind.Repne)],
		[64, "6467F2AC", DecoderOptions.None, Instruction.createLodsb(32, Register.FS, RepPrefixKind.Repne)],
		[64, "64F2AC", DecoderOptions.None, Instruction.createLodsb(64, Register.FS, RepPrefixKind.Repne)],
		[32, "646667F2AD", DecoderOptions.None, Instruction.createLodsw(16, Register.FS, RepPrefixKind.Repne)],
		[64, "646667F2AD", DecoderOptions.None, Instruction.createLodsw(32, Register.FS, RepPrefixKind.Repne)],
		[64, "6466F2AD", DecoderOptions.None, Instruction.createLodsw(64, Register.FS, RepPrefixKind.Repne)],
		[32, "6467F2AD", DecoderOptions.None, Instruction.createLodsd(16, Register.FS, RepPrefixKind.Repne)],
		[64, "6467F2AD", DecoderOptions.None, Instruction.createLodsd(32, Register.FS, RepPrefixKind.Repne)],
		[64, "64F2AD", DecoderOptions.None, Instruction.createLodsd(64, Register.FS, RepPrefixKind.Repne)],
		[64, "6467F248AD", DecoderOptions.None, Instruction.createLodsq(32, Register.FS, RepPrefixKind.Repne)],
		[64, "64F248AD", DecoderOptions.None, Instruction.createLodsq(64, Register.FS, RepPrefixKind.Repne)],
		[32, "67F26C", DecoderOptions.None, Instruction.createInsb(16, RepPrefixKind.Repne)],
		[64, "67F26C", DecoderOptions.None, Instruction.createInsb(32, RepPrefixKind.Repne)],
		[64, "F26C", DecoderOptions.None, Instruction.createInsb(64, RepPrefixKind.Repne)],
		[32, "6667F26D", DecoderOptions.None, Instruction.createInsw(16, RepPrefixKind.Repne)],
		[64, "6667F26D", DecoderOptions.None, Instruction.createInsw(32, RepPrefixKind.Repne)],
		[64, "66F26D", DecoderOptions.None, Instruction.createInsw(64, RepPrefixKind.Repne)],
		[32, "67F26D", DecoderOptions.None, Instruction.createInsd(16, RepPrefixKind.Repne)],
		[64, "67F26D", DecoderOptions.None, Instruction.createInsd(32, RepPrefixKind.Repne)],
		[64, "F26D", DecoderOptions.None, Instruction.createInsd(64, RepPrefixKind.Repne)],
		[32, "67F2AA", DecoderOptions.None, Instruction.createStosb(16, RepPrefixKind.Repne)],
		[64, "67F2AA", DecoderOptions.None, Instruction.createStosb(32, RepPrefixKind.Repne)],
		[64, "F2AA", DecoderOptions.None, Instruction.createStosb(64, RepPrefixKind.Repne)],
		[32, "6667F2AB", DecoderOptions.None, Instruction.createStosw(16, RepPrefixKind.Repne)],
		[64, "6667F2AB", DecoderOptions.None, Instruction.createStosw(32, RepPrefixKind.Repne)],
		[64, "66F2AB", DecoderOptions.None, Instruction.createStosw(64, RepPrefixKind.Repne)],
		[32, "67F2AB", DecoderOptions.None, Instruction.createStosd(16, RepPrefixKind.Repne)],
		[64, "67F2AB", DecoderOptions.None, Instruction.createStosd(32, RepPrefixKind.Repne)],
		[64, "F2AB", DecoderOptions.None, Instruction.createStosd(64, RepPrefixKind.Repne)],
		[64, "67F248AB", DecoderOptions.None, Instruction.createStosq(32, RepPrefixKind.Repne)],
		[64, "F248AB", DecoderOptions.None, Instruction.createStosq(64, RepPrefixKind.Repne)],
		[32, "6467F2A6", DecoderOptions.None, Instruction.createCmpsb(16, Register.FS, RepPrefixKind.Repne)],
		[64, "6467F2A6", DecoderOptions.None, Instruction.createCmpsb(32, Register.FS, RepPrefixKind.Repne)],
		[64, "64F2A6", DecoderOptions.None, Instruction.createCmpsb(64, Register.FS, RepPrefixKind.Repne)],
		[32, "646667F2A7", DecoderOptions.None, Instruction.createCmpsw(16, Register.FS, RepPrefixKind.Repne)],
		[64, "646667F2A7", DecoderOptions.None, Instruction.createCmpsw(32, Register.FS, RepPrefixKind.Repne)],
		[64, "6466F2A7", DecoderOptions.None, Instruction.createCmpsw(64, Register.FS, RepPrefixKind.Repne)],
		[32, "6467F2A7", DecoderOptions.None, Instruction.createCmpsd(16, Register.FS, RepPrefixKind.Repne)],
		[64, "6467F2A7", DecoderOptions.None, Instruction.createCmpsd(32, Register.FS, RepPrefixKind.Repne)],
		[64, "64F2A7", DecoderOptions.None, Instruction.createCmpsd(64, Register.FS, RepPrefixKind.Repne)],
		[64, "6467F248A7", DecoderOptions.None, Instruction.createCmpsq(32, Register.FS, RepPrefixKind.Repne)],
		[64, "64F248A7", DecoderOptions.None, Instruction.createCmpsq(64, Register.FS, RepPrefixKind.Repne)],
		[32, "6467F2A4", DecoderOptions.None, Instruction.createMovsb(16, Register.FS, RepPrefixKind.Repne)],
		[64, "6467F2A4", DecoderOptions.None, Instruction.createMovsb(32, Register.FS, RepPrefixKind.Repne)],
		[64, "64F2A4", DecoderOptions.None, Instruction.createMovsb(64, Register.FS, RepPrefixKind.Repne)],
		[32, "646667F2A5", DecoderOptions.None, Instruction.createMovsw(16, Register.FS, RepPrefixKind.Repne)],
		[64, "646667F2A5", DecoderOptions.None, Instruction.createMovsw(32, Register.FS, RepPrefixKind.Repne)],
		[64, "6466F2A5", DecoderOptions.None, Instruction.createMovsw(64, Register.FS, RepPrefixKind.Repne)],
		[32, "6467F2A5", DecoderOptions.None, Instruction.createMovsd(16, Register.FS, RepPrefixKind.Repne)],
		[64, "6467F2A5", DecoderOptions.None, Instruction.createMovsd(32, Register.FS, RepPrefixKind.Repne)],
		[64, "64F2A5", DecoderOptions.None, Instruction.createMovsd(64, Register.FS, RepPrefixKind.Repne)],
		[64, "6467F248A5", DecoderOptions.None, Instruction.createMovsq(32, Register.FS, RepPrefixKind.Repne)],
		[64, "64F248A5", DecoderOptions.None, Instruction.createMovsq(64, Register.FS, RepPrefixKind.Repne)],

		[32, "67F36E", DecoderOptions.None, Instruction.createRepOutsb(16)],
		[64, "67F36E", DecoderOptions.None, Instruction.createRepOutsb(32)],
		[64, "F36E", DecoderOptions.None, Instruction.createRepOutsb(64)],
		[32, "6667F36F", DecoderOptions.None, Instruction.createRepOutsw(16)],
		[64, "6667F36F", DecoderOptions.None, Instruction.createRepOutsw(32)],
		[64, "66F36F", DecoderOptions.None, Instruction.createRepOutsw(64)],
		[32, "67F36F", DecoderOptions.None, Instruction.createRepOutsd(16)],
		[64, "67F36F", DecoderOptions.None, Instruction.createRepOutsd(32)],
		[64, "F36F", DecoderOptions.None, Instruction.createRepOutsd(64)],
		[32, "67F3AE", DecoderOptions.None, Instruction.createRepeScasb(16)],
		[64, "67F3AE", DecoderOptions.None, Instruction.createRepeScasb(32)],
		[64, "F3AE", DecoderOptions.None, Instruction.createRepeScasb(64)],
		[32, "6667F3AF", DecoderOptions.None, Instruction.createRepeScasw(16)],
		[64, "6667F3AF", DecoderOptions.None, Instruction.createRepeScasw(32)],
		[64, "66F3AF", DecoderOptions.None, Instruction.createRepeScasw(64)],
		[32, "67F3AF", DecoderOptions.None, Instruction.createRepeScasd(16)],
		[64, "67F3AF", DecoderOptions.None, Instruction.createRepeScasd(32)],
		[64, "F3AF", DecoderOptions.None, Instruction.createRepeScasd(64)],
		[64, "67F348AF", DecoderOptions.None, Instruction.createRepeScasq(32)],
		[64, "F348AF", DecoderOptions.None, Instruction.createRepeScasq(64)],
		[32, "67F2AE", DecoderOptions.None, Instruction.createRepneScasb(16)],
		[64, "67F2AE", DecoderOptions.None, Instruction.createRepneScasb(32)],
		[64, "F2AE", DecoderOptions.None, Instruction.createRepneScasb(64)],
		[32, "6667F2AF", DecoderOptions.None, Instruction.createRepneScasw(16)],
		[64, "6667F2AF", DecoderOptions.None, Instruction.createRepneScasw(32)],
		[64, "66F2AF", DecoderOptions.None, Instruction.createRepneScasw(64)],
		[32, "67F2AF", DecoderOptions.None, Instruction.createRepneScasd(16)],
		[64, "67F2AF", DecoderOptions.None, Instruction.createRepneScasd(32)],
		[64, "F2AF", DecoderOptions.None, Instruction.createRepneScasd(64)],
		[64, "67F248AF", DecoderOptions.None, Instruction.createRepneScasq(32)],
		[64, "F248AF", DecoderOptions.None, Instruction.createRepneScasq(64)],
		[32, "67F3AC", DecoderOptions.None, Instruction.createRepLodsb(16)],
		[64, "67F3AC", DecoderOptions.None, Instruction.createRepLodsb(32)],
		[64, "F3AC", DecoderOptions.None, Instruction.createRepLodsb(64)],
		[32, "6667F3AD", DecoderOptions.None, Instruction.createRepLodsw(16)],
		[64, "6667F3AD", DecoderOptions.None, Instruction.createRepLodsw(32)],
		[64, "66F3AD", DecoderOptions.None, Instruction.createRepLodsw(64)],
		[32, "67F3AD", DecoderOptions.None, Instruction.createRepLodsd(16)],
		[64, "67F3AD", DecoderOptions.None, Instruction.createRepLodsd(32)],
		[64, "F3AD", DecoderOptions.None, Instruction.createRepLodsd(64)],
		[64, "67F348AD", DecoderOptions.None, Instruction.createRepLodsq(32)],
		[64, "F348AD", DecoderOptions.None, Instruction.createRepLodsq(64)],
		[32, "67F36C", DecoderOptions.None, Instruction.createRepInsb(16)],
		[64, "67F36C", DecoderOptions.None, Instruction.createRepInsb(32)],
		[64, "F36C", DecoderOptions.None, Instruction.createRepInsb(64)],
		[32, "6667F36D", DecoderOptions.None, Instruction.createRepInsw(16)],
		[64, "6667F36D", DecoderOptions.None, Instruction.createRepInsw(32)],
		[64, "66F36D", DecoderOptions.None, Instruction.createRepInsw(64)],
		[32, "67F36D", DecoderOptions.None, Instruction.createRepInsd(16)],
		[64, "67F36D", DecoderOptions.None, Instruction.createRepInsd(32)],
		[64, "F36D", DecoderOptions.None, Instruction.createRepInsd(64)],
		[32, "67F3AA", DecoderOptions.None, Instruction.createRepStosb(16)],
		[64, "67F3AA", DecoderOptions.None, Instruction.createRepStosb(32)],
		[64, "F3AA", DecoderOptions.None, Instruction.createRepStosb(64)],
		[32, "6667F3AB", DecoderOptions.None, Instruction.createRepStosw(16)],
		[64, "6667F3AB", DecoderOptions.None, Instruction.createRepStosw(32)],
		[64, "66F3AB", DecoderOptions.None, Instruction.createRepStosw(64)],
		[32, "67F3AB", DecoderOptions.None, Instruction.createRepStosd(16)],
		[64, "67F3AB", DecoderOptions.None, Instruction.createRepStosd(32)],
		[64, "F3AB", DecoderOptions.None, Instruction.createRepStosd(64)],
		[64, "67F348AB", DecoderOptions.None, Instruction.createRepStosq(32)],
		[64, "F348AB", DecoderOptions.None, Instruction.createRepStosq(64)],
		[32, "67F3A6", DecoderOptions.None, Instruction.createRepeCmpsb(16)],
		[64, "67F3A6", DecoderOptions.None, Instruction.createRepeCmpsb(32)],
		[64, "F3A6", DecoderOptions.None, Instruction.createRepeCmpsb(64)],
		[32, "6667F3A7", DecoderOptions.None, Instruction.createRepeCmpsw(16)],
		[64, "6667F3A7", DecoderOptions.None, Instruction.createRepeCmpsw(32)],
		[64, "66F3A7", DecoderOptions.None, Instruction.createRepeCmpsw(64)],
		[32, "67F3A7", DecoderOptions.None, Instruction.createRepeCmpsd(16)],
		[64, "67F3A7", DecoderOptions.None, Instruction.createRepeCmpsd(32)],
		[64, "F3A7", DecoderOptions.None, Instruction.createRepeCmpsd(64)],
		[64, "67F348A7", DecoderOptions.None, Instruction.createRepeCmpsq(32)],
		[64, "F348A7", DecoderOptions.None, Instruction.createRepeCmpsq(64)],
		[32, "67F2A6", DecoderOptions.None, Instruction.createRepneCmpsb(16)],
		[64, "67F2A6", DecoderOptions.None, Instruction.createRepneCmpsb(32)],
		[64, "F2A6", DecoderOptions.None, Instruction.createRepneCmpsb(64)],
		[32, "6667F2A7", DecoderOptions.None, Instruction.createRepneCmpsw(16)],
		[64, "6667F2A7", DecoderOptions.None, Instruction.createRepneCmpsw(32)],
		[64, "66F2A7", DecoderOptions.None, Instruction.createRepneCmpsw(64)],
		[32, "67F2A7", DecoderOptions.None, Instruction.createRepneCmpsd(16)],
		[64, "67F2A7", DecoderOptions.None, Instruction.createRepneCmpsd(32)],
		[64, "F2A7", DecoderOptions.None, Instruction.createRepneCmpsd(64)],
		[64, "67F248A7", DecoderOptions.None, Instruction.createRepneCmpsq(32)],
		[64, "F248A7", DecoderOptions.None, Instruction.createRepneCmpsq(64)],
		[32, "67F3A4", DecoderOptions.None, Instruction.createRepMovsb(16)],
		[64, "67F3A4", DecoderOptions.None, Instruction.createRepMovsb(32)],
		[64, "F3A4", DecoderOptions.None, Instruction.createRepMovsb(64)],
		[32, "6667F3A5", DecoderOptions.None, Instruction.createRepMovsw(16)],
		[64, "6667F3A5", DecoderOptions.None, Instruction.createRepMovsw(32)],
		[64, "66F3A5", DecoderOptions.None, Instruction.createRepMovsw(64)],
		[32, "67F3A5", DecoderOptions.None, Instruction.createRepMovsd(16)],
		[64, "67F3A5", DecoderOptions.None, Instruction.createRepMovsd(32)],
		[64, "F3A5", DecoderOptions.None, Instruction.createRepMovsd(64)],
		[64, "67F348A5", DecoderOptions.None, Instruction.createRepMovsq(32)],
		[64, "F348A5", DecoderOptions.None, Instruction.createRepMovsq(64)],
	];
	test_instruction_create(data);
});

test("VEX: Instruction.create*()", () => {
	// Check if VEX has been disabled
	if ((getIcedFeatures() & 1) == 0)
		return;
	const data = [
		[64, "C5E814CB", DecoderOptions.None, Instruction.createRegRegReg(Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM1, Register.XMM2, Register.XMM3)],
		[64, "64C5E8148C7501EFCDAB", DecoderOptions.None, Instruction.createRegRegMem(Code.VEX_Vunpcklps_xmm_xmm_xmmm128, Register.XMM1, Register.XMM2, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS))],
		[64, "64C4E261908C7501EFCDAB", DecoderOptions.None, Instruction.createRegMemReg(Code.VEX_Vpgatherdd_xmm_vm32x_xmm, Register.XMM1, new MemoryOperand(Register.RBP, Register.XMM6, 2, -0x543210FFn, 8, false, Register.FS), Register.XMM3)],
		[64, "64C4E2692E9C7501EFCDAB", DecoderOptions.None, Instruction.createMemRegReg(Code.VEX_Vmaskmovps_m128_xmm_xmm, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), Register.XMM2, Register.XMM3)],
		[64, "C4E3694ACB40", DecoderOptions.None, Instruction.createRegRegRegReg(Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM1, Register.XMM2, Register.XMM3, Register.XMM4)],
		[64, "64C4E3E95C8C7501EFCDAB30", DecoderOptions.None, Instruction.createRegRegRegMem(Code.VEX_Vfmaddsubps_xmm_xmm_xmm_xmmm128, Register.XMM1, Register.XMM2, Register.XMM3, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS))],
		[64, "64C4E3694A8C7501EFCDAB40", DecoderOptions.None, Instruction.createRegRegMemReg(Code.VEX_Vblendvps_xmm_xmm_xmmm128_xmm, Register.XMM1, Register.XMM2, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), Register.XMM4)],
		[64, "C4E36948CB40", DecoderOptions.None, Instruction.createRegRegRegRegI32(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM1, Register.XMM2, Register.XMM3, Register.XMM4, 0x0)],
		[64, "C4E36948CB40", DecoderOptions.None, Instruction.createRegRegRegRegU32(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM1, Register.XMM2, Register.XMM3, Register.XMM4, 0x0)],
		[64, "64C4E3E9488C7501EFCDAB31", DecoderOptions.None, Instruction.createRegRegRegMemI32(Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm4, Register.XMM1, Register.XMM2, Register.XMM3, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), 0x1)],
		[64, "64C4E3E9488C7501EFCDAB31", DecoderOptions.None, Instruction.createRegRegRegMemU32(Code.VEX_Vpermil2ps_xmm_xmm_xmm_xmmm128_imm4, Register.XMM1, Register.XMM2, Register.XMM3, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), 0x1)],
		[64, "64C4E369488C7501EFCDAB41", DecoderOptions.None, Instruction.createRegRegMemRegI32(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM1, Register.XMM2, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), Register.XMM4, 0x1)],
		[64, "64C4E369488C7501EFCDAB41", DecoderOptions.None, Instruction.createRegRegMemRegU32(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm4, Register.XMM1, Register.XMM2, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), Register.XMM4, 0x1)],
		[32, "6467C5F9F7D3", DecoderOptions.None, Instruction.createVmaskmovdqu(16, Register.XMM2, Register.XMM3, Register.FS)],
		[64, "6467C5F9F7D3", DecoderOptions.None, Instruction.createVmaskmovdqu(32, Register.XMM2, Register.XMM3, Register.FS)],
		[64, "64C5F9F7D3", DecoderOptions.None, Instruction.createVmaskmovdqu(64, Register.XMM2, Register.XMM3, Register.FS)],
	];
	test_instruction_create(data);
});

test("EVEX: Instruction.create*()", () => {
	// Check if EVEX has been disabled
	if ((getIcedFeatures() & 2) == 0)
		return;
	const data = [
		[64, "62F1F50873D2A5", DecoderOptions.None, Instruction.createRegRegI32(Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM1, Register.XMM2, 0xA5)],
		[64, "6462F1F50873947501EFCDABA5", DecoderOptions.None, Instruction.createRegMemI32(Code.EVEX_Vpsrlq_xmm_k1z_xmmm128b64_imm8, Register.XMM1, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), 0xA5)],
		[64, "62F16D08C4CBA5", DecoderOptions.None, Instruction.createRegRegRegI32(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM1, Register.XMM2, Register.EBX, 0xA5)],
		[64, "62F16D08C4CBA5", DecoderOptions.None, Instruction.createRegRegRegU32(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM1, Register.XMM2, Register.EBX, 0xA5)],
		[64, "6462F16D08C48C7501EFCDABA5", DecoderOptions.None, Instruction.createRegRegMemI32(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM1, Register.XMM2, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), 0xA5)],
		[64, "6462F16D08C48C7501EFCDABA5", DecoderOptions.None, Instruction.createRegRegMemU32(Code.EVEX_Vpinsrw_xmm_xmm_r32m16_imm8, Register.XMM1, Register.XMM2, new MemoryOperand(Register.RBP, Register.RSI, 2, -0x543210FFn, 8, false, Register.FS), 0xA5)],
	];
	test_instruction_create(data);
});

function test_instruction_create(data) {
	for (const info of data) {
		const bitness = info[0];
		const bytes = parseHex(info[1]);
		const options = info[2];
		const expected = info[3];
		const decoder = new Decoder(bitness, bytes, options);
		switch (bitness) {
			case 16:
				decoder.ip = 0x7FF0n;
				break;
			case 32:
				decoder.ip = 0x7FFFFFF0n;
				break;
			case 64:
				decoder.ip = 0x7FFFFFFFFFFFFFF0n;
				break;
			default:
				throw new Error("Invalid bitness");
		}
		const instr = decoder.decode();
		expect(instr.code).not.toBe(Code.INVALID);
		expect(expected.code).not.toBe(Code.INVALID);
		expect(decoder.canDecode).toBe(false);

		instr.codeSize = CodeSize.Unknown;
		instr.length = 0;
		instr.nextIP = 0n;
		expect(expected.equalsAllBits(instr)).toBe(true);

		expected.free();
		decoder.free();
		instr.free();
	}
}
