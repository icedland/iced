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

#if !NO_MASM_FORMATTER && !NO_FORMATTER
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests.Masm {
	public sealed class MasmOptionsTests : OptionsTests {
		[Theory]
		[MemberData(nameof(Format_Data))]
		void Format(int index, OptionsInstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, MasmFormatterFactory.Create_Options());
		public static IEnumerable<object[]> Format_Data => GetFormatData("Masm", nameof(MasmOptionsTests));

		[Theory]
		[MemberData(nameof(Format2_Data))]
		void Format2(int index, OptionsInstructionInfo info, string formattedString) => FormatBase(index, info, formattedString, MasmFormatterFactory.Create_Options());
		public static IEnumerable<object[]> Format2_Data {
			get {
				yield return new object[] {
					0,
					new OptionsInstructionInfo(16, "8B 0E 3412", Code.Mov_r16_rm16, a => ((MasmFormatterOptions)a).AddDsPrefix32 = true),
					"mov cx,ds:[1234h]",
				};
				yield return new object[] {
					1,
					new OptionsInstructionInfo(16, "8B 0E 3412", Code.Mov_r16_rm16, a => ((MasmFormatterOptions)a).AddDsPrefix32 = false),
					"mov cx,[1234h]",
				};
				yield return new object[] {
					2,
					new OptionsInstructionInfo(16, "A1 3412", Code.Mov_AX_moffs16, a => ((MasmFormatterOptions)a).AddDsPrefix32 = true),
					"mov ax,ds:[1234h]",
				};
				yield return new object[] {
					3,
					new OptionsInstructionInfo(16, "A1 3412", Code.Mov_AX_moffs16, a => ((MasmFormatterOptions)a).AddDsPrefix32 = false),
					"mov ax,[1234h]",
				};
				yield return new object[] {
					4,
					new OptionsInstructionInfo(32, "8B 0D 78563412", Code.Mov_r32_rm32, a => ((MasmFormatterOptions)a).AddDsPrefix32 = true),
					"mov ecx,ds:[12345678h]",
				};
				yield return new object[] {
					5,
					new OptionsInstructionInfo(32, "8B 0D 78563412", Code.Mov_r32_rm32, a => ((MasmFormatterOptions)a).AddDsPrefix32 = false),
					"mov ecx,[12345678h]",
				};
				yield return new object[] {
					6,
					new OptionsInstructionInfo(32, "A1 78563412", Code.Mov_EAX_moffs32, a => ((MasmFormatterOptions)a).AddDsPrefix32 = true),
					"mov eax,ds:[12345678h]",
				};
				yield return new object[] {
					7,
					new OptionsInstructionInfo(32, "A1 78563412", Code.Mov_EAX_moffs32, a => ((MasmFormatterOptions)a).AddDsPrefix32 = false),
					"mov eax,[12345678h]",
				};
				yield return new object[] {
					8,
					new OptionsInstructionInfo(64, "8B 0C 25 78563412", Code.Mov_r32_rm32, a => ((MasmFormatterOptions)a).AddDsPrefix32 = true),
					"mov ecx,[12345678h]",
				};
				yield return new object[] {
					9,
					new OptionsInstructionInfo(64, "8B 0C 25 78563412", Code.Mov_r32_rm32, a => ((MasmFormatterOptions)a).AddDsPrefix32 = false),
					"mov ecx,[12345678h]",
				};
				yield return new object[] {
					10,
					new OptionsInstructionInfo(64, "A1 F0DEBC9A78563412", Code.Mov_EAX_moffs32, a => ((MasmFormatterOptions)a).AddDsPrefix32 = true),
					"mov eax,[123456789ABCDEF0h]",
				};
				yield return new object[] {
					11,
					new OptionsInstructionInfo(64, "A1 F0DEBC9A78563412", Code.Mov_EAX_moffs32, a => ((MasmFormatterOptions)a).AddDsPrefix32 = false),
					"mov eax,[123456789ABCDEF0h]",
				};
			}
		}

		[Fact]
		public void TestOptions() {
			var options = new MasmFormatterOptions();
			TestOptionsBase(options);
		}
	}
}
#endif
