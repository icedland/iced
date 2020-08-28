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

const { Decoder, DecoderOptions, FastFormatter, getIcedFeatures } = require("iced-x86");

test("Default fast formatter options", () => {
	const formatter = new FastFormatter();

	expect(formatter.spaceAfterOperandSeparator).toBe(false);
	expect(formatter.ripRelativeAddresses).toBe(false);
	expect(formatter.usePseudoOps).toBe(true);
	expect(formatter.showSymbolAddress).toBe(false);
	expect(formatter.alwaysShowSegmentRegister).toBe(false);
	expect(formatter.alwaysShowMemorySize).toBe(false);
	expect(formatter.uppercaseHex).toBe(true);
	expect(formatter.useHexPrefix).toBe(false);

	formatter.free();
});

test("Format instruction: fast fmt", () => {
	// Check if EVEX has been disabled
	if ((getIcedFeatures() & 2) == 0)
		return;
	const formatter = new FastFormatter();
	const bytes = new Uint8Array([0x62, 0xF2, 0x4F, 0xDD, 0x72, 0x50, 0x03]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);
	const instr = decoder.decode();

	expect(formatter.format(instr)).toBe("vcvtne2ps2bf16 zmm2{k5}{z},zmm6,dword bcst [rax+0Ch]");
	formatter.spaceAfterOperandSeparator = true;
	expect(formatter.format(instr)).toBe("vcvtne2ps2bf16 zmm2{k5}{z}, zmm6, dword bcst [rax+0Ch]");
	formatter.useHexPrefix = true;
	expect(formatter.format(instr)).toBe("vcvtne2ps2bf16 zmm2{k5}{z}, zmm6, dword bcst [rax+0xC]");
	formatter.alwaysShowSegmentRegister = true;
	expect(formatter.format(instr)).toBe("vcvtne2ps2bf16 zmm2{k5}{z}, zmm6, dword bcst ds:[rax+0xC]");
	formatter.uppercaseHex = false;
	expect(formatter.format(instr)).toBe("vcvtne2ps2bf16 zmm2{k5}{z}, zmm6, dword bcst ds:[rax+0xc]");

	instr.free();
	decoder.free();
	formatter.free();
});
