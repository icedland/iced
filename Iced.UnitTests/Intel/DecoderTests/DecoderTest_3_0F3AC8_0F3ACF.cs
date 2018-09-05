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

using System.Collections.Generic;
using Iced.Intel;
using Xunit;

namespace Iced.UnitTests.Intel.DecoderTests {
	public sealed class DecoderTest_3_0F3AC8_0F3ACF : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_Sha1rnds4V_VX_WX_Ib_1_Data))]
		void Test16_Sha1rnds4V_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Sha1rnds4V_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "0F3ACC 08 A5", 5, Code.Sha1rnds4_VX_WX_Ib, Register.XMM1, MemorySize.Packed128_UInt32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Sha1rnds4V_VX_WX_Ib_2_Data))]
		void Test16_Sha1rnds4V_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Sha1rnds4V_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "0F3ACC CD A5", 5, Code.Sha1rnds4_VX_WX_Ib, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Sha1rnds4V_VX_WX_Ib_1_Data))]
		void Test32_Sha1rnds4V_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Sha1rnds4V_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "0F3ACC 08 A5", 5, Code.Sha1rnds4_VX_WX_Ib, Register.XMM1, MemorySize.Packed128_UInt32, 0xA5 };

			}
		}

		[Theory]
		[MemberData(nameof(Test32_Sha1rnds4V_VX_WX_Ib_2_Data))]
		void Test32_Sha1rnds4V_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Sha1rnds4V_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "0F3ACC CD A5", 5, Code.Sha1rnds4_VX_WX_Ib, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Sha1rnds4V_VX_WX_Ib_1_Data))]
		void Test64_Sha1rnds4V_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Sha1rnds4V_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "0F3ACC 08 A5", 5, Code.Sha1rnds4_VX_WX_Ib, Register.XMM1, MemorySize.Packed128_UInt32, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Sha1rnds4V_VX_WX_Ib_2_Data))]
		void Test64_Sha1rnds4V_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Sha1rnds4V_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "0F3ACC CD A5", 5, Code.Sha1rnds4_VX_WX_Ib, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "44 0F3ACC CD A5", 6, Code.Sha1rnds4_VX_WX_Ib, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "41 0F3ACC CD A5", 6, Code.Sha1rnds4_VX_WX_Ib, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "45 0F3ACC CD A5", 6, Code.Sha1rnds4_VX_WX_Ib, Register.XMM9, Register.XMM13, 0xA5 };
			}
		}
	}
}
