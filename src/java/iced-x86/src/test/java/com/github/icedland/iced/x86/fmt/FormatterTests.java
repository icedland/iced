// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;

import com.github.icedland.iced.x86.Instruction;
import com.github.icedland.iced.x86.dec.NonDecodedInstructions;
import com.github.icedland.iced.x86.fmt.fast.FastFormatter;

final class FormatterTests {
	@ParameterizedTest
	@MethodSource("fast_Format_Data_Default_16")
	void fast_Format_Default_16(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, FastFormatterFactory.create_Default());
	}

	public static Iterable<Arguments> fast_Format_Data_Default_16() {
		return FormatterTestCases.getFormatData(16, "Fast", "Default");
	}

	@ParameterizedTest
	@MethodSource("fast_Format_Data_Inverted_16")
	void fast_Format_Inverted_16(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, FastFormatterFactory.create_Inverted());
	}

	public static Iterable<Arguments> fast_Format_Data_Inverted_16() {
		return FormatterTestCases.getFormatData(16, "Fast", "Inverted");
	}

	@ParameterizedTest
	@MethodSource("fast_Format_Data_NonDec_Default_16")
	void fast_Format_NonDec_Default_16(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, FastFormatterFactory.create_Default());
	}

	public static Iterable<Arguments> fast_Format_Data_NonDec_Default_16() {
		return FormatterTestCases.getFormatData(16, NonDecodedInstructions.infos16, "Fast", "NonDec_Default");
	}

	@ParameterizedTest
	@MethodSource("fast_Format_Data_NonDec_Inverted_16")
	void fast_Format_NonDec_Inverted_16(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, FastFormatterFactory.create_Inverted());
	}

	public static Iterable<Arguments> fast_Format_Data_NonDec_Inverted_16() {
		return FormatterTestCases.getFormatData(16, NonDecodedInstructions.infos16, "Fast", "NonDec_Inverted");
	}

	@ParameterizedTest
	@MethodSource("fast_Format_Data_Misc_16")
	void fast_Format_Misc_16(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, FastFormatterFactory.create_Default());
	}

	public static Iterable<Arguments> fast_Format_Data_Misc_16() {
		return FormatterTestCases.getFormatData(16, "Fast", "Misc", true);
	}

	@ParameterizedTest
	@MethodSource("fast_Format_Data_Default_32")
	void fast_Format_Default_32(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, FastFormatterFactory.create_Default());
	}

	public static Iterable<Arguments> fast_Format_Data_Default_32() {
		return FormatterTestCases.getFormatData(32, "Fast", "Default");
	}

	@ParameterizedTest
	@MethodSource("fast_Format_Data_Inverted_32")
	void fast_Format_Inverted_32(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, FastFormatterFactory.create_Inverted());
	}

	public static Iterable<Arguments> fast_Format_Data_Inverted_32() {
		return FormatterTestCases.getFormatData(32, "Fast", "Inverted");
	}

	@ParameterizedTest
	@MethodSource("fast_Format_Data_NonDec_Default_32")
	void fast_Format_NonDec_Default_32(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, FastFormatterFactory.create_Default());
	}

	public static Iterable<Arguments> fast_Format_Data_NonDec_Default_32() {
		return FormatterTestCases.getFormatData(32, NonDecodedInstructions.infos32, "Fast", "NonDec_Default");
	}

	@ParameterizedTest
	@MethodSource("fast_Format_Data_NonDec_Inverted_32")
	void fast_Format_NonDec_Inverted_32(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, FastFormatterFactory.create_Inverted());
	}

	public static Iterable<Arguments> fast_Format_Data_NonDec_Inverted_32() {
		return FormatterTestCases.getFormatData(32, NonDecodedInstructions.infos32, "Fast", "NonDec_Inverted");
	}

	@ParameterizedTest
	@MethodSource("fast_Format_Data_Misc_32")
	void fast_Format_Misc_32(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, FastFormatterFactory.create_Default());
	}

	public static Iterable<Arguments> fast_Format_Data_Misc_32() {
		return FormatterTestCases.getFormatData(32, "Fast", "Misc", true);
	}

	@ParameterizedTest
	@MethodSource("fast_Format_Data_Default_64")
	void fast_Format_Default_64(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, FastFormatterFactory.create_Default());
	}

	public static Iterable<Arguments> fast_Format_Data_Default_64() {
		return FormatterTestCases.getFormatData(64, "Fast", "Default");
	}

	@ParameterizedTest
	@MethodSource("fast_Format_Data_Inverted_64")
	void fast_Format_Inverted_64(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, FastFormatterFactory.create_Inverted());
	}

	public static Iterable<Arguments> fast_Format_Data_Inverted_64() {
		return FormatterTestCases.getFormatData(64, "Fast", "Inverted");
	}

	@ParameterizedTest
	@MethodSource("fast_Format_Data_NonDec_Default_64")
	void fast_Format_NonDec_Default_64(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, FastFormatterFactory.create_Default());
	}

	public static Iterable<Arguments> fast_Format_Data_NonDec_Default_64() {
		return FormatterTestCases.getFormatData(64, NonDecodedInstructions.infos64, "Fast", "NonDec_Default");
	}

	@ParameterizedTest
	@MethodSource("fast_Format_Data_NonDec_Inverted_64")
	void fast_Format_NonDec_Inverted_64(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, FastFormatterFactory.create_Inverted());
	}

	public static Iterable<Arguments> fast_Format_Data_NonDec_Inverted_64() {
		return FormatterTestCases.getFormatData(64, NonDecodedInstructions.infos64, "Fast", "NonDec_Inverted");
	}

	@ParameterizedTest
	@MethodSource("fast_Format_Data_Misc_64")
	void fast_Format_Misc_64(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, FastFormatterFactory.create_Default());
	}

	public static Iterable<Arguments> fast_Format_Data_Misc_64() {
		return FormatterTestCases.getFormatData(64, "Fast", "Misc", true);
	}

	@ParameterizedTest
	@MethodSource("gas_Format_Data_ForceSuffix_16")
	void gas_Format_ForceSuffix_16(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, GasFormatterFactory.create_ForceSuffix());
	}

	public static Iterable<Arguments> gas_Format_Data_ForceSuffix_16() {
		return FormatterTestCases.getFormatData(16, "Gas", "ForceSuffix");
	}

	@ParameterizedTest
	@MethodSource("gas_Format_Data_NoSuffix_16")
	void gas_Format_NoSuffix_16(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, GasFormatterFactory.create_NoSuffix());
	}

	public static Iterable<Arguments> gas_Format_Data_NoSuffix_16() {
		return FormatterTestCases.getFormatData(16, "Gas", "NoSuffix");
	}

	@ParameterizedTest
	@MethodSource("gas_Format_Data_NonDec_ForceSuffix_16")
	void gas_Format_NonDec_ForceSuffix_16(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, GasFormatterFactory.create_ForceSuffix());
	}

	public static Iterable<Arguments> gas_Format_Data_NonDec_ForceSuffix_16() {
		return FormatterTestCases.getFormatData(16, NonDecodedInstructions.infos16, "Gas", "NonDec_ForceSuffix");
	}

	@ParameterizedTest
	@MethodSource("gas_Format_Data_NonDec_NoSuffix_16")
	void gas_Format_NonDec_NoSuffix_16(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, GasFormatterFactory.create_NoSuffix());
	}

	public static Iterable<Arguments> gas_Format_Data_NonDec_NoSuffix_16() {
		return FormatterTestCases.getFormatData(16, NonDecodedInstructions.infos16, "Gas", "NonDec_NoSuffix");
	}

	@ParameterizedTest
	@MethodSource("gas_Format_Data_Misc_16")
	void gas_Format_Misc_16(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, GasFormatterFactory.create());
	}

	public static Iterable<Arguments> gas_Format_Data_Misc_16() {
		return FormatterTestCases.getFormatData(16, "Gas", "Misc", true);
	}

	@ParameterizedTest
	@MethodSource("gas_Format_Data_ForceSuffix_32")
	void gas_Format_ForceSuffix_32(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, GasFormatterFactory.create_ForceSuffix());
	}

	public static Iterable<Arguments> gas_Format_Data_ForceSuffix_32() {
		return FormatterTestCases.getFormatData(32, "Gas", "ForceSuffix");
	}

	@ParameterizedTest
	@MethodSource("gas_Format_Data_NoSuffix_32")
	void gas_Format_NoSuffix_32(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, GasFormatterFactory.create_NoSuffix());
	}

	public static Iterable<Arguments> gas_Format_Data_NoSuffix_32() {
		return FormatterTestCases.getFormatData(32, "Gas", "NoSuffix");
	}

	@ParameterizedTest
	@MethodSource("gas_Format_Data_NonDec_ForceSuffix_32")
	void gas_Format_NonDec_ForceSuffix_32(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, GasFormatterFactory.create_ForceSuffix());
	}

	public static Iterable<Arguments> gas_Format_Data_NonDec_ForceSuffix_32() {
		return FormatterTestCases.getFormatData(32, NonDecodedInstructions.infos32, "Gas", "NonDec_ForceSuffix");
	}

	@ParameterizedTest
	@MethodSource("gas_Format_Data_NonDec_NoSuffix_32")
	void gas_Format_NonDec_NoSuffix_32(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, GasFormatterFactory.create_NoSuffix());
	}

	public static Iterable<Arguments> gas_Format_Data_NonDec_NoSuffix_32() {
		return FormatterTestCases.getFormatData(32, NonDecodedInstructions.infos32, "Gas", "NonDec_NoSuffix");
	}

	@ParameterizedTest
	@MethodSource("gas_Format_Data_Misc_32")
	void gas_Format_Misc_32(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, GasFormatterFactory.create());
	}

	public static Iterable<Arguments> gas_Format_Data_Misc_32() {
		return FormatterTestCases.getFormatData(32, "Gas", "Misc", true);
	}

	@ParameterizedTest
	@MethodSource("gas_Format_Data_ForceSuffix_64")
	void gas_Format_ForceSuffix_64(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, GasFormatterFactory.create_ForceSuffix());
	}

	public static Iterable<Arguments> gas_Format_Data_ForceSuffix_64() {
		return FormatterTestCases.getFormatData(64, "Gas", "ForceSuffix");
	}

	@ParameterizedTest
	@MethodSource("gas_Format_Data_NoSuffix_64")
	void gas_Format_NoSuffix_64(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, GasFormatterFactory.create_NoSuffix());
	}

	public static Iterable<Arguments> gas_Format_Data_NoSuffix_64() {
		return FormatterTestCases.getFormatData(64, "Gas", "NoSuffix");
	}

	@ParameterizedTest
	@MethodSource("gas_Format_Data_NonDec_ForceSuffix_64")
	void gas_Format_NonDec_ForceSuffix_64(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, GasFormatterFactory.create_ForceSuffix());
	}

	public static Iterable<Arguments> gas_Format_Data_NonDec_ForceSuffix_64() {
		return FormatterTestCases.getFormatData(64, NonDecodedInstructions.infos64, "Gas", "NonDec_ForceSuffix");
	}

	@ParameterizedTest
	@MethodSource("gas_Format_Data_NonDec_NoSuffix_64")
	void gas_Format_NonDec_NoSuffix_64(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, GasFormatterFactory.create_NoSuffix());
	}

	public static Iterable<Arguments> gas_Format_Data_NonDec_NoSuffix_64() {
		return FormatterTestCases.getFormatData(64, NonDecodedInstructions.infos64, "Gas", "NonDec_NoSuffix");
	}

	@ParameterizedTest
	@MethodSource("gas_Format_Data_Misc_64")
	void gas_Format_Misc_64(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, GasFormatterFactory.create());
	}

	public static Iterable<Arguments> gas_Format_Data_Misc_64() {
		return FormatterTestCases.getFormatData(64, "Gas", "Misc", true);
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data_MemAlways_16")
	void intel_Format_MemAlways_16(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, IntelFormatterFactory.create_MemAlways());
	}

	public static Iterable<Arguments> intel_Format_Data_MemAlways_16() {
		return FormatterTestCases.getFormatData(16, "Intel", "MemAlways");
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data_MemDefault_16")
	void intel_Format_MemDefault_16(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, IntelFormatterFactory.create_MemDefault());
	}

	public static Iterable<Arguments> intel_Format_Data_MemDefault_16() {
		return FormatterTestCases.getFormatData(16, "Intel", "MemDefault");
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data_MemMinimum_16")
	void intel_Format_MemMinimum_16(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, IntelFormatterFactory.create_MemMinimum());
	}

	public static Iterable<Arguments> intel_Format_Data_MemMinimum_16() {
		return FormatterTestCases.getFormatData(16, "Intel", "MemMinimum");
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data_NonDec_MemAlways_16")
	void intel_Format_NonDec_MemAlways_16(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, IntelFormatterFactory.create_MemAlways());
	}

	public static Iterable<Arguments> intel_Format_Data_NonDec_MemAlways_16() {
		return FormatterTestCases.getFormatData(16, NonDecodedInstructions.infos16, "Intel", "NonDec_MemAlways");
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data_NonDec_MemDefault_16")
	void intel_Format_NonDec_MemDefault_16(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, IntelFormatterFactory.create_MemDefault());
	}

	public static Iterable<Arguments> intel_Format_Data_NonDec_MemDefault_16() {
		return FormatterTestCases.getFormatData(16, NonDecodedInstructions.infos16, "Intel", "NonDec_MemDefault");
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data_NonDec_MemMinimum_16")
	void intel_Format_NonDec_MemMinimum_16(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, IntelFormatterFactory.create_MemMinimum());
	}

	public static Iterable<Arguments> intel_Format_Data_NonDec_MemMinimum_16() {
		return FormatterTestCases.getFormatData(16, NonDecodedInstructions.infos16, "Intel", "NonDec_MemMinimum");
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data_Misc_16")
	void intel_Format_Misc_16(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, IntelFormatterFactory.create());
	}

	public static Iterable<Arguments> intel_Format_Data_Misc_16() {
		return FormatterTestCases.getFormatData(16, "Intel", "Misc", true);
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data_MemAlways_32")
	void intel_Format_MemAlways_32(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, IntelFormatterFactory.create_MemAlways());
	}

	public static Iterable<Arguments> intel_Format_Data_MemAlways_32() {
		return FormatterTestCases.getFormatData(32, "Intel", "MemAlways");
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data_MemDefault_32")
	void intel_Format_MemDefault_32(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, IntelFormatterFactory.create_MemDefault());
	}

	public static Iterable<Arguments> intel_Format_Data_MemDefault_32() {
		return FormatterTestCases.getFormatData(32, "Intel", "MemDefault");
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data_MemMinimum_32")
	void intel_Format_MemMinimum_32(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, IntelFormatterFactory.create_MemMinimum());
	}

	public static Iterable<Arguments> intel_Format_Data_MemMinimum_32() {
		return FormatterTestCases.getFormatData(32, "Intel", "MemMinimum");
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data_NonDec_MemAlways_32")
	void intel_Format_NonDec_MemAlways_32(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, IntelFormatterFactory.create_MemAlways());
	}

	public static Iterable<Arguments> intel_Format_Data_NonDec_MemAlways_32() {
		return FormatterTestCases.getFormatData(32, NonDecodedInstructions.infos32, "Intel", "NonDec_MemAlways");
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data_NonDec_MemDefault_32")
	void intel_Format_NonDec_MemDefault_32(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, IntelFormatterFactory.create_MemDefault());
	}

	public static Iterable<Arguments> intel_Format_Data_NonDec_MemDefault_32() {
		return FormatterTestCases.getFormatData(32, NonDecodedInstructions.infos32, "Intel", "NonDec_MemDefault");
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data_NonDec_MemMinimum_32")
	void intel_Format_NonDec_MemMinimum_32(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, IntelFormatterFactory.create_MemMinimum());
	}

	public static Iterable<Arguments> intel_Format_Data_NonDec_MemMinimum_32() {
		return FormatterTestCases.getFormatData(32, NonDecodedInstructions.infos32, "Intel", "NonDec_MemMinimum");
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data_Misc_32")
	void intel_Format_Misc_32(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, IntelFormatterFactory.create());
	}

	public static Iterable<Arguments> intel_Format_Data_Misc_32() {
		return FormatterTestCases.getFormatData(32, "Intel", "Misc", true);
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data_MemAlways_64")
	void intel_Format_MemAlways_64(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, IntelFormatterFactory.create_MemAlways());
	}

	public static Iterable<Arguments> intel_Format_Data_MemAlways_64() {
		return FormatterTestCases.getFormatData(64, "Intel", "MemAlways");
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data_MemDefault_64")
	void intel_Format_MemDefault_64(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, IntelFormatterFactory.create_MemDefault());
	}

	public static Iterable<Arguments> intel_Format_Data_MemDefault_64() {
		return FormatterTestCases.getFormatData(64, "Intel", "MemDefault");
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data_MemMinimum_64")
	void intel_Format_MemMinimum_64(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, IntelFormatterFactory.create_MemMinimum());
	}

	public static Iterable<Arguments> intel_Format_Data_MemMinimum_64() {
		return FormatterTestCases.getFormatData(64, "Intel", "MemMinimum");
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data_NonDec_MemAlways_64")
	void intel_Format_NonDec_MemAlways_64(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, IntelFormatterFactory.create_MemAlways());
	}

	public static Iterable<Arguments> intel_Format_Data_NonDec_MemAlways_64() {
		return FormatterTestCases.getFormatData(64, NonDecodedInstructions.infos64, "Intel", "NonDec_MemAlways");
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data_NonDec_MemDefault_64")
	void intel_Format_NonDec_MemDefault_64(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, IntelFormatterFactory.create_MemDefault());
	}

	public static Iterable<Arguments> intel_Format_Data_NonDec_MemDefault_64() {
		return FormatterTestCases.getFormatData(64, NonDecodedInstructions.infos64, "Intel", "NonDec_MemDefault");
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data_NonDec_MemMinimum_64")
	void intel_Format_NonDec_MemMinimum_64(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, IntelFormatterFactory.create_MemMinimum());
	}

	public static Iterable<Arguments> intel_Format_Data_NonDec_MemMinimum_64() {
		return FormatterTestCases.getFormatData(64, NonDecodedInstructions.infos64, "Intel", "NonDec_MemMinimum");
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data_Misc_64")
	void intel_Format_Misc_64(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, IntelFormatterFactory.create());
	}

	public static Iterable<Arguments> intel_Format_Data_Misc_64() {
		return FormatterTestCases.getFormatData(64, "Intel", "Misc", true);
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data_MemAlways_16")
	void masm_Format_MemAlways_16(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, MasmFormatterFactory.create_MemAlways());
	}

	public static Iterable<Arguments> masm_Format_Data_MemAlways_16() {
		return FormatterTestCases.getFormatData(16, "Masm", "MemAlways");
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data_MemDefault_16")
	void masm_Format_MemDefault_16(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, MasmFormatterFactory.create_MemDefault());
	}

	public static Iterable<Arguments> masm_Format_Data_MemDefault_16() {
		return FormatterTestCases.getFormatData(16, "Masm", "MemDefault");
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data_MemMinimum_16")
	void masm_Format_MemMinimum_16(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, MasmFormatterFactory.create_MemMinimum());
	}

	public static Iterable<Arguments> masm_Format_Data_MemMinimum_16() {
		return FormatterTestCases.getFormatData(16, "Masm", "MemMinimum");
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data_NonDec_MemAlways_16")
	void masm_Format_NonDec_MemAlways_16(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, MasmFormatterFactory.create_MemAlways());
	}

	public static Iterable<Arguments> masm_Format_Data_NonDec_MemAlways_16() {
		return FormatterTestCases.getFormatData(16, NonDecodedInstructions.infos16, "Masm", "NonDec_MemAlways");
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data_NonDec_MemDefault_16")
	void masm_Format_NonDec_MemDefault_16(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, MasmFormatterFactory.create_MemDefault());
	}

	public static Iterable<Arguments> masm_Format_Data_NonDec_MemDefault_16() {
		return FormatterTestCases.getFormatData(16, NonDecodedInstructions.infos16, "Masm", "NonDec_MemDefault");
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data_NonDec_MemMinimum_16")
	void masm_Format_NonDec_MemMinimum_16(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, MasmFormatterFactory.create_MemMinimum());
	}

	public static Iterable<Arguments> masm_Format_Data_NonDec_MemMinimum_16() {
		return FormatterTestCases.getFormatData(16, NonDecodedInstructions.infos16, "Masm", "NonDec_MemMinimum");
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data_Misc_16")
	void masm_Format_Misc_16(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, MasmFormatterFactory.create());
	}

	public static Iterable<Arguments> masm_Format_Data_Misc_16() {
		return FormatterTestCases.getFormatData(16, "Masm", "Misc", true);
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data_MemAlways_32")
	void masm_Format_MemAlways_32(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, MasmFormatterFactory.create_MemAlways());
	}

	public static Iterable<Arguments> masm_Format_Data_MemAlways_32() {
		return FormatterTestCases.getFormatData(32, "Masm", "MemAlways");
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data_MemDefault_32")
	void masm_Format_MemDefault_32(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, MasmFormatterFactory.create_MemDefault());
	}

	public static Iterable<Arguments> masm_Format_Data_MemDefault_32() {
		return FormatterTestCases.getFormatData(32, "Masm", "MemDefault");
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data_MemMinimum_32")
	void masm_Format_MemMinimum_32(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, MasmFormatterFactory.create_MemMinimum());
	}

	public static Iterable<Arguments> masm_Format_Data_MemMinimum_32() {
		return FormatterTestCases.getFormatData(32, "Masm", "MemMinimum");
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data_NonDec_MemAlways_32")
	void masm_Format_NonDec_MemAlways_32(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, MasmFormatterFactory.create_MemAlways());
	}

	public static Iterable<Arguments> masm_Format_Data_NonDec_MemAlways_32() {
		return FormatterTestCases.getFormatData(32, NonDecodedInstructions.infos32, "Masm", "NonDec_MemAlways");
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data_NonDec_MemDefault_32")
	void masm_Format_NonDec_MemDefault_32(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, MasmFormatterFactory.create_MemDefault());
	}

	public static Iterable<Arguments> masm_Format_Data_NonDec_MemDefault_32() {
		return FormatterTestCases.getFormatData(32, NonDecodedInstructions.infos32, "Masm", "NonDec_MemDefault");
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data_NonDec_MemMinimum_32")
	void masm_Format_NonDec_MemMinimum_32(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, MasmFormatterFactory.create_MemMinimum());
	}

	public static Iterable<Arguments> masm_Format_Data_NonDec_MemMinimum_32() {
		return FormatterTestCases.getFormatData(32, NonDecodedInstructions.infos32, "Masm", "NonDec_MemMinimum");
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data_Misc_32")
	void masm_Format_Misc_32(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, MasmFormatterFactory.create());
	}

	public static Iterable<Arguments> masm_Format_Data_Misc_32() {
		return FormatterTestCases.getFormatData(32, "Masm", "Misc", true);
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data_MemAlways_64")
	void masm_Format_MemAlways_64(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, MasmFormatterFactory.create_MemAlways());
	}

	public static Iterable<Arguments> masm_Format_Data_MemAlways_64() {
		return FormatterTestCases.getFormatData(64, "Masm", "MemAlways");
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data_MemDefault_64")
	void masm_Format_MemDefault_64(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, MasmFormatterFactory.create_MemDefault());
	}

	public static Iterable<Arguments> masm_Format_Data_MemDefault_64() {
		return FormatterTestCases.getFormatData(64, "Masm", "MemDefault");
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data_MemMinimum_64")
	void masm_Format_MemMinimum_64(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, MasmFormatterFactory.create_MemMinimum());
	}

	public static Iterable<Arguments> masm_Format_Data_MemMinimum_64() {
		return FormatterTestCases.getFormatData(64, "Masm", "MemMinimum");
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data_NonDec_MemAlways_64")
	void masm_Format_NonDec_MemAlways_64(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, MasmFormatterFactory.create_MemAlways());
	}

	public static Iterable<Arguments> masm_Format_Data_NonDec_MemAlways_64() {
		return FormatterTestCases.getFormatData(64, NonDecodedInstructions.infos64, "Masm", "NonDec_MemAlways");
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data_NonDec_MemDefault_64")
	void masm_Format_NonDec_MemDefault_64(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, MasmFormatterFactory.create_MemDefault());
	}

	public static Iterable<Arguments> masm_Format_Data_NonDec_MemDefault_64() {
		return FormatterTestCases.getFormatData(64, NonDecodedInstructions.infos64, "Masm", "NonDec_MemDefault");
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data_NonDec_MemMinimum_64")
	void masm_Format_NonDec_MemMinimum_64(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, MasmFormatterFactory.create_MemMinimum());
	}

	public static Iterable<Arguments> masm_Format_Data_NonDec_MemMinimum_64() {
		return FormatterTestCases.getFormatData(64, NonDecodedInstructions.infos64, "Masm", "NonDec_MemMinimum");
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data_Misc_64")
	void masm_Format_Misc_64(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, MasmFormatterFactory.create());
	}

	public static Iterable<Arguments> masm_Format_Data_Misc_64() {
		return FormatterTestCases.getFormatData(64, "Masm", "Misc", true);
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data_MemAlways_16")
	void nasm_Format_MemAlways_16(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, NasmFormatterFactory.create_MemAlways());
	}

	public static Iterable<Arguments> nasm_Format_Data_MemAlways_16() {
		return FormatterTestCases.getFormatData(16, "Nasm", "MemAlways");
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data_MemDefault_16")
	void nasm_Format_MemDefault_16(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, NasmFormatterFactory.create_MemDefault());
	}

	public static Iterable<Arguments> nasm_Format_Data_MemDefault_16() {
		return FormatterTestCases.getFormatData(16, "Nasm", "MemDefault");
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data_MemMinimum_16")
	void nasm_Format_MemMinimum_16(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, NasmFormatterFactory.create_MemMinimum());
	}

	public static Iterable<Arguments> nasm_Format_Data_MemMinimum_16() {
		return FormatterTestCases.getFormatData(16, "Nasm", "MemMinimum");
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data_NonDec_MemAlways_16")
	void nasm_Format_NonDec_MemAlways_16(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, NasmFormatterFactory.create_MemAlways());
	}

	public static Iterable<Arguments> nasm_Format_Data_NonDec_MemAlways_16() {
		return FormatterTestCases.getFormatData(16, NonDecodedInstructions.infos16, "Nasm", "NonDec_MemAlways");
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data_NonDec_MemDefault_16")
	void nasm_Format_NonDec_MemDefault_16(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, NasmFormatterFactory.create_MemDefault());
	}

	public static Iterable<Arguments> nasm_Format_Data_NonDec_MemDefault_16() {
		return FormatterTestCases.getFormatData(16, NonDecodedInstructions.infos16, "Nasm", "NonDec_MemDefault");
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data_NonDec_MemMinimum_16")
	void nasm_Format_NonDec_MemMinimum_16(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, NasmFormatterFactory.create_MemMinimum());
	}

	public static Iterable<Arguments> nasm_Format_Data_NonDec_MemMinimum_16() {
		return FormatterTestCases.getFormatData(16, NonDecodedInstructions.infos16, "Nasm", "NonDec_MemMinimum");
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data_Misc_16")
	void nasm_Format_Misc_16(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, NasmFormatterFactory.create());
	}

	public static Iterable<Arguments> nasm_Format_Data_Misc_16() {
		return FormatterTestCases.getFormatData(16, "Nasm", "Misc", true);
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data_MemAlways_32")
	void nasm_Format_MemAlways_32(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, NasmFormatterFactory.create_MemAlways());
	}

	public static Iterable<Arguments> nasm_Format_Data_MemAlways_32() {
		return FormatterTestCases.getFormatData(32, "Nasm", "MemAlways");
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data_MemDefault_32")
	void nasm_Format_MemDefault_32(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, NasmFormatterFactory.create_MemDefault());
	}

	public static Iterable<Arguments> nasm_Format_Data_MemDefault_32() {
		return FormatterTestCases.getFormatData(32, "Nasm", "MemDefault");
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data_MemMinimum_32")
	void nasm_Format_MemMinimum_32(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, NasmFormatterFactory.create_MemMinimum());
	}

	public static Iterable<Arguments> nasm_Format_Data_MemMinimum_32() {
		return FormatterTestCases.getFormatData(32, "Nasm", "MemMinimum");
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data_NonDec_MemAlways_32")
	void nasm_Format_NonDec_MemAlways_32(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, NasmFormatterFactory.create_MemAlways());
	}

	public static Iterable<Arguments> nasm_Format_Data_NonDec_MemAlways_32() {
		return FormatterTestCases.getFormatData(32, NonDecodedInstructions.infos32, "Nasm", "NonDec_MemAlways");
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data_NonDec_MemDefault_32")
	void nasm_Format_NonDec_MemDefault_32(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, NasmFormatterFactory.create_MemDefault());
	}

	public static Iterable<Arguments> nasm_Format_Data_NonDec_MemDefault_32() {
		return FormatterTestCases.getFormatData(32, NonDecodedInstructions.infos32, "Nasm", "NonDec_MemDefault");
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data_NonDec_MemMinimum_32")
	void nasm_Format_NonDec_MemMinimum_32(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, NasmFormatterFactory.create_MemMinimum());
	}

	public static Iterable<Arguments> nasm_Format_Data_NonDec_MemMinimum_32() {
		return FormatterTestCases.getFormatData(32, NonDecodedInstructions.infos32, "Nasm", "NonDec_MemMinimum");
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data_Misc_32")
	void nasm_Format_Misc_32(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, NasmFormatterFactory.create());
	}

	public static Iterable<Arguments> nasm_Format_Data_Misc_32() {
		return FormatterTestCases.getFormatData(32, "Nasm", "Misc", true);
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data_MemAlways_64")
	void nasm_Format_MemAlways_64(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, NasmFormatterFactory.create_MemAlways());
	}

	public static Iterable<Arguments> nasm_Format_Data_MemAlways_64() {
		return FormatterTestCases.getFormatData(64, "Nasm", "MemAlways");
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data_MemDefault_64")
	void nasm_Format_MemDefault_64(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, NasmFormatterFactory.create_MemDefault());
	}

	public static Iterable<Arguments> nasm_Format_Data_MemDefault_64() {
		return FormatterTestCases.getFormatData(64, "Nasm", "MemDefault");
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data_MemMinimum_64")
	void nasm_Format_MemMinimum_64(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, NasmFormatterFactory.create_MemMinimum());
	}

	public static Iterable<Arguments> nasm_Format_Data_MemMinimum_64() {
		return FormatterTestCases.getFormatData(64, "Nasm", "MemMinimum");
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data_NonDec_MemAlways_64")
	void nasm_Format_NonDec_MemAlways_64(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, NasmFormatterFactory.create_MemAlways());
	}

	public static Iterable<Arguments> nasm_Format_Data_NonDec_MemAlways_64() {
		return FormatterTestCases.getFormatData(64, NonDecodedInstructions.infos64, "Nasm", "NonDec_MemAlways");
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data_NonDec_MemDefault_64")
	void nasm_Format_NonDec_MemDefault_64(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, NasmFormatterFactory.create_MemDefault());
	}

	public static Iterable<Arguments> nasm_Format_Data_NonDec_MemDefault_64() {
		return FormatterTestCases.getFormatData(64, NonDecodedInstructions.infos64, "Nasm", "NonDec_MemDefault");
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data_NonDec_MemMinimum_64")
	void nasm_Format_NonDec_MemMinimum_64(int index, Instruction instr, String formattedString) {
		formatTests(index, instr, formattedString, NasmFormatterFactory.create_MemMinimum());
	}

	public static Iterable<Arguments> nasm_Format_Data_NonDec_MemMinimum_64() {
		return FormatterTestCases.getFormatData(64, NonDecodedInstructions.infos64, "Nasm", "NonDec_MemMinimum");
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data_Misc_64")
	void nasm_Format_Misc_64(int index, FormatterTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, NasmFormatterFactory.create());
	}

	public static Iterable<Arguments> nasm_Format_Data_Misc_64() {
		return FormatterTestCases.getFormatData(64, "Nasm", "Misc", true);
	}

	private void formatTests(int index, FormatterTestCase tc, String formattedString, FastFormatter formatter) {
		FormatterTestUtils.formatTest(tc.bitness, tc.hexBytes, tc.ip, tc.code, tc.options, formattedString, formatter);
	}

	private void formatTests(int index, Instruction instruction, String formattedString, FastFormatter formatter) {
		FormatterTestUtils.formatTest(instruction, formattedString, formatter);
	}

	private void formatTests(int index, FormatterTestCase tc, String formattedString, Formatter formatter) {
		FormatterTestUtils.formatTest(tc.bitness, tc.hexBytes, tc.ip, tc.code, tc.options, formattedString, formatter);
	}

	private void formatTests(int index, Instruction instruction, String formattedString, Formatter formatter) {
		FormatterTestUtils.formatTest(instruction, formattedString, formatter);
	}
}
