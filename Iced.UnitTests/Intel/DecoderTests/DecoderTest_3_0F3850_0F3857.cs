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
	public sealed class DecoderTest_3_0F3850_0F3857 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_Vp4dpwssdV_VX_k1z_HX_WX_1_Data))]
		void Test16_Vp4dpwssdV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vp4dpwssdV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F4B 52 50 01", 7, Code.EVEX_Vp4dpwssd_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F24FCB 52 50 01", 7, Code.EVEX_Vp4dpwssd_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F24F48 52 50 01", 7, Code.EVEX_Vp4dpwssd_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vp4dpwssdV_VX_k1z_HX_WX_1_Data))]
		void Test32_Vp4dpwssdV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vp4dpwssdV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F4B 52 50 01", 7, Code.EVEX_Vp4dpwssd_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F24FCB 52 50 01", 7, Code.EVEX_Vp4dpwssd_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F24F48 52 50 01", 7, Code.EVEX_Vp4dpwssd_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vp4dpwssdV_VX_k1z_HX_WX_1_Data))]
		void Test64_Vp4dpwssdV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vp4dpwssdV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F4B 52 50 01", 7, Code.EVEX_Vp4dpwssd_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 E20FCB 52 50 01", 7, Code.EVEX_Vp4dpwssd_VZ_k1z_HZP3_M, Register.ZMM18, Register.ZMM14, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 724F40 52 50 01", 7, Code.EVEX_Vp4dpwssd_VZ_k1z_HZP3_M, Register.ZMM10, Register.ZMM22, Register.None, MemorySize.Packed128_Int16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vp4dpwssdsV_VX_k1z_HX_WX_1_Data))]
		void Test16_Vp4dpwssdsV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vp4dpwssdsV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F4B 53 50 01", 7, Code.EVEX_Vp4dpwssds_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F24FCB 53 50 01", 7, Code.EVEX_Vp4dpwssds_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F24F48 53 50 01", 7, Code.EVEX_Vp4dpwssds_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vp4dpwssdsV_VX_k1z_HX_WX_1_Data))]
		void Test32_Vp4dpwssdsV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vp4dpwssdsV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F4B 53 50 01", 7, Code.EVEX_Vp4dpwssds_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F24FCB 53 50 01", 7, Code.EVEX_Vp4dpwssds_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F24F48 53 50 01", 7, Code.EVEX_Vp4dpwssds_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vp4dpwssdsV_VX_k1z_HX_WX_1_Data))]
		void Test64_Vp4dpwssdsV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(OpKind.Memory, instr.Op2Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vp4dpwssdsV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F4B 53 50 01", 7, Code.EVEX_Vp4dpwssds_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 E20FCB 53 50 01", 7, Code.EVEX_Vp4dpwssds_VZ_k1z_HZP3_M, Register.ZMM18, Register.ZMM14, Register.K3, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 724F40 53 50 01", 7, Code.EVEX_Vp4dpwssds_VZ_k1z_HZP3_M, Register.ZMM10, Register.ZMM22, Register.None, MemorySize.Packed128_Int16, 16, false };
			}
		}
	}
}
