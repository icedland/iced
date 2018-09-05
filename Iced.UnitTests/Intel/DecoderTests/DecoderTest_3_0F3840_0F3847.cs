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
	public sealed class DecoderTest_3_0F3840_0F3847 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_PmulldV_VX_WX_1_Data))]
		void Test16_PmulldV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_PmulldV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F3840 08", 5, Code.Pmulld_VX_WX, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmulldV_VX_WX_2_Data))]
		void Test16_PmulldV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PmulldV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F3840 CD", 5, Code.Pmulld_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmulldV_VX_WX_1_Data))]
		void Test32_PmulldV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_PmulldV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F3840 08", 5, Code.Pmulld_VX_WX, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmulldV_VX_WX_2_Data))]
		void Test32_PmulldV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PmulldV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F3840 CD", 5, Code.Pmulld_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmulldV_VX_WX_1_Data))]
		void Test64_PmulldV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_PmulldV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F3840 08", 5, Code.Pmulld_VX_WX, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmulldV_VX_WX_2_Data))]
		void Test64_PmulldV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PmulldV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F3840 CD", 5, Code.Pmulld_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3840 CD", 6, Code.Pmulld_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3840 CD", 6, Code.Pmulld_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3840 CD", 6, Code.Pmulld_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmulldV_VX_HX_WX_1_Data))]
		void Test16_VpmulldV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmulldV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 40 10", 5, Code.VEX_Vpmulld_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E24D 40 10", 5, Code.VEX_Vpmulld_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2C9 40 10", 5, Code.VEX_Vpmulld_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2CD 40 10", 5, Code.VEX_Vpmulld_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmulldV_VX_HX_WX_2_Data))]
		void Test16_VpmulldV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpmulldV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 40 D3", 5, Code.VEX_Vpmulld_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 40 D3", 5, Code.VEX_Vpmulld_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmulldV_VX_HX_WX_1_Data))]
		void Test32_VpmulldV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmulldV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 40 10", 5, Code.VEX_Vpmulld_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E24D 40 10", 5, Code.VEX_Vpmulld_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2C9 40 10", 5, Code.VEX_Vpmulld_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2CD 40 10", 5, Code.VEX_Vpmulld_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmulldV_VX_HX_WX_2_Data))]
		void Test32_VpmulldV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpmulldV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 40 D3", 5, Code.VEX_Vpmulld_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 40 D3", 5, Code.VEX_Vpmulld_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmulldV_VX_HX_WX_1_Data))]
		void Test64_VpmulldV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmulldV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 40 10", 5, Code.VEX_Vpmulld_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E24D 40 10", 5, Code.VEX_Vpmulld_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2C9 40 10", 5, Code.VEX_Vpmulld_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2CD 40 10", 5, Code.VEX_Vpmulld_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmulldV_VX_HX_WX_2_Data))]
		void Test64_VpmulldV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpmulldV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 40 D3", 5, Code.VEX_Vpmulld_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 40 D3", 5, Code.VEX_Vpmulld_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C46249 40 D3", 5, Code.VEX_Vpmulld_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4624D 40 D3", 5, Code.VEX_Vpmulld_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E209 40 D3", 5, Code.VEX_Vpmulld_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E20D 40 D3", 5, Code.VEX_Vpmulld_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C249 40 D3", 5, Code.VEX_Vpmulld_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C24D 40 D3", 5, Code.VEX_Vpmulld_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmulldV_VX_k1_HX_WX_1_Data))]
		void Test16_VpmulldV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpmulldV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 40 50 01", 7, Code.EVEX_Vpmulld_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F24D9D 40 50 01", 7, Code.EVEX_Vpmulld_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F24D08 40 50 01", 7, Code.EVEX_Vpmulld_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F24D2B 40 50 01", 7, Code.EVEX_Vpmulld_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F24DBD 40 50 01", 7, Code.EVEX_Vpmulld_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F24D28 40 50 01", 7, Code.EVEX_Vpmulld_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F24D4B 40 50 01", 7, Code.EVEX_Vpmulld_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F24DDD 40 50 01", 7, Code.EVEX_Vpmulld_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F24D48 40 50 01", 7, Code.EVEX_Vpmulld_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F2CD0B 40 50 01", 7, Code.EVEX_Vpmullq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD9D 40 50 01", 7, Code.EVEX_Vpmullq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2CD08 40 50 01", 7, Code.EVEX_Vpmullq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD2B 40 50 01", 7, Code.EVEX_Vpmullq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CDBD 40 50 01", 7, Code.EVEX_Vpmullq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2CD28 40 50 01", 7, Code.EVEX_Vpmullq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD4B 40 50 01", 7, Code.EVEX_Vpmullq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CDDD 40 50 01", 7, Code.EVEX_Vpmullq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2CD48 40 50 01", 7, Code.EVEX_Vpmullq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmulldV_VX_k1_HX_WX_2_Data))]
		void Test16_VpmulldV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpmulldV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 40 D3", 6, Code.EVEX_Vpmulld_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 40 D3", 6, Code.EVEX_Vpmulld_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 40 D3", 6, Code.EVEX_Vpmulld_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 40 D3", 6, Code.EVEX_Vpmulld_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 40 D3", 6, Code.EVEX_Vpmulld_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 40 D3", 6, Code.EVEX_Vpmulld_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 40 D3", 6, Code.EVEX_Vpmullq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 40 D3", 6, Code.EVEX_Vpmullq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 40 D3", 6, Code.EVEX_Vpmullq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 40 D3", 6, Code.EVEX_Vpmullq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 40 D3", 6, Code.EVEX_Vpmullq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 40 D3", 6, Code.EVEX_Vpmullq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmulldV_VX_k1_HX_WX_1_Data))]
		void Test32_VpmulldV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpmulldV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 40 50 01", 7, Code.EVEX_Vpmulld_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F24D9D 40 50 01", 7, Code.EVEX_Vpmulld_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F24D08 40 50 01", 7, Code.EVEX_Vpmulld_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F24D2B 40 50 01", 7, Code.EVEX_Vpmulld_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F24DBD 40 50 01", 7, Code.EVEX_Vpmulld_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F24D28 40 50 01", 7, Code.EVEX_Vpmulld_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F24D4B 40 50 01", 7, Code.EVEX_Vpmulld_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F24DDD 40 50 01", 7, Code.EVEX_Vpmulld_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F24D48 40 50 01", 7, Code.EVEX_Vpmulld_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F2CD0B 40 50 01", 7, Code.EVEX_Vpmullq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD9D 40 50 01", 7, Code.EVEX_Vpmullq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2CD08 40 50 01", 7, Code.EVEX_Vpmullq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD2B 40 50 01", 7, Code.EVEX_Vpmullq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CDBD 40 50 01", 7, Code.EVEX_Vpmullq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2CD28 40 50 01", 7, Code.EVEX_Vpmullq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD4B 40 50 01", 7, Code.EVEX_Vpmullq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CDDD 40 50 01", 7, Code.EVEX_Vpmullq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2CD48 40 50 01", 7, Code.EVEX_Vpmullq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmulldV_VX_k1_HX_WX_2_Data))]
		void Test32_VpmulldV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpmulldV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 40 D3", 6, Code.EVEX_Vpmulld_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 40 D3", 6, Code.EVEX_Vpmulld_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 40 D3", 6, Code.EVEX_Vpmulld_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 40 D3", 6, Code.EVEX_Vpmulld_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 40 D3", 6, Code.EVEX_Vpmulld_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 40 D3", 6, Code.EVEX_Vpmulld_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 40 D3", 6, Code.EVEX_Vpmullq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 40 D3", 6, Code.EVEX_Vpmullq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 40 D3", 6, Code.EVEX_Vpmullq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 40 D3", 6, Code.EVEX_Vpmullq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 40 D3", 6, Code.EVEX_Vpmullq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 40 D3", 6, Code.EVEX_Vpmullq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmulldV_VX_k1_HX_WX_1_Data))]
		void Test64_VpmulldV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpmulldV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 40 50 01", 7, Code.EVEX_Vpmulld_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F24D9D 40 50 01", 7, Code.EVEX_Vpmulld_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F24D08 40 50 01", 7, Code.EVEX_Vpmulld_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F24D2B 40 50 01", 7, Code.EVEX_Vpmulld_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F24DBD 40 50 01", 7, Code.EVEX_Vpmulld_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F24D28 40 50 01", 7, Code.EVEX_Vpmulld_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F24D4B 40 50 01", 7, Code.EVEX_Vpmulld_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F24DDD 40 50 01", 7, Code.EVEX_Vpmulld_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F24D48 40 50 01", 7, Code.EVEX_Vpmulld_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };

				yield return new object[] { "62 F2CD0B 40 50 01", 7, Code.EVEX_Vpmullq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F2CD9D 40 50 01", 7, Code.EVEX_Vpmullq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F2CD08 40 50 01", 7, Code.EVEX_Vpmullq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F2CD2B 40 50 01", 7, Code.EVEX_Vpmullq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F2CDBD 40 50 01", 7, Code.EVEX_Vpmullq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F2CD28 40 50 01", 7, Code.EVEX_Vpmullq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F2CD4B 40 50 01", 7, Code.EVEX_Vpmullq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F2CDDD 40 50 01", 7, Code.EVEX_Vpmullq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F2CD48 40 50 01", 7, Code.EVEX_Vpmullq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmulldV_VX_k1_HX_WX_2_Data))]
		void Test64_VpmulldV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpmulldV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 40 D3", 6, Code.EVEX_Vpmulld_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E20D0B 40 D3", 6, Code.EVEX_Vpmulld_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 124D03 40 D3", 6, Code.EVEX_Vpmulld_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B24D0B 40 D3", 6, Code.EVEX_Vpmulld_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F24D0B 40 D3", 6, Code.EVEX_Vpmulld_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 40 D3", 6, Code.EVEX_Vpmulld_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E20D2B 40 D3", 6, Code.EVEX_Vpmulld_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 124D23 40 D3", 6, Code.EVEX_Vpmulld_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 40 D3", 6, Code.EVEX_Vpmulld_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F24D2B 40 D3", 6, Code.EVEX_Vpmulld_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 40 D3", 6, Code.EVEX_Vpmulld_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E20D4B 40 D3", 6, Code.EVEX_Vpmulld_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 124D43 40 D3", 6, Code.EVEX_Vpmulld_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 40 D3", 6, Code.EVEX_Vpmulld_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F24D4B 40 D3", 6, Code.EVEX_Vpmulld_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 40 D3", 6, Code.EVEX_Vpmullq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E28D0B 40 D3", 6, Code.EVEX_Vpmullq_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12CD03 40 D3", 6, Code.EVEX_Vpmullq_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CD0B 40 D3", 6, Code.EVEX_Vpmullq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2CD0B 40 D3", 6, Code.EVEX_Vpmullq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 40 D3", 6, Code.EVEX_Vpmullq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E28D2B 40 D3", 6, Code.EVEX_Vpmullq_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12CD23 40 D3", 6, Code.EVEX_Vpmullq_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B 40 D3", 6, Code.EVEX_Vpmullq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD2B 40 D3", 6, Code.EVEX_Vpmullq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 40 D3", 6, Code.EVEX_Vpmullq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E28D4B 40 D3", 6, Code.EVEX_Vpmullq_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12CD43 40 D3", 6, Code.EVEX_Vpmullq_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B 40 D3", 6, Code.EVEX_Vpmullq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD4B 40 D3", 6, Code.EVEX_Vpmullq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VphminposuwV_Reg_RegMem_1_Data))]
		void Test16_VphminposuwV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test16_VphminposuwV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3841 08", 5, Code.Phminposuw_VX_WX, Register.XMM1, MemorySize.Packed128_UInt16 };

				yield return new object[] { "C4E279 41 10", 5, Code.VEX_Vphminposuw_VX_WX, Register.XMM2, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E2F9 41 10", 5, Code.VEX_Vphminposuw_VX_WX, Register.XMM2, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VphminposuwV_Reg_RegMem_2_Data))]
		void Test16_VphminposuwV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_VphminposuwV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3841 CD", 5, Code.Phminposuw_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 41 CD", 5, Code.VEX_Vphminposuw_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VphminposuwV_Reg_RegMem_1_Data))]
		void Test32_VphminposuwV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test32_VphminposuwV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3841 08", 5, Code.Phminposuw_VX_WX, Register.XMM1, MemorySize.Packed128_UInt16 };

				yield return new object[] { "C4E279 41 10", 5, Code.VEX_Vphminposuw_VX_WX, Register.XMM2, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E2F9 41 10", 5, Code.VEX_Vphminposuw_VX_WX, Register.XMM2, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VphminposuwV_Reg_RegMem_2_Data))]
		void Test32_VphminposuwV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_VphminposuwV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3841 CD", 5, Code.Phminposuw_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C4E279 41 CD", 5, Code.VEX_Vphminposuw_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VphminposuwV_Reg_RegMem_1_Data))]
		void Test64_VphminposuwV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
		public static IEnumerable<object[]> Test64_VphminposuwV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F3841 08", 5, Code.Phminposuw_VX_WX, Register.XMM1, MemorySize.Packed128_UInt16 };

				yield return new object[] { "C4E279 41 10", 5, Code.VEX_Vphminposuw_VX_WX, Register.XMM2, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E2F9 41 10", 5, Code.VEX_Vphminposuw_VX_WX, Register.XMM2, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VphminposuwV_Reg_RegMem_2_Data))]
		void Test64_VphminposuwV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_VphminposuwV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F3841 CD", 5, Code.Phminposuw_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3841 CD", 6, Code.Phminposuw_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3841 CD", 6, Code.Phminposuw_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3841 CD", 6, Code.Phminposuw_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C4E279 41 CD", 5, Code.VEX_Vphminposuw_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C46279 41 CD", 5, Code.VEX_Vphminposuw_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C279 41 CD", 5, Code.VEX_Vphminposuw_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44279 41 CD", 5, Code.VEX_Vphminposuw_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VgetexppVReg_k1_RegMem_EVEX_1_Data))]
		void Test16_VgetexppVReg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VgetexppVReg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 42 50 01", 7, Code.EVEX_Vgetexpps_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F27D9B 42 50 01", 7, Code.EVEX_Vgetexpps_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F27D28 42 50 01", 7, Code.EVEX_Vgetexpps_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F27DBB 42 50 01", 7, Code.EVEX_Vgetexpps_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F27D48 42 50 01", 7, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F27DDB 42 50 01", 7, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };

				yield return new object[] { "62 F2FD08 42 50 01", 7, Code.EVEX_Vgetexppd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2FD9B 42 50 01", 7, Code.EVEX_Vgetexppd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F2FD28 42 50 01", 7, Code.EVEX_Vgetexppd_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2FDBB 42 50 01", 7, Code.EVEX_Vgetexppd_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F2FD48 42 50 01", 7, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2FDDB 42 50 01", 7, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VgetexppVReg_k1_RegMem_EVEX_2_Data))]
		void Test16_VgetexppVReg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VgetexppVReg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D0B 42 D3", 6, Code.EVEX_Vgetexpps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 42 D3", 6, Code.EVEX_Vgetexpps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D2B 42 D3", 6, Code.EVEX_Vgetexpps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 42 D3", 6, Code.EVEX_Vgetexpps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D4B 42 D3", 6, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D9B 42 D3", 6, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F27D1B 42 D3", 6, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D3B 42 D3", 6, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D5B 42 D3", 6, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D7B 42 D3", 6, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F2FD0B 42 D3", 6, Code.EVEX_Vgetexppd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD8B 42 D3", 6, Code.EVEX_Vgetexppd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD2B 42 D3", 6, Code.EVEX_Vgetexppd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDAB 42 D3", 6, Code.EVEX_Vgetexppd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD4B 42 D3", 6, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD9B 42 D3", 6, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F2FD1B 42 D3", 6, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD3B 42 D3", 6, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD5B 42 D3", 6, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD7B 42 D3", 6, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VgetexppVReg_k1_RegMem_EVEX_1_Data))]
		void Test32_VgetexppVReg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VgetexppVReg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 42 50 01", 7, Code.EVEX_Vgetexpps_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F27D9B 42 50 01", 7, Code.EVEX_Vgetexpps_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F27D28 42 50 01", 7, Code.EVEX_Vgetexpps_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F27DBB 42 50 01", 7, Code.EVEX_Vgetexpps_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F27D48 42 50 01", 7, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F27DDB 42 50 01", 7, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };

				yield return new object[] { "62 F2FD08 42 50 01", 7, Code.EVEX_Vgetexppd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2FD9B 42 50 01", 7, Code.EVEX_Vgetexppd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F2FD28 42 50 01", 7, Code.EVEX_Vgetexppd_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2FDBB 42 50 01", 7, Code.EVEX_Vgetexppd_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F2FD48 42 50 01", 7, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2FDDB 42 50 01", 7, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VgetexppVReg_k1_RegMem_EVEX_2_Data))]
		void Test32_VgetexppVReg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VgetexppVReg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D0B 42 D3", 6, Code.EVEX_Vgetexpps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 42 D3", 6, Code.EVEX_Vgetexpps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D2B 42 D3", 6, Code.EVEX_Vgetexpps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 42 D3", 6, Code.EVEX_Vgetexpps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D4B 42 D3", 6, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D9B 42 D3", 6, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F27D1B 42 D3", 6, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D3B 42 D3", 6, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D5B 42 D3", 6, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D7B 42 D3", 6, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F2FD0B 42 D3", 6, Code.EVEX_Vgetexppd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD8B 42 D3", 6, Code.EVEX_Vgetexppd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD2B 42 D3", 6, Code.EVEX_Vgetexppd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDAB 42 D3", 6, Code.EVEX_Vgetexppd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD4B 42 D3", 6, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD9B 42 D3", 6, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F2FD1B 42 D3", 6, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD3B 42 D3", 6, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD5B 42 D3", 6, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD7B 42 D3", 6, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VgetexppVReg_k1_RegMem_EVEX_1_Data))]
		void Test64_VgetexppVReg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VgetexppVReg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F27D08 42 50 01", 7, Code.EVEX_Vgetexpps_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float32, 16, false };
				yield return new object[] { "62 F27D9B 42 50 01", 7, Code.EVEX_Vgetexpps_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float32, 4, true };

				yield return new object[] { "62 F27D28 42 50 01", 7, Code.EVEX_Vgetexpps_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float32, 32, false };
				yield return new object[] { "62 F27DBB 42 50 01", 7, Code.EVEX_Vgetexpps_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float32, 4, true };

				yield return new object[] { "62 F27D48 42 50 01", 7, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float32, 64, false };
				yield return new object[] { "62 F27DDB 42 50 01", 7, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float32, 4, true };

				yield return new object[] { "62 F2FD08 42 50 01", 7, Code.EVEX_Vgetexppd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F2FD9B 42 50 01", 7, Code.EVEX_Vgetexppd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F2FD28 42 50 01", 7, Code.EVEX_Vgetexppd_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F2FDBB 42 50 01", 7, Code.EVEX_Vgetexppd_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F2FD48 42 50 01", 7, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F2FDDB 42 50 01", 7, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VgetexppVReg_k1_RegMem_EVEX_2_Data))]
		void Test64_VgetexppVReg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VgetexppVReg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F27D0B 42 D3", 6, Code.EVEX_Vgetexpps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D8B 42 D3", 6, Code.EVEX_Vgetexpps_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 327D0B 42 D3", 6, Code.EVEX_Vgetexpps_VX_k1z_WX_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C27D8B 42 D3", 6, Code.EVEX_Vgetexpps_VX_k1z_WX_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D2B 42 D3", 6, Code.EVEX_Vgetexpps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27DAB 42 D3", 6, Code.EVEX_Vgetexpps_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 327D2B 42 D3", 6, Code.EVEX_Vgetexpps_VY_k1z_WY_b, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C27DAB 42 D3", 6, Code.EVEX_Vgetexpps_VY_k1z_WY_b, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F27D4B 42 D3", 6, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F27D9B 42 D3", 6, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 327D1B 42 D3", 6, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 C27D3B 42 D3", 6, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D5B 42 D3", 6, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F27D7B 42 D3", 6, Code.EVEX_Vgetexpps_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F2FD0B 42 D3", 6, Code.EVEX_Vgetexppd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD8B 42 D3", 6, Code.EVEX_Vgetexppd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 32FD0B 42 D3", 6, Code.EVEX_Vgetexppd_VX_k1z_WX_b, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C2FD8B 42 D3", 6, Code.EVEX_Vgetexppd_VX_k1z_WX_b, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD2B 42 D3", 6, Code.EVEX_Vgetexppd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FDAB 42 D3", 6, Code.EVEX_Vgetexppd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 32FD2B 42 D3", 6, Code.EVEX_Vgetexppd_VY_k1z_WY_b, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C2FDAB 42 D3", 6, Code.EVEX_Vgetexppd_VY_k1z_WY_b, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F2FD4B 42 D3", 6, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2FD9B 42 D3", 6, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 32FD1B 42 D3", 6, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 C2FD3B 42 D3", 6, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD5B 42 D3", 6, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F2FD7B 42 D3", 6, Code.EVEX_Vgetexppd_VZ_k1z_WZ_sae_b, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VgetexpssV_VX_k1_HX_WX_1_Data))]
		void Test16_VgetexpssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VgetexpssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 43 50 01", 7, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D8B 43 50 01", 7, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24D08 43 50 01", 7, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D2B 43 50 01", 7, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D4B 43 50 01", 7, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D6B 43 50 01", 7, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F2CD0B 43 50 01", 7, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD8B 43 50 01", 7, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CD08 43 50 01", 7, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD2B 43 50 01", 7, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD4B 43 50 01", 7, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD6B 43 50 01", 7, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VgetexpssV_VX_k1_HX_WX_2_Data))]
		void Test16_VgetexpssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VgetexpssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 43 D3", 6, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DDB 43 D3", 6, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F2CD8B 43 D3", 6, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD7B 43 D3", 6, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VgetexpssV_VX_k1_HX_WX_1_Data))]
		void Test32_VgetexpssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VgetexpssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 43 50 01", 7, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D8B 43 50 01", 7, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24D08 43 50 01", 7, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D2B 43 50 01", 7, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D4B 43 50 01", 7, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D6B 43 50 01", 7, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F2CD0B 43 50 01", 7, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD8B 43 50 01", 7, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CD08 43 50 01", 7, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD2B 43 50 01", 7, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD4B 43 50 01", 7, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD6B 43 50 01", 7, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VgetexpssV_VX_k1_HX_WX_2_Data))]
		void Test32_VgetexpssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VgetexpssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 43 D3", 6, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DDB 43 D3", 6, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F2CD8B 43 D3", 6, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD7B 43 D3", 6, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VgetexpssV_VX_k1_HX_WX_1_Data))]
		void Test64_VgetexpssV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VgetexpssV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 43 50 01", 7, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D8B 43 50 01", 7, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, true };
				yield return new object[] { "62 F24D08 43 50 01", 7, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D2B 43 50 01", 7, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D4B 43 50 01", 7, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };
				yield return new object[] { "62 F24D6B 43 50 01", 7, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float32, 4, false };

				yield return new object[] { "62 F2CD0B 43 50 01", 7, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD8B 43 50 01", 7, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, true };
				yield return new object[] { "62 F2CD08 43 50 01", 7, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.None, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD2B 43 50 01", 7, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD4B 43 50 01", 7, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
				yield return new object[] { "62 F2CD6B 43 50 01", 7, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Float64, 8, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VgetexpssV_VX_k1_HX_WX_2_Data))]
		void Test64_VgetexpssV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VgetexpssV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 43 D3", 6, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D0B 43 D3", 6, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 124D03 43 D3", 6, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D0B 43 D3", 6, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DDB 43 D3", 6, Code.EVEX_Vgetexpss_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, true };

				yield return new object[] { "62 F2CD8B 43 D3", 6, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 E28D0B 43 D3", 6, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 12CD03 43 D3", 6, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B2CD0B 43 D3", 6, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD7B 43 D3", 6, Code.EVEX_Vgetexpsd_VX_k1z_HX_WX_sae, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VplzcntdV_VX_k1_WX_1_Data))]
		void Test16_VplzcntdV_VX_k1_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VplzcntdV_VX_k1_WX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 44 50 01", 7, Code.EVEX_Vplzcntd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F27D9D 44 50 01", 7, Code.EVEX_Vplzcntd_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F27D08 44 50 01", 7, Code.EVEX_Vplzcntd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F27D2B 44 50 01", 7, Code.EVEX_Vplzcntd_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F27DBD 44 50 01", 7, Code.EVEX_Vplzcntd_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F27D28 44 50 01", 7, Code.EVEX_Vplzcntd_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F27D4B 44 50 01", 7, Code.EVEX_Vplzcntd_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F27DDD 44 50 01", 7, Code.EVEX_Vplzcntd_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F27D48 44 50 01", 7, Code.EVEX_Vplzcntd_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2FD0B 44 50 01", 7, Code.EVEX_Vplzcntq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2FD9D 44 50 01", 7, Code.EVEX_Vplzcntq_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2FD08 44 50 01", 7, Code.EVEX_Vplzcntq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2FD2B 44 50 01", 7, Code.EVEX_Vplzcntq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2FDBD 44 50 01", 7, Code.EVEX_Vplzcntq_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2FD28 44 50 01", 7, Code.EVEX_Vplzcntq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2FD4B 44 50 01", 7, Code.EVEX_Vplzcntq_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2FDDD 44 50 01", 7, Code.EVEX_Vplzcntq_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2FD48 44 50 01", 7, Code.EVEX_Vplzcntq_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VplzcntdV_VX_k1_WX_2_Data))]
		void Test16_VplzcntdV_VX_k1_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VplzcntdV_VX_k1_WX_2_Data {
			get {
				yield return new object[] { "62 F27D8B 44 D3", 6, Code.EVEX_Vplzcntd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F27D0B 44 D3", 6, Code.EVEX_Vplzcntd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F27DAB 44 D3", 6, Code.EVEX_Vplzcntd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F27D2B 44 D3", 6, Code.EVEX_Vplzcntd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F27DCB 44 D3", 6, Code.EVEX_Vplzcntd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F27D4B 44 D3", 6, Code.EVEX_Vplzcntd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2FD8B 44 D3", 6, Code.EVEX_Vplzcntq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2FD0B 44 D3", 6, Code.EVEX_Vplzcntq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2FDAB 44 D3", 6, Code.EVEX_Vplzcntq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2FD2B 44 D3", 6, Code.EVEX_Vplzcntq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2FDCB 44 D3", 6, Code.EVEX_Vplzcntq_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2FD4B 44 D3", 6, Code.EVEX_Vplzcntq_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VplzcntdV_VX_k1_WX_1_Data))]
		void Test32_VplzcntdV_VX_k1_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VplzcntdV_VX_k1_WX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 44 50 01", 7, Code.EVEX_Vplzcntd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F27D9D 44 50 01", 7, Code.EVEX_Vplzcntd_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F27D08 44 50 01", 7, Code.EVEX_Vplzcntd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F27D2B 44 50 01", 7, Code.EVEX_Vplzcntd_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F27DBD 44 50 01", 7, Code.EVEX_Vplzcntd_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F27D28 44 50 01", 7, Code.EVEX_Vplzcntd_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F27D4B 44 50 01", 7, Code.EVEX_Vplzcntd_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F27DDD 44 50 01", 7, Code.EVEX_Vplzcntd_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F27D48 44 50 01", 7, Code.EVEX_Vplzcntd_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2FD0B 44 50 01", 7, Code.EVEX_Vplzcntq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2FD9D 44 50 01", 7, Code.EVEX_Vplzcntq_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2FD08 44 50 01", 7, Code.EVEX_Vplzcntq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2FD2B 44 50 01", 7, Code.EVEX_Vplzcntq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2FDBD 44 50 01", 7, Code.EVEX_Vplzcntq_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2FD28 44 50 01", 7, Code.EVEX_Vplzcntq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2FD4B 44 50 01", 7, Code.EVEX_Vplzcntq_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2FDDD 44 50 01", 7, Code.EVEX_Vplzcntq_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2FD48 44 50 01", 7, Code.EVEX_Vplzcntq_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VplzcntdV_VX_k1_WX_2_Data))]
		void Test32_VplzcntdV_VX_k1_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VplzcntdV_VX_k1_WX_2_Data {
			get {
				yield return new object[] { "62 F27D8B 44 D3", 6, Code.EVEX_Vplzcntd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F27D0B 44 D3", 6, Code.EVEX_Vplzcntd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F27DAB 44 D3", 6, Code.EVEX_Vplzcntd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F27D2B 44 D3", 6, Code.EVEX_Vplzcntd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F27DCB 44 D3", 6, Code.EVEX_Vplzcntd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F27D4B 44 D3", 6, Code.EVEX_Vplzcntd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2FD8B 44 D3", 6, Code.EVEX_Vplzcntq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2FD0B 44 D3", 6, Code.EVEX_Vplzcntq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2FDAB 44 D3", 6, Code.EVEX_Vplzcntq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2FD2B 44 D3", 6, Code.EVEX_Vplzcntq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2FDCB 44 D3", 6, Code.EVEX_Vplzcntq_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2FD4B 44 D3", 6, Code.EVEX_Vplzcntq_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VplzcntdV_VX_k1_WX_1_Data))]
		void Test64_VplzcntdV_VX_k1_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(kreg, instr.OpMaskRegister);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VplzcntdV_VX_k1_WX_1_Data {
			get {
				yield return new object[] { "62 F27D0B 44 50 01", 7, Code.EVEX_Vplzcntd_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F27D9D 44 50 01", 7, Code.EVEX_Vplzcntd_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F27D08 44 50 01", 7, Code.EVEX_Vplzcntd_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F27D2B 44 50 01", 7, Code.EVEX_Vplzcntd_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F27DBD 44 50 01", 7, Code.EVEX_Vplzcntd_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F27D28 44 50 01", 7, Code.EVEX_Vplzcntd_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F27D4B 44 50 01", 7, Code.EVEX_Vplzcntd_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F27DDD 44 50 01", 7, Code.EVEX_Vplzcntd_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F27D48 44 50 01", 7, Code.EVEX_Vplzcntd_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2FD0B 44 50 01", 7, Code.EVEX_Vplzcntq_VX_k1z_WX_b, Register.XMM2, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2FD9D 44 50 01", 7, Code.EVEX_Vplzcntq_VX_k1z_WX_b, Register.XMM2, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2FD08 44 50 01", 7, Code.EVEX_Vplzcntq_VX_k1z_WX_b, Register.XMM2, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2FD2B 44 50 01", 7, Code.EVEX_Vplzcntq_VY_k1z_WY_b, Register.YMM2, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2FDBD 44 50 01", 7, Code.EVEX_Vplzcntq_VY_k1z_WY_b, Register.YMM2, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2FD28 44 50 01", 7, Code.EVEX_Vplzcntq_VY_k1z_WY_b, Register.YMM2, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2FD4B 44 50 01", 7, Code.EVEX_Vplzcntq_VZ_k1z_WZ_b, Register.ZMM2, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2FDDD 44 50 01", 7, Code.EVEX_Vplzcntq_VZ_k1z_WZ_b, Register.ZMM2, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2FD48 44 50 01", 7, Code.EVEX_Vplzcntq_VZ_k1z_WZ_b, Register.ZMM2, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VplzcntdV_VX_k1_WX_2_Data))]
		void Test64_VplzcntdV_VX_k1_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VplzcntdV_VX_k1_WX_2_Data {
			get {
				yield return new object[] { "62 F27D8B 44 D3", 6, Code.EVEX_Vplzcntd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E27D0B 44 D3", 6, Code.EVEX_Vplzcntd_VX_k1z_WX_b, Register.XMM18, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 127D0B 44 D3", 6, Code.EVEX_Vplzcntd_VX_k1z_WX_b, Register.XMM10, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B27D0B 44 D3", 6, Code.EVEX_Vplzcntd_VX_k1z_WX_b, Register.XMM2, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F27D0B 44 D3", 6, Code.EVEX_Vplzcntd_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F27DAB 44 D3", 6, Code.EVEX_Vplzcntd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E27D2B 44 D3", 6, Code.EVEX_Vplzcntd_VY_k1z_WY_b, Register.YMM18, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 127D2B 44 D3", 6, Code.EVEX_Vplzcntd_VY_k1z_WY_b, Register.YMM10, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B27D2B 44 D3", 6, Code.EVEX_Vplzcntd_VY_k1z_WY_b, Register.YMM2, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F27D2B 44 D3", 6, Code.EVEX_Vplzcntd_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F27DCB 44 D3", 6, Code.EVEX_Vplzcntd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E27D4B 44 D3", 6, Code.EVEX_Vplzcntd_VZ_k1z_WZ_b, Register.ZMM18, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 127D4B 44 D3", 6, Code.EVEX_Vplzcntd_VZ_k1z_WZ_b, Register.ZMM10, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B27D4B 44 D3", 6, Code.EVEX_Vplzcntd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F27D4B 44 D3", 6, Code.EVEX_Vplzcntd_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2FD8B 44 D3", 6, Code.EVEX_Vplzcntq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E2FD0B 44 D3", 6, Code.EVEX_Vplzcntq_VX_k1z_WX_b, Register.XMM18, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12FD0B 44 D3", 6, Code.EVEX_Vplzcntq_VX_k1z_WX_b, Register.XMM10, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2FD0B 44 D3", 6, Code.EVEX_Vplzcntq_VX_k1z_WX_b, Register.XMM2, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2FD0B 44 D3", 6, Code.EVEX_Vplzcntq_VX_k1z_WX_b, Register.XMM2, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2FDAB 44 D3", 6, Code.EVEX_Vplzcntq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E2FD2B 44 D3", 6, Code.EVEX_Vplzcntq_VY_k1z_WY_b, Register.YMM18, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12FD2B 44 D3", 6, Code.EVEX_Vplzcntq_VY_k1z_WY_b, Register.YMM10, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2FD2B 44 D3", 6, Code.EVEX_Vplzcntq_VY_k1z_WY_b, Register.YMM2, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2FD2B 44 D3", 6, Code.EVEX_Vplzcntq_VY_k1z_WY_b, Register.YMM2, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2FDCB 44 D3", 6, Code.EVEX_Vplzcntq_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E2FD4B 44 D3", 6, Code.EVEX_Vplzcntq_VZ_k1z_WZ_b, Register.ZMM18, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12FD4B 44 D3", 6, Code.EVEX_Vplzcntq_VZ_k1z_WZ_b, Register.ZMM10, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2FD4B 44 D3", 6, Code.EVEX_Vplzcntq_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2FD4B 44 D3", 6, Code.EVEX_Vplzcntq_VZ_k1z_WZ_b, Register.ZMM2, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrlvdV_VX_HX_WX_1_Data))]
		void Test16_VpsrlvdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpsrlvdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 45 10", 5, Code.VEX_Vpsrlvd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E24D 45 10", 5, Code.VEX_Vpsrlvd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };

				yield return new object[] { "C4E2C9 45 10", 5, Code.VEX_Vpsrlvq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E2CD 45 10", 5, Code.VEX_Vpsrlvq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrlvdV_VX_HX_WX_2_Data))]
		void Test16_VpsrlvdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpsrlvdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 45 D3", 5, Code.VEX_Vpsrlvd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 45 D3", 5, Code.VEX_Vpsrlvd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 45 D3", 5, Code.VEX_Vpsrlvq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E2CD 45 D3", 5, Code.VEX_Vpsrlvq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrlvdV_VX_HX_WX_1_Data))]
		void Test32_VpsrlvdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpsrlvdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 45 10", 5, Code.VEX_Vpsrlvd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E24D 45 10", 5, Code.VEX_Vpsrlvd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };

				yield return new object[] { "C4E2C9 45 10", 5, Code.VEX_Vpsrlvq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E2CD 45 10", 5, Code.VEX_Vpsrlvq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrlvdV_VX_HX_WX_2_Data))]
		void Test32_VpsrlvdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpsrlvdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 45 D3", 5, Code.VEX_Vpsrlvd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 45 D3", 5, Code.VEX_Vpsrlvd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 45 D3", 5, Code.VEX_Vpsrlvq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E2CD 45 D3", 5, Code.VEX_Vpsrlvq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrlvdV_VX_HX_WX_1_Data))]
		void Test64_VpsrlvdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpsrlvdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 45 10", 5, Code.VEX_Vpsrlvd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E24D 45 10", 5, Code.VEX_Vpsrlvd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };

				yield return new object[] { "C4E2C9 45 10", 5, Code.VEX_Vpsrlvq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E2CD 45 10", 5, Code.VEX_Vpsrlvq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrlvdV_VX_HX_WX_2_Data))]
		void Test64_VpsrlvdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpsrlvdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 45 D3", 5, Code.VEX_Vpsrlvd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 45 D3", 5, Code.VEX_Vpsrlvd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C46249 45 D3", 5, Code.VEX_Vpsrlvd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4624D 45 D3", 5, Code.VEX_Vpsrlvd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E209 45 D3", 5, Code.VEX_Vpsrlvd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E20D 45 D3", 5, Code.VEX_Vpsrlvd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C249 45 D3", 5, Code.VEX_Vpsrlvd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C24D 45 D3", 5, Code.VEX_Vpsrlvd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C4E2C9 45 D3", 5, Code.VEX_Vpsrlvq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E2CD 45 D3", 5, Code.VEX_Vpsrlvq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C462C9 45 D3", 5, Code.VEX_Vpsrlvq_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462CD 45 D3", 5, Code.VEX_Vpsrlvq_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E289 45 D3", 5, Code.VEX_Vpsrlvq_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E28D 45 D3", 5, Code.VEX_Vpsrlvq_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C2C9 45 D3", 5, Code.VEX_Vpsrlvq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C2CD 45 D3", 5, Code.VEX_Vpsrlvq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrlvdV_VX_k1_HX_WX_1_Data))]
		void Test16_VpsrlvdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsrlvdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 45 50 01", 7, Code.EVEX_Vpsrlvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D9D 45 50 01", 7, Code.EVEX_Vpsrlvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 45 50 01", 7, Code.EVEX_Vpsrlvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 45 50 01", 7, Code.EVEX_Vpsrlvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24DBD 45 50 01", 7, Code.EVEX_Vpsrlvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 45 50 01", 7, Code.EVEX_Vpsrlvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 45 50 01", 7, Code.EVEX_Vpsrlvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24DDD 45 50 01", 7, Code.EVEX_Vpsrlvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 45 50 01", 7, Code.EVEX_Vpsrlvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 45 50 01", 7, Code.EVEX_Vpsrlvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD9D 45 50 01", 7, Code.EVEX_Vpsrlvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2CD08 45 50 01", 7, Code.EVEX_Vpsrlvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 45 50 01", 7, Code.EVEX_Vpsrlvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CDBD 45 50 01", 7, Code.EVEX_Vpsrlvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2CD28 45 50 01", 7, Code.EVEX_Vpsrlvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 45 50 01", 7, Code.EVEX_Vpsrlvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CDDD 45 50 01", 7, Code.EVEX_Vpsrlvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2CD48 45 50 01", 7, Code.EVEX_Vpsrlvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrlvdV_VX_k1_HX_WX_2_Data))]
		void Test16_VpsrlvdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsrlvdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 45 D3", 6, Code.EVEX_Vpsrlvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 45 D3", 6, Code.EVEX_Vpsrlvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 45 D3", 6, Code.EVEX_Vpsrlvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 45 D3", 6, Code.EVEX_Vpsrlvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 45 D3", 6, Code.EVEX_Vpsrlvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 45 D3", 6, Code.EVEX_Vpsrlvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 45 D3", 6, Code.EVEX_Vpsrlvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 45 D3", 6, Code.EVEX_Vpsrlvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 45 D3", 6, Code.EVEX_Vpsrlvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 45 D3", 6, Code.EVEX_Vpsrlvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 45 D3", 6, Code.EVEX_Vpsrlvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 45 D3", 6, Code.EVEX_Vpsrlvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrlvdV_VX_k1_HX_WX_1_Data))]
		void Test32_VpsrlvdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsrlvdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 45 50 01", 7, Code.EVEX_Vpsrlvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D9D 45 50 01", 7, Code.EVEX_Vpsrlvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 45 50 01", 7, Code.EVEX_Vpsrlvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 45 50 01", 7, Code.EVEX_Vpsrlvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24DBD 45 50 01", 7, Code.EVEX_Vpsrlvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 45 50 01", 7, Code.EVEX_Vpsrlvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 45 50 01", 7, Code.EVEX_Vpsrlvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24DDD 45 50 01", 7, Code.EVEX_Vpsrlvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 45 50 01", 7, Code.EVEX_Vpsrlvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 45 50 01", 7, Code.EVEX_Vpsrlvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD9D 45 50 01", 7, Code.EVEX_Vpsrlvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2CD08 45 50 01", 7, Code.EVEX_Vpsrlvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 45 50 01", 7, Code.EVEX_Vpsrlvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CDBD 45 50 01", 7, Code.EVEX_Vpsrlvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2CD28 45 50 01", 7, Code.EVEX_Vpsrlvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 45 50 01", 7, Code.EVEX_Vpsrlvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CDDD 45 50 01", 7, Code.EVEX_Vpsrlvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2CD48 45 50 01", 7, Code.EVEX_Vpsrlvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrlvdV_VX_k1_HX_WX_2_Data))]
		void Test32_VpsrlvdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsrlvdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 45 D3", 6, Code.EVEX_Vpsrlvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 45 D3", 6, Code.EVEX_Vpsrlvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 45 D3", 6, Code.EVEX_Vpsrlvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 45 D3", 6, Code.EVEX_Vpsrlvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 45 D3", 6, Code.EVEX_Vpsrlvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 45 D3", 6, Code.EVEX_Vpsrlvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 45 D3", 6, Code.EVEX_Vpsrlvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 45 D3", 6, Code.EVEX_Vpsrlvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 45 D3", 6, Code.EVEX_Vpsrlvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 45 D3", 6, Code.EVEX_Vpsrlvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 45 D3", 6, Code.EVEX_Vpsrlvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 45 D3", 6, Code.EVEX_Vpsrlvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrlvdV_VX_k1_HX_WX_1_Data))]
		void Test64_VpsrlvdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsrlvdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 45 50 01", 7, Code.EVEX_Vpsrlvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D9D 45 50 01", 7, Code.EVEX_Vpsrlvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 45 50 01", 7, Code.EVEX_Vpsrlvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 45 50 01", 7, Code.EVEX_Vpsrlvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24DBD 45 50 01", 7, Code.EVEX_Vpsrlvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 45 50 01", 7, Code.EVEX_Vpsrlvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 45 50 01", 7, Code.EVEX_Vpsrlvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24DDD 45 50 01", 7, Code.EVEX_Vpsrlvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 45 50 01", 7, Code.EVEX_Vpsrlvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 45 50 01", 7, Code.EVEX_Vpsrlvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD9D 45 50 01", 7, Code.EVEX_Vpsrlvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2CD08 45 50 01", 7, Code.EVEX_Vpsrlvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 45 50 01", 7, Code.EVEX_Vpsrlvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CDBD 45 50 01", 7, Code.EVEX_Vpsrlvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2CD28 45 50 01", 7, Code.EVEX_Vpsrlvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 45 50 01", 7, Code.EVEX_Vpsrlvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CDDD 45 50 01", 7, Code.EVEX_Vpsrlvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2CD48 45 50 01", 7, Code.EVEX_Vpsrlvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrlvdV_VX_k1_HX_WX_2_Data))]
		void Test64_VpsrlvdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsrlvdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 45 D3", 6, Code.EVEX_Vpsrlvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E20D0B 45 D3", 6, Code.EVEX_Vpsrlvd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 124D03 45 D3", 6, Code.EVEX_Vpsrlvd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B24D0B 45 D3", 6, Code.EVEX_Vpsrlvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F24D0B 45 D3", 6, Code.EVEX_Vpsrlvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 45 D3", 6, Code.EVEX_Vpsrlvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E20D2B 45 D3", 6, Code.EVEX_Vpsrlvd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 124D23 45 D3", 6, Code.EVEX_Vpsrlvd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 45 D3", 6, Code.EVEX_Vpsrlvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F24D2B 45 D3", 6, Code.EVEX_Vpsrlvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 45 D3", 6, Code.EVEX_Vpsrlvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E20D4B 45 D3", 6, Code.EVEX_Vpsrlvd_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 124D43 45 D3", 6, Code.EVEX_Vpsrlvd_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 45 D3", 6, Code.EVEX_Vpsrlvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F24D4B 45 D3", 6, Code.EVEX_Vpsrlvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 45 D3", 6, Code.EVEX_Vpsrlvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E28D0B 45 D3", 6, Code.EVEX_Vpsrlvq_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12CD03 45 D3", 6, Code.EVEX_Vpsrlvq_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CD0B 45 D3", 6, Code.EVEX_Vpsrlvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2CD0B 45 D3", 6, Code.EVEX_Vpsrlvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 45 D3", 6, Code.EVEX_Vpsrlvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E28D2B 45 D3", 6, Code.EVEX_Vpsrlvq_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12CD23 45 D3", 6, Code.EVEX_Vpsrlvq_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B 45 D3", 6, Code.EVEX_Vpsrlvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD2B 45 D3", 6, Code.EVEX_Vpsrlvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 45 D3", 6, Code.EVEX_Vpsrlvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E28D4B 45 D3", 6, Code.EVEX_Vpsrlvq_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12CD43 45 D3", 6, Code.EVEX_Vpsrlvq_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B 45 D3", 6, Code.EVEX_Vpsrlvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD4B 45 D3", 6, Code.EVEX_Vpsrlvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsravdV_VX_HX_WX_1_Data))]
		void Test16_VpsravdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpsravdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 46 10", 5, Code.VEX_Vpsravd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E24D 46 10", 5, Code.VEX_Vpsravd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsravdV_VX_HX_WX_2_Data))]
		void Test16_VpsravdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpsravdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 46 D3", 5, Code.VEX_Vpsravd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 46 D3", 5, Code.VEX_Vpsravd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsravdV_VX_HX_WX_1_Data))]
		void Test32_VpsravdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpsravdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 46 10", 5, Code.VEX_Vpsravd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E24D 46 10", 5, Code.VEX_Vpsravd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsravdV_VX_HX_WX_2_Data))]
		void Test32_VpsravdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpsravdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 46 D3", 5, Code.VEX_Vpsravd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 46 D3", 5, Code.VEX_Vpsravd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsravdV_VX_HX_WX_1_Data))]
		void Test64_VpsravdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpsravdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 46 10", 5, Code.VEX_Vpsravd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E24D 46 10", 5, Code.VEX_Vpsravd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsravdV_VX_HX_WX_2_Data))]
		void Test64_VpsravdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpsravdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 46 D3", 5, Code.VEX_Vpsravd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 46 D3", 5, Code.VEX_Vpsravd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C46249 46 D3", 5, Code.VEX_Vpsravd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4624D 46 D3", 5, Code.VEX_Vpsravd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E209 46 D3", 5, Code.VEX_Vpsravd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E20D 46 D3", 5, Code.VEX_Vpsravd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C249 46 D3", 5, Code.VEX_Vpsravd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C24D 46 D3", 5, Code.VEX_Vpsravd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsravdV_VX_k1_HX_WX_1_Data))]
		void Test16_VpsravdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsravdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 46 50 01", 7, Code.EVEX_Vpsravd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D9D 46 50 01", 7, Code.EVEX_Vpsravd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 46 50 01", 7, Code.EVEX_Vpsravd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 46 50 01", 7, Code.EVEX_Vpsravd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24DBD 46 50 01", 7, Code.EVEX_Vpsravd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 46 50 01", 7, Code.EVEX_Vpsravd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 46 50 01", 7, Code.EVEX_Vpsravd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24DDD 46 50 01", 7, Code.EVEX_Vpsravd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 46 50 01", 7, Code.EVEX_Vpsravd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 46 50 01", 7, Code.EVEX_Vpsravq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD9D 46 50 01", 7, Code.EVEX_Vpsravq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2CD08 46 50 01", 7, Code.EVEX_Vpsravq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 46 50 01", 7, Code.EVEX_Vpsravq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CDBD 46 50 01", 7, Code.EVEX_Vpsravq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2CD28 46 50 01", 7, Code.EVEX_Vpsravq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 46 50 01", 7, Code.EVEX_Vpsravq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CDDD 46 50 01", 7, Code.EVEX_Vpsravq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2CD48 46 50 01", 7, Code.EVEX_Vpsravq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsravdV_VX_k1_HX_WX_2_Data))]
		void Test16_VpsravdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsravdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 46 D3", 6, Code.EVEX_Vpsravd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 46 D3", 6, Code.EVEX_Vpsravd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 46 D3", 6, Code.EVEX_Vpsravd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 46 D3", 6, Code.EVEX_Vpsravd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 46 D3", 6, Code.EVEX_Vpsravd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 46 D3", 6, Code.EVEX_Vpsravd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 46 D3", 6, Code.EVEX_Vpsravq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 46 D3", 6, Code.EVEX_Vpsravq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 46 D3", 6, Code.EVEX_Vpsravq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 46 D3", 6, Code.EVEX_Vpsravq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 46 D3", 6, Code.EVEX_Vpsravq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 46 D3", 6, Code.EVEX_Vpsravq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsravdV_VX_k1_HX_WX_1_Data))]
		void Test32_VpsravdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsravdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 46 50 01", 7, Code.EVEX_Vpsravd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D9D 46 50 01", 7, Code.EVEX_Vpsravd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 46 50 01", 7, Code.EVEX_Vpsravd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 46 50 01", 7, Code.EVEX_Vpsravd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24DBD 46 50 01", 7, Code.EVEX_Vpsravd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 46 50 01", 7, Code.EVEX_Vpsravd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 46 50 01", 7, Code.EVEX_Vpsravd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24DDD 46 50 01", 7, Code.EVEX_Vpsravd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 46 50 01", 7, Code.EVEX_Vpsravd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 46 50 01", 7, Code.EVEX_Vpsravq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD9D 46 50 01", 7, Code.EVEX_Vpsravq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2CD08 46 50 01", 7, Code.EVEX_Vpsravq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 46 50 01", 7, Code.EVEX_Vpsravq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CDBD 46 50 01", 7, Code.EVEX_Vpsravq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2CD28 46 50 01", 7, Code.EVEX_Vpsravq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 46 50 01", 7, Code.EVEX_Vpsravq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CDDD 46 50 01", 7, Code.EVEX_Vpsravq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2CD48 46 50 01", 7, Code.EVEX_Vpsravq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsravdV_VX_k1_HX_WX_2_Data))]
		void Test32_VpsravdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsravdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 46 D3", 6, Code.EVEX_Vpsravd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 46 D3", 6, Code.EVEX_Vpsravd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 46 D3", 6, Code.EVEX_Vpsravd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 46 D3", 6, Code.EVEX_Vpsravd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 46 D3", 6, Code.EVEX_Vpsravd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 46 D3", 6, Code.EVEX_Vpsravd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 46 D3", 6, Code.EVEX_Vpsravq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 46 D3", 6, Code.EVEX_Vpsravq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 46 D3", 6, Code.EVEX_Vpsravq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 46 D3", 6, Code.EVEX_Vpsravq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 46 D3", 6, Code.EVEX_Vpsravq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 46 D3", 6, Code.EVEX_Vpsravq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsravdV_VX_k1_HX_WX_1_Data))]
		void Test64_VpsravdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsravdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 46 50 01", 7, Code.EVEX_Vpsravd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D9D 46 50 01", 7, Code.EVEX_Vpsravd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 46 50 01", 7, Code.EVEX_Vpsravd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 46 50 01", 7, Code.EVEX_Vpsravd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24DBD 46 50 01", 7, Code.EVEX_Vpsravd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 46 50 01", 7, Code.EVEX_Vpsravd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 46 50 01", 7, Code.EVEX_Vpsravd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24DDD 46 50 01", 7, Code.EVEX_Vpsravd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 46 50 01", 7, Code.EVEX_Vpsravd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 46 50 01", 7, Code.EVEX_Vpsravq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD9D 46 50 01", 7, Code.EVEX_Vpsravq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2CD08 46 50 01", 7, Code.EVEX_Vpsravq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 46 50 01", 7, Code.EVEX_Vpsravq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CDBD 46 50 01", 7, Code.EVEX_Vpsravq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2CD28 46 50 01", 7, Code.EVEX_Vpsravq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 46 50 01", 7, Code.EVEX_Vpsravq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CDDD 46 50 01", 7, Code.EVEX_Vpsravq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2CD48 46 50 01", 7, Code.EVEX_Vpsravq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsravdV_VX_k1_HX_WX_2_Data))]
		void Test64_VpsravdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsravdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 46 D3", 6, Code.EVEX_Vpsravd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E20D0B 46 D3", 6, Code.EVEX_Vpsravd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 124D03 46 D3", 6, Code.EVEX_Vpsravd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B24D0B 46 D3", 6, Code.EVEX_Vpsravd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F24D0B 46 D3", 6, Code.EVEX_Vpsravd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 46 D3", 6, Code.EVEX_Vpsravd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E20D2B 46 D3", 6, Code.EVEX_Vpsravd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 124D23 46 D3", 6, Code.EVEX_Vpsravd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 46 D3", 6, Code.EVEX_Vpsravd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F24D2B 46 D3", 6, Code.EVEX_Vpsravd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 46 D3", 6, Code.EVEX_Vpsravd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E20D4B 46 D3", 6, Code.EVEX_Vpsravd_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 124D43 46 D3", 6, Code.EVEX_Vpsravd_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 46 D3", 6, Code.EVEX_Vpsravd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F24D4B 46 D3", 6, Code.EVEX_Vpsravd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 46 D3", 6, Code.EVEX_Vpsravq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E28D0B 46 D3", 6, Code.EVEX_Vpsravq_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12CD03 46 D3", 6, Code.EVEX_Vpsravq_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CD0B 46 D3", 6, Code.EVEX_Vpsravq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2CD0B 46 D3", 6, Code.EVEX_Vpsravq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 46 D3", 6, Code.EVEX_Vpsravq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E28D2B 46 D3", 6, Code.EVEX_Vpsravq_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12CD23 46 D3", 6, Code.EVEX_Vpsravq_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B 46 D3", 6, Code.EVEX_Vpsravq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD2B 46 D3", 6, Code.EVEX_Vpsravq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 46 D3", 6, Code.EVEX_Vpsravq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E28D4B 46 D3", 6, Code.EVEX_Vpsravq_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12CD43 46 D3", 6, Code.EVEX_Vpsravq_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B 46 D3", 6, Code.EVEX_Vpsravq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD4B 46 D3", 6, Code.EVEX_Vpsravq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsllvdV_VX_HX_WX_1_Data))]
		void Test16_VpsllvdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpsllvdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 47 10", 5, Code.VEX_Vpsllvd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E24D 47 10", 5, Code.VEX_Vpsllvd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };

				yield return new object[] { "C4E2C9 47 10", 5, Code.VEX_Vpsllvq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E2CD 47 10", 5, Code.VEX_Vpsllvq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsllvdV_VX_HX_WX_2_Data))]
		void Test16_VpsllvdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpsllvdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 47 D3", 5, Code.VEX_Vpsllvd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 47 D3", 5, Code.VEX_Vpsllvd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 47 D3", 5, Code.VEX_Vpsllvq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E2CD 47 D3", 5, Code.VEX_Vpsllvq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsllvdV_VX_HX_WX_1_Data))]
		void Test32_VpsllvdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpsllvdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 47 10", 5, Code.VEX_Vpsllvd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E24D 47 10", 5, Code.VEX_Vpsllvd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };

				yield return new object[] { "C4E2C9 47 10", 5, Code.VEX_Vpsllvq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E2CD 47 10", 5, Code.VEX_Vpsllvq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsllvdV_VX_HX_WX_2_Data))]
		void Test32_VpsllvdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpsllvdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 47 D3", 5, Code.VEX_Vpsllvd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 47 D3", 5, Code.VEX_Vpsllvd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };

				yield return new object[] { "C4E2C9 47 D3", 5, Code.VEX_Vpsllvq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E2CD 47 D3", 5, Code.VEX_Vpsllvq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsllvdV_VX_HX_WX_1_Data))]
		void Test64_VpsllvdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpsllvdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 47 10", 5, Code.VEX_Vpsllvd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E24D 47 10", 5, Code.VEX_Vpsllvd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };

				yield return new object[] { "C4E2C9 47 10", 5, Code.VEX_Vpsllvq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E2CD 47 10", 5, Code.VEX_Vpsllvq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsllvdV_VX_HX_WX_2_Data))]
		void Test64_VpsllvdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpsllvdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 47 D3", 5, Code.VEX_Vpsllvd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 47 D3", 5, Code.VEX_Vpsllvd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C46249 47 D3", 5, Code.VEX_Vpsllvd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4624D 47 D3", 5, Code.VEX_Vpsllvd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E209 47 D3", 5, Code.VEX_Vpsllvd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E20D 47 D3", 5, Code.VEX_Vpsllvd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C249 47 D3", 5, Code.VEX_Vpsllvd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C24D 47 D3", 5, Code.VEX_Vpsllvd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };

				yield return new object[] { "C4E2C9 47 D3", 5, Code.VEX_Vpsllvq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E2CD 47 D3", 5, Code.VEX_Vpsllvq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C462C9 47 D3", 5, Code.VEX_Vpsllvq_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C462CD 47 D3", 5, Code.VEX_Vpsllvq_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E289 47 D3", 5, Code.VEX_Vpsllvq_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E28D 47 D3", 5, Code.VEX_Vpsllvq_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C2C9 47 D3", 5, Code.VEX_Vpsllvq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C2CD 47 D3", 5, Code.VEX_Vpsllvq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsllvdV_VX_k1_HX_WX_1_Data))]
		void Test16_VpsllvdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsllvdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 47 50 01", 7, Code.EVEX_Vpsllvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D9D 47 50 01", 7, Code.EVEX_Vpsllvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 47 50 01", 7, Code.EVEX_Vpsllvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 47 50 01", 7, Code.EVEX_Vpsllvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24DBD 47 50 01", 7, Code.EVEX_Vpsllvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 47 50 01", 7, Code.EVEX_Vpsllvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 47 50 01", 7, Code.EVEX_Vpsllvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24DDD 47 50 01", 7, Code.EVEX_Vpsllvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 47 50 01", 7, Code.EVEX_Vpsllvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 47 50 01", 7, Code.EVEX_Vpsllvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD9D 47 50 01", 7, Code.EVEX_Vpsllvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2CD08 47 50 01", 7, Code.EVEX_Vpsllvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 47 50 01", 7, Code.EVEX_Vpsllvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CDBD 47 50 01", 7, Code.EVEX_Vpsllvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2CD28 47 50 01", 7, Code.EVEX_Vpsllvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 47 50 01", 7, Code.EVEX_Vpsllvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CDDD 47 50 01", 7, Code.EVEX_Vpsllvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2CD48 47 50 01", 7, Code.EVEX_Vpsllvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsllvdV_VX_k1_HX_WX_2_Data))]
		void Test16_VpsllvdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsllvdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 47 D3", 6, Code.EVEX_Vpsllvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 47 D3", 6, Code.EVEX_Vpsllvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 47 D3", 6, Code.EVEX_Vpsllvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 47 D3", 6, Code.EVEX_Vpsllvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 47 D3", 6, Code.EVEX_Vpsllvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 47 D3", 6, Code.EVEX_Vpsllvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 47 D3", 6, Code.EVEX_Vpsllvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 47 D3", 6, Code.EVEX_Vpsllvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 47 D3", 6, Code.EVEX_Vpsllvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 47 D3", 6, Code.EVEX_Vpsllvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 47 D3", 6, Code.EVEX_Vpsllvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 47 D3", 6, Code.EVEX_Vpsllvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsllvdV_VX_k1_HX_WX_1_Data))]
		void Test32_VpsllvdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsllvdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 47 50 01", 7, Code.EVEX_Vpsllvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D9D 47 50 01", 7, Code.EVEX_Vpsllvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 47 50 01", 7, Code.EVEX_Vpsllvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 47 50 01", 7, Code.EVEX_Vpsllvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24DBD 47 50 01", 7, Code.EVEX_Vpsllvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 47 50 01", 7, Code.EVEX_Vpsllvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 47 50 01", 7, Code.EVEX_Vpsllvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24DDD 47 50 01", 7, Code.EVEX_Vpsllvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 47 50 01", 7, Code.EVEX_Vpsllvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 47 50 01", 7, Code.EVEX_Vpsllvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD9D 47 50 01", 7, Code.EVEX_Vpsllvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2CD08 47 50 01", 7, Code.EVEX_Vpsllvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 47 50 01", 7, Code.EVEX_Vpsllvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CDBD 47 50 01", 7, Code.EVEX_Vpsllvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2CD28 47 50 01", 7, Code.EVEX_Vpsllvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 47 50 01", 7, Code.EVEX_Vpsllvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CDDD 47 50 01", 7, Code.EVEX_Vpsllvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2CD48 47 50 01", 7, Code.EVEX_Vpsllvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsllvdV_VX_k1_HX_WX_2_Data))]
		void Test32_VpsllvdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsllvdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 47 D3", 6, Code.EVEX_Vpsllvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F24D0B 47 D3", 6, Code.EVEX_Vpsllvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 47 D3", 6, Code.EVEX_Vpsllvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F24D2B 47 D3", 6, Code.EVEX_Vpsllvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 47 D3", 6, Code.EVEX_Vpsllvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F24D4B 47 D3", 6, Code.EVEX_Vpsllvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 47 D3", 6, Code.EVEX_Vpsllvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F2CD0B 47 D3", 6, Code.EVEX_Vpsllvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 47 D3", 6, Code.EVEX_Vpsllvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F2CD2B 47 D3", 6, Code.EVEX_Vpsllvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 47 D3", 6, Code.EVEX_Vpsllvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F2CD4B 47 D3", 6, Code.EVEX_Vpsllvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsllvdV_VX_k1_HX_WX_1_Data))]
		void Test64_VpsllvdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsllvdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 47 50 01", 7, Code.EVEX_Vpsllvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F24D9D 47 50 01", 7, Code.EVEX_Vpsllvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F24D08 47 50 01", 7, Code.EVEX_Vpsllvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F24D2B 47 50 01", 7, Code.EVEX_Vpsllvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F24DBD 47 50 01", 7, Code.EVEX_Vpsllvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F24D28 47 50 01", 7, Code.EVEX_Vpsllvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F24D4B 47 50 01", 7, Code.EVEX_Vpsllvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F24DDD 47 50 01", 7, Code.EVEX_Vpsllvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F24D48 47 50 01", 7, Code.EVEX_Vpsllvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F2CD0B 47 50 01", 7, Code.EVEX_Vpsllvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F2CD9D 47 50 01", 7, Code.EVEX_Vpsllvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F2CD08 47 50 01", 7, Code.EVEX_Vpsllvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F2CD2B 47 50 01", 7, Code.EVEX_Vpsllvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F2CDBD 47 50 01", 7, Code.EVEX_Vpsllvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F2CD28 47 50 01", 7, Code.EVEX_Vpsllvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F2CD4B 47 50 01", 7, Code.EVEX_Vpsllvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F2CDDD 47 50 01", 7, Code.EVEX_Vpsllvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F2CD48 47 50 01", 7, Code.EVEX_Vpsllvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsllvdV_VX_k1_HX_WX_2_Data))]
		void Test64_VpsllvdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsllvdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D8B 47 D3", 6, Code.EVEX_Vpsllvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E20D0B 47 D3", 6, Code.EVEX_Vpsllvd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 124D03 47 D3", 6, Code.EVEX_Vpsllvd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B24D0B 47 D3", 6, Code.EVEX_Vpsllvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F24D0B 47 D3", 6, Code.EVEX_Vpsllvd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F24DAB 47 D3", 6, Code.EVEX_Vpsllvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E20D2B 47 D3", 6, Code.EVEX_Vpsllvd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 124D23 47 D3", 6, Code.EVEX_Vpsllvd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B24D2B 47 D3", 6, Code.EVEX_Vpsllvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F24D2B 47 D3", 6, Code.EVEX_Vpsllvd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F24DCB 47 D3", 6, Code.EVEX_Vpsllvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E20D4B 47 D3", 6, Code.EVEX_Vpsllvd_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 124D43 47 D3", 6, Code.EVEX_Vpsllvd_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B24D4B 47 D3", 6, Code.EVEX_Vpsllvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F24D4B 47 D3", 6, Code.EVEX_Vpsllvd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F2CD8B 47 D3", 6, Code.EVEX_Vpsllvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E28D0B 47 D3", 6, Code.EVEX_Vpsllvq_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 12CD03 47 D3", 6, Code.EVEX_Vpsllvq_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B2CD0B 47 D3", 6, Code.EVEX_Vpsllvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F2CD0B 47 D3", 6, Code.EVEX_Vpsllvq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F2CDAB 47 D3", 6, Code.EVEX_Vpsllvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E28D2B 47 D3", 6, Code.EVEX_Vpsllvq_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 12CD23 47 D3", 6, Code.EVEX_Vpsllvq_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B2CD2B 47 D3", 6, Code.EVEX_Vpsllvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F2CD2B 47 D3", 6, Code.EVEX_Vpsllvq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F2CDCB 47 D3", 6, Code.EVEX_Vpsllvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E28D4B 47 D3", 6, Code.EVEX_Vpsllvq_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 12CD43 47 D3", 6, Code.EVEX_Vpsllvq_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B2CD4B 47 D3", 6, Code.EVEX_Vpsllvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F2CD4B 47 D3", 6, Code.EVEX_Vpsllvq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}
	}
}
