// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;

namespace Generator.Documentation.Lua {
	sealed class LuaDocCommentWriter : MarkdownDocCommentWriter {
		static readonly Dictionary<string, (string type, bool isKeyword)> toTypeInfo = new(StringComparer.Ordinal) {
			{ "bcd", ("bcd", false) },
			{ "bf16", ("bfloat16", false) },
			{ "f16", ("f16", false) },
			{ "f32", ("f32", false) },
			{ "f64", ("f64", false) },
			{ "f80", ("f80", false) },
			{ "f128", ("f128", false) },
			{ "i8", ("i8", false) },
			{ "i16", ("i16", false) },
			{ "i32", ("i32", false) },
			{ "i64", ("i64", false) },
			{ "i128", ("i128", false) },
			{ "i256", ("i256", false) },
			{ "i512", ("i512", false) },
			{ "u8", ("u8", false) },
			{ "u16", ("u16", false) },
			{ "u32", ("u32", false) },
			{ "u52", ("u52", false) },
			{ "u64", ("u64", false) },
			{ "u128", ("u128", false) },
			{ "u256", ("u256", false) },
			{ "u512", ("u512", false) },
		};

		public LuaDocCommentWriter(IdentifierConverter idConverter, TargetLanguage language = TargetLanguage.Lua)
			: base(idConverter, ".", ".", ":", ":",
					language == TargetLanguage.Lua ? "---" : "///",
					language == TargetLanguage.Lua ? "---" : "/// ",
					false, toTypeInfo) {}
	}
}

