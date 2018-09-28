/*
    Copyright (C) 2018 de4dot@gmail.com

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

#if (!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER
using System;
using System.Linq;
using System.Text;
using Iced.Intel;
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

			var types = GetType().Assembly.GetTypes().Where(a =>
					a.Namespace == GetType().Namespace &&
					(a.Name.StartsWith("InstructionInfos16_") || a.Name.StartsWith("InstructionInfos32_") || a.Name.StartsWith("InstructionInfos64_"))).ToArray();
			foreach (var type in types) {
				var fieldInfo = type.GetField("AllInfos");
				Assert.NotNull(fieldInfo);
				var infos = fieldInfo.GetValue(null) as InstructionInfo[];
				Assert.NotNull(infos);
				foreach (var info in infos)
					tested[(int)info.Code] = 1;
			}

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
