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
using Generator.Constants;
using Generator.Constants.CSharp;
using Generator.Enums;
using Generator.Enums.CSharp;
using Generator.Enums.InstructionInfo;
using Generator.IO;

namespace Generator.InstructionInfo.CSharp {
	[Generator(TargetLanguage.CSharp, GeneratorNames.InstrInfo)]
	sealed class CSharpInstrInfoGenerator : InstrInfoGenerator {
		readonly IdentifierConverter idConverter;
		readonly CSharpEnumsGenerator enumGenerator;
		readonly CSharpConstantsGenerator constantsGenerator;
		readonly GeneratorContext generatorContext;

		public CSharpInstrInfoGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			idConverter = CSharpIdentifierConverter.Create();
			enumGenerator = new CSharpEnumsGenerator(generatorContext);
			constantsGenerator = new CSharpConstantsGenerator(generatorContext);
			this.generatorContext = generatorContext;
		}

		protected override void Generate(EnumType enumType) => enumGenerator.Generate(enumType);
		protected override void Generate(ConstantsType constantsType) => constantsGenerator.Generate(constantsType);

		protected override void Generate((InstrInfo info, uint dword1, uint dword2)[] infos) {
			var filename = Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.InstructionInfoNamespace), "InstrInfoTable.g.cs");
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLineNoIndent($"#if {CSharpConstants.InstructionInfoDefine}");
				writer.WriteLine($"namespace {CSharpConstants.InstructionInfoNamespace} {{");
				using (writer.Indent()) {
					writer.WriteLine("static class InstrInfoTable {");
					using (writer.Indent()) {
						writer.WriteLine($"internal static readonly uint[] Data = new uint[{infos.Length * 2}] {{");
						using (writer.Indent()) {
							foreach (var info in infos)
								writer.WriteLine($"0x{info.dword1:X8}, 0x{info.dword2:X8},// {info.info.Code.Name(idConverter)}");
						}
						writer.WriteLine("};");
					}
					writer.WriteLine("}");
				}
				writer.WriteLine("}");
				writer.WriteLineNoIndent("#endif");
			}
		}

		protected override void Generate(EnumValue[] enumValues, RflagsBits[] read, RflagsBits[] undefined, RflagsBits[] written, RflagsBits[] cleared, RflagsBits[] set, RflagsBits[] modified) {
			var filename = Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.InstructionInfoNamespace), "RflagsInfoConstants.g.cs");
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(filename))) {
				writer.WriteFileHeader();
				writer.WriteLineNoIndent($"#if {CSharpConstants.InstructionInfoDefine}");
				writer.WriteLine($"namespace {CSharpConstants.InstructionInfoNamespace} {{");
				using (writer.Indent()) {
					writer.WriteLine("static class RflagsInfoConstants {");
					using (writer.Indent()) {
						var infos = new (RflagsBits[] rflags, string name)[] {
							(read, "read"),
							(undefined, "undefined"),
							(written, "written"),
							(cleared, "cleared"),
							(set, "set"),
							(modified, "modified"),
						};
						foreach (var info in infos) {
							var rflags = info.rflags;
							if (rflags.Length != infos[0].rflags.Length)
								throw new InvalidOperationException();
							var name = idConverter.Field("flags" + info.name.Substring(0, 1).ToUpperInvariant() + info.name.Substring(1));
							writer.WriteLine($"public static readonly ushort[] {name} = new ushort[{rflags.Length}] {{");
							using (writer.Indent()) {
								for (int i = 0; i < rflags.Length; i++) {
									var rfl = rflags[i];
									uint value = (uint)rfl;
									if (value > ushort.MaxValue)
										throw new InvalidOperationException();
									writer.WriteLine($"0x{value:X4},// {enumValues[i].Name(idConverter)}");
								}
							}
							writer.WriteLine("};");
						}
					}
					writer.WriteLine("}");
				}
				writer.WriteLine("}");
				writer.WriteLineNoIndent("#endif");
			}
		}

		protected override void Generate((EnumValue cpuidInternal, EnumValue[] cpuidFeatures)[] cpuidFeatures) {
			var header = new byte[(cpuidFeatures.Length + 7) / 8];
			for (int i = 0; i < cpuidFeatures.Length; i++) {
				int len = cpuidFeatures[i].cpuidFeatures.Length;
				if (len < 1 || len > 2)
					throw new InvalidOperationException();
				header[i / 8] |= (byte)((len - 1) << (i % 8));
			}

			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.InstructionInfoNamespace), "CpuidFeatureInternalData.g.cs")))) {
				writer.WriteFileHeader();
				writer.WriteLineNoIndent($"#if {CSharpConstants.InstructionInfoDefine}");
				writer.WriteLine($"namespace {CSharpConstants.InstructionInfoNamespace} {{");
				using (writer.Indent()) {
					writer.WriteLine("static partial class CpuidFeatureInternalData {");
					using (writer.Indent()) {
						writer.WriteLineNoIndent($"#if {CSharpConstants.HasSpanDefine}");
						writer.WriteLine("static System.ReadOnlySpan<byte> GetGetCpuidFeaturesData() =>");
						writer.WriteLineNoIndent("#else");
						writer.WriteLine("static byte[] GetGetCpuidFeaturesData() =>");
						writer.WriteLineNoIndent("#endif");
						using (writer.Indent()) {
							writer.WriteLine("new byte[] {");
							using (writer.Indent()) {
								writer.WriteCommentLine("Header");
								foreach (var b in header) {
									writer.WriteByte(b);
									writer.WriteLine();
								}
								writer.WriteLine();
								foreach (var info in cpuidFeatures) {
									foreach (var f in info.cpuidFeatures) {
										if ((uint)f.Value > byte.MaxValue)
											throw new InvalidOperationException();
										writer.WriteByte((byte)f.Value);
									}
									writer.WriteCommentLine(info.cpuidInternal.Name(idConverter));
								}
							}
							writer.WriteLine("};");
						}
					}
					writer.WriteLine("}");
				}
				writer.WriteLine("}");
				writer.WriteLineNoIndent("#endif");
			}
		}

		protected override void GenerateCore() => GenerateOpAccesses();

		void GenerateOpAccesses() {
			var filename = Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.InstructionInfoNamespace), "InfoHandlerFlags.cs");
			new FileUpdater(TargetLanguage.CSharp, "OpAccesses", filename).Generate(writer => GenerateOpAccesses(writer));
		}

		void GenerateOpAccesses(FileWriter writer) {
			var opInfos = instrInfoTypes.EnumOpInfos;
			// We assume max op count is 5, update the code if not
			if (opInfos.Length != 5)
				throw new InvalidOperationException();

			var indexes = new int[] { 1, 2 };
			var opAccessTypeStr = genTypes[TypeIds.OpAccess].Name(idConverter);
			foreach (var index in indexes) {
				var opInfo = opInfos[index];
				writer.WriteLine($"public static readonly {opAccessTypeStr}[] Op{index} = new {opAccessTypeStr}[{opInfo.Values.Length}] {{");
				using (writer.Indent()) {
					foreach (var value in opInfo.Values) {
						var v = ToOpAccess(value);
						writer.WriteLine($"{opAccessTypeStr}.{v.Name(idConverter)},");
					}
				}
				writer.WriteLine("};");
			}
		}
	}
}
