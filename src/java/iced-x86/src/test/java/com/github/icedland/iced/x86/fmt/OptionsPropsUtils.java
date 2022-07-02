// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import com.github.icedland.iced.x86.dec.Decoder;
import com.github.icedland.iced.x86.dec.DecoderOptions;
import com.github.icedland.iced.x86.fmt.fast.FastFormatterOptions;

final class OptionsPropsUtils {
	public static int getDecoderOptions(OptionsTestCase.Info[] props) {
		int options = DecoderOptions.NONE;
		for (OptionsTestCase.Info info : props) {
			if (info.property == OptionsProps.DECODER_OPTIONS)
				options |= (int)info.value;
		}
		return options;
	}

	public static void initialize(FormatterOptions options, int property, Object value) {
		switch (property) {
		case OptionsProps.ADD_LEADING_ZERO_TO_HEX_NUMBERS:
			options.setAddLeadingZeroToHexNumbers((boolean)value);
			break;
		case OptionsProps.ALWAYS_SHOW_SCALE:
			options.setAlwaysShowScale((boolean)value);
			break;
		case OptionsProps.ALWAYS_SHOW_SEGMENT_REGISTER:
			options.setAlwaysShowSegmentRegister((boolean)value);
			break;
		case OptionsProps.BINARY_DIGIT_GROUP_SIZE:
			options.setBinaryDigitGroupSize((int)value);
			break;
		case OptionsProps.BINARY_PREFIX:
			options.setBinaryPrefix((String)value);
			break;
		case OptionsProps.BINARY_SUFFIX:
			options.setBinarySuffix((String)value);
			break;
		case OptionsProps.BRANCH_LEADING_ZEROS:
			options.setBranchLeadingZeros((boolean)value);
			break;
		case OptionsProps.DECIMAL_DIGIT_GROUP_SIZE:
			options.setDecimalDigitGroupSize((int)value);
			break;
		case OptionsProps.DECIMAL_PREFIX:
			options.setDecimalPrefix((String)value);
			break;
		case OptionsProps.DECIMAL_SUFFIX:
			options.setDecimalSuffix((String)value);
			break;
		case OptionsProps.DIGIT_SEPARATOR:
			options.setDigitSeparator((String)value);
			break;
		case OptionsProps.DISPLACEMENT_LEADING_ZEROS:
			options.setDisplacementLeadingZeros((boolean)value);
			break;
		case OptionsProps.FIRST_OPERAND_CHAR_INDEX:
			options.setFirstOperandCharIndex((int)value);
			break;
		case OptionsProps.GAS_NAKED_REGISTERS:
			options.setGasNakedRegisters((boolean)value);
			break;
		case OptionsProps.GAS_SHOW_MNEMONIC_SIZE_SUFFIX:
			options.setGasShowMnemonicSizeSuffix((boolean)value);
			break;
		case OptionsProps.GAS_SPACE_AFTER_MEMORY_OPERAND_COMMA:
			options.setGasSpaceAfterMemoryOperandComma((boolean)value);
			break;
		case OptionsProps.HEX_DIGIT_GROUP_SIZE:
			options.setHexDigitGroupSize((int)value);
			break;
		case OptionsProps.HEX_PREFIX:
			options.setHexPrefix((String)value);
			break;
		case OptionsProps.HEX_SUFFIX:
			options.setHexSuffix((String)value);
			break;
		case OptionsProps.LEADING_ZEROS:
			options.setLeadingZeros((boolean)value);
			break;
		case OptionsProps.MASM_ADD_DS_PREFIX32:
			options.setMasmAddDsPrefix32((boolean)value);
			break;
		case OptionsProps.MEMORY_SIZE_OPTIONS:
			options.setMemorySizeOptions((int)value);
			break;
		case OptionsProps.NASM_SHOW_SIGN_EXTENDED_IMMEDIATE_SIZE:
			options.setNasmShowSignExtendedImmediateSize((boolean)value);
			break;
		case OptionsProps.NUMBER_BASE:
			options.setNumberBase((int)value);
			break;
		case OptionsProps.OCTAL_DIGIT_GROUP_SIZE:
			options.setOctalDigitGroupSize((int)value);
			break;
		case OptionsProps.OCTAL_PREFIX:
			options.setOctalPrefix((String)value);
			break;
		case OptionsProps.OCTAL_SUFFIX:
			options.setOctalSuffix((String)value);
			break;
		case OptionsProps.PREFER_ST0:
			options.setPreferST0((boolean)value);
			break;
		case OptionsProps.RIP_RELATIVE_ADDRESSES:
			options.setRipRelativeAddresses((boolean)value);
			break;
		case OptionsProps.SCALE_BEFORE_INDEX:
			options.setScaleBeforeIndex((boolean)value);
			break;
		case OptionsProps.SHOW_BRANCH_SIZE:
			options.setShowBranchSize((boolean)value);
			break;
		case OptionsProps.SHOW_SYMBOL_ADDRESS:
			options.setShowSymbolAddress((boolean)value);
			break;
		case OptionsProps.SHOW_ZERO_DISPLACEMENTS:
			options.setShowZeroDisplacements((boolean)value);
			break;
		case OptionsProps.SIGNED_IMMEDIATE_OPERANDS:
			options.setSignedImmediateOperands((boolean)value);
			break;
		case OptionsProps.SIGNED_MEMORY_DISPLACEMENTS:
			options.setSignedMemoryDisplacements((boolean)value);
			break;
		case OptionsProps.SMALL_HEX_NUMBERS_IN_DECIMAL:
			options.setSmallHexNumbersInDecimal((boolean)value);
			break;
		case OptionsProps.SPACE_AFTER_MEMORY_BRACKET:
			options.setSpaceAfterMemoryBracket((boolean)value);
			break;
		case OptionsProps.SPACE_AFTER_OPERAND_SEPARATOR:
			options.setSpaceAfterOperandSeparator((boolean)value);
			break;
		case OptionsProps.SPACE_BETWEEN_MEMORY_ADD_OPERATORS:
			options.setSpaceBetweenMemoryAddOperators((boolean)value);
			break;
		case OptionsProps.SPACE_BETWEEN_MEMORY_MUL_OPERATORS:
			options.setSpaceBetweenMemoryMulOperators((boolean)value);
			break;
		case OptionsProps.TAB_SIZE:
			options.setTabSize((int)value);
			break;
		case OptionsProps.UPPERCASE_ALL:
			options.setUppercaseAll((boolean)value);
			break;
		case OptionsProps.UPPERCASE_DECORATORS:
			options.setUppercaseDecorators((boolean)value);
			break;
		case OptionsProps.UPPERCASE_HEX:
			options.setUppercaseHex((boolean)value);
			break;
		case OptionsProps.UPPERCASE_KEYWORDS:
			options.setUppercaseKeywords((boolean)value);
			break;
		case OptionsProps.UPPERCASE_MNEMONICS:
			options.setUppercaseMnemonics((boolean)value);
			break;
		case OptionsProps.UPPERCASE_PREFIXES:
			options.setUppercasePrefixes((boolean)value);
			break;
		case OptionsProps.UPPERCASE_REGISTERS:
			options.setUppercaseRegisters((boolean)value);
			break;
		case OptionsProps.USE_PSEUDO_OPS:
			options.setUsePseudoOps((boolean)value);
			break;
		case OptionsProps.CC_B:
			options.setCC_b((int)value);
			break;
		case OptionsProps.CC_AE:
			options.setCC_ae((int)value);
			break;
		case OptionsProps.CC_E:
			options.setCC_e((int)value);
			break;
		case OptionsProps.CC_NE:
			options.setCC_ne((int)value);
			break;
		case OptionsProps.CC_BE:
			options.setCC_be((int)value);
			break;
		case OptionsProps.CC_A:
			options.setCC_a((int)value);
			break;
		case OptionsProps.CC_P:
			options.setCC_p((int)value);
			break;
		case OptionsProps.CC_NP:
			options.setCC_np((int)value);
			break;
		case OptionsProps.CC_L:
			options.setCC_l((int)value);
			break;
		case OptionsProps.CC_GE:
			options.setCC_ge((int)value);
			break;
		case OptionsProps.CC_LE:
			options.setCC_le((int)value);
			break;
		case OptionsProps.CC_G:
			options.setCC_g((int)value);
			break;
		case OptionsProps.SHOW_USELESS_PREFIXES:
			options.setShowUselessPrefixes((boolean)value);
			break;
		case OptionsProps.IP:
		case OptionsProps.DECODER_OPTIONS:
			break;
		default:
			throw new UnsupportedOperationException();
		}
	}

	public static void initialize(FormatterOptions options, OptionsTestCase.Info[] properties) {
		for (OptionsTestCase.Info info : properties)
			initialize(options, info.property, info.value);
	}

	public static void initialize(FastFormatterOptions options, int property, Object value) {
		switch (property) {
		case OptionsProps.ADD_LEADING_ZERO_TO_HEX_NUMBERS:
		case OptionsProps.ALWAYS_SHOW_SCALE:
		case OptionsProps.BINARY_DIGIT_GROUP_SIZE:
		case OptionsProps.BINARY_PREFIX:
		case OptionsProps.BINARY_SUFFIX:
		case OptionsProps.BRANCH_LEADING_ZEROS:
		case OptionsProps.DECIMAL_DIGIT_GROUP_SIZE:
		case OptionsProps.DECIMAL_PREFIX:
		case OptionsProps.DECIMAL_SUFFIX:
		case OptionsProps.DIGIT_SEPARATOR:
		case OptionsProps.DISPLACEMENT_LEADING_ZEROS:
		case OptionsProps.FIRST_OPERAND_CHAR_INDEX:
		case OptionsProps.GAS_NAKED_REGISTERS:
		case OptionsProps.GAS_SHOW_MNEMONIC_SIZE_SUFFIX:
		case OptionsProps.GAS_SPACE_AFTER_MEMORY_OPERAND_COMMA:
		case OptionsProps.HEX_DIGIT_GROUP_SIZE:
		case OptionsProps.LEADING_ZEROS:
		case OptionsProps.MASM_ADD_DS_PREFIX32:
		case OptionsProps.NASM_SHOW_SIGN_EXTENDED_IMMEDIATE_SIZE:
		case OptionsProps.NUMBER_BASE:
		case OptionsProps.OCTAL_DIGIT_GROUP_SIZE:
		case OptionsProps.OCTAL_PREFIX:
		case OptionsProps.OCTAL_SUFFIX:
		case OptionsProps.PREFER_ST0:
		case OptionsProps.SCALE_BEFORE_INDEX:
		case OptionsProps.SHOW_BRANCH_SIZE:
		case OptionsProps.SHOW_ZERO_DISPLACEMENTS:
		case OptionsProps.SIGNED_IMMEDIATE_OPERANDS:
		case OptionsProps.SIGNED_MEMORY_DISPLACEMENTS:
		case OptionsProps.SMALL_HEX_NUMBERS_IN_DECIMAL:
		case OptionsProps.SPACE_AFTER_MEMORY_BRACKET:
		case OptionsProps.SPACE_BETWEEN_MEMORY_ADD_OPERATORS:
		case OptionsProps.SPACE_BETWEEN_MEMORY_MUL_OPERATORS:
		case OptionsProps.TAB_SIZE:
		case OptionsProps.UPPERCASE_ALL:
		case OptionsProps.UPPERCASE_DECORATORS:
		case OptionsProps.UPPERCASE_KEYWORDS:
		case OptionsProps.UPPERCASE_MNEMONICS:
		case OptionsProps.UPPERCASE_PREFIXES:
		case OptionsProps.UPPERCASE_REGISTERS:
		case OptionsProps.CC_B:
		case OptionsProps.CC_AE:
		case OptionsProps.CC_E:
		case OptionsProps.CC_NE:
		case OptionsProps.CC_BE:
		case OptionsProps.CC_A:
		case OptionsProps.CC_P:
		case OptionsProps.CC_NP:
		case OptionsProps.CC_L:
		case OptionsProps.CC_GE:
		case OptionsProps.CC_LE:
		case OptionsProps.CC_G:
		case OptionsProps.SHOW_USELESS_PREFIXES:
			break;

		case OptionsProps.ALWAYS_SHOW_SEGMENT_REGISTER:
			options.setAlwaysShowSegmentRegister((boolean)value);
			break;
		case OptionsProps.RIP_RELATIVE_ADDRESSES:
			options.setRipRelativeAddresses((boolean)value);
			break;
		case OptionsProps.SHOW_SYMBOL_ADDRESS:
			options.setShowSymbolAddress((boolean)value);
			break;
		case OptionsProps.SPACE_AFTER_OPERAND_SEPARATOR:
			options.setSpaceAfterOperandSeparator((boolean)value);
			break;
		case OptionsProps.UPPERCASE_HEX:
			options.setUppercaseHex((boolean)value);
			break;
		case OptionsProps.USE_PSEUDO_OPS:
			options.setUsePseudoOps((boolean)value);
			break;
		case OptionsProps.MEMORY_SIZE_OPTIONS:
			options.setAlwaysShowMemorySize((int)value == MemorySizeOptions.ALWAYS);
			break;

		case OptionsProps.HEX_PREFIX:
			if (value != null && value.equals("0x"))
				options.setUseHexPrefix(true);
			break;

		case OptionsProps.HEX_SUFFIX:
			if (value != null && value.equals("h"))
				options.setUseHexPrefix(false);
			break;

		case OptionsProps.IP:
		case OptionsProps.DECODER_OPTIONS:
			break;

		default:
			throw new UnsupportedOperationException();
		}
	}

	public static void initialize(FastFormatterOptions options, OptionsTestCase.Info[] properties) {
		for (OptionsTestCase.Info info : properties)
			initialize(options, info.property, info.value);
	}

	public static void initialize(Decoder decoder, int property, Object value) {
		switch (property) {
		case OptionsProps.IP:
			decoder.setIP((long)value);
			break;
		}
	}

	public static void initialize(Decoder decoder, OptionsTestCase.Info[] properties) {
		for (OptionsTestCase.Info info : properties)
			initialize(decoder, info.property, info.value);
	}
}
