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
	public sealed class DecoderTest_2_0F68_0F6F : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_PunpckhbwV_VX_WX_1_Data))]
		void Test16_PunpckhbwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PunpckhbwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F68 08", 3, Code.Punpckhbw_P_Q, Register.MM1, MemorySize.Packed64_Int8 };

				yield return new object[] { "66 0F68 08", 4, Code.Punpckhbw_VX_WX, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PunpckhbwV_VX_WX_2_Data))]
		void Test16_PunpckhbwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PunpckhbwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F68 CD", 3, Code.Punpckhbw_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F68 CD", 4, Code.Punpckhbw_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PunpckhbwV_VX_WX_1_Data))]
		void Test32_PunpckhbwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PunpckhbwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F68 08", 3, Code.Punpckhbw_P_Q, Register.MM1, MemorySize.Packed64_Int8 };

				yield return new object[] { "66 0F68 08", 4, Code.Punpckhbw_VX_WX, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PunpckhbwV_VX_WX_2_Data))]
		void Test32_PunpckhbwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PunpckhbwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F68 CD", 3, Code.Punpckhbw_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F68 CD", 4, Code.Punpckhbw_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PunpckhbwV_VX_WX_1_Data))]
		void Test64_PunpckhbwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PunpckhbwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F68 08", 3, Code.Punpckhbw_P_Q, Register.MM1, MemorySize.Packed64_Int8 };

				yield return new object[] { "66 0F68 08", 4, Code.Punpckhbw_VX_WX, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PunpckhbwV_VX_WX_2_Data))]
		void Test64_PunpckhbwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PunpckhbwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F68 CD", 3, Code.Punpckhbw_P_Q, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F68 CD", 4, Code.Punpckhbw_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F68 CD", 4, Code.Punpckhbw_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F68 CD", 5, Code.Punpckhbw_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F68 CD", 5, Code.Punpckhbw_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F68 CD", 5, Code.Punpckhbw_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpunpckhbwV_VX_HX_WX_1_Data))]
		void Test16_VpunpckhbwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpunpckhbwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 68 10", 4, Code.VEX_Vpunpckhbw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C5CD 68 10", 4, Code.VEX_Vpunpckhbw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E1C9 68 10", 5, Code.VEX_Vpunpckhbw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E1CD 68 10", 5, Code.VEX_Vpunpckhbw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpunpckhbwV_VX_HX_WX_2_Data))]
		void Test16_VpunpckhbwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpunpckhbwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 68 D3", 4, Code.VEX_Vpunpckhbw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 68 D3", 4, Code.VEX_Vpunpckhbw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpunpckhbwV_VX_HX_WX_1_Data))]
		void Test32_VpunpckhbwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpunpckhbwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 68 10", 4, Code.VEX_Vpunpckhbw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C5CD 68 10", 4, Code.VEX_Vpunpckhbw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E1C9 68 10", 5, Code.VEX_Vpunpckhbw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E1CD 68 10", 5, Code.VEX_Vpunpckhbw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpunpckhbwV_VX_HX_WX_2_Data))]
		void Test32_VpunpckhbwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpunpckhbwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 68 D3", 4, Code.VEX_Vpunpckhbw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 68 D3", 4, Code.VEX_Vpunpckhbw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpunpckhbwV_VX_HX_WX_1_Data))]
		void Test64_VpunpckhbwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpunpckhbwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 68 10", 4, Code.VEX_Vpunpckhbw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C5CD 68 10", 4, Code.VEX_Vpunpckhbw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E1C9 68 10", 5, Code.VEX_Vpunpckhbw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E1CD 68 10", 5, Code.VEX_Vpunpckhbw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpunpckhbwV_VX_HX_WX_2_Data))]
		void Test64_VpunpckhbwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpunpckhbwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 68 D3", 4, Code.VEX_Vpunpckhbw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 68 D3", 4, Code.VEX_Vpunpckhbw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 68 D3", 4, Code.VEX_Vpunpckhbw_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 68 D3", 4, Code.VEX_Vpunpckhbw_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 68 D3", 4, Code.VEX_Vpunpckhbw_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 68 D3", 4, Code.VEX_Vpunpckhbw_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 68 D3", 5, Code.VEX_Vpunpckhbw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 68 D3", 5, Code.VEX_Vpunpckhbw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpunpckhbwV_VX_k1_HX_WX_1_Data))]
		void Test16_VpunpckhbwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpunpckhbwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 68 50 01", 7, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F14D8D 68 50 01", 7, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F14D08 68 50 01", 7, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F1CD0B 68 50 01", 7, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F14D2B 68 50 01", 7, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F14DAD 68 50 01", 7, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F14D28 68 50 01", 7, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F1CD2B 68 50 01", 7, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F14D4B 68 50 01", 7, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F14DCD 68 50 01", 7, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F14D48 68 50 01", 7, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F1CD4B 68 50 01", 7, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpunpckhbwV_VX_k1_HX_WX_2_Data))]
		void Test16_VpunpckhbwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpunpckhbwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B 68 D3", 6, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B 68 D3", 6, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B 68 D3", 6, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B 68 D3", 6, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB 68 D3", 6, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B 68 D3", 6, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B 68 D3", 6, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB 68 D3", 6, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B 68 D3", 6, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpunpckhbwV_VX_k1_HX_WX_1_Data))]
		void Test32_VpunpckhbwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpunpckhbwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 68 50 01", 7, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F14D8D 68 50 01", 7, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F14D08 68 50 01", 7, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F1CD0B 68 50 01", 7, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F14D2B 68 50 01", 7, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F14DAD 68 50 01", 7, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F14D28 68 50 01", 7, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F1CD2B 68 50 01", 7, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F14D4B 68 50 01", 7, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F14DCD 68 50 01", 7, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F14D48 68 50 01", 7, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F1CD4B 68 50 01", 7, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpunpckhbwV_VX_k1_HX_WX_2_Data))]
		void Test32_VpunpckhbwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpunpckhbwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B 68 D3", 6, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B 68 D3", 6, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B 68 D3", 6, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B 68 D3", 6, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB 68 D3", 6, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B 68 D3", 6, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B 68 D3", 6, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB 68 D3", 6, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B 68 D3", 6, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpunpckhbwV_VX_k1_HX_WX_1_Data))]
		void Test64_VpunpckhbwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpunpckhbwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 68 50 01", 7, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F14D8D 68 50 01", 7, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F14D08 68 50 01", 7, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F1CD0B 68 50 01", 7, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F14D2B 68 50 01", 7, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F14DAD 68 50 01", 7, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F14D28 68 50 01", 7, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F1CD2B 68 50 01", 7, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F14D4B 68 50 01", 7, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F14DCD 68 50 01", 7, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F14D48 68 50 01", 7, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F1CD4B 68 50 01", 7, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpunpckhbwV_VX_k1_HX_WX_2_Data))]
		void Test64_VpunpckhbwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpunpckhbwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B 68 D3", 6, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B 68 D3", 6, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 68 D3", 6, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B 68 D3", 6, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B 68 D3", 6, Code.EVEX_Vpunpckhbw_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B 68 D3", 6, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB 68 D3", 6, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 68 D3", 6, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B 68 D3", 6, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B 68 D3", 6, Code.EVEX_Vpunpckhbw_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B 68 D3", 6, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB 68 D3", 6, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 68 D3", 6, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B 68 D3", 6, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B 68 D3", 6, Code.EVEX_Vpunpckhbw_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PunpckhwdV_VX_WX_1_Data))]
		void Test16_PunpckhwdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PunpckhwdV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F69 08", 3, Code.Punpckhwd_P_Q, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0F69 08", 4, Code.Punpckhwd_VX_WX, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PunpckhwdV_VX_WX_2_Data))]
		void Test16_PunpckhwdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PunpckhwdV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F69 CD", 3, Code.Punpckhwd_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F69 CD", 4, Code.Punpckhwd_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PunpckhwdV_VX_WX_1_Data))]
		void Test32_PunpckhwdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PunpckhwdV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F69 08", 3, Code.Punpckhwd_P_Q, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0F69 08", 4, Code.Punpckhwd_VX_WX, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PunpckhwdV_VX_WX_2_Data))]
		void Test32_PunpckhwdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PunpckhwdV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F69 CD", 3, Code.Punpckhwd_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F69 CD", 4, Code.Punpckhwd_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PunpckhwdV_VX_WX_1_Data))]
		void Test64_PunpckhwdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PunpckhwdV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F69 08", 3, Code.Punpckhwd_P_Q, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0F69 08", 4, Code.Punpckhwd_VX_WX, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PunpckhwdV_VX_WX_2_Data))]
		void Test64_PunpckhwdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PunpckhwdV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F69 CD", 3, Code.Punpckhwd_P_Q, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F69 CD", 4, Code.Punpckhwd_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F69 CD", 4, Code.Punpckhwd_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F69 CD", 5, Code.Punpckhwd_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F69 CD", 5, Code.Punpckhwd_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F69 CD", 5, Code.Punpckhwd_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpunpckhwdV_VX_HX_WX_1_Data))]
		void Test16_VpunpckhwdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpunpckhwdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 69 10", 4, Code.VEX_Vpunpckhwd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C5CD 69 10", 4, Code.VEX_Vpunpckhwd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E1C9 69 10", 5, Code.VEX_Vpunpckhwd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E1CD 69 10", 5, Code.VEX_Vpunpckhwd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpunpckhwdV_VX_HX_WX_2_Data))]
		void Test16_VpunpckhwdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpunpckhwdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 69 D3", 4, Code.VEX_Vpunpckhwd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 69 D3", 4, Code.VEX_Vpunpckhwd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpunpckhwdV_VX_HX_WX_1_Data))]
		void Test32_VpunpckhwdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpunpckhwdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 69 10", 4, Code.VEX_Vpunpckhwd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C5CD 69 10", 4, Code.VEX_Vpunpckhwd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E1C9 69 10", 5, Code.VEX_Vpunpckhwd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E1CD 69 10", 5, Code.VEX_Vpunpckhwd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpunpckhwdV_VX_HX_WX_2_Data))]
		void Test32_VpunpckhwdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpunpckhwdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 69 D3", 4, Code.VEX_Vpunpckhwd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 69 D3", 4, Code.VEX_Vpunpckhwd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpunpckhwdV_VX_HX_WX_1_Data))]
		void Test64_VpunpckhwdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpunpckhwdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 69 10", 4, Code.VEX_Vpunpckhwd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C5CD 69 10", 4, Code.VEX_Vpunpckhwd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E1C9 69 10", 5, Code.VEX_Vpunpckhwd_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E1CD 69 10", 5, Code.VEX_Vpunpckhwd_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpunpckhwdV_VX_HX_WX_2_Data))]
		void Test64_VpunpckhwdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpunpckhwdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 69 D3", 4, Code.VEX_Vpunpckhwd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 69 D3", 4, Code.VEX_Vpunpckhwd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 69 D3", 4, Code.VEX_Vpunpckhwd_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 69 D3", 4, Code.VEX_Vpunpckhwd_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 69 D3", 4, Code.VEX_Vpunpckhwd_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 69 D3", 4, Code.VEX_Vpunpckhwd_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 69 D3", 5, Code.VEX_Vpunpckhwd_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 69 D3", 5, Code.VEX_Vpunpckhwd_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpunpckhwdV_VX_k1_HX_WX_1_Data))]
		void Test16_VpunpckhwdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpunpckhwdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 69 50 01", 7, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D8D 69 50 01", 7, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F14D08 69 50 01", 7, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1CD0B 69 50 01", 7, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F14D2B 69 50 01", 7, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14DAD 69 50 01", 7, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F14D28 69 50 01", 7, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1CD2B 69 50 01", 7, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F14D4B 69 50 01", 7, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14DCD 69 50 01", 7, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F14D48 69 50 01", 7, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1CD4B 69 50 01", 7, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpunpckhwdV_VX_k1_HX_WX_2_Data))]
		void Test16_VpunpckhwdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpunpckhwdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B 69 D3", 6, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B 69 D3", 6, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B 69 D3", 6, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B 69 D3", 6, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB 69 D3", 6, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B 69 D3", 6, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B 69 D3", 6, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB 69 D3", 6, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B 69 D3", 6, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpunpckhwdV_VX_k1_HX_WX_1_Data))]
		void Test32_VpunpckhwdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpunpckhwdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 69 50 01", 7, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D8D 69 50 01", 7, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F14D08 69 50 01", 7, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1CD0B 69 50 01", 7, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F14D2B 69 50 01", 7, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14DAD 69 50 01", 7, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F14D28 69 50 01", 7, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1CD2B 69 50 01", 7, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F14D4B 69 50 01", 7, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14DCD 69 50 01", 7, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F14D48 69 50 01", 7, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1CD4B 69 50 01", 7, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpunpckhwdV_VX_k1_HX_WX_2_Data))]
		void Test32_VpunpckhwdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpunpckhwdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B 69 D3", 6, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B 69 D3", 6, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B 69 D3", 6, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B 69 D3", 6, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB 69 D3", 6, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B 69 D3", 6, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B 69 D3", 6, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB 69 D3", 6, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B 69 D3", 6, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpunpckhwdV_VX_k1_HX_WX_1_Data))]
		void Test64_VpunpckhwdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpunpckhwdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 69 50 01", 7, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D8D 69 50 01", 7, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F14D08 69 50 01", 7, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1CD0B 69 50 01", 7, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F14D2B 69 50 01", 7, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14DAD 69 50 01", 7, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F14D28 69 50 01", 7, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1CD2B 69 50 01", 7, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F14D4B 69 50 01", 7, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14DCD 69 50 01", 7, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F14D48 69 50 01", 7, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1CD4B 69 50 01", 7, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpunpckhwdV_VX_k1_HX_WX_2_Data))]
		void Test64_VpunpckhwdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpunpckhwdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B 69 D3", 6, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B 69 D3", 6, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 69 D3", 6, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B 69 D3", 6, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B 69 D3", 6, Code.EVEX_Vpunpckhwd_VX_k1z_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B 69 D3", 6, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB 69 D3", 6, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 69 D3", 6, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B 69 D3", 6, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B 69 D3", 6, Code.EVEX_Vpunpckhwd_VY_k1z_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B 69 D3", 6, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB 69 D3", 6, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 69 D3", 6, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B 69 D3", 6, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B 69 D3", 6, Code.EVEX_Vpunpckhwd_VZ_k1z_HZ_WZ, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PunpckhdqV_VX_WX_1_Data))]
		void Test16_PunpckhdqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PunpckhdqV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F6A 08", 3, Code.Punpckhdq_P_Q, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0F6A 08", 4, Code.Punpckhdq_VX_WX, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PunpckhdqV_VX_WX_2_Data))]
		void Test16_PunpckhdqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PunpckhdqV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F6A CD", 3, Code.Punpckhdq_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F6A CD", 4, Code.Punpckhdq_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PunpckhdqV_VX_WX_1_Data))]
		void Test32_PunpckhdqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PunpckhdqV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F6A 08", 3, Code.Punpckhdq_P_Q, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0F6A 08", 4, Code.Punpckhdq_VX_WX, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PunpckhdqV_VX_WX_2_Data))]
		void Test32_PunpckhdqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PunpckhdqV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F6A CD", 3, Code.Punpckhdq_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F6A CD", 4, Code.Punpckhdq_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PunpckhdqV_VX_WX_1_Data))]
		void Test64_PunpckhdqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PunpckhdqV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F6A 08", 3, Code.Punpckhdq_P_Q, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0F6A 08", 4, Code.Punpckhdq_VX_WX, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PunpckhdqV_VX_WX_2_Data))]
		void Test64_PunpckhdqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PunpckhdqV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F6A CD", 3, Code.Punpckhdq_P_Q, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F6A CD", 4, Code.Punpckhdq_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F6A CD", 4, Code.Punpckhdq_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F6A CD", 5, Code.Punpckhdq_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F6A CD", 5, Code.Punpckhdq_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F6A CD", 5, Code.Punpckhdq_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpunpckhdqV_VX_HX_WX_1_Data))]
		void Test16_VpunpckhdqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpunpckhdqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 6A 10", 4, Code.VEX_Vpunpckhdq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C5CD 6A 10", 4, Code.VEX_Vpunpckhdq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1C9 6A 10", 5, Code.VEX_Vpunpckhdq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1CD 6A 10", 5, Code.VEX_Vpunpckhdq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpunpckhdqV_VX_HX_WX_2_Data))]
		void Test16_VpunpckhdqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpunpckhdqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 6A D3", 4, Code.VEX_Vpunpckhdq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 6A D3", 4, Code.VEX_Vpunpckhdq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpunpckhdqV_VX_HX_WX_1_Data))]
		void Test32_VpunpckhdqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpunpckhdqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 6A 10", 4, Code.VEX_Vpunpckhdq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C5CD 6A 10", 4, Code.VEX_Vpunpckhdq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1C9 6A 10", 5, Code.VEX_Vpunpckhdq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1CD 6A 10", 5, Code.VEX_Vpunpckhdq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpunpckhdqV_VX_HX_WX_2_Data))]
		void Test32_VpunpckhdqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpunpckhdqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 6A D3", 4, Code.VEX_Vpunpckhdq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 6A D3", 4, Code.VEX_Vpunpckhdq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpunpckhdqV_VX_HX_WX_1_Data))]
		void Test64_VpunpckhdqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpunpckhdqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 6A 10", 4, Code.VEX_Vpunpckhdq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C5CD 6A 10", 4, Code.VEX_Vpunpckhdq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1C9 6A 10", 5, Code.VEX_Vpunpckhdq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1CD 6A 10", 5, Code.VEX_Vpunpckhdq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpunpckhdqV_VX_HX_WX_2_Data))]
		void Test64_VpunpckhdqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpunpckhdqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 6A D3", 4, Code.VEX_Vpunpckhdq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 6A D3", 4, Code.VEX_Vpunpckhdq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 6A D3", 4, Code.VEX_Vpunpckhdq_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 6A D3", 4, Code.VEX_Vpunpckhdq_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 6A D3", 4, Code.VEX_Vpunpckhdq_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 6A D3", 4, Code.VEX_Vpunpckhdq_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 6A D3", 5, Code.VEX_Vpunpckhdq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 6A D3", 5, Code.VEX_Vpunpckhdq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpunpckhdqV_VX_k1_HX_WX_1_Data))]
		void Test16_VpunpckhdqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpunpckhdqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F14D9D 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F14D08 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F14D2B 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F14DBD 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F14D28 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F14D4B 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F14DDD 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F14D48 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpunpckhdqV_VX_k1_HX_WX_2_Data))]
		void Test16_VpunpckhdqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpunpckhdqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D8B 6A D3", 6, Code.EVEX_Vpunpckhdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F14D0B 6A D3", 6, Code.EVEX_Vpunpckhdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F14DAB 6A D3", 6, Code.EVEX_Vpunpckhdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F14D2B 6A D3", 6, Code.EVEX_Vpunpckhdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F14DCB 6A D3", 6, Code.EVEX_Vpunpckhdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F14D4B 6A D3", 6, Code.EVEX_Vpunpckhdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpunpckhdqV_VX_k1_HX_WX_1_Data))]
		void Test32_VpunpckhdqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpunpckhdqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F14D9D 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F14D08 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F14D2B 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F14DBD 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F14D28 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F14D4B 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F14DDD 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F14D48 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpunpckhdqV_VX_k1_HX_WX_2_Data))]
		void Test32_VpunpckhdqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpunpckhdqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D8B 6A D3", 6, Code.EVEX_Vpunpckhdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F14D0B 6A D3", 6, Code.EVEX_Vpunpckhdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F14DAB 6A D3", 6, Code.EVEX_Vpunpckhdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F14D2B 6A D3", 6, Code.EVEX_Vpunpckhdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F14DCB 6A D3", 6, Code.EVEX_Vpunpckhdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F14D4B 6A D3", 6, Code.EVEX_Vpunpckhdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpunpckhdqV_VX_k1_HX_WX_1_Data))]
		void Test64_VpunpckhdqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpunpckhdqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F14D9D 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F14D08 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F14D2B 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F14DBD 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F14D28 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F14D4B 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F14DDD 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F14D48 6A 50 01", 7, Code.EVEX_Vpunpckhdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpunpckhdqV_VX_k1_HX_WX_2_Data))]
		void Test64_VpunpckhdqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpunpckhdqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D8B 6A D3", 6, Code.EVEX_Vpunpckhdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E10D0B 6A D3", 6, Code.EVEX_Vpunpckhdq_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 114D03 6A D3", 6, Code.EVEX_Vpunpckhdq_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B14D0B 6A D3", 6, Code.EVEX_Vpunpckhdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F14D0B 6A D3", 6, Code.EVEX_Vpunpckhdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F14DAB 6A D3", 6, Code.EVEX_Vpunpckhdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E10D2B 6A D3", 6, Code.EVEX_Vpunpckhdq_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 114D23 6A D3", 6, Code.EVEX_Vpunpckhdq_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B14D2B 6A D3", 6, Code.EVEX_Vpunpckhdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F14D2B 6A D3", 6, Code.EVEX_Vpunpckhdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F14DCB 6A D3", 6, Code.EVEX_Vpunpckhdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E10D4B 6A D3", 6, Code.EVEX_Vpunpckhdq_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 114D43 6A D3", 6, Code.EVEX_Vpunpckhdq_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B14D4B 6A D3", 6, Code.EVEX_Vpunpckhdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F14D4B 6A D3", 6, Code.EVEX_Vpunpckhdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PackssdwV_VX_WX_1_Data))]
		void Test16_PackssdwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PackssdwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F6B 08", 3, Code.Packssdw_P_Q, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0F6B 08", 4, Code.Packssdw_VX_WX, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PackssdwV_VX_WX_2_Data))]
		void Test16_PackssdwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PackssdwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F6B CD", 3, Code.Packssdw_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F6B CD", 4, Code.Packssdw_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PackssdwV_VX_WX_1_Data))]
		void Test32_PackssdwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PackssdwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F6B 08", 3, Code.Packssdw_P_Q, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0F6B 08", 4, Code.Packssdw_VX_WX, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PackssdwV_VX_WX_2_Data))]
		void Test32_PackssdwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PackssdwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F6B CD", 3, Code.Packssdw_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F6B CD", 4, Code.Packssdw_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PackssdwV_VX_WX_1_Data))]
		void Test64_PackssdwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PackssdwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F6B 08", 3, Code.Packssdw_P_Q, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0F6B 08", 4, Code.Packssdw_VX_WX, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PackssdwV_VX_WX_2_Data))]
		void Test64_PackssdwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PackssdwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F6B CD", 3, Code.Packssdw_P_Q, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F6B CD", 4, Code.Packssdw_P_Q, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F6B CD", 4, Code.Packssdw_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F6B CD", 5, Code.Packssdw_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F6B CD", 5, Code.Packssdw_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F6B CD", 5, Code.Packssdw_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpackssdwV_VX_HX_WX_1_Data))]
		void Test16_VpackssdwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpackssdwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 6B 10", 4, Code.VEX_Vpackssdw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C5CD 6B 10", 4, Code.VEX_Vpackssdw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1C9 6B 10", 5, Code.VEX_Vpackssdw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1CD 6B 10", 5, Code.VEX_Vpackssdw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpackssdwV_VX_HX_WX_2_Data))]
		void Test16_VpackssdwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpackssdwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 6B D3", 4, Code.VEX_Vpackssdw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 6B D3", 4, Code.VEX_Vpackssdw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpackssdwV_VX_HX_WX_1_Data))]
		void Test32_VpackssdwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpackssdwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 6B 10", 4, Code.VEX_Vpackssdw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C5CD 6B 10", 4, Code.VEX_Vpackssdw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1C9 6B 10", 5, Code.VEX_Vpackssdw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1CD 6B 10", 5, Code.VEX_Vpackssdw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpackssdwV_VX_HX_WX_2_Data))]
		void Test32_VpackssdwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpackssdwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 6B D3", 4, Code.VEX_Vpackssdw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 6B D3", 4, Code.VEX_Vpackssdw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpackssdwV_VX_HX_WX_1_Data))]
		void Test64_VpackssdwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpackssdwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 6B 10", 4, Code.VEX_Vpackssdw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C5CD 6B 10", 4, Code.VEX_Vpackssdw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1C9 6B 10", 5, Code.VEX_Vpackssdw_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1CD 6B 10", 5, Code.VEX_Vpackssdw_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpackssdwV_VX_HX_WX_2_Data))]
		void Test64_VpackssdwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpackssdwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 6B D3", 4, Code.VEX_Vpackssdw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 6B D3", 4, Code.VEX_Vpackssdw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 6B D3", 4, Code.VEX_Vpackssdw_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 6B D3", 4, Code.VEX_Vpackssdw_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 6B D3", 4, Code.VEX_Vpackssdw_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 6B D3", 4, Code.VEX_Vpackssdw_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 6B D3", 5, Code.VEX_Vpackssdw_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 6B D3", 5, Code.VEX_Vpackssdw_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpackssdwV_VX_k1_HX_WX_1_Data))]
		void Test16_VpackssdwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpackssdwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 6B 50 01", 7, Code.EVEX_Vpackssdw_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F14D9D 6B 50 01", 7, Code.EVEX_Vpackssdw_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F14D08 6B 50 01", 7, Code.EVEX_Vpackssdw_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F14D2B 6B 50 01", 7, Code.EVEX_Vpackssdw_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F14DBD 6B 50 01", 7, Code.EVEX_Vpackssdw_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F14D28 6B 50 01", 7, Code.EVEX_Vpackssdw_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F14D4B 6B 50 01", 7, Code.EVEX_Vpackssdw_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F14DDD 6B 50 01", 7, Code.EVEX_Vpackssdw_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F14D48 6B 50 01", 7, Code.EVEX_Vpackssdw_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpackssdwV_VX_k1_HX_WX_2_Data))]
		void Test16_VpackssdwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpackssdwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D8B 6B D3", 6, Code.EVEX_Vpackssdw_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F14D0B 6B D3", 6, Code.EVEX_Vpackssdw_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F14DAB 6B D3", 6, Code.EVEX_Vpackssdw_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F14D2B 6B D3", 6, Code.EVEX_Vpackssdw_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F14DCB 6B D3", 6, Code.EVEX_Vpackssdw_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F14D4B 6B D3", 6, Code.EVEX_Vpackssdw_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpackssdwV_VX_k1_HX_WX_1_Data))]
		void Test32_VpackssdwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpackssdwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 6B 50 01", 7, Code.EVEX_Vpackssdw_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F14D9D 6B 50 01", 7, Code.EVEX_Vpackssdw_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F14D08 6B 50 01", 7, Code.EVEX_Vpackssdw_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F14D2B 6B 50 01", 7, Code.EVEX_Vpackssdw_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F14DBD 6B 50 01", 7, Code.EVEX_Vpackssdw_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F14D28 6B 50 01", 7, Code.EVEX_Vpackssdw_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F14D4B 6B 50 01", 7, Code.EVEX_Vpackssdw_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F14DDD 6B 50 01", 7, Code.EVEX_Vpackssdw_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F14D48 6B 50 01", 7, Code.EVEX_Vpackssdw_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpackssdwV_VX_k1_HX_WX_2_Data))]
		void Test32_VpackssdwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpackssdwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D8B 6B D3", 6, Code.EVEX_Vpackssdw_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F14D0B 6B D3", 6, Code.EVEX_Vpackssdw_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F14DAB 6B D3", 6, Code.EVEX_Vpackssdw_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F14D2B 6B D3", 6, Code.EVEX_Vpackssdw_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F14DCB 6B D3", 6, Code.EVEX_Vpackssdw_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F14D4B 6B D3", 6, Code.EVEX_Vpackssdw_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpackssdwV_VX_k1_HX_WX_1_Data))]
		void Test64_VpackssdwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpackssdwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B 6B 50 01", 7, Code.EVEX_Vpackssdw_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F14D9D 6B 50 01", 7, Code.EVEX_Vpackssdw_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, true };
				yield return new object[] { "62 F14D08 6B 50 01", 7, Code.EVEX_Vpackssdw_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F14D2B 6B 50 01", 7, Code.EVEX_Vpackssdw_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F14DBD 6B 50 01", 7, Code.EVEX_Vpackssdw_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true };
				yield return new object[] { "62 F14D28 6B 50 01", 7, Code.EVEX_Vpackssdw_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F14D4B 6B 50 01", 7, Code.EVEX_Vpackssdw_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F14DDD 6B 50 01", 7, Code.EVEX_Vpackssdw_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true };
				yield return new object[] { "62 F14D48 6B 50 01", 7, Code.EVEX_Vpackssdw_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpackssdwV_VX_k1_HX_WX_2_Data))]
		void Test64_VpackssdwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpackssdwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D8B 6B D3", 6, Code.EVEX_Vpackssdw_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E10D0B 6B D3", 6, Code.EVEX_Vpackssdw_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 114D03 6B D3", 6, Code.EVEX_Vpackssdw_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B14D0B 6B D3", 6, Code.EVEX_Vpackssdw_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F14D0B 6B D3", 6, Code.EVEX_Vpackssdw_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F14DAB 6B D3", 6, Code.EVEX_Vpackssdw_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E10D2B 6B D3", 6, Code.EVEX_Vpackssdw_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 114D23 6B D3", 6, Code.EVEX_Vpackssdw_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B14D2B 6B D3", 6, Code.EVEX_Vpackssdw_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F14D2B 6B D3", 6, Code.EVEX_Vpackssdw_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F14DCB 6B D3", 6, Code.EVEX_Vpackssdw_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E10D4B 6B D3", 6, Code.EVEX_Vpackssdw_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 114D43 6B D3", 6, Code.EVEX_Vpackssdw_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B14D4B 6B D3", 6, Code.EVEX_Vpackssdw_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F14D4B 6B D3", 6, Code.EVEX_Vpackssdw_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PunpcklqdqV_VX_WX_1_Data))]
		void Test16_PunpcklqdqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PunpcklqdqV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F6C 08", 4, Code.Punpcklqdq_VX_WX, Register.XMM1, MemorySize.Packed128_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PunpcklqdqV_VX_WX_2_Data))]
		void Test16_PunpcklqdqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PunpcklqdqV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F6C CD", 4, Code.Punpcklqdq_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PunpcklqdqV_VX_WX_1_Data))]
		void Test32_PunpcklqdqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PunpcklqdqV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F6C 08", 4, Code.Punpcklqdq_VX_WX, Register.XMM1, MemorySize.Packed128_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PunpcklqdqV_VX_WX_2_Data))]
		void Test32_PunpcklqdqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PunpcklqdqV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F6C CD", 4, Code.Punpcklqdq_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PunpcklqdqV_VX_WX_1_Data))]
		void Test64_PunpcklqdqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PunpcklqdqV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F6C 08", 4, Code.Punpcklqdq_VX_WX, Register.XMM1, MemorySize.Packed128_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PunpcklqdqV_VX_WX_2_Data))]
		void Test64_PunpcklqdqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PunpcklqdqV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F6C CD", 4, Code.Punpcklqdq_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F6C CD", 5, Code.Punpcklqdq_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F6C CD", 5, Code.Punpcklqdq_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F6C CD", 5, Code.Punpcklqdq_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpunpcklqdqV_VX_HX_WX_1_Data))]
		void Test16_VpunpcklqdqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpunpcklqdqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 6C 10", 4, Code.VEX_Vpunpcklqdq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C5CD 6C 10", 4, Code.VEX_Vpunpcklqdq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
				yield return new object[] { "C4E1C9 6C 10", 5, Code.VEX_Vpunpcklqdq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C4E1CD 6C 10", 5, Code.VEX_Vpunpcklqdq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpunpcklqdqV_VX_HX_WX_2_Data))]
		void Test16_VpunpcklqdqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpunpcklqdqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 6C D3", 4, Code.VEX_Vpunpcklqdq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 6C D3", 4, Code.VEX_Vpunpcklqdq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpunpcklqdqV_VX_HX_WX_1_Data))]
		void Test32_VpunpcklqdqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpunpcklqdqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 6C 10", 4, Code.VEX_Vpunpcklqdq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C5CD 6C 10", 4, Code.VEX_Vpunpcklqdq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
				yield return new object[] { "C4E1C9 6C 10", 5, Code.VEX_Vpunpcklqdq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C4E1CD 6C 10", 5, Code.VEX_Vpunpcklqdq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpunpcklqdqV_VX_HX_WX_2_Data))]
		void Test32_VpunpcklqdqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpunpcklqdqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 6C D3", 4, Code.VEX_Vpunpcklqdq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 6C D3", 4, Code.VEX_Vpunpcklqdq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpunpcklqdqV_VX_HX_WX_1_Data))]
		void Test64_VpunpcklqdqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpunpcklqdqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 6C 10", 4, Code.VEX_Vpunpcklqdq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C5CD 6C 10", 4, Code.VEX_Vpunpcklqdq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
				yield return new object[] { "C4E1C9 6C 10", 5, Code.VEX_Vpunpcklqdq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C4E1CD 6C 10", 5, Code.VEX_Vpunpcklqdq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpunpcklqdqV_VX_HX_WX_2_Data))]
		void Test64_VpunpcklqdqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpunpcklqdqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 6C D3", 4, Code.VEX_Vpunpcklqdq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 6C D3", 4, Code.VEX_Vpunpcklqdq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 6C D3", 4, Code.VEX_Vpunpcklqdq_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 6C D3", 4, Code.VEX_Vpunpcklqdq_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 6C D3", 4, Code.VEX_Vpunpcklqdq_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 6C D3", 4, Code.VEX_Vpunpcklqdq_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 6C D3", 5, Code.VEX_Vpunpcklqdq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 6C D3", 5, Code.VEX_Vpunpcklqdq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpunpcklqdqV_VX_k1_HX_WX_1_Data))]
		void Test16_VpunpcklqdqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpunpcklqdqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F1CD9D 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F1CD08 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F1CD2B 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F1CDBD 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F1CD28 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F1CD4B 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F1CDDD 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F1CD48 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpunpcklqdqV_VX_k1_HX_WX_2_Data))]
		void Test16_VpunpcklqdqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpunpcklqdqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F1CD8B 6C D3", 6, Code.EVEX_Vpunpcklqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B 6C D3", 6, Code.EVEX_Vpunpcklqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 6C D3", 6, Code.EVEX_Vpunpcklqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B 6C D3", 6, Code.EVEX_Vpunpcklqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 6C D3", 6, Code.EVEX_Vpunpcklqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B 6C D3", 6, Code.EVEX_Vpunpcklqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpunpcklqdqV_VX_k1_HX_WX_1_Data))]
		void Test32_VpunpcklqdqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpunpcklqdqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F1CD9D 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F1CD08 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F1CD2B 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F1CDBD 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F1CD28 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F1CD4B 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F1CDDD 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F1CD48 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpunpcklqdqV_VX_k1_HX_WX_2_Data))]
		void Test32_VpunpcklqdqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpunpcklqdqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F1CD8B 6C D3", 6, Code.EVEX_Vpunpcklqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B 6C D3", 6, Code.EVEX_Vpunpcklqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 6C D3", 6, Code.EVEX_Vpunpcklqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B 6C D3", 6, Code.EVEX_Vpunpcklqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 6C D3", 6, Code.EVEX_Vpunpcklqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B 6C D3", 6, Code.EVEX_Vpunpcklqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpunpcklqdqV_VX_k1_HX_WX_1_Data))]
		void Test64_VpunpcklqdqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpunpcklqdqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F1CD9D 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F1CD08 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F1CD2B 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F1CDBD 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F1CD28 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F1CD4B 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F1CDDD 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F1CD48 6C 50 01", 7, Code.EVEX_Vpunpcklqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpunpcklqdqV_VX_k1_HX_WX_2_Data))]
		void Test64_VpunpcklqdqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpunpcklqdqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F1CD8B 6C D3", 6, Code.EVEX_Vpunpcklqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E18D0B 6C D3", 6, Code.EVEX_Vpunpcklqdq_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 11CD03 6C D3", 6, Code.EVEX_Vpunpcklqdq_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B1CD0B 6C D3", 6, Code.EVEX_Vpunpcklqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F1CD0B 6C D3", 6, Code.EVEX_Vpunpcklqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 6C D3", 6, Code.EVEX_Vpunpcklqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E18D2B 6C D3", 6, Code.EVEX_Vpunpcklqdq_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 11CD23 6C D3", 6, Code.EVEX_Vpunpcklqdq_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B1CD2B 6C D3", 6, Code.EVEX_Vpunpcklqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F1CD2B 6C D3", 6, Code.EVEX_Vpunpcklqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 6C D3", 6, Code.EVEX_Vpunpcklqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E18D4B 6C D3", 6, Code.EVEX_Vpunpcklqdq_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 11CD43 6C D3", 6, Code.EVEX_Vpunpcklqdq_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B1CD4B 6C D3", 6, Code.EVEX_Vpunpcklqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F1CD4B 6C D3", 6, Code.EVEX_Vpunpcklqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PunpckhqdqV_VX_WX_1_Data))]
		void Test16_PunpckhqdqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PunpckhqdqV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F6D 08", 4, Code.Punpckhqdq_VX_WX, Register.XMM1, MemorySize.Packed128_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PunpckhqdqV_VX_WX_2_Data))]
		void Test16_PunpckhqdqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PunpckhqdqV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F6D CD", 4, Code.Punpckhqdq_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PunpckhqdqV_VX_WX_1_Data))]
		void Test32_PunpckhqdqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PunpckhqdqV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F6D 08", 4, Code.Punpckhqdq_VX_WX, Register.XMM1, MemorySize.Packed128_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PunpckhqdqV_VX_WX_2_Data))]
		void Test32_PunpckhqdqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PunpckhqdqV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F6D CD", 4, Code.Punpckhqdq_VX_WX, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PunpckhqdqV_VX_WX_1_Data))]
		void Test64_PunpckhqdqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PunpckhqdqV_VX_WX_1_Data {
			get {
				yield return new object[] { "66 0F6D 08", 4, Code.Punpckhqdq_VX_WX, Register.XMM1, MemorySize.Packed128_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PunpckhqdqV_VX_WX_2_Data))]
		void Test64_PunpckhqdqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PunpckhqdqV_VX_WX_2_Data {
			get {
				yield return new object[] { "66 0F6D CD", 4, Code.Punpckhqdq_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F6D CD", 5, Code.Punpckhqdq_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F6D CD", 5, Code.Punpckhqdq_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F6D CD", 5, Code.Punpckhqdq_VX_WX, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpunpckhqdqV_VX_HX_WX_1_Data))]
		void Test16_VpunpckhqdqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpunpckhqdqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 6D 10", 4, Code.VEX_Vpunpckhqdq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C5CD 6D 10", 4, Code.VEX_Vpunpckhqdq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
				yield return new object[] { "C4E1C9 6D 10", 5, Code.VEX_Vpunpckhqdq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C4E1CD 6D 10", 5, Code.VEX_Vpunpckhqdq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpunpckhqdqV_VX_HX_WX_2_Data))]
		void Test16_VpunpckhqdqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpunpckhqdqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 6D D3", 4, Code.VEX_Vpunpckhqdq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 6D D3", 4, Code.VEX_Vpunpckhqdq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpunpckhqdqV_VX_HX_WX_1_Data))]
		void Test32_VpunpckhqdqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpunpckhqdqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 6D 10", 4, Code.VEX_Vpunpckhqdq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C5CD 6D 10", 4, Code.VEX_Vpunpckhqdq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
				yield return new object[] { "C4E1C9 6D 10", 5, Code.VEX_Vpunpckhqdq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C4E1CD 6D 10", 5, Code.VEX_Vpunpckhqdq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpunpckhqdqV_VX_HX_WX_2_Data))]
		void Test32_VpunpckhqdqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpunpckhqdqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 6D D3", 4, Code.VEX_Vpunpckhqdq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 6D D3", 4, Code.VEX_Vpunpckhqdq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpunpckhqdqV_VX_HX_WX_1_Data))]
		void Test64_VpunpckhqdqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpunpckhqdqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 6D 10", 4, Code.VEX_Vpunpckhqdq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C5CD 6D 10", 4, Code.VEX_Vpunpckhqdq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
				yield return new object[] { "C4E1C9 6D 10", 5, Code.VEX_Vpunpckhqdq_VX_HX_WX, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int64 };
				yield return new object[] { "C4E1CD 6D 10", 5, Code.VEX_Vpunpckhqdq_VY_HY_WY, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpunpckhqdqV_VX_HX_WX_2_Data))]
		void Test64_VpunpckhqdqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpunpckhqdqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 6D D3", 4, Code.VEX_Vpunpckhqdq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD 6D D3", 4, Code.VEX_Vpunpckhqdq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 6D D3", 4, Code.VEX_Vpunpckhqdq_VX_HX_WX, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D 6D D3", 4, Code.VEX_Vpunpckhqdq_VY_HY_WY, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 6D D3", 4, Code.VEX_Vpunpckhqdq_VX_HX_WX, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D 6D D3", 4, Code.VEX_Vpunpckhqdq_VY_HY_WY, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 6D D3", 5, Code.VEX_Vpunpckhqdq_VX_HX_WX, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D 6D D3", 5, Code.VEX_Vpunpckhqdq_VY_HY_WY, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpunpckhqdqV_VX_k1_HX_WX_1_Data))]
		void Test16_VpunpckhqdqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpunpckhqdqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F1CD9D 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F1CD08 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F1CD2B 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F1CDBD 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F1CD28 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F1CD4B 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F1CDDD 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F1CD48 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpunpckhqdqV_VX_k1_HX_WX_2_Data))]
		void Test16_VpunpckhqdqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpunpckhqdqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F1CD8B 6D D3", 6, Code.EVEX_Vpunpckhqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B 6D D3", 6, Code.EVEX_Vpunpckhqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 6D D3", 6, Code.EVEX_Vpunpckhqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B 6D D3", 6, Code.EVEX_Vpunpckhqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 6D D3", 6, Code.EVEX_Vpunpckhqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B 6D D3", 6, Code.EVEX_Vpunpckhqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpunpckhqdqV_VX_k1_HX_WX_1_Data))]
		void Test32_VpunpckhqdqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpunpckhqdqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F1CD9D 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F1CD08 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F1CD2B 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F1CDBD 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F1CD28 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F1CD4B 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F1CDDD 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F1CD48 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpunpckhqdqV_VX_k1_HX_WX_2_Data))]
		void Test32_VpunpckhqdqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpunpckhqdqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F1CD8B 6D D3", 6, Code.EVEX_Vpunpckhqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B 6D D3", 6, Code.EVEX_Vpunpckhqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 6D D3", 6, Code.EVEX_Vpunpckhqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B 6D D3", 6, Code.EVEX_Vpunpckhqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 6D D3", 6, Code.EVEX_Vpunpckhqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B 6D D3", 6, Code.EVEX_Vpunpckhqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpunpckhqdqV_VX_k1_HX_WX_1_Data))]
		void Test64_VpunpckhqdqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpunpckhqdqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F1CD9D 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, true };
				yield return new object[] { "62 F1CD08 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false };

				yield return new object[] { "62 F1CD2B 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F1CDBD 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true };
				yield return new object[] { "62 F1CD28 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false };

				yield return new object[] { "62 F1CD4B 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F1CDDD 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true };
				yield return new object[] { "62 F1CD48 6D 50 01", 7, Code.EVEX_Vpunpckhqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpunpckhqdqV_VX_k1_HX_WX_2_Data))]
		void Test64_VpunpckhqdqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpunpckhqdqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F1CD8B 6D D3", 6, Code.EVEX_Vpunpckhqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E18D0B 6D D3", 6, Code.EVEX_Vpunpckhqdq_VX_k1z_HX_WX_b, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 11CD03 6D D3", 6, Code.EVEX_Vpunpckhqdq_VX_k1z_HX_WX_b, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B1CD0B 6D D3", 6, Code.EVEX_Vpunpckhqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F1CD0B 6D D3", 6, Code.EVEX_Vpunpckhqdq_VX_k1z_HX_WX_b, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB 6D D3", 6, Code.EVEX_Vpunpckhqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E18D2B 6D D3", 6, Code.EVEX_Vpunpckhqdq_VY_k1z_HY_WY_b, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 11CD23 6D D3", 6, Code.EVEX_Vpunpckhqdq_VY_k1z_HY_WY_b, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B1CD2B 6D D3", 6, Code.EVEX_Vpunpckhqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F1CD2B 6D D3", 6, Code.EVEX_Vpunpckhqdq_VY_k1z_HY_WY_b, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB 6D D3", 6, Code.EVEX_Vpunpckhqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E18D4B 6D D3", 6, Code.EVEX_Vpunpckhqdq_VZ_k1z_HZ_WZ_b, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 11CD43 6D D3", 6, Code.EVEX_Vpunpckhqdq_VZ_k1z_HZ_WZ_b, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B1CD4B 6D D3", 6, Code.EVEX_Vpunpckhqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F1CD4B 6D D3", 6, Code.EVEX_Vpunpckhqdq_VZ_k1z_HZ_WZ_b, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovV_Reg_RegMem_1_Data))]
		void Test16_MovV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_MovV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F6E 08", 3, Code.Movd_P_Ed, Register.MM1, MemorySize.UInt32 };

				yield return new object[] { "66 0F6E 08", 4, Code.Movd_VX_Ed, Register.XMM1, MemorySize.UInt32 };

				yield return new object[] { "C5F9 6E 08", 4, Code.VEX_Vmovd_VX_Ed, Register.XMM1, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovV_Reg_RegMem_2_Data))]
		void Test16_MovV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_MovV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F6E CD", 3, Code.Movd_P_Ed, Register.MM1, Register.EBP };

				yield return new object[] { "66 0F6E CD", 4, Code.Movd_VX_Ed, Register.XMM1, Register.EBP };

				yield return new object[] { "C5F9 6E CD", 4, Code.VEX_Vmovd_VX_Ed, Register.XMM1, Register.EBP };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovV_Reg_RegMem_1_Data))]
		void Test32_MovV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_MovV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F6E 08", 3, Code.Movd_P_Ed, Register.MM1, MemorySize.UInt32 };

				yield return new object[] { "66 0F6E 08", 4, Code.Movd_VX_Ed, Register.XMM1, MemorySize.UInt32 };

				yield return new object[] { "C5F9 6E 08", 4, Code.VEX_Vmovd_VX_Ed, Register.XMM1, MemorySize.UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovV_Reg_RegMem_2_Data))]
		void Test32_MovV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_MovV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F6E CD", 3, Code.Movd_P_Ed, Register.MM1, Register.EBP };

				yield return new object[] { "66 0F6E CD", 4, Code.Movd_VX_Ed, Register.XMM1, Register.EBP };

				yield return new object[] { "C5F9 6E CD", 4, Code.VEX_Vmovd_VX_Ed, Register.XMM1, Register.EBP };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovV_Reg_RegMem_1_Data))]
		void Test64_MovV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_MovV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F6E 08", 3, Code.Movd_P_Ed, Register.MM1, MemorySize.UInt32 };
				yield return new object[] { "48 0F6E 08", 4, Code.Movq_P_Eq, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0F6E 08", 4, Code.Movd_VX_Ed, Register.XMM1, MemorySize.UInt32 };
				yield return new object[] { "66 48 0F6E 08", 5, Code.Movq_VX_Eq, Register.XMM1, MemorySize.UInt64 };

				yield return new object[] { "C5F9 6E 08", 4, Code.VEX_Vmovd_VX_Ed, Register.XMM1, MemorySize.UInt32 };
				yield return new object[] { "C4E1F9 6E 08", 5, Code.VEX_Vmovq_VX_Eq, Register.XMM1, MemorySize.UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovV_Reg_RegMem_2_Data))]
		void Test64_MovV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_MovV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F6E CD", 3, Code.Movd_P_Ed, Register.MM1, Register.EBP };
				yield return new object[] { "41 0F6E CD", 4, Code.Movd_P_Ed, Register.MM1, Register.R13D };

				yield return new object[] { "48 0F6E CD", 4, Code.Movq_P_Eq, Register.MM1, Register.RBP };
				yield return new object[] { "49 0F6E CD", 4, Code.Movq_P_Eq, Register.MM1, Register.R13 };

				yield return new object[] { "66 0F6E CD", 4, Code.Movd_VX_Ed, Register.XMM1, Register.EBP };
				yield return new object[] { "66 44 0F6E CD", 5, Code.Movd_VX_Ed, Register.XMM9, Register.EBP };
				yield return new object[] { "66 41 0F6E CD", 5, Code.Movd_VX_Ed, Register.XMM1, Register.R13D };
				yield return new object[] { "66 45 0F6E CD", 5, Code.Movd_VX_Ed, Register.XMM9, Register.R13D };

				yield return new object[] { "66 48 0F6E CD", 5, Code.Movq_VX_Eq, Register.XMM1, Register.RBP };
				yield return new object[] { "66 4C 0F6E CD", 5, Code.Movq_VX_Eq, Register.XMM9, Register.RBP };
				yield return new object[] { "66 49 0F6E CD", 5, Code.Movq_VX_Eq, Register.XMM1, Register.R13 };
				yield return new object[] { "66 4D 0F6E CD", 5, Code.Movq_VX_Eq, Register.XMM9, Register.R13 };

				yield return new object[] { "C5F9 6E CD", 4, Code.VEX_Vmovd_VX_Ed, Register.XMM1, Register.EBP };
				yield return new object[] { "C579 6E CD", 4, Code.VEX_Vmovd_VX_Ed, Register.XMM9, Register.EBP };
				yield return new object[] { "C4C179 6E CD", 5, Code.VEX_Vmovd_VX_Ed, Register.XMM1, Register.R13D };
				yield return new object[] { "C44179 6E CD", 5, Code.VEX_Vmovd_VX_Ed, Register.XMM9, Register.R13D };

				yield return new object[] { "C4E1F9 6E CD", 5, Code.VEX_Vmovq_VX_Eq, Register.XMM1, Register.RBP };
				yield return new object[] { "C461F9 6E CD", 5, Code.VEX_Vmovq_VX_Eq, Register.XMM9, Register.RBP };
				yield return new object[] { "C4C1F9 6E CD", 5, Code.VEX_Vmovq_VX_Eq, Register.XMM1, Register.R13 };
				yield return new object[] { "C441F9 6E CD", 5, Code.VEX_Vmovq_VX_Eq, Register.XMM9, Register.R13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VmovV_Reg_RegMem_1_Data))]
		void Test16_VmovV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, uint displ) {
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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VmovV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "62 F17D08 6E 50 01", 7, Code.EVEX_Vmovd_VX_Ed, Register.XMM2, MemorySize.UInt32, 4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VmovV_Reg_RegMem_2_Data))]
		void Test16_VmovV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VmovV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "62 F17D08 6E D3", 6, Code.EVEX_Vmovd_VX_Ed, Register.XMM2, Register.EBX };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VmovV_Reg_RegMem_1_Data))]
		void Test32_VmovV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, uint displ) {
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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VmovV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "62 F17D08 6E 50 01", 7, Code.EVEX_Vmovd_VX_Ed, Register.XMM2, MemorySize.UInt32, 4 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VmovV_Reg_RegMem_2_Data))]
		void Test32_VmovV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VmovV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "62 F17D08 6E D3", 6, Code.EVEX_Vmovd_VX_Ed, Register.XMM2, Register.EBX };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VmovV_Reg_RegMem_1_Data))]
		void Test64_VmovV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, uint displ) {
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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VmovV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "62 F17D08 6E 50 01", 7, Code.EVEX_Vmovd_VX_Ed, Register.XMM2, MemorySize.UInt32, 4 };

				yield return new object[] { "62 F1FD08 6E 50 01", 7, Code.EVEX_Vmovq_VX_Eq, Register.XMM2, MemorySize.UInt64, 8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VmovV_Reg_RegMem_2_Data))]
		void Test64_VmovV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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

			Assert.Equal(Register.None, instr.OpMask);
			Assert.False(instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VmovV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "62 F17D08 6E D3", 6, Code.EVEX_Vmovd_VX_Ed, Register.XMM2, Register.EBX };
				yield return new object[] { "62 E17D08 6E D3", 6, Code.EVEX_Vmovd_VX_Ed, Register.XMM18, Register.EBX };
				yield return new object[] { "62 517D08 6E D3", 6, Code.EVEX_Vmovd_VX_Ed, Register.XMM10, Register.R11D };
				yield return new object[] { "62 D17D08 6E D3", 6, Code.EVEX_Vmovd_VX_Ed, Register.XMM2, Register.R11D };

				yield return new object[] { "62 F1FD08 6E D3", 6, Code.EVEX_Vmovq_VX_Eq, Register.XMM2, Register.RBX };
				yield return new object[] { "62 E1FD08 6E D3", 6, Code.EVEX_Vmovq_VX_Eq, Register.XMM18, Register.RBX };
				yield return new object[] { "62 51FD08 6E D3", 6, Code.EVEX_Vmovq_VX_Eq, Register.XMM10, Register.R11 };
				yield return new object[] { "62 D1FD08 6E D3", 6, Code.EVEX_Vmovq_VX_Eq, Register.XMM2, Register.R11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovqV_Reg_RegMem_1_Data))]
		void Test16_MovqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_MovqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F6F 08", 3, Code.Movq_P_Q, Register.MM1, MemorySize.Packed64_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovqV_Reg_RegMem_2_Data))]
		void Test16_MovqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_MovqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F6F CD", 3, Code.Movq_P_Q, Register.MM1, Register.MM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovqV_Reg_RegMem_1_Data))]
		void Test32_MovqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_MovqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F6F 08", 3, Code.Movq_P_Q, Register.MM1, MemorySize.Packed64_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovqV_Reg_RegMem_2_Data))]
		void Test32_MovqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_MovqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F6F CD", 3, Code.Movq_P_Q, Register.MM1, Register.MM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovqV_Reg_RegMem_1_Data))]
		void Test64_MovqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_MovqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "0F6F 08", 3, Code.Movq_P_Q, Register.MM1, MemorySize.Packed64_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovqV_Reg_RegMem_2_Data))]
		void Test64_MovqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_MovqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "0F6F CD", 3, Code.Movq_P_Q, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F6F CD", 4, Code.Movq_P_Q, Register.MM1, Register.MM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovdqV_Reg_RegMem_1_Data))]
		void Test16_MovdqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_MovdqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F6F 08", 4, Code.Movdqa_VX_WX, Register.XMM1, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5F9 6F 10", 4, Code.VEX_Vmovdqa_VX_WX, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1F9 6F 10", 5, Code.VEX_Vmovdqa_VX_WX, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FD 6F 10", 4, Code.VEX_Vmovdqa_VY_WY, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1FD 6F 10", 5, Code.VEX_Vmovdqa_VY_WY, Register.YMM2, MemorySize.Packed256_Int32 };

				yield return new object[] { "F3 0F6F 08", 4, Code.Movdqu_VX_WX, Register.XMM1, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FA 6F 10", 4, Code.VEX_Vmovdqu_VX_WX, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1FA 6F 10", 5, Code.VEX_Vmovdqu_VX_WX, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FE 6F 10", 4, Code.VEX_Vmovdqu_VY_WY, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1FE 6F 10", 5, Code.VEX_Vmovdqu_VY_WY, Register.YMM2, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovdqV_Reg_RegMem_2_Data))]
		void Test16_MovdqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_MovdqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F6F CD", 4, Code.Movdqa_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5F9 6F CD", 4, Code.VEX_Vmovdqa_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FD 6F CD", 4, Code.VEX_Vmovdqa_VY_WY, Register.YMM1, Register.YMM5 };

				yield return new object[] { "F3 0F6F CD", 4, Code.Movdqu_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5FA 6F CD", 4, Code.VEX_Vmovdqu_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FE 6F CD", 4, Code.VEX_Vmovdqu_VY_WY, Register.YMM1, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovdqV_Reg_RegMem_1_Data))]
		void Test32_MovdqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_MovdqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F6F 08", 4, Code.Movdqa_VX_WX, Register.XMM1, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5F9 6F 10", 4, Code.VEX_Vmovdqa_VX_WX, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1F9 6F 10", 5, Code.VEX_Vmovdqa_VX_WX, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FD 6F 10", 4, Code.VEX_Vmovdqa_VY_WY, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1FD 6F 10", 5, Code.VEX_Vmovdqa_VY_WY, Register.YMM2, MemorySize.Packed256_Int32 };

				yield return new object[] { "F3 0F6F 08", 4, Code.Movdqu_VX_WX, Register.XMM1, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FA 6F 10", 4, Code.VEX_Vmovdqu_VX_WX, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1FA 6F 10", 5, Code.VEX_Vmovdqu_VX_WX, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FE 6F 10", 4, Code.VEX_Vmovdqu_VY_WY, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1FE 6F 10", 5, Code.VEX_Vmovdqu_VY_WY, Register.YMM2, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovdqV_Reg_RegMem_2_Data))]
		void Test32_MovdqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_MovdqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F6F CD", 4, Code.Movdqa_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5F9 6F CD", 4, Code.VEX_Vmovdqa_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FD 6F CD", 4, Code.VEX_Vmovdqa_VY_WY, Register.YMM1, Register.YMM5 };

				yield return new object[] { "F3 0F6F CD", 4, Code.Movdqu_VX_WX, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5FA 6F CD", 4, Code.VEX_Vmovdqu_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FE 6F CD", 4, Code.VEX_Vmovdqu_VY_WY, Register.YMM1, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovdqV_Reg_RegMem_1_Data))]
		void Test64_MovdqV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_MovdqV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0F6F 08", 4, Code.Movdqa_VX_WX, Register.XMM1, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5F9 6F 10", 4, Code.VEX_Vmovdqa_VX_WX, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1F9 6F 10", 5, Code.VEX_Vmovdqa_VX_WX, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FD 6F 10", 4, Code.VEX_Vmovdqa_VY_WY, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1FD 6F 10", 5, Code.VEX_Vmovdqa_VY_WY, Register.YMM2, MemorySize.Packed256_Int32 };

				yield return new object[] { "F3 0F6F 08", 4, Code.Movdqu_VX_WX, Register.XMM1, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FA 6F 10", 4, Code.VEX_Vmovdqu_VX_WX, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1FA 6F 10", 5, Code.VEX_Vmovdqu_VX_WX, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FE 6F 10", 4, Code.VEX_Vmovdqu_VY_WY, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1FE 6F 10", 5, Code.VEX_Vmovdqu_VY_WY, Register.YMM2, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovdqV_Reg_RegMem_2_Data))]
		void Test64_MovdqV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_MovdqV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0F6F CD", 4, Code.Movdqa_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F6F CD", 5, Code.Movdqa_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F6F CD", 5, Code.Movdqa_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F6F CD", 5, Code.Movdqa_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C5F9 6F CD", 4, Code.VEX_Vmovdqa_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C579 6F CD", 4, Code.VEX_Vmovdqa_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C179 6F CD", 5, Code.VEX_Vmovdqa_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44179 6F CD", 5, Code.VEX_Vmovdqa_VX_WX, Register.XMM9, Register.XMM13 };
				yield return new object[] { "C5FD 6F CD", 4, Code.VEX_Vmovdqa_VY_WY, Register.YMM1, Register.YMM5 };
				yield return new object[] { "C57D 6F CD", 4, Code.VEX_Vmovdqa_VY_WY, Register.YMM9, Register.YMM5 };
				yield return new object[] { "C4C17D 6F CD", 5, Code.VEX_Vmovdqa_VY_WY, Register.YMM1, Register.YMM13 };
				yield return new object[] { "C4417D 6F CD", 5, Code.VEX_Vmovdqa_VY_WY, Register.YMM9, Register.YMM13 };

				yield return new object[] { "F3 0F6F CD", 4, Code.Movdqu_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F3 44 0F6F CD", 5, Code.Movdqu_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F3 41 0F6F CD", 5, Code.Movdqu_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F3 45 0F6F CD", 5, Code.Movdqu_VX_WX, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C5FA 6F CD", 4, Code.VEX_Vmovdqu_VX_WX, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C57A 6F CD", 4, Code.VEX_Vmovdqu_VX_WX, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C17A 6F CD", 5, Code.VEX_Vmovdqu_VX_WX, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C4417A 6F CD", 5, Code.VEX_Vmovdqu_VX_WX, Register.XMM9, Register.XMM13 };
				yield return new object[] { "C5FE 6F CD", 4, Code.VEX_Vmovdqu_VY_WY, Register.YMM1, Register.YMM5 };
				yield return new object[] { "C57E 6F CD", 4, Code.VEX_Vmovdqu_VY_WY, Register.YMM9, Register.YMM5 };
				yield return new object[] { "C4C17E 6F CD", 5, Code.VEX_Vmovdqu_VY_WY, Register.YMM1, Register.YMM13 };
				yield return new object[] { "C4417E 6F CD", 5, Code.VEX_Vmovdqu_VY_WY, Register.YMM9, Register.YMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovdqV_Reg_RegMem_EVEX_1_Data))]
		void Test16_MovdqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_MovdqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17D08 6F 50 01", 7, Code.EVEX_Vmovdqa32_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F17D8B 6F 50 01", 7, Code.EVEX_Vmovdqa32_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, true };

				yield return new object[] { "62 F17D28 6F 50 01", 7, Code.EVEX_Vmovdqa32_VY_k1z_WY, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F17DAB 6F 50 01", 7, Code.EVEX_Vmovdqa32_VY_k1z_WY, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, true };

				yield return new object[] { "62 F17D48 6F 50 01", 7, Code.EVEX_Vmovdqa32_VZ_k1z_WZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F17DCB 6F 50 01", 7, Code.EVEX_Vmovdqa32_VZ_k1z_WZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, true };

				yield return new object[] { "62 F1FD08 6F 50 01", 7, Code.EVEX_Vmovdqa64_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F1FD8B 6F 50 01", 7, Code.EVEX_Vmovdqa64_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, true };

				yield return new object[] { "62 F1FD28 6F 50 01", 7, Code.EVEX_Vmovdqa64_VY_k1z_WY, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F1FDAB 6F 50 01", 7, Code.EVEX_Vmovdqa64_VY_k1z_WY, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, true };

				yield return new object[] { "62 F1FD48 6F 50 01", 7, Code.EVEX_Vmovdqa64_VZ_k1z_WZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F1FDCB 6F 50 01", 7, Code.EVEX_Vmovdqa64_VZ_k1z_WZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, true };

				yield return new object[] { "62 F17E08 6F 50 01", 7, Code.EVEX_Vmovdqu32_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F17E8B 6F 50 01", 7, Code.EVEX_Vmovdqu32_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, true };

				yield return new object[] { "62 F17E28 6F 50 01", 7, Code.EVEX_Vmovdqu32_VY_k1z_WY, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F17EAB 6F 50 01", 7, Code.EVEX_Vmovdqu32_VY_k1z_WY, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, true };

				yield return new object[] { "62 F17E48 6F 50 01", 7, Code.EVEX_Vmovdqu32_VZ_k1z_WZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F17ECB 6F 50 01", 7, Code.EVEX_Vmovdqu32_VZ_k1z_WZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, true };

				yield return new object[] { "62 F1FE08 6F 50 01", 7, Code.EVEX_Vmovdqu64_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F1FE8B 6F 50 01", 7, Code.EVEX_Vmovdqu64_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed128_Int64, 16, true };

				yield return new object[] { "62 F1FE28 6F 50 01", 7, Code.EVEX_Vmovdqu64_VY_k1z_WY, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F1FEAB 6F 50 01", 7, Code.EVEX_Vmovdqu64_VY_k1z_WY, Register.YMM2, Register.K3, MemorySize.Packed256_Int64, 32, true };

				yield return new object[] { "62 F1FE48 6F 50 01", 7, Code.EVEX_Vmovdqu64_VZ_k1z_WZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F1FECB 6F 50 01", 7, Code.EVEX_Vmovdqu64_VZ_k1z_WZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int64, 64, true };

				yield return new object[] { "62 F17F08 6F 50 01", 7, Code.EVEX_Vmovdqu8_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F17F8B 6F 50 01", 7, Code.EVEX_Vmovdqu8_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed128_Int8, 16, true };

				yield return new object[] { "62 F17F28 6F 50 01", 7, Code.EVEX_Vmovdqu8_VY_k1z_WY, Register.YMM2, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F17FAB 6F 50 01", 7, Code.EVEX_Vmovdqu8_VY_k1z_WY, Register.YMM2, Register.K3, MemorySize.Packed256_Int8, 32, true };

				yield return new object[] { "62 F17F48 6F 50 01", 7, Code.EVEX_Vmovdqu8_VZ_k1z_WZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F17FCB 6F 50 01", 7, Code.EVEX_Vmovdqu8_VZ_k1z_WZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int8, 64, true };

				yield return new object[] { "62 F1FF08 6F 50 01", 7, Code.EVEX_Vmovdqu16_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1FF8B 6F 50 01", 7, Code.EVEX_Vmovdqu16_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed128_Int16, 16, true };

				yield return new object[] { "62 F1FF28 6F 50 01", 7, Code.EVEX_Vmovdqu16_VY_k1z_WY, Register.YMM2, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1FFAB 6F 50 01", 7, Code.EVEX_Vmovdqu16_VY_k1z_WY, Register.YMM2, Register.K3, MemorySize.Packed256_Int16, 32, true };

				yield return new object[] { "62 F1FF48 6F 50 01", 7, Code.EVEX_Vmovdqu16_VZ_k1z_WZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1FFCB 6F 50 01", 7, Code.EVEX_Vmovdqu16_VZ_k1z_WZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int16, 64, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovdqV_Reg_RegMem_EVEX_2_Data))]
		void Test16_MovdqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_MovdqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17D08 6F D3", 6, Code.EVEX_Vmovdqa32_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D0B 6F D3", 6, Code.EVEX_Vmovdqa32_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D8B 6F D3", 6, Code.EVEX_Vmovdqa32_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D28 6F D3", 6, Code.EVEX_Vmovdqa32_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D2B 6F D3", 6, Code.EVEX_Vmovdqa32_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DAB 6F D3", 6, Code.EVEX_Vmovdqa32_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D48 6F D3", 6, Code.EVEX_Vmovdqa32_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D4B 6F D3", 6, Code.EVEX_Vmovdqa32_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DCB 6F D3", 6, Code.EVEX_Vmovdqa32_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD08 6F D3", 6, Code.EVEX_Vmovdqa64_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD0B 6F D3", 6, Code.EVEX_Vmovdqa64_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 6F D3", 6, Code.EVEX_Vmovdqa64_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD28 6F D3", 6, Code.EVEX_Vmovdqa64_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD2B 6F D3", 6, Code.EVEX_Vmovdqa64_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 6F D3", 6, Code.EVEX_Vmovdqa64_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD48 6F D3", 6, Code.EVEX_Vmovdqa64_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD4B 6F D3", 6, Code.EVEX_Vmovdqa64_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDCB 6F D3", 6, Code.EVEX_Vmovdqa64_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E08 6F D3", 6, Code.EVEX_Vmovdqu32_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E0B 6F D3", 6, Code.EVEX_Vmovdqu32_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E8B 6F D3", 6, Code.EVEX_Vmovdqu32_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E28 6F D3", 6, Code.EVEX_Vmovdqu32_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E2B 6F D3", 6, Code.EVEX_Vmovdqu32_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17EAB 6F D3", 6, Code.EVEX_Vmovdqu32_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E48 6F D3", 6, Code.EVEX_Vmovdqu32_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E4B 6F D3", 6, Code.EVEX_Vmovdqu32_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17ECB 6F D3", 6, Code.EVEX_Vmovdqu32_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE08 6F D3", 6, Code.EVEX_Vmovdqu64_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE0B 6F D3", 6, Code.EVEX_Vmovdqu64_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE8B 6F D3", 6, Code.EVEX_Vmovdqu64_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE28 6F D3", 6, Code.EVEX_Vmovdqu64_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE2B 6F D3", 6, Code.EVEX_Vmovdqu64_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FEAB 6F D3", 6, Code.EVEX_Vmovdqu64_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE48 6F D3", 6, Code.EVEX_Vmovdqu64_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE4B 6F D3", 6, Code.EVEX_Vmovdqu64_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FECB 6F D3", 6, Code.EVEX_Vmovdqu64_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F08 6F D3", 6, Code.EVEX_Vmovdqu8_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F0B 6F D3", 6, Code.EVEX_Vmovdqu8_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F8B 6F D3", 6, Code.EVEX_Vmovdqu8_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F28 6F D3", 6, Code.EVEX_Vmovdqu8_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F2B 6F D3", 6, Code.EVEX_Vmovdqu8_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17FAB 6F D3", 6, Code.EVEX_Vmovdqu8_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F48 6F D3", 6, Code.EVEX_Vmovdqu8_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F4B 6F D3", 6, Code.EVEX_Vmovdqu8_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17FCB 6F D3", 6, Code.EVEX_Vmovdqu8_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF08 6F D3", 6, Code.EVEX_Vmovdqu16_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF0B 6F D3", 6, Code.EVEX_Vmovdqu16_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF8B 6F D3", 6, Code.EVEX_Vmovdqu16_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF28 6F D3", 6, Code.EVEX_Vmovdqu16_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF2B 6F D3", 6, Code.EVEX_Vmovdqu16_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FFAB 6F D3", 6, Code.EVEX_Vmovdqu16_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF48 6F D3", 6, Code.EVEX_Vmovdqu16_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF4B 6F D3", 6, Code.EVEX_Vmovdqu16_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FFCB 6F D3", 6, Code.EVEX_Vmovdqu16_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovdqV_Reg_RegMem_EVEX_1_Data))]
		void Test32_MovdqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_MovdqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17D08 6F 50 01", 7, Code.EVEX_Vmovdqa32_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F17D8B 6F 50 01", 7, Code.EVEX_Vmovdqa32_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, true };

				yield return new object[] { "62 F17D28 6F 50 01", 7, Code.EVEX_Vmovdqa32_VY_k1z_WY, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F17DAB 6F 50 01", 7, Code.EVEX_Vmovdqa32_VY_k1z_WY, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, true };

				yield return new object[] { "62 F17D48 6F 50 01", 7, Code.EVEX_Vmovdqa32_VZ_k1z_WZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F17DCB 6F 50 01", 7, Code.EVEX_Vmovdqa32_VZ_k1z_WZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, true };

				yield return new object[] { "62 F1FD08 6F 50 01", 7, Code.EVEX_Vmovdqa64_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F1FD8B 6F 50 01", 7, Code.EVEX_Vmovdqa64_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, true };

				yield return new object[] { "62 F1FD28 6F 50 01", 7, Code.EVEX_Vmovdqa64_VY_k1z_WY, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F1FDAB 6F 50 01", 7, Code.EVEX_Vmovdqa64_VY_k1z_WY, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, true };

				yield return new object[] { "62 F1FD48 6F 50 01", 7, Code.EVEX_Vmovdqa64_VZ_k1z_WZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F1FDCB 6F 50 01", 7, Code.EVEX_Vmovdqa64_VZ_k1z_WZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, true };

				yield return new object[] { "62 F17E08 6F 50 01", 7, Code.EVEX_Vmovdqu32_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F17E8B 6F 50 01", 7, Code.EVEX_Vmovdqu32_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, true };

				yield return new object[] { "62 F17E28 6F 50 01", 7, Code.EVEX_Vmovdqu32_VY_k1z_WY, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F17EAB 6F 50 01", 7, Code.EVEX_Vmovdqu32_VY_k1z_WY, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, true };

				yield return new object[] { "62 F17E48 6F 50 01", 7, Code.EVEX_Vmovdqu32_VZ_k1z_WZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F17ECB 6F 50 01", 7, Code.EVEX_Vmovdqu32_VZ_k1z_WZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, true };

				yield return new object[] { "62 F1FE08 6F 50 01", 7, Code.EVEX_Vmovdqu64_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F1FE8B 6F 50 01", 7, Code.EVEX_Vmovdqu64_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed128_Int64, 16, true };

				yield return new object[] { "62 F1FE28 6F 50 01", 7, Code.EVEX_Vmovdqu64_VY_k1z_WY, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F1FEAB 6F 50 01", 7, Code.EVEX_Vmovdqu64_VY_k1z_WY, Register.YMM2, Register.K3, MemorySize.Packed256_Int64, 32, true };

				yield return new object[] { "62 F1FE48 6F 50 01", 7, Code.EVEX_Vmovdqu64_VZ_k1z_WZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F1FECB 6F 50 01", 7, Code.EVEX_Vmovdqu64_VZ_k1z_WZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int64, 64, true };

				yield return new object[] { "62 F17F08 6F 50 01", 7, Code.EVEX_Vmovdqu8_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F17F8B 6F 50 01", 7, Code.EVEX_Vmovdqu8_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed128_Int8, 16, true };

				yield return new object[] { "62 F17F28 6F 50 01", 7, Code.EVEX_Vmovdqu8_VY_k1z_WY, Register.YMM2, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F17FAB 6F 50 01", 7, Code.EVEX_Vmovdqu8_VY_k1z_WY, Register.YMM2, Register.K3, MemorySize.Packed256_Int8, 32, true };

				yield return new object[] { "62 F17F48 6F 50 01", 7, Code.EVEX_Vmovdqu8_VZ_k1z_WZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F17FCB 6F 50 01", 7, Code.EVEX_Vmovdqu8_VZ_k1z_WZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int8, 64, true };

				yield return new object[] { "62 F1FF08 6F 50 01", 7, Code.EVEX_Vmovdqu16_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1FF8B 6F 50 01", 7, Code.EVEX_Vmovdqu16_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed128_Int16, 16, true };

				yield return new object[] { "62 F1FF28 6F 50 01", 7, Code.EVEX_Vmovdqu16_VY_k1z_WY, Register.YMM2, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1FFAB 6F 50 01", 7, Code.EVEX_Vmovdqu16_VY_k1z_WY, Register.YMM2, Register.K3, MemorySize.Packed256_Int16, 32, true };

				yield return new object[] { "62 F1FF48 6F 50 01", 7, Code.EVEX_Vmovdqu16_VZ_k1z_WZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1FFCB 6F 50 01", 7, Code.EVEX_Vmovdqu16_VZ_k1z_WZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int16, 64, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovdqV_Reg_RegMem_EVEX_2_Data))]
		void Test32_MovdqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_MovdqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17D08 6F D3", 6, Code.EVEX_Vmovdqa32_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D0B 6F D3", 6, Code.EVEX_Vmovdqa32_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D8B 6F D3", 6, Code.EVEX_Vmovdqa32_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D28 6F D3", 6, Code.EVEX_Vmovdqa32_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D2B 6F D3", 6, Code.EVEX_Vmovdqa32_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DAB 6F D3", 6, Code.EVEX_Vmovdqa32_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D48 6F D3", 6, Code.EVEX_Vmovdqa32_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D4B 6F D3", 6, Code.EVEX_Vmovdqa32_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17DCB 6F D3", 6, Code.EVEX_Vmovdqa32_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD08 6F D3", 6, Code.EVEX_Vmovdqa64_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD0B 6F D3", 6, Code.EVEX_Vmovdqa64_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B 6F D3", 6, Code.EVEX_Vmovdqa64_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD28 6F D3", 6, Code.EVEX_Vmovdqa64_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD2B 6F D3", 6, Code.EVEX_Vmovdqa64_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB 6F D3", 6, Code.EVEX_Vmovdqa64_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD48 6F D3", 6, Code.EVEX_Vmovdqa64_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD4B 6F D3", 6, Code.EVEX_Vmovdqa64_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDCB 6F D3", 6, Code.EVEX_Vmovdqa64_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E08 6F D3", 6, Code.EVEX_Vmovdqu32_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E0B 6F D3", 6, Code.EVEX_Vmovdqu32_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E8B 6F D3", 6, Code.EVEX_Vmovdqu32_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E28 6F D3", 6, Code.EVEX_Vmovdqu32_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E2B 6F D3", 6, Code.EVEX_Vmovdqu32_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17EAB 6F D3", 6, Code.EVEX_Vmovdqu32_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E48 6F D3", 6, Code.EVEX_Vmovdqu32_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E4B 6F D3", 6, Code.EVEX_Vmovdqu32_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17ECB 6F D3", 6, Code.EVEX_Vmovdqu32_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE08 6F D3", 6, Code.EVEX_Vmovdqu64_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE0B 6F D3", 6, Code.EVEX_Vmovdqu64_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE8B 6F D3", 6, Code.EVEX_Vmovdqu64_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE28 6F D3", 6, Code.EVEX_Vmovdqu64_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE2B 6F D3", 6, Code.EVEX_Vmovdqu64_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FEAB 6F D3", 6, Code.EVEX_Vmovdqu64_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE48 6F D3", 6, Code.EVEX_Vmovdqu64_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE4B 6F D3", 6, Code.EVEX_Vmovdqu64_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FECB 6F D3", 6, Code.EVEX_Vmovdqu64_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F08 6F D3", 6, Code.EVEX_Vmovdqu8_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F0B 6F D3", 6, Code.EVEX_Vmovdqu8_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F8B 6F D3", 6, Code.EVEX_Vmovdqu8_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F28 6F D3", 6, Code.EVEX_Vmovdqu8_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F2B 6F D3", 6, Code.EVEX_Vmovdqu8_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17FAB 6F D3", 6, Code.EVEX_Vmovdqu8_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F48 6F D3", 6, Code.EVEX_Vmovdqu8_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F4B 6F D3", 6, Code.EVEX_Vmovdqu8_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17FCB 6F D3", 6, Code.EVEX_Vmovdqu8_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF08 6F D3", 6, Code.EVEX_Vmovdqu16_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF0B 6F D3", 6, Code.EVEX_Vmovdqu16_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF8B 6F D3", 6, Code.EVEX_Vmovdqu16_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF28 6F D3", 6, Code.EVEX_Vmovdqu16_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF2B 6F D3", 6, Code.EVEX_Vmovdqu16_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FFAB 6F D3", 6, Code.EVEX_Vmovdqu16_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF48 6F D3", 6, Code.EVEX_Vmovdqu16_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF4B 6F D3", 6, Code.EVEX_Vmovdqu16_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FFCB 6F D3", 6, Code.EVEX_Vmovdqu16_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovdqV_Reg_RegMem_EVEX_1_Data))]
		void Test64_MovdqV_Reg_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_MovdqV_Reg_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17D08 6F 50 01", 7, Code.EVEX_Vmovdqa32_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F17D8B 6F 50 01", 7, Code.EVEX_Vmovdqa32_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, true };

				yield return new object[] { "62 F17D28 6F 50 01", 7, Code.EVEX_Vmovdqa32_VY_k1z_WY, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F17DAB 6F 50 01", 7, Code.EVEX_Vmovdqa32_VY_k1z_WY, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, true };

				yield return new object[] { "62 F17D48 6F 50 01", 7, Code.EVEX_Vmovdqa32_VZ_k1z_WZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F17DCB 6F 50 01", 7, Code.EVEX_Vmovdqa32_VZ_k1z_WZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, true };

				yield return new object[] { "62 F1FD08 6F 50 01", 7, Code.EVEX_Vmovdqa64_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F1FD8B 6F 50 01", 7, Code.EVEX_Vmovdqa64_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, true };

				yield return new object[] { "62 F1FD28 6F 50 01", 7, Code.EVEX_Vmovdqa64_VY_k1z_WY, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F1FDAB 6F 50 01", 7, Code.EVEX_Vmovdqa64_VY_k1z_WY, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, true };

				yield return new object[] { "62 F1FD48 6F 50 01", 7, Code.EVEX_Vmovdqa64_VZ_k1z_WZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F1FDCB 6F 50 01", 7, Code.EVEX_Vmovdqa64_VZ_k1z_WZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, true };

				yield return new object[] { "62 F17E08 6F 50 01", 7, Code.EVEX_Vmovdqu32_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F17E8B 6F 50 01", 7, Code.EVEX_Vmovdqu32_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed128_Int32, 16, true };

				yield return new object[] { "62 F17E28 6F 50 01", 7, Code.EVEX_Vmovdqu32_VY_k1z_WY, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F17EAB 6F 50 01", 7, Code.EVEX_Vmovdqu32_VY_k1z_WY, Register.YMM2, Register.K3, MemorySize.Packed256_Int32, 32, true };

				yield return new object[] { "62 F17E48 6F 50 01", 7, Code.EVEX_Vmovdqu32_VZ_k1z_WZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 F17ECB 6F 50 01", 7, Code.EVEX_Vmovdqu32_VZ_k1z_WZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int32, 64, true };

				yield return new object[] { "62 F1FE08 6F 50 01", 7, Code.EVEX_Vmovdqu64_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F1FE8B 6F 50 01", 7, Code.EVEX_Vmovdqu64_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed128_Int64, 16, true };

				yield return new object[] { "62 F1FE28 6F 50 01", 7, Code.EVEX_Vmovdqu64_VY_k1z_WY, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F1FEAB 6F 50 01", 7, Code.EVEX_Vmovdqu64_VY_k1z_WY, Register.YMM2, Register.K3, MemorySize.Packed256_Int64, 32, true };

				yield return new object[] { "62 F1FE48 6F 50 01", 7, Code.EVEX_Vmovdqu64_VZ_k1z_WZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F1FECB 6F 50 01", 7, Code.EVEX_Vmovdqu64_VZ_k1z_WZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int64, 64, true };

				yield return new object[] { "62 F17F08 6F 50 01", 7, Code.EVEX_Vmovdqu8_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F17F8B 6F 50 01", 7, Code.EVEX_Vmovdqu8_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed128_Int8, 16, true };

				yield return new object[] { "62 F17F28 6F 50 01", 7, Code.EVEX_Vmovdqu8_VY_k1z_WY, Register.YMM2, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F17FAB 6F 50 01", 7, Code.EVEX_Vmovdqu8_VY_k1z_WY, Register.YMM2, Register.K3, MemorySize.Packed256_Int8, 32, true };

				yield return new object[] { "62 F17F48 6F 50 01", 7, Code.EVEX_Vmovdqu8_VZ_k1z_WZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F17FCB 6F 50 01", 7, Code.EVEX_Vmovdqu8_VZ_k1z_WZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int8, 64, true };

				yield return new object[] { "62 F1FF08 6F 50 01", 7, Code.EVEX_Vmovdqu16_VX_k1z_WX, Register.XMM2, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1FF8B 6F 50 01", 7, Code.EVEX_Vmovdqu16_VX_k1z_WX, Register.XMM2, Register.K3, MemorySize.Packed128_Int16, 16, true };

				yield return new object[] { "62 F1FF28 6F 50 01", 7, Code.EVEX_Vmovdqu16_VY_k1z_WY, Register.YMM2, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1FFAB 6F 50 01", 7, Code.EVEX_Vmovdqu16_VY_k1z_WY, Register.YMM2, Register.K3, MemorySize.Packed256_Int16, 32, true };

				yield return new object[] { "62 F1FF48 6F 50 01", 7, Code.EVEX_Vmovdqu16_VZ_k1z_WZ, Register.ZMM2, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1FFCB 6F 50 01", 7, Code.EVEX_Vmovdqu16_VZ_k1z_WZ, Register.ZMM2, Register.K3, MemorySize.Packed512_Int16, 64, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovdqV_Reg_RegMem_EVEX_2_Data))]
		void Test64_MovdqV_Reg_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_MovdqV_Reg_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F17D08 6F D3", 6, Code.EVEX_Vmovdqa32_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D0B 6F D3", 6, Code.EVEX_Vmovdqa32_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317D8B 6F D3", 6, Code.EVEX_Vmovdqa32_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17D8B 6F D3", 6, Code.EVEX_Vmovdqa32_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D28 6F D3", 6, Code.EVEX_Vmovdqa32_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D2B 6F D3", 6, Code.EVEX_Vmovdqa32_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317DAB 6F D3", 6, Code.EVEX_Vmovdqa32_VY_k1z_WY, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17DAB 6F D3", 6, Code.EVEX_Vmovdqa32_VY_k1z_WY, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17D48 6F D3", 6, Code.EVEX_Vmovdqa32_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17D4B 6F D3", 6, Code.EVEX_Vmovdqa32_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317DCB 6F D3", 6, Code.EVEX_Vmovdqa32_VZ_k1z_WZ, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17DCB 6F D3", 6, Code.EVEX_Vmovdqa32_VZ_k1z_WZ, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD08 6F D3", 6, Code.EVEX_Vmovdqa64_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD0B 6F D3", 6, Code.EVEX_Vmovdqa64_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FD8B 6F D3", 6, Code.EVEX_Vmovdqa64_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FD8B 6F D3", 6, Code.EVEX_Vmovdqa64_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD28 6F D3", 6, Code.EVEX_Vmovdqa64_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD2B 6F D3", 6, Code.EVEX_Vmovdqa64_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FDAB 6F D3", 6, Code.EVEX_Vmovdqa64_VY_k1z_WY, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FDAB 6F D3", 6, Code.EVEX_Vmovdqa64_VY_k1z_WY, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD48 6F D3", 6, Code.EVEX_Vmovdqa64_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD4B 6F D3", 6, Code.EVEX_Vmovdqa64_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FDCB 6F D3", 6, Code.EVEX_Vmovdqa64_VZ_k1z_WZ, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FDCB 6F D3", 6, Code.EVEX_Vmovdqa64_VZ_k1z_WZ, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E08 6F D3", 6, Code.EVEX_Vmovdqu32_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E0B 6F D3", 6, Code.EVEX_Vmovdqu32_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317E8B 6F D3", 6, Code.EVEX_Vmovdqu32_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17E8B 6F D3", 6, Code.EVEX_Vmovdqu32_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E28 6F D3", 6, Code.EVEX_Vmovdqu32_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E2B 6F D3", 6, Code.EVEX_Vmovdqu32_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317EAB 6F D3", 6, Code.EVEX_Vmovdqu32_VY_k1z_WY, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17EAB 6F D3", 6, Code.EVEX_Vmovdqu32_VY_k1z_WY, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E48 6F D3", 6, Code.EVEX_Vmovdqu32_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E4B 6F D3", 6, Code.EVEX_Vmovdqu32_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317ECB 6F D3", 6, Code.EVEX_Vmovdqu32_VZ_k1z_WZ, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17ECB 6F D3", 6, Code.EVEX_Vmovdqu32_VZ_k1z_WZ, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE08 6F D3", 6, Code.EVEX_Vmovdqu64_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE0B 6F D3", 6, Code.EVEX_Vmovdqu64_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FE8B 6F D3", 6, Code.EVEX_Vmovdqu64_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FE8B 6F D3", 6, Code.EVEX_Vmovdqu64_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE28 6F D3", 6, Code.EVEX_Vmovdqu64_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE2B 6F D3", 6, Code.EVEX_Vmovdqu64_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FEAB 6F D3", 6, Code.EVEX_Vmovdqu64_VY_k1z_WY, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FEAB 6F D3", 6, Code.EVEX_Vmovdqu64_VY_k1z_WY, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE48 6F D3", 6, Code.EVEX_Vmovdqu64_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE4B 6F D3", 6, Code.EVEX_Vmovdqu64_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FECB 6F D3", 6, Code.EVEX_Vmovdqu64_VZ_k1z_WZ, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FECB 6F D3", 6, Code.EVEX_Vmovdqu64_VZ_k1z_WZ, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F08 6F D3", 6, Code.EVEX_Vmovdqu8_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F0B 6F D3", 6, Code.EVEX_Vmovdqu8_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317F8B 6F D3", 6, Code.EVEX_Vmovdqu8_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17F8B 6F D3", 6, Code.EVEX_Vmovdqu8_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F28 6F D3", 6, Code.EVEX_Vmovdqu8_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F2B 6F D3", 6, Code.EVEX_Vmovdqu8_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317FAB 6F D3", 6, Code.EVEX_Vmovdqu8_VY_k1z_WY, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17FAB 6F D3", 6, Code.EVEX_Vmovdqu8_VY_k1z_WY, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17F48 6F D3", 6, Code.EVEX_Vmovdqu8_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F17F4B 6F D3", 6, Code.EVEX_Vmovdqu8_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 317FCB 6F D3", 6, Code.EVEX_Vmovdqu8_VZ_k1z_WZ, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C17FCB 6F D3", 6, Code.EVEX_Vmovdqu8_VZ_k1z_WZ, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF08 6F D3", 6, Code.EVEX_Vmovdqu16_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF0B 6F D3", 6, Code.EVEX_Vmovdqu16_VX_k1z_WX, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FF8B 6F D3", 6, Code.EVEX_Vmovdqu16_VX_k1z_WX, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FF8B 6F D3", 6, Code.EVEX_Vmovdqu16_VX_k1z_WX, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF28 6F D3", 6, Code.EVEX_Vmovdqu16_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF2B 6F D3", 6, Code.EVEX_Vmovdqu16_VY_k1z_WY, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FFAB 6F D3", 6, Code.EVEX_Vmovdqu16_VY_k1z_WY, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FFAB 6F D3", 6, Code.EVEX_Vmovdqu16_VY_k1z_WY, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF48 6F D3", 6, Code.EVEX_Vmovdqu16_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF4B 6F D3", 6, Code.EVEX_Vmovdqu16_VZ_k1z_WZ, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 31FFCB 6F D3", 6, Code.EVEX_Vmovdqu16_VZ_k1z_WZ, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 C1FFCB 6F D3", 6, Code.EVEX_Vmovdqu16_VZ_k1z_WZ, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.None, true, false };
			}
		}
	}
}
