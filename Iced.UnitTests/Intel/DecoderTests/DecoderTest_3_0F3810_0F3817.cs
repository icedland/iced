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
	public sealed class DecoderTest_3_0F3810_0F3817 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_PblendvbV_VX_WX_1_Data))]
		void Test16_PblendvbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_PblendvbV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F3810 08", 5, Code.Pblendvb_VX_WX, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PblendvbV_VX_WX_2_Data))]
		void Test16_PblendvbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test16_PblendvbV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F3810 CD", 5, Code.Pblendvb_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PblendvbV_VX_WX_1_Data))]
		void Test32_PblendvbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_PblendvbV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F3810 08", 5, Code.Pblendvb_VX_WX, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PblendvbV_VX_WX_2_Data))]
		void Test32_PblendvbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test32_PblendvbV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F3810 CD", 5, Code.Pblendvb_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PblendvbV_VX_WX_1_Data))]
		void Test64_PblendvbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_PblendvbV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F3810 08", 5, Code.Pblendvb_VX_WX, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PblendvbV_VX_WX_2_Data))]
		void Test64_PblendvbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test64_PblendvbV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F3810 CD", 5, Code.Pblendvb_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3810 CD", 6, Code.Pblendvb_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3810 CD", 6, Code.Pblendvb_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3810 CD", 6, Code.Pblendvb_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrlvwV_VX_k1z_HX_WX_1_Data))]
		void Test16_VpsrlvwV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsrlvwV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD0B 10 50 01", 7, Code.EVEX_Vpsrlvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CD8D 10 50 01", 7, Code.EVEX_Vpsrlvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F2CD08 10 50 01", 7, Code.EVEX_Vpsrlvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F2CD2B 10 50 01", 7, Code.EVEX_Vpsrlvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CDAD 10 50 01", 7, Code.EVEX_Vpsrlvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F2CD28 10 50 01", 7, Code.EVEX_Vpsrlvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F2CD4B 10 50 01", 7, Code.EVEX_Vpsrlvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CDCD 10 50 01", 7, Code.EVEX_Vpsrlvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F2CD48 10 50 01", 7, Code.EVEX_Vpsrlvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrlvwV_VX_k1z_HX_WX_2_Data))]
		void Test16_VpsrlvwV_VX_k1z_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpsrlvwV_VX_k1z_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD0B 10 D3", 6, Code.EVEX_Vpsrlvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD8B 10 D3", 6, Code.EVEX_Vpsrlvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD2B 10 D3", 6, Code.EVEX_Vpsrlvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDAB 10 D3", 6, Code.EVEX_Vpsrlvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD4B 10 D3", 6, Code.EVEX_Vpsrlvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDCB 10 D3", 6, Code.EVEX_Vpsrlvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrlvwV_VX_k1z_HX_WX_1_Data))]
		void Test32_VpsrlvwV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsrlvwV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD0B 10 50 01", 7, Code.EVEX_Vpsrlvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CD8D 10 50 01", 7, Code.EVEX_Vpsrlvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F2CD08 10 50 01", 7, Code.EVEX_Vpsrlvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F2CD2B 10 50 01", 7, Code.EVEX_Vpsrlvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CDAD 10 50 01", 7, Code.EVEX_Vpsrlvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F2CD28 10 50 01", 7, Code.EVEX_Vpsrlvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F2CD4B 10 50 01", 7, Code.EVEX_Vpsrlvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CDCD 10 50 01", 7, Code.EVEX_Vpsrlvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F2CD48 10 50 01", 7, Code.EVEX_Vpsrlvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrlvwV_VX_k1z_HX_WX_2_Data))]
		void Test32_VpsrlvwV_VX_k1z_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpsrlvwV_VX_k1z_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD0B 10 D3", 6, Code.EVEX_Vpsrlvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD8B 10 D3", 6, Code.EVEX_Vpsrlvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD2B 10 D3", 6, Code.EVEX_Vpsrlvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDAB 10 D3", 6, Code.EVEX_Vpsrlvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD4B 10 D3", 6, Code.EVEX_Vpsrlvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDCB 10 D3", 6, Code.EVEX_Vpsrlvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrlvwV_VX_k1z_HX_WX_1_Data))]
		void Test64_VpsrlvwV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsrlvwV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD0B 10 50 01", 7, Code.EVEX_Vpsrlvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CD8D 10 50 01", 7, Code.EVEX_Vpsrlvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F2CD08 10 50 01", 7, Code.EVEX_Vpsrlvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F2CD2B 10 50 01", 7, Code.EVEX_Vpsrlvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CDAD 10 50 01", 7, Code.EVEX_Vpsrlvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F2CD28 10 50 01", 7, Code.EVEX_Vpsrlvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F2CD4B 10 50 01", 7, Code.EVEX_Vpsrlvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CDCD 10 50 01", 7, Code.EVEX_Vpsrlvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F2CD48 10 50 01", 7, Code.EVEX_Vpsrlvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrlvwV_VX_k1z_HX_WX_2_Data))]
		void Test64_VpsrlvwV_VX_k1z_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpsrlvwV_VX_k1z_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD0B 10 D3", 6, Code.EVEX_Vpsrlvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E28D8B 10 D3", 6, Code.EVEX_Vpsrlvw_VX_k1z_HX_WX, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 12CD03 10 D3", 6, Code.EVEX_Vpsrlvw_VX_k1z_HX_WX, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD0B 10 D3", 6, Code.EVEX_Vpsrlvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD2B 10 D3", 6, Code.EVEX_Vpsrlvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E28DAB 10 D3", 6, Code.EVEX_Vpsrlvw_VY_k1z_HY_WY, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 12CD23 10 D3", 6, Code.EVEX_Vpsrlvw_VY_k1z_HY_WY, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD2B 10 D3", 6, Code.EVEX_Vpsrlvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD4B 10 D3", 6, Code.EVEX_Vpsrlvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E28DCB 10 D3", 6, Code.EVEX_Vpsrlvw_VZ_k1z_HZ_WZ, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 12CD43 10 D3", 6, Code.EVEX_Vpsrlvw_VZ_k1z_HZ_WZ, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD4B 10 D3", 6, Code.EVEX_Vpsrlvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovuswbV_WX_k1z_VX_1_Data))]
		void Test16_VpmovuswbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovuswbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 10 50 01", 7, Code.EVEX_Vpmovuswb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_UInt8, 8, false };
				yield return new object[] { "62 F27E08 10 50 01", 7, Code.EVEX_Vpmovuswb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };

				yield return new object[] { "62 F27E2B 10 50 01", 7, Code.EVEX_Vpmovuswb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F27E28 10 50 01", 7, Code.EVEX_Vpmovuswb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F27E4B 10 50 01", 7, Code.EVEX_Vpmovuswb_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F27E48 10 50 01", 7, Code.EVEX_Vpmovuswb_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_UInt8, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovuswbV_WX_k1z_VX_2_Data))]
		void Test16_VpmovuswbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovuswbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 10 D3", 6, Code.EVEX_Vpmovuswb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 10 D3", 6, Code.EVEX_Vpmovuswb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 10 D3", 6, Code.EVEX_Vpmovuswb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 10 D3", 6, Code.EVEX_Vpmovuswb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 10 D3", 6, Code.EVEX_Vpmovuswb_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 10 D3", 6, Code.EVEX_Vpmovuswb_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovuswbV_WX_k1z_VX_1_Data))]
		void Test32_VpmovuswbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovuswbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 10 50 01", 7, Code.EVEX_Vpmovuswb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_UInt8, 8, false };
				yield return new object[] { "62 F27E08 10 50 01", 7, Code.EVEX_Vpmovuswb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };

				yield return new object[] { "62 F27E2B 10 50 01", 7, Code.EVEX_Vpmovuswb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F27E28 10 50 01", 7, Code.EVEX_Vpmovuswb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F27E4B 10 50 01", 7, Code.EVEX_Vpmovuswb_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F27E48 10 50 01", 7, Code.EVEX_Vpmovuswb_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_UInt8, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovuswbV_WX_k1z_VX_2_Data))]
		void Test32_VpmovuswbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovuswbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 10 D3", 6, Code.EVEX_Vpmovuswb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 10 D3", 6, Code.EVEX_Vpmovuswb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 10 D3", 6, Code.EVEX_Vpmovuswb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 10 D3", 6, Code.EVEX_Vpmovuswb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 10 D3", 6, Code.EVEX_Vpmovuswb_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 10 D3", 6, Code.EVEX_Vpmovuswb_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovuswbV_WX_k1z_VX_1_Data))]
		void Test64_VpmovuswbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovuswbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 10 50 01", 7, Code.EVEX_Vpmovuswb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_UInt8, 8, false };
				yield return new object[] { "62 F27E08 10 50 01", 7, Code.EVEX_Vpmovuswb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };

				yield return new object[] { "62 F27E2B 10 50 01", 7, Code.EVEX_Vpmovuswb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F27E28 10 50 01", 7, Code.EVEX_Vpmovuswb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F27E4B 10 50 01", 7, Code.EVEX_Vpmovuswb_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F27E48 10 50 01", 7, Code.EVEX_Vpmovuswb_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_UInt8, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovuswbV_WX_k1z_VX_2_Data))]
		void Test64_VpmovuswbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovuswbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 10 D3", 6, Code.EVEX_Vpmovuswb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E8B 10 D3", 6, Code.EVEX_Vpmovuswb_WX_k1z_VX, Register.XMM3, Register.XMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E0B 10 D3", 6, Code.EVEX_Vpmovuswb_WX_k1z_VX, Register.XMM27, Register.XMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E0B 10 D3", 6, Code.EVEX_Vpmovuswb_WX_k1z_VX, Register.XMM19, Register.XMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E2B 10 D3", 6, Code.EVEX_Vpmovuswb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27EAB 10 D3", 6, Code.EVEX_Vpmovuswb_WX_k1z_VY, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E2B 10 D3", 6, Code.EVEX_Vpmovuswb_WX_k1z_VY, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E2B 10 D3", 6, Code.EVEX_Vpmovuswb_WX_k1z_VY, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E4B 10 D3", 6, Code.EVEX_Vpmovuswb_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27ECB 10 D3", 6, Code.EVEX_Vpmovuswb_WY_k1z_VZ, Register.YMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E4B 10 D3", 6, Code.EVEX_Vpmovuswb_WY_k1z_VZ, Register.YMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E4B 10 D3", 6, Code.EVEX_Vpmovuswb_WY_k1z_VZ, Register.YMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsravwV_VX_k1z_HX_WX_1_Data))]
		void Test16_VpsravwV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsravwV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD0B 11 50 01", 7, Code.EVEX_Vpsravw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CD8D 11 50 01", 7, Code.EVEX_Vpsravw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F2CD08 11 50 01", 7, Code.EVEX_Vpsravw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F2CD2B 11 50 01", 7, Code.EVEX_Vpsravw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CDAD 11 50 01", 7, Code.EVEX_Vpsravw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F2CD28 11 50 01", 7, Code.EVEX_Vpsravw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F2CD4B 11 50 01", 7, Code.EVEX_Vpsravw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CDCD 11 50 01", 7, Code.EVEX_Vpsravw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F2CD48 11 50 01", 7, Code.EVEX_Vpsravw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsravwV_VX_k1z_HX_WX_2_Data))]
		void Test16_VpsravwV_VX_k1z_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpsravwV_VX_k1z_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD0B 11 D3", 6, Code.EVEX_Vpsravw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD8B 11 D3", 6, Code.EVEX_Vpsravw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD2B 11 D3", 6, Code.EVEX_Vpsravw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDAB 11 D3", 6, Code.EVEX_Vpsravw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD4B 11 D3", 6, Code.EVEX_Vpsravw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDCB 11 D3", 6, Code.EVEX_Vpsravw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsravwV_VX_k1z_HX_WX_1_Data))]
		void Test32_VpsravwV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsravwV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD0B 11 50 01", 7, Code.EVEX_Vpsravw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CD8D 11 50 01", 7, Code.EVEX_Vpsravw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F2CD08 11 50 01", 7, Code.EVEX_Vpsravw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F2CD2B 11 50 01", 7, Code.EVEX_Vpsravw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CDAD 11 50 01", 7, Code.EVEX_Vpsravw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F2CD28 11 50 01", 7, Code.EVEX_Vpsravw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F2CD4B 11 50 01", 7, Code.EVEX_Vpsravw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CDCD 11 50 01", 7, Code.EVEX_Vpsravw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F2CD48 11 50 01", 7, Code.EVEX_Vpsravw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsravwV_VX_k1z_HX_WX_2_Data))]
		void Test32_VpsravwV_VX_k1z_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpsravwV_VX_k1z_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD0B 11 D3", 6, Code.EVEX_Vpsravw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD8B 11 D3", 6, Code.EVEX_Vpsravw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD2B 11 D3", 6, Code.EVEX_Vpsravw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDAB 11 D3", 6, Code.EVEX_Vpsravw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD4B 11 D3", 6, Code.EVEX_Vpsravw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDCB 11 D3", 6, Code.EVEX_Vpsravw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsravwV_VX_k1z_HX_WX_1_Data))]
		void Test64_VpsravwV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsravwV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD0B 11 50 01", 7, Code.EVEX_Vpsravw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CD8D 11 50 01", 7, Code.EVEX_Vpsravw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F2CD08 11 50 01", 7, Code.EVEX_Vpsravw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F2CD2B 11 50 01", 7, Code.EVEX_Vpsravw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CDAD 11 50 01", 7, Code.EVEX_Vpsravw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F2CD28 11 50 01", 7, Code.EVEX_Vpsravw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F2CD4B 11 50 01", 7, Code.EVEX_Vpsravw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CDCD 11 50 01", 7, Code.EVEX_Vpsravw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F2CD48 11 50 01", 7, Code.EVEX_Vpsravw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsravwV_VX_k1z_HX_WX_2_Data))]
		void Test64_VpsravwV_VX_k1z_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpsravwV_VX_k1z_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD0B 11 D3", 6, Code.EVEX_Vpsravw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E28D8B 11 D3", 6, Code.EVEX_Vpsravw_VX_k1z_HX_WX, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 12CD03 11 D3", 6, Code.EVEX_Vpsravw_VX_k1z_HX_WX, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD0B 11 D3", 6, Code.EVEX_Vpsravw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD2B 11 D3", 6, Code.EVEX_Vpsravw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E28DAB 11 D3", 6, Code.EVEX_Vpsravw_VY_k1z_HY_WY, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 12CD23 11 D3", 6, Code.EVEX_Vpsravw_VY_k1z_HY_WY, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD2B 11 D3", 6, Code.EVEX_Vpsravw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD4B 11 D3", 6, Code.EVEX_Vpsravw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E28DCB 11 D3", 6, Code.EVEX_Vpsravw_VZ_k1z_HZ_WZ, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 12CD43 11 D3", 6, Code.EVEX_Vpsravw_VZ_k1z_HZ_WZ, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD4B 11 D3", 6, Code.EVEX_Vpsravw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovusdbV_WX_k1z_VX_1_Data))]
		void Test16_VpmovusdbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovusdbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 11 50 01", 7, Code.EVEX_Vpmovusdb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed32_UInt8, 4, false };
				yield return new object[] { "62 F27E08 11 50 01", 7, Code.EVEX_Vpmovusdb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed32_UInt8, 4, false };

				yield return new object[] { "62 F27E2B 11 50 01", 7, Code.EVEX_Vpmovusdb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed64_UInt8, 8, false };
				yield return new object[] { "62 F27E28 11 50 01", 7, Code.EVEX_Vpmovusdb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };

				yield return new object[] { "62 F27E4B 11 50 01", 7, Code.EVEX_Vpmovusdb_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F27E48 11 50 01", 7, Code.EVEX_Vpmovusdb_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed128_UInt8, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovusdbV_WX_k1z_VX_2_Data))]
		void Test16_VpmovusdbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovusdbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovusdbV_WX_k1z_VX_1_Data))]
		void Test32_VpmovusdbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovusdbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 11 50 01", 7, Code.EVEX_Vpmovusdb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed32_UInt8, 4, false };
				yield return new object[] { "62 F27E08 11 50 01", 7, Code.EVEX_Vpmovusdb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed32_UInt8, 4, false };

				yield return new object[] { "62 F27E2B 11 50 01", 7, Code.EVEX_Vpmovusdb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed64_UInt8, 8, false };
				yield return new object[] { "62 F27E28 11 50 01", 7, Code.EVEX_Vpmovusdb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };

				yield return new object[] { "62 F27E4B 11 50 01", 7, Code.EVEX_Vpmovusdb_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F27E48 11 50 01", 7, Code.EVEX_Vpmovusdb_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed128_UInt8, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovusdbV_WX_k1z_VX_2_Data))]
		void Test32_VpmovusdbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovusdbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovusdbV_WX_k1z_VX_1_Data))]
		void Test64_VpmovusdbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovusdbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 11 50 01", 7, Code.EVEX_Vpmovusdb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed32_UInt8, 4, false };
				yield return new object[] { "62 F27E08 11 50 01", 7, Code.EVEX_Vpmovusdb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed32_UInt8, 4, false };

				yield return new object[] { "62 F27E2B 11 50 01", 7, Code.EVEX_Vpmovusdb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed64_UInt8, 8, false };
				yield return new object[] { "62 F27E28 11 50 01", 7, Code.EVEX_Vpmovusdb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };

				yield return new object[] { "62 F27E4B 11 50 01", 7, Code.EVEX_Vpmovusdb_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F27E48 11 50 01", 7, Code.EVEX_Vpmovusdb_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed128_UInt8, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovusdbV_WX_k1z_VX_2_Data))]
		void Test64_VpmovusdbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovusdbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E8B 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VX, Register.XMM3, Register.XMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E0B 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VX, Register.XMM27, Register.XMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E0B 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VX, Register.XMM19, Register.XMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E2B 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27EAB 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VY, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E2B 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VY, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E2B 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VY, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E4B 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27ECB 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VZ, Register.XMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E4B 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VZ, Register.XMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E4B 11 D3", 6, Code.EVEX_Vpmovusdb_WX_k1z_VZ, Register.XMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsllvwV_VX_k1z_HX_WX_1_Data))]
		void Test16_VpsllvwV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsllvwV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD0B 12 50 01", 7, Code.EVEX_Vpsllvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CD8D 12 50 01", 7, Code.EVEX_Vpsllvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F2CD08 12 50 01", 7, Code.EVEX_Vpsllvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F2CD2B 12 50 01", 7, Code.EVEX_Vpsllvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CDAD 12 50 01", 7, Code.EVEX_Vpsllvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F2CD28 12 50 01", 7, Code.EVEX_Vpsllvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F2CD4B 12 50 01", 7, Code.EVEX_Vpsllvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CDCD 12 50 01", 7, Code.EVEX_Vpsllvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F2CD48 12 50 01", 7, Code.EVEX_Vpsllvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsllvwV_VX_k1z_HX_WX_2_Data))]
		void Test16_VpsllvwV_VX_k1z_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpsllvwV_VX_k1z_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD0B 12 D3", 6, Code.EVEX_Vpsllvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD8B 12 D3", 6, Code.EVEX_Vpsllvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD2B 12 D3", 6, Code.EVEX_Vpsllvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDAB 12 D3", 6, Code.EVEX_Vpsllvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD4B 12 D3", 6, Code.EVEX_Vpsllvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDCB 12 D3", 6, Code.EVEX_Vpsllvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsllvwV_VX_k1z_HX_WX_1_Data))]
		void Test32_VpsllvwV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsllvwV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD0B 12 50 01", 7, Code.EVEX_Vpsllvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CD8D 12 50 01", 7, Code.EVEX_Vpsllvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F2CD08 12 50 01", 7, Code.EVEX_Vpsllvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F2CD2B 12 50 01", 7, Code.EVEX_Vpsllvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CDAD 12 50 01", 7, Code.EVEX_Vpsllvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F2CD28 12 50 01", 7, Code.EVEX_Vpsllvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F2CD4B 12 50 01", 7, Code.EVEX_Vpsllvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CDCD 12 50 01", 7, Code.EVEX_Vpsllvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F2CD48 12 50 01", 7, Code.EVEX_Vpsllvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsllvwV_VX_k1z_HX_WX_2_Data))]
		void Test32_VpsllvwV_VX_k1z_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpsllvwV_VX_k1z_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD0B 12 D3", 6, Code.EVEX_Vpsllvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD8B 12 D3", 6, Code.EVEX_Vpsllvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD2B 12 D3", 6, Code.EVEX_Vpsllvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDAB 12 D3", 6, Code.EVEX_Vpsllvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2CD4B 12 D3", 6, Code.EVEX_Vpsllvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CDCB 12 D3", 6, Code.EVEX_Vpsllvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsllvwV_VX_k1z_HX_WX_1_Data))]
		void Test64_VpsllvwV_VX_k1z_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsllvwV_VX_k1z_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD0B 12 50 01", 7, Code.EVEX_Vpsllvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F2CD8D 12 50 01", 7, Code.EVEX_Vpsllvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F2CD08 12 50 01", 7, Code.EVEX_Vpsllvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F2CD2B 12 50 01", 7, Code.EVEX_Vpsllvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F2CDAD 12 50 01", 7, Code.EVEX_Vpsllvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F2CD28 12 50 01", 7, Code.EVEX_Vpsllvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F2CD4B 12 50 01", 7, Code.EVEX_Vpsllvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F2CDCD 12 50 01", 7, Code.EVEX_Vpsllvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F2CD48 12 50 01", 7, Code.EVEX_Vpsllvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsllvwV_VX_k1z_HX_WX_2_Data))]
		void Test64_VpsllvwV_VX_k1z_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpsllvwV_VX_k1z_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD0B 12 D3", 6, Code.EVEX_Vpsllvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E28D8B 12 D3", 6, Code.EVEX_Vpsllvw_VX_k1z_HX_WX, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 12CD03 12 D3", 6, Code.EVEX_Vpsllvw_VX_k1z_HX_WX, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD0B 12 D3", 6, Code.EVEX_Vpsllvw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD2B 12 D3", 6, Code.EVEX_Vpsllvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E28DAB 12 D3", 6, Code.EVEX_Vpsllvw_VY_k1z_HY_WY, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 12CD23 12 D3", 6, Code.EVEX_Vpsllvw_VY_k1z_HY_WY, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD2B 12 D3", 6, Code.EVEX_Vpsllvw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F2CD4B 12 D3", 6, Code.EVEX_Vpsllvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E28DCB 12 D3", 6, Code.EVEX_Vpsllvw_VZ_k1z_HZ_WZ, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 12CD43 12 D3", 6, Code.EVEX_Vpsllvw_VZ_k1z_HZ_WZ, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD4B 12 D3", 6, Code.EVEX_Vpsllvw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovusqbV_WX_k1z_VX_1_Data))]
		void Test16_VpmovusqbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovusqbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 12 50 01", 7, Code.EVEX_Vpmovusqb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed16_UInt8, 2, false };
				yield return new object[] { "62 F27E08 12 50 01", 7, Code.EVEX_Vpmovusqb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed16_UInt8, 2, false };

				yield return new object[] { "62 F27E2B 12 50 01", 7, Code.EVEX_Vpmovusqb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed32_UInt8, 4, false };
				yield return new object[] { "62 F27E28 12 50 01", 7, Code.EVEX_Vpmovusqb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed32_UInt8, 4, false };

				yield return new object[] { "62 F27E4B 12 50 01", 7, Code.EVEX_Vpmovusqb_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed64_UInt8, 8, false };
				yield return new object[] { "62 F27E48 12 50 01", 7, Code.EVEX_Vpmovusqb_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovusqbV_WX_k1z_VX_2_Data))]
		void Test16_VpmovusqbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovusqbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovusqbV_WX_k1z_VX_1_Data))]
		void Test32_VpmovusqbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovusqbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 12 50 01", 7, Code.EVEX_Vpmovusqb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed16_UInt8, 2, false };
				yield return new object[] { "62 F27E08 12 50 01", 7, Code.EVEX_Vpmovusqb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed16_UInt8, 2, false };

				yield return new object[] { "62 F27E2B 12 50 01", 7, Code.EVEX_Vpmovusqb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed32_UInt8, 4, false };
				yield return new object[] { "62 F27E28 12 50 01", 7, Code.EVEX_Vpmovusqb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed32_UInt8, 4, false };

				yield return new object[] { "62 F27E4B 12 50 01", 7, Code.EVEX_Vpmovusqb_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed64_UInt8, 8, false };
				yield return new object[] { "62 F27E48 12 50 01", 7, Code.EVEX_Vpmovusqb_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovusqbV_WX_k1z_VX_2_Data))]
		void Test32_VpmovusqbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovusqbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovusqbV_WX_k1z_VX_1_Data))]
		void Test64_VpmovusqbV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovusqbV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 12 50 01", 7, Code.EVEX_Vpmovusqb_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed16_UInt8, 2, false };
				yield return new object[] { "62 F27E08 12 50 01", 7, Code.EVEX_Vpmovusqb_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed16_UInt8, 2, false };

				yield return new object[] { "62 F27E2B 12 50 01", 7, Code.EVEX_Vpmovusqb_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed32_UInt8, 4, false };
				yield return new object[] { "62 F27E28 12 50 01", 7, Code.EVEX_Vpmovusqb_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed32_UInt8, 4, false };

				yield return new object[] { "62 F27E4B 12 50 01", 7, Code.EVEX_Vpmovusqb_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed64_UInt8, 8, false };
				yield return new object[] { "62 F27E48 12 50 01", 7, Code.EVEX_Vpmovusqb_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed64_UInt8, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovusqbV_WX_k1z_VX_2_Data))]
		void Test64_VpmovusqbV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovusqbV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E8B 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VX, Register.XMM3, Register.XMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E0B 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VX, Register.XMM27, Register.XMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E0B 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VX, Register.XMM19, Register.XMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E2B 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27EAB 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VY, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E2B 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VY, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E2B 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VY, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E4B 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27ECB 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VZ, Register.XMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E4B 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VZ, Register.XMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E4B 12 D3", 6, Code.EVEX_Vpmovusqb_WX_k1z_VZ, Register.XMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vcvtph2psV_Reg_RegMem_1_Data))]
		void Test16_Vcvtph2psV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_Vcvtph2psV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E279 13 10", 5, Code.VEX_Vcvtph2ps_VX_WX, Register.XMM2, MemorySize.Packed64_Float16 };

				yield return new object[] { "C4E27D 13 10", 5, Code.VEX_Vcvtph2ps_VY_WX, Register.YMM2, MemorySize.Packed128_Float16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vcvtph2psV_Reg_RegMem_2_Data))]
		void Test16_Vcvtph2psV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test16_Vcvtph2psV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E279 13 CD", 5, Code.VEX_Vcvtph2ps_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 13 CD", 5, Code.VEX_Vcvtph2ps_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vcvtph2psV_Reg_RegMem_1_Data))]
		void Test32_Vcvtph2psV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_Vcvtph2psV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E279 13 10", 5, Code.VEX_Vcvtph2ps_VX_WX, Register.XMM2, MemorySize.Packed64_Float16 };

				yield return new object[] { "C4E27D 13 10", 5, Code.VEX_Vcvtph2ps_VY_WX, Register.YMM2, MemorySize.Packed128_Float16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vcvtph2psV_Reg_RegMem_2_Data))]
		void Test32_Vcvtph2psV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test32_Vcvtph2psV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E279 13 CD", 5, Code.VEX_Vcvtph2ps_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 13 CD", 5, Code.VEX_Vcvtph2ps_VY_WX, Register.YMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vcvtph2psV_Reg_RegMem_1_Data))]
		void Test64_Vcvtph2psV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_Vcvtph2psV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E279 13 10", 5, Code.VEX_Vcvtph2ps_VX_WX, Register.XMM2, MemorySize.Packed64_Float16 };

				yield return new object[] { "C4E27D 13 10", 5, Code.VEX_Vcvtph2ps_VY_WX, Register.YMM2, MemorySize.Packed128_Float16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vcvtph2psV_Reg_RegMem_2_Data))]
		void Test64_Vcvtph2psV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test64_Vcvtph2psV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E279 13 CD", 5, Code.VEX_Vcvtph2ps_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 13 CD", 5, Code.VEX_Vcvtph2ps_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 13 CD", 5, Code.VEX_Vcvtph2ps_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 13 CD", 5, Code.VEX_Vcvtph2ps_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E27D 13 CD", 5, Code.VEX_Vcvtph2ps_VY_WX, Register.YMM1, Register.XMM5 };
				yield return new object[] { "C4627D 13 CD", 5, Code.VEX_Vcvtph2ps_VY_WX, Register.YMM9, Register.XMM5 };
				yield return new object[] { "C4C27D 13 CD", 5, Code.VEX_Vcvtph2ps_VY_WX, Register.YMM1, Register.XMM13 };
				yield return new object[] { "C4427D 13 CD", 5, Code.VEX_Vcvtph2ps_VY_WX, Register.YMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VrmeoveV_VX_k1_HX_WX_1_Data))]
		void Test16_VrmeoveV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VrmeoveV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 13 50 01", 7, Code.EVEX_Vcvtph2ps_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_Float16, 8, false };
				yield return new object[] { "62 F27D08 13 50 01", 7, Code.EVEX_Vcvtph2ps_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_Float16, 8, false };

				yield return new object[] { "62 F27D2B 13 50 01", 7, Code.EVEX_Vcvtph2ps_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed128_Float16, 16, false };
				yield return new object[] { "62 F27D28 13 50 01", 7, Code.EVEX_Vcvtph2ps_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_Float16, 16, false };

				yield return new object[] { "62 F27D4B 13 50 01", 7, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM2, Register.K3, MemorySize.Packed256_Float16, 32, false };
				yield return new object[] { "62 F27D48 13 50 01", 7, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM2, Register.None, MemorySize.Packed256_Float16, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VrmeoveV_VX_k1_HX_WX_2_Data))]
		void Test16_VrmeoveV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VrmeoveV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F27D8B 13 D3", 6, Code.EVEX_Vcvtph2ps_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F27D08 13 D3", 6, Code.EVEX_Vcvtph2ps_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D9B 13 D3", 6, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F27DAB 13 D3", 6, Code.EVEX_Vcvtph2ps_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F27D28 13 D3", 6, Code.EVEX_Vcvtph2ps_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DBB 13 D3", 6, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F27DCB 13 D3", 6, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F27D48 13 D3", 6, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DDB 13 D3", 6, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VrmeoveV_VX_k1_HX_WX_1_Data))]
		void Test32_VrmeoveV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VrmeoveV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 13 50 01", 7, Code.EVEX_Vcvtph2ps_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_Float16, 8, false };
				yield return new object[] { "62 F27D08 13 50 01", 7, Code.EVEX_Vcvtph2ps_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_Float16, 8, false };

				yield return new object[] { "62 F27D2B 13 50 01", 7, Code.EVEX_Vcvtph2ps_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed128_Float16, 16, false };
				yield return new object[] { "62 F27D28 13 50 01", 7, Code.EVEX_Vcvtph2ps_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_Float16, 16, false };

				yield return new object[] { "62 F27D4B 13 50 01", 7, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM2, Register.K3, MemorySize.Packed256_Float16, 32, false };
				yield return new object[] { "62 F27D48 13 50 01", 7, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM2, Register.None, MemorySize.Packed256_Float16, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VrmeoveV_VX_k1_HX_WX_2_Data))]
		void Test32_VrmeoveV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VrmeoveV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F27D8B 13 D3", 6, Code.EVEX_Vcvtph2ps_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F27D08 13 D3", 6, Code.EVEX_Vcvtph2ps_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D9B 13 D3", 6, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F27DAB 13 D3", 6, Code.EVEX_Vcvtph2ps_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F27D28 13 D3", 6, Code.EVEX_Vcvtph2ps_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DBB 13 D3", 6, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F27DCB 13 D3", 6, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F27D48 13 D3", 6, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DDB 13 D3", 6, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VrmeoveV_VX_k1_HX_WX_1_Data))]
		void Test64_VrmeoveV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VrmeoveV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 13 50 01", 7, Code.EVEX_Vcvtph2ps_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed64_Float16, 8, false };
				yield return new object[] { "62 F27D08 13 50 01", 7, Code.EVEX_Vcvtph2ps_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed64_Float16, 8, false };

				yield return new object[] { "62 F27D2B 13 50 01", 7, Code.EVEX_Vcvtph2ps_VY_k1z_WX, Register.YMM2, Register.K3, MemorySize.Packed128_Float16, 16, false };
				yield return new object[] { "62 F27D28 13 50 01", 7, Code.EVEX_Vcvtph2ps_VY_k1z_WX, Register.YMM2, Register.None, MemorySize.Packed128_Float16, 16, false };

				yield return new object[] { "62 F27D4B 13 50 01", 7, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM2, Register.K3, MemorySize.Packed256_Float16, 32, false };
				yield return new object[] { "62 F27D48 13 50 01", 7, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM2, Register.None, MemorySize.Packed256_Float16, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VrmeoveV_VX_k1_HX_WX_2_Data))]
		void Test64_VrmeoveV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VrmeoveV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F27D8B 13 D3", 6, Code.EVEX_Vcvtph2ps_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 E27D0B 13 D3", 6, Code.EVEX_Vcvtph2ps_VX_k1z_WX, Register.XMM18, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 127D0B 13 D3", 6, Code.EVEX_Vcvtph2ps_VX_k1z_WX, Register.XMM10, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27D0B 13 D3", 6, Code.EVEX_Vcvtph2ps_VX_k1z_WX, Register.XMM2, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DDB 13 D3", 6, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F27DAB 13 D3", 6, Code.EVEX_Vcvtph2ps_VY_k1z_WX, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 E27D2B 13 D3", 6, Code.EVEX_Vcvtph2ps_VY_k1z_WX, Register.YMM18, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 127D2B 13 D3", 6, Code.EVEX_Vcvtph2ps_VY_k1z_WX, Register.YMM10, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27D2B 13 D3", 6, Code.EVEX_Vcvtph2ps_VY_k1z_WX, Register.YMM2, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D1B 13 D3", 6, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F27DCB 13 D3", 6, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 E27D4B 13 D3", 6, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM18, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 127D4B 13 D3", 6, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM10, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27D4B 13 D3", 6, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM2, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D7B 13 D3", 6, Code.EVEX_Vcvtph2ps_VZ_k1z_WY_sae, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovusdwV_WX_k1z_VX_1_Data))]
		void Test16_VpmovusdwV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovusdwV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 13 50 01", 7, Code.EVEX_Vpmovusdw_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_UInt16, 8, false };
				yield return new object[] { "62 F27E08 13 50 01", 7, Code.EVEX_Vpmovusdw_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_UInt16, 8, false };

				yield return new object[] { "62 F27E2B 13 50 01", 7, Code.EVEX_Vpmovusdw_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F27E28 13 50 01", 7, Code.EVEX_Vpmovusdw_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F27E4B 13 50 01", 7, Code.EVEX_Vpmovusdw_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F27E48 13 50 01", 7, Code.EVEX_Vpmovusdw_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_UInt16, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovusdwV_WX_k1z_VX_2_Data))]
		void Test16_VpmovusdwV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovusdwV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 13 D3", 6, Code.EVEX_Vpmovusdw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 13 D3", 6, Code.EVEX_Vpmovusdw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 13 D3", 6, Code.EVEX_Vpmovusdw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 13 D3", 6, Code.EVEX_Vpmovusdw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 13 D3", 6, Code.EVEX_Vpmovusdw_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 13 D3", 6, Code.EVEX_Vpmovusdw_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovusdwV_WX_k1z_VX_1_Data))]
		void Test32_VpmovusdwV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovusdwV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 13 50 01", 7, Code.EVEX_Vpmovusdw_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_UInt16, 8, false };
				yield return new object[] { "62 F27E08 13 50 01", 7, Code.EVEX_Vpmovusdw_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_UInt16, 8, false };

				yield return new object[] { "62 F27E2B 13 50 01", 7, Code.EVEX_Vpmovusdw_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F27E28 13 50 01", 7, Code.EVEX_Vpmovusdw_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F27E4B 13 50 01", 7, Code.EVEX_Vpmovusdw_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F27E48 13 50 01", 7, Code.EVEX_Vpmovusdw_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_UInt16, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovusdwV_WX_k1z_VX_2_Data))]
		void Test32_VpmovusdwV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovusdwV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 13 D3", 6, Code.EVEX_Vpmovusdw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 13 D3", 6, Code.EVEX_Vpmovusdw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 13 D3", 6, Code.EVEX_Vpmovusdw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 13 D3", 6, Code.EVEX_Vpmovusdw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 13 D3", 6, Code.EVEX_Vpmovusdw_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 13 D3", 6, Code.EVEX_Vpmovusdw_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovusdwV_WX_k1z_VX_1_Data))]
		void Test64_VpmovusdwV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovusdwV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 13 50 01", 7, Code.EVEX_Vpmovusdw_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_UInt16, 8, false };
				yield return new object[] { "62 F27E08 13 50 01", 7, Code.EVEX_Vpmovusdw_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_UInt16, 8, false };

				yield return new object[] { "62 F27E2B 13 50 01", 7, Code.EVEX_Vpmovusdw_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F27E28 13 50 01", 7, Code.EVEX_Vpmovusdw_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F27E4B 13 50 01", 7, Code.EVEX_Vpmovusdw_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F27E48 13 50 01", 7, Code.EVEX_Vpmovusdw_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_UInt16, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovusdwV_WX_k1z_VX_2_Data))]
		void Test64_VpmovusdwV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovusdwV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 13 D3", 6, Code.EVEX_Vpmovusdw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E8B 13 D3", 6, Code.EVEX_Vpmovusdw_WX_k1z_VX, Register.XMM3, Register.XMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E0B 13 D3", 6, Code.EVEX_Vpmovusdw_WX_k1z_VX, Register.XMM27, Register.XMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E0B 13 D3", 6, Code.EVEX_Vpmovusdw_WX_k1z_VX, Register.XMM19, Register.XMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E2B 13 D3", 6, Code.EVEX_Vpmovusdw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27EAB 13 D3", 6, Code.EVEX_Vpmovusdw_WX_k1z_VY, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E2B 13 D3", 6, Code.EVEX_Vpmovusdw_WX_k1z_VY, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E2B 13 D3", 6, Code.EVEX_Vpmovusdw_WX_k1z_VY, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E4B 13 D3", 6, Code.EVEX_Vpmovusdw_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27ECB 13 D3", 6, Code.EVEX_Vpmovusdw_WY_k1z_VZ, Register.YMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E4B 13 D3", 6, Code.EVEX_Vpmovusdw_WY_k1z_VZ, Register.YMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E4B 13 D3", 6, Code.EVEX_Vpmovusdw_WY_k1z_VZ, Register.YMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_BlendvV_VX_WX_Reg_1_Data))]
		void Test16_BlendvV_VX_WX_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_BlendvV_VX_WX_Reg_1_Data {
			get {
				yield return new object[] { "66 0F3814 08", 5, Code.Blendvps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F3815 08", 5, Code.Blendvpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_BlendvV_VX_WX_Reg_2_Data))]
		void Test16_BlendvV_VX_WX_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test16_BlendvV_VX_WX_Reg_2_Data {
			get {
				yield return new object[] { "66 0F3814 CD", 5, Code.Blendvps_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F3815 CD", 5, Code.Blendvpd_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_BlendvV_VX_WX_Reg_1_Data))]
		void Test32_BlendvV_VX_WX_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_BlendvV_VX_WX_Reg_1_Data {
			get {
				yield return new object[] { "66 0F3814 08", 5, Code.Blendvps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F3815 08", 5, Code.Blendvpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_BlendvV_VX_WX_Reg_2_Data))]
		void Test32_BlendvV_VX_WX_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test32_BlendvV_VX_WX_Reg_2_Data {
			get {
				yield return new object[] { "66 0F3814 CD", 5, Code.Blendvps_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "66 0F3815 CD", 5, Code.Blendvpd_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_BlendvV_VX_WX_Reg_1_Data))]
		void Test64_BlendvV_VX_WX_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_BlendvV_VX_WX_Reg_1_Data {
			get {
				yield return new object[] { "66 0F3814 08", 5, Code.Blendvps_VX_WX, Register.XMM1, MemorySize.Packed128_Float32 };

				yield return new object[] { "66 0F3815 08", 5, Code.Blendvpd_VX_WX, Register.XMM1, MemorySize.Packed128_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_BlendvV_VX_WX_Reg_2_Data))]
		void Test64_BlendvV_VX_WX_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test64_BlendvV_VX_WX_Reg_2_Data {
			get {
				yield return new object[] { "66 0F3814 CD", 5, Code.Blendvps_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3814 CD", 6, Code.Blendvps_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3814 CD", 6, Code.Blendvps_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3814 CD", 6, Code.Blendvps_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "66 0F3815 CD", 5, Code.Blendvpd_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3815 CD", 6, Code.Blendvpd_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3815 CD", 6, Code.Blendvpd_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3815 CD", 6, Code.Blendvpd_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VprorvV_VX_k1_HX_WX_1_Data))]
		void Test16_VprorvV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VprorvV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 14 50 01", 7, Code.EVEX_Vprorvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D9D 14 50 01", 7, Code.EVEX_Vprorvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 14 50 01", 7, Code.EVEX_Vprorvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 14 50 01", 7, Code.EVEX_Vprorvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24DBD 14 50 01", 7, Code.EVEX_Vprorvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 14 50 01", 7, Code.EVEX_Vprorvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 14 50 01", 7, Code.EVEX_Vprorvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24DDD 14 50 01", 7, Code.EVEX_Vprorvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 14 50 01", 7, Code.EVEX_Vprorvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 14 50 01", 7, Code.EVEX_Vprorvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD9D 14 50 01", 7, Code.EVEX_Vprorvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2CD08 14 50 01", 7, Code.EVEX_Vprorvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 14 50 01", 7, Code.EVEX_Vprorvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CDBD 14 50 01", 7, Code.EVEX_Vprorvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2CD28 14 50 01", 7, Code.EVEX_Vprorvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 14 50 01", 7, Code.EVEX_Vprorvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CDDD 14 50 01", 7, Code.EVEX_Vprorvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2CD48 14 50 01", 7, Code.EVEX_Vprorvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VprorvV_VX_k1_HX_WX_2_Data))]
		void Test16_VprorvV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VprorvV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 14 D3", 6, Code.EVEX_Vprorvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 14 D3", 6, Code.EVEX_Vprorvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 14 D3", 6, Code.EVEX_Vprorvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 14 D3", 6, Code.EVEX_Vprorvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 14 D3", 6, Code.EVEX_Vprorvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 14 D3", 6, Code.EVEX_Vprorvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 14 D3", 6, Code.EVEX_Vprorvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 14 D3", 6, Code.EVEX_Vprorvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 14 D3", 6, Code.EVEX_Vprorvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 14 D3", 6, Code.EVEX_Vprorvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 14 D3", 6, Code.EVEX_Vprorvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 14 D3", 6, Code.EVEX_Vprorvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VprorvV_VX_k1_HX_WX_1_Data))]
		void Test32_VprorvV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VprorvV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 14 50 01", 7, Code.EVEX_Vprorvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D9D 14 50 01", 7, Code.EVEX_Vprorvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 14 50 01", 7, Code.EVEX_Vprorvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 14 50 01", 7, Code.EVEX_Vprorvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24DBD 14 50 01", 7, Code.EVEX_Vprorvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 14 50 01", 7, Code.EVEX_Vprorvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 14 50 01", 7, Code.EVEX_Vprorvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24DDD 14 50 01", 7, Code.EVEX_Vprorvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 14 50 01", 7, Code.EVEX_Vprorvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 14 50 01", 7, Code.EVEX_Vprorvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD9D 14 50 01", 7, Code.EVEX_Vprorvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2CD08 14 50 01", 7, Code.EVEX_Vprorvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 14 50 01", 7, Code.EVEX_Vprorvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CDBD 14 50 01", 7, Code.EVEX_Vprorvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2CD28 14 50 01", 7, Code.EVEX_Vprorvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 14 50 01", 7, Code.EVEX_Vprorvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CDDD 14 50 01", 7, Code.EVEX_Vprorvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2CD48 14 50 01", 7, Code.EVEX_Vprorvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VprorvV_VX_k1_HX_WX_2_Data))]
		void Test32_VprorvV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VprorvV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 14 D3", 6, Code.EVEX_Vprorvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 14 D3", 6, Code.EVEX_Vprorvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 14 D3", 6, Code.EVEX_Vprorvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 14 D3", 6, Code.EVEX_Vprorvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 14 D3", 6, Code.EVEX_Vprorvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 14 D3", 6, Code.EVEX_Vprorvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 14 D3", 6, Code.EVEX_Vprorvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 14 D3", 6, Code.EVEX_Vprorvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 14 D3", 6, Code.EVEX_Vprorvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 14 D3", 6, Code.EVEX_Vprorvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 14 D3", 6, Code.EVEX_Vprorvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 14 D3", 6, Code.EVEX_Vprorvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VprorvV_VX_k1_HX_WX_1_Data))]
		void Test64_VprorvV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VprorvV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 14 50 01", 7, Code.EVEX_Vprorvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D9D 14 50 01", 7, Code.EVEX_Vprorvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 14 50 01", 7, Code.EVEX_Vprorvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 14 50 01", 7, Code.EVEX_Vprorvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24DBD 14 50 01", 7, Code.EVEX_Vprorvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 14 50 01", 7, Code.EVEX_Vprorvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 14 50 01", 7, Code.EVEX_Vprorvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24DDD 14 50 01", 7, Code.EVEX_Vprorvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 14 50 01", 7, Code.EVEX_Vprorvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 14 50 01", 7, Code.EVEX_Vprorvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD9D 14 50 01", 7, Code.EVEX_Vprorvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2CD08 14 50 01", 7, Code.EVEX_Vprorvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 14 50 01", 7, Code.EVEX_Vprorvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CDBD 14 50 01", 7, Code.EVEX_Vprorvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2CD28 14 50 01", 7, Code.EVEX_Vprorvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 14 50 01", 7, Code.EVEX_Vprorvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CDDD 14 50 01", 7, Code.EVEX_Vprorvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2CD48 14 50 01", 7, Code.EVEX_Vprorvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VprorvV_VX_k1_HX_WX_2_Data))]
		void Test64_VprorvV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VprorvV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 14 D3", 6, Code.EVEX_Vprorvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E20D0B 14 D3", 6, Code.EVEX_Vprorvd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 124D03 14 D3", 6, Code.EVEX_Vprorvd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B24D0B 14 D3", 6, Code.EVEX_Vprorvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F24D0B 14 D3", 6, Code.EVEX_Vprorvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 14 D3", 6, Code.EVEX_Vprorvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E20D2B 14 D3", 6, Code.EVEX_Vprorvd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 124D23 14 D3", 6, Code.EVEX_Vprorvd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 14 D3", 6, Code.EVEX_Vprorvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F24D2B 14 D3", 6, Code.EVEX_Vprorvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 14 D3", 6, Code.EVEX_Vprorvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E20D4B 14 D3", 6, Code.EVEX_Vprorvd_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 124D43 14 D3", 6, Code.EVEX_Vprorvd_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 14 D3", 6, Code.EVEX_Vprorvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F24D4B 14 D3", 6, Code.EVEX_Vprorvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 14 D3", 6, Code.EVEX_Vprorvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E28D0B 14 D3", 6, Code.EVEX_Vprorvq_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12CD03 14 D3", 6, Code.EVEX_Vprorvq_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CD0B 14 D3", 6, Code.EVEX_Vprorvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2CD0B 14 D3", 6, Code.EVEX_Vprorvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 14 D3", 6, Code.EVEX_Vprorvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E28D2B 14 D3", 6, Code.EVEX_Vprorvq_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12CD23 14 D3", 6, Code.EVEX_Vprorvq_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B 14 D3", 6, Code.EVEX_Vprorvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD2B 14 D3", 6, Code.EVEX_Vprorvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 14 D3", 6, Code.EVEX_Vprorvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E28D4B 14 D3", 6, Code.EVEX_Vprorvq_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12CD43 14 D3", 6, Code.EVEX_Vprorvq_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B 14 D3", 6, Code.EVEX_Vprorvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD4B 14 D3", 6, Code.EVEX_Vprorvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovusqwV_WX_k1z_VX_1_Data))]
		void Test16_VpmovusqwV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovusqwV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 14 50 01", 7, Code.EVEX_Vpmovusqw_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed32_UInt16, 4, false };
				yield return new object[] { "62 F27E08 14 50 01", 7, Code.EVEX_Vpmovusqw_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed32_UInt16, 4, false };

				yield return new object[] { "62 F27E2B 14 50 01", 7, Code.EVEX_Vpmovusqw_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed64_UInt16, 8, false };
				yield return new object[] { "62 F27E28 14 50 01", 7, Code.EVEX_Vpmovusqw_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed64_UInt16, 8, false };

				yield return new object[] { "62 F27E4B 14 50 01", 7, Code.EVEX_Vpmovusqw_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F27E48 14 50 01", 7, Code.EVEX_Vpmovusqw_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed128_UInt16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovusqwV_WX_k1z_VX_2_Data))]
		void Test16_VpmovusqwV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovusqwV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovusqwV_WX_k1z_VX_1_Data))]
		void Test32_VpmovusqwV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovusqwV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 14 50 01", 7, Code.EVEX_Vpmovusqw_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed32_UInt16, 4, false };
				yield return new object[] { "62 F27E08 14 50 01", 7, Code.EVEX_Vpmovusqw_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed32_UInt16, 4, false };

				yield return new object[] { "62 F27E2B 14 50 01", 7, Code.EVEX_Vpmovusqw_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed64_UInt16, 8, false };
				yield return new object[] { "62 F27E28 14 50 01", 7, Code.EVEX_Vpmovusqw_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed64_UInt16, 8, false };

				yield return new object[] { "62 F27E4B 14 50 01", 7, Code.EVEX_Vpmovusqw_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F27E48 14 50 01", 7, Code.EVEX_Vpmovusqw_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed128_UInt16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovusqwV_WX_k1z_VX_2_Data))]
		void Test32_VpmovusqwV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovusqwV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovusqwV_WX_k1z_VX_1_Data))]
		void Test64_VpmovusqwV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovusqwV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 14 50 01", 7, Code.EVEX_Vpmovusqw_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed32_UInt16, 4, false };
				yield return new object[] { "62 F27E08 14 50 01", 7, Code.EVEX_Vpmovusqw_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed32_UInt16, 4, false };

				yield return new object[] { "62 F27E2B 14 50 01", 7, Code.EVEX_Vpmovusqw_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed64_UInt16, 8, false };
				yield return new object[] { "62 F27E28 14 50 01", 7, Code.EVEX_Vpmovusqw_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed64_UInt16, 8, false };

				yield return new object[] { "62 F27E4B 14 50 01", 7, Code.EVEX_Vpmovusqw_WX_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F27E48 14 50 01", 7, Code.EVEX_Vpmovusqw_WX_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed128_UInt16, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovusqwV_WX_k1z_VX_2_Data))]
		void Test64_VpmovusqwV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovusqwV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E8B 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VX, Register.XMM3, Register.XMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E0B 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VX, Register.XMM27, Register.XMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E0B 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VX, Register.XMM19, Register.XMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E2B 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27EAB 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VY, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E2B 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VY, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E2B 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VY, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E4B 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VZ, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27ECB 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VZ, Register.XMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E4B 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VZ, Register.XMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E4B 14 D3", 6, Code.EVEX_Vpmovusqw_WX_k1z_VZ, Register.XMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VprolvV_VX_k1_HX_WX_1_Data))]
		void Test16_VprolvV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VprolvV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 15 50 01", 7, Code.EVEX_Vprolvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D9D 15 50 01", 7, Code.EVEX_Vprolvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 15 50 01", 7, Code.EVEX_Vprolvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 15 50 01", 7, Code.EVEX_Vprolvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24DBD 15 50 01", 7, Code.EVEX_Vprolvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 15 50 01", 7, Code.EVEX_Vprolvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 15 50 01", 7, Code.EVEX_Vprolvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24DDD 15 50 01", 7, Code.EVEX_Vprolvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 15 50 01", 7, Code.EVEX_Vprolvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 15 50 01", 7, Code.EVEX_Vprolvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD9D 15 50 01", 7, Code.EVEX_Vprolvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2CD08 15 50 01", 7, Code.EVEX_Vprolvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 15 50 01", 7, Code.EVEX_Vprolvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CDBD 15 50 01", 7, Code.EVEX_Vprolvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2CD28 15 50 01", 7, Code.EVEX_Vprolvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 15 50 01", 7, Code.EVEX_Vprolvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CDDD 15 50 01", 7, Code.EVEX_Vprolvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2CD48 15 50 01", 7, Code.EVEX_Vprolvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VprolvV_VX_k1_HX_WX_2_Data))]
		void Test16_VprolvV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VprolvV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 15 D3", 6, Code.EVEX_Vprolvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 15 D3", 6, Code.EVEX_Vprolvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 15 D3", 6, Code.EVEX_Vprolvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 15 D3", 6, Code.EVEX_Vprolvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 15 D3", 6, Code.EVEX_Vprolvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 15 D3", 6, Code.EVEX_Vprolvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 15 D3", 6, Code.EVEX_Vprolvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 15 D3", 6, Code.EVEX_Vprolvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 15 D3", 6, Code.EVEX_Vprolvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 15 D3", 6, Code.EVEX_Vprolvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 15 D3", 6, Code.EVEX_Vprolvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 15 D3", 6, Code.EVEX_Vprolvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VprolvV_VX_k1_HX_WX_1_Data))]
		void Test32_VprolvV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VprolvV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 15 50 01", 7, Code.EVEX_Vprolvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D9D 15 50 01", 7, Code.EVEX_Vprolvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 15 50 01", 7, Code.EVEX_Vprolvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 15 50 01", 7, Code.EVEX_Vprolvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24DBD 15 50 01", 7, Code.EVEX_Vprolvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 15 50 01", 7, Code.EVEX_Vprolvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 15 50 01", 7, Code.EVEX_Vprolvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24DDD 15 50 01", 7, Code.EVEX_Vprolvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 15 50 01", 7, Code.EVEX_Vprolvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 15 50 01", 7, Code.EVEX_Vprolvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD9D 15 50 01", 7, Code.EVEX_Vprolvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2CD08 15 50 01", 7, Code.EVEX_Vprolvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 15 50 01", 7, Code.EVEX_Vprolvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CDBD 15 50 01", 7, Code.EVEX_Vprolvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2CD28 15 50 01", 7, Code.EVEX_Vprolvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 15 50 01", 7, Code.EVEX_Vprolvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CDDD 15 50 01", 7, Code.EVEX_Vprolvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2CD48 15 50 01", 7, Code.EVEX_Vprolvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VprolvV_VX_k1_HX_WX_2_Data))]
		void Test32_VprolvV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VprolvV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 15 D3", 6, Code.EVEX_Vprolvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 15 D3", 6, Code.EVEX_Vprolvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 15 D3", 6, Code.EVEX_Vprolvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 15 D3", 6, Code.EVEX_Vprolvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 15 D3", 6, Code.EVEX_Vprolvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 15 D3", 6, Code.EVEX_Vprolvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 15 D3", 6, Code.EVEX_Vprolvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 15 D3", 6, Code.EVEX_Vprolvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 15 D3", 6, Code.EVEX_Vprolvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 15 D3", 6, Code.EVEX_Vprolvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 15 D3", 6, Code.EVEX_Vprolvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 15 D3", 6, Code.EVEX_Vprolvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VprolvV_VX_k1_HX_WX_1_Data))]
		void Test64_VprolvV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VprolvV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 15 50 01", 7, Code.EVEX_Vprolvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D9D 15 50 01", 7, Code.EVEX_Vprolvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 15 50 01", 7, Code.EVEX_Vprolvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 15 50 01", 7, Code.EVEX_Vprolvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24DBD 15 50 01", 7, Code.EVEX_Vprolvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 15 50 01", 7, Code.EVEX_Vprolvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 15 50 01", 7, Code.EVEX_Vprolvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24DDD 15 50 01", 7, Code.EVEX_Vprolvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 15 50 01", 7, Code.EVEX_Vprolvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 15 50 01", 7, Code.EVEX_Vprolvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD9D 15 50 01", 7, Code.EVEX_Vprolvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2CD08 15 50 01", 7, Code.EVEX_Vprolvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 15 50 01", 7, Code.EVEX_Vprolvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CDBD 15 50 01", 7, Code.EVEX_Vprolvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2CD28 15 50 01", 7, Code.EVEX_Vprolvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 15 50 01", 7, Code.EVEX_Vprolvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CDDD 15 50 01", 7, Code.EVEX_Vprolvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2CD48 15 50 01", 7, Code.EVEX_Vprolvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VprolvV_VX_k1_HX_WX_2_Data))]
		void Test64_VprolvV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VprolvV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 15 D3", 6, Code.EVEX_Vprolvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E20D0B 15 D3", 6, Code.EVEX_Vprolvd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 124D03 15 D3", 6, Code.EVEX_Vprolvd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B24D0B 15 D3", 6, Code.EVEX_Vprolvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F24D0B 15 D3", 6, Code.EVEX_Vprolvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 15 D3", 6, Code.EVEX_Vprolvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E20D2B 15 D3", 6, Code.EVEX_Vprolvd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 124D23 15 D3", 6, Code.EVEX_Vprolvd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 15 D3", 6, Code.EVEX_Vprolvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F24D2B 15 D3", 6, Code.EVEX_Vprolvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 15 D3", 6, Code.EVEX_Vprolvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E20D4B 15 D3", 6, Code.EVEX_Vprolvd_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 124D43 15 D3", 6, Code.EVEX_Vprolvd_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 15 D3", 6, Code.EVEX_Vprolvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F24D4B 15 D3", 6, Code.EVEX_Vprolvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 15 D3", 6, Code.EVEX_Vprolvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E28D0B 15 D3", 6, Code.EVEX_Vprolvq_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12CD03 15 D3", 6, Code.EVEX_Vprolvq_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CD0B 15 D3", 6, Code.EVEX_Vprolvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2CD0B 15 D3", 6, Code.EVEX_Vprolvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 15 D3", 6, Code.EVEX_Vprolvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E28D2B 15 D3", 6, Code.EVEX_Vprolvq_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12CD23 15 D3", 6, Code.EVEX_Vprolvq_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B 15 D3", 6, Code.EVEX_Vprolvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD2B 15 D3", 6, Code.EVEX_Vprolvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 15 D3", 6, Code.EVEX_Vprolvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E28D4B 15 D3", 6, Code.EVEX_Vprolvq_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12CD43 15 D3", 6, Code.EVEX_Vprolvq_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B 15 D3", 6, Code.EVEX_Vprolvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD4B 15 D3", 6, Code.EVEX_Vprolvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovusqdV_WX_k1z_VX_1_Data))]
		void Test16_VpmovusqdV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovusqdV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 15 50 01", 7, Code.EVEX_Vpmovusqd_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_UInt32, 8, false };
				yield return new object[] { "62 F27E08 15 50 01", 7, Code.EVEX_Vpmovusqd_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_UInt32, 8, false };

				yield return new object[] { "62 F27E2B 15 50 01", 7, Code.EVEX_Vpmovusqd_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F27E28 15 50 01", 7, Code.EVEX_Vpmovusqd_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F27E4B 15 50 01", 7, Code.EVEX_Vpmovusqd_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F27E48 15 50 01", 7, Code.EVEX_Vpmovusqd_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_UInt32, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmovusqdV_WX_k1z_VX_2_Data))]
		void Test16_VpmovusqdV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmovusqdV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 15 D3", 6, Code.EVEX_Vpmovusqd_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 15 D3", 6, Code.EVEX_Vpmovusqd_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 15 D3", 6, Code.EVEX_Vpmovusqd_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 15 D3", 6, Code.EVEX_Vpmovusqd_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 15 D3", 6, Code.EVEX_Vpmovusqd_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 15 D3", 6, Code.EVEX_Vpmovusqd_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovusqdV_WX_k1z_VX_1_Data))]
		void Test32_VpmovusqdV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovusqdV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 15 50 01", 7, Code.EVEX_Vpmovusqd_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_UInt32, 8, false };
				yield return new object[] { "62 F27E08 15 50 01", 7, Code.EVEX_Vpmovusqd_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_UInt32, 8, false };

				yield return new object[] { "62 F27E2B 15 50 01", 7, Code.EVEX_Vpmovusqd_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F27E28 15 50 01", 7, Code.EVEX_Vpmovusqd_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F27E4B 15 50 01", 7, Code.EVEX_Vpmovusqd_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F27E48 15 50 01", 7, Code.EVEX_Vpmovusqd_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_UInt32, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmovusqdV_WX_k1z_VX_2_Data))]
		void Test32_VpmovusqdV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmovusqdV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 15 D3", 6, Code.EVEX_Vpmovusqd_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27E8B 15 D3", 6, Code.EVEX_Vpmovusqd_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E2B 15 D3", 6, Code.EVEX_Vpmovusqd_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27EAB 15 D3", 6, Code.EVEX_Vpmovusqd_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27E4B 15 D3", 6, Code.EVEX_Vpmovusqd_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27ECB 15 D3", 6, Code.EVEX_Vpmovusqd_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovusqdV_WX_k1z_VX_1_Data))]
		void Test64_VpmovusqdV_WX_k1z_VX_1(string hexBytes, int byteLength, Code code, Register reg, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovusqdV_WX_k1z_VX_1_Data {
			get {
				yield return new object[] { "62 F27E0B 15 50 01", 7, Code.EVEX_Vpmovusqd_WX_k1z_VX, Register.XMM2, Register.K3, MemorySize.Packed64_UInt32, 8, false };
				yield return new object[] { "62 F27E08 15 50 01", 7, Code.EVEX_Vpmovusqd_WX_k1z_VX, Register.XMM2, Register.None, MemorySize.Packed64_UInt32, 8, false };

				yield return new object[] { "62 F27E2B 15 50 01", 7, Code.EVEX_Vpmovusqd_WX_k1z_VY, Register.YMM2, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F27E28 15 50 01", 7, Code.EVEX_Vpmovusqd_WX_k1z_VY, Register.YMM2, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F27E4B 15 50 01", 7, Code.EVEX_Vpmovusqd_WY_k1z_VZ, Register.ZMM2, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F27E48 15 50 01", 7, Code.EVEX_Vpmovusqd_WY_k1z_VZ, Register.ZMM2, Register.None, MemorySize.Packed256_UInt32, 32, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmovusqdV_WX_k1z_VX_2_Data))]
		void Test64_VpmovusqdV_WX_k1z_VX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmovusqdV_WX_k1z_VX_2_Data {
			get {
				yield return new object[] { "62 F27E0B 15 D3", 6, Code.EVEX_Vpmovusqd_WX_k1z_VX, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27E8B 15 D3", 6, Code.EVEX_Vpmovusqd_WX_k1z_VX, Register.XMM3, Register.XMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E0B 15 D3", 6, Code.EVEX_Vpmovusqd_WX_k1z_VX, Register.XMM27, Register.XMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E0B 15 D3", 6, Code.EVEX_Vpmovusqd_WX_k1z_VX, Register.XMM19, Register.XMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E2B 15 D3", 6, Code.EVEX_Vpmovusqd_WX_k1z_VY, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27EAB 15 D3", 6, Code.EVEX_Vpmovusqd_WX_k1z_VY, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E2B 15 D3", 6, Code.EVEX_Vpmovusqd_WX_k1z_VY, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E2B 15 D3", 6, Code.EVEX_Vpmovusqd_WX_k1z_VY, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F27E4B 15 D3", 6, Code.EVEX_Vpmovusqd_WY_k1z_VZ, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E27ECB 15 D3", 6, Code.EVEX_Vpmovusqd_WY_k1z_VZ, Register.YMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 127E4B 15 D3", 6, Code.EVEX_Vpmovusqd_WY_k1z_VZ, Register.YMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B27E4B 15 D3", 6, Code.EVEX_Vpmovusqd_WY_k1z_VZ, Register.YMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpermpV_VX_HX_WX_1_Data))]
		void Test16_VpermpV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpermpV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E24D 16 10", 5, Code.VEX_Vpermps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpermpV_VX_HX_WX_2_Data))]
		void Test16_VpermpV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpermpV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E24D 16 D3", 5, Code.VEX_Vpermps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpermpV_VX_HX_WX_1_Data))]
		void Test32_VpermpV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpermpV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E24D 16 10", 5, Code.VEX_Vpermps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpermpV_VX_HX_WX_2_Data))]
		void Test32_VpermpV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpermpV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E24D 16 D3", 5, Code.VEX_Vpermps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpermpV_VX_HX_WX_1_Data))]
		void Test64_VpermpV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpermpV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E24D 16 10", 5, Code.VEX_Vpermps_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpermpV_VX_HX_WX_2_Data))]
		void Test64_VpermpV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpermpV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E24D 16 D3", 5, Code.VEX_Vpermps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4624D 16 D3", 5, Code.VEX_Vpermps_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E20D 16 D3", 5, Code.VEX_Vpermps_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C24D 16 D3", 5, Code.VEX_Vpermps_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpermpV_VX_k1_HX_WX_1_Data))]
		void Test16_VpermpV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpermpV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D2B 16 50 01", 7, Code.EVEX_Vpermps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 16 50 01", 7, Code.EVEX_Vpermps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 16 50 01", 7, Code.EVEX_Vpermps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 16 50 01", 7, Code.EVEX_Vpermps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 16 50 01", 7, Code.EVEX_Vpermps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 16 50 01", 7, Code.EVEX_Vpermps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD2B 16 50 01", 7, Code.EVEX_Vpermpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 16 50 01", 7, Code.EVEX_Vpermpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 16 50 01", 7, Code.EVEX_Vpermpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 16 50 01", 7, Code.EVEX_Vpermpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 16 50 01", 7, Code.EVEX_Vpermpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 16 50 01", 7, Code.EVEX_Vpermpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpermpV_VX_k1_HX_WX_2_Data))]
		void Test16_VpermpV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpermpV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24DAB 16 D3", 6, Code.EVEX_Vpermps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 16 D3", 6, Code.EVEX_Vpermps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 16 D3", 6, Code.EVEX_Vpermps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 16 D3", 6, Code.EVEX_Vpermps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 16 D3", 6, Code.EVEX_Vpermpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 16 D3", 6, Code.EVEX_Vpermpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 16 D3", 6, Code.EVEX_Vpermpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 16 D3", 6, Code.EVEX_Vpermpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpermpV_VX_k1_HX_WX_1_Data))]
		void Test32_VpermpV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpermpV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D2B 16 50 01", 7, Code.EVEX_Vpermps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 16 50 01", 7, Code.EVEX_Vpermps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 16 50 01", 7, Code.EVEX_Vpermps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 16 50 01", 7, Code.EVEX_Vpermps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 16 50 01", 7, Code.EVEX_Vpermps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 16 50 01", 7, Code.EVEX_Vpermps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD2B 16 50 01", 7, Code.EVEX_Vpermpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 16 50 01", 7, Code.EVEX_Vpermpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 16 50 01", 7, Code.EVEX_Vpermpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 16 50 01", 7, Code.EVEX_Vpermpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 16 50 01", 7, Code.EVEX_Vpermpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 16 50 01", 7, Code.EVEX_Vpermpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpermpV_VX_k1_HX_WX_2_Data))]
		void Test32_VpermpV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpermpV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24DAB 16 D3", 6, Code.EVEX_Vpermps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 16 D3", 6, Code.EVEX_Vpermps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 16 D3", 6, Code.EVEX_Vpermps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 16 D3", 6, Code.EVEX_Vpermps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 16 D3", 6, Code.EVEX_Vpermpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 16 D3", 6, Code.EVEX_Vpermpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 16 D3", 6, Code.EVEX_Vpermpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 16 D3", 6, Code.EVEX_Vpermpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpermpV_VX_k1_HX_WX_1_Data))]
		void Test64_VpermpV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpermpV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D2B 16 50 01", 7, Code.EVEX_Vpermps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F24DBD 16 50 01", 7, Code.EVEX_Vpermps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float32, 4, true };
				yield return new object[] { "62 F24D28 16 50 01", 7, Code.EVEX_Vpermps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float32, 32, false };

				yield return new object[] { "62 F24D4B 16 50 01", 7, Code.EVEX_Vpermps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F24DDD 16 50 01", 7, Code.EVEX_Vpermps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float32, 4, true };
				yield return new object[] { "62 F24D48 16 50 01", 7, Code.EVEX_Vpermps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float32, 64, false };

				yield return new object[] { "62 F2CD2B 16 50 01", 7, Code.EVEX_Vpermpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2CDBD 16 50 01", 7, Code.EVEX_Vpermpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Float64, 8, true };
				yield return new object[] { "62 F2CD28 16 50 01", 7, Code.EVEX_Vpermpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Float64, 32, false };

				yield return new object[] { "62 F2CD4B 16 50 01", 7, Code.EVEX_Vpermpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2CDDD 16 50 01", 7, Code.EVEX_Vpermpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Float64, 8, true };
				yield return new object[] { "62 F2CD48 16 50 01", 7, Code.EVEX_Vpermpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Float64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpermpV_VX_k1_HX_WX_2_Data))]
		void Test64_VpermpV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpermpV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24DAB 16 D3", 6, Code.EVEX_Vpermps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E20D2B 16 D3", 6, Code.EVEX_Vpermps_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 124D23 16 D3", 6, Code.EVEX_Vpermps_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 16 D3", 6, Code.EVEX_Vpermps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F24D2B 16 D3", 6, Code.EVEX_Vpermps_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 16 D3", 6, Code.EVEX_Vpermps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E20D4B 16 D3", 6, Code.EVEX_Vpermps_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 124D43 16 D3", 6, Code.EVEX_Vpermps_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 16 D3", 6, Code.EVEX_Vpermps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F24D4B 16 D3", 6, Code.EVEX_Vpermps_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 16 D3", 6, Code.EVEX_Vpermpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E28D2B 16 D3", 6, Code.EVEX_Vpermpd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12CD23 16 D3", 6, Code.EVEX_Vpermpd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B 16 D3", 6, Code.EVEX_Vpermpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD2B 16 D3", 6, Code.EVEX_Vpermpd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 16 D3", 6, Code.EVEX_Vpermpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E28D4B 16 D3", 6, Code.EVEX_Vpermpd_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12CD43 16 D3", 6, Code.EVEX_Vpermpd_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B 16 D3", 6, Code.EVEX_Vpermpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD4B 16 D3", 6, Code.EVEX_Vpermpd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PtestV_VX_WX_Reg_1_Data))]
		void Test16_PtestV_VX_WX_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_PtestV_VX_WX_Reg_1_Data {
			get {
				yield return new object[] { "66 0F3817 08", 5, Code.Ptest_VX_WX, Register.XMM1, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PtestV_VX_WX_Reg_2_Data))]
		void Test16_PtestV_VX_WX_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test16_PtestV_VX_WX_Reg_2_Data {
			get {
				yield return new object[] { "66 0F3817 CD", 5, Code.Ptest_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PtestV_VX_WX_Reg_1_Data))]
		void Test32_PtestV_VX_WX_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_PtestV_VX_WX_Reg_1_Data {
			get {
				yield return new object[] { "66 0F3817 08", 5, Code.Ptest_VX_WX, Register.XMM1, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PtestV_VX_WX_Reg_2_Data))]
		void Test32_PtestV_VX_WX_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test32_PtestV_VX_WX_Reg_2_Data {
			get {
				yield return new object[] { "66 0F3817 CD", 5, Code.Ptest_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PtestV_VX_WX_Reg_1_Data))]
		void Test64_PtestV_VX_WX_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_PtestV_VX_WX_Reg_1_Data {
			get {
				yield return new object[] { "66 0F3817 08", 5, Code.Ptest_VX_WX, Register.XMM1, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PtestV_VX_WX_Reg_2_Data))]
		void Test64_PtestV_VX_WX_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test64_PtestV_VX_WX_Reg_2_Data {
			get {
				yield return new object[] { "66 0F3817 CD", 5, Code.Ptest_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3817 CD", 6, Code.Ptest_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3817 CD", 6, Code.Ptest_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3817 CD", 6, Code.Ptest_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VptestV_Reg_RegMem_1_Data))]
		void Test16_VptestV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_VptestV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E279 17 10", 5, Code.VEX_Vptest_VX_WX, Register.XMM2, MemorySize.UInt128 };
				yield return new object[] { "C4E2F9 17 10", 5, Code.VEX_Vptest_VX_WX, Register.XMM2, MemorySize.UInt128 };

				yield return new object[] { "C4E27D 17 10", 5, Code.VEX_Vptest_VY_WY, Register.YMM2, MemorySize.UInt256 };
				yield return new object[] { "C4E2FD 17 10", 5, Code.VEX_Vptest_VY_WY, Register.YMM2, MemorySize.UInt256 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VptestV_Reg_RegMem_2_Data))]
		void Test16_VptestV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test16_VptestV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E279 17 CD", 5, Code.VEX_Vptest_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 17 CD", 5, Code.VEX_Vptest_VY_WY, Register.YMM1, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VptestV_Reg_RegMem_1_Data))]
		void Test32_VptestV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_VptestV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E279 17 10", 5, Code.VEX_Vptest_VX_WX, Register.XMM2, MemorySize.UInt128 };
				yield return new object[] { "C4E2F9 17 10", 5, Code.VEX_Vptest_VX_WX, Register.XMM2, MemorySize.UInt128 };

				yield return new object[] { "C4E27D 17 10", 5, Code.VEX_Vptest_VY_WY, Register.YMM2, MemorySize.UInt256 };
				yield return new object[] { "C4E2FD 17 10", 5, Code.VEX_Vptest_VY_WY, Register.YMM2, MemorySize.UInt256 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VptestV_Reg_RegMem_2_Data))]
		void Test32_VptestV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test32_VptestV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E279 17 CD", 5, Code.VEX_Vptest_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E27D 17 CD", 5, Code.VEX_Vptest_VY_WY, Register.YMM1, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VptestV_Reg_RegMem_1_Data))]
		void Test64_VptestV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_VptestV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "C4E279 17 10", 5, Code.VEX_Vptest_VX_WX, Register.XMM2, MemorySize.UInt128 };
				yield return new object[] { "C4E2F9 17 10", 5, Code.VEX_Vptest_VX_WX, Register.XMM2, MemorySize.UInt128 };

				yield return new object[] { "C4E27D 17 10", 5, Code.VEX_Vptest_VY_WY, Register.YMM2, MemorySize.UInt256 };
				yield return new object[] { "C4E2FD 17 10", 5, Code.VEX_Vptest_VY_WY, Register.YMM2, MemorySize.UInt256 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VptestV_Reg_RegMem_2_Data))]
		void Test64_VptestV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		}
		public static IEnumerable<object[]> Test64_VptestV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "C4E279 17 CD", 5, Code.VEX_Vptest_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 17 CD", 5, Code.VEX_Vptest_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 17 CD", 5, Code.VEX_Vptest_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 17 CD", 5, Code.VEX_Vptest_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E27D 17 CD", 5, Code.VEX_Vptest_VY_WY, Register.YMM1, Register.YMM5 };
				yield return new object[] { "C4627D 17 CD", 5, Code.VEX_Vptest_VY_WY, Register.YMM9, Register.YMM5 };
				yield return new object[] { "C4C27D 17 CD", 5, Code.VEX_Vptest_VY_WY, Register.YMM1, Register.YMM13 };
				yield return new object[] { "C4427D 17 CD", 5, Code.VEX_Vptest_VY_WY, Register.YMM9, Register.YMM13 };
			}
		}
	}
}
