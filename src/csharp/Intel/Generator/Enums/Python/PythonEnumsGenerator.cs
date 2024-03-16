// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Generator.Documentation.Python;
using Generator.IO;
using Generator.Misc.Python;

namespace Generator.Enums.Python {
	[Generator(TargetLanguage.Python)]
	sealed class PythonEnumsGenerator : EnumsGenerator {
		readonly IdentifierConverter pythonIdConverter;
		readonly IdentifierConverter rustIdConverter;
		readonly Dictionary<TypeId, FullEnumFileInfo?> toFullFileInfo;
		readonly Dictionary<TypeId, PartialEnumFileInfo?> toPartialFileInfo;
		readonly Documentation.Rust.RustDocCommentWriter rustDocWriter;
		readonly ExportedPythonTypes exportedPythonTypes;

		sealed class FullEnumFileInfo {
			public readonly string Filename;

			public FullEnumFileInfo(string filename) => Filename = filename;
		}

		sealed class PartialEnumFileInfo {
			public readonly string Id;
			public readonly TargetLanguage Language;
			public readonly string Filename;
			public readonly string[] Attributes;

			public PartialEnumFileInfo(string id, TargetLanguage language, string filename, params string[] attributes) {
				Id = id;
				Language = language;
				Filename = filename;
				Attributes = attributes;
			}
		}

		public PythonEnumsGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			pythonIdConverter = PythonIdentifierConverter.Create();
			rustIdConverter = RustIdentifierConverter.Create();
			rustDocWriter = new Documentation.Rust.RustDocCommentWriter(rustIdConverter, ".", ".", ".", ".");
			exportedPythonTypes = genTypes.GetObject<ExportedPythonTypes>(TypeIds.ExportedPythonTypes);

			var dirs = generatorContext.Types.Dirs;
			toFullFileInfo = new();
			toFullFileInfo.Add(TypeIds.CC_a, new FullEnumFileInfo(dirs.GetPythonPyFilename("CC_a.py")));
			toFullFileInfo.Add(TypeIds.CC_ae, new FullEnumFileInfo(dirs.GetPythonPyFilename("CC_ae.py")));
			toFullFileInfo.Add(TypeIds.CC_b, new FullEnumFileInfo(dirs.GetPythonPyFilename("CC_b.py")));
			toFullFileInfo.Add(TypeIds.CC_be, new FullEnumFileInfo(dirs.GetPythonPyFilename("CC_be.py")));
			toFullFileInfo.Add(TypeIds.CC_e, new FullEnumFileInfo(dirs.GetPythonPyFilename("CC_e.py")));
			toFullFileInfo.Add(TypeIds.CC_g, new FullEnumFileInfo(dirs.GetPythonPyFilename("CC_g.py")));
			toFullFileInfo.Add(TypeIds.CC_ge, new FullEnumFileInfo(dirs.GetPythonPyFilename("CC_ge.py")));
			toFullFileInfo.Add(TypeIds.CC_l, new FullEnumFileInfo(dirs.GetPythonPyFilename("CC_l.py")));
			toFullFileInfo.Add(TypeIds.CC_le, new FullEnumFileInfo(dirs.GetPythonPyFilename("CC_le.py")));
			toFullFileInfo.Add(TypeIds.CC_ne, new FullEnumFileInfo(dirs.GetPythonPyFilename("CC_ne.py")));
			toFullFileInfo.Add(TypeIds.CC_np, new FullEnumFileInfo(dirs.GetPythonPyFilename("CC_np.py")));
			toFullFileInfo.Add(TypeIds.CC_p, new FullEnumFileInfo(dirs.GetPythonPyFilename("CC_p.py")));
			toFullFileInfo.Add(TypeIds.Code, new FullEnumFileInfo(dirs.GetPythonPyFilename("Code.py")));
			toFullFileInfo.Add(TypeIds.CodeSize, new FullEnumFileInfo(dirs.GetPythonPyFilename("CodeSize.py")));
			toFullFileInfo.Add(TypeIds.ConditionCode, new FullEnumFileInfo(dirs.GetPythonPyFilename("ConditionCode.py")));
			toFullFileInfo.Add(TypeIds.CpuidFeature, new FullEnumFileInfo(dirs.GetPythonPyFilename("CpuidFeature.py")));
			toFullFileInfo.Add(TypeIds.DecoderError, new FullEnumFileInfo(dirs.GetPythonPyFilename("DecoderError.py")));
			toFullFileInfo.Add(TypeIds.DecoderOptions, new FullEnumFileInfo(dirs.GetPythonPyFilename("DecoderOptions.py")));
			toFullFileInfo.Add(TypeIds.EncodingKind, new FullEnumFileInfo(dirs.GetPythonPyFilename("EncodingKind.py")));
			toFullFileInfo.Add(TypeIds.FlowControl, new FullEnumFileInfo(dirs.GetPythonPyFilename("FlowControl.py")));
			toFullFileInfo.Add(TypeIds.FormatMnemonicOptions, new FullEnumFileInfo(dirs.GetPythonPyFilename("FormatMnemonicOptions.py")));
			toFullFileInfo.Add(TypeIds.MandatoryPrefix, new FullEnumFileInfo(dirs.GetPythonPyFilename("MandatoryPrefix.py")));
			toFullFileInfo.Add(TypeIds.MemorySize, new FullEnumFileInfo(dirs.GetPythonPyFilename("MemorySize.py")));
			toFullFileInfo.Add(TypeIds.MemorySizeOptions, new FullEnumFileInfo(dirs.GetPythonPyFilename("MemorySizeOptions.py")));
			toFullFileInfo.Add(TypeIds.Mnemonic, new FullEnumFileInfo(dirs.GetPythonPyFilename("Mnemonic.py")));
			toFullFileInfo.Add(TypeIds.OpAccess, new FullEnumFileInfo(dirs.GetPythonPyFilename("OpAccess.py")));
			toFullFileInfo.Add(TypeIds.OpCodeOperandKind, new FullEnumFileInfo(dirs.GetPythonPyFilename("OpCodeOperandKind.py")));
			toFullFileInfo.Add(TypeIds.MvexEHBit, new FullEnumFileInfo(dirs.GetPythonPyFilename("MvexEHBit.py")));
			toFullFileInfo.Add(TypeIds.OpCodeTableKind, new FullEnumFileInfo(dirs.GetPythonPyFilename("OpCodeTableKind.py")));
			toFullFileInfo.Add(TypeIds.OpKind, new FullEnumFileInfo(dirs.GetPythonPyFilename("OpKind.py")));
			toFullFileInfo.Add(TypeIds.Register, new FullEnumFileInfo(dirs.GetPythonPyFilename("Register.py")));
			toFullFileInfo.Add(TypeIds.RepPrefixKind, new FullEnumFileInfo(dirs.GetPythonPyFilename("RepPrefixKind.py")));
			toFullFileInfo.Add(TypeIds.RflagsBits, new FullEnumFileInfo(dirs.GetPythonPyFilename("RflagsBits.py")));
			toFullFileInfo.Add(TypeIds.RoundingControl, new FullEnumFileInfo(dirs.GetPythonPyFilename("RoundingControl.py")));
			toFullFileInfo.Add(TypeIds.TupleType, new FullEnumFileInfo(dirs.GetPythonPyFilename("TupleType.py")));
			toFullFileInfo.Add(TypeIds.FormatterSyntax, new FullEnumFileInfo(dirs.GetPythonPyFilename("FormatterSyntax.py")));
			toFullFileInfo.Add(TypeIds.MvexConvFn, new FullEnumFileInfo(dirs.GetPythonPyFilename("MvexConvFn.py")));
			toFullFileInfo.Add(TypeIds.MvexRegMemConv, new FullEnumFileInfo(dirs.GetPythonPyFilename("MvexRegMemConv.py")));
			toFullFileInfo.Add(TypeIds.MvexTupleTypeLutKind, new FullEnumFileInfo(dirs.GetPythonPyFilename("MvexTupleTypeLutKind.py")));

			toPartialFileInfo = new();
			toPartialFileInfo.Add(TypeIds.FormatterSyntax, new PartialEnumFileInfo("FormatterSyntax", TargetLanguage.Rust, dirs.GetPythonRustFilename("formatter.rs")));
		}

		public override void Generate(EnumType enumType) {
			bool exportedToPython = false;
			if (toFullFileInfo.TryGetValue(enumType.TypeId, out var fullInfo)) {
				if (fullInfo is not null) {
					exportedToPython = true;
					WriteFile(fullInfo, enumType);
				}
			}
			// An enum could be present in both dicts so this should be 'if' and not 'else if'
			if (toPartialFileInfo.TryGetValue(enumType.TypeId, out var partialInfo)) {
				if (partialInfo is not null)
					new FileUpdater(partialInfo.Language, partialInfo.Id, partialInfo.Filename).Generate(writer => WriteEnum(writer, partialInfo, enumType));
			}

			if (exportedToPython)
				exportedPythonTypes.AddEnum(enumType);
		}

		void WriteEnum(FileWriter writer, PartialEnumFileInfo info, EnumType enumType) {
			switch (info.Language) {
			case TargetLanguage.Rust:
				WriteEnumRust(writer, info, enumType);
				break;
			default:
				throw new InvalidOperationException();
			}
		}

		void WriteEnumRust(FileWriter writer, PartialEnumFileInfo info, EnumType enumType) {
			rustDocWriter.WriteSummary(writer, enumType.Documentation.GetComment(TargetLanguage.Python), enumType.RawName);
			var enumTypeName = enumType.Name(rustIdConverter);
			foreach (var attr in info.Attributes)
				writer.WriteLine(attr);
			writer.WriteLine(RustConstants.AttributeAllowDeadCode);
			writer.WriteLine($"pub(crate) enum {enumTypeName} {{");
			using (writer.Indent()) {
				uint expectedValue = 0;
				foreach (var value in enumType.Values) {
					if (value.DeprecatedInfo.IsDeprecatedAndRenamed)
						continue;
					rustDocWriter.WriteSummary(writer, value.Documentation.GetComment(TargetLanguage.Python), enumType.RawName);
					if (enumType.IsFlags)
						writer.WriteLine($"{value.Name(rustIdConverter)} = {NumberFormatter.FormatHexUInt32WithSep(value.Value)},");
					else if (expectedValue != value.Value || enumType.IsPublic)
						writer.WriteLine($"{value.Name(rustIdConverter)} = {value.Value},");
					else
						writer.WriteLine($"{value.Name(rustIdConverter)},");
					expectedValue = value.Value + 1;
				}
			}
			writer.WriteLine("}");
		}

		void WriteFile(FullEnumFileInfo info, EnumType enumType) {
			var docWriter = new PythonDocCommentWriter(pythonIdConverter, TargetLanguage.Python, isInRootModule: false);
			using (var writer = new FileWriter(TargetLanguage.Python, FileUtils.OpenWrite(info.Filename))) {
				writer.WriteFileHeader();
				writer.WriteLine("# pylint: disable=invalid-name");
				writer.WriteLine("# pylint: disable=line-too-long");
				writer.WriteLine("# pylint: disable=too-many-lines");
				writer.WriteLine();
				docWriter.WriteSummary(writer, enumType.Documentation.GetComment(TargetLanguage.Python), enumType.RawName);
				writer.WriteLine();
				// Needed by Sphinx or it will generate a lot of errors
				writer.WriteLine("import typing");
				writer.WriteLine("if typing.TYPE_CHECKING:");
				using (writer.Indent())
					writer.WriteLine($"from ._iced_x86_py import {enumType.Name(pythonIdConverter)}");
				writer.WriteLine("else:");
				using (writer.Indent())
					writer.WriteLine($"{enumType.Name(pythonIdConverter)} = int");
				writer.WriteLine();
				WriteEnumCore(writer, enumType, docWriter);
			}
		}

		void WriteEnumCore(FileWriter writer, EnumType enumType, PythonDocCommentWriter docWriter) {
			bool mustHaveDocs = enumType.TypeId != TypeIds.Register && enumType.TypeId != TypeIds.Mnemonic;
			bool uppercaseRawName = Enums.EnumUtils.UppercaseTypeFields(enumType.TypeId.Id1);
			var enumTypeName = enumType.Name(pythonIdConverter);
			var firstVersion = new Version(1, 13, 0);
			// *****************************************************************************
			// For PERF reasons, we do NOT use Enums. They're incredibly slow to load!
			// Eg. loading 'class Code(IntEnum)' (plus other non-Mnemonic enums and some random
			// code) took ~850ms and when I converted them to constants, it took ~43ms!
			// *****************************************************************************
			foreach (var value in enumType.Values) {
				if (value.DeprecatedInfo.IsDeprecated && value.DeprecatedInfo.Version < firstVersion)
					continue;

				var docs = value.Documentation.GetComment(TargetLanguage.Python);
				// Sphinx doesn't include the public enum items (global vars in a module) if they're not documented
				if (string.IsNullOrEmpty(docs)) {
					if (mustHaveDocs)
						throw new InvalidOperationException();
					docs = "<no docs>";
				}

				var (valueName, numStr) = Enums.EnumUtils.GetEnumNameValue(pythonIdConverter, value, uppercaseRawName);
				writer.WriteLine($"{valueName}: {enumTypeName} = {numStr} # type: ignore");
				if (value.DeprecatedInfo.IsDeprecated) {
					string? extra;
					if (value.DeprecatedInfo.NewName is not null)
						extra = $"Use {value.DeprecatedInfo.NewName} instead";
					else
						extra = null;

					if (extra is null)
						extra = string.Empty;
					else
						extra = $": {extra}";
					docs = $"DEPRECATED({value.DeprecatedInfo.VersionStr}){extra}";
				}
				docWriter.WriteSummary(writer, docs, enumType.RawName);
			}
		}

		enum DocClassKind {
			Class,
			// Less important class
			MiscClass,
			Enum,
		}

		public override void GenerateEnd() {
			var exportedClasses = new (string name, DocClassKind kind)[] {
				("BlockEncoder", DocClassKind.Class),
				("Decoder", DocClassKind.Class),
				("Encoder", DocClassKind.Class),
				("FastFormatter", DocClassKind.Class),
				("Formatter", DocClassKind.Class),
				("Instruction", DocClassKind.Class),
				("InstructionInfo", DocClassKind.Class),
				("InstructionInfoFactory", DocClassKind.Class),
				("OpCodeInfo", DocClassKind.Class),

				("ConstantOffsets", DocClassKind.MiscClass),
				("FpuStackIncrementInfo", DocClassKind.MiscClass),
				("MemoryOperand", DocClassKind.MiscClass),
				("MemorySizeExt", DocClassKind.MiscClass),
				("MemorySizeInfo", DocClassKind.MiscClass),
				("RegisterExt", DocClassKind.MiscClass),
				("RegisterInfo", DocClassKind.MiscClass),
				("UsedMemory", DocClassKind.MiscClass),
				("UsedRegister", DocClassKind.MiscClass),
			}.
			Concat(exportedPythonTypes.Enums.Select(x => (name: x.Name(pythonIdConverter), kind: DocClassKind.Enum))).
			OrderBy(x => x.name, StringComparer.Ordinal).ToArray();

			var librsFilename = genTypes.Dirs.GetPythonRustFilename("lib.rs");
			var initPyFilename = genTypes.Dirs.GetPythonPyFilename("__init__.py");

			new FileUpdater(TargetLanguage.Rust, "EnumClassDefs", librsFilename).Generate(writer => {
				foreach (var cls in exportedClasses.Where(x => x.kind == DocClassKind.Enum)) {
					writer.WriteLine("/// DO NOT USE");
					writer.WriteLine("#[pyclass(module = \"iced_x86._iced_x86_py\")]");
					writer.WriteLine(RustConstants.AttributeAllowNonCamelCaseTypes);
					writer.WriteLine($"struct {cls.name} {{}}");
				}
			});
			new FileUpdater(TargetLanguage.Rust, "ClassExport", librsFilename).Generate(writer => {
				foreach (var name in exportedClasses.Select(x => x.name))
					writer.WriteLine($"m.add_class::<{name}>()?;");
			});

			static string GetNewTypeCheckerName(string pyName) => pyName + "_";
			using (var writer = new FileWriter(TargetLanguage.Python, FileUtils.OpenWrite(initPyFilename))) {
				writer.WriteFileHeader();
				writer.WriteLine("# pylint: disable=line-too-long");
				writer.WriteLine("# pylint: disable=no-name-in-module");
				writer.WriteLine("# pylint: disable=invalid-name");
				writer.WriteLine();
				writer.WriteLine("\"\"\"");
				writer.WriteLine("iced-x86 is a blazing fast and correct x86/x64 disassembler, assembler and instruction decoder written in Rust with Python bindings");
				writer.WriteLine("\"\"\"");
				writer.WriteLine();
				writer.WriteLine("import typing");
				foreach (var cls in exportedClasses.Where(x => x.kind != DocClassKind.Enum))
					writer.WriteLine($"from ._iced_x86_py import {cls.name} # pylint: disable=import-self");
				foreach (var cls in exportedClasses.Where(x => x.kind == DocClassKind.Enum))
					writer.WriteLine($"from . import {cls.name}");
				writer.WriteLine();
				writer.WriteLine("if typing.TYPE_CHECKING:");
				using (writer.Indent()) {
					writer.WriteLine("from . import _iced_x86_py # pylint: disable=import-self");
					foreach (var cls in exportedClasses.Where(x => x.kind == DocClassKind.Enum))
						writer.WriteLine($"{GetNewTypeCheckerName(cls.name)} = _iced_x86_py.{cls.name}");
				}
				writer.WriteLine("else:");
				using (writer.Indent()) {
					foreach (var cls in exportedClasses.Where(x => x.kind == DocClassKind.Enum))
						writer.WriteLine($"{GetNewTypeCheckerName(cls.name)} = int");
				}
				writer.WriteLine();
				writer.WriteLine("__all__ = [");
				using (writer.Indent()) {
					foreach (var cls in exportedClasses)
						writer.WriteLine($"\"{cls.name}\",");
					foreach (var cls in exportedClasses.Where(x => x.kind == DocClassKind.Enum))
						writer.WriteLine($"\"{GetNewTypeCheckerName(cls.name)}\",");
				}
				writer.WriteLine("]");
			}

			foreach (var cls in exportedClasses) {
				var rstFilename = genTypes.Dirs.GetPythonDocsSrcFilename($"{cls.name}.rst");
				var autoStr = cls.kind switch {
					DocClassKind.Class or DocClassKind.MiscClass => $".. autoclass:: iced_x86::{cls.name}",
					DocClassKind.Enum => $".. automodule:: iced_x86.{cls.name}",
					_ => throw new InvalidOperationException(),
				};
				var lines = new[] {
					cls.name,
					new string('=', cls.name.Length),
					string.Empty,
					autoStr,
					"\t:members:",
				};
				File.WriteAllLines(rstFilename, lines, FileUtils.FileEncoding);
			}

			var indexRstFilename = genTypes.Dirs.GetPythonDocsFilename("index.rst");
			UpdateIndexRst(indexRstFilename, exportedClasses);
		}

		static void UpdateIndexRst(string filename, (string name, DocClassKind kind)[] exportedClasses) {
			var lines = File.ReadAllLines(filename);
			var newLines = new List<string>();

			var docClasses = new (List<string> names, bool found)[3];
			for (int i = 0; i < docClasses.Length; i++)
				docClasses[i] = (new(), false);
			foreach (var cls in exportedClasses) {
				ref var info = ref docClasses[(int)cls.kind];
				info.names.Add(cls.name);
			}
			if (docClasses.Any(x => x.names.Count == 0))
				throw new InvalidOperationException();

			for (int lineIndex = 0; lineIndex < lines.Length; lineIndex++) {
				var line = lines[lineIndex];
				if (line.StartsWith(".. toctree::", StringComparison.Ordinal)) {
					newLines.Add(line);

					static bool IsOption(string s) => s.TrimStart().StartsWith(":", StringComparison.Ordinal);

					// Skip all files ("src/filename") until the first option
					int optIndex = -1;
					for (int tmpIndex = lineIndex + 1; tmpIndex < lines.Length; tmpIndex++) {
						if (lines[tmpIndex].Length == 0)
							throw new InvalidOperationException();
						if (IsOption(lines[tmpIndex])) {
							optIndex = tmpIndex;
							break;
						}
					}
					if (optIndex < 0)
						throw new InvalidOperationException();

					// Find index of last option and also the caption
					string caption = string.Empty;
					int optIndexEnd = -1;
					for (int tmpIndex = optIndex; tmpIndex < lines.Length; tmpIndex++) {
						var optLine = lines[tmpIndex];
						if (!IsOption(optLine)) {
							optIndexEnd = tmpIndex - 1;
							break;
						}
						if (optLine.TrimStart().StartsWith(":caption:", StringComparison.Ordinal)) {
							if (caption.Length != 0)
								throw new InvalidOperationException();
							caption = optLine.Trim();
						}
					}
					if (caption.Length == 0)
						throw new InvalidOperationException();
					if (optIndexEnd < 0)
						throw new InvalidOperationException();

					var kind = caption switch {
						":caption: Classes:" => DocClassKind.Class,
						":caption: Misc Classes:" => DocClassKind.MiscClass,
						":caption: Enums:" => DocClassKind.Enum,
						_ => throw new InvalidOperationException(),
					};
					ref var info = ref docClasses[(int)kind];
					if (info.found)
						throw new InvalidOperationException($"Dupe {kind}");
					info.found = true;

					foreach (var name in info.names.OrderBy(x => x, StringComparer.Ordinal))
						newLines.Add($"\tsrc/{name}");
					for (int i = optIndex; i <= optIndexEnd; i++)
						newLines.Add(lines[i]);

					lineIndex = optIndexEnd;
				}
				else
					newLines.Add(line);
			}
			if (docClasses.Any(x => !x.found))
				throw new InvalidOperationException();
			File.WriteAllLines(filename, newLines, FileUtils.FileEncoding);
		}
	}
}
