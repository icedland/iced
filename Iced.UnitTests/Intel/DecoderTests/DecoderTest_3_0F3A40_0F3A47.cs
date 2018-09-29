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
	public sealed class DecoderTest_3_0F3A40_0F3A47 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_DotProdV_VX_WX_Ib_1_Data))]
		void Test16_DotProdV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_DotProdV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A40 08 A5", 6, Code.Dpps_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float32, 0xA5 };

				yield return new object[] { "66 0F3A41 08 A5", 6, Code.Dppd_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_DotProdV_VX_WX_Ib_2_Data))]
		void Test16_DotProdV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_DotProdV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A40 CD A5", 6, Code.Dpps_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "66 0F3A41 CD A5", 6, Code.Dppd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_DotProdV_VX_WX_Ib_1_Data))]
		void Test32_DotProdV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_DotProdV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A40 08 A5", 6, Code.Dpps_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float32, 0xA5 };

				yield return new object[] { "66 0F3A41 08 A5", 6, Code.Dppd_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_DotProdV_VX_WX_Ib_2_Data))]
		void Test32_DotProdV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_DotProdV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A40 CD A5", 6, Code.Dpps_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };

				yield return new object[] { "66 0F3A41 CD A5", 6, Code.Dppd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_DotProdV_VX_WX_Ib_1_Data))]
		void Test64_DotProdV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_DotProdV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A40 08 A5", 6, Code.Dpps_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float32, 0xA5 };

				yield return new object[] { "66 0F3A41 08 A5", 6, Code.Dppd_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_Float64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_DotProdV_VX_WX_Ib_2_Data))]
		void Test64_DotProdV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_DotProdV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A40 CD A5", 6, Code.Dpps_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0F3A40 CD 5A", 7, Code.Dpps_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0x5A };
				yield return new object[] { "66 41 0F3A40 CD A5", 7, Code.Dpps_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0F3A40 CD 5A", 7, Code.Dpps_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0x5A };

				yield return new object[] { "66 0F3A41 CD A5", 6, Code.Dppd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0F3A41 CD 5A", 7, Code.Dppd_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0x5A };
				yield return new object[] { "66 41 0F3A41 CD A5", 7, Code.Dppd_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0F3A41 CD 5A", 7, Code.Dppd_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VdpV_VX_HX_WX_Ib_1_Data))]
		void Test16_VdpV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VdpV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 40 10 A5", 6, Code.VEX_Vdpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C4E34D 40 10 5A", 6, Code.VEX_Vdpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0x5A };
				yield return new object[] { "C4E3C9 40 10 A5", 6, Code.VEX_Vdpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C4E3CD 40 10 5A", 6, Code.VEX_Vdpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0x5A };

				yield return new object[] { "C4E349 41 10 A5", 6, Code.VEX_Vdppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C4E3C9 41 10 5A", 6, Code.VEX_Vdppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VdpV_VX_HX_WX_Ib_2_Data))]
		void Test16_VdpV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VdpV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 40 D3 A5", 6, Code.VEX_Vdpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E34D 40 D3 5A", 6, Code.VEX_Vdpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0x5A };

				yield return new object[] { "C4E349 41 D3 A5", 6, Code.VEX_Vdppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VdpV_VX_HX_WX_Ib_1_Data))]
		void Test32_VdpV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VdpV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 40 10 A5", 6, Code.VEX_Vdpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C4E34D 40 10 5A", 6, Code.VEX_Vdpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0x5A };
				yield return new object[] { "C4E3C9 40 10 A5", 6, Code.VEX_Vdpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C4E3CD 40 10 5A", 6, Code.VEX_Vdpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0x5A };

				yield return new object[] { "C4E349 41 10 A5", 6, Code.VEX_Vdppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C4E3C9 41 10 5A", 6, Code.VEX_Vdppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VdpV_VX_HX_WX_Ib_2_Data))]
		void Test32_VdpV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VdpV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 40 D3 A5", 6, Code.VEX_Vdpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E34D 40 D3 5A", 6, Code.VEX_Vdpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0x5A };

				yield return new object[] { "C4E349 41 D3 A5", 6, Code.VEX_Vdppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VdpV_VX_HX_WX_Ib_1_Data))]
		void Test64_VdpV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VdpV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 40 10 A5", 6, Code.VEX_Vdpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C4E34D 40 10 5A", 6, Code.VEX_Vdpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0x5A };
				yield return new object[] { "C4E3C9 40 10 A5", 6, Code.VEX_Vdpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float32, 0xA5 };
				yield return new object[] { "C4E3CD 40 10 5A", 6, Code.VEX_Vdpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Float32, 0x5A };

				yield return new object[] { "C4E349 41 10 A5", 6, Code.VEX_Vdppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0xA5 };
				yield return new object[] { "C4E3C9 41 10 5A", 6, Code.VEX_Vdppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_Float64, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VdpV_VX_HX_WX_Ib_2_Data))]
		void Test64_VdpV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VdpV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 40 D3 A5", 6, Code.VEX_Vdpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E34D 40 D3 5A", 6, Code.VEX_Vdpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0x5A };
				yield return new object[] { "C46349 40 D3 A5", 6, Code.VEX_Vdpps_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4634D 40 D3 5A", 6, Code.VEX_Vdpps_ymm_ymm_ymmm256_imm8, Register.YMM10, Register.YMM6, Register.YMM3, 0x5A };
				yield return new object[] { "C4E309 40 D3 A5", 6, Code.VEX_Vdpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E30D 40 D3 5A", 6, Code.VEX_Vdpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM14, Register.YMM3, 0x5A };
				yield return new object[] { "C4C349 40 D3 A5", 6, Code.VEX_Vdpps_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };
				yield return new object[] { "C4C34D 40 D3 5A", 6, Code.VEX_Vdpps_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM11, 0x5A };

				yield return new object[] { "C4E349 41 D3 A5", 6, Code.VEX_Vdppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C46349 41 D3 5A", 6, Code.VEX_Vdppd_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0x5A };
				yield return new object[] { "C4E309 41 D3 A5", 6, Code.VEX_Vdppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C349 41 D3 5A", 6, Code.VEX_Vdppd_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MpsadbwV_VX_WX_Ib_1_Data))]
		void Test16_MpsadbwV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_MpsadbwV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A42 08 A5", 6, Code.Mpsadbw_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_MpsadbwV_VX_WX_Ib_2_Data))]
		void Test16_MpsadbwV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_MpsadbwV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A42 CD A5", 6, Code.Mpsadbw_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MpsadbwV_VX_WX_Ib_1_Data))]
		void Test32_MpsadbwV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_MpsadbwV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A42 08 A5", 6, Code.Mpsadbw_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_MpsadbwV_VX_WX_Ib_2_Data))]
		void Test32_MpsadbwV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_MpsadbwV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A42 CD A5", 6, Code.Mpsadbw_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MpsadbwV_VX_WX_Ib_1_Data))]
		void Test64_MpsadbwV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_MpsadbwV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A42 08 A5", 6, Code.Mpsadbw_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_MpsadbwV_VX_WX_Ib_2_Data))]
		void Test64_MpsadbwV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_MpsadbwV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A42 CD A5", 6, Code.Mpsadbw_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0F3A42 CD A5", 7, Code.Mpsadbw_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0xA5 };
				yield return new object[] { "66 41 0F3A42 CD A5", 7, Code.Mpsadbw_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0F3A42 CD A5", 7, Code.Mpsadbw_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VmpsadbwV_VX_HX_WX_Ib_1_Data))]
		void Test16_VmpsadbwV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VmpsadbwV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 42 10 A5", 6, Code.VEX_Vmpsadbw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "C4E34D 42 10 A5", 6, Code.VEX_Vmpsadbw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8, 0xA5 };
				yield return new object[] { "C4E3C9 42 10 A5", 6, Code.VEX_Vmpsadbw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "C4E3CD 42 10 A5", 6, Code.VEX_Vmpsadbw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VmpsadbwV_VX_HX_WX_Ib_2_Data))]
		void Test16_VmpsadbwV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_VmpsadbwV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 42 D3 A5", 6, Code.VEX_Vmpsadbw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E34D 42 D3 A5", 6, Code.VEX_Vmpsadbw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VmpsadbwV_VX_HX_WX_Ib_1_Data))]
		void Test32_VmpsadbwV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VmpsadbwV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 42 10 A5", 6, Code.VEX_Vmpsadbw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "C4E34D 42 10 A5", 6, Code.VEX_Vmpsadbw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8, 0xA5 };
				yield return new object[] { "C4E3C9 42 10 A5", 6, Code.VEX_Vmpsadbw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "C4E3CD 42 10 A5", 6, Code.VEX_Vmpsadbw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VmpsadbwV_VX_HX_WX_Ib_2_Data))]
		void Test32_VmpsadbwV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_VmpsadbwV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 42 D3 A5", 6, Code.VEX_Vmpsadbw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E34D 42 D3 A5", 6, Code.VEX_Vmpsadbw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VmpsadbwV_VX_HX_WX_Ib_1_Data))]
		void Test64_VmpsadbwV_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VmpsadbwV_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 42 10 A5", 6, Code.VEX_Vmpsadbw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "C4E34D 42 10 A5", 6, Code.VEX_Vmpsadbw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8, 0xA5 };
				yield return new object[] { "C4E3C9 42 10 A5", 6, Code.VEX_Vmpsadbw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt8, 0xA5 };
				yield return new object[] { "C4E3CD 42 10 A5", 6, Code.VEX_Vmpsadbw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt8, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VmpsadbwV_VX_HX_WX_Ib_2_Data))]
		void Test64_VmpsadbwV_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_VmpsadbwV_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 42 D3 A5", 6, Code.VEX_Vmpsadbw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E34D 42 D3 A5", 6, Code.VEX_Vmpsadbw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C46349 42 D3 A5", 6, Code.VEX_Vmpsadbw_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4634D 42 D3 A5", 6, Code.VEX_Vmpsadbw_ymm_ymm_ymmm256_imm8, Register.YMM10, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C4E309 42 D3 A5", 6, Code.VEX_Vmpsadbw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E30D 42 D3 A5", 6, Code.VEX_Vmpsadbw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM14, Register.YMM3, 0xA5 };
				yield return new object[] { "C4C349 42 D3 A5", 6, Code.VEX_Vmpsadbw_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };
				yield return new object[] { "C4C34D 42 D3 A5", 6, Code.VEX_Vmpsadbw_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VdbpsadbwV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test16_VdbpsadbwV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VdbpsadbwV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false, 0xA5 };
				yield return new object[] { "62 F34D8D 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true, 0xA5 };
				yield return new object[] { "62 F34D08 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false, 0xA5 };
				yield return new object[] { "62 F34DAD 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true, 0xA5 };
				yield return new object[] { "62 F34D28 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
				yield return new object[] { "62 F34DCD 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true, 0xA5 };
				yield return new object[] { "62 F34D48 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VdbpsadbwV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test16_VdbpsadbwV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VdbpsadbwV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D8B 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F34D2B 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DAB 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F34D4B 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DCB 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VdbpsadbwV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test32_VdbpsadbwV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VdbpsadbwV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false, 0xA5 };
				yield return new object[] { "62 F34D8D 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true, 0xA5 };
				yield return new object[] { "62 F34D08 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false, 0xA5 };
				yield return new object[] { "62 F34DAD 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true, 0xA5 };
				yield return new object[] { "62 F34D28 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
				yield return new object[] { "62 F34DCD 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true, 0xA5 };
				yield return new object[] { "62 F34D48 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VdbpsadbwV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test32_VdbpsadbwV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VdbpsadbwV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34D8B 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F34D2B 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DAB 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F34D4B 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DCB 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VdbpsadbwV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test64_VdbpsadbwV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VdbpsadbwV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt8, 16, false, 0xA5 };
				yield return new object[] { "62 F34D8D 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt8, 16, true, 0xA5 };
				yield return new object[] { "62 F34D08 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt8, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt8, 32, false, 0xA5 };
				yield return new object[] { "62 F34DAD 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt8, 32, true, 0xA5 };
				yield return new object[] { "62 F34D28 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt8, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
				yield return new object[] { "62 F34DCD 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt8, 64, true, 0xA5 };
				yield return new object[] { "62 F34D48 42 50 01 A5", 8, Code.EVEX_Vdbpsadbw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt8, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VdbpsadbwV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test64_VdbpsadbwV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VdbpsadbwV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30D8B 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 134D03 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D0B 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D2B 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30DAB 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 134D23 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D2B 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D4B 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30DCB 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 134D43 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D4B 42 D3 A5", 7, Code.EVEX_Vdbpsadbw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vshufi32x4V_VX_k1z_HX_WX_Ib_1_Data))]
		void Test16_Vshufi32x4V_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vshufi32x4V_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D2B 43 50 01 A5", 8, Code.EVEX_Vshufi32x4_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 43 50 01 A5", 8, Code.EVEX_Vshufi32x4_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 43 50 01 A5", 8, Code.EVEX_Vshufi32x4_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 43 50 01 A5", 8, Code.EVEX_Vshufi32x4_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 43 50 01 A5", 8, Code.EVEX_Vshufi32x4_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 43 50 01 A5", 8, Code.EVEX_Vshufi32x4_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 43 50 01 A5", 8, Code.EVEX_Vshufi64x2_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 43 50 01 A5", 8, Code.EVEX_Vshufi64x2_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 43 50 01 A5", 8, Code.EVEX_Vshufi64x2_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 43 50 01 A5", 8, Code.EVEX_Vshufi64x2_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 43 50 01 A5", 8, Code.EVEX_Vshufi64x2_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 43 50 01 A5", 8, Code.EVEX_Vshufi64x2_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vshufi32x4V_VX_k1z_HX_WX_Ib_2_Data))]
		void Test16_Vshufi32x4V_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vshufi32x4V_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D2B 43 D3 A5", 7, Code.EVEX_Vshufi32x4_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DAB 43 D3 A5", 7, Code.EVEX_Vshufi32x4_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D4B 43 D3 A5", 7, Code.EVEX_Vshufi32x4_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DCB 43 D3 A5", 7, Code.EVEX_Vshufi32x4_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F3CDAB 43 D3 A5", 7, Code.EVEX_Vshufi64x2_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD2B 43 D3 A5", 7, Code.EVEX_Vshufi64x2_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB 43 D3 A5", 7, Code.EVEX_Vshufi64x2_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD4B 43 D3 A5", 7, Code.EVEX_Vshufi64x2_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vshufi32x4V_VX_k1z_HX_WX_Ib_1_Data))]
		void Test32_Vshufi32x4V_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vshufi32x4V_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D2B 43 50 01 A5", 8, Code.EVEX_Vshufi32x4_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 43 50 01 A5", 8, Code.EVEX_Vshufi32x4_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 43 50 01 A5", 8, Code.EVEX_Vshufi32x4_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 43 50 01 A5", 8, Code.EVEX_Vshufi32x4_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 43 50 01 A5", 8, Code.EVEX_Vshufi32x4_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 43 50 01 A5", 8, Code.EVEX_Vshufi32x4_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 43 50 01 A5", 8, Code.EVEX_Vshufi64x2_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 43 50 01 A5", 8, Code.EVEX_Vshufi64x2_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 43 50 01 A5", 8, Code.EVEX_Vshufi64x2_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 43 50 01 A5", 8, Code.EVEX_Vshufi64x2_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 43 50 01 A5", 8, Code.EVEX_Vshufi64x2_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 43 50 01 A5", 8, Code.EVEX_Vshufi64x2_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vshufi32x4V_VX_k1z_HX_WX_Ib_2_Data))]
		void Test32_Vshufi32x4V_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vshufi32x4V_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D2B 43 D3 A5", 7, Code.EVEX_Vshufi32x4_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DAB 43 D3 A5", 7, Code.EVEX_Vshufi32x4_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D4B 43 D3 A5", 7, Code.EVEX_Vshufi32x4_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DCB 43 D3 A5", 7, Code.EVEX_Vshufi32x4_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F3CDAB 43 D3 A5", 7, Code.EVEX_Vshufi64x2_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD2B 43 D3 A5", 7, Code.EVEX_Vshufi64x2_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB 43 D3 A5", 7, Code.EVEX_Vshufi64x2_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD4B 43 D3 A5", 7, Code.EVEX_Vshufi64x2_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vshufi32x4V_VX_k1z_HX_WX_Ib_1_Data))]
		void Test64_Vshufi32x4V_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vshufi32x4V_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D2B 43 50 01 A5", 8, Code.EVEX_Vshufi32x4_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 43 50 01 A5", 8, Code.EVEX_Vshufi32x4_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 43 50 01 A5", 8, Code.EVEX_Vshufi32x4_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 43 50 01 A5", 8, Code.EVEX_Vshufi32x4_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 43 50 01 A5", 8, Code.EVEX_Vshufi32x4_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 43 50 01 A5", 8, Code.EVEX_Vshufi32x4_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 43 50 01 A5", 8, Code.EVEX_Vshufi64x2_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 43 50 01 A5", 8, Code.EVEX_Vshufi64x2_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 43 50 01 A5", 8, Code.EVEX_Vshufi64x2_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 43 50 01 A5", 8, Code.EVEX_Vshufi64x2_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 43 50 01 A5", 8, Code.EVEX_Vshufi64x2_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 43 50 01 A5", 8, Code.EVEX_Vshufi64x2_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vshufi32x4V_VX_k1z_HX_WX_Ib_2_Data))]
		void Test64_Vshufi32x4V_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vshufi32x4V_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D2B 43 D3 A5", 7, Code.EVEX_Vshufi32x4_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 E30D2B 43 D3 A5", 7, Code.EVEX_Vshufi32x4_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 134D23 43 D3 A5", 7, Code.EVEX_Vshufi32x4_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D2B 43 D3 A5", 7, Code.EVEX_Vshufi32x4_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DAB 43 D3 A5", 7, Code.EVEX_Vshufi32x4_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D4B 43 D3 A5", 7, Code.EVEX_Vshufi32x4_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 E30D4B 43 D3 A5", 7, Code.EVEX_Vshufi32x4_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 134D43 43 D3 A5", 7, Code.EVEX_Vshufi32x4_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D4B 43 D3 A5", 7, Code.EVEX_Vshufi32x4_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DCB 43 D3 A5", 7, Code.EVEX_Vshufi32x4_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F3CDAB 43 D3 A5", 7, Code.EVEX_Vshufi64x2_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D2B 43 D3 A5", 7, Code.EVEX_Vshufi64x2_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD23 43 D3 A5", 7, Code.EVEX_Vshufi64x2_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD2B 43 D3 A5", 7, Code.EVEX_Vshufi64x2_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD2B 43 D3 A5", 7, Code.EVEX_Vshufi64x2_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB 43 D3 A5", 7, Code.EVEX_Vshufi64x2_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D4B 43 D3 A5", 7, Code.EVEX_Vshufi64x2_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD43 43 D3 A5", 7, Code.EVEX_Vshufi64x2_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD4B 43 D3 A5", 7, Code.EVEX_Vshufi64x2_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD4B 43 D3 A5", 7, Code.EVEX_Vshufi64x2_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PclmulqdqV_VX_WX_Ib_1_Data))]
		void Test16_PclmulqdqV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.BX, instr.MemoryBase);
			Assert.Equal(Register.SI, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_PclmulqdqV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A44 08 A5", 6, Code.Pclmulqdq_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_PclmulqdqV_VX_WX_Ib_2_Data))]
		void Test16_PclmulqdqV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_PclmulqdqV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A44 CD A5", 6, Code.Pclmulqdq_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PclmulqdqV_VX_WX_Ib_1_Data))]
		void Test32_PclmulqdqV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.EAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_PclmulqdqV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A44 08 A5", 6, Code.Pclmulqdq_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_PclmulqdqV_VX_WX_Ib_2_Data))]
		void Test32_PclmulqdqV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_PclmulqdqV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A44 CD A5", 6, Code.Pclmulqdq_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PclmulqdqV_VX_WX_Ib_1_Data))]
		void Test64_PclmulqdqV_VX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
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
			Assert.Equal(reg, instr.Op0Register);

			Assert.Equal(OpKind.Memory, instr.Op1Kind);
			Assert.Equal(Register.DS, instr.MemorySegment);
			Assert.Equal(Register.RAX, instr.MemoryBase);
			Assert.Equal(Register.None, instr.MemoryIndex);
			Assert.Equal(0U, instr.MemoryDisplacement);
			Assert.Equal(1, instr.MemoryIndexScale);
			Assert.Equal(memSize, instr.MemorySize);
			Assert.Equal(0, instr.MemoryDisplSize);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_PclmulqdqV_VX_WX_Ib_1_Data {
			get {
				yield return new object[] { "66 0F3A44 08 A5", 6, Code.Pclmulqdq_xmm_xmmm128_imm8, Register.XMM1, MemorySize.Packed128_UInt64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_PclmulqdqV_VX_WX_Ib_2_Data))]
		void Test64_PclmulqdqV_VX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_PclmulqdqV_VX_WX_Ib_2_Data {
			get {
				yield return new object[] { "66 0F3A44 CD A5", 6, Code.Pclmulqdq_xmm_xmmm128_imm8, Register.XMM1, Register.XMM5, 0xA5 };
				yield return new object[] { "66 44 0F3A44 CD 5A", 7, Code.Pclmulqdq_xmm_xmmm128_imm8, Register.XMM9, Register.XMM5, 0x5A };
				yield return new object[] { "66 41 0F3A44 CD A5", 7, Code.Pclmulqdq_xmm_xmmm128_imm8, Register.XMM1, Register.XMM13, 0xA5 };
				yield return new object[] { "66 45 0F3A44 CD 5A", 7, Code.Pclmulqdq_xmm_xmmm128_imm8, Register.XMM9, Register.XMM13, 0x5A };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpclmulqdq_VX_HX_WX_Ib_1_Data))]
		void Test16_Vpclmulqdq_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Vpclmulqdq_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 44 10 A5", 6, Code.VEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64, 0xA5 };
				yield return new object[] { "C4E3C9 44 10 A5", 6, Code.VEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64, 0xA5 };

				yield return new object[] { "C4E34D 44 10 A5", 6, Code.VEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64, 0xA5 };
				yield return new object[] { "C4E3CD 44 10 A5", 6, Code.VEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vpclmulqdq_VX_HX_WX_Ib_2_Data))]
		void Test16_Vpclmulqdq_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Vpclmulqdq_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 44 D3 A5", 6, Code.VEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };

				yield return new object[] { "C4E34D 44 D3 A5", 6, Code.VEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpclmulqdq_VX_HX_WX_Ib_1_Data))]
		void Test32_Vpclmulqdq_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Vpclmulqdq_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 44 10 A5", 6, Code.VEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64, 0xA5 };
				yield return new object[] { "C4E3C9 44 10 A5", 6, Code.VEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64, 0xA5 };

				yield return new object[] { "C4E34D 44 10 A5", 6, Code.VEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64, 0xA5 };
				yield return new object[] { "C4E3CD 44 10 A5", 6, Code.VEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vpclmulqdq_VX_HX_WX_Ib_2_Data))]
		void Test32_Vpclmulqdq_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Vpclmulqdq_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 44 D3 A5", 6, Code.VEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };

				yield return new object[] { "C4E34D 44 D3 A5", 6, Code.VEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpclmulqdq_VX_HX_WX_Ib_1_Data))]
		void Test64_Vpclmulqdq_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Vpclmulqdq_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E349 44 10 A5", 6, Code.VEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64, 0xA5 };
				yield return new object[] { "C4E3C9 44 10 A5", 6, Code.VEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, MemorySize.Packed128_UInt64, 0xA5 };

				yield return new object[] { "C4E34D 44 10 A5", 6, Code.VEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64, 0xA5 };
				yield return new object[] { "C4E3CD 44 10 A5", 6, Code.VEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_UInt64, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vpclmulqdq_VX_HX_WX_Ib_2_Data))]
		void Test64_Vpclmulqdq_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Vpclmulqdq_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E349 44 D3 A5", 6, Code.VEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C46349 44 D3 A5", 6, Code.VEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E309 44 D3 A5", 6, Code.VEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C349 44 D3 A5", 6, Code.VEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM11, 0xA5 };

				yield return new object[] { "C4E34D 44 D3 A5", 6, Code.VEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C4634D 44 D3 A5", 6, Code.VEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM10, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C4E30D 44 D3 A5", 6, Code.VEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM14, Register.YMM3, 0xA5 };
				yield return new object[] { "C4C34D 44 D3 A5", 6, Code.VEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpclmulqdqV_EVEX_VX_HX_WX_Ib_1_Data))]
		void Test16_VpclmulqdqV_EVEX_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpclmulqdqV_EVEX_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D08 44 50 01 A5", 8, Code.EVEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, 0xA5, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F34D28 44 50 01 A5", 8, Code.EVEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, 0xA5, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F34D48 44 50 01 A5", 8, Code.EVEX_Vpclmulqdq_zmm_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, 0xA5, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpclmulqdqV_EVEX_VX_HX_WX_Ib_2_Data))]
		void Test16_VpclmulqdqV_EVEX_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VpclmulqdqV_EVEX_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D08 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F3CD08 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F34D28 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F3CD28 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F34D48 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_zmm_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, 0xA5, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F3CD48 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_zmm_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, 0xA5, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpclmulqdqV_EVEX_VX_HX_WX_Ib_1_Data))]
		void Test32_VpclmulqdqV_EVEX_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpclmulqdqV_EVEX_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D08 44 50 01 A5", 8, Code.EVEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, 0xA5, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F34D28 44 50 01 A5", 8, Code.EVEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, 0xA5, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F34D48 44 50 01 A5", 8, Code.EVEX_Vpclmulqdq_zmm_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, 0xA5, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpclmulqdqV_EVEX_VX_HX_WX_Ib_2_Data))]
		void Test32_VpclmulqdqV_EVEX_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VpclmulqdqV_EVEX_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D08 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F3CD08 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F34D28 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F3CD28 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F34D48 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_zmm_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, 0xA5, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F3CD48 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_zmm_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, 0xA5, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpclmulqdqV_EVEX_VX_HX_WX_Ib_1_Data))]
		void Test64_VpclmulqdqV_EVEX_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate, Register kreg, MemorySize memSize, uint displ, bool z) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpclmulqdqV_EVEX_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D08 44 50 01 A5", 8, Code.EVEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, 0xA5, Register.None, MemorySize.Packed128_UInt64, 16, false };

				yield return new object[] { "62 F34D28 44 50 01 A5", 8, Code.EVEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, 0xA5, Register.None, MemorySize.Packed256_UInt64, 32, false };

				yield return new object[] { "62 F34D48 44 50 01 A5", 8, Code.EVEX_Vpclmulqdq_zmm_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, 0xA5, Register.None, MemorySize.Packed512_UInt64, 64, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpclmulqdqV_EVEX_VX_HX_WX_Ib_2_Data))]
		void Test64_VpclmulqdqV_EVEX_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate, Register kreg, RoundingControl rc, bool z, bool sae) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VpclmulqdqV_EVEX_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D08 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E30D08 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM18, Register.XMM14, Register.XMM3, 0xA5, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 134D00 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM10, Register.XMM22, Register.XMM27, 0xA5, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B34D08 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM19, 0xA5, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F3CD08 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_xmm_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, 0xA5, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F34D28 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E30D28 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM18, Register.YMM14, Register.YMM3, 0xA5, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 134D20 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM10, Register.YMM22, Register.YMM27, 0xA5, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B34D28 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM19, 0xA5, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F3CD28 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5, Register.None, RoundingControl.None, false, false };

				yield return new object[] { "62 F34D48 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_zmm_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, 0xA5, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 E30D48 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_zmm_zmm_zmmm512_imm8, Register.ZMM18, Register.ZMM14, Register.ZMM3, 0xA5, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 134D40 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_zmm_zmm_zmmm512_imm8, Register.ZMM10, Register.ZMM22, Register.ZMM27, 0xA5, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 B34D48 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_zmm_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM19, 0xA5, Register.None, RoundingControl.None, false, false };
				yield return new object[] { "62 F3CD48 44 D3 A5", 7, Code.EVEX_Vpclmulqdq_zmm_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, 0xA5, Register.None, RoundingControl.None, false, false };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vperm2i128V_VX_HX_WX_Ib_1_Data))]
		void Test16_Vperm2i128V_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Vperm2i128V_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E34D 46 10 A5", 6, Code.VEX_Vperm2i128_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int128, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vperm2i128V_VX_HX_WX_Ib_2_Data))]
		void Test16_Vperm2i128V_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Vperm2i128V_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E34D 46 D3 A5", 6, Code.VEX_Vperm2i128_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vperm2i128V_VX_HX_WX_Ib_1_Data))]
		void Test32_Vperm2i128V_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Vperm2i128V_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E34D 46 10 A5", 6, Code.VEX_Vperm2i128_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int128, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vperm2i128V_VX_HX_WX_Ib_2_Data))]
		void Test32_Vperm2i128V_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Vperm2i128V_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E34D 46 D3 A5", 6, Code.VEX_Vperm2i128_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vperm2i128V_VX_HX_WX_Ib_1_Data))]
		void Test64_Vperm2i128V_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Vperm2i128V_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E34D 46 10 A5", 6, Code.VEX_Vperm2i128_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, MemorySize.Packed256_Int128, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vperm2i128V_VX_HX_WX_Ib_2_Data))]
		void Test64_Vperm2i128V_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(4, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op3Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Vperm2i128V_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E34D 46 D3 A5", 6, Code.VEX_Vperm2i128_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C4634D 46 D3 A5", 6, Code.VEX_Vperm2i128_ymm_ymm_ymmm256_imm8, Register.YMM10, Register.YMM6, Register.YMM3, 0xA5 };
				yield return new object[] { "C4E30D 46 D3 A5", 6, Code.VEX_Vperm2i128_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM14, Register.YMM3, 0xA5 };
				yield return new object[] { "C4C34D 46 D3 A5", 6, Code.VEX_Vperm2i128_ymm_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM11, 0xA5 };
			}
		}
	}
}
