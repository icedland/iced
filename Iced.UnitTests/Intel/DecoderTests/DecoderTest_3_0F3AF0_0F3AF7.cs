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
	public sealed class DecoderTest_3_0F3AF0_0F3AF7 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_AesV_VX_WX_Ib_1_Data))]
		void Test16_AesV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_AesV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E37B F0 10 5A", 6, Code.VEX_Rorx_Gd_Ed_Ib, Register.EDX, MemorySize.UInt32, 0x5A };
				yield return new object[] { "C4E3FB F0 10 5A", 6, Code.VEX_Rorx_Gd_Ed_Ib, Register.EDX, MemorySize.UInt32, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_AesV_VX_WX_Ib_2_Data))]
		void Test16_AesV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_AesV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E37B F0 D3 A5", 6, Code.VEX_Rorx_Gd_Ed_Ib, Register.EDX, Register.EBX, 0xA5 };
				yield return new object[] { "C4E3FB F0 D3 A5", 6, Code.VEX_Rorx_Gd_Ed_Ib, Register.EDX, Register.EBX, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_AesV_VX_WX_Ib_1_Data))]
		void Test32_AesV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_AesV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E37B F0 10 5A", 6, Code.VEX_Rorx_Gd_Ed_Ib, Register.EDX, MemorySize.UInt32, 0x5A };
				yield return new object[] { "C4E3FB F0 10 5A", 6, Code.VEX_Rorx_Gd_Ed_Ib, Register.EDX, MemorySize.UInt32, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_AesV_VX_WX_Ib_2_Data))]
		void Test32_AesV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_AesV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E37B F0 D3 A5", 6, Code.VEX_Rorx_Gd_Ed_Ib, Register.EDX, Register.EBX, 0xA5 };
				yield return new object[] { "C4E3FB F0 D3 A5", 6, Code.VEX_Rorx_Gd_Ed_Ib, Register.EDX, Register.EBX, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_AesV_VX_WX_Ib_1_Data))]
		void Test64_AesV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_AesV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E37B F0 10 5A", 6, Code.VEX_Rorx_Gd_Ed_Ib, Register.EDX, MemorySize.UInt32, 0x5A };

				yield return new object[] { "C4E3FB F0 10 A5", 6, Code.VEX_Rorx_Gq_Eq_Ib, Register.RDX, MemorySize.UInt64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_AesV_VX_WX_Ib_2_Data))]
		void Test64_AesV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_AesV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E37B F0 D3 A5", 6, Code.VEX_Rorx_Gd_Ed_Ib, Register.EDX, Register.EBX, 0xA5 };
				yield return new object[] { "C4637B F0 D3 5A", 6, Code.VEX_Rorx_Gd_Ed_Ib, Register.R10D, Register.EBX, 0x5A };
				yield return new object[] { "C4C37B F0 D3 A5", 6, Code.VEX_Rorx_Gd_Ed_Ib, Register.EDX, Register.R11D, 0xA5 };
				yield return new object[] { "C4437B F0 D3 5A", 6, Code.VEX_Rorx_Gd_Ed_Ib, Register.R10D, Register.R11D, 0x5A };

				yield return new object[] { "C4E3FB F0 D3 A5", 6, Code.VEX_Rorx_Gq_Eq_Ib, Register.RDX, Register.RBX, 0xA5 };
				yield return new object[] { "C463FB F0 D3 5A", 6, Code.VEX_Rorx_Gq_Eq_Ib, Register.R10, Register.RBX, 0x5A };
				yield return new object[] { "C4C3FB F0 D3 A5", 6, Code.VEX_Rorx_Gq_Eq_Ib, Register.RDX, Register.R11, 0xA5 };
				yield return new object[] { "C443FB F0 D3 5A", 6, Code.VEX_Rorx_Gq_Eq_Ib, Register.R10, Register.R11, 0x5A };
			}
		}
	}
}
