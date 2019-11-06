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
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.FormatterTests {
	public abstract class OpIndexTests {
		protected void TestBase(Formatter formatter) {
			var instrToFormatter = new int[IcedConstants.MaxOpCount];
			foreach (var info in DecoderTests.DecoderTestUtils.GetDecoderTests(includeOtherTests: true, includeInvalid: false)) {
				var decoder = Decoder.Create(info.Bitness, new ByteArrayCodeReader(info.HexBytes), info.Options);
				decoder.Decode(out var instr);
				Assert.Equal(info.Code, instr.Code);

				for (int i = 0; i < instrToFormatter.Length; i++)
					instrToFormatter[i] = -1;

				int formatterOpCount = formatter.GetOperandCount(instr);
				int instrOpCount = instr.OpCount;

				uint instrOpUsed = 0;
				Assert.True(instrOpCount <= 32);// uint is 32 bits
				for (int formatterOpIndex = 0; formatterOpIndex < formatterOpCount; formatterOpIndex++) {
					int instrOpIndex = formatter.GetInstructionOperand(instr, formatterOpIndex);
					if (instrOpIndex >= 0) {
						Assert.True(instrOpIndex < instrOpCount);
						instrToFormatter[instrOpIndex] = formatterOpIndex;

#if !NO_INSTR_INFO
						Assert.False(formatter.TryGetOpAccess(instr, formatterOpIndex, out var access));
						Assert.Equal(OpAccess.None, access);
#endif

						uint instrOpBit = 1U << instrOpIndex;
						Assert.True(0U == (instrOpUsed & instrOpBit), "More than one formatter operand index maps to the same instruction op index");
						instrOpUsed |= instrOpBit;

						Assert.Equal(formatterOpIndex, formatter.GetFormatterOperand(instr, instrOpIndex));
					}
					else {
						Assert.Equal(-1, instrOpIndex);
#if !NO_INSTR_INFO
						Assert.True(formatter.TryGetOpAccess(instr, formatterOpIndex, out var access));
						Assert.True(access >= OpAccess.None && access <= OpAccess.NoMemAccess);
#endif
					}
				}

				for (int instrOpIndex = 0; instrOpIndex < instrOpCount; instrOpIndex++) {
					int formatterOpIndex = formatter.GetFormatterOperand(instr, instrOpIndex);
					Assert.Equal(instrToFormatter[instrOpIndex], formatterOpIndex);
				}

				for (int instrOpIndex = instrOpCount; instrOpIndex < IcedConstants.MaxOpCount; instrOpIndex++)
					Assert.Throws<ArgumentOutOfRangeException>(() => formatter.GetFormatterOperand(instr, instrOpIndex));
			}
		}
	}
}
#endif
