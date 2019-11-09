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
using System.Collections.Generic;
using System.IO;
using Generator.Documentation.CSharp;
using Generator.IO;

namespace Generator.Enums.CSharp {
	sealed class CSharpEnumsGenerator : IEnumsGenerator {
		readonly IdentifierConverter idConverter;
		readonly Dictionary<TypeId, FullEnumFileInfo> toFullFileInfo;
		readonly Dictionary<TypeId, PartialEnumFileInfo> toPartialFileInfo;
		readonly CSharpDocCommentWriter docWriter;

		sealed class FullEnumFileInfo {
			public readonly string Filename;
			public readonly string Namespace;
			public readonly string? Define;
			public readonly string? BaseType;

			public FullEnumFileInfo(string filename, string @namespace, string? define = null, string? baseType = null) {
				Filename = filename;
				Namespace = @namespace;
				Define = define;
				BaseType = baseType;
			}
		}

		sealed class PartialEnumFileInfo {
			public readonly string Id;
			public readonly string Filename;
			public readonly string? BaseType;

			public PartialEnumFileInfo(string id, string filename, string? baseType) {
				Id = id;
				Filename = filename;
				BaseType = baseType;
			}
		}

		public CSharpEnumsGenerator(ProjectDirs projectDirs) {
			idConverter = CSharpIdentifierConverter.Create();
			docWriter = new CSharpDocCommentWriter(idConverter);

			toFullFileInfo = new Dictionary<TypeId, FullEnumFileInfo>();
			toFullFileInfo.Add(TypeIds.Code, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), nameof(TypeIds.Code) + ".g.cs"), CSharpConstants.IcedNamespace));
			toFullFileInfo.Add(TypeIds.CodeSize, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), nameof(TypeIds.CodeSize) + ".g.cs"), CSharpConstants.IcedNamespace));
			toFullFileInfo.Add(TypeIds.CpuidFeature, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), nameof(TypeIds.CpuidFeature) + ".g.cs"), CSharpConstants.IcedNamespace, CSharpConstants.InstructionInfoDefine));
			toFullFileInfo.Add(TypeIds.CpuidFeatureInternal, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.InstructionInfoNamespace), nameof(TypeIds.CpuidFeatureInternal) + ".g.cs"), CSharpConstants.InstructionInfoNamespace, CSharpConstants.InstructionInfoDefine));
			toFullFileInfo.Add(TypeIds.DecoderOptions, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), nameof(TypeIds.DecoderOptions) + ".g.cs"), CSharpConstants.IcedNamespace, CSharpConstants.DecoderDefine, baseType: "uint"));
			toFullFileInfo.Add(TypeIds.EvexOpCodeHandlerKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.DecoderNamespace), nameof(TypeIds.EvexOpCodeHandlerKind) + ".g.cs"), CSharpConstants.DecoderNamespace, CSharpConstants.DecoderDefine, baseType: "byte"));
			toFullFileInfo.Add(TypeIds.HandlerFlags, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.DecoderNamespace), nameof(TypeIds.HandlerFlags) + ".g.cs"), CSharpConstants.DecoderNamespace, CSharpConstants.DecoderDefine, baseType: "uint"));
			toFullFileInfo.Add(TypeIds.LegacyHandlerFlags, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.DecoderNamespace), nameof(TypeIds.LegacyHandlerFlags) + ".g.cs"), CSharpConstants.DecoderNamespace, CSharpConstants.DecoderDefine, baseType: "uint"));
			toFullFileInfo.Add(TypeIds.MemorySize, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), nameof(TypeIds.MemorySize) + ".g.cs"), CSharpConstants.IcedNamespace));
			toFullFileInfo.Add(TypeIds.OpCodeHandlerKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.DecoderNamespace), nameof(TypeIds.OpCodeHandlerKind) + ".g.cs"), CSharpConstants.DecoderNamespace, CSharpConstants.DecoderDefine, baseType: "byte"));
			toFullFileInfo.Add(TypeIds.PseudoOpsKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.FormatterNamespace), nameof(TypeIds.PseudoOpsKind) + ".g.cs"), CSharpConstants.FormatterNamespace, CSharpConstants.AnyFormatterDefine));
			toFullFileInfo.Add(TypeIds.Register, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), nameof(TypeIds.Register) + ".g.cs"), CSharpConstants.IcedNamespace));
			toFullFileInfo.Add(TypeIds.SerializedDataKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.DecoderNamespace), nameof(TypeIds.SerializedDataKind) + ".g.cs"), CSharpConstants.DecoderNamespace, CSharpConstants.DecoderDefine, baseType: "byte"));
			toFullFileInfo.Add(TypeIds.TupleType, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), nameof(TypeIds.TupleType) + ".g.cs"), CSharpConstants.IcedNamespace, CSharpConstants.DecoderOrEncoderDefine));
			toFullFileInfo.Add(TypeIds.VexOpCodeHandlerKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.DecoderNamespace), nameof(TypeIds.VexOpCodeHandlerKind) + ".g.cs"), CSharpConstants.DecoderNamespace, CSharpConstants.DecoderDefine, baseType: "byte"));
			toFullFileInfo.Add(TypeIds.Mnemonic, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), nameof(TypeIds.Mnemonic) + ".g.cs"), CSharpConstants.IcedNamespace));
			toFullFileInfo.Add(TypeIds.GasCtorKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.GasFormatterNamespace), "CtorKind.g.cs"), CSharpConstants.GasFormatterNamespace, CSharpConstants.GasFormatterDefine));
			toFullFileInfo.Add(TypeIds.IntelCtorKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IntelFormatterNamespace), "CtorKind.g.cs"), CSharpConstants.IntelFormatterNamespace, CSharpConstants.IntelFormatterDefine));
			toFullFileInfo.Add(TypeIds.MasmCtorKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.MasmFormatterNamespace), "CtorKind.g.cs"), CSharpConstants.MasmFormatterNamespace, CSharpConstants.MasmFormatterDefine));
			toFullFileInfo.Add(TypeIds.NasmCtorKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.NasmFormatterNamespace), "CtorKind.g.cs"), CSharpConstants.NasmFormatterNamespace, CSharpConstants.NasmFormatterDefine));

			toFullFileInfo.Add(TypeIds.GasSizeOverride, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.GasFormatterNamespace), "SizeOverride.g.cs"), CSharpConstants.GasFormatterNamespace, CSharpConstants.GasFormatterDefine));
			toFullFileInfo.Add(TypeIds.GasInstrOpInfoFlags, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.GasFormatterNamespace), "InstrOpInfoFlags.g.cs"), CSharpConstants.GasFormatterNamespace, CSharpConstants.GasFormatterDefine, "ushort"));

			toFullFileInfo.Add(TypeIds.IntelSizeOverride, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IntelFormatterNamespace), "SizeOverride.g.cs"), CSharpConstants.IntelFormatterNamespace, CSharpConstants.IntelFormatterDefine));
			toFullFileInfo.Add(TypeIds.IntelBranchSizeInfo, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IntelFormatterNamespace), "BranchSizeInfo.g.cs"), CSharpConstants.IntelFormatterNamespace, CSharpConstants.IntelFormatterDefine));
			toFullFileInfo.Add(TypeIds.IntelInstrOpInfoFlags, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IntelFormatterNamespace), "InstrOpInfoFlags.g.cs"), CSharpConstants.IntelFormatterNamespace, CSharpConstants.IntelFormatterDefine, "ushort"));

			toFullFileInfo.Add(TypeIds.MasmInstrOpInfoFlags, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.MasmFormatterNamespace), "InstrOpInfoFlags.g.cs"), CSharpConstants.MasmFormatterNamespace, CSharpConstants.MasmFormatterDefine, "ushort"));

			toFullFileInfo.Add(TypeIds.NasmSignExtendInfo, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.NasmFormatterNamespace), "SignExtendInfo.g.cs"), CSharpConstants.NasmFormatterNamespace, CSharpConstants.NasmFormatterDefine));
			toFullFileInfo.Add(TypeIds.NasmSizeOverride, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.NasmFormatterNamespace), "SizeOverride.g.cs"), CSharpConstants.NasmFormatterNamespace, CSharpConstants.NasmFormatterDefine));
			toFullFileInfo.Add(TypeIds.NasmBranchSizeInfo, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.NasmFormatterNamespace), "BranchSizeInfo.g.cs"), CSharpConstants.NasmFormatterNamespace, CSharpConstants.NasmFormatterDefine));
			toFullFileInfo.Add(TypeIds.NasmInstrOpInfoFlags, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.NasmFormatterNamespace), "InstrOpInfoFlags.g.cs"), CSharpConstants.NasmFormatterNamespace, CSharpConstants.NasmFormatterDefine, "uint"));

			toFullFileInfo.Add(TypeIds.RoundingControl, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), nameof(TypeIds.RoundingControl) + ".g.cs"), CSharpConstants.IcedNamespace));
			toFullFileInfo.Add(TypeIds.OpKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), nameof(TypeIds.OpKind) + ".g.cs"), CSharpConstants.IcedNamespace));

			toPartialFileInfo = new Dictionary<TypeId, PartialEnumFileInfo>();
			toPartialFileInfo.Add(TypeIds.Instruction_MemoryFlags, new PartialEnumFileInfo("MemoryFlags", Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), "Instruction.cs"), "ushort"));
			toPartialFileInfo.Add(TypeIds.Instruction_OpKindFlags, new PartialEnumFileInfo("OpKindFlags", Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), "Instruction.cs"), "uint"));
			toPartialFileInfo.Add(TypeIds.Instruction_CodeFlags, new PartialEnumFileInfo("CodeFlags", Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), "Instruction.cs"), "uint"));
		}

		public void Generate(EnumType enumType) {
			if (toFullFileInfo.TryGetValue(enumType.TypeId, out var fullFileInfo))
				WriteFile(fullFileInfo, enumType);
			else if (toPartialFileInfo.TryGetValue(enumType.TypeId, out var partialInfo)) {
				if (!(partialInfo is null))
					new FileUpdater(TargetLanguage.CSharp, partialInfo.Id, partialInfo.Filename).Generate(writer => WriteEnum(writer, partialInfo, enumType));
			}
			else
				throw new InvalidOperationException();
		}

		void WriteEnum(FileWriter writer, EnumType enumType, string? baseType) {
			docWriter.Write(writer, enumType.Documentation, enumType.RawName);
			if (enumType.IsFlags)
				writer.WriteLine("[Flags]");
			var pub = enumType.IsPublic ? "public " : string.Empty;
			var theBaseType = !(baseType is null) ? $" : {baseType}" : string.Empty;
			writer.WriteLine($"{pub}enum {enumType.Name(idConverter)}{theBaseType} {{");

			writer.Indent();
			uint expectedValue = 0;
			foreach (var value in enumType.Values) {
				docWriter.Write(writer, value.Documentation, enumType.RawName);
				if (enumType.IsFlags)
					writer.WriteLine($"{value.Name(idConverter)} = 0x{value.Value:X8},");
				else if (expectedValue != value.Value)
					writer.WriteLine($"{value.Name(idConverter)} = {value.Value},");
				else
					writer.WriteLine($"{value.Name(idConverter)},");
				expectedValue = value.Value + 1;
			}
			writer.Unindent();

			writer.WriteLine("}");
		}

		void WriteFile(FullEnumFileInfo info, EnumType enumType) {
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(info.Filename))) {
				writer.WriteFileHeader();
				if (!(info.Define is null))
					writer.WriteLine($"#if {info.Define}");

				if (enumType.IsFlags) {
					writer.WriteLine("using System;");
					writer.WriteLine();
				}

				writer.WriteLine($"namespace {info.Namespace} {{");

				if (enumType.IsPublic && enumType.IsMissingDocs)
					writer.WriteLine("#pragma warning disable 1591 // Missing XML comment for publicly visible type or member");
				writer.Indent();
				WriteEnum(writer, enumType, info.BaseType);
				writer.Unindent();

				writer.WriteLine("}");
				if (!(info.Define is null))
					writer.WriteLine("#endif");
			}
		}

		void WriteEnum(FileWriter writer, PartialEnumFileInfo partialInfo, EnumType enumType) =>
			WriteEnum(writer, enumType, partialInfo.BaseType);
	}
}
