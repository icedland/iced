// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.api.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;

import com.github.icedland.iced.x86.Code;
import com.github.icedland.iced.x86.HexUtils;
import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.PathUtils;
import com.github.icedland.iced.x86.Register;
import com.github.icedland.iced.x86.dec.ByteArrayCodeReader;
import com.github.icedland.iced.x86.dec.Decoder;
import com.github.icedland.iced.x86.fmt.fast.FastFormatter;
import com.github.icedland.iced.x86.fmt.fast.FastFormatterOptions;
import com.github.icedland.iced.x86.fmt.gas.GasFormatter;
import com.github.icedland.iced.x86.fmt.intel.IntelFormatter;
import com.github.icedland.iced.x86.fmt.masm.MasmFormatter;
import com.github.icedland.iced.x86.fmt.nasm.NasmFormatter;
import com.github.icedland.iced.x86.internal.IcedConstants;

final class MiscTests {
	@Test
	void test_FormatterOperandOptions_properties() {
		FormatterOperandOptions options = new FormatterOperandOptions();

		options.setMemorySizeOptions(MemorySizeOptions.ALWAYS);
		assertEquals(MemorySizeOptions.ALWAYS, options.getMemorySizeOptions());

		options.setMemorySizeOptions(MemorySizeOptions.MINIMAL);
		assertEquals(MemorySizeOptions.MINIMAL, options.getMemorySizeOptions());

		options.setMemorySizeOptions(MemorySizeOptions.NEVER);
		assertEquals(MemorySizeOptions.NEVER, options.getMemorySizeOptions());

		options.setMemorySizeOptions(MemorySizeOptions.DEFAULT);
		assertEquals(MemorySizeOptions.DEFAULT, options.getMemorySizeOptions());

		options.setBranchSize(true);
		assertTrue(options.getBranchSize());
		options.setBranchSize(false);
		assertFalse(options.getBranchSize());

		options.setRipRelativeAddresses(true);
		assertTrue(options.getRipRelativeAddresses());
		options.setRipRelativeAddresses(false);
		assertFalse(options.getRipRelativeAddresses());
	}

	@Test
	void numberFormattingOptions_ctor_throws_if_null_options() {
		assertThrows(NullPointerException.class, () -> new NumberFormattingOptions(null, false, false, false));
	}

	public static Iterable<Arguments> getAllFormatters() {
		ArrayList<Arguments> result = new ArrayList<Arguments>();
		for (Formatter formatter : Utils.getAllFormatters())
			result.add(Arguments.of(formatter));
		return result;
	}

	@ParameterizedTest
	@MethodSource("getAllFormatters")
	void methods_throw_if_null_input(Formatter formatter) {
		Instruction instruction = new Instruction();
		instruction.setCode(Code.MOV_RM64_R64);
		instruction.setOp0Register(Register.RAX);
		instruction.setOp1Register(Register.RCX);
		assertThrows(NullPointerException.class, () -> formatter.formatMnemonic(instruction, null));
		assertThrows(NullPointerException.class, () -> formatter.formatMnemonic(instruction, null, FormatMnemonicOptions.NONE));
		assertThrows(NullPointerException.class, () -> formatter.formatOperand(instruction, null, 0));
		assertThrows(NullPointerException.class, () -> formatter.formatOperandSeparator(instruction, null));
		assertThrows(NullPointerException.class, () -> formatter.formatAllOperands(instruction, null));
		assertThrows(NullPointerException.class, () -> formatter.format(instruction, null));
	}

	@ParameterizedTest
	@MethodSource("getAllFormatters")
	void methods_throw_if_invalid_operand_or_instructionOperand(Formatter formatter) {
		{
			Instruction instruction = new Instruction();
			instruction.setCode(Code.MOV_RM64_R64);
			instruction.setOp0Register(Register.RAX);
			instruction.setOp1Register(Register.RCX);
			final int numOps = 2;
			final int numInstrOps = 2;
			assertEquals(numOps, formatter.getOperandCount(instruction));
			assertEquals(numInstrOps, instruction.getOpCount());
			assertThrows(IllegalArgumentException.class, () -> formatter.tryGetOpAccess(instruction, -1));
			assertThrows(IllegalArgumentException.class, () -> formatter.tryGetOpAccess(instruction, numOps));
			assertThrows(IllegalArgumentException.class, () -> formatter.getInstructionOperand(instruction, -1));
			assertThrows(IllegalArgumentException.class, () -> formatter.getInstructionOperand(instruction, numOps));
			assertThrows(IllegalArgumentException.class, () -> formatter.getFormatterOperand(instruction, -1));
			assertThrows(IllegalArgumentException.class, () -> formatter.getFormatterOperand(instruction, numInstrOps));
			assertThrows(IllegalArgumentException.class, () -> formatter.formatOperand(instruction, new StringOutput(), -1));
			assertThrows(IllegalArgumentException.class, () -> formatter.formatOperand(instruction, new StringOutput(), numOps));
		}

		{
			Instruction invalid = new Instruction();
			assertThrows(IllegalArgumentException.class, () -> formatter.tryGetOpAccess(invalid, -1));
			assertThrows(IllegalArgumentException.class, () -> formatter.tryGetOpAccess(invalid, 0));
			assertThrows(IllegalArgumentException.class, () -> formatter.getInstructionOperand(invalid, -1));
			assertThrows(IllegalArgumentException.class, () -> formatter.getInstructionOperand(invalid, 0));
			assertThrows(IllegalArgumentException.class, () -> formatter.getFormatterOperand(invalid, -1));
			assertThrows(IllegalArgumentException.class, () -> formatter.getFormatterOperand(invalid, 0));
			assertThrows(IllegalArgumentException.class, () -> formatter.formatOperand(invalid, new StringOutput(), -1));
			assertThrows(IllegalArgumentException.class, () -> formatter.formatOperand(invalid, new StringOutput(), 0));
		}

		{
			Instruction db = Instruction.createDeclareByte(new byte[8]);
			assertThrows(IllegalArgumentException.class, () -> formatter.tryGetOpAccess(db, -1));
			assertThrows(IllegalArgumentException.class, () -> formatter.getInstructionOperand(db, -1));
			assertThrows(IllegalArgumentException.class, () -> formatter.getFormatterOperand(db, -1));
			assertThrows(IllegalArgumentException.class, () -> formatter.formatOperand(db, new StringOutput(), -1));
			assertEquals(8, db.getDeclareDataCount());
			for (int i = 0; i < db.getDeclareDataCount(); i++) {
				formatter.tryGetOpAccess(db, i);
				formatter.getInstructionOperand(db, i);
				formatter.formatOperand(db, new StringOutput(), i);
			}
			for (int i = db.getDeclareDataCount(); i < 17; i++) {
				final int ii = i;
				assertThrows(IllegalArgumentException.class, () -> formatter.tryGetOpAccess(db, ii));
				assertThrows(IllegalArgumentException.class, () -> formatter.getInstructionOperand(db, ii));
				assertThrows(IllegalArgumentException.class, () -> formatter.formatOperand(db, new StringOutput(), ii));
			}
			assertThrows(IllegalArgumentException.class, () -> formatter.getFormatterOperand(db, 0));
		}

		{
			Instruction dw = Instruction.createDeclareWord(new byte[8]);
			assertThrows(IllegalArgumentException.class, () -> formatter.tryGetOpAccess(dw, -1));
			assertThrows(IllegalArgumentException.class, () -> formatter.getInstructionOperand(dw, -1));
			assertThrows(IllegalArgumentException.class, () -> formatter.getFormatterOperand(dw, -1));
			assertThrows(IllegalArgumentException.class, () -> formatter.formatOperand(dw, new StringOutput(), -1));
			assertEquals(4, dw.getDeclareDataCount());
			for (int i = 0; i < dw.getDeclareDataCount(); i++) {
				formatter.tryGetOpAccess(dw, i);
				formatter.getInstructionOperand(dw, i);
				formatter.formatOperand(dw, new StringOutput(), i);
			}
			for (int i = dw.getDeclareDataCount(); i < 17; i++) {
				final int ii = i;
				assertThrows(IllegalArgumentException.class, () -> formatter.tryGetOpAccess(dw, ii));
				assertThrows(IllegalArgumentException.class, () -> formatter.getInstructionOperand(dw, ii));
				assertThrows(IllegalArgumentException.class, () -> formatter.formatOperand(dw, new StringOutput(), ii));
			}
			assertThrows(IllegalArgumentException.class, () -> formatter.getFormatterOperand(dw, 0));
		}

		{
			Instruction dd = Instruction.createDeclareDword(new byte[8]);
			assertThrows(IllegalArgumentException.class, () -> formatter.tryGetOpAccess(dd, -1));
			assertThrows(IllegalArgumentException.class, () -> formatter.getInstructionOperand(dd, -1));
			assertThrows(IllegalArgumentException.class, () -> formatter.getFormatterOperand(dd, -1));
			assertThrows(IllegalArgumentException.class, () -> formatter.formatOperand(dd, new StringOutput(), -1));
			assertEquals(2, dd.getDeclareDataCount());
			for (int i = 0; i < dd.getDeclareDataCount(); i++) {
				formatter.tryGetOpAccess(dd, i);
				formatter.getInstructionOperand(dd, i);
				formatter.formatOperand(dd, new StringOutput(), i);
			}
			for (int i = dd.getDeclareDataCount(); i < 17; i++) {
				final int ii = i;
				assertThrows(IllegalArgumentException.class, () -> formatter.tryGetOpAccess(dd, ii));
				assertThrows(IllegalArgumentException.class, () -> formatter.getInstructionOperand(dd, ii));
				assertThrows(IllegalArgumentException.class, () -> formatter.formatOperand(dd, new StringOutput(), ii));
			}
			assertThrows(IllegalArgumentException.class, () -> formatter.getFormatterOperand(dd, 0));
		}

		{
			Instruction dq = Instruction.createDeclareQword(new byte[8]);
			assertThrows(IllegalArgumentException.class, () -> formatter.tryGetOpAccess(dq, -1));
			assertThrows(IllegalArgumentException.class, () -> formatter.getInstructionOperand(dq, -1));
			assertThrows(IllegalArgumentException.class, () -> formatter.getFormatterOperand(dq, -1));
			assertThrows(IllegalArgumentException.class, () -> formatter.formatOperand(dq, new StringOutput(), -1));
			assertEquals(1, dq.getDeclareDataCount());
			for (int i = 0; i < dq.getDeclareDataCount(); i++) {
				formatter.tryGetOpAccess(dq, i);
				formatter.getInstructionOperand(dq, i);
				formatter.formatOperand(dq, new StringOutput(), i);
			}
			for (int i = dq.getDeclareDataCount(); i < 17; i++) {
				final int ii = i;
				assertThrows(IllegalArgumentException.class, () -> formatter.tryGetOpAccess(dq, ii));
				assertThrows(IllegalArgumentException.class, () -> formatter.getInstructionOperand(dq, ii));
				assertThrows(IllegalArgumentException.class, () -> formatter.formatOperand(dq, new StringOutput(), ii));
			}
			assertThrows(IllegalArgumentException.class, () -> formatter.getFormatterOperand(dq, 0));
		}
	}

	@Test
	void stringOutput_throws_if_invalid_input() {
		assertThrows(NullPointerException.class, () -> new StringOutput(null));
	}

	@Test
	void stringOutput_uses_input_sb() {
		StringBuilder sb = new StringBuilder();
		sb.append("Text");
		StringOutput output = new StringOutput(sb);
		output.write("hello", FormatterTextKind.TEXT);
		assertEquals("Texthello", sb.toString());
		assertEquals("Texthello", output.toString());
	}

	@Test
	void verify_default_formatter_options() {
		FormatterOptions options = new FormatterOptions();
		assertFalse(options.getUppercasePrefixes());
		assertFalse(options.getUppercaseMnemonics());
		assertFalse(options.getUppercaseRegisters());
		assertFalse(options.getUppercaseKeywords());
		assertFalse(options.getUppercaseDecorators());
		assertFalse(options.getUppercaseAll());
		assertEquals(0, options.getFirstOperandCharIndex());
		assertEquals(0, options.getTabSize());
		assertFalse(options.getSpaceAfterOperandSeparator());
		assertFalse(options.getSpaceAfterMemoryBracket());
		assertFalse(options.getSpaceBetweenMemoryAddOperators());
		assertFalse(options.getSpaceBetweenMemoryMulOperators());
		assertFalse(options.getScaleBeforeIndex());
		assertFalse(options.getAlwaysShowScale());
		assertFalse(options.getAlwaysShowSegmentRegister());
		assertFalse(options.getShowZeroDisplacements());
		assertNull(options.getHexPrefix());
		assertNull(options.getHexSuffix());
		assertEquals(4, options.getHexDigitGroupSize());
		assertNull(options.getDecimalPrefix());
		assertNull(options.getDecimalSuffix());
		assertEquals(3, options.getDecimalDigitGroupSize());
		assertNull(options.getOctalPrefix());
		assertNull(options.getOctalSuffix());
		assertEquals(4, options.getOctalDigitGroupSize());
		assertNull(options.getBinaryPrefix());
		assertNull(options.getBinarySuffix());
		assertEquals(4, options.getBinaryDigitGroupSize());
		assertNull(options.getDigitSeparator());
		assertFalse(options.getLeadingZeros());
		assertTrue(options.getUppercaseHex());
		assertTrue(options.getSmallHexNumbersInDecimal());
		assertTrue(options.getAddLeadingZeroToHexNumbers());
		assertEquals(NumberBase.HEXADECIMAL, options.getNumberBase());
		assertTrue(options.getBranchLeadingZeros());
		assertFalse(options.getSignedImmediateOperands());
		assertTrue(options.getSignedMemoryDisplacements());
		assertFalse(options.getDisplacementLeadingZeros());
		assertEquals(MemorySizeOptions.DEFAULT, options.getMemorySizeOptions());
		assertFalse(options.getRipRelativeAddresses());
		assertTrue(options.getShowBranchSize());
		assertTrue(options.getUsePseudoOps());
		assertFalse(options.getShowSymbolAddress());
		assertFalse(options.getPreferST0());
		assertEquals(CC_b.B, options.getCC_b());
		assertEquals(CC_ae.AE, options.getCC_ae());
		assertEquals(CC_e.E, options.getCC_e());
		assertEquals(CC_ne.NE, options.getCC_ne());
		assertEquals(CC_be.BE, options.getCC_be());
		assertEquals(CC_a.A, options.getCC_a());
		assertEquals(CC_p.P, options.getCC_p());
		assertEquals(CC_np.NP, options.getCC_np());
		assertEquals(CC_l.L, options.getCC_l());
		assertEquals(CC_ge.GE, options.getCC_ge());
		assertEquals(CC_le.LE, options.getCC_le());
		assertEquals(CC_g.G, options.getCC_g());
		assertFalse(options.getShowUselessPrefixes());
		assertFalse(options.getGasNakedRegisters());
		assertFalse(options.getGasShowMnemonicSizeSuffix());
		assertFalse(options.getGasSpaceAfterMemoryOperandComma());
		assertTrue(options.getMasmAddDsPrefix32());
		assertTrue(options.getMasmSymbolDisplInBrackets());
		assertTrue(options.getMasmDisplInBrackets());
		assertFalse(options.getNasmShowSignExtendedImmediateSize());
	}

	@Test
	void throws_if_invalid_CC_value() {
		assertThrows(IllegalArgumentException.class, () -> new FormatterOptions().setCC_b(IcedConstants.CC_B_ENUM_COUNT));
		assertThrows(IllegalArgumentException.class, () -> new FormatterOptions().setCC_ae(IcedConstants.CC_AE_ENUM_COUNT));
		assertThrows(IllegalArgumentException.class, () -> new FormatterOptions().setCC_e(IcedConstants.CC_E_ENUM_COUNT));
		assertThrows(IllegalArgumentException.class, () -> new FormatterOptions().setCC_ne(IcedConstants.CC_NE_ENUM_COUNT));
		assertThrows(IllegalArgumentException.class, () -> new FormatterOptions().setCC_be(IcedConstants.CC_BE_ENUM_COUNT));
		assertThrows(IllegalArgumentException.class, () -> new FormatterOptions().setCC_a(IcedConstants.CC_A_ENUM_COUNT));
		assertThrows(IllegalArgumentException.class, () -> new FormatterOptions().setCC_p(IcedConstants.CC_P_ENUM_COUNT));
		assertThrows(IllegalArgumentException.class, () -> new FormatterOptions().setCC_np(IcedConstants.CC_NP_ENUM_COUNT));
		assertThrows(IllegalArgumentException.class, () -> new FormatterOptions().setCC_l(IcedConstants.CC_L_ENUM_COUNT));
		assertThrows(IllegalArgumentException.class, () -> new FormatterOptions().setCC_ge(IcedConstants.CC_GE_ENUM_COUNT));
		assertThrows(IllegalArgumentException.class, () -> new FormatterOptions().setCC_le(IcedConstants.CC_LE_ENUM_COUNT));
		assertThrows(IllegalArgumentException.class, () -> new FormatterOptions().setCC_g(IcedConstants.CC_G_ENUM_COUNT));
	}

	@Test
	void fast_verify_default_formatter_options() {
		FastFormatterOptions options = new FastFormatter().getOptions();
		assertFalse(options.getSpaceAfterOperandSeparator());
		assertFalse(options.getAlwaysShowSegmentRegister());
		assertTrue(options.getUppercaseHex());
		assertFalse(options.getUseHexPrefix());
		assertFalse(options.getAlwaysShowMemorySize());
		assertFalse(options.getRipRelativeAddresses());
		assertTrue(options.getUsePseudoOps());
		assertFalse(options.getShowSymbolAddress());
	}

	@Test
	void gas_verify_default_formatter_options() {
		FormatterOptions options = new GasFormatter().getOptions();
		assertFalse(options.getUppercasePrefixes());
		assertFalse(options.getUppercaseMnemonics());
		assertFalse(options.getUppercaseRegisters());
		assertFalse(options.getUppercaseKeywords());
		assertFalse(options.getUppercaseDecorators());
		assertFalse(options.getUppercaseAll());
		assertEquals(0, options.getFirstOperandCharIndex());
		assertEquals(0, options.getTabSize());
		assertFalse(options.getSpaceAfterOperandSeparator());
		assertFalse(options.getSpaceAfterMemoryBracket());
		assertFalse(options.getSpaceBetweenMemoryAddOperators());
		assertFalse(options.getSpaceBetweenMemoryMulOperators());
		assertFalse(options.getScaleBeforeIndex());
		assertFalse(options.getAlwaysShowScale());
		assertFalse(options.getAlwaysShowSegmentRegister());
		assertFalse(options.getShowZeroDisplacements());
		assertNull(options.getHexSuffix());
		assertEquals("0x", options.getHexPrefix());
		assertEquals(4, options.getHexDigitGroupSize());
		assertNull(options.getDecimalPrefix());
		assertNull(options.getDecimalSuffix());
		assertEquals(3, options.getDecimalDigitGroupSize());
		assertNull(options.getOctalSuffix());
		assertEquals("0", options.getOctalPrefix());
		assertEquals(4, options.getOctalDigitGroupSize());
		assertNull(options.getBinarySuffix());
		assertEquals("0b", options.getBinaryPrefix());
		assertEquals(4, options.getBinaryDigitGroupSize());
		assertNull(options.getDigitSeparator());
		assertFalse(options.getLeadingZeros());
		assertTrue(options.getUppercaseHex());
		assertTrue(options.getSmallHexNumbersInDecimal());
		assertTrue(options.getAddLeadingZeroToHexNumbers());
		assertEquals(NumberBase.HEXADECIMAL, options.getNumberBase());
		assertTrue(options.getBranchLeadingZeros());
		assertFalse(options.getSignedImmediateOperands());
		assertTrue(options.getSignedMemoryDisplacements());
		assertFalse(options.getDisplacementLeadingZeros());
		assertEquals(MemorySizeOptions.DEFAULT, options.getMemorySizeOptions());
		assertFalse(options.getRipRelativeAddresses());
		assertTrue(options.getShowBranchSize());
		assertTrue(options.getUsePseudoOps());
		assertFalse(options.getShowSymbolAddress());
		assertFalse(options.getPreferST0());
		assertEquals(CC_b.B, options.getCC_b());
		assertEquals(CC_ae.AE, options.getCC_ae());
		assertEquals(CC_e.E, options.getCC_e());
		assertEquals(CC_ne.NE, options.getCC_ne());
		assertEquals(CC_be.BE, options.getCC_be());
		assertEquals(CC_a.A, options.getCC_a());
		assertEquals(CC_p.P, options.getCC_p());
		assertEquals(CC_np.NP, options.getCC_np());
		assertEquals(CC_l.L, options.getCC_l());
		assertEquals(CC_ge.GE, options.getCC_ge());
		assertEquals(CC_le.LE, options.getCC_le());
		assertEquals(CC_g.G, options.getCC_g());
		assertFalse(options.getShowUselessPrefixes());
		assertFalse(options.getGasNakedRegisters());
		assertFalse(options.getGasShowMnemonicSizeSuffix());
		assertFalse(options.getGasSpaceAfterMemoryOperandComma());
		assertTrue(options.getMasmAddDsPrefix32());
		assertTrue(options.getMasmSymbolDisplInBrackets());
		assertTrue(options.getMasmDisplInBrackets());
		assertFalse(options.getNasmShowSignExtendedImmediateSize());
	}

	@ParameterizedTest
	@MethodSource("gas_FormatMnemonicOptions_Data")
	void gas_FormatMnemonicOptions(String hexBytes, int code, int bitness, long ip, String formattedString, int options) {
		Decoder decoder = new Decoder(bitness, new ByteArrayCodeReader(HexUtils.toByteArray(hexBytes)));
		decoder.setIP(ip);
		Instruction instruction = decoder.decode();
		assertEquals(code, instruction.getCode());
		GasFormatter formatter = GasFormatterFactory.create();
		StringOutput output = new StringOutput();
		formatter.formatMnemonic(instruction, output, options);
		String actualFormattedString = output.toStringAndReset();
		assertEquals(formattedString, actualFormattedString);
	}

	public static Iterable<Arguments> gas_FormatMnemonicOptions_Data() {
		ArrayList<Arguments> result = new ArrayList<Arguments>();
		String filename = PathUtils.getTestTextFilename("Formatter", "Gas", "MnemonicOptions.txt");
		for (MnemonicOptionsTestCase tc : MnemonicOptionsTestsReader.readFile(filename))
			result.add(Arguments.of(tc.hexBytes, tc.code, tc.bitness, tc.ip, tc.formattedString, tc.flags));
		return result;
	}

	@Test
	void gas_TestFormattingWithDefaultFormatterCtor() {
		FormatterTestUtils.testFormatterDoesNotThrow(new GasFormatter());
	}

	@Test
	void gas_TestFormattingWithDefaultFormatterCtor2() {
		FormatterTestUtils.testFormatterDoesNotThrow(new GasFormatter((SymbolResolver)null));
	}

	@Test
	void intel_verify_default_formatter_options() {
		FormatterOptions options = new IntelFormatter().getOptions();
		assertFalse(options.getUppercasePrefixes());
		assertFalse(options.getUppercaseMnemonics());
		assertFalse(options.getUppercaseRegisters());
		assertFalse(options.getUppercaseKeywords());
		assertFalse(options.getUppercaseDecorators());
		assertFalse(options.getUppercaseAll());
		assertEquals(0, options.getFirstOperandCharIndex());
		assertEquals(0, options.getTabSize());
		assertFalse(options.getSpaceAfterOperandSeparator());
		assertFalse(options.getSpaceAfterMemoryBracket());
		assertFalse(options.getSpaceBetweenMemoryAddOperators());
		assertFalse(options.getSpaceBetweenMemoryMulOperators());
		assertFalse(options.getScaleBeforeIndex());
		assertFalse(options.getAlwaysShowScale());
		assertFalse(options.getAlwaysShowSegmentRegister());
		assertFalse(options.getShowZeroDisplacements());
		assertNull(options.getHexPrefix());
		assertEquals("h", options.getHexSuffix());
		assertEquals(4, options.getHexDigitGroupSize());
		assertNull(options.getDecimalPrefix());
		assertNull(options.getDecimalSuffix());
		assertEquals(3, options.getDecimalDigitGroupSize());
		assertNull(options.getOctalPrefix());
		assertEquals("o", options.getOctalSuffix());
		assertEquals(4, options.getOctalDigitGroupSize());
		assertNull(options.getBinaryPrefix());
		assertEquals("b", options.getBinarySuffix());
		assertEquals(4, options.getBinaryDigitGroupSize());
		assertNull(options.getDigitSeparator());
		assertFalse(options.getLeadingZeros());
		assertTrue(options.getUppercaseHex());
		assertTrue(options.getSmallHexNumbersInDecimal());
		assertTrue(options.getAddLeadingZeroToHexNumbers());
		assertEquals(NumberBase.HEXADECIMAL, options.getNumberBase());
		assertTrue(options.getBranchLeadingZeros());
		assertFalse(options.getSignedImmediateOperands());
		assertTrue(options.getSignedMemoryDisplacements());
		assertFalse(options.getDisplacementLeadingZeros());
		assertEquals(MemorySizeOptions.DEFAULT, options.getMemorySizeOptions());
		assertFalse(options.getRipRelativeAddresses());
		assertTrue(options.getShowBranchSize());
		assertTrue(options.getUsePseudoOps());
		assertFalse(options.getShowSymbolAddress());
		assertFalse(options.getPreferST0());
		assertEquals(CC_b.B, options.getCC_b());
		assertEquals(CC_ae.AE, options.getCC_ae());
		assertEquals(CC_e.E, options.getCC_e());
		assertEquals(CC_ne.NE, options.getCC_ne());
		assertEquals(CC_be.BE, options.getCC_be());
		assertEquals(CC_a.A, options.getCC_a());
		assertEquals(CC_p.P, options.getCC_p());
		assertEquals(CC_np.NP, options.getCC_np());
		assertEquals(CC_l.L, options.getCC_l());
		assertEquals(CC_ge.GE, options.getCC_ge());
		assertEquals(CC_le.LE, options.getCC_le());
		assertEquals(CC_g.G, options.getCC_g());
		assertFalse(options.getShowUselessPrefixes());
		assertFalse(options.getGasNakedRegisters());
		assertFalse(options.getGasShowMnemonicSizeSuffix());
		assertFalse(options.getGasSpaceAfterMemoryOperandComma());
		assertTrue(options.getMasmAddDsPrefix32());
		assertTrue(options.getMasmSymbolDisplInBrackets());
		assertTrue(options.getMasmDisplInBrackets());
		assertFalse(options.getNasmShowSignExtendedImmediateSize());
	}

	@ParameterizedTest
	@MethodSource("intel_FormatMnemonicOptions_Data")
	void intel_FormatMnemonicOptions(String hexBytes, int code, int bitness, long ip, String formattedString, int options) {
		Decoder decoder = new Decoder(bitness, new ByteArrayCodeReader(HexUtils.toByteArray(hexBytes)));
		decoder.setIP(ip);
		Instruction instruction = decoder.decode();
		assertEquals(code, instruction.getCode());
		IntelFormatter formatter = IntelFormatterFactory.create();
		StringOutput output = new StringOutput();
		formatter.formatMnemonic(instruction, output, options);
		String actualFormattedString = output.toStringAndReset();
		assertEquals(formattedString, actualFormattedString);
	}

	public static Iterable<Arguments> intel_FormatMnemonicOptions_Data() {
		ArrayList<Arguments> result = new ArrayList<Arguments>();
		String filename = PathUtils.getTestTextFilename("Formatter", "Intel", "MnemonicOptions.txt");
		for (MnemonicOptionsTestCase tc : MnemonicOptionsTestsReader.readFile(filename))
			result.add(Arguments.of(tc.hexBytes, tc.code, tc.bitness, tc.ip, tc.formattedString, tc.flags));
		return result;
	}

	@Test
	void intel_TestFormattingWithDefaultFormatterCtor() {
		FormatterTestUtils.testFormatterDoesNotThrow(new IntelFormatter());
	}

	@Test
	void intel_TestFormattingWithDefaultFormatterCtor2() {
		FormatterTestUtils.testFormatterDoesNotThrow(new IntelFormatter((SymbolResolver)null));
	}

	@Test
	void masm_verify_default_formatter_options() {
		FormatterOptions options = new MasmFormatter().getOptions();
		assertFalse(options.getUppercasePrefixes());
		assertFalse(options.getUppercaseMnemonics());
		assertFalse(options.getUppercaseRegisters());
		assertFalse(options.getUppercaseKeywords());
		assertFalse(options.getUppercaseDecorators());
		assertFalse(options.getUppercaseAll());
		assertEquals(0, options.getFirstOperandCharIndex());
		assertEquals(0, options.getTabSize());
		assertFalse(options.getSpaceAfterOperandSeparator());
		assertFalse(options.getSpaceAfterMemoryBracket());
		assertFalse(options.getSpaceBetweenMemoryAddOperators());
		assertFalse(options.getSpaceBetweenMemoryMulOperators());
		assertFalse(options.getScaleBeforeIndex());
		assertFalse(options.getAlwaysShowScale());
		assertFalse(options.getAlwaysShowSegmentRegister());
		assertFalse(options.getShowZeroDisplacements());
		assertNull(options.getHexPrefix());
		assertEquals("h", options.getHexSuffix());
		assertEquals(4, options.getHexDigitGroupSize());
		assertNull(options.getDecimalPrefix());
		assertNull(options.getDecimalSuffix());
		assertEquals(3, options.getDecimalDigitGroupSize());
		assertNull(options.getOctalPrefix());
		assertEquals("o", options.getOctalSuffix());
		assertEquals(4, options.getOctalDigitGroupSize());
		assertNull(options.getBinaryPrefix());
		assertEquals("b", options.getBinarySuffix());
		assertEquals(4, options.getBinaryDigitGroupSize());
		assertNull(options.getDigitSeparator());
		assertFalse(options.getLeadingZeros());
		assertTrue(options.getUppercaseHex());
		assertTrue(options.getSmallHexNumbersInDecimal());
		assertTrue(options.getAddLeadingZeroToHexNumbers());
		assertEquals(NumberBase.HEXADECIMAL, options.getNumberBase());
		assertTrue(options.getBranchLeadingZeros());
		assertFalse(options.getSignedImmediateOperands());
		assertTrue(options.getSignedMemoryDisplacements());
		assertFalse(options.getDisplacementLeadingZeros());
		assertEquals(MemorySizeOptions.DEFAULT, options.getMemorySizeOptions());
		assertFalse(options.getRipRelativeAddresses());
		assertTrue(options.getShowBranchSize());
		assertTrue(options.getUsePseudoOps());
		assertFalse(options.getShowSymbolAddress());
		assertFalse(options.getPreferST0());
		assertEquals(CC_b.B, options.getCC_b());
		assertEquals(CC_ae.AE, options.getCC_ae());
		assertEquals(CC_e.E, options.getCC_e());
		assertEquals(CC_ne.NE, options.getCC_ne());
		assertEquals(CC_be.BE, options.getCC_be());
		assertEquals(CC_a.A, options.getCC_a());
		assertEquals(CC_p.P, options.getCC_p());
		assertEquals(CC_np.NP, options.getCC_np());
		assertEquals(CC_l.L, options.getCC_l());
		assertEquals(CC_ge.GE, options.getCC_ge());
		assertEquals(CC_le.LE, options.getCC_le());
		assertEquals(CC_g.G, options.getCC_g());
		assertFalse(options.getShowUselessPrefixes());
		assertFalse(options.getGasNakedRegisters());
		assertFalse(options.getGasShowMnemonicSizeSuffix());
		assertFalse(options.getGasSpaceAfterMemoryOperandComma());
		assertTrue(options.getMasmAddDsPrefix32());
		assertTrue(options.getMasmSymbolDisplInBrackets());
		assertTrue(options.getMasmDisplInBrackets());
		assertFalse(options.getNasmShowSignExtendedImmediateSize());
	}

	@ParameterizedTest
	@MethodSource("masm_FormatMnemonicOptions_Data")
	void masm_FormatMnemonicOptions(String hexBytes, int code, int bitness, long ip, String formattedString, int options) {
		Decoder decoder = new Decoder(bitness, new ByteArrayCodeReader(HexUtils.toByteArray(hexBytes)));
		decoder.setIP(ip);
		Instruction instruction = decoder.decode();
		assertEquals(code, instruction.getCode());
		MasmFormatter formatter = MasmFormatterFactory.create();
		StringOutput output = new StringOutput();
		formatter.formatMnemonic(instruction, output, options);
		String actualFormattedString = output.toStringAndReset();
		assertEquals(formattedString, actualFormattedString);
	}

	public static Iterable<Arguments> masm_FormatMnemonicOptions_Data() {
		ArrayList<Arguments> result = new ArrayList<Arguments>();
		String filename = PathUtils.getTestTextFilename("Formatter", "Masm", "MnemonicOptions.txt");
		for (MnemonicOptionsTestCase tc : MnemonicOptionsTestsReader.readFile(filename))
			result.add(Arguments.of(tc.hexBytes, tc.code, tc.bitness, tc.ip, tc.formattedString, tc.flags));
		return result;
	}

	@Test
	void masm_TestFormattingWithDefaultFormatterCtor() {
		FormatterTestUtils.testFormatterDoesNotThrow(new MasmFormatter());
	}

	@Test
	void masm_TestFormattingWithDefaultFormatterCtor2() {
		FormatterTestUtils.testFormatterDoesNotThrow(new MasmFormatter((SymbolResolver)null));
	}

	@Test
	void nasm_verify_default_formatter_options() {
		FormatterOptions options = new NasmFormatter().getOptions();
		assertFalse(options.getUppercasePrefixes());
		assertFalse(options.getUppercaseMnemonics());
		assertFalse(options.getUppercaseRegisters());
		assertFalse(options.getUppercaseKeywords());
		assertFalse(options.getUppercaseDecorators());
		assertFalse(options.getUppercaseAll());
		assertEquals(0, options.getFirstOperandCharIndex());
		assertEquals(0, options.getTabSize());
		assertFalse(options.getSpaceAfterOperandSeparator());
		assertFalse(options.getSpaceAfterMemoryBracket());
		assertFalse(options.getSpaceBetweenMemoryAddOperators());
		assertFalse(options.getSpaceBetweenMemoryMulOperators());
		assertFalse(options.getScaleBeforeIndex());
		assertFalse(options.getAlwaysShowScale());
		assertFalse(options.getAlwaysShowSegmentRegister());
		assertFalse(options.getShowZeroDisplacements());
		assertNull(options.getHexPrefix());
		assertEquals("h", options.getHexSuffix());
		assertEquals(4, options.getHexDigitGroupSize());
		assertNull(options.getDecimalPrefix());
		assertNull(options.getDecimalSuffix());
		assertEquals(3, options.getDecimalDigitGroupSize());
		assertNull(options.getOctalPrefix());
		assertEquals("o", options.getOctalSuffix());
		assertEquals(4, options.getOctalDigitGroupSize());
		assertNull(options.getBinaryPrefix());
		assertEquals("b", options.getBinarySuffix());
		assertEquals(4, options.getBinaryDigitGroupSize());
		assertNull(options.getDigitSeparator());
		assertFalse(options.getLeadingZeros());
		assertTrue(options.getUppercaseHex());
		assertTrue(options.getSmallHexNumbersInDecimal());
		assertTrue(options.getAddLeadingZeroToHexNumbers());
		assertEquals(NumberBase.HEXADECIMAL, options.getNumberBase());
		assertTrue(options.getBranchLeadingZeros());
		assertFalse(options.getSignedImmediateOperands());
		assertTrue(options.getSignedMemoryDisplacements());
		assertFalse(options.getDisplacementLeadingZeros());
		assertEquals(MemorySizeOptions.DEFAULT, options.getMemorySizeOptions());
		assertFalse(options.getRipRelativeAddresses());
		assertTrue(options.getShowBranchSize());
		assertTrue(options.getUsePseudoOps());
		assertFalse(options.getShowSymbolAddress());
		assertFalse(options.getPreferST0());
		assertEquals(CC_b.B, options.getCC_b());
		assertEquals(CC_ae.AE, options.getCC_ae());
		assertEquals(CC_e.E, options.getCC_e());
		assertEquals(CC_ne.NE, options.getCC_ne());
		assertEquals(CC_be.BE, options.getCC_be());
		assertEquals(CC_a.A, options.getCC_a());
		assertEquals(CC_p.P, options.getCC_p());
		assertEquals(CC_np.NP, options.getCC_np());
		assertEquals(CC_l.L, options.getCC_l());
		assertEquals(CC_ge.GE, options.getCC_ge());
		assertEquals(CC_le.LE, options.getCC_le());
		assertEquals(CC_g.G, options.getCC_g());
		assertFalse(options.getShowUselessPrefixes());
		assertFalse(options.getGasNakedRegisters());
		assertFalse(options.getGasShowMnemonicSizeSuffix());
		assertFalse(options.getGasSpaceAfterMemoryOperandComma());
		assertTrue(options.getMasmAddDsPrefix32());
		assertTrue(options.getMasmSymbolDisplInBrackets());
		assertTrue(options.getMasmDisplInBrackets());
		assertFalse(options.getNasmShowSignExtendedImmediateSize());
	}

	@ParameterizedTest
	@MethodSource("nasm_FormatMnemonicOptions_Data")
	void nasm_FormatMnemonicOptions(String hexBytes, int code, int bitness, long ip, String formattedString, int options) {
		Decoder decoder = new Decoder(bitness, new ByteArrayCodeReader(HexUtils.toByteArray(hexBytes)));
		decoder.setIP(ip);
		Instruction instruction = decoder.decode();
		assertEquals(code, instruction.getCode());
		NasmFormatter formatter = NasmFormatterFactory.create();
		StringOutput output = new StringOutput();
		formatter.formatMnemonic(instruction, output, options);
		String actualFormattedString = output.toStringAndReset();
		assertEquals(formattedString, actualFormattedString);
	}

	public static Iterable<Arguments> nasm_FormatMnemonicOptions_Data() {
		ArrayList<Arguments> result = new ArrayList<Arguments>();
		String filename = PathUtils.getTestTextFilename("Formatter", "Nasm", "MnemonicOptions.txt");
		for (MnemonicOptionsTestCase tc : MnemonicOptionsTestsReader.readFile(filename))
			result.add(Arguments.of(tc.hexBytes, tc.code, tc.bitness, tc.ip, tc.formattedString, tc.flags));
		return result;
	}

	@Test
	void nasm_TestFormattingWithDefaultFormatterCtor() {
		FormatterTestUtils.testFormatterDoesNotThrow(new NasmFormatter());
	}

	@Test
	void nasm_TestFormattingWithDefaultFormatterCtor2() {
		FormatterTestUtils.testFormatterDoesNotThrow(new NasmFormatter((SymbolResolver)null));
	}
}
