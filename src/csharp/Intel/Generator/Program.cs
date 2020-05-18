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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Generator {
	sealed class GeneratorInfoComparer : IComparer<GeneratorInfo> {
		public int Compare(GeneratorInfo x, GeneratorInfo y) {
			int c = GetOrder(x.Language).CompareTo(GetOrder(y.Language));
			if (c != 0)
				return c;
			return StringComparer.OrdinalIgnoreCase.Compare(x.Name, y.Name);
		}

		static int GetOrder(TargetLanguage language) => (int)language;
	}

	sealed class GeneratorInfo {
		const string InvokeMethodName = "Generate";
		readonly ConstructorInfo ctor;
		readonly MethodInfo method;

		public TargetLanguage Language { get; }
		public string Name { get; }

		public GeneratorInfo(TargetLanguage language, string name, Type type) {
			Language = language;
			Name = name;

			var ctor = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, CallingConventions.Standard, new[] { typeof(GeneratorContext) }, null);
			if (ctor is null)
				throw new InvalidOperationException($"Generator {type.FullName} doesn't have a constructor that takes a {nameof(GeneratorContext)} argument");
			this.ctor = ctor;

			var method = type.GetMethod(InvokeMethodName, 0, BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.Standard, Array.Empty<Type>(), null);
			if (method is null || method.ReturnType != typeof(void))
				throw new InvalidOperationException($"Generator {type.FullName} doesn't have a public void {InvokeMethodName}() method");
			this.method = method;
		}

		public void Invoke(GeneratorContext generatorContext) {
			var instance = ctor.Invoke(new[] { generatorContext }) ?? throw new InvalidOperationException();
			method.Invoke(instance, null);
		}
	}

	sealed class CommandLineOptions {
		public readonly HashSet<TargetLanguage> Languages = new HashSet<TargetLanguage>();
		public GeneratorFlags GeneratorFlags = GeneratorFlags.None;
	}

	static class Program {
		static int Main(string[] args) {
			try {
				if (!TryParseCommandLine(args, out var options, out var error)) {
					Help();
					if (error != string.Empty) {
						Console.WriteLine();
						Console.WriteLine(error);
					}
					return 1;
				}

				var generatorContext = CreateGeneratorContext(options.GeneratorFlags);
				CodeComments.AddComments(generatorContext.Types, generatorContext.UnitTestsDir);

				// It's not much of an improvement in speed at the moment.
				// Group by lang since different lang gens don't write to the same files.
				Parallel.ForEach(Filter(GetGenerators(), options).GroupBy(a => a.Language).Select(a => a.ToArray()), genInfos => {
					foreach (var genInfo in genInfos)
						genInfo.Invoke(generatorContext);
				});

				return 0;
			}
			catch (Exception ex) {
				Console.WriteLine(ex.ToString());
				Debug.Fail("Exception:\n\n" + ex.ToString());
				return 1;
			}
		}

		static IEnumerable<GeneratorInfo> Filter(List<GeneratorInfo> genInfos, CommandLineOptions options) {
			var okLang = new bool[Enum.GetValues(typeof(TargetLanguage)).Length];
			if (options.Languages.Count == 0) {
				for (int i = 0; i < okLang.Length; i++)
					okLang[i] = true;
			}
			else {
				foreach (var lang in options.Languages)
					okLang[(int)lang] = true;
			}

			foreach (var genInfo in genInfos) {
				if (!okLang[(int)genInfo.Language])
					continue;

				yield return genInfo;
			}
		}

		static void Help() {
			Console.WriteLine(@"
Generates code and data

Options:

-h, --help
    Show this message
-l, --language <language>
    Select only this language. Multiple language options are allowed.
    Valid languages (case insensitive):
        C#
        CSharp
        Rust
        RustJS
--no-formatter
    Don't include any formatter
--no-gas-formatter
    Don't include the gas (AT&T) formatter
--no-intel-formatter
    Don't include the Intel (XED) formatter
--no-masm-formatter
    Don't include the masm formatter
--no-nasm-formatter
    Don't include the nasm formatter
");
		}

		static bool TryParseCommandLine(string[] args, [NotNullWhen(true)] out CommandLineOptions? options, [NotNullWhen(false)] out string? error) {
			if (Enum.GetValues(typeof(TargetLanguage)).Length != 3)
				throw new InvalidOperationException("Enum updated, update help message and this method");
			options = new CommandLineOptions();
			for (int i = 0; i < args.Length; i++) {
				var arg = args[i];
				var value = i + 1 < args.Length ? args[i + 1] : null;
				switch (arg) {
				case "-h":
				case "--help":
					error = string.Empty;
					return false;

				case "-l":
				case "--language":
					if (value is null) {
						error = "Missing language";
						return false;
					}
					i++;
					switch (value.ToLowerInvariant()) {
					case "c#":
					case "csharp":
						options.Languages.Add(TargetLanguage.CSharp);
						break;
					case "rust":
						options.Languages.Add(TargetLanguage.Rust);
						break;
					case "rustjs":
						options.Languages.Add(TargetLanguage.RustJS);
						break;
					default:
						error = $"Unknown language: {value}";
						return false;
					}
					break;

				case "--no-formatter":
					options.GeneratorFlags |= GeneratorFlags.NoFormatter;
					break;

				case "--no-gas-formatter":
					options.GeneratorFlags |= GeneratorFlags.NoGasFormatter;
					break;

				case "--no-intel-formatter":
					options.GeneratorFlags |= GeneratorFlags.NoIntelFormatter;
					break;

				case "--no-masm-formatter":
					options.GeneratorFlags |= GeneratorFlags.NoMasmFormatter;
					break;

				case "--no-nasm-formatter":
					options.GeneratorFlags |= GeneratorFlags.NoNasmFormatter;
					break;

				default:
					error = $"Unknown option: {value}";
					return false;
				}
			}
			error = null;
			return true;
		}

		static GeneratorContext CreateGeneratorContext(GeneratorFlags flags) {
			var dir = Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(typeof(Program).Assembly.Location)))))));
			if (dir is null || !File.Exists(Path.Combine(dir, "csharp", "Iced.sln")))
				throw new InvalidOperationException();
			return new GeneratorContext(dir, flags);
		}

		static List<GeneratorInfo> GetGenerators() {
			var result = new List<GeneratorInfo>();
			foreach (var type in typeof(Program).Assembly.GetTypes()) {
				var attr = (GeneratorAttribute?)type.GetCustomAttribute(typeof(GeneratorAttribute));
				if (attr is null)
					continue;
				result.Add(new GeneratorInfo(attr.Language, attr.Name, type));
			}
			result.Sort(new GeneratorInfoComparer());
			return result;
		}
	}
}
