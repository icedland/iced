/*
    Copyright (C) 2018-2019 de4dot@gmail.com

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

#if !NO_INSTR_INFO
using System;
using System.Collections.Generic;
using System.Text;
using Iced.Intel;
using Iced.Intel.InstructionInfoInternal;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	public sealed class CodeValueTests {
		[Fact]
		void Make_sure_all_Code_values_are_tested() {
			int numCodeValues = -1;
			foreach (var f in typeof(Code).GetFields()) {
				if (f.IsLiteral) {
					int value = (int)f.GetValue(null);
					Assert.Equal(numCodeValues + 1, value);
					numCodeValues = value;
				}
			}
			numCodeValues++;
			var tested = new bool[numCodeValues];

			foreach (var info in GetTests())
				tested[(int)(Code)info[1]] = true;

			var sb = new StringBuilder();
			int missing = 0;
			var codeNames = Enum.GetNames(typeof(Code));
			Assert.Equal(tested.Length, codeNames.Length);
			for (int i = 0; i < tested.Length; i++) {
				if (!tested[i]) {
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

		[Fact]
		void Verify_Code_values_in_InfoHandlers_table() {
			Assert.Equal(InfoHandlers.Data.Length, 2 * Iced.Intel.DecoderConstants.NumberOfCodeValues);
			for (int i = 0; i < Iced.Intel.DecoderConstants.NumberOfCodeValues; i++) {
				var code = (Code)(InfoHandlers.Data[i * 2] & (uint)InfoFlags1.CodeMask);
				Assert.Equal((Code)i, code);
			}
		}
	}
}
#endif
