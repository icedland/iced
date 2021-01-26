// SPDX-License-Identifier: MIT
// Copyright wtfsckgh@gmail.com
// Copyright iced contributors

#if GAS || INTEL || MASM || NASM
using System;

namespace Iced.UnitTests.Intel.FormatterTests {
	static class OptionsParser {
		static readonly char[] kvseps = new char[] { '=' };
		public static (OptionsProps property, object value) ParseOption(string keyValue) {
			var kv = keyValue.Split(kvseps, 2);
			if (kv.Length != 2)
				throw new InvalidOperationException($"Expected key=value: '{keyValue}'");
			var valueStr = kv[1].Trim();
			var prop = ToEnumConverter.GetOptionsProps(kv[0].Trim());
			object value;
			switch (prop) {
			case OptionsProps.AddLeadingZeroToHexNumbers:
			case OptionsProps.AlwaysShowScale:
			case OptionsProps.AlwaysShowSegmentRegister:
			case OptionsProps.BranchLeadingZeroes:
			case OptionsProps.DisplacementLeadingZeroes:
			case OptionsProps.GasNakedRegisters:
			case OptionsProps.GasShowMnemonicSizeSuffix:
			case OptionsProps.GasSpaceAfterMemoryOperandComma:
			case OptionsProps.LeadingZeroes:
			case OptionsProps.MasmAddDsPrefix32:
			case OptionsProps.NasmShowSignExtendedImmediateSize:
			case OptionsProps.PreferST0:
			case OptionsProps.RipRelativeAddresses:
			case OptionsProps.ScaleBeforeIndex:
			case OptionsProps.ShowBranchSize:
			case OptionsProps.ShowSymbolAddress:
			case OptionsProps.ShowZeroDisplacements:
			case OptionsProps.SignedImmediateOperands:
			case OptionsProps.SignedMemoryDisplacements:
			case OptionsProps.SmallHexNumbersInDecimal:
			case OptionsProps.SpaceAfterMemoryBracket:
			case OptionsProps.SpaceAfterOperandSeparator:
			case OptionsProps.SpaceBetweenMemoryAddOperators:
			case OptionsProps.SpaceBetweenMemoryMulOperators:
			case OptionsProps.UppercaseAll:
			case OptionsProps.UppercaseDecorators:
			case OptionsProps.UppercaseHex:
			case OptionsProps.UppercaseKeywords:
			case OptionsProps.UppercaseMnemonics:
			case OptionsProps.UppercasePrefixes:
			case OptionsProps.UppercaseRegisters:
			case OptionsProps.UsePseudoOps:
			case OptionsProps.ShowUselessPrefixes:
				value = NumberConverter.ToBoolean(valueStr);
				break;

			case OptionsProps.BinaryDigitGroupSize:
			case OptionsProps.DecimalDigitGroupSize:
			case OptionsProps.FirstOperandCharIndex:
			case OptionsProps.HexDigitGroupSize:
			case OptionsProps.OctalDigitGroupSize:
			case OptionsProps.TabSize:
				value = NumberConverter.ToInt32(valueStr);
				break;

			case OptionsProps.IP:
				value = NumberConverter.ToUInt64(valueStr);
				break;

			case OptionsProps.BinaryPrefix:
			case OptionsProps.BinarySuffix:
			case OptionsProps.DecimalPrefix:
			case OptionsProps.DecimalSuffix:
			case OptionsProps.DigitSeparator:
			case OptionsProps.HexPrefix:
			case OptionsProps.HexSuffix:
			case OptionsProps.OctalPrefix:
			case OptionsProps.OctalSuffix:
				value = valueStr == "<null>" ? null : valueStr;
				break;

			case OptionsProps.MemorySizeOptions:
				value = ToEnumConverter.GetMemorySizeOptions(valueStr);
				break;

			case OptionsProps.NumberBase:
				value = ToEnumConverter.GetNumberBase(valueStr);
				break;

			case OptionsProps.CC_b:
				value = ToEnumConverter.GetCC_b(valueStr);
				break;

			case OptionsProps.CC_ae:
				value = ToEnumConverter.GetCC_ae(valueStr);
				break;

			case OptionsProps.CC_e:
				value = ToEnumConverter.GetCC_e(valueStr);
				break;

			case OptionsProps.CC_ne:
				value = ToEnumConverter.GetCC_ne(valueStr);
				break;

			case OptionsProps.CC_be:
				value = ToEnumConverter.GetCC_be(valueStr);
				break;

			case OptionsProps.CC_a:
				value = ToEnumConverter.GetCC_a(valueStr);
				break;

			case OptionsProps.CC_p:
				value = ToEnumConverter.GetCC_p(valueStr);
				break;

			case OptionsProps.CC_np:
				value = ToEnumConverter.GetCC_np(valueStr);
				break;

			case OptionsProps.CC_l:
				value = ToEnumConverter.GetCC_l(valueStr);
				break;

			case OptionsProps.CC_ge:
				value = ToEnumConverter.GetCC_ge(valueStr);
				break;

			case OptionsProps.CC_le:
				value = ToEnumConverter.GetCC_le(valueStr);
				break;

			case OptionsProps.CC_g:
				value = ToEnumConverter.GetCC_g(valueStr);
				break;

			case OptionsProps.DecoderOptions:
				value = ToEnumConverter.GetDecoderOptions(valueStr);
				break;

			default:
				throw new InvalidOperationException();
			}
			return (prop, value);
		}
	}
}
#endif
