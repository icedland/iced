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

using System.IO;
using Generator.Constants;
using Generator.IO;
using Generator.Tables;

namespace Generator.Decoder.CSharp {
	[Generator(TargetLanguage.CSharp, GeneratorNames.Code_OpCount)]
	sealed class CSharpInstructionOpCountsGenerator {
		readonly IdentifierConverter idConverter;
		readonly GeneratorContext generatorContext;

		public CSharpInstructionOpCountsGenerator(GeneratorContext generatorContext) {
			idConverter = CSharpIdentifierConverter.Create();
			this.generatorContext = generatorContext;
		}

		public void Generate() {
			var genTypes = generatorContext.Types;
			var icedConstants = genTypes.GetConstantsType(TypeIds.IcedConstants);
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			const string ClassName = "InstructionOpCounts";
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.IcedNamespace), ClassName + ".g.cs")))) {
				writer.WriteFileHeader();

				writer.WriteLine($"namespace {CSharpConstants.IcedNamespace} {{");
				using (writer.Indent()) {
					writer.WriteLine($"static class {ClassName} {{");
					using (writer.Indent()) {
						writer.WriteLineNoIndent($"#if {CSharpConstants.HasSpanDefine}");
						writer.WriteLine($"internal static System.ReadOnlySpan<byte> OpCount => new byte[{icedConstants.Name(idConverter)}.{icedConstants[IcedConstants.NumberOfCodeValuesName].Name(idConverter)}] {{");
						writer.WriteLineNoIndent("#else");
						writer.WriteLine($"internal static readonly byte[] OpCount = new byte[{icedConstants.Name(idConverter)}.{icedConstants[IcedConstants.NumberOfCodeValuesName].Name(idConverter)}] {{");
						writer.WriteLineNoIndent("#endif");
						using (writer.Indent()) {
							foreach (var def in defs)
								writer.WriteLine($"{def.OpCount},// {def.OpCodeInfo.Code.Name(idConverter)}");
						}
						writer.WriteLine("};");
					}
					writer.WriteLine("}");
				}
				writer.WriteLine("}");
			}
		}
	}
}
