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
	public sealed class DecoderTest_3_0F3A18_0F3A1F : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_Vinsertf128V_VX_HX_WX_Ib_1_Data))]
		void Test16_Vinsertf128V_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Vinsertf128V_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E34D 18 10 A5", 6, Code.VEX_Vinsertf128_ymm_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, MemorySize.Float128, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vinsertf128V_VX_HX_WX_Ib_2_Data))]
		void Test16_Vinsertf128V_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Vinsertf128V_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E34D 18 D3 A5", 6, Code.VEX_Vinsertf128_ymm_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vinsertf128V_VX_HX_WX_Ib_1_Data))]
		void Test32_Vinsertf128V_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Vinsertf128V_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E34D 18 10 A5", 6, Code.VEX_Vinsertf128_ymm_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, MemorySize.Float128, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vinsertf128V_VX_HX_WX_Ib_2_Data))]
		void Test32_Vinsertf128V_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Vinsertf128V_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E34D 18 D3 A5", 6, Code.VEX_Vinsertf128_ymm_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vinsertf128V_VX_HX_WX_Ib_1_Data))]
		void Test64_Vinsertf128V_VX_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, MemorySize memSize, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Vinsertf128V_VX_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "C4E34D 18 10 A5", 6, Code.VEX_Vinsertf128_ymm_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, MemorySize.Float128, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vinsertf128V_VX_HX_WX_Ib_2_Data))]
		void Test64_Vinsertf128V_VX_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Vinsertf128V_VX_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "C4E34D 18 D3 A5", 6, Code.VEX_Vinsertf128_ymm_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4634D 18 D3 A5", 6, Code.VEX_Vinsertf128_ymm_ymm_xmmm128_imm8, Register.YMM10, Register.YMM6, Register.XMM3, 0xA5 };
				yield return new object[] { "C4E30D 18 D3 A5", 6, Code.VEX_Vinsertf128_ymm_ymm_xmmm128_imm8, Register.YMM2, Register.YMM14, Register.XMM3, 0xA5 };
				yield return new object[] { "C4C34D 18 D3 A5", 6, Code.VEX_Vinsertf128_ymm_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM11, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vinsertf32x4V_VX_k1z_HX_WX_Ib_1_Data))]
		void Test16_Vinsertf32x4V_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Vinsertf32x4V_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D2B 18 50 01 A5", 8, Code.EVEX_Vinsertf32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F34DAD 18 50 01 A5", 8, Code.EVEX_Vinsertf32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_Float32, 16, true, 0xA5 };
				yield return new object[] { "62 F34D28 18 50 01 A5", 8, Code.EVEX_Vinsertf32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D4B 18 50 01 A5", 8, Code.EVEX_Vinsertf32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F34DCD 18 50 01 A5", 8, Code.EVEX_Vinsertf32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_Float32, 16, true, 0xA5 };
				yield return new object[] { "62 F34D48 18 50 01 A5", 8, Code.EVEX_Vinsertf32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 18 50 01 A5", 8, Code.EVEX_Vinsertf64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CDAD 18 50 01 A5", 8, Code.EVEX_Vinsertf64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_Float64, 16, true, 0xA5 };
				yield return new object[] { "62 F3CD28 18 50 01 A5", 8, Code.EVEX_Vinsertf64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 18 50 01 A5", 8, Code.EVEX_Vinsertf64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CDCD 18 50 01 A5", 8, Code.EVEX_Vinsertf64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_Float64, 16, true, 0xA5 };
				yield return new object[] { "62 F3CD48 18 50 01 A5", 8, Code.EVEX_Vinsertf64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vinsertf32x4V_VX_k1z_HX_WX_Ib_2_Data))]
		void Test16_Vinsertf32x4V_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Vinsertf32x4V_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D2B 18 D3 A5", 7, Code.EVEX_Vinsertf32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DAB 18 D3 A5", 7, Code.EVEX_Vinsertf32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F34D4B 18 D3 A5", 7, Code.EVEX_Vinsertf32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DCB 18 D3 A5", 7, Code.EVEX_Vinsertf32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 18 D3 A5", 7, Code.EVEX_Vinsertf64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDAB 18 D3 A5", 7, Code.EVEX_Vinsertf64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 18 D3 A5", 7, Code.EVEX_Vinsertf64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDCB 18 D3 A5", 7, Code.EVEX_Vinsertf64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vinsertf32x4V_VX_k1z_HX_WX_Ib_1_Data))]
		void Test32_Vinsertf32x4V_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Vinsertf32x4V_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D2B 18 50 01 A5", 8, Code.EVEX_Vinsertf32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F34DAD 18 50 01 A5", 8, Code.EVEX_Vinsertf32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_Float32, 16, true, 0xA5 };
				yield return new object[] { "62 F34D28 18 50 01 A5", 8, Code.EVEX_Vinsertf32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D4B 18 50 01 A5", 8, Code.EVEX_Vinsertf32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F34DCD 18 50 01 A5", 8, Code.EVEX_Vinsertf32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_Float32, 16, true, 0xA5 };
				yield return new object[] { "62 F34D48 18 50 01 A5", 8, Code.EVEX_Vinsertf32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 18 50 01 A5", 8, Code.EVEX_Vinsertf64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CDAD 18 50 01 A5", 8, Code.EVEX_Vinsertf64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_Float64, 16, true, 0xA5 };
				yield return new object[] { "62 F3CD28 18 50 01 A5", 8, Code.EVEX_Vinsertf64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 18 50 01 A5", 8, Code.EVEX_Vinsertf64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CDCD 18 50 01 A5", 8, Code.EVEX_Vinsertf64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_Float64, 16, true, 0xA5 };
				yield return new object[] { "62 F3CD48 18 50 01 A5", 8, Code.EVEX_Vinsertf64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vinsertf32x4V_VX_k1z_HX_WX_Ib_2_Data))]
		void Test32_Vinsertf32x4V_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Vinsertf32x4V_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D2B 18 D3 A5", 7, Code.EVEX_Vinsertf32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DAB 18 D3 A5", 7, Code.EVEX_Vinsertf32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F34D4B 18 D3 A5", 7, Code.EVEX_Vinsertf32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DCB 18 D3 A5", 7, Code.EVEX_Vinsertf32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 18 D3 A5", 7, Code.EVEX_Vinsertf64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDAB 18 D3 A5", 7, Code.EVEX_Vinsertf64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 18 D3 A5", 7, Code.EVEX_Vinsertf64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDCB 18 D3 A5", 7, Code.EVEX_Vinsertf64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vinsertf32x4V_VX_k1z_HX_WX_Ib_1_Data))]
		void Test64_Vinsertf32x4V_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Vinsertf32x4V_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D2B 18 50 01 A5", 8, Code.EVEX_Vinsertf32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F34DAD 18 50 01 A5", 8, Code.EVEX_Vinsertf32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_Float32, 16, true, 0xA5 };
				yield return new object[] { "62 F34D28 18 50 01 A5", 8, Code.EVEX_Vinsertf32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D4B 18 50 01 A5", 8, Code.EVEX_Vinsertf32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F34DCD 18 50 01 A5", 8, Code.EVEX_Vinsertf32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_Float32, 16, true, 0xA5 };
				yield return new object[] { "62 F34D48 18 50 01 A5", 8, Code.EVEX_Vinsertf32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 18 50 01 A5", 8, Code.EVEX_Vinsertf64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CDAD 18 50 01 A5", 8, Code.EVEX_Vinsertf64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed128_Float64, 16, true, 0xA5 };
				yield return new object[] { "62 F3CD28 18 50 01 A5", 8, Code.EVEX_Vinsertf64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 18 50 01 A5", 8, Code.EVEX_Vinsertf64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CDCD 18 50 01 A5", 8, Code.EVEX_Vinsertf64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed128_Float64, 16, true, 0xA5 };
				yield return new object[] { "62 F3CD48 18 50 01 A5", 8, Code.EVEX_Vinsertf64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vinsertf32x4V_VX_k1z_HX_WX_Ib_2_Data))]
		void Test64_Vinsertf32x4V_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Vinsertf32x4V_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D2B 18 D3 A5", 7, Code.EVEX_Vinsertf32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30DAB 18 D3 A5", 7, Code.EVEX_Vinsertf32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM18, Register.YMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 134D23 18 D3 A5", 7, Code.EVEX_Vinsertf32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM10, Register.YMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D2B 18 D3 A5", 7, Code.EVEX_Vinsertf32x4_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F34D4B 18 D3 A5", 7, Code.EVEX_Vinsertf32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30DCB 18 D3 A5", 7, Code.EVEX_Vinsertf32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM18, Register.ZMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 134D43 18 D3 A5", 7, Code.EVEX_Vinsertf32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM10, Register.ZMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D4B 18 D3 A5", 7, Code.EVEX_Vinsertf32x4_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 18 D3 A5", 7, Code.EVEX_Vinsertf64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E38DAB 18 D3 A5", 7, Code.EVEX_Vinsertf64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM18, Register.YMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 13CD23 18 D3 A5", 7, Code.EVEX_Vinsertf64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM10, Register.YMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD2B 18 D3 A5", 7, Code.EVEX_Vinsertf64x2_ymm_k1z_ymm_xmmm128_imm8, Register.YMM2, Register.YMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 18 D3 A5", 7, Code.EVEX_Vinsertf64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E38DCB 18 D3 A5", 7, Code.EVEX_Vinsertf64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM18, Register.ZMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 13CD43 18 D3 A5", 7, Code.EVEX_Vinsertf64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM10, Register.ZMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD4B 18 D3 A5", 7, Code.EVEX_Vinsertf64x2_zmm_k1z_zmm_xmmm128_imm8, Register.ZMM2, Register.ZMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vextractf128V_WX_VY_Ib_1_Data))]
		void Test16_Vextractf128V_WX_VY_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Vextractf128V_WX_VY_Ib_1_Data {
			get {
				yield return new object[] { "C4E37D 19 10 A5", 6, Code.VEX_Vextractf128_xmmm128_ymm_imm8, Register.YMM2, MemorySize.Float128, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vextractf128V_WX_VY_Ib_2_Data))]
		void Test16_Vextractf128V_WX_VY_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Vextractf128V_WX_VY_Ib_2_Data {
			get {
				yield return new object[] { "C4E37D 19 D3 A5", 6, Code.VEX_Vextractf128_xmmm128_ymm_imm8, Register.XMM3, Register.YMM2, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vextractf128V_WX_VY_Ib_1_Data))]
		void Test32_Vextractf128V_WX_VY_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Vextractf128V_WX_VY_Ib_1_Data {
			get {
				yield return new object[] { "C4E37D 19 10 A5", 6, Code.VEX_Vextractf128_xmmm128_ymm_imm8, Register.YMM2, MemorySize.Float128, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vextractf128V_WX_VY_Ib_2_Data))]
		void Test32_Vextractf128V_WX_VY_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Vextractf128V_WX_VY_Ib_2_Data {
			get {
				yield return new object[] { "C4E37D 19 D3 A5", 6, Code.VEX_Vextractf128_xmmm128_ymm_imm8, Register.XMM3, Register.YMM2, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vextractf128V_WX_VY_Ib_1_Data))]
		void Test64_Vextractf128V_WX_VY_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Vextractf128V_WX_VY_Ib_1_Data {
			get {
				yield return new object[] { "C4E37D 19 10 A5", 6, Code.VEX_Vextractf128_xmmm128_ymm_imm8, Register.YMM2, MemorySize.Float128, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vextractf128V_WX_VY_Ib_2_Data))]
		void Test64_Vextractf128V_WX_VY_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Vextractf128V_WX_VY_Ib_2_Data {
			get {
				yield return new object[] { "C4E37D 19 D3 A5", 6, Code.VEX_Vextractf128_xmmm128_ymm_imm8, Register.XMM3, Register.YMM2, 0xA5 };
				yield return new object[] { "C4637D 19 D3 A5", 6, Code.VEX_Vextractf128_xmmm128_ymm_imm8, Register.XMM3, Register.YMM10, 0xA5 };
				yield return new object[] { "C4C37D 19 D3 A5", 6, Code.VEX_Vextractf128_xmmm128_ymm_imm8, Register.XMM11, Register.YMM2, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vextractf32x4V_WX_k1z_VY_Ib_1_Data))]
		void Test16_Vextractf32x4V_WX_k1z_VY_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vextractf32x4V_WX_k1z_VY_Ib_1_Data {
			get {
				yield return new object[] { "62 F37D2B 19 50 01 A5", 8, Code.EVEX_Vextractf32x4_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F37DAD 19 50 01 A5", 8, Code.EVEX_Vextractf32x4_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K5, MemorySize.Packed128_Float32, 16, true, 0xA5 };
				yield return new object[] { "62 F37D28 19 50 01 A5", 8, Code.EVEX_Vextractf32x4_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F37D4B 19 50 01 A5", 8, Code.EVEX_Vextractf32x4_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F37DCD 19 50 01 A5", 8, Code.EVEX_Vextractf32x4_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed128_Float32, 16, true, 0xA5 };
				yield return new object[] { "62 F37D48 19 50 01 A5", 8, Code.EVEX_Vextractf32x4_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 19 50 01 A5", 8, Code.EVEX_Vextractf64x2_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FDAD 19 50 01 A5", 8, Code.EVEX_Vextractf64x2_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K5, MemorySize.Packed128_Float64, 16, true, 0xA5 };
				yield return new object[] { "62 F3FD28 19 50 01 A5", 8, Code.EVEX_Vextractf64x2_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 19 50 01 A5", 8, Code.EVEX_Vextractf64x2_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FDCD 19 50 01 A5", 8, Code.EVEX_Vextractf64x2_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed128_Float64, 16, true, 0xA5 };
				yield return new object[] { "62 F3FD48 19 50 01 A5", 8, Code.EVEX_Vextractf64x2_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vextractf32x4V_WX_k1z_VY_Ib_2_Data))]
		void Test16_Vextractf32x4V_WX_k1z_VY_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vextractf32x4V_WX_k1z_VY_Ib_2_Data {
			get {
				yield return new object[] { "62 F37D2B 19 D3 A5", 7, Code.EVEX_Vextractf32x4_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DAB 19 D3 A5", 7, Code.EVEX_Vextractf32x4_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D4B 19 D3 A5", 7, Code.EVEX_Vextractf32x4_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DCB 19 D3 A5", 7, Code.EVEX_Vextractf32x4_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 19 D3 A5", 7, Code.EVEX_Vextractf64x2_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FDAB 19 D3 A5", 7, Code.EVEX_Vextractf64x2_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 19 D3 A5", 7, Code.EVEX_Vextractf64x2_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FDCB 19 D3 A5", 7, Code.EVEX_Vextractf64x2_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vextractf32x4V_WX_k1z_VY_Ib_1_Data))]
		void Test32_Vextractf32x4V_WX_k1z_VY_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vextractf32x4V_WX_k1z_VY_Ib_1_Data {
			get {
				yield return new object[] { "62 F37D2B 19 50 01 A5", 8, Code.EVEX_Vextractf32x4_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F37DAD 19 50 01 A5", 8, Code.EVEX_Vextractf32x4_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K5, MemorySize.Packed128_Float32, 16, true, 0xA5 };
				yield return new object[] { "62 F37D28 19 50 01 A5", 8, Code.EVEX_Vextractf32x4_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F37D4B 19 50 01 A5", 8, Code.EVEX_Vextractf32x4_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F37DCD 19 50 01 A5", 8, Code.EVEX_Vextractf32x4_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed128_Float32, 16, true, 0xA5 };
				yield return new object[] { "62 F37D48 19 50 01 A5", 8, Code.EVEX_Vextractf32x4_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 19 50 01 A5", 8, Code.EVEX_Vextractf64x2_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FDAD 19 50 01 A5", 8, Code.EVEX_Vextractf64x2_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K5, MemorySize.Packed128_Float64, 16, true, 0xA5 };
				yield return new object[] { "62 F3FD28 19 50 01 A5", 8, Code.EVEX_Vextractf64x2_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 19 50 01 A5", 8, Code.EVEX_Vextractf64x2_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FDCD 19 50 01 A5", 8, Code.EVEX_Vextractf64x2_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed128_Float64, 16, true, 0xA5 };
				yield return new object[] { "62 F3FD48 19 50 01 A5", 8, Code.EVEX_Vextractf64x2_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vextractf32x4V_WX_k1z_VY_Ib_2_Data))]
		void Test32_Vextractf32x4V_WX_k1z_VY_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vextractf32x4V_WX_k1z_VY_Ib_2_Data {
			get {
				yield return new object[] { "62 F37D2B 19 D3 A5", 7, Code.EVEX_Vextractf32x4_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DAB 19 D3 A5", 7, Code.EVEX_Vextractf32x4_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F37D4B 19 D3 A5", 7, Code.EVEX_Vextractf32x4_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DCB 19 D3 A5", 7, Code.EVEX_Vextractf32x4_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 19 D3 A5", 7, Code.EVEX_Vextractf64x2_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FDAB 19 D3 A5", 7, Code.EVEX_Vextractf64x2_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 19 D3 A5", 7, Code.EVEX_Vextractf64x2_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FDCB 19 D3 A5", 7, Code.EVEX_Vextractf64x2_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vextractf32x4V_WX_k1z_VY_Ib_1_Data))]
		void Test64_Vextractf32x4V_WX_k1z_VY_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vextractf32x4V_WX_k1z_VY_Ib_1_Data {
			get {
				yield return new object[] { "62 F37D2B 19 50 01 A5", 8, Code.EVEX_Vextractf32x4_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F37DAD 19 50 01 A5", 8, Code.EVEX_Vextractf32x4_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K5, MemorySize.Packed128_Float32, 16, true, 0xA5 };
				yield return new object[] { "62 F37D28 19 50 01 A5", 8, Code.EVEX_Vextractf32x4_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F37D4B 19 50 01 A5", 8, Code.EVEX_Vextractf32x4_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed128_Float32, 16, false, 0xA5 };
				yield return new object[] { "62 F37DCD 19 50 01 A5", 8, Code.EVEX_Vextractf32x4_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed128_Float32, 16, true, 0xA5 };
				yield return new object[] { "62 F37D48 19 50 01 A5", 8, Code.EVEX_Vextractf32x4_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed128_Float32, 16, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 19 50 01 A5", 8, Code.EVEX_Vextractf64x2_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FDAD 19 50 01 A5", 8, Code.EVEX_Vextractf64x2_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K5, MemorySize.Packed128_Float64, 16, true, 0xA5 };
				yield return new object[] { "62 F3FD28 19 50 01 A5", 8, Code.EVEX_Vextractf64x2_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 19 50 01 A5", 8, Code.EVEX_Vextractf64x2_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed128_Float64, 16, false, 0xA5 };
				yield return new object[] { "62 F3FDCD 19 50 01 A5", 8, Code.EVEX_Vextractf64x2_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed128_Float64, 16, true, 0xA5 };
				yield return new object[] { "62 F3FD48 19 50 01 A5", 8, Code.EVEX_Vextractf64x2_xmmm128_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed128_Float64, 16, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vextractf32x4V_WX_k1z_VY_Ib_2_Data))]
		void Test64_Vextractf32x4V_WX_k1z_VY_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vextractf32x4V_WX_k1z_VY_Ib_2_Data {
			get {
				yield return new object[] { "62 F37D2B 19 D3 A5", 7, Code.EVEX_Vextractf32x4_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E37DAB 19 D3 A5", 7, Code.EVEX_Vextractf32x4_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 137D2B 19 D3 A5", 7, Code.EVEX_Vextractf32x4_xmmm128_k1z_ymm_imm8, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B37D2B 19 D3 A5", 7, Code.EVEX_Vextractf32x4_xmmm128_k1z_ymm_imm8, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F37D4B 19 D3 A5", 7, Code.EVEX_Vextractf32x4_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E37DCB 19 D3 A5", 7, Code.EVEX_Vextractf32x4_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 137D4B 19 D3 A5", 7, Code.EVEX_Vextractf32x4_xmmm128_k1z_zmm_imm8, Register.XMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B37D4B 19 D3 A5", 7, Code.EVEX_Vextractf32x4_xmmm128_k1z_zmm_imm8, Register.XMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3FD2B 19 D3 A5", 7, Code.EVEX_Vextractf64x2_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E3FDAB 19 D3 A5", 7, Code.EVEX_Vextractf64x2_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 13FD2B 19 D3 A5", 7, Code.EVEX_Vextractf64x2_xmmm128_k1z_ymm_imm8, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3FD2B 19 D3 A5", 7, Code.EVEX_Vextractf64x2_xmmm128_k1z_ymm_imm8, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 19 D3 A5", 7, Code.EVEX_Vextractf64x2_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E3FDCB 19 D3 A5", 7, Code.EVEX_Vextractf64x2_xmmm128_k1z_zmm_imm8, Register.XMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 13FD4B 19 D3 A5", 7, Code.EVEX_Vextractf64x2_xmmm128_k1z_zmm_imm8, Register.XMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3FD4B 19 D3 A5", 7, Code.EVEX_Vextractf64x2_xmmm128_k1z_zmm_imm8, Register.XMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vinsertf32x8V_VZ_k1z_HZ_WY_Ib_1_Data))]
		void Test16_Vinsertf32x8V_VZ_k1z_HZ_WY_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Vinsertf32x8V_VZ_k1z_HZ_WY_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D4B 1A 50 01 A5", 8, Code.EVEX_Vinsertf32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DCD 1A 50 01 A5", 8, Code.EVEX_Vinsertf32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed256_Float32, 32, true, 0xA5 };
				yield return new object[] { "62 F34D48 1A 50 01 A5", 8, Code.EVEX_Vinsertf32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 1A 50 01 A5", 8, Code.EVEX_Vinsertf64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDCD 1A 50 01 A5", 8, Code.EVEX_Vinsertf64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed256_Float64, 32, true, 0xA5 };
				yield return new object[] { "62 F3CD48 1A 50 01 A5", 8, Code.EVEX_Vinsertf64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vinsertf32x8V_VZ_k1z_HZ_WY_Ib_2_Data))]
		void Test16_Vinsertf32x8V_VZ_k1z_HZ_WY_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Vinsertf32x8V_VZ_k1z_HZ_WY_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D4B 1A D3 A5", 7, Code.EVEX_Vinsertf32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DCB 1A D3 A5", 7, Code.EVEX_Vinsertf32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 1A D3 A5", 7, Code.EVEX_Vinsertf64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDCB 1A D3 A5", 7, Code.EVEX_Vinsertf64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vinsertf32x8V_VZ_k1z_HZ_WY_Ib_1_Data))]
		void Test32_Vinsertf32x8V_VZ_k1z_HZ_WY_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Vinsertf32x8V_VZ_k1z_HZ_WY_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D4B 1A 50 01 A5", 8, Code.EVEX_Vinsertf32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DCD 1A 50 01 A5", 8, Code.EVEX_Vinsertf32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed256_Float32, 32, true, 0xA5 };
				yield return new object[] { "62 F34D48 1A 50 01 A5", 8, Code.EVEX_Vinsertf32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 1A 50 01 A5", 8, Code.EVEX_Vinsertf64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDCD 1A 50 01 A5", 8, Code.EVEX_Vinsertf64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed256_Float64, 32, true, 0xA5 };
				yield return new object[] { "62 F3CD48 1A 50 01 A5", 8, Code.EVEX_Vinsertf64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vinsertf32x8V_VZ_k1z_HZ_WY_Ib_2_Data))]
		void Test32_Vinsertf32x8V_VZ_k1z_HZ_WY_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Vinsertf32x8V_VZ_k1z_HZ_WY_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D4B 1A D3 A5", 7, Code.EVEX_Vinsertf32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F34DCB 1A D3 A5", 7, Code.EVEX_Vinsertf32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 1A D3 A5", 7, Code.EVEX_Vinsertf64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDCB 1A D3 A5", 7, Code.EVEX_Vinsertf64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vinsertf32x8V_VZ_k1z_HZ_WY_Ib_1_Data))]
		void Test64_Vinsertf32x8V_VZ_k1z_HZ_WY_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Vinsertf32x8V_VZ_k1z_HZ_WY_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D4B 1A 50 01 A5", 8, Code.EVEX_Vinsertf32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DCD 1A 50 01 A5", 8, Code.EVEX_Vinsertf32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed256_Float32, 32, true, 0xA5 };
				yield return new object[] { "62 F34D48 1A 50 01 A5", 8, Code.EVEX_Vinsertf32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 1A 50 01 A5", 8, Code.EVEX_Vinsertf64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDCD 1A 50 01 A5", 8, Code.EVEX_Vinsertf64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed256_Float64, 32, true, 0xA5 };
				yield return new object[] { "62 F3CD48 1A 50 01 A5", 8, Code.EVEX_Vinsertf64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vinsertf32x8V_VZ_k1z_HZ_WY_Ib_2_Data))]
		void Test64_Vinsertf32x8V_VZ_k1z_HZ_WY_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Vinsertf32x8V_VZ_k1z_HZ_WY_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D4B 1A D3 A5", 7, Code.EVEX_Vinsertf32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E30DCB 1A D3 A5", 7, Code.EVEX_Vinsertf32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM18, Register.ZMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 134D43 1A D3 A5", 7, Code.EVEX_Vinsertf32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM10, Register.ZMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B34D4B 1A D3 A5", 7, Code.EVEX_Vinsertf32x8_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 1A D3 A5", 7, Code.EVEX_Vinsertf64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E38DCB 1A D3 A5", 7, Code.EVEX_Vinsertf64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM18, Register.ZMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 13CD43 1A D3 A5", 7, Code.EVEX_Vinsertf64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM10, Register.ZMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD4B 1A D3 A5", 7, Code.EVEX_Vinsertf64x4_zmm_k1z_zmm_ymmm256_imm8, Register.ZMM2, Register.ZMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vextractf32x8V_WY_k1z_VZ_Ib_1_Data))]
		void Test16_Vextractf32x8V_WY_k1z_VZ_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vextractf32x8V_WY_k1z_VZ_Ib_1_Data {
			get {
				yield return new object[] { "62 F37D4B 1B 50 01 A5", 8, Code.EVEX_Vextractf32x8_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F37DCD 1B 50 01 A5", 8, Code.EVEX_Vextractf32x8_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed256_Float32, 32, true, 0xA5 };
				yield return new object[] { "62 F37D48 1B 50 01 A5", 8, Code.EVEX_Vextractf32x8_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 1B 50 01 A5", 8, Code.EVEX_Vextractf64x4_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3FDCD 1B 50 01 A5", 8, Code.EVEX_Vextractf64x4_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed256_Float64, 32, true, 0xA5 };
				yield return new object[] { "62 F3FD48 1B 50 01 A5", 8, Code.EVEX_Vextractf64x4_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vextractf32x8V_WY_k1z_VZ_Ib_2_Data))]
		void Test16_Vextractf32x8V_WY_k1z_VZ_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_Vextractf32x8V_WY_k1z_VZ_Ib_2_Data {
			get {
				yield return new object[] { "62 F37D4B 1B D3 A5", 7, Code.EVEX_Vextractf32x8_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DCB 1B D3 A5", 7, Code.EVEX_Vextractf32x8_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 1B D3 A5", 7, Code.EVEX_Vextractf64x4_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FDCB 1B D3 A5", 7, Code.EVEX_Vextractf64x4_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vextractf32x8V_WY_k1z_VZ_Ib_1_Data))]
		void Test32_Vextractf32x8V_WY_k1z_VZ_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vextractf32x8V_WY_k1z_VZ_Ib_1_Data {
			get {
				yield return new object[] { "62 F37D4B 1B 50 01 A5", 8, Code.EVEX_Vextractf32x8_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F37DCD 1B 50 01 A5", 8, Code.EVEX_Vextractf32x8_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed256_Float32, 32, true, 0xA5 };
				yield return new object[] { "62 F37D48 1B 50 01 A5", 8, Code.EVEX_Vextractf32x8_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 1B 50 01 A5", 8, Code.EVEX_Vextractf64x4_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3FDCD 1B 50 01 A5", 8, Code.EVEX_Vextractf64x4_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed256_Float64, 32, true, 0xA5 };
				yield return new object[] { "62 F3FD48 1B 50 01 A5", 8, Code.EVEX_Vextractf64x4_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vextractf32x8V_WY_k1z_VZ_Ib_2_Data))]
		void Test32_Vextractf32x8V_WY_k1z_VZ_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_Vextractf32x8V_WY_k1z_VZ_Ib_2_Data {
			get {
				yield return new object[] { "62 F37D4B 1B D3 A5", 7, Code.EVEX_Vextractf32x8_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DCB 1B D3 A5", 7, Code.EVEX_Vextractf32x8_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 1B D3 A5", 7, Code.EVEX_Vextractf64x4_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3FDCB 1B D3 A5", 7, Code.EVEX_Vextractf64x4_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vextractf32x8V_WY_k1z_VZ_Ib_1_Data))]
		void Test64_Vextractf32x8V_WY_k1z_VZ_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vextractf32x8V_WY_k1z_VZ_Ib_1_Data {
			get {
				yield return new object[] { "62 F37D4B 1B 50 01 A5", 8, Code.EVEX_Vextractf32x8_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed256_Float32, 32, false, 0xA5 };
				yield return new object[] { "62 F37DCD 1B 50 01 A5", 8, Code.EVEX_Vextractf32x8_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed256_Float32, 32, true, 0xA5 };
				yield return new object[] { "62 F37D48 1B 50 01 A5", 8, Code.EVEX_Vextractf32x8_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed256_Float32, 32, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 1B 50 01 A5", 8, Code.EVEX_Vextractf64x4_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K3, MemorySize.Packed256_Float64, 32, false, 0xA5 };
				yield return new object[] { "62 F3FDCD 1B 50 01 A5", 8, Code.EVEX_Vextractf64x4_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.K5, MemorySize.Packed256_Float64, 32, true, 0xA5 };
				yield return new object[] { "62 F3FD48 1B 50 01 A5", 8, Code.EVEX_Vextractf64x4_ymmm256_k1z_zmm_imm8, Register.ZMM2, Register.None, MemorySize.Packed256_Float64, 32, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vextractf32x8V_WY_k1z_VZ_Ib_2_Data))]
		void Test64_Vextractf32x8V_WY_k1z_VZ_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_Vextractf32x8V_WY_k1z_VZ_Ib_2_Data {
			get {
				yield return new object[] { "62 F37D4B 1B D3 A5", 7, Code.EVEX_Vextractf32x8_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E37DCB 1B D3 A5", 7, Code.EVEX_Vextractf32x8_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 137D4B 1B D3 A5", 7, Code.EVEX_Vextractf32x8_ymmm256_k1z_zmm_imm8, Register.YMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B37D4B 1B D3 A5", 7, Code.EVEX_Vextractf32x8_ymmm256_k1z_zmm_imm8, Register.YMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3FD4B 1B D3 A5", 7, Code.EVEX_Vextractf64x4_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E3FDCB 1B D3 A5", 7, Code.EVEX_Vextractf64x4_ymmm256_k1z_zmm_imm8, Register.YMM3, Register.ZMM18, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 13FD4B 1B D3 A5", 7, Code.EVEX_Vextractf64x4_ymmm256_k1z_zmm_imm8, Register.YMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3FD4B 1B D3 A5", 7, Code.EVEX_Vextractf64x4_ymmm256_k1z_zmm_imm8, Register.YMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vcvtps2phV_RegMem_Reg_1_Data))]
		void Test16_Vcvtps2phV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test16_Vcvtps2phV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "C4E379 1D 10 A5", 6, Code.VEX_Vcvtps2ph_xmmm64_xmm_imm8, Register.XMM2, MemorySize.Packed64_Float16, 0xA5 };

				yield return new object[] { "C4E37D 1D 10 A5", 6, Code.VEX_Vcvtps2ph_xmmm128_ymm_imm8, Register.YMM2, MemorySize.Packed128_Float16, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_Vcvtps2phV_RegMem_Reg_2_Data))]
		void Test16_Vcvtps2phV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_Vcvtps2phV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "C4E379 1D CD A5", 6, Code.VEX_Vcvtps2ph_xmmm64_xmm_imm8, Register.XMM5, Register.XMM1, 0xA5 };

				yield return new object[] { "C4E37D 1D CD A5", 6, Code.VEX_Vcvtps2ph_xmmm128_ymm_imm8, Register.XMM5, Register.YMM1, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vcvtps2phV_RegMem_Reg_1_Data))]
		void Test32_Vcvtps2phV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test32_Vcvtps2phV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "C4E379 1D 10 A5", 6, Code.VEX_Vcvtps2ph_xmmm64_xmm_imm8, Register.XMM2, MemorySize.Packed64_Float16, 0xA5 };

				yield return new object[] { "C4E37D 1D 10 A5", 6, Code.VEX_Vcvtps2ph_xmmm128_ymm_imm8, Register.YMM2, MemorySize.Packed128_Float16, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_Vcvtps2phV_RegMem_Reg_2_Data))]
		void Test32_Vcvtps2phV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_Vcvtps2phV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "C4E379 1D CD A5", 6, Code.VEX_Vcvtps2ph_xmmm64_xmm_imm8, Register.XMM5, Register.XMM1, 0xA5 };

				yield return new object[] { "C4E37D 1D CD A5", 6, Code.VEX_Vcvtps2ph_xmmm128_ymm_imm8, Register.XMM5, Register.YMM1, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vcvtps2phV_RegMem_Reg_1_Data))]
		void Test64_Vcvtps2phV_RegMem_Reg_1(string hexBytes, int byteLength, Code code, Register reg, MemorySize memSize, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);
		}
		public static IEnumerable<object[]> Test64_Vcvtps2phV_RegMem_Reg_1_Data {
			get {
				yield return new object[] { "C4E379 1D 10 A5", 6, Code.VEX_Vcvtps2ph_xmmm64_xmm_imm8, Register.XMM2, MemorySize.Packed64_Float16, 0xA5 };

				yield return new object[] { "C4E37D 1D 10 A5", 6, Code.VEX_Vcvtps2ph_xmmm128_ymm_imm8, Register.YMM2, MemorySize.Packed128_Float16, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_Vcvtps2phV_RegMem_Reg_2_Data))]
		void Test64_Vcvtps2phV_RegMem_Reg_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_Vcvtps2phV_RegMem_Reg_2_Data {
			get {
				yield return new object[] { "C4E379 1D CD A5", 6, Code.VEX_Vcvtps2ph_xmmm64_xmm_imm8, Register.XMM5, Register.XMM1, 0xA5 };
				yield return new object[] { "C46379 1D CD A5", 6, Code.VEX_Vcvtps2ph_xmmm64_xmm_imm8, Register.XMM5, Register.XMM9, 0xA5 };
				yield return new object[] { "C4C379 1D CD A5", 6, Code.VEX_Vcvtps2ph_xmmm64_xmm_imm8, Register.XMM13, Register.XMM1, 0xA5 };
				yield return new object[] { "C44379 1D CD A5", 6, Code.VEX_Vcvtps2ph_xmmm64_xmm_imm8, Register.XMM13, Register.XMM9, 0xA5 };

				yield return new object[] { "C4E37D 1D CD A5", 6, Code.VEX_Vcvtps2ph_xmmm128_ymm_imm8, Register.XMM5, Register.YMM1, 0xA5 };
				yield return new object[] { "C4637D 1D CD A5", 6, Code.VEX_Vcvtps2ph_xmmm128_ymm_imm8, Register.XMM5, Register.YMM9, 0xA5 };
				yield return new object[] { "C4C37D 1D CD A5", 6, Code.VEX_Vcvtps2ph_xmmm128_ymm_imm8, Register.XMM13, Register.YMM1, 0xA5 };
				yield return new object[] { "C4437D 1D CD A5", 6, Code.VEX_Vcvtps2ph_xmmm128_ymm_imm8, Register.XMM13, Register.YMM9, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VrmeoveV_VX_k1_HX_WX_1_Data))]
		void Test16_VrmeoveV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder16(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VrmeoveV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F37D0B 1D 50 01 A5", 8, Code.EVEX_Vcvtps2ph_xmmm64_k1z_xmm_imm8, Register.XMM2, Register.K3, MemorySize.Packed64_Float16, 8, false, 0xA5 };
				yield return new object[] { "62 F37D08 1D 50 01 A5", 8, Code.EVEX_Vcvtps2ph_xmmm64_k1z_xmm_imm8, Register.XMM2, Register.None, MemorySize.Packed64_Float16, 8, false, 0xA5 };

				yield return new object[] { "62 F37D2B 1D 50 01 A5", 8, Code.EVEX_Vcvtps2ph_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K3, MemorySize.Packed128_Float16, 16, false, 0xA5 };
				yield return new object[] { "62 F37D28 1D 50 01 A5", 8, Code.EVEX_Vcvtps2ph_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.None, MemorySize.Packed128_Float16, 16, false, 0xA5 };

				yield return new object[] { "62 F37D4B 1D 50 01 A5", 8, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.ZMM2, Register.K3, MemorySize.Packed256_Float16, 32, false, 0xA5 };
				yield return new object[] { "62 F37D48 1D 50 01 A5", 8, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.ZMM2, Register.None, MemorySize.Packed256_Float16, 32, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VrmeoveV_VX_k1_HX_WX_2_Data))]
		void Test16_VrmeoveV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test16_VrmeoveV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F37D8B 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_xmmm64_k1z_xmm_imm8, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F37D08 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_xmmm64_k1z_xmm_imm8, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D9B 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F37DAB 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F37D28 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DBB 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F37DCB 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F37D48 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.YMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DDB 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VrmeoveV_VX_k1_HX_WX_1_Data))]
		void Test32_VrmeoveV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder32(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VrmeoveV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F37D0B 1D 50 01 A5", 8, Code.EVEX_Vcvtps2ph_xmmm64_k1z_xmm_imm8, Register.XMM2, Register.K3, MemorySize.Packed64_Float16, 8, false, 0xA5 };
				yield return new object[] { "62 F37D08 1D 50 01 A5", 8, Code.EVEX_Vcvtps2ph_xmmm64_k1z_xmm_imm8, Register.XMM2, Register.None, MemorySize.Packed64_Float16, 8, false, 0xA5 };

				yield return new object[] { "62 F37D2B 1D 50 01 A5", 8, Code.EVEX_Vcvtps2ph_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K3, MemorySize.Packed128_Float16, 16, false, 0xA5 };
				yield return new object[] { "62 F37D28 1D 50 01 A5", 8, Code.EVEX_Vcvtps2ph_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.None, MemorySize.Packed128_Float16, 16, false, 0xA5 };

				yield return new object[] { "62 F37D4B 1D 50 01 A5", 8, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.ZMM2, Register.K3, MemorySize.Packed256_Float16, 32, false, 0xA5 };
				yield return new object[] { "62 F37D48 1D 50 01 A5", 8, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.ZMM2, Register.None, MemorySize.Packed256_Float16, 32, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VrmeoveV_VX_k1_HX_WX_2_Data))]
		void Test32_VrmeoveV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test32_VrmeoveV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F37D8B 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_xmmm64_k1z_xmm_imm8, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F37D08 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_xmmm64_k1z_xmm_imm8, Register.XMM3, Register.XMM2, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D9B 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F37DAB 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F37D28 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DBB 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F37DCB 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 F37D48 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.YMM3, Register.ZMM2, Register.None, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DDB 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VrmeoveV_VX_k1_HX_WX_1_Data))]
		void Test64_VrmeoveV_VX_k1_HX_WX_1(string hexBytes, int byteLength, Code code, Register reg1, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
			var decoder = CreateDecoder64(hexBytes);
			var instr = decoder.Decode();

			Assert.Equal(code, instr.Code);
			Assert.Equal(3, instr.OpCount);
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
			Assert.Equal(reg1, instr.Op1Register);

			Assert.Equal(OpKind.Immediate8, instr.Op2Kind);
			Assert.Equal(immediate8, instr.Immediate8);

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(RoundingControl.None, instr.RoundingControl);
			Assert.False(instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VrmeoveV_VX_k1_HX_WX_1_Data {
			get {
				yield return new object[] { "62 F37D0B 1D 50 01 A5", 8, Code.EVEX_Vcvtps2ph_xmmm64_k1z_xmm_imm8, Register.XMM2, Register.K3, MemorySize.Packed64_Float16, 8, false, 0xA5 };
				yield return new object[] { "62 F37D08 1D 50 01 A5", 8, Code.EVEX_Vcvtps2ph_xmmm64_k1z_xmm_imm8, Register.XMM2, Register.None, MemorySize.Packed64_Float16, 8, false, 0xA5 };

				yield return new object[] { "62 F37D2B 1D 50 01 A5", 8, Code.EVEX_Vcvtps2ph_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.K3, MemorySize.Packed128_Float16, 16, false, 0xA5 };
				yield return new object[] { "62 F37D28 1D 50 01 A5", 8, Code.EVEX_Vcvtps2ph_xmmm128_k1z_ymm_imm8, Register.YMM2, Register.None, MemorySize.Packed128_Float16, 16, false, 0xA5 };

				yield return new object[] { "62 F37D4B 1D 50 01 A5", 8, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.ZMM2, Register.K3, MemorySize.Packed256_Float16, 32, false, 0xA5 };
				yield return new object[] { "62 F37D48 1D 50 01 A5", 8, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.ZMM2, Register.None, MemorySize.Packed256_Float16, 32, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VrmeoveV_VX_k1_HX_WX_2_Data))]
		void Test64_VrmeoveV_VX_k1_HX_WX_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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

			Assert.Equal(kreg, instr.OpMask);
			Assert.Equal(z, instr.ZeroingMasking);
			Assert.Equal(rc, instr.RoundingControl);
			Assert.Equal(sae, instr.SuppressAllExceptions);
		}
		public static IEnumerable<object[]> Test64_VrmeoveV_VX_k1_HX_WX_2_Data {
			get {
				yield return new object[] { "62 F37D8B 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_xmmm64_k1z_xmm_imm8, Register.XMM3, Register.XMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 E37D0B 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_xmmm64_k1z_xmm_imm8, Register.XMM3, Register.XMM18, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 137D0B 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_xmmm64_k1z_xmm_imm8, Register.XMM27, Register.XMM10, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B37D0B 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_xmmm64_k1z_xmm_imm8, Register.XMM19, Register.XMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37DDB 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, true, 0xA5 };

				yield return new object[] { "62 F37DAB 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 E37D2B 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_xmmm128_k1z_ymm_imm8, Register.XMM3, Register.YMM18, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 137D2B 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_xmmm128_k1z_ymm_imm8, Register.XMM27, Register.YMM10, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B37D2B 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_xmmm128_k1z_ymm_imm8, Register.XMM19, Register.YMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D1B 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, true, 0xA5 };

				yield return new object[] { "62 F37DCB 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 E37D4B 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.YMM3, Register.ZMM18, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 137D4B 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.YMM27, Register.ZMM10, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B37D4B 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.YMM19, Register.ZMM2, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F37D7B 1D D3 A5", 7, Code.EVEX_Vcvtps2ph_ymmm256_k1z_zmm_imm8_sae, Register.YMM3, Register.ZMM2, Register.K3, RoundingControl.None, false, true, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpudV_K_k1_HX_WX_Ib_1_Data))]
		void Test16_VpcmpudV_K_k1_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpcmpudV_K_k1_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D1D 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D08 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false, 0xA5 };
				yield return new object[] { "62 F34D3D 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D28 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
				yield return new object[] { "62 F34D5D 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D48 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD1D 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD08 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CD3D 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD28 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CD5D 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD48 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpudV_K_k1_HX_WX_Ib_2_Data))]
		void Test16_VpcmpudV_K_k1_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpcmpudV_K_k1_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 1E D3 A5", 7, Code.EVEX_Vpcmpud_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F34D2B 1E D3 A5", 7, Code.EVEX_Vpcmpud_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F34D4B 1E D3 A5", 7, Code.EVEX_Vpcmpud_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 1E D3 A5", 7, Code.EVEX_Vpcmpuq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 1E D3 A5", 7, Code.EVEX_Vpcmpuq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 1E D3 A5", 7, Code.EVEX_Vpcmpuq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpudV_K_k1_HX_WX_Ib_1_Data))]
		void Test32_VpcmpudV_K_k1_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpcmpudV_K_k1_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D1D 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D08 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false, 0xA5 };
				yield return new object[] { "62 F34D3D 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D28 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
				yield return new object[] { "62 F34D5D 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D48 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD1D 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD08 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CD3D 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD28 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CD5D 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD48 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpudV_K_k1_HX_WX_Ib_2_Data))]
		void Test32_VpcmpudV_K_k1_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpcmpudV_K_k1_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 1E D3 A5", 7, Code.EVEX_Vpcmpud_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F34D2B 1E D3 A5", 7, Code.EVEX_Vpcmpud_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F34D4B 1E D3 A5", 7, Code.EVEX_Vpcmpud_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 1E D3 A5", 7, Code.EVEX_Vpcmpuq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 1E D3 A5", 7, Code.EVEX_Vpcmpuq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 1E D3 A5", 7, Code.EVEX_Vpcmpuq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpudV_K_k1_HX_WX_Ib_1_Data))]
		void Test64_VpcmpudV_K_k1_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpcmpudV_K_k1_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D1D 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D08 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false, 0xA5 };
				yield return new object[] { "62 F34D3D 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D28 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
				yield return new object[] { "62 F34D5D 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D48 1E 50 01 A5", 8, Code.EVEX_Vpcmpud_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD1D 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD08 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CD3D 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD28 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CD5D 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD48 1E 50 01 A5", 8, Code.EVEX_Vpcmpuq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpudV_K_k1_HX_WX_Ib_2_Data))]
		void Test64_VpcmpudV_K_k1_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpcmpudV_K_k1_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 1E D3 A5", 7, Code.EVEX_Vpcmpud_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F30D0B 1E D3 A5", 7, Code.EVEX_Vpcmpud_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM14, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 934D03 1E D3 A5", 7, Code.EVEX_Vpcmpud_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM22, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D0B 1E D3 A5", 7, Code.EVEX_Vpcmpud_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.XMM19, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F34D2B 1E D3 A5", 7, Code.EVEX_Vpcmpud_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F30D2B 1E D3 A5", 7, Code.EVEX_Vpcmpud_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 934D23 1E D3 A5", 7, Code.EVEX_Vpcmpud_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D2B 1E D3 A5", 7, Code.EVEX_Vpcmpud_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F34D4B 1E D3 A5", 7, Code.EVEX_Vpcmpud_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F30D4B 1E D3 A5", 7, Code.EVEX_Vpcmpud_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 934D43 1E D3 A5", 7, Code.EVEX_Vpcmpud_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D4B 1E D3 A5", 7, Code.EVEX_Vpcmpud_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 1E D3 A5", 7, Code.EVEX_Vpcmpuq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F38D0B 1E D3 A5", 7, Code.EVEX_Vpcmpuq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM14, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 93CD03 1E D3 A5", 7, Code.EVEX_Vpcmpuq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM22, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD0B 1E D3 A5", 7, Code.EVEX_Vpcmpuq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.XMM19, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 1E D3 A5", 7, Code.EVEX_Vpcmpuq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F38D2B 1E D3 A5", 7, Code.EVEX_Vpcmpuq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 93CD23 1E D3 A5", 7, Code.EVEX_Vpcmpuq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD2B 1E D3 A5", 7, Code.EVEX_Vpcmpuq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 1E D3 A5", 7, Code.EVEX_Vpcmpuq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F38D4B 1E D3 A5", 7, Code.EVEX_Vpcmpuq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 93CD43 1E D3 A5", 7, Code.EVEX_Vpcmpuq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD4B 1E D3 A5", 7, Code.EVEX_Vpcmpuq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpdV_K_k1_HX_WX_Ib_1_Data))]
		void Test16_VpcmpdV_K_k1_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpcmpdV_K_k1_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D1D 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D08 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false, 0xA5 };
				yield return new object[] { "62 F34D3D 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D28 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false, 0xA5 };
				yield return new object[] { "62 F34D5D 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D48 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD1D 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD08 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CD3D 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD28 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CD5D 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD48 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpcmpdV_K_k1_HX_WX_Ib_2_Data))]
		void Test16_VpcmpdV_K_k1_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpcmpdV_K_k1_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 1F D3 A5", 7, Code.EVEX_Vpcmpd_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F34D2B 1F D3 A5", 7, Code.EVEX_Vpcmpd_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F34D4B 1F D3 A5", 7, Code.EVEX_Vpcmpd_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 1F D3 A5", 7, Code.EVEX_Vpcmpq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 1F D3 A5", 7, Code.EVEX_Vpcmpq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 1F D3 A5", 7, Code.EVEX_Vpcmpq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpdV_K_k1_HX_WX_Ib_1_Data))]
		void Test32_VpcmpdV_K_k1_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpcmpdV_K_k1_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D1D 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D08 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false, 0xA5 };
				yield return new object[] { "62 F34D3D 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D28 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false, 0xA5 };
				yield return new object[] { "62 F34D5D 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D48 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD1D 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD08 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CD3D 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD28 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CD5D 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD48 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpcmpdV_K_k1_HX_WX_Ib_2_Data))]
		void Test32_VpcmpdV_K_k1_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpcmpdV_K_k1_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 1F D3 A5", 7, Code.EVEX_Vpcmpd_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F34D2B 1F D3 A5", 7, Code.EVEX_Vpcmpd_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F34D4B 1F D3 A5", 7, Code.EVEX_Vpcmpd_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 1F D3 A5", 7, Code.EVEX_Vpcmpq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 1F D3 A5", 7, Code.EVEX_Vpcmpq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 1F D3 A5", 7, Code.EVEX_Vpcmpq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpdV_K_k1_HX_WX_Ib_1_Data))]
		void Test64_VpcmpdV_K_k1_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpcmpdV_K_k1_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D1D 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D08 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int32, 32, false, 0xA5 };
				yield return new object[] { "62 F34D3D 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D28 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int32, 64, false, 0xA5 };
				yield return new object[] { "62 F34D5D 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int32, 4, false, 0xA5 };
				yield return new object[] { "62 F34D48 1F 50 01 A5", 8, Code.EVEX_Vpcmpd_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.K3, MemorySize.Packed128_Int64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD1D 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.K5, MemorySize.Broadcast128_Int64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD08 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.None, MemorySize.Packed128_Int64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.K3, MemorySize.Packed256_Int64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CD3D 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.K5, MemorySize.Broadcast256_Int64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD28 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.None, MemorySize.Packed256_Int64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.K3, MemorySize.Packed512_Int64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CD5D 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_Int64, 8, false, 0xA5 };
				yield return new object[] { "62 F3CD48 1F 50 01 A5", 8, Code.EVEX_Vpcmpq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.None, MemorySize.Packed512_Int64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpcmpdV_K_k1_HX_WX_Ib_2_Data))]
		void Test64_VpcmpdV_K_k1_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpcmpdV_K_k1_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 1F D3 A5", 7, Code.EVEX_Vpcmpd_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F30D0B 1F D3 A5", 7, Code.EVEX_Vpcmpd_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM14, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 934D03 1F D3 A5", 7, Code.EVEX_Vpcmpd_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM22, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D0B 1F D3 A5", 7, Code.EVEX_Vpcmpd_k_k1_xmm_xmmm128b32_imm8, Register.K2, Register.XMM6, Register.XMM19, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F34D2B 1F D3 A5", 7, Code.EVEX_Vpcmpd_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F30D2B 1F D3 A5", 7, Code.EVEX_Vpcmpd_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 934D23 1F D3 A5", 7, Code.EVEX_Vpcmpd_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D2B 1F D3 A5", 7, Code.EVEX_Vpcmpd_k_k1_ymm_ymmm256b32_imm8, Register.K2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F34D4B 1F D3 A5", 7, Code.EVEX_Vpcmpd_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F30D4B 1F D3 A5", 7, Code.EVEX_Vpcmpd_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 934D43 1F D3 A5", 7, Code.EVEX_Vpcmpd_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D4B 1F D3 A5", 7, Code.EVEX_Vpcmpd_k_k1_zmm_zmmm512b32_imm8, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 1F D3 A5", 7, Code.EVEX_Vpcmpq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F38D0B 1F D3 A5", 7, Code.EVEX_Vpcmpq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM14, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 93CD03 1F D3 A5", 7, Code.EVEX_Vpcmpq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM22, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD0B 1F D3 A5", 7, Code.EVEX_Vpcmpq_k_k1_xmm_xmmm128b64_imm8, Register.K2, Register.XMM6, Register.XMM19, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 1F D3 A5", 7, Code.EVEX_Vpcmpq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F38D2B 1F D3 A5", 7, Code.EVEX_Vpcmpq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 93CD23 1F D3 A5", 7, Code.EVEX_Vpcmpq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD2B 1F D3 A5", 7, Code.EVEX_Vpcmpq_k_k1_ymm_ymmm256b64_imm8, Register.K2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 1F D3 A5", 7, Code.EVEX_Vpcmpq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F38D4B 1F D3 A5", 7, Code.EVEX_Vpcmpq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 93CD43 1F D3 A5", 7, Code.EVEX_Vpcmpq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD4B 1F D3 A5", 7, Code.EVEX_Vpcmpq_k_k1_zmm_zmmm512b64_imm8, Register.K2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };
			}
		}
	}
}
