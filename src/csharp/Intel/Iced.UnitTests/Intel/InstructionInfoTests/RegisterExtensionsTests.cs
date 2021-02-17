// SPDX-License-Identifier: MIT
// Copyright (C) 2018-present iced project and contributors

#if INSTR_INFO
using System;
using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.InstructionInfoTests {
	public sealed class RegisterExtensionsTests {
		[Theory]
		[InlineData((Register)(-1))]
		[InlineData((Register)IcedConstants.RegisterEnumCount)]
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

			const RegisterFlags allFlags =
				RegisterFlags.SegmentRegister |
				RegisterFlags.GPR |
				RegisterFlags.GPR8 |
				RegisterFlags.GPR16 |
				RegisterFlags.GPR32 |
				RegisterFlags.GPR64 |
				RegisterFlags.XMM |
				RegisterFlags.YMM |
				RegisterFlags.ZMM |
				RegisterFlags.VectorRegister |
				RegisterFlags.IP |
				RegisterFlags.K |
				RegisterFlags.BND |
				RegisterFlags.CR |
				RegisterFlags.DR |
				RegisterFlags.TR |
				RegisterFlags.ST |
				RegisterFlags.MM |
				RegisterFlags.TMM;
			// If it fails, update the flags above and the code below, eg. add a IsTMM() test
			Assert.Equal(flags, flags & allFlags);

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
			Assert.Equal((flags & RegisterFlags.IP) != 0, register.IsIP());
			Assert.Equal((flags & RegisterFlags.K) != 0, register.IsK());
			Assert.Equal((flags & RegisterFlags.BND) != 0, register.IsBND());
			Assert.Equal((flags & RegisterFlags.CR) != 0, register.IsCR());
			Assert.Equal((flags & RegisterFlags.DR) != 0, register.IsDR());
			Assert.Equal((flags & RegisterFlags.TR) != 0, register.IsTR());
			Assert.Equal((flags & RegisterFlags.ST) != 0, register.IsST());
			Assert.Equal((flags & RegisterFlags.MM) != 0, register.IsMM());
			Assert.Equal((flags & RegisterFlags.TMM) != 0, register.IsTMM());
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
