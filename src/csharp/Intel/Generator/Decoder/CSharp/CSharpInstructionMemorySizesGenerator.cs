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

namespace Generator.Decoder.CSharp {
	sealed class CSharpInstructionMemorySizesGenerator : IInstructionMemorySizesGenerator {
		readonly ProjectDirs projectDirs;

		public CSharpInstructionMemorySizesGenerator(ProjectDirs projectDirs) =>
			this.projectDirs = projectDirs;

		public void Generate((EnumValue codeEnum, EnumValue mem, EnumValue bcst)[] data) {
			if (data.Select(a => a.codeEnum).ToHashSet().Count != CodeEnum.Instance.Values.Length)
				throw new InvalidOperationException();

			const string ClassName = "InstructionMemorySizes";
			var memSizeName = MemorySizeEnum.Instance.Name;
			using (var writer = new FileWriter(FileUtils.OpenWrite(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), ClassName + ".g.cs")))) {
				writer.WriteCSharpHeader();

				writer.WriteLine($"namespace {CSharpConstants.IcedNamespace} {{");
				writer.Indent();
				writer.WriteLine($"static class {ClassName} {{");
				writer.Indent();

				writer.WriteCommentLine("0 = memory size");
				writer.WriteCommentLine("1 = broadcast memory size");
				writer.WriteLine("internal static readonly byte[] Sizes = new byte[IcedConstants.NumberOfCodeValues * 2] {");
				writer.Indent();
				foreach (var d in data) {
					if (d.mem.Value > byte.MaxValue)
						throw new InvalidOperationException();
					string value;
					if (d.mem.Value == 0)
						value = "0";
					else
						value = $"(byte){memSizeName}.{d.mem.Name}";
					writer.WriteLine($"{value},// {d.codeEnum.Name}");
				}
				foreach (var d in data) {
					if (d.bcst.Value > byte.MaxValue)
						throw new InvalidOperationException();
					string value;
					if (d.bcst.Value == 0)
						value = "0";
					else
						value = $"(byte){memSizeName}.{d.bcst.Name}";
					writer.WriteLine($"{value},// {d.codeEnum.Name}");
				}
				writer.Unindent();
				writer.WriteLine("};");
				writer.Unindent();
				writer.WriteLine("}");
				writer.Unindent();
				writer.WriteLine("}");
			}
		}
	}
}
