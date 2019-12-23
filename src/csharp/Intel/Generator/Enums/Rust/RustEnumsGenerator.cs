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
using Generator.Constants;
using Generator.Constants.Rust;
using Generator.Documentation.Rust;
using Generator.IO;

namespace Generator.Enums.Rust {
	[Generator(TargetLanguage.Rust, GeneratorNames.Enums)]
	sealed class RustEnumsGenerator : EnumsGenerator {
		readonly IdentifierConverter idConverter;
		readonly Dictionary<TypeId, PartialEnumFileInfo?> toPartialFileInfo;
		readonly RustDocCommentWriter docWriter;
		readonly RustConstantsWriter constantsWriter;

		sealed class PartialEnumFileInfo {
			public readonly string Id;
			public readonly string Filename;
			public readonly string[] Attributes;

			public PartialEnumFileInfo(string id, string filename, string? attribute = null) {
				Id = id;
				Filename = filename;
				Attributes = attribute is null ? Array.Empty<string>() : new string[] { attribute };
			}

			public PartialEnumFileInfo(string id, string filename, string[] attributes) {
				Id = id;
				Filename = filename;
				Attributes = attributes;
			}
		}

		public RustEnumsGenerator(GeneratorOptions generatorOptions) {
			idConverter = RustIdentifierConverter.Create();
			docWriter = new RustDocCommentWriter(idConverter);
			constantsWriter = new RustConstantsWriter(idConverter, docWriter);

			toPartialFileInfo = new Dictionary<TypeId, PartialEnumFileInfo?>();
			toPartialFileInfo.Add(TypeIds.Code, new PartialEnumFileInfo("Code", Path.Combine(generatorOptions.RustDir, "code.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CodeSize, new PartialEnumFileInfo("CodeSize", Path.Combine(generatorOptions.RustDir, "enums.rs"), RustConstants.AttributeCopyEqOrdHash));
			toPartialFileInfo.Add(TypeIds.ConditionCode, new PartialEnumFileInfo("ConditionCode", Path.Combine(generatorOptions.RustDir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CpuidFeature, new PartialEnumFileInfo("CpuidFeature", Path.Combine(generatorOptions.RustDir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CpuidFeatureInternal, new PartialEnumFileInfo("CpuidFeatureInternal", Path.Combine(generatorOptions.RustDir, "info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.DecoderOptions, new PartialEnumFileInfo("DecoderOptions", Path.Combine(generatorOptions.RustDir, "decoder", "mod.rs")));
			toPartialFileInfo.Add(TypeIds.EvexOpCodeHandlerKind, null);
			toPartialFileInfo.Add(TypeIds.HandlerFlags, new PartialEnumFileInfo("HandlerFlags", Path.Combine(generatorOptions.RustDir, "decoder", "mod.rs")));
			toPartialFileInfo.Add(TypeIds.LegacyHandlerFlags, null);
			toPartialFileInfo.Add(TypeIds.MemorySize, new PartialEnumFileInfo("MemorySize", Path.Combine(generatorOptions.RustDir, "memory_size.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpCodeHandlerKind, null);
			toPartialFileInfo.Add(TypeIds.PseudoOpsKind, null);
			toPartialFileInfo.Add(TypeIds.Register, new PartialEnumFileInfo("Register", Path.Combine(generatorOptions.RustDir, "register.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive }));
			toPartialFileInfo.Add(TypeIds.SerializedDataKind, null);
			toPartialFileInfo.Add(TypeIds.TupleType, new PartialEnumFileInfo("TupleType", Path.Combine(generatorOptions.RustDir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive, RustConstants.FeatureDecoderOrEncoder, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.VexOpCodeHandlerKind, null);
			toPartialFileInfo.Add(TypeIds.Mnemonic, new PartialEnumFileInfo("Mnemonic", Path.Combine(generatorOptions.RustDir, "mnemonic.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive }));
			toPartialFileInfo.Add(TypeIds.GasCtorKind, null);
			toPartialFileInfo.Add(TypeIds.IntelCtorKind, null);
			toPartialFileInfo.Add(TypeIds.MasmCtorKind, null);
			toPartialFileInfo.Add(TypeIds.NasmCtorKind, null);
			toPartialFileInfo.Add(TypeIds.GasSizeOverride, null);
			toPartialFileInfo.Add(TypeIds.GasInstrOpInfoFlags, null);
			toPartialFileInfo.Add(TypeIds.IntelSizeOverride, null);
			toPartialFileInfo.Add(TypeIds.IntelBranchSizeInfo, null);
			toPartialFileInfo.Add(TypeIds.IntelInstrOpInfoFlags, null);
			toPartialFileInfo.Add(TypeIds.MasmInstrOpInfoFlags, null);
			toPartialFileInfo.Add(TypeIds.NasmSignExtendInfo, null);
			toPartialFileInfo.Add(TypeIds.NasmSizeOverride, null);
			toPartialFileInfo.Add(TypeIds.NasmBranchSizeInfo, null);
			toPartialFileInfo.Add(TypeIds.NasmInstrOpInfoFlags, null);
			toPartialFileInfo.Add(TypeIds.NasmMemorySizeInfo, null);
			toPartialFileInfo.Add(TypeIds.NasmFarMemorySizeInfo, null);
			toPartialFileInfo.Add(TypeIds.MandatoryPrefix, new PartialEnumFileInfo("MandatoryPrefix", Path.Combine(generatorOptions.RustDir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.FeatureEncoder }));
			toPartialFileInfo.Add(TypeIds.OpCodeTableKind, new PartialEnumFileInfo("OpCodeTableKind", Path.Combine(generatorOptions.RustDir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.FeatureEncoder }));
			toPartialFileInfo.Add(TypeIds.RoundingControl, new PartialEnumFileInfo("RoundingControl", Path.Combine(generatorOptions.RustDir, "enums.rs"), RustConstants.AttributeCopyEqOrdHash));
			toPartialFileInfo.Add(TypeIds.OpKind, new PartialEnumFileInfo("OpKind", Path.Combine(generatorOptions.RustDir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.Instruction_MemoryFlags, new PartialEnumFileInfo("MemoryFlags", Path.Combine(generatorOptions.RustDir, "instruction.rs")));
			toPartialFileInfo.Add(TypeIds.Instruction_OpKindFlags, new PartialEnumFileInfo("OpKindFlags", Path.Combine(generatorOptions.RustDir, "instruction.rs")));
			toPartialFileInfo.Add(TypeIds.Instruction_CodeFlags, new PartialEnumFileInfo("CodeFlags", Path.Combine(generatorOptions.RustDir, "instruction.rs")));
			toPartialFileInfo.Add(TypeIds.VectorLength, new PartialEnumFileInfo("VectorLength", Path.Combine(generatorOptions.RustDir, "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureDecoderOrEncoder }));
			toPartialFileInfo.Add(TypeIds.MandatoryPrefixByte, new PartialEnumFileInfo("MandatoryPrefixByte", Path.Combine(generatorOptions.RustDir, "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureDecoderOrEncoder }));
			toPartialFileInfo.Add(TypeIds.StateFlags, new PartialEnumFileInfo("StateFlags", Path.Combine(generatorOptions.RustDir, "decoder", "mod.rs")));
			toPartialFileInfo.Add(TypeIds.EncodingKind, new PartialEnumFileInfo("EncodingKind", Path.Combine(generatorOptions.RustDir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive, RustConstants.FeatureDecoderOrEncoderOrInstrInfo }));
			toPartialFileInfo.Add(TypeIds.FlowControl, new PartialEnumFileInfo("FlowControl", Path.Combine(generatorOptions.RustDir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.FeatureInstrInfo }));
			toPartialFileInfo.Add(TypeIds.OpCodeOperandKind, new PartialEnumFileInfo("OpCodeOperandKind", Path.Combine(generatorOptions.RustDir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive, RustConstants.FeatureEncoder, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.RflagsBits, new PartialEnumFileInfo("RflagsBits", Path.Combine(generatorOptions.RustDir, "enums.rs"), RustConstants.FeatureInstrInfo));
			toPartialFileInfo.Add(TypeIds.CodeInfo, new PartialEnumFileInfo("CodeInfo", Path.Combine(generatorOptions.RustDir, "info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.RflagsInfo, new PartialEnumFileInfo("RflagsInfo", Path.Combine(generatorOptions.RustDir, "info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpInfo0, new PartialEnumFileInfo("OpInfo0", Path.Combine(generatorOptions.RustDir, "info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpInfo1, new PartialEnumFileInfo("OpInfo1", Path.Combine(generatorOptions.RustDir, "info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpInfo2, new PartialEnumFileInfo("OpInfo2", Path.Combine(generatorOptions.RustDir, "info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpInfo3, new PartialEnumFileInfo("OpInfo3", Path.Combine(generatorOptions.RustDir, "info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpInfo4, new PartialEnumFileInfo("OpInfo4", Path.Combine(generatorOptions.RustDir, "info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.InfoFlags1, new PartialEnumFileInfo("InfoFlags1", Path.Combine(generatorOptions.RustDir, "info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo }));
			toPartialFileInfo.Add(TypeIds.InfoFlags2, new PartialEnumFileInfo("InfoFlags2", Path.Combine(generatorOptions.RustDir, "info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo }));
			toPartialFileInfo.Add(TypeIds.OpAccess, new PartialEnumFileInfo("OpAccess", Path.Combine(generatorOptions.RustDir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.FeatureInstrInfo }));
			toPartialFileInfo.Add(TypeIds.MemorySizeFlags, new PartialEnumFileInfo("MemorySizeFlags", Path.Combine(generatorOptions.RustDir, "info", "tests", "constants.rs"), new[] { RustConstants.AttributeCopyEqOrdHash }));
			toPartialFileInfo.Add(TypeIds.RegisterFlags, new PartialEnumFileInfo("RegisterFlags", Path.Combine(generatorOptions.RustDir, "info", "tests", "constants.rs"), new[] { RustConstants.AttributeCopyEqOrdHash }));
			toPartialFileInfo.Add(TypeIds.LegacyOpCodeTable, new PartialEnumFileInfo("LegacyOpCodeTable", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.VexOpCodeTable, new PartialEnumFileInfo("VexOpCodeTable", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.XopOpCodeTable, new PartialEnumFileInfo("XopOpCodeTable", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.EvexOpCodeTable, new PartialEnumFileInfo("EvexOpCodeTable", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.Encodable, new PartialEnumFileInfo("Encodable", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpCodeHandlerFlags, new PartialEnumFileInfo("OpCodeHandlerFlags", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.FeatureEncoder }));
			toPartialFileInfo.Add(TypeIds.LegacyOpKind, new PartialEnumFileInfo("LegacyOpKind", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.VexOpKind, new PartialEnumFileInfo("VexOpKind", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.XopOpKind, new PartialEnumFileInfo("XopOpKind", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.EvexOpKind, new PartialEnumFileInfo("EvexOpKind", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OperandSize, new PartialEnumFileInfo("OperandSize", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.AddressSize, new PartialEnumFileInfo("AddressSize", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.VexVectorLength, new PartialEnumFileInfo("VexVectorLength", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.XopVectorLength, new PartialEnumFileInfo("XopVectorLength", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.EvexVectorLength, new PartialEnumFileInfo("EvexVectorLength", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.DisplSize, new PartialEnumFileInfo("DisplSize", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.ImmSize, new PartialEnumFileInfo("ImmSize", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.EncoderFlags, new PartialEnumFileInfo("EncoderFlags", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.EncFlags1, new PartialEnumFileInfo("EncFlags1", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.LegacyFlags3, new PartialEnumFileInfo("LegacyFlags3", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.VexFlags3, new PartialEnumFileInfo("VexFlags3", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.XopFlags3, new PartialEnumFileInfo("XopFlags3", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.EvexFlags3, new PartialEnumFileInfo("EvexFlags3", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.AllowedPrefixes, new PartialEnumFileInfo("AllowedPrefixes", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.LegacyFlags, new PartialEnumFileInfo("LegacyFlags", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.VexFlags, new PartialEnumFileInfo("VexFlags", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.XopFlags, new PartialEnumFileInfo("XopFlags", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.EvexFlags, new PartialEnumFileInfo("EvexFlags", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.D3nowFlags, new PartialEnumFileInfo("D3nowFlags", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.WBit, new PartialEnumFileInfo("WBit", Path.Combine(generatorOptions.RustDir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
		}

		public override void Generate(EnumType enumType) {
			if (toPartialFileInfo.TryGetValue(enumType.TypeId, out var partialInfo)) {
				if (!(partialInfo is null))
					new FileUpdater(TargetLanguage.Rust, partialInfo.Id, partialInfo.Filename).Generate(writer => WriteEnum(writer, partialInfo, enumType));
			}
			else
				throw new InvalidOperationException();
		}

		void WriteEnum(FileWriter writer, PartialEnumFileInfo info, EnumType enumType) {
			if (enumType.IsFlags) {
				var attrs = new List<string>();
				if (enumType.IsPublic)
					attrs.Add(RustConstants.AttributeAllowMissingCopyImplementations);
				attrs.AddRange(info.Attributes.Where(a => a.StartsWith(RustConstants.FeaturePrefix)));
				constantsWriter.Write(writer, enumType.ToConstantsType(ConstantKind.UInt32), attrs.ToArray());
			}
			else
				WriteEnumCore(writer, info, enumType);
		}

		void WriteEnumCore(FileWriter writer, PartialEnumFileInfo info, EnumType enumType) {
			docWriter.Write(writer, enumType.Documentation, enumType.RawName);
			var enumTypeName = enumType.Name(idConverter);
			foreach (var attr in info.Attributes)
				writer.WriteLine(attr);
			if (enumType.IsPublic && enumType.IsMissingDocs)
				writer.WriteLine(RustConstants.AttributeAllowMissingDocs);
			var pub = enumType.IsPublic ? "pub " : "pub(crate) ";
			writer.WriteLine($"{pub}enum {enumTypeName} {{");
			using (writer.Indent()) {
				uint expectedValue = 0;
				foreach (var value in enumType.Values) {
					docWriter.Write(writer, value.Documentation, enumType.RawName);
					if (expectedValue != value.Value)
						writer.WriteLine($"{value.Name(idConverter)} = {value.Value},");
					else
						writer.WriteLine($"{value.Name(idConverter)},");
					expectedValue = value.Value + 1;
				}
			}
			writer.WriteLine("}");

			var arrayName = idConverter.Constant("GenDebug" + enumType.RawName);
			var feature = info.Attributes.FirstOrDefault(a => a.StartsWith(RustConstants.FeaturePrefix) && a.Contains("(feature"));
			if (!(feature is null))
				writer.WriteLine(feature);
			writer.WriteLine(RustConstants.AttributeNoRustFmt);
			writer.WriteLine($"static {arrayName}: [&str; {enumType.Values.Length}] = [");
			using (writer.Indent()) {
				for (int i = 0; i < enumType.Values.Length; i++)
					writer.WriteLine($"\"{enumType.Values[i].Name(idConverter)}\",");
			}
			writer.WriteLine("];");

			// #[derive(Debug)] isn't used since it generates a big switch statement. This code
			// uses a simple array lookup which has very little code. For small enums the default
			// implementation might be better though.
			if (!(feature is null))
				writer.WriteLine(feature);
			writer.WriteLine($"impl fmt::Debug for {enumTypeName} {{");
			using (writer.Indent()) {
				writer.WriteLine(RustConstants.AttributeAllowMissingInlineInPublicItems);
				writer.WriteLine($"fn fmt<'a>(&self, f: &mut fmt::Formatter<'a>) -> fmt::Result {{");
				using (writer.Indent()) {
					writer.WriteLine($"write!(f, \"{{}}\", {arrayName}[*self as usize])?;");
					writer.WriteLine("Ok(())");
				}
				writer.WriteLine("}");
			}
			writer.WriteLine("}");

			if (!(feature is null))
				writer.WriteLine(feature);
			writer.WriteLine($"impl Default for {enumTypeName} {{");
			using (writer.Indent()) {
				writer.WriteLine(RustConstants.AttributeMustUse);
				writer.WriteLine(RustConstants.AttributeInline);
				writer.WriteLine("fn default() -> Self {");
				using (writer.Indent()) {
					var defaultValue = enumType.Values[0];
					// The first one should always have value 0 (eg. "None" field), and if the first one doesn't
					// have value 0, there must be no other enum fields == 0.
					if (defaultValue.Value != 0 && enumType.Values.Any(a => a.Value == 0))
						throw new InvalidOperationException();
					writer.WriteLine($"{enumTypeName}::{defaultValue.Name(idConverter)}");
				}
				writer.WriteLine("}");
			}
			writer.WriteLine("}");
		}
	}
}
