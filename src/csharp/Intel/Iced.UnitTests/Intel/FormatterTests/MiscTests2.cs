// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS || INTEL || MASM || NASM || FAST_FMT
using Iced.Intel;
using Iced.UnitTests.Intel.DecoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	public sealed class MiscTests2 {
		[Fact]
		void Make_sure_all_Code_values_are_formatted() {
			var tested = new byte[IcedConstants.CodeEnumCount];

			var allArgs = new (int bitness, bool isMisc)[] {
				(16, false),
				(32, false),
				(64, false),
				(16, true),
				(32, true),
				(64, true),
			};
			foreach (var args in allArgs) {
				var data = FormatterTestCases.GetTests(args.bitness, args.isMisc);
				foreach (var tc in data.tests)
					tested[(int)tc.Code] = 1;
			}
#if ENCODER
			foreach (var tc in NonDecodedInstructions.GetTests())
				tested[(int)tc.instruction.Code] = 1;
#else
			foreach (var code in CodeValueTests.NonDecodedCodeValues1632)
				tested[(int)code] = 1;
			foreach (var code in CodeValueTests.NonDecodedCodeValues)
				tested[(int)code] = 1;
#endif

			var sb = new System.Text.StringBuilder();
			int missing = 0;
			var codeNames = ToEnumConverter.GetCodeNames();
			for (int i = 0; i < tested.Length; i++) {
				if (tested[i] != 1 && !CodeUtils.IsIgnored(codeNames[i])) {
					sb.Append(codeNames[i] + " ");
					missing++;
				}
			}
			Assert.Equal("Fmt: 0 ins ", $"Fmt: {missing} ins " + sb.ToString());
		}

		[Fact]
		void Instruction_ToString() {
			var decoder = Decoder.Create(64, new byte[] { 0x00, 0xCE }, DecoderOptions.None);
			var instr = decoder.Decode();
			string expected;
			// The order of #if/elif must be the same as in Instruction.ToString()
#if MASM
			expected = "add dh,cl";
#elif NASM
			expected = "add dh,cl";
#elif INTEL
			expected = "add dh,cl";
#elif GAS
			expected = "add %cl,%dh";
#elif FAST_FMT
			expected = "add dh,cl";
#else
#error No formatter
#endif
			var actual = instr.ToString();
			Assert.Equal(expected, actual);
		}
	}
}
#endif
