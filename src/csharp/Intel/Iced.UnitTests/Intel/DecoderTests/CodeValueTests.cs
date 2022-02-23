// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

using System;
using System.Text;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.DecoderTests {
	public sealed class CodeValueTests {
		public static readonly Code[] NonDecodedCodeValues1632 = new Code[] {
			Code.Popw_CS,
			Code.Fstdw_AX,
			Code.Fstsg_AX,
		};
		public static readonly Code[] NonDecodedCodeValues = new Code[] {
			Code.DeclareByte,
			Code.DeclareDword,
			Code.DeclareQword,
			Code.DeclareWord,
			Code.Zero_bytes,
			Code.Fclex,
			Code.Fdisi,
			Code.Feni,
			Code.Finit,
			Code.Fsave_m108byte,
			Code.Fsave_m94byte,
			Code.Fsetpm,
			Code.Fstcw_m2byte,
			Code.Fstenv_m14byte,
			Code.Fstenv_m28byte,
			Code.Fstsw_AX,
			Code.Fstsw_m2byte,
		};

		[Fact]
		void Make_sure_all_Code_values_are_tested_in_16_32_64_bit_modes() {
			const byte T16 = 0x01;
			const byte T32 = 0x02;
			const byte T64 = 0x04;
			var tested = new byte[IcedConstants.CodeEnumCount];
			tested[(int)Code.INVALID] = T16 | T32 | T64;

			foreach (var info in DecoderTestUtils.GetDecoderTests(includeOtherTests: false, includeInvalid: false)) {
				Assert.False(DecoderTestUtils.NotDecoded.Contains(info.Code), $"{info.Code} has a decoder test but it shouldn't be decoded");

				tested[(int)info.Code] |= info.Bitness switch {
					16 => T16,
					32 => T32,
					64 => T64,
					_ => throw new InvalidOperationException(),
				};
			}

#if ENCODER
			foreach (var info in NonDecodedInstructions.GetTests()) {
				tested[(int)info.instruction.Code] |= info.bitness switch {
					16 => T16,
					32 => T32,
					64 => T64,
					_ => throw new InvalidOperationException(),
				};
			}
#else
			foreach (var code in NonDecodedCodeValues1632)
				tested[(int)code] |= T16 | T32;
			foreach (var code in NonDecodedCodeValues)
				tested[(int)code] |= T16 | T32 | T64;
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
			var codeNames = ToEnumConverter.GetCodeNames();
			Assert.Equal(tested.Length, codeNames.Length);
			for (int i = 0; i < tested.Length; i++) {
				if (tested[i] != (T16 | T32 | T64) && !CodeUtils.IsIgnored(codeNames[i])) {
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
