// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;

namespace Generator.Documentation.Rust {
	sealed class RustDocCommentWriter : MarkdownDocCommentWriter {
		static readonly Dictionary<string, (string type, bool isKeyword)> toTypeInfo = new(StringComparer.Ordinal) {
			{ "bcd", ("bcd", false) },
			{ "bf16", ("bfloat16", false) },
			{ "f16", ("f16", false) },
			{ "f32", ("f32", true) },
			{ "f64", ("f64", true) },
			{ "f80", ("f80", false) },
			{ "f128", ("f128", false) },
			{ "i8", ("i8", true) },
			{ "i16", ("i16", true) },
			{ "i32", ("i32", true) },
			{ "i64", ("i64", true) },
			{ "i128", ("i128", true) },
			{ "i256", ("i256", false) },
			{ "i512", ("i512", false) },
			{ "u8", ("u8", true) },
			{ "u16", ("u16", true) },
			{ "u32", ("u32", true) },
			{ "u52", ("u52", false) },
			{ "u64", ("u64", true) },
			{ "u128", ("u128", true) },
			{ "u256", ("u256", false) },
			{ "u512", ("u512", false) },
		};

		public RustDocCommentWriter(IdentifierConverter idConverter, string enumSeparator = "::", string fieldSeparator = "::", string propertySeparator = "::", string methodSeparator = "::")
			: base(idConverter, enumSeparator, fieldSeparator, propertySeparator, methodSeparator, "///", "/// ", true, toTypeInfo) {}
	}
}
