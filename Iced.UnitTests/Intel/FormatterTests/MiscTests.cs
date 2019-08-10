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

#if (!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER
using System;
using System.Text;
using Iced.Intel;
using Iced.UnitTests.Intel.DecoderTests;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	public sealed class MiscTests {
		[Fact]
		void Make_sure_all_Code_values_are_formatted() {
			int numCodeValues = -1;
			foreach (var f in typeof(Code).GetFields()) {
				if (f.IsLiteral) {
					int value = (int)f.GetValue(null);
					Assert.Equal(numCodeValues + 1, value);
					numCodeValues = value;
				}
			}
			numCodeValues++;
			var tested = new byte[numCodeValues];

			var allArgs = new (int bitness, bool isMisc)[] {
				(16, false),
				(32, false),
				(64, false),
				(16, true),
				(32, true),
				(64, true),
			};
			foreach (var args in allArgs) {
				var infos = FormatterTest.GetInstructionInfos(args.bitness, args.isMisc);
				foreach (var info in infos)
					tested[(int)info.Code] = 1;
			}
#if !NO_ENCODER
			foreach (var info in NonDecodedInstructions.GetTests())
				tested[(int)info.instruction.Code] = 1;
#endif

			var sb = new StringBuilder();
			int missing = 0;
			var codeNames = Enum.GetNames(typeof(Code));
			for (int i = 0; i < tested.Length; i++) {
				if (tested[i] != 1) {
					sb.Append(codeNames[i] + " ");
					missing++;
				}
			}
			Assert.Equal("Fmt: 0 ins ", $"Fmt: {missing} ins " + sb.ToString());
		}
	}
}
#endif
