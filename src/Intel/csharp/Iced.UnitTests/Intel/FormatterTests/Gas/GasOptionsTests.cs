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
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Gas {
	public sealed class GasOptionsTests : OptionsTests {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, OptionsInstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, GasFormatterFactory.Create_Options());
		public static IEnumerable<object[]> Format_Data => GetFormatData("Gas", nameof(GasOptionsTests));

		[Theory]
		[MemberData(nameof(Format2_Data))]
		void Format2(int index, OptionsInstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, GasFormatterFactory.Create_Options());
		public static IEnumerable<object[]> Format2_Data {
			get {
				yield return new object[] {
					0,
					new OptionsInstructionInfo(64, "48 01 CE", Code.Add_rm64_r64, a => ((GasFormatterOptions)a).NakedRegisters = true),
					"add rcx,rsi",
				};
				yield return new object[] {
					1,
					new OptionsInstructionInfo(64, "48 01 CE", Code.Add_rm64_r64, a => ((GasFormatterOptions)a).NakedRegisters = false),
					"add %rcx,%rsi",
				};
				yield return new object[] {
					2,
					new OptionsInstructionInfo(64, "48 01 CE", Code.Add_rm64_r64, a => ((GasFormatterOptions)a).ShowMnemonicSizeSuffix = true),
					"addq %rcx,%rsi",
				};
				yield return new object[] {
					3,
					new OptionsInstructionInfo(64, "48 01 CE", Code.Add_rm64_r64, a => ((GasFormatterOptions)a).ShowMnemonicSizeSuffix = false),
					"add %rcx,%rsi",
				};
				yield return new object[] {
					4,
					new OptionsInstructionInfo(32, "66 67 E0 5A", Code.Loopne_rel8_16_CX, a => a.UpperCaseKeywords = true),
					".BYTE 0x66; loopnew 0x004e",
				};
				yield return new object[] {
					5,
					new OptionsInstructionInfo(32, "66 67 E0 5A", Code.Loopne_rel8_16_CX, a => a.UpperCaseKeywords = false),
					".byte 0x66; loopnew 0x004e",
				};
				yield return new object[] {
					6,
					new OptionsInstructionInfo(32, "66 67 E0 5A", Code.Loopne_rel8_16_CX, a => a.UpperCaseAll = true),
					".BYTE 0x66; LOOPNEW 0x004e",
				};
				yield return new object[] {
					7,
					new OptionsInstructionInfo(32, "66 67 E0 5A", Code.Loopne_rel8_16_CX, a => a.UpperCaseAll = false),
					".byte 0x66; loopnew 0x004e",
				};
				yield return new object[] {
					8,
					new OptionsInstructionInfo(64, "2E 70 00", Code.Jo_rel8_64, a => a.UpperCasePrefixes = true),
					"jo,PN 0x7ffffffffffffff3",
				};
				yield return new object[] {
					9,
					new OptionsInstructionInfo(64, "2E 70 00", Code.Jo_rel8_64, a => a.UpperCasePrefixes = false),
					"jo,pn 0x7ffffffffffffff3",
				};
				yield return new object[] {
					10,
					new OptionsInstructionInfo(64, "2E 70 00", Code.Jo_rel8_64, a => a.UpperCaseAll = true),
					"JO,PN 0x7ffffffffffffff3",
				};
				yield return new object[] {
					11,
					new OptionsInstructionInfo(64, "2E 70 00", Code.Jo_rel8_64, a => a.UpperCaseAll = false),
					"jo,pn 0x7ffffffffffffff3",
				};
				yield return new object[] {
					12,
					new OptionsInstructionInfo(64, "48 8B 0C 90", Code.Mov_r64_rm64, a => ((GasFormatterOptions)a).SpaceAfterMemoryOperandComma = true),
					"mov (%rax, %rdx, 4),%rcx",
				};
				yield return new object[] {
					13,
					new OptionsInstructionInfo(64, "48 8B 0C 90", Code.Mov_r64_rm64, a => ((GasFormatterOptions)a).SpaceAfterMemoryOperandComma = false),
					"mov (%rax,%rdx,4),%rcx",
				};
			}
		}

		[Fact]
		public void TestOptions() {
			var options = new GasFormatterOptions();
			TestOptionsBase(options);
		}
	}
}
#endif
