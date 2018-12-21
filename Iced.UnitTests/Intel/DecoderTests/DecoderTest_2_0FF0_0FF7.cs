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
	public sealed class DecoderTest_2_0FF0_0FF7 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_Load_Reg_Mem_1_Data))]
		void Test16_Load_Reg_Mem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_Load_Reg_Mem_1_Data {
			get {
				yield return new object[] { "F2 0FF0 08", 4, Code.Lddqu_xmm_m128, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "C5FB F0 10", 4, Code.VEX_Vlddqu_xmm_m128, Register.XMM2, MemorySize.UInt128 };
				yield return new object[] { "C4E1FB F0 10", 5, Code.VEX_Vlddqu_xmm_m128, Register.XMM2, MemorySize.UInt128 };

				yield return new object[] { "C5FF F0 10", 4, Code.VEX_Vlddqu_ymm_m256, Register.YMM2, MemorySize.UInt256 };
				yield return new object[] { "C4E1FF F0 10", 5, Code.VEX_Vlddqu_ymm_m256, Register.YMM2, MemorySize.UInt256 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Load_Reg_Mem_1_Data))]
		void Test32_Load_Reg_Mem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_Load_Reg_Mem_1_Data {
			get {
				yield return new object[] { "F2 0FF0 08", 4, Code.Lddqu_xmm_m128, Register.XMM1, MemorySize.UInt128 };

				yield return new object[] { "C5FB F0 10", 4, Code.VEX_Vlddqu_xmm_m128, Register.XMM2, MemorySize.UInt128 };
				yield return new object[] { "C4E1FB F0 10", 5, Code.VEX_Vlddqu_xmm_m128, Register.XMM2, MemorySize.UInt128 };

				yield return new object[] { "C5FF F0 10", 4, Code.VEX_Vlddqu_ymm_m256, Register.YMM2, MemorySize.UInt256 };
				yield return new object[] { "C4E1FF F0 10", 5, Code.VEX_Vlddqu_ymm_m256, Register.YMM2, MemorySize.UInt256 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Load_Reg_Mem_1_Data))]
		void Test64_Load_Reg_Mem_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_Load_Reg_Mem_1_Data {
			get {
				yield return new object[] { "F2 0FF0 08", 4, Code.Lddqu_xmm_m128, Register.XMM1, MemorySize.UInt128 };
				yield return new object[] { "F2 44 0FF0 08", 5, Code.Lddqu_xmm_m128, Register.XMM9, MemorySize.UInt128 };

				yield return new object[] { "C5FB F0 10", 4, Code.VEX_Vlddqu_xmm_m128, Register.XMM2, MemorySize.UInt128 };
				yield return new object[] { "C4E1FB F0 10", 5, Code.VEX_Vlddqu_xmm_m128, Register.XMM2, MemorySize.UInt128 };
				yield return new object[] { "C57B F0 10", 4, Code.VEX_Vlddqu_xmm_m128, Register.XMM10, MemorySize.UInt128 };

				yield return new object[] { "C5FF F0 10", 4, Code.VEX_Vlddqu_ymm_m256, Register.YMM2, MemorySize.UInt256 };
				yield return new object[] { "C4E1FF F0 10", 5, Code.VEX_Vlddqu_ymm_m256, Register.YMM2, MemorySize.UInt256 };
				yield return new object[] { "C57F F0 10", 4, Code.VEX_Vlddqu_ymm_m256, Register.YMM10, MemorySize.UInt256 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsllwV_VX_WX_1_Data))]
		void Test16_PsllwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PsllwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF1 08", 3, Code.Psllw_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FF1 08", 4, Code.Psllw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsllwV_VX_WX_2_Data))]
		void Test16_PsllwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PsllwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF1 CD", 3, Code.Psllw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF1 CD", 4, Code.Psllw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsllwV_VX_WX_1_Data))]
		void Test32_PsllwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PsllwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF1 08", 3, Code.Psllw_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FF1 08", 4, Code.Psllw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsllwV_VX_WX_2_Data))]
		void Test32_PsllwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PsllwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF1 CD", 3, Code.Psllw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF1 CD", 4, Code.Psllw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsllwV_VX_WX_1_Data))]
		void Test64_PsllwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PsllwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF1 08", 3, Code.Psllw_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FF1 08", 4, Code.Psllw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsllwV_VX_WX_2_Data))]
		void Test64_PsllwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PsllwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF1 CD", 3, Code.Psllw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FF1 CD", 4, Code.Psllw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF1 CD", 4, Code.Psllw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FF1 CD", 5, Code.Psllw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FF1 CD", 5, Code.Psllw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FF1 CD", 5, Code.Psllw_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsllwV_VX_HX_WX_1_Data))]
		void Test16_VpsllwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpsllwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F1 10", 4, Code.VEX_Vpsllw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD F1 10", 4, Code.VEX_Vpsllw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 F1 10", 5, Code.VEX_Vpsllw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD F1 10", 5, Code.VEX_Vpsllw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsllwV_VX_HX_WX_2_Data))]
		void Test16_VpsllwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpsllwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F1 D3", 4, Code.VEX_Vpsllw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F1 D3", 4, Code.VEX_Vpsllw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsllwV_VX_HX_WX_1_Data))]
		void Test32_VpsllwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpsllwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F1 10", 4, Code.VEX_Vpsllw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD F1 10", 4, Code.VEX_Vpsllw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 F1 10", 5, Code.VEX_Vpsllw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD F1 10", 5, Code.VEX_Vpsllw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsllwV_VX_HX_WX_2_Data))]
		void Test32_VpsllwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpsllwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F1 D3", 4, Code.VEX_Vpsllw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F1 D3", 4, Code.VEX_Vpsllw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsllwV_VX_HX_WX_1_Data))]
		void Test64_VpsllwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpsllwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F1 10", 4, Code.VEX_Vpsllw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD F1 10", 4, Code.VEX_Vpsllw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 F1 10", 5, Code.VEX_Vpsllw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD F1 10", 5, Code.VEX_Vpsllw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsllwV_VX_HX_WX_2_Data))]
		void Test64_VpsllwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpsllwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F1 D3", 4, Code.VEX_Vpsllw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F1 D3", 4, Code.VEX_Vpsllw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
				yield return new object[] { "C549 F1 D3", 4, Code.VEX_Vpsllw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D F1 D3", 4, Code.VEX_Vpsllw_ymm_ymm_xmmm128, Register.YMM10, Register.YMM6, Register.XMM3 };
				yield return new object[] { "C589 F1 D3", 4, Code.VEX_Vpsllw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D F1 D3", 4, Code.VEX_Vpsllw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM14, Register.XMM3 };
				yield return new object[] { "C4C149 F1 D3", 5, Code.VEX_Vpsllw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D F1 D3", 5, Code.VEX_Vpsllw_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsllwV_VX_k1_HX_WX_1_Data))]
		void Test16_VpsllwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsllwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B F1 50 01", 7, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14D8D F1 50 01", 7, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D08 F1 50 01", 7, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD0B F1 50 01", 7, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D2B F1 50 01", 7, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DAD F1 50 01", 7, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D28 F1 50 01", 7, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD2B F1 50 01", 7, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D4B F1 50 01", 7, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DCD F1 50 01", 7, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D48 F1 50 01", 7, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD4B F1 50 01", 7, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsllwV_VX_k1_HX_WX_2_Data))]
		void Test16_VpsllwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpsllwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B F1 D3", 6, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B F1 D3", 6, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B F1 D3", 6, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B F1 D3", 6, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB F1 D3", 6, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B F1 D3", 6, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B F1 D3", 6, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB F1 D3", 6, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B F1 D3", 6, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsllwV_VX_k1_HX_WX_1_Data))]
		void Test32_VpsllwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsllwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B F1 50 01", 7, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14D8D F1 50 01", 7, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D08 F1 50 01", 7, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD0B F1 50 01", 7, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D2B F1 50 01", 7, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DAD F1 50 01", 7, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D28 F1 50 01", 7, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD2B F1 50 01", 7, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D4B F1 50 01", 7, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DCD F1 50 01", 7, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D48 F1 50 01", 7, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD4B F1 50 01", 7, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsllwV_VX_k1_HX_WX_2_Data))]
		void Test32_VpsllwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpsllwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B F1 D3", 6, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B F1 D3", 6, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B F1 D3", 6, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B F1 D3", 6, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB F1 D3", 6, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B F1 D3", 6, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B F1 D3", 6, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB F1 D3", 6, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B F1 D3", 6, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsllwV_VX_k1_HX_WX_1_Data))]
		void Test64_VpsllwV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsllwV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B F1 50 01", 7, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14D8D F1 50 01", 7, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D08 F1 50 01", 7, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD0B F1 50 01", 7, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D2B F1 50 01", 7, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DAD F1 50 01", 7, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D28 F1 50 01", 7, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD2B F1 50 01", 7, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D4B F1 50 01", 7, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DCD F1 50 01", 7, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D48 F1 50 01", 7, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD4B F1 50 01", 7, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsllwV_VX_k1_HX_WX_2_Data))]
		void Test64_VpsllwV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpsllwV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B F1 D3", 6, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B F1 D3", 6, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 F1 D3", 6, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B F1 D3", 6, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B F1 D3", 6, Code.EVEX_Vpsllw_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B F1 D3", 6, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB F1 D3", 6, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM18, Register.YMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 F1 D3", 6, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM10, Register.YMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B F1 D3", 6, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B F1 D3", 6, Code.EVEX_Vpsllw_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B F1 D3", 6, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB F1 D3", 6, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM18, Register.ZMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 F1 D3", 6, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM10, Register.ZMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B F1 D3", 6, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B F1 D3", 6, Code.EVEX_Vpsllw_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PslldV_VX_WX_1_Data))]
		void Test16_PslldV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PslldV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF2 08", 3, Code.Pslld_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FF2 08", 4, Code.Pslld_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PslldV_VX_WX_2_Data))]
		void Test16_PslldV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PslldV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF2 CD", 3, Code.Pslld_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF2 CD", 4, Code.Pslld_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PslldV_VX_WX_1_Data))]
		void Test32_PslldV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PslldV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF2 08", 3, Code.Pslld_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FF2 08", 4, Code.Pslld_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PslldV_VX_WX_2_Data))]
		void Test32_PslldV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PslldV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF2 CD", 3, Code.Pslld_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF2 CD", 4, Code.Pslld_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PslldV_VX_WX_1_Data))]
		void Test64_PslldV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PslldV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF2 08", 3, Code.Pslld_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FF2 08", 4, Code.Pslld_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PslldV_VX_WX_2_Data))]
		void Test64_PslldV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PslldV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF2 CD", 3, Code.Pslld_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FF2 CD", 4, Code.Pslld_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF2 CD", 4, Code.Pslld_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FF2 CD", 5, Code.Pslld_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FF2 CD", 5, Code.Pslld_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FF2 CD", 5, Code.Pslld_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpslldV_VX_HX_WX_1_Data))]
		void Test16_VpslldV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpslldV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F2 10", 4, Code.VEX_Vpslld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD F2 10", 4, Code.VEX_Vpslld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 F2 10", 5, Code.VEX_Vpslld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD F2 10", 5, Code.VEX_Vpslld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpslldV_VX_HX_WX_2_Data))]
		void Test16_VpslldV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpslldV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F2 D3", 4, Code.VEX_Vpslld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F2 D3", 4, Code.VEX_Vpslld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpslldV_VX_HX_WX_1_Data))]
		void Test32_VpslldV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpslldV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F2 10", 4, Code.VEX_Vpslld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD F2 10", 4, Code.VEX_Vpslld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 F2 10", 5, Code.VEX_Vpslld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD F2 10", 5, Code.VEX_Vpslld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpslldV_VX_HX_WX_2_Data))]
		void Test32_VpslldV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpslldV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F2 D3", 4, Code.VEX_Vpslld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F2 D3", 4, Code.VEX_Vpslld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpslldV_VX_HX_WX_1_Data))]
		void Test64_VpslldV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpslldV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F2 10", 4, Code.VEX_Vpslld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD F2 10", 4, Code.VEX_Vpslld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 F2 10", 5, Code.VEX_Vpslld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD F2 10", 5, Code.VEX_Vpslld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpslldV_VX_HX_WX_2_Data))]
		void Test64_VpslldV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpslldV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F2 D3", 4, Code.VEX_Vpslld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F2 D3", 4, Code.VEX_Vpslld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
				yield return new object[] { "C549 F2 D3", 4, Code.VEX_Vpslld_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D F2 D3", 4, Code.VEX_Vpslld_ymm_ymm_xmmm128, Register.YMM10, Register.YMM6, Register.XMM3 };
				yield return new object[] { "C589 F2 D3", 4, Code.VEX_Vpslld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D F2 D3", 4, Code.VEX_Vpslld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM14, Register.XMM3 };
				yield return new object[] { "C4C149 F2 D3", 5, Code.VEX_Vpslld_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D F2 D3", 5, Code.VEX_Vpslld_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpslldV_VX_k1_HX_WX_1_Data))]
		void Test16_VpslldV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpslldV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B F2 50 01", 7, Code.EVEX_Vpslld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14D8D F2 50 01", 7, Code.EVEX_Vpslld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D08 F2 50 01", 7, Code.EVEX_Vpslld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D2B F2 50 01", 7, Code.EVEX_Vpslld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DAD F2 50 01", 7, Code.EVEX_Vpslld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D28 F2 50 01", 7, Code.EVEX_Vpslld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D4B F2 50 01", 7, Code.EVEX_Vpslld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DCD F2 50 01", 7, Code.EVEX_Vpslld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D48 F2 50 01", 7, Code.EVEX_Vpslld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpslldV_VX_k1_HX_WX_2_Data))]
		void Test16_VpslldV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpslldV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B F2 D3", 6, Code.EVEX_Vpslld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B F2 D3", 6, Code.EVEX_Vpslld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F14D2B F2 D3", 6, Code.EVEX_Vpslld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB F2 D3", 6, Code.EVEX_Vpslld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F14D4B F2 D3", 6, Code.EVEX_Vpslld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB F2 D3", 6, Code.EVEX_Vpslld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpslldV_VX_k1_HX_WX_1_Data))]
		void Test32_VpslldV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpslldV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B F2 50 01", 7, Code.EVEX_Vpslld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14D8D F2 50 01", 7, Code.EVEX_Vpslld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D08 F2 50 01", 7, Code.EVEX_Vpslld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D2B F2 50 01", 7, Code.EVEX_Vpslld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DAD F2 50 01", 7, Code.EVEX_Vpslld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D28 F2 50 01", 7, Code.EVEX_Vpslld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D4B F2 50 01", 7, Code.EVEX_Vpslld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DCD F2 50 01", 7, Code.EVEX_Vpslld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D48 F2 50 01", 7, Code.EVEX_Vpslld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpslldV_VX_k1_HX_WX_2_Data))]
		void Test32_VpslldV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpslldV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B F2 D3", 6, Code.EVEX_Vpslld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B F2 D3", 6, Code.EVEX_Vpslld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F14D2B F2 D3", 6, Code.EVEX_Vpslld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB F2 D3", 6, Code.EVEX_Vpslld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F14D4B F2 D3", 6, Code.EVEX_Vpslld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB F2 D3", 6, Code.EVEX_Vpslld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpslldV_VX_k1_HX_WX_1_Data))]
		void Test64_VpslldV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpslldV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B F2 50 01", 7, Code.EVEX_Vpslld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14D8D F2 50 01", 7, Code.EVEX_Vpslld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D08 F2 50 01", 7, Code.EVEX_Vpslld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D2B F2 50 01", 7, Code.EVEX_Vpslld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DAD F2 50 01", 7, Code.EVEX_Vpslld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D28 F2 50 01", 7, Code.EVEX_Vpslld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F14D4B F2 50 01", 7, Code.EVEX_Vpslld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F14DCD F2 50 01", 7, Code.EVEX_Vpslld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F14D48 F2 50 01", 7, Code.EVEX_Vpslld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpslldV_VX_k1_HX_WX_2_Data))]
		void Test64_VpslldV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpslldV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B F2 D3", 6, Code.EVEX_Vpslld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B F2 D3", 6, Code.EVEX_Vpslld_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 F2 D3", 6, Code.EVEX_Vpslld_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B F2 D3", 6, Code.EVEX_Vpslld_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B F2 D3", 6, Code.EVEX_Vpslld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB F2 D3", 6, Code.EVEX_Vpslld_ymm_k1z_ymm_xmmm128, Register.YMM18, Register.YMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 F2 D3", 6, Code.EVEX_Vpslld_ymm_k1z_ymm_xmmm128, Register.YMM10, Register.YMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B F2 D3", 6, Code.EVEX_Vpslld_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B F2 D3", 6, Code.EVEX_Vpslld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB F2 D3", 6, Code.EVEX_Vpslld_zmm_k1z_zmm_xmmm128, Register.ZMM18, Register.ZMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 F2 D3", 6, Code.EVEX_Vpslld_zmm_k1z_zmm_xmmm128, Register.ZMM10, Register.ZMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B F2 D3", 6, Code.EVEX_Vpslld_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsllqV_VX_WX_1_Data))]
		void Test16_PsllqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PsllqV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF3 08", 3, Code.Psllq_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FF3 08", 4, Code.Psllq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsllqV_VX_WX_2_Data))]
		void Test16_PsllqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PsllqV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF3 CD", 3, Code.Psllq_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF3 CD", 4, Code.Psllq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsllqV_VX_WX_1_Data))]
		void Test32_PsllqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PsllqV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF3 08", 3, Code.Psllq_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FF3 08", 4, Code.Psllq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsllqV_VX_WX_2_Data))]
		void Test32_PsllqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PsllqV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF3 CD", 3, Code.Psllq_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF3 CD", 4, Code.Psllq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsllqV_VX_WX_1_Data))]
		void Test64_PsllqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PsllqV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF3 08", 3, Code.Psllq_mm_mmm64, Register.MM1, MemorySize.UInt64 };

				yield return new object[] { "66 0FF3 08", 4, Code.Psllq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsllqV_VX_WX_2_Data))]
		void Test64_PsllqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PsllqV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF3 CD", 3, Code.Psllq_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FF3 CD", 4, Code.Psllq_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF3 CD", 4, Code.Psllq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FF3 CD", 5, Code.Psllq_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FF3 CD", 5, Code.Psllq_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FF3 CD", 5, Code.Psllq_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsllqV_VX_HX_WX_1_Data))]
		void Test16_VpsllqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpsllqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F3 10", 4, Code.VEX_Vpsllq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD F3 10", 4, Code.VEX_Vpsllq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 F3 10", 5, Code.VEX_Vpsllq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD F3 10", 5, Code.VEX_Vpsllq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsllqV_VX_HX_WX_2_Data))]
		void Test16_VpsllqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpsllqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F3 D3", 4, Code.VEX_Vpsllq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F3 D3", 4, Code.VEX_Vpsllq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsllqV_VX_HX_WX_1_Data))]
		void Test32_VpsllqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpsllqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F3 10", 4, Code.VEX_Vpsllq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD F3 10", 4, Code.VEX_Vpsllq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 F3 10", 5, Code.VEX_Vpsllq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD F3 10", 5, Code.VEX_Vpsllq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsllqV_VX_HX_WX_2_Data))]
		void Test32_VpsllqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpsllqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F3 D3", 4, Code.VEX_Vpsllq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F3 D3", 4, Code.VEX_Vpsllq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsllqV_VX_HX_WX_1_Data))]
		void Test64_VpsllqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpsllqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F3 10", 4, Code.VEX_Vpsllq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C5CD F3 10", 4, Code.VEX_Vpsllq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1C9 F3 10", 5, Code.VEX_Vpsllq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64 };
				yield return new object[] { "C4E1CD F3 10", 5, Code.VEX_Vpsllq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, MemorySize.Packed128_UInt64 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsllqV_VX_HX_WX_2_Data))]
		void Test64_VpsllqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpsllqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F3 D3", 4, Code.VEX_Vpsllq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F3 D3", 4, Code.VEX_Vpsllq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3 };
				yield return new object[] { "C549 F3 D3", 4, Code.VEX_Vpsllq_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D F3 D3", 4, Code.VEX_Vpsllq_ymm_ymm_xmmm128, Register.YMM10, Register.YMM6, Register.XMM3 };
				yield return new object[] { "C589 F3 D3", 4, Code.VEX_Vpsllq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D F3 D3", 4, Code.VEX_Vpsllq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM14, Register.XMM3 };
				yield return new object[] { "C4C149 F3 D3", 5, Code.VEX_Vpsllq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D F3 D3", 5, Code.VEX_Vpsllq_ymm_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsllqV_VX_k1_HX_WX_1_Data))]
		void Test16_VpsllqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsllqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B F3 50 01", 7, Code.EVEX_Vpsllq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD8D F3 50 01", 7, Code.EVEX_Vpsllq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD08 F3 50 01", 7, Code.EVEX_Vpsllq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD2B F3 50 01", 7, Code.EVEX_Vpsllq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CDAD F3 50 01", 7, Code.EVEX_Vpsllq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD28 F3 50 01", 7, Code.EVEX_Vpsllq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD4B F3 50 01", 7, Code.EVEX_Vpsllq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CDCD F3 50 01", 7, Code.EVEX_Vpsllq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD48 F3 50 01", 7, Code.EVEX_Vpsllq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsllqV_VX_k1_HX_WX_2_Data))]
		void Test16_VpsllqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpsllqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F1CD0B F3 D3", 6, Code.EVEX_Vpsllq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD8B F3 D3", 6, Code.EVEX_Vpsllq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1CD2B F3 D3", 6, Code.EVEX_Vpsllq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CDAB F3 D3", 6, Code.EVEX_Vpsllq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1CD4B F3 D3", 6, Code.EVEX_Vpsllq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CDCB F3 D3", 6, Code.EVEX_Vpsllq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsllqV_VX_k1_HX_WX_1_Data))]
		void Test32_VpsllqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsllqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B F3 50 01", 7, Code.EVEX_Vpsllq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD8D F3 50 01", 7, Code.EVEX_Vpsllq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD08 F3 50 01", 7, Code.EVEX_Vpsllq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD2B F3 50 01", 7, Code.EVEX_Vpsllq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CDAD F3 50 01", 7, Code.EVEX_Vpsllq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD28 F3 50 01", 7, Code.EVEX_Vpsllq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD4B F3 50 01", 7, Code.EVEX_Vpsllq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CDCD F3 50 01", 7, Code.EVEX_Vpsllq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD48 F3 50 01", 7, Code.EVEX_Vpsllq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsllqV_VX_k1_HX_WX_2_Data))]
		void Test32_VpsllqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpsllqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F1CD0B F3 D3", 6, Code.EVEX_Vpsllq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD8B F3 D3", 6, Code.EVEX_Vpsllq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1CD2B F3 D3", 6, Code.EVEX_Vpsllq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CDAB F3 D3", 6, Code.EVEX_Vpsllq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };

				yield return new object[] { "62 F1CD4B F3 D3", 6, Code.EVEX_Vpsllq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CDCB F3 D3", 6, Code.EVEX_Vpsllq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsllqV_VX_k1_HX_WX_1_Data))]
		void Test64_VpsllqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsllqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B F3 50 01", 7, Code.EVEX_Vpsllq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CD8D F3 50 01", 7, Code.EVEX_Vpsllq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD08 F3 50 01", 7, Code.EVEX_Vpsllq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD2B F3 50 01", 7, Code.EVEX_Vpsllq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CDAD F3 50 01", 7, Code.EVEX_Vpsllq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD28 F3 50 01", 7, Code.EVEX_Vpsllq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F1CD4B F3 50 01", 7, Code.EVEX_Vpsllq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false };
				yield return new object[] { "62 F1CDCD F3 50 01", 7, Code.EVEX_Vpsllq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_UInt64, 16, true };
				yield return new object[] { "62 F1CD48 F3 50 01", 7, Code.EVEX_Vpsllq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_UInt64, 16, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsllqV_VX_k1_HX_WX_2_Data))]
		void Test64_VpsllqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpsllqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F1CD0B F3 D3", 6, Code.EVEX_Vpsllq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E18D8B F3 D3", 6, Code.EVEX_Vpsllq_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 11CD03 F3 D3", 6, Code.EVEX_Vpsllq_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B1CD0B F3 D3", 6, Code.EVEX_Vpsllq_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F1CD2B F3 D3", 6, Code.EVEX_Vpsllq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E18DAB F3 D3", 6, Code.EVEX_Vpsllq_ymm_k1z_ymm_xmmm128, Register.YMM18, Register.YMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 11CD23 F3 D3", 6, Code.EVEX_Vpsllq_ymm_k1z_ymm_xmmm128, Register.YMM10, Register.YMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B1CD2B F3 D3", 6, Code.EVEX_Vpsllq_ymm_k1z_ymm_xmmm128, Register.YMM2, Register.YMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F1CD4B F3 D3", 6, Code.EVEX_Vpsllq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E18DCB F3 D3", 6, Code.EVEX_Vpsllq_zmm_k1z_zmm_xmmm128, Register.ZMM18, Register.ZMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 11CD43 F3 D3", 6, Code.EVEX_Vpsllq_zmm_k1z_zmm_xmmm128, Register.ZMM10, Register.ZMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B1CD4B F3 D3", 6, Code.EVEX_Vpsllq_zmm_k1z_zmm_xmmm128, Register.ZMM2, Register.ZMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmuludqV_VX_WX_1_Data))]
		void Test16_PmuludqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PmuludqV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF4 08", 3, Code.Pmuludq_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt32 };

				yield return new object[] { "66 0FF4 08", 4, Code.Pmuludq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmuludqV_VX_WX_2_Data))]
		void Test16_PmuludqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PmuludqV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF4 CD", 3, Code.Pmuludq_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF4 CD", 4, Code.Pmuludq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmuludqV_VX_WX_1_Data))]
		void Test32_PmuludqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PmuludqV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF4 08", 3, Code.Pmuludq_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt32 };

				yield return new object[] { "66 0FF4 08", 4, Code.Pmuludq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmuludqV_VX_WX_2_Data))]
		void Test32_PmuludqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PmuludqV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF4 CD", 3, Code.Pmuludq_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF4 CD", 4, Code.Pmuludq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmuludqV_VX_WX_1_Data))]
		void Test64_PmuludqV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PmuludqV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF4 08", 3, Code.Pmuludq_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt32 };

				yield return new object[] { "66 0FF4 08", 4, Code.Pmuludq_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmuludqV_VX_WX_2_Data))]
		void Test64_PmuludqV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PmuludqV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF4 CD", 3, Code.Pmuludq_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF4 CD", 4, Code.Pmuludq_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FF4 CD", 5, Code.Pmuludq_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FF4 CD", 5, Code.Pmuludq_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FF4 CD", 5, Code.Pmuludq_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmuludqV_VX_HX_WX_1_Data))]
		void Test16_VpmuludqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmuludqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F4 10", 4, Code.VEX_Vpmuludq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C5CD F4 10", 4, Code.VEX_Vpmuludq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };
				yield return new object[] { "C4E1C9 F4 10", 5, Code.VEX_Vpmuludq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E1CD F4 10", 5, Code.VEX_Vpmuludq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmuludqV_VX_HX_WX_2_Data))]
		void Test16_VpmuludqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpmuludqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F4 D3", 4, Code.VEX_Vpmuludq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F4 D3", 4, Code.VEX_Vpmuludq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmuludqV_VX_HX_WX_1_Data))]
		void Test32_VpmuludqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmuludqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F4 10", 4, Code.VEX_Vpmuludq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C5CD F4 10", 4, Code.VEX_Vpmuludq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };
				yield return new object[] { "C4E1C9 F4 10", 5, Code.VEX_Vpmuludq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E1CD F4 10", 5, Code.VEX_Vpmuludq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmuludqV_VX_HX_WX_2_Data))]
		void Test32_VpmuludqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpmuludqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F4 D3", 4, Code.VEX_Vpmuludq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F4 D3", 4, Code.VEX_Vpmuludq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmuludqV_VX_HX_WX_1_Data))]
		void Test64_VpmuludqV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmuludqV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F4 10", 4, Code.VEX_Vpmuludq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C5CD F4 10", 4, Code.VEX_Vpmuludq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };
				yield return new object[] { "C4E1C9 F4 10", 5, Code.VEX_Vpmuludq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt32 };
				yield return new object[] { "C4E1CD F4 10", 5, Code.VEX_Vpmuludq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt32 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmuludqV_VX_HX_WX_2_Data))]
		void Test64_VpmuludqV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpmuludqV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F4 D3", 4, Code.VEX_Vpmuludq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F4 D3", 4, Code.VEX_Vpmuludq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 F4 D3", 4, Code.VEX_Vpmuludq_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D F4 D3", 4, Code.VEX_Vpmuludq_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 F4 D3", 4, Code.VEX_Vpmuludq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D F4 D3", 4, Code.VEX_Vpmuludq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 F4 D3", 5, Code.VEX_Vpmuludq_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D F4 D3", 5, Code.VEX_Vpmuludq_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmuludqV_VX_k1_HX_WX_1_Data))]
		void Test16_VpmuludqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpmuludqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B F4 50 01", 7, Code.EVEX_Vpmuludq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F1CD9D F4 50 01", 7, Code.EVEX_Vpmuludq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_2xUInt32, 8, true };
				yield return new object[] { "62 F1CD08 F4 50 01", 7, Code.EVEX_Vpmuludq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F1CD2B F4 50 01", 7, Code.EVEX_Vpmuludq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F1CDBD F4 50 01", 7, Code.EVEX_Vpmuludq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_2xUInt32, 8, true };
				yield return new object[] { "62 F1CD28 F4 50 01", 7, Code.EVEX_Vpmuludq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F1CD4B F4 50 01", 7, Code.EVEX_Vpmuludq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F1CDDD F4 50 01", 7, Code.EVEX_Vpmuludq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_2xUInt32, 8, true };
				yield return new object[] { "62 F1CD48 F4 50 01", 7, Code.EVEX_Vpmuludq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmuludqV_VX_k1_HX_WX_2_Data))]
		void Test16_VpmuludqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpmuludqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F1CD8B F4 D3", 6, Code.EVEX_Vpmuludq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B F4 D3", 6, Code.EVEX_Vpmuludq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB F4 D3", 6, Code.EVEX_Vpmuludq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B F4 D3", 6, Code.EVEX_Vpmuludq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB F4 D3", 6, Code.EVEX_Vpmuludq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B F4 D3", 6, Code.EVEX_Vpmuludq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmuludqV_VX_k1_HX_WX_1_Data))]
		void Test32_VpmuludqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpmuludqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B F4 50 01", 7, Code.EVEX_Vpmuludq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F1CD9D F4 50 01", 7, Code.EVEX_Vpmuludq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_2xUInt32, 8, true };
				yield return new object[] { "62 F1CD08 F4 50 01", 7, Code.EVEX_Vpmuludq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F1CD2B F4 50 01", 7, Code.EVEX_Vpmuludq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F1CDBD F4 50 01", 7, Code.EVEX_Vpmuludq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_2xUInt32, 8, true };
				yield return new object[] { "62 F1CD28 F4 50 01", 7, Code.EVEX_Vpmuludq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F1CD4B F4 50 01", 7, Code.EVEX_Vpmuludq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F1CDDD F4 50 01", 7, Code.EVEX_Vpmuludq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_2xUInt32, 8, true };
				yield return new object[] { "62 F1CD48 F4 50 01", 7, Code.EVEX_Vpmuludq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmuludqV_VX_k1_HX_WX_2_Data))]
		void Test32_VpmuludqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpmuludqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F1CD8B F4 D3", 6, Code.EVEX_Vpmuludq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 F1CD0B F4 D3", 6, Code.EVEX_Vpmuludq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB F4 D3", 6, Code.EVEX_Vpmuludq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 F1CD2B F4 D3", 6, Code.EVEX_Vpmuludq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB F4 D3", 6, Code.EVEX_Vpmuludq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 F1CD4B F4 D3", 6, Code.EVEX_Vpmuludq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmuludqV_VX_k1_HX_WX_1_Data))]
		void Test64_VpmuludqV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpmuludqV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F1CD0B F4 50 01", 7, Code.EVEX_Vpmuludq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false };
				yield return new object[] { "62 F1CD9D F4 50 01", 7, Code.EVEX_Vpmuludq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_2xUInt32, 8, true };
				yield return new object[] { "62 F1CD08 F4 50 01", 7, Code.EVEX_Vpmuludq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false };

				yield return new object[] { "62 F1CD2B F4 50 01", 7, Code.EVEX_Vpmuludq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false };
				yield return new object[] { "62 F1CDBD F4 50 01", 7, Code.EVEX_Vpmuludq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_2xUInt32, 8, true };
				yield return new object[] { "62 F1CD28 F4 50 01", 7, Code.EVEX_Vpmuludq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false };

				yield return new object[] { "62 F1CD4B F4 50 01", 7, Code.EVEX_Vpmuludq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false };
				yield return new object[] { "62 F1CDDD F4 50 01", 7, Code.EVEX_Vpmuludq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_2xUInt32, 8, true };
				yield return new object[] { "62 F1CD48 F4 50 01", 7, Code.EVEX_Vpmuludq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmuludqV_VX_k1_HX_WX_2_Data))]
		void Test64_VpmuludqV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z) {
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
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpmuludqV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F1CD8B F4 D3", 6, Code.EVEX_Vpmuludq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true };
				yield return new object[] { "62 E18D0B F4 D3", 6, Code.EVEX_Vpmuludq_xmm_k1z_xmm_xmmm128b64, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false };
				yield return new object[] { "62 11CD03 F4 D3", 6, Code.EVEX_Vpmuludq_xmm_k1z_xmm_xmmm128b64, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false };
				yield return new object[] { "62 B1CD0B F4 D3", 6, Code.EVEX_Vpmuludq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false };
				yield return new object[] { "62 F1CD0B F4 D3", 6, Code.EVEX_Vpmuludq_xmm_k1z_xmm_xmmm128b64, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false };

				yield return new object[] { "62 F1CDAB F4 D3", 6, Code.EVEX_Vpmuludq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true };
				yield return new object[] { "62 E18D2B F4 D3", 6, Code.EVEX_Vpmuludq_ymm_k1z_ymm_ymmm256b64, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false };
				yield return new object[] { "62 11CD23 F4 D3", 6, Code.EVEX_Vpmuludq_ymm_k1z_ymm_ymmm256b64, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false };
				yield return new object[] { "62 B1CD2B F4 D3", 6, Code.EVEX_Vpmuludq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false };
				yield return new object[] { "62 F1CD2B F4 D3", 6, Code.EVEX_Vpmuludq_ymm_k1z_ymm_ymmm256b64, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false };

				yield return new object[] { "62 F1CDCB F4 D3", 6, Code.EVEX_Vpmuludq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true };
				yield return new object[] { "62 E18D4B F4 D3", 6, Code.EVEX_Vpmuludq_zmm_k1z_zmm_zmmm512b64, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false };
				yield return new object[] { "62 11CD43 F4 D3", 6, Code.EVEX_Vpmuludq_zmm_k1z_zmm_zmmm512b64, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false };
				yield return new object[] { "62 B1CD4B F4 D3", 6, Code.EVEX_Vpmuludq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false };
				yield return new object[] { "62 F1CD4B F4 D3", 6, Code.EVEX_Vpmuludq_zmm_k1z_zmm_zmmm512b64, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmaddwdV_VX_WX_1_Data))]
		void Test16_PmaddwdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PmaddwdV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF5 08", 3, Code.Pmaddwd_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0FF5 08", 4, Code.Pmaddwd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PmaddwdV_VX_WX_2_Data))]
		void Test16_PmaddwdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PmaddwdV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF5 CD", 3, Code.Pmaddwd_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF5 CD", 4, Code.Pmaddwd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmaddwdV_VX_WX_1_Data))]
		void Test32_PmaddwdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PmaddwdV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF5 08", 3, Code.Pmaddwd_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0FF5 08", 4, Code.Pmaddwd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PmaddwdV_VX_WX_2_Data))]
		void Test32_PmaddwdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PmaddwdV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF5 CD", 3, Code.Pmaddwd_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF5 CD", 4, Code.Pmaddwd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmaddwdV_VX_WX_1_Data))]
		void Test64_PmaddwdV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PmaddwdV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF5 08", 3, Code.Pmaddwd_mm_mmm64, Register.MM1, MemorySize.Packed64_Int16 };

				yield return new object[] { "66 0FF5 08", 4, Code.Pmaddwd_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PmaddwdV_VX_WX_2_Data))]
		void Test64_PmaddwdV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PmaddwdV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF5 CD", 3, Code.Pmaddwd_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FF5 CD", 4, Code.Pmaddwd_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF5 CD", 4, Code.Pmaddwd_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FF5 CD", 5, Code.Pmaddwd_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FF5 CD", 5, Code.Pmaddwd_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FF5 CD", 5, Code.Pmaddwd_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaddwdV_VX_HX_WX_1_Data))]
		void Test16_VpmaddwdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpmaddwdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F5 10", 4, Code.VEX_Vpmaddwd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C5CD F5 10", 4, Code.VEX_Vpmaddwd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E1C9 F5 10", 5, Code.VEX_Vpmaddwd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E1CD F5 10", 5, Code.VEX_Vpmaddwd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaddwdV_VX_HX_WX_2_Data))]
		void Test16_VpmaddwdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpmaddwdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F5 D3", 4, Code.VEX_Vpmaddwd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F5 D3", 4, Code.VEX_Vpmaddwd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaddwdV_VX_HX_WX_1_Data))]
		void Test32_VpmaddwdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpmaddwdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F5 10", 4, Code.VEX_Vpmaddwd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C5CD F5 10", 4, Code.VEX_Vpmaddwd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E1C9 F5 10", 5, Code.VEX_Vpmaddwd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E1CD F5 10", 5, Code.VEX_Vpmaddwd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaddwdV_VX_HX_WX_2_Data))]
		void Test32_VpmaddwdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpmaddwdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F5 D3", 4, Code.VEX_Vpmaddwd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F5 D3", 4, Code.VEX_Vpmaddwd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaddwdV_VX_HX_WX_1_Data))]
		void Test64_VpmaddwdV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpmaddwdV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F5 10", 4, Code.VEX_Vpmaddwd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C5CD F5 10", 4, Code.VEX_Vpmaddwd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
				yield return new object[] { "C4E1C9 F5 10", 5, Code.VEX_Vpmaddwd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_Int16 };
				yield return new object[] { "C4E1CD F5 10", 5, Code.VEX_Vpmaddwd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int16 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaddwdV_VX_HX_WX_2_Data))]
		void Test64_VpmaddwdV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpmaddwdV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F5 D3", 4, Code.VEX_Vpmaddwd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F5 D3", 4, Code.VEX_Vpmaddwd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 F5 D3", 4, Code.VEX_Vpmaddwd_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D F5 D3", 4, Code.VEX_Vpmaddwd_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 F5 D3", 4, Code.VEX_Vpmaddwd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D F5 D3", 4, Code.VEX_Vpmaddwd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 F5 D3", 5, Code.VEX_Vpmaddwd_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D F5 D3", 5, Code.VEX_Vpmaddwd_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaddwdV_VX_k1_HX_WX_1_Data))]
		void Test16_VpmaddwdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpmaddwdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B F5 50 01", 7, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D8D F5 50 01", 7, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F14D08 F5 50 01", 7, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1CD0B F5 50 01", 7, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F14D2B F5 50 01", 7, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14DAD F5 50 01", 7, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F14D28 F5 50 01", 7, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1CD2B F5 50 01", 7, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F14D4B F5 50 01", 7, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14DCD F5 50 01", 7, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F14D48 F5 50 01", 7, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1CD4B F5 50 01", 7, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpmaddwdV_VX_k1_HX_WX_2_Data))]
		void Test16_VpmaddwdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpmaddwdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B F5 D3", 6, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B F5 D3", 6, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B F5 D3", 6, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B F5 D3", 6, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB F5 D3", 6, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B F5 D3", 6, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B F5 D3", 6, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB F5 D3", 6, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B F5 D3", 6, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaddwdV_VX_k1_HX_WX_1_Data))]
		void Test32_VpmaddwdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpmaddwdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B F5 50 01", 7, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D8D F5 50 01", 7, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F14D08 F5 50 01", 7, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1CD0B F5 50 01", 7, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F14D2B F5 50 01", 7, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14DAD F5 50 01", 7, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F14D28 F5 50 01", 7, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1CD2B F5 50 01", 7, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F14D4B F5 50 01", 7, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14DCD F5 50 01", 7, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F14D48 F5 50 01", 7, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1CD4B F5 50 01", 7, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpmaddwdV_VX_k1_HX_WX_2_Data))]
		void Test32_VpmaddwdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpmaddwdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B F5 D3", 6, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14D8B F5 D3", 6, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD0B F5 D3", 6, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B F5 D3", 6, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DAB F5 D3", 6, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD2B F5 D3", 6, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B F5 D3", 6, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F14DCB F5 D3", 6, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 F1CD4B F5 D3", 6, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaddwdV_VX_k1_HX_WX_1_Data))]
		void Test64_VpmaddwdV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpmaddwdV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D0B F5 50 01", 7, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F14D8D F5 50 01", 7, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_Int16, 16, true };
				yield return new object[] { "62 F14D08 F5 50 01", 7, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_Int16, 16, false };
				yield return new object[] { "62 F1CD0B F5 50 01", 7, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_Int16, 16, false };

				yield return new object[] { "62 F14D2B F5 50 01", 7, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F14DAD F5 50 01", 7, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_Int16, 32, true };
				yield return new object[] { "62 F14D28 F5 50 01", 7, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int16, 32, false };
				yield return new object[] { "62 F1CD2B F5 50 01", 7, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int16, 32, false };

				yield return new object[] { "62 F14D4B F5 50 01", 7, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F14DCD F5 50 01", 7, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_Int16, 64, true };
				yield return new object[] { "62 F14D48 F5 50 01", 7, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int16, 64, false };
				yield return new object[] { "62 F1CD4B F5 50 01", 7, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int16, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpmaddwdV_VX_k1_HX_WX_2_Data))]
		void Test64_VpmaddwdV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpmaddwdV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D0B F5 D3", 6, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D8B F5 D3", 6, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D03 F5 D3", 6, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D0B F5 D3", 6, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD0B F5 D3", 6, Code.EVEX_Vpmaddwd_xmm_k1z_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D2B F5 D3", 6, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DAB F5 D3", 6, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D23 F5 D3", 6, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D2B F5 D3", 6, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD2B F5 D3", 6, Code.EVEX_Vpmaddwd_ymm_k1z_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D4B F5 D3", 6, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 E10DCB F5 D3", 6, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false };
				yield return new object[] { "62 114D43 F5 D3", 6, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D4B F5 D3", 6, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD4B F5 D3", 6, Code.EVEX_Vpmaddwd_zmm_k1z_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsadbwV_VX_WX_1_Data))]
		void Test16_PsadbwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_PsadbwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF6 08", 3, Code.Psadbw_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "66 0FF6 08", 4, Code.Psadbw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PsadbwV_VX_WX_2_Data))]
		void Test16_PsadbwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test16_PsadbwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF6 CD", 3, Code.Psadbw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF6 CD", 4, Code.Psadbw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsadbwV_VX_WX_1_Data))]
		void Test32_PsadbwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_PsadbwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF6 08", 3, Code.Psadbw_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "66 0FF6 08", 4, Code.Psadbw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PsadbwV_VX_WX_2_Data))]
		void Test32_PsadbwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test32_PsadbwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF6 CD", 3, Code.Psadbw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF6 CD", 4, Code.Psadbw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsadbwV_VX_WX_1_Data))]
		void Test64_PsadbwV_VX_WX_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_PsadbwV_VX_WX_1_Data {
			get {
				yield return new object[] { "0FF6 08", 3, Code.Psadbw_mm_mmm64, Register.MM1, MemorySize.Packed64_UInt8 };

				yield return new object[] { "66 0FF6 08", 4, Code.Psadbw_xmm_xmmm128, Register.XMM1, MemorySize.Packed128_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PsadbwV_VX_WX_2_Data))]
		void Test64_PsadbwV_VX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2) {
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
		public static IEnumerable<object[]> Test64_PsadbwV_VX_WX_2_Data {
			get {
				yield return new object[] { "0FF6 CD", 3, Code.Psadbw_mm_mmm64, Register.MM1, Register.MM5 };
				yield return new object[] { "4F 0FF6 CD", 4, Code.Psadbw_mm_mmm64, Register.MM1, Register.MM5 };

				yield return new object[] { "66 0FF6 CD", 4, Code.Psadbw_xmm_xmmm128, Register.XMM1, Register.XMM5 };
				yield return new object[] { "66 44 0FF6 CD", 5, Code.Psadbw_xmm_xmmm128, Register.XMM9, Register.XMM5 };
				yield return new object[] { "66 41 0FF6 CD", 5, Code.Psadbw_xmm_xmmm128, Register.XMM1, Register.XMM13 };
				yield return new object[] { "66 45 0FF6 CD", 5, Code.Psadbw_xmm_xmmm128, Register.XMM9, Register.XMM13 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsadbwV_VX_HX_WX_1_Data))]
		void Test16_VpsadbwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test16_VpsadbwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F6 10", 4, Code.VEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C5CD F6 10", 4, Code.VEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
				yield return new object[] { "C4E1C9 F6 10", 5, Code.VEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E1CD F6 10", 5, Code.VEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsadbwV_VX_HX_WX_2_Data))]
		void Test16_VpsadbwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test16_VpsadbwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F6 D3", 4, Code.VEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F6 D3", 4, Code.VEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsadbwV_VX_HX_WX_1_Data))]
		void Test32_VpsadbwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test32_VpsadbwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F6 10", 4, Code.VEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C5CD F6 10", 4, Code.VEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
				yield return new object[] { "C4E1C9 F6 10", 5, Code.VEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E1CD F6 10", 5, Code.VEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsadbwV_VX_HX_WX_2_Data))]
		void Test32_VpsadbwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test32_VpsadbwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F6 D3", 4, Code.VEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F6 D3", 4, Code.VEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsadbwV_VX_HX_WX_1_Data))]
		void Test64_VpsadbwV_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize) {
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
		public static IEnumerable<object[]> Test64_VpsadbwV_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "C5C9 F6 10", 4, Code.VEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C5CD F6 10", 4, Code.VEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
				yield return new object[] { "C4E1C9 F6 10", 5, Code.VEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8 };
				yield return new object[] { "C4E1CD F6 10", 5, Code.VEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsadbwV_VX_HX_WX_2_Data))]
		void Test64_VpsadbwV_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3) {
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
		public static IEnumerable<object[]> Test64_VpsadbwV_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "C5C9 F6 D3", 4, Code.VEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C5CD F6 D3", 4, Code.VEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C549 F6 D3", 4, Code.VEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM6, Register.XMM3 };
				yield return new object[] { "C54D F6 D3", 4, Code.VEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM10, Register.YMM6, Register.YMM3 };
				yield return new object[] { "C589 F6 D3", 4, Code.VEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM14, Register.XMM3 };
				yield return new object[] { "C58D F6 D3", 4, Code.VEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM14, Register.YMM3 };
				yield return new object[] { "C4C149 F6 D3", 5, Code.VEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM11 };
				yield return new object[] { "C4C14D F6 D3", 5, Code.VEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM11 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsadbwV_EVEX_VX_HX_WX_1_Data))]
		void Test16_VpsadbwV_EVEX_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test16_VpsadbwV_EVEX_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D08 F6 50 01", 7, Code.EVEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F14D28 F6 50 01", 7, Code.EVEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F14D48 F6 50 01", 7, Code.EVEX_Vpsadbw_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpsadbwV_EVEX_VX_HX_WX_2_Data))]
		void Test16_VpsadbwV_EVEX_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test16_VpsadbwV_EVEX_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D08 F6 D3", 6, Code.EVEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD08 F6 D3", 6, Code.EVEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D28 F6 D3", 6, Code.EVEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD28 F6 D3", 6, Code.EVEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D48 F6 D3", 6, Code.EVEX_Vpsadbw_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD48 F6 D3", 6, Code.EVEX_Vpsadbw_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsadbwV_EVEX_VX_HX_WX_1_Data))]
		void Test32_VpsadbwV_EVEX_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test32_VpsadbwV_EVEX_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D08 F6 50 01", 7, Code.EVEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F14D28 F6 50 01", 7, Code.EVEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F14D48 F6 50 01", 7, Code.EVEX_Vpsadbw_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpsadbwV_EVEX_VX_HX_WX_2_Data))]
		void Test32_VpsadbwV_EVEX_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test32_VpsadbwV_EVEX_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D08 F6 D3", 6, Code.EVEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD08 F6 D3", 6, Code.EVEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D28 F6 D3", 6, Code.EVEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD28 F6 D3", 6, Code.EVEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D48 F6 D3", 6, Code.EVEX_Vpsadbw_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD48 F6 D3", 6, Code.EVEX_Vpsadbw_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsadbwV_EVEX_VX_HX_WX_1_Data))]
		void Test64_VpsadbwV_EVEX_VX_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z) {
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
		public static IEnumerable<object[]> Test64_VpsadbwV_EVEX_VX_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F14D08 F6 50 01", 7, Code.EVEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false };

				yield return new object[] { "62 F14D28 F6 50 01", 7, Code.EVEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false };

				yield return new object[] { "62 F14D48 F6 50 01", 7, Code.EVEX_Vpsadbw_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpsadbwV_EVEX_VX_HX_WX_2_Data))]
		void Test64_VpsadbwV_EVEX_VX_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae) {
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
		public static IEnumerable<object[]> Test64_VpsadbwV_EVEX_VX_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F14D08 F6 D3", 6, Code.EVEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D08 F6 D3", 6, Code.EVEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM18, Register.XMM14, Register.XMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 114D00 F6 D3", 6, Code.EVEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM10, Register.XMM22, Register.XMM27, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D08 F6 D3", 6, Code.EVEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM19, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD08 F6 D3", 6, Code.EVEX_Vpsadbw_xmm_xmm_xmmm128, Register.XMM2, Register.XMM6, Register.XMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D28 F6 D3", 6, Code.EVEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D28 F6 D3", 6, Code.EVEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM18, Register.YMM14, Register.YMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 114D20 F6 D3", 6, Code.EVEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM10, Register.YMM22, Register.YMM27, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D28 F6 D3", 6, Code.EVEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM19, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD28 F6 D3", 6, Code.EVEX_Vpsadbw_ymm_ymm_ymmm256, Register.YMM2, Register.YMM6, Register.YMM3, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F14D48 F6 D3", 6, Code.EVEX_Vpsadbw_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E10D48 F6 D3", 6, Code.EVEX_Vpsadbw_zmm_zmm_zmmm512, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 114D40 F6 D3", 6, Code.EVEX_Vpsadbw_zmm_zmm_zmmm512, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B14D48 F6 D3", 6, Code.EVEX_Vpsadbw_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F1CD48 F6 D3", 6, Code.EVEX_Vpsadbw_zmm_zmm_zmmm512, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Maskmovdqu_1_Data))]
		void Test16_Maskmovdqu_1(string hexBytes, int byteLength, Code code, Register reg2, Register reg3, MemorySize memSize, OpKind memOpKind, Register segPrefix, Register memSeg) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(segPrefix, instr.SegmentPrefix);

			Assert.Equal(memOpKind, instr.Op0Kind);
			Assert.Equal(memSeg, instr.MemorySegment);
			Assert.Equal(memSize, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test16_Maskmovdqu_1_Data {
			get {
				yield return new object[] { "0FF7 D3", 3, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegDI, Register.None, Register.DS };
				yield return new object[] { "67 0FF7 D3", 4, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegEDI, Register.None, Register.DS };
				yield return new object[] { "26 0FF7 D3", 4, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegDI, Register.ES, Register.ES };
				yield return new object[] { "2E 0FF7 D3", 4, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegDI, Register.CS, Register.CS };
				yield return new object[] { "36 0FF7 D3", 4, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegDI, Register.SS, Register.SS };
				yield return new object[] { "3E 0FF7 D3", 4, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegDI, Register.DS, Register.DS };
				yield return new object[] { "64 0FF7 D3", 4, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegDI, Register.FS, Register.FS };
				yield return new object[] { "65 0FF7 D3", 4, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegDI, Register.GS, Register.GS };

				yield return new object[] { "66 0FF7 D3", 4, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegDI, Register.None, Register.DS };
				yield return new object[] { "67 66 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegEDI, Register.None, Register.DS };
				yield return new object[] { "26 66 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegDI, Register.ES, Register.ES };
				yield return new object[] { "2E 66 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegDI, Register.CS, Register.CS };
				yield return new object[] { "36 66 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegDI, Register.SS, Register.SS };
				yield return new object[] { "3E 66 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegDI, Register.DS, Register.DS };
				yield return new object[] { "64 66 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegDI, Register.FS, Register.FS };
				yield return new object[] { "65 66 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegDI, Register.GS, Register.GS };

				yield return new object[] { "C5F9 F7 D3", 4, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegDI, Register.None, Register.DS };
				yield return new object[] { "67 C5F9 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegEDI, Register.None, Register.DS };
				yield return new object[] { "26 C5F9 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegDI, Register.ES, Register.ES };
				yield return new object[] { "2E C5F9 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegDI, Register.CS, Register.CS };
				yield return new object[] { "36 C5F9 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegDI, Register.SS, Register.SS };
				yield return new object[] { "3E C5F9 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegDI, Register.DS, Register.DS };
				yield return new object[] { "64 C5F9 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegDI, Register.FS, Register.FS };
				yield return new object[] { "65 C5F9 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegDI, Register.GS, Register.GS };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Maskmovdqu_1_Data))]
		void Test32_Maskmovdqu_1(string hexBytes, int byteLength, Code code, Register reg2, Register reg3, MemorySize memSize, OpKind memOpKind, Register segPrefix, Register memSeg) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(segPrefix, instr.SegmentPrefix);

			Assert.Equal(memOpKind, instr.Op0Kind);
			Assert.Equal(memSeg, instr.MemorySegment);
			Assert.Equal(memSize, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test32_Maskmovdqu_1_Data {
			get {
				yield return new object[] { "0FF7 D3", 3, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegEDI, Register.None, Register.DS };
				yield return new object[] { "67 0FF7 D3", 4, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegDI, Register.None, Register.DS };
				yield return new object[] { "26 0FF7 D3", 4, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegEDI, Register.ES, Register.ES };
				yield return new object[] { "2E 0FF7 D3", 4, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegEDI, Register.CS, Register.CS };
				yield return new object[] { "36 0FF7 D3", 4, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegEDI, Register.SS, Register.SS };
				yield return new object[] { "3E 0FF7 D3", 4, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegEDI, Register.DS, Register.DS };
				yield return new object[] { "64 0FF7 D3", 4, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegEDI, Register.FS, Register.FS };
				yield return new object[] { "65 0FF7 D3", 4, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegEDI, Register.GS, Register.GS };

				yield return new object[] { "66 0FF7 D3", 4, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegEDI, Register.None, Register.DS };
				yield return new object[] { "67 66 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegDI, Register.None, Register.DS };
				yield return new object[] { "26 66 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegEDI, Register.ES, Register.ES };
				yield return new object[] { "2E 66 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegEDI, Register.CS, Register.CS };
				yield return new object[] { "36 66 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegEDI, Register.SS, Register.SS };
				yield return new object[] { "3E 66 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegEDI, Register.DS, Register.DS };
				yield return new object[] { "64 66 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegEDI, Register.FS, Register.FS };
				yield return new object[] { "65 66 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegEDI, Register.GS, Register.GS };

				yield return new object[] { "C5F9 F7 D3", 4, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegEDI, Register.None, Register.DS };
				yield return new object[] { "67 C5F9 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegDI, Register.None, Register.DS };
				yield return new object[] { "26 C5F9 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegEDI, Register.ES, Register.ES };
				yield return new object[] { "2E C5F9 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegEDI, Register.CS, Register.CS };
				yield return new object[] { "36 C5F9 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegEDI, Register.SS, Register.SS };
				yield return new object[] { "3E C5F9 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegEDI, Register.DS, Register.DS };
				yield return new object[] { "64 C5F9 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegEDI, Register.FS, Register.FS };
				yield return new object[] { "65 C5F9 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegEDI, Register.GS, Register.GS };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Maskmovdqu_1_Data))]
		void Test64_Maskmovdqu_1(string hexBytes, int byteLength, Code code, Register reg2, Register reg3, MemorySize memSize, OpKind memOpKind, Register segPrefix, Register memSeg) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
			Assert.Equal(byteLength, instr.ByteLength);
			Assert.False(instr.HasRepePrefix);
			Assert.False(instr.HasRepnePrefix);
			Assert.False(instr.HasLockPrefix);
			Assert.Equal(segPrefix, instr.SegmentPrefix);

			Assert.Equal(memOpKind, instr.Op0Kind);
			Assert.Equal(memSeg, instr.MemorySegment);
			Assert.Equal(memSize, instr.MemorySize);

			Assert.Equal(OpKind.Register, instr.Op1Kind);
			Assert.Equal(reg2, instr.Op1Register);

			Assert.Equal(OpKind.Register, instr.Op2Kind);
			Assert.Equal(reg3, instr.Op2Register);
		}
		public static IEnumerable<object[]> Test64_Maskmovdqu_1_Data {
			get {
				yield return new object[] { "0FF7 D3", 3, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegRDI, Register.None, Register.DS };
				yield return new object[] { "67 0FF7 D3", 4, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegEDI, Register.None, Register.DS };
				yield return new object[] { "26 0FF7 D3", 4, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegRDI, Register.ES, Register.ES };
				yield return new object[] { "2E 0FF7 D3", 4, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegRDI, Register.CS, Register.CS };
				yield return new object[] { "36 0FF7 D3", 4, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegRDI, Register.SS, Register.SS };
				yield return new object[] { "3E 0FF7 D3", 4, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegRDI, Register.DS, Register.DS };
				yield return new object[] { "64 0FF7 D3", 4, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegRDI, Register.FS, Register.FS };
				yield return new object[] { "65 0FF7 D3", 4, Code.Maskmovq_rDI_mm_mm, Register.MM2, Register.MM3, MemorySize.UInt64, OpKind.MemorySegRDI, Register.GS, Register.GS };

				yield return new object[] { "66 0FF7 D3", 4, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegRDI, Register.None, Register.DS };
				yield return new object[] { "66 41 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM11, MemorySize.UInt128, OpKind.MemorySegRDI, Register.None, Register.DS };
				yield return new object[] { "66 44 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM10, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegRDI, Register.None, Register.DS };
				yield return new object[] { "66 45 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM10, Register.XMM11, MemorySize.UInt128, OpKind.MemorySegRDI, Register.None, Register.DS };
				yield return new object[] { "67 66 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegEDI, Register.None, Register.DS };
				yield return new object[] { "26 66 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegRDI, Register.ES, Register.ES };
				yield return new object[] { "2E 66 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegRDI, Register.CS, Register.CS };
				yield return new object[] { "36 66 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegRDI, Register.SS, Register.SS };
				yield return new object[] { "3E 66 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegRDI, Register.DS, Register.DS };
				yield return new object[] { "64 66 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegRDI, Register.FS, Register.FS };
				yield return new object[] { "65 66 0FF7 D3", 5, Code.Maskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegRDI, Register.GS, Register.GS };

				yield return new object[] { "C5F9 F7 D3", 4, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegRDI, Register.None, Register.DS };
				yield return new object[] { "C4C179 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM11, MemorySize.UInt128, OpKind.MemorySegRDI, Register.None, Register.DS };
				yield return new object[] { "C579 F7 D3", 4, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM10, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegRDI, Register.None, Register.DS };
				yield return new object[] { "C44179 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM10, Register.XMM11, MemorySize.UInt128, OpKind.MemorySegRDI, Register.None, Register.DS };
				yield return new object[] { "67 C5F9 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegEDI, Register.None, Register.DS };
				yield return new object[] { "26 C5F9 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegRDI, Register.ES, Register.ES };
				yield return new object[] { "2E C5F9 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegRDI, Register.CS, Register.CS };
				yield return new object[] { "36 C5F9 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegRDI, Register.SS, Register.SS };
				yield return new object[] { "3E C5F9 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegRDI, Register.DS, Register.DS };
				yield return new object[] { "64 C5F9 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegRDI, Register.FS, Register.FS };
				yield return new object[] { "65 C5F9 F7 D3", 5, Code.VEX_Vmaskmovdqu_rDI_xmm_xmm, Register.XMM2, Register.XMM3, MemorySize.UInt128, OpKind.MemorySegRDI, Register.GS, Register.GS };
			}
		}
	}
}
