/*
    Copyright (C) 2018 de4dot@gmail.com

    This file is part of Iced.

    Iced is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    Iced is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Lesser General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with Iced.  If not, see <https://www.gnu.org/licenses/>.
*/

#if !NO_INTEL_FORMATTER && !NO_FORMATTER
using System;
using Iced.Intel;
using Iced.Intel.IntelFormatterInternal;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Intel {
	public sealed class MiscTests {
		static int GetEnumSize(Type enumType) {
			Assert.True(enumType.IsEnum);
			int maxValue = -1;
			foreach (var f in enumType.GetFields()) {
				if (f.IsLiteral) {
					int value = (int)f.GetValue(null);
					Assert.Equal(maxValue + 1, value);
					maxValue = value;
				}
			}
			return maxValue + 1;
		}

		[Fact]
		void Register_is_not_too_big() {
			int maxValue = GetEnumSize(typeof(Register)) - 1;
			maxValue += Registers.ExtraRegisters;
			Assert.True(maxValue < (1 << InstrOpInfo.TEST_RegisterBits));
			Assert.True(maxValue >= (1 << (InstrOpInfo.TEST_RegisterBits - 1)));
		}

		[Fact]
		void Verify_InstrInfos_have_valid_Code_values() {
			var infos = InstrInfos.AllInfos;
			for (int i = 0; i < infos.Length; i++) {
				var expectedCodeValue = (Code)i;
				Assert.Equal(expectedCodeValue, infos[i].TEST_Code);
			}
		}

		[Fact]
		void Verify_MemorySizes_have_valid_MemorySize_values() {
			var infos = MemorySizes.AllMemorySizes;
			for (int i = 0; i < infos.Length; i++) {
				var expectedCodeValue = (MemorySize)i;
				Assert.Equal(expectedCodeValue, infos[i].memorySize);
			}
		}

		[Fact]
		void Verify_default_formatter_options() {
			var options = new IntelFormatterOptions();
			Assert.False(options.UpperCasePrefixes);
			Assert.False(options.UpperCaseMnemonics);
			Assert.False(options.UpperCaseRegisters);
			Assert.False(options.UpperCaseKeywords);
			Assert.False(options.UpperCaseOther);
			Assert.False(options.UpperCaseAll);
			Assert.Equal(0, options.FirstOperandCharIndex);
			Assert.Equal(0, options.TabSize);
			Assert.False(options.SpaceAfterOperandSeparator);
			Assert.False(options.SpaceAfterMemoryBracket);
			Assert.False(options.SpaceBetweenMemoryAddOperators);
			Assert.False(options.SpaceBetweenMemoryMulOperators);
			Assert.False(options.ScaleBeforeIndex);
			Assert.False(options.AlwaysShowScale);
			Assert.False(options.AlwaysShowSegmentRegister);
			Assert.False(options.ShowZeroDisplacements);
			Assert.Null(options.HexPrefix);
			Assert.Equal("h", options.HexSuffix);
			Assert.Equal(4, options.HexDigitGroupSize);
			Assert.Null(options.DecimalPrefix);
			Assert.Null(options.DecimalSuffix);
			Assert.Equal(3, options.DecimalDigitGroupSize);
			Assert.Null(options.OctalPrefix);
			Assert.Equal("o", options.OctalSuffix);
			Assert.Equal(4, options.OctalDigitGroupSize);
			Assert.Null(options.BinaryPrefix);
			Assert.Equal("b", options.BinarySuffix);
			Assert.Equal(4, options.BinaryDigitGroupSize);
			Assert.Null(options.DigitSeparator);
			Assert.True(options.ShortNumbers);
			Assert.True(options.UpperCaseHex);
			Assert.True(options.SmallHexNumbersInDecimal);
			Assert.True(options.AddLeadingZeroToHexNumbers);
			Assert.Equal(NumberBase.Hexadecimal, options.NumberBase);
			Assert.False(options.ShortBranchNumbers);
			Assert.False(options.SignedImmediateOperands);
			Assert.True(options.SignedMemoryDisplacements);
			Assert.False(options.SignExtendMemoryDisplacements);
			Assert.Equal(MemorySizeOptions.Default, options.MemorySizeOptions);
			Assert.False(options.RipRelativeAddresses);
			Assert.True(options.ShowBranchSize);
			Assert.True(options.UsePseudoOps);
			Assert.False(options.ShowSymbolAddress);
		}
	}
}
#endif
