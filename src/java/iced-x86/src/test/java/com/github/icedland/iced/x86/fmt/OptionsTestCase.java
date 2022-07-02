// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

package com.github.icedland.iced.x86.fmt;

import com.github.icedland.iced.x86.dec.Decoder;
import com.github.icedland.iced.x86.fmt.fast.FastFormatterOptions;

public final class OptionsTestCase {
	public static class Info {
		int property;
		Object value;

		public Info(int property, Object value) {
			this.property = property;
			this.value = value;
		}
	}

	public final int bitness;
	public final String hexBytes;
	public final long ip;
	public final int code;
	public final int decoderOptions;
	final Info[] properties;

	OptionsTestCase(int bitness, String hexBytes, long ip, int code, Info[] properties) {
		this.bitness = bitness;
		this.hexBytes = hexBytes;
		this.ip = ip;
		this.code = code;
		this.properties = properties;
		this.decoderOptions = OptionsPropsUtils.getDecoderOptions(properties);
	}

	void initialize(FormatterOptions options) {
		OptionsPropsUtils.initialize(options, properties);
	}

	void initialize(FastFormatterOptions options) {
		OptionsPropsUtils.initialize(options, properties);
	}

	void initialize(Decoder decoder) {
		OptionsPropsUtils.initialize(decoder, properties);
	}
}
