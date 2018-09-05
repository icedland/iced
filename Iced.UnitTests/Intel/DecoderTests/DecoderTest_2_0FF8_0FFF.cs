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
	public sealed class DecoderTest_2_0FF8_0FFF : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_PsubbV_VX_WX_1_Data))]
		void Test16_PsubbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PsubbV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF8 08", 3, Code.Psubb_P_Q, Register.MM1, MemorySize.Packed64_Int8 };

				yield return new object[] { "66 0FF8 08", 4, Code.Psubb_VX_WX, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsubbV_VX_WX_2_Data))]
		void Test16_PsubbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PsubbV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF8 CD", 3, Code.Psubb_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF8 CD", 4, Code.Psubb_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsubbV_VX_WX_1_Data))]
		void Test32_PsubbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PsubbV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF8 08", 3, Code.Psubb_P_Q, Register.MM1, MemorySize.Packed64_Int8 };

				yield return new object[] { "66 0FF8 08", 4, Code.Psubb_VX_WX, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsubbV_VX_WX_2_Data))]
		void Test32_PsubbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PsubbV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF8 CD", 3, Code.Psubb_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF8 CD", 4, Code.Psubb_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsubbV_VX_WX_1_Data))]
		void Test64_PsubbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PsubbV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF8 08", 3, Code.Psubb_P_Q, Register.MM1, MemorySize.Packed64_Int8 };

				yield return new object[] { "66 0FF8 08", 4, Code.Psubb_VX_WX, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsubbV_VX_WX_2_Data))]
		void Test64_PsubbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PsubbV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF8 CD", 3, Code.Psubb_P_Q, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FF8 CD", 4, Code.Psubb_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF8 CD", 4, Code.Psubb_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FF8 CD", 5, Code.Psubb_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FF8 CD", 5, Code.Psubb_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FF8 CD", 5, Code.Psubb_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubbV_VX_HX_WX_1_Data))]
		void Test16_VpsubbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpsubbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F8 10", 4, Code.VEX_Vpsubb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C5CD F8 10", 4, Code.VEX_Vpsubb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E1C9 F8 10", 5, Code.VEX_Vpsubb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E1CD F8 10", 5, Code.VEX_Vpsubb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubbV_VX_HX_WX_2_Data))]
		void Test16_VpsubbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpsubbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F8 D3", 4, Code.VEX_Vpsubb_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F8 D3", 4, Code.VEX_Vpsubb_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubbV_VX_HX_WX_1_Data))]
		void Test32_VpsubbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpsubbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F8 10", 4, Code.VEX_Vpsubb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C5CD F8 10", 4, Code.VEX_Vpsubb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E1C9 F8 10", 5, Code.VEX_Vpsubb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E1CD F8 10", 5, Code.VEX_Vpsubb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubbV_VX_HX_WX_2_Data))]
		void Test32_VpsubbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpsubbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F8 D3", 4, Code.VEX_Vpsubb_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F8 D3", 4, Code.VEX_Vpsubb_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubbV_VX_HX_WX_1_Data))]
		void Test64_VpsubbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpsubbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F8 10", 4, Code.VEX_Vpsubb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C5CD F8 10", 4, Code.VEX_Vpsubb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E1C9 F8 10", 5, Code.VEX_Vpsubb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E1CD F8 10", 5, Code.VEX_Vpsubb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubbV_VX_HX_WX_2_Data))]
		void Test64_VpsubbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpsubbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F8 D3", 4, Code.VEX_Vpsubb_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F8 D3", 4, Code.VEX_Vpsubb_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 F8 D3", 4, Code.VEX_Vpsubb_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D F8 D3", 4, Code.VEX_Vpsubb_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 F8 D3", 4, Code.VEX_Vpsubb_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D F8 D3", 4, Code.VEX_Vpsubb_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 F8 D3", 5, Code.VEX_Vpsubb_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D F8 D3", 5, Code.VEX_Vpsubb_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubbV_VX_k1_HX_WX_1_Data))]
		void Test16_VpsubbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsubbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B F8 50 01", 7, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F14D8D F8 50 01", 7, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F14D08 F8 50 01", 7, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F1CD0B F8 50 01", 7, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F14D2B F8 50 01", 7, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F14DAD F8 50 01", 7, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F14D28 F8 50 01", 7, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F1CD2B F8 50 01", 7, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F14D4B F8 50 01", 7, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F14DCD F8 50 01", 7, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F14D48 F8 50 01", 7, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F1CD4B F8 50 01", 7, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubbV_VX_k1_HX_WX_2_Data))]
		void Test16_VpsubbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpsubbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B F8 D3", 6, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B F8 D3", 6, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B F8 D3", 6, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B F8 D3", 6, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB F8 D3", 6, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B F8 D3", 6, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B F8 D3", 6, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB F8 D3", 6, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B F8 D3", 6, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubbV_VX_k1_HX_WX_1_Data))]
		void Test32_VpsubbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsubbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B F8 50 01", 7, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F14D8D F8 50 01", 7, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F14D08 F8 50 01", 7, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F1CD0B F8 50 01", 7, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F14D2B F8 50 01", 7, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F14DAD F8 50 01", 7, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F14D28 F8 50 01", 7, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F1CD2B F8 50 01", 7, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F14D4B F8 50 01", 7, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F14DCD F8 50 01", 7, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F14D48 F8 50 01", 7, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F1CD4B F8 50 01", 7, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubbV_VX_k1_HX_WX_2_Data))]
		void Test32_VpsubbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpsubbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B F8 D3", 6, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B F8 D3", 6, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B F8 D3", 6, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B F8 D3", 6, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB F8 D3", 6, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B F8 D3", 6, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B F8 D3", 6, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB F8 D3", 6, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B F8 D3", 6, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubbV_VX_k1_HX_WX_1_Data))]
		void Test64_VpsubbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsubbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B F8 50 01", 7, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F14D8D F8 50 01", 7, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F14D08 F8 50 01", 7, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F1CD0B F8 50 01", 7, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F14D2B F8 50 01", 7, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F14DAD F8 50 01", 7, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F14D28 F8 50 01", 7, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F1CD2B F8 50 01", 7, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F14D4B F8 50 01", 7, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F14DCD F8 50 01", 7, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F14D48 F8 50 01", 7, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F1CD4B F8 50 01", 7, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubbV_VX_k1_HX_WX_2_Data))]
		void Test64_VpsubbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpsubbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B F8 D3", 6, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B F8 D3", 6, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 F8 D3", 6, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B F8 D3", 6, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B F8 D3", 6, Code.EVEX_Vpsubb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B F8 D3", 6, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB F8 D3", 6, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 F8 D3", 6, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B F8 D3", 6, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B F8 D3", 6, Code.EVEX_Vpsubb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B F8 D3", 6, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB F8 D3", 6, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 F8 D3", 6, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B F8 D3", 6, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B F8 D3", 6, Code.EVEX_Vpsubb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsubwV_VX_WX_1_Data))]
		void Test16_PsubwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PsubwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF9 08", 3, Code.Psubw_P_Q, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0FF9 08", 4, Code.Psubw_VX_WX, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsubwV_VX_WX_2_Data))]
		void Test16_PsubwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PsubwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF9 CD", 3, Code.Psubw_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF9 CD", 4, Code.Psubw_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsubwV_VX_WX_1_Data))]
		void Test32_PsubwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PsubwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF9 08", 3, Code.Psubw_P_Q, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0FF9 08", 4, Code.Psubw_VX_WX, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsubwV_VX_WX_2_Data))]
		void Test32_PsubwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PsubwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF9 CD", 3, Code.Psubw_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF9 CD", 4, Code.Psubw_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsubwV_VX_WX_1_Data))]
		void Test64_PsubwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PsubwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF9 08", 3, Code.Psubw_P_Q, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0FF9 08", 4, Code.Psubw_VX_WX, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsubwV_VX_WX_2_Data))]
		void Test64_PsubwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PsubwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF9 CD", 3, Code.Psubw_P_Q, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FF9 CD", 4, Code.Psubw_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF9 CD", 4, Code.Psubw_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FF9 CD", 5, Code.Psubw_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FF9 CD", 5, Code.Psubw_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FF9 CD", 5, Code.Psubw_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubwV_VX_HX_WX_1_Data))]
		void Test16_VpsubwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpsubwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F9 10", 4, Code.VEX_Vpsubw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C5CD F9 10", 4, Code.VEX_Vpsubw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E1C9 F9 10", 5, Code.VEX_Vpsubw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E1CD F9 10", 5, Code.VEX_Vpsubw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubwV_VX_HX_WX_2_Data))]
		void Test16_VpsubwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpsubwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F9 D3", 4, Code.VEX_Vpsubw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F9 D3", 4, Code.VEX_Vpsubw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubwV_VX_HX_WX_1_Data))]
		void Test32_VpsubwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpsubwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F9 10", 4, Code.VEX_Vpsubw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C5CD F9 10", 4, Code.VEX_Vpsubw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E1C9 F9 10", 5, Code.VEX_Vpsubw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E1CD F9 10", 5, Code.VEX_Vpsubw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubwV_VX_HX_WX_2_Data))]
		void Test32_VpsubwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpsubwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F9 D3", 4, Code.VEX_Vpsubw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F9 D3", 4, Code.VEX_Vpsubw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubwV_VX_HX_WX_1_Data))]
		void Test64_VpsubwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpsubwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F9 10", 4, Code.VEX_Vpsubw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C5CD F9 10", 4, Code.VEX_Vpsubw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E1C9 F9 10", 5, Code.VEX_Vpsubw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E1CD F9 10", 5, Code.VEX_Vpsubw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubwV_VX_HX_WX_2_Data))]
		void Test64_VpsubwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpsubwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F9 D3", 4, Code.VEX_Vpsubw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F9 D3", 4, Code.VEX_Vpsubw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 F9 D3", 4, Code.VEX_Vpsubw_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D F9 D3", 4, Code.VEX_Vpsubw_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 F9 D3", 4, Code.VEX_Vpsubw_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D F9 D3", 4, Code.VEX_Vpsubw_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 F9 D3", 5, Code.VEX_Vpsubw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D F9 D3", 5, Code.VEX_Vpsubw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubwV_VX_k1_HX_WX_1_Data))]
		void Test16_VpsubwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsubwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B F9 50 01", 7, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D8D F9 50 01", 7, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F14D08 F9 50 01", 7, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1CD0B F9 50 01", 7, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F14D2B F9 50 01", 7, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14DAD F9 50 01", 7, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F14D28 F9 50 01", 7, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1CD2B F9 50 01", 7, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F14D4B F9 50 01", 7, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14DCD F9 50 01", 7, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F14D48 F9 50 01", 7, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1CD4B F9 50 01", 7, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubwV_VX_k1_HX_WX_2_Data))]
		void Test16_VpsubwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpsubwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B F9 D3", 6, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B F9 D3", 6, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B F9 D3", 6, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B F9 D3", 6, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB F9 D3", 6, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B F9 D3", 6, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B F9 D3", 6, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB F9 D3", 6, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B F9 D3", 6, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubwV_VX_k1_HX_WX_1_Data))]
		void Test32_VpsubwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsubwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B F9 50 01", 7, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D8D F9 50 01", 7, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F14D08 F9 50 01", 7, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1CD0B F9 50 01", 7, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F14D2B F9 50 01", 7, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14DAD F9 50 01", 7, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F14D28 F9 50 01", 7, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1CD2B F9 50 01", 7, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F14D4B F9 50 01", 7, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14DCD F9 50 01", 7, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F14D48 F9 50 01", 7, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1CD4B F9 50 01", 7, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubwV_VX_k1_HX_WX_2_Data))]
		void Test32_VpsubwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpsubwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B F9 D3", 6, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B F9 D3", 6, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B F9 D3", 6, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B F9 D3", 6, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB F9 D3", 6, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B F9 D3", 6, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B F9 D3", 6, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB F9 D3", 6, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B F9 D3", 6, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubwV_VX_k1_HX_WX_1_Data))]
		void Test64_VpsubwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsubwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B F9 50 01", 7, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D8D F9 50 01", 7, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F14D08 F9 50 01", 7, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1CD0B F9 50 01", 7, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F14D2B F9 50 01", 7, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14DAD F9 50 01", 7, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F14D28 F9 50 01", 7, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1CD2B F9 50 01", 7, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F14D4B F9 50 01", 7, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14DCD F9 50 01", 7, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F14D48 F9 50 01", 7, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1CD4B F9 50 01", 7, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubwV_VX_k1_HX_WX_2_Data))]
		void Test64_VpsubwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpsubwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B F9 D3", 6, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B F9 D3", 6, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 F9 D3", 6, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B F9 D3", 6, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B F9 D3", 6, Code.EVEX_Vpsubw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B F9 D3", 6, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB F9 D3", 6, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 F9 D3", 6, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B F9 D3", 6, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B F9 D3", 6, Code.EVEX_Vpsubw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B F9 D3", 6, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB F9 D3", 6, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 F9 D3", 6, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B F9 D3", 6, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B F9 D3", 6, Code.EVEX_Vpsubw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsubdV_VX_WX_1_Data))]
		void Test16_PsubdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PsubdV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FFA 08", 3, Code.Psubd_P_Q, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0FFA 08", 4, Code.Psubd_VX_WX, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsubdV_VX_WX_2_Data))]
		void Test16_PsubdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PsubdV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FFA CD", 3, Code.Psubd_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FFA CD", 4, Code.Psubd_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsubdV_VX_WX_1_Data))]
		void Test32_PsubdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PsubdV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FFA 08", 3, Code.Psubd_P_Q, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0FFA 08", 4, Code.Psubd_VX_WX, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsubdV_VX_WX_2_Data))]
		void Test32_PsubdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PsubdV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FFA CD", 3, Code.Psubd_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FFA CD", 4, Code.Psubd_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsubdV_VX_WX_1_Data))]
		void Test64_PsubdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PsubdV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FFA 08", 3, Code.Psubd_P_Q, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0FFA 08", 4, Code.Psubd_VX_WX, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsubdV_VX_WX_2_Data))]
		void Test64_PsubdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PsubdV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FFA CD", 3, Code.Psubd_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FFA CD", 4, Code.Psubd_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FFA CD", 5, Code.Psubd_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FFA CD", 5, Code.Psubd_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FFA CD", 5, Code.Psubd_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubdV_VX_HX_WX_1_Data))]
		void Test16_VpsubdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpsubdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 FA 10", 4, Code.VEX_Vpsubd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C5CD FA 10", 4, Code.VEX_Vpsubd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1C9 FA 10", 5, Code.VEX_Vpsubd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1CD FA 10", 5, Code.VEX_Vpsubd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubdV_VX_HX_WX_2_Data))]
		void Test16_VpsubdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpsubdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 FA D3", 4, Code.VEX_Vpsubd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD FA D3", 4, Code.VEX_Vpsubd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubdV_VX_HX_WX_1_Data))]
		void Test32_VpsubdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpsubdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 FA 10", 4, Code.VEX_Vpsubd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C5CD FA 10", 4, Code.VEX_Vpsubd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1C9 FA 10", 5, Code.VEX_Vpsubd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1CD FA 10", 5, Code.VEX_Vpsubd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubdV_VX_HX_WX_2_Data))]
		void Test32_VpsubdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpsubdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 FA D3", 4, Code.VEX_Vpsubd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD FA D3", 4, Code.VEX_Vpsubd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubdV_VX_HX_WX_1_Data))]
		void Test64_VpsubdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpsubdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 FA 10", 4, Code.VEX_Vpsubd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C5CD FA 10", 4, Code.VEX_Vpsubd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1C9 FA 10", 5, Code.VEX_Vpsubd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1CD FA 10", 5, Code.VEX_Vpsubd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubdV_VX_HX_WX_2_Data))]
		void Test64_VpsubdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpsubdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 FA D3", 4, Code.VEX_Vpsubd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD FA D3", 4, Code.VEX_Vpsubd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 FA D3", 4, Code.VEX_Vpsubd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D FA D3", 4, Code.VEX_Vpsubd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 FA D3", 4, Code.VEX_Vpsubd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D FA D3", 4, Code.VEX_Vpsubd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 FA D3", 5, Code.VEX_Vpsubd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D FA D3", 5, Code.VEX_Vpsubd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubdV_VX_k1_HX_WX_1_Data))]
		void Test16_VpsubdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsubdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B FA 50 01", 7, Code.EVEX_Vpsubd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F14D9D FA 50 01", 7, Code.EVEX_Vpsubd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F14D08 FA 50 01", 7, Code.EVEX_Vpsubd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F14D2B FA 50 01", 7, Code.EVEX_Vpsubd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F14DBD FA 50 01", 7, Code.EVEX_Vpsubd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F14D28 FA 50 01", 7, Code.EVEX_Vpsubd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F14D4B FA 50 01", 7, Code.EVEX_Vpsubd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F14DDD FA 50 01", 7, Code.EVEX_Vpsubd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F14D48 FA 50 01", 7, Code.EVEX_Vpsubd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubdV_VX_k1_HX_WX_2_Data))]
		void Test16_VpsubdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsubdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D8B FA D3", 6, Code.EVEX_Vpsubd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F14D0B FA D3", 6, Code.EVEX_Vpsubd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F14DAB FA D3", 6, Code.EVEX_Vpsubd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F14D2B FA D3", 6, Code.EVEX_Vpsubd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F14DCB FA D3", 6, Code.EVEX_Vpsubd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F14D4B FA D3", 6, Code.EVEX_Vpsubd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubdV_VX_k1_HX_WX_1_Data))]
		void Test32_VpsubdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsubdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B FA 50 01", 7, Code.EVEX_Vpsubd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F14D9D FA 50 01", 7, Code.EVEX_Vpsubd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F14D08 FA 50 01", 7, Code.EVEX_Vpsubd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F14D2B FA 50 01", 7, Code.EVEX_Vpsubd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F14DBD FA 50 01", 7, Code.EVEX_Vpsubd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F14D28 FA 50 01", 7, Code.EVEX_Vpsubd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F14D4B FA 50 01", 7, Code.EVEX_Vpsubd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F14DDD FA 50 01", 7, Code.EVEX_Vpsubd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F14D48 FA 50 01", 7, Code.EVEX_Vpsubd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubdV_VX_k1_HX_WX_2_Data))]
		void Test32_VpsubdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsubdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D8B FA D3", 6, Code.EVEX_Vpsubd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F14D0B FA D3", 6, Code.EVEX_Vpsubd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F14DAB FA D3", 6, Code.EVEX_Vpsubd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F14D2B FA D3", 6, Code.EVEX_Vpsubd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F14DCB FA D3", 6, Code.EVEX_Vpsubd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F14D4B FA D3", 6, Code.EVEX_Vpsubd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubdV_VX_k1_HX_WX_1_Data))]
		void Test64_VpsubdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsubdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B FA 50 01", 7, Code.EVEX_Vpsubd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F14D9D FA 50 01", 7, Code.EVEX_Vpsubd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F14D08 FA 50 01", 7, Code.EVEX_Vpsubd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F14D2B FA 50 01", 7, Code.EVEX_Vpsubd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F14DBD FA 50 01", 7, Code.EVEX_Vpsubd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F14D28 FA 50 01", 7, Code.EVEX_Vpsubd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F14D4B FA 50 01", 7, Code.EVEX_Vpsubd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F14DDD FA 50 01", 7, Code.EVEX_Vpsubd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F14D48 FA 50 01", 7, Code.EVEX_Vpsubd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubdV_VX_k1_HX_WX_2_Data))]
		void Test64_VpsubdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsubdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D8B FA D3", 6, Code.EVEX_Vpsubd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E10D0B FA D3", 6, Code.EVEX_Vpsubd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 114D03 FA D3", 6, Code.EVEX_Vpsubd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B14D0B FA D3", 6, Code.EVEX_Vpsubd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F14D0B FA D3", 6, Code.EVEX_Vpsubd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F14DAB FA D3", 6, Code.EVEX_Vpsubd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E10D2B FA D3", 6, Code.EVEX_Vpsubd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 114D23 FA D3", 6, Code.EVEX_Vpsubd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B14D2B FA D3", 6, Code.EVEX_Vpsubd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F14D2B FA D3", 6, Code.EVEX_Vpsubd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F14DCB FA D3", 6, Code.EVEX_Vpsubd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E10D4B FA D3", 6, Code.EVEX_Vpsubd_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 114D43 FA D3", 6, Code.EVEX_Vpsubd_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B14D4B FA D3", 6, Code.EVEX_Vpsubd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F14D4B FA D3", 6, Code.EVEX_Vpsubd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsubqV_VX_WX_1_Data))]
		void Test16_PsubqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PsubqV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FFB 08", 3, Code.Psubq_P_Q, Register.MM1, MemorySize.Int64 };

				yield return new object[] { "66 0FFB 08", 4, Code.Psubq_VX_WX, Register.XMM1, MemorySize.Packed128_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsubqV_VX_WX_2_Data))]
		void Test16_PsubqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PsubqV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FFB CD", 3, Code.Psubq_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FFB CD", 4, Code.Psubq_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsubqV_VX_WX_1_Data))]
		void Test32_PsubqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PsubqV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FFB 08", 3, Code.Psubq_P_Q, Register.MM1, MemorySize.Int64 };

				yield return new object[] { "66 0FFB 08", 4, Code.Psubq_VX_WX, Register.XMM1, MemorySize.Packed128_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsubqV_VX_WX_2_Data))]
		void Test32_PsubqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PsubqV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FFB CD", 3, Code.Psubq_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FFB CD", 4, Code.Psubq_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsubqV_VX_WX_1_Data))]
		void Test64_PsubqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PsubqV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FFB 08", 3, Code.Psubq_P_Q, Register.MM1, MemorySize.Int64 };

				yield return new object[] { "66 0FFB 08", 4, Code.Psubq_VX_WX, Register.XMM1, MemorySize.Packed128_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsubqV_VX_WX_2_Data))]
		void Test64_PsubqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PsubqV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FFB CD", 3, Code.Psubq_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FFB CD", 4, Code.Psubq_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FFB CD", 5, Code.Psubq_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FFB CD", 5, Code.Psubq_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FFB CD", 5, Code.Psubq_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubqV_VX_HX_WX_1_Data))]
		void Test16_VpsubqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpsubqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 FB 10", 4, Code.VEX_Vpsubq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C5CD FB 10", 4, Code.VEX_Vpsubq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
				yield return new object[] { "C4E1C9 FB 10", 5, Code.VEX_Vpsubq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C4E1CD FB 10", 5, Code.VEX_Vpsubq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubqV_VX_HX_WX_2_Data))]
		void Test16_VpsubqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpsubqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 FB D3", 4, Code.VEX_Vpsubq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD FB D3", 4, Code.VEX_Vpsubq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubqV_VX_HX_WX_1_Data))]
		void Test32_VpsubqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpsubqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 FB 10", 4, Code.VEX_Vpsubq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C5CD FB 10", 4, Code.VEX_Vpsubq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
				yield return new object[] { "C4E1C9 FB 10", 5, Code.VEX_Vpsubq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C4E1CD FB 10", 5, Code.VEX_Vpsubq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubqV_VX_HX_WX_2_Data))]
		void Test32_VpsubqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpsubqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 FB D3", 4, Code.VEX_Vpsubq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD FB D3", 4, Code.VEX_Vpsubq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubqV_VX_HX_WX_1_Data))]
		void Test64_VpsubqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpsubqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 FB 10", 4, Code.VEX_Vpsubq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C5CD FB 10", 4, Code.VEX_Vpsubq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
				yield return new object[] { "C4E1C9 FB 10", 5, Code.VEX_Vpsubq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C4E1CD FB 10", 5, Code.VEX_Vpsubq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubqV_VX_HX_WX_2_Data))]
		void Test64_VpsubqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpsubqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 FB D3", 4, Code.VEX_Vpsubq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD FB D3", 4, Code.VEX_Vpsubq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 FB D3", 4, Code.VEX_Vpsubq_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D FB D3", 4, Code.VEX_Vpsubq_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 FB D3", 4, Code.VEX_Vpsubq_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D FB D3", 4, Code.VEX_Vpsubq_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 FB D3", 5, Code.VEX_Vpsubq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D FB D3", 5, Code.VEX_Vpsubq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubqV_VX_k1_HX_WX_1_Data))]
		void Test16_VpsubqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsubqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B FB 50 01", 7, Code.EVEX_Vpsubq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F1CD9D FB 50 01", 7, Code.EVEX_Vpsubq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F1CD08 FB 50 01", 7, Code.EVEX_Vpsubq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F1CD2B FB 50 01", 7, Code.EVEX_Vpsubq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F1CDBD FB 50 01", 7, Code.EVEX_Vpsubq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F1CD28 FB 50 01", 7, Code.EVEX_Vpsubq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F1CD4B FB 50 01", 7, Code.EVEX_Vpsubq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F1CDDD FB 50 01", 7, Code.EVEX_Vpsubq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F1CD48 FB 50 01", 7, Code.EVEX_Vpsubq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubqV_VX_k1_HX_WX_2_Data))]
		void Test16_VpsubqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsubqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F1CD8B FB D3", 6, Code.EVEX_Vpsubq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B FB D3", 6, Code.EVEX_Vpsubq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB FB D3", 6, Code.EVEX_Vpsubq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B FB D3", 6, Code.EVEX_Vpsubq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB FB D3", 6, Code.EVEX_Vpsubq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B FB D3", 6, Code.EVEX_Vpsubq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubqV_VX_k1_HX_WX_1_Data))]
		void Test32_VpsubqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsubqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B FB 50 01", 7, Code.EVEX_Vpsubq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F1CD9D FB 50 01", 7, Code.EVEX_Vpsubq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F1CD08 FB 50 01", 7, Code.EVEX_Vpsubq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F1CD2B FB 50 01", 7, Code.EVEX_Vpsubq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F1CDBD FB 50 01", 7, Code.EVEX_Vpsubq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F1CD28 FB 50 01", 7, Code.EVEX_Vpsubq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F1CD4B FB 50 01", 7, Code.EVEX_Vpsubq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F1CDDD FB 50 01", 7, Code.EVEX_Vpsubq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F1CD48 FB 50 01", 7, Code.EVEX_Vpsubq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubqV_VX_k1_HX_WX_2_Data))]
		void Test32_VpsubqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsubqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F1CD8B FB D3", 6, Code.EVEX_Vpsubq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B FB D3", 6, Code.EVEX_Vpsubq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB FB D3", 6, Code.EVEX_Vpsubq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B FB D3", 6, Code.EVEX_Vpsubq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB FB D3", 6, Code.EVEX_Vpsubq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B FB D3", 6, Code.EVEX_Vpsubq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubqV_VX_k1_HX_WX_1_Data))]
		void Test64_VpsubqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsubqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B FB 50 01", 7, Code.EVEX_Vpsubq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F1CD9D FB 50 01", 7, Code.EVEX_Vpsubq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F1CD08 FB 50 01", 7, Code.EVEX_Vpsubq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F1CD2B FB 50 01", 7, Code.EVEX_Vpsubq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F1CDBD FB 50 01", 7, Code.EVEX_Vpsubq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F1CD28 FB 50 01", 7, Code.EVEX_Vpsubq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F1CD4B FB 50 01", 7, Code.EVEX_Vpsubq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F1CDDD FB 50 01", 7, Code.EVEX_Vpsubq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F1CD48 FB 50 01", 7, Code.EVEX_Vpsubq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubqV_VX_k1_HX_WX_2_Data))]
		void Test64_VpsubqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsubqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F1CD8B FB D3", 6, Code.EVEX_Vpsubq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E18D0B FB D3", 6, Code.EVEX_Vpsubq_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 11CD03 FB D3", 6, Code.EVEX_Vpsubq_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B1CD0B FB D3", 6, Code.EVEX_Vpsubq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F1CD0B FB D3", 6, Code.EVEX_Vpsubq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB FB D3", 6, Code.EVEX_Vpsubq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E18D2B FB D3", 6, Code.EVEX_Vpsubq_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 11CD23 FB D3", 6, Code.EVEX_Vpsubq_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B1CD2B FB D3", 6, Code.EVEX_Vpsubq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F1CD2B FB D3", 6, Code.EVEX_Vpsubq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB FB D3", 6, Code.EVEX_Vpsubq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E18D4B FB D3", 6, Code.EVEX_Vpsubq_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 11CD43 FB D3", 6, Code.EVEX_Vpsubq_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B1CD4B FB D3", 6, Code.EVEX_Vpsubq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F1CD4B FB D3", 6, Code.EVEX_Vpsubq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PaddbV_VX_WX_1_Data))]
		void Test16_PaddbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PaddbV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FFC 08", 3, Code.Paddb_P_Q, Register.MM1, MemorySize.Packed64_Int8 };

				yield return new object[] { "66 0FFC 08", 4, Code.Paddb_VX_WX, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PaddbV_VX_WX_2_Data))]
		void Test16_PaddbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PaddbV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FFC CD", 3, Code.Paddb_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FFC CD", 4, Code.Paddb_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PaddbV_VX_WX_1_Data))]
		void Test32_PaddbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PaddbV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FFC 08", 3, Code.Paddb_P_Q, Register.MM1, MemorySize.Packed64_Int8 };

				yield return new object[] { "66 0FFC 08", 4, Code.Paddb_VX_WX, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PaddbV_VX_WX_2_Data))]
		void Test32_PaddbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PaddbV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FFC CD", 3, Code.Paddb_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FFC CD", 4, Code.Paddb_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PaddbV_VX_WX_1_Data))]
		void Test64_PaddbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PaddbV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FFC 08", 3, Code.Paddb_P_Q, Register.MM1, MemorySize.Packed64_Int8 };

				yield return new object[] { "66 0FFC 08", 4, Code.Paddb_VX_WX, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PaddbV_VX_WX_2_Data))]
		void Test64_PaddbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PaddbV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FFC CD", 3, Code.Paddb_P_Q, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FFC CD", 4, Code.Paddb_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FFC CD", 4, Code.Paddb_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FFC CD", 5, Code.Paddb_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FFC CD", 5, Code.Paddb_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FFC CD", 5, Code.Paddb_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpaddbV_VX_HX_WX_1_Data))]
		void Test16_VpaddbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpaddbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 FC 10", 4, Code.VEX_Vpaddb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C5CD FC 10", 4, Code.VEX_Vpaddb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E1C9 FC 10", 5, Code.VEX_Vpaddb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E1CD FC 10", 5, Code.VEX_Vpaddb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpaddbV_VX_HX_WX_2_Data))]
		void Test16_VpaddbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpaddbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 FC D3", 4, Code.VEX_Vpaddb_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD FC D3", 4, Code.VEX_Vpaddb_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpaddbV_VX_HX_WX_1_Data))]
		void Test32_VpaddbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpaddbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 FC 10", 4, Code.VEX_Vpaddb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C5CD FC 10", 4, Code.VEX_Vpaddb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E1C9 FC 10", 5, Code.VEX_Vpaddb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E1CD FC 10", 5, Code.VEX_Vpaddb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpaddbV_VX_HX_WX_2_Data))]
		void Test32_VpaddbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpaddbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 FC D3", 4, Code.VEX_Vpaddb_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD FC D3", 4, Code.VEX_Vpaddb_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpaddbV_VX_HX_WX_1_Data))]
		void Test64_VpaddbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpaddbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 FC 10", 4, Code.VEX_Vpaddb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C5CD FC 10", 4, Code.VEX_Vpaddb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E1C9 FC 10", 5, Code.VEX_Vpaddb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E1CD FC 10", 5, Code.VEX_Vpaddb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpaddbV_VX_HX_WX_2_Data))]
		void Test64_VpaddbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpaddbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 FC D3", 4, Code.VEX_Vpaddb_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD FC D3", 4, Code.VEX_Vpaddb_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 FC D3", 4, Code.VEX_Vpaddb_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D FC D3", 4, Code.VEX_Vpaddb_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 FC D3", 4, Code.VEX_Vpaddb_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D FC D3", 4, Code.VEX_Vpaddb_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 FC D3", 5, Code.VEX_Vpaddb_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D FC D3", 5, Code.VEX_Vpaddb_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpaddbV_VX_k1_HX_WX_1_Data))]
		void Test16_VpaddbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpaddbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B FC 50 01", 7, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F14D8D FC 50 01", 7, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F14D08 FC 50 01", 7, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F1CD0B FC 50 01", 7, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F14D2B FC 50 01", 7, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F14DAD FC 50 01", 7, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F14D28 FC 50 01", 7, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F1CD2B FC 50 01", 7, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F14D4B FC 50 01", 7, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F14DCD FC 50 01", 7, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F14D48 FC 50 01", 7, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F1CD4B FC 50 01", 7, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpaddbV_VX_k1_HX_WX_2_Data))]
		void Test16_VpaddbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpaddbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B FC D3", 6, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B FC D3", 6, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B FC D3", 6, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B FC D3", 6, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB FC D3", 6, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B FC D3", 6, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B FC D3", 6, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB FC D3", 6, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B FC D3", 6, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpaddbV_VX_k1_HX_WX_1_Data))]
		void Test32_VpaddbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpaddbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B FC 50 01", 7, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F14D8D FC 50 01", 7, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F14D08 FC 50 01", 7, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F1CD0B FC 50 01", 7, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F14D2B FC 50 01", 7, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F14DAD FC 50 01", 7, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F14D28 FC 50 01", 7, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F1CD2B FC 50 01", 7, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F14D4B FC 50 01", 7, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F14DCD FC 50 01", 7, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F14D48 FC 50 01", 7, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F1CD4B FC 50 01", 7, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpaddbV_VX_k1_HX_WX_2_Data))]
		void Test32_VpaddbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpaddbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B FC D3", 6, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B FC D3", 6, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B FC D3", 6, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B FC D3", 6, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB FC D3", 6, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B FC D3", 6, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B FC D3", 6, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB FC D3", 6, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B FC D3", 6, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpaddbV_VX_k1_HX_WX_1_Data))]
		void Test64_VpaddbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpaddbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B FC 50 01", 7, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F14D8D FC 50 01", 7, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F14D08 FC 50 01", 7, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F1CD0B FC 50 01", 7, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F14D2B FC 50 01", 7, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F14DAD FC 50 01", 7, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F14D28 FC 50 01", 7, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F1CD2B FC 50 01", 7, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F14D4B FC 50 01", 7, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F14DCD FC 50 01", 7, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F14D48 FC 50 01", 7, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F1CD4B FC 50 01", 7, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpaddbV_VX_k1_HX_WX_2_Data))]
		void Test64_VpaddbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpaddbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B FC D3", 6, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B FC D3", 6, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 FC D3", 6, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B FC D3", 6, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B FC D3", 6, Code.EVEX_Vpaddb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B FC D3", 6, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB FC D3", 6, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 FC D3", 6, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B FC D3", 6, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B FC D3", 6, Code.EVEX_Vpaddb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B FC D3", 6, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB FC D3", 6, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 FC D3", 6, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B FC D3", 6, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B FC D3", 6, Code.EVEX_Vpaddb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PaddwV_VX_WX_1_Data))]
		void Test16_PaddwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PaddwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FFD 08", 3, Code.Paddw_P_Q, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0FFD 08", 4, Code.Paddw_VX_WX, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PaddwV_VX_WX_2_Data))]
		void Test16_PaddwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PaddwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FFD CD", 3, Code.Paddw_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FFD CD", 4, Code.Paddw_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PaddwV_VX_WX_1_Data))]
		void Test32_PaddwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PaddwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FFD 08", 3, Code.Paddw_P_Q, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0FFD 08", 4, Code.Paddw_VX_WX, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PaddwV_VX_WX_2_Data))]
		void Test32_PaddwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PaddwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FFD CD", 3, Code.Paddw_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FFD CD", 4, Code.Paddw_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PaddwV_VX_WX_1_Data))]
		void Test64_PaddwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PaddwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FFD 08", 3, Code.Paddw_P_Q, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0FFD 08", 4, Code.Paddw_VX_WX, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PaddwV_VX_WX_2_Data))]
		void Test64_PaddwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PaddwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FFD CD", 3, Code.Paddw_P_Q, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FFD CD", 4, Code.Paddw_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FFD CD", 4, Code.Paddw_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FFD CD", 5, Code.Paddw_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FFD CD", 5, Code.Paddw_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FFD CD", 5, Code.Paddw_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpaddwV_VX_HX_WX_1_Data))]
		void Test16_VpaddwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpaddwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 FD 10", 4, Code.VEX_Vpaddw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C5CD FD 10", 4, Code.VEX_Vpaddw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E1C9 FD 10", 5, Code.VEX_Vpaddw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E1CD FD 10", 5, Code.VEX_Vpaddw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpaddwV_VX_HX_WX_2_Data))]
		void Test16_VpaddwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpaddwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 FD D3", 4, Code.VEX_Vpaddw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD FD D3", 4, Code.VEX_Vpaddw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpaddwV_VX_HX_WX_1_Data))]
		void Test32_VpaddwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpaddwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 FD 10", 4, Code.VEX_Vpaddw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C5CD FD 10", 4, Code.VEX_Vpaddw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E1C9 FD 10", 5, Code.VEX_Vpaddw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E1CD FD 10", 5, Code.VEX_Vpaddw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpaddwV_VX_HX_WX_2_Data))]
		void Test32_VpaddwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpaddwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 FD D3", 4, Code.VEX_Vpaddw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD FD D3", 4, Code.VEX_Vpaddw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpaddwV_VX_HX_WX_1_Data))]
		void Test64_VpaddwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpaddwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 FD 10", 4, Code.VEX_Vpaddw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C5CD FD 10", 4, Code.VEX_Vpaddw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E1C9 FD 10", 5, Code.VEX_Vpaddw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E1CD FD 10", 5, Code.VEX_Vpaddw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpaddwV_VX_HX_WX_2_Data))]
		void Test64_VpaddwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpaddwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 FD D3", 4, Code.VEX_Vpaddw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD FD D3", 4, Code.VEX_Vpaddw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 FD D3", 4, Code.VEX_Vpaddw_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D FD D3", 4, Code.VEX_Vpaddw_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 FD D3", 4, Code.VEX_Vpaddw_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D FD D3", 4, Code.VEX_Vpaddw_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 FD D3", 5, Code.VEX_Vpaddw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D FD D3", 5, Code.VEX_Vpaddw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpaddwV_VX_k1_HX_WX_1_Data))]
		void Test16_VpaddwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpaddwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B FD 50 01", 7, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D8D FD 50 01", 7, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F14D08 FD 50 01", 7, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1CD0B FD 50 01", 7, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F14D2B FD 50 01", 7, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14DAD FD 50 01", 7, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F14D28 FD 50 01", 7, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1CD2B FD 50 01", 7, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F14D4B FD 50 01", 7, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14DCD FD 50 01", 7, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F14D48 FD 50 01", 7, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1CD4B FD 50 01", 7, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpaddwV_VX_k1_HX_WX_2_Data))]
		void Test16_VpaddwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpaddwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B FD D3", 6, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B FD D3", 6, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B FD D3", 6, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B FD D3", 6, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB FD D3", 6, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B FD D3", 6, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B FD D3", 6, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB FD D3", 6, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B FD D3", 6, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpaddwV_VX_k1_HX_WX_1_Data))]
		void Test32_VpaddwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpaddwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B FD 50 01", 7, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D8D FD 50 01", 7, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F14D08 FD 50 01", 7, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1CD0B FD 50 01", 7, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F14D2B FD 50 01", 7, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14DAD FD 50 01", 7, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F14D28 FD 50 01", 7, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1CD2B FD 50 01", 7, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F14D4B FD 50 01", 7, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14DCD FD 50 01", 7, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F14D48 FD 50 01", 7, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1CD4B FD 50 01", 7, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpaddwV_VX_k1_HX_WX_2_Data))]
		void Test32_VpaddwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpaddwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B FD D3", 6, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B FD D3", 6, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B FD D3", 6, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B FD D3", 6, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB FD D3", 6, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B FD D3", 6, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B FD D3", 6, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB FD D3", 6, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B FD D3", 6, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpaddwV_VX_k1_HX_WX_1_Data))]
		void Test64_VpaddwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpaddwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B FD 50 01", 7, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D8D FD 50 01", 7, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F14D08 FD 50 01", 7, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1CD0B FD 50 01", 7, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F14D2B FD 50 01", 7, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14DAD FD 50 01", 7, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F14D28 FD 50 01", 7, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1CD2B FD 50 01", 7, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F14D4B FD 50 01", 7, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14DCD FD 50 01", 7, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F14D48 FD 50 01", 7, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1CD4B FD 50 01", 7, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpaddwV_VX_k1_HX_WX_2_Data))]
		void Test64_VpaddwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpaddwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B FD D3", 6, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B FD D3", 6, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 FD D3", 6, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B FD D3", 6, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B FD D3", 6, Code.EVEX_Vpaddw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B FD D3", 6, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB FD D3", 6, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 FD D3", 6, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B FD D3", 6, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B FD D3", 6, Code.EVEX_Vpaddw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B FD D3", 6, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB FD D3", 6, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 FD D3", 6, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B FD D3", 6, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B FD D3", 6, Code.EVEX_Vpaddw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PadddV_VX_WX_1_Data))]
		void Test16_PadddV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PadddV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FFE 08", 3, Code.Paddd_P_Q, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0FFE 08", 4, Code.Paddd_VX_WX, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PadddV_VX_WX_2_Data))]
		void Test16_PadddV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PadddV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FFE CD", 3, Code.Paddd_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FFE CD", 4, Code.Paddd_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PadddV_VX_WX_1_Data))]
		void Test32_PadddV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PadddV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FFE 08", 3, Code.Paddd_P_Q, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0FFE 08", 4, Code.Paddd_VX_WX, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PadddV_VX_WX_2_Data))]
		void Test32_PadddV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PadddV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FFE CD", 3, Code.Paddd_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FFE CD", 4, Code.Paddd_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PadddV_VX_WX_1_Data))]
		void Test64_PadddV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PadddV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FFE 08", 3, Code.Paddd_P_Q, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0FFE 08", 4, Code.Paddd_VX_WX, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PadddV_VX_WX_2_Data))]
		void Test64_PadddV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PadddV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FFE CD", 3, Code.Paddd_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FFE CD", 4, Code.Paddd_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FFE CD", 5, Code.Paddd_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FFE CD", 5, Code.Paddd_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FFE CD", 5, Code.Paddd_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpadddV_VX_HX_WX_1_Data))]
		void Test16_VpadddV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpadddV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 FE 10", 4, Code.VEX_Vpaddd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C5CD FE 10", 4, Code.VEX_Vpaddd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1C9 FE 10", 5, Code.VEX_Vpaddd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1CD FE 10", 5, Code.VEX_Vpaddd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpadddV_VX_HX_WX_2_Data))]
		void Test16_VpadddV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpadddV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 FE D3", 4, Code.VEX_Vpaddd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD FE D3", 4, Code.VEX_Vpaddd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpadddV_VX_HX_WX_1_Data))]
		void Test32_VpadddV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpadddV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 FE 10", 4, Code.VEX_Vpaddd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C5CD FE 10", 4, Code.VEX_Vpaddd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1C9 FE 10", 5, Code.VEX_Vpaddd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1CD FE 10", 5, Code.VEX_Vpaddd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpadddV_VX_HX_WX_2_Data))]
		void Test32_VpadddV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpadddV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 FE D3", 4, Code.VEX_Vpaddd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD FE D3", 4, Code.VEX_Vpaddd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpadddV_VX_HX_WX_1_Data))]
		void Test64_VpadddV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpadddV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 FE 10", 4, Code.VEX_Vpaddd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C5CD FE 10", 4, Code.VEX_Vpaddd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1C9 FE 10", 5, Code.VEX_Vpaddd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1CD FE 10", 5, Code.VEX_Vpaddd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpadddV_VX_HX_WX_2_Data))]
		void Test64_VpadddV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpadddV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 FE D3", 4, Code.VEX_Vpaddd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD FE D3", 4, Code.VEX_Vpaddd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 FE D3", 4, Code.VEX_Vpaddd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D FE D3", 4, Code.VEX_Vpaddd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 FE D3", 4, Code.VEX_Vpaddd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D FE D3", 4, Code.VEX_Vpaddd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 FE D3", 5, Code.VEX_Vpaddd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D FE D3", 5, Code.VEX_Vpaddd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpadddV_VX_k1_HX_WX_1_Data))]
		void Test16_VpadddV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpadddV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B FE 50 01", 7, Code.EVEX_Vpaddd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F14D9D FE 50 01", 7, Code.EVEX_Vpaddd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F14D08 FE 50 01", 7, Code.EVEX_Vpaddd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F14D2B FE 50 01", 7, Code.EVEX_Vpaddd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F14DBD FE 50 01", 7, Code.EVEX_Vpaddd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F14D28 FE 50 01", 7, Code.EVEX_Vpaddd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F14D4B FE 50 01", 7, Code.EVEX_Vpaddd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F14DDD FE 50 01", 7, Code.EVEX_Vpaddd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F14D48 FE 50 01", 7, Code.EVEX_Vpaddd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpadddV_VX_k1_HX_WX_2_Data))]
		void Test16_VpadddV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpadddV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D8B FE D3", 6, Code.EVEX_Vpaddd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F14D0B FE D3", 6, Code.EVEX_Vpaddd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F14DAB FE D3", 6, Code.EVEX_Vpaddd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F14D2B FE D3", 6, Code.EVEX_Vpaddd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F14DCB FE D3", 6, Code.EVEX_Vpaddd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F14D4B FE D3", 6, Code.EVEX_Vpaddd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpadddV_VX_k1_HX_WX_1_Data))]
		void Test32_VpadddV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpadddV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B FE 50 01", 7, Code.EVEX_Vpaddd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F14D9D FE 50 01", 7, Code.EVEX_Vpaddd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F14D08 FE 50 01", 7, Code.EVEX_Vpaddd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F14D2B FE 50 01", 7, Code.EVEX_Vpaddd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F14DBD FE 50 01", 7, Code.EVEX_Vpaddd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F14D28 FE 50 01", 7, Code.EVEX_Vpaddd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F14D4B FE 50 01", 7, Code.EVEX_Vpaddd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F14DDD FE 50 01", 7, Code.EVEX_Vpaddd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F14D48 FE 50 01", 7, Code.EVEX_Vpaddd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpadddV_VX_k1_HX_WX_2_Data))]
		void Test32_VpadddV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpadddV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D8B FE D3", 6, Code.EVEX_Vpaddd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F14D0B FE D3", 6, Code.EVEX_Vpaddd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F14DAB FE D3", 6, Code.EVEX_Vpaddd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F14D2B FE D3", 6, Code.EVEX_Vpaddd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F14DCB FE D3", 6, Code.EVEX_Vpaddd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F14D4B FE D3", 6, Code.EVEX_Vpaddd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpadddV_VX_k1_HX_WX_1_Data))]
		void Test64_VpadddV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpadddV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B FE 50 01", 7, Code.EVEX_Vpaddd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F14D9D FE 50 01", 7, Code.EVEX_Vpaddd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F14D08 FE 50 01", 7, Code.EVEX_Vpaddd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F14D2B FE 50 01", 7, Code.EVEX_Vpaddd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F14DBD FE 50 01", 7, Code.EVEX_Vpaddd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F14D28 FE 50 01", 7, Code.EVEX_Vpaddd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F14D4B FE 50 01", 7, Code.EVEX_Vpaddd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F14DDD FE 50 01", 7, Code.EVEX_Vpaddd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F14D48 FE 50 01", 7, Code.EVEX_Vpaddd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpadddV_VX_k1_HX_WX_2_Data))]
		void Test64_VpadddV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpadddV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D8B FE D3", 6, Code.EVEX_Vpaddd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E10D0B FE D3", 6, Code.EVEX_Vpaddd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 114D03 FE D3", 6, Code.EVEX_Vpaddd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B14D0B FE D3", 6, Code.EVEX_Vpaddd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F14D0B FE D3", 6, Code.EVEX_Vpaddd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F14DAB FE D3", 6, Code.EVEX_Vpaddd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E10D2B FE D3", 6, Code.EVEX_Vpaddd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 114D23 FE D3", 6, Code.EVEX_Vpaddd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B14D2B FE D3", 6, Code.EVEX_Vpaddd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F14D2B FE D3", 6, Code.EVEX_Vpaddd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F14DCB FE D3", 6, Code.EVEX_Vpaddd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E10D4B FE D3", 6, Code.EVEX_Vpaddd_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 114D43 FE D3", 6, Code.EVEX_Vpaddd_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B14D4B FE D3", 6, Code.EVEX_Vpaddd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F14D4B FE D3", 6, Code.EVEX_Vpaddd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Fact]
		void Test16_Ud0_Gw_Ew_1() {
			var decoder = CreateDecoder16("0FFF CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Ud0_Gw_Ew, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.CX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.SI, instr.Op1Register);
		}

		[Fact]
		void Test16_Ud0_Gw_Ew_2() {
			var decoder = CreateDecoder16("0FFF 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Ud0_Gw_Ew, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.BX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test32_Ud0_Gw_Ew_1() {
			var decoder = CreateDecoder32("66 0FFF CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Ud0_Gw_Ew, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.CX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.SI, instr.Op1Register);
		}

		[Fact]
		void Test32_Ud0_Gw_Ew_2() {
			var decoder = CreateDecoder32("66 0FFF 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Ud0_Gw_Ew, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.BX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("66 0FFF CE", 4, Register.CX, Register.SI)]
		[InlineData("66 44 0FFF C5", 5, Register.R8W, Register.BP)]
		[InlineData("66 41 0FFF D6", 5, Register.DX, Register.R14W)]
		[InlineData("66 45 0FFF D0", 5, Register.R10W, Register.R8W)]
		[InlineData("66 41 0FFF D9", 5, Register.BX, Register.R9W)]
		[InlineData("66 44 0FFF EC", 5, Register.R13W, Register.SP)]
		void Test64_Ud0_Gw_Ew_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Ud0_Gw_Ew, instr.Code);
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

		[Fact]
		void Test64_Ud0_Gw_Ew_2() {
			var decoder = CreateDecoder64("66 0FFF 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Ud0_Gw_Ew, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.BX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt16, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test16_Ud0_Gd_Ed_1() {
			var decoder = CreateDecoder16("66 0FFF CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Ud0_Gd_Ed, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);
		}

		[Fact]
		void Test16_Ud0_Gd_Ed_2() {
			var decoder = CreateDecoder16("66 0FFF 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Ud0_Gd_Ed, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EBX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Fact]
		void Test32_Ud0_Gd_Ed_1() {
			var decoder = CreateDecoder32("0FFF CE");
			var instr = decoder.Decode();

			Assert.Equal(Code.Ud0_Gd_Ed, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.ECX, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(Register.ESI, instr.Op1Register);
		}

		[Fact]
		void Test32_Ud0_Gd_Ed_2() {
			var decoder = CreateDecoder32("0FFF 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Ud0_Gd_Ed, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EBX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("0FFF CE", 3, Register.ECX, Register.ESI)]
		[InlineData("44 0FFF C5", 4, Register.R8D, Register.EBP)]
		[InlineData("41 0FFF D6", 4, Register.EDX, Register.R14D)]
		[InlineData("45 0FFF D0", 4, Register.R10D, Register.R8D)]
		[InlineData("41 0FFF D9", 4, Register.EBX, Register.R9D)]
		[InlineData("44 0FFF EC", 4, Register.R13D, Register.ESP)]
		void Test64_Ud0_Gd_Ed_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Ud0_Gd_Ed, instr.Code);
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

		[Fact]
		void Test64_Ud0_Gd_Ed_2() {
			var decoder = CreateDecoder64("0FFF 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Ud0_Gd_Ed, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(3, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.EBX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt32, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}

		[Theory]
		[InlineData("48 0FFF CE", 4, Register.RCX, Register.RSI)]
		[InlineData("4C 0FFF C5", 4, Register.R8, Register.RBP)]
		[InlineData("49 0FFF D6", 4, Register.RDX, Register.R14)]
		[InlineData("4D 0FFF D0", 4, Register.R10, Register.R8)]
		[InlineData("49 0FFF D9", 4, Register.RBX, Register.R9)]
		[InlineData("4C 0FFF EC", 4, Register.R13, Register.RSP)]
		void Test64_Ud0_Gq_Eq_1(string hexBytes, int byteLength, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(Code.Ud0_Gq_Eq, instr.Code);
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

		[Fact]
		void Test64_Ud0_Gq_Eq_2() {
			var decoder = CreateDecoder64("48 0FFF 18");
			var instr = decoder.Decode();

			Assert.Equal(Code.Ud0_Gq_Eq, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(4, instr.ByteLength);
			Assert.False(instr.HasPrefixRepe);
			Assert.False(instr.HasPrefixRepne);
			Assert.False(instr.HasPrefixLock);
			Assert.Equal(Register.None, instr.PrefixSegment);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(Register.RBX, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(0, instr.MemoryIndexScale);
			Assert.Equal(MemorySize.UInt64, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);
		}
	}
}
