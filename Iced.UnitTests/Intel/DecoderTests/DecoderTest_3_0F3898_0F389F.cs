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
	public sealed class DecoderTest_3_0F3898_0F389F : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_Vfmadd132psV_VX_HX_WX_1_Data))]
		void Test16_Vfmadd132psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Vfmadd132psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 98 10", 5, Code.VEX_Vfmadd132ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 98 10", 5, Code.VEX_Vfmadd132ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 98 10", 5, Code.VEX_Vfmadd132pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD 98 10", 5, Code.VEX_Vfmadd132pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmadd132psV_VX_HX_WX_2_Data))]
		void Test16_Vfmadd132psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfmadd132psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 98 D3", 5, Code.VEX_Vfmadd132ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D 98 D3", 5, Code.VEX_Vfmadd132ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 98 D3", 5, Code.VEX_Vfmadd132pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD 98 D3", 5, Code.VEX_Vfmadd132pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd132psV_VX_HX_WX_1_Data))]
		void Test32_Vfmadd132psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Vfmadd132psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 98 10", 5, Code.VEX_Vfmadd132ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 98 10", 5, Code.VEX_Vfmadd132ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 98 10", 5, Code.VEX_Vfmadd132pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD 98 10", 5, Code.VEX_Vfmadd132pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd132psV_VX_HX_WX_2_Data))]
		void Test32_Vfmadd132psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfmadd132psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 98 D3", 5, Code.VEX_Vfmadd132ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D 98 D3", 5, Code.VEX_Vfmadd132ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 98 D3", 5, Code.VEX_Vfmadd132pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD 98 D3", 5, Code.VEX_Vfmadd132pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd132psV_VX_HX_WX_1_Data))]
		void Test64_Vfmadd132psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Vfmadd132psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 98 10", 5, Code.VEX_Vfmadd132ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 98 10", 5, Code.VEX_Vfmadd132ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 98 10", 5, Code.VEX_Vfmadd132pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD 98 10", 5, Code.VEX_Vfmadd132pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd132psV_VX_HX_WX_2_Data))]
		void Test64_Vfmadd132psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfmadd132psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 98 D3", 5, Code.VEX_Vfmadd132ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 98 D3", 5, Code.VEX_Vfmadd132ps_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 98 D3", 5, Code.VEX_Vfmadd132ps_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 98 D3", 5, Code.VEX_Vfmadd132ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E24D 98 D3", 5, Code.VEX_Vfmadd132ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D 98 D3", 5, Code.VEX_Vfmadd132ps_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D 98 D3", 5, Code.VEX_Vfmadd132ps_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D 98 D3", 5, Code.VEX_Vfmadd132ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C4E2C9 98 D3", 5, Code.VEX_Vfmadd132pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 98 D3", 5, Code.VEX_Vfmadd132pd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 98 D3", 5, Code.VEX_Vfmadd132pd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 98 D3", 5, Code.VEX_Vfmadd132pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2CD 98 D3", 5, Code.VEX_Vfmadd132pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C462CD 98 D3", 5, Code.VEX_Vfmadd132pd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E28D 98 D3", 5, Code.VEX_Vfmadd132pd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C2CD 98 D3", 5, Code.VEX_Vfmadd132pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmadd132psV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfmadd132psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmadd132psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 98 50 01", 7, Code.EVEX_Vfmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 98 50 01", 7, Code.EVEX_Vfmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 98 50 01", 7, Code.EVEX_Vfmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 98 50 01", 7, Code.EVEX_Vfmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 98 50 01", 7, Code.EVEX_Vfmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 98 50 01", 7, Code.EVEX_Vfmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 98 50 01", 7, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 98 50 01", 7, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 98 50 01", 7, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 98 50 01", 7, Code.EVEX_Vfmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 98 50 01", 7, Code.EVEX_Vfmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 98 50 01", 7, Code.EVEX_Vfmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 98 50 01", 7, Code.EVEX_Vfmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 98 50 01", 7, Code.EVEX_Vfmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 98 50 01", 7, Code.EVEX_Vfmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 98 50 01", 7, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 98 50 01", 7, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 98 50 01", 7, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmadd132psV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfmadd132psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmadd132psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 98 D3", 6, Code.EVEX_Vfmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB 98 D3", 6, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB 98 D3", 6, Code.EVEX_Vfmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B 98 D3", 6, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB 98 D3", 6, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 98 D3", 6, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 98 D3", 6, Code.EVEX_Vfmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB 98 D3", 6, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB 98 D3", 6, Code.EVEX_Vfmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B 98 D3", 6, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB 98 D3", 6, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 98 D3", 6, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd132psV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfmadd132psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmadd132psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 98 50 01", 7, Code.EVEX_Vfmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 98 50 01", 7, Code.EVEX_Vfmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 98 50 01", 7, Code.EVEX_Vfmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 98 50 01", 7, Code.EVEX_Vfmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 98 50 01", 7, Code.EVEX_Vfmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 98 50 01", 7, Code.EVEX_Vfmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 98 50 01", 7, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 98 50 01", 7, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 98 50 01", 7, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 98 50 01", 7, Code.EVEX_Vfmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 98 50 01", 7, Code.EVEX_Vfmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 98 50 01", 7, Code.EVEX_Vfmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 98 50 01", 7, Code.EVEX_Vfmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 98 50 01", 7, Code.EVEX_Vfmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 98 50 01", 7, Code.EVEX_Vfmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 98 50 01", 7, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 98 50 01", 7, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 98 50 01", 7, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd132psV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfmadd132psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmadd132psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 98 D3", 6, Code.EVEX_Vfmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB 98 D3", 6, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB 98 D3", 6, Code.EVEX_Vfmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B 98 D3", 6, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB 98 D3", 6, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 98 D3", 6, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 98 D3", 6, Code.EVEX_Vfmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB 98 D3", 6, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB 98 D3", 6, Code.EVEX_Vfmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B 98 D3", 6, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB 98 D3", 6, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 98 D3", 6, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd132psV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfmadd132psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmadd132psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 98 50 01", 7, Code.EVEX_Vfmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 98 50 01", 7, Code.EVEX_Vfmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 98 50 01", 7, Code.EVEX_Vfmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 98 50 01", 7, Code.EVEX_Vfmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 98 50 01", 7, Code.EVEX_Vfmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 98 50 01", 7, Code.EVEX_Vfmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 98 50 01", 7, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 98 50 01", 7, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 98 50 01", 7, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 98 50 01", 7, Code.EVEX_Vfmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 98 50 01", 7, Code.EVEX_Vfmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 98 50 01", 7, Code.EVEX_Vfmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 98 50 01", 7, Code.EVEX_Vfmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 98 50 01", 7, Code.EVEX_Vfmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 98 50 01", 7, Code.EVEX_Vfmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 98 50 01", 7, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 98 50 01", 7, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 98 50 01", 7, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd132psV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfmadd132psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmadd132psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 98 D3", 6, Code.EVEX_Vfmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B 98 D3", 6, Code.EVEX_Vfmadd132ps_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 98 D3", 6, Code.EVEX_Vfmadd132ps_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B 98 D3", 6, Code.EVEX_Vfmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24DDB 98 D3", 6, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB 98 D3", 6, Code.EVEX_Vfmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D2B 98 D3", 6, Code.EVEX_Vfmadd132ps_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D23 98 D3", 6, Code.EVEX_Vfmadd132ps_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D2B 98 D3", 6, Code.EVEX_Vfmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D1B 98 D3", 6, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB 98 D3", 6, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D4B 98 D3", 6, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D43 98 D3", 6, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D4B 98 D3", 6, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B 98 D3", 6, Code.EVEX_Vfmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 98 D3", 6, Code.EVEX_Vfmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B 98 D3", 6, Code.EVEX_Vfmadd132pd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 98 D3", 6, Code.EVEX_Vfmadd132pd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B 98 D3", 6, Code.EVEX_Vfmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CDDB 98 D3", 6, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB 98 D3", 6, Code.EVEX_Vfmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D2B 98 D3", 6, Code.EVEX_Vfmadd132pd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD23 98 D3", 6, Code.EVEX_Vfmadd132pd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD2B 98 D3", 6, Code.EVEX_Vfmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD1B 98 D3", 6, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB 98 D3", 6, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D4B 98 D3", 6, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD43 98 D3", 6, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD4B 98 D3", 6, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B 98 D3", 6, Code.EVEX_Vfmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmadd132ssV_VX_HX_WX_1_Data))]
		void Test16_Vfmadd132ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Vfmadd132ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 99 10", 5, Code.VEX_Vfmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D 99 10", 5, Code.VEX_Vfmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 99 10", 5, Code.VEX_Vfmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD 99 10", 5, Code.VEX_Vfmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmadd132ssV_VX_HX_WX_2_Data))]
		void Test16_Vfmadd132ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfmadd132ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 99 D3", 5, Code.VEX_Vfmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 99 D3", 5, Code.VEX_Vfmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd132ssV_VX_HX_WX_1_Data))]
		void Test32_Vfmadd132ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Vfmadd132ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 99 10", 5, Code.VEX_Vfmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D 99 10", 5, Code.VEX_Vfmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 99 10", 5, Code.VEX_Vfmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD 99 10", 5, Code.VEX_Vfmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd132ssV_VX_HX_WX_2_Data))]
		void Test32_Vfmadd132ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfmadd132ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 99 D3", 5, Code.VEX_Vfmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 99 D3", 5, Code.VEX_Vfmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd132ssV_VX_HX_WX_1_Data))]
		void Test64_Vfmadd132ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Vfmadd132ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 99 10", 5, Code.VEX_Vfmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D 99 10", 5, Code.VEX_Vfmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 99 10", 5, Code.VEX_Vfmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD 99 10", 5, Code.VEX_Vfmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd132ssV_VX_HX_WX_2_Data))]
		void Test64_Vfmadd132ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfmadd132ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 99 D3", 5, Code.VEX_Vfmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 99 D3", 5, Code.VEX_Vfmadd132ss_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 99 D3", 5, Code.VEX_Vfmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 99 D3", 5, Code.VEX_Vfmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2C9 99 D3", 5, Code.VEX_Vfmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 99 D3", 5, Code.VEX_Vfmadd132sd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 99 D3", 5, Code.VEX_Vfmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 99 D3", 5, Code.VEX_Vfmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmadd132ssV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfmadd132ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmadd132ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 99 50 01", 7, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 99 50 01", 7, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB 99 50 01", 7, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB 99 50 01", 7, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB 99 50 01", 7, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B 99 50 01", 7, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 99 50 01", 7, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB 99 50 01", 7, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB 99 50 01", 7, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB 99 50 01", 7, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmadd132ssV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfmadd132ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmadd132ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 99 D3", 6, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 99 D3", 6, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 99 D3", 6, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 99 D3", 6, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd132ssV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfmadd132ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmadd132ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 99 50 01", 7, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 99 50 01", 7, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB 99 50 01", 7, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB 99 50 01", 7, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB 99 50 01", 7, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B 99 50 01", 7, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 99 50 01", 7, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB 99 50 01", 7, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB 99 50 01", 7, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB 99 50 01", 7, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd132ssV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfmadd132ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmadd132ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 99 D3", 6, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 99 D3", 6, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 99 D3", 6, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 99 D3", 6, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd132ssV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfmadd132ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmadd132ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 99 50 01", 7, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 99 50 01", 7, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB 99 50 01", 7, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB 99 50 01", 7, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB 99 50 01", 7, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B 99 50 01", 7, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 99 50 01", 7, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB 99 50 01", 7, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB 99 50 01", 7, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB 99 50 01", 7, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd132ssV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfmadd132ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmadd132ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 99 D3", 6, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B 99 D3", 6, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 99 D3", 6, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B 99 D3", 6, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B 99 D3", 6, Code.EVEX_Vfmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 99 D3", 6, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B 99 D3", 6, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 99 D3", 6, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B 99 D3", 6, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B 99 D3", 6, Code.EVEX_Vfmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub132psV_VX_HX_WX_1_Data))]
		void Test16_Vfmsub132psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Vfmsub132psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 9A 10", 5, Code.VEX_Vfmsub132ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 9A 10", 5, Code.VEX_Vfmsub132ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 9A 10", 5, Code.VEX_Vfmsub132pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD 9A 10", 5, Code.VEX_Vfmsub132pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub132psV_VX_HX_WX_2_Data))]
		void Test16_Vfmsub132psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfmsub132psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 9A D3", 5, Code.VEX_Vfmsub132ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D 9A D3", 5, Code.VEX_Vfmsub132ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 9A D3", 5, Code.VEX_Vfmsub132pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD 9A D3", 5, Code.VEX_Vfmsub132pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub132psV_VX_HX_WX_1_Data))]
		void Test32_Vfmsub132psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Vfmsub132psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 9A 10", 5, Code.VEX_Vfmsub132ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 9A 10", 5, Code.VEX_Vfmsub132ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 9A 10", 5, Code.VEX_Vfmsub132pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD 9A 10", 5, Code.VEX_Vfmsub132pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub132psV_VX_HX_WX_2_Data))]
		void Test32_Vfmsub132psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfmsub132psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 9A D3", 5, Code.VEX_Vfmsub132ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D 9A D3", 5, Code.VEX_Vfmsub132ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 9A D3", 5, Code.VEX_Vfmsub132pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD 9A D3", 5, Code.VEX_Vfmsub132pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub132psV_VX_HX_WX_1_Data))]
		void Test64_Vfmsub132psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Vfmsub132psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 9A 10", 5, Code.VEX_Vfmsub132ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 9A 10", 5, Code.VEX_Vfmsub132ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 9A 10", 5, Code.VEX_Vfmsub132pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD 9A 10", 5, Code.VEX_Vfmsub132pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub132psV_VX_HX_WX_2_Data))]
		void Test64_Vfmsub132psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfmsub132psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 9A D3", 5, Code.VEX_Vfmsub132ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 9A D3", 5, Code.VEX_Vfmsub132ps_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 9A D3", 5, Code.VEX_Vfmsub132ps_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 9A D3", 5, Code.VEX_Vfmsub132ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E24D 9A D3", 5, Code.VEX_Vfmsub132ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D 9A D3", 5, Code.VEX_Vfmsub132ps_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D 9A D3", 5, Code.VEX_Vfmsub132ps_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D 9A D3", 5, Code.VEX_Vfmsub132ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C4E2C9 9A D3", 5, Code.VEX_Vfmsub132pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 9A D3", 5, Code.VEX_Vfmsub132pd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 9A D3", 5, Code.VEX_Vfmsub132pd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 9A D3", 5, Code.VEX_Vfmsub132pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2CD 9A D3", 5, Code.VEX_Vfmsub132pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C462CD 9A D3", 5, Code.VEX_Vfmsub132pd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E28D 9A D3", 5, Code.VEX_Vfmsub132pd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C2CD 9A D3", 5, Code.VEX_Vfmsub132pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub132psV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfmsub132psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmsub132psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub132psV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfmsub132psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmsub132psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 9A D3", 6, Code.EVEX_Vfmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB 9A D3", 6, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB 9A D3", 6, Code.EVEX_Vfmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B 9A D3", 6, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB 9A D3", 6, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 9A D3", 6, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 9A D3", 6, Code.EVEX_Vfmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB 9A D3", 6, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB 9A D3", 6, Code.EVEX_Vfmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B 9A D3", 6, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB 9A D3", 6, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 9A D3", 6, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub132psV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfmsub132psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmsub132psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub132psV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfmsub132psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmsub132psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 9A D3", 6, Code.EVEX_Vfmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB 9A D3", 6, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB 9A D3", 6, Code.EVEX_Vfmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B 9A D3", 6, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB 9A D3", 6, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 9A D3", 6, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 9A D3", 6, Code.EVEX_Vfmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB 9A D3", 6, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB 9A D3", 6, Code.EVEX_Vfmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B 9A D3", 6, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB 9A D3", 6, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 9A D3", 6, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub132psV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfmsub132psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmsub132psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 9A 50 01", 7, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 9A 50 01", 7, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub132psV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfmsub132psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmsub132psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 9A D3", 6, Code.EVEX_Vfmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B 9A D3", 6, Code.EVEX_Vfmsub132ps_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 9A D3", 6, Code.EVEX_Vfmsub132ps_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B 9A D3", 6, Code.EVEX_Vfmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24DDB 9A D3", 6, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB 9A D3", 6, Code.EVEX_Vfmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D2B 9A D3", 6, Code.EVEX_Vfmsub132ps_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D23 9A D3", 6, Code.EVEX_Vfmsub132ps_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D2B 9A D3", 6, Code.EVEX_Vfmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D1B 9A D3", 6, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB 9A D3", 6, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D4B 9A D3", 6, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D43 9A D3", 6, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D4B 9A D3", 6, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B 9A D3", 6, Code.EVEX_Vfmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 9A D3", 6, Code.EVEX_Vfmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B 9A D3", 6, Code.EVEX_Vfmsub132pd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 9A D3", 6, Code.EVEX_Vfmsub132pd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B 9A D3", 6, Code.EVEX_Vfmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CDDB 9A D3", 6, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB 9A D3", 6, Code.EVEX_Vfmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D2B 9A D3", 6, Code.EVEX_Vfmsub132pd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD23 9A D3", 6, Code.EVEX_Vfmsub132pd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD2B 9A D3", 6, Code.EVEX_Vfmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD1B 9A D3", 6, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB 9A D3", 6, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D4B 9A D3", 6, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD43 9A D3", 6, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD4B 9A D3", 6, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B 9A D3", 6, Code.EVEX_Vfmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_V4fmaddpsV_VX_k1z_HX_WX_1_Data))]
		void Test16_V4fmaddpsV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_V4fmaddpsV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F4B 9A 50 01", 7, Code.EVEX_V4fmaddps_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24FCB 9A 50 01", 7, Code.EVEX_V4fmaddps_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Float32, 16, true };
				yield return new object[] { "62 F24F48 9A 50 01", 7, Code.EVEX_V4fmaddps_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Float32, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_V4fmaddpsV_VX_k1z_HX_WX_1_Data))]
		void Test32_V4fmaddpsV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_V4fmaddpsV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F4B 9A 50 01", 7, Code.EVEX_V4fmaddps_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24FCB 9A 50 01", 7, Code.EVEX_V4fmaddps_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Float32, 16, true };
				yield return new object[] { "62 F24F48 9A 50 01", 7, Code.EVEX_V4fmaddps_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Float32, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_V4fmaddpsV_VX_k1z_HX_WX_1_Data))]
		void Test64_V4fmaddpsV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_V4fmaddpsV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F4B 9A 50 01", 7, Code.EVEX_V4fmaddps_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 E20FCB 9A 50 01", 7, Code.EVEX_V4fmaddps_VZ_k1z_HZP3_M, Register.ZMM18, Register.ZMM14, Register.K3, MemorySize.Packed128_Float32, 16, true };
				yield return new object[] { "62 724F40 9A 50 01", 7, Code.EVEX_V4fmaddps_VZ_k1z_HZP3_M, Register.ZMM10, Register.ZMM22, Register.None, MemorySize.Packed128_Float32, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub132ssV_VX_HX_WX_1_Data))]
		void Test16_Vfmsub132ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Vfmsub132ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 9B 10", 5, Code.VEX_Vfmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D 9B 10", 5, Code.VEX_Vfmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 9B 10", 5, Code.VEX_Vfmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD 9B 10", 5, Code.VEX_Vfmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub132ssV_VX_HX_WX_2_Data))]
		void Test16_Vfmsub132ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfmsub132ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 9B D3", 5, Code.VEX_Vfmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 9B D3", 5, Code.VEX_Vfmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub132ssV_VX_HX_WX_1_Data))]
		void Test32_Vfmsub132ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Vfmsub132ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 9B 10", 5, Code.VEX_Vfmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D 9B 10", 5, Code.VEX_Vfmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 9B 10", 5, Code.VEX_Vfmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD 9B 10", 5, Code.VEX_Vfmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub132ssV_VX_HX_WX_2_Data))]
		void Test32_Vfmsub132ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfmsub132ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 9B D3", 5, Code.VEX_Vfmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 9B D3", 5, Code.VEX_Vfmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub132ssV_VX_HX_WX_1_Data))]
		void Test64_Vfmsub132ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Vfmsub132ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 9B 10", 5, Code.VEX_Vfmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D 9B 10", 5, Code.VEX_Vfmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 9B 10", 5, Code.VEX_Vfmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD 9B 10", 5, Code.VEX_Vfmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub132ssV_VX_HX_WX_2_Data))]
		void Test64_Vfmsub132ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfmsub132ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 9B D3", 5, Code.VEX_Vfmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 9B D3", 5, Code.VEX_Vfmsub132ss_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 9B D3", 5, Code.VEX_Vfmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 9B D3", 5, Code.VEX_Vfmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2C9 9B D3", 5, Code.VEX_Vfmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 9B D3", 5, Code.VEX_Vfmsub132sd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 9B D3", 5, Code.VEX_Vfmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 9B D3", 5, Code.VEX_Vfmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub132ssV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfmsub132ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmsub132ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 9B 50 01", 7, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 9B 50 01", 7, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB 9B 50 01", 7, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB 9B 50 01", 7, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB 9B 50 01", 7, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B 9B 50 01", 7, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 9B 50 01", 7, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB 9B 50 01", 7, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB 9B 50 01", 7, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB 9B 50 01", 7, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub132ssV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfmsub132ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmsub132ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 9B D3", 6, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 9B D3", 6, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 9B D3", 6, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 9B D3", 6, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub132ssV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfmsub132ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmsub132ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 9B 50 01", 7, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 9B 50 01", 7, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB 9B 50 01", 7, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB 9B 50 01", 7, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB 9B 50 01", 7, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B 9B 50 01", 7, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 9B 50 01", 7, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB 9B 50 01", 7, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB 9B 50 01", 7, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB 9B 50 01", 7, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub132ssV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfmsub132ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmsub132ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 9B D3", 6, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 9B D3", 6, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 9B D3", 6, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 9B D3", 6, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub132ssV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfmsub132ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmsub132ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 9B 50 01", 7, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 9B 50 01", 7, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB 9B 50 01", 7, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB 9B 50 01", 7, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB 9B 50 01", 7, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B 9B 50 01", 7, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 9B 50 01", 7, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB 9B 50 01", 7, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB 9B 50 01", 7, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB 9B 50 01", 7, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub132ssV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfmsub132ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmsub132ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 9B D3", 6, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B 9B D3", 6, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 9B D3", 6, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B 9B D3", 6, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B 9B D3", 6, Code.EVEX_Vfmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 9B D3", 6, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B 9B D3", 6, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 9B D3", 6, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B 9B D3", 6, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B 9B D3", 6, Code.EVEX_Vfmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_V4fmaddssV_VX_k1z_HX_WX_1_Data))]
		void Test16_V4fmaddssV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_V4fmaddssV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F0B 9B 50 01", 7, Code.EVEX_V4fmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F8B 9B 50 01", 7, Code.EVEX_V4fmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, true };
				yield return new object[] { "62 F24F08 9B 50 01", 7, Code.EVEX_V4fmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F2B 9B 50 01", 7, Code.EVEX_V4fmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F4B 9B 50 01", 7, Code.EVEX_V4fmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F6B 9B 50 01", 7, Code.EVEX_V4fmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_V4fmaddssV_VX_k1z_HX_WX_1_Data))]
		void Test32_V4fmaddssV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_V4fmaddssV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F0B 9B 50 01", 7, Code.EVEX_V4fmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F8B 9B 50 01", 7, Code.EVEX_V4fmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, true };
				yield return new object[] { "62 F24F08 9B 50 01", 7, Code.EVEX_V4fmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F2B 9B 50 01", 7, Code.EVEX_V4fmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F4B 9B 50 01", 7, Code.EVEX_V4fmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F6B 9B 50 01", 7, Code.EVEX_V4fmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_V4fmaddssV_VX_k1z_HX_WX_1_Data))]
		void Test64_V4fmaddssV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_V4fmaddssV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F0B 9B 50 01", 7, Code.EVEX_V4fmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 E20F8B 9B 50 01", 7, Code.EVEX_V4fmaddss_VX_k1z_HXP3_M, Register.XMM18, Register.XMM14, Register.K3, MemorySize.Packed128_Float32, 16, true };
				yield return new object[] { "62 724F00 9B 50 01", 7, Code.EVEX_V4fmaddss_VX_k1z_HXP3_M, Register.XMM10, Register.XMM22, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F2B 9B 50 01", 7, Code.EVEX_V4fmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F4B 9B 50 01", 7, Code.EVEX_V4fmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F6B 9B 50 01", 7, Code.EVEX_V4fmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd132psV_VX_HX_WX_1_Data))]
		void Test16_Vfnmadd132psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Vfnmadd132psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 9C 10", 5, Code.VEX_Vfnmadd132ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 9C 10", 5, Code.VEX_Vfnmadd132ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 9C 10", 5, Code.VEX_Vfnmadd132pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD 9C 10", 5, Code.VEX_Vfnmadd132pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd132psV_VX_HX_WX_2_Data))]
		void Test16_Vfnmadd132psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfnmadd132psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 9C D3", 5, Code.VEX_Vfnmadd132ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D 9C D3", 5, Code.VEX_Vfnmadd132ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 9C D3", 5, Code.VEX_Vfnmadd132pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD 9C D3", 5, Code.VEX_Vfnmadd132pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd132psV_VX_HX_WX_1_Data))]
		void Test32_Vfnmadd132psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Vfnmadd132psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 9C 10", 5, Code.VEX_Vfnmadd132ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 9C 10", 5, Code.VEX_Vfnmadd132ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 9C 10", 5, Code.VEX_Vfnmadd132pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD 9C 10", 5, Code.VEX_Vfnmadd132pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd132psV_VX_HX_WX_2_Data))]
		void Test32_Vfnmadd132psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfnmadd132psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 9C D3", 5, Code.VEX_Vfnmadd132ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D 9C D3", 5, Code.VEX_Vfnmadd132ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 9C D3", 5, Code.VEX_Vfnmadd132pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD 9C D3", 5, Code.VEX_Vfnmadd132pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd132psV_VX_HX_WX_1_Data))]
		void Test64_Vfnmadd132psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Vfnmadd132psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 9C 10", 5, Code.VEX_Vfnmadd132ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 9C 10", 5, Code.VEX_Vfnmadd132ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 9C 10", 5, Code.VEX_Vfnmadd132pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD 9C 10", 5, Code.VEX_Vfnmadd132pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd132psV_VX_HX_WX_2_Data))]
		void Test64_Vfnmadd132psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfnmadd132psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 9C D3", 5, Code.VEX_Vfnmadd132ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 9C D3", 5, Code.VEX_Vfnmadd132ps_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 9C D3", 5, Code.VEX_Vfnmadd132ps_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 9C D3", 5, Code.VEX_Vfnmadd132ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E24D 9C D3", 5, Code.VEX_Vfnmadd132ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D 9C D3", 5, Code.VEX_Vfnmadd132ps_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D 9C D3", 5, Code.VEX_Vfnmadd132ps_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D 9C D3", 5, Code.VEX_Vfnmadd132ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C4E2C9 9C D3", 5, Code.VEX_Vfnmadd132pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 9C D3", 5, Code.VEX_Vfnmadd132pd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 9C D3", 5, Code.VEX_Vfnmadd132pd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 9C D3", 5, Code.VEX_Vfnmadd132pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2CD 9C D3", 5, Code.VEX_Vfnmadd132pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C462CD 9C D3", 5, Code.VEX_Vfnmadd132pd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E28D 9C D3", 5, Code.VEX_Vfnmadd132pd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C2CD 9C D3", 5, Code.VEX_Vfnmadd132pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd132psV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfnmadd132psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfnmadd132psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd132psV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfnmadd132psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfnmadd132psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 9C D3", 6, Code.EVEX_Vfnmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB 9C D3", 6, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB 9C D3", 6, Code.EVEX_Vfnmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B 9C D3", 6, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB 9C D3", 6, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 9C D3", 6, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 9C D3", 6, Code.EVEX_Vfnmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB 9C D3", 6, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB 9C D3", 6, Code.EVEX_Vfnmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B 9C D3", 6, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB 9C D3", 6, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 9C D3", 6, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd132psV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfnmadd132psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfnmadd132psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd132psV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfnmadd132psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfnmadd132psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 9C D3", 6, Code.EVEX_Vfnmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB 9C D3", 6, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB 9C D3", 6, Code.EVEX_Vfnmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B 9C D3", 6, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB 9C D3", 6, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 9C D3", 6, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 9C D3", 6, Code.EVEX_Vfnmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB 9C D3", 6, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB 9C D3", 6, Code.EVEX_Vfnmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B 9C D3", 6, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB 9C D3", 6, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 9C D3", 6, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd132psV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfnmadd132psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfnmadd132psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 9C 50 01", 7, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 9C 50 01", 7, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd132psV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfnmadd132psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfnmadd132psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 9C D3", 6, Code.EVEX_Vfnmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B 9C D3", 6, Code.EVEX_Vfnmadd132ps_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 9C D3", 6, Code.EVEX_Vfnmadd132ps_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B 9C D3", 6, Code.EVEX_Vfnmadd132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24DDB 9C D3", 6, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB 9C D3", 6, Code.EVEX_Vfnmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D2B 9C D3", 6, Code.EVEX_Vfnmadd132ps_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D23 9C D3", 6, Code.EVEX_Vfnmadd132ps_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D2B 9C D3", 6, Code.EVEX_Vfnmadd132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D1B 9C D3", 6, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB 9C D3", 6, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D4B 9C D3", 6, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D43 9C D3", 6, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D4B 9C D3", 6, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B 9C D3", 6, Code.EVEX_Vfnmadd132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 9C D3", 6, Code.EVEX_Vfnmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B 9C D3", 6, Code.EVEX_Vfnmadd132pd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 9C D3", 6, Code.EVEX_Vfnmadd132pd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B 9C D3", 6, Code.EVEX_Vfnmadd132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CDDB 9C D3", 6, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB 9C D3", 6, Code.EVEX_Vfnmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D2B 9C D3", 6, Code.EVEX_Vfnmadd132pd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD23 9C D3", 6, Code.EVEX_Vfnmadd132pd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD2B 9C D3", 6, Code.EVEX_Vfnmadd132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD1B 9C D3", 6, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB 9C D3", 6, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D4B 9C D3", 6, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD43 9C D3", 6, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD4B 9C D3", 6, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B 9C D3", 6, Code.EVEX_Vfnmadd132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd132ssV_VX_HX_WX_1_Data))]
		void Test16_Vfnmadd132ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Vfnmadd132ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 9D 10", 5, Code.VEX_Vfnmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D 9D 10", 5, Code.VEX_Vfnmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 9D 10", 5, Code.VEX_Vfnmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD 9D 10", 5, Code.VEX_Vfnmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd132ssV_VX_HX_WX_2_Data))]
		void Test16_Vfnmadd132ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfnmadd132ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 9D D3", 5, Code.VEX_Vfnmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 9D D3", 5, Code.VEX_Vfnmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd132ssV_VX_HX_WX_1_Data))]
		void Test32_Vfnmadd132ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Vfnmadd132ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 9D 10", 5, Code.VEX_Vfnmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D 9D 10", 5, Code.VEX_Vfnmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 9D 10", 5, Code.VEX_Vfnmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD 9D 10", 5, Code.VEX_Vfnmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd132ssV_VX_HX_WX_2_Data))]
		void Test32_Vfnmadd132ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfnmadd132ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 9D D3", 5, Code.VEX_Vfnmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 9D D3", 5, Code.VEX_Vfnmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd132ssV_VX_HX_WX_1_Data))]
		void Test64_Vfnmadd132ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Vfnmadd132ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 9D 10", 5, Code.VEX_Vfnmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D 9D 10", 5, Code.VEX_Vfnmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 9D 10", 5, Code.VEX_Vfnmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD 9D 10", 5, Code.VEX_Vfnmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd132ssV_VX_HX_WX_2_Data))]
		void Test64_Vfnmadd132ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfnmadd132ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 9D D3", 5, Code.VEX_Vfnmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 9D D3", 5, Code.VEX_Vfnmadd132ss_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 9D D3", 5, Code.VEX_Vfnmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 9D D3", 5, Code.VEX_Vfnmadd132ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2C9 9D D3", 5, Code.VEX_Vfnmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 9D D3", 5, Code.VEX_Vfnmadd132sd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 9D D3", 5, Code.VEX_Vfnmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 9D D3", 5, Code.VEX_Vfnmadd132sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd132ssV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfnmadd132ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfnmadd132ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 9D 50 01", 7, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 9D 50 01", 7, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB 9D 50 01", 7, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB 9D 50 01", 7, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB 9D 50 01", 7, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B 9D 50 01", 7, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 9D 50 01", 7, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB 9D 50 01", 7, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB 9D 50 01", 7, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB 9D 50 01", 7, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd132ssV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfnmadd132ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfnmadd132ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 9D D3", 6, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 9D D3", 6, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 9D D3", 6, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 9D D3", 6, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd132ssV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfnmadd132ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfnmadd132ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 9D 50 01", 7, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 9D 50 01", 7, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB 9D 50 01", 7, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB 9D 50 01", 7, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB 9D 50 01", 7, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B 9D 50 01", 7, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 9D 50 01", 7, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB 9D 50 01", 7, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB 9D 50 01", 7, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB 9D 50 01", 7, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd132ssV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfnmadd132ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfnmadd132ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 9D D3", 6, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 9D D3", 6, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 9D D3", 6, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 9D D3", 6, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd132ssV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfnmadd132ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfnmadd132ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 9D 50 01", 7, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 9D 50 01", 7, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB 9D 50 01", 7, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB 9D 50 01", 7, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB 9D 50 01", 7, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B 9D 50 01", 7, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 9D 50 01", 7, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB 9D 50 01", 7, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB 9D 50 01", 7, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB 9D 50 01", 7, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd132ssV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfnmadd132ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfnmadd132ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 9D D3", 6, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B 9D D3", 6, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 9D D3", 6, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B 9D D3", 6, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B 9D D3", 6, Code.EVEX_Vfnmadd132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 9D D3", 6, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B 9D D3", 6, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 9D D3", 6, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B 9D D3", 6, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B 9D D3", 6, Code.EVEX_Vfnmadd132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub132psV_VX_HX_WX_1_Data))]
		void Test16_Vfnmsub132psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Vfnmsub132psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 9E 10", 5, Code.VEX_Vfnmsub132ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 9E 10", 5, Code.VEX_Vfnmsub132ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 9E 10", 5, Code.VEX_Vfnmsub132pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD 9E 10", 5, Code.VEX_Vfnmsub132pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub132psV_VX_HX_WX_2_Data))]
		void Test16_Vfnmsub132psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfnmsub132psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 9E D3", 5, Code.VEX_Vfnmsub132ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D 9E D3", 5, Code.VEX_Vfnmsub132ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 9E D3", 5, Code.VEX_Vfnmsub132pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD 9E D3", 5, Code.VEX_Vfnmsub132pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub132psV_VX_HX_WX_1_Data))]
		void Test32_Vfnmsub132psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Vfnmsub132psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 9E 10", 5, Code.VEX_Vfnmsub132ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 9E 10", 5, Code.VEX_Vfnmsub132ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 9E 10", 5, Code.VEX_Vfnmsub132pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD 9E 10", 5, Code.VEX_Vfnmsub132pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub132psV_VX_HX_WX_2_Data))]
		void Test32_Vfnmsub132psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfnmsub132psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 9E D3", 5, Code.VEX_Vfnmsub132ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D 9E D3", 5, Code.VEX_Vfnmsub132ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 9E D3", 5, Code.VEX_Vfnmsub132pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD 9E D3", 5, Code.VEX_Vfnmsub132pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub132psV_VX_HX_WX_1_Data))]
		void Test64_Vfnmsub132psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Vfnmsub132psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 9E 10", 5, Code.VEX_Vfnmsub132ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D 9E 10", 5, Code.VEX_Vfnmsub132ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 9E 10", 5, Code.VEX_Vfnmsub132pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD 9E 10", 5, Code.VEX_Vfnmsub132pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub132psV_VX_HX_WX_2_Data))]
		void Test64_Vfnmsub132psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfnmsub132psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 9E D3", 5, Code.VEX_Vfnmsub132ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 9E D3", 5, Code.VEX_Vfnmsub132ps_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 9E D3", 5, Code.VEX_Vfnmsub132ps_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 9E D3", 5, Code.VEX_Vfnmsub132ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E24D 9E D3", 5, Code.VEX_Vfnmsub132ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D 9E D3", 5, Code.VEX_Vfnmsub132ps_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D 9E D3", 5, Code.VEX_Vfnmsub132ps_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D 9E D3", 5, Code.VEX_Vfnmsub132ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C4E2C9 9E D3", 5, Code.VEX_Vfnmsub132pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 9E D3", 5, Code.VEX_Vfnmsub132pd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 9E D3", 5, Code.VEX_Vfnmsub132pd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 9E D3", 5, Code.VEX_Vfnmsub132pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2CD 9E D3", 5, Code.VEX_Vfnmsub132pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C462CD 9E D3", 5, Code.VEX_Vfnmsub132pd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E28D 9E D3", 5, Code.VEX_Vfnmsub132pd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C2CD 9E D3", 5, Code.VEX_Vfnmsub132pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub132psV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfnmsub132psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfnmsub132psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub132psV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfnmsub132psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfnmsub132psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 9E D3", 6, Code.EVEX_Vfnmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB 9E D3", 6, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB 9E D3", 6, Code.EVEX_Vfnmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B 9E D3", 6, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB 9E D3", 6, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 9E D3", 6, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 9E D3", 6, Code.EVEX_Vfnmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB 9E D3", 6, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB 9E D3", 6, Code.EVEX_Vfnmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B 9E D3", 6, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB 9E D3", 6, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 9E D3", 6, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub132psV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfnmsub132psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfnmsub132psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub132psV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfnmsub132psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfnmsub132psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 9E D3", 6, Code.EVEX_Vfnmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB 9E D3", 6, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB 9E D3", 6, Code.EVEX_Vfnmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B 9E D3", 6, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB 9E D3", 6, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 9E D3", 6, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 9E D3", 6, Code.EVEX_Vfnmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB 9E D3", 6, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB 9E D3", 6, Code.EVEX_Vfnmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B 9E D3", 6, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB 9E D3", 6, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 9E D3", 6, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub132psV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfnmsub132psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfnmsub132psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 9E 50 01", 7, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 9E 50 01", 7, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub132psV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfnmsub132psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfnmsub132psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 9E D3", 6, Code.EVEX_Vfnmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B 9E D3", 6, Code.EVEX_Vfnmsub132ps_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 9E D3", 6, Code.EVEX_Vfnmsub132ps_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B 9E D3", 6, Code.EVEX_Vfnmsub132ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24DDB 9E D3", 6, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB 9E D3", 6, Code.EVEX_Vfnmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D2B 9E D3", 6, Code.EVEX_Vfnmsub132ps_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D23 9E D3", 6, Code.EVEX_Vfnmsub132ps_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D2B 9E D3", 6, Code.EVEX_Vfnmsub132ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D1B 9E D3", 6, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB 9E D3", 6, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D4B 9E D3", 6, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D43 9E D3", 6, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D4B 9E D3", 6, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B 9E D3", 6, Code.EVEX_Vfnmsub132ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 9E D3", 6, Code.EVEX_Vfnmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B 9E D3", 6, Code.EVEX_Vfnmsub132pd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 9E D3", 6, Code.EVEX_Vfnmsub132pd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B 9E D3", 6, Code.EVEX_Vfnmsub132pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CDDB 9E D3", 6, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB 9E D3", 6, Code.EVEX_Vfnmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D2B 9E D3", 6, Code.EVEX_Vfnmsub132pd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD23 9E D3", 6, Code.EVEX_Vfnmsub132pd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD2B 9E D3", 6, Code.EVEX_Vfnmsub132pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD1B 9E D3", 6, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB 9E D3", 6, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D4B 9E D3", 6, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD43 9E D3", 6, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD4B 9E D3", 6, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B 9E D3", 6, Code.EVEX_Vfnmsub132pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub132ssV_VX_HX_WX_1_Data))]
		void Test16_Vfnmsub132ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Vfnmsub132ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 9F 10", 5, Code.VEX_Vfnmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D 9F 10", 5, Code.VEX_Vfnmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 9F 10", 5, Code.VEX_Vfnmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD 9F 10", 5, Code.VEX_Vfnmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub132ssV_VX_HX_WX_2_Data))]
		void Test16_Vfnmsub132ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfnmsub132ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 9F D3", 5, Code.VEX_Vfnmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 9F D3", 5, Code.VEX_Vfnmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub132ssV_VX_HX_WX_1_Data))]
		void Test32_Vfnmsub132ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Vfnmsub132ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 9F 10", 5, Code.VEX_Vfnmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D 9F 10", 5, Code.VEX_Vfnmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 9F 10", 5, Code.VEX_Vfnmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD 9F 10", 5, Code.VEX_Vfnmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub132ssV_VX_HX_WX_2_Data))]
		void Test32_Vfnmsub132ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfnmsub132ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 9F D3", 5, Code.VEX_Vfnmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 9F D3", 5, Code.VEX_Vfnmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub132ssV_VX_HX_WX_1_Data))]
		void Test64_Vfnmsub132ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Vfnmsub132ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 9F 10", 5, Code.VEX_Vfnmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D 9F 10", 5, Code.VEX_Vfnmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 9F 10", 5, Code.VEX_Vfnmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD 9F 10", 5, Code.VEX_Vfnmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub132ssV_VX_HX_WX_2_Data))]
		void Test64_Vfnmsub132ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfnmsub132ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 9F D3", 5, Code.VEX_Vfnmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 9F D3", 5, Code.VEX_Vfnmsub132ss_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 9F D3", 5, Code.VEX_Vfnmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 9F D3", 5, Code.VEX_Vfnmsub132ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2C9 9F D3", 5, Code.VEX_Vfnmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 9F D3", 5, Code.VEX_Vfnmsub132sd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 9F D3", 5, Code.VEX_Vfnmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 9F D3", 5, Code.VEX_Vfnmsub132sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub132ssV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfnmsub132ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfnmsub132ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 9F 50 01", 7, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 9F 50 01", 7, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB 9F 50 01", 7, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB 9F 50 01", 7, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB 9F 50 01", 7, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B 9F 50 01", 7, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 9F 50 01", 7, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB 9F 50 01", 7, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB 9F 50 01", 7, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB 9F 50 01", 7, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub132ssV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfnmsub132ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfnmsub132ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 9F D3", 6, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 9F D3", 6, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 9F D3", 6, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 9F D3", 6, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub132ssV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfnmsub132ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfnmsub132ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 9F 50 01", 7, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 9F 50 01", 7, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB 9F 50 01", 7, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB 9F 50 01", 7, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB 9F 50 01", 7, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B 9F 50 01", 7, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 9F 50 01", 7, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB 9F 50 01", 7, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB 9F 50 01", 7, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB 9F 50 01", 7, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub132ssV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfnmsub132ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfnmsub132ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 9F D3", 6, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B 9F D3", 6, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 9F D3", 6, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B 9F D3", 6, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub132ssV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfnmsub132ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfnmsub132ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 9F 50 01", 7, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 9F 50 01", 7, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB 9F 50 01", 7, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB 9F 50 01", 7, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB 9F 50 01", 7, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B 9F 50 01", 7, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 9F 50 01", 7, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB 9F 50 01", 7, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB 9F 50 01", 7, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB 9F 50 01", 7, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub132ssV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfnmsub132ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfnmsub132ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 9F D3", 6, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B 9F D3", 6, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 9F D3", 6, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B 9F D3", 6, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B 9F D3", 6, Code.EVEX_Vfnmsub132ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B 9F D3", 6, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B 9F D3", 6, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 9F D3", 6, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B 9F D3", 6, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B 9F D3", 6, Code.EVEX_Vfnmsub132sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}
	}
}
