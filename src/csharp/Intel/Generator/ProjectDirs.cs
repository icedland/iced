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
	sealed class ProjectDirs {
		public readonly string UnitTestsDir;
		public string CSharpDir => langDirs[(int)TargetLanguage.CSharp];
		public string RustDir => langDirs[(int)TargetLanguage.Rust];
		readonly string[] langDirs;

		public ProjectDirs(string baseDir) {
			UnitTestsDir = GetAndVerifyPath(baseDir, "UnitTests", "Intel");
			langDirs = new string[(int)TargetLanguage.Last];
			for (int i = 0; i < langDirs.Length; i++) {
				string path;
				switch ((TargetLanguage)i) {
				case TargetLanguage.CSharp:
					path = GetAndVerifyPath(baseDir, "csharp", "Intel", "Iced");
					break;
				case TargetLanguage.Rust:
					path = GetAndVerifyPath(baseDir, "rust", "iced-x86", "src", "x86");
					break;

				case TargetLanguage.Last:
				default:
					throw new InvalidOperationException();
				}
				langDirs[i] = path;
			}
		}

		static string GetAndVerifyPath(params string[] paths) {
			var path = Path.Combine(paths);
			if (!Directory.Exists(path))
				throw new InvalidOperationException($"Directory {path} doesn't exist");
			return path;
		}
	}
}
