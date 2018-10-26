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
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	public abstract class OpIndexTests {
		protected void TestBase(Formatter formatter) {
			var instrToFormatter = new int[Iced.Intel.DecoderConstants.MaxOpCount];
			foreach (var info in DecoderTests.DecoderTestUtils.GetDecoderTests(needHexBytes: true, includeOtherTests: true)) {
				var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(info.HexBytes), info.Options);
				decoder.Decode(out var instr);
				Assert.Equal(info.Code, instr.Code);

				for (int i = 0; i < instrToFormatter.Length; i++)
					instrToFormatter[i] = -1;

				int formatterOpCount = formatter.GetOperandCount(ref instr);
				int instrOpCount = instr.OpCount;

				uint instrOpUsed = 0;
				Assert.True(instrOpCount <= 32);// uint is 32 bits
				for (int formatterOpIndex = 0; formatterOpIndex < formatterOpCount; formatterOpIndex++) {
					int instrOpIndex = formatter.GetInstructionOperand(ref instr, formatterOpIndex);
					if (instrOpIndex >= 0) {
						Assert.True(instrOpIndex < instrOpCount);
						instrToFormatter[instrOpIndex] = formatterOpIndex;

#if !NO_INSTR_INFO
						Assert.False(formatter.TryGetOpAccess(ref instr, formatterOpIndex, out var access));
						Assert.Equal(OpAccess.None, access);
#endif

						uint instrOpBit = 1U << instrOpIndex;
						Assert.True(0U == (instrOpUsed & instrOpBit), "More than one formatter operand index maps to the same instruction op index");
						instrOpUsed |= instrOpBit;

						Assert.Equal(formatterOpIndex, formatter.GetFormatterOperand(ref instr, instrOpIndex));
					}
					else {
						Assert.Equal(-1, instrOpIndex);
#if !NO_INSTR_INFO
						Assert.True(formatter.TryGetOpAccess(ref instr, formatterOpIndex, out var access));
						Assert.True(access >= OpAccess.None && access <= OpAccess.NoMemAccess);
#endif
					}
				}

				for (int instrOpIndex = 0; instrOpIndex < instrOpCount; instrOpIndex++) {
					int formatterOpIndex = formatter.GetFormatterOperand(ref instr, instrOpIndex);
					Assert.Equal(instrToFormatter[instrOpIndex], formatterOpIndex);
				}

				for (int instrOpIndex = instrOpCount; instrOpIndex < Iced.Intel.DecoderConstants.MaxOpCount; instrOpIndex++) {
					int formatterOpIndex = formatter.GetFormatterOperand(ref instr, instrOpIndex);
					Assert.Equal(-1, formatterOpIndex);
				}
			}
		}
	}
}
#endif
