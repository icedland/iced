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

using System;
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
			Assert.True(Instruction.TEST_BitByBitEquals(ref instr1, ref instr2));
		}
	}
}
