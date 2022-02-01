// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

const { Code, Decoder, DecoderError, DecoderOptions, Instruction, OpKind, Register } = require("iced-x86");

test("Create a Decoder with no bytes", () => {
	const bytes = new Uint8Array([]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);
	expect(decoder.canDecode).toBe(false);
	const instr = decoder.decode();
	expect(instr.isInvalid).toBe(true);
	expect(instr.code).toBe(Code.INVALID);
	expect(decoder.lastError).toBe(DecoderError.NoMoreBytes);
	instr.free();
	decoder.free();
});

test("Create a 16-bit Decoder", () => {
	const bytes = new Uint8Array([0x01, 0x18]);
	const decoder = new Decoder(16, bytes, DecoderOptions.None);

	expect(decoder.bitness).toBe(16);
	const instr = decoder.decode();
	expect(instr.isInvalid).toBe(false);
	expect(instr.code).toBe(Code.Add_rm16_r16);
	expect(instr.op0Kind).toBe(OpKind.Memory);
	expect(instr.memoryBase).toBe(Register.BX);
	expect(instr.memoryIndex).toBe(Register.SI);

	decoder.free();
	instr.free();
});

test("Create a 32-bit Decoder", () => {
	const bytes = new Uint8Array([0x01, 0x18]);
	const decoder = new Decoder(32, bytes, DecoderOptions.None);

	expect(decoder.bitness).toBe(32);
	const instr = decoder.decode();
	expect(instr.isInvalid).toBe(false);
	expect(instr.code).toBe(Code.Add_rm32_r32);
	expect(instr.op0Kind).toBe(OpKind.Memory);
	expect(instr.memoryBase).toBe(Register.EAX);
	expect(instr.memoryIndex).toBe(Register.None);

	decoder.free();
	instr.free();
});

test("Create a 64-bit Decoder", () => {
	const bytes = new Uint8Array([0x48, 0x01, 0x18]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);

	expect(decoder.bitness).toBe(64);
	const instr = decoder.decode();
	expect(instr.isInvalid).toBe(false);
	expect(instr.code).toBe(Code.Add_rm64_r64);
	expect(instr.op0Kind).toBe(OpKind.Memory);
	expect(instr.memoryBase).toBe(Register.RAX);
	expect(instr.memoryIndex).toBe(Register.None);

	decoder.free();
	instr.free();
});

test("Decode multiple instructions", () => {
	const bytes = new Uint8Array([0x86, 0x64, 0x32, 0x16, 0xF0, 0xF2, 0x83, 0x00, 0x5A]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);

	expect(decoder.maxPosition).toBe(9);
	expect(decoder.canDecode).toBe(true);
	expect(decoder.position).toBe(0);
	const instr1 = decoder.decode();
	expect(instr1.code).toBe(Code.Xchg_rm8_r8);
	expect(decoder.canDecode).toBe(true);
	expect(decoder.position).toBe(4);
	const instr2 = decoder.decode();
	expect(instr2.code).toBe(Code.Add_rm32_imm8);
	expect(decoder.canDecode).toBe(false);
	expect(decoder.position).toBe(9);
	const instr3 = decoder.decode();
	expect(instr3.code).toBe(Code.INVALID);
	expect(decoder.canDecode).toBe(false);
	expect(decoder.position).toBe(9);

	decoder.free();
	instr1.free();
	instr2.free();
	instr3.free();
});

test("Decode with decodeOut()", () => {
	const bytes = new Uint8Array([0x86, 0x64, 0x32, 0x16, 0xF0, 0xF2, 0x83, 0x00, 0x5A, 0x48, 0x89, 0xCE]);
	const decodera = new Decoder(64, bytes, DecoderOptions.None);
	const decoderb = new Decoder(64, bytes, DecoderOptions.None);
	const instr = new Instruction();

	const instr1 = decodera.decode();
	decoderb.decodeOut(instr);
	expect(instr1.equals(instr)).toBe(true);

	const instr2 = decodera.decode();
	decoderb.decodeOut(instr);
	expect(instr2.equals(instr)).toBe(true);

	const instr3 = decodera.decode();
	decoderb.decodeOut(instr);
	expect(instr3.equals(instr)).toBe(true);

	const instr4 = decodera.decode();
	decoderb.decodeOut(instr);
	expect(instr4.equals(instr)).toBe(true);

	decodera.free();
	decoderb.free();
	instr.free();
	instr1.free();
	instr2.free();
	instr3.free();
	instr4.free();
});

test("Decode all instructions", () => {
	const bytes = new Uint8Array([0x86, 0x64, 0x32, 0x16, 0xF0, 0xF2, 0x83, 0x00, 0x5A, 0x48, 0x89, 0xCE]);
	const decodera = new Decoder(64, bytes, DecoderOptions.None);
	const decoderb = new Decoder(64, bytes, DecoderOptions.None);
	const decoderc = new Decoder(64, bytes, DecoderOptions.None);

	const instrs = decoderb.decodeInstructions(0);
	expect(instrs.length).toBe(0);
	const instrsa = decodera.decodeAll();
	const instrsb = decoderb.decodeInstructions(0xFFFFFFFF);
	expect(decodera.canDecode).toBe(false);
	expect(decoderb.canDecode).toBe(false);
	expect(instrsa.length).toBe(3);
	expect(instrsb.length).toBe(3);
	for (let i = 0; i < instrsa.length; i++)
		expect(instrsa[i].equals(instrsb[i])).toBe(true);
	const instrsc = decodera.decodeAll();
	const instrsd = decoderb.decodeInstructions(0xFFFFFFFF);
	expect(instrsc.length).toBe(0);
	expect(instrsd.length).toBe(0);

	const instrse = decoderc.decodeInstructions(2);
	const instrsf = decoderc.decodeInstructions(3);
	expect(instrse.length).toBe(2);
	expect(instrsf.length).toBe(1);
	for (let i = 0; i < 2; i++)
		expect(instrsa[i].equals(instrse[i])).toBe(true);
	for (let i = 2; i < 3; i++)
		expect(instrsa[i].equals(instrsf[i - 2])).toBe(true);

	decodera.free();
	decoderb.free();
	decoderc.free();
	instrs.forEach(a => a.free());
	instrsa.forEach(a => a.free());
	instrsb.forEach(a => a.free());
	instrsc.forEach(a => a.free());
	instrsd.forEach(a => a.free());
	instrse.forEach(a => a.free());
	instrsf.forEach(a => a.free());
});

test("Decode with DecoderOptions.NoInvalidCheck", () => {
	const bytes = new Uint8Array([0xF0, 0x02, 0xCE]);
	const decodera = new Decoder(64, bytes, DecoderOptions.None);
	const decoderb = new Decoder(64, bytes, DecoderOptions.NoInvalidCheck);

	const instra = decodera.decode();
	const instrb = decoderb.decode();
	expect(instra.code).toBe(Code.INVALID);
	expect(decodera.lastError).toBe(DecoderError.InvalidInstruction);
	expect(instrb.code).toBe(Code.Add_r8_rm8);

	decodera.free();
	decoderb.free();
	instra.free();
	instrb.free();
});

test("Decode with DecoderOptions.AMD", () => {
	const bytes = new Uint8Array([0x66, 0x70, 0x5A]);
	const decodera = new Decoder(64, bytes, DecoderOptions.None);
	const decoderb = new Decoder(64, bytes, DecoderOptions.AMD);

	const instra = decodera.decode();
	const instrb = decoderb.decode();
	expect(instra.code).toBe(Code.Jo_rel8_64);
	expect(instrb.code).toBe(Code.Jo_rel8_16);

	decodera.free();
	decoderb.free();
	instra.free();
	instrb.free();
});

test("Creating a Decoder with an invalid bitness throws", () => {
	expect(() => new Decoder(63, new Uint8Array([0x90]), DecoderOptions.None)).toThrow();
});

// Make sure it's not an enum arg in the Rust code since it's a flags enum. It must be a u32 in the method sig.
test("Create a Decoder with multiple options", () => {
	const decoder = new Decoder(64, new Uint8Array([0xF3, 0x90]), DecoderOptions.AMD | DecoderOptions.NoPause);
	const instr = decoder.decode();
	expect(instr.code).toBe(Code.Nopd);
	decoder.free();
	instr.free();
});

test("Decoder.IP prop", () => {
	const decoder = new Decoder(64, new Uint8Array([0x90]), DecoderOptions.None);
	expect(decoder.ip).toBe(0n);
	decoder.ip = 0x12345678n;
	expect(decoder.ip).toBe(0x12345678n);
	decoder.ip = 0x9ABCDEFD12345678n;
	expect(decoder.ip).toBe(0x9ABCDEFD12345678n);
	decoder.free();
});

test("Set Decoder position", () => {
	const bytes = new Uint8Array([0x86, 0x64, 0x32, 0x16, 0xF0, 0xF2, 0x83, 0x00, 0x5A, 0x48, 0x89, 0xCE]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);

	const instrs1 = decoder.decodeAll();
	const instrs2 = decoder.decodeAll();
	decoder.position = 0;
	const instrs3 = decoder.decodeAll();
	const instrs4 = decoder.decodeAll();
	decoder.position = 4;
	const instrs5 = decoder.decodeAll();
	const instrs6 = decoder.decodeAll();

	expect(instrs2.length).toBe(0);
	expect(instrs4.length).toBe(0);
	expect(instrs6.length).toBe(0);
	expect(instrs1.length).toBe(3);
	expect(instrs3.length).toBe(3);
	expect(instrs5.length).toBe(2);
	for (let i = 0; i < instrs1.length; i++) {
		expect(instrs1[i].equals(instrs3[i]));
		if (i >= 1)
			expect(instrs1[i].equals(instrs5[i - 1]));
	}

	decoder.free();
	instrs1.forEach(a => a.free());
	instrs2.forEach(a => a.free());
	instrs3.forEach(a => a.free());
	instrs4.forEach(a => a.free());
	instrs5.forEach(a => a.free());
	instrs6.forEach(a => a.free());
});

test("Decoder lastError prop", () => {
	const bytes = new Uint8Array([0xF0, 0x00, 0xCE, 0xF3]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);

	const instr1 = decoder.decode();
	expect(instr1.code).toBe(Code.INVALID);
	expect(decoder.lastError).toBe(DecoderError.InvalidInstruction);
	const instr2 = decoder.decode();
	expect(instr2.code).toBe(Code.INVALID);
	expect(decoder.lastError).toBe(DecoderError.NoMoreBytes);

	decoder.free();
	instr1.free();
	instr2.free();
});

test("Decoder constant offsets", () => {
	const bytes = new Uint8Array([0x90, 0x83, 0xB3, 0x34, 0x12, 0x5A, 0xA5, 0x5A]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);

	const instr1 = decoder.decode();
	const co1 = decoder.getConstantOffsets(instr1);
	expect(co1.hasDisplacement).toBe(false);
	expect(co1.displacementOffset).toBe(0);
	expect(co1.displacementSize).toBe(0);
	expect(co1.hasImmediate).toBe(false);
	expect(co1.immediateOffset).toBe(0);
	expect(co1.immediateSize).toBe(0);
	expect(co1.hasImmediate2).toBe(false);
	expect(co1.immediateOffset2).toBe(0);
	expect(co1.immediateSize2).toBe(0);

	const instr2 = decoder.decode();
	const co2 = decoder.getConstantOffsets(instr2);
	expect(co2.hasDisplacement).toBe(true);
	expect(co2.displacementOffset).toBe(2);
	expect(co2.displacementSize).toBe(4);
	expect(co2.hasImmediate).toBe(true);
	expect(co2.immediateOffset).toBe(6);
	expect(co2.immediateSize).toBe(1);
	expect(co2.hasImmediate2).toBe(false);
	expect(co2.immediateOffset2).toBe(0);
	expect(co2.immediateSize2).toBe(0);

	decoder.free();
	instr1.free();
	instr2.free();
	co1.free();
	co2.free();
});
