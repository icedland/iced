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
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Generator {
	enum Command {
		Decoder,
		Formatter,
		CpuidFeature,
		Constants,
	}

	sealed class CommandLineOptions {
		public readonly List<Command> Commands = new List<Command>();
		public ProjectDirs? ProjectDirs = null;
	}

	static class Program {
		static int Main(string[] args) {
			try {
				var projectDirs = GetProjectDirs();
				Enums.CodeEnum.AddComments(projectDirs.UnitTestsDir);

				new Decoder.DecoderTableGenerator(projectDirs).Generate();
#if (!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER
				new Formatters.FormatterTableGenerator(projectDirs).Generate();
#endif
				new InstructionInfo.CpuidFeatureTableGenerator(projectDirs).Generate();
				new Enums.EnumsGenerator(projectDirs).Generate();
				new Constants.ConstantsGenerator(projectDirs).Generate();

				return 0;
			}
			catch (Exception ex) {
				Console.WriteLine(ex.ToString());
				Debug.Fail("Excetion:\n\n" + ex.ToString());
				return 1;
			}
		}

		static ProjectDirs GetProjectDirs() {
			var dir = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(typeof(Program).Assembly.Location)))))));
			if (dir is null || !File.Exists(Path.Combine(dir, "csharp", "Iced.sln")))
				throw new InvalidOperationException();
			return new ProjectDirs(dir);
		}
	}
}
