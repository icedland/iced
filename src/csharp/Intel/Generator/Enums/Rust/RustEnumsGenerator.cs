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
using Generator.Documentation;
using Generator.Documentation.Rust;
using Generator.IO;

namespace Generator.Enums.Rust {
	[Generator(TargetLanguage.Rust, GeneratorNames.Enums)]
	sealed class RustEnumsGenerator : EnumsGenerator {
		readonly IdentifierConverter idConverter;
		readonly Dictionary<TypeId, PartialEnumFileInfo?> toPartialFileInfo;
		readonly RustDocCommentWriter docWriter;
		readonly DeprecatedWriter deprecatedWriter;
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

		public RustEnumsGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			idConverter = RustIdentifierConverter.Create();
			docWriter = new RustDocCommentWriter(idConverter);
			deprecatedWriter = new RustDeprecatedWriter(idConverter);
			constantsWriter = new RustConstantsWriter(genTypes, idConverter, docWriter, deprecatedWriter);

			var dir = generatorContext.RustDir;
			toPartialFileInfo = new Dictionary<TypeId, PartialEnumFileInfo?>();
			toPartialFileInfo.Add(TypeIds.Code, new PartialEnumFileInfo("Code", Path.Combine(dir, "code.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CodeSize, new PartialEnumFileInfo("CodeSize", Path.Combine(dir, "enums.rs"), RustConstants.AttributeCopyEqOrdHash));
			toPartialFileInfo.Add(TypeIds.ConditionCode, new PartialEnumFileInfo("ConditionCode", Path.Combine(dir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CpuidFeature, new PartialEnumFileInfo("CpuidFeature", Path.Combine(dir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CpuidFeatureInternal, new PartialEnumFileInfo("CpuidFeatureInternal", Path.Combine(dir, "info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.DecoderError, new PartialEnumFileInfo("DecoderError", Path.Combine(dir, "decoder", "mod.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive }));
			toPartialFileInfo.Add(TypeIds.DecoderOptions, new PartialEnumFileInfo("DecoderOptions", Path.Combine(dir, "decoder", "mod.rs")));
			toPartialFileInfo.Add(TypeIds.SerializedDataKind, new PartialEnumFileInfo("SerializedDataKind", Path.Combine(dir, "decoder", "table_de", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpCodeHandlerKind, new PartialEnumFileInfo("OpCodeHandlerKind", Path.Combine(dir, "decoder", "table_de", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.LegacyHandlerFlags, new PartialEnumFileInfo("LegacyHandlerFlags", Path.Combine(dir, "decoder", "enums.rs")));
			toPartialFileInfo.Add(TypeIds.EvexOpCodeHandlerKind, new PartialEnumFileInfo("EvexOpCodeHandlerKind", Path.Combine(dir, "decoder", "table_de", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureEvex }));
			toPartialFileInfo.Add(TypeIds.VexOpCodeHandlerKind, new PartialEnumFileInfo("VexOpCodeHandlerKind", Path.Combine(dir, "decoder", "table_de", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureVexOrXop }));
			toPartialFileInfo.Add(TypeIds.HandlerFlags, new PartialEnumFileInfo("HandlerFlags", Path.Combine(dir, "decoder", "mod.rs")));
			toPartialFileInfo.Add(TypeIds.MemorySize, new PartialEnumFileInfo("MemorySize", Path.Combine(dir, "memory_size.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.PseudoOpsKind, new PartialEnumFileInfo("PseudoOpsKind", Path.Combine(dir, "formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.Register, new PartialEnumFileInfo("Register", Path.Combine(dir, "register.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive }));
			toPartialFileInfo.Add(TypeIds.TupleType, new PartialEnumFileInfo("TupleType", Path.Combine(dir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive, RustConstants.FeatureDecoderOrEncoder, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.Mnemonic, new PartialEnumFileInfo("Mnemonic", Path.Combine(dir, "mnemonic.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive }));
			toPartialFileInfo.Add(TypeIds.FormatterFlowControl, new PartialEnumFileInfo("FormatterFlowControl", Path.Combine(dir, "formatter", "enums.rs"), RustConstants.AttributeCopyEq));
			toPartialFileInfo.Add(TypeIds.GasCtorKind, new PartialEnumFileInfo("CtorKind", Path.Combine(dir, "formatter", "gas", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.GasSizeOverride, new PartialEnumFileInfo("SizeOverride", Path.Combine(dir, "formatter", "gas", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.GasInstrOpInfoFlags, new PartialEnumFileInfo("InstrOpInfoFlags", Path.Combine(dir, "formatter", "gas", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.GasInstrOpKind, new PartialEnumFileInfo("InstrOpKind", Path.Combine(dir, "formatter", "gas", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.IntelCtorKind, new PartialEnumFileInfo("CtorKind", Path.Combine(dir, "formatter", "intel", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.IntelSizeOverride, new PartialEnumFileInfo("SizeOverride", Path.Combine(dir, "formatter", "intel", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.IntelBranchSizeInfo, new PartialEnumFileInfo("BranchSizeInfo", Path.Combine(dir, "formatter", "intel", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.IntelInstrOpInfoFlags, new PartialEnumFileInfo("InstrOpInfoFlags", Path.Combine(dir, "formatter", "intel", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.IntelInstrOpKind, new PartialEnumFileInfo("InstrOpKind", Path.Combine(dir, "formatter", "intel", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.MasmCtorKind, new PartialEnumFileInfo("CtorKind", Path.Combine(dir, "formatter", "masm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.MasmInstrOpInfoFlags, new PartialEnumFileInfo("InstrOpInfoFlags", Path.Combine(dir, "formatter", "masm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.MasmInstrOpKind, new PartialEnumFileInfo("InstrOpKind", Path.Combine(dir, "formatter", "masm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.MasmSymbolTestFlags, new PartialEnumFileInfo("SymbolTestFlags", Path.Combine(dir, "formatter", "masm", "tests", "sym_opts.rs")));
			toPartialFileInfo.Add(TypeIds.NasmCtorKind, new PartialEnumFileInfo("CtorKind", Path.Combine(dir, "formatter", "nasm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.NasmSignExtendInfo, new PartialEnumFileInfo("SignExtendInfo", Path.Combine(dir, "formatter", "nasm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.NasmSizeOverride, new PartialEnumFileInfo("SizeOverride", Path.Combine(dir, "formatter", "nasm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.NasmBranchSizeInfo, new PartialEnumFileInfo("BranchSizeInfo", Path.Combine(dir, "formatter", "nasm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.NasmInstrOpInfoFlags, new PartialEnumFileInfo("InstrOpInfoFlags", Path.Combine(dir, "formatter", "nasm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.NasmInstrOpKind, new PartialEnumFileInfo("InstrOpKind", Path.Combine(dir, "formatter", "nasm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.NasmMemorySizeInfo, new PartialEnumFileInfo("MemorySizeInfo", Path.Combine(dir, "formatter", "nasm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.NasmFarMemorySizeInfo, new PartialEnumFileInfo("FarMemorySizeInfo", Path.Combine(dir, "formatter", "nasm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.MandatoryPrefix, new PartialEnumFileInfo("MandatoryPrefix", Path.Combine(dir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.FeatureOpCodeInfo }));
			toPartialFileInfo.Add(TypeIds.OpCodeTableKind, new PartialEnumFileInfo("OpCodeTableKind", Path.Combine(dir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.FeatureOpCodeInfo, RustConstants.AttributeNonExhaustive }));
			toPartialFileInfo.Add(TypeIds.RoundingControl, new PartialEnumFileInfo("RoundingControl", Path.Combine(dir, "enums.rs"), RustConstants.AttributeCopyEqOrdHash));
			toPartialFileInfo.Add(TypeIds.OpKind, new PartialEnumFileInfo("OpKind", Path.Combine(dir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.Instruction_MemoryFlags, new PartialEnumFileInfo("MemoryFlags", Path.Combine(dir, "instruction.rs")));
			toPartialFileInfo.Add(TypeIds.Instruction_OpKindFlags, new PartialEnumFileInfo("OpKindFlags", Path.Combine(dir, "instruction.rs")));
			toPartialFileInfo.Add(TypeIds.Instruction_CodeFlags, new PartialEnumFileInfo("CodeFlags", Path.Combine(dir, "instruction.rs")));
			toPartialFileInfo.Add(TypeIds.VectorLength, new PartialEnumFileInfo("VectorLength", Path.Combine(dir, "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureDecoderOrEncoder }));
			toPartialFileInfo.Add(TypeIds.MandatoryPrefixByte, new PartialEnumFileInfo("MandatoryPrefixByte", Path.Combine(dir, "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureDecoderOrEncoder }));
			toPartialFileInfo.Add(TypeIds.OpSize, new PartialEnumFileInfo("OpSize", Path.Combine(dir, "decoder", "mod.rs"), RustConstants.AttributeCopyEq));
			toPartialFileInfo.Add(TypeIds.StateFlags, new PartialEnumFileInfo("StateFlags", Path.Combine(dir, "decoder", "mod.rs")));
			toPartialFileInfo.Add(TypeIds.EncodingKind, new PartialEnumFileInfo("EncodingKind", Path.Combine(dir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive, RustConstants.FeatureDecoderOrEncoderOrInstrInfo }));
			toPartialFileInfo.Add(TypeIds.FlowControl, new PartialEnumFileInfo("FlowControl", Path.Combine(dir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.FeatureInstrInfo }));
			toPartialFileInfo.Add(TypeIds.OpCodeOperandKind, new PartialEnumFileInfo("OpCodeOperandKind", Path.Combine(dir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive, RustConstants.FeatureOpCodeInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.RflagsBits, new PartialEnumFileInfo("RflagsBits", Path.Combine(dir, "enums.rs"), RustConstants.FeatureInstrInfo));
			toPartialFileInfo.Add(TypeIds.CodeInfo, new PartialEnumFileInfo("CodeInfo", Path.Combine(dir, "info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.RflagsInfo, new PartialEnumFileInfo("RflagsInfo", Path.Combine(dir, "info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpInfo0, new PartialEnumFileInfo("OpInfo0", Path.Combine(dir, "info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpInfo1, new PartialEnumFileInfo("OpInfo1", Path.Combine(dir, "info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpInfo2, new PartialEnumFileInfo("OpInfo2", Path.Combine(dir, "info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpInfo3, new PartialEnumFileInfo("OpInfo3", Path.Combine(dir, "info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpInfo4, new PartialEnumFileInfo("OpInfo4", Path.Combine(dir, "info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.InfoFlags1, new PartialEnumFileInfo("InfoFlags1", Path.Combine(dir, "info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo }));
			toPartialFileInfo.Add(TypeIds.InfoFlags2, new PartialEnumFileInfo("InfoFlags2", Path.Combine(dir, "info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo }));
			toPartialFileInfo.Add(TypeIds.OpAccess, new PartialEnumFileInfo("OpAccess", Path.Combine(dir, "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.FeatureInstrInfo }));
			toPartialFileInfo.Add(TypeIds.MemorySizeFlags, new PartialEnumFileInfo("MemorySizeFlags", Path.Combine(dir, "info", "tests", "constants.rs"), new[] { RustConstants.AttributeCopyEqOrdHash }));
			toPartialFileInfo.Add(TypeIds.RegisterFlags, new PartialEnumFileInfo("RegisterFlags", Path.Combine(dir, "info", "tests", "constants.rs"), new[] { RustConstants.AttributeCopyEqOrdHash }));
			toPartialFileInfo.Add(TypeIds.LegacyOpCodeTable, new PartialEnumFileInfo("LegacyOpCodeTable", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.VexOpCodeTable, new PartialEnumFileInfo("VexOpCodeTable", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureVex }));
			toPartialFileInfo.Add(TypeIds.XopOpCodeTable, new PartialEnumFileInfo("XopOpCodeTable", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureXop }));
			toPartialFileInfo.Add(TypeIds.EvexOpCodeTable, new PartialEnumFileInfo("EvexOpCodeTable", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureEvex }));
			toPartialFileInfo.Add(TypeIds.Encodable, new PartialEnumFileInfo("Encodable", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpCodeHandlerFlags, new PartialEnumFileInfo("OpCodeHandlerFlags", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.FeatureEncoder }));
			toPartialFileInfo.Add(TypeIds.LegacyOpKind, new PartialEnumFileInfo("LegacyOpKind", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.VexOpKind, new PartialEnumFileInfo("VexOpKind", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureVex }));
			toPartialFileInfo.Add(TypeIds.XopOpKind, new PartialEnumFileInfo("XopOpKind", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureXop }));
			toPartialFileInfo.Add(TypeIds.EvexOpKind, new PartialEnumFileInfo("EvexOpKind", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureEvex }));
			toPartialFileInfo.Add(TypeIds.OperandSize, new PartialEnumFileInfo("OperandSize", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.AddressSize, new PartialEnumFileInfo("AddressSize", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.VexVectorLength, new PartialEnumFileInfo("VexVectorLength", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureVex }));
			toPartialFileInfo.Add(TypeIds.XopVectorLength, new PartialEnumFileInfo("XopVectorLength", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureXop }));
			toPartialFileInfo.Add(TypeIds.EvexVectorLength, new PartialEnumFileInfo("EvexVectorLength", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureEvex }));
			toPartialFileInfo.Add(TypeIds.DisplSize, new PartialEnumFileInfo("DisplSize", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.ImmSize, new PartialEnumFileInfo("ImmSize", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.EncoderFlags, new PartialEnumFileInfo("EncoderFlags", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.EncFlags1, new PartialEnumFileInfo("EncFlags1", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.LegacyFlags3, new PartialEnumFileInfo("LegacyFlags3", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.VexFlags3, new PartialEnumFileInfo("VexFlags3", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureVex }));
			toPartialFileInfo.Add(TypeIds.XopFlags3, new PartialEnumFileInfo("XopFlags3", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureXop }));
			toPartialFileInfo.Add(TypeIds.EvexFlags3, new PartialEnumFileInfo("EvexFlags3", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureEvex }));
			toPartialFileInfo.Add(TypeIds.AllowedPrefixes, new PartialEnumFileInfo("AllowedPrefixes", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.LegacyFlags, new PartialEnumFileInfo("LegacyFlags", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.VexFlags, new PartialEnumFileInfo("VexFlags", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureVex }));
			toPartialFileInfo.Add(TypeIds.XopFlags, new PartialEnumFileInfo("XopFlags", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureXop }));
			toPartialFileInfo.Add(TypeIds.EvexFlags, new PartialEnumFileInfo("EvexFlags", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureEvex }));
			toPartialFileInfo.Add(TypeIds.D3nowFlags, new PartialEnumFileInfo("D3nowFlags", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureD3now }));
			toPartialFileInfo.Add(TypeIds.WBit, new PartialEnumFileInfo("WBit", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureVexOrXopOrEvex }));
			toPartialFileInfo.Add(TypeIds.LKind, new PartialEnumFileInfo("LKind", Path.Combine(dir, "encoder", "op_code_fmt.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpCodeFlags, new PartialEnumFileInfo("Flags", Path.Combine(dir, "encoder", "op_code.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.RepPrefixKind, new PartialEnumFileInfo("RepPrefixKind", Path.Combine(dir, "encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive }));
			toPartialFileInfo.Add(TypeIds.RelocKind, new PartialEnumFileInfo("RelocKind", Path.Combine(dir, "block_enc", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive }));
			toPartialFileInfo.Add(TypeIds.BlockEncoderOptions, new PartialEnumFileInfo("BlockEncoderOptions", Path.Combine(dir, "block_enc", "enums.rs"), RustConstants.AttributeCopyEqOrdHash));
			toPartialFileInfo.Add(TypeIds.NumberBase, new PartialEnumFileInfo("NumberBase", Path.Combine(dir, "formatter", "enums.rs"), RustConstants.AttributeCopyEqOrdHash));
			toPartialFileInfo.Add(TypeIds.MemorySizeOptions, new PartialEnumFileInfo("MemorySizeOptions", Path.Combine(dir, "formatter", "enums.rs"), RustConstants.AttributeCopyEqOrdHash));
			toPartialFileInfo.Add(TypeIds.FormatMnemonicOptions, new PartialEnumFileInfo("FormatMnemonicOptions", Path.Combine(dir, "formatter", "enums.rs"), RustConstants.AttributeCopyEqOrdHash));
			toPartialFileInfo.Add(TypeIds.PrefixKind, new PartialEnumFileInfo("PrefixKind", Path.Combine(dir, "formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive }));
			toPartialFileInfo.Add(TypeIds.DecoratorKind, new PartialEnumFileInfo("DecoratorKind", Path.Combine(dir, "formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive }));
			toPartialFileInfo.Add(TypeIds.NumberKind, new PartialEnumFileInfo("NumberKind", Path.Combine(dir, "formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive }));
			toPartialFileInfo.Add(TypeIds.FormatterTextKind, new PartialEnumFileInfo("FormatterTextKind", Path.Combine(dir, "formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive }));
			toPartialFileInfo.Add(TypeIds.SymbolFlags, new PartialEnumFileInfo("SymbolFlags", Path.Combine(dir, "formatter", "enums.rs"), RustConstants.AttributeCopyEqOrdHash));
			toPartialFileInfo.Add(TypeIds.CC_b, new PartialEnumFileInfo("CC_b", Path.Combine(dir, "formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_ae, new PartialEnumFileInfo("CC_ae", Path.Combine(dir, "formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_e, new PartialEnumFileInfo("CC_e", Path.Combine(dir, "formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_ne, new PartialEnumFileInfo("CC_ne", Path.Combine(dir, "formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_be, new PartialEnumFileInfo("CC_be", Path.Combine(dir, "formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_a, new PartialEnumFileInfo("CC_a", Path.Combine(dir, "formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_p, new PartialEnumFileInfo("CC_p", Path.Combine(dir, "formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_np, new PartialEnumFileInfo("CC_np", Path.Combine(dir, "formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_l, new PartialEnumFileInfo("CC_l", Path.Combine(dir, "formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_ge, new PartialEnumFileInfo("CC_ge", Path.Combine(dir, "formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_le, new PartialEnumFileInfo("CC_le", Path.Combine(dir, "formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_g, new PartialEnumFileInfo("CC_g", Path.Combine(dir, "formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OptionsProps, new PartialEnumFileInfo("OptionsProps", Path.Combine(dir, "formatter", "tests", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
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
				if (enumType.IsPublic) {
					attrs.Add(RustConstants.AttributeAllowMissingCopyImplementations);
					attrs.Add(RustConstants.AttributeAllowMissingDebugImplementations);
				}
				attrs.AddRange(info.Attributes.Where(a => a.StartsWith(RustConstants.FeaturePrefix)));
				constantsWriter.Write(writer, enumType.ToConstantsType(ConstantKind.UInt32), attrs.ToArray());
			}
			else
				WriteEnumCore(writer, info, enumType);
		}

		void WriteEnumCore(FileWriter writer, PartialEnumFileInfo info, EnumType enumType) {
			docWriter.WriteSummary(writer, enumType.Documentation, enumType.RawName);
			var enumTypeName = enumType.Name(idConverter);
			foreach (var attr in info.Attributes)
				writer.WriteLine(attr);
			if (enumType.IsPublic && enumType.IsMissingDocs)
				writer.WriteLine(RustConstants.AttributeAllowMissingDocs);
			if (!enumType.IsPublic)
				writer.WriteLine(RustConstants.AttributeAllowDeadCode);
			var pub = enumType.IsPublic ? "pub " : "pub(crate) ";
			writer.WriteLine($"{pub}enum {enumTypeName} {{");
			using (writer.Indent()) {
				uint expectedValue = 0;
				foreach (var value in enumType.Values) {
					docWriter.WriteSummary(writer, value.Documentation, enumType.RawName);
					deprecatedWriter.WriteDeprecated(writer, value);
					if (expectedValue != value.Value || enumType.IsPublic)
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
				writer.WriteLine(RustConstants.AttributeInline);
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
