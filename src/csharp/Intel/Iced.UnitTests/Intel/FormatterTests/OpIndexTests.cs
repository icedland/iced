// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if GAS || INTEL || MASM || NASM
using System;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	public abstract class OpIndexTests {
		protected void TestBase(Formatter formatter) {
			var instrToFormatter = new int[IcedConstants.MaxOpCount];
			foreach (var tc in DecoderTests.DecoderTestUtils.GetDecoderTests(includeOtherTests: true, includeInvalid: false)) {
				var decoder = Decoder.Create(tc.Bitness, new ByteArrayCodeReader(tc.HexBytes), tc.Options);
				decoder.Decode(out var instruction);
				Assert.Equal(tc.Code, instruction.Code);

				for (int i = 0; i < instrToFormatter.Length; i++)
					instrToFormatter[i] = -1;

				int formatterOpCount = formatter.GetOperandCount(instruction);
				int instrOpCount = instruction.OpCount;

				uint instrOpUsed = 0;
				Assert.True(instrOpCount <= 32);// uint is 32 bits
				for (int formatterOpIndex = 0; formatterOpIndex < formatterOpCount; formatterOpIndex++) {
					int instrOpIndex = formatter.GetInstructionOperand(instruction, formatterOpIndex);
					if (instrOpIndex >= 0) {
						Assert.True(instrOpIndex < instrOpCount);
						instrToFormatter[instrOpIndex] = formatterOpIndex;

#if INSTR_INFO
						Assert.False(formatter.TryGetOpAccess(instruction, formatterOpIndex, out var access));
						Assert.Equal(OpAccess.None, access);
#endif

						uint instrOpBit = 1U << instrOpIndex;
						Assert.True(0U == (instrOpUsed & instrOpBit), "More than one formatter operand index maps to the same instruction op index");
						instrOpUsed |= instrOpBit;

						Assert.Equal(formatterOpIndex, formatter.GetFormatterOperand(instruction, instrOpIndex));
					}
					else {
						Assert.Equal(-1, instrOpIndex);
#if INSTR_INFO
						Assert.True(formatter.TryGetOpAccess(instruction, formatterOpIndex, out var access));
						Assert.True(access >= OpAccess.None && access <= OpAccess.NoMemAccess);
#endif
					}
				}

				for (int instrOpIndex = 0; instrOpIndex < instrOpCount; instrOpIndex++) {
					int formatterOpIndex = formatter.GetFormatterOperand(instruction, instrOpIndex);
					Assert.Equal(instrToFormatter[instrOpIndex], formatterOpIndex);
				}

				for (int instrOpIndex = instrOpCount; instrOpIndex < IcedConstants.MaxOpCount; instrOpIndex++)
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(instruction, instrOpIndex));
			}
		}
	}
}
#endif
