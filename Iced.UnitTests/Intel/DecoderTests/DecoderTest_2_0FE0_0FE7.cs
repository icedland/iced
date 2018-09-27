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
	public sealed class DecoderTest_2_0FE0_0FE7 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_PavgbV_VX_WX_1_Data))]
		void Test16_PavgbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_PavgbV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FE0 08", 3, Code.Pavgb_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "66 0FE0 08", 4, Code.Pavgb_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PavgbV_VX_WX_2_Data))]
		void Test16_PavgbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_PavgbV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FE0 CD", 3, Code.Pavgb_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FE0 CD", 4, Code.Pavgb_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PavgbV_VX_WX_1_Data))]
		void Test32_PavgbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_PavgbV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FE0 08", 3, Code.Pavgb_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "66 0FE0 08", 4, Code.Pavgb_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PavgbV_VX_WX_2_Data))]
		void Test32_PavgbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_PavgbV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FE0 CD", 3, Code.Pavgb_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FE0 CD", 4, Code.Pavgb_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PavgbV_VX_WX_1_Data))]
		void Test64_PavgbV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_PavgbV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FE0 08", 3, Code.Pavgb_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "66 0FE0 08", 4, Code.Pavgb_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PavgbV_VX_WX_2_Data))]
		void Test64_PavgbV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_PavgbV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FE0 CD", 3, Code.Pavgb_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FE0 CD", 4, Code.Pavgb_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FE0 CD", 4, Code.Pavgb_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FE0 CD", 5, Code.Pavgb_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FE0 CD", 5, Code.Pavgb_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FE0 CD", 5, Code.Pavgb_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpavgbV_VX_HX_WX_1_Data))]
		void Test16_VpavgbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpavgbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 E0 10", 4, Code.VEX_Vpavgb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C5CD E0 10", 4, Code.VEX_Vpavgb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
				yield return new object[] { "C4E1C9 E0 10", 5, Code.VEX_Vpavgb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E1CD E0 10", 5, Code.VEX_Vpavgb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpavgbV_VX_HX_WX_2_Data))]
		void Test16_VpavgbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_VpavgbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 E0 D3", 4, Code.VEX_Vpavgb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD E0 D3", 4, Code.VEX_Vpavgb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpavgbV_VX_HX_WX_1_Data))]
		void Test32_VpavgbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpavgbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 E0 10", 4, Code.VEX_Vpavgb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C5CD E0 10", 4, Code.VEX_Vpavgb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
				yield return new object[] { "C4E1C9 E0 10", 5, Code.VEX_Vpavgb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E1CD E0 10", 5, Code.VEX_Vpavgb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpavgbV_VX_HX_WX_2_Data))]
		void Test32_VpavgbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_VpavgbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 E0 D3", 4, Code.VEX_Vpavgb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD E0 D3", 4, Code.VEX_Vpavgb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpavgbV_VX_HX_WX_1_Data))]
		void Test64_VpavgbV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpavgbV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 E0 10", 4, Code.VEX_Vpavgb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C5CD E0 10", 4, Code.VEX_Vpavgb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
				yield return new object[] { "C4E1C9 E0 10", 5, Code.VEX_Vpavgb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E1CD E0 10", 5, Code.VEX_Vpavgb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpavgbV_VX_HX_WX_2_Data))]
		void Test64_VpavgbV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_VpavgbV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 E0 D3", 4, Code.VEX_Vpavgb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD E0 D3", 4, Code.VEX_Vpavgb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 E0 D3", 4, Code.VEX_Vpavgb_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D E0 D3", 4, Code.VEX_Vpavgb_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 E0 D3", 4, Code.VEX_Vpavgb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D E0 D3", 4, Code.VEX_Vpavgb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 E0 D3", 5, Code.VEX_Vpavgb_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D E0 D3", 5, Code.VEX_Vpavgb_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpavgbV_VX_k1_HX_WX_1_Data))]
		void Test16_VpavgbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpavgbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B E0 50 01", 7, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F14D8D E0 50 01", 7, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F14D08 E0 50 01", 7, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F1CD0B E0 50 01", 7, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F14D2B E0 50 01", 7, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F14DAD E0 50 01", 7, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F14D28 E0 50 01", 7, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F1CD2B E0 50 01", 7, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F14D4B E0 50 01", 7, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F14DCD E0 50 01", 7, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true };
				yield return new object[] { "62 F14D48 E0 50 01", 7, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F1CD4B E0 50 01", 7, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpavgbV_VX_k1_HX_WX_2_Data))]
		void Test16_VpavgbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpavgbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B E0 D3", 6, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B E0 D3", 6, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B E0 D3", 6, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B E0 D3", 6, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB E0 D3", 6, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B E0 D3", 6, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B E0 D3", 6, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB E0 D3", 6, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B E0 D3", 6, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpavgbV_VX_k1_HX_WX_1_Data))]
		void Test32_VpavgbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpavgbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B E0 50 01", 7, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F14D8D E0 50 01", 7, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F14D08 E0 50 01", 7, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F1CD0B E0 50 01", 7, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F14D2B E0 50 01", 7, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F14DAD E0 50 01", 7, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F14D28 E0 50 01", 7, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F1CD2B E0 50 01", 7, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F14D4B E0 50 01", 7, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F14DCD E0 50 01", 7, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true };
				yield return new object[] { "62 F14D48 E0 50 01", 7, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F1CD4B E0 50 01", 7, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpavgbV_VX_k1_HX_WX_2_Data))]
		void Test32_VpavgbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpavgbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B E0 D3", 6, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B E0 D3", 6, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B E0 D3", 6, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B E0 D3", 6, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB E0 D3", 6, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B E0 D3", 6, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B E0 D3", 6, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB E0 D3", 6, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B E0 D3", 6, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpavgbV_VX_k1_HX_WX_1_Data))]
		void Test64_VpavgbV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpavgbV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B E0 50 01", 7, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F14D8D E0 50 01", 7, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true };
				yield return new object[] { "62 F14D08 E0 50 01", 7, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };
				yield return new object[] { "62 F1CD0B E0 50 01", 7, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F14D2B E0 50 01", 7, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F14DAD E0 50 01", 7, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true };
				yield return new object[] { "62 F14D28 E0 50 01", 7, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };
				yield return new object[] { "62 F1CD2B E0 50 01", 7, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F14D4B E0 50 01", 7, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F14DCD E0 50 01", 7, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true };
				yield return new object[] { "62 F14D48 E0 50 01", 7, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
				yield return new object[] { "62 F1CD4B E0 50 01", 7, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpavgbV_VX_k1_HX_WX_2_Data))]
		void Test64_VpavgbV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpavgbV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B E0 D3", 6, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B E0 D3", 6, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 E0 D3", 6, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B E0 D3", 6, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B E0 D3", 6, Code.EVEX_Vpavgb_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B E0 D3", 6, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB E0 D3", 6, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 E0 D3", 6, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B E0 D3", 6, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B E0 D3", 6, Code.EVEX_Vpavgb_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B E0 D3", 6, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB E0 D3", 6, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 E0 D3", 6, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B E0 D3", 6, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B E0 D3", 6, Code.EVEX_Vpavgb_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsrawV_VX_WX_1_Data))]
		void Test16_PsrawV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_PsrawV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FE1 08", 3, Code.Psraw_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FE1 08", 4, Code.Psraw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsrawV_VX_WX_2_Data))]
		void Test16_PsrawV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_PsrawV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FE1 CD", 3, Code.Psraw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FE1 CD", 4, Code.Psraw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsrawV_VX_WX_1_Data))]
		void Test32_PsrawV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_PsrawV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FE1 08", 3, Code.Psraw_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FE1 08", 4, Code.Psraw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsrawV_VX_WX_2_Data))]
		void Test32_PsrawV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_PsrawV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FE1 CD", 3, Code.Psraw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FE1 CD", 4, Code.Psraw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsrawV_VX_WX_1_Data))]
		void Test64_PsrawV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_PsrawV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FE1 08", 3, Code.Psraw_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FE1 08", 4, Code.Psraw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsrawV_VX_WX_2_Data))]
		void Test64_PsrawV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_PsrawV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FE1 CD", 3, Code.Psraw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FE1 CD", 4, Code.Psraw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FE1 CD", 4, Code.Psraw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FE1 CD", 5, Code.Psraw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FE1 CD", 5, Code.Psraw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FE1 CD", 5, Code.Psraw_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrawV_VX_HX_WX_1_Data))]
		void Test16_VpsrawV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpsrawV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 E1 10", 4, Code.VEX_Vpsraw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD E1 10", 4, Code.VEX_Vpsraw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 E1 10", 5, Code.VEX_Vpsraw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD E1 10", 5, Code.VEX_Vpsraw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrawV_VX_HX_WX_2_Data))]
		void Test16_VpsrawV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_VpsrawV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 E1 D3", 4, Code.VEX_Vpsraw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD E1 D3", 4, Code.VEX_Vpsraw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrawV_VX_HX_WX_1_Data))]
		void Test32_VpsrawV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpsrawV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 E1 10", 4, Code.VEX_Vpsraw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD E1 10", 4, Code.VEX_Vpsraw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 E1 10", 5, Code.VEX_Vpsraw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD E1 10", 5, Code.VEX_Vpsraw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrawV_VX_HX_WX_2_Data))]
		void Test32_VpsrawV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_VpsrawV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 E1 D3", 4, Code.VEX_Vpsraw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD E1 D3", 4, Code.VEX_Vpsraw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrawV_VX_HX_WX_1_Data))]
		void Test64_VpsrawV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpsrawV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 E1 10", 4, Code.VEX_Vpsraw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD E1 10", 4, Code.VEX_Vpsraw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 E1 10", 5, Code.VEX_Vpsraw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD E1 10", 5, Code.VEX_Vpsraw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrawV_VX_HX_WX_2_Data))]
		void Test64_VpsrawV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_VpsrawV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 E1 D3", 4, Code.VEX_Vpsraw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD E1 D3", 4, Code.VEX_Vpsraw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
				yield return new object[] { "C549 E1 D3", 4, Code.VEX_Vpsraw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D E1 D3", 4, Code.VEX_Vpsraw_ymm_ymm_xmmm128, Register.YMM10, Register.YMM6, Register.XMM3 };
				yield return new object[] { "C589 E1 D3", 4, Code.VEX_Vpsraw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D E1 D3", 4, Code.VEX_Vpsraw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM14, Register.XMM3 };
				yield return new object[] { "C4C149 E1 D3", 5, Code.VEX_Vpsraw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D E1 D3", 5, Code.VEX_Vpsraw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrawV_VX_k1_HX_WX_1_Data))]
		void Test16_VpsrawV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpsrawV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B E1 50 01", 7, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14D8D E1 50 01", 7, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D08 E1 50 01", 7, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD0B E1 50 01", 7, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D2B E1 50 01", 7, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DAD E1 50 01", 7, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D28 E1 50 01", 7, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD2B E1 50 01", 7, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D4B E1 50 01", 7, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DCD E1 50 01", 7, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D48 E1 50 01", 7, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD4B E1 50 01", 7, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsrawV_VX_k1_HX_WX_2_Data))]
		void Test16_VpsrawV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpsrawV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B E1 D3", 6, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B E1 D3", 6, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B E1 D3", 6, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B E1 D3", 6, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB E1 D3", 6, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B E1 D3", 6, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B E1 D3", 6, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB E1 D3", 6, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B E1 D3", 6, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrawV_VX_k1_HX_WX_1_Data))]
		void Test32_VpsrawV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpsrawV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B E1 50 01", 7, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14D8D E1 50 01", 7, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D08 E1 50 01", 7, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD0B E1 50 01", 7, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D2B E1 50 01", 7, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DAD E1 50 01", 7, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D28 E1 50 01", 7, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD2B E1 50 01", 7, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D4B E1 50 01", 7, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DCD E1 50 01", 7, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D48 E1 50 01", 7, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD4B E1 50 01", 7, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsrawV_VX_k1_HX_WX_2_Data))]
		void Test32_VpsrawV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpsrawV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B E1 D3", 6, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B E1 D3", 6, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B E1 D3", 6, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B E1 D3", 6, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB E1 D3", 6, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B E1 D3", 6, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B E1 D3", 6, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB E1 D3", 6, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B E1 D3", 6, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrawV_VX_k1_HX_WX_1_Data))]
		void Test64_VpsrawV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpsrawV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B E1 50 01", 7, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14D8D E1 50 01", 7, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D08 E1 50 01", 7, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD0B E1 50 01", 7, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D2B E1 50 01", 7, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DAD E1 50 01", 7, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D28 E1 50 01", 7, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD2B E1 50 01", 7, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D4B E1 50 01", 7, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DCD E1 50 01", 7, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D48 E1 50 01", 7, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD4B E1 50 01", 7, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsrawV_VX_k1_HX_WX_2_Data))]
		void Test64_VpsrawV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpsrawV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B E1 D3", 6, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B E1 D3", 6, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 E1 D3", 6, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B E1 D3", 6, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B E1 D3", 6, Code.EVEX_Vpsraw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B E1 D3", 6, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB E1 D3", 6, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM18, Register.YMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 E1 D3", 6, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM10, Register.YMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B E1 D3", 6, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B E1 D3", 6, Code.EVEX_Vpsraw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B E1 D3", 6, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB E1 D3", 6, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM18, Register.ZMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 E1 D3", 6, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM10, Register.ZMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B E1 D3", 6, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B E1 D3", 6, Code.EVEX_Vpsraw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsradV_VX_WX_1_Data))]
		void Test16_PsradV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_PsradV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FE2 08", 3, Code.Psrad_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FE2 08", 4, Code.Psrad_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsradV_VX_WX_2_Data))]
		void Test16_PsradV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_PsradV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FE2 CD", 3, Code.Psrad_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FE2 CD", 4, Code.Psrad_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsradV_VX_WX_1_Data))]
		void Test32_PsradV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_PsradV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FE2 08", 3, Code.Psrad_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FE2 08", 4, Code.Psrad_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsradV_VX_WX_2_Data))]
		void Test32_PsradV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_PsradV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FE2 CD", 3, Code.Psrad_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FE2 CD", 4, Code.Psrad_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsradV_VX_WX_1_Data))]
		void Test64_PsradV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_PsradV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FE2 08", 3, Code.Psrad_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FE2 08", 4, Code.Psrad_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsradV_VX_WX_2_Data))]
		void Test64_PsradV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_PsradV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FE2 CD", 3, Code.Psrad_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FE2 CD", 4, Code.Psrad_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FE2 CD", 4, Code.Psrad_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FE2 CD", 5, Code.Psrad_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FE2 CD", 5, Code.Psrad_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FE2 CD", 5, Code.Psrad_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsradV_VX_HX_WX_1_Data))]
		void Test16_VpsradV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpsradV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 E2 10", 4, Code.VEX_Vpsrad_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD E2 10", 4, Code.VEX_Vpsrad_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 E2 10", 5, Code.VEX_Vpsrad_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD E2 10", 5, Code.VEX_Vpsrad_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsradV_VX_HX_WX_2_Data))]
		void Test16_VpsradV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_VpsradV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 E2 D3", 4, Code.VEX_Vpsrad_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD E2 D3", 4, Code.VEX_Vpsrad_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsradV_VX_HX_WX_1_Data))]
		void Test32_VpsradV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpsradV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 E2 10", 4, Code.VEX_Vpsrad_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD E2 10", 4, Code.VEX_Vpsrad_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 E2 10", 5, Code.VEX_Vpsrad_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD E2 10", 5, Code.VEX_Vpsrad_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsradV_VX_HX_WX_2_Data))]
		void Test32_VpsradV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_VpsradV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 E2 D3", 4, Code.VEX_Vpsrad_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD E2 D3", 4, Code.VEX_Vpsrad_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsradV_VX_HX_WX_1_Data))]
		void Test64_VpsradV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpsradV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 E2 10", 4, Code.VEX_Vpsrad_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD E2 10", 4, Code.VEX_Vpsrad_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 E2 10", 5, Code.VEX_Vpsrad_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD E2 10", 5, Code.VEX_Vpsrad_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsradV_VX_HX_WX_2_Data))]
		void Test64_VpsradV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_VpsradV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 E2 D3", 4, Code.VEX_Vpsrad_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD E2 D3", 4, Code.VEX_Vpsrad_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
				yield return new object[] { "C549 E2 D3", 4, Code.VEX_Vpsrad_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D E2 D3", 4, Code.VEX_Vpsrad_ymm_ymm_xmmm128, Register.YMM10, Register.YMM6, Register.XMM3 };
				yield return new object[] { "C589 E2 D3", 4, Code.VEX_Vpsrad_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D E2 D3", 4, Code.VEX_Vpsrad_ymm_ymm_xmmm128, Register.YMM2, Register.YMM14, Register.XMM3 };
				yield return new object[] { "C4C149 E2 D3", 5, Code.VEX_Vpsrad_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D E2 D3", 5, Code.VEX_Vpsrad_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsradV_VX_k1_HX_WX_1_Data))]
		void Test16_VpsradV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpsradV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B E2 50 01", 7, Code.EVEX_Vpsrad_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14D8D E2 50 01", 7, Code.EVEX_Vpsrad_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D08 E2 50 01", 7, Code.EVEX_Vpsrad_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D2B E2 50 01", 7, Code.EVEX_Vpsrad_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DAD E2 50 01", 7, Code.EVEX_Vpsrad_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D28 E2 50 01", 7, Code.EVEX_Vpsrad_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D4B E2 50 01", 7, Code.EVEX_Vpsrad_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DCD E2 50 01", 7, Code.EVEX_Vpsrad_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D48 E2 50 01", 7, Code.EVEX_Vpsrad_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD0B E2 50 01", 7, Code.EVEX_Vpsraq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD8D E2 50 01", 7, Code.EVEX_Vpsraq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD08 E2 50 01", 7, Code.EVEX_Vpsraq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD2B E2 50 01", 7, Code.EVEX_Vpsraq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CDAD E2 50 01", 7, Code.EVEX_Vpsraq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD28 E2 50 01", 7, Code.EVEX_Vpsraq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD4B E2 50 01", 7, Code.EVEX_Vpsraq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CDCD E2 50 01", 7, Code.EVEX_Vpsraq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD48 E2 50 01", 7, Code.EVEX_Vpsraq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsradV_VX_k1_HX_WX_2_Data))]
		void Test16_VpsradV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpsradV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B E2 D3", 6, Code.EVEX_Vpsrad_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B E2 D3", 6, Code.EVEX_Vpsrad_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F14D2B E2 D3", 6, Code.EVEX_Vpsrad_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB E2 D3", 6, Code.EVEX_Vpsrad_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F14D4B E2 D3", 6, Code.EVEX_Vpsrad_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB E2 D3", 6, Code.EVEX_Vpsrad_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1CD0B E2 D3", 6, Code.EVEX_Vpsraq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD8B E2 D3", 6, Code.EVEX_Vpsraq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1CD2B E2 D3", 6, Code.EVEX_Vpsraq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CDAB E2 D3", 6, Code.EVEX_Vpsraq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1CD4B E2 D3", 6, Code.EVEX_Vpsraq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CDCB E2 D3", 6, Code.EVEX_Vpsraq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsradV_VX_k1_HX_WX_1_Data))]
		void Test32_VpsradV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpsradV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B E2 50 01", 7, Code.EVEX_Vpsrad_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14D8D E2 50 01", 7, Code.EVEX_Vpsrad_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D08 E2 50 01", 7, Code.EVEX_Vpsrad_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D2B E2 50 01", 7, Code.EVEX_Vpsrad_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DAD E2 50 01", 7, Code.EVEX_Vpsrad_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D28 E2 50 01", 7, Code.EVEX_Vpsrad_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D4B E2 50 01", 7, Code.EVEX_Vpsrad_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DCD E2 50 01", 7, Code.EVEX_Vpsrad_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D48 E2 50 01", 7, Code.EVEX_Vpsrad_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD0B E2 50 01", 7, Code.EVEX_Vpsraq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD8D E2 50 01", 7, Code.EVEX_Vpsraq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD08 E2 50 01", 7, Code.EVEX_Vpsraq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD2B E2 50 01", 7, Code.EVEX_Vpsraq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CDAD E2 50 01", 7, Code.EVEX_Vpsraq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD28 E2 50 01", 7, Code.EVEX_Vpsraq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD4B E2 50 01", 7, Code.EVEX_Vpsraq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CDCD E2 50 01", 7, Code.EVEX_Vpsraq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD48 E2 50 01", 7, Code.EVEX_Vpsraq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsradV_VX_k1_HX_WX_2_Data))]
		void Test32_VpsradV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpsradV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B E2 D3", 6, Code.EVEX_Vpsrad_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B E2 D3", 6, Code.EVEX_Vpsrad_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F14D2B E2 D3", 6, Code.EVEX_Vpsrad_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB E2 D3", 6, Code.EVEX_Vpsrad_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F14D4B E2 D3", 6, Code.EVEX_Vpsrad_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB E2 D3", 6, Code.EVEX_Vpsrad_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1CD0B E2 D3", 6, Code.EVEX_Vpsraq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD8B E2 D3", 6, Code.EVEX_Vpsraq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1CD2B E2 D3", 6, Code.EVEX_Vpsraq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CDAB E2 D3", 6, Code.EVEX_Vpsraq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1CD4B E2 D3", 6, Code.EVEX_Vpsraq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CDCB E2 D3", 6, Code.EVEX_Vpsraq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsradV_VX_k1_HX_WX_1_Data))]
		void Test64_VpsradV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpsradV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B E2 50 01", 7, Code.EVEX_Vpsrad_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14D8D E2 50 01", 7, Code.EVEX_Vpsrad_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D08 E2 50 01", 7, Code.EVEX_Vpsrad_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D2B E2 50 01", 7, Code.EVEX_Vpsrad_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DAD E2 50 01", 7, Code.EVEX_Vpsrad_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D28 E2 50 01", 7, Code.EVEX_Vpsrad_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D4B E2 50 01", 7, Code.EVEX_Vpsrad_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DCD E2 50 01", 7, Code.EVEX_Vpsrad_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D48 E2 50 01", 7, Code.EVEX_Vpsrad_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD0B E2 50 01", 7, Code.EVEX_Vpsraq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD8D E2 50 01", 7, Code.EVEX_Vpsraq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD08 E2 50 01", 7, Code.EVEX_Vpsraq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD2B E2 50 01", 7, Code.EVEX_Vpsraq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CDAD E2 50 01", 7, Code.EVEX_Vpsraq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD28 E2 50 01", 7, Code.EVEX_Vpsraq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD4B E2 50 01", 7, Code.EVEX_Vpsraq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CDCD E2 50 01", 7, Code.EVEX_Vpsraq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD48 E2 50 01", 7, Code.EVEX_Vpsraq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsradV_VX_k1_HX_WX_2_Data))]
		void Test64_VpsradV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpsradV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B E2 D3", 6, Code.EVEX_Vpsrad_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B E2 D3", 6, Code.EVEX_Vpsrad_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 E2 D3", 6, Code.EVEX_Vpsrad_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B E2 D3", 6, Code.EVEX_Vpsrad_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B E2 D3", 6, Code.EVEX_Vpsrad_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB E2 D3", 6, Code.EVEX_Vpsrad_ymm_k1z_ymm_xmmm128, Register.YMM18, Register.YMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 E2 D3", 6, Code.EVEX_Vpsrad_ymm_k1z_ymm_xmmm128, Register.YMM10, Register.YMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B E2 D3", 6, Code.EVEX_Vpsrad_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B E2 D3", 6, Code.EVEX_Vpsrad_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB E2 D3", 6, Code.EVEX_Vpsrad_zmm_k1z_zmm_xmmm128, Register.ZMM18, Register.ZMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 E2 D3", 6, Code.EVEX_Vpsrad_zmm_k1z_zmm_xmmm128, Register.ZMM10, Register.ZMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B E2 D3", 6, Code.EVEX_Vpsrad_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F1CD0B E2 D3", 6, Code.EVEX_Vpsraq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E18D8B E2 D3", 6, Code.EVEX_Vpsraq_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 11CD03 E2 D3", 6, Code.EVEX_Vpsraq_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B1CD0B E2 D3", 6, Code.EVEX_Vpsraq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F1CD2B E2 D3", 6, Code.EVEX_Vpsraq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E18DAB E2 D3", 6, Code.EVEX_Vpsraq_ymm_k1z_ymm_xmmm128, Register.YMM18, Register.YMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 11CD23 E2 D3", 6, Code.EVEX_Vpsraq_ymm_k1z_ymm_xmmm128, Register.YMM10, Register.YMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B1CD2B E2 D3", 6, Code.EVEX_Vpsraq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F1CD4B E2 D3", 6, Code.EVEX_Vpsraq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E18DCB E2 D3", 6, Code.EVEX_Vpsraq_zmm_k1z_zmm_xmmm128, Register.ZMM18, Register.ZMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 11CD43 E2 D3", 6, Code.EVEX_Vpsraq_zmm_k1z_zmm_xmmm128, Register.ZMM10, Register.ZMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B1CD4B E2 D3", 6, Code.EVEX_Vpsraq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PavgwV_VX_WX_1_Data))]
		void Test16_PavgwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_PavgwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FE3 08", 3, Code.Pavgw_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt16 };

				yield return new object[] { "66 0FE3 08", 4, Code.Pavgw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PavgwV_VX_WX_2_Data))]
		void Test16_PavgwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_PavgwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FE3 CD", 3, Code.Pavgw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FE3 CD", 4, Code.Pavgw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PavgwV_VX_WX_1_Data))]
		void Test32_PavgwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_PavgwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FE3 08", 3, Code.Pavgw_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt16 };

				yield return new object[] { "66 0FE3 08", 4, Code.Pavgw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PavgwV_VX_WX_2_Data))]
		void Test32_PavgwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_PavgwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FE3 CD", 3, Code.Pavgw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FE3 CD", 4, Code.Pavgw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PavgwV_VX_WX_1_Data))]
		void Test64_PavgwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_PavgwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FE3 08", 3, Code.Pavgw_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt16 };

				yield return new object[] { "66 0FE3 08", 4, Code.Pavgw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PavgwV_VX_WX_2_Data))]
		void Test64_PavgwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_PavgwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FE3 CD", 3, Code.Pavgw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FE3 CD", 4, Code.Pavgw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FE3 CD", 4, Code.Pavgw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FE3 CD", 5, Code.Pavgw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FE3 CD", 5, Code.Pavgw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FE3 CD", 5, Code.Pavgw_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpavgwV_VX_HX_WX_1_Data))]
		void Test16_VpavgwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpavgwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 E3 10", 4, Code.VEX_Vpavgw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C5CD E3 10", 4, Code.VEX_Vpavgw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
				yield return new object[] { "C4E1C9 E3 10", 5, Code.VEX_Vpavgw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E1CD E3 10", 5, Code.VEX_Vpavgw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpavgwV_VX_HX_WX_2_Data))]
		void Test16_VpavgwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_VpavgwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 E3 D3", 4, Code.VEX_Vpavgw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD E3 D3", 4, Code.VEX_Vpavgw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpavgwV_VX_HX_WX_1_Data))]
		void Test32_VpavgwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpavgwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 E3 10", 4, Code.VEX_Vpavgw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C5CD E3 10", 4, Code.VEX_Vpavgw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
				yield return new object[] { "C4E1C9 E3 10", 5, Code.VEX_Vpavgw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E1CD E3 10", 5, Code.VEX_Vpavgw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpavgwV_VX_HX_WX_2_Data))]
		void Test32_VpavgwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_VpavgwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 E3 D3", 4, Code.VEX_Vpavgw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD E3 D3", 4, Code.VEX_Vpavgw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpavgwV_VX_HX_WX_1_Data))]
		void Test64_VpavgwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpavgwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 E3 10", 4, Code.VEX_Vpavgw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C5CD E3 10", 4, Code.VEX_Vpavgw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
				yield return new object[] { "C4E1C9 E3 10", 5, Code.VEX_Vpavgw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E1CD E3 10", 5, Code.VEX_Vpavgw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpavgwV_VX_HX_WX_2_Data))]
		void Test64_VpavgwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_VpavgwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 E3 D3", 4, Code.VEX_Vpavgw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD E3 D3", 4, Code.VEX_Vpavgw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 E3 D3", 4, Code.VEX_Vpavgw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D E3 D3", 4, Code.VEX_Vpavgw_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 E3 D3", 4, Code.VEX_Vpavgw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D E3 D3", 4, Code.VEX_Vpavgw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 E3 D3", 5, Code.VEX_Vpavgw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D E3 D3", 5, Code.VEX_Vpavgw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpavgwV_VX_k1_HX_WX_1_Data))]
		void Test16_VpavgwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpavgwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B E3 50 01", 7, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F14D8D E3 50 01", 7, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F14D08 E3 50 01", 7, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F1CD0B E3 50 01", 7, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F14D2B E3 50 01", 7, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F14DAD E3 50 01", 7, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F14D28 E3 50 01", 7, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F1CD2B E3 50 01", 7, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F14D4B E3 50 01", 7, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F14DCD E3 50 01", 7, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F14D48 E3 50 01", 7, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F1CD4B E3 50 01", 7, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpavgwV_VX_k1_HX_WX_2_Data))]
		void Test16_VpavgwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpavgwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B E3 D3", 6, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B E3 D3", 6, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B E3 D3", 6, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B E3 D3", 6, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB E3 D3", 6, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B E3 D3", 6, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B E3 D3", 6, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB E3 D3", 6, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B E3 D3", 6, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpavgwV_VX_k1_HX_WX_1_Data))]
		void Test32_VpavgwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpavgwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B E3 50 01", 7, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F14D8D E3 50 01", 7, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F14D08 E3 50 01", 7, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F1CD0B E3 50 01", 7, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F14D2B E3 50 01", 7, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F14DAD E3 50 01", 7, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F14D28 E3 50 01", 7, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F1CD2B E3 50 01", 7, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F14D4B E3 50 01", 7, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F14DCD E3 50 01", 7, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F14D48 E3 50 01", 7, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F1CD4B E3 50 01", 7, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpavgwV_VX_k1_HX_WX_2_Data))]
		void Test32_VpavgwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpavgwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B E3 D3", 6, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B E3 D3", 6, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B E3 D3", 6, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B E3 D3", 6, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB E3 D3", 6, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B E3 D3", 6, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B E3 D3", 6, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB E3 D3", 6, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B E3 D3", 6, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpavgwV_VX_k1_HX_WX_1_Data))]
		void Test64_VpavgwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpavgwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B E3 50 01", 7, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F14D8D E3 50 01", 7, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F14D08 E3 50 01", 7, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F1CD0B E3 50 01", 7, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F14D2B E3 50 01", 7, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F14DAD E3 50 01", 7, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F14D28 E3 50 01", 7, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F1CD2B E3 50 01", 7, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F14D4B E3 50 01", 7, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F14DCD E3 50 01", 7, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F14D48 E3 50 01", 7, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F1CD4B E3 50 01", 7, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpavgwV_VX_k1_HX_WX_2_Data))]
		void Test64_VpavgwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpavgwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B E3 D3", 6, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B E3 D3", 6, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 E3 D3", 6, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B E3 D3", 6, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B E3 D3", 6, Code.EVEX_Vpavgw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B E3 D3", 6, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB E3 D3", 6, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 E3 D3", 6, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B E3 D3", 6, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B E3 D3", 6, Code.EVEX_Vpavgw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B E3 D3", 6, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB E3 D3", 6, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 E3 D3", 6, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B E3 D3", 6, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B E3 D3", 6, Code.EVEX_Vpavgw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmulhuwV_VX_WX_1_Data))]
		void Test16_PmulhuwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_PmulhuwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FE4 08", 3, Code.Pmulhuw_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt16 };

				yield return new object[] { "66 0FE4 08", 4, Code.Pmulhuw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmulhuwV_VX_WX_2_Data))]
		void Test16_PmulhuwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_PmulhuwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FE4 CD", 3, Code.Pmulhuw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FE4 CD", 4, Code.Pmulhuw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmulhuwV_VX_WX_1_Data))]
		void Test32_PmulhuwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_PmulhuwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FE4 08", 3, Code.Pmulhuw_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt16 };

				yield return new object[] { "66 0FE4 08", 4, Code.Pmulhuw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmulhuwV_VX_WX_2_Data))]
		void Test32_PmulhuwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_PmulhuwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FE4 CD", 3, Code.Pmulhuw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FE4 CD", 4, Code.Pmulhuw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmulhuwV_VX_WX_1_Data))]
		void Test64_PmulhuwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_PmulhuwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FE4 08", 3, Code.Pmulhuw_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt16 };

				yield return new object[] { "66 0FE4 08", 4, Code.Pmulhuw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmulhuwV_VX_WX_2_Data))]
		void Test64_PmulhuwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_PmulhuwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FE4 CD", 3, Code.Pmulhuw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FE4 CD", 4, Code.Pmulhuw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FE4 CD", 4, Code.Pmulhuw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FE4 CD", 5, Code.Pmulhuw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FE4 CD", 5, Code.Pmulhuw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FE4 CD", 5, Code.Pmulhuw_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmulhuwV_VX_HX_WX_1_Data))]
		void Test16_VpmulhuwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpmulhuwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 E4 10", 4, Code.VEX_Vpmulhuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C5CD E4 10", 4, Code.VEX_Vpmulhuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
				yield return new object[] { "C4E1C9 E4 10", 5, Code.VEX_Vpmulhuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E1CD E4 10", 5, Code.VEX_Vpmulhuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmulhuwV_VX_HX_WX_2_Data))]
		void Test16_VpmulhuwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_VpmulhuwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 E4 D3", 4, Code.VEX_Vpmulhuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD E4 D3", 4, Code.VEX_Vpmulhuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmulhuwV_VX_HX_WX_1_Data))]
		void Test32_VpmulhuwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpmulhuwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 E4 10", 4, Code.VEX_Vpmulhuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C5CD E4 10", 4, Code.VEX_Vpmulhuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
				yield return new object[] { "C4E1C9 E4 10", 5, Code.VEX_Vpmulhuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E1CD E4 10", 5, Code.VEX_Vpmulhuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmulhuwV_VX_HX_WX_2_Data))]
		void Test32_VpmulhuwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_VpmulhuwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 E4 D3", 4, Code.VEX_Vpmulhuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD E4 D3", 4, Code.VEX_Vpmulhuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmulhuwV_VX_HX_WX_1_Data))]
		void Test64_VpmulhuwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpmulhuwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 E4 10", 4, Code.VEX_Vpmulhuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C5CD E4 10", 4, Code.VEX_Vpmulhuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
				yield return new object[] { "C4E1C9 E4 10", 5, Code.VEX_Vpmulhuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt16 };
				yield return new object[] { "C4E1CD E4 10", 5, Code.VEX_Vpmulhuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmulhuwV_VX_HX_WX_2_Data))]
		void Test64_VpmulhuwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_VpmulhuwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 E4 D3", 4, Code.VEX_Vpmulhuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD E4 D3", 4, Code.VEX_Vpmulhuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 E4 D3", 4, Code.VEX_Vpmulhuw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D E4 D3", 4, Code.VEX_Vpmulhuw_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 E4 D3", 4, Code.VEX_Vpmulhuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D E4 D3", 4, Code.VEX_Vpmulhuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 E4 D3", 5, Code.VEX_Vpmulhuw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D E4 D3", 5, Code.VEX_Vpmulhuw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmulhuwV_VX_k1_HX_WX_1_Data))]
		void Test16_VpmulhuwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpmulhuwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B E4 50 01", 7, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F14D8D E4 50 01", 7, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F14D08 E4 50 01", 7, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F1CD0B E4 50 01", 7, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F14D2B E4 50 01", 7, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F14DAD E4 50 01", 7, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F14D28 E4 50 01", 7, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F1CD2B E4 50 01", 7, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F14D4B E4 50 01", 7, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F14DCD E4 50 01", 7, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F14D48 E4 50 01", 7, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F1CD4B E4 50 01", 7, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmulhuwV_VX_k1_HX_WX_2_Data))]
		void Test16_VpmulhuwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpmulhuwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B E4 D3", 6, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B E4 D3", 6, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B E4 D3", 6, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B E4 D3", 6, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB E4 D3", 6, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B E4 D3", 6, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B E4 D3", 6, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB E4 D3", 6, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B E4 D3", 6, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmulhuwV_VX_k1_HX_WX_1_Data))]
		void Test32_VpmulhuwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpmulhuwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B E4 50 01", 7, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F14D8D E4 50 01", 7, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F14D08 E4 50 01", 7, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F1CD0B E4 50 01", 7, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F14D2B E4 50 01", 7, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F14DAD E4 50 01", 7, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F14D28 E4 50 01", 7, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F1CD2B E4 50 01", 7, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F14D4B E4 50 01", 7, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F14DCD E4 50 01", 7, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F14D48 E4 50 01", 7, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F1CD4B E4 50 01", 7, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmulhuwV_VX_k1_HX_WX_2_Data))]
		void Test32_VpmulhuwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpmulhuwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B E4 D3", 6, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B E4 D3", 6, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B E4 D3", 6, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B E4 D3", 6, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB E4 D3", 6, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B E4 D3", 6, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B E4 D3", 6, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB E4 D3", 6, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B E4 D3", 6, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmulhuwV_VX_k1_HX_WX_1_Data))]
		void Test64_VpmulhuwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpmulhuwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B E4 50 01", 7, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F14D8D E4 50 01", 7, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true };
				yield return new object[] { "62 F14D08 E4 50 01", 7, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false };
				yield return new object[] { "62 F1CD0B E4 50 01", 7, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false };

				yield return new object[] { "62 F14D2B E4 50 01", 7, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F14DAD E4 50 01", 7, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true };
				yield return new object[] { "62 F14D28 E4 50 01", 7, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false };
				yield return new object[] { "62 F1CD2B E4 50 01", 7, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false };

				yield return new object[] { "62 F14D4B E4 50 01", 7, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F14DCD E4 50 01", 7, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true };
				yield return new object[] { "62 F14D48 E4 50 01", 7, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false };
				yield return new object[] { "62 F1CD4B E4 50 01", 7, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmulhuwV_VX_k1_HX_WX_2_Data))]
		void Test64_VpmulhuwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpmulhuwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B E4 D3", 6, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B E4 D3", 6, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 E4 D3", 6, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B E4 D3", 6, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B E4 D3", 6, Code.EVEX_Vpmulhuw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B E4 D3", 6, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB E4 D3", 6, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 E4 D3", 6, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B E4 D3", 6, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B E4 D3", 6, Code.EVEX_Vpmulhuw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B E4 D3", 6, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB E4 D3", 6, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 E4 D3", 6, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B E4 D3", 6, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B E4 D3", 6, Code.EVEX_Vpmulhuw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmulhwV_VX_WX_1_Data))]
		void Test16_PmulhwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_PmulhwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FE5 08", 3, Code.Pmulhw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0FE5 08", 4, Code.Pmulhw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmulhwV_VX_WX_2_Data))]
		void Test16_PmulhwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_PmulhwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FE5 CD", 3, Code.Pmulhw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FE5 CD", 4, Code.Pmulhw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmulhwV_VX_WX_1_Data))]
		void Test32_PmulhwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_PmulhwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FE5 08", 3, Code.Pmulhw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0FE5 08", 4, Code.Pmulhw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmulhwV_VX_WX_2_Data))]
		void Test32_PmulhwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_PmulhwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FE5 CD", 3, Code.Pmulhw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FE5 CD", 4, Code.Pmulhw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmulhwV_VX_WX_1_Data))]
		void Test64_PmulhwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_PmulhwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FE5 08", 3, Code.Pmulhw_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0FE5 08", 4, Code.Pmulhw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmulhwV_VX_WX_2_Data))]
		void Test64_PmulhwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_PmulhwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FE5 CD", 3, Code.Pmulhw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FE5 CD", 4, Code.Pmulhw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FE5 CD", 4, Code.Pmulhw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FE5 CD", 5, Code.Pmulhw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FE5 CD", 5, Code.Pmulhw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FE5 CD", 5, Code.Pmulhw_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmulhwV_VX_HX_WX_1_Data))]
		void Test16_VpmulhwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpmulhwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 E5 10", 4, Code.VEX_Vpmulhw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C5CD E5 10", 4, Code.VEX_Vpmulhw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E1C9 E5 10", 5, Code.VEX_Vpmulhw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E1CD E5 10", 5, Code.VEX_Vpmulhw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmulhwV_VX_HX_WX_2_Data))]
		void Test16_VpmulhwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_VpmulhwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 E5 D3", 4, Code.VEX_Vpmulhw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD E5 D3", 4, Code.VEX_Vpmulhw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmulhwV_VX_HX_WX_1_Data))]
		void Test32_VpmulhwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpmulhwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 E5 10", 4, Code.VEX_Vpmulhw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C5CD E5 10", 4, Code.VEX_Vpmulhw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E1C9 E5 10", 5, Code.VEX_Vpmulhw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E1CD E5 10", 5, Code.VEX_Vpmulhw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmulhwV_VX_HX_WX_2_Data))]
		void Test32_VpmulhwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_VpmulhwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 E5 D3", 4, Code.VEX_Vpmulhw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD E5 D3", 4, Code.VEX_Vpmulhw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmulhwV_VX_HX_WX_1_Data))]
		void Test64_VpmulhwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpmulhwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 E5 10", 4, Code.VEX_Vpmulhw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C5CD E5 10", 4, Code.VEX_Vpmulhw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E1C9 E5 10", 5, Code.VEX_Vpmulhw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E1CD E5 10", 5, Code.VEX_Vpmulhw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmulhwV_VX_HX_WX_2_Data))]
		void Test64_VpmulhwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_VpmulhwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 E5 D3", 4, Code.VEX_Vpmulhw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD E5 D3", 4, Code.VEX_Vpmulhw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 E5 D3", 4, Code.VEX_Vpmulhw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D E5 D3", 4, Code.VEX_Vpmulhw_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 E5 D3", 4, Code.VEX_Vpmulhw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D E5 D3", 4, Code.VEX_Vpmulhw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 E5 D3", 5, Code.VEX_Vpmulhw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D E5 D3", 5, Code.VEX_Vpmulhw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmulhwV_VX_k1_HX_WX_1_Data))]
		void Test16_VpmulhwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpmulhwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B E5 50 01", 7, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D8D E5 50 01", 7, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F14D08 E5 50 01", 7, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1CD0B E5 50 01", 7, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F14D2B E5 50 01", 7, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14DAD E5 50 01", 7, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F14D28 E5 50 01", 7, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1CD2B E5 50 01", 7, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F14D4B E5 50 01", 7, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14DCD E5 50 01", 7, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F14D48 E5 50 01", 7, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1CD4B E5 50 01", 7, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmulhwV_VX_k1_HX_WX_2_Data))]
		void Test16_VpmulhwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_VpmulhwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B E5 D3", 6, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B E5 D3", 6, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B E5 D3", 6, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B E5 D3", 6, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB E5 D3", 6, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B E5 D3", 6, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B E5 D3", 6, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB E5 D3", 6, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B E5 D3", 6, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmulhwV_VX_k1_HX_WX_1_Data))]
		void Test32_VpmulhwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpmulhwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B E5 50 01", 7, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D8D E5 50 01", 7, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F14D08 E5 50 01", 7, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1CD0B E5 50 01", 7, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F14D2B E5 50 01", 7, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14DAD E5 50 01", 7, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F14D28 E5 50 01", 7, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1CD2B E5 50 01", 7, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F14D4B E5 50 01", 7, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14DCD E5 50 01", 7, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F14D48 E5 50 01", 7, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1CD4B E5 50 01", 7, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmulhwV_VX_k1_HX_WX_2_Data))]
		void Test32_VpmulhwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_VpmulhwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B E5 D3", 6, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B E5 D3", 6, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B E5 D3", 6, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B E5 D3", 6, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB E5 D3", 6, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B E5 D3", 6, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B E5 D3", 6, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB E5 D3", 6, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B E5 D3", 6, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmulhwV_VX_k1_HX_WX_1_Data))]
		void Test64_VpmulhwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpmulhwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B E5 50 01", 7, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D8D E5 50 01", 7, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F14D08 E5 50 01", 7, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1CD0B E5 50 01", 7, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F14D2B E5 50 01", 7, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14DAD E5 50 01", 7, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F14D28 E5 50 01", 7, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1CD2B E5 50 01", 7, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F14D4B E5 50 01", 7, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14DCD E5 50 01", 7, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F14D48 E5 50 01", 7, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1CD4B E5 50 01", 7, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmulhwV_VX_k1_HX_WX_2_Data))]
		void Test64_VpmulhwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_VpmulhwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B E5 D3", 6, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B E5 D3", 6, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 E5 D3", 6, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B E5 D3", 6, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B E5 D3", 6, Code.EVEX_Vpmulhw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B E5 D3", 6, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB E5 D3", 6, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 E5 D3", 6, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B E5 D3", 6, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B E5 D3", 6, Code.EVEX_Vpmulhw_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B E5 D3", 6, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB E5 D3", 6, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 E5 D3", 6, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B E5 D3", 6, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B E5 D3", 6, Code.EVEX_Vpmulhw_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_CvtV_Reg_RegMem_1_Data))]
		void Test16_CvtV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_CvtV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0FE6 08", 4, Code.Cvttpd2dq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0FE6 08", 4, Code.Cvtdq2pd_xmm_xmmm64, Register.XMM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "F2 0FE6 08", 4, Code.Cvtpd2dq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5F9 E6 10", 4, Code.VEX_Vcvttpd2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1F9 E6 10", 5, Code.VEX_Vcvttpd2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FD E6 10", 4, Code.VEX_Vcvttpd2dq_xmm_ymmm256, Register.XMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FD E6 10", 5, Code.VEX_Vcvttpd2dq_xmm_ymmm256, Register.XMM2, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5FA E6 10", 4, Code.VEX_Vcvtdq2pd_xmm_xmmm64, Register.XMM2, MemorySize.Packed64_Int32 };
				yield return new object[] { "C4E1FA E6 10", 5, Code.VEX_Vcvtdq2pd_xmm_xmmm64, Register.XMM2, MemorySize.Packed64_Int32 };

				yield return new object[] { "C5FE E6 10", 4, Code.VEX_Vcvtdq2pd_ymm_xmmm128, Register.YMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1FE E6 10", 5, Code.VEX_Vcvtdq2pd_ymm_xmmm128, Register.YMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FB E6 10", 4, Code.VEX_Vcvtpd2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1FB E6 10", 5, Code.VEX_Vcvtpd2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FF E6 10", 4, Code.VEX_Vcvtpd2dq_xmm_ymmm256, Register.XMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FF E6 10", 5, Code.VEX_Vcvtpd2dq_xmm_ymmm256, Register.XMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_CvtV_Reg_RegMem_2_Data))]
		void Test16_CvtV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_CvtV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0FE6 CD", 4, Code.Cvttpd2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0FE6 CD", 4, Code.Cvtdq2pd_xmm_xmmm64, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0FE6 CD", 4, Code.Cvtpd2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5F9 E6 CD", 4, Code.VEX_Vcvttpd2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FD E6 CD", 4, Code.VEX_Vcvttpd2dq_xmm_ymmm256, Register.XMM1, Register.YMM5 };

				yield return new object[] { "C5FA E6 CD", 4, Code.VEX_Vcvtdq2pd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FE E6 CD", 4, Code.VEX_Vcvtdq2pd_ymm_xmmm128, Register.YMM1, Register.XMM5 };

				yield return new object[] { "C5FB E6 CD", 4, Code.VEX_Vcvtpd2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FF E6 CD", 4, Code.VEX_Vcvtpd2dq_xmm_ymmm256, Register.XMM1, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CvtV_Reg_RegMem_1_Data))]
		void Test32_CvtV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_CvtV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0FE6 08", 4, Code.Cvttpd2dq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0FE6 08", 4, Code.Cvtdq2pd_xmm_xmmm64, Register.XMM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "F2 0FE6 08", 4, Code.Cvtpd2dq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5F9 E6 10", 4, Code.VEX_Vcvttpd2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1F9 E6 10", 5, Code.VEX_Vcvttpd2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FD E6 10", 4, Code.VEX_Vcvttpd2dq_xmm_ymmm256, Register.XMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FD E6 10", 5, Code.VEX_Vcvttpd2dq_xmm_ymmm256, Register.XMM2, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5FA E6 10", 4, Code.VEX_Vcvtdq2pd_xmm_xmmm64, Register.XMM2, MemorySize.Packed64_Int32 };
				yield return new object[] { "C4E1FA E6 10", 5, Code.VEX_Vcvtdq2pd_xmm_xmmm64, Register.XMM2, MemorySize.Packed64_Int32 };

				yield return new object[] { "C5FE E6 10", 4, Code.VEX_Vcvtdq2pd_ymm_xmmm128, Register.YMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1FE E6 10", 5, Code.VEX_Vcvtdq2pd_ymm_xmmm128, Register.YMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FB E6 10", 4, Code.VEX_Vcvtpd2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1FB E6 10", 5, Code.VEX_Vcvtpd2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FF E6 10", 4, Code.VEX_Vcvtpd2dq_xmm_ymmm256, Register.XMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FF E6 10", 5, Code.VEX_Vcvtpd2dq_xmm_ymmm256, Register.XMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CvtV_Reg_RegMem_2_Data))]
		void Test32_CvtV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_CvtV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0FE6 CD", 4, Code.Cvttpd2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F3 0FE6 CD", 4, Code.Cvtdq2pd_xmm_xmmm64, Register.XMM1, Register.XMM5 };

				yield return new object[] { "F2 0FE6 CD", 4, Code.Cvtpd2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };

				yield return new object[] { "C5F9 E6 CD", 4, Code.VEX_Vcvttpd2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FD E6 CD", 4, Code.VEX_Vcvttpd2dq_xmm_ymmm256, Register.XMM1, Register.YMM5 };

				yield return new object[] { "C5FA E6 CD", 4, Code.VEX_Vcvtdq2pd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FE E6 CD", 4, Code.VEX_Vcvtdq2pd_ymm_xmmm128, Register.YMM1, Register.XMM5 };

				yield return new object[] { "C5FB E6 CD", 4, Code.VEX_Vcvtpd2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C5FF E6 CD", 4, Code.VEX_Vcvtpd2dq_xmm_ymmm256, Register.XMM1, Register.YMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CvtV_Reg_RegMem_1_Data))]
		void Test64_CvtV_Reg_RegMem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_CvtV_Reg_RegMem_1_Data {
			get {
				yield return new object[] { "66 0FE6 08", 4, Code.Cvttpd2dq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "F3 0FE6 08", 4, Code.Cvtdq2pd_xmm_xmmm64, Register.XMM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "F2 0FE6 08", 4, Code.Cvtpd2dq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5F9 E6 10", 4, Code.VEX_Vcvttpd2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1F9 E6 10", 5, Code.VEX_Vcvttpd2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FD E6 10", 4, Code.VEX_Vcvttpd2dq_xmm_ymmm256, Register.XMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FD E6 10", 5, Code.VEX_Vcvttpd2dq_xmm_ymmm256, Register.XMM2, MemorySize.Packed256_Float64 };

				yield return new object[] { "C5FA E6 10", 4, Code.VEX_Vcvtdq2pd_xmm_xmmm64, Register.XMM2, MemorySize.Packed64_Int32 };
				yield return new object[] { "C4E1FA E6 10", 5, Code.VEX_Vcvtdq2pd_xmm_xmmm64, Register.XMM2, MemorySize.Packed64_Int32 };

				yield return new object[] { "C5FE E6 10", 4, Code.VEX_Vcvtdq2pd_ymm_xmmm128, Register.YMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1FE E6 10", 5, Code.VEX_Vcvtdq2pd_ymm_xmmm128, Register.YMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FB E6 10", 4, Code.VEX_Vcvtpd2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };
				yield return new object[] { "C4E1FB E6 10", 5, Code.VEX_Vcvtpd2dq_xmm_xmmm128, Register.XMM2, MemorySize.Packed128_Float64 };

				yield return new object[] { "C5FF E6 10", 4, Code.VEX_Vcvtpd2dq_xmm_ymmm256, Register.XMM2, MemorySize.Packed256_Float64 };
				yield return new object[] { "C4E1FF E6 10", 5, Code.VEX_Vcvtpd2dq_xmm_ymmm256, Register.XMM2, MemorySize.Packed256_Float64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CvtV_Reg_RegMem_2_Data))]
		void Test64_CvtV_Reg_RegMem_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_CvtV_Reg_RegMem_2_Data {
			get {
				yield return new object[] { "66 0FE6 CD", 4, Code.Cvttpd2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FE6 CD", 5, Code.Cvttpd2dq_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FE6 CD", 5, Code.Cvttpd2dq_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FE6 CD", 5, Code.Cvttpd2dq_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F3 0FE6 CD", 4, Code.Cvtdq2pd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F3 44 0FE6 CD", 5, Code.Cvtdq2pd_xmm_xmmm64, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F3 41 0FE6 CD", 5, Code.Cvtdq2pd_xmm_xmmm64, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F3 45 0FE6 CD", 5, Code.Cvtdq2pd_xmm_xmmm64, Register.XMM9, Register.XMM13 };

				yield return new object[] { "F2 0FE6 CD", 4, Code.Cvtpd2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "F2 44 0FE6 CD", 5, Code.Cvtpd2dq_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "F2 41 0FE6 CD", 5, Code.Cvtpd2dq_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "F2 45 0FE6 CD", 5, Code.Cvtpd2dq_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C5F9 E6 CD", 4, Code.VEX_Vcvttpd2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C579 E6 CD", 4, Code.VEX_Vcvttpd2dq_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C179 E6 CD", 5, Code.VEX_Vcvttpd2dq_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C44179 E6 CD", 5, Code.VEX_Vcvttpd2dq_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C5FD E6 CD", 4, Code.VEX_Vcvttpd2dq_xmm_ymmm256, Register.XMM1, Register.YMM5 };
				yield return new object[] { "C57D E6 CD", 4, Code.VEX_Vcvttpd2dq_xmm_ymmm256, Register.XMM9, Register.YMM5 };
				yield return new object[] { "C4C17D E6 CD", 5, Code.VEX_Vcvttpd2dq_xmm_ymmm256, Register.XMM1, Register.YMM13 };
				yield return new object[] { "C4417D E6 CD", 5, Code.VEX_Vcvttpd2dq_xmm_ymmm256, Register.XMM9, Register.YMM13 };

				yield return new object[] { "C5FA E6 CD", 4, Code.VEX_Vcvtdq2pd_xmm_xmmm64, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C57A E6 CD", 4, Code.VEX_Vcvtdq2pd_xmm_xmmm64, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C17A E6 CD", 5, Code.VEX_Vcvtdq2pd_xmm_xmmm64, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C4417A E6 CD", 5, Code.VEX_Vcvtdq2pd_xmm_xmmm64, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C5FE E6 CD", 4, Code.VEX_Vcvtdq2pd_ymm_xmmm128, Register.YMM1, Register.XMM5 };
				yield return new object[] { "C57E E6 CD", 4, Code.VEX_Vcvtdq2pd_ymm_xmmm128, Register.YMM9, Register.XMM5 };
				yield return new object[] { "C4C17E E6 CD", 5, Code.VEX_Vcvtdq2pd_ymm_xmmm128, Register.YMM1, Register.XMM13 };
				yield return new object[] { "C4417E E6 CD", 5, Code.VEX_Vcvtdq2pd_ymm_xmmm128, Register.YMM9, Register.XMM13 };

				yield return new object[] { "C5FB E6 CD", 4, Code.VEX_Vcvtpd2dq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "C57B E6 CD", 4, Code.VEX_Vcvtpd2dq_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "C4C17B E6 CD", 5, Code.VEX_Vcvtpd2dq_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "C4417B E6 CD", 5, Code.VEX_Vcvtpd2dq_xmm_xmmm128, Register.XMM9, Register.XMM13 };

				yield return new object[] { "C5FF E6 CD", 4, Code.VEX_Vcvtpd2dq_xmm_ymmm256, Register.XMM1, Register.YMM5 };
				yield return new object[] { "C57F E6 CD", 4, Code.VEX_Vcvtpd2dq_xmm_ymmm256, Register.XMM9, Register.YMM5 };
				yield return new object[] { "C4C17F E6 CD", 5, Code.VEX_Vcvtpd2dq_xmm_ymmm256, Register.XMM1, Register.YMM13 };
				yield return new object[] { "C4417F E6 CD", 5, Code.VEX_Vcvtpd2dq_xmm_ymmm256, Register.XMM9, Register.YMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_CvtV_Reg_k1_RegMem_EVEX_1_Data))]
		void Test16_CvtV_Reg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test16_CvtV_Reg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F1FD08 E6 50 01", 7, Code.EVEX_Vcvttpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD9B E6 50 01", 7, Code.EVEX_Vcvttpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FD28 E6 50 01", 7, Code.EVEX_Vcvttpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDBB E6 50 01", 7, Code.EVEX_Vcvttpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FD48 E6 50 01", 7, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDDB E6 50 01", 7, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };

				yield return new object[] { "62 F17E08 E6 50 01", 7, Code.EVEX_Vcvtdq2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.None, MemorySize.Packed64_Int32, 8, false };
				yield return new object[] { "62 F17E9B E6 50 01", 7, Code.EVEX_Vcvtdq2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.K3, MemorySize.Broadcast64_Int32, 4, true };

				yield return new object[] { "62 F17E28 E6 50 01", 7, Code.EVEX_Vcvtdq2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F17EBB E6 50 01", 7, Code.EVEX_Vcvtdq2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.K3, MemorySize.Broadcast128_Int32, 4, true };

				yield return new object[] { "62 F17E48 E6 50 01", 7, Code.EVEX_Vcvtdq2pd_zmm_k1z_ymmm256b32, Register.ZMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F17EDB E6 50 01", 7, Code.EVEX_Vcvtdq2pd_zmm_k1z_ymmm256b32, Register.ZMM2, Register.K3, MemorySize.Broadcast256_Int32, 4, true };

				yield return new object[] { "62 F1FE08 E6 50 01", 7, Code.EVEX_Vcvtqq2pd_xmm_k1z_xmmm128b64, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F1FE9B E6 50 01", 7, Code.EVEX_Vcvtqq2pd_xmm_k1z_xmmm128b64, Register.XMM2, Register.K3, MemorySize.Broadcast128_Int64, 8, true };

				yield return new object[] { "62 F1FE28 E6 50 01", 7, Code.EVEX_Vcvtqq2pd_ymm_k1z_ymmm256b64, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F1FEBB E6 50 01", 7, Code.EVEX_Vcvtqq2pd_ymm_k1z_ymmm256b64, Register.YMM2, Register.K3, MemorySize.Broadcast256_Int64, 8, true };

				yield return new object[] { "62 F1FE48 E6 50 01", 7, Code.EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F1FEDB E6 50 01", 7, Code.EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Int64, 8, true };

				yield return new object[] { "62 F1FF08 E6 50 01", 7, Code.EVEX_Vcvtpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FF9B E6 50 01", 7, Code.EVEX_Vcvtpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FF28 E6 50 01", 7, Code.EVEX_Vcvtpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FFBB E6 50 01", 7, Code.EVEX_Vcvtpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FF48 E6 50 01", 7, Code.EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FFDB E6 50 01", 7, Code.EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_CvtV_Reg_k1_RegMem_EVEX_2_Data))]
		void Test16_CvtV_Reg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_CvtV_Reg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F1FD0B E6 D3", 6, Code.EVEX_Vcvttpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B E6 D3", 6, Code.EVEX_Vcvttpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD2B E6 D3", 6, Code.EVEX_Vcvttpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB E6 D3", 6, Code.EVEX_Vcvttpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD4B E6 D3", 6, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDDB E6 D3", 6, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F1FD1B E6 D3", 6, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD3B E6 D3", 6, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD5B E6 D3", 6, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD7B E6 D3", 6, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F17E0B E6 D3", 6, Code.EVEX_Vcvtdq2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E8B E6 D3", 6, Code.EVEX_Vcvtdq2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E2B E6 D3", 6, Code.EVEX_Vcvtdq2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17EAB E6 D3", 6, Code.EVEX_Vcvtdq2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E4B E6 D3", 6, Code.EVEX_Vcvtdq2pd_zmm_k1z_ymmm256b32, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17ECB E6 D3", 6, Code.EVEX_Vcvtdq2pd_zmm_k1z_ymmm256b32, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE0B E6 D3", 6, Code.EVEX_Vcvtqq2pd_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE8B E6 D3", 6, Code.EVEX_Vcvtqq2pd_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE2B E6 D3", 6, Code.EVEX_Vcvtqq2pd_ymm_k1z_ymmm256b64, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FEAB E6 D3", 6, Code.EVEX_Vcvtqq2pd_ymm_k1z_ymmm256b64, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE4B E6 D3", 6, Code.EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE9B E6 D3", 6, Code.EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F1FE3B E6 D3", 6, Code.EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F1FEDB E6 D3", 6, Code.EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FE7B E6 D3", 6, Code.EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F1FF0B E6 D3", 6, Code.EVEX_Vcvtpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF8B E6 D3", 6, Code.EVEX_Vcvtpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF2B E6 D3", 6, Code.EVEX_Vcvtpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FFAB E6 D3", 6, Code.EVEX_Vcvtpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF4B E6 D3", 6, Code.EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF9B E6 D3", 6, Code.EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F1FF3B E6 D3", 6, Code.EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F1FFDB E6 D3", 6, Code.EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FF7B E6 D3", 6, Code.EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CvtV_Reg_k1_RegMem_EVEX_1_Data))]
		void Test32_CvtV_Reg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test32_CvtV_Reg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F1FD08 E6 50 01", 7, Code.EVEX_Vcvttpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD9B E6 50 01", 7, Code.EVEX_Vcvttpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FD28 E6 50 01", 7, Code.EVEX_Vcvttpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDBB E6 50 01", 7, Code.EVEX_Vcvttpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FD48 E6 50 01", 7, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDDB E6 50 01", 7, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };

				yield return new object[] { "62 F17E08 E6 50 01", 7, Code.EVEX_Vcvtdq2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.None, MemorySize.Packed64_Int32, 8, false };
				yield return new object[] { "62 F17E9B E6 50 01", 7, Code.EVEX_Vcvtdq2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.K3, MemorySize.Broadcast64_Int32, 4, true };

				yield return new object[] { "62 F17E28 E6 50 01", 7, Code.EVEX_Vcvtdq2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F17EBB E6 50 01", 7, Code.EVEX_Vcvtdq2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.K3, MemorySize.Broadcast128_Int32, 4, true };

				yield return new object[] { "62 F17E48 E6 50 01", 7, Code.EVEX_Vcvtdq2pd_zmm_k1z_ymmm256b32, Register.ZMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F17EDB E6 50 01", 7, Code.EVEX_Vcvtdq2pd_zmm_k1z_ymmm256b32, Register.ZMM2, Register.K3, MemorySize.Broadcast256_Int32, 4, true };

				yield return new object[] { "62 F1FE08 E6 50 01", 7, Code.EVEX_Vcvtqq2pd_xmm_k1z_xmmm128b64, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F1FE9B E6 50 01", 7, Code.EVEX_Vcvtqq2pd_xmm_k1z_xmmm128b64, Register.XMM2, Register.K3, MemorySize.Broadcast128_Int64, 8, true };

				yield return new object[] { "62 F1FE28 E6 50 01", 7, Code.EVEX_Vcvtqq2pd_ymm_k1z_ymmm256b64, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F1FEBB E6 50 01", 7, Code.EVEX_Vcvtqq2pd_ymm_k1z_ymmm256b64, Register.YMM2, Register.K3, MemorySize.Broadcast256_Int64, 8, true };

				yield return new object[] { "62 F1FE48 E6 50 01", 7, Code.EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F1FEDB E6 50 01", 7, Code.EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Int64, 8, true };

				yield return new object[] { "62 F1FF08 E6 50 01", 7, Code.EVEX_Vcvtpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FF9B E6 50 01", 7, Code.EVEX_Vcvtpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FF28 E6 50 01", 7, Code.EVEX_Vcvtpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FFBB E6 50 01", 7, Code.EVEX_Vcvtpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FF48 E6 50 01", 7, Code.EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FFDB E6 50 01", 7, Code.EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_CvtV_Reg_k1_RegMem_EVEX_2_Data))]
		void Test32_CvtV_Reg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_CvtV_Reg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F1FD0B E6 D3", 6, Code.EVEX_Vcvttpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B E6 D3", 6, Code.EVEX_Vcvttpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD2B E6 D3", 6, Code.EVEX_Vcvttpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB E6 D3", 6, Code.EVEX_Vcvttpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD4B E6 D3", 6, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDDB E6 D3", 6, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 F1FD1B E6 D3", 6, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD3B E6 D3", 6, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD5B E6 D3", 6, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD7B E6 D3", 6, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F17E0B E6 D3", 6, Code.EVEX_Vcvtdq2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E8B E6 D3", 6, Code.EVEX_Vcvtdq2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E2B E6 D3", 6, Code.EVEX_Vcvtdq2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17EAB E6 D3", 6, Code.EVEX_Vcvtdq2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E4B E6 D3", 6, Code.EVEX_Vcvtdq2pd_zmm_k1z_ymmm256b32, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17ECB E6 D3", 6, Code.EVEX_Vcvtdq2pd_zmm_k1z_ymmm256b32, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE0B E6 D3", 6, Code.EVEX_Vcvtqq2pd_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE8B E6 D3", 6, Code.EVEX_Vcvtqq2pd_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE2B E6 D3", 6, Code.EVEX_Vcvtqq2pd_ymm_k1z_ymmm256b64, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FEAB E6 D3", 6, Code.EVEX_Vcvtqq2pd_ymm_k1z_ymmm256b64, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE4B E6 D3", 6, Code.EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE9B E6 D3", 6, Code.EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F1FE3B E6 D3", 6, Code.EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F1FEDB E6 D3", 6, Code.EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FE7B E6 D3", 6, Code.EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F1FF0B E6 D3", 6, Code.EVEX_Vcvtpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF8B E6 D3", 6, Code.EVEX_Vcvtpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF2B E6 D3", 6, Code.EVEX_Vcvtpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FFAB E6 D3", 6, Code.EVEX_Vcvtpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF4B E6 D3", 6, Code.EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF9B E6 D3", 6, Code.EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 F1FF3B E6 D3", 6, Code.EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 F1FFDB E6 D3", 6, Code.EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FF7B E6 D3", 6, Code.EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CvtV_Reg_k1_RegMem_EVEX_1_Data))]
		void Test64_CvtV_Reg_k1_RegMem_EVEX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

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
		public static IEnumerable<object[]> Test64_CvtV_Reg_k1_RegMem_EVEX_1_Data {
			get {
				yield return new object[] { "62 F1FD08 E6 50 01", 7, Code.EVEX_Vcvttpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FD9B E6 50 01", 7, Code.EVEX_Vcvttpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FD28 E6 50 01", 7, Code.EVEX_Vcvttpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FDBB E6 50 01", 7, Code.EVEX_Vcvttpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FD48 E6 50 01", 7, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FDDB E6 50 01", 7, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };

				yield return new object[] { "62 F17E08 E6 50 01", 7, Code.EVEX_Vcvtdq2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.None, MemorySize.Packed64_Int32, 8, false };
				yield return new object[] { "62 F17E9B E6 50 01", 7, Code.EVEX_Vcvtdq2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.K3, MemorySize.Broadcast64_Int32, 4, true };

				yield return new object[] { "62 F17E28 E6 50 01", 7, Code.EVEX_Vcvtdq2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 F17EBB E6 50 01", 7, Code.EVEX_Vcvtdq2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.K3, MemorySize.Broadcast128_Int32, 4, true };

				yield return new object[] { "62 F17E48 E6 50 01", 7, Code.EVEX_Vcvtdq2pd_zmm_k1z_ymmm256b32, Register.ZMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 F17EDB E6 50 01", 7, Code.EVEX_Vcvtdq2pd_zmm_k1z_ymmm256b32, Register.ZMM2, Register.K3, MemorySize.Broadcast256_Int32, 4, true };

				yield return new object[] { "62 F1FE08 E6 50 01", 7, Code.EVEX_Vcvtqq2pd_xmm_k1z_xmmm128b64, Register.XMM2, Register.None, MemorySize.Packed128_Int64, 16, false };
				yield return new object[] { "62 F1FE9B E6 50 01", 7, Code.EVEX_Vcvtqq2pd_xmm_k1z_xmmm128b64, Register.XMM2, Register.K3, MemorySize.Broadcast128_Int64, 8, true };

				yield return new object[] { "62 F1FE28 E6 50 01", 7, Code.EVEX_Vcvtqq2pd_ymm_k1z_ymmm256b64, Register.YMM2, Register.None, MemorySize.Packed256_Int64, 32, false };
				yield return new object[] { "62 F1FEBB E6 50 01", 7, Code.EVEX_Vcvtqq2pd_ymm_k1z_ymmm256b64, Register.YMM2, Register.K3, MemorySize.Broadcast256_Int64, 8, true };

				yield return new object[] { "62 F1FE48 E6 50 01", 7, Code.EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er, Register.ZMM2, Register.None, MemorySize.Packed512_Int64, 64, false };
				yield return new object[] { "62 F1FEDB E6 50 01", 7, Code.EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er, Register.ZMM2, Register.K3, MemorySize.Broadcast512_Int64, 8, true };

				yield return new object[] { "62 F1FF08 E6 50 01", 7, Code.EVEX_Vcvtpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.None, MemorySize.Packed128_Float64, 16, false };
				yield return new object[] { "62 F1FF9B E6 50 01", 7, Code.EVEX_Vcvtpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.K3, MemorySize.Broadcast128_Float64, 8, true };

				yield return new object[] { "62 F1FF28 E6 50 01", 7, Code.EVEX_Vcvtpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.None, MemorySize.Packed256_Float64, 32, false };
				yield return new object[] { "62 F1FFBB E6 50 01", 7, Code.EVEX_Vcvtpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.K3, MemorySize.Broadcast256_Float64, 8, true };

				yield return new object[] { "62 F1FF48 E6 50 01", 7, Code.EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.None, MemorySize.Packed512_Float64, 64, false };
				yield return new object[] { "62 F1FFDB E6 50 01", 7, Code.EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.K3, MemorySize.Broadcast512_Float64, 8, true };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_CvtV_Reg_k1_RegMem_EVEX_2_Data))]
		void Test64_CvtV_Reg_k1_RegMem_EVEX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Register, instr.Op0Kind);
			Assert.Equal(reg1, instr.Op0Register);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_CvtV_Reg_k1_RegMem_EVEX_2_Data {
			get {
				yield return new object[] { "62 F1FD0B E6 D3", 6, Code.EVEX_Vcvttpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FD8B E6 D3", 6, Code.EVEX_Vcvttpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FD0B E6 D3", 6, Code.EVEX_Vcvttpd2dq_xmm_k1z_xmmm128b64, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FD8B E6 D3", 6, Code.EVEX_Vcvttpd2dq_xmm_k1z_xmmm128b64, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD2B E6 D3", 6, Code.EVEX_Vcvttpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDAB E6 D3", 6, Code.EVEX_Vcvttpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FD2B E6 D3", 6, Code.EVEX_Vcvttpd2dq_xmm_k1z_ymmm256b64, Register.XMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FDAB E6 D3", 6, Code.EVEX_Vcvttpd2dq_xmm_k1z_ymmm256b64, Register.XMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FD4B E6 D3", 6, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FDDB E6 D3", 6, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, true, true };
				yield return new object[] { "62 31FD1B E6 D3", 6, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM10, Register.ZMM19, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 C1FD3B E6 D3", 6, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM18, Register.ZMM11, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD5B E6 D3", 6, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };
				yield return new object[] { "62 F1FD7B E6 D3", 6, Code.EVEX_Vcvttpd2dq_ymm_k1z_zmmm512b64_sae, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, true };

				yield return new object[] { "62 F17E0B E6 D3", 6, Code.EVEX_Vcvtdq2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17E8B E6 D3", 6, Code.EVEX_Vcvtdq2pd_xmm_k1z_xmmm64b32, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317E0B E6 D3", 6, Code.EVEX_Vcvtdq2pd_xmm_k1z_xmmm64b32, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17E8B E6 D3", 6, Code.EVEX_Vcvtdq2pd_xmm_k1z_xmmm64b32, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E2B E6 D3", 6, Code.EVEX_Vcvtdq2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17EAB E6 D3", 6, Code.EVEX_Vcvtdq2pd_ymm_k1z_xmmm128b32, Register.YMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 317E2B E6 D3", 6, Code.EVEX_Vcvtdq2pd_ymm_k1z_xmmm128b32, Register.YMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C17EAB E6 D3", 6, Code.EVEX_Vcvtdq2pd_ymm_k1z_xmmm128b32, Register.YMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F17E4B E6 D3", 6, Code.EVEX_Vcvtdq2pd_zmm_k1z_ymmm256b32, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F17ECB E6 D3", 6, Code.EVEX_Vcvtdq2pd_zmm_k1z_ymmm256b32, Register.ZMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE0B E6 D3", 6, Code.EVEX_Vcvtqq2pd_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE8B E6 D3", 6, Code.EVEX_Vcvtqq2pd_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FE0B E6 D3", 6, Code.EVEX_Vcvtqq2pd_xmm_k1z_xmmm128b64, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FE8B E6 D3", 6, Code.EVEX_Vcvtqq2pd_xmm_k1z_xmmm128b64, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE2B E6 D3", 6, Code.EVEX_Vcvtqq2pd_ymm_k1z_ymmm256b64, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FEAB E6 D3", 6, Code.EVEX_Vcvtqq2pd_ymm_k1z_ymmm256b64, Register.YMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FE2B E6 D3", 6, Code.EVEX_Vcvtqq2pd_ymm_k1z_ymmm256b64, Register.YMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FEAB E6 D3", 6, Code.EVEX_Vcvtqq2pd_ymm_k1z_ymmm256b64, Register.YMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FE4B E6 D3", 6, Code.EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FE9B E6 D3", 6, Code.EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 31FE3B E6 D3", 6, Code.EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er, Register.ZMM10, Register.ZMM19, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 C1FEDB E6 D3", 6, Code.EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er, Register.ZMM18, Register.ZMM11, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FE7B E6 D3", 6, Code.EVEX_Vcvtqq2pd_zmm_k1z_zmmm512b64_er, Register.ZMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };

				yield return new object[] { "62 F1FF0B E6 D3", 6, Code.EVEX_Vcvtpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF8B E6 D3", 6, Code.EVEX_Vcvtpd2dq_xmm_k1z_xmmm128b64, Register.XMM2, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FF0B E6 D3", 6, Code.EVEX_Vcvtpd2dq_xmm_k1z_xmmm128b64, Register.XMM10, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FF8B E6 D3", 6, Code.EVEX_Vcvtpd2dq_xmm_k1z_xmmm128b64, Register.XMM18, Register.XMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF2B E6 D3", 6, Code.EVEX_Vcvtpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FFAB E6 D3", 6, Code.EVEX_Vcvtpd2dq_xmm_k1z_ymmm256b64, Register.XMM2, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 31FF2B E6 D3", 6, Code.EVEX_Vcvtpd2dq_xmm_k1z_ymmm256b64, Register.XMM10, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 C1FFAB E6 D3", 6, Code.EVEX_Vcvtpd2dq_xmm_k1z_ymmm256b64, Register.XMM18, Register.YMM11, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1FF4B E6 D3", 6, Code.EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1FF9B E6 D3", 6, Code.EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundToNearest, true, false };
				yield return new object[] { "62 31FF3B E6 D3", 6, Code.EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er, Register.YMM10, Register.ZMM19, Register.K3, RoundingControl.RoundDown, false, false };
				yield return new object[] { "62 C1FFDB E6 D3", 6, Code.EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er, Register.YMM18, Register.ZMM11, Register.K3, RoundingControl.RoundUp, true, false };
				yield return new object[] { "62 F1FF7B E6 D3", 6, Code.EVEX_Vcvtpd2dq_ymm_k1z_zmmm512b64_er, Register.YMM2, Register.ZMM3, Register.K3, RoundingControl.RoundTowardZero, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovntV_RegMem_Reg_1_Data))]
		void Test16_MovntV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test16_MovntV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0FE7 08", 3, Code.Movntq_m64_mm, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0FE7 08", 4, Code.Movntdq_m128_xmm, Register.XMM1, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5F9 E7 10", 4, Code.VEX_Vmovntdq_m128_xmm, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1F9 E7 10", 5, Code.VEX_Vmovntdq_m128_xmm, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FD E7 10", 4, Code.VEX_Vmovntdq_m256_ymm, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1FD E7 10", 5, Code.VEX_Vmovntdq_m256_ymm, Register.YMM2, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovntV_RegMem_Reg_1_Data))]
		void Test32_MovntV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test32_MovntV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0FE7 08", 3, Code.Movntq_m64_mm, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0FE7 08", 4, Code.Movntdq_m128_xmm, Register.XMM1, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5F9 E7 10", 4, Code.VEX_Vmovntdq_m128_xmm, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1F9 E7 10", 5, Code.VEX_Vmovntdq_m128_xmm, Register.XMM2, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FD E7 10", 4, Code.VEX_Vmovntdq_m256_ymm, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1FD E7 10", 5, Code.VEX_Vmovntdq_m256_ymm, Register.YMM2, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovntV_RegMem_Reg_1_Data))]
		void Test64_MovntV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg, instr.Op1Register);
		}
		public static IEnumerable<object[]> Test64_MovntV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "0FE7 08", 3, Code.Movntq_m64_mm, Register.MM1, MemorySize.Packed64_Int32 };

				yield return new object[] { "66 0FE7 08", 4, Code.Movntdq_m128_xmm, Register.XMM1, MemorySize.Packed128_Int32 };
				yield return new object[] { "66 44 0FE7 08", 5, Code.Movntdq_m128_xmm, Register.XMM9, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5F9 E7 10", 4, Code.VEX_Vmovntdq_m128_xmm, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C4E1F9 E7 10", 5, Code.VEX_Vmovntdq_m128_xmm, Register.XMM2, MemorySize.Packed128_Int32 };
				yield return new object[] { "C579 E7 10", 4, Code.VEX_Vmovntdq_m128_xmm, Register.XMM10, MemorySize.Packed128_Int32 };

				yield return new object[] { "C5FD E7 10", 4, Code.VEX_Vmovntdq_m256_ymm, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C4E1FD E7 10", 5, Code.VEX_Vmovntdq_m256_ymm, Register.YMM2, MemorySize.Packed256_Int32 };
				yield return new object[] { "C57D E7 10", 4, Code.VEX_Vmovntdq_m256_ymm, Register.YMM10, MemorySize.Packed256_Int32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MovntV_RegMem_Reg_EVEX_1_Data))]
		void Test16_MovntV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_MovntV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17D08 E7 50 01", 7, Code.EVEX_Vmovntdq_m128_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F17D28 E7 50 01", 7, Code.EVEX_Vmovntdq_m256_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F17D48 E7 50 01", 7, Code.EVEX_Vmovntdq_m512_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MovntV_RegMem_Reg_EVEX_1_Data))]
		void Test32_MovntV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_MovntV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17D08 E7 50 01", 7, Code.EVEX_Vmovntdq_m128_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F17D28 E7 50 01", 7, Code.EVEX_Vmovntdq_m256_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F17D48 E7 50 01", 7, Code.EVEX_Vmovntdq_m512_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MovntV_RegMem_Reg_EVEX_1_Data))]
		void Test64_MovntV_RegMem_Reg_EVEX_1(string hexBytes, int byteLength, Code code, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(2, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(Register.None, instr.SegmentPrefix);

			Assert.Equal(OpKind.Memory, instr.Op0Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(displ, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(1, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_MovntV_RegMem_Reg_EVEX_1_Data {
			get {
				yield return new object[] { "62 F17D08 E7 50 01", 7, Code.EVEX_Vmovntdq_m128_xmm, Register.XMM2, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 717D08 E7 50 01", 7, Code.EVEX_Vmovntdq_m128_xmm, Register.XMM10, Register.None, MemorySize.Packed128_Int32, 16, false };
				yield return new object[] { "62 617D08 E7 50 01", 7, Code.EVEX_Vmovntdq_m128_xmm, Register.XMM26, Register.None, MemorySize.Packed128_Int32, 16, false };

				yield return new object[] { "62 F17D28 E7 50 01", 7, Code.EVEX_Vmovntdq_m256_ymm, Register.YMM2, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 717D28 E7 50 01", 7, Code.EVEX_Vmovntdq_m256_ymm, Register.YMM10, Register.None, MemorySize.Packed256_Int32, 32, false };
				yield return new object[] { "62 617D28 E7 50 01", 7, Code.EVEX_Vmovntdq_m256_ymm, Register.YMM26, Register.None, MemorySize.Packed256_Int32, 32, false };

				yield return new object[] { "62 F17D48 E7 50 01", 7, Code.EVEX_Vmovntdq_m512_zmm, Register.ZMM2, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 717D48 E7 50 01", 7, Code.EVEX_Vmovntdq_m512_zmm, Register.ZMM10, Register.None, MemorySize.Packed512_Int32, 64, false };
				yield return new object[] { "62 617D48 E7 50 01", 7, Code.EVEX_Vmovntdq_m512_zmm, Register.ZMM26, Register.None, MemorySize.Packed512_Int32, 64, false };
			}
		}
	}
}
