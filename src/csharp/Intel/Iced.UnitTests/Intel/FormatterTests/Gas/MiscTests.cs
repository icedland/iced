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

#if !NO_GAS_FORMATTER && !NO_FORMATTER
using Iced.Intel;
using Iced.Intel.GasFormatterInternal;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Gas {
	public sealed class MiscTests {
		[Fact]
		void Register_is_not_too_big() {
			const int maxValue = IcedConstants.NumberOfRegisters - 1 + Registers.ExtraRegisters;
			Static.Assert(maxValue < (1 << InstrOpInfo.TEST_RegisterBits) ? 0 : -1);
			Static.Assert(maxValue >= (1 << (InstrOpInfo.TEST_RegisterBits - 1)) ? 0 : -1);
		}

		[Fact]
		void Verify_default_formatter_options() {
			var options = new GasFormatterOptions();
			Assert.False(options.UpperCasePrefixes);
			Assert.False(options.UpperCaseMnemonics);
			Assert.False(options.UpperCaseRegisters);
			Assert.False(options.UpperCaseKeywords);
			Assert.False(options.UpperCaseDecorators);
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
			Assert.Null(options.HexSuffix);
			Assert.Equal("0x", options.HexPrefix);
			Assert.Equal(4, options.HexDigitGroupSize);
			Assert.Null(options.DecimalPrefix);
			Assert.Null(options.DecimalSuffix);
			Assert.Equal(3, options.DecimalDigitGroupSize);
			Assert.Null(options.OctalSuffix);
			Assert.Equal("0", options.OctalPrefix);
			Assert.Equal(4, options.OctalDigitGroupSize);
			Assert.Null(options.BinarySuffix);
			Assert.Equal("0b", options.BinaryPrefix);
			Assert.Equal(4, options.BinaryDigitGroupSize);
			Assert.Null(options.DigitSeparator);
			Assert.False(options.LeadingZeroes);
			Assert.True(options.UpperCaseHex);
			Assert.True(options.SmallHexNumbersInDecimal);
			Assert.True(options.AddLeadingZeroToHexNumbers);
			Assert.Equal(NumberBase.Hexadecimal, options.NumberBase);
			Assert.True(options.BranchLeadingZeroes);
			Assert.False(options.SignedImmediateOperands);
			Assert.True(options.SignedMemoryDisplacements);
			Assert.False(options.SignExtendMemoryDisplacements);
			Assert.Equal(MemorySizeOptions.Default, options.MemorySizeOptions);
			Assert.False(options.RipRelativeAddresses);
			Assert.True(options.ShowBranchSize);
			Assert.True(options.UsePseudoOps);
			Assert.False(options.ShowSymbolAddress);
			Assert.False(options.NakedRegisters);
			Assert.False(options.ShowMnemonicSizeSuffix);
			Assert.False(options.SpaceAfterMemoryOperandComma);
		}

		[Theory]
		[InlineData("10 08", Code.Adc_rm8_r8, 64, "adc", FormatMnemonicOptions.None)]
		[InlineData("10 08", Code.Adc_rm8_r8, 64, "adc", FormatMnemonicOptions.NoPrefixes)]
		[InlineData("10 08", Code.Adc_rm8_r8, 64, "", FormatMnemonicOptions.NoMnemonic)]
		[InlineData("10 08", Code.Adc_rm8_r8, 64, "", FormatMnemonicOptions.NoPrefixes | FormatMnemonicOptions.NoMnemonic)]

		[InlineData("F0 10 08", Code.Adc_rm8_r8, 64, "lock adc", FormatMnemonicOptions.None)]
		[InlineData("F0 10 08", Code.Adc_rm8_r8, 64, "adc", FormatMnemonicOptions.NoPrefixes)]
		[InlineData("F0 10 08", Code.Adc_rm8_r8, 64, "lock", FormatMnemonicOptions.NoMnemonic)]

		[InlineData("F3 6C", Code.Insb_m8_DX, 64, "rep insb", FormatMnemonicOptions.None)]
		[InlineData("F3 6C", Code.Insb_m8_DX, 64, "insb", FormatMnemonicOptions.NoPrefixes)]
		[InlineData("F3 6C", Code.Insb_m8_DX, 64, "rep", FormatMnemonicOptions.NoMnemonic)]

		[InlineData("F2 A6", Code.Cmpsb_m8_m8, 64, "repne cmpsb", FormatMnemonicOptions.None)]
		[InlineData("F2 A6", Code.Cmpsb_m8_m8, 64, "cmpsb", FormatMnemonicOptions.NoPrefixes)]
		[InlineData("F2 A6", Code.Cmpsb_m8_m8, 64, "repne", FormatMnemonicOptions.NoMnemonic)]

		[InlineData("F2 F0 10 08", Code.Adc_rm8_r8, 64, "xacquire lock adc", FormatMnemonicOptions.None)]
		[InlineData("F2 F0 10 08", Code.Adc_rm8_r8, 64, "adc", FormatMnemonicOptions.NoPrefixes)]
		[InlineData("F2 F0 10 08", Code.Adc_rm8_r8, 64, "xacquire lock", FormatMnemonicOptions.NoMnemonic)]

		[InlineData("2E 70 00", Code.Jo_rel8_64, 64, "jo,pn", FormatMnemonicOptions.None)]
		[InlineData("2E 70 00", Code.Jo_rel8_64, 64, "jo", FormatMnemonicOptions.NoPrefixes)]
		[InlineData("2E 70 00", Code.Jo_rel8_64, 64, ",pn", FormatMnemonicOptions.NoMnemonic)]

		[InlineData("F2 70 00", Code.Jo_rel8_64, 64, "bnd jo", FormatMnemonicOptions.None)]
		[InlineData("F2 70 00", Code.Jo_rel8_64, 64, "jo", FormatMnemonicOptions.NoPrefixes)]
		[InlineData("F2 70 00", Code.Jo_rel8_64, 64, "bnd", FormatMnemonicOptions.NoMnemonic)]

		[InlineData("3E FF 10", Code.Call_rm64, 64, "notrack callq", FormatMnemonicOptions.None)]
		[InlineData("3E FF 10", Code.Call_rm64, 64, "callq", FormatMnemonicOptions.NoPrefixes)]
		[InlineData("3E FF 10", Code.Call_rm64, 64, "notrack", FormatMnemonicOptions.NoMnemonic)]
		void FormatMnemonicOptions1(string hexBytes, Code code, int bitness, string formattedString, FormatMnemonicOptions options) {
			var decoder = Decoder.Create(bitness, new ByteArrayCodeReader(hexBytes));
			decoder.Decode(out var instr);
			Assert.Equal(code, instr.Code);
			var formatter = GasFormatterFactory.Create();
			var output = new StringBuilderFormatterOutput();
			formatter.FormatMnemonic(instr, output, options);
			var actualFormattedString = output.ToStringAndReset();
#pragma warning disable xUnit2006 // Do not use invalid string equality check
			// Show the full string without ellipses by using Equal<string>() instead of Equal()
			Assert.Equal<string>(formattedString, actualFormattedString);
#pragma warning restore xUnit2006 // Do not use invalid string equality check
		}

		[Fact]
		void TestFormattingWithDefaultFormatterCtor() => FormatterTestUtils.TestFormatterDoesNotThrow(new GasFormatter());
	}
}
#endif
