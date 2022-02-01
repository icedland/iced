// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

const {
	Decoder, DecoderOptions, InstructionInfoFactory, InstructionInfoOptions, MemorySize,
	OpAccess, Register
} = require("iced-x86");

test("Instruction info factory", () => {
	// add [rdi+r12*8-5AA5EDCCh],esi
	const bytes = new Uint8Array([0x42, 0x01, 0xB4, 0xE7, 0x34, 0x12, 0x5A, 0xA5]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);
	const infoFactory = new InstructionInfoFactory();
	const instr = decoder.decode();

	const info1 = infoFactory.info(instr);
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
	expect(instr.fpuTopIncrement).toBe(0);
	expect(instr.fpuCondWritesTop).toBe(false);
	expect(instr.fpuWritesTop).toBe(false);

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
	expect(mem1[0].displacement).toBe(0xFFFFFFFFA55A1234n);
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
	expect(mem3[0].displacement).toBe(0xFFFFFFFFA55A1234n);
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
