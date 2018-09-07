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
	public sealed class DecoderTest_3_0F38A8_0F38AF : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_Vfmadd213psV_VX_HX_WX_1_Data))]
		void Test16_Vfmadd213psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_Vfmadd213psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 A8 10", 5, Code.VEX_Vfmadd213ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D A8 10", 5, Code.VEX_Vfmadd213ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 A8 10", 5, Code.VEX_Vfmadd213pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD A8 10", 5, Code.VEX_Vfmadd213pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmadd213psV_VX_HX_WX_2_Data))]
		void Test16_Vfmadd213psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfmadd213psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 A8 D3", 5, Code.VEX_Vfmadd213ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D A8 D3", 5, Code.VEX_Vfmadd213ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 A8 D3", 5, Code.VEX_Vfmadd213pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD A8 D3", 5, Code.VEX_Vfmadd213pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd213psV_VX_HX_WX_1_Data))]
		void Test32_Vfmadd213psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_Vfmadd213psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 A8 10", 5, Code.VEX_Vfmadd213ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D A8 10", 5, Code.VEX_Vfmadd213ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 A8 10", 5, Code.VEX_Vfmadd213pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD A8 10", 5, Code.VEX_Vfmadd213pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd213psV_VX_HX_WX_2_Data))]
		void Test32_Vfmadd213psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfmadd213psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 A8 D3", 5, Code.VEX_Vfmadd213ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D A8 D3", 5, Code.VEX_Vfmadd213ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 A8 D3", 5, Code.VEX_Vfmadd213pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD A8 D3", 5, Code.VEX_Vfmadd213pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd213psV_VX_HX_WX_1_Data))]
		void Test64_Vfmadd213psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_Vfmadd213psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 A8 10", 5, Code.VEX_Vfmadd213ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D A8 10", 5, Code.VEX_Vfmadd213ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 A8 10", 5, Code.VEX_Vfmadd213pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD A8 10", 5, Code.VEX_Vfmadd213pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd213psV_VX_HX_WX_2_Data))]
		void Test64_Vfmadd213psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfmadd213psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 A8 D3", 5, Code.VEX_Vfmadd213ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 A8 D3", 5, Code.VEX_Vfmadd213ps_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 A8 D3", 5, Code.VEX_Vfmadd213ps_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 A8 D3", 5, Code.VEX_Vfmadd213ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E24D A8 D3", 5, Code.VEX_Vfmadd213ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D A8 D3", 5, Code.VEX_Vfmadd213ps_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D A8 D3", 5, Code.VEX_Vfmadd213ps_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D A8 D3", 5, Code.VEX_Vfmadd213ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C4E2C9 A8 D3", 5, Code.VEX_Vfmadd213pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 A8 D3", 5, Code.VEX_Vfmadd213pd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 A8 D3", 5, Code.VEX_Vfmadd213pd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 A8 D3", 5, Code.VEX_Vfmadd213pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2CD A8 D3", 5, Code.VEX_Vfmadd213pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C462CD A8 D3", 5, Code.VEX_Vfmadd213pd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E28D A8 D3", 5, Code.VEX_Vfmadd213pd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C2CD A8 D3", 5, Code.VEX_Vfmadd213pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmadd213psV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfmadd213psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmadd213psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B A8 50 01", 7, Code.EVEX_Vfmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D A8 50 01", 7, Code.EVEX_Vfmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 A8 50 01", 7, Code.EVEX_Vfmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B A8 50 01", 7, Code.EVEX_Vfmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD A8 50 01", 7, Code.EVEX_Vfmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 A8 50 01", 7, Code.EVEX_Vfmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B A8 50 01", 7, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD A8 50 01", 7, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 A8 50 01", 7, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B A8 50 01", 7, Code.EVEX_Vfmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D A8 50 01", 7, Code.EVEX_Vfmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 A8 50 01", 7, Code.EVEX_Vfmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B A8 50 01", 7, Code.EVEX_Vfmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD A8 50 01", 7, Code.EVEX_Vfmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 A8 50 01", 7, Code.EVEX_Vfmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B A8 50 01", 7, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD A8 50 01", 7, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 A8 50 01", 7, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmadd213psV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfmadd213psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmadd213psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B A8 D3", 6, Code.EVEX_Vfmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB A8 D3", 6, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB A8 D3", 6, Code.EVEX_Vfmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B A8 D3", 6, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB A8 D3", 6, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B A8 D3", 6, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B A8 D3", 6, Code.EVEX_Vfmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB A8 D3", 6, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB A8 D3", 6, Code.EVEX_Vfmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B A8 D3", 6, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB A8 D3", 6, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B A8 D3", 6, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd213psV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfmadd213psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmadd213psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B A8 50 01", 7, Code.EVEX_Vfmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D A8 50 01", 7, Code.EVEX_Vfmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 A8 50 01", 7, Code.EVEX_Vfmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B A8 50 01", 7, Code.EVEX_Vfmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD A8 50 01", 7, Code.EVEX_Vfmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 A8 50 01", 7, Code.EVEX_Vfmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B A8 50 01", 7, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD A8 50 01", 7, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 A8 50 01", 7, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B A8 50 01", 7, Code.EVEX_Vfmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D A8 50 01", 7, Code.EVEX_Vfmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 A8 50 01", 7, Code.EVEX_Vfmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B A8 50 01", 7, Code.EVEX_Vfmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD A8 50 01", 7, Code.EVEX_Vfmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 A8 50 01", 7, Code.EVEX_Vfmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B A8 50 01", 7, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD A8 50 01", 7, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 A8 50 01", 7, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd213psV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfmadd213psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmadd213psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B A8 D3", 6, Code.EVEX_Vfmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB A8 D3", 6, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB A8 D3", 6, Code.EVEX_Vfmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B A8 D3", 6, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB A8 D3", 6, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B A8 D3", 6, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B A8 D3", 6, Code.EVEX_Vfmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB A8 D3", 6, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB A8 D3", 6, Code.EVEX_Vfmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B A8 D3", 6, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB A8 D3", 6, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B A8 D3", 6, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd213psV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfmadd213psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmadd213psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B A8 50 01", 7, Code.EVEX_Vfmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D A8 50 01", 7, Code.EVEX_Vfmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 A8 50 01", 7, Code.EVEX_Vfmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B A8 50 01", 7, Code.EVEX_Vfmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD A8 50 01", 7, Code.EVEX_Vfmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 A8 50 01", 7, Code.EVEX_Vfmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B A8 50 01", 7, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD A8 50 01", 7, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 A8 50 01", 7, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B A8 50 01", 7, Code.EVEX_Vfmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D A8 50 01", 7, Code.EVEX_Vfmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 A8 50 01", 7, Code.EVEX_Vfmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B A8 50 01", 7, Code.EVEX_Vfmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD A8 50 01", 7, Code.EVEX_Vfmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 A8 50 01", 7, Code.EVEX_Vfmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B A8 50 01", 7, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD A8 50 01", 7, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 A8 50 01", 7, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd213psV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfmadd213psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmadd213psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B A8 D3", 6, Code.EVEX_Vfmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B A8 D3", 6, Code.EVEX_Vfmadd213ps_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 A8 D3", 6, Code.EVEX_Vfmadd213ps_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B A8 D3", 6, Code.EVEX_Vfmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24DDB A8 D3", 6, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB A8 D3", 6, Code.EVEX_Vfmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D2B A8 D3", 6, Code.EVEX_Vfmadd213ps_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D23 A8 D3", 6, Code.EVEX_Vfmadd213ps_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D2B A8 D3", 6, Code.EVEX_Vfmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D1B A8 D3", 6, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB A8 D3", 6, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D4B A8 D3", 6, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D43 A8 D3", 6, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D4B A8 D3", 6, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B A8 D3", 6, Code.EVEX_Vfmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B A8 D3", 6, Code.EVEX_Vfmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B A8 D3", 6, Code.EVEX_Vfmadd213pd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 A8 D3", 6, Code.EVEX_Vfmadd213pd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B A8 D3", 6, Code.EVEX_Vfmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CDDB A8 D3", 6, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB A8 D3", 6, Code.EVEX_Vfmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D2B A8 D3", 6, Code.EVEX_Vfmadd213pd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD23 A8 D3", 6, Code.EVEX_Vfmadd213pd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD2B A8 D3", 6, Code.EVEX_Vfmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD1B A8 D3", 6, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB A8 D3", 6, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D4B A8 D3", 6, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD43 A8 D3", 6, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD4B A8 D3", 6, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B A8 D3", 6, Code.EVEX_Vfmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmadd213ssV_VX_HX_WX_1_Data))]
		void Test16_Vfmadd213ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_Vfmadd213ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 A9 10", 5, Code.VEX_Vfmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D A9 10", 5, Code.VEX_Vfmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 A9 10", 5, Code.VEX_Vfmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD A9 10", 5, Code.VEX_Vfmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmadd213ssV_VX_HX_WX_2_Data))]
		void Test16_Vfmadd213ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfmadd213ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 A9 D3", 5, Code.VEX_Vfmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 A9 D3", 5, Code.VEX_Vfmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd213ssV_VX_HX_WX_1_Data))]
		void Test32_Vfmadd213ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_Vfmadd213ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 A9 10", 5, Code.VEX_Vfmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D A9 10", 5, Code.VEX_Vfmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 A9 10", 5, Code.VEX_Vfmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD A9 10", 5, Code.VEX_Vfmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd213ssV_VX_HX_WX_2_Data))]
		void Test32_Vfmadd213ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfmadd213ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 A9 D3", 5, Code.VEX_Vfmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 A9 D3", 5, Code.VEX_Vfmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd213ssV_VX_HX_WX_1_Data))]
		void Test64_Vfmadd213ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_Vfmadd213ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 A9 10", 5, Code.VEX_Vfmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D A9 10", 5, Code.VEX_Vfmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 A9 10", 5, Code.VEX_Vfmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD A9 10", 5, Code.VEX_Vfmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd213ssV_VX_HX_WX_2_Data))]
		void Test64_Vfmadd213ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfmadd213ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 A9 D3", 5, Code.VEX_Vfmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 A9 D3", 5, Code.VEX_Vfmadd213ss_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 A9 D3", 5, Code.VEX_Vfmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 A9 D3", 5, Code.VEX_Vfmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2C9 A9 D3", 5, Code.VEX_Vfmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 A9 D3", 5, Code.VEX_Vfmadd213sd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 A9 D3", 5, Code.VEX_Vfmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 A9 D3", 5, Code.VEX_Vfmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmadd213ssV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfmadd213ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmadd213ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B A9 50 01", 7, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 A9 50 01", 7, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB A9 50 01", 7, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB A9 50 01", 7, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB A9 50 01", 7, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B A9 50 01", 7, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 A9 50 01", 7, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB A9 50 01", 7, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB A9 50 01", 7, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB A9 50 01", 7, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmadd213ssV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfmadd213ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmadd213ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B A9 D3", 6, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B A9 D3", 6, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B A9 D3", 6, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B A9 D3", 6, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd213ssV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfmadd213ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmadd213ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B A9 50 01", 7, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 A9 50 01", 7, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB A9 50 01", 7, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB A9 50 01", 7, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB A9 50 01", 7, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B A9 50 01", 7, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 A9 50 01", 7, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB A9 50 01", 7, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB A9 50 01", 7, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB A9 50 01", 7, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmadd213ssV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfmadd213ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmadd213ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B A9 D3", 6, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B A9 D3", 6, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B A9 D3", 6, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B A9 D3", 6, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd213ssV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfmadd213ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmadd213ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B A9 50 01", 7, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 A9 50 01", 7, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB A9 50 01", 7, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB A9 50 01", 7, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB A9 50 01", 7, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B A9 50 01", 7, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 A9 50 01", 7, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB A9 50 01", 7, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB A9 50 01", 7, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB A9 50 01", 7, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmadd213ssV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfmadd213ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmadd213ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B A9 D3", 6, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B A9 D3", 6, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 A9 D3", 6, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B A9 D3", 6, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B A9 D3", 6, Code.EVEX_Vfmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B A9 D3", 6, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B A9 D3", 6, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 A9 D3", 6, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B A9 D3", 6, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B A9 D3", 6, Code.EVEX_Vfmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub213psV_VX_HX_WX_1_Data))]
		void Test16_Vfmsub213psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_Vfmsub213psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 AA 10", 5, Code.VEX_Vfmsub213ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D AA 10", 5, Code.VEX_Vfmsub213ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 AA 10", 5, Code.VEX_Vfmsub213pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD AA 10", 5, Code.VEX_Vfmsub213pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub213psV_VX_HX_WX_2_Data))]
		void Test16_Vfmsub213psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfmsub213psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 AA D3", 5, Code.VEX_Vfmsub213ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D AA D3", 5, Code.VEX_Vfmsub213ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 AA D3", 5, Code.VEX_Vfmsub213pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD AA D3", 5, Code.VEX_Vfmsub213pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub213psV_VX_HX_WX_1_Data))]
		void Test32_Vfmsub213psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_Vfmsub213psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 AA 10", 5, Code.VEX_Vfmsub213ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D AA 10", 5, Code.VEX_Vfmsub213ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 AA 10", 5, Code.VEX_Vfmsub213pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD AA 10", 5, Code.VEX_Vfmsub213pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub213psV_VX_HX_WX_2_Data))]
		void Test32_Vfmsub213psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfmsub213psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 AA D3", 5, Code.VEX_Vfmsub213ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D AA D3", 5, Code.VEX_Vfmsub213ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 AA D3", 5, Code.VEX_Vfmsub213pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD AA D3", 5, Code.VEX_Vfmsub213pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub213psV_VX_HX_WX_1_Data))]
		void Test64_Vfmsub213psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_Vfmsub213psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 AA 10", 5, Code.VEX_Vfmsub213ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D AA 10", 5, Code.VEX_Vfmsub213ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 AA 10", 5, Code.VEX_Vfmsub213pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD AA 10", 5, Code.VEX_Vfmsub213pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub213psV_VX_HX_WX_2_Data))]
		void Test64_Vfmsub213psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfmsub213psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 AA D3", 5, Code.VEX_Vfmsub213ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 AA D3", 5, Code.VEX_Vfmsub213ps_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 AA D3", 5, Code.VEX_Vfmsub213ps_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 AA D3", 5, Code.VEX_Vfmsub213ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E24D AA D3", 5, Code.VEX_Vfmsub213ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D AA D3", 5, Code.VEX_Vfmsub213ps_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D AA D3", 5, Code.VEX_Vfmsub213ps_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D AA D3", 5, Code.VEX_Vfmsub213ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C4E2C9 AA D3", 5, Code.VEX_Vfmsub213pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 AA D3", 5, Code.VEX_Vfmsub213pd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 AA D3", 5, Code.VEX_Vfmsub213pd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 AA D3", 5, Code.VEX_Vfmsub213pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2CD AA D3", 5, Code.VEX_Vfmsub213pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C462CD AA D3", 5, Code.VEX_Vfmsub213pd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E28D AA D3", 5, Code.VEX_Vfmsub213pd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C2CD AA D3", 5, Code.VEX_Vfmsub213pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub213psV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfmsub213psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmsub213psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B AA 50 01", 7, Code.EVEX_Vfmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D AA 50 01", 7, Code.EVEX_Vfmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 AA 50 01", 7, Code.EVEX_Vfmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B AA 50 01", 7, Code.EVEX_Vfmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD AA 50 01", 7, Code.EVEX_Vfmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 AA 50 01", 7, Code.EVEX_Vfmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B AA 50 01", 7, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD AA 50 01", 7, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 AA 50 01", 7, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B AA 50 01", 7, Code.EVEX_Vfmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D AA 50 01", 7, Code.EVEX_Vfmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 AA 50 01", 7, Code.EVEX_Vfmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B AA 50 01", 7, Code.EVEX_Vfmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD AA 50 01", 7, Code.EVEX_Vfmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 AA 50 01", 7, Code.EVEX_Vfmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B AA 50 01", 7, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD AA 50 01", 7, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 AA 50 01", 7, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub213psV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfmsub213psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmsub213psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B AA D3", 6, Code.EVEX_Vfmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB AA D3", 6, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB AA D3", 6, Code.EVEX_Vfmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B AA D3", 6, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB AA D3", 6, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B AA D3", 6, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B AA D3", 6, Code.EVEX_Vfmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB AA D3", 6, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB AA D3", 6, Code.EVEX_Vfmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B AA D3", 6, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB AA D3", 6, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B AA D3", 6, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub213psV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfmsub213psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmsub213psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B AA 50 01", 7, Code.EVEX_Vfmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D AA 50 01", 7, Code.EVEX_Vfmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 AA 50 01", 7, Code.EVEX_Vfmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B AA 50 01", 7, Code.EVEX_Vfmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD AA 50 01", 7, Code.EVEX_Vfmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 AA 50 01", 7, Code.EVEX_Vfmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B AA 50 01", 7, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD AA 50 01", 7, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 AA 50 01", 7, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B AA 50 01", 7, Code.EVEX_Vfmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D AA 50 01", 7, Code.EVEX_Vfmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 AA 50 01", 7, Code.EVEX_Vfmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B AA 50 01", 7, Code.EVEX_Vfmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD AA 50 01", 7, Code.EVEX_Vfmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 AA 50 01", 7, Code.EVEX_Vfmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B AA 50 01", 7, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD AA 50 01", 7, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 AA 50 01", 7, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub213psV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfmsub213psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmsub213psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B AA D3", 6, Code.EVEX_Vfmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB AA D3", 6, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB AA D3", 6, Code.EVEX_Vfmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B AA D3", 6, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB AA D3", 6, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B AA D3", 6, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B AA D3", 6, Code.EVEX_Vfmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB AA D3", 6, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB AA D3", 6, Code.EVEX_Vfmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B AA D3", 6, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB AA D3", 6, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B AA D3", 6, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub213psV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfmsub213psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmsub213psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B AA 50 01", 7, Code.EVEX_Vfmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D AA 50 01", 7, Code.EVEX_Vfmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 AA 50 01", 7, Code.EVEX_Vfmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B AA 50 01", 7, Code.EVEX_Vfmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD AA 50 01", 7, Code.EVEX_Vfmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 AA 50 01", 7, Code.EVEX_Vfmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B AA 50 01", 7, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD AA 50 01", 7, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 AA 50 01", 7, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B AA 50 01", 7, Code.EVEX_Vfmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D AA 50 01", 7, Code.EVEX_Vfmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 AA 50 01", 7, Code.EVEX_Vfmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B AA 50 01", 7, Code.EVEX_Vfmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD AA 50 01", 7, Code.EVEX_Vfmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 AA 50 01", 7, Code.EVEX_Vfmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B AA 50 01", 7, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD AA 50 01", 7, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 AA 50 01", 7, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub213psV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfmsub213psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmsub213psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B AA D3", 6, Code.EVEX_Vfmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B AA D3", 6, Code.EVEX_Vfmsub213ps_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 AA D3", 6, Code.EVEX_Vfmsub213ps_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B AA D3", 6, Code.EVEX_Vfmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24DDB AA D3", 6, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB AA D3", 6, Code.EVEX_Vfmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D2B AA D3", 6, Code.EVEX_Vfmsub213ps_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D23 AA D3", 6, Code.EVEX_Vfmsub213ps_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D2B AA D3", 6, Code.EVEX_Vfmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D1B AA D3", 6, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB AA D3", 6, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D4B AA D3", 6, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D43 AA D3", 6, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D4B AA D3", 6, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B AA D3", 6, Code.EVEX_Vfmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B AA D3", 6, Code.EVEX_Vfmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B AA D3", 6, Code.EVEX_Vfmsub213pd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 AA D3", 6, Code.EVEX_Vfmsub213pd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B AA D3", 6, Code.EVEX_Vfmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CDDB AA D3", 6, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB AA D3", 6, Code.EVEX_Vfmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D2B AA D3", 6, Code.EVEX_Vfmsub213pd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD23 AA D3", 6, Code.EVEX_Vfmsub213pd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD2B AA D3", 6, Code.EVEX_Vfmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD1B AA D3", 6, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB AA D3", 6, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D4B AA D3", 6, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD43 AA D3", 6, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD4B AA D3", 6, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B AA D3", 6, Code.EVEX_Vfmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_V4fnmaddpsV_VX_k1z_HX_WX_1_Data))]
		void Test16_V4fnmaddpsV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_V4fnmaddpsV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F4B AA 50 01", 7, Code.EVEX_V4fnmaddps_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24FCB AA 50 01", 7, Code.EVEX_V4fnmaddps_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Float32, 16, true };
				yield return new object[] { "62 F24F48 AA 50 01", 7, Code.EVEX_V4fnmaddps_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Float32, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_V4fnmaddpsV_VX_k1z_HX_WX_1_Data))]
		void Test32_V4fnmaddpsV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_V4fnmaddpsV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F4B AA 50 01", 7, Code.EVEX_V4fnmaddps_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24FCB AA 50 01", 7, Code.EVEX_V4fnmaddps_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Float32, 16, true };
				yield return new object[] { "62 F24F48 AA 50 01", 7, Code.EVEX_V4fnmaddps_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Float32, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_V4fnmaddpsV_VX_k1z_HX_WX_1_Data))]
		void Test64_V4fnmaddpsV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_V4fnmaddpsV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F4B AA 50 01", 7, Code.EVEX_V4fnmaddps_VZ_k1z_HZP3_M, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 E20FCB AA 50 01", 7, Code.EVEX_V4fnmaddps_VZ_k1z_HZP3_M, Register.ZMM18, Register.ZMM14, Register.K3, MemorySize.Packed128_Float32, 16, true };
				yield return new object[] { "62 724F40 AA 50 01", 7, Code.EVEX_V4fnmaddps_VZ_k1z_HZP3_M, Register.ZMM10, Register.ZMM22, Register.None, MemorySize.Packed128_Float32, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub213ssV_VX_HX_WX_1_Data))]
		void Test16_Vfmsub213ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_Vfmsub213ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 AB 10", 5, Code.VEX_Vfmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D AB 10", 5, Code.VEX_Vfmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 AB 10", 5, Code.VEX_Vfmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD AB 10", 5, Code.VEX_Vfmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub213ssV_VX_HX_WX_2_Data))]
		void Test16_Vfmsub213ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfmsub213ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 AB D3", 5, Code.VEX_Vfmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 AB D3", 5, Code.VEX_Vfmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub213ssV_VX_HX_WX_1_Data))]
		void Test32_Vfmsub213ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_Vfmsub213ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 AB 10", 5, Code.VEX_Vfmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D AB 10", 5, Code.VEX_Vfmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 AB 10", 5, Code.VEX_Vfmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD AB 10", 5, Code.VEX_Vfmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub213ssV_VX_HX_WX_2_Data))]
		void Test32_Vfmsub213ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfmsub213ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 AB D3", 5, Code.VEX_Vfmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 AB D3", 5, Code.VEX_Vfmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub213ssV_VX_HX_WX_1_Data))]
		void Test64_Vfmsub213ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_Vfmsub213ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 AB 10", 5, Code.VEX_Vfmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D AB 10", 5, Code.VEX_Vfmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 AB 10", 5, Code.VEX_Vfmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD AB 10", 5, Code.VEX_Vfmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub213ssV_VX_HX_WX_2_Data))]
		void Test64_Vfmsub213ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfmsub213ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 AB D3", 5, Code.VEX_Vfmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 AB D3", 5, Code.VEX_Vfmsub213ss_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 AB D3", 5, Code.VEX_Vfmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 AB D3", 5, Code.VEX_Vfmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2C9 AB D3", 5, Code.VEX_Vfmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 AB D3", 5, Code.VEX_Vfmsub213sd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 AB D3", 5, Code.VEX_Vfmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 AB D3", 5, Code.VEX_Vfmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub213ssV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfmsub213ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmsub213ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B AB 50 01", 7, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 AB 50 01", 7, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB AB 50 01", 7, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB AB 50 01", 7, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB AB 50 01", 7, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B AB 50 01", 7, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 AB 50 01", 7, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB AB 50 01", 7, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB AB 50 01", 7, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB AB 50 01", 7, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfmsub213ssV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfmsub213ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfmsub213ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B AB D3", 6, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B AB D3", 6, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B AB D3", 6, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B AB D3", 6, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub213ssV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfmsub213ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmsub213ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B AB 50 01", 7, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 AB 50 01", 7, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB AB 50 01", 7, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB AB 50 01", 7, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB AB 50 01", 7, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B AB 50 01", 7, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 AB 50 01", 7, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB AB 50 01", 7, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB AB 50 01", 7, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB AB 50 01", 7, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfmsub213ssV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfmsub213ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfmsub213ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B AB D3", 6, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B AB D3", 6, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B AB D3", 6, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B AB D3", 6, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub213ssV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfmsub213ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmsub213ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B AB 50 01", 7, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 AB 50 01", 7, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB AB 50 01", 7, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB AB 50 01", 7, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB AB 50 01", 7, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B AB 50 01", 7, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 AB 50 01", 7, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB AB 50 01", 7, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB AB 50 01", 7, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB AB 50 01", 7, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfmsub213ssV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfmsub213ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfmsub213ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B AB D3", 6, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B AB D3", 6, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 AB D3", 6, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B AB D3", 6, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B AB D3", 6, Code.EVEX_Vfmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B AB D3", 6, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B AB D3", 6, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 AB D3", 6, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B AB D3", 6, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B AB D3", 6, Code.EVEX_Vfmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_V4fnmaddssV_VX_k1z_HX_WX_1_Data))]
		void Test16_V4fnmaddssV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_V4fnmaddssV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F0B AB 50 01", 7, Code.EVEX_V4fnmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F8B AB 50 01", 7, Code.EVEX_V4fnmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, true };
				yield return new object[] { "62 F24F08 AB 50 01", 7, Code.EVEX_V4fnmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F2B AB 50 01", 7, Code.EVEX_V4fnmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F4B AB 50 01", 7, Code.EVEX_V4fnmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F6B AB 50 01", 7, Code.EVEX_V4fnmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_V4fnmaddssV_VX_k1z_HX_WX_1_Data))]
		void Test32_V4fnmaddssV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_V4fnmaddssV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F0B AB 50 01", 7, Code.EVEX_V4fnmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F8B AB 50 01", 7, Code.EVEX_V4fnmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, true };
				yield return new object[] { "62 F24F08 AB 50 01", 7, Code.EVEX_V4fnmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F2B AB 50 01", 7, Code.EVEX_V4fnmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F4B AB 50 01", 7, Code.EVEX_V4fnmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F6B AB 50 01", 7, Code.EVEX_V4fnmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_V4fnmaddssV_VX_k1z_HX_WX_1_Data))]
		void Test64_V4fnmaddssV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_V4fnmaddssV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24F0B AB 50 01", 7, Code.EVEX_V4fnmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 E20F8B AB 50 01", 7, Code.EVEX_V4fnmaddss_VX_k1z_HXP3_M, Register.XMM18, Register.XMM14, Register.K3, MemorySize.Packed128_Float32, 16, true };
				yield return new object[] { "62 724F00 AB 50 01", 7, Code.EVEX_V4fnmaddss_VX_k1z_HXP3_M, Register.XMM10, Register.XMM22, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F2B AB 50 01", 7, Code.EVEX_V4fnmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F4B AB 50 01", 7, Code.EVEX_V4fnmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24F6B AB 50 01", 7, Code.EVEX_V4fnmaddss_VX_k1z_HXP3_M, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd213psV_VX_HX_WX_1_Data))]
		void Test16_Vfnmadd213psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_Vfnmadd213psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 AC 10", 5, Code.VEX_Vfnmadd213ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D AC 10", 5, Code.VEX_Vfnmadd213ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 AC 10", 5, Code.VEX_Vfnmadd213pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD AC 10", 5, Code.VEX_Vfnmadd213pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd213psV_VX_HX_WX_2_Data))]
		void Test16_Vfnmadd213psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfnmadd213psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 AC D3", 5, Code.VEX_Vfnmadd213ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D AC D3", 5, Code.VEX_Vfnmadd213ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 AC D3", 5, Code.VEX_Vfnmadd213pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD AC D3", 5, Code.VEX_Vfnmadd213pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd213psV_VX_HX_WX_1_Data))]
		void Test32_Vfnmadd213psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_Vfnmadd213psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 AC 10", 5, Code.VEX_Vfnmadd213ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D AC 10", 5, Code.VEX_Vfnmadd213ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 AC 10", 5, Code.VEX_Vfnmadd213pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD AC 10", 5, Code.VEX_Vfnmadd213pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd213psV_VX_HX_WX_2_Data))]
		void Test32_Vfnmadd213psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfnmadd213psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 AC D3", 5, Code.VEX_Vfnmadd213ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D AC D3", 5, Code.VEX_Vfnmadd213ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 AC D3", 5, Code.VEX_Vfnmadd213pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD AC D3", 5, Code.VEX_Vfnmadd213pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd213psV_VX_HX_WX_1_Data))]
		void Test64_Vfnmadd213psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_Vfnmadd213psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 AC 10", 5, Code.VEX_Vfnmadd213ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D AC 10", 5, Code.VEX_Vfnmadd213ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 AC 10", 5, Code.VEX_Vfnmadd213pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD AC 10", 5, Code.VEX_Vfnmadd213pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd213psV_VX_HX_WX_2_Data))]
		void Test64_Vfnmadd213psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfnmadd213psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 AC D3", 5, Code.VEX_Vfnmadd213ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 AC D3", 5, Code.VEX_Vfnmadd213ps_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 AC D3", 5, Code.VEX_Vfnmadd213ps_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 AC D3", 5, Code.VEX_Vfnmadd213ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E24D AC D3", 5, Code.VEX_Vfnmadd213ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D AC D3", 5, Code.VEX_Vfnmadd213ps_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D AC D3", 5, Code.VEX_Vfnmadd213ps_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D AC D3", 5, Code.VEX_Vfnmadd213ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C4E2C9 AC D3", 5, Code.VEX_Vfnmadd213pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 AC D3", 5, Code.VEX_Vfnmadd213pd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 AC D3", 5, Code.VEX_Vfnmadd213pd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 AC D3", 5, Code.VEX_Vfnmadd213pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2CD AC D3", 5, Code.VEX_Vfnmadd213pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C462CD AC D3", 5, Code.VEX_Vfnmadd213pd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E28D AC D3", 5, Code.VEX_Vfnmadd213pd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C2CD AC D3", 5, Code.VEX_Vfnmadd213pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd213psV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfnmadd213psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfnmadd213psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd213psV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfnmadd213psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfnmadd213psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B AC D3", 6, Code.EVEX_Vfnmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB AC D3", 6, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB AC D3", 6, Code.EVEX_Vfnmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B AC D3", 6, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB AC D3", 6, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B AC D3", 6, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B AC D3", 6, Code.EVEX_Vfnmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB AC D3", 6, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB AC D3", 6, Code.EVEX_Vfnmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B AC D3", 6, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB AC D3", 6, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B AC D3", 6, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd213psV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfnmadd213psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfnmadd213psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd213psV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfnmadd213psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfnmadd213psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B AC D3", 6, Code.EVEX_Vfnmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB AC D3", 6, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB AC D3", 6, Code.EVEX_Vfnmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B AC D3", 6, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB AC D3", 6, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B AC D3", 6, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B AC D3", 6, Code.EVEX_Vfnmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB AC D3", 6, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB AC D3", 6, Code.EVEX_Vfnmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B AC D3", 6, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB AC D3", 6, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B AC D3", 6, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd213psV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfnmadd213psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfnmadd213psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 AC 50 01", 7, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 AC 50 01", 7, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd213psV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfnmadd213psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfnmadd213psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B AC D3", 6, Code.EVEX_Vfnmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B AC D3", 6, Code.EVEX_Vfnmadd213ps_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 AC D3", 6, Code.EVEX_Vfnmadd213ps_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B AC D3", 6, Code.EVEX_Vfnmadd213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24DDB AC D3", 6, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB AC D3", 6, Code.EVEX_Vfnmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D2B AC D3", 6, Code.EVEX_Vfnmadd213ps_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D23 AC D3", 6, Code.EVEX_Vfnmadd213ps_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D2B AC D3", 6, Code.EVEX_Vfnmadd213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D1B AC D3", 6, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB AC D3", 6, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D4B AC D3", 6, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D43 AC D3", 6, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D4B AC D3", 6, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B AC D3", 6, Code.EVEX_Vfnmadd213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B AC D3", 6, Code.EVEX_Vfnmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B AC D3", 6, Code.EVEX_Vfnmadd213pd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 AC D3", 6, Code.EVEX_Vfnmadd213pd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B AC D3", 6, Code.EVEX_Vfnmadd213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CDDB AC D3", 6, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB AC D3", 6, Code.EVEX_Vfnmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D2B AC D3", 6, Code.EVEX_Vfnmadd213pd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD23 AC D3", 6, Code.EVEX_Vfnmadd213pd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD2B AC D3", 6, Code.EVEX_Vfnmadd213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD1B AC D3", 6, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB AC D3", 6, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D4B AC D3", 6, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD43 AC D3", 6, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD4B AC D3", 6, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B AC D3", 6, Code.EVEX_Vfnmadd213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd213ssV_VX_HX_WX_1_Data))]
		void Test16_Vfnmadd213ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_Vfnmadd213ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 AD 10", 5, Code.VEX_Vfnmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D AD 10", 5, Code.VEX_Vfnmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 AD 10", 5, Code.VEX_Vfnmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD AD 10", 5, Code.VEX_Vfnmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd213ssV_VX_HX_WX_2_Data))]
		void Test16_Vfnmadd213ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfnmadd213ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 AD D3", 5, Code.VEX_Vfnmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 AD D3", 5, Code.VEX_Vfnmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd213ssV_VX_HX_WX_1_Data))]
		void Test32_Vfnmadd213ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_Vfnmadd213ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 AD 10", 5, Code.VEX_Vfnmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D AD 10", 5, Code.VEX_Vfnmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 AD 10", 5, Code.VEX_Vfnmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD AD 10", 5, Code.VEX_Vfnmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd213ssV_VX_HX_WX_2_Data))]
		void Test32_Vfnmadd213ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfnmadd213ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 AD D3", 5, Code.VEX_Vfnmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 AD D3", 5, Code.VEX_Vfnmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd213ssV_VX_HX_WX_1_Data))]
		void Test64_Vfnmadd213ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_Vfnmadd213ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 AD 10", 5, Code.VEX_Vfnmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D AD 10", 5, Code.VEX_Vfnmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 AD 10", 5, Code.VEX_Vfnmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD AD 10", 5, Code.VEX_Vfnmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd213ssV_VX_HX_WX_2_Data))]
		void Test64_Vfnmadd213ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfnmadd213ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 AD D3", 5, Code.VEX_Vfnmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 AD D3", 5, Code.VEX_Vfnmadd213ss_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 AD D3", 5, Code.VEX_Vfnmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 AD D3", 5, Code.VEX_Vfnmadd213ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2C9 AD D3", 5, Code.VEX_Vfnmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 AD D3", 5, Code.VEX_Vfnmadd213sd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 AD D3", 5, Code.VEX_Vfnmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 AD D3", 5, Code.VEX_Vfnmadd213sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd213ssV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfnmadd213ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfnmadd213ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B AD 50 01", 7, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 AD 50 01", 7, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB AD 50 01", 7, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB AD 50 01", 7, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB AD 50 01", 7, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B AD 50 01", 7, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 AD 50 01", 7, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB AD 50 01", 7, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB AD 50 01", 7, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB AD 50 01", 7, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmadd213ssV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfnmadd213ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfnmadd213ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B AD D3", 6, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B AD D3", 6, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B AD D3", 6, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B AD D3", 6, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd213ssV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfnmadd213ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfnmadd213ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B AD 50 01", 7, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 AD 50 01", 7, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB AD 50 01", 7, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB AD 50 01", 7, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB AD 50 01", 7, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B AD 50 01", 7, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 AD 50 01", 7, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB AD 50 01", 7, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB AD 50 01", 7, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB AD 50 01", 7, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmadd213ssV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfnmadd213ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfnmadd213ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B AD D3", 6, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B AD D3", 6, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B AD D3", 6, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B AD D3", 6, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd213ssV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfnmadd213ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfnmadd213ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B AD 50 01", 7, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 AD 50 01", 7, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB AD 50 01", 7, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB AD 50 01", 7, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB AD 50 01", 7, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B AD 50 01", 7, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 AD 50 01", 7, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB AD 50 01", 7, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB AD 50 01", 7, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB AD 50 01", 7, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmadd213ssV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfnmadd213ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfnmadd213ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B AD D3", 6, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B AD D3", 6, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 AD D3", 6, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B AD D3", 6, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B AD D3", 6, Code.EVEX_Vfnmadd213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B AD D3", 6, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B AD D3", 6, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 AD D3", 6, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B AD D3", 6, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B AD D3", 6, Code.EVEX_Vfnmadd213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub213psV_VX_HX_WX_1_Data))]
		void Test16_Vfnmsub213psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_Vfnmsub213psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 AE 10", 5, Code.VEX_Vfnmsub213ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D AE 10", 5, Code.VEX_Vfnmsub213ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 AE 10", 5, Code.VEX_Vfnmsub213pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD AE 10", 5, Code.VEX_Vfnmsub213pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub213psV_VX_HX_WX_2_Data))]
		void Test16_Vfnmsub213psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfnmsub213psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 AE D3", 5, Code.VEX_Vfnmsub213ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D AE D3", 5, Code.VEX_Vfnmsub213ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 AE D3", 5, Code.VEX_Vfnmsub213pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD AE D3", 5, Code.VEX_Vfnmsub213pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub213psV_VX_HX_WX_1_Data))]
		void Test32_Vfnmsub213psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_Vfnmsub213psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 AE 10", 5, Code.VEX_Vfnmsub213ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D AE 10", 5, Code.VEX_Vfnmsub213ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 AE 10", 5, Code.VEX_Vfnmsub213pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD AE 10", 5, Code.VEX_Vfnmsub213pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub213psV_VX_HX_WX_2_Data))]
		void Test32_Vfnmsub213psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfnmsub213psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 AE D3", 5, Code.VEX_Vfnmsub213ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E24D AE D3", 5, Code.VEX_Vfnmsub213ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 AE D3", 5, Code.VEX_Vfnmsub213pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2CD AE D3", 5, Code.VEX_Vfnmsub213pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub213psV_VX_HX_WX_1_Data))]
		void Test64_Vfnmsub213psV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_Vfnmsub213psV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 AE 10", 5, Code.VEX_Vfnmsub213ps_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32 };

				yield return new object[] { "C4E24D AE 10", 5, Code.VEX_Vfnmsub213ps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };

				yield return new object[] { "C4E2C9 AE 10", 5, Code.VEX_Vfnmsub213pd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64 };

				yield return new object[] { "C4E2CD AE 10", 5, Code.VEX_Vfnmsub213pd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub213psV_VX_HX_WX_2_Data))]
		void Test64_Vfnmsub213psV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfnmsub213psV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 AE D3", 5, Code.VEX_Vfnmsub213ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 AE D3", 5, Code.VEX_Vfnmsub213ps_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 AE D3", 5, Code.VEX_Vfnmsub213ps_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 AE D3", 5, Code.VEX_Vfnmsub213ps_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E24D AE D3", 5, Code.VEX_Vfnmsub213ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D AE D3", 5, Code.VEX_Vfnmsub213ps_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D AE D3", 5, Code.VEX_Vfnmsub213ps_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D AE D3", 5, Code.VEX_Vfnmsub213ps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C4E2C9 AE D3", 5, Code.VEX_Vfnmsub213pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 AE D3", 5, Code.VEX_Vfnmsub213pd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 AE D3", 5, Code.VEX_Vfnmsub213pd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 AE D3", 5, Code.VEX_Vfnmsub213pd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2CD AE D3", 5, Code.VEX_Vfnmsub213pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C462CD AE D3", 5, Code.VEX_Vfnmsub213pd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E28D AE D3", 5, Code.VEX_Vfnmsub213pd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C2CD AE D3", 5, Code.VEX_Vfnmsub213pd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub213psV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfnmsub213psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfnmsub213psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub213psV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfnmsub213psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfnmsub213psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B AE D3", 6, Code.EVEX_Vfnmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB AE D3", 6, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB AE D3", 6, Code.EVEX_Vfnmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B AE D3", 6, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB AE D3", 6, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B AE D3", 6, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B AE D3", 6, Code.EVEX_Vfnmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB AE D3", 6, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB AE D3", 6, Code.EVEX_Vfnmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B AE D3", 6, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB AE D3", 6, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B AE D3", 6, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub213psV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfnmsub213psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfnmsub213psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub213psV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfnmsub213psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfnmsub213psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B AE D3", 6, Code.EVEX_Vfnmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24DDB AE D3", 6, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB AE D3", 6, Code.EVEX_Vfnmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D1B AE D3", 6, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB AE D3", 6, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B AE D3", 6, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B AE D3", 6, Code.EVEX_Vfnmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CDDB AE D3", 6, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB AE D3", 6, Code.EVEX_Vfnmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD1B AE D3", 6, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB AE D3", 6, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B AE D3", 6, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub213psV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfnmsub213psV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfnmsub213psV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F24D9D AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float32, 4, true };
				yield return new object[] { "62 F24D08 AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float32, 16, false };

				yield return new object[] { "62 F24D2B AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 AE 50 01", 7, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD0B AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2CD9D AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Float64, 8, true };
				yield return new object[] { "62 F2CD08 AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Float64, 16, false };

				yield return new object[] { "62 F2CD2B AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 AE 50 01", 7, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub213psV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfnmsub213psV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfnmsub213psV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B AE D3", 6, Code.EVEX_Vfnmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B AE D3", 6, Code.EVEX_Vfnmsub213ps_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 AE D3", 6, Code.EVEX_Vfnmsub213ps_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B AE D3", 6, Code.EVEX_Vfnmsub213ps_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24DDB AE D3", 6, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F24DAB AE D3", 6, Code.EVEX_Vfnmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D2B AE D3", 6, Code.EVEX_Vfnmsub213ps_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D23 AE D3", 6, Code.EVEX_Vfnmsub213ps_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D2B AE D3", 6, Code.EVEX_Vfnmsub213ps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D1B AE D3", 6, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F24DCB AE D3", 6, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D4B AE D3", 6, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D43 AE D3", 6, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D4B AE D3", 6, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B AE D3", 6, Code.EVEX_Vfnmsub213ps_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B AE D3", 6, Code.EVEX_Vfnmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B AE D3", 6, Code.EVEX_Vfnmsub213pd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 AE D3", 6, Code.EVEX_Vfnmsub213pd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B AE D3", 6, Code.EVEX_Vfnmsub213pd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CDDB AE D3", 6, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true };

				yield return new object[] { "62 F2CDAB AE D3", 6, Code.EVEX_Vfnmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D2B AE D3", 6, Code.EVEX_Vfnmsub213pd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD23 AE D3", 6, Code.EVEX_Vfnmsub213pd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD2B AE D3", 6, Code.EVEX_Vfnmsub213pd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD1B AE D3", 6, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, false };

				yield return new object[] { "62 F2CDCB AE D3", 6, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D4B AE D3", 6, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD43 AE D3", 6, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD4B AE D3", 6, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B AE D3", 6, Code.EVEX_Vfnmsub213pd_VZ_k1z_HZ_WZ_er_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub213ssV_VX_HX_WX_1_Data))]
		void Test16_Vfnmsub213ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_Vfnmsub213ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 AF 10", 5, Code.VEX_Vfnmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D AF 10", 5, Code.VEX_Vfnmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 AF 10", 5, Code.VEX_Vfnmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD AF 10", 5, Code.VEX_Vfnmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub213ssV_VX_HX_WX_2_Data))]
		void Test16_Vfnmsub213ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_Vfnmsub213ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 AF D3", 5, Code.VEX_Vfnmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 AF D3", 5, Code.VEX_Vfnmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub213ssV_VX_HX_WX_1_Data))]
		void Test32_Vfnmsub213ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_Vfnmsub213ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 AF 10", 5, Code.VEX_Vfnmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D AF 10", 5, Code.VEX_Vfnmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 AF 10", 5, Code.VEX_Vfnmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD AF 10", 5, Code.VEX_Vfnmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub213ssV_VX_HX_WX_2_Data))]
		void Test32_Vfnmsub213ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_Vfnmsub213ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 AF D3", 5, Code.VEX_Vfnmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };

				yield return new object[] { "C4E2C9 AF D3", 5, Code.VEX_Vfnmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub213ssV_VX_HX_WX_1_Data))]
		void Test64_Vfnmsub213ssV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_Vfnmsub213ssV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 AF 10", 5, Code.VEX_Vfnmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };
				yield return new object[] { "C4E24D AF 10", 5, Code.VEX_Vfnmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float32 };

				yield return new object[] { "C4E2C9 AF 10", 5, Code.VEX_Vfnmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
				yield return new object[] { "C4E2CD AF 10", 5, Code.VEX_Vfnmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub213ssV_VX_HX_WX_2_Data))]
		void Test64_Vfnmsub213ssV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_Vfnmsub213ssV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 AF D3", 5, Code.VEX_Vfnmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C46249 AF D3", 5, Code.VEX_Vfnmsub213ss_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E209 AF D3", 5, Code.VEX_Vfnmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C249 AF D3", 5, Code.VEX_Vfnmsub213ss_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };

				yield return new object[] { "C4E2C9 AF D3", 5, Code.VEX_Vfnmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462C9 AF D3", 5, Code.VEX_Vfnmsub213sd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E289 AF D3", 5, Code.VEX_Vfnmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4C2C9 AF D3", 5, Code.VEX_Vfnmsub213sd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub213ssV_VX_k1_HX_WX_1_Data))]
		void Test16_Vfnmsub213ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfnmsub213ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B AF 50 01", 7, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 AF 50 01", 7, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB AF 50 01", 7, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB AF 50 01", 7, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB AF 50 01", 7, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B AF 50 01", 7, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 AF 50 01", 7, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB AF 50 01", 7, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB AF 50 01", 7, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB AF 50 01", 7, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vfnmsub213ssV_VX_k1_HX_WX_2_Data))]
		void Test16_Vfnmsub213ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test16_Vfnmsub213ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B AF D3", 6, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B AF D3", 6, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B AF D3", 6, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B AF D3", 6, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub213ssV_VX_k1_HX_WX_1_Data))]
		void Test32_Vfnmsub213ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfnmsub213ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B AF 50 01", 7, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 AF 50 01", 7, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB AF 50 01", 7, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB AF 50 01", 7, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB AF 50 01", 7, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B AF 50 01", 7, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 AF 50 01", 7, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB AF 50 01", 7, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB AF 50 01", 7, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB AF 50 01", 7, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vfnmsub213ssV_VX_k1_HX_WX_2_Data))]
		void Test32_Vfnmsub213ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test32_Vfnmsub213ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B AF D3", 6, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F24D7B AF D3", 6, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B AF D3", 6, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 F2CD7B AF D3", 6, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub213ssV_VX_k1_HX_WX_1_Data))]
		void Test64_Vfnmsub213ssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfnmsub213ssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B AF 50 01", 7, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D08 AF 50 01", 7, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24DAB AF 50 01", 7, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DCB AF 50 01", 7, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24DEB AF 50 01", 7, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };

				yield return new object[] { "62 F2CD0B AF 50 01", 7, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD08 AF 50 01", 7, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CDAB AF 50 01", 7, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDCB AF 50 01", 7, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CDEB AF 50 01", 7, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vfnmsub213ssV_VX_k1_HX_WX_2_Data))]
		void Test64_Vfnmsub213ssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z) {
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
		public static IEnumerable<object[]> Test64_Vfnmsub213ssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B AF D3", 6, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E20D0B AF D3", 6, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 124D03 AF D3", 6, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B24D0B AF D3", 6, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F24D7B AF D3", 6, Code.EVEX_Vfnmsub213ss_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };

				yield return new object[] { "62 F2CD8B AF D3", 6, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true };
				yield return new object[] { "62 E28D0B AF D3", 6, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 12CD03 AF D3", 6, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 B2CD0B AF D3", 6, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false };
				yield return new object[] { "62 F2CD7B AF D3", 6, Code.EVEX_Vfnmsub213sd_VX_k1z_HX_WX_er, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.RoundTowardZero, false };
			}
		}
	}
}
