// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

const { Decoder, DecoderOptions, BlockEncoder, BlockEncoderOptions } = require("iced-x86");

test("BlockEncoder: 16-bit", () => {
	const bytes = new Uint8Array([0x90, 0x75, 0x02, 0xF3, 0x90, 0x01, 0x18]);
	const decoder = new Decoder(16, bytes, DecoderOptions.None);
	decoder.ip = 0x8123n;
	const instrs = decoder.decodeAll();
	const blockEncoder = new BlockEncoder(16, BlockEncoderOptions.None);

	expect(instrs.length).toBe(4);
	instrs.forEach(instr => blockEncoder.add(instr));
	const newCode = blockEncoder.encode(0x1234n);
	expect(newCode).toStrictEqual(new Uint8Array([0x90, 0x75, 0x02, 0xF3, 0x90, 0x01, 0x18]));

	decoder.free();
	instrs.forEach(a => a.free());
	blockEncoder.free();
});

test("BlockEncoder: 32-bit", () => {
	const bytes = new Uint8Array([0x90, 0x75, 0x02, 0xF3, 0x90, 0x01, 0x18]);
	const decoder = new Decoder(32, bytes, DecoderOptions.None);
	decoder.ip = 0x81234567n;
	const instrs = decoder.decodeAll();
	const blockEncoder = new BlockEncoder(32, BlockEncoderOptions.None);

	expect(instrs.length).toBe(4);
	instrs.forEach(instr => blockEncoder.add(instr));
	const newCode = blockEncoder.encode(0x12345678n);
	expect(newCode).toStrictEqual(new Uint8Array([0x90, 0x75, 0x02, 0xF3, 0x90, 0x01, 0x18]));

	decoder.free();
	instrs.forEach(a => a.free());
	blockEncoder.free();
});

test("BlockEncoder: 64-bit", () => {
	const bytes = new Uint8Array([0x90, 0x75, 0x02, 0xF3, 0x90, 0x01, 0x18]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);
	decoder.ip = 0x81234567ABCDEF01n;
	const instrs = decoder.decodeAll();
	const blockEncoder = new BlockEncoder(64, BlockEncoderOptions.None);

	expect(instrs.length).toBe(4);
	instrs.forEach(instr => blockEncoder.add(instr));
	const newCode = blockEncoder.encode(0x5AA51234DEFABC12n);
	expect(newCode).toStrictEqual(new Uint8Array([0x90, 0x75, 0x02, 0xF3, 0x90, 0x01, 0x18]));

	decoder.free();
	instrs.forEach(a => a.free());
	blockEncoder.free();
});

// Make sure it's not an enum arg in the Rust code since it's a flags enum. It must be a u32 in the method sig.
test("BlockEncoder flags", () => {
	const blockEncoder1 = new BlockEncoder(64, BlockEncoderOptions.DontFixBranches |
		BlockEncoderOptions.ReturnConstantOffsets | BlockEncoderOptions.ReturnNewInstructionOffsets |
		BlockEncoderOptions.ReturnRelocInfos);
	blockEncoder1.free();
});
