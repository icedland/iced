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
		readonly Dictionary<EnumKind, FullEnumFileInfo> toFullFileInfo;
		readonly Dictionary<EnumKind, PartialEnumFileInfo> toPartialFileInfo;
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

			toFullFileInfo = new Dictionary<EnumKind, FullEnumFileInfo>();
			toFullFileInfo.Add(EnumKind.Code, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), nameof(EnumKind.Code) + ".g.cs"), CSharpConstants.IcedNamespace));
			toFullFileInfo.Add(EnumKind.CodeSize, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), nameof(EnumKind.CodeSize) + ".g.cs"), CSharpConstants.IcedNamespace));
			toFullFileInfo.Add(EnumKind.CpuidFeature, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), nameof(EnumKind.CpuidFeature) + ".g.cs"), CSharpConstants.IcedNamespace, CSharpConstants.InstructionInfoDefine));
			toFullFileInfo.Add(EnumKind.CpuidFeatureInternal, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.InstructionInfoNamespace), nameof(EnumKind.CpuidFeatureInternal) + ".g.cs"), CSharpConstants.InstructionInfoNamespace, CSharpConstants.InstructionInfoDefine));
			toFullFileInfo.Add(EnumKind.DecoderOptions, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), nameof(EnumKind.DecoderOptions) + ".g.cs"), CSharpConstants.IcedNamespace, CSharpConstants.DecoderDefine, baseType: "uint"));
			toFullFileInfo.Add(EnumKind.EvexOpCodeHandlerKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.DecoderNamespace), nameof(EnumKind.EvexOpCodeHandlerKind) + ".g.cs"), CSharpConstants.DecoderNamespace, CSharpConstants.DecoderDefine, baseType: "byte"));
			toFullFileInfo.Add(EnumKind.HandlerFlags, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.DecoderNamespace), nameof(EnumKind.HandlerFlags) + ".g.cs"), CSharpConstants.DecoderNamespace, CSharpConstants.DecoderDefine, baseType: "uint"));
			toFullFileInfo.Add(EnumKind.LegacyHandlerFlags, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.DecoderNamespace), nameof(EnumKind.LegacyHandlerFlags) + ".g.cs"), CSharpConstants.DecoderNamespace, CSharpConstants.DecoderDefine, baseType: "uint"));
			toFullFileInfo.Add(EnumKind.MemorySize, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), nameof(EnumKind.MemorySize) + ".g.cs"), CSharpConstants.IcedNamespace));
			toFullFileInfo.Add(EnumKind.OpCodeHandlerKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.DecoderNamespace), nameof(EnumKind.OpCodeHandlerKind) + ".g.cs"), CSharpConstants.DecoderNamespace, CSharpConstants.DecoderDefine, baseType: "byte"));
			toFullFileInfo.Add(EnumKind.PseudoOpsKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.FormatterNamespace), nameof(EnumKind.PseudoOpsKind) + ".g.cs"), CSharpConstants.FormatterNamespace, CSharpConstants.AnyFormatterDefine));
			toFullFileInfo.Add(EnumKind.Register, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), nameof(EnumKind.Register) + ".g.cs"), CSharpConstants.IcedNamespace));
			toFullFileInfo.Add(EnumKind.SerializedDataKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.DecoderNamespace), nameof(EnumKind.SerializedDataKind) + ".g.cs"), CSharpConstants.DecoderNamespace, CSharpConstants.DecoderDefine, baseType: "byte"));
			toFullFileInfo.Add(EnumKind.TupleType, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), nameof(EnumKind.TupleType) + ".g.cs"), CSharpConstants.IcedNamespace, CSharpConstants.DecoderOrEncoderDefine));
			toFullFileInfo.Add(EnumKind.VexOpCodeHandlerKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.DecoderNamespace), nameof(EnumKind.VexOpCodeHandlerKind) + ".g.cs"), CSharpConstants.DecoderNamespace, CSharpConstants.DecoderDefine, baseType: "byte"));
			toFullFileInfo.Add(EnumKind.Mnemonic, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), nameof(EnumKind.Mnemonic) + ".g.cs"), CSharpConstants.IcedNamespace));
			toFullFileInfo.Add(EnumKind.GasCtorKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.GasFormatterNamespace), "CtorKind.g.cs"), CSharpConstants.GasFormatterNamespace, CSharpConstants.GasFormatterDefine));
			toFullFileInfo.Add(EnumKind.IntelCtorKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IntelFormatterNamespace), "CtorKind.g.cs"), CSharpConstants.IntelFormatterNamespace, CSharpConstants.IntelFormatterDefine));
			toFullFileInfo.Add(EnumKind.MasmCtorKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.MasmFormatterNamespace), "CtorKind.g.cs"), CSharpConstants.MasmFormatterNamespace, CSharpConstants.MasmFormatterDefine));
			toFullFileInfo.Add(EnumKind.NasmCtorKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.NasmFormatterNamespace), "CtorKind.g.cs"), CSharpConstants.NasmFormatterNamespace, CSharpConstants.NasmFormatterDefine));

			toFullFileInfo.Add(EnumKind.GasSizeOverride, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.GasFormatterNamespace), "SizeOverride.g.cs"), CSharpConstants.GasFormatterNamespace, CSharpConstants.GasFormatterDefine));
			toFullFileInfo.Add(EnumKind.GasInstrOpInfoFlags, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.GasFormatterNamespace), "InstrOpInfoFlags.g.cs"), CSharpConstants.GasFormatterNamespace, CSharpConstants.GasFormatterDefine, "ushort"));

			toFullFileInfo.Add(EnumKind.IntelSizeOverride, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IntelFormatterNamespace), "SizeOverride.g.cs"), CSharpConstants.IntelFormatterNamespace, CSharpConstants.IntelFormatterDefine));
			toFullFileInfo.Add(EnumKind.IntelBranchSizeInfo, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IntelFormatterNamespace), "BranchSizeInfo.g.cs"), CSharpConstants.IntelFormatterNamespace, CSharpConstants.IntelFormatterDefine));
			toFullFileInfo.Add(EnumKind.IntelInstrOpInfoFlags, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IntelFormatterNamespace), "InstrOpInfoFlags.g.cs"), CSharpConstants.IntelFormatterNamespace, CSharpConstants.IntelFormatterDefine, "ushort"));

			toFullFileInfo.Add(EnumKind.MasmInstrOpInfoFlags, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.MasmFormatterNamespace), "InstrOpInfoFlags.g.cs"), CSharpConstants.MasmFormatterNamespace, CSharpConstants.MasmFormatterDefine, "ushort"));

			toFullFileInfo.Add(EnumKind.NasmSignExtendInfo, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.NasmFormatterNamespace), "SignExtendInfo.g.cs"), CSharpConstants.NasmFormatterNamespace, CSharpConstants.NasmFormatterDefine));
			toFullFileInfo.Add(EnumKind.NasmSizeOverride, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.NasmFormatterNamespace), "SizeOverride.g.cs"), CSharpConstants.NasmFormatterNamespace, CSharpConstants.NasmFormatterDefine));
			toFullFileInfo.Add(EnumKind.NasmBranchSizeInfo, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.NasmFormatterNamespace), "BranchSizeInfo.g.cs"), CSharpConstants.NasmFormatterNamespace, CSharpConstants.NasmFormatterDefine));
			toFullFileInfo.Add(EnumKind.NasmInstrOpInfoFlags, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.NasmFormatterNamespace), "InstrOpInfoFlags.g.cs"), CSharpConstants.NasmFormatterNamespace, CSharpConstants.NasmFormatterDefine, "uint"));

			toFullFileInfo.Add(EnumKind.RoundingControl, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), nameof(EnumKind.RoundingControl) + ".g.cs"), CSharpConstants.IcedNamespace));
			toFullFileInfo.Add(EnumKind.OpKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), nameof(EnumKind.OpKind) + ".g.cs"), CSharpConstants.IcedNamespace));

			toPartialFileInfo = new Dictionary<EnumKind, PartialEnumFileInfo>();
			toPartialFileInfo.Add(EnumKind.Instruction_MemoryFlags, new PartialEnumFileInfo("MemoryFlags", Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), "Instruction.cs"), "ushort"));
			toPartialFileInfo.Add(EnumKind.Instruction_OpKindFlags, new PartialEnumFileInfo("OpKindFlags", Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), "Instruction.cs"), "uint"));
			toPartialFileInfo.Add(EnumKind.Instruction_CodeFlags, new PartialEnumFileInfo("CodeFlags", Path.Combine(CSharpConstants.GetDirectory(projectDirs, CSharpConstants.IcedNamespace), "Instruction.cs"), "uint"));

			if ((toFullFileInfo.Count + toPartialFileInfo.Count) != Enum.GetValues(typeof(EnumKind)).Length)
				throw new InvalidOperationException();
		}

		public void Generate(EnumType enumType) {
			if (toFullFileInfo.TryGetValue(enumType.EnumKind, out var fullFileInfo))
				WriteFile(fullFileInfo, enumType);
			else if (toPartialFileInfo.TryGetValue(enumType.EnumKind, out var partialInfo)) {
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

		void WriteEnum(FileWriter writer, PartialEnumFileInfo partialInfo, EnumType enumType) {
			writer.Indent(2);
			WriteEnum(writer, enumType, partialInfo.BaseType);
			writer.Unindent(2);
		}
	}
}
