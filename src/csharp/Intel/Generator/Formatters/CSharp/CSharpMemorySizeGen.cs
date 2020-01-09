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
using Generator.Enums;
using Generator.IO;
using Generator.Tables;

namespace Generator.Formatters.CSharp {
	[Generator(TargetLanguage.CSharp, GeneratorNames.FormatterMemSize)]
	sealed class CSharpMemorySizeGen : MemorySizeGen {
		readonly IdentifierConverter idConverter;
		readonly GeneratorOptions generatorOptions;

		public CSharpMemorySizeGen(GeneratorOptions generatorOptions) {
			idConverter = CSharpIdentifierConverter.Create();
			this.generatorOptions = generatorOptions;
		}

		protected override void Generate((EnumValue memSize, BroadcastToKind bcst, IntelMemoryKeywords intel, MasmMemoryKeywords masm, NasmMemoryKeywords nasm)[] memInfos) {
			{
				var filename = Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.GasFormatterNamespace), "MemorySizes.cs");
				new FileUpdater(TargetLanguage.CSharp, "BcstTo", filename).Generate(writer => {
					var sizeTbl = MemorySizeInfoTable.Data;
					int first = (int)IcedConstantsType.Instance[IcedConstants.FirstBroadcastMemorySizeName].ValueUInt64;
					for (int i = first; i < memInfos.Length; i++)
						writer.WriteLine($"(byte)BroadcastToKind.{memInfos[i].bcst},");
				});
			}
			{
				var filename = Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IntelFormatterNamespace), "MemorySizes.cs");
				new FileUpdater(TargetLanguage.CSharp, "MemorySizes", filename).Generate(writer => {
					var codeStr = CodeEnum.Instance.Name(idConverter);
					foreach (var info in memInfos)
						writer.WriteLine($"(byte)((uint)MemoryKeywords.{info.intel} | ((uint)BroadcastToKind.{info.bcst} << BroadcastToKindShift)),");
				});
			}
			{
				var filename = Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.MasmFormatterNamespace), "MemorySizes.cs");
				new FileUpdater(TargetLanguage.CSharp, "MemorySizes", filename).Generate(writer => {
					var sizeTbl = MemorySizeInfoTable.Data;
					foreach (var info in memInfos) {
						var size = sizeTbl[(int)info.memSize.Value].Size;
						writer.WriteLine($"(ushort)((uint)MemoryKeywords.{info.masm} | ((uint)Size.S{size} << SizeKindShift)),");
					}
				});
			}
			{
				var filename = Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.NasmFormatterNamespace), "MemorySizes.cs");
				new FileUpdater(TargetLanguage.CSharp, "MemorySizes", filename).Generate(writer => {
					var sizeTbl = MemorySizeInfoTable.Data;
					foreach (var info in memInfos) {
						var size = sizeTbl[(int)info.memSize.Value].Size;
						var name = info.nasm.ToString();
						if (name == "byte")
							name = "@" + name;
						writer.WriteLine($"(byte)((uint)MemoryKeywords.{name} | ((uint)Size.S{size} << SizeKindShift)),");
					}
				});
				new FileUpdater(TargetLanguage.CSharp, "BcstTo", filename).Generate(writer => {
					var sizeTbl = MemorySizeInfoTable.Data;
					int first = (int)IcedConstantsType.Instance[IcedConstants.FirstBroadcastMemorySizeName].ValueUInt64;
					for (int i = first; i < memInfos.Length; i++)
						writer.WriteLine($"(byte)BroadcastToKind.{memInfos[i].bcst},");
				});
			}
		}
	}
}
