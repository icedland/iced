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
	public sealed class DecoderTest_3_0F38B0_0F38B7 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_Vpmadd52luqV_VX_k1_HX_WX_1_Data))]
		void Test16_Vpmadd52luqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vpmadd52luqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD0B B4 50 01", 7, Code.EVEX_Vpmadd52luq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt52, 16, false };
				yield return new object[] { "62 F2CD9D B4 50 01", 7, Code.EVEX_Vpmadd52luq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt52, 8, true };
				yield return new object[] { "62 F2CD08 B4 50 01", 7, Code.EVEX_Vpmadd52luq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt52, 16, false };

				yield return new object[] { "62 F2CD2B B4 50 01", 7, Code.EVEX_Vpmadd52luq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt52, 32, false };
				yield return new object[] { "62 F2CDBD B4 50 01", 7, Code.EVEX_Vpmadd52luq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt52, 8, true };
				yield return new object[] { "62 F2CD28 B4 50 01", 7, Code.EVEX_Vpmadd52luq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt52, 32, false };

				yield return new object[] { "62 F2CD4B B4 50 01", 7, Code.EVEX_Vpmadd52luq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt52, 64, false };
				yield return new object[] { "62 F2CDDD B4 50 01", 7, Code.EVEX_Vpmadd52luq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt52, 8, true };
				yield return new object[] { "62 F2CD48 B4 50 01", 7, Code.EVEX_Vpmadd52luq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt52, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpmadd52luqV_VX_k1_HX_WX_2_Data))]
		void Test16_Vpmadd52luqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vpmadd52luqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD8B B4 D3", 6, Code.EVEX_Vpmadd52luq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B B4 D3", 6, Code.EVEX_Vpmadd52luq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB B4 D3", 6, Code.EVEX_Vpmadd52luq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B B4 D3", 6, Code.EVEX_Vpmadd52luq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB B4 D3", 6, Code.EVEX_Vpmadd52luq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B B4 D3", 6, Code.EVEX_Vpmadd52luq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpmadd52luqV_VX_k1_HX_WX_1_Data))]
		void Test32_Vpmadd52luqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vpmadd52luqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD0B B4 50 01", 7, Code.EVEX_Vpmadd52luq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt52, 16, false };
				yield return new object[] { "62 F2CD9D B4 50 01", 7, Code.EVEX_Vpmadd52luq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt52, 8, true };
				yield return new object[] { "62 F2CD08 B4 50 01", 7, Code.EVEX_Vpmadd52luq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt52, 16, false };

				yield return new object[] { "62 F2CD2B B4 50 01", 7, Code.EVEX_Vpmadd52luq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt52, 32, false };
				yield return new object[] { "62 F2CDBD B4 50 01", 7, Code.EVEX_Vpmadd52luq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt52, 8, true };
				yield return new object[] { "62 F2CD28 B4 50 01", 7, Code.EVEX_Vpmadd52luq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt52, 32, false };

				yield return new object[] { "62 F2CD4B B4 50 01", 7, Code.EVEX_Vpmadd52luq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt52, 64, false };
				yield return new object[] { "62 F2CDDD B4 50 01", 7, Code.EVEX_Vpmadd52luq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt52, 8, true };
				yield return new object[] { "62 F2CD48 B4 50 01", 7, Code.EVEX_Vpmadd52luq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt52, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpmadd52luqV_VX_k1_HX_WX_2_Data))]
		void Test32_Vpmadd52luqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vpmadd52luqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD8B B4 D3", 6, Code.EVEX_Vpmadd52luq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B B4 D3", 6, Code.EVEX_Vpmadd52luq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB B4 D3", 6, Code.EVEX_Vpmadd52luq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B B4 D3", 6, Code.EVEX_Vpmadd52luq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB B4 D3", 6, Code.EVEX_Vpmadd52luq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B B4 D3", 6, Code.EVEX_Vpmadd52luq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpmadd52luqV_VX_k1_HX_WX_1_Data))]
		void Test64_Vpmadd52luqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vpmadd52luqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD0B B4 50 01", 7, Code.EVEX_Vpmadd52luq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt52, 16, false };
				yield return new object[] { "62 F2CD9D B4 50 01", 7, Code.EVEX_Vpmadd52luq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt52, 8, true };
				yield return new object[] { "62 F2CD08 B4 50 01", 7, Code.EVEX_Vpmadd52luq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt52, 16, false };

				yield return new object[] { "62 F2CD2B B4 50 01", 7, Code.EVEX_Vpmadd52luq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt52, 32, false };
				yield return new object[] { "62 F2CDBD B4 50 01", 7, Code.EVEX_Vpmadd52luq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt52, 8, true };
				yield return new object[] { "62 F2CD28 B4 50 01", 7, Code.EVEX_Vpmadd52luq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt52, 32, false };

				yield return new object[] { "62 F2CD4B B4 50 01", 7, Code.EVEX_Vpmadd52luq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt52, 64, false };
				yield return new object[] { "62 F2CDDD B4 50 01", 7, Code.EVEX_Vpmadd52luq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt52, 8, true };
				yield return new object[] { "62 F2CD48 B4 50 01", 7, Code.EVEX_Vpmadd52luq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt52, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpmadd52luqV_VX_k1_HX_WX_2_Data))]
		void Test64_Vpmadd52luqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vpmadd52luqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD8B B4 D3", 6, Code.EVEX_Vpmadd52luq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E28D0B B4 D3", 6, Code.EVEX_Vpmadd52luq_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12CD03 B4 D3", 6, Code.EVEX_Vpmadd52luq_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CD0B B4 D3", 6, Code.EVEX_Vpmadd52luq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2CD0B B4 D3", 6, Code.EVEX_Vpmadd52luq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB B4 D3", 6, Code.EVEX_Vpmadd52luq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E28D2B B4 D3", 6, Code.EVEX_Vpmadd52luq_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12CD23 B4 D3", 6, Code.EVEX_Vpmadd52luq_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B B4 D3", 6, Code.EVEX_Vpmadd52luq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD2B B4 D3", 6, Code.EVEX_Vpmadd52luq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB B4 D3", 6, Code.EVEX_Vpmadd52luq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E28D4B B4 D3", 6, Code.EVEX_Vpmadd52luq_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12CD43 B4 D3", 6, Code.EVEX_Vpmadd52luq_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B B4 D3", 6, Code.EVEX_Vpmadd52luq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD4B B4 D3", 6, Code.EVEX_Vpmadd52luq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpmadd52huqV_VX_k1_HX_WX_1_Data))]
		void Test16_Vpmadd52huqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vpmadd52huqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD0B B5 50 01", 7, Code.EVEX_Vpmadd52huq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt52, 16, false };
				yield return new object[] { "62 F2CD9D B5 50 01", 7, Code.EVEX_Vpmadd52huq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt52, 8, true };
				yield return new object[] { "62 F2CD08 B5 50 01", 7, Code.EVEX_Vpmadd52huq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt52, 16, false };

				yield return new object[] { "62 F2CD2B B5 50 01", 7, Code.EVEX_Vpmadd52huq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt52, 32, false };
				yield return new object[] { "62 F2CDBD B5 50 01", 7, Code.EVEX_Vpmadd52huq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt52, 8, true };
				yield return new object[] { "62 F2CD28 B5 50 01", 7, Code.EVEX_Vpmadd52huq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt52, 32, false };

				yield return new object[] { "62 F2CD4B B5 50 01", 7, Code.EVEX_Vpmadd52huq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt52, 64, false };
				yield return new object[] { "62 F2CDDD B5 50 01", 7, Code.EVEX_Vpmadd52huq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt52, 8, true };
				yield return new object[] { "62 F2CD48 B5 50 01", 7, Code.EVEX_Vpmadd52huq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt52, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpmadd52huqV_VX_k1_HX_WX_2_Data))]
		void Test16_Vpmadd52huqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vpmadd52huqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD8B B5 D3", 6, Code.EVEX_Vpmadd52huq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B B5 D3", 6, Code.EVEX_Vpmadd52huq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB B5 D3", 6, Code.EVEX_Vpmadd52huq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B B5 D3", 6, Code.EVEX_Vpmadd52huq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB B5 D3", 6, Code.EVEX_Vpmadd52huq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B B5 D3", 6, Code.EVEX_Vpmadd52huq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpmadd52huqV_VX_k1_HX_WX_1_Data))]
		void Test32_Vpmadd52huqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vpmadd52huqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD0B B5 50 01", 7, Code.EVEX_Vpmadd52huq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt52, 16, false };
				yield return new object[] { "62 F2CD9D B5 50 01", 7, Code.EVEX_Vpmadd52huq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt52, 8, true };
				yield return new object[] { "62 F2CD08 B5 50 01", 7, Code.EVEX_Vpmadd52huq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt52, 16, false };

				yield return new object[] { "62 F2CD2B B5 50 01", 7, Code.EVEX_Vpmadd52huq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt52, 32, false };
				yield return new object[] { "62 F2CDBD B5 50 01", 7, Code.EVEX_Vpmadd52huq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt52, 8, true };
				yield return new object[] { "62 F2CD28 B5 50 01", 7, Code.EVEX_Vpmadd52huq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt52, 32, false };

				yield return new object[] { "62 F2CD4B B5 50 01", 7, Code.EVEX_Vpmadd52huq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt52, 64, false };
				yield return new object[] { "62 F2CDDD B5 50 01", 7, Code.EVEX_Vpmadd52huq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt52, 8, true };
				yield return new object[] { "62 F2CD48 B5 50 01", 7, Code.EVEX_Vpmadd52huq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt52, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpmadd52huqV_VX_k1_HX_WX_2_Data))]
		void Test32_Vpmadd52huqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vpmadd52huqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD8B B5 D3", 6, Code.EVEX_Vpmadd52huq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B B5 D3", 6, Code.EVEX_Vpmadd52huq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB B5 D3", 6, Code.EVEX_Vpmadd52huq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B B5 D3", 6, Code.EVEX_Vpmadd52huq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB B5 D3", 6, Code.EVEX_Vpmadd52huq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B B5 D3", 6, Code.EVEX_Vpmadd52huq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpmadd52huqV_VX_k1_HX_WX_1_Data))]
		void Test64_Vpmadd52huqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vpmadd52huqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD0B B5 50 01", 7, Code.EVEX_Vpmadd52huq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt52, 16, false };
				yield return new object[] { "62 F2CD9D B5 50 01", 7, Code.EVEX_Vpmadd52huq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt52, 8, true };
				yield return new object[] { "62 F2CD08 B5 50 01", 7, Code.EVEX_Vpmadd52huq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt52, 16, false };

				yield return new object[] { "62 F2CD2B B5 50 01", 7, Code.EVEX_Vpmadd52huq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt52, 32, false };
				yield return new object[] { "62 F2CDBD B5 50 01", 7, Code.EVEX_Vpmadd52huq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt52, 8, true };
				yield return new object[] { "62 F2CD28 B5 50 01", 7, Code.EVEX_Vpmadd52huq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt52, 32, false };

				yield return new object[] { "62 F2CD4B B5 50 01", 7, Code.EVEX_Vpmadd52huq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt52, 64, false };
				yield return new object[] { "62 F2CDDD B5 50 01", 7, Code.EVEX_Vpmadd52huq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt52, 8, true };
				yield return new object[] { "62 F2CD48 B5 50 01", 7, Code.EVEX_Vpmadd52huq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt52, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpmadd52huqV_VX_k1_HX_WX_2_Data))]
		void Test64_Vpmadd52huqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vpmadd52huqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD8B B5 D3", 6, Code.EVEX_Vpmadd52huq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E28D0B B5 D3", 6, Code.EVEX_Vpmadd52huq_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12CD03 B5 D3", 6, Code.EVEX_Vpmadd52huq_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CD0B B5 D3", 6, Code.EVEX_Vpmadd52huq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2CD0B B5 D3", 6, Code.EVEX_Vpmadd52huq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB B5 D3", 6, Code.EVEX_Vpmadd52huq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E28D2B B5 D3", 6, Code.EVEX_Vpmadd52huq_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12CD23 B5 D3", 6, Code.EVEX_Vpmadd52huq_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B B5 D3", 6, Code.EVEX_Vpmadd52huq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD2B B5 D3", 6, Code.EVEX_Vpmadd52huq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB B5 D3", 6, Code.EVEX_Vpmadd52huq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E28D4B B5 D3", 6, Code.EVEX_Vpmadd52huq_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12CD43 B5 D3", 6, Code.EVEX_Vpmadd52huq_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B B5 D3", 6, Code.EVEX_Vpmadd52huq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD4B B5 D3", 6, Code.EVEX_Vpmadd52huq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmaddsub231psV_VX_HX_WX_1_Data))]
		void Test16_Vfmaddsub231psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_Vfmaddsub231psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 B6 10", 5, Code.VEX_Vfmaddsub231ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D B6 10", 5, Code.VEX_Vfmaddsub231ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 B6 10", 5, Code.VEX_Vfmaddsub231pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD B6 10", 5, Code.VEX_Vfmaddsub231pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmaddsub231psV_VX_HX_WX_2_Data))]
		void Test16_Vfmaddsub231psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		}
		public static IEnumerable<object[]> Test16_Vfmaddsub231psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 B6 D3", 5, Code.VEX_Vfmaddsub231ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D B6 D3", 5, Code.VEX_Vfmaddsub231ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 B6 D3", 5, Code.VEX_Vfmaddsub231pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD B6 D3", 5, Code.VEX_Vfmaddsub231pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmaddsub231psV_VX_HX_WX_1_Data))]
		void Test32_Vfmaddsub231psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_Vfmaddsub231psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 B6 10", 5, Code.VEX_Vfmaddsub231ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D B6 10", 5, Code.VEX_Vfmaddsub231ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 B6 10", 5, Code.VEX_Vfmaddsub231pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD B6 10", 5, Code.VEX_Vfmaddsub231pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmaddsub231psV_VX_HX_WX_2_Data))]
		void Test32_Vfmaddsub231psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		}
		public static IEnumerable<object[]> Test32_Vfmaddsub231psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 B6 D3", 5, Code.VEX_Vfmaddsub231ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D B6 D3", 5, Code.VEX_Vfmaddsub231ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 B6 D3", 5, Code.VEX_Vfmaddsub231pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD B6 D3", 5, Code.VEX_Vfmaddsub231pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmaddsub231psV_VX_HX_WX_1_Data))]
		void Test64_Vfmaddsub231psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_Vfmaddsub231psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 B6 10", 5, Code.VEX_Vfmaddsub231ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D B6 10", 5, Code.VEX_Vfmaddsub231ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 B6 10", 5, Code.VEX_Vfmaddsub231pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD B6 10", 5, Code.VEX_Vfmaddsub231pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmaddsub231psV_VX_HX_WX_2_Data))]
		void Test64_Vfmaddsub231psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		}
		public static IEnumerable<object[]> Test64_Vfmaddsub231psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 B6 D3", 5, Code.VEX_Vfmaddsub231ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 B6 D3", 5, Code.VEX_Vfmaddsub231ps_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 B6 D3", 5, Code.VEX_Vfmaddsub231ps_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 B6 D3", 5, Code.VEX_Vfmaddsub231ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E24D B6 D3", 5, Code.VEX_Vfmaddsub231ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D B6 D3", 5, Code.VEX_Vfmaddsub231ps_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D B6 D3", 5, Code.VEX_Vfmaddsub231ps_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D B6 D3", 5, Code.VEX_Vfmaddsub231ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C4E2C9 B6 D3", 5, Code.VEX_Vfmaddsub231pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 B6 D3", 5, Code.VEX_Vfmaddsub231pd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 B6 D3", 5, Code.VEX_Vfmaddsub231pd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 B6 D3", 5, Code.VEX_Vfmaddsub231pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2CD B6 D3", 5, Code.VEX_Vfmaddsub231pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C462CD B6 D3", 5, Code.VEX_Vfmaddsub231pd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E28D B6 D3", 5, Code.VEX_Vfmaddsub231pd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C2CD B6 D3", 5, Code.VEX_Vfmaddsub231pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmaddsub231psV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfmaddsub231psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vfmaddsub231psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmaddsub231psV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfmaddsub231psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vfmaddsub231psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmaddsub231psV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfmaddsub231psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vfmaddsub231psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmaddsub231psV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfmaddsub231psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vfmaddsub231psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmaddsub231psV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfmaddsub231psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vfmaddsub231psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 B6 50 01", 7, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 B6 50 01", 7, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmaddsub231psV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfmaddsub231psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vfmaddsub231psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24DDB B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D2B B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D23 B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D2B B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D1B B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D4B B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D43 B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D4B B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B B6 D3", 6, Code.EVEX_Vfmaddsub231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CDDB B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D2B B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD23 B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD2B B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD1B B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D4B B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD43 B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD4B B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B B6 D3", 6, Code.EVEX_Vfmaddsub231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsubadd231psV_VX_HX_WX_1_Data))]
		void Test16_Vfmsubadd231psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_Vfmsubadd231psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 B7 10", 5, Code.VEX_Vfmsubadd231ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D B7 10", 5, Code.VEX_Vfmsubadd231ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 B7 10", 5, Code.VEX_Vfmsubadd231pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD B7 10", 5, Code.VEX_Vfmsubadd231pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsubadd231psV_VX_HX_WX_2_Data))]
		void Test16_Vfmsubadd231psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		}
		public static IEnumerable<object[]> Test16_Vfmsubadd231psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 B7 D3", 5, Code.VEX_Vfmsubadd231ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D B7 D3", 5, Code.VEX_Vfmsubadd231ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 B7 D3", 5, Code.VEX_Vfmsubadd231pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD B7 D3", 5, Code.VEX_Vfmsubadd231pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsubadd231psV_VX_HX_WX_1_Data))]
		void Test32_Vfmsubadd231psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_Vfmsubadd231psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 B7 10", 5, Code.VEX_Vfmsubadd231ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D B7 10", 5, Code.VEX_Vfmsubadd231ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 B7 10", 5, Code.VEX_Vfmsubadd231pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD B7 10", 5, Code.VEX_Vfmsubadd231pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsubadd231psV_VX_HX_WX_2_Data))]
		void Test32_Vfmsubadd231psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		}
		public static IEnumerable<object[]> Test32_Vfmsubadd231psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 B7 D3", 5, Code.VEX_Vfmsubadd231ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D B7 D3", 5, Code.VEX_Vfmsubadd231ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 B7 D3", 5, Code.VEX_Vfmsubadd231pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD B7 D3", 5, Code.VEX_Vfmsubadd231pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsubadd231psV_VX_HX_WX_1_Data))]
		void Test64_Vfmsubadd231psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_Vfmsubadd231psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 B7 10", 5, Code.VEX_Vfmsubadd231ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D B7 10", 5, Code.VEX_Vfmsubadd231ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 B7 10", 5, Code.VEX_Vfmsubadd231pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD B7 10", 5, Code.VEX_Vfmsubadd231pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsubadd231psV_VX_HX_WX_2_Data))]
		void Test64_Vfmsubadd231psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		}
		public static IEnumerable<object[]> Test64_Vfmsubadd231psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 B7 D3", 5, Code.VEX_Vfmsubadd231ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 B7 D3", 5, Code.VEX_Vfmsubadd231ps_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 B7 D3", 5, Code.VEX_Vfmsubadd231ps_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 B7 D3", 5, Code.VEX_Vfmsubadd231ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E24D B7 D3", 5, Code.VEX_Vfmsubadd231ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D B7 D3", 5, Code.VEX_Vfmsubadd231ps_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D B7 D3", 5, Code.VEX_Vfmsubadd231ps_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D B7 D3", 5, Code.VEX_Vfmsubadd231ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C4E2C9 B7 D3", 5, Code.VEX_Vfmsubadd231pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 B7 D3", 5, Code.VEX_Vfmsubadd231pd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 B7 D3", 5, Code.VEX_Vfmsubadd231pd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 B7 D3", 5, Code.VEX_Vfmsubadd231pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2CD B7 D3", 5, Code.VEX_Vfmsubadd231pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C462CD B7 D3", 5, Code.VEX_Vfmsubadd231pd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E28D B7 D3", 5, Code.VEX_Vfmsubadd231pd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C2CD B7 D3", 5, Code.VEX_Vfmsubadd231pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsubadd231psV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfmsubadd231psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vfmsubadd231psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsubadd231psV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfmsubadd231psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vfmsubadd231psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsubadd231psV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfmsubadd231psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vfmsubadd231psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsubadd231psV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfmsubadd231psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vfmsubadd231psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsubadd231psV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfmsubadd231psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vfmsubadd231psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 B7 50 01", 7, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 B7 50 01", 7, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsubadd231psV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfmsubadd231psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vfmsubadd231psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24DDB B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D2B B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D23 B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D2B B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D1B B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D4B B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D43 B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D4B B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B B7 D3", 6, Code.EVEX_Vfmsubadd231ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CDDB B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D2B B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD23 B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD2B B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD1B B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D4B B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD43 B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD4B B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B B7 D3", 6, Code.EVEX_Vfmsubadd231pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}
	}
}
