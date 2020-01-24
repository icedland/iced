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

#if !NO_INSTR_INFO
using System;
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	public sealed class RegisterExtensionsTests {
		[Theory]
		[InlineData((Register)(-1))]
		[InlineData((Register)IcedConstants.NumberOfRegisters)]
		void GetInfo_throws_if_invalid_value(Register register) {
			Assert.Throws<ArgumentOutOfRangeException>(() => register.GetInfo());
			Assert.Throws<ArgumentOutOfRangeException>(() => register.GetBaseRegister());
			Assert.Throws<ArgumentOutOfRangeException>(() => register.GetNumber());
			Assert.Throws<ArgumentOutOfRangeException>(() => register.GetFullRegister());
			Assert.Throws<ArgumentOutOfRangeException>(() => register.GetFullRegister32());
			Assert.Throws<ArgumentOutOfRangeException>(() => register.GetSize());
		}

		[Theory]
		[MemberData(nameof(VerifyRegisterProperties_Data))]
		void VerifyRegisterProperties(Register register, int number, Register baseRegister, Register fullRegister, Register fullRegister32, int size, RegisterFlags flags) {
			var info = register.GetInfo();
			Assert.Equal(register, info.Register);
			Assert.Equal(baseRegister, info.Base);
			Assert.Equal(number, info.Number);
			Assert.Equal(fullRegister, info.FullRegister);
			Assert.Equal(fullRegister32, info.FullRegister32);
			Assert.Equal(size, info.Size);

			Assert.Equal(baseRegister, register.GetBaseRegister());
			Assert.Equal(number, register.GetNumber());
			Assert.Equal(fullRegister, register.GetFullRegister());
			Assert.Equal(fullRegister32, register.GetFullRegister32());
			Assert.Equal(size, register.GetSize());

			Assert.Equal((flags & RegisterFlags.SegmentRegister) != 0, register.IsSegmentRegister());
			Assert.Equal((flags & RegisterFlags.GPR) != 0, register.IsGPR());
			Assert.Equal((flags & RegisterFlags.GPR8) != 0, register.IsGPR8());
			Assert.Equal((flags & RegisterFlags.GPR16) != 0, register.IsGPR16());
			Assert.Equal((flags & RegisterFlags.GPR32) != 0, register.IsGPR32());
			Assert.Equal((flags & RegisterFlags.GPR64) != 0, register.IsGPR64());
			Assert.Equal((flags & RegisterFlags.XMM) != 0, register.IsXMM());
			Assert.Equal((flags & RegisterFlags.YMM) != 0, register.IsYMM());
			Assert.Equal((flags & RegisterFlags.ZMM) != 0, register.IsZMM());
			Assert.Equal((flags & RegisterFlags.VectorRegister) != 0, register.IsVectorRegister());
		}
		public static IEnumerable<object[]> VerifyRegisterProperties_Data {
			get {
				foreach (var tc in RegisterInfoTestReader.GetTestCases())
					yield return new object[] { tc.Register, tc.Number, tc.BaseRegister, tc.FullRegister, tc.FullRegister32, tc.Size, tc.Flags };
			}
		}
	}
}
#endif
