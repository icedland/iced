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

#if INSTR_INFO
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	public sealed class CodeValueTests {
		[Fact]
		void Make_sure_all_Code_values_are_tested() {
			var tested = new bool[IcedConstants.NumberOfCodeValues];

			foreach (var info in GetTests())
				tested[(int)(Code)info[1]] = true;

			var sb = new StringBuilder();
			int missing = 0;
			var codeNames = ToEnumConverter.GetCodeNames().ToArray();
			Assert.Equal(tested.Length, codeNames.Length);
			for (int i = 0; i < tested.Length; i++) {
				if (!tested[i] && !CodeUtils.IsIgnored(codeNames[i])) {
					sb.Append(codeNames[i] + " ");
					missing++;
				}
			}
			Assert.Equal("0 ins ", $"{missing} ins " + sb.ToString());
		}

		static IEnumerable<object[]> GetTests() {
			foreach (var info in InstructionInfoTest_16.Test16_InstructionInfo_Data)
				yield return info;
			foreach (var info in InstructionInfoTest_32.Test32_InstructionInfo_Data)
				yield return info;
			foreach (var info in InstructionInfoTest_64.Test64_InstructionInfo_Data)
				yield return info;
		}
	}
}
#endif
