// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Collections.Generic;
using Generator.Constants;
using Generator.Constants.Java;
using Generator.Documentation.Java;

namespace Generator.Enums.Java {
	[Generator(TargetLanguage.Java)]
	sealed class JavaEnumsGenerator : EnumsGenerator {
		readonly Dictionary<TypeId, FullEnumFileInfo?> toFullFileInfo;
		readonly JavaConstantsWriter constantsWriter;

		sealed class FullEnumFileInfo {
			public readonly string Filename;
			public readonly string Package;
			public readonly bool IsTestFile;
			public readonly string? Id;

			public static FullEnumFileInfo Create(GenTypes genTypes, string package, string filename, string? id = null) =>
				Create(genTypes, package, new[] { filename }, id);

			public static FullEnumFileInfo Create(GenTypes genTypes, string package, string[] paths, string? id = null) =>
				new(JavaConstants.GetFilename(genTypes, package, paths), package, false, id);

			public static FullEnumFileInfo CreateTest(GenTypes genTypes, string package, string filename, string? id = null) =>
				CreateTest(genTypes, package, new[] { filename }, id);

			public static FullEnumFileInfo CreateTest(GenTypes genTypes, string package, string[] paths, string? id = null) =>
				new(JavaConstants.GetTestFilename(genTypes, package, paths), package, true, id);

			public FullEnumFileInfo(string filename, string package, bool isTestFile, string? id) {
				Filename = filename;
				Package = package;
				IsTestFile = isTestFile;
				Id = id;
			}
		}

		public JavaEnumsGenerator(GeneratorContext generatorContext)
			: base(generatorContext.Types) {
			var idConverter = JavaIdentifierConverter.Create();
			var docWriter = new JavaDocCommentWriter(idConverter);
			var deprecatedWriter = new JavaDeprecatedWriter(idConverter);
			constantsWriter = new JavaConstantsWriter(genTypes, idConverter, docWriter, deprecatedWriter);

			var dirs = genTypes.Dirs;
			toFullFileInfo = new();
			toFullFileInfo.Add(TypeIds.Code, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedPackage, nameof(TypeIds.Code) + ".java", "Variants"));
			toFullFileInfo.Add(TypeIds.CodeSize, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedPackage, nameof(TypeIds.CodeSize) + ".java"));
			toFullFileInfo.Add(TypeIds.ConditionCode, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedPackage, nameof(TypeIds.ConditionCode) + ".java"));
			toFullFileInfo.Add(TypeIds.CpuidFeature, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedPackage, nameof(TypeIds.CpuidFeature) + ".java"));
			toFullFileInfo.Add(TypeIds.CpuidFeatureInternal, FullEnumFileInfo.Create(genTypes, JavaConstants.InstructionInfoInternalPackage, nameof(TypeIds.CpuidFeatureInternal) + ".java"));
			toFullFileInfo.Add(TypeIds.DecoderError, FullEnumFileInfo.Create(genTypes, JavaConstants.DecoderPackage, nameof(TypeIds.DecoderError) + ".java"));
			toFullFileInfo.Add(TypeIds.DecoderOptions, FullEnumFileInfo.Create(genTypes, JavaConstants.DecoderPackage, nameof(TypeIds.DecoderOptions) + ".java"));
			toFullFileInfo.Add(TypeIds.DecoderTestOptions, FullEnumFileInfo.CreateTest(genTypes, JavaConstants.DecoderPackage, nameof(TypeIds.DecoderTestOptions) + ".java"));
			toFullFileInfo.Add(TypeIds.EvexOpCodeHandlerKind, FullEnumFileInfo.Create(genTypes, JavaConstants.DecoderInternalPackage, nameof(TypeIds.EvexOpCodeHandlerKind) + ".java"));
			toFullFileInfo.Add(TypeIds.MvexOpCodeHandlerKind, FullEnumFileInfo.Create(genTypes, JavaConstants.DecoderInternalPackage, nameof(TypeIds.MvexOpCodeHandlerKind) + ".java"));
			toFullFileInfo.Add(TypeIds.HandlerFlags, FullEnumFileInfo.Create(genTypes, JavaConstants.DecoderInternalPackage, nameof(TypeIds.HandlerFlags) + ".java"));
			toFullFileInfo.Add(TypeIds.LegacyHandlerFlags, FullEnumFileInfo.Create(genTypes, JavaConstants.DecoderInternalPackage, nameof(TypeIds.LegacyHandlerFlags) + ".java"));
			toFullFileInfo.Add(TypeIds.MemorySize, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedPackage, nameof(TypeIds.MemorySize) + ".java", "Variants"));
			toFullFileInfo.Add(TypeIds.LegacyOpCodeHandlerKind, FullEnumFileInfo.Create(genTypes, JavaConstants.DecoderInternalPackage, nameof(TypeIds.LegacyOpCodeHandlerKind) + ".java"));
			toFullFileInfo.Add(TypeIds.PseudoOpsKind, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterInternalPackage, nameof(TypeIds.PseudoOpsKind) + ".java"));
			toFullFileInfo.Add(TypeIds.Register, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedPackage, nameof(TypeIds.Register) + ".java", "Variants"));
			toFullFileInfo.Add(TypeIds.SerializedDataKind, FullEnumFileInfo.Create(genTypes, JavaConstants.DecoderInternalPackage, nameof(TypeIds.SerializedDataKind) + ".java"));
			toFullFileInfo.Add(TypeIds.TupleType, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedPackage, nameof(TypeIds.TupleType) + ".java"));
			toFullFileInfo.Add(TypeIds.VexOpCodeHandlerKind, FullEnumFileInfo.Create(genTypes, JavaConstants.DecoderInternalPackage, nameof(TypeIds.VexOpCodeHandlerKind) + ".java"));
			toFullFileInfo.Add(TypeIds.Mnemonic, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedPackage, nameof(TypeIds.Mnemonic) + ".java"));
			toFullFileInfo.Add(TypeIds.GasCtorKind, FullEnumFileInfo.Create(genTypes, JavaConstants.GasFormatterPackage, "CtorKind.java"));
			toFullFileInfo.Add(TypeIds.IntelCtorKind, FullEnumFileInfo.Create(genTypes, JavaConstants.IntelFormatterPackage, "CtorKind.java"));
			toFullFileInfo.Add(TypeIds.MasmCtorKind, FullEnumFileInfo.Create(genTypes, JavaConstants.MasmFormatterPackage, "CtorKind.java"));
			toFullFileInfo.Add(TypeIds.NasmCtorKind, FullEnumFileInfo.Create(genTypes, JavaConstants.NasmFormatterPackage, "CtorKind.java"));
			toFullFileInfo.Add(TypeIds.GasSizeOverride, FullEnumFileInfo.Create(genTypes, JavaConstants.GasFormatterPackage, "SizeOverride.java"));
			toFullFileInfo.Add(TypeIds.GasInstrOpInfoFlags, FullEnumFileInfo.Create(genTypes, JavaConstants.GasFormatterPackage, "InstrOpInfoFlags.java"));
			toFullFileInfo.Add(TypeIds.IntelSizeOverride, FullEnumFileInfo.Create(genTypes, JavaConstants.IntelFormatterPackage, "SizeOverride.java"));
			toFullFileInfo.Add(TypeIds.IntelBranchSizeInfo, FullEnumFileInfo.Create(genTypes, JavaConstants.IntelFormatterPackage, "BranchSizeInfo.java"));
			toFullFileInfo.Add(TypeIds.IntelInstrOpInfoFlags, FullEnumFileInfo.Create(genTypes, JavaConstants.IntelFormatterPackage, "InstrOpInfoFlags.java"));
			toFullFileInfo.Add(TypeIds.MasmInstrOpInfoFlags, FullEnumFileInfo.Create(genTypes, JavaConstants.MasmFormatterPackage, "InstrOpInfoFlags.java"));
			toFullFileInfo.Add(TypeIds.NasmSignExtendInfo, FullEnumFileInfo.Create(genTypes, JavaConstants.NasmFormatterPackage, "SignExtendInfo.java"));
			toFullFileInfo.Add(TypeIds.NasmSizeOverride, FullEnumFileInfo.Create(genTypes, JavaConstants.NasmFormatterPackage, "SizeOverride.java"));
			toFullFileInfo.Add(TypeIds.NasmBranchSizeInfo, FullEnumFileInfo.Create(genTypes, JavaConstants.NasmFormatterPackage, "BranchSizeInfo.java"));
			toFullFileInfo.Add(TypeIds.NasmInstrOpInfoFlags, FullEnumFileInfo.Create(genTypes, JavaConstants.NasmFormatterPackage, "InstrOpInfoFlags.java"));
			toFullFileInfo.Add(TypeIds.FastFmtFlags, FullEnumFileInfo.Create(genTypes, JavaConstants.FastFormatterPackage, "FastFmtFlags.java"));
			toFullFileInfo.Add(TypeIds.RoundingControl, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedPackage, nameof(TypeIds.RoundingControl) + ".java"));
			toFullFileInfo.Add(TypeIds.OpKind, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedPackage, nameof(TypeIds.OpKind) + ".java"));
			toFullFileInfo.Add(TypeIds.InstrScale, null);
			toFullFileInfo.Add(TypeIds.VectorLength, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedInternalPackage, nameof(TypeIds.VectorLength) + ".java"));
			toFullFileInfo.Add(TypeIds.MandatoryPrefixByte, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedInternalPackage, nameof(TypeIds.MandatoryPrefixByte) + ".java"));// 'uint' not 'byte' since it gets zx to uint when OR'ing values
			toFullFileInfo.Add(TypeIds.EncodingKind, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedPackage, nameof(TypeIds.EncodingKind) + ".java"));
			toFullFileInfo.Add(TypeIds.FlowControl, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedPackage, nameof(TypeIds.FlowControl) + ".java"));
			toFullFileInfo.Add(TypeIds.OpCodeOperandKind, FullEnumFileInfo.Create(genTypes, JavaConstants.InstructionInfoPackage, nameof(TypeIds.OpCodeOperandKind) + ".java"));
			toFullFileInfo.Add(TypeIds.MvexEHBit, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedPackage, nameof(TypeIds.MvexEHBit) + ".java"));
			toFullFileInfo.Add(TypeIds.MvexInfoFlags1, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedInternalPackage, nameof(TypeIds.MvexInfoFlags1) + ".java"));
			toFullFileInfo.Add(TypeIds.MvexInfoFlags2, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedInternalPackage, nameof(TypeIds.MvexInfoFlags2) + ".java"));
			toFullFileInfo.Add(TypeIds.RflagsBits, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedPackage, nameof(TypeIds.RflagsBits) + ".java"));
			toFullFileInfo.Add(TypeIds.OpAccess, FullEnumFileInfo.Create(genTypes, JavaConstants.InstructionInfoPackage, nameof(TypeIds.OpAccess) + ".java"));
			toFullFileInfo.Add(TypeIds.MandatoryPrefix, FullEnumFileInfo.Create(genTypes, JavaConstants.InstructionInfoPackage, nameof(TypeIds.MandatoryPrefix) + ".java"));
			toFullFileInfo.Add(TypeIds.OpCodeTableKind, FullEnumFileInfo.Create(genTypes, JavaConstants.InstructionInfoPackage, nameof(TypeIds.OpCodeTableKind) + ".java"));
			toFullFileInfo.Add(TypeIds.FormatterTextKind, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterPackage, nameof(TypeIds.FormatterTextKind) + ".java"));
			toFullFileInfo.Add(TypeIds.MemorySizeOptions, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterPackage, nameof(TypeIds.MemorySizeOptions) + ".java"));
			toFullFileInfo.Add(TypeIds.CodeAsmMemoryOperandSize, FullEnumFileInfo.Create(genTypes, JavaConstants.CodeAssemblerPackage, "MemoryOperandSize.java"));
			toFullFileInfo.Add(TypeIds.MvexConvFn, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedPackage, nameof(TypeIds.MvexConvFn) + ".java"));
			toFullFileInfo.Add(TypeIds.MvexRegMemConv, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedPackage, nameof(TypeIds.MvexRegMemConv) + ".java"));
			toFullFileInfo.Add(TypeIds.MvexTupleTypeLutKind, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedPackage, nameof(TypeIds.MvexTupleTypeLutKind) + ".java"));

			toFullFileInfo.Add(TypeIds.InstrFlags1, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedInternalPackage, nameof(TypeIds.InstrFlags1) + ".java"));
			toFullFileInfo.Add(TypeIds.MvexInstrFlags, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedInternalPackage, nameof(TypeIds.MvexInstrFlags) + ".java"));
			toFullFileInfo.Add(TypeIds.OpSize, FullEnumFileInfo.Create(genTypes, JavaConstants.DecoderInternalPackage, nameof(TypeIds.OpSize) + ".java"));
			toFullFileInfo.Add(TypeIds.StateFlags, FullEnumFileInfo.Create(genTypes, JavaConstants.DecoderInternalPackage, nameof(TypeIds.StateFlags) + ".java"));
			toFullFileInfo.Add(TypeIds.ImpliedAccess, FullEnumFileInfo.Create(genTypes, JavaConstants.InstructionInfoInternalPackage, nameof(TypeIds.ImpliedAccess) + ".java"));
			toFullFileInfo.Add(TypeIds.RflagsInfo, FullEnumFileInfo.Create(genTypes, JavaConstants.InstructionInfoInternalPackage, nameof(TypeIds.RflagsInfo) + ".java"));
			toFullFileInfo.Add(TypeIds.OpInfo0, FullEnumFileInfo.Create(genTypes, JavaConstants.InstructionInfoPackage, nameof(TypeIds.OpInfo0) + ".java"));
			toFullFileInfo.Add(TypeIds.OpInfo1, FullEnumFileInfo.Create(genTypes, JavaConstants.InstructionInfoPackage, nameof(TypeIds.OpInfo1) + ".java"));
			toFullFileInfo.Add(TypeIds.OpInfo2, FullEnumFileInfo.Create(genTypes, JavaConstants.InstructionInfoPackage, nameof(TypeIds.OpInfo2) + ".java"));
			toFullFileInfo.Add(TypeIds.OpInfo3, FullEnumFileInfo.Create(genTypes, JavaConstants.InstructionInfoPackage, nameof(TypeIds.OpInfo3) + ".java"));
			toFullFileInfo.Add(TypeIds.OpInfo4, FullEnumFileInfo.Create(genTypes, JavaConstants.InstructionInfoPackage, nameof(TypeIds.OpInfo4) + ".java"));
			toFullFileInfo.Add(TypeIds.InfoFlags1, FullEnumFileInfo.Create(genTypes, JavaConstants.InstructionInfoInternalPackage, nameof(TypeIds.InfoFlags1) + ".java"));
			toFullFileInfo.Add(TypeIds.InfoFlags2, FullEnumFileInfo.Create(genTypes, JavaConstants.InstructionInfoInternalPackage, nameof(TypeIds.InfoFlags2) + ".java"));
			toFullFileInfo.Add(TypeIds.MemorySizeFlags, FullEnumFileInfo.CreateTest(genTypes, JavaConstants.InstructionInfoPackage, nameof(TypeIds.MemorySizeFlags) + ".java"));
			toFullFileInfo.Add(TypeIds.RegisterFlags, FullEnumFileInfo.CreateTest(genTypes, JavaConstants.InstructionInfoPackage, nameof(TypeIds.RegisterFlags) + ".java"));

			toFullFileInfo.Add(TypeIds.LegacyOpCodeTable, FullEnumFileInfo.Create(genTypes, JavaConstants.EncoderInternalPackage, nameof(TypeIds.LegacyOpCodeTable) + ".java"));
			toFullFileInfo.Add(TypeIds.VexOpCodeTable, FullEnumFileInfo.Create(genTypes, JavaConstants.EncoderInternalPackage, nameof(TypeIds.VexOpCodeTable) + ".java"));
			toFullFileInfo.Add(TypeIds.XopOpCodeTable, FullEnumFileInfo.Create(genTypes, JavaConstants.EncoderInternalPackage, nameof(TypeIds.XopOpCodeTable) + ".java"));
			toFullFileInfo.Add(TypeIds.EvexOpCodeTable, FullEnumFileInfo.Create(genTypes, JavaConstants.EncoderInternalPackage, nameof(TypeIds.EvexOpCodeTable) + ".java"));
			toFullFileInfo.Add(TypeIds.MvexOpCodeTable, FullEnumFileInfo.Create(genTypes, JavaConstants.EncoderInternalPackage, nameof(TypeIds.MvexOpCodeTable) + ".java"));

			toFullFileInfo.Add(TypeIds.FormatterFlowControl, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterInternalPackage, nameof(TypeIds.FormatterFlowControl) + ".java"));
			toFullFileInfo.Add(TypeIds.GasInstrOpKind, FullEnumFileInfo.Create(genTypes, JavaConstants.GasFormatterPackage, "InstrOpKind.java"));
			toFullFileInfo.Add(TypeIds.IntelInstrOpKind, FullEnumFileInfo.Create(genTypes, JavaConstants.IntelFormatterPackage, "InstrOpKind.java"));
			toFullFileInfo.Add(TypeIds.MasmInstrOpKind, FullEnumFileInfo.Create(genTypes, JavaConstants.MasmFormatterPackage, "InstrOpKind.java"));
			toFullFileInfo.Add(TypeIds.MasmSymbolTestFlags, FullEnumFileInfo.CreateTest(genTypes, JavaConstants.MasmFormatterPackage, "SymbolTestFlags.java"));
			toFullFileInfo.Add(TypeIds.NasmInstrOpKind, FullEnumFileInfo.Create(genTypes, JavaConstants.NasmFormatterPackage, "InstrOpKind.java"));
			toFullFileInfo.Add(TypeIds.NasmMemorySizeInfo, FullEnumFileInfo.Create(genTypes, JavaConstants.NasmFormatterPackage, "MemorySizeInfo.java"));
			toFullFileInfo.Add(TypeIds.NasmFarMemorySizeInfo, FullEnumFileInfo.Create(genTypes, JavaConstants.NasmFormatterPackage, "FarMemorySizeInfo.java"));
			toFullFileInfo.Add(TypeIds.NumberBase, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterPackage, nameof(TypeIds.NumberBase) + ".java"));

			toFullFileInfo.Add(TypeIds.DisplSize, FullEnumFileInfo.Create(genTypes, JavaConstants.EncoderInternalPackage, nameof(TypeIds.DisplSize) + ".java"));
			toFullFileInfo.Add(TypeIds.ImmSize, FullEnumFileInfo.Create(genTypes, JavaConstants.EncoderInternalPackage, nameof(TypeIds.ImmSize) + ".java"));
			toFullFileInfo.Add(TypeIds.EncoderFlags, FullEnumFileInfo.Create(genTypes, JavaConstants.EncoderInternalPackage, nameof(TypeIds.EncoderFlags) + ".java"));
			toFullFileInfo.Add(TypeIds.EncFlags1, FullEnumFileInfo.Create(genTypes, JavaConstants.EncoderInternalPackage, nameof(TypeIds.EncFlags1) + ".java"));
			toFullFileInfo.Add(TypeIds.EncFlags2, FullEnumFileInfo.Create(genTypes, JavaConstants.EncoderInternalPackage, nameof(TypeIds.EncFlags2) + ".java"));
			toFullFileInfo.Add(TypeIds.EncFlags3, FullEnumFileInfo.Create(genTypes, JavaConstants.EncoderInternalPackage, nameof(TypeIds.EncFlags3) + ".java"));
			toFullFileInfo.Add(TypeIds.OpCodeInfoFlags1, FullEnumFileInfo.Create(genTypes, JavaConstants.EncoderInternalPackage, nameof(TypeIds.OpCodeInfoFlags1) + ".java"));
			toFullFileInfo.Add(TypeIds.OpCodeInfoFlags2, FullEnumFileInfo.Create(genTypes, JavaConstants.EncoderInternalPackage, nameof(TypeIds.OpCodeInfoFlags2) + ".java"));
			toFullFileInfo.Add(TypeIds.DecOptionValue, null);
			toFullFileInfo.Add(TypeIds.InstrStrFmtOption, FullEnumFileInfo.Create(genTypes, JavaConstants.EncoderInternalPackage, nameof(TypeIds.InstrStrFmtOption) + ".java"));
			toFullFileInfo.Add(TypeIds.WBit, FullEnumFileInfo.Create(genTypes, JavaConstants.EncoderInternalPackage, nameof(TypeIds.WBit) + ".java"));
			toFullFileInfo.Add(TypeIds.LBit, FullEnumFileInfo.Create(genTypes, JavaConstants.EncoderInternalPackage, nameof(TypeIds.LBit) + ".java"));
			toFullFileInfo.Add(TypeIds.LKind, FullEnumFileInfo.Create(genTypes, JavaConstants.EncoderInternalPackage, nameof(TypeIds.LKind) + ".java"));
			toFullFileInfo.Add(TypeIds.RepPrefixKind, FullEnumFileInfo.Create(genTypes, JavaConstants.IcedPackage, nameof(TypeIds.RepPrefixKind) + ".java"));
			toFullFileInfo.Add(TypeIds.RelocKind, FullEnumFileInfo.Create(genTypes, JavaConstants.BlockEncoderPackage, nameof(TypeIds.RelocKind) + ".java"));
			toFullFileInfo.Add(TypeIds.BlockEncoderOptions, FullEnumFileInfo.Create(genTypes, JavaConstants.BlockEncoderPackage, nameof(TypeIds.BlockEncoderOptions) + ".java"));
			toFullFileInfo.Add(TypeIds.FormatMnemonicOptions, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterPackage, nameof(TypeIds.FormatMnemonicOptions) + ".java"));
			toFullFileInfo.Add(TypeIds.PrefixKind, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterPackage, nameof(TypeIds.PrefixKind) + ".java"));
			toFullFileInfo.Add(TypeIds.DecoratorKind, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterPackage, nameof(TypeIds.DecoratorKind) + ".java"));
			toFullFileInfo.Add(TypeIds.NumberKind, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterPackage, nameof(TypeIds.NumberKind) + ".java"));
			toFullFileInfo.Add(TypeIds.SymbolFlags, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterPackage, nameof(TypeIds.SymbolFlags) + ".java"));
			toFullFileInfo.Add(TypeIds.CC_b, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterPackage, nameof(TypeIds.CC_b) + ".java"));
			toFullFileInfo.Add(TypeIds.CC_ae, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterPackage, nameof(TypeIds.CC_ae) + ".java"));
			toFullFileInfo.Add(TypeIds.CC_e, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterPackage, nameof(TypeIds.CC_e) + ".java"));
			toFullFileInfo.Add(TypeIds.CC_ne, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterPackage, nameof(TypeIds.CC_ne) + ".java"));
			toFullFileInfo.Add(TypeIds.CC_be, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterPackage, nameof(TypeIds.CC_be) + ".java"));
			toFullFileInfo.Add(TypeIds.CC_a, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterPackage, nameof(TypeIds.CC_a) + ".java"));
			toFullFileInfo.Add(TypeIds.CC_p, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterPackage, nameof(TypeIds.CC_p) + ".java"));
			toFullFileInfo.Add(TypeIds.CC_np, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterPackage, nameof(TypeIds.CC_np) + ".java"));
			toFullFileInfo.Add(TypeIds.CC_l, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterPackage, nameof(TypeIds.CC_l) + ".java"));
			toFullFileInfo.Add(TypeIds.CC_ge, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterPackage, nameof(TypeIds.CC_ge) + ".java"));
			toFullFileInfo.Add(TypeIds.CC_le, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterPackage, nameof(TypeIds.CC_le) + ".java"));
			toFullFileInfo.Add(TypeIds.CC_g, FullEnumFileInfo.Create(genTypes, JavaConstants.FormatterPackage, nameof(TypeIds.CC_g) + ".java"));

			toFullFileInfo.Add(TypeIds.OptionsProps, FullEnumFileInfo.CreateTest(genTypes, JavaConstants.FormatterPackage, nameof(TypeIds.OptionsProps) + ".java"));

			toFullFileInfo.Add(TypeIds.TestInstrFlags, FullEnumFileInfo.CreateTest(genTypes, JavaConstants.CodeAssemblerPackage, nameof(TypeIds.TestInstrFlags) + ".java"));
		}

		public override void Generate(EnumType enumType) {
			if (toFullFileInfo.TryGetValue(enumType.TypeId, out var fullFileInfo)) {
				if (fullFileInfo is not null)
					WriteFile(fullFileInfo, enumType);
			}
		}

		void WriteFile(FullEnumFileInfo info, EnumType enumType) {
			var constantsType = enumType.ToConstantsType(ConstantKind.UInt32);
			constantsWriter.WriteFile(info.Package, info.Filename, constantsType, Array.Empty<string>(), info.IsTestFile, info.Id);
		}
	}
}
