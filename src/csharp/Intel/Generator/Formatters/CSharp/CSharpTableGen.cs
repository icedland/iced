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
using System.Text;
using Generator.Constants;
using Generator.Enums;
using Generator.IO;
using Generator.Tables;

namespace Generator.Formatters.CSharp {
	[Generator(TargetLanguage.CSharp, GeneratorNames.FormatterMemSize)]
	sealed class CSharpTableGen : TableGen {
		readonly GeneratorContext generatorContext;
		readonly IdentifierConverter idConverter;

		public CSharpTableGen(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			this.generatorContext = generatorContext;
			idConverter = CSharpIdentifierConverter.Create();
		}

		protected override void Generate((EnumValue memSize, BroadcastToKind bcst, IntelMemoryKeywords intel, MasmMemoryKeywords masm, NasmMemoryKeywords nasm)[] memInfos) {
			var icedConstants = genTypes.GetConstantsType(TypeIds.IcedConstants);
			var sizeTbl = genTypes.GetObject<MemorySizeInfoTable>(TypeIds.MemorySizeInfoTable).Data;
			{
				var filename = Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.GasFormatterNamespace), "MemorySizes.cs");
				new FileUpdater(TargetLanguage.CSharp, "BcstTo", filename).Generate(writer => {
					int first = (int)icedConstants[IcedConstants.FirstBroadcastMemorySizeName].ValueUInt64;
					for (int i = first; i < memInfos.Length; i++)
						writer.WriteLine($"(byte)BroadcastToKind.{memInfos[i].bcst},");
				});
			}
			{
				var filename = Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.IntelFormatterNamespace), "MemorySizes.cs");
				new FileUpdater(TargetLanguage.CSharp, "MemorySizes", filename).Generate(writer => {
					var codeStr = genTypes[TypeIds.Code].Name(idConverter);
					foreach (var info in memInfos)
						writer.WriteLine($"(byte)((uint)MemoryKeywords.{info.intel} | ((uint)BroadcastToKind.{info.bcst} << BroadcastToKindShift)),");
				});
			}
			{
				var filename = Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.MasmFormatterNamespace), "MemorySizes.cs");
				new FileUpdater(TargetLanguage.CSharp, "MemorySizes", filename).Generate(writer => {
					foreach (var info in memInfos) {
						var size = sizeTbl[(int)info.memSize.Value].Size;
						writer.WriteLine($"(ushort)((uint)MemoryKeywords.{info.masm} | ((uint)Size.S{size} << SizeKindShift)),");
					}
				});
			}
			{
				var filename = Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.NasmFormatterNamespace), "MemorySizes.cs");
				new FileUpdater(TargetLanguage.CSharp, "MemorySizes", filename).Generate(writer => {
					foreach (var info in memInfos) {
						var size = sizeTbl[(int)info.memSize.Value].Size;
						var name = info.nasm.ToString();
						if (name == "byte")
							name = "@" + name;
						writer.WriteLine($"(byte)MemoryKeywords.{name},");
					}
				});
				new FileUpdater(TargetLanguage.CSharp, "BcstTo", filename).Generate(writer => {
					int first = (int)icedConstants[IcedConstants.FirstBroadcastMemorySizeName].ValueUInt64;
					for (int i = first; i < memInfos.Length; i++)
						writer.WriteLine($"(byte)BroadcastToKind.{memInfos[i].bcst},");
				});
			}
		}

		protected override void GenerateRegisters(string[] registers) {
			var filename = Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.FormatterNamespace), "RegistersTable.cs");
			new FileUpdater(TargetLanguage.CSharp, "Registers", filename).Generate(writer => {
				writer.WriteLineNoIndent($"#if {CSharpConstants.HasSpanDefine}");
				writer.WriteLine("static ReadOnlySpan<byte> GetRegistersData() =>");
				writer.WriteLineNoIndent("#else");
				writer.WriteLine("static byte[] GetRegistersData() =>");
				writer.WriteLineNoIndent("#endif");
				int maxLen = 0;
				using (writer.Indent()) {
					writer.WriteLine("new byte[] {");
					using (writer.Indent()) {
						foreach (var register in registers) {
							maxLen = Math.Max(maxLen, register.Length);
							var bytes = Encoding.UTF8.GetBytes(register);
							writer.Write($"0x{bytes.Length:X2}");
							foreach (var b in bytes)
								writer.Write($", 0x{b:X2}");
							writer.Write(",");
							writer.WriteCommentLine(register);
						}
					}
					writer.WriteLine("};");
				}
				writer.WriteLine($"const int MaxStringLength = {maxLen};");
				writer.WriteLine($"const int StringsCount = {registers.Length};");
			});
		}

		protected override void GenerateFormatterFlowControl((EnumValue flowCtrl, EnumValue[] code)[] infos) {
			var filename = Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.IcedNamespace), "FormatterUtils.cs");
			new FileUpdater(TargetLanguage.CSharp, "FormatterFlowControlSwitch", filename).Generate(writer => {
				var codeStr = genTypes[TypeIds.Code].Name(idConverter);
				var flowCtrlStr = genTypes[TypeIds.FormatterFlowControl].Name(idConverter);
				foreach (var info in infos) {
					if (info.code.Length == 0)
						continue;
					foreach (var c in info.code)
						writer.WriteLine($"case {codeStr}.{c.Name(idConverter)}:");
					using (writer.Indent())
						writer.WriteLine($"return {flowCtrlStr}.{info.flowCtrl.Name(idConverter)};");
				}
			});
		}
	}
}
