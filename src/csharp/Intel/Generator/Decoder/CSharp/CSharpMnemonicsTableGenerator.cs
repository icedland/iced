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
using Generator.Enums;
using Generator.IO;

namespace Generator.Decoder.CSharp {
	sealed class CSharpMnemonicsTableGenerator : IMnemonicsTableGenerator {
		readonly ProjectDirs projectDirs;

		public CSharpMnemonicsTableGenerator(ProjectDirs projectDirs) =>
			this.projectDirs = projectDirs;

		public void Generate((EnumValue codeEnum, EnumValue mnemonicEnum)[] data) {
			const string ClassName = "MnemonicUtils";
			var mnemonicName = MnemonicEnum.Instance.Name;
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), ClassName + ".Data.g.cs")))) {
				writer.WriteFileHeader();

				writer.WriteLine($"namespace {CSharpConstants.IcedNamespace} {{");
				writer.Indent();
				writer.WriteLine($"static partial class {ClassName} {{");
				writer.Indent();

				writer.WriteLine("internal static readonly ushort[] toMnemonic = new ushort[IcedConstants.NumberOfCodeValues] {");
				writer.Indent();
				foreach (var d in data) {
					if (d.mnemonicEnum.Value > ushort.MaxValue)
						throw new InvalidOperationException();
					writer.WriteLine($"(ushort){mnemonicName}.{d.mnemonicEnum.Name},// {d.codeEnum.Name}");
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
