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
using System.Linq;

namespace Generator.Constants {
	abstract class ConstantsTypeGenerator {
		public abstract void Generate(ConstantsType constantsType);
	}

	sealed class ConstantsGenerator {
		readonly ProjectDirs projectDirs;

		public ConstantsGenerator(ProjectDirs projectDirs) => this.projectDirs = projectDirs;

		static readonly ConstantsType[] allConstants = new ConstantsType[] {
			IcedConstantsType.Instance,
		};

		public void Generate() {
			if (allConstants.Select(a => a.Kind).ToHashSet().Count != Enum.GetValues(typeof(ConstantsTypeKind)).Length)
				throw new InvalidOperationException($"Missing at least one {nameof(ConstantsTypeKind)} value");

			var generators = new ConstantsTypeGenerator[] {
				new CSharp.CSharpConstantsTypeGenerator(projectDirs),
			};

			foreach (var generator in generators) {
				foreach (var constantsType in allConstants)
					generator.Generate(constantsType);
			}
		}
	}
}
