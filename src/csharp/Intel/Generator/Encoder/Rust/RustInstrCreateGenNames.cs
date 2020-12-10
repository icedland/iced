/*
Copyright (C) 2018-2019 de4dot@gmail.com

Permission is hereby granted, free of charge, to any person obtaining
a copy of this software and associated documentation files (the
"Software"), to deal in the Software without restriction, including
without limitation the rights to use, copy, modify, merge, publish,
distribute, sublicense, and/or sell copies of the Software, and to
permit persons to whom the Software is furnished to do so, subject to
the following conditions:

The above copyright notice and this permission notice shall be
included in all copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY
CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE
SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
*/

namespace Generator.Encoder.Rust {
	static class RustInstrCreateGenNames {
		public const string with_branch = nameof(with_branch);
		public const string with_far_branch = nameof(with_far_branch);
		public const string with_xbegin = nameof(with_xbegin);
		public const string with_mem64_reg = nameof(with_mem64_reg);
		public const string with_reg_mem64 = nameof(with_reg_mem64);
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
