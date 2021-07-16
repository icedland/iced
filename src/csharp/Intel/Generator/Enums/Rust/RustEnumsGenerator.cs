// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using System.Linq;
using Generator.Constants;
using Generator.Constants.Rust;
using Generator.Documentation;
using Generator.Documentation.Rust;
using Generator.IO;

namespace Generator.Enums.Rust {
	[Generator(TargetLanguage.Rust)]
	sealed class RustEnumsGenerator : EnumsGenerator {
		readonly IdentifierConverter idConverter;
		readonly Dictionary<TypeId, PartialEnumFileInfo?> toPartialFileInfo;
		readonly RustDocCommentWriter docWriter;
		readonly RustDeprecatedWriter deprecatedWriter;
		readonly RustConstantsWriter constantsWriter;

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

		public RustEnumsGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			idConverter = RustIdentifierConverter.Create();
			docWriter = new RustDocCommentWriter(idConverter);
			deprecatedWriter = new RustDeprecatedWriter(idConverter);
			constantsWriter = new RustConstantsWriter(genTypes, idConverter, docWriter, deprecatedWriter);

			var dirs = generatorContext.Types.Dirs;
			toPartialFileInfo = new Dictionary<TypeId, PartialEnumFileInfo?>();
			toPartialFileInfo.Add(TypeIds.Code, new PartialEnumFileInfo("Code", dirs.GetRustFilename("code.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CodeSize, new PartialEnumFileInfo("CodeSize", dirs.GetRustFilename("enums.rs"), RustConstants.AttributeCopyEqOrdHash));
			toPartialFileInfo.Add(TypeIds.ConditionCode, new PartialEnumFileInfo("ConditionCode", dirs.GetRustFilename("enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CpuidFeature, new PartialEnumFileInfo("CpuidFeature", dirs.GetRustFilename("enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CpuidFeatureInternal, new PartialEnumFileInfo("CpuidFeatureInternal", dirs.GetRustFilename("info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.DecoderError, new PartialEnumFileInfo("DecoderError", dirs.GetRustFilename("decoder.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive }));
			toPartialFileInfo.Add(TypeIds.DecoderOptions, new PartialEnumFileInfo("DecoderOptions", dirs.GetRustFilename("decoder.rs")));
			toPartialFileInfo.Add(TypeIds.DecoderTestOptions, new PartialEnumFileInfo("DecoderTestOptions", dirs.GetRustFilename("decoder", "tests", "enums.rs")));
			toPartialFileInfo.Add(TypeIds.SerializedDataKind, new PartialEnumFileInfo("SerializedDataKind", dirs.GetRustFilename("decoder", "table_de", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpCodeHandlerKind, new PartialEnumFileInfo("OpCodeHandlerKind", dirs.GetRustFilename("decoder", "table_de", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.LegacyHandlerFlags, new PartialEnumFileInfo("LegacyHandlerFlags", dirs.GetRustFilename("decoder", "enums.rs")));
			toPartialFileInfo.Add(TypeIds.EvexOpCodeHandlerKind, new PartialEnumFileInfo("EvexOpCodeHandlerKind", dirs.GetRustFilename("decoder", "table_de", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureEvex }));
			toPartialFileInfo.Add(TypeIds.VexOpCodeHandlerKind, new PartialEnumFileInfo("VexOpCodeHandlerKind", dirs.GetRustFilename("decoder", "table_de", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureVexOrXop }));
			toPartialFileInfo.Add(TypeIds.HandlerFlags, new PartialEnumFileInfo("HandlerFlags", dirs.GetRustFilename("decoder.rs")));
			toPartialFileInfo.Add(TypeIds.MemorySize, new PartialEnumFileInfo("MemorySize", dirs.GetRustFilename("memory_size.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.PseudoOpsKind, new PartialEnumFileInfo("PseudoOpsKind", dirs.GetRustFilename("formatter", "enums_shared.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.Register, new PartialEnumFileInfo("Register", dirs.GetRustFilename("register.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive }));
			toPartialFileInfo.Add(TypeIds.TupleType, new PartialEnumFileInfo("TupleType", dirs.GetRustFilename("enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive, RustConstants.FeatureDecoderOrEncoder, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.Mnemonic, new PartialEnumFileInfo("Mnemonic", dirs.GetRustFilename("mnemonic.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.FormatterFlowControl, new PartialEnumFileInfo("FormatterFlowControl", dirs.GetRustFilename("formatter", "enums.rs"), RustConstants.AttributeCopyEq));
			toPartialFileInfo.Add(TypeIds.GasCtorKind, new PartialEnumFileInfo("CtorKind", dirs.GetRustFilename("formatter", "gas", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.GasSizeOverride, new PartialEnumFileInfo("SizeOverride", dirs.GetRustFilename("formatter", "gas", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.GasInstrOpInfoFlags, new PartialEnumFileInfo("InstrOpInfoFlags", dirs.GetRustFilename("formatter", "gas", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.GasInstrOpKind, new PartialEnumFileInfo("InstrOpKind", dirs.GetRustFilename("formatter", "gas", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.IntelCtorKind, new PartialEnumFileInfo("CtorKind", dirs.GetRustFilename("formatter", "intel", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.IntelSizeOverride, new PartialEnumFileInfo("SizeOverride", dirs.GetRustFilename("formatter", "intel", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.IntelBranchSizeInfo, new PartialEnumFileInfo("BranchSizeInfo", dirs.GetRustFilename("formatter", "intel", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.IntelInstrOpInfoFlags, new PartialEnumFileInfo("InstrOpInfoFlags", dirs.GetRustFilename("formatter", "intel", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.IntelInstrOpKind, new PartialEnumFileInfo("InstrOpKind", dirs.GetRustFilename("formatter", "intel", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.MasmCtorKind, new PartialEnumFileInfo("CtorKind", dirs.GetRustFilename("formatter", "masm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.MasmInstrOpInfoFlags, new PartialEnumFileInfo("InstrOpInfoFlags", dirs.GetRustFilename("formatter", "masm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.MasmInstrOpKind, new PartialEnumFileInfo("InstrOpKind", dirs.GetRustFilename("formatter", "masm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.MasmSymbolTestFlags, new PartialEnumFileInfo("SymbolTestFlags", dirs.GetRustFilename("formatter", "masm", "tests", "sym_opts.rs")));
			toPartialFileInfo.Add(TypeIds.NasmCtorKind, new PartialEnumFileInfo("CtorKind", dirs.GetRustFilename("formatter", "nasm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.NasmSignExtendInfo, new PartialEnumFileInfo("SignExtendInfo", dirs.GetRustFilename("formatter", "nasm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.NasmSizeOverride, new PartialEnumFileInfo("SizeOverride", dirs.GetRustFilename("formatter", "nasm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.NasmBranchSizeInfo, new PartialEnumFileInfo("BranchSizeInfo", dirs.GetRustFilename("formatter", "nasm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.NasmInstrOpInfoFlags, new PartialEnumFileInfo("InstrOpInfoFlags", dirs.GetRustFilename("formatter", "nasm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.NasmInstrOpKind, new PartialEnumFileInfo("InstrOpKind", dirs.GetRustFilename("formatter", "nasm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.NasmMemorySizeInfo, new PartialEnumFileInfo("MemorySizeInfo", dirs.GetRustFilename("formatter", "nasm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.NasmFarMemorySizeInfo, new PartialEnumFileInfo("FarMemorySizeInfo", dirs.GetRustFilename("formatter", "nasm", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.FastFmtFlags, new PartialEnumFileInfo("FastFmtFlags", dirs.GetRustFilename("formatter", "fast", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.MandatoryPrefix, new PartialEnumFileInfo("MandatoryPrefix", dirs.GetRustFilename("enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.FeatureOpCodeInfo }));
			toPartialFileInfo.Add(TypeIds.OpCodeTableKind, new PartialEnumFileInfo("OpCodeTableKind", dirs.GetRustFilename("enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.FeatureOpCodeInfo, RustConstants.AttributeNonExhaustive }));
			toPartialFileInfo.Add(TypeIds.RoundingControl, new PartialEnumFileInfo("RoundingControl", dirs.GetRustFilename("enums.rs"), RustConstants.AttributeCopyEqOrdHash));
			toPartialFileInfo.Add(TypeIds.OpKind, new PartialEnumFileInfo("OpKind", dirs.GetRustFilename("enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.InstrFlags1, new PartialEnumFileInfo("InstrFlags1", dirs.GetRustFilename("instruction.rs")));
			toPartialFileInfo.Add(TypeIds.VectorLength, new PartialEnumFileInfo("VectorLength", dirs.GetRustFilename("enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureDecoderOrEncoder, RustConstants.AttrReprU32 }));
			toPartialFileInfo.Add(TypeIds.MandatoryPrefixByte, new PartialEnumFileInfo("MandatoryPrefixByte", dirs.GetRustFilename("enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureEncoder }));
			toPartialFileInfo.Add(TypeIds.OpSize, new PartialEnumFileInfo("OpSize", dirs.GetRustFilename("decoder.rs"), RustConstants.AttributeCopyEq));
			toPartialFileInfo.Add(TypeIds.StateFlags, new PartialEnumFileInfo("StateFlags", dirs.GetRustFilename("decoder.rs")));
			toPartialFileInfo.Add(TypeIds.EncodingKind, new PartialEnumFileInfo("EncodingKind", dirs.GetRustFilename("enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive, RustConstants.FeatureDecoderOrEncoderOrInstrInfo }));
			toPartialFileInfo.Add(TypeIds.FlowControl, new PartialEnumFileInfo("FlowControl", dirs.GetRustFilename("enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.FeatureInstrInfo }));
			toPartialFileInfo.Add(TypeIds.OpCodeOperandKind, new PartialEnumFileInfo("OpCodeOperandKind", dirs.GetRustFilename("enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive, RustConstants.FeatureOpCodeInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.RflagsBits, new PartialEnumFileInfo("RflagsBits", dirs.GetRustFilename("enums.rs"), RustConstants.FeatureInstrInfo));
			toPartialFileInfo.Add(TypeIds.ImpliedAccess, new PartialEnumFileInfo("ImpliedAccess", dirs.GetRustFilename("info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.RflagsInfo, new PartialEnumFileInfo("RflagsInfo", dirs.GetRustFilename("info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpInfo0, new PartialEnumFileInfo("OpInfo0", dirs.GetRustFilename("info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpInfo1, new PartialEnumFileInfo("OpInfo1", dirs.GetRustFilename("info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpInfo2, new PartialEnumFileInfo("OpInfo2", dirs.GetRustFilename("info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpInfo3, new PartialEnumFileInfo("OpInfo3", dirs.GetRustFilename("info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpInfo4, new PartialEnumFileInfo("OpInfo4", dirs.GetRustFilename("info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.InfoFlags1, new PartialEnumFileInfo("InfoFlags1", dirs.GetRustFilename("info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo }));
			toPartialFileInfo.Add(TypeIds.InfoFlags2, new PartialEnumFileInfo("InfoFlags2", dirs.GetRustFilename("info", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.FeatureInstrInfo }));
			toPartialFileInfo.Add(TypeIds.OpAccess, new PartialEnumFileInfo("OpAccess", dirs.GetRustFilename("enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.FeatureInstrInfo }));
			toPartialFileInfo.Add(TypeIds.MemorySizeFlags, new PartialEnumFileInfo("MemorySizeFlags", dirs.GetRustFilename("info", "tests", "constants.rs"), new[] { RustConstants.AttributeCopyEqOrdHash }));
			toPartialFileInfo.Add(TypeIds.RegisterFlags, new PartialEnumFileInfo("RegisterFlags", dirs.GetRustFilename("info", "tests", "constants.rs"), new[] { RustConstants.AttributeCopyEqOrdHash }));
			toPartialFileInfo.Add(TypeIds.LegacyOpCodeTable, new PartialEnumFileInfo("LegacyOpCodeTable", dirs.GetRustFilename("encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.VexOpCodeTable, new PartialEnumFileInfo("VexOpCodeTable", dirs.GetRustFilename("encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureVex }));
			toPartialFileInfo.Add(TypeIds.XopOpCodeTable, new PartialEnumFileInfo("XopOpCodeTable", dirs.GetRustFilename("encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureXop }));
			toPartialFileInfo.Add(TypeIds.EvexOpCodeTable, new PartialEnumFileInfo("EvexOpCodeTable", dirs.GetRustFilename("encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureEvex }));
			toPartialFileInfo.Add(TypeIds.DisplSize, new PartialEnumFileInfo("DisplSize", dirs.GetRustFilename("encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.ImmSize, new PartialEnumFileInfo("ImmSize", dirs.GetRustFilename("encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.EncoderFlags, new PartialEnumFileInfo("EncoderFlags", dirs.GetRustFilename("encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.EncFlags1, new PartialEnumFileInfo("EncFlags1", dirs.GetRustFilename("encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.EncFlags2, new PartialEnumFileInfo("EncFlags2", dirs.GetRustFilename("encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.EncFlags3, new PartialEnumFileInfo("EncFlags3", dirs.GetRustFilename("encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OpCodeInfoFlags1, new PartialEnumFileInfo("OpCodeInfoFlags1", dirs.GetRustFilename("encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureOpCodeInfo }));
			toPartialFileInfo.Add(TypeIds.OpCodeInfoFlags2, new PartialEnumFileInfo("OpCodeInfoFlags2", dirs.GetRustFilename("encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureOpCodeInfo }));
			toPartialFileInfo.Add(TypeIds.DecOptionValue, new PartialEnumFileInfo("DecOptionValue", dirs.GetRustFilename("encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureOpCodeInfo }));
			toPartialFileInfo.Add(TypeIds.InstrStrFmtOption, new PartialEnumFileInfo("InstrStrFmtOption", dirs.GetRustFilename("encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureOpCodeInfo }));
			toPartialFileInfo.Add(TypeIds.WBit, new PartialEnumFileInfo("WBit", dirs.GetRustFilename("encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureVexOrXopOrEvex }));
			toPartialFileInfo.Add(TypeIds.LBit, new PartialEnumFileInfo("LBit", dirs.GetRustFilename("encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes, RustConstants.FeatureVexOrXopOrEvex }));
			toPartialFileInfo.Add(TypeIds.LKind, new PartialEnumFileInfo("LKind", dirs.GetRustFilename("encoder", "op_code_fmt.rs"), new[] { RustConstants.AttributeCopyEq, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.RepPrefixKind, new PartialEnumFileInfo("RepPrefixKind", dirs.GetRustFilename("encoder", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive }));
			toPartialFileInfo.Add(TypeIds.RelocKind, new PartialEnumFileInfo("RelocKind", dirs.GetRustFilename("block_enc", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive }));
			toPartialFileInfo.Add(TypeIds.BlockEncoderOptions, new PartialEnumFileInfo("BlockEncoderOptions", dirs.GetRustFilename("block_enc", "enums.rs"), RustConstants.AttributeCopyEqOrdHash));
			toPartialFileInfo.Add(TypeIds.NumberBase, new PartialEnumFileInfo("NumberBase", dirs.GetRustFilename("formatter", "enums.rs"), RustConstants.AttributeCopyEqOrdHash));
			toPartialFileInfo.Add(TypeIds.MemorySizeOptions, new PartialEnumFileInfo("MemorySizeOptions", dirs.GetRustFilename("formatter", "enums_shared.rs"), RustConstants.AttributeCopyEqOrdHash));
			toPartialFileInfo.Add(TypeIds.FormatMnemonicOptions, new PartialEnumFileInfo("FormatMnemonicOptions", dirs.GetRustFilename("formatter", "enums.rs"), RustConstants.AttributeCopyEqOrdHash));
			toPartialFileInfo.Add(TypeIds.PrefixKind, new PartialEnumFileInfo("PrefixKind", dirs.GetRustFilename("formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive }));
			toPartialFileInfo.Add(TypeIds.DecoratorKind, new PartialEnumFileInfo("DecoratorKind", dirs.GetRustFilename("formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive }));
			toPartialFileInfo.Add(TypeIds.NumberKind, new PartialEnumFileInfo("NumberKind", dirs.GetRustFilename("formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive }));
			toPartialFileInfo.Add(TypeIds.FormatterTextKind, new PartialEnumFileInfo("FormatterTextKind", dirs.GetRustFilename("formatter", "enums_shared.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeNonExhaustive }));
			toPartialFileInfo.Add(TypeIds.SymbolFlags, new PartialEnumFileInfo("SymbolFlags", dirs.GetRustFilename("formatter", "enums_shared.rs"), RustConstants.AttributeCopyEqOrdHash));
			toPartialFileInfo.Add(TypeIds.CC_b, new PartialEnumFileInfo("CC_b", dirs.GetRustFilename("formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_ae, new PartialEnumFileInfo("CC_ae", dirs.GetRustFilename("formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_e, new PartialEnumFileInfo("CC_e", dirs.GetRustFilename("formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_ne, new PartialEnumFileInfo("CC_ne", dirs.GetRustFilename("formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_be, new PartialEnumFileInfo("CC_be", dirs.GetRustFilename("formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_a, new PartialEnumFileInfo("CC_a", dirs.GetRustFilename("formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_p, new PartialEnumFileInfo("CC_p", dirs.GetRustFilename("formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_np, new PartialEnumFileInfo("CC_np", dirs.GetRustFilename("formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_l, new PartialEnumFileInfo("CC_l", dirs.GetRustFilename("formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_ge, new PartialEnumFileInfo("CC_ge", dirs.GetRustFilename("formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_le, new PartialEnumFileInfo("CC_le", dirs.GetRustFilename("formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.CC_g, new PartialEnumFileInfo("CC_g", dirs.GetRustFilename("formatter", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
			toPartialFileInfo.Add(TypeIds.OptionsProps, new PartialEnumFileInfo("OptionsProps", dirs.GetRustFilename("formatter", "tests", "enums.rs"), new[] { RustConstants.AttributeCopyEqOrdHash, RustConstants.AttributeAllowNonCamelCaseTypes }));
		}

		public override void Generate(EnumType enumType) {
			if (toPartialFileInfo.TryGetValue(enumType.TypeId, out var partialInfo)) {
				if (partialInfo is not null)
					new FileUpdater(TargetLanguage.Rust, partialInfo.Id, partialInfo.Filename).Generate(writer => WriteEnum(writer, partialInfo, enumType));
			}
		}

		void WriteEnum(FileWriter writer, PartialEnumFileInfo info, EnumType enumType) {
			if (enumType.IsFlags) {
				var attrs = new List<string>();
				if (enumType.IsPublic) {
					attrs.Add(RustConstants.AttributeAllowMissingCopyImplementations);
					attrs.Add(RustConstants.AttributeAllowMissingDebugImplementations);
				}
				attrs.AddRange(info.Attributes.Where(a => a.StartsWith(RustConstants.FeaturePrefix, StringComparison.Ordinal)));
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
			if (enumType.IsPublic)
				writer.WriteLine("#[cfg_attr(feature = \"__internal_serde\", derive(Serialize, Deserialize))]");
			if (enumType.IsPublic && enumType.IsMissingDocs)
				writer.WriteLine(RustConstants.AttributeAllowMissingDocs);
			if (!enumType.IsPublic)
				writer.WriteLine(RustConstants.AttributeAllowDeadCode);
			var pub = enumType.IsPublic ? "pub " : "pub(crate) ";
			writer.WriteLine($"{pub}enum {enumTypeName} {{");
			// Identical enum values aren't allowed so just remove them
			var enumValues = enumType.Values.Where(a => !a.DeprecatedInfo.IsDeprecatedAndRenamed).ToArray();
			using (writer.Indent()) {
				uint expectedValue = 0;
				foreach (var value in enumValues) {
					docWriter.WriteSummary(writer, value.Documentation, enumType.RawName);
					deprecatedWriter.WriteDeprecated(writer, value, "__internal_serde");
					if (expectedValue != value.Value || enumType.IsPublic)
						writer.WriteLine($"{value.Name(idConverter)} = {value.Value},");
					else
						writer.WriteLine($"{value.Name(idConverter)},");
					expectedValue = value.Value + 1;
				}
			}
			writer.WriteLine("}");

			var arrayName = idConverter.Constant("GenDebug" + enumType.RawName);
			var feature = info.Attributes.FirstOrDefault(a => a.StartsWith(RustConstants.FeaturePrefix, StringComparison.Ordinal) && a.Contains("(feature", StringComparison.Ordinal));
			if (feature is not null)
				writer.WriteLine(feature);
			writer.WriteLine(RustConstants.AttributeNoRustFmt);
			writer.WriteLine($"static {arrayName}: [&str; {enumValues.Length}] = [");
			using (writer.Indent()) {
				foreach (var value in enumValues)
					writer.WriteLine($"\"{value.Name(idConverter)}\",");
			}
			writer.WriteLine("];");

			// #[derive(Debug)] isn't used since it generates a big switch statement. This code
			// uses a simple array lookup which has very little code. For small enums the default
			// implementation might be better though.
			if (feature is not null)
				writer.WriteLine(feature);
			writer.WriteLine($"impl fmt::Debug for {enumTypeName} {{");
			using (writer.Indent()) {
				writer.WriteLine(RustConstants.AttributeInline);
				writer.WriteLine($"fn fmt(&self, f: &mut fmt::Formatter<'_>) -> fmt::Result {{");
				using (writer.Indent())
					writer.WriteLine($"write!(f, \"{{}}\", {arrayName}[*self as usize])");
				writer.WriteLine("}");
			}
			writer.WriteLine("}");

			if (feature is not null)
				writer.WriteLine(feature);
			writer.WriteLine($"impl Default for {enumTypeName} {{");
			using (writer.Indent()) {
				writer.WriteLine(RustConstants.AttributeMustUse);
				writer.WriteLine(RustConstants.AttributeInline);
				writer.WriteLine("fn default() -> Self {");
				using (writer.Indent()) {
					var defaultValue = enumValues[0];
					// The first one should always have value 0 (eg. "None" field), and if the first one doesn't
					// have value 0, there must be no other enum fields == 0.
					if (defaultValue.Value != 0 && enumValues.Any(a => a.Value == 0))
						throw new InvalidOperationException();
					writer.WriteLine($"{enumTypeName}::{defaultValue.Name(idConverter)}");
				}
				writer.WriteLine("}");
			}
			writer.WriteLine("}");

			if (enumType.IsPublic) {
				var enumIterType = $"{enumTypeName}Iterator";
				var icedConstValue = "IcedConstants::" + idConverter.Constant(IcedConstants.GetEnumCountName(enumType.TypeId));
				var enumUnderlyingType = GetUnderlyingTypeStr(enumType);

				if (feature is not null)
					writer.WriteLine(feature);
				writer.WriteLine(RustConstants.AttributeAllowNonCamelCaseTypes);
				writer.WriteLine(RustConstants.AttributeAllowDeadCode);
				writer.WriteLine($"pub(crate) type {enumTypeName}UnderlyingType = {enumUnderlyingType};");

				// Associated method: values()

				if (feature is not null)
					writer.WriteLine(feature);
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"impl {enumTypeName} {{");
				using (writer.Indent()) {
					writer.WriteLine($"/// Iterates over all `{enumTypeName}` enum values");
					writer.WriteLine("#[inline]");
					writer.WriteLine($"pub fn values() -> impl Iterator<Item = {enumTypeName}> + DoubleEndedIterator + ExactSizeIterator + FusedIterator {{");
					using (writer.Indent()) {
						if (enumType.Values.Length == 1) {
							writer.WriteLine($"static VALUES: [{enumTypeName}; 1] = [{enumTypeName}::{enumType.Values[0].Name(idConverter)}];");
							writer.WriteLine($"VALUES.iter().copied()");
						}
						else {
							writer.WriteLine("// SAFETY: all values 0-max are valid enum values");
							writer.WriteLine($"(0..{icedConstValue}).map(|x| unsafe {{ mem::transmute::<{enumUnderlyingType}, {enumTypeName}>(x as {enumUnderlyingType}) }})");
						}
					}
					writer.WriteLine("}");
				}
				writer.WriteLine("}");
				writer.WriteLine("#[test]");
				if (feature is not null)
					writer.WriteLine(feature);
				writer.WriteLine(RustConstants.AttributeNoRustFmt);
				writer.WriteLine($"fn test_{enumTypeName.ToLowerInvariant()}_values() {{");
				using (writer.Indent()) {
					writer.WriteLine($"let mut iter = {enumTypeName}::values();");
					writer.WriteLine($"assert_eq!(iter.size_hint(), ({icedConstValue}, Some({icedConstValue})));");
					writer.WriteLine($"assert_eq!(iter.len(), {icedConstValue});");
					writer.WriteLine("assert!(iter.next().is_some());");
					writer.WriteLine($"assert_eq!(iter.size_hint(), ({icedConstValue} - 1, Some({icedConstValue} - 1)));");
					writer.WriteLine($"assert_eq!(iter.len(), {icedConstValue} - 1);");
					writer.WriteLine();
					writer.WriteLine($"let values: Vec<{enumTypeName}> = {enumTypeName}::values().collect();");
					writer.WriteLine($"assert_eq!(values.len(), {icedConstValue});");
					writer.WriteLine("for (i, value) in values.into_iter().enumerate() {");
					using (writer.Indent())
						writer.WriteLine("assert_eq!(i, value as usize);");
					writer.WriteLine("}");
					writer.WriteLine();
					writer.WriteLine($"let values1: Vec<{enumTypeName}> = {enumTypeName}::values().collect();");
					writer.WriteLine($"let mut values2: Vec<{enumTypeName}> = {enumTypeName}::values().rev().collect();");
					writer.WriteLine("values2.reverse();");
					writer.WriteLine("assert_eq!(values1, values2);");
				}
				writer.WriteLine("}");

				// impl trait TryFrom

				var tryFromTypes = new string[] {
					"usize",
					//"u32",
				};
				foreach (var tryFromType in tryFromTypes) {
					var castToFromType = tryFromType == "usize" ? string.Empty : $" as {tryFromType}";
					if (feature is not null)
						writer.WriteLine(feature);
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					writer.WriteLine($"impl TryFrom<{tryFromType}> for {enumTypeName} {{");
					using (writer.Indent()) {
						writer.WriteLine("type Error = IcedError;");
						writer.WriteLine("#[inline]");
						writer.WriteLine($"fn try_from(value: {tryFromType}) -> Result<Self, Self::Error> {{");
						using (writer.Indent()) {
							writer.WriteLine($"if value < {icedConstValue}{castToFromType} {{");
							using (writer.Indent()) {
								if (enumType.Values.Length == 1)
									writer.WriteLine($"Ok({enumTypeName}::{enumType.Values[0].Name(idConverter)})");
								else {
									writer.WriteLine("// SAFETY: all values 0-max are valid enum values");
									writer.WriteLine($"Ok(unsafe {{ mem::transmute(value as {enumUnderlyingType}) }})");
								}
							}
							writer.WriteLine("} else {");
							using (writer.Indent())
								writer.WriteLine($"Err(IcedError::new(\"Invalid {enumTypeName} value\"))");
							writer.WriteLine("}");
						}
						writer.WriteLine("}");
					}
					writer.WriteLine("}");
					if (feature is not null)
						writer.WriteLine(feature);
					writer.WriteLine("#[test]");
					writer.WriteLine(RustConstants.AttributeNoRustFmt);
					writer.WriteLine($"fn test_{enumTypeName.ToLowerInvariant()}_try_from_{tryFromType}() {{");
					using (writer.Indent()) {
						writer.WriteLine($"for value in {enumTypeName}::values() {{");
						using (writer.Indent()) {
							writer.WriteLine($"let converted = <{enumTypeName} as TryFrom<{tryFromType}>>::try_from(value as {tryFromType}).unwrap();");
							writer.WriteLine("assert_eq!(converted, value);");
						}
						writer.WriteLine("}");
						writer.WriteLine($"assert!(<{enumTypeName} as TryFrom<{tryFromType}>>::try_from({icedConstValue}{castToFromType}).is_err());");
						writer.WriteLine($"assert!(<{enumTypeName} as TryFrom<{tryFromType}>>::try_from(core::{tryFromType}::MAX).is_err());");
					}
					writer.WriteLine("}");
				}
			}
		}

		static string GetUnderlyingTypeStr(EnumType enumType) {
			if (enumType.Values.Length <= 1)
				return "()";
			if (enumType.Values.Length <= byte.MaxValue + 1)
				return "u8";
			if (enumType.Values.Length <= ushort.MaxValue + 1)
				return "u16";
			return "u32";
		}
	}
}
