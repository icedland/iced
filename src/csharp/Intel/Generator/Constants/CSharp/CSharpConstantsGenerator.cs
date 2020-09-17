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

			public FullConstantsFileInfo(string filename, string @namespace, string? define = null) {
				Filename = filename;
				Namespace = @namespace;
				Define = define;
			}
		}

		sealed class PartialConstantsFileInfo {
			public readonly string Id;
			public readonly string Filename;

			public PartialConstantsFileInfo(string id, string filename) {
				Id = id;
				Filename = filename;
			}
		}

		public CSharpConstantsGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			idConverter = CSharpIdentifierConverter.Create();
			docWriter = new CSharpDocCommentWriter(idConverter);
			deprecatedWriter = new CSharpDeprecatedWriter(idConverter);

			var baseDir = CSharpConstants.GetDirectory(generatorContext, CSharpConstants.IcedNamespace);
			toFullFileInfo = new Dictionary<TypeId, FullConstantsFileInfo>();
			toFullFileInfo.Add(TypeIds.IcedConstants, new FullConstantsFileInfo(Path.Combine(baseDir, nameof(TypeIds.IcedConstants) + ".g.cs"), CSharpConstants.IcedNamespace));
			toFullFileInfo.Add(TypeIds.DecoderConstants, new FullConstantsFileInfo(Path.Combine(generatorContext.CSharpTestsDir, "Intel", nameof(TypeIds.DecoderConstants) + ".g.cs"), CSharpConstants.IcedUnitTestsNamespace));

			toPartialFileInfo = new Dictionary<TypeId, PartialConstantsFileInfo?>();
			toPartialFileInfo.Add(TypeIds.DecoderTestParserConstants, new PartialConstantsFileInfo("DecoderTestText", Path.Combine(generatorContext.CSharpTestsDir, "Intel", "DecoderTests", "DecoderTestParser.cs")));
			toPartialFileInfo.Add(TypeIds.InstrInfoConstants, new PartialConstantsFileInfo("InstrInfoConstants", Path.Combine(CSharpConstants.GetDirectory(generatorContext, CSharpConstants.InstructionInfoNamespace), "InfoHandlerFlags.cs")));
			toPartialFileInfo.Add(TypeIds.MiscInstrInfoTestConstants, new PartialConstantsFileInfo("MiscConstants", Path.Combine(generatorContext.CSharpTestsDir, "Intel", "InstructionInfoTests", "InstructionInfoConstants.cs")));
			toPartialFileInfo.Add(TypeIds.InstructionInfoKeys, new PartialConstantsFileInfo("KeysConstants", Path.Combine(generatorContext.CSharpTestsDir, "Intel", "InstructionInfoTests", "InstructionInfoConstants.cs")));
			toPartialFileInfo.Add(TypeIds.RflagsBitsConstants, new PartialConstantsFileInfo("RflagsBitsConstants", Path.Combine(generatorContext.CSharpTestsDir, "Intel", "InstructionInfoTests", "InstructionInfoConstants.cs")));
			toPartialFileInfo.Add(TypeIds.MiscSectionNames, new PartialConstantsFileInfo("MiscSectionNames", Path.Combine(generatorContext.CSharpTestsDir, "Intel", "InstructionInfoTests", "MiscTestsData.cs")));
			toPartialFileInfo.Add(TypeIds.OpCodeInfoKeys, new PartialConstantsFileInfo("OpCodeInfoKeys", Path.Combine(generatorContext.CSharpTestsDir, "Intel", "EncoderTests", "OpCodeInfoConstants.cs")));
			toPartialFileInfo.Add(TypeIds.OpCodeInfoFlags, new PartialConstantsFileInfo("OpCodeInfoFlags", Path.Combine(generatorContext.CSharpTestsDir, "Intel", "EncoderTests", "OpCodeInfoConstants.cs")));
		}

		public override void Generate(ConstantsType constantsType) {
			if (toFullFileInfo.TryGetValue(constantsType.TypeId, out var fullFileInfo))
				WriteFile(fullFileInfo, constantsType);
			else if (toPartialFileInfo.TryGetValue(constantsType.TypeId, out var partialInfo)) {
				if (!(partialInfo is null))
					new FileUpdater(TargetLanguage.CSharp, partialInfo.Id, partialInfo.Filename).Generate(writer => WriteConstants(writer, constantsType));
			}
			else
				throw new InvalidOperationException();
		}

		void WriteFile(FullConstantsFileInfo info, ConstantsType constantsType) {
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(info.Filename))) {
				writer.WriteFileHeader();
				if (!(info.Define is null))
					writer.WriteLineNoIndent($"#if {info.Define}");

				writer.WriteLine($"namespace {info.Namespace} {{");

				if (constantsType.IsPublic && constantsType.IsMissingDocs)
					writer.WriteLine(CSharpConstants.PragmaMissingDocsDisable);
				using (writer.Indent())
					WriteConstants(writer, constantsType);
				writer.WriteLine("}");

				if (!(info.Define is null))
					writer.WriteLineNoIndent("#endif");
			}
		}

		void WriteConstants(FileWriter writer, ConstantsType constantsType) {
			docWriter.WriteSummary(writer, constantsType.Documentation, constantsType.RawName);
			var pub = constantsType.IsPublic ? "public " : string.Empty;
			writer.WriteLine($"{pub}static class {constantsType.Name(idConverter)} {{");

			using (writer.Indent()) {
				foreach (var constant in constantsType.Constants) {
					docWriter.WriteSummary(writer, constant.Documentation, constantsType.RawName);
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

		string GetType(ConstantKind kind) {
			switch (kind) {
			case ConstantKind.Char:
				return "char";
			case ConstantKind.String:
				return "string";
			case ConstantKind.Int32:
			case ConstantKind.Index:
				return "int";
			case ConstantKind.UInt32:
				return "uint";
			case ConstantKind.UInt64:
				return "ulong";
			case ConstantKind.Register:
			case ConstantKind.MemorySize:
				return EnumUtils.GetEnumType(genTypes, kind).Name(idConverter);
			default:
				throw new InvalidOperationException();
			}
		}

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
			return $"{enumType.Name(idConverter)}.{enumValue.Name(idConverter)}";
		}
	}
}
