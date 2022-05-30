// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System.Collections.Generic;
using Generator.Documentation;
using Generator.Documentation.Rust;
using Generator.Documentation.RustJS;
using Generator.IO;

namespace Generator.Enums.RustJS {
	[Generator(TargetLanguage.RustJS)]
	sealed class RustJSEnumsGenerator : EnumsGenerator {
		readonly IdentifierConverter idConverter;
		readonly Dictionary<TypeId, PartialEnumFileInfo?> toPartialFileInfo;
		readonly RustDocCommentWriter docWriter;
		readonly DeprecatedWriter deprecatedWriter;

		sealed class PartialEnumFileInfo {
			public readonly string Id;
			public readonly string Filename;
			public readonly string[] Attributes;

			public PartialEnumFileInfo(string id, string filename, params string[] attributes) {
				Id = id;
				Filename = filename;
				Attributes = attributes;
			}
		}

		public RustJSEnumsGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			idConverter = RustJSIdentifierConverter.Create();
			docWriter = new RustDocCommentWriter(idConverter, ".", ".", ".", ".");
			deprecatedWriter = new RustJSDeprecatedWriter(idConverter);

			var dirs = generatorContext.Types.Dirs;
			toPartialFileInfo = new();
			toPartialFileInfo.Add(TypeIds.BlockEncoderOptions, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("block_encoder_options.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_a, new PartialEnumFileInfo("CC_a", dirs.GetRustJSFilename("cc.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_ae, new PartialEnumFileInfo("CC_ae", dirs.GetRustJSFilename("cc.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_b, new PartialEnumFileInfo("CC_b", dirs.GetRustJSFilename("cc.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_be, new PartialEnumFileInfo("CC_be", dirs.GetRustJSFilename("cc.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_e, new PartialEnumFileInfo("CC_e", dirs.GetRustJSFilename("cc.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_g, new PartialEnumFileInfo("CC_g", dirs.GetRustJSFilename("cc.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_ge, new PartialEnumFileInfo("CC_ge", dirs.GetRustJSFilename("cc.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_l, new PartialEnumFileInfo("CC_l", dirs.GetRustJSFilename("cc.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_le, new PartialEnumFileInfo("CC_le", dirs.GetRustJSFilename("cc.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_ne, new PartialEnumFileInfo("CC_ne", dirs.GetRustJSFilename("cc.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_np, new PartialEnumFileInfo("CC_np", dirs.GetRustJSFilename("cc.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_p, new PartialEnumFileInfo("CC_p", dirs.GetRustJSFilename("cc.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.Code, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("code.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CodeSize, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("code_size.rs"), RustConstants.AttributeCopyClone));
			toPartialFileInfo.Add(TypeIds.ConditionCode, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("condition_code.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CpuidFeature, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("cpuid_feature.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.DecoderError, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("decoder_error.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.DecoderOptions, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("decoder_options.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.EncodingKind, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("encoding_kind.rs"), new[] { RustConstants.AttributeCopyClone }));
			toPartialFileInfo.Add(TypeIds.FlowControl, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("flow_control.rs"), new[] { RustConstants.AttributeCopyClone }));
			toPartialFileInfo.Add(TypeIds.FormatMnemonicOptions, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("format_mnemonic_options.rs"), RustConstants.AttributeCopyClone));
			toPartialFileInfo.Add(TypeIds.MandatoryPrefix, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("mandatory_prefix.rs"), new[] { RustConstants.AttributeCopyClone }));
			toPartialFileInfo.Add(TypeIds.MemorySize, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("memory_size.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.MemorySizeOptions, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("memory_size_options.rs"), RustConstants.AttributeCopyClone));
			toPartialFileInfo.Add(TypeIds.Mnemonic, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("mnemonic.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpAccess, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("op_access.rs"), new[] { RustConstants.AttributeCopyClone }));
			toPartialFileInfo.Add(TypeIds.OpCodeOperandKind, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("op_code_operand_kind.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.MvexEHBit, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("mvex_eh_bit.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpCodeTableKind, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("op_code_table_kind.rs"), new[] { RustConstants.AttributeCopyClone }));
			toPartialFileInfo.Add(TypeIds.OpKind, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("op_kind.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.Register, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("register.rs"), new[] { RustConstants.AttributeCopyClone }));
			toPartialFileInfo.Add(TypeIds.RepPrefixKind, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("rep_prefix_kind.rs"), new[] { RustConstants.AttributeCopyClone }));
			toPartialFileInfo.Add(TypeIds.RflagsBits, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("rflags_bits.rs"), RustConstants.AttributeCopyClone));
			toPartialFileInfo.Add(TypeIds.RoundingControl, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("rounding_control.rs"), RustConstants.AttributeCopyClone));
			toPartialFileInfo.Add(TypeIds.TupleType, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("tuple_type.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.FormatterSyntax, new PartialEnumFileInfo("FormatterSyntax", dirs.GetRustJSFilename("formatter.rs")));
			toPartialFileInfo.Add(TypeIds.MvexConvFn, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("mvex_cvt_fn.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.MvexRegMemConv, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("mvex_rm_conv.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.MvexTupleTypeLutKind, new PartialEnumFileInfo("Enum", dirs.GetRustJSFilename("mvex_tt_lut.rs"), new[] { RustConstants.AttributeCopyClone, RustConstants.AttributeAllowNonCamelCaseTypes }));
		}

		public override void Generate(EnumType enumType) {
			if (toPartialFileInfo.TryGetValue(enumType.TypeId, out var partialInfo)) {
				if (partialInfo is not null)
					new FileUpdater(TargetLanguage.RustJS, partialInfo.Id, partialInfo.Filename).Generate(writer => WriteEnum(writer, partialInfo, enumType));
			}
		}

		void WriteEnum(FileWriter writer, PartialEnumFileInfo info, EnumType enumType) =>
			WriteEnumCore(writer, info, enumType);

		void WriteEnumCore(FileWriter writer, PartialEnumFileInfo info, EnumType enumType) {
			// Don't add the Code comments since they're generated (less useful comments) and will generate bigger ts/js files
			bool writeComments = enumType.TypeId != TypeIds.Code;
			docWriter.WriteSummary(writer, enumType.Documentation.GetComment(TargetLanguage.RustJS), enumType.RawName);
			var enumTypeName = enumType.Name(idConverter);
			if (enumType.IsPublic)
				writer.WriteLine(RustConstants.AttributeWasmBindgen);
			foreach (var attr in info.Attributes)
				writer.WriteLine(attr);
			if (!writeComments || (enumType.IsPublic && enumType.IsMissingDocs))
				writer.WriteLine(RustConstants.AttributeAllowMissingDocs);
			if (!enumType.IsPublic)
				writer.WriteLine(RustConstants.AttributeAllowDeadCode);
			var pub = enumType.IsPublic ? "pub " : "pub(crate) ";
			writer.WriteLine($"{pub}enum {enumTypeName} {{");
			using (writer.Indent()) {
				uint expectedValue = 0;
				foreach (var value in enumType.Values) {
					// Identical enum values aren't allowed so just remove them
					if (value.DeprecatedInfo.IsDeprecatedAndRenamed)
						continue;
					if (writeComments)
						docWriter.WriteSummary(writer, value.Documentation.GetComment(TargetLanguage.RustJS), enumType.RawName);
					deprecatedWriter.WriteDeprecated(writer, value);
					if (enumType.IsFlags)
						writer.WriteLine($"{value.Name(idConverter)} = {NumberFormatter.FormatHexUInt32WithSep(value.Value)},");
					else if (expectedValue != value.Value || enumType.IsPublic)
						writer.WriteLine($"{value.Name(idConverter)} = {value.Value},");
					else
						writer.WriteLine($"{value.Name(idConverter)},");
					expectedValue = value.Value + 1;
				}
			}
			writer.WriteLine("}");
		}
	}
}
