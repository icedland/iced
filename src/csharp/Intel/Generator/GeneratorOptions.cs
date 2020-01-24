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

using System;
using System.IO;

namespace Generator {
	[Flags]
	enum GeneratorFlags {
		None				= 0,
		NoFormatter			= 0x0000_0001,
		NoGasFormatter		= 0x0000_0002,
		NoIntelFormatter	= 0x0000_0004,
		NoMasmFormatter		= 0x0000_0008,
		NoNasmFormatter		= 0x0000_0010,
	}

	sealed class GeneratorOptions {
		public bool HasGasFormatter { get; }
		public bool HasIntelFormatter { get; }
		public bool HasMasmFormatter { get; }
		public bool HasNasmFormatter { get; }
		public string UnitTestsDir { get; }
		public string CSharpDir => langDirs[(int)TargetLanguage.CSharp];
		public string CSharpTestsDir { get; }
		public string RustDir => langDirs[(int)TargetLanguage.Rust];
		readonly string[] langDirs;

		public GeneratorOptions(string baseDir, GeneratorFlags flags) {
			HasGasFormatter = (flags & GeneratorFlags.NoGasFormatter) == 0 && (flags & GeneratorFlags.NoFormatter) == 0;
			HasIntelFormatter = (flags & GeneratorFlags.NoIntelFormatter) == 0 && (flags & GeneratorFlags.NoFormatter) == 0;
			HasMasmFormatter = (flags & GeneratorFlags.NoMasmFormatter) == 0 && (flags & GeneratorFlags.NoFormatter) == 0;
			HasNasmFormatter = (flags & GeneratorFlags.NoNasmFormatter) == 0 && (flags & GeneratorFlags.NoFormatter) == 0;
			UnitTestsDir = GetAndVerifyPath(baseDir, "UnitTests", "Intel");
			langDirs = new string[Enum.GetValues(typeof(TargetLanguage)).Length];
			for (int i = 0; i < langDirs.Length; i++) {
				string path = (TargetLanguage)i switch {
					TargetLanguage.CSharp => GetAndVerifyPath(baseDir, "csharp", "Intel", "Iced"),
					TargetLanguage.Rust => GetAndVerifyPath(baseDir, "rust", "iced-x86", "src"),
					_ => throw new InvalidOperationException(),
				};
				langDirs[i] = path;
			}
			CSharpTestsDir = GetAndVerifyPath(baseDir, "csharp", "Intel", "Iced.UnitTests");
		}

		static string GetAndVerifyPath(params string[] paths) {
			var path = Path.Combine(paths);
			if (!Directory.Exists(path))
				throw new InvalidOperationException($"Directory {path} doesn't exist");
			return path;
		}
	}
}
