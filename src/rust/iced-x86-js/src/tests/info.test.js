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
	CpuidFeature, Decoder, DecoderOptions, EncodingKind, FlowControl, InstructionInfoFactory,
	InstructionInfoOptions, MemorySize, OpAccess, Register, RflagsBits
} = require("iced-x86");

test("Instruction info factory", () => {
	// add [rdi+r12*8-5AA5EDCCh],esi
	const bytes = new Uint8Array([0x42, 0x01, 0xB4, 0xE7, 0x34, 0x12, 0x5A, 0xA5]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);
	const infoFactory = new InstructionInfoFactory();
	const instr = decoder.decode();

	const info1 = infoFactory.info(instr);
	expect(info1.isPrivileged).toBe(false);
	expect(info1.isStackInstruction).toBe(false);
	expect(info1.isSaveRestoreInstruction).toBe(false);
	expect(info1.encoding).toBe(EncodingKind.Legacy);
	expect(info1.cpuidFeatures()).toStrictEqual(new Int32Array([CpuidFeature.INTEL386]));
	expect(info1.flowControl).toBe(FlowControl.Next);
	expect(info1.op0Access).toBe(OpAccess.ReadWrite);
	expect(info1.op1Access).toBe(OpAccess.Read);
	expect(info1.op2Access).toBe(OpAccess.None);
	expect(info1.op3Access).toBe(OpAccess.None);
	expect(info1.op4Access).toBe(OpAccess.None);
	expect(info1.opAccess(0)).toBe(OpAccess.ReadWrite);
	expect(info1.opAccess(1)).toBe(OpAccess.Read);
	expect(info1.opAccess(2)).toBe(OpAccess.None);
	expect(info1.opAccess(3)).toBe(OpAccess.None);
	expect(info1.opAccess(4)).toBe(OpAccess.None);
	expect(info1.rflagsRead).toBe(RflagsBits.None);
	expect(info1.rflagsWritten).toBe(RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF);
	expect(info1.rflagsCleared).toBe(RflagsBits.None);
	expect(info1.rflagsSet).toBe(RflagsBits.None);
	expect(info1.rflagsUndefined).toBe(RflagsBits.None);
	expect(info1.rflagsModified).toBe(RflagsBits.OF | RflagsBits.SF | RflagsBits.ZF | RflagsBits.AF | RflagsBits.CF | RflagsBits.PF);

	const regs1 = info1.usedRegisters();
	expect(regs1.length).toBe(3);
	expect(regs1[0].register).toBe(Register.RDI);
	expect(regs1[0].access).toBe(OpAccess.Read);
	expect(regs1[1].register).toBe(Register.R12);
	expect(regs1[1].access).toBe(OpAccess.Read);
	expect(regs1[2].register).toBe(Register.ESI);
	expect(regs1[2].access).toBe(OpAccess.Read);
	const mem1 = info1.usedMemory();
	expect(mem1.length).toBe(1);
	expect(mem1[0].segment).toBe(Register.DS);
	expect(mem1[0].base).toBe(Register.RDI);
	expect(mem1[0].index).toBe(Register.R12);
	expect(mem1[0].scale).toBe(8);
	expect(mem1[0].displacementHi).toBe(0xFFFFFFFF);
	expect(mem1[0].displacementLo).toBe(0xA55A1234);
	expect(mem1[0].memorySize).toBe(MemorySize.UInt32);
	expect(mem1[0].access).toBe(OpAccess.ReadWrite);

	const info2 = infoFactory.infoOptions(instr, InstructionInfoOptions.NoMemoryUsage);
	const regs2 = info2.usedRegisters();
	expect(regs2.length).toBe(3);
	expect(regs2[0].register).toBe(Register.RDI);
	expect(regs2[0].access).toBe(OpAccess.Read);
	expect(regs2[1].register).toBe(Register.R12);
	expect(regs2[1].access).toBe(OpAccess.Read);
	expect(regs2[2].register).toBe(Register.ESI);
	expect(regs2[2].access).toBe(OpAccess.Read);
	const mem2 = info2.usedMemory();
	expect(mem2.length).toBe(0);

	const info3 = infoFactory.infoOptions(instr, InstructionInfoOptions.NoRegisterUsage);
	const regs3 = info3.usedRegisters();
	expect(regs3.length).toBe(0);
	const mem3 = info3.usedMemory();
	expect(mem3.length).toBe(1);
	expect(mem3[0].segment).toBe(Register.DS);
	expect(mem3[0].base).toBe(Register.RDI);
	expect(mem3[0].index).toBe(Register.R12);
	expect(mem3[0].scale).toBe(8);
	expect(mem3[0].displacementHi).toBe(0xFFFFFFFF);
	expect(mem3[0].displacementLo).toBe(0xA55A1234);
	expect(mem3[0].memorySize).toBe(MemorySize.UInt32);
	expect(mem3[0].access).toBe(OpAccess.ReadWrite);

	const info4 = infoFactory.infoOptions(instr, InstructionInfoOptions.NoMemoryUsage | InstructionInfoOptions.NoRegisterUsage);
	const regs4 = info4.usedRegisters();
	expect(regs4.length).toBe(0);
	const mem4 = info4.usedMemory();
	expect(mem4.length).toBe(0);

	decoder.free();
	infoFactory.free();
	instr.free();
	info1.free();
	info2.free();
	info3.free();
	info4.free();
	regs1.forEach(a => a.free());
	regs2.forEach(a => a.free());
	regs3.forEach(a => a.free());
	regs4.forEach(a => a.free());
	mem1.forEach(a => a.free());
	mem2.forEach(a => a.free());
	mem3.forEach(a => a.free());
	mem4.forEach(a => a.free());
});
