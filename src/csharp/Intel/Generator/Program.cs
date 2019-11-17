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
		public GeneratorOptions? GeneratorOptions = null;
	}

	static class Program {
		static int Main(string[] args) {
			try {
				var generatorOptions = CreateGeneratorOptions(GeneratorFlags.None);
				Enums.CodeEnum.AddComments(generatorOptions.UnitTestsDir);

				new Decoder.DecoderTableGenerator(generatorOptions).Generate();
				new Decoder.InstructionMemorySizesGenerator(generatorOptions).Generate();
				new Decoder.InstructionOpCountsGenerator(generatorOptions).Generate();
				new Decoder.MnemonicsTableGenerator(generatorOptions).Generate();
				new Formatters.FormatterTableGenerator(generatorOptions).Generate();
				new InstructionInfo.CpuidFeatureTableGenerator(generatorOptions).Generate();
				new Enums.EnumsGenerator(generatorOptions).Generate();
				new Constants.ConstantsGenerator(generatorOptions).Generate();
				new Tables.MemorySizeInfoTableGenerator(generatorOptions).Generate();
				new Tables.RegisterInfoTableGenerator(generatorOptions).Generate();
				new Tables.D3nowCodeValuesTableGenerator(generatorOptions).Generate();

				return 0;
			}
			catch (Exception ex) {
				Console.WriteLine(ex.ToString());
				Debug.Fail("Excetion:\n\n" + ex.ToString());
				return 1;
			}
		}

		static GeneratorOptions CreateGeneratorOptions(GeneratorFlags flags) {
			var dir = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(typeof(Program).Assembly.Location)))))));
			if (dir is null || !File.Exists(Path.Combine(dir, "csharp", "Iced.sln")))
				throw new InvalidOperationException();
			return new GeneratorOptions(dir, flags);
		}
	}
}
