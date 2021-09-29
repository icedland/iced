// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using Generator.Documentation;
using Generator.Documentation.CSharp;
using Generator.IO;

namespace Generator.Constants.CSharp {
	[Generator(TargetLanguage.CSharp)]
	sealed class CSharpConstantsGenerator : ConstantsGenerator {
		readonly IdentifierConverter idConverter;
		readonly Dictionary<TypeId, FullConstantsFileInfo> toFullFileInfo;
		readonly Dictionary<TypeId, PartialConstantsFileInfo?> toPartialFileInfo;
		readonly CSharpDocCommentWriter docWriter;
		readonly DeprecatedWriter deprecatedWriter;

		sealed class FullConstantsFileInfo {
			public readonly string Filename;
			public readonly string Namespace;
			public readonly string? Define;
			public readonly bool PartialClass;

			public FullConstantsFileInfo(string filename, string @namespace, string? define = null, bool partialClass = false) {
				Filename = filename;
				Namespace = @namespace;
				Define = define;
				PartialClass = partialClass;
			}
		}

		sealed class PartialConstantsFileInfo {
			public readonly string Id;
			public readonly string Filename;
			public readonly bool PartialClass;

			public PartialConstantsFileInfo(string id, string filename, bool partialClass = false) {
				Id = id;
				Filename = filename;
				PartialClass = partialClass;
			}
		}

		public CSharpConstantsGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			idConverter = CSharpIdentifierConverter.Create();
			docWriter = new CSharpDocCommentWriter(idConverter);
			deprecatedWriter = new CSharpDeprecatedWriter(idConverter);

			var dirs = genTypes.Dirs;
			toFullFileInfo = new Dictionary<TypeId, FullConstantsFileInfo>();
			toFullFileInfo.Add(TypeIds.IcedConstants, new FullConstantsFileInfo(CSharpConstants.GetFilename(genTypes, CSharpConstants.IcedNamespace, nameof(TypeIds.IcedConstants) + ".g.cs"), CSharpConstants.IcedNamespace, partialClass: true));
			toFullFileInfo.Add(TypeIds.DecoderConstants, new FullConstantsFileInfo(dirs.GetCSharpTestFilename("Intel", nameof(TypeIds.DecoderConstants) + ".g.cs"), CSharpConstants.IcedUnitTestsNamespace));

			toPartialFileInfo = new Dictionary<TypeId, PartialConstantsFileInfo?>();
			toPartialFileInfo.Add(TypeIds.DecoderTestParserConstants, new PartialConstantsFileInfo("DecoderTestText", dirs.GetCSharpTestFilename("Intel", "DecoderTests", "DecoderTestParser.cs")));
			toPartialFileInfo.Add(TypeIds.InstrInfoConstants, new PartialConstantsFileInfo("InstrInfoConstants", CSharpConstants.GetFilename(genTypes, CSharpConstants.InstructionInfoNamespace, "InfoHandlerFlags.cs")));
			toPartialFileInfo.Add(TypeIds.MiscInstrInfoTestConstants, new PartialConstantsFileInfo("MiscConstants", dirs.GetCSharpTestFilename("Intel", "InstructionInfoTests", "InstructionInfoConstants.cs")));
			toPartialFileInfo.Add(TypeIds.InstructionInfoKeys, new PartialConstantsFileInfo("KeysConstants", dirs.GetCSharpTestFilename("Intel", "InstructionInfoTests", "InstructionInfoConstants.cs")));
			toPartialFileInfo.Add(TypeIds.RflagsBitsConstants, new PartialConstantsFileInfo("RflagsBitsConstants", dirs.GetCSharpTestFilename("Intel", "InstructionInfoTests", "InstructionInfoConstants.cs")));
			toPartialFileInfo.Add(TypeIds.MiscSectionNames, new PartialConstantsFileInfo("MiscSectionNames", dirs.GetCSharpTestFilename("Intel", "InstructionInfoTests", "MiscTestsData.cs")));
			toPartialFileInfo.Add(TypeIds.OpCodeInfoKeys, new PartialConstantsFileInfo("OpCodeInfoKeys", dirs.GetCSharpTestFilename("Intel", "EncoderTests", "OpCodeInfoConstants.cs")));
			toPartialFileInfo.Add(TypeIds.OpCodeInfoFlags, new PartialConstantsFileInfo("OpCodeInfoFlags", dirs.GetCSharpTestFilename("Intel", "EncoderTests", "OpCodeInfoConstants.cs")));
		}

		public override void Generate(ConstantsType constantsType) {
			if (toFullFileInfo.TryGetValue(constantsType.TypeId, out var fullFileInfo))
				WriteFile(fullFileInfo, constantsType);
			else if (toPartialFileInfo.TryGetValue(constantsType.TypeId, out var partialInfo)) {
				if (partialInfo is not null)
					new FileUpdater(TargetLanguage.CSharp, partialInfo.Id, partialInfo.Filename).Generate(writer => WriteConstants(writer, constantsType, isPartialClass: partialInfo.PartialClass));
			}
			else
				throw new InvalidOperationException();
		}

		void WriteFile(FullConstantsFileInfo info, ConstantsType constantsType) {
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(info.Filename))) {
				writer.WriteFileHeader();
				if (info.Define is not null)
					writer.WriteLineNoIndent($"#if {info.Define}");

				writer.WriteLine($"namespace {info.Namespace} {{");

				if (constantsType.IsPublic && constantsType.IsMissingDocs)
					writer.WriteLine(CSharpConstants.PragmaMissingDocsDisable);
				using (writer.Indent())
					WriteConstants(writer, constantsType, isPartialClass: info.PartialClass);
				writer.WriteLine("}");

				if (info.Define is not null)
					writer.WriteLineNoIndent("#endif");
			}
		}

		void WriteConstants(FileWriter writer, ConstantsType constantsType, bool isPartialClass) {
			docWriter.WriteSummary(writer, constantsType.Documentation.GetComment(TargetLanguage.CSharp), constantsType.RawName);
			var pub = constantsType.IsPublic ? "public " : string.Empty;
			var partial = isPartialClass ? " partial" : string.Empty;
			writer.WriteLine($"{pub}static{partial} class {constantsType.Name(idConverter)} {{");

			using (writer.Indent()) {
				foreach (var constant in constantsType.Constants) {
					docWriter.WriteSummary(writer, constant.Documentation.GetComment(TargetLanguage.CSharp), constantsType.RawName);
					deprecatedWriter.WriteDeprecated(writer, constant);
					writer.Write(constant.IsPublic ? "public " : "internal ");
					writer.Write("const ");
					writer.Write(GetType(constant.Kind));
					writer.Write(" ");
					writer.Write(constant.Name(idConverter));
					writer.Write(" = ");
					writer.Write(GetValue(constant));
					writer.WriteLine(";");
				}
			}

			writer.WriteLine("}");
		}

		string GetType(ConstantKind kind) =>
			kind switch {
				ConstantKind.Char => "char",
				ConstantKind.String => "string",
				ConstantKind.Int32 or ConstantKind.Index => "int",
				ConstantKind.UInt32 => "uint",
				ConstantKind.UInt64 => "ulong",
				ConstantKind.Register or ConstantKind.MemorySize => EnumUtils.GetEnumType(genTypes, kind).Name(idConverter),
				_ => throw new InvalidOperationException(),
			};

		string GetValue(Constant constant) {
			switch (constant.Kind) {
			case ConstantKind.Char:
				var c = (char)constant.ValueUInt64;
				return "'" + c.ToString() + "'";

			case ConstantKind.String:
				if (constant.RefValue is string s)
					return "\"" + EscapeStringValue(s) + "\"";
				throw new InvalidOperationException();

			case ConstantKind.Int32:
			case ConstantKind.Index:
				if (constant.UseHex)
					return NumberFormatter.FormatHexUInt32WithSep((uint)constant.ValueUInt64);
				return ((int)constant.ValueUInt64).ToString();

			case ConstantKind.UInt32:
				if (constant.UseHex)
					return NumberFormatter.FormatHexUInt32WithSep((uint)constant.ValueUInt64);
				return ((uint)constant.ValueUInt64).ToString();

			case ConstantKind.UInt64:
				if (constant.UseHex)
					return NumberFormatter.FormatHexUInt64WithSep(constant.ValueUInt64);
				return constant.ValueUInt64.ToString();

			case ConstantKind.Register:
			case ConstantKind.MemorySize:
				return GetValueString(constant);

			default:
				throw new InvalidOperationException();
			}
		}

		static string EscapeStringValue(string s) => s;

		string GetValueString(Constant constant) {
			var enumType = EnumUtils.GetEnumType(genTypes, constant.Kind);
			var enumValue = enumType.Values.First(a => a.Value == constant.ValueUInt64);
			return idConverter.ToDeclTypeAndValue(enumValue);
		}
	}
}
