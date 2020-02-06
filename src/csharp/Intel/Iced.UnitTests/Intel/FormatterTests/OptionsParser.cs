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

#if !NO_GAS || !NO_INTEL || !NO_MASM || !NO_NASM
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

			default:
				throw new InvalidOperationException();
			}
			return (prop, value);
		}
	}
}
#endif
