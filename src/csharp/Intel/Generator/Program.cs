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
using System.Reflection;

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

			var ctor = type.GetConstructor(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, CallingConventions.Standard, new[] { typeof(GeneratorOptions) }, null);
			if (ctor is null)
				throw new InvalidOperationException($"Generator {type.FullName} doesn't have a constructor that takes a {nameof(GeneratorOptions)} argument");
			this.ctor = ctor;

			var method = type.GetMethod(InvokeMethodName, 0, BindingFlags.Public | BindingFlags.Instance, null, CallingConventions.Standard, Array.Empty<Type>(), null);
			if (method is null || method.ReturnType != typeof(void))
				throw new InvalidOperationException($"Generator {type.FullName} doesn't have a public void {InvokeMethodName}() method");
			this.method = method;
		}

		public void Invoke(GeneratorOptions generatorOptions) {
			var instance = ctor.Invoke(new[] { generatorOptions }) ?? throw new InvalidOperationException();
			method.Invoke(instance, null);
		}
	}

	static class Program {
		static int Main(string[] args) {
			try {
				var generatorOptions = CreateGeneratorOptions(GeneratorFlags.None);
				Enums.CodeEnum.AddComments(generatorOptions.UnitTestsDir);

				var genInfos = GetGenerators();
				foreach (var genInfo in genInfos)
					genInfo.Invoke(generatorOptions);

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
