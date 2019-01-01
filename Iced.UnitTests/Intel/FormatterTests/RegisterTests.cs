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

#if (!NO_GAS_FORMATTER || !NO_INTEL_FORMATTER || !NO_MASM_FORMATTER || !NO_NASM_FORMATTER) && !NO_FORMATTER
using System;
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	public abstract class RegisterTests {
		protected static IEnumerable<object[]> GetFormatData(string[] formattedRegisters) {
			if (Iced.Intel.DecoderConstants.NumberOfRegisters != formattedRegisters.Length)
				throw new ArgumentException($"({nameof(Iced.Intel.DecoderConstants.NumberOfRegisters)}) {Iced.Intel.DecoderConstants.NumberOfRegisters} != (formattedRegisters.Length) {formattedRegisters.Length}");
			var res = new object[formattedRegisters.Length][];
			for (int i = 0; i < res.Length; i++)
				res[i] = new object[2] { (Register)i, formattedRegisters[i] };
			return res;
		}

		protected void FormatBase(Register register, string formattedString, Formatter formatter) {
			var actualFormattedString = formatter.Format(register);
			Assert.Equal(formattedString, actualFormattedString);
		}
	}
}
#endif
