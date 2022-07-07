// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import com.github.icedland.iced.x86.NumberConverter;
import com.github.icedland.iced.x86.ToCC_a;
import com.github.icedland.iced.x86.ToCC_ae;
import com.github.icedland.iced.x86.ToCC_b;
import com.github.icedland.iced.x86.ToCC_be;
import com.github.icedland.iced.x86.ToCC_e;
import com.github.icedland.iced.x86.ToCC_g;
import com.github.icedland.iced.x86.ToCC_ge;
import com.github.icedland.iced.x86.ToCC_l;
import com.github.icedland.iced.x86.ToCC_le;
import com.github.icedland.iced.x86.ToCC_ne;
import com.github.icedland.iced.x86.ToCC_np;
import com.github.icedland.iced.x86.ToCC_p;
import com.github.icedland.iced.x86.ToDecoderOptions;
import com.github.icedland.iced.x86.ToMemorySizeOptions;
import com.github.icedland.iced.x86.ToNumberBase;
import com.github.icedland.iced.x86.ToOptionsProps;

final class OptionsParser {
	public static OptionsTestCase.Info parseOption(String keyValue) {
		String[] kv = keyValue.split("=", 2);
		if (kv.length != 2)
			throw new UnsupportedOperationException(String.format("Expected key=value: '%s'", keyValue));
		String valueStr = kv[1].trim();
		int prop = ToOptionsProps.get(kv[0].trim());
		Object value;
		switch (prop) {
		case OptionsProps.ADD_LEADING_ZERO_TO_HEX_NUMBERS:
		case OptionsProps.ALWAYS_SHOW_SCALE:
		case OptionsProps.ALWAYS_SHOW_SEGMENT_REGISTER:
		case OptionsProps.BRANCH_LEADING_ZEROS:
		case OptionsProps.DISPLACEMENT_LEADING_ZEROS:
		case OptionsProps.GAS_NAKED_REGISTERS:
		case OptionsProps.GAS_SHOW_MNEMONIC_SIZE_SUFFIX:
		case OptionsProps.GAS_SPACE_AFTER_MEMORY_OPERAND_COMMA:
		case OptionsProps.LEADING_ZEROS:
		case OptionsProps.MASM_ADD_DS_PREFIX32:
		case OptionsProps.NASM_SHOW_SIGN_EXTENDED_IMMEDIATE_SIZE:
		case OptionsProps.PREFER_ST0:
		case OptionsProps.RIP_RELATIVE_ADDRESSES:
		case OptionsProps.SCALE_BEFORE_INDEX:
		case OptionsProps.SHOW_BRANCH_SIZE:
		case OptionsProps.SHOW_SYMBOL_ADDRESS:
		case OptionsProps.SHOW_ZERO_DISPLACEMENTS:
		case OptionsProps.SIGNED_IMMEDIATE_OPERANDS:
		case OptionsProps.SIGNED_MEMORY_DISPLACEMENTS:
		case OptionsProps.SMALL_HEX_NUMBERS_IN_DECIMAL:
		case OptionsProps.SPACE_AFTER_MEMORY_BRACKET:
		case OptionsProps.SPACE_AFTER_OPERAND_SEPARATOR:
		case OptionsProps.SPACE_BETWEEN_MEMORY_ADD_OPERATORS:
		case OptionsProps.SPACE_BETWEEN_MEMORY_MUL_OPERATORS:
		case OptionsProps.UPPERCASE_ALL:
		case OptionsProps.UPPERCASE_DECORATORS:
		case OptionsProps.UPPERCASE_HEX:
		case OptionsProps.UPPERCASE_KEYWORDS:
		case OptionsProps.UPPERCASE_MNEMONICS:
		case OptionsProps.UPPERCASE_PREFIXES:
		case OptionsProps.UPPERCASE_REGISTERS:
		case OptionsProps.USE_PSEUDO_OPS:
		case OptionsProps.SHOW_USELESS_PREFIXES:
			value = NumberConverter.toBoolean(valueStr);
			break;

		case OptionsProps.BINARY_DIGIT_GROUP_SIZE:
		case OptionsProps.DECIMAL_DIGIT_GROUP_SIZE:
		case OptionsProps.FIRST_OPERAND_CHAR_INDEX:
		case OptionsProps.HEX_DIGIT_GROUP_SIZE:
		case OptionsProps.OCTAL_DIGIT_GROUP_SIZE:
		case OptionsProps.TAB_SIZE:
			value = NumberConverter.toInt32(valueStr);
			break;

		case OptionsProps.IP:
			value = NumberConverter.toUInt64(valueStr);
			break;

		case OptionsProps.BINARY_PREFIX:
		case OptionsProps.BINARY_SUFFIX:
		case OptionsProps.DECIMAL_PREFIX:
		case OptionsProps.DECIMAL_SUFFIX:
		case OptionsProps.DIGIT_SEPARATOR:
		case OptionsProps.HEX_PREFIX:
		case OptionsProps.HEX_SUFFIX:
		case OptionsProps.OCTAL_PREFIX:
		case OptionsProps.OCTAL_SUFFIX:
			value = valueStr.equals("<null>") ? null : valueStr;
			break;

		case OptionsProps.MEMORY_SIZE_OPTIONS:
			value = ToMemorySizeOptions.get(valueStr);
			break;

		case OptionsProps.NUMBER_BASE:
			value = ToNumberBase.get(valueStr);
			break;

		case OptionsProps.CC_B:
			value = ToCC_b.get(valueStr);
			break;

		case OptionsProps.CC_AE:
			value = ToCC_ae.get(valueStr);
			break;

		case OptionsProps.CC_E:
			value = ToCC_e.get(valueStr);
			break;

		case OptionsProps.CC_NE:
			value = ToCC_ne.get(valueStr);
			break;

		case OptionsProps.CC_BE:
			value = ToCC_be.get(valueStr);
			break;

		case OptionsProps.CC_A:
			value = ToCC_a.get(valueStr);
			break;

		case OptionsProps.CC_P:
			value = ToCC_p.get(valueStr);
			break;

		case OptionsProps.CC_NP:
			value = ToCC_np.get(valueStr);
			break;

		case OptionsProps.CC_L:
			value = ToCC_l.get(valueStr);
			break;

		case OptionsProps.CC_GE:
			value = ToCC_ge.get(valueStr);
			break;

		case OptionsProps.CC_LE:
			value = ToCC_le.get(valueStr);
			break;

		case OptionsProps.CC_G:
			value = ToCC_g.get(valueStr);
			break;

		case OptionsProps.DECODER_OPTIONS:
			value = ToDecoderOptions.get(valueStr);
			break;

		default:
			throw new UnsupportedOperationException();
		}
		return new OptionsTestCase.Info(prop, value);
	}
}
