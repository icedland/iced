// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import static org.junit.jupiter.api.Assertions.*;

import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;

import com.github.icedland.iced.x86.ToMemorySizeOptions;
import com.github.icedland.iced.x86.ToNumberBase;
import com.github.icedland.iced.x86.fmt.fast.FastFormatter;
import com.github.icedland.iced.x86.fmt.gas.GasFormatter;
import com.github.icedland.iced.x86.fmt.intel.IntelFormatter;
import com.github.icedland.iced.x86.fmt.masm.MasmFormatter;
import com.github.icedland.iced.x86.fmt.nasm.NasmFormatter;

final class OptionsTests {
	@ParameterizedTest
	@MethodSource("fast_FormatCommon_Data")
	void fast_FormatCommon(int index, OptionsTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, FastFormatterFactory.create_Options());
	}

	public static Iterable<Arguments> fast_FormatCommon_Data() {
		return OptionsTestsUtils.getFormatData_Common("Fast", "OptionsResult.Common");
	}

	@ParameterizedTest
	@MethodSource("fast_Format2_Data")
	void fast_Format2(int index, OptionsTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, FastFormatterFactory.create_Options());
	}

	public static Iterable<Arguments> fast_Format2_Data() {
		return OptionsTestsUtils.getFormatData("Fast", "OptionsResult2", "Options2");
	}

	@ParameterizedTest
	@MethodSource("gas_FormatCommon_Data")
	void gas_FormatCommon(int index, OptionsTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, GasFormatterFactory.create_Options());
	}

	public static Iterable<Arguments> gas_FormatCommon_Data() {
		return OptionsTestsUtils.getFormatData_Common("Gas", "OptionsResult.Common");
	}

	@ParameterizedTest
	@MethodSource("gas_FormatAll_Data")
	void gas_FormatAll(int index, OptionsTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, GasFormatterFactory.create_Options());
	}

	public static Iterable<Arguments> gas_FormatAll_Data() {
		return OptionsTestsUtils.getFormatData_All("Gas", "OptionsResult");
	}

	@ParameterizedTest
	@MethodSource("gas_Format2_Data")
	void gas_Format2(int index, OptionsTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, GasFormatterFactory.create_Options());
	}

	public static Iterable<Arguments> gas_Format2_Data() {
		return OptionsTestsUtils.getFormatData("Gas", "OptionsResult2", "Options2");
	}

	@Test
	public void gas_TestOptions() {
		testOptionsBase(new GasFormatter().getOptions());
	}

	@ParameterizedTest
	@MethodSource("intel_FormatCommon_Data")
	void intel_FormatCommon(int index, OptionsTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, IntelFormatterFactory.create_Options());
	}

	public static Iterable<Arguments> intel_FormatCommon_Data() {
		return OptionsTestsUtils.getFormatData_Common("Intel", "OptionsResult.Common");
	}

	@ParameterizedTest
	@MethodSource("intel_FormatAll_Data")
	void intel_FormatAll(int index, OptionsTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, IntelFormatterFactory.create_Options());
	}

	public static Iterable<Arguments> intel_FormatAll_Data() {
		return OptionsTestsUtils.getFormatData_All("Intel", "OptionsResult");
	}

	@ParameterizedTest
	@MethodSource("intel_Format2_Data")
	void intel_Format2(int index, OptionsTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, IntelFormatterFactory.create_Options());
	}

	public static Iterable<Arguments> intel_Format2_Data() {
		return OptionsTestsUtils.getFormatData("Intel", "OptionsResult2", "Options2");
	}

	@Test
	public void intel_TestOptions() {
		testOptionsBase(new IntelFormatter().getOptions());
	}

	@ParameterizedTest
	@MethodSource("masm_FormatCommon_Data")
	void masm_FormatCommon(int index, OptionsTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, MasmFormatterFactory.create_Options());
	}

	public static Iterable<Arguments> masm_FormatCommon_Data() {
		return OptionsTestsUtils.getFormatData_Common("Masm", "OptionsResult.Common");
	}

	@ParameterizedTest
	@MethodSource("masm_FormatAll_Data")
	void masm_FormatAll(int index, OptionsTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, MasmFormatterFactory.create_Options());
	}

	public static Iterable<Arguments> masm_FormatAll_Data() {
		return OptionsTestsUtils.getFormatData_All("Masm", "OptionsResult");
	}

	@ParameterizedTest
	@MethodSource("masm_Format2_Data")
	void masm_Format2(int index, OptionsTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, MasmFormatterFactory.create_Options());
	}

	public static Iterable<Arguments> masm_Format2_Data() {
		return OptionsTestsUtils.getFormatData("Masm", "OptionsResult2", "Options2");
	}

	@Test
	public void masm_TestOptions() {
		testOptionsBase(new MasmFormatter().getOptions());
	}

	@ParameterizedTest
	@MethodSource("nasm_FormatCommon_Data")
	void nasm_FormatCommon(int index, OptionsTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, NasmFormatterFactory.create_Options());
	}

	public static Iterable<Arguments> nasm_FormatCommon_Data() {
		return OptionsTestsUtils.getFormatData_Common("Nasm", "OptionsResult.Common");
	}

	@ParameterizedTest
	@MethodSource("nasm_FormatAll_Data")
	void nasm_FormatAll(int index, OptionsTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, NasmFormatterFactory.create_Options());
	}

	public static Iterable<Arguments> nasm_FormatAll_Data() {
		return OptionsTestsUtils.getFormatData_All("Nasm", "OptionsResult");
	}

	@ParameterizedTest
	@MethodSource("nasm_Format2_Data")
	void nasm_Format2(int index, OptionsTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, NasmFormatterFactory.create_Options());
	}

	public static Iterable<Arguments> nasm_Format2_Data() {
		return OptionsTestsUtils.getFormatData("Nasm", "OptionsResult2", "Options2");
	}

	@Test
	public void nasm_TestOptions() {
		testOptionsBase(new NasmFormatter().getOptions());
	}

	private void formatTests(int index, OptionsTestCase tc, String formattedString, Formatter formatter) {
		tc.initialize(formatter.getOptions());
		FormatterTestUtils.simpleFormatTest(tc.bitness, tc.hexBytes, tc.ip, tc.code, tc.decoderOptions, formattedString, formatter,
				decoder -> tc.initialize(decoder));
	}

	private void formatTests(int index, OptionsTestCase tc, String formattedString, FastFormatter formatter) {
		tc.initialize(formatter.getOptions());
		FormatterTestUtils.simpleFormatTest(tc.bitness, tc.hexBytes, tc.ip, tc.code, tc.decoderOptions, formattedString, formatter,
				decoder -> tc.initialize(decoder));
	}

	private void testOptionsBase(FormatterOptions options) {
		{
			int min = 0x7FFF_FFFF, max = -0x8000_0000;
			for (int value : ToNumberBase.values()) {
				min = Math.min(min, value);
				max = Math.max(max, value);
				options.setNumberBase(value);
			}
			final int mmin = min, mmax = max;
			assertThrows(IllegalArgumentException.class, () -> options.setNumberBase(mmin - 1));
			assertThrows(IllegalArgumentException.class, () -> options.setNumberBase(mmax + 1));
			assertThrows(IllegalArgumentException.class, () -> options.setNumberBase(-0x8000_0000));
			assertThrows(IllegalArgumentException.class, () -> options.setNumberBase(0x7FFF_FFFF));
		}

		{
			int min = 0x7FFF_FFFF, max = -0x8000_0000;
			for (int value : ToMemorySizeOptions.values()) {
				min = Math.min(min, value);
				max = Math.max(max, value);
				options.setMemorySizeOptions(value);
			}
			final int mmin = min, mmax = max;
			assertThrows(IllegalArgumentException.class, () -> options.setMemorySizeOptions(mmin - 1));
			assertThrows(IllegalArgumentException.class, () -> options.setMemorySizeOptions(mmax + 1));
			assertThrows(IllegalArgumentException.class, () -> options.setMemorySizeOptions(-0x8000_0000));
			assertThrows(IllegalArgumentException.class, () -> options.setMemorySizeOptions(0x7FFF_FFFF));
		}
	}
}
