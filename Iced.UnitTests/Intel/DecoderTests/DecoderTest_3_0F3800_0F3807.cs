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
	public sealed class DecoderTest_3_0F3800_0F3807 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_PshufbV_VX_WX_1_Data))]
		void Test16_PshufbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PshufbV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3800 08", 4, Code.Pshufb_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "66 0F3800 08", 5, Code.Pshufb_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PshufbV_VX_WX_2_Data))]
		void Test16_PshufbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PshufbV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3800 CD", 4, Code.Pshufb_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3800 CD", 5, Code.Pshufb_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PshufbV_VX_WX_1_Data))]
		void Test32_PshufbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PshufbV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3800 08", 4, Code.Pshufb_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "66 0F3800 08", 5, Code.Pshufb_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PshufbV_VX_WX_2_Data))]
		void Test32_PshufbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PshufbV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3800 CD", 4, Code.Pshufb_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3800 CD", 5, Code.Pshufb_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PshufbV_VX_WX_1_Data))]
		void Test64_PshufbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PshufbV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3800 08", 4, Code.Pshufb_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "66 0F3800 08", 5, Code.Pshufb_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PshufbV_VX_WX_2_Data))]
		void Test64_PshufbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PshufbV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3800 CD", 4, Code.Pshufb_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F3800 CD", 5, Code.Pshufb_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3800 CD", 5, Code.Pshufb_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3800 CD", 6, Code.Pshufb_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3800 CD", 6, Code.Pshufb_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3800 CD", 6, Code.Pshufb_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpshufbV_VX_HX_WX_1_Data))]
		void Test16_VpshufbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpshufbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 00 10", 5, Code.VEX_Vpshufb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E24D 00 10", 5, Code.VEX_Vpshufb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
				yield return new object[] { "C4E2C9 00 10", 5, Code.VEX_Vpshufb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E2CD 00 10", 5, Code.VEX_Vpshufb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpshufbV_VX_HX_WX_2_Data))]
		void Test16_VpshufbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpshufbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 00 D3", 5, Code.VEX_Vpshufb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 00 D3", 5, Code.VEX_Vpshufb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpshufbV_VX_HX_WX_1_Data))]
		void Test32_VpshufbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpshufbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 00 10", 5, Code.VEX_Vpshufb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E24D 00 10", 5, Code.VEX_Vpshufb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
				yield return new object[] { "C4E2C9 00 10", 5, Code.VEX_Vpshufb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E2CD 00 10", 5, Code.VEX_Vpshufb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpshufbV_VX_HX_WX_2_Data))]
		void Test32_VpshufbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpshufbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 00 D3", 5, Code.VEX_Vpshufb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 00 D3", 5, Code.VEX_Vpshufb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpshufbV_VX_HX_WX_1_Data))]
		void Test64_VpshufbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpshufbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 00 10", 5, Code.VEX_Vpshufb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E24D 00 10", 5, Code.VEX_Vpshufb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
				yield return new object[] { "C4E2C9 00 10", 5, Code.VEX_Vpshufb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E2CD 00 10", 5, Code.VEX_Vpshufb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpshufbV_VX_HX_WX_2_Data))]
		void Test64_VpshufbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpshufbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 00 D3", 5, Code.VEX_Vpshufb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 00 D3", 5, Code.VEX_Vpshufb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C46249 00 D3", 5, Code.VEX_Vpshufb_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4624D 00 D3", 5, Code.VEX_Vpshufb_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E209 00 D3", 5, Code.VEX_Vpshufb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E20D 00 D3", 5, Code.VEX_Vpshufb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C249 00 D3", 5, Code.VEX_Vpshufb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C24D 00 D3", 5, Code.VEX_Vpshufb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpshufbV_VX_k1_HX_WX_1_Data))]
		void Test16_VpshufbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpshufbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 00 50 01", 7, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24D8D 00 50 01", 7, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F24D08 00 50 01", 7, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F2CD0B 00 50 01", 7, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F24D2B 00 50 01", 7, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24DAD 00 50 01", 7, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F24D28 00 50 01", 7, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F2CD2B 00 50 01", 7, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F24D4B 00 50 01", 7, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24DCD 00 50 01", 7, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true };
				yield return new object[] { "62 F24D48 00 50 01", 7, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F2CD4B 00 50 01", 7, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpshufbV_VX_k1_HX_WX_2_Data))]
		void Test16_VpshufbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpshufbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 00 D3", 6, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 00 D3", 6, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD0B 00 D3", 6, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 00 D3", 6, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DAB 00 D3", 6, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD2B 00 D3", 6, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 00 D3", 6, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DCB 00 D3", 6, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD4B 00 D3", 6, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpshufbV_VX_k1_HX_WX_1_Data))]
		void Test32_VpshufbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpshufbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 00 50 01", 7, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24D8D 00 50 01", 7, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F24D08 00 50 01", 7, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F2CD0B 00 50 01", 7, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F24D2B 00 50 01", 7, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24DAD 00 50 01", 7, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F24D28 00 50 01", 7, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F2CD2B 00 50 01", 7, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F24D4B 00 50 01", 7, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24DCD 00 50 01", 7, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true };
				yield return new object[] { "62 F24D48 00 50 01", 7, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F2CD4B 00 50 01", 7, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpshufbV_VX_k1_HX_WX_2_Data))]
		void Test32_VpshufbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpshufbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 00 D3", 6, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 00 D3", 6, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD0B 00 D3", 6, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 00 D3", 6, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DAB 00 D3", 6, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD2B 00 D3", 6, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 00 D3", 6, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DCB 00 D3", 6, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD4B 00 D3", 6, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpshufbV_VX_k1_HX_WX_1_Data))]
		void Test64_VpshufbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpshufbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 00 50 01", 7, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F24D8D 00 50 01", 7, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F24D08 00 50 01", 7, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F2CD0B 00 50 01", 7, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F24D2B 00 50 01", 7, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F24DAD 00 50 01", 7, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F24D28 00 50 01", 7, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F2CD2B 00 50 01", 7, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F24D4B 00 50 01", 7, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F24DCD 00 50 01", 7, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true };
				yield return new object[] { "62 F24D48 00 50 01", 7, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F2CD4B 00 50 01", 7, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpshufbV_VX_k1_HX_WX_2_Data))]
		void Test64_VpshufbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpshufbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 00 D3", 6, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D8B 00 D3", 6, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D03 00 D3", 6, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D0B 00 D3", 6, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD0B 00 D3", 6, Code.EVEX_Vpshufb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 00 D3", 6, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20DAB 00 D3", 6, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D23 00 D3", 6, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D2B 00 D3", 6, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD2B 00 D3", 6, Code.EVEX_Vpshufb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 00 D3", 6, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20DCB 00 D3", 6, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D43 00 D3", 6, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D4B 00 D3", 6, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD4B 00 D3", 6, Code.EVEX_Vpshufb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PhaddwV_VX_WX_1_Data))]
		void Test16_PhaddwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PhaddwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3801 08", 4, Code.Phaddw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0F3801 08", 5, Code.Phaddw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PhaddwV_VX_WX_2_Data))]
		void Test16_PhaddwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PhaddwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3801 CD", 4, Code.Phaddw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3801 CD", 5, Code.Phaddw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PhaddwV_VX_WX_1_Data))]
		void Test32_PhaddwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PhaddwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3801 08", 4, Code.Phaddw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0F3801 08", 5, Code.Phaddw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PhaddwV_VX_WX_2_Data))]
		void Test32_PhaddwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PhaddwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3801 CD", 4, Code.Phaddw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3801 CD", 5, Code.Phaddw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PhaddwV_VX_WX_1_Data))]
		void Test64_PhaddwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PhaddwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3801 08", 4, Code.Phaddw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0F3801 08", 5, Code.Phaddw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PhaddwV_VX_WX_2_Data))]
		void Test64_PhaddwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PhaddwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3801 CD", 4, Code.Phaddw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F3801 CD", 5, Code.Phaddw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3801 CD", 5, Code.Phaddw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3801 CD", 6, Code.Phaddw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3801 CD", 6, Code.Phaddw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3801 CD", 6, Code.Phaddw_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VphaddwV_VX_HX_WX_1_Data))]
		void Test16_VphaddwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VphaddwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 01 10", 5, Code.VEX_Vphaddw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E24D 01 10", 5, Code.VEX_Vphaddw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E2C9 01 10", 5, Code.VEX_Vphaddw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E2CD 01 10", 5, Code.VEX_Vphaddw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VphaddwV_VX_HX_WX_2_Data))]
		void Test16_VphaddwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VphaddwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 01 D3", 5, Code.VEX_Vphaddw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 01 D3", 5, Code.VEX_Vphaddw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VphaddwV_VX_HX_WX_1_Data))]
		void Test32_VphaddwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VphaddwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 01 10", 5, Code.VEX_Vphaddw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E24D 01 10", 5, Code.VEX_Vphaddw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E2C9 01 10", 5, Code.VEX_Vphaddw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E2CD 01 10", 5, Code.VEX_Vphaddw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VphaddwV_VX_HX_WX_2_Data))]
		void Test32_VphaddwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VphaddwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 01 D3", 5, Code.VEX_Vphaddw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 01 D3", 5, Code.VEX_Vphaddw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VphaddwV_VX_HX_WX_1_Data))]
		void Test64_VphaddwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VphaddwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 01 10", 5, Code.VEX_Vphaddw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E24D 01 10", 5, Code.VEX_Vphaddw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E2C9 01 10", 5, Code.VEX_Vphaddw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E2CD 01 10", 5, Code.VEX_Vphaddw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VphaddwV_VX_HX_WX_2_Data))]
		void Test64_VphaddwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VphaddwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 01 D3", 5, Code.VEX_Vphaddw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 01 D3", 5, Code.VEX_Vphaddw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C46249 01 D3", 5, Code.VEX_Vphaddw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4624D 01 D3", 5, Code.VEX_Vphaddw_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E209 01 D3", 5, Code.VEX_Vphaddw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E20D 01 D3", 5, Code.VEX_Vphaddw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C249 01 D3", 5, Code.VEX_Vphaddw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C24D 01 D3", 5, Code.VEX_Vphaddw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PhadddV_VX_WX_1_Data))]
		void Test16_PhadddV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PhadddV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3802 08", 4, Code.Phaddd_mm_mmm64, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0F3802 08", 5, Code.Phaddd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PhadddV_VX_WX_2_Data))]
		void Test16_PhadddV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PhadddV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3802 CD", 4, Code.Phaddd_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3802 CD", 5, Code.Phaddd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PhadddV_VX_WX_1_Data))]
		void Test32_PhadddV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PhadddV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3802 08", 4, Code.Phaddd_mm_mmm64, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0F3802 08", 5, Code.Phaddd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PhadddV_VX_WX_2_Data))]
		void Test32_PhadddV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PhadddV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3802 CD", 4, Code.Phaddd_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3802 CD", 5, Code.Phaddd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PhadddV_VX_WX_1_Data))]
		void Test64_PhadddV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PhadddV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3802 08", 4, Code.Phaddd_mm_mmm64, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0F3802 08", 5, Code.Phaddd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PhadddV_VX_WX_2_Data))]
		void Test64_PhadddV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PhadddV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3802 CD", 4, Code.Phaddd_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F3802 CD", 5, Code.Phaddd_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3802 CD", 5, Code.Phaddd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3802 CD", 6, Code.Phaddd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3802 CD", 6, Code.Phaddd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3802 CD", 6, Code.Phaddd_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VphadddV_VX_HX_WX_1_Data))]
		void Test16_VphadddV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VphadddV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 02 10", 5, Code.VEX_Vphaddd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E24D 02 10", 5, Code.VEX_Vphaddd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2C9 02 10", 5, Code.VEX_Vphaddd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2CD 02 10", 5, Code.VEX_Vphaddd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VphadddV_VX_HX_WX_2_Data))]
		void Test16_VphadddV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VphadddV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 02 D3", 5, Code.VEX_Vphaddd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 02 D3", 5, Code.VEX_Vphaddd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VphadddV_VX_HX_WX_1_Data))]
		void Test32_VphadddV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VphadddV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 02 10", 5, Code.VEX_Vphaddd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E24D 02 10", 5, Code.VEX_Vphaddd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2C9 02 10", 5, Code.VEX_Vphaddd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2CD 02 10", 5, Code.VEX_Vphaddd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VphadddV_VX_HX_WX_2_Data))]
		void Test32_VphadddV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VphadddV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 02 D3", 5, Code.VEX_Vphaddd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 02 D3", 5, Code.VEX_Vphaddd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VphadddV_VX_HX_WX_1_Data))]
		void Test64_VphadddV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VphadddV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 02 10", 5, Code.VEX_Vphaddd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E24D 02 10", 5, Code.VEX_Vphaddd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2C9 02 10", 5, Code.VEX_Vphaddd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2CD 02 10", 5, Code.VEX_Vphaddd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VphadddV_VX_HX_WX_2_Data))]
		void Test64_VphadddV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VphadddV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 02 D3", 5, Code.VEX_Vphaddd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 02 D3", 5, Code.VEX_Vphaddd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C46249 02 D3", 5, Code.VEX_Vphaddd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4624D 02 D3", 5, Code.VEX_Vphaddd_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E209 02 D3", 5, Code.VEX_Vphaddd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E20D 02 D3", 5, Code.VEX_Vphaddd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C249 02 D3", 5, Code.VEX_Vphaddd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C24D 02 D3", 5, Code.VEX_Vphaddd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PhaddswV_VX_WX_1_Data))]
		void Test16_PhaddswV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PhaddswV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3803 08", 4, Code.Phaddsw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0F3803 08", 5, Code.Phaddsw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PhaddswV_VX_WX_2_Data))]
		void Test16_PhaddswV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PhaddswV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3803 CD", 4, Code.Phaddsw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3803 CD", 5, Code.Phaddsw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PhaddswV_VX_WX_1_Data))]
		void Test32_PhaddswV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PhaddswV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3803 08", 4, Code.Phaddsw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0F3803 08", 5, Code.Phaddsw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PhaddswV_VX_WX_2_Data))]
		void Test32_PhaddswV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PhaddswV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3803 CD", 4, Code.Phaddsw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3803 CD", 5, Code.Phaddsw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PhaddswV_VX_WX_1_Data))]
		void Test64_PhaddswV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PhaddswV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3803 08", 4, Code.Phaddsw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0F3803 08", 5, Code.Phaddsw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PhaddswV_VX_WX_2_Data))]
		void Test64_PhaddswV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PhaddswV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3803 CD", 4, Code.Phaddsw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F3803 CD", 5, Code.Phaddsw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3803 CD", 5, Code.Phaddsw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3803 CD", 6, Code.Phaddsw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3803 CD", 6, Code.Phaddsw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3803 CD", 6, Code.Phaddsw_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VphaddswV_VX_HX_WX_1_Data))]
		void Test16_VphaddswV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VphaddswV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 03 10", 5, Code.VEX_Vphaddsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E24D 03 10", 5, Code.VEX_Vphaddsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E2C9 03 10", 5, Code.VEX_Vphaddsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E2CD 03 10", 5, Code.VEX_Vphaddsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VphaddswV_VX_HX_WX_2_Data))]
		void Test16_VphaddswV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VphaddswV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 03 D3", 5, Code.VEX_Vphaddsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 03 D3", 5, Code.VEX_Vphaddsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VphaddswV_VX_HX_WX_1_Data))]
		void Test32_VphaddswV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VphaddswV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 03 10", 5, Code.VEX_Vphaddsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E24D 03 10", 5, Code.VEX_Vphaddsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E2C9 03 10", 5, Code.VEX_Vphaddsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E2CD 03 10", 5, Code.VEX_Vphaddsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VphaddswV_VX_HX_WX_2_Data))]
		void Test32_VphaddswV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VphaddswV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 03 D3", 5, Code.VEX_Vphaddsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 03 D3", 5, Code.VEX_Vphaddsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VphaddswV_VX_HX_WX_1_Data))]
		void Test64_VphaddswV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VphaddswV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 03 10", 5, Code.VEX_Vphaddsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E24D 03 10", 5, Code.VEX_Vphaddsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E2C9 03 10", 5, Code.VEX_Vphaddsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E2CD 03 10", 5, Code.VEX_Vphaddsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VphaddswV_VX_HX_WX_2_Data))]
		void Test64_VphaddswV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VphaddswV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 03 D3", 5, Code.VEX_Vphaddsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 03 D3", 5, Code.VEX_Vphaddsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C46249 03 D3", 5, Code.VEX_Vphaddsw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4624D 03 D3", 5, Code.VEX_Vphaddsw_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E209 03 D3", 5, Code.VEX_Vphaddsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E20D 03 D3", 5, Code.VEX_Vphaddsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C249 03 D3", 5, Code.VEX_Vphaddsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C24D 03 D3", 5, Code.VEX_Vphaddsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmaddubswV_VX_WX_1_Data))]
		void Test16_PmaddubswV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PmaddubswV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3804 08", 4, Code.Pmaddubsw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int8 };

				yield return new object[] { "66 0F3804 08", 5, Code.Pmaddubsw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmaddubswV_VX_WX_2_Data))]
		void Test16_PmaddubswV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PmaddubswV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3804 CD", 4, Code.Pmaddubsw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3804 CD", 5, Code.Pmaddubsw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmaddubswV_VX_WX_1_Data))]
		void Test32_PmaddubswV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PmaddubswV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3804 08", 4, Code.Pmaddubsw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int8 };

				yield return new object[] { "66 0F3804 08", 5, Code.Pmaddubsw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmaddubswV_VX_WX_2_Data))]
		void Test32_PmaddubswV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PmaddubswV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3804 CD", 4, Code.Pmaddubsw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3804 CD", 5, Code.Pmaddubsw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmaddubswV_VX_WX_1_Data))]
		void Test64_PmaddubswV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PmaddubswV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3804 08", 4, Code.Pmaddubsw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int8 };

				yield return new object[] { "66 0F3804 08", 5, Code.Pmaddubsw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmaddubswV_VX_WX_2_Data))]
		void Test64_PmaddubswV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PmaddubswV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3804 CD", 4, Code.Pmaddubsw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F3804 CD", 5, Code.Pmaddubsw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3804 CD", 5, Code.Pmaddubsw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3804 CD", 6, Code.Pmaddubsw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3804 CD", 6, Code.Pmaddubsw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3804 CD", 6, Code.Pmaddubsw_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaddubswV_VX_HX_WX_1_Data))]
		void Test16_VpmaddubswV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmaddubswV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 04 10", 5, Code.VEX_Vpmaddubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E24D 04 10", 5, Code.VEX_Vpmaddubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E2C9 04 10", 5, Code.VEX_Vpmaddubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E2CD 04 10", 5, Code.VEX_Vpmaddubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaddubswV_VX_HX_WX_2_Data))]
		void Test16_VpmaddubswV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpmaddubswV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 04 D3", 5, Code.VEX_Vpmaddubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 04 D3", 5, Code.VEX_Vpmaddubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaddubswV_VX_HX_WX_1_Data))]
		void Test32_VpmaddubswV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmaddubswV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 04 10", 5, Code.VEX_Vpmaddubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E24D 04 10", 5, Code.VEX_Vpmaddubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E2C9 04 10", 5, Code.VEX_Vpmaddubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E2CD 04 10", 5, Code.VEX_Vpmaddubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaddubswV_VX_HX_WX_2_Data))]
		void Test32_VpmaddubswV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpmaddubswV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 04 D3", 5, Code.VEX_Vpmaddubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 04 D3", 5, Code.VEX_Vpmaddubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaddubswV_VX_HX_WX_1_Data))]
		void Test64_VpmaddubswV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmaddubswV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 04 10", 5, Code.VEX_Vpmaddubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E24D 04 10", 5, Code.VEX_Vpmaddubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
				yield return new object[] { "C4E2C9 04 10", 5, Code.VEX_Vpmaddubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int8 };
				yield return new object[] { "C4E2CD 04 10", 5, Code.VEX_Vpmaddubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaddubswV_VX_HX_WX_2_Data))]
		void Test64_VpmaddubswV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpmaddubswV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 04 D3", 5, Code.VEX_Vpmaddubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 04 D3", 5, Code.VEX_Vpmaddubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C46249 04 D3", 5, Code.VEX_Vpmaddubsw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4624D 04 D3", 5, Code.VEX_Vpmaddubsw_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E209 04 D3", 5, Code.VEX_Vpmaddubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E20D 04 D3", 5, Code.VEX_Vpmaddubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C249 04 D3", 5, Code.VEX_Vpmaddubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C24D 04 D3", 5, Code.VEX_Vpmaddubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaddubswV_VX_k1_HX_WX_1_Data))]
		void Test16_VpmaddubswV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpmaddubswV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 04 50 01", 7, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D8D 04 50 01", 7, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F24D08 04 50 01", 7, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F2CD0B 04 50 01", 7, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 04 50 01", 7, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DAD 04 50 01", 7, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F24D28 04 50 01", 7, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F2CD2B 04 50 01", 7, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 04 50 01", 7, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DCD 04 50 01", 7, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F24D48 04 50 01", 7, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F2CD4B 04 50 01", 7, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaddubswV_VX_k1_HX_WX_2_Data))]
		void Test16_VpmaddubswV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpmaddubswV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 04 D3", 6, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 04 D3", 6, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD0B 04 D3", 6, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 04 D3", 6, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DAB 04 D3", 6, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD2B 04 D3", 6, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 04 D3", 6, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DCB 04 D3", 6, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD4B 04 D3", 6, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaddubswV_VX_k1_HX_WX_1_Data))]
		void Test32_VpmaddubswV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpmaddubswV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 04 50 01", 7, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D8D 04 50 01", 7, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F24D08 04 50 01", 7, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F2CD0B 04 50 01", 7, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 04 50 01", 7, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DAD 04 50 01", 7, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F24D28 04 50 01", 7, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F2CD2B 04 50 01", 7, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 04 50 01", 7, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DCD 04 50 01", 7, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F24D48 04 50 01", 7, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F2CD4B 04 50 01", 7, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaddubswV_VX_k1_HX_WX_2_Data))]
		void Test32_VpmaddubswV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpmaddubswV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 04 D3", 6, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24D8B 04 D3", 6, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD0B 04 D3", 6, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 04 D3", 6, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DAB 04 D3", 6, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD2B 04 D3", 6, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 04 D3", 6, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F24DCB 04 D3", 6, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F2CD4B 04 D3", 6, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaddubswV_VX_k1_HX_WX_1_Data))]
		void Test64_VpmaddubswV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpmaddubswV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F24D0B 04 50 01", 7, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F24D8D 04 50 01", 7, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int8, 16, true };
				yield return new object[] { "62 F24D08 04 50 01", 7, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int8, 16, false };
				yield return new object[] { "62 F2CD0B 04 50 01", 7, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int8, 16, false };

				yield return new object[] { "62 F24D2B 04 50 01", 7, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F24DAD 04 50 01", 7, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int8, 32, true };
				yield return new object[] { "62 F24D28 04 50 01", 7, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int8, 32, false };
				yield return new object[] { "62 F2CD2B 04 50 01", 7, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int8, 32, false };

				yield return new object[] { "62 F24D4B 04 50 01", 7, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F24DCD 04 50 01", 7, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int8, 64, true };
				yield return new object[] { "62 F24D48 04 50 01", 7, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int8, 64, false };
				yield return new object[] { "62 F2CD4B 04 50 01", 7, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaddubswV_VX_k1_HX_WX_2_Data))]
		void Test64_VpmaddubswV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpmaddubswV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F24D0B 04 D3", 6, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20D8B 04 D3", 6, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D03 04 D3", 6, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D0B 04 D3", 6, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD0B 04 D3", 6, Code.EVEX_Vpmaddubsw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D2B 04 D3", 6, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20DAB 04 D3", 6, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D23 04 D3", 6, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D2B 04 D3", 6, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD2B 04 D3", 6, Code.EVEX_Vpmaddubsw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F24D4B 04 D3", 6, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E20DCB 04 D3", 6, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 124D43 04 D3", 6, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B24D4B 04 D3", 6, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F2CD4B 04 D3", 6, Code.EVEX_Vpmaddubsw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PhsubwV_VX_WX_1_Data))]
		void Test16_PhsubwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PhsubwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3805 08", 4, Code.Phsubw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0F3805 08", 5, Code.Phsubw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PhsubwV_VX_WX_2_Data))]
		void Test16_PhsubwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PhsubwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3805 CD", 4, Code.Phsubw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3805 CD", 5, Code.Phsubw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PhsubwV_VX_WX_1_Data))]
		void Test32_PhsubwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PhsubwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3805 08", 4, Code.Phsubw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0F3805 08", 5, Code.Phsubw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PhsubwV_VX_WX_2_Data))]
		void Test32_PhsubwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PhsubwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3805 CD", 4, Code.Phsubw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3805 CD", 5, Code.Phsubw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PhsubwV_VX_WX_1_Data))]
		void Test64_PhsubwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PhsubwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3805 08", 4, Code.Phsubw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0F3805 08", 5, Code.Phsubw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PhsubwV_VX_WX_2_Data))]
		void Test64_PhsubwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PhsubwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3805 CD", 4, Code.Phsubw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F3805 CD", 5, Code.Phsubw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3805 CD", 5, Code.Phsubw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3805 CD", 6, Code.Phsubw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3805 CD", 6, Code.Phsubw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3805 CD", 6, Code.Phsubw_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VphsubwV_VX_HX_WX_1_Data))]
		void Test16_VphsubwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VphsubwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 05 10", 5, Code.VEX_Vphsubw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E24D 05 10", 5, Code.VEX_Vphsubw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E2C9 05 10", 5, Code.VEX_Vphsubw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E2CD 05 10", 5, Code.VEX_Vphsubw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VphsubwV_VX_HX_WX_2_Data))]
		void Test16_VphsubwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VphsubwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 05 D3", 5, Code.VEX_Vphsubw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 05 D3", 5, Code.VEX_Vphsubw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VphsubwV_VX_HX_WX_1_Data))]
		void Test32_VphsubwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VphsubwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 05 10", 5, Code.VEX_Vphsubw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E24D 05 10", 5, Code.VEX_Vphsubw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E2C9 05 10", 5, Code.VEX_Vphsubw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E2CD 05 10", 5, Code.VEX_Vphsubw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VphsubwV_VX_HX_WX_2_Data))]
		void Test32_VphsubwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VphsubwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 05 D3", 5, Code.VEX_Vphsubw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 05 D3", 5, Code.VEX_Vphsubw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VphsubwV_VX_HX_WX_1_Data))]
		void Test64_VphsubwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VphsubwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 05 10", 5, Code.VEX_Vphsubw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E24D 05 10", 5, Code.VEX_Vphsubw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E2C9 05 10", 5, Code.VEX_Vphsubw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E2CD 05 10", 5, Code.VEX_Vphsubw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VphsubwV_VX_HX_WX_2_Data))]
		void Test64_VphsubwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VphsubwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 05 D3", 5, Code.VEX_Vphsubw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 05 D3", 5, Code.VEX_Vphsubw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C46249 05 D3", 5, Code.VEX_Vphsubw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4624D 05 D3", 5, Code.VEX_Vphsubw_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E209 05 D3", 5, Code.VEX_Vphsubw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E20D 05 D3", 5, Code.VEX_Vphsubw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C249 05 D3", 5, Code.VEX_Vphsubw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C24D 05 D3", 5, Code.VEX_Vphsubw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PhsubdV_VX_WX_1_Data))]
		void Test16_PhsubdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PhsubdV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3806 08", 4, Code.Phsubd_mm_mmm64, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0F3806 08", 5, Code.Phsubd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PhsubdV_VX_WX_2_Data))]
		void Test16_PhsubdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PhsubdV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3806 CD", 4, Code.Phsubd_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3806 CD", 5, Code.Phsubd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PhsubdV_VX_WX_1_Data))]
		void Test32_PhsubdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PhsubdV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3806 08", 4, Code.Phsubd_mm_mmm64, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0F3806 08", 5, Code.Phsubd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PhsubdV_VX_WX_2_Data))]
		void Test32_PhsubdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PhsubdV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3806 CD", 4, Code.Phsubd_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3806 CD", 5, Code.Phsubd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PhsubdV_VX_WX_1_Data))]
		void Test64_PhsubdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PhsubdV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3806 08", 4, Code.Phsubd_mm_mmm64, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0F3806 08", 5, Code.Phsubd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PhsubdV_VX_WX_2_Data))]
		void Test64_PhsubdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PhsubdV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3806 CD", 4, Code.Phsubd_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F3806 CD", 5, Code.Phsubd_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3806 CD", 5, Code.Phsubd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3806 CD", 6, Code.Phsubd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3806 CD", 6, Code.Phsubd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3806 CD", 6, Code.Phsubd_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VphsubdV_VX_HX_WX_1_Data))]
		void Test16_VphsubdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VphsubdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 06 10", 5, Code.VEX_Vphsubd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E24D 06 10", 5, Code.VEX_Vphsubd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2C9 06 10", 5, Code.VEX_Vphsubd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2CD 06 10", 5, Code.VEX_Vphsubd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VphsubdV_VX_HX_WX_2_Data))]
		void Test16_VphsubdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VphsubdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 06 D3", 5, Code.VEX_Vphsubd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 06 D3", 5, Code.VEX_Vphsubd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VphsubdV_VX_HX_WX_1_Data))]
		void Test32_VphsubdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VphsubdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 06 10", 5, Code.VEX_Vphsubd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E24D 06 10", 5, Code.VEX_Vphsubd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2C9 06 10", 5, Code.VEX_Vphsubd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2CD 06 10", 5, Code.VEX_Vphsubd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VphsubdV_VX_HX_WX_2_Data))]
		void Test32_VphsubdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VphsubdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 06 D3", 5, Code.VEX_Vphsubd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 06 D3", 5, Code.VEX_Vphsubd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VphsubdV_VX_HX_WX_1_Data))]
		void Test64_VphsubdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VphsubdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 06 10", 5, Code.VEX_Vphsubd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E24D 06 10", 5, Code.VEX_Vphsubd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E2C9 06 10", 5, Code.VEX_Vphsubd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E2CD 06 10", 5, Code.VEX_Vphsubd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VphsubdV_VX_HX_WX_2_Data))]
		void Test64_VphsubdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VphsubdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 06 D3", 5, Code.VEX_Vphsubd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 06 D3", 5, Code.VEX_Vphsubd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C46249 06 D3", 5, Code.VEX_Vphsubd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4624D 06 D3", 5, Code.VEX_Vphsubd_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E209 06 D3", 5, Code.VEX_Vphsubd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E20D 06 D3", 5, Code.VEX_Vphsubd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C249 06 D3", 5, Code.VEX_Vphsubd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C24D 06 D3", 5, Code.VEX_Vphsubd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PhsubswV_VX_WX_1_Data))]
		void Test16_PhsubswV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PhsubswV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3807 08", 4, Code.Phsubsw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0F3807 08", 5, Code.Phsubsw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PhsubswV_VX_WX_2_Data))]
		void Test16_PhsubswV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PhsubswV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3807 CD", 4, Code.Phsubsw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3807 CD", 5, Code.Phsubsw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PhsubswV_VX_WX_1_Data))]
		void Test32_PhsubswV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PhsubswV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3807 08", 4, Code.Phsubsw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0F3807 08", 5, Code.Phsubsw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PhsubswV_VX_WX_2_Data))]
		void Test32_PhsubswV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PhsubswV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3807 CD", 4, Code.Phsubsw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3807 CD", 5, Code.Phsubsw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PhsubswV_VX_WX_1_Data))]
		void Test64_PhsubswV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PhsubswV_VX_WX_1_Data {
			get {
				yield return new object[] { "0F3807 08", 4, Code.Phsubsw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0F3807 08", 5, Code.Phsubsw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PhsubswV_VX_WX_2_Data))]
		void Test64_PhsubswV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PhsubswV_VX_WX_2_Data {
			get {
				yield return new object[] { "0F3807 CD", 4, Code.Phsubsw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0F3807 CD", 5, Code.Phsubsw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0F3807 CD", 5, Code.Phsubsw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0F3807 CD", 6, Code.Phsubsw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0F3807 CD", 6, Code.Phsubsw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0F3807 CD", 6, Code.Phsubsw_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VphsubswV_VX_HX_WX_1_Data))]
		void Test16_VphsubswV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VphsubswV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 07 10", 5, Code.VEX_Vphsubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E24D 07 10", 5, Code.VEX_Vphsubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E2C9 07 10", 5, Code.VEX_Vphsubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E2CD 07 10", 5, Code.VEX_Vphsubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VphsubswV_VX_HX_WX_2_Data))]
		void Test16_VphsubswV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VphsubswV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 07 D3", 5, Code.VEX_Vphsubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 07 D3", 5, Code.VEX_Vphsubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VphsubswV_VX_HX_WX_1_Data))]
		void Test32_VphsubswV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VphsubswV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 07 10", 5, Code.VEX_Vphsubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E24D 07 10", 5, Code.VEX_Vphsubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E2C9 07 10", 5, Code.VEX_Vphsubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E2CD 07 10", 5, Code.VEX_Vphsubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VphsubswV_VX_HX_WX_2_Data))]
		void Test32_VphsubswV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VphsubswV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 07 D3", 5, Code.VEX_Vphsubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 07 D3", 5, Code.VEX_Vphsubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VphsubswV_VX_HX_WX_1_Data))]
		void Test64_VphsubswV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VphsubswV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C4E249 07 10", 5, Code.VEX_Vphsubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E24D 07 10", 5, Code.VEX_Vphsubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E2C9 07 10", 5, Code.VEX_Vphsubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E2CD 07 10", 5, Code.VEX_Vphsubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VphsubswV_VX_HX_WX_2_Data))]
		void Test64_VphsubswV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VphsubswV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C4E249 07 D3", 5, Code.VEX_Vphsubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4E24D 07 D3", 5, Code.VEX_Vphsubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C46249 07 D3", 5, Code.VEX_Vphsubsw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C4624D 07 D3", 5, Code.VEX_Vphsubsw_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C4E209 07 D3", 5, Code.VEX_Vphsubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C4E20D 07 D3", 5, Code.VEX_Vphsubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C249 07 D3", 5, Code.VEX_Vphsubsw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C24D 07 D3", 5, Code.VEX_Vphsubsw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}
	}
}
