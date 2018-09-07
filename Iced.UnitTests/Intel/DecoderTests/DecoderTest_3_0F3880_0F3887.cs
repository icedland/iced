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
	public sealed class DecoderTest_3_0F3880_0F3887 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_GvM_1_Data))]
		void Test16_GvM_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_GvM_1_Data {
			get {
				yield return new object[] { "66 0F3880 10", 5, Code.Invept_Gd_M, Register.EDX, MemorySize.UInt128 };

				yield return new object[] { "66 0F3881 10", 5, Code.Invvpid_Gd_M, Register.EDX, MemorySize.UInt128 };

				yield return new object[] { "66 0F3882 10", 5, Code.Invpcid_Gd_M, Register.EDX, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_GvM_1_Data))]
		void Test32_GvM_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_GvM_1_Data {
			get {
				yield return new object[] { "66 0F3880 10", 5, Code.Invept_Gd_M, Register.EDX, MemorySize.UInt128 };

				yield return new object[] { "66 0F3881 10", 5, Code.Invvpid_Gd_M, Register.EDX, MemorySize.UInt128 };

				yield return new object[] { "66 0F3882 10", 5, Code.Invpcid_Gd_M, Register.EDX, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_GvM_1_Data))]
		void Test64_GvM_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_GvM_1_Data {
			get {
				yield return new object[] { "66 0F3880 10", 5, Code.Invept_Gq_M, Register.RDX, MemorySize.UInt128 };
				yield return new object[] { "66 44 0F3880 10", 6, Code.Invept_Gq_M, Register.R10, MemorySize.UInt128 };

				yield return new object[] { "66 0F3881 10", 5, Code.Invvpid_Gq_M, Register.RDX, MemorySize.UInt128 };
				yield return new object[] { "66 44 0F3881 10", 6, Code.Invvpid_Gq_M, Register.R10, MemorySize.UInt128 };

				yield return new object[] { "66 0F3882 10", 5, Code.Invpcid_Gq_M, Register.RDX, MemorySize.UInt128 };
				yield return new object[] { "66 44 0F3882 10", 6, Code.Invpcid_Gq_M, Register.R10, MemorySize.UInt128 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmultishiftqbV_VX_k1_HX_WX_1_Data))]
		void Test16_VpmultishiftqbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpmultishiftqbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD0B 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD9D 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2CD08 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD2B 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CDBD 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2CD28 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD4B 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CDDD 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2CD48 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmultishiftqbV_VX_k1_HX_WX_2_Data))]
		void Test16_VpmultishiftqbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpmultishiftqbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD8B 83 D3", 6, Code.EVEX_Vpmultishiftqb_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 83 D3", 6, Code.EVEX_Vpmultishiftqb_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 83 D3", 6, Code.EVEX_Vpmultishiftqb_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 83 D3", 6, Code.EVEX_Vpmultishiftqb_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 83 D3", 6, Code.EVEX_Vpmultishiftqb_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 83 D3", 6, Code.EVEX_Vpmultishiftqb_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmultishiftqbV_VX_k1_HX_WX_1_Data))]
		void Test32_VpmultishiftqbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpmultishiftqbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD0B 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD9D 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2CD08 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD2B 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CDBD 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2CD28 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD4B 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CDDD 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2CD48 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmultishiftqbV_VX_k1_HX_WX_2_Data))]
		void Test32_VpmultishiftqbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpmultishiftqbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD8B 83 D3", 6, Code.EVEX_Vpmultishiftqb_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 83 D3", 6, Code.EVEX_Vpmultishiftqb_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 83 D3", 6, Code.EVEX_Vpmultishiftqb_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 83 D3", 6, Code.EVEX_Vpmultishiftqb_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 83 D3", 6, Code.EVEX_Vpmultishiftqb_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 83 D3", 6, Code.EVEX_Vpmultishiftqb_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmultishiftqbV_VX_k1_HX_WX_1_Data))]
		void Test64_VpmultishiftqbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpmultishiftqbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F2CD0B 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD9D 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2CD08 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD2B 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CDBD 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2CD28 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD4B 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CDDD 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2CD48 83 50 01", 7, Code.EVEX_Vpmultishiftqb_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmultishiftqbV_VX_k1_HX_WX_2_Data))]
		void Test64_VpmultishiftqbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpmultishiftqbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F2CD8B 83 D3", 6, Code.EVEX_Vpmultishiftqb_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E28D0B 83 D3", 6, Code.EVEX_Vpmultishiftqb_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12CD03 83 D3", 6, Code.EVEX_Vpmultishiftqb_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CD0B 83 D3", 6, Code.EVEX_Vpmultishiftqb_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2CD0B 83 D3", 6, Code.EVEX_Vpmultishiftqb_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 83 D3", 6, Code.EVEX_Vpmultishiftqb_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E28D2B 83 D3", 6, Code.EVEX_Vpmultishiftqb_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12CD23 83 D3", 6, Code.EVEX_Vpmultishiftqb_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B 83 D3", 6, Code.EVEX_Vpmultishiftqb_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD2B 83 D3", 6, Code.EVEX_Vpmultishiftqb_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 83 D3", 6, Code.EVEX_Vpmultishiftqb_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E28D4B 83 D3", 6, Code.EVEX_Vpmultishiftqb_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12CD43 83 D3", 6, Code.EVEX_Vpmultishiftqb_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B 83 D3", 6, Code.EVEX_Vpmultishiftqb_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD4B 83 D3", 6, Code.EVEX_Vpmultishiftqb_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}
	}
}
