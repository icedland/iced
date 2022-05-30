// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using Generator.Documentation.Lua;
using Generator.IO;

namespace Generator.Enums.Lua {
	[Generator(TargetLanguage.Lua)]
	sealed class LuaEnumsGenerator : EnumsGenerator {
		readonly IdentifierConverter luaIdConverter;
		readonly IdentifierConverter rustIdConverter;
		readonly Dictionary<TypeId, FullEnumFileInfo?> toFullFileInfo;
		readonly Dictionary<TypeId, PartialEnumFileInfo?> toPartialFileInfo;
		readonly Documentation.Rust.RustDocCommentWriter rustDocWriter;

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

		public LuaEnumsGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			luaIdConverter = LuaIdentifierConverter.Create();
			rustIdConverter = RustIdentifierConverter.Create();
			rustDocWriter = new Documentation.Rust.RustDocCommentWriter(rustIdConverter);

			var dirs = generatorContext.Types.Dirs;
			toFullFileInfo = new();
			toFullFileInfo.Add(TypeIds.BlockEncoderOptions, new FullEnumFileInfo(dirs.GetLuaFilename("BlockEncoderOptions.lua")));
			toFullFileInfo.Add(TypeIds.CC_a, new FullEnumFileInfo(dirs.GetLuaFilename("CC_a.lua")));
			toFullFileInfo.Add(TypeIds.CC_ae, new FullEnumFileInfo(dirs.GetLuaFilename("CC_ae.lua")));
			toFullFileInfo.Add(TypeIds.CC_b, new FullEnumFileInfo(dirs.GetLuaFilename("CC_b.lua")));
			toFullFileInfo.Add(TypeIds.CC_be, new FullEnumFileInfo(dirs.GetLuaFilename("CC_be.lua")));
			toFullFileInfo.Add(TypeIds.CC_e, new FullEnumFileInfo(dirs.GetLuaFilename("CC_e.lua")));
			toFullFileInfo.Add(TypeIds.CC_g, new FullEnumFileInfo(dirs.GetLuaFilename("CC_g.lua")));
			toFullFileInfo.Add(TypeIds.CC_ge, new FullEnumFileInfo(dirs.GetLuaFilename("CC_ge.lua")));
			toFullFileInfo.Add(TypeIds.CC_l, new FullEnumFileInfo(dirs.GetLuaFilename("CC_l.lua")));
			toFullFileInfo.Add(TypeIds.CC_le, new FullEnumFileInfo(dirs.GetLuaFilename("CC_le.lua")));
			toFullFileInfo.Add(TypeIds.CC_ne, new FullEnumFileInfo(dirs.GetLuaFilename("CC_ne.lua")));
			toFullFileInfo.Add(TypeIds.CC_np, new FullEnumFileInfo(dirs.GetLuaFilename("CC_np.lua")));
			toFullFileInfo.Add(TypeIds.CC_p, new FullEnumFileInfo(dirs.GetLuaFilename("CC_p.lua")));
			toFullFileInfo.Add(TypeIds.Code, new FullEnumFileInfo(dirs.GetLuaFilename("Code.lua")));
			toFullFileInfo.Add(TypeIds.CodeSize, new FullEnumFileInfo(dirs.GetLuaFilename("CodeSize.lua")));
			toFullFileInfo.Add(TypeIds.ConditionCode, new FullEnumFileInfo(dirs.GetLuaFilename("ConditionCode.lua")));
			toFullFileInfo.Add(TypeIds.CpuidFeature, new FullEnumFileInfo(dirs.GetLuaFilename("CpuidFeature.lua")));
			toFullFileInfo.Add(TypeIds.DecoderError, new FullEnumFileInfo(dirs.GetLuaFilename("DecoderError.lua")));
			toFullFileInfo.Add(TypeIds.DecoderOptions, new FullEnumFileInfo(dirs.GetLuaFilename("DecoderOptions.lua")));
			toFullFileInfo.Add(TypeIds.EncodingKind, new FullEnumFileInfo(dirs.GetLuaFilename("EncodingKind.lua")));
			toFullFileInfo.Add(TypeIds.FlowControl, new FullEnumFileInfo(dirs.GetLuaFilename("FlowControl.lua")));
			toFullFileInfo.Add(TypeIds.FormatMnemonicOptions, new FullEnumFileInfo(dirs.GetLuaFilename("FormatMnemonicOptions.lua")));
			toFullFileInfo.Add(TypeIds.MandatoryPrefix, new FullEnumFileInfo(dirs.GetLuaFilename("MandatoryPrefix.lua")));
			toFullFileInfo.Add(TypeIds.MemorySize, new FullEnumFileInfo(dirs.GetLuaFilename("MemorySize.lua")));
			toFullFileInfo.Add(TypeIds.MemorySizeOptions, new FullEnumFileInfo(dirs.GetLuaFilename("MemorySizeOptions.lua")));
			toFullFileInfo.Add(TypeIds.Mnemonic, new FullEnumFileInfo(dirs.GetLuaFilename("Mnemonic.lua")));
			toFullFileInfo.Add(TypeIds.OpAccess, new FullEnumFileInfo(dirs.GetLuaFilename("OpAccess.lua")));
			toFullFileInfo.Add(TypeIds.OpCodeOperandKind, new FullEnumFileInfo(dirs.GetLuaFilename("OpCodeOperandKind.lua")));
			toFullFileInfo.Add(TypeIds.MvexEHBit, new FullEnumFileInfo(dirs.GetLuaFilename("MvexEHBit.lua")));
			toFullFileInfo.Add(TypeIds.OpCodeTableKind, new FullEnumFileInfo(dirs.GetLuaFilename("OpCodeTableKind.lua")));
			toFullFileInfo.Add(TypeIds.OpKind, new FullEnumFileInfo(dirs.GetLuaFilename("OpKind.lua")));
			toFullFileInfo.Add(TypeIds.Register, new FullEnumFileInfo(dirs.GetLuaFilename("Register.lua")));
			toFullFileInfo.Add(TypeIds.RepPrefixKind, new FullEnumFileInfo(dirs.GetLuaFilename("RepPrefixKind.lua")));
			toFullFileInfo.Add(TypeIds.RflagsBits, new FullEnumFileInfo(dirs.GetLuaFilename("RflagsBits.lua")));
			toFullFileInfo.Add(TypeIds.RoundingControl, new FullEnumFileInfo(dirs.GetLuaFilename("RoundingControl.lua")));
			toFullFileInfo.Add(TypeIds.TupleType, new FullEnumFileInfo(dirs.GetLuaFilename("TupleType.lua")));
			toFullFileInfo.Add(TypeIds.FormatterSyntax, new FullEnumFileInfo(dirs.GetLuaFilename("FormatterSyntax.lua")));
			toFullFileInfo.Add(TypeIds.MvexConvFn, new FullEnumFileInfo(dirs.GetLuaFilename("MvexConvFn.lua")));
			toFullFileInfo.Add(TypeIds.MvexRegMemConv, new FullEnumFileInfo(dirs.GetLuaFilename("MvexRegMemConv.lua")));
			toFullFileInfo.Add(TypeIds.MvexTupleTypeLutKind, new FullEnumFileInfo(dirs.GetLuaFilename("MvexTupleTypeLutKind.lua")));

			toPartialFileInfo = new();
			toPartialFileInfo.Add(TypeIds.FormatterSyntax, new PartialEnumFileInfo("FormatterSyntax", TargetLanguage.Rust, dirs.GetLuaRustFilename("fmt.rs")));
		}

		public override void Generate(EnumType enumType) {
			if (toFullFileInfo.TryGetValue(enumType.TypeId, out var fullInfo)) {
				if (fullInfo is not null)
					WriteFile(fullInfo, enumType);
			}
			// An enum could be present in both dicts so this should be 'if' and not 'else if'
			if (toPartialFileInfo.TryGetValue(enumType.TypeId, out var partialInfo)) {
				if (partialInfo is not null)
					new FileUpdater(partialInfo.Language, partialInfo.Id, partialInfo.Filename).Generate(writer => WriteEnum(writer, partialInfo, enumType));
			}
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
			rustDocWriter.WriteSummary(writer, enumType.Documentation.GetComment(TargetLanguage.Lua), enumType.RawName);
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
					rustDocWriter.WriteSummary(writer, value.Documentation.GetComment(TargetLanguage.Lua), enumType.RawName);
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
			var docWriter = new LuaDocCommentWriter(luaIdConverter);
			using (var writer = new FileWriter(TargetLanguage.Lua, FileUtils.OpenWrite(info.Filename))) {
				writer.WriteFileHeader();
				docWriter.WriteSummary(writer, enumType.Documentation.GetComment(TargetLanguage.Lua), enumType.RawName);
				writer.WriteLine("return {");
				using (writer.Indent())
					WriteEnumCore(writer, enumType, docWriter);
				writer.WriteLine("}");
			}
		}

		void WriteEnumCore(FileWriter writer, EnumType enumType, LuaDocCommentWriter docWriter) {
			var firstVersion = new Version(1, 18, 0);
			foreach (var value in enumType.Values) {
				if (value.DeprecatedInfo.IsDeprecated && value.DeprecatedInfo.Version < firstVersion)
					continue;

				var docs = value.Documentation.GetComment(TargetLanguage.Lua);
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

				var (valueName, numStr) = LuaUtils.GetEnumNameValue(luaIdConverter, value);
				writer.WriteLine($"{valueName} = {numStr},");
			}
		}
	}
}
