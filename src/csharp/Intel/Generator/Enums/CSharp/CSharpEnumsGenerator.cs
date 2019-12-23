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
using Generator.Documentation.CSharp;
using Generator.IO;

namespace Generator.Enums.CSharp {
	[Generator(TargetLanguage.CSharp, GeneratorNames.Enums)]
	sealed class CSharpEnumsGenerator : EnumsGenerator {
		readonly IdentifierConverter idConverter;
		readonly Dictionary<TypeId, FullEnumFileInfo> toFullFileInfo;
		readonly Dictionary<TypeId, PartialEnumFileInfo> toPartialFileInfo;
		readonly CSharpDocCommentWriter docWriter;

		sealed class FullEnumFileInfo {
			public readonly string Filename;
			public readonly string Namespace;
			public readonly string? Define;
			public readonly string? BaseType;

			public FullEnumFileInfo(string filename, string @namespace, string? define = null, string? baseType = null) {
				Filename = filename;
				Namespace = @namespace;
				Define = define;
				BaseType = baseType;
			}
		}

		sealed class PartialEnumFileInfo {
			public readonly string Id;
			public readonly string Filename;
			public readonly string? BaseType;

			public PartialEnumFileInfo(string id, string filename, string? baseType) {
				Id = id;
				Filename = filename;
				BaseType = baseType;
			}
		}

		public CSharpEnumsGenerator(GeneratorOptions generatorOptions) {
			idConverter = CSharpIdentifierConverter.Create();
			docWriter = new CSharpDocCommentWriter(idConverter);

			toFullFileInfo = new Dictionary<TypeId, FullEnumFileInfo>();
			toFullFileInfo.Add(TypeIds.Code, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), nameof(TypeIds.Code) + ".g.cs"), CSharpConstants.IcedNamespace));
			toFullFileInfo.Add(TypeIds.CodeSize, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), nameof(TypeIds.CodeSize) + ".g.cs"), CSharpConstants.IcedNamespace));
			toFullFileInfo.Add(TypeIds.ConditionCode, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), nameof(TypeIds.ConditionCode) + ".g.cs"), CSharpConstants.IcedNamespace, CSharpConstants.InstructionInfoDefine));
			toFullFileInfo.Add(TypeIds.CpuidFeature, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), nameof(TypeIds.CpuidFeature) + ".g.cs"), CSharpConstants.IcedNamespace, CSharpConstants.InstructionInfoDefine));
			toFullFileInfo.Add(TypeIds.CpuidFeatureInternal, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.InstructionInfoNamespace), nameof(TypeIds.CpuidFeatureInternal) + ".g.cs"), CSharpConstants.InstructionInfoNamespace, CSharpConstants.InstructionInfoDefine));
			toFullFileInfo.Add(TypeIds.DecoderOptions, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), nameof(TypeIds.DecoderOptions) + ".g.cs"), CSharpConstants.IcedNamespace, CSharpConstants.DecoderDefine, baseType: "uint"));
			toFullFileInfo.Add(TypeIds.EvexOpCodeHandlerKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.DecoderNamespace), nameof(TypeIds.EvexOpCodeHandlerKind) + ".g.cs"), CSharpConstants.DecoderNamespace, CSharpConstants.DecoderDefine, baseType: "byte"));
			toFullFileInfo.Add(TypeIds.HandlerFlags, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.DecoderNamespace), nameof(TypeIds.HandlerFlags) + ".g.cs"), CSharpConstants.DecoderNamespace, CSharpConstants.DecoderDefine, baseType: "uint"));
			toFullFileInfo.Add(TypeIds.LegacyHandlerFlags, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.DecoderNamespace), nameof(TypeIds.LegacyHandlerFlags) + ".g.cs"), CSharpConstants.DecoderNamespace, CSharpConstants.DecoderDefine, baseType: "uint"));
			toFullFileInfo.Add(TypeIds.MemorySize, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), nameof(TypeIds.MemorySize) + ".g.cs"), CSharpConstants.IcedNamespace));
			toFullFileInfo.Add(TypeIds.OpCodeHandlerKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.DecoderNamespace), nameof(TypeIds.OpCodeHandlerKind) + ".g.cs"), CSharpConstants.DecoderNamespace, CSharpConstants.DecoderDefine, baseType: "byte"));
			toFullFileInfo.Add(TypeIds.PseudoOpsKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.FormatterNamespace), nameof(TypeIds.PseudoOpsKind) + ".g.cs"), CSharpConstants.FormatterNamespace, CSharpConstants.AnyFormatterDefine));
			toFullFileInfo.Add(TypeIds.Register, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), nameof(TypeIds.Register) + ".g.cs"), CSharpConstants.IcedNamespace));
			toFullFileInfo.Add(TypeIds.SerializedDataKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.DecoderNamespace), nameof(TypeIds.SerializedDataKind) + ".g.cs"), CSharpConstants.DecoderNamespace, CSharpConstants.DecoderDefine, baseType: "byte"));
			toFullFileInfo.Add(TypeIds.TupleType, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), nameof(TypeIds.TupleType) + ".g.cs"), CSharpConstants.IcedNamespace, CSharpConstants.DecoderOrEncoderDefine));
			toFullFileInfo.Add(TypeIds.VexOpCodeHandlerKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.DecoderNamespace), nameof(TypeIds.VexOpCodeHandlerKind) + ".g.cs"), CSharpConstants.DecoderNamespace, CSharpConstants.DecoderDefine, baseType: "byte"));
			toFullFileInfo.Add(TypeIds.Mnemonic, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), nameof(TypeIds.Mnemonic) + ".g.cs"), CSharpConstants.IcedNamespace));
			toFullFileInfo.Add(TypeIds.GasCtorKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.GasFormatterNamespace), "CtorKind.g.cs"), CSharpConstants.GasFormatterNamespace, CSharpConstants.GasFormatterDefine));
			toFullFileInfo.Add(TypeIds.IntelCtorKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IntelFormatterNamespace), "CtorKind.g.cs"), CSharpConstants.IntelFormatterNamespace, CSharpConstants.IntelFormatterDefine));
			toFullFileInfo.Add(TypeIds.MasmCtorKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.MasmFormatterNamespace), "CtorKind.g.cs"), CSharpConstants.MasmFormatterNamespace, CSharpConstants.MasmFormatterDefine));
			toFullFileInfo.Add(TypeIds.NasmCtorKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.NasmFormatterNamespace), "CtorKind.g.cs"), CSharpConstants.NasmFormatterNamespace, CSharpConstants.NasmFormatterDefine));
			toFullFileInfo.Add(TypeIds.GasSizeOverride, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.GasFormatterNamespace), "SizeOverride.g.cs"), CSharpConstants.GasFormatterNamespace, CSharpConstants.GasFormatterDefine));
			toFullFileInfo.Add(TypeIds.GasInstrOpInfoFlags, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.GasFormatterNamespace), "InstrOpInfoFlags.g.cs"), CSharpConstants.GasFormatterNamespace, CSharpConstants.GasFormatterDefine, "ushort"));
			toFullFileInfo.Add(TypeIds.IntelSizeOverride, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IntelFormatterNamespace), "SizeOverride.g.cs"), CSharpConstants.IntelFormatterNamespace, CSharpConstants.IntelFormatterDefine));
			toFullFileInfo.Add(TypeIds.IntelBranchSizeInfo, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IntelFormatterNamespace), "BranchSizeInfo.g.cs"), CSharpConstants.IntelFormatterNamespace, CSharpConstants.IntelFormatterDefine));
			toFullFileInfo.Add(TypeIds.IntelInstrOpInfoFlags, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IntelFormatterNamespace), "InstrOpInfoFlags.g.cs"), CSharpConstants.IntelFormatterNamespace, CSharpConstants.IntelFormatterDefine, "ushort"));
			toFullFileInfo.Add(TypeIds.MasmInstrOpInfoFlags, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.MasmFormatterNamespace), "InstrOpInfoFlags.g.cs"), CSharpConstants.MasmFormatterNamespace, CSharpConstants.MasmFormatterDefine, "ushort"));
			toFullFileInfo.Add(TypeIds.NasmSignExtendInfo, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.NasmFormatterNamespace), "SignExtendInfo.g.cs"), CSharpConstants.NasmFormatterNamespace, CSharpConstants.NasmFormatterDefine));
			toFullFileInfo.Add(TypeIds.NasmSizeOverride, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.NasmFormatterNamespace), "SizeOverride.g.cs"), CSharpConstants.NasmFormatterNamespace, CSharpConstants.NasmFormatterDefine));
			toFullFileInfo.Add(TypeIds.NasmBranchSizeInfo, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.NasmFormatterNamespace), "BranchSizeInfo.g.cs"), CSharpConstants.NasmFormatterNamespace, CSharpConstants.NasmFormatterDefine));
			toFullFileInfo.Add(TypeIds.NasmInstrOpInfoFlags, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.NasmFormatterNamespace), "InstrOpInfoFlags.g.cs"), CSharpConstants.NasmFormatterNamespace, CSharpConstants.NasmFormatterDefine, "uint"));
			toFullFileInfo.Add(TypeIds.RoundingControl, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), nameof(TypeIds.RoundingControl) + ".g.cs"), CSharpConstants.IcedNamespace));
			toFullFileInfo.Add(TypeIds.OpKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), nameof(TypeIds.OpKind) + ".g.cs"), CSharpConstants.IcedNamespace));
			toFullFileInfo.Add(TypeIds.VectorLength, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), nameof(TypeIds.VectorLength) + ".g.cs"), CSharpConstants.IcedNamespace, CSharpConstants.DecoderOrEncoderDefine));
			toFullFileInfo.Add(TypeIds.MandatoryPrefixByte, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), nameof(TypeIds.MandatoryPrefixByte) + ".g.cs"), CSharpConstants.IcedNamespace, CSharpConstants.DecoderOrEncoderDefine, "uint"));// 'uint' not 'byte' since it gets zx to uint when OR'ing values
			toFullFileInfo.Add(TypeIds.EncodingKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), nameof(TypeIds.EncodingKind) + ".g.cs"), CSharpConstants.IcedNamespace, CSharpConstants.DecoderOrEncoderOrInstrInfoDefine));
			toFullFileInfo.Add(TypeIds.FlowControl, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), nameof(TypeIds.FlowControl) + ".g.cs"), CSharpConstants.IcedNamespace, CSharpConstants.InstructionInfoDefine));
			toFullFileInfo.Add(TypeIds.OpCodeOperandKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), nameof(TypeIds.OpCodeOperandKind) + ".g.cs"), CSharpConstants.IcedNamespace, CSharpConstants.EncoderDefine));
			toFullFileInfo.Add(TypeIds.RflagsBits, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), nameof(TypeIds.RflagsBits) + ".g.cs"), CSharpConstants.IcedNamespace, CSharpConstants.InstructionInfoDefine));
			toFullFileInfo.Add(TypeIds.OpAccess, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), nameof(TypeIds.OpAccess) + ".g.cs"), CSharpConstants.IcedNamespace, CSharpConstants.InstructionInfoDefine));
			toFullFileInfo.Add(TypeIds.MandatoryPrefix, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), nameof(TypeIds.MandatoryPrefix) + ".g.cs"), CSharpConstants.IcedNamespace, CSharpConstants.EncoderDefine));
			toFullFileInfo.Add(TypeIds.OpCodeTableKind, new FullEnumFileInfo(Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), nameof(TypeIds.OpCodeTableKind) + ".g.cs"), CSharpConstants.IcedNamespace, CSharpConstants.EncoderDefine));

			toPartialFileInfo = new Dictionary<TypeId, PartialEnumFileInfo>();
			toPartialFileInfo.Add(TypeIds.Instruction_MemoryFlags, new PartialEnumFileInfo("MemoryFlags", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), "Instruction.cs"), "ushort"));
			toPartialFileInfo.Add(TypeIds.Instruction_OpKindFlags, new PartialEnumFileInfo("OpKindFlags", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), "Instruction.cs"), "uint"));
			toPartialFileInfo.Add(TypeIds.Instruction_CodeFlags, new PartialEnumFileInfo("CodeFlags", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), "Instruction.cs"), "uint"));
			toPartialFileInfo.Add(TypeIds.StateFlags, new PartialEnumFileInfo("StateFlags", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), "Decoder.cs"), "uint"));
			toPartialFileInfo.Add(TypeIds.CodeInfo, new PartialEnumFileInfo("CodeInfo", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.InstructionInfoNamespace), "InfoHandlerFlags.cs"), null));
			toPartialFileInfo.Add(TypeIds.RflagsInfo, new PartialEnumFileInfo("RflagsInfo", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.InstructionInfoNamespace), "InfoHandlerFlags.cs"), null));
			toPartialFileInfo.Add(TypeIds.OpInfo0, new PartialEnumFileInfo("OpInfo0", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.InstructionInfoNamespace), "InfoHandlerFlags.cs"), null));
			toPartialFileInfo.Add(TypeIds.OpInfo1, new PartialEnumFileInfo("OpInfo1", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.InstructionInfoNamespace), "InfoHandlerFlags.cs"), null));
			toPartialFileInfo.Add(TypeIds.OpInfo2, new PartialEnumFileInfo("OpInfo2", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.InstructionInfoNamespace), "InfoHandlerFlags.cs"), null));
			toPartialFileInfo.Add(TypeIds.OpInfo3, new PartialEnumFileInfo("OpInfo3", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.InstructionInfoNamespace), "InfoHandlerFlags.cs"), null));
			toPartialFileInfo.Add(TypeIds.OpInfo4, new PartialEnumFileInfo("OpInfo4", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.InstructionInfoNamespace), "InfoHandlerFlags.cs"), null));
			toPartialFileInfo.Add(TypeIds.InfoFlags1, new PartialEnumFileInfo("InfoFlags1", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.InstructionInfoNamespace), "InfoHandlerFlags.cs"), "uint"));
			toPartialFileInfo.Add(TypeIds.InfoFlags2, new PartialEnumFileInfo("InfoFlags2", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.InstructionInfoNamespace), "InfoHandlerFlags.cs"), "uint"));
			toPartialFileInfo.Add(TypeIds.MemorySizeFlags, new PartialEnumFileInfo("MemorySizeFlags", Path.Combine(generatorOptions.CSharpTestsDir, "Intel", "InstructionInfoTests", "InstructionInfoConstants.cs"), "uint"));
			toPartialFileInfo.Add(TypeIds.RegisterFlags, new PartialEnumFileInfo("RegisterFlags", Path.Combine(generatorOptions.CSharpTestsDir, "Intel", "InstructionInfoTests", "InstructionInfoConstants.cs"), "uint"));

			toPartialFileInfo.Add(TypeIds.LegacyOpCodeTable, new PartialEnumFileInfo("LegacyOpCodeTable", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), null));
			toPartialFileInfo.Add(TypeIds.VexOpCodeTable, new PartialEnumFileInfo("VexOpCodeTable", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), null));
			toPartialFileInfo.Add(TypeIds.XopOpCodeTable, new PartialEnumFileInfo("XopOpCodeTable", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), null));
			toPartialFileInfo.Add(TypeIds.EvexOpCodeTable, new PartialEnumFileInfo("EvexOpCodeTable", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), null));
			toPartialFileInfo.Add(TypeIds.Encodable, new PartialEnumFileInfo("Encodable", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), null));
			toPartialFileInfo.Add(TypeIds.OpCodeHandlerFlags, new PartialEnumFileInfo("OpCodeHandlerFlags", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), "uint"));
			toPartialFileInfo.Add(TypeIds.LegacyOpKind, new PartialEnumFileInfo("LegacyOpKind", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), "byte"));
			toPartialFileInfo.Add(TypeIds.VexOpKind, new PartialEnumFileInfo("VexOpKind", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), "byte"));
			toPartialFileInfo.Add(TypeIds.XopOpKind, new PartialEnumFileInfo("XopOpKind", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), "byte"));
			toPartialFileInfo.Add(TypeIds.EvexOpKind, new PartialEnumFileInfo("EvexOpKind", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), "byte"));

			toPartialFileInfo.Add(TypeIds.NasmMemorySizeInfo, new PartialEnumFileInfo("MemorySizeInfo", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.NasmFormatterNamespace), "InstrInfo.cs"), null));
			toPartialFileInfo.Add(TypeIds.NasmFarMemorySizeInfo, new PartialEnumFileInfo("FarMemorySizeInfo", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.NasmFormatterNamespace), "InstrInfo.cs"), null));

			toPartialFileInfo.Add(TypeIds.OperandSize, new PartialEnumFileInfo("OperandSize", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), null));
			toPartialFileInfo.Add(TypeIds.AddressSize, new PartialEnumFileInfo("AddressSize", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), null));
			toPartialFileInfo.Add(TypeIds.VexVectorLength, new PartialEnumFileInfo("VexVectorLength", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), null));
			toPartialFileInfo.Add(TypeIds.XopVectorLength, new PartialEnumFileInfo("XopVectorLength", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), null));
			toPartialFileInfo.Add(TypeIds.EvexVectorLength, new PartialEnumFileInfo("EvexVectorLength", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), null));
			toPartialFileInfo.Add(TypeIds.DisplSize, new PartialEnumFileInfo("DisplSize", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), null));
			toPartialFileInfo.Add(TypeIds.ImmSize, new PartialEnumFileInfo("ImmSize", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), null));
			toPartialFileInfo.Add(TypeIds.EncoderFlags, new PartialEnumFileInfo("EncoderFlags", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), "uint"));
			toPartialFileInfo.Add(TypeIds.EncFlags1, new PartialEnumFileInfo("EncFlags1", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), "uint"));
			toPartialFileInfo.Add(TypeIds.LegacyFlags3, new PartialEnumFileInfo("LegacyFlags3", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), "uint"));
			toPartialFileInfo.Add(TypeIds.VexFlags3, new PartialEnumFileInfo("VexFlags3", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), "uint"));
			toPartialFileInfo.Add(TypeIds.XopFlags3, new PartialEnumFileInfo("XopFlags3", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), "uint"));
			toPartialFileInfo.Add(TypeIds.EvexFlags3, new PartialEnumFileInfo("EvexFlags3", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), "uint"));
			toPartialFileInfo.Add(TypeIds.AllowedPrefixes, new PartialEnumFileInfo("AllowedPrefixes", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), null));
			toPartialFileInfo.Add(TypeIds.LegacyFlags, new PartialEnumFileInfo("LegacyFlags", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), "uint"));
			toPartialFileInfo.Add(TypeIds.VexFlags, new PartialEnumFileInfo("VexFlags", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), "uint"));
			toPartialFileInfo.Add(TypeIds.XopFlags, new PartialEnumFileInfo("XopFlags", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), "uint"));
			toPartialFileInfo.Add(TypeIds.EvexFlags, new PartialEnumFileInfo("EvexFlags", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), "uint"));
			toPartialFileInfo.Add(TypeIds.D3nowFlags, new PartialEnumFileInfo("D3nowFlags", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), "uint"));
			toPartialFileInfo.Add(TypeIds.WBit, new PartialEnumFileInfo("WBit", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "Enums.cs"), "uint"));
			toPartialFileInfo.Add(TypeIds.LKind, new PartialEnumFileInfo("LKind", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.EncoderNamespace), "OpCodeFormatter.cs"), "byte"));
			toPartialFileInfo.Add(TypeIds.OpCodeFlags, new PartialEnumFileInfo("Flags", Path.Combine(CSharpConstants.GetDirectory(generatorOptions, CSharpConstants.IcedNamespace), "OpCodeInfo.cs"), "uint"));
		}

		public override void Generate(EnumType enumType) {
			if (toFullFileInfo.TryGetValue(enumType.TypeId, out var fullFileInfo))
				WriteFile(fullFileInfo, enumType);
			else if (toPartialFileInfo.TryGetValue(enumType.TypeId, out var partialInfo)) {
				if (!(partialInfo is null))
					new FileUpdater(TargetLanguage.CSharp, partialInfo.Id, partialInfo.Filename).Generate(writer => WriteEnum(writer, partialInfo, enumType));
			}
			else
				throw new InvalidOperationException();
		}

		void WriteEnum(FileWriter writer, EnumType enumType, string? baseType) {
			docWriter.Write(writer, enumType.Documentation, enumType.RawName);
			if (enumType.IsFlags)
				writer.WriteLine("[Flags]");
			var pub = enumType.IsPublic ? "public " : string.Empty;
			var theBaseType = !(baseType is null) ? $" : {baseType}" : string.Empty;
			writer.WriteLine($"{pub}enum {enumType.Name(idConverter)}{theBaseType} {{");
			using (writer.Indent()) {
				uint expectedValue = 0;
				foreach (var value in enumType.Values) {
					docWriter.Write(writer, value.Documentation, enumType.RawName);
					if (enumType.IsFlags)
						writer.WriteLine($"{value.Name(idConverter)} = 0x{value.Value:X8},");
					else if (expectedValue != value.Value)
						writer.WriteLine($"{value.Name(idConverter)} = {value.Value},");
					else
						writer.WriteLine($"{value.Name(idConverter)},");
					expectedValue = value.Value + 1;
				}
			}
			writer.WriteLine("}");
		}

		void WriteFile(FullEnumFileInfo info, EnumType enumType) {
			using (var writer = new FileWriter(TargetLanguage.CSharp, FileUtils.OpenWrite(info.Filename))) {
				writer.WriteFileHeader();
				if (!(info.Define is null))
					writer.WriteLine($"#if {info.Define}");

				if (enumType.IsFlags) {
					writer.WriteLine("using System;");
					writer.WriteLine();
				}

				writer.WriteLine($"namespace {info.Namespace} {{");

				if (enumType.IsPublic && enumType.IsMissingDocs)
					writer.WriteLine("#pragma warning disable 1591 // Missing XML comment for publicly visible type or member");
				using (writer.Indent())
					WriteEnum(writer, enumType, info.BaseType);

				writer.WriteLine("}");
				if (!(info.Define is null))
					writer.WriteLine("#endif");
			}
		}

		void WriteEnum(FileWriter writer, PartialEnumFileInfo partialInfo, EnumType enumType) =>
			WriteEnum(writer, enumType, partialInfo.BaseType);
	}
}
