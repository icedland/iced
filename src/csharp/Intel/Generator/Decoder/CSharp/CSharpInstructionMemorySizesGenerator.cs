// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using Generator.Constants;
using Generator.IO;
using Generator.Tables;

namespace Generator.Decoder.CSharp {
	[Generator(TargetLanguage.CSharp)]
	sealed class CSharpInstructionMemorySizesGenerator {
		readonly IdentifierConverter idConverter;
		readonly GenTypes genTypes;

		public CSharpInstructionMemorySizesGenerator(GeneratorContext generatorContext) {
			idConverter = CSharpIdentifierConverter.Create();
			genTypes = generatorContext.Types;
		}

		public void Generate() {
			var icedConstants = genTypes.GetConstantsType(TypeIds.IcedConstants);
			var defs = genTypes.GetObject<InstructionDefs>(TypeIds.InstructionDefs).Defs;
			const string ClassName = "InstructionMemorySizes";
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(CSharpConstants.GetFilename(genTypes, CSharpConstants.IcedNamespace, ClassName + ".g.cs")))) {
				writer.WriteFileHeader();

				writer.WriteLine($"namespace {CSharpConstants.IcedNamespace} {{");
				using (writer.Indent()) {
					writer.WriteLine($"static class {ClassName} {{");
					using (writer.Indent()) {
						writer.WriteLineNoIndent($"#if {CSharpConstants.HasSpanDefine}");
						writer.WriteLine($"internal static System.ReadOnlySpan<byte> SizesNormal => new byte[{icedConstants.Name(idConverter)}.{icedConstants[IcedConstants.GetEnumCountName(TypeIds.Code)].Name(idConverter)}] {{");
						writer.WriteLineNoIndent("#else");
						writer.WriteLine($"internal static readonly byte[] SizesNormal = new byte[{icedConstants.Name(idConverter)}.{icedConstants[IcedConstants.GetEnumCountName(TypeIds.Code)].Name(idConverter)}] {{");
						writer.WriteLineNoIndent("#endif");
						using (writer.Indent()) {
							foreach (var def in defs) {
								if (def.Memory.Value > byte.MaxValue)
									throw new InvalidOperationException();
								string value;
								if (def.Memory.Value == 0)
									value = "0";
								else
									value = $"(byte){idConverter.ToDeclTypeAndValue(def.Memory)}";
								writer.WriteLine($"{value},// {def.Code.Name(idConverter)}");
							}
						}
						writer.WriteLine("};");
						writer.WriteLine();
						writer.WriteLineNoIndent($"#if {CSharpConstants.HasSpanDefine}");
						writer.WriteLine($"internal static System.ReadOnlySpan<byte> SizesBcst => new byte[{icedConstants.Name(idConverter)}.{icedConstants[IcedConstants.GetEnumCountName(TypeIds.Code)].Name(idConverter)}] {{");
						writer.WriteLineNoIndent("#else");
						writer.WriteLine($"internal static readonly byte[] SizesBcst = new byte[{icedConstants.Name(idConverter)}.{icedConstants[IcedConstants.GetEnumCountName(TypeIds.Code)].Name(idConverter)}] {{");
						writer.WriteLineNoIndent("#endif");
						using (writer.Indent()) {
							foreach (var def in defs) {
								if (def.MemoryBroadcast.Value > byte.MaxValue)
									throw new InvalidOperationException();
								string value;
								if (def.MemoryBroadcast.Value == 0)
									value = "0";
								else
									value = $"(byte){idConverter.ToDeclTypeAndValue(def.MemoryBroadcast)}";
								writer.WriteLine($"{value},// {def.Code.Name(idConverter)}");
							}
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
