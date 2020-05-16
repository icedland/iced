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

using System.Linq;

namespace Generator.Enums.Formatter {
	enum OptionsProps {
		AddLeadingZeroToHexNumbers,
		AlwaysShowScale,
		AlwaysShowSegmentRegister,
		BinaryDigitGroupSize,
		BinaryPrefix,
		BinarySuffix,
		BranchLeadingZeroes,
		DecimalDigitGroupSize,
		DecimalPrefix,
		DecimalSuffix,
		DigitSeparator,
		DisplacementLeadingZeroes,
		FirstOperandCharIndex,
		GasNakedRegisters,
		GasShowMnemonicSizeSuffix,
		GasSpaceAfterMemoryOperandComma,
		HexDigitGroupSize,
		HexPrefix,
		HexSuffix,
		IP,
		LeadingZeroes,
		MasmAddDsPrefix32,
		MemorySizeOptions,
		NasmShowSignExtendedImmediateSize,
		NumberBase,
		OctalDigitGroupSize,
		OctalPrefix,
		OctalSuffix,
		PreferST0,
		RipRelativeAddresses,
		ScaleBeforeIndex,
		ShowBranchSize,
		ShowSymbolAddress,
		ShowZeroDisplacements,
		SignedImmediateOperands,
		SignedMemoryDisplacements,
		SmallHexNumbersInDecimal,
		SpaceAfterMemoryBracket,
		SpaceAfterOperandSeparator,
		SpaceBetweenMemoryAddOperators,
		SpaceBetweenMemoryMulOperators,
		TabSize,
		UppercaseAll,
		UppercaseDecorators,
		UppercaseHex,
		UppercaseKeywords,
		UppercaseMnemonics,
		UppercasePrefixes,
		UppercaseRegisters,
		UsePseudoOps,
		CC_b,
		CC_ae,
		CC_e,
		CC_ne,
		CC_be,
		CC_a,
		CC_p,
		CC_np,
		CC_l,
		CC_ge,
		CC_le,
		CC_g,
		DecoderOptions,
	}

	static class OptionsPropsEnum {
		const string? documentation = null;

		static EnumValue[] GetValues() =>
			typeof(OptionsProps).GetFields().Where(a => a.IsLiteral).Select(a => new EnumValue((uint)(OptionsProps)a.GetValue(null)!, a.Name, CommentAttribute.GetDocumentation(a))).ToArray();

		public static readonly EnumType Instance = new EnumType(TypeIds.OptionsProps, documentation, GetValues(), EnumTypeFlags.None);
	}
}
