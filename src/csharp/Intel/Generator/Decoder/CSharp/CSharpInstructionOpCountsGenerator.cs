// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using Generator.Constants;
using Generator.IO;
using Generator.Tables;

namespace Generator.Decoder.CSharp {
	[Generator(TargetLanguage.CSharp)]
	sealed class CSharpInstructionOpCountsGenerator {
		readonly IdentifierConverter idConverter;
		readonly GenTypes genTypes;

		public CSharpInstructionOpCountsGenerator(GeneratorContext generatorContext) {
			idConverter = CSharpIdentifierConverter.Create();
			genTypes = generatorContext.Types;
		}

		public void Generate() {
			var icedConstants = genTypes.GetConstantsType(TypeIds.IcedConstants);
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			const string ClassName = "InstructionOpCounts";
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(CSharpConstants.GetFilename(genTypes, CSharpConstants.IcedNamespace, ClassName + ".g.cs")))) {
				writer.WriteFileHeader();

				writer.WriteLine($"namespace {CSharpConstants.IcedNamespace} {{");
				using (writer.Indent()) {
					writer.WriteLine($"static class {ClassName} {{");
					using (writer.Indent()) {
						writer.WriteLineNoIndent($"#if {CSharpConstants.HasSpanDefine}");
						writer.WriteLine($"internal static System.ReadOnlySpan<byte> OpCount => new byte[{icedConstants.Name(idConverter)}.{icedConstants[IcedConstants.GetEnumCountName(TypeIds.Code)].Name(idConverter)}] {{");
						writer.WriteLineNoIndent("#else");
						writer.WriteLine($"internal static readonly byte[] OpCount = new byte[{icedConstants.Name(idConverter)}.{icedConstants[IcedConstants.GetEnumCountName(TypeIds.Code)].Name(idConverter)}] {{");
						writer.WriteLineNoIndent("#endif");
						using (writer.Indent()) {
							foreach (var def in defs)
								writer.WriteLine($"{def.OpCount},// {def.Code.Name(idConverter)}");
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
