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

namespace Generator.Enums {
	abstract class EnumsGenerator {
		static readonly EnumType[] allEnums = new EnumType[] {
			CodeEnum.Instance,
			CodeSizeEnum.Instance,
			InstructionInfo.ConditionCodeEnum.Instance,
			InstructionInfo.CpuidFeatureEnum.Instance,
			Decoder.DecoderOptionsEnum.Instance,
			Decoder.EvexOpCodeHandlerKindEnum.Instance,
			Decoder.HandlerFlagsEnum.Instance,
			Decoder.LegacyHandlerFlagsEnum.Instance,
			MemorySizeEnum.Instance,
			Decoder.OpCodeHandlerKindEnum.Instance,
			Formatter.PseudoOpsKindEnum.Instance,
			RegisterEnum.Instance,
			Decoder.SerializedDataKindEnum.Instance,
			TupleTypeEnum.Instance,
			Decoder.VexOpCodeHandlerKindEnum.Instance,
			MnemonicEnum.Instance,
			Formatter.Gas.CtorKindEnum.Instance,
			Formatter.Gas.SizeOverrideEnum.Instance,
			Formatter.Gas.InstrOpInfoFlagsEnum.Instance,
			Formatter.Gas.InstrOpKindEnum.Instance,
			Formatter.Intel.CtorKindEnum.Instance,
			Formatter.Intel.SizeOverrideEnum.Instance,
			Formatter.Intel.BranchSizeInfoEnum.Instance,
			Formatter.Intel.InstrOpInfoFlagsEnum.Instance,
			Formatter.Intel.InstrOpKindEnum.Instance,
			Formatter.Masm.CtorKindEnum.Instance,
			Formatter.Nasm.CtorKindEnum.Instance,
			Formatter.Masm.InstrOpInfoFlagsEnum.Instance,
			Formatter.Nasm.SignExtendInfoEnum.Instance,
			Formatter.Masm.InstrOpKindEnum.Instance,
			Formatter.Nasm.SizeOverrideEnum.Instance,
			Formatter.Nasm.BranchSizeInfoEnum.Instance,
			Formatter.Nasm.InstrOpInfoFlagsEnum.Instance,
			Formatter.Nasm.MemorySizeInfoEnum.Instance,
			Formatter.Nasm.FarMemorySizeInfoEnum.Instance,
			Formatter.Nasm.InstrOpKindEnum.Instance,
			Formatter.MemorySizeOptionsEnum.Instance,
			Formatter.NumberBaseEnum.Instance,
			Formatter.FormatMnemonicOptionsEnum.Instance,
			Formatter.PrefixKindEnum.Instance,
			Formatter.DecoratorKindEnum.Instance,
			Formatter.NumberKindEnum.Instance,
			Formatter.FormatterOutputTextKindEnum.Instance,
			Formatter.SymbolFlagsEnum.Instance,
			RoundingControlEnum.Instance,
			OpKindEnum.Instance,
			Instruction.CodeFlagsEnum.Instance,
			Instruction.MemoryFlagsEnum.Instance,
			Instruction.OpKindFlagsEnum.Instance,
			VectorLengthEnum.Instance,
			Decoder.MandatoryPrefixByteEnum.Instance,
			Decoder.StateFlagsEnum.Instance,
			Decoder.OpSizeEnum.Instance,
			EncodingKindEnum.Instance,
			InstructionInfo.FlowControlEnum.Instance,
			Encoder.OpCodeOperandKindEnum.Instance,
			InstructionInfo.RflagsBitsEnum.Instance,
			InstructionInfo.OpAccessEnum.Instance,
			InstructionInfo.MemorySizeFlagsEnum.Instance,
			InstructionInfo.RegisterFlagsEnum.Instance,
			Encoder.LegacyOpCodeTableEnum.Instance,
			Encoder.VexOpCodeTableEnum.Instance,
			Encoder.XopOpCodeTableEnum.Instance,
			Encoder.EvexOpCodeTableEnum.Instance,
			Encoder.EncodableEnum.Instance,
			Encoder.OpCodeHandlerFlagsEnum.Instance,
			Encoder.LegacyOpKindEnum.Instance,
			Encoder.VexOpKindEnum.Instance,
			Encoder.XopOpKindEnum.Instance,
			Encoder.EvexOpKindEnum.Instance,
			Encoder.MandatoryPrefixEnum.Instance,
			Encoder.OpCodeTableKindEnum.Instance,
			Encoder.OperandSizeEnum.Instance,
			Encoder.AddressSizeEnum.Instance,
			Encoder.VexVectorLengthEnum.Instance,
			Encoder.XopVectorLengthEnum.Instance,
			Encoder.EvexVectorLengthEnum.Instance,
			Encoder.DisplSizeEnum.Instance,
			Encoder.ImmSizeEnum.Instance,
			Encoder.EncoderFlagsEnum.Instance,
			Encoder.WBitEnum.Instance,
			Encoder.LKindEnum.Instance,
			Encoder.OpCodeFlagsEnum.Instance,
			Encoder.RepPrefixKindEnum.Instance,
			Encoder.RelocKindEnum.Instance,
			Encoder.BlockEncoderOptionsEnum.Instance,
		};

		public abstract void Generate(EnumType enumType);

		public void Generate() {
			foreach (var enumType in allEnums)
				Generate(enumType);
		}
	}
}
