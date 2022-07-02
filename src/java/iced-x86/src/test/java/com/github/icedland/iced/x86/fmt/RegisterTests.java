// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.Locale;

import static org.junit.jupiter.api.Assertions.*;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;

import com.github.icedland.iced.x86.internal.IcedConstants;

final class RegisterTests {
	@ParameterizedTest
	@MethodSource("gas_Format_Data")
	void gas_Format(int register, String formattedString) {
		formatTests(register, formattedString, GasFormatterFactory.create_Registers(false));
	}

	public static Iterable<Arguments> gas_Format_Data() {
		return getFormatData("Gas", "RegisterTests_1");
	}

	@ParameterizedTest
	@MethodSource("gas_Format2_Data")
	void gas_Format2(int register, String formattedString) {
		formatTests(register, formattedString, GasFormatterFactory.create_Registers(true));
	}

	public static Iterable<Arguments> gas_Format2_Data() {
		return getFormatData("Gas", "RegisterTests_2");
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data")
	void intel_Format(int register, String formattedString) {
		formatTests(register, formattedString, IntelFormatterFactory.create_Registers());
	}

	public static Iterable<Arguments> intel_Format_Data() {
		return getFormatData("Intel", "RegisterTests");
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data")
	void masm_Format(int register, String formattedString) {
		formatTests(register, formattedString, MasmFormatterFactory.create_Registers());
	}

	public static Iterable<Arguments> masm_Format_Data() {
		return getFormatData("Masm", "RegisterTests");
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data")
	void nasm_Format(int register, String formattedString) {
		formatTests(register, formattedString, NasmFormatterFactory.create_Registers());
	}

	public static Iterable<Arguments> nasm_Format_Data() {
		return getFormatData("Nasm", "RegisterTests");
	}

	private static Iterable<Arguments> getFormatData(String formatterDir, String formattedRegistersFile) {
		ArrayList<String> formattedRegisters = FmtFileUtils.readRawStrings(Paths.get(formatterDir, formattedRegistersFile).toString());
		if (IcedConstants.REGISTER_ENUM_COUNT != formattedRegisters.size())
			throw new UnsupportedOperationException();
		ArrayList<Arguments> result = new ArrayList<Arguments>(formattedRegisters.size());
		for (int i = 0; i < formattedRegisters.size(); i++)
			result.add(Arguments.of(i, formattedRegisters.get(i)));
		return result;
	}

	private void formatTests(int register, String formattedString, Formatter formatter) {
		{
			String actualFormattedString = formatter.formatRegister(register);
			assertEquals(formattedString, actualFormattedString);
		}
		{
			formatter.getOptions().setUppercaseRegisters(false);
			String actualFormattedString = formatter.formatRegister(register);
			assertEquals(formattedString.toLowerCase(Locale.ROOT), actualFormattedString);
		}
		{
			formatter.getOptions().setUppercaseRegisters(true);
			String actualFormattedString = formatter.formatRegister(register);
			assertEquals(formattedString.toUpperCase(Locale.ROOT), actualFormattedString);
		}
	}
}
