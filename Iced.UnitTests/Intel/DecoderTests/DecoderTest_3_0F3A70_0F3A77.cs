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
	public sealed class DecoderTest_3_0F3A70_0F3A77 : DecoderTest {
		[Theory]
		[MemberData(nameof(Test16_VpshldwV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test16_VpshldwV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpshldwV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3CD0B 70 50 01 A5", 8, Code.EVEX_Vpshldw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD8D 70 50 01 A5", 8, Code.EVEX_Vpshldw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true, 0xA5 };
				yield return new object[] { "62 F3CD08 70 50 01 A5", 8, Code.EVEX_Vpshldw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 70 50 01 A5", 8, Code.EVEX_Vpshldw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDAD 70 50 01 A5", 8, Code.EVEX_Vpshldw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true, 0xA5 };
				yield return new object[] { "62 F3CD28 70 50 01 A5", 8, Code.EVEX_Vpshldw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 70 50 01 A5", 8, Code.EVEX_Vpshldw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDCD 70 50 01 A5", 8, Code.EVEX_Vpshldw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true, 0xA5 };
				yield return new object[] { "62 F3CD48 70 50 01 A5", 8, Code.EVEX_Vpshldw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpshldwV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test16_VpshldwV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpshldwV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3CD0B 70 D3 A5", 7, Code.EVEX_Vpshldw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD8B 70 D3 A5", 7, Code.EVEX_Vpshldw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 70 D3 A5", 7, Code.EVEX_Vpshldw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDAB 70 D3 A5", 7, Code.EVEX_Vpshldw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 70 D3 A5", 7, Code.EVEX_Vpshldw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDCB 70 D3 A5", 7, Code.EVEX_Vpshldw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpshldwV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test32_VpshldwV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpshldwV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3CD0B 70 50 01 A5", 8, Code.EVEX_Vpshldw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD8D 70 50 01 A5", 8, Code.EVEX_Vpshldw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true, 0xA5 };
				yield return new object[] { "62 F3CD08 70 50 01 A5", 8, Code.EVEX_Vpshldw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 70 50 01 A5", 8, Code.EVEX_Vpshldw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDAD 70 50 01 A5", 8, Code.EVEX_Vpshldw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true, 0xA5 };
				yield return new object[] { "62 F3CD28 70 50 01 A5", 8, Code.EVEX_Vpshldw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 70 50 01 A5", 8, Code.EVEX_Vpshldw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDCD 70 50 01 A5", 8, Code.EVEX_Vpshldw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true, 0xA5 };
				yield return new object[] { "62 F3CD48 70 50 01 A5", 8, Code.EVEX_Vpshldw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpshldwV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test32_VpshldwV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpshldwV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3CD0B 70 D3 A5", 7, Code.EVEX_Vpshldw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD8B 70 D3 A5", 7, Code.EVEX_Vpshldw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 70 D3 A5", 7, Code.EVEX_Vpshldw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDAB 70 D3 A5", 7, Code.EVEX_Vpshldw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 70 D3 A5", 7, Code.EVEX_Vpshldw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDCB 70 D3 A5", 7, Code.EVEX_Vpshldw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpshldwV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test64_VpshldwV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpshldwV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3CD0B 70 50 01 A5", 8, Code.EVEX_Vpshldw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD8D 70 50 01 A5", 8, Code.EVEX_Vpshldw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true, 0xA5 };
				yield return new object[] { "62 F3CD08 70 50 01 A5", 8, Code.EVEX_Vpshldw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 70 50 01 A5", 8, Code.EVEX_Vpshldw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDAD 70 50 01 A5", 8, Code.EVEX_Vpshldw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true, 0xA5 };
				yield return new object[] { "62 F3CD28 70 50 01 A5", 8, Code.EVEX_Vpshldw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 70 50 01 A5", 8, Code.EVEX_Vpshldw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDCD 70 50 01 A5", 8, Code.EVEX_Vpshldw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true, 0xA5 };
				yield return new object[] { "62 F3CD48 70 50 01 A5", 8, Code.EVEX_Vpshldw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpshldwV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test64_VpshldwV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpshldwV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3CD0B 70 D3 A5", 7, Code.EVEX_Vpshldw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E38D8B 70 D3 A5", 7, Code.EVEX_Vpshldw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 13CD03 70 D3 A5", 7, Code.EVEX_Vpshldw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD0B 70 D3 A5", 7, Code.EVEX_Vpshldw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 70 D3 A5", 7, Code.EVEX_Vpshldw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E38DAB 70 D3 A5", 7, Code.EVEX_Vpshldw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 13CD23 70 D3 A5", 7, Code.EVEX_Vpshldw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD2B 70 D3 A5", 7, Code.EVEX_Vpshldw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 70 D3 A5", 7, Code.EVEX_Vpshldw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E38DCB 70 D3 A5", 7, Code.EVEX_Vpshldw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 13CD43 70 D3 A5", 7, Code.EVEX_Vpshldw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD4B 70 D3 A5", 7, Code.EVEX_Vpshldw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpshlddV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test16_VpshlddV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpshlddV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 71 50 01 A5", 8, Code.EVEX_Vpshldd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D9D 71 50 01 A5", 8, Code.EVEX_Vpshldd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 71 50 01 A5", 8, Code.EVEX_Vpshldd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 71 50 01 A5", 8, Code.EVEX_Vpshldd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 71 50 01 A5", 8, Code.EVEX_Vpshldd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 71 50 01 A5", 8, Code.EVEX_Vpshldd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 71 50 01 A5", 8, Code.EVEX_Vpshldd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 71 50 01 A5", 8, Code.EVEX_Vpshldd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 71 50 01 A5", 8, Code.EVEX_Vpshldd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 71 50 01 A5", 8, Code.EVEX_Vpshldq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D 71 50 01 A5", 8, Code.EVEX_Vpshldq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 71 50 01 A5", 8, Code.EVEX_Vpshldq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 71 50 01 A5", 8, Code.EVEX_Vpshldq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 71 50 01 A5", 8, Code.EVEX_Vpshldq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 71 50 01 A5", 8, Code.EVEX_Vpshldq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 71 50 01 A5", 8, Code.EVEX_Vpshldq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 71 50 01 A5", 8, Code.EVEX_Vpshldq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 71 50 01 A5", 8, Code.EVEX_Vpshldq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpshlddV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test16_VpshlddV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpshlddV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 71 D3 A5", 7, Code.EVEX_Vpshldd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34D8B 71 D3 A5", 7, Code.EVEX_Vpshldd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D2B 71 D3 A5", 7, Code.EVEX_Vpshldd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DAB 71 D3 A5", 7, Code.EVEX_Vpshldd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D4B 71 D3 A5", 7, Code.EVEX_Vpshldd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DCB 71 D3 A5", 7, Code.EVEX_Vpshldd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 71 D3 A5", 7, Code.EVEX_Vpshldq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD0B 71 D3 A5", 7, Code.EVEX_Vpshldq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDAB 71 D3 A5", 7, Code.EVEX_Vpshldq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD2B 71 D3 A5", 7, Code.EVEX_Vpshldq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB 71 D3 A5", 7, Code.EVEX_Vpshldq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD4B 71 D3 A5", 7, Code.EVEX_Vpshldq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpshlddV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test32_VpshlddV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpshlddV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 71 50 01 A5", 8, Code.EVEX_Vpshldd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D9D 71 50 01 A5", 8, Code.EVEX_Vpshldd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 71 50 01 A5", 8, Code.EVEX_Vpshldd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 71 50 01 A5", 8, Code.EVEX_Vpshldd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 71 50 01 A5", 8, Code.EVEX_Vpshldd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 71 50 01 A5", 8, Code.EVEX_Vpshldd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 71 50 01 A5", 8, Code.EVEX_Vpshldd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 71 50 01 A5", 8, Code.EVEX_Vpshldd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 71 50 01 A5", 8, Code.EVEX_Vpshldd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 71 50 01 A5", 8, Code.EVEX_Vpshldq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D 71 50 01 A5", 8, Code.EVEX_Vpshldq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 71 50 01 A5", 8, Code.EVEX_Vpshldq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 71 50 01 A5", 8, Code.EVEX_Vpshldq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 71 50 01 A5", 8, Code.EVEX_Vpshldq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 71 50 01 A5", 8, Code.EVEX_Vpshldq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 71 50 01 A5", 8, Code.EVEX_Vpshldq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 71 50 01 A5", 8, Code.EVEX_Vpshldq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 71 50 01 A5", 8, Code.EVEX_Vpshldq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpshlddV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test32_VpshlddV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpshlddV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 71 D3 A5", 7, Code.EVEX_Vpshldd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34D8B 71 D3 A5", 7, Code.EVEX_Vpshldd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D2B 71 D3 A5", 7, Code.EVEX_Vpshldd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DAB 71 D3 A5", 7, Code.EVEX_Vpshldd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D4B 71 D3 A5", 7, Code.EVEX_Vpshldd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DCB 71 D3 A5", 7, Code.EVEX_Vpshldd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 71 D3 A5", 7, Code.EVEX_Vpshldq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD0B 71 D3 A5", 7, Code.EVEX_Vpshldq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDAB 71 D3 A5", 7, Code.EVEX_Vpshldq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD2B 71 D3 A5", 7, Code.EVEX_Vpshldq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB 71 D3 A5", 7, Code.EVEX_Vpshldq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD4B 71 D3 A5", 7, Code.EVEX_Vpshldq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpshlddV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test64_VpshlddV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpshlddV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 71 50 01 A5", 8, Code.EVEX_Vpshldd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D9D 71 50 01 A5", 8, Code.EVEX_Vpshldd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 71 50 01 A5", 8, Code.EVEX_Vpshldd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 71 50 01 A5", 8, Code.EVEX_Vpshldd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 71 50 01 A5", 8, Code.EVEX_Vpshldd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 71 50 01 A5", 8, Code.EVEX_Vpshldd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 71 50 01 A5", 8, Code.EVEX_Vpshldd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 71 50 01 A5", 8, Code.EVEX_Vpshldd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 71 50 01 A5", 8, Code.EVEX_Vpshldd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 71 50 01 A5", 8, Code.EVEX_Vpshldq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D 71 50 01 A5", 8, Code.EVEX_Vpshldq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 71 50 01 A5", 8, Code.EVEX_Vpshldq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 71 50 01 A5", 8, Code.EVEX_Vpshldq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 71 50 01 A5", 8, Code.EVEX_Vpshldq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 71 50 01 A5", 8, Code.EVEX_Vpshldq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 71 50 01 A5", 8, Code.EVEX_Vpshldq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 71 50 01 A5", 8, Code.EVEX_Vpshldq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 71 50 01 A5", 8, Code.EVEX_Vpshldq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpshlddV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test64_VpshlddV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpshlddV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 71 D3 A5", 7, Code.EVEX_Vpshldd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 E30D0B 71 D3 A5", 7, Code.EVEX_Vpshldd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 134D03 71 D3 A5", 7, Code.EVEX_Vpshldd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D0B 71 D3 A5", 7, Code.EVEX_Vpshldd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34D8B 71 D3 A5", 7, Code.EVEX_Vpshldd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D2B 71 D3 A5", 7, Code.EVEX_Vpshldd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 E30D2B 71 D3 A5", 7, Code.EVEX_Vpshldd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 134D23 71 D3 A5", 7, Code.EVEX_Vpshldd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D2B 71 D3 A5", 7, Code.EVEX_Vpshldd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DAB 71 D3 A5", 7, Code.EVEX_Vpshldd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D4B 71 D3 A5", 7, Code.EVEX_Vpshldd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 E30D4B 71 D3 A5", 7, Code.EVEX_Vpshldd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 134D43 71 D3 A5", 7, Code.EVEX_Vpshldd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D4B 71 D3 A5", 7, Code.EVEX_Vpshldd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DCB 71 D3 A5", 7, Code.EVEX_Vpshldd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 71 D3 A5", 7, Code.EVEX_Vpshldq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D0B 71 D3 A5", 7, Code.EVEX_Vpshldq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD03 71 D3 A5", 7, Code.EVEX_Vpshldq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD0B 71 D3 A5", 7, Code.EVEX_Vpshldq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD0B 71 D3 A5", 7, Code.EVEX_Vpshldq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDAB 71 D3 A5", 7, Code.EVEX_Vpshldq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D2B 71 D3 A5", 7, Code.EVEX_Vpshldq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD23 71 D3 A5", 7, Code.EVEX_Vpshldq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD2B 71 D3 A5", 7, Code.EVEX_Vpshldq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD2B 71 D3 A5", 7, Code.EVEX_Vpshldq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB 71 D3 A5", 7, Code.EVEX_Vpshldq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D4B 71 D3 A5", 7, Code.EVEX_Vpshldq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD43 71 D3 A5", 7, Code.EVEX_Vpshldq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD4B 71 D3 A5", 7, Code.EVEX_Vpshldq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD4B 71 D3 A5", 7, Code.EVEX_Vpshldq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpshrdwV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test16_VpshrdwV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpshrdwV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3CD0B 72 50 01 A5", 8, Code.EVEX_Vpshrdw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD8D 72 50 01 A5", 8, Code.EVEX_Vpshrdw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true, 0xA5 };
				yield return new object[] { "62 F3CD08 72 50 01 A5", 8, Code.EVEX_Vpshrdw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 72 50 01 A5", 8, Code.EVEX_Vpshrdw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDAD 72 50 01 A5", 8, Code.EVEX_Vpshrdw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true, 0xA5 };
				yield return new object[] { "62 F3CD28 72 50 01 A5", 8, Code.EVEX_Vpshrdw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 72 50 01 A5", 8, Code.EVEX_Vpshrdw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDCD 72 50 01 A5", 8, Code.EVEX_Vpshrdw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true, 0xA5 };
				yield return new object[] { "62 F3CD48 72 50 01 A5", 8, Code.EVEX_Vpshrdw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpshrdwV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test16_VpshrdwV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpshrdwV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3CD0B 72 D3 A5", 7, Code.EVEX_Vpshrdw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD8B 72 D3 A5", 7, Code.EVEX_Vpshrdw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 72 D3 A5", 7, Code.EVEX_Vpshrdw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDAB 72 D3 A5", 7, Code.EVEX_Vpshrdw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 72 D3 A5", 7, Code.EVEX_Vpshrdw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDCB 72 D3 A5", 7, Code.EVEX_Vpshrdw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpshrdwV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test32_VpshrdwV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpshrdwV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3CD0B 72 50 01 A5", 8, Code.EVEX_Vpshrdw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD8D 72 50 01 A5", 8, Code.EVEX_Vpshrdw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true, 0xA5 };
				yield return new object[] { "62 F3CD08 72 50 01 A5", 8, Code.EVEX_Vpshrdw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 72 50 01 A5", 8, Code.EVEX_Vpshrdw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDAD 72 50 01 A5", 8, Code.EVEX_Vpshrdw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true, 0xA5 };
				yield return new object[] { "62 F3CD28 72 50 01 A5", 8, Code.EVEX_Vpshrdw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 72 50 01 A5", 8, Code.EVEX_Vpshrdw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDCD 72 50 01 A5", 8, Code.EVEX_Vpshrdw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true, 0xA5 };
				yield return new object[] { "62 F3CD48 72 50 01 A5", 8, Code.EVEX_Vpshrdw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpshrdwV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test32_VpshrdwV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpshrdwV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3CD0B 72 D3 A5", 7, Code.EVEX_Vpshrdw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CD8B 72 D3 A5", 7, Code.EVEX_Vpshrdw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 72 D3 A5", 7, Code.EVEX_Vpshrdw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDAB 72 D3 A5", 7, Code.EVEX_Vpshrdw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 72 D3 A5", 7, Code.EVEX_Vpshrdw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 F3CDCB 72 D3 A5", 7, Code.EVEX_Vpshrdw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpshrdwV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test64_VpshrdwV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpshrdwV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F3CD0B 72 50 01 A5", 8, Code.EVEX_Vpshrdw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt16, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD8D 72 50 01 A5", 8, Code.EVEX_Vpshrdw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Packed128_UInt16, 16, true, 0xA5 };
				yield return new object[] { "62 F3CD08 72 50 01 A5", 8, Code.EVEX_Vpshrdw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt16, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 72 50 01 A5", 8, Code.EVEX_Vpshrdw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt16, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDAD 72 50 01 A5", 8, Code.EVEX_Vpshrdw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Packed256_UInt16, 32, true, 0xA5 };
				yield return new object[] { "62 F3CD28 72 50 01 A5", 8, Code.EVEX_Vpshrdw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt16, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 72 50 01 A5", 8, Code.EVEX_Vpshrdw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDCD 72 50 01 A5", 8, Code.EVEX_Vpshrdw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Packed512_UInt16, 64, true, 0xA5 };
				yield return new object[] { "62 F3CD48 72 50 01 A5", 8, Code.EVEX_Vpshrdw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt16, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpshrdwV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test64_VpshrdwV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, RoundingControl rc, bool z, bool sae, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpshrdwV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F3CD0B 72 D3 A5", 7, Code.EVEX_Vpshrdw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E38D8B 72 D3 A5", 7, Code.EVEX_Vpshrdw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 13CD03 72 D3 A5", 7, Code.EVEX_Vpshrdw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD0B 72 D3 A5", 7, Code.EVEX_Vpshrdw_xmm_k1z_xmm_xmmm128_imm8, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 72 D3 A5", 7, Code.EVEX_Vpshrdw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E38DAB 72 D3 A5", 7, Code.EVEX_Vpshrdw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 13CD23 72 D3 A5", 7, Code.EVEX_Vpshrdw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD2B 72 D3 A5", 7, Code.EVEX_Vpshrdw_ymm_k1z_ymm_ymmm256_imm8, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 72 D3 A5", 7, Code.EVEX_Vpshrdw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 E38DCB 72 D3 A5", 7, Code.EVEX_Vpshrdw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, RoundingControl.None, true, false, 0xA5 };
				yield return new object[] { "62 13CD43 72 D3 A5", 7, Code.EVEX_Vpshrdw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, RoundingControl.None, false, false, 0xA5 };
				yield return new object[] { "62 B3CD4B 72 D3 A5", 7, Code.EVEX_Vpshrdw_zmm_k1z_zmm_zmmm512_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, RoundingControl.None, false, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpshrddV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test16_VpshrddV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpshrddV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 73 50 01 A5", 8, Code.EVEX_Vpshrdd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D9D 73 50 01 A5", 8, Code.EVEX_Vpshrdd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 73 50 01 A5", 8, Code.EVEX_Vpshrdd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 73 50 01 A5", 8, Code.EVEX_Vpshrdd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 73 50 01 A5", 8, Code.EVEX_Vpshrdd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 73 50 01 A5", 8, Code.EVEX_Vpshrdd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 73 50 01 A5", 8, Code.EVEX_Vpshrdd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 73 50 01 A5", 8, Code.EVEX_Vpshrdd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 73 50 01 A5", 8, Code.EVEX_Vpshrdd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 73 50 01 A5", 8, Code.EVEX_Vpshrdq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D 73 50 01 A5", 8, Code.EVEX_Vpshrdq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 73 50 01 A5", 8, Code.EVEX_Vpshrdq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 73 50 01 A5", 8, Code.EVEX_Vpshrdq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 73 50 01 A5", 8, Code.EVEX_Vpshrdq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 73 50 01 A5", 8, Code.EVEX_Vpshrdq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 73 50 01 A5", 8, Code.EVEX_Vpshrdq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 73 50 01 A5", 8, Code.EVEX_Vpshrdq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 73 50 01 A5", 8, Code.EVEX_Vpshrdq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test16_VpshrddV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test16_VpshrddV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test16_VpshrddV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 73 D3 A5", 7, Code.EVEX_Vpshrdd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34D8B 73 D3 A5", 7, Code.EVEX_Vpshrdd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D2B 73 D3 A5", 7, Code.EVEX_Vpshrdd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DAB 73 D3 A5", 7, Code.EVEX_Vpshrdd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D4B 73 D3 A5", 7, Code.EVEX_Vpshrdd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DCB 73 D3 A5", 7, Code.EVEX_Vpshrdd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 73 D3 A5", 7, Code.EVEX_Vpshrdq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD0B 73 D3 A5", 7, Code.EVEX_Vpshrdq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDAB 73 D3 A5", 7, Code.EVEX_Vpshrdq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD2B 73 D3 A5", 7, Code.EVEX_Vpshrdq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB 73 D3 A5", 7, Code.EVEX_Vpshrdq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD4B 73 D3 A5", 7, Code.EVEX_Vpshrdq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpshrddV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test32_VpshrddV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpshrddV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 73 50 01 A5", 8, Code.EVEX_Vpshrdd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D9D 73 50 01 A5", 8, Code.EVEX_Vpshrdd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 73 50 01 A5", 8, Code.EVEX_Vpshrdd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 73 50 01 A5", 8, Code.EVEX_Vpshrdd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 73 50 01 A5", 8, Code.EVEX_Vpshrdd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 73 50 01 A5", 8, Code.EVEX_Vpshrdd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 73 50 01 A5", 8, Code.EVEX_Vpshrdd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 73 50 01 A5", 8, Code.EVEX_Vpshrdd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 73 50 01 A5", 8, Code.EVEX_Vpshrdd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 73 50 01 A5", 8, Code.EVEX_Vpshrdq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D 73 50 01 A5", 8, Code.EVEX_Vpshrdq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 73 50 01 A5", 8, Code.EVEX_Vpshrdq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 73 50 01 A5", 8, Code.EVEX_Vpshrdq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 73 50 01 A5", 8, Code.EVEX_Vpshrdq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 73 50 01 A5", 8, Code.EVEX_Vpshrdq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 73 50 01 A5", 8, Code.EVEX_Vpshrdq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 73 50 01 A5", 8, Code.EVEX_Vpshrdq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 73 50 01 A5", 8, Code.EVEX_Vpshrdq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test32_VpshrddV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test32_VpshrddV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test32_VpshrddV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 73 D3 A5", 7, Code.EVEX_Vpshrdd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34D8B 73 D3 A5", 7, Code.EVEX_Vpshrdd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D2B 73 D3 A5", 7, Code.EVEX_Vpshrdd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DAB 73 D3 A5", 7, Code.EVEX_Vpshrdd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D4B 73 D3 A5", 7, Code.EVEX_Vpshrdd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DCB 73 D3 A5", 7, Code.EVEX_Vpshrdd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 73 D3 A5", 7, Code.EVEX_Vpshrdq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD0B 73 D3 A5", 7, Code.EVEX_Vpshrdq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDAB 73 D3 A5", 7, Code.EVEX_Vpshrdq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD2B 73 D3 A5", 7, Code.EVEX_Vpshrdq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB 73 D3 A5", 7, Code.EVEX_Vpshrdq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 F3CD4B 73 D3 A5", 7, Code.EVEX_Vpshrdq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpshrddV_VX_k1z_HX_WX_Ib_1_Data))]
		void Test64_VpshrddV_VX_k1z_HX_WX_Ib_1(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register kreg, MemorySize memSize, uint displ, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpshrddV_VX_k1z_HX_WX_Ib_1_Data {
			get {
				yield return new object[] { "62 F34D0B 73 50 01 A5", 8, Code.EVEX_Vpshrdd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt32, 16, false, 0xA5 };
				yield return new object[] { "62 F34D9D 73 50 01 A5", 8, Code.EVEX_Vpshrdd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D08 73 50 01 A5", 8, Code.EVEX_Vpshrdd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt32, 16, false, 0xA5 };

				yield return new object[] { "62 F34D2B 73 50 01 A5", 8, Code.EVEX_Vpshrdd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt32, 32, false, 0xA5 };
				yield return new object[] { "62 F34DBD 73 50 01 A5", 8, Code.EVEX_Vpshrdd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D28 73 50 01 A5", 8, Code.EVEX_Vpshrdd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt32, 32, false, 0xA5 };

				yield return new object[] { "62 F34D4B 73 50 01 A5", 8, Code.EVEX_Vpshrdd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt32, 64, false, 0xA5 };
				yield return new object[] { "62 F34DDD 73 50 01 A5", 8, Code.EVEX_Vpshrdd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt32, 4, true, 0xA5 };
				yield return new object[] { "62 F34D48 73 50 01 A5", 8, Code.EVEX_Vpshrdd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt32, 64, false, 0xA5 };

				yield return new object[] { "62 F3CD0B 73 50 01 A5", 8, Code.EVEX_Vpshrdq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K3, MemorySize.Packed128_UInt64, 16, false, 0xA5 };
				yield return new object[] { "62 F3CD9D 73 50 01 A5", 8, Code.EVEX_Vpshrdq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.K5, MemorySize.Broadcast128_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD08 73 50 01 A5", 8, Code.EVEX_Vpshrdq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.None, MemorySize.Packed128_UInt64, 16, false, 0xA5 };

				yield return new object[] { "62 F3CD2B 73 50 01 A5", 8, Code.EVEX_Vpshrdq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K3, MemorySize.Packed256_UInt64, 32, false, 0xA5 };
				yield return new object[] { "62 F3CDBD 73 50 01 A5", 8, Code.EVEX_Vpshrdq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.K5, MemorySize.Broadcast256_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD28 73 50 01 A5", 8, Code.EVEX_Vpshrdq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.None, MemorySize.Packed256_UInt64, 32, false, 0xA5 };

				yield return new object[] { "62 F3CD4B 73 50 01 A5", 8, Code.EVEX_Vpshrdq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K3, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
				yield return new object[] { "62 F3CDDD 73 50 01 A5", 8, Code.EVEX_Vpshrdq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.K5, MemorySize.Broadcast512_UInt64, 8, true, 0xA5 };
				yield return new object[] { "62 F3CD48 73 50 01 A5", 8, Code.EVEX_Vpshrdq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.None, MemorySize.Packed512_UInt64, 64, false, 0xA5 };
			}
		}

		[Theory]
		[MemberData(nameof(Test64_VpshrddV_VX_k1z_HX_WX_Ib_2_Data))]
		void Test64_VpshrddV_VX_k1z_HX_WX_Ib_2(string hexBytes, int byteLength, Code code, Register reg1, Register reg2, Register reg3, Register kreg, bool z, byte immediate8) {
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
		public static IEnumerable<object[]> Test64_VpshrddV_VX_k1z_HX_WX_Ib_2_Data {
			get {
				yield return new object[] { "62 F34D0B 73 D3 A5", 7, Code.EVEX_Vpshrdd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 E30D0B 73 D3 A5", 7, Code.EVEX_Vpshrdd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 134D03 73 D3 A5", 7, Code.EVEX_Vpshrdd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D0B 73 D3 A5", 7, Code.EVEX_Vpshrdd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34D8B 73 D3 A5", 7, Code.EVEX_Vpshrdd_xmm_k1z_xmm_xmmm128b32_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D2B 73 D3 A5", 7, Code.EVEX_Vpshrdd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 E30D2B 73 D3 A5", 7, Code.EVEX_Vpshrdd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 134D23 73 D3 A5", 7, Code.EVEX_Vpshrdd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D2B 73 D3 A5", 7, Code.EVEX_Vpshrdd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DAB 73 D3 A5", 7, Code.EVEX_Vpshrdd_ymm_k1z_ymm_ymmm256b32_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F34D4B 73 D3 A5", 7, Code.EVEX_Vpshrdd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 E30D4B 73 D3 A5", 7, Code.EVEX_Vpshrdd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 134D43 73 D3 A5", 7, Code.EVEX_Vpshrdd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B34D4B 73 D3 A5", 7, Code.EVEX_Vpshrdd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F34DCB 73 D3 A5", 7, Code.EVEX_Vpshrdd_zmm_k1z_zmm_zmmm512b32_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };

				yield return new object[] { "62 F3CD8B 73 D3 A5", 7, Code.EVEX_Vpshrdq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D0B 73 D3 A5", 7, Code.EVEX_Vpshrdq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM18, Register.XMM14, Register.XMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD03 73 D3 A5", 7, Code.EVEX_Vpshrdq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM10, Register.XMM22, Register.XMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD0B 73 D3 A5", 7, Code.EVEX_Vpshrdq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD0B 73 D3 A5", 7, Code.EVEX_Vpshrdq_xmm_k1z_xmm_xmmm128b64_imm8, Register.XMM2, Register.XMM6, Register.XMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDAB 73 D3 A5", 7, Code.EVEX_Vpshrdq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D2B 73 D3 A5", 7, Code.EVEX_Vpshrdq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM18, Register.YMM14, Register.YMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD23 73 D3 A5", 7, Code.EVEX_Vpshrdq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM10, Register.YMM22, Register.YMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD2B 73 D3 A5", 7, Code.EVEX_Vpshrdq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD2B 73 D3 A5", 7, Code.EVEX_Vpshrdq_ymm_k1z_ymm_ymmm256b64_imm8, Register.YMM2, Register.YMM6, Register.YMM3, Register.K3, false, 0xA5 };

				yield return new object[] { "62 F3CDCB 73 D3 A5", 7, Code.EVEX_Vpshrdq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, true, 0xA5 };
				yield return new object[] { "62 E38D4B 73 D3 A5", 7, Code.EVEX_Vpshrdq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM18, Register.ZMM14, Register.ZMM3, Register.K3, false, 0xA5 };
				yield return new object[] { "62 13CD43 73 D3 A5", 7, Code.EVEX_Vpshrdq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM10, Register.ZMM22, Register.ZMM27, Register.K3, false, 0xA5 };
				yield return new object[] { "62 B3CD4B 73 D3 A5", 7, Code.EVEX_Vpshrdq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM19, Register.K3, false, 0xA5 };
				yield return new object[] { "62 F3CD4B 73 D3 A5", 7, Code.EVEX_Vpshrdq_zmm_k1z_zmm_zmmm512b64_imm8, Register.ZMM2, Register.ZMM6, Register.ZMM3, Register.K3, false, 0xA5 };
			}
		}
	}
}
