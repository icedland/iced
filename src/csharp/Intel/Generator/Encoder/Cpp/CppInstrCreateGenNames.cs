// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

namespace Generator.Encoder.Cpp {
	static class CppInstrCreateGenNames {
		public const string with_branch = nameof(with_branch);
		public const string with_far_branch = nameof(with_far_branch);
		public const string with_xbegin = nameof(with_xbegin);
		public const string with_declare_byte = nameof(with_declare_byte);
		public const string with_declare_word = nameof(with_declare_word);
		public const string with_declare_dword = nameof(with_declare_dword);
		public const string with_declare_qword = nameof(with_declare_qword);
		const string _span = nameof(_span);
		public const string with_declare_byte_span = with_declare_byte + _span;
		public const string with_declare_word_span = with_declare_word + _span;
		public const string with_declare_dword_span = with_declare_dword + _span;
		public const string with_declare_qword_span = with_declare_qword + _span;

		public static string AppendArgCount(string methodName, int argCount) =>
			methodName + "_" + argCount.ToString();
	}
}
