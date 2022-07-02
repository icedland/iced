// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import org.junit.jupiter.params.*;
import org.junit.jupiter.params.provider.*;

import com.github.icedland.iced.x86.fmt.fast.FastFormatter;

final class SymbolResolverTests {
	public static final class Info {
		public Formatter formatter;
		public SymbolResolver symbolResolver;

		public Info(Formatter formatter, SymbolResolver symbolResolver) {
			this.formatter = formatter;
			this.symbolResolver = symbolResolver;
		}
	}

	public static final class FastInfo {
		FastFormatter formatter;
		SymbolResolver symbolResolver;

		public FastInfo(FastFormatter formatter, SymbolResolver symbolResolver) {
			this.formatter = formatter;
			this.symbolResolver = symbolResolver;
		}
	}

	@ParameterizedTest
	@MethodSource("fast_Format_Data")
	void fast_Format(int index, SymbolResolverTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, FastFormatterFactory.create_Resolver(new TestSymbolResolver(tc)));
	}

	public static Iterable<Arguments> fast_Format_Data() {
		return SymbolResolverTestUtils.getFormatData("Fast", "SymbolResolverTests");
	}

	@ParameterizedTest
	@MethodSource("gas_Format_Data")
	void gas_Format(int index, SymbolResolverTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, GasFormatterFactory.create_Resolver(new TestSymbolResolver(tc)));
	}

	public static Iterable<Arguments> gas_Format_Data() {
		return SymbolResolverTestUtils.getFormatData("Gas", "SymbolResolverTests");
	}

	@ParameterizedTest
	@MethodSource("intel_Format_Data")
	void intel_Format(int index, SymbolResolverTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, IntelFormatterFactory.create_Resolver(new TestSymbolResolver(tc)));
	}

	public static Iterable<Arguments> intel_Format_Data() {
		return SymbolResolverTestUtils.getFormatData("Intel", "SymbolResolverTests");
	}

	@ParameterizedTest
	@MethodSource("masm_Format_Data")
	void masm_Format(int index, SymbolResolverTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, MasmFormatterFactory.create_Resolver(new TestSymbolResolver(tc)));
	}

	public static Iterable<Arguments> masm_Format_Data() {
		return SymbolResolverTestUtils.getFormatData("Masm", "SymbolResolverTests");
	}

	@ParameterizedTest
	@MethodSource("nasm_Format_Data")
	void nasm_Format(int index, SymbolResolverTestCase tc, String formattedString) {
		formatTests(index, tc, formattedString, NasmFormatterFactory.create_Resolver(new TestSymbolResolver(tc)));
	}

	public static Iterable<Arguments> nasm_Format_Data() {
		return SymbolResolverTestUtils.getFormatData("Nasm", "SymbolResolverTests");
	}

	private void formatTests(int index, SymbolResolverTestCase tc, String formattedString, Info formatterInfo) {
		Formatter formatter = formatterInfo.formatter;
		int decoderOptions = OptionsPropsUtils.getDecoderOptions(tc.options);
		OptionsPropsUtils.initialize(formatter.getOptions(), tc.options);
		FormatterTestUtils.simpleFormatTest(tc.bitness, tc.hexBytes, tc.ip, tc.code, decoderOptions, formattedString, formatter,
				decoder -> OptionsPropsUtils.initialize(decoder, tc.options));
	}

	private void formatTests(int index, SymbolResolverTestCase tc, String formattedString, FastInfo formatterInfo) {
		FastFormatter formatter = formatterInfo.formatter;
		int decoderOptions = OptionsPropsUtils.getDecoderOptions(tc.options);
		OptionsPropsUtils.initialize(formatter.getOptions(), tc.options);
		FormatterTestUtils.simpleFormatTest(tc.bitness, tc.hexBytes, tc.ip, tc.code, decoderOptions, formattedString, formatter,
				decoder -> OptionsPropsUtils.initialize(decoder, tc.options));
	}
}
