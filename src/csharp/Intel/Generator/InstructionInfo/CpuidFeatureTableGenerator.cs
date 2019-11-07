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
using System.Linq;
using Generator.Enums;
using Generator.IO;

namespace Generator.InstructionInfo {
	sealed class CpuidFeatureTableGenerator {
		readonly ProjectDirs projectDirs;

		public CpuidFeatureTableGenerator(ProjectDirs projectDirs) => this.projectDirs = projectDirs;

		public void Generate() {
			var cpuidFeatures = CpuidFeatureInternalEnum.AllCombinations;
			var header = new byte[(cpuidFeatures.Length + 7) / 8];
			for (int i = 0; i < cpuidFeatures.Length; i++) {
				int len = cpuidFeatures[i].Length;
				if (len < 1 || len > 2)
					throw new InvalidOperationException();
				header[i / 8] |= (byte)((len - 1) << (i % 8));
			}

			using (var writer = new FileWriter(FileUtils.OpenWrite(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.InstructionInfoNamespace), "CpuidFeatureInternalData.g.cs")))) {
				writer.WriteCSharpHeader();
				writer.WriteLine($"#if {CSharpConstants.InstructionInfoDefine}");
				writer.WriteLine($"namespace {CSharpConstants.InstructionInfoNamespace} {{");
				writer.Indent();
				writer.WriteLine("static partial class CpuidFeatureInternalData {");
				writer.Indent();
				writer.WriteLine("static byte[] GetGetCpuidFeaturesData() =>");
				writer.Indent();
				writer.WriteLine("new byte[] {");
				writer.Indent();

				writer.WriteCommentLine("Header");
				foreach (var b in header) {
					writer.WriteByte(b);
					writer.WriteLine();
				}
				writer.WriteLine();
				foreach (var info in cpuidFeatures) {
					foreach (var f in info) {
						if ((uint)f.Value > byte.MaxValue)
							throw new InvalidOperationException();
						writer.WriteByte((byte)f.Value);
					}
					writer.WriteCommentLine(string.Join(", ", info.Select(a => a.Name).ToArray()));
				}

				writer.Unindent();
				writer.WriteLine("};");
				writer.Unindent();
				writer.Unindent();
				writer.WriteLine("}");
				writer.Unindent();
				writer.WriteLine("}");
				writer.WriteLine("#endif");
			}
		}
	}
}
