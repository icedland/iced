// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

const { Decoder, DecoderOptions, Encoder, getIcedFeatures } = require("iced-x86");

test("Creating an Encoder with an invalid bitness throws", () => {
	expect(() => new Encoder(63)).toThrow();
	expect(() => Encoder.withCapacity(63, 64)).toThrow();
});

test("Encoder options", () => {
	const encoder = new Encoder(64);

	expect(encoder.preventVEX2).toBe(false);
	encoder.preventVEX2 = true;
	expect(encoder.preventVEX2).toBe(true);

	expect(encoder.VEX_WIG).toBe(0);
	encoder.VEX_WIG = 1;
	expect(encoder.VEX_WIG).toBe(1);

	expect(encoder.VEX_LIG).toBe(0);
	encoder.VEX_LIG = 1;
	expect(encoder.VEX_LIG).toBe(1);

	expect(encoder.EVEX_WIG).toBe(0);
	encoder.EVEX_WIG = 1;
	expect(encoder.EVEX_WIG).toBe(1);

	expect(encoder.EVEX_LIG).toBe(0);
	encoder.EVEX_LIG = 1;
	expect(encoder.EVEX_LIG).toBe(1);
	encoder.EVEX_LIG = 2;
	expect(encoder.EVEX_LIG).toBe(2);
	encoder.EVEX_LIG = 3;
	expect(encoder.EVEX_LIG).toBe(3);

	// Check if MVEX support
	if ((getIcedFeatures() & 0x10) != 0) {
		expect(encoder.MVEX_WIG).toBe(0);
		encoder.MVEX_WIG = 1;
		expect(encoder.MVEX_WIG).toBe(1);
	}

	encoder.free();
});

test("16-bit encoder", () => {
	const bytes = new Uint8Array([0x75, 0x02]);
	const decoder = new Decoder(16, bytes, DecoderOptions.None);
	decoder.ip = 0x8123n;
	const instr = decoder.decode();
	const encoder = new Encoder(16);

	expect(encoder.bitness).toBe(16);

	encoder.writeU8(0xCC);
	expect(encoder.encode(instr, 0x8120n)).toBe(2);
	encoder.writeU8(0x90);

	const buffer = encoder.takeBuffer();
	expect(encoder.takeBuffer().length).toBe(0);
	encoder.setBuffer(new Uint8Array([0x11, 0x22, 0x33]));
	expect(encoder.takeBuffer()).toStrictEqual(new Uint8Array([0x11, 0x22, 0x33]));
	expect(buffer).toStrictEqual(new Uint8Array([0xCC, 0x75, 0x05, 0x90]));

	decoder.free();
	instr.free();
	encoder.free();
});

test("32-bit encoder", () => {
	const bytes = new Uint8Array([0x75, 0x02]);
	const decoder = new Decoder(32, bytes, DecoderOptions.None);
	decoder.ip = 0x81234567n;
	const instr = decoder.decode();
	const encoder = new Encoder(32);

	expect(encoder.bitness).toBe(32);

	encoder.writeU8(0x90);
	expect(encoder.encode(instr, 0x81234563n)).toBe(2);
	encoder.writeU8(0xCC);

	const buffer = encoder.takeBuffer();
	expect(encoder.takeBuffer().length).toBe(0);
	encoder.setBuffer(new Uint8Array([0x11, 0x22, 0x33]));
	expect(encoder.takeBuffer()).toStrictEqual(new Uint8Array([0x11, 0x22, 0x33]));
	expect(buffer).toStrictEqual(new Uint8Array([0x90, 0x75, 0x06, 0xCC]));

	decoder.free();
	instr.free();
	encoder.free();
});

test("64-bit encoder", () => {
	const bytes = new Uint8Array([0x75, 0x02]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);
	decoder.ip = 0x81234567ABCDEF01n;
	const instr = decoder.decode();
	const encoder = new Encoder(64);

	expect(encoder.bitness).toBe(64);

	encoder.writeU8(0xCD);
	expect(encoder.encode(instr, 0x81234567ABCDEEFFn)).toBe(2);
	encoder.writeU8(0x91);

	const buffer = encoder.takeBuffer();
	expect(encoder.takeBuffer().length).toBe(0);
	encoder.setBuffer(new Uint8Array([0x11, 0x22, 0x33]));
	expect(encoder.takeBuffer()).toStrictEqual(new Uint8Array([0x11, 0x22, 0x33]));
	expect(buffer).toStrictEqual(new Uint8Array([0xCD, 0x75, 0x04, 0x91]));

	decoder.free();
	instr.free();
	encoder.free();
});

test("Encoder constant offsets", () => {
	const bytes = new Uint8Array([0x90, 0x83, 0xB3, 0x34, 0x12, 0x5A, 0xA5, 0x5A]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);
	const instr1 = decoder.decode();
	const instr2 = decoder.decode();
	const encoder = Encoder.withCapacity(64, 100);

	encoder.encode(instr1, 0n);
	const co1 = encoder.getConstantOffsets();
	expect(co1.hasDisplacement).toBe(false);
	expect(co1.displacementOffset).toBe(0);
	expect(co1.displacementSize).toBe(0);
	expect(co1.hasImmediate).toBe(false);
	expect(co1.immediateOffset).toBe(0);
	expect(co1.immediateSize).toBe(0);
	expect(co1.hasImmediate2).toBe(false);
	expect(co1.immediateOffset2).toBe(0);
	expect(co1.immediateSize2).toBe(0);

	encoder.encode(instr2, 0n);
	const co2 = encoder.getConstantOffsets();
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
	encoder.free();
	instr1.free();
	instr2.free();
	co1.free();
	co2.free();
});
