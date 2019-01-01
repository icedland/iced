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
using System.Text;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.DecoderTests {
	public sealed class CodeValueTests {
		[Fact]
		void Make_sure_all_Code_values_are_tested_in_16_32_64_bit_modes() {
			int numCodeValues = -1;
			foreach (var f in typeof(Code).GetFields()) {
				if (f.IsLiteral) {
					int value = (int)f.GetValue(null);
					Assert.Equal(numCodeValues + 1, value);
					numCodeValues = value;
				}
			}
			numCodeValues++;
			const byte T16 = 0x01;
			const byte T32 = 0x02;
			const byte T64 = 0x04;
			var tested = new byte[numCodeValues];
			tested[(int)Code.INVALID] = T16 | T32 | T64;

			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false)) {
				Assert.False(DecoderTestUtils.NotDecoded.Contains(info.Code), $"{info.Code} has a decoder test but it shouldn't be decoded");

				byte testedFlags;
				if (info.Bitness == 16)
					testedFlags = T16;
				else if (info.Bitness == 32)
					testedFlags = T32;
				else if (info.Bitness == 64)
					testedFlags = T64;
				else
					continue;

				tested[(int)info.Code] |= testedFlags;
			}

#if !NO_ENCODER
			foreach (var info in NonDecodedInstructions.GetTests()) {
				byte testedFlags;
				if (info.bitness == 16)
					testedFlags = T16;
				else if (info.bitness == 32)
					testedFlags = T32;
				else if (info.bitness == 64)
					testedFlags = T64;
				else
					continue;

				tested[(int)info.instruction.Code] |= testedFlags;
			}
#endif

			foreach (var c in DecoderTestUtils.NotDecoded) {
				Assert.DoesNotContain(c, DecoderTestUtils.Code32Only);
				Assert.DoesNotContain(c, DecoderTestUtils.Code64Only);
			}

			foreach (var c in DecoderTestUtils.NotDecoded32Only)
				tested[(int)c] ^= T64;
			foreach (var c in DecoderTestUtils.NotDecoded64Only)
				tested[(int)c] ^= T16 | T32;

			foreach (var c in DecoderTestUtils.Code32Only) {
				Assert.DoesNotContain(c, DecoderTestUtils.Code64Only);
				tested[(int)c] ^= T64;
			}

			foreach (var c in DecoderTestUtils.Code64Only) {
				Assert.DoesNotContain(c, DecoderTestUtils.Code32Only);
				tested[(int)c] ^= T16 | T32;
			}

			var sb16 = new StringBuilder();
			var sb32 = new StringBuilder();
			var sb64 = new StringBuilder();
			int missing16 = 0, missing32 = 0, missing64 = 0;
			var codeNames = Enum.GetNames(typeof(Code));
			Assert.Equal(tested.Length, codeNames.Length);
			for (int i = 0; i < tested.Length; i++) {
				if (tested[i] != (T16 | T32 | T64)) {
					if ((tested[i] & T16) == 0) {
						sb16.Append(codeNames[i] + " ");
						missing16++;
					}
					if ((tested[i] & T32) == 0) {
						sb32.Append(codeNames[i] + " ");
						missing32++;
					}
					if ((tested[i] & T64) == 0) {
						sb64.Append(codeNames[i] + " ");
						missing64++;
					}
				}
			}
			Assert.Equal("16: 0 ins ", $"16: {missing16} ins " + sb16.ToString());
			Assert.Equal("32: 0 ins ", $"32: {missing32} ins " + sb32.ToString());
			Assert.Equal("64: 0 ins ", $"64: {missing64} ins " + sb64.ToString());
		}
	}
}
