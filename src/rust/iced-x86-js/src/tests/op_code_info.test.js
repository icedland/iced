/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

const {
	Code, CodeExt, Decoder, DecoderOptions, EncodingKind, MandatoryPrefix, OpCodeOperandKind,
	OpCodeTableKind, TupleType
} = require("iced-x86");

test("OpCodeInfo", () => {
	const bytes = new Uint8Array([0x48, 0x01, 0x18]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);
	const instr = decoder.decode();

	const info1 = instr.opCode;
	const info2 = CodeExt.opCode(instr.code);
	expect(info1.code).toBe(Code.Add_rm64_r64);
	expect(info2.code).toBe(Code.Add_rm64_r64);

	expect(info1.encoding).toBe(EncodingKind.Legacy);
	expect(info1.isInstruction).toBe(true);
	expect(info1.mode16).toBe(false);
	expect(info1.mode32).toBe(false);
	expect(info1.mode64).toBe(true);
	expect(info1.fwait).toBe(false);
	expect(info1.operandSize).toBe(64);
	expect(info1.addressSize).toBe(0);
	expect(info1.L).toBe(0);
	expect(info1.W).toBe(0);
	expect(info1.isLIG).toBe(false);
	expect(info1.isWIG).toBe(false);
	expect(info1.isWIG32).toBe(false);
	expect(info1.tupleType).toBe(TupleType.N1);
	expect(info1.canBroadcast).toBe(false);
	expect(info1.canUseRoundingControl).toBe(false);
	expect(info1.canSuppressAllExceptions).toBe(false);
	expect(info1.canUseOpMaskRegister).toBe(false);
	expect(info1.requireOpMaskRegister).toBe(false);
	expect(info1.canUseZeroingMasking).toBe(false);
	expect(info1.canUseLockPrefix).toBe(true);
	expect(info1.canUseXacquirePrefix).toBe(true);
	expect(info1.canUseXreleasePrefix).toBe(true);
	expect(info1.canUseRepPrefix).toBe(false);
	expect(info1.canUseRepnePrefix).toBe(false);
	expect(info1.canUseBndPrefix).toBe(false);
	expect(info1.canUseHintTakenPrefix).toBe(false);
	expect(info1.canUseNotrackPrefix).toBe(false);
	expect(info1.ignoresRoundingControl).toBe(false);
	expect(info1.amdLockRegBit).toBe(false);
	expect(info1.defaultOpSize64).toBe(false);
	expect(info1.forceOpSize64).toBe(false);
	expect(info1.intelForceOpSize64).toBe(false);
	expect(info1.cpl0).toBe(true);
	expect(info1.cpl1).toBe(true);
	expect(info1.cpl2).toBe(true);
	expect(info1.cpl3).toBe(true);
	expect(info1.isInputOutput).toBe(false);
	expect(info1.isNop).toBe(false);
	expect(info1.isReservedNop).toBe(false);
	expect(info1.isSerializingIntel).toBe(false);
	expect(info1.isSerializingAmd).toBe(false);
	expect(info1.mayRequireCpl0).toBe(false);
	expect(info1.isCetTracked).toBe(false);
	expect(info1.isNonTemporal).toBe(false);
	expect(info1.isFpuNoWait).toBe(false);
	expect(info1.ignoresModBits).toBe(false);
	expect(info1.no66).toBe(false);
	expect(info1.nfx).toBe(false);
	expect(info1.requiresUniqueRegNums).toBe(false);
	expect(info1.isPrivileged).toBe(false);
	expect(info1.isSaveRestore).toBe(false);
	expect(info1.isStackInstruction).toBe(false);
	expect(info1.ignoresSegment).toBe(false);
	expect(info1.isOpMaskReadWrite).toBe(false);
	expect(info1.realMode).toBe(false);
	expect(info1.protectedMode).toBe(false);
	expect(info1.virtual8086Mode).toBe(false);
	expect(info1.compatibilityMode).toBe(false);
	expect(info1.longMode).toBe(true);
	expect(info1.useOutsideSmm).toBe(true);
	expect(info1.useInSmm).toBe(true);
	expect(info1.useOutsideEnclaveSgx).toBe(true);
	expect(info1.useInEnclaveSgx1).toBe(true);
	expect(info1.useInEnclaveSgx2).toBe(true);
	expect(info1.useOutsideVmxOp).toBe(true);
	expect(info1.useInVmxRootOp).toBe(true);
	expect(info1.useInVmxNonRootOp).toBe(true);
	expect(info1.useOutsideSeam).toBe(true);
	expect(info1.useInSeam).toBe(true);
	expect(info1.tdxNonRootGenUd).toBe(false);
	expect(info1.tdxNonRootGenVe).toBe(false);
	expect(info1.tdxNonRootMayGenEx).toBe(false);
	expect(info1.intelVmExit).toBe(false);
	expect(info1.intelMayVmExit).toBe(false);
	expect(info1.intelSmmVmExit).toBe(false);
	expect(info1.amdVmExit).toBe(false);
	expect(info1.amdMayVmExit).toBe(false);
	expect(info1.tsxAbort).toBe(false);
	expect(info1.tsxImplAbort).toBe(false);
	expect(info1.tsxMayAbort).toBe(false);
	expect(info1.intelDecoder16).toBe(false);
	expect(info1.intelDecoder32).toBe(false);
	expect(info1.intelDecoder64).toBe(true);
	expect(info1.amdDecoder16).toBe(false);
	expect(info1.amdDecoder32).toBe(false);
	expect(info1.amdDecoder64).toBe(true);
	expect(info1.decoderOption).toBe(DecoderOptions.None);
	expect(info1.table).toBe(OpCodeTableKind.Normal);
	expect(info1.mandatoryPrefix).toBe(MandatoryPrefix.None);
	expect(info1.opCode).toBe(0x01);
	expect(info1.opCodeLength).toBe(1);
	expect(info1.isGroup).toBe(false);
	expect(info1.groupIndex).toBe(-1);
	expect(info1.isRmGroup).toBe(false);
	expect(info1.rmGroupIndex).toBe(-1);
	expect(info1.opCount).toBe(2);
	expect(info1.op0Kind).toBe(OpCodeOperandKind.r64_or_mem);
	expect(info1.op1Kind).toBe(OpCodeOperandKind.r64_reg);
	expect(info1.op2Kind).toBe(OpCodeOperandKind.None);
	expect(info1.op3Kind).toBe(OpCodeOperandKind.None);
	expect(info1.op4Kind).toBe(OpCodeOperandKind.None);
	expect(info1.opKind(0)).toBe(OpCodeOperandKind.r64_or_mem);
	expect(info1.opKind(1)).toBe(OpCodeOperandKind.r64_reg);
	expect(info1.opKind(2)).toBe(OpCodeOperandKind.None);
	expect(info1.opKind(3)).toBe(OpCodeOperandKind.None);
	expect(info1.opKind(4)).toBe(OpCodeOperandKind.None);
	expect(info1.isAvailableInMode(16)).toBe(false);
	expect(info1.isAvailableInMode(32)).toBe(false);
	expect(info1.isAvailableInMode(64)).toBe(true);
	expect(info1.opCodeString).toBe("REX.W 01 /r");
	expect(info1.instructionString).toBe("ADD r/m64, r64");

	decoder.free();
	instr.free();
	info1.free();
	info2.free();
});
