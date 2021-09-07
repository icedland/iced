// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Encoder.Rust {
	static class RustInstrCreateGenNames {
		public const string with_branch = nameof(with_branch);
		public const string with_far_branch = nameof(with_far_branch);
		public const string with_xbegin = nameof(with_xbegin);
		public const string with_declare_byte = nameof(with_declare_byte);
		public const string with_declare_word = nameof(with_declare_word);
		public const string with_declare_dword = nameof(with_declare_dword);
		public const string with_declare_qword = nameof(with_declare_qword);
		const string _slice_u8 = nameof(_slice_u8);
		public const string with_declare_word_slice_u8 = with_declare_word + _slice_u8;
		public const string with_declare_dword_slice_u8 = with_declare_dword + _slice_u8;
		public const string with_declare_qword_slice_u8 = with_declare_qword + _slice_u8;

		public static string AppendArgCount(string methodName, int argCount) =>
			methodName + "_" + argCount.ToString();
	}
}
