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
	public sealed class DecoderTest_2_0FD8_0FDF : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_PsubusbV_VX_WX_1_Data))]
		void Test16_PsubusbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PsubusbV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FD8 08", 3, Code.Psubusb_P_Q, Register.MM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "66 0FD8 08", 4, Code.Psubusb_VX_WX, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsubusbV_VX_WX_2_Data))]
		void Test16_PsubusbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PsubusbV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FD8 CD", 3, Code.Psubusb_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FD8 CD", 4, Code.Psubusb_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsubusbV_VX_WX_1_Data))]
		void Test32_PsubusbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PsubusbV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FD8 08", 3, Code.Psubusb_P_Q, Register.MM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "66 0FD8 08", 4, Code.Psubusb_VX_WX, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsubusbV_VX_WX_2_Data))]
		void Test32_PsubusbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PsubusbV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FD8 CD", 3, Code.Psubusb_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FD8 CD", 4, Code.Psubusb_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsubusbV_VX_WX_1_Data))]
		void Test64_PsubusbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PsubusbV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FD8 08", 3, Code.Psubusb_P_Q, Register.MM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "66 0FD8 08", 4, Code.Psubusb_VX_WX, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsubusbV_VX_WX_2_Data))]
		void Test64_PsubusbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PsubusbV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FD8 CD", 3, Code.Psubusb_P_Q, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FD8 CD", 4, Code.Psubusb_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FD8 CD", 4, Code.Psubusb_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FD8 CD", 5, Code.Psubusb_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FD8 CD", 5, Code.Psubusb_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FD8 CD", 5, Code.Psubusb_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubusbV_VX_HX_WX_1_Data))]
		void Test16_VpsubusbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpsubusbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D8 10", 4, Code.VEX_Vpsubusb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C5CD D8 10", 4, Code.VEX_Vpsubusb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
				yield return new object[] { "C4E1C9 D8 10", 5, Code.VEX_Vpsubusb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E1CD D8 10", 5, Code.VEX_Vpsubusb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubusbV_VX_HX_WX_2_Data))]
		void Test16_VpsubusbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpsubusbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D8 D3", 4, Code.VEX_Vpsubusb_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D8 D3", 4, Code.VEX_Vpsubusb_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubusbV_VX_HX_WX_1_Data))]
		void Test32_VpsubusbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpsubusbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D8 10", 4, Code.VEX_Vpsubusb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C5CD D8 10", 4, Code.VEX_Vpsubusb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
				yield return new object[] { "C4E1C9 D8 10", 5, Code.VEX_Vpsubusb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E1CD D8 10", 5, Code.VEX_Vpsubusb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubusbV_VX_HX_WX_2_Data))]
		void Test32_VpsubusbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpsubusbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D8 D3", 4, Code.VEX_Vpsubusb_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D8 D3", 4, Code.VEX_Vpsubusb_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubusbV_VX_HX_WX_1_Data))]
		void Test64_VpsubusbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpsubusbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D8 10", 4, Code.VEX_Vpsubusb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C5CD D8 10", 4, Code.VEX_Vpsubusb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
				yield return new object[] { "C4E1C9 D8 10", 5, Code.VEX_Vpsubusb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E1CD D8 10", 5, Code.VEX_Vpsubusb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubusbV_VX_HX_WX_2_Data))]
		void Test64_VpsubusbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpsubusbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D8 D3", 4, Code.VEX_Vpsubusb_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D8 D3", 4, Code.VEX_Vpsubusb_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 D8 D3", 4, Code.VEX_Vpsubusb_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D D8 D3", 4, Code.VEX_Vpsubusb_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 D8 D3", 4, Code.VEX_Vpsubusb_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D D8 D3", 4, Code.VEX_Vpsubusb_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 D8 D3", 5, Code.VEX_Vpsubusb_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D D8 D3", 5, Code.VEX_Vpsubusb_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubusbV_VX_k1_HX_WX_1_Data))]
		void Test16_VpsubusbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsubusbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B D8 50 01", 7, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F14D8D D8 50 01", 7, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F14D08 D8 50 01", 7, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F1CD0B D8 50 01", 7, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F14D2B D8 50 01", 7, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F14DAD D8 50 01", 7, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F14D28 D8 50 01", 7, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F1CD2B D8 50 01", 7, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F14D4B D8 50 01", 7, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F14DCD D8 50 01", 7, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true };
				yield return new object[] { "62 F14D48 D8 50 01", 7, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F1CD4B D8 50 01", 7, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubusbV_VX_k1_HX_WX_2_Data))]
		void Test16_VpsubusbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpsubusbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B D8 D3", 6, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B D8 D3", 6, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B D8 D3", 6, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B D8 D3", 6, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB D8 D3", 6, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B D8 D3", 6, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B D8 D3", 6, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB D8 D3", 6, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B D8 D3", 6, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubusbV_VX_k1_HX_WX_1_Data))]
		void Test32_VpsubusbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsubusbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B D8 50 01", 7, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F14D8D D8 50 01", 7, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F14D08 D8 50 01", 7, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F1CD0B D8 50 01", 7, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F14D2B D8 50 01", 7, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F14DAD D8 50 01", 7, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F14D28 D8 50 01", 7, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F1CD2B D8 50 01", 7, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F14D4B D8 50 01", 7, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F14DCD D8 50 01", 7, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true };
				yield return new object[] { "62 F14D48 D8 50 01", 7, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F1CD4B D8 50 01", 7, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubusbV_VX_k1_HX_WX_2_Data))]
		void Test32_VpsubusbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpsubusbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B D8 D3", 6, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B D8 D3", 6, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B D8 D3", 6, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B D8 D3", 6, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB D8 D3", 6, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B D8 D3", 6, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B D8 D3", 6, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB D8 D3", 6, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B D8 D3", 6, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubusbV_VX_k1_HX_WX_1_Data))]
		void Test64_VpsubusbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsubusbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B D8 50 01", 7, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F14D8D D8 50 01", 7, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F14D08 D8 50 01", 7, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F1CD0B D8 50 01", 7, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F14D2B D8 50 01", 7, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F14DAD D8 50 01", 7, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F14D28 D8 50 01", 7, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F1CD2B D8 50 01", 7, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F14D4B D8 50 01", 7, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F14DCD D8 50 01", 7, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true };
				yield return new object[] { "62 F14D48 D8 50 01", 7, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F1CD4B D8 50 01", 7, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubusbV_VX_k1_HX_WX_2_Data))]
		void Test64_VpsubusbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpsubusbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B D8 D3", 6, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B D8 D3", 6, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 D8 D3", 6, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B D8 D3", 6, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B D8 D3", 6, Code.EVEX_Vpsubusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B D8 D3", 6, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB D8 D3", 6, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 D8 D3", 6, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B D8 D3", 6, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B D8 D3", 6, Code.EVEX_Vpsubusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B D8 D3", 6, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB D8 D3", 6, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 D8 D3", 6, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B D8 D3", 6, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B D8 D3", 6, Code.EVEX_Vpsubusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsubuswV_VX_WX_1_Data))]
		void Test16_PsubuswV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PsubuswV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FD9 08", 3, Code.Psubusw_P_Q, Register.MM1, MemorySize.Packed64_UInt16 };

				yield return new object[] { "66 0FD9 08", 4, Code.Psubusw_VX_WX, Register.XMM1, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsubuswV_VX_WX_2_Data))]
		void Test16_PsubuswV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PsubuswV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FD9 CD", 3, Code.Psubusw_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FD9 CD", 4, Code.Psubusw_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsubuswV_VX_WX_1_Data))]
		void Test32_PsubuswV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PsubuswV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FD9 08", 3, Code.Psubusw_P_Q, Register.MM1, MemorySize.Packed64_UInt16 };

				yield return new object[] { "66 0FD9 08", 4, Code.Psubusw_VX_WX, Register.XMM1, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsubuswV_VX_WX_2_Data))]
		void Test32_PsubuswV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PsubuswV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FD9 CD", 3, Code.Psubusw_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FD9 CD", 4, Code.Psubusw_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsubuswV_VX_WX_1_Data))]
		void Test64_PsubuswV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PsubuswV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FD9 08", 3, Code.Psubusw_P_Q, Register.MM1, MemorySize.Packed64_UInt16 };

				yield return new object[] { "66 0FD9 08", 4, Code.Psubusw_VX_WX, Register.XMM1, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsubuswV_VX_WX_2_Data))]
		void Test64_PsubuswV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PsubuswV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FD9 CD", 3, Code.Psubusw_P_Q, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FD9 CD", 4, Code.Psubusw_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FD9 CD", 4, Code.Psubusw_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FD9 CD", 5, Code.Psubusw_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FD9 CD", 5, Code.Psubusw_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FD9 CD", 5, Code.Psubusw_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubuswV_VX_HX_WX_1_Data))]
		void Test16_VpsubuswV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpsubuswV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D9 10", 4, Code.VEX_Vpsubusw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C5CD D9 10", 4, Code.VEX_Vpsubusw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
				yield return new object[] { "C4E1C9 D9 10", 5, Code.VEX_Vpsubusw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E1CD D9 10", 5, Code.VEX_Vpsubusw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubuswV_VX_HX_WX_2_Data))]
		void Test16_VpsubuswV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpsubuswV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D9 D3", 4, Code.VEX_Vpsubusw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D9 D3", 4, Code.VEX_Vpsubusw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubuswV_VX_HX_WX_1_Data))]
		void Test32_VpsubuswV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpsubuswV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D9 10", 4, Code.VEX_Vpsubusw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C5CD D9 10", 4, Code.VEX_Vpsubusw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
				yield return new object[] { "C4E1C9 D9 10", 5, Code.VEX_Vpsubusw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E1CD D9 10", 5, Code.VEX_Vpsubusw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubuswV_VX_HX_WX_2_Data))]
		void Test32_VpsubuswV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpsubuswV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D9 D3", 4, Code.VEX_Vpsubusw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D9 D3", 4, Code.VEX_Vpsubusw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubuswV_VX_HX_WX_1_Data))]
		void Test64_VpsubuswV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpsubuswV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 D9 10", 4, Code.VEX_Vpsubusw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C5CD D9 10", 4, Code.VEX_Vpsubusw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
				yield return new object[] { "C4E1C9 D9 10", 5, Code.VEX_Vpsubusw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E1CD D9 10", 5, Code.VEX_Vpsubusw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubuswV_VX_HX_WX_2_Data))]
		void Test64_VpsubuswV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpsubuswV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 D9 D3", 4, Code.VEX_Vpsubusw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD D9 D3", 4, Code.VEX_Vpsubusw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 D9 D3", 4, Code.VEX_Vpsubusw_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D D9 D3", 4, Code.VEX_Vpsubusw_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 D9 D3", 4, Code.VEX_Vpsubusw_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D D9 D3", 4, Code.VEX_Vpsubusw_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 D9 D3", 5, Code.VEX_Vpsubusw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D D9 D3", 5, Code.VEX_Vpsubusw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubuswV_VX_k1_HX_WX_1_Data))]
		void Test16_VpsubuswV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsubuswV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B D9 50 01", 7, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F14D8D D9 50 01", 7, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F14D08 D9 50 01", 7, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F1CD0B D9 50 01", 7, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F14D2B D9 50 01", 7, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F14DAD D9 50 01", 7, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F14D28 D9 50 01", 7, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F1CD2B D9 50 01", 7, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F14D4B D9 50 01", 7, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F14DCD D9 50 01", 7, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F14D48 D9 50 01", 7, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F1CD4B D9 50 01", 7, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsubuswV_VX_k1_HX_WX_2_Data))]
		void Test16_VpsubuswV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpsubuswV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B D9 D3", 6, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B D9 D3", 6, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B D9 D3", 6, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B D9 D3", 6, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB D9 D3", 6, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B D9 D3", 6, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B D9 D3", 6, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB D9 D3", 6, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B D9 D3", 6, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubuswV_VX_k1_HX_WX_1_Data))]
		void Test32_VpsubuswV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsubuswV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B D9 50 01", 7, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F14D8D D9 50 01", 7, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F14D08 D9 50 01", 7, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F1CD0B D9 50 01", 7, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F14D2B D9 50 01", 7, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F14DAD D9 50 01", 7, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F14D28 D9 50 01", 7, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F1CD2B D9 50 01", 7, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F14D4B D9 50 01", 7, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F14DCD D9 50 01", 7, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F14D48 D9 50 01", 7, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F1CD4B D9 50 01", 7, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsubuswV_VX_k1_HX_WX_2_Data))]
		void Test32_VpsubuswV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpsubuswV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B D9 D3", 6, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B D9 D3", 6, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B D9 D3", 6, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B D9 D3", 6, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB D9 D3", 6, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B D9 D3", 6, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B D9 D3", 6, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB D9 D3", 6, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B D9 D3", 6, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubuswV_VX_k1_HX_WX_1_Data))]
		void Test64_VpsubuswV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsubuswV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B D9 50 01", 7, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F14D8D D9 50 01", 7, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F14D08 D9 50 01", 7, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F1CD0B D9 50 01", 7, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F14D2B D9 50 01", 7, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F14DAD D9 50 01", 7, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F14D28 D9 50 01", 7, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F1CD2B D9 50 01", 7, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F14D4B D9 50 01", 7, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F14DCD D9 50 01", 7, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F14D48 D9 50 01", 7, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F1CD4B D9 50 01", 7, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsubuswV_VX_k1_HX_WX_2_Data))]
		void Test64_VpsubuswV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpsubuswV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B D9 D3", 6, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B D9 D3", 6, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 D9 D3", 6, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B D9 D3", 6, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B D9 D3", 6, Code.EVEX_Vpsubusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B D9 D3", 6, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB D9 D3", 6, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 D9 D3", 6, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B D9 D3", 6, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B D9 D3", 6, Code.EVEX_Vpsubusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B D9 D3", 6, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB D9 D3", 6, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 D9 D3", 6, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B D9 D3", 6, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B D9 D3", 6, Code.EVEX_Vpsubusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PminubV_VX_WX_1_Data))]
		void Test16_PminubV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PminubV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FDA 08", 3, Code.Pminub_P_Q, Register.MM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "66 0FDA 08", 4, Code.Pminub_VX_WX, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PminubV_VX_WX_2_Data))]
		void Test16_PminubV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PminubV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FDA CD", 3, Code.Pminub_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FDA CD", 4, Code.Pminub_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PminubV_VX_WX_1_Data))]
		void Test32_PminubV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PminubV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FDA 08", 3, Code.Pminub_P_Q, Register.MM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "66 0FDA 08", 4, Code.Pminub_VX_WX, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PminubV_VX_WX_2_Data))]
		void Test32_PminubV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PminubV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FDA CD", 3, Code.Pminub_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FDA CD", 4, Code.Pminub_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PminubV_VX_WX_1_Data))]
		void Test64_PminubV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PminubV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FDA 08", 3, Code.Pminub_P_Q, Register.MM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "66 0FDA 08", 4, Code.Pminub_VX_WX, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PminubV_VX_WX_2_Data))]
		void Test64_PminubV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PminubV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FDA CD", 3, Code.Pminub_P_Q, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FDA CD", 4, Code.Pminub_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FDA CD", 4, Code.Pminub_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FDA CD", 5, Code.Pminub_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FDA CD", 5, Code.Pminub_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FDA CD", 5, Code.Pminub_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpminubV_VX_HX_WX_1_Data))]
		void Test16_VpminubV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpminubV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 DA 10", 4, Code.VEX_Vpminub_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C5CD DA 10", 4, Code.VEX_Vpminub_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
				yield return new object[] { "C4E1C9 DA 10", 5, Code.VEX_Vpminub_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E1CD DA 10", 5, Code.VEX_Vpminub_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpminubV_VX_HX_WX_2_Data))]
		void Test16_VpminubV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpminubV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 DA D3", 4, Code.VEX_Vpminub_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD DA D3", 4, Code.VEX_Vpminub_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpminubV_VX_HX_WX_1_Data))]
		void Test32_VpminubV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpminubV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 DA 10", 4, Code.VEX_Vpminub_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C5CD DA 10", 4, Code.VEX_Vpminub_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
				yield return new object[] { "C4E1C9 DA 10", 5, Code.VEX_Vpminub_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E1CD DA 10", 5, Code.VEX_Vpminub_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpminubV_VX_HX_WX_2_Data))]
		void Test32_VpminubV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpminubV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 DA D3", 4, Code.VEX_Vpminub_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD DA D3", 4, Code.VEX_Vpminub_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpminubV_VX_HX_WX_1_Data))]
		void Test64_VpminubV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpminubV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 DA 10", 4, Code.VEX_Vpminub_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C5CD DA 10", 4, Code.VEX_Vpminub_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
				yield return new object[] { "C4E1C9 DA 10", 5, Code.VEX_Vpminub_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E1CD DA 10", 5, Code.VEX_Vpminub_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpminubV_VX_HX_WX_2_Data))]
		void Test64_VpminubV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpminubV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 DA D3", 4, Code.VEX_Vpminub_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD DA D3", 4, Code.VEX_Vpminub_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 DA D3", 4, Code.VEX_Vpminub_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D DA D3", 4, Code.VEX_Vpminub_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 DA D3", 4, Code.VEX_Vpminub_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D DA D3", 4, Code.VEX_Vpminub_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 DA D3", 5, Code.VEX_Vpminub_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D DA D3", 5, Code.VEX_Vpminub_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpminubV_VX_k1_HX_WX_1_Data))]
		void Test16_VpminubV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpminubV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B DA 50 01", 7, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F14D8D DA 50 01", 7, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F14D08 DA 50 01", 7, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F1CD0B DA 50 01", 7, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F14D2B DA 50 01", 7, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F14DAD DA 50 01", 7, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F14D28 DA 50 01", 7, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F1CD2B DA 50 01", 7, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F14D4B DA 50 01", 7, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F14DCD DA 50 01", 7, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true };
				yield return new object[] { "62 F14D48 DA 50 01", 7, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F1CD4B DA 50 01", 7, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpminubV_VX_k1_HX_WX_2_Data))]
		void Test16_VpminubV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpminubV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B DA D3", 6, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B DA D3", 6, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B DA D3", 6, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B DA D3", 6, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB DA D3", 6, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B DA D3", 6, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B DA D3", 6, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB DA D3", 6, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B DA D3", 6, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpminubV_VX_k1_HX_WX_1_Data))]
		void Test32_VpminubV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpminubV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B DA 50 01", 7, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F14D8D DA 50 01", 7, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F14D08 DA 50 01", 7, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F1CD0B DA 50 01", 7, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F14D2B DA 50 01", 7, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F14DAD DA 50 01", 7, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F14D28 DA 50 01", 7, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F1CD2B DA 50 01", 7, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F14D4B DA 50 01", 7, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F14DCD DA 50 01", 7, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true };
				yield return new object[] { "62 F14D48 DA 50 01", 7, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F1CD4B DA 50 01", 7, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpminubV_VX_k1_HX_WX_2_Data))]
		void Test32_VpminubV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpminubV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B DA D3", 6, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B DA D3", 6, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B DA D3", 6, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B DA D3", 6, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB DA D3", 6, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B DA D3", 6, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B DA D3", 6, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB DA D3", 6, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B DA D3", 6, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpminubV_VX_k1_HX_WX_1_Data))]
		void Test64_VpminubV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpminubV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B DA 50 01", 7, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F14D8D DA 50 01", 7, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F14D08 DA 50 01", 7, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F1CD0B DA 50 01", 7, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F14D2B DA 50 01", 7, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F14DAD DA 50 01", 7, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F14D28 DA 50 01", 7, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F1CD2B DA 50 01", 7, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F14D4B DA 50 01", 7, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F14DCD DA 50 01", 7, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true };
				yield return new object[] { "62 F14D48 DA 50 01", 7, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F1CD4B DA 50 01", 7, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpminubV_VX_k1_HX_WX_2_Data))]
		void Test64_VpminubV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpminubV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B DA D3", 6, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B DA D3", 6, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 DA D3", 6, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B DA D3", 6, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B DA D3", 6, Code.EVEX_Vpminub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B DA D3", 6, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB DA D3", 6, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 DA D3", 6, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B DA D3", 6, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B DA D3", 6, Code.EVEX_Vpminub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B DA D3", 6, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB DA D3", 6, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 DA D3", 6, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B DA D3", 6, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B DA D3", 6, Code.EVEX_Vpminub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PandV_VX_WX_1_Data))]
		void Test16_PandV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PandV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FDB 08", 3, Code.Pand_P_Q, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FDB 08", 4, Code.Pand_VX_WX, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PandV_VX_WX_2_Data))]
		void Test16_PandV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PandV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FDB CD", 3, Code.Pand_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FDB CD", 4, Code.Pand_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PandV_VX_WX_1_Data))]
		void Test32_PandV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PandV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FDB 08", 3, Code.Pand_P_Q, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FDB 08", 4, Code.Pand_VX_WX, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PandV_VX_WX_2_Data))]
		void Test32_PandV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PandV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FDB CD", 3, Code.Pand_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FDB CD", 4, Code.Pand_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PandV_VX_WX_1_Data))]
		void Test64_PandV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PandV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FDB 08", 3, Code.Pand_P_Q, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FDB 08", 4, Code.Pand_VX_WX, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PandV_VX_WX_2_Data))]
		void Test64_PandV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PandV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FDB CD", 3, Code.Pand_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FDB CD", 4, Code.Pand_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FDB CD", 5, Code.Pand_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FDB CD", 5, Code.Pand_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FDB CD", 5, Code.Pand_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpandV_VX_HX_WX_1_Data))]
		void Test16_VpandV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpandV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 DB 10", 4, Code.VEX_Vpand_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD DB 10", 4, Code.VEX_Vpand_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
				yield return new object[] { "C4E1C9 DB 10", 5, Code.VEX_Vpand_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD DB 10", 5, Code.VEX_Vpand_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpandV_VX_HX_WX_2_Data))]
		void Test16_VpandV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpandV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 DB D3", 4, Code.VEX_Vpand_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD DB D3", 4, Code.VEX_Vpand_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpandV_VX_HX_WX_1_Data))]
		void Test32_VpandV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpandV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 DB 10", 4, Code.VEX_Vpand_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD DB 10", 4, Code.VEX_Vpand_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
				yield return new object[] { "C4E1C9 DB 10", 5, Code.VEX_Vpand_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD DB 10", 5, Code.VEX_Vpand_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpandV_VX_HX_WX_2_Data))]
		void Test32_VpandV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpandV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 DB D3", 4, Code.VEX_Vpand_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD DB D3", 4, Code.VEX_Vpand_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpandV_VX_HX_WX_1_Data))]
		void Test64_VpandV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpandV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 DB 10", 4, Code.VEX_Vpand_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD DB 10", 4, Code.VEX_Vpand_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
				yield return new object[] { "C4E1C9 DB 10", 5, Code.VEX_Vpand_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD DB 10", 5, Code.VEX_Vpand_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpandV_VX_HX_WX_2_Data))]
		void Test64_VpandV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpandV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 DB D3", 4, Code.VEX_Vpand_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD DB D3", 4, Code.VEX_Vpand_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 DB D3", 4, Code.VEX_Vpand_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D DB D3", 4, Code.VEX_Vpand_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 DB D3", 4, Code.VEX_Vpand_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D DB D3", 4, Code.VEX_Vpand_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 DB D3", 5, Code.VEX_Vpand_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D DB D3", 5, Code.VEX_Vpand_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpandV_VX_k1_HX_WX_1_Data))]
		void Test16_VpandV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpandV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B DB 50 01", 7, Code.EVEX_Vpandd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F14D9D DB 50 01", 7, Code.EVEX_Vpandd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F14D08 DB 50 01", 7, Code.EVEX_Vpandd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F14D2B DB 50 01", 7, Code.EVEX_Vpandd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F14DBD DB 50 01", 7, Code.EVEX_Vpandd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F14D28 DB 50 01", 7, Code.EVEX_Vpandd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F14D4B DB 50 01", 7, Code.EVEX_Vpandd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F14DDD DB 50 01", 7, Code.EVEX_Vpandd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F14D48 DB 50 01", 7, Code.EVEX_Vpandd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F1CD0B DB 50 01", 7, Code.EVEX_Vpandq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD9D DB 50 01", 7, Code.EVEX_Vpandq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F1CD08 DB 50 01", 7, Code.EVEX_Vpandq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD2B DB 50 01", 7, Code.EVEX_Vpandq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F1CDBD DB 50 01", 7, Code.EVEX_Vpandq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F1CD28 DB 50 01", 7, Code.EVEX_Vpandq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F1CD4B DB 50 01", 7, Code.EVEX_Vpandq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F1CDDD DB 50 01", 7, Code.EVEX_Vpandq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F1CD48 DB 50 01", 7, Code.EVEX_Vpandq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpandV_VX_k1_HX_WX_2_Data))]
		void Test16_VpandV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpandV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D8B DB D3", 6, Code.EVEX_Vpandd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F14D0B DB D3", 6, Code.EVEX_Vpandd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F14DAB DB D3", 6, Code.EVEX_Vpandd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F14D2B DB D3", 6, Code.EVEX_Vpandd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F14DCB DB D3", 6, Code.EVEX_Vpandd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F14D4B DB D3", 6, Code.EVEX_Vpandd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F1CD8B DB D3", 6, Code.EVEX_Vpandq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B DB D3", 6, Code.EVEX_Vpandq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB DB D3", 6, Code.EVEX_Vpandq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B DB D3", 6, Code.EVEX_Vpandq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB DB D3", 6, Code.EVEX_Vpandq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B DB D3", 6, Code.EVEX_Vpandq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpandV_VX_k1_HX_WX_1_Data))]
		void Test32_VpandV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpandV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B DB 50 01", 7, Code.EVEX_Vpandd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F14D9D DB 50 01", 7, Code.EVEX_Vpandd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F14D08 DB 50 01", 7, Code.EVEX_Vpandd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F14D2B DB 50 01", 7, Code.EVEX_Vpandd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F14DBD DB 50 01", 7, Code.EVEX_Vpandd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F14D28 DB 50 01", 7, Code.EVEX_Vpandd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F14D4B DB 50 01", 7, Code.EVEX_Vpandd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F14DDD DB 50 01", 7, Code.EVEX_Vpandd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F14D48 DB 50 01", 7, Code.EVEX_Vpandd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F1CD0B DB 50 01", 7, Code.EVEX_Vpandq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD9D DB 50 01", 7, Code.EVEX_Vpandq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F1CD08 DB 50 01", 7, Code.EVEX_Vpandq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD2B DB 50 01", 7, Code.EVEX_Vpandq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F1CDBD DB 50 01", 7, Code.EVEX_Vpandq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F1CD28 DB 50 01", 7, Code.EVEX_Vpandq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F1CD4B DB 50 01", 7, Code.EVEX_Vpandq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F1CDDD DB 50 01", 7, Code.EVEX_Vpandq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F1CD48 DB 50 01", 7, Code.EVEX_Vpandq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpandV_VX_k1_HX_WX_2_Data))]
		void Test32_VpandV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpandV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D8B DB D3", 6, Code.EVEX_Vpandd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F14D0B DB D3", 6, Code.EVEX_Vpandd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F14DAB DB D3", 6, Code.EVEX_Vpandd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F14D2B DB D3", 6, Code.EVEX_Vpandd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F14DCB DB D3", 6, Code.EVEX_Vpandd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F14D4B DB D3", 6, Code.EVEX_Vpandd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F1CD8B DB D3", 6, Code.EVEX_Vpandq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B DB D3", 6, Code.EVEX_Vpandq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB DB D3", 6, Code.EVEX_Vpandq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B DB D3", 6, Code.EVEX_Vpandq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB DB D3", 6, Code.EVEX_Vpandq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B DB D3", 6, Code.EVEX_Vpandq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpandV_VX_k1_HX_WX_1_Data))]
		void Test64_VpandV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpandV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B DB 50 01", 7, Code.EVEX_Vpandd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F14D9D DB 50 01", 7, Code.EVEX_Vpandd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F14D08 DB 50 01", 7, Code.EVEX_Vpandd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F14D2B DB 50 01", 7, Code.EVEX_Vpandd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F14DBD DB 50 01", 7, Code.EVEX_Vpandd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F14D28 DB 50 01", 7, Code.EVEX_Vpandd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F14D4B DB 50 01", 7, Code.EVEX_Vpandd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F14DDD DB 50 01", 7, Code.EVEX_Vpandd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F14D48 DB 50 01", 7, Code.EVEX_Vpandd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F1CD0B DB 50 01", 7, Code.EVEX_Vpandq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD9D DB 50 01", 7, Code.EVEX_Vpandq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F1CD08 DB 50 01", 7, Code.EVEX_Vpandq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD2B DB 50 01", 7, Code.EVEX_Vpandq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F1CDBD DB 50 01", 7, Code.EVEX_Vpandq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F1CD28 DB 50 01", 7, Code.EVEX_Vpandq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F1CD4B DB 50 01", 7, Code.EVEX_Vpandq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F1CDDD DB 50 01", 7, Code.EVEX_Vpandq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F1CD48 DB 50 01", 7, Code.EVEX_Vpandq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpandV_VX_k1_HX_WX_2_Data))]
		void Test64_VpandV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpandV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D8B DB D3", 6, Code.EVEX_Vpandd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E10D0B DB D3", 6, Code.EVEX_Vpandd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 114D03 DB D3", 6, Code.EVEX_Vpandd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B14D0B DB D3", 6, Code.EVEX_Vpandd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F14D0B DB D3", 6, Code.EVEX_Vpandd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F14DAB DB D3", 6, Code.EVEX_Vpandd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E10D2B DB D3", 6, Code.EVEX_Vpandd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 114D23 DB D3", 6, Code.EVEX_Vpandd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B14D2B DB D3", 6, Code.EVEX_Vpandd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F14D2B DB D3", 6, Code.EVEX_Vpandd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F14DCB DB D3", 6, Code.EVEX_Vpandd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E10D4B DB D3", 6, Code.EVEX_Vpandd_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 114D43 DB D3", 6, Code.EVEX_Vpandd_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B14D4B DB D3", 6, Code.EVEX_Vpandd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F14D4B DB D3", 6, Code.EVEX_Vpandd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F1CD8B DB D3", 6, Code.EVEX_Vpandq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E18D0B DB D3", 6, Code.EVEX_Vpandq_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 11CD03 DB D3", 6, Code.EVEX_Vpandq_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B1CD0B DB D3", 6, Code.EVEX_Vpandq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F1CD0B DB D3", 6, Code.EVEX_Vpandq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB DB D3", 6, Code.EVEX_Vpandq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E18D2B DB D3", 6, Code.EVEX_Vpandq_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 11CD23 DB D3", 6, Code.EVEX_Vpandq_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B1CD2B DB D3", 6, Code.EVEX_Vpandq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F1CD2B DB D3", 6, Code.EVEX_Vpandq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB DB D3", 6, Code.EVEX_Vpandq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E18D4B DB D3", 6, Code.EVEX_Vpandq_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 11CD43 DB D3", 6, Code.EVEX_Vpandq_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B1CD4B DB D3", 6, Code.EVEX_Vpandq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F1CD4B DB D3", 6, Code.EVEX_Vpandq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PaddusbV_VX_WX_1_Data))]
		void Test16_PaddusbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PaddusbV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FDC 08", 3, Code.Paddusb_P_Q, Register.MM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "66 0FDC 08", 4, Code.Paddusb_VX_WX, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PaddusbV_VX_WX_2_Data))]
		void Test16_PaddusbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PaddusbV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FDC CD", 3, Code.Paddusb_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FDC CD", 4, Code.Paddusb_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PaddusbV_VX_WX_1_Data))]
		void Test32_PaddusbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PaddusbV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FDC 08", 3, Code.Paddusb_P_Q, Register.MM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "66 0FDC 08", 4, Code.Paddusb_VX_WX, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PaddusbV_VX_WX_2_Data))]
		void Test32_PaddusbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PaddusbV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FDC CD", 3, Code.Paddusb_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FDC CD", 4, Code.Paddusb_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PaddusbV_VX_WX_1_Data))]
		void Test64_PaddusbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PaddusbV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FDC 08", 3, Code.Paddusb_P_Q, Register.MM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "66 0FDC 08", 4, Code.Paddusb_VX_WX, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PaddusbV_VX_WX_2_Data))]
		void Test64_PaddusbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PaddusbV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FDC CD", 3, Code.Paddusb_P_Q, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FDC CD", 4, Code.Paddusb_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FDC CD", 4, Code.Paddusb_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FDC CD", 5, Code.Paddusb_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FDC CD", 5, Code.Paddusb_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FDC CD", 5, Code.Paddusb_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpaddusbV_VX_HX_WX_1_Data))]
		void Test16_VpaddusbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpaddusbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 DC 10", 4, Code.VEX_Vpaddusb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C5CD DC 10", 4, Code.VEX_Vpaddusb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
				yield return new object[] { "C4E1C9 DC 10", 5, Code.VEX_Vpaddusb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E1CD DC 10", 5, Code.VEX_Vpaddusb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpaddusbV_VX_HX_WX_2_Data))]
		void Test16_VpaddusbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpaddusbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 DC D3", 4, Code.VEX_Vpaddusb_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD DC D3", 4, Code.VEX_Vpaddusb_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpaddusbV_VX_HX_WX_1_Data))]
		void Test32_VpaddusbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpaddusbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 DC 10", 4, Code.VEX_Vpaddusb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C5CD DC 10", 4, Code.VEX_Vpaddusb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
				yield return new object[] { "C4E1C9 DC 10", 5, Code.VEX_Vpaddusb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E1CD DC 10", 5, Code.VEX_Vpaddusb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpaddusbV_VX_HX_WX_2_Data))]
		void Test32_VpaddusbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpaddusbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 DC D3", 4, Code.VEX_Vpaddusb_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD DC D3", 4, Code.VEX_Vpaddusb_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpaddusbV_VX_HX_WX_1_Data))]
		void Test64_VpaddusbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpaddusbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 DC 10", 4, Code.VEX_Vpaddusb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C5CD DC 10", 4, Code.VEX_Vpaddusb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
				yield return new object[] { "C4E1C9 DC 10", 5, Code.VEX_Vpaddusb_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E1CD DC 10", 5, Code.VEX_Vpaddusb_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpaddusbV_VX_HX_WX_2_Data))]
		void Test64_VpaddusbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpaddusbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 DC D3", 4, Code.VEX_Vpaddusb_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD DC D3", 4, Code.VEX_Vpaddusb_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 DC D3", 4, Code.VEX_Vpaddusb_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D DC D3", 4, Code.VEX_Vpaddusb_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 DC D3", 4, Code.VEX_Vpaddusb_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D DC D3", 4, Code.VEX_Vpaddusb_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 DC D3", 5, Code.VEX_Vpaddusb_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D DC D3", 5, Code.VEX_Vpaddusb_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpaddusbV_VX_k1_HX_WX_1_Data))]
		void Test16_VpaddusbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpaddusbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B DC 50 01", 7, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F14D8D DC 50 01", 7, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F14D08 DC 50 01", 7, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F1CD0B DC 50 01", 7, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F14D2B DC 50 01", 7, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F14DAD DC 50 01", 7, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F14D28 DC 50 01", 7, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F1CD2B DC 50 01", 7, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F14D4B DC 50 01", 7, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F14DCD DC 50 01", 7, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true };
				yield return new object[] { "62 F14D48 DC 50 01", 7, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F1CD4B DC 50 01", 7, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpaddusbV_VX_k1_HX_WX_2_Data))]
		void Test16_VpaddusbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpaddusbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B DC D3", 6, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B DC D3", 6, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B DC D3", 6, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B DC D3", 6, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB DC D3", 6, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B DC D3", 6, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B DC D3", 6, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB DC D3", 6, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B DC D3", 6, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpaddusbV_VX_k1_HX_WX_1_Data))]
		void Test32_VpaddusbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpaddusbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B DC 50 01", 7, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F14D8D DC 50 01", 7, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F14D08 DC 50 01", 7, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F1CD0B DC 50 01", 7, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F14D2B DC 50 01", 7, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F14DAD DC 50 01", 7, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F14D28 DC 50 01", 7, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F1CD2B DC 50 01", 7, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F14D4B DC 50 01", 7, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F14DCD DC 50 01", 7, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true };
				yield return new object[] { "62 F14D48 DC 50 01", 7, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F1CD4B DC 50 01", 7, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpaddusbV_VX_k1_HX_WX_2_Data))]
		void Test32_VpaddusbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpaddusbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B DC D3", 6, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B DC D3", 6, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B DC D3", 6, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B DC D3", 6, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB DC D3", 6, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B DC D3", 6, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B DC D3", 6, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB DC D3", 6, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B DC D3", 6, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpaddusbV_VX_k1_HX_WX_1_Data))]
		void Test64_VpaddusbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpaddusbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B DC 50 01", 7, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F14D8D DC 50 01", 7, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F14D08 DC 50 01", 7, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F1CD0B DC 50 01", 7, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F14D2B DC 50 01", 7, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F14DAD DC 50 01", 7, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F14D28 DC 50 01", 7, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F1CD2B DC 50 01", 7, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F14D4B DC 50 01", 7, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F14DCD DC 50 01", 7, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true };
				yield return new object[] { "62 F14D48 DC 50 01", 7, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F1CD4B DC 50 01", 7, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpaddusbV_VX_k1_HX_WX_2_Data))]
		void Test64_VpaddusbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpaddusbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B DC D3", 6, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B DC D3", 6, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 DC D3", 6, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B DC D3", 6, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B DC D3", 6, Code.EVEX_Vpaddusb_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B DC D3", 6, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB DC D3", 6, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 DC D3", 6, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B DC D3", 6, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B DC D3", 6, Code.EVEX_Vpaddusb_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B DC D3", 6, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB DC D3", 6, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 DC D3", 6, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B DC D3", 6, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B DC D3", 6, Code.EVEX_Vpaddusb_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PadduswV_VX_WX_1_Data))]
		void Test16_PadduswV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PadduswV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FDD 08", 3, Code.Paddusw_P_Q, Register.MM1, MemorySize.Packed64_UInt16 };

				yield return new object[] { "66 0FDD 08", 4, Code.Paddusw_VX_WX, Register.XMM1, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PadduswV_VX_WX_2_Data))]
		void Test16_PadduswV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PadduswV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FDD CD", 3, Code.Paddusw_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FDD CD", 4, Code.Paddusw_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PadduswV_VX_WX_1_Data))]
		void Test32_PadduswV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PadduswV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FDD 08", 3, Code.Paddusw_P_Q, Register.MM1, MemorySize.Packed64_UInt16 };

				yield return new object[] { "66 0FDD 08", 4, Code.Paddusw_VX_WX, Register.XMM1, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PadduswV_VX_WX_2_Data))]
		void Test32_PadduswV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PadduswV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FDD CD", 3, Code.Paddusw_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FDD CD", 4, Code.Paddusw_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PadduswV_VX_WX_1_Data))]
		void Test64_PadduswV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PadduswV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FDD 08", 3, Code.Paddusw_P_Q, Register.MM1, MemorySize.Packed64_UInt16 };

				yield return new object[] { "66 0FDD 08", 4, Code.Paddusw_VX_WX, Register.XMM1, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PadduswV_VX_WX_2_Data))]
		void Test64_PadduswV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PadduswV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FDD CD", 3, Code.Paddusw_P_Q, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FDD CD", 4, Code.Paddusw_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FDD CD", 4, Code.Paddusw_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FDD CD", 5, Code.Paddusw_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FDD CD", 5, Code.Paddusw_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FDD CD", 5, Code.Paddusw_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpadduswV_VX_HX_WX_1_Data))]
		void Test16_VpadduswV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpadduswV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 DD 10", 4, Code.VEX_Vpaddusw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C5CD DD 10", 4, Code.VEX_Vpaddusw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
				yield return new object[] { "C4E1C9 DD 10", 5, Code.VEX_Vpaddusw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E1CD DD 10", 5, Code.VEX_Vpaddusw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpadduswV_VX_HX_WX_2_Data))]
		void Test16_VpadduswV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpadduswV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 DD D3", 4, Code.VEX_Vpaddusw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD DD D3", 4, Code.VEX_Vpaddusw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpadduswV_VX_HX_WX_1_Data))]
		void Test32_VpadduswV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpadduswV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 DD 10", 4, Code.VEX_Vpaddusw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C5CD DD 10", 4, Code.VEX_Vpaddusw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
				yield return new object[] { "C4E1C9 DD 10", 5, Code.VEX_Vpaddusw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E1CD DD 10", 5, Code.VEX_Vpaddusw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpadduswV_VX_HX_WX_2_Data))]
		void Test32_VpadduswV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpadduswV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 DD D3", 4, Code.VEX_Vpaddusw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD DD D3", 4, Code.VEX_Vpaddusw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpadduswV_VX_HX_WX_1_Data))]
		void Test64_VpadduswV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpadduswV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 DD 10", 4, Code.VEX_Vpaddusw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C5CD DD 10", 4, Code.VEX_Vpaddusw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
				yield return new object[] { "C4E1C9 DD 10", 5, Code.VEX_Vpaddusw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E1CD DD 10", 5, Code.VEX_Vpaddusw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpadduswV_VX_HX_WX_2_Data))]
		void Test64_VpadduswV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpadduswV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 DD D3", 4, Code.VEX_Vpaddusw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD DD D3", 4, Code.VEX_Vpaddusw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 DD D3", 4, Code.VEX_Vpaddusw_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D DD D3", 4, Code.VEX_Vpaddusw_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 DD D3", 4, Code.VEX_Vpaddusw_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D DD D3", 4, Code.VEX_Vpaddusw_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 DD D3", 5, Code.VEX_Vpaddusw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D DD D3", 5, Code.VEX_Vpaddusw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpadduswV_VX_k1_HX_WX_1_Data))]
		void Test16_VpadduswV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpadduswV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B DD 50 01", 7, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F14D8D DD 50 01", 7, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F14D08 DD 50 01", 7, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F1CD0B DD 50 01", 7, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F14D2B DD 50 01", 7, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F14DAD DD 50 01", 7, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F14D28 DD 50 01", 7, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F1CD2B DD 50 01", 7, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F14D4B DD 50 01", 7, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F14DCD DD 50 01", 7, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F14D48 DD 50 01", 7, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F1CD4B DD 50 01", 7, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpadduswV_VX_k1_HX_WX_2_Data))]
		void Test16_VpadduswV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpadduswV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B DD D3", 6, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B DD D3", 6, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B DD D3", 6, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B DD D3", 6, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB DD D3", 6, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B DD D3", 6, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B DD D3", 6, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB DD D3", 6, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B DD D3", 6, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpadduswV_VX_k1_HX_WX_1_Data))]
		void Test32_VpadduswV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpadduswV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B DD 50 01", 7, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F14D8D DD 50 01", 7, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F14D08 DD 50 01", 7, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F1CD0B DD 50 01", 7, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F14D2B DD 50 01", 7, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F14DAD DD 50 01", 7, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F14D28 DD 50 01", 7, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F1CD2B DD 50 01", 7, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F14D4B DD 50 01", 7, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F14DCD DD 50 01", 7, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F14D48 DD 50 01", 7, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F1CD4B DD 50 01", 7, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpadduswV_VX_k1_HX_WX_2_Data))]
		void Test32_VpadduswV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpadduswV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B DD D3", 6, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B DD D3", 6, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B DD D3", 6, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B DD D3", 6, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB DD D3", 6, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B DD D3", 6, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B DD D3", 6, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB DD D3", 6, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B DD D3", 6, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpadduswV_VX_k1_HX_WX_1_Data))]
		void Test64_VpadduswV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpadduswV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B DD 50 01", 7, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F14D8D DD 50 01", 7, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F14D08 DD 50 01", 7, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F1CD0B DD 50 01", 7, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F14D2B DD 50 01", 7, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F14DAD DD 50 01", 7, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F14D28 DD 50 01", 7, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F1CD2B DD 50 01", 7, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F14D4B DD 50 01", 7, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F14DCD DD 50 01", 7, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F14D48 DD 50 01", 7, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F1CD4B DD 50 01", 7, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpadduswV_VX_k1_HX_WX_2_Data))]
		void Test64_VpadduswV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpadduswV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B DD D3", 6, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B DD D3", 6, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 DD D3", 6, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B DD D3", 6, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B DD D3", 6, Code.EVEX_Vpaddusw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B DD D3", 6, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB DD D3", 6, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 DD D3", 6, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B DD D3", 6, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B DD D3", 6, Code.EVEX_Vpaddusw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B DD D3", 6, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB DD D3", 6, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 DD D3", 6, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B DD D3", 6, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B DD D3", 6, Code.EVEX_Vpaddusw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmaxubV_VX_WX_1_Data))]
		void Test16_PmaxubV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PmaxubV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FDE 08", 3, Code.Pmaxub_P_Q, Register.MM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "66 0FDE 08", 4, Code.Pmaxub_VX_WX, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmaxubV_VX_WX_2_Data))]
		void Test16_PmaxubV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PmaxubV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FDE CD", 3, Code.Pmaxub_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FDE CD", 4, Code.Pmaxub_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmaxubV_VX_WX_1_Data))]
		void Test32_PmaxubV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PmaxubV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FDE 08", 3, Code.Pmaxub_P_Q, Register.MM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "66 0FDE 08", 4, Code.Pmaxub_VX_WX, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmaxubV_VX_WX_2_Data))]
		void Test32_PmaxubV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PmaxubV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FDE CD", 3, Code.Pmaxub_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FDE CD", 4, Code.Pmaxub_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmaxubV_VX_WX_1_Data))]
		void Test64_PmaxubV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PmaxubV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FDE 08", 3, Code.Pmaxub_P_Q, Register.MM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "66 0FDE 08", 4, Code.Pmaxub_VX_WX, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmaxubV_VX_WX_2_Data))]
		void Test64_PmaxubV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PmaxubV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FDE CD", 3, Code.Pmaxub_P_Q, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FDE CD", 4, Code.Pmaxub_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FDE CD", 4, Code.Pmaxub_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FDE CD", 5, Code.Pmaxub_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FDE CD", 5, Code.Pmaxub_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FDE CD", 5, Code.Pmaxub_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaxubV_VX_HX_WX_1_Data))]
		void Test16_VpmaxubV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmaxubV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 DE 10", 4, Code.VEX_Vpmaxub_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C5CD DE 10", 4, Code.VEX_Vpmaxub_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
				yield return new object[] { "C4E1C9 DE 10", 5, Code.VEX_Vpmaxub_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E1CD DE 10", 5, Code.VEX_Vpmaxub_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaxubV_VX_HX_WX_2_Data))]
		void Test16_VpmaxubV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpmaxubV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 DE D3", 4, Code.VEX_Vpmaxub_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD DE D3", 4, Code.VEX_Vpmaxub_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaxubV_VX_HX_WX_1_Data))]
		void Test32_VpmaxubV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmaxubV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 DE 10", 4, Code.VEX_Vpmaxub_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C5CD DE 10", 4, Code.VEX_Vpmaxub_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
				yield return new object[] { "C4E1C9 DE 10", 5, Code.VEX_Vpmaxub_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E1CD DE 10", 5, Code.VEX_Vpmaxub_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaxubV_VX_HX_WX_2_Data))]
		void Test32_VpmaxubV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpmaxubV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 DE D3", 4, Code.VEX_Vpmaxub_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD DE D3", 4, Code.VEX_Vpmaxub_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaxubV_VX_HX_WX_1_Data))]
		void Test64_VpmaxubV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmaxubV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 DE 10", 4, Code.VEX_Vpmaxub_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C5CD DE 10", 4, Code.VEX_Vpmaxub_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
				yield return new object[] { "C4E1C9 DE 10", 5, Code.VEX_Vpmaxub_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E1CD DE 10", 5, Code.VEX_Vpmaxub_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaxubV_VX_HX_WX_2_Data))]
		void Test64_VpmaxubV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpmaxubV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 DE D3", 4, Code.VEX_Vpmaxub_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD DE D3", 4, Code.VEX_Vpmaxub_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 DE D3", 4, Code.VEX_Vpmaxub_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D DE D3", 4, Code.VEX_Vpmaxub_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 DE D3", 4, Code.VEX_Vpmaxub_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D DE D3", 4, Code.VEX_Vpmaxub_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 DE D3", 5, Code.VEX_Vpmaxub_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D DE D3", 5, Code.VEX_Vpmaxub_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaxubV_VX_k1_HX_WX_1_Data))]
		void Test16_VpmaxubV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpmaxubV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B DE 50 01", 7, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F14D8D DE 50 01", 7, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F14D08 DE 50 01", 7, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F1CD0B DE 50 01", 7, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F14D2B DE 50 01", 7, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F14DAD DE 50 01", 7, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F14D28 DE 50 01", 7, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F1CD2B DE 50 01", 7, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F14D4B DE 50 01", 7, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F14DCD DE 50 01", 7, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true };
				yield return new object[] { "62 F14D48 DE 50 01", 7, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F1CD4B DE 50 01", 7, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaxubV_VX_k1_HX_WX_2_Data))]
		void Test16_VpmaxubV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpmaxubV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B DE D3", 6, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B DE D3", 6, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B DE D3", 6, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B DE D3", 6, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB DE D3", 6, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B DE D3", 6, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B DE D3", 6, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB DE D3", 6, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B DE D3", 6, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaxubV_VX_k1_HX_WX_1_Data))]
		void Test32_VpmaxubV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpmaxubV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B DE 50 01", 7, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F14D8D DE 50 01", 7, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F14D08 DE 50 01", 7, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F1CD0B DE 50 01", 7, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F14D2B DE 50 01", 7, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F14DAD DE 50 01", 7, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F14D28 DE 50 01", 7, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F1CD2B DE 50 01", 7, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F14D4B DE 50 01", 7, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F14DCD DE 50 01", 7, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true };
				yield return new object[] { "62 F14D48 DE 50 01", 7, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F1CD4B DE 50 01", 7, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaxubV_VX_k1_HX_WX_2_Data))]
		void Test32_VpmaxubV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpmaxubV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B DE D3", 6, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B DE D3", 6, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B DE D3", 6, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B DE D3", 6, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB DE D3", 6, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B DE D3", 6, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B DE D3", 6, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB DE D3", 6, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B DE D3", 6, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaxubV_VX_k1_HX_WX_1_Data))]
		void Test64_VpmaxubV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpmaxubV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B DE 50 01", 7, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F14D8D DE 50 01", 7, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F14D08 DE 50 01", 7, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F1CD0B DE 50 01", 7, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F14D2B DE 50 01", 7, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F14DAD DE 50 01", 7, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F14D28 DE 50 01", 7, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F1CD2B DE 50 01", 7, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F14D4B DE 50 01", 7, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F14DCD DE 50 01", 7, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true };
				yield return new object[] { "62 F14D48 DE 50 01", 7, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F1CD4B DE 50 01", 7, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaxubV_VX_k1_HX_WX_2_Data))]
		void Test64_VpmaxubV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpmaxubV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B DE D3", 6, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B DE D3", 6, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 DE D3", 6, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B DE D3", 6, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B DE D3", 6, Code.EVEX_Vpmaxub_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B DE D3", 6, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB DE D3", 6, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 DE D3", 6, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B DE D3", 6, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B DE D3", 6, Code.EVEX_Vpmaxub_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B DE D3", 6, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB DE D3", 6, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 DE D3", 6, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B DE D3", 6, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B DE D3", 6, Code.EVEX_Vpmaxub_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PandnV_VX_WX_1_Data))]
		void Test16_PandnV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PandnV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FDF 08", 3, Code.Pandn_P_Q, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FDF 08", 4, Code.Pandn_VX_WX, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PandnV_VX_WX_2_Data))]
		void Test16_PandnV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PandnV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FDF CD", 3, Code.Pandn_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FDF CD", 4, Code.Pandn_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PandnV_VX_WX_1_Data))]
		void Test32_PandnV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PandnV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FDF 08", 3, Code.Pandn_P_Q, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FDF 08", 4, Code.Pandn_VX_WX, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PandnV_VX_WX_2_Data))]
		void Test32_PandnV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PandnV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FDF CD", 3, Code.Pandn_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FDF CD", 4, Code.Pandn_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PandnV_VX_WX_1_Data))]
		void Test64_PandnV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PandnV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FDF 08", 3, Code.Pandn_P_Q, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FDF 08", 4, Code.Pandn_VX_WX, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PandnV_VX_WX_2_Data))]
		void Test64_PandnV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PandnV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FDF CD", 3, Code.Pandn_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FDF CD", 4, Code.Pandn_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FDF CD", 5, Code.Pandn_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FDF CD", 5, Code.Pandn_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FDF CD", 5, Code.Pandn_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpandnV_VX_HX_WX_1_Data))]
		void Test16_VpandnV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpandnV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 DF 10", 4, Code.VEX_Vpandn_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD DF 10", 4, Code.VEX_Vpandn_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
				yield return new object[] { "C4E1C9 DF 10", 5, Code.VEX_Vpandn_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD DF 10", 5, Code.VEX_Vpandn_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpandnV_VX_HX_WX_2_Data))]
		void Test16_VpandnV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpandnV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 DF D3", 4, Code.VEX_Vpandn_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD DF D3", 4, Code.VEX_Vpandn_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpandnV_VX_HX_WX_1_Data))]
		void Test32_VpandnV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpandnV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 DF 10", 4, Code.VEX_Vpandn_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD DF 10", 4, Code.VEX_Vpandn_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
				yield return new object[] { "C4E1C9 DF 10", 5, Code.VEX_Vpandn_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD DF 10", 5, Code.VEX_Vpandn_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpandnV_VX_HX_WX_2_Data))]
		void Test32_VpandnV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpandnV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 DF D3", 4, Code.VEX_Vpandn_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD DF D3", 4, Code.VEX_Vpandn_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpandnV_VX_HX_WX_1_Data))]
		void Test64_VpandnV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpandnV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 DF 10", 4, Code.VEX_Vpandn_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD DF 10", 4, Code.VEX_Vpandn_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
				yield return new object[] { "C4E1C9 DF 10", 5, Code.VEX_Vpandn_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD DF 10", 5, Code.VEX_Vpandn_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpandnV_VX_HX_WX_2_Data))]
		void Test64_VpandnV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpandnV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 DF D3", 4, Code.VEX_Vpandn_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD DF D3", 4, Code.VEX_Vpandn_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 DF D3", 4, Code.VEX_Vpandn_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D DF D3", 4, Code.VEX_Vpandn_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 DF D3", 4, Code.VEX_Vpandn_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D DF D3", 4, Code.VEX_Vpandn_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 DF D3", 5, Code.VEX_Vpandn_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D DF D3", 5, Code.VEX_Vpandn_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpandnV_VX_k1_HX_WX_1_Data))]
		void Test16_VpandnV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpandnV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B DF 50 01", 7, Code.EVEX_Vpandnd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F14D9D DF 50 01", 7, Code.EVEX_Vpandnd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F14D08 DF 50 01", 7, Code.EVEX_Vpandnd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F14D2B DF 50 01", 7, Code.EVEX_Vpandnd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F14DBD DF 50 01", 7, Code.EVEX_Vpandnd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F14D28 DF 50 01", 7, Code.EVEX_Vpandnd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F14D4B DF 50 01", 7, Code.EVEX_Vpandnd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F14DDD DF 50 01", 7, Code.EVEX_Vpandnd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F14D48 DF 50 01", 7, Code.EVEX_Vpandnd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F1CD0B DF 50 01", 7, Code.EVEX_Vpandnq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD9D DF 50 01", 7, Code.EVEX_Vpandnq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F1CD08 DF 50 01", 7, Code.EVEX_Vpandnq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD2B DF 50 01", 7, Code.EVEX_Vpandnq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F1CDBD DF 50 01", 7, Code.EVEX_Vpandnq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F1CD28 DF 50 01", 7, Code.EVEX_Vpandnq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F1CD4B DF 50 01", 7, Code.EVEX_Vpandnq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F1CDDD DF 50 01", 7, Code.EVEX_Vpandnq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F1CD48 DF 50 01", 7, Code.EVEX_Vpandnq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpandnV_VX_k1_HX_WX_2_Data))]
		void Test16_VpandnV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test16_VpandnV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D8B DF D3", 6, Code.EVEX_Vpandnd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F14D0B DF D3", 6, Code.EVEX_Vpandnd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F14DAB DF D3", 6, Code.EVEX_Vpandnd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F14D2B DF D3", 6, Code.EVEX_Vpandnd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F14DCB DF D3", 6, Code.EVEX_Vpandnd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F14D4B DF D3", 6, Code.EVEX_Vpandnd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F1CD8B DF D3", 6, Code.EVEX_Vpandnq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B DF D3", 6, Code.EVEX_Vpandnq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB DF D3", 6, Code.EVEX_Vpandnq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B DF D3", 6, Code.EVEX_Vpandnq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB DF D3", 6, Code.EVEX_Vpandnq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B DF D3", 6, Code.EVEX_Vpandnq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpandnV_VX_k1_HX_WX_1_Data))]
		void Test32_VpandnV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpandnV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B DF 50 01", 7, Code.EVEX_Vpandnd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F14D9D DF 50 01", 7, Code.EVEX_Vpandnd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F14D08 DF 50 01", 7, Code.EVEX_Vpandnd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F14D2B DF 50 01", 7, Code.EVEX_Vpandnd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F14DBD DF 50 01", 7, Code.EVEX_Vpandnd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F14D28 DF 50 01", 7, Code.EVEX_Vpandnd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F14D4B DF 50 01", 7, Code.EVEX_Vpandnd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F14DDD DF 50 01", 7, Code.EVEX_Vpandnd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F14D48 DF 50 01", 7, Code.EVEX_Vpandnd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F1CD0B DF 50 01", 7, Code.EVEX_Vpandnq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD9D DF 50 01", 7, Code.EVEX_Vpandnq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F1CD08 DF 50 01", 7, Code.EVEX_Vpandnq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD2B DF 50 01", 7, Code.EVEX_Vpandnq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F1CDBD DF 50 01", 7, Code.EVEX_Vpandnq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F1CD28 DF 50 01", 7, Code.EVEX_Vpandnq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F1CD4B DF 50 01", 7, Code.EVEX_Vpandnq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F1CDDD DF 50 01", 7, Code.EVEX_Vpandnq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F1CD48 DF 50 01", 7, Code.EVEX_Vpandnq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpandnV_VX_k1_HX_WX_2_Data))]
		void Test32_VpandnV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test32_VpandnV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D8B DF D3", 6, Code.EVEX_Vpandnd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F14D0B DF D3", 6, Code.EVEX_Vpandnd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F14DAB DF D3", 6, Code.EVEX_Vpandnd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F14D2B DF D3", 6, Code.EVEX_Vpandnd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F14DCB DF D3", 6, Code.EVEX_Vpandnd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F14D4B DF D3", 6, Code.EVEX_Vpandnd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F1CD8B DF D3", 6, Code.EVEX_Vpandnq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B DF D3", 6, Code.EVEX_Vpandnq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB DF D3", 6, Code.EVEX_Vpandnq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B DF D3", 6, Code.EVEX_Vpandnq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB DF D3", 6, Code.EVEX_Vpandnq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B DF D3", 6, Code.EVEX_Vpandnq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpandnV_VX_k1_HX_WX_1_Data))]
		void Test64_VpandnV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpandnV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B DF 50 01", 7, Code.EVEX_Vpandnd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F14D9D DF 50 01", 7, Code.EVEX_Vpandnd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true };
				yield return new object[] { "62 F14D08 DF 50 01", 7, Code.EVEX_Vpandnd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F14D2B DF 50 01", 7, Code.EVEX_Vpandnd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F14DBD DF 50 01", 7, Code.EVEX_Vpandnd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true };
				yield return new object[] { "62 F14D28 DF 50 01", 7, Code.EVEX_Vpandnd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F14D4B DF 50 01", 7, Code.EVEX_Vpandnd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F14DDD DF 50 01", 7, Code.EVEX_Vpandnd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true };
				yield return new object[] { "62 F14D48 DF 50 01", 7, Code.EVEX_Vpandnd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };

				yield return new object[] { "62 F1CD0B DF 50 01", 7, Code.EVEX_Vpandnq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD9D DF 50 01", 7, Code.EVEX_Vpandnq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true };
				yield return new object[] { "62 F1CD08 DF 50 01", 7, Code.EVEX_Vpandnq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD2B DF 50 01", 7, Code.EVEX_Vpandnq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false };
				yield return new object[] { "62 F1CDBD DF 50 01", 7, Code.EVEX_Vpandnq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true };
				yield return new object[] { "62 F1CD28 DF 50 01", 7, Code.EVEX_Vpandnq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F1CD4B DF 50 01", 7, Code.EVEX_Vpandnq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false };
				yield return new object[] { "62 F1CDDD DF 50 01", 7, Code.EVEX_Vpandnq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true };
				yield return new object[] { "62 F1CD48 DF 50 01", 7, Code.EVEX_Vpandnq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpandnV_VX_k1_HX_WX_2_Data))]
		void Test64_VpandnV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
		public static IEnumerable<object[]> Test64_VpandnV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D8B DF D3", 6, Code.EVEX_Vpandnd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E10D0B DF D3", 6, Code.EVEX_Vpandnd_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 114D03 DF D3", 6, Code.EVEX_Vpandnd_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B14D0B DF D3", 6, Code.EVEX_Vpandnd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F14D0B DF D3", 6, Code.EVEX_Vpandnd_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F14DAB DF D3", 6, Code.EVEX_Vpandnd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E10D2B DF D3", 6, Code.EVEX_Vpandnd_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 114D23 DF D3", 6, Code.EVEX_Vpandnd_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B14D2B DF D3", 6, Code.EVEX_Vpandnd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F14D2B DF D3", 6, Code.EVEX_Vpandnd_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F14DCB DF D3", 6, Code.EVEX_Vpandnd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E10D4B DF D3", 6, Code.EVEX_Vpandnd_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 114D43 DF D3", 6, Code.EVEX_Vpandnd_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B14D4B DF D3", 6, Code.EVEX_Vpandnd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F14D4B DF D3", 6, Code.EVEX_Vpandnd_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };

				yield return new object[] { "62 F1CD8B DF D3", 6, Code.EVEX_Vpandnq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E18D0B DF D3", 6, Code.EVEX_Vpandnq_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 11CD03 DF D3", 6, Code.EVEX_Vpandnq_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B1CD0B DF D3", 6, Code.EVEX_Vpandnq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F1CD0B DF D3", 6, Code.EVEX_Vpandnq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB DF D3", 6, Code.EVEX_Vpandnq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E18D2B DF D3", 6, Code.EVEX_Vpandnq_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 11CD23 DF D3", 6, Code.EVEX_Vpandnq_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B1CD2B DF D3", 6, Code.EVEX_Vpandnq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F1CD2B DF D3", 6, Code.EVEX_Vpandnq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB DF D3", 6, Code.EVEX_Vpandnq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E18D4B DF D3", 6, Code.EVEX_Vpandnq_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 11CD43 DF D3", 6, Code.EVEX_Vpandnq_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B1CD4B DF D3", 6, Code.EVEX_Vpandnq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F1CD4B DF D3", 6, Code.EVEX_Vpandnq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}
	}
}
