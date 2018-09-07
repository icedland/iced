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
	public sealed class DecoderTest_3_0F3848_0F384F : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_Vrcp14psV_VX_k1_WX_1_Data))]
		void Test16_Vrcp14psV_VX_k1_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vrcp14psV_VX_k1_WX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 4C 50 01", 7, Code.EVEX_Vrcp14ps_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F27D9D 4C 50 01", 7, Code.EVEX_Vrcp14ps_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F27D08 4C 50 01", 7, Code.EVEX_Vrcp14ps_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F27D2B 4C 50 01", 7, Code.EVEX_Vrcp14ps_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F27DBD 4C 50 01", 7, Code.EVEX_Vrcp14ps_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F27D28 4C 50 01", 7, Code.EVEX_Vrcp14ps_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F27D4B 4C 50 01", 7, Code.EVEX_Vrcp14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F27DDD 4C 50 01", 7, Code.EVEX_Vrcp14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F27D48 4C 50 01", 7, Code.EVEX_Vrcp14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2FD0B 4C 50 01", 7, Code.EVEX_Vrcp14pd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2FD9D 4C 50 01", 7, Code.EVEX_Vrcp14pd_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2FD08 4C 50 01", 7, Code.EVEX_Vrcp14pd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2FD2B 4C 50 01", 7, Code.EVEX_Vrcp14pd_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2FDBD 4C 50 01", 7, Code.EVEX_Vrcp14pd_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2FD28 4C 50 01", 7, Code.EVEX_Vrcp14pd_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2FD4B 4C 50 01", 7, Code.EVEX_Vrcp14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2FDDD 4C 50 01", 7, Code.EVEX_Vrcp14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2FD48 4C 50 01", 7, Code.EVEX_Vrcp14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vrcp14psV_VX_k1_WX_2_Data))]
		void Test16_Vrcp14psV_VX_k1_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vrcp14psV_VX_k1_WX_2_Data {
			get {
				yield return new object[] { "62 F27D8B 4C D3", 6, Code.EVEX_Vrcp14ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F27D0B 4C D3", 6, Code.EVEX_Vrcp14ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F27DAB 4C D3", 6, Code.EVEX_Vrcp14ps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F27D2B 4C D3", 6, Code.EVEX_Vrcp14ps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F27DCB 4C D3", 6, Code.EVEX_Vrcp14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F27D4B 4C D3", 6, Code.EVEX_Vrcp14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2FD8B 4C D3", 6, Code.EVEX_Vrcp14pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2FD0B 4C D3", 6, Code.EVEX_Vrcp14pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2FDAB 4C D3", 6, Code.EVEX_Vrcp14pd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2FD2B 4C D3", 6, Code.EVEX_Vrcp14pd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2FDCB 4C D3", 6, Code.EVEX_Vrcp14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2FD4B 4C D3", 6, Code.EVEX_Vrcp14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vrcp14psV_VX_k1_WX_1_Data))]
		void Test32_Vrcp14psV_VX_k1_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vrcp14psV_VX_k1_WX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 4C 50 01", 7, Code.EVEX_Vrcp14ps_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F27D9D 4C 50 01", 7, Code.EVEX_Vrcp14ps_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F27D08 4C 50 01", 7, Code.EVEX_Vrcp14ps_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F27D2B 4C 50 01", 7, Code.EVEX_Vrcp14ps_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F27DBD 4C 50 01", 7, Code.EVEX_Vrcp14ps_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F27D28 4C 50 01", 7, Code.EVEX_Vrcp14ps_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F27D4B 4C 50 01", 7, Code.EVEX_Vrcp14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F27DDD 4C 50 01", 7, Code.EVEX_Vrcp14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F27D48 4C 50 01", 7, Code.EVEX_Vrcp14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2FD0B 4C 50 01", 7, Code.EVEX_Vrcp14pd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2FD9D 4C 50 01", 7, Code.EVEX_Vrcp14pd_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2FD08 4C 50 01", 7, Code.EVEX_Vrcp14pd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2FD2B 4C 50 01", 7, Code.EVEX_Vrcp14pd_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2FDBD 4C 50 01", 7, Code.EVEX_Vrcp14pd_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2FD28 4C 50 01", 7, Code.EVEX_Vrcp14pd_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2FD4B 4C 50 01", 7, Code.EVEX_Vrcp14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2FDDD 4C 50 01", 7, Code.EVEX_Vrcp14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2FD48 4C 50 01", 7, Code.EVEX_Vrcp14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vrcp14psV_VX_k1_WX_2_Data))]
		void Test32_Vrcp14psV_VX_k1_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vrcp14psV_VX_k1_WX_2_Data {
			get {
				yield return new object[] { "62 F27D8B 4C D3", 6, Code.EVEX_Vrcp14ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F27D0B 4C D3", 6, Code.EVEX_Vrcp14ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F27DAB 4C D3", 6, Code.EVEX_Vrcp14ps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F27D2B 4C D3", 6, Code.EVEX_Vrcp14ps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F27DCB 4C D3", 6, Code.EVEX_Vrcp14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F27D4B 4C D3", 6, Code.EVEX_Vrcp14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2FD8B 4C D3", 6, Code.EVEX_Vrcp14pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2FD0B 4C D3", 6, Code.EVEX_Vrcp14pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2FDAB 4C D3", 6, Code.EVEX_Vrcp14pd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2FD2B 4C D3", 6, Code.EVEX_Vrcp14pd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2FDCB 4C D3", 6, Code.EVEX_Vrcp14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2FD4B 4C D3", 6, Code.EVEX_Vrcp14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vrcp14psV_VX_k1_WX_1_Data))]
		void Test64_Vrcp14psV_VX_k1_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vrcp14psV_VX_k1_WX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 4C 50 01", 7, Code.EVEX_Vrcp14ps_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F27D9D 4C 50 01", 7, Code.EVEX_Vrcp14ps_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F27D08 4C 50 01", 7, Code.EVEX_Vrcp14ps_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F27D2B 4C 50 01", 7, Code.EVEX_Vrcp14ps_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F27DBD 4C 50 01", 7, Code.EVEX_Vrcp14ps_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F27D28 4C 50 01", 7, Code.EVEX_Vrcp14ps_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F27D4B 4C 50 01", 7, Code.EVEX_Vrcp14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F27DDD 4C 50 01", 7, Code.EVEX_Vrcp14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F27D48 4C 50 01", 7, Code.EVEX_Vrcp14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2FD0B 4C 50 01", 7, Code.EVEX_Vrcp14pd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2FD9D 4C 50 01", 7, Code.EVEX_Vrcp14pd_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2FD08 4C 50 01", 7, Code.EVEX_Vrcp14pd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2FD2B 4C 50 01", 7, Code.EVEX_Vrcp14pd_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2FDBD 4C 50 01", 7, Code.EVEX_Vrcp14pd_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2FD28 4C 50 01", 7, Code.EVEX_Vrcp14pd_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2FD4B 4C 50 01", 7, Code.EVEX_Vrcp14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2FDDD 4C 50 01", 7, Code.EVEX_Vrcp14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2FD48 4C 50 01", 7, Code.EVEX_Vrcp14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vrcp14psV_VX_k1_WX_2_Data))]
		void Test64_Vrcp14psV_VX_k1_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vrcp14psV_VX_k1_WX_2_Data {
			get {
				yield return new object[] { "62 F27D8B 4C D3", 6, Code.EVEX_Vrcp14ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E27D0B 4C D3", 6, Code.EVEX_Vrcp14ps_VX_k1z_WX_b, Register.XMM18, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 127D0B 4C D3", 6, Code.EVEX_Vrcp14ps_VX_k1z_WX_b, Register.XMM10, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B27D0B 4C D3", 6, Code.EVEX_Vrcp14ps_VX_k1z_WX_b, Register.XMM2, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F27D0B 4C D3", 6, Code.EVEX_Vrcp14ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F27DAB 4C D3", 6, Code.EVEX_Vrcp14ps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E27D2B 4C D3", 6, Code.EVEX_Vrcp14ps_VY_k1z_WY_b, Register.YMM18, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 127D2B 4C D3", 6, Code.EVEX_Vrcp14ps_VY_k1z_WY_b, Register.YMM10, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B27D2B 4C D3", 6, Code.EVEX_Vrcp14ps_VY_k1z_WY_b, Register.YMM2, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F27D2B 4C D3", 6, Code.EVEX_Vrcp14ps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F27DCB 4C D3", 6, Code.EVEX_Vrcp14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E27D4B 4C D3", 6, Code.EVEX_Vrcp14ps_VZ_k1z_WZ_b, Register.ZMM18, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 127D4B 4C D3", 6, Code.EVEX_Vrcp14ps_VZ_k1z_WZ_b, Register.ZMM10, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B27D4B 4C D3", 6, Code.EVEX_Vrcp14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F27D4B 4C D3", 6, Code.EVEX_Vrcp14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2FD8B 4C D3", 6, Code.EVEX_Vrcp14pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E2FD0B 4C D3", 6, Code.EVEX_Vrcp14pd_VX_k1z_WX_b, Register.XMM18, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12FD0B 4C D3", 6, Code.EVEX_Vrcp14pd_VX_k1z_WX_b, Register.XMM10, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2FD0B 4C D3", 6, Code.EVEX_Vrcp14pd_VX_k1z_WX_b, Register.XMM2, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2FD0B 4C D3", 6, Code.EVEX_Vrcp14pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2FDAB 4C D3", 6, Code.EVEX_Vrcp14pd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E2FD2B 4C D3", 6, Code.EVEX_Vrcp14pd_VY_k1z_WY_b, Register.YMM18, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12FD2B 4C D3", 6, Code.EVEX_Vrcp14pd_VY_k1z_WY_b, Register.YMM10, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2FD2B 4C D3", 6, Code.EVEX_Vrcp14pd_VY_k1z_WY_b, Register.YMM2, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2FD2B 4C D3", 6, Code.EVEX_Vrcp14pd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2FDCB 4C D3", 6, Code.EVEX_Vrcp14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E2FD4B 4C D3", 6, Code.EVEX_Vrcp14pd_VZ_k1z_WZ_b, Register.ZMM18, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12FD4B 4C D3", 6, Code.EVEX_Vrcp14pd_VZ_k1z_WZ_b, Register.ZMM10, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2FD4B 4C D3", 6, Code.EVEX_Vrcp14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2FD4B 4C D3", 6, Code.EVEX_Vrcp14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vrcp14ssV_VX_k1_HX_WX_1_Data))]
		void Test16_Vrcp14ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vrcp14ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 4D 50 01", 7, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D8B 4D 50 01", 7, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24D08 4D 50 01", 7, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D28 4D 50 01", 7, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D48 4D 50 01", 7, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D68 4D 50 01", 7, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F2CD0B 4D 50 01", 7, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD8B 4D 50 01", 7, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CD08 4D 50 01", 7, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD28 4D 50 01", 7, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD48 4D 50 01", 7, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD68 4D 50 01", 7, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vrcp14ssV_VX_k1_HX_WX_2_Data))]
		void Test16_Vrcp14ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vrcp14ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 4D D3", 6, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 4D D3", 6, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD0B 4D D3", 6, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD8B 4D D3", 6, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vrcp14ssV_VX_k1_HX_WX_1_Data))]
		void Test32_Vrcp14ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vrcp14ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 4D 50 01", 7, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D8B 4D 50 01", 7, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24D08 4D 50 01", 7, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D28 4D 50 01", 7, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D48 4D 50 01", 7, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D68 4D 50 01", 7, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F2CD0B 4D 50 01", 7, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD8B 4D 50 01", 7, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CD08 4D 50 01", 7, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD28 4D 50 01", 7, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD48 4D 50 01", 7, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD68 4D 50 01", 7, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vrcp14ssV_VX_k1_HX_WX_2_Data))]
		void Test32_Vrcp14ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vrcp14ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 4D D3", 6, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 4D D3", 6, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD0B 4D D3", 6, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD8B 4D D3", 6, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vrcp14ssV_VX_k1_HX_WX_1_Data))]
		void Test64_Vrcp14ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vrcp14ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 4D 50 01", 7, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D8B 4D 50 01", 7, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24D08 4D 50 01", 7, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D28 4D 50 01", 7, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D48 4D 50 01", 7, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D68 4D 50 01", 7, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F2CD0B 4D 50 01", 7, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD8B 4D 50 01", 7, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CD08 4D 50 01", 7, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD28 4D 50 01", 7, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD48 4D 50 01", 7, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD68 4D 50 01", 7, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vrcp14ssV_VX_k1_HX_WX_2_Data))]
		void Test64_Vrcp14ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vrcp14ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 4D D3", 6, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D8B 4D D3", 6, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D03 4D D3", 6, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D0B 4D D3", 6, Code.EVEX_Vrcp14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD0B 4D D3", 6, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E28D8B 4D D3", 6, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 12CD03 4D D3", 6, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD0B 4D D3", 6, Code.EVEX_Vrcp14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vrsqrt14psV_VX_k1_WX_1_Data))]
		void Test16_Vrsqrt14psV_VX_k1_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vrsqrt14psV_VX_k1_WX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F27D9D 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F27D08 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F27D2B 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F27DBD 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F27D28 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F27D4B 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F27DDD 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F27D48 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2FD0B 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2FD9D 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2FD08 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2FD2B 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2FDBD 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2FD28 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2FD4B 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2FDDD 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2FD48 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vrsqrt14psV_VX_k1_WX_2_Data))]
		void Test16_Vrsqrt14psV_VX_k1_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vrsqrt14psV_VX_k1_WX_2_Data {
			get {
				yield return new object[] { "62 F27D8B 4E D3", 6, Code.EVEX_Vrsqrt14ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F27D0B 4E D3", 6, Code.EVEX_Vrsqrt14ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F27DAB 4E D3", 6, Code.EVEX_Vrsqrt14ps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F27D2B 4E D3", 6, Code.EVEX_Vrsqrt14ps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F27DCB 4E D3", 6, Code.EVEX_Vrsqrt14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F27D4B 4E D3", 6, Code.EVEX_Vrsqrt14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2FD8B 4E D3", 6, Code.EVEX_Vrsqrt14pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2FD0B 4E D3", 6, Code.EVEX_Vrsqrt14pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2FDAB 4E D3", 6, Code.EVEX_Vrsqrt14pd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2FD2B 4E D3", 6, Code.EVEX_Vrsqrt14pd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2FDCB 4E D3", 6, Code.EVEX_Vrsqrt14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2FD4B 4E D3", 6, Code.EVEX_Vrsqrt14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vrsqrt14psV_VX_k1_WX_1_Data))]
		void Test32_Vrsqrt14psV_VX_k1_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vrsqrt14psV_VX_k1_WX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F27D9D 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F27D08 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F27D2B 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F27DBD 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F27D28 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F27D4B 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F27DDD 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F27D48 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2FD0B 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2FD9D 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2FD08 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2FD2B 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2FDBD 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2FD28 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2FD4B 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2FDDD 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2FD48 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vrsqrt14psV_VX_k1_WX_2_Data))]
		void Test32_Vrsqrt14psV_VX_k1_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vrsqrt14psV_VX_k1_WX_2_Data {
			get {
				yield return new object[] { "62 F27D8B 4E D3", 6, Code.EVEX_Vrsqrt14ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F27D0B 4E D3", 6, Code.EVEX_Vrsqrt14ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F27DAB 4E D3", 6, Code.EVEX_Vrsqrt14ps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F27D2B 4E D3", 6, Code.EVEX_Vrsqrt14ps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F27DCB 4E D3", 6, Code.EVEX_Vrsqrt14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F27D4B 4E D3", 6, Code.EVEX_Vrsqrt14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2FD8B 4E D3", 6, Code.EVEX_Vrsqrt14pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2FD0B 4E D3", 6, Code.EVEX_Vrsqrt14pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2FDAB 4E D3", 6, Code.EVEX_Vrsqrt14pd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2FD2B 4E D3", 6, Code.EVEX_Vrsqrt14pd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2FDCB 4E D3", 6, Code.EVEX_Vrsqrt14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2FD4B 4E D3", 6, Code.EVEX_Vrsqrt14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vrsqrt14psV_VX_k1_WX_1_Data))]
		void Test64_Vrsqrt14psV_VX_k1_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vrsqrt14psV_VX_k1_WX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F27D9D 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F27D08 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F27D2B 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F27DBD 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F27D28 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F27D4B 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F27DDD 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F27D48 4E 50 01", 7, Code.EVEX_Vrsqrt14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2FD0B 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2FD9D 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2FD08 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2FD2B 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2FDBD 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2FD28 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2FD4B 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2FDDD 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2FD48 4E 50 01", 7, Code.EVEX_Vrsqrt14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vrsqrt14psV_VX_k1_WX_2_Data))]
		void Test64_Vrsqrt14psV_VX_k1_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vrsqrt14psV_VX_k1_WX_2_Data {
			get {
				yield return new object[] { "62 F27D8B 4E D3", 6, Code.EVEX_Vrsqrt14ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E27D0B 4E D3", 6, Code.EVEX_Vrsqrt14ps_VX_k1z_WX_b, Register.XMM18, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 127D0B 4E D3", 6, Code.EVEX_Vrsqrt14ps_VX_k1z_WX_b, Register.XMM10, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B27D0B 4E D3", 6, Code.EVEX_Vrsqrt14ps_VX_k1z_WX_b, Register.XMM2, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F27D0B 4E D3", 6, Code.EVEX_Vrsqrt14ps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F27DAB 4E D3", 6, Code.EVEX_Vrsqrt14ps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E27D2B 4E D3", 6, Code.EVEX_Vrsqrt14ps_VY_k1z_WY_b, Register.YMM18, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 127D2B 4E D3", 6, Code.EVEX_Vrsqrt14ps_VY_k1z_WY_b, Register.YMM10, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B27D2B 4E D3", 6, Code.EVEX_Vrsqrt14ps_VY_k1z_WY_b, Register.YMM2, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F27D2B 4E D3", 6, Code.EVEX_Vrsqrt14ps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F27DCB 4E D3", 6, Code.EVEX_Vrsqrt14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E27D4B 4E D3", 6, Code.EVEX_Vrsqrt14ps_VZ_k1z_WZ_b, Register.ZMM18, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 127D4B 4E D3", 6, Code.EVEX_Vrsqrt14ps_VZ_k1z_WZ_b, Register.ZMM10, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B27D4B 4E D3", 6, Code.EVEX_Vrsqrt14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F27D4B 4E D3", 6, Code.EVEX_Vrsqrt14ps_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2FD8B 4E D3", 6, Code.EVEX_Vrsqrt14pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E2FD0B 4E D3", 6, Code.EVEX_Vrsqrt14pd_VX_k1z_WX_b, Register.XMM18, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12FD0B 4E D3", 6, Code.EVEX_Vrsqrt14pd_VX_k1z_WX_b, Register.XMM10, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2FD0B 4E D3", 6, Code.EVEX_Vrsqrt14pd_VX_k1z_WX_b, Register.XMM2, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2FD0B 4E D3", 6, Code.EVEX_Vrsqrt14pd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2FDAB 4E D3", 6, Code.EVEX_Vrsqrt14pd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E2FD2B 4E D3", 6, Code.EVEX_Vrsqrt14pd_VY_k1z_WY_b, Register.YMM18, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12FD2B 4E D3", 6, Code.EVEX_Vrsqrt14pd_VY_k1z_WY_b, Register.YMM10, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2FD2B 4E D3", 6, Code.EVEX_Vrsqrt14pd_VY_k1z_WY_b, Register.YMM2, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2FD2B 4E D3", 6, Code.EVEX_Vrsqrt14pd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2FDCB 4E D3", 6, Code.EVEX_Vrsqrt14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E2FD4B 4E D3", 6, Code.EVEX_Vrsqrt14pd_VZ_k1z_WZ_b, Register.ZMM18, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12FD4B 4E D3", 6, Code.EVEX_Vrsqrt14pd_VZ_k1z_WZ_b, Register.ZMM10, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2FD4B 4E D3", 6, Code.EVEX_Vrsqrt14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2FD4B 4E D3", 6, Code.EVEX_Vrsqrt14pd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vrsqrt14ssV_VX_k1_HX_WX_1_Data))]
		void Test16_Vrsqrt14ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vrsqrt14ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 4F 50 01", 7, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D8B 4F 50 01", 7, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24D08 4F 50 01", 7, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D28 4F 50 01", 7, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D48 4F 50 01", 7, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D68 4F 50 01", 7, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F2CD0B 4F 50 01", 7, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD8B 4F 50 01", 7, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CD08 4F 50 01", 7, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD28 4F 50 01", 7, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD48 4F 50 01", 7, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD68 4F 50 01", 7, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vrsqrt14ssV_VX_k1_HX_WX_2_Data))]
		void Test16_Vrsqrt14ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vrsqrt14ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 4F D3", 6, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 4F D3", 6, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD0B 4F D3", 6, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD8B 4F D3", 6, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vrsqrt14ssV_VX_k1_HX_WX_1_Data))]
		void Test32_Vrsqrt14ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vrsqrt14ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 4F 50 01", 7, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D8B 4F 50 01", 7, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24D08 4F 50 01", 7, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D28 4F 50 01", 7, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D48 4F 50 01", 7, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D68 4F 50 01", 7, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F2CD0B 4F 50 01", 7, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD8B 4F 50 01", 7, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CD08 4F 50 01", 7, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD28 4F 50 01", 7, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD48 4F 50 01", 7, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD68 4F 50 01", 7, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vrsqrt14ssV_VX_k1_HX_WX_2_Data))]
		void Test32_Vrsqrt14ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vrsqrt14ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 4F D3", 6, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 4F D3", 6, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD0B 4F D3", 6, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD8B 4F D3", 6, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vrsqrt14ssV_VX_k1_HX_WX_1_Data))]
		void Test64_Vrsqrt14ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vrsqrt14ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 4F 50 01", 7, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D8B 4F 50 01", 7, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24D08 4F 50 01", 7, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D28 4F 50 01", 7, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D48 4F 50 01", 7, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D68 4F 50 01", 7, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F2CD0B 4F 50 01", 7, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD8B 4F 50 01", 7, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CD08 4F 50 01", 7, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD28 4F 50 01", 7, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD48 4F 50 01", 7, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD68 4F 50 01", 7, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vrsqrt14ssV_VX_k1_HX_WX_2_Data))]
		void Test64_Vrsqrt14ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vrsqrt14ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 4F D3", 6, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D8B 4F D3", 6, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D03 4F D3", 6, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D0B 4F D3", 6, Code.EVEX_Vrsqrt14ss_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD0B 4F D3", 6, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E28D8B 4F D3", 6, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 12CD03 4F D3", 6, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD0B 4F D3", 6, Code.EVEX_Vrsqrt14sd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
			}
		}
	}
}
