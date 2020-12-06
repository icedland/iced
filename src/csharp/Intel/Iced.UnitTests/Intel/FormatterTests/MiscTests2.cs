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
				var data = FormatterTestCases.GetInstructionInfos(args.bitness, args.isMisc);
				foreach (var info in data.infos)
					tested[(int)info.Code] = 1;
			}
#if ENCODER
			foreach (var info in NonDecodedInstructions.GetTests())
				tested[(int)info.instruction.Code] = 1;
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
