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
	CC_a, CC_ae, CC_b, CC_be, CC_e, CC_g, CC_ge, CC_l, CC_le, CC_ne, CC_np, CC_p,
	Decoder, DecoderOptions, Formatter, FormatterSyntax, FormatMnemonicOptions, getFeatures,
	MemorySizeOptions, Register
} = require("iced-x86");

test("Default gas formatter options", () => {
	const formatter = new Formatter(FormatterSyntax.Gas);

	expect(formatter.addLeadingZeroToHexNumbers).toBe(true);
	expect(formatter.alwaysShowScale).toBe(false);
	expect(formatter.alwaysShowSegmentRegister).toBe(false);
	expect(formatter.binaryDigitGroupSize).toBe(4);
	expect(formatter.binaryPrefix).toBe("0b");
	expect(formatter.binarySuffix).toBe("");
	expect(formatter.branchLeadingZeroes).toBe(true);
	expect(formatter.decimalDigitGroupSize).toBe(3);
	expect(formatter.decimalPrefix).toBe("");
	expect(formatter.decimalSuffix).toBe("");
	expect(formatter.digitSeparator).toBe("");
	expect(formatter.displacementLeadingZeroes).toBe(false);
	expect(formatter.firstOperandCharIndex).toBe(0);
	expect(formatter.gasNakedRegisters).toBe(false);
	expect(formatter.gasShowMnemonicSizeSuffix).toBe(false);
	expect(formatter.gasSpaceAfterMemoryOperandComma).toBe(false);
	expect(formatter.hexDigitGroupSize).toBe(4);
	expect(formatter.hexPrefix).toBe("0x");
	expect(formatter.hexSuffix).toBe("");
	expect(formatter.leadingZeroes).toBe(false);
	expect(formatter.masmAddDsPrefix32).toBe(true);
	expect(formatter.masmDisplInBrackets).toBe(true);
	expect(formatter.masmSymbolDisplInBrackets).toBe(true);
	expect(formatter.memorySizeOptions).toBe(MemorySizeOptions.Default);
	expect(formatter.nasmShowSignExtendedImmediateSize).toBe(false);
	expect(formatter.numberBase).toBe(16);
	expect(formatter.octalDigitGroupSize).toBe(4);
	expect(formatter.octalPrefix).toBe("0");
	expect(formatter.octalSuffix).toBe("");
	expect(formatter.preferSt0).toBe(false);
	expect(formatter.ripRelativeAddresses).toBe(false);
	expect(formatter.scaleBeforeIndex).toBe(false);
	expect(formatter.showBranchSize).toBe(true);
	expect(formatter.showSymbolAddress).toBe(false);
	expect(formatter.showZeroDisplacements).toBe(false);
	expect(formatter.signedImmediateOperands).toBe(false);
	expect(formatter.signedMemoryDisplacements).toBe(true);
	expect(formatter.smallHexNumbersInDecimal).toBe(true);
	expect(formatter.spaceAfterMemoryBracket).toBe(false);
	expect(formatter.spaceAfterOperandSeparator).toBe(false);
	expect(formatter.spaceBetweenMemoryAddOperators).toBe(false);
	expect(formatter.spaceBetweenMemoryMulOperators).toBe(false);
	expect(formatter.tabSize).toBe(0);
	expect(formatter.uppercaseAll).toBe(false);
	expect(formatter.uppercaseDecorators).toBe(false);
	expect(formatter.uppercaseHex).toBe(true);
	expect(formatter.uppercaseKeywords).toBe(false);
	expect(formatter.uppercaseMnemonics).toBe(false);
	expect(formatter.uppercasePrefixes).toBe(false);
	expect(formatter.uppercaseRegisters).toBe(false);
	expect(formatter.usePseudoOps).toBe(true);
	expect(formatter.cc_b).toBe(CC_b.b);
	expect(formatter.cc_ae).toBe(CC_ae.ae);
	expect(formatter.cc_e).toBe(CC_e.e);
	expect(formatter.cc_ne).toBe(CC_ne.ne);
	expect(formatter.cc_be).toBe(CC_be.be);
	expect(formatter.cc_a).toBe(CC_a.a);
	expect(formatter.cc_p).toBe(CC_p.p);
	expect(formatter.cc_np).toBe(CC_np.np);
	expect(formatter.cc_l).toBe(CC_l.l);
	expect(formatter.cc_ge).toBe(CC_ge.ge);
	expect(formatter.cc_le).toBe(CC_le.le);
	expect(formatter.cc_g).toBe(CC_g.g);

	formatter.free();
});

test("Default Intel formatter options", () => {
	const formatter = new Formatter(FormatterSyntax.Intel);

	expect(formatter.addLeadingZeroToHexNumbers).toBe(true);
	expect(formatter.alwaysShowScale).toBe(false);
	expect(formatter.alwaysShowSegmentRegister).toBe(false);
	expect(formatter.binaryDigitGroupSize).toBe(4);
	expect(formatter.binaryPrefix).toBe("");
	expect(formatter.binarySuffix).toBe("b");
	expect(formatter.branchLeadingZeroes).toBe(true);
	expect(formatter.decimalDigitGroupSize).toBe(3);
	expect(formatter.decimalPrefix).toBe("");
	expect(formatter.decimalSuffix).toBe("");
	expect(formatter.digitSeparator).toBe("");
	expect(formatter.displacementLeadingZeroes).toBe(false);
	expect(formatter.firstOperandCharIndex).toBe(0);
	expect(formatter.gasNakedRegisters).toBe(false);
	expect(formatter.gasShowMnemonicSizeSuffix).toBe(false);
	expect(formatter.gasSpaceAfterMemoryOperandComma).toBe(false);
	expect(formatter.hexDigitGroupSize).toBe(4);
	expect(formatter.hexPrefix).toBe("");
	expect(formatter.hexSuffix).toBe("h");
	expect(formatter.leadingZeroes).toBe(false);
	expect(formatter.masmAddDsPrefix32).toBe(true);
	expect(formatter.masmDisplInBrackets).toBe(true);
	expect(formatter.masmSymbolDisplInBrackets).toBe(true);
	expect(formatter.memorySizeOptions).toBe(MemorySizeOptions.Default);
	expect(formatter.nasmShowSignExtendedImmediateSize).toBe(false);
	expect(formatter.numberBase).toBe(16);
	expect(formatter.octalDigitGroupSize).toBe(4);
	expect(formatter.octalPrefix).toBe("");
	expect(formatter.octalSuffix).toBe("o");
	expect(formatter.preferSt0).toBe(false);
	expect(formatter.ripRelativeAddresses).toBe(false);
	expect(formatter.scaleBeforeIndex).toBe(false);
	expect(formatter.showBranchSize).toBe(true);
	expect(formatter.showSymbolAddress).toBe(false);
	expect(formatter.showZeroDisplacements).toBe(false);
	expect(formatter.signedImmediateOperands).toBe(false);
	expect(formatter.signedMemoryDisplacements).toBe(true);
	expect(formatter.smallHexNumbersInDecimal).toBe(true);
	expect(formatter.spaceAfterMemoryBracket).toBe(false);
	expect(formatter.spaceAfterOperandSeparator).toBe(false);
	expect(formatter.spaceBetweenMemoryAddOperators).toBe(false);
	expect(formatter.spaceBetweenMemoryMulOperators).toBe(false);
	expect(formatter.tabSize).toBe(0);
	expect(formatter.uppercaseAll).toBe(false);
	expect(formatter.uppercaseDecorators).toBe(false);
	expect(formatter.uppercaseHex).toBe(true);
	expect(formatter.uppercaseKeywords).toBe(false);
	expect(formatter.uppercaseMnemonics).toBe(false);
	expect(formatter.uppercasePrefixes).toBe(false);
	expect(formatter.uppercaseRegisters).toBe(false);
	expect(formatter.usePseudoOps).toBe(true);
	expect(formatter.cc_b).toBe(CC_b.b);
	expect(formatter.cc_ae).toBe(CC_ae.ae);
	expect(formatter.cc_e).toBe(CC_e.e);
	expect(formatter.cc_ne).toBe(CC_ne.ne);
	expect(formatter.cc_be).toBe(CC_be.be);
	expect(formatter.cc_a).toBe(CC_a.a);
	expect(formatter.cc_p).toBe(CC_p.p);
	expect(formatter.cc_np).toBe(CC_np.np);
	expect(formatter.cc_l).toBe(CC_l.l);
	expect(formatter.cc_ge).toBe(CC_ge.ge);
	expect(formatter.cc_le).toBe(CC_le.le);
	expect(formatter.cc_g).toBe(CC_g.g);

	formatter.free();
});

test("Default masm formatter options", () => {
	const formatter = new Formatter(FormatterSyntax.Masm);

	expect(formatter.addLeadingZeroToHexNumbers).toBe(true);
	expect(formatter.alwaysShowScale).toBe(false);
	expect(formatter.alwaysShowSegmentRegister).toBe(false);
	expect(formatter.binaryDigitGroupSize).toBe(4);
	expect(formatter.binaryPrefix).toBe("");
	expect(formatter.binarySuffix).toBe("b");
	expect(formatter.branchLeadingZeroes).toBe(true);
	expect(formatter.decimalDigitGroupSize).toBe(3);
	expect(formatter.decimalPrefix).toBe("");
	expect(formatter.decimalSuffix).toBe("");
	expect(formatter.digitSeparator).toBe("");
	expect(formatter.displacementLeadingZeroes).toBe(false);
	expect(formatter.firstOperandCharIndex).toBe(0);
	expect(formatter.gasNakedRegisters).toBe(false);
	expect(formatter.gasShowMnemonicSizeSuffix).toBe(false);
	expect(formatter.gasSpaceAfterMemoryOperandComma).toBe(false);
	expect(formatter.hexDigitGroupSize).toBe(4);
	expect(formatter.hexPrefix).toBe("");
	expect(formatter.hexSuffix).toBe("h");
	expect(formatter.leadingZeroes).toBe(false);
	expect(formatter.masmAddDsPrefix32).toBe(true);
	expect(formatter.masmDisplInBrackets).toBe(true);
	expect(formatter.masmSymbolDisplInBrackets).toBe(true);
	expect(formatter.memorySizeOptions).toBe(MemorySizeOptions.Default);
	expect(formatter.nasmShowSignExtendedImmediateSize).toBe(false);
	expect(formatter.numberBase).toBe(16);
	expect(formatter.octalDigitGroupSize).toBe(4);
	expect(formatter.octalPrefix).toBe("");
	expect(formatter.octalSuffix).toBe("o");
	expect(formatter.preferSt0).toBe(false);
	expect(formatter.ripRelativeAddresses).toBe(false);
	expect(formatter.scaleBeforeIndex).toBe(false);
	expect(formatter.showBranchSize).toBe(true);
	expect(formatter.showSymbolAddress).toBe(false);
	expect(formatter.showZeroDisplacements).toBe(false);
	expect(formatter.signedImmediateOperands).toBe(false);
	expect(formatter.signedMemoryDisplacements).toBe(true);
	expect(formatter.smallHexNumbersInDecimal).toBe(true);
	expect(formatter.spaceAfterMemoryBracket).toBe(false);
	expect(formatter.spaceAfterOperandSeparator).toBe(false);
	expect(formatter.spaceBetweenMemoryAddOperators).toBe(false);
	expect(formatter.spaceBetweenMemoryMulOperators).toBe(false);
	expect(formatter.tabSize).toBe(0);
	expect(formatter.uppercaseAll).toBe(false);
	expect(formatter.uppercaseDecorators).toBe(false);
	expect(formatter.uppercaseHex).toBe(true);
	expect(formatter.uppercaseKeywords).toBe(false);
	expect(formatter.uppercaseMnemonics).toBe(false);
	expect(formatter.uppercasePrefixes).toBe(false);
	expect(formatter.uppercaseRegisters).toBe(false);
	expect(formatter.usePseudoOps).toBe(true);
	expect(formatter.cc_b).toBe(CC_b.b);
	expect(formatter.cc_ae).toBe(CC_ae.ae);
	expect(formatter.cc_e).toBe(CC_e.e);
	expect(formatter.cc_ne).toBe(CC_ne.ne);
	expect(formatter.cc_be).toBe(CC_be.be);
	expect(formatter.cc_a).toBe(CC_a.a);
	expect(formatter.cc_p).toBe(CC_p.p);
	expect(formatter.cc_np).toBe(CC_np.np);
	expect(formatter.cc_l).toBe(CC_l.l);
	expect(formatter.cc_ge).toBe(CC_ge.ge);
	expect(formatter.cc_le).toBe(CC_le.le);
	expect(formatter.cc_g).toBe(CC_g.g);

	formatter.free();
});

test("Default nasm formatter options", () => {
	const formatter = new Formatter(FormatterSyntax.Nasm);

	expect(formatter.addLeadingZeroToHexNumbers).toBe(true);
	expect(formatter.alwaysShowScale).toBe(false);
	expect(formatter.alwaysShowSegmentRegister).toBe(false);
	expect(formatter.binaryDigitGroupSize).toBe(4);
	expect(formatter.binaryPrefix).toBe("");
	expect(formatter.binarySuffix).toBe("b");
	expect(formatter.branchLeadingZeroes).toBe(true);
	expect(formatter.decimalDigitGroupSize).toBe(3);
	expect(formatter.decimalPrefix).toBe("");
	expect(formatter.decimalSuffix).toBe("");
	expect(formatter.digitSeparator).toBe("");
	expect(formatter.displacementLeadingZeroes).toBe(false);
	expect(formatter.firstOperandCharIndex).toBe(0);
	expect(formatter.gasNakedRegisters).toBe(false);
	expect(formatter.gasShowMnemonicSizeSuffix).toBe(false);
	expect(formatter.gasSpaceAfterMemoryOperandComma).toBe(false);
	expect(formatter.hexDigitGroupSize).toBe(4);
	expect(formatter.hexPrefix).toBe("");
	expect(formatter.hexSuffix).toBe("h");
	expect(formatter.leadingZeroes).toBe(false);
	expect(formatter.masmAddDsPrefix32).toBe(true);
	expect(formatter.masmDisplInBrackets).toBe(true);
	expect(formatter.masmSymbolDisplInBrackets).toBe(true);
	expect(formatter.memorySizeOptions).toBe(MemorySizeOptions.Default);
	expect(formatter.nasmShowSignExtendedImmediateSize).toBe(false);
	expect(formatter.numberBase).toBe(16);
	expect(formatter.octalDigitGroupSize).toBe(4);
	expect(formatter.octalPrefix).toBe("");
	expect(formatter.octalSuffix).toBe("o");
	expect(formatter.preferSt0).toBe(false);
	expect(formatter.ripRelativeAddresses).toBe(false);
	expect(formatter.scaleBeforeIndex).toBe(false);
	expect(formatter.showBranchSize).toBe(true);
	expect(formatter.showSymbolAddress).toBe(false);
	expect(formatter.showZeroDisplacements).toBe(false);
	expect(formatter.signedImmediateOperands).toBe(false);
	expect(formatter.signedMemoryDisplacements).toBe(true);
	expect(formatter.smallHexNumbersInDecimal).toBe(true);
	expect(formatter.spaceAfterMemoryBracket).toBe(false);
	expect(formatter.spaceAfterOperandSeparator).toBe(false);
	expect(formatter.spaceBetweenMemoryAddOperators).toBe(false);
	expect(formatter.spaceBetweenMemoryMulOperators).toBe(false);
	expect(formatter.tabSize).toBe(0);
	expect(formatter.uppercaseAll).toBe(false);
	expect(formatter.uppercaseDecorators).toBe(false);
	expect(formatter.uppercaseHex).toBe(true);
	expect(formatter.uppercaseKeywords).toBe(false);
	expect(formatter.uppercaseMnemonics).toBe(false);
	expect(formatter.uppercasePrefixes).toBe(false);
	expect(formatter.uppercaseRegisters).toBe(false);
	expect(formatter.usePseudoOps).toBe(true);
	expect(formatter.cc_b).toBe(CC_b.b);
	expect(formatter.cc_ae).toBe(CC_ae.ae);
	expect(formatter.cc_e).toBe(CC_e.e);
	expect(formatter.cc_ne).toBe(CC_ne.ne);
	expect(formatter.cc_be).toBe(CC_be.be);
	expect(formatter.cc_a).toBe(CC_a.a);
	expect(formatter.cc_p).toBe(CC_p.p);
	expect(formatter.cc_np).toBe(CC_np.np);
	expect(formatter.cc_l).toBe(CC_l.l);
	expect(formatter.cc_ge).toBe(CC_ge.ge);
	expect(formatter.cc_le).toBe(CC_le.le);
	expect(formatter.cc_g).toBe(CC_g.g);

	formatter.free();
});

test("Format instruction: gas", () => {
	// Check if EVEX has been disabled
	if ((getFeatures() & 2) == 0)
		return;
	const formatter = new Formatter(FormatterSyntax.Gas);
	const bytes = new Uint8Array([0x62, 0xF2, 0x4F, 0xDD, 0x72, 0x50, 0x01, 0xF0, 0x00, 0x18]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);
	const instr = decoder.decode();
	const instr2 = decoder.decode();

	expect(formatter.format(instr)).toBe("vcvtne2ps2bf16 4(%rax){1to16},%zmm6,%zmm2{%k5}{z}");
	expect(formatter.formatMnemonic(instr)).toBe("vcvtne2ps2bf16");
	expect(formatter.formatMnemonicOptions(instr, FormatMnemonicOptions.None)).toBe("vcvtne2ps2bf16");
	expect(formatter.formatMnemonicOptions(instr, FormatMnemonicOptions.NoMnemonic)).toBe("");
	expect(formatter.formatMnemonicOptions(instr, FormatMnemonicOptions.NoPrefixes)).toBe("vcvtne2ps2bf16");
	expect(formatter.formatMnemonicOptions(instr, FormatMnemonicOptions.NoMnemonic | FormatMnemonicOptions.NoPrefixes)).toBe("");
	expect(formatter.formatMnemonic(instr2)).toBe("lock add");
	expect(formatter.formatMnemonicOptions(instr2, FormatMnemonicOptions.None)).toBe("lock add");
	expect(formatter.formatMnemonicOptions(instr2, FormatMnemonicOptions.NoMnemonic)).toBe("lock");
	expect(formatter.formatMnemonicOptions(instr2, FormatMnemonicOptions.NoPrefixes)).toBe("add");
	expect(formatter.formatMnemonicOptions(instr2, FormatMnemonicOptions.NoMnemonic | FormatMnemonicOptions.NoPrefixes)).toBe("");
	expect(formatter.operandCount(instr)).toBe(3);
	expect(formatter.opAccess(instr, 0)).toBe(undefined);
	expect(formatter.getInstructionOperand(instr, 0)).toBe(2);
	expect(formatter.getInstructionOperand(instr, 1)).toBe(1);
	expect(formatter.getInstructionOperand(instr, 2)).toBe(0);
	expect(formatter.getFormatterOperand(instr, 0)).toBe(2);
	expect(formatter.getFormatterOperand(instr, 1)).toBe(1);
	expect(formatter.getFormatterOperand(instr, 2)).toBe(0);
	expect(formatter.formatOperand(instr, 0)).toBe("4(%rax){1to16}");
	expect(formatter.formatOperand(instr, 1)).toBe("%zmm6");
	expect(formatter.formatOperand(instr, 2)).toBe("%zmm2{%k5}{z}");
	expect(formatter.formatOperandSeparator(instr)).toBe(",");
	expect(formatter.formatAllOperands(instr)).toBe("4(%rax){1to16},%zmm6,%zmm2{%k5}{z}");
	expect(formatter.formatRegister(Register.RCX)).toBe("%rcx");
	expect(formatter.formatI8(-0x10)).toBe("-0x10");
	expect(formatter.formatI16(-0x10)).toBe("-0x10");
	expect(formatter.formatI32(-0x10)).toBe("-0x10");
	expect(formatter.formatI64(0xFFFFFFFF, 0xFFFFFFF0)).toBe("-0x10");
	expect(formatter.formatU8(0x5A)).toBe("0x5A");
	expect(formatter.formatU16(0x5A)).toBe("0x5A");
	expect(formatter.formatU32(0x5A)).toBe("0x5A");
	expect(formatter.formatU64(0x12345678, 0x9ABCDEF1)).toBe("0x123456789ABCDEF1");

	instr.free();
	instr2.free();
	decoder.free();
	formatter.free();
});

test("Format instruction: Intel", () => {
	// Check if EVEX has been disabled
	if ((getFeatures() & 2) == 0)
		return;
	const formatter = new Formatter(FormatterSyntax.Intel);
	const bytes = new Uint8Array([0x62, 0xF2, 0x4F, 0xDD, 0x72, 0x50, 0x01, 0xF0, 0x00, 0x18]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);
	const instr = decoder.decode();
	const instr2 = decoder.decode();

	expect(formatter.format(instr)).toBe("vcvtne2ps2bf16 zmm2{k5}{z},zmm6,[rax+4]{1to16}");
	expect(formatter.formatMnemonic(instr)).toBe("vcvtne2ps2bf16");
	expect(formatter.formatMnemonicOptions(instr, FormatMnemonicOptions.None)).toBe("vcvtne2ps2bf16");
	expect(formatter.formatMnemonicOptions(instr, FormatMnemonicOptions.NoMnemonic)).toBe("");
	expect(formatter.formatMnemonicOptions(instr, FormatMnemonicOptions.NoPrefixes)).toBe("vcvtne2ps2bf16");
	expect(formatter.formatMnemonicOptions(instr, FormatMnemonicOptions.NoMnemonic | FormatMnemonicOptions.NoPrefixes)).toBe("");
	expect(formatter.formatMnemonic(instr2)).toBe("lock add");
	expect(formatter.formatMnemonicOptions(instr2, FormatMnemonicOptions.None)).toBe("lock add");
	expect(formatter.formatMnemonicOptions(instr2, FormatMnemonicOptions.NoMnemonic)).toBe("lock");
	expect(formatter.formatMnemonicOptions(instr2, FormatMnemonicOptions.NoPrefixes)).toBe("add");
	expect(formatter.formatMnemonicOptions(instr2, FormatMnemonicOptions.NoMnemonic | FormatMnemonicOptions.NoPrefixes)).toBe("");
	expect(formatter.operandCount(instr)).toBe(3);
	expect(formatter.opAccess(instr, 0)).toBe(undefined);
	expect(formatter.getInstructionOperand(instr, 0)).toBe(0);
	expect(formatter.getInstructionOperand(instr, 1)).toBe(1);
	expect(formatter.getInstructionOperand(instr, 2)).toBe(2);
	expect(formatter.getFormatterOperand(instr, 0)).toBe(0);
	expect(formatter.getFormatterOperand(instr, 1)).toBe(1);
	expect(formatter.getFormatterOperand(instr, 2)).toBe(2);
	expect(formatter.formatOperand(instr, 0)).toBe("zmm2{k5}{z}");
	expect(formatter.formatOperand(instr, 1)).toBe("zmm6");
	expect(formatter.formatOperand(instr, 2)).toBe("[rax+4]{1to16}");
	expect(formatter.formatOperandSeparator(instr)).toBe(",");
	expect(formatter.formatAllOperands(instr)).toBe("zmm2{k5}{z},zmm6,[rax+4]{1to16}");
	expect(formatter.formatRegister(Register.RCX)).toBe("rcx");
	expect(formatter.formatI8(-0x10)).toBe("-10h");
	expect(formatter.formatI16(-0x10)).toBe("-10h");
	expect(formatter.formatI32(-0x10)).toBe("-10h");
	expect(formatter.formatI64(0xFFFFFFFF, 0xFFFFFFF0)).toBe("-10h");
	expect(formatter.formatU8(0x5A)).toBe("5Ah");
	expect(formatter.formatU16(0x5A)).toBe("5Ah");
	expect(formatter.formatU32(0x5A)).toBe("5Ah");
	expect(formatter.formatU64(0x12345678, 0x9ABCDEF1)).toBe("123456789ABCDEF1h");

	instr.free();
	instr2.free();
	decoder.free();
	formatter.free();
});

test("Format instruction: masm", () => {
	// Check if EVEX has been disabled
	if ((getFeatures() & 2) == 0)
		return;
	const formatter = new Formatter(FormatterSyntax.Masm);
	const bytes = new Uint8Array([0x62, 0xF2, 0x4F, 0xDD, 0x72, 0x50, 0x01, 0xF0, 0x00, 0x18]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);
	const instr = decoder.decode();
	const instr2 = decoder.decode();

	expect(formatter.format(instr)).toBe("vcvtne2ps2bf16 zmm2{k5}{z},zmm6,dword bcst [rax+4]");
	expect(formatter.formatMnemonic(instr)).toBe("vcvtne2ps2bf16");
	expect(formatter.formatMnemonicOptions(instr, FormatMnemonicOptions.None)).toBe("vcvtne2ps2bf16");
	expect(formatter.formatMnemonicOptions(instr, FormatMnemonicOptions.NoMnemonic)).toBe("");
	expect(formatter.formatMnemonicOptions(instr, FormatMnemonicOptions.NoPrefixes)).toBe("vcvtne2ps2bf16");
	expect(formatter.formatMnemonicOptions(instr, FormatMnemonicOptions.NoMnemonic | FormatMnemonicOptions.NoPrefixes)).toBe("");
	expect(formatter.formatMnemonic(instr2)).toBe("lock add");
	expect(formatter.formatMnemonicOptions(instr2, FormatMnemonicOptions.None)).toBe("lock add");
	expect(formatter.formatMnemonicOptions(instr2, FormatMnemonicOptions.NoMnemonic)).toBe("lock");
	expect(formatter.formatMnemonicOptions(instr2, FormatMnemonicOptions.NoPrefixes)).toBe("add");
	expect(formatter.formatMnemonicOptions(instr2, FormatMnemonicOptions.NoMnemonic | FormatMnemonicOptions.NoPrefixes)).toBe("");
	expect(formatter.operandCount(instr)).toBe(3);
	expect(formatter.opAccess(instr, 0)).toBe(undefined);
	expect(formatter.getInstructionOperand(instr, 0)).toBe(0);
	expect(formatter.getInstructionOperand(instr, 1)).toBe(1);
	expect(formatter.getInstructionOperand(instr, 2)).toBe(2);
	expect(formatter.getFormatterOperand(instr, 0)).toBe(0);
	expect(formatter.getFormatterOperand(instr, 1)).toBe(1);
	expect(formatter.getFormatterOperand(instr, 2)).toBe(2);
	expect(formatter.formatOperand(instr, 0)).toBe("zmm2{k5}{z}");
	expect(formatter.formatOperand(instr, 1)).toBe("zmm6");
	expect(formatter.formatOperand(instr, 2)).toBe("dword bcst [rax+4]");
	expect(formatter.formatOperandSeparator(instr)).toBe(",");
	expect(formatter.formatAllOperands(instr)).toBe("zmm2{k5}{z},zmm6,dword bcst [rax+4]");
	expect(formatter.formatRegister(Register.RCX)).toBe("rcx");
	expect(formatter.formatI8(-0x10)).toBe("-10h");
	expect(formatter.formatI16(-0x10)).toBe("-10h");
	expect(formatter.formatI32(-0x10)).toBe("-10h");
	expect(formatter.formatI64(0xFFFFFFFF, 0xFFFFFFF0)).toBe("-10h");
	expect(formatter.formatU8(0x5A)).toBe("5Ah");
	expect(formatter.formatU16(0x5A)).toBe("5Ah");
	expect(formatter.formatU32(0x5A)).toBe("5Ah");
	expect(formatter.formatU64(0x12345678, 0x9ABCDEF1)).toBe("123456789ABCDEF1h");

	instr.free();
	instr2.free();
	decoder.free();
	formatter.free();
});

test("Format instruction: nasm", () => {
	// Check if EVEX has been disabled
	if ((getFeatures() & 2) == 0)
		return;
	const formatter = new Formatter(FormatterSyntax.Nasm);
	const bytes = new Uint8Array([0x62, 0xF2, 0x4F, 0xDD, 0x72, 0x50, 0x01, 0xF0, 0x00, 0x18]);
	const decoder = new Decoder(64, bytes, DecoderOptions.None);
	const instr = decoder.decode();
	const instr2 = decoder.decode();

	expect(formatter.format(instr)).toBe("vcvtne2ps2bf16 zmm2{k5}{z},zmm6,[rax+4]{1to16}");
	expect(formatter.formatMnemonic(instr)).toBe("vcvtne2ps2bf16");
	expect(formatter.formatMnemonicOptions(instr, FormatMnemonicOptions.None)).toBe("vcvtne2ps2bf16");
	expect(formatter.formatMnemonicOptions(instr, FormatMnemonicOptions.NoMnemonic)).toBe("");
	expect(formatter.formatMnemonicOptions(instr, FormatMnemonicOptions.NoPrefixes)).toBe("vcvtne2ps2bf16");
	expect(formatter.formatMnemonicOptions(instr, FormatMnemonicOptions.NoMnemonic | FormatMnemonicOptions.NoPrefixes)).toBe("");
	expect(formatter.formatMnemonic(instr2)).toBe("lock add");
	expect(formatter.formatMnemonicOptions(instr2, FormatMnemonicOptions.None)).toBe("lock add");
	expect(formatter.formatMnemonicOptions(instr2, FormatMnemonicOptions.NoMnemonic)).toBe("lock");
	expect(formatter.formatMnemonicOptions(instr2, FormatMnemonicOptions.NoPrefixes)).toBe("add");
	expect(formatter.formatMnemonicOptions(instr2, FormatMnemonicOptions.NoMnemonic | FormatMnemonicOptions.NoPrefixes)).toBe("");
	expect(formatter.operandCount(instr)).toBe(3);
	expect(formatter.opAccess(instr, 0)).toBe(undefined);
	expect(formatter.getInstructionOperand(instr, 0)).toBe(0);
	expect(formatter.getInstructionOperand(instr, 1)).toBe(1);
	expect(formatter.getInstructionOperand(instr, 2)).toBe(2);
	expect(formatter.getFormatterOperand(instr, 0)).toBe(0);
	expect(formatter.getFormatterOperand(instr, 1)).toBe(1);
	expect(formatter.getFormatterOperand(instr, 2)).toBe(2);
	expect(formatter.formatOperand(instr, 0)).toBe("zmm2{k5}{z}");
	expect(formatter.formatOperand(instr, 1)).toBe("zmm6");
	expect(formatter.formatOperand(instr, 2)).toBe("[rax+4]{1to16}");
	expect(formatter.formatOperandSeparator(instr)).toBe(",");
	expect(formatter.formatAllOperands(instr)).toBe("zmm2{k5}{z},zmm6,[rax+4]{1to16}");
	expect(formatter.formatRegister(Register.RCX)).toBe("rcx");
	expect(formatter.formatI8(-0x10)).toBe("-10h");
	expect(formatter.formatI16(-0x10)).toBe("-10h");
	expect(formatter.formatI32(-0x10)).toBe("-10h");
	expect(formatter.formatI64(0xFFFFFFFF, 0xFFFFFFF0)).toBe("-10h");
	expect(formatter.formatU8(0x5A)).toBe("5Ah");
	expect(formatter.formatU16(0x5A)).toBe("5Ah");
	expect(formatter.formatU32(0x5A)).toBe("5Ah");
	expect(formatter.formatU64(0x12345678, 0x9ABCDEF1)).toBe("123456789ABCDEF1h");

	instr.free();
	instr2.free();
	decoder.free();
	formatter.free();
});
