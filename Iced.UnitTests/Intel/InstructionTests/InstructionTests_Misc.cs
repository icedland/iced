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

using System;
using System.Runtime.CompilerServices;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionTests {
	public sealed class InstructionTests_Misc {
		static int GetEnumSize(Type enumType) {
			Assert.True(enumType.IsEnum);
			int maxValue = -1;
			foreach (var f in enumType.GetFields()) {
				if (f.IsLiteral) {
					int value = (int)f.GetValue(null);
					Assert.Equal(maxValue + 1, value);
					maxValue = value;
				}
			}
			return maxValue + 1;
		}

		[Fact]
		void OpKind_is_not_too_big() {
			int maxValue = GetEnumSize(typeof(OpKind)) - 1;
			Assert.True(maxValue < (1 << Instruction.TEST_OpKindBits));
			Assert.True(maxValue >= (1 << (Instruction.TEST_OpKindBits - 1)));
		}

		[Fact]
		void Code_is_not_too_big() {
			int maxValue = GetEnumSize(typeof(Code)) - 1;
			Assert.True(maxValue < (1 << Instruction.TEST_CodeBits));
			Assert.True(maxValue >= (1 << (Instruction.TEST_CodeBits - 1)));
		}

		[Fact]
		void Register_is_not_too_big() {
			int maxValue = GetEnumSize(typeof(Register)) - 1;
			Assert.True(maxValue < (1 << Instruction.TEST_RegisterBits));
			Assert.True(maxValue >= (1 << (Instruction.TEST_RegisterBits - 1)));
		}

		[Fact]
		void OpKind_Register_is_zero() {
			// The opcode handlers assume it's zero. They have Debug.Assert()s too.
			Assert.True(OpKind.Register == 0);
		}

		[Fact]
		void INVALID_Code_value_is_zero() {
			// A 'default' Instruction should be an invalid instruction
			Assert.True((int)Code.INVALID == 0);
			Instruction instr1 = default;
			Assert.Equal(Code.INVALID, instr1.Code);
			var instr2 = new Instruction();
			Assert.Equal(Code.INVALID, instr2.Code);
			Assert.True(Instruction.TEST_BitByBitEquals(instr1, instr2));
		}

#if !NO_ENCODER
		[Fact]
		void Equals_and_GetHashCode_ignore_some_fields() {
			var instr1 = Instruction.Create(Code.VEX_Vpermil2ps_xmm_xmm_xmmm128_xmm_imm8, Register.XMM1, Register.XMM2, new MemoryOperand(Register.RCX, Register.R14, 8, 0x12345678, 8, false, Register.FS), Register.XMM10, 0xA5);
			var instr2 = instr1;
			Assert.True(Instruction.EqualsAllBits(instr1, instr2));
			instr1.CodeSize = CodeSize.Code32;
			instr2.CodeSize = CodeSize.Code64;
			Assert.False(Instruction.EqualsAllBits(instr1, instr2));
			instr1.ByteLength = 10;
			instr2.ByteLength = 5;
			Assert.True(instr1.Equals(instr2));
			Assert.True(instr1.Equals(ToObject(instr2)));
			Assert.Equal(instr1, instr2);
			Assert.Equal(instr1.GetHashCode(), instr2.GetHashCode());
		}
		[MethodImpl(MethodImplOptions.NoInlining)]
		static object ToObject<T>(T value) => value;
#endif
	}
}
