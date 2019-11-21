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
using Generator.Constants;
using Generator.Constants.Rust;
using Generator.Documentation.Rust;
using Generator.IO;

namespace Generator.Enums.Rust {
	sealed class RustEnumsGenerator : IEnumsGenerator {
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
			toPartialFileInfo.Add(TypeIds.Code, new PartialEnumFileInfo("Code", Path.Combine(generatorOptions.RustDir, "code.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CodeSize, new PartialEnumFileInfo("CodeSize", Path.Combine(generatorOptions.RustDir, "enums.rs"), RustConstants.AttributeCopyEq));
			toPartialFileInfo.Add(TypeIds.CpuidFeature, new PartialEnumFileInfo("CpuidFeature", Path.Combine(generatorOptions.RustDir, "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CpuidFeatureInternal, null);
			toPartialFileInfo.Add(TypeIds.DecoderOptions, new PartialEnumFileInfo("DecoderOptions", Path.Combine(generatorOptions.RustDir, "decoder", "mod.rs")));
			toPartialFileInfo.Add(TypeIds.EvexOpCodeHandlerKind, null);
			toPartialFileInfo.Add(TypeIds.HandlerFlags, new PartialEnumFileInfo("HandlerFlags", Path.Combine(generatorOptions.RustDir, "decoder", "mod.rs")));
			toPartialFileInfo.Add(TypeIds.LegacyHandlerFlags, null);
			toPartialFileInfo.Add(TypeIds.MemorySize, new PartialEnumFileInfo("MemorySize", Path.Combine(generatorOptions.RustDir, "memory_size.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpCodeHandlerKind, null);
			toPartialFileInfo.Add(TypeIds.PseudoOpsKind, null);
			toPartialFileInfo.Add(TypeIds.Register, new PartialEnumFileInfo("Register", Path.Combine(generatorOptions.RustDir, "register.rs"), RustConstants.AttributeCopyEqOrdHash));
			toPartialFileInfo.Add(TypeIds.SerializedDataKind, null);
			toPartialFileInfo.Add(TypeIds.TupleType, new PartialEnumFileInfo("TupleType", Path.Combine(generatorOptions.RustDir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.FeatureDecoderOrEncoder, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.VexOpCodeHandlerKind, null);
			toPartialFileInfo.Add(TypeIds.Mnemonic, new PartialEnumFileInfo("Mnemonic", Path.Combine(generatorOptions.RustDir, "mnemonic.rs"), RustConstants.AttributeCopyEqOrdHash));
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
			toPartialFileInfo.Add(TypeIds.RoundingControl, new PartialEnumFileInfo("RoundingControl", Path.Combine(generatorOptions.RustDir, "enums.rs"), RustConstants.AttributeCopyEq));
			toPartialFileInfo.Add(TypeIds.OpKind, new PartialEnumFileInfo("OpKind", Path.Combine(generatorOptions.RustDir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.Instruction_MemoryFlags, new PartialEnumFileInfo("MemoryFlags", Path.Combine(generatorOptions.RustDir, "instruction.rs")));
			toPartialFileInfo.Add(TypeIds.Instruction_OpKindFlags, new PartialEnumFileInfo("OpKindFlags", Path.Combine(generatorOptions.RustDir, "instruction.rs")));
			toPartialFileInfo.Add(TypeIds.Instruction_CodeFlags, new PartialEnumFileInfo("CodeFlags", Path.Combine(generatorOptions.RustDir, "instruction.rs")));
			toPartialFileInfo.Add(TypeIds.VectorLength, new PartialEnumFileInfo("VectorLength", Path.Combine(generatorOptions.RustDir, "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeReprU8, RustConstants.FeatureDecoderOrEncoder }));
			toPartialFileInfo.Add(TypeIds.MandatoryPrefixByte, new PartialEnumFileInfo("MandatoryPrefixByte", Path.Combine(generatorOptions.RustDir, "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureDecoderOrEncoder }));
			toPartialFileInfo.Add(TypeIds.StateFlags, new PartialEnumFileInfo("StateFlags", Path.Combine(generatorOptions.RustDir, "decoder", "mod.rs")));
			toPartialFileInfo.Add(TypeIds.EncodingKind, new PartialEnumFileInfo("EncodingKind", Path.Combine(generatorOptions.RustDir, "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureDecoderOrEncoderOrInstrInfo }));
			toPartialFileInfo.Add(TypeIds.FlowControl, new PartialEnumFileInfo("FlowControl", Path.Combine(generatorOptions.RustDir, "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo }));
			toPartialFileInfo.Add(TypeIds.OpCodeOperandKind, new PartialEnumFileInfo("OpCodeOperandKind", Path.Combine(generatorOptions.RustDir, "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureEncoder, RustConstants.AttributeAllowNonCamelCaseTypes }));
		}

		public void Generate(EnumType enumType) {
			if (toPartialFileInfo.TryGetValue(enumType.TypeId, out var partialInfo)) {
				if (!(partialInfo is null))
					new FileUpdater(TargetLanguage.Rust, partialInfo.Id, partialInfo.Filename).Generate(writer => WriteEnum(writer, partialInfo, enumType));
			}
			else
				throw new InvalidOperationException();
		}

		void WriteEnum(FileWriter writer, PartialEnumFileInfo info, EnumType enumType) {
			if (enumType.IsFlags) {
				var attrs = enumType.IsPublic ? new[] { "#[allow(missing_copy_implementations)]" } : Array.Empty<string>();
				constantsWriter.Write(writer, enumType.ToConstantsType(ConstantKind.UInt32), attrs);
			}
			else
				WriteEnumCore(writer, info, enumType);
		}

		void WriteEnumCore(FileWriter writer, PartialEnumFileInfo info, EnumType enumType) {
			docWriter.Write(writer, enumType.Documentation, enumType.RawName);
			foreach (var attr in info.Attributes)
				writer.WriteLine(attr);
			if (enumType.IsPublic && enumType.IsMissingDocs)
				writer.WriteLine("#[allow(missing_docs)]");
			var pub = enumType.IsPublic ? "pub " : "pub(crate) ";
			writer.WriteLine($"{pub}enum {enumType.Name(idConverter)} {{");
			writer.Indent();

			uint expectedValue = 0;
			foreach (var value in enumType.Values) {
				docWriter.Write(writer, value.Documentation, enumType.RawName);
				if (expectedValue != value.Value)
					writer.WriteLine($"{value.Name(idConverter)} = {value.Value},");
				else
					writer.WriteLine($"{value.Name(idConverter)},");
				expectedValue = value.Value + 1;
			}

			writer.Unindent();
			writer.WriteLine("}");
		}
	}
}
