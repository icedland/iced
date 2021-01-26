// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

const { Decoder, DecoderOptions, BlockEncoder, BlockEncoderOptions } = require("iced-x86");

test("BlockEncoder: 16-bit", () => {
	const bytes = new Uint8Array([0x90, 0x75, 0x02, 0xF3, 0x90, 0x01, 0x18]);
	const decoder = new Decoder(16, bytes, DecoderOptions.None);
	decoder.ipLo = 0x8123;
	decoder.ipHi = 0;
	const instrs = decoder.decodeAll();
	const blockEncoder = new BlockEncoder(16, BlockEncoderOptions.None);

	expect(instrs.length).toBe(4);
	instrs.forEach(instr => blockEncoder.add(instr));
	const newCode = blockEncoder.encode(0, 0x1234);
	expect(newCode).toStrictEqual(new Uint8Array([0x90, 0x75, 0x02, 0xF3, 0x90, 0x01, 0x18]));

	decoder.free();
	instrs.forEach(a => a.free());
	blockEncoder.free();
});

test("BlockEncoder: 32-bit", () => {
	const bytes = new Uint8Array([0x90, 0x75, 0x02, 0xF3, 0x90, 0x01, 0x18]);
	const decoder = new Decoder(32, bytes, DecoderOptions.None);
	decoder.ipLo = 0x81234567;
	decoder.ipHi = 0;
	const instrs = decoder.decodeAll();
	const blockEncoder = new BlockEncoder(32, BlockEncoderOptions.None);

	expect(instrs.length).toBe(4);
	instrs.forEach(instr => blockEncoder.add(instr));
	const newCode = blockEncoder.encode(0, 0x12345678);
	expect(newCode).toStrictEqual(new Uint8Array([0x90, 0x75, 0x02, 0xF3, 0x90, 0x01, 0x18]));

	decoder.free();
	instrs.forEach(a => a.free());
	blockEncoder.free();
});

test("BlockEncoder: 64-bit", () => {
	const bytes = new Uint8Array([0x90, 0x75, 0x02, 0xF3, 0x90, 0x01, 0x18]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);
	decoder.ipLo = 0xABCDEF01;
	decoder.ipHi = 0x81234567;
	const instrs = decoder.decodeAll();
	const blockEncoder = new BlockEncoder(64, BlockEncoderOptions.None);

	expect(instrs.length).toBe(4);
	instrs.forEach(instr => blockEncoder.add(instr));
	const newCode = blockEncoder.encode(0x5AA51234, 0xDEFABC12);
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
