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
	enum Command {
		None,
		Formatter,
	}

	sealed class CommandLineOptions {
		public Command Command = Command.None;
		public string? IcedProjectDir = null;
	}

	static class Program {
		static int Main(string[] args) {
			try {
				if (!ParseCommandLine(args, out var options)) {
					Usage();
					return 1;
				}

				if (options.IcedProjectDir is null)
					options.IcedProjectDir = GetIcedProjectDir();

				switch (options.Command) {
				case Command.Formatter:
					new Formatters.FormatterTableGenerator(options.IcedProjectDir).Generate();
					break;

				case Command.None:
				default:
					throw new InvalidOperationException();
				}

				return 0;
			}
			catch (Exception ex) {
				Console.WriteLine(ex.ToString());
				return 1;
			}
		}

		static string GetIcedProjectDir() {
			var dir = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(typeof(Program).Assembly.Location)))));
			if (!File.Exists(Path.Combine(dir, "Iced.sln")))
				throw new InvalidOperationException();
			dir = Path.Combine(dir, "Iced");
			if (!File.Exists(Path.Combine(dir, "Iced.csproj")))
				throw new InvalidOperationException();
			return dir;
		}

		static void Usage() {
			Console.WriteLine(@"Generator <command>
command:
    --formatter         Generate formatter tables
");
		}

		static bool ParseCommandLine(string[] args, out CommandLineOptions options) {
			options = new CommandLineOptions();
			foreach (var arg in args) {
				switch (arg) {
				case "-?":
				case "--help":
					return false;

				case "--formatter":
					if (options.Command != Command.None)
						return false;
					options.Command = Command.Formatter;
					break;

				default:
					return false;
				}
			}
			return options.Command != Command.None;
		}
	}
}
