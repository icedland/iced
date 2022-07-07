// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import java.util.ArrayList;

import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;

import com.github.icedland.iced.x86.internal.IcedConstants;

final class NumberTests {
	@ParameterizedTest
	@MethodSource("gas_Format_Data")
	void gas_Format(int index, TestNumber number, String[] formattedStrings) {
		formatTests(index, number, formattedStrings, GasFormatterFactory.create_Numbers());
	}

	public static Iterable<Arguments> gas_Format_Data() {
		return getFormatData();
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data")
	void intel_Format(int index, TestNumber number, String[] formattedStrings) {
		formatTests(index, number, formattedStrings, IntelFormatterFactory.create_Numbers());
	}

	public static Iterable<Arguments> intel_Format_Data() {
		return getFormatData();
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data")
	void masm_Format(int index, TestNumber number, String[] formattedStrings) {
		formatTests(index, number, formattedStrings, MasmFormatterFactory.create_Numbers());
	}

	public static Iterable<Arguments> masm_Format_Data() {
		return getFormatData();
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data")
	void nasm_Format(int index, TestNumber number, String[] formattedStrings) {
		formatTests(index, number, formattedStrings, NasmFormatterFactory.create_Numbers());
	}

	public static Iterable<Arguments> nasm_Format_Data() {
		return getFormatData();
	}

	private static Iterable<Arguments> getFormatData() {
		TestNumber[] numbers = NumberFileReader.readNumberFile(FmtFileUtils.getFormatterFilename("Number"));
		ArrayList<String[]> formattedNumbers = new ArrayList<String[]>();
		for (String s : FmtFileUtils.readRawStrings("NumberTests")) {
			String[] strings = s.split(",");
			if (strings.length != numberBases.length)
				throw new UnsupportedOperationException(String.format("Invalid line: %s", s));
			for (int i = 0; i < strings.length; i++)
				strings[i] = strings[i].trim();
			formattedNumbers.add(strings);
		}
		if (numbers.length != formattedNumbers.size())
			throw new UnsupportedOperationException(
					String.format("Files don't have the same amount of lines: %d != %d", numbers.length, formattedNumbers.size()));
		ArrayList<Arguments> result = new ArrayList<Arguments>(formattedNumbers.size());
		for (int i = 0; i < formattedNumbers.size(); i++)
			result.add(Arguments.of(i, numbers[i], formattedNumbers.get(i)));
		return result;
	}

	private static final int[] numberBases = new int[] {
			NumberBase.HEXADECIMAL,
			NumberBase.DECIMAL,
			NumberBase.OCTAL,
			NumberBase.BINARY,
	};

	private void formatTests(int index, TestNumber number, String[] formattedStrings, Formatter formatter) {
		if (numberBases.length != IcedConstants.NUMBER_BASE_ENUM_COUNT)
			throw new UnsupportedOperationException();
		if (formattedStrings.length != numberBases.length)
			throw new UnsupportedOperationException();
		for (int i = 0; i < formattedStrings.length; i++) {
			int numberBase = numberBases[i];
			formatter.getOptions().setNumberBase(numberBase);

			String actualFormattedString1;
			String actualFormattedString2;
			NumberFormattingOptions numberOptions = NumberFormattingOptions.createImmediate(formatter.getOptions());
			switch (number.kind) {
			case TestNumberKind.BYTE:
				actualFormattedString1 = formatter.formatInt8((byte)number.number);
				actualFormattedString2 = formatter.formatInt8((byte)number.number, numberOptions);
				break;

			case TestNumberKind.SHORT:
				actualFormattedString1 = formatter.formatInt16((short)number.number);
				actualFormattedString2 = formatter.formatInt16((short)number.number, numberOptions);
				break;

			case TestNumberKind.INT:
				actualFormattedString1 = formatter.formatInt32((int)number.number);
				actualFormattedString2 = formatter.formatInt32((int)number.number, numberOptions);
				break;

			case TestNumberKind.LONG:
				actualFormattedString1 = formatter.formatInt64((long)number.number);
				actualFormattedString2 = formatter.formatInt64((long)number.number, numberOptions);
				break;

			case TestNumberKind.UBYTE:
				actualFormattedString1 = formatter.formatUInt8((byte)number.number);
				actualFormattedString2 = formatter.formatUInt8((byte)number.number, numberOptions);
				break;

			case TestNumberKind.USHORT:
				actualFormattedString1 = formatter.formatUInt16((short)number.number);
				actualFormattedString2 = formatter.formatUInt16((short)number.number, numberOptions);
				break;

			case TestNumberKind.UINT:
				actualFormattedString1 = formatter.formatUInt32((int)number.number);
				actualFormattedString2 = formatter.formatUInt32((int)number.number, numberOptions);
				break;

			case TestNumberKind.ULONG:
				actualFormattedString1 = formatter.formatUInt64((long)number.number);
				actualFormattedString2 = formatter.formatUInt64((long)number.number, numberOptions);
				break;

			default:
				throw new UnsupportedOperationException();
			}
			assertEquals(formattedStrings[i], actualFormattedString1);
			assertEquals(formattedStrings[i], actualFormattedString2);
		}
	}
}
